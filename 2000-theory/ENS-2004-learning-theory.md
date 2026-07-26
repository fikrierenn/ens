---
id: ENS-2004
title: Learning Theory (ENS)
type: theory
canon: false
constitutive: false
maturity: M3
origin: ENS-0000 §III (P4), ENS-2001, ENS-2003
depends_on: [ENS-0000, ENS-1000, ENS-2001, ENS-2002, ENS-2003, ENS-4000, ENS-4025]
referenced_by: [ENS-2002, ENS-2003, ENS-4010]
principles: [P4, P3, P5, P6, P7]
status: review
owner: ens-philosopher
version: 0.3.3
last_reviewed: 2026-07-26
failure_conditions: stated
skeptic_review: [SKR-042, SKR-043, SKR-044]   # v0.3.3 (§Implications hizalaması, AUDIT-WAVE2/D-5) HENÜZ SKEPTIC GÖRMEDİ — status: ratified → review. | v0.3.0: SKR-042 → wounded (3 talep). v0.3.1: T1/T2 TAM kapandı; T3 KISMEN — bağımsız 2. tur SKR-043 → wounded: §Theoretical model §1 (satır 128) hâlâ otomatik-yazma dili taşıyordu. v0.3.2: T3-artığı kapatıldı — §1 "kaydeder (otomatik) + öneri (P7-gate'li)" ikili dile çevrildi, §Implications'taki "L1 ile beslenir" ifadesi de netleştirildi. Bağımsız 3. tur SKR-044 → survives: T3-artığı TAM kapandı + dosya-geneli otomatik-yazma taraması temiz (desen 3. kez tekrar etmedi). status: review → ratified. canon: false KALIR — Külliyat-girişi ayrı governance edimi.
---

# ENS Learning Theory

