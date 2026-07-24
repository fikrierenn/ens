---
id: SKR-031
type: skeptic-review
validation_dimension: ontology
origin: ENS-4031
depends_on: [ENS-4031, ENS-4025, ENS-4010, ENS-4001, ENS-4030]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-031 — Inference Rules (ENS-4031) Saldırısı

## Verdict
**wounded.** Yöntem sağlam ve dürüst (atıflar *gerçek*, uydurma yok; D-1 örnek bir dürüstlükle
açığa çıkarılmış; deferral'lar muhafazakâr), ama iki gerçek çatlak var: (1) **IR-002/IR-005 `part_of`
transitive zinciri, ENS-4010'un kendi tiplemesi (`Actor → Organization`) tarafından lisanslı
DEĞİL** — ve bu, D-1 ile *yapısal olarak özdeş* bir Registry tutarsızlığı olmasına rağmen belge
tarafından **bayrakla işaretlenmemiş** ("uydurma yok" vaadini yerel olarak deler); (2) "her kural
ENS-4010 Registry'deki gerçek relation'a dayanır" **tekil-kaynak iddiası fazla geniş** — IR-007'nin
öncül relation'ı `contradicts` ENS-4010'da değil, ENS-4001 Meta Model'de yaşar. İkisi de
giderilebilir; kavram temelde ayakta ama kapıyı geçmeden önce owner bu iki borcu kapatmalı.

## Yenilik incelemesi
Prior-art konumlandırması dürüst ve **doğrulanabilir** (uydurma yok):
- **OWL 2 property chain axioms** (`SubObjectPropertyOf(ObjectPropertyChain(...))`) — IR-001/IR-004'ün
  tam karşılığı. Gerçek, standart.
- **`owl:TransitiveProperty`** — IR-002/IR-003. Gerçek.
- **SWRL, Datalog kural gövdeleri, RETE (üretim sistemleri)** — kural biçimi. Gerçek.
- **ProbLog, Markov Logic Networks, fuzzy t-norm'lar** — confidence tarafı (min = Gödel t-norm).
  Gerçek. min'in "weakest-link" muhafazakârlığı fuzzy mantıkta standart bir seçimdir.
- **Truth Maintenance System / justification tree** — proof-trace. Gerçek.

**ENS'in gerçek katkısı (delta):** Belge *yeni bir mantık icat etmiyor* ve bunu açıkça söylüyor.
Delta üç yerde ve **ENS-4025'ten miras** (bağımsız yeni bir iddia değil): (a) kuralların Registry'ye
kapatılması (L4/L6), (b) her kuralın confidence taşıması (L7), (c) zorunlu proof-trace (L8, P6/Madde
VI). Bu üçlü bileşim ENS'e özgü olmakla birlikte *bu belgenin değil, ENS-4025'in* katkısıdır; ENS-4031
onu somut kurallara indirger. Bu meşru bir "uygulama" katkısıdır, "keşif" iddiası yok — dürüst.
Terminoloji `⋈` (co-target join) IR-001 için aslında Registry'nin `∘` (line 154) notasyonundan **daha
doğru** (serves ve supports baş-kuyruk değil, ortak-hedef bağlanır); küçük bir iyileştirme.

## Yanlışlanabilirlik
Belge **yanlışlanabilir** ve bunu somutça beyan ediyor: "L1-L8 uyum matrisini ihlal eden ya da
Registry-dışı relation'a dayanan bir kural bu belgeye giremez" (line 224-225, ENS-4025 §"Aksi kural
geçersizdir"). Bu, gözlemle çürütülebilir bir ret kriteridir — ve ben onu bu belgenin *kendi
kurallarına* uyguladığımda iki kural takılıyor (aşağıda). Yani kriter **çalışıyor**; sorun kriterin
kendisi değil, yazarın onu tüm kurallara **eşit titizlikle uygulamamış** olması (D-1'i yakaladı,
`part_of` ikizini kaçırdı). Bu, "falsifiable ama self-application eksik" durumudur — kusur teoride
değil, öz-denetimde.

## Varsayım haritası
Her kuralı ENS-4010 Relation Registry'ye karşı bağımsız denetledim:

| Kural | Öncül tipleme (Registry'ye karşı) | Durum |
|-------|-----------------------------------|-------|
| **IR-001** | `serves: Decision→Purpose` ✓ + `supports: Capability/Evidence→Purpose/Claim` ✓ (Purpose range'de). Ortak-hedef join. Türetilen `indirectly_supported_by` = Registry §Composition line 154 ile birebir. | **TEMİZ** |
| **IR-002** | `part_of: Actor→Organization`, Trans ✓. AMA 2-hop zincir B'nin hem range (Organization) hem domain (Actor) olmasını ister. **`Organization --part_of--> Organization` Registry domain'inde (Actor) YOK.** | **ÇATLAK (B1)** |
| **IR-003** | `specializes: Node→Node (aynı tür)`, Trans ✓. Domain=range → gerçekten zincirlenebilir. Bridge şartı (cross-ns) ENS-4001 v0.3'te artık deklare (line 112). | **TEMİZ** |
| **IR-004** | `pursues: Actor/Organization→Goal` ✓ + `refines: Goal→Purpose` ✓. Baş-kuyruk (Goal ortak), her kenar tekil, transitivity gerekmez. | **TEMİZ** |
| **IR-005** | min-t-norm propagation, L7 ile birebir. AMA somut örnek `Team part_of Division part_of Company` = IR-002'nin çatlağını miras alır. | **ÇATLAK (B1 mirası)** |
| **IR-006** | `invalidates: Evidence→Claim` ✓. True→Unknown (L3), valid_to (L2), downstream blok (L5). MC-004 atfı **doğrulandı** (ENS-4030 line 63). | **TEMİZ** |
| **IR-007** | `contradicts` symmetric **doğrulandı** ama ENS-4001'de (line 82), **ENS-4010 Registry'de değil**. MC-004/005/007 atıfları doğrulandı. | **TEMİZ öncül, ama tekil-kaynak iddiasını deler (B2)** |
| **IR-008** | Proof-arası aggregation'ı RFC'ye (SKR-022 OL-logic-2) erteler; max-proof-içi raporlar, güçlenme iddia etmez. | **MEŞRU (ince ama kaçamak değil)** |

**Kırılma koşulları:**
- **B1 (IR-002/IR-005):** ENS-4010, `part_of`'u `Actor → Organization` (domain≠range) tiplerken aynı
  anda `Trans: ✓` işaretler ve §Composition'da `A part_of B ∧ B part_of C ⇒ A part_of C` zincirini
  deklare eder (line 152). Bu tipleme **kendi içinde tutarsızdır**: `R: X→Y` (X≠Y) bir zincir
  oluşturamaz çünkü orta düğüm hem Y (range) hem X (domain) olmak zorundadır. `Team/Division/Company`
  hepsi Organization'dır → `Organization --part_of--> Organization` gerekir, ama `part_of`'un domain'i
  yalnızca `Actor`. Belge bunu "Organization Agent-profili node'u olduğundan meşru" diye geçiştirir
  (IR-002 parantezi) — **aynı profil (Agent) ≠ aynı domain-uygunluğu.** `Organization specializes
  Actor` hiçbir yerde deklare değildir. Bu, D-1 ile **birebir aynı sınıf** kusurdur, ama bu sefer
  ENS-4031 onu *yakalayıp devretmek yerine* örttü ve `failure_conditions`'a yazmadı. Kırılır: bir
  Linter "her transitive relation'ın range'i domain'inde midir?" invariant'ını çalıştırdığında
  `part_of` fail eder.
- **B2 (IR-007):** "Her kural ENS-4010 Registry'deki *gerçek* relation'lara dayanır" (line 27) ve
  "Registry-dışı relation yasak" (line 240-241) mutlak iddiası, IR-007'nin `contradicts`'i ENS-4001'den
  çekmesiyle çürür. Belge IR-007 gövdesinde bunu dürüstçe belirtir ("ENS-4001 Meta Model'de contradicts
  symmetric") — yani **fabrikasyon değil**, ama üst-başlıktaki tekil-kaynak sözü fazla geniş.
- **IR-008 (bilinçli açık):** min proof-*içi* iken max proof-*arası* raporlamak L7 (default min) ile
  görünüşte gerilir; ama L7 yalnızca yol-içi min taahhüt eder, yollar-arası **sessizdir** ve ENS-4025
  L7-failure + SKR-022 OL-logic-2 bunu zaten açık bırakmıştır. Dolayısıyla deferral **meşru**, kaçamak
  değil. Küçük çekince: IR-008 ve IR-005 birer *meta-kural* (L7'nin yeniden ifadesi), Registry-kenarı
  üzerine composition değil — "8 kural" sayısı bu ikisiyle bir miktar şişirilmiş; stilistik, kusur değil.

## En güçlü karşı-argüman (steelman)
Owner haklı olarak şöyle diyebilir: *"B1 benim değil, ENS-4010'un borcudur — tıpkı D-1 gibi. Ben
Registry'nin deklare ettiği `part_of transitive`'i sadakatle uyguladım; ENS-4010 line 152 bu zinciri
zaten deklare ediyor. Upstream tipleme kusurunu düzeltmek benim yetkimde değil (skeptic bile yapamaz)."*

Bu itiraz **kısmen** geçerli — evet, kök kusur ENS-4010'dadır (D-1 ile aynı biçimde). Ama tam da bu
yüzden owner'ın onu **D-1 ile aynı muameleye** tabi tutması gerekirdi: bir `failure_condition` + skeptic'e
devir. Owner D-1'i (supports/Strategy) yakalarken yapısal ikizini (part_of/Organization) **aynı
Registry'de, aynı türde** kaçırmıştır. Bu bir tutarsızlık: belgenin öz-denetim standardı düzgün
uygulanmamış. "Ben upstream'i düzeltemem" doğru; "ben upstream çatlağını *görmezden gelebilirim*"
yanlış — çünkü belgenin merkezî vaadi "her öncül gerçek bir Registry kenarıdır" ve B1 bu vaadi iki
kuralda yerel olarak deler. Steelman'in götürdüğü yer: **verdict refuted değil** (kök kusur upstream,
kural biçimi sağlam), **ama survives da değil** (öz-denetim eşit uygulanmadı) → **wounded**.

## İç tutarlılık
- **D-1 bağımsız teyidi — DOĞRULANDI.** ENS-4025 proof-trace örneği (`Decision-42 --serves-->
  Purpose-3 --supports--> Strategy-1`, ENS-4025 line 75-77) ENS-4010 Registry ile **üç** biçimde
  çelişir: (a) Node Registry'de (line 79-95) `Strategy` node'u **yok**; (b) `supports`'un domain'i
  `{Capability, Evidence}` — Purpose domain'de **değil** (line 118), yani `Purpose --supports-->`
  Forbidden; (c) `supports`'un range'i `{Purpose, Claim}` — `Strategy` range'de de **değil**. Çifte-
  artı-eksik ihlal. **Konum: bu ENS-4025'in kusurudur, ENS-4031'in değil.** ENS-4031 örneği
  *yeniden üretmeyi reddedip* IR-001'i Registry-sadık join olarak tanımlayarak **doğru** davranmıştır.
  **Kimin düzeltmeli:** ENS-4025'in owner'ı (ens-philosopher). Registry-önceliği (Madde: Registry tek
  kaynak) gereği düzeltme yönü = **ENS-4025 örneğini Registry'ye hizala** (Strategy→Purpose/Claim,
  `supports` yerine doğru yön), Registry'yi örneğe uydurmak değil.
- **Ek bulgu — SKR-022'nin kaçırdığı:** ENS-4025 `SKR-022` ile "survives → M2 (ratified)" aldı, ama
  SKR-022 bu Registry-illicit örneği **yakalamadı.** Yani D-1 aslında *ratified bir yapıttaki kaçmış
  bir kusurdur.* ENS-4025'in yeniden dokunulması gerekir; SKR-022 bu noktada eksik kalmıştır.
- **B1 — ikinci, işaretlenmemiş Registry tutarsızlığı** (yukarıda): `part_of` transitive tiplemesi.
  D-1 ile aynı sınıf, aynı Registry, farklı satır. Bu SKR'nin özgün katkısı.
- **B2 — tekil-kaynak iddiası ile IR-007 gerilimi** (yukarıda).
- **Atıf dürüstlüğü — TEMİZ.** MC-004 (ENS-4030 line 63, eşzamanlı çelişki), MC-005 (line 64,
  self-support), MC-007 (line 66, namespace), `contradicts` symmetric (ENS-4001 line 82),
  valid_from/valid_to (ENS-4001 line 126), Bridge/Semantic Connector + `specializes*` cross-ns
  (ENS-4001 line 87-116) — **hepsi gerçek ve bulunabilir.** Uydurma atıf yok. SKR-022 OL-logic-1/2
  atıfları da doğru (OL-logic-1 Bridge, OL-logic-2 t-norm-RFC). Bu, belgenin en güçlü yanı.
- **Sözlük (ENS-4000) sürüklenmesi:** `indirectly_supported_by`, `indirectly_serves`, `contested_by`
  türetilmiş relation'lardır (Registry'de yok, olmaları da gerekmez — türetilir, saklanmaz). Ancak
  `indirectly_serves` (IR-004), `serves`'in (Decision→Purpose) semantiğini Actor→Purpose'a taşır;
  Actor `serves`'in domain'inde değildir. Türetilmiş olduğu için Forbidden değil, ama isim seçimi
  `serves` ile karışma riski taşır (küçük terminoloji notu). ENS-4000 sözlüğüne bu türetilmiş
  relation'ların eklenip eklenmeyeceği açık.

## Sahibine talepler (kapıyı geçmek için)
1. **B1'i D-1 ile aynı statüye çıkar.** `part_of` transitive tipleme kusurunu (`Actor→Organization`
   zincirlenemez) bir açık `failure_condition` olarak ekle ve ENS-4010 owner'ına (ens-architect)
   devret. Çözüm iki yoldan biri: (a) ENS-4010'da `Organization specializes Actor` (ya da ortak bir
   `CollectiveAgent` üsttürü) deklare et → Organization part_of domain'ine girer; VEYA (b) `part_of`
   domain'ini `Actor/Organization → Organization` olarak genişlet. IR-002/IR-005 bu düzelene dek
   "Registry-bağımlı, henüz lisanslı değil" notu taşımalı — tıpkı IR-001'in D-1 notu gibi.
