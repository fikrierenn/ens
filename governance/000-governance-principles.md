---
id: GOV-000
title: Governance İlkeleri
type: standard
canon: false
constitutive: true
origin: ENS-0000, ENS-4001
depends_on: [ENS-0000, ENS-4001]
referenced_by: [GOV-010, GOV-020, GOV-030]
principles: [P7, P8]
status: review
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-23
failure_conditions: pending
maturity: M1
evidence: {sci: E1, eng: E0, ops: E0, econ: E0}
---

# Governance İlkeleri (Governance Core)

> Governance'ın anayasasıdır — kısa. Roller, yetkiler ve süreçler bu ilkelerden **türer**.
> Governance, Anayasa'ya (ENS-0000) tabidir; onun yerine geçmez.

## Temel ilke: ne yönetilir
**Governance dosyaları yönetmez. Governance, Meta Model'in (ENS-4001) node'larını ve
edge'lerini yönetir.** Yönetilen şey artık markdown değil, tipli-semantik bilgi varlıklarıdır:
Concept, Theory, Law, Evidence, ilişkiler (implements, supports, invalidates…). Dosya, bir
node'un yalnızca serileştirmesidir.

## İlkeler

- **G1 — Authority follows accountability.** Yetki, sorumlulukla gelir; sorumlu olmayan yetki veremez.
- **G2 — No author canonizes their own work.** Bir yapıtı yazan, onu Canonical yapamaz.
- **G3 — Validation ve approval ayrıdır.** Doğrulayan onaylamaz; onaylayan doğrulamaz.
- **G4 — Her Canonical yapıtın ≥2 bağımsız validator'ı vardır** (farklı boyutlardan).
- **G5 — Governance kararları izlenebilirdir.** Her karar bir kayıt (SkepticReview/ADR/RFC) ve
  Meta Model kenarı bırakır; sessiz karar yoktur.
- **G6 — İtiraz (appeal) mümkündür.** Her promotion/deprecation kararına gerekçeli itiraz edilebilir;
  itiraz yeni bir validation turu açar.
- **G7 — Governance bireylere değil, Anayasa'ya hizmet eder.** Çatışmada Anayasa kazanır (Madde XV).

## Sonuçlar
- Canonical statü **oylama ile değil, kanıt zinciri ile** kazanılır (bkz. GOV-030). Governance
  yalnızca son kapıyı açar.
- G2 + G4 gereği, mevcut Külliyat'ta yazar (ens-philosopher) hiçbir şeyi tek başına Canonical
  yapamaz; ve Engineering Validation Faz 4'ü gerektirdiğinden **M5 şu an ulaşılamaz — Canon boş.**
  Bu, ilkelerin doğrudan ve dürüst sonucudur.
