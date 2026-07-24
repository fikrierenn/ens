---
id: SKR-027
type: skeptic-review
validation_dimension: engineering
origin: ADR-0002
depends_on: [ADR-0002, SKR-025, ENS-4020]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-027 — ADR-0002 (Operations Capability Pack) Bağımsız Validation

> **Bağımsızlık beyanı (G2/G3):** Bu inceleme önceki inline **SKR-025**'ten *bağımsız*
> üretildi. SKR-025, ADR'yi yazan sürekli context içinde inline yazılmıştı (G2/G3 ihlal riski).
> Bu kayıt ADR-0002'yi + zincirini (ADR-0001, ENS-4020) sıfırdan sınadı; SKR-025'i doğrulamak
> değil yeniden yargılamak için. Böylece ADR-0002 ≥2 bağımsız validation kaydına (SKR-025 +
> SKR-027) sahiptir (G4). **Kapsam genişletmesi:** SKR-025'in bakmadığı **ENS-4020**'yi de
> incelemeye kattım — çünkü ADR-0002'nin Madde IX uyumu artık ENS-4020'ye dayanıyor.

## Verdict
**wounded — SKR-025'ten daha derin.** SKR-025 "wounded (sınırda), üç küçük talep" dedi. D1
ölçütünün (§7.1) gerçek yanlışlanabilirlik taşıdığına, D2/D3'ün operax'a ampirik oturduğuna
katılıyorum; bunlar sağlam katkılar. Ama SKR-025'in "küçük" dediği yerin *altında* **iki yapısal
kusur** var: (C) ADR-0002'nin Madde IX uyumunu taşıyan **ENS-4020 hiç Ontology Validation'dan
geçmedi** (M0, SKR yok) — yani "operax terimleri Külliyat'ta tanımlı, ADR'de icat edilmedi"
savunması *doğrulanmamış* bir ontolojiye yaslanıyor; (D) **ADR-0002 ↔ ENS-4020 döngüsel bağımlı**
(ontoloji ADR'ye depends_on ediyor — Madde XII yetki-yönü + Meta Model circular-dependency ihlali,
P8 tersine türetme). Bunlar fixable, dolayısıyla *refuted* değil — ama ADR-0002 Accepted'a
ilerlemeden **ENS-4020'nin bağımsız Ontology Validation'ı ve döngünün kırılması Faz-blocking'dir.**

## Yenilik / prior-art incelemesi (bağımsız)
§3 prior-art'ı dürüst: **(s,S) envanter politikası** (Arrow-Harris-Marschak 1951), **EOQ**
(Harris 1913), ERP/MRP replenishment (SAP/Odoo/Logo). Bağımsız kontrol: operax'ın MinQty/MaxQty
mantığı gerçekten klasik (s,S) — atıflar gerçek, uydurma yok. ENS'in delta iddiası (ikmal
*matematiğini* değil, ikmal *önerisini* commit-mühürlü Decision + learning kapanışı + proof-trace
+ VOI ile disipline etmek) meşru — **ama bu delta'nın learning-kapanışı bacağı bu ADR'de teslim
EDİLMİYOR** (aşağıda Bulgu 2). İcat iddiası yok; Madde VI temiz.

## SKR-025 bulgularının bağımsız yeniden yargısı

