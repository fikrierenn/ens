using Ens.Kernel.Domain;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ENS-2003 v0.4.0 (Company Memory) §1 (düğümler), §3 (RetentionPriority = |Learning| —
// c'den BAĞIMSIZ; karşı-survivorship TABANI), §3a (decayFactor = exp(−λ_π·Δt) — c İÇERMEZ;
// value = |L|·c; Salience = value × decayFactor; asserted_at/last_verified),
// §3b (Curator — İKİ inceleme sinyali: stale + weakly-attributed; silme yok).
//
// v0.4.0 (AUDIT-WAVE2-FIDELITY / D-5) sonrası değişen test iddiaları — HİÇBİRİ zayıflatılmadı,
// yönü değiştirildi ve YENİ iddialarla güçlendirildi:
//   • `RetentionPriority_matches_DecisionCapital_Value_by_design` → ARTIK YANLIŞ bir iddiadır
//     (v0.3'ün çift-sayımının ta kendisiydi). İkiye bölündü: RetentionPriority = |L| (saf) ve
//     CapitalValue = |L|·c (ENS-3023 §Model 1).
//   • `High_confidence_record_decays_slower_than_low_confidence_record` → ARTIK YANLIŞ bir
//     iddiadır. Yerine TERS yönde, daha güçlü bir regresyon bekçisi kondu: yalnızca confidence'ı
//     farklı iki kaydın decayFactor'ı BİREBİR AYNI olmak zorunda (D-5'in geri gelmesini engeller).
//   • +5 yeni test: karşı-survivorship tabanı (kesme invariant'ı) ve weakly-attributed sinyali.
//
// ⚠️ Bu dosyayı yazan context'te shell aracı YOKTU: `dotnet test` ÇALIŞTIRILAMADI. Testler statik
// olarak yazıldı; geçtiklerine dair hiçbir iddia yoktur — CI/owner teyidi beklenir.

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
        Assert.True(memory.Salience(old, Now, contextDecayRate: 0.01) < old.CapitalValue);
    }

    [Fact]
    public void Salience_zero_decay_rate_equals_capital_value()
    {
        // λ_π = 0 → decayFactor ≡ 1 → Salience = value(m) = |L|·c (ENS-3023 §Model 1).
        var memory = new CompanyMemory();
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.8, Now.AddDays(-1000));
        Assert.Equal(record.CapitalValue, memory.Salience(record, Now, contextDecayRate: 0), precision: 10);
    }

    [Fact]
    public void Salience_negative_decay_rate_throws()
    {
        var memory = new CompanyMemory();
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.8, Now);
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Salience(record, Now, contextDecayRate: -0.1));
    }

    // --- v0.4.0: ÜÇ NİCELİK, ÜÇ SÜRÜCÜ (AUDIT-WAVE2/D-5 — çift-sayımın regresyon bekçileri) ---

    [Fact]
    public void RetentionPriority_is_pure_learning_magnitude_and_ignores_attribution_confidence()
    {
        // D-5 REGRESYON BEKÇİSİ #1 (ENS-2003 v0.4.0 §3).
        // v0.3'te RetentionPriority = |L|·c idi; bu, sönümdeki (1−c)^γ ile birlikte `c`'yi İKİ KEZ
        // sayıyordu. Artık kayıp-koruma ekseni yalnızca |Learning|'e bakar.
        var strong = new MemoryRecord(Identity.New(), "x", LearningMagnitude: 8, AttributionConfidence: 0.95, Now);
        var weak = new MemoryRecord(Identity.New(), "x", LearningMagnitude: 8, AttributionConfidence: 0.05, Now);

        Assert.Equal(8.0, strong.RetentionPriority, precision: 10);
        Assert.Equal(8.0, weak.RetentionPriority, precision: 10);
        Assert.Equal(strong.RetentionPriority, weak.RetentionPriority, precision: 10);

        // ...ama epistemik ağırlıkları AYRIŞIR (bu, doğru ve tek kalan ceza):
        Assert.True(strong.CapitalValue > weak.CapitalValue);
    }

    [Fact]
    public void CapitalValue_is_ENS3023_value_and_is_not_a_reimplementation()
    {
        // TRACE: ENS-3023 §Model 1 — value(d) = |Learning|·attribution_confidence.
        // Alias yasağı (Anayasa Madde IV): hesap kopyalanmaz, DecisionCapital'e delege edilir.
        var record = new MemoryRecord(Identity.New(), "x", LearningMagnitude: 8, AttributionConfidence: 0.6, Now);
        Assert.Equal(Ens.Kernel.Laws.DecisionCapital.Value(8, 0.6), record.CapitalValue, precision: 10);
    }

    [Fact]
    public void Records_differing_only_in_confidence_decay_identically()
    {
        // D-5 REGRESYON BEKÇİSİ #2 — v0.4.0'ın ÇEKİRDEK iddiası (ENS-2003 §3a).
        // v0.3'te bu test GEÇMEZDİ: λ(c) = λ_base·(1−c)^γ, c'de monotondu. Artık sönüm hızı
        // Purpose-tipinin context yarı-ömrüne bağlıdır; `c` bir ÖLÇÜM özelliğidir, bir HIZ değil.
        var memory = new CompanyMemory();
        var trusted = new MemoryRecord(Identity.New(), "x", LearningMagnitude: 5, AttributionConfidence: 0.95, Now.AddDays(-200));
        var shaky = new MemoryRecord(Identity.New(), "x", LearningMagnitude: 5, AttributionConfidence: 0.05, Now.AddDays(-200));
        memory.Record(trusted);
        memory.Record(shaky);

        double trustedFreshness = memory.DecayFactor(trusted, Now, contextDecayRate: 0.02);
        double shakyFreshness = memory.DecayFactor(shaky, Now, contextDecayRate: 0.02);

        // BİREBİR aynı — yaklaşık değil: aynı Δt, aynı λ_π, formülde `c` yok.
        Assert.Equal(trustedFreshness, shakyFreshness);
        Assert.Equal(Math.Exp(-0.02 * 200), trustedFreshness, precision: 12);
    }

    [Fact]
    public void Full_confidence_no_longer_buys_an_unaudited_exemption_from_decay()
    {
        // v0.3'te λ(1.0) = 0 idi: `AttributionConfidence: 1.0` yazan herkes kaydını, hiçbir gerekçe
        // sunmadan ve hiçbir iz bırakmadan, sönüm yasasının ve Curator'ın DIŞINA çıkarabiliyordu.
        // (Verify için dört kısıt + zorunlu gerekçe istenirken.) v0.4.0'da bu kapı YAPISAL kapalı.
        var memory = new CompanyMemory();
        var certain = new MemoryRecord(Identity.New(), "x", 5, AttributionConfidence: 1.0, Now.AddYears(-10));
        memory.Record(certain);

        Assert.True(memory.DecayFactor(certain, Now, contextDecayRate: 0.02) < 0.001);
        Assert.Contains(certain, memory.FindStale(Now, contextDecayRate: 0.02, staleThreshold: 0.5));
    }

    [Fact]
    public void Retrieve_unknown_purpose_type_returns_empty_not_throw()
    {
        var memory = new CompanyMemory();
        memory.Record(new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 5, 0.8, Now));

        Assert.Empty(memory.Retrieve("hiç-görülmemiş-tip", Now));
    }

    // --- v0.4.0: karşı-survivorship TABANI (kesme invariant'ı, §3) ---

    [Fact]
    public void Truncated_retrieval_can_never_drop_the_largest_lesson_of_a_purpose_type()
    {
        // D-5'in "patoloji ÇÖZÜLMELİ" talebinin yapısal cevabı (ENS-2003 v0.4.0 §3).
        // Senaryo tam da denetimin verdiği somut zarar örneği: "talep tahmini" — satış düştü ama
        // tahmin mi yanlıştı hava mı bozdu ayrılamıyor → BÜYÜK öğrenim (|L|=9), ZAYIF atıf (c=0.25).
        // Yanında üç taze ama önemsiz kayıt var. Kesme yapılınca büyük ders ELENEMEZ.
        var memory = new CompanyMemory();
        var weakLesson = new MemoryRecord(Identity.New(), "talep-tahmini", LearningMagnitude: 9, AttributionConfidence: 0.25, Now.AddDays(-200));
        memory.Record(weakLesson);
        for (int i = 0; i < 3; i++)
            memory.Record(new MemoryRecord(Identity.New(), "talep-tahmini", LearningMagnitude: 1, AttributionConfidence: 0.95, Now));

        // Dürüstlük: sıralamada GERİDE — bunu gizlemiyoruz (tek kalan ceza, doğru ceza).
        var ordered = memory.Retrieve("talep-tahmini", Now, contextDecayRate: 0.01);
        Assert.Equal(4, ordered.Count);
        Assert.Equal(weakLesson, ordered[^1]);

        // Ama KESİLDİĞİNDE elenemez: taban invariant'ı.
        var top3 = memory.RetrieveTop("talep-tahmini", limit: 3, Now, contextDecayRate: 0.01);
        Assert.Equal(3, top3.Count);
        Assert.Contains(weakLesson, top3);

        // ...ve en dar kesmede bile (k=1) hayatta kalır.
        var top1 = memory.RetrieveTop("talep-tahmini", limit: 1, Now, contextDecayRate: 0.01);
        Assert.Single(top1);
        Assert.Equal(weakLesson, top1[0]);
    }

    [Fact]
    public void The_floor_replaces_the_lowest_ranked_slot_and_leaves_the_rest_of_the_order_intact()
    {
        var memory = new CompanyMemory();
        var weakLesson = new MemoryRecord(Identity.New(), "t", LearningMagnitude: 9, AttributionConfidence: 0.25, Now.AddDays(-200));
        var top = new MemoryRecord(Identity.New(), "t", LearningMagnitude: 3, AttributionConfidence: 0.95, Now);
        var second = new MemoryRecord(Identity.New(), "t", LearningMagnitude: 2, AttributionConfidence: 0.95, Now);
        var third = new MemoryRecord(Identity.New(), "t", LearningMagnitude: 1, AttributionConfidence: 0.95, Now);
        memory.Record(weakLesson); memory.Record(top); memory.Record(second); memory.Record(third);

        var result = memory.RetrieveTop("t", limit: 3, Now, contextDecayRate: 0.01);

        Assert.Equal(new[] { top, second, weakLesson }, result);   // `third` düştü, taban girdi
    }

    [Fact]
    public void The_floor_does_nothing_when_the_result_is_not_truncated()
    {
        // Taban yalnızca KESMEDE devrededir; kesilmeyen retrieval hiçbir dersi kaybetmez.
        var memory = new CompanyMemory();
        var a = new MemoryRecord(Identity.New(), "t", 9, 0.25, Now.AddDays(-200));
        var b = new MemoryRecord(Identity.New(), "t", 1, 0.95, Now);
        memory.Record(a); memory.Record(b);

        Assert.Equal(memory.Retrieve("t", Now, 0.01), memory.RetrieveTop("t", limit: 5, Now, 0.01));
    }

    [Fact]
    public void CounterSurvivorshipFloor_picks_max_learning_and_breaks_ties_by_oldest_assertion()
    {
        var memory = new CompanyMemory();
        var older = new MemoryRecord(Identity.New(), "t", 7, 0.1, Now.AddDays(-300));
        var newerTwinMagnitude = new MemoryRecord(Identity.New(), "t", 7, 0.9, Now);
        var smaller = new MemoryRecord(Identity.New(), "t", 6, 0.9, Now);
        memory.Record(newerTwinMagnitude); memory.Record(smaller); memory.Record(older);

        // Determinizm: eşit |Learning|'de ÖZGÜN (en eski) ders kazanır; confidence hiç bakılmaz.
        Assert.Same(older, memory.CounterSurvivorshipFloor("t"));
        Assert.Null(memory.CounterSurvivorshipFloor("hiç-yok"));
    }

    [Fact]
    public void RetrieveTop_rejects_a_zero_limit()
    {
        var memory = new CompanyMemory();
        memory.Record(new MemoryRecord(Identity.New(), "t", 5, 0.5, Now));
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.RetrieveTop("t", limit: 0, Now));
    }

    // --- v0.4.0: DecayFunction (λ_π = ln2/τ_π) ---

    [Fact]
    public void RateFromHalfLife_and_HalfLifeDays_are_inverses()
    {
        double rate = DecayFunction.RateFromHalfLife(contextHalfLifeDays: 90);
        Assert.Equal(Math.Log(2) / 90.0, rate, precision: 12);
        Assert.Equal(90.0, DecayFunction.HalfLifeDays(rate), precision: 10);
    }

    [Fact]
    public void A_record_loses_exactly_half_its_freshness_after_one_context_half_life()
    {
        // τ_π'nin ANLAMI budur ve doğrudan elicit edilebilir olmasının sebebi de: "bu karar
        // sınıfının context'i kaç günde yarı yarıya bayatlar?" (v0.3'ün γ'sı sorulamıyordu.)
        var memory = new CompanyMemory();
        var record = new MemoryRecord(Identity.New(), "fiyatlandırma", 5, 0.4, Now.AddDays(-90));
        memory.Record(record);

        double rate = DecayFunction.RateFromHalfLife(90);
        Assert.Equal(0.5, memory.DecayFactor(record, Now, rate), precision: 12);
    }

    [Fact]
    public void Zero_decay_rate_means_infinite_half_life()
    {
        Assert.True(double.IsPositiveInfinity(DecayFunction.HalfLifeDays(0.0)));
    }

    [Fact]
    public void DaysUntilStale_equals_the_half_life_when_the_threshold_is_one_half()
    {
        // ENS-2003 v0.4.0 §3a: t_stale(π) = τ_π · log₂(1/θ); θ=0.5 ⇒ t_stale = τ_π.
        double rate = DecayFunction.RateFromHalfLife(120);
        Assert.Equal(120.0, DecayFunction.DaysUntilStale(rate, staleThreshold: 0.5), precision: 9);
    }

    [Fact]
    public void DecayFunction_rejects_impossible_parameters()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => DecayFunction.RateFromHalfLife(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecayFunction.RateFromHalfLife(-30));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecayFunction.HalfLifeDays(-0.01));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecayFunction.HalfLifeDays(double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecayFunction.DaysUntilStale(0.01, staleThreshold: 0.0));
        Assert.Throws<ArgumentOutOfRangeException>(() => DecayFunction.DaysUntilStale(0.01, staleThreshold: 1.0));
    }

    [Fact]
    public void Faster_context_decays_faster_at_the_same_age_and_confidence()
    {
        // Sönümün TEK sürücüsü artık Purpose-tipinin volatilitesi: fiyat-context'i hızlı bayatlar,
        // tedarikçi güvenilirliği yavaş (ENS-2003 §3a, adaptive-decay-KG'nin kategoriye-koşullu decay'i).
        var memory = new CompanyMemory();
        var pricing = new MemoryRecord(Identity.New(), "fiyatlandırma", 5, 0.6, Now.AddDays(-180));
        var supplier = new MemoryRecord(Identity.New(), "tedarikçi-seçimi", 5, 0.6, Now.AddDays(-180));
        memory.Record(pricing); memory.Record(supplier);

        double fastRate = DecayFunction.RateFromHalfLife(30);    // τ = 30 gün
        double slowRate = DecayFunction.RateFromHalfLife(720);   // τ = 720 gün

        Assert.True(memory.DecayFactor(pricing, Now, fastRate) < memory.DecayFactor(supplier, Now, slowRate));
    }

    // --- v0.3: asserted_at (AssertedAt) vs last_verified ayrımı ---

    [Fact]
    public void Verify_resets_decay_clock_without_touching_original_evidence()
    {
        // NOT (AUDIT §5.4/b): `Verify` artık DecisionId'yi değil KAYDIN KENDİSİNİ alır —
        // aynı karardan iki öğrenim varsa birini doğrulamak diğerini tazelemez (çapraz-kirlenme).
        // Saat enjekte ediliyor ki "gelecek tarihli doğrulama reddi" testi sistem saatine
        // bağlı olmasın (determinizm).
        var memory = new CompanyMemory(new FixedTimeProvider(Now));
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.5, Now.AddDays(-300));
        memory.Record(record);

        double beforeVerify = memory.Salience(record, Now, contextDecayRate: 0.02);
        memory.Verify(record, Now.AddDays(-1), "yıllık gözden geçirmede teyit edildi"); // dün yeniden doğrulandı
        double afterVerify = memory.Salience(record, Now, contextDecayRate: 0.02);

        Assert.True(afterVerify > beforeVerify);
        Assert.Equal(Now.AddDays(-300), record.AssertedAt); // evidence hiç değişmedi
        Assert.Single(memory.Verifications);                // ve doğrulama iz bıraktı (§3a, audit)
    }

    [Fact]
    public void Verify_rejects_a_future_timestamp_so_decay_cannot_be_switched_off()
    {
        // TRACE: AUDIT.md §5.4/a — ileri tarihli damga, decay yasasını denetimsiz biçimde
        // devre dışı bırakıp kaydı Curator'a sonsuza dek görünmez yapıyordu.
        var memory = new CompanyMemory(new FixedTimeProvider(Now));
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.5, Now.AddDays(-300));
        memory.Record(record);

        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Verify(record, Now.AddDays(1), "sahte"));
        Assert.Empty(memory.Verifications);
    }

    [Fact]
    public void Verify_cannot_predate_the_evidence_or_move_backwards()
    {
        var memory = new CompanyMemory(new FixedTimeProvider(Now));
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.5, Now.AddDays(-300));
        memory.Record(record);

        // kanıttan önce doğrulama: anlamsız
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Verify(record, Now.AddDays(-400), "geçmişe dönük"));

        memory.Verify(record, Now.AddDays(-10), "ilk teyit");
        // saati geriye alma: decay geçmişi uydurulamaz (monotonluk)
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Verify(record, Now.AddDays(-20), "geri sarma"));
    }

    [Fact]
    public void LastVerifiedOf_falls_back_to_evidence_when_never_verified()
    {
        var memory = new CompanyMemory();
        var record = new MemoryRecord(Identity.New(), "x", 5, 0.5, Now.AddDays(-10));

        Assert.Equal(record.AssertedAt, memory.LastVerifiedOf(record));
    }

    // --- §3b Curator: İKİ sinyal — asla silmez, yalnızca bayraklar ---

    [Fact]
    public void FindStale_flags_decayed_records_without_removing_them()
    {
        var memory = new CompanyMemory();
        var stale = new MemoryRecord(Identity.New(), "x", 5, 0.3, Now.AddDays(-500));
        memory.Record(stale);

        var staleList = memory.FindStale(Now, contextDecayRate: 0.02, staleThreshold: 0.5);

        Assert.Contains(stale, staleList);
        Assert.Contains(stale, memory.AllRecords); // silinmedi
    }

    [Fact]
    public void FindStale_excludes_fresh_records_regardless_of_confidence()
    {
        var memory = new CompanyMemory();
        var freshCertain = new MemoryRecord(Identity.New(), "x", 5, 0.99, Now);
        var freshShaky = new MemoryRecord(Identity.New(), "x", 5, 0.05, Now);
        memory.Record(freshCertain); memory.Record(freshShaky);

        // Tazelik ekseni confidence'a bakmaz: ikisi de TAZE (v0.3'te `freshShaky` de tazeydi ama
        // yaşlandığında çok daha hızlı sönerdi — o asimetri kalktı).
        Assert.Empty(memory.FindStale(Now, contextDecayRate: 0.02, staleThreshold: 0.5));
    }

    [Fact]
    public void FindStale_out_of_range_threshold_throws()
    {
        var memory = new CompanyMemory();
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.FindStale(Now, 0.02, staleThreshold: 1.5));
    }

    [Fact]
    public void FindWeaklyAttributed_flags_the_epistemic_axis_and_is_independent_of_age()
    {
        // v0.4.0'ın TELAFİSİ (§3a "ikinci inceleme sinyali"): `c` sönümden çıkınca düşük-c kayıtlar
        // tazelik ekseninde bayraklanmaz oldu. Sinyal kaybolmadı — DOĞRU eksene taşındı.
        // Anlamı da farklı: "yeniden doğrula" değil, "attribution SEVİYESİNİ yükselt" (ENS-2004 §4a).
        var memory = new CompanyMemory();
        var freshButUnattributable = new MemoryRecord(Identity.New(), "talep-tahmini", 9, 0.25, Now);
        var oldButWellAttributed = new MemoryRecord(Identity.New(), "talep-tahmini", 9, 0.9, Now.AddDays(-900));
        memory.Record(freshButUnattributable); memory.Record(oldButWellAttributed);

        var weak = memory.FindWeaklyAttributed(minConfidence: 0.5);

        Assert.Contains(freshButUnattributable, weak);       // taze ama atfedilemez → epistemik bayrak
        Assert.DoesNotContain(oldButWellAttributed, weak);   // eski ama atfı sağlam → bu eksende temiz

        // ...ve tazelik ekseni TAM TERSİNİ söyler: iki sinyal birbirinin yerine geçmez.
        var stale = memory.FindStale(Now, contextDecayRate: 0.02, staleThreshold: 0.5);
        Assert.DoesNotContain(freshButUnattributable, stale);
        Assert.Contains(oldButWellAttributed, stale);
    }

    [Fact]
    public void Neither_curator_signal_ever_lowers_retention_priority_or_deletes()
    {
        // §3b/P7: iki bayrak da YALNIZCA sinyaldir; taban (§3) ikisinden de bağımsız durur.
        var memory = new CompanyMemory();
        var doublyFlagged = new MemoryRecord(Identity.New(), "t", LearningMagnitude: 9, AttributionConfidence: 0.1, Now.AddDays(-900));
        memory.Record(doublyFlagged);

        Assert.Contains(doublyFlagged, memory.FindStale(Now, 0.02, 0.5));
        Assert.Contains(doublyFlagged, memory.FindWeaklyAttributed(0.5));

        Assert.Equal(9.0, doublyFlagged.RetentionPriority, precision: 10);   // düşmedi
        Assert.Single(memory.AllRecords);                                    // silinmedi
        Assert.Same(doublyFlagged, memory.CounterSurvivorshipFloor("t"));    // ve hâlâ taban
    }

    [Fact]
    public void FindWeaklyAttributed_out_of_range_threshold_throws()
    {
        var memory = new CompanyMemory();
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.FindWeaklyAttributed(minConfidence: 1.5));
    }
}
