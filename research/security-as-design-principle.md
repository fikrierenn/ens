# Araştırma Notu: Bir Tasarım İlkesi Olarak Güvenlik

**Durum:** Tamamlandı (araştırma notu — normatif değil)
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

Bu bölüm görevin **en kritik sorusuna** cevap arıyor: Madde V *"Çatışmada Explainable ve
Testable kazanır"* diyor. Güvenlik eklenirse bu kural ne olur?

**Bulgu özeti — ikiye ayrılıyor:**
Literatür bu ilişkiyi **tek bir çatışma** olarak değil, **iki ayrı ilişki** olarak ele alıyor.
Bunları karıştırmak ENS için en büyük risktir.

### 2.1 İlişki A — *Mekanizmanın* şeffaflığı: ÇATIŞMA YOK, DESTEK VAR

Güvenlik literatürünün kanonik konumu, sistemin **tasarımının/mekanizmasının** açık olması
gerektiğidir.

- **Kerckhoffs (1883), 2. ilke:** *"Il faut qu'il n'exige pas le secret, et qu'il puisse sans
  inconvénient tomber entre les mains de l'ennemi"* — sistem gizlilik gerektirmemeli, düşmanın
  eline geçmesi zarar vermemeli.
  Künye: Kerckhoffs, A. (1883). "La cryptographie militaire." *Journal des sciences militaires*,
  IX, Ocak 1883, s. 5–83 ve Şubat 1883, s. 161–191.
  Tarama: <https://www.petitcolas.net/kerckhoffs/crypto_militaire_1.pdf>
- **Saltzer & Schroeder (1975), open design:** *"The design should not be secret."*
- **NCSC (2019), 4. ilke:** *"Make compromise detection easier"* — gözlemlenebilirlik burada
  güvenliğin **aracıdır**, rakibi değil.
- **ISO/IEC 25010:2023:** *accountability* ve *non-repudiation* güvenliğin **alt-karakteristiği**
  olarak tanımlanır — yani izlenebilirlik güvenliğin parçasıdır.

**ENS için sonuç:** Madde V'in Explainable ilkesi ile güvenlik arasında **bu düzeyde çatışma
yoktur**. Aksine, ENS'in Madde VIII (İzlenebilirlik Yasası) ve `ProofTrace`'i güvenliğin
accountability/non-repudiation ayağını zaten taşır. Güvenlik eklemek Explainable'ı zayıflatmaz;
Explainable'ın *neden* zorunlu olduğuna ikinci bir gerekçe verir.

### 2.2 İlişki B — *Çıktının* ayrıntısı: ÇATIŞMA GERÇEK VE ÖLÇÜLMÜŞ

Çatışma mekanizmanın açıklığında değil, **belirli bir çağırana verilen açıklamanın
ayrıntısındadır**. Bu, literatürde ölçülmüş gerçek bir saldırı yüzeyidir:

| Kanal | Kaynak | Ne kanıtlanmış |
|-------|--------|----------------|
| Hata mesajı | **CWE-209**, "Generation of Error Message Containing Sensitive Information", MITRE CWE. <https://cwe.mitre.org/data/definitions/209.html> | Ayrıntılı hata mesajı saldırgana sorgu mantığını, iç yapıyı, hatta kimlik bilgisini sızdırır. Saldırı yöntemi: fuzzing + stack trace gözlemi. |
| Açıklama (XAI) | **Milli, S., Schmidt, L., Dragan, A. D. & Hardt, M. (2019).** "Model Reconstruction from Model Explanations." *Proc. ACM FAT\* 2019*, 1–9. DOI: `10.1145/3287560.3287562` · arXiv:1807.05185 | Gradyan tabanlı açıklamalar **modelin kendisini hızla ifşa eder**. Yazarların kendi ifadesiyle sonuç, "modeli gizli tutma isteği ile açıklama sunma yeteneği arasındaki bir gerilimi" ortaya koyar. |
| Açıklama (XAI) | **Shokri, R., Strobel, M. & Zick, Y. (2021).** "On the Privacy Risks of Model Explanations." *Proc. AAAI/ACM AIES 2021*. DOI: `10.1145/3461702.3462533` · arXiv:1907.00164 · PDF: <https://www.comp.nus.edu.sg/~reza/files/Shokri-AIES2021.pdf> | Tahmin + açıklama birlikte verildiğinde, bir veri noktasının eğitim kümesinde olup olmadığı **membership inference** ile çıkarılabilir. Yani açıklama ölçülebilir bir sızıntı kanalıdır. |

**ENS için doğrudan risk:** ENS'in `ProofTrace`'i tam da bu kategoridedir. Bir karar
önerisinin gerekçe zinciri, ENS'in iç eşiklerini, ağırlıklarını, memory içeriğini ve hangi
kapının hangi koşulda açıldığını **çağırana** anlatır. Bu, `DEFECT-REGISTER.md` §2'nin tehdit
modeliyle ("saldırgana özel yetki gerekmiyor") birleştiğinde somut bir yüzeydir.