### Bulgu 1 (SKR-025) — K2/K4 iddia edildi, gözlenmedi: **geçerli ve DAHA da geniş**
SKR-025 haklı: §7.2 Katman B'de K2 (attention çekişmesi) ve K4 (ortak invariant) *mantıksal
çıkarım*, tek somut log/senaryo yok. Bağımsız olarak **daha derin bir sorun** görüyorum:
Katman B'nin tamamı (dolayısıyla D1'in "kernel" sonucu) operax'ın **≥4 heterojen lifecycle
gerçekten mevcut** varsayımına dayanıyor — Replenishment, **Pricing (M04)**, Price-variance
(M03.P2), RFQ (M03.R1/R2). Ama §1 yalnızca replenishment + M03 Purchasing'i *çalışan* olarak
tanımlıyor; **M04 pricing-optimizasyonu (marj/elastikiyet) ve RFQ modüllerinin fiilen inşa edilmiş
mi yoksa roadmap mi olduğu belirsiz.** Eğer M04 henüz kod değilse, K1 ("≥2 farklı-yapılı
lifecycle") *inşa edilmemiş modüllere* dayanıyor demektir — ki bu tam olarak F2'nin uyardığı
çöküş. Yani SKR-025'in "K2/K4 gözlenmedi" itirazı, aslında **K1'in kendisine** kadar iner: kernel
gerekçesinin ampirik zemini, çalıştığı doğrulanmamış capability çeşitliliğidir.
- **Talep (güçlendirilmiş):** operax'ta hangi modüllerin *fiilen çalıştığı* (replenishment + M03
  kesin; M04 pricing / RFQ optimizasyonu?) net listelensin. Çalışmayanlar Katman B'den düşürülüp
  "gelecekte materyalize olursa kernel haklı" diye F2'ye taşınsın. Aksi hâlde D1'in kernel sonucu
  gözlemle değil vaatle destekleniyor — SKR-024'ün asıl itirazının (North Star'ı kanıt say)
  operax'a taşınmış hâli.

### Bulgu 2 (SKR-025) — §3 delta ↔ F4 tutarsızlığı: **geçerli, teyit**
§3 delta tablosu ENS'in ERP'ye delta'sını "Expected/Actual learning kapanışını (ENS-2004) sağlar"
diye yazıyor; §5.3 + §13 F4 ise bunun *henüz gerçekleşmediğini* (Expected persist edilmiyor,
Actual toplanmıyor, `learning_signal` hesaplanamaz) itiraf ediyor. ENS-4020 de bunu doğruluyor
(Confidence-üretici node yok). SKR-025'e katılıyorum: delta tablosu "sağlar" → "sağlayacak
(OQ1/OQ2 ile)" diye yumuşatılmalı. İlan edilen delta'nın bir bacağı vaat düzeyinde.

### Bulgu 3 (SKR-025) — Confidence elicitasyon boşluğu: **geçerli, teyit + ontoloji tarafı**
§5.1 `Confidence` alanı "Context Score'a bağlı" diyor ama operax deterministik SQL confidence
üretmiyor; OQ'da işaretlenmemiş. ENS-4020 §"ADR-0002'ye geri-bağ" bunu ontoloji tarafında da
doğruluyor: `Replenishment` node'unun hiçbir alanı confidence-üretici ilişki taşımıyor, bir
`ens-ent:DemandForecast (Evidence)` eksik. SKR-025'e katılıyorum: OQ'ya eklenmeli. Bağımsız
katkı: bu yalnızca "eksik alan" değil — ENS-3022 InfoNeed = Stake × (1−Confidence) *Confidence'a*
bağlı olduğundan, Confidence üretilemiyorsa **§7.3'teki VOI-önceliklendirme (K2'nin operasyonel
biçimi) de hesaplanamaz.** Yani Bulgu 3, Bulgu 1'in (K2 gözlenmedi) *nedenini* açıklıyor:
attention önceliği zaten girdisi (Confidence) yokken ölçülemez.

## Kaçırılmış yeni bulgular (SKR-025'te yok)

### Bulgu C — ENS-4020 hiç Ontology Validation'dan geçmedi, ama Madde IX'u o taşıyor — **blocking**
ADR-0002 §Uyumlaştırma notu, Madde IX uyumunu ENS-4020'ye devrediyor: "operax terimleri artık
düz-yazı değil, ENS-4020'de resmî `ens-ent:` node'ları." Yani "ADR yeni kavram tanıtmıyor"
savunması ENS-4020'nin geçerliliğine *bağlı.* Oysa:
- ENS-4020 künyesi: `status: review, maturity: M0`, `skeptic_review` alanı **yok**, kendi header'ı
  diyor ki "Ontology Validation'dan geçince M2." Yani **hiçbir validation kaydı yok.**
- validation-framework.md: `ens-ent:` bir ontoloji → **Ontology Validation** (node/edge
  completeness, directionality, cardinality, identity, semantics, closure) gerektirir; Faz-1'de
  Ontology Validator + formal-checker aktif. ENS-4020 bu turdan geçmemiş.
