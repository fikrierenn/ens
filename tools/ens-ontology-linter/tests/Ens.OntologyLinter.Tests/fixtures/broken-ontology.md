# SYNTHETIC BROKEN ONTOLOGY — negative-control fixture (NOT part of the corpus)

> This file exists ONLY to prove the linter actually catches the historical defect classes.
> It intentionally re-introduces three known past defects, one per invariant/variant:
>   1. Transitivity: `part_of: Actor → Organization` marked `Trans: ✓` (Kusur 3 / D-1 shape) —
>      range {Organization} ⊄ domain {Actor}, so a 2-hop chain cannot be formed.
>   2. Profile satisfiability: the Assertion profile requires `supports`|`invalidates`, but the
>      `supports`/`invalidates` domain is {Evidence} only, so `Claim` cannot satisfy it (Kusur 1).
>   3. Unregistered relation: the Rule profile requires `derived_from`, which is absent from the
>      Relation Registry (Yara A-2 shape).
> The real ENS-4010 corpus fixed all three; this fixture keeps them broken on purpose.

## Node Registry
| Node | Profile | Definition |
|------|---------|------------|
| Evidence | Assertion | supporting information |
| Claim | Assertion | a proposition in reasoning |
| Constraint | Rule | a limiting rule |
| Actor | Agent | a decision maker |
| Organization | Agent | structured whole of actors |

## Semantic Profiles
| Profile | Zorunlu | Nodes |
|---------|---------|-------|
| **Assertion** | Identity + `supports`\|`invalidates` | Evidence, Claim |
| **Rule** | Identity, `constrains`≥1, `derived_from` | Constraint |
| **Agent** | Identity, `pursues` | Actor, Organization |

## Relation Registry
| Relation | Domain → Range | Dir | Card | Inverse | Trans | Sym | Default |
|----------|----------------|-----|------|---------|-------|-----|---------|
| supports | Evidence → Claim | → | N:N | supported_by | ✗ | ✗ | Allowed |
| invalidates | Evidence → Claim | → | N:N | invalidated_by | ✗ | ✗ | Allowed |
| constrains | Constraint → Decision | → | N:N | constrained_by | ✗ | ✗ | Allowed |
| pursues | Actor/Organization → Goal | → | N:N | pursued_by | ✗ | ✗ | Allowed |
| part_of | Actor → Organization | → | N:1 | contains | **✓** | ✗ | Allowed |
