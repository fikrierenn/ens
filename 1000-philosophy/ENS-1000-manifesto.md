---
id: ENS-1000
title: ENS Manifestosu
type: philosophy
canon: true
origin: ENS-0000 §I, §III
depends_on: [ENS-0000]
referenced_by: [ENS-4000]
principles: [P1, P2, P3, P4, P5, P6, P7, P8]
status: ratified
owner: ens-philosopher
version: 0.2.0
last_reviewed: 2026-07-23
failure_conditions: stated
skeptic_review: SKR-002
---

# ENS Manifestosu

> Anayasa (ENS-0000) değişmez ilkeleri **sıralar**. Bu manifesto onları **savunur** ve
> ENS'in önceki bilime göre gerçek katkısını dürüstçe konumlar. Bir iddia belgesidir;
> kendi yanılma koşullarını (§XI) taşır.
>
> **v0.2 notu:** Bu sürüm, [SKR-001](reviews/SKR-001-manifesto.md) skeptic saldırısına
> yanıttır. v0.1 çekirdek sezgiyi "yeni bir kavrayış" gibi sunuyordu; skeptic bunu haklı
> olarak çürüttü. v0.2, sezginin **eski** olduğunu kabul eder ve ENS'in katkısını yeni bir
> yere — inşa edilebilirliğe ve standartlaşmaya — taşır. §XII, her SKR-001 talebini nerede
> karşıladığımızı gösterir.

---

## I. Eski bir soru

"Bir şirket temelde ne yapar?" sorusuna "karar verir" yanıtı **yeni değildir.** Herbert
Simon karar vermeyi yönetimin merkezine koydu ve bounded rationality'yi tanımladı. Cyert
& March (1963) firmayı, çatışan hedefli bir koalisyonun *karar verme sistemi* olarak
modelledi. Stafford Beer (1972) organizasyonu, geri bildirimi izleyen ve kendini düzelten
bir *beyin* gibi tasarladı. Walsh & Ungson (1991) organizational memory'yi "geçmişten
bugünkü kararlara uygulanabilen saklanmış bilgi" olarak tanımladı.

Yani "şirket bir karar üreten bilişsel sistemdir" önermesi ENS'in keşfi değil, **miras
aldığı zemindir.** Bunu saklamak sahtekârlık olur (Anayasa Madde VI). ENS bu zemini kabul
eder ve şunu sorar: *bu doğruysa ve altmış yıldır biliniyorsa, neden hâlâ inşa edilmedi?*

## II. Tez (miras alınan, keskinleştirilen)

Bir şirketin birincil çıktısı transaction değil **karardır**; transaction, kararın
gözlemlenebilir kalıntısıdır (P1). Şirket, context alan, reasoning yapan, decision üreten,
sonucu ölçüp learning çıkaran ve memory'ye yazan bir bilişsel döngüdür. Bu döngünün
kalitesi, organizasyonun zekâsının tavanıdır.

ENS'in tezi bu değildir — bu, Simon-March-Beer'in tezidir. **ENS'in tezi şudur:** bu
döngü, tarihte ilk kez, bir *mühendislik yapıtı* olarak somutlaştırılabilir hale geldi;
ve somutlaştırıldığında, üzerine başkalarının inşa edebileceği bir *standarda* dönüşür.

## III. Öncüller ve Delta

ENS neyin üzerine kurulur ve ne ekler — açıkça:

| Öncül | Ne verdi | ENS ile örtüşme | ENS'in delta'sı |
|-------|----------|-----------------|-----------------|
| **Simon; Cyert-March (1963)** | Firma = karar sistemi; bounded rationality | P1, P2'nin kökü | Kararı *çalışan bir yapıt* (event-sourced aggregate) yapmak; teoriyi kod içinde kanıtlamak |
| **Beer, VSM (1972)** | Sibernetik biliş; feedback, self-regulation; System 4 intelligence | Katmanlı biliş, "nervous system" metaforu | Beer bir *teşhis modeli* verdi; ENS bir *inşa standardı* (numaralandırma, traceability, Külliyat) verir |
| **Walsh-Ungson (1991)** | Organizational memory: *ne*'nin saklanması | Company Memory (P3) | *Neden*'in ve *ölçülen sonucun* saklanması; bellek + learning döngüsünün kapatılması (P4) |
| **Gartner Decision Intelligence** | Ticari DIP pazarı; decision lifecycle, explainable models | Decision Engine, explainability | ENS bir *ürün pazarı* değil, teknolojiden bağımsız bir *disiplin/standart*; explainability'yi opsiyon değil **invariant** yapar (P6) |

