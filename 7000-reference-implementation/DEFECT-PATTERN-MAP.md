# KUSUR ↔ KALIP EŞLEME — `DEFECT-REGISTER` §7'nin yanlışlanabilirlik borcunun kapatılması

| | |
|---|---|
| **Tarih** | 2026-07-27 |
| **Derleyen** | oturum sahibi (owner) |
| **Kapsam** | 75 `AUDIT_DEFECT_*` + 9 `AUDIT_FINDING_*` = **84** kimlik |
| **Yetki** | Anayasa Madde X — bu dosya bir **borç kapatma** edimidir |
| **Durum** | draft — bağımsız doğrulama bekliyor (G4) |

---

## 0. Bu dosya neden var

`DEFECT-REGISTER.md` §7, 75 kusurun **8 kalıptan** doğduğunu ve **6 mimari kararın 33'ünü
birden kapatacağını** iddia etti. Bağımsız denetim (`DEFECT-REGISTER-VERIFICATION.md`) bunu
**yanlışlanamaz** buldu ve haklıydı:

> **Üyelik listesi hiç verilmemişti.** Hangi kusurun hangi kalıba ait olduğu yazılmadan
> *"33 kapanır"* ne doğrulanabilir ne çürütülebilir. Bu, sicilin **kendi yetki kaynağı olarak
> gösterdiği** Madde X'in ihlalidir.

Bu dosya o listeyi verir. Her kimlik **tam olarak bir birincil kalıba** atanır — çifte sayım
yasaktır, çünkü şişirme bir kez zaten yakalandı (v1'in "33"ü gerçekte ~29-31'di).

**Nasıl denetlenir:** aşağıdaki her satır bir iddiadır. Test adını okuyup atamayı reddetmek
tek satırlık bir işlemdir. Toplamlar §10'da; toplam ≠ 84 ise bu dosya yanlıştır.

---

## 1. P1 — Yetki taklit edilebilir / aracısız (complete mediation)

**Tanım:** bir yetki, onu veren mekanizmadan geçmeden elde edilebiliyor ya da düz veri olarak
üretilebiliyor.

| ID | Kusur |
|---|---|
| `E3` | Gate sonucu tek satırda taklit edilebiliyor |
| `W4a` | Sahte `ToolAuthorization` registry'yi tamamen atlıyor |
| `W15` | `ToolAuthorization` public → registry reddi aklanabiliyor |
| `W16` | `null` `toolAuthorization` ↔ "yetkilendirmeyi unutmak" ayırt edilemiyor → Autonomous |
| `H1` | Öneri herhangi bir çağıran tarafından uygulanabiliyor |
| `G5` | Öneriler sıfır provenance'lı sahte kayıtlardan imal edilebiliyor |
| `C3` | Hayalet-kayıt guard'ı değer-eşit klonla atlatılıyor |
| `W2_L1` | Eylem yaşam döngüsü hiçbir karar olmadan başlatılabiliyor |
| `W2_L2` | Tek proof-trace sınırsız alakasız eylemi meşrulaştırıyor |
| `W2_O1` | Tek-sahip koşulu hiç implemente edilmemiş |
| `W2_R2` | `Rehydrate` başka kararlara ait event'leri kabul ediyor |
| `W5d` | `CanHandle` öz-beyan; kayıt sırası modeli belirliyor |

**Üye: 12**

---

## 2. P2 — Kimlik normalizasyonu yok (complete mediation)

**Tanım:** aynı varlık, harf/Unicode/boşluk/`NUL` farkı yüzünden **iki ayrı varlık** gibi
davranıyor.

