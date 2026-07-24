---
id: SKR-026
type: skeptic-review
validation_dimension: engineering
origin: ADR-0001
depends_on: [ADR-0001, SKR-024]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-026 — ADR-0001 (Agent Runtime & Bounded Autonomy) Bağımsız Validation

> **Bağımsızlık beyanı (G2/G3):** Bu inceleme, önceki inline **SKR-024**'ten *bağımsız*
> üretildi. SKR-024, ADR'yi yazan sürekli context içinde inline yazılmıştı — G2 ("no author
> canonizes own work") / G3 ("validation ≠ approval") ihlali riski taşıyordu. Bu kayıt,
> ADR-0001'i sıfırdan, kendi bulgularımla sınadı; SKR-024'ü *doğrulamak* değil, *yeniden
> yargılamak* için. SKR-024 ile örtüşme ve ayrışma açıkça işaretlenmiştir. Böylece
> ADR-0001 artık ≥2 bağımsız validation kaydına (SKR-024 + SKR-026) sahiptir (G4).

## Verdict
**wounded.** SKR-024 ile aynı sonuç etikette, ama kısmen *farklı gerekçelerle*: D2 ve D3
gerçekten kapanmış (teyit ediyorum, aşağıda); D1'in downstream'e ertelenmesi savunulabilir.
Ancak SKR-024'ün **kaçırdığı iki gerçek açık** var: (A) ADR, kendi itiraf ettiği **efferent
teori boşluğunun** (plan → act → actuate → operate) üstüne mimari kuruyor — bu Külliyat'ta
karşılığı olmayan runtime primitifleri (`Planner`, `Actuation Layer`) doğuruyor (Madde IX +
Madde VIII orphan); (B) commitment-vs-mechanical **sınıflandırma edimi** kendisi izsiz,
yüksek-frekanslı bir meta-karar — regres riski. Hiçbiri *refuted* seviyesinde değil; ADR'nin
çekirdek türetmesi (kernel = Attention/Decision/Capability bileşimi) sağlam. Ama Accepted
öncesi A ve B kapatılmalı.

## Yenilik / prior-art incelemesi (bağımsız)
ADR-0001 §3 prior-art'ı (AIOS, MemGPT/Letta, LangGraph, AutoGen, CrewOps) dürüst; her satırın
delta'sı somut. Bağımsız kontrolüm: **AIOS** (arXiv:2403.16971, Mei vd.) gerçek ve iddia
edildiği şeyi (LLM-as-kernel, syscall, scheduler/context/memory manager) taşıyor — atıf doğru,
uydurma değil. **MemGPT/Letta** (Packer vd. 2023) üç-katman bellek iddiası doğru. ENS'in
"delta = çekirdeğin üstüne bilişsel disiplin" konumu abartılı değil; icat iddiası yok. Bu
boyutta ADR temiz — Madde VI (pazarlama dili / yanlışlanamaz iddia) ihlali görmedim.

## SKR-024 bulgularının bağımsız yeniden yargısı

### D1 — kernel-vs-pipeline ölçütü (SKR-024 Bulgu 1): **kısmen geçerli, downstream'e ertelenmiş**
SKR-024 haklıydı: ADR-0001 §1.1'de "kernel > pipeline" kararı North Star *varsayımına*
dayanıyor (döngüsel). ADR bunu ADR-0002 §7.1'e (D1 ölçütü) erteliyor. İki gözlem:
- **Yetki ters akışı (yeni):** ADR-0001'in merkezî mimari gerekçesi (neden kernel), *kendisine
  bağımlı* bir alt-akış yapıtında (ADR-0002) üretiliyor. Madde XII "yetki tek yönde akar, üstteki
  tüketir" ilkesiyle hafif gerilimde: karar, onu haklı çıkaran ölçütten *önce* veriliyor.
  Savunulabilir ("desen burada tanımlanır, orada sınanır") ama künyeye açıkça yazılmalı.
