---
id: SKR-035
type: skeptic-review
origin: RFC-6001
depends_on: [RFC-6001, SKR-034, ENS-0000, ENS-4000, STD-METADATA-HEADER, STD-MATURITY-MODEL]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
validation_dimension: constitutional
---

# SKR-035 — RFC-6001 (Constitutive Artifact Ayrımı) 2. Tur Saldırısı

**Bağımsızlık beyanı:** Bu inceleme, RFC-6001'i v0.1.0→v0.2.0'a düzelten `ens-philosopher`
çağrısından **ve** 1. tur SKR-034'ü yazan çağrıdan tamamen ayrı, taze context'te yapıldı
(G2/G3 — yazar kendi düzeltmesini onaylayamaz). Görev: v0.2.0 düzeltmelerinin SKR-034'ün D1/D2/D3
blocking + D4/D5 keskinleştirme taleplerini **gerçekten** karşılayıp karşılamadığını, yalnızca
RFC'nin kendi yanıt tablosuna (§11) güvenmeden, **depodaki fiili künyeleri bağımsız okuyarak**
sınamak. Tüm header okumaları 2026-07-24 tarihiyle doğrulandı.

## Verdict

`wounded` — D2 (Lakatos hard-core / immutable-core), D3'ün temiz vakaları (turnusol Test A/B/C),
D4 (çift-owner kapı) ve D5 (Kelsen/Hart/Carnap prior-art) **gerçekten kapatıldı**; çekirdek tez
(iki dik eksen, tek atomik RFC) yine sağ çıkıyor. Ancak D1'i kapatmak için getirilen mekanizma —
`maturity` taşıyan ⟺ `constitutive:false` **çift-yönlü invariant**'ı (§7.3/§8.1) — deponun fiili
künyeleriyle **hâlâ çelişiyor**: RFC'nin `constitutive:true` ilan ettiği **governance belgeleri
(GOV-000, GOV-010, `capability-matrix`, `canonical-process`) fiilen `maturity:M1` + `evidence`
taşıyor**. Bu, SKR-034 W1'in (RFC sınıflaması ↔ fiili künye çelişkisi) tam olarak **bir aralık öteye
taşınmış tekrarıdır**: yazar SKR-034'ün gösterdiği 4000-aralığı örneklerini düzeltti ama invariant'ın
"TÜM depoda tutuyor" iddiasını doğrulamak için geri kalanı denetlemedi. Dahası §10.5 açık bir olgusal
yanlış içeriyor: *"constitutive:true yapıtlar (ENS-0000, ENS-4000, **GOV-***) M-ekseninde değildir"* —
GOV-* M-ekseninde. Tek blocking talep (D6) karşılanınca `survives` erişilebilir; Faz durmaz.

## Yenilik incelemesi (D5 prior-art gerçek mi, savunulabilir mi?)

SKR-034 D5'in istediği iki eklenti gerçek ve doğru yerleştirilmiş — uydurma yok:

- **Lakatos, *hard core / protective belt*** (*The Methodology of Scientific Research Programmes*,
  1970) — gerçek, doğru kullanılmış; `immutable-core` ↔ `revisable-constitutive` eşlemesi meşru.
- **Kelsen *Grundnorm*** (*Reine Rechtslehre*, 1934) / **Hart *rule of recognition*** (*The Concept
  of Law*, 1961) — gerçek, tarih doğru; öz-yetkilendiren kökün "ad hoc istisna değil, aksiyomatik
  zorunluluk" gerekçesi için doğru prior-art. Münchhausen trilemması atfı da yerinde.
- **Carnap, internal/external questions** (*Empiricism, Semantics, and Ontology*, 1950) — gerçek;
  "çerçeveye-göreli analitiklik Quine'a Kant'tan dayanıklı" iddiası felsefe-tarihsel olarak doğru.

**Sonuç:** D5 gereksiz karmaşıklık değil — bunlar SKR-034'ün sahaya sürülmesini istediği tam
müttefikler ve doğru kullanılmışlar. Atıf uydurma testinden temiz geçiyor. D5 **kapatıldı.**

## D1 doğrulaması — invariant fiili künyelerle çelişmeye devam ediyor (**yeni blocking: D6**)