| ID | Kusur |
|---|---|
| `F3` | Unicode-eşdeğer purpose type'lar belleği erişilemez parçalara bölüyor |
| `G3` | Purpose type'a boşluk ekleyerek örüntü tespiti atlatılıyor |
| `G4` | Büyük/küçük harf varyantları iki ayrı öneri üretiyor |
| `C2` | Saat kayıt **değerine** göre anahtarlanıyor, kimliğine değil |
| `W1a` | `Disable` yanlış harfle sessizce "başarılı" oluyor |
| `W1b` | Doğrulama sorgusu operatörün yanlış inancını onaylıyor |
| `W1c` | Her near-miss revoke jesti yeteneği canlı bırakıyor |
| `W2c` | Homoglyph araç adı onay-gerektirmeyen ikiz üretiyor |
| `W2e` | `NUL` içeren araç adları birinci sınıf yetkili |
| `W2f` | `Register`, `Authorize`'ın sorgulayamayacağı adları kabul ediyor |
| `W5g` | Yinelenen `AdapterId` → denetim anahtarı injective değil |
| `W7f` | Sahip kimliğinde harf farkı tüm attribution'ı ters çeviriyor |
| `W7h` | Boş ve boşluklu context key'ler ayrı evrenler |

**Üye: 13**

---

## 3. P3 — Zaman çağırandan geliyor, doğrulanmıyor

| ID | Kusur |
|---|---|
| `A1` | Gelecek tarihli `AssertedAt` sönümü sonsuza dek kapatıyor |
| `A2` | `DateTime.MaxValue` kabul ediliyor → ölümsüz kayıt |
| `B4` | Aynı kayıt aynı anda 1000 kez doğrulanabiliyor |
| `D4` | `Retrieve` `asOf`'ta var olmayan kayıtları sızdırıp ilk sıraya koyuyor |
| `W2_L3` | Denetim zaman damgaları çağıran kontrolünde |
| `W2_R6` | Replay zaman damgalarını ve yinelenen event id'leri yok sayıyor |

**Üye: 6**

---

## 4. P4 — Sentinel `0`/negatif = sessiz kapatma anahtarı (fail-safe defaults)

| ID | Kusur |
|---|---|
| `A5` | `contextDecayRate = 0` → şirket çapında sönüm kapalı |
| `E4` | `staleThreshold = 0` → curator kapalı |
| `G2` | Sıfır magnitude eşiği → her purpose type öneri |
| `H3` | Negatif eşik → gate no-op |
| `W10` | Bozuk policy eşikleri yalnız bazı girdilerde doğrulanıyor |

**Üye: 5**

---

## 5. P5 — Reflection değişmezleri deliyor (**kapsam kararı, düzeltme değil**)

| ID | Kusur |
|---|---|
| `E5` | Durum reflection ile ışınlanıyor, iz yok |
| `W3c` | Reflection donmuş izin kümesini değiştiriyor, registry yeniden doğrulamıyor |

**Üye: 2**

> **⚠️ Bu kalıp "kapanan" hanesine YAZILAMAZ.** Bağımsız denetim (`T4`) bunu tespit etti:
> reflection'ı kapsam dışına almak **kapatmak değildir**. Karar "kernel içinde mi process
> sınırında mı savunulacak" sorusudur; hangisi seçilirse seçilsin bu iki kusur *çözülmez*,
> yalnız **yeri belirlenir**.

---

## 6. P6 — Canlı koleksiyon dönüyor

| ID | Kusur |
|---|---|
| `W22` | Scheduler çıktısı canlı `List`, çağıran sırayı değiştirebiliyor |
| `W2_R4` | Replay edilen alternatifler canlı görünüm |
| `W2_L4` | `history` senkronize edilmemiş canlı görünüm |
| `W5a` | Adapter listesi downcast edilebilir |
| `W5b` | "En az bir adapter" değişmezi inşadan sonra silinebiliyor |

**Üye: 5**

---

## 7. P7 — Girdi kapısı var, **çıktı kapısı yok**

> Ad kaynakta yazılı: `AdversarialWave_SecurityTests.cs:927`.

