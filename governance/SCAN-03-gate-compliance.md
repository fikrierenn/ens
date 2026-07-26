---
id: SCAN-03
title: Yönetişim Kapısı Uyum Taraması (Gate Compliance Scan)
type: audit-report
status: final
owner: ens-architect
created: 2026-07-26
scope: Külliyat genelinde G2/G3/G4/G5 + boyut çeşitliliği + faz kapıları
verdict: kısmen geçiyor
---

# SCAN-03 — Yönetişim Kapısı Uyum Taraması

> **Verdict: Külliyat kendi yönetişim kapısından KISMEN GEÇİYOR.** Ayrıntı §8.
> Bu rapor **salt denetimdir**; hiçbir yapıt değiştirilmemiştir. §7.4'teki önerilerin
> tamamı RFC/ADR yordamı gerektirir (Madde XIV/XV) ve bu raporla yürürlüğe girmez.

## §0. Taramanın Gerekçesi

2026-07-26'da yazılan `.claude/rules/work-protocol.md`, her yapıta **tek** doğrulama kapısı
atıyordu. Bağımsız inceleme, `governance/000-governance-principles.md:35` (**G4**) ve
`governance/canonical-process.md:44` (**GOV-030 Kural 2**) uyarınca **≥2 bağımsız boyut
validator'ı** gerektiğini gösterdi — yani yeni operasyonel kural, mevcut yönetişimden **daha
gevşekti**.

Bu tarama, doğal takip sorusunu sorar: **Külliyat'ın kendisi kendi kapısından geçmiş mi?**
Yani ENS, kendi yazdığı G2/G3/G4'ü kendi yapıtlarına uygulamış mı?

> **Kapsam notu.** Bu bir *statü denetimi*dir, içerik denetimi değil. Bir SKR'nin verdict'inin
> doğru olup olmadığı sorgulanmaz; yalnızca **kaç tane, kim tarafından, hangi boyutta** ve
> **statüyü kim çevirdi** sorulur.

## §1. Yer Gerçeği (Ground Truth)

### 1.1 G1..G7 — `governance/000-governance-principles.md:32-40`

| ID | Metin (tam) | Satır |
|----|-------------|-------|
| **G1** | Authority follows accountability. Yetki, sorumlulukla gelir; sorumlu olmayan yetki veremez. | `:32` |
| **G2** | **No author canonizes their own work.** Bir yapıtı yazan, onu Canonical yapamaz. | `:33` |
| **G3** | **Validation ve approval ayrıdır.** Doğrulayan onaylamaz; onaylayan doğrulamaz. | `:34` |
| **G4** | **Her Canonical yapıtın ≥2 bağımsız validator'ı vardır** (farklı boyutlardan). | `:35` |
| **G5** | Governance kararları izlenebilirdir. Her karar bir kayıt (SkepticReview/ADR/RFC) ve Meta Model kenarı bırakır; sessiz karar yoktur. | `:36-37` |
| **G6** | İtiraz (appeal) mümkündür. Her promotion/deprecation kararına gerekçeli itiraz edilebilir. | `:38-39` |
| **G7** | Governance bireylere değil, Anayasa'ya hizmet eder. Çatışmada Anayasa kazanır (Madde XV). | `:40` |

**Kritik okuma (kapsam belirsizliği — B-01).** G2/G4 metinleri **"Canonical"** der. GOV-000'in
kendi sonucu (`:45-47`) bunu M5 ile eşitler: *"Engineering Validation Faz 4'ü gerektirdiğinden
M5 şu an ulaşılamaz — Canon boş."* Dolayısıyla harfiyen okumada G4, `ratified` (M2/M3) statüsü
için **hiçbir şey söylemez**; yalnızca M5 kapısını bağlar.

Ancak `governance/roles.md:38-39` ayrımları koşulsuz koyar (*"Validator ≠ Author (G2)"*,
*"Validator ≠ Governance (G3)"*), ve Külliyat pratiği (ROADMAP boyunca tekrarlanan
"**ÖZ-ONAY YOK (G2/G3)**" işareti, ör. `ROADMAP.md:39`) G2/G3'ü **her statü geçişine**
uygular. Bu tarama **iki okumayı da ayrı ayrı raporlar**; verdict §8'de ikisine göre verilir.

### 1.2 GOV-030 — Canonical olma süreci (`governance/canonical-process.md`)

Zincir (`:24-39`): `M3 Stable → Scientific Validation ✓ → Ontology Validation ✓ →
Engineering Validation ✓ → M4 Reference → Operational Evidence → Governance Approval → M5`.

Kurallar (`:41-48`):
1. `:42-43` — **Her kapı bir kanıttır**, oy değil. Governance kanıt üretmez; zincirin tamlığını
   doğrulayıp son kapıyı açar.
2. `:44` — **G4: ≥2 bağımsız boyut validator'ı** (ör. Scientific + Ontology, uygunsa + Engineering).
3. `:45` — **G2: Author zincirin hiçbir kapısını kendi açamaz.**
4. `:46-47` — M4+ Faz 4 gerektirir → **M5 şu an ulaşılamaz, Canon boştur.**
5. `:48` — İtiraz (G6) yeni bir validation turu açar.

**Sayı ve boyut ayrı koşullardır.** `:44` "≥2 **bağımsız boyut** validator'ı" der — parantezli
örnek (`Scientific + Ontology`) sayının **farklı boyutlardan** gelmesini şart koşar. İki
Scientific turu, harfiyen okumada `:44`'ü karşılamaz. Bu, §5'in (Denetim D) dayanağıdır.

### 1.3 Anayasa — Madde XIV ve XV

- **Madde XIV** (`0000-constitution/ENS-0000-constitution.md:237-245`): RFC yaşam döngüsü
  `Draft → Review → Skeptic-Challenged → Accepted | Rejected → Superseded`; ADR yaşam döngüsü
  `Proposed → Accepted → Superseded`. `:244-245` — *"`ens-ceo` uzun vadeli hizayı korur;
  `ens-skeptic` sağlamlığı korur; `ens-style-guardian` tutarlılığı korur. **Önemli kararlarda
  hiçbiri atlanmaz.**"* → önemli kararlarda **üç ayrı rol** zorunludur. Bu, G4'ün Anayasa
  düzeyindeki karşılığıdır ve `ens-skeptic`in **tek başına** yeterli olmadığını söyler.
- **Madde XV** (`:247-251`): Anayasa yalnızca (a) maddeye atıf yapan, (b) skeptic saldırısından
  geçen, (c) Madde XIV uyarınca kabul edilen bir RFC ile değişir.
- **Madde IV** (`:98-106`): Anayasa **biricik kendi kendini yetkilendiren** yapıttır
  (`constitutive: true`, `canon: true`); Münchhausen trilemmasını durduran Grundnorm. Bu,
  ENS-0000'in G2/G4 dışında tutulmasının **ilkeli** gerekçesidir — ad hoc istisna değil.
- **Madde IV** (`:107-112`): `constitutive: true` yapıtlar `canon: true` olmak için
  **ratifikasyon** yolunu izler ve *"`ens-skeptic` bu tutarlılık incelemesinden sağ çıkar"* —
  **tekil** ifade. Bu cümle G4'ün "≥2 farklı boyut" şartıyla **gerilim** hâlindedir (bkz. Ç-01).

### 1.4 Maturity (M0-M5) ve Evidence (E0-E4)

Maturity (`.claude/standards/maturity-model.md:24-31`):

| Seviye | Giriş koşulu (özet) | `status` | Faz |
|--------|---------------------|----------|-----|
| M0 Draft | Yazıldı | `draft` | 1-2 |
| M1 Proposed | Künye tam; prior-art; failure conditions | `review` | 1-2 |
| **M2 Reviewed** | Scientific skeptic'ten **survives (≥1 SKR)** | `skeptic-challenged`→`ratified` | 1-2 |
| **M3 Stable** | M2 + ≥2 alt-akış yapıtı kırmadan kullanıyor + sci evidence ≥ **E3** | `ratified` | 1-2 |
| M4 Reference | M3 + Faz-4 çalışan implementation + eng evidence ≥ E3 | `ratified` | **4** |
| M5 Canonical | M4 + temel + **dört-skeptic (Sci+Eng+Biz+Ethical)** + governance kabulü | `ratified` | 4-5 |

