namespace Ens.Kernel.Domain;

// TRACE: ENS-2004 (Learning Theory) v0.3.2 §4a "Reflective double-loop" (ratified, SKR-044
// survives). Akış: (geçmiş kararların) trace'i oku → sistematik öngörü hatasının *neden*'ini
// analiz et → Assumptions/relevance-model/attribution-seviyesi için hedefli iyileştirme
// ÖNERİSİ üret → öneri ASLA otomatik uygulanmaz, insan onayı (P7, Anayasa Madde III) gerekir.
//
// TRACE: ENS-4025 L8 (zorunlu proof-trace) — KISMEN. §4a'nın substratı tam proof-trace'tir
// (kuralı + öncülleri taşıyan türetim zinciri: Assumption→Expected eşlemesi). Faz-4'te o
// altyapı henüz karar-öngörüsü türetimine bağlanmadı (ENS-2004 §Failure "operasyonelleşmedi",
// E1). Bu implementasyon proof-trace yerine yalnızca CompanyMemory kayıtlarını (MemoryRecord)
// okur — reflektif "neden" analizinin ZAYIF bir yaklaşığıdır, tam trace okuması değildir.
//
// Prior art (ENS-2004 §Prior art, hepsi gerçek kaynak — SKR-001 dürüstlük dersi):
//   - GEPA (arXiv:2507.19457, "Reflective Prompt Evolution Can Outperform RL"): execution-
//     trace'ten "neden başarısız" reflektif analizi + hedefli aday üretimi. §4a'nın "neden"
//     analizinin kaynağı — ama ENS prompt değil Assumptions/relevance-model günceller, ve
//     çıktı bir öneridir.
//   - DSPy (arXiv:2310.03714): optimize-et-değerlendir döngüsü.
//   - Hermes Agent self-evolution (NousResearch): constraint-gate + insan-PR-onayı, otonom
//     commit YOK → ENS'te P7'nin doğrudan eşi.
//
// ============================ P7 KAPISI YAPISAL OLARAK ZORLANIR ============================
// Bu sınıfın state değiştiren HİÇBİR metodu yoktur — ne Apply, ne Commit, ne Update, ne Set.
// Yalnızca `Propose(...)` vardır ve salt-okunur bir öneri listesi döndürür. P7 (öneri asla
// otomatik uygulanmaz) bu tasarımda bir runtime-kontrol değil, TİPİN ŞEKLİDİR: mekanizmanın
// öneriyi uygulaması imkânsızdır çünkü uygulayacak metodu yoktur. Bunu bir refleksiyon-testi
// (ReflectiveDoubleLoopTests) yapısal olarak doğrular. Reflective double-loop bir ÖNERİ
// üreticisidir, bir mutasyon motoru değildir (ENS-2004 §4a "P7 kapısı").
// ==========================================================================================
//
// DÜRÜSTÇE İŞARETLİ FAZ-4 SADELEŞTİRMELERİ (SKR-001/SKR-040 disiplini — dar ve dürüst delta):
//   (a) YÖN-SAPMASI DEĞİL, BÜYÜKLÜK-TEKRARI. ENS-2004 §1 gerçek sinyal
//       learning_signal = Actual − Expected (İŞARETLİ, yönü var). Ama MemoryRecord.
//       LearningMagnitude İŞARETSİZ (|Learning|) ve DecisionEvents.cs'teki OutcomeObserved/
//       LearningRecorded sonuçları `string` tutuyor (sayısal değil). Bu yüzden bu Faz-4
//       versiyonu gerçek "sistematik yön-sapması"nı (öngörülerin hep aynı yöne kaçması)
//       tespit EDEMEZ — yalnızca "aynı Purpose-tipinde tekrarlayan büyük-büyüklükte öğrenim
//       sinyali" tespit eder. Bu, §4a'nın tam iddiasının bir alt-kümesidir, tümü değil.
//   (b) EŞİKLER KALİBRE EDİLMEDİ. minSupportingRecords ve magnitudeThreshold ampirik değil,
//       tıpkı ENS-2003'ün γ'sı gibi (aynı kalibrasyon borcu, ENS-2003 §Failure). Varsayılan
//       değerler (3, 5.0) yalnızca yapısal örnektir; hangi eşiğin "sistematik"i doğru
//       kestiği açık tasarım borcudur.
//   (c) ÖNERİ-YORGUNLUĞU (P5) BAĞLANMADI. ENS-2004 §Failure "öneri-yorgunluğu": hacimli
//       öneri → P7 rubber-stamp'e ya da görmezden gelmeye dejenere olur. `Propose` SINIRSIZ
//       sayıda öneri dönebilir — bir önceliklendirme (ör. Decision Gravity, ENS-3022) ya da
//       eşik/batch limiti YOKTUR. Bu bilinçli açık borçtur; P5 dikkat bütçesi henüz devrede
//       değil.

