---
id: SKR-038
type: skeptic-review
validation_dimension: ontology
origin: ENS-4010
depends_on: [ENS-4010, ENS-4001, ENS-4020, ENS-4031, SKR-028, SKR-030, SKR-031]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
---

# SKR-038 — Foundational Ontology (ENS-4010 v0.4.0) profil↔registry düzeltmesi Saldırısı

> **Bağımsızlık beyanı (G2/G3):** Bu inceleme, ENS-4010'u v0.3.0→v0.4.0'a taşıyan (blocking-5 /
> Kusur 1-2-3) düzeltme çağrısından **tamamen ayrı, taze context'te** yapıldı. Yazarın öz-beyanına
> (§"SKR-028/030/031'e yanıt") güvenmedim; Relation Registry, Semantic Profiles ve `depends_on`
> zincirinin gerçek satırlarını sıfırdan denetledim. Boyut: **ontology** (profil↔registry
> satisfiability, transitivity well-formedness) — Scientific değil.

## Verdict
**wounded.** Üç hedeflenmiş düzeltme (Kusur 1/2/3) **teknik olarak doğru** ve bağımsız denetimden
sağ çıkıyor: (1) `supports` domain'ine `Claim` + node-tipine-özel Assertion profili → Claim/Learning
artık conformant; (2) Resource profili genellemesi `required_by`'ı (SupplierRelationship) kapsıyor
ve fazla gevşetmiyor; (3) `part_of` domain-widening range⊆domain'i sağlayıp zinciri well-formed
kılıyor, IR-002/IR-005 bayrağı gerçekten kaldırılabilir hâle geliyor. **Ama iki gerçek yara var:**
(A) belgenin merkezî iddiası — "kök tema sistematik olarak çözüldü, dağınık bulgular değil" (line
248-251) — **kendi eklediği yeni invariant tarafından yanlışlanıyor:** *aynı sınıftan* dördüncü bir
kusur (**Intent profili / Goal / `served-by`**) taranmadan kalmış; "Profile satisfiability"
invariant'ı gerçekten çalıştırılsaydı Goal'i (ve Rule/`derived_from`'u) da yakalardı. (B) Agent
profilindeki "**kök Organization muaf**" istisnası **mekanik denetlenemez** — kök ile "gerçekten
üstsüz ama yanlış-modellenmiş" Organization'ı ayırt edecek formal işaret yok; `part_of`
Organization için sessizce optional'a düşüyor. Üç hedeflenmiş yama sağlam; sistematik-kapanış
iddiası ve enforceability eksik → wounded, refuted değil.

## Yenilik incelemesi
Bu tur bir *kavram* değil, bir *iç-tutarlılık düzeltmesi* sınıyor; yenilik ekseni ilgisiz. Prior-art
konumu (OWL domain/range/transitive, SHACL shapes = Semantic Profiles) SKR-019/SKR-031'de zaten
dürüstçe kurulmuştu; v0.4.0 yalnızca uygular. Eklenen iki "formal-checker invariant'ı" (profil
satisfiability + transitivity well-formedness) **SHACL constraint-validation ve OWL property
characteristics'in standart iç-tutarlılık kontrolleridir** (uydurma yok); ENS'e özgü olan yalnızca
bunları kendi Registry'sine uygulaması. Delta dürüst, şişirme yok.

## Kusur 1 — Assertion profili (Claim/Learning conformance): **DOĞRU**
Relation Registry (line 132) artık `supports | Capability/Evidence/Claim → Purpose/Claim`. Bağımsız
conformance denetimi (profil line 120, "en az bir assertive bağ"):
- **Evidence:** `supports` (Evidence∈domain ✓, →Purpose/Claim) veya `invalidates` (Evidence∈{Evidence} ✓, →Claim). **Conformant ✓**
- **Claim:** `supports` çıkan (Claim artık domain'de ✓, →Purpose/Claim) **veya** `supported_by`
  gelen (= supports⁻¹, domain {Purpose,Claim} ∋ Claim ✓) **veya** `invalidated_by` gelen
  (= invalidates⁻¹, domain {Claim} ✓). Üç yoldan da sağlanır. **Conformant ✓**
- **Learning:** `updates` (Learning→Memory, Learning∈domain ✓). **Conformant ✓**

Üç node de gerçekten conformant instance üretebiliyor. **Regresyon kontrolü:** `supports` domain'ine
Claim eklemek D-1 fix'ini bozmuyor — `supports` domain'i hâlâ Purpose'u içermez (`Purpose --supports-->`
hâlâ Forbidden). Range zaten {Purpose,Claim} idi, dokunulmadı. `supports` Trans:✗ olduğundan
Claim→Claim öz-desteği otomatik döngü türetmez (gerçek döngüyü ENS-4030 MC-005 yakalar). Profil
`invalidates`'i Claim için doğru biçimde dışlıyor (invalidates domain'i yalnız Evidence). Temiz.

