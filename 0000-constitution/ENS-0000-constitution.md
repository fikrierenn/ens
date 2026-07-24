---
id: ENS-0000
title: ENS Anayasası
type: constitution
canon: true
constitutive: true
immutable_core_sections: [Madde III]
origin: ENS projesinin kuruluş edimi
depends_on: []
referenced_by: [ENS-1000, ENS-3000, ENS-4000, "ALL"]
principles: [P1, P2, P3, P4, P5, P6, P7, P8]
status: ratified
owner: ens-philosopher
version: 0.3.0
last_reviewed: 2026-07-24
amended_by: [RFC-6001]
---

# ENS Anayasası

> Enterprise Nervous System'in en üst belgesi. Bu depodaki her yapıt — Külliyat,
> mimari, kod, test, anlatı — referans zinciriyle buradaki bir maddeye geri
> bağlanır. Bu Anayasa'daki yetkisini gösteremeyen bir yapıt depoda yer almaz.

**Anayasa tek doğruluk kaynağıdır. Agent'lar değiştirilebilir; Anayasa değildir.
Hiçbir agent, araç veya model ENS felsefesini tanımlayamaz — onu yalnızca tüketir.**

> **v0.3 notu (Madde XV değişikliği):** [RFC-6001](../6000-rfc/RFC-6001-constitutive-artifact-ayrimi.md)
> kabul edildi (3 bağımsız skeptic turu — SKR-034/035/036 → survives; çift-owner kabul kapısı —
> `ens-ceo` hiza [CEO-0002] + `ens-style-guardian` şema-imzası [STYLE-SIGNOFF-RFC-6001]). Madde IV,
> Külliyat üyeliğini (`canon`) aralığa değil yapıt-türüne-uygun doğrulama yoluna bağlayacak biçimde
> yeniden yazıldı — `constitutive`/`canon` iki bağımsız eksen oldu; Anayasa'nın kendisi bu vesileyle
> tekdüze `constitutive` olmadığını (Madde III = immutable-core, geri kalanı = protective-belt)
> açıkça işaretledi. Diğer Külliyat yapıtlarının `constitutive` alanı ve ENS-4000 canon-incelemesi
> ayrı bir retrofit adımıdır (ROADMAP.md), bu değişiklikle hiçbir skeptic-kazanılmış grade sökülmedi.

---

## Madde I — Amaç

ENS, yeni bir kurumsal bilişim disiplini tanımlar: bir şirketi, süreç ya da transaction
işleyen bir sistem olarak değil, bir **decision-producing cognitive system** (karar
üreten bilişsel sistem) olarak ele almak. ERP **veri parçalanmasını** çözdü; ENS **karar
karmaşıklığını** ele alır — kurumsal kararların üretimi, gerekçelendirilmesi, hatırlanması
ve iyileştirilmesi, ölçekte. Başarı ölçütü: on yıl sonra "Enterprise Nervous Systems"in,
Operating Systems, Distributed Systems, Database Systems ve Domain-Driven Design'ın
yanında öğretilebilir olması — kod içinde bir varlık kanıtı taşıyan tutarlı bir teori.

## Madde II — Kapsam

ENS bir **standart** olarak yönetilir, yalnızca bir ürün olarak değil. Şunları kapsar: bir
Külliyat (teori, yasalar, ontoloji), bu Külliyat'tan **türetilmiş** bir mimari, teoriyi
**kanıtlayan** bir reference implementation, mimariden doğal olarak beliren ürün modülleri
ve disiplini başkaları için tesis eden bir kitap. ENS açıkça **şunlar değildir**: daha iyi
bir ERP, bir dashboard, bir analytics paketi, bir workflow engine ya da bir chatbot.
Bunlar birer yüzey olabilir; asla işin kendisi değil.

## Madde III — Değiştirilemez İlkeler (Immutable Principles)

Sözcükleri iyileştirilebilir, ama niyetleri kaldırılamaz. Kaldırmak, farklı bir proje ilan
etmektir.

