---
id: ENS-3021
title: Decision Entropy
type: law
canon: false
constitutive: false
maturity: M3
origin: ENS-0000 §III (P1, P5), ENS-3000 (LAW-ENTROPY), ENS-2001, ENS-2002, ENS-2003
depends_on: [ENS-0000, ENS-2001, ENS-2002, ENS-2003, ENS-2004, ENS-3000, ENS-4000]
referenced_by: []
principles: [P1, P5, P3]
status: ratified
owner: ens-philosopher
version: 0.2.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-012
evidence: {sci: E3, eng: E1, ops: E0, econ: E0}
---

# Decision Entropy

> LAW-ENTROPY'nin biçimsel, **ölçülebilir** hâli ve ilk nicel yasa; R1'in sınavı. `canon:
> false` — skeptic'ten sağ çıkınca Külliyat'a girer.
>
> **v0.2 notu:** [SKR-011](reviews/SKR-011-decision-entropy.md)'e yanıt. (1) Kahneman delta'sı
> daraltıldı; level/pattern noise **bilgi-teorik olarak** kredilendi, (2) exploration-noise
> ayrımı **commitment intent-etiketi** ile çözüldü, (3) kestirim uyarısı eklendi. §Yanıt
> tablosu sonda.

## Definition

**Decision Entropy, aynı Purpose-tipinde ve benzer Context'te alınan _exploit-niyetli_
kararların seçimlerindeki, Context farkıyla açıklanamayan tutarsızlıktır** — seçilen
Alternative `A`'nın, Context kümesi `C` verildiğindeki **Shannon koşullu entropisi**:
```
DecisionEntropy = H(A | C) = − Σ p(c) Σ p(a|c) log p(a|c)   (yalnızca intent=exploit kararlar)
```
Metafor değildir; literal bilgi-teorik niceliktir. ENS "entropy"yi termodinamikten değil,
Shannon'dan alır (Shannon 1948).

## Motivation

LAW-ENTROPY ölçülebilir olmazsa slogandır. `H(A|C)`, tutarsızlığı ölçülebilir kılar ve
yasayı yanlışlanabilir öngörüye çevirir: *karar-verici sayısı arttıkça, tutarlılık kuvveti
(memory) yoksa `H(A|C)` artar.*

## Historical context — ve konumlandırma

| Öncül | Ne verdi | ENS ile örtüşme | ENS'in (dar) delta'sı |
|-------|----------|-----------------|------------------------|
| **Shannon (1948)** | `H` = koşullu entropi | **Ölçünün kendisi** | `H`'yi karar seçimlerinin context-koşullu dağılımına uygulama |
| **Kahneman-Sibony-Sunstein, *Noise* (2021)** | system noise; **level/pattern noise**; decision hygiene | **Kavramın kalbi VE azaltma fikri** | Yalnızca: `H(A|C)` **formalizasyonu** + sürekli örgüt-düzeyi ölçüm. Azaltma (memory) *örtüşür* (decision hygiene) |
| **Inter-rater reliability** (Cohen κ, Krippendorff α) | Değerlendiriciler arası uyum | Aynı-context-aynı-seçim | Memory Graph üzerinde sürekli, context-koşullu, zaman-serisi |
| **Six Sigma** | Süreç çıktı varyansı | Değişkenlik azaltma | Çıktı değil *seçim* varyansı, context'e koşullu |
| **Termodinamik entropi** | Düzensizlik metaforu | — | **Kullanılmaz** |

**Dürüst delta:** Decision Entropy'nin kavramı (aynı durumda farklı karar = kötü tutarsızlık)
Kahneman *Noise*'un ta kendisidir; azaltma mekanizması (paylaşılan çerçeve/memory) de decision
hygiene ile örtüşür. ENS'in **tek** dar katkısı: bu gürültüyü `H(A|C)` olarak formelleştirip
ENS'in Context/Memory altyapısı üzerinde **sürekli ve otomatik ölçmek** ve bir büyüme yasasına
bağlamak.

## Theoretical model

### 1. Ölçü ve Kahneman ayrıştırmasına eşleme (SKR-011 Bulgu 1)
Bir Purpose-tipi içinde, kararları Context-benzerliğine göre (ENS-2002) kümele; `H(A|C)`'yi
hesapla. Kahneman'ın level/pattern ayrımı bilgi-teorik olarak **doğal biçimde** çıkar:

