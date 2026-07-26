# AUDIT — Faz-4 Reference Implementation Bağımsız Düşmanca Denetimi

| | |
|---|---|
| **Denetleyen** | `ens-skeptic` — demoyu ve testleri YAZAN context'ten ayrı, taze bir context |
| **Denetlenen** | `Ens.Kernel.Demo/Program.cs`, `Ens.Kernel/**`, `Ens.Kernel.Tests/**` (HEAD, 2026-07-25) |
| **Tetikleyen itiraz** | *"hiç bir görmedim ne verdik ne aldık bilmiyorum — test verisi yazıyorsun, manipüle etmediğini nereden bileceğim"* |
| **Yetki** | Anayasa Madde X (Yanlışlanabilirlik Ödevi) |
| **Ürettiğim kanıt** | `Ens.Kernel.Tests/AdversarialAuditTests.cs` (52 test, koordinatörün görmediği girdilerle) |

---

## 0. ÖNCE DÜRÜSTLÜK: bu denetimin kendi sınırı

**Bu denetimde `dotnet test` ve `dotnet run` ÇALIŞTIRILAMADI.** Bana verilen görev "Bash var"
diyordu; gerçekte bu context'te Bash/shell aracı **etkin değil**. Elimde yalnızca dosya okuma,
yazma ve arama vardı.

Bu yüzden:

- **Hiçbir test çıktısı, hiçbir konsol çıktısı bu raporda fabrike EDİLMEDİ.** Aşağıda tek bir
  satır bile "şu çıktıyı aldım" demiyor.
- Tüm bulgular **statik analiz + elle hesap**tır. Her bulgunun yanında güven derecesi var.
- `AdversarialAuditTests.cs` yazıldı ama **derlenmedi/koşturulmadı**. Onu koşturmak
  denetimin tamamlanması için **zorunlu son adımdır** (§7).

Kullanıcının itirazı bu noktada haklılığını koruyor: *bu rapor da henüz çalıştırılmış bir
kanıt değil.* Farkı şu: bu rapor, koşturulunca **kendini yanlışlayabilecek** somut testler
bırakıyor — bulgularımın hangisi yanlışsa, hangi testin kırılacağı yazılı.

---

## 1. ANA SORUYA CEVAP: demo manipüle edilmiş mi?

### Hardcoded / sahte sonuç: **YOK.**

Program.cs'i satır satır taradım. Ekrana basılan **her sayısal değer gerçekten hesaplanıyor**:

| Demo iddiası | Nereden geliyor | Sahte mi? |
|---|---|---|
| "Confidence ATANMADI, min t-norm ile HESAPLANDI ... = `{trace.Confidence:F2}`" | `ProofTrace` ctor'ında `premises.Min(p => p.Confidence)` | **Hayır**, gerçek hesap |
| Scheduler tablosundaki `InfoNeed` / `Öncelik` / `Tier` / `Gate` sütunları | `Scheduler.Schedule(...)` çıktısındaki nesne alanları | **Hayır** |
| "✖ Geçersiz action geçişi: Planned → Acting" | Gerçek `InvalidTransitionException`, gerçek try/catch | **Hayır** |
| "Decision zaten commit edildi" | Gerçek aggregate invariant'ı | **Hayır** |
| Memory `retention` / `tazelik` / `salience` sayıları | `memory.Salience(...)`, `r.RetentionPriority` | **Hayır** |
| "Sınıfın TÜM public metotları: [Propose]" | Gerçek `System.Reflection` taraması | **Hayır** |

`Console.WriteLine` ile elle yazılıp "hesaplanmış" gibi sunulan **tek bir sayı bulamadım.**
Bu, itiraz karşısında koordinatör lehine gerçek ve önemli bir bulgudur.

### Ama: **üç sunum kusuru + bir gerçek kod hatası var, ve şişirme burada.**

Kusur, sayıları uydurmakta değil; **sayıların kanıtlamadığı şeyi kanıtlamış gibi anlatmakta**.

---

## 2. EN KRİTİK BULGU — demo'nun "DÜRÜST BULGU" bölümü, bir overload hatasının ürünü

