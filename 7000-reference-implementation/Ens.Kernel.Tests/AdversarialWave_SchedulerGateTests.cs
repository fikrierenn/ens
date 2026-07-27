using System.Reflection;
using Ens.Kernel;
using Ens.Kernel.Adapter;
using Ens.Kernel.Capability;
using Ens.Kernel.Domain;
using Ens.Kernel.Laws;
using Xunit;

namespace Ens.Kernel.Tests;

// ============================================================================================
// DUSMANCA DENETIM DALGA-2 (ens-skeptic) — hedef: Scheduler.cs + BoundedAutonomyGate.cs + Guard.cs
//
// Onceki tur (AUDIT.md 5.1) NaN fail-open buldu ve Guard.cs ile "kapattigini" iddia ediyor.
// Bu dosya o DUZELTMEYE saldirir. Varsayim: duzeltme eksiktir.
//
// ADLANDIRMA (skill sozlesmesi):
//   AUDIT_DEFECT_SCH_W*  -> gecerse KUSUR VAR (yeni acik ya da hala acik).
//   AUDIT_HOLDS_SCH_W*   -> saldiriya ragmen saglam; regresyon bekcisi.
//
// Ayrintili gerekce: 7000-reference-implementation/AUDIT-WAVE2-SCHEDULER.md
// ============================================================================================

public sealed class AdversarialWave_SchedulerGateTests
{
    private const double Autonomy = 5_000;
    private const double Block = 50_000;

    private static PendingDecision P(
        double stake, double? conf, double deficit = 0.0, bool irreversible = false,
        ToolAuthorization? auth = null)
        => new(Identity.New(), stake, conf, deficit, irreversible, auth);