## Kusur 2 — Resource profili genellemesi: **DOĞRU, fazla gevşetmiyor**
Profil (line 122): "en az bir kaynak-rol kenarı: `consumed_by`|`allocated_to`|`required_by`|`supports`;
scarcity". Üç Resource-profili node'u için bağımsız denetim:
- **Resource** (ör. SupplierRelationship): `required_by` (= requires⁻¹, requires:{Decision,Capability}→{Resource}
  ⇒ required_by domain {Resource} ∋ Resource ✓) **veya** `consumed_by` (= consumes⁻¹, range{Attention,Resource}∋Resource ✓). **Conformant ✓** — SKR-030'un SupplierRelationship kaygısı kapatıldı.
- **Capability:** `supports` (Capability∈domain ✓). **Conformant ✓**
- **Attention:** `allocated_to` (Attention→Decision ✓) veya `consumed_by` (∈consumes range ✓). **Conformant ✓**

**"Fazla gevşetme" testi (task Q2):** Profil hâlâ "en az bir" kenar **+ scarcity** ister; kenarsız
bir Resource "en az bir kaynak-rol kenarı"nı sağlamaz → conformant sayılmaz. Yeni bir kaçak
açılmıyor. Genelleme gelen (`consumed_by`/`required_by`) + çıkan (`allocated_to`/`supports`)
kenarları karıştırıyor, ama bu §"Profile satisfiability" invariant'ının (line 159-161) açıkça izin
verdiği "çıkan ya da gelen" biçimiyle tutarlı. Temiz.

## Kusur 3 — `part_of` domain-widening: **YAPISAL OLARAK DOĞRU**
Registry line 147: `part_of | Actor/Organization → Organization | Trans ✓`.
- **range ⊆ domain?** range {Organization} ⊆ domain {Actor, Organization} → **evet ✓.** Well-formedness sağlanıyor.
- **Elle hop-izleme (task Q3a):** Team─part_of→Division─part_of→Company. Her kenar geçerli
  (Team/Division/Company hepsi Organization ∈ domain ∧ ∈ range). Transitivity: orta düğüm Division
  hem hop-1 range'i (Organization ✓) hem hop-2 domain'i (Organization ✓) → **well-formed ⇒ Team
  part_of Company türetilir.** Actor kuyruğu: person─part_of→Team─part_of→Company; person yalnız
  domain'de (Actor), hiçbir hop'ta range olması gerekmiyor → **well-formed ⇒ person part_of Company.**
  Zincir gerçekten kuruluyor.
- **IR-002/IR-005 bayrağı (task Q5):** B1'in kök nedeni "`Organization --part_of--> Organization`
  Registry'de yok" idi; artık Organization domain'de olduğundan bu kenar lisanslı → ENS-4031 owner'ı
  IR-002/IR-005 "lisanslı değil" bayrağını **gerçekten kaldırabilir.** ENS-4010 bunu doğru biçimde
  ENS-4031 owner'ına (ens-philosopher) bırakıyor, kendi dokunmuyor (G3 doğru). **Doğrulandı.**

### Enumeration-vs-subsumption (task Q3b): savunulabilir, `pursues` argümanı YÜZEYSEL DEĞİL
`pursues: Actor/Organization → Goal` **Trans: ✗** taşır; dolayısıyla transitivity well-formedness
invariant'ı ona hiç uygulanmaz — `pursues` "aynı sorunu taşımıyor" (task'ın şüphesinin aksine).
Yani `pursues`'a atıf yalnızca **enumeration notasyonunun** belgede zaten var olduğunu gösterir,
"pursues da transitive-tutarlı" iddiası **yapılmıyor** — argüman dürüst ve dar. Subsumption'ı reddetme
gerekçesi ("Organization, actor'lardan *oluşan* bir bütündür, özelleşmiş bir birey-actor değil")
ENS'in kendi Node Registry'siyle de tutarlı: Actor ve Organization **kardeş** olarak ayrı ayrı Agent
profili taşır (line 102-103), ikisi de `pursues` edebilir — parent/child değil, kardeş. Enumeration
bu kardeşliği korur; subsumption gereksiz ve tartışmalı bir "her Organization bir Actor'dır" taahhüdü
yükler. **Karar savunulabilir, hatta subsumption'dan daha temiz.**

### Kök Organization istisnası (task Q3c): **YENİ, MEKANİK-DENETLENEMEZ KAÇAK (Yara B)**
Agent profili (line 121): "`part_of` (kök-olmayan üye için — kök Organization muaf)". Bağımsız itiraz:
- Validator, hangi Organization'ın "kök" olduğunu **nasıl** bilir? Formal bir `is_root` işareti,
  ya da "part_of'suz Organization = kök" invariant'ı **yok.**
