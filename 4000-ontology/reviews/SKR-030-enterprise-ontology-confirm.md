---
id: SKR-030
type: skeptic-review
validation_dimension: ontology
origin: ENS-4020
depends_on: [ENS-4020, SKR-027, SKR-028]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-030 — Enterprise Ontology (ENS-4020, operax) M2 Bağımsız Teyit Turu

> **Bağımsızlık beyanı (G2/G4):** Bu, ENS-4020'nin **ikinci bağımsız** Ontology Validation'ıdır.
> SKR-028 iki yapısal Bridge kusurunu (C.a `SupplierRelationship→Capability`, C.b `Replenishment→Decision`)
> *kendisi* v0.3'te düzeltti ve dürüstçe **M2 vermedi** — "kendi düzeltmemi kendim canonize edemem
> (G2); bağımsız teyit turu M2'yi hak edecek" dedi (M1'de bıraktı). Bu kayıt, o teyit turudur:
> v0.3'ün C.a/C.b düzeltmelerini — düzeltmeyi yapan context'ten bağımsız — ENS-2001 §Individuation,
> ENS-4001 §Semantic Connectors ve ENS-4010 (Node/Relation Registry, Semantic Profiles) ölçütlerine
> karşı sıfırdan yeniden yargılar. Boyut: **ontology** (specialization doğruluğu, Bridge sem-preservation,
> directionality/cardinality, Semantic Closure) — Scientific değil.

## Verdict
**survives → M2.** SKR-028'in iki düzeltmesi bağımsız incelemeden sağ çıkıyor: (C.b) Replenishment
bölünmesi ENS-2001 §Individuation'ı ve Bridge `semantic-preservation` şartını *gerçekten* sağlıyor;
(C.a) `SupplierRelationship→Resource` eşlemesi ENS-4010 Relation Registry `requires`-range'i tarafından
*mekanik olarak zorunlu kılınıyor (zorlama değil, tip-türevli).* İki bağımsız validator (SKR-028 +
SKR-030 → G4) sağlandığından **maturity: M2** hak edildi. ENS-4020'ye özgü hiçbir modelleme hatası
bulmadım.

**Ama iki önemli üst-akış (ENS-4010) kusuru teyit/genişletildi:** SKR-028'in "yukarı-bildirim"
olarak işaretlediği ENS-4010 profil↔registry gerilimi **gerçek** — üstelik düşündüğünden daha geniş
(Claim *ve* Learning'i etkiler). Bağımsız turda **ikinci bir örnek** buldum (Resource profili vs
`required_by`). Bunlar ENS-4020'nin değil **ENS-4010'un borcudur**; ENS-4020 doğru specialize
edip kusuru *yüzeye çıkardığı* için M2'yi bloke etmezler — ama ENS-4010'a karşı açık, takip edilen
kusur olarak kaydedilmelidir.

## SKR-028 ile örtüşme / ayrışma
- **Örtüşme:** C.a ve C.b'nin gerçek kusurlar olduğu ve v0.3 çözümlerinin doğru yönde olduğu —
  teyit ediyorum. SKR-028'in bildirdiği ENS-4010 iç-gerilimini de teyit ediyorum (aşağıda, hatta
  güçlendirerek).
- **Ayrışma:** SKR-028 M1'de bıraktı çünkü kendi düzeltmesiydi (G2). Bu tur M2 veriyor çünkü
  bağımsız ikinci validator sağlandı. Ek olarak SKR-028'in *kaçırdığı* ikinci profil↔registry
  örneğini (Resource/`required_by`) getiriyorum — bu turun asıl katkısı.

## C.b teyidi — Replenishment bölünmesi Individuation + sem-preservation sağlıyor mu?

**Bölünme:** `ReplenishmentSuggestion specializes ens-core:Claim` (tvf-önerisi, deliberation) +
`ReplenishmentOrder specializes ens-core:Decision` (POSTED/onaylı, commitment atomu).

**ENS-2001 §Individuation'a karşı (bağımsız):** Atom = commitment-mühürlü karar; deliberation atom
*değil* (§Model 1, 4 koşul: Owner/Purpose/Alternatives/Commitment). tvf çıktısı (öneri) commit
edilmeden önceki Reasoning-fazı önermesidir → **Decision olamaz, Claim olmalı.** POSTED emir
Commitment olayını taşır → **Decision atomu.** Bölünme bu ayrımı ontolojiye mühürlüyor; ENS-2001
lifecycle (Reasoning → **Commitment** ← ATOM SINIRI → Enactment) ile birebir hizalı. Doğru.

