---
id: SCAN-01
type: skeptic-scan
origin: ens-skeptic
status: review
owner: ens-skeptic
version: 1.0.0
last_reviewed: 2026-07-27
verdict: kısmen-güvenilir
scope: [1000-philosophy, 2000-theory, 3000-laws, 4000-ontology, 5000-architecture, 6000-rfc, 7000-reference-implementation, governance, .claude, root]
---

# SCAN-01 — Külliyat Geneli Uydurma Yetki Atfı Taraması

> **Durum:** TAMAMLANDI (2026-07-27) · **DÜZELTMELER UYGULANDI** (2026-07-27, §8).
> Verdict (tarama anı): **kısmen güvenilir** (§7).
> Denetlenen atıf: ≈599 · Kusurlu: ≈70 (%11,7) · Ağır kusurlu: 39 (%6,5).
> Kök neden tek satırda: `governance/roles.md:38` (Ç-1) — **kaynakta düzeltildi** (`7938ca5` turu).
> Uygulama sonrası kalan: yalnızca Ç-7 (~25 hafif örnek) — bkz. §8.2.

## 0. Amaç ve tetikleyici vaka

2026-07-26'da `.claude/rules/` katmanının üç dosyasında *"Madde X gecikmeyi yasaklar"*
iddiası bulundu. Madde X ("Yanlışlanabilirlik Ödevi") düzeltme hızı hakkında tek kelime
içermiyor. Aynı turda ikinci hata: *"Anayasa G2/G3"* — G-ilkeleri Anayasa'da değil,
`governance/000-governance-principles.md`'de. Bu tarama, aynı kalıbın Külliyat'ın
başka yerlerinde olup olmadığını sınar.

Kusur sınıfları:
- **UYDURMA** — maddede olmayan bir hüküm atfediliyor (en ağır, `refuted`)
- **YANLIŞ KAYNAK** — ilke doğru ama yanlış belgeye atfediliyor
- **ÇARPITMA** — madde var ama anlamı kaydırılmış/genişletilmiş

---

## 1. Yer gerçeği (ground truth) referans tablosu

### 1.1 Anayasa maddeleri (ENS-0000, v0.3.0, `0000-constitution/ENS-0000-constitution.md`)

Kaynak: dosyanın tamamı okundu (256 satır). Madde başlıkları ve **gerçekte hükmettikleri**:

