using Ens.Kernel;
using Ens.Kernel.Adapter;
using Ens.Kernel.Capability;
using Ens.Kernel.Domain;
using Ens.Kernel.Domain.Events;
using Ens.Kernel.Laws;

// ENS Faz-4 Demo — Cognitive Kernel'in ALTI bileşeni tek akışta (ADR-0001 §5.2 diyagramı).
// Senaryo: üç bekleyen karar yarışıyor, kıt dikkat (P5) tahsis ediliyor, biri uçtan uca koşuyor.
// Bu bir "her şey yolunda" gösterisi DEĞİL — kernel'in NEYİ REDDETTİĞİ de gösteriliyor.

Console.OutputEncoding = System.Text.Encoding.UTF8;
var now = new DateTimeOffset(2026, 7, 25, 9, 0, 0, TimeSpan.Zero);

void H(string title) => Console.WriteLine($"\n{new string('═', 78)}\n  {title}\n{new string('═', 78)}");

// ─────────────────────────────────────────────────────────────────────────────
H("1. CAPABILITY REGISTRY (§6) — kernel değişmeden yeti takılıyor");

var registry = new CapabilityRegistry();
registry.Register(new CapabilityPack("Operations", "1.2",
    allowedTools: ["read_stock", "create_purchase_order", "cancel_contract"],
    requiresHumanApprovalFor: ["create_purchase_order", "cancel_contract"]));
registry.Register(new CapabilityPack("Reporting", "0.9",
    allowedTools: ["read_stock", "render_report"]));

foreach (var p in registry.EnabledPacks)
    Console.WriteLine($"  ✔ Pack: {p.Name} v{p.Version} — {p.AllowedTools.Count} araç, " +
                      $"{p.RequiresHumanApprovalFor.Count} tanesi insan onaylı");

Console.WriteLine("\n  Yetkilendirme kararları (deklaratif izinler → P7 gate'e sinyal):");
foreach (var tool in new[] { "read_stock", "create_purchase_order", "delete_database" })
{
    var a = registry.Authorize(tool);
    var mark = !a.IsAllowed ? "✖ RED" : a.RequiresHumanApproval ? "⚠ ONAY" : "✔ SERBEST";
    Console.WriteLine($"    {mark,-10} {tool,-24} {a.Reason}");
}

// ─────────────────────────────────────────────────────────────────────────────
H("2. SCHEDULER (§5.3) — kıt dikkat (P5), Decision Gravity'ye göre tahsis");

var supplierSwitch = Identity.New();
var pending = new[]
{
    new PendingDecision(Identity.New(), Stake: 800, Confidence: 0.92, ConformanceDeficit: 0.10),
    new PendingDecision(supplierSwitch, Stake: 45_000, Confidence: 0.55, ConformanceDeficit: 0.70),
    new PendingDecision(Identity.New(), Stake: 250_000, Confidence: 0.80, ConformanceDeficit: 0.40, IsIrreversible: true)
};

// Eşikler bu senaryonun TL-ölçeğine göre verildi — kalibre edilmiş değerler DEĞİL (aşağıda).
const double ComplexTier = 5_000, CriticalTier = 40_000;
var scheduled = Scheduler.Schedule(pending, autonomyThreshold: 5_000, blockThreshold: 60_000,
    complexThreshold: ComplexTier, criticalThreshold: CriticalTier);

Console.WriteLine("  Sıra  Stake      Conf   Açık   InfoNeed   Öncelik    Model tier    Gate");
Console.WriteLine("  " + new string('─', 74));
for (int i = 0; i < scheduled.Count; i++)
{
    var s = scheduled[i];
    var d = s.Decision;
    Console.WriteLine($"  #{i + 1}    {d.Stake,-10:N0} {d.Confidence,-6:F2} {d.ConformanceDeficit,-6:F2} " +
                      $"{s.Gate.InfoNeed,-10:N0} {s.AttentionPriority,-10:N0} {s.Tier,-13} {s.Gate.Decision}");
}
Console.WriteLine("\n  ⇒ Sıralama ölçütü keyfi DEĞİL: AttentionPriority = InfoNeed × ConformanceDeficit");
Console.WriteLine("    (ENS-3022 §Model 3 — Howard 1966 VOI'nin sürekli örgütsel biçimi)");
Console.WriteLine("  ⇒ İki kaynak AYRI tahsis edildi: hesaplama (model tier) + insan dikkati (gate)");

