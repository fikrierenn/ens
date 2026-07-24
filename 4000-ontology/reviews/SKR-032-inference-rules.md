---
id: SKR-032
type: skeptic-review
validation_dimension: ontology
origin: ENS-4031
depends_on: [ENS-4031, ENS-4025, ENS-4010, ENS-4001, ENS-4030]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
supersedes_review: SKR-031  # bağımsız 2. tur (aynı yapıt, wounded→düzeltme sonrası)
---

# SKR-032 — Inference Rules (ENS-4031 v0.2.0) Bağımsız 2. Tur Saldırısı

> **Bağımsızlık beyanı (governance G2/G3).** Bu inceleme, ENS-4025'i (D-1) ve ENS-4031'i (B1/B2)
> düzelten ens-philosopher çağrısından **ayrı, taze bir context'te** yapıldı. Her Registry
> kenarı ENS-4010/ENS-4001'in *kendisinde* yeniden doğrulandı; yazarın öz-beyan tablosuna
> (ENS-4031 §"SKR-031'e yanıt") **güvenilmedi**, bağımsız denetlendi.

## Verdict
**survives.** SKR-031'in üç maddi talebi (B1, B2, D-1) *gerçekten* karşılanmış; her düzeltme
Registry'ye karşı bağımsız olarak doğrulandı, yeni bir kaçak açılmamış, ve ENS-4025 v0.1.1 ile
ENS-4031 v0.2.0 artık aynı örneği aynı biçimde anlatıyor (tutarlılık kazanıldı). Talep 4 (küçük,
opsiyonel) dürüstçe ertelenmiş. **Not:** `survives` yalnızca skeptic kapısıdır; `ratified`/
`canon: true` ayrı bir governance edimidir ve bu SKR onu vermez.

## Talep-talep doğrulama (SKR-031 → v0.2.0)

### Talep 1 — B1 (`part_of` transitive lisansı) — KARŞILANDI
Bağımsız Registry denetimi (ENS-4010 line 133 + line 152):
- `part_of | Actor → Organization | Trans: ✓` — **domain ≠ range** teyit edildi.
- §Relation Composition line 152: `A part_of B ∧ B part_of C ⇒ A part_of C` deklarasyonu teyit
  edildi. B1'in özü doğrulandı: 2-hop zincir orta düğümün hem `Organization` (1. hop range) hem
  `Actor` (2. hop domain) olmasını ister; `Organization specializes Actor` ENS-4010'da **hiçbir
  yerde deklare değil** (Node Registry line 88-89 Actor ve Organization'ı ayrı, her ikisi de
  yalnızca "Agent profili" — profil ortaklığı ≠ domain uygunluğu). Tutarsızlık gerçek ve upstream.
- **v0.2.0'ın yanıtı (doğrulandı):** IR-002 başlığı artık "Registry-bağımlı — henüz lisanslı
  DEĞİL, B1" (line 87); geçersiz "Agent-profili" savunması çıkarılmış (line 97-99 onu açıkça
  *geçersiz* ilan ediyor); illicit çok-hop iddiası kaldırılmış ("hiçbir lisanslı örnek üretmez",
  line 100-102); çözüm ens-architect'e devredilmiş (line 103-107 + §Failure conditions B1 line
  293-303); L1-L8 matrisine † bayrağı + açıklama (line 232, 240-243) eklenmiş.
- **IR-005 örnek değişimi (doğrulandı):** Eski illicit `Team part_of Division part_of Company`
  örneği **tamamen çıkarılmış**; yerine `pursues ∘ refines` yolu (line 148-153) konmuş. Bu yeni
  yolu Registry'ye karşı bağımsız doğruladım: `pursues | Actor/Organization → Goal` (ENS-4010
  line 121) → `Actor-A --pursues--> Goal-G` domain/range **uyumlu**; `refines | Goal → Purpose`
  (line 136) → `Goal-G --refines--> Purpose-P` domain/range **uyumlu**; baş-kuyruk (Goal ortak
  düğüm = 1. hop range, 2. hop domain), transitivity gerekmez. **Lisanslı.** IR-002 örneğinin
  aksine bu yol gerçekten fire eder.

### Talep 2 — B2 (tekil-kaynak iddiası) — KARŞILANDI
- ENS-4001 line 82 bağımsız teyit: `contradicts | Concept → Concept | symmetric | N:N`. Meta
  Model'de gerçek/tipli ve **symmetric**. ENS-4010 Relation Registry'de (line 117-136)
  **yok** — B2'nin öncülü doğru.
