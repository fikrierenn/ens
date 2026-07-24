using Ens.Kernel;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ADR-0001 §5.6, Anayasa P7 — Gate'in dört sonucunun (Autonomous/NotifyHuman/
// Blocked/CriticalBlock) her biri InfoNeed eşiklerine ve geri-dönülemezlik bayrağına göre
// deterministik olarak doğru koluna düşmeli.

public sealed class BoundedAutonomyGateTests
{
    [Fact]
    public void Low_infoneed_within_bounds_is_autonomous()
    {
        var result = BoundedAutonomyGate.Evaluate(
            stake: 10, confidence: 0.95, conformanceDeficit: 0,
            isIrreversible: false, autonomyThreshold: 5, blockThreshold: 20);

        Assert.Equal(GateDecision.Autonomous, result.Decision);
    }

    [Fact]
    public void Infoneed_above_autonomy_threshold_but_below_block_notifies_human()
    {
        var result = BoundedAutonomyGate.Evaluate(
            stake: 100, confidence: 0.5, conformanceDeficit: 0, // InfoNeed = 50
            isIrreversible: false, autonomyThreshold: 10, blockThreshold: 80);

        Assert.Equal(GateDecision.NotifyHuman, result.Decision);
    }

    [Fact]
    public void Infoneed_at_or_above_block_threshold_is_blocked()
    {
        var result = BoundedAutonomyGate.Evaluate(
            stake: 100, confidence: 0.0, conformanceDeficit: 0, // InfoNeed = 100
            isIrreversible: false, autonomyThreshold: 10, blockThreshold: 80);

        Assert.Equal(GateDecision.Blocked, result.Decision);
    }

    [Fact]
    public void Irreversible_action_is_critical_block_regardless_of_infoneed()
    {
        // InfoNeed = 0 (tam güven) olsa bile geri-dönülemezlik otomatik blok — P7
        var result = BoundedAutonomyGate.Evaluate(
            stake: 1000, confidence: 1.0, conformanceDeficit: 0,
            isIrreversible: true, autonomyThreshold: 10, blockThreshold: 80);

        Assert.Equal(GateDecision.CriticalBlock, result.Decision);
    }

    [Fact]
    public void Inconsistent_thresholds_throw()
    {
        Assert.Throws<ArgumentException>(() => BoundedAutonomyGate.Evaluate(
            stake: 10, confidence: 0.5, conformanceDeficit: 0,
            isIrreversible: false, autonomyThreshold: 80, blockThreshold: 10));
    }

    [Fact]
    public void Result_carries_computed_infoneed_for_proof_trace()
    {
        var result = BoundedAutonomyGate.Evaluate(
            stake: 100, confidence: 0.5, conformanceDeficit: 0,
            isIrreversible: false, autonomyThreshold: 10, blockThreshold: 80);

        Assert.Equal(50.0, result.InfoNeed, precision: 10);
    }
}
