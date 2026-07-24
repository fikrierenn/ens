using Ens.Kernel.Laws;

namespace Ens.Kernel;

// TRACE: ADR-0001 §5.6 (Bounded-Autonomy Gate) — P7'nin çalışma-zamanı karşılığı.
// TRACE: Anayasa P7 — "sorumluluk insanda; ENS önerir, emretmez."
//
// Bu, ENS'in en felsefi-yüklü ilkesinin ilk gerçek zorlamasıdır — yorum değil, kod.
// DecisionAggregate.Commit() bunu VARSAYMAZ (çağıran katmanın sorumluluğu, ADR-0001 §5.6);
// bu sınıf o sorumluluğu somutlaştırır.
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
    /// Policy = Constraint bundle (ADR-0001 §5.6). Burada minimalist: bir InfoNeed eşiği
    /// ve geri-dönülemezlik bayrağı. Gerçek Policy/Constraint node'ları (ENS-4010) Faz-4'ün
    /// sonraki adımıdır — bu, o modelin ilk çalışan yaklaşımı.
    /// </summary>
    public static GateResult Evaluate(
        double stake,
        double? confidence,
        double conformanceDeficit,
        bool isIrreversible,
        double autonomyThreshold,
        double blockThreshold)
    {
        if (blockThreshold < autonomyThreshold)
            throw new ArgumentException("blockThreshold, autonomyThreshold'dan küçük olamaz — tutarsız Policy.");

        double infoNeed = DecisionGravity.InfoNeed(stake, confidence);

        // TRACE: Anayasa P7 — geri-dönülemez action'lar bypass'sız bloklanır, InfoNeed'den bağımsız.
        if (isIrreversible)
            return new GateResult(GateDecision.CriticalBlock, "Geri-dönülemez action — otomatik blok, bypass yok (P7).", infoNeed);

        if (infoNeed >= blockThreshold)
            return new GateResult(GateDecision.Blocked, $"InfoNeed ({infoNeed:F2}) blok eşiğini ({blockThreshold:F2}) aştı — insan onayı gerekli.", infoNeed);

        if (infoNeed >= autonomyThreshold)
            return new GateResult(GateDecision.NotifyHuman, $"InfoNeed ({infoNeed:F2}) otonomi eşiğini aştı — icra devam eder, insana bildirilir.", infoNeed);

        return new GateResult(GateDecision.Autonomous, "Sınır içi — otonom icra.", infoNeed);
    }
}
