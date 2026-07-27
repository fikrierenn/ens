---
id:            SKR-051
title:         ADR-0003 v0.5.0 — bölünmüş kapsam saldırısı
type:          skeptic-review
origin:        ADR-0003 v0.5.0 (§0.10 — ADR BÖLÜNDÜ)
depends_on:    [ADR-0003, SKR-049, SKR-050, ENG-0001, ENG-0002]
referenced_by: []
status:        draft
owner:         ens-skeptic
version:       0.1.0
last_reviewed: 2026-07-27
maturity:      M0
dimension:     Scientific
failure_conditions: stated
evidence:      {sci: E2, eng: E1, ops: E0, econ: E0}
---

# SKR-051 — ADR-0003 v0.5.0 Saldırısı

> **Bağımsızlık beyanı (GOV-000 G2/G3):** ADR-0003'ün sahibi `ens-ai-architect`; bu kayıt
> `ens-skeptic` rolüyle, ayrı context'te üretildi. **G3 gereği bu verdict bir *validation*'dır,
> *approval* değildir** — statü değişimini owner onaylar.
>
> **Bahis kaydı:** bu tur `Accepted`'a gidebilir. Anayasa Madde VII: *kod yalnız Accepted
> ADR'lere dayanır.* Bu yüzden "draft, sonra düzeltilir" toleransı **uygulanmadı**; her bulgu
> "bu belge bugün Accepted olsa, `Ens.Kernel`'e ne yazılırdı?" sorusuyla sınandı.
>
> **Yöntem:** `work-protocol.md` §3.5'in dört kontrolü — dosya var mı · satır onu mu söylüyor ·
> bulgu iddiayı taşıyor mu · **sayı yeniden hesaplandı mı**. Devralınan hiçbir bulgu
> doğrulanmadan kullanılmadı. §3.2 gereği `grep -a` kullanıldı (4 NUL baytı tuzağı).

## Verdict

**`refuted`** — v0.5.0'ın bölme işlemi **ilan edildi, gövdeye uygulanmadı**: K-1 ve K-2 hâlâ
§3'ün kapsam beyanında, §4.1/§4.2'de tam karar cümleleriyle, §6 denetim tablosunda, §7
yanlışlanma koşulunda, §8 `// TRACE:` zorunluluğunda ve künye `provides:` alanında normatif
olarak duruyor. **Bu, ADR'nin kendi sicilinde "changelog ≠ gövde" kalıbının altıncı
tekrarıdır** — ve ilk beşinden farklı olarak bu sefer `Accepted`'a gitmek üzeredir.

Ayrıca bölmeyi meşrulaştıran **sonlanma ölçütü**, ENS'in kendi kaydıyla (`SCAN-03:433-436`)
**ampirik olarak çürütülmüştür** ve uygulanışı üç yerde ölçütü kendi kendine sağlatacak
biçimde seçilmiştir.

> **Neden `wounded` değil:** yaralı bir belge, doğru kararlar taşıyıp eksik gerekçeye
> sahip olandır. Burada durum farklıdır: **belge bugün Accepted olsa, `Ens.Kernel`'e
> `// TRACE: ADR-0003 K-1` yazılırdı** (§8, satır 1417) — hâlbuki `ADR-0004:106` aynı kararın
> devredildiğini, `ADR-0004:29` ise öncülünün çöktüğünü söylüyor. İki Accepted ADR aynı kararı
> çelişik güçte iddia eder. Bu bir eksiklik değil, **normatif çatışmadır** ve Madde VII'yi
> doğrudan kırar.
>
> **Bu verdict ucuz değildir ve kararların kendisine ait değildir.** K-4/K-5/K-6'nın *içeriği*
> hakkındaki kanaatim §K bölümünde ayrıdır: üçü de savunulabilir kararlardır. Çürütülen şey
> **bu belgenin bugünkü hâlinin Accepted olabileceği** iddiasıdır. Ayırım önemlidir: mekanik
> bir gövde temizliği + üç talep, verdict'i tersine çevirebilir.

## Bulgu tablosu

| # | Bulgu | Şiddet | Tur içi durum |
|---|---|---|---|
| **B-1** | Bölme ilan edildi, **gövdeye uygulanmadı** — K-1/K-2 yedi yerde normatif | ⛔ bloke edici | **yeni** |
| **B-2** | Sonlanma ölçütü ENS'in kendi kaydıyla çürük (`SCAN-03:433`) **ve** üç yerde kendine göre uygulanmış | ⛔ bloke edici | **yeni** |
| **B-3** | K-4 "tuttu" **cherry-pick** — `ENG-0002` E-1.7 K-4'e turda **yeni bloke edici** verdi | ⛔ bloke edici | **yeni** |
| **B-4** | K-5 **hiç incelenmedi**; "yeni bulgu yok" = yokluğu kanıt saymak | ⛔ bloke edici | **yeni** |
| **B-5** | Bölme, K-4'ü K-1'e olan **mekanizma bağımlılığından** kopardı (atomiklik) | ⛔ bloke edici | **yeni** |
| **B-6** | 22 **beşinci** sayıdır ve yalnız changelog'da; §7 hâlâ **40** diyor | ⛔ bloke edici | **devralındı, kapanmadı** |
| **B-7** | K-3'ün koşulu Accepted ile bağdaşmıyor — üç yüzeyin ikisi ADR'nin **kendi kapattığı sınıflar** | ⛔ bloke edici | **yeni** |
| **B-8** | OQ1 açıkken K-5/K-6 Accepted olamaz — ADR bunu **kendisi** söylüyor | ⛔ bloke edici | **yeni** |
| **B-9** | D-7'nin ilan edilmemesi dürüstlük değil: künye `principles:` yeni kapsamla **aktif çelişkide** | ⚠️ yüksek | **kötüleşti** |
| **B-10** | 22'nin en az üç üyesi ölçümde kapanmıyor / koşullu | ⚠️ yüksek | **yeni** |
| S-1 | `ENG-0002` "aynı rol, ikinci ölçüm" kaydı — G4 sınırı dürüstçe yazılmış | ✅ katılıyorum | — |
| S-2 | §2 devralınan bulgu doğrulaması (dört yanlış atama) hâlâ ADR'nin en sağlam bölümü | ✅ katılıyorum | — |

---

## B-1 ⛔ — Bölme **ilan edildi, gövdeye uygulanmadı**: K-1 ve K-2 yedi yerde normatif

Ana soru şuydu: *bölme dürüst mü, yoksa sorunu gizledi mi?* Cevap üçüncü bir şıktır:
**bölme gerçekleşmedi.** §0.10 bir *changelog*'dur; §3-§9 *normatif gövdedir*. Gövde
dokunulmamış.

### Ölçüm — `grep`, aynı anlık görüntü (v0.5.0, 1442 satır)

| # | Yer | Satır | Ne diyor | Sonuç |
|---|---|---|---|---|
| 1 | Künye `provides:` | 25 | `[**Authority Token**, **Canonical Identity**, Time Acceptance Window, Explicit Policy Value, Sealed Collection, Measured Output]` | K-1 ve K-2'nin çıktıları **hâlâ ADR-0003'ün taahhüdü** |
| 2 | §3 Kapsam | 657-660 | *"Kapsamda: **P1** (11 üye), **P2** (11 üye), P3 (6), P4 (5), P6 (5), P7 (6) … bu ADR **40** iddia ediyor"* | Kapsam bölünmemiş |
| 3 | §4.1 | 694-813 | `### K-1 —` **tam karar cümlesi**, mekanizma, prior art, 11 üyelik kapanma tablosu, maliyet, 3 reddedilen alternatif, R1-R4 | 120 satır normatif metin |
| 4 | §4.2 | 816-940 | `### K-2 —` **tam karar cümlesi**, `CanonicalName` dört adımı, M-3, M-4, 11 üyelik tablo, R5-R8 | 125 satır normatif metin |
| 5 | §6 Meta-kalıp | 1358-1359 | `| **K-1** | private kurucu … | **Sınıf** (derleyici) |` ve `| **K-2** | Tip değişimi … |` | Denetim tablosunda |
| 6 | §7 Failure conditions | 1377 | *"İddia: **K-1…K-6** uygulandığında **40** kimlik kapanır"* | **Yanlışlanma noktası bölünmemiş** |
| 7 | §8 İzlenebilirlik | 1417-1418 | `| **K-1** | // TRACE: **ADR-0003 K-1** (brand/sealer…) |` | **Kod bu izi yazmak ZORUNDA** |

Ek: §7 OQ tablosu (1400-1405) hâlâ **OQ3** (*"K-1 mührü dağıtık kernel'e nasıl taşınır"*),
**OQ4** (`confusables.txt` — K-2), **OQ5** (K-2 Unicode sürümü) ve **OQ6**
(*"Bu **altı** karar hangi sırayla uygulanır? **K-1 ve K-2 birbirine bağlıdır**"*) taşıyor.
Belgenin açık soruları, kapsamdan çıktığı iddia edilen kararlar hakkındadır.

