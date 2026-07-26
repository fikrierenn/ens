# Çalışma Protokolü — Danış → Yap → SKR'ye Sok → Kanıtla

Her substantive iş bu 4 adımlı döngüden geçer. Teori, ontoloji, ADR, kernel kodu, test,
yönetişim değişikliği — istisnasız. `paths:` yok — compact sonrası da geçerli.
**Tier 1 trivial hariç.**

> **Prior art:** operax `work-protocol.md` (Danış → Yap → Kontrol Ettir → Smoke).
> **3. adım ENS'te yeniden kurulmadı** — ENS'in SKR zinciri + G2/G3 kapısı, operax'ın
> reviewer zincirinden zaten güçlüdür. Bu kural onu *referans alır*, kopyalamaz.
> (`footprint-ladder.md` anti-pattern: "zaten olanı tekrar kurmak.")

---

## 1. ÖNCE DANIŞ (üretimden ÖNCE)

İş bir danışman alanına giriyorsa, üretmeden önce danış — `advisor-skills.md` kataloğu.
Danışman çıktısı **çerçeve/ölçüt** verir, dayatmaz; kararı **gerekçeyle** sen verirsin.

Danışılmadan yazılan teori/mimari/ontoloji işi **eksik** sayılır.

## 2. YAP

Uygula. İlgili disipline uy: `plan-first`, `footprint-ladder`, künye şeması
(`.claude/standards/metadata-header.md`), dil politikası, REGISTRY numaralandırması.

> **Yargı kararı verdiğin her noktayı İŞARETLE.** Adım 3'te adversarial olarak sınanacak.
> Kod tarafında bu, ENS'te zaten bir konvansiyondur: `// AUDIT_*`, `// TRACE: ADR-NNNN`.

## 3. SKR'YE SOK — bağımsız + adversarial (üretimden SONRA)

**Öz-onay YOK.** Anayasa G2/G3: yazan doğrulayamaz. Bu, ENS'te *zaten* anayasal bir
kuraldır — bu protokol onu tekrar etmiyor, uygulanmasını zorunlu kılıyor.

| Yapıt türü | Kapı |
|---|---|
| Teori / felsefe / ontoloji (Faz 0-2) | **`ens-skeptic` → SKR kaydı** (verdict: survives / wounded / refuted) |
| ADR / mimari | Bağımsız `ens-architect` context + gerekiyorsa hizalama incelemesi |
| Kernel kodu | `.claude/skills/adversarial-test/` — 8 kanıtlanmış saldırı yüzeyi |
| Künye / terminoloji / numaralandırma | `ens-style-guardian` |

**Adversarial demek:** "doğru" varsayma; **kırmaya çalış.** Kanıt iste (`dosya:satır`).
Emin değilse **DOĞRULANMADI** yaz — uydurma.

> **Bu adımın ölüm kalım kuralı:** denetleyen ajan bir aracı çalıştıramıyorsa (ör. `Bash`
> yok, `dotnet test` koşulamıyor) bunu **dürüstçe bildirir ve sonuç UYDURMAZ**. SKR-041
> emsali. Çalıştırılamayan test, geçmiş test değildir.

## 4. KANITLA

"Derlendi" / "yazıldı" YETMEZ — **gerçekten öyle mi** kanıtla:

- **Kod:** `dotnet test` **sahibi tarafından** çalıştırılır, sonuç sayıyla raporlanır.
  Kırmızıysa **söylenir, gizlenmez.**
- **Teori:** iddia `Failure conditions` bölümünde yanlışlanabilir hâlde yazılı mı? Künye
  `eng`/`sci` seviyesi gerçekten hak edildi mi?
- **Sayısal iddia:** *"doğrulandı"* demeden önce sor — **tautoloji mi?** Bir sayı kendi
  tanımından değil **artık (residual)** olarak hesaplanıyorsa, onu içeren özdeşliği test
  etmek hiçbir şeyi yanlışlamaz.
  Gerçek vaka: ROADMAP aylarca *"zincir kuralı matematiksel olarak doğrulandı"* dedi;
  `LevelNoise` `DecisionEntropy.cs:48`'de artık olarak hesaplanıyordu. İddia geri çekildi.

## Atlanırsa

1. **Kabul et** — mazeret yok. 2. Dön, eksik adımı tamamla.
3. **"Yazdım = doğru" ve "yeşil test = sağlıklı" varsayımı YASAK.**
   ENS'te `AUDIT_DEFECT_*` adlı bir testin **geçmesi**, kusurun **var olduğu** anlamına
   gelir. Yeşil panel envanterdir, sağlık değil (`DEFECT-REGISTER.md` §0).

## NE ZAMAN UYGULANMAZ

- **Tier 1 trivial:** typo, künye alanı, bozuk link, format.
- **Adım 1 muafiyeti:** danışmanın kendi alanının yapıtını yazması (kendine danışmaz) —
  kapı üretim sonrasına, adım 3'e kayar.
- **Adım 4 muafiyeti:** yalnız-doküman değişikliğinde `dotnet test` beklenmez; ama iddia
  koda atıfta bulunuyorsa atıf **doğrulanır**.
- **Acil düzeltme:** yayınlanmış yanlış iddia bulunduğunda adım 1 atlanabilir (Madde X
  gecikmeyi yasaklar) — ama adım 3 ve 4 **atlanamaz**.

## İlişkili
- `.claude/rules/advisor-skills.md` — adım 1 kataloğu
- `.claude/rules/plan-first.md` — Tier tespiti (bu döngünün ne zaman zorunlu olduğu)
- `.claude/skills/adversarial-test/SKILL.md` — adım 3, kod tarafı
- Anayasa Madde X (Yanlışlanabilirlik), G2/G3 (yazan ≠ doğrulayan)
