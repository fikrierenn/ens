using System.Collections.ObjectModel;
using Ens.Kernel.Domain;

namespace Ens.Kernel;

// TRACE: ADR-0001 (accepted, v0.3.1) §5.4 "Action ve Decision atomu (P1) — granülerlik" +
//        action yaşam döngüsü state-machine'i (Planned→…→Remembered).
// TRACE: ADR-0001 §5.2 diyagramı "ACT[Action / Actuation Layer, guarded]" — kernel'in beşinci
//        ve son bileşeni. §6: "her Capability action'ı guarded koşar (otomatik yıkıcı edim yok)."
// TRACE: Anayasa P7 (bounded autonomy) — Gate BU KATMANIN ÜSTÜNDE; Acting'e ancak GateChecked
//        üzerinden geçilir; state machine bunu yapısal olarak zorlar.
// TRACE: ENS-2001 §Lifecycle (Enactment sonrası), ENS-2004 (Learned), ENS-2003 (Remembered).
//
// ADR-0001 §5.4'ün çekirdek iddiası: "Her geçiş bir AuditEvent üretir; hiçbir katman bu
// lifecycle'ı ATLAYAMAZ — CrewOps ProjectStateMachine'in InvalidTransitionException
// invariant'ının ENS karşılığı." Bu dosya o invariant'ı kodlar: geçersiz geçiş = exception,
// sessiz kabul YOK. "Karar bir satır değil, olay geçmişidir" iddiasının çalışma-zamanına inişi.
//
// ============================ DÜRÜST SINIRLAR (Faz-4) ============================
// (a) "GUARDED" BURADA YAPISAL, OPERASYONEL DEĞİL. Gerçek sandbox (izole workspace, dosya
//     sistemi/ağ kısıtı, timeout, kill), rollback/geri-alma, kaynak kotası YOK. Guard'ın
//     kodlanmış kısmı: (1) Gate'siz Acting'e geçilemez, (2) Blocked terminal, (3) Failed bile
//     Traced'ten geçmek ZORUNDA (izsiz başarısızlık yasak). Sandbox = ADR-0001 §6, ayrı iş.
// (b) Bu bir ÇALIŞTIRICI DEĞİL — action'ı fiilen koşan kod yok; katman yalnızca lifecycle'ı
//     ve invariant'larını yönetir (Scheduler'ın "sıralayıcı ama çalıştırıcı değil" sınırıyla
//     aynı disiplin). Gerçek yürütme Capability Runtime'ın (§6, kodlanmadı) işi.
// (c) AuditEvent akışı ayrı bir event-store'a YAZILMIYOR — geçiş geçmişi in-memory listede
//     tutulur. DecisionAggregate'in event-sourcing'i ile henüz birleştirilmedi.
// (d) Proof-trace ZORUNLU kılınır (Traced'e geçiş `ProofTrace` ister) ama trace'in İÇERİĞİNİ
//     bu katman üretmez — çağıran verir. Otomatik emit hâlâ açık (ProofTrace.cs'te de işaretli).
// (e) AÇIK KALAN (AUDIT §4.1, kapatılmadı): `ApplyGate` yalnızca "bir `GateResult` NESNESİ
//     verildi mi" diye bakar; o nesnenin gerçekten `BoundedAutonomyGate.Evaluate`'ten geldiğini
//     DOĞRULAYAMAZ (`GateResult` public bir record). Doğru ifade "Gate'siz action imkânsız"
//     değil, "GateResult nesnesiz action imkânsız"dır. Kapatılması imzalı/opak bir gate-token'ı
//     gerektirir — açık borç, README'de işaretli.
// ================================================================================
//
// DÜZELTME (AUDIT §5.2): `History` canlı bir `List<ActionTransition>` döndürüyordu ve
// `((List<ActionTransition>)layer.History).Clear()` ile tüm audit izi tek satırda (reflection
// GEREKMEDEN) silinebiliyordu — action "tamamlandı" görünürken geçmişi yok oluyordu. Artık
// değiştirilemez bir görünüm (`ReadOnlyCollection`) döner.

/// <summary>ADR-0001 §5.4 action yaşam döngüsü durumları (state diagram'ın birebir karşılığı).</summary>
public enum ActionState
{
    /// <summary>Decision commit edildi (ENS-2001), action planlandı.</summary>
    Planned,

    /// <summary>Bounded-Autonomy Gate (P7) değerlendirdi.</summary>
    GateChecked,

    /// <summary>Policy dışı — insana eskale edildi. TERMİNAL.</summary>
    Blocked,

    /// <summary>Policy içinde; context bağlandı, koşmaya hazır.</summary>
    Contextualized,

    /// <summary>Guarded action koşuyor.</summary>
    Acting,

    /// <summary>Actual Outcome toplandı (P4).</summary>
    Observed,

    /// <summary>Guarded stop / timeout — başarısız ama izlenmek ZORUNDA.</summary>
    Failed,

    /// <summary>Proof-Trace üretildi (P6 / ENS-4025 L8).</summary>
    Traced,

    /// <summary>Expected vs Actual değerlendirildi (ENS-2004).</summary>
    Learned,

    /// <summary>Company Memory'ye yazıldı (ENS-2003). TERMİNAL.</summary>
    Remembered
}

