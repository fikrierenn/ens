using Ens.Kernel;
using Ens.Kernel.Adapter;
using Ens.Kernel.Domain;
using Ens.Kernel.Laws;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ADR-0001 §5.3 (Scheduler) — sıralama ölçütü ENS-3022'den TÜREMELİ, keyfi olmamalı.
// Bu testler §5.3'ün "AIOS'ta boş; ENS onu VOI'ye bağlar" iddiasını doğrular.

public sealed class SchedulerTests
{
    private const double AutonomyThreshold = 5_000;
    private const double BlockThreshold = 50_000;

    private static IReadOnlyList<ScheduledDecision> Run(params PendingDecision[] pending)
        => Scheduler.Schedule(pending, AutonomyThreshold, BlockThreshold);

    [Fact]
    public void Higher_attention_priority_is_scheduled_first()
    {
        var low = new PendingDecision(Identity.New(), Stake: 100, Confidence: 0.9, ConformanceDeficit: 0.1);
        var high = new PendingDecision(Identity.New(), Stake: 10_000, Confidence: 0.2, ConformanceDeficit: 0.8);

        var result = Run(low, high);

        Assert.Equal(high.DecisionId, result[0].Decision.DecisionId);
        Assert.Equal(low.DecisionId, result[1].Decision.DecisionId);
    }

    [Fact]
    public void Priority_matches_DecisionGravity_formula_exactly()
    {
        // §5.3'ün özü: ölçüt keyfi değil, ENS-3022'den türer. Kanıt: birebir aynı değer.
        var d = new PendingDecision(Identity.New(), Stake: 5_000, Confidence: 0.4, ConformanceDeficit: 0.6);

        var scheduled = Run(d)[0];

        Assert.Equal(
            DecisionGravity.AttentionPriority(5_000, 0.4, 0.6),
            scheduled.AttentionPriority,
            precision: 10);
    }

    [Fact]
    public void Zero_conformance_deficit_yields_zero_priority()
    {
        // ENS-3022: peer-açık yoksa dikkat çekmez (Company Memory bağlanmadan yanlış-negatif riski,
        // DecisionGravity'de ve Scheduler dosya-başında dürüstçe işaretli).
        var d = new PendingDecision(Identity.New(), Stake: 1_000_000, Confidence: 0.0, ConformanceDeficit: 0.0);

        Assert.Equal(0.0, Run(d)[0].AttentionPriority, precision: 10);
    }

    [Fact]
    public void High_infoneed_routes_to_critical_tier()
    {
        // §5.3: "yüksek InfoNeed → daha güçlü reasoning modeli (Critical tier)"
        var d = new PendingDecision(Identity.New(), Stake: 1_000, Confidence: 0.1, ConformanceDeficit: 0.5);

        Assert.Equal(LlmTier.Critical, Run(d)[0].Tier);
    }

    [Fact]
    public void Low_infoneed_routes_to_operational_tier()
    {
        // §5.3: "rutin, düşük-stake action → hafif model (Operational tier)"
        var d = new PendingDecision(Identity.New(), Stake: 5, Confidence: 0.95, ConformanceDeficit: 0.5);

        Assert.Equal(LlmTier.Operational, Run(d)[0].Tier);
    }

    [Fact]
    public void Irreversible_decision_gets_critical_block_regardless_of_priority()
    {
        // İki kaynak ayrı tahsis edilir: düşük InfoNeed hafif model alabilir ama geri-dönülemezlik
        // insan dikkatini (P7) yine de zorunlu kılar.
        var d = new PendingDecision(Identity.New(), Stake: 10, Confidence: 0.99,
            ConformanceDeficit: 0.1, IsIrreversible: true);

        var scheduled = Run(d)[0];

        Assert.Equal(GateDecision.CriticalBlock, scheduled.Gate.Decision);
        Assert.Equal(LlmTier.Operational, scheduled.Tier); // hesaplama ucuz, insan dikkati pahalı
    }

