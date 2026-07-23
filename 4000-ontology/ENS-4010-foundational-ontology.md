---
id: ENS-4010
title: Foundational Ontology (Semantic Type System)
type: ontology
canon: false
origin: ENS-0000 §IV, ENS-4001, ENS-2001, ENS-2002, ENS-2003, ENS-2004
depends_on: [ENS-0000, ENS-4001, ENS-2001, ENS-2002, ENS-2003, ENS-2004]
referenced_by: []
principles: [P1, P2, P3, P4, P5]
status: ratified
owner: ens-philosopher
version: 0.2.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-020
maturity: M2
evidence: {sci: E3, eng: E0, ops: E0, econ: E0}
---

# ENS Foundational Ontology — Semantic Type System

> ENS'in **semantik çekirdeği** ve **tip sistemi**. Kurumsal bilişin (şirketten bağımsız)
> node ve relation'larını, *makine-doğrulanabilir* biçimde tanımlar. `canon: false` (M1).
>
> **v0.2 notu:** [SKR-019](reviews/SKR-019-foundational-ontology.md)'un 5 bulgusu tek tek
> yamanmadı; kök neden çözüldü — ontoloji bir **tip sistemine** dönüştü (Node Registry,
> Relation Registry, Semantic Profiles, Namespace, Composition, Semantic Closure). §Yanıt sonda.

## Prior art (dürüst konumlandırma)
Bu tip sistemi özgün değildir: **OWL** (domain/range/transitive/symmetric/inverse = object
property characteristics), **RDFS** (namespace, subClassOf = `specializes`), **SHACL**
(Semantic Profiles = shapes), **description logic**. ENS bunları *icat etmez, uygular.* Dar
delta: minimal, Meta Model'e (ENS-4001) bağlı ve **Principle'a kadar Semantic Closure** — bu
sonuncusu ENS'e özgüdür (traceability'yi ontolojiye gömer).

## Seviye ve namespace
- **`ens-meta:`** — Meta Model yapıt türleri (Theory, SKR, ADR, `ens-meta:Claim`).
- **`ens-core:`** — Foundational Ontology (bu belge; `ens-core:Decision`, `ens-core:Claim`).
- **`ens-ent:`** — Enterprise Ontology (domain; `ens-ent:Invoice`), Foundational'ı specialize eder.

**Homonim çözümü (SKR-019 Bulgu 4):** `ens-meta:Claim` (ENS teorisindeki önerme) ≠
`ens-core:Claim` (kurumsal karar gerekçesi). Namespace ile kesin ayrılır; Linter iki seviyeyi
karıştıramaz.

## İki katman
`Foundational Ontology` (bu belge, `ens-core:`) + `Enterprise Ontology` (ENS-4020, `ens-ent:`).
ENS şirket ontolojisini değil, **kurumsal düşünmenin ontolojisini** tanımlar.

---

## Node Registry (`ens-core:`)
Her node tek kimlik + bir Semantic Profile taşır.

| Node | Profile | Definition (özet) |
|------|---------|-------------------|
| Decision | Deliberative | Commitment ile mühürlenen taahhüt (ENS-2001) |
| Purpose | Intent | Hizmet edilen niyet (*neden*) |
| Goal | Intent | Ölçülebilir arzu edilen durum |
| Constraint | Rule | Kararları sınırlayan kural/invariant |
| Context | Temporal | Kararı anlamlı kılan ilgili durum (ENS-2002) |
| Memory | Store | *Neden*'in kalıcı deposu (ENS-2003) |
| Learning | Assertion | Expected/Actual atfedilmiş farkı (ENS-2004) |
| Evidence | Assertion | Bir `ens-core:Claim`'i destekleyen bilgi |
| Claim | Assertion | Karar gerekçesindeki önerme |
| Actor | Agent | Karar veren/sorumlu insan/ajan (P7) |
| Organization | Agent | Actor'ların yapılandırılmış bütünü |
| Capability | Resource | Amaca hizmet edebilen örgütsel yeti |
| Resource | Resource | Kararın tükettiği varlık |
| **Attention** | Resource | **P5'in kıt kaynağı** (SKR-019 Bulgu 3) |
| Event | Temporal | Olan biten olgu |
| State | Temporal | Varlığın zamandaki durumu |
| Metric | Measure | Tanımlı ölçü (ör. `H(A\|C)`) |

