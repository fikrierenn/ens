---
id: RFC-6003
type: rfc
canon: false
status: draft
owner: ens-philosopher
version: 0.1.0
last_reviewed: 2026-07-27
depends_on: [ENS-0000, GOV-000, GOV-010, RFC-6001, RFC-6002]
referenced_by: []
skeptic_review: pending
failure_conditions: stated
principles: [P6, P7]
---

# RFC-6003 — Boyut Sözlüğü ve Validator Kadrosu

> RFC-6002 *"kaç boyut"* sorusunu çözer. Bu RFC *"hangi boyutlar, kim yürütür"* sorusunu
> çözer. İkisi ayrıdır çünkü farklı yanılma kipleri taşırlar: RFC-6002 yanlış olursa kapı
> yanlış yerde durur; bu RFC yanlış olursa **kapı hiç açılmaz.**

## 1. Problem: bir kural var, uygulayacak kimse yok

G4 *"farklı boyutlardan ≥2 bağımsız validator"* istiyor. Ölçüm:

| Ölçüt | Değer |
|---|---|
| Bugüne kadar yazılmış SKR | **45** |
| Bunlardan `ens-skeptic` tarafından yazılan | **45** — yani **%100** |
| Ethical boyutunda yapılmış doğrulama | **0** |
| `canon: true` yapıtlardan çok-boyutlu zinciri olan | **0** |

Yani boyut çeşitliliği bir **kural olarak var, pratik olarak yok.** Ve bunun nedeni ihmal
değil **kadro**: `governance/roles.md` bugün *"Engineering/Business/Ethical Validator,
Governance body: fazı gelince"* diyor. Rol atanmamışsa boyut çalışamaz.

### 1.1 Bunun bedeli ölçüldü — ve kanıt Külliyat'ın kendi kaydında

ENS-2003'teki `c` çift-sayım hatası (D-5):

- **SKR-040**'tan geçti — bağımsız, scientific.
- **SKR-041**'den geçti — bağımsız, scientific.
- Ancak `AUDIT-WAVE2-FIDELITY` yakaladı — ki o fiilen bir **engineering** denetimidir.

İki bağımsız *aynı boyuttan* tur, hatayı görmedi. **Farklı** boyuttan bir bakış, gördü.
G4'ün neden "farklı boyutlardan" dediğinin ampirik kanıtı burada, deponun kendi tarihinde.

## 2. Ç-04 — Ethical Validation ne zaman aktif?

| Taraf | Der ki |
|---|---|
| `.claude/standards/validation-framework.md` | **tüm fazlar** |
| `governance/roles.md` | **fazı gelince** |

**Çözüm: `validation-framework.md` kazanır.** Gerekçe yordamsaldır: `roles.md` boyut
otoritesi olarak **onu gösterir**; bir belge, atıf yaptığı kaynağı geçersizleştiremez.
(Aynı yordam RFC-6002 §3'te `roles.md` ↔ GOV-000 için de uygulandı.)

Maddi gerekçe daha da güçlü: ENS Faz 3-4'te **agent runtime ADR'leri** üretti — bounded
autonomy, tool authorization, insan onayı kapıları. **P7 (Sorumluluk insandadır) hiç bağımsız
etik incelemeden geçmedi.** Etik doğrulamanın ertelenmesi için gösterilebilecek en kötü faz,
tam da bu fazdı.

> **Sonuç:** 0 ethical SKR **meşru erteleme değil, kayıtlı borçtur** (ROADMAP G-27).

## 3. Ç-05 — Kurucu (`constitutive`) yol G4'ten muaf mı?

**Hayır.** Üç dayanak:

1. **RFC-6001 kendi metninde reddediyor** (`:175`): *"`constitutive` bir belgeyi doğrulamadan
   muaf tutmaz. Yalnızca **yanılma kipini** değiştirir."*
2. **Sessizlik ilga etmez.** RFC-6001 Madde IV'ü ve künye şemasını değiştirdi; GOV-000'i
   değiştirmedi. Değinilmeyen kural yürürlükte kalır.
3. **Emsal ters yönde.** RFC-6001'in kendi kabul zinciri — 3 skeptic turu (SKR-034 wounded →
   SKR-035 wounded → SKR-036 survives) + CEO-0002 hiza-onayı + STYLE-SIGNOFF şema-imzası —
   korpusun **en ağır** doğrulama yoludur. Kurucu yolu "hafif" ilan etmek, kendi en iyi
   emsaline karşı gerileme olurdu.

### 3.1 Gerçek boşluk: boyut sözlüğü eksik

Muafiyet yoksa, kurucu bir yapıt **hangi** iki boyuttan doğrulanacak? Doğal cevap
{ontology, constitutional} — ama **`constitutional` diye bir boyut tanımlı değil**
(`validation-framework.md` beş boyut sayar: Scientific · Ontology · Engineering · Business ·
Ethical).