`Program.cs:207-211` demonun en gurur duyduğu yer: kendi kusurunu itiraf ettiği bölüm.
**O itiraf yanlış.** Sebebi bir C# overload çözümleme tuzağı.

`LlmTierSelector`'da iki overload var:

```csharp
// M1
public static LlmTier SelectTier(double infoNeed, double complexThreshold = 10.0, double criticalThreshold = 40.0)
// M2  (convenience)
public static LlmTier SelectTier(double stake, double? confidence, double complexThreshold = 10.0, double criticalThreshold = 40.0)
```

Demo satır 201:

```csharp
var naive = LlmTierSelector.SelectTier(stake, conf); // varsayılan eşikler (10/40)
```

`stake` ve `conf` burada **ikisi de `double`** (tuple `(500.0, 0.95, "...")`'ten geliyor).
C# "better conversion target" kuralı (`double` → `double`, `double` → `double?`'tan daha
iyidir) gereği bu çağrı **M2'ye değil M1'e** bağlanır. Yani gerçekte çalışan şey:

```csharp
SelectTier(infoNeed: stake, complexThreshold: conf, criticalThreshold: 40.0)
```

**InfoNeed hiç hesaplanmıyor**; stake doğrudan eşikle karşılaştırılıyor ve confidence
eşik yerine geçiyor. Sonuç: 500, 45 000 ve 250 000'in üçü de `>= 40` olduğu için üçü de
`Critical` basılıyor — demo da bunu "varsayılan eşikler HER kararı Critical'a atıyor"
diye yorumluyor.

**Doğru hesap:** `InfoNeed(500, 0.95) = 25`; 10/40 eşikleriyle bu `Complex`'tir, `Critical`
değil. Yani demo'nun "her karar Critical'a düşüyor" tespiti **gerçek değil**.

