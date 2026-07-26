# AUDIT-WAVE2 — TRACE Sadakat Denetimi (kod ↔ teori)

> **Bağımsızlık beyanı.** Bu denetimi yazan context, `Ens.Kernel` kodunu, testlerini, demoyu
> ya da README'yi **yazmadı**. Tek girdi: repo'daki dosyalar. Dalga-1 (`AUDIT.md`) kodu
> *kırmaya* çalıştı; bu dalga farklı bir soru sorar: **kod, TRACE ettiği teori bölümünü
> gerçekten uyguluyor mu, yoksa TRACE bir süs mü?**
>
> **Yöntem.** `Ens.Kernel/*.cs` içindeki **55 `// TRACE:` iddiasının tamamı** tek tek alındı;
> her biri için atıf yapılan teori/ADR bölümü açıldı ve satır düzeyinde karşılaştırıldı.
> Test dosyalarındaki 25 TRACE kapsam dışıdır (kanıt değil, niyet beyanıdır).
>
> **⚠️ Bu denetimin sınırı (SKR-041 emsali — fabrikasyon yok).** Bu context'te **shell/Bash
> aracı yoktu**; `dotnet build` ve `dotnet test` **ÇALIŞTIRILAMADI.** Aşağıdaki hiçbir bulgu
> bir test koşusuna dayanmıyor — hepsi **statik kaynak okuması + teori metni karşılaştırması**.
> "Şu çağrı başarılı olur" biçimindeki iddialar (özellikle D-1) kod okumasından türetilmiştir
> ve owner/CI tarafından bir testle teyit edilmelidir. Hiçbir çıktı satırı uydurulmamıştır.

---

## 0. Özet — kaç TRACE, kaçı yanıltıcı

| Verdict | Sayı | Anlamı |
|---|---:|---|
| **uyguluyor** | 36 | Kod, atıf yaptığı bölümü gerçekten ve dar biçimde uyguluyor |
| **kısmen** | 13 | Bölümün bir alt-kümesi uygulanmış; eksik kısım *çoğunlukla* işaretli |
| **atıf yalan** | 6 | Yorum, kodda karşılığı **olmayan** bir şeyi var gibi anlatıyor |
| **Toplam** | **55** | |

**Dürüst ana bulgu — README büyük ölçüde haklı.** `7000-reference-implementation/README.md`
Faz-4 sadeleştirmelerinin **ezici çoğunluğunu zaten dürüstçe işaretliyor** (26 maddelik
"Bilinçli sadeleştirmeler" + 5 maddelik "AÇIK KALAN KUSURLAR" listesi). Bu kod tabanının
öz-raporlama disiplini, gördüğüm ortalamanın çok üstünde: `LlmAdapter.cs`, `ActuationLayer.cs`,
`Guard.cs` ve `Scheduler.cs`'in "dürüst sınırlar" blokları **fazla** bile dürüst (kodlanan
şeyi küçümsüyorlar). Bu denetim o dürüstlüğü kabul eder ve yalnızca **işaretlenMEYEN**
uyuşmazlıkları hedefler.

**Bulunan işaretlenmemiş uyuşmazlık: 18.** Bunlardan **5'i ciddi** (aşağıda D-1..D-5),
**7'si orta**, **6'sı küçük/kayıt-amaçlı**. Ayrıca 2 adet **teori→kod yönünde** desenkron
(teori, kodun durumu hakkında olgusal olarak yanlış konuşuyor) bulundu — bunlar SKR-040/D1'in
tekrarıdır.

---

## 1. CİDDİ — işaretlenmemiş uyuşmazlıklar

### D-1 — `DecisionAggregate` "izsiz commitment yasak (L8)" diyor; **izsiz commitment serbest**

**TRACE (`Domain/DecisionAggregate.cs:8`):**
```
// TRACE: ENS-4025 §L8 (proof-trace invariant — izsiz commitment yasak, Anayasa Madde VI)
```
ve satır 105-106:
```
// TRACE: ENS-4025 §L8 — proof-trace burada doğar: confidence + expectedOutcome +
// selectedAlternative, bu event'in kendisi "hangi kural/öncüllerle" sorusunun cevabıdır.
```

**Teori ne diyor (ENS-4025 L8):** *"Her türetilmiş olgu, onu üreten **kuralı + öncülleri**
taşır. İzsiz çıkarım = black-box = Anayasa Madde VI ihlali (yasak)."*

**Kod ne yapıyor:** `DecisionCommitted(SelectedAlternative, Owner, Confidence, ExpectedOutcome)`
— **kural kimliği yok, öncül listesi yok, `ProofTrace` referansı yok.** Dahası:

- `IdentifyAlternatives(emitter, alternatives, evidence)` — `evidence` **yalnızca null-check'ten
  geçer** (satır 67). Boş liste kabul edilir. `alternatives` için `Count == 0` kontrolü var
  (satır 77), `evidence` için **yok**.
- `Apply()` `AlternativesIdentified`'da yalnızca `Alternatives`'i saklar (satır 159-162);
  `Evidence` aggregate'te bir property olarak **hiç yok**.