| Madde | Satır | Başlık | Gerçekte ne hükmediyor (özet, metne sadık) |
|-------|-------|--------|--------------------------------------------|
| **I** | 39 | Amaç | ENS = yeni kurumsal bilişim disiplini; şirket = decision-producing cognitive system. ERP veri parçalanmasını, ENS karar karmaşıklığını çözer. Başarı ölçütü: 10 yıl sonra öğretilebilirlik. |
| **II** | 49 | Kapsam | ENS bir **standart** olarak yönetilir. Kapsar: Külliyat, türetilmiş mimari, kanıtlayan reference implementation, beliren ürün modülleri, kitap. ENS **değildir**: daha iyi ERP, dashboard, analytics, workflow engine, chatbot. |
| **III** | 58 | Değiştirilemez İlkeler | P1-P8. Sözcükleri iyileştirilebilir, niyetleri kaldırılamaz. P1 decision atom, P2 context>data, P3 memory→zekâ, P4 learning=ölçülmüş sonuç, P5 attention kıt, P6 explainability pazarlıksız, P7 sorumluluk insanda (ENS önerir, emretmez), P8 teori implementasyondan önce. |
| **IV** | 72 | Külliyat | Külliyat = değiştirilemez, **teknolojiden bağımsız** kavramlar gövdesi. Aralık **alanı** belirler, canon'u **değil**. İki bağımsız eksen: `constitutive` (kurucu/normatif mi, ampirik mi) ve `canon` (doğrulanıp girdi mi — kazanılır, ilan edilmez). Anayasa biricik öz-yetkilendiren yapıt (Grundnorm). Madde III = immutable core, I/II/IV-XIV = protective belt. `constitutive:true` → ratifikasyon yolu (tutarlılık/örneklenebilirlik kipinde failure conditions, skeptic); ampirik kanıt zincirine tabi değil. `constitutive:false` → failure conditions + skeptic → `ratified` (M3); tam Canon (M5) yalnızca Faz-4 kanıt zinciriyle. Failure conditions her iki durumda **zorunlu**. |
| **V** | 121 | Tasarım İlkeleri | Modular · Observable · Testable · Replaceable · Versioned · Explainable · Deterministic (mümkünse) · Event-driven · DDD · CQRS · Cloud-native. **Çatışmada Explainable ve Testable kazanır.** |
| **VI** | 128 | Anti-Pattern'ler | Reddedilen: "AI ERP"/"ERP 2.0"/"dashboard"/"copilot" adlandırması; black-box çıktı; Külliyat atfı olmayan mimari; mimari atfı olmayan kod; **terminoloji sürüklenmesi**; pazarlama dili ya da yanlışlanamaz iddialar; reference platform'u teoriyi kanıtlamadan optimize etmek. |
| **VII** | 137 | Faz Modeli | Kapılı fazlar; kapı ancak **çıkış ölçütleri kaydedilince** açılır. **Erken bir fazda bulunan kusur, yukarı akışta düzeltilene dek bağımlı sonraki işi durdurur.** Faz 0..6 haritası. |
| **VIII** | 154 | İzlenebilirlik Yasası | Her yapıt yukarı akışa işaret eder: kod/test → ADR → theory/law/ontology → first principle (Madde III). **Öksüz düğüm bir kusurdur.** Künye (Madde XI) ve `/validate-theory` ile denetlenir. |
| **IX** | 166 | Kavram Tanıtım Kuralı | **Hiçbir mimari, kod, RFC ya da ADR, Külliyat'ta var olmayan bir kavram tanıtamaz.** Yeni kavram önce Külliyat'a girer (philosopher önerir → 2000/4000'de tanımlanır → skeptic saldırır). Teori mimariden, mimari implementasyondan önce. |
| **X** | 174 | Yanlışlanabilirlik Ödevi | Saldırılamayan teoriye güvenilemez. Her Külliyat kavramı **kendi belgesinde** yanlış olacağı koşulları (varsayımlar + başarısızlık modları) taşır. `ens-skeptic` sürekli karşıt inceleme yürütür. Failure conditions'ı olmayan kavram **eksiktir**. — **Düzeltme hızı / gecikme / SLA hakkında tek kelime yok.** |
| **XI** | 181 | Evrensel İzlenebilirlik Başlığı | Her `.md` ve her kaynak modül (README'si aracılığıyla) YAML künye ile **başlar**: id, title, type, canon, origin, depends_on, referenced_by, principles, status, owner, version, last_reviewed. `status` değerleri: `draft\|review\|skeptic-challenged\|ratified\|superseded`. |
| **XII** | 205 | Bağımlılık Grafiği (Yetki Sırası) | Yetki **tek yönde** akar: Anayasa → Külliyat(1000/2000/3000/4000) → Standards → Commands → **Agents (felsefeyi tüketir; asla üretmez)** → Implementation. Paralel: Külliyat→Mimari→Impl; Külliyat→Kitap; Mimari→Testler. Felsefe üreten agent ya da kavram uyduran mimari bu Madde'yi ihlal eder. |
| **XIII** | 227 | Doküman ve Kod Standartları | Doküman stili (DDIA/DDD/Thinking in Systems/RFC), **dil: Türkçe + orijinal teknik terimler**. Kod: demo kod yok, oyuncak mimari yok, kestirme yok; finansal ölçek varsay; reference platform optimize etmeden önce teoriyi kanıtlar. |
| **XIV** | 237 | Karar Yönetişimi (RFC/ADR) | RFC (`6000-rfc/`, `RFC-6xxx`): Draft→Review→Skeptic-Challenged→Accepted\|Rejected→Superseded. ADR (`5000-architecture/adr/`, `ADR-NNNN`): Proposed→Accepted→Superseded. `ens-ceo` hiza, `ens-skeptic` sağlamlık, `ens-style-guardian` tutarlılık; önemli kararlarda hiçbiri atlanmaz. |
| **XV** | 247 | Değişiklik (Amendment) | Anayasa yalnızca şu RFC ile değişir: (a) değiştirilen maddeye atıf yapar, (b) skeptic saldırısından geçer, (c) Madde XIV uyarınca kabul edilir. Madde III yalnızca sözcük düzeyinde değişir. |

**Anayasa'da BULUNMAYAN, sık uydurulan hükümler (negatif liste):**
- Düzeltme **hızı / gecikme yasağı / SLA / "hemen"** — hiçbir maddede yok. Madde VII "durdurur"
  der (blocking), "hızlı düzelt" demez.
- **G1..G7** — Anayasa'da yok; `governance/000-governance-principles.md` (GOV-000) belgesinde.
- **Maturity (M0..M5) / evidence (E0..E4) ölçekleri** — Anayasa'da tanımlı değil; Madde IV yalnızca
  `.claude/standards/maturity-model.md`'ye **atıf** yapar (M5/Faz-4 bağını kurar).
- **Numaralı "P" ilkeleri dışında ilke seti** — Anayasa'nın kendi ilke seti yalnızca P1..P8 (Madde III).

### 1.2 Governance ilkeleri G1..G7 (`governance/000-governance-principles.md`, GOV-000 v0.1.0, satır 32-40)

| İlke | Tam metin (bire bir) |
|------|----------------------|
| **G1** | *Authority follows accountability.* Yetki, sorumlulukla gelir; sorumlu olmayan yetki veremez. |
| **G2** | *No author canonizes their own work.* Bir yapıtı yazan, onu Canonical yapamaz. |
| **G3** | *Validation ve approval ayrıdır.* **Doğrulayan onaylamaz; onaylayan doğrulamaz.** (DİKKAT: "yazan doğrulayamaz" DEĞİL — o G2'nin komşusu, ama G2 bile "canonize edemez" der, "doğrulayamaz" demez.) |
| **G4** | Her Canonical yapıtın **≥2 bağımsız validator'ı** vardır (farklı boyutlardan). |
| **G5** | Governance kararları **izlenebilirdir**. Her karar bir kayıt (SkepticReview/ADR/RFC) ve Meta Model kenarı bırakır; sessiz karar yoktur. |
| **G6** | **İtiraz (appeal) mümkündür.** Her promotion/deprecation kararına gerekçeli itiraz edilebilir; itiraz yeni bir validation turu açar. |
| **G7** | Governance bireylere değil, **Anayasa'ya** hizmet eder. Çatışmada Anayasa kazanır (Madde XV). |

GOV-000'in kendi künyesi: `canon: false`, `constitutive: true`, `origin: ENS-0000, ENS-4001`.
Yani G-ilkeleri Anayasa'dan **türer**, Anayasa'nın **parçası değildir**.

### 1.3 GOV-030 kanonik süreç (`governance/canonical-process.md`)

Zincir: **M3 Stable** → Scientific Validation (ens-skeptic, sci ≥ E3) → Ontology Validation
(Meta Model uyumu, SKR ontology) → Engineering Validation (uygunsa, eng ≥ E3) → **M4 Reference**
(reference platform'da yaşayan implementation, Faz 4) → Operational Evidence (varsa, Faz 5) →
**Governance Approval (son kapı, yalnızca zincir tamsa)** → **M5 Canonical**.

Kurallar: (1) her kapı bir **kanıttır**, oy değil; governance kanıt üretmez. (2) G4: ≥2 bağımsız
boyut validator'ı. (3) G2: author zincirin hiçbir kapısını kendi açamaz. (4) M4+ Faz 4 gerektirir
→ **M5 şu an ulaşılamaz, Canon boştur.** (5) G6 itiraz yeni tur açar.

**Re-grading yetkisi:** GOV-030 §"Re-grading yetkisi", yanlış `canon:true`'ların düzeltilmesini
yetkilendirir; bu bir *canonization* değil **demotion**'dır, G2 kısıtı canonization içindir,
demotion `ens-style-guardian` (Custodian) tarafından yapılabilir.

---

## 2. Bulgular — UYDURMA

### U-1 — `.claude/standards/context-management.md:77-78` — "G2/G3: yazan validate etmez"

**Atfedilen hüküm:**
> *"agent'ı yazan zihin onu validate etmez (G2/G3, bu projede bir kez ihlal edilip düzeltildi
> — bkz. SKR-024→026, SKR-025→027 dersi)."*

Aynı iddia `:83` satırında tekrar: *"Bağımsız validation (Scientific/Ontology/Engineering) |
**Her zaman subagent** (G2/G3 zorunlu)"*.

**Gerçek metin (GOV-000:33-34):**
- G2 — *"Bir yapıtı yazan, onu **Canonical yapamaz**."*
- G3 — *"Validation ve approval ayrıdır. **Doğrulayan onaylamaz; onaylayan doğrulamaz.**"*

**Kusur:** "Yazan doğrulayamaz" hükmü **ne G2'de ne G3'te vardır**. G2 canonization'ı yasaklar,
validation'ı değil; G3 validator↔governance ayrımıdır, author↔validator ayrımı değil. Bu,
2026-07-26'da `.claude/rules/work-protocol.md:41-43`'te **açıkça tespit edilip düzeltilen**
hatanın birebir aynısıdır — düzeltme `rules/` katmanında yapıldı, **`standards/` katmanında
yapılmadı**. Yani hata giderilmiş değil, **taşınmıştır**.

Ağırlaştırıcı: `standards/` katmanı Madde XII yetki grafiğinde `rules/`'un üstündedir
(Anayasa → Külliyat → **Standards** → Commands → Agents). Yanlış hüküm şu an daha **yüksek**
yetki basamağında duruyor.

**Ek kusur (YANLIŞ KAYNAK bileşeni):** her iki satırda da kaynak belge adı **hiç verilmemiş**
("G2/G3" çıplak). Okuyucu için G-ilkelerinin nerede tanımlı olduğu belirsiz; ENS'in kendi
düzeltme notu (`work-protocol.md:41-42`) tam da bu belirsizliğin Anayasa'ya atfa dönüştüğünü
kaydediyor.

**Sınıf:** UYDURMA (hüküm hiçbir ilkede yok) + YANLIŞ KAYNAK (kaynak belge yok).
**Şiddet:** yüksek — `refuted` seviyesi.
**Talep:** `:77-78` → *"agent'ı yazan zihin onu **canonize etmez** (GOV-000 **G2**); doğrulayan
da onaylamaz (**G3**); Canonical için ≥2 bağımsız validator gerekir (**G4**)"*. `:83` → G4 atfı.

### U-2 — `.claude/agents/ens-silent-failure-hunter.md:19` — "sessiz başarısızlık Madde VI ihlalidir"

**Atfedilen hüküm:**
> *"**Sessiz başarısızlık = anayasal kusur.** İz bırakmadan düşen her güvence Madde VI ihlalidir."*

**Gerçek metin (Madde VI, ENS-0000:128-135):** Anti-pattern listesi **kapalı ve sayılıdır**:
ENS'i "AI ERP"/"ERP 2.0"/"dashboard"/"copilot" diye adlandırmak; black-box çıktı (açıklama
nesnesi olmayan öneri); Külliyat atfı olmadan uydurulmuş mimari; mimari atfı olmadan uydurulmuş
kod; terminoloji sürüklenmesi; pazarlama dili / yanlışlanamaz iddialar; reference platform'u
teoriyi kanıtlamadan optimize etmek.

**Kusur:** "İz bırakmadan düşen güvence" bu listede **yoktur**. Madde VI, ENS'in *ne olmadığına*
dair bir liste; runtime güvence-düşmesi hakkında hüküm içermez. İddianın **gerçek ve daha güçlü**
dayanağı mevcuttur ve kullanılmamıştır:
- **GOV-000 G5** — *"Governance kararları izlenebilirdir. Her karar bir kayıt … bırakır;
  **sessiz karar yoktur**."* (birebir aynı iddia)
- **Madde V** — bileşen niteliği **Observable**.
- **Madde VIII** — öksüz düğüm / iz bırakmama kusurdur.

Yani ajan, elinin altındaki doğru yetkiyi bırakıp var olmayan bir hükme atıf yapmış.

**Sınıf:** UYDURMA (Madde VI'da yok) — düzeltmesi kolay, çünkü doğru kaynak mevcut.
**Şiddet:** orta.
**Talep:** *"…her güvence **GOV-000 G5** ('sessiz karar yoktur') ve **Madde V (Observable)**
ihlalidir"* biçiminde düzelt.

---

## 3. Bulgular — YANLIŞ KAYNAK

### Y-1 — "**Anayasa G2/G3**" — Külliyat'ın içinde, hâlâ açık (6 örnek)

Tetikleyici vakada `.claude/rules/` içinde düzeltilen *"Anayasa G2/G3"* kalıbı,
**`2000-theory/` Külliyat yapıtlarında düzeltilmeden duruyor**:

> Satır numaraları 2026-07-27 taraması sırasındaki hâldir; `ENS-2003`/`ENS-2004` eşzamanlı
> düzenlendiğinden kayabilir. Metin kalıbı (`Anayasa G2/G3`) değişmez.

| Dosya:satır | Metin |
|-------------|-------|
| `2000-theory/ENS-2003-company-memory.md:720` | *"**Öz-onay yok (Anayasa G2/G3):** Bu tur yazar tarafından `survives` işaretlenemez."* |
| `2000-theory/ENS-2003-company-memory.md:774` | *"**Öz-onay yok (Anayasa G2/G3).** Bu tur `survives` değildir."* |
| `2000-theory/ENS-2003-company-memory.md:818` | *"**Öz-onay yok (Anayasa G2/G3).** v0.4.1 `survives` değildir."* |
| `2000-theory/ENS-2004-learning-theory.md:418` | *"**Öz-onay yok (Anayasa G2/G3):** bu tur yazar tarafından `survives` …"* |
| `2000-theory/ENS-2004-learning-theory.md:429` | *"**Öz-onay yok (Anayasa G2/G3):** bu düzeltmeler yazar tarafından …"* |
| `2000-theory/ENS-2004-learning-theory.md:491` | *"**Öz-onay yok (Anayasa G2/G3):** `status: ratified → review`."* |
| `2000-theory/ENS-2004-learning-theory.md:520` | *"**Öz-onay yok (Anayasa G2/G3).** v0.4.0 `survives` değildir ve BREAKING'tir."* |
| `2000-theory/reviews/SKR-043-…:17` | *"Yazar öz-onay veremez (**Anayasa G2/G3**)."* |
| `2000-theory/reviews/SKR-044-…:19` | *"…öz-onay veremez (**Anayasa G2/G3**); bu bağımsız tur o boşluğu doldurur."* |
| `5000-architecture/reviews/SKR-037-…:17` | *"Yazar kendi düzeltmesini onaylayamaz (**Anayasa G2/G3**)…"* |

Ayrıca yarı-örtük bir örnek: `7000-reference-implementation/DEFECT-REGISTER-VERIFICATION.md:8` —
*"| **Yetki** | **Anayasa** Madde X (Yanlışlanabilirlik), **G2/G3 (yazar kendi işini
doğrulayamaz)** |"* — burada hem kaynak (aynı hücrede "Anayasa" ile yan yana) hem **içerik**
yanlış (aşağıda Ç-1).

**Toplam canlı örnek: 10 açık "Anayasa G2/G3" + 1 örtük = 11.**

**Gerçek:** Anayasa'nın 15 maddesinin hiçbirinde "G2" ya da "G3" geçmez (ENS-0000 tam metni
okundu; `grep "G[0-9]" 0000-constitution/` → 0 sonuç). G2/G3 `governance/000-governance-principles.md`
(GOV-000) satır 33-34'te tanımlıdır. GOV-000'in kendi künyesi `canon: false`, `origin: ENS-0000` —
yani **Anayasa'dan türeyen, Anayasa'nın parçası olmayan** bir belge.

**Neden önemli:** bu, `.claude/rules/`'daki aynı hatadan **daha ağırdır**, çünkü:
1. Yer, kural katmanı değil **Külliyat**'tır (Faz 1 teori belgesi).
2. Belgeler `status: review`/`ratified` seviyesinde ve skeptic turlarından geçmiş; yani
   **denetim zincirinin kendisi bu yanlış atfı yakalamadan onayladı.**
3. `.claude/rules/SKR-046-tier3-discipline-rules.md:239` bunu 2026-07-26'da zaten görmüş ve yazmış
   (*"`ENS-2003:491` 'Anayasa G2/G3'"*), ama düzeltme yalnızca `rules/` katmanında yapılmış.
   Bilinen ve kayıtlı bir kusurun **düzeltilmeden bırakılması** Madde VII'nin (erken fazdaki
   kusur bağımlı işi durdurur) doğrudan ihlalidir.

