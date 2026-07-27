using System.Collections.ObjectModel;
using Ens.Kernel.Domain;
using Ens.Kernel.Domain.Events;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-4025 §L8 (proof-trace invariant — izsiz commitment yasak) + §L7 (min t-norm)
// TRACE: ADR-0001 (accepted, v0.3.1) §5.5 "proof-trace action'ın VAR-OLMA KOŞULUDUR" + §5.4
//        ("proof-trace atom düzeyinde zorunludur" — atom = DecisionCommitted)
// TRACE: ENS-2001 §Individuation, ENS-2002 §Implications ("Evidence, context'in commit-edilmiş
//        alt kümesidir", P6), Anayasa Madde VI (black-box yasağı)
//
// ============================================================================================
// KAPANIŞ KANITI — AUDIT-WAVE2-FIDELITY.md §D-1
//
// BULGU (bağımsız denetim): `DecisionAggregate.cs:8` "proof-trace invariant — izsiz commitment
// yasak" İDDİA EDİYOR, ama:
//   • `evidence` yalnızca null-check'ten geçiyor (boş liste kabul),
//   • `Apply()` Evidence'ı aggregate'e hiç yazmıyor,
//   • `Commit()` Evidence'a hiç bakmıyor,
//   • `DecisionCommitted` ne kural kimliği ne öncül taşıyor,
//   • `ProofTrace` o dosyada HİÇ kullanılmıyor.
// Yani `IdentifyAlternatives(who, ["A","B"], evidence: [])` → `Commit(...)` SIFIR-ÖNCÜLLÜ bir
// commitment üretiyordu. Denetim bunu "statik okumadan türetildi, TESTLE TEYİT EDİLMELİ" diye
// işaretledi — bu dosya o teyidi ve kapanışı birlikte verir.
//
// SEÇİLEN YOL: (a) kodu iddiaya yükseltmek. Gerekçe ADR-0001 §5.5/§5.4'tür (status: accepted):
// proof-trace atom düzeyinde ZORUNLUDUR; iddiayı düşürmek (yol b) kabul edilmiş bir ADR'yi
// uygulamamak olurdu. Ayrıntılı gerekçe: `Domain/DecisionAggregate.cs` dosya başı bloğu.
//
// ADLANDIRMA (denetim sözleşmesi):
//   AUDIT_FIXED_CPT_D1_*   → geçerse kusur KAPALI (regresyon bekçisi; kırılırsa kusur geri geldi).
//   AUDIT_DEFECT_CPT_D1_*  → geçerse HÂLÂ AÇIK bir borç var (dürüstlük kaydı, kapanış iddiası yok).
// ============================================================================================

public sealed class AuditFixed_CommitmentProofTraceTests
{
    private static readonly Identity Who = Identity.New();

    private static DecisionAggregate Deliberated(params Premise[] evidence)
    {
        var d = DecisionAggregate.Frame(Who, "tedarikçi seç");
        d.IdentifyAlternatives(Who, ["A", "B"], evidence);
        return d;
    }

    // ========================================================================================
    // 1. DENETİMİN KOD ÖRNEĞİ — artık DERLENİYOR ama ÇALIŞMIYOR (kusurun tam kapanışı)
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_CPT_D1_the_audits_own_zero_premise_commitment_snippet_is_now_rejected()
    {
        // AUDIT-WAVE2-FIDELITY §D-1'in birebir kod örneği:
        //     var d = DecisionAggregate.Frame(who, "tedarikçi seç");
        //     d.IdentifyAlternatives(who, ["A", "B"], evidence: []);   // ← eskiden KABUL
        //     d.Commit(who, "A", 0.9, "maliyet düşer");                // ← eskiden BAŞARILI
        var d = DecisionAggregate.Frame(Who, "tedarikçi seç");

        var ex = Assert.Throws<ArgumentException>(() => d.IdentifyAlternatives(Who, ["A", "B"], []));
        Assert.Equal("evidence", ex.ParamName);
        Assert.Contains("L8", ex.Message);

        // Ve zincirin ikinci halkası da kapalı: öncül yoksa commitment de yok.
        var commitEx = Assert.Throws<InvalidOperationException>(
            () => d.Commit(Who, "A", 0.9, "maliyet düşer"));
        Assert.Contains("Alternatives olmadan commit edilemez", commitEx.Message);

        // Yapısal kanıt: aggregate hiçbir olay üretmedi — "sıfır-öncüllü atom" temsil EDİLEMEZ.
        Assert.False(d.IsCommitted);
        Assert.Null(d.CommitmentTrace);
        Assert.Single(d.History);   // yalnızca DecisionFramed
    }

