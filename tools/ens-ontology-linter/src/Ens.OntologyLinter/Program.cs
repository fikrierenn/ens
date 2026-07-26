using Ens.OntologyLinter;

// ens-ontology-linter — deterministic Ontology Linter (ENS G-09/10, formal-checker V1).
// Usage:
//   ens-ontology-linter [path-to-ENS-4010.md]
// Exit codes: 0 = zero violations (clean); 1 = violations found; 2 = tool/parse error.

try
{
    var path = args.Length > 0 ? args[0] : RepoLocator.DefaultCorpusPath();
    Console.WriteLine($"ENS Ontology Linter V1 — auditing: {path}");
    Console.WriteLine(new string('-', 72));

    var model = CorpusLoader.LoadFromFile(path);
    Console.WriteLine($"Parsed: {model.Nodes.Count} node types, {model.Relations.Count} relations, " +
                      $"{model.Profiles.Count} profiles.");
    Console.WriteLine($"Trans:✓ relations: " +
                      $"{string.Join(", ", model.Relations.Where(r => r.Transitive).Select(r => r.Name))}");
    Console.WriteLine();

    var violations = Invariants.CheckAll(model);
    if (violations.Count == 0)
    {
        Console.WriteLine("RESULT: 0 violations. Corpus is consistent under invariants A + B.");
        return 0;
    }

    Console.WriteLine($"RESULT: {violations.Count} violation(s):");
    Console.WriteLine();
    foreach (var group in violations.GroupBy(v => v.Kind))
    {
        Console.WriteLine($"[{group.Key}]");
        foreach (var v in group)
            Console.WriteLine($"  - ({v.Subject}) {v.Message}");
        Console.WriteLine();
    }
    return 1;
}
catch (Exception ex)
{
    Console.Error.WriteLine($"LINTER ERROR: {ex.Message}");
    return 2;
}
