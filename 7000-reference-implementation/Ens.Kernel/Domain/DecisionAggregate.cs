using Ens.Kernel.Domain.Events;

namespace Ens.Kernel.Domain;

// TRACE: ENS-2001 (Decision Theory) — Decision Object + §Individuation + Lifecycle
// TRACE: ADR-0001 §5.4 (Action = Decision atomu, commitment-sealed granülerlik)
// TRACE: ENS-4025 §L8 (proof-trace invariant — izsiz commitment yasak, Anayasa Madde VI)
//
// Bu, ENS'in ilk çalışan Decision atomudur (Faz 4 reference implementation).
// Event-sourced: durum yalnızca Event akışının fold'udur (ENS-4001 §Axiom 2, Computational
// Closure). Aggregate kendi invariant'larını (§Individuation) korur; Bounded-Autonomy Gate
// (Policy/yetkilendirme) çağıran katmanın sorumluluğudur — aggregate onu varsaymaz, yalnızca
// commitment sırasını (Framed → Alternatives → Committed) zorlar.
public sealed class DecisionAggregate
{
    private readonly List<DomainEvent> _uncommitted = [];
    private readonly List<DomainEvent> _history = [];

    public Identity Id { get; private set; }
    public string? Purpose { get; private set; }
    public IReadOnlyList<string> Alternatives { get; private set; } = [];
    public double? Confidence { get; private set; }
    public bool IsCommitted { get; private set; }
    public bool IsEnacted { get; private set; }
    public bool HasOutcome { get; private set; }

    private DecisionAggregate(Identity id) => Id = id;

    /// <summary>Yeni bir Decision başlatır — Framing fazı. Henüz atom değil (deliberation).</summary>
    public static DecisionAggregate Frame(Identity emitter, string purpose)
    {
        var decision = new DecisionAggregate(Identity.New());
        decision.Raise(new DecisionFramed(purpose) { Emitter = emitter, Target = decision.Id });
        return decision;
    }

    /// <summary>Reasoning fazı: Alternatives + Evidence toplanır. Commitment için ön-koşul.</summary>
    public void IdentifyAlternatives(Identity emitter, IReadOnlyList<string> alternatives, IReadOnlyList<string> evidence)
    {
        if (IsCommitted)
            throw new InvalidOperationException("Commit-edilmiş bir Decision'a yeni Alternative eklenemez (ENS-2001 §Individuation: tek Commitment olayı).");
        if (alternatives.Count == 0)
            throw new ArgumentException("En az bir Alternative gerekli — karşı-olgusuz karar atom olamaz (ENS-2001 §Definition).");

        Raise(new AlternativesIdentified(alternatives, evidence) { Emitter = emitter, Target = Id });
    }

    /// <summary>
    /// ATOM SINIRI. §Individuation dört koşulu burada mühürlenir: tek Owner (bu çağrı),
    /// tek Purpose (Frame'de belirlendi), açık Alternatives (yukarıda), tek Commitment
    /// olayı (bu event — ikinci kez çağrılamaz).
    /// </summary>
    public void Commit(Identity owner, string selectedAlternative, double confidence, string expectedOutcome)
    {
        if (Purpose is null)
            throw new InvalidOperationException("Purpose olmadan commit edilemez (Framing fazı atlanmış).");
        if (Alternatives.Count == 0)
            throw new InvalidOperationException("Alternatives olmadan commit edilemez — deliberation tamamlanmamış.");
        if (IsCommitted)
            throw new InvalidOperationException("Decision zaten commit edildi — §Individuation ihlali: tek Commitment olayı.");
        if (!Alternatives.Contains(selectedAlternative))
            throw new ArgumentException("Seçilen Alternative, tanımlanan kümede değil.");
        if (confidence is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(confidence), "Confidence [0,1] aralığında olmalı (P6 kalibrasyon).");

        // TRACE: ENS-4025 §L8 — proof-trace burada doğar: confidence + expectedOutcome +
        // selectedAlternative, bu event'in kendisi "hangi kural/öncüllerle" sorusunun cevabıdır.
        Raise(new DecisionCommitted(selectedAlternative, owner, confidence, expectedOutcome) { Emitter = owner, Target = Id });
    }

    /// <summary>Enactment fazı — ADR-0001 Actuation Layer, ENS-2001 Enactment'i realizes eder.</summary>
    public void Enact(Identity emitter, string actionDescription)
    {
        if (!IsCommitted)
            throw new InvalidOperationException("Commit edilmemiş Decision enact edilemez.");
        if (IsEnacted)
            throw new InvalidOperationException("Decision zaten enact edildi.");

        Raise(new DecisionEnacted(actionDescription) { Emitter = emitter, Target = Id });
    }

    /// <summary>Measurement fazı (P4) — Actual Outcome, Learning'in girdisi.</summary>
    public void ObserveOutcome(Identity emitter, string actualOutcome)
    {
        if (!IsEnacted)
            throw new InvalidOperationException("Enact edilmemiş Decision'ın Outcome'u gözlenemez.");

        Raise(new OutcomeObserved(actualOutcome) { Emitter = emitter, Target = Id });
    }

    /// <summary>
    /// Learning fazı — ENS-2004 §1: learning_signal = Actual − Expected. Attribution
    /// seviyesi dürüstçe kaydedilir (L0-L3); sahte kesinlik iddia edilmez.
    /// </summary>
    public void RecordLearning(Identity emitter, string delta, AttributionLevel level, double attributionConfidence)
    {
        if (!HasOutcome)
            throw new InvalidOperationException("Outcome gözlenmeden Learning kaydedilemez.");

        Raise(new LearningRecorded(delta, level, attributionConfidence) { Emitter = emitter, Target = Id });
    }

    private void Raise(DomainEvent @event)
    {
        Apply(@event);
        _uncommitted.Add(@event);
        _history.Add(@event);
    }

    private void Apply(DomainEvent @event)
    {
        switch (@event)
        {
            case DecisionFramed e: Purpose = e.Purpose; break;
            case AlternativesIdentified e: Alternatives = e.Alternatives; break;
            case DecisionCommitted e: IsCommitted = true; Confidence = e.Confidence; break;
            case DecisionEnacted: IsEnacted = true; break;
            case OutcomeObserved: HasOutcome = true; break;
            case LearningRecorded: break; // Memory'ye yazma sorumluluğu ayrı (ENS-2003, Faz 4 sonraki adım)
        }
    }

    /// <summary>Event akışından yeniden inşa — Axiom 2 (Computational Closure)'nin uygulanışı.</summary>
    public static DecisionAggregate Rehydrate(Identity id, IEnumerable<DomainEvent> history)
    {
        var decision = new DecisionAggregate(id);
        foreach (var e in history)
        {
            decision.Apply(e);
            decision._history.Add(e);
        }
        return decision;
    }

    public IReadOnlyList<DomainEvent> UncommittedEvents => _uncommitted;
    public IReadOnlyList<DomainEvent> History => _history;
    public void ClearUncommitted() => _uncommitted.Clear();
}
