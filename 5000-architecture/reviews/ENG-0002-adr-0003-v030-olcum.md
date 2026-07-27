---
id:            ENG-0002
title:         ADR-0003 v0.3.0 — değişen kararların ölçümü
type:          review
canon:         false
origin:        ENG-0001 (v0.1.0 ölçümü), ADR-0003 §0.7 (D-1..D-9)
depends_on:    [ADR-0003, ENG-0001]
referenced_by: []
status:        draft
owner:         ens-backend-architect
version:       0.1.0
last_reviewed: 2026-07-27
maturity:      M0
dimension:     Engineering
failure_conditions: stated
evidence:      {sci: E0, eng: E3, ops: E0, econ: E0}
---

# ENG-0002 — ADR-0003 v0.3.0 ölçümü

> **Bağımsızlık beyanı (GOV-000 G2):** ADR-0003'ün yazarı `ens-ai-architect`; bu kayıt
> `ens-backend-architect` rolüyle, ayrı context'te üretildi. Bu, `ENG-0001`'in **ikinci
> turudur** — aynı rol, farklı sürüm. Aynı-rol tekrarının G4 anlamındaki sınırı §8'de
> açıkça kaydedilmiştir.
>
> **Yöntem:** tartışmak değil **derlemek/çalıştırmak**. Her iddia ya bir spike çıktısıyla
> ya bir `grep` sayımıyla ya bir test koşumuyla desteklenir. Desteklenemeyen her şey
> **DOĞRULANMADI** yazılır (`work-protocol.md` §3).
>
> **Kapsam sınırı:** `Ens.Kernel/` **değiştirilmedi** — ADR `draft`, Madde VII gereği
> referans implementasyon ona dayanamaz. Bütün spike'lar ayrı bir scratchpad projesinde
> derlendi.

## Verdict tablosu

<!-- DOLDURULACAK -->

## 0. Ölçüm ortamı

| | |
|---|---|
| SDK | `dotnet --version` → **10.0.101** |
| Hedef | `net10.0`, `Nullable=enable`, `LangVersion=latest` |
| **`AllowUnsafeBlocks`** | spike csproj'larında **YOK** (§1'in E-1.5'i için kritik) |
| Spike konumu | `…/scratchpad/spike2/{FakeKernel,Attacker}`, `…/scratchpad/neg` |
| Kernel'e dokunma | **yok** — `git status` `7000-reference-implementation/` altında değişiklik göstermiyor |

**Taban koşumu (`work-protocol.md` §4 — sahibi değil, ölçen çalıştırdı):**

```
dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj
Başarılı! - Başarısız: 0, Başarılı: 373, Atlanan: 0, Toplam: 373, Süre: 306 ms
```

**373/373 hâlâ geçiyor.** `ENG-0001`'in 217 ms'i bu koşumda 306 ms — süre farkı makine
gürültüsüdür, sayı aynıdır. v0.2.0/v0.3.0 metin değişiklikleri kernel'e dokunmadı;
bu beklenen sonuçtur ve **ADR'nin `draft` kalmasının kanıtıdır** (Madde VII korunuyor).

## 1. D-1 — `sealed class` gerçekten kapatıyor mu?

ADR §4.1 iskeleti (`sealed class` + `private` kurucu + get-only property + `AuthoritySeal`
referans eşitliği) birebir kuruldu; saldırı **ayrı assembly'den** yapıldı. Aşağıdaki her
satır `dotnet run` çıktısıdır.

### E-1.1 Kapanan saldırılar — D-1 gerçek kazanç sağlıyor

| # | Saldırı | Sonuç | Kanıt |
|---|---|---|---|
| B1 | `legit with { }` | **derlenmiyor** | `error CS8858: 'ToolAuthorization' alıcı türü geçerli bir kayıt türü değil ve bir yapı türü değil` |
| B2 | `RuntimeHelpers.GetUninitializedObject` | `seal=-1` → **KABUL=False** | fail-closed |
| B4 | `Activator.CreateInstance(t, nonPublic: true)` | **`MissingMethodException`** | parametresiz kurucu yok |
| B4b | private kurucu + **sahte** `AuthoritySeal` | `seal=12479570` → **KABUL=False** | `ReferenceEquals` tutuyor |
| B5 | `JsonSerializer.Deserialize<ToolAuthorization>` | **`NotSupportedException`** | aşağıda |
| B6 | `FormatterServices.GetUninitializedObject` | tip **var** ve çalışıyor, ama **KABUL=False** | B2 ile aynı |

