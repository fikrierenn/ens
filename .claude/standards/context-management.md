---
id:            STD-CONTEXT-MANAGEMENT
title:         Bağlam Yönetimi (Context Management)
type:          standard
canon:         false
origin:        ENS-0000 §X
depends_on:    [ENS-0000]
principles:    []
status:        ratified
owner:         ens-style-guardian
version:       0.1.0
last_reviewed: 2026-07-24
---

# Bağlam Yönetimi (Context Management)

**Yetki:** [ENS Anayasası, Madde X](../../0000-constitution/ENS-0000-constitution.md) (dürüstlük/
yanlışlanabilirlik ilkesinin operasyonel uzantısı) + bu standardın kendisi bir uygulamalı kanıttır:
LAW-ORG-MEMORY'nin ("unutulan kararlar tekrarlanan hatalara dönüşür") **projeye uygulanışı.**

## Prior art (5-başlık disiplini)
- **Kaynak:** `D:\Dev\reporthub\docs\CONTEXT_MANAGEMENT.md` (22 Nisan 2026) ve
  `D:\Dev\operax\docs\CONTEXT_MANAGEMENT.md` (26 Mayıs 2026) — **iki bağımsız proje, aynı
  disiplinde birleşmiş** ("2 paralel araştırma agent'ı + Claude Code resmi dokümanları
  sentezi"). Onların da kaynağı: Claude Code resmi dokümanları (memory/sub-agents/skills/hooks/
  context-window), Addy Osmani (Spec→Plan→Save-point), Geoffrey Huntley "Ralph" pattern'i
  (primary=scheduler), Armin Ronacher (Agentic Coding Recommendations).
- **Delta:** ENS bu disiplini icat etmiyor; **kendi mevcut yapısına eşliyor** (aşağıda) ve
  ENS'in kendi teorisine (LAW-ORG-MEMORY, ENS-2002 Context Theory'nin `staleness`/`noise_penalty`
  kavramları) bağlıyor — context çöküşü, LAW-CONTEXT'in ("context azaldıkça/gürültü arttıkça
  kalite düşer") oturum-düzeyinde somut bir örneğidir.

## İlke 1 — Üç katman ayrımı (ENS'te zaten kısmen var, burada resmîleşiyor)
| Katman | ENS'te karşılığı | Ne yazılır |
|--------|-------------------|------------|
| **Kimlik** | `README.md`, Anayasa | Proje tanımı, değişmez ilkeler, okuma sırası |
| **Kurallar** | `.claude/standards/*.md` | Davranış/format kuralları, konuya göre bölünmüş |
| **Süreç** | `ROADMAP.md`, `REGISTRY.md`, `*/reviews/SKR-*.md` | Açık işler, kayıtlar, geçmiş kararlar |

**Kural:** aynı bilgi iki yerde durmaz. "Her oturumda mı gerekli?" → Kimlik. "Belli konuda iş
yapılırken mi?" → Kurallar. "Tarihli/geçici/karar-kaydı mı?" → Süreç.

## İlke 2 — 200 satır eşiği
`.claude/standards/*.md` her biri 200 satır altında kalmalı; aşarsa alt-konuya bölünür.
**Uygulama (şimdi):** Anayasa (`0000-constitution/ENS-0000-constitution.md`) bu eşiğin **çok
üzerinde** — bu, ROADMAP.md'deki **G-06** ("Constitution çok uzun, 6-8 sayfa hedefi") borcunun
somut ölçütüdür; bağımsız olarak aynı teşhise varan iki dış kaynak bunu doğruluyor.

