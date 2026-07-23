---
id: SKR-013
type: skeptic-review
origin: ENS-3022
depends_on: [ENS-3022]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-013 — Decision Gravity (ENS-3022) Saldırısı

## Verdict
**wounded.** Metafor tuzağından kaçış başarılı: Gravity fiziksel çekim değil, stake↔context
ilişkisi + context açığı + attention tahsisi olarak operasyonel. VOI/Payne-Bettman-Johnson
konumlaması proaktif ve SKR-001'in normatif/descriptive uyarısı ele alınmış. Ama iki gerçek
teorik hata var: (1) Gravity stake'e *tek başına* dayanıyor, oysa Value of Information stake
**ve** belirsizliği birlikte ister; (2) "NormativeContext" aslında normatif değil, descriptive
bir peer-ortalaması — bir kategori hatası. Üç talep karşılanmadan `canon:true` olamaz.

## Bulgu 1 — Gravity stake'e tek başına dayanıyor; VOI stake × belirsizlik ister (en ağır)

Belge, context ihtiyacını `Stake(d)` ile yönlendiriyor ve §failure #4'te "stake bazen context
ihtiyacının kesin ölçüsü değil" diye geçiştiriyor. Ama bu geçiştirilecek bir dipnot değil,
**Howard VOI'nin (1966) merkezî bulgusu:** bilginin değeri, sonuçların (stake) *ve* belirsizliğin
**birlikte** fonksiyonudur — "sonuçları dikkate almayan hiçbir teori belirsizliğin önemini
tanımlayamaz" (Howard). Yüksek stake + düşük belirsizlik = düşük VOI = az context gerekir
(karar açık). Yüksek stake + yüksek belirsizlik = yüksek VOI.

ENS zaten belirsizlik ölçüsüne sahip: **Confidence (P6)** — düşük Confidence = yüksek
belirsizlik. Dolayısıyla Gravity/attention **`Stake × Belirsizlik`** (ör. `Stake × (1 −
Confidence)`) ile yönlendirilmeli, stake'le değil. Bu, hem VOI ile hizalar hem ENS'in mevcut
Confidence alanını kullanır. Şu hâliyle model, açık ama kesin kararlara boşuna dikkat çeker.

## Bulgu 2 — "NormativeContext" descriptive'dir, normatif değil (kategori hatası)

§Model 3, `NormativeContext = E[ContextDensity | Stake]` diyor ve buna "normatif" adını veriyor.
Ama bu bir **peer-ortalamasıdır** (benzer stake'li kararlar *tipik olarak* ne kadar context
çekti) — yani **descriptive** bir uyum ölçüsü, *rasyonel optimum* değil. Howard VOI ise gerçek
bir **normatif** çıta verir (optimal bilgi yatırımı: marjinal context değeri = marjinal maliyet).
Peer-ortalamasına "normatif" demek, herkes az-context'liyse "yeterli" demektir (belge bunu
§failure #2'de zaten itiraf ediyor). Bu:
- ya dürüstçe **"PeerContext / uyum açığı"** olarak yeniden adlandırılmalı (peer'a uyum ölçüyoruz,
  rasyonaliteyi değil),
- ya da gerçek normatif çıta için **VOI** entegre edilmeli (context'i marjinal değeri maliyeti
  eşitleyene dek topla).

## Bulgu 3 — Stake normalizasyonu (carry-forward)
Heterojen kararların stake'ini ortak birimde kıyaslamak (finansal vs itibari vs stratejik)
çözülmemiş; OL1 gerekli ama yetersiz. Popülasyon fiti bu olmadan anlamsız. Bir stake-normalizasyon
şeması gerekir.

## Sahibine talepler (kapıyı geçmek için)
1. **Gravity'yi stake × belirsizlik ile tanımla** (Confidence'ı belirsizlik terimi yap);
   Howard-VOI ile hizala. Stake-tek-başına modelini bırak.
2. **"NormativeContext"i dürüstçe yeniden adlandır** (peer-uyum) *veya* gerçek normatif çıta
   için VOI entegre et. İkisini karıştırma.
3. **Stake normalizasyon şeması** ekle (heterojen kararlar arası).

## İç tutarlılık
Context açığı → attention (P5) mantığı sağlam ve ENS'in dikkat-optimizasyonuyla tutarlı.
Decision Entropy ile birlikte "kırmızı bölge" okuması yerinde. Sorun: stake-tek-başınalık ve
descriptive/normatif karışıklığı — ikisi de VOI'yi tam kullanmamaktan geliyor.

## Kaynaklar
- **Howard, R. A. (1966). Information Value Theory.** — VOI; stake **ve** belirsizlik birlikte;
  peer-ortalaması değil, optimal bilgi yatırımı.
- Payne, Bettman & Johnson (1993), *The Adaptive Decision Maker*; Simon (bounded rationality/
  attention) — belgede konumlanmış.
