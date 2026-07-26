# ENS Reference Implementation — Faz 4

**Yetki:** Anayasa Madde VII (Faz 4: teoriyi kanıtla, optimize etme) + Madde VIII (yalnızca
**Accepted** ADR'lere dayanır).

Bu, ENS'in ilk çalışan kodudur. Amaç production değil — **teorinin çalışabilir olduğunu
kanıtlamak** (P8).

## Yapı
```
Ens.Kernel/
  Domain/
    Identity.cs           — TRACE: ENS-4001 §Computational primitifler (deneysel, açık soru)
    DomainEvent.cs         — TRACE: ENS-4001 §Event, Axiom 3 (Non-Leakage)
    DecisionAggregate.cs   — TRACE: ENS-2001 §Individuation, ADR-0001 §5.4
    ContextScore.cs         — TRACE: ENS-2002 §3 (coverage−noise−staleness) + §Implications (gate)
    CompanyMemory.cs        — TRACE: ENS-2003 v0.4.0 §3/§3a/§3b (RetentionPriority=|L| ⊥ value=|L|·c ⊥ decayFactor=exp(−λ_π·Δt); karşı-survivorship tabanı; asserted_at/last_verified; Curator'ın İKİ sinyali)
    ReflectiveDoubleLoop.cs — TRACE: ENS-2004 v0.3.2 §4a (öneri-üreticisi, P7 yapısal — state mutasyonu yok)
    Events/DecisionEvents.cs — TRACE: ENS-2001 §Lifecycle, ENS-2004 §3 (Attribution)
  Laws/
    DecisionEntropy.cs     — TRACE: ENS-3021 (H(A|C) = I(A;Owner|C) + H(A|C,Owner))
    DecisionGravity.cs     — TRACE: ENS-3022 (InfoNeed = Stake × (1−Confidence), Howard 1966)
    DecisionCapital.cs     — TRACE: ENS-3023 (Value/ΔCapital/ReuseROI — akış, stok değil)
  Adapter/
    LlmAdapter.cs           — TRACE: ADR-0001 §7 (model-agnostik port) + §5.3 (tier↔InfoNeed bağı)
  Scheduler.cs             — TRACE: ADR-0001 §5.3, ENS-3022, P5 (attention tahsisi — sıralayıcı, çalıştırıcı değil)
  ProofTrace.cs            — TRACE: ADR-0001 §5.5, ENS-4025 L7/L8, Madde VI/P6 (izsiz türetim temsil EDİLEMEZ)
  ActuationLayer.cs        — TRACE: ADR-0001 §5.4 (guarded action lifecycle; geçersiz geçiş = exception)
  Capability/CapabilityRegistry.cs — TRACE: ADR-0001 §6/§6.1 (Pack izinleri → P7 gate; ENS deltası)
  BoundedAutonomyGate.cs   — TRACE: ADR-0001 §5.6, Anayasa P7 (bounded autonomy'nin ilk kodu)
  Guard.cs                 — TRACE: Anayasa P7/P6 + AUDIT.md §5.1 (fail-closed sayısal girdi kapısı)
Ens.Kernel.Tests/           — invariant testleri (ADR/teori iddialarını doğrular)
  AdversarialAuditTests.cs  — BAĞIMSIZ düşmanca denetim (ens-skeptic), bkz. AUDIT.md
Ens.Kernel.Demo/            — uçtan uca senaryo: tedarikçi seçimi kararı, tüm parçalar tek akışta
```

## Durum
| Yapıt | Test | Kanıt seviyesi (evidence-standard.md) |
|---|---|---|
| ENS-2001 (Decision, §Individuation) | 8/8 geçti | eng: E0 → **E1** |
| ENS-2002 (Context Score) | 9/9 geçti | eng: E0 → **E1** |
| ENS-2003 (Company Memory) | ✅ v0.4.0 sonrası **owner koşusuyla doğrulandı** (D-5 çift-sayım düzeltmesi + karşı-survivorship tabanı; breaking API, 5 dosya) | eng: E0 → **E1** |
| ENS-2004 §4a (Reflective double-loop) | 10/10 geçti | eng: E0 → **E1** (kısmi: yön-sapması değil büyüklük-tekrarı, proof-trace yok) |
| ENS-3021 (Decision Entropy) | 5/5 geçti | eng: E0 → **E1** |
| ENS-3022 (Decision Gravity) | 8/8 geçti | eng: E0 → **E1** |
| ENS-3023 (Decision Capital) | 10/10 geçti | eng: E0 → **E1** |
| ADR-0001 §5.6 (Bounded-Autonomy Gate) | 6/6 geçti | eng: E0 → **E1** |
| ADR-0001 §7 (LLM Adapter Port) | 15/15 geçti | eng: E0 → **E1** (saf port; somut sağlayıcı yok) |
| ADR-0001 §5.3 (Scheduler) | 14/14 geçti | eng: E0 → **E1** (sıralayıcı; çalıştırıcı/kuyruk değil) |
| ADR-0001 §5.5 + ENS-4025 L7/L8 (Proof-Trace) | 11/11 geçti | eng: E0 → **E1** (zarf + invariant; otomatik emit yok) |
| ADR-0001 §5.4 (Actuation Layer) | 15/15 geçti | eng: E0 → **E1** (lifecycle invariant; sandbox/yürütme yok) |
| ADR-0001 §6 (Capability Registry) | 11/11 geçti | eng: E0 → **E1** (izin→gate bağı **artık kodda**; sandbox/dinamik yükleme yok) |
| **Bağımsız düşmanca denetim** (AUDIT.md) | 52/52 geçti | ayrı context'in yazdığı kırma testleri |

> **GÜNCEL KOŞU (2026-07-26, owner doğrulaması — tek gerçek sayı budur):**
> `dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj` → **Toplam 373, Başarılı 373, Başarısız 0.**
> İçerir: D-5 (confidence çift-sayımı, ENS-2003 v0.4.0), W11 (NaN 8. nokta), D-1 (ProofTrace↔
> DecisionAggregate bağı), E6 (kapıda-doğrulama) düzeltmeleri. Yukarıdaki tablo **artım-artım
> yazıldığı için bileşen bazında eskiyebilir** — çelişki hâlinde bu satır geçerlidir.

### Bağımsız denetim (AUDIT.md) ve kapanışı — 2026-07-25

Demoyu ve testleri **yazmayan** ayrı bir context (`ens-skeptic`) kernel'i kırmak için 52 test
yazdı (`Ens.Kernel.Tests/AdversarialAuditTests.cs`). Bulduğu kusurların çoğu gerçekti ve
kapatıldı. Test adları sonucun kendisini taşır:

| Önek | Sayı | Anlamı |
|---|---|---|
| `AUDIT_FIXED_*` | 25 | Kusur **kapandı**; test artık düzeltmeyi bekçiliyor (regresyonda kırılır) |
| `AUDIT_HOLDS_*` | 18 | İddia saldırıdan sağ çıktı |
| `AUDIT_DEFECT_*` | 5 | Kusur **hâlâ açık** — bilinçli, gerekçesi aşağıda |
| `AUDIT_FINDING_*` | 4 | Kod doğru, **demo'nun sunumu** yanıltıcı |

**Kapanan kusurların özeti** (tümü `AUDIT_FIXED_*` ile kanıtlı):

1. **P7 gate'i NaN/aralık altında FAIL-OPEN'dı** (en kritik). `stake = NaN` ya da
   `confidence = 5.0` veren bir çağıran **Autonomous** (tam otonomi) alıyordu; Scheduler
   üzerinden üçlü fail-open oluşuyordu (gate açık + en ucuz model + sıralamada en son). Kök
   neden `x is < 0 or > 1` deseninin NaN körlüğüydü (IEEE-754) ve 5+ dosyada tekrarlıyordu.
   → Tek kapı: `Ens.Kernel/Guard.cs`. Ölçülemeyen girdi artık **reddedilir**, otonomi kazanamaz.
2. **"Asla silinmez" downcast'le kırılıyordu** (reflection bile gerekmeden): memory kayıtları,
   action audit geçmişi, decision event akışı silinebiliyor; Capability Pack'e **sonradan
   yetki eklenebiliyordu**. → `ReadOnlyCollection` / `FrozenSet`; downcast artık `InvalidCastException`.
3. **`Rehydrate` §Individuation'ı atlıyordu** — Purpose'suz, Alternative'siz, iki commitment'lı
   "karar" replay ile üretilebiliyordu. → Replay yolu artık canlı yolla **aynı** invariant'lardan
   geçer; `IdentifyAlternatives` deliberation kümesini snapshot'lar.
4. **Company Memory decay bypass'ları** — gelecek tarihli `Verify` decay'i kalıcı olarak
   kapatıyordu; `Verify` DecisionId ile anahtarlıydı (çapraz-kirlenme); `|Learning|=0` /
   `confidence=0` kayıtlar Curator'a **asla** görünmüyordu; geçersiz kayıt sessizce yazılıp
   sonra her retrieval'i patlatıyordu. → Dördü de kapandı; `Verify` artık gerekçe ister ve iz bırakır.
5. **Registry↔Gate bağı kodda yoktu** — ADR-0001 §6.1'in "ENS deltası" iddiası gerçekleşmemişti.
   → `BoundedAutonomyGate.Evaluate` artık `ToolAuthorization` alıyor: yetkisiz araç → blok,
   onay-gerektiren araç → asla otonom. Ayrıca `Disable` bir kısıtı **artık kaldıramıyor**.
6. **`ConformanceDeficit` sınırsızdı** (`1e9` ile dikkat kuyruğu ele geçirilebiliyordu) ve
   `ReflectiveDoubleLoop`'un guard'ı **kozmetikti** (`minSupportingRecords: 1` kabul ediyordu).
   → Deficit [0,1]'e normalize edilir; guard iddia ettiği şeyi gerçekten zorlar.

### Cognitive Kernel — beş bileşenin tamamı kodlandı (ADR-0001 §5.2 diyagramı)
| Bileşen | Dosya | Durum |
|---|---|---|
| Scheduler (attention/gravity) | `Scheduler.cs` | ✅ 14 test |
| Planner (decision framing) | `Domain/DecisionAggregate.cs` | ✅ 8 test |
| Bounded-Autonomy Gate (P7) | `BoundedAutonomyGate.cs` | ✅ 6 test |
| Action / Actuation Layer (guarded) | `ActuationLayer.cs` | ✅ 15 test |
| Proof-Trace Emitter (P6/L8) | `ProofTrace.cs` | ✅ 11 test |
| *(substrat)* LLM Adapter Port | `Adapter/LlmAdapter.cs` | ✅ 15 test |
| *(runtime)* Capability Registry §6 | `Capability/CapabilityRegistry.cs` | ✅ 11 test |

**Uçtan uca demo:** `dotnet run --project Ens.Kernel.Demo` — altı bileşen tek akışta, kernel'in
neyi *reddettiği* dahil (gate'siz action, izsiz trace, ikinci commit, yetkisiz araç).

Afferent yarı (Company Memory, Learning/Reflective double-loop, Context Score) + fizik üçlüsü
(Entropy/Gravity/Capital) de kodlu — bkz. yukarıdaki tablo.

> **Not (Anayasa Madde VI kodda zorlanıyor):** `ProofTrace` öncülsüz **kurulamaz** (constructor
> reddeder) ve `Confidence` yalnızca ENS-4025 L7'nin `min` t-norm'uyla hesaplanır, atanamaz —
> yani "izsiz çıkarım" bu kod tabanında **temsil edilemez**. ADR-0001 §5.5'in "onlarda audit-log
> sonradan eklenir; ENS'te proof-trace action'ın var-olma koşuludur" iddiasının yapısal karşılığı.
> Denetim sonrası bu artık **kurulduktan sonra da** geçerli (öncüller `ReadOnlyCollection`,
> downcast'le boşaltılamıyor — `AUDIT_FIXED_D3/E6`).
>
> **2026-07-26 (D-1 kapanışı):** İz artık `ProofTrace.cs`'te *duran* bir zarf değil, **Decision
> atomuna bağlı**: `DecisionAggregate.Commit` bir `ProofTrace` **üretir** (`CommitmentTrace`),
> Evidence boşsa commitment **reddedilir** ve commitment confidence'ı öncüllerin L7 t-norm'unu
> **aşamaz** — hem canlı hem replay yolunda. Daha önce iki dosya birbirini tanımıyordu; iddia
> vardı, bağ yoktu (`AUDIT-WAVE2-FIDELITY.md` §D-1).
>
> **Ama sınır dar olarak ifade edilmeli:** zorlanan şey *kardinalite* + *bağlantı*dır (en az bir
> boş-olmayan öncül, atomun izine bağlı), *kanıt* değil — uydurma bir öncül hâlâ
> `confidence = 1.00` üretebilir (`AUDIT_FINDING_D5`, `AUDIT_DEFECT_D1_residual_*`). Öncülün
> ENS-4010 node'una tipli referans taşıması Faz-4'ün açık borcudur.

> **Not (ADR-0001 §5.3/§7'nin ENS-deltası kodda):** Ne port ne scheduler jenerik değil —
> `Scheduler` sıralama ölçütünü `DecisionGravity.AttentionPriority`'den (ENS-3022), `LlmTierSelector`
> tier seçimini `DecisionGravity.InfoNeed`'den türetir. Bu, ADR-0001 §5.3'ün "AIOS'un scheduler'ı
> boştur; ENS onu VOI'ye (Howard 1966) bağlar" iddiasının çalışan kanıtıdır — bir test bunu
> formülle birebir karşılaştırarak doğruluyor (`Priority_matches_DecisionGravity_formula_exactly`).

## AÇIK KALAN KUSURLAR (denetim buldu, KAPATILMADI — gerekçesiyle)

Bunlar "sadeleştirme" değil, **bilinen kusur**. Her biri hâlâ `AUDIT_DEFECT_*` adıyla test
edilmekte, yani kusur bir gün kapanırsa test kırılıp bu bölümün güncellenmesini zorlayacak.

### Dalga-2 denetimlerinden KAPANAN iki kusur (2026-07-26)

İki **bağımsız** denetim (`AUDIT-WAVE2-SCHEDULER.md`, `AUDIT-WAVE2-FIDELITY.md`) iki gerçek kod
kusuru buldu; ikisi de kapatıldı ve kapanış `AUDIT_FIXED_*` testleriyle bekçileniyor.

1. **W11 — NaN fail-open sınıfı KAPANMAMIŞTI (sekizinci nokta).** `Guard.cs` "kök neden 7 noktada
   kapatıldı" diyordu; sayım **yanlıştı**. `Adapter/LlmAdapter.cs`'teki `LlmTierSelector` `Guard`'ı
   **hiç çağırmıyordu** ve aynı NaN-kör deseni tekrarlıyordu (`criticalThreshold < complexThreshold`
   → `NaN < NaN` → `false`). `Scheduler` eşikleri doğrudan geçirdiği için `complexThreshold: NaN`
   veren bir çağıran — ya da `0.0/0.0` üreten bir kalibrasyon fonksiyonu — **1 milyar TL stake'li
   bir kararı sessizce en zayıf modele** yönlendirebiliyordu (P5 ihlali; saldırgan gerekmez).
   → Tier seçiminin **üç `double` girdisi de** `Guard`'dan geçer; `Scheduler` tier politikasını
   **parti boş olsa bile** doğrular. `Guard.cs` nokta listesi 7 → **9**. Kanıt: `AUDIT_FIXED_W11`,
   `AUDIT_FIXED_W6a/W6b/W6c/W6f` (aynı kök nedeni üçüncü bir dalga da bulmuştu).
   *Kapsam sınırı:* gate eşikleri (`autonomyThreshold`/`blockThreshold`) **bilerek** dokunulmadı —
   o, ayrı ve hâlâ açık olan `AUDIT_DEFECT_W10`'un (`GatePolicy` tipi talebi) konusudur.
2. **D-1 — `ProofTrace` Decision atomuna bağlı DEĞİLDİ.** `DecisionAggregate.cs:8` "izsiz
   commitment yasak (ENS-4025 L8, Madde VI)" **iddia ediyordu**; gerçekte `evidence` yalnızca
   null-check'ten geçiyor, `Apply()` onu saklamıyor, `Commit()` ona bakmıyordu ve `ProofTrace`
   o dosyada **hiç kullanılmıyordu** → `IdentifyAlternatives(who, ["A","B"], evidence: [])` +
   `Commit(...)` **sıfır-öncüllü** bir commitment üretiyordu.
   → **Seçilen yol (a): kodu iddiaya yükseltmek** (iddiayı düşürmek değil). Gerekçe ADR-0001
   (**accepted**, v0.3.1) §5.5 *"proof-trace action'ın var-olma koşuludur"* + §5.4 *"proof-trace
   **atom düzeyinde** zorunludur"* — ve bu dosyadaki atom `DecisionCommitted`'dır. Bağ, kabul
   edilmiş bir ADR'nin **gereğidir**, yeni mimari icadı değildir (Madde VIII/IX). Yol (b) —
   TRACE'i dürüstleştirip borç yazmak — Accepted bir ADR'yi kodda uygulamamak olurdu.
   → Somut: `Evidence` artık `Premise` (kaynak + confidence) listesidir ve **boş olamaz**;
   `Commit` bir `ProofTrace` **üretir** (`CommitmentTrace`), commitment confidence'ı öncüllerin
   **L7 min-t-norm'unu aşamaz**; üçü de **replay yolunda da** zorlanır; iz olayın içine
   kopyalanmaz, **akıştan hesaplanır** (Axiom 2 — canlı/replay ayrışması yapısal olarak imkânsız).
   Kanıt: `AuditFixed_CommitmentProofTraceTests.cs`.
   *Bilinçli sapma (dürüst kayıt):* ENS-4025 L7 `conf(sonuç) = **min**(öncüller)` der; kod `≤`
   zorlar. Gerekçe: commitment confidence'ı ENS-2001'de karar vericinin kalibre öz-değerlendirmesi
   de içerir; `=` dayatmak `confidence` parametresini anlamsız kılardı. `≤` L7'nin muhafazakâr
   yönüdür (öncüllerin desteklemediği güven iddia edilemez; temkinli olmak serbest). t-norm seçimi
   ENS-4025 §Failure'da zaten açık bir RFC borcudur — bu kural o RFC ile yeniden ele alınmalı.
   *Şema notu:* `AlternativesIdentified.Evidence` tipi `string[]` → `Premise[]` oldu. Kalıcı bir
   event-store olmadığı için upcaster yazılmadı; gerçek bir store'da **gerekirdi**.

**D-1'in KAPATMADIKLARI** (kapandığı iddia edilmiyor, testle işaretli):
- Öncüller hâlâ **serbest metin + öz-beyan confidence**; ENS-4010 Context/Evidence/Memory node'una
  tipli referans yok, kalibrasyon yok (`AUDIT_DEFECT_D1_residual_premises_are_still_uncalibrated_free_text`).
- İz yalnızca **commitment atomunda** üretilir; Enactment/Measurement/Learning kendi izlerini
  üretmez (`AUDIT_DEFECT_D1_residual_only_the_commitment_atom_emits_a_trace`). ADR-0001 §5.4 bunu
  atom düzeyiyle sınırladığı için kapsam meşrudur, ama §5.5'in tam hâli değildir.
- `min` idempotenttir: korroborasyon (çok sayıda bağımsız öncül) confidence'ı **artırmaz**
  (`AUDIT_FINDING_W2_P5`) — dolayısıyla `≤` kuralı korroborasyonu ödüllendirmez.
- `Apply`'daki `as ReadOnlyCollection<string>` kestirmesi **Alternatives** tarafında hâlâ canlı
  görünüm sızdırıyor (`AUDIT_DEFECT_W2_R4`, açık). **Evidence** tarafında aynı delik açılmadı:
  öncül kümesi her zaman gerçek kopyadır.
- **W6f artığı:** sonlu ama absürt büyük tier eşikleri (`1e300`) hâlâ kabul ediliyor; doğru çözüm
  bir **kalibrasyon** kararıdır (ENS-3022 stake normalizasyonu), doğrulama kapısı değil —
  uydurulmuş bir üst sınır Madde IX ihlali olurdu.

- **Sahte Gate sonucu tek satırda üretilebiliyor** (`AUDIT_DEFECT_E3`). `ActuationLayer.ApplyGate`
  yalnızca *bir `GateResult` nesnesi verildi mi* diye bakar; o nesnenin gerçekten
  `BoundedAutonomyGate.Evaluate`'ten geldiğini **doğrulayamaz** (`GateResult` public bir record).
  Doğru ifade: *"Gate'siz action imkânsız"* değil, *"GateResult **nesnesiz** action imkânsız"*.
  **Neden kapatılmadı:** kapatmak imzalı/opak bir gate-token'ı gerektirir — yeni tip, anahtar
  yönetimi, serializasyon. Bu ADR-0001'de **kararı verilmemiş** bir mimari eklemedir; Madde VIII
  gereği önce ADR/teori borcu. Faz-5 kapı şartı.
- **İnsan-onayı diye bir TİP yok** (`AUDIT_DEFECT_H1`). `ReflectiveDoubleLoop`'ta `Apply` metodu
  olmaması **yokluktan argümandır** ve yalnızca tek bir sınıf için geçerlidir; `CompanyMemory.Record`
  public ve korumasızdır, bir öneriyi otonom uygulamak üç satırdır. Demo'nun "insan onayı MİMARİ
  olarak zorunlu" cümlesi bu kadarını kanıtlamıyor. **Neden kapatılmadı:** E3'le aynı eksik parça
  (onay-token'ı = P7'nin birinci-sınıf tipi), aynı ADR borcu.
- **Reflection ile state ışınlanabiliyor, iz bırakmadan** (`AUDIT_DEFECT_E5`). .NET'in genel
  gerçeği; kapatmak proses izolasyonu ya da doğrulanmış event-store ister (ADR-0001 §6 sandbox
  borcu). Kayıt için: doğru ifade *"normal API üzerinden atlanamaz"*dır.
- **Tam eşitlikte sıralama girdi sırasına bağlı** (`AUDIT_DEFECT_B6`). `AttentionPriority` **ve**
  `InfoNeed` birebir eşitse `Scheduler` girdi permütasyonuna duyarlıdır. **Neden kapatılmadı:**
  üçüncü bir tie-breaker (ör. DecisionId ordinal) eklemek gerekir; ENS-3022 böyle bir ölçüt
  tanımlamıyor ve teoride karşılığı olmayan bir sıralama kuralı **uydurmak** Madde IX'un
  yasakladığı şeydir. Önce teori borcu.
- **`LlmTierSelector`'ın iki overload'ı bir tuzak taşıyor** (`AUDIT_DEFECT_C1`). İki pozisyonel
  `double` ile çağrılınca C# `SelectTier(infoNeed, complexThreshold)` overload'ına bağlanır —
  InfoNeed hiç hesaplanmaz. **Kernel doğru overload'ı kullanıyor** (`AUDIT_HOLDS_C2`); tuzak
  demo tarafında. **Neden kapatılmadı:** doğru düzeltme convenience overload'ı yeniden
  adlandırmaktır (`SelectTierFor`), bu bir public-API kırılmasıdır ve ayrı bir artımın işidir.

### Demo'nun sunum kusurları (kod doğru, gösteri zayıf — `AUDIT_FINDING_*`)
- Curator demosu **sıfır kayıt** bayraklıyor (`AUD-G7`): veri seçimi kıl payı eşiğin üstünde kalmış.
- Scheduler demosu `ConformanceDeficit`'in sıralamaya katkısını **hiç göstermiyor** (`B5`):
  çarpanı sıfırlasanız sıralama değişmiyor.
- Memory demosu **confounded** (`AUD-G8`): kazanan kayıt hem daha büyük |Learning|'e hem daha taze
  olmaya sahip; iki neden ayrıştırılmamış.
- Proof-trace invariant'ı **kardinalite** kontrolüdür, kanıt kontrolü değil (`D5`): tamamen
  uydurma bir öncül `confidence = 1.00` üretebilir.

## Bilinçli sadeleştirmeler (Faz-4, dürüstçe işaretli)
- **Identity**'nin primitif statüsü hâlâ açık (ENS-4001 Design Review) — kod bu tartışmayı
  çözmez, yalnızca mühendislik ihtiyacını (aggregate-id) karşılar.
- **DecisionEntropy** context-benzerliğini hâlâ `ContextKey` (string) ile temsil ediyor —
  `ContextScore` artık kodlu ama henüz `ContextKey` üretimine bağlanmadı (sıradaki adım).
- **ContextScore.coverage hâlâ dışarıdan verilir** — `CompanyMemory` artık kodlu ama
  `ContextScore.coverage`'a henüz bağlanmadı (ENS-2002 §Model 2'nin tam kapanışı, sıradaki adım).
- **CompanyMemory'nin decay fonksiyonu v0.4.0'da DEĞİŞTİ (breaking).** Bağımsız TRACE denetimi
  (`AUDIT-WAVE2-FIDELITY.md` / D-5) v0.3'ün formülünde gerçek bir teori hatası buldu: attribution
  confidence hem retention ağırlığında (`|L|·c`) hem sönüm hızında (`λ_base·(1−c)^γ`) sayılıyordu —
  **çift-sayım**, ve tam da karşı-survivorship'i (§3) zayıflatan yönde. ENS-2003 v0.4.0'da üç nicelik
  ayrıştırıldı: `RetentionPriority = |L|` (c'siz), `value = |L|·c` (= ENS-3023 §Model 1),
  `decayFactor = exp(−λ_π·Δt)` (c'siz; `λ_π = ln2/τ_π`, Purpose-tipinin context yarı-ömrü).
  `γ`/`λ_base` **kaldırıldı**; `RetrieveTop` bir **karşı-survivorship tabanı** (kesme invariant'ı)
  zorlar; Curator'a ikinci sinyal (`FindWeaklyAttributed`) eklendi. Kalibrasyon borcu kapanmadı,
  **yer değiştirdi**: artık `τ_π` kalibre edilmemiştir *ve* ontoloji bu alanı taşımaz (ENS-2003
  v0.4.0 §Failure conditions'ta v0.4.0'ın en zayıf noktası olarak yazılı).
  Teori `status: review` — bağımsız skeptic turu bekliyor (yazar kendi işini onaylamaz, G2/G3).
  ⚠️ **Bu değişikliği yapan context'te shell aracı yoktu: `dotnet test` çalıştırılamadı.** Kod ve
  testler statik olarak hizalandı; test sayıları/sonuçları **CI ile teyit edilmelidir** — bu README
  satırı hiçbir koşu sonucu iddia etmez.
- **Purpose-tipi string'dir** — ENS-2003 §Model 2'nin gerektirdiği Enterprise Ontology (ENS-4020)
  kaynaklı sınıflandırmaya henüz bağlı değil.
- **Exploration modu (§4) kodlanmadı** — CompanyMemory yalnızca exploitation-retrieval yapar.
- **DecisionGravity.Stake** dışarıdan verilir — Alternative-başına `ExpectedValue` (OL1) henüz
  Decision Object'te yok, `Stake = spread(ExpectedValue)` hesaplanamıyor.
- **BoundedAutonomyGate.ConformanceDeficit** Company Memory (ENS-2003) olmadan hesaplanamaz,
  çağıran katman 0 verirse `AttentionPriority` yanlış-negatif riski taşır (kod içi not).
- **BoundedAutonomyGate Policy** minimalist (iki eşik + isIrreversible bayrağı) — gerçek
  Policy/Constraint node'ları (ENS-4010) henüz bağlanmadı; bu modelin ilk çalışan yaklaşımı.
- **DecisionCapital Stok hesabı kasıtlı olarak yok** — ENS-3023 "stok=Memory, Capital=onun
  dinamiği" der; Company Memory (ENS-2003) henüz kodlanmadı, bu yüzden yalnızca akış
  (yatırım−amortisman) ve ROI kodlandı, `investment`/`amortization` toplamları dışarıdan verilir.
- **`intent: exploit|explore`** (OE1) ve **Alternative-başına ExpectedValue** (OL1) artık
  ENS-2001 v0.3'te teoride var (SKR-033 survives) ama `DecisionAggregate`'e henüz kodlanmadı —
  DecisionGravity.Stake/DecisionEntropy'nin intent-filtresi hâlâ dışarıdan/eksik.
- **ReflectiveDoubleLoop (§4a) yön-sapması değil BÜYÜKLÜK-TEKRARI tespit ediyor** — ENS-2004 §1
  gerçek sinyal `learning_signal = Actual − Expected` (işaretli) ister, ama `MemoryRecord.
  LearningMagnitude` işaretsiz (|Learning|) ve `DecisionEvents.cs`'teki OutcomeObserved/
  LearningRecorded sonuçları `string` (sayısal değil) — yönlü sistematik-sapma Faz-4'te yok,
  yalnızca aynı Purpose-tipinde tekrarlayan büyük büyüklük yakalanıyor. Ayrıca ENS-4025 L8
  proof-trace okunmuyor; girdi yalnızca CompanyMemory kayıtları (reflektif "neden"in zayıf yaklaşığı).
- **ReflectiveDoubleLoop eşikleri kalibre edilmedi** — `minSupportingRecords`/`magnitudeThreshold`
  ampirik değil (ENS-2003'ün `τ_π`'siyle aynı kalibrasyon borcu — v0.3'ün γ'sı v0.4.0'da
  kaldırıldı, bkz. D-5; ENS-2004/2003 §Failure); varsayılan
  (3, 5.0) yalnızca yapısal örnek. Alt sınır artık 2'dir (AUDIT §5.6: guard eskiden 1 kabul
  edip "tek gözlemden 'sistematik' iddia edilemez" diyordu — kozmetikti). **2 de kalibre bir
  eşik değil**, yalnızca kavramsal alt sınırdır: n=1 bir örnektir, bir örüntü değildir.
- **ReflectiveDoubleLoop öneri-yorgunluğu (P5) bağlanmadı** — `Propose` sınırsız sayıda öneri
  dönebilir; önceliklendirme (ör. Decision Gravity, ENS-3022) ya da eşik/batch limiti yok
  (ENS-2004 §Failure "öneri-yorgunluğu", bilinçli açık borç). **P7 kapısı ise yapısal:** sınıfın
  state değiştiren hiçbir metodu yok, yalnızca `Propose` (salt-okunur öneri döner) — bir
  refleksiyon-testi bunu doğruluyor.
- **LlmAdapter SAF PORT — hiçbir somut sağlayıcı bağlı değil.** `ILlmAdapter` arayüzü + registry +
  tier-seçici var; Cerebras/DeepInfra/OpenAI gibi gerçek bir implementasyon YOK (ağ, API-anahtarı,
  retry/timeout hiç ele alınmadı). Testlerdeki `EchoLlmAdapter` bir test-double, sağlayıcı değil.
- **LlmAdapter tier eşikleri (10/40) kalibre edilmedi** — ENS-2003 `τ_π`'si (v0.4.0; eski γ) ve
  ReflectiveDoubleLoop eşikleriyle aynı Faz-4 kalibrasyon borcu.
- **LlmAdapter proof-trace üretmiyor** — ADR-0001 §5.5 Proof-Trace Emitter hâlâ kodlanmadı; port
  yalnızca `ModelId`+token sayısını zarf olarak taşır (audit substratı), P6 trace'i atom düzeyinde
  (`DecisionAggregate`) kalır, driver'da değil.
- **Tier eşikleri KALİBRE DEĞİL** — ENS-3022 §Model 1 Stake'in Purpose-tipi içinde normalize
  edilmesini (z-skoru/persentil, ENS-2003 Memory'den) şart koşuyor; **o normalizasyon henüz
  kodlanmadı.** Bu gerçek bir borç.
  > **DÜZELTME (AUDIT.md §2):** Bu README daha önce bu borcu "demo somut olarak gösterdi:
  > varsayılan eşiklerle HER karar `Critical` çıkıyor" diye kanıtlıyordu. **O kanıt geçersizdi.**
  > Demo'nun sağ sütunu bir C# overload çözümleme tuzağının ürünüydü (`AUDIT_DEFECT_C1`):
  > `SelectTier(stake, conf)` pozisyonel çağrısı InfoNeed'i hiç hesaplamıyor, `conf`'u eşik
  > yerine koyuyordu. Doğrusu: `InfoNeed(500, 0.95) = 25` ve 10/40 eşikleriyle bu `Complex`'tir,
  > `Critical` değil. Borç **gerçek** (ENS-3022'de kayıtlı) ama **demo onu kanıtlamıyor**. Bir
  > teori borcunu bir kod hatasıyla "kanıtlamak", uydurulmuş veriden farklı ama aynı ciddiyette
  > bir epistemik kusurdur — Madde X gereği burada kayda geçirilmiştir.
- **CapabilityRegistry: sandbox ve dinamik yükleme YOK** — §6 "her Capability action'ı izole
  workspace'te koşar" der; izolasyon/prompt-injection sanitizasyonu/kaynak kotası kodlanmadı.
  Pack'ler kod içinde kaydedilir (Mosaik ModuleLoader'ın assembly-scan deseni uygulanmadı);
  progressive 3-tier context loading yok; marketplace yok (§6'nın kendi V1 sınırı).
- **ActuationLayer "guarded" YAPISAL, operasyonel değil** — gerçek sandbox (izole workspace,
  fs/ağ kısıtı, timeout, kill), rollback, kaynak kotası YOK. Kodlanan guard: Gate'siz Acting'e
  geçilemez, Blocked terminal, Failed bile Traced'ten geçmek zorunda (izsiz başarısızlık yasak).
  Ayrıca **çalıştırıcı değil** — action'ı fiilen koşan kod yok (Scheduler ile aynı sınır);
  AuditEvent akışı in-memory, `DecisionAggregate`'in event-sourcing'iyle henüz birleştirilmedi.
- **ProofTrace bir ZARF + invariant, otomatik üretici değil** — kernel bileşenleri (Scheduler,
  Gate, Adapter) henüz her karar noktasında otomatik trace emit etmiyor; bağlama ayrı adım.
  Öncüller serbest metin (ENS-4010 node'una tipli referans yok, Purpose-tipi string'liğiyle aynı
  borç). Yalnızca `min` t-norm (ENS-4025 §Failure: t-norm seçimi ileride RFC gerektirebilir).
  Trace'in kendisi event-sourced değil; §5.4'ün action lifecycle state-machine'i kodlanmadı.
- **Scheduler bir SIRALAYICI, çalıştırıcı değil** — gerçek iş kuyruğu, thread-pool, async dispatch,
  preemption yok; `Schedule` saf fonksiyon (bekleyenleri sıraya dizip tier+gate kararı iliştirir),
  yürütme çağıran katmanın işi. **Starvation önlenmiyor** (fairness/yaşlandırma mekanizması yok —
  açık tasarım borcu). `ConformanceDeficit` dışarıdan verilir: ENS-3022 onu Company Memory'nin
  peer-context fitinden ister, o fit henüz kodlanmadı → 0 verilirse öncelik 0'a düşer (yanlış-negatif).

## Çalıştırma
```bash
dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj
dotnet run --project Ens.Kernel.Demo/Ens.Kernel.Demo.csproj

# yalnızca bağımsız düşmanca denetim (AUDIT.md) — CI'da ayrı gate olarak koşturulmalı
dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj --filter "FullyQualifiedName~AdversarialAuditTests"
```

> **Denetim disiplini (AUDIT.md §7/6):** Demoyu ve testleri yazan bileşen kendi sınavını
> yazmamalı. `AdversarialAuditTests.cs` bunun ilk adımıdır ve **ayrı bir CI gate'i olarak**
> koşturulmalıdır. Faz-5'in kapı şartı budur; E3/H1'in gerektirdiği onay-/gate-token'ı da
> aynı kapıda ele alınmalıdır.
