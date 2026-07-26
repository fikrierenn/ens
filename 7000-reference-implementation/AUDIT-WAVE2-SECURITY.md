# AUDIT — Dalga 2: `CapabilityRegistry` + `LlmAdapter` + `BoundedAutonomyGate` yetki sınırı + `DecisionEntropy`

| | |
|---|---|
| **Denetleyen** | `ens-skeptic` — yeniden türetme context'i (özgün denetim ajanı raporunu yazamadan öldü) |
| **Denetlenen** | `Ens.Kernel/Capability/CapabilityRegistry.cs` (ADR-0001 §6), `Ens.Kernel/Adapter/LlmAdapter.cs` (ADR-0001 §7), `Ens.Kernel/BoundedAutonomyGate.cs` (ADR-0001 §5.6), `Ens.Kernel/Laws/DecisionEntropy.cs` (ENS-3021) |
| **Kanıt** | `Ens.Kernel.Tests/AdversarialWave_SecurityTests.cs` — 51 test metodu, 69 test vakası |
| **Bulgu** | **26 DEFECT + 5 FINDING**; ayrıca 8 FIXED (regresyon bekçisi) + 12 HOLDS (ayakta kalan iddia) |
| **Verdict** | **`wounded`** (§11) |
| **Yetki** | Anayasa Madde X (Yanlışlanabilirlik Ödevi), Madde VIII (İzlenebilirlik Yasası), Madde VI (Anti-Pattern'ler — black-box çıktı) |
| **Tarih** | 2026-07-26 |

> **Bu rapor YENİDEN TÜRETİLMİŞTİR.** Testleri yazan denetim ajanı raporunu yazamadan öldü;
> gerekçesi ve saldırı senaryoları kayıptı. Bu belge, test gövdeleri ve denetlenen kaynak kod
> okunarak yeniden inşa edildi. Sınırları §0'da yazılıdır — **`dotnet test` çalıştırılamadı ve
> hiçbir test sonucu uydurulmadı.**

> **DEFECT-REGISTER.md ile ilişki:** sicil bu dosya için **19 DEFECT + 4 FINDING** sayıyor;
> bağımsız sayımım **26 + 5**'tir (§10.1 — `W8` grubunun tamamı ve `W5d`/`W5e`/`W5f`/`W7h`
> sicilde yok). Sicilin **dokuz şiddet atamasına katılmıyorum** (§10.2).

---

## 0. ÖNCE DÜRÜSTLÜK: bu raporun kendi sınırları

### 0.1 Bu rapor YENİDEN TÜRETİLMİŞTİR — özgün gerekçe kayıptır

`AdversarialWave_SecurityTests.cs` testlerini **ben yazmadım.** Onları yazan ajan, raporunu
yazamadan API stall ile öldü (DEFECT-REGISTER §9 bunu kaydediyor). Elimde **testler ve kaynak
kod** vardı; ajanın akıl yürütmesi yoktu. Bu raporun her satırı, test gövdeleri + kaynak
kod okunarak **yeniden türetildi.**

Bunun somut sonucu: **özgün ajanın niyetiyle benim yorumum ayrışmış olabilir.** Test
yorumlarının çoğu gerekçeyi taşıyor (dosya bu yönden iyi yazılmış), ama bir testin *neden* o
şiddette görüldüğü çoğu yerde yazılı değil. Şiddet atamaları bu raporda **benimdir**, özgün
ajanın değil. Bu ayrımı saklamıyorum.

### 0.2 `dotnet test` ÇALIŞTIRILAMADI — hiçbir sonuç uydurulmadı

Bu context'e verilen araç seti `Read`, `Grep`, `Glob`, `Write`, `Edit`, `WebSearch`,
`WebFetch`'tir. **`Bash` ya da herhangi bir komut çalıştırma aracı yoktur.** Dolayısıyla:

- Bu raporda **tek bir test çıktısı satırı fabrike edilmedi.**
- 51 test metodunun (69 test vakası) geçtiğini **iddia etmiyorum** — doğrulayamadım.
- Bütün bulgular **statik analizdir**: test gövdesi + denetlenen kaynak dosya + C# / IEEE-754
  semantiği. Her bulgunun yanında hangi kaynak satırına dayandığı yazılıdır.

Bu, üst üste **üçüncü** turdur ki denetim testlerini koşturamıyor (`…SCHEDULER.md` §0 aynı
şeyi söylüyordu). SKR-041 emsali gereği bu, uydurmaktansa yazılmalıdır — ama artık bir
**yapısal kusur** hâline gelmiştir ve kapatılması gerekir (§12).

**Doğrulama yaptığım şey:** her `AUDIT_FIXED_*` iddiasını denetlenen kaynak dosyada elle
karşılaştırdım. Örneğin W6a-c-f'nin dayandığı `Guard` çağrılarının `LlmAdapter.cs:134-141`'de
gerçekten var olduğunu ve `Guard.Finite`'in (`Guard.cs:70-77`)
`ArgumentOutOfRangeException` attığını satır satır teyit ettim. Bu bir **çıkarımdır**, ölçüm
değil — ama boşlukta bir iddia da değil.

### 0.3 Kanıt dosyasının kendisinde bir kusur buldum (yeni bulgu)

`AdversarialWave_SecurityTests.cs:27-29` şunu iddia ediyor:

> *"KAYNAK KODLAMASI: bu dosyada hiçbir çıplak non-ASCII karakter YOKTUR. Homoglif/zero-width/
> NUL/RTL saldırıları BİLEREK `\uXXXX` escape'leriyle yazılmıştır — aksi hâlde testin kendisi
> dosya-kodlamasına bağımlı ve okunamaz olurdu."*

**Bu ifade yanlıştır.** Mekanik olarak taradım:

| Ölçüm | Sonuç |
|---|---|
| `\uXXXX` escape sayısı | **0** |
| Çıplak non-ASCII karakter içeren satır sayısı | **21** |

Homoglif (`а` U+0430), ZWSP, ZWJ, ZWNJ, RTL-override (U+202E), NBSP, NUL ve NFC/NFD `é`
karakterlerinin **hepsi dosyaya çıplak gömülmüş** durumda.

**Etki:** W1c, W2a, W2b, W2c, W2d, W2e — yani **6 test / ~22 test vakası** — dosyanın bayt
düzeyindeki bütünlüğüne bağımlı. Bir editör "dosyayı normalize et" ya da "görünmez karakterleri
temizle" uygularsa, bu testler **sessizce anlamsızlaşır**: `W2c`'nin homoglif ikizi gerçek adla
özdeşleşir ve test artık hiçbir şey sınamaz. W2b'de `Assert.NotEqual(Nfc, Nfd)` gürültülü
şekilde kırılır (iyi), ama W1c/W2a'daki `InlineData` varyantları **sessizce** eş hâle gelir (kötü).

Bu, kusurun kendisinden çok **kanıtın dayanıklılığına** dair bir bulgudur ve tam olarak benim
işimdir: dosya, uyguladığını iddia ettiği metodolojik güvenceyi **uygulamamış**; ve doğrulanmamış
bir iddia Madde VI'nın reddettiği şeydir ("yanlışlanamaz iddialar" — anti-pattern listesi);
Madde X ise böyle bir iddiayı taşıyan yapıtı **eksik** sayar. **Talep:** ya yorum düzeltilmeli, ya da saldırı
karakterleri gerçekten `\uXXXX` escape'lerine çevrilmelidir. İkincisi tercih edilir.

---

## 1. W1 — `Disable` yolu: sessiz başarı ve operatörün yanlış inancını onaylayan doğrulama

Tek bir kök neden, beş sonuç. Kök neden **üç satırdır**:

```csharp
// CapabilityRegistry.cs:131
public void Disable(string packName) => _disabled.Add(packName);
// CapabilityRegistry.cs:134
public bool IsEnabled(string packName) => _packs.ContainsKey(packName) && !_disabled.Contains(packName);
// CapabilityRegistry.cs:116
private readonly HashSet<string> _disabled = new(StringComparer.Ordinal);
```

`Disable` **void**'dur, dönüş değeri yoktur, kayıtlı isimlere karşı **doğrulama yapmaz**, ve
`HashSet.Add` zaten var olan bir eleman için `false` dönerken bu değer de atılır. Bir
yetki-KALDIRMA jesti — güvenlik-kritik bir işlem — hiçbir koşulda başarısızlık sinyali üretemez.

### 1.1 `W1a` / `W1c` — yanlış harf ya da biçim, sessizce "başarılı" olur

**Saldırı senaryosu:** operatör `registry.Disable("operations")` yazar (küçük harf).
Exception yok, dönüş değeri yok, uyarı yok. Pack etkin kalır, araçlar yetkili kalır:

```csharp
registry.Disable("operations");
registry.IsEnabled("Operations");                      // true  — Pack HÂLÂ ETKİN
registry.Authorize("read_stock").IsAllowed;            // true  — yetki DURUYOR
registry.Authorize("create_purchase_order").IsAllowed; // true
```

`W1c` bunu dokuz varyantla genelliyor ve hepsi aynı sonucu veriyor: sondaki boşluk, baştaki
boşluk, büyük harf, Kiril `а` (U+0430) homoglifi, zero-width space, zero-width joiner, NUL,
RTL-override, no-break space. **Dokuzunun da hiçbir etkisi yok.** Bunların çoğu bir saldırgan
gerektirmez — bir PDF'ten, bir wiki sayfasından ya da bir sohbet mesajından kopyalanan Pack adı
görünmez bir karakter taşıyabilir.

**Etki:** P7. Revocation, yönetişimin **geri alma** yeteneğidir; sessizce başarısız olan bir
revocation, operatör açısından **fail-open**'dır. Anayasa Madde VIII (İzlenebilirlik Yasası) ve
GOV-000 G5 ("sessiz karar yoktur") de düşer: işlemin gerçekleşmediğine dair hiçbir iz üretilmiyor.

### 1.2 `W1b` (KRİTİK) — doğrulama sorgusu, operatörün yanlış inancını ONAYLIYOR

**Bu, W1 ailesinin ve muhtemelen bu dalganın en kötü bulgusudur — ve kötülüğü kusurun
büyüklüğünden değil, kusurun kendini gizleme biçiminden gelir.**

Dikkatli bir operatör revocation'ı doğrulamak ister. Yazdığı sorgu, kapatırken kullandığı
ismin aynısıdır — çünkü doğru olduğuna inandığı isim odur:

```csharp
registry.Disable("operations");
registry.IsEnabled("operations");                 // false  <-- "revoke edildi" görünüyor
registry.Authorize("read_stock").IsAllowed;       // true   <-- ama edilmedi
```

`IsEnabled` (`CapabilityRegistry.cs:134`) **"kayıtlı değil"** ile **"devre dışı"** arasında ayrım
yapmaz: `_packs.ContainsKey(x) && !_disabled.Contains(x)`. `"operations"` kayıtlı olmadığı için
ilk terim `false` döner ve fonksiyon `false` verir. Operatör bunu "kapalı" diye okur.

**İki bağımsız gözlem aynı yalanı söylüyor.** Sistem yalnızca yanlış davranmıyor; operatörün
yanlış güvenlik hissini **kendisi üretiyor.** Denetim açısından bu, kusurun kendisinden daha
kötüdür: kusur sessiz kalsaydı operatör er ya da geç yeteneğin çalıştığını fark ederdi; burada
sistem ona "kapattın" diyor. Yanlış bir güvence, güvence yokluğundan tehlikelidir — çünkü
araştırmayı durdurur.

