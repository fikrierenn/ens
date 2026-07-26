using System.Reflection;
using Ens.Kernel;
using Ens.Kernel.Adapter;
using Ens.Kernel.Capability;
using Ens.Kernel.Domain;
using Ens.Kernel.Domain.Events;
using Ens.Kernel.Laws;
using Xunit;

namespace Ens.Kernel.Tests;

// ============================================================================================
// BAGIMSIZ DUSMANCA DENETIM (ens-skeptic) — koordinatorun YAZMADIGI testler.
//
// Bu dosya kernel'i dogrulamak icin DEGIL, KIRMAK icin yazildi. Diger test dosyalari
// bilesenleri saglikli girdilerle sinar; bu dosya patolojik girdilerle, downcast'le,
// reflection'la ve fuzz'la ayni iddialari curutmeye calisir.
//
// ADLANDIRMA SOZLESMESI — testin ne iddia ettigini adindan okuyun:
//   AUDIT_HOLDS_*   -> iddia saldiridan SAG CIKTI. Assertion = istenen davranis.
//   AUDIT_FIXED_*   -> denetimin buldugu kusur KAPANDI. Assertion artik DUZELTILMIS davranisi
//                      sabitler; kusur geri gelirse test KIRILIR (regresyon bekcisi).
//                      Her birinin basinda: hangi kusurdu / nasil kapandi.
//   AUDIT_DEFECT_*  -> KUSUR HALA ACIK. Assertion, MEVCUT (kusurlu) davranisi sabitler
//                      (characterization test). Bilincli acik birakilanlar; gerekcesi yorumda.
//   AUDIT_FINDING_* -> kod dogru ama DEMO'nun sunumu yaniltici / iddiasini kanitlamiyor.
//
// DUZELTME TURU (2026-07-25, AUDIT.md 5 kapanisi): 22 AUDIT_DEFECT testi AUDIT_FIXED'e cevrildi.
// Hicbiri SILINMEDI — hepsi tersine cevrildi ki kusurun KAPANDIGI kanitlansin ve geri gelmesi
// halinde kirmizi yansin. Acik kalan 4 kusur (B6, C1, E3, E5, H1) DEFECT olarak duruyor ve
// gerekceleri yorumlarinda + README'de yazili.
//
// Ayrintili gerekce: 7000-reference-implementation/AUDIT.md
// ============================================================================================

public sealed class AdversarialAuditTests
{
    private static readonly DateTimeOffset T0 = new(2026, 7, 25, 9, 0, 0, TimeSpan.Zero);

    /// <summary>Deterministik bellek: "gelecek tarihli Verify" kontrolu sistem saatine bagli olmasin.</summary>
    private static CompanyMemory MemoryAt(DateTimeOffset now) => new(new FixedTimeProvider(now));

    // ========================================================================================
    // A. BOUNDED-AUTONOMY GATE (P7) — "sinir asimi bloklanir" iddiasini fail-open ile kirma
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_A1_NaN_stake_is_rejected_instead_of_granting_full_autonomy()
    {
        // KUSURDU: NaN her karsilastirmada false doner. `stake < 0` guard'i NaN'i geciriyordu;
        //   InfoNeed NaN oluyor, `NaN >= blockThreshold` ve `NaN >= autonomyThreshold` de false
        //   -> en PERMISIF dal -> Autonomous. Olculemeyen bir stake "sinir ici, otonom icra et"
        //   kararina donusuyordu. FAIL-OPEN.
        // KAPANDI: Guard.NonNegativeFinite (Ens.Kernel/Guard.cs) hem DecisionGravity.InfoNeed
        //   hem BoundedAutonomyGate.Evaluate yolunda NaN/Infinity'yi REDDEDIYOR. Olculemeyen
        //   girdi artik otonomi kazanamaz — fail-CLOSED.
        var gateEx = Assert.Throws<ArgumentOutOfRangeException>(() => BoundedAutonomyGate.Evaluate(
            stake: double.NaN, confidence: 0.5, conformanceDeficit: 0.5, isIrreversible: false,
            autonomyThreshold: 5_000, blockThreshold: 60_000));
        Assert.Equal("stake", gateEx.ParamName);

        // Kok neden yasanin kendisinde de kapali (tek Guard, iki cagri yolu):
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionGravity.InfoNeed(double.NaN, 0.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionGravity.InfoNeed(double.PositiveInfinity, 0.5));
    }

    [Fact]
    public void AUDIT_FIXED_A2_NaN_confidence_is_rejected_instead_of_granting_full_autonomy()
    {
        // KUSURDU: 250 milyonluk bir karar, confidence NaN ise otonom icraya aciliyordu.
        // KAPANDI: Guard.OptionalUnitInterval — null gecerli (maksimum belirsizlik), NaN degil.
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => BoundedAutonomyGate.Evaluate(
            stake: 250_000_000, confidence: double.NaN, conformanceDeficit: 0.9, isIrreversible: false,
            autonomyThreshold: 5_000, blockThreshold: 60_000));
        Assert.Equal("confidence", ex.ParamName);