**Sınıf:** YANLIŞ KAYNAK (ilke doğru, belge yanlış).
**Şiddet:** yüksek.
**Talep:** her altı satırda `Anayasa G2/G3` → `GOV-000 G2/G3`. **NOT: bu iki dosya şu an başka
bir ajan tarafından düzenleniyor — bu tarama dosyalara dokunmadı; düzeltme owner'a bırakıldı.**

### Y-2 — "alias yasağı — Anayasa **Madde IV**" (5 örnek) — Madde IV'te alias yasağı yok

| Dosya:satır | Metin |
|-------------|-------|
| `2000-theory/ENS-2003-company-memory.md:322` | *"**Adlandırma (alias yasağı, Anayasa Madde IV):** `asserted_at` … P6'nın `Evidence` alanı değildir."* |
| `2000-theory/ENS-2003-company-memory.md:341-342` | *"…niceliğinin ta kendisidir (**alias yasağı, Anayasa Madde IV** — bu yüzden ona ayrı bir ad verilmemiştir)."* |
| `2000-theory/ENS-2004-learning-theory.md:263` | *"**ENS-3023 §Model 1'in `value(d)`'sidir** (yeni kavram değil, **alias yasağı — Anayasa Madde IV**)"* |
| `2000-theory/reviews/SKR-045-…:586` | *"Aynı kelime altında bunları taşımak, **Madde IV'ün alias yasağının** ruhuna aykırıdır…"* |
| `.claude/standards/documentation-style.md:43` | *"Kavram başına tek kanonik terim; sessiz takma ad yok (**Anayasa Madde IV**)."* |

**Gerçek metin (Madde IV, ENS-0000:72-119):** Madde IV'ün konusu Külliyat'ın **tanımı**
(teknolojiden bağımsızlık), **aralık↔alan** ilişkisi ve `constitutive`/`canon` iki-eksenidir.
Terim, ad, alias ya da adlandırma hakkında **tek bir hüküm içermez**.

**Doğru kaynaklar var ve kullanılmamış:**
- **Madde VI** — anti-pattern listesinde birebir: *"terminoloji sürüklenmesi (terminology drift)"*.
- **ENS-4000:19-26** — *"…adlandırmanın tek doğruluk kaynağıdır"*, `:22` *"…sessizce takma
  adlandıramaz"*. Alias yasağının **gerçek** metni budur.
- **Madde IX** — Külliyat'ta olmayan kavramın alt akışta tanıtılamaması.

**Ağırlaştırıcı:** kusuru bir **skeptic review**'in kendisi (`SKR-045:586`) tekrarlıyor. Yani
yanlış atıf, denetim katmanına sızmış ve orada **meşrulaştırılmıştır**. `ens-skeptic`'in görevi
uydurmayı yakalamaktır; burada uydurmayı çoğaltmıştır.

**Sınıf:** YANLIŞ KAYNAK + ÇARPITMA (madde var, hüküm yok).
**Şiddet:** orta-yüksek (yaygınlık ve denetim katmanına sızma nedeniyle).
**Talep:** `Anayasa Madde IV` → `Anayasa Madde VI (terminoloji sürüklenmesi) + ENS-4000 §Kapsam`.

### Y-3 — `README.md:77` — "Kimse kendi işini onaylamaz (G2/G3)" kaynaksız

**Metin:** *"- **Kimse kendi işini onaylamaz (G2/G3).** Validasyon, yazardan bağımsız bir
çağrıda yapılır."*

**Kusur (iki katmanlı):**
1. **Kaynak yok** — G2/G3'ün hangi belgede tanımlı olduğu yazılmamış. Kök `README.md`, projeye
   ilk giren okuyucunun belgesidir; "G2/G3"ün Anayasa sanılmasının **kaynağı tam olarak budur**.
   Aynı paragrafın diğer maddeleri kaynağını yazıyor ("Madde IX", "Madde IV") — G-ilkeleri
   yazmıyor. Asimetri, okuyucuyu Anayasa'ya yönlendirir.
