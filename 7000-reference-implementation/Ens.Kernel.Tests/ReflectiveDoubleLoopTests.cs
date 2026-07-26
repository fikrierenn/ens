using System.Reflection;
using Ens.Kernel.Domain;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-2004 v0.3.2 §4a (Reflective double-loop, ratified/SKR-044). Bu testler §4a'nın
// Faz-4 öneri-üreticisini (ReflectiveDoubleLoop) doğrular. En kritik test P7'nin YAPISAL
// olduğunu kanıtlayan refleksiyon-testidir: sınıfta hiçbir mutasyon/apply metodu yoktur.

public sealed class ReflectiveDoubleLoopTests
{
    private static readonly DateTimeOffset Now = new(2026, 7, 24, 0, 0, 0, TimeSpan.Zero);

    private static MemoryRecord Rec(string purpose, double magnitude, double confidence = 0.7) =>
        new(Identity.New(), purpose, magnitude, confidence, Now);

    [Fact]
    public void Below_threshold_group_yields_no_proposal()
    {
        // Aynı Purpose-tipinde 5 kayıt AMA hepsi düşük-büyüklük (< magnitudeThreshold) →
        // destekleyici kayıt yok → öneri yok.
        var records = new[]
        {
            Rec("fiyatlandırma", 1), Rec("fiyatlandırma", 2), Rec("fiyatlandırma", 3),
            Rec("fiyatlandırma", 1), Rec("fiyatlandırma", 2),
        };

        var proposals = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 3, magnitudeThreshold: 5.0);

        Assert.Empty(proposals);
    }

    [Fact]
    public void Too_few_supporting_records_yields_no_proposal()
    {
        // Büyük-büyüklük var ama yalnızca 2 kayıt (< minSupportingRecords=3) → "sistematik" değil.
        var records = new[] { Rec("fiyatlandırma", 8), Rec("fiyatlandırma", 9) };

        var proposals = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 3, magnitudeThreshold: 5.0);

        Assert.Empty(proposals);
    }

    [Fact]
    public void Repeated_large_magnitude_same_purpose_yields_proposal()
    {
        var records = new[]
        {
            Rec("fiyatlandırma", 8, 0.6), Rec("fiyatlandırma", 9, 0.8), Rec("fiyatlandırma", 7, 0.7),
            Rec("tedarikçi-seçimi", 1), // gürültü, eşik-altı, başka tip
        };

        var proposals = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 3, magnitudeThreshold: 5.0);

        var p = Assert.Single(proposals);
        Assert.Equal("fiyatlandırma", p.PurposeType);
        Assert.Equal(3, p.SupportingRecordCount);
        Assert.Equal(0.7, p.AverageAttributionConfidence, precision: 10); // (0.6+0.8+0.7)/3
    }

    [Fact]
    public void Distinct_purpose_types_yield_distinct_proposals()
    {
        var records = new[]
        {
            Rec("fiyatlandırma", 8), Rec("fiyatlandırma", 9), Rec("fiyatlandırma", 7),
            Rec("tedarikçi-seçimi", 6), Rec("tedarikçi-seçimi", 10), Rec("tedarikçi-seçimi", 8),
        };

        var proposals = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 3, magnitudeThreshold: 5.0);

        Assert.Equal(2, proposals.Count);
        Assert.Contains(proposals, p => p.PurposeType == "fiyatlandırma");
        Assert.Contains(proposals, p => p.PurposeType == "tedarikçi-seçimi");
    }

    [Fact]
    public void Empty_input_yields_no_proposal()
    {
        Assert.Empty(ReflectiveDoubleLoop.Propose([]));
    }

    // --- Input validasyonu ---

    [Fact]
    public void Null_records_throws()
    {
        Assert.Throws<ArgumentNullException>(() => ReflectiveDoubleLoop.Propose(null!));
    }

    [Fact]
    public void Non_positive_min_supporting_records_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ReflectiveDoubleLoop.Propose([], minSupportingRecords: 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => ReflectiveDoubleLoop.Propose([], minSupportingRecords: -1));
    }

    [Fact]
    public void Negative_magnitude_threshold_throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ReflectiveDoubleLoop.Propose([], magnitudeThreshold: -0.1));
    }

    // --- P7 YAPISAL KANITI: sınıfta hiçbir mutasyon/apply metodu YOK ---

    [Fact]
    public void ReflectiveDoubleLoop_exposes_only_Propose_no_apply_or_mutation_method()
    {
        // ENS-2004 §4a P7 kapısı: öneri asla otomatik uygulanmaz. Bu, runtime-kontrol değil
        // TİPİN ŞEKLİDİR — sınıfın tek public API'si Propose'dur. Bu test P7'nin gerçekten
        // yapısal olduğunu kanıtlar: uygulayacak/commit edecek metod yoksa, uygulanamaz.
        MethodInfo[] declared = typeof(ReflectiveDoubleLoop)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        Assert.Single(declared);
        Assert.Equal(nameof(ReflectiveDoubleLoop.Propose), declared[0].Name);
    }

    [Fact]
    public void ReflectiveDoubleLoop_has_no_state_mutating_verbs_in_any_method_name()
    {
        // Object'ten miras (Equals/GetHashCode/ToString/GetType) dahil TÜM public metodları
        // tara — hiçbiri Apply/Commit/Mutate/Update/Set/Write/Save gibi bir mutasyon fiili
        // taşımamalı (P7 defense-in-depth: sınıfa sonradan böyle bir metod eklenirse test kırılır).
        string[] forbidden = ["Apply", "Commit", "Mutate", "Update", "Set", "Write", "Save", "Delete"];

        foreach (var method in typeof(ReflectiveDoubleLoop).GetMethods())
            Assert.DoesNotContain(forbidden, verb =>
                method.Name.Contains(verb, StringComparison.OrdinalIgnoreCase));
    }
}