2. **Tekil-kaynak iddiasını (B2) yumuşat.** line 27 ve line 240-241'deki "her kural ENS-4010
   Registry'deki relation'a dayanır" ifadesini "ENS-4010 Relation Registry **veya** ENS-4001 Meta
   Model'deki (ör. `contradicts`) gerçek, tipli relation'lara dayanır" olarak düzelt — IR-007 ile
   tutarlı olsun.
3. **D-1'i doğru adrese yönlendir.** D-1 ENS-4031'in değil ENS-4025'in borcudur; ENS-4031 onu
   kapatamaz (doğru). ROADMAP/G-14'te D-1 sahipliği açıkça **ENS-4025 (+ SKR-022 kaçırması)** olarak
   kaydedilmeli; ENS-4025 örneği Registry'ye hizalanana dek ENS-4025 `M2→` yeniden-dokunma borcu
   taşımalı.
4. **(İsteğe bağlı, küçük)** `indirectly_serves` isminin `serves` ile karışmaması için türetilmiş
   relation'ları ENS-4000 sözlüğünde ayrı işaretle; IR-005/IR-008'in meta-kural doğasını "8 kural"
   sayımında dipnotla.

Bu dört talep karşılandığında (özellikle 1 ve 2) belge **survives** eşiğine gelir. Kural biçimleri
(IR-001/003/004/006 temiz, IR-007 öncülü gerçek, IR-008 deferral'ı meşru) ve atıf dürüstlüğü zaten
sağlam; eksik olan tek şey öz-denetimin **eşit** uygulanmasıdır.
