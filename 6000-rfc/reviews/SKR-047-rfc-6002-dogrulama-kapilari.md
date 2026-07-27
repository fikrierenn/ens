---
id: SKR-047
type: skeptic-review
origin: RFC-6002
depends_on: [RFC-6002]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-27
validation_dimension: constitutional
verdict: wounded
---

# SKR-047 — RFC-6002 (Doğrulama Kapılarının Kapsamı) Saldırısı

> **Bağımsızlık beyanı (GOV-000 G4, `governance/000-governance-principles.md:36`):**
> RFC-6002'yi oturum sahibi yazdı; bu kayıt ayrı `ens-skeptic` context'inde üretildi.
> Paralel yürüyen `ens-architect` turu (ARCH-0001) **okunmadı** — bağımsızlık bunu gerektirir.
> `dotnet test` gerekmedi (yalnız-doküman). Hiçbir çıktı uydurulmadı; doğrulanamayan her şey
> **DOĞRULANMADI** olarak işaretlendi.

## Verdict

**`wounded`** — Merkezî yorum (`ratified` ≠ `Canonical`; G4'ün öznesi `canon`'dur, `status`
değil) **doğrudur ve sağ çıkar** — ama RFC'nin *kendi kullandığı* delillerle değil, kullanmadığı
daha güçlü delillerle. Buna karşılık: temel taşı olan alıntı **bağımlılığı tarafından
yürürlükten kaldırılmış**, dört failure condition'ının ikisi **lafzen tetiklenmiş**, §2.1'in
"iki bağımsız türetme" iddiası **savunulamaz**, §4'ün taşıyıcı vaadi (*"sessizce muafiyete
dönüşmez"*) **olgusal olarak yanlış** ve §10 tam da SCAN-01'in öldürdüğü G2 hatasını
**yeniden üretiyor**.

Kısaca: **doğru tez, yanlış ispat.** Tezi kurtarmak ucuzdur; ispatı bu hâliyle savunmak
mümkün değildir.

| Eksen | Sonuç |
|---|---|
| §2 çekirdek tez (`ratified` ≠ `Canonical`) | **survives** — ama RFC'nin atlamış olduğu delillerle |
| §2 `Canonical = M5` denklemi | **refuted** — RFC-6001 §7.2 ikinci bir canon yolu açtı |
| §2.1 "iki bağımsız türetme" | **refuted** — ortak-kaynak + ardışık bağımlılık |
| §2.2 "9 ihlal üretir" argümanı | **wounded** — meşru yorum ilkesi, ama §8.2 kendi kendini çürütüyor |
| §3 boşluk teşhisi (onay makamı yok) | **survives** — ve RFC'nin dediğinden daha güçlü |
| §3 "`review → ratified`" adlandırması | **wounded** — gerçek geçiş `skeptic-challenged → ratified` |
| §3.1 R1-R3 kapısı | **wounded** — R2'nin "aktör"ü tanımsız; fc-3 açık bırakılmış |
| §4 "tüm aktif boyutlar" | **refuted** — tek-aktör kaçağı; "görünür borç" vaadi yanlış |
| §5 ikiye bölme gerekçesi | **wounded** — RFC-6002 tek başına kabul edilirse durum bugünden kötü |
| §10 öz-uygulama | **refuted** — SCAN-01'in kapattığı G2 hatasının tekrarı |
| Künye alıntı hijyeni | **survives** — SCAN-03'ün üç satır kaymasını sessizce düzeltmiş |

## Alıntı doğrulaması (kural 3.5)

RFC-6002, `work-protocol.md` §3.5'in doğduğu oturumda yazıldı. O kural üç şey arar: yol
gerçek mi, satır o şeyi mi diyor, bulgu iddia ettiğini mi kanıtlıyor. RFC'nin **her** alıntısı
tek tek açıldı.

| # | RFC'nin atfı | Gerçek | Sonuç |
|---|---|---|---|
| 1 | `maturity-model.md:28` — M2 = ≥1 SKR → `ratified` | satır 28: *"Scientific skeptic saldırısından **survives** (≥1 SKR); iç tutarlı \| `skeptic-challenged`→`ratified`"* | ✅ içerik doğru, **ama** ara statü düşürülmüş (bkz. D-4) |
| 2 | `000-governance-principles.md:36` — G4 öznesi **Canonical** | satır 36: *"**G4 — Her Canonical yapıtın ≥2 bağımsız validator'ı vardır** (farklı boyutlardan)."* | ✅ **birebir**; özne gerçekten `Canonical` |
| 3 | `canonical-process.md:45` — G4 zinciri | satır 45: *"**G4:** ≥2 bağımsız boyut validator'ı (**ör.** Scientific + Ontology, uygunsa + Engineering)."* | ⚠️ *"ör."* düşürülmüş (bkz. D-8) |
| 4 | `maturity-model.md:34` — *"`canon: true` yalnızca M5'tir"* | satır 34: *"**`canon: true` yalnızca M5'tir.** Skeptic-survives (M2/M3) Canon yapmaz."* | ✅ **birebir var** — ama **bayat** (D-1) |
| 5 | `maturity-model.md:31` — M5 = Sci+Eng+Biz+Ethical, Ontology yok | satır 31 doğrulandı; Ontology gerçekten yok | ✅ |
| 6 | `GOV-000:22` — *"Roller, yetkiler ve süreçler bu ilkelerden **türer**"* | satır 22: birebir aynı cümle | ✅ |
| 7 | `roles.md` — *"Governance body: fazı gelince"* | `governance/roles.md:63`: *"Engineering/Business/Ethical Validator, **Governance body: fazı gelince** (ROSTER)."* | ✅ (RFC satır no vermemiş; içerik doğru) |
| 8 | SCAN-01 emsali: `roles.md` G2 türevi yanlıştı, düzeltildi | `roles.md:44-53` düzeltme kutusu birebir doğrulandı | ✅ — **ve RFC'yi §10'da vuruyor (D-6)** |
| 9 | SCAN-03: 9 `ratified`, 6'sı sayıca G4 | `SCAN-03:551`: *"**6/9** \| **%67**"*; `SCAN-03:207`: *"`ratified` Külliyat yapıtı: **9**"* | ✅ |
| 10 | `RFC-6001:175` — *"`constitutive` … muaf tutmaz"* | satır 175 birebir | ✅ |
| 11 | §3.1 gerekçesi: 9 `ratified` yapıtta onaylayan kayıtlı değil | `SCAN-03:249`, `:269` (**0/9**, %0), `:576` (B-01) | ✅ |
| 12 | §4: *"maturity-model'in dört-skeptic'i, validation-framework'ün 5 boyutundan **önce** yazılmış"* | iki dosya da `version: 0.1.0`, `last_reviewed: 2026-07-24`; depoda sıralamayı gösteren kayıt yok | ❌ **DOĞRULANMADI** — kronoloji iddiası desteksiz (D-9) |

