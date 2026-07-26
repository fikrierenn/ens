---
id: SKR-041
type: skeptic-review
origin: ENS-2003
depends_on: [ENS-2003, ENS-2001, ENS-2002, ENS-4000, ENS-4010]
status: draft
owner: ens-skeptic
version: 0.1.0
last_reviewed: 2026-07-24
---

# SKR-041 — Company Memory v0.3.1 (D1/D2/D3 desenkron-kapanışı) Saldırısı

**Bağımsızlık beyanı:** Bu inceleme, ENS-2003 v0.3.1'i yazan (SKR-040'a yanıt veren)
`ens-philosopher` çağrısından **tamamen ayrı, taze context'te** yapıldı (G2/G3 öz-onay
yasağı). Bu, ENS-2003 v0.3 katmanının **2. bağımsız skeptic turudur** (1. tur: SKR-040 →
`wounded`, üç teori-kod desenkronu D1/D2/D3). Görev: düzeltmelerin **gerçekten** kapandığını,
düzeltme sırasında **yeni kaçak açılmadığını** ve kodun/testlerin tutarlı kaldığını bağımsız
doğrulamak. Üç matematiksel iddia (formül limitleri, γ anchor oranları, FindStale cebirsel
izolasyonu) elle yeniden hesaplandı; `CompanyMemory.cs` + `DecisionCapital.cs` +
`CompanyMemoryTests.cs` + glossary v0.2.4 satır satır okundu.

## Verdict
`survives` — **üç blocking desenkron (D1/D2/D3) gerçekten kapatıldı**; SKR-040'ta zaten sağ
çıkan çekirdek tez (math doğru, 5/5 prior-art gerçek, novelty dürüst) yara almadan duruyor;
teori/kod/glossary artık hizalı. Skeptic-kapısı geçildi → `ratified` hak edildi. **İki kayıt
altı:** (i) `dotnet test` bu context'te **çalıştırılamadı** (Bash/dotnet yok) — statik
tutarlılık tam doğrulandı, ama yeşil koşu owner/CI'da teyit edilmeli (fabrike edilmedi);
(ii) üç küçük **non-blocking** artık-kaçak açık (aşağıda) — hiçbiri ratified'ı bloke etmez,
ama sıradaki dokunuşta temizlenmeli. `canon` kararı bu edimin dışındadır (ayrı governance).

## D1 — "7000 formülü implemente etmemiştir" olgusal-yanlışı: KAPANDI ✓
SKR-040/D1, §Failure-condition'ın "7000 bu formülü henüz implemente etmemiştir" cümlesinin
**kodu yalanladığını** bulmuştu. v0.3.1 §Failure conditions (satır 324-328) artık dürüst:
*"ENS reference platform (7000) formülü **implemente etmiştir** — `DecayFunction.Rate` (λ),
`Salience` (bileşik retrieval skoru), `FindStale` (stale bayrağı), `Verify`/`LastVerifiedOf`
(asserted/verified ayrımı) kodludur... Açık olan **kod değil kalibrasyondur**."* E-grade
gerekçesi "kodlanmadı" → "**kalibre edilmedi** (γ/λ_base ampirik değil)" olarak düzeltildi;
`eng: E1` **doğru** kalır (implemented-ama-kalibre-değil).

**Bağımsız kod-teyidi:** İddia edilen beş yapıtın hepsi `CompanyMemory.cs`/`DecayFunction`'da
gerçekten mevcut: `DecayFunction.Rate` (satır 114), `Salience` (74), `FindStale` (90),
`Verify` (46), `LastVerifiedOf` (49), `HalfLifeDays` (127). İddia artık **gerçekten doğru**.

