---
id: SKR-005
type: skeptic-review
origin: ENS-2002
depends_on: [ENS-2002]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-005 — ENS Context Theory (ENS-2002) Saldırısı

## Verdict
**wounded.** Belge güçlü: context'i bir *ilişki* (karara-göreli) olarak tanımlaması ve
LAW-CONTEXT'i ters-U ile iki yönlü kılması doğru yönde. Ama iki prior art hatası ve bir
çözülmemiş temel sorun var. Birincisi SKR-001/003 kalıbının nüksüdür: yerleşik bir sonucu
kredisiz "keskinleştirme" gibi sunmak. Üç talep karşılanmadan `canon:true` olamaz.

## Bulgu 1 — Ters-U zaten information overload literatürüdür (kredisiz, en ağır)

§Model 3, karar kalitesinin context'te ters-U izlediğini "LAW-CONTEXT'in keskinleştirilmesi"
olarak sunuyor. Bu ters-U **yeni değildir ve iyi kurulmuştur:**

- **Eppler & Mengis (2004)** — information overload'ı ters-U ile modeller: karar
  performansı optimal noktaya kadar artar, bilişsel kapasite aşılınca düşer. **Yerkes-Dodson
  yasası** kökenli; Schroder-Driver-Streufert (1967) bilgi yükü–performans eğrisine dek gider.

ENS'in delta'sı savunulabilir ama belirtilmeli: ENS, yükü *ham hacim* değil **karara-göreli
ilgililik** cinsinden ölçer ve bir **attention bütçesine** bağlar (P5), üstelik Context
Score ile *operasyonelleştirir*. Bu bileşim yeni olabilir; ama ters-U'nun kendisi Eppler-
Mengis'e aittir ve öyle anılmalıdır (Anayasa Madde VI).

## Bulgu 2 — Dey-Abowd delta'sı fazla iddia edilmiş

§Historical context, ENS'i Dey & Abowd'dan "karara-göreli, 'herhangi bir bilgi' değil" diye
ayırıyor. Ama Dey-Abowd'un context-*awareness* tanımı zaten görev-görelidir: *"bir sistem,
context'i kullanıcının görevine ilgili bilgi/servis sağlamak için kullanıyorsa
context-aware'dir; ilgililik göreve bağlıdır."* Yani "görev/karar-göreli ilgililik" kısmen
onlarda da var. ENS'in gerçek delta'sı daha dar: **ölçülebilir Density/Score + attention-
bütçeli ters-U + staleness.** İddia bu dar delta'ya çekilmeli; yoksa var olmayan bir
özgünlük iddia edilir.

## Bulgu 3 — İlgililik-döngüselliği çözülmemiş (en derin)

Belge bunu en ciddi failure condition olarak dürüstçe işaretliyor: `relevance(s, p)`'yi
bilmek için kararı bilmek gerekir; kararı vermek için ilgililik gerekir. Ama işaret etmek
çözmek değildir. Bu döngü kırılmazsa:
- `Context(d)`, `ContextDensity`, `ContextScore` **tanımlı ama hesaplanamaz** kalır;
- dolayısıyla Decision Entropy'nin dayandığı "context-benzerliği" (R1) ölçülemez.

Bu, teorinin *operasyonel* olduğu iddiasını boşa çıkarır. Bir **döngü-kırıcı** gerekir.
Skeptic'in işaret ettiği yön (öneri, çözüm değil): ilgililik, mevcut karardan değil,
**Company Memory'den** kestirilebilir — benzer Purpose'lu geçmiş commit-edilmiş kararların
fiilen kullandığı ve sonucu iyileştiren context. Böylece ilgililik, dairesel biçimde
şimdiki karardan değil, tarihsel karar-sonuç verisinden türer (P3'e bağlanır). Bunu owner
geliştirmeli.

## Sahibine talepler (kapıyı geçmek için)
1. **Ters-U'yu Eppler-Mengis (2004) / Yerkes-Dodson'a kredile;** ENS delta'sını (ilgililik-
   göreli yük + attention bütçesi + operasyonelleştirme) açıkça yaz.
2. **Dey-Abowd delta'sını daralt;** onların görev-göreli ilgililiğini kabul et, ENS'in
   farkını ölçülebilirlik + staleness ile sınırla.
3. **İlgililik-döngüselliğini kır;** hesaplanabilir, dairesel-olmayan bir relevance kestirim
   yordamı ver (ör. Memory-temelli tarihsel ilgililik). Bu olmadan Context Score
   operasyonel değildir ve R1 zinciri kopar.

## İç tutarlılık
Context'in ilişki oluşu, Decision Theory (ENS-2001) alanıyla ve attention (P5) ile tutarlı.
Staleness'in LAW-ORG-MEMORY'ye bağlanması yerinde. Çelişki yok; sorun kredisiz prior art ve
hesaplanamaz relevance.

## Kaynaklar
- **Eppler, M. J. & Mengis, J. (2004).** The Concept of Information Overload. *The
  Information Society*, 20(5). — ters-U, information overload.
- **Yerkes-Dodson (1908)** — arousal/performans ters-U kökeni.
- **Dey, A. K. (2001).** Understanding and Using Context. *Personal and Ubiquitous
  Computing* — context tanımı; context-awareness'ta görev-göreli ilgililik.
- Sperber & Wilson, Relevance Theory — ilgililik/çaba ödünleşimi.