    // ========================================================================================
    // W1. GUARD'IN KENDISI — her deger donduren public metot NaN'a kapali mi? (reflection taramasi)
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_SCH_W1_every_public_Guard_method_is_NaN_closed()
    {
        // Yontem: Guard'in TUM public static metotlarini reflection'la tara. Boylece yarin
        // eklenecek yeni bir Guard metodu bu testi otomatik olarak kapsam altina girer;
        // NaN'a kapali degilse test KIRILIR. "Bir yol atlanmis mi?" sorusunun mekanik cevabi.
        var methods = typeof(Guard)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        Assert.NotEmpty(methods);

        foreach (var m in methods)
        {
            var ps = m.GetParameters();
            Assert.True(ps.Length >= 1, $"Guard.{m.Name}: parametresiz — meta-test kapsamiyor.");
            Assert.True(
                ps[0].ParameterType == typeof(double) || ps[0].ParameterType == typeof(double?),
                $"Guard.{m.Name}: ilk parametre double/double? degil — meta-test onu KAPSAMIYOR, elle incele.");

            var args = new object?[ps.Length];
            args[0] = double.NaN;
            for (int i = 1; i < ps.Length; i++)
            {
                Assert.True(ps[i].ParameterType == typeof(string),
                    $"Guard.{m.Name}: beklenmeyen parametre tipi {ps[i].ParameterType} — meta-test kapsamiyor.");
                args[i] = "denetim";
            }

            if (m.ReturnType == typeof(bool))
            {
                // IsMeasurable / IsMeasurableConfidence: exception atmaz, false DONMELI.
                var r = m.Invoke(null, args);
                Assert.False((bool)r!, $"Guard.{m.Name}(NaN) true dondu — olculemez girdi olculebilir sayildi.");
            }
            else
            {
                var ex = Assert.Throws<TargetInvocationException>(() => { m.Invoke(null, args); });
                Assert.IsType<ArgumentOutOfRangeException>(ex.InnerException);
            }
        }
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W2_Guard_rejects_infinities_and_out_of_range_but_accepts_denormals()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Finite(double.PositiveInfinity, "x", "x"));
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.Finite(double.NegativeInfinity, "x", "x"));
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.UnitInterval(Math.BitIncrement(1.0), "x", "x"));
        Assert.Throws<ArgumentOutOfRangeException>(() => Guard.UnitInterval(Math.BitDecrement(0.0), "x", "x"));

        // Denormal (double.Epsilon) OLCULEBILIR bir sayidir — reddedilmemeli (yanlis-pozitif olurdu).
        Assert.Equal(double.Epsilon, Guard.UnitInterval(double.Epsilon, "x", "x"));
        Assert.Equal(double.MaxValue, Guard.NonNegativeFinite(double.MaxValue, "x", "x"));

        // -0.0: `value < 0` false -> kabul. Sayisal olarak sifir, isaret biti negatif.
        Assert.Equal(0.0, Guard.NonNegativeFinite(-0.0, "x", "x"), precision: 10);
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W3_NormalizedDeficit_clamp_does_not_normalize_negative_zero()
    {
        // Math.Clamp(-0.0, 0.0, 1.0): `-0.0 < 0.0` false, `-0.0 > 1.0` false -> -0.0 DONER.
        // Yani "clamp(.,0,1)" cikisinin isaret biti negatif olabiliyor. Sayisal olarak zararsiz
        // (== 0.0), AMA sıralamada Comparer<double>.Default -0.0'i 0.0'in ALTINA koyar.
        // Guvenlik acisi: yalnizca kendi onceligini DUSURUR (muhafazakar yon), bu yuzden
        // istismar edilemez; yine de "clamp cikisi >= 0" iddiasi IEEE anlamda tam degil.
        double clamped = Guard.NormalizedDeficit(-0.0, "d", "d");
        Assert.Equal(0.0, clamped, precision: 10);
        Assert.True(clamped <= 0.0 && clamped >= 0.0);

        // Guvenlik-ilgili asil iddia: -0.0 deficit HICBIR ZAMAN onceligi ARTIRMAZ.
        double withNegZero = DecisionGravity.AttentionPriority(1000, 0.0, -0.0);
        double withPosZero = DecisionGravity.AttentionPriority(1000, 0.0, 0.0);
        Assert.True(withNegZero <= withPosZero);
    }

    // ========================================================================================
    // W4-W7. SAYISAL SINIRLAR — tasma, denormal, tam esitlik, ULP ucurumu
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_SCH_W4_priority_chain_cannot_overflow_to_infinity()
    {
        // Gorev hipotezi: `stake * (1-conf)` tasip Infinity uretebilir mi?
        // YAPISAL CEVAP: HAYIR. Iki carpan da [0,1] araligindadir ((1-conf) ve clamp(deficit)),
        // yani sonuc her zaman <= stake. stake sonlu oldugu icin urun de sonludur.
        double infoNeed = DecisionGravity.InfoNeed(double.MaxValue, 0.0);
        Assert.True(double.IsFinite(infoNeed));
        Assert.Equal(double.MaxValue, infoNeed);

        double prio = DecisionGravity.AttentionPriority(double.MaxValue, 0.0, 1.0);
        Assert.True(double.IsFinite(prio));

        var g = BoundedAutonomyGate.Evaluate(1e308, 0.0, 1.0, false, Autonomy, Block);
        Assert.True(double.IsFinite(g.InfoNeed));
        Assert.Equal(GateDecision.Blocked, g.Decision);

        var s = Scheduler.Schedule([P(double.MaxValue, 0.0, 1.0)], Autonomy, Block)[0];
        Assert.True(double.IsFinite(s.AttentionPriority));
        Assert.Equal(LlmTier.Critical, s.Tier);
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W5_denormals_and_negative_zero_stake_stay_fail_closed_conservative()
    {
        // double.Epsilon (denormal): underflow onceligi DUSURUR (muhafazakar), exception atmaz.
        double tiny = DecisionGravity.InfoNeed(double.Epsilon, 0.5);
        Assert.True(double.IsFinite(tiny) && tiny >= 0.0 && tiny <= double.Epsilon);

        // -0.0 stake: kabul edilir, InfoNeed 0, en ucuz tier + otonom (dogru: sifir stake).
        var g = BoundedAutonomyGate.Evaluate(-0.0, 0.5, 0.0, false, Autonomy, Block);
        Assert.Equal(GateDecision.Autonomous, g.Decision);
        Assert.Equal(0.0, g.InfoNeed, precision: 10);
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W6_threshold_equality_resolves_to_the_more_restrictive_branch()
    {
        // Tam esitlikte fail-CLOSED olmali: `>=` kullanildigi icin esik degeri BLOKLU tarafta.
        // InfoNeed = 100 * 0.5 = 50.0 (tam, ikili gosterimde temsil edilebilir).
        var atBlock = BoundedAutonomyGate.Evaluate(100, 0.5, 0, false, autonomyThreshold: 10, blockThreshold: 50);
        Assert.Equal(GateDecision.Blocked, atBlock.Decision);

        var atAutonomy = BoundedAutonomyGate.Evaluate(100, 0.5, 0, false, autonomyThreshold: 50, blockThreshold: 80);
        Assert.Equal(GateDecision.NotifyHuman, atAutonomy.Decision);

        // Bir ULP yukarida esik: artik asilmiyor -> Autonomous. Sinir tam olarak burada.
        var justBelow = BoundedAutonomyGate.Evaluate(
            100, 0.5, 0, false, autonomyThreshold: Math.BitIncrement(50.0), blockThreshold: 80);
        Assert.Equal(GateDecision.Autonomous, justBelow.Decision);

        // Tier tarafi da ayni yonde: esitlikte UST tier.
        Assert.Equal(LlmTier.Critical, LlmTierSelector.SelectTier(infoNeed: 40.0, complexThreshold: 10, criticalThreshold: 40));
        Assert.Equal(LlmTier.Complex, LlmTierSelector.SelectTier(infoNeed: 10.0, complexThreshold: 10, criticalThreshold: 40));
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W7_confidence_exactly_one_buys_unlimited_autonomy_one_ulp_below_does_not()
    {
        // Guard OLCULEBILIRLIGI kapatti, KALIBRASYONU degil (Guard.cs kendi "DURUST SINIR"inda
        // bunu yaziyor). Sonuc: kendi beyan ettigi confidence = 1.0 olan bir aktör, 1e300
        // stake'lik bir karari TAM OTONOM aldirabilir — InfoNeed carpani tam olarak 0'a duser.
        var perfect = BoundedAutonomyGate.Evaluate(1e300, 1.0, 0, false, Autonomy, Block);
        Assert.Equal(GateDecision.Autonomous, perfect.Decision);
        Assert.Equal(0.0, perfect.InfoNeed, precision: 10);

        // Bir ULP asagisi ayni stake'te BLOKLU. Yani otonomi/blok farki, kalibre edilmemis bir
        // ozbeyanin son bitine baglidir. Gate matematiksel olarak dogru, epistemik olarak kirilgan.
        var oneUlpLess = BoundedAutonomyGate.Evaluate(1e300, Math.BitDecrement(1.0), 0, false, Autonomy, Block);
        Assert.Equal(GateDecision.Blocked, oneUlpLess.Decision);
    }

    // ========================================================================================
    // W8-W11. SCHEDULER <-> GATE ARASINDAKI CATLAK — duzeltmenin ATLADIGI yollar
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_SCH_W8_scheduler_preempts_the_gates_own_fail_closed_CriticalBlock_branch()
    {
        // BoundedAutonomyGate.cs "(0) FAIL-CLOSED ONCELIGI" blogu acikca soyle diyor:
        //   "Geri-donulemez bir action, TUM girdileri olculemez (NaN) olsa bile bloklanmak ZORUNDA."
        // Gate tek basina bu sozu TUTUYOR:
        var gateAlone = BoundedAutonomyGate.Evaluate(double.NaN, null, 0.0, true, Autonomy, Block);
        Assert.Equal(GateDecision.CriticalBlock, gateAlone.Decision);

        // AMA gate'in TEK gercek tuketicisi olan Scheduler, gate'i cagirmadan ONCE
        // DecisionGravity.InfoNeed'i cagiriyor (Scheduler.cs:99) ve orada patliyor.
        // Yani o ozenle yazilmis CriticalBlock dali Scheduler yolundan ULASILAMAZ.
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => Scheduler.Schedule([P(double.NaN, null, 0.0, irreversible: true)], Autonomy, Block));
        Assert.Equal("stake", ex.ParamName);

        // Neden onemli: exception, DENETLENEBILIR bir gate kararı DEGILDIR. CriticalBlock bir
        // kayit birakir (Reason + InfoNeed); ArgumentOutOfRangeException birakmaz ve `catch/log/
        // continue` yazan bir cagirici icin gate HIC calismamis olur.
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W9_one_poisoned_decision_denies_attention_to_the_whole_batch()
    {
        // P5: attention KIT KAYNAKTIR ve tahsisi Scheduler yapar. Tek bir olculemez girdi
        // TUM partiyi dusuruyor — 999 saglikli karar hicbir tahsis alamiyor.
        // Guard.cs'in kendi yorumu (NormalizedDeficit) bu vektoru tanimliyor ve deficit icin
        // KIRPMA'yi tam da bu yuzden secmis: "tum partiyi bir tek bozuk sinyal yuzunden
        // exception'la dusurmek servis-disi birakma vektorudur". Ayni gerekce stake/confidence
        // icin UYGULANMAMIS — politika kendi icinde tutarsiz.
        var batch = new List<PendingDecision>();
        for (int i = 1; i <= 999; i++) batch.Add(P(i * 1000.0, 0.5, 0.5));
        var poisoned = P(double.NaN, 0.5, 0.5);
        batch.Insert(500, poisoned);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => Scheduler.Schedule(batch, Autonomy, Block));

        // Ve hata, SUCLU KARARIN kimligini tasimiyor: 1000 kayit icinde hangisi oldugu bulunamaz.
        Assert.DoesNotContain(poisoned.DecisionId.Value, ex.Message);

        // Kanit: zehirli kayit cikarilinca ayni parti sorunsuz sıralaniyor — yani kaybedilen
        // 999 tahsis, tek bir kotu komsunun eseri (kismi sonuc / karantina yolu YOK).
        var healthyOnly = Scheduler.Schedule(batch.Where(d => !double.IsNaN(d.Stake)), Autonomy, Block);
        Assert.Equal(999, healthyOnly.Count);
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W10_broken_policy_thresholds_are_only_validated_on_some_inputs()
    {
        // Politika DOGRULAMASI (blockThreshold >= autonomyThreshold, esiklerin sonlulugu) gate'in
        // ICINDE, kisitlayici erken-donuslerden SONRA yapiliyor. Sonuc: politikanin bozuk olup
        // olmadigi VERIYE bagli olarak tespit ediliyor.

        // (a) Yetkisiz arac -> erken Blocked donusu; tutarsiz esikler HIC kontrol edilmiyor.
        var unauthorized = new ToolAuthorization(IsAllowed: false, RequiresHumanApproval: false, Reason: "hicbir pack izin vermiyor");
        var r = BoundedAutonomyGate.Evaluate(100, 0.5, 0, false,
            autonomyThreshold: 80, blockThreshold: 10, toolAuthorization: unauthorized);
        Assert.Equal(GateDecision.Blocked, r.Decision); // ArgumentException BEKLENIRDI, gelmedi.

        // (b) Geri-donulemez karar -> erken CriticalBlock; NaN esikli bir politika sessizce gecti.
        var okBatch = Scheduler.Schedule([P(100, 0.5, 0.0, irreversible: true)],
            autonomyThreshold: double.NaN, blockThreshold: double.NaN);
        Assert.Equal(GateDecision.CriticalBlock, okBatch[0].Gate.Decision);

        // (c) AYNI politika, geri-donulebilir bir kararla PATLIYOR. Yani ayni bozuk politika,
        //     parti icerigine gore bazen kabul bazen ret — politika dogrulamasi deterministik degil.
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => Scheduler.Schedule([P(100, 0.5, 0.0)], double.NaN, double.NaN));
        Assert.Equal("autonomyThreshold", ex.ParamName);
    }

    [Fact]
    public void AUDIT_FIXED_SCH_W11_NaN_tier_thresholds_are_rejected_instead_of_routing_to_the_cheapest_model()
    {
        // ESKI KUSUR (AUDIT-WAVE2-SCHEDULER.md §2): AUDIT §5.1 "uclu fail-open"in IKINCI ayagi
        // (model tier) ACIKTI. Guard.cs'in kendi listesi 7 cagri noktasi sayiyordu —
        // `LlmTierSelector` o listede YOKTU. `is < 0 or > 1` deseninin NaN korlugu burada
        // `criticalThreshold < complexThreshold` kontrolu olarak tekrarliyordu:
        // NaN < NaN -> false -> tutarsizlik GORULMUYOR -> en permisif dala (Operational) dusuyordu.
        //
        // Eski (kirmizi) davranis, kayit icin:
        //     SelectTier(infoNeed: NaN)                              -> Operational
        //     SelectTier(1e9, complexThreshold: NaN, critical: NaN)  -> Operational
        //     Scheduler.Schedule(..., NaN, NaN)                      -> Tier = Operational, hata YOK
        //
        // YENI DAVRANIS: her uc `double` girdi de Guard'dan gecer; olculemeyen girdi
        // hesaplama kaynagi KAZANAMAZ (fail-closed, P5/P7). Guard.cs listesi 7 -> 9 oldu.
        var exInfo = Assert.Throws<ArgumentOutOfRangeException>(
            () => LlmTierSelector.SelectTier(infoNeed: double.NaN));
        Assert.Equal("infoNeed", exInfo.ParamName);

        var exThreshold = Assert.Throws<ArgumentOutOfRangeException>(() => LlmTierSelector.SelectTier(
            infoNeed: 1e9, complexThreshold: double.NaN, criticalThreshold: double.NaN));
        Assert.Equal("complexThreshold", exThreshold.ParamName);

        // Scheduler bu esikleri DOGRUDAN geciriyordu — 1 milyar stake'lik, sifir guvenli bir
        // karar sessizce en zayif modele gidiyordu. Artik parti SIRALANMADAN once patlar
        // (ve sebep, hangi PARAMETRENIN olculemez oldugunu soyler).
        var exSched = Assert.Throws<ArgumentOutOfRangeException>(
            () => Scheduler.Schedule([P(1e9, 0.0, 1.0)], Autonomy, Block,
                complexThreshold: double.NaN, criticalThreshold: double.NaN));
        Assert.Equal("complexThreshold", exSched.ParamName);

        // Politika, VERIDEN bagimsiz dogrulanir: BOS bir parti bile bozuk esikle gecemez.
        // (W10'un "politika dogrulamasi veriye bagli" itirazinin tier ayagindaki kapanisi;
        //  gate ayagi — autonomy/block esikleri — HALA ACIK, bkz. AUDIT_DEFECT_SCH_W10.)
        Assert.Throws<ArgumentOutOfRangeException>(
            () => Scheduler.Schedule([], Autonomy, Block, complexThreshold: double.NaN, criticalThreshold: 40.0));

        // Tutarsiz (ama olculebilir) esikler de sessizce gecmez.
        Assert.Throws<ArgumentException>(
            () => Scheduler.Schedule([P(1e9, 0.0, 1.0)], Autonomy, Block,
                complexThreshold: 40.0, criticalThreshold: 10.0));

        // GUARD COK GENIS DEGIL: saglikli esiklerle ayni karar dogru tier'i alir ve gate ayagi
        // (eskiden de saglamdi) korunur — duzeltme kaynak tahsisini bozmadi.
        var ok = Scheduler.Schedule([P(1e9, 0.0, 1.0)], Autonomy, Block,
            complexThreshold: 10.0, criticalThreshold: 40.0);
        Assert.Equal(LlmTier.Critical, ok[0].Tier);
        Assert.Equal(GateDecision.Blocked, ok[0].Gate.Decision);
    }

    // ========================================================================================
    // W12-W17. REGISTRY -> GATE BAGI (ADR-0001 6.1(2), AUDIT 5.5'in "kapanisi") — bagi kirmaya calis
    // ========================================================================================

    private static CapabilityRegistry BuildRegistry()
    {
        var reg = new CapabilityRegistry();
        reg.Register(new CapabilityPack("ops", "1.0.0",
            allowedTools: ["read_file", "wire_transfer"],
            requiresHumanApprovalFor: ["wire_transfer"]));
        return reg;
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W12_unauthorized_tool_is_really_blocked_not_silently_swallowed()
    {
        var reg = BuildRegistry();
        var denied = reg.Authorize("delete_database");
        Assert.False(denied.IsAllowed);

        // InfoNeed KUCUCUK (0.1) — esik mantigi tek basina Autonomous verirdi. Registry ezmeli.
        var g = BoundedAutonomyGate.Evaluate(10, 0.99, 0, false, Autonomy, Block, denied);
        Assert.Equal(GateDecision.Blocked, g.Decision);
        Assert.Contains("Capability Registry", g.Reason);

        // Ayni sey Scheduler uzerinden de gecerli olmali (bag gercekten ucdan uca mi?).
        var s = Scheduler.Schedule([P(10, 0.99, 0.0, auth: denied)], Autonomy, Block)[0];
        Assert.Equal(GateDecision.Blocked, s.Gate.Decision);
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W13_approval_requirement_survives_disabling_the_strict_pack_end_to_end()
    {
        // AUDIT 5.5/F3 regresyon bekcisi — ama bu kez REGISTRY duzeyinde degil, GATE cikisinda.
        var reg = BuildRegistry();
        reg.Register(new CapabilityPack("loose", "1.0.0", allowedTools: ["wire_transfer"]));
        reg.Disable("ops"); // kati pack devre disi

        var auth = reg.Authorize("wire_transfer");
        Assert.True(auth.IsAllowed);
        Assert.True(auth.RequiresHumanApproval);

        var g = BoundedAutonomyGate.Evaluate(10, 0.99, 0, false, Autonomy, Block, auth);
        Assert.Equal(GateDecision.Blocked, g.Decision); // InfoNeed 0.1 olmasina ragmen
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W14_toolAuthorization_can_only_tighten_never_loosen()
    {
        var reg = BuildRegistry();
        var allowedNoApproval = reg.Authorize("read_file");
        Assert.True(allowedNoApproval.IsAllowed);
        Assert.False(allowedNoApproval.RequiresHumanApproval);

        // Yuksek InfoNeed -> Blocked. "Izinli arac" bunu GEVSETMEMELI.
        var high = BoundedAutonomyGate.Evaluate(1_000_000, 0.0, 0, false, Autonomy, Block, allowedNoApproval);
        Assert.Equal(GateDecision.Blocked, high.Decision);

        // Geri-donulemez + izinli arac -> yine CriticalBlock (en kisitlayici kazanir).
        var irr = BoundedAutonomyGate.Evaluate(1, 1.0, 0, true, Autonomy, Block, allowedNoApproval);
        Assert.Equal(GateDecision.CriticalBlock, irr.Decision);

        // Onay-sarti, zaten CriticalBlock olan bir karari ASAGI cekmemeli.
        var wt = reg.Authorize("wire_transfer");
        var irr2 = BoundedAutonomyGate.Evaluate(1, 1.0, 0, true, Autonomy, Block, wt);
        Assert.Equal(GateDecision.CriticalBlock, irr2.Decision);
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W15_ToolAuthorization_is_a_public_record_so_a_registry_denial_can_be_laundered()
    {
        // AUDIT 4.1 GateResult icin bu kusuru buldu ve BoundedAutonomyGate "DURUST SINIRLAR (a)"
        // maddesinde ACIK BORC olarak yazdi. Ama 5.5 duzeltmesiyle gelen YENI guven siniri —
        // ToolAuthorization — ayni delige sahip ve o listede YOK.
        var reg = BuildRegistry();

        // (a) Gercek bir registry REDDI, tek satirda "izinli"ye cevriliyor. Reflection GEREKMIYOR.
        var denied = reg.Authorize("delete_database");
        var laundered = denied with { IsAllowed = true };
        var g1 = BoundedAutonomyGate.Evaluate(10, 0.99, 0, false, Autonomy, Block, laundered);
        Assert.Equal(GateDecision.Autonomous, g1.Decision);

        // (b) Gercek bir "insan onayi SART" kisiti, tek satirda soyuluyor.
        var wt = reg.Authorize("wire_transfer");
        Assert.True(wt.RequiresHumanApproval);
        var stripped = wt with { RequiresHumanApproval = false };
        var g2 = BoundedAutonomyGate.Evaluate(10, 0.99, 0, false, Autonomy, Block, stripped);
        Assert.Equal(GateDecision.Autonomous, g2.Decision);

        // (c) Registry'ye hic ugramadan sifirdan uydurmak da mumkun.
        var forged = new ToolAuthorization(true, false, "gate registry'yi hic sormadi");
        Assert.Equal(GateDecision.Autonomous,
            BoundedAutonomyGate.Evaluate(10, 0.99, 0, false, Autonomy, Block, forged).Decision);
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W16_null_toolAuthorization_is_indistinguishable_from_forgetting_to_authorize()
    {
        // Bag OPT-IN: `PendingDecision.ToolAuthorization` varsayilan olarak null ve null =
        // "arac-bagimsiz karar" (en permisif okuma). Ayni null, "arac kullaniyorum ama
        // yetkilendirmeyi unuttum" demek de olabilir; gate ikisini AYIRT EDEMEZ.
        var g = BoundedAutonomyGate.Evaluate(10, 0.99, 0, false, Autonomy, Block, toolAuthorization: null);
        Assert.Equal(GateDecision.Autonomous, g.Decision);

        // Yapisal kanit: PendingDecision'da arac ADI tasiyan bir alan YOK, dolayisiyla Scheduler
        // yetkiyi KENDISI cozemez; cagiranin dogru ToolAuthorization'i iliştirmesine guvenir.
        var props = typeof(PendingDecision).GetProperties();
        Assert.DoesNotContain(props, p =>
            p.PropertyType == typeof(string) && p.Name.Contains("Tool", StringComparison.OrdinalIgnoreCase));

        // Ve Scheduler'in imzasinda CapabilityRegistry yok — "registry -> gate" bagi kernel
        // icinde ZORLANMIYOR, cagirana delege ediliyor.
        var scheduleParams = typeof(Scheduler).GetMethod(nameof(Scheduler.Schedule))!.GetParameters();
        Assert.DoesNotContain(scheduleParams, p => p.ParameterType == typeof(CapabilityRegistry));
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W17_gate_emits_NaN_InfoNeed_out_of_the_guarded_boundary()
    {
        // Guard'in vaadi "olculemeyen girdi kernel'in karar yollarina GIREMEZ" idi. Gate'in iki
        // erken-donus dali bunu tersinden deliyor: olculemeyen bir DEGER, GateResult icinde
        // DISARI CIKIYOR. Kararin kendisi kisitlayici (fail-closed), ama asagi akistaki her
        // tuketici artik NaN'a hazirlikli olmak ZORUNDA — Guard'in tek-kapi iddiasi tam degil.
        var irr = BoundedAutonomyGate.Evaluate(double.NaN, 0.5, 0, true, Autonomy, Block);
        Assert.Equal(GateDecision.CriticalBlock, irr.Decision);
        Assert.True(double.IsNaN(irr.InfoNeed));

        var unauthorized = new ToolAuthorization(false, false, "yok");
        var den = BoundedAutonomyGate.Evaluate(double.PositiveInfinity, 5.0, 0, false, Autonomy, Block, unauthorized);
        Assert.Equal(GateDecision.Blocked, den.Decision);
        Assert.True(double.IsNaN(den.InfoNeed));

        // Sonlu ama tanim-disi (negatif) stake de ayni sekilde NaN'a cevriliyor: olculebilir bir
        // ihlal, olculemez bir cikti haline geliyor (bilgi kaybi).
        var neg = BoundedAutonomyGate.Evaluate(-5.0, 0.5, 0, true, Autonomy, Block);
        Assert.True(double.IsNaN(neg.InfoNeed));
    }

    // ========================================================================================
    // W18-W20. FUZZ — 1000 karar, beklenen deger ELLE hesaplandi (koddan cagrilmadi)
    // ========================================================================================

    /// <summary>Bagimsiz referans hesap: ENS-3022 formulu, DecisionGravity'ye HIC dokunmadan.</summary>
    private static double InfoNeedByHand(PendingDecision d) => d.Stake * (1.0 - (d.Confidence ?? 0.0));

    private static double PriorityByHand(PendingDecision d)
    {
        double clamped = d.ConformanceDeficit < 0.0 ? 0.0
                       : d.ConformanceDeficit > 1.0 ? 1.0
                       : d.ConformanceDeficit;
        return InfoNeedByHand(d) * clamped;
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W18_fuzz_1000_decisions_match_hand_computed_gravity_tier_and_gate()
    {
        const double Cx = 10.0, Cr = 40.0;
        var rng = new Random(20260726); // deterministik tohum — bulgu tekrar uretilebilir olmali
        var pending = new List<PendingDecision>(1000);
        for (int i = 0; i < 1000; i++)
        {
            double stake = rng.NextDouble() * 1_000_000;
            double? conf = rng.Next(10) == 0 ? null : rng.NextDouble();
            double deficit = rng.NextDouble() * 2.0 - 0.5; // [-0.5, 1.5) -> kirpma yolu da sinanir
            bool irreversible = rng.Next(20) == 0;
            pending.Add(new PendingDecision(Identity.New(), stake, conf, deficit, irreversible));
        }

        var result = Scheduler.Schedule(pending, Autonomy, Block, Cx, Cr);

        // (1) Hicbir karar kaybolmadi/cogalmadi (siralama kodunun klasik hatasi).
        Assert.Equal(pending.Count, result.Count);
        Assert.Equal(
            pending.Select(p => p.DecisionId.Value).OrderBy(v => v, StringComparer.Ordinal).ToList(),
            result.Select(r => r.Decision.DecisionId.Value).OrderBy(v => v, StringComparer.Ordinal).ToList());

        // (2) Her satirin uc ciktisi elle hesapla BIREBIR ayni mi?
        foreach (var s in result)
        {
            Assert.Equal(PriorityByHand(s.Decision), s.AttentionPriority);

            double inf = InfoNeedByHand(s.Decision);
            var expectedTier = inf >= Cr ? LlmTier.Critical : inf >= Cx ? LlmTier.Complex : LlmTier.Operational;
            Assert.Equal(expectedTier, s.Tier);

            var expectedGate = s.Decision.IsIrreversible ? GateDecision.CriticalBlock
                             : inf >= Block ? GateDecision.Blocked
                             : inf >= Autonomy ? GateDecision.NotifyHuman
                             : GateDecision.Autonomous;
            Assert.Equal(expectedGate, s.Gate.Decision);

            // Tasma yoklugu yapisal: her iki carpan da <= 1 oldugu icin oncelik <= stake.
            Assert.True(double.IsFinite(s.AttentionPriority));
            Assert.True(s.AttentionPriority <= s.Decision.Stake);
        }

        // (3) Siralama gercekten (AttentionPriority DESC, InfoNeed DESC) mi?
        //     Karsilastirici olarak implementasyonun kullandigi Comparer<double>.Default semantigi.
        for (int i = 1; i < result.Count; i++)
        {
            double prev = result[i - 1].AttentionPriority, cur = result[i].AttentionPriority;
            Assert.True(prev.CompareTo(cur) >= 0,
                $"{i}. sirada oncelik ARTTI: {prev} -> {cur}");
            if (prev.CompareTo(cur) == 0)
                Assert.True(InfoNeedByHand(result[i - 1].Decision).CompareTo(InfoNeedByHand(result[i].Decision)) >= 0,
                    $"{i}. sirada esit oncelikte InfoNeed tie-breaker'i bozuldu.");
        }
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W19_ConformanceDeficit_actually_discriminates_it_is_not_a_decorative_factor()
    {
        // AUDIT 3.1'in itirazi: demo'da deficit carpaninin AYIRT EDICI GUCU sifirdi (InfoNeed
        // siralamasi zaten ayniydi). Burada ikisini KASITLI olarak CATISTIRIYORUM: InfoNeed
        // siralamasi ile AttentionPriority siralamasi TERS olmali.
        var bigInfoTinyGap = P(1000, 0.0, 0.01); // InfoNeed 1000, priority   10
        var smallInfoFullGap = P(100, 0.0, 1.0); // InfoNeed  100, priority  100

        var res = Scheduler.Schedule([bigInfoTinyGap, smallInfoFullGap], Autonomy, Block);

        Assert.Equal(smallInfoFullGap.DecisionId, res[0].Decision.DecisionId);
        Assert.Equal(1000.0, InfoNeedByHand(res[1].Decision), precision: 10); // 10x InfoNeed geride kaldi
        Assert.Equal(100.0, res[0].AttentionPriority, precision: 10);
        Assert.Equal(10.0, res[1].AttentionPriority, precision: 10);
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W20_full_ties_still_depend_on_input_order()
    {
        // Scheduler.cs "(e) ACIK KALAN" maddesi bunu itiraf ediyor — kapanmadi, kanitli duruyor.
        var a = P(1000, 0.5, 0.5);
        var b = P(1000, 0.5, 0.5); // birebir ayni agirliklar, farkli kimlik

        Assert.Equal(a.DecisionId, Scheduler.Schedule([a, b], Autonomy, Block)[0].Decision.DecisionId);
        Assert.Equal(b.DecisionId, Scheduler.Schedule([b, a], Autonomy, Block)[0].Decision.DecisionId);
        // "Ayni girdi, ayni cikti" ancak PERMUTASYON sabitken dogru; kume olarak deterministik degil.
    }

    // ========================================================================================
    // W21-W24. SCHEDULETOP BUTCESI + CIKTI BUTUNLUGU
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_SCH_W21_ScheduleTop_budget_extremes_are_safe_and_prefix_consistent()
    {
        var pending = Enumerable.Range(1, 50)
            .Select(i => P(i * 1000.0, 0.5, 0.5)).ToList();

        // int.MaxValue: Take(int.MaxValue) tasmaz, hepsi doner.
        Assert.Equal(50, Scheduler.ScheduleTop(pending, int.MaxValue, Autonomy, Block).Count);

        // Butce > eleman sayisi: sessizce kirpilir, hata degil.
        Assert.Equal(50, Scheduler.ScheduleTop(pending, 10_000, Autonomy, Block).Count);

        // Negatif ve int.MinValue: acik hata.
        Assert.Throws<ArgumentOutOfRangeException>(() => Scheduler.ScheduleTop(pending, -1, Autonomy, Block));
        Assert.Throws<ArgumentOutOfRangeException>(() => Scheduler.ScheduleTop(pending, int.MinValue, Autonomy, Block));

        // Onek ozelligi: ScheduleTop(n), Schedule()'in ilk n'i OLMALI (ayri bir siralama degil).
        var full = Scheduler.Schedule(pending, Autonomy, Block);
        var top7 = Scheduler.ScheduleTop(pending, 7, Autonomy, Block);
        Assert.Equal(
            full.Take(7).Select(x => x.Decision.DecisionId.Value).ToList(),
            top7.Select(x => x.Decision.DecisionId.Value).ToList());
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W22_scheduler_output_is_a_live_List_and_can_be_reordered_by_the_caller()
    {
        // AUDIT 5.2 deseni ("IReadOnlyList<T> korumaz") bu dosyada KAPATILMADI: donen nesne
        // canli bir List<>. Hafifletici sebep: her cagrida TAZE liste uretiliyor, yani baska bir
        // cagiranin gordugu sonuc bozulmaz. Yine de P5 tahsis sirasi, tuketiciler arasinda
        // dolasirken tek satirda tersine cevrilebilir — "oncelik sirasi" bir garanti degil.
        var pending = Enumerable.Range(1, 5).Select(i => P(i * 1000.0, 0.5, 0.5)).ToList();
        var result = Scheduler.Schedule(pending, Autonomy, Block);

        var live = Assert.IsType<List<ScheduledDecision>>(result);
        double topBefore = result[0].AttentionPriority;
        live.Reverse();
        Assert.True(result[0].AttentionPriority < topBefore); // en dusuk oncelik basa gecti
    }

    [Fact]
    public void AUDIT_DEFECT_SCH_W23_invalid_PendingDecision_is_constructible_and_a_null_element_throws_NRE()
    {
        // (a) Dogrulama KULLANIM yerinde, YAPIM yerinde degil: olculemez bir karar nesnesi
        //     serbestce uretilip tasinabiliyor; patlama cok sonra, baska bir katmanda oluyor.
        //     (AUDIT 5.4/G6'da CompanyMemory icin bulunan ayni desen, burada tekrar ediyor.)
        var bad = new PendingDecision(Identity.New(), double.NaN, double.NaN, double.NaN, false);
        Assert.True(double.IsNaN(bad.Stake));

        // (b) Koleksiyon null'a karsi korunuyor ama ELEMAN null'a karsi korunmuyor:
        //     alan-disi bir NullReferenceException, ArgumentException yerine.
        Assert.Throws<NullReferenceException>(() => Scheduler.Schedule([null!], Autonomy, Block));
    }

    [Fact]
    public void AUDIT_HOLDS_SCH_W24_gate_decision_enum_order_is_the_hidden_dependency_of_the_P7_upgrade()
    {
        // BoundedAutonomyGate.cs:126 `decision < GateDecision.Blocked` yaziyor — yani "insan onayi
        // sart" yukseltmesi enum'un SAYISAL SIRASINA bagli. Enum uyelerini yeniden siralamak
        // (tamamen kozmetik gorunen bir refactor) bu guvenlik kontrolunu SESSIZCE tersine cevirir.
        // Bu test o gizli bagimliligi acik bir kanaryaya donusturur.
        Assert.Equal(0, (int)GateDecision.Autonomous);
        Assert.Equal(1, (int)GateDecision.NotifyHuman);
        Assert.Equal(2, (int)GateDecision.Blocked);
        Assert.Equal(3, (int)GateDecision.CriticalBlock);
        Assert.True(GateDecision.Autonomous < GateDecision.Blocked && GateDecision.NotifyHuman < GateDecision.Blocked);
        Assert.False(GateDecision.CriticalBlock < GateDecision.Blocked);
    }
}
