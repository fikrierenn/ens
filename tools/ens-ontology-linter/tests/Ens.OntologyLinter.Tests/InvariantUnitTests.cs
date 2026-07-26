using Ens.OntologyLinter;
using Xunit;

namespace Ens.OntologyLinter.Tests;

/// <summary>Focused unit tests for each invariant against tiny in-memory models.</summary>
public class InvariantUnitTests
{
    private static OntologyModel Model(
        IReadOnlyList<Relation> relations,
        IReadOnlyList<NodeType> nodes,
        IReadOnlyList<ProfileDef> profiles)
        => new() { Relations = relations, Nodes = nodes, Profiles = profiles };

    [Fact]
    public void Transitivity_flags_domain_not_equal_range_on_trans_true()
    {
        // Kusur 3 / D-1 shape: part_of Actor -> Organization, Trans:true.
        var m = Model(
            new[] { new Relation("part_of", new[] { "Actor" }, new[] { "Organization" }, "contains", true) },
            Array.Empty<NodeType>(),
            Array.Empty<ProfileDef>());

        var v = Invariants.CheckTransitivity(m);
        Assert.Single(v);
        Assert.Equal(InvariantKind.TransitivityWellFormedness, v[0].Kind);
    }

    [Fact]
    public void Transitivity_passes_when_range_subset_of_domain()
    {
        var m = Model(
            new[]
            {
                new Relation("part_of", new[] { "Actor", "Organization" }, new[] { "Organization" }, "contains", true),
                new Relation("specializes", new[] { "Node" }, new[] { "Node" }, "generalizes", true),
            },
            Array.Empty<NodeType>(),
            Array.Empty<ProfileDef>());

        Assert.Empty(Invariants.CheckTransitivity(m));
    }

    [Fact]
    public void ProfileSatisfiability_flags_node_with_no_satisfiable_edge()
    {
        // Kusur 1 shape: Claim requires supports|invalidates but is in neither's domain.
        var m = Model(
            new[]
            {
                new Relation("supports", new[] { "Evidence" }, new[] { "Claim" }, "supported_by", false),
                new Relation("invalidates", new[] { "Evidence" }, new[] { "Claim" }, "invalidated_by", false),
            },
            new[] { new NodeType("Claim", "Assertion"), new NodeType("Evidence", "Assertion") },
            new[] { new ProfileDef("Assertion", "`supports`|`invalidates`", new[] { "Evidence", "Claim" }) });

        var v = Invariants.CheckProfileSatisfiability(m);
        Assert.Contains(v, x => x.Kind == InvariantKind.ProfileSatisfiability && x.Subject == "Assertion/Claim");
        Assert.DoesNotContain(v, x => x.Subject == "Assertion/Evidence"); // Evidence IS in supports.domain
    }

    [Fact]
    public void ProfileSatisfiability_flags_unregistered_relation()
    {
        // Yara A-2 shape: profile requires derived_from, absent from the Registry.
        var m = Model(
            new[] { new Relation("constrains", new[] { "Constraint" }, new[] { "Decision" }, "constrained_by", false) },
            new[] { new NodeType("Constraint", "Rule") },
            new[] { new ProfileDef("Rule", "`constrains`, `derived_from`", new[] { "Constraint" }) });

        var v = Invariants.CheckProfileSatisfiability(m);
        Assert.Contains(v, x => x.Kind == InvariantKind.UnregisteredRelationReference && x.Subject == "Rule/derived_from");
        // Constraint is still satisfiable via `constrains`, so NO ProfileSatisfiability violation.
        Assert.DoesNotContain(v, x => x.Kind == InvariantKind.ProfileSatisfiability);
    }

    [Fact]
    public void ProfileSatisfiability_resolves_inverse_names_and_hyphens()
    {
        // `served-by` (prose, hyphen) must resolve to serves.inverse `served_by`; Purpose in serves.range.
        var m = Model(
            new[] { new Relation("serves", new[] { "Decision" }, new[] { "Purpose" }, "served_by", false) },
            new[] { new NodeType("Purpose", "Intent") },
            new[] { new ProfileDef("Intent", "Purpose → `served-by`", new[] { "Purpose" }) });

        Assert.Empty(Invariants.CheckProfileSatisfiability(m));
    }

    [Fact]
    public void ProfileSatisfiability_ignores_is_root_stoplist_token()
    {
        // `is_root` is an inferential predicate, not a Registry relation — must not be flagged.
        var m = Model(
            new[] { new Relation("pursues", new[] { "Actor", "Organization" }, new[] { "Goal" }, "pursued_by", false) },
            new[] { new NodeType("Organization", "Agent") },
            new[] { new ProfileDef("Agent", "Organization → `pursues`; `is_root` inferred", new[] { "Organization" }) });

        Assert.Empty(Invariants.CheckProfileSatisfiability(m));
    }
}