### Neden bu bir "temizlik borcu" değil, **normatif çatışma**

`ADR-0004:106` şöyle diyor: *"`ADR-0003` K-1'i **devretti** — bu belgeye. `ADR-0003` v0.5.0'da
K-3/K-4/K-5/K-6'ya **daraldı**."* Ama daralma yalnız ADR-0004'ün beyanında var; ADR-0003'te
yok. Sonuç, iki belgenin aynı karar hakkında **karşıt güçte** iddia taşımasıdır:

| | `ADR-0003` §4.1 (satır 694-699) | `ADR-0004` (satır 29, 101) |
|---|---|---|
| K-1'in gücü | *"tüketici, mührü **referans eşitliğiyle doğrulamadan** yetkiyi kabul etmez"* | *"hedefi **küçülür** — mühür **kazayla ve gündelik** taklidi durdurur"* |
| §6 sınıf tablosu | *"**Sınıf** (derleyici)"* (1358) | `Unsafe.As` ile **kırıldı**; güven sınırı **process** |

`ADR-0003` §4.1 hâlâ *"güven sınırı **çağrı grafiğidir**"* (satır 708) diyor — `ENG-0002`
E-1.5'in **ölçümle çürüttüğü** cümle. `ENG-0002` T-1 bunu **bloke edici** ilan etmişti; v0.5.0
cümleyi düzeltmedi, **belgeden çıkardığını ilan etti ve çıkarmadı.**

### Madde VII testi — "bu belge bugün Accepted olsa ne olurdu?"

§8, `Accepted` bir ADR'nin kod tarafında **zorunlu** kıldığı iz satırlarını sayar. Satır 1417:

```
// TRACE: ADR-0003 K-1 (brand/sealer; Morris 1973, Miller 2006) — yetki nesnedir, ortam değil
```

Bu iz, `ADR-0004`'ün *"öncül çöktü, hedef küçüldü"* kararının **tersini** kodlar. Madde VII
(*"kod yalnız Accepted ADR'lere dayanır"*) bir tekillik varsayımı taşır: bir karar için **tek
bir** normatif kaynak. v0.5.0 bu tekilliği kırıyor ve kırdığını fark etmiyor.

### Bu, sicilin **altıncı** tekrarıdır — ve ilk beşten farklı olarak Accepted'a gidiyor

ADR kendi kaydında sayıyor: §0.8 (*"aynı ders, aynı oturumda **dördüncü** kez: yazdım ≠ oldu"*,
satır 267), §0.9 D-7 (*"**Beşinci** tekrar"*, satır 376). §0.10'un D-7 bölümü (satır 467-471)
kalıbı **adıyla teşhis ediyor** ve çözümü olarak *"ilan etmemeyi"* seçiyor:

> *"v0.5.0 bu değişikliği ayrı bir edim olarak bırakıyor — ilan edip uygulamamaktansa, **hiç
> ilan etmemek** dürüsttür."*

**İşte ironi:** aynı sürüm, **çok daha büyük** bir değişikliği (kapsamın yarısının devri) ilan
edip uygulamadı. Kalıp, kendisini teşhis eden paragrafın **48 satır üstünde** (satır 420-438)
yeniden gerçekleşti. Bu, D-7'nin "hiç ilan etmemek dürüsttür" gerekçesini de geçersiz kılar:
sorun ilan etmek değil, **ilan ile edimi ayrı bırakmayı bir yöntem hâline getirmektir**.

### Talep T-1 (bloke edici) — B-1 için

§4.1 ve §4.2 gövdeden **çıkarılır** (`ADR-0004`/`ADR-0005`'e referansla değiştirilir); §3
kapsam beyanı DP3/DP4/DP6/DP7'ye indirilir; §6 tablosundan K-1/K-2 satırları silinir; §7'nin
1377. satırı ve OQ3/OQ4/OQ5/OQ6 yeniden yazılır; §8'den K-1/K-2 iz satırları çıkarılır; künye
`provides:` dörde iner. **Ve — bu turun asıl dersi — değişiklik yapıldıktan sonra
`grep -c "K-1"` ile doğrulanır.** §0.8'in `version:` hizalama vakası (satır 262-264) tam olarak
bunun doğrulanmadığı vakadır.

---

## B-2 ⛔ — Sonlanma ölçütü: ENS'in **kendi kaydıyla** çürütülmüş, ve üç yerde kendine göre uygulanmış

Satır 442:

> **Bir karar, iki ardışık turda yeni bloke edici bulgu almazsa kapanır.**

Bir sonlanma ölçütü koymak **doğru bir refleksti** — sonsuz iterasyon gerçek bir risktir ve
§0.10'un teşhisi (*"tur sayısını kararların zorluğuyla değil paketlemeyle şişirdi"*, satır
426-427) haklıdır. İtirazım ölçütün *varlığına* değil, **içeriğine ve uygulanışına**dır.

### B-2.1 — Ölçüt, ENS'in kendi sicilinde **zaten yanlışlanmış**

`governance/SCAN-03-gate-compliance.md:433-436` (doğrulandı, alıntı birebir):

> *"`ROADMAP.md:19` — "**SKR-040 ve SKR-041'in ikisi de bunu kaçırdı**" — ENS-2003 §3a'daki
> `c` çift-sayım hatası **iki ardışık bağımsız scientific turdan da geçti** ve ancak **farklı
> boyuttan** bir denetimle … yakalandı."*

Aynı belge `:634` (G4 satırı) bu vakayı *"G4'ün 'farklı boyutlardan' şartının **ampirik
kanıtı**"* diye kaydediyor.

**Yani ENS'in Külliyat'ında, "iki ardışık tur temiz geçti" ölçütünün gerçek bir teori hatasını
kapatacağı bir vaka kayıtlıdır.** Ölçüt, ENS'in kendi hata sicilini görmezden gelerek yazılmış.

Daha keskini: aynı hata **bu ADR'nin kendi geçmişinde de** gerçekleşti. `SKR-049` (tur 1) ve
`SKR-050` (tur 2) — **ikisi de scientific**, ikisi de `ens-skeptic` — `Unsafe.As` deliğini
bulamadı. Onu bulan `ENG-0002`'dir, **farklı boyut**. K-1 iki *scientific* turdan geçmişti;
ölçüt o gün yürürlükte olsaydı **K-1 kapanırdı** — ve bugünkü `ADR-0004` yazılmazdı.

> **Bu, ölçütün en güçlü ve en somut çürütmesidir: ölçüt, bu ADR'nin en önemli bulgusunu
> (D-1 çöküşü) engellerdi.**

### B-2.2 — Ölçüt **boyut-kör**; G4 boyut-duyarlıdır

`governance/000-governance-principles.md` G4: *"Her Canonical yapıtın **≥2 bağımsız
validator'ı** vardır (**farklı boyutlardan**)."* Ölçüt yalnız **tur sayar**, boyut saymaz.
`ENG-0002` bunu kendi künyesinde dürüstçe yazmış (satır 21-24, 407-409):

> *"`ENG-0002`, `ENG-0001` ile **aynı roldür**. G4 anlamında **ikinci boyut değildir**;
> Engineering'in **ikinci ölçümüdür**."*

ADR bu itirafı §0.9'a **alıntılamış** (satır 405-410) ve **sonlanma ölçütünü kurarken
kullanmamıştır**. K-4 ve K-6 için sayılan "tur 2", `ENG-0002`'dir — yani ölçüt, ADR'nin kendi
belgelediği **boyut tekrarını** bağımsızlık gibi saymaktadır.

### B-2.3 — Ölçüt, **teste tabi olmayan** bir karara ✅ verecek biçimde yazılmış

Ölçütün yazılışı kritik: *"yeni bloke edici bulgu **almazsa**"*. Bu edilgen çatı, iki farklı
olguyu birleştirir:

| Olgu | Ölçüte göre |
|---|---|
| (a) Karar **incelendi**, saldırı denendi, **dayandı** | "bulgu almadı" ✅ |
| (b) Karara **hiç bakılmadı** | "bulgu almadı" ✅ |

(b) bir kanıt değil, **kanıtın yokluğudur**. Ölçüt ikisini ayırt edemiyor ve §0.10 tablosunda
**tam olarak (b) kullanılmıştır** — B-4'e bakınız (K-5).

**Yanlışlanabilirlik testi (Madde X):** bu ölçüt hangi gözlemle çürür? *"İki tur temiz geçen
bir karar sonradan kırıldı"* gözlemiyle. O gözlem **zaten mevcuttur** (`SCAN-03:433`,
ve K-1'in `SKR-049`+`SKR-050` sicili). Ölçüt, kendi yanlışlayıcısı elde varken kondu.

### Talep T-2 (bloke edici)

Ölçüt üç ek şartla yeniden yazılır, yoksa kaldırılır:

> **Bir karar, (a) **iki farklı boyuttan** (Scientific + Engineering), (b) karara **adıyla
> yönelmiş** ve kapsamı kayıtlı iki turda, (c) yeni bloke edici bulgu almazsa kapanır.
> **Bir turda incelenmemiş karar, o turdan "temiz geçmiş" sayılmaz** — kapsam dışı sayılır ve
> sayaç ilerlemez.**

