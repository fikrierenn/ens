---
id:            GOV-SCAN-02
title:         Biçimsel Tutarlılık Taraması (SCAN-02)
type:          standard
canon:         false
constitutive:  false
origin:        ens-style-guardian tarama görevi
depends_on:    [ENS-0000]
principles:    []
status:        draft
owner:         ens-style-guardian
version:       0.2.0
last_reviewed: 2026-07-27
---

# SCAN-02 — Biçimsel Tutarlılık Taraması

Bu doküman ENS deposunun tamamında yapılan mekanik tutarlılık taramasının bulgularını
kaydeder. Kapsam: künye (metadata header) uyumu, REGISTRY senkronu, SKR numaralandırma
bütünlüğü, `status` dağılımı, cross-doc atıf bütünlüğü, dil politikası.

**Not:** `2000-theory/ENS-2003-company-memory.md`, `2000-theory/ENS-2004-learning-theory.md`
ve `governance/SCAN-01-authority-citations.md` başka ajanlar tarafından eş-zamanlı
düzenlendiği için yalnızca okunmuş, değiştirilmemiştir.

**Uygulama notu (2026-07-27, takip turu):** Aşağıdaki mekanik bulgular koordinatör onayıyla
doğrudan uygulandı: B-2 (REGISTRY ADR-0001 satırı), B-5 (REGISTRY çelişkili örnek-tahsis
satırları silindi), B-3 (REGISTRY aralık şeması + CEO-*/STYLE-SIGNOFF-* satırları eklendi),
A-6/C-1/E-2 (SKR-045 çakışması → `.claude/rules/SKR-046-tier3-discipline-rules.md` olarak
taşındı ve yeniden numaralandı — bu adım başka bir ajan tarafından yapıldı), E-1
(`referenced_by` — 12 dosyadan **10'u** düzeltildi; `ENS-2003`/`ENS-2004` yasaklı kaldığı için
**uygulanamadı**, aşağıda işaretli), A-2 (metadata-header.md `type` enum'u genişletildi),
A-3 (`status` enum'u genişletildi + `accepted` casing'i RFC-6001'de tekleştirildi), A-5
(GOV-ailesinin 4 dosyasına `failure_conditions: pending` eklendi). Aşağıdaki bulgu metinleri
**tarama anındaki orijinal hâliyle** korunmuştur (tarihsel kayıt); güncel durumu bu not ve
madde başlarındaki `[UYGULANDI]`/`[KISMEN UYGULANDI]` etiketleri yansıtır.

## Özet (sayılarla)

- Repo genelinde **129 `.md` dosyası** bulundu (2 tanesi `tools/.../bin/…` derleme çıktısı —
  gerçek kaynak değil, taramaya dahil edilmedi → efektif **127**).
- **91 dosya** künye (`id:` alanlı YAML front-matter) taşıyor; bunların hepsi bu taramada
  alan-alan (id/type/canon/constitutive/status/owner/version/maturity/evidence/
  failure_conditions/skeptic_review/depends_on/referenced_by/principles/origin) incelendi.
- **36 dosya** künyesiz (README'ler, `journal/`, `KULLIYAT.md`, `ROADMAP.md`, `AUDIT*.md`,
  `DEFECT-REGISTER*.md`, `.claude/agents/*.md` [ens-skeptic.md hariç — o örnek şablon
  içeriyor], `.claude/skills/*/SKILL.md`, `.claude/rules/*.md` [REVIEW-tier3-discipline.md
  hariç], `ROSTER.md`) — bunların Külliyat/GOV/STD kapsamına girip girmediği belirsiz, bkz.
  §A bulgu A-9.
- **Çekirdek Külliyat/GOV/ADR/RFC yapıtı (numaralı, karar-taşıyan) sayısı: 24** — bunların
  `status` dağılımı: **ratified 9**, **review 9**, **skeptic-cleared 2**, **accepted/Accepted
  (ADR/RFC terminal) 3**, **draft 1**.
- **SKR sayısı: 46 satır** (SKR-001..SKR-045), ancak **SKR-045 numarası İKİ FARKLI dosyada
  kullanılmış** → gerçek benzersiz SKR sayısı 45, bir numara çakışması var (bkz. B-1/C-1).
- **1 kesin `canon` kural ihlali** (ENS-3000, bkz. A-1), **1 zaten-bilinen/kayıtlı ihlal**
  (ENS-4000, RFC-6001 §8.3'te açık borç olarak izleniyor — yeni bulgu değil).
- **D ekseni (status/SKR-verdict tutarlılığı): sıfır ihlal bulundu** — taranan tüm
  `review`/`ratified`/`skeptic-cleared` yapıtlarında `status` alanı son SKR verdict'iyle
  tutarlı (bkz. §D). Bu, deponun bu eksende disiplinli olduğunu gösteriyor.
- **`referenced_by` (geri-bağlantı) alanında geniş çaplı drift**: GOV-ailesi (4 dosya)
  %100 doğru; ENS-1000/2001-2004/3000/4000/4001/4010/4025/4030/ADR-0001 ailesinde
  **9 dosyada eksik ve/veya yanlış geri-bağlantı** (bkz. E-1).

## A. Künye (metadata header) uyumu

**A-1 — `canon` kural ihlali (ratifikasyon şartı karşılanmadan `canon:true`).**
`3000-laws/ENS-3000-laws.md:5-16`: `canon: true`, `constitutive: true`, `status: draft`,
`skeptic_review: pending`. STD-METADATA-HEADER kuralı (§Değer kümeleri): `constitutive:true`
yapıt canon'unu **yalnızca ratifiye edilip skeptic incelemesinden sağ çıkınca** kazanır. Bu
yapıt ne ratifiye ne skeptic-incelenmiş; `canon:true` erken/geçersiz. → **`canon: false`
olmalı** (ENS-3000 ratifiye/skeptic-cleared olana kadar). Mekanik düzeltilebilir ama değeri
philosopher/architect'in onayına bağlı olduğundan işaretliyorum, dokunmuyorum.
(Karşılaştır: `ENS-4000` de aynı ihlali taşıyor [`canon:true`, `status:review`] ama bu **zaten
bilinen ve RFC-6001 §8.3'te açığa alınmış bir borç** — REGISTRY.md satır 42 bunu açıkça
belgeliyor. ENS-3000 için böyle bir kayıt YOK; bu yeni/kayıtsız bir bulgu.)

**A-2 — `type` değer kümesi eksik: inceleme/hiza yapıtları şemada tanımsız.**
`.claude/standards/metadata-header.md:55`: `type` enum'u yalnızca
`constitution | philosophy | theory | law | ontology | adr | rfc | module | book | standard | command | agent`
listeler. Ama disk üzerinde fiilen kullanılan ve REGISTRY'de kayıtlı 4 ayrı `type` değeri
şemada YOK: `skeptic-review` (45 SKR dosyası), `ceo-review` (CEO-0001/0002/0003), `style-signoff`
(STYLE-SIGNOFF-RFC-6001), `audit-report` (SCAN-03), `plan` (plans/*.md). Bu, dosyaların 45+'i
etkileyen bir şema-eksikliği — yeni bir ajan şemayı okuyup "SKR'ler standarda uymuyor" diye
yanlış alarm verebilir. **Mekanik düzeltme:** metadata-header.md §Değer kümeleri'ne bu 5 değeri
ekle. **Semantik değil** — yalnızca dokümantasyon eksikliği, style-guardian kapsamında.

**A-3 — `status` değer kümesi eksik/tutarsız.**
Şema enum'u: `draft | review | skeptic-challenged | ratified | superseded`. Fiili kullanım:
`skeptic-cleared` (ENS-4010, ENS-4031 — enum'da yok), `accepted` (ADR-0001, ADR-0002 — enum'da
yok, küçük harf), `Accepted` (RFC-6001 — enum'da yok, büyük harf; ADR'lerle **tutarsız
casing**), `final` (STYLE-SIGNOFF-RFC-6001 — enum'da yok). `skeptic-challenged` enum'da var ama
disk'te hiç kullanılmamış (ölü değer). **Mekanik düzeltme:** enum'u fiili kullanıma göre
genişlet (`skeptic-cleared`, `accepted`/`Accepted` casing'ini tekleştir — öneri: `Accepted`
büyük-İ, ADR-0001/0002'yi buna hizala —, `final`) veya ADR/RFC'nin kendi terminal-durum
alt-kümesini (`Proposed|Accepted|Deprecated|Superseded`) ayrı belgele.

**A-4 — `constitutive` alanı ADR'lerde tamamen eksik.**
`ADR-0001-agent-runtime.md` ve `ADR-0002-operations-capability.md` künyelerinde `constitutive:`
alanı YOK. Şema: "Külliyat yapıtları için zorunlu (kök varsayılan: false)". ADR'ler REGISTRY'nin
aralık şemasında ayrı bir satır olarak (Külliyat'ın parçası) listeleniyor; alan eksikliği
RFC-6001'in kurduğu ayrımın ADR katmanına hiç uygulanmadığını gösteriyor. Ayrıca ironik bir
nokta: **RFC-6001'in kendi künyesi de `constitutive:` taşımıyor** (`6000-rfc/RFC-6001-…md:2-16`)
— alanı icat eden belge kendi başlığında örneklemiyor (alan yalnızca gövde-içi şema örneğinde,
satır 333'te geçiyor).

**A-5 — `failure_conditions` alanı 7 yapıtta eksik.**
Kural 4/7 (Anayasa Madde X, her iki türde de zorunlu) karşılanmıyor:
`ADR-0001-agent-runtime.md`, `ADR-0002-operations-capability.md`, `4000-ontology/ENS-4000-glossary.md`,
`governance/000-governance-principles.md`, `governance/roles.md`, `governance/capability-matrix.md`,
`governance/canonical-process.md`. GOV-ailesinin 4'ü de bu alanı hiç taşımıyor — sistematik bir
boşluk (tek dosyaya özgü değil, GOV şablonunun kendisinde eksik).

**A-6 — SKR-045 numarası iki farklı dosyada, iki farklı yapıt için kullanılmış.**
`2000-theory/reviews/SKR-045-company-memory-v040-confidence-double-count.md:2` (`id: SKR-045`,
ENS-2003 v0.4.0 incelemesi) ve `.claude/rules/REVIEW-tier3-discipline.md:2` (`id: SKR-045`,
Tier-3 kural katmanı incelemesi) **aynı kimliği taşıyor**. Bu, görev talimatındaki emsalin
(SKR-037 çakışması → SKR-038'e taşındı) aynısı, tekrar oluşmuş. ENS-2003 künyesi zaten
`SKR-045`'i doğru şekilde kendi `skeptic_review:` listesine işlemiş (company-memory incelemesi
"gerçek" SKR-045 olarak kabul edilmiş görünüyor); `.claude/rules/REVIEW-tier3-discipline.md`
dosyasının kimliği **SKR-046 olarak yeniden numaralanmalı** (REGISTRY.md'ye satır eklenerek).
Detaylı analiz için bkz. §B/§C.

**A-7 — `type: standard` içinde `constitutive` alanı tutarsız uygulanıyor.**
`governance/*.md` (GOV-000/010/020/030, hepsi `type: standard`) `constitutive: true` taşıyor;
`.claude/standards/*.md` (STD-*, hepsi de `type: standard`) hiçbiri `constitutive` taşımıyor
(10 dosya: metadata-header, traceability, language-policy, maturity-model, evidence-standard,
validation-framework, context-management, architecture-principles, coding-standards,
documentation-style, ens-phase-model). Aynı `type` değeri iki farklı künye-profiline tabi —
şema bunu açıkça ayırt etmiyor. Muhtemel gerekçe: GOV-* "Külliyat" (REGISTRY'de numaralı,
`governance/` = yönetişim katmanı), STD-* Madde XII "Standards" katmanı (REGISTRY'nin aralık
şemasında "Külliyat'ın numaralı aralığı dışında" diye not düşülmüş) — ama bu ayrım
metadata-header.md'nin §Kurallar bölümünde **yazılı değil**. **Semantik/politika kararı** —
ens-philosopher veya ens-chief-architect'e devrediyorum: STD-* için `constitutive` alanının
neden muaf tutulduğu (ya da tutulmaması gerektiği) açıkça yazılmalı.

**A-8 — `tools/ens-ontology-linter/README.md` künyesiz.**
Bir "kaynak modül" README'si (kod + test barındırıyor) olduğu için Kurallar-6 gereği künye
taşımalı ("Kaynak modüllerde başlık, modül README.md'sinde durur"). Şu an hiç YAML
front-matter yok. Aynı kural altında `7000-reference-implementation/README.md` de künyesiz
(bu ikinci dosya için de aynı boşluk geçerli, ama 7000 kapsamı zaten AUDIT.md gibi başka
kayıt dosyalarıyla iz sürüyor — daha düşük öncelik).

**A-9 — Künye kapsamı belirsizliği (36 dosya).** `.claude/agents/*.md` (8 dosya, ens-skeptic.md
hariç), `.claude/skills/*/SKILL.md` (3), `.claude/rules/*.md` (4, REVIEW-tier3-discipline.md
hariç), `journal/*.md` (3), `KULLIYAT.md`, `ROADMAP.md`, `README.md` (kök), `AUDIT*.md`/
`DEFECT-REGISTER*.md` (6, `7000-reference-implementation/`), `ROSTER.md` künyesiz.
metadata-header.md kuralı harfiyen "depodaki **her** `.md`" diyor ama fiili pratik bunu
yalnızca Külliyat+GOV+STD+ADR/RFC+SKR/CEO/style-signoff katmanına uyguluyor. Bu **çelişkili
değil ama açık da değil** — standardın kapsam cümlesi daraltılmalı (örn. "çalışma dokümanları
[agent tanımı, skill, rule, journal, ROADMAP, KULLIYAT, AUDIT/DEFECT-REGISTER] kapsam dışıdır").
Mekanik/dokümantasyon önerisi, dokunmuyorum.

## B. REGISTRY ↔ dosya senkronu

**B-1 — SKR-045 çift-tahsis (bkz. A-6/C-1).** REGISTRY.md'nin "Ayrılmış numaralar" tablosunda
SKR-045 için satır yok (SKR-040..045 aralığı hiç tabloya girmemiş — bu ayrı bir bulgu, B-4'e
bkz.), dolayısıyla REGISTRY bu çakışmayı yakalayamazdı. Gerçek kaynak dosyalarda çakışma var.

**B-2 — `REGISTRY.md` ADR-0001 satırı bayat (stale).** `REGISTRY.md:52`:
`ADR-0001 | ... | draft/Proposed (... Accepted için ens-ceo hiza incelemesi bekliyor, Madde XIV)`.
Ama `5000-architecture/adr/ADR-0001-agent-runtime.md:11` zaten `status: accepted` ve
`5000-architecture/reviews/CEO-0001-adr-0001-alignment.md` (2026-07-23, `status: ratified`)
kararı **"ONAYLANDI — Proposed → Accepted"** çoktan vermiş. REGISTRY satırı bu kararı hiç
yansıtmıyor — dosya ile REGISTRY arasında gerçek bir senkron kopukluğu. **Mekanik düzeltme:**
REGISTRY.md:52 satırını "Accepted (CEO-0001 ratified, 2026-07-23 — …)" olacak şekilde güncelle.

**B-3 — CEO-0001/0002/0003 ve STYLE-SIGNOFF-RFC-6001 kimlikleri REGISTRY'de hiç yok.**
Bu 4 dosya diskte var, gerçek kararlar taşıyor (ADR-0001/RFC-6001/ADR-0002'nin kabul zincirinin
son halkası) ve REGISTRY.md'nin "Yeni numara tahsisi: yapıt oluşturulurken bu tabloya satır
eklenir" kuralına tabi olmalıydı — ama "Ayrılmış numaralar" tablosunda **hiçbirinin satırı
yok**, hatta "Aralık şeması" tablosunda `CEO-*` ve `STYLE-SIGNOFF-*` önek-aralığı **hiç
tanımlanmamış**. Aynı şekilde `PLAN-*` (plans/*.md) ve `SCAN-*` (governance/SCAN-*.md,
bu rapor dahil) önekleri de aralık şemasında yok. **Mekanik düzeltme:** aralık şemasına
`CEO-*`, `STYLE-SIGNOFF-*` satırları eklensin (SKR-* ile aynı model: "ilgili yapıt içinde");
`PLAN-*`/`SCAN-*` için de en azından bir dipnot ("çalışma dokümanı, Külliyat numaralandırmasına
tabi değil") eklensin.

**B-4 — REGISTRY'nin SKR satır-tutma pratiği tutarsız/seçmeli.** SKR-024..030 ve SKR-033..038
için REGISTRY'de ayrı satır var; ama SKR-001..023, **SKR-031, SKR-032**, SKR-039, SKR-040..045
için YOK. SKR-031/032'nin atlanması özellikle dikkat çekici çünkü aynı seri (ENS-4031 inceleme
zinciri) içindeki komşu numaralar (030, 033) satır almış. Aralık şeması "SKR-* → ilgili yapıt
içinde" dediği için bu teknik bir ihlal değil, ama **kuralsız/öngörülemez** bir uygulama —
ya her "sonuç belirleyici" (wounded/survives) SKR'ye tutarlı biçimde satır açılmalı ya da
pratik tamamen bırakılıp yalnızca "ilgili yapıt içinde" izlenmeli. Şu anki karışık durum,
bir sonraki ajanın "bu SKR'nin REGISTRY'de satırı yok, öksüz mü?" diye yanlış alarm vermesine
yol açabilir. **Semantik/politika kararı, philosopher/architect'e devrediyorum.**

**B-5 — `REGISTRY.md`'nin kendi içinde çelişki: aynı id iki farklı tabloda.**
`REGISTRY.md:80-87` ("Örnek hedef tahsisatlar (henüz yazılmadı)") tablosu `ENS-2001` (Decision
Theory) ve `ENS-3021` (Decision Entropy) kimliklerini **"henüz yazılmadı"** olarak listeliyor.
Ama aynı dosyanın 34 ve 39. satırlarında bu ikisi zaten **ratified, tam yazılı, skeptic-onaylı**
yapıtlar olarak kayıtlı. Bu, ids gerçekten tahsis edildikten sonra temizlenmemiş bir
şablon/örnek kalıntısı — REGISTRY'nin kendi iç tutarlılığını bozuyor ve yanlışlıkla "bu id
boş, kullanılabilir" izlenimi verebilir. **Mekanik düzeltme:** "Örnek hedef tahsisatlar"
tablosundan `ENS-2001` ve `ENS-3021` satırlarını sil (ya da tüm tabloyu kaldır — amacı
zaten örneklemekse gerçek tahsis edilmiş id'lerle çelişmeyen örnekler kullanılmalı, örn.
`ENS-2010`/`ENS-2021`/`RFC-6042` gibi gerçekten boş olanlar kalabilir).

**B-6 — Diskteki her numaralı yapıt REGISTRY'de kayıtlı mı? (çekirdek kontrol) — TEMİZ.**
ENS-0000/1000/2001-2004/3000/3021-3023/4000/4001/4010/4020/4025/4030/4031, ADR-0001/0002,
RFC-6001, GOV-000/010/020/030, tüm STD-* (11) — REGISTRY'de birebir karşılığı var, numara
tekrarı veya boşluk yok. (SKR/CEO/STYLE-SIGNOFF istisnaları B-3/B-4'te ayrı ele alındı.)

## C. SKR numaralandırma bütünlüğü

**C-1 — SKR-045 çakışması (bkz. A-6/B-1) — TEK gerçek numara hatası.**
SKR-001'den SKR-045'e kadar aralıkta **başka hiçbir eksik veya çift numara yok** — dizi
tam ve sıralı, tek istisna SKR-045'in iki kez tahsis edilmiş olması. Önerilen çözüm:
`.claude/rules/REVIEW-tier3-discipline.md` → `id: SKR-046` olarak yeniden numaralansın
(dosya adı `REVIEW-tier3-discipline.md` zaten `SKR-046-...` kalıbına uymuyor — bu ayrıca bir
adlandırma-şeması sapması, bkz. §E/adlandırma notu altında).

**C-2 — Her SKR'nin `origin` alanı gerçek bir yapıtı gösteriyor mu? — TEMİZ.**
45 SKR dosyasının tamamı tarandı; `origin:` alanları (ENS-1000/2001/2002/2003/2004/3021/
3022/3023/4001/4010/4020/4025/4030/4031, ADR-0001/ADR-0002, RFC-6001) hepsi diskte gerçekten
var olan yapıtlara çözülüyor. `.claude/rules/REVIEW-tier3-discipline.md` (SKR-045/gerçekte
SKR-046 olması gereken) için `origin: .claude/rules/ (Tier-3 discipline rule layer)` —
bu bir ENS-NNNN değil, bir dizin — ama SKR'nin kendi metni bunu açıkça "Kapsam sınırı:
`.claude/rules/` bir Külliyat katmanı DEĞİLDİR" diyerek gerekçelendiriyor; kasıtlı ve
belgelenmiş bir istisna, hata değil.

**C-3 — Yapıtların `skeptic_review:` listesi diskteki SKR'lerle tutuyor mu? — büyük ölçüde
TEMİZ, 1 boşluk.** Çapraz kontrol edilen tüm yapıtlar (ENS-1000/2001/2002/2003/2004/3021/
3022/3023/4010/4020/4025/4030/4031, ADR-0001/0002, RFC-6001) `skeptic_review:` alanlarında
disk-üzerindeki gerçek SKR'lere işaret ediyor, ters yönde de (SKR → hedef yapıt) tutuyor.
**Tek boşluk:** `4000-ontology/ENS-4000-glossary.md` hiç `skeptic_review:` alanı taşımıyor
(REGISTRY notu "skeptic bekliyor" diyor ama künyede bu açıkça `pending` olarak yazılı değil,
alan tamamen yok — bkz. A-5 ile birlikte değerlendirilebilir, aynı kök neden).

**C-4 — `status` dağılımı özet (SKR'ler).** 45 SKR'nin `status` alanı: incelenen örneklerin
çoğunda `draft` (görev-içi, henüz üst yapıta tam ratifiye yansımamış — bu SKR türü için
normal bir çalışma durumu) veya `ratified`/`review` (daha eski, 001-023 aralığı). Bu alan
SKR'ler için ayrı bir yaşam-döngüsü izliyor (`draft` = "verdict verildi ama üst yapıtın
kendi ratifikasyonu ayrı" gibi görünüyor) — anlamlı bir ihlal gözlenmedi, yalnızca not
düşülüyor.

## D. `status` dağılımı ve takılı kalmışlar

**Dağılım (24 çekirdek Külliyat/GOV/ADR/RFC yapıtı):**
- `ratified` (9): ENS-0000, ENS-1000, ENS-2001, ENS-2002, ENS-3021, ENS-3022, ENS-3023,
  ENS-4025, ENS-4030
- `review` (9): ENS-2003, ENS-2004, ENS-4000, ENS-4001, ENS-4020, GOV-000, GOV-010, GOV-020,
  GOV-030
- `skeptic-cleared` (2): ENS-4010, ENS-4031 (bilinçli ara-durum — "ratified/canon ayrı
  governance edimi" notu her ikisinde de var; skeptic sağ çıktı ama resmi ratifikasyon
  ayrı bir edim olarak bekletiliyor — **kasıtlı, sorun değil**)
- `accepted`/`Accepted` (3): ADR-0001, ADR-0002, RFC-6001
- `draft` (1): ENS-3000

**D-1 — "review'de kalıp SKR survives verdiği hâlde terfi etmemiş" taraması — SIFIR İHLAL.**
- ENS-2003: `review` + son SKR-045 = **wounded** (4 blocking bulgu) → `review`de kalması
  DOĞRU, terfi beklemiyor.
- ENS-2004: `review` + son SKR-044 = survives, ANCAK v0.3.3'te skeptic-görmemiş yeni bir
  değişiklik (§Implications hizalaması) eklenmiş ve künye kendi içinde bunu açıkça
  belgeliyor ("status: ratified → review" notu) → kasıtlı geri-alma, DOĞRU.
- ENS-4000, ENS-4001, GOV-ailesi: hiç skeptic incelemesi görmemiş (`skeptic_review` yok veya
  `pending`) → `review`de kalmaları tutarlı, "terfi hakkı doğdu ama verilmedi" durumu yok.
- ENS-4020: SKR-030 survives (M2) ama status hâlâ `review` — kasıtlı, "ratified/canon ayrı
  governance edimi" notuyla açıkça gerekçelendirilmiş (ENS-4010/4031 ile aynı desen).

**D-2 — "ratified olduğu hâlde son SKR'si wounded/refuted" taraması — SIFIR İHLAL.**
ENS-2001 (SKR-033 survives), ENS-2002 (SKR-006 survives), ENS-3021/3022/3023 (SKR-012/014/016
survives), ENS-4025 (SKR-022 survives + SKR-031 D-1 düzeltmesi), ENS-4030 (SKR-021 survives),
RFC-6001 (SKR-036 survives + CEO-0002 + STYLE-SIGNOFF), ADR-0001 (SKR-029 survives + CEO-0001),
ADR-0002 (SKR-037 survives + CEO-0003) — **hepsinde son verdict survives/ratified**, hiçbir
`ratified`/`accepted` yapıtın arkasında wounded/refuted bir SKR yok. Bu eksende depo
**disiplinli**: canon-erken-ilanı (A-1) dışında, `status` alanı hiçbir yerde skeptic
verdict'ini yanlış yansıtmıyor.

**D-3 — Tek gerçek `draft`: ENS-3000.** "Enterprise Laws (Kayıt)" — bir kayıt/dizin belgesi,
henüz hiç skeptic görmemiş (`skeptic_review: pending`), `referenced_by: []` iken fiilen
ENS-3021/3022/3023 tarafından `depends_on` ile referans alınıyor (bkz. E-1). `draft` durumu
kendi başına sorun değil ama `canon: true` ile birleşince A-1'deki ihlali oluşturuyor.

## E. Cross-doc atıf bütünlüğü

**E-0 — `depends_on` çözünürlüğü — TEMİZ.** Tüm çekirdek yapıtların (24 + 45 SKR + 3 CEO +
STYLE-SIGNOFF) `depends_on:` listeleri tek tek çözüldü; hepsi diskte gerçekten var olan
kimliklere işaret ediyor. **Öksüz düğüm veya kırık `depends_on` bulunamadı.**

**E-0b — Markdown link bütünlüğü — örneklem TEMİZ.** Göreli markdown linkleri (`](../…)`,
`](./…)`) içeren ~35 satır örneklendi (ENS-0000, RFC-6001, tüm `.claude/standards/*.md`,
README.md, ADR-0001/0002, ENS-4000, ENS-3000, KULLIYAT.md) — hedeflerin tümü diskte mevcut.
Kırık link bulunamadı.

**E-0c — Anayasa madde atıfları — TEMİZ.** ENS-0000 Madde I–XV hepsi mevcut; depoda geçen
`§III/§IV/§V/§VII/§VIII/§IX/§X/§XI/§XII/§XIII/§XIV/§XV` atıflarının tamamı gerçek bölümlere
karşılık geliyor. Olmayan bir maddeye atıf bulunamadı.

**E-1 — `referenced_by` (geri-bağlantı) alanında geniş çaplı drift — asıl bulgu.**
`depends_on` grafiğinden hesaplanan "doğru" `referenced_by` kümesiyle künyede yazan değer
karşılaştırıldı:

| Yapıt | Künyede yazan `referenced_by` | Olması gereken (depends_on grafiğinden) | Durum |
|---|---|---|---|
| ENS-1000 | `[ENS-4000]` | `[ENS-2001, ENS-2002, ENS-2003, ENS-2004]` | **Yanlış+eksik** — ENS-4000 aslında ENS-1000'e depends_on ile bağlı değil (ENS-4000 depends_on: `[ENS-0000]` sadece); gerçek 4 bağımlı hiç yok |
| ENS-2001 | `[ENS-2004, ENS-3021, ENS-3022, ENS-4010]` | + `ENS-2002, ENS-2003, ENS-3023, ADR-0001, ADR-0002` | Eksik 5 |
| ENS-2002 | `[ENS-4010]` | `[ENS-2003, ENS-2004, ENS-3021, ENS-3022, ADR-0001, ADR-0002, ENS-4010]` | Eksik 6 |
| ENS-2003 | `[ENS-2004, ENS-3023, ENS-4010]` | + `ENS-3021, ENS-3022, ADR-0001, ADR-0002` | Eksik 4 |
| ENS-2004 | `[ENS-2002, ENS-2003, ENS-4010]` | `[ENS-3021, ENS-3022, ENS-3023, ENS-4010, ADR-0001, ADR-0002]` | **Yanlış+eksik** — ENS-2002/ENS-2003 aslında ENS-2004'e depends_on ile bağlı değil (yön ters); gerçek 6 bağımlıdan yalnızca ENS-4010 doğru |
| ENS-3000 | `[]` | `[ENS-3021, ENS-3022, ENS-3023]` | Tamamen eksik |
| ENS-4000 | `[ENS-2001..2004, ENS-3021..3023, ENS-4001]` | + `RFC-6001` | Eksik 1 |
| ENS-4001 | `[]` | `[ENS-4010, ENS-4020, ENS-4030, ENS-4031, GOV-000, GOV-010, GOV-020, GOV-030]` | Tamamen eksik (8) |
| ENS-4010 | `[ENS-4020]` | + `ENS-4025, ENS-4030, ENS-4031, ADR-0001, ADR-0002` | Eksik 5 |
| ENS-4025 | `[]` | `[ENS-2004, ENS-4031, ADR-0001, ADR-0002]` | Tamamen eksik |
| ENS-4030 | `[]` | `[ENS-4025, ENS-4031]` | Tamamen eksik |
| ADR-0001 | `[]` | `[ADR-0002]` | Eksik |
| ENS-4020 | `[ADR-0002]` | `[ADR-0002]` | ✅ Doğru |
| GOV-000/010/020/030 | tam ve doğru | — | ✅ Doğru (dört dosya da birbirini doğru işaret ediyor) |

Toplam **9 dosyada** eksik/yanlış geri-bağlantı. `referenced_by` şemada "opsiyonel, validator
tutabilir" olarak işaretli (metadata-header.md satır 36) — yani teknik olarak "kırık atıf"
(rule 2, `depends_on` için) kadar ağır değil, ama izlenebilirlik değerini büyük ölçüde
kaybettiriyor: GOV-ailesi dışında geri-bağlantı fiilen güvenilmez. **Mekanik düzeltme**
(`depends_on` grafiğinden yeniden türetilebilir, anlam değişikliği yok): yukarıdaki tabloyu
uygulayarak 9 dosyanın `referenced_by` alanını güncelle.

**E-2 — Adlandırma/dosya-yapı sapması: `.claude/rules/REVIEW-tier3-discipline.md`.**
Bu dosya `id: SKR-045` (çakışan, olması gereken SKR-046) taşıyor ama `2000-theory/reviews/
SKR-NNN-*.md` adlandırma kalıbına uymuyor (`REVIEW-` öneki + `.claude/rules/` konumu — diğer
tüm SKR'ler `<domain>/reviews/SKR-NNN-<slug>.md` kalıbında). Kapsam notunda kasıtlı olduğu
belirtilmiş ("`.claude/rules/` bir Külliyat katmanı DEĞİLDİR") ama SKR numaralandırma
alanını (kıt, tekil kaynak) paylaşması adlandırma-şeması açısından karışıklığa yol açıyor.
Öneri: ya farklı bir önek (örn. `RULE-REVIEW-NNN`) kullansın ya da gerçek `SKR-*` şemasına
taşınsın (dosya adı `SKR-046-tier3-discipline.md` + uygun `reviews/` dizini).

## F. Dil politikası

**F-1 — `tools/ens-ontology-linter/README.md` tamamen İngilizce.**
`language-policy.md` kuralı istisnasız: "ENS deposundaki tüm dokümanlar Türkçe yazılır." Bu
dosyanın tamamı (başlıklar dahil: "Why this is a tool, not an LLM agent", "How to run",
"Failure conditions / honest limits") İngilizce düz yazı — teknik terim istisnası kapsamına
girmiyor, tam cümleler/paragraflar İngilizce. En büyük tek dil-politikası ihlali bu taramada
bulunan. **Semantik değil, çeviri gerektiren biçimsel bir düzeltme** — ama içerik hacmi
(106 satır) nedeniyle style-guardian'ın doğrudan üstlenebileceği bir iş; philosopher/architect
onayı gerektirmiyor, sahibi (`ens-architect`, README'deki "design decision" notuna göre)
bilgilendirilerek Türkçeye çevrilebilir. Mekanik listeye **eklemedim** (çeviri, "biçim"den
öte bir efor) — ayrı bir takip görevi olarak öneriyorum.

**F-2 — Kök `README.md` satır 3: tek İngilizce slogan cümlesi.**
`"Redefining companies in the age of artificial intelligence."` — başlık altında italik bir
slogan olarak duruyor, hemen ardından Türkçe açıklama geliyor. Bu bir teknik terim değil, tam
bir cümle. Muhtemelen kasıtlı bir "tagline" (pazarlama sloganı) ama dil politikası
istisnasız Türkçe istiyor. Düşük öncelik, ama not düşülüyor — semantik bir karar
(slogan kalsın mı, Türkçeleşsin mi) philosopher/style-guardian sahipliğinde.

**F-3 — Geri kalan taranan külliyat (91 künyeli dosya + örneklem) — TEMİZ.**
`.claude/standards/*`, `governance/*`, `0000-2000-3000-4000-5000-6000` katmanlarındaki
düz-yazı içerik örneklemesinde (başlıklar, gerekçe paragrafları) tutarlı biçimde Türkçe;
teknik terimler (Decision Capital, Context Score, Bounded Autonomy, vb.) doğru şekilde
orijinal bırakılmış, ilk-kullanım açıklamaları genel olarak mevcut. Yaygın bir "sessiz
İngilizceleşme" deseni bulunamadı — F-1/F-2 dışında geniş çaplı bir dil-politikası sorunu yok.

## Mekanik düzeltilebilir bulgular (toplu liste)

Anlam değiştirmeyen, doğrudan uygulanabilir düzeltmeler — tek commit'te kapatılabilir:

1. **[B-2] [UYGULANDI]** `REGISTRY.md` — ADR-0001 satırı "draft/Proposed … bekliyor" yerine
   "**Accepted** (CEO-0001 ratified, 2026-07-23)" olacak şekilde güncellendi.
2. **[B-5] [UYGULANDI]** `REGISTRY.md` — "Örnek hedef tahsisatlar" tablosundan `ENS-2001` ve
   `ENS-3021` satırları silindi (zaten tahsis edilmiş/yazılmış, tabloyla çelişiyordu).
3. **[B-3] [UYGULANDI]** `REGISTRY.md` "Aralık şeması" tablosuna `CEO-NNNN`, `STYLE-SIGNOFF-*`,
   `PLAN-*`, `SCAN-*` satırları eklendi. "Ayrılmış numaralar" tablosuna CEO-0001/0002/0003 ve
   STYLE-SIGNOFF-RFC-6001 satırları eklendi. (GOV-* için aralık şeması satırı bu turun
   kapsamında değildi, ayrı bırakıldı — bkz. B-3 orijinal metni.)
4. **[A-6/C-1/E-2] [UYGULANDI — başka ajan tarafından]** `.claude/rules/REVIEW-tier3-discipline.md`
   → `.claude/rules/SKR-046-tier3-discipline-rules.md` olarak taşındı, `id: SKR-046` yapıldı,
   atıfları güncellendi.
5. **[E-1] [KISMEN UYGULANDI — 10/12]** Aşağıdaki 10 dosyanın `referenced_by:` alanı E-1
   tablosundaki hesaplanmış değerlerle değiştirildi: ENS-1000, ENS-2001, ENS-2002, ENS-3000,
   ENS-4000, ENS-4001, ENS-4010, ENS-4025, ENS-4030, ADR-0001. **`ENS-2003` ve `ENS-2004`
   UYGULANMADI** — bu iki dosya başka bir ajanın çalışma alanında olduğu için dokunulmadı;
   bu iki dosyanın `referenced_by` düzeltmesi (E-1 tablosundaki değerler hâlâ geçerli) ileride
   ilgili ajan/philosopher tarafından ya da bu dosyalar serbest kaldığında style-guardian
   tarafından tamamlanmalı.
6. **[A-2] [UYGULANDI]** `.claude/standards/metadata-header.md` §Değer kümeleri `type` enum'una
   `skeptic-review | ceo-review | style-signoff | audit-report | plan | scan-report` eklendi.
7. **[A-3] [UYGULANDI]** `.claude/standards/metadata-header.md` §Değer kümeleri `status` enum'una
   `skeptic-cleared | accepted | final` eklendi, ADR/RFC/style-signoff'un tür-özel terminal-durum
   alt-kümesi olduğu açıkça not edildi (şema boşluğu, dosya hatası değil); `RFC-6001`'in
   `status: Accepted` değeri `accepted` olarak küçültülüp ADR-0001/ADR-0002 ile tekleştirildi.
8. **[A-5] [UYGULANDI]** Şu 4 dosyaya `failure_conditions: pending` eklendi (dosyalarda konuya
   ilişkin bir bölüm bulunmadığından `stated` değil, dürüst `pending` değeri kullanıldı):
   `governance/000-governance-principles.md`, `governance/roles.md`,
   `governance/capability-matrix.md`, `governance/canonical-process.md`. (ADR-0001/ADR-0002 ve
   `ENS-4000`'in `failure_conditions` eksikliği bu turun kapsamı dışında bırakıldı — talimat
   yalnızca `governance/` altındaki 4 dosyayı belirtti.)

## Semantik / anlam sorunları — devredilecek

Değeri veya doğruluğu benim yetkimde olmayan, ilgili sahibe devredilen bulgular:

- **[A-1] → ens-philosopher.** `ENS-3000` (`3000-laws/ENS-3000-laws.md`) `canon: true` taşıyor
  ama `status: draft` + `skeptic_review: pending` — RFC-6001 §7.2 kuralına göre bu geçersiz
  bir canon-ilanı (kazanılmamış canon). `canon: false`'a çekilmeli ya da yapıt hızla
  ratifiye/skeptic-incelemeye alınmalı. ENS-4000'deki benzer/zaten-kayıtlı duruma paralel
  ama bu **kayıtsız** bir tekrar — ayrı bir borç maddesi olarak açılmalı.
- **[A-7] → ens-philosopher / ens-chief-architect.** `type: standard` içinde `GOV-*` ailesi
  `constitutive` taşırken `STD-*` ailesi taşımıyor; metadata-header.md bu ayrımı açıkça
  gerekçelendirmiyor. Politika netleştirilmeli: STD-* muaf mı, değil mi, ve neden.
- **[B-4] → ens-philosopher / ens-skeptic.** REGISTRY'nin hangi SKR'lere ayrı satır açacağı
  konusunda tutarlı bir kural yok (SKR-031/032/039/040-045 atlanmış, komşuları almış).
  Ya sistematik hale getirilmeli ya da tamamen bırakılıp "ilgili yapıt içinde" izlenmeli.
- **[A-9] → ens-philosopher / ens-style-guardian (üst-politika).** metadata-header.md'nin
  "depodaki her `.md`" kapsam cümlesi fiili pratikle (36 künyesiz dosya) uyuşmuyor; kapsam
  netleştirilmeli.
- **[F-1] → ens-architect (sahip) / ens-style-guardian (uygulayıcı).** `tools/ens-ontology-linter/
  README.md` tamamen İngilizce; Türkçeye çevrilmeli. İçerik/teknik doğruluk sahibi ens-architect
  olduğundan çeviri onayı için işaretliyorum.
- **[A-8] → ens-architect.** `tools/ens-ontology-linter/README.md` ve
  `7000-reference-implementation/README.md` künyesiz; kaynak-modül README kuralı (Kurallar-6)
  gereği künye eklenmeli — hangi `id`/`type` altında ele alınacağı (STD mi, ayrı bir MOD-* mı)
  mimari sahibinin kararı.
