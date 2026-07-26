# ENS Agent Roster

**Yetki:** [ENS Anayasası, Madde XII](../../0000-constitution/ENS-0000-constitution.md)

Agent'lar bağımlılık grafiğinin en altındadır: **felsefeyi tüketirler, üretmezler.**
Değiştirilebilirler; Anayasa değildir. Roster, tam ekibi (kullanıcının 20-rollük tasarımı)
ve her rolün hangi fazda devreye girdiğini belgeler. **JIT ilkesi:** bir agent, ancak fazı
geldiğinde gerçek dosya olur; erken üretim boş dosya kalabalığıdır.

## Aktif (dosya var)
| Agent | Faz | Görev | Yazma alanı |
|-------|-----|-------|-------------|
| `ens-philosopher` | 0-2, 6 | Teori/Külliyat yazar ve korur | Külliyat, kitap |
| `ens-skeptic` | 0-2 | Teoriye saldırır (SKR) | reviews/ |
| `ens-researcher` | 0-2, 6 | Literatür/prior art/kanıt | research/ |
| `ens-style-guardian` | 0+ | Terminoloji/künye/traceability tutarlılığı | biçimsel düzeltmeler |
| `ens-architect` | 3+ (freeze şimdi) | Katman/bağımlılık/roadmap/refactor/hedef mimari | 5000-architecture/ |
| `ens-ai-architect` | 3-4 | Cognitive Kernel, açık kaynak LLM, agent runtime, bounded autonomy | 5000/7000 |
| `ens-backend-architect` | 4-5 | .NET, CQRS, DDD, Event Sourcing, MediatR, Clean Architecture | 7000/ |
| `ens-test-engineer` | 4-5 | Unit/contract/integration/BDD, test stratejisi | 7000/*.Tests |
| `ens-memory-engine` | 4-5 (erken materyalize, 2026-07-24 — K5 aktif iş) | Company Memory/Context Score Faz-4 kodu, confidence/decay, K5 dış-repo değer-değerlendirmesi | 7000/Domain, K5 araştırma |
| `ens-test-runner` | 4+ (2026-07-26) | `dotnet test`'i **gerçekten** çalıştırır; AUDIT envanterini makineyle üretip DEFECT-REGISTER ile karşılaştırır. `Edit`/`Write` **yok** — ölçen düzeltemez | 7000/*.Tests |
| `ens-silent-failure-hunter` | 4+ (2026-07-26) | Sessiz başarısızlık avı: fail-open sayısal kapı, eşik=0 kapatma anahtarı, yutulan event, izsiz geçiş, çıktı kapısı yok, kalibre edilmemiş öz-beyan. `Edit`/`Write` **yok** | 7000/Ens.Kernel |

> **2026-07-26 notu — neden bu iki ajan var:** `ens-test-runner`, denetim ajanlarının
> kronik *"`dotnet test` çalıştıramadım"* boşluğunu kapatır (SKR-041, SKR-045,
> AUDIT-WAVE2-SECURITY, DEFECT-REGISTER-VERIFICATION — dördü de bunu yazmak zorunda kaldı).
> `ens-silent-failure-hunter`, 75 kusurun büyük kısmının tek bir aileden geldiği
> bulgusundan doğdu: sistem yanlış davranıyor ve kimseye söylemiyor.

## Ertelenmiş (fazı gelince üretilecek)
| Agent | Faz | Görev |
|-------|-----|-------|
| `ens-ceo` | tüm | Uzun vadeli hiza / teknik borç kararı (önemli kararlarda) |
| `ens-chief-architect` | 3 | Genel mimari, bounded context, C4, ADR, tradeoff |
| `ens-domain-modeler` | 3-4 | DDD: aggregate, value object, domain event, ubiquitous language |
| `ens-rfc-writer` | 3+ | RFC / ADR / design proposal biçimlendirme |
| `ens-reasoning-engine` | 5 | Reasoning pipeline, hypothesis, confidence, explainability |
| `ens-simulation-engine` | 5 | What-if, Monte Carlo, risk, senaryo |
| `ens-frontend` | 5 | React/Next.js/Tailwind, decision & graph görselleştirme |
| `ens-devops` | 4-5 | Docker, Kubernetes, Terraform, CI/CD, observability |
| `ens-security` | 4-5 | RBAC/ABAC, zero trust, audit, encryption, compliance |
| `ens-performance` | 4-5 | Caching, latency, scalability, benchmark |
| `ens-code-reviewer` | 4-5 | Her PR: mimari, naming, DDD, güvenlik, clean code |
| `ens-product-manager` | 5 | Roadmap, epic, feature, MVP, release |
| `ens-book-author` | 6 | Kitap; akademik dil, Mermaid, kaynak (kod yazmaz) |

## Kural
Ertelenmiş bir agent'ı üretmeden önce fazının açık olduğunu doğrula. Her agent, Anayasa'ya
ve ilgili standartlara (`.claude/standards/`) atıf yapar; hiçbiri felsefe üretmez.
