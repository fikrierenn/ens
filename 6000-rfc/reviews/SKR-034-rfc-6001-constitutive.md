---
id: SKR-034
type: skeptic-review
origin: RFC-6001
depends_on: [RFC-6001, ENS-0000, ENS-4000, STD-METADATA-HEADER, STD-MATURITY-MODEL]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
validation_dimension: constitutional
---

# SKR-034 — RFC-6001 (Constitutive Artifact Ayrımı) Saldırısı

**Bağımsızlık beyanı:** Bu inceleme, RFC-6001'i yazan `ens-philosopher` çağrısından tamamen
ayrı, taze context'te yapıldı (G2/G3 — yazar kendi önerisini onaylayamaz). Hedef bir teori
değil, **Anayasa'nın kendi değişiklik mekanizmasıdır** (Madde XV-b kapısı); bu yüzden bar
bilinçli olarak yükseltildi: öneri yalnızca kendi içinde değil, ENS'in mevcut felsefi
taahhütleri (Madde III/P8/X) ve depodaki **gerçek belgelerin fiili künyeleriyle** karşı
karşıya getirildi.

## Verdict
`wounded` — Çekirdek tez (iki dik eksen; canon'un aralıktan değil türe-uygun doğrulama
yolundan kazanılması; Madde IV yeniden yazımı) sağlam ve iyi savunulmuş; single-RFC kararı ve
Quine'a verilen mütevazı yanıt gerçekten güçlü. Ancak üç somut, giderilemedik yara var: (W1)
RFC'nin §8 sınıflaması, tam da retrofit'in gövdesini oluşturan 4000-aralığının **fiili
künyeleriyle çelişiyor** — `constitutive:true` dediği ENS-4001/4010/4025 belgeleri şu an
`maturity:M2` taşıyor, oysa RFC'nin kendi §7.3'ü bunu yasaklıyor; ve poster-çocuk ENS-4000
`canon:true` ama `status:review` (RFC'nin kendi canon-kazanma ölçütünü karşılamıyor). (W2)
`constitutive:true` **tekdüze bir sınıf değil**: kendi-kendini-yetkilendiren kök (ENS-0000),
Madde III gereği ampirik olarak **yanlışlanamaz bir çekirdek** taşır; RFC'nin "tutarlılık
yanılma-kipi" bunu dokunamaz, ama RFC sınıfı tekdüze varsayıyor. (W3) Sınıflamayı "sıradaki
adım"a ertelerken §8 zaten (çelişkili) sınıflama taahhütleri veriyor — en zor sınav (4000
aralığı: şema mı, dünya-modeli mi) gizleniyor ve editöryel takdiri sınırlayan operasyonel bir
turnusol yok (failure-condition #3'ü, canon-kaçamağı, savunmasız bırakıyor). Kapıyı bu haliyle
geçemez; talepler karşılanınca `survives` erişilebilir.

## Yenilik incelemesi (prior art gerçekten uygun mu, abartı var mı?)

RFC yeni kavram icat etmiyor, iki yerleşik ayrımı ENS künyesine bağlıyor — bu doğru duruş ve
atıflar gerçek. Ancak prior-art seçimi hem **bir yerde fazla zayıf hem iki yerde eksik**.

- **Searle (constitutive/regulative, *Speech Acts* 1969)** — uygun ve doğru kullanılmış.
  Delta ("türün üstüne dik bir doğrulama-durumu ekseni") meşru; Searle bir doğrulama rejimi
  önermez, ENS önerir. Overreach yok.
- **Kant/Quine (analitik-sentetik; *Two Dogmas* 1951)** — RFC Quine'ın keskinlik itirazını
  **kabul edip** metafizik iddiadan geri çekiliyor ("operasyonel/editöryel sınıflama"). Bu
  epistemik olarak sağlam bir hamle (SKR-001 dersi: savunulabilir küçük iddia). **Ama
  bedeli var:** Quine holizmi tam da "hangi cümlenin tanımla-doğru sayılacağı belgenin
  içeriğiyle değil, editöryel kararla belirlenir" der. RFC bunu "skeptic yeniden
  sınıflandırabilir" governance güvencesine yaslıyor — bu kavramsal bir garanti değil,
  usuli bir emniyet. Turnusol testi olmadan, failure-condition #3 (bir yazarın ampirik
  iddiayı `constitutive:true` etiketleyip skeptic + Faz-4'ten kaçırması) yalnızca skeptic'in
  uyanıklığına bağlı kalıyor (aşağıda W3).
- **Eksik-1 (kritik): Lakatos, hard core vs protective belt** (*Research Programmes*, 1970).
  Bu, RFC'nin en çok ihtiyaç duyduğu prior art ve hiç anılmamış. Lakatos'ta araştırma
  programının *hard core*'u metodolojik kararla yanlışlamadan **korunur**, *protective belt*
  ise sınanır. Bu, `constitutive:true` (korunan çekirdek) ↔ `constitutive:false` (sınanan
  kuşak) ayrımının neredeyse birebir felsefe-bilim karşılığıdır ve Madde III'ün
  değiştirilemez ilkelerini (P1-P8) "dogma" olmaktan çıkarıp *meşru hard core* olarak
  konumlar. W2'yi RFC'nin lehine çözer — ama RFC bu kaynağı kullanmadığı için W2 şu an açık.
- **Eksik-2: Kelsen *Grundnorm* / Hart *rule of recognition***. RFC ENS-0000'i "biricik
  kendi-kendini-yetkilendiren yapıt" ilan ediyor ama bunu bir *iddia* olarak bırakıyor;
  hukuk felsefesinde temel-norm/tanıma-kuralı, aksiyomatik bir sistemin sonsuz gerilemeyi
  durdurmak için **tek bir öz-yetkilendiren köke ihtiyaç duyduğunu** gösteren yerleşik prior
  art'tır. Bu atıf, Test-2'nin (dairesellik) "ad hoc istisna mı, ilkeli zorunluluk mu"
  sorusunu RFC lehine kapatır.
- **Öneri (Carnap, internal/external questions)** ham Kant'tan daha iyi oturur: Carnap'ın
  "çerçeveye-göreli analitiklik"i Quine'a Kant'tan daha dayanıklıdır ve `constitutive`'i
  "ENS çerçevesine-göreli analitik" olarak tam modeller.

**Sonuç:** Kullanılan atıflar gerçek ve doğru; uydurma yok. Ama RFC en güçlü iki müttefikini
(Lakatos, Kelsen/Hart) sahaya sürmediği için, W2'yi kendi elindeki araçla kapatabilecekken
açık bırakmış.

## Yanlışlanabilirlik — kaçamak testi (Test 1)

**Soru:** `constitutive:true` gerçekten Madde X'ten muaf değil mi, yoksa "yanılma-kipi farklı
ama muafiyet yok" bir kelime oyunu mu?

**Kısmi beraat + kısmi mahkûmiyet.** RFC'nin iddiası **şema/ontoloji belgeleri için gerçek**:
ENS-4010'un `part_of` tipleme kusuru (domain≠range, transitive zincir kuramaz) ve ENS-4025
D-1 (proof-trace örneği Registry'de lisanssız) — ROADMAP'te belgelenmiş, skeptic tarafından
yakalanmış **işleyen** tutarlılık/örneklenebilirlik yanılmalarıdır. Bir tip-şeması
"örneklenemez / kendi Registry'siyle çelişir / daha iyi bir ayrım var" ile gerçekten
çürütülebiliyor. Bu belgelerde `constitutive:true` bir muafiyet değil; RFC haklı.

**Ama en üst `constitutive:true` yapıt — Anayasa'nın kendisi — için kip çöküyor.** RFC
ENS-0000'i paradigma constitutive örneği ilan ediyor. Oysa Madde III P1-P8'i **niyet
düzeyinde değiştirilemez** kılar ("sözcükleri iyileştirilebilir, niyetleri kaldırılamaz").
Şu gözlemi düşünün: *"karar, örgütün en küçük anlamlı birimi değildir; asıl atom
transaction'dır"* (P1'in ampirik reddi). Bu gözlem P1'i **çürütmez** — Anayasa onu tanım
gereği yanlışlamaya kapatmıştır; ancak farklı bir proje ilan ederek reddedebilirsiniz. Yani
Anayasa'nın çekirdeği için RFC'nin "tutarlılık kipi" *iç-çelişkiyi* yakalayabilir ama
"P1 dünya hakkında yanlış" iddiasını **prensipte** yakalayamaz. Bu, en üst kurucu yapıt için
yanlışlanabilirlik ödevinin fiilen *coherence-düzeyinde* karşılandığı, ampirik-düzeyde ise
**karşılanamadığı** anlamına gelir.

Bu bir çürütme değil — çünkü ENS bu felsefi taahhüdü (immutable principles) zaten bilinçli
vermiş ve bunu Lakatosçu bir hard core olarak savunmak meşrudur. Ama RFC bunu **görmezden
geliyor**: `constitutive:true` sınıfını tekdüze ("kip = tutarlılık") sunuyor, oysa sınıfın
tavanı (ENS-0000/Madde III) tutarlılıkla dokunulamaz bir ampirik çekirdek içeriyor.
Sınıf **heterojen**: (a) tutarlılık-yanlışlanabilir kurucu belgeler (şemalar, ontolojiler —
kanıtlı), (b) öz-yetkilendiren kök, ampirik çekirdeği Madde III fiat'ıyla korunmuş. RFC (b)'yi
(a) gibi konuşuyor. **Talep D1/D2.**

## Dairesellik testi (Test 2)

**Soru:** Anayasa kendini `constitutive:true, canon:true` ilan ederek, Madde IV'ün düzeltmeye
çalıştığı TAM sorunu (koşulsuz canon iddiası) kendisi için yeniden mi üretiyor? İşaretlenmiş
mi, gizli mi?

**İşaretlenmiş — gizli değil.** RFC §5.2 birinci madde ENS-0000'i açıkça "biricik
kendi-kendini-yetkilendiren yapıt" diye ayırıyor. Ve öneri fiilen bir **daraltmadır**: eski
Madde IV koşulsuz canon'u *dört aralığa* (0/1/3/4) veriyordu; RFC bunu 1/3/4 için kaldırıp
(artık doğrulama yoluyla kazanılır) yalnızca **tek yapıta** (0) bırakıyor. Koşulsuz-canon
yüzeyi dört aralıktan tek köke iniyor — gerçek bir iyileştirme, ve aksiyomatik bir sistem
sonsuz gerilemeyi durdurmak için mantıken tek öz-yetkilendiren köke ihtiyaç duyar
(Münchhausen; Kelsen Grundnorm; Hart rule of recognition).

**Ama iki incelik açık:**
1. RFC bu zorunluluğu *iddia* ediyor, *gerekçelendirmiyor* — Kelsen/Hart prior art'ı yok
   (yukarıda Eksik-2). "Biricik" bir ilan olarak kalıyor; ilkeli olduğunu göstermiyor.
2. Dairesellik gerçekte **failure-mode sınırıyla** birleşiyor (W2): kök yalnızca canon'unu
   öz-ilan etmiyor, aynı zamanda çekirdek ilkelerini (Madde III) yanlışlamaya da kapatıyor.
   Yani öz-yetkilendirme + öz-koruma üst üste biniyor. RFC yalnızca birincisini (canon
   öz-ilanı) işaretliyor; ikincisini (ampirik yanlışlanamazlık) işaretlemiyor. Test-2 ve
   Test-1 aynı noktada buluşuyor: kök için hem yetki hem doğruluk öz-referanslı.

Verdict: dairesellik **fatal değil ve dürüstçe işaretlenmiş**, ama yarım işaretlenmiş — canon
tarafı görülmüş, falsifiability tarafı görülmemiş.

## Pratikle tutarlılık (Test 3) — **en güçlü yara (W1)**

RFC'nin §8 sınıflama tablosu, depodaki **gerçek künyelerle doğrudan çelişiyor** ve RFC bunu
fark etmemiş. Somut kanıt (fiili header okumaları, 2026-07-24):

| id | Fiili künye (bugün) | RFC §8 diyor | Çelişki |
|----|---------------------|--------------|---------|
| ENS-4001 | `canon:false, maturity:M2, status:review` | `constitutive:true` | **§7.3 ihlali:** constitutive:true "maturity/evidence taşımaz" der; ENS-4001 M2 taşıyor |
| ENS-4010 | `canon:false, maturity:M2, status:review` | `constitutive:true` | Aynı — M2 taşıyor |
| ENS-4025 | `canon:false, maturity:M2, status:ratified` | `constitutive:true (aday)` | Aynı — M2 taşıyor; skeptic-kazanılmış grade |
| ENS-4000 | `canon:true, status:review` (SKR yok) | `constitutive:true, canon:true` | **RFC'nin kendi canon ölçütünü karşılamıyor:** canon "ratifikasyon + skeptic tutarlılık incelemesinden sağ çıkınca" kazanılır; ENS-4000 `review`, hiç SKR geçmemiş |
| ENS-2001 | `canon:false, maturity:M3, status:ratified` | `constitutive:false` | ✅ tutarlı |

**İki bağımsız kırık:**

1. **Maturity çakışması (retrofit'in gizli maliyeti).** RFC §7.3: *"`constitutive:true`
   yapıtlar `maturity` ve `evidence` alanlarını taşımaz."* Ama constitutive:true dediği
   üç 4000-belgesi (4001/4010/4025) **şu an M2 taşıyor** ve bu grade'ler skeptic turlarıyla
   (SKR-017..023, SKR-030) kazanılmış. Kural kabul edilirse retrofit yalnızca alan *eklemek*
   değil, ratifiye ontolojilerden skeptic-kazanılmış M2'yi **söküp atmak** olur. Dahası bu,
   `maturity-model.md`'nin (ENS-4001..4025'i açıkça M-ekseninde derecelendirir) ve
   KULLIYAT.md'nin (aynı belgeleri "Normatif/Constitutive çekirdek" *ve* "ratified (M2)" diye
   listeler — ikisi bir arada) mevcut metniyle çelişir. RFC §8 bunu "nihai atama owner+skeptic"
   diye geçiştiriyor; ama çakışmanın *yönünü* (M sökülecek mi, yoksa ontolojiler aslında
   constitutive:false mı?) hiç ele almıyor.

