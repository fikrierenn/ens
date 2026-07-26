using System.Text.RegularExpressions;

namespace Ens.OntologyLinter;

public enum InvariantKind
{
    /// <summary>Invariant A: every `Trans: ✓` relation must satisfy range ⊆ domain (chainable 2-hop).</summary>
    TransitivityWellFormedness,

    /// <summary>Invariant B: every profile node must have at least one registry-satisfiable required edge.</summary>
    ProfileSatisfiability,

    /// <summary>Invariant B (variant): a profile references a relation absent from the Relation Registry.</summary>
    UnregisteredRelationReference,
}

public sealed record Violation(InvariantKind Kind, string Subject, string Message);

/// <summary>
/// The two core deterministic invariants derived from ENS-4010 §Relation Registry
/// ("Transitivity well-formedness" + "Profile satisfiability"). Historically every real
/// ontology defect found by hand (Kusur 1/2/3, D-1, Yara A-1/A-2) was a violation of exactly
/// one of these — this tool catches them instantly, for free, and prevents regression.
/// </summary>
public static class Invariants
{
    // Backtick-quoted identifier tokens are the only relation candidates. This deliberately
    // ignores plain-word Decision-Object components (Evidence, Context, Alternatives, Outcome)
    // that SKR-039 warned a naive checker might mistake for relations — they are never backticked.
    private static readonly Regex TokenRx = new(@"`([A-Za-z][\w\-]*)`", RegexOptions.Compiled);
    private static readonly Regex NodeArrow = new(@"(?:→|->)", RegexOptions.Compiled);

