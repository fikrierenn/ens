using Ens.Kernel.Laws;
using Xunit;
using static Ens.Kernel.Laws.DecisionEntropy;

namespace Ens.Kernel.Tests;

// TRACE: ENS-3021 (Decision Entropy) — SKR-012'nin zincir-kuralı iddiasını doğrular:
// H(A|C) = I(A;Owner|C) + H(A|C,Owner)

public sealed class DecisionEntropyTests
{
    [Fact]
    public void Perfectly_consistent_choices_have_zero_entropy()
    {
        // Aynı context'te herkes hep aynı seçimi yapıyor → tam tutarlı → H=0
        var obs = new List<Observation>
        {
            new("ctx-A", "owner-1", "sipariş-ver"),
            new("ctx-A", "owner-2", "sipariş-ver"),
            new("ctx-A", "owner-3", "sipariş-ver"),
        };

        Assert.Equal(0.0, ConditionalEntropy(obs), precision: 10);
    }

    [Fact]
    public void Maximally_dispersed_binary_choice_has_entropy_one()
    {
        // Tek context, 50/50 iki seçenek → H(A|C) = 1 bit (maksimum ikili entropi)
        var obs = new List<Observation>
        {
            new("ctx-A", "owner-1", "seçenek-1"),
            new("ctx-A", "owner-2", "seçenek-2"),
        };

        Assert.Equal(1.0, ConditionalEntropy(obs), precision: 10);
    }

    [Fact]
    public void Chain_rule_holds_HAC_equals_level_plus_pattern_noise()
    {
        // TRACE: SKR-012 — H(A|C) = I(A;Owner|C) + H(A|C,Owner), zincir kuralı matematiksel
        // olarak her zaman doğru olmalı (bu, formülün kendi iç-tutarlılığının testi).
        var obs = new List<Observation>
        {
            new("ctx-A", "owner-1", "X"),
            new("ctx-A", "owner-1", "Y"), // owner-1 tutarsız (pattern noise)
            new("ctx-A", "owner-2", "X"),
            new("ctx-A", "owner-2", "X"),
            new("ctx-B", "owner-1", "Z"),
        };

        double hac = ConditionalEntropy(obs);
        double levelNoise = LevelNoise(obs);
        double patternNoise = ConditionalEntropyGivenOwner(obs);

        Assert.Equal(hac, levelNoise + patternNoise, precision: 9);
    }

    [Fact]
    public void Different_owners_always_choosing_differently_produces_level_noise()
    {
        // owner-1 hep A, owner-2 hep B → aynı context'te owner farkı seçimi belirliyor
        // → pattern noise=0 (her owner kendi içinde tutarlı), level noise > 0
        var obs = new List<Observation>
        {
            new("ctx-A", "owner-1", "A"),
            new("ctx-A", "owner-1", "A"),
            new("ctx-A", "owner-2", "B"),
            new("ctx-A", "owner-2", "B"),
        };

        Assert.Equal(0.0, ConditionalEntropyGivenOwner(obs), precision: 10);
        Assert.True(LevelNoise(obs) > 0.9); // ~1 bit'e yakın
    }

    [Fact]
    public void Empty_observations_yield_zero_entropy()
    {
        Assert.Equal(0.0, ConditionalEntropy([]));
    }
}
