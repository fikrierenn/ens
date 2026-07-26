---
name: yetenek-uret
description: ENS çatısına yeni yetenek (skill / agent / hook / rule) SİSTEMATİK üretir ya da mevcut olanı GÜNCELLER. footprint-ladder'ın en-dar-basamak kuralını zorunlu kılar. "yeni skill", "yeni ajan", "hook ekle", "kural yaz", "yetenek üret" denince devreye gir.
allowed-tools: Read, Write, Edit, Grep, Glob, Bash
user-invocable: true
model: inherit
---

# ENS Yetenek Üretimi

> Prior art: operax `yetenek-uret`. ENS'e uyarlandı — ENS'te yeteneğin **en pahalı
> basamağı** yeni Razor sayfası değil, **yeni ENS-NNNN yapıtıdır** (REGISTRY numarası
> harcar, geri alınamaz).

## 0. ÖNCE DUR — merdiven kontrolü (atlanamaz)

`.claude/rules/footprint-ladder.md`. Sırayla sor, **ilk "evet"te dur**:

1. Mevcut bir yapıtı/skill'i/rule'u **genişleterek** çözülür mü? → genişlet, üretme.
2. Tekrarlanan bir iş akışı mı? → **skill** (tetik-bazlı, ucuz).
3. Her oturumda geçerli kalıcı davranış kuralı mı? → **rule** (bağlam maliyeti var).
4. Bağımsız context gerektiren bir rol mü (G2/G3)? → **agent**.
5. Mekanik, tekrar eden bir doğrulama mı? → **hook**.
6. Yeni bir kavram/belge mi? → **ENS-NNNN yapıtı** — Tier 3, plan zorunlu.

> **Mevcut listeye BAKMADAN üretme.** `.claude/skills/`, `.claude/agents/`,
> `.claude/rules/`, `.claude/hooks/` ve `ROSTER.md`'ye bak. Aynısı varsa **genişlet**.

## 1. Hangisi ne zaman

| Tür | Ne zaman | Kaçınılacak |
|---|---|---|
| **skill** | Tekrarlanan, tetiklenebilir iş akışı | Tek kullanımlık iş |
| **rule** | Her oturumda geçerli olması gereken davranış | "Bazen faydalı" olan şey |
| **agent** | **Bağımsız context** gerekiyorsa (yazan ≠ doğrulayan) | Ana bağlamda yapılabilecek iş |
| **hook** | Mekanik, insan yargısı gerektirmeyen kontrol | Yargı gerektiren denetim |

> **ENS'e özgü kural:** denetleyici bir rol üretiyorsan **agent** olmalı, skill değil —
> G2/G3 bağımsız context ister. Ve o ajanın `Edit`/`Write` aracı **olmamalı**: ölçen,
> ölçtüğünü düzeltemez.

## 2. Zorunlu içerik

**Her yetenek** şunları taşır:
- **Var olma sebebi** — hangi somut boşluğu/hatayı kapatıyor. Mümkünse gerçek vakayla
  (`ens-test-runner` → "dört denetim ajanı `dotnet test` çalıştıramadı").
- **NE ZAMAN UYGULANMAZ** bölümü. İstisnası yazılmamış kural, ya körü körüne uygulanır
  ya sessizce terk edilir.
- **İlişkili** bölümü — hangi rule/skill/Anayasa maddesine bağlı.
- **Prior art dürüstlüğü** — dışarıdan uyarlandıysa kaynak yazılır, "biz bulduk" denmez.

**Agent** ek olarak: `tools:` listesi (minimum yetki), `model:`, ve **mutlak kurallar**
(uydurma yasağı, kanıt zorunluluğu).

**Hook** ek olarak: bloke mi ediyor, bilgilendiriyor mu — açıkça yazılır. ENS'te varsayılan
**bilgilendirir, bloke etmez**: sessiz sapma ile bilinçli sapmayı ayırt etmek insanın işidir.

## 3. Üretim adımları

1. Merdiven kontrolü (§0) — hangi basamak, gerekçe yaz.
2. Mevcut listeyi tara — duplikasyon var mı?
3. Dosyayı yaz (`.claude/<tür>/<ad>[/SKILL.md]`).
4. **agent ise** `ROSTER.md`'ye satır ekle.
5. **hook ise** `.claude/settings.json`'a bağla **ve çalıştırıp doğrula** — bağlanmamış
   hook, olmayan hook'tur.
6. **rule ise** ilgili diğer rule'ların "İlişkili" bölümlerine karşılıklı bağ ekle.
7. 5. basamak ve üstüyse: `plan-first.md` gereği **plan yaz, onay al**.

## 4. Güncelleme (yeni üretmekten önce hep bunu dene)

Mevcut yetenek yetmiyorsa: **önce genişlet.** Yeni dosya açmak son çare. Genişletirken
"NE ZAMAN UYGULANMAZ" bölümünü de güncelle — yeni kapsam yeni istisna doğurur.

## NE ZAMAN UYGULANMAZ

- Tek seferlik iş — inline çöz, yetenek üretme.
- Anayasa/RFC doğrudan bir yapıt talep ediyorsa — merdiven tartışılmaz.
- Acil düzeltme sırasında — önce düzelt, yeteneği sonra üret.

## İlişkili
- `.claude/rules/footprint-ladder.md` — merdivenin sahibi
- `.claude/rules/plan-first.md` — 5-6. basamak Tier 3
- `.claude/rules/advisor-skills.md` — yeni ajan üretilince kataloğa satır
- `.claude/agents/ROSTER.md` — kadro kaydı
