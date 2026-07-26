---
id: SKR-039
type: skeptic-review
validation_dimension: ontology
origin: ENS-4010
depends_on: [ENS-4010, ENS-4001, ENS-4030, ENS-4031, SKR-038, SKR-020]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
---

# SKR-039 — Foundational Ontology (ENS-4010 v0.5.0) invariant-kapanış (Yara A/B) Saldırısı

> **Bağımsızlık beyanı (G2/G3):** Bu inceleme, ENS-4010'u v0.4.0→v0.5.0'a taşıyan düzeltme
> çağrısından **tamamen ayrı, taze context'te** yapıldı. SKR-038'in düzeltmesini SKR-038 onaylayamaz;
> bu blocking-5 hattının **2. bağımsız skeptic turudur** (1. tur: SKR-038, `wounded`). Yazarın öz-beyanına
> (§"Invariant denetim tablosu", §"SKR-038 Yara A/B'ye yanıt") **güvenmedim**: Node Registry'nin 17
> node'unu, Semantic Profiles'ın 9 profilini, Relation Registry'nin 21 satırını ve tüm `Trans:✓`
> işaretlerini sıfırdan elle taradım; her satisfiability iddiasını inverse-domain/range'e inerek yeniden
> türettim. Boyut: **ontology** — Scientific değil.

## Verdict
**survives.** SKR-038'in iki yarası da gerçekten kapatıldı ve — en kritik test — v0.5.0'ın merkezî
iddiası ("iki invariant TÜM profillere/Trans:✓ relation'a uygulandı, başka taranmamış aynı-sınıf kusur
yok") **bağımsız tam-taramadan sağ çıktı.** Yara A-1 (Intent/Goal): `refines`|`pursued_by` gerçekten
Goal'i conformant kılıyor (Registry-türetimiyle doğrulandı). Yara A-2 (Rule/derived_from): Registry
satırı eklendi, tutarlı, ve Constraint artık `derived_from`→Purpose→P1 ile Semantic Closure'da gerçekten
Principle'a ulaşıyor (elle takip edildi). **Kapsamlılık testi (task Q3):** 17 node'un tamamı denetim
tablosunda; 9 profilin her zorunlu ilişkisel-kenarını bağımsız türettim, **hepsi sağlanıyor**; yalnızca
`part_of` ve `specializes` `Trans:✓` taşıyor ve her ikisi de `range ⊆ domain` — **SKR-038/ENS-4031-D-1
deseninin 3. tekrarı yok.** Yara B: `is_root` çıkarımsal tanımı mekanik test edilebilir; "≥2 kök →
uyarı" kuralı da öyle; root/orphan yapısal ayırt-edilemezliği gizlenmemiş, dürüstçe uyarı-seviyesine
indirilmiş. Refuted-değil, wounded-değil: hedeflenmiş boşluklar kapandı, geri kalan üç bulgu
**bloke-etmeyen keskinleştirme**. Statü `skeptic-challenged` → `skeptic-cleared` önerilir.

## Yenilik incelemesi
Bu tur da (SKR-038 gibi) bir *kavram* değil, bir *iç-tutarlılık kapanışı* sınıyor; yenilik ekseni
ilgisiz. Eklenen mekanizmalar — profil-satisfiability tam-taraması (SHACL shape-conformance),
transitivity well-formedness (OWL property-characteristics), çıkarımsal `is_root` (deductive class
membership) — standart tip-sistemi kontrolleridir, uydurma yok; ENS'e özgü olan yalnızca bunların kendi
Registry'sine tam uygulanması. Prior-art konumu SKR-019/SKR-031/SKR-038'de dürüstçe kuruldu; delta darlığı
korunuyor.

## Task Q1 — Yara A-1 (Intent/Goal/served-by): **DOĞRU, bağımsız türetildi**
Registry'den inverse-domain/range'i elle çıkardım:
- `serves: Decision → Purpose` ⇒ `served_by` domain = {Purpose}, range = {Decision}. **Purpose** ∈ served_by
  domain ✓ → Purpose profili conformant.
- `refines: Goal → Purpose` (N:1) ⇒ **Goal** ∈ refines domain ✓ (çıkan kenar). Conformant yol 1.
- `pursues: Actor/Organization → Goal` ⇒ `pursued_by` domain = {Goal} (range = {Actor,Organization}).
  **Goal** ∈ pursued_by domain ✓ (gelen kenar) → conformant yol 2.
İki node-tipi de artık conformant instance üretir. SKR-038 Yara A-1 birebir kapandı — Goal'ün v0.4.0'da
sağlayamadığı düz `served-by` obligasyonu, node-tipine-özel `refines`|`pursued_by` ile değiştirildi.
Assertion/Resource ile aynı disjunctive tarz. **Doğrulandı.**

## Task Q2 — Yara A-2 (Rule/derived_from): **DOĞRU + closure gerçekten kuruluyor**
- **(a) Registry satırı:** line 166 `derived_from | Constraint → Purpose/Constraint | → | N:N | derives | ✗ | ✗ | Allowed`.
  Format 21 sütun-şemasına uygun, inverse `derives` tanımlı, Trans:✗. Tutarlı, eklendi. ✓
- **(b) Constraint artık Principle'a izlenebiliyor mu?** Elle Semantic Closure zinciri:
  Constraint `--derived_from-->` Purpose (line 166 Registry-lisanslı) → Purpose closure `→ P1` (line 275).
  Yani **Constraint → Purpose → P1 zinciri gerçekten kuruluyor** (line 276-277 iddiası doğru). v0.4.0'da
  Constraint yalnız downstream `constrains` taşıdığından hiçbir upstream yol yoktu → orphan riski
  gerçekti; v0.5.0 bunu kapatır. Constraint→Constraint dalı (derived_from range'inde Constraint da var)
  bir Purpose'a ulaşmadan biterse leaf-Constraint orphan kalır — ama bu, line 279 orphan-linter'ının
  yakaladığı **modelleme sorumluluğudur**, yapısal kusur değil (traversal transitivity gerektirmez,
  yalnız kenar-takibi). **Doğrulandı.**

## Task Q3 — Invariant denetim tablosunun kapsamlılığı (EN KRİTİK): **GERÇEKTEN KAPSAMLI**
Bu, SKR-038'in (v0.4.0'ın kaçırdığı Goal) ve ENS-4031 D-1'in (ENS-4025'in kaçırdığı örnek) aynı hatayı
3. kez tekrarlayıp tekrarlamadığının testi. **Node Registry'yi sıfırdan saydım:** 17 node (Decision,
Purpose, Goal, Constraint, Context, Memory, Learning, Evidence, Claim, Actor, Organization, Capability,
Resource, Attention, Event, State, Metric). Denetim tablosu (line 213-230) **tam 17 satır** — her node
kapsanmış, atlanan node yok. Her satırın satisfiability iddiasını inverse'e inerek bağımsız türettim:

| Profil / Node | Bağımsız türetim (inverse-domain/range) | Sonuç |
|---|---|---|
| Deliberative/Decision | serves (Dec∈dom), has_context (Dec∈dom), constrained_by=constrains⁻¹ (dom∋Dec) | ✓ |
| Intent/Purpose | served_by=serves⁻¹ dom {Purpose} | ✓ |
| Intent/Goal | refines dom {Goal}; pursued_by=pursues⁻¹ dom {Goal} | ✓ |
| Rule/Constraint | constrains dom∋Constraint; derived_from dom {Constraint} | ✓ |
| Store/Memory | stores/retrieves dom {Memory} | ✓ |
| Assertion/Evidence | supports dom∋Evidence; invalidates dom {Evidence} | ✓ |
| Assertion/Claim | supports dom∋Claim; supported_by=supports⁻¹ dom∋Claim; invalidated_by=invalidates⁻¹ dom {Claim} | ✓ |
| Assertion/Learning | updates dom {Learning} | ✓ |
| Agent/Actor | owns dom {Actor}; pursues dom∋Actor | ✓ |
| Agent/Organization | pursues dom∋Organization (owns dom yalnız {Actor} → doğru dışlanmış) | ✓ |
| Resource/Capability | supports dom∋Capability | ✓ |
| Resource/Resource | consumed_by=consumes⁻¹ dom∋Resource; required_by=requires⁻¹ dom {Resource} | ✓ |
| Resource/Attention | allocated_to dom {Attention}; consumed_by dom∋Attention | ✓ |
| Measure/Metric | measures dom {Metric} | ✓ |
| Temporal/Context | context_of=has_context⁻¹ dom {Context} | ✓ |
| Temporal/Event | changes dom {Event}; produced_by=produces⁻¹ dom {Event} | ✓ |
| Temporal/State | state_of=has_state⁻¹ dom {State}; changed_by=changes⁻¹ dom {State} | ✓ |