**Ara sonuç:** RFC-6002 alıntı sadakatinde SCAN-03'ün **üstündedir** ve bu takdir edilmelidir
(bkz. "Katıldığım noktalar" K-1). Tek uydurulmuş atıf yok. Sorun alıntıların *doğruluğunda*
değil, **güncelliğinde** (D-1) ve **eksikliğindedir** (D-2).

## Yenilik incelemesi — tez sağ, temel taşı çürük

### D-1 — Temel taşı **bayat**: `maturity-model.md:34`'ü RFC-6001 zaten değiştirdi

RFC-6002 `:158-159` kendi failure condition 1'ini şöyle koyuyor:

> *"**`maturity-model.md:34` bulunamazsa ya da başka bir şey söylüyorsa.** Tüm §2 o tek
> cümleye dayanır. O cümle kaldırılırsa RFC çöker."*

Cümle **duruyor** ve birebir o. Ama RFC'nin sormadığı soru şu: *hâlâ yürürlükte mi?*

`RFC-6001:339-345` (§7.2 — *"§Değer kümeleri — `canon` satırının yeni hâli"*):

> - **ESKİ:** `canon`: `true | false` — **yalnızca `maturity: M5` ise true**
> - **YENİ:** `canon`: `true | false` — kazanılır, ilan edilmez.
>   `constitutive: false` (ampirik) yapıtta **yalnızca `maturity: M5` ise true**;
>   `constitutive: true` (kurucu) yapıtta **ratifiye edilip skeptic tutarlılık incelemesinden
>   sağ çıkınca** true — **maturity/evidence eksenine tabi değil.**

Ve `RFC-6001:382`: *"**kurucu yapıt M5/Faz-4 kanıt zincirinden muaftır**."*
Ve `RFC-6001:385-386`: *"`maturity-model.md` ve `KULLIYAT.md` bu ayrıştırmayla **hizalanır**
(§10.5)."* — yani RFC-6001 satır 34'ün **güncellenmesi gerektiğini kendisi söylemiş**; iş
yapılmamış, satır eski hâliyle durmaya devam etmiş.

Sonuç: `maturity-model.md:34` bugün **açık bir hizalama borcudur**, bir dayanak değil.
RFC-6002 onu `depends_on: [… RFC-6001]` diye künyesine yazdığı belgenin yürürlükten
kaldırdığını fark etmeden **tüm tezinin temeline** koydu.

**Bu, tezi çürütmez** — çünkü `ratified ≠ Canonical` iddiası her iki canon yolunda da doğrudur.
**Çürüttüğü şey `:48`'deki denklemdir:**

> RFC-6002 `:48`: *"≥1 SKR → `ratified` (M2). **≥2 bağımsız boyut → `canon: true` (M5).**"*

Bu ikinci yarı, `constitutive: true` sınıfı için **yanlıştır**: o sınıf canon'unu M5'ten değil
RFC-6001 §7.2 yolundan kazanır. `Canonical` ile `M5`'i özdeşleştirmek, RFC-6002'nin *kendi
teşhis ettiği* terim karışıklığının bir başka örneğidir — bu kez RFC'nin kendisinde.

### D-2 — RFC, elindeki **en güçlü üç delili** kullanmamış

Tez doğru; ispatı gereksiz yere zayıf. §2.1 (bağımsız türetme) ve §2.2 (ihlal sayısı)
yerine üç **doğrudan metinsel** delil vardı ve hiçbiri anılmıyor:

1. **`GOV-000:46-48` — G4'ün yazarları kuralı zaten M5'e uygulamış:**
   > *"**G2 + G4 gereği**, mevcut Külliyat'ta yazar (ens-philosopher) hiçbir şeyi tek başına
   > Canonical yapamaz; ve Engineering Validation Faz 4'ü gerektirdiğinden **M5 şu an
   > ulaşılamaz — Canon boş.** Bu, ilkelerin doğrudan ve dürüst sonucudur."*

   Bu, yorum değil **kaynağın kendi uygulamasıdır**: G4'ten çıkarılan sonuç `ratified`
   yapıtların ihlali değil, *Canon'un boş olmasıdır*. Ç-01'i tek başına bitirir.

2. **`canonical-process.md:47-48`** aynı sonucu bağımsızca tekrarlar: *"M5 şu an ulaşılamaz;
   Canon boştur — bu doğru durumdur."* G4'ü (`:45`) yazan belge, `ratified` yapıtları ihlalde
   saymıyor.

3. **`ENS-4000:40`** (`canon: true` bir yapıt): *"`M3` = **ratified** teori/yasa (Faz 1-2'den
   geçti, ama **henüz Canon değil** — M5 Faz 4 ister)."* Sözlük ayrımı zaten yapıyor.

Üç delil de RFC'nin ulaştığı sonucu **doğrudan** verir ve hiçbiri "şu yorum rahatsız edici
sonuç üretiyor" biçiminde dolaylı değildir. §2.1 ve §2.2 silinip yerlerine bunlar konsa,
RFC bu incelemenin iki `refuted`'ından kurtulurdu.