- **P1 — Decision atomdur.** En küçük anlamlı örgütsel birim.
- **P2 — Context, data'dan üstündür.** Context olmadan data anlamsızdır.
- **P3 — Memory zekâ yaratır.** *Neden*'in belleği yoksa → hatalar tekrarlanır.
- **P4 — Learning, ölçülmüş sonuçtur; eğitim değildir.** Sonucu niyetle karşılaştır, belleği güncelle.
- **P5 — Attention kıt kaynaktır.** Önce insan dikkatini optimize et.
- **P6 — Explainability pazarlık konusu değildir.** Why, Why-not, Confidence, Evidence, önceki kararlar, Risks, Alternatives.
- **P7 — Sorumluluk insandadır.** ENS önerir; emretmez.
- **P8 — Teori implementasyondan önce gelir.** Implementasyon teoriyi kanıtlar; teori asla koddan geri türetilmez.

## Madde IV — Külliyat

**ENS Külliyat**, değiştirilemez, **teknolojiden bağımsız** kavramların gövdesidir. Hiçbir
Külliyat yapıtı herhangi bir teknolojiye, sağlayıcıya veya modele bağımlı olamaz — asla.
Külliyat, numaralı deponun alt aralıklarını işgal eder — aralık **alanı** belirler, canon'u
değil:

| Aralık | Külliyat içeriği | Dizin |
|--------|---------------|-------|
| `ENS-0xxx` | Anayasa | `0000-constitution/` |
| `ENS-1xxx` | Felsefe — Manifesto, First Principles, Ana Tez | `1000-philosophy/` |
| `ENS-2xxx` | Teori — kavramlar (onaylanınca Külliyat) | `2000-theory/` |
| `ENS-3xxx` | Enterprise Laws | `3000-laws/` |
| `ENS-4xxx` | Ontoloji — Decision Ontology, Enterprise Ontology, Sözlük, Anti-pattern'ler | `4000-ontology/` |

Aralık, bir yapıtın **alanını** (hangi tür içeriğe ev sahipliği yaptığını) belirler; Külliyat
üyeliğini (`canon`) tek başına belirlemez. Her Külliyat yapıtının iki **bağımsız** niteliği vardır:

- **`constitutive`** — yapıt, ENS akıl yürütmesini *olanaklı kılan* bir kural, tanım ya da
  tip-şeması mıdır (normatif; "X, C bağlamında Y sayılır" biçiminde kurucu), yoksa dünya hakkında
  **yanlışlanabilir ampirik** bir iddia mı taşır? `constitutive: true` yapıt tanımla yürürlüktedir;
  `constitutive: false` yapıt kanıtla.
- **`canon`** — yapıt doğrulanıp Külliyat'a girmiş midir? Kazanılır, ilan edilmez.

**Külliyat üyeliği hiçbir aralıkta aralığın kendisinden doğmaz:**

- **`0` (Anayasa)** biricik **kendi kendini yetkilendiren** yapıttır: `constitutive: true`,
  `canon: true`. Bu, ad hoc bir istisna değil, aksiyomatik bir sistemin **ilkeli zorunluluğudur:**
  sonsuz gerilemeyi (Münchhausen trilemması) durdurmak için sistem tek bir öz-yetkilendiren köke —
  Kelsen'in *Grundnorm*'una / Hart'ın *rule of recognition*'ına — dayanmak zorundadır. Onu üstten
  doğrulayan bir yapıt yoktur; yanlışlanabilirlik ödevini (Madde X) kendi değişiklik yordamı
  (Madde XV: RFC + skeptic) üzerinden yerine getirir. **Anayasa'nın kendisi tekdüze `constitutive`
  değildir:** Madde III (P1-P8) ampirik yanlışlamaya kapalı **immutable-core** (Lakatosçu
  hard core); Madde I/II/IV-XIV ise normal RFC ile revize edilebilen **protective belt**'tir. Bu
  Madde'nin (RFC-6001 ile) değiştirilmiş olması, ikincinin revize-edilebilirliğinin kanıtıdır.
- **`constitutive: true` yapıtlar** (Anayasa; felsefenin kurucu tezleri; yasa-çerçevesi;
  ontolojinin tip/şema belgeleri; governance kuralları — 1/3/4 aralıklarında bulunabilir)
  `canon: true` olmak için **ratifikasyon** yolunu izler: failure conditions'ını **tutarlılık/
  örneklenebilirlik** kipinde belirtir (tanım tutarsız mı, örneklenemez mi, başka bir Külliyat
  kuralıyla çelişir mi, daha iyi bir ayrım var mı) ve `ens-skeptic` bu tutarlılık incelemesinden
  sağ çıkar. Ampirik kanıt zincirine (M5 / Faz-4) **tabi değildir**, çünkü ampirik iddia taşımaz.
