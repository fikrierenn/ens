---
id: ENS-4030
title: Semantic Axioms
type: ontology
canon: false
origin: ENS-4001, ENS-4010
depends_on: [ENS-0000, ENS-4001, ENS-4010]
referenced_by: []
principles: [P8]
status: ratified
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-021
maturity: M2
evidence: {sci: E2, eng: E0, ops: E0, econ: E0}
---

# ENS Semantic Axioms

> ENS'in **resmi semantik spesifikasyonu** — bir kural listesi değil. Ontoloji ve tip
> sisteminden (ENS-4010) türeyen; yapısal doğruluğu, anlamsal tutarlılığı ve güvenli evrimi
> tek yerde tanımlayan önermeler. **Validation Rules bu belgeden _üretilir_** (elle yazılmaz).
> `canon: false` (M1).

## "Axiom" ne demek (dürüstlük)
Burada Axiom = *sistemin sağlaması gereken resmi semantik önerme*. Bunların çoğu katı mantıksal
aksiyom değil, tip sisteminden **türetilir** (Structural) ya da anlam taahhüdüdür (Semantic) ya
da evrim invariant'ıdır (Evolution). "Axiom" adı, bunları ENS'in *doğruluk spesifikasyonu* olarak
işaretler — tek doğruluk kaynağı korunur.

## Constraint şeması (her axiom bunu taşır)
```
ID · Category · Severity(Error|Warning) · Source · Predicate · Failure Message · Auto-fix(opsiyonel)
```

## Katman 1 — Structural Constraints (SC-*)
Ontolojinin yapısal doğruluğu. **Profiles + Relation Registry'den (ENS-4010) sistematik türer** —
aşağıdakiler örnek değil, üretim kuralının çıktısıdır. Üretim kuralı:
> Her Semantic Profile'ın her *zorunlu* ilişkisi → bir `≥1`/`=1` Structural Constraint.
> Her kullanılan relation → "Relation Registry'de olmalı" constraint'i.

| ID | Severity | Predicate | Failure |
|----|----------|-----------|---------|
| SC-001 | Error | `Decision serves ≥1 Purpose` (Deliberative profili) | Decision'ın Purpose ilişkisi yok |
| SC-002 | Error | `Evidence supports\|invalidates ≥1 Claim` (Assertion) | Evidence hiçbir Claim'e bağlı değil |
| SC-003 | Error | `Constraint constrains ≥1 target` (Rule) | Constraint hiçbir şeyi sınırlamıyor |
| SC-004 | Error | `Theory derived_from ≥1 Principle` (meta) | Orphan Theory (Madde VIII) |
| SC-005 | Error | `∀ edge: relation ∈ Relation Registry` | Tanımsız relation (undefined-relation) |
| SC-006 | Error | `∀ node: conforms to its Semantic Profile` | Profil ihlali (eksik zorunlu alan/ilişki) |
| SC-007 | Error | `∀ node: SemanticClosure ≠ ∅` (≥1 Principle) | Orphan concept (kapanış boş) |
| SC-… | — | *(üretilen)* profile-başına zorunlu ilişkiler | — |

## Katman 2 — Semantic Constraints (MC-*)
Anlamı korur; tip sistemi + Allowed/Discouraged/Forbidden'dan türer.

| ID | Severity | Predicate | Failure |
|----|----------|-----------|---------|
| MC-001 | Error | `¬(Metric implements Decision)` (Forbidden) | Seviye/tür ihlali |
| MC-002 | Error | `¬(Metric measures Purpose)` — Purpose doğrudan ölçülemez | Purpose bir Metric'in range'i |
| MC-003 | Error | `¬(Constraint owns Capability)` | Kural sahiplenmez |
| MC-004 | Error | `¬(Evidence support AND invalidate same Claim, same context, **same time**)` | Eşzamanlı çelişki |
| MC-005 | Error | `¬(Claim supports itself)` — döngüsel gerekçe | Self-support |
| MC-006 | Error | `¬(Decision serves Decision)` | Karar Purpose'a hizmet eder |
| MC-007 | Error | `¬(ens-meta:* relation ens-core:*)` çapraz-seviye | Namespace ihlali |
| MC-008 | Warning | `Metric supports Decision` = **Discouraged** | Zayıf anlam (Linter uyarır) |

