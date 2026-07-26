# Danışma Kuralı — üretimden ÖNCE danış

> **Prior art:** operax `advisor-skills.md`. ENS'te danışman **skill** değil çoğunlukla
> **ajan**dır — çünkü ENS'in ajan kadrosu zaten rol-ayrımlıdır (Madde XIV) ve G2/G3
> bağımsız context zorunluluğu getirir.

## Temel ilke

> **"Doğru görünüyor" yetmez.** İş aşağıdaki bir alana giriyorsa, **üretmeden önce**
> eşleşen danışmana danışılır. Danışılmadan yazılan teori/mimari/ontoloji işi **eksik
> kabul edilir**.

Danışman **salt-rehberdir**: kendisi nihai yapıtı yazmaz, doğrulanacak noktaları ve
kaynakları verir. Kararı sen verirsin — ama **gerekçeyle**.

## Katalog (iş türü → danışman, üretim ÖNCESİ)

| İş türü / tetik | Danışman | Not |
|---|---|---|
| Teori kavramı, Külliyat metni, terminoloji, yasa/ontoloji yazımı | **`ens-philosopher`** | Asla kod yazmaz |
| Bir kavramın yeniliği / prior art / literatür dayanağı sorgulanıyor | **`ens-researcher`** | Gerçek, bulunabilir kaynak; **kaynak uydurmaz** |
| Katman, bağımlılık yönü, faz kapısı, refactor, mimari donma | **`ens-architect`** | Teori ya da kod yazmaz; mimariyi türetir |
| Kernel, LLM entegrasyonu, agent runtime, planning, tool-calling, bounded autonomy | **`ens-ai-architect`** | Prior-art dürüstlüğü zorunlu (AIOS, MemGPT/Letta, LangGraph) |
| Memory / Context Score / decay / staleness / continuous learning | **`ens-memory-engine`** | ENS-2002/2003 hattı |
| .NET kodu, event sourcing, CQRS, Faz-4 implementasyon | **`ens-backend-architect`** | Yalnız Accepted ADR'lere dayanır (Madde VII) |
| Test yazımı, invariant testi, proof-trace testi | **`ens-test-engineer`** | Her test hangi iddiayı doğruluyor izler |
| Künye, numaralandırma, terminoloji tutarlılığı, dosya adlandırma | **`ens-style-guardian`** | Anlam değiştirmez |
| Geniş arama: "bu kavram repoda nerede geçiyor" | **`Explore`** | Salt-okuma, sonuç döner |
| Yüksek InfoNeed karar: mimari yön, hangi seçenek, borç kapatılsın mı taşınsın mı | **`karar-konseyi`** (skill) | Bağımsız panel + karşılıklı eleştiri; `ens-skeptic` her konseyde zorunlu |
| Yeni skill/agent/hook/rule üretimi | **`yetenek-uret`** (skill) | Footprint-ladder'ı zorunlu kılar |
| Uygulama stratejisi / adım planı tasarımı | **`Plan`** | Tier 3 planın §6 fazları için |

> **`ens-ceo` ROSTER'da planlıdır ama ajan dosyası YOKTUR.** CEO-\* hizalama incelemeleri
> (CEO-0001, CEO-0003) elle yazılmıştır. Bu satır, katalogda uydurma ajan bulunmadığını
> göstermek için burada duruyor — materyalize edilirse katalog güncellenir.

## Denetim (üretimden SONRA — ayrı rol, ayrı kural)

| Amaç | Rol |
|---|---|
| Teori/felsefe yapıtına saldırı, SKR kaydı | **`ens-skeptic`** |
| Koda adversarial saldırı | `.claude/skills/adversarial-test/` |
| Sessiz başarısızlık avı (fail-open, eşik=0, yutulan event, izsiz geçiş, çıktı kapısı yok) | **`ens-silent-failure-hunter`** |
| Testlerin **gerçekten** çalıştırılması + AUDIT envanteri | **`ens-test-runner`** |

> `ens-test-runner` ve `ens-silent-failure-hunter`'ın **`Edit`/`Write` aracı yoktur** —
> bilerek. Ölçen, ölçtüğünü düzeltemez (G2/G3).

Bu ikisi **danışman değildir** — `work-protocol.md` adım 3'e aittir. Danışman ile denetleyen
**aynı context olamaz** (Madde G2/G3: yazan doğrulayamaz).

## Kural

1. İş bir danışman alanına giriyorsa → **önce danışman, sonra yapıt.**
2. Danışman yoksa ve tekrarlayan ihtiyaçsa → `footprint-ladder.md` ile en dar basamakta üret.
3. Danışman çıktısı **rehberdir, dayatmaz.** Karar senin — gerekçen kayda geçer.
4. Yeni danışman ajan eklenince **bu kataloğa satır ekle** ve ROSTER'ı güncelle.
5. Danışmanın "bilmiyorum / doğrulayamadım" demesi geçerli çıktıdır ve **kaydedilir**.
   Uydurulmuş kesinlik, kaydedilmiş belirsizlikten kötüdür (Madde X).

## NE ZAMAN UYGULANMAZ

- **Tier 1 trivial** işler — künye alanı, typo, bozuk link.
- **Zaten danışılmış ve kararı verilmiş** konuda tekrar üretim — plan referansı yeterli.
- **Danışman kendi alanının yapıtını yazıyorsa** kendine danışmaz (ör. `ens-philosopher`
  teori yazarken). Bu durumda kapı üretim *sonrasındadır*: `ens-skeptic` turu.
- Acil düzeltme (yanlış/yanıltıcı yayınlanmış iddia) — Madde X gecikmeyi yasaklar.

## İlişkili
- `.claude/rules/work-protocol.md` — 4 adımlı döngü (bu katalog = adım 1)
- `.claude/rules/footprint-ladder.md` — yeni danışman üretme basamağı
- `.claude/agents/ROSTER.md` — kadro
- Anayasa Madde XIV — rol ayrımı; G2/G3 — yazan ≠ doğrulayan
