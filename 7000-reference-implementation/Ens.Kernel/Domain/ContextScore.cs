namespace Ens.Kernel.Domain;

// TRACE: ENS-2002 (Context Theory) §3 — ContextScore = coverage − noise_penalty − staleness
// TRACE: ENS-2002 §Implications — "Context Score, Confidence'ı kapılar (P6)"
//
// Faz-4 sadeleştirmesi (dürüstçe işaretli): `coverage` (relevance ağırlıklı kapsam) burada
// dışarıdan verilir. ENS-2002 §Model 2, relevance'ın Company Memory'den (ENS-2003, henüz
// kodlanmadı) kestirilmesini ZORUNLU kılar — "döngüyü şimdiki karardan değil geçmişten kır."
// Bu sınıf o kestirim mekanizmasını içermez; yalnızca ENS-2002 §3'ün ters-U formülünü ve
// §Implications'ın Confidence-kapılama kuralını kodlar. Memory kodlanınca `coverage` bu
// sınıfa dışarıdan değil, gerçek relevance kestiriminden gelecek.
public static class ContextScore
{
    /// <summary>
    /// ContextScore = coverage − noise_penalty − staleness (§3, ters-U: az context = gap,
    /// çok context = noise). Değerler negatife düşebilir — bu geçerlidir (ağır noise/staleness
    /// coverage'ı boğar).
    /// </summary>
    public static double Compute(double coverage, double noisePenalty, double staleness)
    {
        // AUDIT §5.1 ile aynı kök neden (`is < 0` NaN'ı geçirir) — burada da kapatıldı.
        Guard.NonNegativeFinite(coverage, nameof(coverage), "Coverage");
        Guard.NonNegativeFinite(noisePenalty, nameof(noisePenalty), "Noise penalty");
        Guard.NonNegativeFinite(staleness, nameof(staleness), "Staleness");

        return coverage - noisePenalty - staleness;
    }

    /// <summary>
    /// §Implications: "düşük Score → düşük Confidence ya da ertele." ContextScore eşiğin
    /// altındaysa, ham Confidence Score'a kapılanır (yukarı sınırlanır) — cold-start'ta
    /// (memory yok, coverage≈0) bu otomatik olarak düşük Confidence'a yol açar; bu bir kusur
    /// değil, doğru davranıştır (§Model 2, "bilinmeyen alanda düşük güven").
    /// </summary>
    public static double GateConfidence(double rawConfidence, double contextScore, double threshold)
    {
        Guard.UnitInterval(rawConfidence, nameof(rawConfidence), "Confidence");
        Guard.Finite(contextScore, nameof(contextScore), "ContextScore");
        Guard.Finite(threshold, nameof(threshold), "Eşik");

        if (contextScore >= threshold) return rawConfidence;

        // Score eşiğin altında: Confidence, Score'un [0,threshold] aralığındaki oranına kapılanır.
        double cap = threshold <= 0 ? 0.0 : Math.Clamp(contextScore / threshold, 0.0, 1.0);
        return Math.Min(rawConfidence, cap);
    }
}