**Artık-kaçak (non-blocking, N1):** Aynı D1-olgusu **komşu dosyada hâlâ yaşıyor.**
`Ens.Kernel/Laws/DecisionCapital.cs` satır 8 yorumu: *"(ENS-2003 Company Memory henüz
kodlanmadı)."* Bu, D1'in ENS-2003 metninde düzelttiği cümlenin **birebir aynı olgusal
yanlışıdır** — yalnızca başka dosyada. Company Memory artık kodlu; bu yorum bayat/yanlış.
Zararsız (Capital'in stok-tutmadığı tasarım gerekçesini açıklıyor) ama D1-temizliği eksik
kaldı. Sıradaki dokunuşta güncellenmeli.

## D2 — `decayFactor` (saf) vs `Salience` (bileşik) ayrımı: KAPANDI ✓ (en kritik)
SKR-040/D2, kodun `Salience`'ı `RetentionPriority × exp(−λΔt)` döndürürken §3'ün "iki dik
ekseni karıştırma" uyarısıyla isim/tanım drift'i taşıdığını bulmuştu. v0.3.1 çözümü **teoriyi
koda hizalamak** (kodu değiştirmek değil — doğru yön, çünkü kod hep bileşiği döndürüyordu):

- **Teori (§3a, satır 171-193)** artık iki niceliği titizlikle **ayrı adlandırıyor:**
  `decayFactor(m,t) = exp(−λ(c)·Δt)` — saf tazelik ekseni `(0,1]`; ve
  `Salience(m,t) = RetentionPriority(m) × decayFactor(m,t)` — retrieval-sıralama bileşiği.
  §3a "İki dik eksen" (satır 235-245): ortogonalite **stale-tespitinin bileşiği bölmesiyle**
  operasyonel korunuyor.

- **Kod (satır 74-80)** ayrımı gerçekten yansıtıyor: yerel değişken `double decayFactor =
  Math.Exp(-rate * ageDays); // saf tazelik ekseni (0,1]` **var**, ve `return
  record.RetentionPriority * decayFactor; // bileşik: önem × tazelik`. Yorumlar (satır 65-72)
  teoriyle tutarlı; `decayFactor` adı teoriyle birebir aynı.

- **`FindStale`'in saf `decayFactor`'ı izole ettiği elle doğrulandı (satır 96):**
  `Salience(r,...) / r.RetentionPriority < staleThreshold`. Cebir:
  `(RetentionPriority × decayFactor) / RetentionPriority = decayFactor` — **doğru**, retention
  önemi bölünerek sadeleşir, yargı saf tazelik eksenine iner. Stale-eşiği `θ` bileşik
  `Salience`'ta değil saf `decayFactor`'da tanımlı (teori satır 224 ile tutarlı).

- **Bölme-sıfır riski GUARD'lı (satır 96):** `r.RetentionPriority > 0 && ...` koşulu var.
  `RetentionPriority = DecisionCapital.Value(LM, conf) = LM × conf`; tüm test kayıtları
  LM>0 ∧ conf∈(0,1] taşıdığından RP>0 — guard hiçbir test kaydını dışlamaz **ve** sıfıra
  bölme imkânsız. (İnce not, non-blocking: RP=0 olan bir kayıt — LM=0 — FindStale'den
  tümüyle **dışlanır**, yani hiç stale-bayraklanamaz; ama böyle bir kayıt zaten Salience=0
  ile retrieval'da görünmezdir, dolayısıyla davranış savunulabilir.)

**Artık-kaçak (non-blocking, N2):** Teori+kod artık `Salience`=bileşik, `decayFactor`=saf
üzerinde anlaşıyor; **ama glossary v0.2.4** aynı saf niceliği **"Salience Decay"** adıyla
tanımlıyor (satır 92-96: *"saf tazelik faktörünün (`decayFactor = exp(−λΔt)`)..."*). Yani
üç ad iki kavramı dolaşıyor: glossary **"Salience Decay"** = saf `decayFactor`; kod
**`Salience`** metodu = bileşik; teori **`decayFactor`** = saf. Çelişki **değil** (Salience
Decay = salience'ın sönümü = faktörün kendisi, savunulabilir), ama "Salience Decay" (saf) ile
"Salience" (bileşik) tek kelime arayla zıt kavramlara işaret ediyor — dikkatsiz okur
karıştırabilir. Glossary "Salience Decay" girdisi zaten bu bileşik ayrımını **açıkça** not
ediyor (satır 94-96), o yüzden bloke etmez; ama bir gün "Freshness Factor / decayFactor" gibi
karışmaz bir ad tercih edilebilir.

## D3 — γ≥1 gerekçesiz kısıtı: KAPANDI ✓
SKR-040/D3, `γ≥1`'in (a) türetilmemiş, (b) kodda zorlanmayan (guard yalnızca γ>0), (c) elde
tek anchor-veriyle (pusula) çelişen bir beyan olduğunu bulmuştu. v0.3.1:

- **Teori (satır 198):** `λ(c) = λ_base·(1−c)^γ , γ > 0 , λ_base > 0` — `γ≥1` **kaldırıldı**.
  Satır 216: *"Tek kısıt `γ > 0`'dır... `γ`'ya alt taban (ör. `γ≥1`) dayatan yapısal bir
  gerekçe **yoktur**."* Beyan artık kodla hizalı.
- **Kod (DecayFunction.Rate, satır 120-121):** `if (gamma <= 0) throw... "γ pozitif olmalı
  (ENS-2003 v0.3.1 §3a: tek kısıt γ>0)"`. Beyan/gerekçe/kod **üçü hizalı**.
- **Pusula fit-imkânsızlığı §Failure'a dürüstçe işlendi mi? EVET — hatta güçlüce** (satır
  316-322): *"pusula'nın üç çapa-noktası **hiçbir *tek* γ ile eşzamanlı fit edilemez**:
  uç-uca oran (180/30=6) γ≈0.72 ister, orta-alt oran (90/30=3) γ≈2.04 ister — ikisi tutarsız,
  üstelik endpoint-γ bir zamanlar beyan edilen γ≥1 tabanının altında."* Gizlenmemiş,
  yumuşatılmamış; savunmanın "pusula-fit'e değil yalnızca yapısal argümana" dayandığı açıkça
  yazılmış (SKR-040 talep-4 tam karşılanmış).

**Anchor oranları elle yeniden doğrulandı (bağımsız):** t_stale ∝ 1/λ(c) ∝ (1−c)^(−γ);
oran yalnızca γ'ya bağlı (λ_base ölçek, iptal olur).
- Uç-uca: (0.60/0.05)^γ = 12^γ = 6 → γ = ln6/ln12 = 1.7918/2.4849 = **0.7211** ✓
- Orta-alt: (0.60/0.35)^γ = 1.7143^γ = 3 → γ = ln3/ln1.7143 = 1.0986/0.5390 = **2.038** ✓
- 0.72 ≠ 2.04 → tek γ ile fit **imkânsız**; endpoint-γ (0.72) < eski γ≥1 tabanı. Doc **doğru**.

## Yanlışlanabilirlik / Novelty (SKR-040'tan devralınan — yeniden sınandı)
SKR-040'ın bulguları bu turda **geçerliliğini koruyor** ve düzeltmeler onları zayıflatmadı:
formül limitleri (λ(1)=0, λ(0)=λ_base, c'de monoton) hâlâ doğru; beş prior-art atıfı
(ECC, Hermes Curator, adaptive-decay-KG arXiv:2604.26970, TempValid ACL 2024 long.580,
Temporal RAG arXiv:2509.19376) SKR-040'ta bağımsız web-doğrulanmıştı — bu tur onları yeniden
sorgulamadı (kapsam: desenkron-kapanışı), atıf metinleri değişmedi. Novelty konumlaması
("yeni yasa değil, KG/RAG mühendisliğinin ENS invariant'larına sentezi") dokunulmadan duruyor.

## Testler (Soru 5) — statik doğrulandı, runtime KOŞULAMADI
**Kayıt altı: bu context'te Bash/dotnet yok; `dotnet test` çalıştırılamadı.** İddiaya körce
güvenmedim — bunun yerine 21 test-case'i koda karşı **statik olarak** doğruladım ve temsilî
sönüm değerlerini elle hesapladım:
- `Salience_decays_with_age...` (satır 65): rate=0.01·0.2=0.002, df=exp(−0.002·365)=0.482 <1 →
  Salience<RP ✓
- `Salience_zero_base_rate...` (73): rate=0 → df=1 → Salience=RP ✓
- `High_confidence...decays_slower` (184): df_trusted=exp(−0.2)=0.819 > df_shaky=exp(−3.2)=
  0.041 ✓
- `DecayFunction_*`: conf=1→rate=0 ✓; conf=0→rate=base ✓; monoton ✓; γ≤0 throws (guard) ✓;
  γ>1 kontrast: γ=2 için (0.8²/0.1²)=64 > γ=1 için (0.8/0.1)=8 ✓
- `FindStale_flags...` (219): conf=0.3, age=500 → rate=0.014, df=exp(−7)=0.0009 <0.5,
  RP=5·0.3=1.5>0 → bayraklanır **ve** AllRecords'ta kalır ✓
- `FindStale_excludes_fresh...` (229): conf=0.99, age=0 → df=1 >0.5 → dışlanır ✓
- range/throw testleri (81, 127, 133, 139, 242): guard'larla eşleşir ✓

**Logic-değişmedi iddiası tutarlı:** `Salience` = RetentionPriority × decayFactor bileşiği,
SKR-040'ın D2'de *zaten kodda bulduğu* şeydir (SKR-040 kodun bileşiği döndürdüğünü şikâyet
etmişti); v0.3.1 teoriyi buna hizaladı, kodu değil. Dolayısıyla testlerin kırılmaması
beklenendir. **Yine de: yeşil koşuyu owner/CI teyit etmeli** — statik analiz güçlü ama
runtime'ın yerini tutmaz; bunu fabrike etmedim.

## İç tutarlılık (Soru 4 — yeni kaçak taraması)
- **N1 (non-blocking):** `DecisionCapital.cs:8` — "ENS-2003 Company Memory henüz kodlanmadı"
  bayat/yanlış yorumu (D1-olgusunun komşu-dosya kopyası). Temizlenmeli.
- **N2 (non-blocking):** glossary "Salience Decay" (saf) vs kod `Salience` (bileşik) ad-yakınlığı.
- **N3 (non-blocking, SKR-040 talep-5 devri):** "memory assertion ≠ ENS-4010 Assertion node"
  homonim notu §Homonim'e **hâlâ eklenmedi** (owner v0.3.1 notunda bilinçle sonraki tura
  erteledi — satır 350). Homonim gerçek ama ratified'ı bloke etmez; kayıt-birimi Decision'dır.
- **SKR-040 talep-6 (Hermes konumlaması):** owner tarafından sonraki tura ertelendi;
  §Prior art (satır 95) hâlâ "kaynaklarda curation otonom olabilir" diyor — dürüstlüğü artıran
  ama bloke-etmeyen bir keskinleştirme borcu. Açık, non-blocking.
- Glossary v0.2.4 başlığı (satır 12) D2-ayrımına doğru atıfla güncellenmiş; beş terim temiz.

## Sahibine talepler
**Ratified'ı bloke eden hiçbir şey yok** — skeptic-kapısı geçildi. Aşağıdakiler **non-blocking
hijyen** (sıradaki dokunuşta):
1. **N1:** `DecisionCapital.cs:8` yorumundan "ENS-2003 Company Memory henüz kodlanmadı"yı
   düzelt (artık kodlu) — D1-temizliğini komşu dosyaya taşı.
2. **N2 (opsiyonel):** glossary "Salience Decay" için karışmaz bir ad (ör. "Freshness Factor")
   düşün, ya da "≠ kod `Salience` (bileşik)" satırını daha görünür yap.
3. **N3:** §Homonim/alias'a "memory assertion ≠ ENS-4010 Assertion node (kayıt-birimi
   Decision'dır)" satırını ekle (SKR-040 talep-5 devri).
4. **Hermes:** "curation otonom olabilir" → "Hermes'te otonom prune/archive varsayılandır;
   ENS bilinçle yalnızca-sinyale indirir" (delta daha güçlü görünür).
5. **Runtime teyit:** `dotnet test` yeşil koşusunu CI/owner kayda geçirsin (bu incelemede
   statik doğrulandı, canlı çalıştırılamadı).

---

*Üç desenkron ucuzdu ve gerçekten kapandı: teori/kod/glossary artık `decayFactor`(saf) /
`Salience`(bileşik) üzerinde anlaşıyor, `γ>0` üç yerde de tutarlı, pusula fit-imkânsızlığı
dürüstçe yazılı, D1-cümlesi olgusal doğru. Çekirdek zaten SKR-040'ta sağ çıkmıştı; bu tur
yarayı kapattı. Kalan üç artık-kaçak (bayat komşu-yorum, ad-yakınlığı, ertelenen homonim)
non-blocking hijyendir — ratified hak edildi. `canon` ayrı bir governance edimidir; bu
inceleme yalnızca skeptic-kapısının geçildiğini işaretler.*