Kurallar (`:34-39`): **`canon: true` yalnızca M5'tir**; M4+ Faz 4 gerektirir; *"Faz 1-2 boyunca
azami olgunluk M3'tür ve hiçbir yapıt Canonical (M5) değildir. Canon şu an boştur."*

Evidence (`.claude/standards/evidence-standard.md:26-31, 35-39`): seviyeler E0 Opinion → E1
Case Study → E2 Multiple Cases → E3 Academic/Peer → E4 Formal Proof/Empirical; boyutlar
Scientific / Engineering / Operational / Economic. Beyan edilen mevcut durum (`:50-52`):
sci çerçeve E3 / delta E0-E1, eng+ops+econ **hepsi E0**.

**M2 ile G4 arasındaki normatif çatışma (Ç-01).** `maturity-model.md:28` M2 için **≥1 SKR**
yeter der; `canonical-process.md:44` (G4) **≥2 farklı boyut** ister. İkisi farklı eşiklerdir
ve **hangisinin `ratified` statüsünü yönettiği hiçbir yerde çözülmemiştir.** Külliyat'ın
`ratified` yapıtlarının çoğu M2 kuralını sağlar, G4'ü sağlamaz (§2). Bu bir *belge*
uyuşmazlığıdır; kimin haklı olduğu bu raporun kararı değildir — **RFC ile çözülmelidir**.

## §2. Denetim A — G4 Uyumu (≥2 bağımsız boyut validator'ı)

### 2.1 Evren

`status: ratified` taşıyan **36 dosya** var. Bunlar dört sınıfa ayrılır:

| Sınıf | Adet | Not |
|-------|------|-----|
| Külliyat yapıtı (ENS-*) | **9** | G4 denetiminin asıl hedefi |
| Standard (`.claude/standards/*`) | **11** | Anayasa Madde XII'de Külliyat'ın *altında* |
| SKR kaydı (kendi statüsü) | **13** | doğrulama kaydının kendi statüsü — meta |
| CEO hiza kaydı | **3** | CEO-0001/0002/0003 |

Ayrıca `canon: true` taşıyan **4 gerçek yapıt** vardır (`ENS-0000:5`, `ENS-1000:5`,
`ENS-3000:5`, `ENS-4000:5`; diğer iki isabet `traceability.md:43` ve `metadata-header.md:29`
şablon örneği içindedir, gerçek künye değil).

### 2.2 A-tablosu — `ratified` Külliyat yapıtları ve doğrulama zincirleri

| # | Yapıt (`dosya:satır`) | canon | maturity | Künyedeki `skeptic_review` | Fiili tur | Boyut(lar) | Validator | G4 sayı | G4 boyut |
|---|---|---|---|---|---|---|---|---|---|
| 1 | `0000-constitution/ENS-0000-constitution.md:12` | **true** | — | **alan yok** | **0** | — | — | ✗ | ✗ |
| 2 | `1000-philosophy/ENS-1000-manifesto.md:11` | **true** | — | `SKR-002` (`:16`) | 2 (SKR-001 *wounded* → SKR-002 *survives*) | scientific | ens-skeptic | ✓* | **✗** |
| 3 | `2000-theory/ENS-2001-decision-theory.md:12` | false | M3 | `SKR-004; SKR-033` (`:17`) | 3 (003 *w* → 004 *s*; 033 *s*) | scientific | ens-skeptic | ✓* | **✗** |
| 4 | `2000-theory/ENS-2002-context-theory.md:12` | false | M3 | `SKR-006` (`:17`) | 2 (005 *w* → 006 *s*) | scientific | ens-skeptic | ✓* | **✗** |
| 5 | `3000-laws/ENS-3021-decision-entropy.md:12` | false | M3 | `SKR-012` (`:17`) | 2 (011 *w* → 012 *s*) | scientific | ens-skeptic | ✓* | **✗** |
| 6 | `3000-laws/ENS-3022-decision-gravity.md:12` | false | M3 | `SKR-014` (`:17`) | 2 (013 *w* → 014 *s*) | scientific | ens-skeptic | ✓* | **✗** |
| 7 | `3000-laws/ENS-3023-decision-capital.md:12` | false | M3 | `SKR-016` (`:17`) | 2 (015 *w* → 016 *s*) | scientific | ens-skeptic | ✓* | **✗** |
| 8 | `4000-ontology/ENS-4030-semantic-axioms.md:11` | false | M2 | `SKR-021` (`:16`) | **1** | ontology | ens-skeptic | **✗** | **✗** |
| 9 | `4000-ontology/ENS-4025-semantic-logic.md:11` | false | M2 | `SKR-022` (`:16`) | **1** | ontology | ens-skeptic | **✗** | **✗** |

`✓*` = **tur** sayısı ≥2, ama turlar **ardışık aynı-boyut** turlarıdır (wounded → düzeltme →
survives), G4'ün istediği *farklı boyut* değil. Ayrıntı §5.

**A-01 — Tek validator ile `ratified` olmuş yapıtlar (G4 sayı-ihlali):**
- `4000-ontology/ENS-4030-semantic-axioms.md:11` — tek SKR (`SKR-021`, verdict `survives → M2`,
  `4000-ontology/reviews/SKR-021-semantic-axioms.md:16`). Ön wounded tur **yok**.
- `4000-ontology/ENS-4025-semantic-logic.md:11` — tek SKR (`SKR-022`, `survives → M2`,
  `4000-ontology/reviews/SKR-022-semantic-logic.md:16`).
- `0000-constitution/ENS-0000-constitution.md:12` — **sıfır** SKR; künyede `skeptic_review`
  alanı hiç yok. **MUAF** sayılır: Madde IV `:98-106` Anayasa'yı biricik kendi-kendini-
  yetkilendiren Grundnorm ilan eder ve yanlışlanabilirlik ödevini Madde XV'e (RFC+skeptic)
  devreder. RFC-6001 için SKR-034/035/036 + CEO-0002 + STYLE-SIGNOFF vardır → v0.3 değişikliği
  bu yolu **fiilen** kullanmıştır. Muafiyet ilkeli, ihlal değil.

**A-02 — `canon: true` ama doğrulama zinciri eksik/yok (G4 + maturity-model ihlali):**

| Yapıt | `canon` | `status` | `skeptic_review` | Sorun |
|---|---|---|---|---|
| `3000-laws/ENS-3000-laws.md:5` | **true** | `draft` (`:11`) | **`pending`** (`:16`) | **Sıfır validator ile `canon: true`.** Hem G4 (≥2) hem G4'ün en zayıf okuması (≥1) ihlal. Ayrıca `maturity-model.md:34` (*"`canon: true` yalnızca M5'tir"*) ve `KULLIYAT.md:31`'in kendi kaydı (`draft`) ile çelişir. |
| `4000-ontology/ENS-4000-glossary.md:5` | **true** | `review` (`:11`) | **alan yok** | **Sıfır validator ile `canon: true`.** RFC-6001 `:572` bunu bir "yan bulgu" olarak *kendisi tespit etmiş* ve §8.3 ile **açık borç** olarak kaydetmiştir (`:543`) — yani bilinen, kabul edilmiş, **kapatılmamış** ihlal. |
| `1000-philosophy/ENS-1000-manifesto.md:5` | **true** | `ratified` | `SKR-002` | 2 ardışık scientific tur; **farklı boyut yok** → G4 boyut-ihlali. `constitutive: true` olduğundan RFC-6001 ratifikasyon yolu geçerlidir, ama o yol da G4'ü kaldırmaz (RFC-6001 G4'ü değiştirmez). |

**A-03 — Standards katmanı: 11 `ratified`, 0 kayıtlı validator.**
`.claude/standards/` altındaki 11 dosyanın **hiçbirinde** `skeptic_review` künye alanı yoktur
(`architecture-principles.md:9`, `coding-standards.md:9`, `context-management.md:9`,
`documentation-style.md:9`, `evidence-standard.md:9`, `ens-phase-model.md:9`,
`language-policy.md:9`, `maturity-model.md:9`, `metadata-header.md:9`,
`validation-framework.md:9`, `traceability.md:9` — hepsi `status: ratified`).
`traceability.md:42` ve `metadata-header.md:44`'teki `skeptic_review:` satırları **örnek
blok içindedir**, o dosyaların kendi künyesi değildir.

