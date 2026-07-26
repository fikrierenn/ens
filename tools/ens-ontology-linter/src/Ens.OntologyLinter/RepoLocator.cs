namespace Ens.OntologyLinter;

/// <summary>Locates the ENS repo root and the default corpus file by walking up from a start directory.</summary>
public static class RepoLocator
{
    public const string DefaultCorpusRelative = "4000-ontology/ENS-4010-foundational-ontology.md";

    public static string FindRepoRoot(string? startDir = null)
    {
        var dir = new DirectoryInfo(startDir ?? AppContext.BaseDirectory);
        while (dir != null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "4000-ontology")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new DirectoryNotFoundException(
            "Could not locate ENS repo root (no '4000-ontology' directory found walking up from " +
            $"'{startDir ?? AppContext.BaseDirectory}').");
    }

    public static string DefaultCorpusPath(string? startDir = null)
        => Path.Combine(FindRepoRoot(startDir), DefaultCorpusRelative.Replace('/', Path.DirectorySeparatorChar));
}
