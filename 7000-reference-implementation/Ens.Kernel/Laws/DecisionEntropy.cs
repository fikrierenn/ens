namespace Ens.Kernel.Laws;

// TRACE: ENS-3021 (Decision Entropy) — H(A|C) = I(A;Owner|C) + H(A|C,Owner)
// TRACE: ADR-0001 §5.3 (Scheduler, Decision Gravity/VOI için zemin)
//
// Kahneman-Sibony-Sunstein "Noise" (2021)'in Shannon biçimi. Metafor değil — literal
// bilgi-teorik nicelik. Yalnızca `intent=exploit` commitment'lar üzerinden hesaplanır
// (ENS-3021 §Model 3); exploration entropisi ayrı izlenir (Faz-4 borcu, intent alanı
// henüz DecisionAggregate'te yok — OE1, ROADMAP).
//
// Faz-4 sadeleştirmesi (dürüstçe işaretli): Context-benzerliği burada ContextKey (string)
// ile temsil ediliyor — tam Context Score (ENS-2002) henüz bağlı değil. `H(A|C)` bu yüzden
// yaklaşık; gerçek değer Context Theory entegrasyonunu bekliyor.
public static class DecisionEntropy
{
    public sealed record Observation(string ContextKey, string Owner, string SelectedAlternative);

    /// <summary>
    /// H(A|C) — context-koşullu Shannon entropisi. Aynı ContextKey içindeki seçimlerin
    /// dağınıklığını ölçer. 0 = tam tutarlı (her zaman aynı seçim); yüksek = dağınık.
    /// </summary>
    public static double ConditionalEntropy(IReadOnlyList<Observation> observations)
    {
        if (observations.Count == 0) return 0.0;

        var byContext = observations.GroupBy(o => o.ContextKey);
        double total = observations.Count;
        double hac = 0.0;

        foreach (var contextGroup in byContext)
        {
            double pContext = contextGroup.Count() / total;
            double hGivenContext = ShannonEntropy(contextGroup.Select(o => o.SelectedAlternative));
            hac += pContext * hGivenContext;
        }

        return hac;
    }

    /// <summary>
    /// I(A;Owner|C) — level noise. Owner kimliğinin, context sabitken seçime kattığı bilgi.
    /// TRACE: ENS-3021 §Model 1 (Kahneman level noise).
    /// </summary>
    public static double LevelNoise(IReadOnlyList<Observation> observations)
    {
        double hac = ConditionalEntropy(observations);
        double hacOwner = ConditionalEntropyGivenOwner(observations);
        return Math.Max(0, hac - hacOwner); // I(A;Owner|C) = H(A|C) − H(A|C,Owner)
    }

    /// <summary>
    /// H(A|C,Owner) — pattern noise. Aynı Owner'ın aynı context'te tutarsızlığı.
    /// TRACE: ENS-3021 §Model 1 (Kahneman pattern noise).
    /// </summary>
    public static double ConditionalEntropyGivenOwner(IReadOnlyList<Observation> observations)
    {
        if (observations.Count == 0) return 0.0;

        var byContextOwner = observations.GroupBy(o => (o.ContextKey, o.Owner));
        double total = observations.Count;
        double h = 0.0;

        foreach (var group in byContextOwner)
        {
            double p = group.Count() / total;
            double hGiven = ShannonEntropy(group.Select(o => o.SelectedAlternative));
            h += p * hGiven;
        }

        return h;
    }

    private static double ShannonEntropy(IEnumerable<string> choices)
    {
        var list = choices.ToList();
        if (list.Count == 0) return 0.0;

        return list
            .GroupBy(c => c)
            .Select(g => (double)g.Count() / list.Count)
            .Sum(p => p > 0 ? -p * Math.Log2(p) : 0.0);
    }
}
