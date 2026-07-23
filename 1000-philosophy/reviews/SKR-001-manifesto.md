---
id: SKR-001
type: skeptic-review
origin: ENS-1000
depends_on: [ENS-1000]
status: review
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-23
---

# SKR-001 — ENS Manifestosu (ENS-1000) Saldırısı

## Verdict
**wounded.** Manifestonun akıl yürütmesi tutarlı ve iyi yazılmış; ama merkezî sezgisi —
"şirket bir decision-producing cognitive system'dir" — **özgün değildir.** En az üç
yerleşik literatürde ve bir güncel ticari pazarda büyük ölçüde önceden vardır. Manifesto
bu öncülleri hiç anmıyor; bu haliyle bir *yeniden adlandırma* riski taşıyor (Anayasa Madde
VI — terminology drift / "yeni isim, eski içerik"). Faz 1'e geçmeden önce ENS'in gerçek
**delta**'sı dürüstçe konumlandırılmalı. Kavram çürük (refuted) değil, çünkü savunulabilir
bir özgünlük çekirdeği var — ama şu anki metin onu iddia etmiyor bile.

## Yenilik incelemesi (en ağır bulgu)

Manifesto §I-II, "miras alınan yanlış model süreç makinesidir; gerçek model karar/biliş
sistemidir" diyor ve bunu yeni bir kavrayış gibi sunuyor. Literatür bunu onlarca yıldır
söylüyor:

- **Management Cybernetics — Stafford Beer, *Brain of the Firm* (1972), Viable System
  Model.** Beer, organizasyonu "geri bildirimi izleyen, öğrenen, kendini düzelten bir
  beyin gibi" tasarlar. VSM'in System 4'ü tam olarak "intelligence", System 5'i "policy/
  identity"dir. ENS'in "nervous system" metaforu ve katmanlı biliş modeli, Beer'in
  yönetim sibernetiğinin doğrudan komşusudur. **Bu, en güçlü örtüşmedir ve Manifesto onu
  hiç anmıyor.**
- **Behavioral Theory of the Firm — Cyert & March (1963), Simon'ın bounded rationality'si
  üzerine.** Firma açıkça "karar verme sistemi" olarak modellenir; ampirik karar süreci,
  çoklu hedef, belirsizlik. ENS'in P1 (Decision atomdur) tezi buradan bağımsız değil.
- **Organizational Memory — Walsh & Ungson (1991), AMR 16(1):57–91.** Tanım: "bir
  organizasyonun geçmişinden, bugünkü kararlara uygulanabilen saklanmış bilgi." Bu,
  ENS'in Company Memory (P3) tanımının neredeyse birebir eşidir — üstelik beş "retention
  bin" (individuals, culture, transformations, structures, ecology) ile yapısallaştırılmış.
- **Ticari önceki-sanat — Gartner "Decision Intelligence Platforms" / "decision-centric
  enterprise."** Gartner, "decision-centric"in "data-driven"ın yerini alacağını (2028),
  kararların AI ile augment/otomasyonunu (2027), açıklanabilir decision model'leri ve
  decision lifecycle yönetimini zaten tanımlıyor. ENS'in Decision Engine + explainability
  + decision lifecycle öğeleri bu pazarla örtüşüyor.

**Sonuç:** "Şirket bir karar/biliş sistemidir" ENS'in keşfi değildir. ENS'in
savunulabilir özgünlüğü, olsa olsa şu üçünün *kesişimindedir* ve Manifesto bunları açıkça
iddia etmelidir:
1. **Mühendislik disiplini + standart** olarak paketleme (ISO-tarzı numaralandırma,
   traceability yasası, Külliyat) — Beer/March/Walsh bir *teori* verdi, bir *inşa
   standardı* değil.
2. **AI-çağı fizibilite argümanı (§IV):** sibernetik/davranışsal teoriyi *ilk kez
   uygulanabilir* kılan koşulların çöküşü. Bu, teorik değil, zamanlama/mühendislik
   katkısıdır — ve dürüstçe öyle adlandırılmalı.
3. **Nicelleştirme iddiası:** Decision Entropy/Gravity/Capital'i *ölçülebilir* yasalara
   dönüştürmek. Ama bkz. aşağıdaki uyarı — şu an bunlar metafor.

## Yanlışlanabilirlik

Manifesto §VII kendi yanılma koşullarını listeleyerek Madde X'i onurlandırıyor — bu iyi.
Ama iki koşul "kolay" seçilmiş, en öldürücü olan yumuşatılmış:

- **Ölçüm-atfedilebilirliği (attributability) sorunu yeterince ciddiye alınmamış.** §VII
  bunu bir olasılık olarak anıyor; oysa bu, tüm Learning katmanının (P4) *varlık koşuludur*.
  Kurumsal kararların sonuçları confounding (karışık nedensellik) altında karara
  atfedilemezse, "unmeasured decisions never improve" (LAW-LEARNING) doğru ama *boş* olur:
  ölçüm ilkesel olarak mümkün değilse, döngü hiç kapanmaz. Bu, ENS için teorik bir süs
  değil, potansiyel bir ölüm koşuludur ve Faz 1'de doğrudan ele alınmalı.

