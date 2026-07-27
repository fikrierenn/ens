# KUSUR SİCİLİ — Ens.Kernel açık kusurların tam envanteri

| | |
|---|---|
| **Tarih** | 2026-07-26 |
| **Derleyen** | oturum sahibi (owner), test adlarından mekanik türetme |
| **Kapsam** | `Ens.Kernel.Tests/` içindeki **tüm** `AUDIT_DEFECT_*` ve `AUDIT_FINDING_*` testleri |
| **Yetki** | Anayasa Madde X (Yanlışlanabilirlik Ödevi) — bulunan kusur gizlenemez |
| **Durum** | **75** açık kusur (`DEFECT`), **9** açık gözlem (`FINDING`) |
| **Doğrulama** | `DEFECT-REGISTER-VERIFICATION.md` — bağımsız denetim, yargı: **kısmen güvenilir** |

> ### ⚠️ v1'in sayım hatası — düzeltildi (2026-07-26)
> İlk sürüm **68 DEFECT / 8 FINDING** dedi. **Yanlıştı.** Gerçek sayı **75 / 9**.
> Üç ayrı kesme hatası üst üste bindi ve hepsi aynı dosyayı vurdu
> (`AdversarialWave_SecurityTests.cs`):
> 1. Dosya **UTF-16**; `rg`/`grep` onu *binary* sayıp sıfır sonuç döndürüyor.
> 2. `grep -a` ile aşıldı ama çıktı `head -40` ile kesildi — dosyada **48** metot var,
>    son 8'i (`W7h` + tüm `W8` grubu) düştü.
> 3. Sayım `public void` ile yapıldı; `W5d`/`W5e`/`W5f` **`public async Task`**.
>
> Eksik kalanlar: `W5d`, `W5e`, `W5f`, `W7h`, `W8a`, `W8b`, `W8d` (DEFECT) + `W8c` (FINDING).
>
> **Ders (T1):** envanter elle değil **komutla** üretilir. Kanonik komut:
> ```bash
> for f in Ens.Kernel.Tests/*.cs; do
>   grep -aoE "public (void|async Task) AUDIT_[A-Za-z0-9_]+" "$f" | tr -d '\000'
> done | sed -E 's/public (void|async Task) //' | sort -u
> ```
> 2026-07-26 çıktısı: **DEFECT 75 · FINDING 9 · FIXED 51 · HOLDS 66** (toplam 201 benzersiz
> metot). Bu sayı iki bağımsız yolla teyit edildi (bu komut + bağımsız denetim ajanı).

---

## 0. Bu sicilin ne olduğu ve NE OLMADIĞI

**Ne olduğu:** ENS'te bir test adı bir hüküm taşır:

| Ön ek | Anlamı | Test GEÇERSE |
|---|---|---|
| `AUDIT_DEFECT_*` | Kusur **hâlâ açık** | Kusur **var** — geçmesi kötü haberdir |
| `AUDIT_FIXED_*` | Kusur kapatıldı | Düzeltme tutuyor |
| `AUDIT_HOLDS_*` | İddia saldırıya dayandı | İddia ayakta |
| `AUDIT_FINDING_*` | Kusur değil, ama iddia zayıf | Gözlem doğru |

373/373 test geçiyor. Bu, **"68 kusur kanıtlanmış durumda"** demektir — "kusur yok" değil.
Yeşil test paneli burada sağlık değil, **envanter** anlamına gelir.

**Ne OLMADIĞI:** bu belge kusurların yeniden analizi değil. Test adlarından ve hangi raporun
onları kapsadığından türetilmiş bir **indeks**tir. Her satırın gerçek gövdesi, işaret edilen
test dosyası ve satır numarasındadır. Şiddet sütunu benim değerlendirmemdir, bağımsız
denetim ürünü değildir — bu ayrımı saklamıyorum.

**Bu sicilin var olma sebebi:** DENETİM raporlarının bir kısmı hiç yazılmadı. `SECURITY`
dalgasının ajanı raporunu yazamadan API stall ile öldü; **testleri diskte, bulguları hiçbir
raporda değil.** 68 kusurun 34'ü bugüne kadar yalnızca test adı olarak var oldu. Aşağıdaki
"Rapor" sütunundaki `—` işareti bunu gösterir.

---

## 1. Özet tablo

| Dalga / dosya | DEFECT | FINDING | Rapor |
|---|---|---|---|
| `AdversarialAuditTests.cs` (ilk bağımsız denetim) | 5 | 3 | `AUDIT.md` (kısmi) |
| `AdversarialWave_MemoryTests.cs` | 18 | 0 | **YOK** |
| `AdversarialWave_InvariantTests.cs` | 13 | 1 | `AUDIT-WAVE2-INVARIANTS.md` (kısmi) |
| `AdversarialWave_SchedulerGateTests.cs` | 11 | 0 | `AUDIT-WAVE2-SCHEDULER.md` (kısmi) |
| `AdversarialWave_SecurityTests.cs` | 19 | 4 | **YOK — ajan öldü** |
| `AuditFixed_CommitmentProofTraceTests.cs` | 2 | 0 | `AUDIT-WAVE2-FIDELITY.md` |
| **Toplam** | **68** | **8** | |

