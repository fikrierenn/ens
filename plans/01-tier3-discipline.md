---
id: PLAN-01
type: plan
status: draft
owner: fikri-eren
version: 0.1.0
created: 2026-07-26
depends_on: [ENS-3022, ENS-2001, ADR-0001, ENS-0000]
---

# PLAN-01 — Tier-3 Disiplini: çalışma katmanı + kernel katmanı

> **Bu planın kendisi bir Tier-3 işidir** (3+ klasör, yeni pattern, mimari karar).
> İçe aktardığımız kural kendi plansız uygulanmasını yasakladığı için önce bu yazıldı.
> Onaysız implement yok.

---

## 1. Problem

`LlmTier.Critical` — kernel'in "Tier 3"ü — **hiçbir yerde tüketilmiyor**. Tek geçtiği yer
onu üreten eşik karşılaştırması (`Adapter/LlmAdapter.cs:143`). Hesaplanıyor,
`ScheduledDecision`'a yapıştırılıyor, sonra hiçbir davranış değişmiyor. Bugün sadece bir
etiket.

Aynı boşluk çalışma katmanında da var: ENS'te `.claude/rules/` **yok**, `plans/` **yok**,
2 skill var. Karşılaştırma için operax'ta 25 rule + 18 skill.

| Katman | Tier 3 ne yapmalı | ENS'te bugün |
|---|---|---|
| Çalışma disiplini | Plan zorunlu, onay kapısı, danışman | Kural yok |
| Kernel | Yükümlülük tetiklemeli | Boş etiket |

**Tetikleyici bulgu (W8d, AUDIT-WAVE2-SECURITY):** `confidence = 1.0` öz-beyanı InfoNeed'i,
tier'ı ve gate'i **birlikte** sıfırlıyor. Yani Tier 3 bir yükümlülük taşısaydı bile, bir
ajan "eminim" diyerek tümünden muaf olurdu. Bu bir routing hatası değil, **kural kaçağıdır**.

---

## 2. Ana içgörü — bu bir kopyala-yapıştır işi DEĞİL

operax'ın 6 Tier-3 sinyali, ENS'in zaten sahip olduğu formülün **sezgisel vekilleridir**:

| operax sinyali (heuristik) | ENS karşılığı (biçimsel) |
|---|---|
| 3+ klasör · kullanıcı-görünür · harici bağımlılık · mimari karar | `Stake` |
| Yeni pattern · yeni dosya tipi | düşük `Confidence` |
| Geri alınması zor (migration, kolon silme) | `IsIrreversible` — **`PendingDecision`'da zaten var** |

Yani operax "3+ dosyaysa plan yaz" diyor; ENS **"InfoNeed yüksekse önce bilgi topla"**
diyebilir — aynı kuralın türetilmiş hâli. ENS-3022'nin `InfoNeed = Stake × (1−Confidence)`
formülü (Howard 1966 VOI) operax'ın merdiveninin biçimsel karşılığıdır.

Aynı şey `llm-council` için de geçerli: Karpathy'nin 5-danışman + peer-review + chairman
sentezi, tetikleyicisi **"yüksek belirsizlik + yüksek maliyet"** — yani birebir
`Stake × (1−Confidence)`. operax bunu insan sürecinde kurmuş; ENS aynı büyüklüğü kernel'de
hesaplıyor ama kullanmıyor.

> **Bu planın teorik değeri:** operax'ın kuralları ENS için **prior art**tır, kaynak değil.
> Kuralları kopyalarsak heuristik ithal etmiş oluruz; türetirsek ENS-3022'nin ilk gerçek
> operasyonel karşılığını üretmiş oluruz.

---

## 3. ENS'te ZATEN olan — tekrar kurmayacağımız

Footprint-ladder'ın kendi anti-pattern'i: *"dış repodan esin diye zaten olanı tekrar
kurmak."* ENS'te operax'ın protokolünün bazı adımları başka adla mevcut:

| operax adımı | ENS'teki karşılığı | Durum |
|---|---|---|
| 3. Kontrol Ettir (bağımsız, öz-onay yok) | **SKR zinciri** + Anayasa **G2/G3** | ✅ Var, güçlü |
| 3. Adversarial geri-götür | `.claude/skills/adversarial-test/` | ✅ Var |
| Kanıt disiplini / DOĞRULANMADI | Madde X + `E0–E4` kanıt seviyeleri | ✅ Var |
| Doküman değişmezliği | EC-001 audit invariant, SKR'ler silinmez | ✅ Var |
| 1. Önce Danış | — | ❌ **YOK** |
| Plan-first / Tier sistemi | — | ❌ **YOK** |
| 4. Smoke test | (fiilen `dotnet test` çalıştırılıyor, kural değil) | ⚠️ Yazısız |
| Footprint ladder | — | ❌ **YOK** |

