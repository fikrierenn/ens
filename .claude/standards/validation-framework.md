---
id:            STD-VALIDATION-FRAMEWORK
title:         Doğrulama Çerçevesi (Validation Framework)
type:          standard
canon:         false
origin:        ENS-0000 §X
depends_on:    [ENS-0000]
principles:    []
status:        ratified
owner:         ens-skeptic
version:       0.1.0
last_reviewed: 2026-07-24
---

# Doğrulama Çerçevesi (Validation Framework)

**Yetki:** [ENS Anayasası, Madde X](../../0000-constitution/ENS-0000-constitution.md)
**Yeniden çerçeveleme:** "Skeptic" tek başına *saldırır*; yaptığı iş aslında **Validation**'dır.
Hedef çürütmek değil, doğrularken **en güçlü itirazları sistematik üretmektir.** Skeptic, bu
doğrulama sürecindeki adversarial *stance*'tir — bir rol, sürecin tamamı değil.

## İki katman
```
Validation
├── Scientific Validation   (adversarial = "Skeptic")   — Faz 1-2
├── Ontology Validation     (Meta-Ontology / şema)        — Faz 1 (Meta Model, ontoloji)
├── Engineering Validation  (inşa edilebilir mi/edildi mi?)— Faz 3-4
├── Business Validation     (şirket gerçekten ister mi?)   — Faz 5
└── Ethical Validation      (AI çağında zorunlu)           — tüm fazlar
```
Her boyut faz-aktifleşir; hepsi bir **Validation Review** kaydı (`SKR-NNN`, artık "Structured
Validation Review") üretir ve künyeye `validation_dimension` ve ilgili `evidence` boyutunu
(bkz. evidence-standard.md) atar.

## Kayıt (SKR) alanları
Var olan 16 SKR = Scientific Validation. İleride her kayıt bir `dimension` taşır:
`scientific | ontology | engineering | business | ethical`. Numaralandırma sürekli (SKR-NNN).

## Dimension-özel checklist'ler
- **Scientific:** yenilik, yanlışlanabilirlik, kanıt, varsayım, karşı-argüman, iç tutarlılık
  (mevcut ens-skeptic).
- **Ontology (Meta-Ontology):** node completeness, edge completeness, directionality,
  cardinality, temporal model, identity, semantics, closure (bkz. ENS-4001 doğrulaması).
- **Engineering:** inşa edilebilirlik, ölçek, replaceability, test edilebilirlik (Faz 3-4).
- **Business:** değer, talep, maliyet, benimsenme (Faz 5).
- **Ethical:** zarar, önyargı, sorumluluk, şeffaflık, insan-kontrolü (P7).

## Agent eşlemesi
- `ens-skeptic` = **Scientific Validator** (adversarial).
- Ontology Validator + `formal-checker` (Ontology Linter) — Faz 1'de aktif (Meta Model gerektirir).
- Engineering/Business/Ethical Validator — fazı gelince (ROSTER).

## Kural
Bir yapıt, ilgili boyutun Validation'ından **survives** almadan bir sonraki fazın temeli
olamaz (Anayasa Madde VII, X). Governance dahil hiçbir üst-yapı, doğrulanmamış bir Meta
Model'in üstüne kurulamaz.
