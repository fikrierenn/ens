using Ens.Kernel.Laws;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-3022 (Decision Gravity) — InfoNeed = Stake × (1−Confidence)

public sealed class DecisionGravityTests
{
    [Fact]
    public void Full_confidence_yields_zero_infoneed()
    {
        Assert.Equal(0.0, DecisionGravity.InfoNeed(stake: 100, confidence: 1.0), precision: 10);
    }

    [Fact]
    public void Zero_confidence_yields_infoneed_equal_to_stake()
    {
        Assert.Equal(100.0, DecisionGravity.InfoNeed(stake: 100, confidence: 0.0), precision: 10);
    }

    [Fact]
    public void Null_confidence_treated_as_maximal_uncertainty()
    {
        // Henüz commit edilmemiş Decision — confidence yok — belirsizlik maksimum
        Assert.Equal(100.0, DecisionGravity.InfoNeed(stake: 100, confidence: null), precision: 10);
    }

    [Fact]
    public void InfoNeed_scales_linearly_with_stake()
    {
        Assert.Equal(25.0, DecisionGravity.InfoNeed(stake: 50, confidence: 0.5), precision: 10);
    }

    [Fact]
    public void Negative_stake_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionGravity.InfoNeed(stake: -1, confidence: 0.5));
    }

    [Fact]
    public void AttentionPriority_zero_when_conformance_deficit_zero()
    {
        Assert.Equal(0.0, DecisionGravity.AttentionPriority(stake: 100, confidence: 0.0, conformanceDeficit: 0.0), precision: 10);
    }

    [Fact]
    public void AttentionPriority_scales_with_conformance_deficit()
    {
        double low = DecisionGravity.AttentionPriority(stake: 100, confidence: 0.0, conformanceDeficit: 0.2);
        double high = DecisionGravity.AttentionPriority(stake: 100, confidence: 0.0, conformanceDeficit: 0.8);
        Assert.True(high > low);
    }

    [Fact]
    public void AttentionPriority_negative_conformance_deficit_clamped_to_zero()
    {
        // Negatif deficit (aşırı-uyum) risk artırmamalı — Math.Max(0, ...) ile clamp edilir
        Assert.Equal(0.0, DecisionGravity.AttentionPriority(stake: 100, confidence: 0.0, conformanceDeficit: -0.5), precision: 10);
    }
}