Daha da kötüsü: demo bu bulgudan bir **teori borcu** çıkarıyor ("Stake'in Purpose-tipi içinde
normalize edilmesi şart, kalibrasyon borcu"). O borç ENS-3022'de zaten kayıtlı ve muhtemelen
gerçek — ama **demonun sunduğu kanıt onu desteklemiyor**. Bir teori borcunu bir kod hatasıyla
"kanıtlamak", uydurulmuş veriden farklı ama aynı derecede ciddi bir epistemik kusurdur.

Ayrıca demo çıktısı **kendi kendisiyle çelişiyor**: aynı satırda `InfoNeed=25` yazıp sağ
sütunda "10/40 eşikleriyle Critical" demek aritmetik olarak imkânsız. Bu, çıktıya dikkatle
bakan birinin yakalayabileceği bir tutarsızlıktır — ve kimse bakmamış.

**Neden testler yakalamadı:** `LlmAdapterTests.cs` bu overload'ı **adlandırılmış argümanla**
çağırıyor (`SelectTier(stake: 100, confidence: 0.5)`), bu da doğru overload'a gider. Testler
API'yi doğru kullanıyor, demo yanlış kullanıyor; test suite'i tam da bu farkı göremeyecek
şekilde yazılmış. Bu, "kendi sınavını yazıp kendi geçmek"in ders kitabı örneğidir — kasıt
gerekmez, sadece aynı zihnin aynı varsayımı iki kez yapması yeter.

> **Kanıt testi:** `AUDIT_DEFECT_C1_positional_call_silently_binds_to_the_wrong_overload`
> **Güven:** yüksek (C# spec, better conversion target kuralı) — *ama yürütülerek doğrulanmadı.*
> Bu test kırmızı yanarsa bu bulgum çürütülmüş demektir.
>
> **KERNEL TEMİZ:** `Scheduler.cs:84` üç `double` argümanla çağırıyor ve doğru overload'a
> gidiyor (`AUDIT_HOLDS_C2`). Hata yalnızca demoda.

---

## 3. GİRDİLER CHERRY-PICKED Mİ? — evet, üç yerde

### 3.1 Scheduler: gösterilen formülün ayırt edici gücü SIFIR

Demo §2'nin ana iddiası:

> "Sıralama ölçütü keyfi DEĞİL: AttentionPriority = InfoNeed × ConformanceDeficit"

Seçilen üç kararla elle hesapladım:

| | Stake | Conf | Deficit | InfoNeed | Priority |
|---|---|---|---|---|---|
| #1 | 250 000 | 0.80 | 0.40 | 50 000 | 20 000 |
| #2 | 45 000 | 0.55 | 0.70 | 20 250 | 14 175 |
| #3 | 800 | 0.92 | 0.10 | 64 | 6.4 |

Deficit sütununu **tamamen sıfırlarsanız** sıralama **değişmiyor** — çünkü `Scheduler`'ın
tie-breaker'ı `InfoNeed` ve InfoNeed sıralaması zaten aynı. Yani demo, iddia ettiği çarpanın
sonuca **hiçbir katkısını göstermiyor**. Sadece InfoNeed'e bakan bir sistem de aynı çıktıyı
verirdi.

Bu, veriyi manipüle etmek değil ama **hiçbir şeyi ayırt etmeyen bir gösteri kurmak**tır.

Daha ağırı: `Scheduler.cs`'in kendi dürüst-sınır notu (b) diyor ki *ConformanceDeficit
Company Memory'den türetilmeli, o bağ HENÜZ YOK, 0 varsayılır*. Yani demo'nun elle girdiği
0.10 / 0.70 / 0.40 değerleri, **sistemin bugün üretemeyeceği** sayılardır. Gerçek kullanımda
deficit hep 0 olur, tüm AttentionPriority'ler 0'a düşer ve "teoriden türeyen ölçüt" fiilen
devre dışı kalır. Demo bu kritik uyarıyı tekrarlamıyor; kaynak dosya tekrarlıyor.

> **Kanıt testi:** `AUDIT_FINDING_B5_demo_inputs_do_not_demonstrate_ConformanceDeficit_at_all`

### 3.2 Curator sweep: sıfır kayıt bayrakladı

Demo §6 sonunda:

```
Curator sweep: {stale.Count} bayat kayıt BAYRAKLANDI (silinmedi — §3b, P7)
```

Demo'nun kendi verisiyle bu sayıyı elle hesapladım:

- Eski kayıt: `AttributionConfidence = 0.85`, `AssertedAt = now − 400g`, `asOf = now + 60g`
- λ = 0.01 · (1 − 0.85)¹ = **0.0015**, yaş = **460 gün**
- decayFactor = exp(−0.0015 · 460) = exp(−0.69) ≈ **0.5016**
- Eşik: **0.5** → 0.5016 > 0.5 → **bayat DEĞİL**

**Sonuç: `stale.Count` = 0.** Demo'nun curator gösterisi **hiçbir şey göstermiyor**;
"BAYRAKLANDI (silinmedi)" cümlesi sıfır kayıt üzerine kuruluyor.

Ve fark kıl payı: 462.1 gün olsaydı bayraklanacaktı. Demo 400 gün seçmiş. Bunun bilinçli
bir seçim olduğuna dair bir kanıtım **yok** — büyük ihtimalle sadece kimse çıktıya bakmadı.
Ama etkisi aynı: çalışmayan bir özellik çalışıyormuş gibi anlatıldı.

> **Kanıt testi:** `AUDIT_FINDING_G7_demo_curator_sweep_flags_exactly_zero_records`
> **Güven:** yüksek (saf aritmetik).

### 3.3 Memory sıralaması: karıştırıcı değişken (confounder) ayrıştırılmamış

Demo §6, "|Learning|=8 olan kayıt öne çıkıyor → karşı-survivorship" diyor. Ama o kayıt
rakibinden **hem 4× büyük |Learning|'e HEM de 430 gün daha taze** olmaya sahip. Sıralama
tek başına tazelikle de açıklanabilir. Gösteri iki nedeni ayrıştırmıyor.

Altta yatan yasa **sağlam** (yaşı sabitleyince iddia hâlâ tutuyor — testte doğruladım). Ama
demo'nun sunduğu kanıt bunu göstermiyor, ve "ENS başarısızlığın nedenini daha güçlü
hatırlar" iddiası **mutlak değil**: yeterince eski + düşük-confidence bir kayıt, taze ve
yüksek-confidence bir kayda yenilir. Bu bir kusur değil, tasarım — ama demo'nun kesin dili
bunu gizliyor.

> **Kanıt testi:** `AUDIT_FINDING_G8_demo_memory_ordering_is_confounded_age_not_isolated`

---

## 4. "YAPISAL OLARAK İMKÂNSIZ" İDDİALARINA SALDIRI

Demo özeti dört şeyin "politika değil, tip sistemi" düzeyinde imkânsız olduğunu söylüyor.
Dördünü de kırmaya çalıştım. **Üçü kısmen kırıldı, biri sağlam.**

### 4.1 "Gate'siz action → imkânsız" — **KIRILDI** (en ciddi bulgu)

`ActuationLayer.ApplyGate(GateResult gate, ...)` yalnızca **bir `GateResult` nesnesi verildi
mi** diye bakar. O nesnenin `BoundedAutonomyGate.Evaluate`'ten geldiğini **doğrulamaz**.
`GateResult` public bir `record`:

```csharp
var forged = new GateResult(GateDecision.Autonomous, "gate hiç çalıştırılmadı", 0.0);
layer.ApplyGate(forged, now);   // → Contextualized
layer.BeginActing(now);         // → Acting. P7 politikası HİÇ değerlendirilmedi.
```

Doğru ifade: *"Gate'siz action imkânsız"* değil, *"GateResult **nesnesi** olmadan action
imkânsız"*. P7'nin **şekli** zorlanıyor, **özü** zorlanmıyor. Tek satırlık, reflection
gerektirmeyen bir kaçış.

> `AUDIT_DEFECT_E3_the_gate_result_can_be_forged_in_one_line`

### 4.2 "İzsiz action → imkânsız" — **ZAYIF** (kod dürüst, demo değil)

İki ayrı delik:

**(a) Invariant sadece kardinalite kontrol ediyor.** Aşağıdaki tamamen uydurma trace tüm
invariant'ları geçiyor ve `Confidence = 1.00` üretiyor:

```csharp
new ProofTrace("uydurma-kural", "her şey yolunda", [new Premise("kanıt-yok", 1.0)]);
```

Gerçek iddia: "en az bir boş-olmayan string olmadan action olmaz". `ProofTrace.cs`'in
dürüst-sınır notu (b) bunu **açıkça kabul ediyor** — ama demo'nun özet bölümü etmiyor.
**Kaynak dosya demodan daha dürüst.** Bu ters bir durum.

**(b) Kurulduktan sonra boşaltılabiliyor.** `Premises` bir `IReadOnlyList<Premise>` olarak
sunuluyor ama arkasındaki nesne canlı bir `List<Premise>`:

```csharp
((List<Premise>)trace.Premises).Clear();   // öncülsüz trace artık VAR
layer.RecordTrace(trace, now);             // ve Traced'e geçiyor
```

> `AUDIT_FINDING_D5`, `AUDIT_DEFECT_D3`, `AUDIT_DEFECT_E6`

### 4.3 "İzsiz başarısızlık → imkânsız" — **SAĞLAM**

`Failed → Traced` zorunluluğu gerçek. Tüm state machine'i bağımsız olarak yeniden tanımlayıp
her durumdan her metodu denedim (9 durum × 7 hamle = 63 kombinasyon): **kaçak yol bulamadım.**
Exception'ı yakalayıp 1000 kez tekrar denemek de state'i ilerletmiyor.

**Tek istisna:** reflection ile `State` alanına doğrudan yazarak Gate atlanabilir ve
**hiçbir audit izi bırakmaz**. Bu .NET'in genel gerçeği, tek başına ağır bir suçlama değil;
ama "atlanamaz" yerine "normal API üzerinden atlanamaz" demek daha doğru olur.

> `AUDIT_HOLDS_E1`, `AUDIT_HOLDS_E2`, `AUDIT_DEFECT_E5`

### 4.4 "Otomatik model-revizyonu → imkânsız" — **YOKLUKTAN ARGÜMAN**

Demo: *"öneri uygulanamaz, çünkü uygulayacak metot yok. İnsan onayı MİMARİ olarak zorunlu."*

`ReflectiveDoubleLoop` sınıfında gerçekten `Apply/Commit/Update` yok — bu **doğru**. Ama bu
tek bir sınıf hakkında bir gözlem; **sistem düzeyinde bir garanti değil**. `CompanyMemory.Record`
public ve korumasız. Kod tabanında "insan onayı" diye bir **tip bile yok** (onay token'ı,
approval gate, imza — hiçbiri). Bir öneriyi otonom uygulamak üç satır:

```csharp
foreach (var p in ReflectiveDoubleLoop.Propose(records))
    memory.Record(new MemoryRecord(Identity.New(), p.PurposeType, 0, 1.0, now));
```

"Bir sınıfta bir metot yok" ile "mimari olarak zorunlu" arasında büyük bir mesafe var.
Demo bu mesafeyi kapatıyormuş gibi konuşuyor.

> `AUDIT_DEFECT_H1`, `AUDIT_HOLDS_H3`

---

## 5. TESTLERİN GÖRMEDİĞİ KUSURLAR (kendi düşmanca testlerimin bulduğu)

Bunlar demoyla ilgili değil, **kernel'in kendisiyle** ilgili ve mevcut test suite'inin hiç
denemediği girdilerden çıktı.

### 5.1 P7 gate'i NaN ve `confidence > 1` altında **FAIL-OPEN** (en tehlikeli teknik bulgu)

`DecisionGravity.InfoNeed` ve `BoundedAutonomyGate.Evaluate` confidence'ı **hiç doğrulamıyor**
(oysa `DecisionAggregate.Commit`, `Premise`, `DecayFunction` doğruluyor — güvenlik-kritik
yol, doğrulanmayan yol).

IEEE-754'te NaN her karşılaştırmada `false` döner. Sonuç:

| Girdi | Gate kararı | Neden |
|---|---|---|
| `stake = NaN` | **Autonomous** | `NaN >= blockThreshold` false, `NaN >= autonomyThreshold` false → en permisif dal |
| `confidence = NaN`, stake = 250M | **Autonomous** | aynı |
| `confidence = 5.0`, stake = 1 milyar | **Autonomous** | belirsizlik = −4 → InfoNeed negatif → hiçbir eşiği aşmaz |

Ve `Scheduler` üzerinden bakıldığında **üçlü fail-open**:

1. Gate → `Autonomous` (insan görmez)
2. Tier → `Operational` (en ucuz/zayıf model bakar)
3. Sıra → **en son** (NaN, `OrderByDescending`'de en küçük sayılır → dikkat bütçesi asla ulaşmaz)

Exception de atılmıyor. Ölçülemeyen bir karar, sistemin en karanlık köşesine **sessizce**
düşüyor ve tam otonomi alıyor. Bu, P7'nin ("sorumluluk insanda") tam tersi bir davranış.

**Kök neden `is < 0 or > 1` deseninin NaN körlüğü** ve aynı kök neden 5 ayrı yerde tekrarlıyor:
`DecisionGravity`, `BoundedAutonomyGate`, `Premise`, `DecisionCapital.Value`,
`DecisionAggregate.Commit`. Zincir kapalı değil: NaN confidence ile commit edilen bir karar
doğrudan gate'e akıp otonomi alabilir.

**Tek fail-CLOSED kural:** `IsIrreversible` → NaN yağmuruna rağmen `CriticalBlock`. Bu
sağlam ve övgüyü hak ediyor.

> `AUDIT_DEFECT_A1/A2/A3`, `AUDIT_HOLDS_A4`, `AUDIT_DEFECT_B3`, `AUDIT_DEFECT_I3`

### 5.2 "Asla silinmez" invariant'ı — **üç yerde tek satırda kırılıyor**

`IReadOnlyList<T>` / `IReadOnlySet<T>` döndürülüyor ama arkadaki canlı koleksiyon
downcast edilebiliyor. **Reflection gerekmiyor.**

```csharp
((List<MemoryRecord>)memory.AllRecords).Clear();       // "kayıt asla silinmez" (ENS-2003 §3)
((List<ActionTransition>)layer.History).Clear();       // "her geçiş bir AuditEvent üretir"
((List<DomainEvent>)decision.History).Clear();         // "karar bir satır değil, olay geçmişidir"
((HashSet<string>)pack.AllowedTools).Add("delete_database");  // yetki kaçağı
```

Sonuncusu özellikle ciddi: kayıtlı bir Capability Pack'e **sonradan** yetki eklenebiliyor ve
registry fark etmiyor. "Deklaratif izinler" runtime'da mutable.

İlginç: `ProofTraceTests.Premises_are_defensively_copied` testi **girdi** listesinin
mutasyonunu test ediyor (kolay yön) ama **çıktı** downcast'ini test etmiyor (zor yön).
Aynı desen dört sınıfta tekrarlıyor.

> `AUDIT_DEFECT_G1/E4/I5/F2/D3`

### 5.3 `DecisionAggregate.Rehydrate` — §Individuation mührü replay yolunda YOK

Demo §3'ün gururu: "Atom sınırı MÜHÜRLENDİ: tek Owner, tek Purpose, açık Alternatives, tek
Commitment." Canlı API'de bu **gerçekten sağlam** (doğruladım). Ama `Rehydrate` public static
ve olayları **hiçbir doğrulama yapmadan** uyguluyor:

```csharp
var forged = new DecisionCommitted("hiç-değerlendirilmemiş", owner, 0.99, "harika")
             { Emitter = owner, Target = id };
var d = DecisionAggregate.Rehydrate(id, [forged]);
// d.IsCommitted == true, d.Purpose == null, d.Alternatives boş
```

İki `DecisionCommitted` de kabul ediliyor ("tek Commitment" invariant'ı yok). Event-sourced
bir sistemde replay yolu **birincil** yoldur; invariant'ları yalnızca yazma yolunda korumak
yetersizdir.

Ayrıca `IdentifyAlternatives` gelen listeyi kopyalamıyor — çağıran sonradan bir alternatif
ekleyip **hiç değerlendirilmemiş** bir seçeneğe commit edebiliyor.

> `AUDIT_DEFECT_I1/I2/I4`

### 5.4 Company Memory: decay yasası denetimsiz devre dışı bırakılabiliyor

- `Verify(id, gelecekTarih)` → `ageDays = max(0, negatif) = 0` → decayFactor kalıcı **1.0**.
  Yeniden-doğrulama **hiçbir kanıt istemiyor**, gelecek tarih kabul ediyor, iz bırakmıyor.
  Bir kayıt tek çağrıyla sonsuza dek "taze" yapılabilir ve curator onu bir daha göremez.
- `Verify` **DecisionId** ile anahtarlanıyor, kayıt bazında değil → aynı karardan iki öğrenim
  varsa birini doğrulamak diğerini de tazeliyor (çapraz-kirlenme).
- `FindStale`'in `RetentionPriority > 0` filtresi: `|Learning| = 0` **veya** `confidence = 0`
  olan kayıt **100 yıl geçse de asla bayraklanmıyor** — curator'un kör noktası.
- `MemoryRecord` ctor'ı doğrulama yapmıyor; doğrulama `RetentionPriority` erişiminde. Tek bir
  geçersiz kayıt (negatif `LearningMagnitude`) sessizce yazılıyor, sonra **her** `Retrieve` ve
  `FindStale` çağrısını patlatıyor → belleğin tamamı servis dışı.

> `AUDIT_DEFECT_G2/G3/G4/G5/G6`

### 5.5 ADR-0001 §6.1'in iddia ettiği "ENS deltası" kodda BAĞLI DEĞİL

`CapabilityRegistry.cs` şöyle diyor:

> "(2) Pack'in deklaratif `AllowedTools`/`RequiresHumanApprovalFor` izinleri **doğrudan
> Bounded-Autonomy Gate'e (P7) beslenir** — per-capability human-approval'ı birinci-sınıf
> registry alanı yapan tek sistem."

`BoundedAutonomyGate.Evaluate` imzasında `ToolAuthorization`, `CapabilityRegistry` ya da
`CapabilityPack` **yok**. Demo da ikisini yan yana **yazdırıyor** ama birbirine
**bağlamıyor** (satır 98 `Authorize(...)`'ın sonucunu ekrana basıyor, sonra kullanmıyor).

ENS'in prior-art'a karşı en dar ve en spesifik özgünlük iddiası, kodda **henüz gerçekleşmemiş**.
Bu bir yalan değil — dosya "beslenir" derken tasarım niyetini anlatıyor olabilir — ama
Faz-4'ün "çalışan kanıt" iddiası bu noktada karşılanmıyor.

Buna bağlı ikinci bulgu: bir aracı iki Pack veriyorsa ve katı olanı `Disable` ederseniz,
araç **yetkili kalıyor ama insan-onayı şartı düşüyor**. "Kapasite kapatma" jesti bir güvenlik
kontrolünü sessizce kaldırıyor.

> `AUDIT_FINDING_F4`, `AUDIT_DEFECT_F3`

### 5.6 Diğerleri

- `ConformanceDeficit` [0,1] ile sınırlanmıyor → `deficit = 1e9` veren bir çağıran, önemsiz
  bir kararı en kritik kararın önüne geçirebiliyor. Dikkat kuyruğu manipüle edilebilir.
- `ReflectiveDoubleLoop`'un guard mesajı *"tek gözlemden 'sistematik' iddia edilemez"* diyor
  ama `minSupportingRecords: 1` kabul ediliyor → tam olarak tek gözlemden "sistematik" öneri
  üretiliyor. Guard kozmetik.
- `SchedulerTests` "girdi sırası sonucu değiştirmez" diyor ama bunu InfoNeed'in **farklı**
  olduğu bir örnekle gösteriyor. Tam eşitlikte sonuç girdi sırasına bağlı.

---

## 6. SAĞLAM ÇIKANLAR (dürüstlük gereği)

Saldırdım, kırılmadı:

| İddia | Nasıl sınadım | Sonuç |
|---|---|---|
| `AttentionPriority = Stake·(1−Conf)·max(0,Deficit)` | 200 rastgele karar, formülü **elle** hesaplayıp karşılaştırma + sıralama monotonluğu | **Sağlam** |
| `InfoNeed → Tier` eşlemesi | 500 rastgele (stake, conf), eşikleri elle uygulama | **Sağlam** |
| L7 min t-norm | 500 rastgele öncül kümesi, min'i LINQ'siz elle hesaplama | **Sağlam** |
| `λ(c)=λ_base(1−c)^γ`, `exp(−λΔt)` | 300 rastgele (conf, γ, baseRate, yaş) elle hesap | **Sağlam** |
| Action lifecycle geçiş matrisi | 9 durum × 7 hamle tam tarama + exception-yutma döngüsü | **Kaçak yok** |
| Geri-dönülemezlik bloğu | NaN bombardımanı altında | **Fail-closed** |
| §Individuation (canlı yol) | 7 farklı ihlal denemesi | **Mühürlü** |
| Ordinal tool-adı eşleşmesi | 12 homoglif / boşluk / NUL / zero-width varyantı | **Hepsi reddedildi** |
| Model-agnostisizm | Somut sağlayıcı bağı arandı | **Gerçekten yok, saf port** |
| `Confidence` atanamazlığı | `CanWrite`, `IsInitOnly`, `with` ifadesi | **C# düzeyinde immutable** |

Ve en önemlisi: **kaynak dosyaların "DÜRÜST SINIRLAR" blokları gerçekten dürüst.** Sandbox
yokluğu, kalibre edilmemiş eşikler, bağlanmamış ConformanceDeficit, proof-trace'in otomatik
üretilmemesi — hepsi kodda açıkça yazılı. Bu, ENS'in kendi Madde X disiplinine gerçek bir
uyum işaretidir. **Sorun kodun dürüstlüğünde değil, demonun o dürüstlüğü daraltmasında.**

---

## 7. VERDICT ve KULLANICININ GÜVENMESİ İÇİN EKSİK OLAN

### Verdict: **`wounded` — manipüle edilmemiş ama şişirilmiş.**

- **Uydurma veri: YOK.** Demo'daki her sayı gerçekten hesaplanıyor. İtirazın en sert hâli
  ("sahte sonuç yazıyorsun") **doğrulanmadı** ve koordinatör bu konuda temiz çıktı.
- **Ama demo bir sınav değil, bir sunum.** Girdiler, iddiaları **ayırt edecek** şekilde değil,
  akış **güzel görünecek** şekilde seçilmiş. Üç gösteriden biri (curator) sıfır sonuç
  üretiyor, biri (scheduler) formülünün katkısını göstermiyor, biri (memory) confounded.
- **"Yapısal olarak imkânsız" dilinin kapsamı abartılı.** Kod, tip düzeyinde **şekilleri**
  zorluyor; **özleri** zorlamıyor (sahte gate, sahte öncül, tek satırda silinen audit).
- **Bir gerçek kod hatası var** (§2) ve talihsiz biçimde demonun *en dürüst görünen* bölümünü
  geçersiz kılıyor.

### Kullanıcının güvenmesi için gereken 6 şey

1. **`dotnet test` ve `dotnet run` gerçekten koşturulup çıktısı repoya yapıştırılmalı.**
   Bu denetimde bunu yapamadım; koordinatör de raporlamamış. `AdversarialAuditTests.cs`
   dahil tam çıktı, kesilmeden.
2. **`Program.cs:201` düzeltilmeli** (`SelectTier(stake: ..., confidence: ...)` adlandırılmış
   argümanla) ve §8'deki "DÜRÜST BULGU" paragrafı **doğru sayılarla yeniden yazılmalı**.
   Bonus: `LlmTierSelector`'ın iki overload'ı bu tuzağı taşıyor — convenience overload'a
   ayrı bir isim verilmeli (`SelectTierFor(stake, confidence)`).
3. **Demo, iddialarını AYIRT EDEN girdilerle yeniden kurulmalı.** Her iddia için: "bu sayıyı
   değiştirirsem sonuç değişir mi?" Değişmiyorsa o gösteri hiçbir şey göstermiyor. Curator
   örneğinde en az bir kayıt gerçekten bayraklanmalı.
4. **NaN/aralık-dışı doğrulaması `DecisionGravity` ve `BoundedAutonomyGate`'e eklenmeli.**
   Fail-open bir P7 gate'i, olmayan bir gate'ten daha tehlikelidir çünkü güvence hissi verir.
   Politika net olmalı: ölçülemeyen girdi → **blokla**, otonomi verme.
5. **Tüm koleksiyon getter'ları savunmacı kopya döndürmeli** (`.ToList()`/`.AsReadOnly()`),
   ve `Rehydrate` invariant kontrolü yapmalı. "Asla silinmez" bir slogan değil, bir invariant
   olmalı.
6. **Bağımsız denetim kalıcılaşmalı.** Demoyu ve testleri yazan bileşen, kendi sınavını
   yazmamalı. `AdversarialAuditTests.cs` bunun ilk adımı; CI'da ayrı bir gate olarak
   koşturulmalı. Faz-5'in kapı şartı bu olmalı.

### Bu raporun kendi yanlışlanabilirliği (Madde X)

Bulgularımın çoğu tip-sistemi ve IEEE-754 gerçeklerine dayanıyor (yüksek güven). En zayıf
halkam **§2'deki overload analizi** — C# spec'ine dayanıyor ama yürütülerek doğrulanmadı.
`AUDIT_DEFECT_C1` kırmızı yanarsa **§2 tümüyle çürümüş** demektir ve bu raporu buna göre
düzeltmek gerekir. Aynı şekilde `AUDIT_FINDING_G7` kırmızı yanarsa §3.2 çürür.

Testleri koşturun. Kırılan her `AUDIT_*` testi, benim bir hatamdır — ve onu görmek isterim.

---

*ens-skeptic, 2026-07-25. Bu belge koordinatörün README'sinden bağımsızdır ve onunla
çelişebilir; çelişki hâlinde önce hangisinin çalıştırılmış kanıta dayandığına bakın.*
