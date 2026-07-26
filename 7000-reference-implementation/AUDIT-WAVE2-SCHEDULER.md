# AUDIT — Dalga 2: `Scheduler` + `BoundedAutonomyGate` + `Guard`

| | |
|---|---|
| **Denetleyen** | `ens-skeptic` — `Guard.cs`'i YAZAN context'ten ayrı, taze bir context |
| **Denetlenen** | `Ens.Kernel/Scheduler.cs`, `Ens.Kernel/BoundedAutonomyGate.cs`, `Ens.Kernel/Guard.cs` (HEAD, 2026-07-26) |
| **Görev** | AUDIT.md §5.1'in NaN fail-open düzeltmesinin gerçekten kapattığını **doğrulamak** ve **yeni kaçak aramak** |
| **Yetki** | Anayasa Madde X (Yanlışlanabilirlik Ödevi) |
| **Ürettiğim kanıt** | `Ens.Kernel.Tests/AdversarialWave_SchedulerGateTests.cs` (24 test) |

---

## 0. ÖNCE DÜRÜSTLÜK: bu denetimin sınırı

**`dotnet test` bu turda DA çalıştırılamadı.** Bu context'e verilen araç seti `Read`, `Grep`,
`Glob`, `Write`, `Edit`, `WebSearch`, `WebFetch`'tir; **`Bash` aracı etkin değil**
(`Error: No such tool available: Bash` — çağırdım, reddedildi). MCP `computer-use` üzerinden
terminale yazmak da mümkün değil: terminaller "click" kademesinde, `type` bloklu.

Sonuç olarak:

- Bu raporda **tek bir test çıktısı satırı fabrike EDİLMEDİ.** 199 mevcut testin kırılmadığını
  **iddia etmiyorum** — doğrulayamadım. `AdversarialWave_SchedulerGateTests.cs` mevcut hiçbir
  dosyayı değiştirmediği ve yalnızca yeni bir sınıf eklediği için mevcut testleri kırması
  beklenmez, ama bu bir **çıkarım**, ölçüm değil.
- 24 yeni test **derlenmedi/koşturulmadı**. Derleme hatası ihtimali sıfır değildir.
- Bütün bulgular **statik analiz + elle IEEE-754 hesabı**dır; her birinin yanında güven derecesi
  var ve her biri "hangi test kırılırsa bu bulgu çürür" biçiminde yazıldı.

Bu, SKR-041 emsalinin (bir satır bile uydurma) ikinci kez tekrarlanmasıdır ve **bu denetimin en
büyük zaafıdır.** Kapanış şartı §7'de.

---

## 1. ANA SORUYA CEVAP: NaN düzeltmesi gerçekten kapattı mı?

### Kısmen. **Kök neden kapatıldığı İDDİA EDİLEN yerlerde kapandı; iddianın kapsadığı yer listesi EKSİK.**

`Guard.cs` dosya başı, kök nedenin kapatıldığı **yedi** noktayı sayıyor:
DecisionGravity, BoundedAutonomyGate, ProofTrace, DecisionCapital, DecisionAggregate,
CompanyMemory, ContextScore.

Bu yedisini tek tek grepledim — **hepsi gerçekten `Guard`'dan geçiyor.** Ve `Guard`'ın kendisi
sızdırmıyor: değer döndüren beş metodunun (`Finite`, `NonNegativeFinite`, `PositiveFinite`,
`UnitInterval`, `OptionalUnitInterval`, `NormalizedDeficit`) hepsi `double.IsFinite` üzerinden
başlıyor, yani NaN'a kapalı. `IsMeasurable*` çiftleri exception atmaz ama NaN'da `false` döner.
Bunu tek tek elle değil, **reflection ile mekanik olarak** taradım ki yarın eklenecek bir Guard
metodu otomatik kapsam altına girsin (`AUDIT_HOLDS_W1`).

**Ama listede olmayan bir sekizinci nokta var ve o hâlâ fail-open:** `LlmTierSelector`.

---

## 2. EN KRİTİK YENİ BULGU — `LlmTierSelector` Guard'a hiç bağlanmadı (üçlü fail-open'ın ikinci ayağı açık)

AUDIT.md §5.1 "üçlü fail-open" tarif ediyordu: (1) gate Autonomous, (2) tier Operational —
en ucuz/zayıf model, (3) sıralamada en son. Düzeltme **(1) ve (3)'ü** kapattı. **(2) açık kaldı.**

