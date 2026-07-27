# ARCH-0001 — RFC-6002 & RFC-6003 Yapısal İnceleme

| Alan | Değer |
|---|---|
| ID | ARCH-0001 |
| Tür | Mimari/yapısal inceleme (validator boyutu: **engineering/architecture**) |
| İnceleyen | `ens-architect` |
| Konu | RFC-6002, RFC-6003 |
| Tarih | 2026-07-27 |
| Gerekçe | GOV-000 G4 — "farklı boyutlardan ≥2 bağımsız validator" şartının fiilî uygulaması |
| Bağımsızlık | Paralel `ens-skeptic` turu (SKR-047/048) görülmeden yazıldı; SCAN-03'ten **bağımsız değil** (§0) |
| Verdict | **RFC-6002: `yapısal kusurlu`** · **RFC-6003: `yapısal olarak sağlam`** (§11) |
| Durum | final |

## 0. Yetki sınırı

**Önerebileceklerim:** katman konumu, bağımlılık yönü, RFC'lerin bölünme sınırı, kabul sırası,
faz-kapısı etkisi, ölçüm aracı mimarisi (`tools/`), ADR gerektiren her şey.

**Öneremeyeceklerim — bunlar başka owner'ın alanıdır:** GOV-000/GOV-010/GOV-030 metninin
değişmesi (`ens-philosopher` + Madde XIV), rol ataması (GOV-010), künye şeması
(`ens-style-guardian`), Anayasa metni (Madde XV/RFC). Aşağıdaki hiçbir talep bu incelemeyle
yürürlüğe girmez.

**Bağımsızlık beyanı ve kirlilik uyarısı.** Bu inceleme `ens-skeptic`in paralel turundan
(SKR-047/048) bağımsızdır — o kayıtlar okunmadı. **Ama SCAN-03'ten bağımsız DEĞİLDİR:**
`governance/SCAN-03-gate-compliance.md:6` owner'ı `ens-architect`tir, yani bu incelemeyi yapan
rolün önceki işidir. RFC-6002/6003 SCAN-03'e cevap yazdığına göre, burada **kısmi bir G2
durumu** vardır: kendi raporumun cevabını değerlendiriyorum. Bunu gizlemek yerine yöntemle
sınırlandırdım — SCAN-03'ün *sonuçlarını* değil, RFC'lerin **birincil kaynaklara** karşı
doğruluğunu denetledim ve SCAN-03'ün kendi hatalarını da (§1, D-1.3) rapor ettim.

## 1. Alıntı doğrulama (`work-protocol.md` §3.5)

RFC-6002/6003, §3.5'in kendisini doğuran doğrulama turundan çıktı. O hâlde ilk sınav
kendileridir. **11 atıfın 8'i tam doğru, 2'si sadakat kusurlu, 1'i kaynağını yanlış
aktarıyor.**

