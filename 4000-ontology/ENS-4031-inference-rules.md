---
id: ENS-4031
title: Inference Rules
type: ontology
canon: false
constitutive: true
origin: ENS-4025 §"Inference Rules bu sözleşmeye uymalı"
depends_on: [ENS-0000, ENS-4025, ENS-4030, ENS-4001, ENS-4010]
referenced_by: []
principles: [P6, P2]
status: skeptic-cleared
owner: ens-philosopher
version: 0.3.0
last_reviewed: 2026-07-25
failure_conditions: stated
skeptic_review: [SKR-031, SKR-032]  # SKR-031 wounded → B1/B2/D-1 düzeltildi → SKR-032 (bağımsız 2. tur) survives. v0.3.0: B1 upstream (ENS-4010 v0.5.0, `part_of` domain-widening; SKR-038+SKR-039 iki bağımsız tur, survives/M2) kapandı → IR-002/IR-005-part_of artık Registry-lisanslı; bu düzeltme yeni iddia eklemez, kapanmış borcu propagate eder. ratified/canon:true AYRI governance edimi.
maturity: M0
evidence: {sci: E1, eng: E0, ops: E0, econ: E0}
requires: [ENS-4025, ENS-4010, ENS-4001]
provides: [Inference Rules, Derived Relations]
consumed_by: [reasoning engine (Faz 4), Validation Rules]
---

# ENS Inference Rules

