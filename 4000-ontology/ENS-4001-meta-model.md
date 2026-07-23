---
id: ENS-4001
title: ENS Meta Model
type: ontology
canon: false
origin: ENS-0000 §IV, §VIII, §XI
depends_on: [ENS-0000, ENS-4000]
referenced_by: []
principles: [P1, P2]
status: ratified
owner: ens-philosopher
version: 0.3.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-023
maturity: M2
evidence: {sci: E1, eng: E0, ops: E0, econ: E0}
---

# ENS Meta Model

> ENS'in **kendi yapıtlarının** ontolojisidir (meta-seviye) = **Knowledge Graph şeması**
> (tipli düğümler + semantik kenarlar). Domain ontolojisiyle (kurumsal varlıklar) karıştırılmaz.
>
> **v0.2 notu:** [SKR-017](reviews/SKR-017-meta-model.md) Meta-Ontology Validation'a yanıt.
> Eksik node/edge eklendi; directionality, cardinality, temporal, identity tanımlandı;
> implements/realizes keskinleştirildi; **closure geçirildi**. §Yanıt tablosu sonda.

## Reference graph ≠ Knowledge graph
`depends_on` tipsiz **reference graph**'tir ("A, B'ye bağlı"). Knowledge graph **tipli
semantik** kenar ister ("Decision *implements* Principle"). Anlam kenardadır. `depends_on`
artık genel kenardan biridir; uygun tipli ilişki varsa o tercih edilir.

## Düğüm türleri (nodes)

| Tür | Anlamı | Nerede |
|-----|--------|--------|
| **Constitution** | En üst normatif konteyner (Principle içerir) | `0000-` |
| **Principle** | P1–P8 | Anayasa Madde III |
| **Philosophy** | Manifesto, ana tez | `1000-` |
| **Concept** | Decision, Context… (atomik fikir) | Theory içinde / sözlük |
| **Theory** | Bir kavramı geliştiren belge | `2000-` |
| **Law** | Bir davranış yasası | `3000-` |
| **Metric** | Tanımlı ölçü (ör. `H(A\|C)`) — Law ≠ Metric | `3000-` |
| **Ontology** | Domain/meta ontoloji | `4000-` |
| **Claim** | Bir belge içindeki önerme | Theory/Law içinde |
| **Hypothesis** | Kanıt bekleyen test edilebilir önerme | Theory / Reasoning |
| **Constraint** | Sınırlayıcı kural / invariant | Anayasa / Standard |
| **Evidence** | Kaynak / deney sonucu (E0-E4) | evidence-standard |
| **Experiment** | Faz 4 doğrulama | `7000-` |
| **ADR** | Mimari karar | `5000-adr/` |
| **RFC** | Öneri | `6000-` |
| **Module** | Kod modülü | `7000/8000-` |
| **BookChapter** | Kitap bölümü | `9000-` |
| **Agent / Command / Standard** | Operasyonel yapıtlar | `.claude/` |
| **SkepticReview** | Validation kaydı (SKR) | `reviews/` |
| **Index** | Manifest/kayıt (REGISTRY, KULLIYAT) | kök |

**Kapsam dışı (domain ontolojisi, bu meta-model değil):** Actor, Organization, Capability —
kurumsal varlıklardır; ENS-artifact meta-modeline değil, gelecek *domain ontolojisine* (4xxx)
aittir. Observation, Dataset → Faz 4'te Experiment ile gelir.

## Kenar türleri (semantic relations) — yön + cardinality