// ─────────────────────────────────────────────────────────────────────────────
H("3. PLANNER (§5.2/ENS-2001) — commitment-sealed karar atomu");

var owner = Identity.New();
var decision = DecisionAggregate.Frame(owner, "Ana tedarikçi değişimi (Q3)");
// Evidence = öncül kümesi (kaynak + confidence). ENS-4025 L8: commitment'ın proof-trace'i
// öncüllerini BURADAN alır; boş bırakılamaz (D-1 düzeltmesi).
decision.IdentifyAlternatives(owner,
    alternatives: ["Tedarikçi-A (mevcut)", "Tedarikçi-B", "Tedarikçi-C"],
    evidence:
    [
        new Premise("Teslimat performansı 12 ay", 0.85),
        new Premise("Kalite denetim raporu", 0.70),
        new Premise("Fiyat teklifi", 0.90)
    ]);
decision.Commit(owner, "Tedarikçi-B", confidence: 0.55, expectedOutcome: "Gecikme %15→%5");

Console.WriteLine($"  Purpose      : {decision.Purpose}");
Console.WriteLine($"  Alternatives : {string.Join(" | ", decision.Alternatives)}");
Console.WriteLine($"  Commitment   : Tedarikçi-B @ conf={decision.Confidence:F2}");
Console.WriteLine($"  Event akışı  : {string.Join(" → ", decision.History.Select(e => e.GetType().Name))}");
Console.WriteLine("\n  Commitment'ın proof-trace'i (ENS-4025 L8 — izsiz commitment YOK):");
Console.WriteLine("    " + decision.CommitmentTrace!.Render().Replace("\n", "\n    "));
Console.WriteLine($"    L7 t-norm: min(öncüller) = {decision.CommitmentTrace.Confidence:F2} " +
                  $"≥ commitment confidence {decision.Confidence:F2} ✓");
Console.WriteLine("\n  ⇒ Atom sınırı MÜHÜRLENDİ (ENS-2001 §Individuation): tek Owner, tek Purpose,");
Console.WriteLine("    açık Alternatives, tek Commitment. İkinci commit denenirse:");
try { decision.Commit(owner, "Tedarikçi-C", 0.9, "..."); }
catch (InvalidOperationException ex) { Console.WriteLine($"    ✖ {ex.Message}"); }

Console.WriteLine("\n  Öncülsüz (Evidence'sız) bir commitment denenirse (ENS-4025 L8 / ADR-0001 §5.5):");
var traceless = DecisionAggregate.Frame(owner, "İzsiz karar denemesi");
try { traceless.IdentifyAlternatives(owner, ["A", "B"], []); }
catch (ArgumentException ex) { Console.WriteLine($"    ✖ {ex.Message.Split('(')[0].Trim()}"); }

Console.WriteLine("  Öncüllerin desteklemediği bir güven iddiası denenirse (ENS-4025 L7):");
var overconfident = DecisionAggregate.Frame(owner, "Aşırı-güven denemesi");
overconfident.IdentifyAlternatives(owner, ["A", "B"], [new Premise("zayıf-kanıt", 0.30)]);
try { overconfident.Commit(owner, "A", 0.95, "harika sonuç"); }
catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"    ✖ {ex.Message.Split('(')[0].Trim()} …"); }

// ─────────────────────────────────────────────────────────────────────────────
H("4. ACTUATION LAYER (§5.4) — guarded lifecycle, atlanamaz geçişler");

