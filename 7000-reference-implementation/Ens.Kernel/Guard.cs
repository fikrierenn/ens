namespace Ens.Kernel;

// TRACE: ADR-0001 (accepted, v0.3.1) §5.6 "Bounded-Autonomy Gate" — P7'nin çalışma-zamanı zorlaması.
// TRACE: Anayasa P7 ("sorumluluk insanda"), P6 (kalibre confidence), Madde X (yanlışlanabilirlik).
// TRACE: AUDIT.md §5.1 — "P7 gate'i NaN ve confidence > 1 altında FAIL-OPEN" (en tehlikeli teknik bulgu).
//
// ============================ NEDEN BU DOSYA VAR ============================
// Bağımsız düşmanca denetim (AUDIT.md §5.1) tek bir kök neden buldu ve o kök neden BEŞ ayrı
// dosyada tekrarlıyordu: `x is < 0 or > 1` deseni **NaN'ı yakalamaz**. IEEE-754'te NaN her
// karşılaştırmada `false` döner, yani `NaN is < 0 or > 1` → false → guard delinir.
//
// Somut sonuç (denetimin ürettiği kanıt): `stake = NaN` ya da `confidence = 5.0` veren bir
// çağıran, `BoundedAutonomyGate`'ten **Autonomous** (tam otonomi) alıyordu; Scheduler üzerinden
// bakıldığında üçlü fail-open oluşuyordu — gate açık + en ucuz model + `OrderByDescending`'de
// en sonda (dikkat bütçesi asla ulaşmaz). Ölçülemeyen bir karar, sistemin en karanlık köşesine
// SESSİZCE düşüp tam otonomi kazanıyordu. Bu, P7'nin tam tersidir.
//
// POLİTİKA (AUDIT.md §7/4'ün talebi): **ölçülemeyen girdi otonomi KAZANAMAZ.** Bu dosya tek
// kapıdır: sonlu-olmayan (NaN/±Infinity) ya da tanım-kümesi dışındaki her sayısal girdi
// REDDEDİLİR (exception), sessizce geçirilmez. Fail-CLOSED.
//
// Guard'ın kullanıldığı yerler (kök nedenin kapatıldığı DOKUZ nokta):
//   1. Laws/DecisionGravity.cs      (InfoNeed / AttentionPriority)
//   2. BoundedAutonomyGate.cs       (Evaluate — güvenlik-kritik yol)
//   3. ProofTrace.cs                (Premise.Confidence — L7 t-norm girdisi)
//   4. Laws/DecisionCapital.cs      (Value / ReuseROI)
//   5. Domain/DecisionAggregate.cs  (Commit + Rehydrate confidence'ı)
//   6. Domain/CompanyMemory.cs      (MemoryRecord ctor + DecayFactor/FindStale/FindWeaklyAttributed
//                                    + DecayFunction; v0.4.0'da λ_π kapısı DecayFactor'a taşındı)
//   7. Domain/ContextScore.cs       (aynı `is < 0 or > 1` deseninin altıncı örneği)
//   8. Adapter/LlmAdapter.cs        (LlmTierSelector — infoNeed + iki tier eşiği)   ← W11
//   9. Scheduler.cs                 (tier eşiklerinin parti-öncesi doğrulaması)     ← W11
//
// ============================ DÜZELTİLMİŞ SAYIM (AUDIT-WAVE2 §2 / W11) ============================
// Bu dosyanın önceki hâli "kök neden YEDİ noktada kapatıldı" diyordu ve bu SAYIM YANLIŞTI.
// Bağımsız ikinci dalga (AUDIT-WAVE2-SCHEDULER.md §2) sekizinci noktayı buldu: `LlmTierSelector`
// Guard'ı HİÇ çağırmıyordu; `criticalThreshold < complexThreshold` kontrolü NaN altında `false`
// dönüyor ve her karar en zayıf modele (`LlmTier.Operational`) sessizce yönleniyordu. Scheduler
// eşikleri doğrudan geçirdiği için vektör uçtan uca açıktı. Sekizinci ve dokuzuncu nokta
// (adapter + scheduler) yukarıda kapatıldı; kanıt: `AUDIT_FIXED_W11_*` testleri.
// DERS: "kapattım" iddiası bir SAYIMDIR ve sayımlar yanlışlanabilir (Madde X) — bu blok o yüzden
// listeyi taşımaya devam ediyor; yeni bir sayısal karar yolu eklenirse buraya YAZILMALIDIR.
// ==================================================================================================
//
// ============================ DÜRÜST SINIR ============================
// Bu bir DOĞRULAMA kapısıdır, bir KALİBRASYON kapısı değil. "confidence = 0.83 gerçekten
// kalibre mi?" sorusunu yanıtlamaz — yalnızca "bu sayı ÖLÇÜLEBİLİR bir sayı mı?" sorusunu
// yanıtlar. Kalibrasyon borcu (ENS-3022 §Model 1 stake-normalizasyonu, ENS-2003 v0.4.0'ın `τ_π`
// context yarı-ömrü — v0.3'ün `γ`'sı artık YOK, bkz. AUDIT-WAVE2/D-5 —, LlmTierSelector eşikleri)
// AÇIK KALMAYA devam eder — README'de işaretli.
// ======================================================================

