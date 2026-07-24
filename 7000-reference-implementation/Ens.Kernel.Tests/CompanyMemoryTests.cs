using Ens.Kernel.Domain;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-2003 (Company Memory) §1 (düğümler), §3 (retention ∝ |Learning|, sönümle-silme)

public sealed class CompanyMemoryTests
{
    private static readonly DateTimeOffset Now = new(2026, 7, 24, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Record_never_removes_existing_records()
    {
        var memory = new CompanyMemory();
        memory.Record(new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 5, 0.5, Now.AddDays(-100)));
        memory.Record(new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 1, 0.1, Now));

        Assert.Equal(2, memory.AllRecords.Count);
    }

    [Fact]
    public void Retrieve_filters_by_purpose_type()
    {
        var memory = new CompanyMemory();
        var match = new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 5, 0.8, Now);
        var other = new MemoryRecord(Identity.New(), "fiyatlandırma", 5, 0.8, Now);
        memory.Record(match);
        memory.Record(other);

        var result = memory.Retrieve("tedarikçi-seçimi", Now);

        Assert.Single(result);
        Assert.Equal(match, result[0]);
    }

    [Fact]
    public void Failed_but_measured_decision_outranks_higher_confidence_low_learning_decision()
    {
        // §3 karşı-survivorship: |Learning| büyükse (başarısızlıktan çok öğrenilmişse),
        // outcome'un pozitifliği değil, öğrenim büyüklüğü retention'ı belirler.
        var memory = new CompanyMemory();
        var bigFailureLesson = new MemoryRecord(Identity.New(), "tedarikçi-seçimi", LearningMagnitude: 10, AttributionConfidence: 0.9, Now);
        var smallSuccessNoise = new MemoryRecord(Identity.New(), "tedarikçi-seçimi", LearningMagnitude: 1, AttributionConfidence: 0.9, Now);
        memory.Record(smallSuccessNoise);
        memory.Record(bigFailureLesson);

        var result = memory.Retrieve("tedarikçi-seçimi", Now);

        Assert.Equal(bigFailureLesson, result[0]);
    }

    [Fact]
    public void Salience_decays_with_age_but_record_stays_retrievable()
    {
        var memory = new CompanyMemory();
        var old = new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 5, 0.8, Now.AddDays(-365));
        memory.Record(old);

        var result = memory.Retrieve("tedarikçi-seçimi", Now);

        // Sönmüş ama silinmemiş — hâlâ getirilebilir (§3, audit)
        Assert.Single(result);
        Assert.True(CompanyMemory.Salience(old, Now, decayRatePerDay: 0.01) < old.RetentionPriority);
    }

    [Fact]
    public void Salience_zero_decay_rate_never_decays()
    {
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.8, Now.AddDays(-1000));
        Assert.Equal(record.RetentionPriority, CompanyMemory.Salience(record, Now, decayRatePerDay: 0), precision: 10);
    }

    [Fact]
    public void Salience_negative_decay_rate_throws()
    {
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.8, Now);
        Assert.Throws<ArgumentOutOfRangeException>(() => CompanyMemory.Salience(record, Now, decayRatePerDay: -0.1));
    }

    [Fact]
    public void RetentionPriority_matches_DecisionCapital_Value_by_design()
    {
        // TRACE: ENS-2003 §Laws — "Decision Capital... Memory Graph üzerinde tanımlanacak"
        var record = new MemoryRecord(Identity.New(), "x", LearningMagnitude: 8, AttributionConfidence: 0.6, Now);
        Assert.Equal(Ens.Kernel.Laws.DecisionCapital.Value(8, 0.6), record.RetentionPriority, precision: 10);
    }

    [Fact]
    public void Retrieve_unknown_purpose_type_returns_empty_not_throw()
    {
        var memory = new CompanyMemory();
        memory.Record(new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 5, 0.8, Now));

        Assert.Empty(memory.Retrieve("hiç-görülmemiş-tip", Now));
    }
}
