---
id: RFC-6004
type: rfc
canon: false
status: draft
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-27
depends_on: [ENS-0000, GOV-000, RFC-6001]
referenced_by: []
skeptic_review: pending
failure_conditions: stated
principles: [P6, P7]
---

# RFC-6004 — Madde V'e Güvenlik Niteliği: `Secure` DEĞİL, `Fail-safe` + `Mediated`

> **Anayasa değişikliği önerisidir** (Madde XV). Emsal RFC-6001'dir: Madde IV üç skeptic
> turu (ikisi `wounded`) + `ens-ceo` hiza-onayı + `ens-style-guardian` şema-imzası ile
> değiştirildi. Bu RFC **en az** o kadar ağır bir yoldan geçmelidir. Tek SKR yetersizdir.

## 1. Problem

Anayasa Madde V, her ENS bileşeninin taşıması gereken nitelikleri **kapalı bir liste**
olarak sayar:

> Modular · Observable · Testable · Replaceable · Versioned · Explainable · mümkün olduğunda
> Deterministic · Event-driven · DDD uyumlu · CQRS uyumlu · Cloud-native.
> Çatışmada **Explainable** ve **Testable** kazanır.

**Güvenlikle ilgili tek bir nitelik yok.**

Bu 2026-07-27'de bir atıf taramasında ortaya çıktı: `ADR-0001` Madde V'ten *"güvenli"* diye
**tırnak içinde** alıntı yapıyordu. Listede öyle bir nitelik yok — ADR olmayan bir hükmü
alıntılamıştı (`governance/SCAN-01-authority-citations.md`).

Oysa ENS güvenlik gereksinimini **fiilen kullanıyor**: `DEFECT-REGISTER.md`'deki 75 açık
kusurun büyük bölümü güvenlik nitelikli. Yani gereksinim var, dayanak yok — ADR'ler onu
P7'den ve kendi failure condition'larından türetmek zorunda kalıyor.

## 2. Ama önce: neden `Secure` EKLEMİYORUZ

En güçlü karşı-argüman arandı ve **beklenenden güçlü çıktı**:

> **Herley, C. (2016). "Unfalsifiability of security claims." *PNAS* 113(23):6415–6420.**
> DOI: [10.1073/pnas.1517797113](https://doi.org/10.1073/pnas.1517797113)

Tezi: *"X yapmazsan güvenli değilsin"* biçimindeki iddialar **görgül olarak
yanlışlanamaz**. Başarılı bir saldırı bir önlemin **yetersiz** olduğunu gösterir; ama hiçbir
gözlem bir önlemin **gereksiz** olduğunu gösteremez. Bu asimetri, gereklilik iddialarını
"ileri sürmesi kolay, çürütmesi imkânsız" yapar — ve israfı **birikimli** kılar.

Bu, ENS'in **Madde X'i (Yanlışlanabilirlik Ödevi)** ile doğrudan çarpışır:

> **`Secure` eklenirse, Madde V'in tek yanlışlanamaz üyesi olur.**

Bir yapıtın *"güvenli değil"* olduğu hangi gözlemle gösterilir? Gösterilemez — yalnızca
"henüz kırılmadı" gözlenir. Madde X'in tam olarak yasakladığı kip budur.

Destekleyici kaynaklar aynı yönde: NIST SP 800-160 güvenliği *"emergent property"* sayar —
bu, Madde V'in **"her bileşen"** ifadesiyle kategori gerilimi doğurur (emergent bir özellik
bileşen başına talep edilemez). UK NCSC ise kendi ifadesiyle: *"Secure by Default isn't a
set of requirements… It's more like an ethos or a philosophy."* Bir *ethos*, Madde V'in
denetlenebilir nitelik listesine ait değildir.

> **Karar: `Secure` reddedildi.** Ama Herley'in itirazı **geniş** adlara güçlü,
> **dar ve gözlemlenebilir** adlara zayıftır. Çözüm, güvenliği atmak değil, onu
> yanlışlanabilir parçalara ayırmaktır.

## 3. Öneri: iki dar nitelik

### 3.1 `Fail-safe`

> **Tanım:** belirsizlik, hata ya da ölçülemez girdi karşısında bileşen **kapalı** tarafa
> düşer; açık tarafa değil. Yetki, yokluğunda değil **varlığında** verilir.

