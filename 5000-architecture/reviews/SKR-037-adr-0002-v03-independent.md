---
id: SKR-037
type: skeptic-review
validation_dimension: engineering
origin: ADR-0002
depends_on: [ADR-0002, SKR-025, SKR-027]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
---

# SKR-037 — ADR-0002 v0.3 Öz-Düzeltmesinin Bağımsız Sınaması

> **Bağımsızlık beyanı ([GOV-000](../../governance/000-governance-principles.md) G4):** Bu
> inceleme, ADR-0002'yi v0.3'e taşıyan (SKR-025/027 Bulgu 1/2/3
> öz-düzeltmesini yapan) önceki ens-architect/agent çağrısından **tamamen ayrı, taze context'te**
> üretildi. Yazar kendi düzeltmesini onaylayamaz (GOV-000 G2+G4); bu kayıt v0.3'ün üç kapatma
> iddiasını **operax koduna ve ENS kod tabanına karşı bağımsız yeniden denetler** — SKR-025/027'yi
> doğrulamak için değil, v0.3'ün gerçekten kapattığını *ölçmek* için.

## Verdict
**survives** — v0.3, SKR-025/027'nin kalan üç talebini (Bulgu 1/2/3) kapatıyor; en kritik olan
ampirik Bulgu 1'in her iddiası `D:\Dev\operax` reposunda dosya düzeyinde bağımsız doğrulandı
(uydurma yok, aksine iddialar konservatif). İki küçük, bloke-etmeyen gözlem kalıyor (aşağıda).
Bulgu C/D'nin kapalı kaldığı teyit edildi.

## Bulgu 1 (ampirik — en kritik): operax kod-denetimi — DOĞRULANDI

v0.3'ün operax hakkındaki her olgusal iddiasını bağımsız Glob/Grep ile sınadım:

| ADR iddiası (v0.3) | Bağımsız bulgu | Sonuç |
|---|---|---|
| RFQ = 0 kod dosyası | `**/*{Rfq,RFQ,rfq}*` → **hiç dosya yok** (yalnızca PLAN.md/TODO.md/roadmap'te anılıyor) | **Doğru** |
| M04 Pricing = yalnızca spec | `**/M04*` → tek dosya: `docs/MODULE_SPECS/M04_SalesInvoice_Pricing.md` | **Doğru** |
| M04 = satış fiyat-listesi çözümü, optimizasyon değil | Spec içeriği: `sp_ResolveSalesPrice` — 4 katmanlı liste çözümü (Promotion→PartnerList→GeneralList→ItemDefault). Marj/elastikiyet *optimizasyon* mantığı yok | **Doğru** |
| optimizasyon kodu = 0 dosya | `elasticity\|price optim\|margin optim` (i) → **hiç dosya yok** | **Doğru** |
| Replenishment çalışıyor | `Replenishment.cshtml.cs` var; `OnGetAsync` → `SELECT * FROM tvf_ReplenishmentSuggestions(@CompanyId)`; `OnPostCreateTransferAsync` → bin-to-bin transfer. tvf `db_objects.sql`'de kayıtlı | **Doğru** |
| PurchaseOrders/M03 çalışıyor | `Features/PurchaseOrders/` → 8 kod dosyası (Index/Details/Handlers/Dtos) | **Doğru** |
| Price-variance onayı çalışıyor | `PriceVariances.cshtml.cs` + `Operax.Tests/Integration/PriceVarianceTests.cs` + `schema_M02_PriceVarianceAi.sql` | **Doğru** |

**"≥4 → 3 lifecycle" düzeltmesi doğru ve dürüst.** v0.1'in "Pricing(M04) + RFQ" iddiaları operax'ta
gerçekten kod değil — v0.3 bunları düşürüp §11 F2'ye taşımakta haklı. Kod-doğrulanmış 3 heterojen
lifecycle (Replenishment / PurchaseOrder / Price-variance) fiilen mevcut.

**K1 eşiği (≥2 farklı-yapılı lifecycle) hâlâ sağlanıyor — sağlamca.** 3 doğrulanmış lifecycle ≥2'yi
geçer. Dahası sayım *fazla değil, konservatif*: bin-to-bin `StockTransfer` (§7.2 Katman A'da ayrı
lifecycle sayılıyor) dahil edilmemiş; edilseydi 4 olurdu. Yani K1'in ampirik zemini v0.3'te
gözlemle (vaatle değil) destekleniyor — SKR-024'ün "North Star'ı kanıt say" itirazının operax'a
taşınma riski bu turda kapanmış.

## Bulgu 2 (delta ↔ F tutarlılığı): KAPALI

§3 delta özeti (satır 123-129) artık teslim edileni vaat edilenden açıkça ayırıyor: *"fiilen teslim
edilen yalnızca (a)'nın commitment-sınırıdır (§5.2); (b) learning kapanışı, (c) tam proof-trace ve
(d) VOI-önceliği henüz vaat düzeyindedir."* ERP/MRP satırı (satır 118) "sağlayacak (§5.3'te henüz
eksik, OQ1/OQ2 ile kapanacak) ... vaat edilen delta, henüz teslim edilen değil" diyor. Bu §5.3 ve
§13 F3/F4 ile tutarlı.

**Güçlü-iddia dili sızıntısı taraması:** `sağlar/sağlıyor` yalnızca iki yerde geçiyor ve ikisi de
delta tablosunda **değil**, olgusal: (satır 72) "M03 Purchasing tam yaşam döngüsünü sağlar" —
operax'ta doğrulandı; (satır 389) "operax'tan model-agnostisizm kanıtı sağlar" — deterministik SQL
gerçeği, doğru. Delta tablosunda tek bir "sağlar" kalmamış. Bulgu 2 kapandı.

## Bulgu 3 (Confidence-elicitasyon): KAPALI, üstelik doğru-bağlanmış

- §5.1 Confidence hücresi (satır 176) artık dürüst: *"operax bugün deterministik SQL confidence
  üretmiyor; elicitasyon açık (Bulgu 3 / OQ6). Faz-4 `ContextScore.cs` formülü kodlu ama operax'a
  bağlanmadı."*
- OQ6 (satır 461-468) ENS-3022 zincirine doğru bağlanmış: `InfoNeed = Stake × (1−Confidence)`
  Confidence olmadan hesaplanamaz → §7.3 VOI-önceliklendirmesi (K2'nin operasyonel biçimi) şu an
  ölçülemez; **Bulgu 1'de K2'nin neden gözlenemediğinin nedeni budur** diye açıkça bağlanmış.
- **`ContextScore.cs` referansı fazla-iddialı DEĞİL — bağımsız doğrulandı.**
  `7000-reference-implementation/Ens.Kernel/Domain/ContextScore.cs` gerçekten var ve iddia edileni
  yapıyor: `Compute(coverage, noise, staleness)` (ENS-2002 §3 ters-U) + `GateConfidence(raw, score,
  threshold)` (§Implications Confidence-kapılama). Dosya kendi yorumunda "coverage dışarıdan
  verilir, Memory (ENS-2003) henüz kodlanmadı" diye dürüstçe işaretli ve **operax'a hiçbir referans
  taşımıyor** — yani ADR'nin "kodlu ama operax'a bağlanmadı (referans, iddia değil)" ifadesi tam
  isabetli. Ne uydurma ne şişirme. Bulgu 3 kapandı.

## Bulgu 4 (yeni tutarsızlık taraması): 3-vs-4 karışıklığı YOK

"3 lifecycle" ile "4/4 boyut" ayrımı belge boyunca tutarlı ve açıkça disambiguate edilmiş:
- §2 (satır 96-100): "3 kod-doğrulanmış heterojen ... (v0.3: '≥4' iddiası düşürüldü)".
- §7.2 (satır 299-300) + §7.3 (satır 308-309): açık not — *"'4/4' = K1-K4 boyutlarının hepsi;
  '3 lifecycle' = doğrulanmış commitment-lifecycle sayısı — K1 eşiği ≥2 olduğundan sağlanır."*
- §13 F2 (satır 432-437): "3 kod-kanıtlı lifecycle" — tutarlı.
- Kalan tek "≥4" geçişleri (satır 30 header, satır 98) yalnızca *düzeltilen tarihsel iddiaya* atıf,
  canlı iddia değil.

Boyut-sayısı (4) ile lifecycle-sayısı (3) hiçbir yerde karıştırılmamış. Tutarsızlık bulunamadı.

## Bulgu C/D (kapsam dışı — dokunulmadığı doğrulandı)
- **Bulgu D (döngü):** `ENS-4020.depends_on = [ENS-0000, ENS-4001, ENS-4010]` — ADR-0002 çıkarılmış;
  döngü kırık. Yön tek: ADR-0002 ontolojiyi tüketir. Madde XII / Linter / P8 temiz.
- **Bulgu C (ontoloji validation):** `ENS-4020: skeptic_review [SKR-028, SKR-030], maturity M2` —
  Ontology Validation'dan geçmiş. ADR-0002'nin Madde IX temizliği artık doğrulanmış ontolojiye
  yaslanıyor. İkisi de kapalı; v0.3 bunları bozmamış.

## Kalan gözlemler (obligation, BLOKE ETMEZ)
1. **Traceability asimetrisi (küçük).** `ENS-4020.consumed_by/referenced_by = [ADR-0002]` ve ADR
   gövdesi (Uyumlaştırma notu, §4, §5.1) ENS-4020'ye dayanıyor; ama **ADR-0002 künye `depends_on`
   (satır 7) ENS-4020'yi listelemiyor.** Döngü-kırma (Bulgu D) sırasında geri-yön kaldırılırken
   ileri-yön künyeye eklenmemiş → tek-yönlü ama *deklare edilmemiş* bağ. Bu bir döngü değil (iyi),
   ama asimetrik iz. Öneri: `ADR-0002.depends_on`'a `ENS-4020` eklenerek "mimari ontolojiyi tüketir"
   bağı künyede de açık hale getirilsin. Bloke etmez.
2. **Price-variance'ın lifecycle statüsü (küçük).** Price-variance onayı `Features/PurchaseOrders/`
   *altında* yaşıyor (M03.P2 = PO'nun alt-akışı). Onu PO'dan *tam bağımsız* üçüncü lifecycle saymak
   savunulabilir (ayrı commitment-şekli: tolerans-eşikli, exception-driven, çok-seviyeli onay) ama
   üçünün *en zayıfıdır* — agresif bir okur onu PO lifecycle'ına geri katlayabilir. Yine de K1≥2 o
   durumda bile korunur (Replenishment + PO) ve bin-to-bin `StockTransfer` ek marj sağlar. Öneri:
   ADR, §7.2 Katman A'da zaten ayrı saydığı bin-to-bin `StockTransfer`'ı §7.2 tablosunda da açıkça
   listeleyerek kernel sonucunu price-variance sınıflandırmasına daha az bağımlı kılabilir. İzlenmeli.

## Sahibine talepler
Accepted'a ilerlemek için **skeptic tarafında bloke eden talep yok** — üç talep de karşılanmış.
İki küçük gözlem (traceability asimetrisi + price-variance sınıflandırması) opsiyonel iyileştirme;
bir sonraki düzeltme turunda ele alınabilir. Kalan kapı **ens-ceo hiza incelemesi** (K4, Madde XIV);
Accepted ayrı governance edimidir (ens-ceo), bu skeptic turu vermez.

## Sonuç
ADR-0002 v0.3, öz-düzeltme olmasına rağmen (G2/G3) bağımsız turdan **survives** ile çıkıyor.
Ayırt edici bulgu: v0.3'ün en riskli hamlesi olan ampirik "≥4→3" düşürmesi, operax reposunda
dosya düzeyinde **bağımsız olarak doğru** çıktı — RFQ ve M04-optimizasyonu gerçekten kod değil,
kalan 3 lifecycle gerçekten kod. ADR iddialarını *şişirmiyor, konservatif tutuyor*. SKR-025/027
Bulgu 1/2/3 kapandı; Bulgu C/D kapalı kaldı. `status: skeptic-challenged` → survives; ADR Proposed
kalır, K4 (ens-ceo) bekler.
