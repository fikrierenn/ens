using System.Collections.ObjectModel;
using Ens.Kernel.Domain.Events;

namespace Ens.Kernel.Domain;

// TRACE: ENS-2001 (Decision Theory) — Decision Object + §Individuation + Lifecycle
// TRACE: ADR-0001 §5.4 (Action = Decision atomu, commitment-sealed granülerlik)
// TRACE: ENS-4025 §L8 + §L7 (proof-trace invariant — izsiz commitment yasak, Anayasa Madde VI);
//        ADR-0001 (accepted, v0.3.1) §5.5 "proof-trace action'ın var-olma koşuludur" — bu iddia
//        artık `Commit`/`Rehydrate` içinde ZORLANIR (D-1 düzeltmesi, aşağıdaki blok).
// TRACE: AUDIT.md §5.3 — "Rehydrate §Individuation mührünü ATLIYOR" + §5.2 (audit silinebiliyor).
// TRACE: AUDIT-WAVE2-FIDELITY.md §D-1 — "izsiz commitment yasak" DENİYOR, izsiz commitment SERBESTTİ.
//
// Bu, ENS'in ilk çalışan Decision atomudur (Faz 4 reference implementation).
// Event-sourced: durum yalnızca Event akışının fold'udur (ENS-4001 §Axiom 2, Computational
// Closure). Aggregate kendi invariant'larını (§Individuation) korur; Bounded-Autonomy Gate
// (Policy/yetkilendirme) çağıran katmanın sorumluluğudur — aggregate onu varsaymaz, yalnızca
// commitment sırasını (Framed → Alternatives → Committed) zorlar.
//
// ============================ DENETİM SONRASI ÜÇ DÜZELTME (AUDIT.md §5.2/§5.3) ============================
// (1) REPLAY YOLU ARTIK MÜHÜRLÜ. `Rehydrate` olayları HİÇBİR doğrulama yapmadan uyguluyordu:
//     Purpose'suz, Alternative'siz, İKİ commitment'lı bir "karar" replay ile üretilebiliyordu.
//     Event-sourced bir sistemde replay yolu BİRİNCİL yoldur; invariant'ları yalnızca yazma
//     yolunda korumak yetersizdir. Artık her replay olayı, canlı yolla AYNI §Individuation
//     invariant'larından geçer (`EnsureReplayInvariant`).
// (2) ALTERNATIVES ARTIK KOPYALANIYOR. `IdentifyAlternatives` gelen listeyi kopyalamıyordu;
//     event ve aggregate aynı canlı `List<string>`'i paylaşıyordu, çağıran sonradan bir
//     alternatif ekleyip HİÇ DEĞERLENDİRİLMEMİŞ bir seçeneğe commit edebiliyordu.
// (3) OLAY GEÇMİŞİ ARTIK SİLİNEMİYOR. `History`/`UncommittedEvents` canlı `List<DomainEvent>`
//     döndürüyordu; `((List<DomainEvent>)d.History).Clear()` ile "karar bir olay geçmişidir"
//     iddiası tek satırda çürütülebiliyordu.
// ========================================================================================================
//
// ============ DENETİM DALGA-2 DÜZELTMESİ — D-1: "İZSİZ COMMITMENT" ARTIK GERÇEKTEN YASAK ============
// AUDIT-WAVE2-FIDELITY.md §D-1 haklıydı ve bulgusu ağırdı: bu dosya satır başında "proof-trace
// invariant — izsiz commitment yasak" DİYORDU ama kod bunu HİÇBİR YERDE zorlamıyordu:
//   - `evidence` yalnızca null-check'ten geçiyordu (boş liste kabul),
//   - `Apply` Evidence'ı aggregate'e hiç yazmıyordu,
//   - `Commit` Evidence'a hiç bakmıyordu,
//   - `DecisionCommitted` ne kural kimliği ne öncül taşıyordu,
//   - `ProofTrace` bu dosyada HİÇ kullanılmıyordu.
// Yani `IdentifyAlternatives(who, ["A","B"], evidence: [])` → `Commit(...)` SIFIR-ÖNCÜLLÜ bir
// commitment üretiyordu. İddia ile kod ayrıydı.
//
// SEÇİLEN YOL: (a) — TRACE'i düşürmek değil, KODU İDDİAYA YÜKSELTMEK. Gerekçe:
//   • ADR-0001 **Accepted** (v0.3.1) §5.5: *"İzsiz action yoktur… Proof-trace opsiyon değil,
//     aksiyomdur… ENS'te proof-trace action'ın VAR-OLMA KOŞULUDUR."*
//   • ADR-0001 §5.4: *"proof-trace (P6/L8) **atom düzeyinde** zorunludur"* — ve bu dosyadaki
//     atom, tam olarak `DecisionCommitted`'dır.
// Dolayısıyla bağ, kabul edilmiş bir ADR'nin GEREĞİdir; yeni mimari icadı DEĞİLDİR (Madde IX/VIII).
// İddiayı düşürmek (yol b) Accepted bir ADR'yi kodda uygulamamak anlamına gelirdi.
//
// ARTIK ZORLANANLAR (üçü de hem CANLI hem REPLAY yolunda):
//   (D1-1) Evidence BOŞ OLAMAZ. Öncülsüz deliberation, öncülsüz commitment demektir (L8).
//   (D1-2) `Commit` bir `ProofTrace` ÜRETİR (`CommitmentTrace`): RuleId + Evidence öncülleri +
//          L7 min-t-norm confidence'ı. Commitment'ın var-olma koşulu artık trace'in KURULABİLİR
//          olmasıdır — `ProofTrace` ctor'u öncülsüz kurulmayı zaten reddediyor.
//   (D1-3) L7 KISITI: commitment confidence'ı, öncüllerinin t-norm'unu AŞAMAZ.
//          ENS-4025 L7 `conf(sonuç) = min(conf(öncüller))` der; burada `≤` zorlanır (aşağıda
//          "L7 SAPMASI" başlığında dürüstçe gerekçelendirildi).
//
// L7 SAPMASI (dürüst kayıt): teori `=`, kod `≤`. Neden: ENS-2001'de commitment confidence'ı
// yalnızca öncüllerin fonksiyonu değil, karar vericinin kendi kalibre öz-değerlendirmesidir (P6);
// `=` dayatmak `confidence` parametresini anlamsız kılardı. `≤` L7'nin MUHAFAZAKÂR yönüdür:
// öncüllerin desteklemediği bir güven iddia EDİLEMEZ, ama daha temkinli olmak serbesttir.
// Bilinen sınır (`AUDIT_FINDING_W2_P5`): `min` idempotenttir — korroborasyon (çok sayıda bağımsız
// öncül) confidence'ı ARTIRMAZ, bu yüzden `≤` kuralı korroborasyonu ödüllendirmez. t-norm seçimi
// ENS-4025 §Failure'da zaten açık bir RFC borcudur; bu kural o RFC ile yeniden ele alınmalıdır.
//
// HÂLÂ AÇIK (bu düzeltmenin KAPATMADIĞI, iddia edilmemesi gerekenler):
//   • Öncüller hâlâ serbest metin + confidence çiftidir; ENS-4010 Context/Evidence/Memory
//     NODE'una tipli referans YOK (ProofTrace.cs "(b)" borcu aynen duruyor).
//   • Öncül confidence'ı KALİBRE değildir; çağıran ne derse odur (Guard ölçülebilirliği kapatır,
//     kalibrasyonu değil).
//   • Döngüsel öncül (`X ⊢ X`) hâlâ temsil edilebilir (`AUDIT_DEFECT_W2_P4` açık).
//   • `Enact`/`ObserveOutcome`/`RecordLearning` KENDİ trace'lerini üretmez — L8 bu artımda
//     yalnızca ATOM SINIRINDA (commitment) zorlanır; ADR-0001 §5.4'ün izin verdiği kapsam budur.
// ====================================================================================================
public sealed class DecisionAggregate
{
    private readonly List<DomainEvent> _uncommitted = [];
    private readonly List<DomainEvent> _history = [];
    private readonly ReadOnlyCollection<DomainEvent> _uncommittedView;
    private readonly ReadOnlyCollection<DomainEvent> _historyView;