    // ========================================================================================
    // 2. COMMITMENT BİR PROOF-TRACE ÜRETİR (L8) VE İZ ÖNCÜLLERİNE BAĞLIDIR
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_CPT_D1_commitment_emits_a_proof_trace_bound_to_its_evidence()
    {
        var evidence = new[]
        {
            new Premise("skorkart-2026Q2", 0.90),
            new Premise("denetim-raporu", 0.70),
            new Premise("fiyat-teklifi", 0.85)
        };
        var d = Deliberated(evidence);
        d.Commit(Who, "A", 0.65, "maliyet %8 düşer");

        var trace = d.CommitmentTrace;
        Assert.NotNull(trace);

        // (a) HANGİ KURAL (L8'in birinci sorusu)
        Assert.Equal(DecisionCommitted.DefaultRuleId, trace!.RuleId);

        // (b) HANGİ ÖNCÜLLER (L8'in ikinci sorusu) — deliberation'daki Evidence'ın TAM KENDİSİ
        Assert.Equal(evidence.Length, trace.Premises.Count);
        Assert.Equal(
            evidence.Select(p => (p.Source, p.Confidence)).ToList(),
            trace.Premises.Select(p => (p.Source, p.Confidence)).ToList());

        // (c) HANGİ CONFIDENCE (L7: min t-norm) — hesaplanır, atanmaz
        Assert.Equal(0.70, trace.Confidence, precision: 10);

        // (d) NE TÜRETİLDİ (`⊢` sağ tarafı): karar kimliği + seçilen alternatif + beklenen sonuç
        Assert.Contains(d.Id.Value, trace.Conclusion);
        Assert.Contains("'A'", trace.Conclusion);
        Assert.Contains("maliyet %8 düşer", trace.Conclusion);

        // (e) P6: açıklama nesnesi İNSANA GÖSTERİLEBİLİR ve öncülleri adıyla taşır.
        var rendered = trace.Render();
        foreach (var p in evidence) Assert.Contains(p.Source, rendered);

        // (f) Commit edilmiş her aggregate'te iz vardır — "izsiz commitment" durumu YOK.
        Assert.True(d.IsCommitted);
        Assert.NotNull(d.CommitmentTrace);
    }

    [Fact]
    public void AUDIT_FIXED_CPT_D1_an_uncommitted_decision_has_no_trace_and_a_committed_one_always_has_one()
    {
        var d = Deliberated(new Premise("kanit", 0.9));
        Assert.False(d.IsCommitted);
        Assert.Null(d.CommitmentTrace);          // henüz atom değil → iz de yok

        d.Commit(Who, "B", 0.5, "beklenen");
        Assert.True(d.IsCommitted);
        Assert.NotNull(d.CommitmentTrace);       // atom ⇒ iz (çift-yönlü invariant)
    }

    // ========================================================================================
    // 3. L7 — ÖNCÜLLERİN DESTEKLEMEDİĞİ GÜVEN İDDİA EDİLEMEZ (muhafazakâr `≤` okuması)
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_CPT_D1_commitment_confidence_cannot_exceed_the_t_norm_of_its_premises()
    {
        var d = Deliberated(new Premise("zayıf-kanıt", 0.30), new Premise("güçlü-kanıt", 0.95));

        // min(0.30, 0.95) = 0.30 → 0.31 bile fazladır.
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => d.Commit(Who, "A", 0.31, "harika sonuç"));
        Assert.Equal("confidence", ex.ParamName);
        Assert.Contains("L7", ex.Message);
        Assert.False(d.IsCommitted);   // reddedilen commit HİÇBİR iz bırakmadı (kısmi durum yok)