2. **ENS-4000 canon ölçütünü karşılamıyor.** RFC canon'u "ilan edilmez, kazanılır"
   temel-sloganı üzerine kuruluyor; §5.2 constitutive:true için canon = "ratifiye edilip
   skeptic tutarlılık incelemesinden sağ çıkınca". ENS-4000 ise `status:review`, hiçbir SKR
   geçmemiş, ama `canon:true`. RFC bunu §4 dörtlü-hücrede ve §8'de "ratifiye kurucu belge"
   örneği diye kullanıyor — oysa fiilen *ratifiye değil*. Yani RFC'nin canon:true poster-çocuğu,
   RFC'nin kendi canon-kazanma kuralını **bugün ihlal ediyor**. Bu, RFC'nin "ilan değil kazanım"
   ilkesinin en görünür yerde uygulanmadığını gösteriyor.

**Neden bu ölümcül değil ama ciddi:** Sınıf çizgisi tam da 4000-aralığında bulanık ve
4000-aralığı retrofit'in gövdesi. Bir ontoloji/tip-sistemi "dünyayı doğru eklemliyor mu?"
(sentetik/ampirik adequacy) iddiasını örtük taşır — ENS-4001/4010/4020 zaten *senaryo stres
testleri* ve *3-senaryo doğrulaması* ile sınandı (ROADMAP), ki bu saf tutarlılık-kontrolünden
çok ampirik-yeterlilik testine yakın. Yani "şema mı (constitutive), dünya-modeli mi (empirical)"
sorusu bu belgeler için **gerçekten** ikircikli — RFC'nin kendi failure-condition #1'i. RFC bu
ikircikliği "sıradaki adım" diye erteleyerek en zor sınavı gözden kaçırıyor.