## Yanlışlanabilirlik — dört koşulun ikisi lafzen tetiklendi

RFC-6002 `:154-170` dört failure condition sayıyor. Madde X'in istediği şey budur ve RFC bunu
ciddiyetle yapmış. Ama koşullar **sınandığında** tablo şu:

| # | Koşul | Durum |
|---|---|---|
| fc-1 | `maturity-model.md:34` bulunamaz/başka şey der | **tetiklenmedi** (satır aynen var) — ama koşul yanlış soruyu soruyor (D-1: satır var, *yürürlüğü* yok) |
| fc-2 | `canon: true` olup M5 olmayan bir yapıt **meşru** sayılıyorsa | **TETİKLENDİ** (D-3) |
| fc-3 | R2'yi uygulayacak aktör hiç atanmazsa | **açık** — RFC kendisi §9.1'de ertelemiş (D-7) |
| fc-4 | "tüm aktif boyutlar" sıfır-validator'la M5 geçirilebilir kılarsa | **TETİKLENDİ ve tarif edilenden daha kötü** (D-5) |

### D-3 — fc-2 tetiklendi: `canon: true` + non-M5 yapıt **meşrudur**, ve bunu RFC-6001 kurdu

RFC `:160-164` fc-2'yi *"kısmen gerçekleşmiş"* sayıp iki örnek veriyor: ENS-4000 ve
(2026-07-27'ye kadar) ENS-3000. İkisi de **borç** olarak sunuluyor (ROADMAP G-24, G-25).

Depoda `canon: true` taşıyan Külliyat yapıtları (grep, `^canon: true`):
`0000-constitution/ENS-0000-constitution.md`, `1000-philosophy/ENS-1000-manifesto.md`,
`4000-ontology/ENS-4000-glossary.md` — **üç** yapıt. RFC ikisini saymış, **ENS-0000'i hiç
anmamış.**

Ve ENS-0000 bir borç **değildir**. `RFC-6001:430` turnusolu uygulayıp sonucu yazmış:

> `| **ENS-0000** | canon:true, maturity yok | … | `true` — immutable-core (Md III) +
> protective-belt | **Grade sökülür mü? Hayır (M taşımıyor)** |`

Ve `RFC-6001:169`: *"`constitutive: true, canon: true` — **ratifiye kurucu belge** (ör.
ENS-0000, ENS-4000)."*

Yani `canon: true` **olup M5 olmayan meşru bir yapıt** bugün vardır, tasarımla vardır ve
meşruiyetini kabul edilmiş bir RFC'den alır. RFC-6002'nin kendi ifadesiyle: *"bu durumda
`ratified`/`Canonical` ayrımı … tanımsal değil, temenni olur."*

**Doğru okuma:** ayrım tanımsaldır, ama RFC-6002'nin kurduğu eksende değil. Ayrımın gerçek
ekseni `M5` değil **`canon` alanının kendisidir**; `canon`'a giden **iki** yol vardır
(ampirik: M5; kurucu: RFC-6001 §7.2). fc-2 bu yüzden **kötü formüle edilmiştir**: RFC'yi
çürütmeyen bir olguyu çürütücü ilan ediyor. Madde X açısından bu, uydurulmuş kesinlikten
farklı ama akraba bir kusurdur — **yanlış hedefe nişan almış falsifier**, koşul tetiklendiğinde
tezi değil yazarı vurur.

### D-4 — Yönetilecek geçişin **adı yanlış**: `review → ratified` diye bir kapı yok

RFC §3 başlığı, §5 ve §6 tablosu hep *"`review → ratified` geçişi"* diyor. `maturity-model.md`
`:27-28`:

- `M1 Proposed` → `status: review`
- `M2 Reviewed` → `status:` **`skeptic-challenged`→`ratified`**

Yani modelin tanımladığı geçiş `skeptic-challenged → ratified`'dır; `review` M1'in etiketidir
ve `review`'dan doğrudan `ratified`'a giden bir yol modelde yoktur. RFC'nin `canonical-process.md`'ye
yazılmasını önerdiği kural (`:136`) bu hâliyle **var olmayan bir geçişi** düzenler ve
**gerçek geçişi düzenlemeden bırakır**. Bu, normatif metin önerisinde kabul edilemez bir
imprecision'dır — üstelik RFC'nin kendi tezi *tam olarak* statü adlarının karıştırılmasıdır.

## Varsayım haritası

| # | Varsayım | Nerede | Kırılma koşulu | Durum |
|---|---|---|---|---|
| V1 | `maturity-model.md:34` yürürlüktedir | §2, fc-1 | RFC-6001 §7.2 onu kurucu sınıf için değiştirdi | **kırıldı** (D-1) |
| V2 | İki türetme birbirinden bağımsızdı | §2.1 | ortak kaynak / ardışıklık gösterilirse | **kırıldı** (D-10) |
| V3 | Bir yorumun çok ihlal üretmesi ona karşı delildir | §2.2 | korpus kendi kurallarını fiilen ihlal ediyorsa | **kırıldı** (D-11) |
| V4 | "Yazardan farklı aktör" ENS'te uygulanabilir | §3.1 R2 | "aktör" tanımsızsa / tek-operatör | **açık** (D-7) |
| V5 | "Aktif boyut" listesi borcu görünür kılar | §4 | aktiflik ROSTER'a bağlıysa, atanmamış boyut *gereksiz* olur | **kırıldı** (D-5) |
| V6 | `Canonical` ≡ `M5` | §2 `:48` | ikinci bir canon yolu varsa | **kırıldı** (D-1/D-3) |

### D-10 — §2.1 "iki bağımsız türetme": bağımsızlık değil, **ortak kaynak + ardışıklık**

RFC `:51-59` bunu **Madde X ve G4'ü karşılayan bir kanıt** olarak sunuyor: *"İki ayrı context,
**birbirini görmeden** aynı sonuca vardı."* İki türetme:

1. `ens-philosopher`, SCAN-03'ün Ç-01'ini çözerken.
2. Oturum sahibi, alıntıları `dosya:satır` doğrularken.

Üç ayrı nedenle bu bağımsızlık değildir:

**(a) Ortak kaynak.** İkisi de aynı girdiyi okudu: `SCAN-03:587` Ç-01 satırı, çatışmayı zaten
*"`maturity-model.md:28` ↔ `canonical-process.md:44`"* diye çerçeveleyerek sunuyor. İki
okuyucunun aynı çerçeveden aynı sonuca varması, bağımsız doğrulama değil **aynı girdinin iki
kez işlenmesidir**. G4'ün "bağımsız validator" şartının koruduğu şey tam olarak budur.

**(b) Ardışıklık.** İkinci türetme birinciyi *görmeden* yapılmadı: birinci türetmenin sonucu
`ROADMAP.md:240`'a (G-26) **yazılmıştı** ve orada RFC-6002 §2/§3/§4'ün neredeyse birebir
metnini içeriyor — *"Ç-01 çatışma değil terim karışıklığıdır — G4 öznesi 'Canonical (M5)',
`ratified` ise M2-M5'in ortak `status` etiketi; ≥1 SKR `ratified`'ı, ≥2 boyut `canon:true`'yu
yönetir."* RFC-6002 §2, bu cümlenin genişletilmiş hâlidir. İkinci "türetme" bir türetme değil,
**birincinin alıntılarının denetimidir**.

**(c) Kategori hatası.** Alıntı doğrulama ile yorum türetme farklı edimlerdir. Oturum sahibinin
bulduğu şey RFC'nin kendi anlatımıyla *"iki dosyanın o yolda bulunmadığı ve satır numaralarının
kaymış olduğu"* (`:57-59`) — yani **girdi hijyeni**, sonuç değil. Girdiyi temizlemek, sonucu
bağımsız üretmek değildir.

> **Steelman.** Savunulabilir çekirdek şudur: aynı sonuca iki farklı *yöntemle* (yorumsal
> çıkarım + mekanik alıntı denetimi) varılmıştır ve ikincisi birincinin girdi hatalarını
> düzeltmiştir. Bu **gerçek bir güçlendirmedir** — ama adı "bağımsız türetme" değil,
> **"kaynak doğrulaması"**dır ve G4'ün yerine geçmez. §2.1 bu adla yeniden yazılırsa dürüst
> olur; bugünkü hâliyle RFC, kendisine olmayan bir doğrulama kredisi veriyor.

### D-11 — §2.2 sonuçtan geriye akıl yürütüyor ve §8.2 tarafından çürütülüyor

§2.2 (`:61-66`): karşıt okuma kabul edilirse Külliyat'ın kendi beyanı *"dokuz ihlal beyanına"*
döner; *"Bir yorum, yorumladığı metni toplu ihlale çeviriyorsa **önce yorumdan şüphelenilir**."*

**Steelman — bu geçerli bir yorum ilkesidir.** Hukuk yorumunda karşılığı vardır (*ut res magis
valeat quam pereat*): bir metni işlemez ya da toptan ihlal edilmiş kılan okuma, onu işler
kılan okumaya yeğlenmez. İlke kendi başına kusur değildir.

**Ama burada çalışmıyor, çünkü öncülü olgusal olarak yanlış.** İlkenin gücü, "bu korpus
kurallarına uyar" varsayımından gelir. RFC **aynı belgede** bu varsayımı çürütüyor:
fc-2 (`:160-164`) *"Bugün böyle **iki** yapıt var: ENS-4000 ve … ENS-3000"* diyor — yani korpus
kendi canon kuralını fiilen ihlal etmiş durumda. `ROADMAP:238` ENS-3000 için *"sıfır doğrulama
turlu canon ilanı"*, `:239` ENS-4000 için *"kazanılmamış canon"* diyor.

Kendi kurallarını iki yerde ihlal ettiği belgelenmiş bir korpusta, *"bu okuma dokuz ihlal
üretir"* neredeyse hiçbir delil taşımaz — çünkü ihlal, bu korpus için **beklenen** bir
gözlemdir. §2.2 kendi belgesinin §8.2'si tarafından etkisizleştiriliyor.

**Ek olarak yanlışlanabilirlik sorunu:** §2.2 biçim olarak *"sonucu sevmediğim yorum
yanlıştır"*a çok yakın durur. Onu bilimsel kılan tek şey, bağımsız metinsel delille
desteklenmesidir — ki D-2'de gösterildiği gibi o delil **vardı** ve kullanılmadı. §2.2'yi
silmek RFC'yi zayıflatmaz, **güçlendirir**.

## En güçlü karşı-argüman

### D-5 — §4 "tüm aktif boyutlar", G4'ü **tek aktörle** geçilebilir kılar ve muafiyeti *görünür*
### değil **görünmez** yapar

Bu, RFC'nin en ciddi kusurudur ve fc-4'ün tarif ettiğinden **farklı ve daha kötü** bir kaçaktır.

**Önerinin metni** (`:111-114`):

> *"sabit sayı yerine **'tüm aktif boyutlar'** yazılsın. Bir boyut 'aktif'tir ancak ve ancak
> ROSTER'da o boyutun validator rolü **atanmışsa**. Böylece … bir boyutun atanmamış olması
> sessizce muafiyete dönüşmez — **görünür bir borç olur**."*

**Kaçak 1 — vaat mekanizmasızdır ve tersine çalışır.** Kural *"gerekli boyutlar = atanmış
boyutlar"* diyor. Bir boyut atanmamışsa **gerekli değildir**. Gereksiz bir şey borç
üretmez — tanım gereği. Yani kural, atanmamış bir boyutu borç yapmaz; **muafiyet yapar.**
Vaat edilen görünürlük için ayrı bir *"olması gereken boyutlar"* kütüğü gerekir; RFC böyle bir
kütük **kurmuyor**. Bugünkü sabit dörtlü liste (`maturity-model.md:31`) en azından Ethical'ı
**adıyla** zorunlu sayıyor ve G-27'yi (`ROADMAP:241`) görünür tutuyor. Öneri kabul edilirse
Ethical, ROSTER'da rolü olmadığı için **listeden düşer** ve G-27'nin normatif dayanağı buharlaşır.
**Öneri, kapatmayı vaat ettiği deliği kendisi açıyor.**

