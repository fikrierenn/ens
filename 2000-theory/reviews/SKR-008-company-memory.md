---
id: SKR-008
type: skeptic-review
origin: ENS-2003
depends_on: [ENS-2003, SKR-007]
status: ratified
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-008 — ENS Company Memory v0.2 İkinci Saldırısı

## Verdict
**survives** (iki adlandırılmış ileri-bağımlılıkla). v0.2, SKR-007'nin üç talebini de
karşılıyor. Purpose-tipi döngüselliği doğru kırılmış, survivorship bias'a gerçek bir
mekanizma verilmiş, attribution borcu dürüstçe adlandırılmış bir kavrama yükseltilmiş.
Kalan iki nokta mevcut kusur değil, açıkça işaretlenmiş forward-dependency'dir. **Kavram
Külliyat'a girebilir (canon:true).**

## Talep-talep doğrulama
1. **Purpose-tipi taksonomisi — KARŞILANDI.** §Model 2, sınıflandırmayı **beyan edilen
   niyetten** (framing anında, memory getiriminden önce) yapıyor ve Enterprise Ontology'ye
   bağlıyor. Girdi outcome/context'ten bağımsız olduğu için döngü yok. Doğru çözüm.
2. **Survivorship bias — KARŞILANDI (güçlü).** Retention önceliği ∝ |Learning|, outcome
   pozitifliği değil; başarısız-ama-ölçülmüş kararlar *öncelikli* saklanıyor. Bu, bias'ın
   doğrudan panzehri ve zarif bir hamle: ENS başarısızlığı daha güçlü hatırlar.
3. **Attribution yükseltmesi — KARŞILANDI.** §Model 5, R2'yi **ENS-2004 Learning Theory**'ye
   yükseltiyor, bağımlılığı `referenced_by` ile işaretliyor ve Company Memory'nin yalnızca
   Expected/Actual + attribution confidence sakladığını, atfı *çözmediğini* açıkça söylüyor.
   CBR forgetting delta'sı da "salience sönümle, silme" dar hâline çekilmiş.

## Kalan riskler (ileri-bağımlılıklar — kapı açık, borç adlandırılmış)
- **OM1 — Enterprise Ontology bağımlılığı.** Purpose-tipi taksonomisi, henüz yazılmamış
  Enterprise Ontology'ye (ENS-4xxx) dayanıyor. Döngüsellik yok (ilke sağlam), ama getirim
  kalitesi ontoloji olgunlaşana dek sınırlı. Ontoloji Faz 2'de üretilmeli.
- **OM2 — ENS-2004 Learning Theory zorunlu.** Retention (∝|Learning|), Context relevance ve
  Learning, üçü de attribution'a bağlı; bu artık ENS-2004'e yüklü. ENS-2004 attribution
  modelini (counterfactual, atfedilebilir sınıf) üretmeden nicel katman (Decision Entropy)
  kanıtlanmamış varsayım üstünde kalır. **ENS-2004 sıradaki zorunlu kavramdır.**

## İç tutarlılık
salience/record ayrımı, karşı-survivorship retention ve Purpose-tipi getirim birbirini
tutuyor. Decision Theory düğümleri ve Context relevance ile tutarlı. Çelişki yok.

## Sonuç
Company Memory, ENS'in yapısal kilit taşı olarak sağlamdır; Context'i hesaplanabilir kılar ve
Decision Entropy'nin zeminini kurar. `ratified` edilebilir. Teori kendi sırasını dayatmaya
devam ediyor: **Context → Memory → Learning (ENS-2004).**

## Kaynaklar
Aamodt & Plaza (1994); Walsh & Ungson (1991); March (1991) — önceki SKR'ler. v0.2 için yeni
dış kaynak gerekmedi.