/// <summary>
/// Sayısal girdi kapısı (fail-closed). Ölçülemeyen (NaN/±Infinity) ya da tanım-kümesi dışındaki
/// hiçbir değer kernel'in karar yollarına giremez. Bkz. AUDIT.md §5.1.
/// </summary>
public static class Guard
{
    /// <summary>Değer ölçülebilir mi (sonlu)? Exception atmaz — dallanma için.</summary>
    public static bool IsMeasurable(double value) => double.IsFinite(value);

    /// <summary>
    /// Confidence ölçülebilir mi? `null` = "henüz commit edilmedi" (geçerli, maksimum belirsizlik
    /// anlamına gelir — ENS-3022). Sonlu-olmayan ya da [0,1] dışı = ölçülemez.
    /// </summary>
    public static bool IsMeasurableConfidence(double? confidence)
        => confidence is null || (double.IsFinite(confidence.Value) && confidence.Value is >= 0 and <= 1);

    /// <summary>Sonlu olmalı: NaN ve ±Infinity reddedilir.</summary>
    public static double Finite(double value, string paramName, string what)
    {
        if (!double.IsFinite(value))
            throw new ArgumentOutOfRangeException(paramName, value,
                $"{what} ölçülebilir bir sayı olmalı (NaN/±Infinity reddedilir). " +
                "Ölçülemeyen girdi otonomi kazanamaz — fail-closed (Anayasa P7, AUDIT §5.1).");
        return value;
    }

    /// <summary>Sonlu ve ≥ 0 olmalı.</summary>
    public static double NonNegativeFinite(double value, string paramName, string what)
    {
        Finite(value, paramName, what);
        if (value < 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"{what} negatif olamaz.");
        return value;
    }

    /// <summary>Sonlu ve &gt; 0 olmalı.</summary>
    public static double PositiveFinite(double value, string paramName, string what)
    {
        Finite(value, paramName, what);
        if (value <= 0)
            throw new ArgumentOutOfRangeException(paramName, value, $"{what} pozitif olmalı.");
        return value;
    }

    /// <summary>
    /// Sonlu ve [0,1] aralığında olmalı (P6 kalibrasyon aralığı). NaN burada kesin olarak
    /// reddedilir — eski `is &lt; 0 or &gt; 1` deseninin körlüğü kapanır.
    /// </summary>
    public static double UnitInterval(double value, string paramName, string what)
    {
        Finite(value, paramName, what);
        if (value is < 0 or > 1)
            throw new ArgumentOutOfRangeException(paramName, value,
                $"{what} [0,1] aralığında olmalı (P6 kalibrasyon).");
        return value;
    }

    /// <summary>
    /// Opsiyonel confidence: `null` geçerlidir (henüz commit edilmemiş karar — ENS-3022'de
    /// maksimum belirsizlik varsayılır). Değer varsa [0,1] ve sonlu olmak ZORUNDA.
    /// </summary>
    public static double? OptionalUnitInterval(double? value, string paramName, string what)
    {
        if (value is null) return null;
        UnitInterval(value.Value, paramName, what);
        return value;
    }

    /// <summary>
    /// ConformanceDeficit gibi TANIM GEREĞİ normalize [0,1] büyüklükleri için: sonluluk ZORUNLU
    /// (NaN/Infinity reddedilir), aralık dışı değerler ise MUHAFAZAKÂR yönde kırpılır.
    ///
    /// Neden kırpma, neden exception değil (AUDIT.md §5.6):
    ///   - Negatif deficit (aşırı-uyum) zaten `max(0, ·)` ile kırpılıyordu ve bu KASITLI:
    ///     "aşırı-uyum dikkat önceliğini artırmamalı" (DecisionGravity, mevcut test).
    ///   - Üst uçta kırpma da AYNI yönde muhafazakârdır: önceliği DÜŞÜRÜR, asla artırmaz.
    ///     Denetimin bulduğu saldırı (`deficit = 1e9` ile dikkat kuyruğunu ele geçirme) tam
    ///     olarak burada kapanır — çünkü hiçbir çağıran 1'in üstüne çıkamaz.
    ///   - Tüm partiyi bir tek bozuk peer-sinyali yüzünden exception'la düşürmek, dikkat
    ///     tahsisini komple durdurur (servis-dışı bırakma vektörü); kırpma fail-closed kalır.
    /// NaN ise KIRPILMAZ — reddedilir: ölçülemeyen bir açık, "açık yok" demek değildir.
    /// </summary>
    public static double NormalizedDeficit(double value, string paramName, string what)
    {
        Finite(value, paramName, what);
        return Math.Clamp(value, 0.0, 1.0);
    }
}
