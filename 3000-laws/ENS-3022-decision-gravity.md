---
id: ENS-3022
title: Decision Gravity
type: law
canon: false
constitutive: false
maturity: M3
origin: ENS-0000 §III (P2, P5), ENS-3000 (LAW-DECISION-GRAVITY), ENS-2001, ENS-2002
depends_on: [ENS-0000, ENS-2001, ENS-2002, ENS-2003, ENS-2004, ENS-3000, ENS-4000]
referenced_by: []
principles: [P2, P5, P1, P6]
status: ratified
owner: ens-philosopher
version: 0.2.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-014
evidence: {sci: E3, eng: E1, ops: E0, econ: E0}
---

# Decision Gravity

> LAW-DECISION-GRAVITY'nin biçimsel, **ölçülebilir** hâli; ikinci nicel yasa. `canon: false`
> — skeptic'ten sağ çıkınca Külliyat'a girer.
>
> **v0.2 notu:** [SKR-013](reviews/SKR-013-decision-gravity.md)'e yanıt. (1) Gravity artık
> **stake × belirsizlik** (Howard-VOI ile hizalı; belirsizlik = Confidence'tan), stake-tek
> değil; (2) "normatif" karışıklığı çözüldü — **peer-uyum** ile **VOI-normatif optimum** ayrıldı;
> (3) stake, Purpose-tipi içinde normalize edildi. §Yanıt tablosu sonda.

## Definition

**Decision Gravity, bir kararın _bilgi ihtiyacı_ (stake × belirsizlik) ile ona biriken
_context_ arasındaki ilişkidir.** Fiziksel çekim değildir — "gravity" yalnızca "çok riskli ve
belirsiz olan çok context hak eder" ilişkisinin adıdır. Operasyonel özü: bir kararın hak
ettiği context ile fiili context arasındaki **açık**, kıt attention'ın (P5) nereye gitmesi
gerektiğini söyler.

## Motivation

Değer, yasanın kendisi değil ihlalidir: yüksek stake **ve** yüksek belirsizlik ama düşük
context taşıyan kararlar — organizasyonun en tehlikelileri. Decision Gravity bunları bulup
attention'ı (P5) oraya yönlendirir. ENS kıt kaynağı dikkat olduğundan, Gravity ENS'in dikkat
triage motorudur.

## Historical context — ve konumlandırma

| Öncül | Ne verdi | ENS ile örtüşme | ENS'in (dar) delta'sı |
|-------|----------|-----------------|------------------------|
| **Howard (1966) Value of Information** | Bilginin değeri = stake **ve** belirsizliğin birlikte fonksiyonu; optimal bilgi yatırımı | **Kavramın kalbi** | ENS *fiili* birikimi ölçer, **peer** ve **VOI** çıtalarına karşı açığı hesaplar, attention'a çevirir; per-decision VOI değil, sürekli örgütsel |
| **Payne, Bettman & Johnson (1993)** | Effort önemle ölçeklenir | Effort↔stake | Örgütsel context birikimi, ölçülür |
| **Simon — bounded rationality/attention** | Attention kıt, tahsis edilir | P5 | Stake×belirsizliğe göre operasyonel tahsis |
| **Fiziksel gravity** | Kütle çeker metaforu | — | **Kullanılmaz** |

**Dürüst delta:** "riskli+belirsiz karar daha çok bilgi hak eder" özgün değil — **Howard VOI**
tam bunu, üstelik stake ve belirsizliği birlikte, formelleştirir. ENS'in dar katkısı: bunu
*popülasyon* üzerinde sürekli ölçmek, iki farklı çıtaya (peer-uyum ve VOI-optimum) karşı açığı
hesaplamak ve açığı **attention tahsisine** çevirmek.

## Theoretical model

### 1. Bilgi ihtiyacı = stake × belirsizlik (SKR-013 Bulgu 1)
```
Stake(d)       = spread( ExpectedValue(aᵢ) ) , aᵢ ∈ Alternatives(d)   (OL1'e bağlı)
Uncertainty(d) = 1 − Confidence(d)                                    (P6; kalibrasyonu ENS-2004)
InfoNeed(d)    = Stake(d) × Uncertainty(d)                            (VOI-proxy, Howard)
```
Yüksek stake + düşük belirsizlik (açık karar) → düşük InfoNeed → çok context gerekmez. Yüksek
stake + yüksek belirsizlik → yüksek InfoNeed. Stake-tek-başına modeli terk edildi.

**Stake normalizasyonu (SKR-013 Bulgu 3):** heterojen kararları kıyaslamak için Stake, kendi
**Purpose-tipi** içindeki geçmiş kararlara göre normalize edilir (z-skoru/persentil, ENS-2003
Memory'den). Böylece "yüksek stake" tip-içi görelidir ve finansal/itibari/stratejik kararlar
kıyaslanabilir olur.

### 2. İki çıta: peer-uyum vs VOI-normatif (SKR-013 Bulgu 2)
- **PeerContext(d) = E[ ContextDensity | InfoNeed(d) ]** (Memory'den fit) — *descriptive*
  benzer-ihtiyaçlı kararların tipik context'i. Buna karşı açık bir **uyum (conformance) açığı**dır,
  rasyonalite değil.
- **VOI-optimum:** context'i, marjinal VOI = marjinal maliyet olana dek topla (Howard). Bu
  gerçek *normatif* çıtadır ama per-decision hesabı pahalıdır.
- **Dürüst duruş:** ENS pratikte **PeerContext'i proxy** kullanır (sürekli, ucuz) ama bunun
  *uyum* ölçtüğünü, *optimum* olmadığını açıkça belirtir. Peer sistematik olarak az-context'liyse
  (ENS-2003 borçları), uyum açığı optimumu olduğundan küçük gösterir — bu bilinen bir sınır.
```
ConformanceDeficit(d) = PeerContext(d) − ContextDensity(d)
```

### 3. Attention tahsisi (P5 — operasyonel ödeme)
```
AttentionPriority(d) ∝ InfoNeed(d) × max(ConformanceDeficit(d), 0)
                     = Stake × Uncertainty × (peer-açık)
```
Yani: çok riskli + belirsiz + peer'a göre az-context'li kararlar dikkatin önüne çıkar. Bu,
Gravity'yi metafordan çıkarıp VOI'nin sürekli örgütsel biçimi yapar.

### 4. Normatif vs descriptive (SKR-001 uyarısı)
Panik kararı (yüksek stake+belirsizlik, düşük context) yasayı çürütmez; yüksek
AttentionPriority olarak *ölçülür* ve tam da dikkat gereken durumdur. Gravity hem eğilimi
(normatif VOI) hem ihlali (fiili açık) ayrı ayrı taşır.

## Implications
- **Attention motoru (P5):** dikkatin nereye gideceğinin nicel temeli; VOI-hizalı.
- **Decision Entropy ile:** yüksek InfoNeed + yüksek entropi + yüksek açık = kırmızı bölge.
- **Confidence'a bağımlılık:** belirsizlik terimi Confidence'tan gelir; Confidence kötü
  kalibreyse (ENS-2004) InfoNeed bozulur — kalibrasyon Gravity'yi de besler.

## Relationships
- **→ Decision (ENS-2001):** Stake = EV yayılımı (OL1); Uncertainty = 1−Confidence.
- **→ Context (ENS-2002):** ContextDensity fiili birikim.
- **→ Company Memory (ENS-2003):** PeerContext fiti + stake normalizasyonu.
- **→ Learning (ENS-2004):** Confidence kalibrasyonu belirsizlik terimini geçerli kılar.
- **→ Attention (P5); LAW-DECISION-GRAVITY:** operasyonel biçim.

## Examples
**Sermaye kararı:** tip-içi yüksek Stake, düşük Confidence (0.4 → yüksek belirsizlik) → yüksek
InfoNeed; ama 2 Evidence, peer tipik 15 → yüksek ConformanceDeficit → yüksek AttentionPriority.
**Açık ama büyük karar:** yüksek Stake ama Confidence 0.95 (düşük belirsizlik) → düşük InfoNeed
→ az context yeterli, boşuna dikkat çekilmez (stake-tek model bunu yanlış yapardı).

## Laws
**LAW-DECISION-GRAVITY**'yi operasyonelleştirir ve VOI'ye hizalar: *context, stake × belirsizlik
ile artmalıdır; artmadığında ölçülen açık attention'ı yönlendirir.* Yasa artık stake-tek değil.

## Failure conditions (Anayasa Madde X)
- **PeerContext optimum değil (bilinen sınır).** Uyum açığı, peer sistematik yanlıysa optimumu
  gizler; gerçek VOI-optimum pahalıdır. ENS proxy kullandığını açıkça taşır.
- **Confidence kalibrasyonuna bağımlılık.** Belirsizlik = 1−Confidence; Confidence kötü
  kalibreyse (ENS-2004 borçları) InfoNeed yanıltır. Gravity, kalibrasyon olgunluğuna bağlı.
- **Stake ölçümü OL1'e bağlı.** Alternative-başına EV yoksa Stake kaba; tip-içi normalizasyon
  yardımcı olur ama EV elicitasyonu gerekir.
- **Gaming.** Açık bir metrik olunca ilgisiz context bağlanarak kapatılabilir; ENS-2002
  noise_penalty sınırlar, tam engellemez.

## SKR-013'e yanıt
| Talep | Karşılandığı yer |
|-------|------------------|
| 1. Stake × belirsizlik (VOI hizası) | §Model 1 (InfoNeed = Stake × (1−Confidence)) |
| 2. Peer-uyum vs VOI-normatif ayır | §Model 2 (ConformanceDeficit + VOI-optimum, dürüst proxy) |
| 3. Stake normalizasyonu | §Model 1 (Purpose-tipi içi z-skoru/persentil) |

---

*Decision Gravity fiziksel çekim değil, stake × belirsizlik ile context arasındaki ilişkidir.
ENS'in kullandığı yasanın kendisi değil ihlalidir: riskli, belirsiz ve az-düşünülmüş kararı bulup
dikkati oraya yöneltmek (P5) — Howard'ın Value of Information'ının sürekli örgütsel biçimi.*
