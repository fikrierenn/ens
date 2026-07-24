namespace Ens.Kernel.Domain;

// TRACE: ENS-4001 §Computational primitifler (Event — tek gerçek runtime-atom)
// TRACE: ENS-4001 §Üç Aksiyom, Axiom 3 (Non-Leakage) — bir kez emit edilen Event asla değişmez.
//
// Event(id, emitter, target, timestamp, payload). `emitter` Identity'dir (ens-comp:emits);
// bir Event'in Principal'ı (P7 hesap-verebilirliği) sabit bir alan değil, ayrı bir
// `AccountableTo` ilişkisidir (ENS-4001 §Principal — rol, alt-tip değil) — bkz. DecisionEvents.
public abstract record DomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>Bu Event'i üreten Identity (ens-comp:emits). Sensör, ajan, insan olabilir.</summary>
    public required Identity Emitter { get; init; }

    /// <summary>Bu Event'in hakkında olduğu Identity (aggregate/varlık kimliği).</summary>
    public required Identity Target { get; init; }

    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