**Kaçak 2 — bugünkü ROSTER'la G4 tek aktörle sağlanır.** `governance/roles.md:61`:

> *"**Scientific + Ontology Validator: `ens-skeptic`.**"*

Yani bugün "aktif" boyut kümesi = {Scientific, Ontology} ve **her ikisi de aynı aktöre aittir**.
Önerilen kural altında bir yapıt, `ens-skeptic`'in iki SKR'siyle *"tüm aktif boyutlar"* şartını
karşılar. G4'ün lafzı **"≥2 bağımsız validator (farklı boyutlardan)"**dır
(`GOV-000:36`) — **bağımsızlık aktör niteliğidir, boyut etiketi değil.** §4 önerisi boyut
şartını korurken bağımsızlık şartını sessizce düşürüyor.

Bu soyut bir risk değil, **korpusta zaten gerçekleşmiş** bir desendir: `4000-ontology/reviews/`
altındaki SKR-017/018/019/020/021/022/023/028/030/031/032/038/039'un tamamı
`validation_dimension: ontology` **ve** `owner: ens-skeptic` taşıyor; `5000-architecture/reviews/`
altındaki SKR-024/025/026/027/029/037 `validation_dimension: engineering` **ve**
`owner: ens-skeptic`. Yani "farklı boyut" etiketi ENS'te **zaten** tek aktör tarafından
üretiliyor. §4, bu fiilî durumu düzeltmek yerine **hukukileştirir**.

**Kaçak 3 — M4 kapısı artık kapalı değil, yani kaçak canlıdır.** RFC dolaylı olarak M5'in
ulaşılamazlığına güveniyor olabilir (`GOV-000:47`, `canonical-process.md:47-48`: *"M5 şu an
ulaşılamaz"*). O gerekçe **Faz 4 başlamadan önce** doğruydu. `ROADMAP:245`: *"**✅ Faz 4
BAŞLADI** — ilk çalışan kod (7000-reference-implementation)"*. M4'ün faz koşulu artık
sağlanıyor. Dolayısıyla D-5'teki tek-aktör kaçağı teorik değil **operasyoneldir**.

**Sonuç:** §4 kabul edilirse, *"G4 ≥2 bağımsız validator ister"* kuralı, tek bir `ens-skeptic`
context'inin iki farklı etiketli kaydıyla karşılanabilir hâle gelir. Bu, RFC-6002'nin bütün
amacına — kapıyı doğru yere koymaya — aykırıdır ve **§4'ü tek başına `refuted` yapar.**

> **Kapatan minimal düzeltme (T-D'de talep ediliyor):** "aktif boyut" tanımına ikinci koşul:
> *"Bir yapıtın G4'ü ancak **farklı aktörlere** ait, farklı boyutlu ≥2 kayıtla sağlanır; aynı
> aktörün ürettiği iki boyut kaydı **tek** validator sayılır."* Ve borcun görünürlüğü için:
> *"Aktif olmayan her boyut, ROADMAP'te açık borç satırı olarak kayıtlıdır; muafiyet değil
> **ertelemedir**"* — G-27'nin dayanağı böyle korunur.

## İç tutarlılık

### D-6 — §10, SCAN-01'in **öldürdüğü** G2 hatasını yeniden üretiyor

RFC `:185` (son satır, öz-uygulama hükmü):

> *"**Yazarı kendi turunu `survives` işaretleyemez** (GOV-000 G2 + G4)."*

`governance/roles.md:44-49`, 2026-07-27 tarihli SCAN-01 düzeltme kutusu:

> *"Bu satır önceki hâlinde **'Validator ≠ Author (G2): kendi işini doğrulayan olamaz'**
> diyordu. **Yanlıştı.** GOV-000'ün gerçek metni *'No author **canonizes** their own work'* —
> yani **kanonlaştırma** yasağıdır, **doğrulama** yasağı değil. **Bir yazarın kendi işini
> doğrulaması G2 tarafından yasaklanmaz**; yasaklanan, onu tek başına Canonical ilan etmesidir.
> Bağımsız doğrulama zorunluluğunun gerçek kaynağı **G4**'tür."*

Ve aynı kutu `:51-53`: *"Bu tek satır, korpus genelinde ≈10 dosyaya yayılan *'G2: yazan
doğrulayamaz'* ailesinin **kök nedeniydi**."*

RFC-6002 `:75-77` bu düzeltmeyi **kendi yordamsal argümanının emsali olarak alıntılıyor** —
ve sekiz satır sonra, `:185`'te, ailenin yeni bir üyesini üretiyor. "Kendi turunu `survives`
işaretlemek" bir **doğrulama** edimidir; G2 onu yasaklamaz. Doğru dayanak yalnızca **G4**
(bağımsız validator) ve **G3** (`GOV-000:35` — doğrulayan onaylamaz) ile
`canonical-process.md:46`'dır (*"G2: Author zincirin hiçbir kapısını kendi açamaz"* — ki bu da
GOV-000'den *türeyen* ve onu genişleten bir ifadedir, yani RFC'nin kendi §3 ilkesine göre
şüpheli).

Bu, en sert bulgu tipidir: **RFC, teşhis ettiği hatayı, teşhisin hemen yanında tekrarlıyor.**

### D-7 — R2'nin "aktör"ü tanımsız; fc-3 RFC tarafından **açıkça ertelenmiş**

R2 (`:93`): *"Yazardan **farklı** bir aktörün, **kayda geçen** ratifikasyon edimi."*
fc-3 (`:165-167`): *"R2'yi uygulayacak aktör hiç atanmazsa … kâğıt üzerinde kapatılmış bir
boşluk gerçekte açık kalır — ve bu, boşluğu **kayıtlı** hâlinden **daha kötüdür**."*
§9.1 (`:174-176`): *"Bu RFC bunu **önerir, karara bağlamaz** — rol ataması GOV-010 alanıdır."*

Yani RFC kendi failure condition'ını okuyor, "daha kötü" olacağını yazıyor ve sonra o koşulu
**gerçekleştiren** kararı veriyor. `ROADMAP.md:235` de tabanı zayıflatıyor:
*"G-16 | Governance tek-operatör (rol ayrımı G2/G3 **fiilen zayıf**) | P3"*.

**Steelman — R2 aslında bugün uygulanabilir.** Korpus "farklı aktör"ü **rol** düzeyinde
yorumluyor ve bu desen çalıştı: RFC-6001'in kabul zinciri `ens-ceo` hiza-onayı +
`ens-style-guardian` şema-imzası taşır (`6000-rfc/reviews/STYLE-SIGNOFF-RFC-6001.md` gerçekten
mevcut). `SCAN-03:603` (Ö-03) bunu zaten öneriyor: *"mevcut çift-owner kapısını … Külliyat
ratifikasyonuna da genişlet — desen zaten kanıtlanmış."*

Yani RFC'nin ihtiyacı olan tek şey **bir cümlelik varsayılan**: *"R2'nin aktörü, GOV-010 aksini
söyleyene kadar `ens-style-guardian`'dır (Custodian); hiza gerektiren yapıtlarda ek olarak
`ens-ceo`."* Bunu yazmamak, uygulanamaz bir kural bırakır — ve RFC'nin kendi ölçütüyle bu,
**mevcut hâlden kötüdür**. Erteleme burada nötr değil, **zararlıdır**.

### D-8 — `canonical-process.md:45` alıntısından *"ör."* düşürülmüş

RFC `:29` tablosu: *"**G4:** ≥2 bağımsız boyut validator'ı (Scientific + Ontology, uygunsa +
Engineering)"*. Gerçek satır 45: *"(**ör.** Scientific + Ontology, uygunsa + Engineering)"*.

