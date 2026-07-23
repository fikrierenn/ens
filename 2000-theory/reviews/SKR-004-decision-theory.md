---
id: SKR-004
type: skeptic-review
origin: ENS-2001
depends_on: [ENS-2001, SKR-003]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-004 — ENS Decision Theory v0.2 İkinci Saldırısı

## Verdict
**survives** (üç ileri-taşıma yükümlülüğüyle). v0.2, SKR-003'ün üç talebini de karşılıyor.
Individuation talebine verilen yanıt — atomu deliberation yerine **commitment** sınırında
bireylemek — yalnızca yeterli değil, ENS'in bu belgedeki **gerçek özgün katkısıdır**:
DMN (kural), Simon (süreç fazları) ve Mintzberg (sınırsız süreç) hiçbiri atomu taahhüt anına
oturtmaz. Kalan zayıflıklar mevcut kusur değil, dürüstçe işaretlenmiş kapsam sınırlarıdır.
**Kavram Külliyat'a girebilir (canon:true).**

## Talep-talep doğrulama
1. **Simon/Mintzberg kredisi — KARŞILANDI.** §Prior art tablosu ve §Model 3'te fazlar
   etiketli; delta (Measurement→Learning→Memory kapanışı) açık. Simon/Mintzberg döngüyü
   kapatmaz — bu ayrım doğru.
2. **DMN/ADR konumu — KARŞILANDI.** Tablo, DMN'in tekrarlanabilir/kural kararlarını
   modellediğini ve memory-of-why/outcome/learning taşımadığını doğru saptıyor. ADR'nin
   genellenmesi de yerinde.
3. **Individuation — KARŞILANDI (güçlü).** Dört koşullu, commitment-mühürlü ölçüt,
   Mintzberg'in (1976) "süreçler sınırsızdır" itirazını, sınırı *taahhüde* taşıyarak aşıyor.
   Decision Graph düğümlerinin commitment olması, nicel yapının (R1) zeminini kuruyor.

## Yeni saldırı yüzeyi — emergent strategy (en güçlü kalan itiraz)

Commitment ölçütü, commit-*edilmeyen* kararları dışarıda bırakır. Bunun en tehlikeli biçimi
belgede "örtük karar" olarak anılıyor ama adı konmamış: **Mintzberg & Waters (1985), "Of
Strategies, Deliberate and Emergent" (SMJ 6:257–272).** Emergent strateji, *niyet
yokluğunda* eylemde tutarlılıktır — yani ne tek bir Purpose'u ne de tek bir Commitment anı
vardır, ama gerçek ve sonuç doğuran bir "karardır." ENS'in atomu bunu **yapısal olarak
yakalayamaz.**

Bu çürütme değildir — ENS zaten yalnızca deliberate/commit-edilmiş kararları atom sayacağını
dürüstçe söylüyor (ENS-1000 §VII ile tutarlı). Ama sınırın *büyüklüğü* önemlidir: eğer
stratejik değerin önemli kısmı emergent ise, ENS atomu operasyonel/taahhütlü katmanı yakalar,
stratejik-emergent katmanı kaçırır. Bu, bir Faz 1-2 borcudur, Faz kapısının engeli değil.

## Kalan riskler (ileri-taşıma yükümlülükleri)
- **O1 — Emergent sınıfı sınırla/ölç.** Mintzberg-Waters (1985) emergent stratejinin ENS
  atomunun dışında kaldığını kabul et; bu sınıfın *pattern olarak* (transaction/context
  üzerinden) tespit edilip edilemeyeceğini Faz 2'de araştır. Şu an açık borç.
- **O2 — "Mühür"ü keskinleştir.** §Failure conditions'ın kendi önerisi — commitment anını
  **geri-dönülemezliğin (irreversibility) başladığı an** olarak tanımlamak — muğlak
  "kademeli commitment" sorununu büyük ölçüde çözer. Bu tanımı ölçüte terfi ettir.
- **O3 — Revizyon vs yeni-karar kimliği.** Aynı Purpose'a zamanla verilen iki commitment
  "bir kararın revizyonu" mu, "iki ayrı karar" mı? Aggregate kimlik kuralı (event-sourcing)
  netleştirilmeli; Decision Entropy'nin doğru sayması buna bağlı.

## İç tutarlılık
Klasik DT ayrımı, özyineleme ve commitment ölçütü birbirini tutuyor. Lifecycle diyagramı
model metniyle uyumlu. Terminoloji sözlükle (ENS-4000) tutarlı. Çelişki bulunamadı.

## Sonuç
Commitment-mühürlü individuation, ENS'e prior art'ta bulunmayan savunulabilir bir delta
kazandırır ve nicel katmanın (Decision Entropy/Gravity/Capital) önünü açar. Kavram `ratified`
edilebilir. O1-O3, Faz 1-2'de — özellikle Decision Entropy yazılırken — yeniden gündeme
gelecek.

## Kaynaklar
- Simon (1960); Mintzberg vd. (1976) — SKR-003.
- **Mintzberg, H. & Waters, J. A. (1985). Of Strategies, Deliberate and Emergent.**
  *Strategic Management Journal*, 6(3), 257–272. — emergent strateji, ENS atomunun kapsam sınırı.
- OMG DMN; Nygard ADR — SKR-003.