| İlişki | Domain → Range | Yön | Cardinality | Anlamı |
|--------|----------------|-----|-------------|--------|
| **implements** | ADR → Module | directed | 1:N | spec'i yürütmeye döker |
| **realizes** | Theory/Law → ADR | directed | N:N | soyutu tasarıma döker |
| **contains** | Constitution→Principle; Theory→Concept | directed | 1:N | kapsar |
| **derived_from** | Law → Principle; Concept → Concept | directed | N:N | türetilmiş |
| **specializes** | Concept → Concept | directed | N:1 | alt-tür (taksonomi) |
| **generalizes** | Concept → Concept | directed | 1:N | üst-tür (taksonomi) |
| **supports** | Evidence → Claim; Theory → Law | directed | N:N | destekler |
| **invalidates** | Evidence → Claim | directed | N:N | çürütür (Madde X) |
| **measures** | Metric → Concept | directed | N:1 | ölçer |
| **causes** | Concept/Decision → Outcome | directed | N:N | nedensel iddia (R2) |
| **requires** | * → * | directed | N:N | önkoşul (zorunlu) |
| **owned_by** | Decision/Artifact → Agent/Owner | directed | N:1 | sorumluluk (P7) |
| **challenges** | SkepticReview → Theory/Law | directed | N:1 | validation saldırısı |
| **produces** | Experiment → Evidence | directed | 1:N | üretir |
| **governs** | Standard → NodeType | directed | 1:N | yönetir |
| **contradicts** | Concept → Concept | **symmetric** | N:N | çelişir |
| **supersedes** | X → aynı tür X | directed (temporal) | 1:1 | yerine geçer |
| **version_of** | X → aynı tür X | directed (temporal) | N:1 | sürüm ilişkisi |
| **depends_on** | * → * | directed | N:N | genel yukarı-akış (tipli yoksa) |

## Semantic Connectors (v0.3 — OL-logic-1'i kapatır)
Kenarlar iki türdür:
```
Relation
├── Internal Relation   (aynı namespace içinde: serves, supports, implements…)
└── Semantic Connector  (namespace/katman arası — first-class, tip sistemli)
    ├── Bridge          (namespace ↔ namespace: meta ↔ core ↔ ent)
    ├── Realization     (soyut → somut: core → arch)   ← Faz 3'te kullanılır
    ├── Mapping         (reserved — RFC ile)
    └── Projection      (reserved — RFC ile)
```
**Neden:** Semantic Logic L6 "cross-namespace inference yalnızca bridge ile" der; IR-5 buna
dayanacak. Bridge tanımsızsa IR-5 tanımsız primitive'e dayanır (single-source + no-implicit-
semantics ihlali). Anti-ossification: bugün yalnızca **Bridge + Realization** tanımlı;
Mapping/Projection reserved.

### Semantic Connector tip sistemi
Her connector şunları taşır: `source-namespace · target-namespace · allowed-direction ·
multiplicity · semantic-preservation(bool) · traceability-required(bool)`. Böylece Linter
"tanımsız bridge kullanan inference geçersiz" diyebilir.

### Declared connectors
| Connector | Tür | Source → Target | Dir | Mult | Sem-preserve | Trace |
|-----------|-----|-----------------|-----|------|--------------|-------|
| **describes** | Bridge | ens-meta:Theory/Law → ens-core:Concept | meta→core | N:N | ✓ (betimleme içeriği değiştirmez) | zorunlu |
| **specializes*** | Bridge | ens-ent:* → ens-core:* | ent→core | N:1 | ✓ (alt-tür üst-türü korur) | zorunlu |
| **realized_by** | Realization | ens-core:Concept → ens-arch:Module/Aggregate | core→arch | 1:N | ✓ | zorunlu |

`*` Namespace-*içi* `specializes` bir Internal Relation'dır (taksonomi); yalnızca *cross-namespace*
(ent→core) hâli bir Bridge'dir.

### OA1 çözümü (measures namespace)
`measures` bir **Internal Relation'dır** (ens-core:Metric → ens-core:Concept/State) — meta↔core
köprüsü değil. Bir Law-doc (meta, ör. ENS-3021) ise bir Metric'i (core, `H(A|C)`) `describes`
**Bridge**'iyle betimler. Böylece SKR-021 OA1 kapanır: measures'ın range'i namespace-net
(ens-core), MC-002 (Purpose ölçülemez) core-internal bir kural, redundant değil.

## Temporal model
Bilgi yaşayan bir ağdır; zaman birinci-sınıftır. **Kenarlar geçerlilik zamanı taşır:**
`valid_from` / `valid_to`. Bir `supports` kenarı, çürütülünce (`invalidates`) kapanır
(`valid_to` atanır) — silinmez (audit). `supersedes` ve `version_of` doğası gereği zamansaldır.
Böylece "bugün desteklenen, yarın çürütülen" ifade edilebilir ve graf geçmişe sorgulanabilir.

