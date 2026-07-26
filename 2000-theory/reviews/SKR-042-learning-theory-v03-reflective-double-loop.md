---
id: SKR-042
type: skeptic-review
origin: ENS-2004
depends_on: [ENS-2004, SKR-009, SKR-010, ENS-2001, ENS-2002, ENS-2003, ENS-4025, ENS-0000]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
---

# SKR-042 — ENS Learning Theory v0.3 (§4a Reflective Double-Loop) Saldırısı

> **Bağımsızlık beyanı (G2/G3).** Bu tur, ENS-2004 v0.3'ü yazan agent çağrısından tamamen
> ayrı, taze context'te yürütüldü. Yazar kendi işini `survives` işaretleyemez; bu bağımsız
> tur o boşluğu doldurur.

## Verdict
**wounded** — §4a mühendislik olarak sağlam ve prior-art dürüstlüğü **örnek niteliğinde**
(GEPA/DSPy/Hermes üçü de bağımsız doğrulandı, birebir doğru konumlanmış); "yeni yasa değil"
iddiası da savunulabilir. Ama üç giderilebilir kusur additive metnin içinde: (1) §4a'nın
"sistematik per-Alternative EV sapması tespit et" iddiası, ENS'in kendi attribution çekirdeğini
(seçilmeyen Alternative'in Actual'ı asla gözlenmez) aşıyor; (2) P5 (Attention kıt kaynaktır)
hiç devrede değil — bir öneri-üreticisi + zorunlu-insan-onay mekanizması, öneri-yorgunluğu
riskini adlandırmadan P7'yi pratikte lastik-damgaya (rubber-stamp) çevirebilir; (3) §1
Definition ("learning ... ilgililik/varsayım modeline yazılan bir güncellemeye dönüştürülür")
ile §4a'nın "bu güncellemeler insan-onaylı öneridir" kaydı arasında uzlaştırılmamış bir yüzey
gerilimi var. `status: review`'da kalır; üç talep karşılanınca `ratified`.

## Yenilik incelemesi — prior art bağımsız doğrulaması (SKR-001 dersi)
Üç atıf da bağımsız WebFetch ile doğrulandı; **hiçbiri uydurma değil, hiçbiri yanlış konumlanmamış.**

