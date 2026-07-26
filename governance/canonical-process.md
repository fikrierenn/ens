---
id: GOV-030
title: Canonical Process
type: standard
canon: false
constitutive: true
origin: GOV-000, GOV-020, maturity-model, evidence-standard
depends_on: [GOV-000, GOV-010, GOV-020, ENS-4001]
referenced_by: []
principles: [P8, P7]
status: review
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-23
failure_conditions: pending
maturity: M1
evidence: {sci: E1, eng: E0, ops: E0, econ: E0}
---

# Canonical Process

> Canonical (M5) statü **oylama ile değil, kanıt zinciri ile** kazanılır. Governance yalnızca
> **son kapıyı** açar — ve ancak zincir tamamsa.

## Zincir (M3 → M5)
```
M3 Stable
   │  Scientific Validation ✓        (ens-skeptic; sci evidence ≥ E3)
   ▼
   │  Ontology Validation ✓          (Meta Model uyumu; SKR ontology)
   ▼
   │  Engineering Validation ✓       (uygunsa; eng evidence ≥ E3)
   ▼
M4 Reference — reference platform'da çalışan implementation'da yaşıyor (Faz 4)
   │  Operational Evidence           (varsa; ops evidence, Faz 5)
   ▼
   │  Governance Approval            (son kapı — yalnızca zincir tamamsa)
   ▼
M5 Canonical
```

## Kurallar
1. **Her kapı bir kanıttır**, oy değil. Governance kanıt üretmez, yalnızca zincirin tamlığını
   doğrulayıp son kapıyı açar.
2. **G4:** ≥2 bağımsız boyut validator'ı (ör. Scientific + Ontology, uygunsa + Engineering).
3. **G2:** Author zincirin hiçbir kapısını kendi açamaz.
4. **M4+ Faz 4 gerektirir** (reference implementation). Dolayısıyla **M5 şu an ulaşılamaz;
   Canon boştur** — bu doğru durumdur (maturity-model.md).
5. **İtiraz (G6):** M5 kararına gerekçeli itiraz, yeni bir validation turu açar.

## Re-grading yetkisi (mevcut borç)
Bu süreç, mevcut yanlış `canon:true`'ların **düzeltilmesini** yetkilendirir. Not: bu bir
*canonization* değil, bir **demotion**'dır (yanlış statüyü doğruya çekmek) — G2 kısıtı
canonization içindir, demotion Custodian (`ens-style-guardian`) tarafından yapılabilir.
- `ENS-2001..2004`, `ENS-3021..3023`: `canon:true` → `canon:false, maturity:M3`.
- Canon (M5) tablosu **boşalır**; M3 çekirdek ayrı listelenir (KULLIYAT.md).