> ### ⚠️ Yukarıdaki tablo BAYAT (v1 sayıları) — v2 düzeltmeleri §1.5'te
> Satır toplamları 68/8'dir; gerçek 75/9. `…SecurityTests.cs` satırı 19/4 diyor, bağımsız
> sayım **26/5** buldu. Dosya-bazlı yeniden sayım yapılmadan bu tablo güncellenmiyor —
> **uydurulmuş bir düzeltme, kayıtlı bir hatadan kötüdür.**

### Şiddet dağılımı — **GERİ ÇEKİLDİ**

| Şiddet | ~~v1 iddiası~~ | Durum |
|---|---|---|
| **K (Kritik)** | ~~14~~ | Geçersiz |
| **Y (Yüksek)** | ~~27~~ | Geçersiz |
| **O (Orta)** | ~~21~~ | Geçersiz |
| **D (Düşük)** | ~~6~~ | Geçersiz |

Bağımsız denetim (`DEFECT-REGISTER-VERIFICATION.md`, T10) bu dağılımın **dört kovada da
yanlış** olduğunu gösterdi — o 68 için gerçek değerler 17/25/22/4'tü. Toplamı tutup
bileşenleri tutmayan bir tablo, **sayılmadığının kanıtıdır.**

Yeni bir dağılım **yazılmadı**, çünkü 75 kusurun tam şiddet analizi henüz yapılmadı ve
şiddet ataması bağımsız gözden geçirme ister (GOV-000 G4). Sayı uydurmaktansa boş bırakmak
tercih edildi.

---

## 1.5 v2 düzeltmeleri (bağımsız denetimler sonrası)

### (a) Sicilde HİÇ olmayan 8 bulgu — eklendi

| ID | Kusur | Dosya:satır |
|---|---|---|
| **W8d** | Tek bir öz-beyan `confidence = 1.0`, `stake = 1e12`'de bile InfoNeed'i, AttentionPriority'yi, tier'ı **ve** gate'i birlikte sıfırlıyor | `…SecurityTests.cs:958` |
| **W5d** | `Resolve` ilk `CanHandle=true` diyeni **kayıt sırasına** göre seçiyor → ADR-0001 §5.3'ün "yüksek InfoNeed → güçlü model" iddiası kodda karşılıksız | `…SecurityTests.cs:488` |
| **W5e** | `null` `LlmResponse` geçip proof-trace substratını yok ediyor | `…SecurityTests.cs:507` |
| **W5f** | Önceden iptal edilmiş token yine de "başarılı" yanıt veriyor | `…SecurityTests.cs:520` |
| **W7h** | Boş ve boşluklu context key'ler ayrı evrenler üretiyor | `…SecurityTests.cs:912` |
| **W8a** | `ReuseROI` iki ölçülebilir girdiden `+∞` dönüyor | `…SecurityTests.cs:931` |
| **W8b** | `DeltaCapital` sonlu girdilerden sonsuza taşıyor | `…SecurityTests.cs:940` |
| **W8c** *(FINDING)* | Doc yorumu "mutlak learning" diyor, kod negatif learning'i reddediyor | `…SecurityTests.cs:947` |

**W8d, kernel'in muhtemelen en ağır kusurudur** ve sicilde hiç yoktu. İki bağımsız denetim
de onu **Kritik** verdi; v1'in Orta ataması ("kabul edilmiş borç") reddedildi — gerekçe
sicilin kendi §0 ilkesi: *borcu ilan etmek dürüst yapar, hafif yapmaz.*

### (b) Şiddet itirazları — kabul edilenler

| ID | v1 | Düzeltilmiş | Gerekçe |
|---|---|---|---|
| **H1** | Kritik | **FINDING** | Gövde yalnız `CompanyMemory.Record`'un public olduğunu kanıtlıyor, "öneri uygulanabiliyor"u değil (`AdversarialAuditTests.cs:1020-1033`) |
| **W16** | Düşük | **Kritik** | `toolAuthorization: null` → Autonomous; `Scheduler.Schedule` imzasında registry yok. W4a'dan **daha ucuz** bir atlatma — sahte nesne bile gerekmiyor |
| **W2_R4** | Yüksek | **Kritik** | Testin kendi yorumu "EN AĞIR BULGU"; Individuation mührü **public API** ile deliniyor, reflection gerekmiyor |
| **W2d** | Düşük | **Yüksek** | `Reason`, P7 altında insana gösterilen tek bağlam; RTL-override onay istemini **tersine çevirir** |
| **W7f** | Orta | **Yüksek** | İzlenebilir *değil* — iki `double`, iz yok; sicilin kendi "Orta" tanımını karşılamıyor |
| **W3c** | Kritik | **Düşük (kod) / Yüksek (iddia)** | Testi yazan ajan `…SecurityTests.cs:328`'de harfiyen `// KUSUR (dusuk siddet, YUKSEK iddia-hatasi)` yazmış. Kalıcı kusur `CapabilityRegistry.cs:41`'in mutlak "gerçekten sabittir" iddiası |
| **W8** | Kritik | **Yüksek** | W9 ile aynı satır, aynı exception, aynı kök — biri K biri O olamaz |
| **W6d** | Orta | **Düşük** | `W6e` yönün güvenli olduğunu 200 örnekle kanıtladı |
| **W5f** | DEFECT | **FINDING** | Kanıt tümüyle `EchoLlmAdapter` test-double'ı hakkında; interface iptali zorlayamaz |