- **GEPA (arXiv:2507.19457).** Başlık birebir: *"GEPA: Reflective Prompt Evolution Can Outperform
  Reinforcement Learning."* Metod gerçekten "reflective prompt evolution" + "genetic-pareto"
  (Pareto-frontier'dan içgörü sentezi); RL'i (GRPO) ortalama %6, en çok %20 geçtiğini, 35x daha
  az rollout ile, iddia ediyor. ENS-2004'ün "trace oku → *neden* başarısız analiz et → hedefli
  aday üret" konumlaması **doğru**.
- **DSPy (arXiv:2310.03714).** Başlık birebir: *"DSPy: Compiling Declarative Language Model Calls
  into Self-Improving Pipelines"* (Khattab vd., Stanford). "Self-improving pipelines" +
  optimize-et-değerlendir döngüsü konumlaması **doğru**.
- **Hermes Agent self-evolution (github.com/NousResearch/hermes-agent-self-evolution).** Repo
  gerçek (4.8k yıldız). DSPy+GEPA kullanıyor; constraint-gate birebir doğrulandı (pytest %100,
  boyut sınırı ≤15KB/≤500 char, caching, semantic-preservation) ve dökümantasyon **birebir**
  "All changes go through human review, never direct commit" diyor. ENS'in "constraint-gate +
  insan-PR-onayı, otonom commit YOK → P7 karşılığı" konumlaması **doğru**.

**ENS'in gerçek deltası:** mekanizma icadı değil, üç bağlama — (a) reflektif "neden" prompt'a
değil commit-edilmiş karar atomunun proof-trace'ine (ENS-4025 L8) + §3 attribution seviyesine;
(b) çıktı kod/skill-mutasyonu değil Assumptions/relevance/attribution-hedefi önerisi; (c) insan-
gate P7 invariant'ıyla özdeş. Delta dar ve dürüstçe dar ilan edilmiş. Bu boyutta bir yenilik
iddiası **sağ çıkar**.

## Yanlışlanabilirlik
§4a bir *operasyonel desendir*, ampirik bir iddia değildir — dolayısıyla doğrudan
yanlışlanabilirlik ödevi zayıf. Ama metin bunu dürüstçe sahiplenir: yeni failure condition
"teorik desen sentezidir, çalışan mekanizma değil (E1, Faz-4'te kodlanmadı)" der. Bu bir kusur
değil, dürüst bir maturity beyanı. Desen-sınıfının çalıştığını dış sistemler (GEPA/Hermes)
gösterir; ENS-spesifik bağlamanın (karar-atomu + attribution + P7) çalıştığı **kanıtlanmadı** —
metin bunu da açıkça yazıyor. Sorun yok.

## P7-gate dürüstlüğü (en kritik kontrol) — §4a'nın kendi metni TEMİZ
§4a içinde gizli otomatik-uygulama dili **bulamadım**. Gate dört ayrı yerde tutarlı tekrarlanıyor:
Adım 3 ("Hedefli öneri üret — **asla uygulama**"), P7-kapısı paragrafı ("Öneri **hiçbir zaman
otomatik uygulanmaz**"), §Failure#1 bağı ("önerebilir (uygulamaz)"), §Examples ("**uygulanmaz** —
Owner onaylarsa"). Constitution P7 metniyle ("ENS önerir; emretmez", Madde III, immutable-core)
birebir tutarlı. Bu kontrol **geçer**.

**Ancak — §1 Definition kaçağı (talep 3).** Kritik kontrolün *sınırı* §4a değil, teorinin
tabanı. §1 Definition: learning "Company Memory'ye **ve ilgililik/varsayım modeline yazılan bir
güncellemeye** dönüştürülmesidir." Düz okunuşta bu, güncellemeyi *otomatik* yazar — oysa §4a
tam da ilgililik/varsayım modeli güncellemelerinin insan-onaylı öneri olduğunu söyler. Metin
hangi güncellemenin otomatik (sonucun kayda geçmesi, §1) hangisinin gate'li (double-loop model
revizyonu, §4a) olduğunu **hiçbir yerde ayırmıyor**. Bu bir yalan değil — §4a §1'i daraltıyor
olarak okunabilir — ama uzlaştırılmamış bir yüzey çelişkisi. P7-gate'in "metnin geri kalanıyla
tutarlı mı" sorusunun dürüst yanıtı: §4a içinde evet, taban tanımıyla açık değil.

## §4 ↔ §4a ilişkisi — meşru operasyonelleşme, otonomi genişlemesi YOK
§4a §4'ün somut biçimi mi, yoksa gizli yetki genişlemesi mi? İnceledim: §4 iki hedef sayar
(Assumptions + relevance modeli); §4a üç hedef sunar — üçüncüsü **attribution-seviyesi hedefini
yükseltme** (ör. "L1'e sıkışan Purpose-tipine L2 doğal-deney kur"). Bu §4'te yok. Ama metin
üçüncü hedefi **§3 attribution-merdivenine** açıkça bağlar ve iddiasını "§4 + P7 + **§3**
sentezi" olarak kurar — yani üçüncü hedef gizlice değil, ilan edilmiş §3 katkısı. Üç hedefin
üçü de öneriyle biter (P7). **Hiçbir otonomi/yetki genişlemesi yok.** İddia dürüst.

## §Failure#1 (EV elicitasyonu) bağı — en güçlü teknik itiraz (talep 1)
§4a "sistematik-sapmış **per-Alternative** EV tahminlerini tespit edip önerebilir" diyor. Bu
iddia ENS'in **kendi attribution çekirdeğini aşıyor (§2):** seçilen Alternative `a` için `Y(a)`
(Actual) gözlenir; seçilmeyen `a′` için `Y(a′)` **asla gözlenmez** — bir counterfactual. EV
"sapmasını" ölçmek Actual↔Expected kıyası ister; bu yalnızca **seçilen** Alternative için
mümkündür. Dolayısıyla mekanizma en fazla "*seçilen*-Alternative EV'sinin bir karar sınıfında
sistematik sapması"nı tespit edebilir; **seçilmeyen** Alternative'lerin EV'leri hiçbir zaman
outcome'a karşı doğrulanamaz.

Bunu ENS'in kendi belgeleri de teyit ediyor: ENS-2001 v0.3 §Failure ("ExpectedValue
elicitasyonu, OL1, en ciddi yeni koşul") *seçilmeyen* Alternative EV'lerinin "kaydedilmez ya da
kaba" olabileceğini açıkça söylüyor. Yani §4a'nın girdisi hem gözlem-imkânsız (unchosen'da Actual
yok) hem de kayıt-kırılgan. Döngüsel bağımlılık *tam* değil — çünkü sapma tespiti EV'nin
"doğruluğunu" varsaymaz, Actual'a karşı ıraksamayı ölçer — ama iddia **aşırıdır:** "per-Alternative"
kelimesi, ENS'in attribution ontolojisinin izin verdiğinden fazlasını vaat ediyor.

