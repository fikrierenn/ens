---
id: RFC-6003
type: rfc
canon: false
status: draft
owner: fikri-eren            # G5: fiilî yazar (SKR-048 D-10). v0.1.0 yanlış yazıyordu
version: 0.2.0
last_reviewed: 2026-07-27
depends_on: [ENS-0000, GOV-000, GOV-010, RFC-6001, RFC-6002]
referenced_by: []
skeptic_review: [SKR-048, ARCH-0001]
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
| Bugüne kadar yazılmış SKR | **46** |
| Bunlardan `ens-skeptic` tarafından yazılan | **46** — yani **%100** |
| Ethical boyutunda yapılmış doğrulama | **0** |
| `canon: true` yapıtlardan çok-boyutlu zinciri olan | **0** |

> **⚠️ v0.1.0 burada 45 diyordu — ve bu, RFC'nin kendi fc-1'ini tetikledi.** RFC
> `find . -name 'SKR-*.md'` komutunu **mekanik doğrulanabilir bir failure condition olarak
> ilan etti ve çalıştırmadı** (`SKR-048`). Çalıştırılınca eksik olan bulundu:
> `.claude/rules/SKR-046-tier3-discipline-rules.md` — `reviews/` dışındaki **tek** SKR, ve
> aynı oturumda üretilmiş. Oran (**%100**) değişmedi; ama *"kendi ilan ettiğin testi
> çalıştırmamak"* bu deponun tekrarlayan kusurudur ve burada tekrarlandı.

Yani boyut çeşitliliği bir **kural olarak var, pratik olarak yok.** Ve bunun nedeni ihmal
değil **kadro**: `governance/roles.md` bugün *"Engineering/Business/Ethical Validator,
Governance body: fazı gelince"* diyor. Rol atanmamışsa boyut çalışamaz.

### 1.1 ⛔ v0.1.0'ın ampirik ayağı ÇÜRÜTÜLDÜ — ve yerine gerçeği kondu

**v0.1.0 şunu iddia ediyordu:** ENS-2003'teki `c` çift-sayım hatası iki *scientific* turdan
(SKR-040, SKR-041) geçti, `AUDIT-WAVE2-FIDELITY` yakaladı, ve o *"fiilen bir **engineering**
denetimidir"*.

**Yanlıştı.** `SKR-048` belgenin kendi sınır beyanını gösterdi
(`AUDIT-WAVE2-FIDELITY.md:12-16`):

> *"Bu context'te **shell/Bash aracı yoktu**; `dotnet build` ve `dotnet test`
> **ÇALIŞTIRILAMADI.** Aşağıdaki hiçbir bulgu bir test koşusuna dayanmıyor — hepsi
> **statik kaynak okuması + teori metni karşılaştırması**."*

`validation-framework.md:44`'ün engineering ölçütlerinin (inşa edilebilirlik, ölçek,
replaceability, test edilebilirlik) **hiçbirini** karşılamıyor. Dahası bulgunun kendisi bir
**teori** kusuruydu (`:236-239` — *"Yanlış olan kodun formülü değil, **ortogonallik
iddiasıdır**"*) ve hat `ens-skeptic`'in SKR-045'iyle sürdü.

> **Gerçek ayırt edici boyut değil, KAPSAMDI:** iki metnin yan yana okunması. Bu, RFC'nin
> **tek** ampirik kanıtıydı ve fc-2'yi lafzen tetikledi. Doğrulandı (oturum sahibi,
> `dosya:satır`, kural §3.5).

### 1.2 Yerine geçen kanıt — bu RFC'nin kendi doğrulama turundan

İronik olan şu: v0.1.0 sahte bir kanıt kullanırken, **gerçek kanıt onun kendi incelemesinde
üretildi.** Aynı iki belgeye (`RFC-6002` + `RFC-6003`) iki farklı boyut baktı ve
**birbirinin kaçırdığını buldu**:

| Boyut | Yakaladığı | Kaçırdığı |
|---|---|---|
| `ARCH-0001` — architectural | Atomiklik testi (6002 tek başına kabul edilirse G4 uyumu %0→~%40 **sahte** yükselir); R2'nin uygulanamazlığı; kabul sırasının tersliği | 45/46 sayım hatası; üç kanıt kusuru |
| `SKR-048` — scientific | §1.1'in çürük ampirik ayağı; sayım hatası; `constitutional`'ın şema-dışı kullanımı; §8'in zırhı | İki yapısal ölçüm |

**Ve ikisi de birbirini görmeden aynı yere vardı** (Ç-04 çatışmasının intra-doküman olduğu:
`validation-framework.md:29` ↔ `:51`).

Bu, v0.1.0'ın uydurduğundan **daha iyi** bir kanıttır çünkü: (a) bu depoda, bu hafta,
gözlemlenerek üretildi; (b) boyutların **etiketi** değil **çıktısı** farklıydı; (c) yakınsama
ve ayrışma **birlikte** gözlendi — G4'ün öngördüğü tam desen budur.

> **Dürüstlük sınırı:** bu tek bir vakadır (n=1). Bir eğilim değil, bir **varoluş
> kanıtıdır**: "farklı boyutlar farklı kusur bulur" önermesinin en az bir doğrulayıcı örneği
> vardır. v0.1.0'ın iddiası bundan fazlasını söylüyordu ve dayanağı yoktu.

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

## 8. Bu RFC'nin kendi yolu — ve v0.1.0'ın zırhının sökülmesi

En az bir bağımsız `ens-skeptic` turu + Madde XIV yordamı.

### 8.1 ⛔ v0.1.0'ın §8'i bir YANLIŞLANAMAZLIK ZIRHIYDI — kabul ediliyor

v0.1.0 şöyle yazıyordu:

> *"Boyut çeşitliliğini savunan bir belgenin kendisi de tek boyuttan doğrulanacaktır…
> Bu, RFC'nin kendi tezinin en iyi kanıtıdır."*

`SKR-048` bunu **zırh** olarak teşhis etti ve haklı. Cümle iki tezi karıştırıyordu:

| | Tez | Durumu |
|---|---|---|
| **T1** | Kadro eksik (boyut rolleri atanmamış) | **Önemsizce doğru** — ROSTER'a bakmak yeter |
| **T2** | Boyut çeşitliliği **korur** (kusur yakalar) | **Asıl iddia** — kanıt ister |

v0.1.0, T1'in gözlemini T2'nin kanıtı sayıyordu. Ve yapı kendini yanlışlanamaz kılıyordu:
tek boyuttan geçerse *"tezim doğru"*, çok boyuttan geçerse *"kural işledi"* — **her iki
sonuç da RFC'yi doğruluyorsa Madde X ihlali vardır.**

### 8.2 Öngörü CANLI SINANDI — ve yanlış çıktı

v0.1.0 *"tek boyuttan doğrulanacak"* diye **öngörüde** bulundu. Öngörü **çürüdü**: bu RFC
iki boyuttan geçti (`SKR-048` scientific + `ARCH-0001` architectural).

Ama ilginç olan şu: **tam da o çürüme, T2'ye ilk gerçek veriyi verdi** (§1.2). Yani:

> Öngörü yanlış çıktı, **tez ayakta kaldı** — ve ayakta kalmasının nedeni öngörünün
> tutması değil, **tutmaması** oldu.

Bu, zırhın sökülmüş hâlidir. T2 artık yanlışlanabilir bir biçimde duruyor: *"iki farklı
boyut aynı belgeye baksa **aynı** kusurları bulsaydı, T2 için kanıt olmazdı."* Bulmadılar
(§1.2 tablosu). Bir dahaki turda bulurlarsa, bu RFC zayıflar.

### 8.3 Kalan borç

`ARCH-0001:50` 45/45 sayımını **"DOĞRU"** damgaladı ama aynı satırda *"yeniden sayılmadı,
**DEVRALINDI**"* dedi (`SKR-048`). Bu, `work-protocol.md` **§3.5** ihlalidir — ve hata
zincir boyunca **üç belge** taşındı: `SCAN-03` → `RFC-6003` → `ARCH-0001`. §3.5'in var olma
sebebi tam olarak budur; kural yazıldı, aynı oturumda ihlal edildi.
