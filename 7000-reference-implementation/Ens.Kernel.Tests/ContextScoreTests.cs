using Ens.Kernel.Domain;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-2002 (Context Theory) §3 — ContextScore = coverage−noise−staleness; §Implications gate

public sealed class ContextScoreTests
{
    [Fact]
    public void Compute_subtracts_noise_and_staleness_from_coverage()
    {
        Assert.Equal(0.5, ContextScore.Compute(coverage: 0.8, noisePenalty: 0.2, staleness: 0.1), precision: 10);
    }

    [Fact]
    public void Compute_can_go_negative_when_noise_and_staleness_dominate()
    {
        // Ters-U: ağır noise/staleness coverage'ı boğabilir — geçerli sonuç, hata değil
        Assert.True(ContextScore.Compute(coverage: 0.2, noisePenalty: 0.5, staleness: 0.4) < 0);
    }

    [Fact]
    public void Compute_negative_coverage_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.Compute(-0.1, 0, 0));
    }

    [Fact]
    public void GateConfidence_passes_through_when_score_meets_threshold()
    {
        Assert.Equal(0.9, ContextScore.GateConfidence(rawConfidence: 0.9, contextScore: 0.7, threshold: 0.5), precision: 10);
    }

    [Fact]
    public void GateConfidence_caps_when_score_below_threshold()
    {
        // Score eşiğin yarısında (0.25/0.5) → Confidence en fazla 0.5'e kapılanır
        double gated = ContextScore.GateConfidence(rawConfidence: 0.9, contextScore: 0.25, threshold: 0.5);
        Assert.Equal(0.5, gated, precision: 10);
    }

    [Fact]
    public void GateConfidence_coldstart_zero_coverage_forces_near_zero_confidence()
    {
        // Cold-start (ENS-2002 §Model 2): memory yok → coverage≈0 → Score≈0 → Confidence boğulur.
        // Bu bir kusur değil, doğru davranış — "bilinmeyen alanda düşük güven" (P6).
        double score = ContextScore.Compute(coverage: 0.0, noisePenalty: 0.0, staleness: 0.0);
        double gated = ContextScore.GateConfidence(rawConfidence: 0.9, contextScore: score, threshold: 0.5);
        Assert.Equal(0.0, gated, precision: 10);
    }

    [Fact]
    public void GateConfidence_negative_score_clamps_cap_to_zero()
    {
        double gated = ContextScore.GateConfidence(rawConfidence: 0.8, contextScore: -0.3, threshold: 0.5);
        Assert.Equal(0.0, gated, precision: 10);
    }

    [Fact]
    public void GateConfidence_never_raises_confidence_above_raw()
    {
        // Score yüksek olsa bile (threshold'u aşan) Confidence olduğu gibi kalır — asla yükseltilmez
        double gated = ContextScore.GateConfidence(rawConfidence: 0.4, contextScore: 5.0, threshold: 0.5);
        Assert.Equal(0.4, gated, precision: 10);
    }

    [Fact]
    public void GateConfidence_out_of_range_raw_confidence_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.GateConfidence(1.5, 0.5, 0.5));
    }
}
