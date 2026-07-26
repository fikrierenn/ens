---
id: SKR-043
type: skeptic-review
origin: ENS-2004
depends_on: [ENS-2004, SKR-042, ENS-2001, ENS-2003, ENS-4025, ENS-4000, ENS-0000]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
---

# SKR-043 — ENS Learning Theory v0.3.1 (SKR-042'ye yanıt) 2. Bağımsız Saldırı

> **Bağımsızlık beyanı (G2/G3).** Bu tur, ENS-2004 v0.3.1'i (SKR-042'ye yanıtı) yazan agent
> çağrısından tamamen ayrı, taze context'te yürütüldü. SKR-042 (1. tur) `wounded` idi; bu tur
> o üç talebin gerçekten kapanıp kapanmadığını ve düzeltmelerin yeni bir kaçak açıp açmadığını
> bağımsız sınar. Yazar öz-onay veremez
> ([GOV-000](../../governance/000-governance-principles.md) G4 — bağımsız validator).

## Verdict
**wounded** — SKR-042'nin üç talebinden **T1 ve T2 tam kapandı**; ama **T3 yalnızca kısmen
kapandı**: §Definition düzeltildi ve yeni "İki farklı yazım (P7 ayrımı)" paragrafı eklendi, oysa
aynı belgenin **§Theoretical model §1'i (satır 128) hâlâ eski otomatik-yazma dilini taşıyor**:
"Learning bu farkı memory'ye **ve ilgililik/varsayım modeline yazar**." Bu cümle, üç bölüm yukarıda
eklenen "(b) model revizyonu **asla otomatik yazılmaz; insan onayından geçen bir öneridir**"
ifadesiyle **doğrudan çelişir** — yani T3'ün kapatmayı amaçladığı P7-çelişkisi belgenin biçimsel
model bölümünde canlı kalmıştır. Tek cümlelik düzeltmeyle giderilebilir, ama bir canon-adayı teori
kendi çekirdek invariant'ında (P7: otomatik-yazma yok) kendisiyle çelişirken `ratified` işaretlenemez.
`status: review` KALIR; T3-artığı kapanınca (muhtemelen tek micro-tur) `survives`.

## T1 incelemesi — TAM KAPANDI
§4a'nın "§Failure #1'e bağlantı" paragrafı (satır 197-208) artık **yalnızca commit-edilen
(seçilen) Alternative'in** EV kalibrasyonunu hedefliyor. "per-Alternative bir kalibrasyon iddiası
**değildir**" (satır 206) açıkça yazılmış; §2'nin counterfactual sınırına (`Y(a′)` asla gözlenmez)
ve ENS-2001 §Failure'ın unchosen-EV kırılganlığının **miras alındığına** atıf var (satır 202-205).

- **Kaynak doğrulaması (uydurma yakalama):** §4a, "ENS-2001 v0.3 §Failure de seçilmeyen Alternative
  EV'lerinin 'kaydedilmez ya da kaba' olduğunu kabul eder" diyor. Bunu bağımsız doğruladım:
  **ENS-2001 §Failure satır 241-243** birebir "Alternative-başına EV, *seçilmeyen* seçenekler için
  de değer tahmini ister; bu tahminler **kaydedilmez ya da kabaysa** ... seçim rasyonalitesi
  (ENS-2004 §5ii) bozulur" diyor. Atıf **gerçek ve doğru konumlanmış** — uydurma yok.
- **Artık "per-Alternative" izleri temiz.** Belgede kalan üç occurrence: satır 47 (v0.3.1 künye-notu,
  düzeltmeyi anlatıyor), satır 206 (negasyon: "değildir"), satır 325 (SKR-yanıt tablosu). Hiçbiri
  aşırı-iddia bağlamında değil. Satır 277'deki "Alternative-başına beklenen değeri saklamalı" ise
  §5(ii) **seçim rasyonalitesine** aittir — bu **outcome-bağımsız**tır (Actual'a değil, diğer
  Alternative'lerin commit-anı EV'lerine bakar), dolayısıyla per-Alternative EV saklamak burada
  meşrudur ve T1'in daralttığı EV-vs-Actual sapma iddiasıyla **çelişmez**. İki kavram doğru ayrışmış.

**Sonuç:** T1 çözümü hem doğru hem içsel-tutarlı. Kabul.

## T2 incelemesi — TAM KAPANDI
- **P5 gerçekten künyede.** `principles: [P4, P3, P5, P6, P7]` (satır 10). SKR-042'de eksik olan
  P5 eklenmiş.
