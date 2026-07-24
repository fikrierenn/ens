---
id: ENS-4001
title: ENS Meta Model
type: ontology
canon: false
origin: ENS-0000 §IV, §VIII, §XI
depends_on: [ENS-0000, ENS-4000]
referenced_by: []
principles: [P1, P2]
status: review
owner: ens-architect
version: 0.6.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: [SKR-023, pending]
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

## v0.5 — Ontolojik Katmanlar: Computational vs Organizational (EXPERIMENTAL —
## Design Review'dan geçti, Ontology Validation bekliyor)

> **Statü uyarısı:** Bu bölüm **ratified primitif küme değildir.** ADR-0001/0002 üzerinden
> North Star analizinden çıktı; 3 senaryoda (ERP, saf-bilişsel, fiziksel/IoT) stres testinden
> ve bir **Design Review** turundan (6 hedefli saldırı, 4 gerçek çatlak bulundu ve düzeltildi)
> geçti. Ontology Validation'dan geçmeden `canon:false` kalır (GOV-030).

### Prior-art (5-başlık disiplini — bu sürümde ciddi ödünç alındı)
| Kaynak | Ne verdi | ENS'e ödünç | Delta |
|---|---|---|---|
| **Entity-Component-System** (oyun motoru deseni, onlarca yıllık) | Identity/data/behavior ayrımı; "disjoint mutation → deterministic concurrency" | Identity/Event/Capability üçlüsünün yapısal iskeleti | ENS icat etmedi — ECS'in enterprise-decision alanına uygulanışı |
| **Greg Young, Event Sourcing/CQRS** | `Projection = left-fold(EventLog)`; projection stateful/stateless, filter/transformer olabilir | `projects` ilişkisinin kesin tanımı | Projection kavramı yeni değil; onu Axiom-düzeyine çıkarıp Organizational Construct'lara bağlamak ENS'in katkısı |
| **TOVE Enterprise Ontology** (Fox & Grüninger) | "Empowerment = agent'ın durum-değiştiren eylem yapma **hakkı**"; agents/roles/goals/authority/commitment | Policy'nin kesin tanımı: Empowerment-veren/kısıtlayan kural | Bounded-Autonomy Gate, Empowerment'ın çalışma-zamanı uygulanışı |
| **LOM-action** (arXiv:2604.08603, 2026) | `Event → Simulation → Decision`; Sandbox→deterministik mutasyon→G_sim; Skill Registry; audit hedef-odaklı | Simulation'ın kesin tanımı; Sandbox deseni | **En derin karşılaştırma:** LOM-action **Enterprise Ontology'yi başlangıç/temel** kabul ediyor. ENS onun **altında** daha temel bir hesaplama katmanı (Computational Ontology) arıyor — bu daha iddialı bir konum. Ayrıca: LOM-action Skill=Capability özdeşliyor (ENS ayırıyor, yukarı bkz.), Simulation'ı mimari-merkez yapıyor (ENS onu `projects`'in bir örneğine indirgiyor), audit'i hedef yapıyor (ENS'te yan-etki), **Identity'yi hiç tartışmıyor** (ENS'in açık bıraktığı soru orada da yok — yalnız değiliz) |
| Rippletide "Decision Kernel", Tencent "Cognitive Kernel" | İsim-alanı emsali | — | Yalnızca konumlandırma; mimari örtüşme yok |

