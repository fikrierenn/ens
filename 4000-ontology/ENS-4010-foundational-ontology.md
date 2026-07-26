---
id: ENS-4010
title: Foundational Ontology (Organizational Ontology)
type: ontology
canon: false
constitutive: false
origin: ENS-0000 §IV, ENS-4001, ENS-2001, ENS-2002, ENS-2003, ENS-2004
depends_on: [ENS-0000, ENS-4001, ENS-2001, ENS-2002, ENS-2003, ENS-2004]
referenced_by: [ENS-4020, ENS-4025, ENS-4030, ENS-4031, ADR-0001, ADR-0002]
principles: [P1, P2, P3, P4, P5]
status: skeptic-cleared
owner: ens-architect
version: 0.5.0
last_reviewed: 2026-07-24
failure_conditions: stated
skeptic_review: [SKR-020, SKR-038, SKR-039]  # v0.4.0: SKR-028/030/031-B1 kaynaklı üç kusur (Kusur 1/2/3) düzeltildi → SKR-038 (bağımsız yeni tur, G2/G3) üçünü de DOĞRU buldu AMA verdict=wounded (Yara A: taranmamış 4. aynı-sınıf kusur Intent/Goal/served-by + Rule/derived_from; Yara B: "kök Organization muaf" mekanik denetlenemez). v0.5.0: Yara A/B kapatıldı — Intent+Rule+Agent profilleri node-tipine-özel yeniden yazıldı, `derived_from` Registry'ye kayıtlı relation olarak eklendi, kök `is_root` çıkarımsal olarak operasyonelleştirildi, iki invariant TÜM profillere/Trans:✓ relation'lara elle uygulanıp §"Invariant denetim tablosu"na yazıldı. **SKR-039 (bağımsız 2. tur, G2/G3, taze context) → `survives`:** Yara A-1/A-2/B kapandı bağımsız doğrulandı; kapsamlılık testi geçti (17 node + 2 Trans:✓ relation sıfırdan türetildi, SKR-038/D-1 deseninin 3. tekrarı YOK). 3 bloke-etmeyen keskinleştirme: (1) Deliberative profili hâlâ flat/karışık (relation vs Decision-Object bileşeni) — SKR-038 talep-3'ün kalanı; (2) `ens-core:derived_from` ≠ `ens-meta:derived_from` (ENS-4001) homonimi §Homonim notuna eklenmeli; (3) downstream propagasyon: ENS-4031 IR-002/IR-005 bayrağı kaldırılabilir + 21 relation sayımı (owner edimleri).
maturity: M2  # M2 iki bağımsız turla desteklenir: SKR-038 üç yamayı DOĞRU buldu, SKR-039 kapsamlılığı+Yara A/B kapanışını bağımsız teyit etti (ENS-4020 G4 iki-validator deseni). canon:false korunur; ratified/canon ayrı governance edimi.
evidence: {sci: E3, eng: E0, ops: E0, econ: E0}
---

# ENS Foundational Ontology — Organizational Ontology