- **`constitutive: false` yapıtlar** (ampirik teori ve yasalar; `2000` aralığı doğası gereği
  böyledir, `1/3/4` de olabilir) `canon: true` olmak için failure conditions'ını belirtip skeptic
  incelemesinden sağ çıkarak `ratified` (M3) olur; **tam Canon (M5)** yalnızca reference platform
  kanıt zinciriyle (Faz 4) kazanılır (bkz. [`maturity-model.md`](../.claude/standards/maturity-model.md)).

Her iki durumda da failure conditions **zorunludur** (Madde X); yalnızca *yanılma kipi* türe göre
değişir. Külliyat üyeliği [`KULLIYAT.md`](../KULLIYAT.md) dosyasında kayıtlıdır.

## Madde V — Tasarım İlkeleri

Her ENS bileşeni: Modular · Observable · Testable · Replaceable · Versioned ·
Explainable · mümkün olduğunda Deterministic · Event-driven · DDD uyumlu · CQRS uyumlu ·
Cloud-native. Çatışmada **Explainable** ve **Testable** kazanır — incelenemeyen bir
disiplin öğretilemez.

## Madde VI — Anti-Pattern'ler (ENS Ne Değildir)

Görüldüğü yerde reddedilir: ENS'i "AI ERP"/"ERP 2.0"/"dashboard"/"copilot" diye
adlandırmak; black-box çıktı (açıklama nesnesi olmayan öneri); Külliyat atfı olmadan
uydurulmuş mimari (Madde VIII/IX); mimari atfı olmadan uydurulmuş kod; terminoloji
sürüklenmesi (terminology drift); herhangi bir Külliyat yapıtında pazarlama dili ya da
yanlışlanamaz iddialar; reference platform'u teoriyi kanıtlamadan önce üretim için
optimize etmek. Tam kayıt `4000-ontology/` altında.

## Madde VII — Faz Modeli

Kapılı fazlar; bir kapı ancak çıkış ölçütleri kaydedilince açılır. Erken bir fazda
bulunan bir kusur, yukarı akışta düzeltilene dek bağımlı sonraki işi **durdurur**.

```
Faz 0  Felsefe           → 0000-constitution/, 1000-philosophy/
Faz 1  Teori             → 2000-theory/            (en önemli faz)
Faz 2  Bilim             → 3000-laws/, 4000-ontology/  (yasalar, modeller, ontoloji)
Faz 3  Mimari            → 5000-architecture/, 6000-rfc/
Faz 4  Reference Platform→ 7000-reference-implementation/  (kanıt, optimizasyon değil)
Faz 5  Ürün              → 8000-product/           (mimariden beliren modüller)
Faz 6  Kitap             → 9000-book/              (disiplini tanımlar)
```

Ayrıntı: [`.claude/standards/ens-phase-model.md`](../.claude/standards/ens-phase-model.md).

## Madde VIII — İzlenebilirlik Yasası (Traceability Law)

Anayasa'nın kalbi. Her yapıt **yukarı akışa**, ilkelere doğru işaret eder:

```
kod / test  →  ADR  →  theory / law / ontology (Külliyat)  →  first principle (Madde III)
```

Bir **öksüz düğüm** (yukarı akış kenarı olmayan düğüm), kalitesi ne olursa olsun bir
kusurdur. Evrensel künye (Madde XI) ve `/validate-theory` ile denetlenir.
Ayrıntı: [`.claude/standards/traceability.md`](../.claude/standards/traceability.md).

## Madde IX — Kavram Tanıtım Kuralı (Concept Introduction Rule)

**Hiçbir mimari, kod, RFC ya da ADR, Külliyat'ta halihazırda var olmayan bir kavram tanıtamaz.**
Yeni bir kavram gerekiyorsa, önce **Külliyat'a girmelidir** — `ens-philosopher` önerir,
`2000-theory`/`4000-ontology` içinde tanımlanır, `ens-skeptic` saldırır — ancak ondan
sonra herhangi bir alt akış yapıtı ona atıfta bulunabilir. Teori mimariden önce gelir;
mimari implementasyondan önce gelir. Her zaman.

## Madde X — Yanlışlanabilirlik Ödevi (Falsifiability Duty)

