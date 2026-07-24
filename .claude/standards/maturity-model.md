---
id:            STD-MATURITY-MODEL
title:         Olgunluk Modeli (Theory Maturity Model)
type:          standard
canon:         false
origin:        ENS-0000 §IV, §VII
depends_on:    [ENS-0000]
principles:    []
status:        ratified
owner:         ens-philosopher
version:       0.1.0
last_reviewed: 2026-07-24
---

# Olgunluk Modeli (Theory Maturity Model)

**Yetki:** [ENS Anayasası, Madde IV & VII](../../0000-constitution/ENS-0000-constitution.md)
**Çözdüğü sorun:** Canon ile Theory'nin karışması. **Canon, Theory'nin tamamı değildir;
_gerçek hayatta yaşamış_ çekirdeğidir.** TCP önce reference implementation oldu, sonra
standard — bir kavram Canonical olmadan önce çalışan bir implementation'da yaşamalıdır.

## Basamaklar (6 kademe)

| Seviye | Ad | Giriş koşulu | `status` | Faz |
|--------|-----|--------------|----------|-----|
| **M0** | Draft | Yazıldı; tutarlılık garantisi yok | `draft` | 1-2 |
| **M1** | Proposed | Künye tam; prior-art konumlanmış; failure conditions belirtilmiş | `review` | 1-2 |
| **M2** | Reviewed | Scientific skeptic saldırısından **survives** (≥1 SKR); iç tutarlı | `skeptic-challenged`→`ratified` | 1-2 |
| **M3** | Stable | M2 **+** ≥2 alt-akış yapıtı onu kırmadan kullanıyor **+** Scientific evidence ≥ **E3** | `ratified` | 1-2 |
| **M4** | Reference | M3 **+** reference platform'da (Faz 4) *çalışan* implementation'da yaşıyor **+** Engineering evidence ≥ **E3** | `ratified` | **4** |
| **M5** | Canonical | M4 **+** *temel* (çok bağımlı) **+** dört-skeptic (Sci+Eng+Biz+Ethical) geçti **+** governance ile Canon'a **açıkça** kabul edildi | `ratified` | 4-5 |

## Kurallar
- **`canon: true` yalnızca M5'tir.** Skeptic-survives (M2/M3) Canon yapmaz.
- **M4+ Faz 4 gerektirir** (reference platform yoksa hiçbir şey "gerçek hayatta yaşamış"
  olamaz). **Dolayısıyla Faz 1-2 boyunca azami olgunluk M3'tür ve hiçbir yapıt Canonical
  (M5) değildir.** Canon şu an *boştur* — bu doğru ve dürüst durumdur.
- M5, bir governance kararı gerektirir (bkz. governance.md) — otomatik değil.
- Bir kavram M3'te kalıcı olabilir (yararlı ama çekirdek değil); Canonical olmak zorunda değildir.

## Mevcut Külliyat'ın yeniden derecelendirilmesi (borç)
`canon:true`'yu skeptic-survives'te veriyorduk — çok gevşek. Yeniden derecelendirme:
- `ENS-2001..2004` (Decision/Context/Memory/Learning): **M3 (Stable)**, M5-Canonical *adayı*
  (Faz 4 sonrası). `canon:true` → `canon:false, maturity:M3`.
- `ENS-3021..3023` (Entropy/Gravity/Capital): **M3 (Stable)**; delta evidence düşük (E1).
  `canon:true` → `canon:false, maturity:M3`.
- Bu re-grading bir governance + `ens-style-guardian` işidir (ENS-2001 v0.3 ile birlikte).
  **Şu an Külliyat çekirdeği M3'tür; Canon (M5) Faz 4'ü bekliyor.**

## Metadata alanı
Her Faz 1-2 yapıtı künyesine `maturity: M0..M5` ekler (bkz. metadata-header.md).