- ADR-0002'de K3 ("geç genişletilebilirlik") *yine* North Star'ı ("başka Pack'ler sonradan
  takılır") varsayıyor — yani SKR-024'ün döngüsellik itirazı ADR-0002'de tam kapanmıyor, 4
  ölçütten 1'ine sızıyor (ayrıntı SKR-027 Bulgu 1). Yine de operax K1+K2+K4 = 3/4 ile eşiği
  K3'süz de geçtiğinden sonuç dayanıklı. **Net:** SKR-024'ün D1 talebi meşruydu; ADR-0002 onu
  büyük ölçüde ödüyor ama tam değil.

### D2 — "her action = Decision atomu" fazla-iddiası (SKR-024 Bulgu 2): **GERÇEKTEN kapandı**
ADR-0001 v0.2 §5.4 iddiayı **commitment-sealed action'lara** daralttı ve granülerlik ölçütü
verdi (geri-dönülemez/eşik-aşan taahhüt **veya** alternatif-arası seçim ⇒ atom; aksi ⇒ mekanik
sub-step). ENS-2001 §Individuation (4 koşul) ve §4 recursion ile birebir tutarlı — bağımsız
kontrolde teoriden zorlama değil. SKR-024'e katılıyorum: **çözüldü.**

### D3 — `Policy` Madde IX tanımsız-primitive (SKR-024 Bulgu 3, blocking): **kapandı, küçük çekince**
ADR-0001 v0.2 §5.6 `Policy = ens-core:Constraint bundle` eşlemesi yaptı. ENS-4010'a karşı
doğruladım: `Constraint` node'u mevcut (profil "Rule", `constrains → Decision/Capability`).
Bir Constraint *çokluğu* yeni tip değil — Madde IX uyumu sağlanıyor. **Küçük çekince:** "bundle"
(adlandırılmış Constraint demeti) için ENS-4010'da bir *aggregation* relation'ı yok; bu bir
çokluk mu yoksa örtük yeni bir yapı mı, ontoloji düzeyinde belirsiz. Refuted etmez; ENS-4020
Ontology Validation'ında netleşmeli. SKR-024'ün "çözüldü" yargısına katılıyorum, çekince ekli.

## Kaçırılmış yeni bulgular (SKR-024'te yok)

### Bulgu A — Efferent teori boşluğu üstüne mimari (Madde IX + Madde VIII) — **en güçlü açık**
ADR §1 açıkça itiraf ediyor: afferent yarı (sense→think→remember→learn) Külliyat'ta var, ama
**efferent yarı — `plan → act → actuate → operate` — YOK** ("BU YARI YOK", satır 59). Sorun:
ADR bu boşluğun *üstüne* kernel bileşenleri kuruyor — §5.2 diyagramında `Planner` ve
`Action / Actuation Layer` birinci-sınıf runtime primitifleri. Oysa:
- **§4 traceability tablosunda `Planner` ve `Actuation Layer` için `realizes` satırı YOK.**
  Tablo yalnızca Scheduler→ENS-3022, Action(atom)→ENS-2001, Proof-Trace→ENS-4025, Memory→ENS-2003,
  Learning→ENS-2004, Gate→ENS-2001§Indiv, Capability→ENS-4010 eşliyor. `plan/actuate/operate`
  fiillerinin Külliyat karşılığı gösterilmiyor — çünkü ADR'nin kendi dediği gibi *yok.*
- Madde IX: "hiçbir ADR Külliyat'ta olmayan kavram tanıtamaz." Madde VII: "erken bir fazda
  eksik, bağımlı sonraki işi durdurur." Doğru sıra: önce efferent teori (`2000-theory`'de bir
  kavram — ör. "Enactment/Actuation Theory"), `ens-philosopher` önerir, `ens-skeptic` saldırır,
  **sonra** ADR ona `realizes` eder. ADR bunun yerine mimariyi teori-boşluğunun üstüne koyuyor.
- ADR kısmen savunuyor: her şey mevcut kavramların "bileşimi" (§4 son paragraf) ve Action=Decision
  Enactment'ı olarak ENS-2001 lifecycle'ının Commitment-sonrası kısmına indirgeniyor. Bu, `Action`
  için geçerli — ama `Planner` ve `operate/actuate` için lifecycle karşılığı gösterilmiyor.
- **Sonuç:** wounded-düzey Madde IX açığı. Talep: (a) efferent primitifleri (`Planner`,
  `Actuation`) ya mevcut ENS-2001 Enactment aşamasına açıkça indirge (traceability satırı ekle),
  ya da (b) eksik efferent kavramı önce Külliyat'a al (Madde IX/VII). Şu hâliyle bu iki bileşen
  **orphan** (Madde VIII: yukarı-akış kenarı olmayan düğüm = kusur).

### Bulgu B — Commitment sınıflandırması izsiz meta-karar (regres riski)
§5.4 granülerlik ölçütü, çalışma-zamanında her action için "bu commitment-sealed mı, mekanik
sub-step mi?" sorusunu yanıtlamayı gerektirir. Bu **sınıflandırma ediminin kendisi** bir karardır
(alternatifler arası seçim: atom-say vs sub-step-say). Eğer kernel bunu otomatik yapıyorsa:
- Bu meta-karar yüksek-frekanslı ve **izsiz** (§5.5 proof-trace yalnızca atom düzeyinde zorunlu).
  Ama yanlış sınıflandırma, gerçek bir commitment'ı proof-trace'siz "mekanik" sayarak Madde VI
  (black-box) ihlaline yol açabilir — invariant'ın delik noktası.
