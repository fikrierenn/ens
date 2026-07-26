using Ens.Kernel.Laws;

namespace Ens.Kernel.Adapter;

// TRACE: ADR-0001 (accepted, v0.3.1) §7 "Model-agnostik LLM Adapter" — kernel hiçbir modele
// bağlı değildir (Anayasa: "mimariyi tek bir AI modeline kilitleme"). Bu, §5.1 eşleme
// tablosundaki "device driver" primitifidir: kernel kararı verir, adapter donanımı (modeli) sürer.
// TRACE: ADR-0001 §7 — CrewOps `IExecutionWorker` / `CanHandle(RoleProfile)` deseninin ENS
// soyutlaması. Yeni model = yeni Adapter implementasyonu; kernel/capability koduna dokunulmaz.
// TRACE: ADR-0001 §5.3 (Scheduler) — "Model tier seçimi de buradan türer: yüksek InfoNeed →
// daha güçlü reasoning modeli (Critical tier); rutin/düşük-stake → hafif model (Operational)."
// Tier seçimi bu dosyada Decision Gravity'ye (ENS-3022) bağlanır — AIOS'ta scheduler boştur;
// ENS onu VOI'ye bağlar. Burası o bağın somut kodudur.
//
// ============================ NE OLDUĞU / NE OLMADIĞI (dürüst sınır) ============================
// BU BİR PORT'TUR — gerçek bir sağlayıcıya BAĞLI DEĞİLDİR. `ILlmAdapter` sözleşmesi + normalize
// istek/yanıt zarfları + tier-seçim + registry vardır; hiçbir HTTP/API/model çağrısı YOKTUR.
// Somut adapter (Cerebras/DeepInfra/yerel CLI ...) bu port'un arkasına AYRI bir implementasyon
// olarak takılır — o adım (ağ, API anahtarı, retry, streaming) bu artımın kapsamı DEĞİLDİR.
// Test tarafındaki EchoLlmAdapter yalnızca port'u sınayan bir test-double'dır, sağlayıcı değil.
//
// 2026-07 sağlayıcı ortamı (web-doğrulandı, uydurma yok): Cerebras OpenAI-uyumlu chat-completions
// endpoint'i sunar (ücretsiz kademe ~1M token/gün, Llama 3.3 70B / Qwen3 32B/235B / gpt-oss-120B),
// stateless-hızlı doğası ENS'in "Memory kernel'de, inference provider'da değil" ilkesiyle uyumlu;
// DeepInfra daha geniş katalog (DeepSeek V4, GLM-5, Qwen3.5) taşır. Bunların HEPSİ bu port'un
// arkasına takılabilir — kernel kodu değişmez. (§7 model listesi bu turda doğrulandı.)
//
// P6 (proof-trace): Adapter, action ATOMUNUN ALTINDAKİ device-driver katmanıdır; proof-trace
// (ENS-4025 L8) atom düzeyinde zorunludur (ADR-0001 §5.4), driver düzeyinde DEĞİL. Bu port
// proof-trace ÜRETMEZ — üreten ayrı bir bileşendir (§5.5 Proof-Trace Emitter, HENÜZ kodlanmadı).
// Ama `LlmResponse` zarfı ModelId + token-kullanımını taşır ki üstteki atomun proof-trace'i
// "hangi model, ne üretti" bilgisini kaydedebilsin (audit substratı).
// P7 (bounded autonomy): Bounded-Autonomy Gate (BoundedAutonomyGate, kodlu) bu adapter'ın
// ÜSTÜNDEDİR; adapter yalnızca gate geçtikten sonra çağrılır. Port gate'i yeniden uygulamaz.
//
// DÜRÜSTÇE İŞARETLİ FAZ-4 SADELEŞTİRMESİ: tier eşikleri (complexThreshold/criticalThreshold)
// KALİBRE EDİLMEDİ — ENS-2003 γ'sı ve ReflectiveDoubleLoop eşikleriyle aynı borç. Varsayılan
// değerler (10/40) yalnızca yapısal örnektir; hangi InfoNeed'in hangi tier'ı gerektirdiği açık
// tasarım borcudur (ENS-3022/ENS-2004 kalibrasyonuna bağlı).
// ==============================================================================================
//
// ============================ DENETİM DALGA-2 DÜZELTMESİ (W11) ============================
// AUDIT-WAVE2-SCHEDULER.md §2 (`AUDIT_DEFECT_W11`): AUDIT §5.1'in "NaN fail-open" kök nedeni
// SEKİZİNCİ bir noktada daha duruyordu ve `Guard.cs`'in "yedi nokta kapatıldı" listesi bunu
// ATLAMIŞTI: `LlmTierSelector` Guard'ı hiç çağırmıyordu ve aynı NaN-kör deseni tekrarlıyordu:
//     if (criticalThreshold < complexThreshold) throw ...;   // NaN < NaN → false, GÖRMEZ
//     if (infoNeed >= criticalThreshold) return Critical;     // NaN >= NaN → false
//     if (infoNeed >= complexThreshold)  return Complex;      // false
//     return LlmTier.Operational;                             // ← en permisif dal
// `Scheduler.Schedule(...)` bu eşikleri DOĞRUDAN geçirdiği için `complexThreshold: NaN` veren
// (ya da `0.0/0.0` üreten bir kalibrasyon fonksiyonu olan) bir çağıran, 1 milyar TL stake'li,
// sıfır-güvenli bir kararı SESSİZCE en zayıf modele yönlendirebiliyordu. Saldırgan gerekmez.
// Bu, üçlü fail-open'ın ikinci ayağıdır (P5 ihlali: kıt hesaplama kaynağı yanlış tahsis edilir).
//
// POLİTİKA (Guard.cs ile AYNI): ölçülemeyen girdi otonomi/kaynak KAZANAMAZ. Tier seçiminin
// ÜÇ `double` girdisi de (infoNeed + iki eşik) `Guard`'dan geçer; NaN/±Infinity/negatif REDDEDİLİR
// (exception), en permisif dala sessizce düşmez. Fail-CLOSED. Kanıt: `AUDIT_FIXED_W11_*`.
// ==========================================================================================

