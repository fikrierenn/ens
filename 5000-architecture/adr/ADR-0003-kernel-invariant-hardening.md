---
id:            ADR-0003
title:         Kernel Invariant Hardening — altı kalıp-kapatıcı karar
type:          adr
canon:         false
origin:        DEFECT-PATTERN-MAP.md (P1-P9 kalıp eşlemesi), AUDIT-WAVE2-SECURITY.md §10.5
depends_on:    [ADR-0001, ENS-0000]
referenced_by: []
realizes:      []
principles:    [P1, P5, P6, P7, P8]
status:        draft
owner:         ens-ai-architect
version:       0.6.0
last_reviewed: 2026-07-27
maturity:      M0
skeptic_review: [SKR-049, ENG-0001, SKR-050, ENG-0002, SKR-051]   # SKR-049: wounded (T-A..T-E) · ENG-0001: koşullu
                          # SKR-050 (v0.3.0, 2. tur): wounded — 5 bloke edici (T-A..T-E).
                          # En ağırı T-A: §0.7'nin 9 kararından 7'si gövdeye UYGULANMADI;
                          # belge kendi kararlarıyla çelişiyor. Manşet 43 savunulamaz
                          # (yeniden hesap: 40 koşulsuz + 4 koşullu). status: draft KALIR
                          # (skeptic-challenged önerilir; statü değişimini owner onaylar)
                          # SKR-051 (v0.5.0, 3. tur): REFUTED — 8 bloke edici (T-1..T-8).
                          # En ağırı T-1: BÖLME İLAN EDİLDİ, GÖVDEYE UYGULANMADI — K-1/K-2
                          # yedi yerde normatif (§3, §4.1, §4.2, §6, §7:1377, §8:1417,
                          # künye provides:). ADR-0004:106 "devretti" diyor; ADR-0003 devretmemiş.
                          # Accepted olursa iki ADR aynı kararı çelişik güçte iddia eder → Madde VII.
                          # Ayrıca: sonlanma ölçütü SCAN-03:433-436 ile ampirik olarak çürük;
                          # K-5 tur 2'de HİÇ incelenmedi (yokluğu kanıt saymak); K-4'ün "tuttu"su
                          # cherry-pick (ENG-0002 E-1.7 K-4'e yeni bloke edici verdi); 22 doğru
                          # türetiliyor AMA beşinci sayıdır ve §7 hâlâ 40 diyor.
                          # status: draft KALIR — accepted ÖNERİLMEZ (statü değişimini owner onaylar)
failure_conditions: stated
evidence:      {sci: E0, eng: E1, ops: E0, econ: E0}
requires:      [ADR-0001]
provides:      [Time Acceptance Window, Explicit Policy Value, Sealed Collection, Measured Output]   # v0.6.0: Authority Token→ADR-0004, Canonical Identity→ADR-0005
consumed_by:   []
---

# ADR-0003 — Kernel Invariant Hardening

> **Durum: `draft`.** Bu belge bir **öneridir**, yürürlük değil. Kabul, Anayasa Madde XIV
> yordamıyla olur (skeptic saldırısı + hiza incelemesi + Accepted). Madde VII gereği
> `7000-reference-implementation/` bu ADR **Accepted olana kadar** buradaki hiçbir karara
> dayanamaz.

> ### ⚠️ BAĞIMSIZ SKEPTIC TURU — `wounded` (SKR-049, 2026-07-27)
> `5000-architecture/reviews/SKR-049-adr-0003-kernel-invariant-hardening.md`. Beş bloke edici
> bulgu, **kabul öncesi** kapatılmalıdır:
> **T-A** aşağıdaki §1'in "41"i **aritmetik olarak yanlış** — `12+13+6+5+5+6 = 47`; ADR'nin
> "40"ı da türetilemiyor (`W1b` çifte çıkarılmış, `C3` kapanmadığı doğrulandığı hâlde sayıda).
> Doğru manşet **43**. · **T-B** `AdversarialWave_SecurityTests.cs:27-29` künyesi yanlış →
> doğrulama yordamı sessizce fail-open. · **T-C** K-1'in `record` taslağı `with` ile mühür
> kopyalamaya açık → `W15`/`W4a`/`E3` geri gelir. · **T-D** K-4'ün `Disabled(..., DateTimeOffset At)`
> taslağı K-3'ü ihlal ediyor. · **T-E** `ToUpperInvariant` Türkçede `ı`/`i`'yi birleştirir,
> `İ`/`i`'yi ayırır → `G4`/`W7f`/`W1a`/`W1c` kapanışları desteklenmiyor.
> Ayrıca: `AdversarialWave_SecurityTests.cs` **altı maliyet tablosunun hiçbirinde yok**
> (44 kimliğin 13'ü orada). SKR-049 §Katıldığım noktalar, ADR'nin yanlışlanabilirlik
> disiplinini ayrıca kayda geçirir — `C3` uyarısı (§4.1) **haklı çıkmıştır**.

> ### ⛔ ÜÇÜNCÜ SKEPTIC TURU — `refuted` (SKR-051, 2026-07-27, v0.5.0)
> `5000-architecture/reviews/SKR-051-adr-0003-v050-bolunmus.md`. **Sekiz bloke edici bulgu;
> `accepted` ÖNERİLMEZ.** Verdict kararların içeriğine değil, **belgenin bugünkü durumuna**
> aittir — K-3/K-4/K-5/K-6 savunulabilir kararlardır (SKR-051 §K).
>
> **T-1 (en ağır):** §0.10'un ilan ettiği **bölme gövdeye uygulanmadı.** K-1 ve K-2 hâlâ
> §3 kapsam beyanında, §4.1/§4.2'de tam karar cümleleriyle, §6 tablosunda, §7'nin 1377.
> satırında, §8'in `// TRACE:` zorunluluğunda ve künye `provides:` alanında **normatiftir**.
> `ADR-0004:106` *"K-1'i devretti"* diyor; bu belge devretmemiş. Accepted olursa iki ADR aynı
> kararı **çelişik güçte** iddia eder (§4.1 *"referans eşitliğiyle doğrulanır"* ↔ `ADR-0004:29`
> *"hedefi küçülür, kazayla taklidi durdurur"*) → **Madde VII kırılır.** Bu, sicilin
> *"changelog ≠ gövde"* kalıbının **altıncı** tekrarıdır. ·
> **T-2** Sonlanma ölçütü `SCAN-03:433-436` ile ampirik olarak çürük (ENS'in kendi kaydında
> bir hata **iki ardışık scientific turdan** geçmiştir); ölçüt boyut-kör ve "incelenmedi" ile
> "dayandı"yı ayırt edemiyor. ·
> **T-3** K-4'ün `✅`'i cherry-pick: `ENG-0002` E-1.7 aynı turda K-4'ün `DecayPolicy.Disabled`
> varyantını kırdı (`with` ile `Approver=mallory`, derleme hatası **yok**). ·
> **T-4** K-5 tur 2'de **hiç incelenmedi** — `ENG-0002`'de K-5'e ait ölçüm bölümü yok;
> "yeni bulgu yok" burada **yokluğu kanıt saymaktır**. ·
> **T-5** Bölme K-4'ü K-1'den kopardı: §4.4'ün iz mekanizması ve R12 azaltması **K-1 mührüne**
> dayanıyor, K-1 artık `ADR-0004`'te (atomiklik ihlali). ·
> **T-6** 22 **doğru türetiliyor** (bağımsız sayım: 6+5+5+6, çakışma yok) ama gövdede
> **beşinci** sayıdır; §7:1377 hâlâ *"K-1…K-6 … 40 kimlik"* diyor. ·
> **T-7** K-3'ün analyzer koşulunun üç yüzeyinden ikisi (`#pragma` bastırma, sessiz yasak
> satırı) ADR'nin **kendi kapattığını iddia ettiği** sınıflardır (`E3`, sessiz başarı). ·
> **T-8** OQ1 açık ve ADR'nin kendi cümlesiyle *"K-5/K-6'nın tip seçimini bağlar"*.
> SKR-051 §Katıldığım noktalar, §2'nin devralınan-bulgu doğrulamasını ve K-0'ın kaldırılışını
> ayrıca kayda geçirir.

---

## 0. v0.1.0 → v0.2.0 — iki boyutun bulguları

v0.1.0 **iki bağımsız incelemeden de** bloke edici bulgu aldı. `work-protocol.md` §3.1
gereği ADR'ler iki boyut ister; bu bölüm ikisinin çıktısını kaydeder.

| İnceleme | Boyut | Verdict |
|---|---|---|
| `SKR-049` | Scientific | `wounded` — 5 bloke edici |
| `ENG-0001` | Engineering | K-5 `uygulanabilir`, diğer beşi `koşullu` — 3 bloke edici |

### 0.1 İkisinin de bulduğu (yakınsama)

| Bulgu | Nasıl bulundu |
|---|---|
| **K-1'in mührü `record` `with` ile kopyalanıyor** | `SKR-049` C# semantiğinden türetti; `ENG-0001` **ayrı assembly'den derleyip kanıtladı**: `legit with { IsAllowed = true }` → registry KABUL. `record`'un `<Clone>$` copy-constructor'ı **public**tir; `private` kurucu onu kapatmaz |
| **Türkçe casing kararı yanlış sonuç veriyor** | `SKR-049` Unicode tablolarından; `ENG-0001` **ölçtü**: `'işletme'` vs `'İŞLETME'` → `IŞLETME` ≠ `İŞLETME` |

### 0.2 Yalnız `SKR-049` (okuyarak bulunabilenler)

- **T-A — sayı yanlış.** `DEFECT-PATTERN-MAP` `12+13+6+5+5+6 = 41` yazmıştı; doğrusu **47**.
  Bu ADR onu **doğrulamadan devraldı**. §3.5 düzyazıya uygulandı, **aritmetiğe uygulanmadı**;
  kurala dördüncü kontrol eklendi (*"sayıyı yeniden hesapla"*). ADR'nin kendi "40"ı da
  türetilemiyor: `W1b` **çifte çıkarılmış**. Doğru sayı **43**.