- **v0.2.0'ın yanıtı (doğrulandı):** Header blockquote (line 26-29) "ENS-4010 Relation
  Registry'sindeki gerçek relation'lara **veya** ... ENS-4001 Meta Model'deki gerçek, tipli
  relation'lara (ör. IR-007'nin `contradicts`'i) dayanır" olarak yumuşatılmış. Aynı yumuşatma
  §Relationships/ENS-4010 (line 264-266) ve §"Doğrulama sözü" (line 245-247: "ne ENS-4010
  Registry'de ne ENS-4001 Meta Model'de gerçek/tipli olmayan bir kural") ve §Failure conditions
  B2 (line 304-308) içinde tutarlı biçimde uygulanmış. Tekil-kaynak sözü artık IR-007 ile
  çelişmiyor.

### Talep 3 — D-1'i doğru adrese yönlendir (ENS-4025) — KARŞILANDI
Bu, SKR-031'in en kritik bulgusuydu: D-1 ENS-4031'in değil, **ratified ENS-4025'in** borcuydu ve
SKR-022 ratified turunda kaçmıştı. Bağımsız doğrulama:
- **ENS-4025 v0.1.1 yeni proof-trace örneği (line 75-77) — Registry'ye karşı TEK TEK denetlendi:**
  ```
  Decision-42 --serves--> Purpose-3 <--supports-- Capability-7
     ⊢ Decision-42 indirectly_supported_by Capability-7
  ```
  (a) `serves | Decision → Purpose` (ENS-4010 line 117) → `Decision-42 --serves--> Purpose-3`
  domain/range **uyumlu** ✓; (b) `supports | Capability/Evidence → Purpose/Claim` (line 118) →
  `Capability-7 --supports--> Purpose-3`: Capability domain ∈ {Capability,Evidence} ✓, Purpose
  range ∈ {Purpose,Claim} ✓; (c) Node Registry: `Decision` (line 79), `Purpose` (line 80),
  `Capability` (line 90) **üçü de mevcut** — eski örnekteki hayalet `Strategy` node'u tamamen
  gitmiş; (d) türetilen `indirectly_supported_by` = ENS-4010 §Composition (line 154-155) ile
  **birebir**. **Örnek tam lisanslı — D-1 kapandı.** Eski üç-katlı ihlal (Strategy yok /
  Purpose domain-dışı / Strategy range-dışı) tamamen giderilmiş.
- ENS-4025 künyesi güncellenmiş: `version: 0.1.1`, `skeptic_review` notu D-1 düzeltmesini
  belirtiyor; §Proof trace düzeltme notu (line 82-92) ve §Failure conditions (line 108-113) D-1'i
  ENS-4025'in borcu + SKR-022 kaçırması olarak dürüstçe kaydediyor.
- ENS-4031'in D-1 failure-condition'ı (line 286-292) "ENS-4025 tarafında KAPATILDI, v0.1.1" olarak
  doğru adrese yönlendirilmiş.

### Talep 4 (opsiyonel) — `indirectly_serves` isim + meta-kural sayımı — DÜRÜSTÇE ERTELENDİ
v0.2.0 bunu küçük/stilistik kabul edip erteledi (§SKR-031'e yanıt, satır 279). Meşru: `survives`
eşiği için gerekli değil; ENS-4000 sözlüğüne türetilmiş-relation kaydı ayrı bir iş kalemi. Kaçamak
değil, açık borç.

## Yeni kaçak taraması (SKR-031'in olmayan 5. sorusu — bağımsız)
Düzeltme sırasında yeni bir Registry-uyumsuzluğu ya da yeni illicit örnek açılmış mı diye tüm
IR-001..IR-008 örneklerini yeniden taradım:
- **IR-001** serves⋈supports co-target (lisanslı) · **IR-002** örnek üretmiyor (bayraklı) ·
  **IR-003** generic specializes (domain=range, lisanslı) · **IR-004** pursues∘refines (lisanslı,
  yukarıda doğrulandı) · **IR-005** artık pursues∘refines (lisanslı) · **IR-006** invalidates
  Evidence→Claim (ENS-4010 line 119, lisanslı) · **IR-007** contradicts (ENS-4001 line 82,
  lisanslı) · **IR-008** generic F⟵P₁,P₂ (somut kenar yok). **Belgede kalan illicit örnek YOK.**
- **ENS-4025 v0.1.1 ↔ ENS-4031 IR-001 tutarlılığı — KAZANILDI (regresyon değil, iyileşme).**
  SKR-031 döneminde iki belge D-1 örneğini *farklı* anlatıyordu (biri illicit zincir, biri sadık
  join). Şimdi ikisi de **aynı** co-target join'i, **aynı** `⋈` notasyonuyla, **aynı** türetilmiş
  `indirectly_supported_by` ile anlatıyor (ENS-4025 line 75-77 ≡ ENS-4031 line 73-76, yalnızca
  somut vs generic düğüm adları farkı). İki belge arasında **yeni tutarsızlık açılmadı**, var olan
  kapandı.

## Kalan küçük notlar (bloke etmez — kayıt için)
1. **Notasyon sürüklenmesi ⋈ vs ∘.** ENS-4025 ve ENS-4031 co-target join'i `⋈` ile yazıyor;
   ENS-4010 §Composition line 154 aynı bileşimi `∘` ile yazıyor. Semantik özdeş (SKR-031 `⋈`'nin
   daha doğru olduğunu saptamıştı — baş-kuyruk değil ortak-hedef), dolayısıyla bu bir *iyileştirme*,
   kusur değil. Yine de tek-kaynak hijyeni için ENS-4010 §Composition'ın da `⋈`'ye hizalanması (ya
   da eşdeğerliğin orada notlanması) küçük bir borç — ens-architect'in G-14-sonrası temizliği.
2. **`indirectly_serves` semantiği** (IR-004): `serves`'in Decision→Purpose semantiğini
   Actor→Purpose'a taşır; türetilmiş relation olduğu için Forbidden değil ama isim yakınlığı
   karışma riski taşır. Talep 4 kapsamında ertelendi — sözlük kaydıyla kapatılmalı.
3. **"8 kural" sayımı.** IR-005/IR-008 birer meta-kural (L7'nin yeniden ifadesi), Registry-kenarı
   üzerine composition değil; sayım hafif şişkin. Stilistik, kabul edilmiş.

## part_of upstream borcu — bloke ediyor mu? (bağımsız karar)
Fixer'ın itiraf ettiği açık borç (ENS-4010 `part_of` tiplemesi, owner ens-architect, henüz
düzeltilmedi) **ENS-4031'i bloke ETMİYOR.** Gerekçe: IR-002 artık "lisanslı değil" bayrağıyla
duruyor, **hiçbir lisanslı türetim üretmiyor**, ve tek illicit örneği (IR-005) çıkarıldı. Bu, D-1'in
IR-001'de ele alınışıyla **yapısal olarak özdeş** ve doğru bir muhafazakâr davranıştır: belge, kendi
üretemeyeceği bir çıkarımı *iddia etmiyor*. Kök kusur upstream'de (ENS-4010) ve orada
kapatılmalı — ama bu, ENS-4031'in skeptic kapısını geçmesini engellemez; ENS-4031 kendi vaadini
(her fire eden öncül gerçek bir Registry/Meta kenarıdır) artık **iki kuralda da yerel olarak
ihlal etmiyor.** B1 açık kalırsa etkilenen tek şey IR-002'nin *ileride* lisanslı hâle gelmesidir;
belgenin bugünkü tutarlılığı değil.

## Kalıcı sistemik ders (SKR-031'den taşınan, kapanmadı)
D-1'in kaynağı — **ratified/M2 bir yapıtta (ENS-4025) SKR-022 turunda kaçmış Registry-illicit bir
örnek** — bir kez daha vurgulanmalı: ratified statü, örnek-düzeyi Registry uyumunu garanti etmiyor.
ENS-4025 v0.1.1 bunu §Failure conditions line 108-113'te "formal-checker (G-09/10) yazılınca 'her
illüstrasyon kenarı Registry-domain/range'e uyar mı?' invariant'ı bu sınıf kusuru mekanik
yakalamalı" olarak kaydetmiş — doğru refleks. Bu, ENS-4031'in borcu değil (kapanış talebi yok),
ama Faz 4 formal-checker backlog'unda canlı tutulmalı.

## Sahibine talepler (kapıyı geçmek için)
**Yok — kapı geçildi.** B1/B2/D-1 karşılandı; yeni kaçak yok; iç tutarlılık kazanıldı. Kalan üç
küçük not (⋈/∘ hizalama, `indirectly_serves` sözlük kaydı, meta-kural sayımı) bloke edici değil ve
ROADMAP'e küçük iş kalemi olarak düşülebilir. `ratified`/`canon: true`'ya geçiş **ayrı bir
governance edimidir** (owner/ADR süreci) — bu SKR onu vermez; yalnızca skeptic dördüncü kez
(D-1→ENS-4025, B1/B2→ENS-4031) doğrulanmış olarak **skeptic-cleared** işaretler.
