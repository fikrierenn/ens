using Ens.Kernel.Laws;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-3023 (Decision Capital) — §Model 1-3: value, akış (yatırım−amortisman), reuse-ROI

public sealed class DecisionCapitalTests
{
    [Fact]
    public void Value_scales_learning_by_attribution_confidence()
    {
        Assert.Equal(5.0, DecisionCapital.Value(learningMagnitude: 10, attributionConfidence: 0.5), precision: 10);
    }

    [Fact]
    public void Zero_attribution_confidence_yields_zero_value()
    {
        // L0_NoAttribution seviyesinde (ENS-2004) öğrenim sermayeye katkı yapmamalı
        Assert.Equal(0.0, DecisionCapital.Value(learningMagnitude: 100, attributionConfidence: 0.0), precision: 10);
    }

    [Fact]
    public void Negative_learning_magnitude_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(-1, 0.5));
    }

    [Fact]
    public void Attribution_confidence_out_of_range_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(10, 1.5));
    }

    [Fact]
    public void DeltaCapital_positive_when_investment_exceeds_amortization()
    {
        Assert.Equal(30.0, DecisionCapital.DeltaCapital(investment: 50, amortization: 20), precision: 10);
    }

    [Fact]
    public void DeltaCapital_negative_when_amortization_exceeds_investment()
    {
        // Erime hızı yatırımı geçerse sermaye küçülür — bu geçerli bir sonuçtur, hata değil
        Assert.True(DecisionCapital.DeltaCapital(investment: 10, amortization: 40) < 0);
    }

    [Fact]
    public void ReuseROI_computes_infoneed_reduction_per_maintenance_cost()
    {
        Assert.Equal(2.5, DecisionCapital.ReuseROI(infoNeedReduction: 25, maintenanceCost: 10), precision: 10);
    }

    [Fact]
    public void ReuseROI_zero_maintenance_cost_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.ReuseROI(10, 0));
    }

    [Fact]
    public void ReuseROI_negative_infoneed_reduction_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.ReuseROI(-1, 10));
    }

    [Fact]
    public void Unreused_memory_yields_zero_roi()
    {
        // Hiç reuse edilmeyen bellek — ölü sermaye (§Model 3)
        Assert.Equal(0.0, DecisionCapital.ReuseROI(infoNeedReduction: 0, maintenanceCost: 10), precision: 10);
    }
}
