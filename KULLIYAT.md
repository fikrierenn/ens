# ENS Külliyat Manifesti

**Yetki:** [ENS Anayasası, Madde IV](0000-constitution/ENS-0000-constitution.md) ·
[maturity-model.md](.claude/standards/maturity-model.md)

**Külliyat**, ENS'in değiştirilemez, **teknolojiden bağımsız** çekirdeğidir. Ama olgunluk
modeli (M0-M5) sonrası **"Külliyat"ın iki anlamı** ayrıştı — bu ayrım [RFC-6001](6000-rfc/RFC-6001-constitutive-artifact-ayrimi.md)
ile Anayasa'ya (Madde IV) ve künye şemasına resmî işlendi (**gap #1 kapandı**, 2026-07-25 korpus
retrofit'i — ROADMAP.md G-19):

- **`constitutive: true` çekirdek** (RFC-6001 §4): yapıt normatif/kurucu — kural, tanım ya da
  tip-şeması, tanımla yürürlükte; canon'unu **ratifikasyonla** (kurucu-tutarlılık skeptic
  incelemesi) kazanır, M5/Faz-4 kanıt zincirine tabi değildir. Turnusol testiyle (§4.2)
  belirlenir — aralık (0/1/3/4) tek başına belirlemez.
- **`constitutive: false` — Teorik/ampirik Canon (M5)**: yanlışlanabilir ampirik iddia taşıyan
  teori/yasa/ontoloji kavramları, **kanıt zinciriyle** (GOV-030) M5 kazanır. **M4+ Faz 4
  gerektirdiğinden tam Canon (M5) kümesi şu an BOŞtur**; `constitutive: false` yapıtlar bugün
  M2/M3'te (`ratified`, canon'a henüz girmemiş) durur.

> Not: bu iki eksen v0.2'ye dek `canon:true`'da karışıyordu; olgunluk modeli onları ayırdı ama
> üst-kaynağa (Anayasa) hiç resmî işlenmemişti. RFC-6001 (Accepted, 3 bağımsız skeptic turu +
> çift-owner kabul kapısı) bu borcu kapattı; §8.1/§8.2 turnusol tablosu korpus geneline
> mekanik olarak işlendi.

## Normatif/Constitutive çekirdek (`constitutive: true`, doğası gereği teori değil)
| id | Başlık | Statü |
|----|--------|-------|
| ENS-0000 | ENS Anayasası | ratified (supreme, immutable-core: Madde III) |
| ENS-1000 | ENS Manifestosu | ratified |
| ENS-4000 | Sözlük (Glossary) | review (canon-borcu açık — RFC-6001 §8.3, ayrı edim) |
| ENS-3000 | Enterprise Laws (kayıt) | draft (2026-07-27: kazanılmamış `canon:true` geri alındı — sıfır doğrulama turu; SCAN-03 A-02a, ROADMAP G-24) |
| ENS-4030 | Semantic Axioms | ratified (M2 — olumsal/uygulama-izleme etiketi, canon-gate etmez) |
| ENS-4031 | Inference Rules | skeptic-cleared (M0 — olumsal/uygulama-izleme etiketi, canon-gate etmez) |
| GOV-000..030 | Governance Core | review (M1 — olumsal/uygulama-izleme etiketi, canon-gate etmez) |

> Not (ENS-4001 taşındı): önceki sürüm ENS-4001'i (Meta Model) bu tabloda listeliyordu; RFC-6001
> §8.1 turnusolu onu `constitutive: false` (sentetik-yeterlilik iddiası taşıyan tip sistemi)
> olarak sınıflar — aşağıdaki M2 tabloya taşındı. Hiçbir grade sökülmedi (M2 korunur).

## M2 Ontoloji/tip-sistemi (`constitutive: false`, sentetik-yeterlilik iddiasıyla sınanır)
| id | Başlık | maturity | Statü |
|----|--------|----------|-------|
| ENS-4001 | ENS Meta Model | M2 | review |
| ENS-4010 | Foundational Ontology | M2 | skeptic-cleared |
| ENS-4020 | Enterprise Ontology (operax) | M2 | review |
| ENS-4025 | Semantic Logic | M2 | ratified |

## Teorik Canon (M5 — kanıt zinciriyle kazanılır)
**BOŞ.** M4 Reference (Faz 4) ve dört-skeptic + governance onayı olmadan hiçbir kavram M5
olamaz (GOV-030). Bu, dürüst ve doğru durumdur: *Canon, kod teoriyi kanıtlayınca dolar (P8).*

## M3 Stable çekirdek (Canonical adayı — Faz 4 bekliyor)
| id | Başlık | maturity | Scientific evidence |
|----|--------|----------|---------------------|
| ENS-2001 | Decision Theory | M3 | frame E3, delta E1 |
| ENS-2002 | Context Theory | M3 | frame E3, delta E1 |
| ENS-2003 | Company Memory | M3 | frame E3, delta E1 |
| ENS-2004 | Learning Theory | M3 | frame E3, delta E1 |
| ENS-3021 | Decision Entropy | M3 | frame E3, delta E1 |
| ENS-3022 | Decision Gravity | M3 | frame E3, delta E1 |
| ENS-3023 | Decision Capital | M3 | frame E3, delta E1 |

> M3 → M4 için reference platform (Faz 4) + Engineering Validation; M4 → M5 için GOV-030 zinciri.
