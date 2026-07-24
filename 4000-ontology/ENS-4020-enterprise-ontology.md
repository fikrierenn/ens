---
id: ENS-4020
title: Enterprise Ontology (operax — Operations domain)
type: ontology
canon: false
origin: ENS-4010 §İki katman, ENS-4001 §Semantic Connectors
depends_on: [ENS-0000, ENS-4001, ENS-4010]
referenced_by: [ADR-0002]
principles: [P1, P2]
status: review
owner: ens-ai-architect
version: 0.3.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: [SKR-028, SKR-030]
maturity: M2
evidence: {sci: E1, eng: E0, ops: E1, econ: E0}
requires: [ENS-4010]
provides: [ens-ent namespace instances — Operations domain]
consumed_by: [ADR-0002]
---

# ENS Enterprise Ontology — Operations Domain (`ens-ent:`)

> ENS-4010'un rezerve ettiği `ens-ent:` namespace'in **ilk dolduruluşu.** ADR-0002 (Operations
> Capability Pack) operax terimlerini şimdiye dek **düz-yazı tabloyla** ENS'e bağlıyordu —
> resmî değildi (Node Registry'de kayıtsız, Bridge kenarı deklare edilmemiş). Bu belge SSOT
> ihlalini kapatır: her operax kavramı burada **tipli bir `ens-ent:` node'u** ve **Bridge kenarı**
> olarak kayıtlıdır. `canon: false`.
>
> **v0.3 notu ([SKR-028](reviews/SKR-028-enterprise-ontology.md) — ilk Ontology Validation):**
> İki yapısal Bridge kusuru giderildi. (1) `Replenishment` **iki node'a bölündü** —
> `ReplenishmentSuggestion specializes ens-core:Claim` (tvf-önerisi = deliberation, atom değil) ve
> `ReplenishmentOrder specializes ens-core:Decision` (POSTED/onaylı commitment = atom). Böylece
> ENS-2001 §Individuation ve ADR-0002 §5.2 DRAFT/POSTED sınırı ontolojiye mühürlendi; Bridge'in
> `semantic-preservation` şartı artık korunuyor. (2) `SupplierRelationship specializes
> ens-core:Resource` (Capability değil — tedarikçi ilişkisi bir *yeti* değil, Procure yetisinin
> dayandığı *duran kaynaktır*; `requires: Decision→Resource` kenarı ancak böyle Allowed olur).
> Ek: 10 node'un tamamı için Bridge kenarı ve node-başına Semantic Closure deklare edildi.
> Verdict: **wounded → v0.3'te giderildi**; M2 için bağımsız teyit turu bekliyor (M1).

## Namespace kuralı (ENS-4010'dan miras)
Her `ens-ent:` node, tam olarak bir `ens-core:` node'unu **specializes** eder (Bridge connector,
ENS-4001 §Semantic Connectors, N:1). Domain-özel alanlar node'un kendi şemasında durur; core
node yalnızca *tip* ilişkisini bilir.

## Node Registry — Operations domain (operax)

> **v0.3 kritik ayrım (SKR-028 C.b):** operax'ın ikmal akışı *tek* bir ENS node'u değildir. tvf
> çıktısı bir **öneri** (deliberation → `Claim`), commit-mühürlü POSTED emir ise **atom**
> (`Decision`). ENS-2001 §Individuation gereği bunlar ayrı node'lardır; birini diğerine karıştırmak
> atomu deliberation'a çökertir (terminoloji sürüklenmesi).

