using System.Text.RegularExpressions;

namespace Ens.OntologyLinter;

/// <summary>
/// Minimal, deliberately-narrow GitHub-flavoured-markdown table reader.
///
/// FRAGILITY (honest failure condition — see README §Limits): this parser assumes the ENS-4010
/// table shape stays stable. It anchors each table by its section heading, treats the first
/// following pipe-delimited block as the table, splits cells on unescaped '|' (respecting the
/// in-cell disjunction escape '\|'), and skips the '---' separator row. If a heading is renamed,
/// a column is reordered, or a table is reformatted, the linter can silently read the wrong data.
/// That risk is accepted for V1 and surfaced loudly rather than hidden.
/// </summary>
public static class MarkdownTableParser
{
    public sealed record Table(IReadOnlyList<string> Headers, IReadOnlyList<IReadOnlyList<string>> Rows);

    // Splits a table row into cells on '|' that is NOT escaped as '\|'.
    private static readonly Regex CellSplit = new(@"(?<!\\)\|", RegexOptions.Compiled);

    /// <summary>
    /// Finds the first markdown heading (line starting with '#') whose text contains
    /// <paramref name="headingSubstring"/>, then returns the first pipe-table beneath it.
    /// </summary>
    public static Table FindTable(string markdown, string headingSubstring)
    {
        var lines = markdown.Replace("\r\n", "\n").Split('\n');

        int headingIdx = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            var t = lines[i].TrimStart();
            if (t.StartsWith('#') && lines[i].Contains(headingSubstring, StringComparison.Ordinal))
            {
                headingIdx = i;
                break;
            }
        }
        if (headingIdx < 0)
            throw new InvalidOperationException($"Heading containing '{headingSubstring}' not found.");

        // Find the first table line (starts with '|') after the heading.
        int start = -1;
        for (int i = headingIdx + 1; i < lines.Length; i++)
        {
            var t = lines[i].TrimStart();
            if (t.StartsWith('|')) { start = i; break; }
            // Stop if we hit the next heading before any table.
            if (t.StartsWith('#') && i > headingIdx) break;
        }
        if (start < 0)
            throw new InvalidOperationException($"No table found under heading '{headingSubstring}'.");

        var block = new List<string>();
        for (int i = start; i < lines.Length; i++)
        {
            if (lines[i].TrimStart().StartsWith('|')) block.Add(lines[i]);
            else break;
        }

        var headers = SplitCells(block[0]);
        var rows = new List<IReadOnlyList<string>>();
        for (int i = 1; i < block.Count; i++)
        {
            var cells = SplitCells(block[i]);
            // Skip the '|---|---|' separator row.
            if (cells.All(c => c.Length == 0 || c.Trim('-', ':', ' ').Length == 0)) continue;
            rows.Add(cells);
        }
        return new Table(headers, rows);
    }

    /// <summary>Splits a single markdown row into trimmed cells, dropping the outer border cells.</summary>
    public static IReadOnlyList<string> SplitCells(string line)
    {
        var parts = CellSplit.Split(line).ToList();
        // Outer '|' produce leading/trailing empty fragments — drop them.
        if (parts.Count > 0 && parts[0].Trim().Length == 0) parts.RemoveAt(0);
        if (parts.Count > 0 && parts[^1].Trim().Length == 0) parts.RemoveAt(parts.Count - 1);
        // Unescape in-cell '\|' back to '|' now that splitting is done.
        return parts.Select(p => p.Replace("\\|", "|").Trim()).ToList();
    }

    /// <summary>Finds the index of the first header whose text contains <paramref name="needle"/>.</summary>
    public static int ColumnIndex(IReadOnlyList<string> headers, string needle)
    {
        for (int i = 0; i < headers.Count; i++)
            if (headers[i].Contains(needle, StringComparison.OrdinalIgnoreCase)) return i;
        throw new InvalidOperationException($"Column containing '{needle}' not found in [{string.Join(", ", headers)}].");
    }
}