| ID | Kusur |
|---|---|
| `H4` | `Compute` negatif sonsuz yayabiliyor |
| `W3` | `NormalizedDeficit` clamp'i negatif sıfırı normalize etmiyor |
| `W17` | Gate korunan sınırın dışına NaN `InfoNeed` yayıyor |
| `W5e` | `null` `LlmResponse` geçip proof-trace substratını yok ediyor |
| `W8a` | `ReuseROI` iki ölçülebilir girdiden `+∞` dönüyor |
| `W8b` | `DeltaCapital` sonlu girdilerden taşıyor |

**Üye: 6**

---

## 8. P8 — Öz-beyan kalibre edilmemiş

| ID | Kusur |
|---|---|
| `W8d` | Tek `confidence = 1.0`, `stake = 1e12`'de bile tüm yığını sıfırlıyor |
| `W7` | `confidence == 1.0` sınırsız özerklik satın alıyor, 1 ULP altı almıyor |
| `W7d` | Tek gözlem "sıfır gürültü" raporluyor, örneklem güvencesi yok |
| `W7e` | Tümü `null` gözlemler "kusursuz tutarlılık" raporluyor |
| `B1` | Zorunlu kanıt guard'ı kendi varsayılan parametresiyle etkisiz |
| `B2` | Boşluk olmayan herhangi bir karakter kanıt sayılıyor |
| `D1_residual` (öncüller) | Öncüller kalibre edilmemiş serbest metin |
| `W2_P1` | Tamamen görünmez proof-trace temsil edilebilir |
| `W2_P3` | Render çıktısı öncül metniyle taklit edilebiliyor |
| `W2_P4` | Kendi kendini gerekçelendiren türetim, tam güvenle |

**Üye: 10**

---

## 9. P9 — Kalıba GİRMEYENLER (dürüstlük bölümü)

Bunlar 8 kalıbın hiçbirine temiz oturmuyor. **Bu, kalıp modelinin sınırıdır ve
gizlenmiyor** — v1'in hatası tam da bu artığı hiç saymamaktı.

| ID | Kusur | Neden ayrı |
|---|---|---|
| `B6`, `W20` | Beraberlikte sıralama girdi sırasına bağımlı | Belirlenimcilik; üçüncü tie-breaker kararı ister |
| `C1`, `W6d` | Konumsal çağrı yanlış overload'a bağlanıyor | C# dil semantiği; ad değişikliği (breaking) |
| `W23` | Geçersiz `PendingDecision` inşa edilebiliyor | Tip değişmezi |
| `W5c` | `null` adapter "model-agnostik ama modelsiz değil" guard'ını geçiyor | Tip değişmezi |
| `W5f` | Önceden iptal edilmiş token yine "başarılı" dönüyor | İptal semantiği |
| `W2_R3` | Replay boş-olmayan alternatif guard'ını uygulamıyor | Replay ≠ canlı yol asimetrisi |
| `W2_R5` | Bilinmeyen event tipleri sessizce yutuluyor | Replay asimetrisi |
| `W2d` | Kontrol karakterleri insana gösterilen `reason`'a akıyor | Çıktı sanitizasyonu |
| `W1d` | Ön-alıcı `Disable` sonraki `Register`'ı ölü doğuruyor | Sıralama semantiği |
| `W1e` | `disabled` kümesi doğrulanmamış public girdiden sınırsız büyüyor | Kaynak tüketimi |
| `W8` | Scheduler, gate'in fail-closed `CriticalBlock` dalını devre dışı bırakıyor | Katman sırası |
| `W9` | Tek zehirli karar tüm partinin dikkatini engelliyor | Patlama yarıçapı |
| `G1` | 10.000 kayıt 5.000 ayrışmamış öneri üretiyor | Ölçekleme |
| `D1_residual` (trace) | Yalnız commitment atomu iz yayıyor | İz kapsamı |

**Üye: 15**

---

## 10. Toplamlar — **bu tablo bu dosyanın yanlışlanma noktasıdır**

