---
id: SKR-021
type: skeptic-review
validation_dimension: ontology
origin: ENS-4030
depends_on: [ENS-4030, ENS-4010]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-021 — Semantic Axioms (ENS-4030) Ontology Validation

## Verdict
**survives → M2 (Reviewed).** Belge, tip sisteminden türeyen resmi bir semantik spesifikasyon
olarak sağlam: üç katman (Structural/Semantic/Evolution) doğru ayrışmış, her constraint tipli,
Structural katman Profiles'tan **üretim kuralıyla** bağlı (single-source korunmuş), MC-004
temporal model'e karşı doğru kapsanmış, ve Axiom/Inference ayrımı net. "Axiom" adının dürüst
kalibrasyonu (çoğu türetilir, katı aksiyom değil) takdir edilir. Kalan noktalar Faz 4 tooling ve
bir küçük namespace netliği.

## Doğrulama
- **Structural ↔ single source — GEÇTİ.** SC katmanı "her profil-zorunlu-ilişki → bir SC" üretim
  kuralına bağlı; elle-seçilmiş liste değil. Doğru yaklaşım.
- **Temporal tutarlılık — GEÇTİ.** MC-004, Meta Model valid_from/valid_to ile uyumlu: support→
  invalidate *zamanla* serbest, yalnızca eşzamanlı çelişki yasak. İyi yakalanmış.
- **Axiom/Inference ayrımı — GEÇTİ.** Constraint (ne yasak/zorunlu) ile Inference (ne türetilir)
  ayrı; reasoning motoru için doğru zemin.
- **Evolution katmanı — GEÇTİ ve değerli.** EC-005 (kapanış sessizce azalamaz) derin bir
  longevity invariant'ı; Governance/Versioning ile bağı doğru kurulmuş.

## Bulgular (obligation — bloke etmez)
- **OA1 — `measures` range namespace belirsizliği.** Relation Registry `measures: Metric →
  Concept/State` diyor; "Concept" `ens-meta:` mi `ens-core:` mi? MC-002 (Purpose ölçülemez)
  bununla kısmen çakışıyor olabilir (range zaten Purpose'u dışlıyorsa MC-002 redundant). ENS-4010
  `measures` range'i namespace ile netleştirilmeli; MC-002'nin redundant olup olmadığı kontrol
  edilmeli.
- **OA2 — Predicate formal dili Faz 4.** Predicate'ler yarı-formal (Türkçe+notasyon);
  makine-çalıştırılabilir SHACL/SPARQL/Datalog karşılığı Faz 4'te üretilecek. Şimdilik insan+niyet.
- **OA3 — Consistency-check Faz 4.** Axiom'ların birbirini çürütmediğinin *mekanik* kanıtı
  formal-checker ister; şimdilik elle tutarlı.
- **OA4 — EC runtime enforcement Governance'ta.** Evolution constraint'leri statik değil;
  supersede/deprecate işlemlerinde çalışmalı — bu bağ Governance (capability-matrix) tarafında
  somutlaşmalı.

## Sonuç
Semantic Axioms **M2**. Zincir ilerleyebilir: `Ontology → Semantic Axioms ✓ → Inference Rules
(sırada) → Validation Rules → Architecture`. Validation Rules artık Registry+Profiles+Axioms'tan
üretilebilir. M3 için ≥2 downstream kullanım (Inference Rules, generated Validation Rules, mimari); M4+ Faz 4.
