# DEFECT-REGISTER DOĞRULAMA — bağımsız saldırı

| | |
|---|---|
| **Tarih** | 2026-07-26 |
| **Denetleyen** | `ens-skeptic` (bağımsız context; sicilin yazarı değil) |
| **Hedef** | `7000-reference-implementation/DEFECT-REGISTER.md` |
| **Yetki** | Anayasa Madde X (Yanlışlanabilirlik), G2/G3 (yazar kendi işini doğrulayamaz) |
| **Kapatılan borç** | DEFECT-REGISTER §9, madde 1 ("şiddet sütunu bağımsız değil") |
| **Verdict** | **`wounded` — KISMEN GÜVENİLİR** (§1, §7) |
| **Okunan test gövdesi** | 26 (hedef: ≥15) |
| **Test koşusu** | **YAPILMADI** — bkz. §0 dürüstlük beyanı |

---

## 0. Yöntem ve dürüstlük beyanı

**Ne yaptım:**
- 6 test dosyasındaki `AUDIT_DEFECT_*` / `AUDIT_FINDING_*` testlerinin **tam envanterini**
  `rg` ile çıkardım (`AdversarialWave_SecurityTests.cs` UTF-16 olduğu için `Read` ile
  elle sayıldı).
- **23 test gövdesini** satır satır okudum (hedef: en az 15). Öncelik §2'nin 14 satırı.
- İddiaların dayandığı üretim kodunu (`DecisionEntropy.cs`) doğrudan okudum.
- Şiddet atamalarını, testlerin **kendi yazdıkları şiddet değerlendirmesiyle** karşılaştırdım
  — bu, sicilin yazarının erişmediği bağımsız bir kanıt kaynağıdır.

**Ne YAPMADIM (dürüstlük beyanı):**
- **`dotnet test` çalıştırmadım.** Bu context'te test koşusu yapmadım; dolayısıyla
  "373/373 geçiyor" iddiasını **doğrulamadım ve yalanlamadım**. Aşağıdaki hiçbir bulgu
  bir test koşusu sonucuna dayanmıyor; hepsi **kaynak okuması**dır. (SKR-041 emsali:
  koşulmamış testin sonucu uydurulmaz.)
- Kusurların kendisini yeniden üretmedim; testin gövdesinin **adının söylediğini
  kanıtlayıp kanıtlamadığını** denetledim. Bu, sicilin iddia ettiği doğrulama düzeyidir.

---

## 1. NİHAİ YARGI (özet — gerekçe §7'de)

> ## **KISMEN GÜVENİLİR** (`wounded`)

Sicil kusur **gizlemiyor** ve listelediği 68 kusurun ID/dosya/satır bilgisi büyük ölçüde
doğru. Ama üç yerden kırıldı:

1. **Eksik envanter:** 68 değil **75** açık kusur var. `AdversarialWave_SecurityTests.cs`
   (tek UTF-16 dosya) içinden **7 DEFECT + 1 FINDING** sicilde hiç geçmiyor — aralarında
   kernel'in muhtemelen **en ağır** kusuru (`W8d`) var.
2. **Kalibre edilmemiş şiddet:** §2'nin kendi ölçütü 7 üye için geçersiz; 5 kusur fazla
   düşük, 3 kusur fazla yüksek atanmış; `W3c`'nin şiddeti, testi yazan ajanın kendi yazılı
   değerlendirmesiyle **çelişiyor**.
3. **§7 yanlışlanamaz:** kalıp üyelik listesi hiç verilmemiş (Madde X ödevi karşılanmamış —
   yanlışlanabilirlik koşulu belirtilmemiş; ayrıca Madde VI "yanlışlanamaz iddia"); iki kalıp
   fazla sayılmış, biri eksik; "33 kapanır" ~29-31'e iniyor; ve kaynakta **adı geçen**
   bir 7. kalıp ile bir 8. kalıp atlanmış.

**Kullanım kararı:** bulunabilirlik indeksi olarak ✅ kullanılabilir; önceliklendirme
temeli olarak ❌ kullanılamaz (T1–T4 kapanana kadar).

---

## 2. SALDIRI 1 — Ad ile gövde örtüşüyor mu?

### 2.1 Envanter denetimi — sicil eksiksiz mi? (ÖNCE BU)

Sicilin en temel iddiası, testlerin **tam** bir indeksi olduğudur. Bunu makine sayımıyla
denetledim.

