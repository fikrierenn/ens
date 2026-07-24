# ENS Roadmap — Açık İşler Kaydı

**Yetki:** Bu dosya konuşma-hafızası değil, **kalıcı proje kaydıdır** — LAW-ORG-MEMORY'nin
("unutulan kararlar tekrarlanan hatalara dönüşür") kendi projeye uygulanışı. Her oturum başında
buradan devam edilir; hiçbir açık iş yalnızca sohbet geçmişinde yaşamaz.

**Güncelleme kuralı:** bir iş kapanınca ✅ işaretlenir + kapatan artifact/SKR referansı eklenir;
asla satır silinmez (audit, EC-001 ile tutarlı).

---

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
| K1 | ADR-0001 (Cognitive Kernel + Bounded Autonomy) | **v0.3 — 3. tur survives** (SKR-029): Bulgu A/B gerçekten kapandı, D2/D3 bozulmadı. `status: draft`/Proposed. **ens-ceo hiza incelemesi (K4) bekliyor** — Accepted için, Madde XIV |
| K2 | ADR-0002 (operax → Operations Capability Pack) | v0.1 — SKR-025+SKR-027 wounded; Bulgu D ✅; **kalan:** Bulgu 1/2/3 (K2/K4 gözlem, delta-dil, Confidence-OQ). _(ENS-2001 v0.3 yan-etkisi: §5 Decision Object alan-sayısı referansı 12→13 senkronlandı — ayrı backlog değil.)_ |
| K3 | ENS-4020 (Enterprise Ontology — operax `ens-ent:`) | **v0.3 — M2** (SKR-030 survives): C.a/C.b teyit; iki bağımsız validator (SKR-028+SKR-030) → G4. `status: review` (ratified ayrı governance edimi). Faz-4: OF1/OF2 |
| K4 | **ens-ceo hiza incelemesi** | **yapılmadı** — Anayasa Madde XIV, Accepted için zorunlu adım |
| K5 | Sonraki değer yakalama | brain→Memory runtime, reporthub→audit/RBAC/plugin, AtlasOPS→MCP, DikkatIQ→Attention(P5) |

### Blocking durumu
1. ✅ **Bulgu D** (ADR-0002↔ENS-4020 döngüsü) — kapandı.
2. ✅ **ENS-4020 Bulgu C** (Replenishment öneri/commitment belirsizliği) — SKR-028 ile çözüldü,
   node ikiye bölündü. ✅ **M1→M2 bağımsız teyit turu tamamlandı (SKR-030 survives → M2, G4).**
3. ✅ **ADR-0001 Bulgu A+B** — inline kapatıldı (v0.3). ✅ **Bağımsız 3. skeptic turu tamamlandı
   (SKR-029 survives).** Kalan yalnızca ens-ceo hiza incelemesi (K4, Accepted için).
4. Açık küçük: ADR-0002 Bulgu 1 (K2/K4 gözlemi güçlendir), Bulgu 2 (delta-tablo dili), Bulgu 3
   (Confidence-OQ) — SKR-025/027 ortak.
5. **ENS-4010 profil↔registry çelişkisi — CONFIRMED + GENİŞLETİLDİ (SKR-030, blocking: ENS-4010'un
   kendi ilerlemesi):** iki bağımsız örnek.
   - **Kusur 1** (SKR-028 bildirdi, SKR-030 teyit+genişletti): Assertion profili zorunlu çıkan
     `supports`|`invalidates` ister; ama `supports` domain'i {Capability,Evidence}, `invalidates`
     domain'i {Evidence} — **Claim VE Learning** bu profili Registry ile hiç sağlayamaz (yalnızca
     Evidence sağlar). `ens-core:Claim`/`ens-core:Learning` conformant instance üretemiyor.
   - **Kusur 2** (SKR-030 yeni): Resource profili zorunlu `consumed_by`|`supports` ister; ama duran/
     required kaynaklar (`SupplierRelationship`) `required_by` kullanır — profil listesinde yok.
   - **Kusur 3** (SKR-031/B1 yeni, owner **ens-architect**): `part_of` `Actor → Organization`
     (domain≠range) tiplenip aynı anda `Trans: ✓` + §Composition line 152 zinciri deklare edilir —
     **kendi içinde tutarsız:** domain≠range bir relation transitive zincir kuramaz (orta node hem
     Organization hem Actor olmalı), zincir gizlice `Organization --part_of--> Organization` iddia
     eder ama bu kenar Registry'de yok. D-1 ile aynı sınıf. Örgütsel hiyerarşi (Team⊂Division⊂Company)
     gerçek olduğundan çözüm muhtemelen Registry genişletmesi: `Organization specializes Actor`
     /`CollectiveAgent` üsttürü VEYA `part_of` domain'ini `Actor/Organization → Organization` yap.
     Bu düzelene dek ENS-4031 IR-002/IR-005-part_of "lisanslı değil" bayrağıyla üretim yapmıyor.
   - Owner: ens-philosopher (Kusur 1/2 profil borcu) + **ens-architect (Kusur 3 = `part_of` tipleme)**.
     formal-checker (G-09/10) yazılınca iki invariant ilk testlerden olmalı: "her profil zorunlu-kenarı
     o node'un Registry-domain'inde var mı?" **ve** "her `Trans: ✓` relation'ın range'i domain'inde
     midir (zincirlenebilir mi)?". ENS-4020'yi bloke ETMEZ (M2 verildi); ENS-4010'un borcu.

