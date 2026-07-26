---
id: SKR-040
type: skeptic-review
origin: ENS-2003
depends_on: [ENS-2003, ENS-2001, ENS-2002, ENS-4000, ENS-4010]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
---

# SKR-040 — Company Memory v0.3 (confidence-koşullu decay katmanı) Saldırısı

**Bağımsızlık beyanı:** Bu inceleme, ENS-2003 v0.3'ü yazan `ens-philosopher` çağrısından ayrı,
taze context'te yapıldı (G2/G3 öz-onay yasağı). Beş prior-art atıfı bağımsız olarak web'de
doğrulandı; iki matematiksel iddia (formül limitleri + γ=1 anchor oranları) elle yeniden
hesaplandı; Faz-4 kodu (`CompanyMemory.cs`) satır satır teoriyle karşılaştırıldı.

## Verdict
`wounded` — **çekirdek tez sağ çıkıyor** (math doğru, 5/5 kaynak gerçek, novelty dürüstçe —
hatta ihtiyatla — konumlanmış, audit tasarımı ve curator-kısıtı tutarlı); ama **teori-kod
desenkronu** ratified'a dönüşü engelliyor: (D1) §Failure condition formülün 7000'de henüz
kodlanmadığını iddia ediyor — **kod bunu yalanlıyor** (DecayFunction/Salience/FindStale/Verify
mevcut); (D2) kodun `Salience`'ı §3a'nın "iki dik ekseni karıştırma" uyarısına rağmen retention
× decay çarpıyor; (D3) `γ≥1` kısıtı gerekçesiz, kodda zorlanmıyor ve elde tek anchor-veriyle
(pusula) çelişiyor.

## Yenilik incelemesi
**Beş atıfın hepsi gerçek ve doğru konumlanmış (uydurma yok):**

| Kaynak | Doğrulama | Konum dürüst mü? |
|--------|-----------|------------------|
| **ECC** (affaan-m/ECC) | ✅ github.com/affaan-m/ECC gerçek; `continuous-learning-v2` skill, confidence-scored instincts + decay (`-0.02/hafta gözlemsiz`) + re-validation | Dürüst. Not: ECC decay'i **lineer-additif** (confidence'a −0.02/hafta), ENS'in **çarpımsal-üstel** salience'ı değil — mekanizma sınıfı aynı, form farklı. Doc bunu "alan deseninin kaynağı" (confidence+asserted+verified) diye sınırlı sunmuş — doğru. |
| **Hermes Curator** (Nous Research) | ✅ hermes-agent.nousresearch.com/docs/.../curator gerçek; inactivity-tetikli (interval 7g, min_idle 2s) | Dürüst **ama yumuşak**: Hermes curator **varsayılan olarak otonom prune/archive yapar** (stale@30g, archive@90g); LLM-consolidation kapalı. Doc "kaynaklarda curation otonom **olabilir**" diyor — Hermes'te otonom prune **varsayılandır**, "olabilir" bunu hafifletiyor. Yine de ENS'in deltası (yalnızca-sinyal) gerçek ve dürüst. |
| **Adaptive-decay-KG** (arXiv:2604.26970, Karhade, 22 Nis 2026) | ✅ gerçek; uniform decay yerine velocity+volatility ile parametrize **sürekli decay surface**, 3-seviye (domain/context/entity) | Dürüst. **Kritik gözlem:** ENS'in λ(c)=λ_base·(1−c)^γ per-Purpose-tipi formu, bu makalenin "context-level parameters"ının **özel bir hâli/sadeleştirmesi**dir (sürücü=confidence, kategori=Purpose-tipi). ENS bunu "yeni yasa değil" diyerek zaten kabul ediyor. |
| **TempValid** (ACL 2024 long.580, Huang ve ark.) | ✅ gerçek; öğrenilebilir confidence × zaman etkileşimi, rule-based TKG forecasting | Dürüst. confidence→decay eşleşmesinin "sabit değil öğrenilen" olması iddiası kaynağa sadık. |
| **Temporal RAG freshness** (arXiv:2509.19376, Grofsky) | ✅ gerçek; half-life recency prior, freshness ≠ similarity ayrımı | Dürüst. "sürekli form (sabit kademe değil)" gerekçesi kaynağa uygun. |

**Novelty iddiası dürüst mü?** Evet — hatta **ihtiyatlı**. Doc "Yeni teorik yasa değil, KG/RAG
mühendisliğinin ENS invariant'larına sentezi (E1→E2)" diyor ve gerçek katkıyı **dört bağlamaya**
indiriyor (Purpose-tipi koşulluluk, asserted/verified audit çaparı, curator-yalnızca-sinyal,
confidence-sürekli). SKR-001 dersi (dar delta önden) doğru uygulanmış. Özgünlük **abartılmamış**;
mekanizma sınıfının literatürde var olduğu açıkça kabul edilmiş. Bu boyutta saldırı isabet
bulamıyor — kaynaklar sağlam, konumlama dürüst.

