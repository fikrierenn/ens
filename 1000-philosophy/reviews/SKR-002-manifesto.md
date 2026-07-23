---
id: SKR-002
type: skeptic-review
origin: ENS-1000
depends_on: [ENS-1000, SKR-001]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-002 — ENS Manifestosu v0.2 İkinci Saldırısı

## Verdict
**survives** (üç ileri-taşıma yükümlülüğüyle). Manifesto v0.2, SKR-001'in beş talebini de
karşılıyor. En ağır iki itiraz — özgünlük ve attributability — dürüstçe ve doğru yönde
çözülmüş. Kalan zayıflıklar artık *mevcut kusur* değil, Faz 1-2'ye devreden *vaatlerdir*;
bir Faz 0 belgesi için kapıyı kapatmazlar. **Faz 0 kapısı açılır** (Anayasa Madde VII, X).

## Talep-talep doğrulama

1. **Yenilik konumu — KARŞILANDI.** §I sezginin eski olduğunu açıkça kabul ediyor; §III
   tablosu Simon/Cyert-March, Beer VSM, Walsh-Ungson ve Gartner DI ile örtüşmeyi ve
   delta'yı tek tek veriyor. "Yeni kavrayış" dili kalkmış. Bu, v0.1'in ölümcül kusurunun
   dürüst onarımıdır.
2. **Disiplin kalibrasyonu — KARŞILANDI.** §II ve §IV, ENS'i "yeni bilim" yerine
   "mühendislik disiplini + standart" olarak konumluyor (TCP/IP, DDD analojisi geçerli).
   Fazla-iddia riski ortadan kalktı.
3. **Metaforların operasyonelleştirilmesi — KISMEN; ileri-taşındı.** §VI her üç yapıya
   ilk-kesim provisional metrik veriyor ve ölçülemezlerse "law" olmayacaklarını taahhüt
   ediyor. Bir manifesto için yeterli; ama metrikler henüz kaba (bkz. Kalan risk R1).
4. **Attributability — KARŞILANDI (güçlü).** §VII, learning'i yalnızca atfedilebilir karar
   sınıflarına daraltıyor, counterfactual temelli atıf ve açık sınır koyuyor. Bu, bir
   zayıflığı dürüst bir kapsam sınırına çevirmenin doğru yoludur.
5. **P6 faithfulness — KARŞILANDI (mantıksal).** §VIII, P6'yı makullükten sadakate taşıyor
   ve sadakat gösterilemezse black-box saymayı taahhüt ediyor. v0.1'in iç çelişkisi
   çözüldü (bkz. Kalan risk R3).

## Yeni saldırı yüzeyi (v0.2'nin getirdikleri)

Dürüst skeptic, onarımın yeni açık yaratıp yaratmadığını sorar:

- **Aşırı-daralma riski.** §VII + §XI, ENS'i gerektiğinde "memory + explainability
  katmanına" küçülmeye hazır kılıyor. Bu entelektüel dürüstlüktür, ama bir sınır da
  çiziyor: eğer atfedilebilir karar sınıfı pratikte darsa, ENS'in *learning* iddiası —
  yani onu Beer/March'tan ayıran delta'nın bir parçası — marjinalleşir. Bu, çürütme değil;
  Faz 1'in ampirik olarak yanıtlaması gereken bir bahis.
- **Standart-önce riski.** ENS kendini "standart" olarak konumluyor ama teori henüz yok.
  Bu, fazlı yaklaşımla (önce Külliyat, sonra mimari) tutarlı; kabul edilir. Yine de
  "standart" dili, içi doldurulana dek bir *hedef* olarak okunmalı, bir *başarı* olarak
  değil.

## Kalan riskler (ileri-taşıma yükümlülükleri — kapı açık ama borç kayıtlı)

- **R1 — §VI metrikleri Faz 1'de sertleştirilmeli.** Decision Entropy/Gravity/Capital
  ölçümleri şu an savunulabilir taslak; `2000-theory`'de yanlışlanabilir, tanımlı
  konstrüktlere dönüşmezlerse orada `refuted` olurlar. Manifesto'yu bloke etmez, teoriyi
  bekler.
- **R2 — Counterfactual atıf bir nedensellik taahhüdü gerektirir.** §VII "counterfactual"
  diyor ama nedensel modelin nasıl kurulacağı Faz 1-2'ye ait açık bir borçtur. Söz verildi,
  ödenmedi.
- **R3 — Sadakat ölçülebilirliği açık bir araştırma iddiasıdır.** §VIII, LLM açıklamalarının
  sadakatinin ölçülebildiğini varsayıyor. Bu, mantıksal tutarlılığı sağlar ama ampirik
  olarak Faz 4'te doğrulanmalı; doğrulanamazsa P6 yeniden zayıflar.

## İç tutarlılık
v0.1'in §IV↔§VII çelişkisi (açıklama üretilebilir vs post-hoc) §VIII ile giderilmiş. Başka
açık çelişki bulunamadı. Terminoloji sözlükle (ENS-4000) tutarlı.

## Sonuç
v0.2, bir Faz 0 felsefe belgesi olarak sağlamdır ve dürüsttür. Üç ileri-taşıma yükümlülüğü
(R1-R3) Faz 1-2'nin sınavıdır, Faz 0'ın engeli değil. Manifesto `ratified` edilebilir ve
Faz 0 kapısı açılır. Skeptic, R1-R3'ü Faz 1 kavramları yazılırken yeniden gündeme
getirecektir.

## Kaynaklar
SKR-001 ile aynı (Beer 1972; Cyert-March 1963; Walsh-Ungson 1991; Gartner DI). v0.2 için
yeni dış kaynak gerekmedi; değerlendirme metin içi tutarlılık ve talep-karşılama üzerineydi.
