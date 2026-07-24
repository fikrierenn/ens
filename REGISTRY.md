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
| `SKR-*` | — | Skeptic incelemeleri | ilgili yapıt içinde |
| `STD-*` | — | Standartlar (Madde XII "Standards" katmanı; Külliyat'ın numaralı aralığı dışında, dosya-adı tabanlı kimlik) | `.claude/standards/` |

## Ayrılmış numaralar (allocated)
| id | Başlık | Statü |
|----|--------|-------|
| ENS-0000 | ENS Anayasası | ratified (v0.3.0, 2026-07-24 — Madde IV RFC-6001 ile amend edildi) |
| RFC-6001 | Constitutive Artifact Ayrımı ve Madde IV Canon Kuralının Düzeltilmesi | **Accepted** (v0.3.0, 2026-07-24 — 3 bağımsız skeptic turu [SKR-034 wounded→SKR-035 wounded→SKR-036 survives] + çift-owner kabul kapısı [`ens-ceo` CEO-0002 hiza-onayı + `ens-style-guardian` STYLE-SIGNOFF-RFC-6001 şema-imzası, ikisi de itirazsız]. **ENS-0000 Madde IV ve STD-METADATA-HEADER fiilen düzenlendi** (§8.4 kapsamı). Korpus retrofit'i (diğer belgelerin `constitutive` alanı, ENS-4000 canon-incelemesi) ayrı sonraki adım — ROADMAP.md. G-03/05 kapandı.) |
| ENS-1000 | ENS Manifestosu | ratified (SKR-002 survives) |
| ENS-2001 | Decision Theory (ENS) | ratified (v0.3 — SKR-004 çekirdeği + **SKR-033 survives** [OL1/OE1 additive alanları `ExpectedValue`/`intent`]; bağımsız skeptic turu tamam, 4 bloke-etmeyen keskinleştirme talebi açık) |
| ENS-2002 | Context Theory (ENS) | ratified (SKR-006 survives) |
| ENS-2003 | Company Memory (ENS) | ratified (SKR-008 survives) |
| ENS-2004 | Learning Theory (ENS) | ratified (SKR-010 survives) |
| ENS-3000 | Enterprise Laws (kayıt) | draft |
| ENS-3021 | Decision Entropy | ratified (SKR-012 survives) |
| ENS-3022 | Decision Gravity | ratified (SKR-014 survives) |
| ENS-3023 | Decision Capital | ratified (SKR-016 survives) |
| ENS-4000 | Sözlük (Glossary) | review (v0.2.2 — G-11 terfi borcu kapandı; +M1 girdiler: `Expected Value` per-Alternative + `Decision Intent` [ENS-2001 v0.3 OL1/OE1, skeptic bekliyor]) |
| GOV-000 | Governance İlkeleri | review (M1) |
| GOV-010 | Governance Rolleri | review (M1) |
| GOV-020 | Capability Matrix | review (M1) |
| GOV-030 | Canonical Process | review (M1) |
| ENS-4001 | ENS Meta Model | review (v0.4 — +Computational Ontology EXPERIMENTAL, Identity/Event/Capability, 3-senaryo stres testi geçti, Ontology Validation bekliyor) |
| ENS-4010 | Foundational Ontology → Organizational Ontology | review (v0.3 — ENS-4001 v0.4'e yeniden konumlandı, Node Registry değişmedi; **G-02 kapandı 2026-07-23: Ontology→Theory yönü DOĞRU teyit edildi — ters-çevirme reddedildi (döngü yaratırdı); Theory künyelerine `referenced_by: ENS-4010` eklendi**) |
| ENS-4025 | Semantic Logic | ratified (M2, SKR-022; v0.1.1 — SKR-031 D-1 düzeltmesi: proof-trace örneği Registry'ye hizalandı) |
| ENS-4030 | Semantic Axioms | ratified (M2, SKR-021) |
| ENS-4031 | Inference Rules | review (M0, v0.2.0 — canon:false; 8 kural IR-001..IR-008; SKR-031 wounded→B1/B2/D-1 düzeltildi; bağımsız 2. skeptic turu bekliyor) |
| ADR-0001 | ENS Agent Runtime & Bounded Autonomy | draft/Proposed (v0.3 — 3. bağımsız tur SKR-029 **survives**: Bulgu A/B gerçekten kapandı; Accepted için ens-ceo hiza incelemesi bekliyor, Madde XIV) |
| ADR-0002 | Operations Capability Pack | draft (M0, SKR-025 + SKR-027 wounded — bağımsız; 2 blocking: ENS-4020) |
| ENS-4020 | Enterprise Ontology (Operations/operax) | review (**M2**, v0.3 — SKR-028+SKR-030 iki bağımsız validator → G4; C.a/C.b düzeltmeleri teyit edildi; ratified ayrı governance edimi) |
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

## Örnek hedef tahsisatlar (henüz yazılmadı)
| id | Planlanan başlık |
|----|------------------|
| ENS-2001 | Decision Theory |
| ENS-2010 | Enterprise Physics |
| ENS-2021 | Company Memory / Organizational Memory |
| ENS-3021 | Decision Entropy |
| RFC-6042 | Reasoning Engine Interface Specification |

> Yeni numara tahsisi: yapıt oluşturulurken bu tabloya satır eklenir. Numara asla
> yeniden kullanılmaz; kaldırılan yapıt `superseded` işaretlenir, numarası korunur.
