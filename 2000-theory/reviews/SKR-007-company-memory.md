---
id: SKR-007
type: skeptic-review
origin: ENS-2003
depends_on: [ENS-2003]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-007 — ENS Company Memory (ENS-2003) Saldırısı

## Verdict
**wounded.** Belge, önceki turların derslerini içselleştirmiş: CBR/Walsh-Ungson/Nonaka
proaktif konumlanmış, LAW-ORG-MEMORY gerilimi (salience/record ayrımı) zarifçe çözülmüş,
OC1 (exploration) ve OC3 (attribution yerelleştirme) kısmen ödenmiş. Ama üç gerçek açık var:
bir çözülmemiş döngü, bir mekanizmasız failure condition ve giderek sistemik hale gelen bir
borç. İkisi doc revizyonu, biri program-düzeyi yükümlülük. Karşılanmadan `canon:true` olamaz.

## Güçlü yönler (önce dürüstlük)
- **CBR konumu doğru.** Aamodt & Plaza (1994) "4 RE" (retrieve-reuse-revise-retain) döngüsü,
  ENS'in karar-getirme + learning + retain'iyle birebir; belge bunu saklamıyor. Delta
  (yapılı Decision Object + commitment-bireyleme + explainability invariant) savunulabilir.
  *Uyarı:* CBR'ın "case-base maintenance" alt-alanı zaten forgetting/silme politikalarını
  içerir; ENS'in "forgetting birinci-sınıf" delta'sı bu yüzden **daha dar** olmalı — gerçek
  fark "salience sönümle, kaydı asla silme (audit)" incelikli hamlesidir. Bunu net söyle.
- **LAW-ORG-MEMORY gerilimi çözümü sağlam.** "Neden'i unutma; superseded ayrıntının
  önceliğini sönümle" ayrımı, yasa ile P5'i uzlaştırıyor. Bu iyi bir teorik hamle.

## Bulgu 1 — Purpose-tipi taksonomisi tanımsız → gizli döngü (wounded sürücüsü)

§Model 2, retrieval'ı "benzer Purpose-tipi" üzerinden yapıyor ve SKR-006 OC2'yi (benzerlik
context'ten değil Purpose-tipinden) böyle ödediğini iddia ediyor. Ama **Purpose-tipi
taksonomisinin nereden geldiği hiç tanımlı değil.** İki kötü olasılık:
- Taksonomi kararlardan *öğreniliyorsa* → sınıflandırma için memory, memory'den getirme için
  sınıflandırma gerekir: SKR-005'te kırılan döngü **başka biçimde geri döner.**
- Taksonomi elle sabitse → ölçeklenmez ve yeni Purpose türlerini kaçırır.

OC2 ancak Purpose-tipi taksonomisinin **dairesel-olmayan** bir kaynağı verilirse ödenmiş
olur (ör. Purpose'un yalnızca niyet-ifadesinden, karar-sonucundan bağımsız türetilen bir
sınıflandırması).

## Bulgu 2 — Survivorship bias işaretli ama mekanizmasız

§Failure conditions, retention'ın "başarılı" kararlara meyledip memory'yi yanlılaştırmasını
(survivorship bias) en ciddi risk sayıyor — doğru. Ama exploration için bir mod verildiği
gibi (§Model 4), buna bir **mekanizma verilmemiş.** Başarısız kararların *neden*'ini korumak
bir retention *ilkesine* dönüşmeli (ör. "outcome ne olursa olsun, learning üreten her karar
saklanır; yalnızca learning'siz gürültü sönümlenir"). Aksi hâlde ENS-2002 relevance kestirimi
yanlılığı sistematik olarak miras alır.

## Bulgu 3 — R2 (attribution) artık sistemik yük (program yükümlülüğü)

Bu belge, attribution borcunu (R2 / ENS-1000 §VII) üçüncü kez taşıyor: **Learning (P4),
Context relevance (ENS-2002) ve şimdi Company Memory retention'ı** — üçü de "sonucu iyileştiren
/ ölçülebilir sonuçlu karar" kavramına, yani outcome'un karara atfına dayanıyor. R2 artık bir
dipnot değil, nicel katmanın **taşıyıcı kolonu.** Süresiz ertelenemez.

**Talep:** attribution'ı Faz 2'de adlandırılmış bir kavrama yükselt (ör. bir "Attribution /
Learning Model" ya da Enterprise Physics'in bir bölümü) ve ENS-2003, bu bağımlılığı açıkça
kabul etsin. Çözümü burada verilmesi gerekmez; ama "yakında ve şu adreste çözülecek" taahhüdü
verilmeli, yoksa Memory + Context + Learning üçlüsü kanıtlanmamış bir varsayımın üzerinde durur.

## Sahibine talepler (kapıyı geçmek için)
1. **Purpose-tipi taksonomisine dairesel-olmayan bir kaynak ver** (OC2'yi gerçekten öde).
2. **Survivorship bias'a bir retention mekanizması ver** (başarısız kararların neden'ini koru).
3. **R2/attribution'ı adlandırılmış bir Faz 2 kavramına yükselt** ve bağımlılığı kabul et;
   ayrıca CBR forgetting delta'sını daralt.

## İç tutarlılık
Memory Graph, Decision Theory düğümleri ve Context relevance ile tutarlı. Salience/record
ayrımı çelişki üretmiyor. Sorun eksik tanım (taksonomi), eksik mekanizma (bias) ve ödenmemiş
sistemik borç (attribution).

## Kaynaklar
- **Aamodt, A. & Plaza, E. (1994).** Case-Based Reasoning: Foundational Issues... *AI
  Communications*, 7(1). — 4 RE döngüsü (retrieve-reuse-revise-retain).
- Walsh & Ungson (1991); Nonaka & Takeuchi (1995); March (1991) — önceki SKR'ler.