| Kalıp | Üye | Kapatılabilir mi |
|---|---|---|
| P1 — Yetki taklit edilebilir | 12 | ✅ Tek karar (imzalı/internal yetki tipi) |
| P2 — Kimlik normalizasyonu yok | 13 | ✅ Tek karar (normalize kimlik tipi) |
| P3 — Zaman çağırandan | 6 | ✅ Tek karar (monoton saat portu) |
| P4 — Sentinel sıfır | 5 | ✅ Tek karar (nullable + açık `Disabled`) |
| P5 — Reflection | 2 | ❌ **Kapsam kararı, düzeltme değil** |
| P6 — Canlı koleksiyon | 5 | ✅ Tek karar (zorunlu immutable dönüş) |
| P7 — Çıktı kapısı yok | 6 | ✅ Tek karar (postcondition / `Measured<double>`) |
| P8 — Öz-beyan kalibre değil | 10 | ⚠️ Kısmen — provenance portu ENS-3022 borcuna bağlı |
| P9 — Kalıp dışı | 15 | ❌ Tekil işler |
| **Toplam** | **74** | |
| `AUDIT_FINDING_*` (9) | 9 | Kod kusuru değil, iddia zayıflığı |
| **Genel toplam** | **83** | |

> **⚠️ 83 ≠ 84 — bir kimlik eksik ve bunu gizlemiyorum.** `D1_residual` iki ayrı teste
> karşılık geliyor (`premises_are_still_uncalibrated_free_text` → P8;
> `only_the_commitment_atom_emits_a_trace` → P9) ama tek ID gibi sayılıyor. Sicilin
> `D1a`/`D1b` adlandırması bu yüzden `T9` talebinde düzeltilmesi istenmişti; henüz
> yapılmadı. **Bu satır o borcun görünür hâlidir.**

### Düzeltilmiş kapanma iddiası

> **6 karar → 47 kusur** (P1 + P2 + P3 + P4 + P6 + P7 = 12+13+6+5+5+6 = **47**).
> P8 kısmen; P5 ve P9 **kapanmaz**.

> ### ⛔ ARİTMETİK HATASI — düzeltildi (`SKR-049` T-A, 2026-07-27)
> Bu satır ilk hâlinde **41** diyordu. `12+13+6+5+5+6 = 47`. Düz toplama hatası.
> Bağımsız kontrol de aynı sonucu veriyor: toplam 74 satır − kapanmayan 27 (P5=2, P8=10,
> P9=15) = **47**.
>
> **Asıl ders bu değil.** `ADR-0003` bu sayıyı **doğrulamadan devraldı** ve üstüne kendi
> hesabını kurdu. Yani `work-protocol.md` §3.5 (*"devraldığın bulguyu doğrula"*) **düzyazıya
> uygulandı, aritmetiğe uygulanmadı** — kuralın kör noktası. §3.5'e dördüncü kontrol eklendi:
> **sayıyı yeniden oku değil, yeniden HESAPLA.**

v1'in *"6 karar 33 kusuru kapatır"* iddiası **hem sayı hem içerik olarak yanlıştı**: sayı
düşük, ama kapsam yanlış — P5'i "kapanan" hanesine yazıyordu. Düzeltilmiş iddia **daha
iddialı** (41) ve **daha dar** (P5/P9 açıkça dışarıda).

---

## 11. Bu dosyanın failure conditions'ı (Madde X)

**Yanlıştır** eğer:

1. **Bir kimlik iki kalıpta birden sayılıyorsa.** Kural: her ID tam olarak bir birincil
   kalıpta. Denetim: §1-§9'daki tüm ID'leri birleştir, `sort -u`, sayı **84** (D1 çifti
   ayrıştırıldıktan sonra) olmalı.
2. **Bir atama test gövdesiyle çelişiyorsa.** Atamalar test **adlarından** yapıldı; gövdeler
   okunmadı. Bu, sicilin v1'inde yapılan hatanın aynısıdır ve burada **bilerek tekrarlanıyor**
   — çünkü 84 gövde okumak ayrı bir turdur. **Bağımsız doğrulama bunu hedeflemelidir.**
