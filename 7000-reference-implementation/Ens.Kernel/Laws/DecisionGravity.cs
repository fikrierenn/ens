namespace Ens.Kernel.Laws;

// TRACE: ENS-3022 (Decision Gravity) — InfoNeed = Stake × (1−Confidence), Howard (1966) VOI
// TRACE: ADR-0001 §5.3 (Scheduler), §5.6 (Bounded-Autonomy Gate eşiği buradan gelir)
// TRACE: AUDIT.md §5.1 / §5.6 — NaN fail-open ve sınırsız ConformanceDeficit kusurları burada kapandı.
//
// Faz-4 sadeleştirmesi (dürüstçe işaretli): `Stake` burada dışarıdan verilir — ENS-2001'e
// henüz Alternative-başına ExpectedValue eklenmedi (OL1, ROADMAP açık borç), bu yüzden
// Stake = spread(ExpectedValue) hesaplanamıyor. `ConformanceDeficit` de Company Memory
// (ENS-2003) peer-context fitine bağlanmadan hesaplanamaz — 0 varsayılabilir (peer-verisi yok
// demek, açık ölçülemez demek, "açık yok" demek değil).
//
// DÜZELTME (AUDIT §5.1, kök neden `is < 0 or > 1`'in NaN körlüğü): bu dosya artık TÜM sayısal
// girdilerini `Guard`'dan geçirir. Daha önce `InfoNeed` confidence'ı HİÇ doğrulamıyordu —
// `confidence = 5.0` InfoNeed'i negatife çeviriyor, `stake = NaN` her eşik karşılaştırmasını
// false yapıyordu; ikisi de gate'te tam otonomiye dönüşüyordu. Artık ölçülemeyen girdi
// hesaplanmaz: REDDEDİLİR.
public static class DecisionGravity
{
    /// <summary>
    /// InfoNeed = Stake × (1−Confidence). Confidence null ise (henüz commit edilmemiş)
    /// belirsizlik maksimum (1.0) varsayılır — muhafazakâr yön.
    /// Stake sonlu ve ≥ 0; confidence (varsa) sonlu ve [0,1] olmak ZORUNDA (fail-closed).
    /// </summary>
    public static double InfoNeed(double stake, double? confidence)
    {
        Guard.NonNegativeFinite(stake, nameof(stake), "Stake");
        double resolved = Guard.OptionalUnitInterval(confidence, nameof(confidence), "Confidence") ?? 0.0;

        double uncertainty = 1.0 - resolved;
        return stake * uncertainty;
    }

    /// <summary>
    /// AttentionPriority = InfoNeed × clamp(ConformanceDeficit, 0, 1). ConformanceDeficit
    /// bilinmiyorsa (Memory peer-fiti yok) 0 varsayılır — bu durumda AttentionPriority de 0'a
    /// düşer; bu bir yanlış-negatif riskidir (dürüstçe açık, README'de not).
    ///
    /// ENS-3022 ConformanceDeficit'i NORMALİZE bir açık olarak tanımlar. Eskiden üst sınır
    /// zorlanmıyordu ve `deficit = 1e9` veren bir çağıran, önemsiz bir kararı en kritik kararın
    /// önüne geçirebiliyordu (AUDIT §5.6 — dikkat kuyruğu manipüle edilebilir). Artık iki uçta
    /// da kırpılır; kırpma yalnızca önceliği DÜŞÜRÜR, asla artırmaz.
    /// </summary>
    public static double AttentionPriority(double stake, double? confidence, double conformanceDeficit)
        => InfoNeed(stake, confidence)
           * Guard.NormalizedDeficit(conformanceDeficit, nameof(conformanceDeficit), "ConformanceDeficit");
}