Saldırılamayan bir teoriye güvenilemez. Her Külliyat kavramı, kendi belgesinde, **yanlış**
olacağı koşulları (varsayımlar + başarısızlık modları) taşır. `ens-skeptic` sürekli bir
karşıt inceleme yürütür. Başarısızlık koşulları belirtilmemiş bir kavram tamamlanmış
değil, eksiktir.

## Madde XI — Evrensel İzlenebilirlik Başlığı (Universal Traceability Header)

Depodaki her yapıt — her `.md` ve her kaynak modül, README'si aracılığıyla — bağımlılık
grafiğini açığa çıkaran bir künye ile **başlar**:

```yaml
---
id:            ENS-3021              # numaralandırma kaydındaki kimlik
title:         Decision Entropy
type:          theory|law|ontology|adr|rfc|module|book|constitution|philosophy
canon:         true|false
origin:        ENS-0000 §III         # bu yapıtı neyin yetkilendirdiği
depends_on:    [ENS-2010, ENS-2001]  # ihtiyaç duyduğu yukarı akış kimlikleri
referenced_by: [RFC-6042, MOD-...]   # alt akış kimlikleri (validator tutabilir)
principles:    [P1, P4]
status:        draft|review|skeptic-challenged|ratified|superseded
owner:         ens-philosopher       # sorumlu rol
version:       0.1.0
last_reviewed: 2026-07-23
---
```

Şema ve doğrulama: [`.claude/standards/metadata-header.md`](../.claude/standards/metadata-header.md).

## Madde XII — Bağımlılık Grafiği (Yetki Sırası)

Yetki tek yönde akar. Alttaki tanımlar; üstteki tüketir. Hiçbir şey Külliyat'ı atlamaz.

```
ENS Anayasası    (0000)
        ↓
Enterprise Laws + Ontoloji  (Külliyat: 1000/2000/3000/4000)
        ↓
Standards        (.claude/standards)
        ↓
Commands         (.claude/commands)
        ↓
Agents           (.claude/agents)   ← felsefeyi tüketir; asla üretmez
        ↓
Implementation   (7000/8000)
```

Paralel türetme: **Külliyat → Mimari (5000/6000) → Implementation (7000/8000)**;
**Külliyat → Kitap (9000)**; **Mimari → Testler**. Felsefe üreten bir agent ya da kavram
uyduran bir mimari, bu Madde'yi ihlal eder.

## Madde XIII — Doküman ve Kod Standartları

Dokümanlar: *Designing Data-Intensive Applications*, *DDD*, *Thinking in Systems* ve
RFC'ler gibi yazın — akademik, gerekçeli, dolgusuz, Mermaid diyagramlarıyla
([`documentation-style.md`](../.claude/standards/documentation-style.md)). Dil: dokümanlar
Türkçe, teknik terimler orijinal ([`language-policy.md`](../.claude/standards/language-policy.md)).
Kod: demo kod yok, oyuncak mimari yok, kestirme yok; finansal düzeyde ölçek varsay;
reference platform optimize etmeden önce teoriyi kanıtlar
([`coding-standards.md`](../.claude/standards/coding-standards.md)).

## Madde XIV — Karar Yönetişimi (RFC / ADR Yaşam Döngüsü)

- **RFC** (`6000-rfc/`, kimlik `RFC-6xxx`): Külliyat'ta, mimaride ya da standartlarda bir
  değişiklik önerir. Yaşam döngüsü: `Draft → Review → Skeptic-Challenged → Accepted |
  Rejected → Superseded`.
- **ADR** (`5000-architecture/adr/`, kimlik `ADR-NNNN`): bir mimari kararı ve Külliyat atfını
  kaydeder. Yaşam döngüsü: `Proposed → Accepted → Superseded`.
- `ens-ceo` uzun vadeli hizayı korur; `ens-skeptic` sağlamlığı korur; `ens-style-guardian`
  tutarlılığı korur. Önemli kararlarda hiçbiri atlanmaz.

## Madde XV — Değişiklik (Amendment)

Yalnızca şu koşulları sağlayan bir RFC ile değiştirilir: (a) değiştirilen maddeye atıf
yapar, (b) skeptic saldırısından geçer, (c) Madde XIV uyarınca kabul edilir. Madde III
yalnızca sözcük düzeyinde değiştirilebilir, niyet düzeyinde asla.

---

*Implementation teoriyi kanıtlayacak. Teori asla implementation'dan türetilmeyecek.*