var action = new ActuationLayer(supplierSwitch);
var chosen = scheduled.First(s => s.Decision.DecisionId.Equals(supplierSwitch));

Console.WriteLine($"  Başlangıç durumu: {action.State}");
Console.WriteLine("\n  Gate'i ATLAMAYI denersek (P7 yapısal savunma):");
try { action.BeginActing(now); }
catch (InvalidTransitionException ex) { Console.WriteLine($"    ✖ {ex.Message}"); }

Console.WriteLine($"\n  Capability yetkisi: create_purchase_order → {registry.Authorize("create_purchase_order").Reason}");
action.ApplyGate(chosen.Gate, now);
Console.WriteLine($"  Gate uygulandı → {action.State}  ({chosen.Gate.Decision}: {chosen.Gate.Reason})");

action.BeginActing(now.AddMinutes(5));
action.Observe(now.AddDays(30), "Gecikme %15→%7 (hedefin altında)");
Console.WriteLine($"  Guarded action koştu, sonuç gözlendi → {action.State}");

Console.WriteLine("\n  Proof-trace'i ATLAMAYI denersek (Anayasa Madde VI):");
try { action.RecordLearning(now.AddDays(30)); }
catch (InvalidTransitionException ex) { Console.WriteLine($"    ✖ {ex.Message}"); }

// ─────────────────────────────────────────────────────────────────────────────
H("5. PROOF-TRACE (§5.5 / ENS-4025 L7-L8) — izsiz türetim TEMSİL EDİLEMEZ");

Console.WriteLine("  Öncülsüz trace kurmayı denersek:");
try { _ = new ProofTrace("BAG-P3", "Action enacted", []); }
catch (ArgumentException ex) { Console.WriteLine($"    ✖ {ex.Message.Split('(')[0].Trim()}"); }

var trace = new ProofTrace(
    ruleId: "BAG-P3 + Scheduler/ENS-3022",
    conclusion: "Action-supplier-switch enacted",
    premises:
    [
        new Premise("plan(Tedarikçi-B)", 0.80),
        new Premise("context(12-ay teslimat verisi)", 0.70),
        new Premise("memory-precedent(2024 tedarikçi krizi)", 0.90)
    ]);

Console.WriteLine("\n" + string.Join("\n", trace.Render().Split('\n').Select(l => "  " + l)));
Console.WriteLine($"\n  ⇒ Confidence ATANMADI, min t-norm ile HESAPLANDI (L7): min(0.80,0.70,0.90) = {trace.Confidence:F2}");

action.RecordTrace(trace, now.AddDays(30));
Console.WriteLine($"  → {action.State}");

// ─────────────────────────────────────────────────────────────────────────────
H("6. LEARNING + MEMORY (ENS-2004 / ENS-2003) — döngü kapanıyor");

decision.Enact(owner, "Tedarikçi-B sözleşmesi imzalandı");
decision.ObserveOutcome(owner, "Gecikme %15→%7");
decision.RecordLearning(owner, "Beklenen %5, gerçekleşen %7 — hedef iyimserdi",
    AttributionLevel.L1_ModelBased, attributionConfidence: 0.60);

action.RecordLearning(now.AddDays(30), "Δ = -2 puan (hedef altı)");

var memory = new CompanyMemory();
memory.Record(new MemoryRecord(supplierSwitch, "tedarikçi-seçimi",
    LearningMagnitude: 8.0, AttributionConfidence: 0.60, AssertedAt: now.AddDays(30)));
memory.Record(new MemoryRecord(Identity.New(), "tedarikçi-seçimi",
    LearningMagnitude: 2.0, AttributionConfidence: 0.85, AssertedAt: now.AddDays(-400)));

action.Remember(now.AddDays(30));
Console.WriteLine($"  Lifecycle tamamlandı → {action.State} (terminal: {action.IsTerminal})");
Console.WriteLine($"  Audit geçmişi: {action.History.Count} geçiş, hiçbiri atlanamadı");
Console.WriteLine("    " + string.Join(" → ", action.History.Select(h => h.To)));

