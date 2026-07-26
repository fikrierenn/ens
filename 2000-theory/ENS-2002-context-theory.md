---
id: ENS-2002
title: Context Theory (ENS)
type: theory
canon: false
constitutive: false
maturity: M3
origin: ENS-0000 §III (P2, P5), ENS-2001
depends_on: [ENS-0000, ENS-1000, ENS-2001, ENS-4000]
referenced_by: [ENS-4010]
principles: [P2, P5, P3]
status: ratified
owner: ens-philosopher
version: 0.2.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-006
evidence: {sci: E3, eng: E1, ops: E0, econ: E0}
---

# ENS Context Theory

> Decision Object'in en yük taşıyan alanının (Context, P2) teorisidir. Decision Theory
> (ENS-2001) atomu tanımladı; Context Theory o atomun *kalitesini* belirleyen boyutu
> tanımlar. `canon: false` — skeptic'ten sağ çıkınca Külliyat'a girer.
>
> **v0.2 notu:** [SKR-005](reviews/SKR-005-context-theory.md)'e yanıt. Üç talep: (1) ters-U
> Eppler-Mengis/Yerkes-Dodson'a kredilendi, (2) Dey-Abowd delta'sı daraltıldı, (3) en
> önemlisi — ilgililik-döngüselliği **Company Memory-temelli kestirimle** kırıldı. Bu
> revizyon, Context'in Company Memory'ye (P3) **yapısal bağımlılığını** ortaya çıkarır ve
> ENS-2003'ü (Company Memory) sıradaki zorunlu kavram yapar.

## Definition

**Context, bir karara bağlı, o kararı anlamlı kılan _ilgili_ çevresel durumdur** (P2).
Context bir *nesne* değil bir **ilişkidir** — her zaman *bir karara göre* ilgilidir. Mutlak
context yoktur. Data ham sinyaldir; **context = data + ilgililik + ilişki.** Aynı veri, bir
karar için context, başkası için gürültüdür.

## Motivation — neden context, data'dan üstün?

P2: context olmadan data anlamsızdır. Bir sayının (stok = 400) bir karar için anlamı ancak
context'le belirir. Veri hacmi arttıkça karar kalitesi kendiliğinden artmaz — ilgili context
arttıkça artar, ilgisiz veri arttıkça azalır. ENS'in yatırımı hacme değil **ilgililiğe**dir.

## Historical context — ve konumlandırma

"Context" birçok alanda tanımlı; ENS dürüstçe ve **dar** konumlanmalı (Anayasa Madde VI):

| Öncül | Ne verdi | ENS ile örtüşme | ENS'in (dar) delta'sı |
|-------|----------|-----------------|------------------------|
| **Dey & Abowd (2001)** | Context tanımı; context-awareness'ta *görev-göreli* ilgililik | Karara/göreve-göreli ilgililik **onlarda da var** | Sadece ölçülebilir **Density/Score + staleness + memory-temelli relevance** |
| **Eppler & Mengis (2004); Yerkes-Dodson** | Bilgi yükü ↔ performans **ters-U**'su | §Model 3'ün ters-U'su **onların** | Yükü ham hacim değil **ilgililik** cinsinden + **attention bütçesi** + operasyonelleştirme |
| **Sperber & Wilson** | İlgililik = etki / çaba | İlgililik-çaba ödünleşimi | Attention bütçesine (P5) ve Context Score'a gömme |
| **Bateson / Shannon** | "Fark yaratan fark"; context'siz sinyal yorumlanamaz | Data ≠ anlam | İlgililiği *ölçülebilir* kılma |

**Dürüst delta:** ENS Context'in "karara-göreli ilgililik" fikri özgün değildir (Dey-Abowd,
Sperber-Wilson). Ters-U da özgün değildir (Eppler-Mengis). ENS'in dar ama gerçek katkısı:
ilgililiği **Company Memory'den kestirip** Density/Score olarak *hesaplanabilir* kılmak ve
staleness eklemek.

## Theoretical model

### 1. Context bir ilişkidir
Karar `d`'nin Purpose'u `p` için:
```
Context(d) = { s ∈ EnterpriseState : relevance(s, p) ≥ θ }
```
İlgililik `relevance(s, p)`, `s`'in `p`'ye dair belirsizliği azaltma derecesidir. Eşik `θ`
attention bütçesinden gelir (§3).

### 2. İlgililik nasıl hesaplanır — döngüyü Memory ile kırmak (SKR-005 Bulgu 3)
SKR-005 haklı: `relevance(s, p)`'yi şimdiki karardan türetmek döngüseldir (kararı bilmek
için ilgililik, ilgililik için karar gerekir). ENS döngüyü **şimdiki karardan değil,
geçmişten** kırar:

> `relevance(s, p)` ≈ benzer Purpose-tipi `p`'ye sahip, geçmiş **commit-edilmiş** kararların
> (ENS-2001) fiilen bağladığı ve **sonucu iyileştirdiği** context öğelerinin ağırlığı.

Yani ilgililik, Company Memory'deki (P3) karar-sonuç verisinden kestirilir: "bu tür kararlar
tarihsel olarak hangi context'i kullandı ve o context Expected/Actual farkını iyileştirdi
mi?" Bu kestirim dairesel değildir çünkü **girdi şimdiki karar değil, tarihtir.**