> Semantic Logic'in (ENS-4025) soyut sözleşmesinin **somut kural kümesi**: "hangi örgütsel
> çıkarım, hangi öncüllerden, hangi kural kimliğiyle, hangi confidence ile üretilir?" Her kural,
> ENS-4010 **Relation Registry**'sindeki *gerçek* relation'lara **veya** — ilgili durumlarda —
> ENS-4001 **Meta Model**'deki gerçek, tipli relation'lara (ör. IR-007'nin `contradicts`'i)
> dayanır (uydurma yok) ve ENS-4025'in sekiz taahhüdüne (L1-L8) uymak zorundadır. **v0.3.0 —
> `canon: false`; SKR-031 `wounded` → düzeltildi → SKR-032 `survives` (skeptic-cleared). B1
> (IR-002/IR-005 `part_of` çok-hop lisansı) ✅ **kapandı**: upstream düzeltme ENS-4010 v0.5.0'da
> yapıldı (`part_of` domain'i `Actor/Organization → Organization`), SKR-038+SKR-039 iki bağımsız
> tur ile teyit edildi (survives/M2); IR-002/IR-005-part_of artık lisanslı türetim üretir.**

## Beş-başlık (ENS-4025'in disiplini sürer)
- **Prior art:** OWL 2 **property chain axioms** (`SubObjectPropertyOf(ObjectPropertyChain(...))`),
  RDFS/OWL **transitive property** (`owl:TransitiveProperty`), **SWRL** (Semantic Web Rule
  Language), **Datalog** kural gövdeleri, üretim sistemleri (RETE). Confidence tarafı: olasılıksal
  mantık (ProbLog, Markov Logic Networks), fuzzy t-norm'lar. Proof-trace tarafı: Truth Maintenance
  System / justification tree.
- **Delta:** ENS bu kural biçimlerini *icat etmez*. Katkı üç yerde: (1) kurallar ENS-4010 Relation
  Registry'ye **kapatılmıştır** — Registry-dışı bir relation'a dayanan kural tanımsızdır (L4/L6
  gereği); (2) her kural **confidence taşır** (min-t-norm, L7) — çoğu chain-axiom crisp'tir; (3) her
  türetim **zorunlu proof-trace** üretir (L8, P6/Madde VI). Bu üçü bir arada, ENS'e özgü.
- **Neden ayrı belge:** ENS-4030 *neyin yasak/zorunlu* olduğunu (Axiom) söyler; bu belge *ne yeni
  bilgi türetilebilir* olduğunu (Inference) söyler. İkisi karıştırılırsa constraint-checker ile
  inference-engine ayrımı (ENS-4030 §"Inference Rules ≠ Semantic Axioms") çöker.
- **Üst katman:** Semantic Logic (ENS-4025), Semantic Axioms (ENS-4030), Meta Model / Relation
  Registry (ENS-4001, ENS-4010).
- **Alt katman:** reasoning motoru + Validation Rules (Faz 4). Bu belge yarı-formal *niyet + kural
  spesifikasyonu*dur; makine-çalıştırılabilir semantik (SHACL/SPARQL/Datalog + t-norm motoru) Faz 4.

## Kural şeması (her IR-* bunu taşır)
```
ID · Name · Type(Composition|Transitive|Negation|Confidence) ·
Premises (ENS-4010 Registry'den tipli kenarlar) ·
Derived fact (türetilmiş — SAKLANMAZ, sorgulanır) ·
Confidence formula (varsayılan min, L7) ·
L-conformance (L1-L8) · Proof-trace template (L8, zorunlu)
```
Türetilmiş olgular **materialize edilmez**; monotonic-temporal graf (L2) yalnızca *asserted*
kenarları saklar, türetimler sorgu-zamanı üretilir ve proof-trace ile döner. Böylece "türetim
graf'a yazıldı, sonra öncül geçersizleşti, türetim öksüz kaldı" tutarsızlığı doğmaz.

---

## Kurallar

### IR-001 — Composition: `serves ⋈ supports` ⇒ `indirectly_supported_by`
- **Type:** Composition (co-target join; ENS-4010 §Relation Composition'da deklare edilmiş).
- **Premises:** `Decision --serves--> Purpose` ∧ `Capability --supports--> Purpose` (aynı Purpose).
- **Derived:** `Decision indirectly_supported_by Capability`.
- **Confidence:** `min(conf(serves), conf(supports))` (L7).
- **Proof-trace:**
  ```
  Decision-D --serves--> Purpose-P <--supports-- Capability-C
     ⇒ [IR-001 Composition: serves ⋈ supports on Purpose-P]
     ⊢ Decision-D indirectly_supported_by Capability-C   (conf = min(conf₁, conf₂))
  ```
- **Not (ENS-4025 örneğiyle uzlaşım — D-1):** ENS-4025'in tanıtım örneği zincir biçimindeydi
  (`Decision-42 --serves--> Purpose-3 --supports--> Strategy-1`). Bu örnek, ENS-4010 Relation
  Registry ile **tam örtüşmez**: (a) Registry'de `Strategy` node'u yok; (b) `supports`'un domain'i
  `{Capability, Evidence}` — `Purpose` `supports`'un domain'inde değildir, dolayısıyla
  `Purpose --supports--> ...` Registry'ce lisanslı değildir. Bu belge IR-001'i **Registry'ye sadık**
  join biçiminde tanımlar (serves ve supports aynı Purpose'a bakar). ENS-4025'in illüstrasyonu ile
  Registry arasındaki bu tutarsızlık, ya ENS-4025 örneğinin ya da Relation Registry'nin
  düzeltilmesini gerektiren gerçek bir borç → skeptic'e **D-1** olarak sunulur.

### IR-002 — Transitivity: `part_of` (Registry-lisanslı — ✅ B1 kapandı, ENS-4010 v0.5.0)
- **Type:** Transitive (ENS-4010 Registry `part_of` satırı `Trans: ✓`).
- **Kural biçimi (L-uyumlu):** `A --part_of--> B` ∧ `B --part_of--> C` ⇒ `A part_of C`,
  `conf = min(conf₁, conf₂)` (L7).
- **Registry lisansı — ✅ VAR (B1 kapandı, ENS-4010 v0.5.0).** ENS-4010 v0.5.0 `part_of`'u
  `Actor/Organization → Organization` tipler (satır 162: `Trans: ✓`, `contains` tersi, N:1).
  Böylece **`range {Organization} ⊆ domain {Actor, Organization}`** — 2-hop zincir well-formed'dir:
  `Team --part_of--> Division --part_of--> Company` (üçü de Organization) orta düğümü hem range
  (ilk hop) hem domain (ikinci hop) olarak taşıyabilir, çünkü Organization artık domain'dedir.
  Bu, ENS-4010'un v0.4.0'da eklediği **transitivity well-formedness invariant'ının** (`Trans: ✓`
  ⇒ `range ⊆ domain`; ENS-4010 satır 177-182) gereğidir ve `Team⊂Division⊂Company` 2-hop zinciri
  bizzat iz sürülerek doğrulandı. Dolayısıyla IR-002 mevcut Registry altında **lisanslı türetim
  üretir** (fire eder): örgütsel hiyerarşinin transitive kapanışı artık kural kaynaklıdır.
- **Tarihsel not (B1 borcu, ✅ kapandı):** v0.2.0'da bu kural "lisanslı DEĞİL" bayrağı taşıyordu —
  eski ENS-4010 tiplemesi `part_of`'u `Actor → Organization` (**domain ≠ range**) yapıp aynı anda
  `Trans: ✓` işaretlediği için zincir kendi içinde tutarsızdı (orta düğüm hem Organization hem Actor
  olamaz; `Organization specializes Actor` hiçbir yerde deklare değildi). Bu, D-1 ile yapısal olarak
  özdeş bir Registry kusuruydu ve SKR-031'de saptandı → ens-architect'e devredildi. **Çözüm:** ENS-4010
  v0.5.0 `part_of` domain'ini `Actor/Organization → Organization` olarak genişletti (subsumption değil
  enumerasyon — mevcut `pursues: Actor/Organization → Goal` desenine tutarlı). Düzeltme **SKR-038 +
  SKR-039 iki bağımsız skeptic turu** ile teyit edildi (survives, ENS-4010 `maturity: M2`). Borç
  kapandı; bu belge bayrağı v0.3.0'da kaldırdı.

### IR-003 — Transitivity: `specializes` (taxonomy closure)
- **Type:** Transitive (ENS-4010 Registry `specializes` satırı `Trans: ✓`).
- **Premises:** `A --specializes--> B` ∧ `B --specializes--> C` (aynı node türü — taksonomi).
- **Derived:** `A specializes C`.
- **Confidence:** `min` (L7).
- **Namespace notu (L6):** `specializes` namespace-*içi* ise Internal Relation'dır ve serbest
  zincirlenir. `ens-ent:* → ens-core:*` (cross-namespace) hâli bir **Bridge**'tir (ENS-4001
  §Semantic Connectors); o durumda zincirin cross-namespace ayağı yalnızca deklare bir Bridge ise
  geçerlidir. Bridge deklare değilse kural o ayak için **fire etmez** (tanımsız primitive'e
  dayanmaz — SKR-022 OL-logic-1 ile tutarlı).