RFC §7.3'ü bir **çift-yönlü invariant**'a çevirdi: *"Bir belge `maturity` alanı taşıyorsa
`constitutive:false`'tur ve tersi"* (§7.3 Teşhis kutusu), ve §8.1 "sınıf çizgisi künyenin kendisinde
zaten kayıtlı" der. 4000-aralığı için bağımsız doğruladım — **bu kısım gerçekten düzeldi:**

| id | Fiili künye (2026-07-24, bağımsız okuma) | RFC v0.2 §8.1 | Tutarlı mı? |
|----|------------------------------------------|---------------|-------------|
| ENS-4001 | `canon:false, maturity:M2, status:review` | `constitutive:false`, M korunur | ✅ |
| ENS-4010 | `canon:false, maturity:M2, status:review` | `constitutive:false`, M korunur | ✅ |
| ENS-4020 | `canon:false, maturity:M2, status:review` | `constitutive:false` (§8.2) | ✅ |
| ENS-4025 | `canon:false, maturity:M2, status:ratified` | `constitutive:false`, M korunur | ✅ |
| ENS-4030 | `canon:false, maturity:M2, status:ratified` | `false` (M taşıyorsa, §8.2) | ✅ |
| ENS-0000 | `canon:true, maturity yok` | `constitutive:true` | ✅ |
| ENS-4000 | `canon:true, status:review, maturity yok, SKR yok` | `constitutive:true` + canon-borcu (§8.3) | ✅ |

Buraya kadar D1'in 4000-cephesi savunulur ve **hiçbir skeptic-kazanılmış M2/M3 grade sökülmüyor** —
SKR-034'ün korktuğu maliyet gerçekten oluşmuyor. §8.3 ENS-4000 canon-borcunu da dürüstçe açığa alıyor.

**Ama invariant, RFC'nin kendi `constitutive:true` listesindeki governance belgeleriyle kırılıyor.**
Fiili künyeler (bağımsız okuma, `grep ^maturity:` korpus geneli):

| id | Fiili künye (2026-07-24) | RFC v0.2 sınıflaması | Çelişki |
|----|--------------------------|----------------------|---------|
| GOV-000 | `type:standard, canon:false, **maturity:M1**, evidence:{sci:E1..}` | §4.1 tablo: "governance (GOV-*)" = **revisable-constitutive** (`true`); §8.2: GOV-000..030 = **`true`** | **§7.3 invariant ihlali:** M taşıyor ⇒ invariant `false` der, RFC `true` der |
| GOV-010 | `type:standard, canon:false, **maturity:M1**, evidence` | Aynı — `true` | Aynı |
| `capability-matrix` | `**maturity:M1**` | GOV ailesi, `true` | Aynı |
| `canonical-process` | `**maturity:M1**` | GOV ailesi, `true` | Aynı |

Ve **§10.5 açık olgusal yanlış:** *"constitutive:true yapıtlar (ENS-0000, ENS-4000, **GOV-***)
M-ekseninde değildir; canon'u kurucu-tutarlılık yoluyla kazanır."* GOV-000/010/... **M-ekseninde**
(hepsi `maturity:M1` + `evidence` bloğu taşıyor). §7.3'ün *"maturity taşımayan yapıtlar kurucudur"*
teşhis cümlesi de bu belgeler için yanlış — taşıyorlar ama RFC onları kurucu ilan ediyor.

**Bu neden W1'in tekrarı:** SKR-034 W1 tam olarak *"RFC §8 sınıflaması, fiili künyelerle çelişiyor
(maturity:M2 ↔ constitutive:true)"* idi. Yazar bunu **yalnızca kendisine gösterilen örnekler için**
(4000-aralığı) düzeltti, sonra invariant'ı *"künyenin kendisinde zaten kayıtlı"* / *"deponun bugünkü
fiili künyeleriyle çelişmez"* (§7.3) diye **evrensel** ilan etti — ama korpusu denetlemedi. Governance
tam da SKR-034'ün istediği türden bir karşı-örnek: `constitutive:true` iddia edilen bir yapıt ailesi
`maturity` taşıyor. İnvariant çift-yönlü (⟺) kurulduğu için **tek bir karşı-örnek onu çürütür.**