**Delta özeti:** ENS özgün bir *teori* iddia etmez. Özgünlük iddiası üç noktadadır:
(1) önceki bilimi tek bir **inşa edilebilir mühendislik disiplinine** sentezlemek,
(2) bunu bir **standart** olarak paketlemek (§IV), (3) explainability ve memory-of-why'ı
**pazarlıksız invariant**'lara yükseltmek. Bunlar bilimsel keşif değil; mühendislik
katkısıdır — ve öyle adlandırılır.

## IV. ENS ne tür bir katkıdır

**ENS bir mühendislik disiplinidir, yeni bir doğa yasası değil.** TCP/IP yeni bir fizik
keşfetmedi; paket anahtarlamayı bir *standarda* çevirdi. DDD yeni bir epistemoloji
bulmadı; modelleme pratiğini bir *disipline* çevirdi. ENS de aynı sınıftadır: bilineni
inşa edilebilir ve öğretilebilir kılar.

Bu kalibrasyon önemlidir çünkü fazla iddia (ENS = yeni bilim) skeptic tarafından haklı
olarak çürütülür; doğru iddia (ENS = yeni mühendislik disiplini + standart) savunulabilir
ve yine de büyüktür.

## V. Neden şimdi — gerçek delta

Simon-March-Beer'in tezi altmış yıl inşa edilmedi çünkü dört varsayım onu imkânsız
kılıyordu. Dördü de son yıllarda çöktü; ENS'in fizibilite argümanı budur:

1. **"Akıl yürütme pahalıdır."** Reasoning modelleri marjinal maliyeti düşürdü — artık her
   karara gerekçe eşlik edebilir.
2. **"Context birleştirilemez."** Knowledge Graph + embedding + semantic search, dağınık
   context'i tek sorgulanabilir yüzeyde birleştirir.
3. **"Bellek geçicidir."** Kalıcı long-term memory, *neden*'i insan kafasından çıkarıp
   organizasyona yazar.
4. **"Açıklama üretilemez."** Öneri, Why/Why-not/Confidence/Evidence ile üretilebilir —
   ama yalnızca *sadıksa* değerlidir (§VIII).

Kıt kalan tek kaynak: **attention** (P5).

## VI. Ölçülebilir yapılar (metafor değil, provisional metrik)

Decision Capital/Entropy/Gravity, şu an sözlükte `provisional`'dır. Skeptic haklı: ölçüm
verilmezse bunlar metafordur ve Külliyat'a giremez (Madde VI). Faz 1'de kanıtlanmak üzere,
her biri için ilk-kesim ölçülebilir bir tanım öneriyoruz. Bu tanımlar iddiadır, sonuç
değil; `ens-skeptic` ve Faz 1 teorisi bunları sınayacak.

- **Decision Entropy** — kararlar arası tutarsızlığın ölçüsü. *İlk-kesim metrik:* benzer
  context'e sahip kararların sonuçları/gerekçeleri arasındaki varyans (context kümelenmesi
  üzerinden). Ölçülemezse "law" değil "gözlem" olarak etiketlenir.
- **Decision Gravity** — bir kararın çektiği context miktarı. *İlk-kesim metrik:* karara
  bağlanan Evidence düğüm sayısı × stake ağırlığı. Yanlışlanır: yüksek-stake kararların
  düşük context ile alındığı ölçülürse.
- **Decision Capital** — geçmiş kararlardan biriken, yeniden kullanılabilir değer. *İlk-kesim
  metrik:* ölçülen sonucu pozitif olan ve sonraki kararlarca fiilen geri getirilen (Memory
  Links) karar sayısı.

## VII. Attributability — Learning'in varlık koşulu

Skeptic'in en öldürücü itirazı: sonuç karara atfedilemezse, Learning (P4) boş bir vaattir.
Bunu ciddiye alıyoruz ve ENS'in kapsamını buna göre **daraltıyoruz**:

- ENS, learning iddiasını yalnızca **atfedilebilir karar sınıflarına** uygular: sonucu,
  makul bir zaman penceresinde ve kabul edilebilir confounding ile karara bağlanabilen
  kararlar (ör. fiyatlandırma, stok, tedarikçi seçimi). Atfedilemeyen kararlarda ENS
  *learning* iddia etmez; yalnızca memory ve explainability sağlar.
