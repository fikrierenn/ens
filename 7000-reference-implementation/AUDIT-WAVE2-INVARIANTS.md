# AUDIT-WAVE2 — Üç Çekirdek Invariant'a Bağımsız Düşmanca Saldırı

| | |
|---|---|
| **Denetleyen** | `ens-skeptic` (dalga 2) — kodu yazan context'ten ayrı |
| **Denetlenen** | `Ens.Kernel/Domain/DecisionAggregate.cs`, `Ens.Kernel/ProofTrace.cs`, `Ens.Kernel/ActuationLayer.cs` (HEAD, 2026-07-26) |
| **Hedef iddialar** | (1) "Atom sınırı mühürlü" — ENS-2001 §Individuation · (2) "İzsiz türetim TEMSİL EDİLEMEZ" — Anayasa Madde VI · (3) "Hiçbir katman lifecycle'ı ATLAYAMAZ" — ADR-0001 §5.4 |
| **Yetki** | Anayasa Madde X (Yanlışlanabilirlik Ödevi) |
| **Üretilen kanıt** | `Ens.Kernel.Tests/AdversarialWave_InvariantTests.cs` — 22 test (13 DEFECT, 1 FINDING, 8 HOLDS) |

---

## 0. ÖNCE DÜRÜSTLÜK: `dotnet test` ÇALIŞTIRILAMADI

Görev açıkça *"`dotnet test` GERÇEKTEN çalıştır"* dedi. **Çalıştıramadım.** Bu context'e
verilen araç kümesi yalnızca dosya okuma/yazma/arama içeriyor; **Bash ya da herhangi bir
komut çalıştırma aracı etkin değil.** `AUDIT.md §0`'daki durumun aynısı tekrarlandı.

Bu yüzden, skill'in dürüstlük zorunluluğu gereği:

- **Bu raporda tek satır test çıktısı fabrike edilmedi.** Hiçbir yerde "şu test yeşil yandı"
  demiyorum.
- `AdversarialWave_InvariantTests.cs` **derlenmedi ve koşturulmadı.**
- **"Mevcut 199 test kırılmadı" iddiasını doğrulayamıyorum.** Üretim kodunda hiçbir değişiklik
  yapmadım (yalnızca yeni bir test dosyası ekledim), dolayısıyla davranışsal regresyon riski
  yok; ama **test projesinde bir derleme hatası olursa tüm suite düşer.** Bu, kapatılmamış
  tek gerçek risktir ve ilk yapılması gereken iş `dotnet test`'i koşturmaktır.
- Tüm bulgular **statik analiz + C#/.NET/IEEE-754 semantiğinden elle çıkarım**dır. Her
  bulgunun yanında güven derecesi var ve her biri **hangi testin kırılmasıyla çürüyeceği**
  yazılı.

> Kullanıcının orijinal itirazı (*"manipüle etmediğini nereden bileceğim"*) bu noktada hâlâ
> haklı: bu rapor da henüz **çalıştırılmış bir kanıt değil.** Farkı, kendini yanlışlayabilecek
> somut testler bırakması.

---

## 1. ÖZET

**13 yeni kusur, 1 yeni beyan-hatası, 8 sağlam çıkan iddia.**

Üç iddianın durumu:

| İddia | Verdict | Neden |
|---|---|---|
| (1) "Atom sınırı mühürlü: tek Owner, tek Purpose, açık Alternatives, tek Commitment" | **wounded** | Dört koşuldan **biri (tek Owner) hiç kodlanmamış**; "açık Alternatives" mührü replay kapısından **yeniden delindi** (R4). Purpose ve Commitment tekliği sağlam. |
| (2) "İzsiz türetim TEMSİL EDİLEMEZ" | **refuted (bu hâliyle)** | **Tamamen görünmez** bir proof-trace kurulabiliyor (P1); açıklama metni **uydurulabiliyor** (P3); **kendi kendini kanıtlayan** trace geçerli (P4); trace kanıtladığı şeye **bağlı değil** (L2). |
| (3) "Hiçbir katman lifecycle'ı ATLAYAMAZ" | **wounded** | State machine'in **içi** gerçekten sızdırmıyor (L5/L6/C1/C2 + AUDIT §4.3). Ama **girişi korumasız**: hiç commit edilmemiş, hatta hiç var olmamış bir karar için tam lifecycle koşturulabiliyor (L1) ve audit zaman damgaları uydurulabiliyor (L3). |

