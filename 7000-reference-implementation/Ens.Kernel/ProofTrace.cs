using System.Collections.ObjectModel;
using Ens.Kernel.Domain;

namespace Ens.Kernel;

// TRACE: ADR-0001 (accepted, v0.3.1) §5.5 "Action Proof-Trace (P6, ENS-4025 L8) — invariant"
// TRACE: ENS-4025 §L8 "Proof trace ZORUNLU (P6). Her türetilmiş olgu, onu üreten kuralı +
//        öncülleri taşır. İzsiz çıkarım = black-box = Anayasa Madde VI ihlali (yasak)."
// TRACE: ENS-4025 §L7 "Confidence propagation: conf(sonuç) = min(conf(öncüller))" — t-norm.
// TRACE: Anayasa Madde VI (anti-pattern: "black-box çıktı — açıklama nesnesi olmayan öneri"),
//        Anayasa P6 (Explainability pazarlık konusu değildir).
//
// ADR-0001 §5.5'in çekirdek iddiası: "onlarda (AIOS/CrewOps) audit-log SONRADAN EKLENEN bir
// gözlemdir; ENS'te proof-trace ACTION'IN VAR-OLMA KOŞULUDUR." Bu dosya o iddiayı yapısal
// olarak zorlar: `ProofTrace` bir öncül listesi OLMADAN kurulamaz (constructor reddeder) ve
// `Confidence` hesaplanmış bir değerdir, dışarıdan atanamaz — "izsiz türetim" temsil EDİLEMEZ.
//
// ============================ DÜRÜST SINIRLAR (Faz-4) ============================
// (a) BU DOSYA TRACE NESNESİ + BİRLEŞTİRİCİDİR. **GÜNCELLEME (2026-07-26, D-1 kapanışı):**
//     `Domain/DecisionAggregate.Commit` artık bunu GERÇEKTEN üretir — commitment atomu bir
//     `ProofTrace` doğurur (`CommitmentTrace`), Evidence boşsa commitment reddedilir ve
//     confidence L7 t-norm'unu aşamaz (hem canlı hem replay yolunda). Yani "kernel bileşenleri
//     bunu HENÜZ üretmiyor" cümlesi ATOM SINIRI için artık DOĞRU DEĞİLDİR.
//     HÂLÂ ÜRETMEYENLER (dürüst kayıt): `Scheduler` (tier/öncelik kararı), `BoundedAutonomyGate`
//     (gate kararı), `Adapter` (model seçimi) ve Decision lifecycle'ının commitment SONRASI
//     aşamaları (Enactment/Measurement/Learning). ADR-0001 §5.4 proof-trace'i atom düzeyinde
//     zorunlu kıldığı için bu kapsam meşrudur, ama §5.5'in tam hâli değildir.
// (b) Öncüller (`Premise`) burada serbest metin + confidence çiftidir. ENS-4025 L8'in tam
//     hâli, öncülün Memory/Context/Evidence NODE'una (ENS-4010) tipli referans olmasını ister —
//     o bağ henüz yok (Purpose-tipi'nin string kalması ile aynı Faz-4 borcu).
// (c) L7 t-norm olarak yalnızca `min` (ENS-4025 varsayılanı) kodlandı. ENS-4025 §Failure
//     "t-norm seçimi (min vs product vs Bayesian) ileride bir RFC gerektirebilir" diyor —
//     alternatif t-norm'lar bilinçli olarak kodlanmadı.
// (d) Proof-trace'in KENDİSİ event-sourced değil (ayrı bir AuditEvent akışına yazılmıyor);
//     §5.4'ün action lifecycle state-machine'i (Planned→Gated→Acting→Traced) kodlanmadı.
// ================================================================================
//
// ============================ DENETİM SONRASI İKİ DÜZELTME (AUDIT.md) ============================
// (1) §5.2 — `Premises` bir `IReadOnlyList<Premise>` olarak sunuluyordu ama arkasındaki nesne
//     canlı bir `List<Premise>`'ti: `((List<Premise>)trace.Premises).Clear()` ile öncüller
//     KURULDUKTAN SONRA boşaltılabiliyor, "öncülsüz trace kurulamaz" invariant'ı tek satırda
//     delinip `Confidence` eski değerinde donmuş kalıyordu. Artık `ReadOnlyCollection` döner —
//     downcast `InvalidCastException` atar.
// (2) §5.1 — `Premise`'in `confidence is < 0 or > 1` guard'ı NaN'ı geçiriyordu (IEEE-754: NaN
//     her karşılaştırmada false). NaN bir öncül, L7 min t-norm'unu TANIMSIZ hâle getiriyordu.
//     Artık `Guard.UnitInterval` üzerinden reddedilir.
// ==============================================================================================

