# Dil Politikası (Language Policy)

**Yetki:** [ENS Anayasası, Madde XIII](../../0000-constitution/ENS-0000-constitution.md)
**Bağlar:** Tüm agent'lar, komutlar ve katkılar.

## Kural
ENS deposundaki tüm **dokümanlar Türkçe** yazılır. Açıklayıcı düz yazı, başlıklar,
gerekçeler ve anlatı Türkçe olur.

## Orijinal (çevrilmeden) kalanlar
- **Teknik terimler:** Decision, Context, Memory, Reasoning Engine, Bounded Context,
  Event Sourcing, CQRS, DDD, Aggregate, Value Object, Domain Event, Knowledge Graph,
  GraphRAG, Vector Database, Semantic Search, Tool Calling, MCP vb.
- **ENS canonical sözlüğü:** Decision Capital, Decision Velocity, Decision Energy,
  Decision Entropy, Decision Gravity, Decision Surface, Decision Friction, Context Score,
  Trust Coefficient, Company Memory, Enterprise Physics, Organizational Consciousness vb.
  (Bunlar `4000-ontology/` sözlüğünde tanımlı kanonik adlardır; adres bütünlüğü için
  orijinal biçimleriyle kullanılır.)
- **Kimlikler:** `ENS-0000`, `P1`..`P8`, `ADR-NNNN`, `RFC-6xxx`, `MOD-*`, `LAW-*`, `SKR-*`.
- **Kod, komut adları, dosya/yol adları, teknoloji ve araç isimleri.**
- **Metadata header YAML anahtarları** (`id`, `type`, `depends_on`, `status` ...) İngilizce;
  bu anahtarların **düz yazı değerleri** (örn. `title`, `origin` açıklaması) Türkçe.

## Gerekçe
Kitap ve teori, Türkçe bir referans eseri olacak. Ancak teknik terimlerin çevrilmesi
terminoloji sürüklenmesi (terminology drift) yaratır ve `ENS-3021 = Decision Entropy`
gibi adresleme bütünlüğünü bozar (Anayasa Madde IV, VI). Bu yüzden kanonik terimler
korunur, anlatı Türkçeleşir.

## İlk terim kullanımı
Bir teknik terim bir dokümanda ilk geçtiğinde, parantez içinde kısa Türkçe açıklaması
verilebilir: örn. "Decision Entropy (kararların büyümeyle tutarlılık kaybı)". Terimin
kendisi orijinal kalır.