### Sıradaki (öneri, 3-paralel-iş eşiğini aşmamak için — context-management.md İlke 4)
K1 (3. tur ✅ survives) ve K3 (M2 ✅) bu turda kapandı. Açık kalan: **K4 (ens-ceo hiza incelemesi —
ADR-0001 Accepted için zorunlu, Madde XIV)**, K2 (ADR-0002 küçük talepler), ve yeni **ENS-4010
profil↔registry borcu** (blocking-5, owner ens-philosopher). K4 kernel hattının kritik yolunda.

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
| G-19 | **RFC-6001 §10 korpus retrofit'i** (yeni, RFC-6001 Accepted'ın doğurduğu takip işi): diğer Külliyat yapıtlarına (`ENS-1000`, `ENS-3000`, `ENS-4001/4010/4020/4025/4030/4031`, `governance/*`) §8.1/8.2'de turnusolla önceden gösterilmiş `constitutive` değerini fiilen ekle (hiçbir M-grade sökülmeyecek — yalnızca eksik bayrak); `ENS-4000` canon'unu kurucu-tutarlılık skeptic incelemesiyle kazandır ya da `canon:false`'a indir (§8.3, owner ens-style-guardian); `maturity-model.md`/`KULLIYAT.md`'ye RFC-6001 §10.5'in iki-parçalı notunu işle. CEO-0002 önerisi: K5 (Faz 5) öncesi/paralel kapatılmalı. Owner: ilgili belge-owner'ları + ens-skeptic. | P1 |
| ~~G-04~~ | ~~10 standard dosyasında künye yok (Madde XI ihlali)~~ | ✅ 2026-07-24 — **ens-style-guardian.** Gerçekte 11 dosyanın (`.claude/standards/*.md`, `context-management.md` dahil) hiçbirinde künye yoktu; hepsine `metadata-header.md` şemasına uygun YAML front-matter eklendi (`type: standard`, `canon: false`, `origin: ENS-0000 §<madde>`, `depends_on: [ENS-0000]`, `status: ratified`, `version: 0.1.0`, `last_reviewed: 2026-07-24`). **Şema kararı:** standartlar Külliyat'ın numaralı aralığı (`ENS-0xxx..9xxx`) dışında olduğundan `id` dosya-adı tabanlı yeni bir `STD-*` ad alanı kullanıyor (`STD-METADATA-HEADER` vb.) — bu ad alanı REGISTRY.md'nin **Aralık şeması**na `MOD-*`/`LAW-*`/`SKR-*` ile aynı desende ("—" numara aralığı) eklendi; sayısal aralık genişletilmedi. Tüm 11 id, traceability.md kural-1 uyarınca REGISTRY.md **Ayrılmış numaralar** tablosuna da işlendi (kayıtsız kimlik kalmadı). `owner` alanı içerik alanına göre en-yakın role atandı (ens-style-guardian/ens-philosopher/ens-skeptic/ens-architect/ens-ceo) — **bu bir stil/format çıkarımıdır, resmî rol ataması değildir**; ileride ilgili agent/governance tarafından değiştirilebilir. `principles: []` bırakıldı çünkü metadata-header.md'nin kendi notu bu alanı yalnızca Külliyat yapıtları için zorunlu kılıyor; standartların hangi P1-P8'i somutlaştırdığı bir anlam kararıdır, style-guardian yetkisi dışında — semantik atama gerekiyorsa ens-philosopher'a bırakılır. İçerik/kurallar değiştirilmedi, yalnızca künye eklendi. | P1 |
| G-06 | Constitution 15 madde — 6-8 sayfa hedefi kapanmadı | P1 |
| G-08 | `governance/versioning.md` + `deprecation.md` yok (EC-002/003/004 dangling ref) | P1 |
| G-09/10 | Validation Generator + Ontology Linter (`formal-checker` agent) hiç yazılmadı | P2 |
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
- `DecisionEntropy` — ENS-3021 formülü (`H(A|C)=I(A;Owner|C)+H(A|C,Owner)`), zincir-kuralı
  matematiksel olarak doğrulandı — **5/5 test geçti**
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

## Sıradaki adım (karar)
**G-02 + G-14 birlikte** — kernel hattının (K1-K5) üstüne oturduğu zemini sağlamlaştırır:
G-02 (ontoloji yönü) düzelmeden yeni Pack'ler (K5) yanlış bağımlılıkla büyür; G-14 (Inference
Rules) ADR-0001'in `indirectly_supports` gibi iddialarının resmî temeli. Skeptic bildirimini
beklerken ikisine paralel başlanabilir (dosya çakışması yok).