    public Identity Id { get; private set; }
    public string? Purpose { get; private set; }
    public IReadOnlyList<string> Alternatives { get; private set; } = ReadOnlyCollection<string>.Empty;

    /// <summary>
    /// Deliberation'ın öncül kümesi (ENS-2002 §Implications: "Evidence, context'in commit-edilmiş
    /// alt kümesidir", P6). D-1 öncesinde bu alan aggregate'te HİÇ YOKTU: event'e yazılıyor, sonra
    /// unutuluyordu. Artık commitment'ın proof-trace'inin öncülleri tam olarak budur.
    /// </summary>
    public IReadOnlyList<Premise> Evidence { get; private set; } = ReadOnlyCollection<Premise>.Empty;

    /// <summary>
    /// ENS-4025 §L8 — commitment'ın proof-trace'i. `null` ⇔ henüz commit edilmemiş.
    /// Commit edilmiş bir aggregate'te bu alan ASLA `null` olamaz (invariant): iz, commitment'ın
    /// var-olma koşuludur (ADR-0001 §5.5). Değer HESAPLANIR (olay akışının fold'u), atanamaz.
    /// </summary>
    public ProofTrace? CommitmentTrace { get; private set; }

    public double? Confidence { get; private set; }
    public bool IsCommitted { get; private set; }
    public bool IsEnacted { get; private set; }
    public bool HasOutcome { get; private set; }