## İlke 3 — Süreç kaydı, Kimlik dosyasında yaşamaz
"Bu oturumda ne oldu" README/Anayasa'ya yazılmaz. ENS'in karşılığı: **ROADMAP.md** (açık işler)
+ `reviews/SKR-*.md` (karar kaydı, asla silinmez — EC-001 audit invariant'ı ile tutarlı) +
auto-memory (`ens-project.md`, makine-yerel, oturumlar-arası pointer).

## İlke 4 — Eşik sinyalleri (adapte edildi)
| Eşik | Kaynak | ENS karşılığı |
|------|--------|----------------|
| 15+ uncommitted dosya | reporthub/operax | Commit-split öncesi yeni iş başlatma |
| **3+ paralel iş** | reporthub/operax | Aynı anda 3'ten fazla açık ADR/validation/freeze-fix hattı olmaz |
| Aynı hata 2. kez | reporthub/operax | Standard/ROADMAP'a yaz, tekrar sözlü söyleme |
| `/compact` sonrası kural unutuldu | reporthub/operax | Kritik kural `path:`-scoped değil, kök standard'da |

**Uygulama (şimdi):** K1-K5 (kernel hattı) + freeze-fix backlog (G-02...G-16) birlikte **5+ paralel
hat** — eşiği aşıyor. ROADMAP.md'nin "Sıradaki adım" bölümü bunun için var: her oturum **tek
öncelik** seçer, diğerleri `pending` kalır.

## İlke 5 — Spec → Plan → Execute (ENS'in zaten uyguladığı, mikro-düzeyde formalize)
ENS'in kendi Faz Modeli (Teori→Mimari→Implementation) bunun makro biçimi. Mikro biçimi: 3'ten
fazla dosyayı etkileyecek her değişiklik önce ROADMAP'a bir satır/plan olarak yazılır, sonra
uygulanır. "Hızlıca şunu yap" → doğrudan çok-dosyalı değişiklik, scope patlamasına yol açar.

## İlke 6 — Karar kalıcılığı (zaten uygulanıyor, açıkça bağlanıyor)
Her "böyle yapalım" konuşma-mutabakatı kalıcı yer bulur: büyük karar → ADR/RFC/SKR; küçük
konvansiyon → `.claude/standards/`; açık iş → `ROADMAP.md`. "Konuşmada kaldı" = kayıp (LAW-ORG-MEMORY).

## İlke 7 — Ralph pattern (agent orkestrasyonu)
**Primary context = scheduler.** Büyük/bağımsızlık-gerektiren iş (validation, derin repo analizi)
subagent'a devredilir, primary'nin context'i şişmez; agent'ı yazan zihin onu validate etmez
(G2/G3, bu projede bir kez ihlal edilip düzeltildi — bkz. SKR-024→026, SKR-025→027 dersi).

| İş türü | Primary mi, subagent mi? |
|---------|---------------------------|
| Dosya okuma, grep, küçük edit | Primary |
| Bağımsız validation (Scientific/Ontology/Engineering) | **Her zaman subagent** (G2/G3 zorunlu) |
| Derin dış-repo analizi (CrewOps/operax gibi) | Subagent |
| Hızlı künye/REGISTRY güncellemesi | Primary |

## Oturum protokolü (gerçek mekanizma — kurulu)
`.claude/hooks/session-start.sh` + `.claude/hooks/pre-compact.sh` + `.claude/skills/session-handoff/`
artık **çalışan** altyapı (reporthub/operax'tan adapte, prose değil).

**Oturum başı:** SessionStart hook otomatik çalışır (ROADMAP + son SKR + son journal + git durumu).
**"Pusula dersi" (operax'tan, iki gerçek hataya yol açtığı için kritik):** hook çıktısı context'te
görünüyor olsa bile, oturum gerçekten başlarken **tekrar** çalıştırılmalı sayılır — "gördüm,
atlarım" varsayımı yasak. Context stale olabilir; cevap her zaman fresh okumaya dayanır.

**Oturum ortası:** `/compact` %60 doluluk sonrası + PreCompact hook otomatik snapshot bırakır
(`journal/YYYY-MM-DD.md`). Yeni karar → doğrudan ilgili `.claude/standards/` veya `ROADMAP.md`'ye.

**Oturum sonu:** `/handoff` (session-handoff skill) → `journal/` yazar + `ROADMAP.md` günceller +
yalnızca journal/ROADMAP/REGISTRY commit'lenir.

## Failure conditions (Anayasa Madde X)
- **Retrofit maliyeti.** Mevcut uzun dosyalar (Anayasa) geriye dönük bölünmezse eşik yalnızca
  yeni yazıya uygulanır — G-06 borcu kapanmadan bu standart kısmen semboliktir.
- **Auto-memory ≠ repo kaydı.** Auto-memory makine-yerel; ROADMAP.md/SKR'ler git'te olmalı —
  tek gerçek kaynak repodur, memory yalnızca "nereye bak" işaretçisidir.
- **Eşik keyfiliği.** 200 satır / 15 dosya / 3 paralel-iş sayıları reporthub/operax'ın kendi
  deneyiminden; ENS'in ölçeğinde farklı olabilir — ampirik olarak kalibre edilmeli (Faz 4).

---

*Context çöküşü, LAW-CONTEXT'in insan-AI işbirliği düzeyinde bir tezahürüdür: gürültü (session
detayı Kimlik dosyasında) ve bayatlık (konuşmada kalan karar) kaliteyi düşürür. Bu standart,
ENS'in kendi teorisini kendi üretim sürecine uygular.*
