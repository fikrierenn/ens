---
id: SKR-033
type: skeptic-review
origin: ENS-2001
depends_on: [ENS-2001, ENS-2004, ENS-3021, ENS-3022, ENS-4000]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
validation_dimension: scientific
---

# SKR-033 — Decision Theory v0.3 (OL1/OE1) Additive Alanları Saldırısı

**Bağımsızlık beyanı:** Bu inceleme, ENS-2001'i v0.3'e taşıyan `ens-philosopher` çağrısından
ayrı, taze context'te yapıldı (G2/G3 — yazar kendi eklemesini onaylayamaz). Yalnızca v0.3'ün
additive iddiaları (`ExpectedValue` per-Alternative = OL1; `intent` = OE1) sınandı; SKR-004'te
ratified edilen v0.2 çekirdeği yeniden yargılanmadı.

## Verdict
`survives` — İki alan da **gerçekten additive**: individuation'ın atom-sayısını (commitment
kümesi) bozmadıkları bağımsız olarak doğrulandı, faz-yerleşimleri mevcut §Lifecycle'a yeni bir
gizli faz icat etmeden oturuyor, yenilik iddiaları (klasik DT'nin EV'si, March'ın
exploration/exploitation'ı) dürüstçe ve abartısız atfedilmiş, en sert problemler (EV
elicitasyonu, ölçek, intent oyunlanması, ikili etiket) *gizlenmemiş*, failure condition olarak
açıkça taşınmış (Madde X'in tam istediği). Kapıyı geçer; ekteki dört talep **bloke etmeyen
keskinleştirmelerdir**, statüyü düşürmez.

## Yenilik incelemesi
İki alan da yeni bir *kavram* iddia etmiyor; var olan iki yerleşik fikri karar-atomuna
**alan** olarak bağlıyor. Bu, doğru duruş — ve doküman bunu abartmıyor.

- **ExpectedValue (OL1).** Klasik karar analizinin olasılık-ağırlıklı beklenen değeri (von
  Neumann–Morgenstern; Savage). Bağımsız doğrulama: EV, standart decision-analysis'te
  "her alternatifin olası sonuçlarının olasılıkla ağırlıklandırılmış ortalaması"dır ve
  EV-maksimize eden alternatif seçilir ([SpringerLink, Decision Analysis — Expected Value
  Maximization](https://link.springer.com/chapter/10.1007/978-3-031-59353-6_7)). ENS'in
  §Prior-art tablosu bunu doğru disclaim ediyor: "ENS EV'yi *hesaplamaz*, karşılaştırılabilir
  tahmin olarak *saklar*." Uydurma atıf yok. ENS'in delta'sı gerçekten mütevazı ve öyle
  sunuluyor: EV'yi **donmuş, per-Alternative, event-sourced** bir kayıt yapıp sonuçtan bağımsız
  seçim-rasyonalitesine (ENS-2004 §5ii) ve stake'e (ENS-3022) besleme. Karar ağaçlarının
  dal-başına EV saklaması yeni değil; ENS'in tek eklediği "donmuş + outcome-independent
  skorlama için" çerçevesi — dürüstçe küçük.
- **intent (OE1).** March (1991) exploration/exploitation. Bağımsız doğrulama: kavram ve
  aralarındaki *yönetilmesi gereken gerilim* doğru ([March 1991, NTNU
  kopyası](http://www.iot.ntnu.no/innovation/norsi-pims-courses/Levinthal/March%20(1991).pdf)).
  ENS'in delta'sı — bunu *sonuçtan bağımsız, commitment-anı bir karar alanı* yapmak — March'ta
  yok; meşru. **Not (aşağıda talep 2'yi besler):** güncel literatür exploration/exploitation'ın
  *ikili/ayrılabilir* olup olmadığını açıkça tartışıyor ([Zhou vd., "Are March's exploration and
  exploitation separable?"](https://gala.gre.ac.uk/id/eprint/41827/3/41827_ZHOU_Are_James_Marchs_exploration_and_exploitation%E2%80%99_separable.pdf)).
  ENS-2001 §Failure bu ikili-sınır sorununu **kendisi işaretlemiş** ("bir karar kısmen işletme
  kısmen keşif olabilir — ikili etiket bu karışımı kaybeder") — yani literatürdeki gerçek
  tartışmayı görmezden gelmemiş.

**Sonuç:** Yenilik cephesinde overreach yok; atıflar gerçek, bulunabilir ve doğru kullanılmış.

## Yanlışlanabilirlik
İki alan da yanlışlanabilir öngörüler doğuruyor, dolayısıyla inanç değil bilim:
- **ExpectedValue:** eğer Faz-4'te seçilmeyen alternatifler için EV elicitasyonu güvenilir
  kaydedilemezse (ya da kabaysa), `Stake=spread(EV)` (ENS-3022) ve seçim-rasyonalitesi
  (ENS-2004 §5ii) **birlikte bozulur** — gözlemle çürütülebilir bir bağımlılık. Doküman bunu
  "en ciddi yeni koşul" olarak işaretlemiş; bu, R2/Faz-4 ampirik sınavına havale edilmiş.
- **intent:** eğer Owner'lar rutin exploit kararlarını sistematik `explore` etiketlerse
  Decision Entropy ölçümü boşalır — etiketleme-oranı denetimiyle *gözlenebilir* bir başarısızlık.
  Doküman bunu da işaretlemiş.

Kusur yok: her iki iddia da hangi gözlemin onları çürüteceğini söylüyor.

## Varsayım haritası
| Varsayım | Kırılma koşulu | Dokümanda durumu |
|----------|----------------|-------------------|
| Alternative-başına cardinal EV *elicit edilebilir* | Çok-boyutlu değer (parasal/itibari/stratejik) tek eksene indirgenemezse `spread(EV)` anlamsızlaşır | ✅ §Failure "ölçek/kıyaslanabilirlik" + "elicitasyon" olarak açık; ama iki *ayrı* kıyaslanabilirlik karışıyor (talep 2) |
| intent commitment-event'inin *içinde* ve depolama *değişmez* (event-sourced) | Değişebilir satır-deposu ya da ayrı sonraki bir `IntentTagged` event → post-hoc koruma çöker | ⚠️ Metinde "event-sourced, commitment anında" deniyor ama "DecisionCommitted event'inin *alanı*" olduğu keskin belirtilmemiş (talep 1) |
| Bir karar exploit *veya* explore (ikili) | Karışık-niyetli karar (kısmen keşif) ikili etikete sığmaz | ✅ §Failure ikili-sınır olarak açık; literatür de ayrılabilirliği tartışıyor |
| EV Commitment'ta *donar* ve hindsight'a kapalı | Donmamış/güncellenebilir EV → seçim-rasyonalitesi sonuç-bilgisiyle kirlenir | ✅ §Model 3 "Commitment'ta donar (ENS-2004 §5 hindsight koruması)" — tutarlı |

## En güçlü karşı-argüman (steelman)
**Tek `ExpectedValue` skaleri iki *farklı güçte* kıyaslanabilirlik talebini gizlice birleştiriyor
— ve iki tüketici bu yüzden aynı elicitasyon-kırılganlığına *eşit* değil.**

- **ENS-2004 §5(ii) seçim-rasyonalitesi** yalnızca *ordinal* karşılaştırma ister: "seçilen
  Alternative, kararın kendi sıralamasında en yüksek miydi?" Sıralama, magnitüd gerektirmez.
- **ENS-3022 Stake = spread(EV) = max−min/std** *cardinal* magnitüd ister: değerlerin gerçek
  aralığı anlamlı olmalı.

Doküman EV'yi tek "ortak, Purpose-tipi içi kıyaslanabilir ölçek" olarak tanımlıyor ve iki
tüketiciyi eşdeğermiş gibi listeliyor. Ama cardinal tanım *seçim-rasyonalitesi için gereğinden
fazla* (over-provision), *Stake için ise tam da en kırılgan yer*. Sonuç: EV kabalaştığında
seçim-rasyonalitesi **zarif biçimde bozulur** (sıralama kaba EV'de bile çoğu zaman ayakta kalır),
Stake ise **doğrudan anlamsızlaşır**. Doküman bu asimetriyi belirtmiyor; "EV kabaysa ikisi de
bozulur" diyerek daha robust olan tüketiciyi olduğundan kırılgan, daha kırılgan olanı olduğundan
robust gösteriyor. Bu bir *çürütme değil* — çünkü cardinal tanım her ikisine de yeter — ama
sahibinin kapatması gereken bir dürüstlük-inceliğidir: hangi tüketici hangi *güçte* EV'ye
dayanıyor, ayrı ayrı söylenmeli. (Talep 2.)

İkinci steelman: **post-hoc koruma iddiası depolama-değişmezliğine koşullu bir mühendislik
varsayımıdır, teori-içi bir garanti değil.** "Kötü sonucu keşifti diye post-hoc etiketlemek
event-sourcing'le imkânsızlaşır" — doğru, *ama yalnızca* intent, commitment mührüyle **aynı
değişmez event'in alanıysa**. Faz-4 kodunda (`DecisionAggregate.cs`, `DecisionCommitted`
record'u `selectedAlternative/owner/confidence/expectedOutcome` taşıyor; intent **yok**, EV de
yok) bu henüz gerçekleşmemiş — teori için bloke değil (teori kodu beklemez) ama garantinin
"disipline mi yoksa yapıya mı" dayandığı sorusu açık: intent ayrı/sonraki bir event ya da
değişebilir bir alan olarak eklenirse koruma buharlaşır. Literatür de bunu doğruluyor: immutable
decision record ilkesi "yayımlanmış kayıt değiştirilmez; yeni karar supersede eder"
([joelparkerhenderson/decision-record](https://github.com/joelparkerhenderson/decision-record)) —
garanti, tam da bu değişmezlik varsayımına bağlı. (Talep 1.)

## İç tutarlılık
Altı-eksen taraması, v0.2 çekirdeğiyle ve cross-doc'larla **çelişki bulmadı**:

- **§Individuation bozulmadı (bağımsız doğrulama).** Dört koşul: tek Owner, tek Purpose, açık
  Alternatives, tek Commitment. Atom = commitment event (Decision Graph düğümü, sayılabilir).
  (a) ExpectedValue, koşul-3'ün *içinde* bir **alt-alan** olarak modellenmiş — Decision Object
  hâlâ **13 alan** (tablo sayımı doğrulandı: Purpose…intent = 13; EV, `Alternatives`'in
  içinde, 14. üst-alan *değil*). Yeni Alternative ya da yeni commitment doğurmuyor → atom
  sayısı sabit. ✅ (b) intent, koşul-4'ün üzerine basılan etiket; ayrı commitment doğurmuyor.
  `explore` kararı hâlâ tek/mühürlü/sayılabilir atom; yalnızca ENS-3021 *ölçümünden* filtreleniyor
  — bu individuation değil, measurement-filter. ✅ İddia sağ çıktı.
- **Küçük tutarsızlık (bloke etmez):** §Individuation "Decision Entropy/Gravity/Capital tanımlı
  bir küme üzerinde ölçülür" derken artık Entropy yalnızca `intent=exploit` **alt-kümesi**
  üzerinde ölçülüyor. Çelişki değil (v0.3 bunu açıkça "ölçüm filtresi" diyor ve ENS-3021 zaten
  böyle tanımlı) ama genel cümle artık gevşek: farklı yasalar aynı atom kümesinin *farklı
  alt-kümeleri* üzerinde çalışıyor. Bir satırlık kabul yeterdi. (Talep 4.)
- **Faz-yerleşimi tutarlı, gizli faz yok.** EV → Reasoning (Simon "design"); Alternatives+
  Evidence+Confidence zaten orada üretiliyor, EV onların yanında doğal. Yeni faz icat *edilmiyor*;
  mermaid diyagramı mevcut Reasoning düğümüne eklemiş. Commitment'ın "geri-dönülmez mühür"
  doğasıyla çelişmiyor: EV mühürden *önce* (Reasoning) üretilip mührde donuyor — Confidence'la
  bire bir aynı muamele, tutarlı. ✅
- **ExpectedValue ≠ Expected Outcome ayrımı kavramsal olarak net** (EV = tüm seçenekler için
  kıyaslanabilir skaler; Expected Outcome = yalnızca seçilenin zengin öngörüsü). Ama **isim
  yakınlığı gerçek bir drift riski**: ikisi de "Expected" + commitment-öncesi öngörü. ENS
  "ISO gibi yönetilen standart" olduğundan bu tam da sözlük-drift'in yakalanması gereken türü.
  Doküman ayrımı özel bir paragrafla yapıyor (çürütülmez) ama ENS-4000 sözlüğünde "Expected
  Outcome" ayrı disambiguation girdisi yok. Ayrıca ENS-2001 metni "ExpectedValue" (tek kelime,
  kod-stili) ile "Expected Value" (sözlük-kanonik, iki kelime) arasında gidip geliyor. (Talep 3
  — ens-style-guardian'a da dokunur.)
- **Cross-doc:** ENS-3021 §Model 3 ("intent ENS-2001 v0.3'te OE1 olarak eklendi, faz=Commitment,
  bağımsız skeptic bekliyor") ✅ tutarlı; ENS-3022 §Model 1 (`Stake=spread(ExpectedValue(aᵢ))`,
  OL1'e bağlı) ✅ tutarlı; ENS-4000 v0.2.2 M1 girdileri (`Expected Value` per-Alternative +
  `Decision Intent`, ikisi de "skeptic bekliyor") ✅ tutarlı. Yeni ENS-2001 tanımıyla çelişki yok.

## Sahibine talepler (bloke etmeyen keskinleştirmeler — survives statüsünü düşürmez)
1. **intent'in event-yerleşimini keskinleştir.** "event-sourced" yetmez; açıkça belirt: intent,
   `DecisionCommitted` event'inin **bir alanıdır** (mühürle atomik), ayrı/sonraki bir event
   değil — ve post-hoc garantisi *değişmez/append-only depolamaya koşulludur* (event-sourcing'i
   teori-düzeyi varsayım olarak sahiplen). Faz-4 not: `DecisionAggregate.DecisionCommitted`
   henüz intent (ve per-Alternative EV) taşımıyor; teori için bloke değil ama ROADMAP'te
   izlenmeli.
2. **İki kıyaslanabilirliği ayır.** "Purpose-tipi içi kıyaslanabilir ölçek" iki farklı işi
   birleştiriyor: (a) *karar-içi* cardinal kıyaslanabilirlik (spread(EV)=Stake'in anlamlı
   olması için) vs (b) *kararlar-arası* Stake normalizasyonu (ENS-3022, Purpose-tipi z-skoru).
   Ve netleştir: seçim-rasyonalitesi (ENS-2004 §5ii) yalnızca *ordinal*, Stake ise *cardinal*
   EV ister — yani EV kabalaştığında ikisi eşit bozulmaz.
3. **Expected Value / Expected Outcome drift'ini kes.** ENS-4000'e bir disambiguation notu ekle
   (öneriyi ens-philosopher, kanonik biçimi ens-style-guardian karara bağlar); ENS-2001 içinde
   "Expected Value" (sözlük-kanonik iki kelime) yazımını tekilleştir ("ExpectedValue" kod-stilini
   yalnızca alan/kod bağlamına bırak).
4. **§Individuation'daki genel cümleyi bir satır gevşet:** "farklı yasalar aynı atom kümesinin
   farklı alt-kümeleri üzerinde ölçülür (ör. Decision Entropy yalnızca `intent=exploit`)" — atom
   kümesi tek ve tanımlı kalır, ama ölçüm-domaini yasaya göre değişir.

---

*İki alan da düşünmenin değil taahhüdün sınırında duruyor: EV, Reasoning'de doğup mühürde
donan bir tahmin; intent, mührün üstündeki geri-alınamaz bir beyan. Individuation sağ çıktı,
atom hâlâ sayılabilir. Kalan iş çürütme değil, keskinleştirme — cardinal ile ordinal'i, alanı
ile disiplini, ismi ile ismi ayırmak.*
