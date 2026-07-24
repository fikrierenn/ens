---
id: SKR-036
type: skeptic-review
origin: RFC-6001
depends_on: [RFC-6001, SKR-034, SKR-035, ENS-0000, ENS-4000, STD-METADATA-HEADER, STD-MATURITY-MODEL, STD-EVIDENCE-STANDARD]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
validation_dimension: constitutional
---

# SKR-036 — RFC-6001 (Constitutive Artifact Ayrımı) 3. Tur Saldırısı

**Bağımsızlık beyanı:** Bu inceleme, RFC-6001'i v0.2.0→v0.3.0'a düzelten `ens-philosopher`
çağrısından **ve** 1./2. tur SKR-034/SKR-035'i yazan çağrılardan tamamen ayrı, taze context'te
yapıldı (G2/G3 — yazar kendi düzeltmesini onaylayamaz). Görev: D6 (blocking) düzeltmesinin —
çift-yönlü invariant'ın tek-yönlü gerekli-koşula indirgenmesi + turnusolun birincil sınıflayıcı
ilan edilmesi — RFC'yi gerçekten sağlamlaştırıp sağlamlaştırmadığını, yoksa yeni bir çatlak mı
açtığını, **depodaki fiili künyeleri ve evidence bloklarını bağımsız okuyarak** sınamak. Tüm
künye/grep okumaları 2026-07-24 tarihiyle doğrulandı.

## Verdict

