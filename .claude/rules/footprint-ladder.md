# Footprint Ladder — yeni yetenek en dar basamakta

> **Prior art:** operax `footprint-ladder.md` (o da pusula'dan, Hermes "narrow waist").
> ENS'e uyarlandı: ENS'in en pahalı basamağı yeni Razor sayfası değil, **yeni numaralı
> yapıttır** — REGISTRY numarası harcar, Külliyat'a girer ve EC-001 gereği silinmez.

## Temel ilke

**Çekirdek dar bel; yetenek kenarda.** Her yeni kalıcı yapı bakım yükü + bağlam maliyeti
getirir. Bir ihtiyaç çıktığında merdivenin **en alt** basamağında çöz; üste ancak alt
basamak yetmezse çık.

## Merdiven (alttan üste — alt = dar/ucuz)

| # | Basamak | Ne zaman | Maliyet |
|---|---|---|---|
| 1 | **Mevcut yapıtı genişlet** | Var olan ENS-NNNN'e bölüm/satır/alan eklemek çözüyorsa | ~0 yeni yüzey |
| 2 | **Yeni skill** | Tekrarlanan iş akışı; tetik-bazlı yüklenir | Düşük — sadece tetiklenince |
| 3 | **Yeni rule** | Kalıcı davranış kuralı (her oturum geçerli) | Orta — her oturum bağlamda |
| 4 | **Yeni agent** | Özelleşmiş, bağımsız-context rol (GOV-000 G2/G4 için gerekli olabilir) | Orta — tanım + ROSTER kaydı |
| 5 | **Yeni ENS-NNNN yapıtı** | Külliyat'a yeni belge | **Yüksek** — REGISTRY numarası, künye, SKR turu, bağımlılık grafiği |
| 6 | **Yeni katman / faz (SON ÇARE)** | Yeni NNNN-bandı ya da yeni bağımlılık yönü | **Çok yüksek** — ADR + mimari donma etkisi + Tier 3 plan |

## Kurallar

1. **Aşağıdan yukarı sor:** "Bunu mevcut X'i genişleterek çözebilir miyim?" → hayırsa bir üst.
2. **Atlama yapma:** 5. basamağa (yeni yapıt) çıkmadan 1-4 elendi mi?
3. **Şüphede aşağıda kal.** Dar çözümü büyütmek kolay; geniş çözümü küçültmek zor —
   ENS'te *imkânsız*, çünkü yayınlanmış yapıt geri çekilmez, yalnızca `deprecated` olur.
4. **5-6. basamak = Tier 3 plan** (`plan-first.md`) + bağımsız SKR turu.
5. **Numara harcamak geri alınamaz.** REGISTRY numarası bir kez verildi mi, yapıt
   iptal edilse bile numara yeniden kullanılmaz. Bu, 5. basamağı `IsIrreversible`
   yapar → `plan-first.md` gereği otomatik Tier 3.

## Anti-pattern

- ❌ "Yeni kavram = yeni ENS belgesi" refleksi → önce mevcut yapıta bölüm eklenebilir mi?
- ❌ Tek kullanımlık iş için yeni skill/agent → mevcut akışta inline çöz.
- ❌ "İleride lazım olur" diye geniş soyutlama.
- ❌ **Yeni skill/agent yaratmadan önce mevcut listeyi kontrol ETMEMEK** → `.claude/skills/`,
  `.claude/agents/` ve ROSTER'a bak; aynısı varsa **genişlet**, yaratma.
- ❌ **Dış repodan "esin" diye ENS'te zaten olanı tekrar kurmak.** ENS'te kontrol katmanı
  (SKR zinciri, GOV-000 G2/G4, Madde X kanıt disiplini) zaten güçlüdür; operax'ın "kontrol ettir"
  adımını yeniden kurmak çift kapı üretir. Bkz. `work-protocol.md` §3.

## NE ZAMAN UYGULANMAZ

- Anayasa ya da RFC'nin **kendisi** yeni bir yapıt talep ediyorsa — merdiven tartışılmaz,
  yapıt açılır.
- SKR/review kayıtları merdivene tabi değildir: bağımsız bir inceleme her zaman kendi
  dosyasına yazılır (GOV-000 G2/G4 gereği izlenebilir olmalı).

## İlişkili
- `.claude/rules/plan-first.md` — 5-6. basamak = Tier 3
- `.claude/rules/advisor-skills.md` — hangi danışmana sorulacağı
- `REGISTRY.md` — numara tahsisi
- `.claude/agents/ROSTER.md` — mevcut ajan kadrosu