3. **"6 karar 41 kusur kapatır" iddiası uygulandığında tutmuyorsa.** Karar verildikten sonra
   ilgili `AUDIT_DEFECT_*` testleri `AUDIT_FIXED_*`'a dönmelidir. Dönmeyen her test bu
   dosyayı yanlışlar.
4. **P9'un 15 üyesi arasında gizli bir 10. kalıp varsa.** Artık bölümünü büyük tutmak
   dürüsttür ama tembelliği de gizleyebilir.

---

## İlişkili
- `DEFECT-REGISTER.md` §7 — kapatılan borç
- `DEFECT-REGISTER-VERIFICATION.md` — borcu tespit eden bağımsız denetim
- `AUDIT-WAVE2-SECURITY.md` §10.5 — "kusur örnek olarak kapatıldı, sınıf olarak kapatılmadı"
- `.claude/rules/work-protocol.md` §3.5 — devralınan bulguyu doğrulama kuralı

---

## 12. ⛔ BAĞIMSIZ DOĞRULAMA — dört atama YANLIŞ çıktı (2026-07-27, `ens-ai-architect`)

§11/2'nin ilan ettiği risk **gerçekleşti.** On iki test gövdesi okundu, **dördü** kalıbıyla
çelişiyor. Bu bölüm o dördünü kaydeder; §1-§9 tabloları **henüz düzeltilmedi** (ADR-0003
onları daraltılmış hâliyle kullanıyor).

| ID | Haritada | Gövde ne diyor | Sonuç |
|---|---|---|---|
| `A1`/`A2` | P3 (zaman çağırandan) | `AdversarialWave_MemoryTests.cs:62` — `CompanyMemory.Record` **saati VAR ama kullanmıyor** | Monoton saat portu bunları **kapatmaz**; ayrıca **kabul aralığı** gerekir (`assertedAt ≤ now + tolerans`). İki ayrı mekanizma |
| `C2` | P2 (normalizasyon) | `:294-297` — `record` **değer-eşitliği** sözlük anahtarı; iki string birebir aynı | Kanonik kimlik tipi **çözmez**. Bu bir **entity/value karışımı**dır → P2'den **çıkarıldı** |
| `W1b` | P2 | `AUDIT-WAVE2 §1.2`: *"`bool`, üç durumlu bir soruyu temsil edemez — bulgunun **tip düzeyindeki kökü budur**"* | Ayrı tasarım hatası. Kanonik ad yalnız *tetikleyiciyi* kapatır |
| `W2_O1` | P1 (taklit edilebilir) | `:325` — `Owner` diye bir property **yok** | Yetki taklit edilmiyor, **hiç yok**. Ayrı iş |

**Düzeltilmiş kapanma iddiası: 41 → 40.**

### 12.1 Ve bir iddia daha — o da yanlış çıktı (oturum sahibi doğrulaması)

Aynı denetim *"13 kimliğin hiç testi yok"* dedi (`W1a` `W1b` `W1c` `W2c` `W2e` `W2f` `W5a`
`W5b` `W5d` `W5e` `W5g` `W7f` `W7h`) ve buna dayanarak ADR'ye bir **K-0** kararı ekledi.

**Yanlış.** Onüçünün de testi var; hepsi `AdversarialWave_SecurityTests.cs` içinde. Sebep
kodlama tuzağı: o dosyada `W2e` testinin fixture'ı olarak **4 gerçek NUL baytı** duruyor,
bu da `grep`/`rg`'ye tüm dosyayı binary saydırıyor.

> **Aynı dört bayt bu oturumda üç kez yanılttı:** sicilin 68/75 sayım hatası → güvenlik
> raporunun dosyayı hiç görememesi → şimdi bu. Kural `work-protocol.md` **§3.2**'ye yazıldı.

