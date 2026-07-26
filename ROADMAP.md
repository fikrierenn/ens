# ENS Roadmap — Açık İşler Kaydı

**Yetki:** Bu dosya konuşma-hafızası değil, **kalıcı proje kaydıdır** — LAW-ORG-MEMORY'nin
("unutulan kararlar tekrarlanan hatalara dönüşür") kendi projeye uygulanışı. Her oturum başında
buradan devam edilir; hiçbir açık iş yalnızca sohbet geçmişinde yaşamaz.

**Güncelleme kuralı:** bir iş kapanınca ✅ işaretlenir + kapatan artifact/SKR referansı eklenir;
asla satır silinmez (audit, EC-001 ile tutarlı).

---

## 🔴 AÇIK — ENS-2003 v0.4.0 (D-5 çift-sayım düzeltmesi): İKİ doğrulama borcu (2026-07-26)

Bağımsız TRACE sadakat denetimi (`7000-reference-implementation/AUDIT-WAVE2-FIDELITY.md`, bulgu
**D-5**) ENS-2003 §3a'da **gerçek bir teori hatası** buldu: attribution confidence `c` hem retention
ağırlığında (`|L|·c`) hem sönüm hızında (`λ_base·(1−c)^γ`) sayılıyordu — **çift-sayım**, üstelik tam
da §3'ün karşı-survivorship amacını tersine çeviren yönde (düşük-atıflı başarısızlık dersi hem geri
plana itiliyor hem hızlı sönüyordu; ENS-2004 §3'e göre bu **tipik** vaka, istisna değil).
**SKR-040 ve SKR-041'in ikisi de bunu kaçırdı** — SKR-040/D2 yalnızca *isim* sürüklenmesini kapatmıştı.

