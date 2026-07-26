---
id: SKR-045
type: skeptic-review
origin: .claude/rules/ (Tier-3 discipline rule layer)
depends_on:
  - .claude/rules/plan-first.md
  - .claude/rules/footprint-ladder.md
  - .claude/rules/advisor-skills.md
  - .claude/rules/work-protocol.md
  - plans/01-tier3-discipline.md
  - plans/feature-template.md
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-25
---

# REVIEW — Tier-3 Discipline Kural Katmanı Saldırısı

> **Bağımsızlık beyanı (G2 — `governance/000-governance-principles.md:33`):** denetlenen 6
> dosyanın tamamı 2026-07-26'da oturum sahibi tarafından yazıldı; yazar kendi işini
> doğrulayamaz. Bu kayıt taze context'te, `ens-skeptic` rolüyle üretildi.
>
> **Kapsam sınırı:** `.claude/rules/` bir Külliyat katmanı DEĞİLDİR. Bu yüzden verdict bir
> Külliyat kapısı değil, **çalışma katmanı kapısıdır**. Ama kurallar Külliyat'a atıf yaparak
> yetki iddia ettiği için atıfların doğruluğu Külliyat standardında sınanmıştır.

## Verdict

**`wounded`** — kural katmanı gerçek bir boşluğu (üretim-öncesi kapı) dolduruyor ve
denetlenebilir biçimde yazılmış; ama **ana iddiası (“operax'tan kopyalanmadı, ENS-3022'den
türetildi”) `refuted`**, üç ayrı yerde **uydurma/yanlış atıf** var (Madde X'e yüklenen
"gecikme yasağı", "Anayasa G2/G3", G3'ün içeriği), mevcut `governance/` katmanıyla **çift
kapı** kuruyor, ve kapattığını iddia ettiği öz-beyan kaçağını **kendi istisna bölümünde
daha genişi olarak yeniden açıyor**.

Kısaca: **iyi bir ithalat, kötü bir türetme iddiası.** İddiayı düşürüp kuralı tutmak
mümkündür; iddiayı tutmak mümkün değildir.

| Eksen | Sonuç |
|---|---|
| 1. Türetme gerçek mi | **refuted** — türetme dekoratif; karar fonksiyonu operax ile aynı |
| 2. Çift kapı | **refuted (çift kapı VAR)** — `governance/` GOV-000/010/020/030 hiç okunmamış |
| 3. Bürokrasi riski | **wounded** — azaltma yetersiz; özyineleme sonlandırılmamış |
| 4. "Uygulanmaz" kaçağı | **refuted (kaçak açık)** — Madde X atfı uydurma, Madde XV by-pass ediliyor |
| 5. Öz-beyan kaçağı | **refuted (kapanmadı, taşındı)** — ENS-2003 D-5 kalıbının tekrarı |
| 6. Katalog doğruluğu | **survives** — uydurma ajan yok; tek zayıflık faz sütunu eksikliği |
| 7. Plan kendi şablonuna uyuyor mu | **refuted** — §2 ve §5 yok; §11 blocking sorular cevapsız |

## 1. Türetme gerçek mi, süs mü?

**Sonuç: süs. `refuted`.**

### D-1.1 — Yapısal izomorfizm: belge kopyalandı, yaprakları yeniden etiketlendi

`plan-first.md:5` diyor ki: *"Kopyalanmadı, türetildi."* İki dosyayı bölüm bölüm karşılaştırdım.

| Öge | operax `plan-first.md` | ENS `plan-first.md` |
|---|---|---|
| Açılış cümlesi | `:3` "Bu kural Operax üzerindeki her oturuma uygulanır. `paths:` filtre yoktur — compact ve clear sonrası da etkilidir." | `:3` "Her ENS oturumuna uygulanır. `paths:` filtresi yok — compact ve clear sonrası da geçerli." |
| Temel kural | `:7` "Tier 3 işlerde plan ZORUNLU. Plan onaylanmadan kod yazılmaz, plan referansı olmadan Tier 3 commit atılmaz." | `:13` aynı cümle, `kod`→`yapıt` |
| Tier tablosu | `:11-15` 3 satır: Trivial/Standard/Substantial | `:18-22` aynı 3 satır, aynı adlar |
| Tier 2 çıktısı | "TODO satırı yeterli" | "TODO satırı (TaskCreate)" |
| Tier 3 çıktısı | `plans/NN-<slug>.md` | `plans/NN-<slug>.md` (aynı dosya adı şeması) |
| Sinyal sayısı | `:19-26` **6** madde | `:45-52` **6** madde |
| Şüphe cümlesi | `:28` "Şüphede kal? **Kullanıcıya sor:** 'Bu Tier 2 mi Tier 3 mü, plan yazayım mı?'" | `:54` "Şüphede kal? **Sahibine sor:** 'Bu Tier 2 mi Tier 3 mü, plan yazayım mı?'" — **birebir** |
| Workflow | `:30-67` Tier→Plan→Onay→Impl→Tamamlanma | `:56-64` aynı 5 adım (+1 kontrol adımı) |
| Commit formatı | `:60` `feat(M11): … (plan: 03)` | `:62` `feat(kernel): X (plan: 01)` |
| Arşivleme | `:65` `git mv plans/NN-*.md plans/archive/` | `:64` **birebir aynı komut** |
| Stale plan | `:69-76` **14 gün**, 3 dallı karar | `:66-71` **14 gün**, aynı 3 dal |
| İstisna 1 | `:80-84` acil bug fix, 3 numaralı adım, `BYPASS-<tarih>` token | `:76-79` aynı 3 adım, aynı `BYPASS-<tarih>` token |
| İstisna 2 | `:86-88` "hızlıca yap" → `[plan-skipped: <gerekçe>]` | `:80-82` aynı, aynı `[plan-skipped: …]` token |
| 5 lens | `:47-52` aynı 5 emoji, aynı sorular | `feature-template.md:59-64` **birebir aynı 5 satır** |

**Argüman:** ENS-3022'den *gerçekten* türetilen bir Tier sistemi, operax'ın bölüm sırasını,
14 sayısını, 6 sinyal sayısını, 2 reddedilen-alternatif sayısını, `BYPASS-<tarih>` ve
`[plan-skipped: …]` token'larını, `git mv` komutunu ve beş lensin emojilerini yeniden üretmek
zorunda değildir. Bunların hepsinin korunmuş olması, üretim yönünün **operax → ENS** olduğunu,
ENS-3022'nin ise sonradan eklenen bir **gerekçe katmanı** olduğunu gösterir.

Dürüst iddia şu olmalıydı: *"operax'tan uyarlandı; Tier ekseni ENS-3022 terimleriyle yeniden
ifade edildi."* Bu, kuralın değerini düşürmez — sadece doğrudur.

### D-1.2 — Formül karar fonksiyonuna hiçbir bilgi eklemiyor (döngüsellik)

`plan-first.md:45`: *"Şunlardan biri varsa **Stake yüksek ya da Confidence düşük demektir**;
formüle dön."*

Bu cümle türetme zincirini **tersine** çeviriyor. İşleyiş şu:

```
heuristik sinyal  →  "demek ki Stake yüksek"  →  InfoNeed yüksek  →  Tier 3
```

Yani `Stake` ve `Confidence`, Tier'ı belirlemek için bağımsız olarak ölçülmüyor; **Tier'ı
zaten belirlemiş olan heuristikten geri okunuyor.** Formül bir hesap değil, bir **yeniden
adlandırma**dır. Bileşke fonksiyon (heuristik ∘ formül), operax'ın heuristiğiyle
*ekstansiyonel olarak özdeştir*: aynı girdide aynı Tier'ı verir. Farklı sonuç veren tek bir
vaka üretilemiyorsa, iki sistem operasyonel olarak aynı sistemdir.

6 sinyalin formüle bağlanma niteliği:

| # | Sinyal (`plan-first.md`) | Formüle bağı |
|---|---|---|
| 1 | Anayasa/GOV-* dokunma | `:47` "**tanım gereği** yüksek Stake" — türetme değil, **aksiyom** |
| 2 | Yeni ENS-NNNN | irreversibility yolu — **formül dışı** (bkz. D-1.3) |
| 3 | Ratified `status` değişimi | "SKR zinciri açılır" — **prosedürel**, Stake ölçümü yok |
| 4 | Kernel davranış değişikliği | formüle **hiç** bağlanmamış |
| 5 | **3+ katmana dokunma** | operax `:21` "**3+** farklı klasöre dokunma" — **aynı sayı**, ENS etiketiyle |
| 6 | Yeni bağımlılık yönü | "ADR gerektirir" — **prosedürel** |

6 sinyalin **sıfırı** InfoNeed'den hesaplanıyor. 5 numara ise doğrudan operax'ın 1 numaralı
sinyalinin kopyası — `plans/01-tier3-discipline.md:46` bu eşlemeyi zaten açıkça yapıyor
("3+ klasör … → `Stake`"). Fakat *neden* 3 klasörün yüksek Stake demek olduğuna dair tek bir
argüman yok. Sayı korunmuş, gerekçe değişmiş: bu **rasyonalizasyondur, türetme değil.**

### D-1.3 — Asıl yük taşıyan eksen ENS-3022'de YOK

`IsIrreversible`, Tier 3'ün *tek başına yeterli* koşulu (`plan-first.md:22,24`) ve
`footprint-ladder.md:31-33` gereği yeni yapıt üretiminin **otomatik** Tier 3 yolu. Yani
pratikte ENS işlerinin çoğunu Tier 3 yapan eksen budur.

Fakat: **`ENS-3022-decision-gravity.md`'de `IsIrreversible` geçmiyor** (dosyanın tamamı
okundu; `Stake`, `Uncertainty`, `InfoNeed`, `ConformanceDeficit`, `AttentionPriority` var,
irreversibility yok). Kaynak `ADR-0001 §5.6` (Faz 3) ve kernel `Scheduler.cs` /
`PendingDecision` (Faz 4).