Bu 11 dosya, Anayasa Madde XII (`:205-221`) uyarınca Külliyat'ın *altındaki* Standards
katmanıdır; G2/G3/G4 metinleri **Canonical yapıt** der ve Standards `canon: false`'tur.
Dolayısıyla harfiyen bir G4 ihlali **DEĞİLDİR**. Ancak bunlar **kapının kendisini tanımlayan**
belgelerdir (maturity-model, validation-framework, evidence-standard) — yani sistemin ölçme
aracı, hiçbir doğrulamadan geçmeden `ratified` sayılmıştır. Bu bir **yönetişim kör noktasıdır**
(bkz. Ç-02, §7).

### 2.3 Karşı-örnek: G4'ün fiilen sağlandığı yer (Faz 3)

Mimari/RFC hattı, teori hattından **yapısal olarak daha sıkıdır**:

| Yapıt | Doğrulama zinciri | Onay kaydı (ayrı rol) |
|---|---|---|
| `5000-architecture/adr/ADR-0001-agent-runtime.md:16` | `[SKR-024, SKR-026, SKR-029]` — 3 tur, hepsi `engineering` | `CEO-0001` (`ens-ceo`) |
| `5000-architecture/adr/ADR-0002-operations-capability.md:16` | `[SKR-025, SKR-027, SKR-037]` — 3 tur, `engineering` | `CEO-0003` (`ens-ceo`) |
| `6000-rfc/RFC-6001-constitutive-artifact-ayrimi.md:16` | `[SKR-034, SKR-035, SKR-036]` — 3 tur, `constitutional` | `CEO-0002` + `STYLE-SIGNOFF-RFC-6001` (`ens-style-guardian`) |

**RFC-6001 tek "tam uyumlu" yapıttır:** 3 bağımsız doğrulama turu **+ iki farklı rolden ayrı
onay kaydı** (`ens-ceo` hiza + `ens-style-guardian` şema-imzası). Bu, Madde XIV `:244-245`'in
("önemli kararlarda hiçbiri atlanmaz") tam uygulanmasıdır ve Külliyat'ın geri kalanı için
**mevcut, kanıtlanmış hedef desen**dir.

Not: ADR'ler yine de tek *boyutta* (engineering) doğrulanmıştır; G4'ün boyut şartı orada da
sayı ile karşılanmıştır, çeşitlilikle değil.

### 2.4 A denetimi sayısal özeti

- `ratified` Külliyat yapıtı: **9**
- G4'ü **sayı** olarak sağlayan (≥2 doğrulama turu): **6/9** (%67) — ENS-1000, 2001, 2002,
  3021, 3022, 3023
- G4'ü **boyut çeşitliliği** olarak sağlayan (≥2 *farklı* boyut): **0/9** (%0)
- Tek validator ile `ratified`: **2** (ENS-4030, ENS-4025); + **1 muaf** (ENS-0000)
- `canon: true` ama sıfır validator: **2** (ENS-3000, ENS-4000)
- Standards: **11 ratified / 0 kayıtlı validator** (kapsam dışı ama kör nokta)

## §3. Denetim B — G2 Uyumu (yazan ≠ onaylayan)

**G2** (`000-governance-principles.md:33`): *"Bir yapıtı yazan, onu Canonical yapamaz."*

### 3.1 Owner ↔ Validator ayrımı: TEMİZ

Tüm 45 SKR kaydının `owner:` alanı **`ens-skeptic`**tir (ör. `SKR-004:7`, `SKR-021:8`,
`SKR-037:8`, `SKR-039:8`). Hiçbir Külliyat yapıtının owner'ı `ens-skeptic` değildir:

| Owner | Yapıtlar |
|---|---|
| `ens-philosopher` | ENS-0000 `:13`, ENS-1000 `:12`, ENS-2001 `:13`, ENS-2002 `:13`, ENS-2003 `:13`, ENS-2004 `:13`, ENS-3000 `:12`, ENS-3021 `:13`, ENS-3022 `:13`, ENS-3023 `:13`, ENS-4025 `:12`, ENS-4030 `:12`, ENS-4031 `:12` |
| `ens-architect` | ENS-4001 `:12`, ENS-4010 `:12`, ADR-0001 `:12`, ADR-0002 `:12` |
| `ens-ai-architect` | ENS-4020 `:12` |
| `ens-style-guardian` | ENS-4000 `:12` |

**Sonuç: yazan ≠ doğrulayan ayrımı Külliyat genelinde ihlal edilmemiştir.** Bu, ENS'in en
sağlam yönetişim özelliğidir ve ROADMAP boyunca tekrarlanan *"⚠️ ÖZ-ONAY YOK (G2/G3)"*
işaretleriyle (ör. `ROADMAP.md:39`, `:97`) bilinçli olarak korunmuştur.

### 3.2 B-01 — Ancak "onaylayan" kim, kayıtlı değil

G2 iki farklı soru sorar: (a) yazan doğruladı mı? — **hayır, temiz**; (b) yazan **Canonical
yaptı** mı? Bunun cevabı için `status: draft/review → ratified` geçişini **kimin** yaptığının
kaydı gerekir.

**Faz 0-2 Külliyat yapıtlarının hiçbirinde böyle bir onay kaydı yoktur.** Ne künyede
(`approved_by` gibi bir alan `metadata-header.md`'de tanımlı değil), ne ayrı bir governance
kaydı olarak. Dosyayı yazma yetkisi olan tek rol `owner`dır; dolayısıyla statüyü fiilen
owner'ın çevirmiş olması **kuvvetle muhtemeldir** — ama bu **DOĞRULANMADI** (bu incelemede
`git log`/`git blame` çalıştırılabilecek bir araç yoktu; iddia fabrike edilmemiştir).

