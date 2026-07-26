using Ens.Kernel.Adapter;
using Ens.Kernel.Laws;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ADR-0001 §7 (Model-agnostik LLM Adapter Port) + §5.3 (tier ← InfoNeed).
// Bu testler PORT'u sınar — gerçek bir sağlayıcıyı DEĞİL. EchoLlmAdapter bir test-double'dır:
// istenen prompt'u yankılar, sağlayıcı taklit etmez. Amaç: (1) tier-seçimin Decision Gravity'ye
// (ENS-3022) doğru bağlandığını, (2) registry'nin CanHandle deseniyle doğru adapter'ı çözdüğünü,
// (3) normalize istek/yanıt zarfının model-agnostik aktığını kanıtlamak.

/// <summary>
/// Test-double: verilen tier'ları sürebilen, prompt'u yankılayan sahte adapter. Sağlayıcı DEĞİL —
/// hiçbir ağ/API çağrısı yok. Port sözleşmesinin (ILlmAdapter) çalıştığını göstermek için.
/// </summary>
internal sealed class EchoLlmAdapter(string adapterId, params LlmTier[] handledTiers) : ILlmAdapter
{
    private readonly HashSet<LlmTier> _tiers = [.. handledTiers];

    public string AdapterId { get; } = adapterId;

    public bool CanHandle(LlmTier tier) => _tiers.Contains(tier);

    public Task<LlmResponse> CompleteAsync(LlmRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return Task.FromResult(new LlmResponse(
            Content: $"echo:{request.Prompt}",
            ModelId: $"{AdapterId}:{request.Tier}",
            PromptTokens: request.Prompt.Length,
            CompletionTokens: request.Prompt.Length + 5));
    }
}

public sealed class LlmAdapterTests
{
    // ---- Tier seçimi: InfoNeed → LlmTier (§5.3) ----

    [Theory]
    [InlineData(0.0, LlmTier.Operational)]
    [InlineData(9.99, LlmTier.Operational)]
    [InlineData(10.0, LlmTier.Complex)]
    [InlineData(39.99, LlmTier.Complex)]
    [InlineData(40.0, LlmTier.Critical)]
    [InlineData(1000.0, LlmTier.Critical)]
    public void Tier_is_selected_by_infoneed(double infoNeed, LlmTier expected)
    {
        Assert.Equal(expected, LlmTierSelector.SelectTier(infoNeed));
    }

    [Fact]
    public void Tier_derives_from_stake_and_confidence_via_decision_gravity()
    {
        // InfoNeed = Stake × (1−Confidence) = 100 × 0.5 = 50 ≥ 40 → Critical
        var tier = LlmTierSelector.SelectTier(stake: 100, confidence: 0.5);
        Assert.Equal(LlmTier.Critical, tier);

        // Aynı stake, yüksek güven → düşük InfoNeed → hafif model
        // InfoNeed = 100 × 0.02 = 2 < 10 → Operational
        var routine = LlmTierSelector.SelectTier(stake: 100, confidence: 0.98);
        Assert.Equal(LlmTier.Operational, routine);
    }

    [Fact]
    public void Tier_selection_matches_decision_gravity_infoneed_exactly()
    {
        // §5.3 bağının kanıtı: convenience overload, InfoNeed overload'ıyla aynı sonucu vermeli.
        double infoNeed = DecisionGravity.InfoNeed(stake: 50, confidence: 0.4); // 30
        Assert.Equal(
            LlmTierSelector.SelectTier(infoNeed),
            LlmTierSelector.SelectTier(stake: 50, confidence: 0.4));
    }

    [Fact]
    public void Negative_infoneed_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => LlmTierSelector.SelectTier(-1.0));
    }

    [Fact]
    public void Inconsistent_thresholds_throw()
    {
        Assert.Throws<ArgumentException>(() =>
            LlmTierSelector.SelectTier(infoNeed: 5, complexThreshold: 40, criticalThreshold: 10));
    }

    // ---- Registry: CanHandle ile adapter çözümü (§7, CrewOps deseni) ----

    [Fact]
    public void Registry_resolves_adapter_that_can_handle_tier()
    {
        var registry = new LlmAdapterRegistry(new ILlmAdapter[]
        {
            new EchoLlmAdapter("edge", LlmTier.Operational),
            new EchoLlmAdapter("reasoning", LlmTier.Complex, LlmTier.Critical)
        });

        Assert.Equal("edge", registry.Resolve(LlmTier.Operational).AdapterId);
        Assert.Equal("reasoning", registry.Resolve(LlmTier.Critical).AdapterId);
    }

    [Fact]
    public void Registry_throws_when_no_adapter_handles_tier()
    {
        var registry = new LlmAdapterRegistry([new EchoLlmAdapter("edge", LlmTier.Operational)]);

        // Sessizce boş dönmez — açık hata (§6 registry felsefesi).
        Assert.Throws<InvalidOperationException>(() => registry.Resolve(LlmTier.Critical));
    }

    [Fact]
    public void Empty_registry_throws()
    {
        Assert.Throws<ArgumentException>(() => new LlmAdapterRegistry([]));
    }

    // ---- Uçtan uca: tier seç → adapter çöz → normalize zarf ak (model-agnostik) ----

    [Fact]
    public async Task End_to_end_high_infoneed_routes_to_critical_adapter_and_returns_normalized_envelope()
    {
        var registry = new LlmAdapterRegistry(new ILlmAdapter[]
        {
            new EchoLlmAdapter("gemma-edge", LlmTier.Operational),
            new EchoLlmAdapter("deepseek-r1", LlmTier.Complex, LlmTier.Critical)
        });

        // Yüksek stake + düşük güven → yüksek InfoNeed → Critical tier → güçlü reasoning adapter
        var tier = LlmTierSelector.SelectTier(stake: 200, confidence: 0.3); // InfoNeed = 140 → Critical
        var adapter = registry.Resolve(tier);
        var response = await adapter.CompleteAsync(new LlmRequest("planla: X", tier));

        Assert.Equal(LlmTier.Critical, tier);
        Assert.Equal("deepseek-r1", adapter.AdapterId);
        Assert.Equal("echo:planla: X", response.Content);
        // ModelId proof-trace substratı için taşınır (üstteki atom "hangi model" kaydeder).
        Assert.Equal("deepseek-r1:Critical", response.ModelId);
        Assert.NotNull(response.PromptTokens);
    }

    [Fact]
    public async Task Same_kernel_call_works_with_a_different_adapter_swapped_in()
    {
        // Model-agnostisizm kanıtı (§7): kernel/istek kodu değişmez, yalnızca adapter değişir.
        var request = new LlmRequest("özetle: rapor", LlmTier.Operational);

        var registryA = new LlmAdapterRegistry([new EchoLlmAdapter("qwen-small", LlmTier.Operational)]);
        var registryB = new LlmAdapterRegistry([new EchoLlmAdapter("gemma", LlmTier.Operational)]);

        var respA = await registryA.Resolve(request.Tier).CompleteAsync(request);
        var respB = await registryB.Resolve(request.Tier).CompleteAsync(request);

        // Aynı istek, farklı model — içerik aynı normalize akışta, yalnızca ModelId farklı.
        Assert.Equal(respA.Content, respB.Content);
        Assert.NotEqual(respA.ModelId, respB.ModelId);
    }
}