- Atfedilebilir sınıflarda bile, atıf **counterfactual temellidir** (karşı-olgusal: "başka
  alternatif seçilseydi ne olurdu") ve belirsizliğiyle birlikte raporlanır — sahte kesinlik
  üretmez.
- Bu, bir zayıflık değil, dürüst bir sınırdır. LAW-LEARNING, ancak ölçümün mümkün olduğu
  yerde geçerlidir; ENS bu koşulu gizlemez, açıkça işaretler.

## VIII. Explainability — sadakat (faithfulness) şartı

v0.1'de §IV ("açıklama üretilebilir") ile §VII ("açıklama post-hoc olabilir") çelişiyordu.
Çözüm: **P6, makullük (plausibility) değil, sadakat (faithfulness) ister.** Bir açıklama,
kararı fiilen üreten Evidence ve reasoning izine bağlı değilse, ENS için geçerli bir
açıklama değildir — makul görünmesi yetmez. Sadakat ölçülebilir olmalıdır (ör. gerekçede
atıf yapılan Evidence'ın karar fonksiyonunu gerçekten etkilediğinin doğrulanması). Sadakat
gösterilemiyorsa, çıktı **black-box** sayılır ve reddedilir (Madde VI). Böylece P6 yapısal
kalır, kozmetik olmaz.

## IX. Paradigma kayması

| Boyut | ERP dünyası | ENS dünyası |
|-------|-------------|-------------|
| Atom | Transaction / kayıt | **Decision** (P1) |
| Değer birimi | Data | **Context** (P2) |
| Zaman modeli | Şimdiki durum | Karar + sonuç + **memory** (P3) |
| Çıktı | Rapor | Sadık, gerekçeli **recommendation** (P6, P7) |
| İyileşme | Yazılım sürümü | Atfedilebilir sonuçtan **learning** (P4, §VII) |
| Darboğaz | İşlem hızı | **Attention** (P5) |
| İnsanın rolü | Veri girer | Karardan **sorumludur** (P7) |

Bu bir ürün yükseltmesi değil, bir mühendislik disiplininin doğuşudur. Yine de "AI ERP"
değildir (Madde VI): ERP kayıt katmanı, ENS biliş katmanıdır.

## X. Ne inşa ederiz

Tezden mimari **türetilir**, uydurulmaz (Madde IX): context için Knowledge Graph; *neden*
için Company Memory; hipotez/alternatif için Reasoning Engine; ön-sınama için Simulation
Engine; öneri (emir değil) için Decision Engine; atfedilebilir sonucu ölçüp belleği
güncelleyen Learning Engine. Tek amaç: **organizasyonun akıl yürütmesini iyileştirmek.**
Etmiyorsa, inşa edilmez.

## XI. Bu manifesto hangi koşullarda yanılır

- **Atfedilebilir karar sınıfı pratikte çok darsa** (§VII), learning katmanı marjinal
  kalır ve ENS, "memory + explainability" katmanına küçülür — büyük ama daha mütevazı bir
  iddia.
- **Sadakat ölçümü güvenilir yapılamıyorsa** (§VIII), P6 çöker ve ENS black-box'a düşer.
- **Context'in maliyeti değerinden yüksek kalırsa**, Decision Gravity ekonomik olarak
  gerçekleşmez.
- **Karar kalitesindeki açık küçükse** (LAW-ENTROPY/CONTEXT), ENS teorik olarak doğru ama
  ekonomik olarak gereksiz olur.
- **Mühendislik sentezi, prior art'a göre yeterli delta üretmezse**, ENS bir standart
  olarak değil, yalnızca iyi bir yeniden-paketleme olarak kalır.

## XII. SKR-001'e yanıt

| SKR-001 talebi | Karşılandığı yer |
|----------------|------------------|
| 1. Yenilik/konumlandırma bölümü | §I (eski soru), §III (Öncüller ve Delta tablosu) |
| 2. Disiplin iddiasını kalibre et | §II, §IV (mühendislik disiplini + standart) |
| 3. Metaforları operasyonel kıl/geri çek | §VI (her yapıya provisional metrik) |
| 4. Attributability'yi cevapla | §VII (atfedilebilir sınıf + counterfactual + açık sınır) |
| 5. P6 faithfulness gerilimini çöz | §VIII (sadakat şartı, ölçülemezse black-box) |

Bu sürüm yeniden `ens-skeptic`'e sunulur; verdict `wounded`'dan `survives`'a çıkarsa Faz 0
kapısı açılır (Anayasa Madde VII, X).

---

*Şirketin karar ürettiği yeni bir fikir değildir. Yeni olan, bu kararı görünür,
hatırlanabilir, sadık biçimde açıklanabilir ve — mümkün olduğu yerde — öğrenilebilir kılan
bir mühendislik disiplininin, ilk kez inşa edilebilir olmasıdır.*
