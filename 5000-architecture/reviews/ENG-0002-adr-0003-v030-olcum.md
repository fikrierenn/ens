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

Her değişen karar için: `düzeltme tuttu` | `kısmen` | `tutmadı`. Sütun **kanıt**, ölçümün
yapıldığı bölümdür — tartışma değil, çıktı.

| # | Karar | Verdict | Belirleyici kanıt |
|---|---|---|---|
| **D-1** | `record` → `sealed class` | **kısmen** | `with` derleme hatası (`CS8858`) ✅; ama `Unsafe.As` ile **reflection'sız** mühür kopyalama ve **yerinde mutasyon** geçiyor (§E-1.3/E-1.4); kural K-4'e süpürülmemiş (§E-1.7) |
| **D-2** | harf katlama → **reddetme** | **kısmen** | Uygulanabilir (kernel kopyasında derlendi+koşuldu); ama "kanonik biçim" tanımsız → **20 ↔ 52** test; zayıf okumada `W1a`/`W1b`/`W1e` **kapanmıyor** (§E-2.2/E-2.3) |
| **D-3** | `W2c` kapsam dışı | **tutmadı** | Öncül doğru (Script API yok) ama `\p{IsCyrillic}` + `UnicodeRanges` (162 üye) `W2c`'nin gövdesini yakalıyor (§E-5A.1) |
| **D-4** | `Disabled`'dan `At` kaldırıldı | **düzeltme tuttu** | Gövde satırı 891 doğrulandı; `DateTimeOffset At` yok (§E-5A.2) |
| **D-5** | `Measured : IComparable<Measured>` | **düzeltme tuttu** | `Scheduler`/`CompanyMemory` desenleri **çalıştı**; NaN tipe giremiyor → sıralama tam (§E-3.1/E-3.2). İki kayıt: non-generic `IComparable` yok; D-6 ile çelişki |
| **D-6** | `class` + `WarningsAsErrors CS8618` | **tutmadı** | `CS8618` kurucu-çıkışı tanısıdır; `default(class)` → **`CS8600`** (listede yok), `default(struct)` → **hiçbir tanı** (§E-4.1). Ayrıca `ENG-0001`'in `Nullable` talebi **daraltılarak** zayıflatıldı (§E-4.3) |
| **D-7** | `P1..P9` → `DP1..DP9` | **tutmadı** | 8 `DP` / **78** `P`; künye hâlâ `principles: [P1, P5, …]` (§E-5A.3) |
| **D-8** | Sayı **43** | **kısmen** | Aritmetik **doğru** — bağımsız sayımla 47 ve `47−4=43` doğrulandı ✅; ama gövdede hâlâ **41**, **40** ve **"40+4"** var (dört sayı), ve 43'ün en az 6 üyesi ölçümde kapanmıyor (§E-5.1/E-5.3/E-5.5) |
| **D-9** | Uygulama sırası | **kısmen** | `ENG-0001` ile **birebir aynı** ✅; ama Faz 0'ın içeriği yanlış (`CS8618` daraltması, tek-cevap OQ1) ve **faz kapısı yok** (§E-8.1/E-8.2) |

**Toplam: 2 tuttu · 4 kısmen · 3 tutmadı.**