Fark önemsiz değil: *"ör."* listeyi **örnek** yapar, **kapalı liste** değil. RFC §4, iki listeyi
"iki rakip liste" olarak çerçeveleyip birleşim öneriyor (`:101-109`) — oysa `canonical-process.md`
hiçbir zaman kapalı bir liste iddia etmemiş. Ç-03'ün "çatışma" olarak sunulmasının bir kısmı
bu düşürülmüş iki harften geliyor. Sonuç değişmez (birleşim yine makul), ama **çatışmanın
şiddeti abartılmış** olur ve RFC'nin *"çatışma yok, terim karışıklığı var"* şeklindeki kendi
ana motifi burada da uygulanabilirdi.

### D-9 — §4'ün kronoloji iddiası **DOĞRULANMADI**

RFC `:106-107`: *"`maturity-model.md`'nin 'dört-skeptic'i, `validation-framework.md`'nin **beş**
boyutu **yazılmadan önceki** bir **sayı-sürüklenmesidir**."*

İki dosyanın künyeleri: `maturity-model.md:11-12` → `version: 0.1.0`, `last_reviewed: 2026-07-24`;
`validation-framework.md:11-12` → `version: 0.1.0`, `last_reviewed: 2026-07-24`. **Aynı sürüm,
aynı tarih.** Depoda hangisinin önce yazıldığını gösteren bir kayıt bulamadım (bu context'te
`git log` çalıştıracak araç yok — **sonuç uydurulmadı**).

