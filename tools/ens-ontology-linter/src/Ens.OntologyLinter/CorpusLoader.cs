using System.Text.RegularExpressions;

namespace Ens.OntologyLinter;

/// <summary>
/// Loads an <see cref="OntologyModel"/> from an ENS-4010-shaped markdown document by reading the
/// Node Registry, Semantic Profiles, and Relation Registry tables. Works equally on the real
/// corpus and on synthetic test fixtures (they share the same table shape).
/// </summary>
public static class CorpusLoader
{
    private static readonly Regex Parenthetical = new(@"\([^)]*\)", RegexOptions.Compiled);

    public static OntologyModel LoadFromFile(string path)
        => LoadFromMarkdown(File.ReadAllText(path));

    public static OntologyModel LoadFromMarkdown(string md)
        => new()
        {
            Relations = LoadRelations(md),
            Nodes = LoadNodes(md),
            Profiles = LoadProfiles(md),
        };

    private static IReadOnlyList<Relation> LoadRelations(string md)
    {
        var table = MarkdownTableParser.FindTable(md, "Relation Registry");
        int iName = MarkdownTableParser.ColumnIndex(table.Headers, "Relation");
        int iDomainRange = MarkdownTableParser.ColumnIndex(table.Headers, "Domain");
        int iInverse = MarkdownTableParser.ColumnIndex(table.Headers, "Inverse");
        int iTrans = MarkdownTableParser.ColumnIndex(table.Headers, "Trans");

        var result = new List<Relation>();
        foreach (var row in table.Rows)
        {
            var name = Clean(row[iName]);
            if (name.Length == 0) continue;
            var (domain, range) = ParseDomainRange(row[iDomainRange]);
            var inverse = Clean(row[iInverse]);
            var transitive = row[iTrans].Contains('✓'); // '✓'
            result.Add(new Relation(name, domain, range, inverse.Length == 0 ? null : inverse, transitive));
        }
        return result;
    }

    private static IReadOnlyList<NodeType> LoadNodes(string md)
    {
        var table = MarkdownTableParser.FindTable(md, "Node Registry");
        int iNode = MarkdownTableParser.ColumnIndex(table.Headers, "Node");
        int iProfile = MarkdownTableParser.ColumnIndex(table.Headers, "Profile");

        var result = new List<NodeType>();
        foreach (var row in table.Rows)
        {
            var node = Clean(row[iNode]);
            if (node.Length == 0) continue;
            result.Add(new NodeType(node, Clean(row[iProfile])));
        }
        return result;
    }

    private static IReadOnlyList<ProfileDef> LoadProfiles(string md)
    {
        var table = MarkdownTableParser.FindTable(md, "Semantic Profiles");
        int iProfile = MarkdownTableParser.ColumnIndex(table.Headers, "Profile");
        int iReq = MarkdownTableParser.ColumnIndex(table.Headers, "Zorunlu");
        int iNodes = MarkdownTableParser.ColumnIndex(table.Headers, "Nodes");

        var result = new List<ProfileDef>();
        foreach (var row in table.Rows)
        {
            var name = Clean(row[iProfile]);
            if (name.Length == 0) continue;
            var nodes = row[iNodes]
                .Split(',')
                .Select(Clean)
                .Where(s => s.Length > 0)
                .ToList();
            // Requirements column keeps backticks (tokens are extracted from them); only strip bold.
            var requirements = row[iReq].Replace("**", "");
            result.Add(new ProfileDef(name, requirements, nodes));
        }
        return result;
    }

    /// <summary>Parses a "Domain → Range" cell into two node-type sets, splitting alternatives on '/'.</summary>
    public static (IReadOnlyList<string> Domain, IReadOnlyList<string> Range) ParseDomainRange(string cell)
    {
        var stripped = Parenthetical.Replace(cell.Replace("`", "").Replace("**", ""), "");
        var parts = stripped.Split('→'); // '→'
        if (parts.Length < 2)
            return (Array.Empty<string>(), Array.Empty<string>());
        return (SplitSet(parts[0]), SplitSet(parts[1]));
    }

    private static IReadOnlyList<string> SplitSet(string s)
        => s.Split('/').Select(x => x.Trim()).Where(x => x.Length > 0).ToList();

    private static string Clean(string s)
        => s.Replace("`", "").Replace("**", "").Replace("*", "").Trim();
}