## Kapsam/etki analizi eksik mi? (Test 4)

Retrofit *edimlerini* (dosya düzenleme) ertelemek meşru ve doğru — atomik değişiklik
(kural+şema şimdi, uygulama sonra) tam da G-03/05'i çözen mantık. **Ama** §8 zaten sınıflama
*taahhütleri* veriyor ve W1'de gösterdiğim gibi bu taahhütler test edilebilir ve kısmen
yanlışlanmış. İkilem:
- §8 normatifse → maturity-sökme ve ENS-4000 çelişkisi konusunda eksik/yanlış.
- §8 yalnızca illüstratifse → RFC, kuralının **en zor vakaları** (4000-aralığı) tutarlı
  sınıflayabildiğini göstermemiş oldu; ki bu tam olarak failure-condition #1'in
  ("sınır vakaları *sık* çıkarsa alan kötü tanımlı") gerçekleşmesi. 4000-aralığı nadir bir
  edge-case değil; korpusun çoğu ve sınırın en bulanık olduğu yer.

Sonuç: erteleme, kuralın en kritik sınavını gizliyor. RFC en az 4000-aralığı için
"şema-tarafı (constitutive) vs ampirik-yeterlilik-tarafı (empirical)" ayrımını *göstererek*
sınıflamalı — aksi halde "prensipte doğru ama hangi belgeye nasıl uygulanacağı belirsiz"
eleştirisi haklı çıkar ve RFC operasyonel olmaz. **Talep D3.**