### En kritik bulgu

**`AUDIT_DEFECT_W2_R4` — replay yolunda `Alternatives` bir kopya değil, çağıranın canlı
listesine açılan bir penceredir.**

`DecisionAggregate.Apply` şunu yapıyor:

```csharp
Alternatives = e.Alternatives as ReadOnlyCollection<string>
               ?? new ReadOnlyCollection<string>(new List<string>(e.Alternatives));
```

ve yanındaki yorum şunu iddia ediyor: *"aggregate her zaman KENDİ değiştirilemez kopyasını
tutar."* **Bu yanlış.** `ReadOnlyCollection<T>` bir kopya değil, bir **görünüm**dür. Çağıran
kendi `List<string>`'ini bir `ReadOnlyCollection`'a sarıp olaya koyarsa `as` dalı tutar ve
aggregate **çağıranın canlı listesini** saklar:

```csharp
var backing  = new List<string> { "A", "B" };
var liveView = new ReadOnlyCollection<string>(backing);
var d = DecisionAggregate.Rehydrate(id, [framed, new AlternativesIdentified(liveView, []) {...}]);

backing.Add("HİÇ-DEĞERLENDİRİLMEMİŞ");        // aggregate durumu DIŞARIDAN değişti
d.Commit(owner, "HİÇ-DEĞERLENDİRİLMEMİŞ", 0.99, "harika");   // ve canlı yol bunu MEŞRU saydı
```

Bu tam olarak `AUDIT.md §5.3`'ün "düzeltme (2)" ile kapattığını ilan ettiği saldırıdır —
**replay kapısından aynen geri geliyor.** Düzeltme canlı yolda gerçek (`IdentifyAlternatives`
gerçek kopya alıyor), replay yolunda **kozmetik**.

> Güven: **çok yüksek** — `ReadOnlyCollection<T>`'un sarmalayıcı (kopyalayıcı değil) olduğu
> BCL'nin belgelenmiş davranışıdır. Bu testin kırılması bulgumu çürütür.

---

## 2. İDDİA (1): "ATOM SINIRI MÜHÜRLÜ"

### 2.1 Rehydrate düzeltmesi gerçek mi? — **Kısmen. Sıralama mührü gerçek, kimlik mührü yok.**

Görevin istediği sahte akışların **hepsini** denedim; sıralama/kardinalite ekseninde düzeltme
**gerçek** (`AUDIT_HOLDS_W2_R1`): ters sıra, eksik Framing, eksik Alternatives, tekrarlanan
Commitment, tekrarlanan Framing, `DecisionCommitted` olmadan `OutcomeObserved`, `Enact`
olmadan `Observe`, commitment sonrası Alternatives, null olay — **dokuzu da reddediliyor**,
sağlıklı akış geçiyor. Bu, `AUDIT_DEFECT_I1/I2` kapanışının gerçek olduğunu gösteriyor.

Ama **dört yeni yol açık**:

| # | Test | Kusur |
|---|---|---|
| R2 | `..._rehydrate_accepts_events_belonging_to_other_decisions` | `Rehydrate(id, history)` hiçbir olayın `Target`'ını `id` ile karşılaştırmıyor, `Emitter`'a bakmıyor. **Üç farklı karara ait olaylar, dördüncü bir kimlik altında tek "karar"a birleştirilebiliyor.** Aggregate sınırının bütünlüğü aggregate'in değil, event-store sorgusunun sorumluluğunda — bu, invariant'ın dışarı ihraç edilmesidir. |
| R3 | `..._replay_does_not_enforce_the_non_blank_alternative_guard` | Canlı yol *"Boş Alternative olamaz — adsız seçenek değerlendirilemez"* diye reddeder; replay yolu yalnızca `Count == 0`'a bakar. **`"   "` ve hatta `null` bir "seçenek"e commit edilmiş atom üretilebiliyor.** İki yol ayrışıyor → *"replay canlı yolla AYNI invariant'lardan geçer"* iddiası **yanlış**. |
| R5 | `..._unknown_event_types_are_silently_swallowed_by_the_fold` | `DomainEvent` public abstract record → dışarıdan türetilebilir. `Apply` ve `EnsureReplayInvariant` switch'lerinin `default` dalı **yok**: tanınmayan olay sessizce history'ye yazılır, duruma etki etmez. Axiom 2 ("durum = Event akışının fold'u") fold'un **total** olmasını gerektirir; şema evriminde eski okuyucu aynı akıştan farklı durum üretir ve **hiç bildirmez**. |
| R6 | `..._replay_ignores_timestamps_and_duplicate_event_ids` | (a) Lifecycle sırası **yalnızca liste konumuyla** zorlanıyor; `Timestamp` hiç okunmuyor → framing'den 10 yıl **önce** commit edilmiş karar geçiyor. (b) `EventId` tekilliği doğrulanmıyor → at-least-once teslimat yapan bir store kaydı sessizce çoğaltır. |

### 2.2 "Tek Owner" — **hiç kodlanmamış** (`AUDIT_DEFECT_W2_O1`)

§Individuation'ın dört koşulundan üçü gerçekten zorlanıyor. **Dördüncüsü temsil bile edilmiyor:**

- `DecisionAggregate`'in **`Owner` diye bir property'si yok**.
- Her metot ayrı bir `Identity` kabul ediyor ve **hiçbiri diğeriyle kıyaslanmıyor**: Alice
  çerçeveler, Bob alternatifleri belirler, Carol commit eder, Dave enact eder, Erin outcome
  gözler — **beş farklı aktör, tek "atom", sıfır itiraz.**
- Replay yolunda `DecisionCommitted.Owner` ile olayın `Emitter`'ı **bağımsız uydurulabiliyor**
  (Owner = Alice, Emitter = Bob) ve kimse sormuyor.

Demo/README'nin *"Atom sınırı MÜHÜRLENDİ: tek Owner, tek Purpose, açık Alternatives, tek
Commitment"* cümlesinin **ilk maddesi kodda karşılıksızdır.** Bu bir "borç" olarak da
işaretlenmemiş — dosyanın dürüst-sınırlar bloğunda geçmiyor. Kaynak dosyanın diğer yerlerdeki
dürüstlüğü göz önüne alındığında bu bir **gözden kaçma**, ama etkisi aynı: kodun karşılamadığı
bir mühür ilan ediliyor.

### 2.3 Sağlam çıkanlar

- Tek Purpose, tek Commitment, lifecycle sırası: replay'de de canlı yolda da **mühürlü**.
- `History` / `UncommittedEvents` downcast'i kapalı, `Alternatives` **canlı yolda** gerçekten
  kopyalanıyor.

---

## 3. İDDİA (2): "İZSİZ TÜRETİM TEMSİL EDİLEMEZ" — **çürüdü**

Bu, ENS'in en yüksek sesle söylediği iddia (ADR-0001 §5.5: *"onlarda audit-log sonradan eklenen
bir gözlemdir; ENS'te proof-trace ACTION'IN VAR-OLMA KOŞULUDUR"*). Dört ayrı yoldan kırıldı.

### 3.1 `AUDIT_DEFECT_W2_P1` — tamamen **görünmez** bir proof-trace kurulabiliyor

`string.IsNullOrWhiteSpace` yalnızca Unicode **whitespace** kategorilerini (Zs/Zl/Zp + birkaç
kontrol karakteri) yakalar. **FORMAT (Cf) ve NUL karakterleri whitespace değildir:**

