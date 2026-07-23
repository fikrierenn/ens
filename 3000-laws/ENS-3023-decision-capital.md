---
id: ENS-3023
title: Decision Capital
type: law
canon: false
maturity: M3
origin: ENS-0000 §III (P3, P4), ENS-2003, ENS-2004
depends_on: [ENS-0000, ENS-2001, ENS-2003, ENS-2004, ENS-3000, ENS-4000]
referenced_by: []
principles: [P3, P4, P1]
status: ratified
owner: ens-philosopher
version: 0.2.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-016
---

# Decision Capital

> Fizik üçlüsünün üçüncüsü (Entropy, Gravity, Capital). Company Memory'de biriken ve
> gelecekteki kararları iyileştiren değerin **akış-amortisman-getiri** hesabı. `canon: false`.
>
> **v0.2 notu:** [SKR-015](reviews/SKR-015-decision-capital.md)'e yanıt. (1) Intellectual
> Capital delta'sı daraltıldı, (2) Capital'in Memory retention'dan **ayırt edici içeriği**
> (stok değil, akış/amortisman/ROI/yatırım-kararı) netleştirildi, (3) transfer sınırı
> (context-indekslilik) tanıma taşındı. §Yanıt tablosu sonda.

## Definition

**Decision Capital, bir organizasyonun karar-belleğinde biriken, _Purpose-tipi içinde_
yeniden kullanılarak gelecekteki kararları iyileştiren değerin akış hesabıdır** (P3, P4).
İki incelik:
- **Context-indekslidir (transfer sınırı, SKR-015 Bulgu 3):** getirisi ancak benzer
  Purpose-tipli kararların reuse'uyla (ENS-2003) gerçekleşir; tipler arası serbestçe transfer
  olmaz. Gerçek sermayenin fungibility'sini tam taşımaz — bu bilinçli bir sınırdır.
