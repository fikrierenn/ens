---
id:            ENG-0001
title:         ADR-0003 — mühendislik uygulanabilirliği incelemesi
type:          review
review_of:     ADR-0003
dimension:     engineering (eng)
status:        in-progress
owner:         ens-backend-architect
version:       0.1.0
last_reviewed: 2026-07-27
independence:  G4 — ens-skeptic'in bilimsel turundan (SKR-049) ayrı boyut; yazar ADR yazarı değildir
method:        derlenen spike + dotnet test ölçümü + kaynak sayımı
---

# ENG-0001 — ADR-0003 Mühendislik Uygulanabilirliği

> **Bağımsızlık beyanı (GOV-000 G4).** ADR-0003'ü `ens-ai-architect` yazdı. Bu inceleme
> `ens-backend-architect` rolüyle, ayrı context'te, **mühendislik** boyutundan yapıldı.
> Paralel yürüyen `ens-skeptic` turu (SKR-049) *bilimsel* boyuttur; bu belge onun bulgularını
> ne bilir ne varsayar.
>
> **Lens farkı:** skeptic *"iddia doğru mu"* sorar. Bu inceleme *"C#'ta gerçekten böyle mi,
> ve ne kadara mal olur"* sorar. Tartışılan her şey **ölçüldü**; ölçülemeyen her şey
> `DOĞRULANMADI` etiketiyle işaretlendi (SKR-041 emsali).

**Durum: TASLAK — doldurulma sırasında.** Bölümler tek tek yazılıyor.

---

## 0. Verdict tablosu

*(en sonda doldurulacak)*

---

## 1. Ölçüm ortamı