## Varsayım haritası

| Varsayım (§IV) | Kırılma koşulu | Risk |
|----------------|----------------|------|
| Akıl yürütme ucuzladı | Ucuz ama *sadık olmayan* (post-hoc) gerekçe | Yüksek |
| Context birleştirilebilir | Birleştirme hâlâ pahalı/gürültülü | Orta |
| Bellek kalıcı | Ölü context'in belleği yanıltır (LAW-ORG-MEMORY'nin kendi uyarısı) | Orta |
| Açıklama üretilebilir | Üretilen açıklama güven üretmez | **Yüksek** |

## En güçlü karşı-argüman (steelman)

> "ENS = Beer'in yönetim sibernetiği (1972) + Cyert-March davranışsal firma teorisi (1963)
> + Walsh-Ungson organizational memory (1991), yeni bir sözlükle yeniden markalanmış ve
> LLM'lerle inşa edilebilir kılınmış. Gerçekten yeni olan tek şey *zamanlama* (AI
> fizibilitesi) ve *paketleme* (bir standart). Öyleyse ENS bilimsel değil **mühendislik**
> bir katkıdır; 'yasaları' keşif değil, ödünç alınmış metafordur. Buna 'yeni bir disiplin'
> demek fazla iddialıdır."

Owner (ens-philosopher) bu itirazı ya **kabul edip** konumu dürüstçe yeniden çerçevelemeli
(ENS = önceki bilimi sentezleyen, AI ile fizibil hale gelen bir mühendislik disiplini), ya
da prior art'ta bulunmayan, **yanlışlanabilir ve özgün** en az bir teorik iddia üretmelidir.
İkincisi yapılamıyorsa, birincisi utanç değil — güç olur; ama sessizce "yeni" demeye devam
etmek Madde VI ihlalidir.

## İç tutarlılık

**Çelişki:** §IV, "açıklama artık üretilebilir" diyerek P6'yı (explainability
pazarlıksız) mümkün kılıyor. §VII ise LLM gerekçelerinin "sadık olmayan (post-hoc)"
olabileceğini itiraf ediyor. Eğer açıklama sadık değilse, P6 *karşılanmış görünür ama
işlevsiz* olur — yani explainability yapısal değil kozmetik olur; bu da Anayasa Madde
VI'nın açıkça reddettiği "black-box" durumunun kılık değiştirmiş hâlidir. Bu gerilim
çözülmeden P6 sağlam bir zemin değildir.

## Sahibine talepler (kapıyı geçmek için)

1. **Yenilik bölümü ekle.** ENS'i Beer (VSM), Cyert-March, Walsh-Ungson ve Gartner DI'ye
   karşı açıkça konumla. Her biriyle örtüşmeyi ve *delta*'yı tek tek yaz. "Yeni kavrayış"
   dilini, kanıtlanana dek kaldır.
2. **Disiplin iddiasını kalibre et.** ENS'in bilimsel mi yoksa mühendislik katkısı mı
   olduğuna karar ver ve o dili kullan. (Skeptic'in görüşü: şu an savunulabilir olan,
   *mühendislik disiplini + sentez* iddiasıdır.)
3. **Metaforları operasyonel kıl ya da geri çek.** Decision Entropy/Gravity/Capital için
   Faz 1'de her birine önerilen bir *ölçüm* getir. Ölçülemiyorsa, "law" değil "metaphor"
   olarak etiketle — aksi hâlde Madde VI (yanlışlanamaz iddia) ihlali.
4. **Attributability'yi Faz 1'de cevapla.** Confounding altında sonucun karara nasıl
   atfedileceğine dair somut bir duruş üret (ör. karşı-olgusal/counterfactual temelli,
   ya da yalnızca atfedilebilir karar sınıflarıyla sınırlı bir kapsam). Bu olmadan P4
   pratikte yanlışlanamaz.
5. **P6'nın faithfulness gerilimini çöz.** Açıklamanın sadakatini (post-hoc olmadığını)
   nasıl garanti/ölçtüğünü belirt; yoksa explainability'nin yapısal olduğu iddiasını
   yumuşat.

## Not
Bu saldırı ENS'i zayıflatmaz; **güçlendirir.** Manifesto bu beş talebi karşılarsa, ENS
"AI ERP" olmaktan çıkıp, önceki bilimi dürüstçe kabul eden ve onu ilk kez inşa edilebilir
kılan bir mühendislik disiplini olarak savunulabilir bir zemine oturur.

## Kaynaklar
- Beer, S. (1972). *Brain of the Firm.* — Viable System Model, management cybernetics.
- Cyert, R. M. & March, J. G. (1963). *A Behavioral Theory of the Firm.*
- Walsh, J. P. & Ungson, G. R. (1991). Organizational Memory. *Academy of Management
  Review*, 16(1), 57–91.
- Gartner, *Magic Quadrant / Market Guide for Decision Intelligence Platforms* ve
  "decision-centric enterprise" öngörüleri.