- **T-D — K-4 kendi taslağında K-3'ü ihlal ediyor.** `Disabled(string Reason, Identity
  Approver, DateTimeOffset At)` — çağıran-verisi denetim damgası, yani `W2_L3`'ün birebir
  kalıbı. K-3↔K-4 bağı yalnız **tek yönde** görülmüştü.
- **T-B — ölçüm aletinin künyesi yalan söylüyor.** `AdversarialWave_SecurityTests.cs:27-29`
  *"hiçbir çıplak non-ASCII yok"* diyor; `:106` Kiril `а`, `:107` ZWSP, `:110` RTL, `:219`
  NFD `é` — hepsi çıplak. Semptom (4 NUL baytı) teşhis edilmişti, **kök neden** değil.
- **`P1..P9` iki anlamda:** künye `principles:` Anayasa §III'e, gövde kalıp sözlüğüne
  gönderiyor → §3 kapsam beyanının **tersi** okunuyor. `DP1..DP9` kullanılacak.

### 0.3 Yalnız `ENG-0001` (yalnız **derlenerek/çalıştırılarak** bulunabilenler)

Bu üçü okuyarak üretilemezdi — kaydı bu yüzden önemli:

- **OQ1 cevaplandı: `class`.** `required` (C# 11+) `default(T)`'yi **kapatmıyor** (ölçüldü).
  `class` varyantı `CS8618` üretiyor — ama `Ens.Kernel.csproj`'da `<WarningsAsErrors>` yok,
  yani bugün o da konvansiyon. **Faz 0'da eklenmeli.**
- **K-6'nın asıl kırılması derlemede değil, çalışma zamanında.** `Measured` `IComparable`
  uygulamıyor → `OrderByDescending` **derleniyor**, çalışırken `InvalidOperationException`
  atıyor. `Scheduler.cs:124` ve `CompanyMemory.cs:260` tam o yolda. v0.1.0'ın *"Breaking?
  Düşük"* değerlendirmesi derlemede doğru, **çalışma zamanında yanlış**.
- **M-3'ün geri çekilme yolu .NET'te YOK.** v0.1.0 yalnız `confusables.txt`'in eksikliğini
  itiraf ediyordu. Ölçüm: BCL'de **hiçbir Script API'si yok** (`CharUnicodeInfo`'nun 4 public
  static üyesinin hiçbiri, `Rune`'da da yok). `W2c` **tamamen** açık, kısmen değil.

### 0.4 Ölçülmüş gerçekler (v0.1.0'ın tahminleri yerine)

| | v0.1.0 | Ölçüm (`ENG-0001`) |
|---|---|---|
| Kernel boyutu | ~899 satır | **2421 ham / 1017 kod / 17 dosya** |
| Altı kararın dokunduğu dosya | tahmin | **17/17 (%100)**, 8'i üç+ karar tarafından |
| Test tabanı | "373" (DOĞRULANMADI) | **373/373 geçiyor, 217 ms** — bağımsız koşuldu |
| `AdversarialWave_SecurityTests.cs` | altı maliyet tablosunun **hiçbirinde yok** | **51 test**, tabanın %13.7'si |
| `R1` (mühür ↔ event-sourcing) | "fail-closed olur" | Kernel'de serileştirme **yok** (grep: sıfır) → bugün teorik. Geldiğinde `System.Text.Json` **`NotSupportedException`** atar — bu fail-closed değil, **çöküş** |
| `OQ2` çelişkisi | açık | **Dar**: `Guard.NormalizedDeficit` üretimde 2 çağrı yeri, 2 test etkileniyor. Cevap `Guard.cs:133`'te zaten var; düzeltilecek olan K-6 değil **R19'un metni** |
| `OQ6` (uygulama sırası) | yok | **Türetildi**: Faz 0 (OQ1 + tarama) → K-5 → K-6 → K-2 → K-3+K-4 birlikte → K-1 |

### 0.5 Ayakta kalanlar

- **P5/P9 sızması YOK** (`SKR-049` `survives`) — v1'in hatası tekrarlanmadı, kapsam-dışı hane
  kendi aleyhine yazılmış (R13'ün P8'i yeniden ürettiği itirafı dâhil).
- **Uydurma prior art YOK** (`SKR-049` `survives`). Tek ifade hatası: JEP 486 Security
  Manager'ı *kaldırmadı*, **kalıcı olarak devre dışı bıraktı**.
- **K-5 tek koşulsuz `uygulanabilir` karar.** K-5/K-6 için mimari tarama **yazıldı ve
  çalıştırıldı** (`Ens.Kernel.dll`): K-5 **22 ihlal**, K-6 **36 ihlal**, ~15 satır. Bu iki
  karar hem en sağlam hem **otomatik denetlenebilir** olanlar.

### 0.6 Bu bölümdeki düzeltmeler v0.2.0'da UYGULANMADI — kaydedildi

Sekiz bloke edici bulgunun **metne işlenmesi** ayrı bir edimdir ve K-1'in tip seçimi
(`sealed class` mı get-only `record` mı), K-2'nin Türkçe katlama mekanizması ve K-4'ün
zaman damgası bu ADR'nin **kararlarını değiştirir** — yani yeni bir tur ister
(`work-protocol.md` §3.1). v0.2.0 bulguları **görünür** kılar; v0.3.0 onları **karara**
dönüştürecek ve yeniden iki boyuta girecektir.

> **Bu ayrım bilinçlidir:** bulguyu kaydetmekle kararı değiştirmek aynı edim değildir.
> İkisini karıştırmak, bu oturumda `K-0`'ın yanlış önermeden karar üretmesine yol açtı.

---

## 0.7 v0.3.0 — bulgular KARARA dönüştü

v0.2.0 sekiz bulguyu **görünür** kıldı ve kararlara dokunmadı (§0.6). Bu sürüm onları
karara çeviriyor. **Beş karar değişti, bir kusur kapsamdan çıktı.**

### D-1 — K-1'in tipi: `record` → `sealed class`

`record`'un sentezlediği `<Clone>$` copy-constructor'ı **public**tir; `private` kurucu onu
kapatmaz. `ENG-0001` ayrı assembly'den ölçtü:

```
[A2] `with`: Tool=wire_transfer Scope=9999 seal=33826822 KABUL=True
```

Meşru bir yetki elde eden saldırgan **mührü koruyarak** payload'u değiştiriyor.

> **Karar:** yetki taşıyan tipler `record` **olamaz** — `sealed class`, `private` kurucu,
> get-only property. `ENG-0001` bu varyantı ölçtü ve deliği **kapattığını** doğruladı.
> (Get-only `record` de kapatıyor ama `with`'i yasaklamak bir konvansiyondur; `sealed class`
> derleyiciyle kapatır. **Konvansiyonla zorlanan karar, karar değildir** — `Guard.cs` dersi.)

### D-2 — K-2: harf katlama **kaldırıldı**, yerine **reddetme**

`ToUpperInvariant` Türkçede yanlış sonuç veriyor (`ENG-0001` ölçtü): `ı`(U+0131)→`I`(U+0049)
yaparak **"ısı" ile "isi"yi birleştiriyor**, `İ`→`İ` bırakarak **"İş" ile "iş"i ayırıyor**.
Ve daha derin sorun: **hiçbir tek-yönlü katlama Türkçe'yi doğru katlamaz** — case *folding*
ile case *mapping* farklı şeylerdir, .NET yalnız ikincisini verir.

> **Karar:** kanonik kimlik **harf katlamaz.** Kimlikler **kanonik biçimde kaydedilir**
> (NFC + belirlenmiş harf düzeni) ve kanonik olmayan girdi **sessizce eşleştirilmez —
> reddedilir**.

**Neden bu daha iyi, sadece daha kolay değil:** `W1a`/`W1c`'nin kusuru *yanlış harf* değil,
**sessiz başarı**ydı — operatör `Disable("Wire_Transfer")` çağırıyor, sistem "tamam" diyor,
yetki canlı kalıyor. Reddetme o kusuru **katlamadan** kapatır: çağrı **hata verir**.
`W1b` (doğrulama sorgusu yanlış inancı onaylıyor) de böyle kapanır — sorgu artık
"bulunamadı" der, "kapalı" demez.

**Bedeli dürüstçe:** kullanıcıya daha katı bir arayüz. Bu, fail-safe defaults'un doğrudan
sonucudur (`RFC-6004` `Fail-safe` niteliği): belirsizlikte **kapalı** tarafa düş.

### D-3 — `W2c` (homoglyph) **kapsamdan çıkarıldı**

`ENG-0001` ölçtü: **BCL'de hiçbir Script API'si yok** — `CharUnicodeInfo`'nun dört public
static üyesinin hiçbiri script vermiyor, `Rune`'da da yok. UTS #39 `confusables.txt` da yok.
Yani M-3'ün ne asıl yolu ne geri çekilme yolu .NET'te mevcut.

> **Karar:** `W2c` bu ADR ile **kapanmaz**. §5 açık borcuna taşındı. Kapatmak harici bir
> veri kümesi (UTS #39) ve onu taşıyan bir karar gerektirir — ayrı bir ADR.

v0.1.0 bunu "kısmen açık" sayıyordu; **tamamen** açık.

### D-4 — K-4'ün `Disabled` varyantı K-3'e uyar

`Disabled(string Reason, Identity Approver, DateTimeOffset At)` — son parametre
**çağıran-verisi denetim damgasıydı**, yani `W2_L3`'ün birebir kalıbı. K-3'ü kendi taslağında
ihlal ediyordu.

> **Karar:** `At` parametresi **kaldırıldı**. Zaman damgası K-3'ün saat portundan alınır;
> `Disabled` varyantı onu **çağırandan almaz**.

### D-5 — K-6: `Measured` `IComparable<Measured>` uygular

`ENG-0001` ölçtü: `Measured` `IComparable` uygulamadığı için `OrderByDescending`
**derleniyor**, çalışma zamanında `InvalidOperationException` atıyor. `Scheduler.cs:124` ve
`CompanyMemory.cs:260` tam o yolda.

> **Karar:** `Measured` `IComparable<Measured>` **uygular**. v0.1.0'ın *"Breaking? Düşük"*
> değerlendirmesi derlemede doğru, çalışma zamanında yanlıştı — düzeltildi.

### D-6 — OQ1 kapandı: `class`, ve `WarningsAsErrors` zorunlu

`required` (C# 11+) `default(T)`'yi **kapatmıyor** (`ENG-0001` ölçtü). `class` varyantı
`CS8618` üretiyor — ama `Ens.Kernel.csproj`'da `<WarningsAsErrors>` **yok**, yani bugün o da
konvansiyon.

> **Karar:** `class`. Ve **Faz 0'da** `<WarningsAsErrors>CS8618</WarningsAsErrors>` eklenir —
> bu olmadan D-6 bir konvansiyondur, karar değil.

### D-7 — `P1..P9` → `DP1..DP9`

Kalıp adları Anayasa Madde III'ün `P1..P8` ilkeleriyle çakışıyordu; künye `principles:`
alanı gövdenin sözlüğüyle okununca §3'ün **tersini** söylüyordu. Kalıplar artık `DP1..DP9`
(*Defect Pattern*).

### D-8 — Sayı: **43**

| Adım | Sayı |
|---|---|
| Haritanın kapanabilir toplamı (DP1+DP2+DP3+DP4+DP6+DP7) | 12+13+6+5+5+6 = **47** |
| − `C2` (DP2'ye yanlış atanmış, §2.2) | −1 |
| − `W1b` (DP2'ye yanlış atanmış, §2.3) | −1 |
| − `W2_O1` (DP1'e zorlama atama, §2.4) | −1 |
| − `W2c` (D-3: .NET'te mekanizma yok) | −1 |
| **Kapanan** | **43** |

v0.1.0 **40** diyordu ve türetilemiyordu (`W1b` çifte çıkarılmıştı — `SKR-049` T-A).
v0.2.0 sayıyı düzeltmedi, yalnız hatayı kaydetti. **43 burada ilk kez türetiliyor.**

> **Yanlışlanma yolu değişmedi:** kararlar uygulanınca bu 43 kimliğin `AUDIT_DEFECT_*`
> testleri `AUDIT_FIXED_*`'a dönmelidir. Dönmeyen her test bu sayıyı çürütür.

### D-9 — Uygulama sırası (OQ6 kapandı)

`ENG-0001`'in ölçülmüş bağımlılık grafiğinden:

> **Faz 0** (OQ1 kararı + `WarningsAsErrors` + mimari tarama) → **K-5** → **K-6** → **K-2**
> → **K-3 + K-4 birlikte** → **K-1**

K-1 en sonda çünkü en geniş yüzeye dokunuyor ve K-2'nin kanonik kimliğine bağımlı.

### Kapanmayan, açıkça

`W2c` (D-3) · `R1`'in event-sourcing çözümü (kernel'de serileştirme yok, geldiğinde
`System.Text.Json` **`NotSupportedException`** atar — fail-closed değil **çöküş**) ·
`DP5` reflection (kapsam kararı) · `DP8` öz-beyan (ENS-3022 kalibrasyon borcu) · `DP9` (15
tekil iş).

> **v0.3.0 yeni bir iki-boyut turu gerektirir** (`work-protocol.md` §3.1): kararlar değişti.

---

## 0.8 v0.3.1 — `SKR-050`: changelog gövdeye uygulanmamıştı

`SKR-050` (`wounded`) en ağır bulgu olarak **S-1**'i verdi ve doğrulandı: `§0.7` bir
*changelog*, `§4.x` *normatif* — ve dokuz kararın **yedisi gövdeye hiç uygulanmamıştı.**
İki bölüm on noktada çelişiyordu.

**Sebebi utandırıcı ve teknik:** künye `version:` alanı **hizalı** yazılmış
(`version:       0.1.0`, çoklu boşluk); uygulanan `replace` tek boşluk arıyordu ve
**sessizce hiçbir şey yapmadı**. Script "v0.3.0 yazıldı" dedi çünkü §0.7 eklenmesi tuttu.
**Kontrol edilmedi.**

> Aynı ders, aynı oturumda **dördüncü kez**: *yazdım ≠ oldu.* `work-protocol.md` §4'ün
> tam konusu bu. Kural vardı, uygulanmadı.

### Uygulananlar (doğrulandı)

| Karar | Ne yapıldı | Doğrulama |
|---|---|---|
| Künye | `0.1.0` → **`0.3.0`** | `sed -n '13p'` ✅ |
| **D-4** | `Disabled`'dan `DateTimeOffset At` **kaldırıldı** | `grep` → 0 eşleşme ✅ |
| **D-5** | `Measured : IComparable<Measured>` | `grep` → var ✅ |
| **D-1** | `sealed class` (v0.3.0'da zaten tutmuştu) | ✅ |

### UYGULANMAYANLAR — bilerek

**D-2 (harf katlama → reddetme) gövdeye İŞLENMEDİ**, çünkü `SKR-050` onu yaraladı ve
itirazı güçlü:

1. **`G4`'ün kapanışı düşüyor.** Test gövdesi (`AdversarialWave_MemoryTests.cs:757-769`)
   `"fiyatlandirma"` vs `"Fiyatlandirma"` için `Assert.Equal(2, proposals.Count)` diyor —
   D-2 sonrası bu assert **hâlâ geçer**. Yani `G4` kapanmıyor, ADR kapandı sayıyordu.
2. **`W7f`'nin gerekçesi yanlışlanıyor** (*"`Ali`/`ali` aynı aktör"*).
3. **Prior art aleyhte ve ADR onu kendi §Prior art'ında anmıştı:** Unicode `CaseFolding.txt`
   Türkçe için ayrı **`T` statüsü** tanımlar — yani sorun *"doğru katlama yok"* değil, BCL
   `toCasefold` **sunmuyor**. PRECIS **iki profil** tanımlar
   (`UsernameCaseMapped`/`CasePreserved`). Ve **IDNA2003→IDNA2008 tam bu kararı verdi;
   ekosistem taşıyamadı, UTS #46 mapping'i geri getirdi.**

**D-3 (`W2c` kapsam dışı) gövdeye İŞLENMEDİ.** Öncül doğru (BCL'de Script *property* yok),
ama çıkarım geçersiz: geri çekilme yolu var ve **ADR'nin kendisi onu adıyla yazmış**
(mixed-script kısıtı). `\p{IsCyrillic}` named blocks ve `System.Text.Unicode.UnicodeRanges`
BCL'de mevcut; `W2c`'nin gerçek gövdesi (`AdversarialWave_SecurityTests.cs:241` — tek Kiril
U+0430, gerisi Basic Latin) bununla kapanır. **Bu bir kaçıştı.**

### Sayı yeniden askıda

`SKR-050` mekanik saydı: taban **47 doğru**. Ama `−W2c` haksız (D-3 kaçış), `G4` haksız
sayılmış (D-2 kapatmıyor), `−W1b` D-2'nin kendi metniyle çelişiyor, `C3` "gövdesi okunmadı"
uyarısına rağmen kesin haneye yazılmış. Yeniden hesap: **40 koşulsuz + 4 koşullu.**

`W2_O1`'in çıkarılması **haklı** bulundu — skeptic itirazı sınadı, tutmadı.

### Kaydedilmemiş üç yeni yüzey (`SKR-050`)

- `sealed class`'ın **değer-eşitliği kaybı** (`record`'dan geçişin bedeli)
- Reddetme + M-3'ün birlikte bir **varlık oracle'ı** üretmesi (`RFC-6004` §4 / CWE-209)
- Reddetme mesajının **`W2d` kanalını yeniden açması**

### D-6 kavram hatası doğrulandı

`CS8618` `default(T)`'yi **kapatmıyor**: struct'ta hiç doğmaz, ve `default(T)` **çağrı
yerinde** üretilir, kurucu çıkışında değil. `Ens.Kernel.csproj:6`'da `Nullable=enable` var,
`WarningsAsErrors` yok. Ayrıca `ImmutableArray<T>` BCL `struct`'ı olduğu için OQ1
blanket `class` kararıyla **kapanamaz** — ADR kapalı ilan ediyordu.

> **v0.4.0 gerekiyor:** D-2 ve D-3 yeniden karara bağlanmalı (PRECIS iki-profil deseni
> ciddi bir aday), D-6'nın OQ1 kapanışı geri alınmalı, üç yeni yüzey kaydedilmeli.
> Paralel `ENG-0002` turu ölçüm getiriyor; **v0.4.0 onun sonucunu bekler.**

---

## 0.9 v0.4.0 — `ENG-0002` ölçtü: **2 tuttu · 4 kısmen · 3 tutmadı**

### D-1 ÇÖKTÜ — ve çöküşü bu ADR'nin en önemli bulgusu

`Unsafe.As` mührü kırıyor. **Reflection yok, `BindingFlags` yok, `AllowUnsafeBlocks=false`.**
Oturum sahibi bağımsız doğruladı (`scratchpad/verify-unsafe`):

```
Tool=wire_transfer Scope=9999
ReferenceEquals(mühür) = True
Aynı nesne mi = True
```

Registry'nin **kendi verdiği** nesne **yerinde** değişiyor. `ReferenceEquals` bunu göremez —
çünkü gerçekten aynı nesnedir. `sealed class`'ın kapattığı şey `with` **operatörüdür**,
saldırı **sınıfı** değil.

> **Sonuç — kabul edilmesi gereken mimari gerçek:** .NET'te **in-process** hiçbir tip-tabanlı
> şema, o kodu yazabilen bir çağırana karşı yetkiyi koruyamaz. §4.1'in *"güven sınırı
> **çağrı grafiğidir**"* cümlesi **ölçümle çürüdü**. Güven sınırı **process**tir.

**Bunun iki sonucu var ve ikisi de ADR'yi değiştiriyor:**

1. **K-1 hedefini küçültür.** Tip-tabanlı mühür **kazayla ve gündelik** taklidi durdurur
   (`new ToolAuthorization(...)`, `with`, düz veri üretimi) — bu gerçek bir değerdir ve
   `E3`/`W15`'in çoğu vakası budur. Ama **kararlı** bir saldırganı durduramaz. Karar cümlesi
   *"taklit edilemez"* değil, **"taklit maliyetini `Unsafe`/process sınırına taşır"** olmalı.
2. **`DP5` (reflection) ile K-1 aynı sorudur.** ADR `DP5`'i *"kapsam kararı"* diye ayırmıştı;
   ölçüm ikisinin **tek** soru olduğunu gösterdi: *güven sınırı nerede?* Cevap process ise,
   `DP5` ve K-1 birlikte kapsam dışına çıkar — ve `E5`/`W3c` gibi `E3`/`W15`/`W4a` de
   **kısmen** açık kalır.

### D-6 tutmadı — ve `ENG-0001`'in talebini **daraltarak** zayıflattı

Derleyici çıktısıyla ölçüldü: `CS8618` yalnız **kurucu-çıkışında** doğuyor.
`default(class)` → **`CS8600`** (ADR'nin listesinde **yok**, uyarı olarak kalıyor).
`default(struct)` ve `new T[n]` → **hiçbir tanı yok**.

`ENG-0001` `Nullable` **kategorisini** istemişti — o `CS8600`'ü de hataya çevirir. ADR tek
koda daralttı ve **tam da `default(T)`'yi yakalayan kodu dışarıda bıraktı.**

> **v0.4.0 kararı:** `<WarningsAsErrors>` yerine `<TreatWarningsAsErrors>` + `Nullable`
> kategorisi. Ve OQ1 **yeniden açık** — `ImmutableArray<T>` BCL `struct`'ı olduğu için
> blanket `class` kuralı kapatamaz (`SKR-050`).

### D-7 tutmadı — ölçüldü: **8 `DP` / 78 `P`**

Ad değişikliği ilan edildi, metne **uygulanmadı**. §0.8'in S-1 kalıbının aynısı: changelog
karar sayılıyor, gövde eski kalıyor. **Beşinci tekrar.**

### D-2 kısmen — maliyeti **yazılmamış bir alt-karara** bağlı

Kernel'in **kopyasında** ölçüldü (asıl ağaç değişmedi):

| Okuma | Sonuç |
|---|---|
| **Zayıf** (yalnız biçim reddi) | `W1a`/`W1b`/`W1e` **yeşil kalıyor** — 43'ün içindeki kimlikler kapanmıyor, çünkü *biçim reddi ≠ varlık kontrolü* |
| **Katı** | **6 `AUDIT_FIXED_*` regresyon bekçisi** kırılıyor |

**20 ↔ 52 test** farkı. ADR hangisini kastettiğini söylemiyor → karar **eksik**.

### D-3 tutmadı (ikinci kez teyit) · D-4, D-5 **tuttu**

`D-4` gövde satırı doğrulandı; `D-5` iki desende de çalıştı, **NaN riski yok**.

### D-8 — sayı türetilebilir ama gövde **dört farklı sayı** taşıyor

`ENG-0002` `DEFECT-PATTERN-MAP` §1-§9'u satır satır **yeniden saydı**: 12+13+6+5+5+6 = **47**
✅, çıkarılan dördü de tek üye, **çifte çıkarma yok** → **43 türetilebilir**.
Ama gövdede **41**, **40**, **43** ve `SKR-050`'nin **"40+4"**'ü birlikte duruyor;
`Failure conditions` hâlâ **40** diyor.

### Yeni yüzey — ADR'de hiç yok

`Register` bir **enumerasyon oracle'ı**, ve daha kötüsü **sorgunun kendisi yetki veriyor**:
başarısız bir tahmin Pack'i **kalıcı kaydediyor**, aracını yetkilendiriyor, ve `Unregister`
**yok**. Hata mesajı kayıtlı versiyonu (`v3.7.1`) sızdırıyor.

### Bağımsızlık kaydı — dürüstlük gereği

`ENG-0002`, `ENG-0001` ile **aynı roldür**. G4 anlamında **ikinci boyut değildir**;
Engineering'in **ikinci ölçümüdür**. ADR-0003 bugün iki gerçek boyuttan geçti
(`SKR-*` scientific + `ENG-*` engineering); `ENG-0002` üçüncü bir boyut **eklemez**.
Ajan bunu kendi raporunda kaydetti — talep edilmeden.

---

> **v0.4.0'ın durumu:** bu sürüm ölçümleri **kaydeder**; D-1'in yeni karar cümlesi, D-2'nin
> alt-kararı ve D-6'nın kategori düzeltmesi **v0.5.0'a** kalır ve yeni bir tur ister.
> §0.6'nın ayrımı korunuyor: **kaydetmek ≠ karara bağlamak.**

---

## 0.10 v0.5.0 — ADR **BÖLÜNDÜ**, kapsam daraldı

### Neden bölündü

v0.1.0 → v0.4.0 arası dört sürüm, altı karara **birden** baktı. Her turda bazı kararlar
sağlam çıktı, bazıları kırıldı — ama hepsi tek belgede olduğu için **bir karar kırılınca
altısı birden** yeni sürüme girdi. Bu, tur sayısını kararların zorluğuyla değil,
**paketlemeyle** şişirdi.

İronik olan: `CEO-0002`'nin `RFC-6001` üzerine düştüğü **kapsam-orantısı** uyarısı bu oturumda
RFC'ler için **alıntılandı** (`RFC-6002` §5, `RFC-6003` §5) ve ADR'de **ihlal edildi**.

### Yeni dağılım

| Belge | Kararlar | Gerekçe |
|---|---|---|
| **ADR-0003** (bu belge) | **K-3, K-4, K-5, K-6** | İki turda da yeni bloke edici bulgu almadı |
| **ADR-0004** | K-1 + `DP5` | Sorun *mekanizma seçimi* değil **"mümkün mü"** — ve cevaplandı |
| **ADR-0005** | K-2 | İki turda da kırıldı; tek profil yetmiyor (PRECIS iki-profil adayı) |

### Sonlanma ölçütü — bu oturumda kondu

> **Bir karar, iki ardışık turda yeni bloke edici bulgu almazsa kapanır.**

Bugünkü durum:

| Karar | Tur 1 | Tur 2 | Durum |
|---|---|---|---|
| **K-4** (`At` kaldırıldı) | — | `ENG-0002`: **tuttu** | ✅ ölçüt sağlandı |
| **K-5** (mühürlü snapshot) | `ENG-0001`: tek koşulsuz `uygulanabilir` | yeni bulgu yok | ✅ ölçüt sağlandı |
| **K-6** (`Measured` + `IComparable`) | `ENG-0001`: kırılma bulundu | `ENG-0002`: **tuttu**, NaN riski yok | ✅ ölçüt sağlandı |
| **K-3** (saat + kabul aralığı) | `ENG-0001`: koşullu (analyzer) | koşul bilinir, karar kırılmadı | ⚠️ koşullu |

K-3'ün koşulu **yazılı ve ölçülmüş**: `BannedApiAnalyzers` çalışıyor (`ENG-0001` derledi),
ama üç sessiz-başarısızlık yüzeyi var — `#pragma` bastırma, **yanlış yazılmış yasak satırının
hiçbir tanı üretmemesi**, ve `WarningsAsErrors` yokluğu. Bunlar **Faz 0** işidir.