- `Commit()` ve `EnsureReplayInvariant` hiçbir noktada Evidence'a bakmaz.

**Sonuç (statik okumadan türetildi, testle teyit edilmeli):**
```csharp
var d = DecisionAggregate.Frame(who, "tedarikçi seç");
d.IdentifyAlternatives(who, ["A", "B"], evidence: []);   // ← BOŞ evidence, kabul ediliyor
d.Commit(who, "A", 0.9, "maliyet düşer");                // ← başarılı: SIFIR öncüllü commitment
```
`ProofTrace.cs` "öncülsüz trace kurulamaz" invariant'ını **gerçekten** zorluyor (constructor
reddediyor) — ama `DecisionAggregate` `ProofTrace`'i **hiç kullanmıyor**. İki dosya birbirine
bağlı değil. Yani "izsiz commitment yasak" cümlesi, ENS'in Madde VI'ya en yakın iddiası,
tam da atom sınırında zorlanmıyor.

**Neden ciddi:** Bu, `ProofTrace.cs`'in dürüstçe işaretlediği "(a) TRACE ÜRETİCİSİ DEĞİL"
sınırının bir alt-kümesi *değil* — orada "kernel bileşenleri henüz otomatik emit etmiyor"
deniyor; burada ise `DecisionAggregate.cs` kendi dosyasında **etmiş gibi** konuşuyor.
ENS-2002 §Implications "Evidence, context'in commit-edilmiş alt kümesidir (P6)" der; kod
Evidence'ı event akışına yazar ama **hiçbir yerde zorlamaz, doğrulamaz, göstermez.**

**Talep (owner: ens-backend-architect):** ya (a) TRACE'i dürüstleştir — "L8 burada
zorlanMIYOR, Evidence opsiyoneldir, ProofTrace bağlanmadı"; ya da (b) `Commit`'i bir
`ProofTrace` (veya en az boş-olmayan Evidence) isteyecek biçimde sıkılaştır. Seçim
ADR-0001 §5.5'e aittir, bu denetime değil.

---

### D-2 — `CapabilityRegistry`: ADR-0001'in **iki** deltasından biri kodda yok, yorum ikisini de var gibi anlatıyor

**TRACE (`Capability/CapabilityRegistry.cs:10` ve 13-21):**
```
// TRACE: ENS-4010 `ens-core:Capability` node'u — Pack, bu node'un örnek-kümesini kaydeder.
// ...
// ENS'İN DAR DELTASI (§6.1'den, abartısız) ... ENS'in gerçek katkısı yalnızca İKİ BAĞ:
//   (1) Eklenti-birimi ENS-4010 `Capability` node'una TİPLENİR (prior-art'ın hiçbiri
//       eklentiyi bir foundational ontology'ye bağlamaz).
//   (2) Pack'in deklaratif izinleri doğrudan Bounded-Autonomy Gate'e beslenir.
```

**Kod ne yapıyor:** Delta **(2) gerçekten kodlu ve iyi kodlu** — `Authorize` → `ToolAuthorization`
→ `BoundedAutonomyGate.Evaluate`; üstelik `Disable`'ın bir kısıtı düşürememesi (satır 155-171)
teorinin bile açıkça söylemediği, doğru bir sıkılaştırma.

Delta **(1) hiç yok.** `CapabilityPack` = `{Name: string, Version: string, AllowedTools:
FrozenSet<string>, RequiresHumanApprovalFor: FrozenSet<string>}`. ENS-4010'a göre bir
`Capability` node'u **Resource profili** taşır ve *"Identity + en az bir kaynak-rol kenarı:
… `supports` (yeti→amaç)"* ister (ENS-4010 satır 138); `supports`'un range'i `{Purpose, Claim}`
(satır 148); `constrains: Constraint → Decision/Capability` (satır 150). Kodda **Purpose bağı
yok, `supports` kenarı yok, `Constraint` yok, node kimliği yok, ontolojiye tek bir referans
yok.** Pack, bir `Capability` node'unun "örnek-kümesini" kaydetmiyor — hiçbir şey kaydetmiyor,
sadece iki string kümesi tutuyor.

**İşaretli mi?** Hayır. Dosyanın "DÜRÜST SINIRLAR (Faz-4)" bloğu (a) marketplace/assembly-scan,
(b) sandbox, (c) progressive 3-tier, (d) versiyon çakışması maddelerini sayar — **node
tiplemesini saymaz.** README satır 52 ve 228-231 de aynı dört şeyi sayar, bunu saymaz.

**Neden ciddi:** ADR-0001 §6.1 kendi delta özetinde şunu yazar: *"§6'nın mekanizması özgün
DEĞİLDİR… ENS'in dar, gerçek katkısı **iki bağ**"*. Kod bu iki bağdan birini uyguluyor,
yorum ise ikisini de ENS'in katkısı olarak sayıyor. Bir prior-art karşısındaki **tek özgünlük
iddiasının yarısı** işaretlenmeden eksik.

---

### D-3 — `Scheduler`/`DecisionGravity`: ENS-3022 §Model 3 formülü **birebir alıntılanmış ama uygulanmıyor**; gerekçe teoride yok

