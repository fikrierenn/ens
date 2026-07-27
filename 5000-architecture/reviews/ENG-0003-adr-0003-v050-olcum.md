---
id:            ENG-0003
title:         ADR-0003 v0.5.0/v0.6.0 — mühendislik ölçümü (üçüncü tur, bölme sonrası)
type:          review
canon:         false
origin:        ADR-0003-kernel-invariant-hardening.md v0.5.0 → v0.6.0 (ölçüm sırasında değişti)
depends_on:    [ADR-0003, ENG-0001, ENG-0002]
referenced_by: []
status:        draft
owner:         ens-backend-architect
version:       1.0.0
last_reviewed: 2026-07-27
maturity:      M0
dimension:     Engineering
verdict:       belge hazır DEĞİL (§2.0) · K-3 koşullu · K-4 hazır değil · K-5 koşullu · K-6 koşullu
---

# ENG-0003 — ADR-0003 v0.5.0 → v0.6.0 ölçümü

> ### Manşet
> **Bölme, kararların kendisi için TUTUYOR — belgenin hâli için TUTMUYOR.**
>
> K-3/K-4/K-5/K-6'nın hiçbiri `ADR-0005`'in kanonik kimlik tipine bağımlı değildir; `ADR-0005`
> reddedilse dördü de ayakta kalır (§2). Bu, v0.5.0'ın ana iddiasıdır ve **ölçümle
> doğrulanmıştır**. `22` sayısı **aritmetik olarak doğrudur** (§3) ve yanlışlanma yolu 22'nin
> 22'si için açıktır (§3.1). D-6 düzeltmesi **tutmuştur** (§5).
>
> Ama: (1) bölme **yalnız changelog'da** yapıldı — gövdede hâlâ **33** `K-1`/`K-2` atfı var ve
> `22` gövdede **sıfır** kez geçiyor (§2.0); (2) `default(DecayRate).Value == 0.0` ölçüldü —
> K-4 kapattığını iddia ettiği sentinel'i geri açıyor (§6.1); (3) K-4'ün tek azaltması
> `ADR-0004`'ün zayıflattığı mühre dayanıyor ve bu **hiç anılmıyor** (§2.2).
>
> **Ölçülen taban: 373/373, değişmedi. `Ens.Kernel/`'e dokunulmadı.**

> ### ⚠️ ÖLÇÜM SIRASINDA BELGE DEĞİŞTİ — v0.5.0 → v0.6.0
> Ölçüm v0.5.0 üzerinde başladı. Ortasında `c15a584` (*"actually perform the split v0.5.0 only
> announced"*) commit'i ADR'yi **380 satır** değiştirdi ve `SKR-051`'i (863 satır, verdict
> **`refuted`**) ekledi. `work-protocol.md` §3.5 gereği **her bulguyu v0.6.0'a karşı yeniden
> ölçtüm**; bu belge v0.6.0 durumunu raporlar ve v0.5.0 ölçümlerini *tarihsel kanıt* olarak
> saklar (hangi bulgunun neyi tetiklediği izlenebilsin diye).
>
> **Yakınsama kaydı (dürüstlük gereği):** §2.0 (bölme gövdeye uygulanmadı) ve §2.2 (K-4'ün
> K-1 bağı koptu) bulgularına `SKR-051` **bağımsız olarak ve önce** ulaştı (T-1 ve
> §0.11 "K-4'ün K-1 bağımlılığı"). İki boyutun **aynı** kusuru bulması, kusurun gerçekliğini
> güçlendirir — ama bu ölçümün özgün katkısı değildir ve öyleymiş gibi sunulmuyor.
> `SKR-051`'in gövdesini **okumadım**; yakınsamayı v0.6.0'ın changelog'undan (§0.11) tespit
> ettim. Bu turun özgün bulguları: **§3.2, §5.2, §6.1, §6.2, §6.3, §7.2, §8.1-8.3.**

## 0. Bağımsızlık kaydı — G4 anlamında bu AYRI BİR BOYUT DEĞİLDİR

`ENG-0002` bunu kendi raporuna yazmıştı; tekrarlıyorum çünkü **hâlâ doğru ve hâlâ bağlayıcı**:

> Bu, `ADR-0003` üzerindeki **Engineering** boyutunun **üçüncü** ölçümüdür
> (`ENG-0001` → v0.1.0, `ENG-0002` → v0.3.0/v0.4.0, `ENG-0003` → v0.5.0).
> GOV-000 **G4** *"≥2 bağımsız validator (**farklı boyutlardan**)"* der. Aynı boyutun üçüncü
> turu **G4'ü ilerletmez** — yalnızca Engineering boyutunun kendi bulgularını tazeler.
> v0.5.0'ın ikinci boyutu paralel `SKR-051` turudur; **onun sonucu bu belgede tahmin
> edilmemiştir** ve edilmemelidir.

Ayrıca G3: bu belge bir **validation**'dır, **approval değildir**. Aşağıdaki
`Accepted'a hazır` işaretleri bir onay değil, mühendislik boyutunun **itirazının olmadığı**
beyanıdır. Statü değişimini owner yapar.

**Bu turda `Ens.Kernel/` üretim koduna dokunulmadı** (ADR `draft`, Madde VII). Bütün spike'lar
`D:\Temp\claude\D--Dev-ENS\762f2e12-fd9e-4fde-81d3-669100cabb34\scratchpad` altındadır.

## 1. Ölçüm ortamı ve taban

