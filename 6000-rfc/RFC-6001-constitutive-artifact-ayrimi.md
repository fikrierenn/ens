---
id:            RFC-6001
title:         Constitutive Artifact Ayrımı ve Madde IV Canon Kuralının Düzeltilmesi
type:          rfc
canon:         false
origin:        ENS-0000 §XV
depends_on:    [ENS-0000, ENS-4000, STD-METADATA-HEADER, STD-MATURITY-MODEL]
referenced_by: []
principles:    [P8]
status:        Accepted
owner:         ens-philosopher
version:       0.3.0
last_reviewed: 2026-07-24
amends:        [ENS-0000 §IV, STD-METADATA-HEADER]
failure_conditions: stated
skeptic_review:     [SKR-034, SKR-035, SKR-036]
ceo_review:         CEO-0002
style_signoff:      STYLE-SIGNOFF-RFC-6001
---

# RFC-6001 — Constitutive Artifact Ayrımı ve Madde IV Canon Kuralının Düzeltilmesi

**Yetki:** [ENS Anayasası, Madde XV (Değişiklik)](../0000-constitution/ENS-0000-constitution.md).
Bu RFC, Madde XV'in üç koşulunu hedefler: (a) değiştirilen maddeye atıf yapar — **Madde IV**
(ana), yan-etki olarak **Madde XI** şema referansı ve **Madde X** ile hizalama; (b) `ens-skeptic`
saldırısına açılır (aşağıda §9); (c) Madde XIV uyarınca kabul için `ens-ceo` hiza incelemesine sunulur.

> **RFC yaşam döngüsü konumu (Madde XIV):** `Review` — **bağımsız 3. tur SKR-036 → `survives`**
> (skeptic-kapısı geçildi; D6 tek-yönlü invariant + turnusol-birincil çözümü bağımsız korpus
> taramasıyla doğrulandı, governance evidence eng/ops/econ=E0 ayrıştırmayı destekliyor, FC#5 dürüst
> açık). **RFC artık `ens-ceo` Madde XIV hiza incelemesine + `ens-style-guardian` şema-imzasına
> (§7.5 çift-owner kapısı) hazırdır.** SKR-036 üç bloke-etmeyen keskinleştirme bıraktı (S1:
> invariant'ın tek yasak hücresini açıkça yaz; S2: `amends:` alanına STD-MATURITY-MODEL ekle —
> canon kuralının üçüncü lokusu maturity-model.md satır 34; S3: governance M1'ini "olumsal
> alışkanlık" değil "zorunlu ama non-canon-gating" diye yeniden nitelendir + FC#5'e eng/ops/econ=E0
> kanıtını ekle) — bunlar hiza incelemesiyle paralel kapatılabilir, survives'ın koşulu değildir.
> **[2026-07-24 GÜNCELLEME] `Accepted`.** Çift-owner kabul kapısı (§7.5) tamamlandı: `ens-ceo`
> hiza incelemesi ([CEO-0002](../5000-architecture/reviews/CEO-0002-rfc-6001-alignment.md), onay,
> itiraz yok) + `ens-style-guardian` şema-imzası ([STYLE-SIGNOFF-RFC-6001](reviews/STYLE-SIGNOFF-RFC-6001.md),
> itiraz yok). Anayasa Madde IV ve `metadata-header.md` şeması **fiilen düzenlendi** (§8.4 kapsamı:
> yalnızca bu ikisi). Korpus retrofit'i (§10 — diğer belgelerin `constitutive` alanı, ENS-4000
> canon-incelemesi, maturity-model.md/KULLIYAT.md notu) **ayrı, sonraki bir adımdır** — ROADMAP.md.
>
> Bu belge hâlâ bir **öneridir**; Anayasa ve künye şeması bu RFC `Accepted` olana dek **fiilen değişmez**.
> SKR-035, SKR-034'ün D2/D3/D4/D5 taleplerinin gerçekten kapatıldığını bağımsız doğruladı; tek yeni
> blocking (**D6**) ve iki keskinleştirme (**D7/D8**) v0.3'te kapatıldı:
> **D6 kökü — `maturity ⟺ constitutive:false` çift-yönlü invariant'ı fazla iddialıydı** (governance
> ailesi `constitutive:true` iken fiilen `maturity:M1` taşıyor). Çözüm (SKR-035 talep-a): invariant
> **tek-yönlü** bir gerekli-koşula indirgendi (`constitutive:false ⇒ maturity taşır`; tersi değil),
> §4.2 turnusolu **birincil sınıflayıcı** ilan edildi (D7), maturity *etiketi* ile canon *yolu*
> constitutive:true yapıtlar için **ayrıştırıldı**, ve §10.5'in olgusal-yanlış cümlesi düzeltildi.
> Governance böylece çelişki değil, **izinli** bir örnek olur; hiçbir M-grade sökülmez. **D8:** §7.4'e
> immutable-core verme yordamı (yalnızca Madde XV) bir cümleyle bağlandı. Sıradaki adım: bağımsız
> **3.** `ens-skeptic` turu. `ens-ceo` Madde XIV incelemesi ancak `survives` sonrası gelir.
> Ayrıntı §11 + SKR-035. **⚠️ Bu sürüm öz-onaylı değildir: `survives` yalnızca 3. tur skeptic'le verilir.**

---

## 1. Özet

Anayasa Madde IV, Külliyat üyeliğini (`canon`) yapıtın **aralığına** bağlar: *"0/1/3/4 aralıkları
doğası gereği Külliyat'tır."* Bu cümle, deponun **fiili pratiğiyle çelişir**: her gerçek
`3000-laws`/`4000-ontology` belgesi `canon: false` ile başlar ve Külliyat'a ancak failure
conditions'ını belirtip `ens-skeptic`'ten sağ çıkınca girer. Aynı zamanda `metadata-header.md`
şeması `canon: true ⇔ maturity: M5` der ve M5 Faz 4 (reference platform) ister — bu, şu an
`canon: true` olan Anayasa (ENS-0000) ve Sözlük (ENS-4000) gibi belgeleri kendi şemasına göre
kural-dışı bırakır.

Bu RFC iki bağımsız ekseni resmîleştirir ve çelişkiyi giderir:

- **`constitutive`** — bir yapıtın *türü*: ENS akıl yürütmesini **olanaklı kılan** bir kural/tanım/
  tip-şeması mı (normatif, tanımla yürürlükte), yoksa dünya hakkında **yanlışlanabilir ampirik**
  bir iddia mı taşıyor?
- **`canon`** — bir yapıtın *doğrulanma durumu*: Külliyat'a girmiş mi?

Öneri: (i) `constitutive: true|false` alanını künye şemasına resmî ekle; (ii) Madde IV'ün
"aralık = canon" cümlesini, canon'u **aralığa değil, yapıt türüne uygun bir doğrulama yoluna**
bağlayan bir kuralla değiştir. Bu ayrım Külliyat'ta zaten fiilen kullanılıyor (ENS-4000 v0.2,
KULLIYAT.md "gap #1") ama Anayasa'ya ve şemaya hiç resmî işlenmedi. Bu RFC o borcu kapatmayı önerir.

## 2. Problem (motivasyon)

Üç somut tutarsızlık:

1. **Aralık ↔ pratik çelişkisi.** Madde IV: "0/1/3/4 doğası gereği Külliyat." Gerçek: `ENS-3021`,
   `ENS-3022`, `ENS-3023`, `ENS-4010`, `ENS-4020`, `ENS-4031` — hepsi `canon: false` ile doğdu;
   canon, aralığa değil **skeptic-hayatta-kalmaya** bağlı işletildi. Yani norm belge, pratik
   normu takip etmiyor. (Not: çelişki yalnızca 0/1/3/4 için; Madde IV zaten 2000 için doğru
   davranıyor — "onaylanınca Külliyat".)

2. **Şema paradoksu.** `metadata-header.md` §Değer kümeleri: *"`canon: true` — yalnızca
   `maturity: M5` ise true"*, ve `maturity` M4+ Faz 4 ister. Ama ENS-0000 (`canon: true`,
   pre-Faz-4) ve ENS-4000 (`canon: true`, M-eksenine tabi değil) bu kuralı bugün ihlal ediyor.
   Kural, ampirik teori için doğru; kurucu belgeler için yanlış — çünkü tek bir eksende iki farklı
   doğrulama rejimini birleştiriyor.