**TRACE (`Scheduler.cs:9`):**
```
// TRACE: ENS-3022 §Model 3 — AttentionPriority(d) ∝ InfoNeed(d) × max(ConformanceDeficit(d), 0)
```

**Teori (ENS-3022 §Model 3, satır 91):** `AttentionPriority(d) ∝ InfoNeed(d) × max(ConformanceDeficit(d), 0)`
— **yalnızca alt kırpma.** §Model 2 (satır 86) `ConformanceDeficit(d) = PeerContext(d) −
ContextDensity(d)` der; bu bir **fark**, normalize bir büyüklük değil.

**Kod (`Laws/DecisionGravity.cs:44-46` + `Guard.NormalizedDeficit`):**
`Math.Clamp(value, 0.0, 1.0)` — **üst kırpma da var.** `deficit = 3.0` veren bir çağıran için
kodun sonucu teorinin formülünün **üçte biri**dir.

Üst kırpma **iyi bir mühendislik kararıdır** (AUDIT §5.6: `deficit = 1e9` ile dikkat kuyruğunu
ele geçirme saldırısı burada kapanıyor) ve muhafazakâr yöndedir. Sorun kırpma değil, **gerekçe**:

> `DecisionGravity.cs:39` — *"ENS-3022 ConformanceDeficit'i NORMALİZE bir açık olarak tanımlar."*

**Bu cümle yanlıştır.** ENS-3022'de "normalize" kelimesi yalnızca **Stake** için geçer
(§Model 1, "Stake, Purpose-tipi içinde normalize edilir — z-skoru/persentil"), deficit için
değil. Yani kod, teoriden bir sapmayı **teoriye atıf yaparak** meşrulaştırıyor. Bu, uydurma
kaynak vermekle aynı sınıfta bir epistemik kusurdur (README'nin kendisi §2 DÜZELTME'de tam
bu dersi veriyor: *"bir teori borcunu bir kod hatasıyla kanıtlamak… aynı ciddiyette bir
epistemik kusurdur"*).

**Kanıt boşluğu:** `SchedulerTests`/`DecisionGravityTests`'te **hiçbir test deficit > 1
denemiyor** (tüm değerler 0.0-0.8 arası) — sapma test yüzeyinde görünmez.
`Priority_matches_DecisionGravity_formula_exactly` testi de `DecisionGravity.AttentionPriority`
ile `Scheduler`'ın *ondan aldığı* değeri karşılaştırır: **tautoloji** (skill §7). Bağlantıyı
kanıtlar, formül sadakatini değil.

**Talep:** ya ENS-3022 §Model 2/3 bir RFC ile `clamp(·,0,1)`'e çekilsin (deficit'in normalize
olduğu teoride *söylensin*), ya da kod yorumu "ENS-3022'den **sapma**, gerekçe AUDIT §5.6"
diye dürüstleşsin. Şu an ikisi de yok.

---

### D-4 — `CompanyMemory` bir **Memory Graph** olduğunu iddia ediyor; ortada graf yok

**TRACE (`Domain/CompanyMemory.cs:6`):**
```
// TRACE: ENS-2003 (Company Memory) §1 (Memory Graph düğümleri = commit-edilmiş kararlar)
```

**Teori (ENS-2003 §Model 1):**
- **Düğümler:** commit-edilmiş kararlar.
- **Kenarlar (Memory Links):** `precedent`, `revision`, `influence`, `similarity(Purpose-tipi)`,
  `contradiction`.
- *"Her düğüm: Decision Object'in **tüm alanları** + `Actual Outcome` + `Learning`."*

**Kod:** `List<MemoryRecord>`; `MemoryRecord = {DecisionId, PurposeType, LearningMagnitude,
AttributionConfidence, AssertedAt}` — **5 alan**, sıfır kenar.

- **Kenar yok.** `precedent`/`revision`/`influence`/`contradiction` kodda hiç geçmiyor.
  `similarity(Purpose-tipi)` bir kenar değil, `Where(r => r.PurposeType == purposeType)`
  string eşitliği. ENS-2003 §Model 1'in mermaid diyagramındaki yapının **hiçbiri** yok.
- **Düğüm içeriği yok.** Purpose, Context, Alternatives, Assumptions, gerekçe, Actual Outcome
  — hiçbiri `MemoryRecord`'da değil. Oysa ENS-2003 §Definition'ın tüm tezi budur:
  *"Veritabanı ne'yi saklar; Company Memory **neden**'i saklar."* Kod `|Learning|` büyüklüğü
  ve bir confidence sayısı saklıyor: bu *ne*'nin bile daha azı.
- **Zincirleme sonuç:** ENS-2002 §Model 2 relevance kestirimi, ENS-3022 §Model 2 PeerContext
  fiti ve ENS-2004 §4a'nın "hangi ortak varsayımdan geldiği" analizi — **üçü de** bu eksik
  alanlara bağlıdır. Yani bu tek eksik, üç ayrı "henüz bağlanmadı" borcunun asıl kök nedenidir;
  README onları üç ayrı borç gibi sayıyor.