> ENS'in **semantik çekirdeği** ve **tip sistemi**. Kurumsal bilişin (şirketten bağımsız)
> node ve relation'larını, *makine-doğrulanabilir* biçimde tanımlar. `canon: false` (M1).
>
> **v0.2 notu:** [SKR-019](reviews/SKR-019-foundational-ontology.md)'un 5 bulgusu tek tek
> yamanmadı; kök neden çözüldü — ontoloji bir **tip sistemine** dönüştü (Node Registry,
> Relation Registry, Semantic Profiles, Namespace, Composition, Semantic Closure). §Yanıt sonda.
>
> **v0.4 notu — YENİDEN KONUMLANMA (EXPERIMENTAL, [ENS-4001](ENS-4001-meta-model.md) v0.4'teki
> "Ontolojik Katmanlar" bölümüne bağlı):** Bu belge artık "primitif tip sistemi" iddia etmiyor. ENS-4001'e eklenen
> **Computational Ontology** (Identity/Event/Capability, deneysel) bulgusuyla, buradaki
> node'lar (Decision, Context, Memory, Goal, Constraint, Policy, Learning...) **primitif
> değil — Event akışı üzerinde `projects` ile türeyen Organizational Construct'lardır.**
> Aşağıdaki tablo bu yeniden-çerçevelemeyi gösterir; Node/Relation Registry içeriği bu *repositioning*
> sırasında **değişmedi** (hâlâ geçerli, `ens-core:` namespace'inde), yalnızca ontolojik statüleri
> düzeltildi. **(v0.4.0 profil↔registry düzeltmesi ayrıdır — `supports`/`part_of` domain'lerini ve
> üç profili günceller; bkz. aşağıdaki "v0.4.0 düzeltme notu".)**
>
> | Node | Eski çerçeve | Yeni çerçeve (v0.4) |
> |---|---|---|
> | Decision | atom, primitif | `projects(EventStream, "commitment-view") → Organizational Construct`; commitment-anı bir ayrıcalıklı **Event**'tir (Model B, açık soru) |
> | Context | — | `project(Snapshot/EventStream, relevance≥θ)` — ENS-2002'nin kendi formülü |
> | Memory | — | `project(Decision×Learning-events, zaman-indeksli)` |
> | Constraint | bağımsız primitif | `project(Events: Declared\|Modified\|Revoked)` — artık primitif değil |
> | Goal | node, zayıf şema | `project(Intent-türü-Event, "yorumlama") + şema` |
> | Policy | Constraint bundle | `f(Constraint-projeksiyonu, Actor-rolü) → yükümlülük` — kural, ham bundle değil |
> | Learning | — | `Δ(Expected(Decision), Observed(Snapshot'))` — ENS-2004'ün kendi formülü |
> | Actor | bağımsız node | **Principal**'ın specialization'ı (ENS-4001 Emitter/Principal ayrımı) |
>
> Bu, Node/Relation Registry'yi **geçersiz kılmaz** — yalnızca "bunlar computational-primitif"
> iddiasını geri çeker, "bunlar organizational-construct, Event'ten türer" der. Semantic
> Profiles, Allowed/Forbidden, Closure hepsi geçerli kalır.
>
> **v0.4.0 düzeltme notu — profil↔registry kapanışı (SKR-028 / SKR-030 / SKR-031-B1):** Üç bağımsız
> Ontology Validation turu, Semantic Profiles'ın zorunlu-kenar listeleri ile Relation Registry'nin o
> node-tipleri için lisansladığı kenarlar arasında **aynı sınıftan üç yapısal tutarsızlık** buldu
> (hepsi bu belgenin *kendi* iç-çelişkisi, downstream değil): (1) Assertion profili `Claim`/`Learning`
> için sağlanamıyor; (2) Resource profili duran/gerekli kaynaklar (`required_by`) için sağlanamıyor;
> (3) `part_of` `Actor → Organization` tiplenip `Trans: ✓` işaretlenmiş — domain≠range olduğundan zincir
> kuramıyor. Üçü de bu sürümde kapatıldı — bkz. §"SKR-028/030/031'e yanıt". Kök tema tek: **her profilin
> zorunlu çıkan-kenarı, o node-tipinin Registry-domain'inde gerçekten var olmalı** (yoksa node conformant
> instance üretemez); ve **her `Trans: ✓` relation'ın range'i domain'inin alt-kümesi olmalı** (yoksa
> zincirlenemez). Değişen tablolar: Relation Registry (`supports`, `part_of` domain'leri), Semantic
> Profiles (Assertion, Resource, Agent). **Bu düzeltme bağımsız yeni bir skeptic turu bekler (G2/G3).**
>
> **v0.5.0 düzeltme notu — invariant'ın TÜM profillere uygulanması (SKR-038 Yara A/B):** SKR-038
> (bağımsız tur) üç yamayı da DOĞRU buldu ama gösterdi ki v0.4.0'ın eklediği iki invariant *tüm*
> profillere uygulanmamıştı; aynı sınıftan taranmamış kusurlar kaldı. v0.5.0 bu boşluğu kapatır:
> **(Yara A-1)** Intent profili node-tipine-özel yeniden yazıldı — `serves` range'i {Purpose}
> olduğundan Goal `served-by` sağlayamıyordu; Goal artık `refines`\|`pursued_by` ile conformant.
> **(Yara A-2)** `derived_from` Relation Registry'de kayıtlı değildi (Rule profili onu zorunlu
> kılmasına rağmen) → Registry'ye resmen eklendi (`Constraint → Purpose/Constraint`, Trans:✗);
> bu aynı zamanda Constraint'in Semantic Closure'da Principle'a ulaşacağı upstream kenarı sağlar.
> **(Yara B)** Agent profilindeki "kök Organization muaf" parantezli istisnası — mekanik
> denetlenemez — kaldırıldı; `part_of` yapısal-opsiyonel (N:1, 0..1) olarak yazıldı ve **kök**
> çıkarımsal olarak operasyonelleştirildi (`is_root(o) ≡ Organization(o) ∧ ¬∃x. part_of(o,x)`);
> ≥2 kök → Linter uyarısı (öksüz-modelleme şüphesi). Ayrıca iki invariant **tüm 9 profile ve tüm
> Trans:✓ relation'a** elle uygulanıp sonuç §"Invariant denetim tablosu"na yazıldı — başka
> taranmamış örnek kalmadığı gösterildi. **Bu düzeltme de bağımsız yeni bir skeptic turu bekler.**

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
| **Intent** | Identity, `specializes` (taksonomi); en az bir **intent-bağı** (node-tipine göre, Registry-lisanslı): Purpose → `served-by` (gelen, = `serves`⁻¹, domain {Purpose}); Goal → `refines` (çıkan, →Purpose) \| `pursued_by` (gelen, = `pursues`⁻¹, range {Goal}) | Purpose, Goal |
| **Rule** | Identity, `constrains`≥1 (çıkan, Constraint∈domain), `derived_from` (çıkan, →Purpose/Constraint — Registry-lisanslı; upstream traceability), Lifecycle | Constraint |
| **Store** | Identity, `stores`, `retrieves`, decay | Memory |
| **Assertion** | Identity + en az bir **assertive bağ** (node-tipine göre, Registry-lisanslı): Evidence → `supports`\|`invalidates`; Claim → `supports` (çıkan) \| `supported_by`\|`invalidated_by` (gelen); Learning → `updates` | Evidence, Claim, Learning |
| **Agent** | Identity + en az bir **agency kenarı** (node-tipine göre): Actor → `owns`\|`pursues`; Organization → `pursues` (`owns` domain'i {Actor}, Organization sağlayamaz). `part_of` **yapısal-opsiyonel** (N:1, 0..1 üst): `part_of`'suz Organization = **kök** (`is_root` çıkarımsal, §"Kök operasyonelleştirmesi"); ≥2 kök → Linter **uyarı** (öksüz-modelleme şüphesi, hata değil) | Actor, Organization |
| **Resource** | Identity + en az bir **kaynak-rol kenarı**: `consumed_by`\|`allocated_to` (tüketilen/tahsis) \| `required_by` (duran/gerekli kaynak) \| `supports` (yeti→amaç, ör. Capability); scarcity | Capability, Resource, Attention |
| **Measure** | Identity, `measures` hedefi, formula | Metric |
| **Temporal** | Identity, timestamp/`has_state`\|`changes` | Context, Event, State |

## Relation Registry (first-class — SKR-019 Bulgu 1)
Her relation tam tanımlı; belge içinde serbest kullanım yasak.

| Relation | Domain → Range | Dir | Card | Inverse | Trans | Sym | Default |
|----------|----------------|-----|------|---------|-------|-----|---------|
| serves | Decision → Purpose | → | N:N | served_by | ✗ | ✗ | Allowed |
| supports | Capability/Evidence/Claim → Purpose/Claim | → | N:N | supported_by | ✗ | ✗ | Allowed |
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
| part_of | Actor/Organization → Organization | → | N:1 | contains | **✓** | ✗ | Allowed |
| owns | Actor → Decision | → | 1:N | owned_by | ✗ | ✗ | Allowed |
| specializes | Node → Node (aynı tür) | → | N:1 | generalizes | **✓** | ✗ | Allowed |
| refines | Goal → Purpose | → | N:1 | refined_by | ✗ | ✗ | Allowed |
| derived_from | Constraint → Purpose/Constraint | → | N:N | derives | ✗ | ✗ | Allowed |

**`derived_from` (v0.5.0 — SKR-038 Yara A-2):** Rule profili Constraint için `derived_from`'u zorunlu
kılıyordu ama relation Registry'de kayıtlı değildi (kayıtsız kimlik → traceability ihlali). Resmen
eklendi: bir Constraint, hizmet ettiği bir **Purpose**'tan ya da genelleştirdiği başka bir
**Constraint**'ten türer (kural, ham değil — bir *neden*'den indirgenir). Bu, Constraint'in tek
downstream kenarı (`constrains`) dışında **upstream** bir kenar kazanmasını, dolayısıyla Semantic
Closure'da Principle'a ulaşmasını sağlar. `Trans: ✗` — türetim zinciri saklanmaz, sorgulanır;
dolayısıyla transitivity well-formedness invariant'ı ona uygulanmaz (range {Purpose, Constraint} ⊄
domain {Constraint} olması sorun değildir, çünkü zincirlenmez).