`survives` — D6 düzeltmesi bir yama değil, bir **geri çekilmedir**: fazla iddialı çift-yönlü
invariant, doğru olan küçük iddiaya (tek-yönlü gerekli-koşul) indirgendi ve sınıflama yükü
zaten SKR-035'in tutarlı bulduğu turnusola (Test A/B/C) devredildi. Governance çelişkisi
gerçekten kapandı; bağımsız korpus taraması RFC §12 tablosuyla birebir uyuşuyor; governance'ın
ampirik evidence boyutları (eng/ops/econ) fiilen **E0** olduğu için failure-condition #5'in
çöküş koşulu tetiklenmiyor; D7/D8 gerçekten kapalı. Çekirdek tez üç tur boyunca **değişmeden**
sağ çıktı ve döngü yamalı-sarmal değil, iddiaları hakikate doğru zayıflatan bir **yakınsamadır**
(SKR-001 dersi doğru uygulanmış). Üç bloke-etmeyen keskinleştirme kalıyor (S1: invariant'ın tek
yasak hücresini açıkça yaz; S2: `amends:` alanına STD-MATURITY-MODEL'i ekle — canon kuralının
üçüncü lokusu; S3: governance M1'ini "olumsal alışkanlık" değil "maturity-model.md tarafından
zorunlu kılınmış ama canon'u gate etmeyen" diye yeniden nitelendir). Hiçbiri Faz'ı durdurmaz;
RFC `ens-ceo` Madde XIV hiza incelemesine hazırdır.

## Yenilik incelemesi

Bu tur yeni prior-art getirmiyor; SKR-035 zaten Lakatos/Kelsen/Hart/Carnap atıflarını gerçek ve
doğru-yerleştirilmiş bulmuştu. Bağımsız kontrol: v0.3'te prior-art bölümü (§3) değişmedi, hiçbir
atıf bozulmadı, uydurma girmedi. Delta iddiaları (Searle üstüne dik doğrulama ekseni; Quine'a
Carnap-göreli analitiklik yanıtı; Lakatos hard-core = immutable-core) yerinde. Temiz.

## Görev-1: Gevşetme RFC'yi anlamsızlaştırıyor mu? (Karşı-örnek araması)

**Hayır — invariant testedilebilirliğini korudu; yalnızca rolü daraldı.** Tek-yönlü invariant
`constitutive:false ⇒ maturity taşır` (contrapositive: `maturity yok ⇒ constitutive:true`). Bu
kural **tam olarak bir künye hücresini yasaklar:**

| constitutive | maturity | invariant | durum |
|--------------|----------|-----------|-------|
| false | var | izinli | ENS-2001.., ENS-4001.. |
| false | **yok** | **YASAK** | (boş — kimse ihlal etmiyor) |
| true | var | izinli | governance (M1) |
| true | yok | izinli | ENS-0000, ENS-4000 |

RFC şemasını gerçekten çürütecek somut karşı-örnek: *`constitutive:false` ilan edilmiş, künyesinde
`maturity` alanı olmayan bir yapıt.* Korpusta böyle bir yapıt **yok** (bağımsız grep: `maturity`
taşımayan tek belgeler ENS-0000, ENS-4000 ve `STD-*` standartları — hiçbiri `constitutive:false`
adayı değil). Yani invariant **falsifiable ama şu an ihlal edilmemiş** — bu, boş/anlamsız değil,
sağlıklı bir invariant durumudur. "Artık her kombinasyon tutarlı sayılıyor" iddiası **yanlış**:
bir hücre hâlâ yasak.

**§7.2 muafiyeti boşaltıldı mı?** Hayır. §7.2 der ki `constitutive:true` yapıt canon'unu M5/Faz-4
kanıt zincirinden değil **ratifikasyondan** kazanır. Bu iddia `maturity` alanının varlığından/
yokluğundan **bağımsızdır** — canon yolu yalnızca yer değiştirdi (M-merdiveni → kurucu-tutarlılık
kapısı), kaldırılmadı. Governance canon:false→canon:true geçişi hâlâ bir kapıya (skeptic
tutarlılık incelemesi) tabi. Dolayısıyla muafiyet vacuous değil; gate hâlâ var, yalnızca türe
uygun.

**Dürüst sınır (S1):** İnvariant'ın ampirik ısırığı artık **minimaldir** — yasakladığı tek hücre
boş. RFC bunu "yalnızca tutarlılık-kontrolü" diyerek dürüstçe kabul ediyor (§4.2), ama metinde
bu tek yasak hücreyi (`constitutive:false + maturity yok = geçersiz`) **açıkça** yazmıyor;
yazsaydı invariant'ın hâlâ test-edilebilir kaldığı ilk bakışta görünürdü. Keskinleştirme, blocking
değil: test-edilebilirlik zaten turnusolla taşınıyor (aşağıda).

## Görev-2: Ayrıştırma çelişkiyi iki yerde mi saklıyor? (governance evidence sınaması)

**Hayır — ve bunu fiili evidence bloğunu okuyarak doğruladım.** Governance'ın (GOV-000/010/020/030)
künye evidence'ı bağımsız okundu:

```
evidence: {sci: E1, eng: E0, ops: E0, econ: E0}   (dördü de aynı)
```

STD-EVIDENCE-STANDARD'a göre boyutlar: **Engineering** ("inşa edildi mi"), **Operational**
("sahada işliyor mu"), **Economic** ("değer üretiyor mu") — yani "bu prosedür pratikte iyi karar
üretir" türü **ampirik-yeterlilik iddiaları** tam olarak bu üç boyutta yaşar. Governance'ta üçü de
**E0 (opinion, destek yok).** Yalnızca `sci:E1` (tek-vaka teorik-dayanak) dolu.

Bu, ayrıştırmayı **doğruluyor**, çelişkiyi saklamıyor: governance'ın evidence bloğu bir
ampirik-yeterlilik iddiası **yüklemiyor** (eng/ops/econ = E0). Test A (kaldır → roller/yetkiler
tanımsız → kurucu) `true` verir; Test C (sınanabilir ampirik-yeterlilik öngörüsü?) `hayır` verir —
çünkü ampirik boyutlar boş. Dolayısıyla governance `constitutive:true` **hem** M5/Faz-4
muafiyetini **hem** M1 etiketini çelişkisiz tutar: M1, STD-MATURITY-MODEL'e göre "künye tam +
prior-art + failure conditions" anlamına gelen bir **giriş-eşiği rütbesidir**, ampirik-kanıt
düzeyi değil. "Bir yerde muaf, bir yerde M1 izleniyor" görünümü gizli çelişki değil, §7.3/§10.5'te
**açıkça** beyan edilmiş bir ayrıştırmadır.

**Not (RFC kendi en güçlü kanıtını az-kullanmış):** RFC §9 FC#5'i savunurken "governance'ın
`maturity`/`evidence`'ı prosedürün-benimsenmesini izler, tanımını değil" diyor ama bunu
**eng/ops/econ=E0** olgusuna dayamıyor. Oysa bu, iddianın en somut ve makine-doğrulanabilir
kanıtı: ampirik boyutlar boşken governance ampirik-yeterlilik yükleyemez. Owner bu cümleyi §9
FC#5'e eklerse itiraz yüzeyi büsbütün kapanır (keskinleştirme, blocking değil).

## Görev-3: Korpus taraması (bağımsız tekrar)

`grep ^maturity:` tüm korpus (bağımsız çalıştırıldı) → RFC §12 tablosuyla **birebir**:

| Grup | maturity (grep) | §12 iddiası | eşleşme |
|------|-----------------|-------------|---------|
| ENS-2001/2002/2003/2004, ENS-3021/3022/3023 | M3 | M3 | ✅ |
| ENS-4001/4010/4020/4025/4030 | M2 | M2 | ✅ |
| ENS-4031 | M0 | M0 | ✅ |
| GOV-000/010/020/030 | M1 (+evidence) | M1 | ✅ |
| ENS-0000, ENS-4000 | yok | yok | ✅ |
| ADR-0001/0002 | M0 (5000-arch) | kapsam-dışı | ✅ |

GOV id eşlemesi bağımsız `grep ^id:` ile doğrulandı: `roles.md`=**GOV-010**, `capability-matrix.md`=
**GOV-020**, `canonical-process.md`=**GOV-030**, `000-governance-principles.md`=**GOV-000**. Yazarın
"SKR iki dosya adını andı, roles.md=GOV-010'u atladı; fiili id'ler GOV-000/010/020/030" düzeltmesi
**doğru** — dördü de `maturity:M1`+`evidence` taşıyor. §12 envanteri dürüst ve tam.

## Görev-4: D7/D8 gerçekten kapandı mı?

- **D7 (turnusol ↔ invariant önceliği):** §4.2 sonuna eklenen paragraf (satır 223-231) açık:
  *"Sınıflamayı belirleyen her zaman Test A/B/C'dir... Turnusol ile invariant çeliştiğinde turnusol
  kazanır; invariant yalnızca bir tutarlılık-kontrolüdür."* §7.3 ve §8.1 bu çerçeveyle yeniden
  yazılmış ("invariant gereği" → "Test C gereği"). **Kapandı.** ✓
- **D8 (immutable-core verme yordamı):** §7.4 (satır 384-391) hard-core statüsünün **yalnızca
  Madde XV Anayasa değişikliği + skeptic turu** ile verilebileceğini, owner/sıradan RFC'nin tek
  taraflı ilan edemeyeceğini, ampirik belgenin kendini hard-core ilan ederek kaçamayacağını
  (Test C yakalar) açıkça yazıyor. **Kapandı.** ✓

## Görev-5: Failure-condition #5 — dürüst açık mı, öz-baltalama mı?

**Dürüst açık; bloke etmez.** FC#5 (§9, satır 503-514) ayrıştırmanın kendi yanılma koşulunu
listeliyor: *eğer governance evidence'ı fiilen sınanabilir ampirik-yeterlilik iddiaları yüklüyorsa,
çözüm (a) değil (b) doğru olurdu.* Bu, RFC'nin kararını (a) baltalayan bir itiraf **değil**, Madde
X'in tam olarak istediği türden dürüst bir yanlışlanabilirlik-beyanıdır: koşul **şu an sağlanmıyor**
(eng/ops/econ=E0, Görev-2), ve FC#5 bunu yeniden-sınama tetikleyicisiyle (governance dosyaları
büyüdükçe) birlikte veriyor. Bir teori kendi çürütülme koşulunu isabetle söyleyip o koşulun şu an
gerçekleşmediğini gösterebiliyorsa, bu güç işaretidir, zaaf değil. **Bloke etmez.**