### D-6 düzeltmesi (v0.4.0'da tespit, burada karar)

`CS8618` `default(T)`'yi kapatmıyor — `default(class)` **`CS8600`** üretiyor (listede yoktu),
`default(struct)` ve `new T[n]` **hiçbir tanı** üretmiyor.

> **Karar:** tek koda daraltma **geri alındı**. `ENG-0001`'in orijinal talebi geçerlidir:
> **`Nullable` kategorisi** hataya çevrilir (`CS8600` dâhil). Ve **OQ1 yeniden açık** —
> `ImmutableArray<T>` BCL `struct`'ı olduğu için blanket `class` kuralı kapatamaz.
> OQ1 artık **ADR-0004/0005'e değil, buraya** aittir ve K-5/K-6'nın tip seçimini bağlar.

### D-7 — bu sefer **uygulanacak**, ilan edilmeyecek

v0.3.0 `P`→`DP` ad değişikliğini ilan etti, metne uygulamadı (`ENG-0002`: **8 `DP` / 78 `P`**).
Beşinci "changelog ≠ gövde" tekrarıydı. **v0.5.0 bu değişikliği ayrı bir edim olarak
bırakıyor** — ilan edip uygulamamaktansa, **hiç ilan etmemek** dürüsttür.

### Sayı — daralan kapsamda yeniden

Eski iddia 6 kararı kapsıyordu. Yeni kapsam **dört** karar:

| | Üye |
|---|---|
| `DP3` (K-3) | 6 |
| `DP4` (K-4) | 5 |
| `DP6` (K-5) | 5 |
| `DP7` (K-6) | 6 |
| **ADR-0003'ün kapanma iddiası** | **22** |

`DP1` (12, → ADR-0004 · kısmen), `DP2` (13, → ADR-0005), `DP5`/`DP8`/`DP9` kapsam dışı.

> **Yanlışlanma yolu değişmedi:** bu 22 kimliğin `AUDIT_DEFECT_*` testleri `AUDIT_FIXED_*`'a
> dönmelidir. Ve **22, gövdedeki dört eski sayının (41/40/43/"40+4") yerine geçer** —
> `ENG-0002` gövdenin dört farklı sayı taşıdığını tespit etmişti.

---

## 0.11 v0.6.0 — sonlanma ölçütü **GERİ ÇEKİLDİ**, bölme **yapıldı**

### Bölme: ilan edildi (v0.5.0) → **yapıldı** (v0.6.0)

`SKR-051` T-1: v0.5.0 bölmeyi ilan etti, K-1 ve K-2 **250 satır normatif metin** olarak
gövdede kaldı. Şimdi çıkarıldı ve **doğrulandı** (normatif bölüm 2 → **0**, `provides:`
temizlendi, ~230 satır). Ayrıntı: §4'teki devir kutusu.

### Sonlanma ölçütü — **çürütüldü ve geri çekildi**

v0.5.0 şunu kural yapmıştı:

> ~~*"Bir karar, iki ardışık turda yeni bloke edici bulgu almazsa kapanır."*~~

**`SKR-051` bunu ENS'in kendi kaydıyla çürüttü** ve doğrulandı
(`governance/SCAN-03-gate-compliance.md:433-436`):

> ENS-2003 §3a'daki `c` çift-sayım hatası **iki ardışık bağımsız scientific turdan da geçti**
> (`SKR-040`, `SKR-041`) ve ancak **farklı boyuttan** bir denetimle yakalandı.

Aynısı bu ADR'de tekrarlandı: `SKR-049` + `SKR-050` — **ikisi de scientific** — `Unsafe.As`
deliğini kaçırdı; `ENG-0002` buldu.

> **Ölçüt o gün yürürlükte olsaydı K-1 kapanırdı ve `ADR-0004` hiç yazılmazdı.**

Üç kusuru vardı:
1. **Boyut-kör.** G4 *farklı boyut* ister; ölçüt yalnız *tur sayar*.
2. **Edilgen çatı yokluğu kanıt sayıyor.** *"Bulgu almazsa"*, **"incelendi ve dayandı"** ile
   **"hiç bakılmadı"** arasını ayırt etmiyor.
3. **Kendine göre uygulandı.** `SKR-051` üç örnek verdi ve **K-5'inki benim de şüphelendiğim
   yerdi**: `ENG-0002`'de K-5'e ait **ölçüm bölümü yok** — `✅` bir **yokluğa** verilmiş.

### Yerine ne kondu — hiçbir şey, bilerek

Yeni bir ölçüt **uydurmuyorum.** Sonlanma sorusu (*"bu işin sonu var mı"*) gerçektir ama
cevabı bir sayaç değil:

> **Zaten yazılı olan ölçüt yeterlidir:** `GOV-000` **G4** (≥2 **farklı boyut**) +
> `RFC-6002`'nin R1–R3 ratifikasyon kapısı. Eksik olan ölçüt değil, **onay makamıydı** —
> ve o, `RFC-6002`/`RFC-6003`'ün konusu.

Bu ADR için pratik sonuç: `Accepted` kararı bu belgenin kendi içinden **çıkarılamaz**.

### `K-4`'ün K-1 bağımlılığı — açık borç

`SKR-051`: §4.4 ve R12 **K-1'in mührüne** dayanıyor, ve mühür devredildiği yerde
(`ADR-0004`) **zayıflatılmış** hâlde. K-4 Accepted'a gitmeden önce çözülmeli.
K-2'nin ayrılması ise **savunulabilir** bulundu — K-3..K-6'nın hiçbiri ona mekanizma
düzeyinde bağlı değil.

### `K-3`'ün "koşullu" statüsü

`SKR-051`: koşullu bir karar Accepted belgede kalamaz — üstelik analyzer'ın üç
sessiz-başarısızlık yüzeyinin ikisi (`#pragma` bastırma · **yanlış yazılmış yasak satırının
hiçbir tanı üretmemesi**) ADR'nin **kendi kapattığını iddia ettiği** sınıflardır.
Ve §4.1 analyzer'ı *"`E3`'ün aynısı"* diye **reddetmişti** — ama K-3'ün tek zorlayıcısı o.

### Verdict kaydı

`SKR-051` = **`refuted`**, 8 bloke edici. Çürütülen şey kararların kendisi değil,
belgenin **`Accepted` olabileceği** iddiasıdır — skeptic bunu açıkça yazdı ve
K-3/K-4/K-5/K-6'yı *"savunulabilir kararlar"* olarak ayırdı.

---

## 1. Bağlam

`Ens.Kernel` (Faz 4 referans implementasyonu, ~899 satır) üzerinde yürütülen düşmanca denetim
dalgaları 84 kimlik üretti (75 `AUDIT_DEFECT_*` + 9 `AUDIT_FINDING_*`).
`DEFECT-PATTERN-MAP.md` bunları 9 kalıba eşledi ve **altı mimari kararın 41'ini birden
kapatacağını** iddia etti (P1+P2+P3+P4+P6+P7 = 12+13+6+5+5+6).

Bu ADR o altı kararı tasarlar. Kararlar **tek tek kusuru değil, kusur SINIFINI** hedefler —
çünkü denetimin kendi meta-bulgusu (`AUDIT-WAVE2-SECURITY.md` §10.5) tam bu noktada:

> *"Kusur **örnek** olarak kapatıldı, **sınıf** olarak kapatılmadı."*

Somut emsal: `Guard.cs`'in başlığı önce "kök neden **yedi** noktada kapatıldı" diyordu; ikinci
dalga sekizinci ve dokuzuncu çağrı yerini buldu (`LlmTierSelector` Guard'ı hiç çağırmıyordu).
Yani **doğru guard, eksik çağrı yeri**. Bu ADR'nin tasarım ölçütü bu yüzden şudur:

> **Bir karar, ancak "unutmak" derleme hatası ya da tip hatası üretiyorsa sınıfı kapatır.**
> Çağrı yerlerini elle saymak, sayımın kendisini yanlışlanabilir bir iddia hâline getirir.

---

## 2. Devralınan bulgunun doğrulanması (work-protocol §3.5)

`DEFECT-PATTERN-MAP.md` §11/2 kendi zayıflığını itiraf ediyor: atamalar **test adlarından**
yapıldı, gövdeler okunmadı. Bu ADR'yi o zemine kurmadan önce **on iki** kimliğin gövdesi
okundu. **Dört ciddi sorun bulundu ve ADR bunlara göre daraltıldı.**

### 2.1 `A1` / `A2` — P3'e ataması YANLIŞ ÇERÇEVELENMİŞ (yüksek etkili)

P3'ün adı *"Zaman **çağırandan** geliyor"*. Ama `A1`'in test gövdesi
(`AdversarialWave_MemoryTests.cs:56-76`) bunun tersini yazıyor:

> *"...ne `MemoryRecord` kurucusunda (saati yok), ne `CompanyMemory.Record`'da
> (**saati VAR ama kullanmıyor**)."*

Yani `CompanyMemory` zaten bir saat taşıyor. Kusur saatin **yokluğu** değil, `assertedAt`
**verisinin** o saate karşı **doğrulanmaması**. `A2` (`:79-90`) aynı açığın üst sınırı
(`DateTimeOffset.MaxValue`).

**Sonuç:** bir `IClock`/`TimeProvider` portu enjekte etmek `A1`/`A2`'yi **kapatmaz** — saat
zaten oradaydı. Bu iki kusuru kapatan şey **kabul aralığıdır**. Bu yüzden K-3 **tek karar
değil, iki ayrı mekanizmadır** (§4.3) ve ADR bunu gizlemez.

### 2.2 `C2` — P2'ye ataması YANLIŞ (kalıp dışı)

P2 "kimlik normalizasyonu yok" (Unicode/harf/boşluk). `C2`'nin gövdesi
(`AdversarialWave_MemoryTests.cs:292-312`) hiçbir string normalizasyon sorunu içermiyor:

> *"`MemoryRecord` bir `record`tur: eşitlik/hashcode **tüm alanlar üzerinden değer-tabanlıdır**.
> İki AYRI nesne, alanları aynı ise `_lastVerified` sözlüğünde AYNI hücredir."*

Bu bir **entity ↔ value-object karıştırması**dır (Evans, *Domain-Driven Design*, 2003:
"Entities have identity; Value Objects do not"). Kanonik string tipi bunu **çözmez** —
`first` ve `twin` zaten birebir aynı stringlere sahip. Çözüm ayrı: kayda **surrogate
entity key** (`RecordId`) vermek ya da sözlüğü `ReferenceEqualityComparer` ile anahtarlamak.

**Sonuç:** `C2` K-2'nin kapsamından **çıkarılır**. P2 12 üyeye iner. `C2`, P9'un (tekil işler)
üyesidir. Bunu §5'te açık borç olarak taşıyorum.

### 2.3 `W1b` — P2'ye ataması YANLIŞ (kalıp dışı, tip hatası)

`AUDIT-WAVE2-SECURITY.md` §1.2'nin kendi kapanış cümlesi zaten farklı bir kök neden söylüyor:

> *"`IsEnabled` **üç durumu** ayırt eden bir tip dönmelidir (`NotRegistered`/`Enabled`/
> `Disabled`) ... **`bool`, üç durumlu bir soruyu temsil edemez** — bulgunun tip düzeyindeki
> kökü budur."*

`registry.Disable("operations")` + `IsEnabled("operations") == false` senaryosunda
`"operations"` **hiç kayıtlı değildir**; `CapabilityRegistry.cs:134`'ün
`_packs.ContainsKey(x) && !_disabled.Contains(x)` ifadesi ilk terimden `false` döner. Harf
farkı burada yalnızca *tetikleyicidir*; yalan söyleyen şey **iki farklı durumu tek `bool`'a
katlayan dönüş tipidir**. Kanonik isim tipi tetikleyiciyi ortadan kaldırır ama **yalanı
ortadan kaldırmaz**: kayıtlı olmayan doğru bir isim de aynı yanlış güvenceyi üretir.

**Sonuç:** `W1b` iki kararı birden gerektirir. K-2 tetikleyiciyi kapatır; **üç-durumlu dönüş
tipi ayrı bir iştir** ve bu ADR'de K-2'nin *zorunlu eş-koşulu* olarak yazılır (§4.2 M-4),
"kapandı" hanesine yazılmaz.

### 2.4 `W2_O1` — P1'e ataması ZORLAMA (en önemli bulgu)

P1 "yetki **taklit edilebilir**". `W2_O1`'in gövdesi
(`AdversarialWave_InvariantTests.cs:320-342`) taklitten söz etmiyor:

> *"`DecisionAggregate`'in **Owner diye bir property'si yok**, her metot AYRI bir `Identity`
> kabul ediyor, hiçbiri diğeriyle kıyaslanmıyor."*

Test beş farklı aktörle (alice/bob/carol/dave/erin) tek bir "atom" kurup
`Assert.Equal(5, d.History.Select(e => e.Emitter).Distinct().Count())` diyor. Yani yetki
**taklit edilmiyor** — ortada zorlanacak bir yetki **hiç yok**. Bir yetki-token tipi,
temsil edilmeyen bir değişmezi zorlayamaz.

**Sonuç:** `W2_O1` K-1 ile **kapanmaz**. K-1'in taşıdığı provenance altyapısı onu
*kapatılabilir* hâle getirir (Owner alanı bir kez var olduğunda K-1 onun bağını zorlar), ama
`Owner` alanının kendisi **ayrı bir iştir** — ENS-2001 Individuation'ın dördüncü koşulunun
implementasyonu. §5'te açık borç.

### 2.5 ⛔ GERİ ÇEKİLDİ — "on üç kimliğin testi yok" iddiası YANLIŞTI

> **Bu bölümün ilk hâli yanlıştı ve K-0 kararı onun üzerine kurulmuştu.** Kayıt, kalıbın
> ne kadar yapışkan olduğunu göstermek için kalıcıdır (EC-001).

İlk hâli şunu iddia ediyordu: `W1a` `W1b` `W1c` `W2c` `W2e` `W2f` `W5a` `W5b` `W5d` `W5e`
`W5g` `W7f` `W7h` için **hiçbir test metodu yok**, dolayısıyla `DEFECT-PATTERN-MAP` §11/3'ün
kapanış testi 41'in %32'si için uygulanamaz.