    private DecisionAggregate(Identity id)
    {
        Id = id;
        _uncommittedView = new ReadOnlyCollection<DomainEvent>(_uncommitted);
        _historyView = new ReadOnlyCollection<DomainEvent>(_history);
    }

    /// <summary>Yeni bir Decision başlatır — Framing fazı. Henüz atom değil (deliberation).</summary>
    public static DecisionAggregate Frame(Identity emitter, string purpose)
    {
        if (string.IsNullOrWhiteSpace(purpose))
            throw new ArgumentException("Purpose boş olamaz — amaçsız karar atom olamaz (ENS-2001 §Individuation).", nameof(purpose));

        var decision = new DecisionAggregate(Identity.New());
        decision.Raise(new DecisionFramed(purpose) { Emitter = emitter, Target = decision.Id });
        return decision;
    }

    /// <summary>
    /// Reasoning fazı: Alternatives + Evidence toplanır. Commitment için ön-koşul.
    ///
    /// D-1: `evidence` artık BOŞ OLAMAZ ve `Premise` (kaynak + confidence) taşır — commitment'ın
    /// proof-trace'i öncüllerini buradan alır (ENS-4025 L8/L7).
    /// </summary>
    public void IdentifyAlternatives(Identity emitter, IReadOnlyList<string> alternatives, IReadOnlyList<Premise> evidence)
    {
        ArgumentNullException.ThrowIfNull(alternatives);
        ArgumentNullException.ThrowIfNull(evidence);

        if (IsCommitted)
            throw new InvalidOperationException("Commit-edilmiş bir Decision'a yeni Alternative eklenemez (ENS-2001 §Individuation: tek Commitment olayı).");

        // AUDIT §5.3: SNAPSHOT. Çağıranın listesi bundan sonra değişse bile deliberation kümesi
        // değişmez — "açık Alternatives" mührü ancak kopya ile gerçek olur. Aynı gerekçe Evidence
        // için de geçerli: öncül kümesi commitment'tan sonra dışarıdan büyütülememeli (D-1).
        var frozenAlternatives = new ReadOnlyCollection<string>(new List<string>(alternatives));
        var frozenEvidence = new ReadOnlyCollection<Premise>(new List<Premise>(evidence));

        if (frozenAlternatives.Count == 0)
            throw new ArgumentException("En az bir Alternative gerekli — karşı-olgusuz karar atom olamaz (ENS-2001 §Definition).", nameof(alternatives));
        if (frozenAlternatives.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Boş Alternative olamaz — adsız seçenek değerlendirilemez.", nameof(alternatives));

        // D-1 (ENS-4025 L8): öncülsüz deliberation → öncülsüz commitment → izsiz çıkarım = yasak.
        if (frozenEvidence.Count == 0)
            throw new ArgumentException(
                "En az bir Evidence (öncül) gerekli — öncülsüz deliberation izsiz commitment üretir; " +
                "izsiz çıkarım = black-box = Anayasa Madde VI ihlali (ENS-4025 L8, ADR-0001 §5.5).",
                nameof(evidence));
        if (frozenEvidence.Any(p => p is null))
            throw new ArgumentException("Evidence listesi null öncül içeremez (L8).", nameof(evidence));

        Raise(new AlternativesIdentified(frozenAlternatives, frozenEvidence) { Emitter = emitter, Target = Id });
    }

    /// <summary>
    /// ATOM SINIRI. §Individuation dört koşulu burada mühürlenir: tek Owner (bu çağrı),
    /// tek Purpose (Frame'de belirlendi), açık Alternatives (yukarıda), tek Commitment
    /// olayı (bu event — ikinci kez çağrılamaz).
    /// </summary>
    /// <param name="ruleId">
    /// ENS-4025 L8: türetimi üreten kural/plan kimliği. Varsayılan, ENS-2001'in commitment
    /// mühürüdür; bir Planner (ADR-0001 §5.2) kendi kural kimliğini geçebilir.
    /// </param>
    public void Commit(
        Identity owner,
        string selectedAlternative,
        double confidence,
        string expectedOutcome,
        string ruleId = DecisionCommitted.DefaultRuleId)
    {
        if (Purpose is null)
            throw new InvalidOperationException("Purpose olmadan commit edilemez (Framing fazı atlanmış).");
        if (Alternatives.Count == 0)
            throw new InvalidOperationException("Alternatives olmadan commit edilemez — deliberation tamamlanmamış.");
        if (IsCommitted)
            throw new InvalidOperationException("Decision zaten commit edildi — §Individuation ihlali: tek Commitment olayı.");
        if (!Alternatives.Contains(selectedAlternative))
            throw new ArgumentException("Seçilen Alternative, tanımlanan kümede değil.");

        // D-1 (ENS-4025 L8 / ADR-0001 §5.5): ÖNCÜLSÜZ COMMITMENT YASAK. Bu kontrol, replay yolundan
        // (Rehydrate) gelen Evidence'sız bir akış için de gereklidir — canlı yol tek başına yetmez.
        if (Evidence.Count == 0)
            throw new InvalidOperationException(
                "Öncülsüz (Evidence'sız) commitment yasak — proof-trace kurulamaz; izsiz çıkarım = " +
                "black-box = Anayasa Madde VI ihlali (ENS-4025 L8, ADR-0001 §5.5: 'proof-trace " +
                "action'ın var-olma koşuludur').");

        // AUDIT §5.1: NaN artık geçemez. Eskiden `confidence is < 0 or > 1` NaN'ı geçiriyordu ve
        // o NaN doğrudan Scheduler/Gate'e akıp üçlü fail-open'a dönüşüyordu — zincir kapalı değildi.
        Guard.UnitInterval(confidence, nameof(confidence), "Confidence");

        // TRACE: ENS-4025 §L8 — proof-trace burada GERÇEKTEN doğar. `BuildCommitmentTrace` ctor'u
        // öncülsüz/kuralsız bir izi kurmayı zaten reddeder; yani commitment ancak izi KURULABİLİYORSA
        // gerçekleşir. (D-1 öncesinde bu cümle yalnızca bir yorumdu.)
        var trace = BuildCommitmentTrace(selectedAlternative, expectedOutcome, ruleId);

        // TRACE: ENS-4025 §L7 (muhafazakâr `≤` okuması — dosya başındaki "L7 SAPMASI" notu).
        if (confidence > trace.Confidence)
            throw new ArgumentOutOfRangeException(nameof(confidence), confidence,
                $"Commitment confidence'ı ({confidence:F4}) öncüllerin t-norm'unu ({trace.Confidence:F4}) " +
                "AŞAMAZ — ENS-4025 L7: conf(sonuç) = min(conf(öncüller)). Öncüllerin desteklemediği " +
                "bir güven iddiası, kalibre olmayan bir güven iddiasıdır (P6).");

        Raise(new DecisionCommitted(selectedAlternative, owner, confidence, expectedOutcome, ruleId) { Emitter = owner, Target = Id });
    }

    /// <summary>
    /// ENS-4025 §L8 — commitment'ın proof-trace'ini AKIŞTAN türetir (Axiom 2: durum, olay akışının
    /// fold'udur). Öncüller `AlternativesIdentified.Evidence`'tan gelir; iz olayın içine denormalize
    /// KOPYALANMAZ, böylece "olayda yazan iz" ile "gerçek öncüller" ayrışamaz.
    /// </summary>
    private ProofTrace BuildCommitmentTrace(string selectedAlternative, string expectedOutcome, string ruleId)
        => new(
            ruleId,
            // ENS-4025 §Proof-trace biçiminde `⊢`'ın SAĞ tarafı — hangi olgu türetildi.
            // (`⊢` sembolünü Render() koyar; burada tekrarlanmaz.)
            $"Decision {Id.Value} commitment '{selectedAlternative}' (beklenen sonuç: {expectedOutcome})",
            Evidence);

    /// <summary>Enactment fazı — ADR-0001 Actuation Layer, ENS-2001 Enactment'i realizes eder.</summary>
    public void Enact(Identity emitter, string actionDescription)
    {
        if (!IsCommitted)
            throw new InvalidOperationException("Commit edilmemiş Decision enact edilemez.");
        if (IsEnacted)
            throw new InvalidOperationException("Decision zaten enact edildi.");

        Raise(new DecisionEnacted(actionDescription) { Emitter = emitter, Target = Id });
    }

    /// <summary>Measurement fazı (P4) — Actual Outcome, Learning'in girdisi.</summary>
    public void ObserveOutcome(Identity emitter, string actualOutcome)
    {
        if (!IsEnacted)
            throw new InvalidOperationException("Enact edilmemiş Decision'ın Outcome'u gözlenemez.");

        Raise(new OutcomeObserved(actualOutcome) { Emitter = emitter, Target = Id });
    }

    /// <summary>
    /// Learning fazı — ENS-2004 §1: learning_signal = Actual − Expected. Attribution
    /// seviyesi dürüstçe kaydedilir (L0-L3); sahte kesinlik iddia edilmez.
    /// </summary>
    public void RecordLearning(Identity emitter, string delta, AttributionLevel level, double attributionConfidence)
    {
        if (!HasOutcome)
            throw new InvalidOperationException("Outcome gözlenmeden Learning kaydedilemez.");

        Guard.UnitInterval(attributionConfidence, nameof(attributionConfidence), "Attribution confidence");

        Raise(new LearningRecorded(delta, level, attributionConfidence) { Emitter = emitter, Target = Id });
    }

    private void Raise(DomainEvent @event)
    {
        Apply(@event);
        _uncommitted.Add(@event);
        _history.Add(@event);
    }

    private void Apply(DomainEvent @event)
    {
        switch (@event)
        {
            case DecisionFramed e: Purpose = e.Purpose; break;

            // AUDIT §5.3: event'in taşıdığı liste canlı olabilir (replay'de dışarıdan gelir) —
            // aggregate her zaman KENDİ değiştirilemez kopyasını tutar.
            case AlternativesIdentified e:
                Alternatives = e.Alternatives as ReadOnlyCollection<string>
                               ?? new ReadOnlyCollection<string>(new List<string>(e.Alternatives));
                // D-1: Evidence HER ZAMAN gerçek kopyadır (`as` kestirmesi YOK). Öncül kümesi,
                // replay'den sonra çağıranın canlı listesinden büyütülememelidir — aksi hâlde
                // "commitment'ın öncülleri" sonradan değiştirilebilir bir iddiaya dönüşürdü.
                // (Alternatives'teki `as` kestirmesi ayrı ve HÂLÂ AÇIK bir bulgudur:
                //  `AUDIT_DEFECT_W2_R4` — bu düzeltmenin kapsamı değil, kapatıldığı da iddia edilmiyor.)
                Evidence = new ReadOnlyCollection<Premise>(new List<Premise>(e.Evidence));
                break;

            case DecisionCommitted e:
                IsCommitted = true;
                Confidence = e.Confidence;
                // ENS-4025 L8: iz, akışın FOLD'udur — olaydan okunmaz, akıştan hesaplanır (Axiom 2).
                CommitmentTrace = BuildCommitmentTrace(e.SelectedAlternative, e.ExpectedOutcome, e.RuleId);
                break;
            case DecisionEnacted: IsEnacted = true; break;
            case OutcomeObserved: HasOutcome = true; break;
            case LearningRecorded: break; // Memory'ye yazma sorumluluğu ayrı (ENS-2003)
        }
    }

    /// <summary>
    /// Event akışından yeniden inşa — Axiom 2 (Computational Closure)'nin uygulanışı.
    ///
    /// AUDIT §5.3'ün kapanışı: replay yolu artık canlı yolla AYNI §Individuation invariant'larını
    /// zorlar. Geçersiz bir olay akışı sessizce "karar" üretmez; hangi olayın hangi invariant'ı
    /// ihlal ettiği açıkça söylenir. Bozuk bir event-store, sessizce bozuk bir aggregate'e değil,
    /// AÇIK bir hataya dönüşür.
    /// </summary>
    public static DecisionAggregate Rehydrate(Identity id, IEnumerable<DomainEvent> history)
    {
        ArgumentNullException.ThrowIfNull(history);

        var decision = new DecisionAggregate(id);
        int index = 0;
        foreach (var e in history)
        {
            if (e is null)
                throw new ArgumentException($"Event akışında null olay var (#{index}).", nameof(history));

            decision.EnsureReplayInvariant(e, index);
            decision.Apply(e);
            decision._history.Add(e);
            index++;
        }
        return decision;
    }

    /// <summary>
    /// Replay-yolu §Individuation mührü (ENS-2001). Canlı yoldaki her ön-koşulun replay
    /// karşılığı; ikisi ayrışırsa aggregate iki farklı gerçeklik üretir — bu kabul edilemez.
    /// </summary>
    private void EnsureReplayInvariant(DomainEvent @event, int index)
    {
        string Where(string message) => $"Event #{index} ({@event.GetType().Name}): {message}";

        switch (@event)
        {
            case DecisionFramed e:
                if (Purpose is not null)
                    throw new InvalidOperationException(Where("İkinci Framing olayı — §Individuation: tek Purpose."));
                if (string.IsNullOrWhiteSpace(e.Purpose))
                    throw new InvalidOperationException(Where("Purpose boş — amaçsız karar atom olamaz."));
                break;

            case AlternativesIdentified e:
                if (Purpose is null)
                    throw new InvalidOperationException(Where("Framing olmadan Alternatives — lifecycle sırası bozuk (ENS-2001 §Lifecycle)."));
                if (IsCommitted)
                    throw new InvalidOperationException(Where("Commitment'tan SONRA Alternatives — §Individuation: atom mühürlendi."));
                if (e.Alternatives is null || e.Alternatives.Count == 0)
                    throw new InvalidOperationException(Where("Boş Alternative kümesi — karşı-olgusuz karar atom olamaz."));
                // D-1: canlı yolun öncül şartının replay karşılığı. İkisi ayrışırsa, canlı olarak
                // ÜRETİLEMEYEN izsiz bir commitment replay ile üretilebilirdi (AUDIT §5.3'ün dersi).
                if (e.Evidence is null || e.Evidence.Count == 0)
                    throw new InvalidOperationException(Where(
                        "Boş Evidence kümesi — öncülsüz deliberation izsiz commitment üretir (ENS-4025 L8)."));
                if (e.Evidence.Any(p => p is null))
                    throw new InvalidOperationException(Where("Evidence listesinde null öncül var (L8)."));
                break;

            case DecisionCommitted e:
                if (Purpose is null)
                    throw new InvalidOperationException(Where("Purpose'suz Commitment — Framing fazı atlanmış (§Individuation: tek Purpose)."));
                if (Alternatives.Count == 0)
                    throw new InvalidOperationException(Where("Alternative'siz Commitment — deliberation tamamlanmamış (§Individuation: açık Alternatives)."));
                if (IsCommitted)
                    throw new InvalidOperationException(Where("İKİNCİ Commitment olayı — §Individuation: tek Commitment olayı."));
                if (!Alternatives.Contains(e.SelectedAlternative))
                    throw new InvalidOperationException(Where($"'{e.SelectedAlternative}' hiç değerlendirilmemiş — tanımlanan Alternative kümesinde yok."));
                Guard.UnitInterval(e.Confidence, nameof(DecisionCommitted.Confidence), Where("Confidence"));

                // ---- D-1: L8/L7 mühürü replay yolunda da zorunlu (canlı yolla BİREBİR aynı) ----
                if (Evidence.Count == 0)
                    throw new InvalidOperationException(Where(
                        "Öncülsüz (Evidence'sız) Commitment — proof-trace kurulamaz; izsiz çıkarım yasak (ENS-4025 L8)."));
                if (string.IsNullOrWhiteSpace(e.RuleId))
                    throw new InvalidOperationException(Where(
                        "Kural kimliği (RuleId) boş — hangi kuralın türettiği bilinmeyen olgu izsizdir (ENS-4025 L8)."));
                var replayTrace = BuildCommitmentTrace(e.SelectedAlternative, e.ExpectedOutcome, e.RuleId);
                if (e.Confidence > replayTrace.Confidence)
                    throw new InvalidOperationException(Where(
                        $"Commitment confidence'ı ({e.Confidence:F4}) öncüllerin t-norm'unu ({replayTrace.Confidence:F4}) aşıyor " +
                        "— ENS-4025 L7: conf(sonuç) = min(conf(öncüller))."));
                break;

            case DecisionEnacted:
                if (!IsCommitted)
                    throw new InvalidOperationException(Where("Commit edilmemiş Decision enact edilemez."));
                if (IsEnacted)
                    throw new InvalidOperationException(Where("Decision zaten enact edildi."));
                break;

            case OutcomeObserved:
                if (!IsEnacted)
                    throw new InvalidOperationException(Where("Enact edilmemiş Decision'ın Outcome'u gözlenemez."));
                break;

            case LearningRecorded e:
                if (!HasOutcome)
                    throw new InvalidOperationException(Where("Outcome gözlenmeden Learning kaydedilemez."));
                Guard.UnitInterval(e.AttributionConfidence, nameof(LearningRecorded.AttributionConfidence), Where("Attribution confidence"));
                break;
        }
    }

    /// <summary>Değiştirilemez görünüm — audit izi downcast'le silinemez (AUDIT §5.2).</summary>
    public IReadOnlyList<DomainEvent> UncommittedEvents => _uncommittedView;

    /// <summary>Değiştirilemez görünüm — "karar bir satır değil, olay geçmişidir" (ADR-0001 §5.4).</summary>
    public IReadOnlyList<DomainEvent> History => _historyView;

    public void ClearUncommitted() => _uncommitted.Clear();
}