### (c) §2'nin ortak-özellik cümlesi — geri çekildi

*"Bu kusurların ortak özelliği: saldırgana özel yetki gerekmiyor"* cümlesi **7 üye için
yanlıştı**: `E5`/`W3c` reflection ister, `W2c` Pack tescil yetkisi ister, `W1a–W1d` saldırgan
değil **operatör hatası**dır. Ölçüt uygulanırsa bu yedisi §2'den çıkar; `W16`, `W2_R4`,
`W2_R2`, `W8d` girer.

### (d) 7. ve 8. kalıp — §7'ye eklendi

| # | Kalıp | Üyeler | Karar |
|---|---|---|---|
| **7** | **"Girdi kapısı var, ÇIKTI kapısı yok"** — adı benim değil, kaynakta yazılı (`…SecurityTests.cs:927`) | W8a, W8b, W17, H4, W5e, W3 | Çıktı postcondition kapısı / `Measured<double>` |
| **8** | **"Öz-beyan kalibre edilmemiş"** | W8d, W7, W7d, B2, B1, D1_residual, G5, W2_P4 | Confidence provenance/kalibrasyon portu (ENS-3022 borcu) |

### (e) §7'nin yanlışlanabilirliği — kabul edilen kusur

Bağımsız denetim §7'yi **yanlışlanamaz** buldu ve haklıydı: hangi kusurun hangi kalıba ait
olduğu **hiç listelenmemişti**, dolayısıyla "33 kapanır" ne doğrulanabilir ne çürütülebilirdi.
Bu, sicilin kendi yetki kaynağı olan Madde X'in ihlalidir — **kendi kuralımı çiğnedim.**

Düzeltilmiş sayılar: kalıp 4 → 4 (W10 çıkar), kalıp 5 → 2 (W5a/W5b downcast'tir, testleri
"REFLECTION GEREKMEZ" diyor), kalıp 2 → ≥11 (W1a–W1d + W7h girer). **Kalıp 5 "kapanan"
hanesinden tümüyle çıkarıldı** — §8.3'ün kendi ifadesiyle o bir *kapsam kararıdır*, kapsam
dışına almak kapatmak değildir.

**Sonuç: "33/68 kapanır" → "~29-31/75".** Üyelik listeleri yazılmadan bu sayı da
yanlışlanamaz kalır; listeler §7'ye yazılana kadar **iddia olarak değil, tahmin olarak**
okunmalıdır.

---

## 2. KRİTİK — anayasal güvenceyi tek satırda düşürenler

Bu 14 kusurun ortak özelliği: **saldırgana özel yetki gerekmiyor.** Public API'yi normal
şekilde çağırarak bir P6 (izlenebilirlik) veya P7 (sınırlı özerklik) güvencesi düşürülüyor.

| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **E3** | Gate sonucu tek satırda taklit edilebilir (`GateResult` public record) | `AdversarialAuditTests.cs:539` | `AUDIT-WAVE2-INVARIANTS.md` |
| **W4a** | Sahte `ToolAuthorization` Capability Registry'yi tamamen atlar | `…SecurityTests.cs` | **—** |
| **W15** | `ToolAuthorization` public olduğu için registry reddi aklanabiliyor | `…SchedulerGateTests.cs:386` | `…SCHEDULER.md` |
| **H1** | Bir öneri (`proposal`) herhangi bir çağıran tarafından 3 satırda otomatik uygulanabiliyor | `AdversarialAuditTests.cs:1010` | **—** |
| **E5** | Durum, reflection ile ışınlanabiliyor — **hiç iz bırakmadan** | `AdversarialAuditTests.cs:581` | `…INVARIANTS.md` |
| **W3c** | Reflection donmuş (`FrozenSet`) izin kümesini hâlâ değiştiriyor; registry yeniden doğrulamıyor | `…SecurityTests.cs` | **—** |
| **W2_P1** | Tamamen görünmez bir proof-trace temsil edilebilir (L8 ihlali) | `…InvariantTests.cs:367` | `…INVARIANTS.md` |
| **W2_P4** | Kendi kendini gerekçelendiren türetim, tam güvenle temsil edilebilir (döngüsel ispat) | `…InvariantTests.cs:474` | `…INVARIANTS.md` |
| **W2_L1** | Eylem yaşam döngüsü **hiçbir karar olmadan** başlatılabiliyor | `…InvariantTests.cs:545` | `…INVARIANTS.md` |
| **W2_L2** | Tek bir proof-trace sınırsız sayıda **alakasız** eylemi meşrulaştırıyor | `…InvariantTests.cs:579` | `…INVARIANTS.md` |
| **W2_O1** | Tek-sahip (single owner) koşulu **hiç implemente edilmemiş** | `…InvariantTests.cs:320` | `…INVARIANTS.md` |
| **W8** | Scheduler, gate'in kendi fail-closed `CriticalBlock` dalını devre dışı bırakıyor | `…SchedulerGateTests.cs:195` | **—** |
| **W1a–W1d** | `Disable` yanlış harf/biçimle sessizce "başarılı" oluyor; yetenek canlı kalıyor — üstelik doğrulama sorgusu da operatörün yanlış inancını onaylıyor | `…SecurityTests.cs` | **—** |
| **W2c** | Homoglyph araç adı, korunan bir aracın **onay gerektirmeyen ikizini** üretiyor | `…SecurityTests.cs` | **—** |