| # | Atıf | Sonuç |
|---|---|---|
| 1 | RFC-6002:27 → `maturity-model.md:28` (M2 = ≥1 SKR → `ratified`) | **DOĞRU** — satır 28, birebir |
| 2 | RFC-6002:44 → `maturity-model.md:34` (*"`canon: true` yalnızca M5'tir"*) | **DOĞRU** — satır 34, birebir. RFC'nin failure condition 1'i karşılanıyor |
| 3 | RFC-6002:28 → `000-governance-principles.md:36` (G4) | **DOĞRU** — satır 36. Not: SCAN-03:42 aynı satırı `:35` diye verir; **SCAN-03 bir satır kaymıştır, RFC düzeltmiştir** |
| 4 | RFC-6002:29 → `canonical-process.md:45` (G4 kuralı) | **KISMEN** — satır doğru, ama alıntıdan **"ör."** düşürülmüş (bkz. D-1.1) |
| 5 | RFC-6002:74 → `GOV-000:22` (*"…bu ilkelerden türer"*) | **DOĞRU** — satır 22, birebir |
| 6 | RFC-6002:103 → `maturity-model.md:31` (M5 = Sci+Eng+Biz+Ethical, Ontology yok) | **DOĞRU** |
| 7 | RFC-6002:183 / RFC-6003:70 → `RFC-6001:175` (*"muaf tutmaz"*) | **DOĞRU** — satır 175, birebir |
| 8 | RFC-6003:52 → `validation-framework.md` (ethical = *"tüm fazlar"*) | **DOĞRU** ama **eksik** — aynı dosya `:51` tersini söyler (D-1.2) |
| 9 | RFC-6003:34 → `roles.md` (*"…Governance body: fazı gelince"*) | **DOĞRU** (satır 63; RFC satır vermemiş, SCAN-03'ün verdiği `:49` **bayat**) |
| 10 | RFC-6003:29 → 45/45 SKR `ens-skeptic` | **DOĞRU** — SCAN-03 §5.1'de sayıldı; bu incelemede yeniden sayılmadı, **DEVRALINDI** |
| 11 | RFC-6002:31-35 → SCAN-03'ün "ürettiği sonuç" | **YANLIŞ AKTARIM** (D-1.3) |

### D-1.1 — `canonical-process.md:45`'ten "ör." düşürülmüş (RFC'nin **kendi aleyhine**)

Gerçek metin (`governance/canonical-process.md:45`):
> `2. **G4:** ≥2 bağımsız boyut validator'ı (**ör.** Scientific + Ontology, uygunsa + Engineering).`

RFC-6002:29 bunu `(Scientific + Ontology, uygunsa + Engineering)` diye aktarıyor. "ör."
düşünce parantez bir **örnek** olmaktan çıkıp **liste** hâline geliyor. Bu, RFC-6002 §4'ün
kurgusunu doğrudan etkiler: §4 *"İki liste var"* der ve ikisini birleştirir. Oysa
`canonical-process.md` bir liste **iddia etmemiştir** — örnek vermiştir.

**Yönü önemli:** bu kusur RFC'nin lehine değil **aleyhine** işliyor. "ör." yerinde dursaydı
§4'ün "sabit sayı yerine tüm aktif boyutlar" önerisi **daha kolay** savunulurdu, çünkü tek
gerçek sabit-liste `maturity-model.md:31` olurdu. Yani sadakat kusuru sonucu değiştirmiyor,
argümanı gereksiz yere zayıflatıyor. Düzeltilmeli; bulguyu bloke edici saymıyorum.

### D-1.2 — RFC-6003 §2'nin Ç-04 çözümü **yanlış yapıya** dayanıyor (yüksek şiddet)

RFC-6003:51-57 çatışmayı iki **belge** arasına koyar (`validation-framework.md` "tüm fazlar"
↔ `roles.md` "fazı gelince") ve yordamsal bir kuralla çözer: *"`roles.md` boyut otoritesi
olarak onu gösterir; bir belge, atıf yaptığı kaynağı geçersizleştiremez."*

**Bu çözüm uygulanamaz, çünkü çatışma iki belge arasında değil, TEK belgenin içindedir:**

| Satır | Metin |
|---|---|
| `validation-framework.md:29` | `└── Ethical Validation (AI çağında zorunlu) — **tüm fazlar**` |
| `validation-framework.md:51` | `Engineering/Business/Ethical Validator — **fazı gelince (ROSTER)**` |

`roles.md:63` bu ikinci cümlenin **sadık türevidir**, kaynağını geçersizleştirmiyor —
kaynağının kendi ikinci cümlesini tekrarlıyor. RFC-6003'ün yordamsal argümanı böylece
**boşa düşer**: `roles.md`'yi hizaya çekmek `validation-framework.md:51`'i olduğu gibi
bırakır ve çelişki kaynakta yaşamaya devam eder.

**Sonuç değişmiyor, gerekçe değişiyor.** Gerçek ayrım şudur ve zaten RFC-6003'ün kendi
tezidir (`:33-35`): `:29` **boyutun aktifliğini**, `:51` **rolün atanmışlığını** söyler.
İkisi farklı yüklemdir; çelişmezler. Doğru cümle: *"Ethical boyut tüm fazlarda aktiftir
(`:29`); validator rolü atanmamıştır (`:51`). Aktif ama atanmamış boyut = **kayıtlı borç**"*
— tam olarak RFC-6003'ün vardığı sonuç. Yani **verdict doğru, ispat yanlış.**
`validation-framework.md:51` her iki RFC'de de hiç anılmıyor; §6'daki "değişecek metinler"
tablosuna da girmemiş. Düzeltilmeden kabul edilirse, kabul edilen norm ile onu tanımlayan
belgenin 51. satırı çelişik kalır.

### D-1.3 — RFC-6002 §1, SCAN-03'ün ne söylediğini yanlış aktarıyor

RFC-6002:31-35 der ki: SCAN-03 *"şu sonucu üretti: 9 `ratified` yapıtın yalnız 6'sı G4'ü
sayıca, hiçbiri boyut olarak sağlıyor. **Bu sonuç yanlıştı** — ve yanlışlığı bu RFC'nin
çekirdeğidir."*

SCAN-03 böyle bir sonuç **üretmedi**. Üç yerde tersini yazıyor:

- `SCAN-03:47-50` — *"**Kritik okuma (kapsam belirsizliği — B-01).** G2/G4 metinleri
  "Canonical" der… Dolayısıyla **harfiyen okumada G4, `ratified` (M2/M3) statüsü için
  hiçbir şey söylemez; yalnızca M5 kapısını bağlar.**"* → **RFC-6002 §2'nin tezinin
  kendisi, kaynakta zaten yazılıdır.**
- `SCAN-03:55` — *"Bu tarama **iki okumayı da ayrı ayrı raporlar**."*
- `SCAN-03:116` — *"Bu bir *belge* uyuşmazlığıdır; **kimin haklı olduğu bu raporun kararı
  değildir** — RFC ile çözülmelidir."*

Aynı yanlış aktarım `.claude/rules/work-protocol.md:86`'da da var (*"Rapor `ratified` ile
`Canonical`'ı aynı saymıştı"*) — yani kural katmanına da geçmiş.

**Etkisi iki yönlü ve ikisi de kayda değer:**
1. **Yenilik iddiası zayıflar.** RFC-6002 §2.1 *"iki bağımsız türetme"* sayar. Gerçekte
   **üçüncü** ve **en eski** ifade kaynağın kendisindedir. Bu, tezi güçlendirir (üç bağımsız
   context aynı okumaya vardı) ama RFC'nin *"bu sonuç yanlıştı"* çerçevesini geçersiz kılar.
2. **§3.5 sınavı.** RFC-6002, üzerine yapı kurduğu raporu doğrulamadan aktarmıştır —
   `work-protocol.md:76-77`'nin 3. kontrolünün (*"Alıntılanan metin, çıkarılan sonucu gerçekten
   taşıyor mu?"*) ihlali. Kuralın doğduğu turdan çıkan belgenin aynı kuralda takılması ağırdır
   ve düzeltilmelidir; ancak **RFC'nin normatif önerisini etkilemez**, yalnızca §1'in
   anlatısını.

## 2. Bağımlılık yönü ve katman ihlali (Madde XII)

### D-2.1 — RFC bir *öneri aracı*dır; katman atlamaz. **KATILIYORUM**

RFC-6002:18-19'un açılış notu (*"Öneri edimidir, norm değildir… kabul edilirse normu … yazar;
kendisi tarihsel kayda döner"*) yapısal olarak **doğrudur** ve bu incelemenin ilk sorusunu
kapatır. Madde XIV `ENS-0000:239` RFC'yi açıkça *"Külliyat'ta, mimaride **ya da standartlarda**
bir değişiklik önerir"* diye tanımlar. Yani `6000-rfc/` içinde durup `governance/` ve
`.claude/standards/`'ı hedeflemek katman ihlali **değildir** — RFC-6001 emsali (`amends:
[ENS-0000 §IV, STD-METADATA-HEADER]`, `RFC-6001:14`) bunu zaten kurmuştur.

**Ama iki türev sorun var, ikisi de gerçek.**

### D-2.2 — `governance/` Madde XII grafiğinde YOK; RFC-6002 §3 çözülemez bir üstünlük iddiası kuruyor

Madde XII (`ENS-0000:209-221`) yetki sırasını sayar:
`Anayasa → Külliyat (1000/2000/3000/4000) → Standards (.claude/standards) → Commands →
Agents → Implementation`; paralel dal `Külliyat → Mimari (5000/6000) → Implementation`.

**`governance/` bu grafikte hiçbir yerde geçmiyor.** Grep ile doğrulandı: Madde XII bloğunda
`governance` sözcüğü yok. Yani GOV-000/010/020/030 — RFC-6002'nin değiştirmeyi önerdiği
metinlerin üçte ikisi — **yetki sırasında konumsuzdur.**

Bunun somut sonucu RFC-6002 §3'te patlıyor. §3 iki üstünlük hükmü kuruyor:

| Hüküm | Dayanak | Değerlendirme |
|---|---|---|
| GOV-000 > `roles.md` | `GOV-000:22` (*"türer"*) + `roles.md:7` (`origin: GOV-000`) | **GEÇERLİ** — iç-governance türetme, dosyaların kendi künyesi kanıtlıyor |
| GOV-000 / `maturity-model.md` arasında? | **yok** | **BOŞLUK** — Madde XII cevap veremez |

İkincisi RFC-6002 için hayatidir: §2'nin tüm tezi `.claude/standards/maturity-model.md:34`'e
dayanır ve §6 aynı anda GOV-000'i (constitutive) o Standards satırına göre değiştirmeyi önerir.
Madde XII'ye göre **Standards, Külliyat'ın altındadır ve tüketir, tanımlamaz.** Bir Standards
satırının constitutive bir governance ilkesinin kapsamını belirlemesi, grafikte **ters yöndür**.

**Yumuşatıcı:** GOV-000'in kendisi bu satırı zaten benimsemiş durumda (`GOV-000:47` —
*"Engineering Validation Faz 4'ü gerektirdiğinden M5 şu an ulaşılamaz — Canon boş"* — bu cümle
`maturity-model.md`'nin M4/M5 kuralının yeniden ifadesidir). Yani pratikte iki belge aynı
şeyi söylüyor; ihlal **yön** ihlalidir, **içerik** ihlali değil. Bu yüzden bunu bloke edici
değil, **kapatılması gereken yapısal boşluk** sayıyorum.

**Talep A (mimari, benim önerebileceğim sınırda):** RFC-6002 kabul edilmeden önce
`governance/`'ın Madde XII grafiğindeki yeri yazılsın. En tutarlı konum **Anayasa ile Külliyat
arasında değil, Külliyat ile Standards arasında** değildir — GOV-000 `constitutive: true` ve
`origin: ENS-0000, ENS-4001` (`:6-7`) taşıdığına göre doğru konum **Külliyat ile aynı
düzlemde, Standards'ın üstünde**dir. Bu bir Anayasa değişikliğidir → **Madde XV / ayrı RFC**.
Ben öneremem, yalnızca boşluğu kaydedebilirim.

### D-2.3 — RFC-6002'nin çekirdek dayanağı, Anayasa tarafından **zaten aşılmış** (BLOKE EDİCİ)

Bu, bu incelemenin en ağır bulgusudur ve `ens-skeptic` lensinden görünmeyebilir çünkü
yapısal bir **öncelik** sorunudur, bilimsel bir geçerlilik sorunu değil.

RFC-6002 §2'nin tamamı tek cümleye dayanır — `maturity-model.md:34`:
> *"**`canon: true` yalnızca M5'tir.** Skeptic-survives (M2/M3) Canon yapmaz."*

Ama Anayasa Madde IV, **RFC-6001 ile değiştirilmiş hâliyle** (`ENS-0000:107-112`) şunu der:

> *"**`constitutive: true` yapıtlar** (Anayasa; felsefenin kurucu tezleri; yasa-çerçevesi;
> ontolojinin tip/şema belgeleri; **governance kuralları**…) `canon: true` olmak için
> **ratifikasyon** yolunu izler: … `ens-skeptic` bu tutarlılık incelemesinden sağ çıkar.
> **Ampirik kanıt zincirine (M5 / Faz-4) tabi değildir**, çünkü ampirik iddia taşımaz."*

Yani Anayasa **`canon: true`'ya M5'ten geçmeyen ikinci bir yol açmıştır.** RFC-6001
`status: accepted` (`RFC-6001:10`) ve Madde IV fiilen düzenlenmiştir (`ENS-0000:106` —
*"Bu Madde'nin (RFC-6001 ile) değiştirilmiş olması…"*). RFC-6001:169 örneği de açıktır:
*"`constitutive: true, canon: true` — ratifiye kurucu belge (ör. **ENS-0000, ENS-4000**)."*

**Sonuç: `maturity-model.md:34` bugün Anayasa'ya aykırıdır.** Madde XII'ye göre Anayasa en
üsttedir; Standards ondan türer. RFC-6002 §2, üstün norm tarafından aşılmış bir alt-norm
satırını **kendi çekirdeği** yapmıştır.

**Bunun RFC-6002 üzerindeki dört somut etkisi:**

1. **§2'nin ana denklemi eksiktir.** `≥2 bağımsız boyut → canon: true (M5)` yerine doğrusu:
   `constitutive: false` için `→ M5`; `constitutive: true` için `→ ratifikasyon yolu`.
   G4'ün öznesi *"her Canonical yapıt"* olduğuna göre G4 **her iki yolu da** bağlar — ama
   RFC-6002 yalnızca birini yönetiyor. Kurucu yolun G4 ile ilişkisi RFC-6003 §3'e bırakılmış
   (Ç-05); bu, §3'teki bölme sorununu doğuruyor (bkz. §3).
2. **Failure condition 1 tersine dönmüştür.** RFC-6002:158-159 der ki: *"`maturity-model.md:34`
   … başka bir şey söylüyorsa … RFC çöker."* Satır aynen duruyor — **ama üstündeki norm başka
   bir şey söylüyor.** FC-1 yanlış yere bakıyor: bir Standards satırının varlığını sınıyor,
   üstünlüğünü değil.
3. **Failure condition 2 kısmen değil, TAMAMEN tetiklenmiştir.** RFC-6002:160-164 der ki:
   *"Külliyat'ta `canon: true` olup M5 olmayan bir yapıt **meşru** sayılıyorsa … ayrım tanımsal
   değil, temenni olur."* Bugün `canon: true` üç yapıt var (grep, 2026-07-27):
   `ENS-0000:5`, `ENS-1000:5`, `ENS-4000:5` — **üçü de M5 değil, üçü de Faz 4'e girmemiş.**
   RFC-6002 bunları "kayıtlı borç" (ROADMAP G-24/G-25) sayıyor. Ama ENS-0000 borç değil, Madde
   IV `:98-106` uyarınca **ilkeli Grundnorm istisnasıdır**; ENS-4000 ise `RFC-6001:169`'da
   **meşru örnek** olarak gösterilmiştir. Yani "meşru sayılan" vaka **vardır** ve kaynağı bir
   `accepted` RFC'dir. RFC-6002 FC-2'yi *"kısmen gerçekleşmiş"* diye niteliyor (`:164`); doğru
   niteleme **"gerçekleşmiş"**tir.
4. **ROADMAP G-24'ün gerekçesiyle çelişki.** ROADMAP:238 ENS-3000'in canon'unu düşürürken
   çıkış yolunu **RFC-6001 §7.2 ratifikasyon yolu** olarak gösteriyor — M5 olarak değil.
   Depo, RFC-6002'nin reddettiği iki-yollu modeli **zaten uyguluyor.**

**Talep B (bloke edici).** RFC-6002 §2, `maturity-model.md:34` yerine **Madde IV `:107-116`**
üzerine yeniden kurulmalıdır. Doğru formülasyon iki yollu olmalı:
> `ratified` bir `status`'tür (M2-M5 ortak etiketi). `canon: true` ise **türe uygun** yoldan
> kazanılır: `constitutive: false` → M5 (Faz-4 kanıt zinciri); `constitutive: true` →
> ratifikasyon (tutarlılık incelemesi, Madde IV `:109-112`). **G4 her iki yolu da bağlar.**

Ve §6'nın "değişecek metinler" tablosuna **`maturity-model.md:34`'ün Madde IV ile hizalanması**
eklenmelidir — ki bu, `SKR-036`'nın S2 keskinleştirmesinin (ROADMAP:228: *"`amends:` alanına
STD-MATURITY-MODEL ekle — canon kuralının üçüncü lokusu `maturity-model.md` satır 34"*)
kapatılmamış hâlidir. **Zaten kayıtlı, hâlâ açık bir borç RFC-6002'nin temeli yapılmıştır.**

## 3. Atomiklik testi — bölme yapısal olarak tutarlı mı?

**Somut test (görevin sorduğu): RFC-6002 kabul + RFC-6003 red → tutarlı bir sistem çıkar mı?**

**Hayır. Çıkan sistem, bugünkünden DAHA GEVŞEKTİR.** Bu bloke edicidir.

### D-3.1 — Mekanizma: "tüm aktif boyutlar" bugün **tek aktöre** çözülüyor

RFC-6002:111-114'ün önerisi: M5 boyut listesi sabit sayı yerine *"tüm aktif boyutlar"*;
*"bir boyut 'aktif'tir ancak ve ancak ROSTER'da o boyutun validator rolü **atanmışsa**."*

Bugünkü rol ataması (`governance/roles.md:59-63`, tek yetkili eşleme):

| Boyut | Atanmış mı | Kime |
|---|---|---|
| Scientific | **evet** | `ens-skeptic` (`:61`) |
| **Ontology** | **evet** | **`ens-skeptic`** (`:61` — aynı satır, aynı ajan) |
| Engineering | hayır | *"fazı gelince"* (`:63`) |
| Business | hayır | *"fazı gelince"* (`:63`) |
| Ethical | hayır | *"fazı gelince"* (`:63`) |

Kuralı mekanik olarak çalıştıralım: **aktif boyut kümesi = {Scientific, Ontology}**. Yani
M5'in boyut şartı, `ens-skeptic`in **iki farklı etiketli SKR yazmasıyla** karşılanır.

Bu, G4'ün lafzını (*"≥2 bağımsız validator, farklı boyutlardan"*) kâğıt üzerinde sağlar ve
amacını (**farklı kör nokta kümesi**) tamamen boşaltır. Üstelik boşaltmanın maliyeti bu
depoda **ölçülmüştür** — RFC-6003:39-46'nın kendi kanıtı: ENS-2003 D-5 çift-sayımı iki
bağımsız *aynı-lens* turdan geçti.

### D-3.2 — Regresyonun ölçüsü: %0 → geriye dönük **%40**

SCAN-03 §5.3 ölçtü: boyut çeşitliliği sağlayan yapıt **0/15**. RFC-6002 tek başına kabul
edilirse, bu ölçüt tanım değiştirir ve **hiçbir yeni doğrulama yapılmadan** şu yapıtlar
"aktif boyutların tamamından geçmiş" sayılabilir hâle gelir — çünkü zincirlerinde zaten
ontology turu var ve eksik olan tek şey bir scientific tur:

`ENS-4001` (SKR-017→018, 023), `ENS-4010` (SKR-019→020, 038→039), `ENS-4020` (SKR-028, 030),
`ENS-4025` (SKR-022), `ENS-4030` (SKR-021), `ENS-4031` (SKR-031→032) — **6/15 (%40)**, hepsi
tek bir ajanın kaleminden.

**Bir kural, ölçtüğü uyumu doğrulama yapmadan %0'dan %40'a çıkarıyorsa, o kural kapıyı
sıkılaştırmıyor — kapıyı yeniden tanımlıyor.** RFC-6002'nin beyan ettiği amaç (`:114`:
*"bir boyutun atanmamış olması sessizce muafiyete dönüşmez"*) sağlanıyor; **ama beyan
edilmeyen ters etki** — atanmış boyutların tek aktöre yığılmasının sessizce **yeterlilik**
üretmesi — hiç ele alınmamış.

### D-3.3 — Yamanın **tamamı** RFC-6003'ün içinde

Bu kaçağı kapatan tek metin RFC-6003 §4 (Ö-07): *"ROSTER'a ayrı bir **Ontology Validator**
rolü"* (`:91-98`). RFC-6002'nin kendi failure condition'ları bu vakayı **kapsamıyor**:
FC-4 (`:168-170`) yalnızca *"hiçbir boyutun atanmadığı"* uç durumu koruyor — bugünkü durum
ise *"iki boyut atanmış, ikisi de aynı aktöre"*. Bu, FC'nin **kör noktasıdır**.

Dolayısıyla RFC-6002 §5'in atomiklik iddiası (`:116-128`: *"Ç-04 ve Ç-05 … farklı owner ve
farklı yanılma kipi taşırlar, RFC-6003'e bırakıldılar"*) **yanlış yerden kesmiştir.**
Doğru kesim çizgisi "kapı nerede / kapıyı kim açar" değildir; çünkü RFC-6002'nin kapı
**tanımı**, RFC-6003'ün kadro **ataması** olmadan tanımsızdır. Bölme, tek yönlü bir
bağımlılık gibi ilan edilmiş (RFC-6003 `depends_on: […, RFC-6002]`, `:9`), gerçekte
**çift yönlüdür**.

### D-3.4 — Ters yön: RFC-6003 kabul + RFC-6002 red

Bu yön **daha az zararlıdır ama boştur.** RFC-6003'ün üç çıktısı:
(a) Ç-04 = ethical borç kaydı — RFC-6002'siz de geçerli;
(b) Ç-05 = kurucu yol G4'ten muaf değil — RFC-6002'siz de geçerli;
(c) Ö-07 = Ontology Validator ayrımı — RFC-6002'siz de uygulanabilir ve **tek başına**
SCAN-03 D-03'ü gerçekten iyileştirir.
Eksik kalan tek şey *"tüm aktif boyutlar"* teriminin yuvası. Yani **RFC-6003 önce ya da tek
başına kabul edilebilir; RFC-6002 edilemez.** Bu, ilan edilen bağımlılık yönünün tersidir.

### D-3.5 — RFC-6004 zinciri: bağımlılık **beyan edilmemiş**

RFC-6004 (`6000-rfc/RFC-6004-madde-v-guvenlik-niteligi.md`) gövdesinde açıkça yazıyor
(`:223-226`):
> *"G4'ün 'farklı boyutlardan' şartını uygulayacak kadro henüz atanmamıştır; bu tam olarak
> **RFC-6003'ün** konusudur… RFC-6004, RFC-6003 kabul edilmeden **tam** yoldan geçemez."*

Ama künyesi (`RFC-6004:9`) `depends_on: [ENS-0000, GOV-000, RFC-6001]` — **RFC-6003 yok.**

Bu bir künye kusurundan fazlasıdır. `traceability.md` ve `formal-checker`/linter hattı
bağımlılığı **`depends_on` grafiğinden** okur; gövdedeki düzyazı hiçbir mekanik denetime
girmez. Sonuç: üç RFC'lik gerçek zincir
**RFC-6003 → RFC-6002 → (paralel) RFC-6004**
makine tarafından görünmez; grafik yalnızca `RFC-6002 → RFC-6003` tek yönlü okunur — yani
**gerçeğin tersi**.

Zincirin doğru sırası, D-3.3 ve D-3.4'ten çıkar:

```
RFC-6003 (kadro + boyut sözlüğü)   ← ÖNCE ya da EŞ ZAMANLI
        ↓  "aktif boyut" kümesini tanımlar
RFC-6002 (kapı kapsamı + R1-R3)    ← RFC-6003 olmadan gevşetici
        ↓  G4'ü işler hâle getirir
RFC-6004 (Madde V nitelikleri)     ← tam yolu her ikisine borçlu
```

**Talep C (bloke edici, iki seçenekten biri).**
- **C-1:** RFC-6002 ve RFC-6003 **tek pakette** kabul edilir (RFC-6001'in Madde IV + künye
  şeması atomikliği emsali) — bu durumda RFC-6002 §5'in ayrım gerekçesi geri çekilir; **veya**
- **C-2:** ayrı kalırlar, ama RFC-6002'ye **bağımsızlık tabanı** eklenir:
  > *"Aktif boyut kümesi, farklı **aktörlere** atanmış en az iki boyut içermedikçe M5 boyut
  > şartı sağlanmış sayılmaz. İki boyutun aynı aktöre atanmış olması, o kümeyi tek boyut
  > sayar."*

  Bu cümle kaçağı RFC-6003'e ihtiyaç duymadan kapatır ve RFC-6002'yi tek başına kabul
  edilebilir kılar.

Her iki hâlde **RFC-6004'ün `depends_on`'ına `RFC-6003` eklenmelidir** (Talep D, düşük
maliyet, künye edimi — owner `ens-style-guardian` alanına girer).

## 4. Uygulanabilirlik — R2 tek-operatörlü depoda çalışır mı?

RFC-6002:93: **R2.** *"Yazardan **farklı** bir **aktörün**, **kayda geçen** ratifikasyon edimi."*

### D-4.1 — "Aktör" yanlış ilkel (primitive); FC-3 **bugün tetikleniyor**

RFC-6002 FC-3 (`:165-167`): *"R2'yi … uygulayacak aktör hiç atanmazsa … kâğıt üzerinde
kapatılmış bir boşluk gerçekte açık kalır — ve bu, boşluğu kayıtlı hâlinden daha kötüdür."*

Durum tespiti:

| Kanıt | Sonuç |
|---|---|
| `ROADMAP.md:235` — `G-16 \| Governance tek-operatör (rol ayrımı G2/G3 fiilen zayıf) \| P3` | Depo kendi tek-operatörlüğünü kayda geçirmiş |
| `governance/roles.md:63` — *"Governance body: fazı gelince"* | Onay makamı **atanmamış** |
| RFC-6002:174-176 — R2'yi yürütecek aday: `ens-ceo` | Önerilen aktör |
| `.claude/agents/` glob (33 dosya) — **`ens-ceo.md` YOK** | Önerilen aktör **dosya olarak mevcut değil** |
| `.claude/agents/ROSTER.md:35` — `ens-ceo \| tüm \| … ` **"Ertelenmiş"** tablosunda | ROSTER da atanmamış sayıyor |

**Yani R2, önerdiği yürütücü var olmadan yazılmıştır → FC-3 doğuşta tetiklenir.** RFC bunu
"açık soru" (`:174-176`) diye işaretliyor; yapısal olarak bu bir açık soru değil, **kabul
engelidir**: R1 ve R3 mekanik olarak uygulanabilir, R2 uygulanamaz. Üçü *"kapı"* diye birlikte
sunulduğu için, uygulanamayan bir bileşen tüm kapıyı beyan-düzeyinde bırakır.

### D-4.2 — Ama depo bu sorunu **zaten çözmüş**; RFC yanlış ilkeli seçmiş

Kritik gözlem: `ens-ceo` bir **ajan dosyası** olarak yoktur, ama **üç CEO kaydı vardır** ve
üçü de fiilen kapı işlevi görmüştür:

| Kayıt | Neyi onayladı | Kanıt |
|---|---|---|
| `CEO-0001` | ADR-0001 | `SCAN-03:193` |
| `CEO-0003` | ADR-0002 | `SCAN-03:194` |
| `CEO-0002` + `STYLE-SIGNOFF-RFC-6001` | RFC-6001 | `RFC-6001:17-18` (künye alanları `ceo_review`, `style_signoff`) |

SCAN-03 §3.4 ölçtü: **onaylayanı kayıtlı Faz-3 yapıtı 3/3 (%100)**; Faz 0-2 Külliyat yapıtı
**0/9 (%0)**. Aynı tek operatör, aynı hafta, aynı depo. Fark **aktör** değildir — aktör
değişmedi. Fark şudur:

> **Ayrı bir rol-lensinden, ayrı bir context'te, ayrı bir dosyaya yazılmış, künyeden
> geri-bağlanan bir edim.**

Depo, tek operatörle **edim ayrımı** üretmeyi başarmıştır; **aktör ayrımı** üretemez ve
üretmesi de gerekmez. RFC-6002 R2'yi *"farklı bir aktör"* diye yazarak, deponun kanıtlanmış
çözümünü kendi metninin dışına atıyor.

### D-4.3 — Alternatif mimari (görevin sorduğu): **edim + lens + context + iz**

R2, aktör kimliği yerine **dört gözlemlenebilir** üzerine kurulabilir. Dördü de bugün
mekanik olarak denetlenebilir:

| # | Koşul | Neden gözlemlenebilir | Emsal |
|---|---|---|---|
| 1 | **Ayrı kayıt dosyası** — ratifikasyon edimi yapıtın kendi içine değil, ayrı bir dosyaya yazılır | dosya var/yok | `CEO-0002`, `STYLE-SIGNOFF-RFC-6001` |
| 2 | **Ayrı rol-lensi** — edimi yazan rol, yapıtın `owner`'ından farklı olmalı | künye `owner:` karşılaştırması | `RFC-6001` owner `ens-philosopher`, onay `ens-ceo` + `ens-style-guardian` |
| 3 | **Ayrı context** — edim, yapıtı yazan turdan farklı bir turda üretilir | kayıtta bağımsızlık beyanı zorunlu | `SCAN-03:420` — *"taze context"* deseni, ENS'te 20+ kez uygulanmış |
| 4 | **İki yönlü iz** — künyede `ratified_by` + `ratified_at`; kayıtta yapıt kimliği | grep | `RFC-6001:17-18` `ceo_review:`/`style_signoff:` alanları |

**Bu, R2'yi zayıflatmaz — güçlendirir.** "Farklı aktör" tek operatörlü depoda
*doğrulanamaz* bir koşuldur (`SCAN-03:244-245`: *"`git log`/`git blame` çalıştırılabilecek
bir araç yoktu; iddia fabrike edilmemiştir"* — statüyü kimin çevirdiği bugün bile
bilinmiyor). Dört koşulun dördü de **dosya sisteminden** doğrulanabilir.

**Zaman ayrımı** (görevin andığı üçüncü seçenek) tek başına yetersizdir: `ratified_at`
kaydı sahtelenmeye açık değildir ama bir **lens** farkı üretmez — D-3.1'deki kaçağın aynısını
zaman eksenine taşır. Bu yüzden 3. koşulu "zaman" değil "context + bağımsızlık beyanı"
olarak yazdım.

**Talep E (bloke edici, RFC-6002 §3.1'e).** R2 şöyle yeniden yazılsın:
> **R2.** Ratifikasyon edimi, yapıtın `owner`'ından **farklı bir rol-lensinden**, **ayrı bir
> context'te** üretilir ve **ayrı bir kayda** (`governance/` ya da ilgili `reviews/` altında)
> yazılır. Tek-operatörlü işletimde bağımsızlık **aktör kimliğiyle değil, edim ayrımıyla**
> sağlanır; kayıt bir bağımsızlık beyanı taşır. *(Emsal: CEO-0001/0002/0003,
> STYLE-SIGNOFF-RFC-6001 — desen kanıtlanmıştır, icat edilmesi gerekmiyor.)*

Bu düzeltmeyle FC-3 **kapanır**, çünkü koşul artık atanmamış bir ajan dosyasına değil,
depoda 4 kez uygulanmış bir edim desenine bağlanır. Düzeltilmezse R2 uygulanamaz kalır.

### D-4.4 — R3'ün alanı şemada **yok**; R1'in alanı da eksik

R3 `ratified_by`/`ratified_at` istiyor (`:94`). `.claude/standards/metadata-header.md`'de
grep: `ratified_by` **yok**, `approved_by` **yok**. RFC-6002:137 bunu doğru biçimde kapsam
dışı bırakıyor (*"`ens-style-guardian` owner'lığında ayrı edim"*) — **katılıyorum**, doğru
yetki ayrımı.

Ama bir ek boşluk var, RFC hiç anmıyor: **`validation_dimension` de şemada tanımlı değildir.**
`metadata-header.md` grep sonucu tek ilgili satır `:61` — `skeptic_review: SKR-014` (üstelik
**skaler**, oysa korpus liste kullanıyor: `RFC-6001:16` `[SKR-034, SKR-035, SKR-036]`).
`validation_dimension` yalnızca `validation-framework.md:32,37`'de yaşıyor.

Bu, RFC-6002 §4 ve RFC-6003 §2-3 için doğrudan sonuç doğurur: **"boyut" üzerine kurulan her
kural, künye şemasında karşılığı olmayan bir alanı sayıyor.** Ölçüm bölümüne bağlanır (§8).

## 5. Kadro ↔ kural bağı — kural kendini gevşetir mi?

**Evet, üç ayrı yoldan. RFC'ler birini görüyor, ikisini görmüyor.**

### D-5.1 — Kuralın yüklemi, işaret ettiği dosyada **mevcut değil**

RFC-6002:112-113: *"Bir boyut 'aktif'tir ancak ve ancak **ROSTER'da** o boyutun validator
rolü atanmışsa."* RFC-6003 §1 aynı bağı kuruyor.

`.claude/agents/ROSTER.md` okundu (52 satır). Yapısı: `Aktif` tablosu (`:11-24`, sütunlar
**Agent · Faz · Görev · Yazma alanı**) + `Ertelenmiş` tablosu (`:33-47`).

- **"Validator boyutu" diye bir sütun yoktur.**
- `scientific`/`ontology`/`engineering`/`business`/`ethical` sözcüklerinin hiçbiri ROSTER'da
  geçmiyor. `ens-skeptic` satırı (`:14`) yalnızca *"Teoriye saldırır (SKR)"* der.
- Boyut→rol eşlemesi **başka bir dosyadadır**: `governance/roles.md:59-63` (*"Mevcut eşleme"*)
  ve türevi `validation-framework.md:49-51`.

**Yani kuralın koşulu, gösterdiği belgeye karşı değerlendirilemez.** Bugün "ROSTER'da ethical
validator atanmış mı?" sorusunun cevabı *"hayır"* değil, **"soru ROSTER'a sorulamaz"**dır.
Bir kapı ölçütü, referansı olmayan bir yüklemle yazılmışsa, uygulamada **her zaman en gevşek
yorumu** alır — çünkü ihlali gösterecek veri yoktur.

**Talep F.** "Aktif boyut" kümesinin **tek yetkili yeri** `governance/roles.md` (GOV-010)
olmalıdır — `constitutive: true` taşır, RFC ile değişir, Madde XIV'e tabidir. ROSTER
bu kümenin **türevi/aynası**dır, kaynağı değil. RFC-6002:112 ve RFC-6003 §1 buna göre
düzeltilmelidir.

### D-5.2 — Yön ihlali: en alt katman, en üst normu belirliyor (asıl mimari bulgu)

RFC-6002:112 kabul edilirse, M5 kapısının **içeriği** `.claude/agents/ROSTER.md`'nin
içeriğinin fonksiyonu olur. Madde XII (`ENS-0000:209-221`):

```
Anayasa → Külliyat → Standards → Commands → Agents (.claude/agents) → Implementation
```

ve `ENS-0000:218`: `Agents (.claude/agents)  ← felsefeyi tüketir; asla üretmez`.
`ROSTER.md:5` bunu kendi ağzıyla tekrarlıyor: *"Agent'lar bağımlılık grafiğinin **en
altındadır**: felsefeyi tüketirler, **üretmezler**."*

**Kapı ölçütü bir norm ise, ROSTER'ı norm kaynağı yapmak Madde XII'nin doğrudan ters
yönüdür.** Bu, `ens-architect` lensinin merkezî reddidir: bağımlılık oku yukarı bakamaz.

Somut ve bugün gözlemlenebilir sonuç: **ROSTER, RFC'siz düzenlenen bir dosyadır.**
Bu incelemenin başındaki `git status`'te `.claude/rules/` ve `6000-rfc/reviews/` altında
commit edilmemiş değişiklikler var; ROSTER'a 2026-07-26 ve 2026-07-27'de üç ajan eklenmiş
(`ROSTER.md:22,23,24`) ve bunların hiçbiri bir RFC'den geçmemiştir. Yani RFC-6002 kabul
edilirse, **Madde XIV yordamı olmadan yapılan bir dosya düzenlemesi M5 kapısını sessizce
gevşetebilir ya da sıkabilir.** RFC-6002:114'ün açıkça engellemek istediği şey — *"bir boyutun
atanmamış olması sessizce muafiyete dönüşmesin"* — kendi seçtiği mekanizma tarafından
üretilir.

### D-5.3 — RFC'ler kaçağın **yanlış yarısını** kapatıyor

RFC-6002 FC-4 (`:168-170`) ve RFC-6003 FC-4 (`:130-131`) kaçağı **görüyor** ama dar
tanımlıyor: *"hiçbir boyutun atanmadığı bir durumda M5'i sıfır validator'la geçirilebilir
kılarsa."*

Kaçağın üç kipi vardır; RFC'ler yalnızca (a)'yı koruyor:

| Kip | Durum | RFC koruması |
|---|---|---|
| (a) **Sıfır atama** → sıfır validator ile M5 | bugün gerçekleşmiyor | FC-4 kapsıyor (RFC-6002:168) |
| (b) **Aynı aktöre çoklu atama** → {Sci, Ont} = tek lens | **bugün gerçek** (`roles.md:61`) | **kapsanmıyor** (bkz. §3, D-3.1) |
| (c) **Atamayı geri çekme** → boyut "pasif" olur, gereksinim düşer | mekanizmanın doğrudan sonucu | **kapsanmıyor** |

(c) en sinsi olanıdır ve tamamen mekaniktir: bir boyutun validator'ı ROSTER'dan/`roles.md`'den
silinirse, kural o boyutu "aktif değil" sayar ve **gereksinim kendiliğinden düşer**. Yani
kuralı gevşetmenin yolu kuralı değiştirmek değil, **kadroyu küçültmektir**. RFC-6003 §4'ün
uyarısı (`:100-102`: Ontology Validator'ın çıkar çatışması) bu ailenin bir üyesidir ama
genel kip adlandırılmamıştır.

### D-5.4 — Kapatılabilir mi? **Evet — taban + tek yönlü mandal.** (görevin sorduğu)

Üç mekanizma birlikte kaçağın üç kipini de kapatır ve üçü de mekanik denetlenebilir:

1. **Devredilemez taban (non-derogable floor).**
   > *"Aktif boyut kümesi hiçbir koşulda {Scientific, Ontology} kümesinin altına inemez ve
   > M5 boyut şartı, **farklı aktörlere** atanmış en az iki boyutla sağlanır."*
   → (a) ve (b) kapanır. Emsal ENS'in kendi kernel disiplinindedir: `ADR-0001 §5.6`
   *"kritik/geri-dönülemez action'larda **bypass yok**"* — taban kuralları ENS'e yabancı değil.

2. **Tek yönlü mandal (ratchet): atanmamış boyut = BORÇ, muafiyet DEĞİL.**
   > *"Madde XII/`validation-framework.md` uyarınca fazı gelmiş bir boyutun validator'ı
   > atanmamışsa, o boyut **aktif ve karşılanmamış** sayılır; ilgili yapıt M5'e **çıkamaz**.
   > Atama eksikliği kayıtlı borç (`ROADMAP` G-NN) üretir, gereksinim düşürmez."*
   → (c) kapanır. Bu, RFC-6003 §2'nin ethical için vardığı sonucun (`:64`: *"0 ethical SKR
   meşru erteleme değil, kayıtlı borçtur"*) **genelleştirilmiş hâlidir** — yani RFC-6003
   doğru ilkeyi bulmuş, ama yalnızca bir boyuta uygulamıştır.

3. **Kümenin yeri constitutive katmanda** (Talep F) → RFC'siz değişemez.

Bu üçü eklenirse "tüm aktif boyutlar" formülü **sağlam** hâle gelir ve RFC-6002'nin özgün
katkısı korunur. Eklenmezse formül, sabit listeden (`maturity-model.md:31`) **daha
gevşektir** — çünkü sabit liste en azından değişmez.

### D-5.5 — RFC-6003'ün dürüstlüğü: kaydedilmesi gereken güçlü nokta

RFC-6003:100-102, Ontology Validator önerisinin kendi kaçağını **gizlemeden** yazıyor:
> *"Ontoloji boyutunun en yetkin aktörü, ontoloji yapıtlarının da owner'ıdır. Rolü ona
> verirsek yapıtlarının yarısında kullanılamaz; başkasına verirsek yetkinlik düşer. Bu
> gerilim **çözülmemiştir ve gizlenmiyor.**"*

Bu gerilim gerçektir ve beni doğrudan ilgilendirir: `ens-architect` ENS-4001/ENS-4010'un
owner'ıdır (`SCAN-03:227`). **Yapısal çözüm mevcuttur ve RFC bunu görmemiş:** rol ataması
yapıt-bazlı değil, **owner-bazlı dışlama** ile yazılabilir —
> *"Ontology Validator rolü `ens-architect`e atanır; `owner: ens-architect` taşıyan yapıtlarda
> bu rol **`ens-style-guardian`e düşer** (Custodian, `roles.md:31` — bütünlük/Meta Model uyumu
> zaten görevidir)."*

Bu, yeni bir rol icat etmeden (footprint-ladder 1. basamak: mevcut yapıtı genişlet) çıkar
çatışmasını kapatır ve `roles.md`'nin mevcut Custodian tanımından türer. RFC-6003 §7 soru 2'ye
**yapısal bir cevaptır**; kararı GOV-010 owner'ınındır, benim değil.

## 6. Faz modeliyle uyum

### D-6.1 — RFC'lerin kendisi faz modeline aykırı konumda üretilmiş

`.claude/standards/ens-phase-model.md:30`:
> `| 3 | Mimari | 5000-architecture/, **6000-rfc/** | ens-chief-architect, ens-domain-modeler, ens-rfc-writer |`

Yani `6000-rfc/` **Faz 3** dizinidir ve sahibi üç ajandır. Doğrulama:

| Atanan sahip | Dosya var mı | ROSTER durumu |
|---|---|---|
| `ens-chief-architect` | **hayır** | `ROSTER.md:36` **Ertelenmiş** |
| `ens-domain-modeler` | **hayır** | `ROSTER.md:37` **Ertelenmiş** |
| `ens-rfc-writer` | **hayır** | `ROSTER.md:38` **Ertelenmiş** |

RFC-6002/6003'ün `owner:`'ı ise **`ens-philosopher`** (`:6`) — `ROSTER.md:13`'e göre **Faz
0-2, 6** ajanı. Yani **Faz-3 dizinine, Faz-1 ajanı, atanmamış üç Faz-3 rolünün yerine
yazıyor.** Bu SCAN-03 E-04'ün (`:518-524`) devamıdır ve iki RFC de bunu anmıyor.

**Ağırlaştırmıyorum:** RFC-6001 de aynı biçimde üretildi (`owner: ens-philosopher`) ve tam
yoldan geçti. Yani bu, bu iki RFC'nin kusuru değil **faz modelinin bayatlığıdır**. Ama
RFC-6002/6003 **doğrudan rol-ataması ve kadro konusunu** düzenlediği için, kendi üretim
rollerinin atanmamış olduğunu **kayda geçirmeleri gerekirdi.** Düşük şiddet, gerçek eksik.

### D-6.2 — SCAN-03 E-03 (Faz 1 açıkken Faz 4 çalışıyor): düzeltilmiyor, **kötüleştiriliyor**

E-03 (`SCAN-03:492-516`) şunu kaydetmişti: ENS-2003/ENS-2004 `status: review`'e düştü, Faz-4
kodu **aynı turda** o doğrulanmamış teoriye göre yeniden yazıldı; Madde VII `:139-140`
lafzına göre kapı ihlali.

SCAN-03 bunun çözümünü **Ö-04**'e (`:604`) bıraktı: *"kapı modelini fiili döngüsel çalışmayla
uzlaştır… **RFC** (Madde VII yorumu ya da `ens-phase-model.md` revizyonu), owner `ens-ceo`"*.

- RFC-6002 §5 (`:125-128`) kapsamını Ç-01/Ç-02/Ç-03 ile sınırlıyor, Ç-04/Ç-05'i RFC-6003'e
  veriyor. **Ö-04 hiçbirinde yok.**
- RFC-6003 §5 tablosu da (`:106-111`) Ö-04'ü anmıyor.

Yani SCAN-03'ün beş çatışmasından Ç-01..Ç-05 ele alınmış, **P1 öncelikli E-03 bulgusu iki
RFC'nin de dışında kalmıştır.** Bu tek başına ihmal sayılabilirdi. Sorun şu ki RFC-6002
E-03'ü **ölçülebilir biçimde ağırlaştırıyor:**

RFC-6002 §3.1, bugün **kuralsız** olan `review → ratified` geçişine üç koşullu bir kapı
(R1+R2+R3) koyuyor. R2 bugün uygulanamaz (§4, D-4.1). Sonuç zinciri:

```
ENS-2003, ENS-2004  →  status: review  (SCAN-03 E-02, ROADMAP:39 "BLOCKING")
        │
        │  RFC-6002 kabul → ratified'e çıkış için R2 gerekli
        │  R2'nin yürütücüsü (ens-ceo) atanmamış
        ▼
review'de kalış süresi  UZAR
        │
        ▼
Faz-4 kodu (CompanyMemory.cs vd.) doğrulanmamış teoriye bağlı KALMAYA DEVAM EDER
```

**Yani RFC-6002, Madde VII ihlalinin *süresini* uzatır.** Kapıyı sıkmak doğru yöndür; ama
kapıyı sıkarken kapıyı **açacak makamı atamamak**, Faz-4'ü daha uzun süre doğrulanmamış
zemin üstünde tutar. Bu, RFC'nin niyetinin tersi bir sonuçtur ve hiçbir yerinde hesaplanmamış.

**Talep G.** RFC-6002 §6'ya bir **geçiş hükmü** girsin:
> *"R2'nin yürütücüsü atanana kadar `review → ratified` geçişi **R1 + R3** ile yapılır;
> R3 kaydına `ratified_by: pending-R2` sentinel'i yazılır ve her böyle geçiş bir kayıtlı
> borç satırı üretir. R2 atandığında bu satırlar toplu kapatılır."*

Bu, kapıyı beyan düzeyinde bırakmadan uygular ve E-03'ü uzatmaz. Alternatif — R2 atanana
kadar hiçbir yapıtın `ratified` olamaması — Madde VII gereği Faz 4'ü **durdurur**; bunu
öneremem, çünkü faz durdurma kararı `ens-ceo` alanıdır (SCAN-03 Ö-04).

### D-6.3 — `validation-framework.md:53-56` ile RFC-6003 §2 arasında hesaplanmamış sonuç

`validation-framework.md`'nin **Kural** bölümü (`:53-56`):
> *"Bir yapıt, **ilgili boyutun** Validation'ından **survives** almadan **bir sonraki fazın
> temeli olamaz** (Anayasa Madde VII, X)."*

RFC-6003 §2 Ç-04'ü *"ethical, `validation-framework.md` uyarınca **tüm fazlarda** aktiftir"*
diye çözüyor. Bu iki cümle birleştirildiğinde çıkan sonuç RFC'de hiç yazmıyor:

> Ethical her fazda "ilgili boyut" ise ve 0 ethical doğrulama varsa, **hiçbir Faz 1-2 yapıtı
> Faz 3-4'ün temeli olamazdı.** Yani ADR-0001/0002 ve tüm `7000-reference-implementation/`
> geriye dönük olarak dayanaksız kalır.

RFC-6003 bunu "kayıtlı borç" (`:64`) diyerek yumuşatıyor — **doğru pratik karar**, ama
`:53-56`'nın lafzı borç değil **yasak** koyuyor. RFC-6003 §2'nin çözümü kabul edilirse
`validation-framework.md:53-56` de aynı edimle yumuşatılmalıdır; aksi hâlde kabul edilen
norm, Faz 3-4'ün tamamını retroaktif olarak geçersiz ilan eden bir cümleyle birlikte
yürürlükte kalır. RFC-6003 §5'in "değişen metin" tablosunda `validation-framework.md` var,
ama yalnızca boyut sözlüğü bağlamında — `:51` (D-1.2) ve `:53-56` (bu bulgu) kapsam dışı.

## 7. Geriye dönük etki

RFC-6002 §9 soru 2 (`:177-178`) açık bırakıyor: *"Toplu geriye dönük ratifikasyon mu, yoksa
'bu tarihten sonrası' mı?"* Yapısal olarak **üçüncü bir seçenek** doğru; ilk ikisi de kusurlu.

**Güncel envanter (grep `^canon:\s*true`, 2026-07-27):**

| Yapıt | Durum |
|---|---|
| `ENS-0000:5` | Madde IV `:98-106` — Grundnorm, **ilkeli muafiyet** (SCAN-03 A-01 de böyle saydı) |
| `ENS-1000:5` | `canon: true`, zincir SKR-001→002, tek boyut → ROADMAP **G-28** açık borç |
| `ENS-4000:5` | `canon: true`, `status: review`, `skeptic_review` alanı yok → ROADMAP **G-25**, **P0** |

**Not — görev metnindeki "4 `canon: true`" sayısı bayattır.** ENS-3000, 2026-07-27'de
`canon: true → false` düşürüldü (ROADMAP:238, G-24). Bugün **3**'tür. SCAN-03'ün 4 sayısı
o düşüşten öncedir.

### D-7.1 — Seçenek analizi

| Seçenek | Sonuç | Değerlendirme |
|---|---|---|
| **(A) Toplu geriye dönük** — 9 `ratified` yapıt R2/R3 taşımadığı için `review`e döner | ENS-2001/2002 (M3) ve ENS-3021/3022/3023 `review`e düşer; bunlar `Ens.Kernel`'de uygulanmış (`DecisionEntropy.cs`, `DecisionCapital.cs` vd.). Madde VII `:139-140` gereği **Faz 4 durur** | **REDDEDİLİR.** Bir künye alanının yokluğu, doğrulaması yapılmış 9 yapıtı geçersiz kılamaz — kapı, kanıtı değil kaydı eksiktir. Ayrıca ENS'in en pahalı fazını bir şema eksiği yüzünden durdurur |
| **(B) "Bu tarihten sonrası"** — eski yapıtlar dokunulmaz | İki rejimli korpus | **TEK BAŞINA REDDEDİLİR.** Denetlenemez hâle gelir: eksik `ratified_by` alanının **ihlal mi, rejim-öncesi mi** olduğu ayırt edilemez. `formal-checker`/linter için bu, sessiz fail-open'dır — `ens-silent-failure-hunter` alanının tam tanımı |
| **(C) Grandfather + kayıtlı borç** — eski yapıtlar `ratified` kalır, **her biri** açık sentinel + borç satırı taşır | İki rejim **görünür** ve sayılabilir | **ÖNERİLEN** |

### D-7.2 — Neden (C): depo bu deseni zaten çalıştırıyor

(C) icat değildir; ENS'in kanıtlanmış davranışıdır. ROADMAP'te her yapısal borç **kendi
kimliğiyle** kayıtlıdır: `G-24` (ENS-3000 canon), `G-25` (ENS-4000 canon, P0), `G-27`
(ethical), `G-28` (ENS-1000 boyut borcu). `G-28:242`'nin gerekçesi burada birebir geçerlidir:
> *"(c) **tek yapıtı cezalandırmak sistemik kusuru gizler.** ENS-1000'e dokunulmadı."*

Aynı akıl yürütme toplu demotion'a da uygulanır: 9 yapıtı birden düşürmek, kusurun
**sistemik** (atanmamış onay makamı) olduğunu bireysel yapıtların statüsüne dağıtarak gizler.

**Talep H (RFC-6002 §9 soru 2'ye yapısal cevap):**
> *"Geriye dönük toplu ratifikasyon **yapılmaz** ve toplu demotion **yapılmaz**. Mevcut 9
> `ratified` yapıt statüsünü korur; her biri künyesine `ratified_by: pre-RFC-6002` sentinel'i
> alır ve ROADMAP'te tek bir toplu borç satırı (`G-NN`) ile kayda geçer. Sentinel, boş alandan
> **ayırt edilebilir** olduğu için linter fail-open'a düşmez: `alan yok` = ihlal,
> `pre-RFC-6002` = kayıtlı miras. Yeni ratifikasyonlar R1-R3'ün tamamını taşır."*

Bu, iki-rejimli korpusun **denetlenebilir** biçimidir — görevin sorduğu *"ikincisi kabul
edilebilir mi?"* sorusunun cevabı: **yalnızca sentinel ile birlikte kabul edilebilir.**

### D-7.3 — `canon: true` üçlüsü RFC-6002 kapsamı dışındadır ve öyle kalmalıdır

ENS-4000 (G-25, P0) ve ENS-1000 (G-28) borçları RFC-6002'nin R1-R3 kapısıyla **çözülmez** —
onlar `review → ratified` değil, `canon` kapısındadır ve RFC-6001 §7.2/§8.3 yolu zaten
tanımlıdır. RFC-6002'nin bunları kapsamaması **doğru bir kapsam kararıdır**; §9'da
"açık soru" diye anılmaları kafa karıştırıcıdır, ayrı borç olarak işaretlenmeleri yeterlidir.

## 8. Araç bağı — ölçülemeyen kapı, kapı mıdır?

**Cevap: hayır. Ölçülemeyen kapı bir kapı değil, bir temennidir.** Ve bu, ENS'te *kanıtlanmış*
bir kusur kalıbıdır — RFC'ler onu tekrarlıyor.

### D-8.1 — Emsal: E-01, aynı hatanın önceki turu

`ens-phase-model.md:73-76` her faz kapısını `/validate-theory` komutuna bağlar; `:56` Faz 3
çıkışını *"`/validate-theory` **geçer**"* diye tanımlar; Anayasa Madde VIII izlenebilirlik
denetimini aynı komuta bağlar.

Doğrulandı (`.claude/**/*.md` glob, 33 dosya): **`.claude/commands/` dizini yoktur.**
Madde XII `:216` bu katmanı grafikte listeler; katman **boştur**. SCAN-03 E-01 sonucu:
**0/5 faz kapısında ölçüm yapılmıştır** — çünkü ölçme aracı hiç yazılmadı.

Yani ENS'te bir kapı ölçütü yazıp aracını yazmamanın bedeli **zaten ölçülmüştür**: beş kapının
beşi de ölçülmeden geçildi ve bu, SCAN-03'ün **P0** bulgusudur (`:570`).

### D-8.2 — RFC-6002/6003 aynı deseni tekrarlıyor

Önerilen yeni kapılar ve ölçülebilirlikleri:

| Kapı | Mekanik denetlenebilir mi | Bugünkü araç |
|---|---|---|
| **R1** — ≥1 `survives` verdict'i taşıyan bağımsız inceleme | **evet** — `skeptic_review` + SKR `verdict` alanı | **yok** |
| **R2** — yazardan farklı aktörün kayda geçen edimi | edim-tabanlı yazılırsa (§4, Talep E) **evet**; "aktör" olarak kalırsa **hayır** | **yok** |
| **R3** — `ratified_by` + `ratified_at` künyede | **evet** — en kolay invariant | **yok**; alanlar şemada da yok (D-4.4) |
| **"tüm aktif boyutlar"** (RFC-6002 §4) | **evet** — `validation_dimension` çokluğu + owner çokluğu | **yok**; `validation_dimension` şemada tanımlı değil |
| **Ç-05 / kurucu yol G4** (RFC-6003 §3) | **evet** — `constitutive` + dimension kesişimi | **yok** |

**Beş yeni kapının beşi de mekanik olarak denetlenebilir ve beşi için de bugün araç yoktur.**
Ne RFC-6002 §6 (değişecek metinler) ne RFC-6003 §5 tablosu bir ölçüm aracı anmıyor.

`tools/ens-ontology-linter/` mevcuttur (README doğrulandı) ama kapsamı ENS-4010'un **iki
ontoloji invariant'ıdır** (profile-satisfiability, transitivity well-formedness — ROADMAP:233,
G-09/10) ve **canlı koşusu owner/CI teyidi beklemektedir**. Yönetişim künyesi denetlemiyor.

### D-8.3 — Bu benim yetki alanım: somut araç önerisi

Ölçüm aracı mimarisi `tools/` ve ADR alanıdır — **önerebileceğim tek somut yapıttır.**

**Öneri: `tools/ens-governance-linter/`** (ya da mevcut linter'a ikinci bir invariant ailesi —
`footprint-ladder.md` 1. basamak gereği **önce genişletme** denenmeli). Kapsam, tamamı
deterministik ve LLM gerektirmeyen dört invariant:

| Inv | Kural | Girdi |
|---|---|---|
| **GL-1** | `canon: true` ⇒ zincirdeki `validation_dimension` kümesi ≥2 **ve** o SKR'lerin `owner` kümesi ≥2 | yapıt künyesi + `skeptic_review` listesi + SKR künyeleri |
| **GL-2** | `status: ratified` ⇒ `ratified_by` alanı var (değeri `pre-RFC-6002` sentinel'i olabilir) | künye |
| **GL-3** | `ratified_by` ≠ `owner` (edim ayrımı, §4 Talep E) | künye |
| **GL-4** | Her SKR'nin `validation_dimension` değeri, `validation-framework.md`'nin izinli kümesinde | SKR künyeleri |

GL-1 doğrudan G4'ü ölçer ve **bugün çalıştırılsa 0/15 verirdi** — yani SCAN-03 §5.3'ün elle
ürettiği sayıyı makineyle üretir, regresyonu önler. GL-4, SCAN-03 D-02a'nın tespit ettiği
`constitutional` şema-sürüklenmesini (3 SKR) anında yakalar.

**Dürüstlük kaydı (SKR-041 emsali):** bu araç **yazılmadı ve çalıştırılmadı.** Yukarıdaki
"0/15 verirdi" ifadesi, SCAN-03 §5.3'ün elle sayımından **türetilmiş bir beklentidir**, bir
koşu çıktısı değildir. Hiçbir test sonucu fabrike edilmemiştir.

**Talep I.** RFC-6002 ve RFC-6003 kabul edilecekse, **kabul kararı bir ölçüm-aracı ADR'sine
bağlanmalıdır** (SCAN-03 Ö-02 zaten bunu istiyordu ve kapatılmadı). Aksi hâlde iki RFC,
E-01'in yönetişim katmanındaki tekrarını üretir: **norm var, ölçü yok.** Aracın yazımı benim
alanımdadır; kapı ölçütünün norm metnine bağlanması değildir.

## 9. Katıldığım noktalar

Yapısal itirazların yanında bunlar kayda geçmezse inceleme kalibre değildir.

1. **`ratified` ≠ `Canonical` ayrımı doğrudur.** `maturity-model.md:34` gerçekten öyle diyor,
   G4'ün öznesi gerçekten *"Canonical"*tır ve iki kural gerçekten farklı kapıları yönetir.
   İtirazım ayrımın **kendisine** değil, `canon: true`'nun **tek yollu** sunulmasınadır
   (D-2.3) — Anayasa iki yol tanıyor.
2. **§2.2'nin argümanı yöntemsel olarak sağlamdır.** *"Bir yorum, yorumladığı metni toplu
   ihlale çeviriyorsa önce yorumdan şüphelenilir"* (`:65-66`) — bu iyi bir yorum ilkesidir
   ve SCAN-03'ün iki-okuma yaklaşımıyla tutarlıdır.
3. **§3'ün gerçek boşluk teşhisi bu incelemenin en değerli bulgusudur.** *"`review → ratified`
   geçişini yöneten hiçbir yazılı kural kalmaz"* — Ç-01'i çözmenin altından **daha büyük** bir
   sorun çıkarmak, çözmekten daha zordur. SCAN-03 C-01/C-02 aynı yere işaret ediyordu;
   RFC bunu bir *kural boşluğu* olarak adlandırarak ileri götürmüştür.
4. **RFC-6002 §6'nın künye alanını kapsam dışı bırakması doğru yetki disiplinidir**
   (`:137` — *"`ens-style-guardian` owner'lığında ayrı edim; burada yalnız gereksinim olarak
   kaydedilir"*). Tam olarak GOV-010'un istediği ayrım.
5. **RFC-6003 §1.1'in ampirik kanıtı gerçektir.** `AUDIT-WAVE2-FIDELITY.md` okundu:
   `:1-10` yöntemi *"55 `// TRACE:` iddiasının tamamı … atıf yapılan teori/ADR bölümü açıldı
   ve satır düzeyinde karşılaştırıldı"*; `:209` D-5 başlığı *"ENS-2003 §3a 'iki dik eksen'
   diyor; kodda **confidence iki kez sayılıyor**"*. Bu lens `validation-framework.md:44`'ün
   Engineering tarifine (*inşa edilebilirlik, test edilebilirlik*) uyar. **RFC-6003 FC-2
   tetiklenmiyor.**
   *Ek — RFC'nin lehine bir bulgu:* o dosyanın **künyesi yoktur**, `validation_dimension`
   taşımaz, REGISTRY'de kayıtlı değildir. Yani boyut çeşitliliğinin tek ampirik kanıtı,
   mevcut SKR sistemi tarafından **kaydedilemeyecek** bir yapıttır. Bu, RFC-6003'ün tezini
   güçlendirir ve §5 tablosuna eklenmelidir: engineering denetimleri SKR ad alanına alınmadıkça
   boyut çeşitliliği ölçülemez.
6. **RFC-6003 §3'ün Ç-05 çözümü sağlamdır.** `RFC-6001:175` birebir doğrulandı; *"sessizlik
   ilga etmez"* ilkesi doğru; RFC-6001'in kendi zincirinin korpusun en ağır yolu olduğu
   `SCAN-03:195-200` ile teyitli. Kurucu yola muafiyet vermemek, D-2.3'te tespit ettiğim
   iki-yollu modelin **gevşemeye dönüşmesini** engelleyen tek koruma budur — yani RFC-6003 §3,
   RFC-6002'nin kendi açığını kapatan parçadır (§3'teki bağımlılık argümanımın bir dayanağı).
