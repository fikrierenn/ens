---
id: ENS-4025
title: Semantic Logic
type: ontology
canon: false
constitutive: false
origin: ENS-4010, ENS-4030
depends_on: [ENS-0000, ENS-4010, ENS-4030]
referenced_by: [ENS-2004, ENS-4031, ADR-0001, ADR-0002]
principles: [P6, P2]
status: ratified
owner: ens-philosopher
version: 0.1.1
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-022  # + SKR-031 D-1 düzeltmesi (v0.1.1): proof-trace örneği Registry'ye hizalandı
maturity: M2
evidence: {sci: E3, eng: E0, ops: E0, econ: E0}
---

# ENS Semantic Logic

> Çıkarım motorunun **semantik sözleşmesi** — algoritma değil. Inference Rules (ENS-4031) ve
> gelecekteki reasoning motoru bu sözleşmenin *üstüne* kurulur; hiçbir katman örtük mantık
> varsayımına dayanmaz. `canon: false` (M1).

## Beş-başlık (bundan sonraki disiplin)
- **Prior art:** Description Logic / OWL (OWA, monotonic), Datalog (CWA, negation-as-failure),
  SHACL (open-world graf üstünde closed-world validation), RDF/OWL, olasılıksal mantık
  (Markov Logic Networks, ProbLog — confidence), Truth Maintenance / justification trees
  (proof trace).
- **Delta:** ENS bunları icat etmez; **belirli bir bileşimi** taahhüt eder — OWA-inference +
  local-CWA-validation + monotonic-temporal + **confidence-propagation** + **zorunlu proof-trace**,
  hepsi P6'ya (explainability) bağlı. Bileşim ve P6-bağı ENS'e özgü.
- **Neden yeni:** Inference Rules yazmak bir mantık sistemi *varsayar*; bu varsayımı örtük
  bırakmak, yarının tooling'ini (Datalog mı? DL mi?) bugün gizlice kısıtlar.
- **Üst katman:** Semantic Axioms (ENS-4030), Type System (ENS-4010).
- **Alt katman:** Inference Rules (ENS-4031), Validation Rules, reasoning motoru (Faz 4-5).

## Semantik taahhütler (sözleşme)

- **L1 — İnference için Open World (OWA); validation için local Closed World.**
  Örgütsel bilgi doğası gereği eksiktir (tüm kararları/context'i asla bilmezsin) → inference
  OWA: bir olgunun yokluğu *yanlış* demek değil (P2). Ama Semantic Axioms *validation'ı*
  local-CWA'dır (SC-001 "Decision'ın Purpose'u yok" fail etmeli — bu, node komşuluğunda
  closed-world ister). SHACL yaklaşımıyla aynı: OWA graf üstünde CWA validation.

- **L2 — Monotonic, temporal graf üzerinde.** Olgu eklemek sonucu geri almaz (traceability +
  audit). "Invalidation" bir olguyu *silmez*; geçerlilik aralığını (valid_to) kapatır (Meta
  Model temporal). Böylece non-monotonic karmaşıklık yok, geçmiş sorgulanabilir.

- **L3 — Unknown ≠ False (üç-değerli: True / False / Unknown).** OWA gereği. Bilinmeyen,
  yanlış değildir; bu Confidence ve P6 için zorunlu (bilmediğini "hayır" diye assert etme).