3. **Resmîleşmemiş fiili ayrım.** `ENS-4000-glossary.md` v0.2 (G-11 kapanışı) `constitutive`
   ile `maturity: M0..M5`'i **iki bağımsız eksen** olarak zaten kullanıyor; `KULLIYAT.md`
   "Normatif/Constitutive çekirdek" ile "Teorik Canon (M5)"i ayırıyor ve açıkça *"bu ayrım henüz
   Anayasa'da netleştirilmedi (Constitution-amendment borcu, gap #1)"* diyor. Yani kavram
   Külliyat'ta yaşıyor ama üst-kaynağı (Anayasa) ve makine-okunur şeması (künye) onu tanımıyor —
   Madde IX/XI'in kendi ruhuna (kavram önce resmîleşir) aykırı bir boşluk.

## 3. Prior art ve delta (SKR-001 dersi: özgünlüğü önden konumla)

Bu RFC yeni bir kavram *icat etmiyor*; iki yerleşik ayrımı ENS künyesine ve Anayasa'ya bağlıyor.

- **Searle, *constitutive vs regulative rules*** (*Speech Acts*, 1969): kurucu kurallar bir
  etkinliği *olanaklı kılar* ("X, C bağlamında Y sayılır"); düzenleyici kurallar önceden var olan
  etkinliği yönetir. **Delta:** ENS, bu tür-ayrımının **üstüne dik bir doğrulama-durumu ekseni**
  (`canon`) koyar ve her türü **farklı bir doğrulama yoluna** bağlar (kurucu → tutarlılık/
  ratifikasyon; ampirik → kanıt zinciri). Searle bir doğrulama rejimi önermez; ENS önerir.
- **Analitik/sentetik ayrımı** (Kant; Quine, *Two Dogmas*, 1951): kurucu ≈ tanımla-doğru,
  ampirik ≈ kanıtla-doğru. **Delta ve dürüst sınır:** Quine bu ayrımın **metafizik keskinliğini**
  çürütür. ENS bu itirazı **kabul eder** ve keskin bir analitik/sentetik sınır **iddia etmez**;
  `constitutive` bir metafizik değil **operasyonel/editöryel** sınıflamadır — belge-başına, açık
  bir varsayılanı ve bir **itiraz yolu** (skeptic yeniden-sınıflandırabilir) olan bir editöryel
  karardır. (Savunulabilir küçük iddia > çürütülebilir büyük iddia — SKR-001.)
- **Standart mühendisliğinde spec-conformance vs empirical-validation** (ISO/IEEE; RFC 2119
  normative/informative): bir *şema/spesifikasyon* uygunluk (conformance) ile doğrulanır; bir
  *ampirik model* öngörü (prediction) ile. **Delta:** ENS bu iki doğrulama kipini tek bir künye
  alanına (`constitutive`) ve tek bir anayasal kurala (Madde IV) bağlar.
- **Lakatos, *hard core* vs *protective belt*** (*The Methodology of Scientific Research
  Programmes*, 1970): bir araştırma programının *hard core*'u, **metodolojik kararla**
  yanlışlamadan korunur (negatif heuristik); *protective belt* ise hipotezlerle sınanır ve
  gerektiğinde revize edilir. **Delta ve kullanım (D2):** Bu, `constitutive: true` sınıfının
  **heterojenliğini** açıklayan doğru prior art'tır. ENS-0000 Madde III (P1-P8), ampirik olarak
  yanlışlanamaz kılınmış (Madde XV: niyet düzeyinde değiştirilemez) bir **hard core**'dur — dogma
  değil, *meşru korunmuş çekirdek*: onu bir gözlemle çürütemezsiniz, ancak *farklı bir program
  ilan ederek* reddedebilirsiniz. Geri kalan constitutive belgeler (Madde I/II/IV-XIV, Sözlük,
  governance) ise **protective belt**'tir: normal RFC + skeptic-tutarlılık incelemesiyle revize
  edilebilir (bu RFC'nin Madde IV'ü değiştirmesi tam da bunun kanıtı). ENS bir Lakatosçu
  hard-core/belt ayrımı *icat etmez*; onun ENS künyesindeki karşılığını (§4, `immutable-core`
  alt-kategorisi) resmîleştirir. **Bedeli dürüstçe:** hard core için Madde X'in yanlışlanabilirlik
  ödevi *ampirik* düzeyde değil, yalnızca *program/tutarlılık* düzeyinde karşılanır (§4).
- **Kelsen *Grundnorm* / Hart *rule of recognition*** (*Pure Theory of Law*, 1934; *The Concept
  of Law*, 1961): aksiyomatik/normatif bir sistem, sonsuz gerilemeyi (Münchhausen trilemması)
  durdurmak için **tek bir öz-yetkilendiren köke** ihtiyaç duyar — kendisini üstten doğrulayan bir
  norm olmayan, sistemin geçerliliğinin dayandığı temel-norm/tanıma-kuralı. **Delta ve kullanım:**
  RFC ENS-0000'i "biricik kendi kendini yetkilendiren yapıt" diye *ilan etmez*, bu köke duyulan
  ihtiyacı **ilkeli bir zorunluluk** olarak gerekçelendirir (Test-2 daireselliği ad hoc istisna
  değil, aksiyomatik sistemin mantıksal gereğidir). Kök, canon'unu **ve** çekirdek ilkelerini
  öz-referansla taşır; bu öz-referans, Grundnorm'un ta kendisidir.
