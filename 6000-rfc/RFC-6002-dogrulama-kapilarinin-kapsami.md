---
id: RFC-6002
type: rfc
canon: false
status: draft
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-27
depends_on: [ENS-0000, GOV-000, GOV-030, RFC-6001]
referenced_by: []
skeptic_review: pending
failure_conditions: stated
principles: [P6, P7]
---

# RFC-6002 — Doğrulama Kapılarının Kapsamı: `ratified` ≠ `Canonical`

> **Öneri edimidir, norm değildir.** Kabul edilirse normu GOV-000, `maturity-model.md` ve
> `canonical-process.md`'ye yazar; kendisi tarihsel kayda döner (RFC-6001 emsali).

## 1. Problem

Külliyat'ın doğrulama kapıları hakkında **üç ayrı belge üç ayrı şey söylüyor gibi görünüyor**:

| Belge | Der ki |
|---|---|
| `.claude/standards/maturity-model.md:28` | M2 = Scientific skeptic'ten **survives (≥1 SKR)** → `status: ratified` |
| `governance/000-governance-principles.md:36` | **G4** — Her **Canonical** yapıtın **≥2 bağımsız** validator'ı vardır (farklı boyutlardan) |
| `governance/canonical-process.md:45` | **G4:** ≥2 bağımsız boyut validator'ı (Scientific + Ontology, uygunsa + Engineering) |

2026-07-27'de yapılan kapı-uyum taraması (`governance/SCAN-03-gate-compliance.md`) bunu bir
**normatif çatışma** (Ç-01) olarak kaydetti ve şu sonucu üretti: 9 `ratified` yapıtın yalnız
6'sı G4'ü sayıca, **hiçbiri** boyut olarak sağlıyor.

**Bu sonuç yanlıştı — ve yanlışlığı bu RFC'nin çekirdeğidir.**

## 2. Çözüm: çatışma yok, terim karışıklığı var

`ratified` ile `Canonical` **aynı şey değildir.**

- **`ratified` bir `status`'tür** — M2, M3, M4 ve M5'in **ortak** durum etiketi
  (`maturity-model.md:28-31`). M2'ye ulaşan bir yapıt `ratified` olur.
- **`Canonical` bir `canon: true` alanıdır** — ve `maturity-model.md:34` bunu açıkça
  sınırlar: **"`canon: true` yalnızca M5'tir. Skeptic-survives (M2/M3) Canon yapmaz."**

G4'ün öznesi *"her **Canonical** yapıt"*tır. Dolayısıyla:

> **≥1 SKR → `ratified` (M2).  ≥2 bağımsız boyut → `canon: true` (M5).**
> İki kural farklı kapıları yönetir. Çelişmiyorlar.

### 2.1 Bu okumanın iki bağımsız türetmesi var

Anayasa Madde X ve GOV-000 G4 gereği bu iddianın kendisi de doğrulanmalıdır. İki ayrı
context, birbirini görmeden aynı sonuca vardı:

1. `ens-philosopher`, SCAN-03'ün Ç-01 maddesini çözerken (2026-07-27).
2. Oturum sahibi, RFC yazmadan önce alıntıları `dosya:satır` doğrularken — ki bu doğrulama
   sırasında raporun atıf yaptığı iki dosyanın **o yolda bulunmadığı** ve satır
   numaralarının **kaymış olduğu** da ortaya çıktı.

### 2.2 Alternatif okumanın maliyeti