**Transitivity well-formedness (v0.4.0 — SKR-031/B1):** `Trans: ✓` işaretli her relation için
**`range ⊆ domain`** olmak zorundadır — aksi hâlde zincir kurulamaz (2-hop `A→B ∧ B→C` orta düğüm
B'nin hem range hem domain olmasını ister). `specializes` (domain=range) bunu sağlar; `part_of`
v0.4.0'da domain'i `Actor/Organization`'a genişletilerek sağlar (range {Organization} ⊆ domain
{Actor, Organization}). Bu, formal-checker'ın (G-09/10) invariant'ıdır; ihlal eden bir `Trans: ✓`
tiplemesi Registry'ye giremez.

**Profile satisfiability (v0.4.0 — SKR-028/SKR-030):** Bir Semantic Profile'ın her zorunlu-kenarı,
o profili taşıyan **her** node-tipinin Relation Registry-domain'inde (çıkan) ya da range'inde (gelen)
gerçekten var olmalıdır — yoksa o node conformant instance üretemez. Bu da formal-checker invariant'ıdır.

**Kök operasyonelleştirmesi (v0.5.0 — SKR-038 Yara B):** v0.4.0'ın Agent profili "kök Organization
muaf" istisnasını *parantezle* taşıyordu; validator bir gerçek kökü, üstü unutulmuş **öksüz** bir
Organization'dan ayırt edemiyordu (formal kök-işareti yoktu → `part_of` Organization için sessizce
optional'a düşüyordu). `part_of` cardinality zaten **N:1 (0..1 üst)** olduğundan "0 üst = kök"
tipleme düzeyinde meşrudur; kök, ayrı bir flag'e gerek olmadan **çıkarımsal** tanımlanır:

```
is_root(o) ≡ Organization(o) ∧ ¬∃x. part_of(o, x)        # hiçbir üst-örgüte part_of etmeyen Organization
```

Formal-checker bunu otomatik test eder: `part_of` yoksa Organization kök sayılır (muafiyet keyfî
owner-kararı değil, invariant). Öksüz-yanlış-modelleme yakalanır çünkü sağlıklı bir örgüt ağacında
**tam bir kök** beklenir; validator **≥2 kök** görürse bunu **uyarı** olarak raporlar (hata değil —
holding/çok-tüzel-kişilik meşru olabilir), böylece "üstü unutulmuş" Organization insan-incelemesine
düşer. Actor için `part_of` de yapısal-opsiyoneldir (bağımsız actor 0 üst taşıyabilir); Actor'ın
zorunluluğu `owns`\|`pursues` agency kenarıdır, `part_of` değil.

### Invariant denetim tablosu (v0.5.0 — SKR-038: iki invariant TÜM profillere/relation'lara uygulandı)
SKR-038, v0.4.0'ın invariant'ı yalnızca üç bilinen satıra uyguladığını, kalanı taramadığını gösterdi.
Aşağıda **her iki invariant elle tüm 9 profile ve tüm Trans:✓ relation'a** uygulanmıştır; başka
taranmamış aynı-sınıf kusur kalmadığı doğrulanmıştır.

**(i) Profile satisfiability — 9 profil × sağlayan Registry-kenarı:**