- **Warm-start:** benzer Purpose-tipinde yeterli geçmiş varsa, relevance ağırlıkları
  memory'den gelir; Context Score hesaplanabilir.
- **Cold-start:** yeni Purpose-tipi (memory yok) → relevance yalnızca zayıf şema önseli
  verir → Context Score düşük → Confidence düşük. Bu bir kusur değil, **doğru davranıştır**:
  bilinmeyen alanda düşük güven (P6).

Böylece Context, Company Memory olmadan hesaplanamaz — bu, teorinin dayattığı bir bağımlılık
sırasıdır (ENS-2003 sıradaki zorunlu kavram).

### 3. İlgililik–attention ödünleşimi (ters-U) — Eppler-Mengis'e dayanır
Karar kalitesinin context yükünde ters-U izlediği **Eppler & Mengis (2004)** (Yerkes-Dodson
kökenli) sonucudur; ENS onu icat etmez, **ilgililik cinsinden yeniden ifade eder:**

- **Az context (gap):** ilgili durum eksik → kalite düşük.
- **Optimal:** ilgililik/attention oranı en yüksek.
- **Fazla context (noise):** ilgisiz veri hem attention tüketir (P5) hem sinyali gömer →
  kalite düşer.

```
quality(d) = g( ContextScore(d) ),   ContextScore = coverage − noise_penalty − staleness
```
ENS'in Eppler-Mengis'e kattığı: yükü **ham hacim değil ilgililik** ölçer (memory-temelli,
§2) ve bir **attention bütçesine** bağlar. LAW-CONTEXT böylece "context azaldıkça düşer"den
**"Context Score optimalden uzaklaştıkça düşer"**e (iki yönlü, yanlışlanabilir) geçer.

### 4. Context zamansaldır (staleness)
Her context öğesinin bir değişim hızı vardır; hızlı değişen çabuk bayatlar. Bayat context
yokluğu kadar tehlikelidir (LAW-ORG-MEMORY). Context Score bir tazelik boyutu taşır.

## Implications
- **Context Score, Confidence'ı kapılar** (P6): düşük Score → düşük Confidence ya da ertele.
- **Evidence, context'in commit-edilmiş alt kümesidir** (P6 explainability).
- **Context ↔ Memory döngüsü:** Context, Memory'den kestirilir; her yeni karar-sonuç,
  Memory'yi güncelleyerek gelecekteki relevance kestirimini iyileştirir (P4 learning ile
  kapanır).

## Relationships
- **→ Decision Theory (ENS-2001):** Context, Decision Object alanı; Contextualization
  fazında toplanır; benzerlik commit-edilmiş kararlar üzerinden tanımlı.
- **→ Company Memory (ENS-2003, gelecek):** relevance kestiriminin kaynağı; **yapısal
  bağımlılık**.
- **→ LAW-CONTEXT:** §3'te iki yönlü/ölçülebilir.
- **→ Attention (P5):** `θ` ve noise_penalty attention bütçesinden.

## Examples
**Fiyatlandırma:** memory, geçmiş fiyat kararlarında {maliyet, rakip fiyatı, elastikiyet}'in
sonucu iyileştirdiğini, {ofis kirası}'nın etkisiz olduğunu gösterir → relevance ağırlıkları
buradan gelir; İK verisini bağlamak noise_penalty'yi artırır.

**Cold-start:** ilk kez verilen bir tür karar (ör. yeni pazar girişi) → memory yok →
relevance zayıf → Context Score düşük → Confidence düşük (doğru davranış).

## Laws
LAW-CONTEXT'i keskinleştirir (§3), LAW-ORG-MEMORY'nin staleness uyarısına zemin verir. Yeni
context yasası ancak Context Score üzerinden ifade edilebiliyorsa geçerlidir.

## Failure conditions (Anayasa Madde X)
- **Relevance bias inheritance (yeni, en ciddi).** Memory-temelli relevance, geçmiş kararlar
  yanlış context bağladıysa o yanlışı miras alır ve sürdürür (LAW-ORG-MEMORY: bayat/yanlış
  memory yanıltır). Kestirim, geçmişin körlüğünü tekrar edebilir; bir düzeltme mekanizması
  (ör. keşifsel context örneklemesi) gerekir.
- **Cold-start kapsamı.** Yeni Purpose-tiplerinde relevance zayıftır; ENS burada yüksek
  Confidence iddia etmez ama bu, memory birikene dek bir kör nokta bırakır.
- **Parametre bağlanması.** `θ`, noise_penalty, staleness ağırlıkları bir ölçüm yordamına
  bağlanmazsa ters-U yanlışlanamaz hale gelir; her biri operasyonel tanım gerektirir.

## SKR-005'e yanıt
| Talep | Karşılandığı yer |
|-------|------------------|
| 1. Ters-U'yu Eppler-Mengis/Yerkes-Dodson'a kredile | §Historical tablo, §Model 3 |
| 2. Dey-Abowd delta'sını daralt | §Historical tablo ("dar delta") |
| 3. İlgililik-döngüselliğini kır | §Model 2 (memory-temelli kestirim) |

---

*Context bir ilişkidir: her zaman bir karara göre ilgilidir. Ve ilgililik, şimdiki karardan
değil, organizasyonun belleğinden — geçmiş kararların neyi işe yarar bulduğundan — kestirilir.*
