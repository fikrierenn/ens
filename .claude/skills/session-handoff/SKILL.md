---
name: session-handoff
description: ENS oturum sonu özeti yazar. Bugün yapılanları, açık blocking'leri, ROADMAP.md durumunu, yarına başlangıç noktasını journal/YYYY-MM-DD.md dosyasına yazar ve ROADMAP.md'yi günceller. Kullanıcı "kapatabiliriz", "iyi geceler", "/handoff", "devam edeceğiz", "kaydet ve kapat" dediğinde ya da /handoff çalıştırıldığında devreye gir.
allowed-tools: Read, Edit, Write, Bash, Grep, Glob
user-invocable: true
model: inherit
---

# ENS Oturum Devir Skill'i

> Kaynak: `D:\Dev\reporthub`, `D:\Dev\operax` `.claude/skills/session-handoff/SKILL.md`
> (prior art, ENS'e adapte edildi — .claude/standards/context-management.md İlke 3, 6).

## Amaç
Oturum sonunda gün içinde olanları kalıcı journal'a yazar; **ROADMAP.md'yi günceller**
(README/Anayasa'ya session log yazılmaz — bu, LAW-ORG-MEMORY'nin projeye uygulanışıdır).

## Kaynak dosya
`journal/YYYY-MM-DD.md`

- **Yoksa:** `Write` ile oluştur.
- **Varsa:** **`Edit` ile ekle. `Write` KULLANMA.** `pre-compact.sh` gün içinde kısmi
  snapshot bırakmış olur; `Write` onu sessizce yok eder ve dosya untracked ise git'te
  yedeği de yoktur — kayıp kalıcıdır.

> **Bu kural bir kayıptan doğdu (2026-07-26).** Skill'in eski hâli aynı cümlede
> "varsa append" ve "üstüne yaz" diyordu; ikinci talimat izlendi ve 15:20 snapshot'ı
> ezildi. İçerik yalnızca okunmuş olduğu için context'ten geri konabildi — şansla.
> Journal bir **audit kaydıdır** (LAW-ORG-MEMORY); üstüne yazılmaz, eklenir.

## Çıktı şablonu
```markdown
# Oturum Günlüğü — YYYY-MM-DD

## Ana Konu
<1-2 cümle: bu oturumda asıl hedef neydi>

## Tamamlananlar
- Madde (dosya referansıyla, örn. ADR-0001 v0.3, ENS-4020 v0.3)

## Validation Durumu
- Hangi SKR'ler yazıldı, verdict ne (survives/wounded/refuted)
- GOV-000 G2/G4 uyarısı: inline mi bağımsız mı validate edildi

## ROADMAP.md Değişiklikleri
- Kapanan blocking'ler / açılan yeni işler

## Yarım Kalan / Yarına Bırakılan
- Madde — neden yarım, nereden devam

## Kararlar
- Bu oturumda alınan mimari/teori/governance kararları

## Yarına Başlangıç Noktası
1. <en kritik 1. adım — ROADMAP.md "Sıradaki adım" ile tutarlı olmalı>
2. ...
```

## Adım adım
1. **Bilgi topla:** `date +%Y-%m-%d`, `git status --porcelain | wc -l`, `git log --since=midnight --oneline`.
2. **Journal yaz:** yukarıdaki şablona göre doldur.
3. **ROADMAP.md güncelle:** kapanan işleri ✅ işaretle (satır silme yok, EC-001 audit invariant'ı), "Sıradaki adım"ı bu oturumun bulgusuna göre revize et.
4. **Commit (yalnızca journal + ROADMAP + o oturumda üretilen artifact'lar):**
   ```bash
   git add journal/ ROADMAP.md REGISTRY.md
   git commit -m "docs: oturum sonu özeti $(date +%Y-%m-%d)"
   ```

## Kurallar
1. **README/Anayasa'ya log yazma** — statik kimlik dosyalarıdır (İlke 3).
2. **ROADMAP.md her zaman güncellenir** — bu skill'in asıl garantisi budur.
3. **Validation durumunu dürüstçe yaz** — inline mi bağımsız mı, hangi G-madde riski var.
4. **Karar referansları** — büyük kararlar SKR/ADR id'siyle anılır, düz yazıyla tekrar anlatılmaz.

## İlişkili
- `.claude/hooks/session-start.sh` — yarınki oturum bu journal'ı okuyacak
- `.claude/hooks/pre-compact.sh` — compact öncesi kısmi snapshot bırakır
- `.claude/standards/context-management.md` — disiplin kaynağı
- `ROADMAP.md` — süreç katmanının SSOT'u
