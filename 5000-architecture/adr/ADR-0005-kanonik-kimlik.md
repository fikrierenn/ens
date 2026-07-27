---
id: ADR-0005
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
principles: [P6]
---

# ADR-0005 — Kanonik Kimlik: harf katlama, Unicode ve homoglyph

> `ADR-0003`'ün K-2 kararı buraya devredildi. Sebep: iki turda da **kırıldı**, ve her
> kırılışta diğer beş karar da yeni sürüme giriyordu (`ADR-0004` §2 — kapsam-orantısı).

---

## 1. Problem

`DEFECT-PATTERN-MAP` `DP2` — **13 kusur**: aynı varlık, harf/Unicode/boşluk/`NUL` farkı
yüzünden **iki ayrı varlık** gibi davranıyor. `F3` `G3` `G4` `C2` `W1a` `W1b` `W1c` `W2c`
`W2e` `W2f` `W5g` `W7f` `W7h`.

> **Not:** `C2` ve `W1b` bu listeden **çıkarıldı** — `ADR-0003` §2.2/§2.3 gövdelerini okuyup
> yanlış atandıklarını gösterdi (`C2` entity/value karışımı, `W1b` `bool`'un üç durumu
> temsil edememesi). Kalan: **11**.

## 2. İki turda kırılan iki çözüm

### 2.1 v0.1.0: `ToUpperInvariant()` — **ölçümle çürüdü**

`ENG-0001` ölçtü:
```
'işletme' vs 'İŞLETME'       -> canon eşit mi: False   (IŞLETME | İŞLETME)
'satın alma' vs 'SATIN ALMA' -> canon eşit mi: False
```
`ı`(U+0131)→`I`(U+0049) yaparak **"ısı" ile "isi"yi birleştiriyor**; `İ`→`İ` bırakarak
**"İş" ile "iş"i ayırıyor**. ENS **Türkçe** bir projedir.

### 2.2 v0.3.0: katlama yok, **reddet** — `SKR-050` + `ENG-0002` çürüttü

Üç ayrı gerekçeyle:

1. **`G4`'ü kapatmıyor.** Test gövdesi (`AdversarialWave_MemoryTests.cs:757-769`)
   `"fiyatlandirma"` vs `"Fiyatlandirma"` için `Assert.Equal(2, proposals.Count)` diyor ve
   reddetme sonrası bu assert **hâlâ geçer**.
2. **`W7f`'nin gerekçesini yanlışlıyor** (*"`Ali`/`ali` aynı aktör"*).
3. **Prior art aleyhte — ve `ADR-0003` onu kendi §Prior art'ında anmıştı:**
   **IDNA2003 → IDNA2008** tam bu kararı verdi (map yerine reject), **ekosistem taşıyamadı**,
   **UTS #46** mapping'i geri getirdi.

Ayrıca `ENG-0002` maliyeti ölçtü: kararın **yazılmamış bir alt-kararı** var —

| Okuma | Sonuç |
|---|---|
| **Zayıf** (yalnız biçim reddi) | `W1a`/`W1b`/`W1e` **yeşil kalıyor** — *biçim reddi ≠ varlık kontrolü* |
| **Katı** | **6 `AUDIT_FIXED_*` regresyon bekçisi** kırılıyor |

**20 ↔ 52 test.** Karar hangisini kastettiğini söylemiyordu.

## 3. Bu ADR'nin çıkış noktası: **tek profil yetmiyor**

İki turun ortak dersi şu: `DP2`'nin 11 kusuru **tek bir kanonikleştirme kuralıyla**
kapanmıyor, çünkü iki farklı gereksinim var ve **çelişiyorlar**:

| Gereksinim | Örnek | İster |
|---|---|---|
| **Eşleştirme** — aynı şeyi aynı say | `G4`: `fiyatlandirma` ≡ `Fiyatlandirma` | **katlama** |
| **Ayırt etme** — farklı şeyi ayır | Türkçe `ısı` ≠ `isi` | **katlamama** |

`ToUpperInvariant` birinciyi Türkçe'de bozuyor; reddetme ikinciyi koruyup **birinciyi
tamamen bırakıyor**.

### 3.1 Aday: PRECIS **iki profil** (RFC 8265)

`SKR-050`'nin işaret ettiği yol — ve `ADR-0003`'ün prior art'ında zaten anılan:

> PRECIS **`UsernameCaseMapped`** (katlar) ve **`UsernameCasePreserved`** (korur) diye
> **iki ayrı profil** tanımlar. Tek profil aramak, standardın çözdüğü sorunu yeniden açmaktır.

