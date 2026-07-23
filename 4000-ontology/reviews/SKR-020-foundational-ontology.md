---
id: SKR-020
type: skeptic-review
validation_dimension: ontology
origin: ENS-4010
depends_on: [ENS-4010, SKR-019]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-020 — Foundational Ontology v0.2 İkinci Ontology Validation

## Verdict
**survives → M2 (Reviewed).** v0.2, SKR-019'un 5 bulgusunu tek tek yamamak yerine kök nedeni
çözdü: ontoloji artık bir **semantik tip sistemi.** Node/Relation Registry first-class, Semantic
Profiles SHACL-benzeri, namespace homonimi kesiyor, Semantic Closure Principle'a kadar. Prior-art
(OWL/RDFS/SHACL/DL) dürüstçe kabul edildi. Kalan noktalar Faz 4 tooling'e devir. **Bir sonraki
katman (Semantic Constraints) bu validated tip sisteminden türeyebilir.**

## Bulgu-bulgu doğrulama
1. **Relation completeness — GEÇTİ.** Relation Registry: domain→range, yön, cardinality, inverse,
   transitivity, symmetry, default status. Belge-içi ad-hoc kullanım artık Registry'ye tabi.
2. **Template — GEÇTİ.** Semantic Profiles (Deliberative/Intent/Rule/Store/Assertion/Agent/
   Resource/Measure/Temporal) node→profil eşlemesiyle; validator profile üzerinden çalışır.
3. **Attention — GEÇTİ.** Node Registry'de (Resource profili); `consumes`/`allocated_to`
   deklare edildi. Decision Gravity (ENS-3022) artık ontolojik zemine oturuyor.
4. **Namespace — GEÇTİ.** ens-meta/ens-core/ens-ent; `ens-meta:Claim` ≠ `ens-core:Claim`.
   Homonim mekanik olarak çözüldü.
5. **Closure — GEÇTİ.** Her node Principle/Law/Theory reachability taşıyor; orphan = Linter reddi.
6. **(+ Discouraged)** Allowed/Discouraged/Forbidden üç kademe eklendi — Metric `supports`
   Decision = Discouraged örneği isabetli.
7. **(+ Composition)** part_of/specializes transitive, serves∘supports declare edildi.

## Kalan riskler (Faz 4 tooling'e devir)
- **OF1 — Profil/closure otomasyonu Faz 4.** Şu an örnek closure'lar elle; tam otomatik kapanış
  KG-sorgusu ister. O zamana dek elle bakım riski.
- **OF2 — Composition inference Faz 4.** Zincir kuralları declare edildi ama *çalıştırılmıyor*
  (aşırı-modelleme'den kaçınma; doğru).
- **OF3 — Registry enforcement Ontology Linter'a bağlı** (formal-checker inşa edilmeli); o
  olmadan tip sistemi disipline dayanır.

## Prior-art dürüstlüğü
Tip sistemi özgün değil (OWL/SHACL/DL); ENS bunu kabul ediyor. Özgün olan yalnızca
Principle-closure ve Meta Model bağı — dar ama gerçek delta. Bu dürüstlük doğru.

## İç tutarlılık
Node Registry ↔ Profiles ↔ Relation Registry ↔ Closure birbirini tutuyor. Namespace, Meta
Model seviye-ayrımıyla uyumlu. Çelişki yok.

## Sonuç
Foundational Ontology **M2**. Validation Rules ve Semantic Constraints artık bu tip sisteminden
*türetilebilir* — senin zincirin: Ontology → **Semantic Constraints** → Validation → Architecture.
M3 için ≥2 downstream yapıt (Semantic Constraints, Enterprise Ontology, mimari) onu kullanınca; M4+ Faz 4.