## Yeni failure condition yeterli mi (talep 5) — P5 boşluğu, en güçlü kaçırılmış itiraz (talep 2)
Yeni koşul iyi kısımları kapsıyor: hangi trace/hangi eşik formelleşmedi, ENS-4025 L8 bağımlılığı,
E1, ve "yanlış *neden* atfı → yanlış Assumption önerisi → memory poisoning" (false-positive öneri
riski). Ama **P5'i (Attention kıt kaynaktır) hiç anmıyor** — ne principles listesinde ([P4, P3,
P6, P7]) ne failure conditions'ta. §4a bir **öneri-üreticisidir** ve her çıktısı zorunlu insan
onayına (P7) tabidir. Bu iki invariant çatışabilir: mekanizma çok öneri üretirse ya (a) insan
onayı lastik-damgaya döner — P7 **fiilen** boşalır — ya da (b) öneriler görmezden gelinir —
mekanizma işe yaramaz. GEPA/DSPy otomasyonunun tüm cazibesi hacimdedir; ENS o hacmi kıt insan
dikkatine (P5) boşaltıyor. Öneri-yorgunluğu (alert fatigue) / dikkat-bütçesi bu additive'in en
büyük operasyonel riski ve failure conditions'ta **yok**.

## Varsayım haritası
- **V1: Karar sınıfları anlamlıdır** ("aynı Purpose-tipi"). Kırılma: Purpose-tipleri heterojen
  ya da az-hacimliyse "sistematik" sinyal istatistiksel gürültüdür (mevcut "kalibrasyon hacmi"
  koşuluyla örtüşür).
- **V2: Proof-trace, Assumption→Expected zincirini taşır.** Kırılma: ENS-4025 L8 proof-trace'i
  *türetilmiş olgu → kural + öncül* için tanımlı; ama karar-öngörüsünün (Assumption→Expected)
  bu formata bağlandığı **henüz yok** (metnin kendi kabulü). O olmadan §4a substratsız.
- **V3: EV'ler ortak-ölçekli ve kayıtlı.** Kırılma: ENS-2001 §Failure'ın işaretlediği
  ölçek/kıyaslanabilirlik ve unchosen-kayıt sorunu — yukarıda (talep 1).
- **V4: İnsan onayı ölçeklenir.** Kırılma: P5 — yukarıda (talep 2).

## En güçlü karşı-argüman (steelman)
*"§4a, otomasyonun faydasını (hacimli, ucuz, sürekli reflektif öneri) alıp maliyetini (kıt insan
dikkati) gizliyor. P7'yi bir erdem olarak sunuyor ama P7 + hacim = ya rubber-stamp (P7 ölür) ya
da ignore (mekanizma ölür). GEPA/Hermes bu gerilimi otomatik-test-gate + boyut-sınırı ile
yönetir — makine-doğrulanabilir constraint'ler insan yükünü azaltır. ENS'in gate'i saf insan
onayı; makine-tarafı constraint'i yalnızca 'proof-trace taşı + L1-etiketli ol' — bu öneri
*hacmini* hiç azaltmaz, yalnızca her önerinin biçimini kısıtlar. Yani ENS, kopyaladığı sistemin
en kritik bileşenini (insan yükünü fiilen kısan otomatik gate) almadan insan-onayını
kopyalamış."* Owner bunu yanıtlamalı: §4a öneri *hacmini* neyle sınırlar (dikkat-bütçesi, eşik,
batch)? Aksi hâlde Hermes-analojisi eksiktir.