**Onüçünün de testi var.** Hepsi `Ens.Kernel.Tests/AdversarialWave_SecurityTests.cs`
içinde. Oturum sahibi tarafından mekanik olarak doğrulandı (kural §3.5):

```
W1a → 1   W1b → 1   W2c → 1   W5a → 1   W5d → 1   W7f → 1   W7h → 1
```

**Neden görünmediler — ve bu, üçüncü tekrar:** o dosyada `W2e` testinin fixture'ı olarak
**4 gerçek NUL baytı** duruyor. Dört bayt, `file`'a *"data"* dedirtiyor ve `grep`/`rg`
**tüm dosyayı** binary sayıp atlıyor. Aynı dört bayt bu oturumda üç kez yanılttı:

1. `DEFECT-REGISTER` v1 → **68** kusur saydı, gerçek **75**'ti.
2. `AUDIT-WAVE2-SECURITY` → dosyayı hiç göremedi, raporu ajan yazamadan öldü.
3. **Bu bölüm** → "13 kimliğin testi yok".

Kural `work-protocol.md` **§3.2**'ye yazıldı: *bir araç sıfır sonuç dediğinde, "yok" ile
"okuyamadım" ayırt edilmeden "yok" yazılmaz.*

**Sonuç:** §11/3'ün kapanış testi **40 kimliğin tamamı için uygulanabilir.** Bu ADR'nin
sayısal iddiası tam olarak sınanabilir durumdadır — ki bu, ilk hâlin sandığından **daha
güçlü** bir konumdur.

### 2.6 ID uzayı global olarak tekil DEĞİL (envanter riski)

`F3`, `G3`, `G4` **iki farklı anlamda** kullanılıyor:

| ID | `AdversarialWave_MemoryTests.cs` | `AdversarialAuditTests.cs` |
|---|---|---|
| `F3` | `AUDIT_DEFECT_F3_Unicode_equivalent_purpose_types_split_memory...` (`:647`) | `AUDIT_FIXED_F3_disabling_a_strict_pack_no_longer_removes_the_human_approval_guard` (`:683`) |
| `G3` | `AUDIT_DEFECT_G3_Padding_the_purpose_type...` (`:739`) | `AUDIT_FIXED_G3_verification_clock_is_now_keyed_by_record...` (`:823`) |
| `G4` | `AUDIT_DEFECT_G4_Case_variants...` (`:757`) | `AUDIT_FIXED_G4_an_invalid_record_is_rejected_at_construction...` (`:847`) |

ID'ler **dalga-kapsamlıdır, global değil.** `DEFECT-PATTERN-MAP` onları global sanıyor. Bu
vakada eşleşen açıklamalar doğru olanları (`AUDIT_DEFECT_*`) işaret ediyor — yani atamalar
**tesadüfen** doğru. Ama şema sağlam değil; bir sonraki dalga aynı harfi üçüncü kez
kullanırsa envanter sessizce bozulur.

### 2.7 `W3` — P7'ye ataması ZAYIF ama savunulabilir

`W3` (`AdversarialWave_SchedulerGateTests.cs:97-112`) `Math.Clamp(-0.0, 0, 1)`'in `-0.0`
döndürmesi. Testin **kendi güvenlik iddiası geçiyor**: `withNegZero <= withPosZero`. Yani
sömürülebilir değil; "IEEE anlamında `clamp` çıktısı ≥ 0" iddiası tam değil, o kadar.
K-6 bunu **yalnızca** postcondition'ın işaret normalizasyonu (`x + 0.0`) içermesi hâlinde
kapatır. Bu §4.6'ya açıkça yazıldı.

### 2.8 Doğrulanan atamalar

`H4` (`:906-917`), `W16` (`:413-431`), `W22` (`:584-597`), `W2_R2` (`:144-166`),
`W2_L1` (`:545-567`), `W2_L2` (`:579-600`), `W2_L4` (`:623-639`) — gövdeler kalıp
tanımlarıyla **uyumlu**. Bu yedisi için itirazım yok.

> **DOĞRULANMADI:** "mevcut 373 test" sayısı bu oturumda `dotnet test` çalıştırılarak
> doğrulanmadı — `work-protocol.md` §4 gereği sayı uydurulmuyor. §4'teki "kaç test değişir"
> tahminleri **etkilenen test dosyası ve metot sayımına** dayanır, koşturma sonucuna değil.

---

## 3. Kapsam ve kapsam-dışı

