---
id: SKR-019
type: skeptic-review
validation_dimension: ontology
origin: ENS-4010
depends_on: [ENS-4010]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-019 — Foundational Ontology (ENS-4010) Ontology Validation

> Scientific değil, Ontology Validation. Kontroller: node completeness, relation completeness,
> template completeness, level-distinction, forbidden-relations, closure.

## Verdict
**wounded.** İki seviye ayrımı (Meta Model / Foundational) ve Purpose/Constraint'i node yapmak
doğru ve güçlü. Ama ontoloji bir **şema** olarak eksik: node şablonu çoğu düğümde tamamlanmamış,
**kullanılan ilişkilerin çoğu declared değil** (relation set eksik), ve birkaç node (özellikle
Attention) muhtemelen eksik. Validation Rules bu ontolojiden türeyeceğine göre, eksik ontoloji
= eksik Linter. Giderilmeden M2 olamaz.

## 1. Relation completeness — EKSİK (en ağır)
Declared foundational relations ~11 (serves, supports, constrains, pursues, requires, has_state,
produces, measures, part_of, owns, specializes/generalizes). Ama tablolarda **declared olmayan**
ilişkiler kullanılıyor: `has_context`, `stores`, `retrieves`, `updates`, `changes`, `contains`,
`refines`, `constrained_by`, `derived_from`, `invalidates`. Meta Model'de yaptığın gibi **her
ilişki domain→range + yön + cardinality ile declared olmalı**; yoksa Linter tanımsız-kenarı
yakalayamaz (kendi kuralın). Şu an ontoloji kendi undefined-relation kuralını ihlal ediyor.

## 2. Template completeness — EKSİK
Yalnızca 3 node (Decision, Purpose, Constraint) tam 7-parça. 13 node concise (Identity,
Lifecycle, Examples, Counter-Examples yok; Forbidden kısmi). **Forbidden Relations'ın gücü
tamlığa bağlı** — Linter yalnızca yazılmış forbidden'ları uygular. En az Forbidden Relations
her node için tamamlanmalı.

## 3. Node completeness — muhtemel eksik
- **Attention** — P5'in kıt kaynağı; Decision Gravity (ENS-3022) attention *tahsis eder* ama
  Attention bir node değil. "AttentionPriority Decision'a atanır" ifade edilemiyor. Resource'un
  bir alt-türü mü, ayrı node mu? Karara bağlanmalı.
- **Reasoning / Hypothesis sınırı:** Reasoning bir Capability mi, node mu? Hypothesis meta-seviye
  (ENS-4001) ama nesne-seviyede "bir karar bir Hypothesis test eder" gerekebilir. Netleştir.
- Trust (Trust Coefficient, sözlükte provisional) — foundational node mu? Şimdilik dışarıda,
  ama gerekçelendir.

## 4. Level-distinction — DOĞRU ama homonim stratejisi zayıf
Meta/nesne ayrımı sağlam. Ama Evidence/Claim/Constraint/Metric homonimlerinde strateji "karıştırma
yasak" demekle kalıyor. Daha güçlüsü: **nesne-seviye adları farklılaştır** (ör. `DecisionClaim`,
`DecisionEvidence`) ya da namespace (`obj:Claim` vs `meta:Claim`). Aksi hâlde Linter iki seviyeyi
ayırt edemez.

## 5. Closure — KISMEN
"Decision serves Purpose", "Actor pursues Goal", "Capability supports Purpose" ifade edilebiliyor
✓. Ama "Attention allocated to Decision" (Gravity) ve "Metric measures Decision-consistency"
(Entropy) tam oturmuyor (Attention node yok; measures'ın range'i belirsiz). Nicel yasaların
(3021-3023) tam ontolojik zemini eksik.

## Talepler (M2 için)
1. **Tüm foundational relations'ı declare et** (domain→range, yön, cardinality) — tablolarda
   kullanılan her ilişki dahil.
2. **Her node için en az Forbidden Relations'ı tamamla** (Linter gücü); ideali tam 7-parça.
3. **Attention'ı node olarak ekle** (P5; Gravity onu gerektiriyor); Reasoning/Hypothesis/Trust
   sınırını gerekçelendir.
4. **Homonim stratejisini güçlendir** (namespace ya da ayrı ad).
5. **Closure'ı nicel yasalara kadar geçir** (Attention + measures range'i).

## Not
Validation Rules ve Semantic Constraints bu ontolojiden türeyecek; bu yüzden ontoloji tam
olmadan bir sonraki katmana geçilmemeli (senin zincirin: Ontology → Semantic Constraints →
Validation → Architecture).