    [Fact]
    public void Gate_result_matches_BoundedAutonomyGate_exactly()
    {
        var d = new PendingDecision(Identity.New(), Stake: 100_000, Confidence: 0.5, ConformanceDeficit: 0.3);

        var scheduled = Run(d)[0];
        var expected = BoundedAutonomyGate.Evaluate(100_000, 0.5, 0.3, false, AutonomyThreshold, BlockThreshold);

        Assert.Equal(expected.Decision, scheduled.Gate.Decision);
        Assert.Equal(expected.InfoNeed, scheduled.Gate.InfoNeed, precision: 10);
    }

    [Fact]
    public void Ordering_is_deterministic_for_equal_priority()
    {
        // Eşit AttentionPriority'de InfoNeed tie-breaker — aynı girdi, aynı çıktı (test edilebilirlik).
        // Aynı priority (100×0.5=50 vs 50×1.0=50) ama farklı InfoNeed (50 vs 50 değil: 100 vs 50).
        var a = new PendingDecision(Identity.New(), Stake: 100, Confidence: 0.0, ConformanceDeficit: 0.5); // InfoNeed 100, prio 50
        var b = new PendingDecision(Identity.New(), Stake: 50, Confidence: 0.0, ConformanceDeficit: 1.0);  // InfoNeed 50,  prio 50

        var first = Scheduler.Schedule([a, b], AutonomyThreshold, BlockThreshold);
        var second = Scheduler.Schedule([b, a], AutonomyThreshold, BlockThreshold);

        Assert.Equal(a.DecisionId, first[0].Decision.DecisionId);
        Assert.Equal(a.DecisionId, second[0].Decision.DecisionId); // girdi sırası sonucu değiştirmez
    }

    [Fact]
    public void Empty_input_yields_empty_schedule()
    {
        Assert.Empty(Scheduler.Schedule([], AutonomyThreshold, BlockThreshold));
    }

    [Fact]
    public void ScheduleTop_respects_attention_budget()
    {
        // P5: kıt kaynak — yalnızca bütçe kadarı dikkat alır.
        var decisions = Enumerable.Range(1, 10)
            .Select(i => new PendingDecision(Identity.New(), Stake: i * 1000, Confidence: 0.5, ConformanceDeficit: 0.5))
            .ToArray();

        var top3 = Scheduler.ScheduleTop(decisions, 3, AutonomyThreshold, BlockThreshold);

        Assert.Equal(3, top3.Count);
        // En yüksek stake'liler önde olmalı
        Assert.Equal(10_000, top3[0].Decision.Stake);
        Assert.Equal(9_000, top3[1].Decision.Stake);
    }

    [Fact]
    public void ScheduleTop_zero_budget_yields_nothing()
    {
        var d = new PendingDecision(Identity.New(), Stake: 1000, Confidence: 0.5, ConformanceDeficit: 0.5);
        Assert.Empty(Scheduler.ScheduleTop([d], 0, AutonomyThreshold, BlockThreshold));
    }

    [Fact]
    public void ScheduleTop_negative_budget_throws()
    {
        var d = new PendingDecision(Identity.New(), Stake: 1000, Confidence: 0.5, ConformanceDeficit: 0.5);
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Scheduler.ScheduleTop([d], -1, AutonomyThreshold, BlockThreshold));
    }

    [Fact]
    public void Null_input_throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => Scheduler.Schedule(null!, AutonomyThreshold, BlockThreshold));
    }

    [Fact]
    public void Inconsistent_gate_thresholds_propagate_as_error()
    {
        // Scheduler gate'i yeniden uygulamaz — hatayı da yutmaz.
        var d = new PendingDecision(Identity.New(), Stake: 1000, Confidence: 0.5, ConformanceDeficit: 0.5);
        Assert.Throws<ArgumentException>(
            () => Scheduler.Schedule([d], autonomyThreshold: 50_000, blockThreshold: 5_000));
    }
}