## İç tutarlılık
- **ENS-4025 L8:** doğrulandı, proof-trace zorunlu (L8) gerçek. Bağ meşru; ama Assumption→Expected
  eşlemesinin L8 formatına bağlanmadığı açık borç (V2).
- **ENS-2001 v0.3:** per-Alternative ExpectedValue gerçekten *tüm* Alternative'ler için saklanıyor
  (§Model 2). §4a atfı doğru. Ama ENS-2001'in *kendi* §Failure'ı unchosen-EV kırılganlığını
  söylüyor — §4a bunu miras alıyor, kabul etmiyor (talep 1).
- **Glossary (ENS-4000 v0.2.5):** "Reflective Double-Loop" girişi §4a ile **tutarlı** (proof-trace
  L8, "neden" analizi, Assumptions/relevance/attribution-seviyesi önerisi, hiçbir zaman
  otomatik-uygulanmaz/P7, "yeni yasa değil"). Terminoloji sürüklenmesi yok. M1 + "skeptic bekliyor"
  doğru işaretlenmiş. **Geçer** (talep 7).
- **"Yeni yasa değil" (talep 6):** Savunulabilir. §4a yeni bir yanlışlanabilir ampirik iddia ya
  da P7/§3/§4'ün ötesinde yeni normatif kural getirmiyor; ENS-2003 v0.3'ün "KG/RAG sentezi, yeni
  yasa değil" precedent'iyle aynı disiplin. Ayrı ENS-3xxx **hak etmiyor** — bu bir operasyonel
  desen. Bu iddia dürüst.

## Sahibine talepler (kapıyı geçmek için)
1. **EV-sapma iddiasını attribution çekirdeğine hizala (blocking).** §4a'daki "per-Alternative EV
   sapması" ifadesini düzelt: seçilmeyen Alternative'in Actual'ı gözlenemediğinden sistematik-sapma
   yalnızca *seçilen* Alternative EV'si için ampirik kontrol edilebilir. Ya "chosen-Alternative EV
   kalibrasyonu"na daralt, ya unchosen-EV için nasıl (yalnızca iç-tutarlılık/spread, outcome değil)
   bir sinyal olduğunu açıkça söyle. ENS-2001 §Failure'ın unchosen-EV kırılganlığını miras aldığını
   kabul et.
2. **P5'i devreye al + öneri-yorgunluğu failure condition ekle (blocking).** principles'a P5
   ekle; failure conditions'a öneri-hacmi/dikkat-bütçesi riskini yaz: hacimli öneri → ya P7
   rubber-stamp ya ignore. §4a öneri hacmini neyle sınırlar (eşik/batch/dikkat-bütçesi)? Hermes-
   analojisinin makine-gate ile insan-yükü kıstığını, ENS'in gate'inin bunu yapmadığını yanıtla.
3. **§1 Definition ↔ §4a gate'ini uzlaştır (blocking, tek cümle yeter).** §1'de learning'in
   "ilgililik/varsayım modeline yazılan güncelleme"si ile §4a'nın "bu güncellemeler insan-onaylı
   öneri"si arasındaki ayrımı netleştir: sonucun kayda geçmesi (otomatik) ≠ double-loop model
   revizyonu (P7-gate'li).

Üçü de metin-içi, küçük ve giderilebilir. Karşılanınca kavram `ratified`'e döner — çekirdek
sağlam, prior-art örnek, gate dürüst; yalnızca iddia-kapsamı, P5-boşluğu ve taban-tanım gerilimi
kapatılmalı.

## Kaynaklar (bağımsız doğrulandı)
- GEPA: arXiv:2507.19457 — başlık/metod/RL-karşılaştırması WebFetch ile teyit.
- DSPy: arXiv:2310.03714 — başlık/self-improving-pipelines WebFetch ile teyit.
- Hermes Agent self-evolution: github.com/NousResearch/hermes-agent-self-evolution — repo/constraint-gate/"never direct commit" WebFetch ile teyit.
- ENS-0000 Madde III (P5, P7, immutable-core); ENS-2001 v0.3 §Model 2/§Failure; ENS-4025 L8; ENS-4000 v0.2.5 glossary; SKR-009/SKR-010 — repo-içi.
