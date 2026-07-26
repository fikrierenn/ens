---
id: GOV-010
title: Governance Rolleri
type: standard
canon: false
constitutive: true
origin: GOV-000
depends_on: [GOV-000, ENS-4001]
referenced_by: [GOV-020, GOV-030]
principles: [P7]
status: review
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-23
maturity: M1
evidence: {sci: E1, eng: E0, ops: E0, econ: E0}
---

# Governance Rolleri

> Roller ilkelerden (GOV-000) türer: authority follows accountability (G1), separation (G3).
> Bir rol, bir Meta Model **owned_by** kenarıyla bir yapıta bağlanır.

## Roller

| Rol | Sorumluluk | Meta Model | Süre |
|-----|------------|------------|------|
| **Author** | Yapıtı ilk yazan | `owned_by` (yaratılış) | Geçici |
| **Owner** | Teknik içerikten *güncel* sorumlu | `owned_by` (güncel) | Değişebilir |
| **Custodian** | Uzun-vadeli **bütünlüğü** korur; yazar ayrılsa da kalır | `owned_by` (bütünlük) | Kalıcı |
| **Validator** | Adversarial doğrulama; 5 boyut (validation-framework) | `challenges` | Boyut-başına |
| **Governance** | Yalnızca **son Canonical kapısını** açar; içerik üretmez | `governs` | Kurumsal |

## Ayrımlar (kritik)
- **Author ≠ Owner ≠ Custodian.** Author bir teoriyi yazar, yıllar sonra ayrılır; Owner içeriği
  güncel tutar; **Custodian** teorinin bütünlüğünü (terminoloji, traceability, Meta Model
  uyumu) *kalıcı olarak* korur. ISO/uzun-ömürlü standart ayrımı.
- **Author ≠ Canonizer** (G2): bir yapıtı **yazan**, onu **Canonical yapamaz**.
- **Validator ≠ Governance** (G3): doğrulayan onaylamaz; onaylayan doğrulamaz.
- **≥2 bağımsız validator** (G4): her Canonical yapıt farklı boyutlardan en az iki
  bağımsız doğrulama taşır (`canonical-process.md`).

> ### ⚠️ Düzeltme — 2026-07-27 (SCAN-01)
> Bu satır önceki hâlinde **"Validator ≠ Author (G2): kendi işini doğrulayan olamaz"**
> diyordu. **Yanlıştı.** GOV-000:33'ün gerçek metni *"No author **canonizes** their own
> work"* — yani **kanonlaştırma** yasağıdır, **doğrulama** yasağı değil. Bir yazarın kendi
> işini doğrulaması G2 tarafından yasaklanmaz; yasaklanan, onu tek başına Canonical
> ilan etmesidir. Bağımsız doğrulama zorunluluğunun gerçek kaynağı **G4**'tür.
>
> Bu tek satır, korpus genelinde ≈10 dosyaya yayılan *"G2: yazan doğrulayamaz"* ailesinin
> **kök nedeniydi** (SCAN-01, `governance/SCAN-01-authority-citations.md`). Hata burada
> kesildi; türevlerinin taranması sürüyor.

## Validator boyutları (validation-framework.md)
Scientific (ens-skeptic) · Ontology · Engineering · Business · Ethical. G4: bir Canonical yapıt
için **≥2 bağımsız boyut** validator'ı zorunlu.

## Mevcut eşleme
- Author/Owner: `ens-philosopher` (Faz 1-2 yapıtları).
- Scientific + Ontology Validator: `ens-skeptic`.
- Custodian: `ens-style-guardian` (bütünlük/tutarlılık) + (gelecek) `ens-librarian`.
- Engineering/Business/Ethical Validator, Governance body: fazı gelince (ROSTER).