**Kapanış:** `IsEnabled` üç durumu ayırt eden bir tip dönmelidir
(`NotRegistered` / `Enabled` / `Disabled`), ve `Disable` kayıtlı olmayan bir isimde ya
`false` dönmeli ya da fırlatmalıdır. `bool`, üç durumlu bir soruyu temsil edemez —
bulgunun tip düzeyindeki kökü budur.

### 1.3 `W1d` — pre-emptive `Disable`, sonraki `Register`'ı ölü doğurtuyor

`_disabled` kümesi **kalıcıdır ve kayıttan bağımsızdır.** Henüz kayıtlı olmayan bir isim
devre dışı bırakılabilir; sonra o isimle `Register` yapılır ve Pack **sessizce ölü doğar**:

```csharp
registry.Disable("Operations");   // henüz kayıtlı değil — sessizce kabul
registry.Register(OpsPack());     // hiçbir uyarı/hata yok
registry.IsEnabled("Operations"); // false
registry.EnabledPacks;            // boş
registry.Packs;                   // 1 kayıt — var ama ölü
```

**Etki:** `CapabilityRegistry.cs:32` dosyanın kendi dürüst-sınır bloğunda ADR-0001 §6'nın
"çakışmayı **sessizce çözmez**, uyarır" ilkesini benimsediğini ilan ediyor ve aynı satırda
"burada en muhafazakâr yorum uygulandı" diyor. `Register`, açıkça çakışan bir durumda
(kayıt vs. önceden konmuş disable) **hiçbir şey söylemiyor**. Dosya, kendi ilan ettiği ilkeyi
kendi içinde ihlal ediyor.

**Şiddet — REGISTER İLE AYNI FİKİRDE DEĞİLİM.** DEFECT-REGISTER §2, `W1a–W1d`'yi tek satırda
**Kritik** sayıyor. W1d'nin **yönü muhafazakârdır**: yetenek ölü doğar, yani fail-*closed*.
Zarar, izinsiz icra değil, açıklanamayan bir arıza ve boşa giden hata ayıklamadır. Bunu
**Orta** olarak derecelendiriyorum. W1a/W1b/W1c ile aynı kutuya konması, kritik sınıfını
şişiriyor ve önceliklendirmeyi bozuyor.

### 1.4 `W1e` — `disabled` kümesi doğrulanmamış public girdiden sınırsız büyüyor

1000 kez `Disable("ghost-" + i)` çağrısı, 1000 kalıcı çöp giriş üretir; hiçbiri bir şey yapmaz,
hiçbiri temizlenmez, hiçbiri sorgulanamaz. Bellek yönünden sınırsız büyüme, anlam yönünden
"revoke edildi" izlenimi veren bir kayıt yığını.

**Şiddet: Düşük-Orta.** Register bunu §4.3'te **Orta** sayıyor; katılıyorum ama sebebiyle
değil: asıl mesele bellek değil, `_disabled`'ın **denetlenebilir bir revocation kaydı olmaması**.
Bugün "hangi yetenekler ne zaman, kim tarafından kapatıldı" sorusunun cevabı sistemde yok —
`_disabled` yalnızca bir string kümesi. Madde VI açısından esas eksiklik budur ve W1e onu
görünür kılıyor.

### 1.5 `W1f` (FIXED) ve `W1g` (HOLDS) — kısıt monotonluğu gerçekten tutuyor

Dürüstlük gereği: `Disable` **bir kısıtı asla kaldıramıyor.** Katı bir Pack (`RequiresHumanApprovalFor`)
doğru isimle devre dışı bırakılsa bile, araç gevşek bir Pack üzerinden yetkili kalıyor ama
insan-onayı şartı **düşmüyor** (`CapabilityRegistry.cs:160` — onay şartı `_packs.Values`'tan,
yani **kayıtlı tüm** Pack'lerden toplanıyor; yetki ise `EnabledPacks`'ten). AUDIT §5.5/F3
kapanışı **gerçek ve çalışıyor.**

`W1g` bunu yapısal olarak da doğruluyor: `Unregister`/`Remove`/`Clear` metodu **yok** (reflection
ile metot listesi taranarak kanıtlandı), dolayısıyla kısıt kümesi monoton büyür. Bu, bilinçli ve
iyi bir tasarım kararıdır.

> **Ama bu güvencenin tek deliği W2c'dir** (§2.1): yeni bir Pack, korunan aracın *homoglif ikizini*
> tescil ederek onay şartından muaf bir sürüm üretebiliyor. W1g'nin ispatladığı monotonluk,
> **isim uzayı sabit olduğu sürece** geçerlidir; W2c isim uzayını genişletebildiğini gösteriyor.
> İki bulgu birlikte okunmalıdır.

---

## 2. W2 — Araç adı normalizasyonu: homoglyph, NUL, kontrol karakteri, NFC/NFD

**Önce kazanım (`W2a`, `W2b` — HOLDS).** `Authorize` tarafında `StringComparer.Ordinal`
**fail-closed yönde** çalışıyor: 11 farklı benzer-görünümlü varyantın (Kiril `а`, büyük/küçük
harf, baştaki/sondaki boşluk, ZWJ, ZWSP, ZWNJ, NUL, RTL-override, NBSP) **hiçbiri** kayıtlı
`read_stock` iznini eşleştirmiyor. NFD biçimli bir isim, NFC biçimli kaydı da açmıyor. Yani
*doğrulama* tarafında Ordinal doğru tercihtir ve savunma yönü doğrudur.

**Kusur, doğrulama tarafında değil, KAYIT tarafındadır:** `CapabilityPack` ctor'ı
(`CapabilityRegistry.cs:81-102`) yalnızca `IsNullOrWhiteSpace` bakıyor. Ne kanonlaştırma
(NFC), ne confusable/homoglif kontrolü, ne karakter-sınıfı doğrulaması var.

### 2.1 `W2c` — homoglif araç adı, korunan bir aracın ONAYSIZ İKİZİNİ üretiyor

```csharp
const string Real = "create_purchase_order";
const string Twin = "creаte_purchase_order";   // Kiril 'a' (U+0430)

registry.Register(OpsPack());                                   // Real -> onay ŞART
registry.Register(new CapabilityPack("Shadow", "1.0", [Twin])); // Twin -> onaysız

registry.Authorize(Real).RequiresHumanApproval;   // true
registry.Authorize(Twin).RequiresHumanApproval;   // false  <-- ikizi KORUMASIZ
Real.Length == Twin.Length;                       // true   <-- gözle ayırt edilemez
```

Onay-şartı taraması da aynı Ordinal anahtarla yapıldığı için (`CapabilityRegistry.cs:160`),
ikiz isim kısıt kümesine hiç değmiyor.

**Etki — asıl formülasyon budur:** W1g, kernel'in kısıt kümesinin **monoton** olduğunu
(hiçbir jest bir kısıtı kaldıramaz) yapısal olarak ispatlıyor. W2c, bu güvencenin **tek
deliğidir**: yeni bir Pack, kısıtı *kaldırmadan*, kısıttan **muaf bir eşdeğer** üretebiliyor.
İspatlanmış bir değişmezin etrafından dolaşılabiliyorsa, ispat eksiktir.

**En güçlü karşı-argüman (kendi bulgumuza karşı — dürüstlük gereği yazıyorum):**
İkiz adın gerçekten zararlı olması için, aşağı akıştaki icra katmanının `"creаte_purchase_order"`
adını **gerçek** satın alma emri aracına çözmesi gerekir. Kernel'de bugün araç-adı → araç
dispatch'i **yoktur** (`PendingDecision`'da araç adı alanı bile yok — `…SCHEDULER.md` §4.3).
Eğer icra katmanı da Ordinal ise, ikiz ad **hiçbir gerçek araca çözülmez** ve saldırı boşa çıkar.
Dolayısıyla W2c bugün bir yetki yükseltmesi **değildir**.

Ama iki gerçek zarar kalıyor ve bunlar bugün de geçerli:
1. **İnsan denetçiye karşı:** policy'yi gözden geçiren insan, iki satırı ayırt edemez. P7'nin
   karar mercii insandır; ona ayırt edilemez iki farklı yetki göstermek P7'ye saldırıdır.
2. **Tanımsızlığın kendisi:** dispatch semantiği ADR-0001'de **tanımlı değil.** Bir LLM'e araç
   listesi verilip çağrı ürettirildiğinde normalizasyonun nerede olacağı belirsizdir. Kusur,
   "bugün istismar edilemez" değil, **"istismar edilip edilemeyeceği tanımlanmamış"**tır.

**Şiddet — REGISTER İLE AYNI FİKİRDE DEĞİLİM.** Register §2 bunu koşulsuz **Kritik** sayıyor.
Ben **Yüksek** diyorum, ve koşulu açıkça yazıyorum: *ADR-0001 §6'nın marketplace hedefi
gerçekleştiğinde (Pack yazarlığı bir güven sınırı hâline geldiğinde) **Kritik**'e yükselir.*
Register bu koşulu gizliyor — oysa testin kendi yorumu (satır 238-239) koşulu açıkça yazmış.
**Bulgunun koşulunu düşürmek, bulguyu güçlendirmez; denetimin güvenilirliğini düşürür.**

### 2.2 `W2d` — kontrol karakterleri, insana gösterilen `reason` metnine sanitize edilmeden akıyor

```csharp
registry.Register(new CapabilityPack("Repor‮troper", "1.0", ["t"], ["t"]));
registry.Authorize("t").Reason;   // U+202E (RTL override) metnin içinde, sanitize EDİLMEDEN
```

Pack adı doğrudan `Authorize(...).Reason` içine enterpole ediliyor
(`CapabilityRegistry.cs:167-171`).