2. **İkinci cümle yanlış ilkeye bağlı** — *"Validasyon, yazardan bağımsız bir çağrıda yapılır"*
   G3 değil **G4**'tür (≥2 bağımsız validator). G3 = *doğrulayan onaylamaz*.

**Sınıf:** YANLIŞ KAYNAK (eksik atıf) + ÇARPITMA (G3→G4).
**Şiddet:** orta (görünürlüğü yüksek belge).
**Talep:** *"Kimse kendi işini kanonlaştıramaz ([GOV-000](governance/000-governance-principles.md)
**G2**); doğrulama onaydan ayrıdır (**G3**); Canonical için ≥2 bağımsız validator (**G4**)."*

---

## 4. Bulgular — ÇARPITMA

### Ç-1 — **KÖK NEDEN:** `governance/roles.md:38` G2'yi yanlış yeniden ifade ediyor

Bu, taramanın en önemli bulgusudur: "yazan doğrulayamaz (G2)" ailesinin **kaynağı**
uydurma değil, **governance katmanının kendi hatalı yeniden-ifadesidir**.

**Metin (`governance/roles.md:38`, GOV-010):**
> *"- **Validator ≠ Author** (G2): **kendi işini doğrulayan olamaz.**"*

**Gerçek metin (GOV-000:33):**
> *"**G2 — No author canonizes their own work.** Bir yapıtı yazan, onu **Canonical yapamaz**."*

**Kusur:** GOV-010, G2'yi *canonization* yasağından *validation* yasağına **genişletmiş**.
Bunlar farklı hükümlerdir: bir yazar kendi işini doğrulayabilir (ve GOV-030 zinciri buna
"öz-doğrulama, zayıf kanıt" der), ama **kanonlaştıramaz**. GOV-010'un cümlesi, GOV-000'in
İngilizce başlığıyla ("No author canonizes") doğrudan çelişir.

**Yayılma (Madde XII yönünde, tam da öngörüldüğü gibi aşağı doğru):**
`roles.md:38` → `.claude/standards/context-management.md:77` ("yazan zihin validate etmez") →
`.claude/rules/work-protocol.md` (ilk sürüm, düzeltildi) → `DEFECT-REGISTER-VERIFICATION.md:8`
("G2/G3: yazar kendi işini doğrulayamaz") → `2000-theory/*` ve `SKR-*` ("Anayasa G2/G3").
`work-protocol.md:41-43`'teki düzeltme notu semptomu tedavi etmiş, **kaynağı değil**.

İlginç karşı-kanıt: `governance/SCAN-03-gate-compliance.md:234` **doğru** okuyor —
*"G2 iki farklı soru sorar: (a) yazan doğruladı mı? — hayır, temiz; (b) yazan **Canonical**
yaptı mı?"*. Yani Külliyat içinde iki çelişen G2 okuması eşzamanlı yaşıyor.

**Sınıf:** ÇARPITMA (kök neden) — G5 anlamında da sorunlu: yanlış ifade sessizce yayıldı.
**Şiddet:** **en yüksek**; tek satırın düzeltilmesi aşağı akıştaki tüm aileyi çözer.
**Talep:** `roles.md:38` → *"**Validator ≠ Author** (G2): yazar kendi işini **Canonical
yapamaz**; öz-doğrulama yapılabilir ama **bağımsız kanıt sayılmaz** (G4)."*

### Ç-2 — "**Madde X … yasaklar**" — Madde X hiçbir şeyi yasaklamaz (5 örnek)

Tetikleyici vakadaki *"Madde X gecikmeyi yasaklar"* hatası, `rules/` katmanında düzeltildi;
ama **aynı yapısal kalıp** — *Madde X'i genel amaçlı bir yasak maddesine dönüştürmek* —
`7000-reference-implementation/` denetim belgelerinde yaşıyor:

| Dosya:satır | Atfedilen hüküm |
|-------------|-----------------|
| `AUDIT-WAVE2-SECURITY.md:84` | *"doğrulanmamış bir iddia **Madde X'in yasakladığı** şeydir"* |
| `AUDIT-WAVE2-SECURITY.md:368` | *"Bu ifade yanlıştır ve **Madde X'in yasakladığı türdendir**"* |
| `DEFECT-REGISTER.md:224` | *"**Madde X, doğrulanmamış iddiayı yasaklar.**"* |
| `DEFECT-REGISTER-VERIFICATION.md:419` | *"**Madde X, doğrulanamayan iddiayı yasaklar.**"* |
| `DEFECT-REGISTER-VERIFICATION.md:50` | *"§7 yanlışlanamaz: … (**Madde X ihlali**)"* |

**Gerçek metin (Madde X, ENS-0000:174-179):** Madde X **yasak koymaz, ödev yükler**:
*"Her Külliyat kavramı, **kendi belgesinde**, yanlış olacağı koşulları taşır … Başarısızlık
koşulları belirtilmemiş bir kavram tamamlanmış değil, **eksiktir**."* Yaptırımı "yasak" değil
"eksik"tir; öznesi **Külliyat kavramı**dır, herhangi bir iddia değil.

**Doğru kaynak var ve kullanılmamış: Madde VI.** Anti-pattern listesi birebir şunu içerir:
*"herhangi bir Külliyat yapıtında pazarlama dili ya da **yanlışlanamaz iddialar**"*. Yasak
kipini taşıyan madde Madde VI'dır (*"Görüldüğü yerde **reddedilir**"*).

**Neden ciddi:** bu belgeler ENS'in kendi denetim organlarıdır ve **başkalarını uydurmakla
suçlarken kendileri uyduruyor**. `AUDIT-WAVE2-SECURITY.md:1113` ironiyi kısmen kaydetmiş
("bu ironi kaydedilmelidir") ama atıf kusurunu görmemiş.

**Sınıf:** ÇARPITMA (madde var, kip ve kapsam kaydırılmış).
**Şiddet:** yüksek — tetikleyici vakanın birebir aynı kalıbı.
**Talep:** *"Madde X'in yasakladığı"* → *"**Madde VI**'nın reddettiği (yanlışlanamaz iddia) /
**Madde X**'in eksik saydığı"*.

### Ç-3 — "**Madde VI (İzlenebilirlik)**" — Madde VI'nın başlığı bu değil (3 örnek)

| Dosya:satır | Metin |
|-------------|-------|
| `7000-reference-implementation/AUDIT-WAVE2-SECURITY.md:10` | *"\| **Yetki** \| Anayasa Madde X (Yanlışlanabilirlik Ödevi), **Madde VI (İzlenebilirlik)** \|"* |
| `7000-reference-implementation/AUDIT-WAVE2-SECURITY.md:125` | *"Anayasa **Madde VI (izlenebilirlik)** de düşer"* |
| `7000-reference-implementation/DEFECT-REGISTER.md:302` | *"Yetki: Anayasa Madde X …, **Madde VI (İzlenebilirlik)**"* |

**Gerçek:** Madde VI'nın başlığı **"Anti-Pattern'ler (ENS Ne Değildir)"**. **İzlenebilirlik
Yasası = Madde VIII** (*"Anayasa'nın kalbi … Bir öksüz düğüm … bir kusurdur"*).

Bu, üç belgenin künye/yetki satırında **var olmayan bir madde başlığı** üretiyor. Anayasa'yı
okumadan bu belgelere güvenen bir okuyucu, Madde VI'yı izlenebilirlik maddesi sanır — ve
gerçek izlenebilirlik maddesi (VIII) atıfsız kalır. Not: aynı belgelerin *içeriği* çoğu yerde
Madde VI'nın black-box yasağını doğru kullanıyor; kusur **etiketleme**dedir, bu yüzden
"uydurma" değil "çarpıtma" sayıldı.

**Sınıf:** ÇARPITMA (yanlış madde başlığı).
**Şiddet:** orta.
**Talep:** `Madde VI (İzlenebilirlik)` → `Madde VIII (İzlenebilirlik Yasası)` ya da
`Madde VI (Anti-Pattern'ler — black-box çıktı)`, hangisi kastediliyorsa.

