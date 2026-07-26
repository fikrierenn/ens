# Araştırma Notu: Bir Tasarım İlkesi Olarak Güvenlik

**Durum:** TASLAK — doldurulma sürüyor
**Tarih:** 2026-07-25
**Yazan:** ens-researcher
**Amaç:** ENS Anayasası Madde V'e güvenlik niteliği eklenmesi tartışması için prior art malzemesi.
**UYARI:** Bu belge karar değildir. Madde V değişikliği RFC + Madde XV yordamıyla alınır.

---

## 0. Sorunun Kısa Hâli

Anayasa Madde V, her ENS bileşeninin taşıması gereken nitelikleri **kapalı bir liste** olarak
sayar:

> Modular · Observable · Testable · Replaceable · Versioned · Explainable · mümkün olduğunda
> Deterministic · Event-driven · DDD uyumlu · CQRS uyumlu · Cloud-native. Çatışmada
> **Explainable** ve **Testable** kazanır.
> — `0000-constitution/ENS-0000-constitution.md:121-126`

Listede **güvenlik yok.** Buna karşılık:

- `ADR-0001` Madde V'ten *"güvenli"* diye tırnaklı alıntı yapıyordu — listede bulunmayan bir
  nitelik (bkz. `governance/SCAN-01-authority-citations.md`).
- `DEFECT-REGISTER.md` §2'deki 14 KRİTİK kusurun ortak özelliği literal olarak bir güvenlik
  tehdit modelidir: *"saldırgana özel yetki gerekmiyor"* (satır 162).
