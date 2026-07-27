---
id: RFC-6005
type: rfc
canon: false
constitutive: false
status: draft
owner: fikri-eren
version: 0.1.0
last_reviewed: 2026-07-27
depends_on: [ENS-0000, GOV-000, GOV-010, RFC-6001, RFC-6002]
referenced_by: []
skeptic_review: pending
failure_conditions: stated
principles: [P6, P7]
---

# RFC-6005 — Onay Makamı: eksik değil, **adsız**

> `RFC-6002` kapıyı (R1–R3) tanımladı ve aktörü **açıkça devretti**: *"Aktör kimliği
> GOV-010'a aittir; bu RFC onu atamaz."* Bu belge o devri karşılar.

---

## 1. Problem — ve bir bootstrap paradoksu

`GOV-000` **G3**: *"Validation ve approval **ayrıdır**. Doğrulayan onaylamaz; onaylayan
doğrulamaz."* Bir ayrım iki taraf ister. Bugün ikinci taraf boş:

> `governance/roles.md:63` — *"Engineering/Business/Ethical Validator, **Governance body:
> fazı gelince** (ROSTER)."*

Ölçülen sonucu (`governance/SCAN-03-gate-compliance.md`):

| | Onaylayanı kayıtlı |
|---|---|
| Faz 0-2 (Külliyat) | **0 / 9** |
| Faz 3 (ADR/RFC) | **3 / 3** |

Faz 0-2'de `review → ratified` geçişini fiilen **SKR verdict'i** tetikliyor — yani
**doğrulama, onayın yerine geçiyor**, ki G3'ün yasakladığı tam budur.

### 1.1 Paradoks

Bu RFC'nin kendisi kabul edilmek için bir onay makamına ihtiyaç duyar. Makam yoksa RFC
kabul edilemez; RFC kabul edilmezse makam atanamaz. **Bu oturumda üç RFC ve üç ADR aynı
duvara çarptı.**

---

## 2. Çözüm: makam **zaten var** — yalnız adı ve kuralı yok

Paradoks, yanlış bir varsayımdan doğuyor: *"makam yaratılmalı."* Ölçüm bunun tersini
gösteriyor. Onay edimi ENS'te **dört kez** yapıldı ve **çalıştı**:

| Kayıt | Onayladığı | Ne yaptı |
|---|---|---|
| `CEO-0001` | `ADR-0001` | Hiza incelemesi → `Proposed` → `Accepted` |
| `CEO-0002` | `RFC-6001` | Hiza onayı (kapsam-orantısı uyarısıyla) |
| `CEO-0003` | `ADR-0002` | Hiza onayı |
| `STYLE-SIGNOFF-RFC-6001` | `RFC-6001` | Şema/tutarlılık imzası |

`RFC-6001` — korpusun **en ağır** yolundan geçen tek yapıt — üç skeptic turu **artı** bu
iki ayrı onay edimiyle kabul edildi. **Desen kanıtlanmıştır; eksik olan onun kural hâline
gelmesidir.**

> **Bu RFC yeni bir makam KURMUYOR; var olan bir pratiği TANIYOR.** Tanıma edimi, tanınan
> şeyin kendisini önceden onaylamasını gerektirmez — paradoks böyle kırılır.

### 2.1 Ama dürüst olalım: `ens-ceo`'nun ajan dosyası **yok**

`ROSTER.md:35` `ens-ceo`'yu *"tüm fazlar"* diye listeliyor; `.claude/agents/` altında
**dosyası yoktur**. CEO-\* kayıtları **elle yazılmıştır**.

Yani fiilî onaylayan, ayrı bir aktör değil, **bir rolde davranan sahiptir**.

---

## 3. Çekirdek karar: ayrım **kişiler** arasında değil, **edimler** arasındadır

`ARCH-0001` bunu ölçerek buldu ve `RFC-6002` v0.2.0 R2'yi ona göre yeniden yazdı:

> Faz-3'ü çalıştıran şey *farklı kişi* değil, **ayrı edim**di: ayrı dosya, ayrı context,
> ayrı kayıt.

Tek-operatörlü bir depoda (`ROADMAP` **G-16**) bu, G3'ün **yerine getirilebilir** tek
okumasıdır. Alternatif — "onaylayan farklı bir insan olmalı" — ENS'te bugün **hiçbir
yapıtın** kabul edilememesi demektir; yani kuralı sağlamak yerine **sistemi durdurur**.

> **Karar:** G3'ün istediği ayrım **edim ayrımıdır**. Onay geçerlidir ancak ve ancak:
>
> **A1.** Doğrulama turundan **ayrı bir kayıtta** yapılmışsa (`CEO-*` / `STYLE-SIGNOFF-*`),
> **A2.** Doğrulama turundan **ayrı bir context**te üretilmişse,
> **A3.** Yapıta ve kayda **iki yönlü iz** bırakıyorsa (`ratified_by`, `ratified_at`),
> **A4.** Ve **onaylayan, o yapıtın hiçbir doğrulama turunu yazmamışsa.**

**A4 kritiktir:** G3'ün gerçek yasağı budur — *doğrulayan onaylamaz*. Bir kişi hem
`SKR-051`'i hem onayı yazarsa, tur sayısı ne olursa olsun G3 ihlal edilmiştir.

---

## 4. Kim onaylar — iki rol, iki eksen

`RFC-6001`'in fiilî deseni genelleştirilir:

| Rol | Ekseni | Neye bakar |
|---|---|---|
| **`ens-ceo`** (hiza) | Uzun vadeli tutarlılık | Yapıt Anayasa ve ROADMAP ile hizalı mı; teknik borç kabul edilebilir mi |
| **`ens-style-guardian`** (şema) | Biçimsel bütünlük | Künye, numaralandırma, terminoloji, atıf zinciri |

**İkisi de itirazsızsa** onay tamamlanır. **Biri itiraz ederse** yapıt `review`e döner.

> **Neden iki:** tek onaylayan, G3'ü sağlar ama **G4'ün ruhunu** (farklı boyut) onay
> tarafında sağlamaz. İki eksen, onayı da çok-boyutlu yapar.

### 4.1 `ens-ceo`'nun materyalize edilmesi — **bu RFC'nin kapsamı dışı**

Ajan dosyasının yazılması ayrı bir edimdir (`yetenek-uret`, 4. basamak). Bu RFC yalnız
**rolün yetkisini** tanımlar. O gün gelene kadar CEO-\* kayıtları elle yazılır — bugün
zaten öyle yapılıyor ve **kayda geçiyor**.

---

## 5. Reddedilen alternatifler

**A. Onaylayan farklı bir insan olmalı.** Reddedildi: ENS tek operatörlü (`G-16`). Bu kural
bugün **hiçbir yapıtın** kabul edilememesi demektir — kuralı sağlamaz, sistemi durdurur.
Ve G3'ün metni *kişi* demiyor, **edim** diyor (*"doğrulayan onaylamaz"*).

**B. SKR verdict'i onay sayılsın.** Reddedildi: G3'ün doğrudan ihlali. Ayrıca bugün fiilen
böyle oluyor ve `SCAN-03` bunu **kusur** olarak kaydetti.

**C. Onay makamı Faz-5'e ertelensin.** Reddedildi: erteleme bir karar değil, **kararsızlığın
kurumsallaşmasıdır**. Bugün 9 `ratified` Külliyat yapıtının **hiçbirinin** onaylayanı
kayıtlı değil; erteleme o sayıyı büyütür.

**D. Yeni bir `governance-body` rolü icat edilsin.** Reddedildi: `footprint-ladder`
1. basamak — **var olanı genişlet**. `ens-ceo` + `ens-style-guardian` deseni dört kez
çalıştı; üçüncü bir rol bakım yükü ekler, kanıt eklemez.