Kesin olan şudur: **onaylayan kayıtlı değildir.** Bu, doğrudan **G5** ihlalidir
(`000-governance-principles.md:36-37`: *"Her karar bir kayıt bırakır; **sessiz karar
yoktur**"*). 9 `ratified` Külliyat yapıtının **9'unda da** ratifikasyon edimi sessizdir.

### 3.3 B-02 — Standards katmanında bir G2-biçimli durum

`.claude/standards/validation-framework.md` — `owner: ens-skeptic` (`:10`),
`status: ratified` (`:9`), `skeptic_review` alanı **yok**. Doğrulama boyutlarını, SKR
formatını ve validator eşlemesini tanımlayan belge, **validator'ın kendisine** aittir ve
hiçbir dış incelemeden geçmeden `ratified`tir. `canon: false` olduğu için G2'nin harfi
ihlal edilmez, ama G1 (*"sorumlu olmayan yetki veremez"*) ve G7 açısından zayıf bir
konumdur: validator kendi yetkisinin kapsamını kendi tanımlamıştır.

Benzer biçimde `governance/000-governance-principles.md:12`, `roles.md:12`,
`canonical-process.md:12` — **hepsinin owner'ı `ens-philosopher`**, yani G2'nin en çok
kısıtladığı rol, G2'yi yazan roldür. **Bunlar `status: review` (M1) olduğundan** (`:11`)
bir ratifikasyon ihlali değildir — ENS bu belgeleri dürüstçe *henüz onaylanmamış* tutmuştur.
Bu, raporun bulabildiği **en dürüst** yönetişim davranışıdır.

### 3.4 B denetimi sayısal özeti

- Yazan = doğrulayan olan yapıt: **0/9** → G2'nin doğrulama ayağı **%100 temiz**
- Onaylayanı kayıtlı olan `ratified` Külliyat yapıtı: **0/9** (%0) → G2'nin *onay* ayağı
  **doğrulanamaz**, G5 ise kesin ihlal
- Onaylayanı kayıtlı olan Faz-3 yapıtı: **3/3** (ADR-0001→CEO-0001, ADR-0002→CEO-0003,
  RFC-6001→CEO-0002 + STYLE-SIGNOFF)

## §4. Denetim C — G3 Uyumu (doğrulayan ≠ onaylayan)

**G3** (`000-governance-principles.md:34`): *"Doğrulayan onaylamaz; onaylayan doğrulamaz."*

### 4.1 C-01 — Yapısal kök neden: **onaylayan rolü BOŞ**

`governance/roles.md:32` "Governance" rolünü tanımlar (*"Yalnızca son Canonical kapısını açar;
içerik üretmez"*), ama aynı dosyanın `:49` satırı bu rolün **atanmadığını** söyler:

> `Engineering/Business/Ethical Validator, Governance body: fazı gelince (ROSTER).`

Yani ENS'te **onay makamı mevcut değildir.** G3, bir ayrımı zorunlu kılar; ayrımın iki
tarafından biri boştur. Bu koşulda doğrulamanın onayın yerine geçmesi bir *ihmal* değil,
**yapının kaçınılmaz sonucudur.** Kusur bireysel değil, mimaridir.

### 4.2 C-02 — SKR verdict'i fiilen ratifikasyon kararını taşıyor

Doğrulama kayıtlarının kendisi canonization dilini kullanır:

- `3000-laws/reviews/SKR-012-decision-entropy.md:19` — *"**Kavram Külliyat'a girebilir
  (canon:true).**"*
- `3000-laws/reviews/SKR-014-decision-gravity.md:19-20` — aynı formül
- `3000-laws/reviews/SKR-016-decision-capital.md:19` — aynı formül
- `2000-theory/reviews/SKR-004-decision-theory.md:20`, `SKR-006:21`, `SKR-008:19`,
  `SKR-010:20` — aynı formül
- `1000-philosophy/reviews/SKR-002-manifesto.md:72` — *"Manifesto `ratified` edilebilir"*
- Ters yönde: `SKR-003:19`, `SKR-005:18`, `SKR-007:19`, `SKR-009:21`, `SKR-011:21`,
  `SKR-013:20`, `SKR-015:19` — *"Üç talep karşılanmadan `canon:true` olamaz."*

**Dilbilimsel savunma vardır:** "girebilir / edilebilir / olamaz" **izin** kipidir, edim değil
— SKR kapıyı *açık* ilan eder, kapıdan geçirmez. Bu, GOV-030 `:42-43`'ün ("her kapı bir
kanıttır, oy değil") tam istediği şeydir ve **doğru tasarımdır**.

**Ama pratikte edim, kayda geçmiş hâliyle SKR'ye bağlanmıştır.** `ROADMAP.md` statü
geçişlerini doğrudan SKR'nin sonucu olarak yazar:

- `ROADMAP.md:97` — *"**SKR-041 ... → `survives`.** ENS-2003 `status: review → ratified`"*
- `ROADMAP.md:97` — *"**SKR-044 ... → `survives`.** ENS-2004 `status: review → ratified`"*

Araya hiçbir onay edimi girmez. **Doğrulama, fiilen onayın yerine geçmiştir** —
`.claude/rules/work-protocol.md`'nin bu oturumda tekrarladığı hatanın Külliyat'taki
karşılığı **budur** ve tarama sorusunun cevabı **evet**tir.

### 4.3 C-03 — Külliyat bu kusuru **kendi kendine düzeltmeye başlamıştır**

Önemli karşı-kanıt: aynı SKR'ler `canon` kararını artık **açıkça kendilerinden ayırıyor**:

- `4000-ontology/ENS-4031-inference-rules.md:16` — *"`ratified`/`canon:true` **AYRI governance
  edimi**"*
- `4000-ontology/ENS-4010-foundational-ontology.md:17` — *"`canon:false` korunur;
  `ratified`/`canon` **ayrı governance edimi**"*
- `ROADMAP.md:95` (K3/ENS-4020) — *"`status: review` (**ratified ayrı governance edimi**)"*
- `ROADMAP.md:97` (SKR-041) — *"**`canon: false` KALIR** — Külliyat-girişi ayrı governance
  edimi; SKR-041 yalnızca skeptic-kapısını işaretler."*

Yani **yeni yapıtlar** (ENS-4010, ENS-4020, ENS-4031) G3'e **uygun** davranmaktadır:
skeptic kapısı geçilir, `status: skeptic-cleared` verilir, `ratified`/`canon` **bekletilir**.
`skeptic-cleared` statüsü, tam da G3'ün gerektirdiği ara duraktır.

**Sorun eski yapıtlarda (Faz 0-2 mirası) kalmıştır:** ENS-1000, 2001, 2002, 3021, 3022,
3023, 4025, 4030 `ratified`e SKR verdict'i ile geçmiş ve **hiç geri dönülmemiştir**.
Bu bir **yönetişim borcudur**, aktif bir kural ihlali değil.

### 4.4 C denetimi sayısal özeti

- Ayrı onay kaydı bulunan `ratified` Külliyat yapıtı: **0/9** (%0)
- SKR verdict'inde canonization dili (`canon:true` / `ratified edilebilir`) bulunan SKR: **8**
  (SKR-002, 004, 006, 008, 010, 012, 014, 016)
- G3'e uygun (`skeptic-cleared` / "ayrı governance edimi") davranan yapıt: **3** (ENS-4010,
  ENS-4031 `skeptic-cleared`; ENS-4020 `review` + açık not) — **düzelme yönü doğru**
- Onay makamı (Governance body) atanmış mı: **HAYIR** (`roles.md:49`)

## §5. Denetim D — Boyut Çeşitliliği

**GOV-030 `:44`**: *"G4: ≥2 **bağımsız boyut** validator'ı (ör. Scientific + Ontology, uygunsa
+ Engineering)."* — sayı ve boyut **ayrı** koşullardır.

### 5.1 D-01 — 45 doğrulama kaydının 45'i de aynı agent'a ait

`**/reviews/SKR-*.md` altındaki **45 kaydın tamamının** `owner:` alanı `ens-skeptic`tir.
İstisna yoktur. Farklı rol tarafından yazılmış doğrulama/onay kayıtları yalnızca dört tanedir
ve hiçbiri bir Külliyat yapıtına bağlı değildir:
`CEO-0001:7`, `CEO-0002:7`, `CEO-0003:7` (`ens-ceo`, hiza incelemesi) ve
`STYLE-SIGNOFF-RFC-6001:11` (`ens-style-guardian`, şema imzası).

Bu, `governance/roles.md:47`'nin **açık tasarım kararıdır**: *"Scientific + Ontology Validator:
`ens-skeptic`."* Yani beş validator boyutundan **ikisi tek role** verilmiştir. Sonuç:
"iki farklı boyut" koşulu, ENS'in mevcut rol tahsisinde **tek bir agent tarafından**
sağlanabilir hâle gelmiştir — G4'ün *bağımsızlık* amacı (farklı bakış açısı, farklı kör nokta
kümesi) bu tahsisle **yapısal olarak zayıflatılmıştır**.

### 5.2 D-02 — `validation_dimension` dağılımı