**17/17 bağımsız olarak sağlanıyor** — tablonun ✓'leri doğru, uydurma yok. **Trans:✓ taraması:**
Relation Registry'nin 21 satırının Trans sütununu tek tek okudum — yalnız `part_of` ({Actor,Organization}
→ {Organization}, range⊆domain ✓) ve `specializes` (domain=range ✓) `Trans:✓`; kalan 19'u `Trans:✗`.
Denetim tablosu (ii) bu ikisini tam listeliyor. **Taranmamış Trans:✓ relation yok, taranmamış node yok.**
SKR-038/D-1 deseni **3. kez tekrarlanmadı** — bu turun aradığı "yeni kaçak" yok. v0.4.0'ın hatası
temada değil kapsamdaydı; v0.5.0 kapsamı fiilen tamamlamış.

## Task Q4 — Yara B (kök tanımı): **MEKANİK TEST EDİLEBİLİR, dürüstçe sınırlanmış**
- `is_root(o) ≡ Organization(o) ∧ ¬∃x. part_of(o,x)` **saf yapısal bir predikat**: formal-checker her
  Organization node'u için "çıkan part_of kenarı var mı?" sorar — karar-verilebilir, keyfî owner-kararı
  gerektirmez. v0.4.0'ın "kök muaf" parantezli istisnası (önsel "hangisi kök" bilgisi isterdi) ile
  kıyasla **gerçek bir operasyonelleşme.** ✓
- "≥2 kök → Linter uyarı" da mekanik: part_of'suz Organization'ları say, >1 ise uyarı. ✓
- **Root/orphan ayırt-edilemezliği gizlenmiş mi?** Hayır — belge (line 199-201) *açıkça kabul ediyor* ki
  gerçek kök ile öksüz aynı yapısal imzayı taşır ve çözümü "karar verme, **uyarı ver, insana devret**"
  biçiminde dürüstçe uyarı-seviyesine indiriyor. Sorun gizlenmiş değil, doğru katmana (human-review)
  taşınmış. Holding/çok-tüzel-kişilik meşru olduğundan hata değil uyarı olması yerinde.