| Profil | Node(lar) | Zorunlu ilişkisel kenar | Sağlayan Registry-kenarı (domain/range) | Sonuç |
|--------|-----------|-------------------------|-----------------------------------------|-------|
| Deliberative | Decision | `serves`≥1, Context, `constrained_by` | `serves` (Decision∈dom); `has_context` (Decision∈dom); `constrained_by`=`constrains`⁻¹ (range∋Decision) | ✓ |
| Intent | Purpose | `served-by` | `served_by`=`serves`⁻¹ (domain {Purpose}∋Purpose) | ✓ (v0.5.0) |
| Intent | Goal | `refines`\|`pursued_by` | `refines` (Goal∈dom); `pursued_by`=`pursues`⁻¹ (range {Goal}∋Goal) | ✓ (v0.5.0) |
| Rule | Constraint | `constrains`≥1, `derived_from` | `constrains` (Constraint∈dom); `derived_from` (Constraint∈dom) | ✓ (v0.5.0) |
| Store | Memory | `stores`, `retrieves` | `stores` (Memory∈dom); `retrieves` (Memory∈dom) | ✓ |
| Assertion | Evidence | `supports`\|`invalidates` | `supports` (Evidence∈dom); `invalidates` (Evidence∈dom) | ✓ (v0.4.0) |
| Assertion | Claim | `supports`\|`supported_by`\|`invalidated_by` | `supports` (Claim∈dom); `supported_by`/`invalidated_by` (Claim∈range) | ✓ (v0.4.0) |
| Assertion | Learning | `updates` | `updates` (Learning∈dom) | ✓ (v0.4.0) |
| Agent | Actor | `owns`\|`pursues` | `owns` (Actor∈dom); `pursues` (Actor∈dom) | ✓ |
| Agent | Organization | `pursues` | `pursues` (Organization∈dom) | ✓ (v0.5.0) |
| Resource | Capability | `supports` | `supports` (Capability∈dom) | ✓ (v0.4.0) |
| Resource | Resource | `consumed_by`\|`required_by`\|`allocated_to` | `consumed_by`=`consumes`⁻¹ (range∋Resource); `required_by`=`requires`⁻¹ (range∋Resource) | ✓ (v0.4.0) |
| Resource | Attention | `allocated_to`\|`consumed_by` | `allocated_to` (Attention∈dom); `consumed_by`=`consumes`⁻¹ (consumes range {Attention,Resource}∋Attention) | ✓ (v0.4.0) |
| Measure | Metric | `measures` | `measures` (Metric∈dom) | ✓ |
| Temporal | Context | timestamp \| `context_of` | `context_of`=`has_context`⁻¹ (range∋Context) | ✓ |
| Temporal | Event | `changes`\|`produced_by` | `changes` (Event∈dom); `produced_by`=`produces`⁻¹ (range∋Event) | ✓ |
| Temporal | State | `has_state`(gelen)\|`changed_by` | `state_of`=`has_state`⁻¹ (range∋State); `changed_by`=`changes`⁻¹ (range∋State) | ✓ |

**Kapsam:** 9 profil, 17 (profil × node-tipi) satırı — hepsi sağlanır. SKR-038'in bulduğu iki boşluk
(Intent/Goal, Rule/derived_from) v0.5.0'da kapatıldı; kalan 15 zaten sağlıyordu.

**(ii) Transitivity well-formedness — tüm Trans:✓ relation'lar (`range ⊆ domain`?):**

| Relation | Domain | Range | range ⊆ domain? | Sonuç |
|----------|--------|-------|-----------------|-------|
| `part_of` | {Actor, Organization} | {Organization} | evet | ✓ (v0.4.0) |
| `specializes` | Node (aynı tür) | Node (aynı tür) | evet (domain=range) | ✓ |