(c) şartı zorunludur çünkü ölçütün bugünkü tek somut kazancı odur ve olmadan ölçüt bir
**kapanış üretecine** dönüşür. Ek olarak `SCAN-03:433-436` vakası ölçütün yanına **bilinen
sınır** olarak yazılmalıdır — ENS'in kendi karşı-örneğini gizleyen bir ölçüt, Madde X
disiplinini biçimselleştirir.

---

## B-3 ⛔ — K-4 "tuttu" bir **cherry-pick**: aynı incelemede K-4'e yeni bloke edici bulgu geldi

§0.10 tablosu (satır 448):

| Karar | Tur 1 | Tur 2 | Durum |
|---|---|---|---|
| **K-4** (`At` kaldırıldı) | — | `ENG-0002`: **tuttu** | ✅ ölçüt sağlandı |

Bu satır **iki kaydırma** içeriyor.

### B-3.1 — K-4 ≠ D-4. Ölçülen şey kararın **bir düzeltmesiydi**, kararın kendisi değil

`ENG-0002`'nin *"düzeltme tuttu"* verdict'i **D-4** içindir: `DecayPolicy.Disabled`'dan
`DateTimeOffset At` parametresinin kaldırılması (`ENG-0002` §E-5A.2, satır 664-669). Bu, K-4'ün
**tek satırlık** bir düzeltmesidir. K-4'ün karar cümlesi ise iki mekanizma taşır (§4.4, satır
1029-1067): **(a)** eşik = kısıtlı tip, **(b)** "kapalı" = varyant + **izli onay**.

Tablo, (a)'nın hiç ölçülmediğini ve (b)'nin **kırıldığını** görmezden gelerek D-4'ün verdict'ini
K-4'ün verdict'i yerine koyuyor.

### B-3.2 ⛔ — `ENG-0002` E-1.7 K-4'ün mekanizma (b)'sini **kırdı**, ve bu tur-2'de yeni bir bloke edicidir

`ENG-0002` §E-1.7 (satır 173-203), başlığı *"D-1 **örnek** olarak uygulandı, **sınıf** olarak
uygulanmadı"*. Tablosunda (satır 182):

| ADR bölümü | Tip | D-1 uygulandı mı |
|---|---|---|
| **§4.4** `DecayPolicy.Disabled(string Reason, **Identity Approver**)` | `sealed record` (satır 891) | ⛔ **HAYIR** |

Ölçüm çıktısı (`ENG-0002` satır 190-192, birebir):

```
[K1] meşru:  Disabled { Reason = planlı bakım, Approver = Identity { Value = alice } }
[K2] `with`: Disabled { Reason = onaylandı,   Approver = Identity { Value = mallory } }
[K3] derleme hatası oluştu mu: HAYIR
```

