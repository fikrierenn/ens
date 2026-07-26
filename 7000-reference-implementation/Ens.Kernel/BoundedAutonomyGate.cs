using Ens.Kernel.Capability;
using Ens.Kernel.Laws;

namespace Ens.Kernel;

// TRACE: ADR-0001 §5.6 (Bounded-Autonomy Gate) — P7'nin çalışma-zamanı karşılığı.
// TRACE: ADR-0001 §6.1(2) — "Pack'in deklaratif AllowedTools/RequiresHumanApprovalFor izinleri
//        DOĞRUDAN Bounded-Autonomy Gate'e (P7) beslenir" (ENS'in dar prior-art deltası).
// TRACE: Anayasa P7 — "sorumluluk insanda; ENS önerir, emretmez."
// TRACE: AUDIT.md §5.1 (NaN/aralık fail-open) ve §5.5 (Registry↔Gate bağı kodda YOKTU).
//
// Bu, ENS'in en felsefi-yüklü ilkesinin ilk gerçek zorlamasıdır — yorum değil, kod.
// DecisionAggregate.Commit() bunu VARSAYMAZ (çağıran katmanın sorumluluğu, ADR-0001 §5.6);
// bu sınıf o sorumluluğu somutlaştırır.
//
// ============================ DENETİM SONRASI İKİ DÜZELTME ============================
// (1) FAIL-CLOSED GİRDİ KAPISI (AUDIT §5.1). Gate confidence'ı ve stake'i HİÇ doğrulamıyordu.
//     `stake = NaN` → her eşik karşılaştırması false → en permisif dal → **Autonomous**.
//     `confidence = 5.0` → InfoNeed negatif → hiçbir eşiği aşmaz → **Autonomous**.
//     Artık ölçülemeyen girdi otonomi kazanmaz; `Guard` üzerinden REDDEDİLİR.
// (2) REGISTRY → GATE BAĞI (AUDIT §5.5). ADR-0001 §6.1'in "deklaratif izinler doğrudan Gate'e
//     beslenir" iddiası kodda GERÇEKLEŞMEMİŞTİ — `Evaluate` imzasında `ToolAuthorization` yoktu
//     ve demo ikisini yan yana yazdırıp birbirine bağlamıyordu. Artık opsiyonel bir
//     `ToolAuthorization` parametresi var ve iki yönde de zorlanıyor:
//       • yetkisiz araç          → `Blocked` (InfoNeed ne olursa olsun)
//       • onay-gerektiren araç   → asla `Autonomous`/`NotifyHuman` değil, en az `Blocked`
//         ("insan onayı ŞART" ile "insana bildirilir, icra devam eder" aynı şey değildir —
//          muhafazakâr okuma P7 yönündedir).
// ======================================================================================
//
// ============================ DÜRÜST SINIRLAR (Faz-4) ============================
// (a) `GateResult` public bir record'dur; bu katman kendisine verilen bir GateResult'ın
//     gerçekten `Evaluate`'ten geldiğini DOĞRULAYAMAZ (AUDIT §4.1 — sahte gate tek satırda
//     üretilebilir). Kapatılması imzalı/opak bir gate-token'ı gerektirir: AÇIK BORÇ.
// (b) `conformanceDeficit` bugün karar DALINA girmez (yalnızca doğrulanır ve Scheduler'ın
//     sıralamasında kullanılır) — Policy/Constraint node'ları (ENS-4010) bağlanınca değişecek.
// (c) Eşikler KALİBRE DEĞİL (ENS-3022 §Model 1 stake-normalizasyonu kodlanmadı).
// ================================================================================
public enum GateDecision
{
    /// <summary>Düşük InfoNeed, sınır içi — otonom icra + proof-trace.</summary>
    Autonomous,

    /// <summary>Yüksek InfoNeed ama sınır içi — insana bildir, icra devam eder.</summary>
    NotifyHuman,

    /// <summary>Sınır aşımı — blokla, insan onayı olmadan icra edilemez.</summary>
    Blocked,

    /// <summary>Kritik/geri-dönülemez — otomatik blok, bypass yok (Exception-Policy hariç).</summary>
    CriticalBlock
}

public sealed record GateResult(GateDecision Decision, string Reason, double InfoNeed);