- Kim/ne sınıflandırıyor, hangi confidence ile, geri-dönülebilir mi? ADR sessiz. Potansiyel
  regres: sınıflandırma-kararını doğrulamak için başka bir sınıflandırma gerekir.
- **Talep:** sınıflandırma edimini ya deterministik/denetlenebilir bir kural yap (izlenebilir),
  ya da onu da bir (düşük-maliyetli) proof-trace'e bağla. En azından bir OQ olarak işaretle.
  Bu, F2 (proof-trace maliyeti) ile B (bu bulgu) arasındaki gerçek gerilimi açığa çıkarır:
  maliyeti düşürmek için sınıflandırma yapıyorsun, ama sınıflandırma da izlenmezse invariant sızar.

### Bulgu C — "Capability Pack" paketleme kavramları — sınır notu (bloke etmez)
`Pack` (versiyonlu, sandbox'lı, registry'ye kayıtlı, tenant-aktive, 3-tier progressive-context,
allowedTools/deniedTools) CrewOps'tan toptan alınmış **paketleme/deployment** kavramlarıdır.
ENS-4010 yalnızca `Capability` (yeti) node'unu tanımlar; "Pack/sandbox/registry" orada yok.
Değerlendirme: bunlar *teori kavramı* değil *mühendislik mekanizması* — Madde IX'un hedefi
semantik-yük taşıyan kavramlardır, mekanizma değil. Dolayısıyla ihlal *değil*, ama sınırda
yürüyor. Not olarak bırakıyorum; Faz 3-4'te bu ayrım (kavram mı, mekanizma mı) netleşmeli.

## İç tutarlılık
- ENS-2001, ENS-3022, ENS-4010, ENS-4025 ile terminoloji tutarlı; sürüklenme (drift) görmedim.
- Confidence propagation `min` kullanımı (§5.5) ENS-4025 L7 ile birebir. Doğru.
- Tek gerçek iç gerilim: gövde "kernel" kesinliğiyle konuşurken F4 "kernel gereksiz olabilir"
  diyor — ama bu dürüst bir failure condition, çelişki değil (SKR-024 ile aynı görüş).

## Kalan (obligation, bloke etmez)
- OQ1 (proof-trace makine-şeması → RFC-6xxx) — ENS-4025 §Failure "formal dil Faz 4" ile zincirli.
- OQ3 (scheduler gerçek-zamanlı vs periyodik) — ENS-3022 per-decision maliyet gerilimi.
- F3 (gate kalibrasyonu Confidence'a bağımlı) — ENS-2004 kalibrasyon borcu.
- F5 (model-agnostisizm sızıntısı) — ADR-0002 §10 deterministik-first capability ile kısmen
  hafifliyor; doğru.

## Sahibine talepler (Accepted için)
1. **Bulgu A (blocking, Madde IX):** `Planner` ve `Actuation Layer`'ı ya ENS-2001 Enactment'a
   açık `realizes` satırıyla indirge, ya da eksik efferent kavramı önce Külliyat'a al. Orphan
   bileşen kalmasın.
2. **Bulgu B:** commitment-vs-mechanical sınıflandırmasının kendisini izlenebilir/denetlenebilir
   kıl ya da OQ olarak işaretle (proof-trace invariant'ının delik noktası).
3. **D1 künye notu:** ADR-0001'in kernel gerekçesinin ADR-0002'de tamamlandığını ve K3'ün North
   Star varsayımını taşıdığını açıkça belirt (SKR-024'ün döngüsellik itirazının kalıntısı).
4. **D3 çekince:** "Constraint bundle" aggregation'ının ontolojik statüsü ENS-4020'de netleşsin.

## Sonuç ve SKR-024 ile örtüşme
ADR-0001 `Proposed` kalır. **SKR-024 ile verdict örtüşür (wounded/wounded)** ve D2/D3'ün
kapandığını bağımsız teyit ederim — yani inline SKR-024'ün *o bulguları doğruydu*. Ancak SKR-024
iki gerçek açığı (efferent teori boşluğu / sınıflandırma regresi) **kaçırdı**; bunlar bu bağımsız
turun asıl katkısıdır. Bu, inline-review şüphesini kısmen doğrular: SKR-024 fena değildi ama
eksikti — bağımsız göz Madde IX'un en derin noktasını (kendi itiraf edilen teori boşluğu) yakaladı.