Registry'de **yalnızca bu iki** relation `Trans: ✓` taşır; diğer 19'u (`derived_from` dahil) `Trans: ✗`
olduğundan bu invariant onlara uygulanmaz. Her iki transitive relation da well-formed. **Taranmamış
Trans:✓ relation kalmadı.**

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
- `part_of` transitive: A part_of B ∧ B part_of C ⇒ A part_of C. **Zincirlenebilir çünkü v0.4.0'da
  domain `Actor/Organization`'a genişletildi → range {Organization} ⊆ domain {Actor, Organization}
  (orta düğüm Organization hem hop-1'in range'i hem hop-2'nin domain'idir). Örgütsel hiyerarşi
  Team ⊂ Division ⊂ Company artık lisanslı; bireysel Actor da bir Organization zincirinin kuyruğundan
  köke türer (person ∈ team ∈ company ⇒ person part_of company).**
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
- **Constraint** → `derived_from` Purpose → P1 (v0.5.0: upstream kenar sayesinde artık Principle'a
  ulaşır; v0.4.0'da Constraint yalnız downstream `constrains` taşıdığından orphan riski vardı). Laws: ENS-3021/3022.

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

## SKR-028 / SKR-030 / SKR-031'e yanıt (v0.4.0 — profil↔registry kapanışı)
Üç bağımsız tur, aynı sınıftan üç kusur buldu: bir Semantic Profile'ın zorunlu çıkan-kenarı, o
profili taşıyan node'un Relation Registry-domain'inde yoktu (Kusur 1/2) ya da `Trans: ✓` bir relation'ın
range'i domain'inde değildi (Kusur 3) → node conformant instance üretemiyor / zincir kuramıyordu. Üçü de
**tip sisteminin kendi içinde** kapatıldı (tek-tek yama değil, kök tema düzeltildi):

| # | Kusur (kaynak) | Kök neden | Düzeltme (v0.4.0) |
|---|----------------|-----------|-------------------|
| 1 | Assertion profili ⊥ `supports`/`invalidates` domain'i (SKR-028 bildirdi, SKR-030 teyit+genişletti) | Profil {Evidence, Claim, Learning}'e zorunlu `supports`\|`invalidates` çıkan-kenarı koyar; ama `supports` domain'i {Capability, Evidence}, `invalidates` domain'i {Evidence} → **Claim ve Learning** bu kenarı sağlayamaz (yalnızca Evidence sağlar) | (a) Relation Registry: `supports` domain'ine **Claim** eklendi (`Capability/Evidence/Claim → Purpose/Claim`) — Claim bir Purpose'u ya da başka bir Claim'i destekler (argüman zinciri; ENS-2001 Reasoning premise'i). (b) Assertion profili node-tipine göre yeniden yazıldı: Evidence→`supports`\|`invalidates`; Claim→`supports` (çıkan) \| `supported_by`\|`invalidated_by` (gelen); Learning→`updates`. Üç node de artık Registry-lisanslı bir assertive bağ ile conformant. |
| 2 | Resource profili ⊥ `required_by` (SKR-030 yeni) | Profil {Capability, Resource, Attention}'a zorunlu `consumed_by`\|`supports` koyar; ama duran/gerekli kaynaklar (`SupplierRelationship` gibi, `specializes Resource`) `required_by` (=`requires` inverse) kullanır — listede yok; ayrıca `supports` yalnızca Capability'yi kapsar (Resource `supports` domain'inde değil) | Resource profili "en az bir kaynak-rol kenarı" olarak genellendi: `consumed_by`\|`allocated_to` (tüketilen/tahsis) \| `required_by` (duran/gerekli kaynak) \| `supports` (yeti→amaç, ör. Capability). **Registry değişmedi** — `required_by` zaten `requires`'ın inverse'ü olarak vardı; kusur salt profil-tanımındaydı. |
| 3 | `part_of` transitive tiplemesi kendi içinde tutarsız (SKR-031/B1, D-1 sınıfı) | `part_of` `Actor → Organization` (domain≠range) tiplenip aynı anda `Trans: ✓` + §Composition zinciri deklare edilir; `R: X→Y` (X≠Y) zincirlenemez — 2-hop zincir orta düğümün hem Organization (hop-1 range) hem Actor (hop-2 domain) olmasını ister, yani gizlice `Organization --part_of--> Organization` iddia eder ama bu kenar domain'de yok | `part_of` domain'i **`Actor/Organization → Organization`** olarak genişletildi (mevcut `pursues: Actor/Organization → Goal` desenine tutarlı — enumerasyon, subsumption değil). Artık range {Organization} ⊆ domain {Actor, Organization} → zincir well-formed; örgütsel hiyerarşi Team ⊂ Division ⊂ Company lisanslı. §Relation Composition part_of maddesi + yeni "Transitivity well-formedness" invariant'ı güncellendi. **ENS-4031 IR-002/IR-005'in "lisanslı değil" bayrağı bu düzeltmeyle kaldırılabilir hâle gelir — ama o değişiklik ENS-4031 owner'ınındır (ens-philosopher); bu belge yalnızca upstream tiplemeyi düzeltir, IR-002'ye dokunmaz.** |

**Neden domain-widening, `Organization specializes Actor` değil:** İki çözüm de range ⊆ domain'i
sağlardı, ama enumerasyon (`Actor/Organization → …`) belgede **zaten var olan** desendir
(`pursues: Actor/Organization → Goal`); subsumption ise "her Organization bir Actor'dır" gibi daha
güçlü ve tartışmalı bir ontolojik taahhüt getirir (bir örgüt, actor'lardan *oluşan* bir bütündür,
özelleşmiş bir birey-actor değil). Minimal + mevcut desene tutarlı olan seçildi (North Star: gereksiz
ontolojik ağırlık ekleme).

**Sistematik ders (formal-checker G-09/10 için iki invariant):** (i) *"Her Semantic Profile
zorunlu-kenarı, o node-tipinin Registry-domain'inde/range'inde gerçekten var mı?"* (ii) *"Her `Trans: ✓`
relation'ın range'i domain'inin alt-kümesi mi?"* İkisi de §Relation Registry'ye invariant olarak yazıldı;
üç kusur da tam olarak bunlardan birinin ihlaliydi — kök tema, dağınık bulgular değil.

**⚠️ Öz-onay yok (G2/G3):** Bu üç düzeltme yazarın (ens-architect) kendi işidir; `survives`/M2-teyidi
**iddia edilmiyor**. Değişen tablolar (Relation Registry `supports`/`part_of` + Assertion/Resource/Agent
profilleri) bağımsız yeni bir ens-skeptic turunu bekler.

**SKR-038 sonucu (bağımsız yeni tur, G2/G3 — 2026-07-24):** `wounded`. Üç hedeflenmiş yamanın (Kusur
1/2/3) hepsi teknik olarak **doğru** bulundu ve bağımsız denetimden sağ çıktı (Claim/Learning artık
conformant; Resource genellemesi required_by'ı kapsıyor ve fazla gevşetmiyor; `part_of` zinciri
well-formed, IR-002/IR-005 bayrağı kaldırılabilir). Ama iki yara açık: **(A)** bu belgenin eklediği
"Profile satisfiability" invariant'ı tüm profillere uygulanmadı → *aynı sınıftan* taranmamış 4. kusur:
**Intent profili / Goal / `served-by`** (`serves` range'i {Purpose}; Goal `served_by` domain'inde yok →
Goal conformant instance üretemez) + ikincil **Rule / `derived_from`** (Registry'de kayıtlı relation
değil). "Kök tema çözüldü, dağınık bulgu değil" iddiası bununla yerel olarak yanlışlanıyor. **(B)**
Agent profilindeki "kök Organization muaf" istisnası mekanik denetlenemez (kök-işareti yok → `part_of`
Organization için sessizce optional'a düşer). Talepler: (1) invariant'ı tüm profillere uygula ve
Intent/Goal + Rule'u düzelt **ya da** "sistematik kapanış" iddiasını daralt; (2) kök istisnasını
formal/node-tipine-özel yaz. Bu ikisi karşılanınca `survives`. Statü `review` → `skeptic-challenged`.

## SKR-038 Yara A/B'ye yanıt (v0.5.0 — invariant'ın tüm profillere uygulanması)
SKR-038 haklıydı: üç yama doğruydu ama v0.4.0'ın kendi "sistematik kapanış" iddiası, invariant'ı
yalnızca üç bilinen satıra uyguladığından **yerel olarak yanlışlanabilirdi**. v0.5.0 iddiayı
retorikle değil, **fiilen tüm profillere invariant'ı uygulayarak** karşılar (§"Invariant denetim
tablosu"):

| Yara | Kök neden | Düzeltme (v0.5.0) |
|------|-----------|-------------------|
| A-1 (Intent/Goal/served-by) | Intent profili {Purpose, Goal}'e düz `served-by` zorunlu kılıyordu; ama `serves` range'i {Purpose} → `served_by` domain'i {Purpose}, **Goal domain'de yok** → Goal conformant üretemez (Kusur 1'in Claim'iyle birebir aynı sınıf) | Intent profili node-tipine-özel yeniden yazıldı (Assertion/Resource ile aynı tarz): Purpose → `served-by`; Goal → `refines` (çıkan) \| `pursued_by` (gelen). İkisi de Registry-lisanslı; ikisi de conformant. |
| A-2 (Rule/derived_from) | Rule profili Constraint için `derived_from`'u zorunlu kılıyordu ama `derived_from` Relation Registry'de **hiç yoktu** — kayıtsız relation, traceability ihlali (satisfiability'nin daha sert biçimi: kenar Registry'de bile yok) | `derived_from` Registry'ye resmen eklendi (`Constraint → Purpose/Constraint`, N:N, inverse `derives`, Trans:✗). Constraint böylece upstream kenar kazanır → Semantic Closure'da Principle'a ulaşır (yan-fayda: v0.4.0'daki Constraint orphan riski de kapanır). |
| B (kök Organization) | "Kök muaf" parantezli istisna mekanik denetlenemezdi; gerçek kök ile öksüz Organization ayırt edilemiyordu → `part_of` sessizce optional'a düşüyordu | `part_of` yapısal-opsiyonel (N:1, 0..1) yazıldı; kök **çıkarımsal** operasyonelleştirildi: `is_root(o) ≡ Organization(o) ∧ ¬∃x. part_of(o,x)`. Formal-checker otomatik test eder; ≥2 kök → uyarı (öksüz-şüphe). Keyfî owner-kararı kalmadı. |

**Sistematik iddianın dürüst hâli (SKR-038 talep 1'e doğrudan yanıt):** "üç kusur tek kök temaydı"
iddiası artık **iki invariant'ın tüm 9 profile ve tüm Trans:✓ relation'a fiilen uygulanmasıyla**
desteklenir (denetim tablosu, 17 profil×node satırı + 2 transitive relation). v0.4.0'ın hatası
temada değil, **tarama kapsamındaydı**; v0.5.0 kapsamı tamamlar. Faz-4 formal-checker (G-09/10) bu
tabloyu otomatik üretecektir; şu anki elle-tarama onun spesifikasyonudur.

**Profil-yazım tarzı (SKR-038 talep 3, küçük):** Intent/Rule/Agent artık Assertion/Resource ile aynı
node-tipine-özel disjunctive tarzda; iki-tarz karışımı giderildi.

**⚠️ Öz-onay yok (G2/G3):** v0.5.0 düzeltmeleri de yazarın (ens-architect) kendi işidir;
`survives`/M2 **iddia edilmiyor**. SKR-038'in düzeltmesini SKR-038 onaylayamaz — değişen tablolar
(Relation Registry `derived_from` + Intent/Rule/Agent profilleri + kök operasyonelleştirmesi +
invariant denetim tablosu) **bağımsız yeni bir ens-skeptic turunu** bekler.

**SKR-039 sonucu (bağımsız 2. tur, G2/G3, taze context — 2026-07-24):** `survives`. SKR-038'in iki
yarası da bağımsız olarak kapatıldı bulundu: (A-1) `refines`|`pursued_by` Goal'i conformant kılıyor
(Registry-türetimiyle doğrulandı); (A-2) `derived_from` Registry satırı tutarlı ve Constraint gerçekten
`derived_from`→Purpose→P1 ile Principle'a ulaşıyor (elle takip). En kritik test — **kapsamlılık**:
denetim tablosunun 17 node × satisfiability iddiası + 21 relation'ın Trans-sütunu **sıfırdan
türetildi**, hepsi geçti; yalnızca `part_of`/`specializes` `Trans:✓` ve ikisi de `range⊆domain` →
**SKR-038/ENS-4031-D-1 deseninin 3. tekrarı YOK, taranmamış aynı-sınıf kusur kalmadı.** Yara B:
`is_root` çıkarımsal predikatı + "≥2 kök→uyarı" mekanik test edilebilir; root/orphan yapısal
ayırt-edilemezliği gizlenmemiş, dürüstçe uyarı-seviyesine indirilmiş. `derived_from` yeni bir
Kusur-1/2/3 sınıfı yaratmıyor (onu yalnız Rule profili ister; başka node muhtaç değil). **3
bloke-etmeyen keskinleştirme** (kapıyı geçmez): (1) Deliberative profili hâlâ flat — ilişkisel-kenar
(`serves`/`constrained_by`) ile Decision-Object bileşenini (Evidence/Alternatives/Outcome)
karıştırıyor, "Evidence" düz-sözcük olarak Evidence-node adıyla çakışıyor → formal-checker'ın
sahte-ilişki sanma riski (SKR-038 talep-3'ün kalan %10'u); (2) `ens-core:derived_from` ≠
`ens-meta:derived_from` (ENS-4001 line 70; ENS-4030 SC-004) **çapraz-seviye homonimi** — §Homonim
çözümü notuna `Claim` homoniminin ikinci örneği olarak eklenmeli; (3) downstream propagasyon —
ENS-4031 IR-002/IR-005 "lisanslı değil" bayrağı artık kaldırılabilir (owner ens-philosopher) +
Registry 21 relation'a çıktı (ENS-4025/4030/4031 relation-sayımı tutarlılık turu, ilgili owner'lar).
Statü `skeptic-challenged` → `skeptic-cleared`. Bkz. `reviews/SKR-039-foundational-ontology-invariant-closure.md`.

---

*Foundational Ontology artık bir kavram listesi değil, bir semantik tip sistemidir: node ve
relation first-class, profile'lar shape, closure Principle'a kadar. Validation, Linter ve
Architecture aynı kaynaktan türer — mekanik olarak tutarlı.*
