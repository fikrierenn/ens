using Ens.OntologyLinter;
using Xunit;
using Xunit.Abstractions;

namespace Ens.OntologyLinter.Tests;

/// <summary>
/// The two mandatory controls (see README §Proof):
///   POSITIVE — the real, current ENS-4010 corpus must yield ZERO violations.
///   NEGATIVE — the synthetic broken fixture must be caught (all three historical defect classes).
/// </summary>
public class ControlTests
{
    private readonly ITestOutputHelper _out;
    public ControlTests(ITestOutputHelper output) => _out = output;

    [Fact]
    public void PositiveControl_RealCorpus_HasZeroViolations()
    {
        var path = RepoLocator.DefaultCorpusPath();
        var model = CorpusLoader.LoadFromFile(path);
        var violations = Invariants.CheckAll(model);

        foreach (var v in violations)
            _out.WriteLine($"[{v.Kind}] ({v.Subject}) {v.Message}");

        // Sanity: the parser actually read the tables (guards against silently-empty parse).
        Assert.True(model.Relations.Count >= 20, $"Expected >=20 relations, got {model.Relations.Count}.");
        Assert.True(model.Nodes.Count >= 15, $"Expected >=15 node types, got {model.Nodes.Count}.");
        Assert.True(model.Profiles.Count >= 8, $"Expected >=8 profiles, got {model.Profiles.Count}.");
        Assert.Equal(2, model.Relations.Count(r => r.Transitive)); // part_of + specializes only

        Assert.Empty(violations);
    }

    [Fact]
    public void NegativeControl_BrokenFixture_CatchesAllThreeDefectClasses()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "fixtures", "broken-ontology.md");
        var model = CorpusLoader.LoadFromFile(path);
        var violations = Invariants.CheckAll(model);

        foreach (var v in violations)
            _out.WriteLine($"[{v.Kind}] ({v.Subject}) {v.Message}");

        // 1. Transitivity: part_of Actor->Organization + Trans:✓ (Kusur 3 / D-1).
        Assert.Contains(violations, v =>
            v.Kind == InvariantKind.TransitivityWellFormedness && v.Subject == "part_of");

        // 2. Profile satisfiability: Claim cannot satisfy supports|invalidates (Kusur 1).
        Assert.Contains(violations, v =>
            v.Kind == InvariantKind.ProfileSatisfiability && v.Subject == "Assertion/Claim");

        // 3. Unregistered relation: Rule requires derived_from, absent from Registry (Yara A-2).
        Assert.Contains(violations, v =>
            v.Kind == InvariantKind.UnregisteredRelationReference && v.Subject == "Rule/derived_from");

        // Exactly these three — no incidental noise (Evidence, Constraint, Actor, Organization are clean).
        Assert.Equal(3, violations.Count);
    }
}