/// <summary>
/// §4a reflektif double-loop çıktısı: bir Purpose-tipi için üretilmiş, insan onayına (P7) tabi,
/// salt-okunur iyileştirme ÖNERİSİ. Kendi başına hiçbir şeyi değiştirmez; yalnızca "şu karar
/// sınıfına bakılmalı" sinyalidir (proof-trace'i taşıması ENS-4025 L8'e bağlı — Faz-4'te eksik).
/// </summary>
public sealed record ReflectiveProposal(
    string PurposeType,
    string Description,
    int SupportingRecordCount,
    double AverageAttributionConfidence);

/// <summary>
/// ENS-2004 v0.3.2 §4a reflektif double-loop'un Faz-4 öneri-üreticisi. STATE DEĞİŞTİRMEZ
/// (P7 yapısal): tek public metodu `Propose`, salt-okunur öneri listesi döndürür.
/// </summary>
public static class ReflectiveDoubleLoop
{
    /// <summary>
    /// Aynı Purpose-tipinde tekrarlayan büyük-büyüklükte öğrenim sinyali taşıyan karar
    /// sınıfları için iyileştirme ÖNERİSİ üretir (uygulamaz — P7). Bir kayıt "destekleyici"
    /// sayılır: LearningMagnitude ≥ magnitudeThreshold. Bir Purpose-tipinde destekleyici kayıt
    /// sayısı ≥ minSupportingRecords ise, o sınıf için bir öneri döner.
    ///
    /// DİKKAT (Faz-4, dosya-başı sadeleştirme (a)): bu "sistematik büyüklük-tekrarı"dır, gerçek
    /// yönlü sistematik-sapma DEĞİLDİR — LearningMagnitude işaretsiz olduğundan öngörünün hangi
    /// yöne kaçtığı burada bilinemez.
    /// </summary>
    public static IReadOnlyList<ReflectiveProposal> Propose(
        IEnumerable<MemoryRecord> records,
        int minSupportingRecords = 3,
        double magnitudeThreshold = 5.0)
    {
        ArgumentNullException.ThrowIfNull(records);

        // AUDIT §5.6: guard KOZMETİKTİ. Mesajı "tek gözlemden 'sistematik' iddia edilemez"
        // diyordu ama `minSupportingRecords: 1` KABUL ediliyordu — yani tam olarak tek
        // gözlemden "sistematik tekrar" önerisi üretilebiliyordu. Guard artık iddia ettiği
        // şeyi gerçekten engelliyor: n=1 bir ÖRNEKTİR, bir ÖRÜNTÜ değildir; "tekrar etti"
        // diyebilmek için en az iki gözlem gerekir.
        // (Not: 2 de kalibre bir eşik DEĞİL — yalnızca kavramsal alt sınır. Ampirik eşik
        //  hâlâ açık borç: dosya-başı sadeleştirme (b).)
        if (minSupportingRecords < 2)
            throw new ArgumentOutOfRangeException(nameof(minSupportingRecords), minSupportingRecords,
                "En az 2 destekleyici kayıt gerekir — tek gözlemden 'sistematik tekrar' iddia edilemez " +
                "(n=1 bir örnektir, bir örüntü değildir).");

        if (!double.IsFinite(magnitudeThreshold) || magnitudeThreshold < 0)
            throw new ArgumentOutOfRangeException(nameof(magnitudeThreshold), magnitudeThreshold,
                "Büyüklük eşiği sonlu ve negatif-olmayan olmalı (|Learning| ≥ 0).");

        return records
            .Where(r => r.LearningMagnitude >= magnitudeThreshold)
            .GroupBy(r => r.PurposeType)
            .Where(g => g.Count() >= minSupportingRecords)
            .Select(g => new ReflectiveProposal(
                PurposeType: g.Key,
                Description:
                    $"'{g.Key}' Purpose-tipinde {g.Count()} kararda büyük-büyüklükte öğrenim " +
                    $"sinyali tekrar etti (|Learning| ≥ {magnitudeThreshold}). Bu sınıfın ortak " +
                    "Assumption'ı / relevance-model ağırlığı / attribution-seviye hedefi gözden " +
                    "geçirilmeli. ÖNERİ — otomatik uygulanmaz, insan onayı (P7) gerekir. " +
                    "(Faz-4: yön-sapması değil büyüklük-tekrarı; tam proof-trace okuması yok.)",
                SupportingRecordCount: g.Count(),
                AverageAttributionConfidence: g.Average(r => r.AttributionConfidence)))
            .ToList();
    }
}
