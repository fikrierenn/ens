---
id: SKR-049
type: skeptic-review
origin: ADR-0003
depends_on: [ADR-0003, DEFECT-PATTERN-MAP.md, AUDIT-WAVE2-SECURITY.md, ADR-0001]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-27
---

# SKR-049 — ADR-0003 (Kernel Invariant Hardening) Saldırısı

> **Bağımsızlık beyanı (GOV-000 G2/G4):** ADR-0003'ü `ens-ai-architect` yazdı,
> `DEFECT-PATTERN-MAP.md`'yi oturum sahibi yazdı. Bu kayıt taze context'te, üçüncü bir rolle
> üretildi. `work-protocol.md` §3.1 gereği ADR'ler **iki boyut** ister; bu kayıt
> **birinci boyuttur (teori/iddia disiplini)**. İkinci boyut (mühendislik/yapısal) ayrıca gelir.
>
> **Kapsam sınırı:** ben kod incelemem. Aşağıdaki her kod atfı, ADR'nin *iddiasını*
> `dosya:satır` ile sınamak içindir — implementasyon kalitesi hakkında hüküm değildir.

## Verdict

**`wounded`** — altı kararın **mekanizmaları** sağlamdır ve gerçekten sınıf-düzeyi kapatma
üretir; ama ADR'nin **tek yanlışlanma noktası olan sayı yanlış hesaplanmıştır** (41 aslında
47, 40 aslında 43), K-1 kendi mührünü `record`/`with` üzerinden **kopyalanabilir** bırakır,
K-4 kendi kod taslağında **K-3'ü ihlal eder**, Türkçe casing kararı Türkçe girdide iddia
ettiğini yapmaz, ve 44 kimliğin 13'ünü barındıran test dosyası **altı maliyet tablosunun
hiçbirinde yoktur**. Hiçbiri kavramsal değil; hepsi giderilebilir. **`refuted` değil** —
çünkü kararların *fikri* doğru, kusurlar *muhasebe ve taslak* düzeyindedir.

| Eksen | Sonuç |
|---|---|
| 1. Sayı iddiası | **refuted** — 41→47, 40→43; `W1b` çifte çıkarılmış, `C3` hâlâ sayıda |
| 2. "Örnek vs sınıf" tuzağı | **wounded** — K-5 kendi meta-kalıbına düşüyor; K-1/K-2/K-4'ün regresyon koruması yok |
| 3. P5/P9 sızması | **survives** — sızma **yok**; v1'in hatası tekrarlanmamış |
| 4. Sayılmayan risk | **refuted** — `with` deliği (UR-1) ve K-4↔K-3 ihlali (UR-2) R1-R20'de yok |
| 5. Açık sorular | **wounded** — OQ2-OQ5 dürüst; **OQ1 ve OQ6 eksik karar** |
| 6. Prior art | **survives** — uydurma atıf **yok**; bir ifade hatası (JEP 486) |
| 7. Türkçe casing | **refuted** — `ToUpperInvariant` `ı`/`i`'yi birleştirir, `İ`/`i`'yi ayırır |
| 8. Maliyet | **refuted** — en çok etkilenen test dosyası altı tablonun hiçbirinde yok |
| 9. İç tutarlılık | **wounded** — `P1..P9` iki anlamda; künye §3'ün tersini ima ediyor |

> **Meta-bulgu:** bu turda bulduğum **dört** kusurun kökü tek bir yerdedir —
> `AdversarialWave_SecurityTests.cs`'in görünmezliği ve **yanlış künyesi** (B-7). Dört NUL
> baytı, sicilin sayımını, güvenlik raporunu ve K-0'ın gerekçesini yanılttıktan sonra bu ADR'nin
> maliyet tablolarını da yanıltmıştır — **dördüncü kez**. ADR kalıbı doğru teşhis edip
> `work-protocol.md` §3.2'yi yazdı ve **aynı belgede kurala uymadı**. Bir kuralı yazmak, ona
> uymak değildir.

## 1. Devralınan zeminin doğrulanması (work-protocol §3.5)

### B-1 — ⛔ ZEMİNDEKİ SAYI ARİTMETİK OLARAK YANLIŞ: "41" gerçekte **47**

