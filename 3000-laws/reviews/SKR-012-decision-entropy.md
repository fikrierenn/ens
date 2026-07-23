---
id: SKR-012
type: skeptic-review
origin: ENS-3021
depends_on: [ENS-3021, SKR-011]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-012 — Decision Entropy v0.2 İkinci Saldırısı

## Verdict
**survives** (üç ileri-taşıma yükümlülüğüyle). v0.2, SKR-011'in üç talebini de karşılıyor ve
ilk nicel yasayı sağlam bir zemine oturtuyor. En önemlisi: **R1 borcu büyük ölçüde kapandı** —
Decision Entropy hesaplanabilir bir Shannon niceliğidir, çekirdek kavramların (Decision,
Context, Memory) operasyonel olduğunun kanıtıdır. Kalan noktalar mevcut kusur değil, additive
alan ve ölçüm inceltmeleridir. **Kavram Külliyat'a girebilir (canon:true).**

## Talep-talep doğrulama
1. **Kahneman delta + level/pattern — KARŞILANDI (matematiksel olarak sağlam).** §Model 1,
   zincir kuralını doğru kullanıyor: `H(A|C) = I(A;Owner|C) + H(A|C,Owner)`. Level noise =
   `I(A;Owner|C)` (Owner kimliğinin context sabitken seçime kattığı bilgi) ve pattern noise =
   `H(A|C,Owner)` eşlemesi doğru. Kahneman'ın azaltma-fikriyle örtüşmesi de dürüstçe kabul
   edilmiş; delta "formalizasyon + sürekli ölçüm"e daraltılmış.
2. **Exploration-noise ölçütü — KARŞILANDI.** Commitment intent-etiketi (`exploit|explore`),
   event-sourced olduğundan post-hoc oyunlanamaz; Decision Entropy yalnızca exploit üzerinden
   ölçülür, exploration ayrı izlenir. Doğru ve March (1991) OC1 ile tutarlı.
3. **Kestirim uyarısı — KARŞILANDI.** §Model 5, sonlu-örneklem bias (Miller-Madow), binning
   duyarlılığı ve güven aralığını içeriyor.

## Yeni saldırı yüzeyi (küçük)
- **Occasion noise ayrık değil.** Kahneman üç bileşen sayar: level, pattern ve **occasion
  noise** (aynı kişi, aynı vaka, farklı zaman/ruh hâli). ENS'in `H(A|C,Owner)`'ı pattern ve
  occasion'ı birleştirir. Zaman boyutuna da koşullanarak (`H(A|C,Owner,zaman-penceresi`)
  ayrılabilir; şart değil ama tam Kahneman eşlemesi için not.
- **Intent alanı bir ENS-2001 additive'i.** `intent` alanı, OL1 (Alternative-başına EV) ile
  aynı sınıf bir Decision Object genişletmesidir; ENS-2001 v0.3'te toplanmalı.

## Kalan riskler (ileri-taşıma yükümlülükleri)
- **OE1 — Decision Object `intent` alanı** (exploit|explore); ENS-2001'e eklenmeli (OL1 ile birlikte).
- **OE2 — Occasion noise** isteğe bağlı olarak zaman-penceresine koşullayarak ayrılabilir.
- **OE3 — Koşullama doğruluğu** Context relevance'a (ENS-2002 residual borçları) bağlı;
  `H(A|C)`'nin geçerliliği C kümelemesinin geçerliliğiyle sınırlı.

## İç tutarlılık
Zincir kuralı ayrıştırması matematiksel olarak doğru; intent-etiketi event-sourcing ve March
exploration ile tutarlı; Learning'le birlikte okunması (entropi ≠ kalite) korunmuş. Çelişki yok.

## Sonuç
İlk nicel yasa `ratified` edilebilir. Bunun önemi büyük: **R1 (metaforları operasyonel kıl)
borcu esasen kapandı** — Decision Entropy `H(A|C)` olarak literal ölçülebilir, dolayısıyla
Decision Gravity ve Decision Capital için de operasyonelleştirme yolu açık. OE1-OE3 sonraki
nicel yasalarda gündeme gelecek.

## Kaynaklar
Kahneman-Sibony-Sunstein (2021); Shannon (1948); Cohen κ; March (1991) — önceki SKR'ler.
Zincir kuralı: koşullu entropi standardı.
