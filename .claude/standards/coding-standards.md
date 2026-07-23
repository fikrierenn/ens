# Kod Standartları (Coding Standards)

**Yetki:** [ENS Anayasası, Madde XIII](../../0000-constitution/ENS-0000-constitution.md)
**Geçerli:** Faz 4'ten itibaren.

## ENS kodunun amacı
Reference platform, **teoriyi kanıtlamak** için vardır. Kanıtın netliği,
mikro-optimizasyondan üstündür. Üretim sağlamlaştırması sonraki, kasıtlı bir edimdir —
asla bir bileşenin şeklinin teorinin öngördüğünden sapmasının nedeni değildir.

## Pazarlıksızlar
- Demo kod yok, oyuncak mimari yok, kestirme yok.
- Ölçek varsay: milyonlarca decision, milyarlarca event, binlerce kullanıcı, multi-region,
  kurumsal güvenlik, audit, finansal düzeyde güvenilirlik. Referans yapı küçük çalışsa da
  bunun için tasarla.
- Her kaynak yapıt bir izlenebilirlik yorumu taşır: `// TRACE: ADR-NNNN[, ...]`.
- Black-box çıktı yok: öneri üreten her bileşen açıklama nesnesini de üretir (Why, Why-not,
  Confidence, Evidence, önceki kararlar, Risks, Alternatives).

## Backend (varsayılan teknoloji yığını)
.NET 10 · ASP.NET Core · C# · DDD taktiksel örüntüleri · CQRS · MediatR · Event Sourcing ·
PostgreSQL · Neo4j · Redis · Kafka · OpenTelemetry · Semantic Kernel · Docker · Kubernetes.
- Aggregate'ler invariant'ları uygular; **Decision** aggregate'i event-sourced'tur.
- Command ve query ayrık; read model'ler event'lerden projekte edilir.
- Bounded context'ler paylaşılan tablolarla değil, kontrat ve domain event'lerle entegre olur.
- Observability gömülüdür: OpenTelemetry ile trace, metric, log — ilk günden.

## Frontend (varsayılan teknoloji yığını)
React · Next.js · TypeScript · Tailwind · ShadCN · realtime UI · graph visualization.
- Decision ve açıklaması birinci sınıf UI nesneleridir.
- Kıt kaynağı optimize et: insan dikkati (P5).

## AI mimarisi
Modüler ve model-agnostik. LLM'ler, embedding'ler, GraphRAG, knowledge graph, vector store,
semantic search, planning agent'ları, tool calling, long-term memory, MCP, model
orchestration — hepsi değiştirilebilir adapter'ların ardında. Tek modele kilitleme.

## Test
Mümkün olduğunda determinizm. Unit, contract, integration, BDD. Bkz. `ens-test-engineer` ve
`test-discipline` skill'i. Öneri üreten bir bileşen, yalnızca çıktısı için değil,
**açıklamasının varlığı ve doğruluğu** için de test edilir.

## Tamamlanma tanımı (Faz 4-5)
Bir ADR'ye izli · testleri var · telemetri yayar · öneri yaptığı yerde açıklama nesnesi
yayar · `ens-code-reviewer` ve `ens-style-guardian`'dan geçer · `/validate-theory` yeşil.
