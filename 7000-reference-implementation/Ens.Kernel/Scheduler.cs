using Ens.Kernel.Adapter;
using Ens.Kernel.Capability;
using Ens.Kernel.Domain;
using Ens.Kernel.Laws;

namespace Ens.Kernel;

// TRACE: ADR-0001 (accepted, v0.3.1) §5.3 "Scheduler — attention tahsisi (P5, ENS-3022)"
// TRACE: ENS-3022 §Model 3 — AttentionPriority(d) ∝ InfoNeed(d) × max(ConformanceDeficit(d), 0)
// TRACE: Anayasa P5 (Attention kıt kaynaktır) — kernel'in kıt kaynağı tahsis eden bileşeni.
//
// ADR-0001 §5.3'ün tam iddiası: "Kernel scheduler'ı keyfi bir kuyruk DEĞİLDİR; Decision
// Gravity'nin operasyonel biçimidir... AIOS'ta bu boş bırakılır; ENS onu VOI'ye (Howard 1966)
// bağlar." Bu dosya o iddianın çalışan kanıtıdır: sıralama ölçütü keyfi bir "priority" int'i
// değil, ENS-3022'nin formülünden TÜREYEN bir niceliktir.
//
// Scheduler üç mevcut parçayı birleştirir (yeni teori icat etmez — §5.3'ün kendi tarifi):
//   1. DecisionGravity.AttentionPriority (ENS-3022) → sıralama ölçütü
//   2. LlmTierSelector (ADR-0001 §7/§5.3)          → "hesaplama kaynağı" tahsisi (model tier)
//   3. BoundedAutonomyGate (ADR-0001 §5.6, P7)     → "insan dikkati" tahsisi (gate kararı)
// §5.3: "yüksek stake + yüksek belirsizlik + az-context'li kararlar HEM insan dikkatinin
// (bounded-autonomy gate) HEM hesaplama kaynağının (model tier seçimi) önüne çıkar."
//
// ============================ DÜRÜST SINIRLAR (Faz-4) ============================
// (a) BU BİR ÇALIŞTIRICI DEĞİL, SIRALAYICI. Gerçek bir iş kuyruğu, thread-pool, async
//     dispatch, preemption, starvation-önleme YOK. `Schedule` saf bir fonksiyondur:
//     bekleyen çağrıları alır, öncelik sırasına dizip her birine tier+gate kararı iliştirir.
//     Yürütme (kimin ne zaman koşacağı) çağıran katmanın işidir.
// (b) ConformanceDeficit DIŞARIDAN VERİLİR — ENS-3022 onu Company Memory'den (peer-context
//     fiti) türetmeyi ister; `CompanyMemory` kodlu ama peer-context fiti henüz YOK. 0 verilirse
//     AttentionPriority 0'a düşer (yanlış-negatif riski) — DecisionGravity'de zaten işaretli.
// (c) Eşikler (tier + gate) KALİBRE DEĞİL — ENS-2003 γ'sı, ReflectiveDoubleLoop eşikleri ve
//     LlmTierSelector eşikleriyle aynı Faz-4 kalibrasyon borcu.
// (d) Starvation: düşük-öncelikli bir karar sonsuza dek beklenebilir (P5 "kıt kaynak" gereği
//     kabul edilen bir sonuç ama fairness/yaşlandırma mekanizması YOK — açık tasarım borcu).
// (e) AÇIK KALAN (AUDIT §5.6/B6, kapatılmadı): AttentionPriority VE InfoNeed tam olarak eşitse
//     sıralama girdi sırasına bağlıdır (`OrderByDescending` kararlıdır ama girdi permütasyonuna
//     duyarlı). İkinci bir tie-breaker (ör. DecisionId ordinal) eklemek determinizmi girdi
//     sırasından da bağımsız kılardı; ENS-3022 böyle bir ölçüt vermediği için UYDURULMADI.
// ================================================================================
//
// DÜZELTME (AUDIT §5.1): `stake = NaN` veren bir karar eskiden üçlü fail-open üretiyordu —
// gate Autonomous, tier Operational (en ucuz model), sıralamada EN SON (NaN,
// `OrderByDescending`'de en küçük sayılır → dikkat bütçesi asla ulaşmaz). Artık ölçülemeyen
// girdi `DecisionGravity`/`BoundedAutonomyGate` tarafından REDDEDİLİR: parti sessizce
// sıralanmaz, açık hata verir (fail-closed; "sessizce çözmez" registry felsefesiyle tutarlı).

/// <summary>
/// Scheduler'a giren bekleyen çağrı (pending Decision). ENS-3022'nin girdilerini taşır.
/// </summary>
/// <param name="ToolAuthorization">
/// ADR-0001 §6.1(2): bu karar bir Capability aracına bağlıysa, `CapabilityRegistry.Authorize`
/// çıktısı Gate'e DOĞRUDAN aktarılır (AUDIT §5.5'in kapanışı — registry↔gate bağı artık kodda).
/// null = araç-bağımsız karar.
/// </param>
public sealed record PendingDecision(
    Identity DecisionId,
    double Stake,
    double? Confidence,
    double ConformanceDeficit = 0.0,
    bool IsIrreversible = false,
    ToolAuthorization? ToolAuthorization = null);