### Ç-4 — `ADR-0001-agent-runtime.md:414` — Madde V'te "güvenli" niteliği yok

**Metin:** *"Prompt-injection'a karşı skill-içeriği sanitizasyonu birinci savunma hattıdır
(**Anayasa Madde V \"güvenli\"** + failure condition §8)."*

**Gerçek metin (Madde V, ENS-0000:123-126):** nitelik listesi kapalıdır — *Modular ·
Observable · Testable · Replaceable · Versioned · Explainable · Deterministic (mümkün olduğunda)
· Event-driven · DDD uyumlu · CQRS uyumlu · Cloud-native*. **"Güvenli"/"Secure" listede yoktur.**

**Ağırlaştırıcı:** terim **tırnak içinde** verilmiş, yani doğrudan alıntı iddiası taşıyor.
Aynı ADR'nin `:151`, `:429` satırları Madde V'ten *gerçekten* var olan nitelikleri
("Replaceable/Modular") aynı tırnaklı biçimde alıntılıyor — okuyucu için ikisi ayırt edilemez.
ADR-0001 **Accepted**'tır ve Faz-4 kodunu yetkilendirir; Madde IX gereği mimari, Külliyat'ta
olmayan bir kavramı tanıtamaz — burada Anayasa'da olmayan bir tasarım niteliği tanıtılmıştır.

**Sınıf:** ÇARPITMA/UYDURMA sınırında (var olmayan alıntı).
**Şiddet:** orta-yüksek (Accepted ADR).
**Talep:** ya Madde V'e güvenlik niteliği eklenmesi için RFC (Madde XV/XIV), ya da atfın
**P7 (bounded autonomy — ENS önerir, emretmez)** + ADR §8 failure condition'a çevrilmesi.

### Ç-5 — "P7 = **Bounded Autonomy**" — Anayasa'da bu ad yok, Sözlük'te de yok

| Dosya:satır | Metin |
|-------------|-------|
| `2000-theory/ENS-2004-learning-theory.md:110` | *"ENS'in **P7 (Bounded Autonomy, Madde III)** … karşılığının doğrudan eşi"* |
| `2000-theory/ENS-2004-learning-theory.md:202` | *"**P7 kapısı (Bounded Autonomy, Anayasa Madde III)** — mekanizmanın çekirdeği."* |
| `5000-architecture/adr/ADR-0001-agent-runtime.md:3,24,336` | başlık: *"ENS Agent Runtime & **Bounded Autonomy**"*, §5.6 *"Bounded Autonomy (P7)"* |

**Gerçek metin (Madde III, P7):** *"**Sorumluluk insandadır.** ENS önerir; emretmez."*
"Bounded Autonomy" ifadesi Anayasa'da **geçmez** (tam metin tarandı).

**Ek kusur:** `4000-ontology/ENS-4000-glossary.md` içinde *"Bounded Autonomy"* girdisi
**yoktur** (grep: 0 sonuç) — oysa Sözlük kendini `:26` *"adlandırmanın tek doğruluk kaynağıdır"*
ilan eder ve `:22` sessiz takma adlandırmayı yasaklar. Yani terim, ADR-0001 başlığında
(mimari katmanı) doğmuş ve oradan teoriye (`ENS-2004`) geri sızmıştır — bu **Madde IX'un
(kavram tanıtım kuralı) ve Madde XII'nin (yetki tek yönde akar) tam olarak yasakladığı yön**dür.

**Sınıf:** ÇARPITMA (Anayasa'ya ait olmayan bir ad Madde III'e atfediliyor) + Madde IX ihlali.
**Şiddet:** orta.
**Talep:** ya "Bounded Autonomy" ENS-4000'e P7'nin kanonik adı olarak girer (philosopher
önerisi + skeptic turu), ya da atıflar *"P7 — sorumluluk insandadır (Madde III)"* biçimine
çevrilir. Üçüncü yol yok: şu anki hâl sessiz takma addır.

### Ç-6 — `README.md:85` — "çift-owner kabul" Madde XV'in yordamı değil

**Metin:** *"Anayasa değişikliği yalnızca **Madde XV'in tam yordamıyla**: RFC → skeptic →
**çift-owner kabul**."*

**Gerçek metin (Madde XV):** üç koşul — (a) değiştirilen maddeye atıf, (b) skeptic saldırısı,
(c) **Madde XIV uyarınca kabul**. "Çift-owner" ifadesi Madde XV'te de Madde XIV'te de yoktur.
Çift-owner kapısı **RFC-6001 §7.5'in kendi kararıdır** (`ens-ceo` + `ens-style-guardian`) —
tek bir emsal, anayasal yordam değil.

**Sınıf:** ÇARPITMA (emsalin kurala terfi ettirilmesi).
**Şiddet:** düşük-orta — ama tam olarak "uydurma yetki"nin doğuş biçimi: bir kez yapılan şey,
bir sonraki belgede "yordam" diye anılır.
**Talep:** *"…Madde XV yordamıyla: atıf + skeptic + Madde XIV kabul (RFC-6001'de bu, çift-owner
kapısı olarak uygulandı — emsal, kural değil)."*

### Ç-7 — "G2/G3" ile "bağımsız validator" gerekçelendirmesi (yaygın, düşük şiddet)

`ROADMAP.md:39,84,113,139,204,337`, `ENS-4010:65,339,377,382`, `ENS-2001:40,274`,
`ADR-0002:16,32`, `RFC-6001:534,574,612` ve `.claude/*` genelinde tekrarlanan kalıp:
*"bağımsız skeptic turu gerekir (G2/G3)"*.

**Gerçek:** "≥2 **bağımsız** validator" hükmü **G4**'tür. G2 canonization, G3 validation↔approval
ayrımıdır. Doğru atıf **G2 + G4**'tür.

`ROADMAP.md:95,106,147` ve `ENS-4010:17` aynı olguyu **doğru** biçimde G4'e bağlıyor — yani
korpus kendi içinde tutarsız (Madde VI: terminoloji sürüklenmesi).

**Sınıf:** ÇARPITMA (eksik/yanlış ilke numarası).
**Şiddet:** düşük (içerik doğru, numara eksik) — ama **yaygın**: ~25 örnek.
**Talep:** `(G2/G3)` → `(G2 + G4)` normalizasyonu; `ens-style-guardian` işi.

### Ç-8 — "G" ad-uzayı çakışması: governance ilkeleri ↔ denetim bulgu kimlikleri

`ROADMAP.md:36` — *"`AdversarialAuditTests.cs` (**G2/G7/G8/G9**, A1-A5, D1-D4, E1-E6)"* —
burada `G2` bir **test/denetim bulgusu kimliğidir**.
`ROADMAP.md:39` — *"öz-onay yok / **G2/G3**"* — burada `G2` bir **governance ilkesidir**.

**Aynı dosyada, üç satır arayla, aynı token iki farklı göndergeye sahip.** Aynı çakışma:
`7000-reference-implementation/README.md:225,228` (`G7`, `G8` = demo bulgu kimlikleri),
`AUDIT-WAVE2-*` (`G5` = kusur kimliği, `.claude/rules/SKR-046-tier3-discipline-rules.md:614`'te
"G5" bu anlamda geçiyor) ↔ GOV-000 G5 (izlenebilirlik ilkesi).

**Kusur:** Madde VI *"terminoloji sürüklenmesi"* anti-pattern'inin ders kitabı örneği. Pratik
sonucu: **G-atıflarının otomatik denetimi imkânsızdır** — bu taramada her `G<n>` eşleşmesi elle
ayıklanmak zorunda kaldı. `G-11`, `G-16`, `G-17` (ROADMAP gap kimlikleri) tireli oldukları için
ayrışıyor; tiresiz `G2/G7/G8` ayrışmıyor.

**Sınıf:** ÇARPITMA / terminoloji sürüklenmesi (Madde VI).
**Şiddet:** orta (denetlenebilirliği yok ediyor).
**Talep:** denetim bulgu kimlikleri `AUD-G2` gibi ön-ekli hâle getirilsin; `G<n>` tiresiz
biçimi **yalnızca** GOV-000 ilkelerine ayrılsın ve bu kural `ENS-4000`'e yazılsın.

### Ç-9 — `.claude/standards/documentation-style.md:43` — alias yasağı Madde IV'e bağlanmış