## Yanlışlanabilirlik
İyi durumda. Doc failure-condition'da tam olarak neyin çürüteceğini söylüyor: "yanlış-kalibre
sürekli eğri, iyi-seçilmiş kademelerden daha kötü davranabilir" — yani formun sürekliliği tek
başına üstünlük **kanıtlamaz**, γ/λ_base'in ampirik doğruluğu gerekir. Bu falsifiable ve dürüst.

**Formül limitleri elle doğrulandı (Soru 1):**
- `λ(1) = λ_base·(1−1)^γ = λ_base·0^γ = 0` (γ>0) → certainty **sönmez**, salience=exp(0)=1. ✅ Doğru.
- `λ(0) = λ_base·1^γ = λ_base` → en hızlı sönüm. ✅ Doğru.
- c'de monoton azalan: `(1−c)^γ`, γ>0 için c'de azalan. ✅ Doğru.
- "1.0 muaf, özel-durum değil limittir" iddiası matematiksel olarak **geçerli**.

## Varsayım haritası
| Varsayım | Kırılma koşulu |
|----------|----------------|
| Salience üstel decay biçimi doğru | Gerçek unutma eğrisi üstel değilse (ör. power-law/Lindy — adaptive-decay-KG'nin bulduğu) yanlış aile |
| confidence tek decay sürücüsü | velocity/volatility (gözlem sıklığı, değişkenlik) confidence'tan bağımsız katkı yaparsa (adaptive-decay-KG bunu **iki eksen** olarak ölçüyor — ENS tek eksen kullanıyor) |
| γ≥1 doğru kısıt | Veri γ<1 istiyorsa (aşağıda: pusula endpoint fit γ≈0.72 istiyor → kısıt **fazla dar**) |
| tek (γ,λ_base) Purpose-tipi başına yeter | Bir Purpose-tipi içinde confidence-decay ilişkisi tek power-law'a oturmuyorsa |
| retention ⊥ salience ortogonalitesi korunuyor | Retrieval sıralaması ikisini çarparsa kavramsal ayrım pratikte bulanır (bkz. D2 — kod tam bunu yapıyor) |

## En güçlü karşı-argüman (steelman)
**"Formun sürekliliği yapısal üstünlük diyorsun ama seçtiğin 2-parametreli power-law ailesi elde
tek referans veriye (pusula) hiç oturmuyor — üstelik oturması için gereken γ senin kendi γ≥1
kısıtını ihlal ediyor."**

Elle hesap (Soru 5, bağımsız doğrulandı — pusula çapa-noktaları c=0.95→180g, 0.65→90g, 0.40→30g;
γ=1'de `t_stale ∝ 1/(1−c)`):
- c=0.95: 1/0.05 = 20 → normalize 12.0
- c=0.65: 1/0.35 = 2.857 → 1.71
- c=0.40: 1/0.60 = 1.667 → 1.0
- **γ=1 oranları = 12 : 1.7 : 1** — doc'un iddiası **DOĞRU** (gözlenen 6:3:1 değil). ✅

Ama daha derini (doc'un söylemediği): power-law'da oran yalnızca γ'ya bağlı (λ_base ölçek, oranı
etkilemez). İki bağımsız oran-kısıtı:
- Uç-uca (0.95↔0.40, gözlenen 180/30 = **6**): `(0.60/0.05)^γ = 12^γ = 6` → **γ = ln6/ln12 ≈ 0.72**
- Orta-alt (0.65↔0.40, gözlenen 90/30 = **3**): `(0.60/0.35)^γ = 1.714^γ = 3` → **γ ≈ 2.04**

**İki kısıt tutarsız (0.72 ≠ 2.04): pusula'nın üç çapa-noktası HİÇBİR tek γ ile fit edilemez.**
Dahası, uç-uca fit'in istediği **γ≈0.72, doc'un kendi `γ≥1` tabanının ALTINDA**. Yani doc "γ=1
12:1.7:1 veriyor, γ bir serbestlik derecesi" diyerek aile-içi kalibrasyonun sorunu çözeceğini
**ima ediyor** — oysa (γ≥1 kısıtıyla) aile pusula'nın uç oranına **ulaşamıyor bile**. Bu, doc'un
kendi failure-condition'ını **söylediğinden daha güçlü** kılıyor.

**Steelman'in adil sınırı:** doc zaten pusula'nın kademelerini "keyfi/kalibrasyonsuz" diye
**reddediyor**, dolayısıyla pusula ground-truth değil — fit edememek tek başına diskalifiye değil.
Ama o zaman doc pusula-fit'i **ima etmemeli**; savunmayı yalnızca yapısal argümana (süreksizlik-yok
+ per-Purpose-tipi) dayandırmalı. Şu anki metin iki yolu karıştırıyor.

## İç tutarlılık
**Teori-kod desenkronu (en kritik bulgu) — `CompanyMemory.cs` teoriyle çelişiyor:**

- **D1 — Failure-condition olgusal yanlış.** ENS-2003 satır 280-283: *"7000 bu formülü **henüz
  implemente etmemiştir** (CompanyMemory yalnızca retention∝|Learning|'i kodladı) → eng-kanıt
  hâlâ E1."* Kod bunu **yalanlıyor**: `DecayFunction.Rate` (λ(c)=λ_base·(1−c)^γ, satır 111),
  `Salience` (exp, satır 71), `HalfLifeDays`, `Verify`/`LastVerifiedOf` (asserted/verified,
  satır 44-48), `FindStale` (stale bayrağı, satır 81-89) hepsi **mevcut** ve TRACE-comment'leri
  "§3a/§3b"ye işaret ediyor. Doc kod yazıldıktan sonra güncellenmemiş. E-grade E1 kalabilir
  (implemented-ama-kalibre-değil), ama cümlenin kendisi düzeltilmeli.

- **D2 — `Salience` iki dik ekseni karıştırıyor.** §3a formülü: `salience(m,t) = exp(−λ(c)·Δt)`
  (saf, (0,1]). §3 açık uyarı: *"İki dik eksen (karıştırma)"* — retention önceliği (∝|Learning|,
  önem) ⊥ salience decay (∝zaman×belirsizlik, tazelik). Kod satır 71: `Salience = RetentionPriority
  × exp(−rate·ageDays)` — **retention'ı salience'a çarpıyor**, teorinin "karıştırma" dediğini
  yaparak. `FindStale` (satır 87) RetentionPriority'yi **bölerek** saf decay faktörünü izole ediyor
  (o kısım teori-tutarlı), ama `Salience`-adlı metodun döndürdüğü şey §3a'nın `salience`'ı **değil**,
  bir birleşik-retrieval-skoru. Ortogonalite pratikte (FindStale, retention-imhadan-korur) korunuyor,
  ama isim/tanım drift'i gerçek ve tam da §3'ün uyardığı kafa karışıklığı. Öneri: kod metodunu
  `RetrievalScore` diye yeniden adlandır + §3a'ya sadık saf `Salience` ayır; VEYA teoriyi "nihai
  sıralama = retention × salience" diye keskinleştir.

- **D3 — γ≥1 kısıtı gerekçesiz + kodda zorlanmıyor.** Doc γ≥1'i formül satırında **beyan ediyor**
  ama **türetmiyor** (Soru 1: keyfi mi?). Savunulabilir bir gerekçe var — γ≥1, decay-hızının c=1'de
  eğimini sınırlar/sıfırlar (γ>1'de d/dc→0; γ=1'de sonlu −λ_base), certainty yakınında pürüzsüz
  "korumalı bölge" verir; γ<1'de c→1'de eğim ∞'a gider (cusp), "certainty korunur" tasarım-niyetine
  aykırı. **Ama doc bu gerekçeyi vermiyor.** Ayrıca kod guard'ı (satır 108) yalnızca `γ>0` zorluyor,
  `γ≥1`'i **değil** — yani kod γ=0.72'ye izin veriyor (teori yasaklıyor). Kod-yorumu "γ≥1" diyor ama
  guard `gamma <= 0`. Teori-kod tutarsızlığı + üstelik (D-steelman) veri γ<1 istiyor. Karar: ya γ≥1'i
  türet (pürüzsüzlük) ya gevşet (veri <1 istiyor) — ama beyan/gerekçe/kod üçü **hizalanmalı**.

**Terminoloji sürüklenmesi (Soru 7) — küçük:** Beş yeni sözlük terimi (Salience Decay,
`asserted_at`, `last_verified`, Stale Flag, Memory Curator) ENS-4000'e temiz girmiş; alias-yasağı
notu (`asserted_at` ≠ Decision Object `Evidence`) doğru ve gerekli. **Tek açık homonim riski:**
ENS-2003 "**memory assertion**" ifadesini genel "saklanmış confidence'lı olgu/kayıt" anlamında
kullanıyor; ama ENS-4010'da **Assertion** resmî bir node-tipi (Claim/Evidence/Learning üsttürü,
kendi profiliyle). Memory kaydı aslında commit-edilmiş **Decision** node'u (§1), Assertion node'u
değil. "assertion" kelimesinin gevşek kullanımı, ENS-4010'un formal Assertion'ıyla karışabilir —
tıpkı `ens-core:derived_from` ≠ `ens-meta:derived_from` homonim dersindeki gibi. §Homonim/alias
notuna bir satır ("memory assertion ≠ ENS-4010 Assertion node; kayıt-birimi Decision'dır")
eklenmeli. Bloke etmez.

**Audit tutarlılığı (Soru 3) — geçti.** `asserted_at`/`last_verified` ayrımı "kaydı asla silme"
ile **tutarlı**: kodda `AssertedAt` immutable record-alanı, `Verify` **ayrı** `_lastVerified`
map'ini güncelliyor, orijinal kayıt hiç mutasyona uğramıyor, `Record` yalnızca ekliyor, **Delete
yok**. `last_verified` güncellemesi audit-ihlali **değil** — asserted_at (giriş anı) korunuyor.
Tek incelik: `_lastVerified` yalnızca **son** teyidi tutuyor, teyit-olaylarının tam geçmişini
değil; eğer EC-001 audit "her teyit olayı" izlenebilirliği isterse bu bir event-log gerektirir.
§3a "son teyit anı" tanımı verili ölçekte tutarlı — ama never-delete'in teyit-olaylarına da uzanıp
uzanmadığı bir keskinleştirme sorusu. Bloke etmez.

**Curator P5/P7 sadakati (Soru 4) — geçti.** Kod curator-yolunu **yapısal** olarak sinyale
kısıtlıyor: `FindStale` bir liste **döndürüyor**, silmiyor/mutasyona uğratmıyor; sınıfta hiç
Delete/otonom-supersede yok. Teori (§3b) "cron-tetikli silme yok, otonom mutasyon yok" diyor ve
kod bunu **doğruluyor** — hatta teori "yalnızca disiplinle korunur, yapıyla değil" diye failure
işaretlemiş, ama kodda **yapısal olarak** da korunuyor (silme metodu yok). "Curator" kelimesi
gizli otomatik-aksiyon kapısı **açmıyor**. Hermes'ten deltası (otonom-prune → yalnızca-sinyal)
gerçek ve dürüst.

## Sahibine talepler (ratified'a dönmeden önce)
1. **D1 (blocking):** §Failure-condition "7000 formülü henüz implemente etmemiştir / yalnızca
   retention∝|Learning| kodladı" cümlesini düzelt — DecayFunction/Salience/FindStale/Verify
   **kodlandı**; E1 gerekçesi "kodlanmadı" değil "**kalibre edilmedi (γ ampirik değil)**" olmalı.
2. **D2 (blocking):** Kod `Salience`'ının retention×decay çarpımı ile §3a'nın saf `salience`'ı
   arasındaki isim/tanım drift'ini gider — ya kodu (`RetrievalScore` + saf `Salience`) ya teoriyi
   ("nihai sıralama = retention × salience, ama iki eksen kavramsal olarak ayrı") hizala. §3'ün
   "karıştırma" uyarısı ile kodun fiili çarpımı çelişmemeli.
3. **D3 (blocking):** `γ≥1`'i (a) türet (c=1'de pürüzsüzlük/bounded-slope argümanı) **veya**
   (b) gevşet, ve **kod guard'ını (`γ>0`) beyanla hizala**. Beyan/gerekçe/kod üçü tutarlı olsun.