| Dosya | Sicilin dediği DEFECT | Saydığım `AUDIT_DEFECT_*` | Sonuç |
|---|---|---|---|
| `AdversarialAuditTests.cs` | 5 | 5 (B6, C1, E3, E5, H1) | ✅ |
| `AdversarialWave_MemoryTests.cs` | 18 | 18 (A1,A2,A5,B1,B2,B4,C2,C3,D4,E4,F3,G1,G2,G3,G4,G5,H3,H4) | ✅ |
| `AdversarialWave_InvariantTests.cs` | 13 | 13 (R2–R6, O1, P1, P3, P4, L1–L4) | ✅ |
| `AdversarialWave_SchedulerGateTests.cs` | 11 | 11 (W3,W7,W8,W9,W10,W15,W16,W17,W20,W22,W23) | ✅ |
| `AdversarialWave_SecurityTests.cs` | 19 | **26** | ❌ **7 EKSİK** |
| `AuditFixed_CommitmentProofTraceTests.cs` | 2 | 2 | ✅ (ID'ler yanlış, bkz. aşağı) |
| **Toplam** | **68** | **75** | ❌ |

Beş dosyada sayım **tam tutuyor** — sicil bu dosyalar için güvenilir. Ama:

#### 🔴 BULGU-1 (en ağır): sicilde HİÇ GEÇMEYEN 8 test var

`AdversarialWave_SecurityTests.cs` içinde, sicilin hiçbir bölümünde adı geçmeyen
**7 `AUDIT_DEFECT_*` + 1 `AUDIT_FINDING_*`** testi var:

| ID | Test | Satır | Ne kanıtlıyor |
|---|---|---|---|
| **W5d** | `…CanHandle_is_self_declared_so_registration_order_alone_decides_the_model` | 488 | Yalan söyleyen adapter Critical kararları kendine çekiyor; gerçek reasoning modeli hiç denenmiyor |
| **W5e** | `…a_null_LlmResponse_passes_through_and_destroys_the_proof_trace_substrate` | 507 | `null` yanıt çağırana ulaşıyor; `ModelId` (P6 substratı) kayboluyor |
| **W5f** | `…a_pre_cancelled_token_still_yields_a_successful_response` | 520 | İptal edilmiş token ile "başarılı" yanıt |
| **W7h** | `…empty_and_whitespace_context_keys_are_distinct_universes` | 912 | `""`, `" "`, `"\t"` üç ayrı bağlam → "her bağlamda tam tutarlı" görünüyor |
| **W8a** | `…ReuseROI_returns_positive_infinity_from_two_perfectly_measurable_inputs` | 931 | Ölçülebilir iki girdiden ölçülemez çıktı |
| **W8b** | `…DeltaCapital_overflows_to_infinity_from_finite_inputs` | 940 | Sonlu girdiden `±Infinity` |
| **W8d** | `…a_single_self_reported_confidence_of_one_zeroes_the_entire_attention_and_tier_stack` | 958 | **conf=1.0 → InfoNeed=0 + AttentionPriority=0 + tier=Operational + gate=Autonomous** (stake 1e12, deficit 1.0 iken) |
| **W8c** (FINDING) | `…the_doc_comment_says_absolute_learning_but_the_code_rejects_negative_learning` | 947 | XML yorumu ile kod aynı şeyi söylemiyor |

**Gerçek sayı: 68 değil, 75 açık kusur; 8 değil, 9 açık gözlem.**

**Neden bu, sicilin en ciddi kusurudur:** sicilin başlığı ("**tam** envanteri") ve §0'ı
("`Ens.Kernel.Tests/` içindeki **tüm** `AUDIT_DEFECT_*`") tam kapsam iddia ediyor. Bu iddia
**yanlıştır**. Üstelik atlanan kusurlardan **W8d, muhtemelen kernel'deki en ağır tek
kusurdur**: tek bir öz-beyan sayısı (`confidence = 1.0`) dikkat kuyruğunu, model
seçimini ve otonomi kapısını **aynı anda** sıfırlıyor — 1e12 stake ve %100 uygunsuzluk
açığı varken bile `Autonomous`. Bu, sicilin §2'de "kritik" dediği çoğu kusurdan daha geniş
etkilidir ve sicilde **hiç yoktur**.

#### Kök neden (doğrulanabilir): UTF-16 kodlaması

`AdversarialWave_SecurityTests.cs` **UTF-16** kodludur; `rg`/`grep` bu dosyayı "binary file"
sayıp **hiçbir eşleşme döndürmez** (bunu bu denetimde bizzat gözledim). Sicil "test
adlarından **mekanik** türetme" diyor — mekanik araç bu dosyada sessizce sıfır sonuç
verdiği için, o dosyanın satırları **elle/kısmen** dolduruldu. Sayım hatasının **yalnızca
ve tam olarak tek UTF-16 dosyada** çıkması bu açıklamayı destekliyor.

Bu ayrıca sicilin kendi §9 dürüstlük notunun neden yetersiz kaldığını gösteriyor: not
"şiddet ataması bağımsız değil" diyor, ama asıl risk **envanterin eksik olması**ydı ve o
risk hiç işaretlenmemiş.

#### 🟡 BULGU-2: var olmayan test ID'leri (`D1a` / `D1b`)

Sicil §3.3'te `D1a` ve `D1b` ID'lerini kullanıyor
(`AuditFixed_…Tests.cs:279` ve `:303`). Kaynakta bu ID'ler **yoktur**; iki testin adı da
`AUDIT_DEFECT_D1_residual_*`'tır. Sicil "mekanik türetme" derken burada **yeni ID
uydurmuş**. Zararı sınırlı ama gerçektir: `D1a` diye arayan hiçbir şey bulamaz — sicilin
tek işlevi (bulunabilirlik) o iki satırda çalışmıyor.

FINDING tarafı: B5, D5, G8 + W2_P5 + W4b, W7b, W7c, W7g = 8 ✅ **artı W8c** = **9**.

### 2.2 Ama şiddet dağılımı tablosu ile bölüm tabloları ÇELİŞİYOR (KIRILDI)

`DEFECT-REGISTER.md:53-58` şu dağılımı veriyor: **K=14, Y=27, O=21, D=6** (toplam 68).
Bölüm tablolarındaki satırları ve ID'leri saydım:

| Bölüm | Sicilin dediği | Tablo **satırı** | Tablodaki **ID** sayısı |
|---|---|---|---|
| §2 Kritik | 14 | 14 | **17** (`W1a–W1d` tek satırda 4 ID) |
| §3 Yüksek | 27 | 25 | **25** |
| §4 Orta | 21 | 20 | **22** (`B6/W20` ve `C1/W6d` her biri 2 ID) |
| §5 Düşük | 6 | 4 | **4** |
| **Toplam** | **68** | **63** | **68** |

Yani: satır sayısıyla okursanız toplam **63** çıkar (68 değil); ID sayısıyla okursanız
toplam 68 çıkar **ama dört kovanın DÖRDÜ DE yanlıştır** (14≠17, 27≠25, 21≠22, 6≠4).

**Bu neden ciddi:** dağılım tablosu toplamı doğru veriyor (68) ama hiçbir kovası doğru
değil. Bir sayı, alt bileşenleri yanlışken toplamı tutturuyorsa, o sayı **sayılmamış,
toplamı tutacak şekilde yazılmıştır**. Sicilin §0'ı "mekanik türetme" diyor — bu tablo
mekanik değil, **elle uydurulmuş**tur. §9'un dürüstlük notu bu hatayı kapsamıyor.

En somut hâli: **§5 "Düşük = 6" diyor, ama §5 tablosunda 4 satır var.** Sicilde adı
geçmeyen 2 "düşük" kusur yoktur — dolayısıyla 6 sayısı karşılıksızdır.

> Not: bu bölüm sicilin **kendi içindeki** tutarsızlığını ölçüyor. BULGU-1 ışığında bu
> tabloların hepsi ayrıca **7 kusur eksiktir**: gerçek toplam 68 değil 75'tir.

### 2.3 §2'nin ortak-özellik cümlesi YANLIŞ (KIRILDI)

`DEFECT-REGISTER.md:64-65`: *"Bu 14 kusurun ortak özelliği: **saldırgana özel yetki
gerekmiyor.** Public API'yi normal şekilde çağırarak..."*

Bu cümle §2'nin kendi içeriğiyle çelişiyor — en az **7 ID** için yanlış:

| ID | Gerekli ayrıcalık | Kanıt |
|---|---|---|
| **E5** | `BindingFlags.NonPublic` reflection — proses içinde tam güven | `AdversarialAuditTests.cs:588-592` |
| **W3c** | `FieldInfo.SetValue` ile private alan yazma | `…SecurityTests.cs:339-342` |
| **W2c** | **CapabilityPack kayıt yetkisi** — testin kendi yorumu bunu açıkça "GUVEN SINIRI" ilan ediyor | `…SecurityTests.cs:238-239, 245` |
| **W1a–W1d** | Saldırgan yok; **operatörün kendi yazım hatası** gerekiyor | `…SecurityTests.cs:70-139` |

Reflection saldırganı zaten prosesin sahibidir; "özel yetki gerekmiyor" onun için anlamsız
bir cümledir. W2c'de saldırgan ikinci bir Pack tescil edebilmelidir — bu tam olarak "özel
yetki"dir. W1a–W1d'de ise bir saldırgan hiç yoktur; kusur bir **fail-open kullanılabilirlik
hatası**dır, bir atlatma değil.

Sonuç: §2'nin başlığı ("anayasal güvenceyi **tek satırda** düşürenler") yalnızca E3, W4a,
W15, W2_P1, W2_P4, W2_L1, W2_L2, W2_O1 için gerçekten doğrudur. Geri kalan 9 ID farklı
tehdit modellerine ait ve aynı kovaya konması sicili **okuyanı yanıltır**.

### 2.4 Ad ↔ gövde doğrulaması (23 test okundu)

**Tam örtüşenler (itirazım yok):**

| ID | Gövde ne kanıtlıyor | Satır |
|---|---|---|
| E3 | Sahte `GateResult` ile `ApplyGate` → `Contextualized` → `BeginActing` çalışıyor | `AdversarialAuditTests.cs:551-558` |
| E5 | Private `ActionState` alanı reflection'la `Acting` yapılıyor, `History` boş kalıyor | `AdversarialAuditTests.cs:588-597` |
| W4a | Aynı girdi: gerçek `honest` yetki → `Blocked`; sahte `forged` → `Autonomous` | `…SecurityTests.cs:385-398` |
| W15 | `denied with { IsAllowed = true }` ve `wt with { RequiresHumanApproval = false }` → `Autonomous` | `…SchedulerGateTests.cs:394-409` |
| W2_O1 | 5 farklı `Identity` tek atomu tamamlıyor; `DecisionAggregate`'te `Owner` property'si yok (reflection ile kanıtlanıyor) | `…InvariantTests.cs:335-347` |
| W2_P1 | Cf/Cc karakterlerinin `IsNullOrWhiteSpace`'ten geçtiği tek tek assert ediliyor; görünmez trace `Traced`'e geçiyor | `…InvariantTests.cs:384-405` |
| W2_P4 | `X ⊢ X` conf=1.0; `t2.AsPremise()` döngüsü kapanıyor; layer kabul ediyor | `…InvariantTests.cs:481-495` |
| W2_L1 | `IsCommitted == false` olan karar için tam 7 geçişlik lifecycle koşuyor; `default` Identity de geçiyor | `…InvariantTests.cs:556-575` |
| W2_L2 | 5 farklı `DecisionId`, 1 tek `shared` trace; `ProofTrace`'te hiç `Identity` alanı yok | `…InvariantTests.cs:586-599` |
| W2_R2 | 3 farklı `Target`'lı olay tek aggregate'te birleşiyor, `IsCommitted` | `…InvariantTests.cs:159-174` |
| W2_R3 | Canlı yol `"   "`'yi reddediyor, replay kabul ediyor; `null` alternatif bile geçiyor | `…InvariantTests.cs:189-214` |
| W2_R4 | `backing.Add(...)` rehydrate'ten SONRA aggregate'e sızıyor, sonra o seçeneğe commit ediliyor | `…InvariantTests.cs:235-253` |
| W2_R5 | `ForgedUnknownEvent` history'ye yazılıyor, duruma etki etmiyor, hata yok | `…InvariantTests.cs:269-277` |
| W2_R6 | `Timestamp` geriye akıyor; aynı `EventId` 3 kez history'de | `…InvariantTests.cs:292-312` |
| W2_P3 | `Render()` içine sahte kural/sonuç/`conf = 1,00` enjekte ediliyor; gerçek confidence 0.10 | `…InvariantTests.cs:449-470` |
| W8 | Gate tek başına `CriticalBlock` veriyor; Scheduler yolu `ArgumentOutOfRangeException("stake")` atıyor | `…SchedulerGateTests.cs:200-208` |
| W1a–W1d | `Disable("operations")` sessiz; `IsEnabled` de yalan söylüyor; ön-Disable Register'ı ölü doğuruyor | `…SecurityTests.cs:79-138` |
| W2c | Kiril `а` ikizi onay kısıtı olmadan yetkileniyor, uzunluklar eşit | `…SecurityTests.cs:243-255` |
| W3c | `_allowedTools` FrozenSet reflection ile değiştiriliyor, registry yeniden doğrulamıyor | `…SecurityTests.cs:339-345` |
| W16 | `toolAuthorization: null` → `Autonomous`; `Scheduler.Schedule` imzasında `CapabilityRegistry` YOK | `…SchedulerGateTests.cs:418-430` |
| W17 | Üç ayrı fail-closed dal `NaN InfoNeed` yayıyor | `…SchedulerGateTests.cs:440-452` |
| W2_L3 | Tüm lifecycle geriye akan zamanla koşuyor, `History[^1].At < History[0].At` | `…InvariantTests.cs:609-619` |

Bu 22 kalemde **ad ile gövde örtüşüyor**. Sicilin bu satırları güvenilirdir.

### 2.5 Ad ↔ gövde UYUŞMAZLIKLARI

#### (a) H1 — ad, gövdenin kanıtladığından FAZLASINI söylüyor ⚠️

Sicil (`§2`): *"Bir öneri (`proposal`) herhangi bir çağıran tarafından **3 satırda otomatik
uygulanabiliyor**"* — KRİTİK.

Gövde (`AdversarialAuditTests.cs:1020-1033`) ne yapıyor: `ReflectiveDoubleLoop.Propose`
çağırıyor, sonra döngüde `memory.Record(new MemoryRecord(..., p.PurposeType, 0, 1.0, T0))`
yazıyor ve tek assert: `Assert.Equal(5, memory.AllRecords.Count)`.

**İtiraz:** bu "öneriyi uygulamak" değildir. Bu, **öneriyle aynı `PurposeType`'a sahip yeni
bir bellek kaydı yazmaktır**. Öneri hiçbir yere "uygulanmadı"; sistemde hiçbir davranış
değişmedi. Testin kanıtladığı tek şey `CompanyMemory.Record`'un public ve korumasız
olduğudur — ki bu bir **bellek deposu için tasarım gereğidir**, bir P7 ihlali değil.

Testin kendi yorumu bunu zaten itiraf ediyor (`:1012-1015`): kusur, demo'nun "uygulayacak
metot yok" demesinin **yokluktan argüman** olmasıdır. Yani H1'in gerçek doğası bir
**FINDING**'dir (iddia zayıf), bir DEFECT değil — üstelik §6'daki W4b ile aynı ailedendir
("insan onayı birinci sınıf tip değil").

**Talep:** H1 ya §6'ya FINDING olarak taşınmalı, ya da sicil satırı gövdenin kanıtladığıyla
sınırlandırılmalı: *"Kod tabanında 'insan onayı' diye bir tip yoktur; 'öneri uygulanamaz'
iddiası yokluktan argümandır."* Mevcut ifade **sicilde olmayan bir kusuru var gösteriyor**.

#### (b) W2_L4 — yanlış bölüme konmuş (kategori hatası) ⚠️

Sicil W2_L4'ü **§3.5 "Canlı koleksiyon sızıntıları (immutability ihlalleri)"** altına
koyuyor. Ama `AUDIT_FIXED_E4` (`AdversarialAuditTests.cs:572`) `History`'nin downcast ile
mutasyona **kapalı** olduğunu kanıtlıyor. W2_L4'ün gövdesi
(`…InvariantTests.cs:634-647`) bir immutability ihlali değil, **enumeration sırasında
koleksiyon değişince `InvalidOperationException`** gösteriyor — bu bir **snapshot/eşzamanlılık**
kusurudur, bir immutability kusuru değil. Aynı tabloda W22, W5a, W5b, W2_R4 gerçek
mutasyon sızıntılarıdır; W2_L4 onlarla aynı kalıba ait değildir.

Sonucu §7'ye taşıyor: kalıp 6 ("Canlı koleksiyon dönüyor") 5 kusur sayıyor; W2_L4 o kalıba
ait değilse **kalıp 6 aslında 4 kusurdur** (bkz. §4.1).

#### (c) W2c — ad "mints an approval-free twin" diyor, gövde iki Pack gerektiriyor ⚠️

Ad, tek aktörün bir ikiz "bastırdığını" ima ediyor. Gövde
(`…SecurityTests.cs:243-245`) **ikinci bir CapabilityPack tescili** gerektiriyor.
Ad ile gövde teknik olarak çelişmiyor ama sicil bunu §2'nin "özel yetki gerekmiyor"
kovasına koyduğu için **okur yanıltılıyor** (bkz. §2.3).

---

## 3. SALDIRI 2 — Şiddet atamaları savunulabilir mi?

### 3.0 Bağımsız bir ölçüt buldum: testlerin KENDİ şiddet beyanları

Sicil, şiddeti yazarın kendisinin atadığını kabul ediyor (§9). Ama **bağımsız bir kaynak
var ve sicil onu kullanmamış**: bazı testler, gövdelerinde denetleyen ajanın kendi şiddet
değerlendirmesini yazıyor. Bu, sicilin yazarından **bağımsız** bir ikinci görüştür.
İkisini karşılaştırdım.

### 3.1 🔴 W3c — sicil KRİTİK diyor, testi yazan ajan "DÜŞÜK ŞİDDET" demiş

`AdversarialWave_SecurityTests.cs:328`, testin ilk yorum satırı, harfiyen:

> `// KUSUR (dusuk siddet, YUKSEK iddia-hatasi): kaynak yorumu "runtime'da GERCEKTEN`
> `// sabittir" der. Bu MUTLAK ifade yanlistir...`

Yani saldırıyı **yapan** ajan bu kusuru "düşük şiddet, yüksek iddia-hatası" olarak
sınıflandırmış — yani onu bir **FINDING'e yakın** görmüş. Sicil onu §2'ye, en üst kovaya,
"anayasal güvenceyi tek satırda düşüren 14 kusur" arasına koymuş.

Bu, sicilin şiddet sütununun **yalnızca bağımsız olmadığını değil, elde bulunan bağımsız
kanıtla çeliştiğini** gösterir. §9'un dürüstlük notu "bağımsız değil" diyor; ama mevcut
bağımsız değerlendirmenin **ters yönde** olduğunu söylemiyor.

**Yönü de önemli:** bu, "yazar kendi kodunu korur" hipotezinin **tersi** yönde bir hata.
Yazar burada şiddeti **abartmış**. Aşağıda bunun tersi örnekler de var — yani sistematik
bir koruma eğilimi değil, **sistematik bir kalibrasyonsuzluk** görüyorum. Bu daha az
ahlaki, ama epistemik olarak aynı derecede kötüdür: her iki yöne de sapan bir ölçü,
ölçü değildir.

### 3.2 🔴 W16 — DÜŞÜK'e konmuş, ama W4a/W15 ile AYNI kusur ailesinden (KRİTİK olmalı)

Sicil `§5 Düşük`: *"`null` `toolAuthorization`, 'yetkilendirmeyi unutmak'tan ayırt
edilemiyor"* — "kozmetik / dayanıklılık" kovası.

Gövdenin gerçekte kanıtladığı (`…SchedulerGateTests.cs:418-430`):
1. `toolAuthorization: null` → `GateDecision.Autonomous` (en permisif okuma, fail-**open**);
2. `PendingDecision`'da **araç adı taşıyan hiçbir alan yok** — Scheduler yetkiyi kendi
   çözemez;
3. `Scheduler.Schedule` imzasında **`CapabilityRegistry` parametresi yok**.

Yani: Capability Registry → Gate bağı **kernel içinde hiç zorlanmıyor**; tamamen çağırana
delege edilmiş ve **varsayılan davranış "yetki yok = tam otonomi"**. W4a "sahte yetki
nesnesiyle registry atlanır" diyor ve KRİTİK; W15 "gerçek red aklanır" diyor ve KRİTİK.
**W16, ikisinden de ucuz bir atlatma yolu gösteriyor: hiçbir şey yapma, `null` bırak.**

Sahte nesne üretmek bile gerekmiyor. Bu, sicilin §2 tanımına ("saldırgana özel yetki
gerekmiyor; public API'yi normal çağırarak P7 düşürülüyor") W4a'dan **daha iyi** uyar.

**İtiraz: W16 D → K.** Bu, aynı zamanda §2'nin "tek satırda" kriterine sıfır satırda uyan
tek kusurdur.

### 3.3 🟠 W8d (sicilde YOK) — kaydedilse KRİTİK olurdu; kaydedilmemiş

BULGU-1'de gösterildi. `…SecurityTests.cs:967-974`: `stake = 1e12`,
`conformanceDeficit = 1.0`, `confidence = 1.0` → `InfoNeed = 0`, `AttentionPriority = 0`,
`tier = Operational`, `gate = Autonomous`. Tek bir **öz-beyan** sayısı, P5 (dikkat
tahsisi) + model seçimi + P7 (sınırlı özerklik) yığınının **üçünü birden** kapatıyor.

Sicil, aynı kök nedenin **zayıf** hâlini (`W7`, `…SchedulerGateTests.cs:175` — "conf=1.0
sınırsız özerklik satın alıyor") **ORTA** olarak kaydetmiş; **güçlü** hâlini hiç
kaydetmemiş. Sonuç okuyucu açısından şudur: sicil, kernel'in en geniş etkili tek kusurunu
"orta şiddetli bir sayısal sınır problemi" gibi gösteriyor.

**İtiraz: W7 O → K, ve W8d sicile K olarak eklenmeli.**

### 3.4 🟠 W2_R4 — YÜKSEK'e konmuş, ama Individuation mührünü PUBLIC API ile deliyor

Testin **kendi** yorumu (`…InvariantTests.cs:220`): **"EN AGIR BULGU."**

Gövde (`:235-253`): reflection yok, downcast yok. Çağıran kendi `List<string>`'ini bir
`ReadOnlyCollection` içine sarıp olaya koyar; `Rehydrate` sonrası `backing.Add(...)` ile
aggregate'in deliberation kümesine **hiç değerlendirilmemiş bir alternatif** sızar; sonra
`d.Commit(owner, "HIC-DEGERLENDIRILMEMIS", ...)` **meşru sayılır**.

Bu, ENS-2001 Individuation'ın "açık Alternatives" koşulunun doğrudan ihlalidir ve
sicilin §2 kriterine (özel yetki yok + tek satır + anayasal güvence) **tam uyar**. Sicil
onu §3.5'e, üstelik **yanlış başlığa** ("canlı koleksiyon sızıntıları") koymuş.

**İtiraz: W2_R4 Y → K.**

### 3.5 🟠 W2_R2 — YÜKSEK, ama W2_O1 (KRİTİK) ile aynı güvenceyi kırıyor

W2_O1 ("tek-sahip koşulu hiç implemente edilmemiş") = KRİTİK.
W2_R2 (`…InvariantTests.cs:159-174`): **üç farklı karara ait olaylar dördüncü bir kimlik
altında birleşiyor** ve `IsCommitted` oluyor. Bu, aggregate sınırının (Individuation'ın
taşıyıcısı) tümden düşmesidir — W2_O1'in "temsil edilmemiş" durumundan **daha somut** bir
ihlaldir, çünkü W2_O1 bir **eksiklik**, W2_R2 bir **aktif kabul**tür.

Birini K, diğerini Y yapmak savunulamaz. **İtiraz: W2_R2 Y → K** (ya da W2_O1 K → Y;
ama ikisinin farklı kovada olması tutarsız).

### 3.6 🟡 W8 (Scheduler) — KRİTİK, ama W9 ile aynı mekanizma ve W9 ORTA

- **W8** (`…SchedulerGateTests.cs:200-208`): ölçülemez girdi → Scheduler `Schedule`
  içinde `ArgumentOutOfRangeException`; gate'in `CriticalBlock` dalına hiç ulaşılmıyor.
- **W9** (`:229-238`): ölçülemez tek girdi → **aynı exception**, 999 sağlıklı karar
  tahsissiz kalıyor.

Aynı satır (`Scheduler.cs:99`), aynı exception, aynı kök. Sicil birini **K**, diğerini
**O** yapmış. Dahası: W8'de eylem **çalışmıyor** (exception fail-closed'dur); kayıp olan
şey *denetlenebilir bir gate kaydı*dır (P6), *otonomi sınırı* (P7) değil. Testin kendi
gerekçesi bile varsayımsaldır: *"`catch/log/continue` yazan bir çağırıcı için gate hiç
çalışmamış olur"* — **böyle bir çağırıcı gösterilmiyor**.

**İtiraz:** W8 ve W9 aynı kovada olmalı. Gösterilen zarar W9'da daha büyük olduğu için
ikisi de **Yüksek** olmalıdır; W8'in K'si abartılmıştır (yine "kendi kodunu koruma"nın
TERSİ yönde bir hata).

### 3.7 🟡 H1 — KRİTİK, ama gövdesi bir DEFECT bile kanıtlamıyor

Bkz. §2.5(a). Gövde yalnızca `CompanyMemory.Record`'un public olduğunu gösteriyor. Bu,
bir bellek deposunun tasarım gereğidir. **İtiraz: H1 K → FINDING** (ya da en fazla O).

### 3.8 🟡 W2_L4 — YÜKSEK, ama immutability ihlali değil

Bkz. §2.5(b). `History` mutasyona kapalı (`AUDIT_FIXED_E4` bunu kanıtlıyor). Kalan risk:
tek iş parçacığında enumeration-invalidation, çok iş parçacığında yarış. Gerçek bir kusur
ama "yönetişim güvencesini sessizce kapatır ya da iz bırakmadan bozar" tanımına uymuyor.
**İtiraz: W2_L4 Y → O.**

### 3.9 Katıldığım şiddet atamaları

Aşağıdakilere **saldırdım ve kıramadım** — atama savunulabilir:

| ID | Şiddet | Neden doğru |
|---|---|---|
| E3, W4a, W15 | K | Tek satırda sahte yetki/karar nesnesi, gerçek registry reddini geçersiz kılıyor; kanıt gövdede tam |
| W2_P1 | K | Görünmez trace `Traced`'e geçiyor; L8/Madde VI doğrudan düşüyor |
| W2_L1, W2_L2 | K | Kararsız lifecycle + kanıtın kanıtladığı şeye bağlı olmaması; ikisi de public API |
| W2_R5 + W2_R2 birleşimi | Y | Sicilin §3.4 altındaki birleşim notu (`:134-136`) doğru ve değerli bir sentez |
| A1, A2, D4 | Y | Zaman girdisi doğrulanmıyor; D4'ün "geleceği görme" notu (`:113-114`) yerinde |
| B2, B1 | Y | Kanıt güvencesi kozmetik; gövdeler bunu gösteriyor |
| W2d, W5c | D | Gerçekten dayanıklılık/kozmetik düzeyde |
| C1/W6d'nin O'su | O | `AUDIT_HOLDS_W6e` (`…SecurityTests.cs:660-673`) yanlış bağlanmanın **yönünün güvenli** olduğunu 200 örnekle gösteriyor — O doğru karar, ve sicil bunu abartmamış (takdir) |

### 3.10 Hipotez testi: "yazar kendi kodunu koruyor" doğru mu?

Görev bu hipotezi ciddiye almamı istedi. Test ettim; **doğrulanmadı**:

- **Koruma yönünde sapmalar:** W16 (D, olmalı K), W7 (O, olmalı K), W8d (hiç yok, olmalı K),
  W2_R4 (Y, olmalı K), W2_R2 (Y, olmalı K) → **5 adet aşağı sapma**.
- **Ters yönde sapmalar:** W3c (K, ajanın kendi değerlendirmesi "düşük"), W8 (K, olmalı Y),
  H1 (K, DEFECT bile değil) → **3 adet yukarı sapma**.

Sapmalar **iki yönlü**. Bu, kasıtlı bir koruma değil, **kalibre edilmemiş bir ölçek**
gösterir: sicilde "Kritik" için verilen tanım ("saldırgana özel yetki gerekmiyor")
uygulanmamış — E5/W3c (reflection = tam güven) ve W2c (Pack tescil yetkisi) o tanımı
karşılamadığı hâlde K kovasındadır (bkz. §2.3), W16 karşıladığı hâlde D kovasındadır.

**Ama BULGU-1 bu tabloyu değiştirir:** eksik 7 kusurun tamamı (W5d/W5e/W5f/W7h/W8a/W8b/W8d)
**tek bir dosyadan** ve içlerinden en ağırı (W8d) `DecisionGravity` + `LlmTierSelector` +
`BoundedAutonomyGate` üçlüsünü, yani **yazarın en çok savunduğu koddan**. Kasıt iddia
etmiyorum — kanıtım yok ve UTF-16 açıklaması yeterlidir. Ama **sonuç** aynıdır: sicil,
kernel'in en zayıf noktasını görünmez kılıyor.

---

## 4. SALDIRI 3 — §7 kök-neden analizi doğru mu?

### 4.0 🔴 Yapısal kusur: §7 YANLIŞLANAMAZ biçimde yazılmış (Madde X ödevi karşılanmamış)

Denetime başlamadan önce yapısal bir itiraz: **§7, hangi kusurun hangi kalıba ait olduğunu
hiçbir yerde yazmıyor.** Yalnızca "6 / 9 / 5 / 5 / 3 / 5" sayıları var, üyelik listesi yok.

Bu, ENS için sıradan bir eksiklik değil. Yanlışlanamaz iddiayı **reddeden** madde Madde VI'dır;
Madde X ise yanlışlanabilirlik koşullarını belirtme **ödevini** yükler ve bunu yapmayan yapıtı
**eksik** sayar (yasak koymaz). Burada her ikisi de devrededir. "33
kusur birden kapanır" **test edilebilir bir iddiadır** ama yalnızca üyelik listesi verilirse.
Liste olmadan iddia, ne doğrulanabilir ne yanlışlanabilir — yani sicilin **kendi
otoritesine dayandığı maddeyi** ihlal eder.

Aşağıdaki denetimi, üyeliği bölüm tablolarından **çıkarsayarak** yaptım; bu çıkarsamanın
kendisi bir zorunluluktur ve sicilin kusurudur.

### 4.1 Sayı denetimi — kalıp başına

| # | Kalıp | Sicil | Çıkarsanabilir üyeler | Gerçek | Verdict |
|---|---|---|---|---|---|
| 1 | Public record = taklit edilebilir yetki | 6 | E3, W4a, W15, H1 (§8 bunları açıkça sayıyor) + W16, W23? | **4 kesin, 6 belirsiz** | ⚠️ doğrulanamaz |
| 2 | Kimlik normalizasyonu yok | 9 | F3, G3, G4, W7f, W2c, W2e, W2f, W1a, W1b, W1c, W1d (+C2, C3, **W7h**) | **≥11, muhtemelen 14** | ❌ **eksik sayılmış** |
| 3 | Zaman çağırandan geliyor | 5 | A1, A2, D4, W2_L3, W2_R6 = §3.2'nin tam beşi | **5** | ✅ **doğru** |
| 4 | Eşik `0` = sessiz kapatma | 5 | A5, E4, G2, H3 (+W10?) | **4** | ❌ **1 fazla** |
| 5 | Reflection tüm değişmezleri deler | 3 | E5, W3c | **2** | ❌ **1 fazla** |
| 6 | Canlı koleksiyon dönüyor | 5 | W22, W2_R4, W5a, W5b (+W2_L4) | **4 + 1 sınırda** | ⚠️ sınırda |

**Kalıp 3 tek başına tam doğrudur** ve §3.2 ile bire bir örtüşür — sicilin bu satırı
güvenilirdir.

**Kalıp 4 (`1 fazla`):** W10'un gövdesi (`…SchedulerGateTests.cs:242-263`) bir "eşik = 0"
kusuru **değildir**. W10, politika doğrulamasının erken-dönüşlerden **sonra** yapılmasıdır
— `NaN` eşikli bir politikanın veriye bağlı olarak bazen geçip bazen patlamasıdır. Önerilen
düzeltme ("Nullable eşik + açık `Disabled` durumu") W10'a **hiçbir şey yapmaz**; W10'un
düzeltmesi doğrulamayı erken-dönüşlerin **önüne** almaktır. Kalıp 4 = 4.

**Kalıp 5 (`1 fazla`):** Kaynakta reflection ile **istismar** eden yalnızca iki test var:
E5 (`AdversarialAuditTests.cs:588`, private `ActionState` alanı) ve W3c
(`…SecurityTests.cs:339`, `_allowedTools`). Diğer reflection kullanımları (W2_O1:346,
W2_L2:597, W16:423) **yokluk kanıtı**dır — istismar değil. W5a/W5b reflection değil,
**downcast**tır ve testin kendi yorumu bunu vurguluyor: *"REFLECTION GEREKMEZ"*
(`…SecurityTests.cs:449`). Kalıp 5 = 2.

**Kalıp 2 (eksik sayılmış):** `W1a–W1d` sicilde tek satır olduğu için muhtemelen **1**
sayılmış; oysa 4 ayrı testtir ve dördü de aynı normalizasyon kökündendir. Ayrıca
BULGU-1'in **W7h**'si (boş/whitespace ContextKey → ayrı evrenler) tam olarak bu kalıptandır
ve sicilde yoktur. Yani kalıp 2 **daha büyüktür**, daha küçük değil.

### 4.2 Çoklu-kalıp üyeliği — 33 şişirilmiş mi?

Evet, ama beklenenden farklı bir yoldan.

**(a) Kesişimler var ama küçük.** Aradım; gerçekten çift-sayım riski taşıyan iki kalem
buldum:
- **C2 / C3** (`…MemoryTests.cs:292, 315`): "saat kayıt **değerine** göre anahtarlanıyor,
  kimliğine göre değil". Bu hem **kalıp 2** (kimlik), hem de zamanla ilgili olduğu için
  §3.2'nin komşusu. Sicil bunları §4.1'e (kimlik) koymuş — savunulabilir, çift sayım yok.
- **W2_R4**: hem **kalıp 6** (canlı koleksiyon) hem de replay-yolu ayrışması (§3.4 ile aynı
  kök). Sicil §3.5'e koymuş; kalıp 6'da sayılıyor. Tek sayım. ✓

Yani **çift sayım kaynaklı şişme küçüktür.** Sicilin bu yönü sağlamdır — saldırdım,
kıramadım.

**(b) Asıl şişme başka yerden geliyor: kalıp 5'in "kararı" bir düzeltme değil.**

`DEFECT-REGISTER.md:233`, kalıp 5'in "Gereken karar" sütunu harfiyen:
*"Kabul edilebilir mi? **Açıkça karar verilmeli**, sessiz bırakılmamalı"*.

Bu bir **mimari düzeltme değildir** — bir *kapsam beyanı*dır. §8.3 bunu zaten kabul ediyor:
*"reflection saldırısı kernel sınırı içinde mi savunulacak, yoksa process sınırında mı?
İkincisi ise bu kusurlar **kapsam dışıdır**"*. Kapsam dışına almak bir kusuru **kapatmaz**;
onu **kusur olmaktan çıkarır**. §7'nin "33'ü birden kapanır" cümlesi bu 2 (sicile göre 3)
kusuru "kapanan" hanesine yazamaz.

**Düzeltilmiş sayı:** 6 + 9 + 5 + 5 + 3 + 5 = 33 →
gerçek üyelikle: 6(belirsiz) + ≥11 + 5 + 4 + 0(kapsam dışı) + 5 = **≥31, ama içinde
"kapanan" olarak sayılabilecek olan en fazla ~29'dur.**

Ve BULGU-1 nedeniyle payda da yanlış: "68 kusurun 33'ü" değil, **"75 kusurun ~29-31'i"**.
Oran %48'den **%39-41**'e düşer. §7'nin okuma talimatı ("6 karar alınırsa yarısı kapanır")
bu oranla aynı gücü taşımaz.

### 4.3 Önerilen kararlar KAPATIYOR mu, GİZLİYOR mu?

#### (a) "İmzalı gate-token" reflection saldırısına (E5/W3c) ne yapar? **HİÇBİR ŞEY.**

Görevin sorduğu somut sorgu. Cevap kesin ve gövdelerle gerekçelendirilebilir:

- **E5** (`AdversarialAuditTests.cs:592`): saldırgan `stateField.SetValue(layer,
  ActionState.Acting)` yapıyor. **Gate hiç çağrılmıyor.** Çağrılmayan bir kapının ürettiği
  token da doğrulanmıyor. İmza, hiç sorulmayan bir soruya verilen cevaptır.
- **W3c** (`…SecurityTests.cs:341-344`): saldırgan `CapabilityPack._allowedTools`'u
  değiştiriyor; sonra **gerçek** registry, **gerçek** `Authorize` çağrısıyla
  `delete_database` için `IsAllowed = true` üretiyor. İmzalı token mimarisinde bu token
  **geçerli biçimde imzalanır** — çünkü yalan, imzanın *altına* değil *üstüne*
  yerleştirilmiştir. **İmza, yalanı taşınabilir ve inandırıcı kılar.**

Yani imzalı token bu iki kusur için ne kapatır ne gizler — **ilgisizdir**. W3c örneğinde
ise durumu bir miktar **kötüleştirir** (sahte yetki artık kriptografik olarak "doğrulanmış"
görünür).

**Sicil bu konuda haklıdır ve bunu ayırmıştır:** kalıp 1 (token) ile kalıp 5 (reflection)
ayrı satırlardır ve §8.3 sandbox borcunu ayrıca yazmıştır. **Bu noktada saldırdım,
kıramadım** — sicilin ayrımı doğrudur. Tek kusur, kalıp 5'in "kapanan 33"e dahil
edilmesidir (§4.2b).

#### (b) "Normalize edilmiş kimlik tipi" kalıp 2'nin 9'unu kapatır mı? Kısmen.

Kapatır: F3, G3, G4, W7f, W2c, W7h, W1a–W1d (Disable'ın normalize edilmiş anahtarla
çalışması). Kapatmaz:
- **W2f** (`…SecurityTests.cs:284-298`): bu bir **asimetri** kusurudur — `CapabilityPack`
  boş araç adını kabul ederken `Authorize` onu `ArgumentException` ile reddediyor.
  Normalizasyon bu asimetriyi çözmez; **aynı guard'ın iki tarafta da uygulanması** gerekir.
- **W2e** (NUL içeren adlar): normalizasyon (NFC/trim/case) NUL'u **kaldırmaz**. Gereken
  şey ayrı bir **karakter sınıfı doğrulaması**dır.
- **W2d** (`Reason` metnine RTL-override akıyor): bu bir **çıktı sanitizasyonu**dur, kimlik
  normalizasyonu değil.

Yani kalıp 2'nin önerilen kararı, kendi üyelerinin **~%70'ini** kapatır. Sicil bunu "9'u
birden kapanır" diye sunuyor.

#### (c) "Nullable eşik + açık Disabled" → W10'u kapatmaz (§4.1'de gösterildi).

#### (d) "Monoton saat portu + gelecek-tarih reddi" (kalıp 3) — gerçekten 5'i kapatır ✓

A1 (gelecek `AssertedAt`), A2 (`MaxValue`), D4 (`asOf` sızıntısı), W2_L3 (geriye akan audit
zamanı), W2_R6a (replay zaman sırası). Beşi de tek bir port kararıyla kapanır.
**Sicilin en sağlam satırı budur.** Saldırdım, kıramadım.

Küçük çekince: W2_R6'nın **(b)** ayağı (yinelenen `EventId`) bir saat kusuru **değildir** —
idempotency/dedup kusurudur ve saat portu onu kapatmaz. Yani kalıp 3, W2_R6'nın yarısını
kapatır.

### 4.4 🔴 Yedinci kalıp VAR — ve testlerin kendisi ona isim vermiş

Görev, 6 kalıba girmeyen kusurlar arasında 7. bir kalıp arayıp aramadığımı sordu. **İki
tane buldum**, ikisi de kaynakta zaten adlandırılmış:

#### Kalıp 7 — "Girdi kapısı var, ÇIKTI kapısı yok"

Bu isim benim değil. `AdversarialWave_SecurityTests.cs:927`, bölüm başlığı, harfiyen:

> `// W8. LAWS - DECISION CAPITAL / GRAVITY: girdi kapisi var, CIKTI kapisi yok`

Üyeler (hepsi doğrulanmış gövdelerle):

| ID | Kanıt | Ölçülemez çıktı |
|---|---|---|
| **W8a** | `…SecurityTests.cs:936` | `ReuseROI(1e308, double.Epsilon)` → `+Infinity` (iki girdi de guard'ı geçiyor) |
| **W8b** | `:942-943` | `DeltaCapital(1e308, -1e308)` → `±Infinity` |
| **W17** | `…SchedulerGateTests.cs:440-452` | Üç fail-closed dal `NaN InfoNeed` yayıyor |
| **H4** | `…MemoryTests.cs:906` | `Compute` `-Infinity` yayıyor |
| **W5e** | `…SecurityTests.cs:516` | `null LlmResponse` çağırana ulaşıyor (P6 substratı kayboluyor) |
| **W3** | `…SchedulerGateTests.cs:97` | Negatif sıfır normalize edilmiyor |

**6 kusur, tek kök neden:** `Guard.cs`'in politikası ("ölçülemeyen değer kernel'in karar
yollarına giremez") **yalnızca girdi sınırında** uygulanıyor; hiçbir fonksiyon dönüş
değerini doğrulamıyor. **Gereken karar:** çıktı postcondition kapısı (`Guard.Result(...)`
ya da ölçülemez değerleri temsil edemeyen bir `Measured<double>` tipi).

Bu kalıbın sicilde olmamasının doğrudan sebebi BULGU-1'dir: üyelerinin 2'si (W8a, W8b) ve
1'i daha (W5e) sicilde hiç yok, kalanlar (W17, H4, W3) §4.2'ye "sayısal sınır" diye
dağıtılmış — yani **kalıp, parçalarına ayrılarak görünmez kılınmış**.

#### Kalıp 8 — "Öz-beyan kalibre edilmemiş" (P6'nın açık kökü)

Üyeler: **W8d** (conf=1.0 tüm yığını sıfırlıyor), **W7** (conf=1.0 sınırsız özerklik),
**W7d** (n=1 → "gürültü yok"), **B2** (`"x"` geçerli kanıt), **B1** (varsayılan parametre
kanıt guard'ını etkisiz kılıyor), **D1_residual** (öncüller kalibre edilmemiş serbest
metin), **G5** (sıfır provenance'lı kayıtlardan öneri), **W2_P4** (kendi kendini
gerekçelendiren türetim).

**8 kusur, tek kök:** kernel, `confidence` ve `Premise` için **aralık** doğruluyor
(`[0,1]`, boş değil) ama **kalibrasyon ya da provenance** hiç istemiyor. Testlerin
kendisi bunu iki ayrı yerde söylüyor: `…SchedulerGateTests.cs:177` (*"Guard
OLCULEBILIRLIGI kapatti, KALIBRASYONU degil"*) ve `…SecurityTests.cs:965` (*"P6
kalibrasyonu ZORLANMADIGI icin ... bu tek sayi, tum yigin icin bir kapatma dugmesidir"*).

**Gereken karar:** `confidence`'ın kaynağı ve kalibrasyonu birinci sınıf hâle gelmeli
(ENS-3022 borcu). Bu, sicilin 6 kalıbının hiçbirinde yok ve **en az 8 kusur** taşıyor —
yani sicilin en büyük kalıbından (kalıp 2) sonra ikinci en büyüğü.

**Sonuç:** §7'nin "68 kusur, 6 kalıp" başlığı yanlıştır. Doğrusu yaklaşık: **75 kusur,
en az 8 kalıp** — ve atlanan 2 kalıp, kernel'in **anayasal olarak en hassas** iki yerine
(ölçülebilirlik sınırı ve öz-beyan kalibrasyonu) denk geliyor.

---

## 5. §6 FINDING denetimi — özellikle W7c

### 5.1 W7c'nin OLGUSAL çekirdeği: DOĞRU ✅

Görev, `DecisionEntropy.cs:48`'in gerçekten `Math.Max(0, hac - hacOwner)` olup olmadığını
sordu. Dosyayı okudum. Satır **48**, harfiyen:

```csharp
return Math.Max(0, hac - hacOwner); // I(A;Owner|C) = H(A|C) − H(A|C,Owner)
```

**`LevelNoise` gerçekten bir artıktır (residual).** Kendi tanımından (ör.
`ΣΣ p(a,o|c) log[p(a,o|c)/(p(a|c)p(o|c))]`) değil, iki bağımsız hesaplanan entropi
arasındaki **fark** olarak üretiliyor. W7c'nin bu iddiası doğrudur ve sicilin §6 satırı bu
noktada güvenilirdir.

`W7b`'nin "clamp ölü kod" iddiası da doğrudur: ampirik dağılımda koşullama entropiyi
artıramaz (`H(A|C,Owner) ≤ H(A|C)` bir teoremdir), dolayısıyla `Math.Max(0, ·)` hiç
tetiklenemez. `AUDIT_FINDING_W7b` (`…SecurityTests.cs:797-808`) bunu 300 veri kümesiyle
gözlemliyor.

### 5.2 Ama W7c'den ÇIKARILAN sonuç FAZLA SÖYLÜYOR ⚠️

Sicil `:216-218`, blok alıntı, harfiyen:

> **"W7c, bu projede bulunan en değerli FINDING'dir.** Bir sayı, kendi tanımından değil
> artık olarak hesaplanıyorsa, **o sayıyı içeren özdeşliği doğrulamak hiçbir şey
> doğrulamaz.**"

İkinci cümle, olduğu genellikte **yanlıştır** — ve karşı-kanıt aynı dosyadadır.

`AUDIT_HOLDS_W7a` (`…SecurityTests.cs:754-789`) tam olarak sicilin "imkânsız" dediği şeyi
yapıyor: `I(A;Owner|C)`'yi **tamamen farklı bir formülle** ve **`DecisionEntropy`'ye hiç
dokunmadan** hesaplıyor —

```
expectedI = H(A|C) + H(O|C) − H(A,O|C)     // testin kendi H/CondH yardımcılarıyla (:726-752)
actualI   = DecisionEntropy.LevelNoise(obs) // kodun artık tanımıyla
```

ve 500 rastgele veri kümesinde `|actualI − expectedI| < 1e-9` olduğunu doğruluyor.

Bu **tautoloji değildir**: iki ifade, ancak (i) `ConditionalEntropy` gerçekten `H(A|C)` ise,
(ii) `ConditionalEntropyGivenOwner` gerçekten `H(A|C,Owner)` ise ve (iii) zincir kuralı
`H(A,O|C) = H(O|C) + H(A|O,C)` empirik dağılımda tutuyorsa eşit olur. Her üçünden biri
bozulursa test **kırmızı yanar**. Dahası `:786` ayrıca `H(A|C) ≥ H(A|C,Owner)`
eşitsizliğini bağımsız olarak sınıyor — bu, artık tanımının **arkasındaki** varsayımdır ve
inşa gereği doğru değildir.

**Doğru ifade şudur:** `LevelNoise + H(A|C,Owner) = H(A|C)` **özdeşliğini** test etmek
neredeyse boştur (W7c haklı); ama `LevelNoise`'un **değerini** bağımsız bir CMI formülüyle
karşılaştırmak boş değildir (W7c'nin genellemesi haksız). Sicil ikincisini hiç anmıyor.

### 5.3 🔴 ROADMAP düzeltmesi GERİ ALINMAMALI — çünkü sicilden daha doğru

Görev, W7c'ye dayanarak ROADMAP'te yapılan düzeltmenin yanlışsa geri alınması gerektiğini
söyledi. **Geri alınmamalı.** `ROADMAP.md:243-251`'i okudum; metin şunu yapıyor:

1. "zincir-kuralı matematiksel olarak doğrulandı" ifadesini **kaldırıyor** (W7c'nin doğru
   çekirdeğine dayanarak — ✅ haklı);
2. Ardından açıkça şunu ekliyor: *"✅ Buna karşılık **değerin kendisi bağımsız olarak
   doğrulandı**: `AUDIT_HOLDS_W7a`, 500 veri kümesi üzerinde koşullu karşılıklı bilgiyi
   (CMI) bağımsız hesaplayıp `LevelNoise` ile karşılaştırdı — eşleşti. Yani sayı doğru;
   yanlış olan, o sayıyı içeren özdeşliği 'doğrulama' saymaktı."*

Bu **tam olarak doğru ifadedir** — §5.2'de bağımsız olarak vardığım sonucun aynısı.

**Asıl bulgu şu: ROADMAP, DEFECT-REGISTER'dan daha doğrudur.** Sicilin §6/W7c satırı ve
`:216-218` blok alıntısı, ROADMAP'in eklediği dengeleyici cümleyi **taşımıyor** ve bu
yüzden W7a'yı okumamış bir okuyucuya "ENS-3021 hiç doğrulanmamış" izlenimi veriyor.

**Talep:** düzeltilmesi gereken ROADMAP değil, **sicilin kendisidir**. `DEFECT-REGISTER.md`
§6/W7c satırına ve `:216-218` blok alıntısına W7a karşı-ağırlığı eklenmelidir; aksi hâlde
sicil, Madde X adına yaptığı eleştiriyi **kendisi ihlal eder** (doğrulanmış bir sonucu
"doğrulanmamış" göstermek de bir doğrulanmamış iddiadır).

### 5.4 W7b ile W7c aynı bulgunun iki yüzü — FINDING sayısı hafif şişkin 🟡

`W7b` ("clamp ölü kod") ve `W7c` ("özdeşlik inşa gereği") **aynı** kök olgudan çıkıyor:
`LevelNoise` bir artıktır. Sicil bunu iki ayrı FINDING olarak sayıyor. Bu bir hata
değil (iki ayrı test var) ama "8 açık gözlem" sayısı, bağımsız gözlem sayısı olarak
okunursa 7'dir.

### 5.5 Diğer FINDING'lerin denetimi

| ID | Verdict | Gerekçe |
|---|---|---|
| **W2_P5** (`min` idempotent) | ✅ **doğru ve değerli** | `…InvariantTests.cs:507-519`: 100 halkalı zincir 0.9'da kalıyor; 1000 aynı öncül korroborasyon üretmiyor. Sicilin "40 adım = 1 adım" özeti gövdeyle örtüşüyor |
| **W4b** (yetkisiz araç ≡ riskli karar) | ✅ **doğru** | `…SecurityTests.cs:411-421`: iki farklı sebep aynı `Blocked` durumuna düşüyor; testin kendi yorumu "allowlist bir onay istemine indirgenebilir" diyor — sicilin özeti bunu **eksik** aktarıyor (asıl mesele "operatör ayırt edemiyor" değil, allowlist'in sert sınır olmaması) |
| **W7g** (normalize edilmemiş entropi) | ✅ **doğru** | `:903-905`: `log2(10000) ≈ 13.29` vs `1.0` |
| **B5, D5, G8** | ⚠️ **doğrulanmadı** | Bu üç gövdeyi okumadım; sicilin bu satırları hakkında yargı bildirmiyorum |
| **W8c** | ❌ **sicilde YOK** | `…SecurityTests.cs:947`: XML yorumu `|Learning(d)|` diyor, kod negatifi mutlak değere çevirmiyor, exception atıyor. Doküman↔kod ayrışması — §6'nın tam konusu |

**§6'nın genel kalitesi, §2-§5'ten yüksektir.** FINDING satırları gövdelerle iyi örtüşüyor;
tek yapısal sorunu W7c'nin fazla-genellemesi ve W8c'nin eksikliğidir.

---

## 6. Katıldığım noktalar — saldırdım, kıramadım

Sadece hata avlamak istemedim; sicilin **hangi kısımlarının sağlam** olduğunu da ölçtüm.

1. **Beş dosyada envanter tam ve doğru.** `AdversarialAuditTests` (5), `MemoryTests` (18),
   `InvariantTests` (13), `SchedulerGateTests` (11), `AuditFixed` (2) — hepsi bire bir
   tutuyor. Hiçbir kusur ID'si uydurulmamış, bu dosyalarda hiçbiri atlanmamış.
2. **Okuduğum 26 gövdenin 22'sinde ad ile gövde tam örtüşüyor** (§2.4 tablosu). Yani
   sicilin dayandığı "test adı bir hüküm taşır" varsayımı, **çoğunlukla geçerlidir**.
   Bu önemsiz bir sonuç değil: sicilin yöntemi (adlardan türetme) prensipte savunulabilir
   çıktı — kırıldığı yer yöntem değil, tek bir dosyanın kodlaması oldu.
3. **§0'ın epistemik çerçevesi doğru ve nadirdir.** "373/373 geçiyor = 68 kusur
   kanıtlanmış durumda, kusur yok değil" ayrımı; "yeşil panel burada sağlık değil,
   envanter" cümlesi. Bu, çoğu projenin yapmadığı bir dürüstlük hamlesidir.
4. **§7'nin kalıp 3'ü (zaman) tam doğrudur** — 5 üye, tek karar, hepsi kapanır (§4.3d).
5. **§7'nin token ↔ reflection ayrımı doğrudur.** İmzalı gate-token'ın E5/W3c'yi
   kapatmayacağını sicil biliyor ve §8.3'te ayrı bir mimari borç olarak yazmış. Bunu
   kırmaya çalıştım (§4.3a); kıramadım — sicil haklı.
6. **§3.4'teki `W2_R5 + W2_R2` sentezi** (`:134-136`) sicilin en iyi analitik katkısıdır:
   iki kusurun **birleşimi**nin, tek tek toplamlarından daha ağır olduğunu gösteriyor.
   Bu, test adlarından mekanik türetmeyle **elde edilemez** — gerçek muhakemedir.
7. **§4.2'deki C1/W6d notu** (`:176-179`) — "demo kendi dürüst bulgusuna kurban gitti,
   öz-eleştiri de doğrulanmadan kanıt değildir" — hem doğru hem de sicilin kendi
   denetlenme gerekçesidir. Sicil bu cümleyi yazarak kendi denetimini talep etmiş.
8. **§9 dürüstlük notunun 4 maddesi de dürüsttür** ve hiçbiri yanlış değildir. Yalnızca
   **eksiktir** (envanter riski işaretlenmemiş).
9. **W7c'nin olgusal çekirdeği doğrulandı** — `DecisionEntropy.cs:48` iddia edildiği
   gibidir (§5.1). Sicil burada kaynak uydurmamıştır.
10. **Şiddet sapmaları iki yönlüdür** (§3.10) — "yazar kendi kodunu koruyor" hipotezini
    ciddiye aldım ve **doğrulayamadım**. Sicilin sahibi, kendi kodunu koruyan bir yazar
    gibi değil, kalibre edilmemiş bir ölçek kullanan bir yazar gibi davranmış.

---

## 7. NİHAİ YARGI: **KISMEN GÜVENİLİR**

`survives` değil, `refuted` da değil: **`wounded`**.

**Neden `refuted` değil:** sicil, 75 kusurun 68'ini doğru ID, doğru dosya:satır ve
çoğunlukla doğru özetle listeliyor. Okuduğum gövdelerin %85'i adıyla örtüşüyor. Kaynak
uydurulmamış, kusur gizlenmemiş, yöntem beyan edilmiş. Bir indeks olarak **işe yarar** ve
yokluğundan iyidir.

**Neden `survives` değil — üç kırılma:**

| # | Kırılma | Kanıt |
|---|---|---|
| **1** | **Tam kapsam iddiası yanlış.** 7 DEFECT + 1 FINDING sicilde hiç yok; gerçek sayı 68 değil **75**. Atlananlar arasında kernel'in muhtemelen en ağır kusuru (**W8d**) var. | §2.1, BULGU-1 |
| **2** | **Şiddet sütunu kalibre edilmemiş ve kendi tanımına uymuyor.** §2'nin "özel yetki gerekmiyor" ölçütü 7 üye için yanlış; 5 kusur aşağı, 3 kusur yukarı sapmış; bir kusurun (W3c) sicildeki şiddeti, testi yazan ajanın kendi yazılı değerlendirmesiyle **çelişiyor**. | §2.3, §3.1–3.8 |
| **3** | **§7 yanlışlanamaz biçimde yazılmış ve sayıları tutmuyor.** Üyelik listesi yok (Madde X); kalıp 4 ve 5 fazla sayılmış, kalıp 2 eksik; "33 kapanır" iddiası ~29-31'e iner; ve kaynakta **adı geçen** bir 7. kalıp ("girdi kapısı var, çıktı kapısı yok") ile 8. bir kalıp ("öz-beyan kalibre edilmemiş", ≥8 kusur) tamamen atlanmış. | §4.0–4.4 |

**Sicilin güvenilirlik profili (bölüm bazında):**

| Bölüm | Yargı |
|---|---|
| §0 (çerçeve) | ✅ **güvenilir** |
| §1 (özet tablo) | ❌ **güvenilmez** — `SecurityTests` satırı 19 diyor, 26 |
| §2 (Kritik) | ⚠️ **kısmen** — 14 satırın 13'ü gerçek kusur; ortak-özellik cümlesi yanlış; H1 yanlış kovada |
| §3 (Yüksek) | ✅ **büyük ölçüde güvenilir** — 2 şiddet itirazı (W2_R2, W2_R4 → K), 1 kategori hatası (W2_L4) |
| §4 (Orta) | ⚠️ **kısmen** — W7 fazla düşük; kalanı sağlam |
| §5 (Düşük) | ❌ **güvenilmez** — sayı yanlış (6 vs 4) ve W16 kritik bir kusurdur |
| §6 (FINDING) | ✅ **en güvenilir bölüm** — tek sorun W7c'nin fazla-genellemesi + W8c eksikliği |
| §7 (kök neden) | ❌ **güvenilmez** — yanlışlanamaz, sayıları tutmuyor, 2 kalıp eksik |
| §8 (bilerek açık) | ✅ **güvenilir** — 5 maddenin hepsi gövdelerle destekleniyor |
| §9 (dürüstlük) | ⚠️ **dürüst ama eksik** — en büyük riski (envanter) işaretlememiş |

**Tek cümlelik hüküm:** DEFECT-REGISTER, kusurları **gizlemeyen** ama kendi kapsamını ve
şiddet ölçeğini **doğrulamamış** bir belgedir; bir *bulunabilirlik indeksi* olarak
kullanılabilir, bir *önceliklendirme temeli* olarak **kullanılamaz**.

---

## 8. Sicilin sahibine talepler

Kapıyı geçmek için gerekenler, öncelik sırasıyla:

**T1 (bloke edici) — envanteri makineyle yeniden üret.**
`AdversarialWave_SecurityTests.cs`'i UTF-8'e çevir (ya da `rg --encoding utf-16le` kullan)
ve **tüm** dosyalarda `AUDIT_DEFECT_` / `AUDIT_FINDING_` sayımını komutla üret. Sicile o
komutu ve çıktısını yaz. Şu 8 satır eklenmeli: **W5d, W5e, W5f, W7h, W8a, W8b, W8d
(DEFECT), W8c (FINDING)**. Başlıktaki sayı **68 → 75**, FINDING **8 → 9**.

**T2 (bloke edici) — W8d'ye şiddet ata ve gerekçelendir.**
`…SecurityTests.cs:958-974` tek bir öz-beyan sayısının P5+model seçimi+P7'yi birlikte
kapattığını gösteriyor. Bu kusur K değilse, K tanımı yeniden yazılmalıdır.

**T3 (bloke edici) — §7'ye üyelik listesi ekle.**
Her kalıbın **hangi kusur ID'lerini** içerdiği tek tek yazılmalı. Liste olmadan "33 kapanır"
Madde X'e aykırıdır. Liste yazılınca kalıp 4 (W10 çıkar → 4), kalıp 5 (→ 2) ve kalıp 2
(W1a–W1d + W7h girer → ≥11) kendiliğinden düzelecektir.

**T4 (bloke edici) — kalıp 5'i "kapanan 33"ten çıkar.**
Sicilin kendi §8.3'ü bunu kapsam kararı olarak tanımlıyor; kapsam dışına almak kapatmak
değildir.

**T5 — 7. ve 8. kalıbı ekle.**
Kalıp 7: *"Girdi kapısı var, çıktı kapısı yok"* (W8a, W8b, W17, H4, W5e, W3 — 6 kusur;
karar: çıktı postcondition kapısı / `Measured<double>`). Bu isim benim değil, kaynakta
zaten var: `…SecurityTests.cs:927`.
Kalıp 8: *"Öz-beyan kalibre edilmemiş"* (W8d, W7, W7d, B2, B1, D1_residual, G5, W2_P4 —
8 kusur; karar: confidence provenance/kalibrasyon portu, ENS-3022 borcu).

**T6 — şiddet sütununu tanımına göre yeniden uygula.**
§2'nin ölçütü ("saldırgana özel yetki gerekmiyor") ya uygulanmalı ya değiştirilmeli.
Uygulanırsa E5, W3c, W2c, W1a–W1d §2'den çıkar; W16, W2_R4, W2_R2, W7, W8d girer.
W3c için, testi yazan ajanın kendi "düşük şiddet" değerlendirmesi (`…SecurityTests.cs:328`)
ya kabul edilmeli ya da neden reddedildiği yazılmalı.

**T7 — H1'i FINDING'e taşı ya da satırı gövdeye indirge.**
Mevcut ifade (`3 satırda otomatik uygulanabiliyor`), gövdenin kanıtlamadığı bir kusur
ilan ediyor (`AdversarialAuditTests.cs:1020-1033`).

**T8 — §6/W7c'ye W7a karşı-ağırlığını ekle.**
`ROADMAP.md:248-251` bunu zaten doğru yapıyor; sicil ROADMAP'in gerisinde kalmış.
**ROADMAP düzeltmesi geri alınmamalıdır** — doğrudur.

**T9 — `D1a`/`D1b` ID'lerini kaynaktaki adlarla değiştir**
(`AUDIT_DEFECT_D1_residual_premises_are_still_uncalibrated_free_text` ve
`…_only_the_commitment_atom_emits_a_trace`).

**T10 — §1 ve şiddet dağılımı tablolarını yeniden say.**
Mevcut dağılım (14/27/21/6) dört kovada da yanlıştır (gerçek: 17/25/22/4 — üstelik
T1'den sonra bunlar da değişecek). Toplamı tutan ama bileşenleri tutmayan bir tablo,
sayılmadığının kanıtıdır.

---

## 9. Bu denetimin kendi sınırları (Madde X, kendime uygulanmış)

- **`dotnet test` çalıştırmadım.** Tüm bulgularım kaynak okumasına dayanıyor. "373/373
  geçiyor" iddiasını doğrulamadım. Eğer bu testlerden bazıları aslında **kırmızıysa**,
  hem sicil hem bu denetim yanlış olur.
- **26 gövde okudum, 68'in tamamını değil.** §2.4'te listelenmeyen kusurlar hakkında
  "ad ile gövde örtüşüyor" demiyorum — **hiçbir şey demiyorum**. Özellikle
  `MemoryTests.cs`'in 18 kusurundan yalnızca sicil özetlerini okudum, gövdelerini değil.
  Orada da uyuşmazlık olabilir.
- **Kalıp üyelikleri benim çıkarsamamdır.** §7'de üyelik listesi olmadığı için §4.1'in
  "gerçek" sütunu bir yorumdur. Sicilin sahibi farklı bir üyelik iddia ederse, §4.1'in
  sayıları değişir — ama §4.0'ın itirazı (liste yok = yanlışlanamaz) **değişmez**.
- **Kalıp 7 ve 8'in üye listeleri de benim önerimdir**, kanonik değildir.
- **W8d'nin "en ağır kusur" nitelemesi bir yargıdır**, ölçüm değil. Gerekçem: tek girdiyle
  üç mekanizmanın (dikkat, model, gate) birlikte düşmesi. Karşı-argüman: `confidence=1.0`
  meşru bir değerdir ve kusur "kalibrasyon yok"tur, "kapı yok" değil. Bu itirazı
  ciddiye alıyorum; yine de kaydedilmemesi savunulamaz.