| Karakter | Kategori | `IsWhiteSpace` | `Premise` guard'ı |
|---|---|---|---|
| U+200B ZERO WIDTH SPACE | Cf | `false` | **geçiyor** |
| U+FEFF ZERO WIDTH NO-BREAK | Cf | `false` | **geçiyor** |
| U+200E LEFT-TO-RIGHT MARK | Cf | `false` | **geçiyor** |
| U+202E RIGHT-TO-LEFT OVERRIDE | Cf | `false` | **geçiyor** |
| U+200D ZERO WIDTH JOINER | Cf | `false` | **geçiyor** |
| U+0000 NUL | Cc | `false` | **geçiyor** |
| U+00A0, U+2000, U+2009, U+202F, U+205F, U+3000, U+2028 | Zs/Zl | `true` | reddediliyor (`AUDIT_HOLDS_W2_P2`) |

Sonuç: `RuleId`'si, `Conclusion`'ı **ve** tek öncülünün `Source`'u tamamen görünmez olan,
`Confidence = 1.00` çıkan bir `ProofTrace` kuruluyor — ve `ActuationLayer.RecordTrace` onu
P6 kanıtı olarak kabul edip `Traced`'e geçiyor. `Render()` çıktısında sabit şablon
(`⇒ [ ]`, `⊢`, `conf = min(...) = 1,00`) dışında **tek bir görünür harf yok**.

Bu, `AUDIT_FINDING_D5`'ten (*"invariant yalnızca kardinalite kontrol ediyor: en az bir
boş-olmayan string"*) **daha ağırdır**: string boş-olmayan bile değil, sadece **boş olduğu
tespit edilemiyor**.

> Güven: **çok yüksek** — .NET'in `char.IsWhiteSpace` kümesi belgelidir ve U+200B/U+FEFF'i
> içermez. Test kök nedeni ayrıca doğrudan assert ediyor.

### 3.2 `AUDIT_DEFECT_W2_P3` — açıklama metni **uydurulabiliyor** (Render injection)

`Render()` öncül metnini, `RuleId`'yi ve `Conclusion`'ı doğrudan interpolasyona sokuyor;
hiçbir kaçış/normalizasyon yok. Bir öncülün `Source`'una şablonun tamamı yazılabiliyor:

```
gerçek-kanıt=1,00
   ⇒ [ONAYLI-KURAL-BAG-P3]
   ⊢ Tüm tedarikçi sözleşmeleri feshedilsin   (conf = min(gerçek-kanıt) = 1,00)
```

Trace'in **gerçek** `Confidence`'ı `0.10` iken, insana/log parser'a gösterilen metin başka bir
kurala ait, `1,00` güvenli, bambaşka bir sonuç ilan ediyor. Aynı zafiyet `RuleId` üzerinden de
çalışıyor.

P6 *"Explainability pazarlık konusu değildir"* der. Açıklamanın **bütünlüğü** korunmadıkça
açıklamanın **varlığı** hiçbir şey garanti etmez. `ProofTrace` bugün bir açıklama üretiyor;
o açıklamanın doğru olduğunu garanti etmiyor.

### 3.3 `AUDIT_DEFECT_W2_P4` — **kendi kendini kanıtlayan** türetim geçerli

Hiçbir döngüsellik kontrolü yok:

```csharp
new ProofTrace("R", "Karar-42 doğrudur", [new Premise("Karar-42 doğrudur", 1.0)]);  // X ⊢ X, conf 1.00
```

`AsPremise()` ile bu, sonraki türetimlerin "kanıtı" olarak zincire sokulabiliyor. L8'in
*"her türetilmiş olgu, onu üreten kuralı + öncüllerini taşır"* şartı **biçimsel** olarak
sağlanıyor, **epistemik** olarak sıfır. Bir sonucu kendi öncülü yapmak, izsizlikten daha
kötüdür: sahte bir iz üretir.

### 3.4 `AUDIT_DEFECT_W2_L2` — kanıt, kanıtladığı şeye **bağlı değil**