var asOf = now.AddDays(60);
// ENS-2003 v0.4.0 §3a: sönüm hızı artık Purpose-tipinin context yarı-ömrüne bağlı (λ_π = ln2/τ_π),
// attribution confidence'a DEĞİL. τ_π ≈ 69 gün ⇒ λ_π ≈ 0.01/gün. (τ_π kalibre EDİLMEMİŞTİR —
// ontoloji bu alanı henüz taşımıyor; buradaki değer bir GÖSTERİ değeridir, ölçüm değil.)
const double contextDecayRate = 0.01;
Console.WriteLine($"\n  Memory retrieval (t+60g, context-koşullu decay λ_π=ln2/τ_π, τ_π≈{Math.Log(2) / contextDecayRate:F0} gün):");
foreach (var r in memory.Retrieve("tedarikçi-seçimi", asOf, contextDecayRate))
{
    double sal = memory.Salience(r, asOf, contextDecayRate);
    double fresh = memory.DecayFactor(r, asOf, contextDecayRate);
    Console.WriteLine($"    |Learning|={r.LearningMagnitude,-4} conf={r.AttributionConfidence:F2}  " +
                      $"retention={r.RetentionPriority,-5:F2} value={r.CapitalValue,-5:F2} " +
                      $"tazelik={fresh:P0}  salience={sal:F2}");
}
Console.WriteLine("  ⇒ Üç ayrı sütun, üç ayrı soru (ENS-2003 v0.4.0 §3):");
Console.WriteLine("     retention = ne KAYBOLMAMALI (yalnızca |Learning|; confidence'tan bağımsız)");
Console.WriteLine("     value     = yeni kararı ne kadar AĞIRLIKLA yönlendirmeli (|Learning|·conf)");
Console.WriteLine("     tazelik   = bağlamı hâlâ geçerli mi (yalnızca zaman × context volatilitesi)");
Console.WriteLine("    v0.3'te confidence hem value'da hem tazelikte sayılıyordu — çift-sayım (AUDIT-WAVE2/D-5).");

// Karşı-survivorship TABANI: kesme yapıldığında büyük ders elenemez (ENS-2003 v0.4.0 §3).
var top1 = memory.RetrieveTop("tedarikçi-seçimi", limit: 1, asOf, contextDecayRate);
Console.WriteLine($"\n  Kesme (k=1) → |Learning|={top1[0].LearningMagnitude} kaydı kaldı: karşı-survivorship tabanı");
Console.WriteLine("    (taban `argmax |Learning|`'i garanti eder — attribution zayıf olsa bile elenemez).");

var stale = memory.FindStale(asOf, contextDecayRate, staleThreshold: 0.5);
Console.WriteLine($"\n  Curator sweep — tazelik ekseni: {stale.Count} bayat kayıt BAYRAKLANDI (silinmedi — §3b, P7)");
var weak = memory.FindWeaklyAttributed(minConfidence: 0.7);
Console.WriteLine($"  Curator sweep — epistemik eksen: {weak.Count} kayıt 'attribution zayıf' (öneri: L1→L2, ENS-2004 §4a)");

// ─────────────────────────────────────────────────────────────────────────────
H("7. REFLECTIVE DOUBLE-LOOP (ENS-2004 §4a) — öneri üretir, ASLA uygulamaz");

var manyRecords = Enumerable.Range(0, 4)
    .Select(i => new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 7.5, 0.6, now.AddDays(-i * 30)))
    .ToList();

foreach (var p in ReflectiveDoubleLoop.Propose(manyRecords))
    Console.WriteLine($"  ⚑ ÖNERİ: {p.Description}\n    (destek: {p.SupportingRecordCount} kayıt, " +
                      $"ort. attribution conf={p.AverageAttributionConfidence:F2})");

