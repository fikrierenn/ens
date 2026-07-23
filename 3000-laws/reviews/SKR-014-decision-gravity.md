---
id: SKR-014
type: skeptic-review
origin: ENS-3022
depends_on: [ENS-3022, SKR-013]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-014 — Decision Gravity v0.2 İkinci Saldırısı

## Verdict
**survives** (üç ileri-taşıma yükümlülüğüyle). v0.2, SKR-013'ün üç talebini de karşılıyor.
Gravity artık Howard-VOI ile hizalı (stake × belirsizlik), belirsizlik terimi ENS'in mevcut
Confidence alanından geliyor, ve en önemlisi: peer-uyum ile gerçek VOI-optimum **dürüstçe
ayrılmış** — ENS peer'ı proxy kullandığını ve bunun optimum olmadığını açıkça taşıyor. Kalan
noktalar proxy-doğası ve bağımlılıklar; mevcut kusur değil. **Kavram Külliyat'a girebilir
(canon:true).**

## Talep-talep doğrulama
1. **Stake × belirsizlik — KARŞILANDI.** §Model 1: `InfoNeed = Stake × (1−Confidence)`. Howard'ın
   "stake ve belirsizlik birlikte" bulgusuyla hizalı; stake-tek model terk edildi. "Açık ama
   büyük karar" örneği (yüksek stake, yüksek Confidence → düşük InfoNeed) tam da eski modelin
   yanlış yaptığını düzeltiyor.
2. **Peer vs VOI-normatif — KARŞILANDI (dürüst).** §Model 2, ConformanceDeficit'i (descriptive,
   peer) VOI-optimumdan (normatif, pahalı) ayırıyor ve ENS'in proxy kullandığını, peer yanlıysa
   optimumu gizlediğini açıkça söylüyor. Kategori hatası giderildi.
3. **Stake normalizasyonu — KARŞILANDI.** Purpose-tipi içi z-skoru/persentil, heterojen kararları
   kıyaslanabilir kılıyor.

## Kalan riskler (ileri-taşıma yükümlülükleri)
- **OG1 — InfoNeed gerçek VOI değil, proxy.** Gerçek VOI = P(bilgi kararı değiştirir) × değişimin
  değeri; `Stake × (1−Confidence)` kaba bir proxy. Belge "VOI-proxy" diyerek dürüst; ama nicel
  kullanımda bu proxy'nin sapması bilinmeli.
- **OG2 — Confidence kalibrasyonu + OL1 bağımlılığı.** Belirsizlik = 1−Confidence yalnızca
  Confidence kalibreyse (ENS-2004) geçerli; Stake yalnızca Alternative-başına EV (OL1) varsa
  kesin. Gravity bu iki borca zincirli.
- **OG3 — PeerContext bias.** Peer sistematik az-context'liyse (ENS-2003 survivorship/exploitation)
  uyum açığı optimumu küçük gösterir; ENS bunu taşıyor ama çözmüyor.

## İç tutarlılık
InfoNeed → açık → attention (P5) zinciri VOI ile ve ENS'in dikkat-optimizasyonuyla tutarlı.
Confidence'ın belirsizlik terimi olması ENS-2004 kalibrasyonuyla doğru bağlı. Decision Entropy
ile "kırmızı bölge" okuması korunmuş. Çelişki yok.

## Sonuç
İkinci nicel yasa `ratified` edilebilir. Fizik üçlüsünün ikisi (Entropy, Gravity) artık
operasyonel ve ölçülebilir; her ikisi de ENS'in kendi altyapısı (Context, Memory, Confidence)
üzerinde tanımlı. Geriye **Decision Capital** kalıyor. OG1-OG3 orada ve OL1 konsolidasyonunda
gündeme gelecek.

## Kaynaklar
Howard (1966); Payne-Bettman-Johnson (1993); Simon — önceki SKR'ler. Yeni dış kaynak gerekmedi.
