---
id: RFC-6002
type: rfc
canon: false
status: draft
owner: fikri-eren            # G5: fiilî yazar. v0.1.0'da `ens-philosopher` yazıyordu — yanlıştı (SKR-047 D-13)
version: 0.2.0
last_reviewed: 2026-07-27
depends_on: [ENS-0000, GOV-000, GOV-030, RFC-6001]
referenced_by: []
skeptic_review: [SKR-047, ARCH-0001]
failure_conditions: stated
principles: [P6, P7]
---

# RFC-6002 — Doğrulama Kapılarının Kapsamı: `ratified` ≠ `canon`

> **Öneri edimidir, norm değildir.** Kabul edilirse normu GOV-000 ve `canonical-process.md`'ye
> yazar; kendisi tarihsel kayda döner (RFC-6001 emsali).

---

## 0. v0.1.0 → v0.2.0 — neyin neden değiştiği

v0.1.0 iki bağımsız incelemeden **ikisinden de** `wounded` aldı ve **ikisi de aynı** bloke
edici bulguya vardı, birbirini görmeden:

| İnceleme | Boyut | Verdict |
|---|---|---|
| `SKR-047` | Scientific | `wounded` — 5 bloke edici |
| `ARCH-0001` | Architectural | `yapısal kusurlu` — 3 bloke edici |

**Bu yakınsama bu RFC'nin konusunun kendisidir:** farklı boyutlar aynı kusuru bulunca teyit
olur, farklı kusurlar bulunca asıl değer çıkar. İkisi de gerçekleşti.

| # | Bulgu | v0.2.0'da |
|---|---|---|
| Temel taşı bayat (`SKR-047` D-1 · `ARCH-0001` D-2.3) | §2 `maturity-model.md:34`'e dayanıyordu; o satırı **RFC-6001 aştı** | §2 tümüyle yeniden yazıldı, temel **Madde IV + RFC-6001 §7.2** |
| Tek-aktör kaçağı (`SKR-047` D-5) | *"Tüm aktif boyutlar"* G4'ü tek aktörle geçilebilir kılıyordu | §4'e iki koruma cümlesi |
| Sahte bağımsızlık (`SKR-047` D-10) | *"İki bağımsız türetme"* iddiası savunulamaz | §2.1 **geri çekildi** |
| Atomiklik başarısız (`ARCH-0001` D-3.1/3.2) | 6002 tek başına kabul edilirse sistem **gevşer** | §5 kabul sırası tersine: **6003 → 6002 → 6004** |
| R2 uygulanamaz (`ARCH-0001` D-4.1) | Önerdiği aktörün ajan dosyası yok | §3.1 "farklı aktör" → **ayrı edim** |
| G2 hatası tekrarı (`SKR-047` D-6) | §10, SCAN-01'in öldürdüğü hatayı yeniden üretti | Düzeltildi (§9 kutusu) |
| Geriye akıl yürütme (`SKR-047` D-11) | §2.2 "9 ihlal üretir, o hâlde yanlış" | **Kaldırıldı** |
| Owner uyumsuzluğu (`SKR-047` D-13) | Künye `ens-philosopher`, yazan başkası — **G5** sorunu | Künye düzeltildi |

Silinen bölümler **geri getirilmeyecek** ama v0.1.0 git geçmişinde durur (EC-001).

---

## 1. Problem

Külliyat'ın doğrulama kapıları hakkında farklı belgeler farklı şeyler söylüyor gibi görünüyor
ve 2026-07-27 kapı-uyum taraması (`governance/SCAN-03-gate-compliance.md`) bunu bir
**normatif çatışma** (Ç-01) olarak kaydetti: 9 `ratified` yapıtın yalnız 6'sı G4'ü sayıca,
**hiçbiri** boyut olarak sağlıyor.

**O ölçüm yanlış bara karşı yapılmıştı.** Bu RFC neden yanlış olduğunu ve doğru barın ne
olduğunu yazar.

---