- **Carnap, *internal/external questions*** (*Empiricism, Semantics, and Ontology*, 1950): bir
  dilsel çerçeve *içindeki* sorular (internal) çerçevenin kurucu kurallarıyla analitik olarak
  yanıtlanır; çerçevenin *kendisine* dair sorular (external) pragmatik/pratiktir. **Delta ve
  kullanım (Quine'a dayanıklılık):** Quine analitik/sentetik sınırın **mutlak** keskinliğini
  çürütür; Carnap'ın **çerçeveye-göreli** analitikliği bu itiraza dayanıklıdır çünkü metafizik bir
  ayrım değil, çerçeve-seçimine bağlı bir ayrım iddia eder. `constitutive: true`, "ENS çerçevesine
  göreli analitik" (çerçeve içinde tanımla-doğru) olarak tam bu şekilde modellenir — Kant'ın ham
  analitik/sentetik ikiliğinden daha savunulabilir bir zemin.
- **Külliyat-içi prior art:** `ENS-4000` v0.2 (`constitutive` vs `maturity`), `KULLIYAT.md`
  (Normatif çekirdek vs M5 Canon), `maturity-model.md` (M0-M5 zinciri). Bu RFC onlara **çelişmez**;
  onları üst-kaynağa terfi ettirir.

## 4. Tanımlar (`constitutive` ↔ `canon` net ayrımı)

İki eksen **diktir**; karıştırılmaz:

| Eksen | Sorduğu soru | Değer | Belirleyen |
|-------|--------------|-------|-----------|
| `constitutive` | *Bu belge, tipin/kuralın/şemanın kendisi mi (normatif), yoksa yanlışlanabilir ampirik bir iddia mı?* | `true` / `false` | belgenin **türü** (editöryel, skeptic-itiraz edilebilir) |
| `canon` | *Bu belge doğrulanıp Külliyat'a girdi mi?* | `true` / `false` | **doğrulama durumu** (kazanılır, ilan edilmez) |

Dört hücrenin anlamı:

- `constitutive: true, canon: false` — kurucu bir belge, henüz skeptic tutarlılık incelemesinden
  geçmemiş (ör. taslak bir tip-şeması).
- `constitutive: true, canon: true` — ratifiye kurucu belge (ör. ENS-0000, ENS-4000).
- `constitutive: false, canon: false` — ampirik teori/yasa, henüz M5 değil (ör. bugünkü
  ENS-2001..2004, ENS-3021..3023 — hepsi `ratified`/M3 ama Faz-4 öncesi Canon değil).
- `constitutive: false, canon: true` — ampirik kavram tam Canon (M5). **Bugün bu küme boştur**
  (P8: Canon, kod teoriyi kanıtlayınca dolar — dürüst durum).

**Kritik nokta:** `constitutive` bir belgeyi doğrulamadan *muaf tutmaz*. Yalnızca **yanılma kipini**
değiştirir (Madde X ile hizalı, §6): kurucu belge *tutarlılık/örneklenebilirlik* ile yanılabilir
(tanım tutarsız / örneklenemez / başka Külliyat kuralıyla çelişir / daha iyi bir ayrım var);
ampirik belge *öngörü* ile yanılır. Her ikisi de failure conditions taşımak **zorundadır**.

### 4.1 `constitutive: true` tekdüze bir sınıf değildir (D2 — SKR-034)

SKR-034 haklı olarak gösterdi ki `constitutive: true` **heterojen**dir: içinde, ampirik olarak
**yanlışlanamaz** kılınmış bir çekirdek ile tutarlılıkla-revize-edilebilir bir kuşak birlikte
bulunur. Lakatosçu *hard core / protective belt* ayrımı (§3) bu heterojenliği tam karşılar. Bu
yüzden `constitutive: true` yapıtları iki alt-kategoriye ayırıyoruz:

| Alt-kategori | Ne | Yanılma kipi | Nasıl değişir | Örnek |
|--------------|----|--------------|---------------|-------|
| **immutable-core** (hard core) | Ampirik yanlışlamaya **metodolojik kararla** kapatılmış çekirdek | *Yalnızca* program-düzeyi: "farklı bir program ilan et" | Yalnızca sözcük düzeyinde (Madde XV) | **ENS-0000 Madde III / P1-P8** |
| **revisable-constitutive** (protective belt) | Kurucu ama RFC + skeptic-tutarlılık ile revize edilebilir kural/tanım/şema | Tutarlılık / örneklenebilirlik / daha iyi ayrım | Normal RFC (Madde XIV/XV) | ENS-0000 Madde I/II/IV-XIV; Sözlük (ENS-4000); governance (GOV-*) |

**Dürüst asimetri (Madde X ile sınır):** immutable-core için Madde X'in yanlışlanabilirlik ödevi
**ampirik düzeyde karşılanamaz** — *"karar, örgütün en küçük anlamlı birimi değildir"* gözlemi P1'i
çürütmez; Anayasa P1'i tanım gereği yanlışlamaya kapatmıştır. Bu bir kaçamak **değildir**, çünkü
(a) Madde XV bunu açıkça işaretler (gizli değil), (b) program düzeyinde yanlışlanabilir kalır
(farklı bir program ENS'i reddedebilir), (c) Lakatos bir hard core'u korumayı meşru kılar. Dolayısıyla
RFC "Madde X keskinleşti" iddiasını **yalnızca protective belt için** tutar; immutable-core için
Madde X *program/tutarlılık düzeyinde* karşılanır, ampirik düzeyde **bilinçli olarak açıktır**.
Bu, SKR-034'ün W2 uyarısına verilen dürüst yanıttır: sınıfın tavanını (kök) tabanı (şema) gibi
konuşmuyoruz.

**Şema kararı (belge-düzeyi tek bayrak yeterli mi?):** `immutable-core`, korpus geneline yayılmış
bir alan **değildir**; şu an tek bir locus'a (ENS-0000 Madde III) uygulanır ve zaten Madde XV
tarafından *yerinde* işaretlenmiştir. Bu yüzden ayrı bir zorunlu künye alanı eklemiyoruz (scope
creep'ten kaçınırız); `immutable-core` bir **belge-içi bölüm niteliği** olarak, yalnızca ENS-0000
künyesinde açık bir `immutable_core_sections: [Madde III]` notu ile temsil edilir (§7.4). İleride
başka bir yapıt hard-core statüsü iddia ederse, bu alan yeniden değerlendirilir — o zamana dek
tek-locus için tam alan aşırı-mühendisliktir.

### 4.2 Operasyonel turnusol testi (D3 — SKR-034)

`constitutive` editöryel takdire bırakılamaz; SKR-034 failure-condition #3'ü (canon kaçamağı)
savunmak için **işleyen** bir turnusol gerekir. Üç ardışık test — göstererek, iddia etmeden
(§8'de her örneğe uygulanır):

- **Test A — Kaldırma testi (anlamsızlaşma mı, yanlışlanma mı?):** *"Bu belgeyi kaldırırsam,
  üzerine kurulu yapıtlar **anlamsız** mı olur (tipleri/terimleri/kuralları tanımsız kalır), yoksa
  yalnızca bir **iddia** mı kanıtsız kalır (ampirik önerme çürür)?"* → Anlamsızlaşırsa **kurucu
  aday**; yalnızca bir iddia çökerse **ampirik** (`constitutive: false`).
- **Test B — Yanılma-kipi testi (kurucu adaylar için: hard core mu?):** *"Bu belge iç-tutarsızlık /
  örneklenemezlik / daha iyi bir ayrımla çürütülebilir mi (tutarlılık kipi), yoksa metodolojik
  fiat'la (Madde XV) her türlü yanlışlamadan korunmuş mu?"* → Tutarlılıkla çürütülebilir →
  **revisable-constitutive** (protective belt); fiat'la korunmuş → **immutable-core** (hard core).
- **Test C — Yeterlilik testi (hibrit ontolojileri ayırır):** *"Bu belge, bir tip sisteminin bir
  **gerçek alanı doğru eklemlediği** (synthetic adequacy) yönünde, senaryo/saha verisiyle
  sınanabilir bir iddia taşıyor mu (temsil edemediği bir alan, daha iyi oturan bir ontoloji ile
  çürütülebilir mi)?"* → Evet → **`constitutive: false`** (ampirik-yeterlilik ekseninde; M-eksenini
  taşır, skeptic-kazanılmış grade'ini korur); Hayır → `constitutive: true`.

**Sıra kuralı ve hibrit çözümü:** Test C, hibrit belgelerde (hem şema tanımlar hem alan-yeterliliği
iddia eder) **öncelikli**dir. Bir belge şema-parçası taşısa bile, sınanabilir bir yeterlilik iddiası
taşıyorsa `constitutive: false` sınıflanır — çünkü P8/Madde X'in en çok yanlışlanabilir tutması
gereken şey o ampirik-yeterlilik iddiasıdır. Belge-içi salt-tanımsal parçalar tanımlayıcı gücünü
**yitirmez** (aşağı-akış onlara başvurmaya devam eder); yalnızca belgenin bütünsel doğrulaması,
yeterlilik-sınamasını da içerir. Bu, **belge-düzeyi tek bayrağı** korur (bölüm-düzeyi şema
şişkinliğinden kaçınır); saf-şema bir kurucu yapıt *çıkarmak* (ör. ENS-4001'in tip-şema çekirdeğini
ileride ayırmak) bir gelecek seçeneğidir, bu RFC'nin zorunluluğu değil.

**Turnusol birincildir; künye-invariant'ı yalnızca bir tutarlılık-kontrolüdür (D7 — SKR-035).**
Sınıflamayı belirleyen **her zaman** Test A/B/C'dir. §7.3'ün `maturity`-tabanlı invariant'ı bir
*sınıflayıcı değil*, yalnızca tek-yönlü bir **gerekli-koşul denetimidir** (`constitutive: false`
yapıt `maturity` taşımalıdır — dolayısıyla `maturity` **taşımayan** bir yapıt `constitutive: true`'dur).
Ters yön geçerli değildir: `maturity` **taşıyan** bir yapıt turnusola göre `constitutive: true` de
olabilir (governance örneği, §8.2). Turnusol ile invariant çeliştiğinde **turnusol kazanır**; invariant
yalnızca "bir `constitutive: false` yapıtın M-eksenini yanlışlıkla düşürmediğimizi" doğrular. Bu
öncelik, invariant'ı deponun olumsal etiketleme alışkanlıklarına (Faz-erken her belgeye `maturity`
kondu — G-04) aşırı-bağımlı olmaktan kurtarır.

## 5. Önerilen değişiklik — Madde IV (eski ↔ yeni)

Değişen yalnızca Madde IV'ün son paragrafıdır (satır 74-77); aralık tablosu (satır 66-72) korunur
ama başlığı yeniden çerçevelenir: **aralık, yapıtın alanını belirler; canon'unu değil.**

### 5.1 ESKİ metin (ENS-0000 Madde IV, satır 74-77)

> Külliyat üyeliği [`KULLIYAT.md`](../KULLIYAT.md) dosyasında kayıtlıdır ve her yapıtta
> `canon: true` ile beyan edilir. **0/1/3/4 aralıkları doğası gereği Külliyat'tır.** Bir
> `2000-theory` kavramı ancak failure conditions'ını (başarısızlık koşullarını) belirtip
> skeptic incelemesinden sağ çıkınca Külliyat olur.

### 5.2 YENİ metin (öneri)

> Aralık, bir yapıtın **alanını** (hangi tür içeriğe ev sahipliği yaptığını) belirler; Külliyat
> üyeliğini (`canon`) tek başına belirlemez. Her Külliyat yapıtının iki **bağımsız** niteliği vardır:
>
> - **`constitutive`** — yapıt, ENS akıl yürütmesini *olanaklı kılan* bir kural, tanım ya da
>   tip-şeması mıdır (normatif; "X, C bağlamında Y sayılır" biçiminde kurucu), yoksa dünya hakkında
>   **yanlışlanabilir ampirik** bir iddia mı taşır? `constitutive: true` yapıt tanımla yürürlüktedir;
>   `constitutive: false` yapıt kanıtla.
> - **`canon`** — yapıt doğrulanıp Külliyat'a girmiş midir? Kazanılır, ilan edilmez.
>
> **Külliyat üyeliği hiçbir aralıkta aralığın kendisinden doğmaz:**
>
> - **`0` (Anayasa)** biricik **kendi kendini yetkilendiren** yapıttır: `constitutive: true`,
>   `canon: true`. Bu, ad hoc bir istisna değil, aksiyomatik bir sistemin **ilkeli zorunluluğudur:**
>   sonsuz gerilemeyi (Münchhausen trilemması) durdurmak için sistem tek bir öz-yetkilendiren köke —
>   Kelsen'in *Grundnorm*'una / Hart'ın *rule of recognition*'ına — dayanmak zorundadır. Onu üstten
>   doğrulayan bir yapıt yoktur; yanlışlanabilirlik ödevini (Madde X) kendi değişiklik yordamı
>   (Madde XV: RFC + skeptic) üzerinden yerine getirir. **Anayasa'nın kendisi tekdüze `constitutive`
>   değildir** (§4.1): Madde III (P1-P8) ampirik yanlışlamaya kapalı **immutable-core** (Lakatosçu
>   hard core); Madde I/II/IV-XIV ise normal RFC ile revize edilebilen **protective belt**'tir. Bu
>   RFC'nin Madde IV'ü değiştirmesi, ikincinin revize-edilebilirliğinin kanıtıdır.
> - **`constitutive: true` yapıtlar** (Anayasa; felsefenin kurucu tezleri; yasa-çerçevesi;
>   ontolojinin tip/şema belgeleri; governance kuralları — 1/3/4 aralıklarında bulunabilir)
>   `canon: true` olmak için **ratifikasyon** yolunu izler: failure conditions'ını **tutarlılık/
>   örneklenebilirlik** kipinde belirtir (tanım tutarsız mı, örneklenemez mi, başka bir Külliyat
>   kuralıyla çelişir mi, daha iyi bir ayrım var mı) ve `ens-skeptic` bu tutarlılık incelemesinden
>   sağ çıkar. Ampirik kanıt zincirine (M5 / Faz-4) **tabi değildir**, çünkü ampirik iddia taşımaz.
> - **`constitutive: false` yapıtlar** (ampirik teori ve yasalar; `2000` aralığı doğası gereği
>   böyledir, `1/3/4` de olabilir) `canon: true` olmak için failure conditions'ını belirtip skeptic
>   incelemesinden sağ çıkarak `ratified` (M3) olur; **tam Canon (M5)** yalnızca reference platform
>   kanıt zinciriyle (GOV-030, Faz 4) kazanılır (bkz. [`maturity-model.md`](../.claude/standards/maturity-model.md)).
>
> Her iki durumda da failure conditions **zorunludur** (Madde X); yalnızca *yanılma kipi* türe göre
> değişir. Külliyat üyeliği [`KULLIYAT.md`](../KULLIYAT.md) dosyasında kayıtlıdır.

Aralık tablosu başlık cümlesi (satır 64) şu şekilde güncellenir: *"Külliyat, numaralı deponun alt
aralıklarını işgal eder — aralık **alanı** belirler, canon'u değil:"*.

## 6. Tutarlılık analizi (Anayasa'nın diğer maddeleriyle)

- **Madde III / P8 (teori implementasyondan önce).** Yeni kural P8'i **güçlendirir:** ampirik
  kavramlar (`constitutive: false`) tam Canon'a (M5) ancak Faz-4 kanıtıyla ulaşır — "Canon, kod
  teoriyi kanıtlayınca dolar." Kurucu belgelerin canon'u P8'i ihlal etmez, çünkü onlar ampirik
  iddia *yapmaz*; kanıtlanacak bir öngörüleri yoktur. Böylece "her canon Faz-4 ister" gibi yanlış
  bir aşırı-genelleme önlenir; yalnızca ampirik canon Faz-4 ister.
- **Madde X (yanlışlanabilirlik ödevi).** Yeni kural Madde X'i **protective belt için
  keskinleştirir**, immutable-core için ise sınırını dürüstçe kabul eder (§4.1). `constitutive`
  bir muafiyet **değildir**; failure conditions her iki türde de zorunludur. Redefinisyon *yanılma
  kipini* türe uygun hale getirir (tutarlılık-yanılması vs öngörü-yanılması). Bu, "tanımlar
  saldırılamaz" kaçamağını protective-belt kurucu belgeler için kapatır — bir tip-şeması
  örneklenemezlik/iç-çelişki ile çürütülebilir kalır (ENS-4010 `part_of` tipleme kusuru, ENS-4025
  D-1 bu kipin işlediğini gösterir). **Dürüst sınır (SKR-034 W2):** immutable-core (ENS-0000 Madde
  III / P1-P8) için bu koruma *ampirik düzeyde değil, yalnızca program/tutarlılık düzeyinde*
  geçerlidir — P1'i bir gözlemle çürütemezsiniz. Bu bir kaçamak değil, işaretlenmiş bir Lakatosçu
  hard-core taahhüdüdür (§3, §4.1); RFC "Madde X her yerde keskinleşti" diye **aşırı-genellemez**.
- **Madde XI (evrensel künye).** `constitutive` yeni bir zorunlu alandır; şema değişikliği §7'de.
- **Madde XIV/XV (yönetişim).** Bu RFC her iki yordamı da karşılar: Madde XV (Anayasa değişikliği:
  atıf + skeptic + Madde XIV kabul) ve Madde XIV (standart değişikliği: RFC yaşam döngüsü).

## 7. Önerilen değişiklik — künye şeması (`metadata-header.md`)

Bu RFC, Anayasa değişikliğinin **ayrılmaz sonucudur**; bilerek **tek RFC** olarak sunulur.
Gerekçe: `constitutive` alanı, Madde IV kuralı olmadan anlamsız; Madde IV kuralı da alan olmadan
ifade edilemez. İkisini ayrı RFC'lere bölmek, biri ratifiye edilip diğeri edilmediğinde tam da
G-03/05'in işaret ettiği tutarsızlığı yeniden yaratırdı. Madde XIV, bir RFC'nin "Külliyat'ta,
mimaride **ya da** standartlarda" değişiklik önermesine izin verir — çoklu-katman tek RFC
kapsamındadır. Şema kısmının eş-sahibi `ens-style-guardian`'dır (STD-METADATA-HEADER owner'ı).

### 7.1 Şema bloğuna eklenecek alan (§Şema)

```yaml
constitutive:  true               # bool — Külliyat yapıtları için zorunlu (kök varsayılan: false)
                                   #   true  = normatif/kurucu (tanımla yürürlükte): kural, tanım,
                                   #           tip/şema belgesi; canon'u ratifikasyonla kazanır
                                   #   false = ampirik teori/yasa: canon'u kanıt zinciriyle (M5) kazanır
```

### 7.2 §Değer kümeleri — `canon` satırının yeni hali

- **ESKİ:** `canon`: `true | false` (bkz. KULLIYAT.md) — **yalnızca `maturity: M5` ise true**
- **YENİ:** `canon`: `true | false` — kazanılır, ilan edilmez.
  `constitutive: false` (ampirik) yapıtta **yalnızca `maturity: M5` ise true**;
  `constitutive: true` (kurucu) yapıtta **ratifiye edilip skeptic tutarlılık incelemesinden sağ
  çıkınca** true — maturity/evidence eksenine tabi değil.

### 7.3 §Kurallar'a eklenecek madde (SKR-034 D1 + SKR-035 D6 — künyelerle tutarlı, tek-yönlü teşhis)

> 7. `constitutive: false` yapıtlar `maturity` ve `evidence` alanlarını **taşır** ve
>    `canon: true ⇔ maturity: M5`. `constitutive: true` yapıtlar canon'unu **ratifikasyonla**
>    (kurucu-tutarlılık skeptic incelemesi) kazanır; M5/Faz-4 kanıt zincirine **tabi değildir**.
>    Bir `constitutive: true` yapıt `maturity` alanı **taşıyabilir ama zorunlu değildir** — taşıdığı
>    yerde bu etiket **canon'unu gate etmez** (aşağıda ayrıştırma). `failure_conditions` her iki
>    türde de zorunludur (Madde X — kurucu için tutarlılık/örneklenebilirlik kipinde).

**SKR-035 D6'nın kökü: eski `⟺` invariant'ı fazla iddialıydı.** SKR-034'e yanıtta (v0.2) §7.3'ü
çift-yönlü bir invariant'a — *"maturity taşıyor **ve ancak öyleyse** constitutive:false"* — çevirdik.
SKR-035 bunu bağımsız korpus taramasıyla (`grep ^maturity:`) çürüttü: RFC'nin `constitutive: true`
ilan ettiği **governance ailesi (GOV-000/010/020/030) fiilen `maturity: M1` + `evidence` taşıyor**
(bağımsız doğrulandı, §8.2). Çift-yönlü invariant tek bir karşı-örnekle düşer. Kök neden, invariant'ın
deponun **olumsal etiketleme alışkanlığını** (Faz-erken her belgeye `maturity` konması — G-04) normatif
bir çift-yönlü kurala **terfi ettirmesiydi**. Düzeltme: invariant **tek-yönlü** bir gerekli-koşula
indirgenir ve turnusol (§4.2) birincil sınıflayıcı olur.

> **Teşhis (tek-yönlü invariant, D6 düzeltmesi):**
> - **Geçerli yön:** `constitutive: false` (ampirik) yapıt `maturity`/`evidence` **taşımalıdır**.
>   Karşıt-önerme (contrapositive) de geçerli: `maturity` **taşımayan** yapıt `constitutive: true`'dur
>   — ENS-0000 ve ENS-4000 (künyelerinde `maturity` yok) böylece kesin kurucudur.
> - **Geçersiz yön (eski `⟺`'nın düşen tarafı):** `maturity` taşıyan her yapıt `constitutive: false`
>   **değildir**. `maturity` taşıyan bir yapıt, turnusola göre `constitutive: true` de olabilir
>   (governance: kurucu prosedür-kuralı **ama** olumsal/miras bir `maturity: M1` etiketi taşıyor).
> - **Sınıflayıcı turnusoldur (§4.2), invariant değil.** İnvariant yalnızca bir tutarlılık-kontrolüdür:
>   bir `constitutive: false` yapıtın M-eksenini yanlışlıkla düşürmediğimizi doğrular. Turnusol ile
>   invariant çeliştiğinde turnusol kazanır (§4.2, D7).
>
> **Maturity etiketi ↔ canon yolu ayrıştırması (governance çözümü):** `constitutive: true` bir yapıtta
> `maturity` alanı bulunabilir; bu, o yapıtın **ne kadar iyi uygulandığını/benimsendiğini** izleyen
> bilgi-amaçlı bir eksendir (governance'ın `eng/ops/econ` evidence'ı: prosedür pratikte işletildi mi),
> ama **canon'unu belirlemez**. Governance canon'unu — 4000-ontolojileri gibi M5'e tırmanarak değil —
> **kurucu-tutarlılık incelemesiyle** kazanır (§4.2 Test A/C: yönetişim kaldırılırsa roller/yetkiler
> tanımsızlaşır → kurucu; sınanabilir sentetik-yeterlilik iddiası yok → ampirik değil). Böylece §7.2'nin
> özü korunur: **kurucu yapıt M5/Faz-4 kanıt zincirinden muaftır** — muafiyeti zayıflatan tek şey, artık
> `maturity` alanının *yokluğunu* bunun makine-okunur işareti saymamamızdır (governance karşı-örnek).
> Retrofit **hiçbir skeptic-kazanılmış grade'i sökmez**: `constitutive: false` ontolojiler M2/M3'ünü
> korur; governance `maturity: M1`'ini korur (yalnızca canon-gate rolünden çıkar). `maturity-model.md`
> ve `KULLIYAT.md` bu ayrıştırmayla hizalanır (§10.5).

### 7.4 §Şema'ya eklenecek isteğe-bağlı alan (immutable-core, SKR-034 D2)

> ```yaml
> immutable_core_sections: [Madde III]   # opsiyonel — yalnızca hard-core taşıyan yapıtta; §4.1
>                                        # şu an yalnızca ENS-0000. Ampirik yanlışlamaya kapalı
>                                        # (Madde XV); Madde X'i yalnızca program-düzeyinde karşılar.
> ```

Bu alan **zorunlu değildir** ve korpus geneline yayılmaz; tek locus (ENS-0000 Madde III) için
`constitutive: true` sınıfının heterojenliğini (hard core vs protective belt) künyede görünür kılar.
Başka bir yapıt hard-core statüsü iddia edene dek genişletilmez.

**İmmutable-core verme yordamı (D8 — SKR-035).** Hard-core statüsü — bir yapıtı ampirik yanlışlamaya
metodolojik kararla kapatmak — Külliyat'ta yalnızca **Madde XV Anayasa değişikliği** ile verilebilir;
hiçbir owner ya da sıradan RFC bir yapıtı tek taraflı immutable-core ilan edemez. Şu an bu statü
yalnızca ENS-0000 Madde III'e (P1-P8, Madde XV tarafından *yerinde* korunmuş) uygulanır. Gelecekte
başka bir yapıt için istenirse, `immutable_core_sections` alanının o yapıta eklenmesi ayrı bir
Madde XV değişikliği + `ens-skeptic` turu gerektirir. Bu, "kendini hard-core ilan ederek Madde X'ten
kaçma" yüzeyini yordamsal olarak kapatır (ampirik bir belge bunu yapamaz: hard core tanım gereği
sınanabilir yeterlilik iddiası taşımaz — Test C onu yakalar).

### 7.5 Kabul kapısı çift-owner'a bağlanır (SKR-034 D4)

Bu RFC iki farklı yetki alanına dokunur: **Madde IV içeriği** (`ens-philosopher` / hiza `ens-ceo`)
ve **künye şeması** (`STD-METADATA-HEADER` owner'ı `ens-style-guardian`). Tek-RFC senkron erdeminin
(§7 giriş) kabul aşamasında tek-imzayla yeniden riske girmemesi için, `Accepted` edimi **her iki
owner onayını** zorunlu kılar: (a) `ens-ceo` hiza incelemesi (Madde XIV) **ve** (b) `ens-style-guardian`
şema-imzası. İkisinden biri eksikse RFC `Accepted` olamaz.

## 8. Sınıflama — turnusol testinin uygulanması (SKR-034 D1/D3'e yanıt: göster, iddia etme)

Bu RFC yalnızca **kuralı ve şemayı** değiştirir; korpus geneli `constitutive` alanı eklemek ayrı,
sonraki bir adımdır (§10, ROADMAP). Ancak SKR-034 haklı olarak, ertelemenin en zor sınavı (4000
aralığı: şema mı, dünya-modeli mi?) gizlediğini ve eski §8 tablosunun **fiili künyelerle
çeliştiğini** gösterdi. Bu sürüm o borcu kapatır: §4.2 turnusolunu **her örneğe uygular** ve
sınıflamayı **gerçek künyelerle hizalar**. Nihai atama yine owner + skeptic ile kesinleşir, ama
gerekçe artık editöryel değil, gösterilmiş bir testtir.

### 8.1 Fiili künyesi doğrulanmış belgeler (turnusol uygulandı)

| id | Fiili künye (2026-07-24) | Test A (kaldırma) | Test B/C | Sonuç | Grade sökülür mü? |
|----|--------------------------|-------------------|----------|-------|-------------------|
| **ENS-0000** | `canon:true`, `maturity` yok | Kaldır → tüm yetki-zinciri tanımsız → **anlamsız** | B: Madde III fiat'la korunur (hard core); Madde IV-XIV revize-edilebilir | `true` — immutable-core (Md III) + protective-belt | Hayır (M taşımıyor) |
| **ENS-4000** | `canon:true`, `status:review`, `maturity` yok, **SKR yok** | Kaldır → tüm terimler tanımsız → **anlamsız** | C: alan-yeterliliği iddiası yok, saf adlandırma-registry → hayır | `true` (kurucu) | Hayır — **ama canon:true şu an kazanılmamış (aşağıda 8.3)** |
| **ENS-4001** | `canon:false`, **`maturity:M2`**, `status:review` | Kaldır → node tipleri tanımsız → aday | C: "bu tipler ENS bilgi-grafiğini/örgütü doğru eklemliyor mu?" — senaryo stres testleriyle sınandı (ERP/bilişsel/IoT) → **evet** | `false` — M-ekseninde | **Hayır — M2 korunur** |
| **ENS-4010** | `canon:false`, **`maturity:M2`**, `status:review` | Kaldır → Node Registry tanımsız → aday | C: örgüt tip sistemi; `part_of` kusuru (SKR-030/031) gerçek yeterlilik/tutarlılık çürütmesi → **evet** | `false` — M-ekseninde | **Hayır — M2 korunur** |
| **ENS-4025** | `canon:false`, **`maturity:M2`**, `status:ratified` | Kaldır → çıkarım semantiği tanımsız → aday | C: sınır vaka (mantık-sözleşmesi) ama D-1 (SKR-031) **örneklenebilirlik çürütmesiyle** sınandı → yeterlilik iddiası **evet** | `false` (Test C) — M-ekseninde | **Hayır — M2 korunur** |
| **ENS-3021** | `canon:false`, `maturity:M3`, `status:ratified` | Kaldır → bir *iddia* (entropi yasası) kanıtsız kalır, aşağı-akış anlamsızlaşmaz | C: ölçülebilir ampirik öngörü (noise ölçüsü) → **evet** | `false` — M-ekseninde | Hayır (zaten M3) |

**Okunuş (D1 çözümü, D6 ile düzeltilmiş):** Bu tablodaki sınıflamayı belirleyen **turnusoldur**
(Test A/B/C, §4.2), invariant değil (D7). `maturity`-yokluğu yalnızca tek-yönlü bir *ipucu* olarak
kullanılır: `maturity` taşımayan ENS-0000/ENS-4000 kesin kurucudur (contrapositive geçerli). `maturity`
**taşıyan** belgeler ise turnusolla ayrılır — buradaki ENS-4001/4010/4025/3021 hepsi Test C'den
`constitutive: false` çıkar (sentetik-yeterlilik/örneklenebilirlik iddiası taşırlar), yani
`maturity: M2/M3`'leri **tutarlıdır ve korunur**. Eski §8'in hatası bu ontolojileri `constitutive:
true` saymasıydı; düzeltilmiş tablo **hiçbir grade sökmez**, yalnızca eksik `constitutive: false`
bayrağını ekler. **Not (D6):** `maturity` taşıyan her belge otomatik `false` değildir — governance
`maturity: M1` taşır ama turnusolla `constitutive: true`'dur (§8.2); bu yüzden §7.3 invariant'ı
çift-yönlü değil tek-yönlüdür. `constitutive: true` sınıfı yine de dar kalır (SKR-001 dersi:
savunulabilir küçük iddia > çürütülebilir büyük iddia): bugün ENS-0000, ENS-4000 ve governance (GOV-*).

### 8.2 Owner'ın aynı turnusolu uygulayacağı belgeler (leaning belirtilir, dayatılmaz)

| id | Leaning | Turnusol notu |
|----|---------|---------------|
| ENS-1000 (Manifesto/Ana Tez) | hibrit | Kurucu-tez parçaları (programı olanaklı kılar) `true` eğilimli; örgüt hakkındaki ampirik iddiaları `false`. Owner (ens-philosopher) Test A/C'yi bölüm bazında uygular; belge M taşımıyorsa çekirdek-tez baskın |
| ENS-3000 (Enterprise Laws çerçevesi) | `true` aday | "Yasa nedir" tanımı + LAW-* registry = çerçeve (kurucu). Somut yasalar (ENS-3021/22/23) `false`. M taşımıyorsa çerçeve `true` |
| ENS-4020 (Enterprise Ontology) | `false` | Test C: örgüt tip sistemi bir gerçek alanı eklemler, senaryoyla sınandı (SKR-030) → **evet** → `false`. `maturity:M2` bu sonuçla tutarlı, korunur |
| ENS-4030 (Axioms) / ENS-4031 (Inference Rules) | owner uygular | Test C birincil: sınanabilir örneklenebilirlik/yeterlilik iddiası varsa `false` (mevcut `maturity:M2`/`M0` bununla tutarlı); saf-aksiyom/kural çekirdeği ayrıştırılıp yeterlilik iddiası taşımıyorsa Test B (tutarlılık-kipi) ile `true` aday |
| GOV-000..030 | `true` | Test A: kaldır → yönetişim rolleri/yetkileri tanımsız → **kurucu**. Test C: sentetik-yeterlilik/ampirik-öngörü iddiası yok → `true`. **Fiilen `maturity:M1`+`evidence` taşır** (bağımsız doğrulandı) ama bu, canon'u gate etmeyen **olumsal/uygulama-izleme** etiketidir (§7.3 ayrıştırma); turnusol `true`, invariant tek-yönlü olduğu için çelişki **yok** |
| ENS-2001..2004 | `false` | Ampirik teori; M3, öngörü-kipiyle yanılır |
| ENS-3022 / ENS-3023 | `false` | Ampirik yasa; M3 |

**Sınır vakası dürüstlüğü (failure-condition #1):** ENS-4025/4030/4031 gerçekten sınırda — mantık/
aksiyom sözleşmeleri hem tutarlılıkla (kurucu kip) hem örneklenebilirlikle sınanır. Bu RFC bunları
**metafizik olarak** "ampirik" ilan etmez (Quine'a saygı, §3 Carnap); **turnusolu** uygular: bu
belgeler Test C'den sınanabilir bir yeterlilik/örneklenebilirlik iddiası taşıdıkları için
`constitutive: false` doğrulanır (mevcut `maturity` künyeleri bu sonuçla *tutarlıdır*, ama sınıflamayı
`maturity`'nin **varlığı** değil turnusol belirler — governance karşı-örneği bunu gerektirir, §8.2 GOV
satırı). Biri saf-sözleşme çekirdeğini *ayırmak* isterse, o çekirdek ayrı bir RFC + skeptic turuyla
`constitutive: true` işaretlenip M-ekseninden çıkarılabilir. Bu, sınır vakasını **gizlemeden**
karara bağlar.

### 8.3 ENS-4000 canon borcu (SKR-034 yan bulgusu — poster-çocuk kendi kuralını ihlal ediyor)

SKR-034 haklı: ENS-4000 `canon: true` ama `status: review` ve **hiç SKR geçmemiş** — RFC'nin kendi
"canon ilan edilmez, kazanılır" sloganını en görünür yerde ihlal ediyor. Bunu örneği çıkararak
gizlemiyoruz; **RFC'nin kuralının uygulanması** olarak açığa alıyoruz: ENS-4000 gerçek bir
`constitutive: true` poster-çocuğudur (saf adlandırma-registry), ama `canon: true`'su şu an
**kazanılmamıştır**. Kabul sonrası retrofit (§10) şunu zorunlu kılar: ENS-4000 canon'unu bir
**kurucu-tutarlılık skeptic incelemesiyle** (tanım-tutarlılığı, çelişkisiz-adlandırma, terminoloji
kapanışı) kazanır **ya da** kazanana dek `canon: false`'a iner. Bu, RFC'nin kuralının kendi örneğine
uygulanmasıdır — istisna değil.

### 8.4 Bu RFC kapsamında düzenlenecek dosyalar

**Kabul edilirse yalnızca:** `ENS-0000` (Madde IV) ve `metadata-header.md` (şema; §7.1/7.4).
Diğer tüm belgelerin `constitutive` alanı ve ENS-4000 canon-incelemesi **sıradaki adımda** (§10)
ilgili owner + skeptic ile yapılır. Bu RFC hiçbir künyeyi şimdi düzenlemez.

## 9. Bu önerinin failure conditions'ı (Madde X, bu RFC'ye uygulanır)

Bu RFC şu koşullarda **yanlış/eksik** sayılmalıdır:

1. **Sınıflama ikircikli belge.** Bir belge hem kurucu tanım hem ampirik iddia taşıyorsa (ör. bir
   teori-belgesi içine gömülü bir tip-şeması) `constitutive` tekil bir değer veremez. → Yanıt/sınır:
   §4.2 turnusolu bu ikircikliği **karara bağlar, gizlemez**: Test C öncelikli (sınanabilir
   yeterlilik iddiası varsa `false`). Turnusol **birincil sınıflayıcıdır**; `maturity`-yokluğu
   yalnızca tek-yönlü bir ipucudur (`maturity` yok → `true`), ama `maturity`'nin *varlığı* tek başına
   sınıflamaz (governance karşı-örneği, §8.2). Salt-şema çekirdeği ayrılmak istenirse ayrı RFC +
   skeptic turu gerekir. Yine de sınır vakalarının *sık* ve turnusolla-çözülemez çıkması, alanın kötü
   tanımlı olduğuna kanıt olur (bu RFC'yi yaralar) — bu yüzden §8'de en zor aralık (4000) açıkça
   sınandı, ertelenmedi.
2. **Kurucu belgenin failure conditions'ı içi boşalırsa.** "Tutarlılık/örneklenebilirlik yanılması"
   pratikte hiçbir kurucu belgeyi çürütemiyorsa, ayrım Madde X'i fiilen zayıflatmış olur (Quine
   itirazının gerçekleşmiş hali). → Karşı-kanıt talebi: her `constitutive: true` belgede *işleyen*
   bir yanılma kipi gösterilebilmeli (ENS-4010 `part_of`, ENS-4025 D-1 bunun mevcut kanıtı).
3. **`constitutive` kötüye kullanımı (canon kaçamağı).** Bir yazar ampirik bir iddiayı skeptic'ten
   ve Faz-4 kanıtından kaçırmak için belgeyi `constitutive: true` etiketlerse, P8 delinir. →
   İki savunma: (i) §4.2 turnusolu editöryel takdiri sınırlar — Test C bir sınanabilir yeterlilik
   iddiası bulursa belge `constitutive: false`'tur, `true` etiketi turnusolla çürütülür. **Bu
   savunma tümüyle turnusola dayanır — `maturity` alanının yokluğuna değil** (SKR-035 D6: `maturity`
   varlığı/yokluğu artık sınıflayıcı değil, çünkü governance `maturity` taşıyan bir `constitutive:
   true`'dur; kaçamağı yakalayan mekanizma Test C'dir, invariant değil). (ii) Sınıflama
   skeptic-itiraz edilebilirdir; `constitutive` **owner'ın tek taraflı ilanı değildir**, skeptic
   reddedebilir. Böylece kaçamak yalnızca skeptic'in uyanıklığına değil, gösterilebilir bir teste
   bağlanır (SKR-034 W3'e yanıt).
4. **Şema-kural senkron kaybı.** Madde IV metni ile `metadata-header.md` §Değer kümeleri zamanla
   ayrışırsa çelişki geri gelir. → Tek RFC + tek kabul edimi bunu şimdilik önler; uzun vadede
   formal-checker (G-09/10) invariant'ı gerekir: "her `canon: true` belge, `constitutive` değerine
   uygun doğrulama yolunu kanıtlıyor mu?".
5. **Maturity–canon ayrıştırması pratikte çökerse (D6 düzeltmesinin kendi yanılma koşulu).** §7.3
   bir `constitutive: true` yapıtın `maturity` etiketini "canon'u gate etmeyen, olumsal/uygulama-izleme"
   olarak ayrıştırır. Bu ayrıştırma şu durumda çöker: eğer governance'ın `evidence` bloğu (`eng/ops/
   econ`) fiilen **sınanabilir ampirik-yeterlilik iddiaları** ("bu prosedür pratikte iyi karar üretir")
   yükleniyorsa, o zaman governance salt prosedür-tanımı değildir ve Test C onu `constitutive: false`'a
   çevirmelidir — bu durumda D6 çözümü (a) değil (b) doğru olurdu. → Karşı-kanıt talebi: governance'ın
   `maturity`/`evidence` alanları **prosedürün-tanımını** değil yalnızca **prosedürün-benimsenmesini**
   izlediği gösterilebilmeli. Governance kaldırıldığında roller/yetkiler *tanımsız* kalıyorsa (Test A
   `true` verir), iddia prosedür-tanımıdır (kurucu); ama governance yalnızca "şu süreç şu sonucu
   verir" öngörüsü taşıyıp kaldırılınca aşağı-akış *anlamlı* kalıyorsa, iddia ampiriktir. Bu ayrım
   governance dosyaları büyüdükçe yeniden sınanmalıdır; şimdilik Test A açıkça `true` veriyor
   (roller/capability-matrix/canonical-process kaldırılırsa yönetişim eklemlenemez).

## 10. Migration ve sıradaki adımlar

1. **Bu RFC (v0.2) → bağımsız 2. `ens-skeptic` turu** (§9'a saldırı; Madde XV-b; G2/G3 gereği yazar
   kendi düzeltmesini onaylamaz). — *sıradaki adım.*
2. Skeptic `survives` → **çift-owner kabul kapısı** (§7.5): `ens-ceo` hiza incelemesi (Madde XIV)
   **ve** `ens-style-guardian` şema-imzası → `Accepted`.
3. `Accepted` sonrası **ayrı** edimler (bu görevin kapsamı DEĞİL): (a) ENS-0000 Madde IV fiilen
   düzenlenir; (b) `metadata-header.md` şeması güncellenir (§7.1/7.4); (c) korpus retrofit'i (§8
   tabloları) belge-owner'ları + skeptic tarafından yapılır — **§8.1 gereği hiçbir M-grade
   sökülmez**, yalnızca `constitutive: false` bayrağı eklenir; (d) `KULLIYAT.md` "gap #1" notu
   kapatılır.
4. **ENS-4000 canon-borcu (§8.3):** retrofit sırasında ENS-4000 canon'unu kurucu-tutarlılık skeptic
   incelemesiyle **kazanır** ya da `canon: false`'a iner. `ens-style-guardian` (owner) yürütür.
5. **`maturity-model.md` / `KULLIYAT.md` hizalama (D1'in eş-düzeltme yükü — sessiz geçilmez;
   D6 ile düzeltildi):** Bu RFC **grade sökmediği** için ikisinde de re-grade gerekmez; yalnızca
   *notlandırma* eklenir. **Olgusal-doğru carve-out (SKR-035 D6 — eski cümle yanlıştı):**
   `maturity-model.md`'ye şu iki-parçalı not eklenir: (i) **ENS-0000 ve ENS-4000** künyelerinde
   `maturity` alanı **yoktur** ve M-eksenine tabi değildir; canon'u kurucu-tutarlılık yoluyla kazanır.
   (ii) **Governance (GOV-000..030)** `constitutive: true`'dur **ama** fiilen `maturity: M1` + `evidence`
   taşır; bu etiket **olumsal/uygulama-izlemedir ve canon'u gate etmez** — governance da canon'unu
   kurucu-tutarlılık incelemesiyle kazanır, M5/Faz-4 zinciriyle değil. (Eski v0.2 taslağının
   *"constitutive:true yapıtlar (ENS-0000, ENS-4000, GOV-*) M-ekseninde değildir"* cümlesi GOV-* için
   **olgusal yanlıştı** — GOV-* M-ekseninde bir etiket taşır; bu sürüm düzeltir.) `KULLIYAT.md`'de
   "Normatif/Constitutive çekirdek" ile "Teorik Canon (M5)" ayrımı bu RFC'ye atıfla resmîleşir. İkisi de
   `Accepted` sonrası owner edimleri; bu RFC şimdi düzenlemez.

## 11. SKR-034'e yanıt

**SKR-034 (2026-07-24, bağımsız context) → verdict: `wounded`.** Çekirdek tez (iki dik eksen; canon
türe-uygun doğrulama yolundan kazanılır; Madde IV yeniden yazımı) + tek-RFC kararı sağ çıktı; üç
blocking + iki keskinleştirme talebi bu sürümde (v0.1.0 → **v0.2.0**, `status: skeptic-challenged →
review`) karşılandı.

| Talep | Özet | Nasıl karşılandı | Durum |
|-------|------|------------------|-------|
| **D1** (blocking) | §7.3 ile ENS-4001/4010/4025'in fiili `maturity:M2`'si çelişiyor; retrofit yönü belirsiz + eş-düzeltme yükü sessiz | §8.1 **yeniden yazıldı**: kök neden §7.3 değil, §8'in yanlış sınıflamasıydı. ENS-4001/4010/4025 → `constitutive: false` (M-ekseninde, **M2 korunur, grade sökülmez**). §7.3 künyelerle tutarlı bir **invariant**'a dönüştü (M taşıyan ⟺ `false`). §10.5 maturity-model.md/KULLIYAT.md eş-notlandırma yükünü açıkça üstlendi | **kapatıldı** |
| **D2** (blocking) | `constitutive:true` heterojen: kök (Md III) ampirik çekirdeği tutarlılıkla dokunulamaz; Lakatos hard-core gerek | §4.1 eklendi: **immutable-core (hard core) vs revisable-constitutive (protective belt)** ayrımı; §3'e **Lakatos** prior-art'ı; §7.4 opsiyonel `immutable_core_sections` alanı; §6 Madde X iddiası "yalnızca protective belt için keskinleşir, immutable-core için ampirik-düzeyde bilinçli açık" diye **nitelendi** (dürüst asimetri) | **kapatıldı** |
| **D3** (blocking) | 4000-aralığı sınıflaması ertelenmiş ama §8 çelişkili taahhüt veriyor; işleyen turnusol gerek | §4.2 eklendi: **Test A (kaldırma) / B (yanılma-kipi) / C (yeterlilik)** turnusolu; §8.1'de **her fiili-künye örneğine uygulandı** (gösterildi, iddia edilmedi); §8.2 owner-leaning'leri turnusolla gerekçelendirir; erteleme kaldırıldı | **kapatıldı** |
| **D4** (keskinleştirme) | Kabul kapısı çift-owner olmalı | §7.5 eklendi: `Accepted` = `ens-ceo` hiza **ve** `ens-style-guardian` şema-imzası; §10.2'de yinelendi | **kapatıldı** |
| **D5** (keskinleştirme) | Prior art: Kelsen/Hart + Carnap | §3'e **Kelsen *Grundnorm* / Hart *rule of recognition*** (öz-yetkilendiren kökün ilkeli zorunluluğu, §5.2'de kullanıldı) + **Carnap internal/external** (Quine'a dayanıklı çerçeve-göreli analitiklik) eklendi | **kapatıldı** |
| **Yan bulgu** | ENS-4000 `canon:true` ama `status:review`, SKR yok — RFC kendi canon-kuralını ihlal | §8.3 eklendi: örnek çıkarılmadı, **RFC kuralının uygulaması** olarak açığa alındı — ENS-4000 canon'unu kurucu-tutarlılık incelemesiyle kazanır ya da `canon:false`'a iner (§10.4 retrofit) | **kapatıldı** |

> **⚠️ Öz-onay YOK (G2/G3).** Bu tablo taleplerin *karşılandığını* iddia eder; **`survives` ya da
> "kabul edildi" demez.** Verdict yalnızca **bağımsız 2. `ens-skeptic` turuyla** verilir. SKR-035 bu
> turu yaptı (aşağıda).

Ayrıntı: [`reviews/SKR-034-rfc-6001-constitutive.md`](reviews/SKR-034-rfc-6001-constitutive.md).

## 12. SKR-035'e yanıt (bağımsız 2. tur)

**SKR-035 (2026-07-24, bağımsız/taze context) → verdict: `wounded`.** SKR-035, SKR-034'ün
D2/D3-çekirdek/D4/D5 taleplerinin **gerçekten** kapatıldığını depodaki fiili künyeleri bağımsız
okuyarak doğruladı (Lakatos immutable-core kaçamak değil; turnusol ENS-4000 & ENS-3021'de tutarlı;
çift-owner kapı sağlam; Kelsen/Hart/Carnap atıfları gerçek). Çekirdek tez (iki dik eksen, tek atomik
RFC) yine sağ çıkıyor. Bir yeni blocking (D6) + iki keskinleştirme (D7/D8) v0.2.0 → **v0.3.0**'da
karşılandı (`status: skeptic-challenged → review`).

| Talep | Özet | Nasıl karşılandı | Durum |
|-------|------|------------------|-------|
| **D6** (blocking) | `maturity ⟺ constitutive:false` çift-yönlü invariant'ı (§7.3/§8.1) governance ailesinin fiili `maturity:M1` künyeleriyle çelişiyor; §10.5 "GOV-* M-ekseninde değildir" olgusal yanlış; §4.2 turnusolu (GOV→`true`) ile §7.3 invariant'ı (M taşır→`false`) zıt karar veriyor | **Kök neden onaylandı: invariant fazla iddialıydı.** SKR-035 talep-(a) seçildi. §7.3 **tek-yönlü** bir gerekli-koşula indirgendi: `constitutive:false ⇒ maturity taşır` (contrapositive: `maturity yok ⇒ true`); ters yön (`maturity var ⇒ false`) **kaldırıldı**. §4.2'de **turnusol birincil sınıflayıcı**, invariant yalnızca tutarlılık-kontrolü ilan edildi. Governance `constitutive:true` kalır; `maturity:M1` etiketi ile canon-yolu **ayrıştırıldı** (etiket olumsal/uygulama-izleme, canon'u gate etmez; canon kurucu-tutarlılıkla kazanılır). §8.1 "Okunuş", §8.2 GOV/4020/4030 satırları ve sınır-vaka paragrafı "invariant gereği"→"Test C gereği" olarak düzeltildi. §10.5 iki-parçalı olgusal-doğru carve-out'la yeniden yazıldı (ENS-0000/4000 M-ekseninde değil; GOV-* M-etiketi taşır ama gate etmez). §9'a failure-condition #5 (ayrıştırmanın kendi yanılma koşulu) eklendi. **Korpus geneli `grep ^maturity:` ile bağımsız doğrulandı** (aşağıda). | **kapatıldı** |
| **D7** (keskinleştirme) | Turnusol ↔ invariant önceliği belirsiz; Test A/B/C birincil olmalı | §4.2 sonuna açık öncelik kuralı eklendi: **sınıflayıcı her zaman turnusoldur**; §7.3 invariant'ı yalnızca tek-yönlü gerekli-koşul denetimi; çeliştiklerinde turnusol kazanır. §7.3 ve §8.1 bu çerçeveyle yeniden yazıldı | **kapatıldı** |
| **D8** (keskinleştirme) | ENS-0000 dışında immutable-core statüsünü hangi yordam verir? | §7.4'e eklendi: hard-core statüsü yalnızca **Madde XV Anayasa değişikliği + skeptic turu** ile verilir; owner/sıradan RFC tek taraflı ilan edemez; ampirik belge kendini hard-core ilan ederek kaçamaz (Test C yakalar) | **kapatıldı** |

**D6 için bağımsız korpus taraması (SKR-035'in yaptığı gibi, bu sefer owner da doğruladı).**
`grep ^maturity:` tüm depoda `maturity` taşıyan yapıtlar ve tek-yönlü invariant altında durumları:

| Grup | `maturity` | Turnusol sonucu | Tek-yönlü invariant'la tutarlı mı? |
|------|-----------|-----------------|-----------------------------------|
| ENS-2001..2004, ENS-3021..3023 | M3 | `false` (ampirik teori/yasa, Test C evet) | ✅ (`false` ⇒ M taşır) |
| ENS-4001/4010/4020/4025/4030 | M2 | `false` (sentetik-yeterlilik, Test C evet) | ✅ |
| ENS-4031 | M0 | owner uygular (Test C birincil) | ✅ (`false` ise M taşır; `true` ise ayrıştırma) |
| **GOV-000/010/020/030** | **M1** | **`true`** (Test A kurucu, Test C ampirik-iddia yok) | ✅ **artık çelişki değil** — `maturity var ⇒ false` yönü kaldırıldığı için `true`+M1 izinli |
| ENS-0000, ENS-4000 | **yok** | `true` (contrapositive: M yok ⇒ true) | ✅ |
| ADR-0001/0002 | M0 | **kapsam dışı** (5000-architecture, `type:adr`, owner ens-architect; Külliyat aralığı değil) | — (Madde IV/`constitutive` bu RFC'de yalnızca Külliyat yapıtlarına uygulanır) |

Not: SKR-035 governance dosyalarını "GOV-000/010/capability-matrix/canonical-process" diye andı;
fiili id'ler **GOV-000/GOV-010/GOV-020/GOV-030** (dosyalar `roles.md`=GOV-010, `capability-matrix.md`
=GOV-020, `canonical-process.md`=GOV-030) ve **dördü de** `maturity:M1`+`evidence` taşıyor —
bulgu doğru, kapsamı biraz daha geniş.

> **⚠️ Öz-onay YOK (G2/G3).** Bu tablo taleplerin *karşılandığını* iddia eder; **`survives` demez.**
> Verdict yalnızca **bağımsız 3. `ens-skeptic` turuyla** verilir — bu sürüm (v0.3) onu bekliyor.
> Yazar (ens-philosopher) kendi düzeltmesini onaylayamaz.

Ayrıntı: [`reviews/SKR-035-rfc-6001-constitutive-round2.md`](reviews/SKR-035-rfc-6001-constitutive-round2.md).