7. **İki RFC de kendi ironisini gizlemiyor** (RFC-6003:145-148; RFC-6002:181-185 —
   *"Yazarı kendi turunu `survives` işaretleyemez"*). Madde X'in istediği davranış budur ve
   bu inceleme onun sonucudur.
8. **RFC-6002 §5'in "neden atomik" gerekçesi biçim olarak doğrudur** — RFC-6001 emsaline
   (Madde IV + künye şeması) yapılan atıf yerindedir. İtirazım gerekçeye değil, **kesim
   çizgisinin yerine**dir (§3).

## 9. Katıldığım noktalar

(doldurulacak)

## 10. Bulgu listesi (ID / sebep / etki / öncelik / bağımlılık / çözüm)

| ID | Bulgu | Sebep | Etki | Önc. | Bağımlılık | Çözüm |
|---|---|---|---|---|---|---|
| **D-2.3** | RFC-6002'nin çekirdeği (`maturity-model.md:34`) Anayasa Madde IV `:107-112` tarafından **aşılmış** | Standards satırı, RFC-6001 ile açılan ikinci canon yolunu tanımıyor | §2 tezi eksik; FC-1 yanlış yere bakıyor; FC-2 **tamamen** tetikli | **P0 / bloke edici** | RFC-6001 (accepted); SKR-036 S2 borcu | **Talep B** — §2'yi Madde IV üzerine kur, iki yollu yaz; §6'ya `maturity-model.md:34` hizalaması ekle |
| **D-3.1/3.2/3.3** | RFC-6002 tek başına kabul edilirse **gevşetir**: aktif boyut kümesi = {Sci, Ont}, ikisi de `ens-skeptic` | `roles.md:61` iki boyutu tek ajana veriyor; RFC-6002 FC-4 bu kipi kapsamıyor | G4-boyut uyumu doğrulama yapılmadan **%0 → %40** | **P0 / bloke edici** | RFC-6003 §4 (Ö-07) | **Talep C** — C-1 tek paket **veya** C-2 bağımsızlık tabanı cümlesi |
| **D-4.1** | R2'nin yürütücüsü (`ens-ceo`) **dosya olarak yok**; FC-3 doğuşta tetikli | ROSTER `:35` "Ertelenmiş"; `roles.md:63` "fazı gelince" | Kapı beyan düzeyinde kalır | **P0 / bloke edici** | GOV-010 rol ataması | **Talep E** — R2'yi *aktör* değil **edim ayrımı** olarak yaz (CEO-0001/2/3 + STYLE-SIGNOFF emsali) |
| **D-1.2** | RFC-6003 §2'nin Ç-04 ispatı yanlış yapıya dayanıyor | Çelişki iki belge arasında değil, `validation-framework.md` **içinde** (`:29` ↔ `:51`) | Verdict doğru, ispat çöküyor; `:51` düzeltilmezse çelişki kaynakta kalır | **P1** | — | §2'yi "boyut aktifliği ≠ rol atanmışlığı" ekseninde yeniden yaz; `:51`'i §5 tablosuna ekle |
| **D-5.1/5.2** | "Aktif boyut" yüklemi ROSTER'da **karşılıksız**; ROSTER Madde XII'nin **en alt** katmanı | ROSTER'da boyut sütunu yok; eşleme `roles.md:59-63`'te | Norm, RFC'siz düzenlenen bir dosyanın fonksiyonu olur → yön ihlali | **P1** | Madde XII | **Talep F** — kümenin yeri GOV-010; ROSTER türev/ayna |
| **D-5.3/5.4** | Kaçağın (b) *"aynı aktöre çoklu atama"* ve (c) *"atamayı geri çekme"* kipleri kapatılmamış | FC-4 yalnız (a)'yı kapsıyor | Kuralı gevşetmenin yolu kadroyu küçültmek olur | **P1** | D-3.1 | Devredilemez taban + tek yönlü mandal (atanmamış boyut = borç, muafiyet değil) |
| **D-6.2** | RFC-6002, SCAN-03 **E-03**'ü uzatıyor (Faz-1 açıkken Faz-4 çalışıyor) | `review→ratified` sıkılaşıyor, açacak makam atanmamış | ENS-2003/2004 `review`de kalır; Faz-4 doğrulanmamış zeminde daha uzun durur | **P1** | SCAN-03 Ö-04 (owner `ens-ceo`) | **Talep G** — geçiş hükmü: R1+R3, `ratified_by: pending-R2` sentinel'i |
| **D-6.3** | RFC-6003 §2 kabul edilirse `validation-framework.md:53-56` Faz 3-4'ü retroaktif dayanaksız bırakır | O satır borç değil **yasak** koyuyor | Kabul edilen norm, kendi kaynağıyla çelişik yürürlüğe girer | **P1** | D-1.2 | `:53-56` aynı edimle yumuşatılsın |
| **D-8.2/8.3** | Beş yeni kapının **beşi de** ölçülemez; E-01 kalıbının tekrarı | Ne RFC bir araç anmıyor; `validation_dimension`/`ratified_by` şemada yok | Norm var, ölçü yok → sessiz fail-open | **P1** | SCAN-03 Ö-02 (kapatılmadı) | **Talep I** — GL-1..GL-4 invariant'ları; kabul kararı ADR'ye bağlansın |
| **D-7.1/7.2** | Geriye dönük etki çözülmemiş; (A) Faz-4'ü durdurur, (B) denetlenemez korpus üretir | RFC-6002 §9 açık soru | Toplu demotion sistemik kusuru gizler (G-28 gerekçesi) | **P1** | ROADMAP G-24/25/27/28 | **Talep H** — grandfather + `ratified_by: pre-RFC-6002` sentinel + tek borç satırı |
| **D-3.5** | RFC-6004'ün `depends_on`'ında **RFC-6003 yok**, gövdesinde (`:225-226`) var | künye/gövde uyumsuzluğu | Gerçek zincir makine tarafından **ters** okunur | **P2** | RFC-6004 | **Talep D** — künyeye `RFC-6003` ekle (`ens-style-guardian`) |
| **D-1.3** | RFC-6002 §1, SCAN-03'ü yanlış aktarıyor (`SCAN-03:47-50,55,116`) | `work-protocol.md:76-77` 3. kontrolü atlanmış | Yenilik iddiası zayıflar; §3.5'in kendi sınavında takılma | **P2** | `work-protocol.md:86` aynı hatayı taşıyor | §1 anlatısı düzeltilsin: *"üç bağımsız context aynı okumaya vardı"* |
| **D-1.1** | `canonical-process.md:45`'ten **"ör."** düşürülmüş | alıntı sadakati | Argümanı **kendi aleyhine** zayıflatıyor | **P2** | — | Alıntı düzeltilsin; §4 "iki liste" ifadesi gözden geçirilsin |
| **D-4.4** | `validation_dimension` ve `ratified_by` `metadata-header.md`'de **tanımlı değil**; `skeptic_review` skaler yazılmış (`:61`), korpus liste kullanıyor | şema sürüklenmesi | "Boyut" üzerine kurulan her kural şemasız alan sayıyor | **P2** | `ens-style-guardian` | Şema v0.3 ile üç alan birlikte tanımlansın |
| **D-6.1** | RFC'ler Faz-3 dizininde, Faz-1 ajanı tarafından, atanmamış üç Faz-3 rolünün yerine üretilmiş | `ens-phase-model.md:30` bayat | Kadro düzenleyen RFC'nin kendi kadro konumu kayıtsız | **P3** | SCAN-03 E-04 | RFC'lere durum notu; faz modeli revizyonu ayrı iş |