Kural §3.5 "devraldığını doğrula" der. ADR düzyazıyı doğruladı (dört atamayı düzeltti,
K-0'ı geri çekti) ama **toplamı hiç toplamadı**. Topladım.

`DEFECT-PATTERN-MAP.md:226` — haritanın manşet iddiası:

> *"**6 karar → 41 kusur** (P1 + P2 + P3 + P4 + P6 + P7 = 12+13+6+5+5+6)."*

Parantez içindeki toplam **47'dir**, 41 değil:

```
12 + 13 = 25 ; +6 = 31 ; +5 = 36 ; +5 = 41 ; +6 = 47
```

Toplama, **P7 eklenmeden** durdurulmuş. Sağlama, haritanın kendi §10 tablosundan bağımsız
olarak yapılabilir (`DEFECT-PATTERN-MAP.md:203-216`): toplam **74** kusur; kapsam dışı olanlar
P5=2, P8=10, P9=15 → 27. `74 − 27 = 47`. İki yoldan aynı sayı çıkıyor: **47**.

`ADR-0003:37-38` bu hatayı **doğrulamadan devraldı** ve tırnak içinde tekrarladı:

> *"...altı mimari kararın **41**'ini birden kapatacağını iddia etti
> (P1+P2+P3+P4+P6+P7 = 12+13+6+5+5+6)"*

Aynı ifade, aynı yanlış toplam. `ADR-0003:54-58` *"bu ADR'yi o zemine kurmadan önce on iki
kimliğin gövdesi okundu"* diyor — gövdeler okundu, **aritmetik okunmadı**.

### B-2 — ⛔ ADR'nin kendi "40" sayısı da türetilemiyor (iki ayrı hata)

`ADR-0003:913-916` (Failure conditions §7/1) — bu ADR'nin **tek yanlışlanma noktası**:

> *"İddia: K-1…K-6 uygulandığında **40 kimlik kapanır** (P1: 11, P2: 11, P3: 6, P4: 5,
> P6: 5, P7: 6 = **44 üye**; eksi `C3` koşullu, eksi `W3`'ün düşük şiddeti, eksi `W1b`'nin
> M-4'e bağımlılığı → doğrulanabilir çekirdek **40**)."*

44 doğru (üye listelerini tek tek saydım: K-1 tablosu `:295-307` 11 satır, K-2 tablosu
`:430-442` 11 satır, K-3 `:508-515` 6, K-4 `:609-615` 5, K-5 `:702-708` 5, K-6 `:790-797` 6).
Ama 44'ten 40'a iniş **iki ayrı hata** taşıyor:

1. **Çifte çıkarma.** `W1b`, P2'nin 11'ine **zaten dahil değil** — `:428` başlığı açıkça
   *"11 üye — `C2` ve `W1b` çıkarıldı"* diyor ve `:430-442` tablosunda `W1b` satırı yok.
   44'ten bir kez daha çıkarılıyor. Aynı kimlik iki kez düşülüyor.
2. **`W3` kapanmayan sayılıyor ama ADR onun kapandığını söylüyor.** `:799` — *"**Altısı da
   kapanıyor**, ancak `W3` için kapanış §2.7'deki dürüst notla birlikte okunmalıdır."*
   Düşük *şiddet* kapanmama değildir. Şiddeti gerekçe göstererek sayıdan düşmek, sayıyı
   yanlışlanabilir olmaktan çıkarır: uygulama sonrası `W3` `AUDIT_FIXED_*`'a **dönerse**
   iddia tutmuş mu olur, yoksa fazla mı sayılmıştır? Metin cevaplamıyor.

Aritmetiği düzelttiğimde tek tutarlı sayı çıkıyor: `44 − C3(koşullu) = **43**`.

### B-3 — Sonuç: dört farklı sayı, hiçbiri diğerinden türemiyor

| Kaynak | Sayı | Durum |
|---|---|---|
| `DEFECT-PATTERN-MAP.md:226` | 41 | **yanlış** — kendi addend'leri 47 veriyor |
| `DEFECT-PATTERN-MAP.md:274` (§12) | 40 | yanlıştan türemiş: 41−(3 yeniden sınıflandırma)=38 bile değil |
| `ADR-0003:37` | 41 | doğrulanmadan devralındı |
| `ADR-0003:198-200`, `:913-916` | 40 | 44'ten çifte çıkarmayla |
| **Aritmetik** | **43** | 47 − 3 yeniden sınıflandırma − 1 koşullu (`C3`) |

Bu, ADR'nin **manşet iddiasıdır** ve Madde X gereği **tek yanlışlanma noktasıdır** (`:911-916`).
Yanlışlanma noktası yanlış hesaplanmışsa, uygulama sonrası test *"40 kapandı mı?"* sorusunu
sorar ve 43 kapansa bile *"iddia tutmadı, fazla kapandı"* gibi anlamsız bir sonuç verir.
`DEFECT-REGISTER` v1'in cezalandırıldığı kusur (`DEFECT-PATTERN-MAP.md:24` — *"v1'in '33'ü
gerçekte ~29-31'di"*) **biçim değiştirerek** üçüncü kez tekrarlıyor: bu kez şişirme değil,
**eksik toplama**.

> **Not — bu, ADR'nin ağır bir kusuru değil, ağır bir *devralma* kusurudur.** Hata haritada
> doğdu; ADR onu kural §3.5'e rağmen taşıdı. Ama sayı ADR'nin *kendi* falsification
> koşulu olduğu için sorumluluk ADR'dedir.

### B-6 — ✅ "4 NUL baytı" iddiası DOĞRULANDI (ve tam olarak dört)

`ADR-0003:145-147` ve `DEFECT-PATTERN-MAP.md:281-283` şunu iddia ediyor: dosyada `W2e`
fixture'ı olarak **4 gerçek NUL baytı** var ve `grep`/`rg` dosyayı binary sayıp atlıyor.

**İkisi de doğru.** Ampirik doğrulama: `AUDIT_(DEFECT|FINDING)_(W1a|W1b|...)` düzenli ifadesi
`AdversarialWave_SchedulerGateTests.cs` ve `AdversarialWave_MemoryTests.cs`'ten sonuç
döndürdü, `AdversarialWave_SecurityTests.cs`'ten **hiç** döndürmedi — oysa `W1b` o dosyada
`:88`'de duruyor. Araç gerçekten atlıyor.

`Read` ile sayım — dört ham NUL, dördü de `// NUL` yorumuyla işaretli:

| Satır | Bağlam |
|---|---|
| `:109` | `[InlineData("Operations\0")] // NUL` — `W1c` |
| `:200` | `[InlineData("read_stock\0")] // NUL` — `W2a` |
| `:278` | `new CapabilityPack("P", "1.0", ["read\0_stock"])` — `W2e` |
| `:280` | `registry.Authorize("read\0_stock")` — `W2e` |

**Tam olarak 4.** Bu, oturum sahibinin sayımının bu turda bağımsız olarak doğrulanan tek
sayısıdır — ve B-1/B-2'nin aksine **doğru** çıkmıştır.

### B-7 — ⛔ ÖLÇÜM ALETİNİN KENDİ KÜNYESİ YALAN SÖYLÜYOR (ADR'nin görmediği kök neden)

ADR ve harita, üç kez yanıltan olayın kök nedenini *"4 NUL baytı"* diye teşhis etti ve
`work-protocol.md` §3.2'yi buna göre yazdı. **Teşhis semptomda durdu.** Gerçek kök neden bir
satır yukarıda: `AdversarialWave_SecurityTests.cs:27-29`, dosyanın kendi künyesi:

> *"**KAYNAK KODLAMASI: bu dosyada hicbir ciplak non-ASCII karakter YOKTUR.** Homoglif/
> zero-width/NUL/RTL saldirilari **BILEREK \uXXXX escape'leriyle yazilmistir** — aksi hâlde
> testin kendisi dosya-kodlamasina bagimli ve okunamaz olurdu."*

Bu beyan **yanlıştır**. Aynı dosyada, çıplak literal olarak:

| Satır | Karakter | Ne olması gerekiyordu |
|---|---|---|
| `:106`, `:241` | Kiril `а` (U+0430) | `а` |
| `:107`, `:198` | ZWSP (U+200B) | `​` |
| `:108`, `:197` | ZWJ (U+200D) | `‍` |
| `:199` | ZWNJ (U+200C) | `‌` |
| `:110`, `:201`, `:266`, `:271` | RTL override (U+202E) | `‮` |
| `:111`, `:202` | NBSP (U+00A0) | ` ` |
| `:218-219` | `é` NFC/NFD çifti | `é` / `é` |
| `:109`, `:200`, `:278`, `:280` | **ham NUL** | `\0` |

Yani künyenin **her iki cümlesi de** yanlış: hem çıplak non-ASCII var, hem escape'ler
kullanılmamış — üstelik künye, bunun sonucunu (*"testin kendisi dosya-kodlamasına bağımlı ve
okunamaz olurdu"*) **doğru öngörüyor** ve tam olarak o sonuç gerçekleşiyor.

**Bu neden bu ADR'nin sorunu:** ADR'nin **tek** yanlışlanma yordamı, uygulama sonrası
`AUDIT_DEFECT_*` testlerinin `AUDIT_FIXED_*`'a dönmesidir (`:916`). O testlerin fixture'ları
kodlamaya bağımlı çıplak literallerdir ve dosya kendi kodlama garantisini **yanlış beyan
ediyor**. Bir `.gitattributes`/`.editorconfig` değişikliği, bir IDE "fix encoding" işlemi ya
da bir merge aracı bu literalleri sessizce dönüştürürse (`Operаtions` → `Operations`), `W2c`
ve `W1c` testleri **kusur ortadan kalkmadan yeşile döner**. Yani ADR'nin yanlışlanma yordamı
**sessizce fail-open**tir.

Bu, sicilin 8. kalıbının (*"öz-beyan kalibre edilmemiş"*) ölçüm aletinin kendisinde
görülmesidir. `DEFECT-PATTERN-MAP.md:246` *"dönmeyen her test bu dosyayı yanlışlar"* diyor;
ama bir testin **neden** döndüğü doğrulanmıyorsa, dönmesi hiçbir şeyi kanıtlamaz —
`work-protocol.md` §4'ün tautoloji uyarısının kodlama düzeyindeki hâli.

> **Talep (T-B):** ADR, K-2'nin kabul kapısına şunu eklemeli: *fixture literalleri `\uXXXX`
> escape'lerine çevrilir ve `AdversarialWave_SecurityTests.cs:27-29` künyesi doğru hâle
> getirilir; ham NUL'lar `\0` olur.* Aksi hâlde 43 kimliğin doğrulanması, doğrulanamayan bir
> alete dayanır. Bu iş **K-2'den önce** yapılmalıdır — çünkü K-2'nin başarısı yalnız bu
> testlerle ölçülebiliyor.

## 2. Sayı iddiası

### B-4 — ✅ ADR'nin kendi DOĞRULANMADI uyarısı **doğrulandı**: `C3` kapanmıyor

`ADR-0003:309-313` dürüst bir uyarı bırakmıştı:

> *"**⚠️ `C3` için dürüst uyarı (DOĞRULANMADI):** `C3`'ün test gövdesi bu turda **okunmadı**.
> ... `C3` de aynı kökten geliyorsa **K-1 onu kapatmaz**. Uygulamadan önce gövde okunmalıdır."*

Gövdeyi okudum — `AdversarialWave_MemoryTests.cs:315-333`:

```csharp
// Kontrol `_index.Contains(record)` -> HashSet deger esitligi kullanir. Yani bellege HIC
// yazilmamis bir nesne, yazilmis birinin alan-klonu ise guard'i gecer
var real  = new MemoryRecord(id, "tedarik", 5, 0.5, Now.AddDays(-300));
memory.Record(real);
var ghost = new MemoryRecord(id, "tedarik", 5, 0.5, Now.AddDays(-300));  // birebir aynı alanlar
Assert.DoesNotContain(memory.AllRecords, r => ReferenceEquals(r, ghost));
memory.Verify(ghost, Now, "hayalet uzerinden");                          // guard'ı geçer
```

`C2` (`:292-312`) ile **aynı kök**: `record` değer-eşitliği, `HashSet`/`Dictionary` anahtarı
olarak kullanılıyor. Ortada taklit edilen bir **yetki** yok; `MemoryRecord`'un `public`
kurucusu zaten meşru bir domain kurucusudur ve K-1 onu kapatmayı **önermiyor** (K-1'in
kapsamı `ToolAuthorization`/`GateResult`/`Proposal`/actuation girişi — `:236-239`).

Dahası: K-1'in mekanizması *referans eşitliği ile mühür doğrulama*dır. `C3`'ün kusuru tam
tersidir — guard **referans eşitliğini kullanmadığı** için (`ReferenceEquals` yerine `HashSet`
değer eşitliği) geçiliyor. Mühür eklemek buraya bir şey katmaz; gereken şey `C2` ile aynı:
surrogate `RecordId` ya da `ReferenceEqualityComparer`.

**Sonuç: `C3` P1'den çıkar, `C2`'nin yanına (P9 / entity-value karışımı) gider.**
ADR'nin kendi koşulu (`:919-920`, failure condition 3) **gerçekleşmiştir**.

### B-5 — Aritmetiğin düzeltilmiş hâli: **43**, ve neden 40 değil

| Adım | Hesap | Sonuç |
|---|---|---|
| Harita P1+P2+P3+P4+P6+P7 | 12+13+6+5+5+6 | **47** (harita 41 diyor — B-1) |
| §2'nin üç yeniden sınıflandırması | −`W2_O1` −`C2` −`W1b` | **44** |
| B-4: `C3` doğrulandı, kapanmıyor | −`C3` | **43** |
| `W3` (§2.7 — kapanıyor, şiddeti düşük) | çıkarılmaz | **43** |

Yani ADR'nin doğru manşeti **"43 kimlik kapanır"** olmalıydı. Bugün yazan sayı 40'tır ve
40'a giden **hiçbir tutarlı yol yoktur** — `W1b` çifte çıkarılmış, `W3` gerekçesizce
düşülmüş, `C3` ise (kapanmadığı artık doğrulandığı hâlde) **hâlâ sayının içinde** duruyor.

> **Kritik nokta — bu sadece bir hesap hatası değil.** `ADR-0003:911-916` bu sayıyı
> *"bu ADR yanlıştır eğer..."* koşulu yapıyor. Yanlış hesaplanmış bir yanlışlanma koşulu,
> yanlışlanabilirlik değil **yanlışlanabilirlik görüntüsüdür** (Madde X). Uygulama sonrası
> 43 kimlik `AUDIT_FIXED_*`'a dönerse ADR "yanlışlanmış" mı sayılacak? Metin bunu
> cevaplayamaz. **Bu, T-A olarak bloke edicidir.**

## 3. "Örnek kapatıldı, sınıf açık" tuzağı

ADR §6 (`:892-905`) bu tuzağa karşı bir öz-denetim tablosu kuruyor ve **yalnız K-3'ü** zayıf
ilan ediyor. Tabloyu depoya karşı sınadım. K-3 gerçekten zayıf — ama **yalnız o değil**.

### B-8 — ⛔ K-5, P6'nın **en yoğun ihlal dosyasını** listelemiyor (kendi meta-kalıbına düşüyor)

`ADR-0003:710` K-5 için şunu yazıyor: *"**Hepsi kapanıyor. Kapanmayan yok.** Bu, altı kararın
**en temiz** kapanışıdır."* Ve `:685-688` bir de emsal veriyor:

> *"`CapabilityRegistry.cs:93` bu dersi **zaten bir yerde öğrenmiş** (`FrozenSet` kullanıyor)
> ... K-5, o tek noktada uygulanan çözümü **sınıf olarak** uygular — `AUDIT-WAVE2 §10.5`'in
> ... **en temiz örneği burasıdır**."*

`:93` atfı doğru (`_allowedTools = allowedTools.ToFrozenSet(StringComparer.Ordinal)`).
Ama aynı dosyada, K-5'in yasakladığı dönüş tipleriyle **dört public üye** duruyor:

| Satır | Üye | Dönüş tipi | K-5'e göre |
|---|---|---|---|
| `CapabilityRegistry.cs:63` | `CapabilityPack.AllowedTools` | `IReadOnlySet<string>` | **yasak** |
| `:70` | `CapabilityPack.RequiresHumanApprovalFor` | `IReadOnlySet<string>` | **yasak** |
| `:135` | `CapabilityRegistry.Packs` | `IReadOnlyCollection<CapabilityPack>` | **yasak** |
| `:136-137` | `CapabilityRegistry.EnabledPacks` | `IReadOnlyCollection<CapabilityPack>` — üstelik `.ToList()` | **yasak** |

Şimdi K-5'in maliyet tablosuna bakın (`:726`):

> *"Dokunulan üretim dosyası | 5 — `Scheduler.cs`, `ActuationLayer.cs`,
> `Domain/DecisionAggregate.cs`, `Adapter/LlmAdapter.cs`, `Domain/CompanyMemory.cs`"*

**`CapabilityRegistry.cs` listede yok.** ADR, o dosyayı "dersi öğrenmiş örnek" diye anıyor ve
aynı dosyanın **dört ihlalini** görmüyor. Bu, `AUDIT-WAVE2 §10.5` meta-kalıbının —
*"kusur örnek olarak kapatıldı, sınıf olarak kapatılmadı"* — ADR'nin kendi metnindeki
tekrarıdır: `:93`'te bir kez çözülmüş olması, `:63`/`:70`/`:135`/`:136`'nın açık olduğunu
görmeyi engellemiş.

Dahası `:135` ve `:136-137` **yeni P6 üyeleridir ve haritada yok**:
- `Packs => _packs.Values` bir `Dictionary.ValueCollection`'dır — **canlı görünüm**.
  Yineleme sırasında `Register` çağrılırsa `InvalidOperationException`. Bu, `W2_L4`'ün
  (canlılık) birebir aynısıdır.
- `EnabledPacks => ....ToList()` her çağrıda taze bir `List<T>` döner ve
  `IReadOnlyCollection<T>`'ye upcast edilir → `(List<CapabilityPack>)registry.EnabledPacks`
  ile downcast + mutasyon mümkündür. Bu, `W22`'nin (`Scheduler` canlı `List`) birebir aynısıdır.

**Sonuç:** K-5'in mekanizması (dönüş tipi + assembly taraması) bunları **kapatır** — mimari
test tam da bu yüzden doğru tasarımdır. Kapanmayan şey **muhasebedir**: P6'nın 5 üyesi eksik
sayımdır, K-5'in dosya listesi eksiktir, ve *"en temiz kapanış"* iddiası eksik bir envanterin
üstünde duruyor. **Bu, K-5'in lehine bir bulgudur ve ADR'nin sayı disiplininin aleyhine.**

### B-9 — ⛔ K-2 ve K-1'in **mimari testi yok**; K-5/K-6'nınki var. Asimetri gerekçesiz

ADR'nin kendi tasarım ölçütü (`:49-50`):

> *"**Bir karar, ancak 'unutmak' derleme hatası ya da tip hatası üretiyorsa sınıfı kapatır.**
> Çağrı yerlerini elle saymak, sayımın kendisini yanlışlanabilir bir iddia hâline getirir."*

§6 tablosu K-1, K-2, K-4'ü *"Sınıf (derleyici)"* sayıyor. Bu **yarı doğrudur** ve fark
kritiktir:

| | Derleyici neyi zorlar | Neyi zorlamaz |
|---|---|---|
| **K-5/K-6** | Mevcut imzalar | Yeni üye — **ama mimari test tarar** (`:692-695`, `:785-786`) |
| **K-2** | Mevcut imzalar | **Yeni** bir public üyenin ham `string` parametre alması |
| **K-1** | Mevcut imzalar | **Yeni** bir yetki tipinin `public` kurucuyla doğması |
| **K-4** | Mevcut imzalar | **Yeni** bir `double` eşik parametresi |

K-5 ve K-6 için tarama yazılmasının gerekçesi (`:695`) — *"Yeni bir public üye eklendiğinde
kural **otomatik** uygulanır. Bu, `Guard.cs`'in 'listeyi elle taşı' çaresizliğinin çözümüdür"*
— K-2/K-1/K-4 için **birebir** geçerlidir ve uygulanmamıştır.

Somut senaryo (`Guard.cs`'in sekizinci-nokta hikâyesinin K-2 sürümü): bir sonraki geliştirici
`CapabilityRegistry`'ye `public bool HasTool(string toolName)` ekler. K-2'nin altı tipi
değişmemiştir, hiçbir mevcut imza kırılmaz, derleyici susar, ve `W2f`'nin asimetrisi **yeni
bir çağrı yerinde** yeniden doğar. K-5 tarzı bir tarama (*"`Ens.Kernel` assembly'sindeki
public üyelerin `string` parametreleri kimlik rolü taşıyamaz"*) bunu yakalardı.

> **Bu, ADR'nin kendi ölçütüne göre bir kusurdur** — ve ADR §6 tablosunda **görünmüyor**.
> §6 yalnız K-3'ü "kısmen" işaretliyor; K-1/K-2/K-4 koşulsuz "Sınıf" yazılmış. Doğru tablo
> üç satırı daha *"sınıf — ama yalnız mevcut yüzey için; regresyon koruması yok"* yapmalıydı.

### B-10 — K-2 kardeş çağrı yerleri: `Disable`/`Enable`/`IsEnabled` gerçekten kapanıyor mu?

Görev metninin sorduğu sınav bu. Doğruladım — `CapabilityRegistry.cs`:

```csharp
:131  public void Disable(string packName)  => _disabled.Add(packName);      // hiçbir doğrulama
:132  public void Enable(string packName)   => _disabled.Remove(packName);
:134  public bool IsEnabled(string packName) => _packs.ContainsKey(packName) && !_disabled.Contains(packName);
:144  public ToolAuthorization Authorize(string toolName)
```

Dört yol, dördü de ham `string`. K-2 **dördünü birden** `PackName`/`ToolName` yapıyor
(`:360-361` altı rolü sayıyor, `PackName` dahil) — yani bu eksende K-2 **kardeş yolları
atlamıyor**. Bu, ADR'nin lehine ve görev metninin şüphesi bu noktada karşılanıyor.

Ama **`Register` tarafında bir asimetri kalıyor:** `Register` bir `CapabilityPack` alır
(`:119`); pack'in içindeki araç adları `CapabilityPack` **kurucusunda** (`:75-103`) toplanır.
K-2'nin kapanış tablosu `W2f` için *"`Register` ve `Authorize` **aynı `ToolName` tipini**
alır"* diyor (`:439`) — ama `Register`'ın parametresi `ToolName` **değil**, `CapabilityPack`'tir.
Kapanış, `CapabilityPack` kurucusunun `IEnumerable<ToolName>` almasına bağlıdır ve ADR bunu
**hiçbir yerde yazmıyor**. `CapabilityPack` `CapabilityRegistry.cs:54`'te yaşıyor, o dosya
K-2'nin listesinde var (`:449`) — yani iş kapsamda, ama **karar cümlesi eksik**. Küçük ama
düzeltilmeli: `W2f`'nin kapanışı bugün metinde *ispatlanmıyor*, *varsayılıyor*.

## 4. P5/P9 sızması — **sızma YOK** (ve bu, ADR'nin en iyi tarafı)

Bu, görev metninin bloke edici eksenidir. Aradım; **bulamadım.** Ayrıntılı kontrol:

| Kontrol | Sonuç |
|---|---|
| P5 (`E5`, `W3c`) kapanan hanesinde mi? | **Hayır** — `:206` kapsam dışı tablosunda, `:878-881` açık borçta, `:349` R3'te *"K-1 reflection'a karşı koruma **iddia etmez**"* |
| P8 (10 üye) kapanan hanesinde mi? | **Hayır** — `:207`, `:860-876`. Dahası `:874-876` ADR'nin P8'i **kendi içinde yeniden ürettiğini** (R13, `Reason` serbest metin) itiraf ediyor |
| P9 (15 üye) kapanan hanesinde mi? | **Hayır** — `:208`, `:878-881` |
| K-1..K-6 kapanış tablolarında P5/P8/P9 üyesi var mı? | **Hayır.** 44 satırın tamamını harita üyelikleriyle karşılaştırdım; her satır kendi kalıbının üyesi |
| `W5e` K-6'da — meşru mu? | **Evet** — `DEFECT-PATTERN-MAP.md:148` `W5e`'yi P7'ye koyuyor |

`DEFECT-REGISTER` v1'in cezalandırıldığı hata (reflection'ı "kapanan" hanesine yazmak)
**tekrarlanmamıştır.** `:211-213` bunu açıkça beyan ediyor ve beyan **doğrudur**:

> *"**v1'in hatası tekrarlanmıyor.** Önceki sicil P5'i 'kapanan' hanesine yazmış ve
> yakalanmıştı."*

### B-11 — Ters yönde bir muhasebe sorunu var: ADR **eksik** sayıyor

Sızma yok; ama sınır iki yönde de gevşek. K-2'nin karakter sınıfı kapısı (`:382`, `Cc`+`Cf`
reddi) haritanın **P9'una** koyulmuş iki kimliği de kapatıyor gibi görünüyor:

- `W2d` (`AdversarialWave_SecurityTests.cs:259-272`) — pack adı `"Repor‮troper"`,
  RTL override doğrudan `Authorize(...).Reason`'a akıyor. K-2 adım 1 `Cf`'yi **reddeder** →
  bu ad hiç kaydedilemez. (Tam kapanmaz: `Reason`'a akan başka metin kaynakları kalır.)
- `W1e` (`:142-152`) — `Disable` doğrulanmamış girdiyle sınırsız büyüyor. M-4 (`:418-421`)
  kayıtlı olmayan adda **hata verir** → küme büyüyemez.

Bu iki kimlik ADR'nin sayısında **yok**. Yani ADR bir yandan `W1b`'yi çifte çıkarıp sayıyı
düşürüyor (B-2), öte yandan kapattığı iki kimliği hiç saymıyor. **Envanter her iki yönde de
kalibre değil** — ki bu, sicilin 8. kalıbının ADR'nin muhasebesindeki hâlidir.

> **Bunu bir kusur olarak değil, T-A'nın kapsamı olarak kaydediyorum:** sayı yeniden
> hesaplanacaksa (B-5), P9'a "yan etkiyle kapananlar" sütunu da eklenmelidir. Eksik sayım
> dürüsttür ama **yanlışlanabilir değildir**: uygulama sonrası `W2d` yeşile dönerse ADR
> "fazladan başarı" mı elde etmiştir, yoksa kalıp modeli mi hatalıydı? Ayırt edilemiyor.

## 5. Sayılmayan risk yüzeyi

ADR R1–R20 sayıyor. Üç tanesini **saymıyor** ve üçü de sayılanların çoğundan ağır.

### B-12 — ⛔ UR-1: `record` + `with` ifadesi K-1'in mührünü **kopyalar** — `W15` yeniden açılır

K-1'in kendi mekanizma taslağı (`ADR-0003:261-267`):

```csharp
public sealed record ToolAuthorization
{
    private readonly AuthoritySeal _issuer;
    private ToolAuthorization(...) { }                 // PUBLIC KURUCU YOK
    internal static ToolAuthorization Issue(AuthoritySeal seal, ...) => new(...);
    internal bool IssuedBy(AuthoritySeal seal) => ReferenceEquals(_issuer, seal);
}
```

Tip `record` olarak bırakılmış. C#'ta **`record` bir `with` ifadesi üretir** ve `with`,
`private` kurucuyu **kullanmaz** — derleyicinin sentezlediği kopya kurucu (`<Clone>$`)
üzerinden çalışır; o kopya, `_issuer` dâhil **tüm instance alanlarını** taşır. Bugünkü tip
zaten pozisyonel `record`tur ve üyeleri `init`tir (`CapabilityRegistry.cs:107`):

```csharp
public sealed record ToolAuthorization(bool IsAllowed, bool RequiresHumanApproval, string Reason);
```

Sonuç, K-1 uygulandıktan **sonra** bile:

```csharp
var legit  = /* registry'nin verdiği meşru, mühürlü ret */;      // IsAllowed: false
var forged = legit with { IsAllowed = true, RequiresHumanApproval = false };
// forged._issuer  ==  legit._issuer   →  IssuedBy(seal) TRUE döner
```

**Mühür kopyalandı, yetki taklit edildi, `private` kurucuya hiç dokunulmadı.** Bu tam olarak
`W15`'tir (`AdversarialWave_SchedulerGateTests.cs:386` — *"`ToolAuthorization` is a public
record so a registry denial can be laundered"*), yani K-1'in kapattığını iddia ettiği
(`:299`) kusurun kendisi. `W4a` (sahte yetki) ve `E3` (gate sonucu taklidi) aynı yoldan
geri gelir.

Bu, R3'ten (reflection) **kategorik olarak farklıdır** ve kapsam dışı sayılamaz: reflection
"tam güven" gerektirir ve ADR onu açıkça dışarıda bırakır; `with` ise **sıradan, derlenen,
tek satırlık C#**'tır. K-1'in tasarım ölçütü (`:49`) *"unutmak derleme hatası üretmeli"*
diyordu; burada **taklit** derleme hatası üretmiyor.

Aynı delik K-4'ün `DecayPolicy.Disabled`'ında da var (`:595`, `public sealed record`) — ki
R12 azaltması *"`Disabled` üretimi K-1 mührü ister"* diyor, `with` o şartı da geçer.

> **Düzeltme (ucuz):** yetki tipleri `record` **değil**, `sealed class` olmalı — ya da
> `record` kalacaksa kopya kurucu `private` yapılmalı (`private ToolAuthorization(ToolAuthorization other)`),
> ki bu `with`'i **derleme hatası** yapar. ADR bu tercihi hiç tartışmıyor; OQ1 (`struct` mü
> `class` mı) yalnız `Measured`/`DecayRate`/`ImmutableArray` için soruluyor, **yetki tipleri
> için sorulmuyor**.

### B-13 — ⛔ UR-2: K-4, kendi kod taslağında **K-3'ü ihlal ediyor**

`ADR-0003:595`:

```csharp
public sealed record Disabled(string Reason, Identity Approver, DateTimeOffset At) : DecayPolicy;
```

`At` bir **çağıran-tarafından-verilen zaman damgasıdır**. K-3'ün karar cümlesi (`:485-487`):

> *"Kernel'de 'şimdi'yi yalnızca enjekte edilmiş `TimeProvider` üretir ve çağırandan gelen
> **her** zaman damgası bir **kabul aralığından** geçmeden hiçbir kayda, event'e ya da hesaba
> giremez."*

Ve M-1 (`:501`): *"*çağırandan zaman alan* imzalar zaman parametresini **kaybeder**."*

`DecayPolicy.Disabled.At` bir denetim damgasıdır ve `PolicyDisabled` event'ine yazılacaktır
(`:600-601`). Yani K-4, `W2_L3`'ün (*"Denetim zaman damgaları çağıran kontrolünde"*,
`DEFECT-PATTERN-MAP.md:88`) birebir kalıbını **yeni bir tipte yeniden üretiyor** — üstelik
K-3'ün o kusuru kapattığını iddia ettiği aynı belgede (`:514`).

ADR R9'da K-3↔K-4 bağını **bir yönde** görüyor (`skew` P4'e tabidir). **Ters yön görülmemiş:**
K-4'ün ürettiği yeni tipler P3'e tabidir. `At` ya kaldırılmalı (kernel `TimeProvider`'dan
doldurur) ya da `TimeWindow.Accept`'ten geçmelidir.