`LlmTierSelector.SelectTier`, `Guard`'ı **hiç çağırmıyor** ve aynı kök nedeni birebir tekrar
ediyor:

```csharp
if (infoNeed < 0) throw ...;                                   // NaN'ı GÖRMEZ
if (criticalThreshold < complexThreshold) throw ...;           // NaN < NaN -> false, GÖRMEZ
if (infoNeed >= criticalThreshold) return Critical;            // NaN >= NaN -> false
if (infoNeed >= complexThreshold)  return Complex;             // false
return LlmTier.Operational;                                    // <-- en permisif dal
```

Ve `Scheduler` bu eşikleri **doğrudan** geçiriyor (`Schedule(..., complexThreshold,
criticalThreshold)` → `SelectTier(infoNeed, complexThreshold, criticalThreshold)`). Yani:

```csharp
Scheduler.Schedule(pending, 5_000, 50_000,
    complexThreshold: double.NaN, criticalThreshold: double.NaN);
// -> 1 milyar stake'lik, sıfır-confidence'lı karar dahil HER karar LlmTier.Operational
// -> hiçbir exception yok, hiçbir uyarı yok
```

**Neden bu en kritik:** saldırgan gerektirmez. Tek bir kalibrasyon-borcu eşiği yanlış hesaplanıp
NaN olursa (ör. `0.0/0.0` üreten bir kalibrasyon fonksiyonu — ENS-3022 §Model 1 normalizasyonu
hâlâ açık borç), tüm hesaplama-kaynağı tahsisi sessizce en zayıf modele çöker ve ADR-0001
§5.3'ün "InfoNeed → model gücü" iddiası fiilen devre dışı kalır. Gate ayağı sağlam kaldığı için
P7 ihlali değil, ama **P5 (attention/kaynak tahsisi) ihlali**dir.

Aynı zamanda `Guard.cs`'in kendi kapanış iddiasını doğrudan yanlışlar: kök neden 7 yerde değil,
**en az 8 yerdeydi**; sayım yapılırken `LlmTierSelector` atlandı.

> **Kanıt testi:** `AUDIT_DEFECT_W11_NaN_tier_thresholds_silently_route_every_decision_to_the_cheapest_model`
> **Güven:** çok yüksek (IEEE-754 + kaynak okuma).
> **Çürütme:** test yeşil yanmazsa (yani `Assert.Equal(LlmTier.Operational, ...)` başarısız
> olursa) bu bulgu tamamen düşer.
> **Talep:** `SelectTier`'ın üç `double` girdisi de `Guard.Finite`/`NonNegativeFinite`'ten
> geçmeli ve `Guard.cs`'in "yedi nokta" listesi sekize çıkmalı.

---

## 3. SCHEDULER ↔ GATE ARASINDAKİ ÇATLAK (iki bulgu, birlikte okunmalı)

### 3.1 Gate'in en özenli fail-closed dalı, Scheduler yolundan ULAŞILAMAZ

`BoundedAutonomyGate.cs`'in `(0) FAIL-CLOSED ÖNCELİĞİ` bloğu şunu vaat ediyor:

> *"Geri-dönülemez bir action, tüm girdileri ölçülemez (NaN) olsa bile bloklanmak ZORUNDA —
> 'ölçemedim, o yüzden değerlendiremiyorum' bir izin değildir."*

Gate **tek başına** bu sözü tutuyor: `Evaluate(NaN, null, 0, isIrreversible: true, ...)` →
`CriticalBlock`. Doğrulandı.

**Ama gate'in kernel içindeki tek gerçek tüketicisi Scheduler'dır ve Scheduler gate'i çağırmadan
ÖNCE `DecisionGravity.InfoNeed`'i çağırıyor** (`Scheduler.cs:99`), orada `Guard` patlıyor:

```csharp
double infoNeed = DecisionGravity.InfoNeed(d.Stake, d.Confidence);   // <-- satır 99, ÖNCE burada patlar
...
var gate = BoundedAutonomyGate.Evaluate(...);                        // <-- satır 102, hiç ulaşılmaz
```

Yani gate'in içine yazılmış o dikkatli açıklama, üretim yolunda **ölü kod**tur.

Bu "fail-open" değildir — karar icra edilmez. Ama üç şey kaybolur:

1. **Denetlenebilirlik.** `CriticalBlock` bir *karar*dır: `Reason` taşır, `GateResult` olarak
   kaydedilebilir, proof-trace'e girer. `ArgumentOutOfRangeException` bir karar değildir.
2. **Semantik.** `catch (Exception e) { log(e); continue; }` yazan bir çağıran için gate
   **hiç çalışmamış** olur. Bir exception'ın fail-closed olması, çağıranın onu yutmamasına
   bağlıdır; bir `CriticalBlock` nesnesinin fail-closed olması ise değildir.
3. **Kimlik.** Exception, hangi kararın suçlu olduğunu **taşımıyor** (`ParamName = "stake"`,
   `DecisionId` yok).

> **Kanıt testi:** `AUDIT_DEFECT_W8_scheduler_preempts_the_gates_own_fail_closed_CriticalBlock_branch`

### 3.2 Tek bozuk karar, TÜM partinin dikkat tahsisini düşürüyor

1000 karardan biri `stake = NaN` ise, `Schedule` fırlatır ve **hiçbiri** tahsis almaz. Kısmî
sonuç, karantina listesi, "bu kaydı atla ve raporla" yolu **yok**.

Bunun asıl ağırlığı şurada: **`Guard.cs` bu vektörü kendi yorumunda TANIYOR ve `ConformanceDeficit`
için tam da bu yüzden exception yerine KIRPMA seçmiş:**

> *"Tüm partiyi bir tek bozuk peer-sinyali yüzünden exception'la düşürmek, dikkat tahsisini komple
> durdurur (servis-dışı bırakma vektörü); kırpma fail-closed kalır."*

Aynı gerekçe `stake` ve `confidence` için **uygulanmamış**. Politika kendi içinde tutarsız:
aynı dosya, aynı riski bir alan için kabul edilemez, diğer iki alan için kabul edilebilir sayıyor.
Bu, denetimin "guard mesajını oku, kodun onu zorladığını doğrula" (saldırı yüzeyi §5) desenidir —
burada guard mesajı doğru, ama **kendi ilkesini yalnızca bir alanda uyguluyor**.

Not: `stake`/`confidence` için kırpma **doğru çözüm değildir** (NaN stake'i 0'a kırpmak sahte bir
sayı uydurmaktır, Madde X). Doğru çözüm, per-decision karantinadır: ölçülemeyen karar
`CriticalBlock` + `Rejected` etiketiyle sonuçta **yer alır**, parti düşmez.

> **Kanıt testi:** `AUDIT_DEFECT_W9_one_poisoned_decision_denies_attention_to_the_whole_batch`

### 3.3 Politika doğrulaması VERİYE bağlı — deterministik değil

`blockThreshold >= autonomyThreshold` ve eşiklerin sonluluğu, gate'in **içinde**, kısıtlayıcı
erken-dönüşlerden **sonra** kontrol ediliyor. Sonuç: bozuk bir politikanın yakalanıp
yakalanmaması, partideki kararların içeriğine bağlı.

| Girdi | Politika (`autonomy=NaN, block=NaN`) | Sonuç |
|---|---|---|
| geri-dönülemez karar | doğrulanmıyor (erken `CriticalBlock`) | **sessizce kabul** |
| yetkisiz araçlı karar | doğrulanmıyor (erken `Blocked`) | **sessizce kabul** |
| sıradan karar | doğrulanıyor | `ArgumentOutOfRangeException` |

Yani aynı bozuk politika, partinin ilk elemanı geri-dönülemezse geçiyor, değilse patlıyor.
Politika doğrulaması **karar-başına bir yan etki** olarak yapılıyor; oysa politika, karardan
bağımsız bir nesnedir ve **kuruluş anında** doğrulanmalıdır (bir `GatePolicy` tipi).

> **Kanıt testi:** `AUDIT_DEFECT_W10_broken_policy_thresholds_are_only_validated_on_some_inputs`

---

## 4. `ToolAuthorization` BAĞI (AUDIT §5.5'in "kapanışı") — bağ kırılabilir mi? EVET, iki yoldan

### 4.1 Yetkisiz araç GERÇEKTEN bloklanıyor — bu sağlam