## Identity kuralları
- **Node id değişmezdir** (immutable); bir kez atanan `ENS-Nxxx`/`SKR-NNN` asla değişmez ya da
  yeniden kullanılmaz (REGISTRY ile tutarlı).
- İçerik **`version_of`** ile sürümlenir (v0.1 → v0.2 aynı id, artan `version`).
- **`supersedes` id'yi korur:** eskisi `superseded` işaretlenir, id ve kayıt durur (audit).
- Bir node'un kimliği içeriğinden bağımsızdır (aynı id, evrilen içerik).

## implements vs realizes (semantik keskinleştirme)
- **realizes** — *soyuttan tasarıma*: bir Theory/Law, bir ADR ile mimariye dökülür. (Ne inşa
  edileceğinin tasarımı.)
- **implements** — *spec'ten yürütmeye*: bir ADR, bir Module ile koda dökülür. (Tasarımın
  çalışan hâli.)
İki farklı seviye (tasarım vs yürütme); artık örtüşmüyor.

## Closure — her yapıt bir node'a oturur
| Yapıt | Node türü |
|-------|-----------|
| ENS-0000 Anayasa | Constitution |
| P1–P8 | Principle |
| ENS-1000 Manifesto | Philosophy |
| ENS-2001 Decision Theory | Theory (contains Concept: Decision) |
| ENS-3021 Decision Entropy | Law + Metric (`H(A\|C)`) |
| ENS-4000 Sözlük / ENS-4001 Meta Model | Ontology |
| SKR-017 | SkepticReview |
| REGISTRY / KULLIYAT | Index |
| maturity-model | Standard |
Test geçer: depodaki her yapıt türü Meta Model'de tanımlıdır.

## Ontology Linter (formal-checker v1) — bunu mümkün kılar
Statik denetim (ispat değil): circular dependency (`derived_from`/`implements` çevrimi),
terminology conflict (sözlük-dışı Concept), orphan concept, undefined relation (tanımsız kenar),
recursive law (`measures` kendini), cardinality ihlali, symmetric/directed uyumsuzluğu.

## Ertelenen (Faz 3-4)
KG deposu + çıkarıcı + generatörler = implementation (Faz 4). **Şema (bu belge) Faz 1'dir** (P8).

## Failure conditions (Anayasa Madde X)
- **Aşırı-modelleme.** Zenginleşirse ossifiye olur; minimal + RFC-ile-genişletilebilir kalmalı.
- **Reference'a çöküş.** Ekip tipli ilişki yerine `depends_on`'a kaçarsa KG reference-graph'a
  düşer; linter zorlamalı.
- **Domain sızıntısı.** Actor/Organization gibi domain node'ları buraya sızarsa meta-seviye
  bozulur; sınır korunmalı.
- **M1'dir.** Reference platform'da yaşamadığından önerilmiş çekirdek; Faz 4 revize edebilir.

## SKR-017'ye yanıt
| Talep | Karşılandığı yer |
|-------|------------------|
| 1. Node ekle + domain havale | §Düğüm türleri (Metric, Hypothesis, Constraint, Philosophy, Index; domain kapsam-dışı) |
| 2. Edge ekle | §Kenar (specializes, generalizes, invalidates, requires, causes, owned_by, version_of) |
| 3. Directionality + cardinality | §Kenar tablosu (yön + cardinality sütunları) |
| 4. Temporal model | §Temporal model (valid_from/valid_to) |
| 5. Identity | §Identity kuralları |
| 6. implements vs realizes | §semantik keskinleştirme |
| 7. Closure | §Closure tablosu (geçer) |

---

*Meta Model, ENS'in kendi bilgisinin şemasıdır. Reference graph "neye bağlı", knowledge graph
"hangi anlamda bağlı" der. ENS'in konusu bilgi olduğuna göre, tipli-semantik-zamansal graf şarttır.*