- **Kalan artık (bloke etmez, temel sınır):** ≥2-kök uyarısı yalnızca **birden çok** kök varsa tetiklenir;
  *tek* bir öksüz-Organization (üstü hiç modellenmemiş, deployment'ta yegâne kök gibi görünen) yapısal
  olarak yakalanamaz. Ama bu, "modellenmemiş bir node'un yokluğunu tespit etme" imkânsızlığıdır — hiçbir
  yapısal kuralla çözülemez, epistemik bir sınırdır, gizli kusur değil. Uyarı-tasarımının dürüst kapsamı
  içindedir. **Yara B kapandı.**

## Task Q5 — `derived_from` yeni bir Kusur-1/2/3 sınıfı yaratıyor mu? **HAYIR**
- **Başka node derived_from'a muhtaç mı?** Registry/Profiles taraması: `derived_from`'u yalnız Rule
  profili (Constraint) zorunlu kılar; başka hiçbir profil onu istemez → sağlayamayan başka node yok.
- **`derives` (inverse) obligasyon yaratır mı?** Hiçbir profil `derives`'ı zorunlu kılmaz → Purpose/
  Constraint'e yeni yük binmez.
- **Trans:✗ tutarlı mı?** range {Purpose,Constraint} ⊄ domain {Constraint}, ama Trans:✗ olduğundan
  transitivity well-formedness ona uygulanmaz (line 174-175 doğru). Zincirlenmez, sorun yok.
**Yeni satisfiability boşluğu açılmıyor.**

## En güçlü karşı-argüman (steelman) — ve neden bloke etmiyor
SKR-038 "invariant'ı gerçekten *çalıştırırsan* dördüncü kusuru görürsün" diyerek wounded verdi. Aynı
sertlikle sordum: *invariant'ı bir kez daha çalıştırınca beşinci bir kusur çıkar mı?* Çıkmadı — 17
node'u ve 21 relation'ı bağımsız türettim, denetim tablosu gerçekten tüm ekseni kapsıyor. En güçlü
kalan itiraz **satisfiability'nin değil, tarzın**: Deliberative profili (line 131) hâlâ *flat* bir liste
ve ilişkisel-kenarlar (`serves`, `constrained_by`, Context=has_context) ile Decision-Object
*bileşenlerini* (Evidence, Alternatives, Outcome) tek listede karıştırıyor — "Evidence" düz sözcük
olarak yazılmış ve Evidence *node*'unun adıyla çakışıyor. Bir formal-checker generator (G-09/10) bunu
yanlışlıkla "Decision→Evidence ilişkisel obligasyonu" diye ayrıştırırsa, Registry'de böyle bir kenar
olmadığından **sahte bir Kusur** üretir; ya da ileride biri onu ilişkisel okursa gerçek boşluk doğar.
Ama şu an: satisfiability invariant'ı açıkça "her zorunlu *kenarı*" der (line 184) ve Evidence/Alternatives/
Outcome kenar değil bileşendir (ENS-2001 Decision anatomisi; Evidence, Decision'a doğrudan değil
Claim üzerinden — `supports: Evidence→Claim` — bağlanır). Yani bugün **kusur değil**, yalnız SKR-038
talep-3'ün (profil-tarz birleştirme) **eksik kalan %10'u**: Intent/Rule/Agent birleştirildi ama
Deliberative flat kaldı. Bloke-etmeyen keskinleştirme.

## İç tutarlılık
- **SKR-038 ile:** Yara A-1, A-2, B talepleri birebir karşılanmış; SKR-038'in DOĞRU bulduğu üç yama
  (Kusur 1/2/3) bozulmamış (regresyon-taraması: `supports`/`part_of`/Resource profili değişmedi).
- **ENS-4001 ile — YENİ ÇAPRAZ-SEVİYE HOMONİM (bloke etmez):** `derived_from` **zaten** ENS-4001'de
  `ens-meta:` seviyesinde vardı (line 70: `Law → Principle; Concept → Concept`, ve ENS-4030 SC-004
  `Theory derived_from ≥1 Principle`). v0.5.0 şimdi `ens-core:derived_from`'u (`Constraint →
  Purpose/Constraint`, farklı domain/range, farklı Trans-karakteristiği) ekliyor. Bu, belgenin §Homonim
  çözümü bölümünde (line 93-95) **açıkça ele aldığı `ens-meta:Claim`≠`ens-core:Claim` deseninin
  ikinci örneğidir** — namespace mekanik olarak ayırır (Kusur değil), ama SKR-019 Bulgu 4 disiplini
  gereği homonim-notuna eklenmeli. Şu an eklenmemiş → terminoloji-hijyeni boşluğu (bloke etmez).
- **ENS-4031 ile:** `part_of` domain-widening doğrulandı (SKR-038); IR-002/IR-005 hâlâ "lisanslı değil"
  bayrağını taşıyor (ENS-4031 line 87-105, 293-302) — ENS-4010 bu edimi doğru biçimde ENS-4031 owner'ına
  (ens-philosopher) bırakmış, kendi dokunmamış (G3 temiz). Bu artık **açık bir downstream borç**: part_of
  düzeldiğine göre IR-002 bayrağı kaldırılabilir; owner edimini bekliyor.
- **ENS-4025/4030/4031 ile — propagasyon gözlemi (bloke etmez):** Registry artık 21 relation (SKR-038
  "20" diyordu). Bu belgelerden biri "Registry'nin N relation'ı" gibi bir sayıma ya da tam-relation
  enümerasyonuna dayanıyorsa, `derived_from`'un eklenmesi bir tutarlılık-turu ister — tıpkı part_of/IR-002
  gibi downstream owner'a bırakılmalı.
- **Terminoloji sürüklenmesi:** Deliberative dışında profiller node-tipine-özel disjunctive tarzda
  birleşti (SKR-038 talep-3 büyük ölçüde karşılandı). Kalan tek istisna Deliberative (yukarıda).

## Varsayım haritası
| Düzeltme | Dayandığı varsayım | Kırılma koşulu |
|----------|--------------------|----------------|
| A-1 (Intent/Goal) | `refines`/`pursues` Registry-domain/range'i Goal'i kapsıyor | Registry'de refines/pursues domain'i değişirse kırılır (invariant yakalar) |
| A-2 (derived_from) | Constraint bir Purpose'a *ulaşan* bir derived_from zinciri taşır | Constraint yalnız Constraint'lerden türeyip Purpose'a hiç ulaşmazsa orphan (orphan-linter yakalar) |
| Kapsamlılık | 17 node + 2 Trans:✓ tam küme; başka profil/transitive yok | Registry'ye yeni node/profil/Trans:✓ eklenirse denetim tablosu elle güncellenmezse boşluk açılır (Faz-4 formal-checker otomatikleştirene dek elle-bakım riski — belge line 296-297 bunu dürüstçe FC olarak listeliyor) |
| Yara B (is_root) | Sağlıklı ağaçta tam-bir-kök beklenir | Tek öksüz-kök yapısal yakalanamaz (temel epistemik sınır, uyarı-tasarımı içinde dürüst) |

## Yanlışlanabilirlik
İki invariant somut ve yanlışlanabilir (line 177-186). Bu turda **kriteri belgenin kendisine tam
uyguladım** (17 node × türetim + 21 relation × Trans-okuması) ve — SKR-038'in aksine — **hepsi geçti.**
Kriter çalışıyor *ve* öz-uygulama bu kez eksiksiz. Kusuru kapsamda değil, artık yalnızca (i) Deliberative
tarz-flatness'ında ve (ii) homonim/propagasyon hijyeninde — hiçbiri satisfiability/well-formedness
ihlali değil.

## Sahibine talepler (hiçbiri bloke etmez — survives eşiği zaten geçildi)
1. **(Keskinleştirme, SKR-038 talep-3'ün kalanı)** Deliberative profilini de node-tipine-özel/işaretli
   yaz: ilişkisel-kenarları (`serves`, `constrained_by`, `has_context`) Decision-Object bileşenlerinden
   (Evidence, Alternatives, Outcome, Lifecycle) tipografik olarak ayır (ör. relation'ları backtick,
   bileşenleri düz) — formal-checker'ın "Evidence"i sahte-ilişki sanmasını önler.
2. **(Terminoloji hijyeni)** §Homonim çözümü notunu `ens-meta:derived_from` (ENS-4001) ≠
   `ens-core:derived_from` (bu belge) çiftini kapsayacak şekilde genişlet — `Claim` homoniminin ikinci
   örneğidir, aynı disiplinle belgelenmeli.
3. **(Downstream propagasyon — owner edimleri)** (a) ENS-4031 IR-002/IR-005 "lisanslı değil" bayrağı
   artık kaldırılabilir (owner ens-philosopher); (b) `derived_from` ile Registry 21 relation'a çıktı —
   ENS-4025/4030/4031'de relation-kümesine dayanan bir sayım/enümerasyon varsa tutarlılık turu
   (ilgili owner'lar).

---

*SKR-038 haklı olarak sordu: "invariant'ı gerçekten çalıştırdın mı, yoksa üç bilinen satırda mı
bıraktın?" v0.5.0 bu kez tüm 17 node'da ve 21 relation'da çalıştırmış — ben de bağımsız çalıştırdım,
aynı sonuca vardım: taranmamış aynı-sınıf kusur kalmadı. İyi ontoloji kendi kenarlarıyla kendini
düzeltir; bu tur, düzelttiğini gösterdi. Geri kalan (Deliberative tarzı, derived_from homonimi) bir
sonraki cila turunun işidir, kapının değil.*
