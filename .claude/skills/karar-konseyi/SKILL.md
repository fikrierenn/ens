---
name: karar-konseyi
description: Yüksek InfoNeed kararları için bağımsız danışman paneli + karşılıklı eleştiri + sentez. Tier 3 planın "en az 2 reddedilen alternatif" gereğini besler. "konsey", "tartışalım", "hangi seçenek", "kararsız kaldım", "pressure-test et" denince devreye gir.
allowed-tools: Read, Grep, Glob, Task
user-invocable: true
model: inherit
---

# Karar Konseyi — yüksek InfoNeed kararları

> Prior art: Karpathy'nin LLM Council metodolojisi (operax `llm-council` üzerinden).
> **ENS'e uyarlanırken tetikleyicisi değiştirildi:** operax "yüksek belirsizlik + yüksek
> maliyet" der; ENS'te bu **zaten ölçülen bir büyüklüktür** —
> `InfoNeed = Stake × (1 − Confidence)` (ENS-3022, Howard 1966 VOI).
> Yani konsey, sezgiyle değil **formülle** toplanır.

## Ne zaman toplanır

`InfoNeed` yüksekse — yani **hem stake yüksek hem güven düşük**:
- Mimari yön kararı (ADR seviyesi)
- Teorik bir kavramın nasıl modelleneceği
- Faz sırası / önceliklendirme
- Bir borcun kapatılması mı taşınması mı (ENS-2003 D-5 tipi)
- Tier 3 planın §4 "reddedilen alternatifler" bölümünü doldurmak

## Ne zaman TOPLANMAZ

- **Stake yüksek ama Confidence de yüksekse** — cevap zaten belli, konsey tiyatrodur.
- **Stake düşükse** — InfoNeed düşük; konsey P5 (attention) israfıdır.
- Basit evet/hayır, tek doğru cevabı olan sorular.
- **Zaten onaylanmış planın içinde** — plan-first yönetir, konsey yeniden açmaz.

> Konsey **pahalıdır** (N bağımsız context). Toplamadan önce InfoNeed'i gerekçelendir.

## Süreç

### 1. Soruyu sabitle
Tek cümle, karar formunda. "X mi Y mi" ya da "X yapmalı mıyız". Belirsiz soru belirsiz
konsey üretir.

### 2. Bağımsız görüşler (paralel, birbirini görmeden)
ENS'in gerçek kadrosundan **konuya uygun 3-5 ajan** seç — hepsi değil, ilgili olanlar:

| Ajan | Getirdiği lens |
|---|---|
| `ens-philosopher` | Teorik tutarlılık, Külliyat'a oturuyor mu |
| `ens-researcher` | Prior art — bu zaten çözülmüş mü, kim nasıl çözmüş |
| `ens-architect` | Katman/bağımlılık etkisi, mimari donma |
| `ens-ai-architect` | Kernel/runtime/model tarafı |
| `ens-memory-engine` | Memory/context/learning tarafı |
| `ens-backend-architect` | Uygulanabilirlik, .NET gerçekliği |
| `ens-skeptic` | **Her konseyde zorunlu** — saldırgan taraf |

**Kritik:** her ajan **aynı soruyu** alır ve **diğerlerinin cevabını görmez**. Görürse
bağımsızlık ölür, konsey yankı odasına döner.

### 3. Karşılıklı eleştiri
Görüşler toplandıktan sonra her ajana **diğerlerinin argümanları** verilir ve sorulur:
*"Bu argümanlardan hangisi seninkinden güçlü? Kendi pozisyonunu değiştiriyor musun?"*
Pozisyon değiştiren ajan **zayıf değil, dürüsttür** — bu kaydedilir.

### 4. Sentez
Kazanan görüş alınır, **ama diğerlerinin en iyi fikirleri aşılanır.** Sentezde
zorunlu olarak yazılır:
- Seçilen yol ve **gerekçesi**
- **Reddedilen alternatifler ve neden reddedildikleri** (Tier 3 planın §4'ünü besler)
- **Azınlık görüşü** — konsey oybirliğine varmadıysa muhalefet kaydedilir, silinmez
- **Bu kararın yanlış olduğunu ne gösterirdi?** (Madde X)

## Çıktı

Konsey sonucu bir **karar kaydıdır**, sohbet değil. Nereye yazılacağı:
- Mimari karar → ADR taslağı
- Teorik karar → ilgili ENS-NNNN yapıtının ilgili bölümü + SKR turu
- Plan içi alternatif değerlendirmesi → `plans/NN-*.md` §4

## Mutlak kurallar

1. **Bağımsızlık korunur.** Ajanlar birbirinin cevabını 2. adımda görmez.
2. **Azınlık görüşü silinmez.** Oybirliği çıkmadıysa bu bir bulgudur.
3. **Konsey karar VERMEZ, seçenek üretir.** Kararı sahibi verir — gerekçeyle.
4. **`ens-skeptic` her konseyde bulunur.** Saldırgan yoksa konsey onay makinesidir.
5. **Sonuç uydurulmaz.** Bir ajan cevap veremediyse bu yazılır.

## İlişkili
- `.claude/rules/plan-first.md` — Tier 3 planın §4'ü
- `.claude/rules/advisor-skills.md` — ajan kataloğu
- `ENS-3022` (Decision Gravity) — toplanma eşiğinin kaynağı
- Anayasa Madde X — "ne yanlışlar" sorusu zorunlu
