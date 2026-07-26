namespace Ens.Kernel.Domain.Events;

// TRACE: ENS-2001 §Decision Lifecycle (Framing → Contextualization → Reasoning → Commitment →
//        Enactment → Measurement → Learning → Memory)
// TRACE: ADR-0001 §5.4 (granülerlik ölçütü) — yalnızca DecisionCommitted atomu mühürler;
//        öncesi (Framed/AlternativesIdentified) deliberation'dır, ayrı atom değildir.

/// <summary>Framing fazı: Purpose belirir. Henüz commitment değil (ENS-2001 §Individuation).</summary>
public sealed record DecisionFramed(string Purpose) : DomainEvent;

/// <summary>
/// Contextualization/Reasoning fazı: Alternatives + Evidence toplanır.
///
/// TRACE: ENS-4025 §L8 — `Evidence` artık <see cref="Premise"/> listesidir (kaynak + confidence),
/// düz string listesi DEĞİL. Gerekçe (AUDIT-WAVE2-FIDELITY §D-1): commitment'ın proof-trace'i
/// öncüllerini buradan alır ve ENS-4025 §L7 (`conf = min(öncüller)`) bir confidence ALANI
/// olmadan hesaplanamaz. ADR-0001 §5.5'in kendi örneği de öncülleri confidence'lı gösterir
/// (`conf = min(plan=0.8, context=0.7)`). Şema değişikliği bilinçlidir ve dürüstçe kayıtlıdır:
/// kalıcı bir event-store OLMADIĞI için upcaster yazılmadı; gerçek bir store'da gerekirdi.
/// </summary>
public sealed record AlternativesIdentified(
    IReadOnlyList<string> Alternatives,
    IReadOnlyList<Premise> Evidence) : DomainEvent;

/// <summary>
/// ATOM SINIRI. ENS-2001 §Individuation dört koşulu mühürler: tek Owner, tek Purpose,
/// açık Alternatives (yukarıda), tek Commitment olayı (bu event).
/// TRACE: ADR-0001 §5.6 Bounded-Autonomy Gate — bu event Policy (ens-core:Constraint bundle,
///        TOVE Empowerment) tarafından yetkilendirilmiş olmalı; Gate kontrolü çağıran katmanın
///        sorumluluğudur (aggregate yalnızca invariant'ı korur, yetkilendirmeyi yapmaz).
/// TRACE: ENS-4025 §L8 — <see cref="RuleId"/>: "hangi KURAL bu olguyu türetti" sorusunun cevabı.
///        Öncüller olayda TEKRARLANMAZ; `AlternativesIdentified.Evidence`'tan gelirler ve
///        proof-trace akışın fold'u olarak HESAPLANIR (ENS-4001 Axiom 2 — Computational Closure).
///        Böylece iz, olayın yanına iliştirilmiş denormalize bir kopya değil, akışın kendisinin
///        bir fonksiyonudur; iki kaynak arasında ayrışma (drift) YAPISAL OLARAK imkânsızdır.
/// </summary>
public sealed record DecisionCommitted(
    string SelectedAlternative,
    Identity Owner,
    double Confidence,
    string ExpectedOutcome,
    string RuleId = DecisionCommitted.DefaultRuleId) : DomainEvent
{
    /// <summary>
    /// Varsayılan türetim kuralı: commitment'ı doğuran kural, ENS-2001 §Individuation'ın
    /// commitment mühürüdür. Bir Planner (ADR-0001 §5.2) kendi kural kimliğini geçebilir.
    /// </summary>
    public const string DefaultRuleId = "ENS-2001§Commitment";
}

/// <summary>
/// Enactment fazı — ADR-0001 §4 traceability: Actuation Layer, ENS-2001'in Enactment
/// aşamasını realizes eder (SKR-029: bu aşama teoride hâlâ "zayıf-teorize", failure condition).
/// </summary>
public sealed record DecisionEnacted(string ActionDescription) : DomainEvent;

/// <summary>Measurement fazı (P4). Actual Outcome — Learning'in girdisi (ENS-2004 §1).</summary>
public sealed record OutcomeObserved(string ActualOutcome) : DomainEvent;

/// <summary>
/// Learning fazı: learning_signal = Actual − Expected (ENS-2004 §1). Attribution seviyesi
/// (L0-L3, ENS-2004 §3) ve confidence dereceli tutulur — sahte kesinlik yok.
/// </summary>
public sealed record LearningRecorded(
    string Delta,
    AttributionLevel Level,
    double AttributionConfidence) : DomainEvent;

/// <summary>TRACE: ENS-2004 §3 Attribution merdiveni.</summary>
public enum AttributionLevel { L0_NoAttribution, L1_ModelBased, L2_QuasiExperimental, L3_Experimental }
