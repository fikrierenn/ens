---
id: SKR-028
type: skeptic-review
validation_dimension: ontology
origin: ENS-4020
depends_on: [ENS-4020, ENS-4010, SKR-027]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-028 — Enterprise Ontology (ENS-4020, operax) Ontology Validation

> **Bağımsızlık beyanı:** Bu, ENS-4020'nin **ilk** bağımsız Ontology Validation'ıdır — SKR-027
> Bulgu C'nin (blocking) talep ettiği tur. SKR-027 ADR-0002'yi (engineering boyut) sınarken
> ENS-4020'nin hiç semantik-şema doğrulamasından geçmediğini ve iki tartışmalı `specializes`
> eşlemesi taşıdığını işaretledi. Bu kayıt yalnızca ENS-4020'yi, ENS-4010 (Semantic Type System)
> ve ENS-4001 (Bridge connector) ölçütlerine karşı sıfırdan doğrular. Boyut: **ontology** (node/
> relation completeness, template completeness, directionality/cardinality, namespace/Bridge,
> Semantic Closure) — Scientific değil.

## Verdict
**wounded → v0.3'te giderildi, tekrar-bekleyen (M1).** İncelenen v0.2, iki **yapısal Bridge
kusuru** taşıyordu: (C.b) `Replenishment specializes ens-core:Decision` — tek node, hem tvf-önerisini
(deliberation) hem POSTED-emri (commitment) kapsayarak ENS-2001 §Individuation'ı ve Bridge'in
`semantic-preservation` şartını ihlal ediyor; (C.a) `SupplierRelationship specializes ens-core:Capability`
— zorlama eşleme, üstelik doğal `requires` kenarını Forbidden yapıyor. Ek olarak **template/Bridge
completeness** eksik (10 node'un yalnızca 4'ünün Bridge kenarı deklare, closure node-başına yok).
Bunlar *fixable* → refuted değil. v0.3'te (bu turda) çözdüm; ama kendi revizyonum bağımsız bir
teyit turundan (SKR-020'nin ENS-4010'a yaptığı gibi) geçmediğinden **M2 vermiyorum — M1, tekrar
bekleyen.** Döngüsel bağımlılık (SKR-027 Bulgu D) `depends_on`'da zaten kırılmış — teyit edildi.

## Yenilik incelemesi
ENS-4020 yeni bir *kavram* iddia etmez; ENS-4010'un `ens-ent:` namespace'ini operax domain'iyle
**doldurur** (specialization örnekleri). Prior-art konumu SKR-027'de doğrulandı: (s,S) politikası
(Arrow-Harris-Marschak 1951), EOQ (Harris 1913), ERP/MRP replenishment. ENS'in ontolojik katkısı
domain terimlerini çekirdeğe *sızdırmadan*, tipli Bridge kenarlarıyla bağlamaktır (REA / RDFS
`subClassOf` deseninin uygulaması — özgün değil, dürüstçe böyle konumlanmış). İcat iddiası yok;
Madde VI temiz. Doğrulanacak olan **özgünlük değil, tip-sistemi tutarlılığıdır.**

