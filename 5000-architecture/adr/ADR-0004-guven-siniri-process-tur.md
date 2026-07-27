---
id: ADR-0004
type: adr
canon: false
constitutive: false
status: draft
owner: fikri-eren
version: 0.1.0
last_reviewed: 2026-07-27
depends_on: [ADR-0001, ADR-0003, ENS-0000]
referenced_by: []
skeptic_review: pending
failure_conditions: stated
principles: [P6, P7]
---

# ADR-0004 — Güven sınırı **process**tir: in-process yetki korunamaz

> **Bu bir kapanış belgesidir, bir iterasyon değil.** `ADR-0003`'ün K-1 kararı ve `DP5`
> (reflection) kapsam sorusu **aynı soruydu** ve ölçümle cevaplandı. Belge o cevabı kaydeder.

---

## 1. Karar

> **.NET'te in-process hiçbir tip-tabanlı şema, o kodu yazabilen bir çağırana karşı yetkiyi
> koruyamaz.** Güven sınırı **çağrı grafiği değil, process**tir.
>
> Bunun sonucu: `ADR-0003` K-1'in hedefi **küçülür** — tip-tabanlı mühür *"taklit edilemez"*
> değil, **"taklit maliyetini `Unsafe`/process sınırına taşır"** demektir.

## 2. Neden ayrı bir ADR

`ADR-0003` altı karara birden bakıyordu. K-1 iki turda da kırıldı ve her kırılışta **diğer beş
karar da** yeni sürüme giriyordu. Ama K-1'in sorunu diğerlerinden **cinsi olarak farklı**:

| | Diğer kararlar | K-1 |
|---|---|---|
| Sorun türü | Mekanizma seçimi | **Mümkün mü?** |
| Çözüm | Doğru tipi seç | Cevap: *bu ortamda hayır* |
| Turların işlevi | Mekanizmayı düzelt | **Sınırı bulmak** — ve bulundu |