| Boyut | SKR'ler | Adet |
|---|---|---|
| *(alan yok — `validation-framework.md:36`'ya göre örtük **scientific**)* | SKR-001…016, SKR-040…045 | **22** |
| `ontology` | SKR-017, 018, 019, 020, 021, 022, 023, 028, 030, 031, 032, 038, 039 | **13** |
| `engineering` | SKR-024, 025, 026, 027, 029, 037 | **6** |
| `constitutional` | SKR-034, 035, 036 | **3** |
| `scientific` (açık) | SKR-033 | **1** |
| `business` | — | **0** |
| `ethical` | — | **0** |

**Toplam 45.**

**D-02a — Şema sürüklenmesi.** `validation-framework.md:37` izin verilen değerleri sayar:
`scientific | ontology | engineering | business | ethical`. **`constitutional` bu listede
yoktur.** SKR-034/035/036 (`:10`) tanımsız bir boyut değeri kullanmaktadır. RFC-6001'in kendi
doğrulaması, doğrulama şemasının dışına çıkmıştır. Küçük ama gerçek bir kusur — düzeltme ya
`validation-framework.md`'ye 6. boyut eklemeyi ya da bu üçünü `scientific`e eşlemeyi gerektirir
(hangisi doğru, bu raporun kararı değildir).

**D-02b — Hiç kullanılmayan iki boyut.** `business`: `validation-framework.md:28` onu Faz 5'e
atar → 0 kayıt **beklenen** ve doğru. `ethical`: `:29` onu **"tüm fazlar"** olarak işaretler →
0 kayıt **bir boşluktur**. Ethical Validation, kendi standardına göre bugün aktif olması
gereken tek dördüncü boyuttur ve hiç uygulanmamıştır. `roles.md:49` bunu "fazı gelince"
diyerek erteler — **iki belge birbiriyle çelişir** (Ç-04).

### 5.3 D-03 — Hiçbir yapıt iki farklı boyuttan doğrulanmamıştır

Her yapıtın doğrulama zinciri **tek boyut** içindedir:

| Yapıt | Zincir | Boyut kümesi | Boyut sayısı |
|---|---|---|---|
| ENS-1000 | SKR-001→002 | {scientific} | **1** |
| ENS-2001 | SKR-003→004, 033 | {scientific} | **1** |
| ENS-2002 | SKR-005→006 | {scientific} | **1** |
| ENS-2003 | SKR-007→008, 040→041, 045 | {scientific} | **1** |
| ENS-2004 | SKR-009→010, 042→043→044 | {scientific} | **1** |
| ENS-3021/3022/3023 | SKR-011→012 / 013→014 / 015→016 | {scientific} | **1** |
| ENS-4001 | SKR-017→018, 023 | {ontology} | **1** |
| ENS-4010 | SKR-019→020, 038→039 | {ontology} | **1** |
| ENS-4020 | SKR-028, 030 | {ontology} | **1** |
| ENS-4025 | SKR-022 | {ontology} | **1** |
| ENS-4030 | SKR-021 | {ontology} | **1** |
| ENS-4031 | SKR-031→032 | {ontology} | **1** |
| ADR-0001 | SKR-024, 026, 029 | {engineering} | **1** |
| ADR-0002 | SKR-025, 027, 037 | {engineering} | **1** |
| RFC-6001 | SKR-034, 035, 036 | {constitutional} | **1** |

**Boyut çeşitliliği sağlayan yapıt sayısı: 0 / 15.**

### 5.4 D-04 — "Bağımsızlık" fiilen *tur* bağımsızlığıdır, *boyut* bağımsızlığı değil

ENS'in uyguladığı bağımsızlık kavramı tutarlı ve **gerçek**tir: yeni tur, **taze context**te,
`ens-skeptic`in önceki turu görmediği bir oturumda yapılır. ROADMAP bunu her seferinde
kaydeder: *"SKR-041 (bağımsız 2. tur, **taze context**, G2/G3)"* (`ROADMAP.md:97`),
*"SKR-039 bağımsız 2. tur (taze context, G2/G3)"* (`ROADMAP.md:139`), *"SKR-037 (bağımsız,
G2/G3)"* (`ADR-0002:16`).

Bu, gerçek bir kontroldür ve **işe yaradığı kanıtlanmıştır**: `ROADMAP.md:84-86` —
*"G2/G3 şüphesi doğrulandı: inline SKR-024/025 'kötü değil ama eksikti' — **bağımsız göz
zincir-dışına bakıp gerçek yapısal sorunlar buldu**."*

**Ama context bağımsızlığı ≠ boyut bağımsızlığı.** Aynı agent, aynı checklist
(`validation-framework.md:40-46` Scientific: yenilik/yanlışlanabilirlik/kanıt/varsayım/
karşı-argüman/iç tutarlılık), aynı sistematik kör noktalar. Bunun somut kanıtı Külliyat'ın
kendi kaydındadır:

> `ROADMAP.md:19` — *"**SKR-040 ve SKR-041'in ikisi de bunu kaçırdı**"* — ENS-2003 §3a'daki
> `c` çift-sayım hatası **iki ardışık bağımsız scientific turdan da geçti** ve ancak
> **farklı boyuttan** bir denetimle (`7000-reference-implementation/AUDIT-WAVE2-FIDELITY.md`,
> TRACE sadakat denetimi = fiilen bir *engineering* validation) yakalandı.

**Bu, G4'ün "farklı boyutlardan" şartının neden yazıldığının ampirik kanıtıdır** — ve o şartın
sağlanmamasının **gerçek bir teori hatasına** mal olduğunun kaydıdır. D bulgusu spekülatif
değildir; Külliyat kendi vakasını kendi belgelemiştir.

### 5.5 D denetimi sayısal özeti

- Toplam doğrulama kaydı: **45 SKR** + 3 CEO + 1 STYLE-SIGNOFF = **49**
- `ens-skeptic` tarafından yazılan SKR: **45/45** (%100)
- ≥2 farklı `validation_dimension` ile doğrulanmış yapıt: **0/15** (%0)
- Tanımsız boyut değeri kullanan SKR: **3** (`constitutional`)
- Kendi standardına göre aktif olması gereken ama hiç uygulanmamış boyut: **1** (`ethical`)
- İki bağımsız aynı-boyut turunun birlikte kaçırdığı, farklı boyutta yakalanan kusur: **≥1**
  (ENS-2003 D-5, `ROADMAP.md:19`) — **G4 boyut şartının maliyeti ölçülmüştür**

## §6. Denetim E — Faz Kapıları

**Yetki:** Anayasa Madde VII (`ENS-0000:137-152`) — *"Kapılı fazlar; bir kapı ancak çıkış
ölçütleri kaydedilince açılır. Erken bir fazda bulunan bir kusur, yukarı akışta düzeltilene
dek bağımlı sonraki işi **durdurur**."*
Ayrıntı: `.claude/standards/ens-phase-model.md`.

### 6.1 E-01 — Kapı kuralının aracı **mevcut değil** (en ağır bulgu)

`ens-phase-model.md:73-76` — **Kapı kuralı**:
> *"Herhangi bir kapıyı geçmeden önce **`/validate-theory`** çalıştır."*

Ayrıca `:56` (Faz 3 çıkış koşulu) *"`/validate-theory` **geçer**"* der ve Anayasa Madde VIII
`:163` izlenebilirlik denetimini aynı komuta bağlar.

**`.claude/commands/` dizini repoda YOKTUR.** (`.claude/` altındaki tüm içerik tarandı:
`agents/`, `hooks/`, `rules/`, `skills/`, `standards/`, `settings*.json` — `commands/` yok.)
Anayasa Madde XII `:216` bağımlılık grafiğinde `Commands (.claude/commands)` katmanını
açıkça listeler; **bu katman boştur.**

**Sonuç: bugüne kadar geçilen hiçbir faz kapısında `/validate-theory` çalıştırılmamıştır** —
çünkü çalıştırılamazdı. Kapı ölçütü tanımlıdır, ölçme aracı yoktur. Bu, faz kapılarının
**tamamının** ölçülmeden geçildiği anlamına gelir; tek tek fazların içeriğine bakmaya gerek
kalmadan E denetiminin sonucu bu tek bulguyla belirlenir.

İlgili: `formal-checker` (Ontology Linter, `validation-framework.md:50`) bir agent olarak
hiç yazılmamıştır; yerine `tools/ens-ontology-linter/` deterministik aracı yazılmış ama
**canlı koşusu owner/CI teyidi beklemektedir** (`ROADMAP.md:222`, G-09/10).

### 6.2 E-02 — Kapı kapı durum