## Node / Relation completeness
| ens-ent node | v0.2 specializes | Değerlendirme |
|--------------|------------------|---------------|
| Replenishment | Decision | **KUSUR (C.b)** — öneri+emir tek node'da; bölünmeli |
| PurchaseOrder | Decision | Geçer (POSTED emir = commitment atomu; ENS-2001 §4 recursion ile ReplenishmentOrder'ın alt-kararı olabilir — OQ3) |
| PriceVarianceApproval | Decision | Geçer (onay = commitment) |
| SupplierSelection | Decision | Geçer (RFQ seçim commitment'ı) |
| ItemBinConfig | Constraint | Geçer (min/max eşik = kural) |
| ApprovalRule | Constraint | Geçer (tutar/rol eşiği = kural) |
| SupplierScorecard | Evidence | Geçer (on-time%/defect% = Claim'i destekleyen bilgi) |
| StockBalance | Context | Geçer (sınırda — aşağı bkz. OF1: State daha temiz olabilir) |
| Buyer | Actor | Geçer |
| SupplierRelationship | Capability | **KUSUR (C.a)** — Resource olmalı |

**Bridge completeness eksik:** §Kenar örnekleri yalnızca 4 node'un (`Replenishment`,
`ItemBinConfig`, `ApprovalRule`, `Buyer`) `specializes` kenarını deklare ediyor. 6 node'un
(`PurchaseOrder`, `PriceVarianceApproval`, `SupplierSelection`, `SupplierScorecard`, `StockBalance`,
`SupplierRelationship`) Bridge kenarı deklare edilmemiş — ENS-4001 "tanımsız bridge kullanan
inference geçersiz" der; her ent node'un Bridge'i **açıkça** deklare olmalı.

**Template completeness:** ENS-4010 7-parça şablon yerine Semantic Profile kullanır ve her ent
node core node'unun profilini *miras alır*. Bu meşru; ama ENS-4020 node'ları (i) hangi core-node
profiline oturduklarını, (ii) domain-özel Allowed/Forbidden kenarlarını, (iii) Counter-Example'ı
listelemiyor. Node tablosu = şablonun kısaltılmış hâli; en azından profil-mirası + anahtar domain
kenarı + karşı-örnek eklenmeli (v0.3'te kısmen kapatıldı).

## SKR-027 Bulgu C'nin çözümü (iki eşleme)

### C.b — Replenishment → Decision: **iki node'a bölündü**
ENS-2001 §Individuation kesin: atom = *commitment-mühürlü* karar; deliberation/öneri atom değildir.
ADR-0002 §5.2 bunu prose'da zaten söylüyor ("tvf satırı öneri, ATOM DEĞİL; atom = DRAFT→POSTED").
Ama ENS-4020 v0.2 **tek** `Replenishment` node'unu `Decision`'a bağlayarak bu ayrımı ontolojiden
sildi — terminoloji sürüklenmesi. Bridge connector'un `semantic-preservation: ✓` şartı (ENS-4001)
burada *ihlal*: alt-tür (öneri) üst-türün (Decision) tanımlayıcı özelliğini (commitment-mühürlü
olmak) **korumuyor**. Bu tam da Ontology Validation'ın yakalaması gereken iç tutarsızlık.

**Çözüm (v0.3):**
- `ens-ent:ReplenishmentSuggestion specializes ens-core:Claim` — tvf-önerisi = karar gerekçesindeki
  önerme (Assertion profili). Deliberation/Reasoning fazının çıktısı; **atom değil.** Namespace
  dikkati: `ens-core:Claim` (karar-gerekçesi önermesi) ≠ `ens-meta:Claim` (ENS-teori önermesi) —
  ENS-4010 §Homonim uyarısı bilinçle uygulandı.
- `ens-ent:ReplenishmentOrder specializes ens-core:Decision` — POSTED/APPROVED commitment = atom.
  `owned_by Buyer`, `constrained_by ItemBinConfig/ApprovalRule`, `has_context StockBalance`.

Yaşam-döngüsü zinciri (ENS-2001 lifecycle ile birebir): `SupplierScorecard/StockBalance (Evidence/
Context)` → `ReplenishmentSuggestion (Claim, Reasoning)` → **commitment** → `ReplenishmentOrder
(Decision, atom)`. Bölme, ADR-0002 §5.2 DRAFT/POSTED sınırını ontolojiye *mühürler*.

### C.a — SupplierRelationship → Capability: **Resource'a yeniden eşlendi**
`ens-core:Capability` = "amaca hizmet edebilen örgütsel *yeti*" (Procure, Replenish, Receive gibi
fiil-benzeri yetiler; ADR-0002 §4'te zaten node'lar). "Tedarikçi X ile ilişki" bir *yeti* değil,
Procure yetisinin *dayandığı duran varlıktır* — sahip olunan bir kanal/kaynak. Her tedarikçi
ilişkisini ayrı bir örgütsel Capability saymak yanlış granülerliktir (yeti ≠ varlık).

**Mekanik kanıt (tip-sistemi türevli, sezgi değil):** ENS-4010 Relation Registry'de
`requires: Decision/Capability → Resource`. Doğal domain kenarı `ReplenishmentOrder requires
SupplierRelationship` (sipariş vermek için tedarikçi ilişkisi gerekir):
- `SupplierRelationship: Capability` ise → kenarın range'i Capability → **Forbidden** (range Resource
  olmalı). Yani zorlama eşleme, en doğal kenarı geçersiz kılıyor.
- `SupplierRelationship: Resource` ise → **Allowed.** Scarcity de sağlanır (nitelikli tedarikçi
  kıttır; RFQ/SupplierSelection'ın varlık nedeni budur — Resource profili `scarcity` şartı).

**Çözüm (v0.3):** `ens-ent:SupplierRelationship specializes ens-core:Resource`.
`required_by ReplenishmentOrder/PurchaseOrder`. (İleride tedarikçiyi *ajan* olarak modellemek
isteyen bir Pack `ens-ent:Supplier specializes ens-core:Organization` ekleyebilir — v0.3'te
gerekli değil; kapsam dışı, RFC ile.)

## Directionality / Cardinality
- Bridge (`specializes*`) yön ent→core, mult **N:1** — v0.2/v0.3 uyuyor (her ent node tam bir core
  node specialize eder). Bölme sonrası hâlâ N:1 (Suggestion→Claim, Order→Decision ayrı N:1).
- İç kenarlar (v0.3, hepsi Registry-içi doğrulandı):
  `ReplenishmentOrder owned_by Buyer` (Decision→Actor ✓), `constrained_by ItemBinConfig/ApprovalRule`
  (Constraint→Decision ✓), `has_context StockBalance` (Decision→Context 1:N ✓),
  `requires SupplierRelationship` (Decision→Resource ✓), `SupplierScorecard supports
  ReplenishmentSuggestion` (Evidence→Claim ✓).

## Namespace / Bridge doğruluğu
- Namespace ayrımı doğru: tüm domain node'ları `ens-ent:`, hepsi `ens-core:`e specialize eder,
  hiçbiri `ens-meta:`e sızmaz. Homonim tehlikesi (`Claim`) v0.3'te namespace-net işaretlendi.
- Bridge tip-sistemi (ENS-4001 §Semantic Connectors): `specializes*` = ent→core, N:1,
  sem-preserve ✓, trace zorunlu. v0.3'te **10 node'un tamamı** için Bridge kenarı deklare edildi
  (v0.2'de 4). C.b/C.a düzeltmeleriyle sem-preservation artık her kenarda gerçekten korunuyor.

## Semantic Closure
Her ent node bir Principle'a ulaşıyor mu (core üzerinden)?
- ReplenishmentOrder → Decision → P1/P2/P4/P7 ✓
- ReplenishmentSuggestion → Claim → (`supported_by` Evidence; Reasoning fazı) → Decision → P1 ✓
- ItemBinConfig/ApprovalRule → Constraint → `constrains` Decision → P1/P2 ✓
- SupplierScorecard → Evidence → `supports` Claim → Decision → P2/P6 ✓
- StockBalance → Context → P2 ✓
- Buyer → Actor → P7 ✓
- SupplierRelationship → Resource → `required_by` Decision → P1 ✓ (Capability iken kapanış da
  zorlamaydı; Resource ile doğrudan.)
Orphan yok. Ama v0.2 kapanışı *node-başına deklare etmiyordu* (SKR-019 Bulgu 5 ENS-4010'da bunu
şart koşmuştu) — v0.3 bir kapanış özeti ekledi.

## İç tutarlılık
- **Çözülen tek gerçek drift:** `Replenishment→Decision` (v0.2) ↔ ENS-2001 §Individuation / ADR-0002
  §5.2 gerilimi — SKR-027 Bulgu C.b + §İç tutarlılık'ta işaretlenmişti. v0.3 bölmesi kapatıyor.
- **Yüzeye çıkan ENS-4010 iç-gerilimi (yukarı-bildirim, blocking değil):** Assertion profili
  "Claim `supports` hedefi" der; ama Relation Registry'de `supports` domain'i *Capability/Evidence*
  (Claim değil). Yani bir Claim'in `supports` *kaynağı* olması Registry-dışı. v0.3, Suggestion'ı
  `supported_by Evidence` (Registry-içi, Evidence→Claim) ile bağlayarak bunu baypas etti; ama
  ENS-4010 sahibi profil↔registry'yi (Claim supports-kaynağı olabilir mi?) uzlaştırmalı. Bu
  ENS-4020 değil ENS-4010 borcu — enterprise instantiation'ın foundational'da açığa çıkardığı kusur.
- Döngüsel bağımlılık (SKR-027 Bulgu D): `ENS-4020.depends_on = [ENS-0000, ENS-4001, ENS-4010]` —
  ADR-0002 çıkarılmış, döngü kırık. `referenced_by/consumed_by: [ADR-0002]` yön-doğru (ADR ontolojiyi
  tüketir). **Teyit: Linter circular-dependency hatası artık yok.**

## Kalan riskler (Faz-4 / owner devir)
- **OF1 — StockBalance: Context mi State mi?** Envanter miktarı aslında bir *State* (varlığın
  zamandaki durumu); replenishment kararının *Context'inin parçası* olur. İkisi de Temporal profil;
  v0.3 Context'te bıraktı (ADR-0002 §5.1 kullanımıyla uyumlu) ama State'e taşıma daha temiz olabilir.
  Non-blocking, Faz-4 rafinman.
- **OF2 — DemandForecast (Evidence) eksik.** SKR-025 Bulgu 3 / SKR-027: Confidence-üretici node yok;
  `ens-ent:DemandForecast specializes ens-core:Evidence` ileride gerekli (F condition). ReplenishmentSuggestion'ın
  Confidence'ı bu eksik node olmadan kalibre edilemez.
- **OF3 — OQ3 granülerliği:** ReplenishmentOrder ↔ PurchaseOrder örtüşmesi (ENS-2001 §4 recursion
  ile iki karar-seviyesi olarak meşru; ama `Replenish-Transfer` vs `Replenish-Purchase` ayrımı
  Capability granülerliğiyle birlikte netleşmeli).
- **OF4 — Template tamlığı Profile-mirası + Linter'a bağlı** (ENS-4010'un OF3 mirası); formal-checker
  olmadan Bridge deklarasyonu disipline dayanır.

## Downstream yükümlülük (ADR-0002)
ADR-0002 §Uyumlaştırma/§4/§5 "Replenishment node"a atıf yapıyor; v0.3'te bu **iki** node'a
(`ReplenishmentSuggestion` = öneri/§5.1-Reasoning, `ReplenishmentOrder` = §5.2-commitment atomu)
çözülüyor. ADR-0002 bir sonraki revizyonunda bu iki adı ayrı ayrı referans vermeli — bu, ADR'nin
§5.2'de zaten yaptığı prose-ayrımını ontolojiyle hizalar (ADR'yi bu turda düzenlemedim; owner işi).

## Sahibine talepler (M2 için — teyit turu)
1. v0.3'ün `ReplenishmentSuggestion/Order` bölmesi ve `SupplierRelationship→Resource` remap'i
   **bağımsız bir teyit turundan** geçsin (SKR-020'nin ENS-4010'a yaptığı gibi) → o zaman M2.
2. Bridge deklarasyonlarının 10/10 tamlığı ve node-başına closure formal-checker'la doğrulansın.
3. OF2 (DemandForecast=Evidence) node'unu ekle — Confidence/learning bacağının ontoloji zemini.
4. ADR-0002'yi iki-node ayrımına göre güncelle (downstream, owner ADR-0002).

---

*Enterprise Ontology'nin en sinsi kusuru orphan değil, **sessiz specialization**tı: bir öneriyi
bir Decision'a bağlayıp atomu deliberation'a çökertmek. Bridge'in `semantic-preservation` şartı
tam da bunun içindir — ve tip sistemi, `requires`in range'i üzerinden, SupplierRelationship'in
Capability değil Resource olduğunu bize kendisi söyledi. İyi ontoloji, kendi kenarlarıyla kendini
düzeltir.*