- **Sonuç:** ADR-0002'nin Madde IX temizliği, *doğrulanmamış* bir ontolojiye yaslanıyor. Bir
  Faz-3 ADR'nin meşruiyeti, kendi Validation'ından geçmemiş bir Faz-2 ontolojiye dayanamaz
  (Madde VII kapı; validation-framework "bir yapıt ilgili boyutun survives'ı olmadan sonraki
  fazın temeli olamaz").
- **Somut şüphe (ENS-4020 neden gerçekten sınanmalı):** ontoloji sınanmadığı için içinde en az iki
  tartışmalı eşleme fark edilmemiş:
  - `ens-ent:SupplierRelationship specializes ens-core:Capability` — "tedarikçi ilişkisi" bir
    *Capability* ("amaca hizmet edebilen örgütsel yeti") mi, yoksa bir Resource/Actor-ilişkisi mi?
    Zorlama bir specialization.
  - `ens-ent:Replenishment specializes ens-core:Decision` — ama ADR-0002 §5.2 + ENS-2001
    §Individuation der ki tvf-önerisi **atom değil** (deliberation); atom yalnızca commit-mühürlü
    POSTED emirdir. ENS-4020 "Replenishment = ikmal *kararı* → Decision" derken öneriyi mi
    commitment'ı mı specialize ediyor belirsiz. Öneriyi kastediyorsa **ENS-2001'i ihlal eder**
    (öneri ≠ Decision atomu) — terminoloji sürüklenmesi. Bu tam da Ontology Validation'ın
    yakalayacağı iç tutarsızlık; kimse bakmadığı için açıkta.
- **Talep (blocking):** ENS-4020 kendi **Ontology Validation** turundan (ayrı bir SKR) geçmeden
  ADR-0002 Accepted'a ilerleyemez. Yukarıdaki iki eşleme orada çözülmeli.

### Bulgu D — ADR-0002 ↔ ENS-4020 döngüsel bağımlılık (Madde XII + Meta Model + P8) — **blocking**
Künyeleri okuyunca mekanik bir döngü var:
- `ENS-4020.depends_on = [ENS-0000, ENS-4001, ENS-4010, **ADR-0002**]` ve
  `ENS-4020.consumed_by = [ADR-0002]`, `referenced_by: [ADR-0002]`.
- `ADR-0002` gövdesi (§Uyumlaştırma notu, §4, §5.1) ENS-4020'ye referans veriyor / ona dayanıyor.

Yani **bir ontoloji (Faz 2, `4000-`), bir ADR'ye (Faz 3) `depends_on` ediyor.** Bu üç ihlal:
- **Madde XII (yetki sırası):** "Yetki tek yönde akar. Alttaki tanımlar; üstteki tüketir."
  Ontoloji tanımlar, mimari tüketir — tersi değil. Ontolojinin ADR'ye bağımlı olması yönü ters
  çeviriyor.
- **Meta Model / Ontology Linter (ENS-4001 §Linter):** açıkça "circular dependency" statik hatası
  listeleniyor. ADR-0002 → ENS-4020 → ADR-0002 tam bir çevrim.
- **P8 / Madde VIII:** "Teori asla implementation'dan türetilmez." ENS-4020 kendi header'ında
  itiraf ediyor: operax terimlerini "SSOT ihlalini kapatmak" için, ADR-0002'nin ihtiyacından
  *sonra* ve *onun için* yazıldı. Yani ontoloji, mimarinin ihtiyacından geriye türetilmiş
  (reverse-engineered) — bootstrapping döngüsü. Madde IX'un doğru sırası: `ens-ent:Replenishment`
  *önce* bağımsız var olmalı, *sonra* ADR ona atıf yapmalı. ADR'ye geri-bağımlı bir ontoloji
  girişi bu sırayı bozuyor.
- **Talep (blocking):** `ENS-4020.depends_on`'dan **ADR-0002 çıkarılmalı** (ontoloji ADR'ye
  bağımlı olamaz); ilişki tek yön kalmalı: `ADR-0002 depends_on ENS-4020` (mimari ontolojiyi
  tüketir). ENS-4020 yalnızca `ENS-4010`/`ENS-4001`'e bağlı olmalı. Döngü kırılınca hem Madde XII
  hem Linter hem P8 düzelir.