Sonuç: **Tier sisteminin ağırlık taşıyan bacağı ENS-3022'den değil, ADR + kernel'den
geliyor.** `plan-first.md:8`'in "ENS'te Tier … **bu formülden** okunur" cümlesi bu yüzden
yanlıştır — belge `:24-26`'da bunu zaten itiraf ediyor ("İkisi farklı eksendir"), ama başlık
iddiası düzeltilmemiş. İki cümle aynı dosyada çelişiyor.

**Ek olarak P8 gerilimi (Anayasa `:70`):** *"Teori implementasyondan önce gelir … teori asla
koddan geri türetilmez."* `plans/01-tier3-discipline.md:48` bir yönetişim ölçütünü kernel
alanının varlığıyla gerekçelendiriyor: *"`IsIrreversible` — `PendingDecision`'da zaten var."*
**Steelman savunma:** `.claude/rules/` Külliyat değildir, P8 onu bağlamaz; ayrıca ADR-0001 §5.6
mimari katmandır, kod değil. Bu savunmayı geçerli buluyorum — bu yüzden bunu *kusur* değil
**gerilim** olarak kaydediyorum. Ama iddia "ENS-3022'den türetildi" olduğu sürece, gerçek
kaynağın 5000/7000 olması iddiayı zayıflatır.

### D-1.4 — Kalibrasyonsuz formül, heuristikten daha mı iyi? Hayır — ve bunu söylemek yasak

`ENS-3022` künyesi: `evidence: {sci: E3, eng: E1, ops: E0, econ: E0}`, `maturity: M3`.
`.claude/standards/evidence-standard.md:27` — **E0 = "Opinion; destek yok."**
Aynı dosya `:45` — **"Bir iddia, seviyesini aşan kesinlikle sunulamaz."**

`plan-first.md` ENS-3022'yi **operasyonel bir kontrol yasası** olarak kullanıyor (her oturumun
her işini sınıflandıran kural). Bu, `ops: E0` bir yasanın ops boyutunda **E3 gibi**
kullanılmasıdır. Standart bunu açıkça yasaklıyor.

Dahası, ENS-3022'nin kendi `Failure conditions` bölümü (`:128-131`) iki kırılma koşulu sayıyor
ve **ikisi de** bu kullanımda aktif:
- *"Confidence kalibrasyonuna bağımlılık … Confidence kötü kalibreyse InfoNeed **yanıltır**."*
  → Oturum işinde Confidence hiç kalibre edilmiyor; kalibrasyon mekanizması (ENS-2004) çalışmıyor.
- *"Stake ölçümü OL1'e bağlı. Alternative-başına EV yoksa Stake **kaba**."*
  → Oturum işinde ne Alternatives ne ExpectedValue var.

Ayrıca ENS-3022 `:71-74` `Stake`'in **Purpose-tipi içinde normalize** edilmesini (z-skoru/
persentil, ENS-2003 Memory'den) şart koşuyor. `plan-first.md`'de ne popülasyon, ne Purpose
tipi, ne normalizasyon var. Yani `plan-first.md`'deki `Stake`, ENS-3022'deki `Stake` **değildir**
— aynı sözcük, farklı gönderge. Bu, Madde IX / ENS-4000 anlamında **terminoloji
sürüklenmesidir** (bkz. §6 İç tutarlılık).

**Cevap:** Kalibrasyonsuz formül heuristikten daha iyi değildir; **daha kötüdür**, çünkü
heuristik kendi keyfîliğini saklamaz ("3 klasör" apaçık keyfîdir), formül saklar. `plans/…:59`
"türetirsek ENS-3022'nin ilk gerçek **operasyonel karşılığını** üretmiş oluruz" cümlesi bu
hâliyle karşılanmamıştır: üretilen şey operasyonel karşılık değil, operasyonel **kılıftır**.

### D-1.5 — Yanlışlanabilirlik (Madde X) sınavı

Soru: *"Bu Tier ataması yanlıştı"* hangi gözlemle gösterilir?

Bugünkü hâliyle **gösterilemez.** Stake ve Confidence yalnızca Tier'ın kendisinden geri
okunduğu için (D-1.2), hiçbir gözlem bir Tier atamasıyla çelişemez. Kural katmanının merkezî
iddiası — "Tier ENS-3022'den okunur" — **yanlışlanamazdır**. Madde X'in yasağı Külliyat
yapıtları içindir, ama bu kural katmanı Külliyat'a *yetki iddiasıyla* atıf yaptığı için aynı
ölçüte kendini açmıştır.