- **Öneri-yorgunluğu gerçekten yeni bir risk tanımlıyor**, yüzeysel cümle değil (satır 284-296):
  hacim→(a) P7 rubber-stamp (P7 fiilen boşalır, gözetimsiz otomasyona eşdeğer) ya da (b) ignore
  (mekanizma işlevsiz) ikilemi adlandırılmış; Hermes'in makine-gate'inin insan-yükünü *önceden*
  kıstığı, §4a'nın gate'inin (proof-trace + L1-etiket) yalnızca önerinin *biçimini* kısıtladığı
  (hacmini değil) dürüstçe kabul edilmiş — bu, SKR-042'nin steelman'ının tam da istediği itiraf.
  §4a'ya paralel "Dikkat kıt kaynaktır (P5)" paragrafı da (satır 186-195) mekanizmayı somutlaştırıyor.
- **ENS-3022 (Decision Gravity) notu dürüst.** "önceliklendirme **henüz operasyonelleşmemiştir** —
  hangi mekanizmanın (Decision Gravity, eşik, batch) hacmi kestiği tasarım borcudur" (satır 194-195,
  295-296). Aşırı-iddia yok: ENS-3022'yi bir *olası* bağlanma noktası olarak işaret ediyor, çözüldü
  demiyor. ENS-3022 dosyası mevcut ve gerçekten Decision Gravity — atıf geçerli.

**Sonuç:** T2 çözümü sağlam ve dürüst. Kabul.

## T3 incelemesi — KISMEN KAPANDI (kritik bulgu)
Düzeltmenin **hedef aldığı yer** (§Definition, satır 57-71) doğru düzeltilmiş:
- Satır 60: edilgen "yazılan güncelleme" → "güncelleme _önerisine_ dönüştürülmesi". ✅
- Yeni "İki farklı yazım — karıştırılmamalı (P7 ayrımı)" paragrafı (satır 65-71): (a) sonucun
  kayda geçmesi = otomatik olgu-kaydı; (b) ilgililik/varsayım modelinin revizyonu = "**asla
  otomatik yazılmaz; bir insanın onayından geçen bir öneridir**". Bu ayrım P7 ile tutarlı ve
  §4a'yla, §3 attribution merdiveniyle (L0 = "yalnızca kayıt (memory)", otomatik), §5 ile
  uyumlu. Bu kısım iyi.

**Ama T3 tüm dosyaya uygulanmadı — biçimsel model bölümünde artık kaçak var.**

**§Theoretical model §1 (satır 122-128), "Learning nedir — prediction error + memory update":**
> "Dış kontrol grubu gerektirmez ... Learning bu farkı memory'ye **ve ilgililik/varsayım modeline
> yazar**." (satır 128)

