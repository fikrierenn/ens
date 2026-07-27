---
id: SKR-048
type: skeptic-review
origin: RFC-6003
depends_on: [RFC-6003]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-27
validation_dimension: constitutional
verdict: wounded
---

# SKR-048 — RFC-6003 (Boyut Sözlüğü ve Validator Kadrosu) Saldırısı

> **Bağımsızlık beyanı (GOV-000 G4, `governance/000-governance-principles.md:36`):**
> RFC-6003'ü oturum sahibi yazdı; bu kayıt ayrı `ens-skeptic` context'inde üretildi.
> Paralel `ens-architect` turu (ARCH-0001) **okunmadı**. `dotnet test` gerekmedi.
> Sayımlar `Glob`/`Grep` ile mekanik yapıldı; hiçbir sayı tahmin edilmedi.

## Verdict

**`wounded`** — Normatif çekirdek (**kurucu yol G4'ten muaf değildir**; **ethical sıfırı meşru
erteleme değil borçtur**; **boyut sözlüğü eksiktir**; **bağlayıcı kısıt yordam değil kadrodur**)
sağ çıkıyor ve §3'ün üç dayanağı **birebir doğrulandı**. Buna karşılık RFC'nin *ampirik* ayağı
kırıldı: §1.1'in tek kanıtı olan D-5 vakası **engineering boyutu değildir** — kaynak belge
kendi yönteminin *"statik kaynak okuması + teori metni karşılaştırması"* olduğunu ve
`dotnet build/test`'in **çalıştırılamadığını** yazıyor. §2'nin yordamsal çözümü **geçersizdir**
(iddia edilen çatışma iki belge arasında değil, `validation-framework.md`'nin **kendi içinde**).
45 sayımı **46**'dır. Ve §8, tezini yanlışlanamaz kılan bir çerçeve kuruyor.

Kısaca: **doğru sonuçlar, üç kırık ayak.** Sonuçlar korunabilir — ama gerekçelerin üçü
değiştirilmelidir.

| Eksen | Sonuç |
|---|---|
| §1 kadro teşhisi (bağlayıcı kısıt ROSTER'dır) | **survives** — ve en değerli kısım |
| §1 "45/45" sayımı | **wounded** — gerçek 46/46; oran sağlam, sayı yanlış, fc-1 lafzen tetiklendi |
| §1.1 ampirik kanıt (D-5 → engineering) | **refuted** — fc-2 tetiklendi |
| §2 Ç-04 çözümü (yordamsal argüman) | **refuted** — çatışma intra-doküman; ama **sonuç yine doğru** |
| §2 maddi gerekçe (P7 hiç sınanmadı) | **survives** — güçlü ve doğrulandı |
| §3 Ç-05 (kurucu muafiyet yok) | **survives** — üç dayanak da birebir doğrulandı |
| §3.1 boyut sözlüğü boşluğu | **wounded** — `constitutional` zaten **fiilen kullanılıyor**, RFC görmemiş |
| §4 Ontology Validator önerisi | **wounded** — çatışmayı çözen üçüncü seçenek atlanmış |
| §5 ayrı-RFC gerekçesi | **wounded** — bağımlılık künyede değil gövdede |
| §8 öz-uygulama | **refuted** — yanlışlanamazlık zırhı (T1/T2 eşitlemesi) |

## Alıntı doğrulaması (kural 3.5)

| # | RFC'nin atfı | Gerçek | Sonuç |
|---|---|---|---|
| 1 | §1 *"Bugüne kadar yazılmış SKR: **45**"* | mekanik sayım: **46** (SKR-001…SKR-046, boşluksuz) | ❌ **yanlış** (D-1) |
| 2 | §1 *"Bunlardan `ens-skeptic` tarafından yazılan: 45 — %100"* | 46/46'sında `owner: ens-skeptic` | ✅ **oran doğru** |
| 3 | §1 `roles.md` *"Engineering/Business/Ethical Validator, Governance body: fazı gelince"* | `governance/roles.md:63` birebir | ✅ |
| 4 | §2 `validation-framework.md` → *"tüm fazlar"* | `:29`: *"Ethical Validation (AI çağında zorunlu) — **tüm fazlar**"* | ✅ |
| 5 | §2 *"`roles.md` boyut otoritesi olarak **onu gösterir**"* | `roles.md:55` başlığı: *"## Validator boyutları (**validation-framework.md**)"* | ✅ atıf doğru — **ama çıkarım geçersiz** (D-3) |
| 6 | §3.1 `RFC-6001:175` *"`constitutive` … muaf tutmaz. Yalnızca yanılma kipini değiştirir"* | satır 175 **birebir** | ✅ |
| 7 | §3 *"RFC-6001 zinciri: SKR-034 wounded → SKR-035 wounded → SKR-036 survives + CEO-0002 + STYLE-SIGNOFF"* | üç SKR dosyası mevcut, `validation_dimension: constitutional`; `STYLE-SIGNOFF-RFC-6001.md` mevcut | ✅ |
| 8 | §3.1 *"`validation-framework.md` **beş** boyut sayar"* | `:25-29` — Scientific · Ontology · Engineering · Business · Ethical | ✅ |
| 9 | §3.1 *"**`constitutional` diye bir boyut tanımlı değil**"* | `validation-framework.md`'de tanımlı **değil** — ama SKR-034/035/036 künyelerinde **kullanılıyor** | ⚠️ eksik (D-5) |
| 10 | §1.1 *"`AUDIT-WAVE2-FIDELITY` … fiilen bir **engineering** denetimidir"* | belge `:8-16` kendi yöntemini *"statik kaynak okuması + teori metni karşılaştırması"* diye tanımlıyor; `dotnet build/test` **çalıştırılamadı** | ❌ **yanlış** (D-2) |
| 11 | §1.1 *"SKR-040 ve SKR-041 … bağımsız, scientific"* | ikisi de `owner: ens-skeptic`, `validation_dimension` alanı **yok** | ⚠️ etiket çıkarım (D-2) |
| 12 | §2 *"ENS Faz 3-4'te agent runtime ADR'leri üretti … P7 hiç bağımsız etik incelemeden geçmedi"* | `SCAN-03:580` (D-02b) ve `ROADMAP:241` (G-27) aynısını söylüyor | ✅ |
| 13 | §7.4 *"`canon: true` olan ENS-1000 tek boyutlu zincire sahip (ROADMAP G-28)"* | `ROADMAP:242` birebir; ENS-1000 `canon: true` doğrulandı | ✅ |
| 14 | §4 *"`ens-architect` … ENS-4001/4010'un owner'ı"* | bu context'te ENS-4001/4010 künyeleri **okunmadı** | ⚠️ **DOĞRULANMADI** (uydurma iddia etmiyorum) |

**Ara sonuç:** uydurulmuş kaynak **yok**; §3'ün dayanakları birebir sağlam. İki atıf olgusal
olarak yanlış (#1, #10) ve ikisi de **RFC'nin kendi failure condition'larının** hedefidir.

## Yenilik incelemesi — sayım ve ampirik ayak

### D-1 — 45 değil **46**; fc-1 lafzen tetiklendi, ama yanlış şeyi ölçüyor

RFC `:120-122` fc-1: *"**45/45 sayımı yanlışsa.** Tüm §1 ona dayanır. Sayım
`find . -name 'SKR-*.md'` + her dosyanın `owner:`/yazar alanı ile **mekanik doğrulanabilir**;
yanlışsa RFC'nin ampirik ayağı çöker."*

Sayımı yaptım (`Glob **/SKR-*.md`, bu incelemenin kendi iki dosyası hariç):

- **46 dosya**, SKR-001 … SKR-046, **boşluksuz**.
- Kaçırılan dosya: **`.claude/rules/SKR-046-tier3-discipline-rules.md`** — tek `reviews/`
  dizini **dışında** duran SKR. `id: SKR-046`, `type: skeptic-review`, `owner: ens-skeptic`
  (`:2,3,13`).
- `owner:` alanı: **46/46'sı `ens-skeptic`.** Farklı bir owner taşıyan tek bir SKR yok.

Yani RFC muhtemelen `reviews/SKR-*` deseniyle saydı; kendi ilan ettiği `find . -name 'SKR-*.md'`
komutuyla saysaydı 46 bulurdu. **RFC, kendi failure condition'ının belirttiği testi
uygulamamış.** Bu, RFC-6002/6003'ün doğduğu dersin (kural 3.5) tam olarak ikinci ayağıdır:
*"bulgu, iddia ettiği şeyi mi kanıtlıyor?"*

**Ama fc-1 kötü formüle edilmiş.** Taşıyıcı iddia numaratör değil **orandır**: "tek rol, tek
lens". 45→46 düzeltmesi oranı **%100'den %100'e** taşır — yani hipotezi hiç etkilemez. fc-1,
tezi çürütmeyen bir olguyu *"ampirik ayak çöker"* ilan ediyor. Madde X açısından bu **yanlış
hedefe nişan almış falsifier**'dır: tetiklendiğinde tezi değil yalnızca özeni vurur.
(Aynı kusur RFC-6002 fc-2'de de var — SKR-047/D-3. İki kardeş RFC'de tekrarlanan bir kalıp.)

> **Daha da keskin bir nokta:** kaçırılan dosyanın **hangisi** olduğu anlamlı. SKR-046, bu
> oturumda `.claude/rules/` katmanına yazılan bir denetimdir — yani korpusun *yeni* ve
> *dizin-dışı* bölgesi. RFC'nin sayımı, korpusun kendi büyüme kenarını göremiyor.

### D-2 — §1.1 refuted: `AUDIT-WAVE2-FIDELITY` **engineering değildir**; fc-2 tetiklendi

Bu, RFC'nin **tek ampirik kanıtıdır** ve RFC bunu kendisi böyle işaretliyor
(`:123-125` fc-2: *"D-5'in 'engineering yakaladı' anlatısı yanlışsa … §1.1'deki tek ampirik
kanıt geçersizleşir ve boyut çeşitliliği **yalnızca teorik bir gereklilik** olarak kalır."*)

Üç bağımsız gerekçeyle bu koşul **tetiklenmiştir**:

**(a) Belge kendi yöntemini engineering'e aykırı tanımlıyor.**
`7000-reference-implementation/AUDIT-WAVE2-FIDELITY.md:12-16`:

> *"**⚠️ Bu denetimin sınırı (SKR-041 emsali — fabrikasyon yok).** Bu context'te
> **shell/Bash aracı yoktu**; `dotnet build` ve `dotnet test` **ÇALIŞTIRILAMADI.** Aşağıdaki
> hiçbir bulgu bir test koşusuna dayanmıyor — hepsi **statik kaynak okuması + teori metni
> karşılaştırması**."*

`validation-framework.md:44` Engineering'i şöyle tanımlıyor: *"inşa edilebilirlik, ölçek,
replaceability, **test edilebilirlik** (Faz 3-4)."* Hiçbir build, hiçbir test koşusu, ölçek
ya da replaceability incelemesi içermeyen bir denetim, bu tanımın **hiçbir** ölçütünü
karşılamıyor.

**(b) D-5'in içeriği matematiksel/kavramsal, mühendislik değil.** `AUDIT-WAVE2-FIDELITY:236-239`
bulgunun kimin kusuru olduğunu açıkça yazıyor:

> *"**Kimin kusuru:** Kod burada teoriye **sadıktır** … Yanlış olan **kodun formülü değil,
> ortogonallik iddiasıdır** — ve o iddia hem teoride hem kod yorumunda tekrarlanıyor."*

Yani bulgu bir **teori iç-tutarlılık** bulgusudur: "ortogonal" ilan edilen iki eksenin ikisi de
`c`'nin monoton fonksiyonu (`:224`). `validation-framework.md:40` Scientific checklist'i
şöyle sayıyor: *"yenilik, yanlışlanabilirlik, kanıt, varsayım, karşı-argüman, **iç tutarlılık**."*
D-5 tam olarak son maddedir. **Aynı lens, farklı korpus.**

**(c) Korpusun kendi kaydı bunu doğruluyor: aynı kusuru bir scientific SKR devraldı ve derinleştirdi.**
`2000-theory/reviews/SKR-045-company-memory-v040-confidence-double-count.md` — `owner: ens-skeptic`,
`origin: ENS-2003`, D-5 düzeltmesinin **1. bağımsız turu** (`:14-15`), verdict `wounded` (`:21`).
Yani D-5 hattı, boyut değiştirmeden `ens-skeptic` tarafından sürdürülebildi.

**Gerçek ayırt edici neydi?** Boyut değil, **kapsam ve zamanlama**: SKR-040/041 ENS-2003'ün
*kendi metnini* okudu; FIDELITY denetimi **teori metni ile kernel kodunu yan yana** koydu
(`:8-9`: *"55 `// TRACE:` iddiasının tamamı … atıf yapılan teori/ADR bölümü açıldı ve satır
düzeyinde karşılaştırıldı"*). İki metnin **birlikte** okunması, tek metnin okunmasında görünmez
olan bir çelişkiyi açığa çıkardı.

**Bu, RFC'nin tezi için hâlâ bir argümandır — ama başka bir tezdir.** Kanıtlanan şey
*"farklı **boyut** gerekir"* değil, *"farklı **kapsam/korpus** gerekir"*dır. RFC bunu boyut
lehine kullanarak **kanıtı fazla genelliyor**. Boyut çeşitliliği teorik olarak hâlâ gerekli
olabilir — ama §1.1 sonrası RFC'nin elinde bunun **hiçbir ampirik kanıtı kalmıyor** ve
RFC'nin kendi ifadesiyle *"yalnızca teorik bir gereklilik olarak kalır."*

## Yanlışlanabilirlik

### D-6 — §8 bir **yanlışlanamazlık zırhıdır** (en ince kusur)

RFC `:143-148`:

> *"**Ve bu RFC için özel bir ironi vardır:** boyut çeşitliliğini savunan bir belgenin kendisi
> de tek boyuttan doğrulanacaktır — çünkü savunduğu roller henüz atanmamıştır. **Bu, RFC'nin
> kendi tezinin en iyi kanıtıdır** ve gizlenmemektedir."*

**Steelman — dürüstlük payı gerçektir.** Bir sınırı gizlememek erdemdir ve RFC bunu yapıyor.
Sorun beyanın kendisinde değil, ona verilen **epistemik statüdedir**: *"kendi tezinin en iyi
kanıtı"*.

**Kusur: iki farklı tez birbirine geçiriliyor.**

| | Tez | Doğrulanabilirliği |
|---|---|---|
| **T1** | *ENS'te tek validator rolü vardır; kadro eksiktir.* | Önemsizce doğru; `roles.md:61,63` + 46/46 sayımı ile **zaten** kanıtlı (§1) |
| **T2** | *Boyut çeşitliliği, tek boyutun kaçırdığı kusurları yakalar.* | **Asıl ve tartışmalı iddia**; tek kanıtı §1.1 idi ve D-2'de düştü |

§8, T1'in gözlemini (tek boyuttan doğrulanacak) alıp **T2'nin kanıtı** diye sunuyor. Ama
"kadroda tek rol var" gözlemi, "çok rol daha iyi korur" iddiasını **hiç** desteklemez — yalnızca
kendisini destekler.

**Yanlışlanabilirlik testi (Madde X):** *Bu RFC'yi hangi gözlem çürütür?*

- Tek boyuttan doğrulanırsa → RFC: *"tezimin kanıtı"*.
- Çok boyuttan doğrulanırsa → RFC: *"kural işledi"* (§8'in zımni alternatifi).
- Tek boyuttan geçip **hiçbir kusur bulunmazsa** → *"tek boyut yetti"* denebilirdi ama
  §8 bu okumayı dışlıyor, çünkü tek-boyutluluğu peşinen bir **eksiklik kanıtı** ilan ediyor.

Her sonuç doğrulayıcı olarak okunabiliyorsa, ortada bir yanlışlanabilirlik yoktur. Anayasa
Madde X'in yasağı budur: *"Saldırılamayan bir teoriye güvenilemez."*

> **Kapatan düzeltme:** §8, T2 için **gerçek bir yanlışlayıcı** ilan etmeli. Örnek ve ucuz
> olanı: *"Ontology Validator ayrı bir aktöre verildikten sonraki ilk **beş** çok-boyutlu
> turda, ikinci boyut birinci boyutun kaçırdığı **hiçbir** bulgu üretmezse, T2 çürümüştür ve
> boyut ayrımı tek aktöre geri döner."* Bu, RFC-6003'ün fc-3'ünü (*"iki isim, tek bakış"*)
> beyandan **ölçüme** çevirir.

### D-7 — fc-3 doğru teşhis edilmiş ama **ölçüm yordamı yok**

`:126-129` fc-3: *"Ayrı bir Ontology Validator atanır ama fiilen `ens-skeptic` ile **aynı lensi
kullanırsa** … kural sağlanmış *görünür*, korumaz … mevcut hâlden **daha kötüdür**, çünkü
kayıtlı borcu görünmez yapar."*

Bu, kaydın en iyi cümlelerinden biridir ve doğrudur. Ama "aynı lens" **nasıl ölçülür**
yazılmamış. Ölçütsüz bir failure condition, tetiklenip tetiklenmediği belirlenemeyen bir
koşuldur — yani pratikte yanlışlanamaz. D-6'daki düzeltme (ilk beş turda ikinci boyutun
**özgün bulgu** üretmesi) bu boşluğu da kapatır.

### D-8 — fc-4 doğru ve **zaten gerçekleşmiş** bir deseni tarif ediyor

`:130-131`: *"**`constitutional` boyutu tanımlanır ama onu yürütecek rol atanmazsa** — Ç-04'ün
ethical için ürettiği durumun aynısı yeni bir boyutta tekrarlanır."*

Bu koşul **hipotetik değil**: `constitutional` boyutu resmen tanımlanmadan **fiilen
kullanılmaya başlanmış** ve onu yürüten ayrı bir rol **yok** (SKR-034/035/036, hepsi
`owner: ens-skeptic`). Yani fc-4'ün tarif ettiği durum, tanımlama adımı **atlanarak** zaten
oluşmuş durumdadır (bkz. D-5). RFC bunu görmüş olsaydı fc-4'ü *"gerçekleşmiş"* olarak
işaretlemesi gerekirdi.

## Varsayım haritası

| # | Varsayım | Nerede | Kırılma koşulu | Durum |
|---|---|---|---|---|
| V1 | 45 SKR var, hepsi tek rol | §1 | mekanik sayım farklı çıkarsa | **kısmen kırıldı** — sayı yanlış, oran sağlam (D-1) |
| V2 | D-5'i farklı bir **boyut** yakaladı | §1.1 | denetim aynı lensi kullanıyorsa | **kırıldı** (D-2) |
| V3 | Ç-04 iki **belge** arasında bir çatışmadır | §2 | çatışma tek belgenin içindeyse | **kırıldı** (D-3) |
| V4 | `constitutional` boyutu tanımlı değil, kullanımda değil | §3.1 | korpusta kullanılıyorsa | **kırıldı** (D-5) |
| V5 | Ontology Validator ya `ens-architect` ya başkası (ikili seçim) | §4 | mekanik/üçüncü bir aktör mümkünse | **kırıldı** (D-4) |
| V6 | Tek boyuttan doğrulanmak tezi destekler | §8 | her sonuç destekliyorsa | **kırıldı** (D-6) |
| V7 | Kurucu yol G4'ten muaf değildir | §3 | RFC-6001 muafiyet vermişse | **ayakta** — üç dayanak doğrulandı |
| V8 | Ethical'ın sıfır olması borçtur, erteleme değil | §2 | "tüm fazlar" okuması düşerse | **ayakta** (D-3'e rağmen — bkz. sonuç) |

### D-3 — §2'nin yordamsal çözümü **geçersiz**: çatışma `roles.md` ile değil, `validation-framework.md`'nin kendisiyle

RFC `:48-57` Ç-04'ü iki belge arasında bir çatışma olarak kuruyor ve şu yordamla çözüyor:

> *"**Çözüm: `validation-framework.md` kazanır.** Gerekçe yordamsaldır: `roles.md` boyut
> otoritesi olarak **onu gösterir**; bir belge, atıf yaptığı kaynağı geçersizleştiremez."*

Atıf doğru: `roles.md:55` başlığı gerçekten *"## Validator boyutları (validation-framework.md)"*.
**Ama çıkarım geçersiz, çünkü `roles.md` kaynağını geçersizleştirmiyor — onu tekrarlıyor.**

`.claude/standards/validation-framework.md:48-51` (§Agent eşlemesi):

> *"- `ens-skeptic` = **Scientific Validator** (adversarial).
> - Ontology Validator + `formal-checker` (Ontology Linter) — Faz 1'de aktif …
> - **Engineering/Business/Ethical Validator — fazı gelince (ROSTER).**"*

`governance/roles.md:63`: *"Engineering/Business/Ethical Validator, Governance body:
**fazı gelince** (ROSTER)."*

Yani *"fazı gelince"* ifadesinin kaynağı `roles.md` **değil**, bizzat `validation-framework.md`'dir.
`roles.md` burada sadık bir türevdir. Çatışma **intra-dokümandır**: `validation-framework.md:29`
(*"Ethical Validation — **tüm fazlar**"*) ↔ `validation-framework.md:51` (*"Ethical
**Validator** — fazı gelince"*).

**Sonuç:** "kazanan belgeyi seç" yordamı burada hiçbir şey çözmez — kazanan belge çatışmanın
**iki tarafını da** içerir. RFC doğru cevaba **geçersiz bir yoldan** varmıştır.

> **Doğru çözüm zaten metnin içinde ve daha güçlü — RFC bunu kullanmalı:** iki satır **çelişmiyor**,
> iki **farklı şeyden** söz ediyor. `:29` bir **boyutun normatif aktifliğini** söyler
> (Ethical Validation tüm fazlarda gereklidir); `:51` bir **rolün atanma zamanlamasını** söyler
> (Ethical Validator henüz atanmadı). Gereklilik ile kadro farklı eksenlerdir.
> Bu okuma altında sonuç **aynen korunur ve pekişir**: gereklilik yürürlüktedir, rol yoktur →
> karşılanamayan bir yükümlülük → **borç**, erteleme değil. Yani `ROADMAP:241`'in (G-27) ve
> RFC `:64`'ün sonucu doğru; yalnızca gerekçesi değişmeli.
>
> Bu düzeltme RFC'yi **güçlendirir**, çünkü yordamsal argüman (`roles.md` türevdir) hem yanlış
> hem de gereksizdi — ve aynı yordam RFC-6002 §3'te de kullanılıyor (`:57`: *"Aynı yordam
> RFC-6002 §3'te de uygulandı"*). Orada geçerlidir (GOV-000 ↔ `roles.md`), burada değildir;
> RFC'nin ikisini aynı sayması bir **yordam aşırı-genellemesidir**.

### D-4 — §4'ün "çözülmemiş gerilimi" aslında **üçüncü bir seçenekle çözülüyor** ve RFC onu atlamış

RFC `:100-102`:

> *"**Uyarı — bu öneri kendi kaçağını taşıyor.** Ontoloji boyutunun en yetkin aktörü, ontoloji
> yapıtlarının da owner'ıdır. Rolü ona verirsek yapıtlarının yarısında kullanılamaz; başkasına
> verirsek yetkinlik düşer. Bu gerilim **çözülmemiştir** ve gizlenmiyor."*

Dürüst — ama ikili seçim (V5) **yanlış kurulmuş**. Korpus üçüncü bir aktör tipini zaten
tanımlamış ve inşa etmiş:

1. `validation-framework.md:50`: *"**Ontology Validator + `formal-checker` (Ontology Linter)** —
   Faz 1'de aktif (Meta Model gerektirir)."* — yani boyut, en başından beri **insan/ajan +
   mekanik denetleyici** çifti olarak tasarlanmış.
2. `ROADMAP.md:233` (G-09/10): Ontology Linter **yazılmış** — *"`tools/ens-ontology-linter/`"*,
   *"Karar: LLM-agent DEĞİL — … deterministik araç"*, iki invariant + pozitif/negatif kontrol.

Deterministik bir linter'ın **yazarlık çıkarı yoktur**: `ens-architect` kendi ENS-4010'unu
yazsa bile linter'ın profile-satisfiability ve transitivity denetimi ondan bağımsızdır. Yani
ontoloji boyutunun **mekanikleştirilebilir kısmı** için G2/G4 gerilimi **yoktur**; gerilim
yalnızca yargısal artık için kalır ve o artık çok daha küçüktür.

**Öneri sayılır mı?** Evet — açık soru bırakmak RFC'de meşrudur (RFC-6001 §8.3 emsali). Ama
*"çözülmemiştir"* demek, **korpusta duran çözümü görmemek** anlamına geldiğinde artık dürüstlük
değil **eksik incelemedir**. §4, üç seçeneği ve aralarındaki iş bölümünü yazarak kapatılabilir:
mekanik invariant → linter; yargısal ontoloji → `ens-architect` (kendi yapıtları hariç);
`ens-architect`'in kendi yapıtları → `ens-skeptic` (bugünkü hâl korunur).

## En güçlü karşı-argüman

### D-5 — `constitutional` boyutu **tanımsız değil; tanımlanmadan kullanılıyor.** Sözlük boşluğu, RFC'nin sandığından daha ciddi

RFC `:79-84` (§3.1) boşluğu şöyle kuruyor:

> *"Muafiyet yoksa, kurucu bir yapıt **hangi** iki boyuttan doğrulanacak? Doğal cevap
> {ontology, constitutional} — ama **`constitutional` diye bir boyut tanımlı değil**
> (`validation-framework.md` beş boyut sayar…)"*

ve iki seçeneği **açık soru** olarak bırakıyor (`:86-89`): (a) altıncı boyut, (b)
{Ontology + Scientific} eşlemesi.

**Depodaki olgu, seçeneği zaten fiilen belirlemiş — ve bunu izinsiz yapmış:**

| Dosya | Künye alanı |
|---|---|
| `6000-rfc/reviews/SKR-034-rfc-6001-constitutive.md:10` | `validation_dimension: constitutional` |
| `6000-rfc/reviews/SKR-035-rfc-6001-constitutive-round2.md:10` | `validation_dimension: constitutional` |
| `6000-rfc/reviews/SKR-036-rfc-6001-constitutive-round3.md:10` | `validation_dimension: constitutional` |

Buna karşılık `validation-framework.md:36-37` **kapalı bir değer kümesi** ilan ediyor:

> *"İleride her kayıt bir `dimension` taşır: **`scientific | ontology | engineering | business |
> ethical`**."*

Yani `constitutional`, izin verilen enum'da **yok** ama korpusta **üç kez yazılmış** — üstelik
tam da RFC-6003'ün §3'te *"korpusun en ağır doğrulama yolu"* diye emsal aldığı zincirde
(`:74-77`). Bu üç kayıt bugün **şema-dışıdır**.

**Neden bu, açık sorudan daha ciddi bir bulgu:**

1. **RFC yanlış soruyu soruyor.** Soru *"tanımlayalım mı?"* değil, *"tanımsız kullanımı nasıl
   geriye dönük meşrulaştıracağız?"*tur. (a) seçeneği artık nötr bir tercih değil, **mevcut
   durumun onaylanmasıdır**; (b) seçeneği ise üç kaydın künyesini **geçersiz** kılar. RFC bu
   asimetriyi görmüyor ve iki seçeneği eşit ağırlıkta sunuyor.
2. **Terminoloji sürüklenmesi (Madde IX / ENS-4000).** Yeni bir doğrulama boyutu, hiçbir
   yapıtta tanımlanmadan künye alanına girmiş. `validation-framework.md` (owner: `ens-skeptic`)
   ve `ENS-4000` (sözlük, `canon: true`) ikisi de bunu bilmiyor.
3. **RFC-6003 bunu yakalamak için en uygun konumdaydı** — konusu tam olarak "boyut sözlüğü".
   Yakalamamış olması, §1'in *"boyut çeşitliliği pratik olarak yok"* tablosunun neden eksik
   olduğunu da açıklıyor: tablo yalnızca `owner` alanına bakmış, `validation_dimension` alanına
   bakmamış.

**Ve bu son nokta §1'in tablosunu düzeltiyor.** Korpusta `validation_dimension` etiketleri
zaten **dört** farklı değer taşıyor: `ontology` (SKR-017/018/019/020/021/022/023/028/030/031/032/038/039),
`engineering` (SKR-024/025/026/027/029/037), `constitutional` (SKR-034/035/036), `scientific`
(SKR-033). Yani RFC'nin *"boyut çeşitliliği bir kural olarak var, pratik olarak yok"* (`:33`)
cümlesi **fazla güçlüdür**: boyut **etiketi** pratikte var; yok olan şey **aktör
çeşitliliğidir**.

**Bu düzeltme RFC'nin tezini zayıflatmaz — hedefini netleştirir ve güçlendirir.** Asıl sorun
"boyut yok" değil, *"boyutlar var ama hepsi tek aktörün elinde"*dir; ve G4'ün lafzı
(`GOV-000:36` — *"≥2 **bağımsız** validator"*) aktör bağımsızlığını ister, etiket çeşitliliğini
değil. RFC bu formülasyonu benimserse hem §1 tablosu doğru olur, hem §4'ün gerekçesi
(*"tek rol, tek bakış açısıdır"*, `:93`) **ampirik** bir temele oturur — D-2'de kaybettiği
ampirik ayağın yerine geçebilecek tek şey budur.

## İç tutarlılık
(doldurulacak)

## Sahibine talepler
(doldurulacak)