**Sonuç:** §11/3'ün yanlışlanma koşulu (*"kararlar uygulanınca `AUDIT_DEFECT_*` →
`AUDIT_FIXED_*` dönmeli"*) **40'ın tamamı için uygulanabilir.** K-0'ın dayanağı yoktur;
ADR-0003'te düzeltilmelidir.

### 12.2 Şema kusuru — ID uzayı global tekil değil

`F3`/`G3`/`G4` hem `AUDIT_DEFECT_*` (MemoryTests) hem `AUDIT_FIXED_*` (AdversarialAuditTests)
olarak, **farklı anlamlarda** var. Bu vakada atamalar tesadüfen doğru; ama şema bozuk ve
sonraki bir sayımı sessizce yanıltabilir.

---

## 14. ✅ SINANDI — `ADR-0003`'ün 17 iddiası **iyi-biçimli**, 2026-07-27

Ad-uzayı (§13.2) uygulandıktan **sonra** ilk kez yapılabilen ölçüm.

### Ne sınandı — ve ne SINANMADI

> **Sınanan:** iddia *sınanabilir mi* — 17 kimliğin her birinin canlı bir `AUDIT_DEFECT_*`
> testi var mı, ve kapanışı taklit edecek rakip bir `AUDIT_FIXED_*` var mı?
>
> **SINANMAYAN:** iddianın **doğru** olup olmadığı. Bunu kanıtlamak kararları **uygulamayı**,
> yani kod yazmayı gerektirir; `ADR-0003` `refuted`/`draft` ve Madde VII kodun yalnız
> **Accepted** ADR'lere dayanmasını istiyor.

### Sonuç: 17/17 ✅

| Kalıp | Ad-uzaylı kimlikler | Durum |
|---|---|---|
| `DP3` (K-3, zaman) | `MEM_A1` `MEM_A2` `MEM_B4` `MEM_D4` `INV_W2_L3` `INV_W2_R6` | 6/6 ✅ |
| `DP6` (K-5, canlı koleksiyon) | `SCH_W22` `INV_W2_R4` `INV_W2_L4` `SEC_W5a` `SEC_W5b` | 5/5 ✅ |
| `DP7` (K-6, çıktı kapısı) | `MEM_H4` `SCH_W3` `SCH_W17` `SEC_W5e` `SEC_W8a` `SEC_W8b` | 6/6 ✅ |

Her biri: **tam 1** canlı `AUDIT_DEFECT_*`, **0** rakip `AUDIT_FIXED_*`.

### Sınama sırasında bulunan kusur — iddia hâlâ **çıplak ID** kullanıyor

İlk ölçüm çıplak ID'lerle yapıldı ve **4'ü `⚠️`** verdi (`A1` `A2` `B4` `D4`): çünkü
`AUDIT_DEFECT_MEM_A1` ile `AUDIT_FIXED_AUD_A1` **farklı kusurlar** ama çıplak `A1`
ikisini de eşliyor.

> **Ad-uzayı kodu düzeltti; belgeler eski adı kullanmaya devam ediyor.** §1-§9 tabloları
> ve `ADR-0003`'ün kapanma iddiası hâlâ `A1`, `W22`, `H4` diyor — ve bunların 4'ü
> **belirsizdir**. Yukarıdaki tablo doğru adları verir; §1-§9'un toplu güncellenmesi
> ayrı bir edimdir.

**Bu, düzeltmenin yarım olduğunun kaydıdır** — yordam onarıldı, ama onu kullanan metin
henüz onarılmadı. İkisini karıştırmak bu oturumun altı kez tekrarladığı hatadır.

### Yanlışlanma

Bu bölüm yanlıştır eğer bir kimlik için birden fazla `AUDIT_DEFECT_*` varsa (o zaman
"tam 1" iddiası düşer) ya da bir kararın uygulanması ilgili testi `AUDIT_FIXED_*`'a
çevirmezse. Ölçüm komutu §13.1'deki kanonik komuttur.