Önce dürüstlük: sorulan soru "yetkisiz araç gerçekten bloklanıyor mu, yoksa `IsAllowed=false`
sessizce yutuluyor mu?" idi. **Yutulmuyor.** Üç ayrı yoldan doğruladım:

- `IsAllowed: false` → `Blocked`, InfoNeed ne kadar küçük olursa olsun (`AUDIT_HOLDS_W12`).
- Bu, Scheduler üzerinden uçtan uca da geçerli.
- `RequiresHumanApproval: true` → otonomi kaldırılıyor; ve F3 düzeltmesi (katı Pack `Disable`
  edilse bile kısıt korunur) **gate çıkışında** da tutuyor (`AUDIT_HOLDS_W13`).
- Bağ yalnızca **sıkılaştırıyor**: izinli bir araç, yüksek InfoNeed'li bir kararı ya da
  geri-dönülemez bir action'ı gevşetemiyor (`AUDIT_HOLDS_W14`).

Bu, ADR-0001 §6.1(2)'nin "deklaratif izinler doğrudan Gate'e beslenir" iddiasının gerçek ve
çalışan bir karşılığıdır. AUDIT §5.5'in ana şikâyeti **giderilmiş**.

### 4.2 Ama yeni bir güven sınırı açıldı ve MÜHÜRSÜZ

`ToolAuthorization` public bir `record`. Bir registry **reddi**, tek satırda, reflection
gerekmeden "izin"e çevriliyor:

```csharp
var denied = registry.Authorize("delete_database");   // IsAllowed = false
var laundered = denied with { IsAllowed = true };     // <-- tek satır
BoundedAutonomyGate.Evaluate(10, 0.99, 0, false, 5_000, 50_000, laundered).Decision;
// -> Autonomous
```

Aynı şekilde gerçek bir "insan onayı ŞART" kısıtı `with { RequiresHumanApproval = false }` ile
soyuluyor. Bu, AUDIT §4.1'in `GateResult` forge bulgusuyla **aynı sınıf** bir kusurdur.

**Asıl mesele şu:** `BoundedAutonomyGate.cs`'in "DÜRÜST SINIRLAR" bloğu (a) maddesinde
`GateResult`'ın forge edilebilirliğini **açıkça kabul ediyor** — ama §5.5 düzeltmesiyle gelen
**yeni** güven sınırı `ToolAuthorization` o listede **yok**. Dürüstlük belgesi, düzeltmeyle
birlikte güncellenmemiş. Kod, kendi dürüst-sınırlar bloğundan daha az dürüst hâle gelmiş —
ki AUDIT §4.2'de bunun **tersi** (kaynak dosya demodan dürüst) övülmüştü. Bu bir gerileme.

> **Kanıt testi:** `AUDIT_DEFECT_W15_ToolAuthorization_is_a_public_record_so_a_registry_denial_can_be_laundered`

### 4.3 `null` geçilince ne oluyor? — en permisif okuma, ve bunu ayırt eden bir mekanizma yok

`null` = "araç-bağımsız karar" demek ve gate'i **hiç etkilemiyor** (doğru okuma). Ama aynı `null`,
"bu karar bir araç çağırıyor ama yetkilendirmeyi geçirmeyi unuttum" demek de olabilir. Gate ikisini
**ayırt edemez** ve ikisinde de en permisif dala düşer.

Bunu yapısal olarak kanıtladım:

- `PendingDecision`'da **araç ADI taşıyan bir alan yok** — dolayısıyla Scheduler yetkiyi kendisi
  çözemez.
- `Scheduler.Schedule` imzasında `CapabilityRegistry` **yok**.
- `PendingDecision.ToolAuthorization` **varsayılan değeri `null`** olan opsiyonel bir parametre.

Yani "registry → gate" bağı kernel içinde **zorlanmıyor**; doğru `ToolAuthorization`'ı iliştirmek
tamamen çağıranın disiplinine bırakılmış. ADR-0001 §6.1(2)'nin iddiası artık *mümkün*, ama hâlâ
*zorunlu* değil. Bir seviye ilerleme, ama "yapısal olarak imkânsız" dilini hâlâ hak etmiyor.

**Doğru kapanış:** `PendingDecision`'a `ToolName` alanı + `Schedule`'a opsiyonel
`CapabilityRegistry` eklenmesi; araç adı verilmiş ama registry verilmemişse **hata**; araç adı
verilmişse yetki **Scheduler tarafından çözülür**, çağırandan alınmaz.