| Kapı | Çıkış ölçütü (`ens-phase-model.md`) | Fiili durum | Karar |
|---|---|---|---|
| **Faz 0 → 1** | `:37` Anayasa onaylı; sözlük tohumlanmış; anti-pattern'ler adlandırılmış | Anayasa `ratified` ✓ (`ENS-0000:12`). Sözlük var ama `status: review` (`ENS-4000:11`) + hak edilmemiş `canon: true` (§2.2/A-02). Anti-pattern'ler Madde VI `:128-135`'te adlandırılmış ✓, ama `:135`'in vaat ettiği *"Tam kayıt `4000-ontology/` altında"* **dosya olarak yok** (dizinde yalnızca ENS-4000/4001/4010/4020/4025/4030/4031). | **KISMİ — geçildi** |
| **Faz 1 → 2** | `:45-46` Her kavram belgesi skeptic-incelenmiş + failure conditions + çözülmemiş terminoloji çatışması yok | `:41-44` **11 kavram** sayar; `2000-theory/` altında **4** belge var (ENS-2001..2004). Yazılmamış: Enterprise Physics, Company Consciousness, Decision Velocity, Decision Energy, Trust Theory, Enterprise Intelligence. Decision Capital/Entropy Faz-2 dizinine (`3000-laws/`) yerleşmiş — faz-atama sürüklenmesi. **Bugün:** ENS-2003 `status: review` (`:12`) ve ENS-2004 `status: review` (`:12`) — ikisi de doğrulanmamış revizyon taşıyor; `ROADMAP.md:39` *"**BORÇ 1 — bağımsız skeptic turu (BLOCKING)** … v0.4.0 `survives` DEĞİLDİR; hiçbir skeptic turu görmemiştir."* | **AÇIK — ama Faz 2/3/4 çalışıyor** |
| **Faz 2 → 3** | `:51` Her model ifade edilebilir, yanlışlanabilir, Faz 1'e izli | `ENS-3000:11` `status: draft`, `:16` `skeptic_review: pending` — Faz-2'nin **kayıt belgesi** hâlâ draft ve doğrulanmamış. `ENS-4001:16` `skeptic_review: [SKR-023, **pending**]`. `ROADMAP.md:63` — *"ENS-4001 v0.6, ENS-4010 — **Yazıldı, henüz Ontology Validation'dan geçmedi**"*. | **AÇIK — ama Faz 3/4 çalışıyor** |
| **Faz 3 → 4** | `:56` Teori atfı olmayan ADR yok; **`/validate-theory` geçer** | Teori atfı: 2 ADR de atıflı ✓ (ADR-0001 `:16`, ADR-0002 `:16`, ikisi de `accepted`). `/validate-theory`: **komut yok** (E-01) → **ölçülemedi**. Ayrıca `:54-55`'in istediği C4 diyagramları, Bounded Context'ler, Context Map, Event Model, Decision Graph, Memory Graph, Service Boundaries — `5000-architecture/` altında **hiçbiri yok** (yalnızca `adr/` 2 dosya + `reviews/` 9 dosya). | **ÖLÇÜLMEDEN geçildi** |
| **Faz 4** | `:61` Kanıt çalışıyor; `kod → ADR → theory` zinciri tam | Faz 4 **açık ve aktif** (`ROADMAP.md:228`). Çıkış iddia edilmiyor → ihlal değil. Ancak `ROADMAP.md:46-52`: *"**BORÇ 2 — `dotnet build` / `dotnet test` HİÇ ÇALIŞTIRILMADI (BLOCKING)**"* — son dalga statik hizalı, koşulmamış. | **Açık (doğru), borçlu** |
| **Faz 5 / 6** | `:67`, `:71` | `8000-product/` ve `9000-book/` dizinleri **yok** → fazlar açılmamış | **✓ DOĞRU** |

### 6.3 E-03 — Sıra ihlali: Faz 1 kapısı açıkken Faz 4 çalışıyor

Madde VII `:139-140` açıktır: *"Erken bir fazda bulunan bir kusur, yukarı akışta düzeltilene
dek bağımlı sonraki işi **durdurur**."*

Bugünkü durum (`ROADMAP.md:12-52`):
1. Faz-4 denetimi (`7000-reference-implementation/AUDIT-WAVE2-FIDELITY.md`, bulgu **D-5**)
   ENS-2003 §3a'da **gerçek bir teori hatası** buldu (`c` çift-sayımı).
2. ENS-2003 `v0.3.1 → v0.4.0 (BREAKING)`, `status: ratified → review` (`ROADMAP.md:22`).
3. ENS-2004 `v0.3.2 → v0.3.3`, `status: ratified → review` (`ROADMAP.md:29`).
4. **Aynı turda** Faz-4 kodu (`CompanyMemory.cs`, `CompanyMemoryTests.cs`, 5 dosya) bu
   **henüz doğrulanmamış** teoriye göre yeniden yazıldı (`ROADMAP.md:33-37`).

Yani Faz-1 kapısı `review`e geri düşmüşken Faz-4 işi devam etti ve doğrulanmamış teoriye
bağlandı. Madde VII'nin lafzına göre bu bir **kapı ihlalidir**.

**Karşı-argüman kayda geçirilmelidir (dürüstlük gereği).** Aynı vaka, kapı modelinin
*doğrusal* okumasının ENS'in fiili değer üretimiyle çeliştiğini gösterir: **teori hatasını
bulan şey Faz-4'ün kendisiydi.** P8 (*"implementation teoriyi kanıtlar"*) tam da bunu ister;
Faz 4 durdurulsaydı D-5 bulunmazdı. Düzeltmenin **yönü de doğruydu** — önce teori (ENS-2003
v0.4.0), sonra kod. İhlal edilen tek şey, teori düzeltmesinin **doğrulanmasını beklememektir**.

Bu, `ens-phase-model.md`'nin kapı modeli ile ENS'in fiili döngüsel (theory ⇄ reference-proof)
çalışma biçimi arasındaki **çözülmemiş bir mimari gerilimdir**. Çözümü bu raporun kararı
değildir; **RFC gerektirir** (bkz. §7 Ö-04).

### 6.4 E-04 — Rol-faz eşlemesi korpusla uyuşmuyor

`ens-phase-model.md:27-33` faz sahibi agent'ları sayar. Bunlardan **`ens-chief-architect`,
`ens-domain-modeler`, `ens-rfc-writer`, `ens-book-author`** `.claude/agents/` altında
**yoktur**. Dahası `ens-ceo` — Madde XIV `:244`'ün önemli kararlarda atlanamaz saydığı ve
CEO-0001/0002/0003 kayıtlarının owner'ı olan rol — **`.claude/agents/` altında bir dosyaya
sahip değildir**. Faz sahipliği, var olmayan rollere atanmıştır.

(Bu bulgu `SCAN-01`/`SCAN-02`'nin alanına da girer; burada yalnızca **faz kapısı sahipliği**
bağlamında kayda geçirilmiştir.)

### 6.5 E denetimi sayısal özeti

- Tanımlı faz kapısı (Faz 0→1 … 4→5): **5**
- Çıkış ölçütü **tam** karşılanarak geçilen kapı: **0/5** (%0)
- `/validate-theory` ile ölçülen kapı: **0/5** (%0) — araç mevcut değil
- Önceki fazın kapısı açıkken açılmış faz: **≥2** (Faz 1 açıkken Faz 2/3/4; Faz 2 açıkken Faz 3/4)
- Doğru biçimde **açılmamış** faz: **2** (Faz 5, Faz 6) ✓
- Faz sahibi olarak atanmış ama mevcut olmayan agent: **5** (`ens-chief-architect`,
  `ens-domain-modeler`, `ens-rfc-writer`, `ens-book-author`, `ens-ceo`)

## §7. Sayısal Özet, Normatif Çatışmalar ve Öneriler

### 7.1 Tek tabloda tüm sayılar

| Ölçüt | Değer | Oran |
|---|---|---|
| `status: ratified` dosya (toplam) | 36 | — |
| — bunlardan Külliyat yapıtı | **9** | — |
| — bunlardan `.claude/standards/` | 11 | — |
| — bunlardan SKR/CEO kaydı | 16 | — |
| `canon: true` gerçek yapıt | **4** (ENS-0000, ENS-1000, ENS-3000, ENS-4000) | — |
| Toplam doğrulama kaydı | 45 SKR + 3 CEO + 1 STYLE-SIGNOFF = **49** | — |
| **G4 sayı** — ≥2 doğrulama turu geçirmiş `ratified` Külliyat yapıtı | **6/9** | **%67** |
| **G4 boyut** — ≥2 *farklı* boyuttan doğrulanmış yapıt (tüm korpus) | **0/15** | **%0** |
| Tek validator ile `ratified` | **2** (ENS-4030, ENS-4025) + 1 muaf (ENS-0000) | — |
| `canon: true` + sıfır validator | **2** (ENS-3000, ENS-4000) | — |
| `ratified` standard / kayıtlı validator'ı olan | **11 / 0** | **%0** |
| **G2 doğrulama ayağı** — yazan = doğrulayan olan yapıt | **0/9** | **%0 ihlal ✓** |
| **G2/G5 onay ayağı** — onaylayanı kayıtlı `ratified` Külliyat yapıtı | **0/9** | **%0** |
| Onaylayanı kayıtlı Faz-3 yapıtı (ADR/RFC) | **3/3** | **%100 ✓** |
| **G3** — ayrı onay makamı atanmış mı | **HAYIR** (`roles.md:49`) | — |
| `ens-skeptic` tarafından yazılan SKR oranı | **45/45** | **%100** |
| Hiç kullanılmamış validation dimension | 2 (`business` beklenen, `ethical` **boşluk**) | — |
| Tanımsız dimension değeri kullanan SKR | 3 (`constitutional`) | — |
| Faz kapısı — çıkış ölçütü tam karşılanarak geçilen | **0/5** | **%0** |
| Faz kapısı — `/validate-theory` ile ölçülen | **0/5** | **%0** (araç yok) |