- **Level noise** = `I(A ; Owner | C)` — Owner kimliğinin, context sabitken seçime kattığı
  bilgi. Yüksekse: *kim karar veriyor* seçimi değiştiriyor (Owner'lar arası tutarlı fark).
- **Pattern noise** = `H(A | C, Owner)` — Owner ve context bilindikten sonra kalan tutarsızlık
  (aynı Owner'ın aynı durumda tutarsızlığı).
- Zincir kuralı: `H(A|C) = I(A;Owner|C) + H(A|C,Owner)` — yani toplam Decision Entropy, level
  ve pattern bileşenlerine **tam ayrışır.** ENS bu ölçüyü icat etmez; Kahneman'ın ayrımına
  bilgi-teorik biçim verir.

### 2. Haklı vs artık entropi
Context gerçekten farklıysa farklı seçim haklıdır; koşullama (`|C`) bunu dışlar. Geriye kalan
`H(A|C)` artık (residual) tutarsızlıktır — doğruluğu Context relevance'a bağlıdır (§failure).

### 3. Exploration vs noise — commitment intent-etiketi (SKR-011 Bulgu 2)
Tüm değişkenlik kötü değildir: kasıtlı keşif (March 1991) sağlıklıdır. Ayrım için karar,
commitment anında (ENS-2001, event-sourced) bir **intent** taşır: `exploit | explore`.
- **Decision Entropy** (istenmeyen noise) yalnızca `intent=exploit` kararlar üzerinden ölçülür.
- `intent=explore` kararların yarattığı varyans **exploration entropisi** olarak ayrı izlenir;
  istenmeyen değildir, korunur.
Etiket commitment'ta (sonuçtan önce) konduğundan, kötü sonucu "keşifti" diye post-hoc
etiketlemek mümkün değildir (event-sourcing). Böylece ayrım niyet temelli, sonuçtan bağımsız.
*(Not: bu, Decision Object'e bir `intent` alanı ister — **ENS-2001 v0.3'te OE1 olarak eklendi**
(§Model 2/3, faz = Commitment; bağımsız skeptic turu bekliyor). OL1 (ExpectedValue) ile aynı sınıf.)*

### 4. Büyüme yasası ve memory azaltıcı kuvvet
Owner çeşitliliği ve decision-surface büyüdükçe, ortak prior yoksa `H(A|C)` (özellikle level
noise `I(A;Owner|C)`) **artar** — yanlışlanabilir öngörü. Company Memory (ENS-2003), tüm
Owner'lara aynı geçmiş kararları/öğrenimleri getirerek ortak prior kurar → level noise düşer.
(Bu azaltma, Kahneman'ın decision-hygiene'inin bir örneğidir; ENS'in katkısı otomatikliği.)

### 5. Kestirim metodolojisi (SKR-011 Bulgu 3)
`H(A|C)` sonlu karar örnekleminden kestirilir; naif tahmin **aşağı-yanlıdır** (entropy
underestimation). Miller-Madow ya da benzeri bir düzeltme uygulanır; sürekli/çok-değerli
seçimler binning gerektirir ve `H` binning'e duyarlıdır — bin şeması raporlanır. Düşük-hacimli
Purpose-tiplerinde `H` istatistiksel olarak zayıftır; güven aralığıyla raporlanır.

## Implications
- **Enterprise IQ ile ters ilişki:** yüksek artık `H(A|C)` = düşük tutarlılık.
- **Sağlık metriği + teşhis:** level vs pattern ayrışması *nerede* müdahale gerektiğini söyler
  (level yüksek → standart/memory; pattern yüksek → tek Owner'ın süreç sorunu).
- **Nicel katman kanıtı (R1):** hesaplanabilir olduğu için kavramlar operasyonel.

## Relationships
- **→ Decision (ENS-2001):** `A` = seçilen Alternative; `intent` alanı buraya eklenir.
- **→ Context (ENS-2002):** `C` = koşullama; relevance'a dayanır.
- **→ Company Memory (ENS-2003):** azaltıcı kuvvet + ölçüm zemini.
- **→ Learning (ENS-2004):** entropi dağınıklığı ölçer, kaliteyi değil; birlikte okunur.
- **→ LAW-ENTROPY:** bu belge onun operasyonel biçimidir.

## Examples
**50 şube, indirim kararı:** aynı context'te seçimler {%10,%20,%0,%30} → yüksek `H(A|C)`.
Ayrıştırma: eğer belirli şubeler *hep* yüksek indirim veriyorsa level noise (Owner farkı);
aynı şube *aynı durumda* dağınıksa pattern noise. Bir pilot şube kasıtlı fiyat deneyi yapıyorsa
(`intent=explore`), onun varyansı Decision Entropy'ye sayılmaz.

## Laws
**LAW-ENTROPY**'yi operasyonelleştirir: *artık (exploit-niyetli) `H(A|C)` büyümeyle artar,
memory ile azalır; hedef sıfır değil, istenmeyen noise'un azaltılması, exploration'ın korunması.*

## Failure conditions (Anayasa Madde X)
- **Intent-etiketi oyunlanması.** Etiket commitment'ta konsa da, bir Owner rutin kararları
  sistematik olarak `explore` etiketleyip noise ölçümünden kaçabilir. Etiketleme oranı
  izlenmeli; anormal explore oranı bir denetim sinyali.
- **Koşullama relevance'a bağlı.** `H(A|C)` doğruluğu `C` kümelemesinin doğruluğuna, yani
  Context relevance'ın kör noktalarına (ENS-2002 borçları) bağlı; yanlış kümeleme haklı
  varyansı artık gibi gösterir.
- **Kestirim + hacim.** Sonlu-örneklem bias ve binning; seyrek Purpose-tiplerinde zayıf.
- **Seçim ≠ kalite.** `H=0` ama herkes aynı *yanlışı* yapıyorsa entropi düşük, karar kötü;
  Learning (ENS-2004) ile birlikte okunmalı.

## SKR-011'e yanıt
| Talep | Karşılandığı yer |
|-------|------------------|
| 1. Kahneman delta daralt + level/pattern kredile | §Historical, §Model 1 (I(A;Owner|C) / H(A|C,Owner)) |
| 2. Exploration-noise ölçütü | §Model 3 (commitment intent-etiketi) |
| 3. Kestirim uyarısı | §Model 5 |

---

*Decision Entropy, Kahneman'ın "noise"unun Shannon ölçüsüdür: `H(A|C) = I(A;Owner|C) +
H(A|C,Owner)` — level artı pattern. Yasa entropiyi sıfırlamaz; istenmeyen noise'u azaltır,
etiketlenmiş keşfi korur.*
