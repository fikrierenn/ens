namespace Ens.Kernel.Laws;

// TRACE: ENS-3023 (Decision Capital) — fizik üçlüsünün üçüncüsü (Entropy, Gravity, Capital)
// TRACE: ENS-3023 §Model 1-3 — stok(=Memory) değil; akış (yatırım−amortisman) + reuse-ROI
//
// Faz-4 sadeleştirmesi (dürüstçe işaretli): Stok hesabı (`Σ value(d)·salience(d)`) burada
// KASITLI olarak yok — ENS-3023 açıkça "bu stok Memory'dir, Decision Capital stoku değil,
// dinamiğini yönetir" der (ENS-2003 Company Memory henüz kodlanmadı). Bu sınıf yalnızca
// Capital'in ayırt edici içeriğini kodlar: akış (yatırım−amortisman) ve reuse-ROI.
public static class DecisionCapital
{
    /// <summary>value(d) = |Learning(d)| × attribution_confidence(d) (§Model 1).</summary>
    public static double Value(double learningMagnitude, double attributionConfidence)
    {
        if (learningMagnitude < 0)
            throw new ArgumentOutOfRangeException(nameof(learningMagnitude), "Learning büyüklüğü negatif olamaz.");
        if (attributionConfidence is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(attributionConfidence), "Attribution confidence [0,1] aralığında olmalı.");

        return learningMagnitude * attributionConfidence;
    }

    /// <summary>ΔCapital = yatırım − amortisman (§Model 2) — statik stok değil, akış.</summary>
    public static double DeltaCapital(double investment, double amortization) => investment - amortization;

    /// <summary>
    /// Reuse ROI (§Model 3): tip-içi reuse'un düşürdüğü InfoNeed (ör. ENS-3022), bakım
    /// maliyetine bölünür. Hiç reuse edilmeyen bellek getirisiz — ölü sermaye.
    /// </summary>
    public static double ReuseROI(double infoNeedReduction, double maintenanceCost)
    {
        if (maintenanceCost <= 0)
            throw new ArgumentOutOfRangeException(nameof(maintenanceCost), "Bakım maliyeti pozitif olmalı — sıfıra bölme, ölçülemez sermaye.");
        if (infoNeedReduction < 0)
            throw new ArgumentOutOfRangeException(nameof(infoNeedReduction), "InfoNeed azalımı negatif olamaz (reuse yararsızsa 0).");

        return infoNeedReduction / maintenanceCost;
    }
}