İddia doğru olabilir; ama bugün **dayanaksızdır** ve bir RFC'nin normatif gerekçesi olamaz.
İyi haber: gerekçeye ihtiyaç yok — Ontology'nin listede olması gerektiği, RFC'nin kendi verdiği
olgusal argümanla (`:107-109`: ontoloji boyutunda fiilen üretilmiş SKR'ler) zaten kanıtlanıyor.
Kronoloji cümlesi **silinmeli**, argüman ayakta kalır.

### D-12 — İkiye bölme: RFC-6002 **tek başına kabul edilirse durum bugünden kötüdür**

§5 (`:116-128`) bölmeyi CEO-0002'nin kapsam-orantısı uyarısına dayandırıyor. Uyarı meşru,
gerekçe makul. Ama bölmenin **sınanması gereken** özelliği şudur: *her parça tek başına kabul
edilebilir bir dünya bırakıyor mu?*

RFC-6002 kabul + RFC-6003 ret senaryosu:

| Sonuç | Bugün | RFC-6002 tek başına |
|---|---|---|
| M5 boyut listesi | sabit dörtlü (Sci+Eng+Biz+Ethical) — Ethical **adıyla zorunlu** | "tüm aktif boyutlar" = {Scientific, Ontology} |
| Ethical | karşılanmamış **zorunluluk** → G-27 borcu (`ROADMAP:241`) | **gereksiz** — rol atanmadığı için "aktif" değil |
| G4 bağımsızlık | fiilen ihlal, ama **kayıtlı** | tek `ens-skeptic` ile **hukuken** sağlanır (D-5) |
| Kurucu yol | belirsiz (Ç-05 açık) | belirsiz (Ç-05 açık) |

Yani RFC-6002'nin tek başına kabulü, ENS'in **en görünür yönetişim borcunu (G-27) normatif
dayanağından eder** ve G4'ün bağımsızlık şartını gevşetir. Bu, bölmenin *yapay* olduğunu
göstermez — ama bölmenin **tek yönlü bağımlı** olduğunu gösterir: RFC-6002 §4, RFC-6003'ün
kadro kararları olmadan **güvenli değildir**, oysa künyesi RFC-6003'e bağımlılık beyan etmiyor
(`RFC-6002:9` — `depends_on: [ENS-0000, GOV-000, GOV-030, RFC-6001]`).

**Zincirin üçüncü halkası aynı deseni tekrarlıyor.** `RFC-6004:224-226` gövdesinde açıkça
diyor ki: *"…uygulayacak kadro henüz atanmamıştır; bu tam olarak **RFC-6003'ün** konusudur …
RFC-6004, RFC-6003 kabul edilmeden **tam** yoldan geçemez."* Fakat `RFC-6004:9` künyesi
`depends_on: [ENS-0000, GOV-000, RFC-6001]` — **RFC-6003 yok.** (Bu, koordinatörün bana
"künyesinde beyan ediyor" diye ilettiği bulgunun doğrulanmış düzeltmesidir: beyan **gövdededir,
künyede değildir**.)

Sonuç: üç RFC'lik zincirde bağımlılık **metinde var, künyede yok**. Madde VIII (izlenebilirlik)
ve `depends_on` hijyeni açısından bu, üçünü birden ilgilendiren tek bir kusurdur. Bölme
savunulabilir; **bölünmüş parçaların künye bağları savunulamaz.**

### D-13 — `owner: ens-philosopher` ↔ fiilî yazar: G1 değil, **G5** sorunu

RFC-6002 `:6` künyesi `owner: ens-philosopher`; işi oturum sahibi yaptı. G1
(`GOV-000:33` — *"Authority follows accountability"*) ihlali **değildir**, çünkü korpusta
`owner` bir *rol-sorumluluğu* alanıdır, yazar alanı değil — `ROADMAP:230` bunu açıkça yazıyor:
*"`owner` alanı içerik alanına göre en-yakın role atandı … **bu bir stil/format çıkarımıdır,
resmî rol ataması değildir**."*

Gerçek kusur **G5**'tir (`GOV-000:37-38` — *"Her karar bir kayıt … bırakır; **sessiz karar
yoktur**"*): künyede **yazarlık provenance'ı taşıyan alan yok**, dolayısıyla *kim yazdı* sorusu
kayıtsız. Ve bu, RFC'yi özel olarak bağlar: RFC-6002 §3.1 R3 (`:94`) **başkalarından**
`ratified_by`/`ratified_at` istiyor, `:96-97` *"G5 ihlal edilir — bugün 9 `ratified` yapıtın
hiçbirinde onaylayan kayıtlı değildir"* diyor. **Kendi yazarını kaydetmeyen bir belge,
herkesten onaylayanını kaydetmesini isteyemez.** Performatif çelişki.

Ayrıca pratik sonuç: D-6'daki *"yazarı kendi turunu işaretleyemez"* kuralı, künye yazarı
yanlış/eksik gösterdiği sürece **denetlenemez**. Kural, kendi uygulanabilirlik koşulunu
kaydetmiyor.

## Katıldığım noktalar

Kalibre olmayan bir saldırı, kendisi de kalibre değildir. Şunlar gerçek ve iyi:

- **K-1 — Alıntı hijyeni SCAN-03'ün üstünde.** RFC, kaynak raporun **üç** satır kaymasını
  sessizce düzeltmiş: `SCAN-03:587` G4'ü `canonical-process.md:44` diye gösteriyor (gerçek: 45);
  `SCAN-03:588` GOV-000'i `:33-35` diye gösteriyor (G4 aslında `:36`, `:33` G1'dir);
  `SCAN-03:590` `roles.md:49` diyor (gerçek: 63). RFC-6002 `:28-29` **doğru** satırları veriyor.
  Kural 3.5'in istediği tam olarak budur ve RFC bu sınavı **geçmiştir**.
- **K-2 — Çekirdek tez doğru.** `ratified` ≠ `Canonical` ayrımı `GOV-000:36`, `GOV-000:46-48`,
  `canonical-process.md:45-48`, `maturity-model.md:28-31` ve `ENS-4000:40` tarafından bağımsızca
  destekleniyor. SCAN-03'ün "9 yapıtın hiçbiri G4'ü sağlamıyor" panik bulgusu **gerçekten
  yanlıştı** ve RFC bunu doğru teşhis etti.
- **K-3 — §3'ün boşluk teşhisi RFC'nin en değerli kısmıdır ve dediğinden güçlüdür.**
  RFC *"hiçbir yazılı kural bırakmaz"* diyor; gerçek daha keskin: **kural vardır ve G3'ü
  ihlal eder.** `maturity-model.md:28` ratifikasyonu doğrudan SKR verdict'ine bağlıyor ve
  `ENS-4000:40,56,66,91` bunu *"ratified — SKR-004"* kalıbıyla korpusa kazımış. Yani doğrulama
  **kurumsallaşmış biçimde** onayın yerine geçiyor. Bu, RFC'nin argümanını zayıflatmaz,
  keskinleştirir.
- **K-4 — Ontology'nin M5 listesinde olması gerektiği olgusal olarak kanıtlanmış.** `:107-109`'un
  iddiası doğrulandı: 13 SKR `validation_dimension: ontology` taşıyor.
- **K-5 — R3 (`ratified_by`/`ratified_at`) doğru ve `SCAN-03:576` (B-01) ile bire bir örtüşüyor;
  kapsamı dışına çıkarıp `ens-style-guardian`'a bırakması (`:137`) rol disiplinine saygıdır.**
- **K-6 — fc-2'yi yazmış olması.** Kusurlu formüle edilmiş olsa da (D-3), RFC kendisini
  vurabilecek koşulu **kendisi** yazdı. Bu incelemenin iki `refuted`'ı, RFC'nin kendi kurduğu
  sınavın çalıştığının kanıtıdır — Madde X'in istediği tam olarak budur.

## Sahibine talepler

Şiddet sırasına göre. **Bloke ediciler kapanmadan RFC Madde XIV yordamına girmemelidir.**

### Bloke edici