### 2.3 Literatür bu çatışmayı nasıl çözüyor?

Üç yerleşik çözüm biçimi bulundu; hiçbiri "biri kazanır" demiyor:

1. **Ayrım: mekanizma açık, örnek-veri kısıtlı.** Kerckhoffs/open design mekanizmayı açar,
   *anahtarı* açmaz. CWE-209'un standart azaltımı da aynı biçimdedir: hata *kaydedilir* (tam
   ayrıntıyla, iç kanala), *gösterilmez* (dışarıya genel mesaj). Yani **açıklama yok edilmez,
   muhatabı ayrıştırılır.**
2. **Senaryo bazlı tradeoff (ATAM).** Kazman, R., Klein, M. & Clements, P. (2000). *ATAM: Method
   for Architecture Evaluation.* CMU/SEI-2000-TR-004, Software Engineering Institute, Carnegie
   Mellon University, Ağustos 2000.
   <https://www.sei.cmu.edu/library/atam-method-for-architecture-evaluation/>
   Yöntemin tezi: kalite nitelikleri arasındaki çatışma **soyut bir sıralamayla değil**, somut
   senaryolar üzerinden *tradeoff point* tespitiyle çözülür.
3. **Nicel açıklama bütçesi (diferansiyel gizlilik).** Patel, N., Shokri, R. & Zick, Y. (2022).
   "Model Explanations with Differential Privacy." *Proc. ACM FAccT 2022*.
   DOI: `10.1145/3531146.3533235`. Açıklamayı kapatmak yerine sızıntıyı **ölçülü** hâle getirir.

### 2.4 ENS'e söylediği: mevcut çatışma kuralı nasıl etkilenir?

**Bulgu:** *"Explainable kazanır"* kuralı, §2.1 anlamında okunduğunda güvenliği **feda etmez**
— aksine ona hizmet eder. §2.2 anlamında okunduğunda ise **feda edebilir**, ve Madde V bugün bu
iki anlamı ayırt etmiyor.

Bu, ENS'in kendi terminolojisiyle bir **terminolojik belirsizliktir**: Madde V "Explainable"ın
*kime karşı* açıklanabilir olduğunu söylemiyor. Literatürün cevabı net — açıklamanın muhatabı
tanımlanmadan güvenlik/şeffaflık dengesi kurulamaz (CWE-209'un iki-muhatap modeli; Milli et al.
2019'un "kim sorgu yapıyor" varsayımı).

**Malzeme, karar değil:** RFC'ye taşınabilecek üç seçenek literatürden türetilebilir —
(a) Explainable'ın üstünlüğü korunur ama *muhatap* eklenir (iç kanal tam, dış kanal
kısıtlı — CWE-209 modeli); (b) çatışma statik sıralamadan çıkarılıp senaryo bazlı karara
bağlanır (ATAM modeli); (c) güvenlik listeye eklenir ama çatışma kuralına **girmez**, çünkü
§2.1 uyarınca Explainable ile aynı yönü gösterir. Seçim Madde XV yordamınındır.

---

## 3. Adlandırma

Madde V'in üslubu tek sözcüklü sıfat/nitelik adıdır (Modular, Observable, Testable,
Replaceable, Versioned, Explainable, Deterministic). Aday adlar ve literatürdeki yükleri:

| Aday | Literatür dayanağı | Kapsam | Belirsizlik riski |
|------|--------------------|--------|-------------------|
| **Secure** | ISO/IEC 25010:2023 (Security, 9 karakteristikten biri); Bass/Clements/Kazman (Security bölümü) | En geniş: confidentiality, integrity, non-repudiation, accountability, authenticity, resistance | **Yüksek** — "herkes güvenli der"; ölçüsüz kalırsa boş etiket (§4) |
| **Fail-safe** | Saltzer & Schroeder (1975), *fail-safe defaults*; ISO/IEC 25010:2023'te *Safety*'nin alt-karakteristiği olarak da geçer | Dar ve keskin: varsayılan **ret** | **Düşük** ama **eksik** — kalıp 1 (taklit edilebilir yetki) ve kalıp 2'yi (kimlik normalizasyonu) kapsamaz |
| **Least-privilege** | Saltzer & Schroeder (1975); Smith (2012) bunu "yerleşen" ilkelerden sayar | Yetki genişliği | **Düşük** ama **çakışıyor** — P7/BoundedAutonomyGate'in policy zarfı zaten budur (aşağı bkz.) |
| **Mediated** | Saltzer & Schroeder, *complete mediation* | Her erişim, her nesne, her aşama denetlenir | Orta — terim ENS dışında az tanınır |
| **Attested / Non-repudiable** | ISO 25010 *non-repudiation* + *authenticity* | İmzalı gate-token ailesi (kalıp 1) | Dar |

### 3.1 Bulgu: tek sözcük yetmiyor gibi görünüyor — ama iki farklı nedenden