> **W1b özellikle tehlikeli:** operatör yeteneği kapattığını sanıyor, doğrulama sorgusu
> "kapalı" diyor, yetenek çalışmaya devam ediyor. Yanlış güvenlik hissini sistem kendisi
> üretiyor. Bu, kusurun kendisinden daha kötü.

---

## 3. YÜKSEK — sessiz kapatma anahtarları ve iz kaybı

### 3.1 Sessiz global "kapat" anahtarları
Ortak kalıp: bir eşiğe `0` veya negatif değer vermek, tüm şirket çapında bir yönetişim
mekanizmasını **hata vermeden** kapatıyor. Hiçbiri belgede yazılı değil.

| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **A5** | `contextDecayRate = 0` → şirket çapında sönümü kapatır | `…MemoryTests.cs:147` | **—** |
| **E4** | `staleThreshold = 0` → curator'ı sessizce kapatır | `…MemoryTests.cs:545` | `AUDIT.md`, `…INVARIANTS.md` |
| **G2** | Sıfır magnitude eşiği → **her** purpose type bir öneriye dönüşür | `…MemoryTests.cs:720` | **—** |
| **H3** | Negatif eşik → gate sessizce no-op olur | `…MemoryTests.cs:889` | **—** |
| **W10** | Bozuk policy eşikleri yalnızca **bazı** girdilerde doğrulanıyor | `…SchedulerGateTests.cs:242` | **—** |

### 3.2 Zaman manipülasyonu — ölümsüz kayıtlar
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **A1** | Gelecek tarihli `AssertedAt` sönümü **sonsuza dek** kapatır; kanıt yok, iz yok | `…MemoryTests.cs:56` | **—** |
| **A2** | `DateTime.MaxValue` `AssertedAt` kabul ediliyor → kayıt ölümsüz | `…MemoryTests.cs:79` | `AUDIT.md` |
| **D4** | `Retrieve`, `asOf` anında **henüz var olmayan** kayıtları sızdırıyor ve onları **ilk sıraya** koyuyor | `…MemoryTests.cs:403` | **—** |
| **W2_L3** | Denetim zaman damgaları çağıran kontrolünde, hiç doğrulanmıyor | `…InvariantTests.cs:603` | `…INVARIANTS.md` |
| **W2_R6** | Replay zaman damgalarını ve yinelenen event id'leri yok sayıyor | `…InvariantTests.cs:281` | `…INVARIANTS.md` |

> **D4 ayrıca bir bilgi sızıntısıdır:** "geçmişte şu an ne biliyorduk" sorgusu, geleceği
> görüyor. Bu, geriye dönük her öğrenme analizini (ENS-2004) geçersiz kılar.

### 3.3 Kozmetik doğrulama — var gibi görünen, olmayan güvenceler
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **B1** | Zorunlu kanıt güvencesi, **kendi varsayılan parametresiyle** etkisiz kılınıyor | `…MemoryTests.cs:193` | **—** |
| **B2** | Boşluk olmayan **herhangi bir karakter** kanıt sayılıyor (`"x"` geçerli kanıt) | `…MemoryTests.cs:217` | **—** |
| **B4** | Aynı kayıt aynı anda 1000 kez doğrulanabiliyor | `…MemoryTests.cs:250` | **—** |
| **D1a** | Öncüller hâlâ kalibre edilmemiş serbest metin | `AuditFixed_…Tests.cs:279` | `…FIDELITY.md` |
| **D1b** | Yalnızca commitment atomu iz yayıyor — diğer geçişler izsiz | `AuditFixed_…Tests.cs:303` | `…FIDELITY.md` |
| **W2_P3** | Render çıktısı, öncül metni üzerinden taklit edilebiliyor | `…InvariantTests.cs:439` | `…INVARIANTS.md` |
| **G5** | Öneriler, **sıfır provenance**'lı sahte kayıtlardan imal edilebiliyor | `…MemoryTests.cs:772` | `AUDIT.md` |

### 3.4 Event sourcing / replay bütünlüğü
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **W2_R2** | `Rehydrate` **başka kararlara ait** event'leri kabul ediyor | `…InvariantTests.cs:144` | `…INVARIANTS.md` |
| **W2_R3** | Replay, boş-olmayan alternatif güvencesini uygulamıyor | `…InvariantTests.cs:178` | `…INVARIANTS.md` |
| **W2_R5** | Bilinmeyen event tipleri fold içinde **sessizce yutuluyor** | `…InvariantTests.cs:257` | `…INVARIANTS.md` |