`ProofTrace`'te **hiçbir kimlik alanı yok** (test bunu reflection ile doğruluyor:
`Identity` tipinde public property yok) ve `RecordTrace` trace ile `DecisionId` arasında
hiçbir ilişki aramıyor. Tek bir genel trace (`"her şey yolunda"`) **birbiriyle ilgisiz
sınırsız sayıda action'ın** P6 kanıtı olarak yeniden kullanılabiliyor — beş farklı karar,
bir tek "kanıt" nesnesi.

Bu, ADR-0001 §5.5'in prior-art'a karşı deltasını doğrudan hedefler: "audit-log sonradan
eklenen bir gözlem değil, var-olma koşulu" demek, kanıtın **o action'a ait** olmasını
gerektirir. Bugün kanıt paylaşılabilir bir jetondur.

### 3.5 `AUDIT_FINDING_W2_P5` — kaynak yorumu L7 hakkında fazla söylüyor

`ProofTrace.AsPremise` yorumu: *"Confidence monoton azalır (min t-norm) — türetim zinciri
**uzadıkça güven düşer**, artamaz."*

`min` **idempotenttir**. 100 halkalı bir çıkarım zinciri kurdum: confidence 100 adım sonra da
`0.9`. Doğru ifade **"artmaz"**dır, "düşer" değil. Aynı idempotanslık, 1000 özdeş öncülün
korroborasyon üretmemesine rağmen `Render()`'da 1000 ayrı kanıt gibi görünmesine yol açıyor.

Kod L7'ye **sadık** (kusur kodda değil, beyanda). ENS-4025 §Failure'ın *"t-norm seçimi ileride
bir RFC gerektirebilir"* borcu tam olarak burada görünür hâle geliyor: zincir uzunluğunu
cezalandırmayan bir t-norm, uzun türetim zincirlerini ucuzlatır.

### 3.6 Sağlam çıkanlar

- `AUDIT_HOLDS_W2_P2`: gerçek Unicode whitespace'in **13 varyantı** (Zs/Zl + TAB/LF/CR) hem
  `Premise` hem `ProofTrace(ruleId)` hem `ProofTrace(conclusion)` için reddediliyor.
- `AUDIT_HOLDS_W2_P6`: girdi-aliasing kapalı, `List<Premise>` downcast'i `InvalidCastException`,
  `IList<Premise>.Clear()` `NotSupportedException`, `Confidence` yazılamaz. **AUDIT §5.2'nin
  ProofTrace kapanışı gerçek.**
- Aynı öncülün 1000 kez tekrarı confidence'ı **değiştirmiyor** (min idempotent) — görevin
  sorduğu saldırı boşa çıktı.

---

## 4. İDDİA (3): "HİÇBİR KATMAN LIFECYCLE'I ATLAYAMAZ"

### 4.1 State machine'in **içi** sağlam

- `AUDIT_HOLDS_W2_L5`: `(GateDecision)999` ve `(GateDecision)(-1)` gibi **tanımsız enum
  değerleri** `Blocked`'a düşüyor. Sınıf pozitif liste (`is Autonomous or NotifyHuman`)
  kullandığı için **fail-CLOSED**. Doğru desen; övgüyü hak ediyor.
- `AUDIT_HOLDS_W2_L6`: `ApplyGate` iki kez çağrılırsa ikincisi reddediliyor ve **yarım
  uygulanmış durum kalmıyor** (iki geçişten ilki reddedildiği için ikincisine sıra gelmiyor).
  Audit izi kirletilmiyor. `Blocked` terminalinden ikinci gate de uygulanamıyor.
- `AUDIT_HOLDS_W2_C1`: 64 bağımsız katman paralelde tam lifecycle'ı doğru tamamlıyor.
- `AUDIT_HOLDS_W2_C2`: paylaşılan katmanda 384 denemeye karşı 7 meşru geçiş — state machine
  ezici çoğunluğu reddediyor, durum alanı tanımlı enum değerinde kalıyor.

Reflection ile `State` alanına yazma (`AUDIT_DEFECT_E5`) hâlâ açık ve kapatılamaz; yeni bir
şey eklemiyorum.

### 4.2 Ama **girişi korumasız** (`AUDIT_DEFECT_W2_L1`) — en önemli yapısal bulgu

