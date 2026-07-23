---
id: SKR-011
type: skeptic-review
origin: ENS-3021
depends_on: [ENS-3021]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-011 — Decision Entropy (ENS-3021) Saldırısı

## Verdict
**wounded** — ama önemli bir kazanımla. İlk nicel yasa, R1 sınavını **geçti**: Decision
Entropy'yi Shannon koşullu entropisi `H(A|C)` olarak tanımlamak, "entropy bir metafor mu"
eleştirisini yapısal olarak defused ediyor — bu literal, hesaplanabilir bir bilgi-teorik
niceliktir, termodinamik benzetmesi değil. Yani çekirdek kavramlar operasyonelleştirilebilir;
metafor değil. Ancak iki gerçek açık var: Kahneman *Noise* karşısında fazla-iddia ve yasanın
ifadesinin dayandığı exploration-vs-noise ayrımının çözümsüzlüğü. Üç talep karşılanmadan
`canon:true` olamaz.

## Kazanım (önce dürüstlük)
Shannon grounding doğru ve güçlü. `H(A|C)`'nin koşullu oluşu, "haklı varyansı dışla, artık
tutarsızlığı ölç" ayrımını biçimsel yapıyor. §failure'daki "sıfır entropi ≠ optimal"
(exploration) nüansı olgun. Bu, nicel katmanın kanıtlanabilir olduğunun ilk somut gösterimi.

## Bulgu 1 — Kahneman *Noise* delta'sı fazla iddia (en ağır)

Belge, Decision Entropy'yi Kahneman-Sibony-Sunstein *Noise* (2021)'e proaktif konumluyor —
iyi. Ama iki nokta eksik/abartılı:
- **Azaltma mekanizması da örtüşüyor.** *Noise*, gürültüyü "decision hygiene", paylaşılan
  çerçeve/kılavuz ve agregasyon ile azaltmayı önerir. ENS'in "memory → ortak prior → düşük
  `H`" mekanizması, Kahneman'ın "paylaşılan çerçeve gürültüyü azaltır"ının bir örneğidir.
  Yani delta, belgenin ima ettiğinden dar: gerçek delta **`H(A|C)` formalizasyonu + sürekli
  örgüt-düzeyi ölçüm**dür, azaltma fikri değil.
- **Established ayrıştırmayı kullanmıyor.** *Noise*, sistem gürültüsünü **level noise**
  (Owner'lar arası tutarlı fark) ve **pattern noise** (vaka-içi tutarsızlık) olarak ayırır.
  ENS'in monolitik `H(A|C)`'si bunu Owner'a da koşullayarak (level = between-Owner, pattern
  = residual) yeniden üretebilir ve bu yerleşik ayrıştırmayı kredilemeli — hem daha zengin
  hem daha dürüst.

## Bulgu 2 — Exploration vs noise ayrımı çözümsüz, üstelik yasa buna dayanıyor

Belge, yasanın "istenmeyen entropiyi azalt, exploration entropisini koru" olması gerektiğini
doğru söylüyor ve ikisini ayırmanın kolay olmadığını dürüstçe itiraf ediyor. Ama bu ayrım
**yasanın ifadesinin ta kendisi** — çözülmezse, ölçülen `H(A|C)` "kötü noise mu, iyi keşif mi"
ayırt edemez ve yasa uygulanamaz. Bir ölçüt gerekir. **İpucu (çözüm değil):** kararlar
event-sourced (ENS-2001); bir karar commitment anında *kasıtlı exploration* olarak
etiketlenebilir (Purpose'ta "keşfet" niyeti). Exploration-etiketli kararların entropi katkısı
ayrı sayılır; etiketsiz divergence = istenmeyen noise. Böylece ayrım niyet-etiketiyle,
sonuçtan bağımsız yapılır. Bunu modele koy.

## Bulgu 3 — Entropi kestirimi metodolojisi (carry-forward)
`H(A|C)` sonlu karar örnekleminden kestirilir; entropi kestirimi sonlu örneklemde **yanlıdır**
(bias) ve düzeltme ister (ör. Miller-Madow). Ayrıca sürekli/çok-değerli seçimler binning
gerektirir ve binning `H`'yi etkiler. Belge bu metodolojik uyarıyı taşımalı; aksi hâlde
küçük Purpose-tiplerinde `H` yanıltır.

## Sahibine talepler (kapıyı geçmek için)
1. **Kahneman delta'sını daralt** (gerçek delta = `H(A|C)` formalizasyonu + sürekli ölçüm);
   azaltma mekanizmasının decision-hygiene ile örtüştüğünü kabul et; **level/pattern noise**
   ayrıştırmasını kredile ve `H`'yi Owner'a koşullayarak buna eşle.
2. **Exploration-vs-noise ölçütü ver** (commitment'ta niyet-etiketi ile), yoksa yasa
   uygulanamaz.
3. **Entropi kestirimi uyarısını ekle** (sonlu-örneklem bias, binning).

## İç tutarlılık
`H(A|C)`, Decision (A), Context (C kümeleme) ve Memory (azaltıcı kuvvet) ile tutarlı.
Learning'le birlikte okunması gerektiği (entropi ≠ kalite) doğru. Çelişki yok; sorun eksik
konumlandırma ve çözülmemiş exploration ayrımı.

## Kaynaklar
- **Kahneman, D., Sibony, O. & Sunstein, C. R. (2021). *Noise: A Flaw in Human Judgment.***
  — system noise; level/pattern noise; decision hygiene.
- **Shannon, C. E. (1948).** A Mathematical Theory of Communication — koşullu entropi `H(A|C)`.
- Cohen's kappa / Krippendorff's α (inter-rater reliability); March (1991, exploration).
