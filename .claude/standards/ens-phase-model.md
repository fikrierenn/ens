# ENS Faz Modeli

**Yetki:** [ENS Anayasası, Madde VII](../../0000-constitution/ENS-0000-constitution.md)
**Bağlar:** Tüm agent'lar, tüm komutlar, tüm katkılar.

ENS bir yazılım sprint'i değil, bir bilim disiplini olarak inşa edilir. İş, yedi kapılı
fazdan akar. Bir kapı ancak çıkış ölçütleri kaydedilince açılır. Erken bir fazda bulunan
bir kusur, yukarı akışta düzeltilene dek bağımlı sonraki işi **durdurur** — teori, önceden
yazılmış koda uysun diye asla yamalanmaz.

| Faz | Ad | Çıktı dizini | Sahip agent'lar |
|-----|----|--------------|-----------------|
| 0 | Felsefe | `0000-constitution/`, `1000-philosophy/` | ens-philosopher, ens-ceo |
| 1 | Teori | `2000-theory/` | ens-philosopher, ens-researcher, ens-skeptic |
| 2 | Bilim | `3000-laws/`, `4000-ontology/` | ens-philosopher, ens-researcher, ens-skeptic, ens-chief-architect |
| 3 | Mimari | `5000-architecture/`, `6000-rfc/` | ens-chief-architect, ens-domain-modeler, ens-rfc-writer |
| 4 | Reference Platform | `7000-reference-implementation/` | ens-backend-architect, ens-ai-architect, engine agent'ları |
| 5 | Ürün | `8000-product/` | tüm implementation agent'ları |
| 6 | Kitap | `9000-book/` | ens-book-author, ens-philosopher |

## Faz 0 — Felsefe
**Üret:** Manifesto, Ana Tez, First Principles, ilk terminoloji, Anayasa.
**Çıkış koşulu:** Anayasa onaylı; sözlük tohumlanmış; anti-pattern'ler adlandırılmış.

## Faz 1 — Teori (en önemli)
**Üret:** Her kavram için — definition, motivation, historical context, theoretical model,
implications, relationships, examples ve o kavramın laws'ı. Kavramlar: Enterprise Physics,
Decision Theory, Organizational Memory, Company Consciousness, Decision Capital, Decision
Entropy, Decision Velocity, Decision Energy, Context Theory, Trust Theory, Enterprise
Intelligence.
**Çıkış koşulu:** Her kavram belgesi skeptic-incelenmiş ve kendi failure conditions'ını
belirtiyor (Anayasa Madde X); çözülmemiş terminoloji çatışması yok.

## Faz 2 — Bilim
**Üret:** ENS Laws, uygun yerlerde matematiksel modeller, Decision Lifecycle, Enterprise
Cognitive Model, Reasoning Model, Learning Model, Memory Model.
**Çıkış koşulu:** Her model ifade edilebilir, yanlışlanabilir ve Faz 1 kavramlarına izli.

## Faz 3 — Mimari (türetilmiş, uydurma değil)
**Üret:** C4 diyagramları, ADR'ler, Bounded Context'ler, Context Maps, Event Model,
Decision Graph, Memory Graph, Service Boundaries — her ADR gerçekleştirdiği teoriye atıflı.
**Çıkış koşulu:** Teori atfı olmayan ADR yok; `/validate-theory` geçer.

## Faz 4 — Reference Platform
**Üret:** Teoriyi **kanıtlayan** referans implementation. Doğruluk ve netlik, üretim
optimizasyonundan önce gelir. Her yapıt bir ADR'ye atıflı.
**Çıkış koşulu:** Kanıt çalışıyor; `kod → ADR → theory` izlenebilirlik zinciri tam.

## Faz 5 — Ürün
**Üret:** Mimariden beliren modüller — Decision Engine, Reasoning Engine, Simulation
Engine, Company Memory, Knowledge Graph, Enterprise Search, Learning Engine, Agent
Framework.
**Çıkış koşulu:** Her modül bir bounded context'e ve teorisine izli.

## Faz 6 — Kitap
**Üret:** Disiplini tanımlayan referans eser. Dokümantasyon değil.
**Çıkış koşulu:** Kitap, kodu hiç görmemiş bir okur için kendi başına ayakta durur.

## Kapı kuralı
Herhangi bir kapıyı geçmeden önce `/validate-theory` çalıştır. Tamamlanan fazdaki bir
yapıt yukarı akış izlenebilirliğinden yoksunsa ya da (Faz 1-2) failure conditions
belirtmiyorsa, kapı kapalı kalır.