### Bulgu E — North Star: karar düzeyinde korunuyor, ontoloji düzeyinde sızıntı riski (bloke etmez)
Task sorusu: North Star (AI-native Enterprise OS, ERP = capability) hâlâ ihlal edilmiyor mu?
Karar düzeyinde **korunuyor**: §1 "operax ERP-lezzetli ama Pack, çekirdek değil"; B1 (operax'ı
merkez yap) ve B4 (min/max heuristiğini çekirdeğe terfi et) gerekçeli reddedilmiş. İyi. Tek
sızıntı riski ontolojide: ENS-4020 domain node'ları (SupplierRelationship→Capability gibi) zorlama
eşlemelerle `ens-core`'a bağlanırsa, ERP kavramları çekirdeğe *specialization yoluyla* sızabilir.
Bulgu C'nin Ontology Validation'ı bunu da denetlemeli. Bloke etmez ama izlenmeli.

## İç tutarlılık
- ADR-0002 ↔ ADR-0001 ↔ ENS-2001/3022/4010/4025: terminoloji büyük ölçüde tutarlı.
- **Tek gerçek drift:** ENS-4020'nin `Replenishment specializes Decision` tanımı ile ADR-0002
  §5.2'nin "öneri ≠ atom" ayrımı arasındaki gerilim (Bulgu C). Çözülmeli.
- §7.3 VOI-önceliklendirmesi Confidence'a bağlı ama Confidence üretilemiyor (Bulgu 3) — iç gerilim.

## Kalan (obligation, bloke etmez)
- F5 (proof-trace WMS ölçek maliyeti) — ADR-0001 F2 mirası; Faz 4 ölçümü. Doğru ertelenmiş.
- F6 (gate kalibrasyonu) — ENS-2004 kalibrasyon borcu. Doğru ertelenmiş.
- OQ3 (Capability granülerliği: Replenish tek mi, Transfer+Purchase iki mi) — ENS-4020 örnekleme
  işi; Ontology Validation'da (Bulgu C) çözülebilir.

## Sahibine talepler (Accepted için)
1. **Bulgu C (blocking):** ENS-4020'yi bağımsız **Ontology Validation** turundan geçir (ayrı
   SKR). `Replenishment→Decision` (öneri mi commitment mi) ve `SupplierRelationship→Capability`
   eşlemelerini orada çöz.
2. **Bulgu D (blocking):** `ENS-4020.depends_on`'dan `ADR-0002`'yi çıkar; döngüyü kır. Yetki
   tek yön: ADR ontolojiyi tüketir, ontoloji ADR'ye bağımlı olamaz (Madde XII, Linter, P8).
3. **Bulgu 1 (güçlendirilmiş):** operax'ta *fiilen çalışan* modülleri (replenishment + M03 kesin;
   M04/RFQ?) net listele; çalışmayanları Katman B'den F2'ye taşı. K1'in ampirik zeminini göster.
4. **Bulgu 2:** §3 delta tablosunu "sağlar" → "sağlayacak (OQ1/OQ2)" ile F4'e tutarlı hale getir.
5. **Bulgu 3:** Confidence-elicitasyon boşluğunu OQ'ya ekle; ENS-3022 VOI-önceliklendirmesinin
   Confidence'sız hesaplanamayacağını not et.

## Sonuç ve SKR-025 ile örtüşme
ADR-0002 `Proposed` kalır. **Verdict etikette örtüşür (wounded/wounded) ama SEVİYEDE ayrışır:**
SKR-025 "sınırda, üç küçük talep" dedi; ben D1/D2/D3'ün sağlamlığını teyit ederken **iki yapısal
blocking kusur** (ENS-4020'nin doğrulanmamışlığı + döngüsel bağımlılık) ekliyorum — bunlar
"küçük" değil, Faz-blocking. Bu, inline-review şüphesini *doğrular*: SKR-025 ADR'nin kendi
dürüstçe işaretlediği F/OQ'ları resmileştirmekte iyiydi, ama **zincirin dışına (ENS-4020, künye
grafiği) hiç bakmadı** — çünkü onu yazan zihin zaten o zincirin içindeydi. Bağımsız göz, tam da
oraya baktı ve döngüyü + doğrulanmamış ontolojiyi buldu. ENS-4020, ADR-0002'nin Madde IX
temizliğini taşıyan yapıt olarak, kendisi kanıtlanmadan bu yükü taşıyamaz.