Bu cümle, düzeltilen §Definition'ın **aynısını biçimsel model dilinde tekrarlar — ama eski,
otomatik-yazma kipiyle.** "yazar" (aktif, otomatik) fiili, tam da §Definition'ın "İki farklı yazım"
paragrafının (b) maddesinin **yasakladığı** şeyi söyler: model revizyonunun otomatik yazılması.
İki ifade aynı belgede birbirini **doğrudan yalanlıyor.** T3'ün SKR-042'deki gerekçesi ("§1
Definition ↔ §4a gate uzlaştırılmamış") tam da bu çelişkiydi; owner çelişkiyi §Definition prose'unda
kapattı ama §Model 1'deki ikizini bıraktı. Biçimsel model bölümü, prose tanımından daha az değil
daha çok bağlayıcıdır — dolayısıyla bu, "kolay olanı seçip zoru bırakma" değil, **eksik tarama**
kaynaklı gerçek bir artık-çelişkidir.

**İkincil (daha yumuşak) eko — §Implications satır 239:** "Context relevance (ENS-2002) L1 ile
**beslenir** (OC3 kapandı)." "beslenir" edilgen-otomatik kipte relevance-modelinin L1'den otomatik
güncellendiğini ima ediyor; §4a'ya göre relevance-model ayarı P7-gate'li bir *öneridir*. Bu, satır
128 kadar sarih değil (L1'in relevance'a *girdi* olması ile relevance-model *revizyonu* farklı
okunabilir), ama aynı desenden — biçimsel/implications bölümleri relevance-güncellemeyi hâlâ
otomatik dille anıyor. §Relationships satır 244 ise **doğru** yapmış: "double-loop relevance'ı
günceller; §4a relevance-model ayarını **önerir** (insan onayıyla uygulanır, P7)" — aynı bülten
içinde uzlaştırmış. Yani belge T3'ü bazı yerlerde uygulamış, satır 128 ve (daha zayıf) 239'da
uygulamamış: **kısmi tarama.**

## Yeni kaçak kontrolü (talep 4) — T1/T2 düzeltmeleri temiz
- **T1 daraltması ↔ §5(ii):** yeni bir çelişki açmadı (yukarıda; §5(ii) outcome-bağımsız, per-Alt
  EV meşru).
- **T2 eklemesi ↔ §Implications/§Relationships/Historical:** §Failure'daki yeni "öneri-yorgunluğu"
  koşulu ve P5 paragrafı, Historical tablo (satır 89: "Öneri, aksiyon değil (P7 gate)") ve
  §Relationships (satır 244-245: "önerir ... P7") ile tutarlı; çelişki yok.
- Tek yeni/artık tutarsızlık **satır 128** (ve zayıf eko 239) — ve bu T1/T2'den değil, T3'ün
  eksik uygulanmasından geliyor.

## Glossary tutarlılığı (talep 5) — GEÇER
ENS-4000 v0.2.5 "Reflective Double-Loop" girdisi (satır 127-131) **EV/per-Alternative iddiasını
hiç içermez** — yalnızca "Assumptions/relevance-model/attribution-seviyesi için hedefli öneri ...
hiçbir zaman otomatik uygulanmaz — P7 ... yeni yasa değil ... GEPA/DSPy/Hermes" der. Dolayısıyla
T1'in per-Alternative→chosen daraltması glossary girdisini **etkilemez**; girdi §4a ile hâlâ
tutarlı, terminoloji sürüklenmesi yok, M1 + "skeptic bekliyor" doğru işaretli. Geçer.

## En güçlü karşı-argüman (owner'a, steelman)
*"Satır 128 zararsız bir kısaltma; §Definition zaten düzeltildi, okur bütünü §4a/§Definition'la
birlikte okur."* — Reddediyorum. Bir canon-adayı teorinin biçimsel model bölümü (§Theoretical
model §1), tanımın **normatif referans** halidir; "learning ... ilgililik/varsayım modeline yazar"
cümlesi, bir implementasyon ekibinin double-loop model revizyonunu **otomatikleştirmesini**
haklı çıkaracak metinsel dayanaktır (P7 ihlali). SKR-042 T3'ü tam da bu risk için açtı; çelişki
prose'da kapatılıp modelde bırakılırsa T3 **kapanmamıştır**, yalnızca yer değiştirmiştir. Tutarlılık,
"en çok atıf yapılacak" bölümde en katı uygulanmalıdır.

## Sahibine talepler (kapıyı geçmek için)
1. **§Theoretical model §1 (satır 128) T3-hizası (blocking, tek cümle).** "Learning bu farkı
   memory'ye ve ilgililik/varsayım modeline yazar" ifadesini §Definition'ın "İki farklı yazım"
   ayrımıyla uzlaştır: sonucun/farkın memory'ye kaydı otomatiktir; ilgililik/varsayım modeli
   revizyonu **P7-gate'li bir öneridir** (otomatik yazılmaz). Ör. "...farkı memory'ye yazar; model
   revizyonu için ise §4a uyarınca bir güncelleme *önerisi* üretir (P7)."
2. **§Implications satır 239 "beslenir" ekosunu netleştir (blocking-lite, tek cümle).** L1'in
   relevance'a *girdi* olması (otomatik) ile relevance-*model revizyonunun* P7-gate'li öneri olması
   ayrımını bir kelimeyle koru (ör. "L1 ile beslenir" → "L1 relevance sinyaline girdi verir;
   model ağırlığı revizyonu P7-önerisidir").

İkisi de metin-içi, tek-cümlelik, giderilebilir; T1/T2 gibi T3 de tüm dosyaya uygulanınca kavram
`ratified`'e döner — çekirdek sağlam, prior-art bağımsız-doğrulanmış (bkz. SKR-042), P5-boşluğu ve
EV-aşımı gerçekten kapandı; yalnızca T3'ün biçimsel-model artığı temizlenmeli.

## Kaynaklar (bağımsız doğrulandı — repo-içi)
- ENS-2001 §Failure satır 241-243 (unchosen-EV "kaydedilmez ya da kaba") — §4a atfını teyit.
- ENS-2004 satır 10 (P5), 65-71 (İki farklı yazım), 128 (artık kaçak), 197-208 (T1), 239 (eko),
  284-296 (T2 öneri-yorgunluğu) — doğrudan okundu.
- ENS-4000 v0.2.5 satır 127-131 (Reflective Double-Loop) — glossary tutarlılığı.
- ENS-3022 (Decision Gravity) dosyası mevcut — T2 önceliklendirme-notunun atıf hedefi gerçek.
- SKR-042 (1. tur, wounded) — üç talebin metni.