---

## 6. Geriye dönük uygulama

**Uygulanmaz.** Mevcut 9 `ratified` yapıt geriye dönük onaylanmaz.

> **Gerekçe:** geriye dönük toplu onay, onayın **anlamını** boşaltır — hiçbiri
> incelenmeden verilmiş olurdu. Bunun yerine her yapıt, **bir sonraki dokunuşunda**
> bu kapıdan geçer. `ROADMAP`'e açık borç olarak kaydedilir.

Bu, iki farklı rejimli bir korpus üretir (`ARCH-0001`'in RFC'ler için sorduğu soru). Kabul
ediliyor ve **görünür** kılınıyor: `ratified_by` alanı **boş** olan her yapıt, "bu kapıdan
geçmemiştir" demektir.

---

## 7. Failure conditions (Madde X)

**Yanlıştır** eğer:

1. **`CEO-*` kayıtlarının dördü de fiilen doğrulama turuyla aynı context'te üretilmişse.**
   O hâlde §2'nin *"desen kanıtlanmıştır"* iddiası çöker — kanıt sanılan şey, kuralın
   ihlalinin kendisi olurdu. **DOĞRULANMADI** — kayıtların context bağımsızlığı bu belgede
   sınanmadı; ilk turun ilk işi bu olmalıdır.
2. **A4 tek-operatörlü depoda uygulanamıyorsa.** Aynı kişi hem doğrulama hem onay yazmak
   zorunda kalıyorsa, kural kâğıt üzerinde kalır — ve kayıtlı boşluktan **kötüdür**.
3. **İki onay ekseni pratikte tek bakışa çöküyorsa.** `ens-ceo` ve `ens-style-guardian`
   aynı kişi tarafından, arka arkaya, aynı çerçeveyle yazılırsa "iki eksen" nominaldir.
   Ölçüt: ikisinin **farklı bulgular** üretmesi.
4. **Bu RFC kendi kapısından geçemezse.** Kabul edilmesi için A1-A4'ü sağlayan bir onay
   gerekir; sağlanamıyorsa öneri **uygulanabilir değildir** ve reddedilmelidir.

---

## 8. Açık sorular

1. `ratified_by` / `ratified_at` künye alanları bu RFC ile mi gelir, `metadata-header`
   şemasının ayrı bir sürümüyle mi? (`RFC-6002` R3 bunu *gereksinim* olarak kaydetmişti.)
2. Onaylayan itiraz ederse yapıt `review`e mi döner, `skeptic-challenged`'a mı?
3. `ens-ceo` materyalize edilene kadar elle yazılan CEO-\* kayıtları **A2'yi (ayrı context)**
   nasıl kanıtlar?

---

## 9. Bu RFC'nin kendi yolu

İki boyut (`work-protocol.md` §3.1) + Madde XIV yordamı.

> **Ve kendi paradoksuna dürüst cevabı:** bu RFC **yeni bir makam kurmuyor**, dört kez
> yapılmış bir edimi **tanıyor ve kurallaştırıyor**. Tanıma, tanınanın önceden
> onaylanmasını gerektirmez. Ama bu savunma **sınanmalıdır** — failure condition 1 tam
> olarak onu hedefler.

**Yazarı kendi turunu `survives` işaretleyemez** — GOV-000 G4 + G3.

## 10. İlişkili
- `RFC-6002` §3.1 — R1-R3 kapısı, aktörü buraya devretti
- `RFC-6003` — boyut kadrosu (doğrulama tarafı)
- `governance/SCAN-03-gate-compliance.md` — 0/9 ↔ 3/3 ölçümü, Ö-03
- `6000-rfc/reviews/ARCH-0001-*` — "farklı aktör değil, ayrı edim" bulgusu
- `ROADMAP` G-16 (tek-operatör), G-27 (ethical borç)
