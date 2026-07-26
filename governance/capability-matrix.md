---
id: GOV-020
title: Capability Matrix
type: standard
canon: false
constitutive: true
origin: GOV-000, GOV-010
depends_on: [GOV-000, GOV-010, ENS-4001]
referenced_by: [GOV-030]
principles: [P7]
status: review
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-23
failure_conditions: pending
maturity: M1
evidence: {sci: E1, eng: E0, ops: E0, econ: E0}
---

# Capability Matrix

> "Yetki" değil, **hangi rol hangi işlemi yapabilir.** İşlemler Meta Model (ENS-4001)
> node/edge operasyonlarıdır — dosya operasyonları değil.

## Matris (rol × işlem)

| İşlem (Meta Model üzerinde) | Author | Owner | Validator | Custodian | Governance |
|-----------------------------|:------:|:-----:|:---------:|:---------:|:----------:|
| Create Draft (node, M0) | ✅ | ✅ | ❌ | ❌ | ❌ |
| Edit content (M0–M1) | ✅ | ✅ | ❌ | ❌ | ❌ |
| Add typed edge | ✅ | ✅ | ❌ | ✅ | ❌ |
| Promote M1→M2 (validation) | ❌ | ❌ | ✅ | ❌ | ❌ |
| Promote M2→M3 (stable) | ❌ | ❌ | ✅ | ✅ | ❌ |
| Promote M3→M4 (reference) | ❌ | ❌ | ✅(eng) | ✅ | ❌ |
| Promote M4→M5 (canonical) | ❌ | ❌ | ❌ | ❌ | ✅ |
| Assign / bump version | ✅ | ✅ | ❌ | ✅ | ❌ |
| Supersede (id-koruyarak) | ❌ | ✅ | ❌ | ✅ | ❌ |
| Deprecate / archive | ❌ | ❌ | ❌ | ✅ | ✅ |
| Amend Constitution / Canon | ❌ | ❌ | ❌ | ❌ | ✅(+RFC) |
| Challenge (validation kaydı) | ❌ | ❌ | ✅ | ❌ | ❌ |

## Kurallar
- **G2:** Author, kendi yapıtını hiçbir M-seviyesine *kendi* promote edemez (satırlar Author'da ❌).
- **G3:** Validator promote-eder (M1→M4 doğrulama), Governance yalnızca M5'i onaylar — ikisi ayrı.
- **G4:** M4→M5 için Governance, ≥2 bağımsız validator imzası olmadan kapıyı **açamaz** (GOV-030).
- Custodian, içerik üretmez ama **bütünlük** işlemleri yapar (edge, supersede, deprecate, version).
- Her işlem Meta Model'de bir kayıt/kenar bırakır (G5, izlenebilirlik).