> **W2_R5 + W2_R2 birlikte:** başka bir karara ait event'i akışa sok, bilinmeyen tipleri de
> sessizce yutulsun — replay, gerçekte hiç olmamış bir karar geçmişi üretebilir. Event
> sourcing'in tek güvencesi (geçmiş yeniden üretilebilir) bu ikili ile düşüyor.

### 3.5 Canlı koleksiyon sızıntıları (immutability ihlalleri)
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **W22** | Scheduler çıktısı canlı `List` — çağıran sırayı değiştirebiliyor | `…SchedulerGateTests.cs:584` | `…SCHEDULER.md` |
| **W2_R4** | Replay edilen alternatifler kopya değil, **canlı görünüm** | `…InvariantTests.cs:218` | `…INVARIANTS.md` |
| **W2_L4** | `history` senkronize edilmemiş canlı görünüm, snapshot değil | `…InvariantTests.cs:623` | `…INVARIANTS.md` |
| **W5a** | Adapter listesi downcast edilebilir `List` — yönlendirme tek satırda ele geçirilir | `…SecurityTests.cs` | **—** |
| **W5b** | "En az bir adapter" değişmezi, inşadan **sonra** silinebiliyor | `…SecurityTests.cs` | **—** |

---

## 4. ORTA — yanlış sonuç, ama izlenebilir

### 4.1 Kimlik/normalizasyon — belleği parçalayan kusurlar
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **F3** | Unicode-eşdeğer purpose type'lar belleği **erişilemez parçalara** bölüyor | `…MemoryTests.cs:647` | `…SCHEDULER.md` |
| **G3** | Purpose type'a boşluk ekleyerek sistematik örüntü tespiti tamamen atlatılıyor | `…MemoryTests.cs:739` | `AUDIT.md` |
| **G4** | Bir karar sınıfının büyük/küçük harf varyantları iki ayrı öneri üretiyor | `…MemoryTests.cs:757` | `AUDIT.md` |
| **C2** | Saat kayıt **kimliğine** değil **değerine** göre anahtarlanıyor — klonlar birbirini kirletiyor | `…MemoryTests.cs:292` | `…INVARIANTS.md` |
| **C3** | Hayalet-kayıt güvencesi, değer-eşit klon üreterek atlatılıyor | `…MemoryTests.cs:315` | `…INVARIANTS.md` |
| **W7f** | Sahip kimliğinde büyük/küçük harf farkı **tüm** level/pattern attribution'ını ters çeviriyor | `…SecurityTests.cs` | **—** |
| **W2e** | `NUL` içeren araç adları birinci sınıf yetkili yetenek oluyor | `…SecurityTests.cs` | **—** |
| **W2f** | `Register`, `Authorize`'ın **hiç sorgulayamayacağı** araç adlarını kabul ediyor | `…SecurityTests.cs` | **—** |
| **W5g** | Yinelenen `AdapterId` kabul ediliyor → denetim anahtarı injective değil | `…SecurityTests.cs` | **—** |

### 4.2 Sayısal sınır ve belirlenimsizlik
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **W17** | Gate, korunan sınırın **dışına** NaN `InfoNeed` yayıyor | `…SchedulerGateTests.cs:434` | **—** |
| **H4** | `Compute` negatif sonsuz — yani ölçülemez — skor yayabiliyor | `…MemoryTests.cs:906` | **—** |
| **W7** | `confidence == 1.0` sınırsız özerklik satın alıyor; 1 ULP altı almıyor | `…SchedulerGateTests.cs:175` | `…SCHEDULER.md` |
| **W3** | `NormalizedDeficit` clamp'i negatif sıfırı normalize etmiyor | `…SchedulerGateTests.cs:97` | `…SCHEDULER.md` |
| **B6 / W20** | Öncelik ve InfoNeed berabere kaldığında sıralama **girdi sırasına** bağımlı | `AdversarialAuditTests.cs:292`, `…SchedulerGateTests.cs:544` | kısmi |
| **C1 / W6d** | Konumsal (positional) çağrı sessizce **yanlış overload'a** bağlanıyor | `AdversarialAuditTests.cs:318`, `…SecurityTests.cs` | `…INVARIANTS.md` |
| **W7d** | Tek gözlem, örneklem büyüklüğü güvencesi olmadan "sıfır gürültü" raporluyor | `…SecurityTests.cs` | **—** |
| **W7e** | Tamamen `null` gözlemler kabul ediliyor ve "kusursuz tutarlılık" raporlanıyor | `…SecurityTests.cs` | **—** |

> **C1/W6d, bu projenin en öğretici kusurudur.** Kernel demosunun kendi "dürüst
> bulgusu" bu hataya kurban gitmişti: `SelectTier(stake, conf)` C#'ın "better conversion
> target" kuralıyla yanlış overload'a bağlandı, InfoNeed hiç hesaplanmadı. Demo, olmayan bir
> şeyi eleştirdiği için kendini dürüst sandı. **Öz-eleştiri de doğrulanmadan kanıt değildir.**