public static class BoundedAutonomyGate
{
    /// <summary>
    /// Policy = Constraint bundle (ADR-0001 §5.6). Burada minimalist: bir InfoNeed eşiği,
    /// geri-dönülemezlik bayrağı ve (yeni) Capability Registry'den gelen araç yetkisi.
    /// Gerçek Policy/Constraint node'ları (ENS-4010) Faz-4'ün sonraki adımıdır.
    /// </summary>
    /// <param name="toolAuthorization">
    /// `CapabilityRegistry.Authorize(...)` çıktısı (ADR-0001 §6.1(2)). null = "bu karar bir
    /// araç çağrısına bağlı değil" demektir; verilirse Gate'i P7 yönünde SIKILAŞTIRIR, asla
    /// gevşetmez.
    /// </param>
    public static GateResult Evaluate(
        double stake,
        double? confidence,
        double conformanceDeficit,
        bool isIrreversible,
        double autonomyThreshold,
        double blockThreshold,
        ToolAuthorization? toolAuthorization = null)
    {
        // ── (0) FAIL-CLOSED ÖNCELİĞİ ────────────────────────────────────────────────────────
        // En kısıtlayıcı iki dal, girdi DOĞRULAMASINDAN ÖNCE gelir. Gerekçe: bir doğrulama
        // hatası ASLA bir güvenlik kontrolünü düşürmemelidir. Geri-dönülemez bir action, tüm
        // girdileri ölçülemez (NaN) olsa bile bloklanmak ZORUNDA — "ölçemedim, o yüzden
        // değerlendiremiyorum" bir izin değildir.
        double measuredInfoNeed =
            Guard.IsMeasurable(stake) && stake >= 0 && Guard.IsMeasurableConfidence(confidence)
                ? stake * (1.0 - (confidence ?? 0.0))
                : double.NaN; // ölçülemedi — sahte bir sayı UYDURULMAZ (Madde X)

        if (isIrreversible)
            return new GateResult(
                GateDecision.CriticalBlock,
                "Geri-dönülemez action — otomatik blok, bypass yok (P7)." +
                (double.IsNaN(measuredInfoNeed) ? " (InfoNeed ÖLÇÜLEMEDİ: girdi sonlu/aralıkta değil.)" : ""),
                measuredInfoNeed);

        // ADR-0001 §6.1(2): hiçbir etkin Capability Pack'in izin vermediği araç çalıştırılamaz.
        if (toolAuthorization is { IsAllowed: false })
            return new GateResult(
                GateDecision.Blocked,
                $"Yetkisiz araç — Capability Registry reddetti (ADR-0001 §6.1, P7): {toolAuthorization.Reason}",
                measuredInfoNeed);

        // ── (1) GİRDİ KAPISI (AUDIT §5.1) ───────────────────────────────────────────────────
        // Buradan sonrası otonomi VEREBİLEN yol. Ölçülemeyen girdi bu yola giremez.
        Guard.NonNegativeFinite(stake, nameof(stake), "Stake");
        Guard.OptionalUnitInterval(confidence, nameof(confidence), "Confidence");
        Guard.NormalizedDeficit(conformanceDeficit, nameof(conformanceDeficit), "ConformanceDeficit");
        Guard.Finite(autonomyThreshold, nameof(autonomyThreshold), "Otonomi eşiği");
        Guard.Finite(blockThreshold, nameof(blockThreshold), "Blok eşiği");

        if (blockThreshold < autonomyThreshold)
            throw new ArgumentException("blockThreshold, autonomyThreshold'dan küçük olamaz — tutarsız Policy.");

        double infoNeed = DecisionGravity.InfoNeed(stake, confidence);

        var (decision, reason) =
            infoNeed >= blockThreshold
                ? (GateDecision.Blocked,
                   $"InfoNeed ({infoNeed:F2}) blok eşiğini ({blockThreshold:F2}) aştı — insan onayı gerekli.")
            : infoNeed >= autonomyThreshold
                ? (GateDecision.NotifyHuman,
                   $"InfoNeed ({infoNeed:F2}) otonomi eşiğini aştı — icra devam eder, insana bildirilir.")
                : (GateDecision.Autonomous, "Sınır içi — otonom icra.");

        // ── (2) REGISTRY → GATE BAĞI (ADR-0001 §6.1(2), AUDIT §5.5) ─────────────────────────
        // Deklaratif "insan onayı ŞART" izni, InfoNeed eşiklerinden BAĞIMSIZ olarak otonomiyi
        // kaldırır. Yalnızca sıkılaştırır: mevcut karar zaten daha kısıtlıysa dokunulmaz.
        if (toolAuthorization is { RequiresHumanApproval: true } && decision < GateDecision.Blocked)
            return new GateResult(
                GateDecision.Blocked,
                $"Capability Pack bu araç için insan onayı ŞART koşuyor (ADR-0001 §6.1, P7): " +
                $"{toolAuthorization.Reason} [InfoNeed tek başına '{decision}' verirdi.]",
                infoNeed);

        return new GateResult(decision, reason, infoNeed);
    }
}