**İşaretli mi?** Hayır. README yalnızca *"Purpose-tipi string'dir"* ve *"Exploration modu (§4)
kodlanmadı"* diyor (ikisi de doğru ve işaretli). "Memory Graph"ın graf olmadığı, düğümün
Decision Object'i taşımadığı **hiçbir yerde yazmıyor.**

**Not:** §3a/§3b (decay, `asserted_at`/`last_verified`, Curator-yalnızca-sinyal) **gerçekten
ve titizlikle** kodlanmış — `λ(c)=λ_base·(1−c)^γ` birebir, `γ>0` guard'ı teoriyle tutarlı,
`FindStale`'in saf tazelik eksenine inmesi doğru. Bu bölümün sadakati örnek niteliğinde.
Kusur §1'de, §3a'da değil.

---

### D-5 — ENS-2003 §3a "iki dik eksen" diyor; kodda **confidence iki kez sayılıyor** (ve tam da karşı-survivorship'i zayıflatıyor)

**TRACE (`Domain/CompanyMemory.cs:7-9`):** *"…retrieval bileşiği `Salience` = RetentionPriority
× decayFactor (SKR-040/D2 — **iki dik eksen çarpımla birleşir**, FindStale bölerek ayrıştırır)"*

**Teori (ENS-2003 §3a "İki dik eksen"):** *"Retention önceliği (∝|Learning|, §3 — **önem**
ekseni) ile decayFactor (∝ zaman × belirsizlik, §3a — **tazelik** ekseni) kavramsal olarak
**ortogonaldir**."*

**Kod:**
```
RetentionPriority(m) = DecisionCapital.Value(|L|, c) = |L| · c
decayFactor(m,t)     = exp( −λ_base·(1−c)^γ · Δt )
Salience(m,t)        = |L| · c · exp( −λ_base·(1−c)^γ · Δt )
```
**Her iki "dik" eksen de `c`'nin (AttributionConfidence) monoton artan fonksiyonudur.**
Yüksek-`c` bir kayıt **iki kez ödüllendirilir** (yüksek retention **ve** yavaş sönüm);
düşük-`c` bir kayıt **iki kez cezalandırılır**. Ortogonallik istatistiksel olarak yanlıştır —
ortak bir sürücü var.

**Neden ciddi (kozmetik değil):** ENS-2003'ün beyan ettiği **dar delta** karşı-survivorship'tir:
*"başarısız ama ölçülmüş kararlar en yüksek retention önceliğini alır."* Ama bir başarısızlığın
atfı tipik olarak **zayıftır** (§Failure "counterfactual infeasibility", ENS-2004 §3: çoğu karar
L1'e sıkışır). Yani düşük-`c`'li başarısızlık dersi — korunması *en çok* istenen kayıt —
kodda hem düşük retention alır hem hızlı söner. Mekanizma, korumak için tasarlandığı şeyi
sistematik olarak aşağı iter.

**Kimin kusuru:** Kod burada teoriye **sadıktır** — ENS-2004 §Implications açıkça
*"Memory retention = |learning_signal| × attribution_confidence"* der ve ENS-3023 §Model 1
`value(d)=|Learning|·attribution_confidence` verir. Yanlış olan **kodun formülü değil,
ortogonallik iddiasıdır** — ve o iddia hem teoride hem kod yorumunda tekrarlanıyor.
SKR-040/D2 *isim* sürüklenmesini kapattı; **çift-sayım** üç skeptic turundan (SKR-040,
SKR-041) sağ çıktı.

**Talep (owner: ens-philosopher, ENS-2003 §3a):** ya "ortogonal" iddiası geri çekilsin
(dürüst hâli: "iki eksen ortak bir `c` sürücüsü paylaşır; bu bilinçlidir ve karşı-survivorship'i
zayıflatır"), ya da retention `c`'den ayrıştırılsın (`RetentionPriority = |L|`, `c` yalnızca
sönümde). İkincisi ENS-2004 §Implications'ı da değiştirir — RFC işi.

---

## 2. ORTA — işaretlenmemiş uyuşmazlıklar

### O-1 — ENS-3021 §Model 5 (kestirim metodolojisi) **tamamen yok** ve hiçbir yerde işaretli değil
`DecisionEntropy.cs:3` dosya-başı TRACE'i ENS-3021'i **bütün olarak** iddia ediyor. §Model 5
şunu ister: (a) **Miller-Madow** ya da benzeri sonlu-örneklem düzeltmesi (naif tahmin
"aşağı-yanlıdır"), (b) binning şemasının raporlanması, (c) düşük-hacimli Purpose-tiplerinde
**güven aralığı**. Repo genelinde `Miller|Madow|binning|güven aralığı` için **sıfır eşleşme**
(grep, tüm `7000-*`). `ShannonEntropy` saf plug-in tahmincidir. README'nin 26 maddelik
sadeleştirme listesinde **yok**. §Model 5 SKR-011 Bulgu 3'e verilmiş bir yanıttır — yani
skeptic'in bir kez zorladığı gereksinim, kodda sessizce düşmüş.

### O-2 — `LevelNoise` bir **artık**tır; "zincir kuralı doğrulandı" iddiası tautoloji
Kod: `LevelNoise = max(0, H(A|C) − H(A|C,Owner))`. Yani `I(A;Owner|C)` kendi tanımından
(`Σ p(c,o,a)·log[p(a|c,o)/p(a|c)]`) **hesaplanmıyor**, çıkarma ile *tanımlanıyor*.
`DecisionEntropyTests.Chain_rule_holds_...` (satır 40-58) `hac == levelNoise + patternNoise`
assert ediyor — bu **her girdi için inşa gereği doğrudur**, hiçbir şey kanıtlamaz.
ROADMAP satır 195-196 bunu *"zincir-kuralı **matematiksel olarak doğrulandı**"* diye kaydediyor:
**olgusal olarak yanlış.** (Formül doğru; kanıt yok. `ConditionalEntropyGivenOwner` ve
`ConditionalEntropy` bağımsız ve doğru hesaplanıyor — sorun yalnızca üçüncü terimde.)

### O-3 — ENS-2004 §4a constraint-gate'in **iki şartından biri** (≥L1 attribution) kodlanmamış, işaretlenmemiş
Teori (§4a, P7 kapısı): *"constraint-gate (ör. öneri **kendi proof-trace'ini taşımalı**, **en az
L1 etiketli olmalı**)"*; adım 2: *"Sistematik sinyal en az L1 attribution (§3) ile etiketlenir."*
- proof-trace şartı: **işaretli** (`ReflectiveDoubleLoop.cs:10-12` ve README).
- L1 şartı: **işaretsiz ve uygulanmıyor.** `AttributionLevel` enum'ı `DecisionEvents.cs`'te var
  ama `MemoryRecord`'a **taşınmıyor** — merdiven bellek katmanına geçerken düşüyor.
  `ReflectiveProposal.AverageAttributionConfidence` merdiven seviyesi **değildir**: L0
  ("atıf yok") bir kayıt pekâlâ `AttributionConfidence = 0.9` taşıyabilir. Yani bugün
  `Propose`, tamamı L0 olan kayıtlardan "sistematik" önerisi üretebilir — teorinin
  constraint-gate'inin yasakladığı tam şey.