## 2. Çözüm: `canon`'a İKİ yol vardır

`ratified` bir `status`'tür; `canon` ayrı bir alandır. Ama v0.1.0'ın sandığı gibi
*"canon = M5"* **değildir** — Anayasa Madde IV, RFC-6001 ile değiştirilmiş hâliyle
(`ENS-0000:107-115`) **iki ayrı yol** tanımlar:

| Yapıt türü | `canon: true` yolu | M5'e tabi mi |
|---|---|---|
| **`constitutive: true`** (Anayasa, kurucu tezler, yasa-çerçevesi, tip/şema, governance) | **Ratifikasyon** — failure conditions'ını *tutarlılık/örneklenebilirlik* kipinde yazar, `ens-skeptic` tutarlılık incelemesinden sağ çıkar | **HAYIR** — *"Ampirik kanıt zincirine (M5 / Faz-4) tabi değildir, çünkü ampirik iddia taşımaz"* |
| **`constitutive: false`** (ampirik teori ve yasalar) | Failure conditions + skeptic → `ratified` (M3); **tam Canon (M5)** yalnızca reference platform ile | **EVET** |

`RFC-6001:341-345` (§7.2) bunu künye kuralı olarak yazar:
> *"`constitutive: false` (ampirik) yapıtta **yalnızca `maturity: M5` ise** true;
> `constitutive: true` (kurucu) yapıtta **ratifiye edilip skeptic tutarlılık incelemesinden
> sağ çıkınca** true — maturity/evidence eksenine tabi değil."*

Ve `RFC-6001:169` örneği verir: `constitutive: true, canon: true` — *"ör. ENS-0000, ENS-4000"*.

### 2.1 Bunun G4 için sonucu

G4'ün öznesi *"her **Canonical** yapıt"*tır (`GOV-000:36`). Dolayısıyla:

> **≥1 SKR → `ratified` (M2).**
> **`canon: true` → yola göre değişir:** ampirik yapıtta M5 + ≥2 bağımsız boyut;
> kurucu yapıtta ratifikasyon + tutarlılık incelemesi.

`ratified` yapıtları G4 ile ölçmek **kategori hatasıdır** — ve SCAN-03'ün "6/9" ölçümü tam
olarak bu hatayı yapmıştır. Doğru soru şudur: **`canon: true` taşıyan 4 yapıt kendi yolunu
tamamlamış mı?**

### 2.2 ⚠️ v0.1.0'ın "iki bağımsız türetme" iddiası — GERİ ÇEKİLDİ

v0.1.0 bu okumayı *"iki bağımsız context aynı sonuca vardı"* diye sundu. **Savunulamaz**
(`SKR-047` D-10): her ikisi de aynı kaynağı (`SCAN-03:587`) okudu ve **ardışık** çalıştılar
(`ROADMAP:240`). Dürüst ifade:

> Bir **yorumsal türetme** (`ens-philosopher`, ROADMAP G-26) ve onun girdilerinin bağımsız
> **alıntı denetimi**. Bu bir **kaynak doğrulamasıdır**, G4 anlamında **ikinci bir validator
> değildir**.

v0.1.0 ayrıca *"alternatif okuma 9 ihlal üretir, o hâlde yanlıştır"* diye akıl yürütüyordu.
Bu **sonuçtan geriye akıl yürütmedir** ve RFC'nin kendi failure condition'ı tarafından
çürütülüyordu (`SKR-047` D-11). **Kaldırıldı.** Okuma, rahatsız edici sonuç üretmediği için
değil, **Madde IV metni öyle dediği için** doğrudur.

### 2.3 Yeni gerilim: `GOV-000:47` "Canon boş" diyor, ama 4 yapıt `canon: true`

> `GOV-000:47-48` — *"Engineering Validation Faz 4'ü gerektirdiğinden **M5 şu an ulaşılamaz —
> Canon boş.** Bu, ilkelerin doğrudan ve dürüst sonucudur."*

