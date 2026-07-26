using Ens.Kernel;
using Ens.Kernel.Domain;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ADR-0001 §5.4 — "hiçbir katman bu lifecycle'ı ATLAYAMAZ" invariant'ının testleri.
// TRACE: Anayasa P7 (Gate'siz Acting yok), Madde VI (izsiz başarısızlık bile yasak).

public sealed class ActuationLayerTests
{
    private static readonly DateTimeOffset T0 = new(2026, 7, 25, 12, 0, 0, TimeSpan.Zero);

    private static ActuationLayer NewLayer() => new(Identity.New());

    private static ProofTrace SampleTrace() =>
        new("BAG-P3", "Action-42 enacted", [new Premise("plan", 0.8), new Premise("context", 0.7)]);

    private static GateResult AllowingGate() =>
        BoundedAutonomyGate.Evaluate(100, 0.9, 0.1, false, autonomyThreshold: 50, blockThreshold: 5000);

    private static GateResult BlockingGate() =>
        BoundedAutonomyGate.Evaluate(100, 0.9, 0.1, isIrreversible: true, autonomyThreshold: 50, blockThreshold: 5000);

    [Fact]
    public void New_action_starts_in_Planned()
    {
        Assert.Equal(ActionState.Planned, NewLayer().State);
    }

    [Fact]
    public void Cannot_act_without_passing_gate()
    {
        // P7'nin yapısal zorlanışı: Planned'dan doğrudan Acting'e geçilemez.
        var layer = NewLayer();
        var ex = Assert.Throws<InvalidTransitionException>(() => layer.BeginActing(T0));
        Assert.Equal(ActionState.Planned, ex.From);
        Assert.Equal(ActionState.Acting, ex.To);
    }

    [Fact]
    public void Blocking_gate_lands_in_Blocked_terminal()
    {
        var layer = NewLayer();
        layer.ApplyGate(BlockingGate(), T0);

        Assert.Equal(ActionState.Blocked, layer.State);
        Assert.True(layer.IsTerminal);
    }

    [Fact]
    public void Blocked_action_cannot_proceed_to_acting()
    {
        var layer = NewLayer();
        layer.ApplyGate(BlockingGate(), T0);

        Assert.Throws<InvalidTransitionException>(() => layer.BeginActing(T0));
    }

    [Fact]
    public void Allowing_gate_lands_in_Contextualized()
    {
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);

        Assert.Equal(ActionState.Contextualized, layer.State);
        Assert.False(layer.IsTerminal);
    }

    [Fact]
    public void Full_happy_path_reaches_Remembered()
    {
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0, "teslimat gecikmesi %7");
        layer.RecordTrace(SampleTrace(), T0);
        layer.RecordLearning(T0, "beklenen %5, gerçekleşen %7");
        layer.Remember(T0);

        Assert.Equal(ActionState.Remembered, layer.State);
        Assert.True(layer.IsTerminal);
    }

    [Fact]
    public void Failed_action_must_still_be_traced()
    {
        // Madde VI: izsiz başarısızlık yasak — Failed'dan yalnızca Traced'e geçilebilir.
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Fail(T0, "timeout");

        Assert.Throws<InvalidTransitionException>(() => layer.RecordLearning(T0));  // trace atlanamaz
        layer.RecordTrace(SampleTrace(), T0);
        Assert.Equal(ActionState.Traced, layer.State);
    }

    [Fact]
    public void Cannot_skip_trace_on_happy_path_either()
    {
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0);

        Assert.Throws<InvalidTransitionException>(() => layer.RecordLearning(T0));
    }

    [Fact]
    public void Cannot_skip_learning_before_remembering()
    {
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0);
        layer.RecordTrace(SampleTrace(), T0);

        Assert.Throws<InvalidTransitionException>(() => layer.Remember(T0));
    }

    [Fact]
    public void Trace_cannot_be_null()
    {
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0);

        Assert.Throws<ArgumentNullException>(() => layer.RecordTrace(null!, T0));
    }

    [Fact]
    public void Recorded_trace_is_retained_for_audit()
    {
        var layer = NewLayer();
        var trace = SampleTrace();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0);
        layer.RecordTrace(trace, T0);

        Assert.Same(trace, layer.Trace);
        Assert.Equal(0.7, layer.Trace!.Confidence, precision: 10); // L7 min t-norm korunuyor
    }

    [Fact]
    public void Every_transition_is_recorded_as_audit_event()
    {
        // §5.4: "Her geçiş bir AuditEvent üretir" — karar bir satır değil, olay geçmişidir.
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0);
        layer.RecordTrace(SampleTrace(), T0);
        layer.RecordLearning(T0);
        layer.Remember(T0);

        // Planned→GateChecked→Contextualized→Acting→Observed→Traced→Learned→Remembered = 7 geçiş
        Assert.Equal(7, layer.History.Count);
        Assert.Equal(ActionState.Planned, layer.History[0].From);
        Assert.Equal(ActionState.Remembered, layer.History[^1].To);
    }

    [Fact]
    public void Terminal_state_rejects_all_further_transitions()
    {
        var layer = NewLayer();
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0);
        layer.RecordTrace(SampleTrace(), T0);
        layer.RecordLearning(T0);
        layer.Remember(T0);

        Assert.Throws<InvalidTransitionException>(() => layer.BeginActing(T0));
        Assert.Throws<InvalidTransitionException>(() => layer.Remember(T0));
    }

    [Fact]
    public void Gate_reason_is_carried_into_audit_history()
    {
        var layer = NewLayer();
        var gate = BlockingGate();
        layer.ApplyGate(gate, T0);

        Assert.Contains(layer.History, h => h.Note == gate.Reason);
    }

    [Fact]
    public void Null_gate_throws()
    {
        Assert.Throws<ArgumentNullException>(() => NewLayer().ApplyGate(null!, T0));
    }
}