### 4.3 Ölçekleme
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **G1** | 10.000 kayıt, 5.000 birbirinden ayrışmamış öneri üretiyor | `…MemoryTests.cs:692` | **—** |
| **W9** | Tek bir zehirli karar, **tüm partinin** dikkatini engelliyor | `…SchedulerGateTests.cs:216` | `…SCHEDULER.md` |
| **W1e** | `disabled` kümesi, doğrulanmamış public girdiden sınırsız büyüyor | `…SecurityTests.cs` | **—** |

---

## 5. DÜŞÜK
| ID | Kusur | Dosya:satır | Rapor |
|---|---|---|---|
| **W16** | `null` `toolAuthorization`, "yetkilendirmeyi unutmak"tan ayırt edilemiyor | `…SchedulerGateTests.cs:413` | **—** |
| **W23** | Geçersiz `PendingDecision` inşa edilebiliyor; `null` eleman NRE fırlatıyor | `…SchedulerGateTests.cs:600` | `…SCHEDULER.md` |
| **W2d** | Kontrol karakterleri, insana gösterilen `reason` metnine sanitize edilmeden akıyor | `…SecurityTests.cs` | **—** |
| **W5c** | `null` adapter, "model-agnostik ama modelsiz değil" güvencesini geçiyor | `…SecurityTests.cs` | **—** |

---

## 6. FINDING'ler — kusur değil, ama iddia zayıf

Bunlar kod hatası değildir. **Yanlış olan, kodun ne kanıtladığına dair iddiadır.** ENS'te
bu daha ciddidir: Madde VI, yanlışlanamaz/doğrulanmamış iddiayı **reddeder**; Madde X ise
böyle bir iddia taşıyan yapıtı **eksik** sayar (Madde X yasak koymaz, ödev yükler).

| ID | Gözlem | Neden önemli |
|---|---|---|
| **W7c** | Zincir kuralı **inşa gereği doğru**, dolayısıyla hiçbir şeyi yanlışlamıyor | ROADMAP "zincir kuralı matematiksel olarak doğrulandı" diyor. Bu **yanlıştır**: `LevelNoise` bir artık (residual) olarak hesaplanıyor, kendi tanımından değil. Toplam elbette tutuyor — çünkü öyle tanımlandı. Tautoloji, kanıt değil. |
| **W7b** | `LevelNoise`'daki `max(0, ...)` clamp'i ölü kod | Aynı kökten: artık olarak hesaplanan bir değer zaten negatif olamaz |
| **W7g** | Koşullu entropi normalize edilmemiş → alanlar arası karşılaştırma anlamsız | ENS-3021'in pratik kullanımını sınırlar |
| **W2_P5** | `min` t-norm idempotent olduğu için zincir uzunluğu güveni **hiç** düşürmüyor | 40 adımlık bir çıkarım, 1 adımlıkla aynı güvene sahip. Sezgiye aykırı; ENS-4025 L7 bunu tartışmıyor |
| **W4b** | Yetkisiz araç ile "yalnızca riskli" karar **aynı** gate durumuna düşüyor | Operatör ikisini ayırt edemiyor |
| **B5** | Demo girdileri `ConformanceDeficit`'i hiç göstermiyor | Demo, tanıtmayı iddia ettiği mekanizmayı tanıtmıyor |
| **D5** | Trace değişmezi yalnızca **kardinalite** hakkında, kanıt hakkında değil | "İzsiz çıkarım imkânsız" iddiası, "en az bir öncül var" ile sınırlı |
| **G8** | Demo bellek sıralaması karışık (confounded) — yaş izole edilmemiş | Sıralamanın sebebi gösterilmiyor |

> **W7c, bu projede bulunan en değerli FINDING'dir.** Bir sayı, kendi tanımından değil
> artık olarak hesaplanıyorsa, o sayıyı içeren özdeşliği doğrulamak hiçbir şey doğrulamaz.
> ROADMAP'teki "matematiksel olarak doğrulandı" ifadesi düzeltilmelidir.

---

## 7. Kök nedenler — 68 kusur, 6 kalıp

Kusurlar bağımsız değil. Altı tekrarlayan kalıp var; her biri **mimari** bir eksiklikten
doğuyor, tek tek yama işi değil:

| # | Kalıp | Kaç kusur | Kök neden | Gereken karar |
|---|---|---|---|---|
| 1 | **Public record = taklit edilebilir yetki** | 6 | Gate/authorization sonuçları düz veri; imzalı değil | **İmzalı gate-token** — ADR-0001 bu kararı vermedi |
| 2 | **Kimlik normalizasyonu yok** | 9 | Purpose type/tool name/owner id serbest string | **Normalize edilmiş kimlik tipi** (case, Unicode NFC, trim) |
| 3 | **Zaman çağırandan geliyor** | 5 | `DateTime` parametre olarak alınıyor, doğrulanmıyor | **Monoton saat portu** + gelecek-tarih reddi |
| 4 | **Eşik `0` = sessiz kapatma** | 5 | `0` hem "kapalı" hem geçerli değer | **Nullable eşik** + açık `Disabled` durumu |
| 5 | **Reflection tüm değişmezleri deler** | 3 | `FrozenSet` yalnızca API düzeyinde koruyor | Kabul edilebilir mi? **Açıkça karar verilmeli**, sessiz bırakılmamalı |
| 6 | **Canlı koleksiyon dönüyor** | 5 | `List` döndürülüyor, snapshot alınmıyor | Dönüş tiplerinde zorunlu `ToImmutable*` |