Bugün `canon: true` taşıyan **4** yapıt var: `ENS-0000`, `ENS-1000`, `ENS-4000`,
`.claude/standards/traceability.md`.

Çelişki değil, **bayatlık**: GOV-000'ün bu cümlesi RFC-6001'in Madde IV değişikliğinden
**önce** yazılmıştır ve yalnız ampirik yolu tarif eder. Kurucu yol açıldıktan sonra "Canon
boş" ifadesi olgusal olarak yanlıştır. **v0.2.0 bunu bir talep olarak kaydeder** (§6).

---

## 3. Asıl boşluk: `review → ratified` geçişini hiçbir kural yönetmiyor

> **Ad uyarısı (`SKR-047` D-4):** ENS'te *"`review → ratified` kapısı"* diye adlandırılmış
> bir mekanizma **yoktur**. Bu RFC o adı **kurar**; var olan bir şeyi tarif ettiğini iddia
> etmez.

G2/G4, GOV-000'de **Canonical**'a bağlıdır. `roles.md` onları koşulsuz uygular gibi
yazılmıştı; ama `roles.md` GOV-000'den **türer** (`GOV-000:22`) ve türev kaynağını
genişletemez. Aynı yönde emsal: `roles.md`'nin G2 türevi 2026-07-27'de **yanlış** bulunup
GOV-000 lehine düzeltildi (`governance/SCAN-01-authority-citations.md`).

Sonuç: `review → ratified` geçişi **yazılı hiçbir kurala tabi değil.** Bugün fiilen **SKR
verdict'i** tetikliyor — yani *doğrulama, onayın yerine geçiyor*, ki bu **G3'ün yasakladığı
şeydir**.

### 3.1 Önerilen: hafif ratifikasyon kapısı — **aktör değil, EDİM ayrımı**

v0.1.0 *"yazardan farklı bir aktör"* istiyordu. `ARCH-0001` D-4.1 bunu **doğuşta
uygulanamaz** buldu: önerilen yürütücü `ens-ceo`'nun **ajan dosyası yok** (ROSTER'da
"Ertelenmiş").

Ama aynı inceleme deponun sorunu **zaten çözdüğünü** gösterdi:

| Faz | Onaylayanı kayıtlı yapıt |
|---|---|
| Faz 3 (ADR/RFC) | **3/3** — CEO-0001/0002/0003 + STYLE-SIGNOFF-RFC-6001 |
| Faz 0-2 (Külliyat) | **0/9** |

Çalışan fark **farklı kişi** değil, **ayrı edim**: ayrı dosya, ayrı lens, ayrı kayıt.
R2 buna göre yeniden yazıldı:

> **R1.** En az bir `survives` verdict'i taşıyan bağımsız inceleme.
> **R2.** Ratifikasyon **ayrı bir edim** olarak yapılır: (a) doğrulama turundan **ayrı bir
> kayıtta**, (b) **ayrı bir context**te üretilmiş, (c) yapıta ve kayda **iki yönlü iz**
> bırakan. Aktör kimliği GOV-010'a aittir; bu RFC onu **atamaz**.
> **R3.** Edim künyede görünür: `ratified_by`, `ratified_at` (şema edimi — `ens-style-guardian`).

R2'nin bu hâli **bugün uygulanabilir**: CEO-\* ve STYLE-SIGNOFF deseni tam olarak budur ve
Faz-3'te 3/3 çalışmıştır. v0.1.0'ın hâli uygulanamazdı.

---

## 4. M5'in boyut listesi — ve tek-aktör kaçağının kapatılması

`maturity-model.md`'nin dört-boyut listesi ile `canonical-process.md`'nin zinciri **alternatif
değil, birleşimdir**; ilki `validation-framework.md`'nin beş boyutu yazılmadan önceki bir
**sayı-sürüklenmesidir**. Ontology'nin dışarıda kalması olgusal olarak da savunulamaz —
ENS-4010/4025/4030/4031 turları fiilen o boyutta üretilmiştir.

