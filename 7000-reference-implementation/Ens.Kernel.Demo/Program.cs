using Ens.Kernel;
using Ens.Kernel.Domain;
using Ens.Kernel.Domain.Events;
using Ens.Kernel.Laws;

// ENS Faz-4 Demo — teori kodun içinde gerçekten çalışıyor mu? Somut bir senaryo:
// "Tedarikçi seçimi" kararının tam yaşam döngüsü + fizik üçlüsü (Entropy/Gravity/Capital)
// + Bounded-Autonomy Gate (P7) tek akışta.

Console.WriteLine("=== ENS Faz-4 Demo — Tedarikçi Seçimi Kararı ===\n");

var owner = Identity.New();

// 1) Framing — Decision henüz atom değil
var decision = DecisionAggregate.Frame(owner, "Ana tedarikçi seçimi (Q3)");
Console.WriteLine($"[Framed] Decision {decision.Id} — Purpose: \"{decision.Purpose}\"");

// 2) Reasoning — Alternatives + Evidence
decision.IdentifyAlternatives(
    owner,
    alternatives: ["Tedarikçi-A", "Tedarikçi-B", "Tedarikçi-C"],
    evidence: ["Fiyat teklifi", "Teslimat performansı (son 12 ay)", "Kalite denetim raporu"]);
Console.WriteLine($"[Alternatives] {string.Join(", ", decision.Alternatives)}");

// 3) Commitment öncesi — Bounded-Autonomy Gate danışılır (ADR-0001 §5.6, P7)
// Stake burada dışarıdan verilir (OL1 açık borç — Alternative-başına ExpectedValue henüz yok)
double stake = 45_000; // TL cinsinden tahmini finansal etki
double preCommitConfidence = 0.55; // henüz commit edilmedi, ön-tahmin
var preCheck = BoundedAutonomyGate.Evaluate(
    stake, preCommitConfidence, conformanceDeficit: 0.3,
    isIrreversible: false, autonomyThreshold: 5_000, blockThreshold: 50_000);
Console.WriteLine($"[Gate — commit öncesi] {preCheck.Decision} (InfoNeed={preCheck.InfoNeed:F0}) — {preCheck.Reason}");

// 4) Commitment — ATOM SINIRI (§Individuation)
decision.Commit(owner, selectedAlternative: "Tedarikçi-B", confidence: 0.8,
    expectedOutcome: "Teslimat gecikmesi %15'ten %5'e düşer");
Console.WriteLine($"[Committed] Confidence={decision.Confidence:F2}");

// 5) Commitment sonrası — Gate artık gerçek Confidence ile yeniden değerlendirir
var postCheck = BoundedAutonomyGate.Evaluate(
    stake, decision.Confidence, conformanceDeficit: 0.3,
    isIrreversible: false, autonomyThreshold: 5_000, blockThreshold: 50_000);
Console.WriteLine($"[Gate — commit sonrası] {postCheck.Decision} (InfoNeed={postCheck.InfoNeed:F0}) — {postCheck.Reason}");

// 6) Enactment + Outcome + Learning
decision.Enact(owner, "Tedarikçi-B ile sözleşme imzalandı");
decision.ObserveOutcome(owner, "Teslimat gecikmesi %15'ten %7'ye düştü (hedefin biraz altında)");
decision.RecordLearning(owner, delta: "Beklenen %5, gerçekleşen %7 — hedef iyimserdi",
    level: AttributionLevel.L1_ModelBased, attributionConfidence: 0.6);
Console.WriteLine($"[Lifecycle] IsCommitted={decision.IsCommitted} IsEnacted={decision.IsEnacted} HasOutcome={decision.HasOutcome}");
Console.WriteLine($"[Event Stream] {decision.History.Count} event: {string.Join(" -> ", decision.History.Select(e => e.GetType().Name))}\n");

// 7) Decision Entropy (ENS-3021) — bu Purpose-tipinde geçmiş 4 karar üzerinden ölçüm
Console.WriteLine("--- Decision Entropy (ENS-3021) — \"Tedarikçi seçimi\" tipi, son 4 karar ---");
var observations = new List<DecisionEntropy.Observation>
{
    new("Q1-benzer-context", "owner-1", "Tedarikçi-B"),
    new("Q1-benzer-context", "owner-2", "Tedarikçi-A"),
    new("Q2-benzer-context", "owner-1", "Tedarikçi-B"),
    new("Q3-benzer-context", "owner-3", "Tedarikçi-B"),
};
double hac = DecisionEntropy.ConditionalEntropy(observations);
double levelNoise = DecisionEntropy.LevelNoise(observations);
double patternNoise = DecisionEntropy.ConditionalEntropyGivenOwner(observations);
Console.WriteLine($"H(A|C) = {hac:F3} bit  (level={levelNoise:F3} + pattern={patternNoise:F3})");
Console.WriteLine(hac < 0.5 ? "-> düşük entropi: örgüt bu Purpose-tipinde tutarlı seçiyor" : "-> yüksek entropi: seçimler dağınık, ortak prior zayıf");

// 8) Decision Capital (ENS-3023) — bu kararın öğreniminin sermayeye katkısı
Console.WriteLine("\n--- Decision Capital (ENS-3023) ---");
double learningValue = DecisionCapital.Value(learningMagnitude: 8.0, attributionConfidence: 0.6);
double roi = DecisionCapital.ReuseROI(infoNeedReduction: 12_000, maintenanceCost: 500);
Console.WriteLine($"value(bu karar) = {learningValue:F2}  (L1 attribution, orta güven)");
Console.WriteLine($"reuse ROI (bir sonraki tedarikçi kararında) = {roi:F1}x");

Console.WriteLine("\n=== Demo tamam — teori (ENS-2001/3021/3022/3023, ADR-0001 §5.6) tek akışta çalıştı ===");