ENS'e uyarlaması: her kimlik **sınıfı** kendi profilini beyan eder.
- `purpose type`, `decision class` → **katlanan** profil (eşleştirme baskın; `G4`, `G3`, `F3`)
- `owner id`, `tool name` → **korunan** profil + **ayrı bir eşitlik kuralı** (`W7f`, `W2e`,
  `W2f`, `W5g`)

**Bu ADR bunu karara bağlamıyor — aday olarak kaydediyor** (§6 açık soru).

### 3.2 Türkçe: sorun *"doğru katlama yok"* değil

`SKR-050` doğruladı: Unicode **`CaseFolding.txt`** Türkçe için ayrı **`T` statüsü** tanımlar.
Yani doğru katlama **tanımlıdır**; **BCL `toCasefold` sunmuyor**. Bu, ADR-0003'ün
*"hiçbir tek-yönlü katlama Türkçe'yi doğru katlamaz"* cümlesini düzeltir — katlama var,
**.NET'te API'si yok.**

## 4. `W2c` (homoglyph) **kapsama geri alındı**

`ADR-0003` D-3 onu kapsam dışına almıştı; `SKR-050` bunu **kaçış** olarak niteledi ve haklı:

- Öncül doğru: BCL'de Script **property** yok (`ENG-0001` ölçtü).
- Ama çıkarım geçersiz: **geri çekilme yolu var ve `ADR-0003` onu kendi metninde adıyla
  yazmıştı** — *mixed-script kısıtı*.
- `\p{IsCyrillic}` named blocks ve `System.Text.Unicode.UnicodeRanges` **BCL'de mevcut**.
- `W2c`'nin gerçek gövdesi (`AdversarialWave_SecurityTests.cs:241`): **tek** Kiril U+0430,
  gerisi Basic Latin → mixed-script kısıtıyla yakalanır.

> **Karar:** `W2c` bu ADR'nin kapsamındadır. Tam UTS #39 `confusables.txt` olmadan
> **aynı-script** varyantı (`rn`↔`m`) açık kalır; bu **yazılı sınırdır**, kapsam dışı değil.

## 5. Kapsam

**Dahil:** `DP2`'nin 11 kusuru + `W2c`. Kimlik sınıfı başına profil seçimi. NFC normalizasyonu
(v0.1.0'da kararlaştırılmıştı ve **kırılmadı** — NFKC reddi ayakta: `ﬁ`→`fi` anlam değiştirir).

**Hariç:** `C2`, `W1b` (yanlış atama, `ADR-0003` §2.2/§2.3) · aynı-script homoglyph (§4) ·
`Register`'ın enumerasyon oracle'ı (`ENG-0002` yeni yüzeyi — ayrı iş).

## 6. Açık sorular

1. **PRECIS iki-profil mi, başka bir ayrım mı?** (§3.1)
2. **Hangi kimlik sınıfı hangi profili alır?** Yanlış eşleme `G4` veya `W7f`'yi açık bırakır.
3. **Türkçe katlama .NET'te nasıl yapılır?** `CaseFolding.txt` gömülecek mi, `ICU` bağımlılığı
   mı, yoksa Türkçe'ye özel elle eşleme mi? **Her üçü de bir bağımlılık kararıdır.**
4. **`ENG-0002`'nin 20↔52 test farkı** hangi okumada kalır?

## 7. Failure conditions (Madde X)

1. **PRECIS iki-profil de `G4` ile Türkçe'yi aynı anda çözemezse** — o zaman `DP2` tek
   kararla kapanmaz ve bu ADR de bölünmelidir.
2. **`\p{IsCyrillic}`/`UnicodeRanges` ile mixed-script kısıtı kurulamıyorsa** — §4 çöker ve
   `W2c` gerçekten kapsam dışıdır (yani `ADR-0003` D-3 haklıydı).
3. **Profil ataması `AUDIT_DEFECT_*` testlerini `AUDIT_FIXED_*`'a çevirmezse.**
   Ölçüt `DEFECT-PATTERN-MAP` §11/3 ile aynı.

## 8. Bu ADR'nin yolu

İki boyut (`work-protocol.md` §3.1). Mühendislik turunun ilk işi §7/2'yi **ölçmek** olmalı.
**Yazarı kendi turunu `survives` işaretleyemez** — GOV-000 G4 + G3.

## 9. İlişkili
- `ADR-0003` §0.7 D-2, §0.8, §0.9 — kırılma kayıtları
- `SKR-050`, `ENG-0001`, `ENG-0002` — ölçümler
- RFC 8265 (PRECIS), Unicode `CaseFolding.txt`, UTS #46, UTS #39