> **Öneri:** sabit sayı yerine **"tüm aktif boyutlar"**.

### 4.1 ⚠️ İki koruma cümlesi — bunlar olmadan öneri kendi deliğini açar

`SKR-047` D-5 gösterdi ki v0.1.0'ın formülü G4'ü **tek aktörle geçilebilir** kılıyor ve
kadrosu olmayan boyutu **muaf** gösteriyordu. Yani kapatmayı vaat ettiği deliği açıyordu.
İki cümle zorunludur:

> **(1)** *Aynı aktörün ürettiği farklı-boyutlu kayıtlar **tek** validator sayılır.
> G4 farklı **aktör** ister, farklı **etiket** değil.*
>
> **(2)** *Aktif olmayan bir boyut **muaf değildir, ertelenmiştir**; ROADMAP'te açık borç
> satırı taşır ve o satır kapanmadan ilgili yapıt `canon: true` olamaz.*

(2) olmadan bu RFC, `ROADMAP` **G-27**'nin (sıfır ethical doğrulama borcu) normatif
dayanağını yok ederdi.

---

## 5. Kabul sırası — **ilan edilenin TERSİ**

v0.1.0 `6002 → 6003` diyordu. `ARCH-0001` D-3.1/3.2 bunu somut bir testle çürüttü:

> **RFC-6002 tek başına kabul edilirse sistem sıkılaşmaz, GEVŞER.**
> *"Tüm aktif boyutlar"* bugün `{Scientific, Ontology}`'ye çözülüyor (`roles.md:61`) ve
> **ikisi de `ens-skeptic`e atanmış**. G4-boyut uyumu **hiçbir yeni doğrulama yapılmadan**
> %0'dan ~%40'a sıçrar (ENS-4001/4010/4020/4025/4030/4031).

Yaması yalnızca RFC-6003 §4'tedir (Ontology Validator'ın ayrılması). Yani bağımlılık
v0.1.0'ın ilan ettiği gibi tek yönlü değil, **çift yönlüdür**.

> **Doğru sıra: `RFC-6003` → `RFC-6002` → `RFC-6004`.**
> Alternatif: üçü RFC-6001 emsaliyle **tek paket** olarak kabul edilir.

---

## 6. Değişecek metinler (kabul edilirse)

| Belge | Değişiklik |
|---|---|
| `governance/000-governance-principles.md` | G4'e kapsam cümlesi: *"G4'ün öznesi `canon: true`'dur; `status: ratified` bunu tetiklemez."* **Ayrıca `:47`'nin "Canon boş" cümlesi bayattır** — kurucu yol açıldıktan sonra olgusal olarak yanlış (§2.3) |
| `governance/canonical-process.md` | `review→ratified` kapısı (R1-R3) eklenir; bugün yalnız canonization'ı tarif ediyor |
| `.claude/standards/maturity-model.md` | M5 boyut listesi → "tüm aktif boyutlar" + §4.1'in iki koruma cümlesi. **`:34` ("canon yalnızca M5") RFC-6001 §10.5 gereği zaten hizalama bekliyor** — bu RFC o satıra dokunduğu için **hizalama borcunu da kapatmalı** (`SKR-047` T-E) |
| `.claude/standards/metadata-header.md` | `ratified_by`, `ratified_at` — **bu RFC'nin kapsamında değil**, `ens-style-guardian` edimi; burada yalnız gereksinim olarak kaydedilir |

---

## 7. Reddedilen alternatifler

**A. Hiçbir şey yapma.** Reddedildi: §3'teki boşluk gerçektir ve Ç-01'in çözümüyle
**açığa çıkar**, kapanmaz. Doğrulamanın onayın yerine geçmesi bugün fiilen oluyor.

**B. G4'ü `ratified`'a da genişlet.** Reddedildi: Madde IV iki yol tanımlar ve `ratified`
ikisinin de ara durağıdır. Hiçbir metin G4'ü `ratified`'a bağlamıyor; bu bir **genişletme**
olurdu, yorum değil.