        // Tam sınır (=) kabul edilir; sınırın bir ULP üstü reddedilir.
        Assert.Throws<ArgumentOutOfRangeException>(
            () => d.Commit(Who, "A", Math.BitIncrement(0.30), "harika"));
        d.Commit(Who, "A", 0.30, "temkinli sonuç");
        Assert.True(d.IsCommitted);
        Assert.Equal(0.30, d.CommitmentTrace!.Confidence, precision: 10);
    }

    [Fact]
    public void AUDIT_FIXED_CPT_D1_being_less_confident_than_the_premises_is_always_allowed()
    {
        // `≤` yönü kasıtlıdır: temkinli olmak serbest, öncülleri aşmak yasak (P6/P7 yönü).
        var d = Deliberated(new Premise("kanit", 1.0));
        d.Commit(Who, "A", 0.01, "belirsiz sonuç");
        Assert.Equal(0.01, d.Confidence!.Value, precision: 10);
        Assert.Equal(1.0, d.CommitmentTrace!.Confidence, precision: 10);
    }

    // ========================================================================================
    // 4. REPLAY YOLU — canlı yolla BİREBİR aynı mühür (AUDIT §5.3'ün dersi tekrar edilmedi)
    // ========================================================================================

    private static DomainEvent[] Stream(Identity id, IReadOnlyList<Premise> evidence, double commitConfidence,
        string ruleId = DecisionCommitted.DefaultRuleId)
        =>
        [
            new DecisionFramed("amaç") { Emitter = Who, Target = id },
            new AlternativesIdentified(["A", "B"], evidence) { Emitter = Who, Target = id },
            new DecisionCommitted("A", Who, commitConfidence, "beklenen", ruleId) { Emitter = Who, Target = id }
        ];

    [Fact]
    public void AUDIT_FIXED_CPT_D1_replay_rejects_a_traceless_commitment_stream()
    {
        var id = Identity.New();

        // (a) Öncülsüz deliberation olayı — canlı yolda üretilemeyen bu akış replay'de de geçmez.
        var ex = Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, Stream(id, [], 0.8)));
        Assert.Contains("Evidence", ex.Message);
        Assert.Contains("L8", ex.Message);

        // (b) Kural kimliği boş — "hangi kural?" cevapsızsa olgu izsizdir (L8).
        var blankRule = Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, Stream(id, [new Premise("kanit", 1.0)], 0.8, ruleId: "   ")));
        Assert.Contains("RuleId", blankRule.Message);

        // (c) L7 ihlali replay'de de yakalanır: 0.99 güven, 0.30'luk öncüllerden türetilemez.
        var l7 = Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, Stream(id, [new Premise("zayıf", 0.30)], 0.99)));
        Assert.Contains("L7", l7.Message);

        // (d) Guard çok geniş değil: sağlıklı akış GEÇİYOR (boş bir sınav değil).
        var ok = DecisionAggregate.Rehydrate(id, Stream(id, [new Premise("kanit", 0.9)], 0.8));
        Assert.True(ok.IsCommitted);
        Assert.NotNull(ok.CommitmentTrace);
    }

    [Fact]
    public void AUDIT_FIXED_CPT_D1_trace_is_a_fold_of_the_stream_so_live_and_replayed_traces_cannot_drift()
    {
        // ENS-4001 Axiom 2: iz olayın içine denormalize KOPYALANMAZ, akıştan HESAPLANIR.
        // Bu yüzden "olayda yazan iz" ile "gerçek öncüller" ayrışamaz — yapısal olarak imkânsız.
        var live = Deliberated(new Premise("kanit-1", 0.9), new Premise("kanit-2", 0.6));
        live.Commit(Who, "A", 0.55, "beklenen", ruleId: "PLAN-7");

        var replayed = DecisionAggregate.Rehydrate(live.Id, live.History);

        Assert.Equal(live.CommitmentTrace!.RuleId, replayed.CommitmentTrace!.RuleId);
        Assert.Equal(live.CommitmentTrace.Conclusion, replayed.CommitmentTrace.Conclusion);
        Assert.Equal(live.CommitmentTrace.Confidence, replayed.CommitmentTrace.Confidence, precision: 12);
        Assert.Equal(
            live.CommitmentTrace.Premises.Select(p => (p.Source, p.Confidence)).ToList(),
            replayed.CommitmentTrace.Premises.Select(p => (p.Source, p.Confidence)).ToList());

        // Record eşitliği (ProofTrace bir record'dur) — iki yol AYNI nesneyi üretir.
        Assert.Equal(live.CommitmentTrace.RuleId + live.CommitmentTrace.Conclusion,
                     replayed.CommitmentTrace.RuleId + replayed.CommitmentTrace.Conclusion);
        Assert.Equal("PLAN-7", replayed.CommitmentTrace.RuleId);   // Planner'ın kural kimliği taşındı
    }

    // ========================================================================================
    // 5. ÖNCÜL KÜMESİ SONRADAN BÜYÜTÜLEMEZ (snapshot mührü — AUDIT §5.3 deseninin Evidence ayağı)
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_CPT_D1_evidence_cannot_be_grown_by_the_caller_after_deliberation()
    {
        var backing = new List<Premise> { new("kanit-1", 0.9) };
        var d = DecisionAggregate.Frame(Who, "amaç");
        d.IdentifyAlternatives(Who, ["A", "B"], backing);

        backing.Add(new Premise("sonradan-eklenen-kanit", 1.0));   // çağıran listeyi büyütüyor

        Assert.Single(d.Evidence);
        Assert.Equal("kanit-1", d.Evidence[0].Source);

        d.Commit(Who, "A", 0.9, "beklenen");
        Assert.Single(d.CommitmentTrace!.Premises);
        Assert.DoesNotContain(d.CommitmentTrace.Premises, p => p.Source == "sonradan-eklenen-kanit");
    }

    [Fact]
    public void AUDIT_FIXED_CPT_D1_replayed_evidence_is_a_real_copy_not_a_live_view()
    {
        // `AUDIT_DEFECT_CPT_W2_R4`, Alternatives için `as ReadOnlyCollection<T>` kestirmesinin bir
        // KOPYA DEĞİL GÖRÜNÜM verdiğini gösterdi (o bulgu HÂLÂ AÇIK — bu düzeltmenin kapsamı değil).
        // Evidence tarafında aynı deliği açmamak için `Apply` her zaman gerçek kopya alır:
        // aksi hâlde bir commitment'ın öncülleri replay'den SONRA dışarıdan büyütülebilirdi.
        var id = Identity.New();
        var backing = new List<Premise> { new("kanit", 0.9) };
        var liveView = new ReadOnlyCollection<Premise>(backing);

        var replayed = DecisionAggregate.Rehydrate(id,
        [
            new DecisionFramed("amaç") { Emitter = Who, Target = id },
            new AlternativesIdentified(["A", "B"], liveView) { Emitter = Who, Target = id }
        ]);

        backing.Add(new Premise("sızdırılan-kanit", 1.0));

        Assert.Single(replayed.Evidence);
        Assert.DoesNotContain(replayed.Evidence, p => p.Source == "sızdırılan-kanit");
    }

    // ========================================================================================
    // 6. HÂLÂ AÇIK OLANLAR — kapanış iddiasının SINIRI (yalnızca kazanım listelemek manipülasyondur)
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_CPT_D1_residual_premises_are_still_uncalibrated_free_text()
    {
        // AÇIK BORÇ (ProofTrace.cs "(b)" + Guard.cs "DÜRÜST SINIR"): öncül hâlâ serbest metin +
        // ÖZ-BEYAN confidence'tır. ENS-4010 Context/Evidence/Memory NODE'una tipli referans YOK,
        // kalibrasyon YOK. Yani L8 KARDİNALİTE ve BAĞLANTI düzeyinde zorlanıyor; İÇERİK düzeyinde
        // değil. Bu test o sınırı görünür tutar — kapandığı iddia EDİLMİYOR.
        var d = DecisionAggregate.Frame(Who, "amaç");
        d.IdentifyAlternatives(Who, ["A", "B"], [new Premise("çünkü ben öyle diyorum", 1.0)]);
        d.Commit(Who, "A", 1.0, "her şey harika olacak");

        Assert.Equal(1.0, d.CommitmentTrace!.Confidence, precision: 10);
        Assert.Single(d.CommitmentTrace.Premises);

        // Ve `AUDIT_FINDING_CPT_W2_P5`'in idempotans bulgusu burada da geçerli: 100 kopya öncül,
        // korroborasyon üretmez — `min` aynı kalır, ama trace "100 kanıt" varmış gibi görünür.
        var many = Enumerable.Repeat(new Premise("tek-kanit", 0.42), 100).ToList();
        var d2 = DecisionAggregate.Frame(Who, "amaç");
        d2.IdentifyAlternatives(Who, ["A"], many);
        d2.Commit(Who, "A", 0.42, "beklenen");
        Assert.Equal(0.42, d2.CommitmentTrace!.Confidence, precision: 12);
        Assert.Equal(100, d2.CommitmentTrace.Premises.Count);
    }

    [Fact]
    public void AUDIT_DEFECT_CPT_D1_residual_only_the_commitment_atom_emits_a_trace()
    {
        // ADR-0001 §5.4 proof-trace'i ATOM düzeyinde zorunlu kılar ve bu artım tam olarak orada
        // zorluyor. Ama §5.5'in "her action bir proof-trace zarfı üretir" cümlesinin lifecycle
        // devamı (Enactment / Measurement / Learning) HENÜZ iz üretmiyor — açık borç.
        var d = Deliberated(new Premise("kanit", 1.0));
        d.Commit(Who, "A", 0.9, "beklenen");
        d.Enact(Who, "yapıldı");
        d.ObserveOutcome(Who, "gerçekleşen");
        d.RecordLearning(Who, "Δ=+1", AttributionLevel.L1_ModelBased, 0.6);

        // Aggregate'te commitment DIŞINDA bir iz yüzeyi yok:
        var traceProps = typeof(DecisionAggregate).GetProperties()
            .Where(p => p.PropertyType == typeof(ProofTrace)).ToList();
        Assert.Single(traceProps);
        Assert.Equal(nameof(DecisionAggregate.CommitmentTrace), traceProps[0].Name);
    }
}