**Bu tabloyu okuma biçimi:** 68 kusuru tek tek kapatmak yanlış strateji. 6 mimari karar
alınırsa 33'ü birden kapanır. Kalanı gerçekten tekil işlerdir.

---

## 8. Bilerek açık bırakılanlar

Beş kusur, **kapatılmadığı için değil, ADR-0001'in henüz bir karar vermediği için** açık:

1. **İmzalı gate-token** (E3, W4a, W15, H1) — gate sonucunun taklit edilemez olması için
   kriptografik bir imza mı, yoksa iç `internal` tip mi? Mimari karar.
2. **İnsan onayı birinci sınıf tip mi?** (W4b) — "onay gerekiyor" bugün bir bayrak; ayrı bir
   durum olmalı mı?
3. **Sandbox izolasyonu** (E5, W3c) — reflection saldırısı kernel sınırı içinde mi
   savunulacak, yoksa process sınırında mı? İkincisi ise bu kusurlar kapsam dışıdır ve
   **öyle yazılmalıdır**.
4. **Üçüncü tie-breaker** (B6, W20) — beraberlik girdi sırasına düşüyor. Üçüncü ölçüt ne
   olmalı? Keyfi seçim teoriye aykırı; ENS-3022'den türetilmeli.
5. **Overload yeniden adlandırma** (C1, W6d) — `SelectTier` overload'ları ayrı adlar almalı.
   Breaking change; sürüm kararı gerekiyor.

---

## 9. Dürüstlük notu — bu sicilin kendi sınırları

- **Şiddet sütunu bağımsız değil.** Ben atadım; denetleyen ajanlar atamadı. G2/G3 gereği
  bu sınıflandırma bağımsız bir context tarafından gözden geçirilmelidir.
- **SECURITY dalgasının 23 test bulgusu hiç raporlanmadı.** Yukarıdaki satırlar test
  adlarından türetildi; ajanın gerekçesi ve saldırı senaryosu **kayıp**. Testler duruyor,
  akıl yürütme durmuyor. Bu dalganın raporu yeniden yazılmalıdır.
- **Kusur sayısı bir üst sınır değil.** 68, *bulunmuş* kusur sayısıdır. Bulunmamış olanlar
  hakkında bu belge hiçbir şey söylemez.
- **Hiçbir kusur, hiçbir aşamada gizlenmedi.** Testler kusuru kanıtlar durumda commit
  edildi, adlarıyla ilan edildi. Bu belge onları bulunabilir kıldı, ortaya çıkarmadı.

---

## 10. Kaynaklar

- Testler: `Ens.Kernel.Tests/` — 373 test, 373 geçiyor (owner tarafından çalıştırıldı,
  2026-07-26)
- Mevcut raporlar: `AUDIT.md`, `AUDIT-WAVE2-FIDELITY.md`, `AUDIT-WAVE2-INVARIANTS.md`,
  `AUDIT-WAVE2-SCHEDULER.md`
