---
id: SKR-029
type: skeptic-review
validation_dimension: engineering
origin: ADR-0001
depends_on: [ADR-0001, SKR-024, SKR-026]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-029 — ADR-0001 (Agent Runtime & Bounded Autonomy) 3. Bağımsız Teyit Turu

> **Bağımsızlık beyanı (G2/G3/G4):** Bu, ADR-0001'in **üçüncü** ve bu turdaki **bağımsız**
> validation'ıdır. SKR-024 (inline, G2/G3 riski) ve SKR-026 (ilk bağımsız tur, wounded) sonrası
> `ens-architect` v0.3'te Bulgu A ve Bulgu B'yi *inline* kapattığını iddia etti. Kendi düzeltmesini
> kendisi canonize edemez (G2 — "no author canonizes own work"). Bu kayıt, v0.3'ün Bulgu A/B'ye
> verdiği yanıtı — düzeltmeleri yapan context'ten bağımsız — sıfırdan yeniden yargılar. Boyut:
> **engineering** (traceability, Madde IX/VIII orphan, invariant tutarlılığı). Odak dar: Bulgu A,
> Bulgu B ve D2/D3'ün bozulmadığının hızlı teyidi; SKR-026'da `survives` çıkan D2/D3'e derinleşmem.

## Verdict
**survives.** SKR-026'nın iki blocking açığı (A: `Planner`/`Actuation Layer` orphan + efferent
teori boşluğu; B: sınıflandırma ediminin izsiz meta-karar olması) v0.3'te **gerçekten** kapanmış —
mekanik/zorlama değil, ENS-2001 lifecycle ve §4 recursion'a dayalı meşru indirgemelerle. Bulgu B
yanıtı SKR-026'nın istediği asgari düzeyi (bir OQ ekle) *aşıyor*: sınıflandırmayı Framing/Reasoning'e
gömüp atomun proof-trace'ine bağlıyor + gate'i ikinci savunma hattı yapıyor. Geriye yalnızca
**non-blocking, failure-condition düzeyinde artıklar** kalıyor (Enactment'ın teorik inceliği; §1↔§4
retorik gerilimi; §4'te künyede olmayan P2). Bunlar `refuted`/`wounded` değil. ADR `Proposed`
(status: draft) kalır; Accepted → `ens-ceo` hiza incelemesi (K4, Madde XIV), skeptic işi değil.

## SKR-026 ile örtüşme / ayrışma
- **Örtüşme:** SKR-026'nın Bulgu A ve Bulgu B'yi *gerçek açıklar* olarak teşhis etmesi doğruydu —
  teyit ediyorum. D2 (commitment-sealed daraltma) ve D3 (Policy = Constraint bundle) SKR-024/026'da
  `survives`ti; v0.3'te dokunulmadı, hâlâ sağlam (aşağıda hızlı teyit).
- **Ayrışma:** SKR-026 `wounded` dedi çünkü A/B o an açıktı. Bu tur `survives` — çünkü A/B artık
  kapalı. Yani verdict değişimi bir *çelişki değil, ilerlemenin ölçümüdür*: aynı ölçüt, farklı
  yapıt-durumu (v0.2→v0.3).

## Bulgu A teyidi — orphan gerçekten kapandı mı, yoksa zorlama eşleme mi?

**SKR-026 Bulgu A neydi:** ADR §1 kendi ağzıyla itiraf ediyordu — efferent yarı
(`plan → act → actuate → operate`) "BU YARI YOK". Ama §5.2 diyagramı `Planner` ve `Actuation Layer`'ı
birinci-sınıf runtime primitifi yapıyordu; §4 traceability tablosunda bunların `realizes` satırı
yoktu → **orphan** (Madde VIII: yukarı-akış kenarı olmayan düğüm) + teori-boşluğu üstüne mimari
(Madde IX/VII).

**v0.3 ne yaptı:** §4 tablosuna iki satır ekledi —
- `Planner → ENS-2001 lifecycle (Framing→Contextualization→Reasoning, commitment-öncesi)` [P1,P2]
- `Actuation Layer → ENS-2001 lifecycle (Enactment, commitment-sonrası)` [P1,P7]

**Bağımsız yargı — zorlama değil, ama artık bir yük noktası var:**
1. **Orphan graf-teorik olarak kapandı (Madde VIII sağlandı).** Her iki bileşen artık açık bir
   `realizes` kenarı taşıyor; yukarı-akış kenarsız düğüm kalmadı. Bu somut ve doğru.
2. **Eşleme mekanik/zorlama DEĞİL.** ENS-2001 §3 lifecycle'ında Framing/Contextualization/Reasoning
   ve Enactment aşamaları gerçekten var; `Planner` bunların çalışma-zamanı yürütücüsü olarak meşru
   okunuyor. Kritik nokta — "operate ayrı primitif değil" iddiası **ENS-2001 §4 recursion'a
   dayanıyor**, uydurma değil: mekanik alt-adımlar §5.4 granülerlik ölçütünü aşmıyorsa tek bir
   Enactment atomunun *içinde* kalır, ayrı düğüm doğurmaz. Bu türetme sağlam.
3. **Madde IX sağlandı.** v0.3 yeni bir Külliyat kavramı *icat etmiyor*; mevcut ENS-2001
   aşamalarına indirgiyor. "Efferent kavramı önce Külliyat'a al" (SKR-026 seçenek b) yerine
   "mevcut Enactment'a indirge" (seçenek a) seçildi — ADR'nin bir *türetme* olması gereğiyle tutarlı.

**Kalan artık (non-blocking, en güçlü hâliyle):** İndirgeme, teori-boşluğunu *doldurmaz*, onu
"mevcut bir aşamanın içi" olarak *yeniden sınıflandırır*. Oysa ENS-2001'de `Enactment` neredeyse
içeriksiz bir geçiştir ("karar dünyaya çıkar", §3) — ADR §1 bunu bizzat "teoride bir sınır nesnesi
olarak durur, çalışma-zamanı karşılığı yoktur" diye teşhis ediyordu. Yani v0.3, "orphan düğüm"ü
(yukarı-akış kenarı yok) "içi zayıf-teorize düğüme kenar" (kenar var ama hedef aşama az-tanımlı)
ile değiştirdi. Guarded action / sandbox / actuate / operate / observe gibi *zengin* runtime
davranışının tümü ENS-2001'in tek satırlık Enactment aşamasına yükleniyor. Bu **wounded düzeyinde
değil** — çünkü (a) Madde IX'un lafzı (yeni kavram yok) sağlanıyor, (b) recursion operate-as-substep'i
gerçekten temellendiriyor. Ama dürüst kayıt: **ENS-2001 `Enactment` aşaması, ADR-0001'in ona
yüklediği ağırlığa göre az-teorize.** Bu, Bulgu A'nın en derin biçiminin *çözülmediği*, yalnızca
*yer değiştirdiği* anlamına gelir. Talep (bloke etmez): ADR bir failure-condition/OQ olarak
"Enactment'ın runtime-içeriği ENS-2001'de elaborate değil; F1'e komşu bir borç" notunu açıkça
taşımalı — ya da ileride `ens-philosopher` Enactment'ı elaborate eden bir teori üretmeli.

## Bulgu B teyidi — sınıflandırma edimi izlenebilir kılındı mı, yeni delik açıldı mı?

**SKR-026 Bulgu B neydi:** "bu action commitment-sealed mi, mekanik sub-step mi?" sorusu yüksek-
frekanslı bir meta-karardır; §5.5 proof-trace yalnızca atom düzeyinde zorunlu olduğundan bu edim
**izsiz** kalabilir. Yanlış sınıflandırma (gerçek commitment'ı "mekanik" sayma) → proof-trace'siz
action → Madde VI (black-box) ihlali. Ayrıca regres: sınıflandırmayı doğrulamak için başka
sınıflandırma gerekir.

**v0.3 §5.4 yeni paragrafı ne yaptı (iki hamle):**
1. **Yeniden çerçeveleme:** Sınıflandırma ayrı/izsiz bir meta-karar *değil*, ENS-2001 lifecycle'ının
   **Framing/Reasoning** aşamasının kendisi. Alternatives kümesi framing sırasında belirlenir;
   tekil+deterministikse (a)/(b) sağlanmaz → sub-step. Bu değerlendirme Decision Object'in
   `Alternatives` alanına yazılır → **atomun proof-trace'i tarafından zaten kapsanır.**
2. **İkinci savunma hattı:** Eşik-aşan bir action alt-sınıflandırılsa bile §5.6 Bounded-Autonomy
   Gate'in stake-eşiği kontrolünde yakalanır.

**Bağımsız yargı — regres terminate ediyor, yeni delik açmıyor:**
- **Regres gerçekten sonlanıyor.** SKR-026'nın "sınıflandırmayı doğrulamak için sınıflandırma
  gerekir" endişesi v0.3'te kapanıyor: her sınıflandırma ya (i) *bir atomun izlenen Framing
  aşamasının parçası* (gerçek choice → yeni atom, kendi proof-trace'i), ya da (ii) *deterministik
  genişleme* (parent atomun proof-trace'inde kapsanır). Zincir, izlenecek bir *seçim taşımayan*
  deterministik/mekanik adımda durur. Sonsuz regres yok — çünkü sub-step tanımı gereği trace
  edilecek bir alternatif taşımaz. Bu tutarlı.
- **Mekanizma somut, hand-wave değil.** "Alternatives alanına yazılır, atomun proof-trace'i kapsar"
  → ENS-2001 §Decision Object (Alternatives alanı mevcut) ve §5.5 (atom-düzey trace) ile birebir.
- **SKR-026'nın asgari talebi aşıldı.** SKR-026 "en azından bir OQ olarak işaretle" demişti; v0.3
  daha güçlüsünü yaptı — sınıflandırmayı izlenen Framing'e *bağladı*. OQ minimumunun üstünde.

**Kalan artık (non-blocking, en güçlü karşı-argüman):** İki savunma hattının *kesişiminde* dar bir
sızıntı bandı kalıyor — **hem eşik-altı hem gerçek-commitment (b: birden çok makul yürütme yolu)**
olan bir action. Gate (a: stake) bunu yakalamaz (eşik-altı); yeniden-çerçeveleme (b) ise ancak
framing gerçekten alternatifleri *tanırsa* yakalar. Enactment sırasında öngörülmemiş bir çatal-nokta
düşük-stake ile belirirse, §5.4 (b) onu yeni atom yapmalı — ama "birden çok makul yol var mı?"
yargısı tam da bulanık çizgidir (ENS-2001 §Failure "kademeli commitment" ile komşu). Bu artık
SKR-026'nın orijinal endişesinden *çok daha dar*: yüksek-stake sızıntılar gate'te, açık-alternatifli
sızıntılar framing'de kapanıyor; geriye yalnızca "eşik-altı + framing'de fark edilmeyen gerçek
alternatif" kesişimi kalıyor. Failure-condition düzeyinde, bloke etmez.

## D2 / D3 hızlı teyit (SKR-024/026'da survives — bozulmadı mı?)
- **D2 (§5.4, commitment-sealed daraltma + granülerlik ölçütü):** Bulgu B paragrafı §5.4'e *eklendi*,
  D2 içeriğini değiştirmedi. Granülerlik ölçütü (geri-dönülemez/eşik-aşan **veya** alternatif-seçim
  ⇒ atom) ENS-2001 §Individuation ile hâlâ birebir. **Bozulmadı — survives.**
- **D3 (§5.6, Policy = ens-core:Constraint bundle):** Değişmedi. Küçük çekince (SKR-026): "bundle"
  aggregation'ının ontolojik statüsü ENS-4020'de netleşmeli. ENS-4020 v0.3'ü inceledim (SKR-030):
  `ApprovalRule` "policy bundle üyesi" diye anılıyor ama **bir aggregation relation'ı hâlâ yok** —
  çekince *açık kalıyor* (non-blocking, ENS-4010/ENS-4020 borcu; ADR'yi bloke etmez).

## İç tutarlılık (v0.3'ün açtığı yeni gerilimler)
1. **§1 ↔ §4 retorik gerilimi (yeni, non-blocking).** §1 dramatik biçimde "efferent yarı YOK"
   diyor; §4 v0.3 ise "hepsi mevcut ENS-2001 aşamaları (Framing/Reasoning/Enactment/Measurement)"
   diyor. İkisi birden doğru olamaz: eğer her efferent fiil mevcut ENS-2001 aşamasına iniyorsa,
   efferent yarı "yok" değil, ENS-2001'de *latent ama elaborate edilmemiş*ti. v0.3 fix'i §1'in
   çerçevesini kısmen çürütüyor. Talep: §1'i "efferent yarının *çalışma-zamanı elaborasyonu* yok"
   biçiminde yumuşat (kavramsal olarak yok değil).
2. **Künye ↔ §4 principle driftı (yeni, küçük).** Header `principles: [P1, P5, P6, P7]`; ama §4
   v0.3 `Planner` satırı **P2** atıfı taşıyor (Contextualization = Context = P2). P2 künyede yok.
   Bulgu A'yı kapatan düzeltme, künyede deklare edilmemiş bir principle (P2) soktu. Talep: header
   `principles`'a P2 ekle ya da Planner satırından P2'yi düş.
3. Terminoloji sürüklenmesi (drift) görmedim; ENS-2001/3022/4010/4025 ile tutarlı. Confidence
   `min` (§5.5) ENS-4025 L7 ile birebir. F4 ("kernel gereksiz olabilir") dürüst failure-condition,
   çelişki değil (SKR-024/026 ile aynı görüş).

## Sahibine talepler (bloke etmez — Accepted-önü rafinman, ens-ceo turuna girdi)
1. **§1 çerçevesini düzelt:** "efferent yarı YOK" → "efferent yarının çalışma-zamanı elaborasyonu
   yok"; §4 indirgemesiyle tutarlı kıl (İç tutarlılık #1).
2. **Enactment inceliği borcunu deklare et:** ENS-2001 `Enactment` aşamasının runtime-içeriğinin
   az-teorize olduğunu bir failure-condition/OQ olarak yaz (Bulgu A artığı); F1'e zincirle.
3. **Künye P2:** header `principles`'a P2 ekle (§4 Planner satırıyla hizala).
4. **Bulgu B dar-sızıntı bandını** (eşik-altı + framing'de görünmeyen gerçek-alternatif) bir OQ
   olarak işaretle — invariant'ın kalan tek delik noktası.

## Sonuç
ADR-0001 **survives** (engineering, 3. bağımsız tur). SKR-026'nın iki blocking açığı gerçekten,
zorlama olmadan kapandı; D2/D3 bozulmadı. Kalan her şey non-blocking rafinman/failure-condition
düzeyinde. `status: draft` (Proposed) kalır — Accepted, `ens-ceo` hiza incelemesinin (K4, Madde XIV)
işidir, skeptic bunu vermez. `skeptic_review` artık [SKR-024, SKR-026, SKR-029] — ≥2 bağımsız
validator (G4) fazlasıyla sağlandı.

---

*İyi bir düzeltme orphan'ı kapatır ama izini bırakır: v0.3 efferent boşluğu doldurmadı, onu mevcut
bir aşamanın içine taşıdı — ve o aşamanın (Enactment) ne kadar zayıf-teorize olduğunu görünür kıldı.
Kapıyı geçmeye yeter; ama gelecek fazın Enactment'ı elaborate etmesi gereğini de miras bırakır.*