**C. Beş çatışmayı tek RFC'de çöz.** Reddedildi: farklı owner, farklı yanılma kipi.
CEO-0002'nin kapsam-orantısı uyarısı geçerli.

---

## 8. Failure conditions (Madde X)

Bu RFC **yanlıştır** eğer:

1. **Madde IV'ün iki-yollu canon modeli kaldırılırsa ya da başka bir şey söylüyorsa.**
   §2'nin tamamı `ENS-0000:107-115` + `RFC-6001:341-345`'e dayanır.
2. **`constitutive: false` olup M5 olmadan `canon: true` taşıyan bir yapıt meşru sayılıyorsa.**
   *(v0.1.0'ın fc-2'si "canon+non-M5 varsa RFC yanlış" diyordu — bu koşul `ENS-0000`'in
   kendisi tarafından **tetiklenmişti**, çünkü ENS-0000 kurucudur ve meşru biçimde
   canon'dur. Koşul `constitutive: false` ile sınırlandırıldı; `SKR-047` D-3.)*
3. **R2'yi uygulayacak bir edim ayrımı hiç kurulmazsa.** Kâğıtta kapatılmış bir boşluk
   gerçekte açık kalırsa, **kayıtlı** hâlinden kötüdür.
4. **§4.1'in iki koruma cümlesi olmadan "tüm aktif boyutlar" kabul edilirse.** O hâlde
   kural kendi kaçağını taşır ve G-27'nin dayanağını yok eder.
5. **Kabul sırası §5'in tersine uygulanırsa** — RFC-6002 tek başına yürürlüğe girerse
   G4-boyut uyumu sahte biçimde yükselir.

---

## 9. Açık sorular

1. R2'nin aktörü kim? `SKR-047` T-F önerisi: *"GOV-010 aksini söyleyene kadar
   `ens-style-guardian` (Custodian); hiza gerektiren yapıtlarda ek olarak `ens-ceo`."*
   Bu RFC **önerir, atamaz**.
2. Geriye dönük uygulanır mı? Mevcut 9 `ratified` yapıt R2/R3 taşımıyor.
3. `GOV-000:47`'nin bayat "Canon boş" cümlesi bu RFC ile mi düzeltilir, ayrı bir edimle mi?

---

## 10. Bu RFC'nin kendi yolu

`constitutive` bir belgeyi doğrulamadan muaf tutmaz (`RFC-6001:175`). v0.2.0 için gereken:
**yeni** bir `ens-skeptic` turu + **farklı boyuttan** ikinci tur (GOV-000 **G4**).

**Yazarı kendi turunu `survives` işaretleyemez** — dayanak **GOV-000 G4** (≥2 bağımsız
validator) ve **G3** (`GOV-000:35` — doğrulayan onaylamaz). **G2 DEĞİL.**

> ### ⚠️ Düzeltme — `SKR-047` D-6 (2026-07-27), kalıcı
> Bu satır v0.1.0'da *"(GOV-000 **G2** + G4)"* diyordu. **Yanlıştı** ve yanlışlığı özellikle
> utandırıcı: G2 **kanonlaştırma** yasağıdır, **doğrulama** yasağı değil.
>
> Utandırıcı olan şu: v0.1.0 **§3'te** `governance/roles.md`'deki tam bu G2 hatasının
> düzeltilmesini **kendi argümanının emsali olarak gösteriyordu** — ve 108 satır sonra
> ailenin yeni bir üyesini üretiyordu. Hata kaynakta bulundu, düzeltildi, emsal gösterildi,
> sonra tekrarlandı.
>
> `SKR-047` bunu *"en sert bulgu tipi: RFC, teşhis ettiği hatayı teşhisin hemen yanında
> tekrarlıyor"* diye adlandırdı. Katılıyorum. Kutu kalıcıdır (EC-001) — kalıbın ne kadar
> yapışkan olduğunun kanıtı budur.