**Yanlışlanabilir hâle getirmenin en ucuz yolu (talep T-1):** her plana `Tier gerekçesi`
bölümünde **öngörü** yazılması — "bu iş Tier 3; öngörüm: en az 1 reddedilen alternatif
gerçekten seçilecek / en az 1 SKR bulgusu çıkacak." Plan arşivlenirken öngörü tutmadıysa Tier
ataması **fazla** işaretlenir. 10 planda tutarlı biçimde tutmuyorsa eşik yanlıştır ve düşürülür.
Bu, kalibrasyon borcunu kapatmaz ama **ölçülebilir** yapar.

## 2. Çift kapı var mı?

**Evet. Ve iddia edilenden daha kötüsü: yeni kapı, mevcut kapıdan ZAYIF.** `refuted`.

### D-2.1 — `governance/` katmanı hiç okunmamış (kanıt: grep)

ENS'in mevcut yönetişim mekanizması `governance/` altında **dört numaralı yapıtla** tanımlı:

| id | Dosya | İçerik |
|---|---|---|
| GOV-000 | `governance/000-governance-principles.md` | G1-G5 ilkeleri |
| GOV-010 | `governance/roles.md` | Author / Validator / Governance rol ayrımı |
| GOV-020 | `governance/capability-matrix.md` | Kim hangi M-seviyesine promote edebilir |
| GOV-030 | `governance/canonical-process.md` | M3→M5 kanıt zinciri, kapılar |

Denetlenen 6 dosyada bu dört yapıta yapılan atıf sayısı:

```
grep "governance|GOV-|roles.md|capability-matrix|canonical-process"
  .claude/rules/  →  1 sonuç:  plan-first.md:47  ("Anayasa / GOV-\* dosyasına dokunma")
  plans/          →  0 sonuç
```

`plans/01-tier3-discipline.md` §3'ün başlığı **"ENS'te ZATEN olan — tekrar kurmayacağımız"**
(`:65`). Bu bölümün tek işi, çift kapıyı önlemek için mevcut mekanizmayı sayıp dökmekti. O
bölüm (`:70-79`) `governance/` katmanının **hiçbir dosyasını** anmıyor. Yani çifte-kapı
denetimi **depoya karşı değil, yazarın hafızasına karşı** yapılmış.

`footprint-ladder.md:42-44` bu anti-pattern'i kendisi tanımlıyor:
> *"Dış repodan 'esin' diye ENS'te zaten olanı tekrar kurmak. ENS'te kontrol katmanı … zaten
> güçlüdür; operax'ın 'kontrol ettir' adımını yeniden kurmak çift kapı üretir."*

Kural doğru; **kardeş kuralı bu kuralı ihlal ediyor.**

### D-2.2 — "Anayasa G2/G3" UYDURMA ATIF

`work-protocol.md:31`: *"**Öz-onay YOK. Anayasa G2/G3:** yazan doğrulayamaz. Bu, ENS'te
*zaten* **anayasal** bir kuraldır."*
Aynı iddia: `advisor-skills.md:43` ("Madde G2/G3"), `:66` ("Anayasa Madde XIV — rol ayrımı;
G2/G3"), `footprint-ladder.md:43`.

Doğrulama:
```
grep "\bG2\b|\bG3\b"  0000-constitution/  →  SIFIR sonuç
```
Anayasa'nın **15 maddesinin hiçbiri** G2/G3 içermiyor (madde başlıkları okundu: Madde I-XV).
G2/G3 **GOV-000'da** tanımlı — `governance/000-governance-principles.md:33-34`. Madde XII
(`:214`) uyarınca `standards` katmanı Anayasa'nın **altındadır**.

**Neden önemli:** kural katmanı yetkisini Anayasa'dan devraldığını iddia ediyor. Gerçek kaynak
bir alt katman. Bu, Madde VIII (İzlenebilirlik) ve Madde XI (atıf) anlamında yanlış künyedir.
`ens-skeptic`'in görevi tam da bunu yakalamaktır.

> **Hafifletici (dürüstlük gereği):** bu yanlış atıf **yeni değildir** — depoda yaygın:
> `ENS-2003:491` "Anayasa G2/G3", `RFC-6001:534` "Madde XV-b; G2/G3". Buna karşılık
> `ENS-4031:294` **doğru** yazıyor: "governance G2". Yani kural katmanı mevcut bir hatayı
> *miras aldı*, icat etmedi. Ama "kaynaktan türettim" iddiasında olan bir belge, kaynağa
> bakmadığını böyle ele vermiş olur.

### D-2.3 — G3 YANLIŞ AKTARILMIŞ: üç-yollu ayrım iki-yollu sanılmış (en ağır bulgu)

GOV-000'un gerçek metni:
- `:33` **G2 — No author canonizes their own work.** *"Bir yapıtı yazan, onu Canonical yapamaz."*
- `:34` **G3 — Validation ve approval ayrıdır.** *"Doğrulayan onaylamaz; onaylayan doğrulamaz."*
- `:35` **G4 — Her Canonical yapıtın ≥2 bağımsız validator'ı vardır** (farklı boyutlardan).

`governance/roles.md:38-39` bunu role çevirir: **Validator ≠ Author (G2)**, **Validator ≠
Governance (G3)**. Yani ENS'te **üç ayrı rol** vardır: Author → Validator → Governance.

`work-protocol.md:31` bu üçlüyü **"G2/G3: yazan doğrulayamaz"** tek cümlesine indiriyor. Bu,
G3'ün içeriğini **siliyor**: G3 yazar-doğrulayan ayrımı değil, **doğrulayan-onaylayan**
ayrımıdır.

Somut sonucu `work-protocol.md:35-40` tablosunda görünüyor. Tablonun sütun başlığı **"Kapı"**
ve teori/ontoloji satırı için tek hücre: *"`ens-skeptic` → SKR kaydı (verdict: survives …)"*.
Yani **skeptic verdict'i kapı sayılıyor.** Oysa:
- G3'e göre validator (skeptic) **onaylayamaz**; onay ayrı bir edimdir.
- `governance/canonical-process.md:42` — *"Her kapı bir kanıttır, oy değil. Governance kanıt
  üretmez, yalnızca zincirin tamlığını doğrulayıp **son kapıyı** açar."*
- `canonical-process.md:44` (G4) — **≥2 bağımsız boyut validator'ı** gerekir.

`work-protocol.md`'nin tablosu her yapıt türüne **BİR** kapı atıyor. GOV-030 **EN AZ İKİ**
bağımsız boyut + ayrı bir Governance onayı istiyor.

**Bu, çift kapıdan daha kötüdür: paralel ve GEVŞEK bir kapı.** Bir uygulayıcı `work-protocol`
adım 3'ü tamamlayıp "kapı geçildi" diyebilir; GOV-030'a göre zincirin **üçte biri** yapılmıştır.
Depodaki mevcut pratik de bunu doğruluyor: `ENS-4020` M2'ye **iki** bağımsız validator ile
çıkarıldı (`ROADMAP.md:95` — "iki bağımsız validator (SKR-028+SKR-030) → G4"). Yeni kural bu
pratiği bilmiyor ve tekilleştiriyor — yani **mevcut disiplini geriye götürüyor.**

### D-2.4 — ADR kapısı, fiilî ENS pratiğiyle çelişiyor

`work-protocol.md:37`: *"ADR / mimari | Bağımsız `ens-architect` context + gerekiyorsa hizalama
incelemesi"*.

Fiilî pratik farklı: ADR incelemeleri `ens-skeptic` tarafından yapılmış —
`5000-architecture/reviews/SKR-024, SKR-025, SKR-026, SKR-027, SKR-029, SKR-037` hepsi SKR
kaydı. `SKR-029:18` açıkça G2'ye atıf yapıyor. Ayrıca `ens-architect` ADR'lerin **yazarıdır**
(`ENS-4010:339` — "yazarın (ens-architect) kendi işidir"), dolayısıyla mimari işin varsayılan
kapısı olarak `ens-architect` atamak **G2'nin doğrudan ihlal riskidir** — kural `:37` "bağımsız
context" diyerek bunu hafifletiyor ama GOV-000 G2 rol düzeyinde konuşur ("Author"), context
düzeyinde değil.

### D-2.5 — Kural katmanı Madde XII grafiğinde YOK

Madde XII (`ENS-0000:209-221`) yetki grafiğini sayar: Anayasa → Külliyat → **Standards** →
**Commands** → **Agents** → Implementation. `.claude/rules/` bu grafikte **yoktur**.

`grep "\.claude[/\\]rules"` → yalnızca yeni yazılan 6 dosyanın kendisi. Yani `.claude/rules/`
depoda **hiçbir yapıt tarafından tanınmıyor**.

Bu iki sonucu doğuruyor:
1. **Yetkisiz katman.** Madde XII "Yetki tek yönde akar … Hiçbir şey Külliyat'ı atlamaz" diyor.
   Ajan davranışını bağlayan ama grafikte yeri olmayan bir katman, yetkisini nereden alıyor?
2. **Kendi merdivenine göre 6. basamak.** `footprint-ladder.md:22` — *"Yeni katman / faz (SON
   ÇARE) … **Çok yüksek** — ADR + mimari donma etkisi + Tier 3 plan."* `.claude/rules/`
   tam olarak yeni bir katmandır. `plans/01:127` ise onu **"Faz 1 — düşük risk, geri
   alınabilir"** diye sınıflıyor ve rollback'i `git rm` sanıyor (`:179`).
   **Kural, kendi doğuşunu yanlış basamağa yerleştirmiş.** Doğru işlem: ADR (ya da Madde XII
   grafiğine dokunduğu için Madde XV uyarınca RFC) + Tier 3.

