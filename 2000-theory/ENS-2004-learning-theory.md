---
id: ENS-2004
title: Learning Theory (ENS)
type: theory
canon: false
maturity: M3
origin: ENS-0000 §III (P4), ENS-2001, ENS-2003
depends_on: [ENS-0000, ENS-1000, ENS-2001, ENS-2002, ENS-2003, ENS-4000]
referenced_by: [ENS-2002, ENS-2003, ENS-4010]
principles: [P4, P3, P6, P7]
status: ratified
owner: ens-philosopher
version: 0.2.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-010
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

## Definition

**Learning, bir kararın `Expected` ve `Actual` sonucu arasındaki, karara _atfedilmiş_ farkın,
Company Memory'ye ve ilgililik/varsayım modeline yazılan bir güncellemeye dönüştürülmesidir**
(P4). Eğitim (training) değildir. Her commit-edilmiş karar (ENS-2001) başına tanımlıdır ve
yalnızca atfedilebilir olduğu ölçüde geçerlidir. Kritik: learning **hem sonuçtan hem süreçten**
öğrenir (§Model 5).

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

**Dürüst delta:** ENS ne nedensel çıkarımı, ne öğrenme döngüsünü, ne kanıt hiyerarşisini icat
eder. Dar katkısı: bunları **commit-edilmiş karar atomu** üzerinde birleştirmek, attribution'ı
**güven-etiketli merdivene** oturtmak (§3) ve karar kalitesini sonuçtan yapısal olarak ayırmak
(§5) — üstelik event-sourcing'in donmuş snapshot'ıyla hindsight'a dirençli.

## Theoretical model

### 1. Learning nedir — prediction error + memory update
`Expected Outcome` saklanmış bir öngörüdür; bu yüzden en yalın sinyal her zaman mevcuttur:
```
learning_signal(d) = Actual(d) − Expected(d)
```
Dış kontrol grubu gerektirmez — kararın kendi öngörüsü bir counterfactual temelidir. Learning
bu farkı memory'ye ve ilgililik/varsayım modeline yazar.

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
- **Memory retention** = |learning_signal| × attribution_confidence (ENS-2003 ∝|Learning| tanımlı).
- **Context relevance** (ENS-2002) L1 ile beslenir (OC3 kapandı).

## Relationships
- **→ Decision Theory (ENS-2001):** learning karar-başına; donmuş snapshot hindsight korur.
- **→ Context Theory (ENS-2002):** double-loop relevance'ı günceller.
- **→ Company Memory (ENS-2003):** learning memory'ye yazılır; retention'ı belirler.
- **→ LAW-LEARNING:** bu kavram yasanın operasyonel biçimidir.

## Examples
**L1:** Expected −%3, Actual −%2 → öngörü hatası; single-loop: elastikiyet yüksek tahmin edilmiş.
**Seçim rasyonalitesi (ii):** üç Alternative'in beklenen değeri {+10, +4, −2}; karar +4'ü
seçmiş → kalibre olsa bile seçim suboptimal; ENS bunu sonuçtan bağımsız yakalar.
**Resulting'den kaçınma:** iyi-context, kalibre, en-iyi-EV seçilen tedarikçi kararı dış şokla
kötü sonuç verdi → donmuş snapshot üzerinde süreç "iyi" değerlendirilir; başarısızlık kodlanmaz.

## Laws
**LAW-LEARNING**'in operasyonel biçimidir ve keskinleştirir: *ölçüm attribution seviyesiyle
etiketli olmalı; L0'da learning yok; ve öğrenilen şey sonuç kalitesiyle karıştırılmamalı.*

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

## SKR-009'a yanıt
| Talep | Karşılandığı yer |
|-------|------------------|
| 1. Karar kalitesini ikiye ayır (kalibrasyon + seçim rasyonalitesi) | §Model 5 (ii)+(iii) |
| 2. Hindsight koruması (donmuş snapshot) | §Model 5 (hindsight koruması) |
| 3. Prior art: merdiven→kanıt hiyerarşisi, outcome bias→Baron-Hershey | §Historical, §Model 3 |

---

*Learning, sonucu niyetle karşılaştırmak ve belleği güncellemektir — ama şansı beceriden
ayırarak. ENS iyi kararı kötü sonuç için cezalandırmaz: seçimini donmuş anına bakarak, öngörüsünü
sonucuna bakarak, güvenini çok karara bakarak — ayrı ayrı öğrenir.*
