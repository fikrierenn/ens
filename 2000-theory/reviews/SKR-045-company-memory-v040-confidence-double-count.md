---
id: SKR-045
type: skeptic-review
origin: ENS-2003
depends_on: [ENS-2003, ENS-2004, SKR-040, SKR-041]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-25
---

# SKR-045 — Company Memory v0.4.0 (D-5 confidence double-count düzeltmesi) Saldırısı

**Bağımsızlık beyanı:** ENS-2003 v0.4.0'ı yazan `ens-philosopher` çağrısından ayrı, taze context'te
yapıldı (G2/G3 öz-onay yasağı). Bu, v0.4.0 formül değişikliğinin **1. bağımsız skeptic turudur**.
`dotnet test` **çalıştırılamadı** (bu context'te shell aracı yok); hiçbir test çıktısı iddia
edilmemiştir.

## Verdict

`wounded` — **çekirdek düzeltme sağ çıkıyor** (`c` bir ölçüm, `1/τ_π` bir süreç özelliğidir;
çift-sayım gerçekten kaldırıldı ve `c` artık sönüme hiç girmiyor — kodda da öyle), **ama D-5'i
kapatan hamlenin kendisi yeni ve daha sert bir çelişki açtı:** `c` retention'dan çıkarılınca,
ENS-2004 §3/§Laws'ın **L0 kuralını** ("atıf yok → yalnızca kayıt, **L0'da learning yok**")
zorlayan tek operasyonel mekanizma da ortadan kalktı; üstelik yeni **karşı-survivorship tabanı**,
`c = 0` (yani ENS-2004'e göre *öğrenme içermeyen*) bir kaydı, `|Learning|`'i en büyükse, her
kesilen retrieval'da **yapısal olarak garanti eder**. Bu, `refuted` değildir çünkü kusur
çekirdek tezde değil **tabanın koşulsuz hâlindedir** ve düzeltmesi çift-sayımı geri getirmez
(gate ≠ multiplier). Ama `ratified`'a dönüşü **bloke eder**.

**Blocking bulgular:** B1 (L0 çelişkisi + koşulsuz taban), B2 (taban yalnızca `RetrieveTop`'ta
var — çağıran-tarafı kesme onu tümüyle atlar; "yapısal invariant" iddiası kodda karşılanmıyor),
B3 (ENS-2004 v0.3.3 "additive" etiketi olgusal olarak yanlış — breaking).
**Non-blocking:** N1 (Kalman ↔ EW-RLS mimari seçimi gerekçesiz), N2 (Goodhart: `|L|` ex-ante
oyunlanabilir ve taban tek-slot ele geçirmeye açık), N3 (`confidence` homonimi), N4 (fail-open
varsayılan parametreler).

## Kapsam ve önceki turlar

**Bağımsızlık beyanı.** Bu inceleme, ENS-2003 v0.4.0'ı yazan `ens-philosopher` çağrısından ayrı,
taze context'te yapıldı (G2/G3). ENS-2003 v0.4.0 (§3, §3a, §3b, §5, §Failure, §AUDIT-WAVE2 D-5
yanıtı), ENS-2004 v0.3.3 (§1, §2, §3, §5, §Implications, §Laws, §Failure, §v0.3.3 notu) ve Faz-4
`CompanyMemory.cs` tümüyle okundu.

**Tekrarlanmayanlar.** SKR-040 (`wounded`: D1/D2/D3 teori-kod desenkronu) ve SKR-041 (`survives`:
D1/D2/D3 kapanışı, `γ>0` hizası, pusula fit-imkânsızlığı) bulguları bu turda **yeniden
saldırılmadı**. `γ` ve `λ_base` v0.4.0'da kaldırıldığı için D3 ve pusula-fit tartışması **konusuz**
kalmıştır (doc bunu dürüstçe "bir açmazı konusuz kılmak onu çözmek değildir" diye yazar — bu
formülasyon doğrudur ve kabul edilir). SKR-040/N-serisi artıklardan **N2 kapandı** (glossary),
**N1/N3 (DecisionCapital.cs:8 bayat yorumu, "memory assertion ≠ ENS-4010 Assertion" homonimi)**
durumu bu turun kapsamı dışıdır.

Bu tur **yalnızca v0.4.0'ın getirdiklerine** saldırır: `RetentionPriority = |Learning|`,
`λ_π = ln2/τ_π`, karşı-survivorship tabanı, `weakly-attributed` sinyali ve ENS-2004 v0.3.3
hizalaması.

## B1 (BLOCKING) — `c` çarpan olmaktan çıkınca ENS-2004'ün L0 kuralı zorlanamaz oldu

**İddia.** v0.4.0'da `RetentionPriority(m) = |Learning(m)|` ve taban `argmax_m RetentionPriority(m)`
kaydını seçer — **`c`'ye hiç bakmadan**. Dolayısıyla `c = 0` olan, yani ENS-2004 §3'e göre **L0**
("atıf yok — saf confounding; yalnızca kayıt") seviyesindeki bir kayıt, `|Actual − Expected|` değeri
tipin en büyüğüyse, o Purpose-tipinin **karşı-survivorship tabanı olur** ve her kesilen retrieval'da
bir slotu **garantili** işgal eder.

**Çelişen metinler — ikisi de Külliyat adayı:**

| Kaynak | İfade |
|---|---|
| ENS-2004 §3 (attribution merdiveni) | **L0** \| atıf yok (saf confounding) \| *"yalnızca kayıt (memory)"* \| Güven: **—** |
| ENS-2004 §Laws | *"ölçüm attribution seviyesiyle etiketli olmalı; **L0'da learning yok**"* |
| ENS-2003 v0.4.0 §3 | *"Retention önceliği = `\|Learning\|` — outcome'un pozitifliği değil **ve attribution confidence de değil**"* |
| ENS-2003 v0.4.0 §3, taban | *"tipin `argmax_m RetentionPriority(m)` kaydı kesilen kümede **kalmak zorundadır** — `c(m)` ne kadar düşük ... olursa olsun"* |

ENS-2004 bir yasa cümlesiyle "L0'da learning **yok**" der. ENS-2003 v0.4.0 ise retention önceliğini
`|Learning|`'e eşitler ve tabana `c`'yi görmeyi **açıkça yasaklar**. L0'da learning yoksa, L0 bir
kaydın `|Learning|`'i **tanımsızdır** — ama ENS-2003 onu bir sayı olarak okur ve o sayı en büyükse
kayda **maksimum korumayı** verir. Bir yasanın "yok" dediği şey, komşu yapıtta korunacak en değerli
şey hâline gelir.

**Kod bunu doğruluyor (spekülasyon değil).**
- `MemoryRecord` kurucusu: `Guard.UnitInterval(AttributionConfidence, ...)` → **`c = 0` geçerli bir
  kayıttır**, belleğe girer.
- `MemoryRecord.RetentionPriority => LearningMagnitude;` → `c` yok.
- `CompanyMemory.CounterSurvivorshipFloor(...)` yalnızca `r.RetentionPriority` karşılaştırır;
  `AttributionConfidence` bu metotta **hiç geçmez**.
- `RetrieveTop(...)`: `if (floor is not null && !top.Any(r => ReferenceEquals(r, floor))) top[^1] = floor;`

Somut senaryo (kodun kabul ettiği): `π = "talep tahmini"`, kayıt A: `|L| = 40, c = 0.0` (sonuç tümüyle
confounder'a gidiyor — L0), kayıt B: `|L| = 9, c = 0.8` (L2, gerçek ders). `limit = 3`, tipte 10 kayıt.
A'nın `CapitalValue = 40 × 0 = 0` → `Salience = 0` → sıralamada **sonuncu**. Ama `RetentionPriority(A)
= 40 > 9` → A **tabandır** ve `top[^1] = A` ile üç slottan birini alır; yerinden ettiği kayıt, gerçekten
atfedilmiş bir derstir. Yani mekanizma, sıralamada **en dipteki** kaydı zorla üste enjekte eder ve bunu
yaparken ENS-2004'ün "burada öğrenme yoktur" dediği kaydı seçer.

**v0.3 bu hatayı yapmıyordu — ve bu, D-5 düzeltmesinin gizli maliyetidir.** v0.3'te
`RetentionPriority = |L|·c` idi; `c = 0` ⇒ retention `0`. Yani L0 kuralının **tüm Külliyat'taki tek
operasyonel dişi** bu çarpandı. v0.4.0 çarpanı — haklı bir gerekçeyle — kaldırdı, ama **yerine hiçbir
şey koymadı**. Üstelik aynı sürümde iki değişiklik daha aynı yöne itti:
1. §3a'da `c ∈ [0.3, 1.0]` beyanı **kaldırıldı** ve `c ∈ [0,1]` yapıldı (v0.4.0 yan etkileri, K-2).
   Yani `c` için var olan **alt taban da** aynı sürümde silindi.
2. Taban invariant'ı eklendi — `c`'yi görmemesi **açıkça şart koşuldu**.

Üç değişiklik ayrı ayrı savunulabilir; **birlikte** L0'ı korumasız bırakır ve L0 kaydına yapısal
ayrıcalık verir.

**Doc'un mevcut failure condition'ı bunu KAPSAMIYOR.** §Failure "(c) memory poisoning'i yükseltir:
`|Learning|` **yanlış ölçülmüşse** ... taban o zehirli kaydın görünürlüğünü garanti eder" der. Bu,
`|L|`'deki **ölçüm gürültüsüdür**. B1 ise farklı ve daha sert bir vaka: `|L|` **doğru ölçülmüş**
olabilir (fark gerçekten 40'tır) ama farkın karara atfı **sıfırdır** — ENS-2004'e göre orada ders
değil, yalnızca bir olay vardır. Doc'un itirafı gürültüyü kapsar, **kategorik L0 vakasını kapsamaz**.

**Neden `refuted` değil.** Düzeltme ucuz, yerel ve **çift-sayımı geri getirmez**: `c`'yi bir *çarpan*
(weighting) olarak değil bir *kapı* (admission gate) olarak kullanmak yeterlidir —
`RetentionPriority(m) = |L(m)|` **ancak** `attribution_level(m) ≥ L1` (denk: `c ≥ c_L0 > 0`) ise;
aksi hâlde kayıt saklanır (never-delete korunur) ama **taban yarışına girmez**. Kapı `c`'yi sönüme
sokmaz, `|L|`'i `c` ile ölçeklemez — v0.4.0'ın Kalman argümanına **hiç dokunmaz**. Dahası bu kapı,
doc'un kendi çerçevesinden **türetilebilir**: Kalman'da ölçüm gürültüsü sonsuza giderse kazanç (gain)
sıfırdır — o gözlem kestirimi **hiç güncellemez**. `c = 0` tam olarak budur. v0.4.0 `c`'yi doğru
biçimde kazanç tarafında (`value`) tutar, ama **taban kazancı tümüyle baypas eder** — tabanın Kalman
karşılığı yoktur. v0.4.0'ın "un-Kalman" parçası tabandır.

### B1'in kanıtı: ENS'in **kendi testi** L0 kaydını tek sonuç olarak sabitliyor

Bu spekülatif bir senaryo değildir. `Ens.Kernel.Tests/AdversarialAuditTests.cs`,
`AUDIT_FINDING_G8_demo_memory_ordering_is_confounded_age_not_isolated` testi (satır 960-973)
tam olarak B1'i **beklenen davranış olarak** kodlar:

```csharp
var bigButStale   = new MemoryRecord(Identity.New(), "t", 8.0, 0.0,  now.AddDays(-2000));
var smallButFresh = new MemoryRecord(Identity.New(), "t", 2.0, 0.99, now);
counter.Record(bigButStale); counter.Record(smallButFresh);
Assert.Equal(smallButFresh, counter.Retrieve("t", asOf, 0.01)[0]);
...
var counterTop1 = counter.RetrieveTop("t", limit: 1, asOf, 0.01);
Assert.Single(counterTop1);
Assert.Same(bigButStale, counterTop1[0]);
```

`bigButStale` kaydının `AttributionConfidence` değeri **tam olarak `0.0`**'dır — ENS-2004 §3'ün
**L0** tanımının kendisi. Test, `limit: 1` ile yapılan bir retrieval'ın **tek** sonucunun bu kayıt
olduğunu doğruluyor; `c = 0.99` olan taze ve gerçekten atfedilmiş ders **tümüyle elenir**.

**Testin kendi yorumu iddiasıyla çelişiyor.** Aynı testin 966-967. satırlarındaki yorum şöyle der:

> *"bu karşı-örnek SIRALAMADA hâlâ geçerlidir (ve geçerli **OLMALIDIR** — atfı olmayan bir ders
> yeni kararı yönlendiremez)."*

Beş satır sonra ise, `limit: 1` retrieval'ında **yeni kararı yönlendirecek tek kayıt olarak** o
"atfı olmayan ders" döndürülür ve bu `Assert.Same` ile sabitlenir. Yorumun normatif cümlesi
("yönlendiremez") ile testin assert'i ("yönlendiren tek şey odur") aynı ekranda birbirini yalanlar.
Bu, teorinin bir yerde kapatılıp komşusunda açık kalan çelişki deseninin (SKR-043'ün adlandırdığı
desen) **dördüncü tekrarıdır** — bu kez teori-metninde değil, teoriyi savunmak için yazılmış testte.

**Nicel düzeltme: "bir slot" maliyeti küçük `k`'de yanıltıcıdır.** §Failure "taban ... her kesilen
retrieval'da **bir slot** tüketir" ve §3 "sıralamayı **bozmaz**" der. Kod `top[^1] = floor` yapar.
`k = 1` iken `top[^1]` **tek elemandır**: taban bir slot değil **sonuç kümesinin %100'ünü** tüketir
ve sıralamayı bozmakla kalmaz, **tümüyle iptal eder**. `k = 3` iken maliyet %33'tür. Yani beyan
edilen maliyet (`bir slot`) mutlak sayı olarak doğru ama **oransal olarak** küçük `k` rejiminde
felakettir — ve küçük `k`, retrieval'ın (context penceresi) tipik rejimidir. §Failure bu oranı
söylemelidir.

## B2 (BLOCKING) — taban "yapısal invariant" değil, opsiyonel bir metot çağrısıdır

**Teorinin iddiası (§3):** *"Bir Purpose-tipi `π` içinde retrieval sonucu `k ≥ 1` kayda
**kesildiğinde**, `π`'nin `argmax_m RetentionPriority(m)` kaydı kesilen kümede kalmak
**zorundadır**"* — ve patolojinin *"yapısal kapanışı"* olduğu, *"§3'ün üçüncü politikasının
**zorlanabilir** hâli"* olduğu söylenir. Kod yorumu da bunu tekrarlar: *"kesme invariant'ı
(`RetrieveTop`) patolojinin kalan yarısını **YAPISAL olarak** kapatır."*

**Kodun gerçeği:** invariant yalnızca `RetrieveTop` metodunun **içinde** yaşar. `Retrieve` public'tir,
kesilmemiş `IReadOnlyList<MemoryRecord>` döndürür ve herhangi bir çağıran şunu yazabilir:

```csharp
memory.Retrieve(purposeType, asOf, rate).Take(5)   // taban DEVREDE DEĞİL
```

Bu, tabanı ihlal eden ama **tamamen meşru görünen** bir çağrıdır: derleyici uyarmaz, guard
tetiklenmez, hiçbir invariant kontrolü çalışmaz. Yani "kesme tabana tabidir" bir **sistem
özelliği** değil, çağıranın hangi metodu seçtiğine bağlı bir **konvansiyondur**. Kodun kendi
yorumu bunu farkında olmadan itiraf eder: *"bu metot [`Retrieve`] KESMEZ, dolayısıyla
karşı-survivorship tabanına **ihtiyaç duymaz**"* — bu doğru değildir; `Retrieve` kesmez ama
**çağıranı keser**, ve kesme tam da tabanın gerekli olduğu andır (teorinin kendi ifadesiyle:
*"Kayıp, 'ilk `k`' alındığı anda doğar"*).

Kayıp, `Retrieve`'in döndürdüğü listede değil, o listeyi kim tüketiyorsa orada doğar — ve kod
o noktayı denetlemez. `Ens.Kernel.Demo/Program.cs:182` zaten `memory.Retrieve(...)` üzerinde
doğrudan `foreach` yapar; bugün kesmiyor olması bir tasarım garantisi değil, bir tesadüftür.

**Bu, SKR-040/D2 sınıfının tekrarıdır** ve kaydedilmelidir: teori metni, kodun sağladığından
**daha güçlü** bir modalite ("zorundadır", "yapısal") kullanıyor. D2'de sorun *isim* driftiydi;
burada **modalite** driftidir — daha ciddisidir, çünkü teori bir invariant ilan ederken kod
yalnızca bir yardımcı metot sunar.

**Ucuz düzeltme yolları (owner seçsin):** (a) `Retrieve`'i internal yapıp tek public retrieval
kapısını `RetrieveTop` bırakmak; (b) `Retrieve`'in dönüş tipini kesilemez/işaretlenmiş bir tipe
(`UntruncatedRetrieval`) sarıp kesmeyi yalnızca tabanı uygulayan bir metotla mümkün kılmak;
(c) iddiayı **küçültmek** — §3'te "yapısal invariant" yerine *"ENS'in sunduğu kesme API'si bu
invariant'ı uygular; çağıran-tarafı kesme ENS'in denetimi dışındadır ve invariant'ı ihlal eder"*
demek. (c) en dürüst ve en ucuzudur; ama o zaman "patolojinin yapısal kapanışı" cümlesi düşmelidir.

## Yenilik incelemesi

**Novelty iddiası yok — ve bu doğru.** v0.4.0'ın çekirdek argümanı (ölçüm gürültüsü ≠ süreç
gürültüsü, ve bunlar farklı yerlere girer) için doc açıkça şunu yazar: *"Ayrım standarttır ve
ENS'in icadı değildir."* Bu doğrudur; saldırı bulamıyorum. Ayrıca v0.3'ün dört bağlamasından
birini (pusula confidence→TTL düzeltmesi) **geri çekmesi**, TempValid'e dayandırdığı gerekçeyi
**kullanımdan düşürmesi** ve bunu "kaynak hakkında değil ENS hakkında bir bulgudur" diye
sınırlaması, örnek alınacak bir dürüstlüktür. Kaynağı yanlış suçlamadan kendi kullanımını geri
çekmek, SKR'lerin nadiren gördüğü bir davranıştır.

**Kaynakların durumu.** ECC, Hermes Curator, adaptive-decay-KG (arXiv:2604.26970), TempValid
(ACL 2024 long.580), Temporal RAG (arXiv:2509.19376) **SKR-040'ta bağımsız olarak
doğrulanmıştı**; metinleri v0.4.0'da değişmediği için bu turda yeniden sorgulanmadı (TempValid'in
*kullanımı* geri çekildi, atfı değil). v0.4.0'ın **yeni** eklediği üç atıf — Kalman (1960,
J. Basic Eng. 82(1)), Åström & Wittenmark (*Adaptive Control*, üstel unutma faktörlü RLS),
Gama, Žliobaitė, Bifet, Pechenizkiy, Bouchachia (2014, "A Survey on Concept Drift Adaptation",
ACM Comput. Surv. 46(4)) — **alanın standart, yaygın bilinen ve gerçek** referanslarıdır;
uydurma yoktur. Konumlamaları da doğrudur: concept drift literatürü eskimenin sürücüsünü
gerçekten **dağılım kaymasına** bağlar, gözlem kalitesine değil.

### N1 (non-blocking, ama argümanın kalbinde) — Kalman, doc'un kurduğu **çarpımsal ayrışmayı** vermez

Doc'un mimarisi bir **çarpımdır**: `Salience = value(c) × decayFactor(τ_π)`, ve iddia edilen erdem
*"argüman kümeleri ayrıktır"*. Doc bunu iki çerçeveye birden dayandırır: **Kalman filtresi** ve
**üstel-unutmalı RLS**. Fakat bu iki çerçeve **farklı mimariler** verir ve doc bunu fark etmemiştir:

**(a) Kalman / süreç-gürültülü durum kestirimi — ayrışma YOKTUR.** Skaler, sabit bir durum, ölçüm
varyansı `R`, birim zamanda süreç gürültüsü `Q` olsun. `t = 0`'da yapılan bir gözlemin `t` anındaki
ağırlığı (bilgi cinsinden) `1/(R + Qt)`'dir. Bu ifade **çarpanlara ayrılamaz**: `1/(R+Qt) ≠ f(R)·g(t)`.
`R` ve `Q` gerçekten *farklı yerlere girer* — ama **aynı toplamın içine** girerler, ayrı iki çarpana
değil. Bu formda iki kaydın göreli ağırlığı zamanla **yakınsar**: `(R₂+Qt)/(R₁+Qt) → 1`. Yani
**iyi-atıflı bir ders yaşlandıkça, zayıf-atıflı dersin ona göre dezavantajı erir.**

**(b) Üstel unutma faktörlü RLS — ayrışma VARDIR.** Burada her gözlem `λ^(t−k)` ile ağırlıklanır ve
bu ağırlık gözlemin kendi gürültüsünden **bağımsızdır**; bilgi katkısı `(1/R)·λ^(t−k)` olur — bu
**tam olarak** doc'un `value × decayFactor` formudur. Göreli ağırlık oranı `R₂/R₁` olarak **sonsuza
dek sabit** kalır.

**Bulgu:** doc'un formu (b)'ye uyar, (a)'ya uymaz. Ama doc retorik ağırlığı **(a)'ya** verir —
"Kalman (1960)" ilk ve en görünür atıftır, §Prior art satırının başlığıdır, ve §3a'nın çekirdek
argümanı onun adıyla anılır. **Atıf, içeriğinin taşımadığı bir işi yapıyor.** Doğru tek atıf
EW-RLS'tir; Kalman'ın adı, ayrışmayı *desteklemediği* hâlde onu *destekliyormuş gibi* duruyor.

**Neden bu sadece bir atıf hijyeni sorunu değil:** iki form, ENS'in **tam da önemsediği vakada**
farklı davranır. B1/D-5'in koruduğu kayıt zayıf-atıflı bir başarısızlık dersidir.
- **(b) EW-RLS / v0.4.0'ın formunda:** o dersin dezavantajı `c` oranında ve **kalıcıdır** — rakip
  ders ne kadar yaşlanırsa yaşlansın, zayıf-atıflı ders **asla yetişemez**. `decayFactor` ikisi için
  de aynı olduğundan oran hiç değişmez.
- **(a) Kalman formunda:** zaman geçtikçe iyi-atıflı dersin üstünlüğü **erir** ve zayıf-atıflı büyük
  ders doğal olarak yüzeye çıkar.

Yani (a), D-5'in patolojisini **taban gerektirmeden**, tek bir fonksiyonel form değişikliğiyle
çözerdi — ve B1'in yarattığı L0 açığını da açmazdı (çünkü `c = 0` ⇒ `R = ∞` ⇒ ağırlık her `t` için
sıfır; L0 kaydı asla terfi edemez). Doc, ihtiyaç duyduğu çözümü **kendi atıf listesinde taşıyor ama
kullanmıyor**; bunun yerine seçtiği (b) formunun yarattığı boşluğu bir yama (taban) ile kapatıyor.

**Bu N1'i neden blocking saymıyorum:** (a) ile (b) arasındaki seçim **ampirik** bir sorudur (eski
bir kaydın göreli ağırlığı yakınsar mı, yoksa oranı sabit mi kalır?) ve ENS'in bunu çözecek verisi
yok. v0.4.0'ın (b)'yi seçmesi savunulabilir. Savunulamaz olan, seçimin **yapıldığının farkında
olunmaması** ve seçimi desteklemeyen bir kaynağın (Kalman) baş gerekçe olarak sunulmasıdır. Talep:
§3a'da bu bir **seçim** olarak adlandırılsın, gerekçesi yazılsın, ve alternatif form (bilgi-toplamsal,
`1/(1/c + Δt/τ_π)` tipi) reddedilen seçenek olarak kaydedilsin.

## Yanlışlanabilirlik

v0.4.0'ın iddiaları üç sınıfa ayrılır ve **yanlışlanabilirlikleri eşit değildir.** Doc bunları
ayırmıyor; ayırmak gerekir.

**Sınıf 1 — Analitik (yanlışlanamaz, ama zaten ampirik iddia değil).** *"`c` ve `Δt` ayrı argüman
kümelerine girer, dolayısıyla çift-sayım yoktur."* Bu, formülün cebirsel bir özelliğidir; gözlemle
çürütülmez, denetlenir — ve **denetledim: doğrudur** (`CapitalValue` yalnızca `c`'yi,
`DecayFactor` yalnızca `λ_π, Δt`'yi görür; kodda da öyle). Doc'un iddiayı *"istatistiksel
ortogonallik değil, argüman-ayrıklığı"* diye **küçültmesi** yerinde ve SKR-001 dersine uygundur.
Bu, v0.4.0'ın en sağlam parçasıdır ve saldırıdan sağ çıkar.

**Sınıf 2 — Ampirik ve gerçekten yanlışlanabilir (iyi).** *"Bir kaydın bağlamsal geçerliliğinin
sönüm hızı, o kaydın attribution confidence'ından bağımsızdır."* Çürütücü gözlem nettir: bir gerçek
memory korpusunda, Purpose-tipi sabit tutularak, düşük-`c` assertion'lar yüksek-`c` olanlardan
**sistematik olarak daha hızlı** geçersizleşiyorsa (yeniden-doğrulama başarısızlık oranıyla ölçülür),
ayrım ampirik olarak yanlıştır. Doc bunun olabileceğini zaten kabul ediyor (*"`|L|`, `c` ve `Δt`
pekâlâ ampirik olarak korele olabilir"*). Bu dürüst ve sınanabilir.

**Sınıf 3 — Ampirik ama şu hâliyle YANLIŞLANAMAZ (kusur).** İki iddia bu tuzağa düşüyor:
- *"survivorship bias, ENS'in ölçeğinde poisoning'den daha sistematik bir hatadır"* (tabanın tüm
  savunması buna dayanıyor) — metrik yok, eşik yok, gözlem yok.
- *"`τ_π` bir uzmana doğrudan sorulabilir"* — kaç uzman, ne uyum, hangi hata payı? Sorulabilirlik
  bir cevap değil, bir umuttur.

Madde X, "doğrulanmamıştır" demeyi değil, **neyin çürüteceğini** söylemeyi ister. İki iddia da
"henüz doğrulanmadı" etiketiyle geçiştirilmiş; çürütücüleri yazılmamıştır. §Sahibine talepler T5/T6
somut çürütücüler öneriyor.

**Kaybedilen bir yanlışlanabilirlik (kayda değer).** v0.3'ün `γ`'sı kötü bir parametreydi ama **bir
şeyi vardı**: pusula'nın üç çapa-noktasına karşı fit edilebilir ve **fit edilemediği gösterilebilirdi**
(SKR-040/D3: γ≈0.72 vs γ≈2.04). Yani v0.3 sönüm modeli, elindeki tek referans veriye karşı
**çürütülebilir** ve fiilen **çürütülmüştü**. v0.4.0 o parametreyi kaldırınca çürütmeyi de kaldırdı:
`τ_π` için ENS'in elinde **hiçbir** çapa-noktası yok, dolayısıyla sönüm modeli şu an
**sınanamaz durumdadır**. Doc bunu "bir açmazı konusuz kılmak onu çözmek değildir" diye kısmen
kabul ediyor — bu formülasyon doğrudur ve takdir edilir. Ama sonucu tam söylemiyor: v0.4.0'ın sönüm
katmanı, v0.3'ünkinden **daha az** sınanabilir hâldedir. E-grade `eng: E1` bu durumda cömerttir;
sönüm katmanı özelinde **E0'a** yakındır (kodlanmış ama ne kalibre edilmiş ne de sınanabilir).

## Varsayım haritası

| # | Varsayım | Nerede | Kırılma koşulu |
|---|---|---|---|
| V1 | `\|Learning\|` bir kayıt için **tanımlıdır ve anlamlıdır**, attribution seviyesinden bağımsız olarak | §3 `RetentionPriority = \|L\|` | ENS-2004 §Laws "L0'da learning yok" der → L0'da varsayım **zaten kırık** (**B1**) |
| V2 | Kesme yalnızca ENS'in kendi `RetrieveTop` kapısından geçer | §3 taban invariant'ı | Çağıran `Retrieve(...).Take(k)` yazar → **bugün kırık** (**B2**) |
| V3 | `\|L\|` aynı Purpose-tipi içinde **karşılaştırılabilir bir ölçektedir** | taban `argmax \|L\|` | Aynı tipte farklı birimler (% vs para vs gün) karışırsa argmax birimi en büyük olanı seçer, dersi en büyük olanı değil |
| V4 | `\|L\|` **manipüle edilemez** | §3, taban | `Expected` ex-ante düşük konursa `\|L\|` şişer (**N2, Goodhart**) |
| V5 | Sönüm hızı Purpose-tipinin bir **özelliğidir**, kaydın değil | §3a `λ_π` | Aynı tip içinde context volatilitesi kayıttan kayda değişiyorsa (ör. kriz dönemi vs normal dönem) tek `τ_π` yanlış |
| V6 | `c` yazma anında sabitlenir ve **zamanla değişmez** | §3a madde 1 | ENS-2003 §3a'nın kendisi *"`c`, Learning tarafından **güncellenebilir**"* der → **doc kendi varsayımıyla çelişir** (aşağıda N5) |
| V7 | Ağırlık ve tazelik **çarpımsal** birleşir | §3a `Salience` | Bilgi-toplamsal (Kalman) form doğruysa yanlış aile (**N1**) |
| V8 | Uzmanlar `τ_π`'yi güvenilir kestirebilir | §3a "elicit edilebilir" | Pilot elicitation'da uzmanlar arası dağılım geniş çıkarsa |
| V9 | Ontoloji `τ_π`'yi taşıyacak | §3a, §Relationships | ENS-4010 bugün taşımıyor; kod global `0.01` varsayılanı kullanıyor (**Q1/b**) |

**V6 hakkında (N5, yeni ve doc'un fark etmediği bir iç çelişki).** v0.4.0'ın Kalman argümanının
**birinci maddesi** şudur: *"`c` ... yazma anında sabitlenir; **geçen zamanla değişmez**."* Bu, `c`'nin
neden bir "hız" değil bir "ölçüm" olduğunun gerekçesidir. Ama aynı belgenin §3a "Yeniden-doğrulama"
paragrafı şunu der: *"`c`, Learning (ENS-2004) tarafından **güncellenebilir**."* Eğer `c` zamanla
güncelleniyorsa, `c` **zamanın bir fonksiyonudur** ve "zamanla değişmez" öncülü düşer — Kalman
argümanının 1. maddesi kendi belgesinde yalanlanır. Savunulabilir uzlaştırma vardır (`c` *takvimle*
kendiliğinden değişmez, yalnızca *yeni kanıt geldiğinde* sıçrar — bu bir ölçüm güncellemesidir,
bir sönüm değil) ama doc bu uzlaştırmayı **yapmıyor** ve iki cümle olduğu gibi çelişiyor. Bir
cümlelik düzeltme yeter; non-blocking.

## Yazarın üç açık sorusuna cevap

### Soru 1 — `τ_π` borcu gerçekten `c` borcundan daha iyi mi?

**Kısa cevap: borç kapanmadı, taşındı — doc bunu doğru söylüyor — ve takas iki eksende zıt
yönlüdür. Net olarak savunulabilir, ama doc'un iddia ettiği gerekçeyle değil.**

Doc'un dürüstlüğü burada örnek alınacak düzeydedir: *"v0.3'ün `c`'si her kayıtta zaten mevcut bir
alandı, `τ_π` ise hiç mevcut olmayan yeni bir ontoloji verisidir... Bu, v0.4.0'ın en zayıf
noktasıdır."* Bu, Madde X'in istediği türden kendi aleyhine bir failure condition'dır ve
saldırıya gerek bırakmaz. Onaylıyorum. Ancak üç ek gözlem var:

**(a) Doc'un söylemediği bir eksende `τ_π` *gerçekten* daha iyidir — ve doc bunu kendi lehine
kullanmıyor.** Kritik soru "hangi parametre daha kolay ölçülür" değil, **"kalibre edilmemişken
sistem nasıl bozulur"** olmalıdır:
- v0.3 kalibre değilken bozulma **patolojiktir**: `λ(c) = λ_base(1−c)^γ` ile `c = 1.0` yazan her
  kayıt `λ = 0` alır, yani **sonsuza dek sönmez** — sönüm yasasının denetimsiz muafiyet kapısı.
- v0.4.0 kalibre değilken bozulma **iyi huyludur**: tek global `τ` ile her kayıt **aynı hızda**
  söner. Bu, savunulabilir bir *null model*'dir (uniform decay), yanlış bir sürücü değil.

Yani takas "kolay ölçülen yanlış parametre" ↔ "zor ölçülen doğru parametre" değil, **"cehalet
altında patolojik bozulma" ↔ "cehalet altında zararsız bozulma"**dır. Bu, v0.4.0 lehine gerçek
ve doc'un kullanmadığı bir argümandır.

**(b) Ama karşılığında bir *iddia* kaybedildi ve doc bunu iki yerde çelişik biçimde raporluyor.**
§Failure der ki: *"ontoloji `τ_π`'leri taşımadığı sürece pratikte tek bir global `τ` kullanılacaktır
ve o hâlde sönüm **Purpose-tipine koşullu olma iddiasını kaybeder** (§3a'nın (a) bağlaması boşa
düşer)."* Doğru. **Fakat aynı belge, aynı bağlamayı hâlâ *teslim edilmiş katkı* olarak sunuyor:**
- §Prior art tablosu, adaptive-decay-KG satırı: *"λ'nın Purpose-tipi başına kalibre edilmesi (§3a)
  — v0.4.0'da bu, sönümün **tek** sürücüsü"*
- §Prior art "Dürüst delta" listesi: *"(a) sönüm generic KG-tipine değil **Purpose-tipi ontoloji
  sınıfına koşulludur**"* — geri çekilmeyen üç bağlamadan biri olarak.
- §Historical "Dürüst delta": *"unutma kaydı silmeden salience sönümü ile"* + §Relationships:
  Enterprise Ontology *"`τ_π`'nin kalibrasyon sınıfı"*.

Kod ise `contextDecayRate = 0.01` **varsayılanıyla** çalışır ve dosya başında açıkça yazar:
*"Tek global bir değer kullanıldığı sürece sönüm 'Purpose-tipine koşullu' DEĞİLDİR."* Yani belge
bir bölümde bağlamayı **teslim edilmiş** sayıyor, başka bölümde **boşa düştüğünü** kabul ediyor.
Bu, B1/B2 ile aynı sınıf bir iç tutarsızlıktır: **düzeltme bir yerde yapılmış, komşusunda
unutulmuş.** Talep: (a) bağlaması §Prior art'ta *"hedeflenen ama henüz teslim edilmemiş"* olarak
işaretlenmeli, ya da §Failure'daki itiraf §Prior art'a çapraz-referansla bağlanmalı.

**(c) `τ_π`'nin "uzmana doğrudan sorulabilir" olması iddiası sınanmamıştır.** Doc `γ`'ya karşı
`τ_π`'nin üstünlüğünü *elicit edilebilirlik* üzerine kurar: *"bu karar sınıfının context'i kaç günde
yarı yarıya bayatlar?"* Bu soru **sorulabilir** — ama sorulabilir olmak, cevabın **güvenilir**
olduğu anlamına gelmez. Yarı-ömür kestirimi, kalibrasyon literatürünün en bilinen zayıf
noktalarından biridir (insanlar üstel süreçleri sistematik olarak yanlış kestirir). Doc,
`γ`'nın kalibre edilemezliğini bir kusur sayarken `τ_π`'nin elicit edilebilirliğini **kanıtsız bir
erdem** sayıyor. En az bir pilot elicitation (aynı Purpose-tipi için ≥3 uzmanın `τ` kestirimi;
dağılım raporlansın) olmadan bu iddia E0'dır, E1 değil.

**Q1 hükmü:** takas savunulabilir ama gerekçe yeniden yazılmalı — üstünlük *elicit edilebilirlikte*
değil, *kalibrasyonsuz bozulmanın zararsızlığındadır*; ve (a) bağlaması teslim edilmiş katkı
listesinden çıkarılmalıdır.

### Soru 2 — Büyütülmüş memory-poisoning riski kabul edilebilir bir takas mı?

**Kısa cevap: hayır — takasın *kendisi* savunulabilir, ama doc'un yaptığı takas hesabı üç yerde
yanlış ve düzeltildiğinde işaret değiştiriyor.**

Doc'un beyanı: *"çift-sayımı, poisoning'e karşı bir kalkanı feda ederek kaldırdık. Bu takas
bilinçlidir ve savunması §3'ün amacına dayanır (survivorship bias, ENS'in ölçeğinde poisoning'den
daha sistematik bir hatadır) — ama ampirik olarak doğrulanmamıştır."*

**(i) Takas asimetrik ve doc yanlış yöne yuvarlıyor — ENS'in *kendi* invariant'ı yüzünden.**
İki hatanın maliyeti **geri döndürülebilirlik** bakımından eşit değildir:
- **Survivorship bias (ders kaybı) GERİ DÖNDÜRÜLEBİLİRDİR.** ENS'in never-delete invariant'ı bunu
  garanti eder: kayıt **silinmez**, yalnızca sırası düşer. Kaçırılan ders hâlâ bellektedir; daha
  geniş bir `k`, bir Curator turu, ya da `weakly-attributed` listesi onu geri getirir.
- **Poisoning (yanlış dersin terfi ettirilmesi) GERİ DÖNDÜRÜLEMEZDİR.** Terfi eden zehirli ders
  yeni bir karara girer, o karar commit edilir (ENS-2001: **değişmez** snapshot), sonucu yeni bir
  memory kaydı üretir. Zehir **yayılır** ve zincirin geçmişi geri alınamaz.

Yani taban, **geri döndürülebilir** bir zarara karşı korunmak için **geri döndürülemez** bir zararı
satın alıyor. Bu, doc'un takas hesabını tersine çevirir — ve argümanın öncülü ENS'in kendi
decay-not-delete ilkesidir, dışarıdan getirilmiş bir sezgi değil.

**(ii) Taban, seçim kuralı olarak mümkün olan en gürültü-duyarlı istatistiktir.** Doc "poisoning'i
yükseltir" der ama *neden* bu kadar yükselttiğini söylemez: taban bir **argmax**'tır. Uç-değer
istatistikleri, gürültüye duyarlılığı **maksimum** olan tahmin edicilerdir; `|L|`'de bir ölçüm
hatası ya da bir şans-sapması varsa, o hatanın sonucu tam olarak **argmax'ı ele geçirmektir**.
Mekanizma, korumak istediği niceliği en kırılgan biçimde seçiyor. Neredeyse bedelsiz ve çok daha
gürbüz alternatifler vardır ve doc bunları değerlendirmemiştir:
- `argmax |L|` **ancak** `attribution_level ≥ L1` olanlar arasında (= B1'in düzeltmesi),
- `argmax |L|` **ancak** en az bir kez `Verify` edilmiş kayıtlar arasında (kod zaten
  `Verifications` izini tutuyor — bedava),
- üst yüzdelik dilimden **en yüksek `c`**'li kayıt (uç-değer yerine dilim).

**(iii) Takasın savunması şu anki hâliyle yanlışlanabilir değildir.** *"survivorship bias, ENS'in
ölçeğinde poisoning'den daha sistematik bir hatadır"* karşılaştırmalı bir **ampirik** iddiadır;
ama doc ne bir metrik, ne bir veri, ne de bir sınama önerir. "Ampirik olarak doğrulanmamıştır ve
yanlış çıkabilir" demek dürüsttür fakat **yeterli değildir** — Madde X, hangi gözlemin çürüteceğini
ister. Şu anki hâliyle bu cümle bir inanç beyanıdır. Talep: yanlışlayıcıyı yaz (aşağıda §Sahibine
talepler T5'te somut bir öneri var).

**Q2 hükmü:** takas *kabul edilebilir bir takas olarak sunulamaz* çünkü (i) asimetri yanlış yönde
hesaplanmış, (ii) seçim kuralı gereksizce kırılgan ve gürbüz alternatifleri değerlendirilmemiş,
(iii) savunması yanlışlanamaz. Bunların üçü de **düzeltilebilirdir** — bu yüzden `refuted` değil.

### Soru 3 — ENS-2004'teki değişiklik gerçekten additive mi?

**Kısa cevap: hayır. BREAKING. Ve bunu kanıtlayan şey ENS'in kendi test dosyasıdır.**

Doc'un savunması: *"Değişiklik **dar ve additive'dir:** yeni bir iddia getirmez, var olan bir **iç
çelişkiyi** giderir."* Yazarın altta yatan (ve gerçekten güçlü) argümanı şudur: eski satır *kendi
içinde çelişkiliydi*, dolayısıyla **belirli bir anlamı yoktu**; anlamı olmayan bir sözleşme
bozulamaz.

Bu argüman **olgusal olarak yanlıştır**, çünkü o satırın belirli bir okuması vardı ve o okuma
**gerçekleştirilmişti**:

1. ENS-2004 v0.3.2 §Implications: *"Memory retention = `|learning_signal| × attribution_confidence`"*.
2. ENS-2003 v0.3.1 §3 bunu aynen uyguladı: `RetentionPriority = |L|·c`.
3. Faz-4 kodu bunu **inşa etti**: `MemoryRecord.RetentionPriority => DecisionCapital.Value(LM, conf)`.
4. Ve bir test bunu **sözleşme olarak sabitledi**. `Ens.Kernel.Tests/CompanyMemoryTests.cs:13`
   bugün şunu yazıyor: *"`RetentionPriority_matches_DecisionCapital_Value_by_design` → **ARTIK
   YANLIŞ bir iddiadır**"*.

Bir davranışın "by design" adıyla test edilmiş, sonra "artık yanlış" diye işaretlenmiş olması,
**breaking change'in tanımıdır**. Çelişkinin parantez içindeki tarafı değil, **metindeki formül
tarafı** hayata geçmişti; v0.3.3 hayata geçmiş olan tarafı ters çevirdi.

**Versiyonlama disiplini tutarsız.** Tamamen aynı semantik değişiklik iki belgede iki farklı
şekilde etiketlenmiş:

| Belge | Değişiklik | Etiket | Sürüm sıçraması |
|---|---|---|---|
| ENS-2003 | `RetentionPriority`: `\|L\|·c` → `\|L\|` | **BREAKING** | 0.3.1 → **0.4.0** |
| ENS-2004 | retention = `\|L\|·c` → retrieval ağırlığı; retention `c`'den bağımsız | *"dar ve additive"* | 0.3.2 → **0.3.3** |

Aynı olgunun iki yarısı; biri major, öteki patch. ENS-2004 **v0.4.0** olmalı ve v0.3.3 notu
"additive" nitelemesini geri çekmelidir.

**Yazarın kendi alt-sorusuna cevap ("§5(iii) kalibrasyon argümanının bir öncülü sessizce değişti mi?").
Hayır — ama rahatlatıcı olan sebep değil, endişe verici olan sebep yüzünden.** §5(iii) *Confidence
kalibrasyonu* ("0.7 dediğim kararların ~%70'i tuttu mu?") **karar Confidence'ını** (ENS-2001 Decision
Object alanı) ölçer; `attribution_confidence` ile **hiçbir ilgisi yoktur**. Yani §5(iii) etkilenmemiştir
çünkü zaten `c`'yi hiç kullanmıyordu. Yazar bu soruyu sorabildiğine göre, **iki farklı "confidence"
tek adla dolaşıyor demektir** — bkz. N3 (homonim bulgusu, §İç tutarlılık).

**Q3'ün asıl tehlikeli sonucu: D-5 düzeltmesi ENS-2004'e kusuru *ihraç etti*.** v0.3.3 §Implications
artık şunu yazıyor: *"Retention önceliği ... = `|learning_signal|`"* — **hiçbir attribution kapısı
olmadan**. Oysa aynı belgenin §Laws bölümü iki ekran aşağıda *"L0'da learning yok"* der. v0.3.2'de
bu çelişki ENS-2004'te **yoktu** (`×c` çarpanı L0'ı sıfırlıyordu); v0.3.3 onu **içeri taşıdı**.
Yani D-5 yanıtı çelişkiyi ENS-2003'te kapatmadı, **ENS-2004'e kopyaladı** ve orada
`|learning_signal|` ile "L0'da learning yok" yan yana durur hâle geldi. B1 artık **iki** yapıtın
ortak kusurudur ve ENS-2004 v0.3.3'ün de düzeltilmesi gerekir.

## En güçlü karşı-argüman (steelman)

> **"Kalman argümanın doğru ama fazla iş yapıyor: D-5'in 2. şıkkını, o şıkkın hiç ileri sürmediği
> bir iddiayı çürüterek reddettin. Sonuç olarak çalışan bir *proxy*'yi, elde olmayan bir *ideal*
> uğruna attın ve açılan boşluğu bir yamayla (taban) kapattın — o yama da yeni bir çelişki
> (B1) doğurdu."**

Açalım. D-5, ikinci seçenek olarak *"retention'ı `c`'den ayrıştır (`RetentionPriority = |L|`),
`c` yalnızca sönümde kalsın"* dedi. Doc bunun **yarısını** aldı ve ikinci yarısını şöyle reddetti:

> *"`c`'yi sönüme koymak, çift-sayımın yönünü tersine çevirmekten başka bir şey değildir: `c` bir
> **ölçüm** özelliğidir, bir **hız** değil."*

Bu red, `c`'nin sönüm hızı **olduğu** iddiasını çürütür. Ama D-5'in 2. şıkkı bunu iddia
etmiyordu — bir mekanizmanın parametresi olmak, o parametreyle **özdeş** olmak değildir. Ayakta
kalan çok daha zayıf ve çok daha savunulabilir bir okuma vardır:

> `c`, `1/τ_π`'nin bir **vekilidir (proxy)**. Vekil, özdeşlik iddia etmez; yalnızca **korelasyon**
> iddia eder. Ve doc bu korelasyonun **varlığını kendi ağzıyla kabul eder**: *"`|L|`, `c` ve `Δt`
> pekâlâ ampirik olarak korele olabilir."* Dahası korelasyonun **mekanizması** da bellidir ve doc'un
> kendi örneğinde yatar: attribution zayıflığının en yaygın nedeni **confounding**'dir; confounding
> ise **gürültülü, hızlı değişen bağlamlarda** yoğunlaşır. Yani düşük `c`, tipik olarak yüksek
> `1/τ_π`'nin gözlenebilir bir izidir.

Bu okuma kabul edilirse tablo tersine döner:

| | v0.3 (c-sürücülü sönüm) | v0.4.0 (τ_π-sürücülü sönüm) |
|---|---|---|
| Sönüm sürücüsü **ontolojik olarak doğru mu?** | Hayır (ölçümü hıza karıştırır) | **Evet** |
| Sürücü **hesaplanabilir mi?** | **Evet** — `c` her kayıtta var | Hayır — `τ_π` hiçbir yerde yok |
| Kalibrasyonsuz davranış | Patolojik (`c=1` ⇒ hiç sönmez) | İyi huylu (uniform) |
| Bugün fiilen çalışan | c-koşullu sönüm | **global sabit `0.01`** |

Son satır belirleyicidir. v0.4.0 "sönüm Purpose-tipine koşulludur" der; kodda çalışan şey **tek bir
global sabittir**. Yani pratikte v0.4.0, sönümü *doğru sürücüye* bağlamadı — **hiçbir sürücüye
bağlamadı**. Kusurlu bir vekil ile hiç sinyal arasındaki seçimde, "kusurlu vekil"in yanlış olduğunu
göstermek onu değiştirmek için yeterli değildir; **yerine konanın daha iyi olduğunu** göstermek
gerekir. Doc bunu göstermemiştir, çünkü yerine konan şey henüz **yoktur**.

**Owner'ın cevaplaması gereken tam soru:** *v0.3'ün `c`-koşullu sönümü, `τ_π` ontolojiye girene
kadar, açıkça "`1/τ_π` için geçici ve kusurlu bir vekil" diye etiketlenerek korunamaz mıydı — ve
çift-sayım yalnızca `RetentionPriority`'den `c`'yi çıkararak (D-5 şık 2'nin alınmayan yarısı)
kaldırılamaz mıydı? Bu yol çift-sayımı da kapatırdı, sönümü hesaplanabilir de tutardı, ve
karşı-survivorship tabanına — dolayısıyla B1'e — hiç ihtiyaç duyulmazdı.*

**Bu itirazın adil sınırı (steelman'i kendime karşı da kuruyorum).** İtiraz iki yerde zayıftır ve
owner bunları kullanabilir:
1. **Vekil, muafiyet kapısını da miras alır.** `λ(1) = 0` özelliği vekil-etiketiyle de kalırdı:
   `c = 1.0` yazan herkes kaydını sönümden muaf tutardı. Bu, Faz-4 adversarial testinin bağımsız
   olarak bulduğu **gerçek** bir açıktır. Ancak bu, `c`'yi sönümden çıkarmayı gerektirmez — form
   değişikliğiyle (ör. `λ = λ_base·(1 − 0.9c)`, taban sönüm garantili) kapanır. Yani bu zayıflık
   vekil fikrini değil, v0.3'ün **belirli fonksiyonel formunu** vurur.
2. **Vekil, ölçülemeyeni ölçülüyormuş gibi gösterir.** "Sönüm Purpose-tipine koşulludur" demek
   yerine "sönüm `c`'ye koşulludur" demek, ontolojik borcu **gizler**; v0.4.0 borcu **görünür**
   kılar (§Failure'ın ilk maddesi). Görünür borç, gizli vekilden yönetişim olarak üstündür.

İkinci nokta gerçekten güçlüdür ve v0.4.0'ı `refuted`'dan kurtaran gerekçelerden biridir: v0.4.0
**daha dürüst ama şu an daha işlevsizdir**. Bu savunulabilir bir tercihtir — yeter ki §Prior art
teslim edilmemiş bir bağlamayı teslim edilmiş gibi listelemesin (Q1/b) ve taban B1'i doğurmasın.

## İç tutarlılık

Blocking çelişkiler (B1: ENS-2004 §Laws L0 kuralı; B2: modalite drifti; B3: additive/breaking
etiketi) yukarıda ayrı bölümlerde. Kalanlar:

### N2 (non-blocking ama ciddi) — `|L| = |Actual − Expected|` ex-ante oyunlanabilir; taban tek-slot ele geçirmeye açık

v0.4.0 `RetentionPriority`'yi **saf** `|Actual − Expected|`'a eşitledi ve tabanı `argmax |L|` yaptı.
Sonuç: **retention önceliği, öngörü hatasında monoton artandır.** Yani sistemde en çok korunan
kayıt, öngörücüsünün **en çok yanıldığı** kayıttır. Bu kendi başına savunulabilir (büyük sürpriz
gerçekten bilgilendiricidir), ama iki sorun doğurur:

**(a) Goodhart / sandbagging.** `Expected`'ı ex-ante kasten düşük (ya da yüksek) koyan bir owner,
`|L|`'i şişirir ve o Purpose-tipinin **tabanını ele geçirir** — kalıcı olarak, çünkü taban `argmax`'tır,
`c`'ye bakmaz ve **sönüme de tabi değildir** (taban seçimi `RetentionPriority` üzerindendir;
`decayFactor` hiç girmez). Yani tek bir şişirilmiş kayıt, o tipin kesilen her retrieval'ında
**süresiz** bir slot işgal eder. Mutlak değer, saldırıyı iki yönlü ve daha kolay kılar: hem
sandbagging (Expected çok düşük) hem felaket-tellallığı (Expected çok yüksek) `|L|`'i büyütür.

**(b) ENS'in savunması var ama bağlı değil.** Üç yapısal fren mevcut ve hiçbiri retention'a
bağlanmamış:
- ENS-2001'in **donmuş commitment snapshot'ı**: `Expected` commit anında dondurulur, geriye dönük
  değiştirilemez. Bu gerçek ve güçlü bir frendir — saldırı **ex-ante** yapılmak zorundadır.
- ENS-2004 §5(iii) **Confidence kalibrasyonu**: kronik yanlış-tahminci, kalibrasyon eğrisinde
  görünür.
- ENS-2004 §5(ii) **seçim rasyonalitesi**.

Ama §5(iii)'ün çıktısı `RetentionPriority`'ye **hiçbir şekilde bağlı değildir**. Yani ENS,
sistematik kötü kalibrasyonu bir yerde *ölçer* (§5(iii)) ve başka bir yerde *ödüllendirir*
(retention + garantili taban). Bu, ENS'in kendi içinde bir teşvik çelişkisidir. ENS-2004 §Failure
"Goodhart" maddesi mevcut ama yalnızca *"Confidence manipülasyonu"*ndan söz eder — **`Expected`
manipülasyonu yoluyla retention ele geçirme** vakası hiçbir yerde yazılı değildir.

**En ucuz kapanış:** tabanı `argmax |L|` yerine *"kalibrasyon geçmişi bozulmamış öngörücülerin
kayıtları arasında `argmax |L|`"* ya da B1'in düzeltmesiyle birleştirip *"`attribution_level ≥ L1`
olanlar arasında `argmax |L|`"* yapmak. İkincisi hem B1'i hem N2'yi aynı satırla kapatır.

### N3 (non-blocking) — `confidence` homonimi artık üç kavramı taşıyor

Yazarın kendi sorusu (*"§5(iii) kalibrasyon argümanının bir öncülü değişti mi?"*) ancak bu homonim
varsa sorulabilir bir sorudur — ve cevabı "hayır"dır çünkü §5(iii) **başka bir confidence**'ı ölçer.
Külliyat'ta en az üç ayrı nicelik "confidence" adıyla dolaşıyor:

| # | Nicelik | Nerede | v0.4.0'daki rolü |
|---|---|---|---|
| 1 | **Decision Confidence** — kararın kendi öngörüsüne güveni | ENS-2001 Decision Object; ENS-2004 §5(iii) kalibrasyon hedefi | **Hiçbiri** |
| 2 | **Attribution confidence `c`** — sonucun karara atfedilebilirliği | ENS-2004 §3 merdiveni; ENS-2003 `value = \|L\|·c` | v0.4.0'ın **tüm argümanı bu** |
| 3 | **Memory-temelli confidence (P6)** | ENS-2003 §3a'nın açılış cümlesi | Belirsiz |

v0.4.0'ın çekirdek argümanı — *"`c` bir ÖLÇÜM özelliğidir"* — yalnızca **(2)** için doğrudur.
(1) bir *öngörü* özelliğidir ve tam tersine zamanla kalibre edilmesi **gereken** bir niceliktir.
Aynı kelime altında bunları taşımak, Madde VI'nın "terminoloji sürüklenmesi" yasağının ve
ENS-4000 §Kapsam'ın alias yasağının ruhuna aykırıdır ve SKR-040'ın
"assertion" homonimi (N3, hâlâ açık) ile aynı sınıftandır — bu ikincisi daha büyüktür çünkü
**yürüyen bir argümanın kalbindedir**. Talep: ENS-4000'de üç terim ayrı girdilerle ayrılsın
(`Decision Confidence` / `Attribution Confidence` / `Memory Confidence`) ve §3a `c`'yi ilk
kullanımda tam adıyla nitelesin.

### N4 (non-blocking) — kalibre edilmemiş parametreler **fail-open** varsayılanlarla gizleniyor

`CompanyMemory.cs` kendi politikasını şöyle beyan eder (satır 252-255): *"**Fail-closed politikası**
(Guard.cs) girdiyi **kapıda reddetmeyi** şart koşar."* Ama üç kalibre-edilmemiş parametre **sessiz
varsayılanlarla** gelir:

| Parametre | Varsayılan | Teori ne diyor |
|---|---|---|
| `contextDecayRate` (`λ_π`) | `0.01` (⇒ `τ ≈ 69.3` gün) | *"kalibre EDİLMEMİŞTİR"*, *"ontoloji bu alanı taşımaz"* |
| `staleThreshold` (`θ`) | `0.5` | *"teoride türetilmiş bir değer **yoktur**"* |
| `minConfidence` (`c_min`) | `0.5` | *"teoride türetilmiş bir değer **yoktur**"* |

Bir parametrenin *kalibre edilmemiş* olması ile *sessizce varsayılanlanması* farklı şeylerdir.
Varsayılan, borcu **çağrı yerinde görünmez** kılar: `memory.Retrieve("fiyat-belirle", asOf)` yazan
biri, farkında olmadan her Purpose-tipi için 69 günlük bir yarı-ömür kabul etmiş olur ve hiçbir
uyarı almaz. Bu, dosyanın kendi ilan ettiği fail-closed duruşuyla çelişir. Talep: `contextDecayRate`
**zorunlu** parametre olsun (varsayılan kaldırılsın) — böylece `τ_π` borcu her çağrı yerinde
derleyici tarafından görünür kılınır. En ucuz ve en dürüst yaptırım budur.

### Temiz bulunanlar (saldırı isabet etmedi)

- **Alias yasağı (Madde VI + ENS-4000 §Kapsam) doğru uygulanmış.** `value(m)`'ye yeni ad
  **verilmemiş**, ENS-3023
  §Model 1 `value(d)` ile özdeşleştirilmiş; kod da hesabı kopyalamak yerine
  `DecisionCapital.Value`'ya **delege ediyor** (`MemoryRecord.CapitalValue`). Bu, alias yasağının
  hem harfine hem ruhuna uyar.
- **ENS-3023 §Model 2 ile tutarlılık gerçekten *geri kazanılmış*.** *"amortisman = salience sönümü
  × value (context değişim hızı)"* ifadesi v0.3'ün confidence-sürücülü sönümüyle çelişiyordu;
  v0.4.0 ile uyumlu hâle geliyor. Doc'un bu iddiası doğrudur.
- **`stale` / `weakly-attributed` iki-sinyal ayrımı tutarlı.** İki bayrak farklı eksenlerde,
  farklı insan eylemi talep ediyor, ikisi de yalnızca sinyal (P7), ve **hiçbiri
  `RetentionPriority`'yi düşürmüyor** — kod bunu doğruluyor (`FindWeaklyAttributed` yalnızca
  filtreliyor). `c`'nin sönümden çıkarılmasıyla kaybolan sinyalin doğru eksene taşınması iyi bir
  hamledir.
- **Never-delete invariant'ı korunuyor.** `Record` yalnızca ekliyor, `AllRecords` salt-okunur
  sarmalanmış, hiçbir `Delete` yolu yok, `Verify` dört kısıtla (gelecek/geçmiş/monotonluk/üyelik)
  korunuyor ve iz bırakıyor. B1/B2 bu invariant'ı **zayıflatmıyor** — aksine, B1'in düzeltmesi
  (attribution kapısı) never-delete sayesinde bedelsizdir: kapıdan geçemeyen kayıt yine saklanır.

## Kod-teori senkronu (SKR-040'ın desenini yeniden tarama)

**Yöntem ve dürüst sınır.** `CompanyMemory.cs` tümüyle okundu; `RetentionPriority`,
`CounterSurvivorshipFloor`, `RetrieveTop`, `Retrieve`, `DecayFactor`, `Salience` çağrı yerleri
depo genelinde tarandı; ilgili test bölümleri okundu. **`dotnet build` / `dotnet test`
ÇALIŞTIRILAMADI — bu incelemenin context'inde shell/Bash aracı yoktur.** Hiçbir test çıktısı
iddia edilmiyor, hiçbir sayı uydurulmuyor (SKR-041 emsali). Aşağıdakiler **statik** bulgulardır.

**Formül düzeyinde senkron: TEMİZ ✓** — v0.4.0'ın üç niceliği kodda birebir karşılanıyor:

| Teori (§3/§3a) | Kod | Durum |
|---|---|---|
| `RetentionPriority = \|Learning\|` | `RetentionPriority => LearningMagnitude` | ✓ |
| `value = \|L\|·c` (= ENS-3023 `value(d)`) | `CapitalValue => DecisionCapital.Value(LM, conf)` (delege) | ✓ |
| `decayFactor = exp(−λ_π·Δt)`, `c` içermez | `DecayFactor(...)` — `AttributionConfidence` hiç geçmez | ✓ |
| `Salience = value × decayFactor` | `Salience => record.CapitalValue * DecayFactor(...)` | ✓ |
| stale-yargısı **saf** `decayFactor`'a bakar | `FindStale` doğrudan `DecayFactor` çağırıyor; eski `Salience/RetentionPriority` bölmesi kaldırılmış | ✓ |
| `γ`, `λ_base` **kaldırıldı** | `DecayFunction`'da yok; yerine `RateFromHalfLife`/`HalfLifeDays`/`DaysUntilStale` | ✓ |
| `t_stale = τ_π·log₂(1/θ)` | `DaysUntilStale = ln(1/θ)/λ_π` — cebirsel olarak aynı (`ln(1/θ)/λ = (ln2/λ)·log₂(1/θ) = τ·log₂(1/θ)`) ✓ elle doğrulandı | ✓ |

Yani D-5'in *formül* düzeltmesi kodda **gerçekten** yapılmış; `c`'nin sönümden çıkarılması eksiksiz.
v0.3'ün iki adversarial açığının (`c=1.0` sönüm muafiyeti, büyük `γ` underflow) **yapısal olarak**
kapandığı iddiası da doğrudur: her iki anahtar da artık kodda mevcut değil.

**Modalite düzeyinde senkron: KIRIK ✗ (B2).** Teori "zorundadır" / "yapısal invariant" der; kod
opsiyonel bir metot sunar ve `Retrieve` üzerinden kesme serbesttir. Ayrıntı yukarıda B2'de.

**Anlam düzeyinde senkron: KIRIK ✗ (B1).** Kod ENS-2003'e sadıktır, ama ENS-2003+kod ikilisi
**ENS-2004'e** aykırıdır (L0). Bu, SKR-040/D2'nin aynası: orada kod teoriye sadıktı ve **teori**
yanlıştı; burada kod ENS-2003'e sadık, ama **ENS-2003 ile ENS-2004** birbirine aykırı. Kusur yine
kodda değil, teori katmanındadır — ve yine **iki tur arayla** aynı desen: bir yerde kapatılan
çelişki komşusunda açılıyor.

**Test yüzeyi hakkında iki gözlem (statik):**
1. `AdversarialAuditTests.cs:960-973` B1'i **beklenen davranış olarak sabitliyor** (yukarıda tam
   alıntı). Bu, B1 düzeltildiğinde **değişmesi gereken** bir testtir; owner düzeltirken bunu
   gözden kaçırmamalıdır.
2. `CompanyMemoryTests.cs:13` eski sözleşme testinin (`RetentionPriority_matches_DecisionCapital_
   Value_by_design`) *"ARTIK YANLIŞ bir iddiadır"* diye işaretlendiğini gösteriyor — B3'ün
   (breaking) doğrudan kanıtı.
3. **Aradığım ve bulamadığım test:** `Retrieve(...).Take(k)`'nin tabanı baypas ettiğini gösteren
   bir adversarial test **yok**. B2 tam olarak bu boşlukta yaşıyor. Owner B2'yi kapatırken bu
   testi de eklemelidir (kırmızıdan yeşile).

**§Failure conditions'ın "✅ KAPANDI (373/373 geçti)" maddesi hakkında.** Doc, owner'ın ayrı bir
koşuda `dotnet test` → 373/373 aldığını kaydediyor. **Bunu doğrulayamadım** (shell yok) ve
sorgulamıyorum — kaydın kendisi usulüne uygun (ayrı koşu, fabrikasyon yok beyanı). Yalnızca şunu
not düşüyorum: **testlerin yeşil olması B1/B2'yi çürütmez**, çünkü B1 testlerde *beklenen davranış
olarak kodlanmıştır* ve B2 için test *hiç yoktur*. Yeşil bir süit, yanlış bir sözleşmeyi doğrular.

## Sahibine talepler

### Blocking (bunlar kapanmadan `ratified` olmaz)

**T1 — B1: retention'a bir attribution *kapısı* koy (çarpan değil).**
ENS-2003 §3 ile ENS-2004 §Laws arasındaki L0 çelişkisini kapat. Önerilen biçim:

> `RetentionPriority(m) = |Learning(m)|` **ancak** `attribution_level(m) ≥ L1` ise; `L0`
> kayıtları saklanır (never-delete) fakat **karşı-survivorship tabanı yarışına giremez.**

Bu bir *gate*'tir, *multiplier* değildir: `|L|`'i `c` ile ölçeklemez, `c`'yi sönüme sokmaz,
çift-sayımı geri getirmez ve v0.4.0'ın Kalman argümanına dokunmaz — aksine ondan **türetilir**
(ölçüm gürültüsü sonsuz ⇒ kazanç sıfır ⇒ o gözlem kestirimi güncellemez). Kodda karşılığı:
`CounterSurvivorshipFloor`'a bir attribution eşiği; `MemoryRecord`'a `AttributionLevel`
(L0..L3) alanı — şu an merdiven **kodda hiç temsil edilmiyor**, yalnızca `c` var; ENS-2004 §3
merdiveni Faz-4'te karşılıksız. `AdversarialAuditTests.cs:960-973` testi buna göre güncellenmeli.
**Aynı düzeltme ENS-2004 v0.3.3 §Implications'ta da yapılmalı** (orada da `= |learning_signal|`
kapısız duruyor — B3/Q3).

**T2 — B2: "yapısal invariant" iddiasını ya kodda karşıla ya metinde küçült.**
Ya (a) kesmeyi yalnızca tabanı uygulayan API'den mümkün kıl (`Retrieve`'i internal yap / kesilemez
dönüş tipi), ya (b) §3'ün *"yapısal kapanış"* ve *"zorundadır"* dilini geri çekip *"ENS'in kesme
API'si bu invariant'ı uygular; çağıran-tarafı kesme onu ihlal eder ve ENS'in denetimi dışındadır"*
de. (b) ucuzdur ve dürüsttür — ama o zaman "patolojinin **yapısal** kapanışı" cümlesi düşmelidir.
Her iki hâlde `Retrieve(...).Take(k)` baypasını gösteren bir test eklensin.

**T3 — B3: ENS-2004 v0.3.3'ün "additive" nitelemesini geri çek; sürümü `0.4.0` yap.**
Gerekçe metne yazılsın: `RetentionPriority = |L|·c` sözleşmesi **gerçekleşmişti** (ENS-2003 v0.3.1,
Faz-4 kodu, ve `RetentionPriority_matches_DecisionCapital_Value_by_design` testi) ve v0.3.3 onu
ters çevirdi. Aynı semantik değişiklik ENS-2003'te BREAKING/major sayılırken ENS-2004'te
additive/patch sayılamaz.

**T4 — Q1/b: teslim edilmemiş bağlamayı teslim edilmiş gibi listeleme.**
§Prior art'ın *"(a) sönüm ... Purpose-tipi ontoloji sınıfına koşulludur"* bağlaması ve
adaptive-decay-KG satırının *"v0.4.0'da bu, sönümün tek sürücüsü"* ifadesi, §Failure'ın kendi
itirafıyla (*"tek global `τ` kullanılacaktır ... bağlaması boşa düşer"*) ve kodun global `0.01`
varsayılanıyla çelişiyor. Bağlama **"hedeflenen, henüz teslim edilmemiş"** olarak işaretlensin.

### Non-blocking (ratified'ı bloke etmez, ama sıradaki dokunuşta)

**T5 — Q2/(iii): tabanın savunmasına bir yanlışlayıcı yaz.** *"Survivorship bias poisoning'den daha
sistematik"* iddiası şu an sınanamaz. Somut öneri: taban devredeyken ve devre dışıyken, aynı Purpose
-tipinde, retrieval'a giren kayıtların sonradan (Curator turunda ya da L2 çalışmasıyla) **geçersiz
çıkma oranı** ölçülsün. Taban açıkken bu oran anlamlı biçimde yükseliyorsa, takas yanlıştır. Ayrıca
§Failure'a Q2/(i)'nin asimetri argümanı eklensin: **kaçırılan ders geri döndürülebilirdir
(never-delete), terfi etmiş zehirli ders değildir** — bu, doc'un mevcut takas gerekçesini zayıflatan
ve bilinmesi gereken bir karşı-argümandır.

**T6 — Q1/c: `τ_π` elicit edilebilirliği sınansın.** En az bir Purpose-tipi için ≥3 uzmandan bağımsız
`τ` kestirimi alınsın, dağılım raporlansın. Uyum zayıfsa "uzmana doğrudan sorulabilir" iddiası
§Failure'a taşınsın. Bu yapılmadan sönüm katmanının `eng: E1` derecesi cömerttir.

**T7 — N1: Kalman ↔ EW-RLS seçimini adlandır.** §3a, çarpımsal ayrışmanın **EW-RLS**'ten geldiğini,
Kalman'ın süreç-gürültülü formunun ise ayrışma **vermediğini** (`1/(R+Qt)` çarpanlara ayrılmaz)
yazsın; seçim gerekçelendirilsin ve bilgi-toplamsal alternatif reddedilen seçenek olarak kaydedilsin.
Kalman atfı kalabilir — ama "ayrım standarttır" derken *hangi* ayrımın hangi mimariyi verdiği
belirtilmelidir.

**T8 — N2: `|L|`'in oyunlanabilirliğini §Failure'a yaz** (`Expected` ex-ante manipülasyonu → taban
ele geçirme; mutlak değer saldırıyı iki yönlü kılar) ve ENS-2004 §Failure'ın Goodhart maddesini
"Confidence manipülasyonu"ndan `Expected` manipülasyonuna genişlet. T1'in attribution kapısı bunu
büyük ölçüde kapatır — ilişki not düşülsün.

**T9 — N3: `confidence` homonimini ENS-4000'de ayır** (Decision Confidence / Attribution Confidence /
Memory Confidence). v0.4.0'ın *"`c` bir ölçüm özelliğidir"* argümanı yalnızca ikincisi için geçerlidir;
§3a ilk kullanımda tam adı kullansın. (SKR-040/N3 "assertion" homonimi hâlâ açık — birlikte kapatılsın.)

**T10 — N4: `contextDecayRate` varsayılanını kaldır** (zorunlu parametre yap). Kalibre edilmemiş bir
parametrenin sessiz varsayılanı, dosyanın kendi ilan ettiği fail-closed politikasıyla çelişir ve
`τ_π` borcunu çağrı yerlerinde görünmez kılar.

**T11 — N5/V6: `c` "zamanla değişmez" ile "Learning tarafından güncellenebilir" cümlelerini uzlaştır**
(bir cümle: `c` takvimle kendiliğinden sönmez; yalnızca yeni kanıtla sıçrar — bu bir ölçüm
güncellemesidir, sönüm değil).

**T12 — §Failure'a nicel dürüstlük:** tabanın maliyeti *"bir slot"* değil *"`1/k`"*'dir; `k=1`'de
sonuç kümesinin **tamamıdır** ve sıralamayı bozmakla kalmaz, iptal eder.

---

## Kapanış

D-5 gerçek bir hataydı ve v0.4.0 onu gerçekten kapattı: `c` artık sönüme hiç girmiyor, ne teoride
ne kodda; ölçüm/süreç ayrımı doğru, argüman-ayrıklığı iddiası **küçültülmüş hâliyle** doğru ve
denetlendi; iki adversarial açık (`c=1.0` muafiyeti, `γ` underflow) yapısal olarak kapandı; geri
çekilen pusula bağlaması ve TempValid kullanımı örnek bir dürüstlükle raporlanmış. Çekirdek tez
saldırıdan sağ çıkıyor.

Yara, düzeltmenin kendisinde değil **düzeltmeyi tamamlamak için eklenen yamadadır.** `c`'yi
retention'dan çıkarmak, farkında olmadan ENS-2004'ün L0 kuralının Külliyat'taki **tek operasyonel
dişini** söktü; aynı sürümde `c`'nin alt tabanı da (`[0.3,1]` → `[0,1]`) kaldırıldı; ve eklenen
karşı-survivorship tabanı, `c`'yi görmeyi **açıkça yasaklayarak** L0 kaydına yapısal ayrıcalık
verdi. ENS'in kendi adversarial testi bunu `c = 0.0` ile, `limit: 1` retrieval'ının **tek** sonucu
olarak sabitliyor — ve aynı testin yorumu beş satır yukarıda "atfı olmayan bir ders yeni kararı
yönlendiremez" diyor. Bu bir teori-kod desenkronu değil, **iki Külliyat adayı arasında bir yasa
çelişkisidir** ve düzeltilene kadar `ratified` gelemez.

Desen kaydedilmeye değer: SKR-040 bir çelişkiyi kapattı, SKR-041 kapanışı doğruladı, AUDIT-WAVE2
ikisinin de kaçırdığı komşu çelişkiyi buldu, ve v0.4.0 onu kapatırken **bir sonraki komşuya**
taşıdı — bu kez ENS-2003'ten ENS-2004'e. Dördüncü tekrar, artık tekil bir dikkatsizlik değil
**yapısal bir şeydir**: ENS'in yapıtları tek tek gözden geçiriliyor, ama bir yapıttaki formül
değişikliğinin **bağımlı yapıtların yasa cümlelerine** ne yaptığı sistematik olarak taranmıyor.
Owner'a asıl önerim T1-T4'ün ötesinde budur: bir formül değiştiğinde, `depends_on` ve
`referenced_by` zincirindeki **yasa/invariant cümleleri** için zorunlu bir tarama adımı tanımlansın.

*Bir dersin ne kadar güvenle konuştuğu ile kaybolup kaybolmayacağının ayrı sorular olduğu doğrudur.
Ama "hiç konuşmayan" bir kayıt için üçüncü bir soru vardır ve v0.4.0 onu sormuyor: ortada bir ders
var mı?*