**ŞİDDET — REGISTER İLE AÇIKÇA AYNI FİKİRDE DEĞİLİM.** Register §5 bunu **Düşük** ("kozmetik /
dayanıklılık") sayıyor. Bu **yanlış sınıflandırmadır.** `Reason` metni dekoratif değil;
**var oluş sebebi insana gösterilmektir** — P7 altında insanın onay verip vermeyeceğine karar
verdiği tek bağlamsal bilgidir. RTL-override, gösterilen metnin **görüntülenme sırasını
tersine çevirir**: onay isteminde okunan cümle ile onaylanan işlem farklılaşabilir. Bu,
"kozmetik" değil, **onay mekanizmasının bütünlüğüne yapılan saldırıdır**.

**Benim derecelendirmem: Yüksek** (koşul: `Reason` bir insan arayüzünde render ediliyorsa —
ki bu alan tam olarak bunun için var). Kapanış ucuz: Pack adı ve araç adları için karakter-sınıfı
doğrulaması (`Cc`, `Cf`, bidi-control kategorilerinin reddi) + `Reason` üretiminde kaçış.

### 2.3 `W2e` — NUL taşıyan araç adları birinci sınıf yetkili yetenek oluyor

`new CapabilityPack("P", "1.0", ["read\0_stock"])` sorunsuz kaydediliyor ve
`Authorize("read\0_stock").IsAllowed == true`. `IsNullOrWhiteSpace` NUL'u boşluk saymaz.

**Şiddet: Düşük.** Register bunu **Orta** sayıyor; katılmıyorum. Kendi başına bir yetki
kazandırmıyor; W2c/W2d ile **aynı kök nedenin** (karakter-sınıfı doğrulaması yok) üçüncü
belirtisi. Ayrı bir kusur olarak sayılması, kusur envanterini şişiriyor. Tek bir düzeltme
(kimlik doğrulama fonksiyonu) üçünü birden kapatır.

### 2.4 `W2f` — `Register`, `Authorize`'ın ASLA sorgulayamayacağı araç adlarını kabul ediyor

Asimetri, iki satır arasında:

```csharp
new CapabilityPack("P", "1.0", ["", "   ", "\t", "ok"]);   // kabul  (ctor sadece name/version bakar)
registry.Authorize("");                                     // ArgumentException  (:146)
```

`CapabilityPack` ctor'ı **Pack adını** ve **versiyonu** `IsNullOrWhiteSpace`'e karşı doğruluyor
ama **araç adlarını hiç doğrulamıyor**. Sonuç: kaydedilmiş ama **ulaşılamaz** yetki satırları.
Policy'yi okuyan "bu izinli" der, runtime o satırı asla test edemez.

**Etki:** Bu bir *fail-open* değil, bir **anlam kaybıdır**: yetki tablosu, sorgulanabilir
olmayan satırlar içeriyor. ADR-0001 §6'nın "deklaratif izinler" iddiası, deklarasyonun
sorgulanabilirliğini varsayar.

**Şiddet: Orta.** Register ile aynı fikirdeyim. Kapanış: ctor, her araç adını `Authorize`
ile **aynı** doğrulamadan geçirmelidir — tek bir ortak `ToolName` tipi bu asimetriyi yapısal
olarak imkânsız kılar (DEFECT-REGISTER §7 kalıp 2 ile aynı mimari karar).

---

## 3. W3 — `FrozenSet` ve reflection: donmuş izin kümesinin delinmesi

### 3.1 Önce kazanım: `W3a`, `W3b` (FIXED), `W3d`, `W3e` (HOLDS)

AUDIT §5.2'nin düzeltmesi **gerçekten tutuyor.** `((HashSet<string>)pack.AllowedTools).Add(...)`
artık `InvalidCastException` atıyor; çağıranın elindeki canlı `List` sonradan değiştirilse de
Pack'in izinleri değişmiyor (`CapabilityRegistry.cs:93-94` — `ToFrozenSet(StringComparer.Ordinal)`
savunmacı kopya + değiştirilemez tip). `registry.Packs` de downcast edilebilir bir `List` değil
(`Dictionary.ValueCollection`). Ve `Authorize`, hiç Pack yokken de, boş Pack varken de
**fail-closed**: izin vermiyor. Bunlar dürüstçe kaydedilmesi gereken kazanımlardır.

### 3.2 `W3c` — reflection donmuş kümeyi hâlâ değiştiriyor, registry bir daha doğrulamıyor

**Saldırı senaryosu:**

```csharp
var pack = OpsPack();
registry.Register(pack);
registry.Authorize("delete_database").IsAllowed;   // false

var field = typeof(CapabilityPack).GetField("_allowedTools",
    BindingFlags.NonPublic | BindingFlags.Instance);      // CapabilityRegistry.cs:72
field.SetValue(pack, new[] { "read_stock", "create_purchase_order", "delete_database" }
    .ToFrozenSet(StringComparer.Ordinal));

registry.Authorize("delete_database").IsAllowed;          // true  <-- yetki kaçağı
registry.Authorize("delete_database").RequiresHumanApproval; // false
```

**ŞİDDET — REGISTER İLE AÇIKÇA AYNI FİKİRDE DEĞİLİM.** DEFECT-REGISTER §2 bunu **Kritik**
listesine, üstelik "saldırgana özel yetki gerekmiyor" başlığı altına koymuş. Bu **yanlıştır**:
`FieldInfo.SetValue` ile private bir instance alanını yazmak, süreç içinde **tam güven**
gerektirir. O yetkiye sahip bir saldırgan zaten `registry.Register(new CapabilityPack("Evil",
"1.0", ["delete_database"]))` çağırabilir — hatta gate'i tümüyle atlayabilir. Reflection
saldırısı, bu tehdit modelinde **hiçbir yeni yetenek kazandırmaz.**

Dahası register kendi içinde tutarsız: §2 bunu Kritik ilan ederken §8.3 aynı kusuru
*"sandbox izolasyonu kararı verilmedi, süreç sınırında savunulacaksa bu kusurlar kapsam
dışıdır"* diye açık bırakıyor. Bir kusur aynı belgede hem Kritik hem muhtemelen-kapsam-dışı
olamaz. `CapabilityRegistry.cs:27-29` zaten **"SANDBOX YOK"** diyerek izolasyonu açık borç
ilan etmiş durumda.

**Benim derecelendirmem — kusuru ikiye ayırmak gerekiyor:**

| Bileşen | Şiddet | Gerekçe |
|---|---|---|
| Reflection ile `FrozenSet` değiştirilebilmesi | **Düşük** (kabul edilmiş tehdit modeli dışı) | Tam-güven saldırganına karşı savunma, süreç/sandbox sınırının işi. Kernel içinde kapatılamaz. |
| Kaynak yorumunun **mutlak** iddiası | **Yüksek (iddia kusuru)** | `CapabilityRegistry.cs:41`: *"deklaratif izinler runtime'da **gerçekten sabittir**"*. Bu ifade yanlıştır ve Madde VI'nın reddettiği türdendir (yanlışlanamaz/doğrulanmamış iddia); Madde X onu **eksik** sayar. |
| Registry'nin Pack'i **canlı referans** tutup bir daha doğrulamaması | **Orta (asıl dayanıklı bulgu)** | `CapabilityRegistry.cs:127` — `_packs[pack.Name] = pack`. Ne hash, ne imza, ne kopya. Pack kimliği kayıt anında **bağlanmıyor.** |

Üçüncü satır, reflection'dan **bağımsız olarak** doğrudur ve bu yüzden bulgunun kalıcı
değerlidir: bugün `CapabilityPack` bir `record` olduğu için değiştirilemez sayılıyor, ama
registry bu varsayımı hiçbir noktada **kontrol etmiyor**. Yarın Pack'e değiştirilebilir tek bir
alan eklenirse (ör. bir konfigürasyon sözlüğü), registry bunu fark etmeyecek. Doğru kapanış:
kayıt anında Pack'in izin kümelerinin bir **content hash**'ini saklamak ve `Authorize` yolunda
(ya da en azından periyodik bir bütünlük kontrolünde) doğrulamak.

**Talep (dürüstlük):** ADR-0001 sandbox sınırına karar verene kadar `CapabilityRegistry.cs:41`
mutlak ifadesi düzeltilmelidir: *"downcast'e ve çağıran-tarafı mutasyona karşı sabittir;
tam-güven reflection'a karşı **değildir** — bu, süreç sınırının borcudur (§(b) SANDBOX YOK)."*
Bu, kod değişikliği gerektirmeyen, bugün kapatılabilir bir Madde X borcudur.

---

## 4. W4 — Sahte `ToolAuthorization` ve ayırt edilemeyen gate durumları

### 4.1 `W4a` (KRİTİK) — sahte bir `ToolAuthorization`, Capability Registry'nin TAMAMINI atlar

**Saldırı senaryosu (tek satır, reflection yok, özel yetki yok):**

```csharp
var honest = registry.Authorize("delete_database");        // IsAllowed = false
var forged = new ToolAuthorization(true, false, "sahte - registry'den gelmedi");

BoundedAutonomyGate.Evaluate(stake: 10, confidence: 0.9, conformanceDeficit: 0,
    isIrreversible: false, autonomyThreshold: 5_000, blockThreshold: 60_000,
    toolAuthorization: forged).Decision;                     // -> Autonomous
```

Aynı çağrı `honest` ile `Blocked` verir. Fark tek bir `new` ifadesidir.

**Kod satırı:** `CapabilityRegistry.cs:107` —
`public sealed record ToolAuthorization(bool IsAllowed, bool RequiresHumanApproval, string Reason);`
ve `BoundedAutonomyGate.cs:95` — `if (toolAuthorization is { IsAllowed: false })`. Gate,
kendisine verilen nesnenin `Authorize(...)` çıktısı olduğunu **doğrulayamaz**; `Evaluate`'in
registry'yi kendisi sorguladığı bir overload **yoktur** (`Evaluate` imzasında `CapabilityRegistry`
tipi geçmiyor — `BoundedAutonomyGate.cs:68-75`).

**Etki:** P7 (sınırlı özerklik) doğrudan düşer. ADR-0001 §6.1(2)'nin ENS'in *dar deltası* olarak
ilan ettiği tek şey — "Pack'in deklaratif izinleri **doğrudan** Gate'e beslenir" — bir
**konvansiyona** indirgenir. `CapabilityRegistry.cs:13-21` bu bağı ENS'in prior-art'a karşı iki
katkısından biri sayar; bağ zorlanmıyorsa katkı iddiası da zayıflar (Madde IX).

**Şiddet: KRİTİK.** Register ile aynı fikirdeyim. Dahası, W4a `…SCHEDULER.md` W15'ten
**daha güçlüdür**: W15 bir *reddi* aklamak için önce gerçek bir `Authorize` çağrısı gerektirir
(`denied with { IsAllowed = true }`); W4a hiçbir registry etkileşimi gerektirmez — yetki
**yoktan** üretilir. Register ikisini ayrı satırlarda listeliyor ama bu ilişkiyi göstermiyor:
**W15, W4a'nın özel bir hâlidir.** W4a kapatılırsa W15 de kapanır; tersi doğru değildir.

**Kapanış (bu raporun talebi):** `ToolAuthorization`'ın ctor'u `internal` olmalı ve yalnızca
`CapabilityRegistry` üretebilmeli; ya da `Evaluate`'e `(CapabilityRegistry registry, string toolName)`
alan bir overload eklenip `ToolAuthorization` parametreli hâli `[Obsolete]` yapılmalı. Kriptografik
imza gerekmez — **tip düzeyi kapatma yeterlidir** ve ADR-0001'in "imzalı token mı?" borcunu
(DEFECT-REGISTER §8.1) beklemeye gerek yoktur. Bunu ayrıca kaydediyorum: §8.1 bu kusuru
"mimari karar bekliyor" diye açık bırakıyor, oysa **ucuz ve tam bir çözümü vardır.**

### 4.2 `W4b` (FINDING) — yetkisiz araç ile "yalnızca riskli" karar aynı gate durumuna düşüyor

`GateDecision.Blocked` iki ayrı olguyu temsil ediyor: (i) hiçbir etkin Pack bu araca izin
vermiyor, (ii) InfoNeed blok eşiğini aştı. `Blocked`'ın kendi tanımı ise
(`BoundedAutonomyGate.cs:48`) *"insan onayı olmadan icra edilemez"* — yani **onayla icra
edilebilir**. Sonuç: bir allowlist ihlali, bir onay istemine indirgenebiliyor. Onay veren insan
`Decision` alanına bakarak ikisini ayırt edemez; yalnızca `Reason` metni farklıdır ve metin
makine tarafından ayrıştırılabilir bir sözleşme değildir.

**Etki:** P7'nin karar mercii insandır; insana verilen sinyal tipi bilgi kaybediyor. Doğru kapanış
ayrı bir `GateDecision.Unauthorized` üyesidir — **ama** `BoundedAutonomyGate.cs:126`'daki
`decision < GateDecision.Blocked` karşılaştırması enum'un sayısal sırasına bağlı olduğu için
(`…SCHEDULER.md` W24) yeni üye eklemek sessiz bir regresyon riski taşır. İki bulgu birbirine
kilitlenmiş durumda; sırayla ele alınmalı.

**Şiddet: FINDING (kod hatası değil).** Register ile aynı fikirdeyim.

### 4.3 `W4c` (HOLDS) — registry reddi, sayılar ölçülemez olsa bile blokluyor