- Sonuç: gerçek bir kök ile *üstü olması gerekirken modelleyicinin unuttuğu* öksüz bir Organization
  validator için **ayırt edilemez.** İkisi de "part_of yok" durumundadır; ikisi de profili geçer.
- Dolayısıyla `part_of`, Organization için pratikte **optional'a** düşer — ama "Zorunlu" sütununda,
  parantezle listelenmiş. Bu bir iç-tutarsızlık: ya part_of Organization için zorunludur (o zaman
  formal kök-işareti şart), ya optional'dır (o zaman "Zorunlu" sütununda parantezle durması yanlış).
- `part_of` cardinality zaten N:1 (0..1 üst) — yani "0 üst = kök" tipleme düzeyinde meşru. O hâlde
  düzeltme muhtemelen basit: part_of'u Agent profilinin **Actor-için-koşullu**, Organization-için
  0..1 (optional) olarak node-tipine-özel yazmak — tıpkı Assertion profilinin v0.4.0'da yapıldığı
  gibi. Ama şu anki hâliyle keyfî owner-kararına açık kapı bırakıyor.

## En güçlü karşı-argüman (steelman) — Yara A: sistematik-kapanış iddiası kendi invariant'ıyla çürüyor
Belge §"Sistematik ders" (line 248-251) merkezî bir iddia kuruyor: *"üç kusur da tam olarak [iki
invariant'tan] birinin ihlaliydi — kök tema, dağınık bulgular değil."* Bu, "üç bulgu değil, bir
tema çözdüm" retoriğidir. Ama **belge kendi eklediği "Profile satisfiability" invariant'ını tüm
profillere uygulamadı.** Ben uyguladım ve *aynı sınıftan* bir dördüncü ihlal buldum:

- **Intent profili / Goal / `served-by`:** Intent profili (line 118) `served-by`'ı **Purpose ve Goal**
  için zorunlu kılar. `serves: Decision → Purpose` (line 131) → `served_by` domain'i {Purpose}.
  **Goal, `served_by` domain'inde YOK.** Goal'ün Registry-kenarları yalnız `refines` (çıkan, →Purpose)
  ve `pursued_by` (gelen, = pursues⁻¹). Yani bir **Goal**, Intent profilinin zorunlu `served-by`
  kenarını Registry ile **hiçbir biçimde sağlayamaz** — tıpkı Kusur 1'in Claim'i, Kusur 2'nin
  SupplierRelationship'i gibi. **Bu, üç kusurla birebir aynı sınıf, aynı belge, farklı satır.**
- İkincil (aynı invariant'ı deler): **Rule profili / `derived_from`** — `derived_from` Relation
  Registry'de **hiç yok** (kayıtlı 20 relation arasında değil); Constraint bu zorunlu kenarı
  sağlayamaz. Bu, "satisfiability"nin daha sert biçimi (kenar Registry'de bile yok).

Steelman: owner "served-by açıkça sadece Purpose için, Goal Identity+specializes ile yetinir"
diyebilir. Ama profil bunu **söylemiyor**; ve Assertion/Resource profillerini v0.4.0'da tam da bu
belirsizliği gidermek için node-tipine-özel yeniden yazdıysanız, Intent'i (ve Rule'u) aynı titizlikle
yazmamış olmanız, "sistematik tarama yaptım" iddiasını **yerel olarak yanlışlıyor.** Bu SKR-031'in
ENS-4031'de yakaladığı desenin aynısı: *falsifiable kriter var ama öz-uygulama eşit değil.* Kök kusur
küçük ve giderilebilir (üç yamayla aynı tür) → **refuted değil**; ama sistematik-kapanış iddiası
düzeltilmeden ya da Intent/Rule taranmadan → **survives da değil** → **wounded.**

## Yanlışlanabilirlik
Üç düzeltme yanlışlanabilir ve somut invariant'la beyan edilmiş (line 152-161): "Trans:✓ ⇒ range⊆domain"
ve "her profil zorunlu-kenarı Registry-domain/range'de var mı". Bu kriteri belgenin *kendisine*
uyguladığımda üçü geçiyor (doğru), ama Intent/Goal ve Rule/derived_from takılıyor — yani **kriter
çalışıyor, öz-uygulama eksik.** Kusur teoride değil, taramanın kapsamında.

## Varsayım haritası
| Düzeltme | Dayandığı varsayım | Kırılma koşulu |
|----------|--------------------|----------------|
| Kusur 1 | `supports` domain'ine Claim eklemek semantik olarak doğru (Claim argüman zincirinde Purpose/Claim destekler) | Bir Linter `supports` Trans:✗ iken argüman-zinciri kapanışı beklerse boşa düşer (ama IR yok, sorun değil) |
| Kusur 2 | "en az bir + scarcity" edgeless-Resource'u dışlar | scarcity'nin formal ölçütü tanımsız (Faz-4); disipline kalırsa kaçak açılabilir |
| Kusur 3 | Örgütsel hiyerarşi enumeration ile lisanslanır | Kök-işareti formal değilse part_of Organization için enforce edilemez (Yara B) |
| "Sistematik kapanış" | İki invariant tüm profillere uygulandı | **Yanlış** — Intent/Goal, Rule/derived_from taranmamış (Yara A) |

## İç tutarlılık
- **ENS-4020 / SKR-028 / SKR-030 ile:** Kusur 1/2'nin ENS-4020 tarafında açığa çıkan hâli (Claim
  supports-kaynağı olamıyor; SupplierRelationship required_by) artık ENS-4010 tarafında gerçekten
  kapatılmış — ENS-4020'nin baypasları (supported_by ile) artık gereksiz, profil doğrudan sağlıyor.
  Tutarlı.