**Bu neden ölümcül değil ama ciddi:** Çekirdek tez (iki dik eksen) governance çelişkisinden
etkilenmez. Sorun invariant'ın **fazla iddialı** kurulmasında: aslında `maturity` alanı,
kavramsal bir doğrulama-rejimi işareti değil, **olumsal bir etiketleme alışkanlığıdır** (Faz-erken
her belgeye maturity kondu — bkz. G-04 künyeleme turu). RFC bu olumsal alışkanlığı normatif bir
çift-yönlü invariant'a terfi ettirince, kendi ilkeli turnusoluyla (Test A) **çatışıyor** (aşağıda D3).

## D2 doğrulaması — immutable-core yeni bir kaçamak açıyor mu? (Hayır)

SKR-034 W2/D2, `constitutive:true` sınıfının heterojenliğinin (kök hard-core vs revize-edilebilir
kuşak) dürüstçe kabul edilmesini istiyordu. §4.1 + Lakatos prior-art'ı bunu **karşılıyor.** Kritik
soru (görev 2. maddesi): *immutable-core, Madde III'ü sessizce Madde X'in dışına mı çıkarıyor, yoksa
RFC'nin kendi kaçamak-testini mi geçiyor?*

Test A/B/C'yi ENS-0000 / Madde III'e **bağımsız** uyguladım:
- **Test A (kaldırma):** Madde III'ü kaldır → P1-P8 tanımsız, tüm aşağı-akış akıl yürütme temelsiz →
  **anlamsızlaşır** → kurucu aday. ✓
- **Test B (yanılma-kipi):** Madde XV, P1-P8'i niyet düzeyinde değiştirilemez kılar → **fiat'la
  korunmuş → immutable-core (hard core).** ✓

Yani RFC'nin kendi turnusolu Madde III'ü tutarlı biçimde immutable-core'a yönlendiriyor. Ve muafiyet
**sessiz değil**: §4.1, §5.2, §6 üç ayrı yerde *"immutable-core için Madde X yalnızca program/
tutarlılık düzeyinde karşılanır, ampirik düzeyde bilinçli olarak açıktır"* diye **yüksek sesle
işaretliyor**. Bu, SKR-034'ün W2'de tam olarak istediği dürüst asimetridir. Lakatosçu bir hard core'u
metodolojik kararla korumak meşrudur ve RFC "Madde X her yerde keskinleşti" diye aşırı-genellemekten
kaçınıyor.

**Kötüye kullanım yüzeyi sınırlı mı?** Evet: (a) §7.4 `immutable_core_sections`'ı yalnızca ENS-0000'e
bağlar, genişletme yeniden-değerlendirme ister; (b) bir ampirik belge kendini "hard core" ilan
ederek kaçamayaz çünkü hard core tanım gereği **sınanabilir yeterlilik iddiası taşımaz** — Test C
onu yakalar; (c) invariant gereği `maturity` taşıyan belge zaten `true` olamaz. Tek küçük boşluk:
RFC, ENS-0000 dışında bir yapıtın immutable-core statüsünü **kimin/hangi yordamla** verebileceğini
(muhtemelen yalnızca Madde XV değişikliğiyle) açıkça yazmıyor; şu an tek-locus olduğu için bu
teorik. **D2 kapatıldı** (bu boşluk keskinleştirme, blocking değil).

## D3 doğrulaması — turnusol tutarlı mı? (Temiz vakalarda evet; governance'ta invariant'la çatışıyor)

Görevin istediği iki örneği (ENS-4000, ENS-3021) Test A/B/C'den **bağımsız** geçirdim:

- **ENS-4000 (Sözlük):** Test A: kaldır → tüm terimler tanımsız → **anlamsız** → kurucu aday. Test C:
  saha verisiyle çürütülebilir yeterlilik-öngörüsü taşıyor mu? Hayır, saf adlandırma-registry. →
  **`constitutive:true`.** RFC §8.1: `constitutive:true` + canon-borcu. **Eşleşiyor.** ✓
- **ENS-3021 (Decision Entropy):** Test A: kaldır → bir *iddia* (entropi yasası) kanıtsız kalır,
  aşağı-akış anlamsızlaşmaz → ampirik. Test C: ölçülebilir öngörü (noise ölçüsü) → **evet.** →
  **`constitutive:false`.** RFC §8.1: `constitutive:false`, M3 korunur. **Eşleşiyor.** ✓

Turnusol bu vakalarda RFC'nin sonucuyla **tutarlı** ve künyelerle hizalı. Bu iyi.

