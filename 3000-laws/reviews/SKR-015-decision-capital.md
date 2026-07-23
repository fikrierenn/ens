---
id: SKR-015
type: skeptic-review
origin: ENS-3023
depends_on: [ENS-3023]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-015 — Decision Capital (ENS-3023) Saldırısı

## Verdict
**wounded.** Metafor tuzağından kaçış iyi başlamış: "capital" gevşek bir benzetme değil,
stok-akış-amortisman yapısı. Ama üç açık var: Intellectual Capital karşısında delta dar
tutulmamış; Decision Capital'in Memory retention'dan *ayrı bir kavram mı yoksa yeniden-toplama
mı* olduğu belirsiz; ve "capital" adının transfer-edilebilirlik varsayımı (fungibility)
karşılanmıyor. Üç talep karşılanmadan `canon:true` olamaz.

## Bulgu 1 — Intellectual Capital delta'sı dar değil
Belge Stewart (1997) ve Edvinsson-Skandia'yı anıyor — iyi. Ama IC literatürü **zaten** bilgiyi
sermaye olarak ölçer (Skandia Navigator: 112 metrik, IC = human + structural capital). ENS'in
"stok + ölçüm" çatısı yeni değil. Gerçek delta yalnızca: **karar-belleği alt kümesi + reuse ×
outcome-iyileştirme + açık amortisman.** Bu daraltma açıkça yazılmalı.

## Bulgu 2 — Capital, Memory retention'dan ayrı mı, türev mi? (wounded sürücüsü)
`value(d) = |Learning(d)| × attribution_confidence(d)` ve `salience(d)` — ikisi de zaten
ENS-2003/ENS-2004 nicelikleri. `Capital = Σ value·salience`, yani mevcut memory büyüklüklerinin
bir **yeniden-toplamı** gibi görünüyor. Decision Capital ayrı bir kavram olmayı hak ediyor mu,
yoksa "Memory değerinin toplamı" için süslü bir isim mi? **Ayırt edici içerik** açıkça
gösterilmeli: bu, stok değil (stok = Memory), **akış dinamiği** (yatırım/amortisman), **ROI**
(reuse getirisi) ve **memory yatırım kararı** çerçevesidir. Bunlar gösterilmezse kavram
bookkeeping'dir.

## Bulgu 3 — "Capital" transfer-edilebilirliği varsayar; karar sermayesi context-bağımlı
Gerçek sermaye fungible/transfer edilebilirdir. Belge, transfer sorununu §failure'da itiraf
ediyor ama merkeze almıyor. Oysa bu, kavramın tanımını etkiler: Decision Capital'in getirisi
yalnızca **Purpose-tipi benzerliği** üzerinden reuse ile gerçekleşir (ENS-2003) — yani sermaye
**context-indeksli**dir, tip-içinde transfer olur, tipler arası olmaz. Bu sınır tanıma
taşınmalı; aksi hâlde "capital" adı fazla vaat eder.

## Sahibine talepler (kapıyı geçmek için)
1. **IC delta'sını daralt** (Stewart/Edvinsson/Skandia zaten stok+ölçüm yapar); ENS farkı =
   karar-belleği + reuse×outcome + amortisman.
2. **Capital'i Memory'den ayrıştır:** ayırt edici içerik akış/amortisman/ROI/yatırım-kararıdır,
   stok değil. Bunu açıkça göster, yoksa kavram türevdir.
3. **Transfer sınırını tanıma taşı:** Decision Capital context-indekslidir; getiri Purpose-tipi
   içinde gerçekleşir. "Capital" adının fungibility varsayımını dürüstçe sınırla.

## Kalan risk (carry-forward)
- **ROI → R2.** Getiri atfı (reuse'un iyileştirdiği outcome) attribution'a (ENS-2004) zincirli;
  bu 4. kavram R2'ye yaslanıyor — R2'nin ampirik gücü tüm nicel katmanın ortak kaderi.

## İç tutarlılık
Stok-akış yapısı, Learning (value) ve Memory (salience/amortisman) ile tutarlı. Entropy/Gravity
ile üçlü ilişki mantıklı. Sorun: IC delta, Memory'den ayrışma ve transfer varsayımı.

## Kaynaklar
- **Stewart, T. A. (1997). *Intellectual Capital: The New Wealth of Organizations.***
- **Edvinsson, L. (1997). Developing Intellectual Capital at Skandia.** *Long Range Planning* —
  Skandia Navigator, IC = human + structural.
- Becker (human capital); Grant (1996, knowledge-based view) — belgede konumlanmış.
