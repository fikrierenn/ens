---
id: ENS-4000
title: Sözlük — Kanonik Terminoloji
type: ontology
canon: true
constitutive: true
origin: ENS-0000 §IV
depends_on: [ENS-0000]
referenced_by: [ENS-2001, ENS-2002, ENS-2003, ENS-2004, ENS-3021, ENS-3022, ENS-3023, ENS-4001]
principles: [P1, P2, P3, P4, P5]
status: review
owner: ens-style-guardian
version: 0.2.5   # v0.2.5: Reflective Double-Loop terimi (ENS-2004 v0.3 §4a) M1 eklendi; skeptic bekliyor
last_reviewed: 2026-07-24
---

# ENS Sözlük — Kanonik Terminoloji

**Yetki:** [ENS Anayasası, Madde IV](../0000-constitution/ENS-0000-constitution.md)

Buradaki terimler kanoniktir. Hiçbir yapıt bunları yeniden tanımlayamaz, çelişemez ya
da sessizce takma adlandıramaz. Yeni terimler yalnızca buraya, `ens-philosopher` önerisiyle
girer ve `ens-skeptic` tarafından sınanır. Terimlerin **kanonik biçimi orijinaldir**
(adresleme ve terminoloji bütünlüğü için); açıklama Türkçedir. Her giriş: **terim — tek
satır tanım — statü**. Kavramların tam teorik işlenişi `2000-theory/` altındadır; bu dosya
adlandırmanın tek doğruluk kaynağıdır.

**Bağımlılık yönü (v0.2.1 — G-17 kapatıldı):** bu dosya bir **terminoloji-sink**'tir —
yalnızca Anayasa'ya (`ENS-0000`) bağımlıdır, `2000-theory`/`3000-laws`'a değil. Teori/Yasa
belgeleri terim tanımına bakmak için buraya `depends_on: ENS-4000` ile başvurur (normal
sözlük semantiği: kullanan tanıma bakar, tanım kullanana bağımlı olmaz); bu dosya onları
`referenced_by` ile geri işaretler. Tersi yön (Glossary → Theory/Laws) Madde XII'yi ihlal
eden bir döngü yaratırdı (Theory → Glossary → Theory).

**Statü (v0.2 — maturity-model.md ile hizalandı; G-11 terfi borcu kapatıldı):** İki bağımsız
eksen var, karıştırılmaz (bkz. KULLIYAT.md constitutive/canonical ayrımı):
- **`constitutive`** — Anayasa'nın katmanlı referans modelinde (architecture-principles.md
  Layer 0-6) doğrudan adlandırılmış, normatif terim. Kanıt-zinciriyle değil, tanımla yürürlükte.