| Öge | Değer | Nasıl ölçüldü |
|---|---|---|
| SDK | **10.0.101** | `dotnet --version` |
| TFM | `net10.0` | `Ens.Kernel/obj/Debug/net10.0/` |
| Test koşucu | VSTest 18.0.1 (x64) | `dotnet test` çıktısı |
| Spike konumu | `D:\Temp\claude\...\scratchpad\spike\` (`FakeKernel` classlib + `Spike1` console) | ENS deposuna **hiç dokunulmadı** |

Spike'ın **iki assembly** olması kasıtlıdır: K-1'in güven sınırı ADR'ye göre "çağrı grafiği"dir,
ama C#'ta erişilebilirlik assembly sınırından okunur. Tek assembly'de yapılan bir spike
`internal` kaçağını gizler.

## 2. Taban ölçüm — mevcut kernel ve test tabanı

```
dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj
→ Başarısız: 0, Başarılı: 373, Atlanan: 0, Toplam: 373, Süre: 217 ms
```

**373/373 doğrulandı** (ADR §4.0'ın "owner tarafından çalıştırıldı" beyanı bu turda bağımsız
olarak teyit edildi). İki xUnit analiz uyarısı var (`xUnit2012`, `xUnit2031`), hata yok.

Üretim yüzeyi: `Ens.Kernel/` altında **15 kaynak dosyası** (obj/ hariç). ADR'nin altı kararı
bu 15 dosyanın **13'üne** dokunuyor (§9'daki birleşim ölçümü) — yani kernel'in %87'si.
Bu, ADR'nin kendi karar-başına maliyet tablolarının **birleşimini** hiçbir yerde vermediği
için görünmeyen bir sayıdır.

## 3. K-1 — sealer/brand deseni: spike ile kırma denemesi

ADR §4.1'in kod iskeleti **birebir** kopyalandı (`FakeKernel/Auth.cs`), tüketici ayrı
assembly'den (`Spike1`) saldırdı. Sonuçlar `dotnet run` çıktısıdır, tahmin değildir.

### E-1.1 ⛔ BLOKE EDİCİ — `record` + `init` = mühür korunarak yetki yükseltme

ADR iskeleti `public sealed record ToolAuthorization` diyor. Bir `record`'un **`<Clone>$`
copy-constructor'ı `public`tir** — `private` kurucu onu kapatmaz. Sonuç:

```
[0]  mesru yetki kabul edildi mi: True   seal=33826822
[A2] `with`: Tool=wire_transfer Scope=9999 seal=33826822 KABUL=True
```

**Bir saldırgan meşru bir `ToolAuthorization` elde ettiği anda, mührü koruyarak payload'u
istediği gibi değiştirebiliyor.** `read_stock` yetkisi tek satırda `wire_transfer`/`scope 9999`
oluyor ve registry onu **kabul ediyor**. Bu tam olarak `W4a`'nın (sahte yetki) ve `W15`'in
(reddi aklama) yeniden doğuşudur — K-1'in kapattığını iddia ettiği iki kimlik.

Mekanizma: `with` ifadesi `<Clone>$()` çağırır, o da **tüm alanları** (private `_issuer` dahil)
kopyalar, sonra object-initializer `init` property'lerini ezer. Mühür yolculuk eder, payload
değişir. `private` kurucu bu yolu **hiç görmez**.

### E-1.2 Kapatan varyantlar — ölçüldü

| Varyant | `with` payload değiştirebiliyor mu | Kaynak |
|---|---|---|
| `record` + `init` property | **EVET — kırık** | `[A2]` |
| `record` + get-only property | Hayır — `error CS0200` derleme hatası; boş `with { }` mührü kopyalar ama zararsız | `[A6]` + negatif derleme |
| `sealed class` (record değil) | Hayır — `with` ifadesi dile göre kullanılamaz | `[A7]` |

**Mühendislik sonucu: K-1 `record` ile uygulanamaz — ya `sealed class` olmalı, ya da tüm
property'ler get-only olmalı ve bu `record`'un tüm ergonomik faydasını (`with`) yok eder.**
ADR'nin K-2'de `readonly record struct` (§4.2), K-4'te `abstract record` (§4.4) ve K-6'da
`readonly record struct` (§4.6) seçmesi bu riski **üç karara daha** yayıyor: her `init`
taşıyan record, kendi değişmezini `with` ile atlatılabilir kılar.

### E-1.3 Sağlam çıkan saldırılar (K-1 lehine)

```
[A3] GetUninitializedObject: seal=NULL  KABUL=False     → fail-closed, doğru
[A4] sahte AuthoritySeal üretilebiliyor ama referans farklı → ReferenceEquals reddediyor
[A8] reg2, reg1'in yetkisini kabul eder mi: False        → R2 fail-closed, doğru
[A9] farklı registry + aynı payload eşit mi: False       → record eşitliği private mühür alanını KAPSIYOR
```

`RuntimeHelpers.GetUninitializedObject` **taklit üretmiyor** — kurucuyu atlıyor, ama `_issuer`
`null` kalıyor ve `ReferenceEquals(null, seal)` `false` dönüyor. ADR'nin bu noktadaki
sezgisi (R3: "reflection ayrı bir kapsam") **doğru**: uninitialized-object tek başına yetmez,
saldırganın ayrıca alan yazması gerekir ki bu tam anlamıyla P5'tir.

`[A9]`'un ikinci satırı ADR'de hiç konuşulmamış bir **artı** bulgudur: record'un sentezlenmiş
`Equals`'ı private `_issuer` alanını da karşılaştırıyor, yani iki farklı registry'nin aynı
payload'lu yetkileri değer-eşit **değil**. `C2`'nin (entity/value karışımı) burada tekrar
etmesi engellenmiş oluyor — ama bu bir tasarım kararı değil, `record`'un yan etkisidir ve
E-1.2 gereği `record` bırakılırsa **kaybolur**.

## 4. OQ1 — `struct` vs `class`, `default(T)` deliği

ADR OQ1'i **açık soru** bırakıyor ve R14/R15/R20'yi "üç kararın ortak açığı" diye tanımlıyor.
Bu bölüm soruyu **ölçerek kapatıyor** — cevap tek ve nettir.

### E-2.1 `default(T)` kurucuyu gerçekten atlıyor; `required` bunu KAPATMIYOR

```
[S1] default(DecayRate).Value = 0                → private ctor + Of() fabrikası ATLANDI
[S2] required uyeli struct: default(T).Value = 0  → required deligi KAPATMIYOR
[S3] new DecayRate[3][0].Value = 0                → dizi tahsisi de atlıyor
[S4] sınıf alanı ilklendirilmemiş: 0              → uyarı yok, hata yok
```

**`required` (C# 11+) `default(T)`'yi engellemez** — çünkü `required` yalnızca *object creation
expression*'ı (`new T { ... }`) bağlar; `default(T)`, dizi tahsisi ve ilklendirilmemiş alan
bunun dışındadır. Bu ölçüldü, tahmin edilmedi. .NET 10 / C# 14'te "non-defaultable struct"
diye bir mekanizma **yoktur**; `DOĞRULANMADI` demeye gerek yok, `[S2]` doğrudan gösteriyor.

### E-2.2 ADR'nin önerdiği `IsValid` bayrağı kısmen çalışıyor — ve K-4'ün kendi ölçütünü çürütüyor

```
[S5] flagged.Get() PATLADI: InvalidOperationException  → bayrak çalışıyor
     ...AMA doğrudan .Value hâlâ 0 döndürüyor          → kaçak açık
```

Bayrak yalnızca **`Get()` çağrıldığında** çalışıyor. `.Value` property'si hâlâ sessizce `0`
veriyor. Yani koruma, ADR'nin kendi ölçütüne göre (§1: *"bir karar, ancak unutmak derleme
hatası üretiyorsa sınıfı kapatır"*) **bir çağrı-yeri sayımıdır** — `Guard.cs`'in sekizinci-nokta
hikâyesinin birebir tekrarı. ADR bunu R14'te zaten sezmiş (*"bu yine bir çağrı-yeri sayımıdır"*);
ölçüm o sezgiyi **doğruluyor**.

### E-2.3 `default(ImmutableArray<T>)` — R15 doğrulandı, ve K-5'in mimari testi yetmez

```
[S6] default(ImmutableArray<int>).Length → NullReferenceException
[S7] IsDefault = True, IsDefaultOrEmpty = True
[S8] foreach → NullReferenceException
```

R15 **gerçektir**. ADR'nin azaltması (*"alanlar `= ImmutableArray<T>.Empty` ile ilklendirilir;
mimari test bunu da tarar"*) mühendislik olarak **yetersizdir**: mimari test alan
*bildirimlerini* tarayabilir, ama `default`, dizi tahsisi ve `struct` içinde taşınan
`ImmutableArray` alanlarını tarayamaz. Reflection tabanlı bir assembly taraması bir alanın
*çalışma zamanında* `IsDefault` olup olmadığını göremez.

### E-2.4 `class` varyantı — derleyici gerçekten zorluyor (ölçüldü)

```
warning CS8618: Null atanamaz alan 'Rate', oluşturucudan çıkış yaparken
                null olmayan bir değer içermelidir.
