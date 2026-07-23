---
id: SKR-006
type: skeptic-review
origin: ENS-2002
depends_on: [ENS-2002, SKR-005]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-006 — ENS Context Theory v0.2 İkinci Saldırısı

## Verdict
**survives** (üç ileri-taşıma yükümlülüğüyle). v0.2, SKR-005'in üç talebini de karşılıyor.
En kritik olan — ilgililik-döngüselliği — **Company Memory-temelli kestirimle** gerçekten
kırılmış: girdi şimdiki karar değil, tarihsel karar-sonuç verisi olduğundan döngü yoktur.
Bu, aynı zamanda değerli bir yapısal keşif üretir: **Context, Company Memory olmadan
hesaplanamaz** — teori kendi bağımlılık sırasını dayatıyor (ENS-2003 zorunlu). Kalan
zayıflıklar dürüstçe işaretlenmiş, Faz 2'ye devreden borçlardır. **Kavram Külliyat'a
girebilir (canon:true).**

## Talep-talep doğrulama
1. **Eppler-Mengis kredisi — KARŞILANDI.** §Historical tablo ve §Model 3, ters-U'yu
   Eppler-Mengis (2004)/Yerkes-Dodson'a atfediyor; ENS delta'sı (ilgililik-cinsinden yük +
   attention bütçesi) açık.
2. **Dey-Abowd delta daraltıldı — KARŞILANDI.** "Dar delta" bölümü, görev-göreli
   ilgililiğin onlarda da olduğunu kabul ediyor; ENS farkını ölçülebilirlik + staleness +
   memory-relevance ile sınırlıyor. Dürüst.
3. **Döngüsellik kırıldı — KARŞILANDI (güçlü).** §Model 2, relevance'ı benzer Purpose-tipli
   geçmiş commit-edilmiş kararların sonucu-iyileştiren context'inden kestiriyor. Dairesel
   değil; cold-start davranışı (düşük memory → düşük Confidence) doğru.

## Yeni saldırı yüzeyi — exploitation körlüğü (en güçlü kalan itiraz)

Memory-temelli relevance, tanımı gereği **saf exploitation**'dır: yalnızca geçmişte işe
yaramış context'i ilgili sayar. Bu, **March (1991), "Exploration and Exploitation in
Organizational Learning" (Org. Science 2:71–87)** ile doğrudan çatışır: March, firmaların
exploitation'ı exploration pahasına aşırı vurguladığını gösterir. Sonuç: memory-relevance,
*yeni* ilgili hale gelmiş bir context öğesini asla keşfedemez — geçmişte bağlanmadığı için
relevance'ı düşük kalır, bu yüzden bağlanmaz, bu yüzden öğrenilmez. Kendini doğrulayan bir
kör nokta.

Belge bunu "relevance bias inheritance" olarak dürüstçe failure condition yapmış ve
"keşifsel context örneklemesi" öneriyor — ama geliştirmemiş. Bu, çürütme değil; teorinin bir
**exploration politikası** borçlandığı anlamına gelir. Faz kapısını engellemez.

## Kalan riskler (ileri-taşıma yükümlülükleri)
- **OC1 — Exploration politikası (March 1991).** Memory-relevance'a, geçmişte bağlanmamış
  context'i ara sıra örnekleyen bir exploration mekanizması eklenmeli; yoksa Context Theory
  kör noktalarını sonsuza dek sürdürür. Faz 2 borcu.
- **OC2 — Benzerlik metriği regresi.** "Benzer Purpose-tipi" bir benzerlik metriği gerektirir;
  bu metrik *tüm context* üzerinden tanımlanırsa yeni bir döngü doğar. Benzerlik, yalnızca
  **Purpose-tipi taksonomisi** üzerinden (context'ten bağımsız) tanımlanmalı.
- **OC3 — Atıf-bağımlılığı (R2'ye zincirli).** "Sonucu iyileştiren context" ifadesi, sonucun
  context'e atfedilmesini gerektirir; bu, ENS-1000 §VII / R2'deki counterfactual attribution
  borcuna zincirlidir. O çözülmeden relevance kestirimi de tam ölçülemez.

## İç tutarlılık
Memory-temelli relevance, Decision Theory (commit-edilmiş karar kümesi) ve P3/P4 ile
tutarlı. Context↔Memory döngüsü (relevance memory'den gelir, her sonuç memory'yi günceller)
learning ile doğru kapanıyor. Çelişki yok.

## Sonuç
İlgililik-döngüselliğinin kırılması, Context Score'u hesaplanabilir kılar ve Decision
Entropy'nin context-benzerliği temelini (R1) sağlamlaştırır. Kavram `ratified` edilebilir.
OC1-OC3, Faz 2'de — özellikle Decision Entropy ve Company Memory (ENS-2003) yazılırken —
yeniden gündeme gelecek.

## Kaynaklar
- Eppler & Mengis (2004); Dey (2001) — SKR-005.
- **March, J. G. (1991). Exploration and Exploitation in Organizational Learning.**
  *Organization Science*, 2(1), 71–87. — exploitation/exploration; memory-relevance'ın kör noktası.