## 11. Verdict

Ölçüt: *"Bu katman doğru yerde mi? Yalnızca üstündekinden mi türüyor? Kabul edilirse ortaya
tutarlı ve ölçülebilir bir sistem çıkar mı?"*

### RFC-6002 — **yapısal kusurlu**

`uygulanamaz` demiyorum: teşhisi (§3'teki kural boşluğu) gerçek ve önemli, kapsam disiplini
(§6'da künye alanını dışarıda bırakması) doğru, R1/R3 uygulanabilir. Ama üç bağımsız
bloke edici kusur taşıyor:

1. **Çekirdeği üstün norm tarafından aşılmış** (D-2.3) — `.claude/standards/` satırı üzerine
   kurulmuş, oysa Anayasa Madde IV `:107-112` ikinci bir canon yolu tanıyor. Bu, bir
   **bağımlılık yönü** kusurudur: alt katman, üst normun kapsamını belirliyor.
2. **Tek başına kabul edilirse kapıyı gevşetiyor** (D-3.1/3.2) — G4-boyut uyumunu hiçbir yeni
   doğrulama yapılmadan %0'dan ~%40'a çıkarır. Bir sıkılaştırma önerisinin ölçülebilir
   gevşetme üretmesi, bölmenin yanlış yerden yapıldığının kanıtıdır.
3. **R2 doğuşta uygulanamaz** (D-4.1) — kendi FC-3'ünü tetikler; önerdiği yürütücünün ajan
   dosyası yok.

**Kabul koşulu:** Talep B + C + E kapanmadan kabul edilmemeli. Üçü de **metin düzeyinde**
çözülebilir — yapıtın yeniden yazılmasını gerektirmiyor. Talep G ve H eklenirse RFC
E-03'ü uzatmak yerine kısaltır.

### RFC-6003 — **yapısal olarak sağlam** (iki düzeltme borcuyla)

Bu RFC'nin **yapısal yeri doğru**: kadro ve boyut sözlüğü, kapı kapsamından önce gelir; ve
D-3.4'te gösterdiğim gibi tek başına kabul edilirse sistemi **gevşetmez, sıkar**. §3'ün Ç-05
çözümü (kurucu yola muafiyet yok), RFC-6002'nin açtığı iki-yollu modelin kaçağa dönüşmesini
engelleyen tek korumadır. §4'ün kendi çıkar çatışmasını gizlememesi (`:100-102`) ve §1.1'in
ampirik kanıtı (doğrulandı) bu RFC'yi korpustaki en dürüst yapıtlardan biri yapıyor.

İki düzeltme borcu, **bloke edici değil** ama kabulden önce kapanmalı:
- **D-1.2** — Ç-04 ispatı yanlış yapıya dayanıyor (`validation-framework.md:29` ↔ `:51`
  **iç** çelişki). Sonuç değişmiyor, gerekçe değişiyor.
- **D-6.3** — `validation-framework.md:53-56` aynı edimle yumuşatılmalı, yoksa kabul edilen
  norm Faz 3-4'ü retroaktif dayanaksız bırakan bir cümleyle birlikte yürürlükte kalır.

Ayrıca §5 tablosuna **engineering denetimlerinin SKR ad alanına alınması** eklenmeli
(§9 madde 5): boyut çeşitliliğinin tek ampirik kanıtı, bugünkü kayıt sisteminin dışındadır.

### Kabul sırası (bu incelemenin tek "mimari karar" niteliğindeki çıktısı)

```
RFC-6003  ──►  RFC-6002  ──►  RFC-6004
(kadro)        (kapı)         (Madde V nitelikleri)
```

İlan edilen sıranın **tersi**. RFC-6002'nin `depends_on`'ı RFC-6003'ü içermez ve RFC-6003'ün
`depends_on`'ı RFC-6002'yi içerir (`:9`) — yani grafik bugün gerçeğin tersini söylüyor.
Alternatif olarak Talep C-1 (tek paket) seçilirse sıra sorunu ortadan kalkar; bu, RFC-6001
emsaliyle en tutarlı yoldur.

### Bu incelemenin kendi sınırları (dürüstlük beyanı)

- **`ens-skeptic` turu (SKR-047/048) okunmadı** — bağımsızlık gereği. Örtüşen bulgular
  teyit, ayrışan bulgular asıl değerdir; hangisinin hangisi olduğunu **ben belirleyemem**.
- **SCAN-03'ten bağımsız değilim** (§0). Kısmi G2 durumu açıkça beyan edildi.
- **45/45 SKR sayımı yeniden yapılmadı** — SCAN-03 §5.1'den **devralındı** ve
  `work-protocol.md` §3.5 gereği bu devralma **işaretlenmiştir**.
- **Hiçbir araç çalıştırılmadı.** `dotnet test`, `git log`, linter koşusu yok. §8.3'teki
  "0/15 verirdi" bir **beklentidir**, koşu çıktısı değil. Hiçbir sonuç fabrike edilmemiştir
  (SKR-041 emsali).
- **Hiçbir dosya değiştirilmedi.** Bu kayıt salt incelemedir; 9 talebin (A-I) hiçbiri bu
  belgeyle yürürlüğe girmez — her biri ilgili owner + Madde XIV/XV yordamı ister.