### O-4 — ENS-2003 §3'ün üçüncü politikası ("Sıkıştır / Decision DNA") kodda yok, işaretsiz
`CompanyMemory.cs:7` §3'ü bütün olarak TRACE ediyor. §3'ün üç maddesinden ikisi
(retention ∝ |Learning|, decay-not-delete) kodlu; üçüncüsü — *"tekrarlayan kararlar bir örüntüye
(Decision DNA) özetlenir; ama **en az bir başarısızlık örneği** örüntü içinde korunur"* —
yok. Grep: `DNA|sıkıştır|compress` → `7000-*` içinde sıfır eşleşme. README'de yok.
ENS-3023 §Implications'ın *"ROI neyi saklamaya/**sıkıştırmaya** değdiğini yönlendirir"*
bağı da bu yüzden kopuk.

### O-5 — `ContextScore` "ters-U"yu TRACE ediyor; kodda ters-U yok
`ContextScore.Compute` docstring'i *"(§3, ters-U: az context = gap, çok context = noise)"* diyor.
Kod: `coverage − noisePenalty − staleness` — **coverage'da monoton artan.** Ters-U ancak
`noisePenalty` `coverage`'ın fonksiyonu olursa doğar; kodda üçü de **bağımsız girdi**.
LAW-CONTEXT'in yanlışlanabilirlik terfisi (*"tek yönlüden **iki yönlü**ye"*, ENS-2002 §Model 3)
bu sınıfta yapısal olarak temsil edilmiyor.
*Dürüst kayıt:* ENS-2002'nin kendisi de `ContextScore`'u lineer yazıp U'yu `g()`'ye koyuyor —
yani kod formüle sadık; **fazla iddialı olan yorumdur.**

### O-6 — `GateConfidence`'ın kapılama eğrisi **uydurma** (teoride karşılığı yok) — üstelik kod tabanının kendi disiplinine aykırı
ENS-2002 §Implications yalnızca şunu der: *"düşük Score → düşük Confidence ya da ertele."*
Kod bunu somut bir eğriye çevirir: `cap = clamp(contextScore / threshold, 0, 1)`. Bu
oran-kapılama teoride **yoktur**; seçilmiş bir fonksiyonel formdur ve kalibre değildir.
Karşılaştırın: `Scheduler.cs:36-39` üçüncü bir tie-breaker eklemeyi reddediyor çünkü
*"ENS-3022 böyle bir ölçüt vermediği için UYDURULMADI"* (README de bunu Madde IX gerekçesiyle
savunuyor). Aynı disiplin `ContextScore.GateConfidence`'a uygulanmamış ve sapma işaretlenmemiş.