**Sonuç:** ENS'in eksiği "kontrol" değil — orası zaten operax'tan güçlü. Eksik olan
**üretim öncesi** kapı (danış + plan) ve **kapanış** kanıtı (smoke).

---

## 4. Kapsam

### Dahil
1. `.claude/rules/` katmanı ENS'e kurulur: `plan-first.md`, `footprint-ladder.md`,
   `advisor-skills.md`, `work-protocol.md` — **ENS-3022'ye bağlanarak yeniden yazılır**,
   kopyalanmaz.
2. `plans/` dizini + `feature-template.md`.
3. Danışman kataloğu ENS'in gerçek ajan kadrosuna göre kurulur (`ens-philosopher`,
   `ens-researcher`, `ens-skeptic`, `ens-architect`, `ens-ai-architect`,
   `ens-memory-engine`, `ens-backend-architect`, `ens-test-engineer`).
4. Kernel: `LlmTier.Critical` tüketilir hâle getirilir (aşağıda §6 Faz 2).

### Hariç (bu planın dışı)
- W8d'nin kendisinin kapatılması → 6-mimari-karar hattına ait (görev #39).
- ENS'in mevcut SKR/G2-G3 zincirine dokunmak — çalışıyor, elleşme.
- operax'ın domain kuralları (muhasebe/UI/SQL) — ENS'in domain'i değil.

---

## 5. Reddedilen alternatifler

**A. operax kurallarını olduğu gibi kopyala.**
Reddedildi: heuristik ithal etmek ENS'in kendi formülünü (ENS-3022) atlar. ENS bir
*disiplin* iddiasındadır; kuralları türetemiyorsa iddiası boştur. Ayrıca operax'ın
sinyalleri Razor/SP/migration'a özgü — ENS'te karşılığı yok.

**B. Sadece kernel'i düzelt, çalışma katmanına dokunma.**
Reddedildi: bu oturumun kendisi karşı-kanıt. DEFECT-REGISTER, 6 mimari karar, journal
onarımı — hepsi Tier 3 işti, hiçbiri plansız yapılmamalıydı. Kernel'e kural koyup
kendimizi muaf tutmak, W8d'nin insan sürümüdür.

**C. Hiçbir şey yapma — SKR zinciri yeterli.**
Reddedildi: SKR **üretimden sonra** çalışır. Bu oturumda üç ajan öldü, bir sicil eksik
türetildi (`head -40`), bir yalan iddia (zincir kuralı) aylarca ROADMAP'te durdu. Hepsi
üretim-öncesi kapının yokluğundan. Kontrol katmanı ne kadar güçlü olursa olsun, yanlış
başlayan işi ucuzlatmaz.

---

## 6. Uygulama fazları

### Faz 1 — Çalışma katmanı (düşük risk, geri alınabilir)
1. `plans/` + `feature-template.md`.
2. `.claude/rules/plan-first.md` — Tier eşikleri **ENS-3022'den türetilmiş**:
   - Tier 1: düşük Stake, yüksek Confidence, geri alınabilir → plan yok
   - Tier 2: orta InfoNeed → TODO satırı
   - Tier 3: yüksek InfoNeed **veya** `IsIrreversible` → tam plan
   - **Kaçak kapatma:** Confidence *öz-beyandır*; tek başına Tier düşüremez. Stake ve
     irreversibility gözlemlenebilir; onlar taban belirler. (W8d'nin insan-katmanı karşılığı.)
3. `.claude/rules/footprint-ladder.md` — ENS basamakları: mevcut yapıtı genişlet → yeni
   skill → yeni rule → yeni agent → **yeni ENS-NNNN yapıtı (REGISTRY numarası harcar)** →
   yeni katman/faz. Numaralı yapıt üretmek ENS'te en pahalı basamaktır.
4. `.claude/rules/advisor-skills.md` — ENS ajan kadrosuna göre katalog.
5. `.claude/rules/work-protocol.md` — 4 adım, ama ENS karşılıklarıyla:
   **Danış → Yap → SKR'ye Sok → Kanıtla** (adım 3 = mevcut SKR zinciri, yeniden kurulmaz).

### Faz 2 — Kernel (yüksek risk, ADR gerektirir)
`LlmTier.Critical`'a yükümlülük bağlanır. Ön-tasarım (ADR-0001'e teklif edilecek):
- Critical tier'da gate **`Autonomous` olamaz** (en az `NotifyHuman`).
- Critical tier'da proof-trace öncül eşiği yükselir (bugün ≥1; Critical'da ≥N + kalibre).
- Critical tier'da `Alternatives ≥ 2` zorunlu (ENS-3021: tek alternatifli "karar" karar değil).
- Tier, `ScheduledDecision`'da veri değil **kapı** olur.

**Faz 2 ADR onayı olmadan koda dokunulmaz.** Faz 1 tek başına değerlidir ve Faz 2'yi
bloke etmez.

---