> P4'ün teorisi ve ENS'in **attribution kilit taşı**: Company Memory (ENS-2003), Context
> relevance (ENS-2002) ve Learning, üçü de "sonucu karara nasıl atfederiz" sorusuna bağlı.
> `canon: false` — skeptic'ten sağ çıkınca Külliyat'a girer.
>
> **v0.2 notu:** [SKR-009](reviews/SKR-009-learning-theory.md)'a yanıt. (1) Karar kalitesi
> kalibrasyon **+ seçim rasyonalitesi** olarak ikiye ayrıldı, (2) hindsight'a karşı **donmuş
> commitment snapshot** koruması eklendi, (3) merdiven nedensel-kanıt hiyerarşisine, outcome
> bias Baron & Hershey (1988)'e kredilendi. §Yanıt tablosu sonda.
>
> **v0.3 notu (additive, skeptic bekliyor):** §4'ün taahhüt ettiği ama operasyonelleştirmediği
> soruya — double-loop'u **kim/nasıl** tetikler ve yürütür — eksik mekanik katman eklendi:
> §4a **Reflective double-loop**. Geçmiş commit-edilmiş kararların proof-trace'i (ENS-4025 L8)
> okunur → sistematik öngörü hatası "**neden**" (yalnızca "başarısız oldu" değil) analiz edilir →
> Assumptions / relevance-model / attribution-seviyesi için **hedefli bir iyileştirme *önerisi***
> üretilir → öneri **hiçbir zaman otomatik uygulanmaz; insan onayı (P7) gerekir**. Bu
> **yeni teorik yasa değildir** — var olan §4 double-loop'un + P7'nin (Bounded Autonomy) +
> §3 attribution-merdiveninin bir sentezini somutlaştırır; deseni ENS icat etmez (GEPA/DSPy/
> Hermes self-evolution), yalnızca "trace-tabanlı öneri + insan-onay-gate"i ENS invariant'larına
> bağlar (§Prior art). +1 failure condition (öneri-üretim mekanizması henüz operasyonelleşmedi,
> ENS-4025 L8'e bağımlı, Faz-4'te kodlanmadı — E1). `status: ratified → review`: additive
> revizyon, yazar kendi işini onaylamaz (G2/G3); bağımsız `ens-skeptic` turu → `survives` ile
> `ratified`'a döner.
>
> **v0.3.1 notu ([SKR-042](reviews/SKR-042-learning-theory-v03-reflective-double-loop.md)'ye
> yanıt — wounded → 3 blocking talep kapatıldı).** (T1) §4a'nın EV-sapma iddiası, ENS'in
> attribution çekirdeğine (§2: seçilmeyen Alternative'in Actual'ı asla gözlenmez) daraltıldı —
> "per-Alternative" kaldırıldı, iddia **yalnızca commit-edilen Alternative'in EV kalibrasyonuna**
> indirildi; seçilmeyen alternatiflerin EV-sapmasının ölçülemeyeceği açıkça kabul edildi. (T2)
> `principles`'a **P5** eklendi; öneri-hacmi/dikkat-bütçesi (öneri-yorgunluğu) yeni bir failure
> condition olarak yazıldı ve önceliklendirmenin ENS-3022 (Decision Gravity) gibi bir mekanizmaya
> bağlanabileceği — ama henüz operasyonelleşmediği — dürüstçe not düşüldü. (T3) §1 Definition'ın
> otomatik-güncelleme ima eden edilgen dili düzeltildi: sonucun kayda geçmesi (otomatik) ile
> double-loop model revizyonu (P7-gate'li öneri) ayrımı bir cümleyle netleştirildi. `status:
> review` KALIR — **öz-onay yok (G2/G3): bu tur `survives` işaretlenemez; bağımsız 2. `ens-skeptic`
> turunu bekler.** §SKR-042'ye yanıt tablosu sonda.

## Definition

**Learning, bir kararın `Expected` ve `Actual` sonucu arasındaki, karara _atfedilmiş_ farkın,
Company Memory'ye kaydedilmesi ve ilgililik/varsayım modeli için bir güncelleme _önerisine_
dönüştürülmesidir** (P4). Eğitim (training) değildir. Her commit-edilmiş karar (ENS-2001) başına
tanımlıdır ve yalnızca atfedilebilir olduğu ölçüde geçerlidir. Kritik: learning **hem sonuçtan
hem süreçten** öğrenir (§Model 5).

**İki farklı yazım — karıştırılmamalı (P7 ayrımı).** Learning iki türde çıktı üretir ve bunlar
farklı otonomi rejimlerine tabidir: (a) **sonucun kayda geçmesi** — bir kararın Actual/Expected
farkının ve attribution seviyesinin Company Memory'ye yazılması — bu, gözlenen bir olgunun
kaydıdır ve otomatiktir; (b) **ilgililik/varsayım modelinin revizyonu** — double-loop model
değişikliği (Assumptions, relevance ağırlıkları) — bu **asla otomatik yazılmaz; bir insanın
onayından geçen bir öneridir** (§4a, P7). Yani (a) olgu-kaydı, (b) model-taahhüdüdür; ENS
yalnızca (b)'yi P7-kapısına tabi tutar.

## Motivation — neden ölçülmüş sonuç?

P4: ölçülmeyen karar iyileşmez (LAW-LEARNING). Sonuç niyetle karşılaştırılmazsa iyileşme
gradyanı yoktur. Learning, ENS döngüsünü kapatır: Context → Reasoning → Decision → **Outcome
→ Learning → Memory** → (daha iyi) Context.

## Historical context — ve konumlandırma

| Öncül | Ne verdi | ENS ile örtüşme | ENS'in (dar) delta'sı |
|-------|----------|-----------------|------------------------|
| **Argyris & Schön** | single-loop / double-loop | Learning'in iki katmanı | Karar-başına, memory-destekli, attribution-kapılı |
| **Deming / Shewhart (PDCA)** | Plan-Do-Check-Act | Measurement→Learning kapanışı | Karar-başına + counterfactual + confidence |
| **Rubin potential outcomes; Pearl** | Y(a) vs Y(a′) nedensel atıf | Attribution çekirdeği | Commit-edilmiş karara uygulama, sınıfa kapsam |
| **Evidence-based medicine kanıt hiyerarşisi** | RCT > quasi-experiment > observational seviyeleri | **Attribution merdiveni (§3) bunu yansıtır** | Hiyerarşiyi karar atomuna + memory'ye bağlama |
| **RL credit assignment** (Sutton) | Sonucu eyleme atfetme | Attribution problemi | İnsan sorumlu (P7), sonuçlar pahalı/seyrek/gerçek |
| **Baron & Hershey (1988) outcome bias**; Duke (popülerleştirme) | İyi karar ≠ iyi sonuç | Karar vs sonuç kalitesi | ENS bunu **yapısal olarak** ayırır (§Model 5) |
| **GEPA / DSPy / Hermes self-evolution** (§Prior art) | trace oku → *neden* başarısız analiz et → hedefli öneri | Double-loop'un somut, otomatikleştirilebilir biçimi (§4a) | **Öneri**, aksiyon değil (P7 gate); insan-PR-onayı; attribution-merdivenine (§3) bağlı |

**Dürüst delta:** ENS ne nedensel çıkarımı, ne öğrenme döngüsünü, ne kanıt hiyerarşisini, ne de
trace-tabanlı öz-iyileştirmeyi icat eder. Dar katkısı: bunları **commit-edilmiş karar atomu**
üzerinde birleştirmek, attribution'ı **güven-etiketli merdivene** oturtmak (§3), karar kalitesini
sonuçtan yapısal olarak ayırmak (§5) ve double-loop'u **trace-tabanlı ama insan-onaylı** bir öneri
mekanizmasına (§4a) somutlaştırmak — üstelik event-sourcing'in donmuş snapshot'ıyla hindsight'a
dirençli.

### Prior art — trace-tabanlı reflective double-loop (v0.3)
§4a'nın "proof-trace oku → *neden* başarısız analiz et → hedefli iyileştirme öner" akışı **ENS'in
icadı değildir**; agent self-evolution / prompt-pipeline optimizasyon mühendisliğinden bilinçli bir
sentezdir. Argyris & Schön double-loop'un *ne* olduğunu (varsayımları güncelle) söyler ama *kim/nasıl*
tetikler ve yürütür sorusunu operasyonelleştirmez — o, aşağıdaki katmanın katkısıdır. Dürüst
konumlama (SKR-001 dersi: dar delta önden):

| Kaynak | Ne verdi | ENS'in kullanımı / delta |
|--------|----------|--------------------------|
| **GEPA** (Genetic-Pareto prompt evolution; "GEPA: Reflective Prompt Evolution Can Outperform Reinforcement Learning", arXiv:2507.19457) | execution-trace'i okuyup **"neden başarısız oldu"** (yalnızca "başarısız" değil) reflektif olarak analiz eden ve hedefli aday-varyant üreten optimizer | Reflektif "neden" analizinin kaynağı — ENS'te double-loop tetikleyicisi olarak §4a; ama ENS **prompt değil Assumptions/relevance-model/attribution-seviyesi** günceller ve çıktı bir **öneridir** |
| **DSPy** (Stanford; declarative self-improving pipelines; arXiv:2310.03714 / dspy.ai) | prompt/pipeline'ı eval dataset'e karşı programatik optimize eden framework | Optimize-et-değerlendir döngüsü deseninin kaynağı — ENS'te L1+ attribution sinyaliyle beslenir |
| **Hermes Agent self-evolution** (Nous Research; github.com/NousResearch/hermes-agent-self-evolution) | GEPA+DSPy ile skill/prompt/kod evrimi + **constraint-gate** (pytest %100, boyut sınırı, semantic-preservation) + **her değişiklik insan-PR-review; otonom commit YOK** | "constraint-gate + insan-onay, otonom commit yok" deseninin kaynağı — ENS'in **P7 (Bounded Autonomy, Madde III)** öğrenme-döngüsü karşılığının doğrudan eşi |

**Dürüst delta (dar).** Trace-tabanlı reflektif öz-iyileştirme yeni bir mekanizma **değildir**;
ENS'in bu turdaki katkısı bir mekanizma icadı değil, **üç bağlamadır**: (a) reflektif "neden" analizi
prompt'a değil **commit-edilmiş karar atomunun** proof-trace'ine (ENS-4025 L8) ve §3 attribution
seviyesine bağlanır; (b) çıktı skill/kod-mutasyonu değil, **Assumptions / relevance-model /
attribution-seviyesi** için bir öneridir; (c) GEPA/Hermes'in "constraint-gate + insan-PR-onayı"
ilkesi ENS'te **P7 invariant'ıyla** özdeşleştirilir — öneri hiçbir zaman otomatik uygulanmaz. Bu,
orijinal teori değil, ENS invariant'larına (P7 + proof-trace + attribution-merdiveni) bağlanmış
mühendislik sentezidir.

## Theoretical model

### 1. Learning nedir — prediction error + memory update
`Expected Outcome` saklanmış bir öngörüdür; bu yüzden en yalın sinyal her zaman mevcuttur:
```
learning_signal(d) = Actual(d) − Expected(d)
```
Dış kontrol grubu gerektirmez — kararın kendi öngörüsü bir counterfactual temelidir. Learning
bu farkı memory'ye **kaydeder** (olgu-kaydı, otomatik) ve ilgililik/varsayım modeli için bir
**güncelleme önerisine** dönüştürür (P7-gate'li, §Definition'daki (a)/(b) ayrımı — model
asla otomatik yazılmaz).

### 2. Attribution problemi (kilit taşı)
Prediction error, farkın karardan mı confounder'dan mı geldiğini söylemez. Potential-outcomes:
karar `d`, `a`'yı seçti; `Y(a)` gözlendi; tam attribution `Y(a′)` (seçilmeyen alternatifler)
tahminini ister — bir counterfactual. RCT nadir olduğundan ENS attribution'ı *kesinlik* değil,
**dereceli güven** olarak ele alır.

### 3. Attribution merdiveni (R2'yi öder) — kanıt hiyerarşisine dayanır
Bu merdiven, evidence-based medicine'in nedensel-kanıt hiyerarşisinin karar-atomuna
uygulanmasıdır (ENS onu icat etmez):

| Seviye | Yöntem | Ne öğrenilir | Güven |
|--------|--------|--------------|-------|
| **L0** | atıf yok (saf confounding) | yalnızca kayıt (memory) | — |
| **L1** | model-temelli: Actual vs Expected | öngörü hatası (her zaman mevcut) | düşük-orta |
| **L2** | quasi-experimental: Memory Graph eşleme, doğal deney | karşılaştırmalı etki | orta |
| **L3** | experimental: kasıtlı A/B (nadir, pahalı) | nedensel etki | yüksek |

Her learning, seviyesi + attribution confidence ile saklanır (OC3: Context relevance için en
az L1 her zaman hesaplanabilir).

### 4. Single-loop vs double-loop (OM2)
- **Single-loop:** `Expected` modelini düzelt (daha iyi öngör).
- **Double-loop:** öngörü hatası sistematikse, **Assumptions** ve **ilgililik modelini**
  güncelle → ENS-2002 relevance ağırlıkları ve ENS-2003 retention buradan iyileşir.

### 4a. Reflective double-loop (v0.3 — operasyonel katman)
§4 double-loop'un *ne* yapılacağını söyler ("öngörü hatası sistematikse Assumptions'ı güncelle")
ama **kim** tespit eder, **nasıl** "sistematik"i tanır ve güncellemeyi **kim** yürütür sorusunu
operasyonelleştirmez. §4a o eksik katmanı ekler. Bu yeni bir yasa değildir; §Prior art'taki
GEPA/DSPy/Hermes mühendisliğinin, ENS'in zaten söz verdiği ilkelere (§4 double-loop + P7 +
§3 attribution-merdiveni) bağlanmasıdır.

**Akış (üç adım, hepsi öneri-üretimiyle biter).**
1. **Trace oku.** Girdi, geçmiş commit-edilmiş kararların (ENS-2001 donmuş snapshot'ı) ve onların
   türetimlerinin **proof-trace'idir** (ENS-4025 L8: her türetilmiş olgu kuralını + öncüllerini
   taşır). Trace, tek bir kararın "başarısız oldu" bitini değil, **öngörünün hangi öncül/varsayım
   zincirinden çıktığını** görünür kılar — reflektif analizin substratı budur.
2. **"Neden" analiz et (yalnızca "ne" değil).** Bir *sınıf* karar üzerinde (aynı Purpose-tipi,
   §ENS-2003 Model 2) öngörü hatası **sistematik** mi? GEPA'nın ayrımı buradadır: yalnızca
   `learning_signal(d) ≠ 0` olduğunu değil, hatanın **hangi ortak varsayımdan / hangi ilgililik-
   ağırlığından / hangi attribution seviyesinden** geldiğini proof-trace üzerinden izler. Sistematik
   sinyal en az **L1 attribution** (§3) ile etiketlenir; L2/L3 varsa öneri daha yüksek güven taşır.
3. **Hedefli öneri üret — asla uygulama.** Çıktı, üç yerden biri için bir **iyileştirme önerisidir:**
   (i) bir **Assumption**'ın revizyonu, (ii) ENS-2002 **relevance-model** ağırlığının ayarı, ya da
   (iii) bir karar sınıfının **attribution-seviyesi** hedefinin yükseltilmesi (ör. "bu Purpose-tipi
   L1'e sıkışıyor, L2 doğal-deney eşlemesi kurulabilir"). Öneri, ürettiği proof-trace ile birlikte
   sunulur (P6/Explainability).

**P7 kapısı (Bounded Autonomy, Anayasa Madde III) — mekanizmanın çekirdeği.** Öneri **hiçbir zaman
otomatik uygulanmaz.** Assumptions/relevance-model/attribution-hedefi güncellemesi, bir insanın
onayından geçer. Bu, GEPA/Hermes'in "constraint-gate + insan-PR-review, otonom commit yok"
ilkesinin ENS-tarafı karşılığıdır ve ENS'in "öneri sunar, emretmez" duruşunun öğrenme-döngüsündeki
somut biçimidir. Reflective double-loop bir **öneri üreticisidir**, bir mutasyon motoru değil:
constraint-gate (ör. öneri kendi proof-trace'ini taşımalı, en az L1 etiketli olmalı) + insan onayı,
never-auto-apply invariant'ını korur.

**Dikkat kıt kaynaktır (P5) — öneri hacminin sınırlanması.** Zorunlu-insan-onay-kapısı (P7) tek
başına yeterli değildir: bir öneri-üreticisi hacimli öneri üretirse, kıt insan dikkati (P5) taşar
ve kapı ya lastik-damgaya (rubber-stamp — P7 fiilen boşalır) ya da görmezden gelmeye (mekanizma
işlevsiz) dejenere olur (bkz. §Failure). GEPA/Hermes bu gerilimi makine-doğrulanabilir gate ile
(pytest %100, boyut sınırı) insan-yükünü *önceden filtreleyerek* yönetir; §4a'nın constraint-gate'i
(proof-trace + L1-etiket) yalnızca önerinin *biçimini* kısıtlar, *hacmini* değil. Bu yüzden §4a
öneri hacmini bir **dikkat bütçesine** tabi tutmalıdır: öneriler önceliklendirilmeli (ör. Decision
Gravity, ENS-3022 — yüksek-ağırlıklı karar sınıfları önce) ve/veya bir eşik/batch ile sınırlanmalı,
böylece insan onayı ölçeklenebilir kalır. **Bu önceliklendirme henüz operasyonelleşmemiştir** — hangi
mekanizmanın (Decision Gravity, eşik, batch) hacmi kestiği tasarım borcudur (§Failure).

**§Failure #1'e (beklenen-değer elicitasyonu) doğrudan bağlantı — attribution çekirdeğiyle sınırlı.**
Trace-tabanlı analiz, **yalnızca commit-edilen (seçilen) Alternative'in** beklenen-değer tahmininin
(Decision Object, ENS-2001 v0.3 `Expected Value`) bir karar sınıfında **sistematik olarak sapmış**
olup olmadığını — yani o Alternative'in `Expected Value`'unun `Actual`'a karşı ıraksamasını
(`learning_signal`, §1) — tespit edip kalibrasyonunun iyileştirilmesini *önerebilir* (uygulamaz).
**Seçilmeyen Alternative'lerin EV-sapması ölçülemez:** §2 gereği seçilmeyen `a′` için `Y(a′)` (Actual)
**asla gözlenmez** — bir counterfactual'dır (RCT hariç) — dolayısıyla onların EV tahminleri hiçbir
zaman outcome'a karşı doğrulanamaz. Mekanizma bu sınırı miras alır (ENS-2001 v0.3 §Failure de
seçilmeyen Alternative EV'lerinin "kaydedilmez ya da kaba" olduğunu kabul eder): §4a'nın EV-sapma
sinyali **yalnızca seçilen Alternative'in EV kalitesine** dairdir, per-Alternative bir kalibrasyon
iddiası değildir. Bu, §5(ii) seçim rasyonalitesinin girdisini zamanla keskinleştirir; ama önerinin
*kendisi* insan onayına tabidir (P7).

### 5. Karar kalitesi ≠ sonuç kalitesi — üç ayrı bileşen (SKR-009 Bulgu 1-2)
Outcome bias (Baron & Hershey 1988): iyi sonucu iyi karara eşitlemek. ENS karar kalitesini
**sonuçtan yapısal olarak ayırır** ve üç ayrı şey öğrenir:

**(i) Sonuç öğrenimi** — Actual vs Expected (§1). Öngörü isabeti. Sonuca bağlı.

**(ii) Seçim rasyonalitesi (outcome-bağımsız, karar-başına).** Kararın **kendi** Confidence/
beklenen-değer tahminlerine göre, seçilen Alternative en iyisi miydi? Bu, yalnızca Decision
Object'ten (Alternatives + Confidence + beklenen değerler) hesaplanır — **sonucu bilmeye gerek
yok.** İyi kalibre ama kötü seçen bir karar (düşük-EV Alternative'i seçmek) burada yakalanır;
§v0.1 bunu atlıyordu.

**(iii) Confidence kalibrasyonu (outcome-temelli, toplam).** Çok karar üzerinde: "0.7 dediğim
kararların ~%70'i tuttu mu?" (Brier/kalibrasyon eğrisi). Bu, tek kararı değil **öngörücüyü**
ölçer; sonuç gerektirir ama toplamda, tek sonuca değil.

**Hindsight koruması (Bulgu 2):** (ii) ve süreç değerlendirmesi, ENS-2001'in event-sourced
**donmuş commitment snapshot'ı** üzerinden yapılır — commitment anında kayıtlı Context,
Alternatives ve Confidence, **sonuç bilgisi olmadan.** Snapshot değişmez (event-sourcing)
olduğundan, değerlendirici sonuca göre süreci yeniden yorumlayamaz. Böylece Baron-Hershey outcome
bias'ı *yapısal olarak* engellenir.

**Sonuç:** kötü sonuç + iyi seçim (ii) + kalibre confidence (iii) = **iyi karar, şanssız.** ENS
bunu başarısızlık kodlamaz; belleğe *karar kalitesini* yazar, yalnızca sonucu değil.

## Implications
- **Nicel katmanın temeli:** Decision Entropy/Gravity/Capital, L1+ attribution'lı learning'e
  ve karar-kalitesi ayrımına dayanır.
- **Memory *retrieval ağırlığı*** = `|learning_signal| × attribution_confidence`. Bu nicelik
  **ENS-3023 §Model 1'in `value(d)`'sidir** (yeni kavram değil, alias yasağı — Anayasa Madde IV):
  bir dersin yeni bir kararı *ne kadar ağırlıkla yönlendireceğini* söyler.
  **Retention önceliği bundan AYRIDIR ve `attribution_confidence`'tan bağımsızdır**
  (= `|learning_signal|`; ENS-2003 v0.4.0 §3): neyin *kaybolmayacağı* ile neyin *ne kadar
  ağırlıkla konuşacağı* farklı sorulardır. (v0.3.2'ye kadar bu satır tek bir formülle iki soruyu
  birden yanıtlıyor ve **kendi içinde çelişiyordu** — aynı satırda ENS-2003'ün `∝|Learning|`
  tanımına atıf yapıyordu. AUDIT-WAVE2-FIDELITY/D-5, bu çelişkinin ENS-2003 §3a'daki sonucunu —
  attribution confidence'ın hem ağırlıkta hem sönümde sayılması — bağımsız olarak yakaladı.)
- **Zayıf attribution artık *sessiz bir ceza* değil, bir talep üretir.** ENS-2003 v0.4.0 §3b'nin
  `weakly-attributed` inceleme sinyali, §4a adım 3(iii)'ün (bir karar sınıfının attribution-seviyesi
  hedefini yükseltme önerisi) doğrudan tetikleyicisidir: "bu Purpose-tipi L1'e sıkışıyor" gözlemi,
  bellek katmanından gelir.
- **Context relevance** (ENS-2002) L1 attribution sinyalini **veri olarak** kullanır (relevance
  bir hesaplamadır, P7-gate'li model-güncellemesi değil) (OC3 kapandı).

## Relationships
- **→ Decision Theory (ENS-2001):** learning karar-başına; donmuş snapshot hindsight korur;
  §4a reflective double-loop bu snapshot'ların proof-trace'ini okur.
- **→ Context Theory (ENS-2002):** double-loop relevance'ı günceller; §4a relevance-model ayarını
  **önerir** (insan onayıyla uygulanır, P7).
- **→ Company Memory (ENS-2003):** learning memory'ye yazılır; retention'ı belirler.
- **→ Semantic Logic (ENS-4025):** §4a'nın trace substratı L8 zorunlu proof-trace'tir; reflektif
  analiz kuralı + öncülleri gören türetimlere bağımlıdır (bu, §Failure'da açık implementasyon borcudur).
- **→ LAW-LEARNING:** bu kavram yasanın operasyonel biçimidir.

## Examples
**L1:** Expected −%3, Actual −%2 → öngörü hatası; single-loop: elastikiyet yüksek tahmin edilmiş.
**Seçim rasyonalitesi (ii):** üç Alternative'in beklenen değeri {+10, +4, −2}; karar +4'ü
seçmiş → kalibre olsa bile seçim suboptimal; ENS bunu sonuçtan bağımsız yakalar.
**Resulting'den kaçınma:** iyi-context, kalibre, en-iyi-EV seçilen tedarikçi kararı dış şokla
kötü sonuç verdi → donmuş snapshot üzerinde süreç "iyi" değerlendirilir; başarısızlık kodlanmaz.
**Reflective double-loop (§4a):** "fiyat belirle" Purpose-tipi'nde son 12 kararın proof-trace'i
okunur; hepsinin Expected'ı "rakip fiyatı sabit kalır" Assumption'ından türemiş ama Actual sistematik
sapmış (L1). Mekanizma bu ortak varsayımı işaret eder ve *önerir*: "bu Assumption'ı 'rakip fiyatı
volatil' ile revize et / relevance-model'de rakip-fiyat context'ine ağırlık ver." Öneri proof-trace'i
ile sunulur; **uygulanmaz** — Owner onaylarsa güncellenir (P7).

## Laws
**LAW-LEARNING**'in operasyonel biçimidir ve keskinleştirir: *ölçüm attribution seviyesiyle
etiketli olmalı; L0'da learning yok; ve öğrenilen şey sonuç kalitesiyle karıştırılmamalı.*

**§4a yeni bir yasa değildir.** Reflective double-loop, yeni bir LAW önermez; var olan §4
double-loop'un + P7'nin (Bounded Autonomy) + §3 attribution-merdiveninin bir **sentezini
somutlaştırır** (ENS-2003 v0.3'ün "yeni yasa değil, KG/RAG mühendisliğinin sentezi" çerçevesiyle
aynı disiplin). Katkı bir yasa değil, bir **operasyonel desendir** ve P7'ye tabidir: mekanizma
öneri üretir, LAW-LEARNING'in "ölç ve iyileş" buyruğunu insan-onaylı bir öneri-akışına dönüştürür.

## Failure conditions (Anayasa Madde X)
- **Beklenen-değer elicitasyonu (yeni, en ciddi).** Seçim rasyonalitesi (ii), her Alternative
  için kararın *kendi* beklenen-değer tahminini gerektirir; bu tahminler kayıtlı değil ya da
  kaba ise, (ii) ölçülemez ve karar kalitesi yine kalibrasyona düşer. Decision Object,
  Alternative-başına beklenen değeri saklamalı — aksi hâlde §5(ii) boş kalır.
- **Kalibrasyon için hacim.** (iii) çok karar ister; az kararlı bir Owner/Purpose-tipinde
  kalibrasyon istatistiksel olarak zayıftır.
- **Counterfactual infeasibility.** L2/L3 çoğu kararda mümkün değil; sistem L1'e sıkışırsa
  learning nedensel değil, öngörü-hatasına indirgenir.
- **Feedback gecikmesi + Goodhart.** Sonuçlar geç ölçülür; ve kalibrasyon/seçim metriği hedefe
  dönüşürse oyunlanır (Confidence manipülasyonu).
- **Öneri-yorgunluğu — P5 (Attention kıt kaynaktır) devrede değil (v0.3.1, en güçlü hâliyle).**
  §4a bir öneri-üreticisidir ve her çıktısı zorunlu insan onayına (P7) tabidir. Bu iki invariant
  hacim altında çatışır: mekanizma çok öneri üretirse ya (a) insan onayı lastik-damgaya (rubber-stamp)
  döner — P7 *fiilen* boşalır, gözetimsiz otomasyona eşdeğer hâle gelir — ya da (b) öneriler görmezden
  gelinir — mekanizma işe yaramaz. GEPA/DSPy otomasyonunun cazibesi tam da hacimdedir; ENS o hacmi
  kıt insan dikkatine (P5) boşaltır. GEPA/Hermes insan-yükünü makine-doğrulanabilir gate (otomatik
  test, boyut sınırı) ile *önceden* kısar; §4a'nın gate'i (proof-trace + L1-etiket) yalnızca önerinin
  biçimini kısıtlar, hacmini değil — bu, Hermes-analojisinin en zayıf halkasıdır ve dürüstçe kabul
  edilir. Çözüm yönü — öneri hacmini bir dikkat bütçesine (P5) tabi kılmak, önceliklendirmeyi bir
  mekanizmaya (ör. Decision Gravity, ENS-3022; ya da InfoNeed-tipi bir ihtiyaç sinyali) bağlamak,
  eşik/batch ile sınırlamak — **işaret edilmiştir ama henüz operasyonelleşmemiştir**; hangi mekanizmanın
  hacmi kestiği ve dikkat bütçesinin nasıl formelleştirileceği açık tasarım borcudur (E1). Bu borç
  kapanmadan §4a'nın P7-gate'i ölçekte savunulamaz.
- **Reflective double-loop henüz operasyonelleşmedi (v0.3, en güçlü hâliyle).** §4a bir *desen*
  tanımlar ama iki kritik parça henüz belirsizdir: (a) **hangi "trace" okunacak** — ENS-4025 L8
  proof-trace altyapısı bu ölçekte var ama karar-öngörüsünün türetim zinciri (Assumption→Expected
  eşlemesi) henüz proof-trace formatına bağlanmadı; (b) **hangi sinyal "sistematik"i tetikler** —
  eşik (kaç kararlık sınıf, hangi |learning_signal| dağılımı, hangi attribution seviyesi) henüz
  formelleştirilmedi. Dolayısıyla §4a bugün bir **teorik desen sentezidir, çalışan bir mekanizma
  değil**: ENS-4025 L8'e bağımlı ve Faz-4'te **kodlanmadı** — eng-kanıt **E1** (tasarlanmış-ama-
  implemente-değil). Dış sistemler (GEPA/DSPy/Hermes) desen-sınıfının çalıştığını gösterir ama
  ENS'in karar-atomu + attribution-seviyesi + P7-gate'e özgü bağlamasını doğrulamaz. Dürüstçe açık;
  önerinin *kalitesi* de (yanlış "neden" atfı → yanlış Assumption revizyonu önerisi) memory
  poisoning (ENS-2003 §Failure) ile aynı R2/attribution borcuna zincirlidir.

## SKR-009'a yanıt
| Talep | Karşılandığı yer |
|-------|------------------|
| 1. Karar kalitesini ikiye ayır (kalibrasyon + seçim rasyonalitesi) | §Model 5 (ii)+(iii) |
| 2. Hindsight koruması (donmuş snapshot) | §Model 5 (hindsight koruması) |
| 3. Prior art: merdiven→kanıt hiyerarşisi, outcome bias→Baron-Hershey | §Historical, §Model 3 |

> **v0.3 additive değişiklik (skeptic bekliyor):** §4a Reflective double-loop, §Prior art
> (GEPA/DSPy/Hermes atıfları), +1 failure condition, Laws'ta "yeni yasa değil" netleştirmesi,
> `depends_on: +ENS-4025`. **Öz-onay yok (Anayasa G2/G3):** bu tur yazar tarafından `survives`
> işaretlenemez; `status: review` bağımsız bir `ens-skeptic` turunu bekler. Sözlüğe 1 terim
> (Reflective Double-Loop) M1 olarak girdi (ENS-4000, skeptic bekliyor).

## SKR-042'ye yanıt (v0.3.1 — wounded → 3 blocking talep)
| Talep | Karşılandığı yer | Nasıl |
|-------|------------------|-------|
| **T1.** EV-sapma iddiasını attribution çekirdeğine hizala; "per-Alternative"ı seçilen Alternative'e daralt, seçilmeyen EV'nin ölçülemezliğini kabul et (§2, ENS-2001 §Failure) | §4a, "§Failure #1'e doğrudan bağlantı — attribution çekirdeğiyle sınırlı" | "per-Alternative" kaldırıldı; iddia **yalnızca commit-edilen Alternative'in EV kalibrasyonuna** (learning_signal, §1) indirildi; seçilmeyen `a′` için `Y(a′)`'nin asla gözlenmediği (§2) ve ENS-2001 §Failure'ın unchosen-EV kırılganlığının miras alındığı açıkça yazıldı |
| **T2.** P5'i devreye al + öneri-yorgunluğu failure condition ekle; hacmi neyle sınırladığını yanıtla | künye `principles: +P5`; §4a "Dikkat kıt kaynaktır (P5)" paragrafı; §Failure "Öneri-yorgunluğu" koşulu | P5 principles'a eklendi; hacim→rubber-stamp/ignore riski adlandırıldı; Hermes'in makine-gate'inin insan-yükünü kıstığı, ENS'in gate'inin yalnızca biçimi kısıtladığı kabul edildi; önceliklendirmenin ENS-3022 (Decision Gravity)/eşik/batch'e bağlanabileceği ama **henüz operasyonelleşmediği** dürüstçe not düşüldü (E1) |
| **T3.** §1 Definition ↔ §4a gate'ini uzlaştır (otomatik kayıt ≠ P7-gate'li model revizyonu) | §1 Definition (yeniden yazım + "İki farklı yazım" paragrafı) | Edilgen "ilgililik/varsayım modeline yazılan güncelleme" → "güncelleme *önerisine* dönüştürülmesi"; ayrıca (a) sonucun kayda geçmesi=otomatik olgu-kaydı ile (b) model revizyonu=P7-gate'li öneri açıkça ayrıştırıldı |

> **Öz-onay yok (Anayasa G2/G3):** bu düzeltmeler yazar tarafından `survives` işaretlenemez;
> `status: review` **bağımsız 2. bir `ens-skeptic` turunu** bekler. Yazar kendi yanıtını onaylamaz.

## SKR-043'e yanıt (v0.3.2 — T3-artığı kapanışı) ve SKR-044 kapanışı
| Artık | Kapatıldığı yer | Nasıl |
|-------|-----------------|-------|
| **T3-artığı (a).** §Theoretical model §1 (eski satır 128) hâlâ "…memory'ye **ve ilgililik/varsayım modeline yazar**" (otomatik-yazma kipi), §Definition'ın (b)-kuralıyla çelişiyordu | §1 (satır 128-130) | "…memory'ye **kaydeder** (olgu-kaydı, otomatik) ve ilgililik/varsayım modeli için bir **güncelleme önerisine** dönüştürür (P7-gate'li — model asla otomatik yazılmaz)". "otomatik" yalnızca olgu-kaydına (Actual−Expected) scope'lanır, model-güncellemesine sızmaz |
| **T3-artığı (b).** §Implications (eski satır 239) "Context relevance L1 ile **beslenir**" edilgen-otomatik ekosu | §Implications (satır 241-242) | "…L1 attribution sinyalini **veri olarak** kullanır (relevance bir hesaplamadır, P7-gate'li model-güncellemesi değil)" — relevance-hesabı (veri-in, otomatik) ile relevance-model-revizyonu (P7-gate'li öneri) ayrıştırıldı |

> **v0.3.2 → SKR-044 (bağımsız 3. tur, taze context, G2/G3) → `survives`.** T3-artığının iki
> parçası TAM kapandı; ayrıca dosyanın **tamamı** (Definition, §Model 1-5, Implications,
> Relationships, Examples, Laws, Failure) otomatik-yazma/edilgen-güncelleme dili için sistematik
> tarandı — SKR-043'ün "bir yerde kapatılıp başka yerde unutulan çelişki" deseni **3. kez tekrar
> etmedi**. §4'ün double-loop "güncelle" dili kavramsal-tanımdır ve §4a onu açıkça P7-kapısına
> devreder (kavram→operasyonelleştirme, çelişki değil). `status: review → ratified`. **`canon:
> false` KALIR** — Külliyat-girişi ayrı governance edimidir; SKR-044 yalnızca skeptic-kapısını
> geçirir. Bkz. `reviews/SKR-044-learning-theory-v032-t3-closure-fulldoc-scan.md`.

## v0.3.3 — §Implications hizalaması (AUDIT-WAVE2-FIDELITY / D-5'in yan etkisi)

**Neden bu belgeye dokunuldu (gerekçe zorunlu — ENS-2004 ayrı bir belgedir).** D-5, ENS-2003 §3a'da
attribution confidence'ın hem retention ağırlığında hem sönüm hızında sayıldığını gösterdi. Bu
çift-sayımın **teori kaynağı kısmen buradaydı:** §Implications *"Memory retention = |learning_signal|
× attribution_confidence (ENS-2003 ∝|Learning| tanımlı)"* diyordu — **tek satırda iki farklı formül**,
biri metinde biri parantezde. ENS-2003 v0.4.0 iki niceliği ayırdığında bu satır ya düzeltilmeli ya
da iki belge açıkça çelişecekti. Değişiklik **dar ve additive'dir:** yeni bir iddia getirmez, var
olan bir **iç çelişkiyi** giderir ve `|L|·c`'yi zaten sahibi olan kavrama (ENS-3023 §Model 1
`value(d)`) iade eder. §4a, attribution merdiveni, EV-sapma sınırı, P5/P7 tartışması ve §Failure
conditions **değişmemiştir**; SKR-042/043/044'ün kapattığı hiçbir madde yeniden açılmamıştır.

| Değişen | Eski | Yeni |
|---------|------|------|
| §Implications 2. madde | "Memory retention = \|learning_signal\| × attribution_confidence" | "Memory **retrieval ağırlığı** = \|learning_signal\| × attribution_confidence (= ENS-3023 `value(d)`); **retention önceliği ayrıdır ve `c`'den bağımsızdır**" |
| §Implications (yeni madde) | — | `weakly-attributed` sinyali (ENS-2003 v0.4.0 §3b) → §4a adım 3(iii)'ün tetikleyicisi |

> **Öz-onay yok (Anayasa G2/G3):** `status: ratified → review`. Bu hizalama yazar tarafından
> `survives` işaretlenemez; **bağımsız bir `ens-skeptic` turu** bekler. Skeptic'e özel soru:
> *bu değişiklik gerçekten additive mi, yoksa §5(iii) kalibrasyon argümanının bir öncülünü sessizce
> mi değiştiriyor?*

---

*Learning, sonucu niyetle karşılaştırmak ve belleği güncellemektir — ama şansı beceriden
ayırarak. ENS iyi kararı kötü sonuç için cezalandırmaz: seçimini donmuş anına bakarak, öngörüsünü
sonucuna bakarak, güvenini çok karara bakarak — ayrı ayrı öğrenir.*
