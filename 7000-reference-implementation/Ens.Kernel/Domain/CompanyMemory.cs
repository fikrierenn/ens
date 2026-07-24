using Ens.Kernel.Laws;

namespace Ens.Kernel.Domain;

// TRACE: ENS-2003 (Company Memory) §1 (Memory Graph düğümleri = commit-edilmiş kararlar)
// TRACE: ENS-2003 §3 (retention ∝ |Learning|, karşı-survivorship — başarısızlık en çok hatırlanır)
// TRACE: ENS-2003 §3 (sönümle, silme — LAW-ORG-MEMORY ile P5 arasındaki gerilim çözümü)
//
// Faz-4 sadeleştirmesi (dürüstçe işaretli): §3 "sönümle (decay)" der ama fonksiyonel biçim
// vermez — burada basit üstel decay seçildi, ENS-2003'ün kendi taahhüdü değil. Purpose-tipi
// (§Model 2) burada string — gerçek Enterprise Ontology (ENS-4020) sınıflandırmasına bağlı
// değil, dışarıdan verilir. RetentionPriority, DecisionCapital.Value ile AYNI formülü
// kasıtlı olarak yeniden kullanır (ENS-2003 §Laws: "Decision Capital... Memory Graph üzerinde
// tanımlanacak" — bu, o bağın ilk kodudur).
public sealed record MemoryRecord(
    Identity DecisionId,
    string PurposeType,
    double LearningMagnitude,
    double AttributionConfidence,
    DateTimeOffset RecordedAt)
{
    /// <summary>Retention önceliği ∝ |Learning| (§3) — outcome'un pozitifliği değil.</summary>
    public double RetentionPriority => DecisionCapital.Value(LearningMagnitude, AttributionConfidence);
}

public sealed class CompanyMemory
{
    private readonly List<MemoryRecord> _records = [];

    /// <summary>Kayıt eklenir, asla silinmez (§3, audit — EC-001 ile tutarlı).</summary>
    public void Record(MemoryRecord record) => _records.Add(record);

    public IReadOnlyList<MemoryRecord> AllRecords => _records;

    /// <summary>
    /// Retrieval: benzer Purpose-tipi + salience sıralaması (§Model 2, §3). Zayıf/sönmüş
    /// kayıtlar hâlâ dönebilir (silinmez) — yalnızca sırası düşer.
    /// </summary>
    public IReadOnlyList<MemoryRecord> Retrieve(string purposeType, DateTimeOffset asOf, double decayRatePerDay = 0.01)
    {
        return _records
            .Where(r => r.PurposeType == purposeType)
            .OrderByDescending(r => Salience(r, asOf, decayRatePerDay))
            .ToList();
    }

    /// <summary>salience = retentionPriority × exp(−decayRate × ageInDays) — sönümle, silme.</summary>
    public static double Salience(MemoryRecord record, DateTimeOffset asOf, double decayRatePerDay)
    {
        if (decayRatePerDay < 0)
            throw new ArgumentOutOfRangeException(nameof(decayRatePerDay), "Decay rate negatif olamaz.");

        double ageDays = Math.Max(0, (asOf - record.RecordedAt).TotalDays);
        return record.RetentionPriority * Math.Exp(-decayRatePerDay * ageDays);
    }
}
