---
id: SKR-009
type: skeptic-review
origin: ENS-2004
depends_on: [ENS-2004]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-009 — ENS Learning Theory (ENS-2004) Saldırısı

## Verdict
**wounded.** Bu, biriken borcun (R2/OC3/OM2) taşıyıcı kavramı ve büyük ölçüde başarılı:
attribution merdiveni (L0-L3) dürüst ve operasyonel; L1 prediction-error'ın her zaman mevcut
oluşu OC3'ü ödüyor; double-loop, Context/Memory güncellemesini (OM2) bağlıyor; resulting
problemine değinmesi olgun. Ama en kritik bölüm — §Model 5 (karar kalitesi ≠ sonuç kalitesi)
— eksik: karar kalitesini yalnızca Confidence kalibrasyonuna indirgiyor, ki bu yetersiz. Bir
prior art düzeltmesi ve bir hindsight koruması da gerekiyor. Üç talep karşılanmadan
`canon:true` olamaz.

## Güçlü yönler
- **Attribution merdiveni gerçek katkı** — sahte kesinlik yerine güven-etiketli seviye. L1'in
  her zaman mevcut oluşu ("Expected saklanmış counterfactual") zarif ve OC3'ü çözüyor.
- **Double-loop → relevance/retention güncellemesi** (OM2) doğru bağlanmış.
- Prior art (Argyris-Schön, PDCA, Rubin/Pearl, RL) dürüstçe konumlanmış.

## Bulgu 1 — §5 karar kalitesini kalibrasyona indirgiyor (wounded sürücüsü)

§Model 5, "süreç öğrenimini" **Confidence kalibrasyonuna** eşitliyor: "0.7 dediklerimin
%70'i tuttu mu?" Ama kalibrasyon, karar kalitesinin **yalnızca yarısıdır.** İyi kalibre bir
öngörücü hâlâ *kötü seçim* yapabilir: olasılıkları doğru tahmin edip yine de düşük-beklenen-
değerli Alternative'i seçmek. Karar kalitesi = **(a) kalibrasyon** + **(b) inançlara-göre
seçim rasyonalitesi** (kendi Confidence'ına göre en iyi Alternative seçildi mi?). §5 (b)'yi
atlıyor. Bu ayrım olmadan ENS, iyi tahmin eden ama kötü seçen bir kararı "iyi süreç" sayar.

## Bulgu 2 — Süreç değerlendirmesi hindsight bias'a açık

§5, "karar, o an bilinebilir olana göre yeterli miydi?" diye soruyor — ama bunu *sonucu
gördükten sonra* değerlendirmek hindsight bias üretir (sonucu bilen değerlendirici, süreci
sonuca göre yeniden yorumlar). Bir koruma gerekir. **İpucu (çözüm değil):** ENS-2001 kararı
event-sourced'tur; commitment anındaki **Context snapshot'ı donmuş olarak kayıtlıdır.** Süreç
değerlendirmesi bu donmuş snapshot üzerinden yapılırsa (sonuç bilgisi olmadan), hindsight
büyük ölçüde engellenir. Bunu modele koy.

## Bulgu 3 — Prior art: merdiven ve outcome bias

- **Attribution merdiveni (L0-L3), nedensel-kanıt hiyerarşisini** (RCT > quasi-experiment >
  observational; evidence-based medicine kanıt seviyeleri) yansıtıyor. Bu özgün değil;
  kredilendir ve ENS'in katkısını "bu hiyerarşiyi karar atomuna ve memory'ye bağlama" olarak
  daralt.
- **Resulting/outcome bias'ın akademik kökü Duke değil**, **Baron & Hershey (1988), "Outcome
  Bias in Decision Evaluation"** (defalarca replike edilmiş). Birincil kaynağı ver.

## Sahibine talepler (kapıyı geçmek için)
1. **Karar kalitesini ikiye ayır:** (a) Confidence kalibrasyonu + (b) inançlara-göre seçim
   rasyonalitesi. §5'i buna göre genişlet.
2. **Hindsight koruması ekle:** süreç değerlendirmesi, commitment anındaki donmuş Context
   snapshot'ı üzerinden (sonuç bilgisi olmadan) yapılır.
3. **Prior art'ı düzelt:** merdiveni nedensel-kanıt hiyerarşisine kredile; outcome bias için
   Baron & Hershey (1988)'i birincil kaynak ver.

## İç tutarlılık
Attribution merdiveni, L1'in Context relevance'ı (OC3) ve Memory retention'ı (∝|Learning| ×
confidence) beslemesiyle tutarlı. Single/double-loop, ENS-2002/2003 ile doğru bağlı. Sorun
§5'in eksikliği ve iki prior art düzeltmesi.

## Kaynaklar
- **Baron, J. & Hershey, J. C. (1988). Outcome Bias in Decision Evaluation.** *Journal of
  Personality and Social Psychology*, 54(4). — resulting/outcome bias birincil kaynağı.
- Rubin (potential outcomes); Pearl (counterfactual); Argyris & Schön; Deming/PDCA;
  Sutton (RL credit assignment) — belgede konumlanmış. Evidence-based medicine kanıt hiyerarşisi.