/// <summary>
/// Bir türetimin öncülü (ENS-4025 L8): neyin, hangi güvenle bu sonuca katkı verdiği.
/// `Source` şimdilik serbest metin — tipli node referansı Faz-4 borcu (dosya-başı).
/// </summary>
public sealed record Premise
{
    public string Source { get; }
    public double Confidence { get; }

    public Premise(string source, double confidence)
    {
        if (string.IsNullOrWhiteSpace(source))
            throw new ArgumentException("Öncül kaynağı boş olamaz — izsiz öncül, izsiz türetimdir (L8).", nameof(source));

        // AUDIT §5.1: NaN artık geçemez — ölçülemeyen bir öncül, L7 min t-norm'unu tanımsız yapar.
        Guard.UnitInterval(confidence, nameof(confidence), "Öncül confidence'ı");

        Source = source;
        Confidence = confidence;
    }
}

/// <summary>
/// ENS-4025 L8 proof-trace zarfı: hangi kural, hangi öncüllerden, hangi confidence ile ne türetti.
/// **Öncülsüz kurulamaz** — bu, "izsiz çıkarım yasak" invariant'ının yapısal zorlanışıdır
/// (Anayasa Madde VI). `Confidence` L7'nin `min` t-norm'uyla HESAPLANIR, atanamaz.
/// </summary>
public sealed record ProofTrace
{
    /// <summary>Türetimi üreten kural/plan kimliği (ör. "IR-001", "BAG-P3", "Scheduler/ENS-3022").</summary>
    public string RuleId { get; }

    /// <summary>Türetilen olgu/action (ENS-4025 örneğindeki `⊢` sağ tarafı).</summary>
    public string Conclusion { get; }

    /// <summary>
    /// Öncüller — en az bir tane olmak ZORUNDA (L8). Savunmacı kopya + `ReadOnlyCollection`:
    /// ne girdi listesinin sonraki mutasyonu ne de çıktının downcast'i bu listeyi değiştiremez
    /// (AUDIT §5.2).
    /// </summary>
    public IReadOnlyList<Premise> Premises { get; }

    /// <summary>ENS-4025 L7: conf(sonuç) = min(conf(öncüller)). Muhafazakâr t-norm, atanamaz.</summary>
    public double Confidence { get; }

    public ProofTrace(string ruleId, string conclusion, IReadOnlyList<Premise> premises)
    {
        if (string.IsNullOrWhiteSpace(ruleId))
            throw new ArgumentException("Kural kimliği zorunlu — hangi kuralın türettiği bilinmeyen olgu izsizdir (L8).", nameof(ruleId));
        if (string.IsNullOrWhiteSpace(conclusion))
            throw new ArgumentException("Sonuç boş olamaz.", nameof(conclusion));
        ArgumentNullException.ThrowIfNull(premises);

        // Önce SNAPSHOT al, sonra yalnızca snapshot üzerinden doğrula ve hesapla — çağıranın
        // listesi bu satırlar arasında değişse bile trace tutarlı kalır (TOCTOU kapalı).
        var snapshot = new List<Premise>(premises);
        if (snapshot.Count == 0)
            throw new ArgumentException(
                "Öncülsüz proof-trace kurulamaz — izsiz çıkarım = black-box = Anayasa Madde VI ihlali (ENS-4025 L8).",
                nameof(premises));
        if (snapshot.Any(p => p is null))
            throw new ArgumentException("Öncül listesi null öğe içeremez (L8).", nameof(premises));

        RuleId = ruleId;
        Conclusion = conclusion;
        Premises = new ReadOnlyCollection<Premise>(snapshot);
        Confidence = snapshot.Min(p => p.Confidence); // L7: min t-norm
    }

    /// <summary>
    /// ENS-4025 §Proof-trace biçimine yakın, insan-okunur gösterim (P6 explainability).
    /// Örnek: `⇒ [IR-001] ⊢ Decision-42 indirectly_supported_by Capability-7 (conf = 0.70)`
    /// </summary>
    public string Render()
    {
        var premiseList = string.Join(", ", Premises.Select(p => $"{p.Source}={p.Confidence:F2}"));
        return $"{premiseList}\n   ⇒ [{RuleId}]\n   ⊢ {Conclusion}   (conf = min({premiseList}) = {Confidence:F2})";
    }

    /// <summary>
    /// Zincirleme türetim (ENS-4025 L8 + L7): bu trace'i bir sonraki türetimin öncülü yapar.
    /// Confidence monoton azalır (min t-norm) — türetim zinciri uzadıkça güven düşer, artamaz.
    /// </summary>
    public Premise AsPremise() => new(Conclusion, Confidence);
}