/// <summary>
/// Scheduler'ın çıktısı: sıralanmış karar + iki kaynak tahsisi (hesaplama tier'ı, insan-dikkati
/// gate kararı). ADR-0001 §5.3'ün "hem insan dikkati hem hesaplama kaynağı" ifadesinin karşılığı.
/// </summary>
public sealed record ScheduledDecision(
    PendingDecision Decision,
    double AttentionPriority,
    LlmTier Tier,
    GateResult Gate);

/// <summary>
/// Cognitive Kernel Scheduler (ADR-0001 §5.3). Bekleyen kararları Decision Gravity'ye göre
/// sıralar ve her birine model-tier + bounded-autonomy kararı iliştirir. Keyfi kuyruk değil:
/// ölçüt ENS-3022'den türer.
/// </summary>
public static class Scheduler
{
    /// <summary>
    /// Bekleyen kararları AttentionPriority'ye göre azalan sırada dizer; her biri için tier
    /// (hesaplama kaynağı) ve gate (insan dikkati) kararını hesaplar.
    /// Eşitlikte InfoNeed'i yüksek olan öne alınır (deterministik sıralama — aynı girdi,
    /// aynı çıktı; test edilebilirlik için zorunlu).
    /// </summary>
    public static IReadOnlyList<ScheduledDecision> Schedule(
        IEnumerable<PendingDecision> pending,
        double autonomyThreshold,
        double blockThreshold,
        double complexThreshold = 10.0,
        double criticalThreshold = 40.0)
    {
        ArgumentNullException.ThrowIfNull(pending);

        // AUDIT-WAVE2 §2 (W11) DÜZELTMESİ — TIER POLİTİKASI PARTİDEN ÖNCE DOĞRULANIR.
        // Eskiden bu iki eşik `LlmTierSelector`'a ham geçiyordu ve orada NaN-kör bir karşılaştırma
        // vardı: `complexThreshold: NaN` verildiğinde HER karar (1e9 stake dâhil) sessizce
        // `LlmTier.Operational`'a — en zayıf modele — düşüyordu. Artık:
        //   (1) `LlmTierSelector` kendi girdilerini `Guard`'dan geçirir (tek kapı orada),
        //   (2) Scheduler eşikleri PARTİ BOŞ OLSA BİLE burada doğrular — bozuk bir politika,
        //       "hiç karar gelmedi" ya da "gelen kararlar erken dala düştü" diye gizlenemez.
        // Not: gate eşikleri (autonomy/block) BURADA doğrulanmaz — o, W10'un ayrı ve HÂLÂ AÇIK
        // bulgusudur (`GatePolicy` tipi talebi, AUDIT-WAVE2 §3.3). Kapsam karıştırılmıyor.
        Guard.NonNegativeFinite(complexThreshold, nameof(complexThreshold), "Complex tier eşiği");
        Guard.NonNegativeFinite(criticalThreshold, nameof(criticalThreshold), "Critical tier eşiği");
        if (criticalThreshold < complexThreshold)
            throw new ArgumentException(
                "criticalThreshold, complexThreshold'dan küçük olamaz — tutarsız tier politikası " +
                "(ölçülemeyen/tutarsız politika kaynak tahsisi kazanamaz, P5).");

        return pending
            .Select(d =>
            {
                double infoNeed = DecisionGravity.InfoNeed(d.Stake, d.Confidence);
                double priority = DecisionGravity.AttentionPriority(d.Stake, d.Confidence, d.ConformanceDeficit);
                var tier = LlmTierSelector.SelectTier(infoNeed, complexThreshold, criticalThreshold);
                var gate = BoundedAutonomyGate.Evaluate(
                    d.Stake, d.Confidence, d.ConformanceDeficit, d.IsIrreversible,
                    autonomyThreshold, blockThreshold, d.ToolAuthorization);

                return (Scheduled: new ScheduledDecision(d, priority, tier, gate), InfoNeed: infoNeed);
            })
            .OrderByDescending(x => x.Scheduled.AttentionPriority)
            .ThenByDescending(x => x.InfoNeed)
            .Select(x => x.Scheduled)
            .ToList();
    }

    /// <summary>
    /// Kıt-kaynak dilimi (P5): yalnızca ilk `attentionBudget` kararı döndürür. Kalanlar bu
    /// turda dikkat almaz — starvation riski dosya-başında dürüstçe işaretli (fairness yok).
    /// </summary>
    public static IReadOnlyList<ScheduledDecision> ScheduleTop(
        IEnumerable<PendingDecision> pending,
        int attentionBudget,
        double autonomyThreshold,
        double blockThreshold,
        double complexThreshold = 10.0,
        double criticalThreshold = 40.0)
    {
        if (attentionBudget < 0)
            throw new ArgumentOutOfRangeException(nameof(attentionBudget), "Attention bütçesi negatif olamaz.");

        return Schedule(pending, autonomyThreshold, blockThreshold, complexThreshold, criticalThreshold)
            .Take(attentionBudget)
            .ToList();
    }
}