**Kaynak:** Saltzer, J. H. & Schroeder, M. D. (1975), "The Protection of Information in
Computer Systems," *Proceedings of the IEEE* 63(9):1278–1308 — **fail-safe defaults**
ilkesi: *"base access decisions on permission rather than exclusion."*
Ayrıca **ISO/IEC 25010:2023**, Safety karakteristiğinin alt-karakteristiği olarak
*fail safe*'i açıkça sayar (2023 revizyonu dokuz karakteristik içerir; Safety yeni eklendi).

**Neden yanlışlanabilir:** *"Bu bileşen fail-safe'tir"* iddiası **tek bir karşı-örnekle
düşer** — ölçülemez bir girdinin (NaN, `null`, boş eşik) açık tarafa düştüğü tek bir vaka
yeter. Herley'in itirazı burada işlemez, çünkü bu bir *gereklilik* iddiası değil, bir
*davranış* iddiasıdır.

**ENS kanıtı:** `DEFECT-REGISTER.md` kalıp 4 — eşik `0` beş ayrı yerde sessiz kapatma
anahtarı oldu (`A5`, `E4`, `G2`, `H3`, `W10`). Kalıp 1'deki NaN-körlüğü de aynı ilkenin
ihlalidir. Bu nitelik olsaydı, o kusurlar **anayasal ihlal** olarak adlandırılabilirdi;
bugün yalnızca "kod hatası"dırlar.

### 3.2 `Mediated`

> **Tanım:** her yetki kullanımı, **her seferinde**, yetki veren mekanizmadan geçer.
> Bir yetki bir kez alınıp saklanamaz, taklit edilemez, yan kapıdan atlanamaz.

**Kaynak:** Saltzer & Schroeder (1975) — **complete mediation**: *"every access to every
object must be checked for authority."*

**Neden bu ikinci nitelik gerekli:** araştırma, ENS'in 8 kök-neden kalıbından **dördünün**
(1, 2, 3, 7) tek bir ilkenin — complete mediation — türevi olduğunu buldu. Yani ENS'in
kusur sicilindeki en büyük tek küme budur:

| Kalıp | Kusur sayısı | İhlal ettiği |
|---|---|---|
| 1 — public record = taklit edilebilir yetki (`E3`, `W4a`, `W15`, `W16`) | 6 | complete mediation |
| 2 — kimlik normalizasyonu yok (homoglyph, case, `NUL`) | ≥11 | complete mediation |
| 3 — zaman çağırandan geliyor, doğrulanmıyor | 5 | complete mediation |
| 7 — girdi kapısı var, **çıktı kapısı yok** | 6 | complete mediation |

**Yanlışlanabilirlik:** *"Bu bileşen mediated'dır"* iddiası, mekanizmayı atlayan **tek bir
çağrı yolu** gösterilerek düşer. `W16` (`toolAuthorization: null` → Autonomous) tam olarak
böyle bir karşı-örnektir.

## 4. Çatışma kuralı — asıl bulgu burada

Madde V bugün diyor ki: *"Çatışmada **Explainable** ve **Testable** kazanır."*
Güvenlik eklenince bu kural ne olur?

**Araştırmanın cevabı: soru yanlış kurulmuş.** Literatür bunu tek bir çatışma değil,
**iki ayrı ilişki** olarak ele alıyor:

| İlişki | Durum | Kaynak |
|---|---|---|
| **Mekanizmanın şeffaflığı** ↔ güvenlik | **Çatışma YOK — destek var** | Kerckhoffs; Saltzer & Schroeder *open design*; ISO 25010'un *accountability* / *non-repudiation* alt-karakteristikleri |
| **Çıktının ayrıntısı** ↔ güvenlik | **Çatışma VAR ve ölçülmüş** | CWE-209; Milli et al. (FAT\* 2019 — gradyan açıklamaları modeli ifşa ediyor); Shokri et al. (AIES 2021 — açıklama + tahmin membership inference'a açık) |

Yani `ProofTrace`'in var olması güvenliğin **parçasıdır**, düşmanı değil. Tehlike
mekanizmanın açıklığında değil, **çıktının kime ne kadar ayrıntı verdiğindedir**.

> ### Asıl açık: `Explainable`ın **muhatabı tanımlı değil**
> Madde V "Explainable" der ama **kime** açıklanabilir demez. Denetçiye mi? Operatöre mi?
> Çağıran sisteme mi? Saldırgana mı? Bu ayrım yapılmadığı sürece, güvenlik eklemek
> çatışmayı çözmez — yalnızca görünür kılar.

ENS'in kendi sicili bunu **zaten gözlemlemiş**: `D4` kusuru için *"ayrıca bir bilgi
sızıntısıdır"* notu düşülmüş — yani sorun fark edilmiş ama anayasal bir dille
adlandırılamamış.

### 4.1 Önerilen çatışma kuralı

> *"Çatışmada **Explainable** ve **Testable** kazanır — ancak `Explainable`ın muhatabı
> **denetim izidir**, çağıran değildir. `Fail-safe` hiçbir nitelikle takas edilemez:
> açıklanabilirlik uğruna açık kalan bir kapı, açıklanmış bir ihlaldir."*

`Fail-safe`'in takas dışı olması gerekçesi ENS'e özgüdür: P7 (Sorumluluk insandadır)
bir kapının kapalı kalmasına dayanır; kapı açıksa P7 bir temenniye döner.

## 5. P7 ile ilişki — çakışma yok, ama bir tuzak var

| Katman | Ne |
|---|---|
| **P7** | *İlke* — sorumluluk insandadır |
| `BoundedAutonomyGate` | *Mekanizma* — P7'nin koda dökülmüş hâli |
| `Fail-safe` / `Mediated` | *Kalite niteliği* — her bileşenin taşıması gereken |

Üçü ayrı katmandır, çakışmazlar.

> **Tuzak:** nitelik `Least-privilege` diye adlandırılırsa P7'nin policy zarfıyla anlam
> örtüşür ve *"bu zaten var"* savunması doğar. **O savunma yanlış olur:** kimlik
> normalizasyonu (kalıp 2), zaman doğrulaması (kalıp 3) ve canlı koleksiyon dönüşü
> (kalıp 6) P7'den **türetilemez**. Bu yüzden `Least-privilege` adı **reddedildi**.

## 6. Reddedilen alternatifler

**A. `Secure` ekle.** Reddedildi — §2, Herley 2016. Madde V'in tek yanlışlanamaz üyesi
olurdu ve Madde X ile çarpışırdı.

**B. Hiçbir şey ekleme; güvenlik P7'den türetilsin.** Reddedildi — §5: kusur kalıplarının
çoğu P7'den türetilemez. Ayrıca mevcut durum zaten bir yanlış alıntıya yol açtı
(`ADR-0001`).

**C. Tek bir `Fail-safe` yeter.** Reddedildi — kusur sicilinin **en büyük** kümesi
(4 kalıp) complete mediation ihlalidir ve fail-safe onu kapsamaz. Bir bileşen fail-safe
olup yine de mediation'sız olabilir: kapı kapalı tarafa düşer ama **yan kapı** vardır.

**D. ISO 25010'un tüm Security alt-karakteristiklerini al.** Reddedildi — Madde V bir
kalite modeli değil, kısa ve denetlenebilir bir nitelik listesidir. Beş alt-karakteristik
eklemek listeyi ikiye katlar ve Madde V'in üslubunu bozar.

## 7. Failure conditions (Madde X)

Bu RFC **yanlıştır** eğer:

1. **Herley'in tezi `Fail-safe`/`Mediated` için de geçerliyse** — yani bu iki ad da
   pratikte gereklilik-iddiasına dönüşüyorsa. §3'ün tüm savunması "bunlar davranış
   iddiasıdır, gereklilik iddiası değildir" ayrımına dayanır. Ayrım tutmuyorsa RFC çöker
   ve doğru cevap **hiçbir şey eklememek** olur.
2. **`Mediated` ile `Fail-safe` pratikte ayırt edilemiyorsa** — her `Mediated` ihlali aynı
   zamanda bir `Fail-safe` ihlali olarak da sınıflanabiliyorsa, iki nitelik yerine bir
   tane olmalıydı ve liste gereksiz uzatılmış olur.
3. **Yeni nitelikler hiçbir mevcut kusuru yeniden sınıflandırmıyorsa.** §3'ün ampirik
   iddiası, 75 kusurun büyük kısmının bu iki başlık altında adlandırılabilir olmasıdır.
   Sınıflandırma denenip tutmuyorsa, nitelikler ENS'in gerçek kusurlarını değil, ödünç
   alınmış bir literatürü tarif ediyor demektir.
4. **`Explainable`ın muhatabı tanımlanmazsa** — §4.1'in çatışma kuralı, tanımsız bir
   terime dayanır ve uygulanamaz kalır.

## 8. Açık sorular

1. **`Mediated` doğru ad mı?** Alternatifler: `Guarded`, `Checked`, `Authorized`.
   Literatürdeki ad *complete mediation*'dır ama tek sözcüğe indirgemek anlamı daraltabilir.
2. **Geriye dönük uygulanır mı?** Kabul edilirse mevcut kernel bileşenlerinin hangileri
   bu nitelikleri **taşımıyor** sayılacak? 75 kusurun yeniden sınıflandırılması gerekir mi?
3. **`Explainable`ın muhatabı** ayrı bir RFC mi olmalı? Madde V'in mevcut bir maddesini
   yeniden tanımlamak, yeni nitelik eklemekten farklı bir edimdir.
4. Madde V'in **kapalı liste** olması korunacak mı? Bugün "her bileşen şunları taşır" der;
   emergent özellikler (NIST SP 800-160) bu kalıba oturmaz.

## 9. Bu RFC'nin kendi yolu — ve neden tek SKR yetmez

Bu bir **anayasa değişikliğidir** (Madde XV). Emsal RFC-6001'dir ve emsalin en önemli
özelliği şudur: üç turun **ikisi `wounded` verdi**. Yani zincir tören değildi, gerçekten
çalıştı ve metni değiştirdi.

Gerekli yol:
1. Bağımsız `ens-skeptic` turu — bilimsel geçerlilik.
2. **Farklı boyuttan** ikinci tur (GOV-000 **G4**) — mimari/yapısal sonuçlar.
3. `ens-ceo` hiza-onayı (Madde XIV).
4. `ens-style-guardian` şema-imzası.

> **Not — bu RFC'nin kendi kapsamındaki ironi:** G4'ün "farklı boyutlardan" şartını
> uygulayacak kadro henüz atanmamıştır; bu tam olarak **RFC-6003'ün** konusudur. Yani
> RFC-6004 doğru yoldan geçebilmek için RFC-6003'ün çözdüğü boşluğa ihtiyaç duyar.
> Bu bağımlılık gizlenmiyor: RFC-6004, RFC-6003 kabul edilmeden **tam** yoldan geçemez.

## 10. Kaynaklar

- Saltzer, J. H. & Schroeder, M. D. (1975). "The Protection of Information in Computer
  Systems." *Proceedings of the IEEE* 63(9):1278–1308. *(Fail-safe defaults, complete
  mediation, open design. DOI için iki değer dolaşımda — `10.1109/PROC.1975.9939` vs
  `.1090`; cilt/sayı/sayfa kesin, DOI IEEE Xplore'dan teyit edilmeli.)*
- Herley, C. (2016). "Unfalsifiability of security claims." *PNAS* 113(23):6415–6420.
  DOI: [10.1073/pnas.1517797113](https://doi.org/10.1073/pnas.1517797113) —
  **doğrulandı** (2026-07-27, oturum sahibi).
- ISO/IEC 25010:2023 — Product quality model. Dokuz karakteristik; Security bunlardan biri,
  Safety yeni eklendi ve alt-karakteristikleri arasında *fail safe* var. **Doğrulandı**
  (2026-07-27). *Not: `iso.org` doğrudan erişime HTTP 403 döndürdü; ikincil kaynaklardan
  çapraz doğrulandı.*
- NIST SP 800-160 (Systems Security Engineering) — güvenlik *emergent property*. *Appendix
  ilke kataloğu okunmadı.*
- Milli et al., FAT\* 2019 · Shokri et al., AIES 2021 · CWE-209 — açıklama ↔ sızıntı
  gerilimi.
- Tam bulgu notu ve erişilemeyen kaynak tablosu: `research/security-as-design-principle.md`
