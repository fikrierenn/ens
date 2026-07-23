---
id: SKR-017
type: skeptic-review
validation_dimension: ontology
origin: ENS-4001
depends_on: [ENS-4001]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-017 — ENS Meta Model (ENS-4001) Meta-Ontology Validation

> Bu **Scientific Skepticism değildir**; Meta-Ontology Validation'dır (validation-framework.md).
> 8 kontrol: node/edge completeness, directionality, cardinality, temporal, identity, semantics,
> closure.

## Verdict
**wounded.** Meta Model doğru fikir ve reference/knowledge graph ayrımı sağlam. Ama bir şema
olarak **eksik**: birkaç node ve edge yok, cardinality/temporal/identity/directionality hiç
tanımlı değil, iki edge semantik olarak örtüşüyor ve **closure testi geçmiyor** (depodaki bazı
gerçek yapıtlar Meta Model'le ifade edilemiyor). Governance bu şemanın üstüne oturacağına göre,
bunlar giderilmeden M2 olamaz.

## 1. Node completeness — EKSİK
Var olanlar iyi. Eksik ve **ENS-artifact** düzeyinde gerekli olanlar:
- **Metric** — tanımlı bir ölçü (ör. `H(A|C)` Decision Entropy bir Metric'tir; şu an "Law" ile
  karışıyor). Law ≠ Metric.
- **Hypothesis** — kanıt bekleyen, henüz Claim olmamış test edilebilir önerme (Reasoning Engine
  bunları üretir).
- **Constraint** — sınırlayıcı kural (Anayasa kısıtları, invariant'lar).
- **Philosophy** — Manifesto (ENS-1000) hiçbir node türüne oturmuyor; "Theory" değil.
- **Index/Manifest** — REGISTRY.md, KULLIYAT.md hiçbir türe girmiyor (closure sorunu, §8).

**Doğru dışarıda bırakılanlar:** Actor, Organization, Capability — bunlar **domain ontolojisidir**
(kurumsal varlıklar), ENS-artifact meta-modeli değil. Bu sınır korunmalı (aksi hâlde meta-model
domain'e taşar). Observation/Dataset → Faz 4 (Experiment ile gelir), şimdilik ertelenebilir.

## 2. Edge completeness — EKSİK
Var olan 12 iyi. Eksik ve gerekli olanlar:
- **specializes / generalizes** — taksonomi (Purpose-tipi taksonomisi OM2 tam bunu ister!).
- **invalidates** — Evidence bir Claim'i *çürütür* (yanlışlanabilirlik için `supports`'un karşıtı;
  Madde X'in kalbi, şu an ifade edilemiyor).
- **requires** — bir yapıt başkasını önkoşul kılar (`depends_on`'dan güçlü: zorunluluk).
- **causes** — nedensel iddia (attribution/R2 nedensel kenarı; measures'tan farklı).
- **owned_by** — Decision → Owner; Agent sorumluluğu (P7).
- **version_of** — bir sürümün öncekiyle ilişkisi (identity ile bağlı, §6).

## 3. Directionality — TANIMSIZ
Hiç belirtilmemiş. Çoğu kenar yönlüdür (`implements`, `derived_from`). Ama **`contradicts`
simetriktir** (A contradicts B ⇒ B contradicts A) — belirtilmeli. Her kenar için
directed/symmetric etiketi gerekir.

## 4. Cardinality — TANIMSIZ (kritik)
Hiç yok. Örn: `Theory contains Concept` = 1:N mi N:N mi? `Concept implements Principle` = N:N.
`Module implements ADR` = N:1? Her kenar bir cardinality (1:1 | 1:N | N:N) taşımalı; yoksa graf
kısıtları uygulanamaz.

## 5. Temporal model — EKSİK
`supersedes` doğası gereği zamansaldır; `supports`/`invalidates` zamanla değişebilir (bugün
destekleyen kanıt yarın çürütülebilir). Graf zamansal geçerlilik (`valid_from`/`valid_to`)
taşımıyor. Bilgi yaşayan bir ağsa (ENS felsefesi), zaman birinci-sınıf olmalı.

## 6. Identity — TANIMSIZ
Node ID değişmez mi? Sürüm nasıl? REGISTRY "numara yeniden kullanılmaz, superseded korunur"
diyor ama Meta Model bunu formelleştirmiyor: **node id immutable; içerik `version_of` ile
sürümlenir; supersede id'yi korur.** Bu kural şemaya girmeli.

## 7. Semantics — ÖRTÜŞME
`implements` ve `realizes` örtüşüyor. Ayrım net değil. Öneri: **`realizes` = soyuttan tasarıma**
(Theory → ADR), **`implements` = spec'ten yürütmeye** (ADR → Module). İkisi farklı seviyeler;
tanım keskinleştirilmeli, yoksa biri gereksiz.

## 8. Closure — GEÇMİYOR (en önemli)
Test: depodaki HER yapıt Meta Model ile ifade edilebiliyor mu?
- Manifesto (ENS-1000) → node türü yok (**Philosophy** eksik).
- REGISTRY/KULLIYAT → node türü yok (**Index/Manifest** eksik).
- Anayasa (ENS-0000) → "Principle" içeren bir konteyner ama kendi türü yok (**Constitution/
  container** belirsiz).
Closure geçmediği için Meta Model şu an **eksik** — governance'ın dayanacağı taban tam değil.

## Talepler (M2 için)
1. Node ekle: Metric, Hypothesis, Constraint, Philosophy, Index. Domain node'ları (Actor/Org/
   Capability) açıkça *domain ontolojisine* havale et.
2. Edge ekle: specializes, generalizes, invalidates, requires, causes, owned_by, version_of.
3. Her kenara **directionality** (directed/symmetric) ve **cardinality** ekle.
4. **Temporal model** ekle (kenarlarda valid_from/valid_to).
5. **Identity kuralları** ekle (immutable id, version_of, supersede id-koruma).
6. `implements` vs `realizes` semantiğini keskinleştir.
7. **Closure**'ı geç: her mevcut yapıt bir node türüne otursun.

## Not
Governance, bu 7 talep karşılanıp Meta Model M2 olduktan sonra türetilmeli — validated şema
üstüne, tersine değil.