`ENG-0001`'in `[A2]` bulgusu (`with` ile mühür koruyarak yetki yükseltme) **kapandı** ve
kapanma **derleyici tanısıyla** oldu — konvansiyonla değil. D-1'in gerekçe cümlesi
(*"konvansiyonla zorlanan karar, karar değildir"*) bu noktada **ölçümle doğrulanıyor**.

`B4b` en önemli olumlu bulgudur: private kurucuya reflection'la ulaşmak **yetmiyor** —
saldırganın ayrıca **gerçek mühür nesnesine** ihtiyacı var. Morris/Miller brand deseni
burada işini yapıyor.

### E-1.2 Hâlâ geçen saldırılar — mühür **kopyalanabiliyor**, sınıf kapanmadı

```
[B3]  MemberwiseClone:                Tool=read_stock    seal=33826822  KABUL=True
[B3b] MemberwiseClone + alan yazma:   Tool=wire_transfer Scope=9999 seal=33826822 KABUL=True
[B4c] private ctor + ÇALINMIŞ mühür:  Tool=wire_transfer Scope=9999 seal=33826822 KABUL=True
[B7]  uninit + mühür enjeksiyonu:     Tool=wire_transfer Scope=9999 seal=33826822 KABUL=True
```

`ENG-0001`'in kırdığı **saldırı sınıfı** — *"mührü koru, payload'u değiştir"* — kapanmadı.
Yalnızca **taşıyıcısı** değişti: `with` operatöründen reflection'a. Üç ayrı reflection yolu
aynı sonucu veriyor; `read_stock` yetkisi `wire_transfer`/`scope 9999` oluyor ve registry
**kabul ediyor**.

ADR bu noktada bir savunma taşıyor: `DP5` (reflection) **kapsam dışı** (§3). O savunma
`B3`/`B4c`/`B7` için geçerlidir ve kabul ediyorum. Ama E-1.3 onu geçersiz kılıyor.

### E-1.3 ⛔ BLOKE EDİCİ — `Unsafe.As`: reflection YOK, `unsafe` blok YOK, mühür yine kopyalanıyor

```
[B8] Unsafe.As tip taklidi (reflection YOK):
     Tool=wire_transfer Scope=9999 Allowed=True seal=33826822 KABUL=True
```

Saldırgan, aynı alan düzenine sahip bir `Shadow` sınıfı tanımlayıp
`Unsafe.As<ToolAuthorization, Shadow>(ref x)` ile referansı yeniden yorumluyor; meşru
nesnenin `_issuer` alanını **okuyor**, sahte nesneye **yazıyor**.

**Bunun neden `DP5` savunmasının dışında olduğu:**
`System.Runtime.CompilerServices.Unsafe` **reflection değildir** — `System.Reflection`
kullanılmadı, `BindingFlags` kullanılmadı, `[UnsafeAccessor]` kullanılmadı. Dahası
**`<AllowUnsafeBlocks>` spike csproj'ında yok** (yukarıda; dosya 8 satır) — yani bu saldırı
tamamen "güvenli" derlenen C# ile yapılıyor. Reflection'ı kısıtlayan hiçbir önlem
(`[UnconditionalSuppressMessage]`, trimming, `ReflectionDisabled` feature switch) bunu
engellemez.

### E-1.4 ⛔ Daha ağırı — **değişmez nesne yerinde değiştiriliyor**

```
[B9] önce:                 Tool=read_stock    Scope=1    seal=33826822 KABUL=True
[B9] sonra (AYNI nesne):   Tool=wire_transfer Scope=9999 seal=33826822 KABUL=True
```

Burada yeni nesne **üretilmiyor**. Registry'nin bizzat verdiği `read_stock` yetkisi,
**aynı referans olarak**, `wire_transfer`'a dönüşüyor. Yani:

- `ReferenceEquals` tabanlı mühür doğrulaması **hiçbir şey ölçmüyor** — nesne gerçekten
  registry'nin ürettiği nesnedir.
- `get-only property` **çalışma zamanında değişmezlik sağlamıyor**; yalnızca C# derleyici
  yüzeyinde sağlıyor.
- Yetki nesnesini bir kez alıp **sonra doğrulayan** her tasarım (TOCTOU) bu yolla kırılır.

### E-1.5 D-1'in gerekçe cümlesindeki kavram hatası

ADR §4.1, `internal` seçeneğini şöyle reddediyor:

> *"`internal`, güven sınırı **assembly** olduğunda işe yarar; burada güven sınırı
> **çağrı grafiğidir**."*

**Ölçüm bu cümleyi çürütüyor.** B8/B9, çağrı grafiğinde hiçbir ayrıcalığı olmayan bir
çağıranın, hiçbir reflection ve hiçbir `unsafe` blok kullanmadan mührü kopyalayabildiğini
gösteriyor. .NET'te **in-process güven sınırı çağrı grafiği olamaz** — bellek düzeni
paylaşıldığı sürece sınır **process**'tir. K-1'in `ReferenceEquals` mührü, düşmanca bir
*in-process* çağırana karşı değil, **kazayla yanlış çağıran koda** karşı koruma sağlar.

Bu bir "K-1'i çöpe at" bulgusu değildir — kaza-koruması gerçek bir değerdir ve `W16`
(`toolAuthorization: null`) gibi kimlikler bununla gerçekten kapanır. Ama **iddianın seviyesi
düşürülmelidir**: K-1 *unforgeable capability* üretmez; **derleme-zamanı zorlaması + kaza
koruması** üretir.

### E-1.6 D-1 VERDICT: **kısmen**

| Alt iddia | Verdict |
|---|---|
| `with` deliği kapandı, derleyiciyle | **düzeltme tuttu** (`CS8858`) |
| Sahte mühür reddediliyor | **düzeltme tuttu** (`B4b`) |
| "Mührü koru, payload değiştir" **sınıfı** kapandı | **tutmadı** (`B3b`,`B4c`,`B7`,`B8`,`B9`) |
| `DP5` kapsam-dışı savunması bu sınıfı kapsıyor | **tutmadı** — `B8`/`B9` reflection değil |

> **Talep T-1 (bloke edici):** §4.1'in *"güven sınırı çağrı grafiğidir"* cümlesi ölçümle
> çelişiyor; ya kaldırılmalı ya *"güven sınırı process'tir; K-1 kaza korumasıdır"* olarak
> düzeltilmelidir. `E3`/`W4a`/`W15`/`H1` kimliklerinin "kapandı" hanesine yazılması bu
> düzeltmeye bağlıdır: **düşmanca in-process çağıran modelinde kapanmıyorlar.**

## 2. D-2 — reddetme kararı uygulanabilir mi?

**Yöntem:** `7000-reference-implementation/` **kopyalandı** (`scratchpad/kcopy`), kopyada
`Canon.Require(...)` beş giriş noktasına eklendi, testler koşuldu. Asıl ağaca dokunulmadı;
kopyanın tabanı önce doğrulandı: **373/373, 195 ms**.

### E-2.1 Çağrı yeri envanteri (ölçüldü, `grep -a`)

| API | Çağrı yeri (kernel+test+demo) |
|---|---|
| `.Authorize(` | **56** |
| `.Register(` | **42** |
| `new CapabilityPack(` | **25** |
| `.Disable(` | **11** |
| `.IsEnabled(` | **5** |
| `.Enable(` | **1** |

Kanonik olmayan **literal** kimlikler: `"Operations"`(8), `"Strict"`(7), `"Loose"`(4),
`"P"`(4), `"X"`(3), `"Repor‮troper"`(1 — **U+202E RTL override**), `"read _stock"`,
`"\t"`, `""`, `"  "`, `"   "`, `"ghost-"+i`(1000 çağrı), ve `W1c`/`W2a`'nın 18 InlineData'sı.

### E-2.2 ⛔ BLOKE EDİCİ — "kanonik biçim" **tanımsız**, ve tanım sonucu 2.6 kat değiştiriyor

D-2 *"NFC + belirlenmiş harf düzeni"* diyor ama **hangi harf düzeni** olduğunu söylemiyor.
İki okuma da savunulabilir; ikisini de uyguladım:

| | Varyant B — NFC + kontrol/format/boşluk yasak, **harf düzeni korunur** | Varyant A — B + **küçük harf zorunlu** |
|---|---|---|
| Kırılan test | **20** | **52** |
| `AUDIT_DEFECT_*` (istenen dönüşüm) | 10 | 19 |
| **Yan hasar** (`HOLDS`/`FIXED`/`FINDING`/düz test) | **10** | **33** |
| Kırılan `AUDIT_FIXED_*` **regresyon bekçisi** | 0 | **6** |