## Semantic Profiles (SHACL-benzeri shape'ler)
Validator, her node'u profiline göre denetler (zorunlu alan/ilişki).

| Profile | Zorunlu | Nodes |
|---------|---------|-------|
| **Deliberative** | Identity, `serves`≥1 Purpose, Context, Alternatives, Evidence, `constrained_by`*, Outcome, Lifecycle | Decision |
| **Intent** | Identity, taksonomi (`specializes`), served-by | Purpose, Goal |
| **Rule** | Identity, `constrains`≥1, `derived_from`, Lifecycle | Constraint |
| **Store** | Identity, `stores`, `retrieves`, decay | Memory |
| **Assertion** | Identity, `supports`\|`invalidates` hedefi | Evidence, Claim, Learning |
| **Agent** | Identity, `owns`\|`pursues`, `part_of` | Actor, Organization |
| **Resource** | Identity, `consumed_by`\|`supports`, scarcity | Capability, Resource, Attention |
| **Measure** | Identity, `measures` hedefi, formula | Metric |
| **Temporal** | Identity, timestamp/`has_state`\|`changes` | Context, Event, State |

## Relation Registry (first-class — SKR-019 Bulgu 1)
Her relation tam tanımlı; belge içinde serbest kullanım yasak.

| Relation | Domain → Range | Dir | Card | Inverse | Trans | Sym | Default |
|----------|----------------|-----|------|---------|-------|-----|---------|
| serves | Decision → Purpose | → | N:N | served_by | ✗ | ✗ | Allowed |
| supports | Capability/Evidence → Purpose/Claim | → | N:N | supported_by | ✗ | ✗ | Allowed |
| invalidates | Evidence → Claim | → | N:N | invalidated_by | ✗ | ✗ | Allowed |
| constrains | Constraint → Decision/Capability | → | N:N | constrained_by | ✗ | ✗ | Allowed |
| pursues | Actor/Organization → Goal | → | N:N | pursued_by | ✗ | ✗ | Allowed |
| requires | Decision/Capability → Resource | → | N:N | required_by | ✗ | ✗ | Allowed |
| consumes | Decision → Attention/Resource | → | N:N | consumed_by | ✗ | ✗ | Allowed |
| allocated_to | Attention → Decision | → | N:N | consumes | ✗ | ✗ | Allowed |
| has_context | Decision → Context | → | 1:N | context_of | ✗ | ✗ | Allowed |
| stores | Memory → Decision | → | 1:N | stored_in | ✗ | ✗ | Allowed |
| retrieves | Memory → Decision/Context | → | N:N | retrieved_by | ✗ | ✗ | Allowed |
| updates | Learning → Memory | → | N:N | updated_by | ✗ | ✗ | Allowed |
| produces | Decision → Event | → | 1:N | produced_by | ✗ | ✗ | Allowed |
| changes | Event → State | → | N:N | changed_by | ✗ | ✗ | Allowed |
| has_state | Entity → State | → | 1:N | state_of | ✗ | ✗ | Allowed |
| measures | Metric → Concept/State | → | N:1 | measured_by | ✗ | ✗ | Allowed |
| part_of | Actor → Organization | → | N:1 | contains | **✓** | ✗ | Allowed |
| owns | Actor → Decision | → | 1:N | owned_by | ✗ | ✗ | Allowed |
| specializes | Node → Node (aynı tür) | → | N:1 | generalizes | **✓** | ✗ | Allowed |
| refines | Goal → Purpose | → | N:1 | refined_by | ✗ | ✗ | Allowed |

## Allowed / Discouraged / Forbidden (SKR-019 Bulgu 6 — üç kademe)
Bir relation, declared domain→range dışında kullanılırsa **Forbidden**. Declared ama zayıf
anlamlı kullanım **Discouraged** (Linter uyarır, reddetmez).

| Örnek kenar | Statü | Neden |
|-------------|-------|-------|
| Metric `measures` Decision-consistency | Allowed | doğru domain→range |
| Metric `supports` Decision | **Discouraged** | Metric bilgilendirir, Claim'i doğrudan desteklemez |
| Metric `implements` Decision | **Forbidden** | seviye/tür ihlali (kullanıcı örneği) |
| Decision `serves` Decision | **Forbidden** | Purpose'a hizmet eder, karara değil |
| Purpose `serves` Decision | **Forbidden** | ters yön |