> **Temporal not (Meta Model uyumu):** MC-004, Meta Model'in valid_from/valid_to'suyla
> uyumludur: bir Evidence bir Claim'i *önce* support edip *sonra* invalidate edebilir (zamanla).
> Çelişki yalnızca **aynı anda** support+invalidate ise doğar — MC-004 buna kapsanmıştır.

## Katman 3 — Evolution Constraints (EC-*)
ENS uzun ömürlü; değişim de semantik. Governance + Versioning ile konuşur.

| ID | Severity | Predicate | Failure |
|----|----------|-----------|---------|
| EC-001 | Error | `Canonical(M5) node cannot be deleted` | M5 silme girişimi |
| EC-002 | Error | `deprecated node → supersedes bir successor` | Halefsiz deprecation |
| EC-003 | Error | `supersedes traceability korur` (successor ≥ aynı Principle kapanışı) | Kapanış kaybı |
| EC-004 | Error | `version_of ∪ supersedes acyclic` (DAG) | Sürüm çevrimi |
| EC-005 | Error | `SemanticClosure explicit deprecation olmadan azalamaz` | Sessiz orphan üretimi |
| EC-006 | Error | `node id immutable` (Meta Model identity) | id değişimi/yeniden kullanımı |

EC-003 ve EC-005 **kanıt-zinciri/governance ile** birlikte çalışır: bir yapıt supersede
edilirken, halefi en az aynı Principle'lara ulaşmalı — aksi hâlde bilgi *sessizce* kaybolur.

## Türetme: Validation Rules elle yazılmaz
```
Relation Registry + Semantic Profiles + Semantic Axioms (SC/MC/EC)
        ↓  (generate)
Validation Rules  →  Ontology Linter (formal-checker)
```
Validator davranışı deklaratif tanımlardan üretilir; tek doğruluk kaynağı korunur (kural elle
yazılırsa ayrışır).

## Inference Rules ≠ Semantic Axioms (ayrım — sonraki belge ENS-4031)
- **Axiom (bu belge):** *ne yasak / ne zorunlu.* Doğruluk.
- **Inference (ENS-4031):** *ne yeni bilgi türetilebilir.* (Relation Composition'dan:
  `A serves B ∧ B supports C ⇒ A indirectly_supported_by C`.)
Zincir: `Ontology → Semantic Axioms → Inference Rules → Validation Rules → Architecture`.
Ayrım, ileride reasoning motoru için kritik (constraint checker ≠ inference engine).

## Failure conditions (Anayasa Madde X)
- **Structural tamlık.** SC katmanı Profiles'tan üretilir; ama üretimin *tam* olduğu ancak
  formal-checker (Faz 4) profilleri gezince kanıtlanır — şimdilik üretim kuralı tanımlı, çıktı elle.
- **Predicate dili.** Predicate'ler yarı-formal (Türkçe+notasyon); makine-çalıştırılabilir bir
  formal dil (ör. SHACL/SPARQL/Datalog) Faz 4'te gerekir. Şimdilik insan+niyet.
- **Auto-fix riski.** Çoğu MC/EC için auto-fix **yok** — semantik hatayı otomatik düzeltmek onu
  maskeler. Auto-fix yalnızca güvenli structural durumlarda (ör. eksik künye alanı).
- **EC runtime enforcement.** Evolution constraint'leri statik değil; Governance/Versioning
  işlemlerinde (supersede/deprecate) çalışmalı — bu bağ Governance tarafında kurulmalı.
- **Consistency.** Axiom'lar birbirini çürütmemeli; consistency-check (bir axiom başka birini
  imkânsız kılıyor mu?) formal-checker işi (Faz 4).

---

*Semantic Axioms, ENS'in doğruluk sözleşmesidir: yapı (SC), anlam (MC) ve evrim (EC). Validation
bundan türer, Inference bunun yanında durur, Architecture bunun üstüne — makine-doğrulanabilir
tek bir semantik spesifikasyondan.*