**Ne anlama geliyor:** K-4'ün tüm değeri, *"sönümü kapatmak artık **açık ve izli**"* iddiasıdır
(§4.4 satır 1063; R12 satır 1118-1121: *"Fark: eskisi **sessiz ve izsiz**, yenisi **açık ve
izli**"*). O izin taşıyıcısı `Approver` alanıdır. Ölçüm gösteriyor ki `Approver`, **tek satır
`with` ile** başkasının üzerine yazılabiliyor — üstelik derleyici hiçbir tanı üretmeden.

Yani K-4, `A5`/`E4`/`G2`/`H3` kimliklerinde sentinel'i kaldırıyor ama yerine koyduğu iz
**taklit edilebilir**. Kapatılan kusur sınıfı `DP4`'tü (sentinel); açılan yüzey `DP1`'dir
(yetki/kimlik taklit edilebilir). **K-4, kendi kapattığı kalıbı `DP1`'e taşıyor** — ve `DP1`
artık `ADR-0004`'e devredildiği için ADR-0003 içinde **hiçbir kararın konusu değil**.

### B-3.3 — Bu, ADR'nin **kendi tasarım ölçütünü** kendi aleyhine çalıştırır

§1, satır 509:

> *"Bir karar, ancak **"unutmak" derleme hatası ya da tip hatası üretiyorsa** sınıfı kapatır."*

`ENG-0002` E-1.7'nin `[K3]` satırı: D-1'i K-4'te unutmak **hiçbir tanı üretmedi**. ADR'nin
kendi ölçütüne göre K-4 sınıfı kapatmıyor. `ENG-0002` bunu açıkça yazmış (satır 199-203) ve
`AUDIT-WAVE2 §10.5`'in meta-bulgusunun *"ADR'nin **kendi düzeltmesinde**"* tekrarı olduğunu
söylemiş. **ADR §0.10 bu paragrafı hiç anmıyor.**

### B-3.4 — Kanıt seçiciliği ölçütü geçersiz kılar

Sonlanma ölçütü *"yeni bloke edici bulgu almazsa"* der. Bulguyu **kim sayar?** Bugün: kararın
sahibi, kendi belgesinde, aynı incelemeden hangi paragrafın "K-4 hakkında" sayılacağını seçerek.
`ENG-0002`'nin bloke edici listesi (satır 53-55) altı talep sayıyor; E-1.7 bunların
kaynağıdır ve **K-4'ün gövde satırını adıyla gösterir**. Bir karar hakkındaki bulgunun,
o bulgunun hangi *changelog kaleminin* altında raporlandığına göre sayılması, ölçütü ölçüt
olmaktan çıkarır.

### Talep T-3 (bloke edici)

K-4'ün `ölçüt sağlandı` işareti **geri alınır**; §0.10 tablosuna `ENG-0002` E-1.7 bir tur-2
bloke edicisi olarak yazılır. Ve karar düzeltilir: `DecayPolicy` ve alt varyantları `record`
**olamaz** (D-1'in sınıf kuralı K-4'e uygulanır) ya da `Approver`/`Reason` bir **denetim
event'ine** taşınır ve tipte tutulmaz. Not: R12'nin bugünkü azaltması (*"`Disabled` üretimi
K-1 mührü ister"*) **artık bu ADR'de mevcut olmayan** bir karara dayanıyor — bkz. B-5.

---

## B-4 ⛔ — K-5 ikinci turda **hiç incelenmedi**; "yeni bulgu yok" yokluğu kanıt saymaktır

§0.10 tablosu (satır 449):

| Karar | Tur 1 | Tur 2 | Durum |
|---|---|---|---|
| **K-5** (mühürlü snapshot) | `ENG-0001`: tek koşulsuz `uygulanabilir` | **yeni bulgu yok** | ✅ ölçüt sağlandı |

### Ölçüm — `ENG-0002`'de K-5 hiç ölçülmedi

`grep -a -n "K-5|K-6|DP6|DP7|W22|W5a|W5b|W2_R4|W2_L4" ENG-0002-adr-0003-v030-olcum.md`,
**tam sonuç kümesi** (11 satır):

| Satır | Bağlam | K-5'e ait mi |
|---|---|---|
| 196, 374, 411 | K-6 ve K-2 hakkında | hayır |
| **533** | `| DP6 | W22 W2_R4 W2_L4 W5a W5b | **5** | 5 ✅ |` | **yalnız üye sayımı** — mekanizma ölçümü değil |
| 534 | `| DP7 | H4 W3 W17 W5e W8a W8b | 6 | 6 ✅ |` | aynı |
| 538 | çakışma taraması (`W2_L4` DP6'da) | sayım |
| 577 | §7'nin "40" alıntısı | hayır |
| **818, 833, 838** | **uygulama sırası** tartışması (`K-5 → K-6 → K-2 → …`) | sıra, karar değil |

`ENG-0002`'nin verdict tablosu (satır 39-51) **D-1'den D-9'a** dokuz satırdır. K-5'e ait bir
satır **yoktur** — çünkü v0.3.0'da K-5 hakkında **değişen bir karar yoktu** ve `ENG-0002`'nin
kapsamı, künyesinde yazıldığı gibi (`title: ADR-0003 v0.3.0 — **değişen kararların** ölçümü`),
yalnızca değişen kararlardı.

**Yani `ENG-0002` K-5'i "temiz" bulmadı; K-5'e bakmadı.** İkisi arasındaki fark, ölçütün tam
olarak ayırt edemediği farktır (B-2.3).

### Neden bu, K-5 için özellikle ağır

K-5 §4.4'ün en iddialı cümlesini taşıyor (satır 1172): *"**Hepsi kapanıyor. Kapanmayan yok.**
Bu, altı kararın **en temiz** kapanışıdır."* Bu iddiayı taşıyan mekanizmanın **iki bacağı**
var ve hiçbiri ölçülmedi:

1. **Dönüş tipi değişimi** (`ImmutableArray<T>`/`FrozenSet<T>`). `ENG-0001` bunu
   *"uygulanabilir"* buldu — ama `ENG-0001`'in ölçtüğü şey (ADR §0.5, satır 117-119) bir
   **mimari tarama**ydı: *"K-5 **22 ihlal**, K-6 **36 ihlal**, ~15 satır"*. Bu, kuralın
   **bugün ihlal edildiğini** sayan bir ölçümdür — kuralın **uygulandığında tutacağını**
   gösteren bir ölçüm değildir. İki farklı iddia.
2. **Mimari test** (satır 1154-1157): *"`typeof(Ens.Kernel).Assembly`'deki tüm `public`
   üyelerin dönüş tipleri taranır."* Bu testin **yazılmadığı** ve K-5'in tek zorlayıcısı
   olduğu §6 tablosunda kayıtlı (satır 1362: *"Sınıf (test, otomatik) — **Test devre dışı
   bırakılabilir**"*).

Ayrıca ADR'nin **kendi §7'si** K-5'e karşı açık bir yanlışlayıcı taşıyor ve iki turda da
kapanmadı — satır 1385:

> *"4. **Üç kararın ortak `default(struct)` deliği (R14/**R15**/R20) kapatılmazsa** — K-4,
> **K-5**, K-6'nın üçü birden tip zorlamasını kaybeder."*

R15 (satır 1206-1208): *"`default(ImmutableArray<T>)` **`null` gibi davranır** ve `.Length`
erişimi `NullReferenceException` atar."* Bu, K-5'in mühürlü-snapshot iddiasına doğrudan
saldıran, **ADR'nin kendi yazdığı** bir açıktır ve `SKR-050` onu (satır 318-319) `ImmutableArray<T>`
BCL `struct`'ı olduğu için **OQ1'in blanket `class` kararıyla kapanamayacağı** biçiminde
teyit etmiştir. §0.10 (satır 464) bunu kabul edip **OQ1'i yeniden açıyor** — yani K-5'in
bilinen bir açığı bugün **açık** ve K-5 aynı sayfada `✅ ölçüt sağlandı` alıyor.

> **İki satır arası çelişki:** satır 449 K-5 için `✅`; satır 464-465 aynı bölümde
> *"OQ1 **yeniden açık** … K-5/K-6'nın **tip seçimini bağlar**"*. Tip seçimi bağlanmamış bir
> karar kapanmış sayılamaz.

### Talep T-4 (bloke edici)

K-5'in `✅` işareti **geri alınır** ve durumu `incelenmedi` olur. Kapanması için gereken tek
ölçüm yazılıdır ve ucuzdur: (a) mimari testin **yazılıp koşulması** ve `ImmutableArray`/
`FrozenSet` dönüşlerinin downcast/canlılık saldırılarına karşı ölçülmesi, (b)
`default(ImmutableArray<T>)` deliğinin `W22`/`W5a`/`W5b`/`W2_L4`/`W2_R4` beşi üzerinde
ayrı ayrı denenmesi. Bu, bir Engineering turudur — `ENG-0003`'ün kapsamına **açıkça**
yazılmalıdır.

---

## B-5 ⛔ — Bölme, K-4'ü K-1'e olan **mekanizma bağımlılığından** kopardı (atomiklik testi)

Görev sorusu: *bölme K-3..K-6'yı K-1/K-2'ye olan bağımlılıklarından kopardı mı; ADR-0005
reddedilirse bunlar da düşer mi?* `ENG-0001`'in ölçtüğü bağımlılıkları ADR metnine karşı
yeniden taradım. Cevap **karar-başına farklı**dır ve bu, bölmeyi tümden geçersiz kılmaz —
ama **bir kararı** kesinlikle geçersiz kılar.

### B-5.1 Bağımlılık haritası (ADR metninden, satır atıflı)

| Bağ | Metin | Bölmeden sonra |
|---|---|---|
| **K-4 → K-1** | §4.4 satır 1063: *"`Disabled` dalı **iz yayar** (`PolicyDisabled` event'i, **K-1 mührüyle**)"* | ⛔ **KOPTU** — K-1 `ADR-0004`'te |
| **K-4 → K-1** | R12, satır 1120: *"Azaltma: `Disabled` üretimi **K-1 mührü ister** (yalnız policy-otoritesi verebilir)"* | ⛔ **KOPTU** — tek azaltma dayanaksız |
| **K-3 ↔ K-4** | R9, satır 1014-1017: `skew` K-4'e tabi; *"bu bağ **zorunludur**"* | ✅ ikisi de ADR-0003'te |
| **K-4/K-5/K-6 → OQ1** | §7 madde 4, satır 1385; R14/R15/R20 | ⚠️ OQ1 açık (B-8) |
| **K-6 → K-3** | yok — bağımsız | ✅ |
| **K-5/K-6 → K-2** | yok (K-2 `W5g` `AdapterId`'yi kapatır; K-5 `W5a`/`W5b` koleksiyonu) | ✅ **gerçekten ayrılabilir** |

**Dürüst sonuç:** K-2'nin ayrılması (→ `ADR-0005`) **savunulabilir bir bölmedir** — K-3..K-6'nın
hiçbiri K-2'nin kanonik kimliğine mekanizma düzeyinde bağlı değil. Bunu kaydediyorum çünkü
bölmenin **tamamen** yapay olduğu iddiası ölçüme uymaz.

**Ama K-1'in ayrılması K-4'ü dayanaksız bırakıyor.**

### B-5.2 — K-4'ün iz mekanizması artık **hiçbir Accepted karara** dayanmıyor

K-4'ün değeri, sentinel'i *"açık ve izli"* bir yolla değiştirmesidir (R12). O izin bütünlüğünü
sağlayan iki şey vardı:

1. `Approver` alanının taklit edilememesi → **B-3'te kırıldı** (`with` ile yazılıyor).
2. `PolicyDisabled` event'inin **K-1 mührüyle** yayılması → **bölmeyle koptu**.

İkisi birden düştüğünde K-4'ün geriye kalanı şudur: *"eşik `0` yerine bir varyant seç."*
Bu, `A5`/`E4`/`G2`/`H3`/`W10`'un **sessizliğini** kaldırır (gerçek bir kazanç, küçümsemiyorum)
ama **kimin kapattığını doğrulanabilir kılmaz**. Sicildeki kusur tarifi ise ikisini birlikte
istiyordu.

Daha kötüsü: `ADR-0004` K-1'i devralırken **hedefini küçültüyor** (`ADR-0004:29, 101` —
*"kazayla ve gündelik taklidi durdurur"*). Yani K-4'ün dayandığı mühür, devredildiği yerde
**zayıflatılmış** hâlde. K-4'ün R12 azaltması, artık *var olmayan güçte* bir mekanizmaya atıf
yapıyor.

### B-5.3 — `ARCH-0001`'in atomiklik testi burada uygulanmalı

Görev metni `ARCH-0001`'in RFC-6002/6003 için verdiği atomiklik testine atıf yapıyor. Testin
özü: *bir belge, bileşenlerinden biri reddedilirse geri kalanı **tutarlı kalıyorsa** bölünebilir.*

| Senaryo | ADR-0003'ün durumu |
|---|---|
| `ADR-0005` (K-2) reddedilirse | K-3/K-4/K-5/K-6 **tutarlı kalır** ✅ |
| `ADR-0004` (K-1) reddedilirse | **K-4'ün iz garantisi düşer** ⛔ — ve `ADR-0004` zaten K-1'in orijinal hedefini reddediyor |

Bölme, atomik olmayan bir sınırdan geçirilmiş. Doğru sınır **K-1+K-4** ya da (daha temiz)
K-4'ün iz mekanizmasının K-1'den **arındırılmasıdır**.

### Talep T-5 (bloke edici)

Şu üçünden biri seçilir ve gerekçesi yazılır:
1. K-4'ün iz mekanizması K-1'den **bağımsızlaştırılır** — `PolicyDisabled` event'i mühürle
   değil, `DecisionAggregate`'in mevcut event zinciriyle (K-3 M-4'ün monotonluk + tekillik
   doğrulaması) korunur. **Bu, en dar ve önerdiğim yoldur** — ADR-0003 içinde çözülür.
2. K-4 `ADR-0004`'e taşınır (K-1 ile birlikte kalır).
3. K-4, `ADR-0004` Accepted olmadan uygulanamaz koşuluyla işaretlenir — ve bu, ADR-0003'ün
   `Accepted` olmasını `ADR-0004`'e bağlar (bölmenin amacını ortadan kaldırır).

Ayrıca künye `requires:` alanı bugün `[ADR-0001]` diyor; hangi seçenek seçilirse seçilsin
**`ADR-0004` bağı künyeye yazılmalıdır** — bugün yok, ve `ADR-0004`/`ADR-0005`'in künyeleri
`depends_on: [ADR-0001, **ADR-0003**, ENS-0000]` diyerek **ters yönde** bir bağ kuruyor.
Bağımlılık okları bugün iki belge arasında **çelişik yönde**dir.

---

## B-6 ⛔ — 22 **yeniden hesaplandı: aritmetik doğru** — ama beşinci sayıdır ve §7 hâlâ 40 diyor

`work-protocol.md` §3.5'in dördüncü kontrolü: **sayıyı devralma, yeniden hesapla.**
`DEFECT-PATTERN-MAP.md`'ye gitmeden önce `ENG-0002` E-5.1'i de devralmadım; ADR'nin kendi
§4 kapanma tablolarından bağımsız saydım, sonra iki kaynağı karşılaştırdım.

### B-6.1 Bağımsız sayım — 22 **türetilebilir**

| Kalıp | ADR §4'ün kapanma tablosundaki ID'ler | Sayım | `ENG-0002` E-5.1 | §0.10 beyanı |
|---|---|---|---|---|
| `DP3` (K-3, §4.3 satır 970-975) | `A1 A2 B4 D4 W2_L3 W2_R6` | **6** | 6 ✅ | 6 ✅ |
| `DP4` (K-4, §4.4 satır 1073-1077) | `A5 E4 G2 H3 W10` | **5** | 5 ✅ | 5 ✅ |
| `DP6` (K-5, §4.5 satır 1166-1170) | `W22 W2_R4 W2_L4 W5a W5b` | **5** | 5 ✅ | 5 ✅ |
| `DP7` (K-6, §4.6 satır 1256-1261) | `H4 W3 W17 W5e W8a W8b` | **6** | 6 ✅ | 6 ✅ |
| **Toplam** | | **22** | — | **22** ✅ |

**Çakışma taraması:** 22 ID arasında tekrar yok. `W2_L3` (DP3) / `W2_L4` (DP6) ve `W2_R4`
(DP6) / `W2_R6` (DP3) benzer görünen **ayrı** kimliklerdir. `W16` §4.4'ün notunda (satır 1081)
*"P1'de sayıldı … burada sayılmıyor ki **çifte sayım** olmasın"* diye açıkça dışlanmış — bu
doğru bir davranıştır ve `W16` artık `ADR-0004`'e gittiği için 22'de **yok**. ✅

**Ayrıca çıkarma yok:** 22, `47 − 4`'ün alt kümesi değil, doğrudan dört kalıbın toplamıdır;
çıkarılan dört kimlik (`C2`, `W1b`, `W2_O1`, `W2c`) DP1/DP2 üyeleridir, yani **hepsi kapsam
dışına çıkan kararlara** aitti. Kalan dört kalıptan hiçbir çıkarma yapılmadı.

> **22'nin aritmetiği doğrudur.** Bunu kaydediyorum: v0.5.0'ın en temiz işi budur.

### B-6.2 ⛔ Ama gövde temizlenmedi — bugün **BEŞ** sayı var

`ENG-0002` E-5.3 dört sayı ölçmüştü (41/40/43/"40+4"). v0.5.0 beşincisini ekledi ve
**hiçbirini kaldırmadı**. Ölçüm (`grep -a -n`, v0.5.0 anlık görüntüsü, 1442 satır):

| Sayı | Satırlar | Nerede |
|---|---|---|
| **22** | **483, 487** | **yalnız §0.10 changelog** |
| **43** | 19, 41, 74, 219, 228, 230-233 | künye + §0.7 changelog |
| **41** | 497, 658 | **§1 Bağlam**, **§3 Kapsam** — normatif gövde |
| **40** | 658, 660, 684, 1377 | **§3 Kapsam**, K-0 kutusu, **§7 Failure conditions** |
| **"40+4"** | 20 | künye `skeptic_review:` yorumu |

Manşet sayı, **iddiayı taşıması gereken tek yerde yok**: §7'nin 1377. satırı hâlâ

> *"İddia: **K-1…K-6** uygulandığında **40** kimlik kapanır (P1: 11, P2: 11, P3: 6, P4: 5,
> P6: 5, P7: 6 = 44 üye …)"*

diyor. **Belgenin yanlışlanma noktası, belgenin kapsamıyla da kararıyla da çelişiyor.**
`ENG-0002` E-5.3 bunu **bloke edici T-9** olarak yazmıştı (satır 55, 576-579); v0.5.0 talebi
karşılamak yerine **yeni bir sayı ekledi**. Bu, B-1'in kalıbının sayısal alandaki tekrarıdır:
karar changelog'da, gövde eski.

Ek olarak §1 (satır 497-498) hâlâ *"altı mimari kararın **41**'ini birden kapatacağını iddia
etti (P1+P2+P3+P4+P6+P7 = 12+13+6+5+5+6)"* diyor — parantez içi **47** eder. Bu, `SKR-049`
T-A'nın **üç tur önce** bulduğu aritmetik hatadır ve gövdede hâlâ duruyor.

### Talep T-6 (bloke edici)

§7'nin 1377. satırı **22** ve DP3/DP4/DP6/DP7 ile yeniden yazılır; §1 ve §3'ün 41/40'ları
düzeltilir ya da tarihsel kayıt olarak açıkça işaretlenir (`~~40~~ → 22, bkz. §0.10`); künye
`skeptic_review:` yorumundaki "40+4" güncellenir. **Ve sonra `grep -a -c "40\|41\|43"` ile
doğrulanır.** Bir manşet sayının changelog'da doğru, gövdede yanlış olması bu belgede artık
bir vaka değil, bir **kalıp**tır.

---

## B-7 ⛔ — K-3'ün "koşullu" statüsü `Accepted` ile bağdaşmıyor; üç yüzeyin ikisi ADR'nin **kendi kapattığı sınıflar**

§0.10 tablosu K-3'e `⚠️ koşullu` veriyor (satır 451) ve koşulu Faz 0'a erteliyor (satır
453-455): *"üç sessiz-başarısızlık yüzeyi var — `#pragma` bastırma, **yanlış yazılmış yasak
satırının hiçbir tanı üretmemesi**, ve `WarningsAsErrors` yokluğu. Bunlar **Faz 0** işidir."*

### B-7.1 — `Accepted` bir ADR'ye kod dayanır; koşulu sağlanmamış karar kod üretemez

Madde VII: *"kod yalnız Accepted ADR'lere dayanır."* ADR'nin kendi §8'i (satır 1419) K-3 için
zorunlu iz satırını yazıyor. Kabul edilirse, bir geliştirici K-3'ü **koşulsuz** uygulayabilir
hâle gelir — çünkü Accepted bir ADR'de "koşullu" bir statü **kategorisi yoktur**. `status:`
alanı belge düzeyindedir; karar düzeyinde bir `conditional` alanı ne künye şemasında
(`.claude/standards/metadata-header.md`) ne ADR-0001/0002 emsalinde vardır.

Ve ADR **kendisi** bunu §6'da yasaklıyor (satır 1365-1369):

> *"Kabul edilirse K-3, bir Roslyn analyzer … ya da K-5 tarzı bir assembly taraması
> **olmadan uygulanmamalıdır.**"*

Yani belge, kendi kabulünün ardından ortaya çıkacak durumu **yasak** ilan ediyor, ama o yasağı
zorlayacak bir mekanizma tanımlamıyor. Bu, `plan-first`/`Guard.cs` dersinin aynısıdır:
**konvansiyonla zorlanan karar, karar değildir** — D-1'in kendi cümlesi (satır 153).

### B-7.2 ⛔ — Üç yüzeyin ikisi, ADR'nin kapattığını iddia ettiği kalıpların **tam kendisi**

Bu, koşulun neden "sonraya bırakılabilir bir detay" olmadığını gösterir:

| Yüzey (`ENG-0001` §E-4) | Hangi ADR kalıbı | ADR'nin kendi sözü |
|---|---|---|
| **`#pragma warning disable RS0030` tek satırda bastırıyor** (ölçüldü, `ENG-0001:360-363`) | **DP1** (`E3` — *"tek satırda taklit edilebilir"*) | §4.1 reddedilen alternatif 3, satır 794-796: *"analyzer devre dışı bırakılabilir ve `#pragma warning disable` tek satırdır — **`E3`'ün aynısı**"* |
| **Yanlış yazılmış yasak satırı hiçbir tanı üretmiyor** (`ENG-0001:349-352`) | **sessiz başarısızlık** — `W1a`/`W1b`'nin çekirdeği | D-2 gerekçesi, satır 166-168: *"`W1a`/`W1c`'nin kusuru … **sessiz başarı**ydı"* |
| `WarningsAsErrors` yokluğu | `DP4` (politika değeri örtük) | D-6, satır 462-464 |

**ADR, K-1'de bir mekanizmayı reddettiği gerekçenin aynısını, K-3'te kabul ediyor.** §4.1
analyzer'ı *"`E3`'ün aynısı"* diye reddetmişti; K-3'ün tek zorlayıcısı o analyzer'dır.
`ENG-0001:352` bunu şöyle yazmış (birebir): *"yanlış yazılmış bir satır, korumanın olmadığı
yanılsamasını"* üretir — yani K-3'ün koruması **kendi ölçütüne göre yanlışlanabilir değildir**.

İkinci yüzey özellikle ağırdır çünkü **kendini gizleyen** bir başarısızlıktır: yasak listesi
yanlış yazılırsa build **yeşil** kalır ve kimse korumanın olmadığını fark etmez. `Guard.cs`'in
"yedi nokta → dokuz nokta" hikâyesinin (satır 505-507) tam olarak modern hâli.

### B-7.3 — "Faz 0 işi" ertelemesi, sıra sorununu çözmüyor

§0.10 koşulları Faz 0'a atıyor. Ama `ENG-0002` §8 (satır 833) ölçtü ki **Faz 0'ın içeriği bugün
yanlış** (`CS8618` daraltması, tek-cevap OQ1) ve **faz kapısı yok**. Yani "Faz 0'da yapılır"
ifadesi, tanımı bu turda düzeltilen ama kapısı hâlâ olmayan bir fazın vaadidir.

### Talep T-7 (bloke edici)

İki yoldan biri:
1. **K-3, `ADR-0003` v0.5.0'dan çıkarılır** ve koşulları karşılandığında ayrı bir turda döner.
   (Bölme mantığının tutarlı uygulanması budur: K-3 "iki turda kırılmadı" değil, **iki turda da
   koşullu** çıktı.)
2. K-3 kalır ama karar cümlesine **zorlayıcı** eklenir: `BannedSymbols.txt`'in her satırı için
   bir **negatif test** (yasaklı çağrının gerçekten tanı ürettiğini kanıtlayan) ve `#pragma`
   kullanımını tarayan bir **mimari test** — `ENG-0001` T-4'ün (satır 845-849) talebi. Bunlar
   K-5'in mimari testinin kardeşidir; ADR'de mekanizma zaten var, K-3'e uygulanmamış.

**Ne olursa olsun `⚠️ koşullu` bir karar `Accepted` bir belgede kalamaz** — ya koşul karara
gömülür ya karar bekler.

---

## B-8 ⛔ — OQ1 açıkken K-5/K-6 `Accepted` olamaz — bunu **ADR'nin kendisi** söylüyor

§0.10 D-6 düzeltmesi (satır 462-465):

> *"**OQ1 yeniden açık** — `ImmutableArray<T>` BCL `struct`'ı olduğu için blanket `class`
> kuralı kapatamaz. OQ1 artık ADR-0004/0005'e değil, **buraya** aittir ve **K-5/K-6'nın tip
> seçimini bağlar**."*

Aynı bölüm, 11 satır yukarıda (449-450) K-5 ve K-6'ya `✅ ölçüt sağlandı` veriyor.

**Bir kararın tip seçimi bağlanmamışsa o karar kapanmamıştır.** §7'nin açık soru tablosu
(satır 1396) bu ayrımı zaten yazıyor: başlığı *"Açık sorular (**kabul öncesi cevaplanmalı**)"*.
OQ1 orada, satır 1400'de duruyor. Belge, kendi kabul-öncesi şartını sağlamadan kabule aday.

### B-8.1 — Fark ölçülmüş ve önemsiz değil

`ENG-0002` E-3.4 (satır 385-396) ölçtü:

```
[C1] class + IComparable, null içeren liste: SESSİZCE sıraladı
     -> b(0,9), a(0,5), null-alani(NULL)
[C4] 100k `class` Measured üretimi: 2349 KB tahsis   (struct'ta 0 KB)
```

`class` seçilirse ilklendirilmemiş bir `Measured`, `Comparer<T>.Default` tarafından **en küçük
değer gibi sessizce sıralanır** — `Scheduler`'ın dikkat bütçesinde bir kararın sessizce en sona
düşmesi demektir. Bu, K-6'nın kapattığını iddia ettiği kalıbın (`DP7` — çıktı kapısı yok)
**yeniden doğuşudur**. `struct` seçilirse D-6'nın `CS8618` gerekçesi `Measured` için hiç
çalışmaz (`ENG-0001` E-2.4).

Yani OQ1 bir "stil" sorusu değil: **her iki cevap da K-6'nın bir üyesini geri açıyor.**
`ENG-0002` T-3'ü (satır 398-400) bunun için bloke edici ilan etmişti; v0.5.0 OQ1'i yeniden
açarak **doğru olanı yaptı** ama K-5/K-6'nın `✅`'ini geri almadı.

### B-8.2 — §7'nin OQ tablosu bölmeden sonra güncellenmedi

Satır 1400: *"**OQ1** — `Measured`/`DecayRate`/`ImmutableArray` alanları `struct` mu `class` mı?
(R14/**R15**/R20 tek bir cevabı bekliyor)"*. Bu formülasyon `SKR-050`'nin ve §0.10'un
düzelttiği şeyi hâlâ taşıyor: **tek bir cevap yok** — `ImmutableArray<T>` BCL `struct`'ıdır,
seçilemez. §0.10 bunu kabul ediyor, §7 hâlâ "tek bir cevabı bekliyor" diyor. B-1'in kalıbı.

### Talep T-8 (bloke edici)

OQ1 **karar-başına** cevaplanır ve §7'ye yazılır: `Measured` için (K-6), `DecayRate`/
`StaleThreshold`/`MagnitudeFloor` için (K-4), ve `ImmutableArray<T>` alanları için (K-5 —
burada tip seçilemez, yalnız **ilklendirme zorlaması** kararlaştırılabilir: `= ImmutableArray<T>.Empty`
+ mimari test, R15'in kendi azaltması). Cevaplanana kadar K-5 ve K-6 `✅` alamaz.

---

## B-9 ⚠️ — D-7'nin ilan **edilmemesi** dürüstlük değil: künye `principles:` yeni kapsamla **aktif çelişkide**

§0.10 (satır 467-471) D-7'yi (`P` → `DP` yeniden adlandırma) ilan etmemeyi seçiyor:
*"ilan edip uygulamamaktansa, hiç ilan etmemek dürüsttür."*

**Bu ilkeye katılıyorum** ve B-1'de onu ADR'ye karşı kullandım. İtirazım ilkeye değil, o ilkenin
burada **yanlış soruya** uygulanmış olmasına.

### B-9.1 — Bölme, D-7'nin kusurunu **kapatmadı; büyüttü**

D-7'nin gerekçesi (satır 215-217, §0.2 satır 81-82): künye `principles:` alanı Anayasa Madde
III'ün ilkelerine gönderiyor, gövde ise kalıp sözlüğüne — ve ikisi karışınca künye §3'ün
**tersini** söylüyor. Bugünkü durum:

```
principles:    [P1, P5, P6, P7, P8]        (künye, satır 10 — DEĞİŞMEDİ)
```

Yeni kapsam: **DP3, DP4, DP6, DP7**. Künyeyi **kalıp** sözlüğüyle okuyan biri şunu görür:

| Künyedeki | Kalıp anlamı | v0.5.0 kapsamına göre |
|---|---|---|
| `P1` | yetki taklit edilebilir | ⛔ **`ADR-0004`'e devredildi** |
| `P5` | reflection | ⛔ kapsam dışı (§3 satır 666) — **ve `ADR-0004`'e gitti** |
| `P6` | koleksiyon sızıntısı | ✅ K-5 |
| `P7` | çıktı kapısı yok | ✅ K-6 |
| `P8` | öz-beyan kalibre değil | ⛔ **açık borç** (§5.2) |

Yani beş girdinin **üçü** bugün yanlış — ve eksik olan **`P3`/`P4`** (K-3/K-4, kapsamın yarısı)
listede **hiç yok**. v0.3.0'da bu liste yalnız *belirsizdi*; v0.5.0'da **ölçülebilir biçimde
yanlış** hâle geldi, çünkü kapsam değişti ve künye değişmedi.

### B-9.2 — "İlan etmemek" burada bir edim gerektirmiyordu

D-7'nin tam uygulaması 78 `P` geçişini düzeltmeyi gerektirir — **evet, ayrı bir edimdir** ve
ertelenmesi savunulabilir. Ama künyenin **tek satırı** kapsam değişikliğinin **doğrudan
sonucudur** ve bölmeyle birlikte güncellenmeliydi; `provides:` alanı da öyle (B-1, sıra 1).
Bunlar D-7'nin (yeniden adlandırma) değil, **v0.5.0'ın kendi kararının** gereğidir.

### Talep T-9 (yüksek)

Künye `principles:` ya Anayasa ilkelerine göre yeniden yazılır (ve gövdenin `P` kullanımından
ayrıldığı bir satırla işaretlenir), ya da `DP3, DP4, DP6, DP7` yapılır. Hangisi seçilirse
seçilsin **künye ile §3 aynı sözlüğü kullanmalıdır** — bu, D-7'nin çözmeye çalıştığı asıl
kusurdur ve tek satırlık kısmı bugün kapatılabilir. `provides:` da aynı edimle dörde iner.

---

## B-10 ⚠️ — 22'nin aritmetiği doğru, **içeriği** en az üç yerde hak edilmemiş

B-6 sayının türetilebilir olduğunu doğruladı. Ayrı bir soru: **22 kimlik gerçekten kapanıyor
mu?** `ENG-0002` bu ayrımı `43` için yapmıştı (E-5.5: *"aritmetiğin değil **kapsam
varsayımının** sorunu"*). Aynı testi 22'ye uyguladım.

| ID | Kalıp | Durum | Kaynak |
|---|---|---|---|
| `W3` | DP7 | **koşullu** — ADR'nin **kendisi** yazıyor | §2.7 satır 636-641: *"K-6 bunu **yalnızca** postcondition'ın işaret normalizasyonu (`x + 0.0`) içermesi hâlinde kapatır"*; ayrıca §4.6 satır 1257: *"bu kusur zaten **sömürülebilir değildi**"* |
| `H4`,`W17`,`W8a`,`W8b` | DP7 | **zorlayıcısı yazılmamış** | §4.6 satır 1249-1250'nin *"mimari test"*i yok; `ENG-0002` E-3.5 ölçtü: `implicit operator double` ile kapı **atlanabiliyor** (`[M11]`), R18 (satır 1298-1302) bunu *"K-6'nın en kırılgan noktası"* diye kaydediyor |
| `W5e` | DP7 | **sayısal değil** — ADR kabul ediyor | §4.6 satır 1244: *"`W5e` … sayısal değildir ama **aynı kalıptır**"*. `AdapterGateway` ayrı bir mekanizmadır ve maliyet tablosunda yeni dosya olarak var; kapanışı K-6'nın tip kararına değil o gateway'e bağlı |
| `A5`,`E4`,`G2`,`H3`,`W10` | DP4 | **iz garantisi düştü** | B-3.2 (`Approver` `with` ile yazılıyor) + B-5.2 (K-1 mührü koptu) |
| DP6'nın beşi | DP6 | **ölçülmedi** | B-4 |

**Kalan sağlam çekirdek:** DP3'ün altısı (`A1 A2 B4 D4 W2_L3 W2_R6`) — ve onlar da K-3'ün
analyzer koşuluna bağlı (B-7). Yani bugün **koşulsuz** kapandığı gösterilebilen kimlik sayısı
**sıfırdır**; bu, kararların kötü olduğu anlamına gelmez, **hiçbirinin uygulanıp
ölçülmediği** anlamına gelir — ki bu Madde VII gereği doğrudur ve beklenendir.

> **Asıl itiraz:** 22 bir **tahmin**tir, bir ölçüm değil. Belge onu *"kapanma iddiası"*
> (satır 483) diye adlandırarak doğru davranıyor. Ama §7'nin yanlışlanma ölçütü —
> *"bu 22 kimliğin `AUDIT_DEFECT_*` testleri `AUDIT_FIXED_*`'a dönmelidir"* (satır 487) —
> `ENG-0002` E-2.6'nın **bloke edici T-2** bulgusuyla zaten çürütülmüştü ve v0.5.0 onu
> **kelimesi kelimesine tekrarladı**.

`ENG-0002` E-2.6 (satır 298-316) ölçtü: bir `AUDIT_DEFECT_*` testi kırmızıya döndüğünde bunun
iki farklı sebebi olabilir — kusur kapandı, **ya da kurulum kırıldı**. Somut çıktı (satır
304-308): `W1a` kırmızıya döndü, ama sebep `Register` çağrısının `ArgumentException` atmasıydı;
testin **saldırı satırı hiç çalışmadı**.

> **Talep T-2 (ENG-0002, bloke edici, birebir kabul ediyorum):** yanlışlanma ölçütü *"test
> kırmızıya döndü"* değil, *"test `AUDIT_FIXED_*` olarak **yeniden yazıldı** ve **aynı saldırı
> satırına** ulaşıp ters iddiayı doğruluyor"* olmalıdır.

v0.5.0 sayıyı 43'ten 22'ye düşürdü ama **ölçütü düzeltmedi**. Sayı küçüldüğünde ölçütün
kalitesi artmaz; yalnız yanlış ölçülecek kimlik sayısı azalır.

### Talep T-10 (yüksek)

§0.10'un yanlışlanma cümlesi `ENG-0002` T-2'ye göre yeniden yazılır. Ayrıca `W3` ve `W5e`
22'nin içinde **koşullu** olarak işaretlenir (ADR bunu §2.7 ve §4.6'da zaten dürüstçe
söylüyor — manşet sayıya yansıtılmamış olması bir tutarsızlıktır, bir yalan değil).

---

## K — Kararların kendisi hakkında (verdict'ten ayrı)

Bir saldırı, neyi **çürütmediğini** de söylemek zorundadır; aksi hâlde kendisi kalibre
değildir. Verdict `refuted`, **belgenin bugünkü hâlinin Accepted olabileceği iddiası**
içindir. Kararların içeriği hakkındaki kanaatim ayrıdır:

| Karar | Kanaatim | Gerekçe |
|---|---|---|
| **K-3** | **doğru karar, eksik zorlayıcı** | §2.1'in `A1`/`A2` teşhisi (saat değil **veri** kusuru) bu ADR'nin en iyi entelektüel işidir; iki mekanizmayı tek karar sayması doğrudur. Sorun analyzer koşulu (B-7) |
| **K-4** | **doğru yön, kırık iz** | "Kapalı bir sayı değil varyanttır" savunulabilir ve prior art'ı sağlam (Hoare, parse-don't-validate, make-illegal-states-unrepresentable). Sorun `Approver`'ın taklit edilebilirliği (B-3) |
| **K-5** | **en sağlam aday** | `IReadOnly*`'ın garanti olmadığı tespiti doğru ve `W2_L4`'ün test yorumuyla desteklenmiş; `CapabilityRegistry.cs:93`'ün *"bir yerde zaten öğrenilmiş ders"*i güçlü bir argüman. Sorun yalnız **ölçülmemiş** olması (B-4) |
| **K-6** | **doğru ama en kırılgan** | Meyer postcondition çerçevesi doğru; `implicit operator double` kasıtlı asimetrisi kapıyı deliyor ve ADR bunu R18'de **kendisi** yazıyor |

**Bu dört karar bir turluk mekanik temizlik + üç ölçümle Accepted'a gidebilir.** Verdict'in
`refuted` olması, kararların değil **belgenin durumunun** hükmüdür.

## Katıldığım noktalar

Bunları kaydetmezsem inceleme eksik olur.

1. **Bölme fikri doğrudur ve teşhisi haklıdır.** §0.10'un gerekçesi (satır 424-427) —
   *"bir karar kırılınca altısı birden yeni sürüme girdi; tur sayısını kararların zorluğuyla
   değil **paketlemeyle** şişirdi"* — doğru bir gözlemdir ve `CEO-0002`'nin kapsam-orantısı
   uyarısına yapılan **kendi aleyhine** atıf (satır 429-430: *"RFC'ler için alıntılandı ve
   ADR'de **ihlal edildi**"*) dürüstlüktür. B-5.1'in ölçümü K-2 ayrımının **gerçekten**
   savunulabilir olduğunu gösteriyor.

2. **Sonlanma ölçütü koymak doğru bir reflekstir.** Sonsuz iterasyon gerçek bir risktir ve
   hiçbir kapı ölçütü olmayan bir süreç, ADR'yi süresiz `draft`'ta tutar. B-2 ölçütün
   *içeriğine* saldırır, varlığına değil.

3. **D-6 düzeltmesi doğru ve zordur.** v0.5.0 tek-koda daraltmayı **geri aldı** ve `ENG-0001`'in
   orijinal `Nullable` kategorisi talebine döndü (satır 462-464). Bir kararın kendi
   düzeltmesini geri alması nadirdir; `ENG-0002` T-4 bunu istemişti ve karşılandı.

4. **OQ1'i yeniden açmak, kapalı ilan etmekten zordur ve doğrudur.** `SKR-050`'nin
   `ImmutableArray<T>` itirazı kabul edildi. Bu, "kapandı" hanesini korumak yerine borcu
   görünür tutan bir davranıştır (Madde X).

5. **`ENG-0002`'nin G4 sınırı ADR'ye alıntılanmış** (satır 405-410): *"`ENG-0002` … G4 anlamında
   **ikinci boyut değildir**."* ADR bunu gizleyebilirdi; gizlemedi. Bu, `governance/`
   SCAN-03'ün G4 bulgusuyla tutarlı bir dürüstlüktür — B-2.2'de ona karşı kullandım, ama
   kaydın kendisi ADR'nin lehinedir.

6. **§2 (devralınan bulgunun doğrulanması) hâlâ bu belgenin en sağlam bölümüdür.** Dört yanlış
   kalıp ataması (`C2`, `W1b`, `W2_O1`, `A1`/`A2`'nin çerçevelenmesi) gövde okunarak bulundu ve
   ADR **kendi sayısını düşürerek** kaydetti. §2.5'in geri çekilmesi (4 NUL baytı vakası) ve
   K-0'ın kaldırılması — *"yanlış bir önermeden türeyen bir karar, sonucu makul olsa bile karar
   değildir"* (satır 681-682) — Madde X'in istediği davranışın ders kitabı örneğidir.

7. **`W16`'nın çifte sayımdan korunması** (satır 1081-1082) küçük ama doğru bir refleks; 22'nin
   temizliğinin sebebi budur (B-6.1).

8. **Belge kendi yanlışlanma koşullarını yazmaya devam ediyor** (§7, 7 madde). B-4, B-8 ve
   B-10'daki bulgularımın **üçü de** ADR'nin kendi §7'sinden ve kendi R-risklerinden çıktı.
   Bir belgenin kendi aleyhine yazdığı maddelerin, ona karşı en güçlü saldırıyı üretmesi
   Madde X'in tam olarak beklediği şeydir ve bu ADR'nin en kalıcı erdemidir.

## Sahibine talepler

Sıra, şiddet sırasıdır. **T-1 … T-8 kapanmadan `draft` → `accepted` yapılmamalıdır.**

### Bloke edici

| # | Talep | Maliyet |
|---|---|---|
| **T-1** | Bölme **gövdeye uygulanır**: §3, §4.1, §4.2, §6, §7 (satır 1377 + OQ3/4/5/6), §8, künye `provides:`. Sonra `grep -a -c "K-1"` ile **doğrulanır** | mekanik, ~1 saat |
| **T-2** | Sonlanma ölçütü üç şartla yeniden yazılır (iki **boyut** · karara **adıyla yönelmiş** tur · incelenmemiş = kapsam dışı) + `SCAN-03:433-436` karşı-örneği ölçütün yanına yazılır | 1 paragraf |
| **T-3** | K-4'ün `✅`'i geri alınır; `ENG-0002` E-1.7 tur-2 bloke edicisi olarak kaydedilir; `DecayPolicy` `record` olmaktan çıkarılır ya da `Approver` event'e taşınır | 1 karar |
| **T-4** | K-5'in `✅`'i geri alınır → `incelenmedi`. Mimari test + `default(ImmutableArray<T>)` ölçümü bir Engineering turuna yazılır | 1 ölçüm turu |
| **T-5** | K-4'ün iz mekanizması K-1'den bağımsızlaştırılır (önerilen: K-3 M-4'ün event zinciri) **veya** K-4 `ADR-0004`'e taşınır. Künye `requires:`/`depends_on:` okları düzeltilir | 1 karar |
| **T-6** | §7 satır 1377 **22** ile yeniden yazılır; §1/§3'ün 40/41'i düzeltilir veya tarihsel işaretlenir; künye "40+4" güncellenir | mekanik |
| **T-7** | K-3 ya çıkarılır ya zorlayıcı eklenir: `BannedSymbols.txt`'in **her satırı için negatif test** + `#pragma` tarayan mimari test (`ENG-0001` T-4) | 1 karar + test |
| **T-8** | OQ1 **karar-başına** cevaplanır (`Measured` · politika tipleri · `ImmutableArray` alanları); cevaplanana kadar K-5/K-6 `✅` almaz | 1 karar |

### Yüksek

| # | Talep |
|---|---|
| **T-9** | Künye `principles:` ile §3 **aynı sözlüğü** kullanır (`DP*` ya da Anayasa ilkeleri — ama biri) |
| **T-10** | Yanlışlanma ölçütü `ENG-0002` T-2'ye göre düzeltilir (*"aynı saldırı satırına ulaşan `AUDIT_FIXED_*`"*); `W3` ve `W5e` 22 içinde **koşullu** işaretlenir |
| **T-11** | §0.10'a bir **"gövdeye uygulandı mı"** doğrulama tablosu eklenir — §0.8'in `version:` hizalama vakasının (satır 262-264) kurumsallaşmış çaresi. Her changelog kalemi için: `grep` komutu + sonucu |

### Orta

- **T-12** — `ADR-0004`/`ADR-0005` künyelerine `provides:` eklenir; devredilen yetenekler
  (`Authority Token`, `Canonical Identity`) bugün **hiçbir belgenin** taahhüdü değil.
- **T-13** — §2.6'nın kaydettiği **global olmayan ID uzayı** riski (satır 620-633) 22'nin
  içindeki `W22`/`W5a`/`W5b` için yeniden kontrol edilir; dalga-kapsamlı ID'ler daralan
  kapsamda daha kolay çakışır.
- **T-14** — §4.1'in *"güven sınırı **çağrı grafiğidir**"* cümlesi (satır 708), §4.2 ile
  birlikte gövdeden çıkacağı için ayrıca düzeltme gerektirmez — ama **çıkmazsa** `ENG-0002`
  T-1 açık kalır. T-1'in doğrulaması bunu da kapsamalıdır.

## Tekrar-sınav koşulu

T-1 … T-8 kapandığında **yeni ve bağımsız** bir tur açılmalıdır ve **bu kaydın yazarı o turu
yapamaz** (GOV-000 G2). Ayrıca G4 gereği o tur **Engineering boyutundan** olmalıdır — bu
belgenin son üç turu (`SKR-049`, `SKR-050`, bu kayıt) Scientific ağırlıklıdır ve B-2.1'in
gösterdiği gibi **aynı boyutun tekrarı sistematik kör nokta üretir**.

O turda sınanacak tek soru: *"Gövde gerçekten daraldı mı, yoksa yalnız changelog'a bir
'daraldı' satırı mı eklendi?"* — ve cevabı `grep` ile verilmelidir, okuyarak değil.

## Bu incelemenin kendi yanlışlanma koşulu (Madde X)

Bu kayıt **yanlıştır** eğer:

1. **B-1 çürürse:** ADR-0003 v0.5.0'ın §3/§4.1/§4.2/§6/§7/§8'inde K-1 ve K-2 **yoksa**.
   Ölçüm: `grep -a -n "K-1|K-2|K-3|K-4|K-5|K-6"` → bu dosyada **110 eşleşme** (v0.5.0'ın
   künye düzeltmesinden önceki anlık görüntü). Bunlardan `K-1|K-2` için satır 1063-1418
   aralığında **17 eşleşme** saydım; ayrıca §4.1 (694-813) ve §4.2 (816-940) **tam bölüm**
   olarak duruyor. B-1'in tablosundaki yedi yerin **her biri satır numarasıyla** verilmiştir;
   herhangi birinin bulunamaması o satırı düşürür, **yedisinin birden** düşmesi verdict'i
   `wounded`'a indirir.
   > **Not (kendi disiplinim gereği):** bu sayıyı ilk yazışımda "18" demiştim; yeniden
   > saydığımda 17 çıktı ve düzelttim. Bir manşet sayının kaynağı kontrol edilmeden
   > yazılmasını eleştiren bir kayıt, kendi sayısını kontrol etmek zorundadır.
2. **B-4 çürürse:** `ENG-0002`'de K-5'in mekanizmasını (dönüş tipi / mimari test /
   `ImmutableArray` deliği) ölçen bir bölüm gösterilirse. Ben 11 eşleşmenin tamamını taradım
   ve bulamadım; kaçırmış olabilirim — **bir bölüm adı gösterilmesi bu bulguyu düşürür.**
3. **B-3 çürürse:** `DecayPolicy.Disabled`'ın `Approver` alanının `with` ile yazılamadığı
   gösterilirse. `ENG-0002`'nin `[K2]`/`[K3]` çıktısını devraldım ama **kendim derlemedim** —
   `Bash`/`dotnet` bu context'te koşturulmadı. **DOĞRULANMADI:** bu tek bulgu, devralınmış bir
   ölçüme dayanıyor; `ENG-0003` onu bağımsız olarak yeniden üretmelidir.
4. **B-2.1 çürürse:** `SCAN-03:433-436`'nın atıf yaptığı `ROADMAP.md:19` vakası, "iki ardışık
   **aynı boyut** turu" değil "iki ardışık **farklı boyut** turu" ise. Ben `SCAN-03`'ün
   metnini okudum (*"iki ardışık bağımsız **scientific** turdan da geçti"*) ama
   `ROADMAP.md:19`'u **doğrudan okumadım** — satır numarası bayat olabilir
   (`work-protocol.md` §3.5/2).

### Bu incelemenin bilinen sınırları

- **Kod çalıştırılmadı.** Hiçbir spike derlenmedi, `dotnet test` koşulmadı. Bu bir *Scientific*
  turdur; mühendislik iddialarının tamamı `ENG-0001`/`ENG-0002`'den **atıflı** olarak alındı
  ve kaynak paragrafları okunarak doğrulandı — ama yeniden ölçülmedi
  (`work-protocol.md` §3'ün ölüm-kalım kuralı: uydurmuyorum, sınırı yazıyorum).
- **`ENG-0003` turunun bulguları bilinmiyor** ve **tahmin edilmedi.** Bu kayıttaki hiçbir
  cümle onun sonucuna dayanmıyor. Çakışma olursa iki kayıt bağımsız olarak okunmalıdır.
- **`DEFECT-PATTERN-MAP.md` bu turda açılmadı.** 22'nin üye listesi ADR'nin **kendi §4 kapanma
  tablolarından** sayıldı ve `ENG-0002` E-5.1 ile karşılaştırıldı; ikisi uyuştu. Haritanın
  kendisiyle üçüncü bir karşılaştırma yapılmadı — B-6.1 bu sınırla okunmalıdır.