Bir mimari kararın maliyeti, yazılmamış bir alt-karara göre **20 ↔ 52** arasında salınıyor.
Bu, ADR'nin kendi ölçütüne (*"karar, unutmayı derleme hatası yapmalı"*) göre bir karar değil,
**bir karar yeri**dir.

### E-2.3 ⛔ BLOKE EDİCİ — zayıf okumada D-2 `W1a`/`W1b`/`W1e`'yi **kapatmıyor**

Varyant B'de kırılan 20 testin içinde `W1a`, `W1b`, `W1e` **yok** — yani hâlâ yeşil, yani
**kusur duruyor**. Sebep ölçülebilir: `Disable("operations")` biçimsel olarak
**kusursuz kanoniktir** (saf küçük ASCII, kontrol yok). Kayıtlı ad `"Operations"`.
Biçim kanonikleştirmesi bu çağrıyı reddetmez; sessizce kabul eder ve Pack etkin kalır.

Aynı sebeple `W1c`'nin **9 InlineData'sının 2'si** varyant B'de hâlâ geçiyor:
`"OPERATIONS"` (yalnız harf düzeni farkı) ve `"Operаtions"` (Kiril `а` — homoglif, D-3 ile
zaten kapsam dışı). Yani `W1c` **kısmen** kapanıyor: 7/9.

> **Ölçülmüş sonuç:** D-2'nin `W1a`/`W1b`/`W1c`/`W1e`'yi kapatması için **biçim reddi
> yetmiyor**; ayrıca bir **varlık kontrolü** (`Disable` kayıtlı olmayan adı reddeder) gerekiyor.
> ADR bu ikinci mekanizmayı **hiç kararlaştırmıyor**. D-2'nin kendi gerekçe paragrafı
> (*"çağrı **hata verir**"*) bu ikinci mekanizmayı varsayıyor ama metne yazmıyor.

### E-2.4 ⛔ `Disable` `void` — reddetme **breaking API değişikliğidir**, ADR bunu saymıyor

`CapabilityRegistry.cs`: `public void Disable(string packName) => _disabled.Add(packName);`
Dönüş değeri yok, `out` yok, sonuç kanalı yok. "Reddedilir" kararının tek uygulanabilir
biçimi **exception atmaktır**. Bu, `ENG-0001` §E-7.4'ün breaking-API tablosunda **yer
almıyor** ve D-2 de "bedeli dürüstçe: daha katı arayüz" derken bunu ölçmüyor. Ölçülmüş bedel:
`.Disable(` 11 çağrı yerinden **10'u** bugün doğrulanmamış girdi geçiriyor.

### E-2.5 Meta-kalıp DOĞRULANDI — örnek değil, **giriş noktaları** ayrışıyor

Soru: `Register`/`Authorize`/`Disable` **aynı** kanonikleştirmeyi mi uyguluyor? Ölçüm
(`CapabilityRegistry.cs` okundu): **hayır — bugün üç ayrı rejim var, kanonikleştirme ise
hiçbirinde yok.**

| Giriş noktası | Bugünkü doğrulama |
|---|---|
| `CapabilityPack` ctor — `name` | `IsNullOrWhiteSpace` → `ArgumentException` |
| `CapabilityPack` ctor — `allowedTools` üyeleri | **hiçbiri** |
| `Register(pack)` | yalnız çift-kayıt kontrolü |
| `Authorize(toolName)` | `IsNullOrWhiteSpace` → `ArgumentException` |
| **`Disable` / `Enable` / `IsEnabled`** | **hiçbiri** — `void`, sessiz |

Yani `AUDIT-WAVE2-SECURITY` §10.5'in meta-bulgusu (*"örnek kapatıldı, sınıf açık"*) burada
**daha keskin** bir biçimde geçerlidir: örnek bile kapatılmamış, ve korumasız kalan üç giriş
noktası tam da güvenlik-kritik olan **revoke** yoludur.

### E-2.6 ⛔ ADR §7'nin yanlışlanma ölçütü **sağlam değil**

D-8 diyor ki: *"kararlar uygulanınca 43 kimliğin `AUDIT_DEFECT_*` testleri `AUDIT_FIXED_*`'a
dönmelidir."* Ölçüm bu ölçütün **ayırt edemediğini** gösteriyor. Varyant A'da `W1a` kırmızıya
döndü — ama sebep şu:

```
System.ArgumentException : 'name' kanonik degil (kucuk harf degil). (Parameter 'name')
  at Ens.Kernel.Capability.CapabilityPack..ctor(...)
  at ...AdversarialWave_SecurityTests.cs(77,0)   <-- registry.Register(OpsPack())
```