**Bridge `semantic-preservation` şartına karşı (ENS-4001 §Semantic Connectors, `specializes*`):**
Bridge, alt-türün üst-türün *tanımlayıcı özelliğini koruması*nı ister.
- `ReplenishmentOrder specializes Decision`: Decision'ın tanımlayıcı özelliği "commitment-mühürlü
  olmak". POSTED emir commitment-mühürlü → **korunuyor ✓.** (v0.2'de tek `Replenishment→Decision`
  düğümü, öneriyi de kapsadığı için bu özelliği KORUMUYORDU — SKR-028'in teşhisi doğruydu; bölünme
  tam da bunu düzeltiyor.)
- `ReplenishmentSuggestion specializes Claim`: Claim'in tanımlayıcı özelliği "karar gerekçesindeki
  önerme (Assertion)". Öneri, "Item-X NeededQty=Q gerekli" önermesidir → betimsel içerik korunuyor.
  Namespace da doğru: `ens-core:Claim` ≠ `ens-meta:Claim` (ENS-4010 §Homonim) bilinçle işaretlenmiş.

**Sonuç C.b: sağlıyor.** Bölünme hem Individuation'ı hem sem-preservation'ı gerçekten karşılıyor.
(Claim tarafının profil-conformance'ında bir *üst-akış* pürüzü var — aşağıda "ENS-4010 kusurları"nda;
bu bölünmenin doğruluğunu değil, ENS-4010'un profil tanımını ilgilendirir.)

## C.a teyidi — SupplierRelationship→Resource, Relation Registry'ye tutarlı mı?

SKR-028'in mekanik argümanını ENS-4010 satır 99'a karşı bağımsız doğruladım:
- Relation Registry: `requires: Decision/Capability → Resource` (domain {Decision, Capability},
  range {Resource}).
- Doğal domain kenarı: `ReplenishmentOrder requires SupplierRelationship`. `ReplenishmentOrder`
  = Decision → `requires` kaynağı geçerli (domain'de). Hedef **Resource olmalı**.
  - `SupplierRelationship: Resource` → **Allowed ✓.**
  - `SupplierRelationship: Capability` → hedef Capability olurdu, ama `requires` range'i yalnızca
    Resource → **Forbidden.** Yani v0.2 eşlemesi en doğal kenarı geçersiz kılıyordu.
- Tip sistemi doğruyu kendisi söylüyor: **Resource.** Bu türetilmiş kanıt, sezgi değil — teyit.
- Scarcity de sağlanıyor (Resource profili şartı): nitelikli tedarikçi kıttır; RFQ/SupplierSelection'ın
  varlık nedeni bu. ENS-4020 node tanımı "duran, kıt kaynak" diyor ✓.

**Sonuç C.a: sağlıyor, mekanik olarak zorunlu.** Zorlama değil; Registry'nin `requires`-range'i
eşlemenin doğrusunu dikte ediyor.

## İç kenarların Registry'ye karşı tam denetimi (bağımsız)
ENS-4020 §İç kenarlar'ın 6'sını da ENS-4010 Relation Registry'ye karşı tek tek denetledim:
| Kenar | Registry kuralı | Statü |
|-------|-----------------|-------|
| SupplierScorecard `supports` ReplenishmentSuggestion | supports: Cap/Evidence → Purpose/Claim; Evidence→Claim | Allowed ✓ |
| ReplenishmentOrder `owned_by` Buyer | owns: Actor→Decision (inv owned_by); Decision→Actor | Allowed ✓ |
| ReplenishmentOrder `constrained_by` ItemBinConfig/ApprovalRule | constrains: Constraint→Decision (inv) | Allowed ✓ |
| ReplenishmentOrder `has_context` StockBalance | has_context: Decision→Context 1:N | Allowed ✓ |
| ReplenishmentOrder `requires` SupplierRelationship | requires: Decision→Resource | Allowed ✓ |
| PurchaseOrder `requires` SupplierRelationship | requires: Decision→Resource | Allowed ✓ |
Hepsi Registry-içi, Allowed. Directionality/cardinality (Bridge N:1, bölünme sonrası hâlâ N:1)
tutarlı. Bridge completeness 10/10 deklare (v0.2'de 4). Semantic Closure node-başına deklare —
orphan node yok. **Bu boyutlarda ENS-4020 temiz.**

## ENS-4010 kusurları — SKR-028'in yukarı-bildirimi TEYİT + GENİŞLETİLDİ (blocking: ENS-4010)

### Kusur 1 (SKR-028 bildirdi) — Assertion profili ⊥ Relation Registry supports-domain: **GERÇEK, hem de daha geniş**
SKR-028: "Assertion profili 'Claim `supports` hedefi' der; ama Registry'de `supports` domain'i
Capability/Evidence (Claim değil) → Claim'in supports *kaynağı* olması Registry-dışı." Bağımsız
doğrulama (ENS-4010):
- Assertion profili (satır 83): zorunlu = "`supports`|`invalidates` hedefi" → **Evidence, Claim,
  Learning** node'larına uygulanır.
- `supports` (satır 95): domain = {Capability, Evidence}. Claim yok → `Claim supports X` **Forbidden**.
- `invalidates` (satır 96): domain = {Evidence}. Claim yok → `Claim invalidates X` **Forbidden**.
- **Sonuç:** Bir `Claim` node'u, Assertion profilinin zorunlu çıkan-kenarını (`supports`|`invalidates`)
  Registry ile **hiçbir biçimde sağlayamaz**. Bu, ENS-4010'un içinde gerçek bir çelişkidir.
- **Genişletme (SKR-028'in görmediği):** Aynı sorun `Learning` node'unu da vurur — Learning de
  Assertion profilinde, ama tek çıkan-kenarı `updates: Learning → Memory` (satır 105); ne `supports`
  ne `invalidates` domain'inde Learning var. Yani Assertion profili, üç node'undan **ikisi (Claim,
  Learning) için Registry-ile-tatmin-edilemez**; yalnızca Evidence sağlayabiliyor. Bu, "yukarı-
  bildirim" değil, sistematik bir profil↔registry hizalama borcudur.
- ENS-4020 nasıl baypas etti: `ReplenishmentSuggestion`'ı `supported_by Evidence` (SupplierScorecard,
  Evidence→Claim, Registry-içi) ile bağladı ve closure'ı *gelen* kenarla kurdu. Ama Assertion profili
  *çıkan* `supports`|`invalidates` ister; `supported_by` (gelen) bunu literal olarak karşılamaz. Yani
  `ReplenishmentSuggestion` profil-conformance'ı ENS-4010 çelişkisi yüzünden **eksik kalıyor** — ama
  bu ENS-4020'nin modelleme hatası değil; ENS-4010'un çözemediği çelişkidir. **ENS-4020 en doğru
  specialization'ı (Claim) yaptı; kırık zemini o kırmadı, ilk instantiate eden olarak açığa çıkardı.**

### Kusur 2 (bu turun yeni bulgusu) — Resource profili ⊥ `required_by`
- Resource profili (satır 85): zorunlu = "`consumed_by`|`supports`, scarcity".
- `SupplierRelationship` (Resource) beyan edilen kenarı: `required_by ReplenishmentOrder/PurchaseOrder`
  (yani `requires`'ın inverse'ü). Closure da `required_by` üzerinden kuruluyor.
- `required_by` ∉ {`consumed_by`, `supports`}. Yani SupplierRelationship, Resource profilinin
  *literal* zorunlu-kenarını **sağlamıyor**.
- Bu semantik olarak doğru bir gerilim: SupplierRelationship *duran/required* bir kaynaktır, sipariş
  başına *consumed* (tüketilen) değil. `required_by`, `consumed_by`'dan daha doğru — ama Resource
  profilinin zorunlu-listesi `required_by`'ı içermiyor. **Kusur 1 ile aynı tür: ENS-4010 Semantic
  Profiles'ın zorunlu-kenar listeleri, Relation Registry'nin o node-tipleri için lisansladığı
  kenarları tam kapsamıyor.** İki bağımsız örnek → sistematik borç.
- Yine: ENS-4020'nin specialization'ı (Resource) *doğru ve mekanik olarak zorunlu* (C.a); pürüz
  ENS-4010'un Resource profil tanımında.

**Neden M2'yi bloke etmez:** Her iki kusur da ENS-4020'nin *modelleme kararından* değil, ENS-4010'un
profil↔registry iç-tutarsızlığından doğuyor. ENS-4020 iki node'u da tip-sistemi tarafından *zorunlu
kılınan* doğru core-tipine bağladı (Claim, Resource). Bir üst-akış (ENS-4010) hatası yüzünden
ENS-4020'yi M1'de rehin tutmak yanlış atıf olur. SKR-028 kendi mantığında Kusur 1'i "non-blocking,
yukarı-bildirim" saymış ve M1'i *yalnızca* kendi-düzeltme-teyidi eksikliğinden (G2) vermişti — o
teyit şimdi sağlandı. Doğru aksiyon: **ENS-4020 → M2**, ENS-4010'a karşı iki confirmed kusur açık.

## Kalan riskler (Faz-4 / owner — SKR-028'den, teyit)
- **OF1 — StockBalance: Context mi State mi?** Envanter miktarı ontolojik State; şu an Context
  (ADR-0002 §5.1 kullanımıyla uyumlu). State'e taşıma daha temiz — non-blocking, Faz-4.
- **OF2 — DemandForecast (Evidence) eksik.** Confidence-üretici node yok; ReplenishmentSuggestion'ın
  Confidence'ı kalibre edilemez. F condition; Faz-4.
- **Constraint-bundle aggregation (ADR-0001 D3 çekincesi):** `ApprovalRule` "policy bundle üyesi"
  diye anılıyor ama ENS-4010/4020'de bir *aggregation* relation'ı yok. ADR-0001 SKR-026/SKR-029
  çekincesi burada da açık kalıyor — non-blocking, ENS-4010 borcu.
- **Öneri/emir senkron bakımı** (SKR-028 OF): Suggestion↔Order izlenebilirlik bağı Faz-4 KG'de
  tutulmalı, yoksa kopar.

## Downstream yükümlülük (ADR-0002 — owner ens-ai-architect)
ENS-4020 v0.3 "Replenishment node" atfını iki node'a çözüyor; ADR-0002 §5.1 (Reasoning) →
`ReplenishmentSuggestion`, §5.2 (commitment) → `ReplenishmentOrder`. ADR-0002 bir sonraki
revizyonunda iki adı ayrı referans vermeli. (Bu turda ADR-0002'yi düzenlemedim — owner işi.)

## Sahibine talepler
**ENS-4020 (M2 verildi — bunlar Faz-4 rafinman):** OF1/OF2'yi Faz-4'te kapat; öneri/emir bağını
KG'de tut.
**ENS-4010 (owner: ens-philosopher — blocking, ENS-4010'un kendi ilerlemesi için):**
1. **Kusur 1:** Assertion profili ⊥ supports/invalidates domain'i. Ya `supports` domain'ine `Claim`
   ekle (Claim bir Decision'ı destekleyebilir — ENS-2001 Reasoning'de premise), ya da Assertion
   profilinin zorunlu-kenarını Claim/Learning için gevşet (gelen `supported_by`/`updates` de sayılsın).
   Aksi hâlde `ens-core:Claim` ve `ens-core:Learning` **hiçbir** conformant instance üretemez.
2. **Kusur 2:** Resource profilinin zorunlu-kenar listesine `required_by`'ı ekle (duran/required
   kaynaklar için), ya da listeyi "en az bir kaynak-dokunan kenar (consumed_by|supports|required_by)"
   olarak genelle.
3. Bu iki örnek, ENS-4010'da **sistematik bir profil↔registry hizalama denetimi** gerektiğine işaret
   ediyor — formal-checker (G-09/10) yazıldığında bu invariant ("her profil zorunlu-kenarı, o node'un
   Registry-domain'inde gerçekten var mı?") ilk testlerden olmalı.

## Sonuç
ENS-4020 **survives → M2**. C.a/C.b düzeltmeleri bağımsız teyitten sağ çıkıyor; iki bağımsız
validator (SKR-028 + SKR-030) G4'ü karşılıyor. ENS-4020'ye özgü modelleme hatası yok. İki
profil↔registry çelişkisi **ENS-4010'un borcudur** (biri SKR-028'den teyit + genişletildi, biri
bu turda yeni) ve ENS-4010'a karşı açık, takip edilen kusur olarak kaydedilir — ENS-4020'yi bloke
etmez. Statü `review` kalır; `ratified`'a terfi ayrı bir governance edimidir (owner/GOV), skeptic
yalnızca maturity'yi (G4 → M2) önerir.

---

*Enterprise Ontology'nin bu turdaki dersi SKR-028'in son cümlesini doğruluyor: iyi ontoloji kendi
kenarlarıyla kendini düzeltir — ve doğru instantiate edildiğinde, altındaki temelin (ENS-4010)
gizli çatlaklarını da görünür kılar. ENS-4020 M2'yi hak ediyor; ama miras olarak ENS-4010'a iki
confirmed profil↔registry çelişkisi bırakıyor.*