    // Backtick tokens that are structural predicates / shape markers, NOT Relation Registry
    // relations. `is_root` is inferential (ENS-4010 §Kök operasyonelleştirmesi). The rest are
    // node-shape fields that happen to appear in profile cells. Documented in README §Limits.
    private static readonly IReadOnlySet<string> NonRelationTokens =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "is_root", "identity", "lifecycle", "decay", "scarcity", "timestamp", "formula",
        };

    public static IReadOnlyList<Violation> CheckAll(OntologyModel model)
    {
        var v = new List<Violation>();
        v.AddRange(CheckTransitivity(model));
        v.AddRange(CheckProfileSatisfiability(model));
        return v;
    }

    // ---- Invariant A ---------------------------------------------------------------------

    public static IReadOnlyList<Violation> CheckTransitivity(OntologyModel model)
    {
        var result = new List<Violation>();
        foreach (var r in model.Relations.Where(r => r.Transitive))
        {
            var notCovered = r.Range.Where(x => !OntologyModel.NodeInSet(x, r.Domain)).ToList();
            if (notCovered.Count > 0)
            {
                result.Add(new Violation(
                    InvariantKind.TransitivityWellFormedness,
                    r.Name,
                    $"`{r.Name}` is Trans:✓ but range ⊄ domain: {{{string.Join(", ", r.Range)}}} " +
                    $"⊄ {{{string.Join(", ", r.Domain)}}} (unchainable: {string.Join(", ", notCovered)} " +
                    $"missing from domain). A 2-hop chain needs range ⊆ domain."));
            }
        }
        return result;
    }

    // ---- Invariant B ---------------------------------------------------------------------

    private enum Res { Satisfiable, NotSatisfiable, Unregistered }

    public static IReadOnlyList<Violation> CheckProfileSatisfiability(OntologyModel model)
    {
        var result = new List<Violation>();
        var reportedUnregistered = new HashSet<string>(StringComparer.Ordinal); // dedupe profile+token

        foreach (var profile in model.Profiles)
        {
            var tokensByNode = ExtractRequiredTokens(profile);
            foreach (var node in profile.Nodes)
            {
                var tokens = tokensByNode[node];
                if (tokens.Count == 0) continue; // purely-structural profile for this node

                var resolutions = tokens.Select(t => (t, res: Resolve(model, t, node))).ToList();

                // Variant: a required relation the Registry has never heard of (Yara A-2 / derived_from shape).
                foreach (var (t, res) in resolutions.Where(x => x.res == Res.Unregistered))
                {
                    var key = $"{profile.Name}::{t}";
                    if (reportedUnregistered.Add(key))
                        result.Add(new Violation(
                            InvariantKind.UnregisteredRelationReference,
                            $"{profile.Name}/{t}",
                            $"Profile `{profile.Name}` requires relation `{t}` but it is absent from the " +
                            $"Relation Registry (no relation named `{t}` and none whose inverse is `{t}`)."));
                }

                // Core: the node must have at least one satisfiable required edge (profiles are
                // "en az bir ... bağ" shaped). If none resolve to Satisfiable → non-conformant node.
                if (!resolutions.Any(x => x.res == Res.Satisfiable))
                {
                    result.Add(new Violation(
                        InvariantKind.ProfileSatisfiability,
                        $"{profile.Name}/{node}",
                        $"Node `{node}` (profile `{profile.Name}`) cannot satisfy any required relational " +
                        $"edge {{{string.Join(", ", tokens)}}}: none has `{node}` in its Registry domain " +
                        $"(outgoing) or range (incoming). This node type cannot produce a conformant instance."));
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Extracts, per node of a profile, the set of required relation tokens. Node-scoped clauses
    /// ("Goal → `refines` | `pursued_by`") bind tokens to that node; unscoped tokens apply to all
    /// nodes of the profile. Stop-list tokens (structural predicates) are dropped.
    /// </summary>
    private static Dictionary<string, List<string>> ExtractRequiredTokens(ProfileDef profile)
    {
        var byNode = profile.Nodes.ToDictionary(n => n, _ => new List<string>(), StringComparer.Ordinal);
        var unscoped = new List<string>();

        foreach (var segment in profile.RequirementsRaw.Split(';'))
        {
            string? scopedNode = null;
            int tokenStart = 0;
            foreach (var node in profile.Nodes)
            {
                var m = Regex.Match(segment, Regex.Escape(node) + @"\s*(?:→|->)");
                if (m.Success) { scopedNode = node; tokenStart = m.Index + m.Length; break; }
            }
            var text = scopedNode != null ? segment[tokenStart..] : segment;
            var tokens = ExtractTokens(text);
            if (scopedNode != null) byNode[scopedNode].AddRange(tokens);
            else unscoped.AddRange(tokens);
        }

        foreach (var node in profile.Nodes)
            byNode[node].AddRange(unscoped);
        return byNode;
    }

    private static IEnumerable<string> ExtractTokens(string text)
        => TokenRx.Matches(text)
            .Select(m => m.Groups[1].Value)
            .Where(t => !NonRelationTokens.Contains(t))
            .Distinct(StringComparer.Ordinal);

    /// <summary>
    /// Resolves a required token against the Relation Registry. Hyphens are normalised to
    /// underscores (`served-by` in prose vs `served_by` in the Registry inverse column).
    /// A token is Satisfiable if it names a forward relation with the node in its domain, or
    /// an inverse relation with the node in its range.
    /// </summary>
    private static Res Resolve(OntologyModel model, string token, string node)
    {
        var t = token.Replace('-', '_');

        var forward = model.Relations.FirstOrDefault(r => string.Equals(r.Name, t, StringComparison.OrdinalIgnoreCase));
        if (forward != null && OntologyModel.NodeInSet(node, forward.Domain)) return Res.Satisfiable;

        var inverse = model.Relations.FirstOrDefault(r => string.Equals(r.Inverse, t, StringComparison.OrdinalIgnoreCase));
        if (inverse != null && OntologyModel.NodeInSet(node, inverse.Range)) return Res.Satisfiable;

        if (forward != null || inverse != null) return Res.NotSatisfiable;
        return Res.Unregistered;
    }
}