## Tek-RFC kararı (Test 5) — **RFC'nin en güçlü tarafı**

Sağlam. Alan (schema) ile kural (Madde IV) karşılıklı bağımlı: kuralsız alan anlamsız,
alansız kural uygulanamaz. Ayırmak, biri ratifiye biri değilken tam da G-03/05'in
tutarsızlığını yeniden üretir — RFC bunu doğru teşhis ediyor. Madde XIV bir RFC'nin
"Külliyat'ta, mimaride **ya da** standartlarda" değişiklik önermesine açıkça izin verir, yani
çoklu-katman tek RFC yetkilidir. İki farklı yetki alanı (Madde IV içerik = ens-philosopher;
metadata-header şeması = ens-style-guardian) gereksiz karışmıyor; §7 şema-tarafının eş-sahibini
`ens-style-guardian` diye açıkça anıyor. **Tek küçük eksik:** RFC kabul kapısını yalnızca
`ens-ceo` hiza incelemesine bağlıyor; şema Madde IV'ten ayrı bir owner'a (style-guardian) ait
olduğundan, kabul ediminin **her iki owner onayını** da (ens-ceo hiza + ens-style-guardian
şema-imzası) gerektirdiğini açıkça yazmalı. Aksi halde tek-RFC'nin erdemi (senkron) kabul
aşamasında tek-imzayla yeniden riske girer. **Talep D4 (küçük).**