### 7.2 Bulgu kaydı (ID / sebep / etki / öncelik)

| ID | Bulgu | Sebep | Etki | Öncelik | Bağımlılık |
|---|---|---|---|---|---|
| **E-01** | `/validate-theory` komutu yok; `.claude/commands/` dizini boş | Kapı ölçütü tanımlandı, aracı hiç yazılmadı | Faz kapılarının **hiçbiri** ölçülmedi; Madde VIII izlenebilirlik denetimi hiç koşmadı | **P0** | `tools/ens-ontology-linter/` V1 (kısmi karşılık) |
| **C-01** | Onay makamı (Governance body) **atanmamış** | `roles.md:49` "fazı gelince" | G3 yapısal olarak sağlanamaz; doğrulama onayın yerine geçmek **zorunda** | **P0** | ROSTER; `ens-ceo` agent dosyası yok (E-04) |
| **A-02a** | `ENS-3000:5` `canon: true` + `status: draft` + `skeptic_review: pending` | Faz-0 mirası, GOV-030 `:54-55` demotion listesinde yer almadı | Sıfır validator'lı `canon` — G4'ün en zayıf okumasını bile ihlal; `maturity-model.md:34` ile çelişir | **P0** | GOV-030 re-grading yetkisi (`:50-53`) zaten var |
| **A-02b** | `ENS-4000:5` `canon: true` + `status: review` + `skeptic_review` alanı yok | RFC-6001 `:572`'de tespit edildi, §8.3'te **açık borç** bırakıldı | Sözlük — tüm terminolojinin kaynağı — doğrulanmamış canon | **P0** | RFC-6001 §8.3; owner `ens-style-guardian` |
| **D-03** | Hiçbir yapıt ≥2 farklı boyuttan doğrulanmadı (0/15) | 5 validator boyutundan 2'si tek agent'a verilmiş (`roles.md:47`); diğer 3'ü atanmamış | G4'ün *amacı* (farklı kör nokta kümesi) hiç sağlanmadı; maliyeti ölçüldü (ENS-2003 D-5, `ROADMAP.md:19`) | **P1** | C-01, ROSTER genişletmesi |
| **A-01** | ENS-4030 ve ENS-4025 tek SKR ile `ratified` | Ontology hattında wounded-tur oluşmadı, tek turda survives | G4 sayı-ihlali; iki `constitutive`/tip-sistemi belgesi tek gözle geçti | **P1** | — |
| **B-01** | 9/9 `ratified` yapıtta ratifikasyon edimi **sessiz** | `metadata-header.md`'de `approved_by`/`ratified_by` alanı yok | **G5 ihlali** ("sessiz karar yoktur"); G2'nin onay ayağı denetlenemez | **P1** | `metadata-header.md` şema değişikliği |
| **E-03** | Faz 1/2 kapısı açıkken Faz 3/4 çalışıyor | Kapı modeli ile fiili döngüsel çalışma biçimi uyuşmuyor | Madde VII lafzı ihlal; ama P8 değeri üretiliyor — **gerilim, basit ihlal değil** | **P1** | RFC gerekir (Ö-04) |
| **A-03** | 11 standard `ratified`, 0 kayıtlı validator | `ROADMAP.md:219` (G-04) künye ekledi, doğrulama eklemedi | Kapıyı **tanımlayan** belgeler (maturity/validation/evidence) hiç sınanmadı | **P2** | `canon:false` olduğundan harfiyen G4 dışı |
| **D-02a** | `constitutional` tanımsız dimension değeri (SKR-034/035/036) | RFC doğrulaması şemanın dışına çıktı | Şema sürüklenmesi; dimension sayımı güvenilmez | **P2** | `validation-framework.md:37` |
| **D-02b** | `ethical` boyut hiç uygulanmadı | `validation-framework.md:29` "tüm fazlar" ↔ `roles.md:49` "fazı gelince" çelişkisi | P7 (insan kontrolü) hiç bağımsız sınanmadı | **P2** | Ç-04 |
| **E-04** | 5 faz-sahibi agent mevcut değil (`ens-ceo` dahil) | ROSTER/standard sürüklenmesi | Kapı sahipliği boşta; CEO-000N kayıtlarının rolü dosyasız | **P2** | SCAN-01/02 kapsamı |

### 7.3 Normatif çatışmalar (kimin haklı olduğu bu raporun kararı DEĞİL)

| ID | Çatışma | Taraflar |
|---|---|---|
| **Ç-01** | `ratified` için kaç validator gerekir? | `maturity-model.md:28` (M2 = **≥1 SKR**) ↔ `canonical-process.md:44` (G4 = **≥2 farklı boyut**) |
| **Ç-02** | G2/G3/G4 yalnızca **Canonical (M5)** için mi, her statü geçişi için mi? | `000-governance-principles.md:33-35` ("Canonical" der) ↔ `roles.md:38-39` + fiili pratik (`ROADMAP.md:39`, koşulsuz uygular) |
| **Ç-03** | M5 hangi boyutları ister? | `maturity-model.md:31` (**Sci+Eng+Biz+Ethical** — Ontology **yok**) ↔ `canonical-process.md:24-39` (Sci+**Ontology**+Eng zinciri) |
| **Ç-04** | Ethical Validation ne zaman aktif? | `validation-framework.md:29` (**tüm fazlar**) ↔ `roles.md:49` (**fazı gelince**) |
| **Ç-05** | `constitutive: true` canon yolu G4'ten muaf mı? | RFC-6001 (ratifikasyon yolu, `KULLIYAT.md:11-14`) G4'e **hiç değinmez** → muafiyet ne verilmiş ne reddedilmiştir |

### 7.4 Öneriler — **hepsi ADR/RFC gerektirir, hiçbiri bu raporla yürürlüğe girmez**

> **Yetki sınırı beyanı.** `ens-architect` olarak yetkim `5000-architecture/` ve mimari
> ADR'lerdir. Aşağıdakiler **governance** ve **Külliyat statüsü** alanına girdiğinden
> **ben uygulayamam**; her biri ilgili owner + RFC/ADR yordamı ister (Madde XIV, Madde XV).