**Ama turnusol ile invariant governance'ta zıt sonuç veriyor** — D3'ün asıl kırığı burada D1 ile
buluşuyor. GOV-000'a Test A/C'yi uygulayın:
- Test A: GOV-000'ı kaldır → yönetişim rolleri/yetkileri tanımsız → **anlamsızlaşır** → kurucu.
- Test C: sınanabilir ampirik-yeterlilik öngörüsü? Hayır, prosedürel kural. →
- **Turnusol sonucu: `constitutive:true`** (RFC §8.2'nin Test-A gerekçesiyle birebir aynı).
- **İnvariant sonucu (§7.3): GOV-000 `maturity:M1` taşıyor ⇒ `constitutive:false`.**

**İki mekanizma aynı belge için zıt karar veriyor.** RFC bunu fark etmiyor çünkü governance'ı §8.1'in
"her örneğe turnusol uygulandı" tablosuna **koymadı** — §8.2'ye "leaning" olarak attı ve orada yalnızca
Test-A'yı (invariant'la çelişen tarafı) not etti. Bu tam olarak SKR-034'ün D3 gerekçesidir: en zor
sınavı §8.2'ye erteleyince çelişki gizlendi. **D3'ün ilkesel turnusolu sağlam; ama D1'in operasyonel
invariant'ı onunla çelişiyor ve governance bu çelişkinin kanıtı.**

## D4 doğrulaması — çift-owner kabul kapısı (kapatıldı)

§7.5 sağlam ve gereksiz karmaşıklık değil: `Accepted` = `ens-ceo` hiza (Madde XIV) **ve**
`ens-style-guardian` şema-imzası; ikisinden biri eksikse kabul olmaz. Tek-RFC senkron erdemini kabul
aşamasında koruyor. §10.2'de yinelenmiş. **D4 kapatıldı.**

## İç tutarlılık (Külliyat + Anayasa)

- **§7.3 ↔ §4.1 ↔ §8.2 ↔ §10.5 iç çelişkisi (yukarıda D6):** RFC kendi içinde tutarsız — invariant
  governance'ı `false` derken sınıflama tabloları `true` diyor, §10.5 ise olgusal olarak yanlış.
- **§4.2 turnusol ↔ §7.3 invariant çelişkisi:** ilkesel test (Test A) ile operasyonel kestirme
  (M-taşıma) governance'ta ayrışıyor. Kurucu bir belgeye sonradan `maturity` eklenirse invariant onu
  **sessizce** `false`'a çevirir — bu, kavramsal olarak kırılgan bir zemin (olumsal etiketi normatif
  invariant'a terfi).
- **Madde X ile:** protective-belt keskinleştirme + immutable-core dürüst-açık asimetrisi metin
  düzeyinde tutarlı (D2). Governance çelişkisi Madde X'i zayıflatmıyor, yalnızca sınıflamayı bozuyor.
- **Terminoloji sürüklenmesi (Madde VI / ENS-4000):** yeni drift yok; `constitutive` kullanımı
  KULLIYAT.md ile hizalı kalıyor.

## Sahibine talepler

1. **D6 (blocking) — governance çelişkisini çöz; invariant'ın evrensellik iddiasını düzelt.**
   GOV-000/010/`capability-matrix`/`canonical-process` `maturity:M1` taşıyor ama RFC onları
   `constitutive:true` (§4.1/§8.2/§10.5) ilan ediyor. Üç yoldan biri **açıkça** seçilmeli:
   (a) governance gerçekten `constitutive:true`'dur ve `maturity:M1` **olumsal bir miras etikettir** →
   o zaman §7.3 çift-yönlü invariant **yanlıştır**, tek-yönlü zayıflatılmalı ("`constitutive:false`
   ⇒ `maturity` taşır", ama tersi değil) ve governance'ın M1'inin nasıl ele alınacağı (carve-out mı,
   sökülecek mi) D1'in "grade sökmez" vaadiyle **tutarlı** yazılmalı; ya da (b) governance
   `constitutive:false`'tur → ama o zaman prosedürel kuralların ampirik öngörü/M5-Faz-4 ile canon
   kazanması gerekir ki bu §8.2 Test-A gerekçesiyle (Madde XIV/felsefe) çelişir; ya da (c) governance
   `type:standard`, Külliyat aralığı dışıdır ve `constitutive` alanı onlara **zorunlu değildir** →
   o zaman §4.1 protective-belt örneğinden, §8.2 satırından ve §10.5 listesinden governance **çıkarılmalı**
   (şu an oralarda `true` iddiası taşıyor). Hangisi seçilirse seçilsin, **§10.5'in "GOV-* M-ekseninde
   değildir" cümlesi olgusal yanlış olduğu için düzeltilmeli.** Sessiz geçilemez — bu, SKR-034 W1'in
   tam tekrarı.