### B-14 — R1 (replay) bir risk olarak *kaydedilmiş* ama **çözülmemiş**; ADR bunu karar diye sunuyor

Görev metninin sorusu: mühür serileşemiyorsa ve kernel event-sourced ise, replay yolunda
yetki nasıl çözülüyor? **ADR çözmüyor.** `:340-344` (R1) sorunu doğru ve dürüst tarif ediyor,
`:925-927` (failure condition 6) *"K-1 net negatif olur"* diyor, `OQ3` (`:938`) soruyu açık
bırakıyor. Üç yerde kabul, sıfır yerde çözüm.

Somut boşluk: `Rehydrate` (`W2_R2`, K-1'in kapattığı iddia edilen üye, `:305`) **replay
yoludur**. K-1'in `W2_R2` kapanışı mühre değil `e.Target == id` doğrulamasına dayanıyor —
ADR bunu parantez içinde itiraf ediyor (*"mühür değil, ama aynı kararın parçası"*). Yani
K-1'in 11 üyesinden **biri K-1'in mekanizmasıyla değil, yanına iliştirilmiş ayrı bir
kontrolle** kapanıyor. Bu, kararın kendi ölçütüne (`:40` — *"tek tek kusuru değil, kusur
SINIFINI"*) göre bir sınıf kapanışı değil, bir **örnek** kapanışıdır.

Daha keskin soru — ADR'nin sormadığı: replay sırasında bir `ToolAuthorization` yeniden
üretilecekse, onu üreten `CapabilityRegistry` **o anki** registry'dir, event'in yazıldığı
andaki değil. Pack'ler arada `Disable` edilmişse replay **farklı bir yetki** üretir ve
geçmiş yeniden yazılır; edilmemişse replay geçmişi doğrular. Hangi semantiğin doğru olduğu
(as-of yetki mi, güncel yetki mi) bir **mimari karardır** ve bu ADR onu ne soruyor ne
cevaplıyor. Bu, `W2_R3`/`W2_R5` "replay ≠ canlı yol asimetrisi" ailesinin (P9) doğrudan
büyümesidir — ADR'nin `:926-927`'de korktuğu şeyin **mekanizması** budur.

## 6. Açık sorular: dürüstlük mü, eksik karar mı?

Görev metninin sorusu keskin: *bir ADR, çözmediği şeyi karar diye sunabilir mi?* Cevabım
ayrımlı: **OQ2–OQ5 dürüst açıklıktır; OQ1 ve OQ6 eksik karardır.**

### B-15 — OQ1 karar değil, **kararların ön koşuludur** — ve kapsamı da dar

ADR kendi failure condition 4'ünü yazıyor (`:921-923`):

> *"Üç kararın ortak `default(struct)` deliği (R14/R15/R20) kapatılmazsa — **K-4, K-5, K-6'nın
> üçü birden tip zorlamasını kaybeder.** `struct` mi `class` mı sorusu bu ADR'de **açıktır**
> ve yanıtsız bırakılmıştır."*

Bu, altı kararın **üçünün** bugün *"unutmak derleme hatası üretir"* ölçütünü (`:49`)
karşılamadığının itirafıdır. Bir ADR, mekanizması ancak açık bir soru cevaplandığında
çalışan üç kararı "karar" başlığı altında sunuyorsa, o üçü **karar değil, koşullu öneridir**.
Fark pratikte şudur: Madde VII gereği `7000-reference-implementation/` yalnız Accepted ADR'lere
dayanabilir; K-4/K-5/K-6 Accepted olursa, uygulayıcı OQ1'i **kendi başına** cevaplar ve
mimari karar kod katmanında verilmiş olur — P8'in (*"teori asla koddan geri türetilmez"*,
`ENS-0000:70`) ihlali.

Ayrıca **OQ1'in kapsamı dar**: yalnız `Measured`/`DecayRate`/`ImmutableArray` soruluyor.
B-12'de gösterdiğim gibi asıl `record` vs `class` sorusu **yetki tiplerindedir** (K-1) ve
orada `record` seçimi mührü delen bir `with` yolu açıyor. OQ1 bu tipleri hiç anmıyor.

### B-16 — OQ2 gerçek bir çelişki, ama ADR onu **olduğundan geniş** anlatıyor

`ADR-0003:839-843` (R19) ve `:928-930` (fc 7): *"`Guard.cs:130-132` kırpma lehine yazılmış
gerekçe ile **doğrudan çelişir**."* Atfı doğruladım — `Guard.cs:131-132`:

> *"Tüm partiyi bir tek bozuk peer-sinyali yüzünden exception'la düşürmek, dikkat tahsisini
> komple durdurur (**servis-dışı bırakma vektörü**); kırpma fail-closed kalır."*

Alıntı doğru. Ama çelişki **`Measured` ile değil**: `Guard.NormalizedDeficit` zaten
`Finite(value, ...)` çağırıp NaN/±∞'yi **reddediyor** (`Guard.cs:137`, ve `:133` — *"NaN ise
KIRPILMAZ — reddedilir"*). K-6'nın `Measured.Of` da yalnız sonluluk arıyor (`:768`). İkisi
**aynı** politikadadır. Çelişen tek şey, ADR'nin bir cümleyle geçtiği `UnitMeasured`
(`[0,1]`, `:777-778`): tanım gereği normalize büyüklüklerde `Guard` **kırpar**, `UnitMeasured`
**reddeder**.

Yani OQ2 gerçek ama **dar**: "hangi çıktı reddedilir, hangisi kırpılır" sorusu yalnızca
birim-aralık tiplerini bağlar. ADR bunu R19'da genel bir çelişki gibi sunarak kendi
kararının şiddetini **abartıyor** — bu, nadir görülen bir yön: aleyhte abartı. Yine de
düzeltilmeli, çünkü yanlış tarif edilmiş bir açık soru yanlış bir işle kapatılır.

### B-17 — ⛔ OQ6 bir "soru" değil, **eksik bir yapıttır** — ve ADR uygulama sırasının kusur ürettiğini biliyor

`:941`: *"Bu altı karar hangi sırayla uygulanır? ... Sıra bir uygulama planı gerektirir —
**bu ADR o planı içermez.**"*

Bu, bir belirsizlik beyanı değil; bir **eksik teslimat** beyanıdır. Üstelik ADR, sıranın
yanlış olmasının **yeni kusur ürettiğini** kendisi yazıyor (`:554-557`, R9):

> *"K-3 ile K-4 arasındaki bu bağ **zorunludur**; ayrı uygulanırlarsa **K-3 kendi P4 kusurunu
> doğurur**."*

Yani: (a) sıra bağımlılığı var, (b) yanlış sıra yeni kusur üretir, (c) sıra yazılmamış.
Bir ADR'nin işlevi kararı **kapatmaktır**; burada ADR, kendi uygulanışının kusur üretebilen
bir serbestlik derecesini uygulayıcıya bırakıyor. `plan-first.md`'ye göre bu iş
`IsIrreversible`'dır (kernel davranış değişikliği + 7 dosya + breaking imza) ve **Tier 3**'tür
— yani sıra sorusu bir *plan* yapıtıdır, bir *açık soru satırı* değil.

Bilinen bağımlılıklar (ADR'nin kendi metninden çıkarılabilir, ama derlenmemiş):

| Bağ | Kaynak | Yön |
|---|---|---|
| K-3 → K-4 | R9 (`:554-557`) | K-4 **önce** (yoksa `skew` sentineli) |
| K-1 ↔ K-2 | OQ6 (`:941`) | `Authorize(ToolName)` imzası ikisini birden bağlar |
| K-4/K-5/K-6 → OQ1 | fc 4 (`:921-923`) | OQ1 **hepsinden önce** |
| K-2 → B-7 (fixture kodlaması) | bu kayıt | doğrulama aleti **K-2'den önce** onarılmalı |

### B-18 — OQ3/OQ4/OQ5: **dürüst açıklık**, itirazım yok

- **OQ3** (mühür dağıtık kernel'e nasıl taşınır) — R1'in uzun vadeli hâli; bugünün in-process
  kernel'i için karar vermek erken olurdu. `:330-331` reddedilen alternatif 1 zaten *"dağıtık
  kernel'e geçildiğinde yeniden değerlendirilir"* diyor. Doğru posture.
- **OQ4** (`confusables.txt` gömülecek mi) — `:404-406` dürüst sınırı zaten yazıyor:
  *"`confusables.txt` .NET BCL'de **yoktur**"* ve mixed-script'in *"aynı-script varyantlarını
  (`rn` ↔ `m`) kapatmadığı"* açıkça kabul ediliyor. Bu, ADR'nin en dürüst paragrafıdır.
- **OQ5** (Unicode sürüm kayması) — R7 ile birlikte gerçek ve iyi tarif edilmiş.

## 7. Prior art dürüstlüğü

Bu, `ens-skeptic`'in en sık kusur bulduğu eksendir (uydurma atıf). **Burada uydurma atıf
bulamadım.** Otuz kadar atfın hepsi gerçek eserlere işaret ediyor ve çoğu iddia ettiğini
söylüyor. İki düzeltme ve bir uyarı var.

| Atıf | Durum |
|---|---|
| Morris, J.H. (1973), *Protection in Programming Languages*, CACM 16(1) — sealer/unsealer | **Gerçek.** Makale ve sealer/unsealer kavramı doğru atfedilmiş |
| Miller, M.S. (2006), *Robust Composition* (PhD) — object-capability, brand | **Gerçek.** "Unforgeable reference" ve marka doğrulama bu tezin merkezi |
| **JEP 411 / JEP 486** | **Gerçek, ama ifade YANLIŞ** — aşağıda B-19 |
| UAX #31 — identifier syntax, NFC | **Gerçek** ve NFC önerisi doğru |
| UTS #39 — confusables, mixed-script, restriction levels | **Gerçek**; "Moderately Restrictive" gerçek bir seviye adı |
| RFC 8264/8265 — PRECIS | **Gerçek**; "her string sınıfı kendi profilini taşır" doğru özet |
| Hoare (2009) "billion-dollar mistake" | **Gerçek** (QCon London 2009) |
| Alexis King (2019) "Parse, don't validate" | **Gerçek** |
| Minsky — "make illegal states unrepresentable" | **Gerçek** |
| Meyer (1988), *OOSC* — Design by Contract, postcondition | **Gerçek** |
| Bloch, *Effective Java*, **Item 50** "Make defensive copies when needed" | **Gerçek** — Item 50 **3. baskı** numarasıdır (2. baskıda Item 39). Baskı belirtilmeli |
| Lamport (1978) — kısmi sıralama | **Gerçek** |
| Kerberos RFC 4120, ~5 dk skew | **Gerçek.** Ama ADR'nin kendi metni bozuk: `:524` *"RFC 4120 **§,** varsayılan..."* — bölüm numarası **yazılmamış**, `§` işareti boşta duruyor |
| JWT RFC 7519 `nbf`/`exp`/`iat` | **Gerçek.** "leeway" RFC'de normatif değil, yaygın uygulama konvansiyonudur — ADR bunu ayırt etmiyor |
| Spanner TrueTime `[earliest, latest]` | **Gerçek** |
| `System.TimeProvider` (.NET 8) | **Gerçek** ve repoda `FixedTimeProvider.cs` var — `:522-523` atfı **doğrulandı** (dosya mevcut) |
| `System.Collections.Immutable` "(Microsoft, **2015**)" | Yıl **yaklaşık**; kütüphane 2013-2014'te yayımlandı. Önemsiz ama tarih verilmişse doğru olmalı |
| `System.Collections.Frozen` (.NET 8) | **Gerçek** |
| Clojure persistent data structures (Hickey, 2007) | **Gerçek** |
| ArchUnit / NetArchTest | **Gerçek** araçlar; "mimari kuralı test olarak zorlama" deseni doğru atfedilmiş |
| Rust `NonZeroU32`/`NonZeroUsize`, `ordered-float`, `newtype` | **Gerçek** |
| IEEE-754 §6.3 — `-0.0`/`+0.0` | **Gerçek** (§6.3 "The sign bit") |
| Evans (2003), *DDD* — "Entities have identity; Value Objects do not" | **Gerçek**; §2.2'deki kullanımı yerinde |

### B-19 — JEP 486 ifadesi yanlış: Security Manager **kaldırılmadı**, *kalıcı olarak devre dışı bırakıldı*

`ADR-0003:286-289`:

> *"...nihayet **JEP 411 ile deprecate, JEP 486 (JDK 24) ile kaldırıldı.**"*

JEP 486'nın başlığı **"Permanently Disable the Security Manager"**dır. JDK 24'te API
**durmaktadır**; başlatmada etkinleştirilemez ve çalışma zamanında kurulamaz.
`System::getSecurityManager` ve `AccessController::doPrivileged` **hâlâ vardır** ve JDK 17'deki
"etkin değil" davranışını sergiler. API'nin **kaldırılması gelecek bir sürüme bırakılmıştır**.

Fark önemsiz değil, çünkü ADR bu atıftan bir **ders** çıkarıyor (`:289` — *"yetki ortam
(ambient) değil, nesne olmalıdır"*). Ders doğrudur; ama "kaldırıldı" demek, kanıtı olduğundan
güçlü göstermektir — `evidence-standard` diliyle, **seviyesini aşan kesinlik**. Düzeltme tek
kelimelik: *"JEP 486 (JDK 24) ile kalıcı olarak devre dışı bırakıldı; API'nin kaldırılması
sonraki bir sürüme bırakıldı."*

### B-20 — DOĞRULANMADI

- **UTS #39 §4**'ün confusable detection'ı tam olarak §4'te tanımlayıp tanımlamadığını
  (bölüm numarası) bu turda kaynak metinden doğrulamadım. UTS #39'un confusable/skeleton
  tanımı içerdiği kesin; **bölüm numarası doğrulanmadı**.
- **RFC 4120**'nin varsayılan skew'inin tam olarak 5 dakika olduğu yaygın bilgidir; RFC
  metnindeki bölümü bu turda açmadım. ADR zaten bölüm numarasını **boş bırakmış** (B-19
  tablosu) — yani ADR de doğrulamamış.
- `AdversarialWave_SecurityTests.cs:927` (P7 adının kaynağı) ve `:958` (W8d) atıflarını
  **doğrulayamadım**: dosya `grep` ile taranamıyor (B-6) ve o satır aralığını bu turda
  okumadım. **"Yanlış" demiyorum — okumadım.**

## 8. Türkçe casing kararı — ⛔ doğru teşhis, **yanlış sonuç**

ADR'nin gerekçesi (`:408-416`) iki iddiayı birleştiriyor. Birincisi doğru, ikincisi **yanlış**
ve ikisi arasındaki fark bir Türkçe ENS kurulumunda doğrudan kusura dönüşüyor.

**Doğru olan:** kültür-bağımlı casing yasaklanmalı. *"İki ENS düğümü farklı kültür ayarıyla
koşarsa aynı ad iki farklı kimliğe düşer"* — bu, P2'nin dağıtık ve teşhis edilemez hâlidir ve
`ToUpperInvariant` bunu **çözer**. `.NET` yönergesi atfı da doğrudur.

**Yanlış olan:** ADR bunu `G4`'ün (*"Büyük/küçük harf varyantları iki ayrı öneri üretiyor"*)
kapanışı olarak sayıyor (`:434`). `ToUpperInvariant` Türkçe metinde **hem yanlış birleştirir
hem yanlış ayırır**.

### B-21 — `ToUpperInvariant` Türkçeyi çözmez; yalnız *deterministik olarak* yanlış yapar

Unicode invariant büyük-harf eşlemesi:

| Girdi | `ToUpperInvariant` | Sonuç |
|---|---|---|
| `ı` (U+0131, dotless i) | **`I` (U+0049)** | `ı` ve `i` **AYNI** kimliğe düşer |
| `i` (U+0069) | `I` (U+0049) | ↑ |
| `İ` (U+0130, dotted capital I) | `İ` (U+0130 — değişmez) | `İ` ve `i` **AYRI** kimliklerde kalır |

Doğrulandı: U+0131'in Unicode uppercase mapping'i U+0049'dur; Türkçeye özgü eşleme
(`ı`↔`I`, `i`↔`İ`) yalnız `tr-TR` kültüründe geçerlidir.

Somut sonuçlar, ENS'in **kendi dili** Türkçeyken (dokümanlar Türkçe, `purpose type` ve
`OwnerName` Türkçe olacak):

1. **Yanlış birleştirme (R5'in gerçekleşmiş hâli).** `PurposeType.Parse("ısı")` ve
   `Parse("isi")` → ikisi de `"ISI"`. **İki farklı Türkçe kelime tek kimliğe düşer.**
   Bu, ADR'nin R5'te *"gerçekten farklı iki varlık aynı kimliğe düşebilir"* diye kaydettiği
   riskin **kuramsal değil, Türkçe için sistematik** bir örneğidir — ve R5'in azaltması
   (*"`Register` kanonik çakışmada hata verir"*) burada **kusuru hataya çevirir, çözmez**:
   meşru bir "isi" purpose type'ı, "ısı" kayıtlıysa **reddedilir**.
2. **Yanlış ayırma — `G4` kapanmıyor.** `OwnerName.Parse("İş")` → `"İŞ"`;
   `Parse("iş")` → `"IŞ"`. **Aynı Türkçe kelimenin büyük ve küçük hâli iki ayrı kimlik.**
   `G4`'ün tanımı tam olarak budur (`DEFECT-PATTERN-MAP.md:64`). Yani `G4`, ASCII adlarda
   kapanır, **Türkçe adlarda kapanmaz** — ve `W7f` (*"Sahip kimliğinde harf farkı tüm
   attribution'ı ters çeviriyor"*, `:441`) için de aynısı geçerlidir; `OwnerName` alanı
   insan adı taşır ve Türkçe insan adları `İ`/`ı` içerir (*İsmail, Işık, İnci*).

### B-22 — ADR'nin listelemediği diğer `ToUpperInvariant` tuzakları

- **Almanca `ß` (U+00DF):** `"ß".ToUpperInvariant()` **`"ß"` döndürür** (tek karakterlik
  `ToUpper` `"SS"` katlaması yapmaz). `"straße"` → `"STRAßE"`, `"strasse"` → `"STRASSE"` —
  **ayrı kimlikler**. Fail-closed yön (bölme), ama `G4` sınıfı yine açık.
- **Yunanca final sigma (`ς` U+03C2):** burada `ToUpperInvariant` **doğru** çalışır
  (`ς` ve `σ` → `Σ`) — ki bu, ADR'nin `ToLowerInvariant` yerine `ToUpperInvariant` seçmesinin
  gerçek teknik gerekçesidir. ADR bu gerekçeyi *"küçük-harfe katlama bazı karakterlerde geri
  dönüşsüzdür"* diye doğru ama **muğlak** ifade ediyor; somut örnek verilse iddia güçlenirdi.
- **Türkçe casing yasağı `Guard`lanmıyor.** `:409-410` *"`CultureInfo.CurrentCulture` bağımlı
  hiçbir çağrı ... kernel'de **yasaktır**"* diyor. Bu, K-3'ün `DateTimeOffset.UtcNow` yasağıyla
  **aynı türden bir konvansiyondur** — ve ADR §6 tablosu K-3'ü tam bu yüzden *"Kısmen"*
  işaretlemiş (`:896`), ama K-2'yi *"Sınıf (derleyici)"* saymış. **Tutarsız:** aynı belge,
  aynı tür yasağı bir yerde zayıflık, başka yerde sınıf-kapanışı sayıyor. `BannedApiAnalyzers`
  önerisi (`:903`) `string.ToLower()`/`ToUpper()`/kültürlü `Compare` için de geçerlidir ve
  yazılmamıştır.

### Doğru çözüm ne olurdu

`ToUpperInvariant` bir **case folding** aracı değildir. Unicode'un bu iş için tanımladığı
işlem **case folding**'dir (`CaseFolding.txt`); .NET'te doğrudan karşılığı yoktur, ama
`StringComparer.OrdinalIgnoreCase` invariant simple case folding'e yakın davranır ve
`ı`/`i` sorununu yaşamaz (ordinal karşılaştırma, `ı` ile `i`'yi **eşitlemez**). Yani:

> **Öneri:** harf katlama adımı ya (a) **kaldırılmalı** — adlar case-sensitive olur, `G4`
> bir *ret* ile kapanır (aynı skeleton'a düşen ikinci ad `Register`'da hata verir, M-3'ün
> aynısı); ya da (b) **açık bir folding tablosuna** dayanmalı ve Türkçe `İ`/`ı` çifti o
> tabloda **özel olarak** ele alınmalıdır. Bugünkü hâliyle karar, Türkçe bir sistemde
> Türkçe adları yanlış eşleştirmektedir — ve bu, ADR'nin kapattığını iddia ettiği kalıbın
> ta kendisidir.

Bu, bu incelemedeki **en somut teknik kusurdur**: kapanış tablosunda `G4`, `W7f`, `W1a`,
`W1c` satırları (`:434-436`, `:441`) `ToUpperInvariant`'a dayanıyor ve dördü de Türkçe
girdide iddia edileni yapmıyor.

## 9. Maliyet iddiası

ADR `:189-191`'de dürüst bir sınır koyuyor: *"'mevcut 373 test' sayısı bu oturumda
`dotnet test` çalıştırılarak doğrulanmadı ... tahminler **etkilenen test dosyası ve metot
sayımına** dayanır."* Bu doğru posture. **Ama sayımın kendisi bozuk.**

### B-23 — ⛔ `AdversarialWave_SecurityTests.cs` **altı maliyet tablosunun HİÇBİRİNDE yok** (kodlama tuzağı, dördüncü kez)

Depodaki test dosyalarını listeledim (19 test dosyası + `FixedTimeProvider.cs`). Sonra altı
kararın "Etkilenen test" listelerini o dosyayla karşılaştırdım:

| Karar | ADR'nin listesi (`satır`) | `AdversarialWave_SecurityTests.cs` var mı |
|---|---|---|
| K-1 | 7 dosya (`:323`) | **YOK** — ama `W5d` orada |
| K-2 | 8 dosya (`:451`) | **YOK** — ama `W1a` `W1c` `W2c` `W2e` `W2f` `W5g` `W7f` `W7h` orada |
| K-3 | 5 dosya (`:537`) | **YOK** |
| K-4 | 5 dosya (`:640`) | **YOK** |
| K-5 | 3-4 dosya (`:728`) | **YOK** — ama `W5a` `W5b` orada |
| K-6 | 7 dosya (`:820`) | **YOK** — ama `W5e` orada |

`DEFECT-PATTERN-MAP.md:281` şunu söylüyor: *"Onüçünün de testi var; **hepsi
`AdversarialWave_SecurityTests.cs` içinde**"* — `W1a` `W1b` `W1c` `W2c` `W2e` `W2f` `W5a`
`W5b` `W5d` `W5e` `W5g` `W7f` `W7h`. Yani ADR'nin kapattığını iddia ettiği **44 kimliğin
13'ü** (yaklaşık üçte biri) o dosyada yaşıyor, ve dosya **hiçbir maliyet tablosunda geçmiyor**.

**Bu, dört NUL baytının dördüncü kurbanıdır.** Sicilin sayımını (68 vs 75), güvenlik
raporunu, ve K-0'ın gerekçesini yanılttıktan sonra şimdi de ADR'nin maliyet tahminlerini
yanıltmış. `ADR-0003:145-154` kalıbı doğru teşhis edip `work-protocol.md` §3.2'yi yazdı —
ve **aynı belgede, altı tabloda üst üste, kurala uymadı**. Kural yazıldı, uygulanmadı.

Somut etki: K-2 kendini *"bu ADR'nin **en geniş yüzeyli** kararı"* ilan ediyor (`:451`) ve
8 test dosyası sayıyor. Doğrusu **en az 9**'dur ve eksik olan dosya, K-2'nin üyelerinin
**%73'ünü** (11'de 8) barındıran dosyadır. Aynı dosya, K-2'nin fixture'larının kodlamaya
bağımlı olduğu dosyadır (B-7). Yani K-2'nin hem maliyeti hem doğrulanabilirliği aynı
görünmez dosyaya bağlı.

### B-24 — K-5'in üretim dosyası listesi de eksik (B-8'in maliyet yüzü)

`:726` beş üretim dosyası sayıyor; `Capability/CapabilityRegistry.cs` **yok**, oysa K-5'in
yasakladığı dönüş tipinden orada **dört** public üye var (`:63`, `:70`, `:135`, `:136`).
`CapabilityRegistryTests.cs` de K-5'in etkilenen test listesinde yok. K-5'in *"test etkisi
**en düşüğüdür**"* iddiası (`:728`) bu iki eksikle birlikte yeniden hesaplanmalıdır.

### B-25 — Makul bulduğum tahminler

Aşağıdakiler eksiklikleri düzeltildiğinde **makuldür**; itirazım yok:

- **K-1 "en breaking" / K-2 "en geniş" ayrımı** (`:322`, `:451`) doğru bir karakterizasyondur:
  K-1 imza *semantiğini* değiştirir (yetki artık parametre değil), K-2 imza *tipini* değiştirir
  (mekanik). "Derin vs geniş" ayrımı gerçek bir mühendislik ayrımıdır.
- **K-5 "çağıranların çoğu derlenmeye devam eder"** (`:727`) — `ImmutableArray<T>` gerçekten
  `IReadOnlyList<T>` uygular; iddia teknik olarak doğru.
- **K-6 "Breaking? Düşük"** (`:819`) — `implicit operator double` bunu sağlar; doğru.
  Ama R18 bu kolaylığın bedelini dürüstçe kaydediyor.
- **`FixedTimeProvider.cs` mevcut** (`:538` hafifletici) — **doğrulandı**, dosya var.
- **"373 test" DOĞRULANMADI beyanı** — `work-protocol.md` §4'e uygun. Ben de çalıştıramadım
  (`Bash` yok); **bu incelemede de doğrulanmamıştır** ve uydurmuyorum.

## İç tutarlılık

### B-26 — ⛔ `P1..P9` **iki ayrı anlamda** kullanılıyor; künye, gövdenin sözlüğünde okununca §3'ün TERSİNİ söylüyor

ADR künyesi (`:10`):

```yaml
principles: [P1, P5, P6, P7, P8]
```

Anayasa §III'te bunlar **ilkelerdir** (`ENS-0000:68-70`):
`P6` = *"Explainability pazarlık konusu değildir"*, `P7` = *"Sorumluluk insandadır"*,
`P8` = *"Teori implementasyondan önce gelir"*. `ADR-0001:10` de aynı anlamda
`principles: [P1, P5, P6, P7]` yazıyor.

Ama ADR-0003'ün **gövdesinin tamamı** `P1..P9`'u **kusur kalıbı** olarak kullanıyor:
`P5` = reflection (`:206`), `P6` = canlı koleksiyon (`:672`), `P7` = çıktı kapısı yok (`:755`),
`P8` = öz-beyan kalibre değil (`:207`). İki sözlük, tek belge, hiçbir ayırt edici işaret yok.

Sonuç, sadece kozmetik değil — **künye gövdenin diliyle okunduğunda §3'ün tam tersini
söylüyor:**

| Okuma | `principles: [P1, P5, P6, P7, P8]` ne demek |
|---|---|
| Anayasa sözlüğü (doğru) | Bu ADR karar/attention/explainability/sorumluluk/teori-önce ilkelerine dayanır |
| Gövdenin sözlüğü (yanlış ama doğal) | Bu ADR **P5 ve P8'i ele alır** |

Oysa `:206-207` ve `:878-881` **tam tersini** ilan ediyor: *"P5 ... **bu ADR bunları
kapattığını İDDİA ETMEZ**"*, *"P8 ... **AÇIK BORÇ, çözülmedi**"*. Bir belge, künyesinde
kapsam dışı ilan ettiği iki kalıbın numarasını taşıyorsa, künye **yanıltıcıdır** — ve bu ADR'nin
merkezî erdemi tam olarak "neyi kapatmadığını dürüstçe saymak" olduğu için, yanılgının
düştüğü yer en kötü yerdir.

Aynı çarpışma `:977`'de de var: *"P6 (proof-trace), P7 (bounded autonomy)"* — burada
Anayasa anlamı; iki satır yukarıda `:975` *"8. kalıp (öz-beyan)"* — burada kalıp anlamı.
Ve kod substratında da: `AdversarialWave_SecurityTests.cs:239` *"O sinir altinda bu bir **P7**
atlatmasidir"* → Anayasa P7.

> **Düzeltme (ucuz ve zorunlu):** kusur kalıpları `DP1..DP9` (defect pattern) olarak yeniden
> adlandırılmalı — `DEFECT-PATTERN-MAP.md`, `DEFECT-REGISTER.md` ve bu ADR birlikte.
> Madde IX (kavram tanıtımı) yeni bir kavramın **ayırt edici bir ad** almasını gerektirir;
> var olan bir numaralandırmayı ikinci bir anlamla yüklemek terminoloji sürüklenmesidir.

### B-27 — `origin: DEFECT-PATTERN-MAP.md` — 5000 katmanı 7000'den türetiliyor (P8 gerilimi)

Künye (`:6`): `origin: DEFECT-PATTERN-MAP.md (P1-P9 kalıp eşlemesi), AUDIT-WAVE2-SECURITY.md §10.5`

Her iki kaynak da `7000-reference-implementation/` altındadır — yani **implementasyon
katmanı**. Bir `5000-architecture` yapıtının `origin`'i implementasyon katmanındaysa,
mimari karar koddan geri türetiliyor demektir. Anayasa P8 (`ENS-0000:70`):

> *"Teori implementasyondan önce gelir. Implementasyon teoriyi kanıtlar; **teori asla koddan
> geri türetilmez**."*

Ve ADR künyesi `principles:` listesinde **P8'i kendisi sayıyor** (`:10`). Yani belge,
ihlal ettiği gerilimin ilkesini kendi dayanağı olarak gösteriyor.

> **Steelman savunma (kısmen kabul ediyorum):** ADR-0003 yeni bir *teori* üretmiyor; var olan
> ADR-0001'i **sertleştiriyor** (`:970` — *"Bu ADR onu değiştirmez, sertleştirir"*). Bir
> mimari kararın, üretimde bulunan kusur sınıflarına yanıt vermesi meşrudur; aksi hâlde hiçbir
> sistem deneyimden öğrenemezdi. P8'in yasakladığı şey *"kodda ne varsa teori odur"* demektir,
> *"kodda bulunan kusur bir mimari kararı tetikleyebilir"* değil.
>
> **Ama savunma künyeyi kurtarmıyor:** `origin` alanı **yetki kaynağını** gösterir, tetikleyiciyi
> değil. Doğru künye `origin: ADR-0001 §5.6/§6` (sertleştirilen karar) olur; kusur haritası ise
> `depends_on` değil, gövdede **kanıt** olarak anılır. Bugünkü hâliyle bağımlılık oku
> **5000 → 7000** yönünde ve Madde XII'nin tek-yönlü akışına aykırıdır.

### B-28 — Künyenin doğru olan tarafları

Dürüstlük gereği: `status: draft`, `canon: false`, `maturity: M0`,
`evidence: {sci: E0, eng: E1, ops: E0, econ: E0}`, `skeptic_review: pending`,
`failure_conditions: stated` — hepsi **doğru ve mütevazı**. `:26-29`'daki durum kutusu
(*"Madde VII gereği `7000-reference-implementation/` bu ADR **Accepted olana kadar** buradaki
hiçbir karara dayanamaz"*) tam olarak doğru bir Madde VII okumasıdır. `eng: E1` iddiası,
ADR'nin gerçekten yaptığı iş (12 gövde okuma, tasarım taslakları) için **hak edilmiştir** —
ve `sci`/`ops`/`econ`'un E0 bırakılması, `evidence-standard`'ın *"seviyesini aşan kesinlikle
sunulamaz"* kuralına uygundur.

`provides:` alanı (`:20`) altı yetenek sayıyor; `draft` bir ADR henüz hiçbir şey
*provide etmez*. Küçük bir künye kusuru — `status: accepted` olduğunda doğru olacak.

## Katıldığım noktalar

Saldırı yalnız zayıflığı raporlarsa kendisi kalibre değildir. Bu ADR, incelediğim ENS
yapıtları arasında **yanlışlanabilirlik disiplini en yüksek olanıdır.**

1. **Tasarım ölçütü doğru ve ENS'e özgü.** `:49-50` — *"Bir karar, ancak 'unutmak' derleme
   hatası ya da tip hatası üretiyorsa sınıfı kapatır. Çağrı yerlerini elle saymak, sayımın
   kendisini yanlışlanabilir bir iddia hâline getirir."* Bu, `Guard.cs`'in gerçek acı
   deneyiminden (7→9 nokta) türetilmiş bir ölçüttür ve prior art'tan kopyalanmamıştır.
   `Guard.cs:40-42`'nin kendi itirafı (*"'kapattım' iddiası bir SAYIMDIR ve sayımlar
   yanlışlanabilir (Madde X)"*) bu ölçütün kaynağıdır. **Doğrulandı.**

2. **K-0'ın geri çekilmesi örnek bir edimdir.** `:217-232` — yanlış bir önermeden türeyen bir
   karar, sonucu makul olsa bile kaldırılmış ve **kutu kalıcı bırakılmıştır** (EC-001).
   *"Yanlış bir önermeden türeyen bir karar, sonucu makul olsa bile **karar değildir**"*
   cümlesi, bu deponun en iyi yönetişim cümlelerinden biridir.

3. **§2'nin dört yeniden sınıflandırması doğru ve zor işti.** `C2`'nin entity/value ayrımına,
   `W1b`'nin üç-durumlu tip köküne, `W2_O1`'in "yetki taklit edilmiyor, hiç yok" tespitine
   **katılıyorum**; üçünü de gövdeden doğruladım. Özellikle `W2_O1` ince bir ayrımdır:
   temsil edilmeyen bir değişmez zorlanamaz.

4. **`C3` uyarısı, saldırıyı önceden yazmaktır.** `:309-313` — kendi sayısını düşürebilecek
   doğrulanmamış varsayımı **DOĞRULANMADI** etiketiyle işaretlemek ve sonucunu (*"sayı 40'tan
   39'a iner"*) önceden yazmak, Madde X'in tam olarak istediği şeydir. Doğruladım: **uyarı
   haklıydı** (B-4). ADR kendi yanlışlanma koşulunu kurdu ve koşul gerçekleşti.

5. **P5/P9 sızması yok** (B-11 üstü). `DEFECT-REGISTER` v1'in cezalandırıldığı hata
   tekrarlanmamış; kapsam dışı hane açık, uzun ve **kendi aleyhine** (R13'ün P8'i yeniden
   ürettiği itirafı dâhil).

6. **§6 meta-kalıp öz-denetim tablosu** (`:892-905`) doğru bir araçtır ve K-3'ü kendi aleyhine
   *"Kısmen"* işaretlemesi dürüsttür. Eksiği (B-9) tabloyu geçersiz kılmaz, genişletmeyi
   gerektirir.

7. **Reddedilen alternatifler gerçekten reddedilmiş.** 18 alternatifin her biri **gerekçeli**
   ve birkaçı **kanıtlı**: `.AsReadOnly()` reddi (`:733-735`) `W2_L4`'ün test yorumuna
   dayanıyor — *denenmiş ve çürütülmüş* bir alternatif. `checked` aritmetiğin `double` taşmasını
   yakalamadığı tespiti (`:828-830`) teknik olarak **doğrudur**.

8. **`Guard.cs`'in elle sayım listesinin kaldırılması** (`:960-963`) — *"O liste bir
   elle-sayımdır ve iki kez yanlış çıktı (7→9). Yerine K-5/K-6'nın mimari testleri geçer:
   sayım artık **otomatik ve yanlışlanabilir** olur."* ADR'nin en küçük ve en karakteristik
   değişikliği budur; katılıyorum.

9. **NFKC'nin reddi teknik olarak doğru.** `:387-393` — NFKC'nin `ﬁ`→`fi`, `①`→`1` gibi
   anlam değiştiren katlamalar yaptığı **ve** homoglyph'leri katlamadığı (Kiril `а` U+0430 ≠
   Latin `a` U+0061) doğrudur. "Yaygın yanlış inanç" tespiti yerindedir ve ADR bu yüzden
   ayrı bir mekanizma (M-3) tanımlamak zorunda kalmıştır — kestirme yapmamış.

10. **Prior art'ta uydurma yok** (§7). Otuz atıfta bir ifade hatası (JEP 486) ve bir boş
    bölüm numarası (`RFC 4120 §`) buldum; **uydurulmuş kaynak bulamadım.** Bu, ENS'te sık
    görülen bir kusur değildir ve kayda geçirilmelidir.

## Sahibine talepler

Sıra şiddet sırasıdır. **Bloke ediciler kapanmadan ADR `accepted` olmamalıdır** — ve Madde VII
gereği `accepted` olmadan kernel bu kararlara dayanamaz.

### Bloke edici

- **T-A (sayı yeniden hesaplanır — B-1…B-5, B-11).** `DEFECT-PATTERN-MAP.md:226`'nın
  `12+13+6+5+5+6 = 41` toplamı **47** olarak düzeltilir (haritada ve ADR `:37`'de).
  ADR'nin manşeti: `47 − W2_O1 − C2 − W1b − C3 = **43**`. `W1b` **ikinci kez** çıkarılmaz;
  `W3` **çıkarılmaz** (kapanıyor, şiddeti düşük — bu ayrı bir not olarak yazılır).
  `C3` §5.1 tablosuna **dördüncü satır** olarak eklenir (B-4 doğrulandı).
  P1'in üye sayısı 11 → **10** olur.

- **T-B (doğrulama aleti onarılır — B-7).** `AdversarialWave_SecurityTests.cs:27-29` künyesi
  **yanlıştır**; ya künye düzeltilir ya fixture'lar `\uXXXX`/`\0` escape'lerine çevrilir.
  **İkincisi tercih edilmelidir** ve **K-2'den ÖNCE** yapılmalıdır: ADR'nin tek yanlışlanma
  yordamı bu testlerdir ve bugün kodlamaya bağımlı, sessizce fail-open bir yordamdır.

- **T-C (K-1'in `with` deliği kapatılır — B-12).** `ToolAuthorization`, `GateResult`,
  `Proposal` ve `DecayPolicy.Disabled` `record` **olmamalı** (ya da kopya kurucu `private`
  yapılmalı). Bugünkü taslak (`:261`, `:595`) `with` ile mühür kopyalamaya açıktır ve `W15`,
  `W4a`, `E3` bu yoldan geri gelir. OQ1'in kapsamı **yetki tiplerini de** içerecek biçimde
  genişletilir.

- **T-D (K-4, K-3'ü ihlal etmez — B-13).** `DecayPolicy.Disabled`'ın `DateTimeOffset At`
  parametresi ya kaldırılır (kernel `TimeProvider`'dan doldurur) ya `TimeWindow.Accept`'ten
  geçer. Aksi hâlde K-4, `W2_L3`'ü yeni bir tipte yeniden üretir.

- **T-E (Türkçe casing kararı yeniden verilir — B-21/B-22).** `ToUpperInvariant`, Türkçe
  girdide `ı`/`i`'yi **birleştirir** ve `İ`/`i`'yi **ayırır**. `G4`, `W7f`, `W1a`, `W1c`
  kapanış satırları (`:434-436`, `:441`) bu hâliyle **desteklenmiyor**. İki seçenekten biri
  yazılı olarak seçilir: (a) harf katlama kaldırılır, `G4` `Register`-tarafı **ret** ile
  kapanır (M-3'ün aynısı); (b) açık bir case-folding tablosu tanımlanır ve `İ`/`ı` çifti
  orada özel olarak ele alınır. Ayrıca `ToLower()`/`ToUpper()`/kültürlü `Compare` yasağı
  K-3'ünkiyle **aynı** `BannedApiAnalyzers` mekanizmasına bağlanır.

### Yüksek

- **T-F (maliyet tabloları yeniden sayılır — B-23/B-24).** `AdversarialWave_SecurityTests.cs`
  K-1, K-2, K-5, K-6'nın "Etkilenen test" listelerine eklenir;
  `Capability/CapabilityRegistry.cs` K-5'in "Dokunulan üretim dosyası" listesine eklenir.
  K-5'in *"test etkisi en düşüğüdür"* ve K-2'nin *"8 dosya"* iddiaları yeniden yazılır.
- **T-G (K-1/K-2/K-4'e mimari test — B-9).** K-5/K-6'nın assembly taramasının kardeşleri:
  (a) public üyelerde kimlik rolü taşıyan ham `string` parametre yok, (b) yetki tiplerinde
  public/kopya kurucu yok, (c) politika rolünde ham `double` parametre yok. §6 tablosunun
  üç satırı *"sınıf — ama regresyon koruması yok"* olarak düzeltilir.
- **T-H (P6 envanteri genişletilir — B-8).** `CapabilityRegistry.Packs` (`:135`, canlı
  `ValueCollection`) ve `EnabledPacks` (`:136-137`, downcast edilebilir `List`) P6'ya eklenir
  ya da eklenmeme gerekçesi yazılır. `CapabilityPack.AllowedTools`/`RequiresHumanApprovalFor`
  (`:63`, `:70`) `IReadOnlySet<string>` → `FrozenSet<string>` yapılır.
- **T-I (`DP` önekiyle ad çakışması giderilir — B-26).** Kusur kalıpları `DP1..DP9` olur;
  `DEFECT-PATTERN-MAP.md`, `DEFECT-REGISTER.md` ve bu ADR birlikte güncellenir. Künyedeki
  `principles:` alanı Anayasa anlamında kalır.
- **T-J (uygulama sırası bir plan yapıtı olur — B-17).** OQ6 bir "açık soru" satırı değil;
  `plan-first.md`'ye göre **Tier 3** bir plandır (`IsIrreversible`: breaking imza + kernel
  davranışı). Bilinen bağlar: OQ1 → hepsi; K-4 → K-3 (R9); K-1 ↔ K-2; T-B → K-2.
- **T-K (`W2f`'nin kapanışı ispatlanır — B-10).** `CapabilityPack` kurucusunun
  `IEnumerable<ToolName>` alacağı **açıkça** yazılır; bugün varsayılıyor.

### Orta

- **T-L (B-19).** *"JEP 486 ile kaldırıldı"* → *"JEP 486 (JDK 24) ile **kalıcı olarak devre
  dışı bırakıldı**; API'nin kaldırılması sonraki bir sürüme bırakıldı."*
- **T-M (B-19 tablosu).** `:524` — `RFC 4120 §` ifadesindeki boş bölüm numarası ya doldurulur
  ya `§` kaldırılır. Bloch atfına **baskı** eklenir (Item 50 = 3. baskı). `System.Collections.Immutable`
  yılı düzeltilir ya da kaldırılır.
- **T-N (B-16).** R19/fc-7'deki çelişki daraltılır: `Guard.NormalizedDeficit` ile çelişen
  `Measured` değil **`UnitMeasured`**'dır. OQ2 buna göre yeniden yazılır.
- **T-O (B-27).** `origin:` alanı `ADR-0001 §5.6/§6` olur; `DEFECT-PATTERN-MAP.md` gövdede
  **kanıt** olarak anılır, künyede yetki kaynağı olarak değil. `provides:` alanı `accepted`
  olana kadar boşaltılır.
- **T-P (B-6/B-7 kural katmanı).** `work-protocol.md` §3.2'ye bir satır: *"Bir dosya araç
  tarafından atlanıyorsa, o dosyanın **kendi kodlama beyanı da** doğrulanır — beyan yanlış
  olabilir ve asıl kök neden odur."* Bu turda kural §3.2'nin kendisi dört tabloda ihlal edildi
  (B-23); ihlalin nedeni kuralın eksikliğidir, ihmal değil.

---

## Tekrar-sınav koşulu

T-A…T-E kapandığında **yeni ve bağımsız** bir tur gerekir (bu kaydın yazarı o turu yapamaz —
GOV-000 G2). O turun tek sorusu: *"Yeniden hesaplanan 43, uygulama sonrası `AUDIT_FIXED_*`
sayımıyla örtüşüyor mu; örtüşmüyorsa fazla mı eksik mi ve hangi kalıpta?"*

`work-protocol.md` §3.1 gereği bu ADR **iki boyut** ister. Bu kayıt **birinci boyuttur**
(iddia/sayı/yanlışlanabilirlik/prior art). İkinci boyut (mühendislik/yapısal) ayrıca
gelmelidir; B-8, B-9, B-12, B-13 onun alanına da girer ve **bağımsız olarak yeniden
sınanmalıdır** — benim bulmam, doğrulanmış olmaları anlamına gelmez.

**Doğrulayamadıklarım (uydurmuyorum):** `dotnet test` çalıştıramadım (`Bash` yok); "373 test"
ve "373/373 geçiyor" (`:230`) **bu turda doğrulanmadı**. `AdversarialWave_SecurityTests.cs`'in
`:927` ve `:958` satırlarını okumadım. `W5a`, `W5b`, `W5d`, `W5e`, `W5g`, `W7f`, `W7h`, `W1a`
gövdelerini okumadım — okuduğum gövdeler: `C2`, `C3`, `W1b`, `W1c`, `W1d`, `W1e`, `W2a`,
`W2b`, `W2c`, `W2d`, `W2e`, `W2f`, `W3a`, `W3b`, `W3c` (15) + `CapabilityRegistry.cs` ve
`Guard.cs` ilgili bölümleri.