- §7'deki kök-neden kalıplarının çoğu — kalıp 1 (taklit edilebilir yetki), 3 (zaman
  çağırandan), 4 (eşik 0 = sessiz kapatma), 7 (çıktı kapısı yok) — klasik güvenlik ilkesi
  ihlalleridir (§5'te eşlendi).

Yani ENS **pratikte** bir güvenlik gereksinimi uyguluyor ama **anayasal dayanağı yok**. Bu not
o dayanağın literatürdeki emsallerini toplar.

---

## 1. Güvenlik Bir "Tasarım İlkesi" Olarak Nasıl Formüle Edilir?

### 1.1 Saltzer & Schroeder (1975) — kanonik kaynak

**Künye:** Saltzer, J. H. & Schroeder, M. D. (1975). "The Protection of Information in Computer
Systems." *Proceedings of the IEEE*, 63(9), 1278–1308. DOI: `10.1109/PROC.1975.9939`.
Tam metin (MIT/UVa aynası): <https://www.cs.virginia.edu/~evans/cs551/saltzer/>
Yazarın kendi yayın listesi: <https://web.mit.edu/Saltzer/www/publications/pubs.html>

**Doğrulama notu:** Metin kopyasının başlığında *"Manuscript received October 11, 1974; revised
April 17, 1975 … Fourth ACM Symposium on Operating System Principles (October 1973). Revised
version in Communications of the ACM 17, 7 (July 1974)."* yazıyor. Yani makalenin bir soyağacı
var; **alıntılanması gereken sürüm Proc. IEEE 1975'tir**. Yıl olarak "1975" doğru.

**Sekiz ilke (yazarların kendi ifadesiyle, kısa):**

| # | İlke | Öz |
|---|------|-----|
| a | **Economy of mechanism** | "Keep the design as simple and small as possible" — koruma mekanizması *incelenebilir* kalmalı |
| b | **Fail-safe defaults** | "Base access decisions on permission rather than exclusion" — varsayılan **ret**, izin açıkça verilmeli |
| c | **Complete mediation** | "Every access to every object must be checked for authority" — başlatma, kurtarma, kapanma ve bakım dâhil |
| d | **Open design** | "The design should not be secret" — güvenlik anahtarda, mekanizmanın gizliliğinde değil |
| e | **Separation of privilege** | Tek bir anahtarın/koşulun kırılması yetmesin; birden çok koşul |
| f | **Least privilege** | Her program ve kullanıcı işi bitirmeye yeten **en küçük** yetki kümesiyle çalışsın |
| g | **Least common mechanism** | Birden fazla kullanıcı arasında paylaşılan mekanizmayı asgariye indir |
| h | **Psychological acceptability** | İnsan arayüzü kolay olsun ki koruma *doğru şekilde ve kendiliğinden* uygulansın |

**Kısmen uygulanabilir sayılan iki ilke daha:** *work factor* (kırma maliyeti vs. saldırganın
kaynağı) ve *compromise recording* (ihlal kanıtının kaydı).

**ENS ile doğrudan örtüşme — doğrulandı:**

- **Fail-safe defaults ↔ ENS'in fail-open kusur ailesi.** Görev tanımındaki hipotez **doğru**.
  `DEFECT-REGISTER.md` §7 kalıp 4 (*"Eşik `0` = sessiz kapatma"*, 4-5 kusur) tam olarak
  fail-safe defaults ihlalidir: `0` hem "kapalı" hem geçerli değer olduğu için sistem
  **izin** yönünde açılıyor; Saltzer & Schroeder varsayılanın **ret** olmasını ister.
- **Complete mediation ↔ §7 kalıp 7** (*"Girdi kapısı var, ÇIKTI kapısı yok"*, 6 kusur).
  Complete mediation "her erişim, her nesne, her aşama" der; ENS'te kapı yalnızca giriş
  aşamasında var.
- **Open design ↔ Explainable.** ENS'in Explainable ilkesi, Saltzer & Schroeder'in *open
  design*'ıyla **çelişmez, örtüşür**. Bu §2'nin çatışma tartışması için kritik.
- **Psychological acceptability ↔ Madde V'in "incelenemeyen bir disiplin öğretilemez"**
  gerekçesi — aynı akıl yürütme kipi (koruma kullanılabilir değilse uygulanmaz).

**Karşı-not (dürüstlük):** Smith (2012) bu ilkelerin hepsinin eşit yaşlanmadığını gösteriyor —
*least privilege* ve *separation of privilege* yerleşti, *economy of mechanism* ve *complete
mediation* "gelişemedi". Künye: Smith, R. E. (2012). "A Contemporary Look at Saltzer and
Schroeder's 1975 Design Principles." *IEEE Security & Privacy*, 10(6), 20–25.
DOI: `10.1109/MSP.2012.85`. (Özet düzeyinde doğrulandı; tam metne erişilmedi — §7'ye bakınız.)

### 1.2 Secure by Design / Secure by Default (CISA, NCSC)

**CISA ve ortakları (2023).**
Künye: CISA, NSA, FBI ve uluslararası ortaklar (2023). *"Shifting the Balance of Cybersecurity
Risk: Principles and Approaches for Secure by Design Software."* İlk yayın 13 Nisan 2023;
güncelleme 16 Ekim 2023 (17 ABD ve uluslararası ortak; güncellemede 8 ek uluslararası ajans
ortak mühür).
URL: <https://www.cisa.gov/resources-tools/resources/secure-by-design>
Duyuru: <https://www.cisa.gov/news-events/alerts/2023/10/16/cisa-nsa-fbi-and-international-partners-release-updated-secure-design-guidance>

**Doğrulama notu:** `cisa.gov` alan adı bu oturumda WebFetch'e **HTTP 403** döndürdü. Aşağıdaki
üç ilke, ikincil ama kurumsal kaynaklardan (DoD CSIAC, US Navy DON CIO CHIPS, HS Today)
doğrulandı; **CISA'nın kendi PDF'i birinci elden okunamadı** (§7).

Üç ilke (ikincil kaynaklardan):
1. **Take Ownership of Customer Security Outcomes** — güvenlik yükü son kullanıcıdan
   üreticiye kayar.
2. **Embrace Radical Transparency and Accountability** — zafiyet yönetimi şeffaf olur.
3. **Lead From the Top** — güvenlik bir mühendislik alt-görevi değil, yönetişim kararıdır.

**ENS için delta:** Bu rehber güvenliği bir *nitelik* olarak değil, bir **sorumluluk dağıtımı**
olarak formüle eder. Madde V bir nitelik listesidir; CISA çerçevesi Madde V'e değil, ENS'in
sorumluluk ilkelerine (P7 — "sorumluluk insandadır") daha yakındır. **Madde V'e ekleme
gerekçesi olarak CISA zayıf bir dayanaktır**; Saltzer & Schroeder ve ISO 25010 daha uygun.

**NCSC (UK) — Cyber Security Design Principles.**
Künye: National Cyber Security Centre (2019). *"Secure design principles: Guides for the design
of cyber secure systems."* Sürüm 1.0, 21 Mayıs 2019.
URL: <https://www.ncsc.gov.uk/collection/cyber-security-design-principles>

Beş ilke (birinci elden doğrulandı):
1. **Establish the context** — "Determine *all* the elements which compose your system, so your
   defensive measures will have no blind spots."
2. **Make compromise difficult**
3. **Make disruption difficult**
4. **Make compromise detection easier** — "Design your system so you can spot suspicious
   activity as it happens and take necessary action."
5. **Reduce the impact of compromise**

**ENS için delta — dikkat çekici:** NCSC'nin 4. ilkesi (*compromise detection easier*)
Saltzer & Schroeder'in *compromise recording*'inin torunudur ve **ENS'in Observable niteliğiyle
aynı yöne bakar**. Yani güvenlik ile gözlemlenebilirlik arasındaki ilişki devlet rehberliğinde
**çatışma değil, bağımlılık** olarak kurulmuş. Bu, §2'nin ana bulgusudur.

NCSC ayrıca *Secure by Default*'u ayrı bir sayfada, açıkça **"bir gereksinim kümesi ya da
uyumluluk şeması değil, bir etos/felsefe"** olarak tanımlar
(<https://www.ncsc.gov.uk/information/secure-default>) — bu, §4'ün karşı-argümanı için
doğrudan malzemedir.

### 1.3 NIST SP 800-160 — "güvenlik bir emergent property'dir"

**Künye:** Ross, R., Winstead, M. & McEvilley, M. (2022). *Engineering Trustworthy Secure
Systems.* NIST Special Publication 800-160, Volume 1, Revision 1. National Institute of
Standards and Technology. Yayın: 16 Kasım 2022. DOI: `10.6028/NIST.SP.800-160v1r1`
URL: <https://csrc.nist.gov/pubs/sp/800/160/v1/r1/final>

**Merkezi tez (NIST'in kendi duyurusundan doğrulandı):** Güvenlik, sistemin bir **emergent
property**'sidir (beliren özellik) — bir bileşene iliştirilen bir etiket değil. NIST'in
ifadesiyle: güvenliği "geleneksel silosundan çıkarıp bir beliren sistem özelliği olarak görmek",
uzay aracı/uçak/köprü mühendisliğinde safety, reliability, availability ve maintainability'nin
ele alınışına benzer bir disiplin gerektirir.
Kaynak: <https://csrc.nist.gov/News/2022/guidance-on-engineering-trustworthy-secure-systems>

**ENS için delta — bu bir gerilimdir, destek değil:**
Madde V şöyle başlar: *"**Her ENS bileşeni:** Modular · Observable · …"*. NIST'in tezi doğruysa,
güvenliği **bileşen düzeyinde** bir nitelik olarak listelemek kategori hatasıdır — çünkü
bileşenlerin her biri güvenli olsa bile bileşimleri güvensiz olabilir. Bu, §4'ün en güçlü
karşı-argümanının çekirdeğidir ve aynı zamanda ENS'in kendi kusur sicilinde **görgül olarak
doğrulanmıştır**: `DEFECT-REGISTER.md` §7 kalıp 7 ("girdi kapısı var, çıktı kapısı yok") tek
bir bileşenin kusuru değil, **bileşenler arası** bir kapı eksikliğidir.

**Doğrulama notu:** SP 800-160 v1r1'in tam PDF'i (yaklaşık 300 sayfa) bu oturumda birinci elden
okunmadı; yukarıdaki tez NIST'in kendi CSRC duyuru sayfasından alındı. Ek: NIST SP 800-160 v1
Appendix'lerinde bir "systems security design principles" kataloğu bulunduğu bilinir ancak
**bu oturumda madde madde doğrulanmadı** (§7).

### 1.4 ISO/IEC 25010 — Madde V'in en yakın kurumsal emsali

**Künye:** ISO/IEC 25010:2023, *Systems and software engineering — Systems and software Quality
Requirements and Evaluation (SQuaRE) — Product quality model.* 2. baskı, 2023. (2011 baskısının
yerini alır.)
URL: <https://www.iso.org/standard/78176.html> · Online browsing platform:
<https://www.iso.org/obp/ui/en/#!iso:std:78176:en>

**Görev tanımındaki bir düzeltme:** Görev "sekiz kalite karakteristiği" diyor. Bu **2011
baskısı** için doğrudur. **2023 baskısında dokuz** karakteristik vardır — *Safety* yeni bir
üst-düzey karakteristik olarak eklenmiş, *Usability* → **Interaction Capability**, *Portability*
→ **Flexibility** olarak yeniden adlandırılmıştır.
Doğrulama: <https://quality.arc42.org/articles/iso-25010-update-2023>

**2023 dokuz karakteristik:** Functional Suitability · Performance Efficiency · Compatibility ·
**Interaction Capability** · Reliability · **Security** · Maintainability · **Flexibility** ·
**Safety**.

**Security'nin alt-karakteristikleri (2023):** Confidentiality · Integrity · Non-repudiation ·
Accountability · Authenticity · **Resistance** (2023'te eklendi — saldırıya dayanma, yanıt
verme ve toparlanma).

**ENS için delta — bu en güçlü emsaldir:**

1. **Yapısal örtüşme.** Madde V fiilen bir *kalite-nitelik listesidir* ve ISO 25010 aynı türden
   bir listedir. ISO 25010, Security'yi listeye almanın **kategori hatası olmadığını** gösteren
   en yerleşik uluslararası emsaldir. Yani NIST'in "emergent property" itirazı (§1.3) ISO
   tarafında **paylaşılmıyor**: iki uluslararası standart bu noktada birbiriyle gerilim
   hâlindedir. Bu gerilim ENS'in kararını *belirlemez*, ama görünür kılar.

2. **Kritik bulgu — Accountability ve Non-repudiation zaten ENS'te var ama güvenlik adı
   altında değil.** ENS'in Madde VIII (İzlenebilirlik Yasası) ve `ProofTrace`'i, ISO 25010'un
   *accountability* ve *non-repudiation* alt-karakteristikleriyle **birebir aynı işi** yapar.
   Bu, ENS'in güvenlik gereksinimini zaten taşıdığı ama başka adla taşıdığı tezinin en somut
   kanıtıdır. Doğrudan sonuç: **eklenecek nitelik yeni bir yük getirmez, var olan yükü
   adlandırır.**

3. **Adlandırma emsali.** ISO 25010'un tercih ettiği ad tek sözcük: **Security**. Madde V'in
   üslubuyla (Modular, Observable, Testable…) uyumlu olan sıfat biçimi **Secure**'dur.

**Doğrulama notu:** ISO'nun kendi sayfaları (`iso.org` ve OBP) bu oturumda WebFetch'e **HTTP
403** döndürdü. Karakteristik ve alt-karakteristik listeleri arc42 Quality Model, Sonar ve
Codacy gibi **ikincil ama tutarlı** kaynaklardan çapraz doğrulandı. Standardın kendi metni
ücretlidir; **birinci elden alıntı yapılmadı ve yapılmamalıdır** (§7).

### 1.5 Quality Attributes / "-ilities" (Bass, Clements & Kazman)

**Künye:** Bass, L., Clements, P. & Kazman, R. (2021). *Software Architecture in Practice.*
4. baskı. Addison-Wesley (SEI Series in Software Engineering). ISBN 978-0136886099.
(3. baskı: 2012, ISBN 978-0321815736.)

**Neden ilgili:** Bu kitap, mimari kalite niteliklerinin (*quality attributes*, halk arasında
"-ilities") kanonik referansıdır ve **Security'ye ayrı bir bölüm** ayırır (4. baskıda kalite
nitelikleri bölümleri: Availability, Deployability, Energy Efficiency, Integrability,
Modifiability, Performance, Safety, **Security**, Testability, Usability). Yani ENS'in Madde V
listesinde bulunan **Testable** ile bulunmayan **Secure**, aynı literatürde aynı düzeyde
kavramlardır — Madde V'in mevcut listesi bu açıdan **eksik bir alt kümedir**.

**Metodolojik katkı — çatışma çözümü:** Kitabın merkezî iddiası, kalite niteliklerinin
**birbiriyle çatıştığı** ve çatışmanın soyut sıralamayla değil, somut **quality attribute
scenario**'larla ve **ATAM** (Architecture Tradeoff Analysis Method) gibi yöntemlerle
çözüldüğüdür. ATAM'ın çekirdek kavramları *sensitivity point* ve **tradeoff point**'tir:
tradeoff point, birden fazla kalite niteliğini aynı anda etkileyen mimari karardır.

**ENS için delta — Madde V'in çatışma kuralına doğrudan itiraz:**
Madde V *"Çatışmada Explainable ve Testable kazanır"* diyerek **statik, bağlamdan bağımsız bir
sıralama** kurar. ATAM literatürünün tezi tam tersidir: çatışma **senaryo bazında** çözülür,
çünkü hangi niteliğin kazanacağı hangi risk kabul edildiğine bağlıdır. Güvenlik eklenirse bu
gerilim keskinleşir (§2).

**Doğrulama notu:** Kitabın 4. baskısının bölüm listesi yayıncı/perakendeci içindekiler
sayfalarından bilinir; **bu oturumda bölüm başlıkları birinci elden doğrulanmadı** (§7).
ATAM'ın birincil künyesi ayrıca doğrulanabilir: Kazman, R., Klein, M. & Clements, P. (2000).
*ATAM: Method for Architecture Evaluation.* CMU/SEI-2000-TR-004. Software Engineering
Institute, Carnegie Mellon University.
URL: <https://insights.sei.cmu.edu/library/atam-method-for-architecture-evaluation/>

---

## 2. Çatışma Kuralı: Explainable/Observable ↔ Security

(doldurulacak)

---

## 3. Adlandırma

(doldurulacak)

---

## 4. Karşı-Argüman: Madde V'e Güvenlik Eklenmemeli

(doldurulacak)

---

## 5. ENS Kusur Sicili ile Eşleme

(doldurulacak)

---

## 6. Kaynakça

(doldurulacak)

---

## 7. Erişilemeyen / Doğrulanamayan Kaynaklar

(doldurulacak)