4. **Failure-condition'ı güçlendir (talep, non-blocking):** "γ=1 fit etmiyor" yerine bağımsız
   bulguyu ekle — pusula'nın 3 çapa-noktası **hiçbir tek γ** ile fit edilemez (endpoint γ≈0.72 vs
   orta γ≈2.04), üstelik endpoint-γ mevcut γ≥1 tabanının altında; dolayısıyla savunma pusula-fit'e
   değil **yalnızca yapısal** (süreksizlik-yok + per-Purpose-tipi) argümana dayanmalı.
5. **Homonim notu (non-blocking):** §Homonim/alias'a "memory assertion ≠ ENS-4010 Assertion node
   (kayıt-birimi Decision'dır)" satırı ekle.
6. **Hermes konumlaması (non-blocking):** "curation otonom **olabilir**" → "Hermes'te otonom
   prune/archive **varsayılandır**; ENS bunu bilinçle yalnızca-sinyale indirir" olarak keskinleştir
   (delta daha güçlü görünür, dürüstlük artar).

---

*Katman özde sağlam: math doğru, beş kaynak gerçek ve dürüstçe konumlanmış, audit ve curator
tasarımı hem teoride hem kodda invariant-sadık. Yara özgünlükte ya da yanlışlanabilirlikte değil —
teori metninin kendi kodunun ilerisinde/gerisinde kaldığı üç desenkron noktasında (D1/D2/D3). Bunlar
ucuz düzeltmeler; ratified'a dönüş onlardan sonra.*