## 7. Riskler

| Risk | Etki | Azaltma |
|---|---|---|
| Kural katmanı bürokrasi olur, iş yavaşlar | Yüksek | Faz 1'de yalnız 4 rule; her rule için "ne zaman UYGULANMAZ" bölümü zorunlu |
| ENS'in mevcut SKR/G2-G3'ü ile çakışır, çift kapı | Orta | §3 tablosu bağlayıcı: kontrol katmanı yeniden kurulmaz, yalnız referans verilir |
| operax heuristikleri sızar, türetme iddiası boşa düşer | Orta | Her Tier eşiği ENS-3022 terimleriyle yazılır; heuristik örnek olarak verilir, ölçüt olarak değil |
| Faz 2 ADR-0001'i şişirir | Orta | Ayrı ADR olarak yazılabilir; karar Faz 1 sonrası |
| Kuralı yazan (ben) kuralı doğrulayan olur — G2/G3 ihlali | **Yüksek** | Faz 1 çıktısı bağımsız `ens-architect` + `ens-skeptic` turuna sokulur |

---

## 8. Done criteria

- [ ] `plans/` + template var, bu plan orada.
- [ ] 4 rule yazıldı, her biri ENS-3022/ENS-2001 referansı taşıyor ve "ne zaman uygulanmaz"
      bölümü içeriyor.
- [ ] Hiçbir rule ENS'in mevcut SKR/G2-G3 kapısını tekrar kurmuyor (§3 tablosuyla denetlendi).
- [ ] Danışman kataloğu ENS'in **gerçekten var olan** ajanlarını listeliyor (uydurma ajan yok).
- [ ] Bağımsız skeptic turu: kurallar ENS-3022'den **türetilmiş mi**, yoksa operax'tan
      kopya mı? Verdict alınmış.
- [ ] Faz 2 için ADR taslağı açıldı (implement edilmedi).

## 9. Rollback

Faz 1 tümüyle yeni dosya — `git rm plans/ .claude/rules/` ile tek commit'te geri alınır,
mevcut hiçbir yapıt değişmez. Faz 2'ye rollback planı ADR'de ayrıca yazılacak (kernel
davranışı değiştiği için testler de döner).

---

## 10. Beş lens

- 🔴 **Contrarian — fatal flaw nerede?** ENS zaten ağır yönetişimli (Anayasa 15 madde,
  SKR zinciri, G-maddeleri, maturity, evidence seviyeleri). Bir kural katmanı daha eklemek
  sistemi *kullanılamaz* yapabilir. Karşı-tedbir: Faz 1'i 4 rule ile sınırlamak ve her
  rule'a "uygulanmaz" bölümü koymak. Yine de gerçek risk bu.
- 🔵 **First Principles — yanlış soruyu mu soruyoruz?** Asıl soru "kural mı eksik" değil,
  *"neden bu oturumda üç ajan öldü ve bir sicil eksik türetildi?"* Cevap kural eksikliği
  değil de araç kırılganlığıysa, rule yazmak semptomu tedavi eder. **Bu plan her iki
  cevabı da kapsamıyor — Faz 1 sonrası ölçülmeli.**
- 🟢 **Expansionist — daha büyük fırsat kaçıyor mu?** Evet: eğer Tier eşikleri gerçekten
  ENS-3022'den türetilirse, ENS'in *kendi çalışma disiplini* teorinin ilk canlı
  uygulaması olur — yani ENS kendi kendini yönetir hâle gelir. Bu, kitap için (Faz 6)
  en güçlü kanıt olurdu: teori kendi geliştirme sürecinde çalışıyor.
- ⚪ **Outsider — yabancı ne garip bulurdu?** "Şirketleri karar üreten bilişsel sistem
  olarak modelleyen bir proje, kendi kararlarını kayıtsız veriyor." Bugüne kadar tek bir
  plan dosyası yok, ama 45 SKR var. Denetim güçlü, niyet kayıtsız.
- 🟡 **Executor — pazartesi sabahı ne yapılır?** `plans/` aç, bu dosyayı koy, 4 rule'u
  yaz (yarım gün), bağımsız skeptic turuna sok. Faz 2'ye dokunma.

---

## 11. Açık sorular (onay öncesi cevaplanmalı)

1. Faz 2 ADR-0001'e mi girer, yoksa ayrı bir ADR-0003 mü olur?
2. `advisor-skills` kataloğu ENS'te zorunlu mu olsun, yoksa öneri mi? (operax'ta
   "danışılmadan yazılan iş **eksik sayılır**" — ENS'te bu kadar sert olmalı mı?)
3. Tier eşiklerinin sayısal değerleri nereden gelecek? ENS-3022'nin kendi kalibrasyon
   borcu açık — eşikler de aynı borcu miras alır. Kabul mü, yoksa Faz 1'de nitel
   (yüksek/orta/düşük) mi kalsın?
