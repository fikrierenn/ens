# Plan-First Disiplini — Tier sistemi (ENS-3022'den türetilmiş)

Her ENS oturumuna uygulanır. `paths:` filtresi yok — compact ve clear sonrası da geçerli.

> **Prior art:** `D:\Dev\operax\.claude\rules\plan-first.md`. **Kopyalanmadı, türetildi.**
> operax'ın Tier sinyalleri (3+ klasör, schema, kullanıcı-görünür…) Razor/SP/migration'a
> özgü heuristiklerdir. ENS'in kendi formülü zaten var: `InfoNeed = Stake × (1 − Confidence)`
> (ENS-3022, Howard 1966 VOI). ENS'te Tier, heuristikten değil **bu formülden** okunur.
> Heuristikler aşağıda yalnızca *örnek* olarak durur, **ölçüt olarak değil**.

## Temel kural

**Tier 3 işlerde plan ZORUNLU.** Plan onaylanmadan yapıt üretilmez, plan referansı olmadan
Tier 3 commit atılmaz.

## Tier eşikleri

| Tier | ENS-3022 karşılığı | Plan? |
|---|---|---|
| **1 — Trivial** | Düşük `Stake`, yüksek `Confidence`, geri alınabilir | **YOK** |
| **2 — Standard** | Orta `InfoNeed`, mevcut pattern, geri alınabilir | **TODO satırı** (TaskCreate) |
| **3 — Substantial** | Yüksek `InfoNeed` **VEYA** `IsIrreversible` | **TAM PLAN** (`plans/NN-<slug>.md`) |

`IsIrreversible` **tek başına** Tier 3 yapar — InfoNeed düşük olsa bile. Gerekçe: VOI
"ne kadar öğrenmeliyim" sorusunu ölçer; geri alınamazlık "yanlışsam ne olur" sorusudur.
İkisi farklı eksendir (ADR-0001 §5.6, P7).

## Öz-beyan kaçağı — KAPALI

`Confidence` **öz-beyandır**. `Stake` ve `IsIrreversible` gözlemlenebilirdir.

> **Kural:** Yüksek `Confidence` beyanı tek başına Tier **düşüremez**. Taban her zaman
> `Stake` ve `IsIrreversible`'dan okunur.

**Neden bu kural var:** kernel'de bunun tam karşılığı bir kusur olarak bulundu —
`AUDIT_DEFECT_W8d` (`AdversarialWave_SecurityTests.cs:958`): `confidence = 1.0` tek başına
InfoNeed'i, AttentionPriority'yi, tier'ı **ve** gate'i birlikte sıfırlıyor. `stake = 1e12`
olsa bile. Yani "eminim" demek, tüm protokolden muafiyet satın alıyor. Aynı kaçağın insan
katmanındaki hâli: *"bunu biliyorum, plana gerek yok."*

Bkz. `DEFECT-REGISTER.md` 8. kalıp — *"öz-beyan kalibre edilmemiş."*

## Tier 3 sinyalleri (heuristik — ölçüt değil, hatırlatıcı)

Şunlardan biri varsa **Stake yüksek ya da Confidence düşük demektir**; formüle dön:

1. **Anayasa / GOV-\* dosyasına dokunma** → tanım gereği yüksek Stake (Madde XV)
2. **Yeni ENS-NNNN yapıtı** → REGISTRY numarası harcar, geri alınamaz (bkz. `footprint-ladder.md` 5. basamak)
3. **Ratified bir yapıtın `status`'ünü değiştirme** → SKR zinciri açılır
4. **Kernel'de davranış değişikliği** (yalnız yorum/test değil)
5. **Üç ya da daha fazla katmana dokunma** (2000-theory + 5000-architecture + 7000-reference…)
6. **Yeni bağımlılık yönü** — katmanlar arası ok eklemek (ADR gerektirir)

Şüphede kal? **Sahibine sor:** "Bu Tier 2 mi Tier 3 mü, plan yazayım mı?"

## Workflow

1. **Tier tespiti** — yukarıdaki tablo.
2. **Plan yaz** (Tier 3): `plans/feature-template.md`'yi kopyala → `plans/NN-<slug>.md`.
   En az **2 reddedilen alternatif** ve **5 lens** zorunlu.
3. **Onay** — sahibine göster. **Onay olmadan üretme.**
4. **Uygula** — commit mesajında plan referansı: `feat(kernel): X (plan: 01)`.
5. **Kontrol ettir** — `work-protocol.md` adım 3. ENS'te bu **SKR zinciridir**, yeniden kurulmaz.
6. **Kapat** — `git mv plans/NN-*.md plans/archive/`, done criteria işaretlenir, journal'a özet.

## Stale plan

**14 gün dokunulmamış aktif plan** ya yeniden ısıtılır ya arşivlenir. Üç yoldan biri:
geçerli+başlıyor → bu oturumda ilk adım; geçerli ama uzak → arşiv ("henüz sırası değil");
geçersiz → arşiv (**gerekçeyle**). Plan dosyası **silinmez** — EC-001 audit invariant'ı
planlara da uygulanır.

## NE ZAMAN UYGULANMAZ

- **Tier 1 trivial** — typo, künye alanı, bozuk link, format düzeltmesi.
- **Acil düzeltme:** yayınlanmış bir yapıtta yanlış/yanıltıcı iddia bulunduğunda plan
  beklenmez — Madde X düzeltmeyi geciktirmeyi yasaklar. Ama: (1) sahibine "bypass
  yapıyorum" denir, (2) commit `(plan: BYPASS-<tarih>)` taşır, (3) sonradan retro plan
  `plans/archive/BYPASS-<tarih>.md` yazılır.
- **Sahibi "hızlıca yap" derse:** Tier 3 sinyali varsa yine uyarılır ("bu 3 katmana
  dokunuyor, mini-plan yazayım mı?"). "Direkt" derse görev kaydına
  `[plan-skipped: <gerekçe>]` düşülür — sessizce atlanmaz.

## İlişkili
- `plans/feature-template.md` — şablon
- `.claude/rules/footprint-ladder.md` — en dar basamak (5-6. basamak = Tier 3)
- `.claude/rules/work-protocol.md` — Danış → Yap → SKR → Kanıtla
- `ENS-3022` (Decision Gravity) — Tier eşiklerinin kaynağı
- `ROADMAP.md` — süreç katmanının SSOT'u