/// <summary>
/// Model gücü katmanı (§5.3). InfoNeed'e göre seçilir: yüksek belirsizlik×stake → güçlü reasoning
/// modeli; rutin/düşük-stake → hafif model. Token maliyeti attention-önceliğiyle (P5) hizalanır.
/// </summary>
public enum LlmTier
{
    /// <summary>Rutin, düşük-stake action — hafif/verimli model (ör. Gemma/Qwen küçük).</summary>
    Operational,

    /// <summary>Orta InfoNeed — dengeli model (ör. Qwen orta boy).</summary>
    Complex,

    /// <summary>Yüksek InfoNeed — güçlü reasoning modeli (ör. DeepSeek-R1/V4, GLM).</summary>
    Critical
}

/// <summary>
/// Kernel'den adapter'a normalize istek zarfı. "Nerede/nasıl" koştuğu implementasyona bırakılır
/// (ADR-0001 §7). Model kimliği/sağlayıcı BURADA yoktur — tier soyut kalır, somut model seçimi
/// adapter'ın işidir (model-agnostisizm).
/// </summary>
public sealed record LlmRequest(
    string Prompt,
    LlmTier Tier,
    string? SystemContext = null,
    int? MaxTokens = null);

/// <summary>
/// Adapter'dan kernel'e normalize yanıt zarfı (CrewOps `TaskObservation` benzeri). ModelId ve
/// token-kullanımı, üstteki atomun proof-trace'inin (ENS-4025 L8) kaydedebilmesi için taşınır.
/// </summary>
public sealed record LlmResponse(
    string Content,
    string ModelId,
    int? PromptTokens = null,
    int? CompletionTokens = null);

/// <summary>
/// Model-agnostik LLM Adapter Port (ADR-0001 §7, "device driver"). Somut sağlayıcı bu sözleşmeyi
/// implemente eder; kernel yalnızca bu port'u tanır. Yeni model = yeni implementasyon.
/// </summary>
public interface ILlmAdapter
{
    /// <summary>Adapter/sağlayıcı kimliği (proof-trace ve seçim kaydı için).</summary>
    string AdapterId { get; }

    /// <summary>Bu adapter verilen tier'ı sürebilir mi? (CrewOps `CanHandle(RoleProfile)` deseni.)</summary>
    bool CanHandle(LlmTier tier);

    /// <summary>Normalize isteği çalıştırır, normalize yanıt döndürür. "Nasıl" adapter'a aittir.</summary>
    Task<LlmResponse> CompleteAsync(LlmRequest request, CancellationToken cancellationToken = default);
}