- **Proof-trace:**
  ```
  A --specializes--> B --specializes--> C
     ⇒ [IR-003 Transitive: specializes]
     ⊢ A specializes C   (conf = min(conf₁, conf₂))
  ```

### IR-004 — Composition: `pursues ∘ refines` ⇒ `indirectly_serves`
- **Type:** Composition (head-to-tail path; Actor→Goal→Purpose).
- **Premises:** `Actor --pursues--> Goal` ∧ `Goal --refines--> Purpose`
  (`refines: Goal → Purpose`, ENS-4010 Registry).
- **Derived:** `Actor indirectly_serves Purpose` (Actor, bir Goal aracılığıyla bir Purpose'u ilerletir).
- **Confidence:** `min(conf(pursues), conf(refines))` (L7).
- **Proof-trace:**
  ```
  Actor-A --pursues--> Goal-G --refines--> Purpose-P
     ⇒ [IR-004 Composition: pursues ∘ refines]
     ⊢ Actor-A indirectly_serves Purpose-P   (conf = min(conf₁, conf₂))
  ```

### IR-005 — Confidence: Weakest-link (min-t-norm) path propagation
- **Type:** Confidence (L7'nin genel biçiminin birinci-sınıf kuralı).
- **Premises:** Uzunluğu *n* olan türetilmiş bir yol `P = e₁ ∘ e₂ ∘ … ∘ eₙ` (her `eᵢ`, herhangi
  bir Composition/Transitive kuralının bir öncül kenarı).
- **Derived:** Yolun ürettiği olgunun confidence'ı `conf(P) = min(conf(e₁), …, conf(eₙ))`.
- **Rasyonel:** min muhafazakârdır (zincir en zayıf halkası kadar güçlüdür) ve OWA (L1) altında
  bilmediğini abartmaz. **Dürüst sınır (L7 failure):** min, *bağımsız* kanıtların birleşimini
  **güçlendirmez** — iki ayrı yol aynı olguya varsa min tek başına yanlış araçtır; bu vaka IR-008'e
  ve SKR-022 OL-logic-2'ye (t-norm seçimi = RFC) devredilir.
- **Concrete instance (Registry-lisanslı yol):**
  ```
  Actor-A --pursues--> Goal-G --refines--> Purpose-P    (conf 0.9, 0.95)
     ⇒ [IR-004 → IR-005 weakest-link]
     ⊢ Actor-A indirectly_serves Purpose-P   (conf = min(0.9, 0.95) = 0.9)
  ```
  Not: örnek IR-004'ün (`pursues: Actor→Goal` ∧ `refines: Goal→Purpose`, ikisi de
  Registry-domain/range-uyumlu) yolunu kullanır. Tarihsel olarak (v0.2.0) bu seçim, IR-002'nin
  (`part_of`) çok-hop zinciri o zaman Registry-lisanslı olmadığı (B1) için yapılmıştı; **B1 artık
  kapandığından** (ENS-4010 v0.5.0, `part_of` domain-widening — SKR-038+SKR-039 ile teyit) `part_of`
  zinciri de eşit derecede lisanslı bir weakest-link illüstrasyonu olurdu (ör.
  `Team --part_of--> Division --part_of--> Company`, `conf = min(...)`). IR-004 örneği yeterli
  olduğundan olduğu gibi bırakıldı. Kural biçimi (min, herhangi uzunlukta yol) tüm
  Composition/Transitive kurallar için geneldir; yalnızca öncül kenarların Registry-lisanslı olması
  gerekir — IR-002 dâhil artık bunu sağlar.

### IR-006 — Negation (explicit): `invalidates` ⇒ Claim `Unknown` + downstream bloke
- **Type:** Negation (explicit-only, L4 — negation-as-failure YOK).
- **Premises:** `Evidence --invalidates--> Claim`, geçerlilik penceresi `[valid_from, valid_to)`
  içinde bir sorgu-anı `t` için aktif.
- **Derived / etki:**
  1. Sorgu-anı `t`'de Claim'in üç-değerli statüsü `True` iken `Unknown/Contested`'e döner (L3 —
     `Unknown ≠ False`; invalidation Claim'i *global yanlış* yapmaz, OWA/L1). Türetilmiş olgu:
     `Claim contested_by Evidence @t`.
  2. **Downstream bloke (L5 temporal scope):** `t`'de contested olan bir Claim'i öncül alan hiçbir
     Composition/Confidence kuralı `t`'de fire etmez. Böylece invalidation, türetim ağacını
     silmeden (L2 monotonic — hiçbir asserted kenar silinmez, yalnızca `valid_to` kapanır)
     *sorgu-zamanı* etkiler.
- **Confidence:** `conf(invalidates)` (contested-olgusunun confidence'ı).
- **Temporal (L2/L5, MC-004 uyumu):** Bir Evidence bir Claim'i `t₁`'de support edip `t₂ > t₁`'de
  invalidate edebilir; çelişki yalnızca **aynı anda** support+invalidate ise doğar (o vaka
  ENS-4030 MC-004'ün işidir, IR-006'nın değil).
- **Proof-trace:**
  ```
  Evidence-E --invalidates--> Claim-K   (valid @t, conf 0.85)
     ⇒ [IR-006 Explicit-negation: invalidates]
     ⊢ Claim-K contested_by Evidence-E @t   (conf = 0.85; statü: True → Unknown)
     ⊢ Claim-K'yı öncül alan türetimler @t bloke
  ```

### IR-007 — Negation (explicit): `contradicts` (symmetric) ⇒ Inconsistency flag
- **Type:** Negation (explicit-only, L4).
- **Premises:** `Claim-A --contradicts--> Claim-B` (ENS-4001 Meta Model'de `contradicts`
  **symmetric**), her ikisi de sorgu-anı `t`'de geçerli (`True`).
- **Derived:** `Inconsistency(A, B) @t` — bir tutarlılık bayrağı (fact değil, uyarı). Bu, ENS-4030
  MC-004/MC-005 (eşzamanlı çelişki, döngüsel gerekçe) validation'ını *besler* ama onun yerine
  geçmez (Inference ≠ Validation).
- **Confidence:** `min(conf(A geçerliliği), conf(B geçerliliği), conf(contradicts))` (L7).
- **Namespace notu (L6):** `contradicts` ENS-4001'de `Concept → Concept` (meta/genel) bir
  relation'dır. İki `ens-core:Claim` arasında namespace-içidir → serbest. Ancak `ens-meta:*` ile
  `ens-core:*` arasında bir contradiction ancak deklare bir **Bridge** üzerinden ifade edilebilir
  (MC-007 / L6); Bridge yoksa IR-007 cross-namespace fire etmez.
- **Proof-trace:**
  ```
  Claim-A <--contradicts--> Claim-B   (symmetric, ikisi @t geçerli)
     ⇒ [IR-007 Explicit-negation: contradicts]
     ⊢ Inconsistency(A, B) @t   (conf = min(...); → MC-004/MC-005 validation'a beslenir)
  ```

### IR-008 — Confidence: Çoklu-türetim uzlaşımı (multi-derivation reconciliation)
- **Type:** Confidence (L7'nin açık kenarı).
- **Premises:** Aynı türetilmiş olgu `F`, ≥2 bağımsız proof `P₁, P₂, …` tarafından üretilir
  (her `Pᵢ` kendi içinde IR-005 ile min).
- **Derived:** `F`, birden çok proof-trace ile döner. **Proof-içi** confidence min'dir (L7); ancak
  **proof-*arası*** birleştirme (bağımsız kanıtı güçlendirme) `min`'in yapamadığı şeydir.
- **Bilinçli AÇIK karar (SKR-022 OL-logic-2):** Proof-arası aggregation fonksiyonu
  (`max` / probabilistic-sum `a+b−ab` / Bayesian) bu belgede **kararlaştırılmaz**; bir RFC'ye
  bırakılır. Şimdilik IR-008'in davranışı: her proof ayrı proof-trace ile sunulur, en yüksek
  proof-içi confidence *raporlanan* değer olur, ancak "bağımsız kanıtla güçlendi" iddiası
  **yapılmaz** (OWA/P6 — bilmediğini abartma). Bu, dürüst-eksik bir kuraldır; RFC gelene dek
  muhafazakâr.
- **Proof-trace:**
  ```
  F  ⟵ P₁ [IR-001] (conf 0.7)
  F  ⟵ P₂ [IR-004] (conf 0.6)
     ⇒ [IR-008 Multi-derivation]
     ⊢ F   (reported conf = 0.7; birleşim-güçlendirme: TANIMSIZ — RFC bekliyor)
  ```

---

## L1-L8 uyum matrisi (her kural sözleşmeyi ihlal etmemeli)
Aşağıdaki tablo, her kuralın ENS-4025'in sekiz taahhüdüne uyumunu doğrular. `—` = kural için
doğrudan ilgili değil ama ihlal de etmez.

| Kural | L1 OWA | L2 mono-temporal | L3 3-değer | L4 explicit-neg | L5 temporal-scope | L6 namespace | L7 confidence-min | L8 proof-trace |
|-------|--------|------------------|-----------|-----------------|-------------------|--------------|-------------------|----------------|
| IR-001 | ✓ (yokluk≠yanlış) | ✓ (türetim saklanmaz) | ✓ | — | ✓ | ✓ (intra-core) | ✓ min | ✓ |
| IR-002 † | ✓ | ✓ | ✓ | — | ✓ | ✓ | ✓ min | ✓ |
| IR-003 | ✓ | ✓ | ✓ | — | ✓ | ✓ (Bridge şartı) | ✓ min | ✓ |
| IR-004 | ✓ | ✓ | ✓ | — | ✓ | ✓ | ✓ min | ✓ |
| IR-005 | ✓ | ✓ | ✓ | — | ✓ | ✓ | ✓ min (**tanım**) | ✓ |
| IR-006 | ✓ (Unknown≠False) | ✓ (silme yok, valid_to) | ✓ (True→Unknown) | ✓ (**invalidates**) | ✓ (downstream blok) | ✓ | ✓ | ✓ |
| IR-007 | ✓ | ✓ | ✓ | ✓ (**contradicts**) | ✓ | ✓ (Bridge şartı) | ✓ min | ✓ |
| IR-008 | ✓ | ✓ | ✓ | — | ✓ | ✓ | ✓ (proof-içi; arası açık) | ✓ (çoklu) |

**† IR-002 (✅ B1 kapandı, ENS-4010 v0.5.0):** L1-L8 uyumu (kural *biçimi*) her zaman temizdi;
v0.2.0'da eksik olan tek şey öncül kenarın (`part_of` çok-hop zinciri) Registry-lisansıydı. ENS-4010
v0.5.0 `part_of` domain'ini `Actor/Organization → Organization` genişleterek (`range ⊆ domain`) bu
lisansı verdi → IR-002 artık **hem L-uyumlu hem Registry-lisanslı**, lisanslı türetim üretir. L-uyumu
≠ Registry-lisansı ayrımı hâlâ geçerlidir (kavramsal), ama IR-002 için ikisi de artık sağlanır. Bkz.
IR-002 gövdesi ve §Failure conditions B1. (Teyit: SKR-038+SKR-039, ENS-4010 M2.)

**Doğrulama sözü:** Bu tabloyu ihlal eden bir kural (ör. proof-trace üretmeyen), ya da öncülü
ne ENS-4010 Registry'de ne ENS-4001 Meta Model'de gerçek/tipli olmayan bir kural bu belgeye
**giremez** — ENS-4025 §"Aksi kural geçersizdir". Öncülü gerçek ama Registry-tiplemesi zincire
lisans **vermeyen** bir kural belgede **kalır ama "lisanslı değil" bayrağı taşır** ve üretim yapmaz.
Bu disiplin geneldir; IR-002 v0.2.0'da bu durumun örneğiydi (B1) — ama ENS-4010 v0.5.0'daki `part_of`
domain-widening ile lisans kazandı, dolayısıyla artık bayraksızdır ve üretim yapar. Bayrak-mekanizması
gelecekteki başka lisanssız-zincir vakaları için yürürlükte kalır.

## Implications (bu kurallar neyi mümkün kılar)
- **Explainable derived reasoning:** "Decision-D bu Capability tarafından mı destekleniyor?" gibi
  sorulara, kural-kimlikli + confidence'lı bir proof-trace ile yanıt (Decision Intelligence
  vizyonu, ENS-4025 §Proof trace).
- **ADR-0001 temeli:** ADR-0001'in `indirectly_supports` türü iddialarının artık *resmî* bir kural
  kaynağı (IR-001) var — daha önce örtük varsayımdı (ROADMAP G-14 gerekçesi).
- **Validation'ı besler ama değiştirmez:** IR-006/IR-007 çıktısı ENS-4030 MC-004/MC-005'e girdi
  olur; inference-engine ≠ constraint-checker ayrımı korunur.

## Relationships (Külliyat bağları)
- **ENS-4025 (Semantic Logic):** Bu belgenin **sözleşmesi**; her kural L1-L8'e uymalı.
- **ENS-4030 (Semantic Axioms):** Kardeş belge — *doğruluk* (Axiom) vs *yeni-bilgi* (Inference).
  IR-006/007 çıktıları SC/MC validation'a beslenir.
- **ENS-4010 (Relation Registry):** Kuralların öncül kenarlarının **birincil kaynağı** — bir
  kuralın öncül relation'ı ya bu Registry'de ya da ENS-4001 Meta Model'de (aşağı) gerçek ve
  tipli olmalıdır; bu iki kaynağın **dışında** hiçbir relation'a dayanılamaz (uydurma yasak, L4/L6).
- **ENS-4001 (Meta Model):** `contradicts` (symmetric), Semantic Connector/Bridge tanımları
  (L6/IR-003/IR-007), temporal model (valid_from/valid_to; IR-006).

## SKR-031'e yanıt (v0.1.0 → v0.2.0)
SKR-031 verdict'i **wounded** idi (yöntem/atıflar sağlam, ama iki gerçek çatlak). Her talep
tek tek karşılandı:

| # | SKR-031 talebi | Yanıt (v0.2.0) | Durum |
|---|----------------|----------------|-------|
| 1 | **B1** — `part_of` transitive kusurunu D-1 ile aynı statüye çıkar: `failure_condition` + ENS-4010 owner'a devir; IR-002/IR-005 "lisanslı değil" notu taşısın. | IR-002 "Registry-bağımlı — henüz lisanslı DEĞİL" başlığıyla yeniden yazıldı; geçersiz "Agent-profili" savunması çıkarıldı, tutarsızlık açıkça anlatıldı, illicit çok-hop iddiası kaldırıldı. IR-005 concrete instance Registry-lisanslı IR-004 yoluna (`pursues ∘ refines`) çevrildi. §Failure conditions'a **B1** eklendi (ens-architect'e devir). L1-L8 matrisine † bayrağı + açıklama. **✅ v0.3.0: üst-borç ens-architect tarafından ENS-4010 v0.5.0'da kapatıldı** (`part_of` domain `Actor/Organization → Organization`; SKR-038+SKR-039 iki bağımsız tur, survives/M2) → IR-002/IR-005-part_of artık lisanslı; bayrak kaldırıldı. | **✅ Tam kapandı** (upstream ENS-4010 v0.5.0) |
| 2 | **B2** — "her kural ENS-4010 Registry'ye dayanır" tekil-kaynak iddiasını yumuşat (IR-007 `contradicts` ENS-4001'de). | Header blockquote + Relationships §ENS-4010: "ENS-4010 Registry **veya** ENS-4001 Meta Model'deki gerçek/tipli relation" olarak düzeltildi. §Failure conditions'a **B2** notu. | **Kapatıldı** |
| 3 | **D-1'i doğru adrese yönlendir** — D-1 ENS-4025'in borcudur; ENS-4025 örneğini Registry'ye hizala; ROADMAP/G-14'te sahiplik netleşsin. | ENS-4025 v0.1.1'de proof-trace örneği co-target join'e hizalandı (Strategy → Capability/Purpose), SKR-031 D-1 kaynak notu eklendi. Bu belgenin D-1 failure-condition'ı "ENS-4025 tarafında kapatıldı" olarak güncellendi. ROADMAP G-14 güncellendi. | **Kapatıldı** |
| 4 | **(İsteğe bağlı)** `indirectly_serves` isim karışması + IR-005/IR-008 meta-kural doğası. | Kabul edildi ama küçük/stilistik; `indirectly_serves`'in `serves` (Decision→Purpose) semantiğini Actor→Purpose'a taşıdığı IR-004 gövdesinde zaten açık. ENS-4000 sözlüğüne türetilmiş-relation kaydı ayrı iş (ROADMAP). Bu turda dokunulmadı — dürüstçe açık. | Ertelendi (küçük) |

**Not:** Bu tablo yazarın öz-beyanıdır; talep 1-3'ün *gerçekten* kapandığı kararı ens-skeptic'in
bağımsız 2. turuna aittir (governance G2: yazar kendi işini kanıtlayamaz). `survives`/`ratified`
**iddia edilmiyor.**

## Failure conditions (Anayasa Madde X)
- **D-1 — ENS-4025 örneği ile Registry çelişkisi (ENS-4025 tarafında KAPATILDI, v0.1.1).**
  IR-001'in ilham örneği (ENS-4025: `Purpose --supports--> Strategy`) mevcut Relation Registry ile
  lisanslı değildi (`Strategy` node yok; `supports`'un domain'i Purpose'u, range'i Strategy'yi
  içermez). Konum SKR-031 ile netleşti: bu **ENS-4025'in kusuruydu, ENS-4031'in değil** (SKR-022
  ratified turunda kaçmış). Registry-önceliği (Madde XII) gereği düzeltme yönü = ENS-4025 örneğini
  Registry'ye hizalamak; ENS-4025 v0.1.1'de bunu yaptı (co-target join: `serves` + `supports` aynı
  Purpose'a). Bu belgenin IR-001'i zaten Registry-sadık join biçiminde tanımlıydı — değişmedi.
- **B1 — `part_of` transitive tiplemesi Registry'ce lisanslı değildi (✅ KAPANDI, ENS-4010 v0.5.0).**
  *Tarihsel tanı:* IR-002/IR-005'in `part_of` çok-hop zinciri, ENS-4010'un eski `Actor → Organization`
  (domain≠range) tiplemesi altında lisanslı değildi: 2-hop zincir gizlice `Organization --part_of-->
  Organization` iddia ediyordu, bu kenar Registry'de yoktu; `Trans: ✓` + §Composition deklarasyonu
  bu relation için **kendi içinde tutarsızdı**. Bu D-1 ile aynı sınıf kusurdu (SKR-031'de saptandı).
  En güçlü hâliyle: örgütsel hiyerarşi (Team⊂Division⊂Company) *gerçekten* var olan bir olgudur, o
  yüzden çözüm Registry'yi genişletmekti — ama bu **ens-architect'in yetkisiydi** (ens-philosopher
  başka owner'ın Registry'sini tek taraflı değiştiremez), bu yüzden devredildi.
  *Çözüm (2026-07): ENS-4010 v0.5.0* `part_of` domain'ini `Actor/Organization → Organization` olarak
  genişletti (subsumption değil enumerasyon — `pursues` desenine tutarlı, daha az ontolojik ağırlık).
  Artık `range {Organization} ⊆ domain {Actor, Organization}` → zincir well-formed; bu ENS-4010'un
  transitivity well-formedness invariant'ının (`Trans: ✓` ⇒ `range ⊆ domain`) gereğidir ve
  `Team⊂Division⊂Company` iz sürülerek doğrulandı. Düzeltme **SKR-038 + SKR-039 iki bağımsız skeptic
  turu** ile teyit edildi (survives, ENS-4010 `maturity: M2`). Sonuç: IR-002/IR-005-part_of **artık
  Registry-lisanslıdır ve lisanslı türetim üretir**; v0.3.0'da bayrak kaldırıldı. Bu bir *failure
  condition* olmaktan çıkıp *çözülmüş borç* kaydına dönüştü; tarihsel referans (SKR-031→SKR-038/039)
  korunur.
- **B2 — birincil-kaynak ifadesi yumuşatıldı.** "Her kural ENS-4010 Registry'ye dayanır" tekil-
  kaynak sözü fazla genişti: IR-007'nin öncülü `contradicts` ENS-4001 Meta Model'de yaşar
  (ENS-4010 Registry'de değil). İfade "ENS-4010 Registry **veya** ENS-4001 Meta Model'deki gerçek,
  tipli relation" olarak düzeltildi (header + Relationships). Kusur framing'deydi, atıfta değil —
  IR-007 gövdesi `contradicts`'in ENS-4001 kaynağını zaten dürüstçe belirtiyordu.
- **Confidence proof-arası birleştirme tanımsız (IR-008).** Bağımsız kanıtın confidence'ı nasıl
  birleşir sorusu bir RFC'ye (SKR-022 OL-logic-2) ertelendi; o gelene dek IR-008 muhafazakâr ama
  *eksik* — bir sistem bağımsız kanıtı yanlışlıkla güçlendirmemeli, ama gerçekten güçlenen kanıtı
  da olduğundan zayıf raporlar. min'in bilgi-kaybı sınırı (ENS-4025 L7 failure) burada canlı.
- **Kural kümesi tam değil (kasıtlı).** Bu ilk taslak yalnızca Registry'nin en açık
  composition/transitive/negation vakalarını kapsar; `stores/retrieves/updates` (Memory),
  `produces/changes/has_state` (Event/State), `constrains` propagation gibi diğer relation'lar için
  kural **henüz yok**. Anti-ossification: eksik kurallar RFC/skeptic turlarıyla eklenir,
  spekülatif olarak doldurulmaz.
- **Materialize-etmeme varsayımı (L2 ile gerilim).** "Türetim saklanmaz, sorgulanır" kararı graf-
  büyümesini önler ama sorgu-zamanı maliyeti (transitive closure hesabı) Faz 4'te ölçek sorunu
  olabilir; caching yaparsa L2 (materialize edilen türetim öksüzleşebilir) ile gerilime girer.
  Bu bir *engineering* borcudur (Faz 4), semantik değil.
- **Formal dil Faz 4.** Kurallar yarı-formal (Türkçe + notasyon). Makine-çalıştırılabilir hâl
  (SHACL property-path / SPARQL / Datalog + t-norm motoru) Faz 4'te bağlanır; şimdilik niyet +
  spesifikasyon. Skeptic doğrulaması olmadan hiçbir kod bu kurallara dayanmamalı (`canon: false`).
- **Skeptic iki turu geçildi; ratified değil (`M0`).** SKR-031 (1. tur) **wounded** verdi; B1/B2
  v0.2.0'da düzeltildi. SKR-032 (bağımsız 2. tur) **survives** → `status: skeptic-cleared`. B1'in
  *upstream* borcu (ENS-4010 `part_of` tiplemesi) SKR-032 turunda "açık ama bloke etmez" olarak
  kaydedilmişti; v0.3.0'da o borç da kapandı (ENS-4010 v0.5.0, SKR-038+SKR-039). Bu belge yine de
  **ratified değildir** — `ratified`/`canon: true` ayrı governance edimidir (skeptic vermez). L1-L8
  uyum iddiası hâlâ yazar beyanıdır. **Dürüst not (self-review yasağı, G2):** v0.3.0 IR-002/IR-005'in
  artık lisanslı *üretim* yaptığı yeni-olumlu bir davranış getirir; kural *biçimi* SKR-031/032'de
  zaten temizlenmişti ve Registry-lisansı ENS-4010 tarafında SKR-038+SKR-039 ile bağımsız doğrulandı,
  bu yüzden bu bir riskli yeni-iddia değil kapanmış-borç propagasyonudur — ama IR-002'nin canlı üretim
  davranışının ENS-4031 bağlamında ayrıca teyidi gelecek bir skeptic turunun işidir; bu belge kendi
  işini `survives` ilan etmez. `maturity` M0 korunur (bu düzeltme yeni ampirik kanıt eklemez).

---

*Inference Rules, Semantic Logic'in soyut sözleşmesini yürüyen kurallara çevirir: her çıkarım bir
kimlik, bir confidence ve bir proof-trace taşır; her öncül ya Relation Registry'de (ENS-4010) ya da
Meta Model'de (ENS-4001) gerçek, tipli bir kenardır. Registry-tiplemesi bir zincire lisans
vermeyen herhangi bir kural "lisanslı değil" bayrağıyla açıkça dururdu; IR-002/part_of v0.2.0'da bu
durumdaydı (B1) ama ENS-4010 v0.5.0 ile lisans kazandı ve bayraksızdır. Örtük mantık değil —
adlandırılmış, izlenebilir, muhafazakâr türetim.*