Karşıt okuma (G4 `ratified`'ı da bağlar) kabul edilirse, Külliyat'ın **kendi beyanı**
—*"Canon şu an boştur, azami olgunluk M3'tür, bu doğru durumdur"*— bir anda **dokuz ihlal
beyanına** dönüşür. Bir yorum, yorumladığı metni toplu ihlale çeviriyorsa önce yorumdan
şüphelenilir.

## 3. Ama gerçek bir boşluk var: `review → ratified` geçişini hiçbir kural yönetmiyor

Ç-01 çözüldüğünde altından **asıl sorun** çıkıyor.

`roles.md` G2/G3'ü koşulsuz uygular gibi yazılmıştı; GOV-000 ise onları açıkça
**Canonical** ile sınırlar. GOV-000 kazanır — çünkü `roles.md` ondan **türer**
(`GOV-000:22`: *"Roller, yetkiler ve süreçler bu ilkelerden türer"*), türev kaynağını
genişletemez. Aynı yönde bir emsal 2026-07-27'de zaten yaşandı: `roles.md`'nin G2 türevi
(*"kendi işini doğrulayan olamaz"*) **yanlıştı** ve GOV-000 lehine düzeltildi
(`governance/SCAN-01-authority-citations.md`).

Sonuç: G2/G4 **M5-kapsamlıdır**. Ve bu, `review → ratified` geçişini yöneten **hiçbir yazılı
kural bırakmaz.**

Bugün o geçiş fiilen **SKR verdict'i tarafından tetikleniyor** — yani *doğrulama, onayın
yerine geçiyor*. Bu tam olarak **G3'ün yasakladığı şeydir** (*"Validation ve approval
ayrıdır"*). Kusur bir kural dosyasında değil, **atanmamış bir onay makamındadır**:
`governance/roles.md` bugün hâlâ *"Governance body: fazı gelince"* diyor. G3 bir ayrım
istiyor; ayrımın bir tarafı boş.

### 3.1 Önerilen: hafif ratifikasyon kapısı

`review → ratified` için, `canon`'un ağır kapısından **ayrı** ve ondan hafif bir kapı:

> **R1.** En az bir `survives` verdict'i taşıyan bağımsız inceleme.
> **R2.** Yazardan **farklı** bir aktörün, **kayda geçen** ratifikasyon edimi.
> **R3.** Edim künyede görünür: `ratified_by` ve `ratified_at` alanları (bkz. §6, açık iş).

R2 olmadan G3 hiçbir statüde sağlanamaz. R3 olmadan G5 (*"sessiz karar yoktur"*) ihlal
edilir — **bugün 9 `ratified` Külliyat yapıtının hiçbirinde onaylayan kayıtlı değildir.**

## 4. Ç-03: M5'in boyut listesi

İki liste var ve **alternatif değil, birleşimdirler**:

- `maturity-model.md:31` — Scientific + Engineering + Business + Ethical (**Ontology yok**)
- `canonical-process.md` zinciri — Scientific + **Ontology** + Engineering

`maturity-model.md`'nin "dört-skeptic"i, `validation-framework.md`'nin **beş** boyutu
yazılmadan önceki bir **sayı-sürüklenmesidir**. Ontology'nin dışarıda kalması olgusal olarak
da savunulamaz: Külliyat'ın SKR'lerinin önemli bir bölümü (ENS-4010/4025/4030/4031 turları)
fiilen bu boyutta üretilmiştir.

> **Öneri:** sabit sayı yerine **"tüm aktif boyutlar"** yazılsın. Bir boyut "aktif"tir ancak
> ve ancak ROSTER'da o boyutun validator rolü **atanmışsa**. Böylece liste, kadro
> gerçekliğiyle otomatik senkron kalır ve bir boyutun atanmamış olması sessizce
> muafiyete dönüşmez — **görünür bir borç olur** (bkz. ROADMAP G-27: bugün sıfır ethical SKR).

## 5. Neden bu üçü tek RFC'de

RFC-6001 emsali: Madde IV + künye şeması **atomik** değiştirildi, çünkü biri diğeri olmadan
tutarsız bir ara durum bırakıyordu. Burada da öyle:

- Ç-01 tek başına çözülürse, `review → ratified`'ı yöneten kural **hiç** kalmaz (§3).
- Ç-02 tek başına çözülürse, hangi kapının hangi statüyü yönettiği belirsiz kalır.
- Ç-03 tek başına çözülürse, çözülen kapının **ölçütü** eksik kalır.

Ama Ç-04 (ethical aktivasyonu) ve Ç-05 (kurucu yolun boyutları) **bu RFC'ye dahil değildir** —
onlar boyut sözlüğü ve validator kadrosu meselesidir, farklı owner ve farklı yanılma kipi
taşırlar. RFC-6003'e bırakıldılar. Gerekçe: CEO-0002'nin RFC-6001 üzerine düştüğü
**kapsam-orantısı** gözlemi, tek dev RFC'ye karşı uyarır.

## 6. Değişecek metinler (kabul edilirse)

| Belge | Değişiklik |
|---|---|
| `governance/000-governance-principles.md` | G4'e açık kapsam cümlesi: *"G4'ün öznesi `canon: true`'dur; `status: ratified` bunu tetiklemez."* |
| `.claude/standards/maturity-model.md` | M5 boyut listesi → "tüm aktif boyutlar"; M2→`ratified` yolunun R1-R3 kapısına bağlanması |
| `governance/canonical-process.md` | `review→ratified` kapısının (R1-R3) eklenmesi; bugün yalnız canonization'ı tarif ediyor |
| `.claude/standards/metadata-header.md` | `ratified_by`, `ratified_at` alanları (**bu RFC'nin kapsamında değil** — `ens-style-guardian` owner'lığında ayrı edim; burada yalnız *gereksinim* olarak kaydedilir) |

## 7. Reddedilen alternatifler

**A. Hiçbir şey yapma — Ç-01 zaten çatışma değilse sorun yok.**
Reddedildi: §3'teki boşluk gerçektir ve Ç-01'in çözümüyle **açığa çıkar**, kapanmaz.
Doğrulamanın onayın yerine geçmesi bugün fiilen oluyor.

**B. G4'ü `ratified`'a da genişlet.**
Reddedildi: §2.2 — Külliyat'ın kendi doğru beyanını dokuz ihlale çevirir ve M2/M5 ayrımını
anlamsızlaştırır. Ayrıca hiçbir metin bunu söylemiyor; bu bir **genişletme** olurdu, bir
yorum değil.

**C. Beş çatışmayı tek RFC'de çöz.**
Reddedildi: §5 — farklı owner, farklı yanılma kipi. RFC-6001'in CEO hiza-incelemesi
kapsam-orantısı konusunda açıkça uyarmıştı.

## 8. Failure conditions (Madde X)

Bu RFC **yanlıştır** eğer:

1. **`maturity-model.md:34` bulunamazsa ya da başka bir şey söylüyorsa.** Tüm §2 o tek
   cümleye (*"`canon: true` yalnızca M5'tir"*) dayanır. O cümle kaldırılırsa RFC çöker.
2. **Külliyat'ta `canon: true` olup M5 olmayan bir yapıt meşru sayılıyorsa** — bu durumda
   `ratified`/`Canonical` ayrımı pratikte uygulanmıyordur ve ayrım tanımsal değil,
   temenni olur. *(Bugün böyle **iki** yapıt var: ENS-4000 ve — 2026-07-27'ye kadar —
   ENS-3000. İkisi de borç olarak kayıtlı: ROADMAP G-24, G-25. Yani bu koşul kısmen
   gerçekleşmiş durumdadır ve RFC bunu gizlemiyor.)*
3. **R2'yi (yazardan farklı onaylayan) uygulayacak aktör hiç atanmazsa.** Kural yazılıp
   uygulanamıyorsa, kâğıt üzerinde kapatılmış bir boşluk gerçekte açık kalır — ve bu,
   boşluğu **kayıtlı** hâlinden daha kötüdür.
4. **"Tüm aktif boyutlar" tanımı, hiçbir boyutun atanmadığı bir durumda M5'i sıfır
   validator'la geçirilebilir kılarsa.** Bu, kuralın kendi kaçağı olurdu; kabul edilmeden
   önce kapatılmalıdır.

## 9. Açık sorular

1. R2'yi kim yürütür? SCAN-03 Ö-03'ün önerisi: RFC-6001'in kanıtlanmış çift-owner deseni
   (`ens-ceo` hiza-onayı + `ens-style-guardian` şema-imzası) Külliyat ratifikasyonuna
   genişletilsin. Bu RFC bunu **önerir, karara bağlamaz** — rol ataması GOV-010 alanıdır.
2. Geçmişe dönük uygulanır mı? Mevcut 9 `ratified` yapıt R2/R3'ü taşımıyor. Toplu geriye
   dönük ratifikasyon mu, yoksa "bu tarihten sonrası" mı?
3. `ratified_by` künye alanı bu RFC ile mi gelir, ayrı bir şema sürümüyle mi?

## 10. Bu RFC'nin kendi yolu

`constitutive: true` bir belgeyi doğrulamadan muaf tutmaz (RFC-6001 §Kritik nokta, `:175`).
Bu RFC de muaf değildir: en az bir bağımsız `ens-skeptic` turu, ardından Madde XIV yordamı.
**Yazarı kendi turunu `survives` işaretleyemez** (GOV-000 G2 + G4).