### D-2.6 — Katman hiç yüklenmiyor: "her oturuma uygulanır" iddiası bugün YANLIŞ

`plan-first.md:3` ve `work-protocol.md:4`: *"`paths:` yok — compact sonrası da geçerli",
"Her ENS oturumuna uygulanır."*

Doğrulama:
- ENS deposunda kök `CLAUDE.md` **YOK** (okuma denendi: "Path does not exist").
- `.claude/` altında yalnızca `settings.json`, `settings.local.json` + dizinler var.
- operax'ta ise kökte **`CLAUDE.md` VE `RULES.md`** mevcut (`D:\Dev\operax\CLAUDE.md`,
  `D:\Dev\operax\RULES.md`) — kuralları oturuma taşıyan mekanizma budur.

**Yani port sırasında kuralların kendisi alındı, onları yükleyen mekanizma alınmadı.** Bugün
bu 4 dosya hiçbir oturuma otomatik girmez. `paths:` cümlesi operax'tan devralınmış bir
frontmatter alışkanlığının artığıdır ve ENS'te **doğrulanmamış bir iddiadır**.

Bu, `work-protocol.md:50` "'yazıldı' YETMEZ — gerçekten öyle mi kanıtla" kuralının kendi
üzerinde ilk ve en net ihlalidir.

## 3. Bürokrasi riski

**`wounded`** — risk gerçek, azaltma yetersiz, ama kural katmanı kurtarılabilir.

### D-3.1 — Somut senaryo: "ENS-2003'ün `failure_conditions`'ına bir cümle ekle"

Gerçekçi, küçük bir iş. Adımları kuralların **kendi metnine göre** sayıyorum:

| # | Adım | Dayanak |
|---|---|---|
| 1 | Tier tespiti | `plan-first.md:58` |
| 2 | Ratified yapıta additive revizyon → `status: ratified → review` gerekiyor → **Tier 3 sinyali #3** ("Ratified bir yapıtın `status`'ünü değiştirme → SKR zinciri açılır") | `plan-first.md:49`; pratik: `ENS-2001:40` |
| 3 | `ens-philosopher`'a danış (teori metni) — üretimden ÖNCE | `advisor-skills.md:20,47` |
| 4 | `plans/NN-<slug>.md` yaz: **11 bölüm**, ≥2 reddedilen alternatif, 5 lens, risk tablosu, done criteria, rollback | `plan-first.md:59-60`, `feature-template.md` |
| 5 | Sahibinden onay bekle (**bloke edici**) | `plan-first.md:61` |
| 6 | Cümleyi ekle + yargı noktalarını işaretle | `work-protocol.md:21-27` |
| 7 | Künye güncelle (`version`, `last_reviewed`, `status`) | `.claude/standards/metadata-header.md` |
| 8 | `ens-skeptic` turu → **yeni SKR dosyası** | `work-protocol.md:36` |
| 9 | (GOV-030 gereği) **ikinci** boyut validator'ı | `canonical-process.md:44` |
| 10 | `skeptic_review:` alanını güncelle, verdict'e göre `status` | mevcut pratik |
| 11 | Adım 4: iddiayı kanıtla (koda atıf varsa doğrula) | `work-protocol.md:48-60` |
| 12 | Commit + plan referansı `(plan: NN)` | `plan-first.md:62` |
| 13 | `git mv plans/NN-*.md plans/archive/` + done criteria + journal | `plan-first.md:64` |
| 14 | REGISTRY / ROADMAP / KULLIYAT senkronu | mevcut pratik |

**14 adım, 3 yeni kalıcı dosya (plan, SKR, journal girdisi), 2 bloke edici bekleme — bir
cümle için.** Üretilen metin ~30 kelime; onu çevreleyen yönetişim metni ~3000 kelime.
Bu oran, `plans/01:187-190`'ın Contrarian lensinin *tam olarak* öngördüğü şeydir.

### D-3.2 — Azaltma ("4 rule + uygulanmaz bölümü") neden yetersiz

`plans/01:158` azaltması: *"Faz 1'de yalnız 4 rule; her rule için 'ne zaman UYGULANMAZ'
bölümü zorunlu."*

İki nedenle yetersiz:

1. **Yük kural *sayısından* gelmiyor, kuralların *bileşiminden* geliyor.** 4 kural birbirine
   zincirlenmiş: `plan-first` → `feature-template` (11 bölüm) → `advisor-skills` (ön danışma)
   → `work-protocol` (4 adım) → `footprint-ladder` (6 basamak). Tek bir Tier 3 tetiği bu
   zincirin tamamını ateşliyor. 4 kuralı 2'ye indirmek yükü yarıya indirmez.
2. **"Uygulanmaz" bölümleri yükü azaltmıyor, yükü *keyfîleştiriyor*** (bkz. §4). Bir iş ya
   14 adım ya 0 adım. Ara rejim yok. `plan-first.md:21`'in "Tier 2 → TODO satırı" seçeneği
   ara rejim gibi görünüyor ama Tier 2'ye düşmek için sinyallerin **hiçbirinin** olmaması
   gerekiyor — ENS'te ratified yapıta dokunan her iş sinyal #3'ü tetikliyor.