- **Eksik rapor:** `AUDIT-WAVE2-SECURITY.md` (yazılmadı)
- Saldırı yöntemi: `.claude/skills/adversarial-test/SKILL.md`
- Yetki: Anayasa Madde X (Yanlışlanabilirlik Ödevi), Madde VIII (İzlenebilirlik Yasası),
  Madde VI (Anti-Pattern'ler — black-box çıktı)

---

## 13. ⛔ T9 KAPANDI — ID uzayı tekil değil, ve **kapanış testi bu yüzden bozuk**

**Tarih:** 2026-07-27 · **Bulan:** `ENG-0003` (6 vaka) · **Ölçen:** oturum sahibi (14 vaka)

### Bulgu

Bu sicilin ve ona dayanan **her** ADR'nin tek yanlışlanma yordamı §11/3'tür:

> *"Kararlar uygulanınca ilgili `AUDIT_DEFECT_*` testleri `AUDIT_FIXED_*`'a dönmelidir."*

**Bu yordam bugün 14 kimlik için sahte pozitif üretiyor.** Çünkü o 14 kimliğin **hem**
`AUDIT_DEFECT_<ID>_*` **hem** `AUDIT_FIXED_<ID>_*` testi var — ve ikisi **farklı kusurları**
doğruluyor:

```
AUDIT_FIXED_A1_NaN_stake_is_rejected_instead_of_granting_full_autonomy   ← gate'te NaN stake
AUDIT_DEFECT_A1_Future_AssertedAt_disables_decay_forever_with_no_evidence ← memory'de zaman
```

Ad üzerinden yapılan her denetim bu 14'ü **"kapandı"** sayar. 373/373 geçtiği için
**ikisi de yeşildir** — yani sistem aynı anda *"kusur açık"* ve *"kusur kapandı"* diyor.

### Kök neden — mekanik ve tam

| Yön | Adet | Kimlikler |
|---|---|---|
| `DEFECT` = `AdversarialWave_MemoryTests` · `FIXED` = `AdversarialAuditTests` | **11** | `A1` `A2` `B4` `D4` `E4` `F3` `G1` `G2` `G3` `G4` `G5` |
| Ters yön | 2 | `C1` `E5` |
| Aynı dosya (§10'da zaten kayıtlı `D1` bölünmesi) | 1 | `D1` |

> **İki denetim dalgası, birbirinden habersiz, aynı `A1/A2/B4/G1…` harf numaralamasını
> kullandı.** Kimse çakışmayı görmedi çünkü **ID uzayı hiçbir yerde tanımlı değildi.**

Ayrıca **31 kimlik** birden fazla verdict taşıyor (`DEFECT`+`HOLDS` gibi kombinasyonlar dâhil);
bunların çoğu meşrudur (aynı alanda bir iddia ayakta, bir kusur açık olabilir).
**Tehlikeli olan yalnız `DEFECT`+`FIXED` çiftidir** — çünkü kapanış sinyali odur.

### Önerilen düzeltme — dalga ad-uzayı

Her ID, üretildiği dalganın ön ekini taşır:

| Dosya | Ön ek |
|---|---|
| `AdversarialAuditTests` | `AUD-` |
| `AdversarialWave_MemoryTests` | `MEM-` |
| `AdversarialWave_InvariantTests` | `INV-` |
| `AdversarialWave_SchedulerGateTests` | `SCH-` |
| `AdversarialWave_SecurityTests` | `SEC-` |
| `AuditFixed_CommitmentProofTraceTests` | `CPT-` |

`MEM-A1` ≠ `AUD-A1`. Ad-uzayı **kendini belgeler** ve çakışma **yapısal olarak** imkânsız hâle
gelir.

> **Bu ADR/sicil bunu tek başına UYGULAMIYOR.** 200+ test metodunun yeniden adlandırılması
> ayrı bir edimdir ve kendi turunu ister. Burada **kayıt** ve **öneri** var.

### Bu bulgunun kapsamı — dürüstçe

**Her kapanış iddiası ID'ler ayrıştırılana kadar sınanamaz.** Bu şunları etkiler:

- `ADR-0003` v0.7.0'ın **17** iddiası — 17'nin kaçının çakıştığı **sayılmadı**
- `ADR-0004`'ün `DP4` (5) ve `DP1` iddiaları
- `DEFECT-PATTERN-MAP` §11/3'ün **kendisi**

> **Kayıtlı bir kusur (§2.6, `SKR-049`), üzerine kurulan yordamı sessizce bozdu.**
> Kusur biliniyordu; **sonucu** bilinmiyordu. Bu, "bilinen borç" ile "ölçülmüş etki"
> arasındaki farkın maliyetidir.

### Yanlışlanma

Bu bölüm **yanlıştır** eğer: `AUDIT_FIXED_<ID>` ile `AUDIT_DEFECT_<ID>` çiftlerinden herhangi
biri gerçekten **aynı** kusuru konu ediyorsa (yani ID paylaşımı meşruysa). Ölçüm komutu:

```bash
for f in Ens.Kernel.Tests/*.cs; do
  grep -aoE "AUDIT_(DEFECT|FIXED)_[A-Za-z0-9]+_[A-Za-z0-9_]+" "$f" | tr -d [octal-NUL]      # <- gercek kacis dizisi ASCII-guvenli yazilmali
done | sort -u
```

### 13.1 Bu bölüm yazılırken tuzak **altıncı kez** tetiklendi — ve hedefi bu dosyaydı

Yukarıdaki komut örneği bir kaçış dizisi içeriyordu ve yazım sırasında o dizi **gerçek bir
NUL baytına** dönüştü. Sonuç:

- `DEFECT-REGISTER.md` `file` komutuna göre **`data`** oldu (bir markdown dosyası).
- `grep` onu **binary** saydı.
- Ve `inventory-check.sh` — **bu tuzağı yakalamak için yazılmış hook** — sicilin sayısını
  okuyamayıp `DEFECT 7000` diye **saçma bir değer** raporladı, yani **yanlış alarm** verdi.

> **Özyineleme:** NUL tuzağını belgeleyen bölüm, kendi dosyasına bir NUL yazdı ve tuzağı
> izleyen aracı bozdu.

**Alınan ders — kurala eklenecek:** kaçış dizileri (` `, ` `, ` `) belge
metnine **yazılmaz**; adıyla anılır. Bir belgeyi bozan şey, o belgenin konusu olabilir.

**Bir ders daha — hook'un kendi kusuru:** araç okuyamadığında **sessizce geçmedi**, ama
*"7000"* gibi anlamsız bir sayı üretip **yanlış alarm** verdi. Doğru davranış
*"dosya okunamadı"* demekti. `inventory-check.sh` bu ayrımı yapmıyor — açık kusur.