2. **D6-yan (blocking ile birlikte) — invariant'ı korpus geneli denetle, tek örnek grubuyla değil.**
   RFC §7.3 "deponun bugünkü fiili künyeleriyle çelişmez" diyor; bu iddia yalnızca 4000-aralığı +
   ENS-0000/4000 için doğrulanmış. `grep ^maturity:` korpus taraması: `maturity` taşıyanlar =
   ENS-2001..2004, ENS-3021..3023 (M3), ENS-4001/4010/4020/4025/4030 (M2), ENS-4031 (M0),
   GOV-000/010/capability-matrix/canonical-process (M1), ADR-0001/0002 (M0). Bunların **tümü**
   invariant'a göre `constitutive:false` çıkmalı; governance ve (Külliyat-dışı ama sınıflanmışsa)
   ADR'lar bu testten geçirilip RFC metnindeki sınıflamayla hizalanmalı.

3. **D7 (keskinleştirme) — turnusol ↔ invariant önceliğini netleştir.** Test A ile §7.3 invariant'ı
   çeliştiğinde hangisi kazanır? İlkesel olarak Test A/B/C birincil olmalı (invariant yalnızca hızlı
   bir kestirme); §8.2'nin governance'ı invariant yerine turnusol sonucuyla (`true`) etiketlemesi bu
   önceliği örtük varsayıyor ama RFC bunu açıkça yazmıyor. Yazılırsa D6(a) yolu doğal çözülür.

4. **D8 (keskinleştirme, D2 artığı) — immutable-core verme yordamı.** ENS-0000 dışında bir yapıtın
   hard-core statüsünü hangi yordamın (muhtemelen yalnızca Madde XV) verebileceğini §7.4'te bir cümleyle
   bağla; şu an tek-locus olduğu için teorik ama gelecekteki kötüye kullanımı kapatır.

## Kapanış

RFC-6001 v0.2, SKR-034'ün beş talebinden dördünü (D2/D3-çekirdek/D4/D5) gerçekten ve iyi kapattı;
prior-art uydurma değil, immutable-core kaçamak değil, çift-owner kapı sağlam, çekirdek tez yine sağ
çıkıyor. Yara, D1'i kapatmak için seçilen aracın kendisinde: `maturity ⟺ constitutive:false`
çift-yönlü invariant'ı, RFC'nin `constitutive:true` ilan ettiği governance ailesinin fiili
`maturity:M1` künyeleriyle çelişiyor ve §10.5 bunu olgusal-yanlış bir cümleye dönüştürüyor. Bu, W1'in
bir aralık öteye taşınmış tekrarıdır: örnekler düzeltildi, kural evrensel ilan edildi, korpus
denetlenmedi. D6 karşılanınca — governance kararı verilip §10.5 düzeltilince ve invariant'ın
evrensellik iddiası korpus taramasıyla desteklenince — `survives` erişilebilir. Faz durmaz; öneri
düzeltilerek ilerler (Madde X: eksik tamamlanır, reddedilmez).

**Süreç notu (görev 5. maddesi):** Verdict `wounded` olduğu için sıradaki adım **`ens-ceo` DEĞİLDİR.**
Gerçek sıradaki adım: `ens-philosopher` D6'ya yanıt verir (RFC v0.3) → bağımsız **3.** `ens-skeptic`
turu. Yalnızca o tur `survives` verirse Madde XIV `ens-ceo` hiza incelemesi devreye girer — ve
`survives` bile Anayasa'yı (Madde IV) **otomatik değiştirmez**; ENS-0000 ancak `ens-ceo` + çift-owner
kabulünden (§7.5) sonra fiilen düzenlenir. Bu RFC hâlâ bir **öneridir**; skeptic-kapısı henüz açık.