**Eksik olan azaltma (talep T-2):** "mini-plan" rejimi. `plan-first.md:80` istisna metninde
*"mini-plan yazayım mı?"* diye geçiyor ama **mini-plan hiçbir yerde tanımlı değil** — ne
şablonu var, ne hangi bölümlerin zorunlu olduğu yazılı. Yani kuralın kendi önerdiği hafif yol
mevcut değil. Tanımlanmalı: Tier 3-hafif = Problem + Tier gerekçesi + 2 alternatif + rollback
(4 bölüm), lens ve risk tablosu opsiyonel.

### D-3.3 — Özyineleme sonlandırılmamış (ölçülebilir bürokrasi patlaması)

`work-protocol.md:3-5`: *"Her substantive iş bu 4 adımlı döngüden geçer. Teori, ontoloji, ADR,
kernel kodu, test, **yönetişim değişikliği — istisnasız.**"*

**Bir SKR kaydı yönetişim yapıtıdır.** O hâlde:
- SKR yazmak substantive iş mi? Metne göre **evet** ("yönetişim değişikliği — istisnasız").
- O hâlde SKR'nin de adım 3'ten (bağımsız kapı) geçmesi gerekir → **SKR'nin SKR'si**.
- `work-protocol.md:35-40` tablosunda SKR/review yapıtları için **satır yok**. Yani özyineleme
  ne kapatılmış ne de açıkça istisna edilmiş.