## Görev-6: Üç turluk döngü — yakınsama mı, yamalı-sarmal mı?

**Yakınsama.** Kanıt:

- **Çekirdek tez üç tur boyunca değişmedi:** iki dik eksen (`constitutive` ⊥ `canon`); canon
  aralıktan değil türe-uygun doğrulama yolundan kazanılır; tek atomik RFC. SKR-034, SKR-035 ve bu
  tur üçü de bu çekirdeği `sağ çıkıyor` buldu. Yara hep **yardımcı mekanizmada** (maturity↔
  constitutive ilişkisi) oldu.
- **Her düzeltme iddiayı zayıflattı, güçlendirmedi:** v0.2 çift-yönlü invariant (fazla iddialı) →
  v0.3 tek-yönlü gerekli-koşul + turnusol-birincil (savunulabilir küçük iddia). Bu, epicycle
  **eklemek** değil, overreach **geri çekmek**tir — yamalı-sarmalın tam tersi. SKR-001'in dersi
  ("savunulabilir küçük > çürütülebilir büyük") burada doğru işletildi.
- **Turnusol 2. turdan beri stabil:** SKR-035 Test A/B/C'yi ENS-4000/ENS-3021'de bağımsız tutarlı
  buldu; v0.3 onu değiştirmeden birincil sınıflayıcı yaptı. Yeni çatlak açılmadı; var olan sağlam
  parça yük-taşır konuma yükseltildi.