| `ens-ent:` node | `specializes` (`ens-core:`) | Definition | Kaynak (operax) |
|-----------------|------------------------------|------------|------------------|
| `ReplenishmentSuggestion` | `Claim` | tvf'nin ürettiği ikmal *önerisi* = karar gerekçesindeki önerme ("Item-X, NeededQty=Q gerekli"). **Deliberation/Reasoning fazı — atom değil.** `ens-core:Claim` (≠ `ens-meta:Claim`) | `tvf_ReplenishmentSuggestions` |
| `ReplenishmentOrder` | `Decision` | Önerinin insan tarafından commit edildiği ikmal *emri* (bin-to-bin transfer ya da PO). **POSTED/onaylı = commitment atomu** | `Replenishment.cshtml.cs::OnPostCreateTransferAsync`; DRAFT→POSTED |
| `PurchaseOrder` | `Decision` | Tedarikçiye verilen sipariş kararı (procure-seviyesi commitment; ENS-2001 §4 recursion ile ReplenishmentOrder'ın alt-kararı olabilir) | M03 Purchasing |
| `PriceVarianceApproval` | `Decision` | Fiyat sapması onay kararı (commitment) | M03.P2 |
| `SupplierSelection` | `Decision` | RFQ üzerinden tedarikçi seçim kararı (commitment) | M03.R1/R2 |
| `ItemBinConfig` | `Constraint` | Bir SKU/bin için MinQty/MaxQty eşiği | `ItemBinConfig` tablosu |
| `ApprovalRule` | `Constraint` | Tutar/rol bazlı onay eşiği (policy bundle üyesi, bkz. ADR-0001 §5.6) | `ApprovalRule` tablosu |
| `SupplierScorecard` | `Evidence` | Tedarikçi güvenilirlik kanıtı (on-time %, defect %); `supports` ReplenishmentSuggestion | tedarikçi skorkartı |
| `StockBalance` | `Context` | Bir SKU'nun güncel envanter durumu; karara *ilgili* durum (OF1: State'e taşınabilir) | `InventoryBalance` |
| `Buyer` | `Actor` | Satınalma kararlarından sorumlu insan (P7) | kullanıcı/rol |
| `SupplierRelationship` | `Resource` | Bir tedarikçiyle sipariş verme *kanalı/varlığı* — Procure yetisinin dayandığı duran, kıt kaynak; `required_by` ReplenishmentOrder/PurchaseOrder. **Capability değil** (SKR-028 C.a) | tedarikçi kaydı |

## Bridge kenarları (v0.3 — 10/10 deklare, hepsi N:1, sem-preserving, trace zorunlu)
Her `ens-ent:` node için `specializes*` Bridge kenarı (ENS-4001 §Semantic Connectors) açıkça
deklare edilir — tanımsız bridge kullanan inference geçersizdir.
```
ens-ent:ReplenishmentSuggestion  specializes  ens-core:Claim        (N:1)
ens-ent:ReplenishmentOrder       specializes  ens-core:Decision     (N:1)
ens-ent:PurchaseOrder            specializes  ens-core:Decision     (N:1)
ens-ent:PriceVarianceApproval    specializes  ens-core:Decision     (N:1)
ens-ent:SupplierSelection        specializes  ens-core:Decision     (N:1)
ens-ent:ItemBinConfig            specializes  ens-core:Constraint   (N:1)
ens-ent:ApprovalRule             specializes  ens-core:Constraint   (N:1)
ens-ent:SupplierScorecard        specializes  ens-core:Evidence     (N:1)
ens-ent:StockBalance             specializes  ens-core:Context      (N:1)
ens-ent:Buyer                    specializes  ens-core:Actor        (N:1)
ens-ent:SupplierRelationship     specializes  ens-core:Resource     (N:1)
```

## İç kenarlar (ens-ent içi — hepsi ENS-4010 Relation Registry-içi doğrulandı)
```
ens-ent:SupplierScorecard  supports        ens-ent:ReplenishmentSuggestion   (Evidence → Claim ✓)
ens-ent:ReplenishmentOrder owned_by        ens-ent:Buyer                     (Decision → Actor ✓)
ens-ent:ReplenishmentOrder constrained_by  ens-ent:ItemBinConfig, ApprovalRule (Constraint → Decision ✓)
ens-ent:ReplenishmentOrder has_context     ens-ent:StockBalance              (Decision → Context ✓)
ens-ent:ReplenishmentOrder requires        ens-ent:SupplierRelationship      (Decision → Resource ✓)
ens-ent:PurchaseOrder      requires        ens-ent:SupplierRelationship      (Decision → Resource ✓)
```
> **Neden Resource, Capability değil (SKR-028 C.a):** `requires`in range'i (ENS-4010) yalnızca
> `Resource`tır. `SupplierRelationship: Capability` olsaydı, en doğal kenar olan `ReplenishmentOrder
> requires SupplierRelationship` **Forbidden** olurdu. Tip sistemi eşlemenin doğrusunu kendisi söyler.