## Relation Composition (SKR-019 ötesi — Bulgu 7)
Zincir kuralları *declare edilir* (inference/reasoning Faz 4 tooling):
- `part_of` transitive: A part_of B ∧ B part_of C ⇒ A part_of C.
- `specializes` transitive: taksonomi kapanışı.
- **serves ∘ supports:** Decision `serves` Purpose ∧ Capability `supports` Purpose ⇒ Decision
  *indirectly_supported_by* Capability (türetilmiş, saklanmaz — sorgulanır).

## Semantic Closure (SKR-019 Bulgu 5 — her node'da, belge sonunda değil)
Her node'un kapanışı: tipli-kenar traversal'ıyla ulaştığı Principle/Law/Theory. (Tam kapanış
Faz 4 KG-sorgusuyla üretilir; örnekler:)
- **Decision** → Principles: P1 (atom), P2, P4, P7. Laws: Entropy, Gravity, Capital. Theory: ENS-2001.
- **Attention** → Principle: P5. Law: Decision Gravity (ENS-3022). Theory: ENS-2002/§attention.
- **Memory** → Principle: P3. Law: LAW-ORG-MEMORY. Theory: ENS-2003.
- **Purpose** → Principle: P1. Theory: ENS-2001; taksonomi ENS-2003 (OM2).

Bir node'un kapanışı hiçbir Principle'a ulaşmıyorsa **orphan concept**'tir (Linter reddeder).

## Enterprise Ontology (ENS-4020, sonra)
Domain node'ları (`ens-ent:`): Invoice, Supplier, Customer… — Foundational'ı specialize eder
(`ens-ent:Invoice specializes ens-core:Event`). Deployment-başına.

## Validation Rules + Semantic Constraints bundan türer
```
Relation/Node Registry + Profiles + Allowed/Discouraged/Forbidden
        → Validation Rules → Ontology Linter
        → ENS Semantic Constraints (invariant'lar, sonraki belge)
```
Kural elle yazılmaz; tip sisteminden üretilir.

## Failure conditions (Anayasa Madde X)
- **Aşırı-modelleme (Meta Model uyarısı tekrarı).** Tip sistemi OWL'a yaklaşırsa ossifiye olur;
  minimal + RFC-genişletilebilir kalmalı. Composition *inference* Faz 4; şimdilik yalnızca declare.
- **Profil/closure tamlığı Faz 4 tooling'e bağlı.** Şu an örnek closure'lar elle; tam otomatik
  kapanış KG-sorgusu (Faz 4) ister — o zamana dek elle bakım riski.
- **Registry sürüklenmesi.** Belge içinde ad-hoc relation kullanımı Registry'yi baypas edebilir;
  Linter zorlamalı (yoksa tip sistemi kâğıt üstünde kalır).
- **Prior-art delta darlığı.** ENS'in özgünlüğü tip sisteminde değil (o OWL/SHACL), yalnızca
  Principle-closure ve Meta Model bağında; bu dürüstçe kabul edilir.

## SKR-019'a yanıt
| Bulgu | Tek-tek yama değil — tip sistemiyle çözüm |
|-------|-------------------------------------------|
| 1. Relation eksik | **Relation Registry** (first-class, tam tanımlı) |
| 2. Şablon eksik | **Semantic Profiles** (SHACL-benzeri, node→profil) |
| 3. Attention yok | **Node Registry**'ye Attention (Resource profili) + consumes/allocated_to |
| 4. Homonim | **Namespace** (ens-meta / ens-core / ens-ent) |
| 5. Closure | **Semantic Closure** her node'da (Principle/Law/Theory reachability) |
| (+ Bulgu 6) | Allowed/**Discouraged**/Forbidden üç kademe |
| (+ Bulgu 7) | **Relation Composition** kuralları |

---

*Foundational Ontology artık bir kavram listesi değil, bir semantik tip sistemidir: node ve
relation first-class, profile'lar shape, closure Principle'a kadar. Validation, Linter ve
Architecture aynı kaynaktan türer — mekanik olarak tutarlı.*