- **`maturity: M0..M5`** — ampirik/teorik iddia taşıyan terimler için (maturity-model.md).
  `M3` = ratified teori/yasa (Faz 1-2'den geçti, ama henüz Canon değil — M5 Faz 4 ister).
  `M0/M1` = önerilmiş, henüz teori belgesi yok ya da skeptic'ten geçmedi.
- Eski `canonical`/`provisional` ikili etiketi **kaldırıldı** — "canonical" M5 ile karıştığı için.

## Temel duruş
- **Enterprise Nervous System (ENS)** — bir şirketi decision-producing cognitive system
  olarak ele alan kurumsal bilişsel altyapı. *constitutive* (Anayasa Madde I)
- **Decision-Producing Cognitive System** — birincil çıktısı transaction değil karar olan
  bir varlık olarak şirket modeli. *constitutive* (Anayasa Madde I)
- **Enterprise Cognitive Infrastructure** — ENS'in ait olduğu kategori; örgütsel akıl
  yürütmenin altyapısı. *constitutive* (Anayasa Madde I)

## Decision ailesi
- **Decision** — en küçük anlamlı örgütsel birim (P1). Şunları taşır: Purpose, Context,
  Alternatives (her biri bir Expected Value taşır), Evidence, Assumptions, Risks, Owner,
  Confidence, Expected Outcome, Actual Outcome, Learning, Memory Links, intent. **M3** (ENS-2001,
  ratified — SKR-004; `intent` ve per-Alternative `Expected Value` alanları v0.3'te eklendi,
  bağımsız skeptic bekliyor)
- **Expected Value** (per-Alternative) — bir kararın her Alternative'i için, sonuçtan bağımsız
  kaydedilen beklenen değer/etki tahmini (ortak, Purpose-tipi içi kıyaslanabilir ölçek).
  `Stake = spread(ExpectedValue)` (ENS-3022) ve seçim rasyonalitesi (ENS-2004 §5ii) için.
  **M1** (ENS-2001 v0.3 Decision Object alanı; skeptic bekliyor — OL1)
- **Decision Intent** — bir kararın commitment anında (sonuçtan önce, event-sourced) konan
  `exploit | explore` etiketi; kasıtlı keşfi istenmeyen tutarsızlıktan ayırır. **M1** (ENS-2001
  v0.3 Decision Object alanı; ENS-3021 §Model 3 ölçüm-filtresi — OE1, skeptic bekliyor)
- **Decision Capital** — geçmiş kararların ve öğrenilmiş sonuçlarının birikmiş değeri.
  **M3** (ENS-3023, ratified — SKR-016)
- **Decision Velocity** — zaman içinde kaliteli karar üretme hızı. **M0** (teori belgesi yok)
- **Decision Energy** — bir karara varmak için gereken çaba/dikkat. **M0** (teori belgesi yok)
- **Decision Entropy** — örgüt büyüdükçe karar tutarlılığının bozulma eğilimi.
  **M3** (ENS-3021, ratified — SKR-012; formeli `H(A|C)`)
- **Decision Gravity** — büyük kararlar daha çok context çeker.
  **M3** (ENS-3022, ratified — SKR-014; formeli `Stake × (1−Confidence)`)
- **Decision Surface** — örgütün o an verdiği açıktaki karar kümesi. **M0** (teori belgesi yok)
- **Decision Friction** — bir kararı yavaşlatan/bozan direnç. **M0** (teori belgesi yok)
- **Decision Confidence** — bir kararın niyetine ulaşma kalibre olasılığı. **M1** (ENS-2001
  Decision Object alanı olarak kullanımda; bağımsız teorisi yok — bkz. ENS-2004 §5 kalibrasyon)
- **Decision Network** — birbirini etkileyen kararların bağlı yapısı. **M0** (teori belgesi yok)
- **Decision DNA** — bir karar sınıfının yeniden kullanılabilir örüntü kodlaması. **M0**
- **Decision Graph** — kararların, context'lerinin ve sonuçlarının grafiği.
  *constitutive* (ENS-4001 Meta Model; Node/Relation Registry'de yapısal desen)

## Context & Trust
- **Context Density** — bir karara bağlı ilgili context miktarı. **M3** (ENS-2002 §Model 2,
  Context Theory'nin ratified gövdesi içinde formelleştirilmiş)
- **Context Score** — bir karar için context yeterliliği ölçüsü. **M3** (ENS-2002 §Model 3)
- **Trust Coefficient** — bir kaynağın/agent'ın kalibre güvenilirlik ağırlığı.
  **M0** (Trust Theory henüz yazılmadı)

## Memory & Knowledge
- **Company Memory** — yalnızca *ne*'yi değil *neden*'i saklayan; örgütün karar belleği.
  **M3** (ENS-2003, ratified — SKR-008)
- **Living Knowledge** — sonuçlar ölçüldükçe güncellenen bilgi. **M0** (teori belgesi yok)
- **Salience Decay** — bir memory assertion'ın **saf tazelik faktörünün** (`decayFactor = exp(−λ_π·Δt)`,
  aralık `(0,1]`) zamanla sürekli sönümü. Sönüm hızı **Purpose-tipinin context değişim hızına**
  bağlıdır (`λ_π = ln2/τ_π`), attribution confidence'a **değil** — `c`'yi sönüme de koymak, onu zaten
  içeren retrieval ağırlığıyla **çift-sayım** üretiyordu (v0.3'ün hatası; AUDIT-WAVE2/D-5).
  Bu, retrieval-sıralama bileşiği **`Salience = value(m) × decayFactor`**'in yalnızca tazelik
  terimidir. **M1** (ENS-2003 v0.4.0 §3a; skeptic bekliyor)
- **Context Half-Life** (`τ_π`) — bir Purpose-tipinin kararlarını geçerli kılan bağlamın yarı yarıya
  bayatlaması için geçen süre (gün). Sönümün **tek** kalibrasyon parametresi; Enterprise Ontology'de
  Purpose-tipi sınıfının bir özelliği olarak yaşar. **M1** (ENS-2003 v0.4.0 §3a; skeptic bekliyor)
- **Retention Priority** — bir memory kaydının *kaybolmama* önceliği: `RetentionPriority(m) =
  |Learning(m)|`. Attribution confidence'tan **bağımsızdır** — atfı zayıf bir ders daha az güvenle
  konuşmalıdır (düşük `value`), ama daha az korunmayı hak etmez. Retrieval ağırlığı (`value = |L|·c`,
  ENS-3023 §Model 1) ile **karıştırılmamalıdır**. **M1** (ENS-2003 v0.4.0 §3; skeptic bekliyor)
- **Counter-Survivorship Floor** — kesme (truncation) invariant'ı: bir Purpose-tipinden `k ≥ 1` kayıt
  alındığında, o tipin `argmax RetentionPriority` kaydı kümede kalmak **zorundadır** — `c`'si ne kadar
  düşük, yaşı ne kadar büyük olursa olsun. §3'ün "sıkıştır ama en az bir başarısızlık örneğini koru"
  politikasının genelleştirilmiş, zorlanabilir hâli. **M1** (ENS-2003 v0.4.0 §3; skeptic bekliyor)
- **Weakly-Attributed Flag** — `c(m) < c_min` olan bir assertion'a konan **epistemik eksen** inceleme
  bayrağı; anlamı *"attribution seviyesini yükselt"*tir (ENS-2004 §4a adım 3(iii)), *"yeniden doğrula"*
  değil (o `stale`'dir). Bir sinyaldir; silme/mutasyon değil ve `RetentionPriority`'yi düşürmez.
  **M1** (ENS-2003 v0.4.0 §3a/§3b; skeptic bekliyor)
- **`asserted_at`** (Assertion Time) — bir memory assertion'ın ilk-keşif/ilk-kayıt anı;
  **değişmez** audit çapası. P6/Explainability. **M1** (ENS-2003 v0.3 §3a; skeptic bekliyor)
- **`last_verified`** (Verification Time) — bir assertion'ın en son teyit anı; yeniden-
  doğrulanınca güncellenir, sönüm saatinin başlangıcı. **M1** (ENS-2003 v0.3 §3a; skeptic bekliyor)
- **Stale Flag** — saf `decayFactor`'ı yeniden-doğrulama eşiğini (`θ`, saf tazelik ekseninde) geçmiş
  assertion'a konan **tazelik ekseni** bayrağı; anlamı *"bağlam değişmiş olabilir, yeniden doğrula"*.
  Bir **inceleme sinyalidir**, otomatik silme/mutasyon değil. Epistemik eksendeki kardeşi
  `Weakly-Attributed Flag`'tir. **M1** (ENS-2003 v0.4.0 §3a; skeptic bekliyor)
- **Memory Curator** — periyodik/inactivity-tetikli uzlaştırma turu; bayat/çelişen assertion'ları
  incelemeye çıkarır ve yeniden-doğrulama *önerir* — asla otonom commit/silme yapmaz (P7).
  **M1** (ENS-2003 v0.3 §3b; skeptic bekliyor)
- **Memory Graph** — Company Memory'nin grafik yapısı.
  *constitutive* (ENS-4001 Meta Model yapısal deseni)
- **Knowledge Graph** — kurumsal varlıkların ve ilişkilerin bağlı temsili (Layer 1).
  *constitutive* (architecture-principles.md Layer modeli)

## Cognition & Intelligence
- **Enterprise Physics** — ölçekte karar davranışını yöneten yasalar bütünü.
  **M0** (şemsiye teori henüz yazılmadı; bkz. ENS-3021/3022/3023 tekil yasalar M3)
- **Organizational Consciousness** — bir örgütün kendi karar durumunun farkındalığı. **M0**
- **Reasoning Engine** — hipotez, alternatif, confidence, açıklama üretir (Layer 3).
  *constitutive* (Layer modeli; çalışma-zamanı karşılığı ADR-0001 Cognitive Kernel)
- **Simulation Engine** — gerçeklikten önce "ne olur eğer…" sınar (Layer 4).
  *constitutive* (Layer modeli; henüz mimari kararı yok)
- **Decision Engine** — emir değil, öneri üretir (Layer 5).
  *constitutive* (Layer modeli; çalışma-zamanı karşılığı ADR-0001 §5.4 Action/Actuation)
- **Prediction Layer** — reasoning'i besleyen sonuç tahminleri. **M0**
- **Memory Layer** — mimari katman olarak Company Memory (Layer 2).
  *constitutive* (Layer modeli; teorisi ENS-2003 M3)
- **Learning Layer / Learning Engine** — sonuçları ölçer, belleği günceller, reasoning'i
  iyileştirir (Layer 6). *constitutive* (Layer modeli; teorisi ENS-2004 **M3**, ratified — SKR-010)
- **Reflective Double-Loop** — geçmiş commit-edilmiş kararların proof-trace'ini (ENS-4025 L8)
  okuyup sistematik öngörü hatasının **"neden"**'ini analiz eden ve Assumptions / relevance-model /
  attribution-seviyesi için **hedefli bir iyileştirme önerisi** üreten öğrenme mekanizması; öneri
  **hiçbir zaman otomatik uygulanmaz — insan onayı (P7) gerekir**. Yeni yasa değil; §4 double-loop +
  P7 + attribution-merdiveninin sentezi (prior art: GEPA/DSPy/Hermes self-evolution). **M1** (ENS-2004
  v0.3 §4a; skeptic bekliyor)
- **Enterprise IQ / Organizational Intelligence** — bir örgütün kaliteli karar üretme
  ölçülebilir yetisi. **M0** (teori belgesi yok)

## Laws (bkz. [ENS-3000](../3000-laws/ENS-3000-laws.md))
Kayıt defteri **M3** (ratified, ampirik-formel olmayan yasa özetleri); tekil operasyonel
biçimleri (Entropy/Gravity/Capital) ayrıca M3 teori belgeleridir (yukarıda).
- **Law of Decision Gravity** — büyük kararlar daha çok context çeker. → ENS-3022
- **Law of Organizational Memory** — unutulan kararlar tekrarlanan hatalara dönüşür. → ENS-2003
- **Law of Context** — karar kalitesi context azaldıkça düşer. → ENS-2002
- **Law of Entropy** — örgüt büyüdükçe karar tutarlılığı azalır. → ENS-3021
- **Law of Learning** — ölçülmeyen kararlar asla iyileşmez. → ENS-2004

> **Terfi kuralı (v0.2):** bir `M0/M1` terim; teori belgesi yazılıp failure conditions
> belirtildiğinde ve skeptic incelemesinden survives aldığında `M3`'e terfi eder. `M3`'ten
> `M5` (Canon) terfisi yalnızca GOV-030 kanıt zinciriyle (Faz 4 reference platform) olur —
> bkz. maturity-model.md. *Constitutive* terimler bu zincire tabi değildir; Anayasa/mimari
> tanımıyla yürürlüktedir.