**Bloke ediciler (kapanmadan `draft` → `accepted` yapılmamalıdır):**
T-1 (D-1'in güven sınırı iddiası), T-2 (yanlışlanma ölçütü), T-3 (D-5 ↔ D-6 çelişkisi),
T-4 (D-6 kavram hatası), T-8 (D-2'nin kanonik biçimi), T-9 (43'ün gövdeye yayılması).

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

### E-1.7 ⛔ D-1 **örnek** olarak uygulandı, **sınıf** olarak uygulanmadı

D-1'in karar cümlesi: *"**yetki taşıyan tipler** `record` **olamaz**."* Bu bir sınıf
kuralıdır. Belge taraması (`grep`, aynı anlık görüntü):

| ADR bölümü | Tip | D-1 uygulandı mı |
|---|---|---|
| §4.1 `ToolAuthorization` | `sealed class` | ✅ |
| §4.2 `ToolName`, `AdapterId`, `ContextKey`, `OwnerName` | `readonly record struct` | ⚠️ (kimlik; yetki değil — savunulabilir) |
| §4.4 `DecayPolicy.Disabled(string Reason, **Identity Approver**)` | `sealed record` (satır 891) | ⛔ **HAYIR** |
| §4.6 `Measured` | `readonly record struct` | ⚠️ (sayı; ayrıca D-6 ile çelişik, E-3.4) |

`DecayPolicy.Disabled` bir **onaylayan kimliği** taşır — D-4'ün kendi gerekçesi onu
*"denetim damgası"* diye adlandırıyor. Ölçüm (§4.4'ün satır 891'deki imzası birebir
kopyalandı):

```
[K1] meşru:  Disabled { Reason = planlı bakım, Approver = Identity { Value = alice } }
[K2] `with`: Disabled { Reason = onaylandı,   Approver = Identity { Value = mallory } }
[K3] derleme hatası oluştu mu: HAYIR
```

Sönümü kapatma onayı, `with` tek satırıyla **başka birinin üzerine** yazılıyor.
`ENG-0001` E-1.2 bunu **önceden** uyarmıştı (*"ADR'nin K-2/K-4/K-6'da `record` seçmesi bu
riski üç karara daha yayıyor"*); v0.3.0 uyarıyı K-1'e uyguladı, **diğer üçüne uygulamadı**.

Bu, `AUDIT-WAVE2-SECURITY` §10.5'in meta-bulgusunun ADR'nin **kendi düzeltmesinde**
tekrarlanmasıdır: *"kusur örnek olarak kapatıldı, sınıf olarak kapatılmadı."*
ADR'nin §1'deki kendi tasarım ölçütü (*"bir karar, ancak unutmak derleme hatası üretiyorsa
sınıfı kapatır"*) burada **kendi aleyhine** işliyor: D-1'i K-4'te unutmak hiçbir tanı
üretmedi.

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

> **Bu bölümün sonucu bloke edicidir ve ADR'nin bir kavram hatasını gösterir.**

D-6 diyor ki: *"`class`. Ve Faz 0'da `<WarningsAsErrors>CS8618</WarningsAsErrors>` eklenir —
bu olmadan D-6 bir konvansiyondur, karar değil."* Bağlamı `default(T)` deliğidir.
Bu iddiayı ölçmek için `CS8618`'i **error** yapan bir proje kuruldu ve dört olgu ayrı ayrı
denendi.

### E-4.1 Ölçüm — `CS8618` yalnızca **kurucudan çıkışı** görüyor

| Olgu | Kaynak satırı | `<WarningsAsErrors>CS8618</WarningsAsErrors>` altında tanı |
|---|---|---|
| İlklendirilmemiş non-nullable **alan** (kurucu çıkışı) | `public sealed class HolderA { public string Field; public HolderA(){} }` | **`error CS8618`** ✅ |
| `default(SealedClass)` bir non-nullable değişkene | `Sealed1 b2 = default(Sealed1);` | **`warning CS8600`** — *farklı tanı*, **error değil** ⚠️ |
| `default(ReadonlyRecordStruct)` | `Struct1 c = default(Struct1);` | **hiçbir tanı yok** ⛔ |
| `new SealedClass[2]` / `new Struct1[2]` | dizi tahsisi | **hiçbir tanı yok** ⛔ |

Ham derleyici çıktısı:

```
Program.cs(7,59):  error   CS8618: Null atanamaz alan 'Field', oluşturucudan çıkış yaparken
                                   null olmayan bir değer içermelidir.
Program.cs(18,22): warning CS8600: Null sabit değeri veya olası null değeri, boş değer
                                   atanamaz türe dönüştürülüyor.
```

Çalışma-zamanı olguları (aynı derlemeden):

```
[N1] default(Sealed1) is null:        True
[N3] default(Struct1).Value = 0       (private ctor + Of ATLANDI)
[N4] new Sealed1[2][0] is null:       True
[N5] new Struct1[2][0].Value = 0
```

### E-4.2 ⛔ BLOKE EDİCİ — ADR iki farklı şeyi birleştiriyor

`CS8618`'in konusu **kurucu sözleşmesidir**: *"bu tipin bir örneğini kurarsan alanları
doldur."* `default(T)`'nin konusu ise **kurucunun hiç çalışmamasıdır**. İkincisi birincinin
kapsamına **girmez** — ölçüm bunu gösteriyor: `default(Struct1)` ve `new Sealed1[2]` için
`CS8618` hiç üretilmiyor, `default(Sealed1)` için ise üretilen tanı `CS8600`'dür ve
`WarningsAsErrors` listesinde **yoktur**.

Yani `<WarningsAsErrors>CS8618</WarningsAsErrors>`, D-6'nın kendi ölçütüne göre
(*"bu olmadan karar değil, konvansiyondur"*) `default(T)` için **hâlâ konvansiyondur**.
Karar, kapatmayı iddia ettiği deliği kapatmıyor.

### E-4.3 v0.3.0, `ENG-0001`'in talebini **daralttığı için** zayıflattı

`ENG-0001` T-1 `<WarningsAsErrors>**Nullable**</WarningsAsErrors>` istemişti. Fark ölçüldü:

```
WarningsAsErrors=Nullable  →  error CS8600  (satır 18)  +  error CS8618  (satır 7)
WarningsAsErrors=CS8618    →          CS8600 yalnız UYARI  +  error CS8618
```

`Nullable` **kategorisi**, `default(class)` atamasını da hataya çeviriyor; `CS8618` **tek
kodu** çevirmiyor. v0.3.0 talebi kabul ederken **kapsamını daralttı** ve tam olarak
`default(T)`'yi yakalayan kodu (`CS8600`) dışarıda bıraktı. Bu, düzeltmenin ters yönde
çalıştığı bir vakadır.

### E-4.4 `Nullable` bile yetmiyor — bir karakter iptal ediyor

```
Sealed1 b2 = default(Sealed1)!;    →  CS8600 sayısı: 0   (Nullable=error iken bile)
```

Null-forgiving operatörü (`!`) tanıyı **tamamen** susturuyor. Bugün `Ens.Kernel`'de
`!.` kullanımı **0** (ölçüldü) — yani sicil temiz — ama bu bir konvansiyondur, zorlama
değildir. Gerçek zorlama `!` kullanımını yasaklayan bir analyzer kuralıdır
(`IDE0031`/özel Roslyn analyzer), ve ADR'de yok.

### E-4.5 Bugünkü durum teyit edildi

`Ens.Kernel/Ens.Kernel.csproj` **7 satır** ve içeriği:
`TargetFramework`, `ImplicitUsings`, `Nullable`, `RootNamespace`. `WarningsAsErrors` /
`TreatWarningsAsErrors` **yok**. `ENG-0001` E-2.4'ün ölçümü hâlâ geçerli.

### E-4.6 D-6 VERDICT: **tutmadı**

| Alt iddia | Verdict |
|---|---|
| OQ1 `class` lehine kapatıldı | **kısmen** — E-3.4'e göre `Measured` için çelişkili |
| `CS8618` `default(T)`'yi kapatır | **tutmadı** — `CS8618` kurucu-çıkışı tanısıdır; `default(T)` çalışma-zamanı olgusudur |
| `WarningsAsErrors` kararı konvansiyonu karara çevirir | **tutmadı** — seçilen kod (`CS8618`) `default(class)`'ı yakalamıyor; `default(struct)`'ı **hiçbir kod** yakalamıyor |

> **Talep T-4 (bloke edici):** D-6 iki cümleye ayrılmalıdır.
> (1) *"İlklendirilmemiş alan"* için: `<WarningsAsErrors>**Nullable**</WarningsAsErrors>`
> (kategori, tek kod değil) — `CS8618` **ve** `CS8600` birlikte.
> (2) *"`default(T)` kurucuyu atlar"* için: **derleyici mekanizması yoktur.** `class`
> seçmek deliği `null`'a çevirir (fail-closed'a yakın), `struct` seçmek `0.0`'a çevirir
> (sessiz sahte-geçerli değer). Bu, kapatılan değil **kabul edilen** bir açıktır ve §5'e
> yazılmalıdır. Bugünkü metin onu kapanmış gösteriyor.

## 5. D-8 — 43 sayısı yeniden hesaplandı

`work-protocol.md` §3.5/4 gereği sayı **yeniden okunmadı, yeniden hesaplandı**:
`DEFECT-PATTERN-MAP.md` §1-§9'un tablo satırları tek tek sayıldı.

### E-5.1 Bağımsız üye sayımı

| Kalıp | Sayılan ID'ler | Sayım | Haritanın beyanı |
|---|---|---|---|
| DP1 | `E3 W4a W15 W16 H1 G5 C3 W2_L1 W2_L2 W2_O1 W2_R2 W5d` | **12** | 12 ✅ |
| DP2 | `F3 G3 G4 C2 W1a W1b W1c W2c W2e W2f W5g W7f W7h` | **13** | 13 ✅ |
| DP3 | `A1 A2 B4 D4 W2_L3 W2_R6` | **6** | 6 ✅ |
| DP4 | `A5 E4 G2 H3 W10` | **5** | 5 ✅ |
| DP6 | `W22 W2_R4 W2_L4 W5a W5b` | **5** | 5 ✅ |
| DP7 | `H4 W3 W17 W5e W8a W8b` | **6** | 6 ✅ |
| **Kapanabilir toplam** | | **47** | 47 ✅ |

Ayrıca **çakışma taraması**: kapanabilir altı kalıbın 47 ID'si arasında **hiçbir tekrar
yok** (`W2_L1/L2` DP1, `W2_L3` DP3, `W2_L4` DP6 gibi benzer görünenler ayrı ID'ler).
Yani 47, çifte sayım içermiyor.

### E-5.2 Çıkarma zinciri — dördü de gerçekten üye, hiçbiri iki kez çıkmıyor

| Çıkarılan | Üyesi olduğu kalıp | Doğrulandı |
|---|---|---|
| `C2` | DP2 (`§2. P2` satır 4) | ✅ tek kez |
| `W1b` | DP2 (satır 6) | ✅ tek kez |
| `W2_O1` | DP1 (satır 10) | ✅ tek kez |
| `W2c` | DP2 (satır 8) | ✅ tek kez |

`47 − 4 = 43`. **D-8'in aritmetiği doğrudur ve türetilebilir.** `SKR-049` T-A'nın işaret
ettiği çifte-çıkarma (`W1b`) tekrarlanmamış. Bu, v0.3.0'ın en temiz düzeltmesidir.

**Yan bulgu (43'ü etkilemez):** DP9 tablosunda **16** ID sayıyorum, beyan **15**. Sebep
`D1_residual`'ın hem DP8 hem DP9'da geçmesi. Harita bunu §10'da **kendisi açıklıyor**
(*"83 ≠ 84 — bir kimlik eksik ve bunu gizlemiyorum"*). Bağımsız sayımım o itirafı
doğruluyor; yeni bir kusur değil, kayıtlı bir borç.

### E-5.3 ⛔ Sayı düzeltildi ama belgeye **yayılmadı** — bugün DÖRT ayrı sayı var

> **Anlık görüntü uyarısı (`work-protocol.md` §3.5):** ADR bu inceleme sürerken değişti —
> paralel `SKR-050` turu §0.8'i (v0.3.1) ekledi, belge 1196 → **1275 satır** oldu.
> Aşağıdaki tüm satır numaraları **`git hash-object` = `4ad8e7e9529e5d66273ee16a09d6e8dc16c1a57f`**
> anlık görüntüsündendir ve **içerikle** teyit edilmiştir (numaraya değil metne güvenilmiştir).
> §1-§9'un spike ölçümleri bu değişimden **etkilenmez** — onlar C#/.NET davranışını ölçer.

| Sayı | Satırlar | Nerede |
|---|---|---|
| **43** | 19, 41, 74, 219, 228, 231, 233 | **yalnız künye + §0.7** |
| **41** | 331, 430, 492 | §1 Bağlam, §2.5, §3 |
| **40** | 450, 492, 494, 518, 607, 1211, 1214, 1216 | §2.5, §3 Kapsam, K-0 kutusu, §4, **§7 yanlışlanma bölümü** |
| **"40 koşulsuz + 4 koşullu"** | 20 (künye), 304 (§0.8) | `SKR-050`'nin yeniden hesabı |

Yani manşet sayı **dört farklı değerle** aynı belgede duruyor ve bunların üçü (`41`, `40`,
`40+4`) gövdededir; `43` yalnız changelog bölümündedir.

En ağırı **satır 1211**: ADR'nin `Failure conditions` bölümü hâlâ
*"İddia: K-1…K-6 uygulandığında **40** kimlik kapanır"* diyor. Yani **belgenin
yanlışlanma noktası, belgenin kararıyla çelişiyor.** Madde X'in istediği şey tam olarak
o bölümdür; orada duran sayı yanlışsa yanlışlanabilirlik disiplini biçimseldir.

Ayrıca **satır 331** (§1 Bağlam) hâlâ şunu yazıyor:
> *"altı mimari kararın **41**'ini birden kapatacağını iddia etti
> (P1+P2+P3+P4+P6+P7 = 12+13+6+5+5+6)"*

Parantez içi ifade **47** eder. `SKR-049` T-A'nın bulduğu hata, ADR'nin **kendi §1'inde
düzeltilmemiş** olarak duruyor; yalnız §0.7'de kaydedilmiş.

### E-5.4 ⛔ D-7 (`P` → `DP` yeniden adlandırma) **ilan edildi, uygulanmadı**

```
ADR icinde "DP1..DP9" gecisi:  8   (hepsi §0.7 icinde)
ADR icinde "P1..P9"  gecisi:  78   (P1:13 P2:13 P3:6 P4:6 P5:9 P6:6 P7:7 P8:9 P9:9)
```

D-7'nin gerekçesi *"künye `principles:` alanı gövdenin sözlüğüyle okununca §3'ün tersini
söylüyordu"* idi. Künye bugün hâlâ `principles: [P1, P5, P6, P7, P8]` taşıyor ve gövde
hâlâ 78 kez `P1..P9`'u **kalıp** anlamında kullanıyor. **Kararın çözdüğü belirsizlik hâlâ
yerinde.**

### E-5.5 43'ün **içeriği** — ölçüme göre en az iki üye hak edilmemiş

Aritmetik doğru, ama 43'ün içindeki iki kimlik bu incelemede **kapanmıyor** ölçüldü:

| ID | Nerede | Ölçüm |
|---|---|---|
| `W1a` | DP2, 43'ün içinde | §E-2.3 — varyant B'de test **yeşil kalıyor**; biçim reddi yetmiyor |
| `W1c` | DP2, 43'ün içinde | §E-2.3 — 9 InlineData'nın **2'si** hâlâ geçiyor (7/9) |
| `E3`,`W4a`,`W15`,`H1` | DP1, 43'ün içinde | §E-1.3/E-1.4 — düşmanca in-process çağıran modelinde kapanmıyor |

Bu, aritmetiğin değil **kapsam varsayımının** sorunudur: 43, "biçim kanonikleştirmesi +
varlık kontrolü" ve "iyi niyetli çağıran" varsayımlarına dayanıyor; ikisi de yazılı değil.

### E-5.6 D-8 VERDICT: **kısmen**

| Alt iddia | Verdict |
|---|---|
| `47` doğru | **düzeltme tuttu** — bağımsız sayımla doğrulandı |
| `43` türetilebilir, çifte çıkarma yok | **düzeltme tuttu** |
| Sayı belgeye yayıldı | **tutmadı** — 40 (9 yer) ve 41 (3 yer) hâlâ duruyor; §7 dâhil |
| D-7 rename uygulandı | **tutmadı** — 8 `DP` / 78 `P` |
| 43'ün üyeleri gerçekten kapanıyor | **kısmen** — en az `W1a`, `W1c` ve DP1'in dördü koşullu |

## 5A. D-3, D-4, D-7 — kısa ölçümler

### E-5A.1 D-3 (`W2c` kapsam dışı) — öncül doğru, **çıkarım yanlış**

Öncül bağımsız olarak yeniden ölçüldü (`ENG-0001`'in bulgusunu devralmadım):

```
CharUnicodeInfo public static üyeler: GetDecimalDigitValue, GetDigitValue,
                                      GetNumericValue, GetUnicodeCategory   (4, hiçbiri script)
Rune üyelerinde "Script" geçen: 0
StringInfo'da script API: 0
```

**Öncül doğru: BCL'de Script *property* API'si yok.** Ama D-3'ün çıkarımı
(*"o hâlde `W2c` .NET'te kapatılamaz"*) ölçümle çelişiyor:

```
[D3-temiz] "read_stock"     IsCyrillic=False  saf-ASCII=True
[D3-kiril] "reаd_stock" IsCyrillic=True   saf-ASCII=False
[D3-a] System.Text.Unicode.UnicodeRanges public üye sayısı: 162
[D3-c] MixedScript(temiz)=False   MixedScript(kiril)=True
```

`Regex` **named blocks** (`\p{IsCyrillic}`) ve `UnicodeRanges` (162 üye) BCL'de mevcut ve
`W2c`'nin **ölçülmüş gövdesini** (tek Kiril `а`, gerisi Basic Latin) yakalıyor. ADR'nin
kendi §4.2'si "mixed-script kısıtı"nı zaten adıyla yazmıştı; D-3 o yolu **aramadan**
kapsamdan çıkardı.

**Kendi deltam (bu ölçümün ötesi):** named-block yaklaşımı **aynı-script** homoglifini
yakalamıyor —

```
[D3-d] MixedScript("rIad_stock") = False   <- büyük 'I' Latin; yakalanmıyor
```

Dürüst konum: mixed-script kısıtı `W2c`'nin **gövdesini** kapatır, **homoglif sınıfını**
kapatmaz. Yani ne "kapatılabilir" ne "kapatılamaz" — **ölçülmüş gövde kapanır, sınıf açık
kalır** ve §5'e o ifadeyle yazılmalıdır.

**D-3 VERDICT: tutmadı** (karar doğru öncülden yanlış sonuç çıkarıyor).

### E-5A.2 D-4 (`At` kaldırıldı) — **düzeltme tuttu**

Gövde satırı doğrulandı (aynı anlık görüntü, satır 891):
`public sealed record Disabled(string Reason, Identity Approver) : DecayPolicy;`
`DateTimeOffset At` **yok**. `grep "record Disabled("` → tek eşleşme, `At` içermiyor.
K-3 ihlali (`W2_L3` kalıbı) gerçekten kalkmış. **v0.3.0'ın en temiz gövde düzeltmesi.**

(Uyarı: aynı tip `record` kaldığı için E-1.7'nin bulgusuna açıktır — ama bu D-4'ün değil
D-1'in kapsam sorunudur.)

### E-5A.3 D-7 (`P` → `DP`) — **tutmadı**

```
"DP1..DP9" geçişi:  8   (hepsi §0.7 içinde)
"P1..P9"   geçişi: 78   (P1:13 P2:13 P3:6 P4:6 P5:9 P6:6 P7:7 P8:9 P9:9)
```

Künye hâlâ `principles: [P1, P5, P6, P7, P8]`. D-7'nin çözdüğünü iddia ettiği çift-anlam
(*"künye §3'ün tersini söylüyor"*) **aynen yerinde**. Karar ilan edilmiş, uygulanmamış.

## 6. Yeni yüzey — factory, hata mesajı, sızıntı

### E-6.1 Factory taklit edilemiyor — ama `internal` yüzeyi ADR'nin kendi itirazına düşüyor

Ölçüm (`B4b`): sahte `AuthoritySeal` ile üretilen yetki **reddediliyor**. Factory
(`Issue`) `public` olsa bile mühürsüz üretim imkânsızdır — çünkü asıl sır factory değil,
**registry örneğinin private alanıdır**. Bu tasarım doğrudur.

Ama `Issue` ve `IssuedBy` ADR taslağında **`internal`**. ADR §4.1, `internal` seçeneğini
şöyle reddediyordu:

> *"`InternalsVisibleTo(Ens.Kernel.Tests)` zaten gerekli olacak → test assembly'si tam
> yetki üretebilir hâle gelir."*

Ölçüm: bugün `Ens.Kernel` assembly'sinde `InternalsVisibleTo` **0** adet. Ama kernel+test
ağacında **`new ToolAuthorization(` 7 çağrı yeri** var ve **ikisi test dosyasında**
(`AdversarialWave_SchedulerGateTests.cs`, `AdversarialWave_SecurityTests.cs`). K-1
uygulandığında bu çağrı yerleri ya `InternalsVisibleTo` ister — **ADR'nin kendi reddettiği
durum** — ya da testler registry üzerinden geçmek zorunda kalır, ki o zaman
`AUDIT_DEFECT_W4a` (*"sahte `ToolAuthorization` registry'yi atlıyor"*) testi **artık
yazılamaz** ve kimliğin regresyon bekçisi kaybolur.

> **Talep T-5:** K-1 taslağı, test assembly'sinin yetki üretme kapasitesini nasıl
> sınırlayacağını yazmalıdır. En dar çözüm: `Issue`'yu `private` yapmak ve testlerin
> saldırıyı **`UnsafeAccessor`/reflection ile** kurması — böylece "sahte yetki üretmek
> ayrıcalık gerektirir" iddiası testin kendisinde görünür kalır.

### E-6.2 ⛔ YENİ BULGU — `Register` bir **varlık oracle'ı**, ve sorgunun kendisi yetki veriyor

Ölçüldü (gerçek `Ens.Kernel` kopyası, değiştirilmemiş):

```
r.Register(new CapabilityPack("gizli-finans-paketi", "0.0", ["x"]))
  -> 'gizli-finans-paketi' zaten kayıtlı (kayıtlı: v3.7.1, gelen: v0.0).
r.Register(new CapabilityPack("yok-boyle-bir-pack",  "0.0", ["x"]))
  -> hata YOK
```

İki ayırt edilebilir sonuç = **enumerasyon oracle'ı**. Ayrıca hata mesajı kayıtlı
**versiyonu** (`v3.7.1`) sızdırıyor — saldırgan tahmin etmedi, sistem söyledi.

**Asıl ağır kısım yan etkidir:** başarısız tahmin (`yok-boyle-bir-pack`) sessizce
**kaydoluyor** ve `"x"` aracını **yetkilendiriyor**. `AUDIT_HOLDS_W1g` ölçtü ki
`Unregister`/`Remove`/`Clear` **yok** — yani her sorgu **kalıcı** bir kayıt bırakıyor.
Sorgulama edimi, sorgulananı yaratıyor.

Bu kimlik `DEFECT-PATTERN-MAP`'in **hiçbir kalıbında yok** ve 43'ün içinde de yok.
`W1e` (sınırsız `_disabled` büyümesi) bunun **zayıf** kardeşidir; bu ise `_packs`'i
büyütüyor **ve yetki veriyor**.

### E-6.3 `Authorize` `Reason` alanı pack adı + versiyon sızdırıyor

```
'read_stock'    izinli ('Ops' v1.0), onay gerekmiyor.
'wire_transfer' izinli ('gizli-finans-paketi' v3.7.1) ancak 'gizli-finans-paketi' v3.7.1
                insan onayı şart koşuyor (P7).
'delete_everything' hiçbir etkin Capability Pack tarafından izinli değil.
```

Yetkisi olmayan bir çağıran, izinli **olduğu** her araç için hangi Pack'in ve hangi
versiyonun yetki verdiğini öğreniyor. `Disable` edilmiş bir Pack'in kısıt cümlesi
(*"bu Pack DEVRE DIŞI olsa da kısıt korunur"*) ayrıca **iç registry durumunu** açıklıyor.

Bu, kasıtlı bir tasarımdır (`W2d`'nin bulduğu "insana gösterilen `reason`") ve tek başına
kusur değildir — ama **D-2'nin reddetme kararı bunu genişletiyor**: ret mesajı hem
reddedilen ham girdiyi hem reddetme sebebini taşıyacak. Spike'ta ürettiğim mesaj:

```
'packName' kanonik degil (yasak karakter U+202E).
```

Bu mesaj çağıranın kendi girdisini yansıtıyor (düşük risk), ama **D-2'nin gerektirdiği
varlık kontrolü** (§E-2.3) eklendiğinde mesaj *"kayıtlı değil"* diyecek — ki bu, E-6.2'nin
oracle'ını **birinci sınıf API** hâline getirir. D-2'nin gerekçesi bunu bir **erdem** olarak
sunuyor (*"sorgu artık 'bulunamadı' der"*); ölçüm, aynı özelliğin bir **enumerasyon
yüzeyi** olduğunu gösteriyor. İkisi aynı madalyondur ve ADR yalnız bir yüzünü yazıyor.

> **Talep T-6:** D-2'ye hata-mesajı politikası eklenmelidir. En muhafazakâr biçim:
> *biçim* hatası ayrıntılı (çağıranın kendi girdisi), *varlık* hatası **ayrıntısız**
> (tek tip "işlem reddedildi") ve ayrıntı yalnız denetim izine yazılır. Aksi hâlde
> fail-safe kararı bir keşif aracına dönüşür.

## 7. Maliyet — 17/17 tablosu ve taban hâlâ geçerli mi?

### E-7.1 Taban değişmedi (yeniden ölçüldü)

```
Ens.Kernel/        17 dosya   2421 ham satır
Ens.Kernel.Tests/             6851 ham satır   373 test
dotnet test        373/373 geçiyor, 306 ms
```

`ENG-0001` E-7.1'in her sayısı **aynı**. v0.2.0 ve v0.3.0 metin sürümleridir; kernel'e
dokunulmamıştır. Bu, Madde VII'nin (`draft` ADR referans implementasyonu bağlamaz)
**ölçülmüş** teyididir — ve ADR'nin bu turdaki en sessiz erdemidir.

### E-7.2 17/17 tablosu **değişmedi** — ve bir karar onu genişletiyor

v0.3.0'ın beş karar değişikliğinin dosya birleşimine etkisi:

| Değişiklik | 17/17 tablosuna etkisi |
|---|---|
| D-1 `record` → `sealed class` | **Yok** — aynı 6 dosya, farklı tip anahtar sözcüğü |
| D-2 katlama → reddetme | **Yok** dosya sayısında; **ama** `Disable`/`Enable`/`IsEnabled` artık **atan** metotlar → `CapabilityRegistry.cs`'de K-2'nin yüzeyi genişliyor |
| D-3 `W2c` kapsam dışı | **Daraltıyor** — homoglyph tablosu/veri kümesi gerekmiyor |
| D-4 `At` parametresi kalktı | **Daraltıyor** — K-4'ün `Disabled` imzası sadeleşti |
| D-5 `IComparable` | **Genişletiyor** — `Measured`'ı **sıralayan** her yer artık davranış değiştiriyor: `Scheduler.cs`, `Domain/CompanyMemory.cs` (ölçüldü: 2 yer) |
| D-6 `class` + `WarningsAsErrors` | **`Ens.Kernel.csproj`'u tabloya EKLİYOR** — 17 → **18 dosya** |

> **Ölçülmüş sonuç:** `ENG-0001`'in *"17/17 üretim dosyası, %100"* tablosu geçerliliğini
> koruyor; D-6 ona **`Ens.Kernel.csproj`'u** ekliyor. "ADR-0001'i sertleştirir, değiştirmez"
> beyanı bu tabloyla hâlâ gerilim hâlindedir: dokunulmayan tek dosya yok.

### E-7.3 `ENG-0001` T-7 **kapatılmadı**

`AdversarialWave_SecurityTests.cs` (51 test, tabanın %13.7'si) ADR'de **7 kez** geçiyor —
ama **altı maliyet tablosunun hiçbirinde geçmiyor** (`grep`: `#### Maliyet` başlıklarından
sonraki "Etkilenen test" satırlarında **0** eşleşme). K-1'in tablosu 7 test dosyası sayıyor;
o dosya aralarında yok.

Bu, `ENG-0001` T-7 ve `SKR-049`'un aynı bulgusudur ve **v0.3.0'da kapatılmamıştır**.
Şiddeti şudur: ADR'nin 43 sayısının **doğrulanacağı** dosya, maliyetinin **sayılmadığı**
dosyadır. Bu incelemede D-2'yi ölçtüğümde varyant B'nin 20 kırılmasının **19'u** o
dosyadaydı (kalan 1: `AUDIT_HOLDS_F1` → `AdversarialAuditTests.cs`; her test dosya
konumu `grep -arl` ile teyit edildi). Yani D-2'nin maliyetinin **%95'i**, ADR'nin K-2
maliyet tablosunda **hiç görünmeyen** bir dosyada.

## 8. D-9 — uygulama sırası tutarlı mı?

### E-8.1 Sıra **birebir aynı** — ama bir uyarı düşürülmüş

| | `ENG-0001` E-9.2 | ADR D-9 |
|---|---|---|
| Faz 0 | OQ1 + `WarningsAsErrors` + mimari tarama | Faz 0 (OQ1 + `WarningsAsErrors` + mimari tarama) |
| 1-5 | K-5 → K-6 → K-2 → K-3+K-4 → K-1 | K-5 → K-6 → K-2 → K-3+K-4 birlikte → K-1 |

**Aynıdır.** D-9 kaynağını da doğru gösteriyor (*"ölçülmüş bağımlılık grafiğinden"*). Bu,
v0.3.0'ın ikinci temiz devralmasıdır.

Düşürülen: `ENG-0001` E-9.2'nin Faz 0 satırı `<WarningsAsErrors>**Nullable**</WarningsAsErrors>`
diyordu; D-9 onu `CS8618`'e daralttı. §E-4.3 bunun **zayıflatma** olduğunu ölçtü. Ayrıca
`ENG-0001`'in kapanış gözlemi — *"kararlar paralel uygulanamaz, her faz arasında yeşil test
kapısı zorunludur"* — D-9'a **taşınmamış**. D-9 bir sıra veriyor, ama **faz kapısı** vermiyor.

### E-8.2 Sıra, v0.3.0'ın kendi değişiklikleriyle hâlâ tutarlı mı? — İki gerilim

**(a) D-6 ↔ Faz 0.** D-6 `class` diyor; §E-3.4'te ölçtüm ki `Measured` için `class`
sıcak yolda 100k adımda **2.3 MB** tahsis ediyor ve `null`'ı **sessizce** sıralıyor.
Faz 0, "OQ1'i kapat" adımını **tek cevapla** kapatıyor. Ölçüm karar-başına ayrışma
gerektiriyor → **Faz 0 bugünkü hâliyle yanlış bir kararı dondurur** ve K-6 (Faz 2) o
kararı geri almak zorunda kalır. Sıra doğru, **Faz 0'ın içeriği** yanlış.

**(b) D-2 ↔ Faz 3 (K-2).** D-2 reddetme kararını verdi ama "kanonik biçim"i tanımlamadı
(§E-2.2). Ölçüm: kırılan test sayısı **20 ↔ 52**. `ENG-0001` E-9.3'ün yanlışlanma koşulu
*"K-5'ten sonra 20'den fazla test kırılırsa sıra çürür"* idi. K-2 için karşılığı yazılmamış.
Varyant A seçilirse tek fazda **52** test kırılır ve bunların **6'sı `AUDIT_FIXED_*`
regresyon bekçisidir** — yani kapanmış kusurların kanıtı kaybolur.

> **Talep T-7:** D-9'a faz kapısı eklenmelidir: *"her fazın sonunda `dotnet test` yeşil;
> bir `AUDIT_FIXED_*` testi kırılırsa faz **geri alınır**, kırmızı bırakılmaz."* Ve K-2
> fazı için `ENG-0001` E-9.3'ün eşdeğeri yazılmalıdır: kırılan test sayısı **ve** kırılan
> `AUDIT_FIXED_*` sayısı önceden ilan edilmeli, koşumla karşılaştırılmalıdır.

### E-8.3 D-9 VERDICT: **kısmen**

| Alt iddia | Verdict |
|---|---|
| Sıra ölçülmüş bağımlılıkla tutarlı | **düzeltme tuttu** — `ENG-0001` ile birebir |
| Faz 0'ın içeriği doğru | **tutmadı** — `CS8618` daraltması (E-4.3) + tek-cevap OQ1 (E-3.4) |
| Faz kapısı tanımlı | **tutmadı** — "paralel uygulanamaz + yeşil kapı" uyarısı taşınmamış |

## 9. Katıldığım noktalar

Ölçüm, yalnız kırılanı değil tutanı da raporlamak zorundadır; aksi hâlde kendisi kalibre
değildir.

1. **`ENG-0001`'in bulguları gerçekten karara dönüştü.** Sekiz bulgunun sekizi §0.7'de
   kararlaştırılmış; hiçbiri sessizce düşürülmemiş. `D-3` gibi *aleyhte* bir sonuç bile
   (kapsam daraltma) açıkça yazılmış. Bu, v0.2.0'ın *"bulguyu kaydetmek ≠ kararı
   değiştirmek"* ayrımının **çalıştığının** kanıtıdır.

2. **D-8'in aritmetiği doğru ve bağımsız olarak yeniden üretildi.** `47` ve `47−4=43`
   satır satır sayarak doğrulandı; `SKR-049` T-A'nın çifte-çıkarma hatası **tekrarlanmamış**.
   Bu, `work-protocol.md` §3.5'e eklenen dördüncü kontrolün (*"sayıyı yeniden hesapla"*)
   ilk gerçek getirisidir — ve bu incelemede aynı kuralı ADR'ye uygularken **aynı sonucu**
   verdi.

3. **D-4 temiz uygulandı.** Gövde satırı doğrulandı. Bir kararın gövdeye gerçekten işlendiği
   nadir vakalardan biri.

4. **D-9 icat edilmedi, devralındı ve kaynağı gösterildi.** ADR sırayı *"`ENG-0001`'in
   ölçülmüş bağımlılık grafiğinden"* diye niteliyor — ve gerçekten öyle. Devralınan bir
   sonucun kaynağını göstermek Madde VIII yükümlülüğüdür; burada yerine getirilmiş.

5. **Madde VII fiilen korunuyor.** `Ens.Kernel` üç sürüm boyunca **dokunulmadı**:
   17 dosya / 2421 satır / **373/373 yeşil**. `draft` bir ADR'nin koda sızmadığı,
   iddiayla değil **koşumla** gösterildi.

6. **D-1'in brand mekanizması, sahte mühüre karşı çalışıyor.** `B4b` ölçümü: private
   kurucuya reflection'la ulaşmak yetmiyor; saldırganın registry'nin **gerçek** mühür
   nesnesine erişmesi gerekiyor. Morris (1973) / Miller (2006) atfı bu noktada
   **hak edilmiş** — desen dekoratif değil, işlevsel.

7. **D-5, ölçülemez değeri tipe sokmuyor.** `Of` `NaN`/`±∞`'u reddediyor ve `default(T)`
   yolu `0.0` üretiyor; yani `IComparable` eklemek sıralamayı **tanımsız yapmıyor**.
   Görevin işaret ettiği risk ölçüldü ve **gerçekleşmedi** — bu, kararın lehinedir.

8. **ADR kendi eksiğini §0.8'de kendisi kaydetti.** Bu inceleme sürerken belgeye eklenen
   *"9 karardan 7'si gövdeye uygulanmadı — kontrol edilmedi"* itirafı, benim §E-5.3/E-5A.3
   ölçümlerimle **bağımsız olarak yakınsıyor**. Yazarın kendi aleyhine yazması, kural
   katmanının çalıştığının göstergesidir.

## 10. Talepler

Şiddet sırasına göre. `ENG-0001`'in numaralandırmasıyla çakışmaması için `T-1…T-9`
bu kayda özeldir.

### Bloke edici

- **T-1 — D-1'in güven sınırı iddiası geri çekilir.** §4.1'in *"burada güven sınırı
  **çağrı grafiğidir**"* cümlesi ölçümle çelişiyor (`B8`/`B9`: reflection'sız, `unsafe`
  bloksuz mühür kopyalama ve yerinde mutasyon). Yerine: *"güven sınırı **process**'tir;
  K-1 düşmanca in-process çağırana karşı değil, **kaza ve yanlış-çağrıya** karşı korur."*
  `E3`/`W4a`/`W15`/`H1`'in "kapandı" hanesi bu ifadeye bağlanmalıdır.

- **T-2 — yanlışlanma ölçütü düzeltilir.** *"`AUDIT_DEFECT_*` kırmızıya döner"* yetersiz;
  ölçüldü ki test **kurulum kırıldığı için** de kırmızıya dönüyor (§E-2.6, yığın izi
  `Register` satırını gösteriyor). Ölçüt: *"test `AUDIT_FIXED_*` olarak yeniden yazıldı,
  **aynı saldırı satırına ulaşıyor** ve ters iddiayı doğruluyor."*

- **T-3 — D-5 ↔ D-6 çelişkisi kapatılır.** D-6 `class`, §4.6 `readonly record struct`.
  Ölçülmüş fark: `class` → sıcak yolda 100k adımda **2.3 MB** tahsis + `null`'ın **sessizce**
  sıralanması. Karar **karar-başına** ayrıştırılmalıdır (`ENG-0001` E-2.4'ün talebi).

- **T-4 — D-6 iki cümleye ayrılır.** (a) İlklendirilmemiş alan →
  `<WarningsAsErrors>**Nullable**</WarningsAsErrors>` (kategori; `CS8600` **ve** `CS8618`).
  (b) `default(T)` → **derleyici mekanizması yoktur**; bu kapatılan değil **kabul edilen**
  bir açıktır ve §5'e yazılmalıdır. Ek: `!` operatörü `CS8600`'ü tek karakterde susturuyor
  (ölçüldü) — zorlama isteniyorsa `!` yasağı bir analyzer kuralı olmalıdır.

- **T-8 — D-2'nin "kanonik biçim"i tanımlanır.** Tanımsız bırakmak maliyeti **20 ↔ 52**
  arasında salındırıyor ve varyant A **6 `AUDIT_FIXED_*` regresyon bekçisini** kırıyor.
  Ayrıca D-2, biçim reddine ek olarak bir **varlık kontrolü** kararı içermelidir —
  `W1a`/`W1b`/`W1c`/`W1e` biçim reddiyle kapanmıyor (ölçüldü).

- **T-9 — 43 gövdeye yayılır ya da geri çekilir.** Belge bugün **dört** sayı taşıyor
  (43 / 41 / 40 / "40+4"). En ağırı: **§7 `Failure conditions` hâlâ 40 diyor** — yanlışlanma
  noktası kararla çelişiyor.

### Yüksek

- **T-5 — K-1'in test yüzeyi kararlaştırılır.** `internal Issue`, `InternalsVisibleTo`
  gerektirirse ADR'nin `internal` seçeneğine kendi itirazı geçerli olur; gerektirmezse
  `AUDIT_DEFECT_W4a` **yazılamaz** hâle gelir ve regresyon bekçisi kaybolur.
  Bugün `InternalsVisibleTo` **0**, `new ToolAuthorization(` **7** çağrı yeri (2'si testte).

- **T-6 — reddetme mesajı politikası.** Ölçüldü: `Register` çift-kayıt hatası kayıtlı
  **versiyonu** sızdırıyor, başarısız tahmin ise Pack'i **kalıcı olarak kaydediyor ve
  aracını yetkilendiriyor** (`Unregister` yok). *Biçim* hatası ayrıntılı, *varlık* hatası
  **ayrıntısız**; ayrıntı yalnız denetim izine.

- **T-7 — D-9'a faz kapısı.** *"Her faz sonunda `dotnet test` yeşil; bir `AUDIT_FIXED_*`
  kırılırsa faz **geri alınır**."* Ve K-2 fazı için `ENG-0001` E-9.3'ün eşdeğeri
  (beklenen kırılma sayısı önceden ilan edilir).

### Orta

- **T-A1** — D-1 kuralı **süpürülür**: `DecayPolicy.Disabled(Reason, **Approver**)`
  `record` olarak duruyor ve `with` ile onaylayan değiştirilebiliyor (ölçüldü, `[K2]`).
- **T-A2** — `Measured`'a non-generic `IComparable` eklenir (tek satır; `[M9]`/`[M10]`).
- **T-A3** — D-7 gövdeye uygulanır (78 `P` → `DP`) ve künye `principles:` alanı düzeltilir.
- **T-A4** — `AdversarialWave_SecurityTests.cs` altı maliyet tablosuna eklenir
  (`ENG-0001` T-7 kapanmadı; D-2 maliyetinin **%95'i** o dosyada).
- **T-A5** — D-3 §5'e *"ölçülmüş gövde kapanabilir (`\p{IsCyrillic}` + `UnicodeRanges`),
  homoglif **sınıfı** açık kalır"* biçiminde yazılır; "mekanizma yok" ifadesi yanlıştır.

## 11. Ölçülemeyenler (DOĞRULANMADI)

`work-protocol.md` §3'ün ölüm-kalım kuralı gereği, ölçemediğimi uydurmuyorum.

1. **`Unsafe.As` saldırısının alan-düzeni bağımlılığı.** `[B8]`/`[B9]` CoreCLR
   `net10.0.101` x64'te çalıştı. CLR sınıf alan düzeni **spesifikasyonla garanti edilmez**;
   farklı runtime/mimaride sonuç değişebilir. Saldırının **taşınabilirliğini ölçmedim** —
   ama bir saldırının kırılgan olması onu geçersiz kılmaz.
2. **`AppDomain.GetAssemblies()` taraması** yalnız **yüklü** assembly'leri sayar; "BCL'de
   hiçbir Script tipi yok" iddiam `CharUnicodeInfo`/`Rune`/`StringInfo` için **doğrudan**,
   BCL geneli için **eksiksiz değildir**.
3. **`class` `Measured`'ın gerçek kernel'deki tahsis maliyeti.** `[C4]`'ün 2.3 MB'ı **sentetik**
   döngüdendir; `Scheduler`/`ContextScore` sıcak yolunda ölçmedim (kernel'e dokunmam
   yasaktı). Büyüklük mertebesi göstergedir, ölçüm değildir.
4. **Varyant A/B dışında üçüncü bir kanonik biçim** (ör. PRECIS `UsernameCasePreserved`)
   denenmedi. 20 ↔ 52 aralığı **iki nokta**dır, tüm uzay değil.
5. **`AdversarialWave_SecurityTests.cs`'in 4 NUL baytı** `grep -a` ile aşıldı (görev
   uyarısı doğruydu), ama dosyanın **tamamının** doğru okunduğunu ayrıca doğrulamadım;
   test adları ve `InlineData` içerikleri `tr -d '\000'` sonrası okundu.
6. **`SKR-050`'nin bulguları — bağımsızlık beyanının tam hâli.** Paralel tur bu inceleme
   sürerken ADR'ye §0.8'i ekledi. Kronoloji dürüstçe: §1 (D-1), §2 (D-2), §3 (D-5),
   §4 (D-6), §5 (D-8), §6, §7, §8 bölümlerinin **tamamı §0.8'i görmeden** ölçüldü ve
   yazıldı. §0.8'i, D-3/D-4 kontrolünü çalıştırdığım komutta fark ettim; o iki ölçümün
   spike'ı da aynı komutta, okumadan önce kurulmuştu. Buna rağmen §5A'nın *yazımı*
   §0.8'i gördükten sonradır ve bu bir bağımsızlık zayıflamasıdır — kaydediyorum.
   `SKR-050`'nin kendi kaydını **doğrulamadım**; o ayrı bir §3.5 işidir.
7. **Bu, `ENG-0001` ile aynı roldür (`ens-backend-architect`).** GOV-000 **G4** *farklı
   boyutlardan* ≥2 bağımsız validator ister; aynı rolün ikinci turu G4'ü **karşılamaz**.
   Bu kayıt Engineering boyutunun **ikinci ölçümüdür**, ikinci boyut değildir.