`ActionState.Planned`'ın tanımı: *"Decision commit edildi (ENS-2001), action planlandı."*
Ama `new ActuationLayer(id)` **hiçbir şey doğrulamaz**: verilen `Identity`'nin bir Decision'a
karşılık gelip gelmediği, o Decision'ın commit edilip edilmediği **sorulmuyor**.

```csharp
var neverCommitted = DecisionAggregate.Frame(actor, "sadece çerçevelendi");   // COMMIT YOK
var layer = new ActuationLayer(neverCommitted.Id);
layer.ApplyGate(...); layer.BeginActing(...); ... layer.Remember(...);        // Remembered
```

Hatta `new ActuationLayer(default)` — `DecisionId.Value == null` — ile **hiç var olmamış** bir
karar için de aynısı geçerli.

Yani *"hiçbir katman lifecycle'ı atlayamaz"* iddiası, lifecycle'ın **yalnızca ikinci yarısı**
için doğrudur. Birinci yarı (Framing → Alternatives → Commitment) **tümüyle atlanabilir**.
Dosyanın dürüst-sınır notu (c) bunu yalnızca *"DecisionAggregate'in event-sourcing'i ile henüz
birleştirilmedi"* diye anıyor — bu, "iki lifecycle bağlanmadığı için ikincisine **kararsız**
girilebiliyor" demekten çok daha zayıf bir ifade.

### 4.3 Audit zamanı uydurulabiliyor (`AUDIT_DEFECT_W2_L3`)

`Transition(to, at)` `at`'i hiç doğrulamıyor. Tüm lifecycle geriye akan damgalarla
koşturulabiliyor (`Traced @ DateTimeOffset.MinValue`, `Learned @ MaxValue`,
`Remembered @ T0−100 yıl`). Reflection **gerekmiyor**. *"Her geçiş bir AuditEvent üretir"*
doğru; *"AuditEvent güvenilir bir zaman taşır"* **değil**. R6(a) ile aynı kök: ENS'te
zaman her yerde çağıranın beyanı.

### 4.4 `History` silinemiyor ama **canlı ve senkronize değil** (`AUDIT_DEFECT_W2_L4`)

`AUDIT §5.2` düzeltmesi `History`'yi **silinemez** yaptı ama **canlı** bıraktı.
`ReadOnlyCollection<T>` bir kopya değil, alttaki `List<T>` üzerine bir görünümdür ve **hiçbir
eş zamanlılık güvencesi vermez.** Tek iş parçacığında bile deterministik olarak kanıtlanıyor:

```csharp
foreach (var _ in layer.History)     // denetçi geçmişi okurken
    layer.BeginActing(T0);           // bir geçiş olursa
// -> InvalidOperationException: Collection was modified
```

Aynısı `DecisionAggregate.History` için de geçerli. Yapısal kanıt (reflection): sınıfta
**kilit yok, concurrent koleksiyon yok, `volatile` durum alanı yok**; `_history` düz bir
`List<ActionTransition>`.

Ek olarak: geçiş tablosu `Allowed` **proses genelinde paylaşılan, değiştirilebilir bir
`Dictionary<ActionState, ActionState[]>`**'tir ve değerleri **mutable dizilerdir**. Tek bir
reflection yazması, o prosesteki **tüm** `ActuationLayer` örneklerinin state machine'ini
kalıcı olarak bozar. Bunu **bilerek test etmedim** — testin kendisi diğer testleri
zehirlerdi; riskin kendisi bulgudur. (Doğru düzeltme: `FrozenDictionary` +
`ImmutableArray`/`FrozenSet`.)

### 4.5 Eş zamanlılık: neyi **kanıtlayamadım**

`ActuationLayer.Transition` ve `DecisionAggregate.Commit` klasik **check-then-act**'tir
(`Allowed[State].Contains(to)` → `_history.Add` → `State = to`) ve senkronize değildir. İki
iş parçacığının aynı geçişi birlikte geçmesi ya da `List<T>.Add` bozulması (kayıp Add,
`IndexOutOfRangeException`) **teorik olarak mümkündür**.

