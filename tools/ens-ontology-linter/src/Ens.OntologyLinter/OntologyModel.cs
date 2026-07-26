namespace Ens.OntologyLinter;

/// <summary>
/// A Relation Registry row (ENS-4010 §Relation Registry). Domain/Range are the
/// enumerated node-type sets on each side of the relation.
/// </summary>
public sealed record Relation(
    string Name,
    IReadOnlyList<string> Domain,
    IReadOnlyList<string> Range,
    string? Inverse,
    bool Transitive);

/// <summary>A Node Registry row (ENS-4010 §Node Registry): node-type → its Semantic Profile.</summary>
public sealed record NodeType(string Name, string Profile);

/// <summary>
/// A Semantic Profiles row (ENS-4010 §Semantic Profiles). <see cref="RequirementsRaw"/> keeps the
/// original prose (backticks intact) because required relational edges are extracted from it.
/// </summary>
public sealed record ProfileDef(string Name, string RequirementsRaw, IReadOnlyList<string> Nodes);

/// <summary>
/// The parsed ontology corpus: the three machine-relevant tables of ENS-4010.
/// This is the *only* input the invariants operate on — nothing is trusted from the
/// document's own hand-authored "Invariant denetim tablosu" (which embeds conclusions,
/// not source of truth). The linter re-derives everything from the raw registries.
/// </summary>
public sealed class OntologyModel
{
    public required IReadOnlyList<Relation> Relations { get; init; }
    public required IReadOnlyList<NodeType> Nodes { get; init; }
    public required IReadOnlyList<ProfileDef> Profiles { get; init; }

    /// <summary>
    /// Meta-supertype tokens that stand for "any node type" in domain/range positions
    /// (e.g. `specializes: Node → Node`, `has_state: Entity → State`, `measures: Metric → Concept`).
    /// Treated as wildcards during set-membership checks.
    /// </summary>
    public static readonly IReadOnlySet<string> Wildcards =
        new HashSet<string>(StringComparer.Ordinal) { "Node", "Entity", "Concept" };

    public static bool IsWildcard(string token) => Wildcards.Contains(token);

    /// <summary>True if <paramref name="node"/> is a member of <paramref name="set"/>, honouring wildcards.</summary>
    public static bool NodeInSet(string node, IReadOnlyList<string> set)
        => set.Contains(node) || set.Any(IsWildcard);
}