- **L4 — Negation yalnızca explicit.** Negation-as-failure **yok** (Datalog'dan ayrışırız).
  Olumsuzluk yalnızca tipli kenarlarla: `invalidates`, `contradicts`, Forbidden (ENS-4030 MC).

- **L5 — Temporal scope.** Çıkarım, öncüllerin geçerlilik penceresine saygı duyar; geçerlilik
  dışı bir olgudan çıkarım yapılmaz. Sorgular point-in-time'dır.

- **L6 — Cross-namespace inference varsayılan yasak.** Namespace-içi (ens-core) serbest;
  seviyeler arası (ens-meta ↔ ens-core) yalnızca **deklare edilmiş bridge relation** ile (ör.
  bir `ens-meta:Theory describes ens-core:Concept`). MC-007 ile tutarlı.

- **L7 — Confidence propagation.** Türetilmiş olgunun confidence'ı, öncüllerin confidence'ının
  bir t-norm'udur; **varsayılan `min`** (muhafazakâr): `conf(sonuç) = min(conf(öncüller))`.
  Çoğu mantık crisp'tir; ENS confidence-aware'dır çünkü P6 (kalibre confidence) çekirdektir.
  Bu, ENS'in ayırt edici mantık taahhüdüdür.

- **L8 — Proof trace ZORUNLU (P6).** Her türetilmiş olgu, onu üreten kuralı + öncülleri taşır.
  İzsiz çıkarım = black-box = **Anayasa Madde VI ihlali** (yasak). Proof trace bu mantığın bir
  aksiyomudur, opsiyon değil.

## Proof trace biçimi
```
Decision-42 --serves--> Purpose-3 <--supports-- Capability-7
   ⇒ [IR-001 Composition: serves ⋈ supports on Purpose-3]
   ⊢ Decision-42 indirectly_supported_by Capability-7   (conf = min(0.8, 0.7) = 0.7)
```
Her `⊢` (türetim) bir kural kimliği ve confidence taşır; geriye izlenebilir. Bu, ENS'in
"Decision Intelligence + explainability" vizyonuyla birebir hizalı.

> **Düzeltme (v0.1.1, SKR-031 D-1 kaynaklı).** Bu örneğin önceki hâli
> (`Purpose-3 --supports--> Strategy-1 ⊢ Decision-42 indirectly_supports Strategy-1`) ENS-4010
> Relation Registry ile **üç biçimde** çelişiyordu: (a) `Strategy` node'u Node Registry'de **yok**;
> (b) `supports`'un domain'i `{Capability, Evidence}` — `Purpose`'u içermez, yani
> `Purpose --supports--> ...` **Forbidden**; (c) `supports`'un range'i `{Purpose, Claim}` —
> `Strategy`'yi içermez. SKR-022 (ratified turu) bu illicit örneği **kaçırmıştı**; SKR-031
> (ENS-4031 saldırısı, D-1) açığa çıkardı. Örnek, Registry-sadık **co-target join** biçimine
> hizalandı: `serves` (Decision→Purpose) ile `supports` (Capability→Purpose) **aynı** Purpose'a
> bakar; türetilen `indirectly_supported_by` ENS-4010 §Relation Composition (line 154) ile
> birebirdir. Registry tek kaynaktır (Madde XII): düzeltme yönü **örneği Registry'ye hizalamak**,
> tersi değil. Bu bir düzeltmedir, yeni bir iddia değil — L1-L8 sözleşmesi değişmedi.

## Inference Rules bu sözleşmeye uymalı (ENS-4031)
Her kural: OWA-uyumlu (L1), monotonic-temporal (L2, L5), explicit-negation (L4), namespace-sınırlı
(L6), confidence-propagating (L7), proof-trace üreten (L8). Aksi kural geçersizdir.

## Failure conditions (Anayasa Madde X)
- **OWA/CWA sınırı bulanıklaşabilir.** İnference-OWA ile validation-CWA'yı ayırmak titizlik
  ister; karışırsa ya eksik olgu "yanlış" sayılır (OWA ihlali) ya da validation hiçbir eksiği
  yakalayamaz. SHACL-benzeri net sınır şart.
- **Confidence t-norm seçimi.** `min` muhafazakâr ama bilgi kaybeder (bağımsız kanıtların
  birleşimini güçlendirmez). İleride t-norm seçimi (min vs product vs Bayesian) bir RFC gerektirebilir.
- **Monotonic-temporal maliyeti.** Hiçbir şey silinmediğinden graf büyür; audit değeri var ama
  ölçek maliyeti (Faz 4).
- **Formal dil Faz 4.** Bu sözleşme yarı-formaldir; makine-çalıştırılabilir semantiği (SHACL +
  bir kural motoru) Faz 4'te bağlanır. Şimdilik niyet + sözleşme.
- **Örnek↔Registry sürüklenmesi (kapatıldı, D-1).** Bu belgenin ilk proof-trace örneği
  (`Purpose --supports--> Strategy`) Relation Registry'ce lisanslı değildi ve SKR-022 turunda
  kaçtı; SKR-031 D-1 ile yakalandı, v0.1.1'de Registry'ye hizalandı (bkz. §Proof trace düzeltme
  notu). Ders: ratified bir yapıt bile örnek düzeyinde Registry ile silent biçimde çelişebilir —
  formal-checker (G-09/10) yazılınca "her illüstrasyon kenarı Registry-domain/range'e uyar mı?"
  invariant'ı bu sınıf kusuru mekanik yakalamalı.

---

*Semantic Logic, ENS'in çıkarım yaparken hangi mantığa bağlı olduğunu açıkça taahhüt eder:
açık dünya, monotonic-temporal, üç-değerli, explicit-negation, confidence-taşıyan ve — her
şeyden önce — proof-trace'li. Inference örtük varsayıma değil, bu sözleşmeye dayanır.*