```

`sealed class` kullanıldığında `default` = `null` olur ve **nullable analizi CS8618 üretir**.
`struct` varyantında **hiçbir tanı üretilmedi**. Bu, OQ1'in cevabıdır:

> **OQ1 CEVABI (mühendislik): `class`.** `struct`, `default(T)` deliğini hiçbir C# mekanizmasıyla
> kapatamaz; `class` deliği nullable analizine devreder ve **derleyici tanısı** üretir.

**Ama bir koşulla:** CS8618 bir **uyarıdır, hata değildir**. `Ens.Kernel.csproj` bugün
`<Nullable>enable</Nullable>` taşıyor ama `<TreatWarningsAsErrors>` / `<WarningsAsErrors>`
**taşımıyor** (ölçüldü — csproj 8 satır, ikisi de yok). Yani `class` seçimi bile, projeye
`<WarningsAsErrors>Nullable</WarningsAsErrors>` eklenmedikçe konvansiyon düzeyinde kalır.

> **Talep T-1:** OQ1 `class` lehine kapatılmalı **ve** `Ens.Kernel.csproj`'a
> `<WarningsAsErrors>Nullable</WarningsAsErrors>` eklenmelidir. Bu tek satır, K-4/K-5/K-6'nın
> üçünün de "derleyici zorlar" iddiasını **doğru** yapan şeydir. Bugün o satır yok.

**Yan etki (ADR'de yok):** `class`'a geçmek `Measured`'ı her sayı dönüşünde **heap tahsisi**
yapar. `Scheduler` ve `ContextScore` sıcak yolunda bu ölçülmedi — `DOĞRULANMADI`. Ama
`Measured` için `class`, K-6'nın `implicit operator double`'ı ile birlikte **her aritmetik
adımda bir alloc** demektir; bu, K-6'nın `struct` kalmasını gerektirebilir ve o zaman OQ1'in
cevabı **karar-başına ayrışır** (K-4/K-5 → `class`, K-6 → `struct` + kabul edilen delik).
ADR'nin "tek bir cevabı bekliyor" varsayımı bu yüzden **yanlıştır**.

## 5. K-2 — Unicode: NFC, ToUpperInvariant, confusables

ADR §4.2'nin dört adımlı `Canonicalize` boru hattı .NET 10 üzerinde birebir kuruldu ve
gerçek girdilerle ölçüldü.

### E-3.1 ⛔ BLOKE EDİCİ — `ToUpperInvariant` Türkçe metni KATLAMIYOR; `G4` Türkçe'de açık kalıyor

ADR, `tr-TR` `I`/`ı` tuzağından kaçınmak için `ToUpperInvariant`'ı **zorunlu** kılıyor.
Gerekçe doğru, ama sonucu ölçülmemiş:

```
[U2] tr dotless i  U+0131  UPPER=U+0131  (DEĞİŞMİYOR)
[U2] tr dotted I   U+0130  UPPER=U+0130  (DEĞİŞMİYOR)
```

`ToUpperInvariant` U+0131 ve U+0130'a **dokunmuyor**. Sonuç, gerçek ENS verisi üzerinde:

```
[U10] 'işletme'          vs 'İŞLETME'          -> canon eşit mi: False   (IŞLETME | İŞLETME)
[U10] 'satın alma'       vs 'SATIN ALMA'       -> canon eşit mi: False   (SATıN ALMA | SATIN ALMA)
[U10] 'İnsan Kaynakları' vs 'insan kaynakları' -> canon eşit mi: False
[U10] 'insan kaynaklari' vs 'INSAN KAYNAKLARI' -> canon eşit mi: True    (yalnız ASCII'de)
```

**`G4` (*"Case variants split memory"*) K-2 ile ASCII adlarda kapanıyor, Türkçe adlarda
AÇIK KALIYOR.** ENS Türkçe bir projedir (`ens-language-policy`); `PurposeType` değerleri
Türkçe olacaktır. Yani K-2'nin `G4` kapanış iddiası, ENS'in **fiilî ana kullanım durumunda
yanlıştır**.

Alternatif comparer'lar da kurtarmıyor — ölçüldü:

```
[U11] 'ı'~'I'  OrdinalIgnoreCase=False  InvariantCultureIgnoreCase=False
[U11] 'İ'~'i'  OrdinalIgnoreCase=False  InvariantCultureIgnoreCase=False
```

Doğru mekanizma Unicode **case folding**'tir (`toCasefold`, UAX #31'in de atıf yaptığı yol),
ama **.NET BCL'de `ToFoldedCase`/`ToCaseFold` API'si yoktur** — `ToUpperInvariant` *simple
case mapping*tir, *case folding* değildir. K-2 bu ayrımı hiç yapmıyor.

> **Talep T-2:** K-2, Türkçe için ya (a) `ToUpperInvariant` öncesi açık bir `I`-ailesi
> ön-eşlemesi (`U+0130→I`, `U+0131→I`) tanımlamalı — ki bu `i` ile `ı`'yı **kasıtlı olarak
> birleştirir** ve R5'i (sessiz birleştirme) Türkçe'de sistematik hâle getirir — ya da
> (b) `G4`'ün Türkçe'de kapanmadığını **açıkça kabul etmelidir**. Üçüncü yol yok:
> Türkçe'de "aynı ad, farklı harf" ile "farklı ad" ayrımı Unicode simple case mapping ile
> yapılamaz. Bu bir ENS-özgü karardır ve ADR'de **hiç görünmüyor**.

### E-3.2 ⛔ BLOKE EDİCİ — M-3'ün mixed-script kısıtı .NET'te uygulanamaz (BCL'de Script API YOK)

ADR M-3 için dürüst bir sınır yazıyor: *"`confusables.txt` .NET BCL'de yoktur"* — ve ardından
**"mixed-script kısıtıyla yetinilir"** diyor. Ölçüm bu geri çekilme yolunun da kapalı olduğunu
gösteriyor:

```
[U6] public tip eşleşmesi (Confus|Script|Skeleton) : SIFIR
[U7] CharUnicodeInfo public static üyeler          : GetDecimalDigitValue, GetDigitValue,
                                                     GetNumericValue, GetUnicodeCategory
[U8] Rune property'leri                            : IsAscii, IsBmp, Plane, ReplacementChar,
                                                     Utf16SequenceLength, Utf8SequenceLength, Value
```

**.NET'in hiçbir public API'si bir karakterin Unicode Script'ini vermiyor.** `UnicodeCategory`
*genel kategoridir* (`Lu`, `Ll`, `Nd`…), Script değil — Latin `a` ile Kiril `а` ikisi de
`LowercaseLetter`tir. Mixed-script kısıtı, `Scripts.txt` **gömülmeden** yazılamaz.

Sonuç: M-3'ün **iki bileşeni de** (confusables + mixed-script) gömülü Unicode veri dosyası
gerektiriyor. ADR yalnızca birini borç olarak kaydetmiş. `W2c` bugünkü K-2 ile **tamamen
açıktır**, kısmen değil.

```
[U3] latin a vs kiril a (homoglyph) -> farklı   ← NFC katlamıyor (ADR doğru diyor)
[U3] rn vs m (aynı-script)          -> farklı   ← ADR bunu zaten kabul ediyor
```

### E-3.3 Boru hattı sıra hatası — `Cs` (surrogate) kapıdan geçiyor, adım 3 patlıyor

ADR adım 1'in ret listesi: **`Cc`, `Cf`, `Cn`, `Co`**. `Cs` (surrogate) **listede yok**.

```
[U12] U+D800 kategori = Surrogate           ← adım 1'den geçer
[U5]  "\uD800abc".Normalize(FormC) PATLADI: ArgumentException
[U5b] IsNormalized(...) de PATLADI
```

Eşlenmemiş bir surrogate adım 1'i geçiyor ve adım 3'te `ArgumentException` fırlatıyor.
ADR'nin tasarladığı *"reddetme `ArgumentException`, sessiz temizleme yok"* politikası burada
kazara sağlanıyor gibi görünür — ama mesaj Unicode'un iç hatasıdır, ENS'in ret gerekçesi
değildir; ve `TryParse(out reason)` yolu bu istisnayı yakalamak zorundadır, ki tasarımda yok.
**Düzeltme tek kelimedir: adım 1 listesine `Cs` eklenir.** Ucuz, ama ölçülmeden görülmüyor.

### E-3.4 ADR lehine ölçümler (doğrulanan üç iddia)

| ADR iddiası | Ölçüm | Sonuç |
|---|---|---|
| NFC homoglyph katlamaz | `[U3]` latin `a` vs kiril `а` → farklı | **doğru** |
| NFKC anlam değiştiren katlama yapar | `[U1]` `ﬁ` U+FB01 → NFKC `U+0066 U+0069`; NFC'de değişmiyor | **doğru** |
| `ToUpper`, `ToLower`'a tercih edilmeli | `[U2]` final sigma `ς`+`σ` → ToUpper ikisini de `Σ` yapıyor; ToLower **yapmıyor** | **doğru ve ölçüldü** |
| `Cc`/`Cf` kapısı gerekli | `[U4]` `U+0000`=Control, `U+202E`=Format, `U+200B`=Format | **doğru** |
| `Zs` → `U+0020` gerekli | `[U4]` `U+00A0`, `U+3000` = SpaceSeparator | **doğru** |

Ek olarak ADR'de yazmayan bir ayrıntı: **NFC, Kelvin işaretini (U+212A) `K`'ye katlıyor**
(`[U1]`) — yani bazı homoglyph-benzeri karakterler NFC ile zaten kapanıyor. Bu K-2 lehinedir.

Bir de not: `ß`'nin `SS`'ye katlanmadığı ölçüldü (`[U2]`, `[U11]`). Şiddeti düşük (ENS'te
Almanca ad beklenmiyor) ama K-2'nin "harf katlama" adımının **tam olmadığının** ikinci kanıtı.

## 6. K-3 — analyzer gerçekten yasaklıyor mu?

ADR §6, K-3'ün diğer beş karardan **zayıf** olduğunu kendisi söylüyor ve çareyi
`Microsoft.CodeAnalysis.BannedApiAnalyzers` + `BannedSymbols.txt` olarak öneriyor.
Bu, spike'ta gerçekten kurularak sınandı (`scratchpad/spike/BanSpike/`, net10.0).

### E-4.1 EVET — çalışıyor, ve `error`'a yükseltilebiliyor

`BannedSymbols.txt`:
```
P:System.DateTimeOffset.UtcNow;ADR-0003 K-3
P:System.DateTime.Now;ADR-0003 K-3
P:System.DateTime.UtcNow;ADR-0003 K-3
P:System.Environment.TickCount64;ADR-0003 K-3
```
`.editorconfig`: `dotnet_diagnostic.RS0030.severity = error`

```
error RS0030: 'DateTimeOffset.UtcNow' sembolü bu projede yasaklı
error RS0030: 'Environment.TickCount64' sembolü bu projede yasaklı
```

**Build kırılıyor.** ADR'nin önerisi mühendislik olarak **geçerlidir** — ve ADR'nin görmediği
bir bonus: `Environment.TickCount64` gibi zamanın *ikinci* kaynakları da aynı listeye girer.
`csproj`'a `<AdditionalFiles Include="BannedSymbols.txt" />` satırı ve `PrivateAssets=all`
paket referansı yeterli; harici altyapı gerekmiyor.

### E-4.2 Üç sessiz-başarısızlık yüzeyi — ADR'de hiçbiri yok

**(a) Yanlış yazılmış yasak satırı SESSİZ kalıyor.** İlk denemede `M:System.Environment.get_TickCount64`
yazıldı (yanlış DocID biçimi). Analyzer **hiçbir tanı üretmedi** — ne yasak devreye girdi, ne
"böyle bir sembol yok" uyarısı çıktı. Aynı şey uydurma bir satır için de doğrulandı
(`P:System.Totally.Bogus.Symbol` → sıfır tanı). Yani **yasak listesinin kendisi
yanlışlanabilir değildir**: yanlış yazılmış bir satır, korumanın olmadığı yanılsamasını
sessizce üretir. Bu, `AUDIT-WAVE2 §10.5`'in meta-kalıbının analyzer katmanındaki hâlidir.

> **Talep T-3:** `BannedSymbols.txt`'in her satırı için **bir negatif test** yazılmalı —
> yasaklı çağrıyı içeren bir dosyanın gerçekten derlenmediğini gösteren bir build testi.
> Aksi hâlde yasak listesi bir `Guard.cs` "kapatılan N nokta" listesine dönüşür ki ADR §8
> tam olarak bunu kaldırmayı öneriyor.

**(b) `#pragma warning disable RS0030` tek satırda bastırıyor — ölçüldü.** Bastırılan çağrı
`error` seviyesinde bile **hiç raporlanmadı**. ADR bu itirazı K-1'de (reddedilen alternatif 3)
ve K-5'te (reddedilen alternatif 3) *analyzer'ları reddetmek için* kullanıyor:
*"analyzer devre dışı bırakılabilir ve `#pragma warning disable` tek satırdır — E3'ün aynısı."*

**Aynı itiraz K-3'ün tek zorlama mekanizmasını da vurur.** ADR bu tutarsızlığı görmüyor:
analyzer K-1 için yeterince güçlü değil, ama K-3 için yeterli sayılıyor. Mühendislik olarak
doğru okuma şudur: analyzer **konvansiyondan güçlü, tipten zayıftır**; K-3 bu ara seviyede
kalır ve K-1/K-2/K-4'ün "derleyici zorlar" sınıfına **girmez**.

**(c) Reflection yolu kapsam dışı** (`typeof(DateTimeOffset).GetProperty("UtcNow")`) —
tanı üretmiyor. Bu P5'tir ve kapsam dışı; ADR ile tutarlı, itirazım yok.

### E-4.3 K-3'ün gerçek zorlaması: mimari test analyzer'dan güçlü

ADR §6 iki seçenek sunuyor: analyzer **ya da** K-5 tarzı assembly taraması. Ölçüm, ikisinin
**eşdeğer olmadığını** gösteriyor:

| Mekanizma | Bastırılabilir mi | İz kalır mı | Kapsam |
|---|---|---|---|
| `BannedApiAnalyzers` + `error` | **Evet** (`#pragma`, tek satır, iz git diff'te ama build yeşil) | Hayır | Kaynak kodu |
| Mimari test (assembly taraması) | Evet ama test **kırmızı olur** ya da `[Skip]` alır | **Evet** | IL/metadata |

`#pragma` ile bastırılan bir yasak, yeşil bir build üretir — ADR'nin kendi ölçütüne göre
(*"unutmak derleme hatası üretmeli"*) bu bir **kaçaktır**. Bir mimari test bastırıldığında
ise ya kırmızı panel ya `Skipped` sayacı kalır.

> **Talep T-4:** K-3, analyzer'a **ek olarak** (alternatif olarak değil) bir mimari test
> gerektirmelidir: `Ens.Kernel` assembly'sinin IL'inde `DateTimeOffset::get_UtcNow` /
> `DateTime::get_Now` / `DateTime::get_UtcNow` / `Environment::get_TickCount64` çağrısı
> **bulunmadığını** doğrulayan bir test. Bu, `#pragma` kaçağını kapatan tek yoldur.
> **DOĞRULANMADI:** böyle bir IL taramasının `System.Reflection.Metadata` ile maliyeti bu
> turda ölçülmedi.

**Verdict katkısı:** K-3'ün mekanizması **uygulanabilir**, ama ADR §6'nın "analyzer **ya da**
tarama" alternatifi yanlıştır — **ikisi de** gerekir.

## 7. K-6 ↔ `Guard.cs:130-132` çelişkisi (OQ2)

### E-5.1 Çelişki gerçek — ve kernel'in kendi testleri kırpma tarafını TUTUYOR

`Guard.cs:125-133` kırpma gerekçesini üç maddede yazıyor; üçüncüsü kritik:

> *"Tüm partiyi bir tek bozuk peer-sinyali yüzünden exception'la düşürmek, dikkat tahsisini
> komple durdurur (**servis-dışı bırakma vektörü**); kırpma fail-closed kalır."*

K-6/R19 bu gerekçeyle **çelişiyor** ve ADR bunu "§7'de açık soru" diye bırakıyor. Ama depo
bu soruya **zaten cevap vermiş** — üç ölçülmüş kanıtla:

**(a) `AUDIT_FIXED_B4` kırpmayı bir KAPANIŞ olarak tescil ediyor**
(`AdversarialAuditTests.cs:236-250`):
```csharp
Assert.Equal(0.5, scheduled[1].AttentionPriority, precision: 10);  // 1 x 0.5 x clamp(1e9)=1
```
Bu, **geçen** bir `AUDIT_FIXED_*` testidir. K-6 kırpmayı reddetmeye çevirirse `1e9` deficit
`ArgumentOutOfRangeException` üretir, bu satır kırılır ve **kapanmış sayılan B4 yeniden açılır**
(farklı bir biçimde).

**(b) `AUDIT_DEFECT_W9` politikanın tutarsızlığını zaten kayda geçmiş**
(`AdversarialWave_SchedulerGateTests.cs:216-243`). Test yorumu birebir:
> *"Guard.cs'in kendi yorumu (NormalizedDeficit) bu vektörü tanımlıyor ve deficit için
> KIRPMA'yı tam da bu yüzden seçmiş... Aynı gerekçe stake/confidence için UYGULANMAMIŞ —
> **politika kendi içinde tutarsız**."*

`AUDIT_DEFECT_W9` **geçen** bir testtir; `DEFECT-REGISTER.md` §0'a göre bu, kusurun
**var olduğu** anlamına gelir. Yani: exception politikası **zaten** bir DoS vektörü olarak
kayıtlıdır. **K-6 bu vektörü daraltmıyor, kernel'in sayı döndüren her public üyesine
genişletiyor.**

> **OQ2 CEVABI (mühendislik): K-6 kayıtsız şartsız reddetme politikası ile uygulanamaz.**
> Doğru ayrım depoda zaten var: *tanım gereği normalize* büyüklükler (`ConformanceDeficit`,
> `[0,1]`) **kırpılır**; *ölçülemeyen* değerler (`NaN`, `±∞`) **reddedilir**. `Guard.cs:133`
> bu ayrımı tek cümlede yazmış: *"NaN ise KIRPILMAZ — reddedilir."* K-6'nın `Measured.Of`'u
> `Guard.Finite`'i kullandığı için **bu ayrımı zaten koruyor** — çelişki `Measured`'da değil,
> ADR'nin R19 metnindedir. R19 fazla iddialıdır ve düzeltilmelidir, K-6 değil.

### E-5.2 Kaç test kırılır? — sayıldı

Kırpma→reddetme değişikliği yapılırsa doğrudan kırılan test metotları
(`grep` ile bulundu, gövdeleri okundu):

| Test | Dosya:satır | Neden kırılır |
|---|---|---|
| `AUDIT_FIXED_B4_ConformanceDeficit_is_clamped_...` | `AdversarialAuditTests.cs:236` | `clamp(1e9)=1` iddiası |
| `AUDIT_DEFECT_W3_NormalizedDeficit_clamp_does_not_normalize_negative_zero` | `AdversarialWave_SchedulerGateTests.cs:97` | `Guard.NormalizedDeficit(-0.0)` çağrısı ve `-0.0` iddiası |

**Sayı: 2 test metodu.** Üretim tarafında `Guard.NormalizedDeficit` yalnız **2 çağrı yerinde**
kullanılıyor (`BoundedAutonomyGate.cs:105`, `Laws/DecisionGravity.cs:46`) — yani OQ2'nin
kapsamı ADR'nin ima ettiğinden çok dardır. Bu **iyi haberdir**: OQ2 bir tasarım krizi değil,
iki çağrı yerinde verilecek bir karardır.

### E-5.3 ⛔ K-6'nın ölçülmemiş asıl kırılması: `Measured` SIRALANAMAZ (derleyici uyarmıyor)

ADR K-6 için *"Breaking? **Düşük** — `implicit operator double` sayesinde okuyan kodun çoğu
derlenmeye devam eder"* diyor. **Derleme tarafı ölçüldü ve ADR haklı:**

```
8 farklı xUnit assertion biçimi (Assert.Equal(double, Measured, precision:10),
Assert.Equal(double, Measured), double x = ..., var y = ... > 0, double.IsFinite(...))
→ 0 Hata, 8/8 test geçiyor
```

**Ama çalışma zamanı ölçülmedi ve orada kırılıyor:**

```csharp
xs.OrderByDescending(z => z).ToList()   // Measured üzerinde
→ System.InvalidOperationException (ObjectComparer`1.Compare)
```

`Measured` `IComparable<Measured>` uygulamadığı için `Comparer<Measured>.Default` çalışma
zamanında patlıyor. **Derleyici tek bir uyarı bile üretmiyor.**

Bu tam olarak `Scheduler`'ın sıcak yoludur:
```
Scheduler.cs:124:   .OrderByDescending(x => x.Scheduled.AttentionPriority)
CompanyMemory.cs:260: .OrderByDescending(r => Salience(r, asOf, contextDecayRate))
```

`AttentionPriority` `Measured` olursa `Scheduler.Schedule` **derlenir ve çalışma zamanında
patlar** — dikkat tahsisi tümüyle durur. Bu, ADR'nin "Breaking? Düşük" değerlendirmesini
yanlışlayan bir ölçümdür ve R18'de **yazmıyor** (R18 yalnız aritmetiği konuşuyor).

> **Talep T-5:** `Measured` (ve `UnitMeasured`) `IComparable<T>`, `IComparable`, ve
> karşılaştırma operatörlerini (`<`, `>`, `<=`, `>=`) **uygulamak zorundadır**. Aksi hâlde
> K-6, kernel'in en kritik iki sıralama yolunu sessizce kırar.

### E-5.4 R18 doğrulandı — kapı aritmetikte deliniyor

```csharp
var a = Measured.Of(double.MaxValue); var b = Measured.Of(double.MaxValue);
double sum = a + b;          // implicit -> double
double.IsInfinity(sum) == true   ✔ (test geçti)
```

`Measured + Measured` ara adımda `double`'a düşüyor ve `+∞` üretiyor. `W8b` (DeltaCapital
taşması) K-6 ile **kapanmıyor** — ADR'nin R18'de kabul ettiği gibi. Azaltma (`Measured`
üzerinde operatör tanımlamak) **zorunludur, opsiyonel değildir**; aksi hâlde K-6'nın 6
üyesinden `W8a`/`W8b` (2 üye) açık kalır ve ADR'nin 40 sayısı **38**'e iner.

## 8. R1 — mühür ↔ event sourcing uyumsuzluğu

Görev metni bunu "bloke edici olabilir" diye işaretledi. Ölçüm **bugün için bloke edici
olmadığını**, ama nedenin ADR'nin sandığı neden olmadığını gösteriyor.

### E-6.1 Kernel'de bugün SERİLEŞTİRME YOK — R1 henüz teorik

```
grep -l "Json|Serializ|BinaryWriter|IEventStore"  Ens.Kernel/ Ens.Kernel.Demo/  → SIFIR eşleşme
```

`Ens.Kernel` bugün **in-memory**dir. `Rehydrate(Identity, IEnumerable<DomainEvent>)`
(`DecisionAggregate.cs:309`) zaten **nesne** alıyor, byte akışı değil. Yani K-1'in mührü
bugünkü replay yolunda **hiç serileştirilmiyor** ve R1 bugün bir kırılma üretmiyor.

> **Ama bu, riskin yokluğu değil, ölçüm anının erkenliğidir.** ADR künyesi ENS'in Faz-4
> hedefinin PostgreSQL + event store olduğunu varsayıyor (`coding-standards`). Serileştirme
> geldiğinde R1 **aktif hâle gelir**.

### E-6.2 Serileştirme geldiğinde ne olur — ölçüldü

```
[A5] serialize   = {"Tool":"read_stock","Scope":1,"SealId":"33826822"}
[A5] deserialize = NotSupportedException: Deserialization of types without a parameterless
                   constructor, a singular parameterized constructor, or a parameterized
                   constructor annotated with 'JsonConstructorAttribute' is not supported
```

İki ayrı sonuç, ikisi de önemli:

1. **`System.Text.Json` mühürlü tipi hiç deserialize edemiyor** — `private` kurucu yüzünden
   `NotSupportedException`. ADR R1'in *"deserialize edilen token mührü kaybeder ve fail-closed
   olur"* öngörüsü **yanlıştır**: nesne hiç doğmaz, **`NotSupportedException`** atar. Fark
   önemlidir: fail-closed bir *karar*dır (izlenebilir), `NotSupportedException` bir *çöküştür*
   (replay tümüyle durur, kısmî sonuç yok). Bu, K-6/R19'un DoS itirazının event-store
   yolundaki ikizidir.
2. **Serileştirme mühür kimliğini SIZDIRIYOR.** `SealId` bir `public` property olduğu için
   JSON'a düştü. ADR'nin iskeleti `SealId`'yi göstermiyor ama teşhis edilebilirlik için böyle
   bir üye kaçınılmazdır (R2: *"teşhis edilmesi zor bir arıza sınıfı"*). Mühür hash'i dışarı
   sızdığı anda `ReferenceEquals` korumasının **teşhis değeri** kalır, **güvenlik değeri**
   azalır (hash çakışması aramak mümkün hâle gelir).

### E-6.3 Uygulanabilir yol var — ve ADR'nin reddettiği alternatife dönüyor

R1 için mühendislik olarak çalışan üç yol:

| Yol | Uygulanabilir mi | Maliyet |
|---|---|---|
| **(a) Mühür event'e hiç yazılmaz; replay'de yetki registry'den YENİDEN çözülür** | **Evet** — bugünkü `Rehydrate` zaten nesne alıyor | `Rehydrate` `CapabilityRegistry` parametresi almalı → **imza değişikliği**, K-1'in maliyet tablosunda **yok** |
| (b) Mühür serileşir | **Hayır** — `ReferenceEquals` süreç-içi kimliktir, byte'a çevrilemez |  |
| (c) İmzalı token (HMAC) | Evet | ADR'nin **reddettiği** alternatif 1 |

**(a) uygulanabilir ve doğru yoldur** — ama bir sonucu var: *yetki, event'te taşınan bir veri
değil, replay anında yeniden hesaplanan bir türevdir.* Bu, event-sourcing'in
"event = tek gerçek" aksiyomuyla (`DomainEvent.cs:4` — *"bir kez emit edilen Event asla
değişmez"*) gerilim içindedir: aynı event akışı, registry değiştiğinde **farklı yetki** verir.
Denetim izi "o an neye izin verilmişti" sorusunu artık cevaplayamaz.

> **Talep T-6:** K-1, `Rehydrate`'in yetkiyi nasıl çözeceğini **karar cümlesine** yazmalıdır.
> Önerilen: event'e mührün kendisi değil, **veren otoritenin kimliği ve versiyonu**
> (`IssuerId`, `RegistryVersion`) yazılır; replay o kayda karşı doğrular. Böylece hem denetim
> izi korunur hem `ReferenceEquals` süreç-içi kalır. Bu, ADR'de **yoktur**.

> **Verdict katkısı:** R1 K-1'i **uygulanamaz yapmaz**, ama K-1'i *"6 dosya + 7 test dosyası"*
> maliyetinden çıkarıp `Rehydrate` imzası + event şeması değişikliğine taşır. ADR'nin maliyet
> tablosu bu turda **eksik** ölçülmüştür.

### E-6.4 Yan bulgu — `DomainEvent`'in kendisi E-1.1'e açık

`DomainEvent.cs:9-19`:
```csharp
public abstract record DomainEvent {
    public Guid EventId { get; init; } = Guid.NewGuid();
    public required Identity Emitter { get; init; }
    public required Identity Target  { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
```

Tüm alanlar `init` → **her event, herhangi bir assembly'den `evt with { Emitter = başkası }`
ile yeniden yazılabilir.** E-1.1'in aynısı, ve burada K-1'in mührü **hiç yok**. Bu, ADR'nin
`W2_O1` için söylediği *"ortada zorlanacak bir yetki hiç yok"* teşhisinin kod düzeyindeki
karşılığıdır ve `W2_R2`'nin (replay provenance) K-1 ile kapanacağı iddiasını zayıflatır:
`Rehydrate` `e.Target == id` doğrulasa bile, saldırgan `with` ile `Target`'ı **düzeltip**
`Emitter`'ı değiştirebilir.

### E-6.5 Yan bulgu — K-3, `DomainEvent.cs:19`'da tasarım değişikliği zorunlu kılıyor

Kernel'de `DateTimeOffset.UtcNow` **tek bir yerde** geçiyor (ölçüldü):
```
Ens.Kernel/Domain/DomainEvent.cs:19:  public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
```
Bu iyi haberdir (K-3'ün M-1 yüzeyi çok dar), ama bir property initializer'a `TimeProvider`
**enjekte edilemez**. K-3 uygulandığında `Timestamp` ya `required` olur (her event üretim
yerinde açıkça verilir — **tüm event üretim çağrıları değişir**) ya da her event bir fabrika
üzerinden doğar. ADR'nin K-3 maliyet tablosunda `Domain/Events/DecisionEvents.cs` var ama
`Domain/DomainEvent.cs` **yok** — asıl değişecek dosya odur.

## 9. Maliyet ölçümü — dosya ve test yüzeyi

*(doldurulacak)*

## 10. "Örnek kapatıldı, sınıf açık" denetimi

*(doldurulacak)*

## 11. OQ6 — uygulama sırası türetilebilir mi?

*(doldurulacak)*

## 12. Talepler

*(doldurulacak)*
