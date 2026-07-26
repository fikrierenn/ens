using Ens.OntologyLinter;
using Xunit;

namespace Ens.OntologyLinter.Tests;

public class MarkdownTableParserTests
{
    [Fact]
    public void SplitCells_respects_escaped_pipe_as_disjunction()
    {
        var cells = MarkdownTableParser.SplitCells(@"| Assertion | `supports`\|`invalidates` | Evidence, Claim |");
        Assert.Equal(3, cells.Count);
        Assert.Equal("Assertion", cells[0]);
        Assert.Equal("`supports`|`invalidates`", cells[1]); // '\|' unescaped back to '|'
        Assert.Equal("Evidence, Claim", cells[2]);
    }

    [Fact]
    public void FindTable_anchors_on_heading_and_reads_first_table()
    {
        const string md = """
            ## Node Registry
            | Node | Profile | Definition |
            |------|---------|------------|
            | Decision | Deliberative | x |

            ## Relation Registry
            | Relation | Domain → Range | Trans |
            |----------|----------------|-------|
            | serves | Decision → Purpose | ✗ |
            """;
        var t = MarkdownTableParser.FindTable(md, "Relation Registry");
        Assert.Equal(3, t.Headers.Count);
        Assert.Single(t.Rows);
        Assert.Equal("serves", t.Rows[0][0]);
    }

    [Fact]
    public void ParseDomainRange_splits_alternatives_and_strips_parentheticals()
    {
        var (domain, range) = CorpusLoader.ParseDomainRange("Capability/Evidence/Claim → Purpose/Claim");
        Assert.Equal(new[] { "Capability", "Evidence", "Claim" }, domain);
        Assert.Equal(new[] { "Purpose", "Claim" }, range);

        var (d2, r2) = CorpusLoader.ParseDomainRange("Node → Node (aynı tür)");
        Assert.Equal(new[] { "Node" }, d2);
        Assert.Equal(new[] { "Node" }, r2);
    }
}
