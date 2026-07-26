using Ens.Kernel.Domain;
using Ens.Kernel.Domain.Events;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-2001 §Individuation — bu testler ENS-2001'in dört koşulunu (tek Owner, tek
// Purpose, açık Alternatives, tek Commitment olayı) kodda DOĞRULAR. Bu, ENS-2001'in ilk
// Engineering-boyutlu kanıtıdır (evidence-standard.md: eng E0 → E1).

public sealed class DecisionAggregateTests
{
    private static readonly Identity Owner = Identity.New();
    private static readonly Identity System = Identity.New();

    [Fact]
    public void Frame_creates_uncommitted_decision_with_purpose()
    {
        var decision = DecisionAggregate.Frame(System, "Stockout'u önle");

        Assert.Equal("Stockout'u önle", decision.Purpose);
        Assert.False(decision.IsCommitted);
        Assert.Single(decision.UncommittedEvents);
    }

    [Fact]
    public void Commit_without_alternatives_throws()
    {
        // TRACE: ENS-2001 §Definition — "karşı-olgusuz karar atom olamaz"
        var decision = DecisionAggregate.Frame(System, "Test");

        Assert.Throws<InvalidOperationException>(() =>
            decision.Commit(Owner, "X", 0.7, "beklenen"));
    }

    [Fact]
    public void Cannot_commit_twice()
    {
        // TRACE: ENS-2001 §Individuation koşul 4 — "tek Commitment olayı"
        var decision = DecisionAggregate.Frame(System, "Fiyat kararı");
        decision.IdentifyAlternatives(System, ["%5 zam", "%0", "%8 zam"], [new Premise("elastikiyet-verisi", 0.8)]);
        decision.Commit(Owner, "%5 zam", 0.7, "hacim -%3");

        Assert.Throws<InvalidOperationException>(() =>
            decision.Commit(Owner, "%0", 0.5, "hacim 0"));
    }

    [Fact]
    public void Cannot_add_alternatives_after_commitment()
    {
        // TRACE: ENS-2001 §Individuation — commitment sonrası deliberation kapanır (atom mühürlendi)
        var decision = DecisionAggregate.Frame(System, "Test");
        decision.IdentifyAlternatives(System, ["A", "B"], [new Premise("kanit", 1.0)]);
        decision.Commit(Owner, "A", 0.6, "beklenen");

        Assert.Throws<InvalidOperationException>(() =>
            decision.IdentifyAlternatives(System, ["C"], []));
    }

    [Fact]
    public void Confidence_out_of_range_throws()
    {
        // TRACE: Anayasa P6 — kalibre confidence [0,1] aralığında olmalı
        var decision = DecisionAggregate.Frame(System, "Test");
        decision.IdentifyAlternatives(System, ["A"], [new Premise("kanit", 1.0)]);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            decision.Commit(Owner, "A", 1.5, "beklenen"));
    }

    [Fact]
    public void Full_lifecycle_produces_ordered_event_stream()
    {
        // TRACE: ENS-2001 §Decision Lifecycle — Framing→Reasoning→Commitment→Enactment→
        // Measurement→Learning. Axiom 2 (Computational Closure): Decision = fold(Events).
        var decision = DecisionAggregate.Frame(System, "Tedarikçi seçimi");
        decision.IdentifyAlternatives(System, ["Tedarikçi-A", "Tedarikçi-B"], [new Premise("skorkart", 0.9)]);
        decision.Commit(Owner, "Tedarikçi-A", 0.8, "leadtime 5 gün");
        decision.Enact(System, "PO-42 oluşturuldu");
        decision.ObserveOutcome(System, "leadtime 6 gün gerçekleşti");
        decision.RecordLearning(System, "Δ=+1 gün", AttributionLevel.L1_ModelBased, 0.6);

        Assert.Equal(6, decision.History.Count);
        Assert.IsType<DecisionFramed>(decision.History[0]);
        Assert.IsType<LearningRecorded>(decision.History[^1]);
        Assert.True(decision.IsCommitted);
        Assert.True(decision.IsEnacted);
        Assert.True(decision.HasOutcome);
    }

    [Fact]
    public void Rehydrate_reconstructs_identical_state_from_event_stream()
    {
        // TRACE: ENS-4001 §Axiom 2 (Computational Closure) — State tamamen Event'ten türetilebilir.
        var original = DecisionAggregate.Frame(System, "Test rehydrate");
        original.IdentifyAlternatives(System, ["A", "B"], [new Premise("kanit", 1.0)]);
        original.Commit(Owner, "A", 0.5, "beklenen");

        var rehydrated = DecisionAggregate.Rehydrate(original.Id, original.History);

        Assert.Equal(original.Purpose, rehydrated.Purpose);
        Assert.Equal(original.IsCommitted, rehydrated.IsCommitted);
        Assert.Equal(original.Alternatives, rehydrated.Alternatives);
    }

    [Fact]
    public void Every_event_carries_emitter_for_accountability()
    {
        // TRACE: ENS-4001 §Principal — Event her zaman bir Emitter taşır (accountable_to
        // ilişkisinin ön-koşulu); kaynaksız Event olamaz.
        var decision = DecisionAggregate.Frame(System, "Test");

        Assert.All(decision.History, e => Assert.NotEqual(default, e.Emitter));
    }
}