`footprint-ladder.md:49-51` yalnızca *merdiven* muafiyeti veriyor ("SKR/review kayıtları
merdivene tabi değildir"), work-protocol muafiyeti vermiyor. Bu **şu anda okuduğunuz belge**
için de geçerli: bu inceleme substantive bir yönetişim işidir ve kurala göre bir kapıdan
geçmelidir; hangi kapı olduğu yazılı değil.

**Talep T-3:** `work-protocol.md` "NE ZAMAN UYGULANMAZ" bölümüne açık bir sonlandırma satırı:
*"Review/SKR kayıtları adım 3'ten muaftır — kendileri adım 3'ün ürünüdür. İtiraz yolu G6
(`canonical-process.md:48`): gerekçeli itiraz yeni bir tur açar."* GOV-030 bu sonlandırmayı
zaten sağlıyor — yine `governance/` okunmadığı için tekrar keşfedilmemiş.

### D-3.4 — ROADMAP'in kendi kaydı, "kontrol katmanı güçlü" iddiasını çürütüyor

`work-protocol.md:8-9`: *"ENS'in SKR zinciri + G2/G3 kapısı, operax'ın reviewer zincirinden
**zaten güçlüdür**."* `plans/01:72`: *"✅ Var, **güçlü**."*

`ROADMAP.md:224` (ENS'in kendi açık risk kaydı):
> `| G-16 | Governance tek-operatör (rol ayrımı G2/G3 **fiilen zayıf**) | P3 |`

Kural katmanı, ENS'in kendi sicilinde **açık bir zayıflık olarak kayıtlı** mekanizmayı "güçlü"
kabul edip üzerine inşa ediyor. Tek-operatör gerçeği (yazan, doğrulayan ve onaylayan aynı
insan) hem G2'yi hem G3'ü fiilen boşaltıyor. Bu, kural katmanının **en temel varsayımının**
depoda çürütülmüş olması demektir.

**Bu, bürokrasi riskinin asıl kaynağıdır:** tek operatörlü bir projede 14 adımlı bir protokol,
bağımsızlık üretmez — yalnızca **aynı kişinin daha çok dosya yazması** anlamına gelir. Yani
maliyet gerçek, fayda (bağımsızlık) nominal.

## 4. "Uygulanmaz" bölümleri kaçak mı?

**Evet — ve dayandıkları anayasal gerekçe UYDURMA.** `refuted`.

### D-4.1 — "Madde X gecikmeyi yasaklar" — Madde X böyle bir şey demiyor

Üç dosyada, aynı cümle, aynı yetki iddiası:
- `plan-first.md:77-78` — *"Acil düzeltme: … plan beklenmez — **Madde X düzeltmeyi geciktirmeyi
  yasaklar.**"*
- `advisor-skills.md:60` — *"Acil düzeltme … — **Madde X gecikmeyi yasaklar.**"*
- `work-protocol.md:76-77` — *"… adım 1 atlanabilir (**Madde X gecikmeyi yasaklar**)"*

Madde X'in **tam metni** (`0000-constitution/ENS-0000-constitution.md:174-179`):

> ## Madde X — Yanlışlanabilirlik Ödevi (Falsifiability Duty)
> Saldırılamayan bir teoriye güvenilemez. Her Külliyat kavramı, kendi belgesinde, **yanlış**
> olacağı koşulları (varsayımlar + başarısızlık modları) taşır. `ens-skeptic` sürekli bir
> karşıt inceleme yürütür. Başarısızlık koşulları belirtilmemiş bir kavram tamamlanmış
> değil, eksiktir.

Madde X **düzeltme hızı hakkında tek kelime içermiyor.** Konusu: her kavramın kendi
`failure_conditions`'ını taşıması ve skeptic'in sürekli çalışması. "Gecikme yasağı" metinde
yoktur.

**Bu bir yorum farkı değil, uydurulmuş bir yetkidir.** Ve tesadüfen kural katmanının **en
geniş istisnasını** çalıştıran şey odur. `work-protocol.md:41-46` başkalarına *"emin değilse
DOĞRULANMADI yaz — uydurma"* diyen bir kural, kendi muafiyetini uydurulmuş bir anayasa
maddesine dayandırıyor.

> **Steelman:** Madde X'in *ruhu*, yanlış bir iddianın Külliyat'ta durmasını hoş görmez;
> `ens-skeptic`'in "sürekli" çalışması bir aciliyet sezgisi verir. Bu makul bir **çıkarım**dır
> — ama çıkarım olarak yazılmalıdır ("Madde X'in ruhu gereği …"), yasak olarak değil. Bir
> istisnanın gücü, dayandığı metnin gücünü aşamaz.

### D-4.2 — "Acil düzeltme" istisnası, ENS'teki işlerin ÇOĞUNU kapsıyor

Tetikleyici koşul (`plan-first.md:77`): *"yayınlanmış bir yapıtta yanlış/yanıltıcı iddia
bulunduğunda."*

ENS'teki tipik iş **tam olarak budur.** Kanıt, bu oturumun kendi git durumu:
`SKR-040`…`SKR-044`, `SKR-037`, `SKR-038`, `SKR-039`, `DEFECT-REGISTER-VERIFICATION.md`,
`AUDIT-WAVE2-*` — hepsi yayınlanmış yapıtlardaki yanlış/yanıltıcı iddiaların düzeltilmesi.
`plans/01:119` bunu kendisi söylüyor: *"bir yalan iddia (zincir kuralı) aylarca ROADMAP'te
durdu."*

Bir disiplinin **modal** iş türü, o disiplinin istisna koşulunu sağlıyorsa, istisna kuralı
yutmuştur. ENS'te "yanlış iddia düzeltiyorum" demek neredeyse her zaman doğrudur.

### D-4.3 — Retro plan, planın TEK işlevini yerine getiremez

İstisnanın azaltması (`plan-first.md:78-79`): bypass duyurusu + `(plan: BYPASS-<tarih>)` +
sonradan `plans/archive/BYPASS-<tarih>.md`.

Ama `plans/01:81-83` planın gerekçesini şöyle koymuştu:
> *"ENS'in eksiği 'kontrol' değil — orası zaten güçlü. Eksik olan **üretim öncesi** kapı."*

**Üretim sonrası yazılan bir plan, üretim öncesi kapı değildir.** Retro plan yalnızca kağıt izi
üretir; kararı etkileme kapasitesi sıfırdır. Yani acil-düzeltme yolu, kural katmanının
**varlık sebebini** koruyormuş gibi görünürken tamamen ortadan kaldırıyor. İz kalır, kapı
kalmaz.

### D-4.4 — "Sahibi hızlıca yap derse" — W8d'nin insan sürümü, tam olarak

`plan-first.md:80-82`:
> *"**Sahibi 'hızlıca yap' derse:** Tier 3 sinyali varsa yine uyarılır … 'Direkt' derse görev
> kaydına `[plan-skipped: <gerekçe>]` düşülür."*

Bu istisnanın:
- **Şiddet tabanı yok.** Tier 3 olması engellemiyor.
- **`IsIrreversible` muafiyeti yok.** Geri alınamaz iş de bypass edilebilir.
- **Anayasa muafiyeti yok.** Sinyal #1 (Anayasa/GOV-* dokunma) bile bu yolla atlanabilir.
- **Kalıcı iz üretmiyor.** `[plan-skipped: …]` görev kaydına düşer — git'e değil, EC-001
  değişmezliğine tabi bir yapıta değil.

Şimdi bunu, kuralın kapattığını iddia ettiği kusurla yan yana koyun.

`AUDIT_DEFECT_W8d` (doğruladım: `AdversarialWave_SecurityTests.cs:958-975`, `plan-first.md:36`
atfı **birebir doğru**): tek bir öz-beyan skaler (`confidence = 1.0`), `stake = 1e12` olsa bile
InfoNeed'i, AttentionPriority'yi, tier'ı ve gate'i **birlikte** sıfırlıyor.

`plan-first.md:80-82`: tek bir öz-beyan sözcük ("direkt"), Tier ne olursa olsun planı, onay
kapısını ve danışma adımını **birlikte** sıfırlıyor.

**Yapı aynı.** Kural, `:32`'de dar bir kaçağı (Confidence) kapatıp, **48 satır sonra** aynı
dosyada daha genişini (sahip iradesi) açıyor. `plans/01:115` bunu önceden ve doğru biçimde
adlandırmıştı: *"Kernel'e kural koyup kendimizi muaf tutmak, W8d'nin insan sürümüdür."*
Kural yazılırken bu tam olarak yapılmış.

### D-4.5 — İstisna, ADR-0001 §5.6 ve Madde XV ile DOĞRUDAN çelişiyor

**(a) ADR-0001 §5.6 ile.** `plan-first.md:26` yetkisini oradan alıyor. O bölümün metni
(`ADR-0001-agent-runtime.md:362, 369`):
> `Q -->|Kritik/geri-dönülemez| C[Otomatik blok, **bypass yok**]`
> *"Kritik/geri-dönülemez action'larda **bypass yasaktır**; istisna yalnızca **immutable audit
> kaydı** üreten kontrollü bir Exception-Policy ile mümkündür … her istisna sonsuza dek izde
> kalır."*

Yani `plan-first.md`, `IsIrreversible`'ı ADR-0001'den ithal ederken **ona bağlı yasağı ithal
etmemiş.** İnsan katmanındaki kural, atıf yaptığı kernel kuralından **daha gevşek**. Bu,
`plans/01`'in tüm argümanının tersine dönmesidir: plan, insanın kendini kernel'den muaf
tutmasını eleştiriyordu; ürettiği kural tam da o muafiyeti veriyor.

**(b) Madde XV ile.** `ENS-0000:249-251`:
> *"Yalnızca şu koşulları sağlayan bir RFC ile değiştirilir: (a) değiştirilen maddeye atıf
> yapar, (b) skeptic saldırısından geçer, (c) Madde XIV uyarınca kabul edilir."*

`plan-first.md:47` Anayasa'ya dokunmayı bir **Tier 3 sinyali** (→ plan yaz) sayıyor. Ama
Anayasa'ya dokunmak plan işi değildir, **RFC işidir**. Daha kötüsü: `:76-79` ve `:80-82`
istisnaları Anayasa'yı kapsam dışı bırakmadığı için, kural metni okunduğunda
*"Anayasa'da yanıltıcı bir ifade buldum → acil düzeltme → BYPASS + retro plan"* yolu **açık
görünüyor.** Madde XV bunu kategorik olarak yasaklıyor.

**Talep T-4:** her iki istisnaya sert taban:
> *"Bu istisnalar `IsIrreversible` işlerde, Anayasa/`governance/` yapıtlarında ve ratified
> Külliyat yapıtlarının `status` değişiminde **uygulanmaz**. Anayasa değişikliği yalnızca
> Madde XV yoluyla (RFC) yapılır — acil düzeltme bu yolu kısaltmaz. Sahip iradesiyle atlanan
> her Tier 3 işi, görev kaydına değil **git'e** düşen kalıcı bir `plans/archive/SKIPPED-<tarih>.md`
> üretir (ADR-0001 §5.6 Exception-Policy karşılığı)."*

### D-4.6 — Katıldığım nokta: "uygulanmaz" bölümleri fikir olarak DOĞRU

Bunu belirtmezsem haksızlık olur: operax'ın 4 kuralından yalnızca 2'sinde "İstisnalar" bölümü
var (`plan-first.md:78`, `phase-review-gate.md:82`); `work-protocol.md` ve `advisor-skills.md`
istisnasız. ENS **dördünde de** zorunlu kılmış. Bu, ithal değil **gerçek bir ENS eklentisidir**
ve iyi bir tasarım kararıdır: kapsamı yazılı olmayan kural, sessizce ihlal edilen kuraldır.
Kusur bölümlerin varlığında değil, **sınırlarının ve dayanaklarının** yazılmamış olmasındadır.

## 5. Öz-beyan kaçağı gerçekten kapandı mı?

**Hayır. Kapanmadı — bir seviye yukarı TAŞINDI.** `refuted`. (ENS-2003 D-5 kalıbının tekrarı.)

### D-5.1 — Kuralın metni, kendi tablosuyla çelişiyor

`plan-first.md:30-33`:
> *"`Confidence` **öz-beyandır**. `Stake` ve `IsIrreversible` **gözlemlenebilirdir**.*
> ***Kural:** Yüksek `Confidence` beyanı tek başına Tier **düşüremez**. Taban her zaman
> `Stake` ve `IsIrreversible`'dan okunur."*

Şimdi tabloyu (`:18-22`) mekanik olarak çalıştıralım. Tier 3 koşulu:
`yüksek InfoNeed` **VEYA** `IsIrreversible`, ve `InfoNeed = Stake × (1 − Confidence)`.

| Vaka | Stake | Confidence | IsIrreversible | InfoNeed | Tier |
|---|---|---|---|---|---|
| A | yüksek | düşük (0.2) | hayır | **yüksek** | **3** |
| B | yüksek | **yüksek (0.95)** | hayır | **düşük** | **2** |

A → B geçişinde **değişen tek değişken `Confidence`'tır** ve Tier 3'ten Tier 2'ye düştü.
Yani "yüksek Confidence beyanı **tek başına** Tier düşüremez" cümlesi, tablonun kendi
aritmetiğiyle **yanlıştır**.

Metin "taban her zaman Stake'ten okunur" diyor — ama **tabloda "yüksek Stake → en az Tier 3"
diye bir satır yok.** Tabloda Stake yalnızca Confidence ile çarpım hâlinde görünüyor.
Vaat edilen taban, mekanizmada mevcut değil: kaçak **beyanla** kapatılmış, **kuralla** değil.

**Bu, W8d'nin birebir tekrarıdır.** W8d'de `confidence = 1.0`, `stake = 1e12` iken bile tüm
yığını sıfırlıyordu (`AdversarialWave_SecurityTests.cs:967-969`). Burada `Confidence = 0.95`,
Stake yüksekken bile Tier'ı düşürüyor. Kural, kernel kusurunu insan katmanında **yeniden
üretmiş** — üstelik onu kapattığını ilan eden bölümde.

**Talep T-5 (kaçağı gerçekten kapatan minimal düzeltme):** tabloya taban satırı:
> *"`Stake` yüksekse Tier ≥ 3'tür — `Confidence` ne beyan edilirse edilsin. `Confidence`
> yalnızca **düşük Stake** aralığında Tier 1↔2 ayrımını yapabilir; Tier 3 eşiğini hiçbir
> koşulda etkilemez."*
>
> Biçimsel karşılığı: `Tier3 ⟺ (Stake ≥ θ) ∨ IsIrreversible` — yani InfoNeed'in Tier-3
> kapısında **rolü yok**. InfoNeed yalnızca *ne kadar context toplanacağını* söyler
> (ENS-3022'nin asıl işi), *kapının açılıp açılmayacağını* değil. Bu, ENS-3022'yi doğru
> kullanmak olurdu — ve ironik biçimde formülü Tier tablosundan çıkarmak, türetme iddiasını
> zayıflatmak yerine **güçlendirirdi**.

### D-5.2 — Stake de öz-beyandır: "gözlemlenebilir" iddiası desteksiz

`plan-first.md:30` `Stake`'i "gözlemlenebilir" ilan ediyor. ENS-3022'nin tanımı (`:64`):

```
Stake(d) = spread( ExpectedValue(aᵢ) ) , aᵢ ∈ Alternatives(d)   (OL1'e bağlı)
```

`ExpectedValue` **beyan edilen** bir büyüklüktür — `Confidence`'tan hiç de daha gözlemlenebilir
değildir. ENS-3022 bunu kendi `Failure conditions`'ında itiraf ediyor (`:130-131`):
*"**Stake ölçümü OL1'e bağlı.** Alternative-başına EV yoksa Stake **kaba**."*

Oturum işinde durum daha kötü: ne `Alternatives` var, ne `ExpectedValue`, ne ENS-3022'nin şart
koştuğu **Purpose-tipi içi normalizasyon** (`:71-74`, z-skoru/persentil, Memory'den). Yani
`plan-first.md`'deki `Stake` pratikte yalnızca *"bence bu iş önemli"* demektir.

**Sonuç:** kural, öz-beyan kaçağını `Confidence`'tan `Stake`'e taşıdı ve taşıdığı yere
"gözlemlenebilir" etiketi yapıştırdı. `Confidence`'a getirilen kısıt (tek başına Tier
düşüremez) `Stake`'e **getirilmedi**. Oysa Tier 3'ün formül-yolu artık tümüyle `Stake` beyanına
bağlı: düşük Stake beyan et → InfoNeed düşsün → Tier 2. Kaçak kapanmadı, **ad değiştirdi.**

### D-5.3 — ENS-2003 D-5 kalıbıyla karşılaştırma

| | ENS-2003 D-5 | `plan-first.md` §"Öz-beyan kaçağı — KAPALI" |
|---|---|---|
| İddia | borç kapatıldı | kaçak **KAPALI** (`:28` başlığı) |
| Gerçek | borç **taşındı** | kaçak **taşındı** (Confidence → Stake) |
| Belirti | kapatan mekanizma taşınan yerde tekrarlanmamış | `Confidence` kısıtı `Stake`'e uygulanmamış |
| Tanı | kapanış ilanı, kapanışın kendisinden önce yazılmış | aynı |

`DEFECT-REGISTER-VERIFICATION.md:796` bunu zaten bir **kalıp** olarak sicile geçirmiş:
> *"Kalıp 8: '**Öz-beyan kalibre edilmemiş**' (W8d, W7, W7d, B2, B1, D1_residual, G5, W2_P4 …)"*

`plan-first.md:41` bu kalıba doğru biçimde atıf yapıyor — **ama kendisi kalıbın 9. üyesidir.**
Bir kusur kalıbını doğru teşhis edip aynı belgede tekrarlamak, kalıbın gücünün kanıtıdır.

### D-5.4 — Üçüncü halka hiç ele alınmamış

Kaçak zinciri üç halkalı:
1. `Confidence` — öz-beyan (kural gördü, kısmen kısıtladı, D-5.1'e göre başarısız)
2. `Stake` — öz-beyan (kural görmedi, "gözlemlenebilir" dedi)
3. **"Bu iş Tier N'dir" kararının kendisi** — öz-beyan (kural hiç tartışmıyor)

3. halka en dıştaki ve en güçlü olanıdır: Tier'ı atayan, işi yapacak olan tarafın kendisidir;
bağımsız bir Tier doğrulaması yoktur. `plan-first.md:54`'ün çaresi *"Şüphede kal? Sahibine
sor"* — ama sahip, D-4.4'te gösterildiği gibi tek sözcükle tüm protokolü kapatabilen taraftır.
Öz-beyan zincirinin en dış halkası, aynı zamanda muafiyet yetkisini elinde tutan halkadır.

**Talep T-6:** Tier atamasını **gözlemlenebilir proxy'ye** bağla: beyan edilen Stake'e değil,
ölçülebilir olguya — kaç Külliyat yapıtı değişti, `status: ratified` mı, REGISTRY numarası
harcanıyor mu, `git diff --stat`. Bu ölçütler kabaca operax'ın heuristikleridir; bu da D-1'in
sonucunu doğrular: **ENS'in gerçekten ihtiyacı olan şey heuristiktir.** ENS-3022 bu yükü
taşıyamaz. Dürüst çözüm heuristiği kabul edip ENS-3022 kılıfını kaldırmaktır.

## 6. Katalog doğruluğu

**`survives`.** Bu, denetlenen katmanın en sağlam parçası. Uydurma ajan **yok**.

### Doğrulama: `advisor-skills.md` katalogu ↔ `.claude/agents/` ↔ ROSTER

`.claude/agents/` dizinindeki gerçek dosyalar (10): `ens-philosopher`, `ens-skeptic`,
`ens-researcher`, `ens-style-guardian`, `ens-architect`, `ens-ai-architect`,
`ens-backend-architect`, `ens-test-engineer`, `ens-memory-engine`, `ROSTER.md`.

| Katalogda (`advisor-skills.md`) | Dosya var mı | ROSTER "Aktif" | Sonuç |
|---|---|---|---|
| `ens-philosopher` `:20` | ✅ | ✅ `ROSTER:13` | doğru |
| `ens-researcher` `:21` | ✅ | ✅ `:15` | doğru |
| `ens-architect` `:22` | ✅ | ✅ `:17` | doğru (bkz. D-6.2) |
| `ens-ai-architect` `:23` | ✅ | ✅ `:18` | doğru |
| `ens-memory-engine` `:24` | ✅ | ✅ `:21` | doğru |
| `ens-backend-architect` `:25` | ✅ | ✅ `:19` | doğru |
| `ens-test-engineer` `:26` | ✅ | ✅ `:20` | doğru |
| `ens-style-guardian` `:27` | ✅ | ✅ `:16` | doğru |
| `ens-skeptic` `:39` (Denetim) | ✅ | ✅ `:14` | doğru — ve **doğru bölümde** |
| `ens-ceo` `:31-33` | ❌ | ROSTER `:26` "Ertelenmiş" | **doğru biçimde işaretlenmiş** |

ROSTER'ın 9 aktif ajanının **tamamı** katalogda var; katalogda ROSTER'da olmayan hiçbir ENS
ajanı yok. Yetenek atamaları ROSTER'ın "Görev" sütunuyla uyumlu (örn. `ens-memory-engine`
→ "Memory / Context Score / decay" ↔ `ROSTER:21` "Company Memory/Context Score … decay").

`.claude/skills/adversarial-test/SKILL.md` ve `.claude/skills/session-handoff/SKILL.md`
gerçekten var — `work-protocol.md:38,82` ve `advisor-skills.md:40` atıfları geçerli.

**En çok takdir ettiğim ayrıntı:** `advisor-skills.md:31-33` `ens-ceo`'nun **yokluğunu** açıkça
belgeliyor: *"ROSTER'da planlıdır ama ajan dosyası YOKTUR … Bu satır, katalogda uydurma ajan
bulunmadığını göstermek için burada duruyor."* Bu, denetlenebilirlik açısından örnek bir
davranıştır ve `plans/01:172`'nin done criteria'sını gerçekten karşılar.

### D-6.1 — `ens-ceo` yokluğu, Madde XIV'e göre AÇIK BİR YÖNETİŞİM BOŞLUĞU (kayıtsız)

Katalog `ens-ceo`'nun yokluğunu bildiriyor ama **sonucunu** yazmıyor. Madde XIV
(`ENS-0000:244-245`):
> *"`ens-ceo` uzun vadeli hizayı korur; `ens-skeptic` sağlamlığı korur; `ens-style-guardian`
> tutarlılığı korur. **Önemli kararlarda hiçbiri atlanmaz.**"*

Yani Anayasa, önemli her kararda `ens-ceo`'yu zorunlu kılıyor; ajan yok. Bu, katalogun bir
kusuru değil, **kataloğun ortaya çıkardığı anayasal bir açıktır** — ve kaydedilmemiş.
(Depoda elle yazılmış `CEO-0001`, `CEO-0002`, `CEO-0003` hizalama incelemeleri bu boşluğu
kısmen dolduruyor; `plans/01` için böyle bir kayıt **yok** — oysa yeni bir yönetişim katmanı
kurmak "önemli karar"dır.)

**Talep T-7:** kataloğa satır: *"`ens-ceo` yok → Madde XIV'in 'önemli kararlarda atlanmaz'
şartı bugün karşılanamıyor; boşluk elle yazılan `CEO-NNNN` kayıtlarıyla dolduruluyor. Bu bir
açık borçtur (ROADMAP'e G-NN olarak girmeli)."*

### D-6.2 — Faz sütunu yok: Madde VII / JIT ilkesi katalogda kaybolmuş

ROSTER her ajana bir **Faz** atıyor ve `:7-8` açık bir ilke koyuyor:
> *"**JIT ilkesi:** bir agent, ancak fazı geldiğinde gerçek dosya olur."*

`advisor-skills.md` kataloğunda **Faz sütunu yok.** Sonuçlar:
- `ens-architect` — `ROSTER:17` **"Faz 3+ (freeze şimdi)"**. Katalog `:22` onu koşulsuz
  danışman olarak sunuyor; "freeze" durumundan söz etmiyor.
- `ens-backend-architect` (Faz 4-5), `ens-test-engineer` (Faz 4-5) — katalogda faz kaydı yok.
  Yalnızca `:25`'te dolaylı bir koruma var ("Yalnız Accepted ADR'lere dayanır (Madde VII)").
- `ens-memory-engine` — `ROSTER:21` "Faz 4-5 (**erken materyalize**, 2026-07-24 — K5 aktif iş)".
  Katalog bu istisnai durumu göstermiyor; okuyucu onu normal bir danışman sanır.

Madde VII (Faz Modeli) ve ROSTER'ın JIT ilkesi, kataloğun tek başına okunmasıyla ihlal
edilebilir. Düşük şiddetli ama gerçek: **katalog, ROSTER'ın taşıdığı bilgiyi kaybederek
kopyalıyor** — yani `footprint-ladder.md:40-41`'in "mevcut listeyi kontrol et, aynısı varsa
genişlet" kuralına göre, ROSTER'ı **genişletmek** (bir "Danışılacak iş türü" sütunu eklemek)
ikinci bir katalog üretmekten daha dar bir çözümdü.

### D-6.3 — `Explore` / `Plan` satırları depodan doğrulanamaz

`advisor-skills.md:28-29` `Explore` ve `Plan`'ı katalog satırı yapıyor. Bunlar ENS yapıtı
değil, platform ajanlarıdır; `.claude/agents/`'ta dosyaları **yok** ve `ROSTER.md`'de
**geçmiyorlar** — oysa kural `:50` "Yeni danışman ajan eklenince bu kataloğa satır ekle ve
**ROSTER'ı güncelle**" diyor. Katalog kendi kuralını kendi satırlarında uygulamamış.

**DOĞRULANMADI:** bu iki ajanın bu ortamda kullanılabilir olup olmadığını depodan
doğrulayamadım; uydurma olduklarını **iddia etmiyorum**. Kusur, varlıklarında değil,
ROSTER ile senkronsuzluklarındadır.

### D-6.4 — `advisor-skills.md:66`'daki "Madde XIV — rol ayrımı" atfı zayıf

Madde XIV'in başlığı **"Karar Yönetişimi (RFC / ADR Yaşam Döngüsü)"**. Rol ayrımını
yalnızca `:244-245`'te üç koruyucuyu sayarak dolaylı yapar. Ajanların yetki kaynağı ise
**Madde XII**'dir — `ROSTER.md:3` bunu doğru biçimde yazıyor: *"**Yetki:** ENS Anayasası,
**Madde XII**"*. Ayrıca `advisor-skills.md:4`'ün "ENS'in ajan kadrosu zaten rol-ayrımlıdır
(Madde XIV)" ifadesi için doğru kaynak **GOV-010 `governance/roles.md`**'dir (Author /
Validator / Governance). Küçük ama düzeltilmeli — atıf zinciri ENS'in kendi Madde VIII
yükümlülüğüdür.

## 7. Plan kendi şablonuna uyuyor mu?

_(doldurulacak)_

## Katıldığım noktalar

_(doldurulacak)_

## Sahibine talepler

_(doldurulacak)_