### O-7 — `DecisionGravity`'nin OL1 mazereti **olgusal olarak yanlış** (D1-sınıfı desenkron)
`DecisionGravity.cs:7-9`: *"`Stake` burada dışarıdan verilir — **ENS-2001'e henüz
Alternative-başına ExpectedValue eklenmedi** (OL1, ROADMAP açık borç)"*.
**ENS-2001 v0.3.0 (ratified, SKR-033 survives, 2026-07-24) ExpectedValue'yu içeriyor**
(§Model 2 anatomi tablosu satır 127/139, §Model 3 faz yerleşimi satır 181). Borç **kodda**,
teoride değil. README aynı hatayı satır 181-182'de tekrarlıyor ve **kendi kendisiyle
çelişiyor**: satır 190-192 doğru hâli söylüyor (*"artık ENS-2001 v0.3'te teoride var… ama
`DecisionAggregate`'e henüz kodlanmadı"*). ROADMAP satır 150 de doğru hâli taşıyor.
Yani üç yerden ikisi doğru, biri (kod yorumu + README'nin bir maddesi) bayat.

---

## 3. KÜÇÜK / kayıt amaçlı

| # | Bulgu | Yer |
|---|---|---|
| K-1 | **"TOVE Empowerment" ADR-0001 §5.6'ya atfedilmiş — orada geçmiyor.** Kavram gerçek ve kaynaklı (Fox & Grüninger; `ENS-4001-meta-model.md:201`), ama §5.6 yalnızca `Policy = ens-core:Constraint bundle` der. Bölüm referansı yanlış — uydurma değil, yanlış adres. | `Domain/Events/DecisionEvents.cs:19-20` |
| K-2 | **ENS-2003 §3a confidence aralığını `[0.3, 1.0]` ilan eder** ("alt taban bir gürültü zemini"); kod `Guard.UnitInterval` ile `[0,1]` kabul eder. `c=0` teorinin dışladığı bir durumdur ve `FindStale` düzeltmesinin gerekçesi tam bu kayıtlar üzerinedir. İşaretsiz. | `Domain/CompanyMemory.cs:61` |
| K-3 | **`ReuseROI`, ENS-3023 §Model 3'ün payını yarıya indiriyor.** Teori: `Σ (reuse'un düşürdüğü InfoNeed **/ iyileştirdiği outcome**) / bakım maliyeti`. Kod yalnızca `infoNeedReduction` alır; outcome terimi düşmüş. İşaretsiz (küçük, çünkü outcome atfı zaten R2'ye zincirli). | `Laws/DecisionCapital.cs:40` |
| K-4 | **ENS-3021 Purpose-tipi bölütlemesi yok.** §Definition: *"aynı Purpose-tipinde **ve** benzer Context'te"*. Kod yalnızca `ContextKey`'e göre gruplar; Purpose-tipi kavramı `DecisionEntropy`'de hiç yok. README yalnızca ContextKey↔ContextScore borcunu işaretliyor. | `Laws/DecisionEntropy.cs:26` |
| K-5 | **ProofTrace, L3 (Unknown) ve L5 (temporal scope) ile gerilimde.** `Premise.Confidence` zorunlu `double [0,1]` — **"Unknown" temsil edilemez** (L3: *"Unknown ≠ False"*), bilinmeyen bir öncül sayıya dönüştürülmek zorunda. Ayrıca öncülde `valid_from/valid_to` yok → geçerliliği dolmuş bir öncülden türetim engellenemez (L5) ve `AsPremise()` confidence'ı **dondurur** (`CompanyMemory`'nin decay'inin tersi). *Hafifletici:* dosya yalnızca L7/L8 TRACE ediyor, L1-L6 iddia etmiyor — bu yüzden "yalan" değil, **kapsam-dışı**. Yine de "dürüst sınırlar (a)-(d)" listesinde yer almalı. | `ProofTrace.cs:47-63` |
| K-6 | **`isIrreversible` bir Policy kısıtı değil, çağıran beyanıdır.** ADR-0001 §5.6 Policy'yi *"geri-dönülemezlik **eşiği** Constraint'i"* olarak sayar (bir sayı); kod bir `bool` alır ve onu kim koyduğunu doğrulayamaz. `CriticalBlock` dalı bu beyana tamamen bağımlı. Policy'nin minimalistliği işaretli, ama bu özel nokta değil. | `BoundedAutonomyGate.cs:75` |

---

## 4. Ters yön — **teori**, kod hakkında yanlış konuşuyor (SKR-040/D1 tekrarı)

Bu iki bulgu 7000'in değil, 2000'in kusurudur; ilgili owner'lara aittir.

### T-1 — ENS-2004 §Failure: *"§4a … Faz-4'te **kodlanmadı** — eng-kanıt E1 (tasarlanmış-ama-implemente-değil)"*
`Ens.Kernel/Domain/ReflectiveDoubleLoop.cs` **var** (README: 10/10 test). Bu, SKR-040'ın
ENS-2003'te yakaladığı D1 kusurunun (*"7000 formülü henüz implemente etmemiştir" olgusal
yanlıştı*) **birebir tekrarıdır** — bu kez ENS-2004'te. Doğru gerekçe (ENS-2003'ün öğrendiği
ders): "kodlanmadı" değil, **"zayıf yaklaşıkla kodlandı + kalibre edilmedi"**.

### T-2 — ENS-2004 künyesinde `evidence:` alanı **hiç yok**
`ENS-2004-learning-theory.md` front-matter'ı (satır 1-18) `evidence` satırı içermiyor —
ENS-2001/2002/2003/3021/3022/3023'ün hepsinde var. Buna rağmen README satır 43
*"ENS-2004 §4a … eng: E0 → **E1**"* terfisini kaydediyor: **yazılacak alan yok.**
Şema ihlali (metadata-header.md) + terfinin izlenemezliği.

### T-3 (bonus, 7000 tarafı) — README, `DecisionCapital.cs`'in zaten kapattığı bayat iddiayı sürdürüyor
SKR-041/N1 *"`DecisionCapital.cs:8`'de bayat 'Company Memory henüz kodlanmadı' yorumu"* dedi.
Kod tarafında **kapatılmış** (satır 8-9 artık *"Company Memory artık kodlu… ama bu iki sınıf
henüz birbirine bağlanmadı"* diyor — doğru). Ama README satır 187-189 hâlâ eski hâli taşıyor:
*"…Company Memory (ENS-2003) **henüz kodlanmadı**, bu yüzden yalnızca akış… kodlandı."*
N1 yarım kapandı.

---

## 5. TRACE-bazlı tam tablo (55 iddia)

| Dosya | TRACE sayısı | uyguluyor | kısmen | atıf yalan | Not |
|---|---:|---:|---:|---:|---|
| `Domain/Identity.cs` | 1 | 1 | – | – | Ontolojik açıklığı dürüstçe kabul ediyor |
| `Domain/DomainEvent.cs` | 2 | 1 | 1 | – | Axiom 3 yorumu geniş; asıl invariant `DecisionAggregate`'te |
| `Domain/Events/DecisionEvents.cs` | 4 | 2 | 1 | 1 | Lifecycle tam, Decision Object eksik (OL1/OE1); K-1 |
| `Domain/DecisionAggregate.cs` | 5 | 2 | 1 | 2 | **D-1** (L8 iddiası ×2); §Individuation dört koşulu gerçekten kodlu |
| `Domain/ContextScore.cs` | 2 | – | 2 | – | O-5, O-6 |
| `Domain/CompanyMemory.cs` | 5 | 3 | 1 | 1 | **D-4** (§1); §3a/§3b sadakati örnek nitelikte; O-4, K-2 |
| `Domain/ReflectiveDoubleLoop.cs` | 2 | 1 | 1 | – | Sadeleştirmeleri kendisi ilan ediyor; O-3 açık |
| `Laws/DecisionEntropy.cs` | 4 | 2 | 2 | – | O-1, O-2, K-4 |
| `Laws/DecisionGravity.cs` | 3 | 3 | – | – | Formül birebir; sapma docstring'de (**D-3**), O-7 |
| `Laws/DecisionCapital.cs` | 2 | 2 | – | – | Stok yokluğu teorice meşru; K-3 |
| `Adapter/LlmAdapter.cs` | 3 | 3 | – | – | En dürüst dosya; "ne olduğu/ne olmadığı" bloğu örnek |
| `Scheduler.cs` | 3 | 2 | – | 1 | **D-3** (alıntı ≠ kod) |
| `ProofTrace.cs` | 4 | 2 | 2 | – | L7 birebir; L8 kardinalite düzeyinde (işaretli); K-5 |
| `ActuationLayer.cs` | 4 | 4 | – | – | State machine ADR §5.4 diyagramıyla **birebir**; sınırlar eksiksiz işaretli |
| `Capability/CapabilityRegistry.cs` | 4 | 2 | 1 | 1 | **D-2**; delta (2) örnek nitelikte kodlu |
| `BoundedAutonomyGate.cs` | 4 | 3 | 1 | – | Policy 4 kısıttan 2'si (işaretli); K-6 |
| `Guard.cs` | 3 | 3 | – | – | "Doğrulama kapısı, kalibrasyon kapısı değil" — doğru ve dürüst |
| **Toplam** | **55** | **36** | **13** | **6** | |

---

## 6. Sağlam çıkanlar (yalnızca kusur listelemek de bir manipülasyon türüdür)

Bu dalga, aşağıdakileri **gerçekten teori-sadık** buldu — hiçbiri süs değil:

1. **`ENS-4025 L7` min t-norm** — `Confidence = snapshot.Min(p => p.Confidence)`, atanamaz,
   `AsPremise()` ile zincirde monoton azalır. Teorinin cümlesiyle birebir.
2. **`ENS-2003 §3a` sönüm ailesi** — `λ(c)=λ_base·(1−c)^γ`, `γ>0` guard'ı teorinin *tek*
   kısıtıyla aynı, `HalfLifeDays = ln2/λ`, `decayFactor` (saf) ile `Salience` (bileşik)
   ayrımı SKR-040/D2'ye sadık, `FindStale` gerçekten saf tazelik eksenine iniyor.
3. **`ENS-2003 §3b` Curator** — "yalnızca inceleme sinyali": `FindStale` **hiçbir şeyi
   silmiyor/değiştirmiyor**, liste döndürüyor. P7 yorumu doğru ve muhafazakâr.
4. **`ENS-2001 §Individuation`** — dört koşul (tek Owner/Purpose/açık Alternatives/tek
   Commitment) hem canlı hem **replay** yolunda zorlanıyor. Replay'in birincil yol olduğunu
   fark etmiş olmaları önemli.
5. **`ADR-0001 §5.4` state machine** — `Allowed` sözlüğü ADR'nin mermaid diyagramıyla
   düğüm-düğüm, kenar-kenar aynı; `Failed → Traced` zorunluluğu ("izsiz başarısızlık yasak")
   teoride *ima* edilen bir şeyi koda çevirmiş.
6. **`ADR-0001 §6.1(2)`** — Registry→Gate bağı. Üstelik `Disable`'ın bir kısıtı düşürememesi,
   teorinin söylemediği ama P7'nin gerektirdiği doğru asimetri.
7. **`ENS-3022 §Model 1`** — `InfoNeed = Stake × (1−Confidence)`, `confidence=null → 1.0`
   belirsizlik. Birebir, muhafazakâr yön doğru.
8. **`Guard.cs` fail-closed politikası** — teoriden değil AUDIT'ten geliyor ama P7'nin
   ("ölçülemeyen girdi otonomi kazanamaz") en dürüst kodlanışı; `BoundedAutonomyGate`'in
   fail-closed dallarını **doğrulamadan önce** koyması (satır 77-92) doğru sıralama.

---

## 7. Talepler (owner'a bırakılır — bu denetim yapıtı düzeltmez)

**7000 / ens-backend-architect:**
1. **D-1:** `DecisionAggregate`'in L8 TRACE'ini ya dürüstleştir ya `Commit`'i sıkılaştır.
   (Şu an kod, kendi dosyasında ihlal ettiği bir invariant'ı iddia ediyor.)
2. **D-2:** `CapabilityRegistry`'nin "İKİ BAĞ" bloğuna *"(1) HENÜZ KODLANMADI"* notu.
3. **D-3:** `DecisionGravity.cs:39`'daki *"ENS-3022 … NORMALİZE olarak tanımlar"* cümlesini
   sil; yerine *"ENS-3022 §Model 3'ten **sapma**; gerekçe AUDIT §5.6; teori borcu açık"* yaz.
   Ek olarak `deficit = 5.0` için bir `AUDIT_DEFECT_*` testi — sapmayı görünür kılsın.
4. **D-4:** `CompanyMemory` dosya-başına: *"Memory **Graph** değil, düz kayıt listesi;
   Memory Links (§1) kodlanmadı; düğüm Decision Object'i taşımıyor"*.
5. **O-1/O-2/O-3/O-4/O-5/O-6/K-*:** README'nin "Bilinçli sadeleştirmeler" listesine ekle.
6. **O-7 / T-3:** README satır 181-182 ve 187-189'daki bayat iki cümleyi düzelt.

**2000 / ens-philosopher:**
7. **D-5:** ENS-2003 §3a'nın "ortogonal" iddiası — geri çekilsin ya da retention `c`'den
   ayrıştırılsın. (Bir RFC gerektirebilir: ENS-2004 §Implications'ı da etkiler.)
8. **T-1:** ENS-2004 §Failure'daki *"Faz-4'te kodlanmadı"* düzeltilsin (ENS-2003 v0.3.1'in
   D1 düzeltmesiyle aynı biçimde).
9. **T-2:** ENS-2004 künyesine `evidence:` alanı eklensin.

**3000 / ens-philosopher + 5000 / ens-architect:**
10. **D-3 teori tarafı:** ENS-3022 §Model 2/3 — `ConformanceDeficit` normalize mi değil mi?
    Kod bir cevabı zaten uyguluyor; teori sessiz. Sessizlik, kodun teoriyi **sessizce
    yazmasına** izin veriyor (Madde XII'nin ters yönü).

---

## 8. Bu denetimin kendi sınırları

- **`dotnet build`/`dotnet test` çalıştırılmadı** (bu context'te shell yoktu). D-1'in kod
  örneği statik okumadan türetilmiştir; bir `AUDIT_DEFECT_*` testiyle teyit edilmelidir.
- **Test dosyalarının içeriği denetlenmedi** (25 TRACE); yalnızca D-3/O-2 için ilgili iki test
  okundu. Testlerin başka tautolojiler taşıyıp taşımadığı **açık**.
- **Demo (`Ens.Kernel.Demo/Program.cs`) okunmadı** — dalga-1 zaten 4 sunum kusuru bulmuştu.
- **`LlmAdapter.cs`'in web-doğrulanmış sağlayıcı iddiaları** (Cerebras ücretsiz kademe,
  DeepInfra kataloğu) **bu turda yeniden doğrulanmadı** — kaynak-uydurma taraması yapılmadı.
- **ENS-4020 (Enterprise Ontology), ENS-4030, ENS-4031** okunmadı; Purpose-tipi taksonomisinin
  oradaki hâli ile kodun string'i arasındaki mesafe ölçülmedi.
- Bu rapor **kusur bulmaya** ayarlıdır; §6 ("sağlam çıkanlar") o yanlılığa karşı bilinçli bir
  dengedir ama tam değildir.