        // NaN reddedilirken `null` (henuz commit edilmemis) HALA gecerli ve MUHAFAZAKAR:
        Assert.Equal(GateDecision.Blocked, BoundedAutonomyGate.Evaluate(
            250_000_000, null, 0.9, false, 5_000, 60_000).Decision);
    }

    [Fact]
    public void AUDIT_FIXED_A3_confidence_above_one_is_now_validated_on_the_security_critical_path()
    {
        // KUSURDU: DecisionAggregate.Commit ve Premise confidence'i [0,1] doguluyordu; ama
        //   GUVENLIK-KRITIK yol olan DecisionGravity.InfoNeed / BoundedAutonomyGate DOGRULAMIYORDU.
        //   confidence = 5.0 -> uncertainty = -4.0 -> InfoNeed = -4 x stake -> negatif -> hicbir
        //   esigi asmaz -> Autonomous. Tek bir kalibrasyon hatasi P7'yi tamamen kapatiyordu.
        // KAPANDI: aralik dogrulamasi artik gate/gravity yolunda da var; InfoNeed asla negatif olamaz.
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => BoundedAutonomyGate.Evaluate(
            stake: 1_000_000_000, confidence: 5.0, conformanceDeficit: 1.0, isIrreversible: false,
            autonomyThreshold: 1, blockThreshold: 2));
        Assert.Equal("confidence", ex.ParamName);

        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionGravity.InfoNeed(1_000_000_000, 5.0));

        // Gecerli aralikta ayni senaryo dogru davranir: yuksek stake + dusuk guven -> BLOK.
        Assert.Equal(GateDecision.Blocked, BoundedAutonomyGate.Evaluate(
            1_000_000_000, 0.5, 1.0, false, 1, 2).Decision);
    }

    [Fact]
    public void AUDIT_HOLDS_A4_irreversibility_blocks_even_under_NaN_poisoning()
    {
        // Tek gercekten fail-CLOSED kural: geri-donulemezlik InfoNeed'den bagimsiz blokluyor.
        // Duzeltmeden SONRA da aynen gecerli: girdi dogrulamasi bu dali ASLA golgelemez —
        // bir dogrulama exception'i bir guvenlik kontrolunu dusurmemelidir (Evaluate'te
        // isIrreversible dali bilincli olarak Guard'lardan ONCE gelir).
        var gate = BoundedAutonomyGate.Evaluate(
            double.NaN, double.NaN, double.NaN, isIrreversible: true, 5_000, 60_000);

        Assert.Equal(GateDecision.CriticalBlock, gate.Decision);

        // Ve olculemedigini SAKLAMIYOR — sahte bir InfoNeed uydurmuyor (Madde X).
        Assert.True(double.IsNaN(gate.InfoNeed));
        Assert.Contains("ÖLÇÜLEMEDİ", gate.Reason);
    }

    [Fact]
    public void AUDIT_HOLDS_A5_negative_stake_and_inconsistent_policy_throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedAutonomyGate.Evaluate(-1, 0.5, 0.5, false, 10, 100));
        Assert.Throws<ArgumentException>(
            () => BoundedAutonomyGate.Evaluate(100, 0.5, 0.5, false, autonomyThreshold: 100, blockThreshold: 10));
    }

    [Fact]
    public void AUDIT_HOLDS_A6_null_confidence_is_treated_as_maximum_uncertainty()
    {
        // Commit edilmemis karar -> belirsizlik 1.0 -> InfoNeed = Stake. Muhafazakar, dogru.
        Assert.Equal(1000.0, DecisionGravity.InfoNeed(1000, null), precision: 10);
        Assert.Equal(GateDecision.Blocked,
            BoundedAutonomyGate.Evaluate(1000, null, 0.5, false, 10, 100).Decision);
    }

    [Fact]
    public void AUDIT_HOLDS_A7_boundary_values_behave_as_specified()
    {
        Assert.Equal(0.0, DecisionGravity.InfoNeed(0, 0.5), precision: 10);       // stake = 0
        Assert.Equal(0.0, DecisionGravity.InfoNeed(1e12, 1.0), precision: 10);    // confidence = 1
        Assert.Equal(GateDecision.Autonomous,
            BoundedAutonomyGate.Evaluate(0, null, 1.0, false, 10, 100).Decision);
        // Esigin tam UZERINDE (>=) blok — sinir dahil.
        Assert.Equal(GateDecision.Blocked,
            BoundedAutonomyGate.Evaluate(100, 0.0, 0.5, false, 10, 100).Decision);
    }

    // ========================================================================================
    // B. SCHEDULER — formulu ELLE hesaplayip fuzz ile dogrula; siralamayi kirmayi dene
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_B1_fuzz_200_decisions_priority_matches_hand_computed_formula()
    {
        // Formulu koddan CAGIRMADAN elle hesapliyoruz:
        //   Stake x (1-Confidence) x clamp(Deficit, 0, 1).
        // NOT (duzeltme sonrasi): ust kirpma AUDIT 5.6'nin kapanisidir — deficit artik [0,1]'e
        // normalize edilir. Fuzz araligi bilincli olarak [-0.5, 1.5]: HER IKI kirpma dali da
        // sinaniyor (asagi kirpma zaten mevcut ve kasitliydi, yukari kirpma yeni).
        var rng = new Random(20260725);
        var pending = new List<PendingDecision>();
        for (int i = 0; i < 200; i++)
        {
            pending.Add(new PendingDecision(
                Identity.New(),
                Stake: rng.NextDouble() * 1_000_000,
                Confidence: rng.NextDouble(),
                ConformanceDeficit: rng.NextDouble() * 2 - 0.5,   // negatifler ve 1'in ustu dahil
                IsIrreversible: rng.NextDouble() < 0.2));
        }

        var scheduled = Scheduler.Schedule(pending, autonomyThreshold: 5_000, blockThreshold: 500_000);

        Assert.Equal(pending.Count, scheduled.Count);

        foreach (var s in scheduled)
        {
            double handInfoNeed = s.Decision.Stake * (1.0 - s.Decision.Confidence!.Value);
            double handPriority = handInfoNeed * Math.Clamp(s.Decision.ConformanceDeficit, 0.0, 1.0);
            Assert.Equal(handPriority, s.AttentionPriority, precision: 6);
            Assert.Equal(handInfoNeed, s.Gate.InfoNeed, precision: 6);
        }

        for (int i = 1; i < scheduled.Count; i++)
            Assert.True(scheduled[i - 1].AttentionPriority >= scheduled[i].AttentionPriority,
                $"Siralama bozuk: #{i - 1}={scheduled[i - 1].AttentionPriority} < #{i}={scheduled[i].AttentionPriority}");

        // Fuzz gercekten her iki kirpma dalina da girdi mi? (bos bir sinav olmasin)
        Assert.Contains(pending, d => d.ConformanceDeficit < 0);
        Assert.Contains(pending, d => d.ConformanceDeficit > 1);
    }

    [Fact]
    public void AUDIT_HOLDS_B2_fuzz_tier_boundaries_match_hand_computed_thresholds()
    {
        var rng = new Random(1966); // Howard 1966
        for (int i = 0; i < 500; i++)
        {
            double stake = rng.NextDouble() * 100_000;
            double conf = rng.NextDouble();
            double infoNeed = stake * (1.0 - conf);

            var expected = infoNeed >= 40.0 ? LlmTier.Critical
                         : infoNeed >= 10.0 ? LlmTier.Complex
                         : LlmTier.Operational;

            Assert.Equal(expected, LlmTierSelector.SelectTier(stake: stake, confidence: conf));
        }
    }

    [Fact]
    public void AUDIT_FIXED_B3_NaN_decision_no_longer_slips_into_the_darkest_corner()
    {
        // KUSURDU: UCLU FAIL-OPEN. Olculemeyen bir karar:
        //   (1) Gate  -> Autonomous  (insan gormez)
        //   (2) Tier  -> Operational (en zayif/ucuz model bakar)
        //   (3) Sira  -> EN SON      (dikkat butcesi asla ona ulasmaz)
        //   Kernel bir istisna da atmiyordu; karar sessizce sistemin en karanlik kosesine dusuyordu.
        // KAPANDI: Scheduler gate'i yeniden uygulamaz ama hatayi da YUTMAZ — olculemeyen girdi
        //   partiyi sessizce siralamaz, ACIK hata verir (registry felsefesiyle tutarli:
        //   "sessizce cozmez"). Fail-closed: hicbir karar otonomi kazanmaz.
        var nan = new PendingDecision(Identity.New(), Stake: double.NaN, Confidence: 0.5, ConformanceDeficit: 0.9);
        var normal = new PendingDecision(Identity.New(), Stake: 1_000, Confidence: 0.5, ConformanceDeficit: 0.1);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => Scheduler.Schedule([nan, normal], autonomyThreshold: 10, blockThreshold: 100));

        // Saglikli parti etkilenmiyor (guard cok genis degil):
        var ok = Scheduler.Schedule([normal], 10, 100);
        Assert.Single(ok);
        Assert.Equal(GateDecision.Blocked, ok[0].Gate.Decision); // InfoNeed 500 >= 100
    }

    [Fact]
    public void AUDIT_FIXED_B4_ConformanceDeficit_is_clamped_so_the_attention_queue_is_not_gameable()
    {
        // KUSURDU: ENS-3022 ConformanceDeficit'i NORMALIZE bir acik olarak tanimlar; kod [0,1]
        //   kisiti KOYMUYORDU. Deficit'i sisiren bir cagiran (1e9), onemsiz bir karari en kritik
        //   kararin onune gecirebiliyordu — dikkat kuyrugu manipule edilebilirdi.
        // KAPANDI: Guard.NormalizedDeficit sonluluk ZORUNLU kilar ve [0,1]'e kirpar. Kirpma
        //   yalnizca onceligi DUSURUR, asla artirmaz (muhafazakar yon).
        var trivial = new PendingDecision(Identity.New(), Stake: 1, Confidence: 0.5, ConformanceDeficit: 1e9);
        var critical = new PendingDecision(Identity.New(), Stake: 10_000_000, Confidence: 0.0, ConformanceDeficit: 1.0);

        var scheduled = Scheduler.Schedule([critical, trivial], 5_000, 500_000);

        Assert.Equal(critical.DecisionId, scheduled[0].Decision.DecisionId);  // manipulasyon etkisiz
        Assert.Equal(0.5, scheduled[1].AttentionPriority, precision: 10);      // 1 x 0.5 x clamp(1e9)=1

        // NaN deficit KIRPILMAZ, REDDEDILIR: "olculemeyen acik" ile "acik yok" ayni sey degildir.
        Assert.Throws<ArgumentOutOfRangeException>(
            () => DecisionGravity.AttentionPriority(100, 0.5, double.NaN));
    }

    [Fact]
    public void AUDIT_FINDING_B5_demo_inputs_do_not_demonstrate_ConformanceDeficit_at_all()
    {
        // Demo bolum 2 sunu iddia ediyor:
        //   "Siralama olcutu keyfi DEGIL: AttentionPriority = InfoNeed x ConformanceDeficit"
        // Ama secilen UC girdi ile, ConformanceDeficit'i TAMAMEN SIFIRLASANIZ BILE siralama
        // AYNI kaliyor (tie-breaker InfoNeed devreye giriyor). Yani demo, iddia ettigi carpanin
        // siralamaya HICBIR katkisini gostermiyor: ayirt edici gucu sifir olan bir gosteri.
        // (Bu bir KOD kusuru degil, DEMO kusuru — kod duzeltmesiyle degismez, duruyor.)
        var a = Identity.New(); var b = Identity.New(); var c = Identity.New();

        var withDeficit = Scheduler.Schedule(
        [
            new PendingDecision(a, 800, 0.92, 0.10),
            new PendingDecision(b, 45_000, 0.55, 0.70),
            new PendingDecision(c, 250_000, 0.80, 0.40, IsIrreversible: true)
        ], 5_000, 60_000, 5_000, 40_000);

        var withoutDeficit = Scheduler.Schedule(
        [
            new PendingDecision(a, 800, 0.92, 0.0),
            new PendingDecision(b, 45_000, 0.55, 0.0),
            new PendingDecision(c, 250_000, 0.80, 0.0, IsIrreversible: true)
        ], 5_000, 60_000, 5_000, 40_000);

        Assert.Equal(
            withDeficit.Select(s => s.Decision.DecisionId).ToArray(),
            withoutDeficit.Select(s => s.Decision.DecisionId).ToArray());

        // Ve deficit'siz halde tum AttentionPriority'ler 0 — yani demo'nun "teoriden tureyen
        // olcut" dedigi nicelik, sistemin bugun gercekten uretebildigi veriyle (deficit = 0,
        // Scheduler.cs durust-sinir (b)) HER ZAMAN 0'dir.
        Assert.All(withoutDeficit, s => Assert.Equal(0.0, s.AttentionPriority, precision: 10));
    }

    [Fact]
    public void AUDIT_DEFECT_B6_ordering_is_input_order_dependent_when_priority_and_infoneed_tie()
    {
        // ACIK BIRAKILDI (bilincli). SchedulerTests "girdi sirasi sonucu degistirmez" diyor ama
        // bunu YALNIZCA InfoNeed'in farkli oldugu bir ornekle gosteriyor. Tam esitlikte sonuc
        // girdi sirasina bagli.
        //
        // NEDEN KAPATILMADI: kapatmak ucuncu bir tie-breaker (or. DecisionId ordinal siralamasi)
        // eklemeyi gerektirir. ENS-3022 boyle bir olcut TANIMLAMIYOR; teoride karsiligi olmayan
        // bir siralama kurali UYDURMAK, Anayasa Madde IX'un yasakladigi seydir ("teoriyi
        // uyguluyorum, yoksa uyduruyor muyum?"). Once teori borcu, sonra kod. Scheduler.cs
        // durust-sinir (e) ve README'de isaretli.
        var x = new PendingDecision(Identity.New(), 100, 0.5, 0.5);
        var y = new PendingDecision(Identity.New(), 100, 0.5, 0.5);

        var first = Scheduler.Schedule([x, y], 10, 1000);
        var second = Scheduler.Schedule([y, x], 10, 1000);

        Assert.Equal(x.DecisionId, first[0].Decision.DecisionId);
        Assert.Equal(y.DecisionId, second[0].Decision.DecisionId); // sira degisti
    }

    // ========================================================================================
    // C. LLM TIER SELECTOR — demo'nun "durust bulgu"sunu curutme
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_C1_positional_call_silently_binds_to_the_wrong_overload()
    {
        // ACIK BIRAKILDI (bilincli, kapsam disi). IKI overload var:
        //   M1: SelectTier(double infoNeed, double complexThreshold = 10, double criticalThreshold = 40)
        //   M2: SelectTier(double stake, double? confidence, double complexThreshold = 10, ...)
        // IKI pozisyonel `double` argumanla cagrildiginda C# overload cozumlemesi M1'i secer
        // (double -> double, double -> double? 'tan daha iyi donusum hedefidir). Yani
        // `SelectTier(stake, conf)` aslinda `SelectTier(infoNeed: stake, complexThreshold: conf)`
        // demektir — InfoNeed HIC HESAPLANMAZ.
        //
        // NEDEN KAPATILMADI: dogru duzeltme convenience overload'i YENIDEN ADLANDIRMAKTIR
        // (`SelectTierFor(stake, confidence)`, AUDIT 7/2). Bu bir public API kirilmasidir ve
        // bu turun gorevi AUDIT 5'teki KERNEL kusurlariydi; C1 DEMO-kapsamli bir tuzak
        // (kernel dogru overload'a bagliyor — AUDIT_HOLDS_C2). Ayri bir artimda ele alinmali.
        var byFormula = LlmTierSelector.SelectTier(DecisionGravity.InfoNeed(500.0, 0.95)); // InfoNeed = 25
        var named = LlmTierSelector.SelectTier(stake: 500.0, confidence: 0.95);
        var positional = LlmTierSelector.SelectTier(500.0, 0.95);

        Assert.Equal(LlmTier.Complex, byFormula);   // 10 <= 25 < 40 -> Complex (KESIN)
        Assert.Equal(LlmTier.Complex, named);       // adlandirilmis cagri dogru overload'a gider
        Assert.Equal(LlmTier.Critical, positional); // BULGU: pozisyonel cagri yanlis overload'a gider
    }

    [Fact]
    public void AUDIT_HOLDS_C2_Scheduler_uses_the_correct_overload()
    {
        // Scheduler uc `double` argumanla cagiriyor -> M1'e (infoNeed overload'i) dogru sekilde
        // baglaniyor. Yani KERNEL dogru, yalnizca DEMO yanlis.
        var d = new PendingDecision(Identity.New(), Stake: 500, Confidence: 0.95, ConformanceDeficit: 0.5);
        var s = Scheduler.Schedule([d], 10, 100_000, complexThreshold: 10, criticalThreshold: 40)[0];

        Assert.Equal(LlmTier.Complex, s.Tier);
        Assert.Equal(25.0, s.Gate.InfoNeed, precision: 6);
    }

    // ========================================================================================
    // D. PROOF-TRACE — "izsiz turetim TEMSIL EDILEMEZ" iddiasini kirma
    // ========================================================================================

    private static ProofTrace SampleTrace() => new("R", "C", [new Premise("p", 0.8)]);

    [Fact]
    public void AUDIT_HOLDS_D1_fuzz_500_traces_confidence_is_exactly_the_hand_computed_min()
    {
        var rng = new Random(4025); // ENS-4025
        for (int i = 0; i < 500; i++)
        {
            int n = rng.Next(1, 8);
            var confs = Enumerable.Range(0, n).Select(_ => rng.NextDouble()).ToArray();
            var premises = confs.Select((c, k) => new Premise($"p{k}", c)).ToList();

            var trace = new ProofTrace("R", "C", premises);

            double handMin = confs[0];
            foreach (var c in confs) if (c < handMin) handMin = c;   // elle min, LINQ'siz

            Assert.Equal(handMin, trace.Confidence, precision: 12);
        }
    }

    [Fact]
    public void AUDIT_HOLDS_D2_confidence_has_no_setter_and_no_init_accessor()
    {
        var prop = typeof(ProofTrace).GetProperty(nameof(ProofTrace.Confidence))!;
        Assert.False(prop.CanWrite);                       // `with { Confidence = ... }` derlenmez
        var backing = typeof(ProofTrace)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(f => f.Name.Contains("Confidence"));
        Assert.True(backing.IsInitOnly);                   // C# duzeyinde gercekten immutable
    }

    [Fact]
    public void AUDIT_FIXED_D3_premises_can_no_longer_be_emptied_after_construction()
    {
        // KUSURDU: "Onculsuz proof-trace KURULAMAZ" dogruydu — ama kurulduktan sonra
        //   BOSALTILABILIYORDU. `Premises` bir `IReadOnlyList<Premise>` olarak donuyordu ama
        //   arkasindaki nesne canli bir `List<Premise>`'ti; reflection GEREKMEDEN tek satirlik
        //   downcast yetiyordu ve `Confidence` eski degerinde donmus kaliyordu.
        // KAPANDI: ProofTrace artik `ReadOnlyCollection<Premise>` doner — downcast derlenir ama
        //   CALISMA ZAMANINDA InvalidCastException atar; invariant post-construction da gecerli.
        var trace = new ProofTrace("BAG-P3", "Action enacted", [new Premise("plan", 0.8)]);
        Assert.Single(trace.Premises);

        Assert.Throws<InvalidCastException>(() => ((List<Premise>)trace.Premises).Clear());

        Assert.Single(trace.Premises);
        Assert.Equal(0.8, trace.Confidence, precision: 10);
    }

    [Fact]
    public void AUDIT_FIXED_D4_NaN_confidence_premise_is_now_rejected()
    {
        // KUSURDU: `confidence is < 0 or > 1` NaN'da HER IKI karsilastirmada false donuyordu ->
        //   guard deliniyordu. ENS-4025 L7 t-norm'unun NaN altindaki davranisi ne teoride ne
        //   kodda tanimliydi — tanimsiz davranis sessizce uretiliyordu.
        // KAPANDI: Guard.UnitInterval (ayni kok neden, ayni yardimci) — olculemeyen oncul
        //   L7 t-norm'una hic giremez.
        Assert.Throws<ArgumentOutOfRangeException>(() => new Premise("olculemeyen-oncul", double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Premise("sonsuz-oncul", double.PositiveInfinity));

        // Ve saglikli oncullerle L7 min t-norm aynen calisiyor (guard cok genis degil):
        var trace = new ProofTrace("R", "C", [new Premise("a", 0.9), new Premise("b", 0.4)]);
        Assert.Equal(0.4, trace.Confidence, precision: 12);
    }

    [Fact]
    public void AUDIT_FINDING_D5_the_trace_invariant_is_only_about_cardinality_not_evidence()
    {
        // "Izsiz action imkansiz" iddiasinin GERCEK gucu: en az BIR bos-olmayan string.
        // Asagidaki trace tamamen uydurma ama tum invariant'lari geciyor ve confidence 1.00.
        // ProofTrace.cs durust-sinir (b) bunu kabul ediyor; DEMO'nun ozet bolumu etmiyor.
        // (Kapatilmasi Premise'in ENS-4010 node'una TIPLI referans tasimasini gerektirir —
        //  Faz-4'un acik borcu, teori tarafi da eksik.)
        var fabricated = new ProofTrace(
            ruleId: "uydurma-kural",
            conclusion: "her sey yolunda",
            premises: [new Premise("kanit-yok", 1.0)]);

        Assert.Equal(1.0, fabricated.Confidence, precision: 10);
        Assert.Single(fabricated.Premises);
    }

    // ========================================================================================
    // E. ACTUATION LAYER — state machine'den kacis yolu arama
    // ========================================================================================

    private static ActuationLayer DriveTo(ActionState target)
    {
        var layer = new ActuationLayer(Identity.New());
        if (target == ActionState.Planned) return layer;

        var allowing = new GateResult(GateDecision.Autonomous, "ok", 0);
        var blocking = new GateResult(GateDecision.CriticalBlock, "blok", 0);

        if (target == ActionState.Blocked) { layer.ApplyGate(blocking, T0); return layer; }

        layer.ApplyGate(allowing, T0);
        if (target == ActionState.Contextualized) return layer;
        layer.BeginActing(T0);
        if (target == ActionState.Acting) return layer;
        if (target == ActionState.Failed) { layer.Fail(T0, "timeout"); return layer; }
        layer.Observe(T0);
        if (target == ActionState.Observed) return layer;
        layer.RecordTrace(SampleTrace(), T0);
        if (target == ActionState.Traced) return layer;
        layer.RecordLearning(T0);
        if (target == ActionState.Learned) return layer;
        layer.Remember(T0);
        return layer;
    }

    [Fact]
    public void AUDIT_HOLDS_E1_exhaustive_transition_matrix_has_no_leak()
    {
        // Beklenen gecis tablosunu KODDAN OKUMADAN, ADR-0001 5.4'ten bagimsiz olarak burada
        // yeniden tanimliyoruz; her durumdan HER metodu deniyoruz. Izin verilmeyen tek bir
        // gecisin bile sessizce kabul edilmemesi gerekiyor.
        var expected = new Dictionary<ActionState, string[]>
        {
            [ActionState.Planned] = ["ApplyGate"],
            [ActionState.Blocked] = [],
            [ActionState.Contextualized] = ["BeginActing"],
            [ActionState.Acting] = ["Observe", "Fail"],
            [ActionState.Observed] = ["RecordTrace"],
            [ActionState.Failed] = ["RecordTrace"],
            [ActionState.Traced] = ["RecordLearning"],
            [ActionState.Learned] = ["Remember"],
            [ActionState.Remembered] = []
        };

        string[] allMoves =
            ["ApplyGate", "BeginActing", "Observe", "Fail", "RecordTrace", "RecordLearning", "Remember"];

        foreach (var (state, allowed) in expected)
        {
            foreach (var move in allMoves)
            {
                var layer = DriveTo(state);
                bool threw = false;
                try
                {
                    switch (move)
                    {
                        case "ApplyGate": layer.ApplyGate(new GateResult(GateDecision.Autonomous, "x", 0), T0); break;
                        case "BeginActing": layer.BeginActing(T0); break;
                        case "Observe": layer.Observe(T0); break;
                        case "Fail": layer.Fail(T0, "r"); break;
                        case "RecordTrace": layer.RecordTrace(SampleTrace(), T0); break;
                        case "RecordLearning": layer.RecordLearning(T0); break;
                        case "Remember": layer.Remember(T0); break;
                    }
                }
                catch (InvalidTransitionException) { threw = true; }

                bool shouldBeAllowed = allowed.Contains(move);
                Assert.True(shouldBeAllowed != threw,
                    $"{state} durumunda '{move}': beklenen={(shouldBeAllowed ? "izinli" : "RED")}, " +
                    $"gerceklesen={(threw ? "RED" : "izinli")}");
            }
        }
    }

    [Fact]
    public void AUDIT_HOLDS_E2_swallowing_exceptions_in_a_loop_does_not_advance_the_state()
    {
        // "Hatayi yakala, tekrar dene" saldirisi: 1000 kez gecersiz gecis denemesi state'i
        // ilerletmiyor ve audit gecmisini kirletmiyor.
        var layer = DriveTo(ActionState.Planned);
        for (int i = 0; i < 1000; i++)
        {
            try { layer.BeginActing(T0); } catch (InvalidTransitionException) { }
            try { layer.Remember(T0); } catch (InvalidTransitionException) { }
            try { layer.RecordTrace(SampleTrace(), T0); } catch (InvalidTransitionException) { }
        }

        Assert.Equal(ActionState.Planned, layer.State);
        Assert.Empty(layer.History);
        Assert.Null(layer.Trace);
    }

    [Fact]
    public void AUDIT_DEFECT_E3_the_gate_result_can_be_forged_in_one_line()
    {
        // ACIK BIRAKILDI (bilincli). State machine yalnizca "bir GateResult NESNESI verildi mi"yi
        // kontrol eder; o nesnenin BoundedAutonomyGate.Evaluate'ten geldigini DOGRULAMAZ.
        // `GateResult` public bir record — herkes istedigi karari uretebilir. Yani "Gate'siz
        // action imkansiz" degil; "GateResult nesnesiz action imkansiz".
        //
        // NEDEN KAPATILMADI: kapatmak imzali/opak bir gate-token'i (or. HMAC'li, yalnizca
        // Evaluate'in uretebildigi bir kanit nesnesi) gerektirir — bu, ADR-0001'de KARARI
        // VERILMEMIS bir mimari eklemedir (yeni tip, anahtar yonetimi, serializasyon).
        // Madde VIII: yalnizca Accepted ADR'ye dayanilir; teori/ADR borcu once. AUDIT 7/6
        // ile birlikte Faz-5 kapi sarti olarak isaretli (README).
        var forged = new GateResult(GateDecision.Autonomous, "gate hic calistirilmadi", 0.0);

        var layer = new ActuationLayer(Identity.New());
        layer.ApplyGate(forged, T0);
        Assert.Equal(ActionState.Contextualized, layer.State);

        layer.BeginActing(T0);
        Assert.Equal(ActionState.Acting, layer.State); // P7 politikasi hic degerlendirilmeden kostu
    }

    [Fact]
    public void AUDIT_FIXED_E4_audit_history_can_no_longer_be_erased_via_downcast()
    {
        // KUSURDU: "Audit gecmisi: N gecis, hicbiri atlanamadi" deniyordu — ama gecmisin kendisi
        //   silinebiliyordu. `History` canli bir `List<ActionTransition>` donduruyordu; reflection
        //   gerekmiyordu. Action "tamamlandi" gorunurken izi yok olabiliyordu.
        // KAPANDI: `ReadOnlyCollection<ActionTransition>` gorunumu doner — downcast calisma
        //   zamaninda InvalidCastException atar, gecmis canli kalir.
        var layer = DriveTo(ActionState.Remembered);
        Assert.Equal(7, layer.History.Count);

        Assert.Throws<InvalidCastException>(() => ((List<ActionTransition>)layer.History).Clear());

        Assert.Equal(7, layer.History.Count);                // audit izi ayakta
        Assert.Equal(ActionState.Remembered, layer.State);
        Assert.Equal(ActionState.Planned, layer.History[0].From);
        Assert.Equal(ActionState.Remembered, layer.History[^1].To);
    }

    [Fact]
    public void AUDIT_DEFECT_E5_state_can_be_teleported_by_reflection_leaving_no_audit_trail()
    {
        // ACIK BIRAKILDI (kapatilamaz). Reflection .NET'te her seyi yener — bu tek basina agir
        // bir suclama degil; kapatmak ancak proses/AppDomain izolasyonu ya da dogrulanmis
        // event-store ile mumkundur (ADR-0001 6'nin sandbox borcu). Kayit: demo "atlanamaz"
        // diyor; dogrusu "normal API uzerinden atlanamaz".
        var layer = new ActuationLayer(Identity.New());
        var stateField = typeof(ActuationLayer)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(f => f.FieldType == typeof(ActionState));

        stateField.SetValue(layer, ActionState.Acting);

        Assert.Equal(ActionState.Acting, layer.State);
        Assert.Empty(layer.History);   // Gate hic uygulanmadi, hicbir iz yok
        layer.Observe(T0);             // ve buradan itibaren normal akis devam ediyor
        Assert.Equal(ActionState.Observed, layer.State);
    }

    [Fact]
    public void AUDIT_FIXED_E6_a_trace_can_no_longer_be_emptied_before_being_recorded()
    {
        // KUSURDU: D3 + E birlesimi — "izsiz action imkansiz" iddiasi, KURULDUKTAN SONRA
        //   bosaltilmis bir trace ile geciliyordu; katman onculsuz bir "kanit"i kabul ediyordu.
        // KAPANDI: D3'un kapanisi bu zinciri de kesti — trace bosaltilamadigi icin katmana
        //   onculsuz trace ULASAMAZ.
        var trace = new ProofTrace("R", "C", [new Premise("p", 0.9)]);
        Assert.Throws<InvalidCastException>(() => ((List<Premise>)trace.Premises).Clear());

        var layer = DriveTo(ActionState.Observed);
        layer.RecordTrace(trace, T0);

        Assert.Equal(ActionState.Traced, layer.State);
        Assert.Single(layer.Trace!.Premises);   // oncul hala orada
        Assert.Equal(0.9, layer.Trace!.Confidence, precision: 10);
    }

    // ========================================================================================
    // F. CAPABILITY REGISTRY — yetkisiz araci gecirmeye calisma
    // ========================================================================================

    private static CapabilityRegistry OpsRegistry()
    {
        var r = new CapabilityRegistry();
        r.Register(new CapabilityPack("Operations", "1.2",
            allowedTools: ["read_stock", "create_purchase_order"],
            requiresHumanApprovalFor: ["create_purchase_order"]));
        return r;
    }

    [Fact]
    public void AUDIT_HOLDS_F1_lookalike_and_injected_tool_names_are_all_rejected()
    {
        // StringComparer.Ordinal dogru secim: case-insensitive ya da normalize eslesme YOK.
        string[] lookalikes =
        [
            "READ_STOCK",
            "Read_Stock",
            "read_stock ",              // sondaki bosluk
            " read_stock",              // bastaki bosluk
            "read-stock",
            "read_stock\t",
            "read​_stock",         // zero-width space enjeksiyonu
            "rеad_stock",          // Kiril 'e' homoglifi
            "read_stock ",         // non-breaking space
            "read_stock\0",             // NUL enjeksiyonu
            "read_stock\n",
            "read__stock"
        ];

        var registry = OpsRegistry();
        foreach (var name in lookalikes)
            Assert.False(registry.Authorize(name).IsAllowed, $"Yetki sizdi: '{name}'");
    }

    [Fact]
    public void AUDIT_FIXED_F2_allowed_tools_set_can_no_longer_be_extended_after_registration()
    {
        // KUSURDU: `AllowedTools` bir `IReadOnlySet<string>` olarak sunuluyordu ama arkasinda
        //   canli bir `HashSet<string>` vardi. Reflection GEREKMIYORDU. Kayitli bir Pack'e
        //   SONRADAN yetki eklenebiliyordu ve registry bunu fark etmiyordu — "deklaratif
        //   izinler" runtime'da mutable idi (yetki kacagi).
        // KAPANDI: `FrozenSet<string>` — downcast calisma zamaninda InvalidCastException atar;
        //   ayrica ctor savunmaci kopya alir (cagiranin kumesi degisse de Pack degismez).
        var mutableSource = new HashSet<string>(StringComparer.Ordinal) { "read_stock" };
        var pack = new CapabilityPack("Operations", "1.2", mutableSource);
        var registry = new CapabilityRegistry();
        registry.Register(pack);

        Assert.False(registry.Authorize("delete_database").IsAllowed);

        // (a) Cikti downcast'i: artik imkansiz.
        Assert.Throws<InvalidCastException>(() => ((HashSet<string>)pack.AllowedTools).Add("delete_database"));

        // (b) Girdi aliasing'i: kaynak kume degisse bile Pack etkilenmez (savunmaci kopya).
        mutableSource.Add("delete_database");

        Assert.False(registry.Authorize("delete_database").IsAllowed);   // yetki kacagi kapali
        Assert.True(registry.Authorize("read_stock").IsAllowed);
    }

    [Fact]
    public void AUDIT_FIXED_F3_disabling_a_strict_pack_no_longer_removes_the_human_approval_guard()
    {
        // KUSURDU: `Disable` "yeti kaldirma" olarak sunuluyordu; ama bir araci BASKA bir Pack de
        //   veriyorsa, kati Pack'i devre disi birakmak yetkiyi kaldirmiyor — yalnizca INSAN
        //   ONAYI SARTINI kaldiriyordu. Bir guvenlik kontrolu, bir "kapasite kapatma" jestiyle
        //   sessizce dusuyordu.
        // KAPANDI: YETKI yalnizca ETKIN Pack'lerden gelir, ONAY SARTI ise KAYITLI TUM Pack'lerden
        //   toplanir (devre-disi olanlar dahil). Asimetri kasitli: Disable yetki kaldirir,
        //   KISIT KALDIRMAZ.
        var registry = new CapabilityRegistry();
        registry.Register(new CapabilityPack("Loose", "1.0", ["cancel_contract"]));
        registry.Register(new CapabilityPack("Strict", "1.0", ["cancel_contract"], ["cancel_contract"]));

        Assert.True(registry.Authorize("cancel_contract").RequiresHumanApproval);

        registry.Disable("Strict");

        var after = registry.Authorize("cancel_contract");
        Assert.True(after.IsAllowed);                 // Loose hala veriyor
        Assert.True(after.RequiresHumanApproval);     // ama P7 korumasi AYAKTA
        Assert.Contains("DEVRE DIŞI", after.Reason);  // ve neden oyle oldugu aciklaniyor

        // Gercekten yeti kaldirmak isteyen HER IKI Pack'i de kapatmali:
        registry.Disable("Loose");
        Assert.False(registry.Authorize("cancel_contract").IsAllowed);
    }

    [Fact]
    public void AUDIT_FIXED_F4_authorization_result_is_now_consumed_by_the_gate()
    {
        // KUSURDU: ADR-0001 6.1'in iddia ettigi ENS deltasi — "deklaratif izinler DOGRUDAN
        //   Bounded-Autonomy Gate'e beslenir" — KODDA YOKTU. `BoundedAutonomyGate.Evaluate`
        //   imzasinda ToolAuthorization/CapabilityRegistry/CapabilityPack parametresi yoktu;
        //   demo ikisini yan yana YAZDIRIP birbirine BAGLAMIYORDU. ENS'in prior-art'a karsi
        //   en dar ve en spesifik ozgunluk iddiasi gerceklesmemisti.
        // KAPANDI: Evaluate artik opsiyonel bir `ToolAuthorization` aliyor ve iki yonde zorluyor.
        var evaluate = typeof(BoundedAutonomyGate).GetMethod(nameof(BoundedAutonomyGate.Evaluate))!;
        Assert.Contains(typeof(ToolAuthorization), evaluate.GetParameters().Select(p => p.ParameterType));

        var registry = OpsRegistry();

        // (1) Yetkisiz arac -> BLOK, InfoNeed ne kadar dusuk olursa olsun.
        var unauthorized = BoundedAutonomyGate.Evaluate(
            stake: 1, confidence: 0.99, conformanceDeficit: 0, isIrreversible: false,
            autonomyThreshold: 1_000, blockThreshold: 10_000,
            toolAuthorization: registry.Authorize("delete_database"));
        Assert.Equal(GateDecision.Blocked, unauthorized.Decision);

        // (2) Onay-gerektiren arac -> asla Autonomous; InfoNeed tek basina Autonomous derdi.
        var needsApproval = BoundedAutonomyGate.Evaluate(
            stake: 1, confidence: 0.99, conformanceDeficit: 0, isIrreversible: false,
            autonomyThreshold: 1_000, blockThreshold: 10_000,
            toolAuthorization: registry.Authorize("create_purchase_order"));
        Assert.Equal(GateDecision.Blocked, needsApproval.Decision);
        Assert.Contains("P7", needsApproval.Reason);

        // (3) Serbest arac -> gate'i GEVSETMEZ ama sikilastirmaz da; InfoNeed ne diyorsa o.
        var free = BoundedAutonomyGate.Evaluate(
            stake: 1, confidence: 0.99, conformanceDeficit: 0, isIrreversible: false,
            autonomyThreshold: 1_000, blockThreshold: 10_000,
            toolAuthorization: registry.Authorize("read_stock"));
        Assert.Equal(GateDecision.Autonomous, free.Decision);

        // (4) Bag Scheduler uzerinden de akiyor (uctan uca).
        var pending = new PendingDecision(Identity.New(), 1, 0.99, 0,
            ToolAuthorization: registry.Authorize("create_purchase_order"));
        Assert.Equal(GateDecision.Blocked, Scheduler.Schedule([pending], 1_000, 10_000)[0].Gate.Decision);

        // (5) Ve yetkilendirme gate'i ASLA GEVSETEMEZ: geri-donulemezlik hala kazanir.
        var irreversible = BoundedAutonomyGate.Evaluate(
            1, 0.99, 0, isIrreversible: true, 1_000, 10_000, registry.Authorize("read_stock"));
        Assert.Equal(GateDecision.CriticalBlock, irreversible.Decision);
    }

    // ========================================================================================
    // G. COMPANY MEMORY — "asla silinmez", decay yasasi, curator
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_G1_records_can_no_longer_be_deleted_via_downcast()
    {
        // KUSURDU: "Kayit eklenir, asla silinmez (3, audit)" deniyordu ama `AllRecords` canli bir
        //   `List<MemoryRecord>` donduruyordu — kurumsal bellek tek satirda silinebiliyordu.
        // KAPANDI: `ReadOnlyCollection<MemoryRecord>` gorunumu; downcast InvalidCastException.
        var memory = MemoryAt(T0);
        memory.Record(new MemoryRecord(Identity.New(), "x", 5, 0.5, T0));
        memory.Record(new MemoryRecord(Identity.New(), "x", 8, 0.9, T0));
        Assert.Equal(2, memory.AllRecords.Count);

        Assert.Throws<InvalidCastException>(() => ((List<MemoryRecord>)memory.AllRecords).Clear());

        Assert.Equal(2, memory.AllRecords.Count);   // audit invariant'i ayakta
    }

    [Fact]
    public void AUDIT_FIXED_G2_Verify_can_no_longer_freeze_decay_with_a_future_timestamp()
    {
        // KUSURDU: Yeniden-dogrulama HICBIR kanit istemiyor ve GELECEK bir tarih kabul ediyordu.
        //   ageDays = max(0, negatif) = 0 -> decayFactor kalici olarak 1.0. Decay yasasi tek bir
        //   Verify cagrisiyla, denetimsiz bicimde, iz birakmadan devre disi birakilabiliyordu ve
        //   kayit Curator'a bir daha gorunmuyordu.
        // KAPANDI (uc katman):
        //   (a) gelecek tarihli damga REDDEDILIR (CompanyMemory'ye enjekte edilen saate gore),
        //   (b) NEDENSELLIK: `asOf` anindaki sorgu, `asOf`'tan SONRAKI bir dogrulamadan
        //       yararlanamaz (saat kontrolu atlansa bile decay geriye donuk kapatilamaz),
        //   (c) her Verify gerekce ister ve `Verifications` audit izine yazilir.
        var memory = MemoryAt(T0);
        var rec = new MemoryRecord(Identity.New(), "x", 5, 0.1, T0.AddDays(-10_000));
        memory.Record(rec);

        // NOT (v0.4.0): karsilastirma tabani `RetentionPriority` (= |L|) degil `CapitalValue`
        // (= |L|*c) olmali — Salience artik value x decayFactor'dur (ENS-2003 v0.4.0 §3a).
        Assert.True(memory.Salience(rec, T0, 0.01) < rec.CapitalValue * 0.001);

        // (a) gelecek tarihli dogrulama REDDEDILIYOR
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => memory.Verify(rec, T0.AddYears(100), "sahte tazeleme"));
        Assert.Contains("GELECEK", ex.Message);

        // kayit hala bayat: Curator onu GOREBILIYOR
        Assert.Contains(rec, memory.FindStale(T0, 0.01, 0.5));

        // (c) mesru dogrulama calisiyor ve iz birakiyor
        memory.Verify(rec, T0.AddDays(-1), "tedarikci ile teyit edildi");
        // v0.4.0: sonum hizi artik `c`'ye bagli DEGIL — eski beklenen deger `exp(-0.01*(1-0.1)*1)`
        // idi (lambda(c) = lambda_base*(1-c)^gamma), yenisi `exp(-0.01*1)` (lambda_pi dogrudan).
        Assert.Equal(rec.CapitalValue * Math.Exp(-0.01 * 1), memory.Salience(rec, T0, 0.01), precision: 9);
        Assert.Single(memory.Verifications);
        Assert.Equal("tedarikci ile teyit edildi", memory.Verifications[0].Evidence);

        // (b) nedensellik: 5 gun ONCEKI bir sorgu, DUNKU dogrulamadan yararlanamaz
        double asOfBefore = memory.Salience(rec, T0.AddDays(-5), 0.01);
        Assert.True(asOfBefore < rec.CapitalValue * 0.001,
            $"Gecmise donuk sorgu gelecekteki dogrulamayla tazelenmis: {asOfBefore}");

        // gerekcesiz dogrulama da reddediliyor
        Assert.Throws<ArgumentException>(() => memory.Verify(rec, T0, "   "));
    }

    [Fact]
    public void AUDIT_FIXED_G3_verification_clock_is_now_keyed_by_record_not_by_DecisionId()
    {
        // KUSURDU: `Verify` DecisionId ile anahtarliydi. Iki farkli MemoryRecord ayni DecisionId'yi
        //   tasiyorsa (ayni karardan iki ogrenim), birini dogrulamak DIGERININ de decay saatini
        //   sifirliyordu — capraz-kirlenme.
        // KAPANDI: `Verify` artik KAYDIN KENDISINI aliyor; saat kayit bazinda tutuluyor.
        var memory = MemoryAt(T0);
        var id = Identity.New();
        var a = new MemoryRecord(id, "tip-A", 5, 0.5, T0.AddDays(-1000));
        var b = new MemoryRecord(id, "tip-B", 5, 0.5, T0.AddDays(-1000));
        memory.Record(a); memory.Record(b);

        memory.Verify(a, T0, "A icin kanit tazelendi");

        Assert.Equal(T0, memory.LastVerifiedOf(a));
        Assert.Equal(T0.AddDays(-1000), memory.LastVerifiedOf(b));   // B'ye BULASMADI
        Assert.True(memory.Salience(a, T0, 0.01) > memory.Salience(b, T0, 0.01));

        // Kayitli olmayan bir kaydi dogrulamak da artik acik hata (hayalet dogrulama yok).
        var ghost = new MemoryRecord(Identity.New(), "tip-C", 5, 0.5, T0);
        Assert.Throws<InvalidOperationException>(() => memory.Verify(ghost, T0, "hayalet"));
    }

    [Fact]
    public void AUDIT_FIXED_G4_an_invalid_record_is_rejected_at_construction_not_at_retrieval()
    {
        // KUSURDU: `MemoryRecord` constructor'i DOGRULAMA YAPMIYORDU; dogrulama `RetentionPriority`
        //   erisiminde (DecisionCapital.Value) yapiliyordu. Gecersiz kayit sessizce yaziliyor,
        //   sonra HER retrieval'i patlatiyordu — bellegin TAMAMI icin servis-disi birakma.
        // KAPANDI: dogrulama kurucuya tasindi; gecersiz kayit bellege HIC GIREMEZ.
        Assert.Throws<ArgumentOutOfRangeException>(() => new MemoryRecord(Identity.New(), "x", -1, 0.5, T0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new MemoryRecord(Identity.New(), "x", 5, 1.5, T0));
        Assert.Throws<ArgumentException>(() => new MemoryRecord(Identity.New(), "   ", 5, 0.5, T0));

        // Ve saglikli bellek zehirlenmiyor: retrieval calismaya devam ediyor.
        var memory = MemoryAt(T0);
        memory.Record(new MemoryRecord(Identity.New(), "x", 5, 0.9, T0));
        Assert.Single(memory.Retrieve("x", T0));
        Assert.Empty(memory.FindStale(T0, 0.01));
    }

    [Fact]
    public void AUDIT_FIXED_G5_NaN_attribution_confidence_can_no_longer_create_an_invisible_record()
    {
        // KUSURDU: NaN, [0,1] guard'ini geciyordu (A3 ile ayni kok neden). Sonuc:
        //   RetentionPriority = NaN -> salience NaN -> siralamada EN SONA duser ve
        //   FindStale'in `RetentionPriority > 0` filtresi NaN'da false -> ASLA bayraklanmaz.
        //   Kayit ne bulunuyor ne de "gozden gecir" listesine giriyordu: SESSIZ KURUMSAL AMNEZI.
        // KAPANDI: kurucuda Guard.UnitInterval — NaN kayit hic olusturulamaz.
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new MemoryRecord(Identity.New(), "x", 10, double.NaN, T0.AddDays(-5000)));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new MemoryRecord(Identity.New(), "x", double.NaN, 0.5, T0));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new MemoryRecord(Identity.New(), "x", double.PositiveInfinity, 0.5, T0));

        // Ayni kok neden yasada da kapali:
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(10, double.NaN));
    }

    [Fact]
    public void AUDIT_FIXED_G6_zero_value_records_are_now_flagged_stale()
    {
        // KUSURDU: FindStale `RetentionPriority > 0` sarti koyuyordu. `|Learning| = 0` ya da
        //   `confidence = 0` olan bir kayit 100 yil beklese de Curator'in gozden gecirme listesine
        //   ASLA girmiyordu — ustelik tam da bu kayitlar gozden gecirilmeye en muhtac olanlar.
        // KAPANDI: stale-yargisi artik SAF tazelik ekseninde (DecayFactor) hesaplaniyor; bolme ve
        //   retention filtresi kalkti (SKR-040/D2'nin ortogonalite gerekcesi de boylece korunuyor).
        var memory = MemoryAt(T0);
        var zeroMagnitude = new MemoryRecord(Identity.New(), "x", 0.0, 0.5, T0.AddDays(-36_500));
        var zeroConfidence = new MemoryRecord(Identity.New(), "x", 7.0, 0.0, T0.AddDays(-36_500));
        var fresh = new MemoryRecord(Identity.New(), "x", 7.0, 0.9, T0);
        memory.Record(zeroMagnitude); memory.Record(zeroConfidence); memory.Record(fresh);

        var stale = memory.FindStale(T0, 0.01, 0.5);

        Assert.Contains(zeroMagnitude, stale);
        Assert.Contains(zeroConfidence, stale);
        Assert.DoesNotContain(fresh, stale);      // esik hala ayirt edici, "hepsini bayrakla" degil
        Assert.Equal(3, memory.AllRecords.Count); // ve hicbiri silinmedi (3b, P7)
    }

    [Fact]
    public void AUDIT_FIXED_G7_demo_curator_sweep_now_flags_the_stale_record_but_for_a_reason_the_finding_did_not_predict()
    {
        // KUSURDU (v0.3.1): Demo bolum 6, curator sweep'i "bayat kayit BAYRAKLANDI (silinmedi)"
        //   diye anlatiyordu ama bayraklanan kayit sayisi SIFIR idi. Sebep kil payiydi: eski
        //   kaydin `c = 0.85` oldugu icin sonum hizi lambda = 0.01*(1-0.85) = 0.0015'e DUSUYOR ve
        //   decayFactor = exp(-0.0015 x 460) yaklasik 0.5016 > 0.5 kaliyordu. Bulgu "KOD kusuru
        //   degil, DEMO VERISI secimi" diye kaydedilmisti.
        // KAPANDI (v0.4.0) — ama tahmin edilen sebeple DEGIL: bulgu, demo verisinin degismesini
        //   bekliyordu; gerceklesen, YASANIN degismesi oldu. `c` sonumden cikinca yuksek-confidence
        //   ARTIK sonumu yavaslatmiyor: lambda_pi = 0.01 (tam), decayFactor = exp(-4.6) = 0.010.
        //   Yani "yuksek confidence ile bayatliktan kacma" yolu kapandi — bu, tam olarak
        //   AUDIT-WAVE2/D-5'in cift-sayim bulgusunun Curator tarafindaki gorunumuydu.
        var now = T0;
        var asOf = now.AddDays(60);
        var memory = MemoryAt(asOf);
        memory.Record(new MemoryRecord(Identity.New(), "tedarikci-secimi", 8.0, 0.60, now.AddDays(30)));
        var old = new MemoryRecord(Identity.New(), "tedarikci-secimi", 2.0, 0.85, now.AddDays(-400));
        memory.Record(old);

        var stale = memory.FindStale(asOf, contextDecayRate: 0.01, staleThreshold: 0.5);
        Assert.Single(stale);
        Assert.Contains(old, stale);

        double freshness = memory.DecayFactor(old, asOf, 0.01);
        Assert.Equal(Math.Exp(-0.01 * 460), freshness, precision: 12);
        Assert.True(freshness < 0.02, $"tazelik = {freshness}");

        // Ve kayit silinmedi (3b, P7) — bayrak bir imha emri degil.
        Assert.Equal(2, memory.AllRecords.Count);
    }

    [Fact]
    public void AUDIT_FINDING_G8_demo_memory_ordering_is_confounded_age_not_isolated()
    {
        // Demo bolum 6: "|Learning| = 8 one cikiyor — karsi-survivorship". Ama secilen iki
        // kayitta kazanan HEM daha buyuk |Learning|'e HEM de 430 gun daha taze olmaya sahip.
        // Gosteri iki nedeni ayristirmiyor: siralama yalniz tazelikle de aciklanabilir.
        var now = T0; var asOf = now.AddDays(60);

        var demoLike = MemoryAt(asOf);
        var bigFresh = new MemoryRecord(Identity.New(), "t", 8.0, 0.60, now.AddDays(30));
        var smallOld = new MemoryRecord(Identity.New(), "t", 2.0, 0.85, now.AddDays(-400));
        demoLike.Record(bigFresh); demoLike.Record(smallOld);
        Assert.Equal(bigFresh, demoLike.Retrieve("t", asOf, 0.01)[0]);

        // Kontrollu versiyon: yas SABIT tutulunca iddia gercekten saglam mi? Evet.
        var controlled = MemoryAt(asOf);
        var big = new MemoryRecord(Identity.New(), "t", 8.0, 0.60, now.AddDays(-100));
        var small = new MemoryRecord(Identity.New(), "t", 2.0, 0.85, now.AddDays(-100));
        controlled.Record(small); controlled.Record(big);
        Assert.Equal(big, controlled.Retrieve("t", asOf, 0.01)[0]);

        // Ama karsi-ornek de var: yeterince yuksek confidence + yeterli yas farkinda
        // dusuk-|Learning| kayit one gecebilir. "Ogrenim buyuklugu belirler" MUTLAK degil.
        var counter = MemoryAt(asOf);
        var bigButStale = new MemoryRecord(Identity.New(), "t", 8.0, 0.0, now.AddDays(-2000));
        var smallButFresh = new MemoryRecord(Identity.New(), "t", 2.0, 0.99, now);
        counter.Record(bigButStale); counter.Record(smallButFresh);
        Assert.Equal(smallButFresh, counter.Retrieve("t", asOf, 0.01)[0]);

        // v0.4.0 EKI: bu karsi-ornek SIRALAMADA hala gecerlidir (ve gecerli OLMALIDIR — atfi
        // olmayan bir ders yeni karari yonlendiremez). Ama artik KESMEDE gecerli DEGILDIR:
        // karsi-survivorship tabani (ENS-2003 v0.4.0 §3), buyuk dersin elenmesini yasaklar.
        // Yani "ogrenim buyuklugu siralamayi belirler" iddiasi hala MUTLAK degil — ama
        // "ogrenim buyuklugu KAYBOLMAMAYI belirler" iddiasi artik mutlaktir.
        var counterTop1 = counter.RetrieveTop("t", limit: 1, asOf, 0.01);
        Assert.Single(counterTop1);
        Assert.Same(bigButStale, counterTop1[0]);
    }

    [Fact]
    public void AUDIT_HOLDS_G9_decay_law_is_mathematically_what_it_claims_to_be()
    {
        // v0.4.0 (ENS-2003 §3a): Salience = value(m) x decayFactor
        //                                 = (|L| * c) x exp(-lambda_pi * dt)
        // — elle hesaplayip dogruluyoruz. ESKISI: lambda(c) = lambda_base*(1-c)^gamma idi ve `c`
        // hem carpanda hem ustelde vardi (cift-sayim, AUDIT-WAVE2/D-5).
        var rng = new Random(2003); // ENS-2003
        var memory = MemoryAt(T0);
        for (int i = 0; i < 300; i++)
        {
            double conf = rng.NextDouble();
            double mag = rng.NextDouble() * 20;
            double contextDecayRate = rng.NextDouble() * 0.05;
            int ageDays = rng.Next(0, 3000);

            var rec = new MemoryRecord(Identity.New(), "x", mag, conf, T0.AddDays(-ageDays));

            double handSalience = (mag * conf) * Math.Exp(-contextDecayRate * ageDays);

            Assert.Equal(handSalience, memory.Salience(rec, T0, contextDecayRate), precision: 9);

            // Ve `c`'nin USTELDE olmadigi, ayni kaydin confidence'ini degistirerek dogrudan
            // kanitlaniyor: decayFactor DEGISMEZ (tautoloji degil — bagimsiz elle hesap).
            var twinOtherConfidence = new MemoryRecord(Identity.New(), "x", mag, 1.0 - conf, T0.AddDays(-ageDays));
            Assert.Equal(Math.Exp(-contextDecayRate * ageDays), memory.DecayFactor(twinOtherConfidence, T0, contextDecayRate), precision: 12);
        }
    }

    // ========================================================================================
    // H. REFLECTIVE DOUBLE-LOOP — "P7 kapisi YAPISAL" iddiasini kirma
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_H1_a_proposal_can_be_auto_applied_by_any_caller_in_three_lines()
    {
        // ACIK BIRAKILDI (bilincli). Demo bolum 7: "oneri uygulanamaz, cunku uygulayacak metot
        // yok. Insan onayi MIMARI olarak zorunlu." Bu, YOKLUKTAN ARGUMAN ve yalnizca TEK BIR
        // SINIF icin gecerli. `CompanyMemory.Record` public ve korumasiz; kod tabaninda
        // "insan onayi" diye bir TIP bile yok.
        //
        // NEDEN KAPATILMADI: kapatmak bir onay-token'i / imzali approval nesnesi (P7'nin
        // birinci-sinif tipi) gerektirir. Bu, E3'un gate-token borcuyla AYNI eksik parcadir ve
        // ADR-0001'de karari verilmemistir (Madde VIII). Teori/ADR once — Faz-5 kapi sarti.
        var memory = MemoryAt(T0);
        var records = Enumerable.Range(0, 4)
            .Select(_ => new MemoryRecord(Identity.New(), "tedarikci-secimi", 7.5, 0.6, T0))
            .ToList();
        foreach (var r in records) memory.Record(r);

        var proposals = ReflectiveDoubleLoop.Propose(records);
        Assert.Single(proposals);

        // "Otonom uygulama" — hicbir gate, hicbir onay, hicbir iz:
        foreach (var p in proposals)
            memory.Record(new MemoryRecord(Identity.New(), p.PurposeType, 0, 1.0, T0));

        Assert.Equal(5, memory.AllRecords.Count);
    }

    [Fact]
    public void AUDIT_FIXED_H2_minSupportingRecords_now_actually_enforces_its_own_guard_message()
    {
        // KUSURDU: Guard mesaji "tek gozlemden 'sistematik' iddia edilemez" diyordu ama tam
        //   olarak 1 KABUL ediliyordu -> tek gozlemden "sistematik tekrar" onerisi uretiliyordu.
        //   Guard, iddia ettigi seyi engellemiyordu: KOZMETIKTI.
        //   (Denetimin kendi testi de kusurluydu: mesajda "sistematik" kelimesini ariyordu ama
        //    uretilen aciklamada o kelime yoktu — tek FAIL eden testti. Artik anlamli.)
        // KAPANDI: alt sinir 2'ye cekildi; n=1 bir ORNEKTIR, bir ORUNTU degildir.
        var single = new[] { new MemoryRecord(Identity.New(), "x", 9, 0.7, T0) };

        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => ReflectiveDoubleLoop.Propose(single, minSupportingRecords: 1, magnitudeThreshold: 5.0));
        Assert.Contains("tek gözlemden", ex.Message);
        Assert.Contains("örüntü", ex.Message);

        // 2 gozlem kabul edilir (kavramsal alt sinir; ampirik esik hala acik borc):
        var two = new[]
        {
            new MemoryRecord(Identity.New(), "x", 9, 0.7, T0),
            new MemoryRecord(Identity.New(), "x", 6, 0.5, T0)
        };
        var proposals = ReflectiveDoubleLoop.Propose(two, minSupportingRecords: 2, magnitudeThreshold: 5.0);
        Assert.Single(proposals);
        Assert.Equal(2, proposals[0].SupportingRecordCount);

        // ve tek kayit, 2 esigiyle oneri URETMEZ:
        Assert.Empty(ReflectiveDoubleLoop.Propose(single, minSupportingRecords: 2, magnitudeThreshold: 5.0));
    }

    [Fact]
    public void AUDIT_HOLDS_H3_no_mutating_method_exists_on_the_type_itself()
    {
        // Sinirli ama gercek: TIP duzeyinde iddia dogru. (Sistem duzeyinde degil — bkz. H1.)
        var methods = typeof(ReflectiveDoubleLoop)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        Assert.Single(methods);
        Assert.Equal("Propose", methods[0].Name);
    }

    [Fact]
    public void AUDIT_HOLDS_H4_proposal_thresholds_behave_at_the_boundary()
    {
        var at = Enumerable.Range(0, 3)
            .Select(_ => new MemoryRecord(Identity.New(), "x", 5.0, 0.5, T0)).ToArray();
        var below = Enumerable.Range(0, 3)
            .Select(_ => new MemoryRecord(Identity.New(), "x", 4.999, 0.5, T0)).ToArray();

        Assert.Single(ReflectiveDoubleLoop.Propose(at, 3, 5.0));    // >= dahil
        Assert.Empty(ReflectiveDoubleLoop.Propose(below, 3, 5.0));
    }

    // ========================================================================================
    // I. DECISION AGGREGATE — "atom siniri MUHURLENDI" iddiasini kirma
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_I1_Rehydrate_now_enforces_every_individuation_invariant()
    {
        // KUSURDU: `Rehydrate` public static'ti ve olaylari HICBIR dogrulama yapmadan uyguluyordu.
        //   Purpose'suz, Alternative'siz, Framing'siz bir "commit edilmis karar" uretilebiliyordu.
        //   Individuation muhru yalnizca CANLI yolda vardi; replay yolunda YOKTU — oysa
        //   event-sourced bir sistemde replay BIRINCIL yoldur.
        // KAPANDI: `EnsureReplayInvariant` her replay olayini canli yolla AYNI ENS-2001
        //   §Individuation on-kosullarindan geciriyor.
        var id = Identity.New();
        var owner = Identity.New();

        var forged = new DecisionCommitted("hic-degerlendirilmemis-alternatif", owner, 0.99, "harika")
        { Emitter = owner, Target = id };

        var ex = Assert.Throws<InvalidOperationException>(() => DecisionAggregate.Rehydrate(id, [forged]));
        Assert.Contains("Purpose", ex.Message);

        // Framing var ama Alternatives yok -> yine RED
        var framed = new DecisionFramed("p") { Emitter = owner, Target = id };
        var ex2 = Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, [framed, forged]));
        Assert.Contains("Alternative", ex2.Message);

        // Alternatives var ama secilen secenek KUMEDE DEGIL -> yine RED
        var alts = new AlternativesIdentified(["A", "B"], [new Premise("kanit", 1.0)]) { Emitter = owner, Target = id };
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, [framed, alts, forged]));

        // Gecerli akis replay ediliyor (guard cok genis degil):
        var good = new DecisionCommitted("A", owner, 0.7, "beklenen") { Emitter = owner, Target = id };
        var ok = DecisionAggregate.Rehydrate(id, [framed, alts, good]);
        Assert.True(ok.IsCommitted);
        Assert.Equal("p", ok.Purpose);
        Assert.Equal(["A", "B"], ok.Alternatives);
    }

    [Fact]
    public void AUDIT_FIXED_I2_Rehydrate_rejects_two_commitments_on_the_same_decision()
    {
        // KUSURDU: "Tek Commitment olayi" invariant'i replay'de tamamen kayboluyordu; ikinci
        //   commit sessizce kazaniyor ve confidence'i eziyordu.
        // KAPANDI: replay de tek-commitment mührünü zorluyor.
        var id = Identity.New(); var o = Identity.New();
        var framed = new DecisionFramed("p") { Emitter = o, Target = id };
        var alts = new AlternativesIdentified(["A", "B"], [new Premise("e", 1.0)]) { Emitter = o, Target = id };
        var c1 = new DecisionCommitted("A", o, 0.5, "x") { Emitter = o, Target = id };
        var c2 = new DecisionCommitted("B", o, 0.9, "y") { Emitter = o, Target = id };

        var ex = Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, [framed, alts, c1, c2]));
        Assert.Contains("İKİNCİ Commitment", ex.Message);

        // Tek commitment'li akis sorunsuz:
        var ok = DecisionAggregate.Rehydrate(id, [framed, alts, c1]);
        Assert.Equal(0.5, ok.Confidence!.Value, precision: 10);
        Assert.Equal(3, ok.History.Count);
    }

    [Fact]
    public void AUDIT_FIXED_I3_NaN_confidence_is_rejected_at_commit_so_the_chain_is_closed()
    {
        // KUSURDU: Commit'in `confidence is < 0 or > 1` guard'i NaN'i geciriyordu. Bu NaN sonra
        //   dogrudan Scheduler/Gate'e akip B3'teki uclu fail-open'a donusuyordu — zincir kapali
        //   degildi (commit -> gravity -> gate -> scheduler).
        // KAPANDI: ayni Guard hem Commit'te hem replay'de hem gate/gravity yolunda. Zincirin
        //   HER halkasi ayni kapiyi kullaniyor.
        var o = Identity.New();
        var d = DecisionAggregate.Frame(o, "p");
        d.IdentifyAlternatives(o, ["A"], [new Premise("kanit", 1.0)]);

        Assert.Throws<ArgumentOutOfRangeException>(() => d.Commit(o, "A", double.NaN, "beklenen"));
        Assert.False(d.IsCommitted);
        Assert.Null(d.Confidence);

        // Replay yolu da ayni: NaN confidence'li bir DecisionCommitted event'i kabul edilmiyor.
        var id = Identity.New();
        var framed = new DecisionFramed("p") { Emitter = o, Target = id };
        var alts = new AlternativesIdentified(["A"], [new Premise("e", 1.0)]) { Emitter = o, Target = id };
        var nanCommit = new DecisionCommitted("A", o, double.NaN, "x") { Emitter = o, Target = id };
        Assert.Throws<ArgumentOutOfRangeException>(
            () => DecisionAggregate.Rehydrate(id, [framed, alts, nanCommit]));

        // Ve zincirin sonu: gate'e NaN confidence gecirilemez.
        Assert.Throws<ArgumentOutOfRangeException>(
            () => BoundedAutonomyGate.Evaluate(1_000_000, double.NaN, 1.0, false, 10, 100));
    }

    [Fact]
    public void AUDIT_FIXED_I4_alternatives_list_is_snapshotted_so_no_undeliberated_option_can_be_committed()
    {
        // KUSURDU: `IdentifyAlternatives` gelen listeyi KOPYALAMIYORDU; event ve aggregate ayni
        //   canli `List<string>`'i paylasiyordu. Cagiran sonradan alternatif ekleyip HIC
        //   DEGERLENDIRILMEMIS bir secenege commit edebiliyordu — "acik Alternatives" muhru delikti.
        // KAPANDI: deliberation kumesi SNAPSHOT alinip `ReadOnlyCollection` olarak muhurleniyor.
        var o = Identity.New();
        var alternatives = new List<string> { "Tedarikci-A", "Tedarikci-B" };

        var d = DecisionAggregate.Frame(o, "Tedarikci secimi");
        d.IdentifyAlternatives(o, alternatives, [new Premise("kanit", 1.0)]);

        alternatives.Add("Tedarikci-Z-hic-degerlendirilmedi");

        Assert.DoesNotContain("Tedarikci-Z-hic-degerlendirilmedi", d.Alternatives);
        Assert.Equal(2, d.Alternatives.Count);
        Assert.Throws<ArgumentException>(() => d.Commit(o, "Tedarikci-Z-hic-degerlendirilmedi", 0.9, "x"));
        Assert.False(d.IsCommitted);

        // Cikti downcast'i de kapali:
        Assert.Throws<InvalidCastException>(() => ((List<string>)d.Alternatives).Add("gizli-secenek"));
    }

    [Fact]
    public void AUDIT_FIXED_I5_event_history_can_no_longer_be_erased_via_downcast()
    {
        // KUSURDU: `History`/`UncommittedEvents` canli `List<DomainEvent>` donduruyordu;
        //   `((List<DomainEvent>)d.History).Clear()` ile "karar bir olay gecmisidir" iddiasi
        //   tek satirda curutulebiliyordu — durum "commit edilmis" gorunurken gecmis yok oluyordu.
        // KAPANDI: `ReadOnlyCollection<DomainEvent>` gorunumleri.
        var o = Identity.New();
        var d = DecisionAggregate.Frame(o, "p");
        d.IdentifyAlternatives(o, ["A"], [new Premise("e", 1.0)]);
        d.Commit(o, "A", 0.5, "x");
        Assert.Equal(3, d.History.Count);

        Assert.Throws<InvalidCastException>(() => ((List<DomainEvent>)d.History).Clear());
        Assert.Throws<InvalidCastException>(() => ((List<DomainEvent>)d.UncommittedEvents).Clear());

        Assert.Equal(3, d.History.Count);
        Assert.True(d.IsCommitted);
    }

    [Fact]
    public void AUDIT_HOLDS_I6_live_path_individuation_seal_survives_direct_attack()
    {
        // Canli API uzerinden Individuation gercekten muhurlu — bu iddia sag cikiyor.
        var o = Identity.New();
        var d = DecisionAggregate.Frame(o, "p");

        Assert.Throws<InvalidOperationException>(() => d.Commit(o, "A", 0.5, "x"));      // alternatifsiz
        Assert.Throws<ArgumentException>(() => d.IdentifyAlternatives(o, [], [new Premise("e", 1.0)])); // bos kume
        // D-1: bos EVIDENCE kumesi de reddediliyor (izsiz commitment yasak, ENS-4025 L8).
        Assert.Throws<ArgumentException>(() => d.IdentifyAlternatives(o, ["A", "B"], []));

        d.IdentifyAlternatives(o, ["A", "B"], [new Premise("e", 1.0)]);
        Assert.Throws<ArgumentException>(() => d.Commit(o, "C", 0.5, "x"));              // kume disi
        Assert.Throws<ArgumentOutOfRangeException>(() => d.Commit(o, "A", 1.5, "x"));    // aralik disi

        d.Commit(o, "A", 0.5, "x");
        Assert.Throws<InvalidOperationException>(() => d.Commit(o, "B", 0.9, "y"));      // ikinci commit
        Assert.Throws<InvalidOperationException>(() => d.IdentifyAlternatives(o, ["C"], [new Premise("e", 1.0)]));
        Assert.Throws<InvalidOperationException>(() => d.ObserveOutcome(o, "sonuc"));    // enact'siz
    }

    // ========================================================================================
    // J. DECISION CAPITAL / GRAVITY — sinir degerleri
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_J1_DecisionCapital_Value_rejects_NaN_and_Infinity()
    {
        // KUSURDU: `Value(NaN, 0.5)` = NaN, `Value(+Inf, 0.5)` = +Inf sessizce donuyordu. Bu deger
        //   dogrudan `MemoryRecord.RetentionPriority`'ye akiyor ve G5'teki "sessiz kurumsal
        //   amnezi"yi (kayit ne bulunur ne bayraklanir) uretiyordu.
        // KAPANDI: Guard.NonNegativeFinite + Guard.UnitInterval.
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(double.NaN, 0.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(5, double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(double.PositiveInfinity, 0.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.DeltaCapital(double.NaN, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.ReuseROI(double.NaN, 1));
    }

    [Fact]
    public void AUDIT_HOLDS_J2_DecisionCapital_guards_the_finite_domain_correctly()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(-1, 0.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.Value(5, 1.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.ReuseROI(10, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecisionCapital.ReuseROI(-1, 5));
    }

    [Fact]
    public void AUDIT_HOLDS_J3_extreme_magnitudes_do_not_silently_wrap()
    {
        // Iddia korunuyor: uc buyuklukler SESSIZCE sarmalanmaz/tasmaz.
        // GUNCELLEME (duzeltme sonrasi semantik): eskiden bu test tasmayi
        // `AttentionPriority(MaxValue, 0.0, 2.0)` -> +Infinity ile gosteriyordu. Deficit artik
        // [0,1]'e normalize edildigi icin (AUDIT 5.6) o carpan yolu KAPALI — ve bu daha
        // iyisidir: tasma artik URETILEMIYOR. Iddiayi kirpma sonrasi haliyle sabitliyoruz.
        Assert.True(DecisionGravity.InfoNeed(double.MaxValue, 0.0) == double.MaxValue);
        Assert.Equal(double.MaxValue, DecisionGravity.AttentionPriority(double.MaxValue, 0.0, 1.0));

        // deficit > 1 artik tasma URETEMEZ; kirpilir (onceligi asla artirmaz).
        Assert.Equal(double.MaxValue, DecisionGravity.AttentionPriority(double.MaxValue, 0.0, 2.0));
        Assert.False(double.IsInfinity(DecisionGravity.AttentionPriority(double.MaxValue, 0.0, 1e300)));

        // Ve sonsuz girdi hic ICERI GIREMEZ (sessiz tasma yerine acik hata).
        Assert.Throws<ArgumentOutOfRangeException>(
            () => DecisionGravity.InfoNeed(double.PositiveInfinity, 0.0));
    }
}