- **Stok değil, dinamiktir (Memory'den ayrım, SKR-015 Bulgu 2):** *stok* zaten Company
  Memory'dir. Decision Capital'in ayırt edici içeriği **akış** (yatırım/amortisman), **getiri**
  (reuse ROI) ve **yatırım kararı** çerçevesidir — Memory'nin sağlamadığı ekonomik dinamik.

## Motivation

Bir organizasyonun değeri iyi karar verme kapasitesinde yatar; bu kapasite Memory'de birikir
ama ekonomik olarak yönetilmezse (hangi belleğe yatırım? ne amortize oldu? getiri ne?) kör
uçuştur. Decision Capital, karar-belleğini bir **sermaye hesabına** çevirir — Memory'nin *ne*
tuttuğunu değil, o stokun *nasıl değer ürettiğini* yönetir.

## Historical context — ve konumlandırma

| Öncül | Ne verdi | ENS ile örtüşme | ENS'in (dar) delta'sı |
|-------|----------|-----------------|------------------------|
| **Intellectual Capital** (Stewart 1997; Edvinsson-Skandia Navigator) | Bilgiyi sermaye olarak **stok + ölçüm** (112 metrik) | Stok+ölçüm çatısı **onlarda** | Yalnızca *akış/amortisman/ROI*; karar-belleğine daraltılmış; reuse×outcome |
| **Human capital** (Becker) | Sermaye = birikmiş kapasite | Birikim | Örgütsel karar kapasitesi |
| **Knowledge-based view** (Grant 1996) | Bilgi temel kaynak | Bilginin değeri | Commit-edilmiş karar bilgisi |
| **Ekonomik sermaye teorisi** | stok/akış/amortisman/ROI | **Dinamik yapı** | Karar-belleği varlığına uygulama |

**Dürüst delta:** Bilgiyi sermaye olarak ölçmek özgün değil (Stewart/Skandia zaten stok ve
metrik verir). ENS'in dar katkısı: stok'u değil **akış dinamiğini** (yatırım − amortisman),
**reuse-temelli ROI**'yi ve **memory yatırım kararını** modellemek — hepsi karar-belleğine özgü.

## Theoretical model

### 1. Stok (= Memory; Capital'in kendisi değil)
Referans için: stok, Company Memory'deki learning-taşıyan kararların değeridir.
`stok = Σ_d value(d)·salience(d)`, `value(d)=|Learning(d)|·attribution_confidence(d)`. **Bu
stok Memory'dir; Decision Capital stoku değil, aşağıdaki dinamiği yönetir.**

### 2. Akış: yatırım − amortisman (ayırt edici içerik #1)
```
ΔCapital = yatırım − amortisman
yatırım(t)   = Σ yeni ratified learning değeri           (ENS-2004 akışı)
amortisman(t)= Σ salience sönümü × value                 (context değişim hızı; ENS-2003)
```
Memory *ne tuttuğunu* söyler; Capital *ne kadar hızlı biriktiğini ve eridiğini* söyler — bir
akış büyüklüğü, statik stok değil.

### 3. Getiri: reuse ROI (ayırt edici içerik #2)
```
ROI = Σ (Purpose-tipi-içi reuse'un düşürdüğü InfoNeed / iyileştirdiği outcome) / bakım maliyeti
```
Sermayeyi Memory'den ayıran budur: getiri, yalnızca *kullanıldığında* ve *tip-içinde* doğar.
Hiç reuse edilmeyen bellek = ölü sermaye (amortize olur). Bu, "hangi belleğe yatırım/hangisini
sıkıştır" **yatırım kararının** (ayırt edici içerik #3) temelidir.

## Implications
- **Enterprise IQ ile doğru ilişki:** yüksek reuse-getirili Capital = geçmişten öğrenen örgüt.
- **Memory yatırım kararı:** ROI, neyi saklamaya/sıkıştırmaya değdiğini yönlendirir (Memory tek
  başına bunu söylemez).
- **Devir riski:** kilit Owner'lar giderse hangi context-indeksli sermaye risk altında, ölçülür.

## Relationships
- **→ Company Memory (ENS-2003):** stok + salience (amortisman); Capital onun *dinamiğini* yönetir.
- **→ Learning (ENS-2004):** yatırım akışının kaynağı; ROI atfı buraya (R2) bağlı.
- **→ Decision (ENS-2001):** birim commit-edilmiş karar; reuse Purpose-tipi içi.
- **→ Entropy/Gravity:** düşük Capital → yüksek Entropy (ortak prior yok) + yüksek Gravity açığı.

## Examples
**Yatırım:** tedarikçi başarısızlığından güçlü öğrenim → yüksek value, sermayeye eklenir.
**Getiri (tip-içi):** sonraki tedarikçi kararlarında reuse → InfoNeed düşer = ROI. Ama *farklı*
bir Purpose-tipinde (ör. fiyatlandırma) bu sermaye işe yaramaz — context-indeksli.
**Amortisman:** pazar tümüyle değiştiyse eski pazar-giriş öğrenimi erimiş sermaye — salience düşük.

## Laws
Yeni yasa değil, bir **sermaye dinamiğidir**; Entropy/Gravity ile fizik üçlüsünü tamamlar:
düşük Capital → yüksek Entropy ve yüksek Gravity açığı. Üçü birbirini belirler.

## Failure conditions (Anayasa Madde X)
- **ROI atfı R2'ye zincirli (en ciddi).** Getiri, reuse'un iyileştirdiği outcome atfını ister
  (ENS-2004); atıf zayıfsa ROI tahmindir. Nicel katmanın ortak kaderi.
- **Amortisman oranı = context değişim hızı**, bilinmezse sermaye abartılır/küçümsenir.
- **Transfer sınırı (tanımda kabul edildi).** Context-indekslilik, "capital" adının
  fungibility vaadini sınırlar; tipler arası transfer yoktur.
- **Survivorship (zincirli).** Yalnızca saklanan kararlar sayılır; ENS-2003 retention yanlıysa
  sermaye yanlı.

## SKR-015'e yanıt
| Talep | Karşılandığı yer |
|-------|------------------|
| 1. IC delta'sını daralt | §Historical (yalnızca akış/amortisman/ROI; stok+ölçüm onlarda) |
| 2. Capital'i Memory'den ayrıştır | §Definition + §Model 1-3 (stok=Memory; Capital=akış/ROI/yatırım) |
| 3. Transfer sınırını tanıma taşı | §Definition (context-indeksli, Purpose-tipi içi getiri) |

---

*Decision Capital, karar-belleğinin stok'u değil, o stokun akış-amortisman-getiri hesabıdır:
learning ile birikir, staleness ile erir, tip-içi reuse ile getiri üretir — context-indeksli,
metafor değil.*