Y-2'nin `.claude/` ayağı; ayrıntı Y-2'de. Madde IV'te terim/alias hükmü yok; doğru kaynak
Madde VI + ENS-4000.

### Ç-10 — `.claude/rules/advisor-skills.md:4,73` — "rol ayrımı (Madde XIV)"

**Metin:** `:4` *"ENS'in ajan kadrosu zaten **rol-ayrımlıdır (Madde XIV)**"*;
`:73` *"**Anayasa Madde XIV — rol ayrımı**; GOV-000 G2 … + G4 …"*.

**Gerçek:** Madde XIV'ün başlığı *"Karar Yönetişimi (RFC / ADR Yaşam Döngüsü)"*; içeriği RFC/ADR
yaşam döngüleridir. Rolleri sayan tek cümlesi *"`ens-ceo` … `ens-skeptic` … `ens-style-guardian`
… hiçbiri atlanmaz"*tır — bu bir **rol ayrımı ilkesi değil**, karar sürecine katılım şartıdır.
Agent yetki sırasının maddesi **Madde XII**'dir (`ROSTER.md:3` bunu doğru yazıyor); rol modeli
ise `governance/roles.md` (GOV-010).

Bu kusur `.claude/rules/SKR-046-tier3-discipline-rules.md:719-726`'da **T-Q talebi olarak zaten
kaydedilmiş** ve **kapatılmamış**. Kayıtlı-ama-açık kusur, Madde VII gereği bağımlı işi
durdurmalıydı.

**Sınıf:** ÇARPITMA (yanlış madde).
**Şiddet:** düşük-orta (bilinen, kayıtlı).
**Talep:** `Madde XIV` → `Madde XII + GOV-010`.

---

## 5. DOĞRULANMADI (şüpheli, kesinleşmemiş)

Bunlar kusur olarak **sayılmadı**; ya savunulabilir bir türetme ya da kanıt yetersizliği var.

| # | Yer | Şüphe | Neden kesinleşmedi |
|---|-----|-------|--------------------|
| D-1 | `.claude/skills/karar-konseyi/SKILL.md:68,89` | *"Bu kararın yanlış olduğunu ne gösterirdi? (**Madde X**)"* — Madde X'in öznesi **Külliyat kavramı**dır, tekil operasyonel karar değil. | Konsey çıktısı teorik kararları da kapsıyor (`:74`); genelleme *ruh* düzeyinde savunulabilir. Yine de "zorunlu" kipi Madde X'ten gelmiyor. |
| D-2 | `.claude/agents/ens-test-runner.md:105` | *"'muhtemelen geçiyordur' yasak (**Madde X**)"* | Ç-2 ailesinden ama tek örnek ve kip yumuşak; Madde VI'ya bağlanması daha doğru olurdu. |
| D-3 | `4000-ontology/ENS-4025-semantic-logic.md:92` | *"**Registry tek kaynaktır (Madde XII)**"* — Madde XII'de "registry önceliği" diye bir hüküm yok. | Madde XII'nin *"alttaki tanımlar, üstteki tüketir"* cümlesinden türetilebilir (ENS-4010 upstream, ENS-4025 downstream). Savunulabilir türetme. |
| D-4 | `4000-ontology/ENS-4000-glossary.md:32` | *"Tersi yön (Glossary → Theory/Laws) **Madde XII'yi ihlal** eden bir döngü yaratırdı"* — Madde XII, Külliyat **içi** sıra tanımlamaz (1000/2000/3000/4000 tek katman). | İddia *döngü* hakkında; Madde XII "yetki tek yönde akar" der, döngü bunu ihlal eder. Geçerli. |
| D-5 | `.claude/rules/plan-first.md:64` | *"Anayasa / **GOV-\*** dosyasına dokunma → tanım gereği yüksek Stake (**Madde XV**)"* — Madde XV yalnızca **Anayasa** değişikliğini düzenler, GOV-* dosyalarını değil. | GOV-000 G7 (*"çatışmada Anayasa kazanır (Madde XV)"*) üzerinden dolaylı bağ var. Genişletme küçük ve sonucu (yüksek stake) doğru. |
| D-6 | `5000-architecture/reviews/SKR-026:118` | *"**P8 / Madde VIII:** 'Teori asla implementation'dan türetilmez.'"* — alıntı Madde III/P8'e ve Anayasa'nın kapanış cümlesine ait, Madde VIII'e değil. | "P8 /" ön-eki doğru kaynağı zaten veriyor; Madde VIII ok-yönü aynı şeyi kodluyor. Etiketleme gevşekliği, uydurma değil. |
| D-7 | `.claude/agents/ens-backend-architect.md:16` | Madde VII tırnak içinde *"erken fazda **eksik**, bağımlı sonraki işi durdurur"* — gerçek metin *"erken bir fazda bulunan bir **kusur**"*. | Anlam korunmuş; sözcük değişimi ("kusur"→"eksik") tırnak içinde olduğu için yine de düzeltilmeli. |
| D-8 | `.claude/standards/context-management.md:17` | Yetki olarak **Madde X** — Madde X bağlam yönetimi hakkında hüküm içermez. | Belge kendini *"operasyonel uzantı"* diye **açıkça işaretliyor**; dürüst hedge. Kusur değil, örnek davranış. |

---

## 6. Sayım ve oran

### 6.1 Kapsam ve yöntem

Taranan kapsam: `1000-philosophy/`, `2000-theory/`, `3000-laws/`, `4000-ontology/`,
`5000-architecture/`, `6000-rfc/`, `7000-reference-implementation/`, `governance/`, `.claude/`,
kök `README.md`/`ROADMAP.md`/`REGISTRY.md`/`KULLIYAT.md`, `tools/`.
Kapsam dışı: `0000-constitution/` (kaynak metnin kendisi), `journal/`, `plans/`, bu dosya.

Yöntem: her eşleşen satır **okundu**; şüpheli olanların çevresindeki bağlam ayrıca açıldı ve
Anayasa/GOV-000 tam metniyle karşılaştırıldı. Bir satırda birden çok atıf varsa tek örnek sayıldı.

### 6.2 "Madde <roman rakam>" atıfları

| Dizin | Atıf sayısı |
|-------|-------------|
| `.claude/` | 112 *(52'si `rules/SKR-046-tier3-discipline-rules.md` — atıfları **inceleyen** meta-belge)* |
| `6000-rfc/` | 125 |
| `5000-architecture/` | 92 |
| `7000-reference-implementation/` | 59 |
| `4000-ontology/` | 25 |
| kök (README/ROADMAP/REGISTRY/KULLIYAT) | 22 |
| `2000-theory/` | 21 |
| `1000-philosophy/` | 10 |
| `3000-laws/` | 5 |
| `governance/` (SCAN-01 hariç) | 2 |
| `tools/` | 1 |
| **Toplam** | **474** |

### 6.3 "G<rakam>" atıfları

Ham eşleşme 242. Bunlardan düşülenler: tanım kaynaklarının kendisi (GOV-000/GOV-030/roles/
capability-matrix ≈ 20), meta-inceleme belgesi (`.claude/rules/SKR-046-tier3-discipline-rules.md`
— tarama sırasındaki adı `REVIEW-tier3-discipline.md`; 41), bu dosya,
kapsam dışı (`journal/`, `plans/` ≈ 10), `SCAN-03` (5).
Kalan ≈ **152 aday**; bunların **≈27'si yanlış-pozitif** (denetim/test bulgu kimlikleri —
bkz. Ç-8), dolayısıyla **≈125 gerçek G-atfı** denetlendi.

> **Uyarı:** bu sayı **kesin değildir** ve kesin olamaz — Ç-8'deki ad-uzayı çakışması yüzünden
> `G2` token'ı otomatik olarak sınıflanamıyor. Sayının belirsizliği, bulgunun kendisidir.

### 6.4 Kusur dağılımı

| Sınıf | Bulgu | Kusurlu atıf örneği |
|-------|-------|---------------------|
| **UYDURMA** | U-1, U-2 | 3 |
| **YANLIŞ KAYNAK** | Y-1, Y-2, Y-3 | 17 |
| **ÇARPITMA (ağır)** | Ç-1, Ç-2, Ç-3, Ç-4, Ç-5, Ç-6, Ç-10 | 19 |
| **ÇARPITMA (hafif/yaygın)** | Ç-7, Ç-8 | ≈31 |
| **DOĞRULANMADI** | D-1…D-8 | 8 (kusur sayılmadı) |
| | **Toplam kusurlu** | **≈70** |