## Yaşam-döngüsü (öneri ≠ atom, ENS-2001 lifecycle ile hizalı)
```
SupplierScorecard(Evidence) + StockBalance(Context)
   → ReplenishmentSuggestion(Claim)         [Reasoning fazı — deliberation, ATOM DEĞİL]
   → [insan commit eder: DRAFT → POSTED / ApprovalLog:APPROVED]  ← ATOM SINIRI (P7)
   → ReplenishmentOrder(Decision)           [commitment atomu]
   → PurchaseOrder(Decision) | bin-to-bin transfer   [enactment / alt-karar]
```

## Semantic Closure (node-başına — her node bir Principle'a ulaşır)
| ens-ent node | core | Principle kapanışı |
|--------------|------|--------------------|
| ReplenishmentSuggestion | Claim | `supported_by` Evidence → Reasoning → Decision → **P1, P2** |
| ReplenishmentOrder | Decision | **P1, P2, P4, P7** (ENS-2001) |
| PurchaseOrder / PriceVarianceApproval / SupplierSelection | Decision | **P1, P2, P4, P7** |
| ItemBinConfig / ApprovalRule | Constraint | `constrains` Decision → **P1, P2** |
| SupplierScorecard | Evidence | `supports` Claim → Decision → **P2, P6** |
| StockBalance | Context | **P2** (ENS-2002) |
| Buyer | Actor | **P7** |
| SupplierRelationship | Resource | `required_by` Decision → **P1** |

Orphan node yok — her `ens-ent:` node tipli-kenar traversal'ıyla en az bir Principle'a ulaşır.

## ADR-0002'ye geri-bağ (SKR-025 Bulgu 3'ün de ön-koşulu)
ADR-0002 §5.1 ve §4 tablolarındaki operax terimleri artık burayı **referans verir**, yeniden
tanımlamaz. **v0.3 downstream yükümlülük:** ADR-0002'nin "Replenishment node" atıfları artık *iki*
node'a çözülür — ADR-0002 §5.1 (Reasoning/öneri) `ReplenishmentSuggestion`'a, ADR-0002 §5.2
(commitment atomu) `ReplenishmentOrder`'a karşılık gelir. ADR-0002 bir sonraki revizyonunda bu iki
adı ayrı referans vermeli (bu, ADR §5.2'nin zaten yaptığı öneri≠atom ayrımını ontolojiyle hizalar).

`Confidence` alanının operax'ta üretilemediği (SKR-025 Bulgu 3) burada da doğrulanır:
`ReplenishmentSuggestion` node'unun hiçbir alanı bir confidence-üretici ilişki taşımıyor — bu,
gelecekteki bir `ens-ent:DemandForecast specializes ens-core:Evidence` node'unun eksikliğine işaret
eder (failure condition; SKR-028 OF2).

## Failure conditions (Anayasa Madde X)
- **Tek-domain darlığı.** Bu belge yalnızca Operations'ı (operax) kapsar; reporthub/brain gibi
  başka Pack'ler kendi `ens-ent:` genişlemelerini gerektirecek — ortak bir üst-taksonomi
  (`ens-ent:` içinde domain-alt-namespace) olmadan node çakışması riski var.
- **Specialization tekilliği zorlanmadı.** Bir `ens-ent:` node'un yalnızca *bir* `ens-core:`
  node'u specialize ettiği varsayılıyor (N:1); çoklu-miras gerekirse şema genişlemeli.
- **Confidence-üretici node eksik** (yukarıda not edildi) — SKR-025 Bulgu 3'ün ontoloji tarafı.
- **Öneri/emir bölmesinin bakım maliyeti (v0.3, SKR-028 C.b).** Suggestion(Claim) ↔ Order(Decision)
  ayrımı doğru ama iki node'u senkron tutmayı gerektirir; hangi Suggestion hangi Order'a commit
  edildi ilişkisi (izlenebilirlik) Faz-4 KG'de tutulmalı, yoksa öneri-emir bağı kopar.
- **StockBalance: Context mi State mi?** (SKR-028 OF1) Envanter miktarı ontolojik olarak bir State;
  şu an Context'te (ADR-0002 §5.1 kullanımıyla uyumlu). State'e taşıma daha temiz olabilir — Faz-4.

---

*Enterprise Ontology, domain terimlerinin ENS çekirdeğine sızmadan, resmî Bridge kenarlarıyla
bağlandığı yerdir. operax artık düz-yazıyla değil, tipli node'larla ENS'e takılıdır.*