**Kapsamda:** P1 (11 üye — `W2_O1` düştü), P2 (11 üye — `C2` ve `W1b` düştü),
P3 (6), P4 (5), P6 (5), P7 (6). **Toplam 44 → 41 iddiası yerine bu ADR 40 iddia ediyor**
(11+11+6+5+5+6 = 44 üye, ancak `W3` koşullu, `W1b` kısmi → §5'e bakınız; net kapanan
kimlik sayısı **40**, ve bu sayı §7'nin yanlışlanma noktasıdır).

**Kapsam DIŞI — bu ADR bunları kapattığını İDDİA ETMEZ:**

| Grup | Neden dışarıda |
|---|---|
| **P5** (`E5`, `W3c`) — reflection | Bir **kapsam kararıdır**, düzeltme değil. "Kernel içinde mi process sınırında mı savunulacak" sorusu ayrı bir ADR'dir. `DEFECT-PATTERN-MAP` §5'in uyarısı aynen kabul edilir. |
| **P8** (10 üye) — öz-beyan kalibre değil | **Açık borç.** §5.2. ENS-3022'nin kalibrasyon borcu kapanmadan çözülemez; bu ADR yalnızca *provenance taşıma yolunu* açar, kalibrasyonu sağlamaz. |
| **P9** (15 üye) — kalıp dışı | Tekil işler. |
| `W2_O1`, `C2`, `W1b` (kısmen) | §2.2–§2.4 gereği yeniden sınıflandırıldı. |

> **v1'in hatası tekrarlanmıyor.** Önceki sicil P5'i "kapanan" hanesine yazmış ve
> yakalanmıştı. Bu ADR'nin kapanan hanesi §7'de **tek bir sayıdır** ve o sayı testle
> yanlışlanabilir.

## 4. Kararlar

### K-0 — ⛔ GERİ ÇEKİLDİ (dayanağı yanlıştı)

> **K-0 kaldırıldı.** Gerekçesi §2.5'in ilk hâliydi: *"13 kimliğin testi yok, o hâlde
> testleri yazılmadan kapandı sayılamaz."* O önerme **yanlıştı** — onüçünün de testi var
> (§2.5, doğrulandı). Yanlış bir önermeden türeyen bir karar, sonucu makul olsa bile
> **karar değildir**; taşıdığı gerekçe onu taşımıyorsa kaldırılır.
>
> **Ne kayboldu:** hiçbir şey. §11/3'ün kapanış testi zaten 40 kimliğin tamamına uygulanır;
> K-0 var olmayan bir boşluğu kapatıyordu.
>
> **Ne kaldı:** `AUDIT-WAVE2-SECURITY.md`'nin bulgularının **kod okumasıyla** üretildiği ve
> `dotnet test` çalıştırılmadığı doğrudur (o dosyanın §0'ı bunu kendisi söyler). Ama bu,
> testlerin **yokluğu** değil, o raporun **kendi sınırıdır**; testler sonradan yazıldı ve
> bugün 373/373 geçiyor (owner tarafından çalıştırıldı, 2026-07-27).
>
> Kutu kalıcıdır (EC-001): bir ADR'nin yanlış zeminden karar üretebildiğinin kaydı.

### K-1, K-2 — ⛔ **DEVREDİLDİ** (v0.6.0, `SKR-051` T-1)

> **v0.5.0 bölmeyi İLAN ETTİ, YAPMADI.** K-1 (mühürlü yetki) ve K-2 (kanonik kimlik)
> bölünme duyurusundan sonra da **250 satır normatif metin** olarak burada durdu.
> `SKR-051` bunu bloke edici bulgu olarak verdi ve haklıydı: `ADR-0004:106` *"K-1'i
> devretti"* diyordu, ADR-0003 devretmemişti.
>
> **Neden ciddi:** bu belge `Accepted` olsaydı `Ens.Kernel`'e `// TRACE: ADR-0003 K-1`
> yazılabilirdi — `ADR-0004`'ün *"öncül çöktü, hedef küçüldü"* kararının **tersi**.
> Madde VII bir **tekillik** varsayımı taşır (kod tek bir Accepted ADR'ye dayanır);
> iki belgede iki farklı K-1 onu kırar.
>
> **Sicilin ALTINCI "changelog ≠ gövde" tekrarı** — ve kalıbı adıyla teşhis eden D-7
> paragrafının **48 satır üstünde** gerçekleşti. Kalıbı bilmek onu durdurmuyor;
> **doğrulama** durduruyor.

**Şimdi gerçekten devredildi:**

| Karar | Yeni yeri | Neden |
|---|---|---|
| **K-1** (+ `DP5`) | `ADR-0004-guven-siniri-process-tur.md` | Sorun mekanizma seçimi değil **fizibilite**; `Unsafe.As` ölçümüyle cevaplandı |
| **K-2** | `ADR-0005-kanonik-kimlik.md` | İki turda kırıldı; tek profil yetmiyor (PRECIS iki-profil adayı) |

v0.5.0'ın metni **silinmedi, taşındı** — git geçmişinde ve iki yeni ADR'de durur (EC-001).

> **`K-4`'ün K-1 bağımlılığı açık borçtur** (`SKR-051`): §4.4 ve R12 mühre dayanıyor,
> ve mühür devredildiği yerde **zayıflatılmış** hâlde. Bu, K-4'ün Accepted'a gitmeden
> önce çözülmesi gereken bir atomiklik sorunudur.

### K-3 — Zaman kernel'e aittir: saat portu **ve** kabul aralığı — iki mekanizma, tek karar (P3)

> **Karar cümlesi:** Kernel'de "şimdi"yi yalnızca enjekte edilmiş `TimeProvider` üretir ve
> çağırandan gelen **her** zaman damgası bir **kabul aralığından** geçmeden hiçbir kayda,
> event'e ya da hesaba giremez.

#### Neden tek karar ama iki (aslında dört) mekanizma

Görev metni doğru bir şüphe taşıyordu ve **§2.1'de doğrulandı**: `A1`/`A2` bir saat sorunu
değildir — `CompanyMemory` zaten saat taşıyor, kullanmıyor. Saat portu tek başına `A1`/`A2`'yi
**kapatmaz**. Ama bunlar iki ayrı *karar* değil, tek bir değişmezin iki yüzüdür:

> **Değişmez:** *Kernel'de zaman, kernel'in sahip olduğu, monoton ve doğrulanmış bir
> büyüklüktür.* Bunun "üretim" yüzü saat portu, "kabul" yüzü aralık kapısıdır. Birini alıp
> diğerini almak, `Guard.cs`'in "doğru guard, eksik çağrı yeri" hatasını tekrarlar.

| # | Mekanizma | Tasarım |
|---|---|---|
| **M-1** | **Saat portu** | `System.TimeProvider` (.NET 8 BCL — yeni port **icat edilmez**). Kernel'de `DateTimeOffset.UtcNow`/`DateTime.Now` çağrısı **yasak**; her zamanlı tip ctor'da `TimeProvider` alır. `ActuationLayer.BeginActing(T0)` gibi *çağırandan zaman alan* imzalar zaman parametresini **kaybeder** |
| **M-2** | **Kabul aralığı** | `TimeWindow.Accept(ts, now, skew)` → `ts` `[genesis, now + skew]` dışındaysa **reddeder**. `skew` varsayılanı **açık bir politika değeri**dir (K-4'e tabi), sentinel değil. `DateTimeOffset.MaxValue`/`MinValue` **koşulsuz** reddedilir |
| **M-3** | **As-of varlık yüklemi** | Zamansal sorgular (`Retrieve(asOf)`) `AssertedAt <= asOf` **filtresini tip düzeyinde zorunlu** kılar: `TemporalQuery<T>` sarmalayıcısı, filtre uygulanmadan `ToList()` vermez |
| **M-4** | **Replay zaman değişmezi** | `Rehydrate` event dizisinde: (a) `EventId` tekilliği, (b) `OccurredAt` **monotonluğu** doğrular; ihlal → hata. Canlı yol ile replay yolu **aynı** zaman değişmezinden geçer |

#### Kalıbın tamamını neden kapatıyor (6 üye)

| ID | Kapatan mekanizma |
|---|---|
| `A1` | **M-2** — gelecek tarihli `AssertedAt` reddedilir. *(M-1 tek başına kapatmazdı — §2.1)* |
| `A2` | **M-2** — `MaxValue` koşulsuz red |
| `B4` | **M-1 + M-2** — `Verify` kendi damgasını üretmez, `TimeProvider`'dan alır; **kesin monotonluk** şartı aynı ana düşen 1000 doğrulamayı 1'e indirir |
| `D4` | **M-3** — `asOf`'ta var olmayan kayıt sorgudan **tip düzeyinde** dışlanır |
| `W2_L3` | **M-1** — `ActuationLayer` metotlarının zaman parametresi kalkar; denetim damgası çağıran kontrolünden çıkar |
| `W2_R6` | **M-4** — replay yinelenen id ve geriye giden damgayı reddeder |

**Hepsi kapanıyor. Kapanmayan yok.**

#### Prior art

- **`System.TimeProvider`** (.NET 8) — saat portu **icat edilmiyor**, BCL'deki standart
  soyutlama kullanılıyor. Repoda zaten `Ens.Kernel.Tests/FixedTimeProvider.cs` var; bu, portun
  test tarafının **kısmen mevcut** olduğunu gösterir.
- **Kerberos v5 (RFC 4120 §, varsayılan 5 dakika saat kayması toleransı)** — "kabul aralığı"
  fikrinin klasik kaynağı; damga geçerliliği mutlak değil, pencere ile tanımlanır.
- **JWT (RFC 7519) `nbf`/`exp`/`iat` + leeway** — aynı desenin modern hâli.
- **Google Spanner TrueTime** — zamanın bir **nokta değil aralık** (`[earliest, latest]`)
  olduğu fikri; M-2'nin `skew` parametresi bunun basitleştirilmiş hâlidir.
- **Lamport (1978)** — monotonluk şartı (M-4) olayların kısmi sıralamasından gelir.

#### Maliyet

| Ölçüt | Tahmin |
|---|---|
| Dokunulan üretim dosyası | 5 — `Domain/CompanyMemory.cs`, `ActuationLayer.cs`, `Domain/DecisionAggregate.cs`, `Domain/Events/DecisionEvents.cs`, `Domain/ContextScore.cs` (+1 yeni `Domain/TimeWindow.cs`) |
| Breaking? | **EVET.** `ActuationLayer`'ın tüm metot imzaları zaman parametresini kaybeder; `MemoryRecord` ctor `assertedAt`'ı doğrular (önceden sessiz kabul) |
| Etkilenen test | `AdversarialWave_MemoryTests`, `CompanyMemoryTests`, `ActuationLayerTests`, `AdversarialWave_InvariantTests`, `AdversarialAuditTests` — **5 dosya**. `ActuationLayerTests`'te `T0` geçen **her** çağrı değişir |
| Hafifletici | `FixedTimeProvider` zaten var → test tarafı hazır |

#### Reddedilen alternatifler

1. **Yalnız saat portu (kabul aralığı yok).** Reddedildi: §2.1'de gövdeyle **çürütüldü** —
   `A1`/`A2` çağıranın verdiği *veridir*, saatin kendisi değil. Bu alternatif 6 üyenin
   yalnız 2'sini (`W2_L3`, kısmen `B4`) kapatırdı ve "kalıp kapandı" **yanlış beyanı** olurdu.
2. **Yalnız kabul aralığı (saat portu yok).** Reddedildi: aralık bir "şimdi" gerektirir;
   "şimdi"yi `DateTimeOffset.UtcNow` ile almak testi imkânsızlaştırır ve `W2_L3` açık kalır.
3. **`assertedAt`'ı sessizce `now`'a kırpmak.** Reddedildi: `Guard.NormalizedDeficit`'te kırpma
   *muhafazakâr yönde* olduğu için savunulabilirdi; burada kırpma **kanıt uydurmaktır** —
   sistem, olmayan bir zamanı varmış gibi kaydeder. `A1`'in en kötü yanı zaten **izsizlikti**;
   kırpma izsizliği sürdürür.

#### Yeni risk

- **R9 — `skew` yeni bir saldırı parametresi.** `skew = TimeSpan.MaxValue` verildiğinde M-2
  no-op olur — **bu tam olarak P4'tür**. Bu yüzden `skew` K-4'e tabidir ve üst sınırı
  (`≤ 1 saat`) tip içinde sabittir. K-3 ile K-4 arasındaki bu bağ **zorunludur**; ayrı
  uygulanırlarsa K-3 kendi P4 kusurunu doğurur.
- **R10 — Meşru geçmiş veri (backfill) engellenir.** Göç/import senaryosunda gerçekten eski
  `assertedAt` yazmak gerekir. Azaltma: `genesis` alt sınırı yapılandırılabilir ve backfill
  **açık, izli** bir yol (`RecordHistorical(..., reason)`) olur — sessiz istisna değil.
- **R11 — Saat geri gitmesi (NTP düzeltmesi).** M-4'ün monotonluk şartı, sistem saati geri
  alındığında **meşru** event'leri reddeder. Azaltma: monotonluk mantıksal sayaç (`Version`)
  üzerinden, duvar saati yalnız `TimeWindow` üzerinden zorlanır — ikisi karıştırılmaz.

---

### K-4 — "Kapalı", bir sayı değil bir **varyanttır**; politika eşikleri tipte doğrulanır (P4)

> **Karar cümlesi:** Bir kontrolü kapatmak, eşiğe `0`/negatif yazmakla değil, yalnızca
> `Disabled(reason, approver)` varyantını **açıkça seçmekle** mümkün olur; eşik değerleri
> kullanım yerinde değil **inşa yerinde**, tipin kurucusunda doğrulanır.

#### Mekanizma

İki parça, ikisi de zorunlu:

**(a) Eşik = kısıtlı tip, opsiyonel parametre değil.**

```
public readonly record struct DecayRate      // (0, 10]  — sonlu, pozitif
public readonly record struct StaleThreshold // (0, 1]
public readonly record struct MagnitudeFloor // (0, ∞)  — sonlu
```

Kurucu `Guard.PositiveFinite` + üst sınır uygular. **Varsayılan parametre değeri yok** —
`= 0` yazılamaz çünkü `default(DecayRate)` geçersizdir ve tip bunu ctor'da yakalar
(`struct` default sorunu için `IsValid` bayrağı + `EnsureInitialized()` kapısı).

**(b) "Kapalı" bir varyant olur — sayı değil.**

```
public abstract record DecayPolicy
{
    public sealed record Active(DecayRate Rate) : DecayPolicy;
    // v0.3.0 / D-4: `At` KALDIRILDI — cagiran-verisi denetim damgasiydi (W2_L3 kalibi).
    // Zaman damgasi K-3'un saat portundan alinir.
    public sealed record Disabled(string Reason, Identity Approver) : DecayPolicy;
}
```

Aynısı `CuratorPolicy`, `ProposalPolicy`, `GatePolicy` için. Tüketici `switch` ile **her iki
dalı da ele almak zorundadır** (exhaustive matching); `Disabled` dalı **iz yayar**
(`PolicyDisabled` event'i, K-1 mührüyle).

**(c) Doğrulama yeri = inşa yeri.** `W10`'un kökü budur: eşikler *bazı* girdilerde
doğrulanıyordu. Tipe taşındığında doğrulama **girdiden bağımsız** hâle gelir — doğrulanmamış
bir eşik nesnesi var **olamaz**.

#### Kalıbın tamamını neden kapatıyor (5 üye)

| ID | Nasıl kapanıyor |
|---|---|
| `A5` | `contextDecayRate: double` → `DecayPolicy`. `0` yazılamaz; sönümü kapatmak `Disabled(reason, approver)` ister ve **iz bırakır** |
| `E4` | `staleThreshold: 0` → `StaleThreshold` tipi reddeder; curator'ı kapatmak `CuratorPolicy.Disabled` |
| `G2` | `MagnitudeFloor` sıfırı reddeder → "her purpose type öneri üretir" hâli açık bir kararla seçilir |
| `H3` | Negatif eşik **tip düzeyinde temsil edilemez** → gate no-op'a düşemez |
| `W10` | Doğrulama kullanım yerinden inşa yerine taşınır → "yalnız bazı girdilerde" durumu ortadan kalkar |

**Hepsi kapanıyor. Kapanmayan yok.**

> Not: **`W16`** (P1'de sayıldı) aslında bu kalıbın kardeşidir — `null` da bir sentineldir.
> K-1 onu yapısal olarak kapatıyor; burada sayılmıyor ki **çifte sayım** olmasın.

#### Prior art

- **Hoare (2009), "null references: my billion-dollar mistake"** — sentinel'in temel eleştirisi.
- **"Parse, don't validate"** (Alexis King, 2019) — doğrulamayı sınıra ve **tipe** taşımak.
- **"Make illegal states unrepresentable"** (F#/OCaml topluluğu, Yaron Minsky) — `Disabled`
  varyantının gerekçesi.
- **Rust `NonZeroU32` / `NonZeroUsize`** — sıfırın tip düzeyinde dışlanması; BCL karşılığı yok,
  elle yazılır.
- **12-Factor / Kubernetes admission policy** — "kapalı" bir konfigürasyon değeri değil, açık
  bir kaynak durumudur.

#### Maliyet

| Ölçüt | Tahmin |
|---|---|
| Yeni dosya | 1 — `Domain/Policies.cs` |
| Dokunulan üretim dosyası | 5 — `Domain/CompanyMemory.cs`, `Laws/DecisionGravity.cs`, `BoundedAutonomyGate.cs`, `Scheduler.cs`, `Adapter/LlmAdapter.cs` |
| Breaking? | **EVET, ama dar.** Yalnız politika parametreli imzalar |
| Etkilenen test | `CompanyMemoryTests`, `AdversarialWave_MemoryTests`, `BoundedAutonomyGateTests`, `AdversarialWave_SchedulerGateTests`, `SchedulerTests` — **5 dosya**. Eşik geçen çağrılar `new DecayRate(1.0)` biçimine döner |

#### Reddedilen alternatifler

1. **`Guard.PositiveFinite`'ı her eşik kullanımına eklemek.** Reddedildi: bu **tam olarak
   `Guard.cs`'in yaşadığı hikâyedir** — "yedi noktada kapatıldı" denilip sekizinci nokta
   bulundu. Çağrı yerini elle saymak sınıfı kapatmaz; `W10`'un kendisi bu alternatifin
   başarısızlık kanıtıdır.
2. **`double?` + `null` = kapalı.** Reddedildi: `null` yeni bir sentineldir ve `W16` tam olarak
   `null`'un iki farklı anlamı taşımasından doğuyor. Ayrıca gerekçe ve onaylayan taşımaz.
3. **Konfigürasyon doğrulamasını başlangıçta (startup) bir kez yapmak.** Reddedildi: kernel
   içinde politika **çalışma zamanında** parametre olarak dolaşıyor; startup doğrulaması
   `Scheduler.Schedule(..., autonomyThreshold, blockThreshold)` gibi çağrıları kapsamaz.

#### Yeni risk

- **R12 — `Disabled` yeni bir bypass yoludur.** Sentinel'i kaldırıp yerine *meşru* bir kapatma
  yolu koyuyoruz. Fark: eskisi **sessiz ve izsiz**, yenisi **açık ve izli**. Ama yol hâlâ
  vardır ve kötüye kullanılabilir. Azaltma: `Disabled` üretimi K-1 mührü ister (yalnız
  policy-otoritesi verebilir) ve `PolicyDisabled` event'i zorunlu.
- **R13 — `Reason` serbest metindir → P8'in tekrarı.** Bu, sicilin **8. kalıbının** (öz-beyan
  kalibre edilmemiş) bu ADR içindeki yeniden doğuşudur ve **gizlenmiyor**: `B2` ("boşluk
  olmayan herhangi bir karakter kanıt sayılıyor") kusuru `Reason` alanında aynen tekrar
  edebilir. K-4 bunu **çözmez**; §5.2'de açık borç.
- **R14 — `struct` `default` deliği.** `readonly record struct` için `default(DecayRate)`
  ctor'u atlar. `IsValid` bayrağı + her kullanım noktasında `EnsureInitialized()` gerekir —
  bu **yine bir çağrı-yeri sayımıdır** ve K-4'ün kendi ölçütünü zayıflatır. Alternatif:
  `sealed class` kullanıp `default`'u `null` yapmak, `null` kontrolünü derleyicinin nullable
  analizine bırakmak. **Bu açık bir tasarım sorusudur, §7'de yazılı.**

---

### K-5 — Sınırdan çıkan her koleksiyon **mühürlü snapshot**'tır; `IReadOnly*` dönüş tipi yasaktır (P6)

> **Karar cümlesi:** Kernel'in hiçbir public üyesi `IReadOnlyList<T>`/`IReadOnlyCollection<T>`/
> `IReadOnlySet<T>` **dönmez**; her koleksiyon dönüşü `ImmutableArray<T>` (sıralı) ya da
> `FrozenSet<T>` (kümesel) — yani **downcast edilemeyen, canlı olmayan** bir tiptir.

#### Mekanizma

`IReadOnlyList<T>` bir **görünümdür, bir garanti değildir**. İki ayrı kaçak üretir:
1. **Downcast:** `(List<T>)result` → `Reverse()`, `Add()`. (`W22`, `W5a`)
2. **Canlılık:** `ReadOnlyCollection<T>` alttaki `List<T>`'ye sarmalayıcıdır; alttaki değişince
   yineleme `InvalidOperationException` ile çöker. (`W2_L4`, `W2_R4`)

`CapabilityRegistry.cs:93` bu dersi **zaten bir yerde öğrenmiş** (`FrozenSet` kullanıyor) ve
dosyanın başındaki not (`:36-41`) bunu açıkça anlatıyor. K-5, o tek noktada uygulanan çözümü
**sınıf olarak** uygular — `AUDIT-WAVE2 §10.5`'in "örnek kapatıldı, sınıf kapatılmadı"
meta-kalıbının en temiz örneği burasıdır.

**Zorlama (kritik nokta — elle sayım değil):**
- Dönüş tipleri değişir (derleme zorlaması).
- Ek olarak bir **mimari test**: `typeof(Ens.Kernel).Assembly`'deki tüm `public` üyelerin
  dönüş tipleri taranır; `IEnumerable<T>` türevi olup `ImmutableArray<T>`/`FrozenSet<T>`/
  `ImmutableDictionary` **olmayan** her üye testi kırar. Yeni bir public üye eklendiğinde
  kural **otomatik** uygulanır. Bu, `Guard.cs`'in "listeyi elle taşı" çaresizliğinin çözümüdür.
- `W5b` için ek: "en az bir adapter" değişmezi inşadan sonra silinemez → adapter koleksiyonu
  `ImmutableArray` **ve** registry `sealed`, mutasyon API'si yok. Değişmez **inşada** kurulur
  ve tip gereği korunur.

#### Kalıbın tamamını neden kapatıyor (5 üye)

| ID | Nasıl kapanıyor |
|---|---|
| `W22` | `Scheduler.Schedule` `ImmutableArray<ScheduledDecision>` döner → `Assert.IsType<List<...>>` başarısız olur, `Reverse()` yok |
| `W2_R4` | Replay edilen alternatifler `ImmutableArray` → canlı görünüm değil |
| `W2_L4` | `History` `ImmutableArray` snapshot → yineleme sırasında geçiş yapılsa bile okuma çökmez |
| `W5a` | Adapter listesi `ImmutableArray` → downcast `InvalidCastException` |
| `W5b` | `ImmutableArray` + mutasyonsuz registry → "en az bir adapter" inşadan sonra silinemez |

**Hepsi kapanıyor. Kapanmayan yok.** Bu, altı kararın **en temiz** kapanışıdır.

#### Prior art

- **`System.Collections.Immutable`** (Microsoft, 2015) ve **`System.Collections.Frozen`**
  (.NET 8) — mekanizma BCL'de hazır, icat yok.
- **Bloch, *Effective Java*, Item 50 "Make defensive copies when needed"** — savunmacı kopya
  klasiği; `IReadOnly*`'ın neden yetmediğini de aynı madde açıklar.
- **Clojure persistent data structures** (Hickey, 2007) — "değişmezlik varsayılan" fikri.
- **ArchUnit / NetArchTest** — mimari kuralın **test olarak** zorlanması deseni; K-5'in
  mimari testi bu prior-art'ın uygulamasıdır (ENS icadı değil).

#### Maliyet

| Ölçüt | Tahmin |
|---|---|
| Dokunulan üretim dosyası | 5 — `Scheduler.cs`, `ActuationLayer.cs`, `Domain/DecisionAggregate.cs`, `Adapter/LlmAdapter.cs`, `Domain/CompanyMemory.cs` (+1 yeni mimari test dosyası) |
| Breaking? | **Kısmen.** `ImmutableArray<T>` zaten `IReadOnlyList<T>` uygular → **çağıranların çoğu derlenmeye devam eder**. Kırılan yalnız downcast yapan (yani kusurlu) kod ve `Assert.IsType<List<T>>` diyen testler |
| Etkilenen test | `AdversarialWave_SchedulerGateTests` (`W22`), `AdversarialWave_InvariantTests` (`W2_L4`, `W2_R4`), `LlmAdapterTests` — **3-4 dosya**. Bu kararın test etkisi **en düşüğüdür** |
| Performans | `ImmutableArray.CreateBuilder` + `MoveToImmutable()` ile ek kopya yok; `Scheduler` sıcak yolunda ihmal edilebilir |

#### Reddedilen alternatifler

1. **`.AsReadOnly()` çağırmak.** Reddedildi: `ReadOnlyCollection<T>` **canlı görünümdür** —
   `W2_L4` tam olarak bu çözümün başarısızlığıdır (test yorumu: *"AUDIT 5.2 düzeltmesi
   `History`'yi SİLİNEMEZ yaptı ama CANLI bıraktı"*). Denenmiş ve **çürütülmüş** alternatif.
2. **`IEnumerable<T>` dönüp `yield return` ile tembel akış.** Reddedildi: tembel akış canlılığı
   *artırır* (yineleme sırasında koleksiyon değişebilir) ve `Scheduler`'ın **sıralama garantisi**
   tüketiciler arasında dolaşırken yeniden hesaplanabilir hâle gelir.
3. **Analyzer uyarısı.** Reddedildi: K-1/RA-3 ile aynı gerekçe — uyarı bastırılabilir.
   Mimari **test** seçildi çünkü test bastırılırsa **iz kalır**.

#### Yeni risk

- **R15 — `ImmutableArray<T>` `default` deliği.** `default(ImmutableArray<T>)` **`null` gibi
  davranır** ve `.Length` erişimi `NullReferenceException` atar. K-4/R14'ün aynı sorunu.
  Azaltma: alanlar `= ImmutableArray<T>.Empty` ile ilklendirilir; mimari test bunu da tarar.
- **R16 — Snapshot bayatlığı.** `History` artık anlık bir kopyadır; çağıran onu saklayıp
  "güncel" sanabilir. Eskiden canlı görünüm en azından güncel kalıyordu. Azaltma: snapshot'a
  `Version` alanı eklenir.
- **R17 — Bellek.** Büyük geçmişlerde her `History` erişimi kopya üretir. `G1` (10.000 kayıt)
  ölçekleme kusuru **büyür**. Bu, P9'daki `G1`'i ağırlaştıran bir yan etkidir ve §5'te kayıtlı.

---

### K-6 — Çıktı kapısı: korunan sınırı geçen her sayı `Measured` olur (P7)

> **Karar cümlesi:** `Guard` yalnız girdi kapısıdır; bundan sonra kernel'in **sayı döndüren
> her public üyesi** `double` değil `Measured` döner — ve `Measured` yalnızca sonlu,
> işaret-normalize edilmiş bir değerden inşa edilebilir.

#### Mekanizma

```
// v0.3.0 / D-5: IComparable<Measured> ZORUNLU — aksi halde OrderByDescending derlenir,
// calisma zamaninda InvalidOperationException atar (Scheduler.cs:124, CompanyMemory.cs:260).
public readonly record struct Measured : IComparable<Measured>
{
    public double Value { get; }
    private Measured(double v) { Value = v; }
    public static Measured Of(double v, string what) => new(Normalize(Guard.Finite(v, "v", what)));
    private static double Normalize(double v) => v + 0.0;   // -0.0 → +0.0  (W3)
    public static implicit operator double(Measured m) => m.Value;   // TÜKETİM serbest
}
```

Asimetri kasıtlıdır: **`double → Measured` yalnızca `Of` üzerinden** (doğrulama zorunlu),
**`Measured → double` implicit** (tüketim ergonomik). Böylece kapı tek yönlüdür.

Ayrıca **birim aralık çıktıları** için `UnitMeasured` (`[0,1]`) — `NormalizedDeficit` gibi
tanım gereği normalize büyüklüklerde tipin kendisi aralığı taşır.

`W5e` (null `LlmResponse`) sayısal değildir ama **aynı kalıptır**: port çıktısı doğrulanmıyor.
Bunun için: `ILlmAdapter.CompleteAsync` dönüş tipi `LlmResponse?` değil `LlmResponse` olur ve
adapter çağrısı kernel tarafında bir **çıktı kapısından** (`AdapterGateway`) geçer; `null` dönen
adapter `InvalidOperationException` alır — proof-trace substratı yok edilemez.

**Zorlama:** K-5'in mimari testinin kardeşi — `Ens.Kernel` assembly'sindeki `public` üyelerden
**ham `double`/`double?` dönen** her üye testi kırar (izin listesi: `Guard`'ın kendisi).

#### Kalıbın tamamını neden kapatıyor (6 üye)

| ID | Nasıl kapanıyor |
|---|---|
| `H4` | `ContextScore.Compute` `Measured` döner → `-∞` **üretildiği yerde** patlar, çağıranın çok ilerisinde değil |
| `W3` | `Normalize` (`v + 0.0`) — `-0.0` → `+0.0`. **Ancak §2.7: bu kusur zaten sömürülebilir değildi**; kapanış gerçek ama şiddeti düşük |
| `W17` | Gate `Measured InfoNeed` yayar → NaN korunan sınırı geçemez |
| `W5e` | `AdapterGateway` — `null` `LlmResponse` reddedilir (sayısal değil, aynı kalıp) |
| `W8a` | `ReuseROI` `Measured` → `+∞` sınırı geçemez |
| `W8b` | `DeltaCapital` `Measured` → taşma sınırı geçemez |

**Altısı da kapanıyor**, ancak `W3` için kapanış §2.7'deki dürüst notla birlikte okunmalıdır.

#### Prior art

- **Design by Contract (Meyer, *Object-Oriented Software Construction*, 1988)** —
  **postcondition** kavramının kaynağı. K-6 tam olarak "kernel'in precondition'ı var,
  postcondition'ı yok" boşluğunu kapatır. Test adının kendisi bunu yazıyor
  (`AdversarialWave_SecurityTests.cs:927`).
- **Ada `range` tipleri / SPARK** — değer aralığının tipte taşınması.
- **F# units of measure**, **Rust `NonNan<f64>` / `ordered-float` crate'i** — sayı üstünde
  değişmez taşıyan sarmalayıcı tipin doğrudan emsali.
- **IEEE-754 §6.3** — `-0.0` ve `+0.0`'ın karşılaştırmada eşit ama sıralamada ayrışabilmesi;
  `W3`'ün kökü.

#### Maliyet

| Ölçüt | Tahmin |
|---|---|
| Yeni dosya | 2 — `Measured.cs`, `Adapter/AdapterGateway.cs` |
| Dokunulan üretim dosyası | 6 — `Laws/DecisionGravity.cs`, `Laws/DecisionCapital.cs`, `Laws/DecisionEntropy.cs`, `Domain/ContextScore.cs`, `BoundedAutonomyGate.cs`, `Adapter/LlmAdapter.cs` |
| Breaking? | **Düşük.** `implicit operator double` sayesinde okuyan kodun çoğu derlenmeye devam eder. Kırılan: `Assert.Equal(0.5, X)` gibi tip çıkarımı yapan testler ve `out double` kullanımları |
| Etkilenen test | `DecisionGravityTests`, `DecisionCapitalTests`, `DecisionEntropyTests`, `ContextScoreTests`, `BoundedAutonomyGateTests`, `AdversarialWave_MemoryTests`, `AdversarialWave_SchedulerGateTests` — **7 dosya**, ama değişiklikler mekanik |

#### Reddedilen alternatifler

1. **Her `return`'e `Guard.Finite(...)` sarmak.** Reddedildi: `W10`'un ve `Guard.cs`'in
   sekizinci-nokta hikâyesinin tekrarı. Elle sayılan çağrı yeri sınıfı kapatmaz.
2. **Debug-only `Debug.Assert` postcondition'ları.** Reddedildi: Release'te **kaybolur** —
   yani üretimde koruma yoktur. "Sessiz başarısızlık" kalıbının ders kitabı örneği.
3. **`Measured` yerine exception fırlatan `checked` aritmetik.** Reddedildi: `checked`
   yalnız tamsayı taşmasını yakalar; `double` taşması C#'ta `checked` kapsamında **değildir**
   (`+∞` döner, exception atmaz). `W8a`/`W8b` bu yolla kapanmaz.

#### Yeni risk

- **R18 — `implicit operator double` kapıyı geri delebilir.** `Measured m` bir kez `double`'a
  düştükten sonra aritmetiğe girer ve **sonuç yeniden `Measured` değildir**. Yani
  `Measured a + Measured b` → `double`, ve taşma o ara adımda oluşur. Azaltma: `Measured`
  üzerinde `+`/`*` operatörleri **tanımlanır ve sonucu yeniden doğrular**; ham `double`'a
  düşüş yalnız *son tüketimde* olur. **Bu, K-6'nın en kırılgan noktasıdır.**
- **R19 — Erken exception = servis dışı bırakma.** `H4` eskiden fail-closed tarafta sessizce
  duruyordu; artık `Compute` çağrısı **patlar**. Tek bozuk peer sinyali tüm partiyi düşürebilir
  — `Guard.NormalizedDeficit`'in yorumunda (`Guard.cs:130-132`) bu tam olarak **kırpma lehine
  gerekçe** olarak yazılmış. K-6 o gerekçeyle **çelişir**; hangi çıktının kırpılıp hangisinin
  reddedileceği üye bazında kararlaştırılmalıdır. §7'de açık soru.
- **R20 — `default(Measured)`** K-4/R14 ve K-5/R15 ile aynı delik. Üç kararın **ortak** açığı;
  tek bir çözüm (`struct` mü `class` mı) hepsini birden bağlar.

## 5. Kapanmayanlar — açık borç

Bu bölüm ADR'nin **iddia etmediklerini** sayar. v1'in hatası (P5'i "kapanan" hanesine yazmak)
burada tekrarlanmıyor.

### 5.1 Yeniden sınıflandırılan üç kimlik

| ID | Sicildeki kalıp | Gerçek kök (§2) | Gereken ayrı iş |
|---|---|---|---|
| `W2_O1` | P1 | Değişmez **temsil edilmiyor** — `Owner` alanı yok | `DecisionAggregate.Owner` + `Emitter == Owner` zorlaması + replay doğrulaması. ENS-2001 Individuation'ın 4. koşulunun implementasyonu |
| `C2` | P2 | Entity ↔ value-object karışımı (`record` değer-eşitliği sözlük anahtarı) | `MemoryRecord`'a surrogate `RecordId` ya da `ReferenceEqualityComparer` |
| `W1b` | P2 | `bool`, üç durumlu soruyu temsil edemez | K-2'nin M-4 eş-koşulu (`PackStatus` üçlüsü). K-2 **tetikleyiciyi** kapatır, yalanı değil |

### 5.2 P8 — öz-beyan kalibre edilmemiş (10 üye): **AÇIK BORÇ, çözülmedi**

`W8d` `W7` `W7d` `W7e` `B1` `B2` `D1_residual(öncüller)` `W2_P1` `W2_P3` `W2_P4`.

Bu ADR P8'i **çözmez** ve çözdüğünü iddia etmez. Gerekçe:

- P8'in kökü ENS-3022'nin **kalibrasyon borcudur** — `Guard.cs:46-50`'nin kendi dürüst sınırı
  bunu yazıyor: *"Bu bir DOĞRULAMA kapısıdır, bir KALİBRASYON kapısı değil."* Kernel
  `confidence = 0.83`'ün ölçülebilir olduğunu doğrulayabilir; **kalibre** olduğunu
  doğrulayamaz, çünkü kalibrasyonu tanımlayan teori (ENS-3022 `ops: E0`) henüz sayısal eşik
  üretmiyor.
- Bu ADR yalnız **provenance yolunu açar**: K-1'in mührü sayesinde bir `confidence` değerinin
  *kim tarafından, hangi otoriteyle* beyan edildiği taşınabilir hâle gelir. Kalibrasyon bunun
  üstüne kurulur — ama **bu ADR'de değil**.
- Dahası bu ADR P8'i **kendi içinde yeniden üretiyor**: R13 (`DecayPolicy.Disabled.Reason`
  serbest metin) `B2`'nin birebir tekrarıdır. Kaydedilmiş bir tekrar, gizlenmiş bir tekrardan
  iyidir (Madde X).

### 5.3 P5 (2) ve P9 (15): kapsam dışı

P5 bir **kapsam kararıdır** — reflection kernel içinde mi process sınırında mı savunulacak?
Bu ADR bu soruya cevap **vermez**; K-1/R3 yalnızca sınırı işaretler. P9 tekil işlerdir.

**Ek olarak K-5/R17:** `G1` (10.000 kayıt ölçekleme) bu ADR'yle **ağırlaşır**.

---

## 6. Meta-kalıp savunması (AUDIT-WAVE2 §10.5)

Meta-kalıp: *"kusur ÖRNEK olarak kapatıldı, SINIF olarak kapatılmadı."* Altı kararın her biri
bu tuzağa karşı **açık bir zorlama mekanizması** taşımak zorundadır. Denetim tablosu:

| Karar | Zorlayan mekanizma | Sınıf mı, örnek mi? | Kalan risk |
|---|---|---|---|
| **K-1** | `private` kurucu — taklit **inşa edilemez** | **Sınıf** (derleyici) | Reflection (P5, kapsam dışı) |
| **K-2** | Tip değişimi — imzalar ham `string` almaz | **Sınıf** (derleyici) | `Parse` çağıran kod yanlış profil seçebilir |
| **K-3** | `TimeProvider` + `DateTimeOffset.UtcNow` yasağı | **Kısmen** — yasak bir **konvansiyondur**, derleyici zorlamaz | **Bir analyzer/mimari test gerekir**; yoksa K-3 örnek düzeyinde kalır |
| **K-4** | Tip kısıtı + exhaustive `switch` | **Sınıf** (derleyici) | `default(struct)` deliği (R14) |
| **K-5** | Dönüş tipi + **mimari test** (assembly taraması) | **Sınıf** (test, otomatik) | Test devre dışı bırakılabilir — ama iz kalır |
| **K-6** | Dönüş tipi + **mimari test** | **Sınıf** (test, otomatik) | `implicit operator double` sonrası aritmetik (R18) |

> **Bu tablonun kendisi bir bulgudur: K-3 diğer beşinden ZAYIFTIR.** "`DateTimeOffset.UtcNow`
> kullanmayın" bir kural cümlesidir, bir tip değişmezi değil. Kabul edilirse K-3, bir Roslyn
> analyzer (`BannedApiAnalyzers` — `BannedSymbols.txt` ile `System.DateTime.get_Now`,
> `System.DateTimeOffset.get_UtcNow` yasaklanır) ya da K-5 tarzı bir assembly taraması
> **olmadan uygulanmamalıdır**. Aksi hâlde `Guard.cs`'in sekizinci-nokta hikâyesi tekrar eder.

---

## 7. Failure conditions (Madde X)

Bu ADR **yanlıştır** eğer:

1. **Sayısal iddia tutmazsa.** İddia: **K-3…K-6 uygulandığında 22 kimlik kapanır**
   (`DP3`+`DP4`+`DP6`+`DP7` = 6+5+5+6; `SKR-051` aritmetiği bağımsız doğruladı, çakışma yok).
   *v0.6.0 düzeltmesi: bu satır `40` diyordu ve **K-1…K-6** kapsıyordu — K-1/K-2 artık
   `ADR-0004`/`ADR-0005`'te. Gövdede beş farklı sayı dolaşıyordu (41/40/43/"40+4"/22);
   geçerli olan tek sayı **22**'dir ve kapsamı **K-3..K-6**'dır.*
   (P1: 11, P2: 11, P3: 6, P4: 5, P6: 5, P7: 6 = 44 üye; eksi `C3` koşullu (§4.1),
   eksi `W3`'ün düşük şiddeti, eksi `W1b`'nin M-4'e bağımlılığı → **doğrulanabilir çekirdek
   40**). Uygulama sonrası `AUDIT_FIXED_*`'a dönmeyen her kimlik bu ADR'yi yanlışlar.
2. ~~**K-0 uygulanmazsa iddia zaten sınanamaz.**~~ **GERİ ÇEKİLDİ** — §2.5'in 13-kimlik önermesi yanlıştı;
   kapanış testi 40'ın tamamına uygulanabilir. Bu koşul artık ADR'yi bloke etmez.
3. **`C3`'ün gövdesi okunduğunda `C2` ile aynı kökten çıkarsa** → K-1 onu kapatmaz, sayı 39'a
   iner (§4.1'deki uyarı).
4. **Üç kararın ortak `default(struct)` deliği (R14/R15/R20) kapatılmazsa** — K-4, K-5, K-6'nın
   üçü birden tip zorlamasını kaybeder. `struct` mi `class` mı sorusu bu ADR'de **açıktır** ve
   yanıtsız bırakılmıştır.
5. **K-3 analyzer olmadan uygulanırsa** — §6'ya göre örnek düzeyinde kalır ve meta-kalıba düşer.
6. **K-1'in mühür deseni event-sourcing ile çelişirse** (R1). Mühürlü nesneler serileşemez;
   replay yolunda yetkinin yeniden çözülmesi gerekir. Bu, `W2_R2`/`W2_R3`/`W2_R5` replay
   asimetrisi ailesini **büyütürse**, K-1 net negatif olur.
7. **K-6'nın erken exception politikası servis dışı bırakma vektörü açarsa** (R19) —
   `Guard.cs:130-132` kırpma lehine yazılmış gerekçe ile doğrudan çelişir. Bu çelişki
   **çözülmemiştir**.

### Açık sorular (kabul öncesi cevaplanmalı)

| # | Soru |
|---|---|
| **OQ1** | `Measured`/`DecayRate`/`ImmutableArray` alanları `struct` mu `class` mı? (R14/R15/R20 tek bir cevabı bekliyor) |
| **OQ2** | K-6'da hangi çıktılar **reddedilir**, hangileri **kırpılır**? `Guard.NormalizedDeficit`'in kırpma gerekçesi hangi üyeler için geçerli kalır? |
| **OQ3** | K-1 mührü çok-process/dağıtık kernel'e nasıl taşınır? İmza o zaman kaçınılmaz mı? |
| **OQ4** | `confusables.txt` gömülecek mi, yoksa `W2c` yalnız mixed-script kısıtıyla mı kapatılacak (aynı-script artığı kabul edilerek)? |
| **OQ5** | K-2'nin kanonik biçimi event store'a yazıldıktan sonra Unicode sürümü değişirse ne olur (R7)? |
| **OQ6** | Bu altı karar hangi sırayla uygulanır? K-1 ve K-2 birbirine bağlıdır (`ToolName` ↔ `Authorize`); K-3 ve K-4 birbirine bağlıdır (R9). Sıra bir uygulama planı gerektirir — **bu ADR o planı içermez.** |

---

## 8. İzlenebilirlik (`// TRACE:` konvansiyonu — Madde VII)

Her karar kod tarafında aşağıdaki iz satırlarını **zorunlu** kılar. Konvansiyon `Guard.cs:3-5`
ve `CapabilityRegistry.cs:5-11`'de zaten kurulmuş; K-* onu genişletir, değiştirmez.

| Karar | Yeni/değişen dosyada zorunlu iz |
|---|---|
| ~~**K-0**~~ | *(geri çekildi — §4.0)* |
| **K-1** | `// TRACE: ADR-0003 K-1 (brand/sealer; Morris 1973, Miller 2006) — yetki nesnedir, ortam değil`<br>`// TRACE: ADR-0001 §5.6 (bounded autonomy), §6 (capability registry)` |
| **K-2** | `// TRACE: ADR-0003 K-2 — UAX #31 (NFC), UTS #39 (confusables/mixed-script); ToUpperInvariant zorunlu (tr-TR I/ı)` |
| **K-3** | `// TRACE: ADR-0003 K-3 M-1..M-4 — TimeProvider + kabul aralığı; A1/A2 SAAT DEĞİL VERİ kusurudur` |
| **K-4** | `// TRACE: ADR-0003 K-4 — "kapalı" varyanttır, sentinel değil (Hoare 2009; parse-don't-validate)` |
| **K-5** | `// TRACE: ADR-0003 K-5 — ImmutableArray/FrozenSet; IReadOnly* GARANTİ DEĞİL (bkz. W2_L4)` |
| **K-6** | `// TRACE: ADR-0003 K-6 — postcondition (Meyer 1988); Guard girdi kapısıydı, bu çıktı kapısıdır` |

**Ek kural:** `Guard.cs`'in başındaki "kapatılan N nokta" listesi **kaldırılır**. O liste bir
elle-sayımdır ve iki kez yanlış çıktı (7→9). Yerine K-5/K-6'nın mimari testleri geçer:
sayım artık **otomatik ve yanlışlanabilir** olur. Bu, bu ADR'nin en küçük ama en
karakteristik değişikliğidir.

---

## 9. İlişkili

- `ADR-0001-agent-runtime.md` (accepted) — §5.2 altı bileşen, §5.6 bounded-autonomy gate,
  §6 capability registry, §7 LLM adapter portu. **Bu ADR onu değiştirmez, sertleştirir.**
- `ADR-0002-operations-capability.md` (accepted)
- `7000-reference-implementation/DEFECT-PATTERN-MAP.md` — kalıp eşlemesi (§2'de dört ataması
  düzeltildi)
- `7000-reference-implementation/AUDIT-WAVE2-SECURITY.md` §10.5 — "örnek vs sınıf" meta-kalıbı
- `7000-reference-implementation/DEFECT-REGISTER.md` §0, §7-8 — 8. kalıp (öz-beyan)
- Anayasa Madde VII (kod yalnız Accepted ADR'lere dayanır), Madde X (yanlışlanabilirlik),
  P6 (proof-trace), P7 (bounded autonomy)