- **ENS-4031 / SKR-031 ile:** B1'in çözüm-seçeneği (b) (`part_of` domain genişletme) birebir
  uygulanmış; SKR-031'in önerdiği iki yoldan minimal olanı seçilmiş. IR-002/IR-005 bayrağının
  kaldırılması ENS-4031 owner'ına doğru biçimde bırakılmış. Tutarlı, G3 temiz.
- **Yeni iç-çelişki (Yara A):** Intent profili, v0.4.0'ın kendi "Profile satisfiability" invariant'ıyla
  çelişiyor — belge içi tutarsızlık.
- **Terminoloji sürüklenmesi:** Assertion/Resource profilleri artık node-tipine-özel disjunctive;
  Intent/Deliberative/Rule hâlâ flat "zorunlu" liste. İki farklı profil-yazım tarzı aynı belgede →
  sözlük/stil sürüklenmesi. ENS-4000 ile birebir karşılaştırma gerekmiyor (bunlar `ens-core:`
  yapısal profilleri), ama tek tarz seçilmeli.
- **Atıf dürüstlüğü:** §yanıt tablosundaki SKR-028/030/031 atıfları gerçek ve satır-doğrulanabilir;
  uydurma yok. "Öz-onay yok (G2/G3)" beyanı (line 253-255) dürüst ve doğru — bu turu davet ediyor.

## Sahibine talepler (kapıyı geçmek için)
1. **Yara A — sistematik iddiayı gerçekle ya da yumuşat.** İki seçenek: (a) "Profile satisfiability"
   invariant'ını **tüm** profillere uygula ve bulunan aynı-sınıf ihlalleri düzelt — en az **Intent
   profili / Goal / `served-by`** (Goal `served_by` domain'inde yok; node-tipine-özel yaz: Purpose→served-by,
   Goal→`refines`|`pursued_by`) ve **Rule profili / `derived_from`** (Registry'de hiç yok — ya relation
   olarak kaydet ya profil-attribute'una indir); VEYA (b) line 248-251'deki "kök tema, dağınık bulgu
   değil / üç kusur da tam olarak bunlardan biriydi" iddiasını "bilinen üç örneği kapattım; invariant'ın
   tam-tarama uygulaması Faz-4 formal-checker'a kalıyor" olarak dürüstçe daralt.
2. **Yara B — kök Organization istisnasını denetlenebilir kıl.** Ya formal bir kök-işareti/invariant
   ekle ("part_of'suz Organization kök sayılır; validator uyarı verir, hata değil"), ya part_of'u Agent
   profilinde node-tipine-özel yaz (Actor için koşullu, Organization için 0..1 optional — N:1
   cardinality'yle zaten uyumlu). Şu anki "Zorunlu sütununda parantezli muafiyet" mekanik denetlenemez.
3. **(Küçük)** Profil-yazım tarzını birleştir: ya hepsi node-tipine-özel disjunctive (Assertion/Resource
   gibi), ya hepsi flat — iki tarzın karışımı okuma ve formal-checker üretimi için sürtünme yaratır.

Talep 1 (özellikle Intent/Goal) ve 2 karşılandığında belge **survives** eşiğine gelir. Üç hedeflenmiş
yama zaten sağlam ve doğru; eksik olan tek şey — SKR-031'in ENS-4031'de yakaladığı desenin aynısı —
öz-denetimin **tüm profillere eşit** uygulanmasıdır.

---

*Üç yamanın hepsi isabetli; ama "bir kök tema çözdüm, dağınık bulgu değil" diyen bir düzeltmenin
kendi eklediği invariant, taranmamış dördüncü bir aynı-sınıf kusuru (Goal/served-by) hemen görünür
kılıyor. İyi ontoloji kendi kenarlarıyla kendini düzeltir — ama yalnızca invariant'ı gerçekten
*çalıştırdığında*, üç bilinen satırda değil tüm profillerde.*