**Bunu deterministik bir testle kanıtlayamadım ve kanıtlamış gibi yapmayacağım.** Yarış
durumları zamanlamaya bağlıdır; "kusuru gösteremediği için yeşil yanan" bir `AUDIT_DEFECT`
yazmak, skill'in yasakladığı manipülasyonun ta kendisi olurdu. Bu yüzden `C2`/`C3` testleri
sayaçları toplar ama **üzerlerine iddia kurmaz**; kanıtladıkları şey dar ama gerçektir
(illegal hamleler reddediliyor, enum bozulmuyor). Yapısal risk `L4`'te reflection ile
gösterilmiştir.

---

## 5. SAĞLAM ÇIKANLAR (dürüstlük gereği)

Saldırdım, kırılmadı:

| İddia | Nasıl sınadım | Sonuç | Test |
|---|---|---|---|
| Replay sıralama/kardinalite mührü | 9 farklı bozuk akış (ters sıra, eksik, tekrarlı, atlamalı, null) | **Mühürlü** | `R1` |
| Unicode whitespace guard'ı | 13 varyant × 3 giriş noktası | **Hepsi reddedildi** | `P2` |
| `Premises` değiştirilemezliği | girdi-aliasing + `List<T>` downcast + `IList<T>.Clear` + setter | **Kapalı** | `P6` |
| min t-norm idempotansı (1000 tekrar) | aynı öncül 1000 kez | **Confidence değişmiyor** | `P5` |
| Tanımsız `GateDecision` | `999`, `-1` | **Fail-CLOSED (Blocked)** | `L5` |
| `ApplyGate` tekrarı | ikinci çağrı + terminal durum | **Reddediliyor, yarım durum yok** | `L6` |
| Bağımsız katmanlarda paralellik | 64 katman × tam lifecycle | **Doğru** | `C1` |
| Paylaşılan katmanda illegal hamleler | 384 deneme / 7 meşru geçiş | **Reddediliyor** | `C2` |

Ve `AUDIT.md`'nin kapattığını söylediği kusurlardan **doğrulayabildiklerim gerçekten kapalı**:
replay invariant'ları (I1/I2), `ProofTrace.Premises` downcast'i (D3), `History` silinmesi (E4).
Kapatılmadığı ilan edilenler (E3 sahte `GateResult`, E5 reflection) hâlâ açık ve bu rapor
onlara yeni bir şey eklemiyor.

---

## 6. SAHİBİNE TALEPLER

Öncelik sırasıyla:

1. **`dotnet test` koşturulup çıktısı kesilmeden repoya yapıştırılmalı.** Bu turun
   tamamlanması için zorunlu son adım. Kırılan her `AUDIT_*_W2_*` testi **benim hatamdır** ve
   görmek isterim.
2. **`Apply`'daki `as ReadOnlyCollection<string>` kısayolu kaldırılmalı** — her zaman gerçek
   kopya alınmalı (`new ReadOnlyCollection<string>(new List<string>(e.Alternatives))`).
   Yanındaki "kendi kopyasını tutar" yorumu ya doğru olmalı ya silinmeli. **(R4)**
3. **`Rehydrate` her olayın `Target`'ını `id` ile doğrulamalı**; `DecisionCommitted.Owner` ile
   `Emitter` ilişkisi bir karara bağlanmalı. **(R2, O1)**
4. **`EnsureReplayInvariant` ile canlı yol arasındaki her ayrışma kapatılmalı** — boş/null
   alternative kontrolü replay'de de olmalı. Daha iyisi: tek bir ortak doğrulama fonksiyonu,
   iki yoldan da çağrılsın (ayrışma yapısal olarak imkânsız hâle gelsin). **(R3)**
5. **Switch'lere `default:` dalı eklenmeli** — tanınmayan olay sessizce yutulmamalı, açık
   hata olmalı. **(R5)**