`stake = NaN`, `confidence = null`, `conformanceDeficit = NaN` ile bile yetkisiz araç `Blocked`
alıyor: yetkisiz-araç dalı (`BoundedAutonomyGate.cs:95`) girdi kapısından (`:103`) **önce** geliyor.
Ve `GateResult.InfoNeed` NaN olarak dönüyor — **sahte bir sayı uydurulmuyor** (Madde X).
Bu doğru bir tasarım kararıdır ve dürüstçe kaydedilmelidir. (NaN'ın korunan sınırın dışına
sızması ayrı bir bulgudur: `…SCHEDULER.md` W17.)

---

## 5. W5 — Adapter registry: downcast, "en az bir adapter" değişmezi, yinelenen `AdapterId`

**Bu bölümün ana tezi:** AUDIT §5.2'de bulunan kusur, **bir örnek olarak** kapatıldı
(`CapabilityPack` → `FrozenSet`), **bir sınıf olarak** kapatılmadı. Aynı desen komşu dosyada,
`LlmAdapter.cs:178-188`'de, düzeltilmeden duruyor.

### 5.1 `W5a` — adapter listesi downcast edilebilir `List`; yönlendirme tek satırda ele geçiriliyor

```csharp
// LlmAdapter.cs:178   private readonly IReadOnlyList<ILlmAdapter> _adapters;
// LlmAdapter.cs:183   _adapters = adapters.ToList();      <-- arkasındaki nesne CANLI List<T>
// LlmAdapter.cs:188   public IReadOnlyList<ILlmAdapter> Adapters => _adapters;

((List<ILlmAdapter>)registry.Adapters).Insert(0, new W2_LyingAdapter());

registry.Resolve(LlmTier.Critical).AdapterId;      // "liar"
registry.Resolve(LlmTier.Operational).AdapterId;   // "liar"
```

**Reflection gerekmez.** `Resolve` (`:196-198`) ilk eşleşeni döndürdüğü için 0. indise
yerleştirilen bir adapter **tüm tier'ların** çözümünü ele geçirir — Critical kararlar dahil.

**Etki:** P5 (dikkat/kaynak tahsisi) ve P6. Hangi modelin ürettiği bilgisi (`LlmResponse.ModelId`)
proof-trace'in audit substratıdır (`LlmAdapter.cs:29-32`); yönlendirme ele geçirilirse bu
substrat da yanlış olur. W4a'nın aksine bir *karar* değil, kararın *icra aracı* ele geçirilir.

**Şiddet: Yüksek.** Register (§3.5) ile aynı fikirdeyim.

### 5.2 `W5b` — "en az bir adapter" değişmezi inşadan SONRA silinebiliyor

```csharp
// LlmAdapter.cs:184-185  ctor: if (_adapters.Count == 0) throw ... "modelsiz değil"
((List<ILlmAdapter>)registry.Adapters).Clear();
registry.Adapters;                              // boş
registry.Resolve(LlmTier.Operational);          // InvalidOperationException
```

Ctor'da zorlanan değişmez, ctor dışında korunmuyor.

**Şiddet: Yüksek — ama W5a ile AYNI KUSURDUR.** Register bunları iki ayrı satır olarak
sayıyor; tek bir düzeltme (`_adapters = adapters.ToImmutableArray()`) ikisini de kapatır.
W5a, W5b ve W5c aslında **tek bir kusurun üç belirtisidir.** Kusur sayımının bunu yansıtması
gerekir; aksi hâlde "68 kusur" sayısı düzeltme işinin büyüklüğünü **abartır** (DEFECT-REGISTER
§7 bunu zaten kabul ediyor: 6 mimari karar 33 kusuru kapatır).

### 5.3 `W5c` — `null` adapter, "model-agnostik ama modelsiz değil" güvencesini geçiyor

`new LlmAdapterRegistry(new ILlmAdapter[] { null! })` ctor'ı geçiyor (Count == 1), hata
`Resolve` anında ve **yanlış tipte** (`NullReferenceException`) yüzeye çıkıyor. Ctor
**kardinaliteyi** doğruluyor, **içeriği** değil.

**Şiddet: Düşük.** Register ile aynı fikirdeyim.

### 5.4 `W5d` — `CanHandle` kendi beyanıdır; modeli yalnızca KAYIT SIRASI belirliyor

**Register bu bulguyu hiç listelememiş** (bkz. §9). Oysa adapter bölümünün en önemli bulgusu
budur.

```csharp
var registry = new LlmAdapterRegistry([ new W2_LyingAdapter(),
                                        new EchoLlmAdapter("deepseek-r1", LlmTier.Critical) ]);
registry.Resolve(LlmTier.Critical).AdapterId;   // "liar"
// CompleteAsync -> InvalidOperationException: arkada sağlayıcı yok
```

`Resolve` (`LlmAdapter.cs:194-202`) kayıtlı adapter'ları sırayla gezer ve `CanHandle(tier)`
diyen **ilkini** döndürür. Ne sıralama, ne yetkinlik doğrulaması, ne fallback, ne post-condition
kontrolü var. `CanHandle` tamamen adapter'ın **kendi beyanıdır**.