| Ölçüt | Değer |
|---|---|
| SDK | .NET 10 (`net10.0` hedefi; `Ens.Kernel.dll` net10.0'a derlendi) |
| Ölçüm tarihi | 2026-07-27 |
| Ölçülen sürüm | `ADR-0003` **v0.5.0**, `last_reviewed: 2026-07-27` |
| Yan belgeler | `ADR-0004` v0.1.0, `ADR-0005` v0.1.0 (ikisi de `status: draft`, `skeptic_review: pending`) |

### 1.1 Taban — `dotnet test`, ÇALIŞTIRILDI

```
dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj --nologo
Başarılı!  - Başarısız: 0, Başarılı: 373, Atlanan: 0, Toplam: 373, Süre: 231 ms
```

**373/373 — değişmedi.** `ENG-0001` ve `ENG-0002`'nin tabanıyla aynı. v0.5.0 salt belge
değişikliğidir; kernel'e tek satır girmedi. Bu **doğru davranıştır** (Madde VII).

İki derleme uyarısı var, ikisi de test projesinde ve ikisi de kozmetik
(`xUnit2012` `AdversarialWave_SecurityTests.cs:182`, `xUnit2031`
`AdversarialWave_InvariantTests.cs:653`). **Not:** bu ikisi `WarningsAsErrors` tartışmasının
(§5) ilk kurbanları olurdu — bugün uyarı, yarın hata.

## 2. ANA ÖLÇÜM — bölme teknik olarak tutuyor mu?

### 2.0 BLOKE EDİCİ — bölme **yalnızca changelog'da** yapıldı; gövdede yapılmadı

Bu, dört sorunun hepsinden önce gelir çünkü diğer üçünü de zehirliyor.

`§0.10` bölmeyi ilan ediyor: *"ADR-0003 (bu belge) → **K-3, K-4, K-5, K-6**"*.
**Gövdeyi ölçtüm. Bölme uygulanmamış.**

| Ölçüm | Komut | Sonuç |
|---|---|---|
| `K-1`/`K-2` geçen satır, **§0.10'dan sonra** (gövde = satır 493+) | `grep -c "K-1\|K-2"` | **33** |
| `### K-1 —` başlığı gövdede var mı | satır **694-815** | ✅ **tam gövdesiyle duruyor** (mekanizma, prior art, maliyet, 3 reddedilen alternatif, R1-R6) |
| `### K-2 —` başlığı gövdede var mı | satır **816-942** | ✅ **tam gövdesiyle duruyor** (4 adımlı `Canonicalize`, M-3, M-4, `ToUpperInvariant` kararı) |
| **`22` sayısı gövdede geçiyor mu** | `grep` | ❌ **SIFIR kez.** Yalnız §0.10'da |
| `ADR-0004`/`ADR-0005` gövdede anılıyor mu | `grep -n` | ❌ **SIFIR.** Dört eşleşmenin dördü de §0.10 içinde (satır 437, 438, 465, 485) |

Somut kanıtlar (gövdeden, birebir):

- **§3 Kapsam (satır 657-660)** — hâlâ bölme öncesi: *"**Kapsamda:** P1 (11 üye), P2 (11 üye),
  P3 (6), P4 (5), P6 (5), P7 (6). Toplam 44 → 41 iddiası yerine bu ADR **40** iddia ediyor."*
- **§7/1 Failure condition (satır 1377)** — *"İddia: **K-1…K-6** uygulandığında **40** kimlik
  kapanır"*. Yani ADR'nin **birincil yanlışlanma koşulu** hâlâ altı karara bağlı.
- **§7 OQ3** — *"**K-1 mührü** çok-process/dağıtık kernel'e nasıl taşınır?"* — başka bir
  belgenin kararı hakkında, bu belgenin kabul-öncesi açık sorusu.
- **§7 OQ4, OQ5** — ikisi de `K-2` hakkında (`confusables.txt`, Unicode sürüm göçü).
- **§7 OQ6** — *"Bu **altı** karar hangi sırayla uygulanır? **K-1 ve K-2 birbirine bağlıdır**"*.
- **§8 İzlenebilirlik tablosu (satır 1417-1418)** — `K-1` ve `K-2` için **`// TRACE: ADR-0003
  K-1`** ve **`// TRACE: ADR-0003 K-2`** satırları **zorunlu** kılınıyor. Yani ADR-0003 hâlâ
  başka belgelere ait kararlar için kod izi dayatıyor. `ADR-0004`/`ADR-0005`'in kendi TRACE
  satırı **yok**.

> ### Bu, "changelog ≠ gövde"nin **ALTINCI** tekrarıdır
> Sicil: `SKR-050` (v0.3.0 — 9 kararın 7'si uygulanmamış), `ENG-0002` (D-7: `8 DP / 78 P`),
> ve şimdi v0.5.0'ın **bölmesi**. İronisi şu: §0.10'un `D-7` maddesi tam bu kalıbı teşhis edip
> *"ilan edip uygulamamaktansa **hiç ilan etmemek** dürüsttür"* diyor — ve **aynı bölümde**,
> `P`→`DP` için uyguladığı bu disiplini **bölmenin kendisine uygulamıyor.** Doğru teşhis,
> yanlış kapsam.

**Bu tek başına `Accepted`'ı bloke eder.** Madde VII gereği kod bu belgeye dayanacaksa,
belgenin **hangi kararları içerdiği** kod yazarı için belirsizdir: §0.10 "dört" der, §3/§7/§8
"altı" der. Bir uygulayıcı §8'i okur ve `// TRACE: ADR-0003 K-1` yazar — `ADR-0004` ise
`draft`. Madde VII **ihlal edilmiş olur ve kimse fark etmez.**

#### v0.6.0 durumu — **kısmen düzeltildi; kalıp daha küçük ölçekte TEKRARLADI**

`c15a584` bu bulguyu (ve `SKR-051` T-1'i) kısmen kapattı. Yeniden ölçtüm:

| Öge | v0.5.0 | **v0.6.0** | Durum |
|---|---|---|---|
| `### K-1` / `### K-2` normatif gövdeleri | 250 satır | **çıkarıldı** → `§4:798-826` "⛔ DEVREDİLDİ" kutusu | ✅ **düzeldi** |
| `§7/1` failure condition | *"K-1…K-6 → **40**"* | *"**K-3…K-6** uygulandığında **22** kimlik kapanır"* | ✅ **düzeldi** |
| `K-1`/`K-2` atfı, gövdede (`§1`+) | 33 | **31** | ⚠️ 2 azaldı |
| **`§3 Kapsam`** | *"P1 (11), P2 (11) … bu ADR **40** iddia ediyor"* | **AYNEN AYNI** (satır 761-764) | ❌ **düzelmedi** |
| **`§7/1`'in kalanı** | — | Düzeltme notu **eklendi**, eski metin **silinmedi**: aynı maddede *"P1: 11, P2: 11 … = 44 üye … doğrulanabilir çekirdek **40**"* (satır 670-672) | ❌ **aynı madde iki sayı taşıyor** |
| **`§7/2`** | *"40'ın tamamına uygulanabilir"* | **AYNEN AYNI** (satır 674) | ❌ |
| **`§7/3`** | *"**K-1** onu kapatmaz, sayı **39**'a iner"* | **AYNEN AYNI** (satır 675) | ❌ kapsam dışı karar hakkında |
| **`§8 TRACE` tablosu** | `K-1` ve `K-2` satırları | **AYNEN AYNI** (satır 705, 710) | ❌ başka belgelerin kararlarına kod izi dayatıyor |
| **`§1 Bağlam`** | *"altı mimari kararın **41**'ini"* | **AYNEN AYNI** | ❌ |

> **Kalıp yedinci kez tekrarladı — bu sefer düzeltmenin kendi içinde.** `§7/1`'e *"geçerli
> olan tek sayı **22**'dir"* diyen bir not eklendi, **ama notun hemen altındaki üç satır hâlâ
> `44` ve `40` diyor.** Yani düzeltme, düzelttiğini iddia ettiği cümlenin **yanına** yazıldı,
> **yerine** değil. `§0.11` bölmeyi *"yapıldı ve **doğrulandı** (normatif bölüm 2 → 0)"* diye
> ilan ediyor; doğrulama **normatif bölüm sayısına** bakmış, **atıflara ve sayılara** değil.

**Ölçümün geri kalanı, kapsamın K-3..K-6 olduğu varsayımıyla yapılmıştır.**

### 2.1 K-3 ↔ ADR-0005 (K-2) bağımlılığı — **YOK. Bölme K-3 için tutuyor.**

K-3'ün dört mekanizmasını, dokunacakları **gerçek kernel yüzeyine** karşı ölçtüm.

| Mek. | Dokunduğu yüzey (ölçüldü) | Kimlik tipi taşıyor mu |
|---|---|---|
| **M-1** saat portu | `DomainEvent.cs:19` — `Timestamp { get; init; } = DateTimeOffset.UtcNow`<br>`ActuationLayer.cs:130,140,143,146,152,160,163,168` — **8 imza** `DateTimeOffset at` alıyor | ❌ hayır |
| **M-2** kabul aralığı | `CompanyMemory.cs:80,87,101` — `AssertedAt`; `:206` mevcut kısmi kontrol | ❌ hayır |
| **M-3** as-of yüklemi | `CompanyMemory.Retrieve/FindStale/FindWeaklyAttributed` (`:247, 365, 391`) | ⚠️ **dolaylı** — aşağıya bak |
| **M-4** replay değişmezi | `DomainEvent.EventId` — tipi **`Guid`** (`DomainEvent.cs:11`) | ❌ hayır |

**Sonuç: K-3, `ADR-0005`'in kanonik kimlik tipine bağımlı DEĞİLDİR.** `ADR-0005` reddedilse
bile K-3 aynen uygulanabilir. Bölme K-3 için **teknik olarak geçerlidir.**

Üç doğrulanmış ayrıntı, ADR'nin lehine:

1. **§2.1'in "saat zaten var" iddiası doğrudur — ölçüldü.**
   `CompanyMemory.cs:144,152,154` — `private readonly TimeProvider _time;` ve
   `TimeProvider? timeProvider = null` ctor parametresi **zaten mevcut**; `:201`
   `_time.GetUtcNow()` çağrılıyor. Yani `A1`/`A2` gerçekten bir **veri kabul** kusurudur,
   saat yokluğu değil. `ENG-0001` bunu bulmuştu; v0.5.0'da hâlâ geçerli.
2. **M-1'in üretim tarafındaki tek gerçek ihlali ölçüldü:** kernel'in tamamında
   `DateTimeOffset.UtcNow`/`DateTime.Now` **tek bir yerde** geçiyor — `DomainEvent.cs:19`.
   (`CompanyMemory.cs:201`'deki `GetUtcNow()` `TimeProvider` üzerindendir, ihlal değil.)
   Yani M-1'in `BannedApiAnalyzers` yükü **1 satırdır**, tarama sonucu değil tahmini değil.
3. **M-1'in `ActuationLayer` maliyeti ADR'nin yazdığından yüksek değil, doğru:** 8 imza.

#### İki düzeltme talebi (bloke edici değil, ama Accepted öncesi)

- **T-1 — `OccurredAt` diye bir alan YOK.** K-3 M-4 (`:964`) *"`OccurredAt` monotonluğunu
  doğrular"* diyor. Kernel'de alan adı **`Timestamp`**'tır (`DomainEvent.cs:19`); `OccurredAt`
  kernel'in **hiçbir yerinde geçmiyor** (`grep`: 0 sonuç). Uygulanabilirliği etkilemez ama
  ADR'nin kod okumadan yazıldığı yerlerden biridir ve `SKR-049` T-B (yanlış künye → sessiz
  fail-open) ile aynı sınıftandır.
- **T-2 — M-3'ün dolaylı bağı yazılmalı.** `CompanyMemory.Retrieve(string purposeType,
  DateTimeOffset asOf, double contextDecayRate = 0.01)` — **tek imza, üç karar**:
  `purposeType` → K-2 (`ADR-0005`), `asOf` → K-3 M-3, `contextDecayRate = 0.01` → K-4.
  Bu bir *bağımlılık* değil, bir **çakışma**dır: aynı imza üç ayrı belgeden üç kez kırılacak.
  Bölme bu maliyeti **görünmez kıldı** — eski §7 OQ6 ("sıra bir uygulama planı gerektirir")
  bunu hiç değilse kaydediyordu. Bölme sonrası **hiçbir belgede uygulama sırası yok.**

### 2.2 K-4 — K-2'ye bağımlı DEĞİL, ama **K-1'e (ADR-0004) iki yerden bağımlı**

Görev metni K-4'ün *kimlik-anahtarlı koleksiyon* taşıyıp taşımadığını sordu. **Taşımıyor.**
`DecayRate`, `StaleThreshold`, `MagnitudeFloor` sayısal kısıtlı tiplerdir;
`DecayPolicy`/`CuratorPolicy`/`ProposalPolicy`/`GatePolicy` varyant tipleridir. Hiçbiri
`ToolName`/`PurposeType`/`PackName` anahtarlı bir sözlük ya da küme tutmuyor.
**K-2 bağımlılığı: YOK.**

Ama ölçüm başka bir bağımlılık buldu ve o **bölmenin gerçek kırılma noktasıdır.**

#### K-4'ün K-1 bağı — iki nokta, ikisi de gövdeden

| Satır | Metin | Neye bağlanıyor |
|---|---|---|
| `ADR-0003:1063` | *"`Disabled` dalı **iz yayar** (`PolicyDisabled` event'i, **K-1 mührüyle**)"* | K-1 → `ADR-0004` |
| `ADR-0003:1120-1121` (**R12**) | *"Azaltma: `Disabled` üretimi **K-1 mührü ister** (yalnız policy-otoritesi verebilir)"* | K-1 → `ADR-0004` |

R12, K-4'ün **kendi ilan ettiği yeni saldırı yüzeyidir**: *"`Disabled` yeni bir bypass
yoludur… Fark: eskisi sessiz ve izsiz, yenisi **açık ve izli**."* Bu farkı üreten tek şey
mühürdür. Şimdi mühür başka bir belgede — ve **o belge mührün hedefini küçültüyor**:

> `ADR-0004:101-102`: *"mühür **kazayla ve gündelik** taklidi durdurur; **kararlı** bir
> saldırganı durdurmaz ve durdurduğunu **iddia etmez**."*
> `ADR-0004:95`: `Unsafe.As` ile yerinde mutasyon → ❌ durdurulamıyor.

Yani K-4'ün R12 azaltması bugün şu hâle geliyor: *"kontrolü kapatmak için mühür gerekir —
ama mühür, kernel'e kod yazabilen birine karşı korumuyor."* Bir **politika kapatma**
yetkisinin tehdit modeli tam olarak kernel'e kod yazabilen roldür. **R12'nin azaltması, kendi
tehdit modelinin altında kalıyor.**

Bu, bölmeyi *geçersiz* kılmaz — ama **bölme bunu görünmez yaptı.** Tek belgedeyken K-1'in
zayıflaması K-4'ün R12'sini otomatik olarak aynı sürüme sokuyordu; şimdi `ADR-0003` v0.5.0,
`ADR-0004`'ün bulgusunu **hiç anmadan** K-4'ü "ölçüt sağlandı ✅" ilan ediyor (`§0.10:448`).

**v0.6.0 durumu:** `§0.11` bu bağı artık **açık borç olarak kaydediyor** (*"§4.4 ve R12
K-1'in mührüne dayanıyor… K-4 Accepted'a gitmeden önce çözülmeli"*) — `SKR-051` de aynı
bulguya ulaşmış. ✅ **Tanı kaydedildi.** Ama **gövde değişmedi**: satır **947**
(*"`PolicyDisabled` event'i, K-1 mührüyle"*) ve satır **1004** (*"`Disabled` üretimi K-1
mührü ister"*) aynen duruyor. Yani K-4'ün metni hâlâ, `ADR-0004`'ün zayıflattığı bir
mekanizmayı azaltma olarak sunuyor. (§2.0'daki kalıp: changelog kabul ediyor, gövde bilmiyor.)

> **T-3 (bloke edici):** K-4'ün R12'si `ADR-0004`'ün sonucuna göre yeniden yazılmalı ya da
> `Disabled`'ın izinin mühürden **bağımsız** bir dayanağı gösterilmeli (ör. event-store'a
> append-only yazma, mühür değil). Aksi hâlde `ADR-0003` bir `draft` belgenin çürüttüğü bir
> azaltmaya dayanıyor — ve `Accepted` olursa Madde VII'e göre **kod o azaltmaya yazılacak**.

#### Ek: `Identity Approver` hiçbir ADR'nin sahibi olmadığı bir tip

`Disabled(string Reason, Identity Approver)` — `Identity`'yi ölçtüm
(`Ens.Kernel/Domain/Identity.cs:9`):

```csharp
public readonly record struct Identity(string Value)
```

Pozisyonel `record struct`: **public kurucu, doğrulama yok.** `new Identity("")`,
`new Identity(" ")`, `default(Identity)` (→ `Value == null`) hepsi geçerli.

- `ADR-0005` K-2'nin kapsamı **altı rol**: `ToolName`, `PurposeType`, `PackName`, `AdapterId`,
  `ContextKey`, `OwnerName`. **`Identity` listede yok** (`ADR-0003:818-819` — doğrulandı).
- `ADR-0005` §5 kapsamı `DP2`'nin 11 kusuru + `W2c`. **`Identity` orada da yok.**
- Yani kernel'in **16 kullanım noktasında** geçen (`grep`: `ActuationLayer`, `CompanyMemory`,
  `DecisionAggregate` ×8, `DomainEvent` ×2, `DecisionEvents`, `Scheduler`) asıl kimlik tipi
  **hiçbir ADR tarafından sahiplenilmiyor.**

Sonuç: K-4'ün hesap-verebilirlik alanı (`Approver`) doğrulanamaz. ADR **`Reason`**'ın
öz-beyan sorununu R13'te dürüstçe kaydediyor ama **`Approver` için aynı sorunu yazmıyor** —
oysa `Approver` boş `string` olabildiği sürece "kim kapattı" sorusunun cevabı da öz-beyandır.
Bu, `DEFECT-REGISTER` 8. kalıbının (öz-beyan kalibre edilmemiş) K-4 içindeki **ikinci**
üyesidir ve **kayıtsızdır**.

### 2.3 K-5 — kap tipi K-2'den bağımsız; **eleman tipi bağımlı (22'nin 2'si)**

Görev metni doğru soruyu sordu: K-5 **kimlik-anahtarlı koleksiyonlar** taşıyor mu?
Taramayı çalıştırdım (§8), 22 ihlalin her birinin eleman/anahtar tipini ayırdım:

| Eleman tipi | Üye sayısı | K-2 (`ADR-0005`) kapsamında mı |
|---|---|---|
| `MemoryRecord`, `DomainEvent`, `ActionTransition`, `Premise`, `ScheduledDecision`, `ReflectiveProposal`, `CapabilityPack`, `ILlmAdapter` | 17 | ❌ hayır — kernel domain tipleri |
| `String` — **serbest metin** (`DecisionAggregate.Alternatives`, `AlternativesIdentified.Alternatives`) | 2 | ❌ hayır — alternatif açıklaması, kimlik değil |
| **`String` — araç adı** (`CapabilityPack.AllowedTools`, `CapabilityPack.RequiresHumanApprovalFor`) | **2** | ✅ **EVET** — K-2'nin `ToolName` rolü |
| `Premise` (`ProofTrace.Premises`) | 1 | ❌ hayır |

**Sonuç: kap (container) kararı K-2'den bağımsızdır.** `IReadOnlyList<T>` → `ImmutableArray<T>`
ve `IReadOnlySet<T>` → `FrozenSet<T>` dönüşümü, `T` ne olursa olsun uygulanabilir. `ADR-0005`
reddedilse K-5 **aynen** uygulanır. Bölme K-5 için **teknik olarak geçerlidir.**

Ama **maliyet bölmeyle gizlendi** ve bu yazılmalı:

> **T-4 — çift kırılma dalgası.** `CapabilityPack.AllowedTools`'un imzası önce K-5 ile
> `IReadOnlySet<string>` → `FrozenSet<string>` olacak, sonra `ADR-0005` kabul edilirse
> `FrozenSet<string>` → `FrozenSet<ToolName>` olacak. **Aynı public imza iki ayrı belgeden
> iki kez kırılır.** Tek belgedeyken §7 OQ6 ("uygulama sırası") bunu hiç değilse *soruyordu*;
> bölme sonrası soru **hiçbir belgede yok** (`ADR-0004` §6, `ADR-0005` §6 — ikisinde de
> uygulama sırası yok).

#### Ölçülen olumlu: K-5'in dayanağı gerçek

`CapabilityRegistry.cs:93-94` — `allowedTools.ToFrozenSet(StringComparer.Ordinal)`. Yani
**alan zaten `FrozenSet<string>`** (`:72-73`); sızdıran tek şey `:63,70`'teki **dönüş tipi
bildirimi**:

```csharp
private readonly FrozenSet<string> _allowedTools;      // :72
public IReadOnlySet<string> AllowedTools => _allowedTools;   // :63  ← K-5 ihlali burada
```

Bu, K-5'in *"örnek kapatıldı, sınıf kapatılmadı"* argümanının **en temiz kanıtıdır** ve
`ADR-0003:1147-1150`'nin iddiası **doğrulanmıştır**: doğru mekanizma bulunmuş, tek yerde
uygulanmış, ama dönüş tipi hâlâ garanti vermeyen arayüzü ilan ediyor. Bu iki üye için K-5'in
maliyeti **iki satır**.

#### `default(ImmutableArray<T>)` — R15 ölçüldü, gerçek

`ADR-0003:1206-1208` (R15) doğru: `default(ImmutableArray<T>)` `null` gibi davranır.
Bu, OQ1 ile birleşiyor — §6'ya bakınız.

### 2.4 K-6 — K-2'ye bağımlılık **YOK**; bölmenin en temiz tarafı

`Measured`/`UnitMeasured` yalnızca `double` üzerinde değişmez taşır. Tarama (§8) 36 ham
`double`/`double?` dönüşü buldu; **hiçbirinin** eleman ya da anahtar tipi bir kimlik değil.
`W5e` için önerilen `AdapterGateway` de `LlmResponse` üzerinde çalışır, `AdapterId` üzerinde
değil.

**Sonuç: K-6, `ADR-0005`'ten tümüyle bağımsızdır.** Ve `ADR-0004`'ten de bağımsızdır — K-6'nın
mekanizmasında mühür geçmiyor. Bölme K-6 için **teknik olarak geçerlidir.**

### 2.5 Bölme verdict'i — **kararların kendisi için tutuyor, belgenin hâli için TUTMUYOR**

| Soru | Cevap | Kanıt |
|---|---|---|
| K-3, K-2 olmadan çalışır mı | ✅ **Evet** | §2.1 — dört mekanizmanın hiçbirinde kimlik tipi yok; `EventId` = `Guid` |
| K-4, K-2 olmadan çalışır mı | ✅ **Evet** | §2.2 — eşikler sayısal, koleksiyon yok |
| K-5, K-2 olmadan çalışır mı | ✅ **Evet** (kap kararı); ⚠️ eleman tipi 2 üyede bağlı | §2.3 |
| K-6, K-2 olmadan çalışır mı | ✅ **Evet** | §2.4 |
| **K-4, K-1 olmadan çalışır mı** | ❌ **HAYIR** — R12 azaltması mühre bağlı | §2.2 |
| **Belge bölünmüş mü** | ❌ **HAYIR** — gövdede 33 `K-1`/`K-2` atfı | §2.0 |

> **Ana ölçümün cevabı:** *"K-3/K-4/K-5/K-6, `ADR-0005` reddedilirse düşer mi?"* → **Hayır.**
> Kanonik kimlik ekseninde bölme **teknik olarak geçerlidir** ve bu, v0.5.0'ın **gerçek
> kazanımıdır**. `ENG-0001`'in ölçtüğü K-1↔K-2 ve K-3↔K-4 bağları K-2 ekseninde
> tekrarlanmıyor.
>
> **Ama iki kusur bölmeyi bugün kabul edilemez kılıyor:** (1) `ADR-0003` gövdesi hâlâ altı
> kararlı belge (§2.0), (2) K-4 → K-1 bağı bölmeyle **koptu ve kaydedilmedi** (§2.2).
> İkisi de metinsel düzeltmedir; **karar tasarımı sağlamdır.**

## 3. 22 sayısının yeniden hesabı — **aritmetik tutuyor, ama "22" brüt sayıdır**

`work-protocol.md` §3.5/4 gereği **yeniden okumadım, yeniden saydım.**
Kaynak: `DEFECT-PATTERN-MAP.md` §3 (`:80-92`), §4 (`:95-107`), §6 (`:125-137`), §7 (`:139-154`).

| Kalıp | Üyeler (tek tek sayıldı) | Sayı |
|---|---|---|
| `DP3` (K-3) | `A1` `A2` `B4` `D4` `W2_L3` `W2_R6` | **6** ✅ |
| `DP4` (K-4) | `A5` `E4` `G2` `H3` `W10` | **5** ✅ |
| `DP6` (K-5) | `W22` `W2_R4` `W2_L4` `W5a` `W5b` | **5** ✅ |
| `DP7` (K-6) | `H4` `W3` `W17` `W5e` `W8a` `W8b` | **6** ✅ |
| | **6+5+5+6** | **22** ✅ |

**Aritmetik doğru.** `SKR-049` T-A'nın (41 ≠ 47) yakaladığı türden bir hata **yok**. Bu, v0.5.0
lehine gerçek bir sonuçtur ve kaydedilmelidir.

### 3.1 Yanlışlanma yolu **22'nin 22'si için uygulanabilir** — ölçüldü

`§0.10:487` diyor ki bu 22 kimliğin `AUDIT_DEFECT_*` testleri `AUDIT_FIXED_*`'a dönmelidir.
Böyle bir test **gerçekten var mı**? Yirmi ikisini de taradım
(`grep -a`, §3.2 NUL-baytı tuzağı gereği):

```
A1..W8b : 22 kimliğin 22'sinde de  AUDIT_DEFECT_<ID>_*  testi bulundu (her biri tam 1 adet)
```

**Yanlışlanma yolu açık ve tam.** `K-0`'ın çöktüğü hata (*"13 kimliğin testi yok"*) burada
tekrarlanmıyor.

### 3.2 YENİ BULGU — 22'nin **6'sında** kapanış testi **belirsiz** (`§12.2` şema kusuru,
ADR-0003'ün kendi kapsamında)

`DEFECT-PATTERN-MAP §12.2` ID uzayının global tekil olmadığını kaydetmiş ve örnek olarak
**`F3`/`G3`/`G4`**'ü vermişti. O üçü `DP2` üyesidir → artık **`ADR-0005`'in** kapsamında.
Ölçüm, kusurun **`ADR-0003`'ün kendi 22'sinde de** olduğunu gösteriyor — ve bu **hiçbir
belgede kayıtlı değil**:

| ID | `AUDIT_DEFECT_<ID>` (kapatılacak) | Zaten var olan `AUDIT_FIXED_<ID>` — **başka bir kusur** |
|---|---|---|
| `A1` | `..._Future_AssertedAt_disables_decay_forever...` | `AdversarialAuditTests.cs:48` — `..._NaN_stake_is_rejected...` |
| `A2` | `..._MaxValue_AssertedAt_is_accepted_and_is_immortal` | `:68` — `..._NaN_confidence_is_rejected...` |
| `B4` | `..._same_record_can_be_verified_1000_times...` | `:236` — `..._ConformanceDeficit_is_clamped...` |
| `D4` | `..._Retrieve_leaks_records_that_did_not_exist_yet...` | `:408` — `..._NaN_confidence_premise_is_now_rejected` |
| `E4` | `..._staleThreshold_zero_is_a_silent_global_curator_off_switch` | `:562` — `..._audit_history_can_no_longer_be_erased_via_downcast` |
| `G2` | `..._zero_magnitude_threshold_turns_every_purpose_type...` | `:778` — `..._Verify_can_no_longer_freeze_decay...` |

**Neden bloke edici:** `§0.10`'un yanlışlanma yolu *"`AUDIT_DEFECT_X` `AUDIT_FIXED_X`'e
dönmelidir"* der. Bu altı ID için `AUDIT_FIXED_X` **zaten yeşil** ve **bambaşka bir şeyi**
doğruluyor. Kapanış denetimini **ad üzerinden** yapan bir sonraki tur, altı kimliği
*"zaten kapanmış"* sayabilir — hiçbiri kapanmamışken. Bu, `DEFECT-REGISTER §0`'ın
*"yeşil panel envanterdir, sağlık değil"* uyarısının tam kendisidir.

> **T-5 (bloke edici):** ADR'nin yanlışlanma koşulu ad-eşleşmesi değil **dosya+ad** eşleşmesi
> olarak yazılmalı (ör. `AdversarialWave_MemoryTests.AUDIT_FIXED_A1_...`), ya da §12.2'nin
> `T9` talebi (`D1a`/`D1b` tarzı benzersizleştirme) **uygulama öncesi** kapatılmalı.
> Kapsam ADR-0003'ün kendi 22'sinin **%27'sidir** — göz ardı edilemez.

### 3.3 "22" brüt; ADR'nin kendi disiplini net sayı ister

Eski manşet **net**ti: `44 brüt − (C3 koşullu, W3 düşük şiddet, W1b kısmi) = 40`.
Yeni manşet **brüt**: `22`, hiçbir çıkarma yok. Ama gövde çıkarmayı hâlâ talep ediyor:

- `ADR-0003:1257` (K-6, `W3` satırı): *"**Ancak §2.7: bu kusur zaten sömürülebilir değildi**;
  kapanış gerçek ama **şiddeti düşük**."*
- `ADR-0003:1263`: *"`W3` için kapanış §2.7'deki **dürüst notla birlikte** okunmalıdır."*

`W3` `DP7`'nin üyesidir ve **hâlâ kapsamdadır**. Eski disiplin uygulanırsa doğrulanabilir
çekirdek **21**'dir. Bu bir hata değil — bir **tutarsızlık**: aynı belge iki farklı sayma
kuralı kullanıyor (§0.10 brüt, §7/1 net). **T-6:** 22 mi 21 mi, tek bir kuralla yazılmalı.

## 4. Gövdede kaç farklı sayı kaldı? — **`ENG-0002` dört bulmuştu; şimdi BEŞ**

`ENG-0002` v0.4.0'da dört sayı saymıştı (**41 / 40 / 43 / "40+4"**). v0.5.0 beşinciyi
(**22**) ekledi ve **hiçbirini kaldırmadı.**

| Sayı | Nerede | Bugün geçerli mi |
|---|---|---|
| **22** | `§0.10:483` — *"ADR-0003'ün kapanma iddiası"* | ✅ v0.5.0'ın manşeti |
| **44** | `§3:658-659` — *"Toplam **44** → …(11+11+6+5+5+6 = **44** üye)"* | ❌ bölme öncesi brüt |
| **41** | `§1:497-498` — *"altı mimari kararın **41**'ini birden kapatacağını iddia etti"* | ❌ `SKR-049` T-A ile çürütülmüş |
| **40** | `§3:658,660` · `§7/1:1377,1380` — *"bu ADR **40** iddia ediyor"*, *"**40** kimlik kapanır"* | ❌ bölme öncesi net |
| **39** | `§7/3:1383` — *"sayı **39**'a iner"* | ❌ `C3` (DP1 üyesi) → `ADR-0004` |

Beşi de **aynı belgede, aynı anda** duruyor. `43` ve `47` yalnız changelog'da (§0.7/§0.9)
kaldığı için "gövdedeki" sayıya dâhil etmedim — ama okuyucu için toplam **yedi** farklı
kapanma sayısı bu belgede geçiyor.

> **Kritik nokta: manşet sayı `§7 Failure conditions`'ta DEĞİL.**
> Madde X gereği ADR'nin yanlışlanma noktası §7'dir. §7/1 hâlâ *"**K-1…K-6** uygulandığında
> **40** kimlik kapanır"* diyor. Yani bugün `ADR-0003` **Accepted** olsa, resmî olarak
> yanlışlanabilir iddiası **40**'tır ve **altı karara** dayanır — `22` yalnızca bir
> changelog satırıdır. `§0.10:488`'in *"22, gövdedeki dört eski sayının yerine geçer"*
> cümlesi bir **niyet beyanıdır, uygulanmamıştır** (§2.0 ile aynı kalıp).

> **T-7 (bloke edici):** `22` (ya da §3.3'e göre `21`) §7/1'e **yazılmalı**; §1, §3, §7/1,
> §7/3'teki 41/44/40/39 ya kaldırılmalı ya *"tarihsel kayıt — geçersiz"* diye işaretlenmelidir.
> `ENG-0002` bu talebi zaten yapmıştı; v0.5.0 talebi karşılamadı, **sayıyı bir artırdı.**

### 4.1 v0.6.0'da yeniden ölçüm — **manşet düzeldi, gövde düzelmedi**

`§7/1` artık *"**K-3…K-6** uygulandığında **22** kimlik kapanır"* diyor. ✅ **T-7'nin
manşet kısmı karşılandı.** Ama eski sayılar **kaldırılmadı**:

| Sayı | v0.6.0'daki yeri | Durum |
|---|---|---|
| **22** | `§7/1:665` — manşet | ✅ geçerli |
| **41** | `§1:601` — *"altı mimari kararın 41'ini"* · `§2.5:700` — *"41'in %32'si"* | ❌ duruyor |
| **44** | `§3:763` · `§7/1:670` | ❌ duruyor |
| **40** | `§2.5:720` · `§3:761,764` · `§7/1:672` · `§7/2:674` | ❌ duruyor — **beş yerde** |
| **39** | `§7/3:675` | ❌ duruyor (K-1 hakkında, kapsam dışı) |

**Beş sayı hâlâ aynı belgede.** Ve en ağırı: `§7/1` **tek bir numaralı maddede** hem
*"geçerli olan tek sayı 22'dir"* hem *"doğrulanabilir çekirdek 40"* diyor. Madde X'in
yanlışlanma noktası, kendi içinde çelişen bir maddedir.

> **T-7 (güncellenmiş, hâlâ bloke edici):** düzeltme notu **eklemek yetmez** — eski metin
> **silinmelidir**. Bugün bir uygulayıcı `§3`'ü okusa kapsamı "P1 + P2 dâhil, 40 kimlik"
> sanır; `§8`'i okusa `// TRACE: ADR-0003 K-1` yazar.

## 5. D-6'nın yeni hâli — **ölçülebilir, ölçüldü, ve düzeltme DOĞRU**

Spike: `scratchpad/d6/` (net10.0, `<Nullable>enable</Nullable>`,
`<WarningsAsErrors>Nullable</WarningsAsErrors>`). Sonuç: **0 uyarı, 6 hata.**

| # | Vaka | Kod | Ölçülen tanı |
|---|---|---|---|
| A | `null` sabiti → non-nullable | `string s = null;` | **`error CS8600`** ✅ |
| B | `default` → non-nullable ref | `string s = default;` | **`error CS8600`** ✅ |
| — | dönüş yolu | `return s;` | **`error CS8603`** ✅ (bonus) |
| H | atanmamış non-nullable alan (`class`) | `public string Name;` | **`error CS8618`** ✅ |
| I | `null` olabilir referanstan okuma | `MeasuredClass? m ⇒ m.Value` | **`error CS8602`** ✅ |
| **C** | **`default(struct)` + non-nullable ref alan** | `default(DecayRate).What` | ❌ **HİÇBİR TANI** |
| **G** | **`default(struct)` ctor atlanır** | `default(DecayRate).Value` | ❌ **HİÇBİR TANI** |
| **D** | **`new T[n]`** | `new string[3]` ⇒ `arr[0]` | ❌ **HİÇBİR TANI** |
| **E** | **`default(ImmutableArray<T>)`** | `.Length` ⇒ NRE | ❌ **HİÇBİR TANI** |
| **J** | **ilklendirilmemiş `ImmutableArray<T>` alanı** | `h.Arr.Length` | ❌ **HİÇBİR TANI** (`CS8618` struct'a uygulanmıyor) |

### 5.1 Verdict: D-6 düzeltmesi **tuttu**

`ENG-0002`'nin çürüttüğü şey tek koda (`CS8618`) daraltmaydı. v0.5.0 daraltmayı geri aldı ve
**`Nullable` kategorisini** seçti. Ölçüm bunu doğruluyor:
**`CS8600` gerçekten hataya dönüyor** ve kategori `CS8600/8602/8603/8618`'i birlikte kapsıyor.
Ayrıca ADR'nin kendi dürüst uyarısı — *"`default(struct)` ve `new T[n]` hiçbir tanı
üretmiyor"* (`§0.10:459-460`) — **ölçümle birebir doğrulandı.** Bu, v0.5.0'daki en temiz
karardır: iddia, tam olarak kanıtın izin verdiği kadar.

### 5.2 YENİ BULGU — `WarningsAsErrors` **iki kanaldan sessizce iptal edilebiliyor**

Bu, `ENG-0001`'in K-3 için ölçtüğü "üç sessiz-başarısızlık yüzeyi"nin D-6'ya da uygulandığını
gösteriyor ve ADR'de **kayıtlı değil**:

| Kanal | Deney | Sonuç |
|---|---|---|
| `#pragma warning disable CS8600` | `Cases2.cs:28-30` | ❌ **hata kayboldu**, derleme geçti |
| `<NoWarn>CS8600</NoWarn>` (proje düzeyi) | `dotnet build -p:NoWarn=CS8600` | ❌ **CS8600 hataları kayboldu** (6 hata → 4) |

Yani `WarningsAsErrors=Nullable` bir **derleme zorlaması değil, bir varsayılandır**.
İkinci kanal (`NoWarn`) daha tehlikelidir: **tek satırlık bir `.csproj` düzenlemesi tüm
assembly'de kategoriyi kapatır** ve `#pragma`'nın aksine kod incelemesinde göze çarpmaz.

> **T-8:** D-6, `Directory.Build.props` + `<NoWarn>` boşluğunu kapatan bir **kontrol testi**
> ister — ör. derleme sonrası `.csproj`/`props` dosyalarında `NoWarn` içinde nullable kodu
> aramak, ya da bilerek bozuk bir dosya derleyip hata beklemek. Aksi hâlde D-6 `Guard.cs`'in
> *"kapatılan N nokta"* hikâyesinin yeni biçimidir: **doğru kural, iptal edilebilir yürürlük.**

## 6. OQ1 açıkken tip seçimi yapılabilir mi? — **K-5 için evet, K-6 için hayır, K-4 için KRİTİK**

Spike: `scratchpad/oq1/`. ADR'nin **kendi taslak kodunu** (satır 1040-1042 ve 1228-1235)
birebir yazdım ve `default`'u ölçtüm.

```
default(DecayRate).Value                = 0        (ctor HİÇ çalışmadı)
default(Measured).Value                 = 0        (sonlu — değişmez İHLAL EDİLMİYOR)
default(ImmutableArray<int>).IsDefault   = True     → .Length  ⇒  NullReferenceException
new DecayRate[3][0].Value               = 0        (üç geçersiz eşik, tek satırda)
```

### 6.1 BLOKE EDİCİ — `default(DecayRate)` **`A5`'i geri açıyor**

`A5`'in tanımı (`DEFECT-PATTERN-MAP §4`): *"`contextDecayRate = 0` → şirket çapında sönüm
kapalı."* K-4 bunu kapattığını iddia ediyor (`ADR-0003:1073`): *"`0` yazılamaz."*

**Ölçüm:** `default(DecayRate).Value == 0.0`. Kurucu **hiç çalışmıyor**, `(0,10]` kısıtı
uygulanmıyor, ve elde edilen değer **tam olarak `A5`'in yasakladığı sıfırdır.** Üstelik
`new DecayRate[3]` tek ifadede üç tanesini üretiyor — ve §5'te ölçüldüğü gibi
`WarningsAsErrors=Nullable` bu satır için **hiçbir tanı üretmiyor**.

> K-4, kapattığını iddia ettiği sentinel'i **tipin `default` değeri olarak geri getiriyor.**
> R14 bunu "delik" diye kaydediyor ama **`A5` kapanış satırını (`:1073`) düzeltmiyor** — tablo
> hâlâ "`0` yazılamaz" diyor. `IsValid` bayrağı + `EnsureInitialized()` çaresi ADR'nin kendi
> ölçütünü ihlal ediyor ve ADR bunu **kendisi söylüyor** (`:1127-1128`: *"bu **yine bir
> çağrı-yeri sayımıdır** ve K-4'ün kendi ölçütünü zayıflatır"*).
>
**v0.6.0'da bu metin AYNEN duruyor** (yeniden ölçüldü). Ve v0.6.0'ı okuyunca bulgu
**güçleniyor** — çünkü ADR yalnız kapanışı iddia etmiyor, **mekanizmayı da yanlış tarif
ediyor** (`§4.4`, satır **930**):

> *"`= 0` yazılamaz çünkü `default(DecayRate)` **geçersizdir ve tip bunu ctor'da yakalar**"*

**Ölçüm bu cümleyi doğrudan çürütüyor:** `default(DecayRate)` için **kurucu hiç çalışmaz** —
`struct` `default`'u bellek sıfırlamasıdır, ctor çağrısı değil. Dolayısıyla ctor "yakalayamaz";
yakalayacak bir şey çalışmıyor. Cümlenin devamı (*"`struct` default sorunu için `IsValid`
bayrağı + `EnsureInitialized()` kapısı"*) sorunu **kabul ediyor** — yani aynı iki satır önce
"tip yakalar" der, sonra "yakalamıyor, bayrak gerek" der. `A5` kapanış satırı (`:957`) ise
ilkine dayanıyor.

> **T-9 (bloke edici):** K-4'ün 5 üyelik kapanış tablosu, `default` deliği kapatılmadan
> **doğru değildir**. Ya OQ1 `class` lehine kapatılır (ölçüldü: §5 CASE-I, `class` için
> `CS8602` **hata** üretiyor — yani zorlanabilir), ya `A5`/`E4`/`G2`/`H3` satırlarına
> *"`default` deliği kapatılmak kaydıyla"* koşulu yazılır.

### 6.2 R14/R15/R20 **tek bir cevabı beklemiyor** — üç ayrı risk

`§7 OQ1` (`:1400`) diyor ki: *"R14/R15/R20 **tek bir cevabı** bekliyor."* Ölçüm bunu
çürütüyor — üçünün şiddeti ve çözümü farklı:

| Risk | Tip | `default` ne veriyor | Değişmez ihlal ediliyor mu | Çözüm |
|---|---|---|---|---|
| **R14** K-4 | `DecayRate` vb. | `0.0` | ✅ **EVET** — `0` yasak sentinel | **`class`** (zorlanabilir) |
| **R20** K-6 | `Measured` | `0.0` | ❌ **HAYIR** — `0.0` sonlu, `Of(0.0)` ile aynı | `struct` **kalabilir** |
| **R15** K-5 | `ImmutableArray<T>` | `IsDefault=true` → **NRE** | ✅ **EVET** | ❌ **seçim yok** — BCL struct'ı |

Yani:
- **K-6 için OQ1 aslında bloke edici değil.** `default(Measured)` `Measured.Of(0.0)` ile
  **özdeştir**; `Measured`'ın tek değişmezi "sonlu ve işaret-normalize" ve `0.0` ikisini de
  sağlıyor. Bu, `Measured`'ın **`struct` kalabileceğinin** ölçülmüş gerekçesidir ve
  `implicit operator double`'ın ergonomisi korunur. **ADR bunu fark etmemiş** — R20'yi
  R14/R15 ile aynı sepete koymuş.
- **K-5 için OQ1'in cevabı K-5'i bağlamıyor**, çünkü `ImmutableArray<T>` seçilmiş bir BCL
  tipidir; `class` yapılamaz. `§0.10:464` bunu doğru tespit ediyor ama **§7 OQ1 hâlâ
  "tek cevap" diyor** — v0.5.0'ın changelog'u ile gövdesi yine çelişiyor (§2.0 kalıbı).
- **K-4 için OQ1 bloke edicidir** (§6.1).

### 6.3 R15'in azaltması **reflection ile uygulanamaz**

`ADR-0003:1208` R15 azaltması: *"alanlar `= ImmutableArray<T>.Empty` ile ilklendirilir;
**mimari test bunu da tarar**."*

K-5'in mimari testi reflection tabanlıdır (`:1154`: *"`typeof(Ens.Kernel).Assembly`'deki tüm
`public` üyelerin dönüş tipleri taranır"*) — ve **reflection bir alanın başlatıcısı olup
olmadığını göremez**: alan başlatıcıları derleyici tarafından kurucuların içine gömülür,
`FieldInfo`'da böyle bir bilgi yoktur. Ölçüm: CASE-J (`Holder.Arr` hiç ilklendirilmemiş) —
derleyici **hiçbir tanı üretmedi**, ve reflection da üretemez.

> **T-10:** R15'in azaltması ya bir **Roslyn analyzer**'a (kaynak düzeyi) çıkarılmalı, ya da
> her `ImmutableArray<T>` alanı **`readonly` + kurucuda zorunlu atama** ile korunmalıdır.
> "Mimari test tarar" bugünkü hâliyle **doğrulanmamış bir azaltmadır** —
> `work-protocol.md` §4'ün *"'yazıldı' YETMEZ"* kuralı buraya uygulanmalı.

## 7. K-3'ün analyzer koşulu Faz 0'da kapatılabilir mi? — **Üçü de kapatılabilir, ama bir seviye ZAYIF mekanizmayla**

Spike: `scratchpad/banned/` (`Microsoft.CodeAnalysis.BannedApiAnalyzers` 4.14.0, net10.0) +
`scratchpad/bannedcheck/`. `ENG-0001`'in üç yüzeyini tek tek sınadım.

### 7.1 Yüzey 1 — "yanlış yazılmış yasak satırı hiçbir tanı üretmiyor": **ÖLÇÜLDÜ, gerçek**

`BannedSymbols.txt`'e iki geçerli, iki bozuk satır koydum:

```
P:System.DateTimeOffset.UtcNow      → warning RS0030  ✅
P:System.DateTime.Now               → warning RS0030  ✅
P:System.DateTimeOffset.UtcNowww    → (hiçbir tanı)   ❌   ← yazım hatası
M:System.Foo.Bar                    → (hiçbir tanı)   ❌   ← uydurma tip
```

**Toplam tanı: 2.** Yasak listesinin **yarısı ölüydü ve derleyici bunu söylemedi.** Bu, tam
olarak `ENG-0001`'in bulgusudur ve v0.5.0'da hâlâ geçerlidir.

### 7.2 Çaresi VAR — ve çalıştırdım

Görev metni sordu: *"bunun bir çaresi var mı (ör. `BannedSymbols.txt`'i doğrulayan bir test)?"*
**Evet. Yazdım, çalıştırdım, iki bozuk satırı da yakaladı.**

`scratchpad/bannedcheck/Program.cs` (~40 satır, reflection): her satırın `DocID`'sini ayrıştırır
(`T:`/`P:`/`M:`/`F:`), tipi ve üyeyi çözmeye çalışır, çözemezse başarısız olur.

```
L3: UYE YOK -> 'System.DateTimeOffset.UtcNowww'   (satir: P:System.DateTimeOffset.UtcNowww)
L4: TIP YOK -> 'System.Foo'                       (satir: M:System.Foo.Bar)

SONUC: gecerli=2  COZULEMEYEN=2
```

> **Bu, K-3'ün koşulunun "kapatılamaz" olmadığının ÖLÇÜLMÜŞ kanıtıdır.** `ENG-0001` yüzeyi
> doğru buldu; v0.5.0 onu Faz 0'a ertelemekte **haklıdır**, ve erteleme **boş bir vaat
> değildir** — mekanizma bugün 40 satırda uygulanabiliyor.

### 7.3 Yüzey 2 — `WarningsAsErrors` yokluğu: **kapatılabilir**

`dotnet build -p:WarningsAsErrors=RS0030` → `error RS0030`. ✅ Ölçüldü.

### 7.4 Yüzey 3 — `#pragma` bastırma: **kapatılabilir ama yalnızca metin taramasıyla**

```
#pragma warning disable RS0030
    public static DateTimeOffset A() => DateTimeOffset.UtcNow;   →  (hiçbir tanı)  ❌
    public static DateTime Bb() => DateTime.Now;                 →  error RS0030   ✅
```

`WarningsAsErrors=RS0030` **açıkken bile** `#pragma` bastırıyor. Aynısı §5.2'de D-6 için de
ölçüldü (`#pragma` ve `<NoWarn>`). Çare kaynak metnini taramaktır (`#pragma warning disable
RS0030|CS86xx` arayan bir test) — uygulanabilir, ama derleyici zorlaması değil.

### 7.5 Kritik değerlendirme — koşul kapanıyor, ama **K-3 kendi ölçütünün altına düşüyor**

`ADR-0003:509` kendi tasarım ölçütünü koyuyor:

> *"Bir karar, ancak **'unutmak' derleme hatası ya da tip hatası üretiyorsa** sınıfı kapatır.
> Çağrı yerlerini elle saymak, sayımın kendisini yanlışlanabilir bir iddia hâline getirir."*

Üç yüzeyin de çaresi ölçüldü ve **üçü de bu ölçütü sağlamıyor**:

| Yüzey | Çare | Derleme hatası mı |
|---|---|---|
| Bozuk yasak satırı | doğrulayıcı **test** (§7.2) | ❌ test — silinebilir |
| `WarningsAsErrors` yok | `.csproj` özelliği | ❌ ayar — `NoWarn` ile geri alınır (§5.2) |
| `#pragma` bastırma | kaynak **metin taraması** | ❌ test — silinebilir |

Yani K-3'ün M-1 mekanizması, K-5/K-6'nın mimari testleriyle **aynı sınıftadır** ve ADR bunu
K-5'te dürüstçe kabul ediyor (`:1201-1202`: *"Analyzer uyarısı reddedildi… Mimari **test**
seçildi çünkü **test bastırılırsa iz kalır**"*).

> **T-11:** K-3'ün M-1'i "derleme zorlaması" olarak sunulmamalı; K-5/K-6 gibi
> **"iz bırakan test"** kategorisine yazılmalıdır. Fark önemlidir: `ADR-0003 §6`
> (meta-kalıp savunması) tablosunda K-3'ün zorlama sütunu **düzeltilmeli**.
>
> **Faz 0 kapanış ölçütü önerisi (ölçülmüş, uygulanabilir):** (1) `BannedSymbols.txt`
> doğrulayıcı testi, (2) `WarningsAsErrors` içeren ve `NoWarn`'da nullable/RS kodu
> **bulunmadığını** doğrulayan proje testi, (3) `#pragma warning disable` tarayıcısı.
> Üçü birlikte K-3'ün koşulunu **kapatır**.

## 8. K-5/K-6 mimari taraması — **yeniden çalıştırıldı: 22 ve 36, DEĞİŞMEDİ**

Spike: `scratchpad/scan/` — `Ens.Kernel.csproj`'a `ProjectReference`, reflection ile
`public`/`nested public` tiplerin `DeclaredOnly` üyeleri.

| Tarama | `ENG-0001` | `ENG-0003` (bugün) | Fark |
|---|---|---|---|
| **K-5** — `IEnumerable` türevi dönen, mühürlü olmayan public üye | 22 | **22** | **0** |
| **K-6** — ham `double`/`double?` dönen public üye | 36 | **36** | **0** |

**Yeniden üretilebilirlik doğrulandı.** Kernel değişmediği için sayılar da değişmedi — bu
beklenen ve **doğru** sonuçtur; v0.5.0 salt belge değişikliğidir.

### 8.1 K-5 — 22 ihlalin dağılımı (7 dosya)

`ActuationLayer.History` · `LlmAdapterRegistry.Adapters` · `CapabilityPack.AllowedTools`,
`.RequiresHumanApprovalFor` · `CapabilityRegistry.Packs`, `.EnabledPacks` ·
`CompanyMemory.AllRecords`, `.Verifications`, `.Retrieve`, `.RetrieveTop`, `.FindStale`,
`.FindWeaklyAttributed` · `DecisionAggregate.Alternatives`, `.Evidence`, `.History`,
`.UncommittedEvents` · `AlternativesIdentified.Alternatives`, `.Evidence` ·
`ReflectiveDoubleLoop.Propose` · `ProofTrace.Premises` · `Scheduler.Schedule`, `.ScheduleTop`

**ADR'nin maliyet tahmini eksik.** `§K-5 Maliyet` (`:1188`) *"Dokunulan üretim dosyası: 5"*
diyor ve `Capability/CapabilityRegistry.cs`, `ProofTrace.cs`, `Domain/Events/DecisionEvents.cs`,
`Domain/ReflectiveDoubleLoop.cs`'i **saymıyor**. Ölçüm: **7 dosya**, 22 üye.
`ProofTrace.Premises` özellikle önemli — proof-trace substratı (P6) canlı koleksiyon dönüyor.

### 8.2 K-6 — 36 ihlalin 30'u gerçek yük

ADR'nin izin listesi `Guard`'ın kendisidir (`:1250`). `Guard`'ın 6 public üyesi
(`Finite`, `NonNegativeFinite`, `NormalizedDeficit`, `OptionalUnitInterval`, `PositiveFinite`,
`UnitInterval`) düşülünce: **36 − 6 = 30 üye** değişir.

`§K-6 Maliyet` (`:1282`) *"Dokunulan üretim dosyası: 6"* diyor. Ölçüm **9 dosya**:
listedeki 6'ya ek olarak `Domain/CompanyMemory.cs` (`MemoryRecord`'un 4 üyesi + `DecayFunction`'ın
3'ü + `CompanyMemory.DecayFactor`/`.Salience`), `Domain/Events/DecisionEvents.cs`
(`DecisionCommitted.Confidence`, `LearningRecorded.AttributionConfidence`),
`ProofTrace.cs` (`Premise.Confidence`, `ProofTrace.Confidence`), `Scheduler.cs`
(`PendingDecision` ×3, `ScheduledDecision.AttentionPriority`).

> **T-12:** K-5 ve K-6'nın maliyet tabloları **ölçümle değiştirilmelidir** (5→7 ve 6→9 dosya).
> Bu, `SKR-049`'un altı maliyet tablosu hakkındaki bulgusuyla aynı sınıftandır: maliyetler
> **tahmin** olarak yazılmış, ölçülebilirken.
>
> **v0.6.0'da yeniden ölçüldü: değişmedi.** `§K-5 Maliyet` satır **1072** hâlâ *"5"*,
> `§K-6 Maliyet` satır **1166** hâlâ *"6"*. Bu tablolar **hiçbir turda ölçülmedi** —
> `SKR-049`'dan beri üç sürüm geçti. Ölçüm 15 dakika sürüyor (bu bölüm onun kanıtıdır).

### 8.3 Bu tarama Faz 0'ın kapanış ölçütü olabilir mi? — **EVET, ve K-5 için ideal**

Görev metni sordu. Cevap: **evet**, üç gerekçeyle:

1. **Sayı bugün belli:** K-5 = 22, K-6 = 36 (30 net). Faz 0 sonunda **0** beklenir. İkili,
   yanlışlanabilir, elle sayım gerektirmeyen bir ölçüt.
2. **Yeni üye otomatik kapsanır** — `Guard.cs`'in "kapatılan N nokta" listesinin çözdüğü
   sorun budur ve ADR bunu doğru gerekçelendiriyor (`:1424-1427`).
3. **Ölçüm ucuz:** tarama ~60 satır reflection, `dotnet test` içinde saniyenin altında koşar.

**İki sınırı yazılı olmalı:**
- Tarama **dönüş tipini** görür, **eleman tipini** ve **`default` durumunu** görmez
  (§6.3 — R15 kapsanmıyor).
- Tarama **parametre tarafını** kapsamıyor. Ölçtüm: **15 public giriş noktası** hâlâ
  değiştirilebilir koleksiyon **kabul ediyor** (`CapabilityPack..ctor(allowedTools)`,
  `DecisionAggregate.Rehydrate(history)`, `Scheduler.Schedule(pending)`, …). K-5 yalnız
  **çıkış** kapısını kapatıyor; `W5b` ("en az bir adapter" değişmezi) tam olarak bir **giriş**
  sorunudur ve `LlmAdapterRegistry..ctor(IEnumerable<ILlmAdapter>)` üzerinden hâlâ açıktır.
  ADR bunu `:1158-1160`'ta "registry `sealed`, mutasyon API'si yok" ile çözdüğünü söylüyor —
  ama savunmacı kopyanın **kurucuda** yapıldığını doğrulamak taramanın kapsamı dışındadır.

## 9. Verdict tablosu

**Belge düzeyinde (v0.6.0): `Accepted`'a HAZIR DEĞİL.** İki sebep:
(a) §2.0/§4.1 — bölme **kısmen** uygulandı; `§3`, `§7/1`'in kalanı, `§7/2`, `§7/3`, `§8 TRACE`
hâlâ altı kararlı belgeyi tarif ediyor ve **beş farklı kapanma sayısı** dolaşıyor;
(b) §6.1 — K-4'ün kapanış tablosu **ölçümle yanlışlandı**.
İkisi de metinsel/kapsam kusurudur; **karar tasarımları sağlamdır.**

> `SKR-051` (Scientific) bağımsız olarak **`refuted`** verdi. Bu ölçüm (Engineering) ona
> **karşı çıkmıyor**; farklı bir eksende ölçtü ve aynı yöne çıktı. İki boyutun yakınsaması
> G4 anlamında anlamlıdır — ama §0'daki uyarı geçerli: bu, Engineering'in **üçüncü** turudur
> ve tek bir boyut sayılır.

**Karar düzeyinde:**

| Karar | Verdict | Gerekçe (ölçüm) |
|---|---|---|
| **K-3** — saat portu + kabul aralığı | **koşullu** | Tasarım sağlam; K-2'den bağımsız (§2.1). Koşul **kapatılabilir ve ölçüldü** (§7.2 — doğrulayıcı test çalıştı). Koşul: Faz 0'ın üç yüzeyi (§7.5) kapanış ölçütü olarak **yazılmalı**; M-1 "derleme zorlaması" değil "iz bırakan test" olarak sınıflanmalı (T-11); `OccurredAt` → `Timestamp` düzeltilmeli (T-1) |
| **K-4** — "kapalı" bir varyanttır | **hazır değil** | İki bloke edici: (1) `default(DecayRate).Value == 0.0` ölçüldü → **`A5` geri açılıyor**, kapanış tablosu (`:1073`) bugün **yanlış** (T-9); (2) R12'nin tek azaltması K-1 mührü, K-1 `ADR-0004`'e taşındı ve **orada zayıflatıldı**, `ADR-0003` bunu anmıyor (T-3). Ayrıca `Identity Approver` hiçbir ADR'nin sahiplenmediği, doğrulamasız bir tip (§2.2) |
| **K-5** — mühürlü snapshot | **koşullu** | En temiz kapanış; kap kararı `ADR-0005`'ten **bağımsız** (§2.3). Koşullar: R15'in azaltması reflection ile **uygulanamaz** (T-10); maliyet tablosu 5 → **7 dosya** (T-12); `W5b` giriş tarafında açık kalıyor (§8.3); `AllowedTools` ikinci bir kırılma dalgası alacak (T-4) |
| **K-6** — çıktı kapısı `Measured` | **koşullu** | Ölçümün en sağlam çıkanı: `ADR-0004`'ten **ve** `ADR-0005`'ten tümüyle bağımsız (§2.4); `default(Measured)` **değişmezi ihlal etmiyor** — R20 K-6 için gerçek risk değil (§6.2). Tek koşul: **OQ2 açık** ve ADR bunu kendi `failure condition §7/7`'sinde *"bu çelişki çözülmemiştir"* diye kaydediyor. Hangi çıktı reddedilir hangisi kırpılır cevaplanmadan kod yazılamaz. Maliyet 6 → **9 dosya** (T-12) |

### 9.1 v0.5.0 neyi gerçekten kazandı — kaydedilmesi gereken

Saldırı sadece kusuru değil, kazanımı da raporlamak zorundadır:

1. **Bölmenin teknik gerekçesi ölçümle DOĞRULANDI.** Dört kararın hiçbiri `ADR-0005`'in
   kanonik kimlik tipine bağımlı değil. `ADR-0005` reddedilse K-3/K-4/K-5/K-6 ayakta kalır.
   Bu, v0.5.0'ın ana iddiasıdır ve **tutuyor**.
2. **Aritmetik ilk kez doğru.** `6+5+5+6 = 22` — `SKR-049` T-A'nın (41≠47) yakaladığı türden
   bir hata yok (§3).
3. **Yanlışlanma yolu tam.** 22 kimliğin **22'sinde** `AUDIT_DEFECT_*` testi var (§3.1).
   `K-0`'ı düşüren hata tekrarlanmadı.
4. **D-6 düzeltmesi ölçümle tuttu.** `Nullable` kategorisi `CS8600`'ü gerçekten hataya
   çeviriyor; ve ADR'nin *"`default(struct)`/`new T[n]` sessiz"* uyarısı **birebir doğru**
   (§5). İddia, tam olarak kanıtın izin verdiği kadar — bu belgede nadir bir erdemdir.
5. **Sonlanma ölçütü ("iki ardışık turda yeni bloke edici bulgu almazsa kapanır") iyi bir
   kuraldır.** Ama bu tur K-4'e **yeni bloke edici bulgu** getirdi (§6.1) — yani ölçüt
   K-4 için **sıfırlanmıştır**, `§0.10:448`'in "✅ ölçüt sağlandı" satırı düzeltilmelidir.

## 10. Talepler

Şiddet sırasıyla. **Mühendislik boyutunun talepleridir; onay değildir (G3).**

### Bloke edici — bunlar kapanmadan `Accepted` olmamalı

| # | Talep | Kanıt |
|---|---|---|
| **T-0** | **Bölmeyi gövdeye TAM uygula.** v0.6.0 normatif K-1/K-2 bölümlerini çıkardı (✅) ama **§3 Kapsam**, **§7/1'in kalan üç satırı**, **§7/2**, **§7/3**, **§8 TRACE tablosunun K-1/K-2 satırları** ve **§1'in "41"i** aynen duruyor. Gövdede hâlâ **31** `K-1`/`K-2` atfı var. Düzeltme notu eklemek yetmez — **eski metin silinmelidir** | §2.0, §4.1 |
| **T-9** | **K-4'ün kapanış tablosu bugün yanlış.** `default(DecayRate).Value == 0.0` ölçüldü — `A5`'in yasakladığı sentinel tipin `default`'u olarak geri geliyor. `new DecayRate[3]` üç tanesini tek satırda üretiyor ve **hiçbir tanı yok**. Ya OQ1 `class` lehine kapatılır (ölçüldü: `class` için `CS8602` **hata**), ya `A5`/`E4`/`G2`/`H3` satırlarına koşul yazılır | §6.1 |
| **T-3** | **K-4'ün R12'si `ADR-0004`'ün bulgusuna göre yeniden yazılmalı.** R12'nin tek azaltması *"`Disabled` üretimi K-1 mührü ister"*; `ADR-0004` mührün **kararlı saldırganı durdurmadığını** ölçtü. `ADR-0003` bunu hiç anmadan K-4'ü "✅ ölçüt sağlandı" ilan ediyor | §2.2 |
| **T-5** | **Yanlışlanma koşulu ad-eşleşmesi olamaz.** 22'nin **6'sında** (`A1` `A2` `B4` `D4` `E4` `G2`) **zaten yeşil** bir `AUDIT_FIXED_<ID>` var ve **bambaşka bir kusuru** doğruluyor. Koşul `dosya+ad` olmalı ya da §12.2'nin `T9` benzersizleştirmesi uygulama öncesi kapanmalı | §3.2 |
| **T-7** | **Manşet sayı `§7 Failure conditions`'a yazılmalı.** Bugün §7/1 hâlâ *"**K-1…K-6** uygulandığında **40** kimlik kapanır"* diyor. Yani ADR'nin resmî yanlışlanma iddiası **40 ve altı karar**; `22` yalnız bir changelog satırı | §4 |

### Yüksek — bloke etmez, açık borç olarak kaydedilmeli

| # | Talep |
|---|---|
| **T-6** | **22 mi 21 mi?** Eski manşet netti (44 brüt − caveat = 40), yeni manşet brüt. `W3`'ün düşük şiddeti gövdede (`:1257`, `:1263`) hâlâ yazılı. Tek sayma kuralı seçilmeli |
| **T-10** | **R15'in azaltması uygulanamaz.** *"Mimari test bunu da tarar"* — reflection bir alanın **başlatıcısı olup olmadığını göremez** (derleyici onu kurucuya gömer). Roslyn analyzer'a çıkarılmalı ya da `readonly` + kurucuda zorunlu atama ile korunmalı |
| **T-11** | **K-3'ün M-1'i "derleme zorlaması" değildir.** `#pragma` ve `<NoWarn>` ikisi de iptal ediyor (ölçüldü). §6 meta-kalıp tablosunda K-5/K-6 ile aynı ("iz bırakan test") kategoriye yazılmalı |
| **T-12** | **Maliyet tabloları ölçümle değiştirilmeli:** K-5 `5 → 7` dosya (`CapabilityRegistry.cs`, `ProofTrace.cs`, `DecisionEvents.cs`, `ReflectiveDoubleLoop.cs` eksik), K-6 `6 → 9` dosya |
| **T-8** | **D-6 bir kontrol testi ister.** `<NoWarn>` proje düzeyinde kategoriyi sessizce kapatıyor ve `#pragma`'nın aksine kod incelemesinde göze çarpmıyor |
| **T-13** | **OQ1 "tek cevap" değildir.** Ölçüm üç ayrı risk gösterdi: R14 `class` ister, R20 `struct` **kalabilir** (`default(Measured)` değişmezi ihlal etmiyor), R15'in **seçimi yok** (BCL struct). `§7 OQ1` yeniden yazılmalı — `§0.10:464` bunu zaten biliyor, gövde bilmiyor |
| **T-14** | **`Identity` hiçbir ADR'nin sahibi olmadığı bir tip.** Kernel'de **16** kullanım noktası; `readonly record struct Identity(string Value)` — public kurucu, doğrulama yok, `default(Identity).Value == null`. K-2'nin altı rolünde **yok**, `ADR-0005` §5 kapsamında **yok**. K-4'ün `Approver`'ı buna dayanıyor |

### Orta

| # | Talep |
|---|---|
| **T-1** | `OccurredAt` diye bir alan yok — kernel'de ad **`Timestamp`** (`DomainEvent.cs:19`). K-3 M-4 düzeltilmeli |
| **T-2** | `CompanyMemory.Retrieve(string purposeType, DateTimeOffset asOf, double contextDecayRate = 0.01)` — **tek imza, üç belge**. Uygulama sırası bölme sonrası **hiçbir belgede yok** (eski OQ6 hiç değilse soruyordu) |
| **T-4** | `CapabilityPack.AllowedTools` K-5 ile bir, `ADR-0005` ile ikinci kez kırılacak. Çift dalga yazılmalı |
| **T-15** | ~~`§0.10:448` — K-4 için *"✅ ölçüt sağlandı"*~~ → **v0.6.0'da kapandı**: sonlanma ölçütünün kendisi `SKR-051` ile çürütülüp geri çekildi (`§0.11`). Bu turun T-9 bulgusu geri çekmeyi **bağımsız olarak destekler**: ölçüt yürürlükte olsaydı K-4 "iki tur temiz" diye kapanacaktı, oysa `default(DecayRate)` deliği **ilk kez bu turda ölçüldü** |
| **T-17** | **`§4.4:930` düzeltilmeli** — *"`default(DecayRate)` geçersizdir ve **tip bunu ctor'da yakalar**"* ölçümle çürütüldü: `struct` `default`'unda **ctor hiç çalışmaz**. Aynı cümlenin devamı sorunu zaten kabul ediyor; iki satır kendi içinde çelişiyor |
| **T-16** | `ADR-0004` künyesi: `principles: [P6, P7]` — K-1 `DP1`/P1 kararıdır, P6/P7 K-5/K-6'nındır. Ayrıca `ADR-0004`/`ADR-0005` `depends_on: [ADR-0003]` diyor ama `ADR-0003`'ün `referenced_by`'ı boş — graf tek yönlü kırık |

### Tekrar-sınav koşulu

T-0, T-3, T-5, T-7, T-9 kapandığında **yeni bir tur** gerekir — ama **Engineering boyutundan
değil.** Bu boyut üç tur harcadı ve G4 anlamında **tek validator sayılır**. Sıradaki tur
`SKR-051`'in (Scientific) sonucuyla birleştirilmeli; `Accepted` kararı iki boyutun
**birlikte** karşılanmasını ister (GOV-000 G4).

**Ölçemediklerim — dürüstlük gereği:**
- **`SKR-051`'in gövdesini okumadım.** Varlığını ve verdict'ini (`refuted`, 8 bloke edici)
  yalnızca v0.6.0'ın `§0.11` changelog'undan öğrendim; bulgularını bu belgeye taşımadım.
  §2.0 ve §2.2'nin `SKR-051` ile yakınsadığını **kendim tespit edip kaydettim** —
  önceliği ona verdim.
- **Bu ölçüm iki sürüme yayıldı.** v0.5.0'da başladı, v0.6.0'da bitti. Her bulgu v0.6.0'a
  karşı yeniden ölçüldü, ama **tek bir tam okuma** yerine iki kısmi okuma yapıldı; v0.6.0'ın
  değiştirdiği 380 satırın tamamını satır satır incelemedim — yalnız bulgularımın dokunduğu
  bölümleri. Kaçırdığım bir v0.6.0 değişikliği olabilir.
- `Ens.Kernel`'e hiçbir karar **uygulanmadı** (ADR `draft`, Madde VII). Yukarıdaki "kapanır /
  kapanmaz" değerlendirmeleri **tasarım ölçümüdür**, uygulama sonucu değil. 22 kimliğin
  gerçekten kapanıp kapanmadığı ancak uygulama sonrası ölçülebilir.
- `ADR-0004`'ün `failure condition 2`'sini (**`BannedApiAnalyzers` `Unsafe`'i yasaklayabilir
  mi**) bu turda sınamadım — o `ADR-0004`'ün mühendislik turunun işidir. §7'nin ölçümü
  (analyzer'ın `#pragma` ile bastırılabildiği) o soruya **kısmi** bir olumsuz sinyal verir,
  ama **DOĞRULANMADI** olarak kayda geçiyorum.