`CEO-0002`'nin `RFC-6001` üzerine düştüğü **kapsam-orantısı** uyarısı burada da geçerliydi;
`ADR-0003` onu ihlal etti (ve ironik olarak aynı uyarı RFC'ler için **alıntılanmıştı**).

## 3. Kanıt — ölçüldü, tartışılmadı

`ENG-0002` buldu, oturum sahibi **bağımsız doğruladı**
(`scratchpad/verify-unsafe`, .NET 10, `AllowUnsafeBlocks=false`):

```csharp
var registrySeal = new Seal();
var legit = ToolAuthorization.Issue("read_file", 1, registrySeal);

var shadow = Unsafe.As<Shadow>(legit);   // reflection YOK, BindingFlags YOK
shadow.Tool  = "wire_transfer";
shadow.Scope = 9999;
```

```
Tool=wire_transfer Scope=9999
ReferenceEquals(mühür) = True
Aynı nesne mi = True
```

**Kritik ayrıntı:** registry'nin **kendi verdiği** nesne **yerinde** değişiyor.
`ReferenceEquals` bunu göremez — çünkü gerçekten aynı nesnedir. Mühür doğrulaması
*yanlış cevap vermiyor*; **doğru cevap veriyor ve o cevap artık anlamsız.**

### 3.1 `sealed class`'ın kapattığı şey

`ENG-0002` ölçtü: `sealed class` + `private` kurucu, `record`'un `with`'ini **kapatıyor**
(`CS8858`). Yani v0.3.0'ın D-1 kararı **bir şey kazandı** — ama kazandığı şey **operatör**dü,
**saldırı sınıfı** değil.

## 4. `DP5` ile birleşme

`ADR-0003` `DP5`'i (reflection: `E5`, `W3c`) *"kapsam kararı, düzeltme değil"* diye ayırmıştı
ve haklıydı — ama **soruyu ayrı sanıyordu.** Ölçüm ikisinin **tek** soru olduğunu gösterdi:

> *Güven sınırı nerede?*

Cevap **process** ise, `DP5` ve K-1 **birlikte** kapsam dışına çıkar. Ve bu, `ADR-0003`'ün
kapanma iddiasını değiştirir: `E3`, `W15`, `W4a` de — `E5`/`W3c` gibi — **kısmen** açık kalır.

## 5. Ne kazanıyoruz — hedef küçülürken kaybolmayan değer

Tip-tabanlı mühür **işe yaramaz değildir**; yanlış tanımlanmıştı:

| Saldırı | Mühür durduruyor mu |
|---|---|
| `new ToolAuthorization(...)` — düz veri üretimi | ✅ |
| `with { IsAllowed = true }` | ✅ (`sealed class`, `CS8858`) |
| Kazayla yanlış nesne geçirme | ✅ |
| `Unsafe.As` ile yerinde mutasyon | ❌ |
| Reflection ile alan yazma | ❌ |

Gerçek kusur kayıtlarının **çoğu** ilk üç satırdır. `E3` (*"tek satırda taklit"*) ve
`W15` (*"public record olduğu için aklanabiliyor"*) tam olarak o sınıftır.

> **Karar cümlesi bu yüzden şöyle olmalı:** mühür **kazayla ve gündelik** taklidi durdurur;
> **kararlı** bir saldırganı durdurmaz ve durdurduğunu **iddia etmez**.

## 6. Bu kararın sonuçları

1. `ADR-0003` K-1'i **devretti** — bu belgeye. `ADR-0003` v0.5.0'da K-3/K-4/K-5/K-6'ya daraldı.
2. `DP5` artık *"karar bekleyen kapsam sorusu"* değil, **cevaplanmış**: kapsam dışı, çünkü
   in-process savunulamaz.
3. `RFC-6004`'ün `Mediated` niteliği bundan etkilenir: *"her yetki kullanımı her seferinde
   mekanizmadan geçer"* in-process **garanti edilemez**. Nitelik ya process sınırına
   koşullanmalı ya yanlışlanma koşuluna bu sınır yazılmalı. **RFC-6004'e talep edildi.**
4. Gerçek kapanış isteniyorsa yol **process ayrımıdır** (ayrı process/AppDomain-benzeri
   izolasyon, ya da yetki kararının kernel dışında verilmesi) — **bu ADR onu önermiyor**,
   yalnız tek yolun o olduğunu kaydediyor.

## 6.5 `ADR-0003`'ün **K-4**'ü buraya devredildi (2026-07-27)

`ENG-0003` ve `SKR-051` K-4'ü (politika eşikleri = varyant) iki ayrı sebeple düşürdü:

1. **R12 azaltmasının tamamı bu belgenin konusu olan mühre dayanıyor** — ve §3'te mührün
   *kararlı bir saldırganı durdurmadığı* ölçüldü. K-4'ün tehdit modeli **tam olarak
   kernel'e kod yazabilen** roldür; yani mühür orada zaten yok.
2. **`default(DecayRate).Value == 0.0`** — kurucu hiç çalışmıyor, yani `A5` (`contextDecayRate
   = 0` sessiz kapatma anahtarı) **geri açılıyor**. `ADR-0003`'ün metni *"tip bunu ctor'da
   yakalar"* diyordu; `struct` `default`'unda ctor **çalışmaz**.

> **Neden burada:** K-4'ün çözülmemiş sorusu *"eşik tipi nasıl yazılır"* değil,
> **"mühür yokken politika eşiği neye dayanır"**dır — ve o soru bu belgenin konusudur.

**Getirdiği kusurlar:** `DP4` = `A5` `E4` `G2` `H3` `W10` (5).
**Getirdiği açık soru:** `struct` `default` deliği `DP4` için nasıl kapatılır — `class` mı,
factory mi, yoksa politika eşiği hiç tip olmamalı mı?

## 7. Failure conditions (Madde X)

**Yanlıştır** eğer:

1. **`Unsafe.As` ölçümü yeniden üretilemezse.** Spike diskte
   (`scratchpad/verify-unsafe`); bağımsız bir tur onu çalıştırıp farklı sonuç alırsa bu
   belgenin tamamı düşer.
2. **.NET'te `Unsafe`'i çağrı-yeri bazında engelleyen bir mekanizma varsa** ve bulunmadıysa.
   `BannedApiAnalyzers` `System.Runtime.CompilerServices.Unsafe`'i yasaklayabilir mi?
   **DOĞRULANMADI** — sınanmalı. Eğer yasaklanabiliyorsa "in-process korunamaz" iddiası
   *"disiplinle korunabilir"*e zayıflar (ama analyzer `#pragma` ile bastırılabilir —
   `ENG-0001` bunu ölçtü).
3. **Kernel bir gün gerçekten process-izole olursa** ve K-1'in orijinal hedefi ulaşılabilir
   hâle gelirse — bu belge o gün `superseded` olur.

## 8. Bu ADR'nin yolu

`work-protocol.md` §3.1 gereği **iki boyut**: bir `ens-skeptic` turu + bir mühendislik turu.
Mühendislik turunun ilk işi **failure condition 2**'yi ölçmek olmalıdır (`Unsafe` yasaklanabilir mi).

**Yazarı kendi turunu `survives` işaretleyemez** — GOV-000 **G4** + **G3**.

## 9. İlişkili
- `ADR-0003` §0.9 — bulgunun ilk kaydı
- `5000-architecture/reviews/ENG-0002-*` — ölçüm
- `7000-reference-implementation/DEFECT-PATTERN-MAP.md` — `DP1`, `DP5`
- `RFC-6004` — `Mediated` niteliği (talep §6/3)