**Denetlenen toplam atıf:** 474 (Madde) + ≈125 (G) = **≈599**
**Temiz:** ≈529 — **%88,3**
**Kusurlu:** ≈70 — **%11,7**
**Ağır kusurlu (UYDURMA + YANLIŞ KAYNAK + ağır ÇARPITMA):** 39 — **%6,5**

### 6.5 Katman bazında sağlık

| Katman | Değerlendirme |
|--------|---------------|
| `1000-philosophy/` | **Temiz** — 10/10. Madde VI atıflarının hepsi listede gerçekten var olan anti-pattern'lere (black-box, yanlışlanamaz iddia, ERP adlandırması) gidiyor. |
| `3000-laws/` | **Temiz** — 5/5. Yalnızca Madde III/VII/X, hepsi doğru. |
| `6000-rfc/` | **Temiz** — 125 atıfın hepsi Madde IV/X/XIV/XV etrafında ve **alıntı düzeyinde doğru**; RFC-6001 eski/yeni metni yan yana veriyor. Külliyat'ın en titiz katmanı. Tek gevşeklik: `:534` "Madde XV-b; G2/G3" yan yanalığı. |
| `5000-architecture/reviews/` (SKR/CEO) | **Temiz** — Madde VIII/IX/XII/XIV kullanımı örnek düzeyde kesin. |
| `4000-ontology/` | **Büyük ölçüde temiz** — 2 sınırda örnek (D-3, D-4). |
| `5000-architecture/adr/` | **Yara** — Ç-4 (Madde V "güvenli"), Ç-5 (Bounded Autonomy). İkisi de **Accepted** ADR'de. |
| kök + `.claude/` | **Yara** — U-1, U-2, Y-3, Ç-9, Ç-10; `rules/` katmanı düzeltildi ama `standards/` katmanı düzeltilmedi. |
| `2000-theory/` | **Yara** — Y-1 (7 örnek), Y-2 (3 örnek), Ç-5. Külliyat'ın kalbinde, bilinen ve kayıtlı bir kusur açık. |
| `7000-reference-implementation/` | **Yara** — Ç-2 (5), Ç-3 (3). Denetim belgeleri, uydurmayı denetlerken kendileri çarpıtıyor. |
| `governance/` | **Kök neden burada** — Ç-1 (`roles.md:38`). Tek satır, en yüksek kaldıraç. |

---

## 7. Verdict

# **KISMEN GÜVENİLİR**

**Gerekçe:** atıfların **%88'i temiz** ve Külliyat'ın en yüksek riskli katmanları
(`6000-rfc/`, felsefe, yasalar, SKR/CEO incelemeleri) **kusursuz** çıktı — bu, gerçek bir
disiplinin varlığını kanıtlar. Ancak tetikleyici vakadaki **iki kalıbın ikisi de hâlâ canlı**:
(i) *"Anayasa G2/G3"* yanlış kaynağı `2000-theory/` **Külliyat'ında** 10 yerde, (ii) *"Madde X
yasaklar"* çarpıtması `7000-reference-implementation/` denetim belgelerinde 5 yerde. `rules/`
katmanında yapılan düzeltme, **kusuru gidermedi; yalnızca en görünür örneğini gidermiş oldu.**

`refuted` değil, çünkü kusurların hiçbiri bir teorik iddiayı geçersiz kılmıyor; hepsi
*atıf hijyeni* düzeyinde ve hepsi mekanik olarak düzeltilebilir.
`güvenilir` de değil, çünkü **bilinen ve yazıya dökülmüş bir kusur açık bırakılmış** —
`SKR-046` (o tarihte `REVIEW-tier3-discipline.md`) 2026-07-26'da `ENS-2003`'teki "Anayasa G2/G3"ü ve
`advisor-skills.md`'deki "Madde XIV — rol ayrımı"nı **isim vererek** kaydetmiş, ikisi de
düzeltilmemiş. Madde VII'ye göre kayıtlı bir kusur, düzeltilene dek bağımlı işi durdurur;
durmadı.

### En güçlü karşı-argüman (steelman — owner'ın cevaplaması gereken)

> *"Bunların çoğu üslup meselesi. 'G2/G3' yazmak ile 'GOV-000 G2 + G4' yazmak arasındaki fark,
> hiçbir kararı değiştirmedi: her durumda bağımsız bir skeptic turu koşuldu. Yani atıf yanlış
> olsa da **davranış doğruydu**. Külliyat'ı bir atıf-denetim bürokrasisine çevirmek, ENS'in
> asıl işine (teori üretmek) harcanacak dikkati (P5!) yer."*

**Cevap:** ENS'in tüm mimarisi *"yetkisini gösteremeyen yapıt depoda yer almaz"* (ENS-0000:23)
üzerine kurulu. Yetki atfı ENS'te süs değil, **taşıyıcı yapıdır**. Ve Ç-1 tam olarak
"davranışın doğru kaldığı" varsayımını çürütüyor: `roles.md:38`'in yanlış G2 ifadesi,
`context-management.md:83`'te *"bağımsız validation her zaman subagent"* diye **operasyonel bir
zorunluluğa** dönüşmüş — yani yanlış atıf, iş akışını fiilen değiştirmiş. Yine de karşı-argüman
Ç-7/Ç-8 (≈31 hafif örnek) için **haklıdır**: bunlar toplu bir normalizasyon turuyla kapatılmalı,
tek tek SKR konusu yapılmamalı.

### Sahibine talepler (öncelik sırasıyla)

| # | Talep | Yer | Kaldıraç |
|---|-------|-----|----------|
| **T-1** | G2'yi doğru ifade et: *"Canonical yapamaz"*, *"doğrulayan olamaz"* değil. | `governance/roles.md:38` | **Kök neden** — tek satır, tüm aileyi çözer |
| **T-2** | `Anayasa G2/G3` → `GOV-000 G2/G3` (10 yer) | `ENS-2003`, `ENS-2004`, `SKR-043/044/037` | Külliyat'ta yanlış kaynak |
| **T-3** | *"Madde X … yasaklar"* → *"Madde VI reddeder / Madde X eksik sayar"* (5 yer) | `7000/AUDIT-WAVE2-SECURITY.md`, `DEFECT-REGISTER*.md` | Tetikleyici kalıbın ikizi |
| **T-4** | `Madde VI (İzlenebilirlik)` → `Madde VIII` (3 yer) | `7000/AUDIT-WAVE2-SECURITY.md:10,125`, `DEFECT-REGISTER.md:302` | Var olmayan madde başlığı |
| **T-5** | `.claude/standards/context-management.md:77,83` düzelt (U-1) | `.claude/standards/` | `rules/`'un üstündeki katman |
| **T-6** | *"alias yasağı, Madde IV"* → `Madde VI + ENS-4000` (5 yer) | `ENS-2003`, `ENS-2004`, `SKR-045`, `documentation-style.md` | Denetim katmanına sızmış |
| **T-7** | `ADR-0001:414` Madde V "güvenli" atfını kaldır ya da RFC ile Madde V'e ekle | `5000-architecture/adr/` | Accepted ADR |
| **T-8** | "Bounded Autonomy"yi ya ENS-4000'e al ya da P7'nin kanonik ifadesine dön | `ENS-4000`, `ENS-2004`, `ADR-0001` | Madde IX ihlali (mimari→teori sızma) |
| **T-9** | `ens-silent-failure-hunter.md:19`: `Madde VI` → `GOV-000 G5 + Madde V` | `.claude/agents/` | Doğru kaynak elinin altında |
| **T-10** | Toplu normalizasyon: `(G2/G3)` → `(G2 + G4)` bağımsız-validator bağlamlarında | korpus geneli (~25) | Hafif, toplu |
| **T-11** | Denetim bulgu kimliklerini `AUD-` ön-ekle; `G<n>` tiresizi GOV-000'e ayır; kuralı ENS-4000'e yaz | `ROADMAP`, `7000/*`, `ENS-4000` | G-atıflarını **denetlenebilir** kılar |
| **T-12** | `README.md:77`'ye kaynak linki; `:85`'te "çift-owner"ı emsal olarak işaretle | kök `README.md` | Yanlış-kaynak salgınının giriş kapısı |