## İç tutarlılık (Anayasa'nın diğer maddeleriyle)

- **P8 / Madde III ile:** RFC P8'i gerçekten **güçlendiriyor** — ampirik canon (M5) Faz-4
  kanıtı ister, kurucu canon istemez çünkü ampirik iddia yapmaz. "Her canon Faz-4 ister"
  aşırı-genellemesini önlemesi doğru ve değerli. Çelişki yok.
- **Madde X ile:** RFC "constitutive muafiyet değildir, failure conditions her iki türde
  zorunlu" diyerek Madde X'i metin düzeyinde koruyor. **Ama** W2'de gösterildiği gibi, en üst
  kurucu yapıt için bu koruma coherence-düzeyinde kalıyor; RFC bunu keskinleştirmezse "Madde X
  keskinleşti" iddiası, sınıfın tavanı için fazla iyimser.
- **Madde IX (kavram önce Külliyat'ta resmîleşir) ile:** RFC bu ruhu **destekliyor** —
  `constitutive` fiilen ENS-4000 v0.2/KULLIYAT.md'de yaşıyor ama üst-kaynak (Anayasa) ve
  makine-okunur şema onu tanımıyordu; RFC o borcu (gap #1) kapatıyor. Tutarlı.
- **Terminoloji sürüklenmesi (Madde VI):** RFC `constitutive` terimini KULLIYAT.md'nin
  "Normatif/Constitutive çekirdek" kullanımıyla hizalı tutuyor. Yeni bir drift açmıyor.

## Sahibine talepler (kapıyı geçmek için — D1/D2/D3 blocking, D4/D5 keskinleştirme)

1. **D1 (blocking) — Maturity çakışmasını çöz.** §7.3 (`constitutive:true` → maturity/evidence
   taşımaz) ile fiili ENS-4001/4010/4025 künyeleri (`maturity:M2`) arasındaki çelişkiyi açıkça
   ele al: ya (a) bu ontolojiler aslında `constitutive:false`/karma ve M-ekseninde kalır —
   ki o zaman §8 tablosu yanlış; ya (b) constitutive'dirler ve M2 sökülür — ki o zaman
   `maturity-model.md` + KULLIYAT.md eş-zamanlı düzeltme yükü ve *skeptic-kazanılmış grade'in
   nasıl korunacağı* RFC kapsamında belirtilmeli. Sessiz geçilemez.
2. **D2 (blocking) — `constitutive:true` sınıfının heterojenliğini kabul et.** Kendi-kendini
   -yetkilendiren kökün (ENS-0000/Madde III) ampirik çekirdeğinin, "tutarlılık yanılma-kipi"
   ile dokunulamaz olduğunu açıkça yaz; Lakatosçu *hard core vs protective belt* prior art'ıyla
   bunu dogma değil meşru korunmuş-çekirdek olarak konumla. "Kip = tutarlılık" tekdüze cümlesini,
   "kök hariç" istisnasıyla nitele.
3. **D3 (blocking) — 4000-aralığı için sınıflamayı göster, erteleme.** En az ENS-4000/4001/4010/
   4025 için "bu belge şema-tarafı (constitutive) mı, ampirik-yeterlilik-tarafı (empirical) mı"
   ayrımını *işleyen bir turnusol testiyle* gerçekleştir (ör. "bu belge saha verisiyle
   çürütülebilir bir öngörü mü taşıyor? → empirical"). Bu, failure-condition #1 ve #3'ü
   (kaçamak) editöryel takdirin ötesinde savunur ve §8'in test-edilebilir olduğunu gösterir.
4. **D4 (keskinleştirme) — Kabul kapısını çift-owner'a bağla.** Madde IV (ens-ceo hiza) + şema
   (ens-style-guardian imzası) her ikisini de zorunlu kıl; tek-RFC senkron erdemini kabul
   aşamasında koru.
5. **D5 (keskinleştirme) — Prior art'ı güçlendir.** Kelsen *Grundnorm* / Hart *rule of
   recognition* (öz-yetkilendiren kökün ilkeli zorunluluğu, Test 2) ve Carnap
   internal/external questions (Quine'a Kant'tan dayanıklı çerçeve-göreli analitiklik) ekle.

## Kapanış

RFC-6001 gerçek bir borcu (G-03/05: Madde IV'ün "aralık=canon" cümlesi ile depo pratiğinin
çelişkisi) doğru teşhis ediyor ve doğru yapıda (iki dik eksen, tek atomik RFC) çözüyor;
çekirdek tez saldırıdan sağ çıkıyor. Yara, tezde değil **öneri ile deponun bugünkü gerçek
durumu arasındaki hizasızlıkta**: RFC kendi kuralını, retrofit'in gövdesini oluşturan
4000-aralığının fiili künyeleriyle test etseydi, `maturity:M2` çakışmasını ve ENS-4000'in
canon-ölçütü ihlalini kendisi görürdü. Bu üç blocking talep karşılanınca — ve sınıfın
heterojenliği (kök hard-core) dürüstçe kabul edilince — öneri `survives` olabilir. Faz durmaz;
öneri düzeltilerek ilerler (Madde X: eksik olan tamamlanır, reddedilmez).