| Ö | Öneri | Gerekli edim | Owner |
|---|---|---|---|
| **Ö-01** | Ç-01 ve Ç-02'yi çöz: G4'ün kapsamını (`ratified` mi yalnızca M5 mi) ve `ratified` için asgari validator sayısını **tek yerde** yaz | **RFC** (GOV-000 + maturity-model birlikte, atomik — RFC-6001 emsali) | `ens-philosopher` + `ens-ceo` |
| **Ö-02** | E-01: `/validate-theory` komutunu yaz ya da kapı ölçütünü mevcut araca (`tools/ens-ontology-linter/`) bağla | **ADR** (araç mimarisi) + `ens-phase-model.md` güncellemesi | `ens-architect` (araç), `ens-ceo` (kapı ölçütü) |
| **Ö-03** | C-01: onay makamını **ata** — yoksa G3 hiç sağlanamaz. En basit çözüm: mevcut çift-owner kapısını (`ens-ceo` hiza + `ens-style-guardian` şema-imzası, RFC-6001 §7.5) **Külliyat ratifikasyonuna da genişlet** — desen zaten kanıtlanmış | **RFC** (GOV-010 rol ataması) | `ens-philosopher` + `ens-ceo` |
| **Ö-04** | E-03: kapı modelini fiili döngüsel çalışmayla uzlaştır — ya kapılar "faz-kapanışı" değil "yapıt-kapısı" olarak yeniden tanımlansın, ya Faz-4 işi teori `review`e düştüğünde dursun | **RFC** (Madde VII yorumu ya da `ens-phase-model.md` revizyonu) | `ens-ceo` |
| **Ö-05** | B-01/G5: künye şemasına `ratified_by` (+ `ratified_at`) alanı ekle; ratifikasyon edimi sessiz kalmasın | **RFC** (`metadata-header.md` v0.3) | `ens-style-guardian` |
| **Ö-06** | A-02a/A-02b: `ENS-3000` ve `ENS-4000`'in `canon: true`'sunu **demote** et ya da kurucu-tutarlılık incelemesiyle kazandır. GOV-030 `:50-53` demotion'ı **Custodian'a zaten yetkilendirmiştir** (G2 kısıtı canonization içindir) → RFC gerekmez | Custodian edimi + kayıt | `ens-style-guardian` |
| **Ö-07** | D-03: ROSTER'a **Ontology Validator**'ı `ens-skeptic`ten ayrı bir role ver (`ens-architect` doğal aday, ENS-4001/4010 owner'ı olduğu için **kendi yapıtlarında değil**) | **RFC** (GOV-010) | `ens-philosopher` + `ens-ceo` |
| **Ö-08** | `.claude/rules/work-protocol.md`'yi GOV-000/GOV-030 ile hizala (§0'daki tetikleyici bulgu) — operasyonel kural yönetişimden gevşek olamaz (Madde XV üstünlük) | Kural düzeltmesi (`rules/` yönetişime tabi) | Kural owner'ı |

### 7.5 Külliyat bu bulguların bir kısmını **zaten biliyor**

Dürüstlük gereği kayda geçirilmelidir: bu taramanın merkezî bulgusu Külliyat'ın kendi açık-iş
kaydında **zaten vardır**:

> `ROADMAP.md:224` — **`G-16 | Governance tek-operatör (rol ayrımı G2/G3 fiilen zayıf) | P3`**

Bulgu yeni değildir; **önceliklendirmesi** tartışmalıdır. Bu rapor, G-16'nın **P3'ten P0'a**
taşınmasını önerir — gerekçe: G-16, E-01 ve C-01 ile birlikte diğer **tüm** kapı ölçütlerinin
kök nedenidir. Öncelik değişikliği bir governance kararıdır; bu rapor onu **öneremez ancak
dayatamaz**.

## §8. Verdict

### Külliyat kendi yönetişim kapısından: **KISMEN GEÇİYOR**

Üç eksende ayrı ayrı:

| Eksen | Verdict | Gerekçe |
|---|---|---|
| **G2 — yazan ≠ doğrulayan** | **GEÇİYOR** | 45/45 SKR `ens-skeptic`e, 0/9 Külliyat yapıtı `ens-skeptic`e ait. Öz-onay yasağı ROADMAP boyunca bilinçle korunmuş. ENS'in en sağlam yönetişim özelliği. |
| **G2/G5 — onay edimi kaydı** | **GEÇMİYOR** | 0/9 `ratified` yapıtta onaylayan kayıtlı. G5'in *"sessiz karar yoktur"* kuralı 9 kez ihlal edilmiş. Faz-3'te (3/3) tam karşılanmış olması, eksikliğin ihmal değil **kapsam boşluğu** olduğunu gösterir. |
| **G3 — doğrulayan ≠ onaylayan** | **GEÇMİYOR (yapısal)** | Onay makamı **atanmamış** (`roles.md:49`). SKR verdict'i fiilen ratifikasyonu tetikliyor (`ROADMAP.md:97`). Kusur bireysel değil mimari: ayrımın bir tarafı boş. **Ama düzelme yönü doğru** — ENS-4010/4020/4031'de `skeptic-cleared` + *"ayrı governance edimi"* deseni G3'e uygun. |
| **G4 — sayı** | **KISMEN** | 6/9 (%67) ≥2 tur geçmiş; 2 yapıt tek validator ile `ratified`; 2 yapıt **sıfır** validator ile `canon: true`. |
| **G4 — boyut çeşitliliği** | **GEÇMİYOR** | **0/15 (%0).** Hiçbir yapıt iki farklı boyuttan doğrulanmadı. Maliyeti **ölçülmüştür**: ENS-2003 D-5 çift-sayım hatası iki ardışık scientific turdan geçip ancak Faz-4 audit'inde (fiilen engineering boyutu) yakalandı (`ROADMAP.md:19`). |
| **E — faz kapıları** | **GEÇMİYOR** | 0/5 kapı çıkış ölçütü tam karşılanarak geçilmedi; 0/5 kapıda `/validate-theory` koştu — **komut mevcut değil**. Faz 5/6'nın doğru biçimde açılmamış olması tek olumlu kayıt. |

### §0'ın sorusuna doğrudan cevap

**Evet — `.claude/rules/work-protocol.md`'nin tek-kapı hatası, Külliyat'ta bir emsale
dayanıyor.** Külliyat'ın **hiçbir** yapıtı G4'ün boyut şartını sağlamamıştır ve `ratified`
statüsü fiilen tek doğrulama otoritesinin (`ens-skeptic`) verdict'ine bağlanmıştır.
`work-protocol.md` yeni bir gevşeklik icat etmedi; **var olan pratiği kodifiye etti.**

Bu, `work-protocol.md`'yi haklı çıkarmaz — GOV-000/GOV-030 yazılı norm olarak üstündür
(Madde XV) ve norm ile pratik çatıştığında düzeltilmesi gereken **pratiktir**. Ama sorunun
kaynağını doğru yere koyar: kusur bir kural dosyasında değil, **atanmamış bir onay makamı ve
yazılmamış bir kapı aracındadır**.

### Neden "geçmiyor" değil "kısmen geçiyor"

Üç ağırlaştırıcı ve üç hafifletici birlikte tartılmıştır.

**Ağırlaştırıcı:** (1) İki yapıt (`ENS-3000`, `ENS-4000`) **sıfır** doğrulama ile `canon: true`
taşıyor — sözlük dahil. (2) Kapı aracı hiç yazılmamış; hiçbir faz kapısı ölçülmemiş.
(3) Onay makamı atanmadığı için G3 **sağlanamaz** durumda.

**Hafifletici:** (1) G2'nin doğrulama ayağı %100 temiz — ENS öz-onaydan sistematik olarak
kaçınmıştır ve bunu her seferinde kayda geçirmiştir. (2) `skeptic-cleared` statüsü ve
*"ratified/canon ayrı governance edimi"* notu, G3'e uygun **doğru deseni** yeni yapıtlarda
zaten uygulamaktadır. (3) RFC-6001 (3 doğrulama turu + `ens-ceo` hiza + `ens-style-guardian`
şema-imzası) **tam uyumlu bir emsal** üretmiştir — çözüm icat edilmesi gerekmiyor,
**genelleştirilmesi** gerekiyor.

Külliyat, kendi kapısından **düşmemiştir**; kapının **bir kanadını hiç takmamıştır**.
Boyut çeşitliliği ve onay makamı eksik olduğu sürece kapı, tasarlandığı işi göremez.

---

**Denetim sınırları (dürüstlük beyanı):**
- `git log` / `git blame` bu inceleme bağlamında **çalıştırılamadı** (shell aracı yoktu).
  Bu nedenle *"statüyü kim çevirdi"* sorusu **DOĞRULANMADI** — §3.2'de çıkarım olarak
  işaretlenmiş, olgu olarak iddia edilmemiştir.
- SKR'lerin **içerik kalitesi** denetlenmedi; yalnızca sayı, sahip, boyut ve statü ilişkisi.
- `2000-theory/ENS-2003`, `ENS-2004` ve `governance/SCAN-01`, `SCAN-02` **okundu, düzenlenmedi**
  (eşzamanlı çalışan ajanlar).
- Hiçbir dosya bu tarama sırasında değiştirilmemiştir; bu rapor **salt denetimdir**.