### Katman modeli (namespace düzeltmesi — Design Review Q1)
```
Meta Concepts            (ens-meta:  — bu belgenin kendi düğüm türleri)
        ↓
Computational Ontology   (ens-comp: — Identity, Event, Capability — YENİ namespace)
        ↓  [Projection Semantic Connector — ENS-4001 v0.3'te zaten rezerve edilmişti]
Organizational Ontology  (ens-core: — Decision, Goal, Policy, Constraint, Memory, Context, Learning — ENS-4010)
        ↓  [specializes Bridge]
Enterprise Ontology       (ens-ent: — domain: Invoice, Supplier… — ENS-4020)
```
**Düzeltme:** Computational primitifler `ens-core:` içine gömülü değil, **ayrı `ens-comp:`**
namespace'inde. Bu sayede `Projection`, ENS-4001 v0.3'ün rezerve ettiği dördüncü Semantic
Connector türünü (Bridge/Realization/Mapping/**Projection**) gerçekten doldurur — cross-
namespace bir köprüdür, genel bir "meta-ilişki" değil (Design Review Bulgu 1).

### Computational primitifler (`ens-comp:`)

| Primitif | Tür | Tanım |
|---|---|---|
| **Identity** | value — **AÇIK, çözülmedi** | Zaman boyunca kimliği korunan, adreslenebilir referans. Design Review'da saldırıya uğradı: Event-kümelenmesinden türeyen bir projeksiyon da olabilir. Dürüstçe açık bırakılıyor. |
| **Event** | value | Atomik, immutable olgu: `Event(id, emitter:ens-comp:Identity, target:Identity?, timestamp, payload)`. |
| **Capability** | **type** (**Contract**, "type" değil — Design Review Q3) | `accepts: Event[] → emits: Event[]`, davranışsal ön/art-koşullarla (ADR-0001 Bounded-Autonomy Gate bunu gerektirir). |

**Capability → Implementation → Skill (LOM-action'ın "Skill Registry"sinden ayrıştırma):**
LOM-action, Skill'i Capability ile özdeşleştiriyor. ENS ayırıyor: **Capability** soyut sözleşme
(`CalculateTax`); **Implementation** onu gerçekleştiren somut backend (Python/SAP/Oracle/LLM);
**Skill** belirli bir Implementation örneği. Bir Capability Pack (ADR-0001 §6), bir Capability'nin
**bir Skill'ini** sağlar — kernel yalnızca Capability-sözleşmesini bilir, hangi Skill'in
çalıştığını bilmesi gerekmez (model-agnostisizmin genel biçimi, yalnızca LLM için değil).

### Üç meta-ilişki (Greg Young'a göre kesinleştirildi)
```
emits:      ens-comp:Identity → ens-comp:Event
Capability: accepts: Event[] → emits: Event[]         (event-sourced yan etki)
projects:   (EventStream × ProjectionDefinition) → ens-core:OrganizationalConstruct
            [Projection Semantic Connector, ens-comp:→ens-core:]
```
`projects`, Greg Young'ın tanımına sadık: **stateful ya da stateless**, **filter ya da
transformer** olabilir. `StockBalance`, `InventoryValuation`, `InventoryTurnover` — aynı
Event'lerin farklı `ProjectionDefinition`'ları (filter/transformer kombinasyonları).

**ProjectionDefinition — yeni Meta node (Design Review Q6):** Node Registry'ye eklenir
(Metric'in eklendiği gibi). Kendi kimliği/versiyonu vardır (EC-* evolution constraints ona
uygulanır — "StockBalance v2" bir `version_of` zinciri gerektirir).

### Organizational Construct
Decision(=commitment), Goal(=intention), Memory(=biriken yorum), Policy(=**Empowerment**-kuralı,
TOVE), Constraint, Context, Learning — **Organizational Construct**: `projects` ile Event
akışından türeyen, örgütsel anlam taşıyan yapılar. Policy artık TOVE'nin kesinliğiyle: *"bir
Principal'ın durum-değiştiren bir Event emit etme **hakkını** (empowerment) Constraint-durumuna
göre veren/kısıtlayan kural."*

### Event Stream Topology — Sandbox = üç altyapı servisi (LOM-action karşılaştırması derinleştirildi)
Design Review Q4: branching bir primitif özelliği değil, **Event Store (altyapı) yeteneği.**
LOM-action'ın "Sandbox" tekil bir yapıtaş gibi ele alınıyor; ENS onu **üçe ayırarak daha temiz**
konumlar:
```
Sandbox = Event Store Service + Branch Manager + Projection Engine
```
```
main            — gerçek, commit-edilmiş tarih (Axiom 3, asla değişmez)
sandbox/*       — izole, deterministik Event-graf mutasyonu; G_sim üretir, ASLA main'e karışmaz
replay/*        — audit/rollback için tarihsel yeniden-oynatma
```

**Simulation, ayrı bir primitif/servis değil — `projects`'in özel bir örneği:**
```
Event Stream → Projection(SimulationDefinition) → Simulation Graph (G_sim) → Decision
```
`SimulationDefinition`, `ProjectionDefinition`'ın bir alt-türüdür (aynı Meta node, aynı
EC-* versiyonlama). Bu genelleme kritik: aynı mekanizma State, Memory, Constraint, Context
için de çalışır — ENS yalnızca "simülasyon yapan" değil, Event'lerden **çok sayıda farklı
örgütsel anlam katmanı türetebilen** genel bir hesaplama modeli sunuyor. LOM-action Simulation'ı
mimarinin merkezine koyuyor; ENS onu genel `projects` mekanizmasının **bir örneği** olarak
tutarak daha geniş bir temel öneriyor — bu, LOM-action'a göre iddialı bir delta.

**Audit, hedef değil yan-etki:** LOM-action audit'i doğrudan tasarım hedefi yapıyor. ENS'te
replay, Event Store'un **doğal sonucu** (Axiom 3) — audit ayrı inşa edilmiyor, mimarinin
zorunlu çıktısı.

### Principal — rol, alt-tip değil (Design Review Q5)
Design Review düzeltmesi: Principal, Identity'nin sabit bir alt-tipi **değil**, **Event-başına
bir rol**: `accountable_to: Event → ens-comp:Identity`. Bir Identity bir Event'te yalnızca
Emitter olabilir, başka bir Event'te Principal (hesap-verebilir) olabilir — CEO'nun otomasyon
ajanı Emitter'dır, CEO accountable_to hedefidir. ENS-4010'un `Actor` node'u artık bu ilişkinin
domain-tarafındaki bir örneklemesi, sabit bir alt-tip değil.

### Üç Aksiyom (Computational Closure çerçevesiyle)
```
Axiom 1 — Computational Independence
  Computational events SHALL exist independently of organizational interpretation.

Axiom 2 — Computational Closure (Semantic Projection)
  Every organizational construct SHALL be derivable as one or more projections
  over computational events. [Yeni primitif önerisi, önce Closure'ı kırdığını
  kanıtlamalıdır — primitif enflasyonuna karşı savunma hattı.]

Axiom 3 — Non-Leakage
  Organizational semantics SHALL NOT alter computational history.
```
Axiom 3'ün sonucu: "Decision'ı değiştirelim" → **hayır, yeni Event.** Projection yeniden
hesaplanır; history değişmez. EC-001 (Canonical silinemez) ve Madde VIII'in hesaplama-düzeyi
ifadesi — yeni felsefe değil, var olanın en temel katmana indirilmesi.

### Stres testi + Design Review özeti
3 senaryo (ERP/bilişsel/IoT) kırılmadan indirgendi. Design Review'un 6 hedefinden **4'ünde
gerçek çatlak bulundu ve yukarıda düzeltildi** (namespace, branching-sınıflandırması,
Principal-as-role, ProjectionDefinition-node); **1'i açık bırakıldı** (Identity); **1'i
terminoloji düzeltmesiydi** (Capability=Contract).

### Failure conditions (Madde X)
- **Identity'nin primitifliği hâlâ kanıtlanmadı** — en zayıf halka, sonraki Ontology
  Validation'ın asıl hedefi olmalı. (LOM-action da bunu tartışmıyor — ENS bu açıdan yalnız
  değil, ama bu bir mazeret değil, çözülmesi gereken açık bir araştırma sorusu.)
- **Deneysel statü ciddiye alınmazsa** ratified zannedilip 7000 buna dayanabilir.
- **Decision⊂Event sorusu açık** (Model B kabul edildi, P1'in "anlamlı" ifadesiyle çelişmediği
  doğrulandı — Anayasa amendment gerekmiyor).
- **`ens-comp:` namespace'i henüz Meta Model'in Node/Relation Registry şablonuna (7-parça,
  ENS-4010 deseni) tam oturmadı** — Ontology Validation'da tamamlanmalı.
- **Değerlendirme ekseni tekil kalırsa yanıltıcı olur ("Illusive Accuracy", LOM-action'dan
  ödünç).** Faz 4/5'te bu modelin başarısını tek bir "accuracy" ile ölçmek, doğru cevabı yanlış
  reasoning-chain ile üretmeyi gizler. **İleri-işaret (şimdi tasarlanmıyor, Faz 4/5 borcu):**
  ayrı eksenler — Decision Validity, Auditability, Replayability, Policy Compliance, Simulation
  Fidelity — evidence-standard.md'nin 4-boyutuyla (sci/eng/ops/econ) hizalanarak genişletilmeli.

---

*Meta Model, ENS'in kendi bilgisinin şemasıdır. Reference graph "neye bağlı", knowledge graph
"hangi anlamda bağlı" der. ENS'in konusu bilgi olduğuna göre, tipli-semantik-zamansal graf şarttır.*