**Etki:** ADR-0001 §5.3'ün iddiası *"yüksek InfoNeed → daha **güçlü** reasoning modeli"*dir
(`LlmAdapter.cs:10-13` bunu ENS'in AIOS'a karşı ayırt edici katkısı olarak sunuyor: "AIOS'ta
scheduler boştur; ENS onu VOI'ye bağlar"). Kod bu iddianın **yarısını** gerçekleştiriyor:
InfoNeed → tier eşlemesi var (ve `LlmTierSelector` artık guard'lı), ama tier → **güç** bağı
**hiç yok**. Registry, "bu tier'ı sürebilirim" diyen ilk adapter'ı seçer; gücü ölçmez,
doğrulamaz, karşılaştırmaz. Kötü niyetli bir adapter gerekmez — **dürüst ama zayıf** bir
adapter, `CanHandle(Critical) => true` dediği ve ilk kaydedildiği için tüm kritik trafiği
sessizce çeker.

**Şiddet: Yüksek.** Saldırgan gerektirmez, yalnızca talihsiz bir kayıt sırası gerektirir; ve
ENS'in bu katmandaki tek ayırt edici iddiasını yapısal olarak boşa çıkarır (Madde IX/X).
**Bu bulgunun DEFECT-REGISTER'da hiç bulunmaması, sicilin mekanik türetiminin gerçek bir
zaafıdır.**

### 5.5 `W5e` — `null` `LlmResponse` geçiyor ve proof-trace substratını yok ediyor

Port hiçbir post-condition zorlamıyor: `Task.FromResult<LlmResponse>(null!)` dönen bir adapter'ın
yanıtı sessizce çağırana ulaşıyor. `LlmResponse.ModelId` — "hangi model, ne üretti" — kaybolur.

**Şiddet: Orta**, dürüst kayıtla: `LlmAdapter.cs:28-30` proof-trace emitter'ın **henüz kodlanmadığını**
açıkça yazıyor, dolayısıyla bugün somut bir P6 ihlali yaşanmıyor. Bu, **gelecekteki** bir
tuzaktır. Register bunu hiç listelememiş.

### 5.6 `W5f` — iptal edilmiş token yine de "başarılı" yanıt üretiyor

**Bu testi bir DEFECT olarak kabul ETMİYORUM ve sınıflandırmasına itiraz ediyorum.**

Test, `EchoLlmAdapter`'ın (`LlmAdapterTests.cs:17-34`) iptal edilmiş bir `CancellationToken` ile
bile yanıt döndürdüğünü gösteriyor. Ama `EchoLlmAdapter` bir **test-double**'dır, üretim kodu
değildir — dosya başı bunu açıkça söylüyor (`LlmAdapter.cs:20`). Bir C# interface'i,
implementasyonun token'ı gözetmesini **zorlayamaz**; bu bir dil sınırıdır, bir kod kusuru değil.

Bulgunun geçerli hâli şudur: **port sözleşmesi, iptal semantiğini yazılı olarak tanımlamıyor**
(`ILlmAdapter.CompleteAsync` XML yorumu iptalden hiç söz etmiyor, `:109-110`). Bu bir
**FINDING**'tir, DEFECT değil. Doğru kapanış: sözleşmeye "implementasyon
`ThrowIfCancellationRequested` çağırmak ZORUNDADIR" yazmak + bunu bir **contract test**
ile tüm adapter'lara uygulamak.

**Şiddet: Düşük (FINDING olarak yeniden sınıflandırıldı).**

### 5.7 `W5g` — yinelenen `AdapterId` kabul ediliyor; denetim anahtarı injective değil

İki adapter aynı `"same-id"` ile kaydedilebiliyor; registry benzersizliği zorlamıyor.
`AdapterId`, `LlmResponse.ModelId` üzerinden proof-trace'in denetim anahtarına giriyor
(`LlmAdapter.cs:88-90`), dolayısıyla **"hangi model üretti" sorusunun tek anlamlı cevabı yok.**

**Etki:** P6/Madde VI. Bir denetim anahtarının **injective** olmaması, izlenebilirliğin sessiz
kaybıdır: kayıt var, ayırt etme gücü yok.

**Şiddet: Orta.** Register ile aynı fikirdeyim. Kapanış ucuz: ctor'da `AdapterId` benzersizliği
(`CapabilityRegistry.Register`'ın Pack adı için yaptığının aynısı — desen zaten repoda var,
komşu dosyaya uygulanmamış).

### 5.8 `W5h` (HOLDS) — çözümleme deterministik

Aynı registry üzerinde `Resolve` 50 tekrarda aynı adapter'ı veriyor (`ToList()` ile
materyalize edildiği için sıralama sabit). Bu bir kazanımdır — ama W5d ile birlikte okununca
ironiktir: **çözümleme deterministik olarak yanlış adapter'ı seçebilir.**

---

## 6. W6 — `LlmTierSelector`: NaN ve overload tuzağı

### 6.1 `W6a`, `W6b`, `W6c`, `W6f` (FIXED) — NaN kök nedeni bu dosyada gerçekten kapandı

Bu dört test, bu dalganın **tarihsel olarak en ilginç** parçasıdır: özgün hâllerinde
`AUDIT_DEFECT_*` idiler ve `LlmTierSelector`'ın `Guard`'a hiç bağlanmamış olduğunu
belgeliyorlardı. Aynı kusur bağımsız olarak `…SCHEDULER.md` §2'de **W11** adıyla da bulundu,
düzeltildi, ve testler **silinmedi — ters çevrildi**: eski kusur ifadesi yorumda aynen duruyor,
assertion'lar artık fail-closed davranışı zorluyor. Bu, doğru regresyon disiplinidir ve
kaydedilmelidir.

Düzeltmeyi kaynakta **bağımsız olarak doğruladım** (`LlmAdapter.cs:134-141`):

```csharp
Guard.NonNegativeFinite(complexThreshold,  ...);   // ÖNCE POLİTİKA
Guard.NonNegativeFinite(criticalThreshold, ...);
if (criticalThreshold < complexThreshold) throw ...;
Guard.NonNegativeFinite(infoNeed, ...);            // SONRA VERİ
```

Üç `double` girdinin üçü de `Guard`'dan geçiyor; `Guard.Finite` (`Guard.cs:70-77`) NaN ve
±Infinity'yi `ArgumentOutOfRangeException` ile reddediyor. Eşikler veriden **önce** doğrulanıyor —
bu, `…SCHEDULER.md` §3.3'ün "politika doğrulaması veriye bağlı olmamalı" itirazının tier
ayağındaki karşılığıdır ve doğru uygulanmış. `Guard.cs:22-32` nokta listesi de yediden **dokuza**
çıkarılmış ve `:34-42` sayımın yanlış olduğunu açıkça kaydetmiş. **Madde X'e uygun davranış.**

`W6f` ayrıca **neyin kapanmadığını** dürüstçe ayırıyor: sonlu ama absürt büyük eşikler hâlâ
kabul ediliyor (`SelectTier(1e9, 1e300, 1e301)` → `Operational`) ve her iki eşik 0 ise her karar
`Critical` oluyor. İkisi de **kalibrasyon borcudur** (ENS-3022 §Model 1), bir doğrulama kapısı
değil; uydurulmuş bir üst sınır (ör. "eşik ≤ 1000") teori dışı bir sayı olurdu (Madde IX).
Bu ayrımı yapan yorum doğrudur ve korunmalıdır.

> **Not — DEFECT-REGISTER §7 kalıp 4 ile çelişki:** sicil "eşik `0` = sessiz kapatma" kalıbını
> Yüksek şiddetli bir kök neden sayıyor. Burada eşik `0`, sistemi **kapatmıyor**; her kararı en
> pahalı tier'a yolluyor — yani muhafazakâr yönde. Kalıp 4 **evrensel değildir**; sicil onu
> koşulsuz uyguluyor. Şiddet, eşiğin yönüne bakılarak atanmalıdır.

### 6.2 `W6d` — konumsal iki-argümanlı çağrı, sessizce YANLIŞ overload'a bağlanıyor

İki overload var (`LlmAdapter.cs:126` ve `:156`):

```csharp
SelectTier(double infoNeed, double complexThreshold = 10, double criticalThreshold = 40)
SelectTier(double stake,    double? confidence, double = 10, double = 40)
```

C#'ın *better conversion target* kuralı: `double → double` (identity) daima
`double → double?`'dan iyidir. Dolayısıyla:

```csharp
LlmTierSelector.SelectTier(5.0, 0.9);                        // -> Complex      (BAĞLANAN)
LlmTierSelector.SelectTier(stake: 5.0, confidence: 0.9);     // -> Operational  (KASTEDİLEN)
```

Yazarın "stake / confidence" sandığı çağrı, `infoNeed = 5.0` ve `complexThreshold = 0.9`
olarak bağlanır. **Confidence hiç hesaba katılmaz.**

**Neden mevcut testler bunu yakalayamaz:** `LlmAdapterTests.cs`'teki tüm çağrılar
`stake:`/`confidence:` **adlandırılmış argüman** kullanıyor. Test paketi, kendisini bu
tuzaktan izole ettiği için tuzağı göremiyor — testlerin kendi yazım stili bir kör nokta üretmiş.

**`W6e` (HOLDS) — yön güvenli, ve bu dürüstçe kaydedilmeli.** 200 rastgele `(stake, conf)`
çiftinde yanlış bağlanan çağrının tier'ı **asla düşürmediği** gösterildi
(`(int)mistaken >= (int)intended`). Yapısal gerekçe: geçerli confidence `[0,1]` olduğu için
yanlış bağlanan eşik `≤ 1 ≤ 10`'dur ve `infoNeed = stake ≥ stake·(1−conf)`. Yani hata **pahalı**
eder, **tehlikeli** etmez.

**ŞİDDET — REGISTER İLE KISMEN AYNI FİKİRDE DEĞİLİM.** Register §4.2 bunu `C1` ile birlikte
**Orta** sayıyor ve *"bu projenin en öğretici kusuru"* diyor. Öğreticiliğine katılıyorum
(kernel demosunun kendi "dürüst bulgusu" tam da bu hataya kurban gitmişti — öz-eleştiri de
doğrulanmadan kanıt değildir). Ama şiddet, öğreticilikle ölçülmez: **W6e yönün güvenli
olduğunu ampirik olarak kanıtladı.** Bu bir derleme-zamanı API tasarım kusurudur, çalışma-zamanı
güvenlik kusuru değil. **Benim derecelendirmem: Düşük** (yön kanıtlı güvenli), tek istisnayla:
çağıran, confidence'ın hesaba katıldığına **inanıyorsa**, kalibrasyon çalışmalarının verisi
sessizce bozulur — bu bir P6 sorunudur ve düzeltmeyi zorunlu kılar.

**Kapanış:** overload'lar ayrı adlar almalı (`SelectTierFromInfoNeed` / `SelectTierFromStake`).
Breaking change'dir; DEFECT-REGISTER §8.5 bunu zaten sürüm kararı olarak açık bırakmış — doğru.

### 6.3 `W6g` (HOLDS) — eşik sınırları kapsayıcı, sonsuz InfoNeed artık reddediliyor

`SelectTier(10.0)` → `Complex`, `SelectTier(40.0)` → `Critical` (eşik değeri kısıtlayıcı
tarafta — fail-closed), `SelectTier(-0.0)` → `Operational`, `SelectTier(double.MaxValue)` →
`Critical` (sonlu ama devasa değer hâlâ geçerli — yanlış-pozitif yok). `±Infinity` artık
reddediliyor: **"sonsuz bilgi ihtiyacı" ölçülmüş bir bilgi ihtiyacı değil, bir taşma
belirtisidir.** Bu gerekçe doğrudur ve Madde X ile tutarlıdır.

---

## 7. W7 — `DecisionEntropy` (ENS-3021): bağımsız CMI doğrulaması ve zincir kuralının tautoloji oluşu

### 7.1 `W7a` (HOLDS) — formül sadakati 500 veri kümesinde BAĞIMSIZ olarak doğrulandı

Bu, tüm dalganın **en güçlü olumlu bulgusudur** ve doğru okunması için yönteminin anlaşılması
gerekir.

Kodun `LevelNoise` tanımı bir **farktır** (`DecisionEntropy.cs:44-49`):

```csharp
I = Math.Max(0, H(A|C) − H(A|C,Owner));
```

Test, beklenen değeri **koddan hiç çağırmadan**, ikinci bir kimlikten hesaplıyor
(`AdversarialWave_SecurityTests.cs:725-752` — kendi `H` ve `CondH` implementasyonları):

```
I(A;Owner|C) = H(A|C) + H(Owner|C) − H(A,Owner|C)
```

500 rastgele veri kümesinde (n ≤ 60, 1–4 context, 1–4 owner, 1–5 alternatif) iki yolun
`1e-9` toleransında **özdeş** olduğu gösteriliyor. Ayrıca `H(A|C) ≥ H(A|C,Owner)`
(koşullama entropiyi artıramaz) her iterasyonda doğrulanıyor.

**Bu nontrivial bir doğrulamadır.** İki ifade cebirsel olarak eşdeğerdir, ama kodun
`ConditionalEntropyGivenOwner` implementasyonu (`:55-71` — `(ContextKey, Owner)` çiftlerine
göre gruplama, `p(c,o)` ağırlıklı toplam) **yanlış yazılmış olabilirdi** (ör. yanlış ağırlık,
yanlış gruplama anahtarı). Test tam olarak bunu yakalardı ve yakalamadı. **ENS-3021'in
Shannon operasyonelleştirmesi kod düzeyinde sadıktır.**

*Metodolojik çekince (kendi kanıtımıza karşı):* birleşik anahtar, alternatif ile owner
stringlerinin **birleştirilmesiyle** kuruluyor. Ayırıcı zayıfsa `("a"+"b")` ile `("ab"+"")`
çakışabilir. Bu fuzz'da üreteç sabit biçimli semboller ürettiği için (`a0..a5`, `o0..o3`)
çakışma **yapısal olarak imkânsızdır** — yani sonuç geçerlidir. Ama üreteç genişletilirse bu
kırılganlık aktifleşir; testin yorumuna bir not düşülmelidir.

### 7.2 `W7c` (FINDING) — zincir kuralını TEST ETMEK tautolojidir

ENS-3021, `H(A|C) = I(A;Owner|C) + H(A|C,Owner)` zincir kuralını bir **iddia** gibi sunuyor.
Kodda `I` **tam olarak o farkın kendisi olarak tanımlanıyor** (`DecisionEntropy.cs:48`).
Dolayısıyla `lhs == rhs` testi hiçbir şeyi yanlışlayamaz: test bunu, istatistiksel olarak
tamamen anlamsız girdilerde (tek boş gözlem; tamamen `null` gözlem) bile özdeşliğin tuttuğunu
göstererek kanıtlıyor.

**W7a ile W7c'nin net ayrımı — bu ayrım kayda geçmelidir:**

| Soru | Cevabı veren | Sonuç |
|---|---|---|
| `LevelNoise` sayısı gerçekten I(A;Owner\|C) mı? | **W7a** (bağımsız CMI, 500 küme) | **EVET — doğrulandı** |
| Zincir kuralı özdeşliğini test etmek bir şey kanıtlar mı? | **W7c** | **HAYIR — tautoloji** |

Yani: **değer doğrudur, ama özdeşliği test etmek onu doğrulamaz.** Bir sayı kendi tanımından
değil artık (residual) olarak hesaplanıyorsa, o sayıyı içeren özdeşliği doğrulamak hiçbir şey
doğrulamaz. Doğrulama ancak W7a'nın yaptığı gibi **ikinci bir hesap yolundan** gelebilir.

> **DEFECT-REGISTER §6'nın W7c yorumuna kısmi itiraz.** Sicil W7c'yi "bu projede bulunan en
> değerli FINDING" ilan ederken *"hiçbir şeyi yanlışlamıyor"* diyor. Bu, **aynı dosyadaki
> W7a'yı görmezden geliyor**: formül sadakati bu dalgada bağımsız olarak **doğrulandı**.
> W7c'nin geçerli ve dar hâli şudur: *zincir kuralı özdeşliğinin kendisi bir test hedefi
> değildir.* Geriye kalan gerçek borç cebirde değil, **kestirimde**dir (W7d) — ve sicil bu iki
> şeyi birbirine karıştırıyor. ROADMAP'in "matematiksel olarak doğrulandı" ifadesi bu yüzden
> ikiye ayrılmalıdır: *ayrıştırma* doğrulandı (W7a); *özdeşliğin testi* kanıt değildir (W7c).

### 7.3 `W7b` (FINDING) — `Math.Max(0, ...)` kırpması ölü kod

`DecisionEntropy.cs:48`'deki kırpma hiçbir zaman devreye girmiyor: 300 rastgele veri kümesinde
`clampFired == 0`. Aynı kökten — ampirik dağılımda koşullama entropiyi artıramaz, dolayısıyla
artık zaten negatif olamaz. Kozmetik bir güvence: okuyucuya "burada bir risk vardı" izlenimi
verirken matematiksel olarak **imkânsız** olanı kırpıyor.

**Şiddet: FINDING / Düşük.** Register ile aynı fikirdeyim. (Not: kırpma kaldırılırsa
kayan-nokta yuvarlamasından doğan çok küçük negatif değerler açığa çıkabilir; doğru kapanış
kırpmayı silmek değil, **niçin var olduğunu** doğru yazmaktır.)

### 7.4 `W7f` — sahip kimliğindeki büyük/küçük harf farkı, TÜM attribution'ı ters çeviriyor

**Bu, ENS-3021'in ayırt edici iddiasını doğrudan vuran bulgudur.**

```csharp
// "Ali" ve "ali" — aynı insan, iki yazım
[("ctx","Ali","A"), ("ctx","ali","B")]  ->  LevelNoise = 1.0 ;  PatternNoise = 0.0
[("ctx","Ali","A"), ("ctx","Ali","B")]  ->  LevelNoise = 0.0 ;  PatternNoise = 1.0
```

Toplam `H(A|C)` her iki durumda da 1 bit — **aynı**. Ama ayrışım **tam tersine dönüyor.**

Kahneman'ın (Noise, 2021) ayrımı ENS-3021'in ithal ettiği asıl içeriktir: *level noise* =
kişiler arası sistematik fark, *pattern noise* = aynı kişinin kendi içindeki tutarsızlık.
Kod bu ayrımı tamamen `Owner` stringinin **ordinal kimliğine** dayandırıyor
(`DecisionEntropy.cs:59` — `GroupBy(o => (o.ContextKey, o.Owner))`, varsayılan string
karşılaştırıcısı = ordinal).

**Etki — neden bu sadece "yanlış sayı" değil:** iki gürültü türü **farklı müdahale** gerektirir.
Level noise → karar standardı/kalibrasyon eğitimi (kişiler arası hizalama). Pattern noise →
kontrol listesi/karar hijyeni (kişi içi tutarlılık). Attribution ters dönerse **yanlış müdahale**
seçilir. ENS-3021'in var oluş sebebi tam olarak bu teşhisi vermektir.

**ŞİDDET — REGISTER İLE AÇIKÇA AYNI FİKİRDE DEĞİLİM.** Register §4.1 bunu **Orta** ("yanlış
sonuç ama **izlenebilir**") sayıyor. **İzlenebilir değildir.** Çıktı iki `double`'dır; hiçbir
şey owner kimliğinin bölündüğünü göstermez, hiçbir exception atılmaz, hiçbir iz üretilmez.
Sicilin kendi "Orta" tanımı ("yanlış sonuç üretir **ama izlenebilir**") burada karşılanmıyor.
Üstelik tetikleyici gerçekçidir: `"Ali"` / `"ali"` / `"ali@firma.com"` / `"ALI"` gibi varyantlar
gerçek kurumsal veride kuraldır, istisna değil.

**Benim derecelendirmem: YÜKSEK.** Gerekçe: iz bırakmadan, ENS-3021'in tek ayırt edici
teşhisinin işaretini tersine çeviriyor; ve teoriden pratiğe giden yolu (hangi müdahale?)
sessizce bozuyor.

### 7.5 `W7d`, `W7e`, `W7h` — kestirim ve kimlik: `Observation` hiçbir kapıdan geçmiyor

`DecisionEntropy.Observation` (`:16`) düz bir `record`'dur; `ContextKey`, `Owner`,
`SelectedAlternative` **hiçbir `Guard`'dan geçmez**.

- **`W7d`** — tek gözlemden dağılım kestirilemez, ama plug-in (MLE) tahmincisi `0` döner ve
  sistem "tam tutarlı, gürültü yok" der. `minObservations` benzeri bir eşik **yok**.
  **Bu bir fail-open'dır:** plug-in entropi tahmincisinin yanlılığı **aşağı yönlüdür**
  (bu, bilgi kuramında standart bir sonuçtur; Miller-Madow türü düzeltmeler ve Paninski'nin
  entropi/karşılıklı bilgi kestirimi üzerine çalışması bu yanlılığı ele alır) — yani sistem
  gürültüyü **sistematik olarak olduğundan az** raporlar. Gürültü tespiti misyonu açısından
  yanlış yön budur.
- **`W7e`** — tamamen `null` gözlemler kabul ediliyor ve "kusursuz tutarlılık" raporlanıyor.
- **`W7h`** — `""`, `" "`, `"\t"` üç **ayrı** context evreni; her biri tek gözlem içerdiği için
  entropi 0, yani "her bağlamda tam tutarlı" görünüyor. **Register bunu hiç listelememiş.**

Üçü de **tek kök nedenin** belirtisi: kimlik alanları normalize edilmiyor ve örneklem
büyüklüğü hiç sorgulanmıyor.

**Şiddet: Orta (üçü birlikte).** Register W7d/W7e için Orta diyor — katılıyorum. W7h eksik.
Kapanış: (i) `Observation` ctor'ında kimlik normalizasyonu (trim + NFC + case-folding kararı
**açıkça** verilmeli), (ii) `minObservations` eşiği altında sayı yerine **"kestirilemez"**
dönmek — `0.0` dönmek bir ölçüm uydurmaktır (Madde X).

### 7.6 `W7g` (FINDING) — koşullu entropi normalize değil, alanlar arası kıyas anlamsız

10.000 farklı seçenek → `H = log2(10000) ≈ 13.29 bit`; iki seçenek arasındaki mükemmel
tutarsızlık → `1.0 bit`. Üst sınır `|A|`'ya bağlıdır ve ENS-3021 `H(A|C)` için bir
normalizasyon **tanımlamıyor**. Kod ikisini de aynı sayı tipiyle döndürüyor; "hangi departman
daha gürültülü?" sorusu bugün **cevaplanamaz**.

**Şiddet: FINDING.** Register ile aynı fikirdeyim. Bu bir kod kusuru değil, **ENS-3021'in teori
düzeyindeki açık borcudur** ve teori belgesine yazılmalıdır (bir kod yamasıyla kapatılamaz —
normalizasyon seçimi, ör. `H/log2|A|`, teoriden türetilmelidir).

---

## 8. W8 — `DecisionCapital` / `DecisionGravity`: girdi kapısı var, ÇIKTI kapısı yok

**Bu grubun tamamı DEFECT-REGISTER'da yoktur** (bkz. §10). Görevin kapsam listesinde de yoktu —
testleri okurken buldum. Dört kusur + bir gözlem içeriyor ve içlerinden biri (`W8d`) bu dosyanın
en ağır bulgularından biridir.

### 8.1 `W8a`, `W8b` — `Guard` yalnızca GİRDİYİ doğruluyor; çıktı taşabiliyor

```csharp
DecisionCapital.ReuseROI(1e308, double.Epsilon);   // -> +Infinity
DecisionCapital.DeltaCapital(1e308, -1e308);       // -> +Infinity
DecisionCapital.DeltaCapital(-1e308, 1e308);       // -> -Infinity
```

Her iki girdi de kapıyı geçiyor — `double.Epsilon` pozitif ve sonlu (`Guard.PositiveFinite`
geçer, `DecisionCapital.cs:49`), `1e308` sonlu (`Guard.NonNegativeFinite` geçer, `:50`) —
ama **bölme/çıkarma taşıyor**.

**Etki:** `Guard.cs:18-20`'nin ilan ettiği politika şudur: *"ölçülemeyen bir değer kernel'in
karar yollarına **giremez**."* Bu politika **giriş** sınırında tutuluyor, **çıkış** sınırında
tutulmuyor. `ReuseROI`'nin `+Infinity` çıktısı, ENS-3023'ün "ölü sermaye" ayrımını anlamsız
kılar: sonsuz ROI, ölçülmüş bir getiri değil, bir taşma belirtisidir — ve bu ayrımı W6g'nin
`+Infinity` InfoNeed için verdiği gerekçe zaten kabul etmiş durumda. **Aynı dosya ailesi, aynı
soruya iki farklı cevap veriyor.**

**Şiddet: Orta.** İç tutarsızlık argümanı bu bulguyu güçlendiriyor: kernel `+Infinity`'yi
girdide reddedip çıktıda üretiyor; bu değer bir sonraki hesabın girdisi olduğunda **orada**
reddedilecek — yani hata, üreten yerden uzakta patlayacak (`…SCHEDULER.md` §5'in `GateResult.InfoNeed`
bulgusuyla aynı sınıf). **Kapanış:** değer döndüren `Laws` metotları çıktılarını da
`Guard.Finite`'ten geçirmeli.

### 8.2 `W8d` — tek bir öz-beyan `confidence = 1.0`, TÜM yığını sıfırlıyor

```csharp
DecisionGravity.InfoNeed(1e12, 1.0);                        // 0
DecisionGravity.AttentionPriority(1e12, 1.0, 1.0);          // 0      -> dikkat kuyruğunun EN SONU
LlmTierSelector.SelectTier(stake: 1e12, confidence: 1.0);   // Operational  -> EN UCUZ model
BoundedAutonomyGate.Evaluate(1e12, 1.0, 1.0, false, 5_000, 60_000).Decision;  // Autonomous -> TAM OTONOMİ
```

Stake **ne olursa olsun** — 1 milyar, 1 trilyon — `confidence = 1.0` üç bağımsız mekanizmayı
aynı anda kapatıyor: P5 (dikkat tahsisi), model gücü seçimi, P7 (gate).

**Kök neden:** `Guard` bir **doğrulama** kapısıdır, bir **kalibrasyon** kapısı değildir —
`Guard.cs:45-51` bunu dürüstçe ilan ediyor. `confidence` çağıranın **öz-beyanıdır** ve
`[0,1]` aralığında olması dışında hiçbir koşula tabi değildir. `1.0` beyan eden bir çağıran,
`InfoNeed = stake × (1 − 1.0) = 0` üzerinden bütün yığını sessizce devre dışı bırakır.

**ŞİDDET — REGISTER İLE AYNI FİKİRDE DEĞİLİM.** Sicil §4.2 bu olguyu (`…SCHEDULER.md` W7
üzerinden) **"Orta (kabul edilmiş borç)"** sayıyor. İki itirazım var:

1. **"Kabul edilmiş borç" bir şiddet indirimi değildir.** Bir kusuru dürüstçe ilan etmek onu
   *dürüst* yapar, *hafif* yapmaz. Sicilin kendi §0'ı bu ayrımı savunuyor ("yeşil test paneli
   sağlık değil, envanter"); §4.2'de aynı ilke çiğneniyor.
2. **W8d, SCHEDULER W7'den daha geniştir.** W7 yalnızca gate ayağını gösteriyordu; W8d üç
   katmanın **aynı anda** çöktüğünü gösteriyor. Tek bir sayı, üç ayrı güvenceyi kapatıyorsa
   bu "sınır durumu" değil, **tek nokta arızasıdır**.

**Benim derecelendirmem: KRİTİK** — saldırgan gerekmez, özel yetki gerekmez, iz bırakmaz ve
P5 + P7'yi birlikte düşürür. Bunun bir "kalibrasyon borcu" olarak çerçevelenmesi, borcun
**büyüklüğünü** gizliyor: kalibre edilmemiş bir `confidence`, ENS'in sınırlı-özerklik
mimarisinde **ana giriş kapısıdır**.

**Kapanış (kısmi, bugün yapılabilir):** `confidence = 1.0`'ın tam eşitliği, stake belirli bir
eşiğin üstündeyken **reddedilmeli** ya da en azından gate'te `NotifyHuman`'a düşürülmelidir.
"Hiç belirsizlik yok" beyanı, yüksek stake'te tanım gereği kalibre edilemez bir iddiadır.

### 8.3 `W8c` (FINDING) — doküman "mutlak öğrenme" diyor, kod negatifi reddediyor

`DecisionCapital.cs:15` XML yorumu: `value(d) = |Learning(d)| × attribution_confidence(d)`.
Kod ise negatif `Learning`'i **mutlak değere çevirmiyor**, `ArgumentOutOfRangeException`
atıyor (`:29`, `Guard.NonNegativeFinite`). Ham (işaretli) öğrenme deltası taşıyan bir çağıran,
dokümanın vaat ettiği büyüklüğü değil, bir hata alır.

**Şiddet: FINDING / Düşük.** Fail-closed olduğu için tehlikeli değil; ama doküman ile kod aynı
şeyi söylemiyor. `| · |` sembolü ya yorumdan kaldırılmalı ya da kodda uygulanmalıdır.
(Register bu bulguyu da listelememiş.)

### 8.4 `W8e` (FIXED), `W8f`, `W8g` (HOLDS) — bu bölümdeki kazanımlar

- **`W8e`:** AUDIT §5.6 kapanışı tutuyor — `deficit = 1e9`, `deficit = 1.0` ile **aynı** sonucu
  veriyor (`Guard.NormalizedDeficit` kırpması, `Guard.cs:135-139`); negatif deficit 0'a
  kırpılıyor; NaN **reddediliyor** (kırpılmıyor — *"ölçülemeyen bir açık, 'açık yok' demek
  değildir"*, `:133`). Bu gerekçe doğrudur.
- **`W8f`:** `DecisionGravity` ve `DecisionCapital`, NaN / ±Infinity / negatif / `[0,1]` dışı
  confidence / sıfır maliyet — hepsini reddediyor. Mekanik olarak tarandı.
- **`W8g`:** `confidence = null` **maksimum belirsizlik** anlamına geliyor, sıfır InfoNeed değil
  (`InfoNeed(100, null) == 100`, tier `Critical`). Muhafazakâr yön doğru: commit edilmemiş karar
  en yüksek dikkati alır.

> **W8g ile W8d yan yana konduğunda tasarımın asimetrisi görünür oluyor:** *bilgi yokluğu*
> (`null`) muhafazakâr yorumlanıyor, ama *mükemmel bilgi iddiası* (`1.0`) hiç sorgulanmadan
> kabul ediliyor. Sistem, "bilmiyorum" diyene güvenmiyor; "her şeyi biliyorum" diyene tamamen
> güveniyor. **Doğru şüphecilik yönü tam tersidir.**

---

## 9. SALDIRDIM, KIRILMADI (`AUDIT_HOLDS_*` / `AUDIT_FIXED_*`)

Dürüstlük gereği — bunlar gerçek kazanımlardır ve bir denetim raporunun yalnızca kusur
listelemesi, kendi başına bir manipülasyon türüdür.

| İddia | Nasıl sınandı | Sonuç |
|---|---|---|
| Ordinal karşılaştırma benzer-görünümlü adlara karşı fail-closed | 11 varyant (homoglif, ZW*, case, whitespace, NUL, RTL, NBSP) | **Sağlam** (`W2a`) |
| NFD biçimi, NFC kaydını açmıyor | NFC/NFD çifti + `Normalize()` karşılaştırması | **Sağlam** (`W2b`) |
| `Disable` bir kısıtı düşüremiyor (F3 regresyonu) | katı Pack devre dışı + gevşek Pack izinli | **Kısıt korunuyor** (`W1f`) |
| Kısıt kümesi monoton | `Unregister`/`Remove`/`Clear` yokluğu **reflection ile** tarandı | **Sağlam** (`W1g`) |
| İzin kümesi downcast edilemiyor (AUDIT §5.2) | `HashSet` cast + çağıran koleksiyonunu sonradan değiştirme | **Kapalı** (`W3a`, `W3b`) |
| `Packs` koleksiyonu downcast edilemiyor | `List` cast + `ICollection.Add` | **Sağlam** (`W3d`) |
| `Authorize`, Pack yokken/boşken fail-closed | boş registry + boş Pack | **Sağlam** (`W3e`) |
| Registry reddi, sayılar ölçülemezken bile blokluyor | `stake = NaN`, `confidence = null` | **Blocked**, sahte sayı uydurulmadı (`W4c`) |
| Adapter çözümlemesi deterministik | 50 tekrar | **Kararlı** (`W5h`) |
| NaN eşik/InfoNeed artık reddediliyor (W11 düzeltmesi) | üç girdi × NaN/±Infinity; kaynak satır satır teyit | **Kapalı** (`W6a`, `W6b`, `W6c`, `W6f`) |
| Overload tuzağının **yönü** güvenli | 200 rastgele `(stake, conf)`; `mistaken ≥ intended` | **Yalnızca pahalı eder** (`W6e`) |
| Eşik sınırları kapsayıcı, `±Infinity` InfoNeed reddediliyor | ULP/sınır değerleri + `double.MaxValue` | **Fail-closed** (`W6g`) |
| **`LevelNoise` gerçekten I(A;Owner\|C) mi** | **500 veri kümesi, BAĞIMSIZ CMI hesabı** | **Formül sadık** (`W7a`) |
| Sınırsız `ConformanceDeficit` dikkat kuyruğunu ele geçiremiyor | `1e9` vs `1.0`, negatif, NaN | **Kapalı** (`W8e`) |
| Gravity/Capital her ölçülemez girdiyi reddediyor | NaN/±Inf/negatif/aralık dışı, mekanik tarama | **Sağlam** (`W8f`) |
| `null` confidence = maksimum belirsizlik | InfoNeed + tier | **Muhafazakâr yön doğru** (`W8g`) |

**En değerli üç kazanım:**

1. **`W7a`** — ENS-3021'in Shannon operasyonelleştirmesi, ikinci bir hesap yolundan doğrulandı.
   Bu dalganın teoriye en somut katkısıdır.
2. **`W1f` + `W1g`** — kısıt monotonluğu yalnızca test edilmedi, **yapısal olarak** ispatlandı
   (kısıt kaldıracak bir API yüzeyi yok). Tek deliği W2c'dir ve o da işaretlendi.
3. **`W6a-c-f`** — W11 düzeltmesi tuttu ve `Guard.cs:34-42` sayımının yanlış olduğunu **kendisi
   kaydetti**. Bir kapanış iddiasının yanlışlanıp düzeltilmesi, Madde X'in tam olarak istediği
   davranıştır.

---

## 10. Şiddet tablosu — DEFECT-REGISTER ile BAĞIMSIZ karşılaştırma

DEFECT-REGISTER §9 kendi sınırını dürüstçe yazıyor: *"Şiddet sütunu bağımsız değil. Ben atadım;
denetleyen ajanlar atamadı… bu sınıflandırma bağımsız bir context tarafından gözden
geçirilmelidir."* Bu bölüm o gözden geçirmedir.

### 10.1 Sicilin sayımı EKSİK — bu dosyada 8 bulgu hiç listelenmemiş

Sicil §1, `AdversarialWave_SecurityTests.cs` için **19 DEFECT + 4 FINDING** diyor. Testleri tek
tek saydım:

| | Sicil | Bu denetim | Fark |
|---|---|---|---|
| `AUDIT_DEFECT_*` | 19 | **26** | **+7** |
| `AUDIT_FINDING_*` | 4 | **5** | **+1** |
| `AUDIT_FIXED_*` | (sayılmamış) | 8 | — |
| `AUDIT_HOLDS_*` | (sayılmamış) | 12 | — |
| **Toplam test metodu** | — | **51** (69 test vakası, iki `[Theory]` genişletilmiş) | — |

**Sicilde hiç görünmeyen bulgular:** `W5d`, `W5e`, `W5f`, `W7h`, `W8a`, `W8b`, `W8d` (DEFECT) ve
`W8c` (FINDING). Bunların arasında **`W5d`** (§5.4 — ADR-0001 §5.3'ün "güçlü model" iddiasını
yapısal olarak boşa çıkarıyor) ve **`W8d`** (§8.2 — tek bir `confidence = 1.0` üç katmanı birden
kapatıyor) vardır; ikisi de sicilin "Kritik/Yüksek" kutularına girmesi gereken bulgulardır.

**Sonuç:** proje geneli toplam **68 → 75 DEFECT** ve **8 → 9 FINDING** olarak düzeltilmelidir.
Sebep, sicilin kötü niyeti değil **yöntemi**: mekanik türetme, test adlarının **eksiksiz**
okunduğunu varsayıyor ve `W8` grubu tümüyle atlanmış. Bu, sicilin kendi §0'ında ilan ettiği
sınırın ("bu bir indekstir, analiz değildir") somut bedelidir.

### 10.2 Katılmadığım şiddet atamaları

| ID | Sicil | **Benim** | Gerekçe |
|---|---|---|---|
| **W3c** | Kritik | **Düşük (kod) + Yüksek (iddia)** | Reflection **tam güven** gerektirir; o yetkiye sahip saldırgan zaten `Register` çağırabilir. Sicil §2 "saldırgana özel yetki gerekmiyor" başlığı altına koymuş — **yanlış**. Üstelik §8.3 aynı kusuru "kapsam dışı olabilir" diyor: **iç çelişki**. Kalıcı olan kusur, `CapabilityRegistry.cs:41`'in mutlak "gerçekten sabittir" iddiasıdır (Madde X). |
| **W2d** | Düşük ("kozmetik") | **Yüksek** | `Reason` metni dekoratif değil; P7 altında **insana gösterilen tek bağlamdır**. RTL-override, onay isteminde okunan cümleyi tersine çevirebilir. Bu, onay mekanizmasının bütünlüğüne saldırıdır — kozmetik değil. |
| **W7f** | Orta ("izlenebilir") | **Yüksek** | **İzlenebilir değil**: çıktı iki `double`, hiçbir iz/exception yok. Sicilin kendi "Orta" tanımı karşılanmıyor. ENS-3021'in tek ayırt edici teşhisinin (level vs pattern noise) **işaretini** ters çeviriyor → yanlış müdahale seçilir. Tetikleyici gerçekçi (`"Ali"`/`"ali"`). |
| **W8d** (sicilde `W7`/SchedulerGate olarak) | Orta ("kabul edilmiş borç") | **Kritik** | "Kabul edilmiş borç" bir şiddet indirimi **değildir** — sicilin kendi §0 ilkesi. Tek sayı, üç katmanı (P5 + tier + P7) birlikte kapatıyor: tek nokta arızası. |
| **W2c** | Kritik (koşulsuz) | **Yüksek bugün / Kritik marketplace altında** | Testin **kendi yorumu** koşulu yazmış (satır 238-239); sicil koşulu düşürmüş. Bugün kernel'de araç-adı dispatch'i yok, dolayısıyla doğrudan yetki yükseltmesi değil. Bulgunun koşulunu gizlemek denetimin güvenilirliğini düşürür. |
| **W1d** | Kritik (`W1a–W1d` tek satırda) | **Orta** | Yönü **muhafazakâr**: Pack ölü doğar (fail-*closed*). Zarar, izinsiz icra değil, açıklanamayan arıza. W1a/W1b/W1c ile aynı kutuya konması Kritik sınıfını şişiriyor. |
| **W6d** | Orta | **Düşük** | `W6e` yönün güvenli olduğunu **ampirik olarak kanıtladı** (200 örnek, `mistaken ≥ intended`). Derleme-zamanı API kusuru, çalışma-zamanı güvenlik kusuru değil. (Öğreticiliği yüksek — ama şiddet öğreticilikle ölçülmez.) |
| **W2e** | Orta | **Düşük** | W2c/W2d ile **aynı kök nedenin** üçüncü belirtisi (karakter-sınıfı doğrulaması yok). Ayrı kusur sayılması envanteri şişiriyor. |
| **W5f** | (Orta, DEFECT) | **FINDING / Düşük** | Kanıt tümüyle bir **test-double**'ın (`EchoLlmAdapter`) davranışı hakkında. Bir C# interface'i iptal gözetimini zorlayamaz — bu dil sınırıdır, kod kusuru değil. Geçerli hâli: **port sözleşmesi iptal semantiğini tanımlamıyor.** |

### 10.3 Katıldığım atamalar

`W4a` (Kritik), `W1a`/`W1b`/`W1c` (Kritik — özellikle W1b'nin yanlış-onay döngüsü),
`W1e` (Orta), `W2f` (Orta), `W5a`/`W5b` (Yüksek), `W5g` (Orta), `W7d`/`W7e` (Orta),
`W4b`/`W7b`/`W7c`/`W7g` (FINDING). Bunlarda sicilin değerlendirmesi bağımsız olarak da
savunulabilir.

### 10.4 Sicilin kök-neden tablosuna (§7) iki itiraz

1. **Kalıp 4 ("eşik `0` = sessiz kapatma") evrensel değildir.** `LlmTierSelector`'da her iki
   eşik `0` ise her karar `Critical` olur — yani **en pahalı**, en muhafazakâr yön (`W6f`).
   Aynı sözdizimsel kalıp, farklı yönlerde farklı şiddettedir. Şiddet, kalıptan değil
   **yönden** türetilmelidir.
2. **Kalıp 6 ("canlı koleksiyon dönüyor") bu dosyada 3 kusur olarak sayılmış, aslında 1'dir.**
   `W5a`, `W5b`, `W5c` tek bir satırın (`LlmAdapter.cs:183`) üç belirtisidir;
   `ToImmutableArray()` üçünü birden kapatır. Sicil §7 bu birleştirmeyi zaten savunuyor
   ("6 mimari karar 33 kusuru kapatır") ama §3.5/§5'te ayrı ayrı sayıyor. **Sayım ile strateji
   çelişiyor.**

### 10.5 Kök neden — bu dalganın kendi kalıbı

Yukarıdaki 26 kusurun büyük kısmı **tek bir meta-kalıba** indirgeniyor:

> **Bir kusur, ÖRNEK olarak kapatıldı; SINIF olarak kapatılmadı.**

- AUDIT §5.2 (canlı koleksiyon) `CapabilityPack`'te kapatıldı → `LlmAdapterRegistry`'de açık (`W5a-c`).
- AUDIT §5.1 (NaN körlüğü) yedi noktada kapatıldı → sekizinci nokta atlandı (W11/`W6a-c`),
  sonra bulundu ve kapatıldı; **çıktı sınırı** hâlâ açık (`W8a`, `W8b`).
- Kimlik normalizasyonu `Authorize`'da düşünüldü → `Register`'da (`W2c`, `W2f`),
  `Disable`'da (`W1a-c`), `Observation`'da (`W7f`, `W7h`) düşünülmedi.
- Benzersizlik `CapabilityRegistry.Register`'da zorlanıyor → `LlmAdapterRegistry`'de zorlanmıyor (`W5g`).

**Gereken karar:** her düzeltme için "bu kusurun **sınıfı** nedir ve o sınıfın repodaki diğer
örnekleri nerede?" sorusu zorunlu hâle getirilmeli. Bu, `Guard.cs:41`'in kendi dersinin
("kapattım iddiası bir SAYIMDIR ve sayımlar yanlışlanabilir") genelleştirilmiş hâlidir.

---

## 11. VERDICT ve sahibine talepler

### `wounded` — yetkilendirme yüzeyi gerçek ve ölçülebilir biçimde sağlamlaştı, ama **yetki nesnelerinin kendisi mühürsüz** ve kusur sınıfları yalnızca örnek düzeyinde kapatıldı.

Gerekçe üç maddede:

- **Kazanımlar gerçek.** İzin kümeleri artık değiştirilemez (`W3a`/`W3b`), kısıt monotonluğu
  yapısal olarak ispatlı (`W1f`/`W1g`), Ordinal karşılaştırma doğrulama tarafında fail-closed
  (`W2a`/`W2b`), NaN kök nedeni tier yolunda kapandı (`W6a-c-f`), ve ENS-3021'in formül sadakati
  bağımsız CMI hesabıyla doğrulandı (`W7a`). Bunlar küçük şeyler değil.
- **Ama yetkilendirme zinciri en zayıf halkasından kopuyor.** `ToolAuthorization` public bir
  record olduğu için Capability Registry'nin **tamamı tek satırda atlanabiliyor** (`W4a`).
  Registry ne kadar doğru çalışırsa çalışsın, çıktısı taklit edilebiliyorsa ADR-0001 §6.1(2)'nin
  "doğrudan beslenir" iddiası bir konvansiyondur, bir güvence değil.
- **Ve revocation yolu, operatöre yalan söylüyor** (`W1b`). Bir yönetişim sisteminde en kötü
  arıza biçimi budur: yanlış bir güvence, güvence yokluğundan tehlikelidir.

**Kusur sayısı (bu dosya): 26 DEFECT + 5 FINDING.** En kritik üçü: **`W4a`** (sahte yetki),
**`W1b`** (yanlış-onay döngüsü), **`W8d`** (tek `confidence` üç katmanı kapatıyor).

### Sahibine talepler (öncelik sırasıyla)

1. **`ToolAuthorization` mühürlenmeli** (`W4a`). Ctor `internal` olsun ve yalnızca
   `CapabilityRegistry` üretebilsin; ya da `Evaluate`'e `(CapabilityRegistry, string toolName)`
   overload'u eklensin. **Kriptografik imza gerekmez** — DEFECT-REGISTER §8.1 bunu "mimari karar
   bekliyor" diye açık bırakmış, oysa tip düzeyinde bugün kapatılabilir.
2. **`Disable` bir sonuç döndürmeli, `IsEnabled` üç durumu ayırt etmeli** (`W1a-d`).
   `bool`, üç durumlu bir soruyu (`NotRegistered`/`Enabled`/`Disabled`) temsil edemez.
   Kayıtlı olmayan bir ismi `Disable` etmek **sessizce başarılı olmamalı**.
3. **Kimlik tipi tanıtılmalı** (`W2c`, `W2e`, `W2f`, `W1a-c`, `W7f`, `W7h`). Tek bir
   normalize edilmiş `ToolName`/`OwnerId`/`ContextKey` tipi (trim + NFC + case kararı **açıkça**
   verilmiş + karakter-sınıfı doğrulaması) bu dosyadaki **9 kusuru** birden kapatır. Karar,
   ENS-4000 Sözlük'te tanımlanmalı ve teoriden türetilmelidir, koddan değil (Madde IX).
4. **`LlmAdapterRegistry` immutable olmalı** (`W5a`, `W5b`, `W5c`): `adapters.ToImmutableArray()`
   + `null` eleman reddi + `AdapterId` benzersizliği (`W5g`). Dört satır, dört kusur.
5. **`Resolve` yetkinliği doğrulamalı** (`W5d`). Bugün model seçimini **kayıt sırası** belirliyor;
   ADR-0001 §5.3'ün "yüksek InfoNeed → **daha güçlü** model" iddiası kodda karşılıksızdır.
   En azından tier→adapter eşlemesi açık ve doğrulanabilir olmalı, "ilk `true` diyen" olmamalı.
6. **`confidence = 1.0` yüksek stake'te sorgulanmalı** (`W8d`). "Hiç belirsizlik yok" beyanı,
   yüksek stake'te tanım gereği kalibre edilemez. En azından gate'te `NotifyHuman`'a düşmeli.
7. **`Laws` metotları ÇIKTILARINI da `Guard`'dan geçirmeli** (`W8a`, `W8b`). `Guard.cs:18-20`'nin
   politikası giriş sınırında tutuluyor, çıkış sınırında tutulmuyor.
8. **Üç doküman düzeltmesi (kod değişikliği gerektirmez, bugün yapılabilir):**
   - `CapabilityRegistry.cs:41` — "runtime'da **gerçekten sabittir**" mutlak ifadesi
     düzeltilmeli (`W3c`); reflection sınırı açıkça yazılmalı.
   - `AdversarialWave_SecurityTests.cs:27-29` — "hiçbir çıplak non-ASCII karakter YOKTUR"
     ifadesi **yanlıştır** (§0.3); ya düzeltilmeli ya da karakterler gerçekten escape'lenmeli.
   - `DecisionCapital.cs:15` — `|Learning|` ya yorumdan kaldırılmalı ya da kodda uygulanmalı (`W8c`).
9. **ENS-3021'e normalizasyon borcu yazılmalı** (`W7g`) ve **`minObservations` eşiği** teoriden
   türetilmeli (`W7d`). `0.0` döndürmek bir ölçüm uydurmaktır; "kestirilemez" ayrı bir sonuçtur
   (Madde X).
10. **DEFECT-REGISTER güncellenmeli:** toplam **75 DEFECT + 9 FINDING**; §10.2'deki dokuz şiddet
    ataması gözden geçirilmeli; §7 kalıp 4 ve 6 düzeltilmeli.

---

## 12. Bu raporun kendi yanlışlanabilirliği (Madde X)

**En zayıf halkam iki tanedir.**

**(1) Hiçbir test koşturulmadı** (§0.2). Bulguların dayanağı kaynak kodu okuma + C#/IEEE-754
semantiğidir. Somut çürütme koşulları:

| Test kırmızı yanarsa | Ne çürür |
|---|---|
| `AUDIT_DEFECT_W4a` | §4.1 tümüyle düşer; `record` ctor erişilebilirliği hakkında yanılmışım demektir — **bu raporun "en kritik" iddiası** düşer |
| `AUDIT_DEFECT_W1b` | §1.2 düşer; `IsEnabled`'ın kısa-devre semantiğini yanlış okumuşum demektir |
| `AUDIT_FIXED_W6a/b/c/f` | W11 düzeltmesi **tutmuyor** demektir → durum bu rapordan **daha kötü** |
| `AUDIT_HOLDS_W7a` | ENS-3021'in formül sadakati düşer → **koordinatör lehine en güçlü bulgum** çöker ve §7.2'nin W7c yorumuna itirazım geçersiz olur |
| `AUDIT_HOLDS_W6e` | W6d'yi "Düşük"e indirme gerekçem düşer; sicilin "Orta" ataması haklı çıkar |
| Herhangi bir `[Theory]` vakası | §0.3'teki kodlama bulgusu gerçekleşmiş demektir (dosya normalize edilmiş) |

**(2) Bu rapor ikinci elden türetildi** (§0.1). Testleri yazan ajanın gerekçesi kayıp; test
yorumlarını okuyup **yeniden inşa ettim**. Bir testin özgün niyetini yanlış anlamış olabilirim —
en muhtemel yer `W5f` (§5.6), çünkü orada özgün ajanın DEFECT sınıflandırmasına **karşı
çıkıyorum**; belki onun görmediğim bir gerekçesi vardı.

**Yapısal talep — bu üçüncü kez oluyor:** `AUDIT.md`, `…SCHEDULER.md` ve bu rapor, üst üste
**hiçbiri testleri koşturmadan** yazıldı. Denetim ajanlarına komut çalıştırma yetkisi verilmiyor
ve bu, denetim zincirinin **en büyük yapısal zaafıdır**. Bir kusur envanterinin tamamı
"çalıştırılmamış kanıt" üzerine kuruluysa, envanterin kendisi doğrulanmamış bir iddiadır
(Madde X, aynen `W7c`'nin eleştirdiği durum — **ve bu ironi kaydedilmelidir**).

**Kapanış şartı:** `dotnet test` koşturulup **tam çıktısı** repoya yapıştırılmalıdır. Derleme
hatası çıkarsa **o da yazılmalıdır**. Kırılan her `AUDIT_*` testi benim bir hatamdır ve onu
görmek isterim.

---

*ens-skeptic, 2026-07-26. Bu belge `DEFECT-REGISTER.md`'den **bağımsızdır**; şiddet atamalarında
çelişki hâlinde §10.2'deki gerekçelere bakın. Sicilin sayımı bu dosya için eksiktir (§10.1) ve
düzeltilmesi gerekir. Hiçbir test sonucu uydurulmadı; koşturulmayan hiçbir şey "geçti" diye
yazılmadı (SKR-041 emsali).*
