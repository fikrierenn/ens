# ENS Numaralandırma Kaydı (Numbering Registry)

**Yetki:** [ENS Anayasası, Madde IV & XI](0000-constitution/ENS-0000-constitution.md)

ENS bir **standart** gibi yönetilir. Numaralar kalıcı adreslerdir: bir gün biri
"ENS-3021" dediğinde herkes bunun **Decision Entropy** standardı, "RFC-6042" dediğinde
**Reasoning Engine Interface Specification** olduğunu bilmelidir (IEEE/ISO/TCP-IP mantığı).

## Aralık şeması
| Önek | Aralık | Alan | Dizin |
|------|--------|------|-------|
| `ENS-0xxx` | 0000–0999 | Anayasa ve değişiklikleri | `0000-constitution/` |
| `ENS-1xxx` | 1000–1999 | Felsefe | `1000-philosophy/` |
| `ENS-2xxx` | 2000–2999 | Teori kavramları | `2000-theory/` |
| `ENS-3xxx` | 3000–3999 | Enterprise Laws | `3000-laws/` |
| `ENS-4xxx` | 4000–4999 | Ontoloji | `4000-ontology/` |
| `ENS-5xxx` | 5000–5999 | Mimari standartları | `5000-architecture/` |
| `RFC-6xxx` | 6000–6999 | RFC'ler | `6000-rfc/` |
| `ADR-NNNN` | — | Mimari kararlar | `5000-architecture/adr/` |
| `ENS-7xxx` | 7000–7999 | Reference Implementation spesifikasyonları | `7000-reference-implementation/` |
| `ENS-8xxx` | 8000–8999 | Ürün modülleri | `8000-product/` |
| `ENS-9xxx` | 9000–9999 | Kitap | `9000-book/` |
| `MOD-*` | — | Kod modülleri | `7000-*`, `8000-*` |
| `LAW-*` | — | Yasa kimlikleri | `3000-laws/` |
| `SKR-*` | 001–046 tahsisli — **sıradaki boş numara: 047** | Skeptic/Validation incelemeleri | `<yapıt-dizini>/reviews/` |
| `STD-*` | — | Standartlar (Madde XII "Standards" katmanı; Külliyat'ın numaralı aralığı dışında, dosya-adı tabanlı kimlik) | `.claude/standards/` |
| `CEO-NNNN` | — | Madde XIV uzun-vade hiza incelemeleri (`ens-ceo`) | `5000-architecture/reviews/` |
| `STYLE-SIGNOFF-*` | — | `ens-style-guardian` şema/tutarlılık imzaları (çift-owner kabul kapısı, ör. RFC-6001 §7.5) | ilgili yapıtın `reviews/` dizini |
| `PLAN-*` | — | Çalışma planları — Külliyat numaralandırmasına tabi değil, dosya-adı tabanlı kimlik | `plans/` |
| `SCAN-*` | — | Tutarlılık/uyum tarama raporları (ör. SCAN-01/02/03) — Külliyat numaralandırmasına tabi değil, dosya-adı tabanlı kimlik | `governance/` |

## SKR kayıt kuralı (2026-07-27, SCAN-02 B-4)
**Karar (`ens-philosopher`):** REGISTRY'nin SKR üzerindeki tek işlevi **numara tahsisidir** —
tekil SKR satırı artık açılmaz. SKR'nin kendi durumu (verdict, bağımlılık, hedef yapıt) ilgili
yapıtın kendi `skeptic_review:` alanında ve SKR dosyasının kendi künyesinde yaşar
("ilgili yapıt içinde", Aralık şeması). REGISTRY yalnızca yukarıdaki tek satırla **sıradaki boş
numarayı** izler.

**Gerekçe:** Tekil satır pratiği (aşağıdaki "legacy" SKR-024..037 satırları) 45 SKR'nin 8'inde
(SKR-001..023, SKR-031, SKR-032, SKR-039, SKR-040..046) hiç uygulanmadı — ve **tek gerçek
numara hatası (SKR-045 çift-tahsisi, SCAN-02 A-6) tam da tabloya hiç girmemiş aralıkta oluştu.**
Kayıt biçimi, atlanması ucuz olduğu için koruma sağlamadı. Tek-satır+sayaç biçimi atlanması zor
bir mekanizmadır ve `reviews/SKR-*.md` dosya adlarının gerçek maksimumuyla **mekanik
karşılaştırılabilir** (ens-ontology-linter V2 adayı bir denetim: "REGISTRY sayacı ↔ disk
maksimumu eşleşiyor mu").

**EC-001 — geriye dönük uygulama yok:** Aşağıdaki `SKR-024`..`SKR-037` satırları **silinmez**,
"legacy, 2026-07-27 öncesi" olarak işaretlenir. Yeni SKR'ler için satır **eklenmez** (SKR-038
ve sonrası zaten tekil satır almamıştı, bu tutarlı devam eder).

## Ayrılmış numaralar (allocated)
| id | Başlık | Statü |
|----|--------|-------|
| ENS-0000 | ENS Anayasası | ratified (v0.3.0, 2026-07-24 — Madde IV RFC-6001 ile amend edildi) |
| RFC-6001 | Constitutive Artifact Ayrımı ve Madde IV Canon Kuralının Düzeltilmesi | **Accepted** (v0.3.0, 2026-07-24 — 3 bağımsız skeptic turu [SKR-034 wounded→SKR-035 wounded→SKR-036 survives] + çift-owner kabul kapısı [`ens-ceo` CEO-0002 hiza-onayı + `ens-style-guardian` STYLE-SIGNOFF-RFC-6001 şema-imzası, ikisi de itirazsız]. **ENS-0000 Madde IV ve STD-METADATA-HEADER fiilen düzenlendi** (§8.4 kapsamı). Korpus retrofit'i (diğer belgelerin `constitutive` alanı, ENS-4000 canon-incelemesi) ayrı sonraki adım — ROADMAP.md. G-03/05 kapandı.) |
| RFC-6002 | Doğrulama Kapılarının Kapsamı: `ratified` ≠ `Canonical` | **draft** (v0.1.0, 2026-07-27 — SCAN-03 Ç-01/Ç-02/Ç-03'ü çözer. Çekirdek: G4'ün öznesi `canon: true`'dur, `status: ratified` değil; ≥1 SKR → `ratified` (M2), ≥2 bağımsız boyut → `canon` (M5). İki bağımsız türetme aynı sonuca vardı. Açığa çıkardığı gerçek boşluk: `review→ratified` geçişini yöneten hiçbir kural yok — bugün SKR verdict'i fiilen onay yerine geçiyor (G3 ihlali). Hafif ratifikasyon kapısı R1-R3 önerir. **skeptic turu bekliyor.**) |
| RFC-6003 | Boyut Sözlüğü ve Validator Kadrosu | **draft** (v0.1.0, 2026-07-27 — SCAN-03 Ç-04/Ç-05 + Ö-07. Ölçüm: 45/45 SKR tek rolden (`ens-skeptic`), 0 ethical doğrulama, çok-boyutlu zinciri olan `canon` yapıt sayısı 0. Ampirik kanıt: D-5 çift-sayımı iki *scientific* turdan geçti, bir *engineering* denetimi yakaladı. Ç-04 → `validation-framework.md` kazanır (roles.md onu kaynak gösterir). Ç-05 → kurucu yol G4'ten **muaf değil** (RFC-6001 `:175`). Açık: `constitutional` 6. boyut mu, Ontology Validator kim. **skeptic turu bekliyor.**) |
| ENS-1000 | ENS Manifestosu | ratified (SKR-002 survives) |
| ENS-2001 | Decision Theory (ENS) | ratified (v0.3 — SKR-004 çekirdeği + **SKR-033 survives** [OL1/OE1 additive alanları `ExpectedValue`/`intent`]; bağımsız skeptic turu tamam, 4 bloke-etmeyen keskinleştirme talebi açık) |
| ENS-2002 | Context Theory (ENS) | ratified (SKR-006 survives) |
| ENS-2003 | Company Memory (ENS) | **review** (v0.4.1, 2026-07-27 — **SKR-045 `wounded` yanıtı**: 4 blocking kapatıldı. **T1** retention'a **attribution kapısı** (`RetentionPriority = \|Learning\|` *ancak* `attribution_level ≥ L1` ise; L0 saklanır ama karşı-survivorship tabanı yarışına giremez) — **gate, multiplier değil**, çift-sayım geri gelmez, gerekçe v0.4.0'ın kendi Kalman kazanç argümanından türetildi. **T2** tabanın *"yapısal kapanış"/"zorundadır"* modalitesi **geri çekildi** → taban bir sistem özelliği değil, ENS'in kesme API'sinin sözleşmesidir; `Retrieve(...).Take(k)` onu ihlal eder ve ENS'in denetimi dışındadır. **T3** ENS-2004 v0.4.0 BREAKING yapıldı. **T4** §Prior art'ta "Purpose-tipine koşullu sönüm" bağlaması **"hedeflenen, teslim edilmemiş"** işaretlendi. Non-blocking T5/T7/T8/T11/T12 eklendi (yanlışlayıcı+asimetri; EW-RLS ↔ Kalman seçimi; `\|L\|`'in ex-ante Goodhart'ı; `c` "değişmez" uzlaştırması; tabanın `1/k` maliyeti). **T6/T9/T10 karşılanmadı**, gerekçeleri belgede yazılı. **Faz-4 kodu hizalanmadı** (ayrı iş: `MemoryRecord.AttributionLevel`, `CounterSurvivorshipFloor` eşiği, `AdversarialAuditTests` güncellemesi). **Bağımsız 2. tur BEKLİYOR — öz-onay yok, G2/G3.** Önceki: v0.4.0, 2026-07-26 — BREAKING formül düzeltmesi, `AUDIT-WAVE2-FIDELITY` D-5'e yanıt: attribution confidence hem retention'da hem decay'de sayılıyordu [**çift-sayım**; SKR-040 ve SKR-041'in İKİSİ de kaçırdı]. `RetentionPriority = \|L\|` (c'siz) / `value = \|L\|·c` (= ENS-3023 §Model 1) / `decayFactor = exp(−λ_π·Δt)` (c'siz, `λ_π = ln2/τ_π`) ayrıştırıldı; `γ`/`λ_base` kaldırıldı; **karşı-survivorship tabanı** (kesme invariant'ı) eklendi; Curator'a ikinci sinyal (`weakly-attributed`). Faz-4 kodu+testleri güncellendi ama **derlenip test edilemedi** (context'te shell yok) — CI teyidi bekliyor. **Bağımsız skeptic turu BEKLİYOR — öz-onay yok, G2/G3.** Önceki: v0.3.1 ratified, SKR-008/040/041) |
| ENS-2004 | Learning Theory (ENS) | **review** (**v0.4.0 BREAKING**, 2026-07-27 — **SKR-045 yanıtı**: (1) **B1/T1** v0.3.3 §Implications'ın `RetentionPriority = \|learning_signal\|` formülü **kapısızdı** ve §Laws'ın *"L0'da learning yok"*uyla aynı belgede çelişiyordu (v0.3.2'de `×c` çarpanı L0'ı sıfırlıyordu; v0.3.3 çelişkiyi ENS-2003'ten buraya **taşıdı**) → **attribution kapısı** eklendi (gate, multiplier değil). (2) **B3/T3** v0.3.3'ün *"dar ve additive"* nitelemesi **olgusal olarak yanlıştı ve geri çekildi**: `= \|L\|·c` sözleşmesi gerçekleşmişti (v0.3.2 → ENS-2003 v0.3.1 → Faz-4 kodu → `RetentionPriority_matches_DecisionCapital_Value_by_design` testi); "by design" test edilip sonra "artık yanlış" işaretlenen davranış **breaking'dir** → **0.3.3 → 0.4.0**. Ayrıca T8 (§Failure Goodhart maddesi `Expected` manipülasyonuna genişletildi: taban ele geçirme) ve yeni failure condition (merdiven ↔ `c` tutarlılığı zorlanmıyor; `attribution_level` Faz-4'te hiç yok). **T9/N3 `confidence` homonimi KARŞILANMADI** — sözlük turu işi. **Bağımsız 2. tur BEKLİYOR — öz-onay yok, G2/G3.** Önceki: v0.3.3, 2026-07-26 — §Implications hizalaması: "Memory retention = \|L\|×c" satırı kendi içinde çelişiyordu; artık **retrieval ağırlığı** (= ENS-3023 `value(d)`) olarak adlandırıldı ve retention'ın `c`'den bağımsız olduğu (ENS-2003 v0.4.0 §3) yazıldı. Dar/additive, D-5'in yan etkisi; **bağımsız skeptic turu bekliyor**. Önceki: v0.3.2 ratified — §4a Reflective double-loop, GEPA/DSPy/Hermes prior art; SKR-042 wounded→SKR-043 wounded→SKR-044 **survives**; `principles: +P5`; Faz-4 kod: `ReflectiveDoubleLoop.cs`) |
| ENS-3000 | Enterprise Laws (kayıt) | draft |
| ENS-3021 | Decision Entropy | ratified (SKR-012 survives) |
| ENS-3022 | Decision Gravity | ratified (SKR-014 survives) |
| ENS-3023 | Decision Capital | ratified (SKR-016 survives) |
| ENS-4000 | Sözlük (Glossary) | review (v0.2.5, 2026-07-24 — G-11 terfi borcu kapandı; RFC-6001 ile terminoloji-sink [depends_on: yalnızca ENS-0000]; +M1 girdiler birikti: `Expected Value`/`Decision Intent` [ENS-2001 v0.3], `Salience Decay`/`asserted_at`/`last_verified`/`Stale Flag`/`Memory Curator` + **v0.4.0 ile yeniden yazılan/eklenen** `Context Half-Life (τ_π)`/`Retention Priority`/`Counter-Survivorship Floor`/`Weakly-Attributed Flag` [ENS-2003 v0.4.0, D-5 çift-sayım düzeltmesine hizalı], `Reflective Double-Loop` [ENS-2004 v0.3.2] — hepsi skeptic bekliyor; ENS-4000'in kendi canon-borcu G-03/05/RFC-6001 §8.3'te açığa alındı, ayrı retrofit adımı) |
| GOV-000 | Governance İlkeleri | review (M1) |
| GOV-010 | Governance Rolleri | review (M1) |
| GOV-020 | Capability Matrix | review (M1) |
| GOV-030 | Canonical Process | review (M1) |
| ENS-4001 | ENS Meta Model | review (v0.4 — +Computational Ontology EXPERIMENTAL, Identity/Event/Capability, 3-senaryo stres testi geçti, Ontology Validation bekliyor) |
| ENS-4010 | Foundational Ontology → Organizational Ontology | skeptic-cleared (**M2**, v0.5.0, 2026-07-24 — blocking-5 Kusur 1/2/3 + SKR-038 Yara A/B düzeltildi [Assertion/Resource/Intent profil node-tipine-özel yeniden yazıldı, `derived_from` Registry'ye eklendi, `part_of` domain genişletildi, kök-Organization çıkarımsal tanımlandı]. **SKR-039 bağımsız 2. tur (G2/G3) → `survives`:** Yara A-1/A-2/B kapandı + kapsamlılık testi geçti [17 node + 2 Trans:✓ relation sıfırdan türetildi, SKR-038/D-1 deseninin 3. tekrarı YOK]. 3 bloke-etmeyen keskinleştirme [Deliberative flat-profil, `ens-core:derived_from`↔`ens-meta:derived_from` homonimi, downstream propagasyon]. ratified/canon ayrı governance edimi. G-02 (2026-07-23): Ontology→Theory yönü DOĞRU teyit edildi.) |
| ENS-4025 | Semantic Logic | ratified (M2, SKR-022; v0.1.1 — SKR-031 D-1 düzeltmesi: proof-trace örneği Registry'ye hizalandı) |
| ENS-4030 | Semantic Axioms | ratified (M2, SKR-021) |
| ENS-4031 | Inference Rules | skeptic-cleared (M0, v0.3.0 — canon:false; 8 kural IR-001..IR-008; SKR-031 wounded→B1/B2/D-1 düzeltildi→SKR-032 survives; B1 ✅ kapandı — IR-002/IR-005 `part_of` artık Registry-lisanslı, ENS-4010 v0.5.0 + SKR-038/039; ratified/canon ayrı governance edimi) |
| ADR-0001 | ENS Agent Runtime & Bounded Autonomy | **Accepted** (v0.3.1, 2026-07-24 — 3. bağımsız tur SKR-029 **survives**: Bulgu A/B gerçekten kapandı; CEO-0001 ile ens-ceo hiza incelemesi ONAYLANDI — Proposed → Accepted, Madde XIV) |
| ADR-0002 | Operations Capability Pack | **Accepted** (v0.3.1, 2026-07-24 — SKR-025+027 wounded → v0.3 düzeltme → SKR-037 survives [bağımsız operax dosya-denetimi] → K4 ens-ceo hiza CEO-0003 Accepted. operax'ın aktif geliştirmesi durdu [kullanıcı kararı], K1 zemini bugünkü koda dayandığından etkilenmiyor.) |
| ENS-4020 | Enterprise Ontology (Operations/operax) | review (**M2**, v0.3 — SKR-028+SKR-030 iki bağımsız validator → G4; C.a/C.b düzeltmeleri teyit edildi; ratified ayrı governance edimi) |
| *(aşağıdaki tekil `SKR-*` satırları — SKR-024'ten SKR-038'e — **legacy, 2026-07-27 öncesi**; §SKR kayıt kuralı (EC-001) gereği silinmez ama yeni SKR'ler için artık satır açılmaz)* | | |
| SKR-024 | ADR-0001 Validation (inline, G2/G3 riski) | review (wounded — SKR-026 ile bağımsız yeniden yargılandı) |
| SKR-025 | ADR-0002 Validation (inline, G2/G3 riski) | review (wounded — SKR-027 ile bağımsız yeniden yargılandı) |
| SKR-026 | ADR-0001 Bağımsız Validation (engineering) | draft (wounded — G2/G3/G4 karşılar) |
| SKR-027 | ADR-0002 Bağımsız Validation (engineering) | draft (wounded — 2 blocking: ENS-4020 doğrulanmamış + döngü) |
| SKR-028 | ENS-4020 Ontology Validation (ontology) | draft (wounded→v0.3'te giderildi: Replenishment bölündü + SupplierRelationship→Resource; SKR-027 Bulgu C kapatır) |
| SKR-029 | ADR-0001 3. Bağımsız Teyit Turu (engineering) | draft (**survives** — Bulgu A/B gerçekten kapandı; ENS-2001 Enactment incelik borcu + §1↔§4 gerilimi non-blocking) |
| SKR-030 | ENS-4020 M2 Teyit Turu (ontology) | draft (**survives→M2** — C.a/C.b teyit; 2 confirmed ENS-4010 profil↔registry çelişkisi yukarı-bildirildi) |
| SKR-033 | ENS-2001 v0.3 OL1/OE1 Validation (scientific, bağımsız) | draft (**survives** — individuation bozulmadı bağımsız doğrulandı; 4 bloke-etmeyen keskinleştirme talebi: intent-event yerleşimi, ordinal/cardinal EV ayrımı, Expected Value/Outcome drift, ölçüm-domaini cümlesi) |
| SKR-034 | RFC-6001 Constitutive Ayrımı Saldırısı (constitutional, bağımsız) | draft (**wounded** — çekirdek tez + tek-RFC kararı sağlam; 3 blocking: D1 §7.3↔ENS-4001/4010/4025 `maturity:M2` çakışması, D2 kök hard-core heterojenliği [Madde III yanlışlanamaz çekirdek], D3 4000-aralığı sınıflaması ertelenmiş/turnusolsuz; +ENS-4000 `canon:true` ama `status:review` canon-ölçütü ihlali) |
| SKR-035 | RFC-6001 Constitutive Ayrımı 2. Tur Saldırısı (constitutional, bağımsız) | draft (**wounded** — D2/D3-çekirdek/D4/D5 gerçekten kapandı; 1 blocking D6: `maturity⟺constitutive:false` çift-yönlü invariant'ı governance ailesinin [GOV-000/010/020/030] fiili `maturity:M1` künyeleriyle çelişiyor, §10.5 "GOV-* M-ekseninde değildir" olgusal yanlış; 2 keskinleştirme D7 turnusol-önceliği/D8 immutable-core yordamı; RFC v0.3'te karşılandı, 3. tur bekliyor) |
| SKR-036 | RFC-6001 Constitutive Ayrımı 3. Tur Saldırısı (constitutional, bağımsız) | draft (**survives** — D6 çözümü [tek-yönlü invariant + turnusol-birincil] doğrulandı; korpus taraması §12 ile birebir [GOV id eşlemesi teyit]; governance evidence eng/ops/econ=E0 → FC#5 çöküş koşulu tetiklenmiyor; D7/D8 kapalı; döngü yakınsama, yamalı-sarmal değil. 3 bloke-etmeyen keskinleştirme S1 [invariant tek yasak hücre]/S2 [amends'e STD-MATURITY-MODEL — canon kuralının 3. lokusu]/S3 [governance M1 nitelemesi]. Sıradaki adım: ens-ceo Madde XIV hiza + ens-style-guardian şema-imzası) |
| SKR-037 | ADR-0002 v0.3 K2 Bulgu 1/2/3 Bağımsız Doğrulama (engineering, bağımsız) | draft (**survives** — Bulgu 1'in operax iddiaları D:\Dev\operax'ta bağımsız Glob/Grep ile doğrulandı [RFQ=0 dosya, M04=yalnızca satış-fiyat spec'i, optimizasyon kodu=0, 3 kod-doğrulanmış lifecycle, K1≥2 konservatif]; Bulgu 2/3 kapalı; ContextScore.cs referansı doğrulandı. 2 bloke-etmeyen gözlem [traceability asimetrisi, price-variance lifecycle statüsü]. Kalan: K4 ens-ceo hiza) |
| SKR-038 | ENS-4010 v0.4.0 blocking-5 Kusur 1/2/3 Bağımsız Doğrulama (ontology, bağımsız) | draft (**wounded** — üç hedeflenmiş yama [Assertion/Resource profil, part_of domain-widening] DOĞRU bulundu; 2 yara: A) v0.4.0'ın kendi eklediği "Profile satisfiability" invariant'ı tüm profillere uygulanmamış — aynı sınıftan taranmamış 4. kusur [Intent/Goal/served-by + Rule/derived_from]; B) Agent profilindeki "kök Organization muaf" istisnası mekanik denetlenemez. Talep 1/2 karşılanınca survives) |
| STD-METADATA-HEADER | Künye Standardı | ratified (v0.2.0, 2026-07-24 — RFC-6001 ile `constitutive`/`immutable_core_sections` alanları eklendi) |
| STD-TRACEABILITY | İzlenebilirlik Standardı | ratified (v0.1.0, G-04 2026-07-24) |
| STD-LANGUAGE-POLICY | Dil Politikası | ratified (v0.1.0, G-04 2026-07-24) |
| STD-MATURITY-MODEL | Olgunluk Modeli | ratified (v0.1.0, G-04 2026-07-24) |
| STD-EVIDENCE-STANDARD | Kanıt Standardı | ratified (v0.1.0, G-04 2026-07-24) |
| STD-VALIDATION-FRAMEWORK | Doğrulama Çerçevesi | ratified (v0.1.0, G-04 2026-07-24) |
| STD-CONTEXT-MANAGEMENT | Bağlam Yönetimi | ratified (v0.1.0, G-04 2026-07-24) |
| STD-ARCHITECTURE-PRINCIPLES | Mimari İlkeler | ratified (v0.1.0, G-04 2026-07-24) |
| STD-CODING-STANDARDS | Kod Standartları | ratified (v0.1.0, G-04 2026-07-24) |
| STD-DOCUMENTATION-STYLE | Doküman Stili | ratified (v0.1.0, G-04 2026-07-24) |
| STD-ENS-PHASE-MODEL | ENS Faz Modeli | ratified (v0.1.0, G-04 2026-07-24) |
| CEO-0001 | ADR-0001 Uzun-Vade Hiza İncelemesi (Madde XIV) | ratified (2026-07-23 — ONAYLANDI: Proposed → Accepted; SKR-024/026/029 zincirine dayanır) |
| CEO-0002 | RFC-6001 Uzun-Vade Hiza İncelemesi (Madde XIV) | ratified (2026-07-24 — `ens-ceo` hiza onayı verildi; Madde XIV'in yalnızca `ens-ceo` yarısı, `Accepted` için STYLE-SIGNOFF-RFC-6001 de gerekliydi) |
| CEO-0003 | ADR-0002 Uzun-Vade Hiza İncelemesi (Madde XIV) | ratified (2026-07-24 — ONAYLANDI: Proposed → Accepted; operax'ın aktif-geliştirme-durdu kararının ADR'yi zayıflatmadığı teyit edildi) |
| STYLE-SIGNOFF-RFC-6001 | RFC-6001 Şema/Tutarlılık İmzası (Madde XIV çift-owner kapısının `ens-style-guardian` yarısı) | final (v0.1.0, 2026-07-24 — itirazsız; RFC-6001 `Accepted` durumunu CEO-0002 ile birlikte açtı) |

## Örnek hedef tahsisatlar (henüz yazılmadı)
| id | Planlanan başlık |
|----|------------------|
| ENS-2010 | Enterprise Physics |
| ENS-2021 | Company Memory / Organizational Memory |
| RFC-6042 | Reasoning Engine Interface Specification |

> Yeni numara tahsisi: yapıt oluşturulurken bu tabloya satır eklenir. Numara asla
> yeniden kullanılmaz; kaldırılan yapıt `superseded` işaretlenir, numarası korunur.