6. **"Tek Owner" ya kodlanmalı ya da iddiadan çıkarılmalı.** Karşılıksız bir mühür ilan etmek,
   Madde X disiplinine aykırıdır. Kodlanacaksa: `Owner` property'si + tüm faz metotlarında
   owner kontrolü + dosyanın dürüst-sınırlar bloğuna kayıt. **(O1)**
7. **`Premise`/`ProofTrace` guard'ı Unicode kategorisine göre yapılmalı** — Cf ve Cc
   karakterleri de reddedilmeli (`s.All(c => char.IsWhiteSpace(c) || char.GetUnicodeCategory(c)
   is Format or Control)` → reddet). **(P1)**
8. **`Render()` kaçış uygulamalı** ya da yapılandırılmış (JSON gibi) bir çıktı üretmeli;
   insan-okunur metin **güvenlik sınırı olamaz**. **(P3)**
9. **`ProofTrace` kanıtladığı şeye bağlanmalı** (`Identity` alanı) ve `RecordTrace` bu bağı
   doğrulamalı; öncülün kendi sonucuna eşit olması reddedilmeli. **(P4, L2)**
10. **`ActuationLayer` girişi bir commitment kanıtı istemeli** — hiç commit edilmemiş bir karar
    için action lifecycle'ı başlatılamamalı. Bu, `AUDIT §4.1`'in gate-token borcuyla aynı
    ailedendir ve muhtemelen aynı ADR'yi bekliyor. **(L1)**
11. **Zaman çağıranın beyanı olmaktan çıkarılmalı** — `TimeProvider` enjeksiyonu
    (`CompanyMemory`'de zaten var) `ActuationLayer` ve replay için de kullanılmalı; geriye akan
    damga reddedilmeli. **(L3, R6a)**
12. **`Allowed` tablosu `FrozenDictionary` + immutable dizi olmalı**; koleksiyon getter'ları
    canlı görünüm yerine snapshot dönmeli ya da sınıf açıkça "thread-safe değildir" diye
    belgelenmeli. **(L4)**

Bunların **1, 2, 3, 4, 5, 7** — teori borcu gerektirmeyen, saf implementasyon düzeltmeleridir.
**6, 9, 10** ise teori/ADR tarafında karşılık istiyor (Madde VIII: önce Accepted ADR); bunları
kod uydurarak kapatmak, Madde IX'un yasakladığı şeydir.

---

## 7. BU RAPORUN KENDİ YANLIŞLANABİLİRLİĞİ (Madde X)

- **En zayıf halkam: hiçbir şey çalıştırılmadı.** Testler derlenmedi bile. Bir derleme hatası
  bu raporun tamamını "henüz kanıtlanmamış" hâlde bırakır.
- **Güven derecelerim:** R4, P1, P2, P4, P5, L1, L2, L3, L6 → *çok yüksek* (dil/BCL semantiği,
  saf okuma). R2, R3, R5, R6, O1 → *çok yüksek* (kodda ilgili kontrolün **yokluğu** doğrudan
  okunabiliyor). P3, L4 → *yüksek* (string interpolasyonu ve `List<T>` enumerator sürüm
  kontrolü belgelidir). C2/C3 → **iddia kurmuyorum**.
- **Neyi test etmedim:** gerçek eş zamanlılık yarışlarını (deterministik kanıt yazılamadı),
  serileştirme/deserileştirme yollarını (henüz yok), `Ens.Kernel.Demo`'yu, `CompanyMemory` ve
  `Scheduler`'ı (bu turun kapsamı dışında), performansı/kaynak tüketimini.
- **Bulguları çürütmenin yolu:** ilgili `AUDIT_DEFECT_W2_*` testini koşturun. Kırmızı yanan
  her DEFECT testi, o bulgunun **yanlış** olduğunu gösterir ve bu raporun ilgili bölümü
  düzeltilmelidir.

---

*ens-skeptic, 2026-07-26. Bu belge koordinatörün README'sinden ve `AUDIT.md`'den bağımsızdır;
onlarla çelişebilir. Çelişki hâlinde önce hangisinin **çalıştırılmış** kanıta dayandığına
bakın — bu tur için cevap: hiçbiri.*
