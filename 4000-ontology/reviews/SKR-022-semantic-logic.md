---
id: SKR-022
type: skeptic-review
validation_dimension: ontology
origin: ENS-4025
depends_on: [ENS-4025, ENS-4010, ENS-4030]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-022 — Semantic Logic (ENS-4025) Ontology Validation

## Verdict
**survives → M2 (Reviewed).** Belge tam da gerekeni yapıyor: Inference'ın örtük mantık
varsayımını açık bir sözleşmeye çeviriyor. 8 taahhüt (L1-L8) ilkelere bağlı ve tutarlı;
özellikle L7 (confidence propagation, P6) ve L8 (zorunlu proof-trace, Madde VI) ENS'in ayırt
edici, savunulabilir katkıları. Prior-art (DL/OWL/Datalog/SHACL/MLN/TMS) dürüstçe konumlanmış;
delta = bileşim + P6-bağı. 5-başlık disiplini uygulanmış.

## Doğrulama
- **L1 OWA-inference / CWA-validation ayrımı — DOĞRU.** SHACL modeliyle tutarlı; örgütsel
  bilginin eksikliğini (P2) doğru ele alıyor.
- **L2+L5 monotonic-temporal — TUTARLI.** invalidation = valid_to kapanışı (silme değil);
  audit + traceability korunur; non-monotonic karmaşıklıktan kaçınılır.
- **L4 explicit-negation — Datalog'dan bilinçli ayrışma**, gerekçeli (Unknown≠False, OWA).
- **L7 confidence propagation — ayırt edici.** min t-norm muhafazakâr; doc bilgi-kaybı sınırını
  dürüstçe belirtiyor.
- **L8 proof-trace = aksiyom** — P6/Madde VI ile doğru bağ; izsiz çıkarım black-box=yasak.

## Bulgular (obligation — bloke etmez)
- **OL-logic-1 — Bridge relation'lar deklare değil.** L6 (cross-namespace yalnızca bridge ile)
  "declared bridge relation" varsayıyor ama meta↔core köprüleri (ör. `describes`, ve `measures`'ın
  seviyesi — SKR-021 OA1) Meta Model'de tam tanımlı değil. Bridge set'i ENS-4001'de deklare
  edilmeli; yoksa L6 uygulanamaz.
- **OL-logic-2 — Confidence t-norm seçimi bir RFC.** min vs product vs Bayesian; bağımsız kanıt
  birleşimi min ile güçlenmez. Faz 4 öncesi bir karar (RFC) gerekir.
- **OL-logic-3 — Formal semantik Faz 4.** Sözleşme yarı-formal; SHACL + kural motoru bağı Faz 4.

## Sonuç
Semantic Logic **M2**. Inference Rules (ENS-4031) artık bu sözleşmeye *dayanarak* yazılabilir:
her kural OWA-uyumlu, monotonic-temporal, explicit-negation, namespace-sınırlı,
confidence-propagating, proof-trace üreten olmalı. Zincir: `Semantic Axioms ✓ → Semantic Logic ✓
→ Inference Rules (sırada) → Validation Rules → Architecture`.
