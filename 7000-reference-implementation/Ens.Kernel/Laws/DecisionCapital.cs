namespace Ens.Kernel.Laws;

// TRACE: ENS-3023 (Decision Capital) — fizik üçlüsünün üçüncüsü (Entropy, Gravity, Capital)
// TRACE: ENS-3023 §Model 1-3 — stok(=Memory) değil; akış (yatırım−amortisman) + reuse-ROI
//
// Faz-4 sadeleştirmesi (dürüstçe işaretli): Stok hesabı (`Σ value(d)·salience(d)`) burada
// KASITLI olarak yok — ENS-3023 açıkça "bu stok Memory'dir, Decision Capital stoku değil,
// dinamiğini yönetir" der. Company Memory artık kodlu (Domain/CompanyMemory.cs, ENS-2003
// v0.3.1 ratified) ama bu iki sınıf henüz birbirine bağlanmadı — stok hesabı hâlâ kasıtlı
// olarak burada değil. Bu sınıf yalnızca Capital'in ayırt edici içeriğini kodlar: akış
// (yatırım−amortisman) ve reuse-ROI.
public static class DecisionCapital
{
    /// <summary>
    /// value(d) = |Learning(d)| × attribution_confidence(d) (§Model 1).
    ///
    /// ENS-2003 v0.4.0'dan beri bu nicelik **Company Memory'nin retrieval ağırlığıdır**
    /// (`MemoryRecord.CapitalValue`) — `RetentionPriority` DEĞİLDİR. v0.3'te `RetentionPriority`
    /// buraya bağlıydı ve attribution confidence hem burada hem sönüm hızında sayılıyordu:
    /// çift-sayım (AUDIT-WAVE2-FIDELITY/D-5). Ayrıştırıldı: `RetentionPriority = |Learning|` (saf),
    /// `value = |Learning|·c` (bu metot), `decayFactor = exp(−λ_π·Δt)` (`c` içermez).
    ///
    /// AUDIT §5.6/J1: eskiden NaN ve ±Infinity sessizce geçiyordu — `Value(NaN, 0.5)` = NaN
    /// doğrudan memory sıralamasına akıp kaydı hem sıralamada en sona düşürüyor
    /// hem de Curator'a görünmez yapıyordu (sessiz kurumsal amnezi). Artık reddedilir.
    /// </summary>
    public static double Value(double learningMagnitude, double attributionConfidence)
    {
        Guard.NonNegativeFinite(learningMagnitude, nameof(learningMagnitude), "Learning büyüklüğü");
        Guard.UnitInterval(attributionConfidence, nameof(attributionConfidence), "Attribution confidence");

        return learningMagnitude * attributionConfidence;
    }

    /// <summary>ΔCapital = yatırım − amortisman (§Model 2) — statik stok değil, akış.</summary>
    public static double DeltaCapital(double investment, double amortization)
    {
        Guard.Finite(investment, nameof(investment), "Yatırım");
        Guard.Finite(amortization, nameof(amortization), "Amortisman");
        return investment - amortization;
    }

    /// <summary>
    /// Reuse ROI (§Model 3): tip-içi reuse'un düşürdüğü InfoNeed (ör. ENS-3022), bakım
    /// maliyetine bölünür. Hiç reuse edilmeyen bellek getirisiz — ölü sermaye.
    /// </summary>
    public static double ReuseROI(double infoNeedReduction, double maintenanceCost)
    {
        Guard.PositiveFinite(maintenanceCost, nameof(maintenanceCost), "Bakım maliyeti (sıfıra bölme = ölçülemez sermaye)");
        Guard.NonNegativeFinite(infoNeedReduction, nameof(infoNeedReduction), "InfoNeed azalımı (reuse yararsızsa 0)");

        return infoNeedReduction / maintenanceCost;
    }
}
