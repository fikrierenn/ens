---
id: SKR-010
type: skeptic-review
origin: ENS-2004
depends_on: [ENS-2004, SKR-009]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-010 — ENS Learning Theory v0.2 İkinci Saldırısı

## Verdict
**survives** (üç ileri-taşıma yükümlülüğüyle). v0.2, SKR-009'un üç talebini de karşılıyor.
Karar kalitesinin üç bileşene ayrılması — sonuç öğrenimi, **outcome-bağımsız seçim
rasyonalitesi**, toplam kalibrasyon — ve hindsight'ın **donmuş commitment snapshot**'ıyla
yapısal engellenmesi, Baron-Hershey outcome bias'ına karşı gerçek bir savunma kuruyor. Kalan
noktalar mevcut kusur değil, biri ENS-2001'e additive bir alan, biri objektif-fonksiyon
genellemesi. **Kavram Külliyat'a girebilir (canon:true).**

## Talep-talep doğrulama
1. **Karar kalitesi ayrımı — KARŞILANDI (güçlü).** §5, (ii) seçim rasyonalitesini
   outcome-bağımsız ve karar-başına, (iii) kalibrasyonu toplam olarak ayırıyor. "İyi kalibre
   ama kötü seçen" kararı artık (ii) yakalıyor — SKR-009'un tam istediği.
2. **Hindsight koruması — KARŞILANDI (zarif).** Süreç değerlendirmesi event-sourced donmuş
   snapshot üzerinden, sonuç-kör. Event-sourcing'in (ENS-2001) bu teorik faydası doğru
   kullanılmış; değişmez snapshot, değerlendiriciyi hindsight'tan yapısal olarak alıkoyar.
3. **Prior art — KARŞILANDI.** Merdiven nedensel-kanıt hiyerarşisine (evidence-based
   medicine), outcome bias Baron & Hershey (1988)'e kredilendi; Duke popülerleştirici olarak
   doğru konumlandı.

## Yeni saldırı yüzeyi

- **Seçim rasyonalitesi EV-maksimizasyonu varsayıyor (en güçlü itiraz).** §5(ii), "en iyi
  beklenen-değerli Alternative seçildi mi" diyor. Ama **firmalar maksimize etmez, satisfice
  eder** (Cyert-March; Simon'ın bounded rationality'si — ENS'in kendi soyağacı, ENS-1000 §I).
  Purpose'un gerçek objektif fonksiyonu risk-averse, çok-kriterli ya da satisficing olabilir.
  Naif EV-max'a göre "irrasyonel" görünen bir seçim, gerçek objektife göre rasyonel olabilir.
  Bu çürütme değil; seçim rasyonalitesi **Purpose'un gerçek objektifine göre** tanımlanmalı.

- **Decision Object, Alternative-başına beklenen değeri saklamıyor.** §5(ii) bunu gerektiriyor
  ama ENS-2001 anatomisinde yalnızca (seçilen için) tekil `Expected Outcome` var. Bu, ENS-2001'e
  additive bir alan (`ExpectedValue` per Alternative) gerektirir — küçük ama gerçek bir
  cross-concept bağımlılık.

## Kalan riskler (ileri-taşıma yükümlülükleri)
- **OL1 — ENS-2001 additive alan.** Decision Object, Alternative-başına beklenen değer/utility
  saklamalı; yoksa §5(ii) ölçülemez. ENS-2001 v0.3'te (ya da bir ADR'de) eklenmeli.
- **OL2 — Seçim rasyonalitesini objektife görele tanımla.** Naif EV-max değil, Purpose'un
  gerçek objektif fonksiyonu (satisficing/çok-kriterli/risk-tutumu). Bounded rationality ile
  tutarlı olmalı.
- **OL3 — Kalibrasyon hacmi.** (iii) çok karar ister; düşük-hacimli Purpose-tiplerinde zayıf.

## İç tutarlılık
Attribution merdiveni, üç-bileşenli karar kalitesi ve donmuş snapshot birbirini tutuyor.
L1'in Context relevance (OC3) ve Memory retention'ı beslemesi tutarlı. Çelişki yok.

## Sonuç
Learning, biriken en büyük borcu (R2/OC3/OM2) taşınabilir kılıyor: attribution artık
seviye-etiketli, karar kalitesi sonuçtan ayrık, hindsight yapısal engelli. `ratified`
edilebilir. Bununla birlikte **ENS'in bilişsel çekirdek döngüsü tamamlanır: Decision →
Context → Memory → Learning, dördü de Külliyat'ta.** OL1-OL3 nicel katmanda (Decision Entropy)
yeniden gündeme gelecek.

## Kaynaklar
- Baron & Hershey (1988); Rubin; Pearl; Argyris & Schön; Deming; Sutton; Cyert & March;
  Simon (satisficing/bounded rationality) — önceki SKR'ler ve ENS-1000'de konumlanmış.