### Bu taramanın kendi yanlışlanabilirliği (Madde X)

1. **Sayım kesin değil.** G-atfı sayısı Ç-8 yüzünden ±%20 belirsiz. Madde-atfı sayısı (474)
   `rg` çıktısına dayanır ve satır başına tek atıf sayar; çok-atıflı satırlar eksik sayılmıştır.
2. **Bağlam örneklemesi.** 599 atfın **hepsinin satırı** okundu, ama **hepsinin geniş bağlamı**
   okunmadı — yalnızca şüpheli görünenlerin. Şüphe filtresini geçen bir çarpıtma kaçmış olabilir.
   Bu taramayı yanlışlayacak gözlem: rastgele seçilmiş 30 "temiz" atıftan birinin, tam bağlamı
   okunduğunda kusurlu çıkması.
3. **Anayasa v0.3.0'a göre denetlendi.** Anayasa değişirse (Madde XV), bu raporun §1 referans
   tablosu ve dolayısıyla tüm verdict'i geçersizleşir.
4. **`.cs` dosyaları taranmadı** — yalnızca `*.md`. Kod yorumlarındaki `// TRACE:` atıfları
   (`ProofTrace.cs`, `CapabilityRegistry.cs` vb.) bu taramanın **dışındadır** ve ayrı bir tur ister.
5. **Çakışma kısıtı.** `ENS-2003`/`ENS-2004` eşzamanlı düzenlendiğinden yalnızca okundu;
   verilen satır numaraları kaymış olabilir, metin kalıpları kaymaz.
   *(2026-07-27: kısıt kalktı, düzeltmeler uygulandı — §8.)*

---

## 8. Düzeltme turu (2026-07-27)

Kök neden (Ç-1) `governance/roles.md`'de owner tarafından düzeltildikten sonra türevler
uygulandı. **İlke: yalnız dayanak düzeltildi, hiçbir cümlenin iddiası değiştirilmedi.**

### 8.1 Uygulananlar

| Bulgu | Yapılan | Dosyalar |
|-------|---------|----------|
| **Y-1** | `Anayasa G2/G3` → içeriğe göre `GOV-000 G4` (bağımsız doğrulama) ya da `GOV-000 G2+G4` (statü/canon ilerlemesi de söz konusuysa); hepsine GOV-000 linki | `ENS-2003` ×3, `ENS-2004` ×4, `SKR-043`, `SKR-044`, `SKR-037` ×2 |
| **Ç-2** | *"Madde X … yasaklar"* → *"Madde VI reddeder; Madde X eksik sayar"*; iki yerde *"Madde X ihlali"* → *"Madde X ödevi karşılanmamış"* | `AUDIT-WAVE2-SECURITY.md` ×2, `DEFECT-REGISTER.md`, `DEFECT-REGISTER-VERIFICATION.md` ×3 |
| **Ç-3** | `Madde VI (İzlenebilirlik)` → `Madde VIII (İzlenebilirlik Yasası)`; Madde VI doğru başlığıyla ("Anti-Pattern'ler — black-box çıktı") ayrıca korundu; bir yere GOV-000 G5 eklendi | `AUDIT-WAVE2-SECURITY.md` ×2, `DEFECT-REGISTER.md` |
| **U-1** | *"yazan validate etmez (G2/G3)"* → G2 (kanonlaştıramaz) + G4 (≥2 bağımsız validator); kaynak belge linklendi; düzeltme notu eklendi | `.claude/standards/context-management.md` ×2 |
| **U-2** | `Madde VI` → `GOV-000 G5` + `Madde VIII`; black-box durumu için Madde VI korundu | `.claude/agents/ens-silent-failure-hunter.md` |
| **Y-2 / Ç-9** | *"alias yasağı, Madde IV"* → `Madde VI ("terminoloji sürüklenmesi") + ENS-4000 §Kapsam` | `ENS-2003` ×2, `ENS-2004`, `SKR-045` ×2, `documentation-style.md` |
| **Ç-4** | **Metin değiştirilmedi** (Accepted ADR, EC-001) — dipnot düzeltme kutusu eklendi: Madde V'in kapalı listesinde "güvenli" yok; iddia geçerli, dayanak P7 + §8 failure condition; Madde V'e ekleme yolu RFC | `ADR-0001-agent-runtime.md` |
| **Ç-5** | §5.6'ya adlandırma notu: "Bounded Autonomy" bu ADR'de tanıtılan **mimari ad**; Madde III'te P7 bu adla geçmez, ENS-4000'de kayıtlı değil; kanonikleşme yolu Madde IX. Teoride iki atıf P7'nin anayasal metnine çevrildi | `ADR-0001` §5.6, `ENS-2004` ×2 |
| **Ç-8** | `G<n>` ad-uzayı kuralı ROADMAP'e yazıldı: öneksiz `G1..G7` = yalnız GOV-000; `AUD-G<n>` = denetim bulgusu; `G-<n>` = gap. Düzyazıdaki çıplak kullanımlar öneklendi. **Test metot adları değiştirilmedi** (kodda zaten `AUDIT_*_G<n>` öneki var) | `ROADMAP.md` ×2, `7000/README.md` ×2 |

### 8.2 Düzeltilmeyenler ve nedenleri

| # | Ne | Neden |
|---|----|-------|
| 1 | **Ç-7** — bağımsız-validator gerekçesi için çıplak `(G2/G3)` (~25 örnek: `ROADMAP`, `ENS-4010`, `ENS-2001`, `ADR-0002`, `RFC-6001`, `SKR-040/041/042/045`, `.claude/*`, `7000/README.md:250`) | **Bu turda görevlendirilmedi.** Doğrusu `G2+G4`. İçerik doğru, yalnızca ilke numarası eksik/kaydırılmış; toplu normalizasyon turu ister. Ç-8 kuralı yürürlüğe girdiği için bu atıfların *hangi* ad-uzayına ait olduğu artık en azından belirsiz değil. |
| 2 | `REGISTRY.md`, `.claude/standards/metadata-header.md`, `governance/SCAN-02-*`, `governance/SCAN-03-*`, `governance/roles.md` | Başka ajanlarda — dokunulmadı (talimat). `REGISTRY.md:55-57`'de "SKR-024/025 (inline, G2/G3 riski)" ifadeleri Ç-7 sınıfındadır ve o dosyanın sahibine kalmıştır. |
| 3 | ADR-0001 başlığı ve `type: adr` künyesindeki *"Bounded Autonomy"* | Accepted ADR'nin **başlığı**; değiştirmek REGISTRY/ROADMAP/3 SKR başlığında zincirleme ad değişikliği doğururdu. Adın statüsü §5.6 notuyla açıkça işaretlendi; kanonikleşmesi ayrı bir Madde IX edimidir (T-8 açık kalır). |
| 4 | `.claude/rules/advisor-skills.md:4,73` — "rol ayrımı (Madde XIV)" (**Ç-10**) | Bu turda görevlendirilmedi; `.claude/rules/SKR-046-tier3-discipline-rules.md` T-Q talebi olarak zaten açık. Doğrusu `Madde XII + GOV-010`. |
| 5 | `README.md:77,85` (**Y-3**, **Ç-6**) | Bu turda görevlendirilmedi. |
| 6 | `.cs` dosyalarındaki `// TRACE:` atıfları | Tarama `*.md` ile sınırlıydı; ayrı tur ister. |

### 8.3 Semantik risk taşıyan tek nokta

Ç-4'te dayanak düzeltilince cümle **savunulamaz hâle gelmedi** — sanitizasyonun birinci savunma
hattı olduğu iddiası P7 ve ADR §8 ile ayakta kalıyor. Ancak **Madde V'te güvenlik niteliğinin
bulunmaması gerçek bir Külliyat boşluğudur**: ENS, güvenlik gereksinimini anayasal bir tasarım
niteliğine bağlayamıyor. Bu, atıf hijyeni değil **içerik** meselesidir ve bir RFC konusudur —
`ens-philosopher`/`ens-ceo`'ya bırakıldı, bu tarama tarafından karara bağlanmadı.