/// <summary>
/// InfoNeed → LlmTier eşlemesi (ADR-0001 §5.3). AIOS scheduler'ının boş bıraktığı "hangi model"
/// sorusunu Decision Gravity'ye (ENS-3022) bağlar. Eşikler kalibre değil (dosya-başı, Faz-4 borcu).
/// </summary>
public static class LlmTierSelector
{
    /// <summary>
    /// Verilen InfoNeed için tier seçer. complexThreshold ≤ criticalThreshold olmalı.
    ///
    /// AUDIT-WAVE2 §2 (W11) DÜZELTMESİ: üç `double` girdinin ÜÇÜ DE `Guard`'dan geçer. NaN ya da
    /// ±Infinity bir eşik/InfoNeed artık "en permisif dal"a (Operational) sessizce düşmez;
    /// <see cref="ArgumentOutOfRangeException"/> ile REDDEDİLİR (fail-closed, P5/P7).
    /// </summary>
    public static LlmTier SelectTier(
        double infoNeed,
        double complexThreshold = 10.0,
        double criticalThreshold = 40.0)
    {
        // ÖNCE POLİTİKA (eşikler), SONRA VERİ (infoNeed): bozuk bir eşik konfigürasyonu,
        // hangi kararın geldiğinden BAĞIMSIZ olarak yakalanmalı. (AUDIT-WAVE2 §3.3'ün
        // "politika doğrulaması veriye bağlı olmamalı" itirazının tier ayağındaki karşılığı.)
        Guard.NonNegativeFinite(complexThreshold, nameof(complexThreshold), "Complex tier eşiği");
        Guard.NonNegativeFinite(criticalThreshold, nameof(criticalThreshold), "Critical tier eşiği");
        if (criticalThreshold < complexThreshold)
            throw new ArgumentException("criticalThreshold, complexThreshold'dan küçük olamaz — tutarsız eşik.");

        // `infoNeed < 0` deseni NaN'ı geçiriyordu (IEEE-754); Guard hem NaN'ı hem ±Infinity'yi
        // hem negatifi aynı kapıda reddeder — exception tipi/paramName davranışı korunur.
        Guard.NonNegativeFinite(infoNeed, nameof(infoNeed), "InfoNeed");

        if (infoNeed >= criticalThreshold) return LlmTier.Critical;
        if (infoNeed >= complexThreshold) return LlmTier.Complex;
        return LlmTier.Operational;
    }

    /// <summary>
    /// Kolaylık: Stake + Confidence'tan doğrudan tier. InfoNeed = Stake × (1−Confidence)
    /// (DecisionGravity, ENS-3022) hesaplanır, sonra tier'a çevrilir. Bu, §5.3'ün "model tier
    /// seçimi buradan türer" iddiasının tek satırlık kanıtıdır.
    ///
    /// W11: eşikler InfoNeed hesabından ÖNCE doğrulanır — bozuk politika, sağlıklı veriyle
    /// maskelenemez.
    /// </summary>
    public static LlmTier SelectTier(
        double stake,
        double? confidence,
        double complexThreshold = 10.0,
        double criticalThreshold = 40.0)
    {
        Guard.NonNegativeFinite(complexThreshold, nameof(complexThreshold), "Complex tier eşiği");
        Guard.NonNegativeFinite(criticalThreshold, nameof(criticalThreshold), "Critical tier eşiği");
        if (criticalThreshold < complexThreshold)
            throw new ArgumentException("criticalThreshold, complexThreshold'dan küçük olamaz — tutarsız eşik.");

        return SelectTier(DecisionGravity.InfoNeed(stake, confidence), complexThreshold, criticalThreshold);
    }
}

/// <summary>
/// Kayıtlı adapter'lardan verilen tier'ı sürebilen ilkini çözer (ADR-0001 §7 + §6 registry
/// felsefesi: merkezî otorite, sessizce çözmez — yoksa açıkça hata verir). Model-agnostik:
/// hangi somut adapter'ların kayıtlı olduğu kernel'in bilgisi dışındadır.
/// </summary>
public sealed class LlmAdapterRegistry
{
    private readonly IReadOnlyList<ILlmAdapter> _adapters;

    public LlmAdapterRegistry(IEnumerable<ILlmAdapter> adapters)
    {
        ArgumentNullException.ThrowIfNull(adapters);
        _adapters = adapters.ToList();
        if (_adapters.Count == 0)
            throw new ArgumentException("En az bir adapter kayıtlı olmalı — model-agnostik ama modelsiz değil.", nameof(adapters));
    }

    public IReadOnlyList<ILlmAdapter> Adapters => _adapters;

    /// <summary>
    /// Verilen tier'ı sürebilen ilk adapter'ı döndürür. Yoksa <see cref="InvalidOperationException"/>
    /// — "sessizce boş dönme" değil, açık hata (§6 registry: çakışmayı/eksiği sessizce çözmez).
    /// </summary>
    public ILlmAdapter Resolve(LlmTier tier)
    {
        foreach (var adapter in _adapters)
            if (adapter.CanHandle(tier))
                return adapter;

        throw new InvalidOperationException(
            $"'{tier}' tier'ını sürebilen kayıtlı adapter yok. (Model-agnostik port ama bu tier için sağlayıcı takılı değil.)");
    }
}