D6'nın "D1-fix'inin kendisi kırıldı" olması bir an için sarmal endişesi doğuruyordu; ama v0.3'ün
yanıtı yeni bir yama değil, kırılan yamanın **kaldırılıp** yükün zaten-sağlam turnusola verilmesi
oldu. Bu, döngünün gerçekten sağlamlaştırdığının kanıtı.

## İç tutarlılık — yeni bulgular (bloke-etmeyen keskinleştirmeler)

1. **S2 (amends alanı eksik — canon kuralının üçüncü lokusu).** RFC canon kuralını değiştiriyor.
   Bu kural depoda **üç** yerde yaşıyor: (i) ENS-0000 Madde IV (amends'te ✅), (ii)
   `metadata-header.md` §Değer kümeleri satır 48 + satır 40 yorumu (STD-METADATA-HEADER amends'te
   ✅), **(iii) `maturity-model.md` satır 34: *"`canon: true` yalnızca M5'tir"* — sert bir ratified
   kural, düz metin değil.** RFC'nin `amends:` alanı = `[ENS-0000 §IV, STD-METADATA-HEADER]`;
   **STD-MATURITY-MODEL yok.** §10.5 maturity-model.md düzenlemesini Accepted-sonrası "hizalama"
   olarak üstleniyor, ama Madde XV izlenebilirliği (değiştirilen yapıta atıf) gereği STD-MATURITY-
   MODEL ya `amends:` listesine girmeli ya da RFC neden bunu "amend" değil "türetilmiş-hizalama"
   saydığını açıkça gerekçelendirmeli. Aksi halde Accepted sonrası kısa bir pencerede
   maturity-model.md satır 34 (evrensel `canon⇔M5`) ile düzeltilmiş Madde IV (constitutive:true
   canon M5'siz) **fiilen çelişir** — §10.5 bunu tamir sözü veriyor ama makine-okunur amends bunu
   bildirmiyor. **Bloke etmez** (frontmatter tamlığı; öz §10.5'te mevcut), ama D4/D7/D8 gibi bir
   keskinleştirme.

2. **S3 (governance M1'inin "olumsal alışkanlık" nitelemesi imprecise).** RFC §7.3/§8.2 governance'ın
   `maturity:M1`'ini "olumsal/miras etiket (Faz-erken alışkanlık)" diye niteliyor. Ama
   `maturity-model.md` satır 51 açıkça der: *"Her Faz 1-2 yapıtı künyesine `maturity` ekler."* Yani
   governance'ın M1'i bir **alışkanlık değil, ratifiye bir standardın zorunlu kıldığı** bir alandır.
   Yük-taşıyan iddia (maturity constitutive yapıtın canon'unu gate etmez) bundan **bağımsız** sağ
   kalır — ama "olumsal alışkanlık" dili yanlış-betimleme. Doğru çerçeve: *"maturity-model.md şu an
   tüm Faz 1-2 yapıtları için maturity'yi zorunlu kılar; bu zorunlu etiket constitutive yapıtlar için
   canon-gate etmez ve S2 hizalamasıyla non-gating olarak işaretlenir."* **Bloke etmez** (wording).

3. **Terminoloji sürüklenmesi (Madde VI / ENS-4000):** yeni drift yok; `constitutive` kullanımı
   KULLIYAT.md/ENS-4000 ile hizalı kalıyor.

## Sahibine talepler (hepsi bloke-etmez — `survives` verildi)

- **S1** — §4.2 ya da §7.3'te invariant'ın yasakladığı **tek hücreyi** açıkça yaz
  (`constitutive:false + maturity yok = geçersiz`); böylece "tutarlılık-kontrolü" hâlâ
  test-edilebilir kalır ve "vacuous" itirazı önden kapanır.
- **S2** — `amends:` alanına **STD-MATURITY-MODEL** ekle (canon kuralının üçüncü lokusu, satır 34)
  ya da §10.5'te neden "amend" değil "türetilmiş-hizalama" sayıldığını gerekçelendir.
- **S3** — governance M1'ini "olumsal alışkanlık" yerine "maturity-model.md satır 51 tarafından
  zorunlu kılınmış ama constitutive yapıt için canon-gate etmeyen etiket" diye yeniden nitelendir;
  §9 FC#5'e **eng/ops/econ=E0** olgusunu ekleyerek ayrıştırmanın makine-doğrulanabilir kanıtını ver.

Bu üçü keskinleştirmedir; `ens-ceo` hiza incelemesi öncesi ya da sırasında owner tarafından
kapatılabilir, kapanmaları survives verdict'inin **koşulu değildir.**

## Kapanış

RFC-6001 v0.3, D6'yı doğru araçla kapattı: fazla iddialı çift-yönlü invariant'ı kaldırıp yükü
zaten-tutarlı turnusola vererek. Bağımsız korpus taraması §12 ile birebir; governance çelişkisi
gerçekten kapandı ve fiili evidence bloğu (eng/ops/econ=E0) RFC'nin ayrıştırmasını doğruluyor;
D7/D8 kapalı; FC#5 dürüst bir açık, öz-baltalama değil. Üç turluk döngü yamalı-sarmal değil,
çekirdek tezi değişmeden bırakıp yardımcı mekanizmayı hakikate doğru zayıflatan bir yakınsamadır.
Kalan üç bulgu (amends eksik lokus, invariant'ın tek yasak hücresi, governance M1 nitelemesi)
bloke-etmez ve `ens-ceo` incelemesiyle paralel kapatılabilir.

**Verdict: `survives`.** RFC skeptic-kapısını geçti. **Ama `survives` Anayasa'yı otomatik
değiştirmez ve bu skeptic "Accepted" veremez (Madde XIV/ens-ceo ayrı adım).** Gerçek sıradaki adım:
`ens-ceo` Madde XIV hiza incelemesi **+** `ens-style-guardian` şema-imzası (§7.5 çift-owner kapısı);
ancak o çift-onaydan sonra ENS-0000 Madde IV + metadata-header + (S2 gereği) maturity-model.md
fiilen düzenlenir ve korpus retrofit'i (§10) başlar. Faz durmadı; öneri kapıyı hak etti.
