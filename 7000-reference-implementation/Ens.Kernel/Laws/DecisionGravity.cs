namespace Ens.Kernel.Laws;

// TRACE: ENS-3022 (Decision Gravity) — InfoNeed = Stake × (1−Confidence), Howard (1966) VOI
// TRACE: ADR-0001 §5.3 (Scheduler), §5.6 (Bounded-Autonomy Gate eşiği buradan gelir)
//
// Faz-4 sadeleştirmesi (dürüstçe işaretli): `Stake` burada dışarıdan verilir — ENS-2001'e
// henüz Alternative-başına ExpectedValue eklenmedi (OL1, ROADMAP açık borç), bu yüzden
// Stake = spread(ExpectedValue) hesaplanamıyor. `ConformanceDeficit` de Company Memory
// (ENS-2003, henüz kodlanmadı) olmadan hesaplanamaz — 0 varsayılabilir (peer-verisi yok
// demek, açık ölçülemez demek, "açık yok" demek değil).
public static class DecisionGravity
{
    /// <summary>InfoNeed = Stake × (1−Confidence). Confidence null ise (henüz commit
    /// edilmemiş) belirsizlik maksimum (1.0) varsayılır.</summary>
    public static double InfoNeed(double stake, double? confidence)
    {
        if (stake < 0) throw new ArgumentOutOfRangeException(nameof(stake), "Stake negatif olamaz.");
        double uncertainty = 1.0 - (confidence ?? 0.0);
        return stake * uncertainty;
    }

    /// <summary>
    /// AttentionPriority = InfoNeed × max(ConformanceDeficit, 0). ConformanceDeficit
    /// bilinmiyorsa (Memory yok) 0 varsayılır — bu durumda AttentionPriority de 0'a düşer;
    /// bu bir yanlış-negatif riskidir (Memory eklenene dek dürüstçe açık, README'de not).
    /// </summary>
    public static double AttentionPriority(double stake, double? confidence, double conformanceDeficit)
        => InfoNeed(stake, confidence) * Math.Max(0, conformanceDeficit);
}