İki seçenek, bu RFC **karara bağlamıyor, açıkça soruyor** (§7):
- **(a)** `constitutional` altıncı boyut olarak tanımlansın.
- **(b)** Kurucu yapıtlar için {Ontology + Scientific} eşlemesi yeterli sayılsın ve
  `constitutional` ayrı bir boyut olarak açılmasın.

## 4. Ö-07 — Ontology Validator, `ens-skeptic`ten ayrılsın

45/45 oranı tek başına yeterli gerekçedir: **tek rol, tek bakış açısıdır.** G4'ün "farklı
boyutlardan" şartı, farklı *turlar* değil farklı *lensler* ister.

Öneri: ROSTER'a ayrı bir **Ontology Validator** rolü. Doğal aday `ens-architect`
(ENS-4001/4010'un owner'ı olduğu için ontolojiye hâkim) — **ama kendi yapıtlarında
değil.** Bu kısıt G2'nin doğrudan sonucudur: owner, kendi yapıtını Canonical yapamaz.

> **Uyarı — bu öneri kendi kaçağını taşıyor.** Ontoloji boyutunun en yetkin aktörü, ontoloji
> yapıtlarının da owner'ıdır. Rolü ona verirsek yapıtlarının yarısında kullanılamaz;
> başkasına verirsek yetkinlik düşer. Bu gerilim çözülmemiştir ve gizlenmiyor.

## 5. Neden RFC-6002'den ayrı

| | RFC-6002 | RFC-6003 |
|---|---|---|
| Sorusu | Kapı **nerede** durur? | Kapıyı **kim** açar? |
| Owner | `ens-philosopher` + `ens-ceo` | `ens-philosopher` + `ens-ceo` (GOV-010 rol ataması) |
| Yanılma kipi | Kapı yanlış statüde konumlanır | Kural yazılır, uygulayan olmadığı için hiç işlemez |
| Değişen metin | GOV-000, maturity-model, canonical-process | `validation-framework.md`, ROSTER, GOV-010 |

CEO-0002'nin RFC-6001 üzerine düştüğü **kapsam-orantısı** gözlemi tek dev RFC'ye karşı
uyarır; ikiye bölme o uyarının uygulanmasıdır.

## 6. Failure conditions (Madde X)

Bu RFC **yanlıştır** eğer:

1. **45/45 sayımı yanlışsa.** Tüm §1 ona dayanır. Sayım
   `find . -name 'SKR-*.md'` + her dosyanın `owner:`/yazar alanı ile mekanik
   doğrulanabilir; yanlışsa RFC'nin ampirik ayağı çöker.
2. **D-5'in "engineering yakaladı" anlatısı yanlışsa** — yani `AUDIT-WAVE2-FIDELITY`
   fiilen scientific bir inceleme idiyse, §1.1'deki tek ampirik kanıt geçersizleşir ve
   boyut çeşitliliği yalnızca *teorik* bir gereklilik olarak kalır.
3. **Ayrı bir Ontology Validator atanır ama fiilen `ens-skeptic` ile aynı lensi
   kullanırsa.** O zaman ROSTER'da iki isim, pratikte tek bakış olur — kural sağlanmış
   *görünür*, korumaz. Bu, G4'ün kâğıt üzerinde kapatılıp gerçekte açık kalmasıdır ve
   mevcut hâlden **daha kötüdür**, çünkü kayıtlı borcu görünmez yapar.
4. **`constitutional` boyutu tanımlanır ama onu yürütecek rol atanmazsa** — Ç-04'ün
   ethical için ürettiği durumun aynısı yeni bir boyutta tekrarlanır.

## 7. Açık sorular

1. `constitutional` altıncı boyut mu, yoksa {Ontology + Scientific} eşlemesi mi? (§3.1)
2. Ontology Validator kim? `ens-architect`'in kendi yapıtlarındaki çıkar çatışması nasıl
   çözülür — yapıt bazında rotasyon mu, ayrı bir rol mü? (§4)
3. Ethical Validator kim? Bu rol ENS'te hiç materyalize edilmedi; `ens-ceo` mu üstlenir,
   yeni bir rol mü açılır?
4. Geçmişe dönük: `canon: true` olan ENS-1000 tek boyutlu bir zincire sahip (ROADMAP G-28).
   Yeni boyutlar atandıktan sonra geriye dönük tamamlama mı, yoksa "bu tarihten sonrası" mı?

## 8. Bu RFC'nin kendi yolu

En az bir bağımsız `ens-skeptic` turu + Madde XIV yordamı. **Ve bu RFC için özel bir
ironi vardır:** boyut çeşitliliğini savunan bir belgenin kendisi de tek boyuttan
doğrulanacaktır — çünkü savunduğu roller henüz atanmamıştır. Bu, RFC'nin kendi tezinin
en iyi kanıtıdır ve **gizlenmemektedir.**