- **T-A (§4 tek-aktör kaçağı — D-5).** "Tüm aktif boyutlar" önerisine iki cümle eklenmeden
  kabul edilmemeli: (1) *"Aynı aktörün ürettiği farklı-boyutlu kayıtlar **tek** validator
  sayılır; G4 farklı **aktör** ister."* (2) *"Aktif olmayan boyut muaf değil **ertelenmiştir**
  ve ROADMAP'te açık borç satırı taşır."* İkincisi olmadan §4, G-27'nin (`ROADMAP:241`)
  normatif dayanağını yok eder — yani RFC, kapatmayı vaat ettiği deliği açar.
- **T-B (§2.1 geri çekilir — D-10).** *"İki bağımsız türetme"* iddiası savunulamaz (ortak
  kaynak `SCAN-03:587`; ardışıklık `ROADMAP:240`). Önerilen dürüst ifade: *"Bir yorumsal
  türetme (`ens-philosopher`, G-26) ve onun girdilerinin bağımsız **alıntı denetimi**. Bu bir
  kaynak doğrulamasıdır, **G4 anlamında ikinci bir validator değildir**."*
- **T-C (§2 temeli tazelenir — D-1/D-3).** `maturity-model.md:34`'e dayanmak yerine
  `GOV-000:46-48` + `canonical-process.md:47-48` + `ENS-4000:40`'a dayanılır. `:48`'deki
  *"≥2 bağımsız boyut → `canon: true` (M5)"* denklemi düzeltilir: **`canon`'a iki yol vardır** —
  ampirik (M5) ve kurucu (`RFC-6001:339-345` §7.2). fc-2 yeniden yazılır: koşul
  *"`constitutive: false` olup M5 olmadan `canon: true` taşıyan bir yapıt meşru sayılıyorsa"*
  olmalıdır; bugünkü hâliyle ENS-0000 tarafından **tetiklenmiş** durumdadır.
- **T-D (§10 düzeltilir — D-6).** *"Yazarı kendi turunu `survives` işaretleyemez (GOV-000 G2 + G4)"*
  → dayanak **G4 + G3**'tür. G2 bir **kanonlaştırma** yasağıdır (`roles.md:44-49`, SCAN-01).
  RFC, emsal aldığı düzeltmenin kendisini ihlal ediyor.
- **T-E (§6 tablosuna bayat satır eklenir — D-1).** `maturity-model.md:34`, `RFC-6001:385-386`
  (§10.5) gereği zaten hizalanmayı bekliyor. RFC-6002 o satıra dokunuyorsa, **hizalama borcunu
  da kapatmalı** ya da açıkça devretmeli; aksi hâlde bilinen-bayat bir cümleyi taşıyıcı yapar.

### Yüksek

- **T-F (R2'ye varsayılan aktör — D-7).** *"GOV-010 aksini söyleyene kadar R2'nin aktörü
  `ens-style-guardian` (Custodian); hiza gerektiren yapıtlarda ek olarak `ens-ceo`."* Emsal
  hazır: `SCAN-03:603` (Ö-03) + `STYLE-SIGNOFF-RFC-6001.md`. Bu cümle yazılmazsa fc-3
  RFC'nin kendi ifadesiyle *"boşluğu kayıtlı hâlinden daha kötü"* bir duruma sokar.
- **T-G (geçiş adı düzeltilir — D-4).** §3/§5/§6'daki *"`review → ratified`"*
  → **`skeptic-challenged → ratified`** (`maturity-model.md:28`). Aksi hâlde önerilen kural
  var olmayan bir geçişi düzenler, gerçek geçişi düzenlemeden bırakır.
- **T-H (künye bağları — D-12).** RFC-6002 §4, RFC-6003'ün kadro kararlarına **operasyonel
  olarak bağımlıdır**; ya `depends_on`'a RFC-6003 eklenir ya da §4, RFC-6003'e devredilir.
  Aynı kusur `RFC-6004:9` ↔ `:224-226`'da da var (gövde bağımlılığı beyan ediyor, künye etmiyor)
  — üçü birlikte düzeltilmeli.
- **T-I (yazarlık kaydı — D-13).** RFC, R3 ile başkalarından onaylayan kaydı isterken kendi
  yazarını kaydetmiyor. Ya künyeye `authored_by` eklenir ya da RFC gövdesinde tek cümleyle
  beyan edilir. **G5 herkese aynı biçimde uygulanır.**

### Orta

- **T-J** §2.2 silinir ya da *"destekleyici yorum ilkesi, birincil delil değil"* diye
  işaretlenir (D-11) — §8.2 onu zaten etkisizleştiriyor.
- **T-K** `:29`'daki `canonical-process.md:45` alıntısına *"ör."* geri konur (D-8); Ç-03'ün
  "çatışma" çerçevesi buna göre yumuşatılır.
- **T-L** `:106-107`'deki kronoloji cümlesi silinir (D-9) — **DOĞRULANMADI**; argüman onsuz da
  ayakta.
- **T-M** §9.2'ye (geriye dönüklük) somut bir sonuç eklenir: R1-R3 kabul edilirse
  `ENS-4000:40,56,66,91`'in *"ratified — SKR-NNN"* kayıtları **uyumsuz** hâle gelir; ENS-4000
  `canon: true` olduğu için bu, sözlüğü de etkileyen bir edimdir.

## Tekrar-sınav koşulu

T-A … T-E kapandığında **yeni ve bağımsız** bir tur gerekir; bu kaydın yazarı o turu yapamaz
(G4 — `GOV-000:36`). O turda sınanacak tek soru: *"§4'ün tek-aktör kaçağı gerçekten kapandı mı,
yoksa 'farklı aktör' tanımı ROSTER'a devredilerek bir kez daha ertelendi mi?"*

Ayrıca bu kayıt **tek boyutludur** (scientific/constitutional). `GOV-000:36` gereği RFC-6002'nin
`Canonical` hedefi varsa bu SKR **tek başına yetmez**; RFC-6001 emsali (3 SKR + `ens-ceo` +
`ens-style-guardian`) asgari çıtadır. İtiraz yolu **G6** (`GOV-000:39-40`).
