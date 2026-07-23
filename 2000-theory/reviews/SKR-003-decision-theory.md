---
id: SKR-003
type: skeptic-review
origin: ENS-2001
depends_on: [ENS-2001]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-003 — ENS Decision Theory (ENS-2001) Saldırısı

## Verdict
**wounded.** Klasik decision theory çakışması (§Historical context) dürüstçe ve doğru
çözülmüş — bu iyi ve SKR-001 dersinin içselleştirildiğini gösteriyor. Ama iki büyük prior
art anılmamış ve teorinin kendi işaret ettiği en derin sorun (individuation) çözümsüz
bırakılmış, üstelik literatür bu sorunun *lehine* değil *aleyhine* delil sunuyor. Üç talep
karşılanmadan `2000-theory`'nin Külliyat'a girmesi (canon:true) mümkün değil.

## Bulgu 1 — Yaşam döngüsü özgün değil (anılmamış prior art)

§Theoretical model 2'deki lifecycle (Framing → Contextualization → Reasoning → Commitment →
Enactment → Measurement → Learning → Memory), iki yerleşik modelin birleşimidir ve ikisi de
anılmamış:

- **Herbert Simon (1960), *The New Science of Management Decision*** — kararı üç faza böler:
  **intelligence → design → choice.** ENS'in Framing/Contextualization (intelligence),
  Reasoning (design), Commitment (choice) fazları bunun neredeyse birebir yeniden
  türetilmesidir.
- **Mintzberg, Raisinghani & Théorêt (1976), ASQ** — 25 stratejik kararın alan çalışması;
  **identification → development → selection** fazları + rutinler. ENS lifecycle'ı bununla
  da örtüşür.

ENS'in bu ikisine kattığı delta — **Measurement → Learning → Memory** kapanışı (Expected vs
Actual + Company Memory) — savunulabilir ve gerçekten ayırt edicidir (Simon/Mintzberg
döngüyü *kapatmaz*). Ama bu delta, ancak prior art açıkça anılıp fark belirtilirse bir
katkıdır; şu anki metin döngüyü sıfırdan icat ediyormuş gibi sunuyor (Anayasa Madde VI).

## Bulgu 2 — "Decision as first-class object" zaten bir OMG standardı (en ağır)

§Definition ve §Motivation, kararı "birinci-sınıf nesne / atom" yapmayı ENS'in ayırt edici
hamlesi gibi sunuyor. **Bu tam olarak DMN'dir (Decision Model and Notation, OMG standardı)**
ve arkasındaki **Decision Management Manifesto** şunu açıkça söyler: *"Kararlar, tıpkı
süreçler ve veri gibi, birinci-sınıf nesnelerdir; iş terimleriyle tanımlanmalı, modellenmeli,
gözden geçirilmeli ve yönetilmelidir."* DMN'in Decision Requirements Diagram'ı ve decision
table'ları vardır; bir XML şeması ve organizasyonlar arası paylaşım hedefi vardır — yani ENS'in
"kararı standart bir nesne yap" iddiası, bir OMG *standardı* olarak zaten mevcuttur. Ayrıca
**Architecture Decision Records (ADR)** kararın *neden*'ini bir domain için zaten yakalar.

Bu, SKR-001'deki kusurun bu belgede nüksetmesidir: mevcut bir standardı yeniden adlandırma
riski. **Ancak** savunulabilir bir delta var ve belirtilmeli:
- DMN, *operasyonel/kural-tabanlı, tekrarlanabilir* kararları modeller ("nasıl karar
  verilir" — deterministik mantık). Memory-of-why, outcome ölçümü ve learning **yoktur**.
- ENS Decision Object; *tekrarlanamayan, stratejik* kararları da kapsar, `Expected/Actual
  Outcome` + `Learning` + `Memory Links` taşır ve explainability'yi invariant yapar.

Yani ENS ≠ DMN; ama ENS-2001 bunu **açıkça** söylemeli, yoksa "DMN'in LLM'li yeniden
paketlemesi" itirazına savunmasızdır.

## Bulgu 3 — Individuation çözümsüz, üstelik literatür aleyhte (en derin)

Belge, individuation'ı en ciddi risk olarak dürüstçe işaretliyor (§Failure conditions) —
bu takdir edilir. Ama iş bununla bitmez: **Mintzberg (1976), stratejik kararların
kesintilere uğradığını, döngülere girdiğini ve net biçimde sınırlanamadığını ampirik olarak
gösterdi.** Yani "kararlar transaction gibi ayrık değildir" yalnızca bir kaygı değil,
belgelenmiş bir bulgudur. Bu, atom iddiasına doğrudan basınç uygular:

- Sınırı olmayan bir şey atom olamaz — sayılamaz, ölçülemez (Decision Entropy/Gravity),
  grafiklenemez (Decision Graph). Individuation ölçütü **yoksa**, üzerine kurulacak tüm
  nicel yapı (R1) temelsizdir.

Teori, bir **işlevsel individuation ölçütü** üretmek zorundadır (ör. bir Owner'ın tek bir
Purpose'a karşı yaptığı, ayrı bir Commitment olayıyla mühürlenen taahhüt = bir karar), ya da
atom iddiasını yalnızca **mühürlenebilir (commit edilebilir) kararlarla** sınırlamalıdır.
İkincisi, ENS-1000 §VII'deki "atfedilebilir sınıf" daraltmasıyla tutarlı olurdu.

## Sahibine talepler (kapıyı geçmek için)

1. **Lifecycle'ı Simon (1960) ve Mintzberg (1976)'ya krediyle konumla;** ENS'in delta'sını
   (Measurement→Learning→Memory kapanışı) açıkça belirt.
2. **DMN / Decision Management Manifesto / ADR karşısında konumlan;** ENS Decision Object'in
   DMN'den farkını (memory-of-why, outcome/learning, tekrarlanamayan kararlar, explainability
   invariant) yaz.
3. **İşlevsel individuation ölçütü ver** ya da atom iddiasını commit-edilebilir/atfedilebilir
   kararlarla sınırla. Bu olmadan Decision Entropy/Gravity/Capital (R1) tanımsız kalır.

## İç tutarlılık
Klasik decision theory ayrımı (§Historical context) doğru ve tutarlı. Özyineleme bölümü
(Beer'e atıfla) sağlam. Terminoloji sözlükle (ENS-4000) uyumlu. Çelişki bulunmadı; sorun
eksik konumlandırma ve çözülmemiş individuation.

## Kaynaklar
- Simon, H. A. (1960). *The New Science of Management Decision.* — intelligence/design/choice.
- Mintzberg, H., Raisinghani, D. & Théorêt, A. (1976). The Structure of "Unstructured"
  Decision Processes. *Administrative Science Quarterly*, 21(2). — identification/development/
  selection; kararların sınırlanamazlığı.
- OMG, *Decision Model and Notation (DMN)*; *Decision Management Manifesto* — "decisions as
  first-class objects."
- Nygard, M. — Architecture Decision Records (ADR): kararın *neden*'inin kaydı.