var mutators = typeof(ReflectiveDoubleLoop)
    .GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.DeclaredOnly)
    .Select(m => m.Name).ToArray();
Console.WriteLine($"\n  ⇒ Sınıfın TÜM public metotları: [{string.Join(", ", mutators)}]");
Console.WriteLine("    Apply/Commit/Update YOK — P7 kapısı YAPISAL: öneri uygulanamaz, çünkü");
Console.WriteLine("    uygulayacak metot yok. İnsan onayı mimari olarak zorunlu.");

// ─────────────────────────────────────────────────────────────────────────────
H("8. LLM ADAPTER PORT (§7) — model-agnostik, tier VOI'den türer");

Console.WriteLine("  InfoNeed → model tier eşlemesi (§5.3: 'AIOS'ta boş; ENS onu VOI'ye bağlar'):");
foreach (var (stake, conf, label) in new[]
         {
             (500.0, 0.95, "rutin stok sorgusu"),
             (45_000.0, 0.55, "tedarikçi değişimi"),
             (250_000.0, 0.30, "fabrika kapatma")
         })
{
    var infoNeed = DecisionGravity.InfoNeed(stake, conf);
    var tier = LlmTierSelector.SelectTier(stake, conf, ComplexTier, CriticalTier);
    var naive = LlmTierSelector.SelectTier(stake, conf); // varsayılan eşikler (10/40)
    Console.WriteLine($"    {label,-24} InfoNeed={infoNeed,-9:N0} → {tier,-12} (varsayılan eşiklerle: {naive})");
}
Console.WriteLine("\n  ⇒ Token maliyeti dikkat-önceliğiyle hizalı (P5): ucuz karar → hafif model.");
Console.WriteLine("  ⇒ Hiçbir somut sağlayıcı bağlı DEĞİL (saf port) — Cerebras/DeepInfra/yerel");
Console.WriteLine("    model bu portun arkasına takılır, kernel kodu değişmez.");
Console.WriteLine("\n  ⚠ DÜRÜST BULGU (bu demo ortaya çıkardı): varsayılan eşikler (10/40) TL-ölçekli");
Console.WriteLine("    stake'lerde HER kararı Critical'a atıyor — sağ sütun. Yani 'ucuz karar → hafif");
Console.WriteLine("    model' vaadi kalibrasyon olmadan ÇALIŞMIYOR. ENS-3022 §Model 1 zaten Stake'in");
Console.WriteLine("    Purpose-tipi içinde normalize edilmesini (z-skoru/persentil) şart koşuyor;");
Console.WriteLine("    o normalizasyon henüz kodlanmadı — README'de işaretli kalibrasyon borcu.");

// ─────────────────────────────────────────────────────────────────────────────
H("ÖZET — kernel neyi GARANTİ ediyor");

Console.WriteLine("""
  Bu akışta kernel dört şeyi YAPISAL olarak imkânsız kıldı (politika değil, tip sistemi):

    1. Gate'siz action        → InvalidTransitionException (P7)
    2. İzsiz action           → Proof-trace atlanamaz, öncülsüz trace kurulamaz (Madde VI)
    3. İzsiz başarısızlık     → Failed bile Traced'ten geçmek zorunda
    4. Otomatik model-revizyonu → ReflectiveDoubleLoop'ta Apply metodu YOK (P7)

  Ve üç şeyi TEORİDEN türetti (keyfi sabit değil):

    • Dikkat sırası    ← ENS-3022 AttentionPriority = InfoNeed × ConformanceDeficit
    • Model tier       ← aynı InfoNeed (Howard 1966 VOI)
    • Bellek sırası    ← ENS-2003 retention ∝ |Learning|, decay ∝ (1−confidence)^γ

  Kaybedilen hiçbir şey silinmiyor: bayat kayıt bayraklanır, karar geçmişi event akışıdır,
  başarısızlık iz bırakır.
""");
