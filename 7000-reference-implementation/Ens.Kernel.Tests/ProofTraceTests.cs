using Ens.Kernel;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ADR-0001 §5.5, ENS-4025 L7 (min t-norm) + L8 (izsiz çıkarım yasak), Anayasa Madde VI/P6.
// Bu testler "proof-trace opsiyon değil, aksiyomdur" iddiasının YAPISAL olarak zorlandığını
// doğrular: izsiz bir türetim temsil EDİLEMEZ (constructor reddeder).

public sealed class ProofTraceTests
{
    [Fact]
    public void Trace_without_premises_cannot_be_constructed()
    {
        // ENS-4025 L8'in yapısal zorlanışı: öncülsüz türetim = black-box = Madde VI ihlali.
        Assert.Throws<ArgumentException>(
            () => new ProofTrace("IR-001", "Decision-42 enacted", []));
    }

    [Fact]
    public void Trace_without_rule_id_cannot_be_constructed()
    {
        Assert.Throws<ArgumentException>(
            () => new ProofTrace("  ", "Decision-42 enacted", [new Premise("Context-7", 0.8)]));
    }

    [Fact]
    public void Confidence_is_min_of_premises_L7_tnorm()
    {
        // ENS-4025 L7: conf(sonuç) = min(conf(öncüller)) — muhafazakâr t-norm.
        var trace = new ProofTrace("IR-001", "Decision-42 indirectly_supported_by Capability-7",
        [
            new Premise("plan", 0.8),
            new Premise("context", 0.7),
            new Premise("memory-precedent", 0.9)
        ]);

        Assert.Equal(0.7, trace.Confidence, precision: 10);
    }

    [Fact]
    public void Single_premise_confidence_passes_through()
    {
        var trace = new ProofTrace("BAG-P3", "Action-42 allowed", [new Premise("policy", 0.95)]);
        Assert.Equal(0.95, trace.Confidence, precision: 10);
    }

    [Fact]
    public void Confidence_cannot_be_assigned_only_derived()
    {
        // Yapısal kanıt: ProofTrace'in Confidence'ı yalnızca getter — atanabilir bir setter yok.
        var prop = typeof(ProofTrace).GetProperty(nameof(ProofTrace.Confidence));
        Assert.NotNull(prop);
        Assert.False(prop!.CanWrite);
    }

    [Fact]
    public void Premise_with_out_of_range_confidence_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Premise("x", 1.5));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Premise("x", -0.1));
    }

    [Fact]
    public void Premise_with_empty_source_throws()
    {
        Assert.Throws<ArgumentException>(() => new Premise("   ", 0.5));
    }

    [Fact]
    public void Chained_derivation_never_increases_confidence()
    {
        // L7 sonucu: türetim zinciri uzadıkça güven monoton azalır, ASLA artmaz.
        var first = new ProofTrace("IR-001", "Fact-A", [new Premise("evidence", 0.6)]);
        var second = new ProofTrace("IR-004", "Fact-B",
        [
            first.AsPremise(),
            new Premise("other-evidence", 0.9)
        ]);

        Assert.Equal(0.6, second.Confidence, precision: 10);
        Assert.True(second.Confidence <= first.Confidence);
    }

    [Fact]
    public void AsPremise_carries_conclusion_and_confidence()
    {
        var trace = new ProofTrace("IR-001", "Decision-42 serves Purpose-3",
            [new Premise("framing", 0.75)]);

        var premise = trace.AsPremise();

        Assert.Equal("Decision-42 serves Purpose-3", premise.Source);
        Assert.Equal(0.75, premise.Confidence, precision: 10);
    }

    [Fact]
    public void Render_contains_rule_conclusion_and_confidence()
    {
        // P6: açıklama nesnesi insan-okunur olmalı (ENS-4025 §Proof-trace biçimi).
        var trace = new ProofTrace("IR-001", "Decision-42 indirectly_supported_by Capability-7",
            [new Premise("serves", 0.8), new Premise("supports", 0.7)]);

        var rendered = trace.Render();

        Assert.Contains("IR-001", rendered);
        Assert.Contains("Decision-42 indirectly_supported_by Capability-7", rendered);
        Assert.Contains("0,70", rendered.Replace(".", ",")); // kültür-bağımsız kontrol
        Assert.Contains("⊢", rendered);
    }

    [Fact]
    public void Premises_are_defensively_copied()
    {
        // Trace kurulduktan sonra öncül listesi dışarıdan değiştirilememeli (audit bütünlüğü).
        var premises = new List<Premise> { new("a", 0.9) };
        var trace = new ProofTrace("R1", "C", premises);

        premises.Add(new Premise("sonradan-eklendi", 0.1));

        Assert.Single(trace.Premises);
        Assert.Equal(0.9, trace.Confidence, precision: 10);
    }
}