Satır 77 **`Register`** çağrısıdır; testin saldırı satırı (`Disable("operations")`, satır 80)
hiç çalışmadı. **Kusur kapanmadı — kurulum kırıldı.** Kırmızı bir `AUDIT_DEFECT_*`, "kusur
kapandı"nın kanıtı değildir; "test artık o kusuru ölçmüyor"un da kanıtı olabilir.

> **Talep T-2 (bloke edici):** yanlışlanma ölçütü *"test kırmızıya döndü"* değil,
> *"test `AUDIT_FIXED_*` olarak **yeniden yazıldı** ve **aynı saldırı satırına** ulaşıp
> ters iddiayı doğruluyor"* olmalıdır. Bugünkü haliyle §7 sayısı kendi kendini onaylar.

### E-2.7 D-2 VERDICT: **kısmen**

| Alt iddia | Verdict |
|---|---|
| `ToUpperInvariant` kaldırıldı (`ENG-0001` E-3.1 kapandı) | **düzeltme tuttu** |
| Reddetme mekanik olarak uygulanabilir | **düzeltme tuttu** (kopyada derlendi + koşuldu) |
| "Kanonik biçim" kararı verilmiş | **tutmadı** — tanımsız; maliyet 20↔52 |
| `W1a`/`W1b`/`W1e` kapanıyor | **tutmadı** (varyant B'de üçü de yeşil kalıyor) |
| `W1c` kapanıyor | **kısmen** — 7/9 |
| Bedel dürüstçe ölçülmüş | **tutmadı** — `void Disable` breaking değişimi sayılmamış |

## 3. D-5 — `IComparable<Measured>` kırılmayı kapatıyor mu?

ADR §4.6'nın `Measured` tipi + `IComparable<Measured>` kuruldu; `Scheduler.cs:124` ve
`CompanyMemory.cs:260` desenleri **çalıştırıldı**.

### E-3.1 Evet — iki desen de çalışıyor

```
[M1] Scheduler deseni (OrderByDescending + ThenByDescending): OK -> b,c,a
[M2] CompanyMemory deseni (OrderByDescending(Salience)):      OK -> b,a,c
[M3] IComparable YOK (ENG-0001 tabanı) -> InvalidOperationException:
     "Failed to compare two elements in the array"
```

`ENG-0001` E-5.3'ün çalışma-zamanı çöküşü **kapandı**. `M3` aynı oturumda tabanı da yeniden
üretiyor — yani karşılaştırma gerçek, tek koşumluk şans değil.

### E-3.2 NaN sorunu YOK — ve nedeni ölçüldü

```
[M4] Of(NaN) reddedildi: ArgumentException
[M5] default(Measured).Value = 0        (kurucu ATLANDI ama NaN değil)
[M8] new Measured[2][0].Value = 0       (dizi tahsisi de 0.0)
[M6] default(Measured).CompareTo(Of(0)) = 0
[M7] -0.0 → 0 · CompareTo(+0)=0 · Equals=True · sözleşme tutarlı: True
```

`Measured` **ölçülemez değer taşıyamıyor**: tek giriş `Of` ve o `NaN`/`±∞`'u reddediyor;
`default(T)`/dizi tahsisi kurucuyu atlıyor ama sonuç `0.0` — geçerli, sonlu, sıralanabilir.
Dolayısıyla sıralama **tam (total) bağıntı** kalıyor. Görevin sorduğu risk **gerçekleşmiyor.**

`M7` ayrıca `IComparable` sözleşmesinin (`CompareTo == 0 ⟺ Equals`) tuttuğunu gösteriyor —
ve bunu sağlayan şey `Normalize(v) = v + 0.0`'dır (`W3` düzeltmesi). İki karar burada
**birbirini destekliyor**; ADR bu bağı yazmıyor, yazmalı.

### E-3.3 Kalan delik 1 — non-generic `IComparable` yok

```
[M9]  Measured : IComparable (non-generic)? False
[M10] ArrayList.Sort (non-generic yol) PATLADI: InvalidOperationException
```

`IComparable<Measured>`, non-generic `IComparable`'ı **karşılamıyor**. `Array.Sort(object[])`,
`ArrayList`, `IComparer` almayan eski API'ler ve bazı serileştirme/rapor yolları hâlâ patlar.
Kernel bugün generic koleksiyon kullanıyor (`grep`: `ArrayList` **sıfır**), yani **bugün
sömürülemez** — ama K-6'nın *"artık sıralanır"* iddiası **generic yolla sınırlıdır** ve ADR
bunu yazmalıdır. Düzeltmesi tek satır: `: IComparable<Measured>, IComparable`.

### E-3.4 ⛔ BLOKE EDİCİ — D-5 ile D-6 **aynı tip hakkında çelişiyor**

- **D-6:** *"Karar: `class`."*
- **§4.6 (v0.3.0 metninde değişmedi):** `public readonly record struct Measured`

İkisi aynı belgede duruyor. Hangisinin bağladığı yazılı değil. Fark **ölçüldü** ve önemsiz
değil:

```
[C1] class + IComparable, null içeren liste: SESSİZCE sıraladı
     -> b(0,9), a(0,5), null-alani(NULL)
[C4] 100k `class` Measured üretimi: 2349 KB tahsis   (struct'ta 0 KB)
```

`class` seçilirse: (a) ilklendirilmemiş bir `Measured` alanı `null` olur ve
`Comparer<T>.Default` onu **en küçük değer gibi** sessizce sıralar — `Scheduler`'ın dikkat
bütçesinde bir kararın sessizce en sona düşmesi demektir; (b) `Scheduler`/`ContextScore`
sıcak yolunda **her ölçüm bir heap tahsisi**dir (100k adımda 2.3 MB, `Release`).
`struct` seçilirse D-6'nın `CS8618` gerekçesi `Measured` için **hiç çalışmaz**
(`ENG-0001` E-2.4: struct varyantında hiçbir tanı üretilmedi).

> **Talep T-3 (bloke edici):** D-6 karar-başına ayrıştırılmalıdır. `ENG-0001` E-2.4 bunu
> zaten talep etmişti (*"OQ1'in cevabı karar-başına ayrışır"*); v0.3.0 tek cevap verdi ve
> §4.6'nın kodunu güncellemedi. Bugünkü metin **iki karar birden** içeriyor.

### E-3.5 Kalan delik 2 — `implicit operator double` kapıyı atlatılabilir bırakıyor

```
[M11] implicit double ile sıralama hâlâ mümkün: b,a,c   <-- kapı ATLANABİLİR
```

`OrderByDescending(x => (double)x.Priority)` derleniyor ve çalışıyor. Bu ADR'nin **kasıtlı**
asimetrisidir (*"tüketim serbest"*) ve E-3.5 onu bir kusur olarak saymıyor — ama sonucu
şudur: `Measured` bir kez `double`'a döndüğünde geri dönüş yolu yoktur, ve `Measured a * b`
ifadesi **`double` üretir**. Yani K-6'nın tek gerçek zorlaması `public` üye taramasıdır —
`ENG-0001` E-5.4'ün *"kapı aritmetikte deliniyor"* bulgusu **D-5 ile değişmedi**.

### E-3.6 D-5 VERDICT: **düzeltme tuttu** (iki kayıtla)

| Alt iddia | Verdict |
|---|---|
| `Scheduler.cs:124` deseni artık çalışıyor | **düzeltme tuttu** (`M1`) |
| `CompanyMemory.cs:260` deseni artık çalışıyor | **düzeltme tuttu** (`M2`) |
| NaN sıralamayı tanımsız yapmıyor | **düzeltme tuttu** (`M4`,`M5`,`M8` — tip NaN taşıyamıyor) |
| `IComparable` sözleşmesi tutarlı | **düzeltme tuttu** (`M7`) |
| Non-generic yol | **kısmen** — `IComparable` eklenmeli (`M9`,`M10`) |
| Tip seçimi (`struct` mı `class` mı) | **tutmadı** — D-5 ↔ D-6 çelişkisi (`C1`,`C4`) |

## 4. D-6 — `WarningsAsErrors CS8618` hangi olguyu yakalıyor?

<!-- DOLDURULACAK -->

## 5. D-8 — 43 sayısı yeniden hesaplandı

<!-- DOLDURULACAK -->

## 6. Yeni yüzey — factory, hata mesajı, sızıntı

<!-- DOLDURULACAK -->

## 7. Maliyet — 17/17 tablosu ve taban hâlâ geçerli mi?

<!-- DOLDURULACAK -->

## 8. D-9 — uygulama sırası tutarlı mı?

<!-- DOLDURULACAK -->

## 9. Katıldığım noktalar

<!-- DOLDURULACAK -->

## 10. Talepler

<!-- DOLDURULACAK -->