**Neden 1 (kapsam):** ENS'in kusur kalıpları **tek bir güvenlik ilkesinin** ihlali değil,
en az üçünün ihlalidir (§5). `Fail-safe` tek başına kalıp 4'ü kapsar, kalıp 1'i ve 7'yi
kapsamaz.

**Neden 2 (soyutlama düzeyi):** ISO 25010 emsali gösteriyor ki bu tür listelerde doğru düzey
**karakteristik** düzeyidir (Security), ilke düzeyi değil (fail-safe defaults). Madde V'in
diğer üyeleri de karakteristik düzeyindedir: "Observable" da bir ilke değil, bir niteliktir;
altında "structured logging", "correlation id" gibi ilkeler yatar.

**Bu ikisinin sonucu:** Literatür emsali **`Secure`** tek sözcüğünü destekler (ISO 25010'un
kendi tercihi, Bass et al.'ın bölüm adı), **koşuluyla ki** altındaki ilke kümesi bir alt
belgede (Külliyat'ta bir ADR ya da ENS-3xxx) sayılsın. Aksi hâlde §4'ün "ucuzlatma" itirazı
haklı çıkar.

### 3.2 P7 / `BoundedAutonomyGate` ile ilişki — çakışma var mı?

ADR-0001 §5.6'ya göre `BoundedAutonomyGate`, insanın koyduğu **Policy** zarfını (= ENS-4010
`Constraint` bundle: bütçe, geri-dönülemezlik eşiği, izinli/yasak capability, risk toleransı)
ihlal eden action'ı **bloklar ve insana eskale eder**.
Kaynak: `5000-architecture/adr/ADR-0001-agent-runtime.md:336-371`

Güvenlik ilkeleriyle eşlemesi:

- Policy zarfı = **least privilege** (Saltzer & Schroeder f). İzinli/yasak capability listesi
  tam olarak "işi bitirmeye yeten en küçük yetki kümesi"dir.
- Gate'in her action'ı kontrol etmesi = **complete mediation** (c) — *hedeflenen* biçimiyle.
- Zarf dışına çıkanın bloklanması = **fail-safe defaults** (b) — *hedeflenen* biçimiyle.

**Çakışma değerlendirmesi:** P7 bir **ilke** (kim sorumlu), `BoundedAutonomyGate` bir
**mekanizma** (nasıl uygulanır). Madde V'e eklenecek nitelik ise bir **kalite gereksinimidir**
(her bileşen ne taşımalı). Üçü farklı katmanlardır; **mantıksal çakışma yoktur**.

Ama **gerçek bir risk var ve adı konmalı:** P7 zaten var olduğu için "güvenlik zaten P7'de"
denilebilir. Bu yanlış olur, çünkü:

1. P7'nin anayasal metni *"Sorumluluk insandadır. ENS önerir; emretmez."*tir
   (`ENS-0000:69`) — bu bir **otonomi sınırı** ilkesidir, bir güvenlik ilkesi değil. Kimlik
   normalizasyonu (kalıp 2), zaman doğrulaması (kalıp 3) veya canlı koleksiyon sızıntısı
   (kalıp 6) P7'den **türetilemez**.
2. `DEFECT-REGISTER.md` §2 açıkça iki ayrı güvenceden söz eder: *"bir **P6** (izlenebilirlik)
   veya **P7** (sınırlı özerklik) güvencesi düşürülüyor"* (satır 163). Yani ENS'in kendi kusur
   sicili bile güvenliği tek bir ilkeye bağlamıyor.
3. **Ayrıca uyarı:** SCAN-01/Ç-5'in tespit ettiği gibi "Bounded Autonomy" adı Anayasa'da ve
   Sözlük'te **yoktur**; ADR-0001'in mimari adıdır. Yeni bir Madde V niteliği bu adla
   ilişkilendirilirse aynı atıf hatası büyütülmüş olur.

**Somut adlandırma riski:** `Least-privilege` seçilirse P7/BoundedAutonomyGate ile **anlam
örtüşmesi** doğar ve okur "bu zaten var" der. `Secure` ya da `Fail-safe` bu örtüşmeyi doğurmaz.

---

## 4. Karşı-Argüman: Madde V'e Güvenlik Eklenmemeli

Görev bunu zorunlu kıldı; ve arama sonucunda **beklenenden güçlü** bir karşı-argüman kümesi
çıktı. Dördü ayrı ayrı sunuluyor, en güçlüsü işaretlendi.

### 4.1 ★ EN GÜÇLÜ — "Güvenlik yanlışlanamaz; Madde X onu reddeder"

**Künye:** Herley, C. (2016). "Unfalsifiability of security claims." *Proceedings of the
National Academy of Sciences (PNAS)*, 113(23), 6415–6420. DOI: `10.1073/pnas.1517797113`
Açık erişim: <https://pmc.ncbi.nlm.nih.gov/articles/PMC4988610/> ·
<https://www.pnas.org/content/113/23/6415.full.pdf>
Ayrıca: Herley, C. (2016). "The Unfalsifiability of Security Claims." *USENIX Security '16*.

Herley'in tezi (özetten, yazarın kendi ifadesi): *"We show that claims that any measure is
necessary for security are empirically unfalsifiable. That is, no possible observation
contradicts a claim of the form 'if you don't do X you are not secure.' If we are wrong about a
measure being sufficient, a successful attack will demonstrate that fact, but if we are wrong
about necessity, no possible observation reveals the error."*

**ENS için neden yıkıcı:** ENS Anayasası **Madde X — Yanlışlanabilirlik Ödevi**, her Külliyat
yapıtından *failure conditions* ister. Madde V'e `Secure` eklenirse, o niteliğin yanlışlanma
kipini yazmak gerekir. Herley'in kanıtı şudur: "güvenli değil" gözlemlenebilir, ama "güvenli"
gözlemlenemez. Diğer Madde V nitelikleri bu asimetriyi taşımaz — bir bileşenin *Testable*,
*Observable*, *Versioned*, *Replaceable* olduğu **pozitif olarak gösterilebilir**. `Secure`
listeye girerse, Madde V'in **tek yanlışlanamaz üyesi** olur.

**Bu argümanın kendi zayıflığı (dürüstlük gereği):** Herley *gereklilik* iddialarının
yanlışlanamazlığını gösterir, *yeterlilik* iddialarının değil. "Fail-safe defaults kullanılmalı"
gereklilik iddiasıdır (yanlışlanamaz); "bu bileşen 0 eşiğinde açılıyor" ise **gözlemlenebilir
bir kusurdur** (`DEFECT-REGISTER.md`'nin 75 kusuru tam olarak budur). Yani Herley'in itirazı
**`Secure` gibi geniş bir ada karşı güçlü, `Fail-safe` gibi dar ve gözlemlenebilir bir ada karşı
zayıftır.** Bu, §3'ün adlandırma tercihini doğrudan etkiler.

### 4.2 Güvenlik bir sistem özelliğidir, bileşen niteliği değil (NIST)

NIST SP 800-160 v1r1'in merkezî tezi (§1.3): güvenlik bir **emergent property**'dir. Madde V ise
*"**Her ENS bileşeni:** …"* diye başlar. Her bileşenin ayrı ayrı "güvenli" olması sistemin
güvenli olduğunu **göstermez** — ve ENS'in kendi sicili bunu görgül olarak doğrular:
kalıp 7 ("girdi kapısı var, çıktı kapısı yok") bileşenler *arasındaki* bir boşluktur, hiçbir tek
bileşenin kusuru değildir. Madde V bu kusuru tanımlayamaz; onu tanımlayacak yer bir **mimari
yasa** (Madde VIII ailesi) ya da bir ADR'dir.

### 4.3 Güvenlik bir süreçtir, bir ürün/nitelik değil (Schneier, ISO 27001, NCSC)

- **Künye:** Schneier, B. (2000). "The Process of Security." *Information Security Magazine*,
  Nisan 2000. <https://www.schneier.com/essays/archives/2000/04/the_process_of_secur.html>
  Alıntı: *"Security is a process, not a product. Products provide some protection, but the only
  way to effectively do business in an insecure world is to put processes in place that
  recognize the inherent insecurity in the products."*
- **ISO/IEC 27001:2022**, *Information security management systems — Requirements.*
  <https://www.iso.org/standard/27001> — güvenliği bir **yönetim sistemi** (ISMS) ve risk
  yönetimi döngüsü olarak kurar; bir ürün kalite karakteristiği olarak değil. Dikkat: ISO'nun
  kendi ailesinde güvenlik **iki ayrı standartta** yaşar — 25010 (ürün kalitesi) ve 27001
  (yönetim süreci). Bu ayrım ENS için tam olarak "Madde V mi, yoksa bir süreç yasası mı?"
  sorusudur.
- **NCSC'nin kendi ifadesi (7 Mart 2018):** *"Secure by Default isn't a set of requirements or
  an assurance scheme. There's no compliance badge or logo for products meeting a set of
  requirements. It's more like an ethos or a philosophy."*
  <https://www.ncsc.gov.uk/information/secure-default>
  Yani devlet otoritesinin kendisi, güvenliği **normatif kapalı bir listeye** koymanın tip
  hatası olduğunu söylüyor.

### 4.4 "Ucuzlatma" itirazı — herkes güvenlik der, kimse ölçmez

Bu itiraz literatürde şu biçimde destekleniyor: bir kalite niteliği **doğrulanabilir ve mümkünse
ölçülebilir** olmalıdır; aksi hâlde mimari değerlendirmede işe yaramaz. Bass/Clements/Kazman ve
SEI'nin ATAM/QAW çizgisi bunu **quality attribute scenario** zorunluluğuyla çözer:
uyaran (stimulus) → kaynak → yanıt → **yanıt ölçüsü**.
Referans: Bass, Clements & Kazman (2021), *Software Architecture in Practice*, 4. baskı;
Kazman, Klein & Clements (2000), CMU/SEI-2000-TR-004.

**ENS için sonuç:** Madde V'e senaryosuz/ölçüsüz bir `Secure` eklemek, listenin diğer
üyelerini de ucuzlatır — çünkü Madde V'in gücü **kapalı ve denetlenebilir** olmasından gelir.
Bu itiraz, "eklenmemeli" demek yerine "**ölçü olmadan eklenmemeli**" demeye de dönüştürülebilir;
hangi biçimde alınacağı RFC'nin işidir.

### 4.5 Karşı-argümanların karşı-argümanı (denge için)

Dürüstlük gereği, yukarıdakilerin hepsine karşı **ISO/IEC 25010:2023 tek başına** duruyor: 9
karakteristikli uluslararası ürün kalite modeli, Security'yi tam da bir **ürün niteliği** olarak
listeliyor ve 2023'te alt-karakteristiklerini genişletiyor (*resistance* eklendi). Yani
"güvenlik bir kalite niteliği olamaz" iddiası, **en yerleşik uluslararası standardın
uygulamasıyla çelişir**. Karar bu iki emsal arasındadır, ve bu not o kararı vermez.

---

## 5. ENS Kusur Sicili ile Eşleme

Bu bölüm literatür değil, **ENS'in kendi görgül kanıtıdır**. Kaynak:
`7000-reference-implementation/DEFECT-REGISTER.md` (75 kusur, §7'de 8 kök-neden kalıbı).
Sol sütun ENS'in kendi adlandırması; sağ sütun Saltzer & Schroeder (1975) / ISO 25010:2023
karşılığı.

### 5.1 Kalıp → ilke eşlemesi

| ENS kalıbı (§7) | Örnek kusurlar | İhlal edilen ilke | Kaynak |
|---|---|---|---|
| **1. Public record = taklit edilebilir yetki** | E3, W4a, W15, H1 | **Complete mediation** (her erişim yetkiye karşı denetlenmeli) + ISO 25010 **authenticity**, **non-repudiation** | Saltzer & Schroeder (1975) c; ISO/IEC 25010:2023 |
| **2. Kimlik normalizasyonu yok** (≥11) | F3, G3, G4, W7f, W2c (homoglyph), W2e (`NUL`), W2f | **Complete mediation**'ın ön koşulu: denetlenecek nesnenin kimliği tek olmalı. Ayrıca **economy of mechanism** (serbest string = denetlenemez yüzey) | Saltzer & Schroeder (1975) a, c |
| **3. Zaman çağırandan geliyor** | A1, A2, D4, W2_L3, W2_R6 | **Complete mediation** (denetim zaman damgası da denetlenmeli) + ISO 25010 **integrity** | Saltzer & Schroeder (1975) c |
| **4. Eşik `0` = sessiz kapatma** | A5, E4, G2, H3 | **Fail-safe defaults** — *"base access decisions on permission rather than exclusion"*. `0` fail-**open** yapıyor. **En temiz eşleşme.** | Saltzer & Schroeder (1975) b |
| **5. Reflection tüm değişmezleri deler** | E5, W3c | **Least common mechanism** + **economy of mechanism**; ENS'in kendisi bunu bir *kapsam kararı* sayıyor (§8.3) | Saltzer & Schroeder (1975) a, g |
| **6. Canlı koleksiyon dönüyor** | W22, W2_R4, W2_L4, W5a, W5b | ISO 25010 **integrity** (yetkisiz değişikliğin engellenmesi) | ISO/IEC 25010:2023 |
| **7. Girdi kapısı var, ÇIKTI kapısı yok** | W8a, W8b, W17, H4, W5e, W3 | **Complete mediation** — Saltzer & Schroeder mediation'ı yalnızca girişe değil, *"initialization, recovery, shutdown, and maintenance"* dâhil **her** aşamaya ister | Saltzer & Schroeder (1975) c |
| **8. Öz-beyan kalibre edilmemiş** | B1, B2, G5, W7, W7d, W2_P4 | ISO 25010 **non-repudiation** + **accountability**; ayrıca Saltzer & Schroeder **compromise recording** (kaydın kendisi güvenilir olmalı) | ISO/IEC 25010:2023; Saltzer & Schroeder (1975) |

### 5.2 Bulgular

**(a) Görev hipotezi doğrulandı.** *"Fail-safe defaults ENS'in fail-open kusur ailesiyle
doğrudan örtüşüyor gibi"* — evet. Kalıp 4'ün tamamı (A5, E4, G2, H3) tam olarak
Saltzer & Schroeder'in b ilkesinin ihlalidir. Sicilin kendi cümlesi bunu literatürsüz olarak
zaten söylüyor: *"bir eşiğe `0` veya negatif değer vermek, tüm şirket çapında bir yönetişim
mekanizmasını **hata vermeden** kapatıyor"* (satır 191-192).

**(b) Tek hâkim ilke: complete mediation.** 8 kalıptan **4'ü** (1, 2, 3, 7) complete
mediation'ın türevidir. Bu, §3'ün adlandırma tartışması için önemlidir: eğer tek bir ilke
seçilecekse, ENS'in görgül kusur dağılımı `Fail-safe`i değil **`Mediated`/complete mediation**'ı
işaret eder. Ama Smith (2012) tam da complete mediation'ı "gelişemeyen" ilkelerden sayar —
yani literatürde az benimsenmiş bir ilkeyi ENS'in anayasasına almak, dışarıdan okunabilirliği
düşürür. Bu bir tradeoff'tur, karar değil.

**(c) Sicilin kendi tehdit modeli bir güvenlik modelidir.** §2'nin giriş cümlesi —
*"Bu 14 kusurun ortak özelliği: **saldırgana özel yetki gerekmiyor.** Public API'yi normal
şekilde çağırarak … güvencesi düşürülüyor"* (satır 162-163) — literatürdeki *unprivileged
attacker* tehdit modelidir. ENS bu modeli **anayasal dayanak olmadan** kullanıyor.

**(d) W1b: "yanlış güvenlik hissini sistem kendisi üretiyor".** Sicilin kendi vurgusu
(satır 182-184) doğrudan **psychological acceptability** (Saltzer & Schroeder h) ihlalidir:
operatörün zihinsel modeli ile sistemin gerçek koruma durumu ayrışıyor. Bu, sekiz ilkenin
en az atıf alanının ENS'te en pahalı kusuru ürettiğine dair somut bir örnektir.

**(e) D4 = bilgi sızıntısı, §2.2'nin doğrulaması.** Sicil D4 için açıkça *"ayrıca bir bilgi
sızıntısıdır"* diyor (satır 211). Yani ENS'te "çıktının fazla şey söylemesi" kusuru **zaten
gözlemlenmiş** — §2.2'nin XAI literatürüyle işaret ettiği risk teorik değil, ENS'te gerçek.

### 5.3 Uyarı — bu eşleme neyi kanıtlamaz

Eşleme, "ENS'te güvenlik kusuru var → Madde V'e güvenlik eklenmeli" çıkarımını **kanıtlamaz**.
Herley (2016, §4.1) tam da bu çıkarım biçimini hedef alır: kusurların varlığı bir tedbirin
*yeterli olmadığını* gösterir, belirli bir tedbirin *gerekli olduğunu* göstermez. Eşlemenin
kanıtladığı daha dar bir şeydir: **ENS'in kusurları rastgele değil, yerleşik güvenlik
ilkelerinin bilinen ihlal kalıplarına düşüyor** — yani ENS zaten örtük olarak bu ilkelere
tabi. Örtük olanın anayasal mı yoksa mimari mi olacağı ayrı bir sorudur.

---

## 6. Kaynakça

### Birincil — güvenlik tasarım ilkeleri

1. **Saltzer, J. H. & Schroeder, M. D. (1975).** "The Protection of Information in Computer
   Systems." *Proceedings of the IEEE*, 63(9), 1278–1308. DOI: `10.1109/PROC.1975.9939`.
   Tam metin: <https://www.cs.virginia.edu/~evans/cs551/saltzer/> ·
   Yazar yayın listesi: <https://web.mit.edu/Saltzer/www/publications/pubs.html>
2. **Kerckhoffs, A. (1883).** "La cryptographie militaire." *Journal des sciences militaires*,
   IX, s. 5–83 (Ocak) ve s. 161–191 (Şubat).
   Tarama: <https://www.petitcolas.net/kerckhoffs/crypto_militaire_1.pdf>
3. **Smith, R. E. (2012).** "A Contemporary Look at Saltzer and Schroeder's 1975 Design
   Principles." *IEEE Security & Privacy*, 10(6), 20–25. DOI: `10.1109/MSP.2012.85`.

### Standartlar ve devlet rehberliği

4. **ISO/IEC 25010:2023.** *Systems and software engineering — SQuaRE — Product quality model.*
   2. baskı. <https://www.iso.org/standard/78176.html>
5. **ISO/IEC 27001:2022.** *Information security management systems — Requirements.*
   <https://www.iso.org/standard/27001>
6. **Ross, R., Winstead, M. & McEvilley, M. (2022).** *Engineering Trustworthy Secure Systems.*
   NIST SP 800-160 Vol. 1 Rev. 1. DOI: `10.6028/NIST.SP.800-160v1r1`.
   <https://csrc.nist.gov/pubs/sp/800/160/v1/r1/final>
7. **CISA, NSA, FBI ve uluslararası ortaklar (2023).** *Shifting the Balance of Cybersecurity
   Risk: Principles and Approaches for Secure by Design Software.* İlk yayın 13 Nisan 2023;
   güncelleme 16 Ekim 2023. <https://www.cisa.gov/resources-tools/resources/secure-by-design>
8. **NCSC (2019).** *Secure design principles: Guides for the design of cyber secure systems.*
   v1.0, 21 Mayıs 2019. <https://www.ncsc.gov.uk/collection/cyber-security-design-principles>
9. **NCSC (2018).** *Secure by Default.* 7 Mart 2018.
   <https://www.ncsc.gov.uk/information/secure-default>
10. **MITRE.** *CWE-209: Generation of Error Message Containing Sensitive Information.*
    <https://cwe.mitre.org/data/definitions/209.html>

### Mimari kalite nitelikleri

11. **Bass, L., Clements, P. & Kazman, R. (2021).** *Software Architecture in Practice.*
    4. baskı. Addison-Wesley (SEI Series). ISBN 978-0136886099.
12. **Kazman, R., Klein, M. & Clements, P. (2000).** *ATAM: Method for Architecture Evaluation.*
    CMU/SEI-2000-TR-004, Software Engineering Institute, Carnegie Mellon University.
    <https://www.sei.cmu.edu/library/atam-method-for-architecture-evaluation/>

### Karşı-argüman ve gerilim kaynakları

13. **Herley, C. (2016).** "Unfalsifiability of security claims." *PNAS*, 113(23), 6415–6420.
    DOI: `10.1073/pnas.1517797113`. Açık erişim:
    <https://pmc.ncbi.nlm.nih.gov/articles/PMC4988610/>
14. **Schneier, B. (2000).** "The Process of Security." *Information Security Magazine*, Nisan
    2000. <https://www.schneier.com/essays/archives/2000/04/the_process_of_secur.html>
15. **Milli, S., Schmidt, L., Dragan, A. D. & Hardt, M. (2019).** "Model Reconstruction from
    Model Explanations." *Proc. ACM FAT\* 2019*, 1–9. DOI: `10.1145/3287560.3287562` ·
    arXiv:1807.05185.
16. **Shokri, R., Strobel, M. & Zick, Y. (2021).** "On the Privacy Risks of Model Explanations."
    *Proc. AAAI/ACM AIES 2021*. DOI: `10.1145/3461702.3462533` · arXiv:1907.00164 ·
    <https://www.comp.nus.edu.sg/~reza/files/Shokri-AIES2021.pdf>
17. **Patel, N., Shokri, R. & Zick, Y. (2022).** "Model Explanations with Differential Privacy."
    *Proc. ACM FAccT 2022*. DOI: `10.1145/3531146.3533235`.

### Güncel / ikincil ilgi

18. **Patnaik, N., Hallett, J. & Rashid, A. (2024).** "Saltzer & Schroeder for 2030: Security
    engineering principles in a world of AI." arXiv:2407.05710, 8 Temmuz 2024.
    <https://arxiv.org/abs/2407.05710> — AI üretimi kodun yaygınlaşmasıyla klasik ilkelerin
    uyarlanması gerektiğini savunur. ENS'in agentic runtime bağlamıyla doğrudan ilgili; **tam
    metin bu oturumda okunmadı**, yalnızca özet doğrulandı.

### ENS iç kaynakları

19. `0000-constitution/ENS-0000-constitution.md:121-126` — Madde V metni.
20. `7000-reference-implementation/DEFECT-REGISTER.md` — 75 kusur, §7'de 8 kök-neden kalıbı.
21. `5000-architecture/adr/ADR-0001-agent-runtime.md:336-371` — Bounded Autonomy (P7) mekanizması.
22. `governance/SCAN-01-authority-citations.md` — Madde V'ten "güvenli" hayalet-alıntısı; Ç-5
    "Bounded Autonomy" adlandırma bulgusu.

---

## 7. Erişilemeyen / Doğrulanamayan Kaynaklar

**Bu bölüm zorunludur.** Aşağıdakiler bu oturumda **birinci elden okunamadı**; ilgili
bölümlerde de işaretlendi. Bunlara dayanan hiçbir cümle "birincil kaynaktan alıntı" olarak
kullanılmamalıdır.

| Kaynak | Durum | Ne yapıldı |
|---|---|---|
| ISO/IEC 25010:2023 tam metni | **Erişilemedi** — `iso.org` ve OBP HTTP 403; standart ayrıca ücretli | Karakteristik/alt-karakteristik listeleri arc42 Quality Model, Sonar, Codacy üzerinden çapraz doğrulandı. **Standarttan doğrudan alıntı yapılmadı.** |
| ISO/IEC 27001:2022 tam metni | **Erişilemedi** — ücretli | Yalnızca "yönetim sistemi / süreç yaklaşımı" tezi için genel referans verildi |
| CISA *Shifting the Balance…* PDF'i | **Erişilemedi** — `cisa.gov` HTTP 403 | Üç ilke DoD CSIAC, DON CIO CHIPS, HS Today üzerinden doğrulandı; **CISA'nın kendi metninden alıntı yapılmadı** |
| NIST SP 800-160 v1r1 tam metni | **Okunmadı** (≈300 sayfa) | "Emergent property" tezi NIST CSRC duyuru sayfasından alındı. **Appendix'lerdeki "systems security design principles" kataloğu madde madde doğrulanmadı** — RFC'ye girecekse ayrıca okunmalı |
| Bass, Clements & Kazman (2021) 4. baskı bölüm listesi | **Doğrulanmadı** | Kitabın Security bölümü içerdiği genel bilgidir; bölüm başlıkları birinci elden teyit edilmedi |
| Kazman, Klein & Clements (2000) CMU/SEI-2000-TR-004 tam metni | **Kısmen** | Künye (başlık, yazarlar, rapor no, Ağustos 2000) SEI kütüphanesinden **doğrulandı**; *sensitivity point* / *tradeoff point* tanımları PDF'ten değil, ikincil kaynaklardan |
| Smith (2012) *IEEE S&P* tam metni | **Okunmadı** | Künye ve "hangi ilkeler yerleşti/yerleşmedi" özeti arama sonuçlarından; DOI doğrulandı |
| Herley (2016) tam metni | **Okunmadı** | Özet ve künye doğrulandı (PNAS 113(23):6415–6420, DOI teyitli); açık erişim PMC bağlantısı verildi |
| Milli et al. (2019), Shokri et al. (2021), Patel et al. (2022) | **Okunmadı** | Künye + DOI + özet düzeyinde doğrulandı; iddialar özetlerin ötesine taşınmadı |
| Kerckhoffs (1883) orijinal | **Kısmen** | 2. ilkenin Fransızca özgün cümlesi ve cilt/sayfa bilgisi doğrulandı; makalenin tamamı okunmadı |
| Saltzer & Schroeder DOI | **Belirsizlik notu** | İki DOI dolaşımda: `10.1109/PROC.1975.9939` (SciRP referanslarında) ve bazı kaynaklarda `10.1109/PROC.1975.1090`. **Cilt/sayı/sayfa (63(9):1278–1308) kesindir**; DOI kullanılacaksa IEEE Xplore'dan teyit edilmelidir |

**Ayrıca doğrulanamayan iddia:** Görev metnindeki *"ISO/IEC 25010 … Security'yi sekiz kalite
karakteristiğinden biri sayar"* ifadesi **2011 baskısı** için doğrudur; **2023 baskısında dokuz
karakteristik** vardır (§1.4). RFC'de hangi baskıya atıf yapılacağı açıkça yazılmalıdır.

---

## 8. `ens-philosopher` ve `ens-skeptic` için özet

**İddia → Prior art → Örtüşme → Delta → Risk** biçiminde:

| # | İddia | Prior art | Örtüşme | Delta | Risk |
|---|---|---|---|---|---|
| 1 | "Güvenlik bir tasarım ilkesi olarak formüle edilebilir" | Saltzer & Schroeder (1975), 8 ilke | Tam — 50 yıllık yerleşik literatür | ENS'in katkısı sıfır; bu **alınacak**, üretilecek bir şey değil | Yeniden keşif; ENS'in özgünlük iddiası burada olamaz |
| 2 | "Madde V bir kalite-nitelik listesidir, Security oraya aittir" | ISO/IEC 25010:2023 (9 karakteristik, Security dâhil) | Yüksek — yapısal emsal | Madde V *anayasal* ve *kapalı*; 25010 tavsiye niteliğinde ve genişletilebilir | Kapalı listeye yanlışlanamaz üye eklemek (§4.1) |
| 3 | "ENS fail-open kusurları taşıyor" | Fail-safe defaults (1975 b) | Tam — kalıp 4 birebir | Yok | — |
| 4 | "Explainable kazanır kuralı güvenliği feda ediyor" | **Kısmen yanlış.** Kerckhoffs/open design + NCSC 4. ilke + ISO accountability → çatışma yok | Mekanizma düzeyinde örtüşme | Çatışma yalnızca **çıktı ayrıntısı** düzeyinde (CWE-209, Milli 2019, Shokri 2021) | Madde V "Explainable"ın **muhatabını** tanımlamıyor; asıl açık burada |
| 5 | "Güvenlik P7'de zaten var" | ADR-0001 §5.6 policy zarfı ≈ least privilege | Kısmi | P7 kalıp 2/3/6'yı türetemez | "Zaten var" savunması sicilin 40+ kusurunu açıklamıyor |
| 6 | "Eklenmemeli" | Herley (2016), Schneier (2000), NIST emergent property, NCSC "ethos not requirements" | — | Bu argümanlar `Secure`'a karşı güçlü, `Fail-safe`e karşı zayıf | Zayıf ad seçilirse itiraz haklı çıkar |

**Bu belgenin kendi failure condition'ı:** Eğer (a) ISO/IEC 25010:2023'ün Security
karakteristiği birinci elden okunduğunda burada yazılandan farklıysa, ya da (b) Herley'in
yanlışlanamazlık tezi `Fail-safe defaults` gibi dar/gözlemlenebilir adlara da uygulanabiliyorsa,
§3'ün adlandırma önerisi ve §4.1'in sınırlaması **çürür**.