/// <summary>Lifecycle'ın geçersiz bir geçişle atlanmaya çalışılması (ADR-0001 §5.4 invariant).</summary>
public sealed class InvalidTransitionException(ActionState from, ActionState to)
    : InvalidOperationException(
        $"Geçersiz action geçişi: {from} → {to}. ADR-0001 §5.4: hiçbir katman lifecycle'ı atlayamaz.")
{
    public ActionState From { get; } = from;
    public ActionState To { get; } = to;
}

/// <summary>Her geçiş bir AuditEvent üretir (ADR-0001 §5.4) — olay geçmişi, tek satır değil.</summary>
public sealed record ActionTransition(ActionState From, ActionState To, DateTimeOffset At, string? Note = null);

/// <summary>
/// Actuation Layer (ADR-0001 §5.2 `ACT`, §5.4). Guarded action lifecycle'ını yönetir ve
/// geçiş invariant'larını yapısal olarak zorlar. Kernel'in beşinci bileşeni.
/// </summary>
public sealed class ActuationLayer
{
    private static readonly Dictionary<ActionState, ActionState[]> Allowed = new()
    {
        [ActionState.Planned] = [ActionState.GateChecked],
        [ActionState.GateChecked] = [ActionState.Blocked, ActionState.Contextualized],
        [ActionState.Contextualized] = [ActionState.Acting],
        [ActionState.Acting] = [ActionState.Observed, ActionState.Failed],
        [ActionState.Observed] = [ActionState.Traced],
        [ActionState.Failed] = [ActionState.Traced],   // izsiz başarısızlık YASAK
        [ActionState.Traced] = [ActionState.Learned],
        [ActionState.Learned] = [ActionState.Remembered],
        [ActionState.Blocked] = [],                     // terminal
        [ActionState.Remembered] = []                   // terminal
    };

    private readonly List<ActionTransition> _history = [];
    private readonly ReadOnlyCollection<ActionTransition> _historyView;

    public Identity DecisionId { get; }
    public ActionState State { get; private set; } = ActionState.Planned;

    /// <summary>
    /// Audit geçmişi — canlı ama DEĞİŞTİRİLEMEZ görünüm (AUDIT §5.2: downcast'le silme kapatıldı).
    /// "Her geçiş bir AuditEvent üretir" (ADR-0001 §5.4) artık bir slogan değil, bir invariant.
    /// </summary>
    public IReadOnlyList<ActionTransition> History => _historyView;

    public ProofTrace? Trace { get; private set; }

    public ActuationLayer(Identity decisionId)
    {
        DecisionId = decisionId;
        _historyView = new ReadOnlyCollection<ActionTransition>(_history);
    }

    /// <summary>Gate sonucunu uygular: sınır dışıysa Blocked (terminal), içindeyse Contextualized.</summary>
    public void ApplyGate(GateResult gate, DateTimeOffset at)
    {
        ArgumentNullException.ThrowIfNull(gate);
        Transition(ActionState.GateChecked, at, gate.Reason);

        bool allowed = gate.Decision is GateDecision.Autonomous or GateDecision.NotifyHuman;
        Transition(allowed ? ActionState.Contextualized : ActionState.Blocked, at, gate.Reason);
    }

    /// <summary>Guarded action başlar. Gate'ten geçmemişse state machine reddeder (P7 yapısal).</summary>
    public void BeginActing(DateTimeOffset at, string? note = null) => Transition(ActionState.Acting, at, note);

    /// <summary>Actual Outcome toplandı (P4, ENS-2004 §1 girdisi).</summary>
    public void Observe(DateTimeOffset at, string? actualOutcome = null) => Transition(ActionState.Observed, at, actualOutcome);

    /// <summary>Guarded stop / timeout. Başarısızlık da izlenmek zorunda — Traced'e geçmeden bitemez.</summary>
    public void Fail(DateTimeOffset at, string reason) => Transition(ActionState.Failed, at, reason);

    /// <summary>
    /// Proof-Trace üretildi (P6 / ENS-4025 L8). Trace ZORUNLU — null geçilemez; izsiz action
    /// bu katmanda temsil edilemez (Anayasa Madde VI, ProofTrace.cs'teki invariant'ın devamı).
    /// </summary>
    public void RecordTrace(ProofTrace trace, DateTimeOffset at)
    {
        ArgumentNullException.ThrowIfNull(trace);
        Transition(ActionState.Traced, at, trace.RuleId);
        Trace = trace;
    }

    /// <summary>Expected vs Actual değerlendirildi (ENS-2004).</summary>
    public void RecordLearning(DateTimeOffset at, string? delta = null) => Transition(ActionState.Learned, at, delta);

    /// <summary>Company Memory'ye yazıldı (ENS-2003). Terminal.</summary>
    public void Remember(DateTimeOffset at) => Transition(ActionState.Remembered, at);

    /// <summary>Terminal durumda mı (Blocked ya da Remembered)?</summary>
    public bool IsTerminal => Allowed[State].Length == 0;

    private void Transition(ActionState to, DateTimeOffset at, string? note = null)
    {
        if (!Allowed[State].Contains(to))
            throw new InvalidTransitionException(State, to);

        _history.Add(new ActionTransition(State, to, at, note));
        State = to;
    }
}