> **Kanıt testi:** `AUDIT_DEFECT_W16_null_toolAuthorization_is_indistinguishable_from_forgetting_to_authorize`

---

## 5. GUARD SINIRINDAN NaN KAÇIYOR (ama karar değil, değer)

`Guard`'ın vaadi: *"ölçülemeyen girdi kernel'in karar yollarına giremez"*. Gate'in iki erken-dönüş
dalı bunu tersinden deliyor: ölçülemeyen bir **değer**, `GateResult.InfoNeed` içinde **dışarı
çıkıyor**:

| Girdi | Karar | `GateResult.InfoNeed` |
|---|---|---|
| `stake = NaN`, irreversible | `CriticalBlock` | **NaN** |
| `stake = +∞`, `confidence = 5.0`, yetkisiz araç | `Blocked` | **NaN** |
| `stake = -5.0` (sonlu ama tanım dışı), irreversible | `CriticalBlock` | **NaN** |

Kararın kendisi fail-closed, bu yüzden **güvenlik açığı değil**. Ve kodun gerekçesi savunulabilir
(*"sahte bir sayı UYDURULMAZ (Madde X)"* — katılıyorum, 0 yazmak daha kötü olurdu). Ama iki sonucu
var:

1. **Guard'ın "tek kapı" iddiası tam değil.** Aşağı akıştaki her `GateResult` tüketicisi NaN'a
   hazırlıklı olmak zorunda. Bugün `GateResult.InfoNeed`'i okuyan bir tüketici yok
   (`ActuationLayer`'da grepledim: sıfır kullanım) — yani bu **gelecekteki** bir tuzak.
2. **Bilgi kaybı:** `stake = -5.0` **ölçülebilir** bir ihlaldir (sonlu, sadece tanım dışı) ama
   çıktıda ölçülemez (NaN) hâle geliyor. `Reason` metni de "girdi sonlu/aralıkta değil" diyerek
   iki ayrı hatayı tek mesajda birleştiriyor.

**Doğru kapanış:** `GateResult.InfoNeed`'i `double?` yapmak — "ölçülemedi" `null`'dur, NaN değil.
Tip sistemi tüketiciyi kontrol etmeye zorlar.

> **Kanıt testi:** `AUDIT_DEFECT_W17_gate_emits_NaN_InfoNeed_out_of_the_guarded_boundary`

---

## 6. SALDIRDIM, KIRILMADI (dürüstlük gereği — bunlar gerçek kazanımlar)

| İddia | Nasıl sınadım | Sonuç |
|---|---|---|
| `Guard`'ın her public metodu NaN'a kapalı | **reflection ile mekanik tarama** (yeni metotlar otomatik kapsanır) | **Sağlam** (`W1`) |
| `stake·(1−conf)·deficit` **taşabilir** mi (→ `Infinity`)? | `double.MaxValue`, `1e308`; + 1000 fuzz'da `priority ≤ stake` invariant'ı | **Yapısal olarak imkânsız** — her iki çarpan da ≤ 1 (`W4`) |
| Denormal / `double.Epsilon` / `-0.0` stake | doğrudan + fuzz | **Muhafazakâr**: underflow önceliği düşürür, exception yok (`W5`) |
| Eşikte **tam eşitlik** (`infoNeed == blockThreshold`) | `>=` semantiği + `Math.BitIncrement` ile bir ULP testi | **Fail-closed**: eşik değeri kısıtlayıcı tarafta (`W6`) |
| Yetkisiz araç gerçekten bloklanıyor mu | registry → gate → Scheduler, uçtan uca | **Bloklanıyor**, sessizce yutulmuyor (`W12`) |
| `Disable` bir güvenlik kontrolünü düşürebilir mi (F3 regresyonu) | katı Pack devre dışı + gevşek Pack izinli | **Kısıt korunuyor** (`W13`) |
| `ToolAuthorization` gate'i GEVŞETEBİLİR mi | yüksek InfoNeed + izinli araç; geri-dönülemez + izinli araç | **Yalnızca sıkılaştırıyor** (`W14`) |
| Sıralama gerçekten `InfoNeed × ConformanceDeficit`'e uygun mu | **1000 rastgele karar**, beklenen değer `DecisionGravity` çağrılmadan **elle** hesaplandı; priority + tier + gate + sıra + çoklu-küme bütünlüğü | **Birebir uyuyor** (`W18`) |
| `ConformanceDeficit` çarpanının ayırt edici gücü var mı (AUDIT §3.1 itirazı) | InfoNeed sırası ile priority sırasını **kasıtlı olarak ters** kurdum | **Var**: 10× InfoNeed'li karar geride kalıyor (`W19`) |
| `ScheduleTop` bütçesi: `int.MaxValue`, `int.MinValue`, negatif, taşma, önek tutarlılığı | hepsi | **Güvenli**; `Take` taşmıyor, önek özelliği tutuyor (`W21`) |

İki not, koordinatör lehine:

- **`stake * (1-conf)` taşma hipotezi (görevde özellikle sorulan) yapısal olarak imkânsız.**
  Bu bir şans değil, formülün biçiminin sonucu: çarpanlar `[0,1]`'e sıkıştırılmış. Kırpmanın
  (`NormalizedDeficit`) yan faydası burada ortaya çıkıyor.
- **Fuzz, formül sadakatinde hiçbir sapma bulmadı.** ENS-3022'nin operasyonelleştirilmesi
  sıralama düzeyinde gerçekten sadık. Bu, ADR-0001 §5.3'ün en spesifik iddiası ve **ayakta**.

---

## 7. İKİNCİL BULGULAR (düşük şiddet, tam liste)

| # | Bulgu | Şiddet | Test |
|---|---|---|---|
| W3 | `Math.Clamp(-0.0, 0, 1)` → `-0.0`; "clamp çıkışı ≥ 0" IEEE anlamda tam değil. `Comparer<double>` `-0.0`'ı `0.0`'ın altına koyar. **İstismar edilemez** (yalnızca kendi önceliğini düşürür). | Düşük | `W3` |
| W7 | `confidence = 1.0` tam eşitliği, `1e300` stake'lik bir karara **tam otonomi** verir; bir ULP aşağısı (`0.99999999999999989`) aynı stake'te **Blocked**. Guard ölçülebilirliği kapattı, **kalibrasyonu kapatmadı** — `Guard.cs` bunu kendi dürüst-sınırında zaten söylüyor. | Orta (kabul edilmiş borç) | `W7` |
| W20 | AttentionPriority **ve** InfoNeed tam eşitse sıra girdi permütasyonuna bağlı. `Scheduler.cs (e)` bunu açıkça açık borç ilan ediyor — kapanmadı, kanıtlandı. | Düşük | `W20` |
| W22 | `Schedule`/`ScheduleTop` canlı `List<>` döndürüyor; `((List<ScheduledDecision>)result).Reverse()` P5 sırasını tersine çevirir. Hafifletici: her çağrıda taze liste. AUDIT §5.2'nin "tüm koleksiyon getter'ları savunmacı kopya" talebi burada uygulanmamış. | Düşük | `W22` |
| W23 | `PendingDecision` **kuruluşta doğrulama yapmıyor** — `new PendingDecision(id, NaN, NaN, NaN)` serbestçe üretilip taşınabiliyor, patlama başka bir katmanda oluyor (AUDIT §5.4/G6'daki `MemoryRecord` deseninin tekrarı). Ayrıca koleksiyon `null`'a karşı korunuyor ama **eleman** `null`'a karşı korunmuyor → `NullReferenceException`. | Düşük | `W23` |
| W24 | `decision < GateDecision.Blocked` (satır 126) enum'un **sayısal sırasına** bağlı. Enum üyelerini yeniden sıralamak — kozmetik görünen bir refactor — P7 yükseltmesini sessizce tersine çevirir. Test bunu bir kanaryaya çevirdi. | Düşük (gizli bağımlılık) | `W24` |

---

## 8. VERDICT

### `wounded` — düzeltme gerçek ve ölçülebilir bir ilerleme, ama **kapanış iddiası fazla geniş.**

- **`Guard.cs`'in kendisi sızdırmıyor.** Saldırdım; her değer döndüren metot NaN'a kapalı, her
  iddia edilen çağrı noktası gerçekten bağlı. Bu bir kazanım ve dürüstçe kaydedilmeli.
- **Ama "kök neden 7 noktada kapatıldı" iddiası yanlış:** sekizinci nokta (`LlmTierSelector`)
  atlandı ve Scheduler üzerinden erişilebilir durumda (§2).
- **Gate'in fail-closed tasarımı, tek tüketicisi tarafından baypas ediliyor** (§3.1) ve tek bozuk
  girdi tüm dikkat tahsisini düşürüyor (§3.2) — `Guard.cs`'in kendi ilan ettiği anti-DoS ilkesine
  aykırı.
- **Registry→gate bağı gerçekten kuruldu** (§4.1, bu turun en net kazanımı) **ama opt-in** (§4.3)
  ve yeni güven sınırı mühürsüz + dürüst-sınırlar bloğunda kayıtsız (§4.2).
- **Sıralama matematiği sadık.** 1000 karar, elle hesap, sapma yok.

**Yeni kusur sayısı: 7 önemli + 6 ikincil = 13.**
**En kritiği: `AUDIT_DEFECT_W11`** — NaN fail-open sınıfı KAPANMADI; `LlmTierSelector` Guard'a hiç
bağlanmadı ve `Scheduler` NaN tier eşiklerini sessizce kabul edip her kararı en zayıf modele
yönlendiriyor. Saldırgan gerekmez.

### Kapıyı geçmek için gereken (sahibine talepler)

1. **`dotnet test` gerçekten koşturulup TAM çıktısı repoya yapıştırılmalı** — bu, üst üste ikinci
   turdur ki denetim bunu yapamıyor. `AdversarialWave_SchedulerGateTests.cs` derlenmeden bu
   raporun hiçbir satırı "çalıştırılmış kanıt" değildir. Derleme hatası çıkarsa **bunu da yazın**.
2. **`LlmTierSelector`'ın üç `double` girdisi `Guard`'dan geçmeli**; `Guard.cs`'in nokta listesi
   sekize çıkmalı (§2).
3. **`Scheduler` ölçülemeyen kararı KARANTİNAYA almalı, partiyi düşürmemeli**: sonuçta
   `CriticalBlock` + "ölçülemedi" etiketiyle **yer almalı**, `DecisionId` ile birlikte (§3.1, §3.2).
4. **Politika ayrı bir tip olmalı** (`GatePolicy`) ve **kuruluşta** doğrulanmalı — karar-başına
   yan etki olarak değil (§3.3).
5. **`ToolAuthorization` mühürlenmeli** (opak token / registry imzası) ya da en azından
   `BoundedAutonomyGate.cs`'in "DÜRÜST SINIRLAR (a)" maddesine **eklenmeli**; bağ opt-in olmaktan
   çıkarılmalı (`PendingDecision.ToolName` + Scheduler'ın registry'yi kendisi sorması) (§4.2, §4.3).
6. **`GateResult.InfoNeed` `double?` olmalı** — "ölçülemedi" `null`'dur, NaN değil (§5).

### Bu raporun kendi yanlışlanabilirliği (Madde X)

En zayıf halkam, **hiçbir testin koşturulmamış olması**. Bulguların dayanağı IEEE-754 semantiği ve
kaynak okumadır (yüksek güven), ama bir derleme hatası ya da benim gözden kaçırdığım bir çağrı
sırası, herhangi bir `AUDIT_DEFECT_W*` testini kırabilir.

Somut çürütme koşulları:

- `AUDIT_DEFECT_W11` kırmızı yanarsa → **§2 tümüyle çürür** ve bu raporun başlığındaki "en kritik"
  iddiası düşer.
- `AUDIT_DEFECT_W8`/`W9` kırmızı yanarsa → §3 düşer; Scheduler'ın çağrı sırasını yanlış okumuşum
  demektir.
- `AUDIT_DEFECT_W15` kırmızı yanarsa → `record` `with` semantiği hakkında yanılmışım demektir.
- `AUDIT_HOLDS_W18` kırmızı yanarsa → **koordinatör lehine olan en güçlü bulgum** (formül sadakati)
  düşer ve durum bu rapordan **daha kötü** demektir.

Testleri koşturun. Kırılan her `AUDIT_*` testi benim bir hatamdır ve onu görmek isterim.

---

*ens-skeptic, 2026-07-26. Bu belge `AUDIT.md`'den ve koordinatörün README'sinden bağımsızdır;
çelişki hâlinde önce hangisinin **çalıştırılmış** kanıta dayandığına bakın — ki bu turda
hiçbirinin dayanmadığını yukarıda yazdım.*