**Yapılan (ens-philosopher, 2026-07-26):**
- `ENS-2003` **v0.3.1 → v0.4.0 (BREAKING)**, `status: ratified → review`. Üç nicelik ayrıştırıldı:
  `RetentionPriority = |L|` (c'siz) · `value = |L|·c` (= ENS-3023 §Model 1, yeni kavram değil) ·
  `decayFactor = exp(−λ_π·Δt)` (c'siz; `λ_π = ln2/τ_π`, Purpose-tipinin context yarı-ömrü).
  `γ`/`λ_base` **kaldırıldı**. Patoloji "iddiayı yumuşatarak" değil **yapısal invariant'la** kapatıldı:
  **karşı-survivorship tabanı** (kesme invariant'ı) + Curator'ın ikinci sinyali (`weakly-attributed`).
  "İki dik eksen" iddiası hem düzeltildi hem **küçültüldü** (argüman-ayrıklığı; istatistiksel
  ortogonallik DEĞİL). Prior art'ta bir bağlama (**confidence→TTL düzeltmesi**) açıkça **geri çekildi**.
- `ENS-2004` **v0.3.2 → v0.3.3**, `status: ratified → review` — §Implications'ın kendi içinde
  çelişen "retention = |L|×c" satırı düzeltildi (o formül *retrieval ağırlığıdır*).
- `ENS-4000`: `Salience Decay`/`Stale Flag` yeniden yazıldı; +4 M1 terim (`Context Half-Life`,
  `Retention Priority`, `Counter-Survivorship Floor`, `Weakly-Attributed Flag`).
- Faz-4: `CompanyMemory.cs` yeniden yazıldı (+`RetrieveTop`, `CounterSurvivorshipFloor`,
  `FindWeaklyAttributed`, `MemoryRecord.CapitalValue`; `DecayFunction` → `RateFromHalfLife`/
  `HalfLifeDays`/`DaysUntilStale`). `CompanyMemoryTests.cs` yeniden yazıldı; `AdversarialWave_MemoryTests.cs`
  ve `AdversarialAuditTests.cs` (G2/G7/G8/G9, A1-A5, D1-D4, E1-E6) uyarlandı; Demo + README + Guard +
  DecisionCapital yorumları hizalandı.

**BORÇ 1 — bağımsız skeptic turu (BLOCKING, öz-onay yok / G2/G3).** v0.4.0 `survives` DEĞİLDİR;
hiçbir skeptic turu görmemiştir. Skeptic'e özel sorular: (a) `τ_π` borcu, kaldırdığı `γ` borcundan
gerçekten daha mı iyi — yoksa "mevcut bir alandan (c) hiç olmayan bir alana (τ_π)" kaçış mı?
(b) karşı-survivorship tabanının memory-poisoning'i *amplifiye etmesi* kabul edilebilir bir takas mı?
(c) ENS-2004 v0.3.3 değişikliği gerçekten additive mi, yoksa §5(iii) kalibrasyon argümanının bir
öncülünü sessizce mi değiştiriyor?

**BORÇ 2 — `dotnet build` / `dotnet test` HİÇ ÇALIŞTIRILMADI (BLOCKING).** v0.4.0'ı yazan context'te
**shell aracı yoktu** (aynı sınır SKR-041 ve AUDIT-WAVE2'de de beyan edildi). Kod ve testler yalnızca
**statik** olarak hizalandı; **hiçbir test çıktısı iddia edilmemiştir.** Bu breaking API değişikliği
(`baseDecayRate`+`gamma` → `contextDecayRate`; `RetentionPriority` semantiği; `DecayFunction` imzaları)
5 dosyaya dokunur — derleme hatası riski **gerçektir** ve CI/owner tarafından ilk fırsatta koşulmalıdır.
Bilinen ayrık durum: `AUDIT_DEFECT_E6` v0.4.0 ÖNCESİNDE de fail ediyordu (başka bir ajanın test-kurgu
kusuru); bu turda **dokunulmadı**, yalnızca kaldırılan `gamma:` argümanı mekanik olarak çıkarıldı.

**Kapsam dışı bırakılanlar (kaydedildi, yapılmadı):** AUDIT-WAVE2/T-1 (ENS-2004 §Failure'daki
"Faz-4'te kodlanmadı" olgusal yanlışı — D-1 sınıfı, ENS-2003'te bir kez düzeltilmişti), T-2
(ENS-2004 künyesinde `evidence:` alanı yok — şema ihlali), D-4 (CompanyMemory bir *graph* değil),
O-4'ün kalanı (Decision DNA / sıkıştırma kodlanmadı).

## 🆕 YENİ İŞ — Computational/Organizational Ontoloji Katmanları (EXPERIMENTAL)
Kernel-vizyon tartışmasından çıktı. 3 senaryoda (ERP/bilişsel/IoT) stres testi + bir **Design
Review** turu (6 hedef, 4 gerçek çatlak bulunup düzeltildi) + gerçek prior-art araştırması
(ECS, Greg Young Event Sourcing, TOVE Enterprise Ontology, LOM-action arXiv:2604.08603) ile
ciddi şekilde sağlamlaştırıldı. **Yazıldı, henüz Ontology Validation'dan geçmedi:**
- `ENS-4001` v0.6 — `ens-comp:` namespace (yeni), Identity/Event/Capability, `projects`
  Semantic Connector (rezerve slotu doldurdu), Sandbox=EventStore+BranchManager+ProjectionEngine
  (LOM-action derinleştirildi), **Simulation = projects(SimulationDefinition) örneği** (kullanıcı
  bulgusu — Simulation artık mimari-merkez değil, genel mekanizmanın bir örneği), Capability→
  Implementation→Skill ayrımı (LOM-action'ın Skill=Capability özdeşliğine düzeltme), Principal-
  as-role, Computational Closure axiom'u, "Illusive Accuracy"→çok-eksenli değerlendirme (Faz 4/5
  ileri-işareti), tam prior-art tablosu (ENS'in LOM-action'a göre bir katman daha derin gitme
  iddiası — Computational Ontology, Enterprise Ontology'nin altında).
- `ENS-4010` v0.3 — "Organizational Ontology" olarak yeniden konumlandı (Node Registry
  değişmedi), Policy artık TOVE'nin Empowerment tanımıyla kesinleşti.
- **Açık, dürüstçe bırakıldı:** Identity'nin primitifliği (Design Review'da saldırıya uğradı,
  savunulamadı tam); Decision⊂Event (Model B, Anayasa amendment gerekmiyor); `ens-comp:`
  namespace'i henüz 7-parça şablona oturmadı. **Sıradaki adım: bağımsız Ontology Validation.**

## ✅ Tamamlanan (bu tur)
- **3. bağımsız teyit turu** — SKR-029 (ADR-0001) + SKR-030 (ENS-4020) yazıldı. **İkisi de
  survives.** ADR-0001 Bulgu A/B'nin v0.3 düzeltmeleri (orphan→realizes, sınıflandırma→Framing/
  proof-trace) zorlama değil, meşru ENS-2001 indirgemeleri olarak teyit edildi. ENS-4020 C.a/C.b
  düzeltmeleri teyit → **M2** (G4). SKR-030 ayrıca ENS-4010'da 2. bir profil↔registry çelişkisi
  buldu (Resource/`required_by`) — ENS-4010 borcu, yukarıda blocking-5.
- **Bağımsız skeptic re-validation** — SKR-026 (ADR-0001), SKR-027 (ADR-0002) yazıldı. G2/G3
  şüphesi doğrulandı: inline SKR-024/025 "kötü değil ama eksikti" — bağımsız göz zincir-dışına
  bakıp gerçek yapısal sorunlar buldu.
- **Bulgu D (döngüsel bağımlılık) kapandı** — `ENS-4020.depends_on`'dan `ADR-0002` çıkarıldı
  (v0.2). Yön artık tek: ADR-0002 → ENS-4020 (Madde XII, Linter, P8 uyumlu).

## Kernel hattı (North Star — AI-native Enterprise OS, aktif çalışma)
| # | İş | Durum |
|---|-----|-------|
| K1 | ADR-0001 (Cognitive Kernel + Bounded Autonomy) | **✅ Accepted** (v0.3, 3. tur survives SKR-029, `ens-ceo` hiza CEO-0001) — bu satır uzun süre eski/çelişkili kaldı, düzeltildi 2026-07-24 |
| K2 | ADR-0002 (operax → Operations Capability Pack) | **✅ Accepted** (v0.3.1, 2026-07-24). SKR-025/027 wounded → v0.3 düzeltme (Bulgu 1: operax dosya-denetimi RFQ=0/M04=yalnızca fiyat-spec, "≥4→3 lifecycle"; Bulgu 2: delta dili; Bulgu 3: Confidence-OQ6) → **SKR-037 survives** (bağımsız operax-denetim teyidi) → **K4 CEO-0003 Accepted** (operax'ın aktif geliştirmesinin durması K1'in bugünkü-koda-dayanan zeminini etkilemiyor). `status: accepted` fiilen çevrildi. 2 bloke-etmeyen gözlem açık (traceability asimetrisi, price-variance lifecycle statüsü). |
| K3 | ENS-4020 (Enterprise Ontology — operax `ens-ent:`) | **v0.3 — M2** (SKR-030 survives): C.a/C.b teyit; iki bağımsız validator (SKR-028+SKR-030) → G4. `status: review` (ratified ayrı governance edimi). Faz-4: OF1/OF2 |
| K4 | **ens-ceo hiza incelemesi** | ADR-0001 için ✅ kapandı (CEO-0001). ADR-0002 için ✅ kapandı (CEO-0003, 2026-07-24) — operax'ın aktif geliştirmesinin durdurulması (kullanıcı kararı, ENS öncelikli) D1'in bugünkü kod-doğrulanmış K1 zeminini etkilemiyor; F3/F4/OQ1/OQ2/OQ6 kapanışı zaten Faz-4 borcu, artık ENS-operax entegrasyon fazına bağlı zamanlama netliğiyle. `status` geçişi (draft→Accepted) owner'a bırakıldı |
| K5 | Sonraki değer yakalama | ~~brain→Memory runtime~~ ❌ **2026-07-24 denetlendi, DEĞERSİZ çıktı** (Explore agent: `grep salience\|decay\|retention\|purpose-tipi\|attribution-confidence` → brain'de **sıfır isabet**; kişisel Obsidian vault, kod/retrieval-motoru yok, ENS-2003'ün hiçbir mekanizmasıyla örtüşmüyor — operax M04/RFQ dersiyle aynı disiplinle reddedildi, bkz. blocking-durumu altı). **🆕 Yeni aday: `D:\Dev\pusula\sema/`** (kullanıcı işaret etti) — gerçek, çalışan bir semantik katman: `confidence`+`evidence` alanları (P6), decay-ama-asla-silme + confidence-katmanlı TTL (ENS-2003 §3 ile ruhsal/yapısal yakın eşleşme), `sema-ogren` ile atomic-öğrenme tetiği (ENS-2004), curator-check uzlaştırma. Envanter tamam, henüz ADR yazılmadı. reporthub→audit/RBAC/plugin, AtlasOPS→MCP, DikkatIQ→Attention(P5) hâlâ araştırılmadı. **🚧 2026-07-24 — ens-researcher doğrulanmış literatür turu yaptı (ECC/affaan-m, Hermes Curator/Nous, adaptive-decay-KG arXiv:2604.26970, TempValid ACL 2024, Temporal RAG arXiv:2509.19376; pusula sema=ECC+Hermes uyarlaması; kritik bulgu: pusula'nın 180/90/30 sabit-kademe TTL'i keyfi/kalibrasyonsuz, literatür confidence'tan *sürekli* türeyen fonksiyon istiyor). Bunun üzerine ens-philosopher `ENS-2003 v0.2.0→v0.3.0` teorik uzantısını yazdı (`status: ratified→review`): §3a confidence-conditioned salience decay `λ(c)=λ_base·(1−c)^γ` (Purpose-tipi başına kalibre; pusula'nın sabit-kademesini düzeltir), `asserted_at`(değişmez)/`last_verified`(teyit) audit-çaparı, stale=bayrak-değil-aksiyon, §3b Memory Curator (yalnızca inceleme sinyali, otonom silme yok), §Prior art'a 5 gerçek atıf, +2 failure condition (formül-kalibrasyonu E1'de açık + Curator aşırı-güveni). Sözlük ENS-4000 v0.2.3'e 5 terim M1 girdi. **🛡️ 2026-07-24 — SKR-040 (bağımsız tur, taze context, G2/G3) yazıldı → `wounded`.** ENS-2003 `status: review` KALIR, `skeptic_review: [SKR-008, SKR-040]`. **Özde sağlam:** beş prior-art atıfı (ECC/affaan-m, Hermes Curator/Nous, adaptive-decay-KG arXiv:2604.26970, TempValid ACL 2024 long.580, Temporal RAG arXiv:2509.19376) bağımsız web-doğrulandı — **5/5 gerçek, uydurma yok**; formül limitleri (λ(1)=0, λ(0)=λ_base, monoton) elle doğrulandı; γ=1 anchor oranları (12:1.7:1) elle teyit; novelty dürüstçe (ihtiyatla) konumlanmış; audit (`asserted_at`/`last_verified`, never-delete) ve Curator-yalnızca-sinyal hem teoride hem kodda invariant-sadık (kodda Delete yok). **Yara = 3 teori-kod desenkronu (blocking, ucuz):** (D1) §Failure-condition "7000 formülü henüz implemente etmemiştir" **olgusal yanlış** — `DecayFunction.Rate`/`Salience`/`FindStale`/`Verify` kodlandı, E1 gerekçesi "kodlanmadı" değil "kalibre edilmedi" olmalı; (D2) kodun `Salience`'ı §3a'nın saf `exp(−λΔt)`'i yerine `RetentionPriority × exp(−λΔt)` döndürüyor — §3'ün "iki dik ekseni karıştırma" uyarısıyla çelişir (isim/tanım drift; ortogonalite FindStale'de operasyonel korunuyor ama isim yanlış); (D3) `γ≥1` gerekçesiz beyan + kod guard'ı yalnızca `γ>0` zorluyor + elde tek anchor-veri (pusula) γ<1 istiyor. **Bağımsız güçlendirme:** pusula'nın 3 çapa-noktası (0.95→180g/0.65→90g/0.40→30g) **hiçbir tek γ ile fit edilemez** (endpoint γ≈0.72 vs orta γ≈2.04, üstelik endpoint-γ γ≥1 tabanının altında) → savunma pusula-fit'e değil yalnızca yapısal argümana dayanmalı. **Sıradaki adım: ens-philosopher D1/D2/D3'e yanıt (ENS-2003 v0.3.1 + `CompanyMemory.cs` hizalama) → bağımsız 2. skeptic turu → survives ile `ratified`. ADR (sema→Memory runtime) hâlâ yazılmadı.** Bkz. `2000-theory/reviews/SKR-040-company-memory-v03-decay-layer.md`. **✍️ 2026-07-24 — ens-philosopher SKR-040'a yanıt verdi, ENS-2003 v0.3.0→v0.3.1 (`status: review` KALIR, `skeptic_review: [SKR-008, SKR-040]`).** Üç blocking desenkron kapatıldı: **D1** — §Failure'ın "7000 formülü henüz implemente etmemiştir" olgusal-yanlış cümlesi düzeltildi (kod `DecayFunction.Rate`/`Salience`/`FindStale`/`Verify`'ı içeriyor); E1 gerekçesi "kodlanmadı"→"**kalibre edilmedi**" (γ/λ_base ampirik değil). **D2** — §3a'da saf tazelik `decayFactor = exp(−λΔt)` ile bileşik retrieval skoru `Salience = RetentionPriority × decayFactor` açıkça ayrıştırıldı; §3'ün "iki dik eksen" uyarısına sadakat, ortogonalite `FindStale`'in böldüğü saf `decayFactor`'da operasyonel korunuyor; **kod değişmedi, yalnızca yorumlar** (testler kırılmadı — Salience aynı değeri döndürüyor). **D3** — gerekçesiz `γ≥1` kaldırıldı, `γ>0`'a (kod guard'ıyla tutarlı) indirildi; pusula'nın 3 çapa-noktasının hiçbir tek γ ile fit edilemediği (endpoint γ≈0.72 vs orta γ≈2.04) §Failure'a eklendi, savunma yalnızca yapısal argümana dayanır. Değişen dosyalar: `ENS-2003-company-memory.md` (v0.3.1, §3a/§Failure/§SKR-040-yanıt tablosu), `CompanyMemory.cs` (yalnızca yorum+bir exception-mesaj string'i). **⚠️ ÖZ-ONAY YOK (G2/G3): `survives` DEĞİL — bağımsız 2. `ens-skeptic` turunu bekliyor.** **🛡️ 2026-07-24 — SKR-041 (bağımsız 2. tur, taze context, G2/G3) yazıldı → `survives`.** ENS-2003 `status: review → ratified`, `skeptic_review: [SKR-008, SKR-040, SKR-041]`. Üç blocking desenkron **gerçekten kapandı** (bağımsız doğrulandı): D1 — §Failure "implemente etmiştir + kalibre değil" cümlesi kodla (DecayFunction.Rate/Salience/FindStale/Verify mevcut) uyumlu; D2 — teori/kod `decayFactor`(saf `(0,1]`)/`Salience`(bileşik = RetentionPriority×decayFactor) ayrımını yansıtıyor, `FindStale` cebirsel izolasyonu ((RP×df)/RP=df) + `RetentionPriority>0` bölme-guard'ı doğrulandı; D3 — `γ>0` teori/gerekçe/kod-guard üçünde hizalı, pusula'nın 3 çapa-noktası fit-imkânsızlığı (endpoint γ≈0.72 vs orta γ≈2.04, elle teyit) §Failure'a dürüstçe işlenmiş. 21 test statik-tutarlı doğrulandı (**kayıt altı: Bash/dotnet skeptic-context'inde yoktu → `dotnet test` canlı çalıştırılamadı; yeşil koşu CI/owner teyidi bekler — fabrike edilmedi**). **`canon: false` KALIR** — Külliyat-girişi ayrı governance edimi; SKR-041 yalnızca skeptic-kapısını işaretler. 3 non-blocking artık-kaçak açık: **N1** `DecisionCapital.cs:8` bayat "Company Memory henüz kodlanmadı" yorumu (D1-olgusunun komşu-dosya kopyası); **N2** glossary "Salience Decay"(saf) vs kod `Salience`(bileşik) ad-yakınlığı; **N3** "memory assertion ≠ ENS-4010 Assertion node" homonim notu hâlâ eklenmedi. Bkz. `2000-theory/reviews/SKR-041-company-memory-v031-desync-closure.md`. **Sıradaki adım: ADR (sema→Memory runtime) yazımı + non-blocking N1/N2/N3 hijyeni.** **🆕 2026-07-24 — K5 ikinci kolu: Hermes Agent self-evolution (github.com/NousResearch/hermes-agent-self-evolution; GEPA+DSPy) → ENS-2004 (Learning Theory).** Doğrulanmış araştırma (GEPA=Genetic-Pareto reflective prompt evolution arXiv:2507.19457, DSPy Stanford arXiv:2310.03714, Hermes constraint-gate + insan-PR-onay/otonom-commit-yok) üzerine ens-philosopher `ENS-2004 v0.2.0→v0.3.0` **additive** uzantı yazdı (`status: ratified→review`): **§4a Reflective double-loop** — geçmiş commit-edilmiş kararların proof-trace'i (ENS-4025 L8) oku → sistematik öngörü hatasının *"neden"*'ini analiz et → Assumptions/relevance-model/attribution-seviyesi için **hedefli iyileştirme önerisi** üret → **öneri asla otomatik uygulanmaz, insan onayı (P7) gerekir**. §Prior art'a 3 gerçek atıf (GEPA/DSPy/Hermes), Historical tabloya 1 satır, §Failure'a +1 condition (öneri-üretim mekanizması operasyonelleşmedi — ENS-4025 L8'e bağımlı, Faz-4'te kodlanmadı, E1), Laws'ta "yeni yasa değil — §4 double-loop + P7 + §3 attribution-merdiveninin sentezi" netleştirmesi, `depends_on: +ENS-4025`. §Failure #1 (beklenen-değer elicitasyonu) ile bağlandı: trace-analiz sistematik-sapmış per-Alternative EV tahminlerini tespit edip iyileştirme *önerebilir* (uygulamaz). Sözlük ENS-4000 v0.2.4→v0.2.5: **Reflective Double-Loop** terimi M1. **⚠️ ÖZ-ONAY YOK (G2/G3): `survives` DEĞİL — bağımsız `ens-skeptic` turunu bekliyor.** **🛡️ 2026-07-24 — SKR-042 (bağımsız tur, taze context, G2/G3) yazıldı → `wounded`.** ENS-2004 `status: review` KALIR, `skeptic_review: SKR-042`. **Özde sağlam:** üç prior-art atıfı (GEPA arXiv:2507.19457 "reflective prompt evolution"+genetic-pareto+RL-geçme, DSPy arXiv:2310.03714 "self-improving pipelines", Hermes repo constraint-gate + "never direct commit") **bağımsız web-doğrulandı — 3/3 gerçek, birebir doğru konumlanmış**; §4a'nın P7-gate dili (en kritik kontrol) kendi içinde **temiz** (4 yerde tutarlı "asla uygulanmaz", Constitution P7 ile birebir); "yeni yasa değil" savunulabilir; glossary (ENS-4000 v0.2.5) tutarlı; §4↔§4a otonomi genişlemesi **yok** (3. hedef=§3'ten, ilan edilmiş). **Yara = 3 giderilebilir kusur (blocking, metin-içi):** (T1) §4a "**per-Alternative** EV sapması tespit et" iddiası ENS'in kendi attribution çekirdeğini aşıyor — seçilmeyen Alternative'in Actual'ı asla gözlenmez (Y(a′) counterfactual); sistematik-sapma yalnızca *seçilen* Alternative EV'si için ampirik kontrol edilebilir; ENS-2001 §Failure'ın unchosen-EV kırılganlığı miras alınıyor ama kabul edilmiyor → "chosen-Alternative"a daralt ya da unchosen-sinyalin outcome-dışı olduğunu söyle; (T2) **P5 (Attention kıt) hiç devrede değil** (ne principles ne failure) — §4a öneri-üreticisi + zorunlu-insan-onay = öneri-yorgunluğu riski; hacimli öneri → ya P7 rubber-stamp (P7 fiilen ölür) ya ignore (mekanizma ölür); Hermes'in makine-gate'i insan-yükünü kısar, ENS'in saf-insan-gate'i kısmaz → P5 ekle + öneri-hacmi failure condition ekle + hacmi neyle sınırladığını yanıtla; (T3) §1 Definition ("learning ... ilgililik/varsayım modeline yazılan güncelleme") ↔ §4a ("bu güncellemeler insan-onaylı öneri") uzlaştırılmamış — sonucun kayda geçmesi (otomatik) ≠ double-loop model revizyonu (P7-gate'li) ayrımı bir cümleyle netleşmeli. **Sıradaki adım: ens-philosopher T1/T2/T3'e yanıt (ENS-2004 v0.3.1) → bağımsız 2. skeptic turu → survives ile `ratified`.** Bkz. `2000-theory/reviews/SKR-042-learning-theory-v03-reflective-double-loop.md`. **✍️ 2026-07-24 — ens-philosopher SKR-042'ye yanıt verdi, ENS-2004 v0.3.0→v0.3.1 (`status: review` KALIR, `skeptic_review: SKR-042`, `principles: +P5`).** Üç blocking talep kapatıldı: **T1** — §4a'nın EV-sapma iddiası attribution çekirdeğine hizalandı: "per-Alternative" kaldırıldı, iddia **yalnızca commit-edilen (seçilen) Alternative'in** EV kalibrasyonuna (learning_signal, §1) daraltıldı; seçilmeyen `a′` için `Y(a′)`'nin asla gözlenmediği (§2 counterfactual) ve ENS-2001 §Failure'ın unchosen-EV kırılganlığının miras alındığı açıkça kabul edildi. **T2** — `principles`'a **P5** eklendi; §4a'ya "Dikkat kıt kaynaktır (P5) — öneri hacminin sınırlanması" paragrafı + §Failure'a "Öneri-yorgunluğu" koşulu yazıldı: hacimli öneri → ya P7 rubber-stamp (P7 fiilen ölür) ya ignore (mekanizma ölür); Hermes'in makine-gate'i insan-yükünü kısar, ENS'in saf-insan-gate'i yalnızca öneri *biçimini* kısıtlar (hacmini değil) — dürüstçe kabul; önceliklendirmenin ENS-3022 (Decision Gravity)/eşik/batch'e bağlanabileceği ama **henüz operasyonelleşmediği** (E1) not düşüldü. **T3 (kaçak taban §1 Definition'daydı, §4a'da değil)** — bulunan cümle: §1'in "…Company Memory'ye **ve ilgililik/varsayım modeline yazılan bir güncellemeye** dönüştürülmesidir" edilgen ifadesi (otomatik-güncelleme ima ediyordu, P7'nin "önerir-emretmez"iyle gerilimde); "…kaydedilmesi ve ilgililik/varsayım modeli için bir güncelleme **_önerisine_** dönüştürülmesi" olarak düzeltildi + yeni "İki farklı yazım (P7 ayrımı)" paragrafı: (a) sonucun kayda geçmesi=otomatik olgu-kaydı ≠ (b) model revizyonu=P7-gate'li öneri. Değişen dosyalar: `ENS-2004-learning-theory.md` (v0.3.1, künye/§1/§4a/§Failure/§SKR-042-yanıt tablosu). **⚠️ ÖZ-ONAY YOK (G2/G3): `survives` DEĞİL — bağımsız 2. `ens-skeptic` turunu bekliyor.** **🛡️ 2026-07-24 — SKR-043 (bağımsız 2. tur, taze context, G2/G3) yazıldı → `wounded`.** ENS-2004 `status: review` KALIR, `skeptic_review: [SKR-042, SKR-043]`. **T1 ve T2 TAM kapandı (bağımsız doğrulandı):** T1 — §4a artık yalnızca commit-edilen Alternative'in EV kalibrasyonunu hedefliyor, "per-Alternative" temizlendi, seçilmeyen `a′`'nın `Y(a′)`-gözlenemezliği (§2) kabul edildi; ENS-2001 §Failure atfı (satır 241-243 "kaydedilmez ya da kaba") **gerçek — uydurma yok**; §5(ii) seçim rasyonalitesinin outcome-bağımsız per-Alt EV'si ile çelişmiyor. T2 — `principles: +P5` künyede; öneri-yorgunluğu failure condition rubber-stamp/ignore ikilemini + Hermes-gate'in insan-yükü kıstığı-ENS-gate'in yalnızca biçim kısıtladığı itirafını içeriyor; ENS-3022 (Decision Gravity) "henüz operasyonelleşmemiştir" notu dürüst. Glossary (ENS-4000 v0.2.5) EV-iddiasını hiç içermediğinden T1-daraltmasından etkilenmedi, tutarlı. **Yara = T3 yalnızca KISMEN kapandı (blocking, tek cümle):** §Definition (satır 60) ve yeni "İki farklı yazım (P7 ayrımı)" paragrafı doğru düzeltildi, **ama aynı belgenin §Theoretical model §1'i (satır 128) hâlâ eski otomatik-yazma kipini taşıyor** — "Learning bu farkı memory'ye **ve ilgililik/varsayım modeline yazar**" — bu, üç bölüm yukarıdaki "(b) model revizyonu **asla otomatik yazılmaz; insan-onaylı öneridir**" ifadesiyle **doğrudan çelişiyor**; T3'ün kapatmayı amaçladığı P7-çelişkisi biçimsel model bölümünde canlı (owner prose'da kapatıp modeldeki ikizini bıraktı — eksik tarama). İkincil zayıf eko: §Implications satır 239 "Context relevance L1 ile **beslenir**" (relevance-model revizyonunu otomatik ima ediyor; §Relationships satır 244 ise doğru "önerir/P7" yapmış). **Sıradaki adım: ens-philosopher §1 satır 128 (+§Implications 239) T3-hizası (tek cümle: fark memory'ye otomatik yazılır; model revizyonu §4a P7-önerisidir) → bağımsız 3. skeptic turu → survives ile `ratified`.** Bkz. `2000-theory/reviews/SKR-043-learning-theory-v031-t3-residual-leak.md`. **✍️ 2026-07-24 — ens-philosopher SKR-043'e yanıt verdi, ENS-2004 v0.3.1→v0.3.2 (`status: review` KALIR).** T3-artığı iki parçada kapatıldı: (a) §Theoretical model §1 (satır 128-130) "…memory'ye **kaydeder** (olgu-kaydı, otomatik) ve … model için **güncelleme önerisine** dönüştürür (P7-gate'li — model asla otomatik yazılmaz)"; (b) §Implications (satır 241-242) "L1 ile beslenir" → "L1 attribution sinyalini **veri olarak** kullanır (relevance bir hesaplamadır, P7-gate'li model-güncellemesi değil)". **⚠️ ÖZ-ONAY YOK (G2/G3) — bağımsız 3. skeptic turu bekledi.** **🛡️ 2026-07-24 — SKR-044 (bağımsız 3. tur, taze context, G2/G3) yazıldı → `survives`.** ENS-2004 `status: review → ratified`, `skeptic_review: [SKR-042, SKR-043, SKR-044]`. **T3-artığının iki parçası da TAM kapandı** (bağımsız doğrulandı): §1'de "otomatik" yalnızca olgu-kaydına (Actual−Expected) scope'lanıyor, model-güncellemesine sızmıyor; §Implications relevance-hesabı(veri-in, otomatik) ile relevance-model-revizyonu(P7-öneri) ayrımını keskinleştiriyor. **Kritik:** SKR-043'ün "bir yerde kapatılıp başka yerde unutulan çelişki" deseninin **3. kez tekrar etmemesi** için dosyanın TAMAMI (Definition, §Model 1-5, Implications, Relationships, Examples, Laws, Failure, SKR-tabloları) otomatik-yazma/edilgen-güncelleme fiilleri (yazar/yazılır/güncelle/günceller/güncellenir/beslenir/iyileşir/yansıtır) için tek tek tarandı — **P7-gate'siz automasyon başka hiçbir yerde bulunmadı**. §Relationships satır 249 "learning memory'ye yazılır" referent'i ENS-2003 olgu-kaydı/retention katmanı (a) olduğundan doğru; §4 double-loop "güncelle" dili kavramsal-tanımdır ve §4a onu açıkça P7-kapısına devreder (kavram→operasyonelleştirme, çelişki değil — indicative "yazar" eylem-iddiasından niteliksel farklı). T1/T2 (SKR-043'te doğrulanmış) regresyona uğramadı; v0.3.2 yeni atıf eklemedi (uydurma-riski yok); glossary (ENS-4000 v0.2.5) tutarlı. **`canon: false` KALIR** — Külliyat-girişi ayrı governance edimi; SKR-044 yalnızca skeptic-kapısını geçirir. Non-blocking N1 (§4 satır 154 double-loop dili isteğe bağlı defense-in-depth parantezi alabilir; kapıyı durdurmaz). **Sıradaki adım: ENS-2004 skeptic-kapısı KAPANDI; `canon` governance kararı + Faz-4 kodlaması (E1: §4a öneri-üretim mekanizması + ENS-4025 L8 Assumption→Expected eşlemesi) açık.** Bkz. `2000-theory/reviews/SKR-044-learning-theory-v032-t3-closure-fulldoc-scan.md`. **✅ 2026-07-24 — Faz-4 kod tarafı yazıldı: `Ens.Kernel/Domain/ReflectiveDoubleLoop.cs` (+`ReflectiveDoubleLoopTests.cs`, 10 test, toplam 69→79 yeşil, gerçek `dotnet test`).** §4a öneri-üreticisi minimal ve dürüst kodlandı: `Propose(...)` salt-okunur `ReflectiveProposal` listesi döner. **P7 kapısı YAPISAL:** sınıfın state değiştiren hiçbir metodu yok (ne Apply/Commit/Update) — bir refleksiyon-testi (`GetMethods` → yalnızca `Propose`) bunu kanıtlıyor; öneri uygulanamaz çünkü uygulayacak metod yok. Dürüstçe işaretli Faz-4 sadeleştirmeleri (SKR-001 disiplini): (a) **yön-sapması DEĞİL büyüklük-tekrarı** — `LearningMagnitude` işaretsiz + `OutcomeObserved`/`LearningRecorded` `string` tutuyor, ENS-2004 §1'in işaretli `learning_signal = Actual−Expected`'ı Faz-4'te yok; ayrıca ENS-4025 L8 proof-trace okunmuyor, girdi yalnızca CompanyMemory kayıtları (reflektif "neden"in zayıf yaklaşığı); (b) eşikler (`minSupportingRecords`/`magnitudeThreshold`) kalibre edilmedi (ENS-2003 γ'sıyla aynı borç); (c) öneri-yorgunluğu (P5) bir önceliklendirme/limite bağlanmadı — `Propose` sınırsız öneri dönebilir (bilinçli açık, ENS-2004 §Failure). **E1'in "öneri-üretim mekanizması" parçası artık kodlu (kısmi); ENS-4025 L8 Assumption→Expected eşlemesi hâlâ açık.** **🔍 2026-07-24 — K5'in kalan üç kolu paralel Explore ile dürüst-envanterlendi (brain/pusula/operax disipliniyle, önce kod-kontrolü sonra iddia):**
- **reporthub→audit/RBAC/plugin: KISMİ değerli.** Gerçek kimlik "Mosaik" (modular monolith portal). Audit: olgun, 65 dosyada kullanılan `IAuditLog`/`AuditLogService`, fail-safe tasarım (audit hatası iş akışını kırmıyor) — doğrudan referans alınabilir desen. Plugin: `ModuleLoader.cs` gerçek dinamik yükleme (assembly-scan + reflection, `Assembly.LoadFrom`/`Activator.CreateInstance`), 6 gerçek modül (`Mosaik.Modules.*`) — **ADR-0001 §6 (Capability Registry, "kernel değişmez, Pack takılır") için somut, çalışan emsal.** RBAC: gerçek ama ham — CSV-tabanlı (`ReportCatalog.AllowedRoles`), kodun kendi yorumu "ADR-004 adayı: deprecate edilecek" diyor; devşirilecek mimari değil, "böyle yapma" dersi.
- **AtlasOPS→MCP: KISMİ değerli, YÖN FARKLI.** `src/AtlasOps.Mcp/` gerçek ve çalışan: resmi `ModelContextProtocol` SDK (v1.2.0), stdio transport, attribute-tabanlı tool-registration (`[McpServerToolType]`), 3 read-only tool. Kanıt-üstü kanıt: bu oturumun kendi deferred-tool listesinde `mcp__atlasops__*` fiilen kayıtlı/çalışır durumda. **Ama ADR-0001 §7 (LLM Adapter Port, model-agnostik çalıştırma) ile örtüşmüyor — ters yönde çalışıyor** (agent'ın dışarıdan AtlasOps verisine erişmesi, ENS kernelinin dışarıdaki bir modele konuşması değil); **§6 Capability Pack örneği** olarak referans verilebilir, §7'ye "hazır bileşen" değil.
- **DikkatIQ→Attention(P5): DEĞERSİZ bu iddia için** (brain gibi reddedildi, ama farklı gerekçeyle — DikkatIQ kendisi gerçek/çalışan bir sistem, yalnızca P5 örtüşmesi yok). İsim yanıltıcı: "dikkat" = sözleşme-yükümlülük hatırlatması, ENS'in "attention = kıt bilişsel kaynak tahsisi" kavramı değil. `DecisionGravity.AttentionPriority = InfoNeed×ConformanceDeficit` formülünün hiçbir karşılığı yok — yalnızca tarih-bucket sayacı (`CriticalCount`/`UpcomingCount`/`OverdueCount`, tek kriter `OrderBy(DueDate)`). `Obligation.Amount` (stake) ve `Confidence` enum'u ayrı ayrı var ama hiç çarpılıp birleştirilmiyor.
- **K5 genel durum:** brain ❌, DikkatIQ ❌ (bu iddia için), reporthub △ (audit+plugin değerli, RBAC değil), AtlasOPS △ (MCP değerli ama §6'ya, §7'ye değil), pusula ✅✅ (iki teori uzantısı + kod, tam zincir). ~~**Sıradaki olası adım:** reporthub'ın ModuleLoader deseni ADR-0001 §6'ya (Capability Registry) somut kanıt/emsal olarak eklenebilir — ayrı bir küçük iş, henüz yapılmadı.~~ **✅ 2026-07-24 — YAPILDI (ens-architect, ADR-0001 v0.3.0→v0.3.1, `status: accepted` KALIR):** §6'ya (a) Mosaik/reporthub `IMosaikModule`+`ModuleLoader` (6 üretim modülü, ADR-018 opt-in-capability deseni) CrewOps'un yanına **ikinci bağımsız dahili kanıt** olarak eklendi — kernel+plugin deseni iki gerçek sistemde bağımsız yakınsadı; (b) yeni **§6.1 Prior art** alt-bölümü (5-başlık formatı, §3 ile aynı) gerçek dış literatürle: **OSGi** (Declarative Services/service registry → Registry+versiyonlama), **MCP** (`tools/list`/`listChanged` → tool-calling+keşif, en yakın çağdaş analog ama yetkilendirmeye agnostik), **Terraform Providers** (SemVer registry+constraint → versiyonlama/çakışma; "sessizce çözmez/uyarır" felsefesi ortak), **VS Code** (activation events+contribution points → progressive 3-tier'ın en güçlü eşi ama perf-güdümlü, ENS'te P5-güdümlü), **K8s Operators/CRD** (deklaratif tipli genişletme; dürüst not: ENS'te asıl karşılığı §6 değil Learning-loop/P4), **WordPress hooks** (ham "çekirdeği değiştirmeden genişlet"; governance karşı-örneği). **Dürüst delta (abartısız):** ENS plugin mimarisini *icat etmez* — mekanizma olgun/çok-kez-yeniden-keşfedilmiş prior-art; ENS'in dar katkısı yalnızca iki bağ: (1) eklenti-birimini ENS-4010 `Capability` node'una tiplemek (prior-art'ın hiçbiri eklentiyi bir foundational ontology'ye bağlamaz), (2) Pack'in deklaratif `allowedTools`/`requiresHumanApprovalFor` izinlerini doğrudan Bounded-Autonomy Gate'e (P7) bağlamak (per-capability human-approval'ı birinci-sınıf registry alanı yapan tek sistem). |

### Blocking durumu
1. ✅ **Bulgu D** (ADR-0002↔ENS-4020 döngüsü) — kapandı.
2. ✅ **ENS-4020 Bulgu C** (Replenishment öneri/commitment belirsizliği) — SKR-028 ile çözüldü,
   node ikiye bölündü. ✅ **M1→M2 bağımsız teyit turu tamamlandı (SKR-030 survives → M2, G4).**
3. ✅ **ADR-0001 Bulgu A+B** — inline kapatıldı (v0.3). ✅ **Bağımsız 3. skeptic turu tamamlandı
   (SKR-029 survives).** Kalan yalnızca ens-ceo hiza incelemesi (K4, Accepted için).
4. ✅ **ADR-0002 Bulgu 1/2/3 kapatıldı (v0.3, 2026-07-24)** — SKR-025/027 ortak talepleri.
   Bulgu 1 operax kod-denetimiyle (Glob/Grep: RFQ 0 dosya, M04 yalnızca satış-faturası fiyat spec'i)
   grounding'lendi; "≥4→3 doğrulanmış lifecycle" tutarsızlığı giderildi; Bulgu 2 delta dili
   yumuşatıldı; Bulgu 3 Confidence-elicitasyon boşluğu OQ6'ya + ENS-3022 zincirine bağlandı.
   **Kalan tek adım: v0.3 için bağımsız YENİ ens-skeptic turu (öz-onay yasağı, G2/G3).**
5. **ENS-4010 profil↔registry çelişkisi — 🔧 ÜÇÜ DE DÜZELTİLDİ (ENS-4010 v0.4.0, ens-architect),
   ✅ SKR-038 bağımsız tur YAPILDI → verdict `wounded` (G2/G3):** üç hedeflenmiş yama (Kusur 1/2/3)
   **teknik olarak DOĞRU** ve bağımsız denetimden sağ çıktı; ama iki yara açık kaldı, blocking-5 KAPANMADI.
   - **✅ SKR-038 doğruladı:** (1) Claim/Learning artık conformant (`supports` domain'ine Claim +
     node-tipine-özel Assertion profili); (2) Resource genellemesi `required_by`'ı (SupplierRelationship)
     kapsıyor ve edgeless-Resource'u dışlıyor (fazla gevşetmiyor); (3) `part_of` domain-widening zinciri
     well-formed kılıyor (Team⊂Division⊂Company iz sürüldü), IR-002/IR-005 bayrağı gerçekten kaldırılabilir;
     enumeration-vs-subsumption kararı savunulabilir (`pursues` argümanı yüzeysel DEĞİL — pursues Trans:✗,
     invariant ona uygulanmaz).
   - **⚠️ Yara A (SKR-038 yeni bulgu, aynı sınıf):** v0.4.0'ın eklediği "Profile satisfiability"
     invariant'ı **tüm profillere uygulanmadı** → taranmamış 4. kusur: **Intent profili / Goal /
     `served-by`** (`serves` range'i {Purpose}; Goal `served_by` domain'inde yok → Goal conformant
     üretemez) + ikincil **Rule / `derived_from`** (Registry'de kayıtlı relation değil). "Kök tema
     çözüldü, dağınık bulgu değil" iddiası bununla yerel olarak yanlışlanıyor.
   - **⚠️ Yara B (SKR-038):** Agent profilindeki "kök Organization muaf" istisnası mekanik denetlenemez
     (kök-işareti yok → `part_of` Organization için sessizce optional'a düşer, keyfî owner-kararına açık).
   - **🔧 Yara A/B DÜZELTİLDİ (ENS-4010 v0.5.0, ens-architect):** (A-1) Intent profili node-tipine-özel
     yeniden yazıldı — Purpose→`served-by`, Goal→`refines`\|`pursued_by` (ikisi de conformant); (A-2)
     `derived_from` Relation Registry'ye resmen eklendi (`Constraint → Purpose/Constraint`, Trans:✗) —
     Constraint upstream kenar + Principle-closure kazandı; (B) "kök muaf" parantezli istisnası kaldırıldı,
     `part_of` yapısal-opsiyonel (N:1 0..1) yazıldı, kök **çıkarımsal** operasyonelleştirildi
     (`is_root(o) ≡ Organization(o) ∧ ¬∃x. part_of(o,x)`; ≥2 kök → Linter uyarı). **İki invariant
     (profile-satisfiability + transitivity well-formedness) TÜM 9 profile/17 profil×node satırına + 2
     Trans:✓ relation'a elle uygulandı** ve §"Invariant denetim tablosu"na yazıldı → başka taranmamış
     aynı-sınıf kusur yok. Profil-yazım tarzı da birleştirildi (hepsi node-tipine-özel disjunctive).
   - **✅ SKR-039 bağımsız 2. tur (taze context, G2/G3) → `survives` (2026-07-24).** Yara A-1/A-2/B
     bağımsız doğrulandı (Registry domain/range'leri elle takip edildi, Constraint gerçekten
     Principle-closure kazandı); **kapsamlılık testi** özellikle sınandı — 17 profil×node satırı +
     2 Trans:✓ relation sıfırdan bağımsız türetildi, SKR-038/D-1 deseninin (kendi invariant'ını
     tam uygulamama) 3. tekrarı **yok**. 3 bloke-etmeyen keskinleştirme: (1) Deliberative profili
     hâlâ flat/karışık; (2) `ens-core:derived_from`≠`ens-meta:derived_from` homonimi not düşülmeli;
     (3) downstream: ENS-4031 IR-002/IR-005 bayrağı artık kaldırılabilir (owner ens-philosopher'a).
     **✅ 2026-07-25 yapıldı: ENS-4031 v0.3.0 bayrağı kaldırdı, B1 kapandı.**
     **blocking-5 KAPANDI.** ENS-4010 `status: skeptic-cleared`, `maturity: M2` (ENS-4020 G4
     iki-bağımsız-validator deseniyle — SKR-038+SKR-039). `ratified`/`canon:true` ayrı governance
     edimi. Bkz. `4000-ontology/reviews/SKR-039-foundational-ontology-invariant-closure.md`.
   - **[TARİHSEL — üç yamanın ayrıntısı, doğrulandı:]** üç bağımsız örnek, tek kök tema — bir profilin
   zorunlu-kenarı o node-tipinin Registry-domain'inde yok / `Trans: ✓` relation'ın range'i domain'inde
   değil. Tek tutarlı düzenlemeyle kapatıldı (Node/Relation Registry + Semantic Profiles).
   - **Kusur 1** (SKR-028 bildirdi, SKR-030 teyit+genişletti): Assertion profili zorunlu çıkan
     `supports`|`invalidates` ister; ama `supports` domain'i {Capability,Evidence}, `invalidates`
     domain'i {Evidence} — **Claim VE Learning** bu profili Registry ile hiç sağlayamaz (yalnızca
     Evidence sağlar). `ens-core:Claim`/`ens-core:Learning` conformant instance üretemiyor.
     **🔧 DÜZELTİLDİ (v0.4.0):** `supports` domain'ine `Claim` eklendi + Assertion profili node-tipine
     göre yeniden yazıldı (Evidence→supports/invalidates; Claim→supports \| supported_by/invalidated_by;
     Learning→updates). Üç node de artık conformant. **Bağımsız skeptic bekliyor.**
   - **Kusur 2** (SKR-030 yeni): Resource profili zorunlu `consumed_by`|`supports` ister; ama duran/
     required kaynaklar (`SupplierRelationship`) `required_by` kullanır — profil listesinde yok.
     **🔧 DÜZELTİLDİ (v0.4.0):** Resource profili "en az bir kaynak-rol kenarı"na genellendi
     (`consumed_by`|`allocated_to`|`required_by`|`supports`); Registry değişmedi (kusur salt profil-
     tanımındaydı). **Bağımsız skeptic bekliyor.**
   - **Kusur 3** (SKR-031/B1 yeni, owner **ens-architect**): `part_of` `Actor → Organization`
     (domain≠range) tiplenip aynı anda `Trans: ✓` + §Composition line 152 zinciri deklare edilir —
     **kendi içinde tutarsız:** domain≠range bir relation transitive zincir kuramaz (orta node hem
     Organization hem Actor olmalı), zincir gizlice `Organization --part_of--> Organization` iddia
     eder ama bu kenar Registry'de yok. D-1 ile aynı sınıf. Örgütsel hiyerarşi (Team⊂Division⊂Company)
     gerçek olduğundan çözüm muhtemelen Registry genişletmesi: `Organization specializes Actor`
     /`CollectiveAgent` üsttürü VEYA `part_of` domain'ini `Actor/Organization → Organization` yap.
     Bu düzelene dek ENS-4031 IR-002/IR-005-part_of "lisanslı değil" bayrağıyla üretim yapmıyor.
     **🔧 DÜZELTİLDİ (v0.4.0):** `part_of` domain'i `Actor/Organization → Organization` genişletildi
     (subsumption değil enumerasyon — mevcut `pursues: Actor/Organization → Goal` desenine tutarlı,
     daha az ontolojik ağırlık). Artık range {Organization} ⊆ domain {Actor,Organization} → zincir
     well-formed. **Bağımsız skeptic bekliyor. `part_of` düzeldiği için ENS-4031 IR-002/IR-005
     bayrağı artık kaldırılabilir hâle geldi — ama o edim ENS-4031 owner'ınındır (ens-philosopher);
     ens-architect IR-002'ye dokunmadı.** **✅ 2026-07-25: ens-philosopher bayrağı ENS-4031 v0.3.0'da
     kaldırdı; B1 kapandı (ENS-4010 v0.5.0 + SKR-038/039 teyidine dayanarak).**
   - Owner: ens-philosopher (Kusur 1/2 profil borcu) + **ens-architect (Kusur 3 = `part_of` tipleme)**.
     🔧 **Üçü de ens-architect tarafından tek tutarlı düzenlemede kapatıldı** (Kusur 1/2 profil borcu
     da ENS-4010 owner'ı olduğu için ens-architect'te; ens-philosopher'ın IR-002 dosyasına dokunulmadı).
     formal-checker (G-09/10) yazılınca iki invariant ilk testlerden olmalı: "her profil zorunlu-kenarı
     o node'un Registry-domain'inde var mı?" **ve** "her `Trans: ✓` relation'ın range'i domain'inde
     midir (zincirlenebilir mi)?" — **ikisi de ENS-4010 v0.4.0 §Relation Registry'ye invariant olarak
     yazıldı.** ENS-4020'yi bloke ETMEZ (M2 verildi); ENS-4010'un borcuydu → düzeltmesi skeptic bekliyor.

### Sıradaki (öneri, 3-paralel-iş eşiğini aşmamak için — context-management.md İlke 4)
K1 (3. tur ✅ survives), K3 (M2 ✅), K2 (v0.3 ✅ survives SKR-037) ve **blocking-5 (ENS-4010 ✅ M2,
skeptic-cleared, SKR-038+039)** bu turda kapandı. Açık kalan: **K4 (ens-ceo hiza incelemesi —
ADR-0001 Accepted için zorunlu, Madde XIV)** — kernel hattının tek kalan kritik-yol adımı.

## Faz 1 Külliyat borcu
| ID | İş | Kaynak |
|----|-----|--------|
| OL1 | ✅ **eklendi (ENS-2001 v0.3, §Model 2/3) → SKR-033 survives (2026-07-24), ENS-2001 `ratified`'a döndü** — Alternative-başına `ExpectedValue`; Reasoning fazında tüm Alternative'ler için; `Stake=spread(EV)` (ENS-3022) + seçim rasyonalitesi (ENS-2004 §5ii). Sözlük ENS-4000 v0.2.2'de M1 girdi. **Açık (bloke etmez): SKR-033 talep-2 — ordinal (seçim-rasyonalitesi) vs cardinal (Stake) EV kıyaslanabilirlik ayrımı + karar-içi/kararlar-arası ölçek ayrımı ens-philosopher'a devredildi.** | SKR-010 (ENS-2004 §5.ii ihtiyacı) |
| OE1 | ✅ **eklendi (ENS-2001 v0.3, §Model 2/3) → SKR-033 survives (2026-07-24), ENS-2001 `ratified`'a döndü** — `intent: exploit\|explore`; Commitment anında (sonuçtan önce, event-sourced); ENS-3021 §Model 3 ölçüm-filtresi. Sözlük ENS-4000 v0.2.2'de M1 girdi. **Açık (bloke etmez): SKR-033 talep-1 — intent'in `DecisionCommitted` event *alanı* olduğu (mühürle atomik) + değişmez-depolama varsayımı teoride keskinleştirilmeli; Faz-4 `DecisionAggregate.DecisionCommitted` henüz intent/EV taşımıyor, kod-borcu izlenmeli (K5).** | SKR-011/012 (ENS-3021 §Model 3 ihtiyacı) |
| R2 | Attribution'ın ampirik gücü kanıtlanmadı | ENS-1000 §VII, ENS-2004 — Faz 4 bekliyor |

> ~~Not: ENS-3021/3022 bu iki alana **dayanıyor** ama ENS-2001'de yoklar — kırık cross-doc bağımlılık.~~
> ✅ **2026-07-24 — kapatıldı (ens-philosopher):** her iki alan ENS-2001 v0.3'e eklendi
> (ExpectedValue → §Model 2 anatomi + alt-alan; intent → §Model 2 + §Individuation "bozmaz"
> kontrolü; ikisi de §Model 3 faz-yerleşimi + §Relationships + §Failure). ENS-2001 `status:
> ratified → review` (additive revizyon, yeni skeptic turu gerekir; G2/G3 gereği yazar
> onaylayamaz). ENS-3021 §Model 3 forward-ref'i "eklendi" olarak güncellendi. **Cross-doc
> bağımlılık artık kırık değil; kalan tek adım: bağımsız ens-skeptic turu → survives ile
> ENS-2001 `ratified`'a döner.** (Faz-4 kod tarafı `stake`/`intent`'i artık teoriden
> besleyebilir — 7000-reference-implementation `DecisionGravity.InfoNeed` ve `DecisionEntropy`
> filtresi.)

## 🧊 Freeze-fix backlog (Global Mimari Denetim Raporu'ndan, uygulanmadı)
| ID | İş | Öncelik |
|----|-----|---------|
| ~~G-02~~ | ~~Ontology→Theory bağımlılık yönü ters (ENS-4010 depends_on ENS-2001'i tersine çevir)~~ | ✅ 2026-07-23 — **bulgu REDDEDİLDİ (ens-architect mimari denetim kararı).** Yön zaten doğru: ENS-4010 (tip sistemi) Theory'yi *formalize eder/tipler*, ondan türer — her node tanımı ENS-2001/2/3/4'ü kaynak gösterir ve Semantic Closure her node'un Theory/Law/Principle'a yukarı-izlenmesini şart koşar (teori-önce; Madde XII + REGISTRY 2000→4000 sırası). Tersine çevirmek **Theory→4010→4001→4000→Theory döngüsü** yaratırdı (Madde XII ihlali); önerinin literal hali (ENS-2001 depends_on ENS-4010) doğrudan 2-döngü. Denetim, denetimin işaret ettiği gerçek eksiği açığa çıkardı: Theory künyelerinde `referenced_by: ENS-4010` back-link'i yoktu → eklendi (ENS-2001/2002/2003/2004). |
| ~~G-17~~ | ~~Theory↔Glossary döngüsel bağımlılık~~ | ✅ 2026-07-23 — **ens-style-guardian, terminoloji-sink çözümüyle kapattı (ENS-4000 v0.2.1).** Glossary içeriği incelendi: hiçbir girdi Theory/Law belgelerinin gövdesine `depends_on` gerektirecek biçimde atıf yapmıyor — yalnızca terim tanımlıyor ve (bilgi amaçlı, mekanik olmayan) "bkz. ENS-2001" işaretçileri taşıyor. Bu normal sözlük semantiği: kullanan (Theory/Law) tanıma bakar, tanım kullanana bağımlı olmaz. `ENS-4000.depends_on` artık yalnızca `[ENS-0000]` (kök); `2000-theory`/`3000-laws` çıkarıldı — ayrıca bu girdiler zaten REGISTRY'de kayıtlı gerçek kimliklere çözülmüyordu (dizin adıydı, traceability.md kural 2 ihlali). `ENS-2001..2004`/`ENS-3021..3023`'ün `depends_on: ENS-4000`'i (Theory/Law → Glossary yönü) korundu — döngü yaratmıyor, terim tanımına başvurmanın normal yönü. `ENS-4000.referenced_by` eklendi: `[ENS-2001, ENS-2002, ENS-2003, ENS-2004, ENS-3021, ENS-3022, ENS-3023, ENS-4001]` (ona `depends_on` ile bağlı olan tüm belgeler). Elle iz sürüldü: ENS-4000 → ENS-0000 → (kök, depends_on: []) — geri kenar yok, döngü kalmadı. | P1 |
| G-18 | **referenced_by back-link hijyeni sistematik eksik/ters** (G-02 denetiminde bulundu): ENS-2002 boş (oysa ENS-2003/2004 ona bağlı), ENS-2004 ters yön ([ENS-2002, ENS-2003] — bunlar 2004'e bağlı değil, 2004 onlara bağlı). formal-checker (G-09/10) yazılınca `depends_on`↔`referenced_by` iki-yönlü tutarlılık invariant'ı ile korpus geneli toplu düzeltilmeli (laws dahil tam graf audit'i). | P2 |
| G-03/05 | `constitutive:true` metadata alanı resmî eklenmedi; Anayasa Madde IV eski "0/1/3/4 doğası gereği Canon" diliyle çelişiyor. **🚧 2026-07-24 — RFC-6001 taslağı yazıldı (ens-philosopher), `status: draft`.** Önerir: (a) `constitutive: true\|false` alanını künye şemasına ekle; (b) Madde IV'ün "aralık=canon" cümlesini "canon aralıktan değil, türe uygun doğrulama yolundan kazanılır" kuralıyla değiştir (`constitutive:true` → ratifikasyon/skeptic-tutarlılık; `constitutive:false` → failure conditions + skeptic → M3, M5 Faz-4 kanıtı). Tek RFC (Anayasa+şema birlikte, atomik). **⚠️ Anayasa DEĞİŞMEDİ — yalnızca RFC taslağı var. Sıradaki adım: `ens-skeptic` §9 failure conditions'a saldırır (Madde XV-b) → survives → ens-ceo hiza (Madde XIV) → ancak o zaman ENS-0000 fiilen düzenlenir + korpus retrofit (§8: ENS-0000/4000/4001/4010 vb.).** **🛡️ 2026-07-24 — SKR-034 (bağımsız context) yazıldı → `wounded`, RFC `status: draft→skeptic-challenged`.** Çekirdek tez (iki dik eksen; canon türe-uygun doğrulama yolundan kazanılır; Madde IV yeniden yazımı) + tek-RFC kararı **sağ çıktı**; Quine'a mütevazı yanıt güçlü. **3 blocking talep (ratified'den önce kapanmalı):** (D1) §7.3 "constitutive:true → maturity taşımaz" ile fiili ENS-4001/4010/4025 künyeleri (`maturity:M2`, skeptic-kazanılmış) çelişiyor — retrofit yönü (M sökülecek mi, yoksa bunlar constitutive:false mı?) belirsiz + maturity-model.md/KULLIYAT.md eş-düzeltme yükü sessiz geçilmiş; (D2) `constitutive:true` tekdüze değil — kök (ENS-0000/Madde III) ampirik çekirdeği "tutarlılık yanılma-kipi"yle dokunulamaz, Lakatos hard-core prior-art'ıyla dürüstçe konumlanmalı; (D3) 4000-aralığı sınıflaması ertelenmiş ama §8 zaten çelişkili taahhüt veriyor — işleyen turnusol testi gerek (failure-condition #1/#3 savunması). **+Yan bulgu:** ENS-4000 `canon:true` ama `status:review` (SKR yok) — RFC'nin kendi canon-kazanma ölçütünü poster-çocuğunda ihlal ediyor. **2 keskinleştirme:** D4 çift-owner kabul kapısı (ens-ceo+ens-style-guardian), D5 Kelsen/Hart + Carnap prior-art. **Sıradaki adım: ens-philosopher D1/D2/D3'e yanıt + RFC v0.2 → bağımsız 2. skeptic turu.** Bkz. `6000-rfc/reviews/SKR-034-rfc-6001-constitutive.md`. **✍️ 2026-07-24 — ens-philosopher SKR-034'e yanıt verdi, RFC v0.1.0→v0.2.0, `status: skeptic-challenged→review`.** Üç blocking + iki keskinleştirme + yan bulgu kapatıldı: (D1) 4000-ontolojileri (ENS-4001/4010/4025/4020) `constitutive:true`→**`constitutive:false`** olarak yeniden sınıflandı — kök neden §7.3 değil §8'in yanlış sınıflamasıydı; §7.3 künyelerle tutarlı bir invariant'a döndü (`maturity` taşıyan ⟺ `false`), **hiçbir skeptic-kazanılmış M2/M3 grade sökülmez**, yalnızca eksik bayrak eklenir (§8.1); (D2) `constitutive:true` heterojenliği kabul edildi — §4.1 **immutable-core (Lakatosçu hard core, ENS-0000 Md III) vs revisable-constitutive (protective belt)** ayrımı + §7.4 opsiyonel `immutable_core_sections` alanı; Madde X iddiası "yalnızca protective belt için keskinleşir, immutable-core için ampirik-düzeyde bilinçli açık" diye nitelendi; (D3) §4.2 **işleyen turnusol testi (Test A kaldırma / B yanılma-kipi / C yeterlilik)** eklendi ve §8.1'de her fiili-künye örneğine **gösterilerek** uygulandı (erteleme kaldırıldı); (D4) çift-owner kabul kapısı (ens-ceo+ens-style-guardian, §7.5); (D5) Kelsen/Hart Grundnorm + Carnap prior-art (§3). **Yan bulgu:** ENS-4000 `canon:true`/`status:review`/SKR-yok — örnek çıkarılmadı, RFC kuralının uygulaması olarak §8.3'te açığa alındı (retrofit'te kurucu-tutarlılık incelemesiyle canon kazanır ya da `canon:false`'a iner). **⚠️ ÖZ-ONAY YOK (G2/G3): RFC `survives` değil — bağımsız 2. `ens-skeptic` turunu bekliyor.** Eş-düzeltme yükü (maturity-model.md/KULLIYAT.md notlandırması) §10.5'te üstlenildi, Accepted sonrasına ertelendi. **🛡️ 2026-07-24 — SKR-035 (bağımsız 2. tur, taze context) yazıldı → `wounded`, RFC `status: review→skeptic-challenged`, `skeptic_review: [SKR-034, SKR-035]`.** Bağımsız künye okumasıyla doğrulandı: D2 (Lakatos immutable-core — kaçamak değil, dürüst asimetri), D3-çekirdek (turnusol Test A/B/C ENS-4000 & ENS-3021'de bağımsız tutarlı), D4 (çift-owner kapı) ve D5 (Kelsen/Hart/Carnap atıfları gerçek, uydurma yok) **gerçekten kapatıldı**; 4000-aralığı D1 çelişkisi çözüldü, hiçbir M-grade sökülmüyor. **Yeni blocking (D6):** D1'i kapatmak için getirilen `maturity ⟺ constitutive:false` çift-yönlü invariant'ı (§7.3/§8.1) fiili künyelerle **hâlâ çelişiyor** — RFC'nin `constitutive:true` ilan ettiği **governance ailesi (GOV-000/GOV-010/capability-matrix/canonical-process) fiilen `maturity:M1`+evidence taşıyor** (grep doğrulandı); §10.5 "GOV-* M-ekseninde değildir" **olgusal yanlış**; dahası §4.2 turnusolu (Test A → GOV `true`) ile §7.3 invariant'ı (M taşır → `false`) governance'ta **zıt karar** veriyor. Bu, SKR-034 W1'in bir aralık öteye taşınmış tekrarı: yazar gösterilen 4000-örneklerini düzeltti ama invariant'ın evrensellik iddiasını korpus geneli denetlemedi. **Sıradaki adım: ens-philosopher D6'ya yanıt (RFC v0.3) → bağımsız 3. skeptic turu; `ens-ceo` Madde XIV yalnızca `survives` sonrası.** Bkz. `6000-rfc/reviews/SKR-035-rfc-6001-constitutive-round2.md`. **✍️ 2026-07-24 — ens-philosopher SKR-035'e yanıt verdi (3. fix turu), RFC v0.2.0→v0.3.0, `status: skeptic-challenged→review`.** D6 (blocking) kapatıldı: **çift-yönlü `maturity ⟺ constitutive:false` invariant'ı fazla iddialıydı** (SKR-035 talep-a seçildi) → §7.3 **tek-yönlü gerekli-koşula** indirgendi (`constitutive:false ⇒ maturity taşır`; contrapositive `maturity yok ⇒ true`; ters yön kaldırıldı), §4.2'de **turnusol birincil sınıflayıcı** ilan edildi (D7 de böyle kapandı), governance `constitutive:true` kaldı ama `maturity:M1` **etiketi ile canon-yolu ayrıştırıldı** (etiket canon'u gate etmez → GOV artık çelişki değil izinli örnek), §10.5'in olgusal-yanlış "GOV-* M-ekseninde değildir" cümlesi düzeltildi, §8.1/8.2 "invariant gereği"→"Test C gereği" olarak yeniden yazıldı, §9'a failure-condition #5 (ayrıştırmanın kendi yanılma koşulu) eklendi. **Korpus geneli `grep ^maturity:` bağımsız doğrulandı** (owner da kontrol etti): governance ailesi tam olarak GOV-000/010/020/030 (dördü de M1+evidence), ADR-0001/0002 kapsam-dışı (5000-architecture). D8 (immutable-core verme yordamı = yalnızca Madde XV) §7.4'te kapatıldı. **⚠️ ÖZ-ONAY YOK (G2/G3): `survives` değil — bağımsız 3. `ens-skeptic` turunu bekliyor.** Bkz. §12 "SKR-035'e yanıt". **🛡️ 2026-07-24 — SKR-036 (bağımsız 3. tur, taze context) yazıldı → `survives`, RFC `status: review` kalır, `skeptic_review: [SKR-034, SKR-035, SKR-036]`.** D6 çözümü bağımsız doğrulandı: çift-yönlü invariant fazla iddialıydı, tek-yönlü gerekli-koşula (`constitutive:false ⇒ maturity taşır`) indirgenmesi + turnusolun birincil sınıflayıcı olması bir **yama değil geri çekilme** — sınıflama yükü SKR-035'in zaten tutarlı bulduğu Test A/B/C'ye devredildi. **Bağımsız `grep ^maturity:` korpus taraması RFC §12 tablosuyla birebir** (ENS-2001..2004/3021..3023 M3; ENS-4001/4010/4020/4025/4030 M2; ENS-4031 M0; GOV-000/010/020/030 M1+evidence; ENS-0000/4000 maturity yok; ADR kapsam-dışı); GOV id eşlemesi (roles.md=GOV-010/capability-matrix.md=GOV-020/canonical-process.md=GOV-030) `grep ^id:` ile teyit — yazarın düzeltmesi doğru. **Kritik teyit:** governance evidence fiilen `{sci:E1, eng:E0, ops:E0, econ:E0}` — ampirik boyutlar (eng/ops/econ) **E0**, yani failure-condition #5'in çöküş koşulu (governance evidence ampirik-yeterlilik yüklerse (b) doğru olurdu) **tetiklenmiyor** → ayrıştırma doğrulandı, FC#5 dürüst açık (öz-baltalama değil). Invariant vacuous değil: tam bir hücre yasak (`constitutive:false + maturity yok`, korpusta boş ama falsifiable). D7 (§4.2 turnusol-önceliği) + D8 (§7.4 immutable-core = yalnızca Madde XV) kapalı. **Üç turluk döngü yakınsama** (çekirdek tez değişmeden sağ çıktı, her düzeltme iddiayı hakikate zayıflattı — SKR-001 dersi), sarmal değil. **3 bloke-etmeyen keskinleştirme:** S1 invariant'ın tek yasak hücresini açıkça yaz; **S2 `amends:` alanına STD-MATURITY-MODEL ekle** — canon kuralının üçüncü lokusu `maturity-model.md` satır 34 (`canon:true yalnızca M5`), şu an amends'te yok; S3 governance M1'ini "olumsal alışkanlık" değil "maturity-model.md satır 51'in zorunlu kıldığı ama non-canon-gating etiket" diye yeniden nitelendir + §9 FC#5'e eng/ops/econ=E0 kanıtını ekle. **GERÇEK SIRADAKİ ADIM: `ens-ceo` Madde XIV hiza incelemesi + `ens-style-guardian` şema-imzası (§7.5 çift-owner kapısı) → ancak o çift-onaydan sonra `Accepted` + ENS-0000 Madde IV/metadata-header/(S2) maturity-model.md fiilen düzenlenir + korpus retrofit (§10). Skeptic `Accepted` veremez.** Bkz. `6000-rfc/reviews/SKR-036-rfc-6001-constitutive-round3.md`. **👔 2026-07-24 — `ens-ceo` Madde XIV hiza incelemesi tamamlandı → onaylandı** (North Star uyumlu, retrofit kayıtlı/engellemeyen borç, 3-tur skeptic döngüsü yakınsama; kapsam-orantısı gözlemi bloke etmiyor). Bkz. `5000-architecture/reviews/CEO-0002-rfc-6001-alignment.md`. **Kalan: `ens-style-guardian` şema-imzası (§7.5 çift-owner kapısı) → ancak o gelince `Accepted`.** Retrofit'in (§10) kendi ROADMAP satırına ihtiyacı var — CEO-0002 önerisi: K5 (Faz 5) başlamadan/onunla paralel kapatılmalı. **✅ 2026-07-24 — `ens-style-guardian` şema-imzası tamamlandı** (itirazsız — bkz. `6000-rfc/reviews/STYLE-SIGNOFF-RFC-6001.md`). **Çift-owner kabul kapısı (§7.5) tamamlandı → RFC-6001 `Accepted`. ENS-0000 Madde IV ve `metadata-header.md` şeması fiilen düzenlendi** (`constitutive`/`immutable_core_sections` alanları eklendi; ENS-0000 v0.3.0, `metadata-header.md` v0.2.0). **G-03/05 KAPANDI.** Kalan: G-19 (aşağıda) — korpus retrofit'i (§10: diğer belgelere `constitutive` alanı, ENS-4000 canon-incelemesi, maturity-model.md/KULLIYAT.md notu), ayrı iş kalemine taşındı. | ✅ P0 |
| ~~G-19~~ | ~~RFC-6001 §10 korpus retrofit'i~~ | ✅ 2026-07-25 — **ens-style-guardian, RFC-6001 §8.1/§8.2 turnusol tablosunu mekanik olarak künyelere işledi.** §8.1 (kesin): `ENS-0000`/`ENS-4000` → `constitutive:true` (ENS-0000 zaten kabul sürecinde işlenmişti; ENS-4000 eklendi), `ENS-4001/4010/4025/3021` → `constitutive:false` eklendi (M2/M3 grade'leri **korunur**, sökülmedi). §8.2 (leaning, owner uygulandı): `ENS-1000`/`ENS-3000` `maturity` taşımıyor → **çekirdek-tez/çerçeve baskın**, `constitutive:true`; `ENS-4020` → `constitutive:false` (Test C: SKR-030 ile sınanmış alan-yeterliliği); `GOV-000/010/020/030` → `constitutive:true` (Test A: kaldırılırsa roller/yetkiler tanımsız; fiili `maturity:M1` etiketi RFC-6001 §7.3 gereği olumsal/canon-gate-etmeyen kalır); `ENS-2001/2002/2004`, `ENS-3022/3023` → `constitutive:false` (ampirik teori/yasa, M3; ENS-2003 zaten işlenmişti). **`ENS-4030`/`ENS-4031` RFC'nin "muhtemelen false" önerisi kör kopyalanmadı — Test A/B/C bağımsız uygulandı:** her iki belge de gerçek-dünya alan-yeterliliği/sentetik-adequacy iddiası taşımıyor (SC/MC/EC axiom'ları ve IR-* kuralları ENS-4010 Registry'sinden **mekanik türetilen** iç-tutarlılık/well-formedness sözleşmeleri; failure conditions'ları "tamlık/predicate-dili/consistency" — domain-fit değil); Test A (kaldırılırsa Validation Rules/derivation tanımsız) + Test B (tutarlılıkla revize edilir, fiat'la değil) → **`constitutive:true`** (mevcut `maturity:M2/M0` olumsal etiket olarak korunur, RFC §7.3 tek-yönlü invariant'la tutarlı). 19 dosyaya `constitutive` eklendi (tam liste: final rapor). `ENS-4000` canon-borcu (§8.3) kapsam dışı bırakıldı — yalnızca not düşüldü, dokunulmadı (ayrı governance edimi, ens-philosopher/ens-ceo). `KULLIYAT.md` "gap #1" notu kapatıldı, tablolar retrofit'le hizalandı (ENS-4001 constitutive-core'dan yeni M2-ontoloji tablosuna taşındı; ENS-4030/4031 constitutive-core'a eklendi). `maturity-model.md` §10.5 iki-parçalı notu **henüz işlenmedi** — ayrı küçük takip (aşağıda). | ✅ P1 |
| ~~G-04~~ | ~~10 standard dosyasında künye yok (Madde XI ihlali)~~ | ✅ 2026-07-24 — **ens-style-guardian.** Gerçekte 11 dosyanın (`.claude/standards/*.md`, `context-management.md` dahil) hiçbirinde künye yoktu; hepsine `metadata-header.md` şemasına uygun YAML front-matter eklendi (`type: standard`, `canon: false`, `origin: ENS-0000 §<madde>`, `depends_on: [ENS-0000]`, `status: ratified`, `version: 0.1.0`, `last_reviewed: 2026-07-24`). **Şema kararı:** standartlar Külliyat'ın numaralı aralığı (`ENS-0xxx..9xxx`) dışında olduğundan `id` dosya-adı tabanlı yeni bir `STD-*` ad alanı kullanıyor (`STD-METADATA-HEADER` vb.) — bu ad alanı REGISTRY.md'nin **Aralık şeması**na `MOD-*`/`LAW-*`/`SKR-*` ile aynı desende ("—" numara aralığı) eklendi; sayısal aralık genişletilmedi. Tüm 11 id, traceability.md kural-1 uyarınca REGISTRY.md **Ayrılmış numaralar** tablosuna da işlendi (kayıtsız kimlik kalmadı). `owner` alanı içerik alanına göre en-yakın role atandı (ens-style-guardian/ens-philosopher/ens-skeptic/ens-architect/ens-ceo) — **bu bir stil/format çıkarımıdır, resmî rol ataması değildir**; ileride ilgili agent/governance tarafından değiştirilebilir. `principles: []` bırakıldı çünkü metadata-header.md'nin kendi notu bu alanı yalnızca Külliyat yapıtları için zorunlu kılıyor; standartların hangi P1-P8'i somutlaştırdığı bir anlam kararıdır, style-guardian yetkisi dışında — semantik atama gerekiyorsa ens-philosopher'a bırakılır. İçerik/kurallar değiştirilmedi, yalnızca künye eklendi. | P1 |
| G-06 | Constitution 15 madde — 6-8 sayfa hedefi kapanmadı | P1 |
| G-08 | `governance/versioning.md` + `deprecation.md` yok (EC-002/003/004 dangling ref) | P1 |
| G-09/10 | ~~Validation Generator + Ontology Linter (`formal-checker` agent) hiç yazılmadı~~ **🔧 2026-07-25 — V1 YAZILDI (ens-architect), deterministik araç olarak.** Karar: LLM-agent DEĞİL — geçmişte elle bulunan HER ontoloji kusuru (Kusur 1/2/3, D-1, Yara A/B, B1) aynı iki mekanik invariant'ın ihlaliydi ve pahalı çok-turlu skeptic döngüleriyle bulunmuştu; bu araç onları anında/ücretsiz/regresyon-önleyici yakalar. **Konum: `tools/ens-ontology-linter/`** (repo kökünde ayrı dizin, .NET/C# stack korundu). Gerekçe: `7000-reference-implementation` Madde VII gereği "teoriyi kanıtlayan referans kod"; bu araç teori kanıtlamıyor, **korpus-tutarlılığı denetliyor** (ENS-4010 markdown'ını okuyup iç-well-formedness'ini kontrol eder) → kernel'e koymak concern-leak olurdu. **İki invariant (V1 kapsamı):** (A) Profile-satisfiability — her profilin zorunlu ilişkisel kenarı, node-tipinin Relation Registry domain/range'inde var mı (kayıtsız relation = ayrı `UnregisteredRelationReference` bulgusu); (B) Transitivity well-formedness — her `Trans:✓` relation için `range ⊆ domain`. Araç iki invariant'ı **ham Node/Relation Registry tablolarından bağımsız türetir** — belgenin kendi "Invariant denetim tablosu"nu (insanın `Sonuç:✓` cevabını) OKUMAZ (tautoloji olurdu). **Pozitif+negatif kontrol yazıldı:** (P) `ControlTests.PositiveControl` gerçek ENS-4010'a karşı sıfır-ihlal bekler (+ boş-parse maskesini engelleyen sanity: ≥20 relation/≥15 node/≥8 profil/tam 2 Trans:✓); (N) `ControlTests.NegativeControl` sentetik `fixtures/broken-ontology.md`'de (gerçek korpus bozulmadı) üç tarihsel kusur sınıfını da (part_of Trans:✓ domain≠range=Kusur 3/D-1; Claim⊥supports/invalidates=Kusur 1; Rule/derived_from kayıtsız=Yara A-2) TAM yakaladığını assert eder. **⚠️ DÜRÜSTLÜK (SKR-001/SKR-041 emsali): araç+testler yazıldı ve iki kontrol de gerçek tablo verisine karşı elle-iz-sürerek doğrulandı, AMA canlı `dotnet build`/`dotnet test` bu ajan-context'inde ÇALIŞTIRILAMADI (shell etkin değildi, terminal apps typing-blocked) → yeşil koşu owner/CI teyidi bekler; hiçbir build/test çıktısı FABRİKE EDİLMEDİ.** **Dürüst sınırlar (README §Limits):** (1) markdown tablo parsing kırılgan — başlık/kolon değişirse sessizce yanlış "temiz" verebilir (en büyük risk); (2) profil zorunlulukları prose'dan çıkarılır (yalnız backtick token'lar relation adayı + stop-list); (3) profile-satisfiability "node başına en az bir sağlanabilir kenar" modeli — saf konjonktif zorunluluklar (Deliberative serves AND constrained_by) eksik-denetlenir → V2; (4) yalnız 2 invariant. **V2'ye ertelendi:** G-18 (`depends_on`↔`referenced_by` back-link hijyeni), node/edge completeness, cardinality, identity, Semantic Closure reachability, ≥2-kök uyarısı. Bkz. `tools/ens-ontology-linter/README.md`. **Sıradaki adım: owner/CI `dotnet test` yeşil koşusu → G-09/10 tam kapanış.** | ✅ P2 (V1, canlı-koşu owner teyidi bekliyor) |
| G-14 | **Inference Rules (ENS-4031)** — 🚧 taslak yazıldı (v0.1.0, canon:false/M0; 8 kural IR-001..IR-008). **ens-skeptic incelendi → SKR-031 `wounded`** (status: skeptic-challenged). Atıflar gerçek (uydurma yok), D-1 dürüstçe açığa çıkarılmış, IR-001/003/004/006/007 öncülleri Registry-sağlam, IR-008 deferral'ı meşru. **İki borç (ratified'den önce kapatılmalı):** (B1) IR-002/IR-005 `part_of` transitive zinciri ENS-4010 tiplemesi (`Actor→Organization`, zincirlenemez) tarafından lisanslı değil — **D-1 ile aynı sınıf ama belge onu işaretlememiş** → ENS-4010 owner'a (ens-architect) devir: `Organization specializes Actor` deklare et VEYA `part_of` domain'ini genişlet; (B2) "tekil-kaynak=ENS-4010 Registry" iddiası fazla geniş — IR-007'nin `contradicts`'i ENS-4001'de → ifadeyi yumuşat. **D-1 konumu netleşti: ENS-4031'in değil ENS-4025'in kusuru** (SKR-022 ratified turunda kaçırmış); ENS-4025 örneğini Registry'ye hizalama borcu taşır. **🔧 2026-07-23 — B1/B2/D-1 DÜZELTİLDİ (ens-philosopher), status: skeptic-challenged→review, v0.1.0→v0.2.0.** B1: IR-002 "Registry-bağımlı, lisanslı DEĞİL" bayrağıyla yeniden yazıldı (geçersiz Agent-profili savunması çıkarıldı, illicit çok-hop iddiası kaldırıldı), IR-005 concrete instance Registry-lisanslı IR-004 yoluna çevrildi, §Failure conditions B1 = ens-architect'e devir (ENS-4010 `part_of` tiplemesi kusuru upstream'de kalır). B2: header+Relationships "ENS-4010 Registry VEYA ENS-4001 Meta Model" olarak yumuşatıldı. D-1: **ENS-4025 v0.1.1'de** proof-trace örneği co-target join'e hizalandı (Strategy→Capability/Purpose), SKR-031 kaynak notu eklendi; D-1 artık ENS-4025 tarafında kapalı. ENS-4031'e "SKR-031'e yanıt" tablosu eklendi. **⚠️ HENÜZ survives/ratified DEĞİL — bağımsız 2. ens-skeptic turu bekliyor** (G2/G3: yazar kendi düzeltmesini onaylayamaz). **Açık kalan üst-borç: ENS-4010 `part_of` tiplemesi (B1, owner ens-architect).** | P2 |
| G-16 | Governance tek-operatör (rol ayrımı G2/G3 fiilen zayıf) | P3 |
| ~~G-11~~ | ~~Sözlük terfi/constitutive-M ayrımı~~ | ✅ 2026-07-23, ENS-4000 v0.2 |
| ~~G-13~~ | ~~Enterprise Ontology yok~~ | ✅ kısmen — ENS-4020 (yalnızca operax); reporthub/brain domain'leri hâlâ açık |

## ✅ Faz 4 BAŞLADI — ilk çalışan kod (7000-reference-implementation)
ADR-0001 **Accepted** (CEO-0001, K4 kapandı). Kod yazıldı ve **gerçekten derlenip test edildi**
(iddia değil, `dotnet test` çıktısı doğrulandı — **46/46 geçti**):
- `ContextScore` — ENS-2002 §3 formülü (`coverage−noise_penalty−staleness`) + §Implications
  Confidence-gate — **9/9 test geçti**; künye `eng: E0→E1` güncellendi ✅. `coverage` hâlâ
  dışarıdan verilir (CompanyMemory kodlandı ama henüz bağlanmadı — sıradaki adım).
- `CompanyMemory` — ENS-2003 §1 (Memory Graph) + §3 (retention ∝ |Learning|, karşı-survivorship,
  sönümle-silme) — **8/8 test geçti**; künye `eng: E0→E1` güncellendi ✅. `RetentionPriority`
  kasıtlı olarak `DecisionCapital.Value`'yu yeniden kullanıyor (ENS-2003 §Laws'ın kendi
  öngördüğü bağ — "Decision Capital... Memory Graph üzerinde tanımlanacak" ilk kez koda döküldü).
- `Ens.Kernel.Demo/` yeni proje — uçtan uca tedarikçi-seçimi senaryosu, gerçekten çalıştırıldı
  (`dotnet run`), teori tek akışta doğrulandı (Gate NotifyHuman→Commit sonrası InfoNeed düşüşü
  dahil, gerçek terminal çıktısıyla).
- `DecisionAggregate` — ENS-2001 §Individuation (event-sourced, commitment-sealed) — **8/8 test geçti**
- `DecisionEntropy` — ENS-3021 formülü (`H(A|C)=I(A;Owner|C)+H(A|C,Owner)`) — **5/5 test geçti**
  - ⚠️ **DÜZELTME (2026-07-26, AUDIT bulgusu W7c):** buradaki eski ifade —"zincir-kuralı
    matematiksel olarak doğrulandı"— **yanlıştı ve kaldırıldı**. `LevelNoise`,
    `Math.Max(0, hac − hacOwner)` olarak, yani bir **artık (residual)** olarak hesaplanıyor
    (`DecisionEntropy.cs:48`). Zincir kuralı özdeşliği bu durumda **inşa gereği** doğrudur;
    onu test etmek hiçbir şeyi yanlışlamaz. Tautoloji kanıt değildir (Madde X).
  - ✅ Buna karşılık **değerin kendisi bağımsız olarak doğrulandı**: `AUDIT_HOLDS_W7a`,
    500 veri kümesi üzerinde koşullu karşılıklı bilgiyi (CMI) bağımsız hesaplayıp
    `LevelNoise` ile karşılaştırdı — eşleşti. Yani sayı doğru; yanlış olan, o sayıyı içeren
    özdeşliği "doğrulama" saymaktı.
  - Açık kalan: `W7b` (clamp ölü kod), `W7d` (tek gözlemde örneklem güvencesi yok),
    `W7e` (tümü `null` gözlem "kusursuz tutarlılık" raporluyor), `W7f` (sahip kimliğinde
    harf farkı attribution'ı ters çeviriyor), `W7g` (normalize edilmemiş entropi).
    Ayrıntı: `7000-reference-implementation/DEFECT-REGISTER.md`
- `DecisionGravity` — ENS-3022 formülü (`InfoNeed=Stake×(1−Confidence)`) — **8/8 test geçti**;
  künye `eng: E0→E1` güncellendi ✅
- `DecisionCapital` — ENS-3023 formülü (Value/ΔCapital=yatırım−amortisman/ReuseROI) —
  **9/9 test geçti**; künye `eng: E0→E1` güncellendi ✅. **Fizik üçlüsü (Entropy, Gravity,
  Capital) artık üçü de kodlanmış ve test edilmiş.**
- `BoundedAutonomyGate` — ADR-0001 §5.6, P7'nin ilk gerçek zorlaması (yorum değil kod) —
  **6/6 test geçti**; `DecisionAggregate`'e eksik olan `Confidence` public property eklendi
- Yeni Faz-4 agent'ları: `ens-backend-architect`, `ens-test-engineer` (ROSTER'dan aktifleşti)
- ENS-2001/ENS-3021 künyeleri `eng: E1` — kontrol edildi, zaten güncel (önceki turda yapılmış).
  Artık ENS-2001/3021/3022/3023 dördü de `eng: E1`.
- **Bilinçli açık:** Context Score entegrasyonu yok, OL1/OE1 hâlâ eksik, Gate'in Policy modeli
  minimalist (gerçek ENS-4010 Constraint/Policy node'larına bağlanmadı), DecisionCapital'in
  Stok hesabı yok (Company Memory/ENS-2003 henüz kodlanmadı).

## 🆕 YENİ İŞ — G-02 + G-14 (paralel başlatıldı)
Kod tarafı (Gravity+Gate) biterken, ROADMAP'in "Sıradaki adım" kararı gereği doküman tarafında
paralel iki bağımsız agent başlatıldı (dosya çakışması yok):
- ✅ **G-02** (ens-architect) — **kapandı (bulgu reddedildi).** Ontology→Theory yönü zaten doğru;
  ENS-4010 tip sistemi Theory'yi formalize eder ve ondan türer (Semantic Closure + REGISTRY
  2000→4000 sırası + Madde XII). Ters çevirme Theory→4010→4001→4000→Theory döngüsü yaratırdı.
  Gerçek eksik (`referenced_by: ENS-4010` back-link'leri) düzeltildi. Denetim iki yeni bulgu
  çıkardı: **G-17** (Theory↔Glossary gerçek döngüsü) + **G-18** (referenced_by hijyeni). Bkz.
  Freeze-fix backlog.
- 🚧 **G-14** (ens-philosopher) — **taslak tamamlandı, ratified değil.** `ENS-4031-inference-rules.md`
  (v0.1.0, canon:false/M0) yazıldı — 8 kural (IR-001..IR-008: Composition ×2, Transitive ×2,
  Confidence-propagation ×2, explicit-negation ×2), hepsi ENS-4025'in L1-L8 sözleşmesine karşı
  doğrulandı. **D-1 (açık, skeptic'e devredildi):** ENS-4025'in kendi tanıtım örneği
  (`Purpose --supports--> Strategy`) mevcut ENS-4010 Registry ile lisanslı değil — `Strategy`
  node'u yok, `supports`'un domain'i Purpose içermez. **Sıradaki adım: ens-skeptic ENS-4031'i
  incelemeli** (D-1 dahil); survives→ratified/canon:true, wounded→D-1 önce kapatılır.
  - ✅ **1. tur — SKR-031 (wounded):** iki gerçek çatlak — B1 (`part_of` transitive Registry'ce
    lisanslı değil) + B2 (tekil-kaynak iddiası IR-007 `contradicts`/ENS-4001 ile gerilir); ayrıca
    D-1'in gerçek sahibi **ENS-4025** (ratified, SKR-022 turunda kaçmış) olarak saptandı.
  - ✅ **Düzeltme:** ENS-4025 v0.1.1 (D-1: proof-trace örneği Registry-sadık co-target join'e
    hizalandı) + ENS-4031 v0.2.0 (B1: IR-002 "lisanslı değil" bayrağı + ens-architect'e devir,
    IR-005 örneği `pursues ∘ refines` lisanslı yola çevrildi; B2: kaynak ifadesi ENS-4001'i
    kapsayacak şekilde yumuşatıldı).
  - ✅ **2. tur — SKR-032 (survives, bağımsız context):** B1/B2/D-1'in her biri Registry'ye karşı
    bağımsız yeniden doğrulandı; yeni kaçak yok; ENS-4025↔ENS-4031 örnek tutarlılığı kazanıldı.
    ENS-4031 `status: skeptic-cleared`. **Açık upstream borç (bloke etmez):** ENS-4010 `part_of`
    tiplemesi (owner ens-architect) — IR-002 bayraklı olduğundan ENS-4031'i engellemiyor.
    **Kalan governance edimi:** `ratified`/`canon: true` ayrı karar (skeptic vermez).

## Faz 4 not — Inference Provider adayı
**Cerebras** (OpenAI-compatible, DeepSeek/Qwen/Llama destekli, 1500-2100 tok/s, ücretsiz
kademe 1M/gün) — ADR-0001 §7 LLM Adapter Port için güçlü aday. Stateless-hızlı doğası ENS'in
"Memory kernel'de, inference provider'da değil" ilkesiyle uyumlu. Kod yazımında adapter
implementasyonu olarak değerlendirilecek.

## Hiç başlanmayan (kullanıcı tespitleri)
- **Repository Identity** — "bu repo ne üretir" tek-sayfa netliği (standart/bilim/framework/platform?)
- **Evolution Strategy** — 2035'te ENS nasıl değişir; teori ekleme/deprecate/paradigma-değişimi tek model (en kritik eksik dendi)
- **Repository Registry şeması yükseltmesi** — REGISTRY.md'ye ID/Owner/Status/Lifecycle/Maturity/Validator/Namespace kolonları

## Sıradaki adım (karar) — 2026-07-26 güncellendi (4. kez)

**Aktif hat: kernel kusur borcunun mimari kapanışı.** 2026-07-26'da
`7000-reference-implementation/DEFECT-REGISTER.md` yazıldı: 68 açık `AUDIT_DEFECT_*` +
8 `AUDIT_FINDING_*`. Bunların **34'ü o güne dek hiçbir denetim raporunda yoktu** —
yalnızca test adı olarak vardılar (SECURITY dalgasının ajanı raporunu yazamadan öldü).

> **Okuma uyarısı:** 373/373 test geçiyor, ama burada yeşil panel sağlık değil
> **envanterdir**. `AUDIT_DEFECT_*` testinin geçmesi kusurun **var** olduğunu gösterir.

**1. öncelik — 6 mimari karar (33 kusuru birden kapatır).** Kusurlar bağımsız değil;
6 kalıptan doğuyorlar. Tek tek yamamak yanlış strateji. ADR-0001'e girmeli:

| # | Kalıp | Kusur | Karar |
|---|---|---|---|
| 1 | Public record = taklit edilebilir yetki (E3, W4a, W15, H1 → P7 düşüyor) | 6 | İmzalı gate-token |
| 2 | Kimlik normalizasyonu yok (homoglyph, case, NUL, boşluk) | 9 | Normalize edilmiş kimlik tipi (case/NFC/trim) |
| 3 | Zaman çağırandan geliyor, doğrulanmıyor | 5 | Monoton saat portu + gelecek-tarih reddi |
| 4 | Eşik `0` = sessiz global kapatma anahtarı | 5 | Nullable eşik + açık `Disabled` durumu |
| 5 | Reflection her değişmezi deliyor | 3 | Kernel içi mi, process sınırı mı? **Açıkça karar ver** |
| 6 | Canlı koleksiyon dönüyor | 5 | Dönüş tiplerinde zorunlu `ToImmutable*` |

**2. öncelik — bekleyen iki bağımsız doğrulama (ikisi de yarım):**
- `SKR-045` (ENS-2003 v0.4.0 / D-5) — ajan **üç kez** API stall ile öldü, iskelet diskte,
  gövde yok. **ENS-2003 v0.4.0 ve ENS-2004 v0.3.3 `review` olarak kalıyor.**
- `AUDIT-WAVE2-SECURITY.md` — iskelet diskte, gövde yazılıyor.
- `DEFECT-REGISTER.md` şiddet atamaları **bağımsız değil** (oturum sahibi atadı) —
  G2/G3 gereği bağımsız gözden geçirme borcu, belgenin §9'unda açık.

**3. öncelik — D-4 (Memory Graph yok).** `CompanyMemory` hâlâ 5 alanlı bir `List`,
sıfır kenar; ENS-2003 §1 beş kenar tipi ve "tüm Decision Object alanları" istiyor.
Üç ayrı borcun ortak kök nedeni (ENS-2002 relevance kestirimi, ENS-3022 PeerContext
uyumu, ENS-2004 §4a "hangi varsayım" analizi). Büyük mimari iş — sahip kararı bekliyor.

---

### Önceki karar (2026-07-24, 3. kez) — tarihsel kayıt
K4 (ADR-0001 CEO-0001, ADR-0002 CEO-0003) **ikisi de kapandı** — kernel hattı K1-K4 tam.
K5'in ilk kolu (pusula/sema→Memory) da kapandı: ENS-2003 v0.3.1 **ratified** (SKR-040 wounded→
SKR-041 survives), Faz-4 kod (`CompanyMemory.cs`) confidence-koşullu sürekli decay + curator
ile güncel, 69/69 test.

**Yeni aktif iş: Hermes Agent (github.com/NousResearch/hermes-agent-self-evolution) →
ENS-2004 (Learning Theory).** GEPA+DSPy trace-tabanlı self-evolution (execution-trace'ten
"neden başarısız" analizi + metric-gate + insan-PR-onay-gate) ENS-2004'ün henüz kodlanmamış
attribution/kalibrasyon tarafına doğrudan oturuyor — K5 araştırma zaten yapıldı, teori
uzantısına geçilebilir.

**Küçük, bloke-etmeyen takip işleri (birikmiş):**
- ~~ENS-4031 IR-002/IR-005 "lisanslı değil" bayrağı kaldırılabilir (owner ens-philosopher,
  part_of yaması ENS-4010 v0.5.0'da doğrulandı).~~ ✅ **2026-07-25, ENS-4031 v0.3.0:** B1 bayrağı
  kaldırıldı; IR-002/IR-005-part_of artık Registry-lisanslı üretim yapıyor. Upstream düzeltme
  ENS-4010 v0.5.0 (`part_of` domain `Actor/Organization → Organization`), SKR-038+SKR-039 iki
  bağımsız tur ile teyitliydi; ENS-4031 tarafında bayrak/başlık/L-matris †/SKR-031-yanıt tablosu/
  §Failure conditions B1 girdileri "kapandı" olarak güncellendi (tarihsel SKR kayıtları korundu).
- Deliberative profili hâlâ flat/karışık (SKR-038 talep-3'ün kalanı, owner ens-architect).
- `ens-core:derived_from` ≠ `ens-meta:derived_from` (ENS-4001) homonimi not düşülmeli.
- N1/N2/N3 (SKR-041): DecisionCapital.cs bayat yorumu ✅ düzeltildi; glossary "Salience Decay"
  ad-yakınlığı + homonim notu hâlâ açık.
- ~~G-19 (RFC-6001 korpus retrofit) hâlâ açık~~ ✅ 2026-07-25 kapandı (bkz. yukarıdaki G-19 satırı).
  Kalan küçük takip: `maturity-model.md`'ye RFC-6001 §10.5'in iki-parçalı notu (ENS-0000/ENS-4000
  M-ekseninde değil; GOV-* M-etiketi taşır ama gate etmez) henüz işlenmedi.
- K5'in araştırılmamış kolları: reporthub→audit/RBAC/plugin, AtlasOPS→MCP, DikkatIQ→Attention(P5).
