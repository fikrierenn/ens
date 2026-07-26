# ens-ontology-linter (formal-checker V1)

Deterministic **Ontology Linter** for the ENS corpus. Closes freeze-fix backlog **G-09/10**
("Validation Generator + Ontology Linter (`formal-checker`) hiç yazılmadı").

## Why this is a tool, not an LLM agent (design decision — ens-architect)

Every real ontology defect found by hand this year — Kusur 1/2/3 (ENS-4010), D-1 (ENS-4025),
Yara A/B (ENS-4010 v0.5.0), B1 (ENS-4031) — was a violation of the **same two mechanical
invariants**, and each was found through expensive multi-round (sometimes 3-turn) skeptic-agent
loops. Those checks are pure structural set-membership tests over two markdown tables. Their value
is precisely that they are **cheap, repeatable, and regression-preventing** — the opposite of what
an LLM agent is good at. So `formal-checker` V1 is a deterministic .NET console tool, not an agent.

## Why it lives in `tools/`, not `7000-reference-implementation/`

`7000-reference-implementation/` is, by Anayasa Madde VII, **reference code that proves theory**
(`ContextScore` = ENS-2002's formula, `CompanyMemory` = ENS-2003's decay, etc.). This linter does
**not** prove any theory — it audits *corpus consistency*: it reads the ENS-4010 markdown and checks
its internal well-formedness. Putting it inside the kernel would be a concern-leak (the linter
operates **on** the corpus; it is not part of the corpus's proof). It therefore lives in a separate
top-level `tools/` directory. It **keeps the .NET/C# stack** (per `coding-standards.md`) for
toolchain consistency with `7000-`.

## What it checks (V1 scope — exactly two invariants)

Both are lifted verbatim from ENS-4010 §Relation Registry, which states them as the two
`formal-checker` invariants:

1. **Profile satisfiability** — every Semantic Profile's required relational edge must actually
   exist in the Relation Registry with a domain/range that admits the node type bearing that
   profile. A node whose profile requires an edge it can never form cannot produce a conformant
   instance. (Catches Kusur 1/2, Yara A-1.) A stronger sub-case: a profile requiring a relation
   that is **not registered at all** (Yara A-2, the `derived_from` case) is reported separately as
   `UnregisteredRelationReference`.
2. **Transitivity well-formedness** — for every relation marked `Trans: ✓`, `range ⊆ domain`
   (otherwise a 2-hop chain `A→B ∧ B→C` cannot be formed). (Catches Kusur 3; note D-1 was a
   *different* shape — the node/relation was absent from the Registry entirely, which the
   `UnregisteredRelationReference` variant also covers.)

The linter re-derives both **independently from the raw Node/Relation Registry tables**. It does
**not** read the document's own hand-authored "Invariant denetim tablosu" — that table embeds the
human's conclusions (`Sonuç: ✓`); trusting it would be a tautology. The point is to verify, not to
re-read the answer.

## How to run

Requires the .NET 10 SDK.

```sh
# Lint the real corpus (default target = 4000-ontology/ENS-4010-foundational-ontology.md)
dotnet run --project tools/ens-ontology-linter/src/Ens.OntologyLinter

# Lint a specific file
dotnet run --project tools/ens-ontology-linter/src/Ens.OntologyLinter -- path/to/ENS-4010.md

# Run the full test suite (unit + positive control + negative control)
dotnet test tools/ens-ontology-linter/Ens.OntologyLinter.slnx
```

Exit codes: `0` = zero violations, `1` = violations found (CI/regression-gate friendly), `2` = tool/parse error.

## Proof — positive and negative control

- **Positive control** (`ControlTests.PositiveControl_RealCorpus_HasZeroViolations`): runs against
  the current, clean ENS-4010 and asserts **zero violations** (plus a sanity check that the parser
  actually read ≥20 relations / ≥15 nodes / ≥8 profiles / exactly 2 `Trans:✓` relations, so an
  empty parse cannot masquerade as "clean"). This round already fixed every known defect
  (SKR-038/039/031/032 survive), so a non-zero result would mean either the tool is wrong or the
  corpus is still broken — both worth investigating.
- **Negative control** (`ControlTests.NegativeControl_BrokenFixture_CatchesAllThreeDefectClasses`):
  runs against `tests/.../fixtures/broken-ontology.md`, a small synthetic ontology (the real corpus
  is untouched) that deliberately re-introduces all three historical defect classes, and asserts the
  linter catches **exactly** them:
  1. `part_of: Actor → Organization` + `Trans:✓` → TransitivityWellFormedness (Kusur 3 / D-1).
  2. `Claim` cannot satisfy `supports|invalidates` → ProfileSatisfiability (Kusur 1).
  3. `Rule` requires `derived_from` absent from the Registry → UnregisteredRelationReference (Yara A-2).

> **Execution honesty (SKR-001 / SKR-041 precedent):** the tool + tests were authored and verified
> by careful manual trace of both controls against the actual table data, but **a live
> `dotnet build` / `dotnet test` was NOT run in the agent context that produced them** (no shell was
> enabled in that sandbox; terminal apps were typing-blocked). The green run must be confirmed by the
> owner / CI. **No build or test output has been fabricated.** See the ROADMAP G-09/10 entry.

## Failure conditions / honest limits (V1)

- **Markdown-table parsing is fragile.** The parser anchors tables by section heading and assumes
  the current ENS-4010 column layout. If a heading is renamed, a column reordered, or a table
  reformatted, the linter can **silently read the wrong data and report a false clean**. This is the
  single biggest risk. A future hardening step is a fixed machine-readable registry export.
- **Profile requirements are extracted from prose.** Only backtick-quoted `snake_case`/`hyphen-case`
  tokens are treated as relation candidates (so plain Decision-Object words — Evidence, Context,
  Alternatives, Outcome — are correctly ignored, per SKR-039 keskinleştirme #1). A small stop-list
  (`is_root`, `identity`, `lifecycle`, `decay`, `scarcity`, `timestamp`, `formula`) excludes
  structural predicates. Adding a new backticked non-relation predicate without updating the
  stop-list would cause a false `UnregisteredRelationReference`.
- **Profile satisfiability uses "at least one satisfiable required edge per node"**, matching the
  corpus's own "en az bir … bağ" profile shape. Pure **conjunctive** requirements (e.g. Deliberative
  = `serves` AND `constrained_by`) are therefore **under-checked**: a defect in one conjunct is
  missed if another conjunct is satisfiable. Tightening this to full conjunction/disjunction parsing
  is deferred to V2.
- **Only two invariants.** Deferred to **V2**: `depends_on`↔`referenced_by` back-link hygiene
  (**G-18**), node/edge completeness, cardinality, identity, and Semantic Closure reachability —
  the rest of `validation-framework.md`'s Ontology-dimension checklist. The `≥2 roots → warning`
  heuristic (ENS-4010 §Kök operasyonelleştirmesi) is also V2.
