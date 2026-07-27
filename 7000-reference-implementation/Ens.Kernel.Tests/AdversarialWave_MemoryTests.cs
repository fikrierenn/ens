using System.Reflection;
using Ens.Kernel.Domain;
using Xunit;

namespace Ens.Kernel.Tests;

// ============================================================================================
// DUSMANCA TEST DALGASI 2 — MEMORY / LEARNING / CONTEXT
//
// Bagimsizlik beyani: bu dosyayi yazan context, CompanyMemory.cs / ReflectiveDoubleLoop.cs /
// ContextScore.cs'i YAZMADI. Onceki denetim (AUDIT.md 5.4) memory'de 4 decay-bypass buldu ve
// bunlar "duzeltildi". Bu dosyanin amaci duzeltmeye GUVENMEMEK: ayni yasayi (decay hic
// devre disi birakilamaz) BASKA kapilardan kirmaya calismak.
//
// ADLANDIRMA (skill: .claude/skills/adversarial-test/SKILL.md):
//   AUDIT_HOLDS_MEM_*   -> iddia saldiridan sag cikti; assertion = istenen davranis.
//   AUDIT_FIXED_MEM_*   -> eski kusur kapandi; assertion regresyon bekcisi.
//   AUDIT_DEFECT_MEM_*  -> kusur ACIK; assertion MEVCUT (kusurlu) davranisi sabitler.
//
// Turkce karakterler bilincli olarak \u escape ile yaziliyor (kaynak-kodlama riskini kaldirmak
// icin); yorumlar aksansiz Turkce.
// ============================================================================================

public sealed class AdversarialWaveMemoryTests
{
    private static readonly DateTimeOffset Now = new(2026, 7, 26, 0, 0, 0, TimeSpan.Zero);

    private static CompanyMemory MemoryAt(DateTimeOffset now) => new(new FixedTimeProvider(now));

    private static MemoryRecord Rec(
        string purpose = "tedarik",
        double magnitude = 5,
        double confidence = 0.5,
        DateTimeOffset? assertedAt = null) =>
        new(Identity.New(), purpose, magnitude, confidence, assertedAt ?? Now);

    /// <summary>
    /// Kodun formulunden BAGIMSIZ salience hesabi (tautoloji olmasin diye elle).
    /// ENS-2003 v0.4.0 §3a: Salience = value(m) x decayFactor = |L|*c * exp(-lambda_pi * dt).
    /// DIKKAT: `confidence` yalnizca CARPANDA gorunur, USTELDE ARTIK GORUNMEZ — v0.3'te
    /// `rate = baseRate * (1-c)^gamma` idi ve `c` iki kez sayiliyordu (AUDIT-WAVE2/D-5).
    /// </summary>
    private static double IndependentSalience(
        double magnitude, double confidence, DateTimeOffset assertedAt,
        DateTimeOffset asOf, double contextDecayRate)
    {
        double ageDays = Math.Max(0.0, (asOf - assertedAt).TotalDays);
        return magnitude * confidence * Math.Exp(-contextDecayRate * ageDays);
    }

    // ========================================================================================
    // A. DECAY BYPASS — "dort kapi kapandi" iddiasina BESINCI, ALTINCI, YEDINCI kapidan saldiri
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_MEM_A1_Future_AssertedAt_disables_decay_forever_with_no_evidence_and_no_trail()
    {
        // EN KRITIK BULGU. AUDIT 5.4/a `Verify(id, gelecek)` kapisini kapatti: ileri tarihli
        // DOGRULAMA damgasi artik reddediliyor, gerekce istiyor, monoton, ize birakiyor.
        // Ama decay saatinin FALLBACK'i `AssertedAt`'tir (LastVerifiedOf -> AssertedAt) ve
        // `AssertedAt` hicbir yerde "gelecekte olamaz" diye denetlenmez: ne MemoryRecord
        // kurucusunda (saati yok), ne CompanyMemory.Record'da (saati VAR ama kullanmiyor).
        //
        // Sonuc: saldirgan zor kapiyi (Verify) hic calmaz — kaydi dogrudan gelecek tarihle
        // YAZAR. `Math.Max(0, negatif yas)` sessiz kirpmasi devreye girer, decayFactor kalici
        // olarak 1.0'da kalir, Curator kaydi ASLA gormez. Ustelik `Verifications` BOS kalir:
        // denetim izinde bu manipulasyonun hicbir izi yoktur. Verify yolunda birakilan iz
        // burada YOK — yani ucuz yol, izsiz yoldur.
        var memory = MemoryAt(Now);
        var forged = Rec(magnitude: 5, confidence: 0.0, assertedAt: Now.AddYears(1000));
        memory.Record(forged);

        Assert.Equal(1.0, memory.DecayFactor(forged, Now, contextDecayRate: 1.0));
        Assert.Empty(memory.FindStale(Now, contextDecayRate: 1.0, staleThreshold: 0.99));
        Assert.Empty(memory.Verifications); // izsiz
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_A2_MaxValue_AssertedAt_is_accepted_and_is_immortal()
    {
        // Ayni acigin ust sinir hali: yil 9999. Tasma da yok, exception da yok — sadece
        // olumsuz kayit. confidence=0 (en hizli sonum) secildi ki savunma "zaten yavas soner"
        // diyemesin.
        var memory = MemoryAt(Now);
        var immortal = Rec(magnitude: 1, confidence: 0.0, assertedAt: DateTimeOffset.MaxValue);
        memory.Record(immortal);

        Assert.Equal(1.0, memory.DecayFactor(immortal, Now, contextDecayRate: double.MaxValue));
        Assert.Empty(memory.FindStale(Now, contextDecayRate: double.MaxValue, staleThreshold: 1.0));
    }

    [Fact]
    public void AUDIT_FIXED_MEM_A3_AttributionConfidence_of_one_no_longer_buys_a_permanent_decay_exemption()
    {
        // KUSURDU (v0.3.1): ENS-2003 3a "confidence->1 iken lambda->0 (asla sonmez) — formulun
        //   LIMITI" diyordu. Teoride limit, kodda NOKTA: `Math.Pow(1.0 - 1.0, gamma) == 0` -> rate
        //   tam 0. Yani `AttributionConfidence: 1.0` yazan HERKES, hicbir gerekce sunmadan, hicbir
        //   iz birakmadan, kaydini kalici olarak decay yasasinin ve Curator'in disina cikariyordu —
        //   `Verify` icin dort kisit + zorunlu gerekce istenirken.
        // KAPANDI (v0.4.0, AUDIT-WAVE2/D-5): sonum hizi artik attribution confidence'i HIC
        //   icermiyor (lambda_pi = ln2/tau_pi). Muafiyet kapisi bir guard'la degil, FORMULDEN
        //   kaldirilarak kapandi — yani atlatilamaz.
        var memory = MemoryAt(Now);
        var certain = Rec(magnitude: 5, confidence: 1.0, assertedAt: Now.AddYears(-100));
        memory.Record(certain);

        Assert.Equal(0.0, memory.DecayFactor(certain, Now, contextDecayRate: 10.0));
        Assert.Single(memory.FindStale(Now, contextDecayRate: 10.0, staleThreshold: 1.0));

        // Ve karsit kontrol: c=1.0 ile c=0.0, AYNI yasta, BIREBIR ayni tazelige sahip.
        var shaky = Rec(magnitude: 5, confidence: 0.0, assertedAt: Now.AddYears(-100));
        memory.Record(shaky);
        Assert.Equal(memory.DecayFactor(certain, Now, 0.001), memory.DecayFactor(shaky, Now, 0.001));
    }

    [Fact]
    public void AUDIT_FIXED_MEM_A4_The_gamma_switch_that_could_disable_the_curator_company_wide_no_longer_exists()
    {
        // KUSURDU (v0.3.1): lambda(c) = lambda_base * (1-c)^gamma; `gamma` "SERBEST parametre
        //   (tek kisit gamma>0)" idi ve Guard.PositiveFinite 1e6'yi kabul ediyordu. Ama
        //   (1-c)^gamma buyuk gamma'da IEEE-754'te TAM SIFIRA underflow eder -> rate tam 0 ->
        //   decayFactor tam 1.0 -> FindStale hicbir seyi bayraklamaz. `gamma` tek basina, TUM
        //   bellek icin decay'i kapatan denetimsiz bir anahtardi; AUDIT 5.4'un kapattigi dort kapi
        //   kayit BASINA idi, bu kapi KURUM CAPINDA.
        // KAPANDI (v0.4.0): sekil parametresi `gamma` KALDIRILDI (attribution confidence sonumden
        //   ciktigi icin (1-c)^gamma ailesinin konusu kalmadi). Anahtar yok, dolayisiyla acilamaz.
        //   REFLEKSIYONLA kanitlaniyor: hicbir memory API'si artik `gamma` almiyor.
        var gammaParams = typeof(CompanyMemory).GetMethods()
            .Concat(typeof(DecayFunction).GetMethods())
            .SelectMany(m => m.GetParameters())
            .Where(p => p.Name is not null && p.Name.Contains("gamma", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        Assert.Empty(gammaParams);

        // Ve davranissal kanit: 50 kayit, 50 yil. Eskiden gamma tek basina bunlarin HEPSINI
        // gorunmez yapabiliyordu; artik hizi kucultmek disinda bir kol yok — ve hiz kucuk olsa
        // bile (tau_pi ~ 693 gun) 50 yillik kayit bayraklanir.
        var memory = MemoryAt(Now);
        for (int i = 0; i < 50; i++)
            memory.Record(Rec(magnitude: 5, confidence: 0.5, assertedAt: Now.AddYears(-50)));

        Assert.Equal(50, memory.FindStale(Now, contextDecayRate: 1.0, staleThreshold: 0.5).Count);
        Assert.Equal(50, memory.FindStale(Now, contextDecayRate: 1e-3, staleThreshold: 0.5).Count);
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_A5_contextDecayRate_zero_is_still_an_undocumented_company_wide_decay_off_switch()
    {
        // ACIK BIRAKILDI — v0.4.0 `gamma` anahtarini kaldirdi ama ONU kaldirmadi: `contextDecayRate: 0`
        // Guard.NonNegativeFinite'ten gecer (0 >= 0) ve decayFactor'i TUM bellek icin tam 1.0'a
        // sabitler. Curator hicbir seyi bayraklamaz, hata da vermez, uyari da. Cagiran tarafindan
        // her cagriya serbestce verilebilir ve hicbir yere yazilmaz.
        //
        // NEDEN KAPATILMADI: `lambda_pi = 0` (sonsuz context yarı-omru) TEORIDE mesru bir degerdir —
        // hic bayatlamayan bir karar sinifi dusunulebilir. Kapatmak, `tau_pi`'yi ontolojiden ALMAK
        // ve cagiranin serbestce vermesini engellemek demektir; o da ENS-2003 v0.4.0 §Failure'daki
        // ACIK kalibrasyon borcudur (ontoloji `tau_pi` alanini henuz tasimiyor). Yani bu anahtar,
        // v0.4.0'in kendi ilan ettigi borcun test yuzeyindeki gorunumudur.
        var memory = MemoryAt(Now);
        memory.Record(Rec(magnitude: 5, confidence: 0.0, assertedAt: Now.AddYears(-500)));

        Assert.Equal(1.0, memory.DecayFactor(memory.AllRecords[0], Now, contextDecayRate: 0.0));
        Assert.Empty(memory.FindStale(Now, contextDecayRate: 0.0, staleThreshold: 0.999));
        Assert.True(double.IsPositiveInfinity(DecayFunction.HalfLifeDays(0.0)));
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_A6_Verify_can_never_increase_decay_only_reduce_it()
    {
        // "Gecmis tarihle dogrulama decay'i ARTIRIR mi?" -> Hayir, iki kisit birlikte kapatiyor:
        // (c) at >= AssertedAt ve (d) at >= onceki damga. Ilk damga zaten AssertedAt'ten geride
        // olamaz, sonrakiler geri gidemez. Yani `Verify` monoton olarak yalnizca tazeler.
        var memory = MemoryAt(Now);
        var record = Rec(confidence: 0.3, assertedAt: Now.AddDays(-300));
        memory.Record(record);

        double before = memory.DecayFactor(record, Now, 0.02);
        memory.Verify(record, Now.AddDays(-299), "ilk teyit");
        double after = memory.DecayFactor(record, Now, 0.02);
        Assert.True(after >= before);

        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Verify(record, Now.AddDays(-400), "geri"));
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Verify(record, Now.AddDays(-300), "geri"));
        Assert.Throws<ArgumentOutOfRangeException>(() => memory.Verify(record, Now.AddDays(1), "ileri"));
        Assert.Equal(after, memory.DecayFactor(record, Now, 0.02));
    }

    // ========================================================================================
    // B. VERIFY GUARD'I — "gerekcesiz dogrulama olamaz" iddiasini kirmak
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_MEM_B1_The_mandatory_evidence_guard_is_defeated_by_its_own_default_parameter()
    {
        // Guard mesaji: "Yeniden-dogrulama gerekcesiz olamaz — izsiz dogrulama, izsiz
        // turetimdir (Madde VI)." Ama `evidence` parametresi OPSIYONELDIR ve derleyicinin
        // gomdugu bir yer-tutucu varsayilani vardir. Yani hicbir cagiran gerekce vermek
        // ZORUNDA degildir; guard yalnizca "acikca bos string yazanlari" yakalar.
        // Skill 5 (guard'in kozmetik olmasi) tam olarak budur: mesaj bir sey iddia ediyor,
        // imza onu zorlamiyor.
        var memory = MemoryAt(Now);
        var record = Rec(assertedAt: Now.AddDays(-100));
        memory.Record(record);

        memory.Verify(record, Now); // gerekce YOK — yine de gecti

        ParameterInfo evidenceParam = typeof(CompanyMemory)
            .GetMethod(nameof(CompanyMemory.Verify))!
            .GetParameters()
            .Single(p => p.Name == "evidence");

        Assert.True(evidenceParam.HasDefaultValue);
        Assert.Equal(evidenceParam.DefaultValue, memory.Verifications[0].Evidence);
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_B2_Any_non_whitespace_character_passes_as_evidence()
    {
        // IsNullOrWhiteSpace yalnizca BOSLUK'u eler. Tek nokta, tek harf, rastgele gurultu
        // "kanit" sayilir. ENS-4025 L8'in tipli proof-trace'i bagli olmadigi icin gerekce
        // alani anlamsal olarak DENETIMSIZDIR (dosyada "acik borc" olarak isaretli — bu test
        // borcun buyuklugunu olcuyor: gerekce alani su an sifir bilgi tasiyabilir).
        var memory = MemoryAt(Now);
        var record = Rec(assertedAt: Now.AddDays(-100));
        memory.Record(record);

        memory.Verify(record, Now.AddDays(-3), ".");
        memory.Verify(record, Now.AddDays(-2), "0");
        memory.Verify(record, Now.AddDays(-1), "asdfgh");

        Assert.Equal(3, memory.Verifications.Count);
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_B3_Empty_and_whitespace_evidence_are_rejected_and_leave_no_trace()
    {
        var memory = MemoryAt(Now);
        var record = Rec(assertedAt: Now.AddDays(-100));
        memory.Record(record);

        foreach (string bad in new[] { "", " ", "\t", "\n", "   \r\n  " })
            Assert.Throws<ArgumentException>(() => memory.Verify(record, Now, bad));

        Assert.Throws<ArgumentNullException>(() => memory.Verify(null!, Now, "x"));
        Assert.Empty(memory.Verifications);
        Assert.Equal(record.AssertedAt, memory.LastVerifiedOf(record));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_B4_The_same_record_can_be_verified_1000_times_at_the_same_instant()
    {
        // Monotonluk kontrolu `at < previous` — yani `at == previous` SERBEST. Ayni ani 1000
        // kez "dogrulamak" mumkun; hicbiri decay'i degistirmez ama denetim izini 1000 sahte
        // satirla doldurur. Idempotans/dedup yok, ust sinir yok. Bu bir DoS degil ama izin
        // sinyal-gurultu oranini dusuren, incelemeyi pratikte imkansizlastiran bir vektordur
        // (P5 dikkat butcesi ile ayni aile).
        var memory = MemoryAt(Now);
        var record = Rec(assertedAt: Now.AddDays(-100));
        memory.Record(record);

        for (int i = 0; i < 1000; i++)
            memory.Verify(record, Now.AddDays(-1), "spam");

        Assert.Equal(1000, memory.Verifications.Count);
        Assert.Equal(Now.AddDays(-1), memory.LastVerifiedOf(record));
    }

    // ========================================================================================
    // C. KIMLIK — "decay saati KAYIT bazinda tutulur" iddiasi
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_MEM_C1_Two_learnings_from_the_same_DecisionId_no_longer_share_a_decay_clock()
    {
        // KUSURDU (AUDIT 5.4/b): saat DecisionId bazinda tutuluyordu; ayni karardan iki
        // ogrenim varsa birini dogrulamak digerini de tazeliyordu.
        // KAPANDI: sozluk anahtari artik MemoryRecord.
        var memory = MemoryAt(Now);
        var id = Identity.New();
        var a = new MemoryRecord(id, "tedarik", 5, 0.5, Now.AddDays(-300));
        var b = new MemoryRecord(id, "tedarik", 9, 0.5, Now.AddDays(-300));
        memory.Record(a);
        memory.Record(b);

        memory.Verify(a, Now, "a icin teyit");

        Assert.Equal(Now, memory.LastVerifiedOf(a));
        Assert.Equal(b.AssertedAt, memory.LastVerifiedOf(b)); // b tazelenmedi
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_C2_The_clock_is_keyed_by_VALUE_not_by_record_identity_so_clones_still_cross_contaminate()
    {
        // Duzeltme kusuru DecisionId'den TAM-DEGER esitligine daralti, KALDIRMADI. MemoryRecord
        // bir `record`tur: esitlik/hashcode tum alanlar uzerinden deger-tabanlidir. Iki AYRI
        // nesne, alanlari ayni ise `_lastVerified` sozlugunde AYNI hucredir.
        // Dosya yorumu "decay saati KAYIT bazinda tutulur" diyor — dogrusu "DEGER bazinda".
        var memory = MemoryAt(Now);
        var id = Identity.New();
        var first = new MemoryRecord(id, "tedarik", 5, 0.5, Now.AddDays(-300));
        var twin = new MemoryRecord(id, "tedarik", 5, 0.5, Now.AddDays(-300));

        Assert.NotSame(first, twin);
        Assert.Equal(first, twin); // deger esitligi

        memory.Record(first);
        memory.Record(twin);
        Assert.Equal(2, memory.AllRecords.Count);

        memory.Verify(first, Now, "yalnizca ilkini dogruladim");
        Assert.Equal(Now, memory.LastVerifiedOf(twin)); // ama ikincisi de tazelendi
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_C3_The_ghost_record_guard_is_bypassed_by_forging_a_value_equal_clone()
    {
        // Kisit (a): "Bu bellege yazilmamis bir kayit yeniden-dogrulanamaz." Kontrol
        // `_index.Contains(record)` -> HashSet deger esitligi kullanir. Yani bellege HIC
        // yazilmamis bir nesne, yazilmis birinin alan-klonu ise guard'i gecer; ustelik
        // dogrulama izine YAZILMAMIS nesne duser (Verifications[0].Record == ghost).
        var memory = MemoryAt(Now);
        var id = Identity.New();
        var real = new MemoryRecord(id, "tedarik", 5, 0.5, Now.AddDays(-300));
        memory.Record(real);

        var ghost = new MemoryRecord(id, "tedarik", 5, 0.5, Now.AddDays(-300));
        Assert.DoesNotContain(memory.AllRecords, r => ReferenceEquals(r, ghost));

        memory.Verify(ghost, Now, "hayalet uzerinden");

        Assert.Equal(Now, memory.LastVerifiedOf(real)); // gercek kaydin saati hayaletle tazelendi
        Assert.Same(ghost, memory.Verifications[0].Record);
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_C4_A_genuinely_unknown_record_still_cannot_be_verified()
    {
        var memory = MemoryAt(Now);
        memory.Record(Rec(magnitude: 5));
        var unknown = Rec(magnitude: 7);

        Assert.Throws<InvalidOperationException>(() => memory.Verify(unknown, Now, "kanit"));
        Assert.Empty(memory.Verifications);
    }

    // ========================================================================================
    // D. ZAMAN SEMANTIGI — offset, yaz saati, MinValue/MaxValue, "as-of" nedenselligi
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_MEM_D1_The_same_instant_in_different_offsets_yields_bit_identical_decay()
    {
        // Saldiri: ayni ani +00:00 ve +03:00 ile ifade edip farkli decay elde etmek.
        // DateTimeOffset aritmetigi AN tabanlidir (UtcTicks farki) — sonuc bit-birebir ayni.
        var memory = MemoryAt(Now);
        var record = Rec(confidence: 0.4, assertedAt: new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        memory.Record(record);

        var utc = new DateTimeOffset(2026, 7, 26, 0, 0, 0, TimeSpan.Zero);
        var istanbul = new DateTimeOffset(2026, 7, 26, 3, 0, 0, TimeSpan.FromHours(3));
        var chatham = new DateTimeOffset(2026, 7, 26, 12, 45, 0, TimeSpan.FromHours(12.75));

        Assert.Equal(utc, istanbul);
        Assert.Equal(memory.DecayFactor(record, utc, 0.02), memory.DecayFactor(record, istanbul, 0.02));
        Assert.Equal(memory.DecayFactor(record, utc, 0.02), memory.DecayFactor(record, chatham, 0.02));
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_D2_A_23_hour_DST_day_decays_by_23_hours_not_24()
    {
        // Yaz saati gecisi (Orta Avrupa, 2026-03-29: +01:00 -> +02:00). Duvar saatiyle "ertesi
        // gun ayni saat" GERCEKTE 23 saattir. Decay duvar saatine degil ANA baglandigi icin
        // 23 saatlik sonum uygular — dogru davranis. (Eger kod .Date/DateTime kullansaydi
        // burada 24 saat cikardi ve gunluk decay sessizce sismis olurdu.)
        var beforeDst = new DateTimeOffset(2026, 3, 28, 12, 0, 0, TimeSpan.FromHours(1));
        var afterDst = new DateTimeOffset(2026, 3, 29, 12, 0, 0, TimeSpan.FromHours(2));
        Assert.Equal(23.0, (afterDst - beforeDst).TotalHours, precision: 10);

        var memory = MemoryAt(afterDst);
        var record = Rec(confidence: 0.0, assertedAt: beforeDst);
        memory.Record(record);

        Assert.Equal(Math.Exp(-1.0 * (23.0 / 24.0)), memory.DecayFactor(record, afterDst, contextDecayRate: 1.0), precision: 12);
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_D3_MinValue_to_MaxValue_span_neither_overflows_nor_produces_NaN()
    {
        var memory = MemoryAt(Now);
        var ancient = Rec(confidence: 0.0, assertedAt: DateTimeOffset.MinValue);
        memory.Record(ancient);

        double far = memory.DecayFactor(ancient, DateTimeOffset.MaxValue, contextDecayRate: 0.02);
        double none = memory.DecayFactor(ancient, DateTimeOffset.MinValue, contextDecayRate: 0.02);

        Assert.False(double.IsNaN(far));
        Assert.Equal(0.0, far);   // tamamen sonmus
        Assert.Equal(1.0, none);  // yasi sifir
        Assert.False(double.IsNaN(memory.Salience(ancient, DateTimeOffset.MaxValue, double.MaxValue)));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_D4_Retrieve_leaks_records_that_did_not_exist_yet_at_asOf_and_ranks_them_first()
    {
        // `EffectiveVerifiedAt` nedensellik kisitini DOGRULAMALARA uygular ("asOf'tan sonraki
        // dogrulamadan yararlanilamaz") ama KAYDIN VAR OLUSUNA uygulamaz: `Retrieve`/`FindStale`
        // icinde `AssertedAt <= asOf` filtresi yoktur.
        //
        // Sonuc: "2026'da ne biliyorduk?" sorusu, 2030'da yazilmis kaydi dondurur — ustelik
        // yasi max(0, negatif)=0 oldugu icin TAM TAZELIKTE, yani siralamanin BASINDA. Bu,
        // event-sourced bir sistemde replay/point-in-time sorgusunu yanlislar ve geriye donuk
        // "biz bunu zaten biliyorduk" iddiasina teknik zemin hazirlar.
        var memory = MemoryAt(Now);
        var known = new MemoryRecord(Identity.New(), "tedarik", 10, 0.9, Now.AddDays(-10));
        var future = new MemoryRecord(Identity.New(), "tedarik", 10, 0.9, Now.AddYears(4));
        memory.Record(known);
        memory.Record(future);

        var asOfPast = memory.Retrieve("tedarik", Now.AddDays(-5), contextDecayRate: 0.05);

        Assert.Equal(2, asOfPast.Count);
        Assert.Same(future, asOfPast[0]);
        Assert.Equal(1.0, memory.DecayFactor(future, Now.AddDays(-5), 0.05));
    }

    // ========================================================================================
    // E. RETRIEVAL SIRALAMASI VE FINDSTALE ESIGI — fuzz + sinir
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_MEM_E1_Retrieve_ordering_matches_an_independently_computed_salience_over_500_random_records()
    {
        // Skill 7: koddan cagirip kendisiyle karsilastirmak tautolojidir. Beklenen salience
        // burada ELLE (mag*conf*exp(-lambda_pi*yas)) hesaplaniyor — ENS-2003 v0.4.0 §3a.
        var rng = new Random(20260726);
        var memory = MemoryAt(Now);
        var expected = new List<(MemoryRecord R, double S)>();

        const double contextDecayRate = 0.03;

        for (int i = 0; i < 500; i++)
        {
            double mag = rng.NextDouble() * 100;
            double conf = rng.NextDouble();
            var asserted = Now.AddDays(-rng.NextDouble() * 2000);
            var r = new MemoryRecord(Identity.New(), "tedarik", mag, conf, asserted);
            memory.Record(r);
            expected.Add((r, IndependentSalience(mag, conf, asserted, Now, contextDecayRate)));
        }

        var ordered = memory.Retrieve("tedarik", Now, contextDecayRate);
        Assert.Equal(500, ordered.Count);

        var lookup = expected.ToDictionary(e => e.R, e => e.S);
        for (int i = 0; i + 1 < ordered.Count; i++)
            Assert.True(lookup[ordered[i]] >= lookup[ordered[i + 1]],
                $"siralama {i}. konumda bozuk: {lookup[ordered[i]]} < {lookup[ordered[i + 1]]}");

        // Ve kodun kendi salience'i elle hesaplananla ayni olmali (formul sadakati).
        foreach (var (r, s) in expected)
            Assert.Equal(s, memory.Salience(r, Now, contextDecayRate), precision: 12);
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_E1b_Attribution_confidence_has_ZERO_influence_on_the_freshness_axis_over_500_random_pairs()
    {
        // v0.4.0'in cekirdek iddiasinin (AUDIT-WAVE2/D-5) FUZZ ile saldiriya ugramis hali:
        // yalnizca `AttributionConfidence`i farkli iki kayit, HER yasta ve HER hizda BIREBIR ayni
        // decayFactor'a sahip olmali. v0.3'te bu testin hicbir satiri gecmezdi.
        // Bu, "cift-sayim geri geldi" regresyonunu tek basina yakalar.
        var rng = new Random(20040725);
        var memory = MemoryAt(Now);

        for (int i = 0; i < 500; i++)
        {
            double confA = rng.NextDouble();
            double confB = rng.NextDouble();
            double rate = rng.NextDouble() * 0.1;
            var asserted = Now.AddDays(-rng.NextDouble() * 3000);

            var a = new MemoryRecord(Identity.New(), "x", 5.0, confA, asserted);
            var b = new MemoryRecord(Identity.New(), "x", 5.0, confB, asserted);
            memory.Record(a); memory.Record(b);

            Assert.Equal(memory.DecayFactor(a, Now, rate), memory.DecayFactor(b, Now, rate));

            // ...ve `c` YALNIZCA epistemik agirlikta gorunur (tek sayim, dogru yerde):
            Assert.Equal(5.0, a.RetentionPriority);
            Assert.Equal(5.0 * confA, a.CapitalValue, precision: 12);
        }
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_E2_Salience_is_never_NaN_or_negative_under_extreme_parameter_fuzz()
    {
        var rng = new Random(7);
        var memory = MemoryAt(Now);

        for (int i = 0; i < 500; i++)
        {
            double mag = rng.NextDouble() * 1e300;
            double conf = rng.Next(0, 3) switch { 0 => 0.0, 1 => 1.0, _ => rng.NextDouble() };
            double rate = rng.Next(0, 4) switch { 0 => 0.0, 1 => double.MaxValue, 2 => double.Epsilon, _ => rng.NextDouble() };
            var asserted = Now.AddDays(-rng.NextDouble() * 500_000); // ~1369 yil: DateTimeOffset araliginda kalir

            var r = new MemoryRecord(Identity.New(), "x", mag, conf, asserted);
            memory.Record(r);

            double s = memory.Salience(r, Now, rate);
            double d = memory.DecayFactor(r, Now, rate);

            Assert.False(double.IsNaN(s), $"NaN salience: mag={mag} conf={conf} rate={rate}");
            Assert.True(s >= 0 && double.IsFinite(s));
            Assert.True(d is >= 0.0 and <= 1.0, $"decayFactor aralik disi: {d}");
        }
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_E3_FindStale_is_a_strict_threshold_and_no_record_is_both_stale_and_fresh()
    {
        // Sinir: decayFactor TAM esik degerine esitse -> stale DEGIL (strict `<`). Tam esitlik
        // contextDecayRate=0 ile deterministik olarak uretiliyor (decayFactor tam 1.0, esik 1.0).
        var memory = MemoryAt(Now);
        var onBoundary = Rec(confidence: 0.5, assertedAt: Now.AddYears(-40));
        memory.Record(onBoundary);

        Assert.Equal(1.0, memory.DecayFactor(onBoundary, Now, contextDecayRate: 0.0));
        Assert.Empty(memory.FindStale(Now, contextDecayRate: 0.0, staleThreshold: 1.0));

        // Ve fuzz: FindStale uyeligi, DecayFactor ile HER ZAMAN tutarli (ikili yargi yok).
        var rng = new Random(99);
        for (int i = 0; i < 200; i++)
            memory.Record(Rec(confidence: rng.NextDouble(), assertedAt: Now.AddDays(-rng.NextDouble() * 3000)));

        const double threshold = 0.5;
        var stale = memory.FindStale(Now, 0.02, threshold).ToHashSet();
        foreach (var r in memory.AllRecords)
            Assert.Equal(memory.DecayFactor(r, Now, 0.02) < threshold, stale.Contains(r));

        // Determinizm: iki cagri ayni kumeyi verir.
        Assert.Equal(stale.Count, memory.FindStale(Now, 0.02, threshold).Count);
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_E4_staleThreshold_zero_is_a_silent_global_curator_off_switch()
    {
        // decayFactor daima >= 0 oldugundan `decayFactor < 0.0` HICBIR ZAMAN dogru olamaz.
        // Yani `staleThreshold: 0` gecerli bir girdidir (Guard.UnitInterval kabul eder) ve
        // Curator'i tamamen kapatir — hata da vermez, uyari da. "0 kayit bayraklandi" ciktisi
        // "bellek saglikli" ile "sweep kapali" arasinda ayirt edilemez (skill 6: cherry-picked
        // parametre). Bu, A4'teki gamma anahtarinin ikinci, daha ucuz surumudur.
        var memory = MemoryAt(Now);
        memory.Record(Rec(magnitude: 5, confidence: 0.0, assertedAt: Now.AddYears(-500)));

        Assert.Equal(0.0, memory.DecayFactor(memory.AllRecords[0], Now, contextDecayRate: 1.0));
        Assert.Empty(memory.FindStale(Now, contextDecayRate: 1.0, staleThreshold: 0.0));
        Assert.Single(memory.FindStale(Now, contextDecayRate: 1.0, staleThreshold: double.Epsilon));
    }

    [Fact]
    public void AUDIT_FIXED_MEM_E5_Zero_learning_and_zero_confidence_records_are_no_longer_invisible_to_the_curator()
    {
        // KUSURDU (AUDIT 5.4/c): filtre `RetentionPriority > 0 && Salience/RetentionPriority
        // < esik` idi; |Learning|=0 veya confidence=0 olan kayit 100 yil gecse de ASLA
        // bayraklanmiyordu. KAPANDI: tazelik ekseni dogrudan hesaplaniyor.
        var memory = MemoryAt(Now);
        var zeroLearning = Rec(magnitude: 0, confidence: 0.1, assertedAt: Now.AddDays(-500));
        var zeroConfidence = Rec(magnitude: 5, confidence: 0.0, assertedAt: Now.AddDays(-500));
        memory.Record(zeroLearning);
        memory.Record(zeroConfidence);

        var stale = memory.FindStale(Now, contextDecayRate: 0.02, staleThreshold: 0.5);
        Assert.Contains(zeroLearning, stale);
        Assert.Contains(zeroConfidence, stale);
        Assert.Equal(2, memory.AllRecords.Count); // ve hicbiri silinmedi
    }

    [Fact]
    public void AUDIT_FIXED_MEM_E6_Numeric_validation_now_happens_at_the_gate_not_per_record()
    {
        // KUSURDU: Guard yalnizca kayit BASINA calisiyordu (OrderByDescending / Where icinden).
        // Eslesen kayit yoksa NaN oran hicbir sey soylemeden kabul ediliyordu — yani ayni hatali
        // cagri, bellek bosken SESSIZ, doluyken PATLIYORDU. Hata ani VERIYE BAGLIYDI; bu, bir
        // kalibrasyon hatasinin uretimde aylarca gizlenip ilk eslesen kayitta patlamasi demek.
        // KAPANDI: `Retrieve`/`FindStale` artik `contextDecayRate`'i KAPIDA dogruluyor
        // (Guard.NonNegativeFinite) — fail-closed politikasi (Guard.cs) tutarli uygulaniyor.
        var memory = MemoryAt(Now);

        // Bos bellek ARTIK maskelemiyor: eslesen kayit olmasa da gecersiz oran reddediliyor.
        Assert.Throws<ArgumentOutOfRangeException>(
            () => memory.Retrieve("hic-yok", Now, contextDecayRate: double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => memory.FindStale(Now, contextDecayRate: double.NaN));

        // Ve dolu bellekte de — davranis artik VERIDEN BAGIMSIZ.
        memory.Record(Rec());
        Assert.Throws<ArgumentOutOfRangeException>(
            () => memory.Retrieve("hic-yok", Now, contextDecayRate: double.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(
            () => memory.FindStale(Now, double.NaN));

        // Gecerli oran her iki durumda da calismaya devam eder (asiri-siki degil).
        Assert.Empty(memory.Retrieve("hic-yok", Now, contextDecayRate: 0.01));
    }

    // ========================================================================================
    // F. DEGISMEZLIK — downcast ile "silinemez bellek" yalanini arama
    // ========================================================================================

    [Fact]
    public void AUDIT_FIXED_MEM_F1_AllRecords_and_Verifications_survive_every_downcast_mutation_attempt()
    {
        // KUSURDU (AUDIT 5.2): canli List donuyordu, ((List<T>)AllRecords).Clear() ile kurumsal
        // bellek tek satirda silinebiliyordu. KAPANDI: ReadOnlyCollection sarmalayici.
        var memory = MemoryAt(Now);
        var record = Rec(assertedAt: Now.AddDays(-10));
        memory.Record(record);
        memory.Verify(record, Now, "kanit");

        Assert.Throws<InvalidCastException>(() => { _ = (List<MemoryRecord>)memory.AllRecords; });
        Assert.Throws<NotSupportedException>(() => { ((IList<MemoryRecord>)memory.AllRecords).Clear(); });
        Assert.Throws<NotSupportedException>(() => { ((ICollection<MemoryRecord>)memory.AllRecords).Remove(record); });
        Assert.Throws<NotSupportedException>(() => { ((IList<MemoryVerification>)memory.Verifications).Clear(); });

        Assert.Single(memory.AllRecords);
        Assert.Single(memory.Verifications);
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_F2_Mutating_a_Retrieve_result_cannot_touch_the_underlying_memory()
    {
        // `Retrieve`/`FindStale` .ToList() dondurur; IReadOnlyList olarak ilan edilse de
        // List'e downcast edilip degistirilebilir. Bu bir KOPYA oldugu icin bellek etkilenmez
        // — ama "salt-okunur" tipin burada sozlesme degil, yalnizca niyet oldugunu belgeler.
        var memory = MemoryAt(Now);
        memory.Record(Rec(assertedAt: Now.AddDays(-900)));
        memory.Record(Rec(assertedAt: Now.AddDays(-800)));

        var result = memory.Retrieve("tedarik", Now, 0.02);
        ((List<MemoryRecord>)result).Clear(); // downcast BASARILI (kopya oldugu icin zararsiz)

        Assert.Empty(result);
        Assert.Equal(2, memory.AllRecords.Count);
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_F3_Unicode_equivalent_purpose_types_split_memory_into_unreachable_shards()
    {
        // PurposeType karsilastirmasi ORDINAL string esitligidir: normalizasyon (NFC/NFD),
        // buyuk-kucuk harf ya da kirpma yok. Gorsel olarak AYNI olan iki tip (U+0130 vs
        // "i"+U+0307, ya da sondaki bosluk) ayri kovalar olusturur. Kullanici "bu tipte ne
        // ogrendik?" diye sordugunda kayitlarin bir kismi HIC donmez — sessiz kurumsal amnezi.
        // Ustelik bu, tespitten kacmak icin kasitli olarak da kullanilabilir (bkz. G3).
        //
        // NOT: literaller bilincli olarak \u escape ile yazildi — kaynak dosya kodlamasi ya da
        // bir editor normalizasyonu iki literali sessizce esitlerse test YANLIS gecerdi.
        // Bu tur `dotnet test` CALISTIRILAMADIGI icin (bkz. AUDIT-WAVE2-MEMORY.md 1) NFC/NFD
        // varyanti asagida DEVRE DISI birakildi; iddia ASCII-kesin varyantlarla kanitlaniyor.
        // NFC/NFD kolu, calistirilabilir bir turda yeniden acilmalidir.
        /*
        const string composed = "İstanbul-tedarik";      // tek kod noktasi: U+0130
        const string decomposed = "İstanbul-tedarik";   // "I" + U+0307 birlesen nokta
        const string padded = "İstanbul-tedarik ";       // sondaki bosluk
        */
        const string canonical = "tedarik-secimi";
        const string cased = "Tedarik-Secimi";
        const string padded = "tedarik-secimi ";

        var memory = MemoryAt(Now);
        /* Ilk taslaktaki bildirimler devre disi — yukaridaki const'lar gecerli:
        var composed = "İstanbul-tedarik";        // I with dot above (tek kod noktasi)
        var decomposed = "İstanbul-tedarik";     // I + combining dot above
        var padded = "İstanbul-tedarik ";         // sondaki bosluk
        */

        memory.Record(Rec(purpose: canonical, assertedAt: Now));
        memory.Record(Rec(purpose: cased, assertedAt: Now));
        memory.Record(Rec(purpose: padded, assertedAt: Now));

        // Insan icin TEK bir karar sinifi; bellek icin UC ayri, birbirini hic gormeyen kova.
        Assert.Equal(3, memory.AllRecords.Count);
        Assert.Single(memory.Retrieve(canonical, Now, 0.02));
        Assert.Single(memory.Retrieve(cased, Now, 0.02));
        Assert.Single(memory.Retrieve(padded, Now, 0.02));
    }

    // ========================================================================================
    // G. REFLECTIVE DOUBLE-LOOP — P5 (oneri-yorgunlugu), P7 (mutasyon yok), tespitten kacma
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_MEM_G1_Ten_thousand_records_produce_five_thousand_undifferentiated_proposals()
    {
        // ENS-2004 Failure "oneri-yorgunlugu" ve ReflectiveDoubleLoop.cs sadelestirme (c) bunu
        // ACIK BORC olarak isaretliyor. Bu test borcun BUYUKLUGUNU olcuyor: siniri yok, sirasi
        // yok, onceliklendirmesi (Decision Gravity) yok. 10.000 kayit -> 5.000 oneri; hepsi
        // P7 kapisindan bir insanin gecirmesi gereken esit-agirlikli oneri.
        //
        // P5 acisindan sonuc, hicbir oneri uretmemekten DAHA KOTUDUR: 5.000 oneri ya
        // rubber-stamp'e ya toptan gormezden gelmeye dejenere olur — yani P7 kapisi pratikte
        // acilir. "Yapisal olarak uygulanamaz" iddiasi teknik olarak dogru kalir ama insan
        // onayinin ANLAMI bu hacimde bosalir.
        var records = new List<MemoryRecord>(10_000);
        for (int i = 0; i < 5_000; i++)
        {
            records.Add(Rec(purpose: $"tip-{i}", magnitude: 9));
            records.Add(Rec(purpose: $"tip-{i}", magnitude: 9));
        }

        var proposals = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 2, magnitudeThreshold: 5.0);

        Assert.Equal(5_000, proposals.Count);
        // Birebir tekrar YOK (gruplama anahtari benzersiz) — ama ayirt edici bilgi de yok:
        // tum aciklamalar ayni sablonun kopyasi, hicbir oncelik alani mevcut degil.
        Assert.Equal(proposals.Count, proposals.Select(p => p.PurposeType).Distinct().Count());
        Assert.All(proposals, p => Assert.Equal(2, p.SupportingRecordCount));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_G2_A_zero_magnitude_threshold_turns_every_purpose_type_into_a_proposal()
    {
        // `magnitudeThreshold: 0` guard'dan gecer (">= 0 olmali"). Ama |Learning| >= 0 TANIM
        // GEREGI her kayit icin dogrudur — yani esik 0 iken "buyuk-buyuklukte ogrenim sinyali
        // tekrar etti" onerisi, hicbir sey ogrenilmemis kayitlardan da uretilir. Guard sayiyi
        // dogruluyor, ANLAMINI dogrulamiyor (skill 5).
        var records = new[]
        {
            Rec(purpose: "bos-1", magnitude: 0), Rec(purpose: "bos-1", magnitude: 0),
            Rec(purpose: "bos-2", magnitude: 0), Rec(purpose: "bos-2", magnitude: 0),
        };

        var proposals = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 2, magnitudeThreshold: 0.0);

        Assert.Equal(2, proposals.Count);
        Assert.All(proposals, p => Assert.Equal(2, p.SupportingRecordCount));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_G3_Padding_the_purpose_type_evades_systematic_pattern_detection_entirely()
    {
        // F3'un saldiri hali. Gruplama ORDINAL string esitligi uzerinden yapildigi icin, ayni
        // karar sinifini iki yazimla kaydetmek grup sayilarini esigin ALTINA dusurur ve
        // sistematik oruntu HIC tespit edilmez. Bir ajanin (ya da ekibin) kendi karar sinifini
        // incelemeden kacirmasi icin gereken tek sey, tipin sonuna bir bosluk koymaktir.
        var honest = Enumerable.Range(0, 6).Select(_ => Rec(purpose: "fiyatlandirma", magnitude: 9)).ToArray();
        Assert.Single(ReflectiveDoubleLoop.Propose(honest, minSupportingRecords: 5, magnitudeThreshold: 5.0));

        var evasive = honest.Take(4)
            .Concat(Enumerable.Range(0, 2).Select(_ => Rec(purpose: "fiyatlandirma ", magnitude: 9)))
            .ToArray();

        Assert.Equal(6, evasive.Length);
        Assert.Empty(ReflectiveDoubleLoop.Propose(evasive, minSupportingRecords: 5, magnitudeThreshold: 5.0));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_G4_Case_variants_of_one_decision_class_yield_two_separate_proposals()
    {
        // Ters yon: ayni sinif, iki yazim -> iki AYRI oneri. Insan gozunde bu tekrardir
        // (G1'deki yorgunlugu carpan etkiyle buyutur), makine gozunde iki farkli sinif.
        var records = Enumerable.Range(0, 3).Select(_ => Rec(purpose: "fiyatlandirma", magnitude: 9))
            .Concat(Enumerable.Range(0, 3).Select(_ => Rec(purpose: "Fiyatlandirma", magnitude: 9)))
            .ToArray();

        var proposals = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 3, magnitudeThreshold: 5.0);

        Assert.Equal(2, proposals.Count);
        Assert.All(proposals, p => Assert.Equal(3, p.SupportingRecordCount));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_G5_Proposals_can_be_manufactured_from_forged_records_with_zero_provenance()
    {
        // `Propose` bir IEnumerable<MemoryRecord> alir; kayitlarin herhangi bir CompanyMemory'ye
        // YAZILMIS olmasi gerekmez ve MemoryRecord kurucusu public'tir. Yani uc nesne uydurup
        // "sistematik oruntu" onerisi imal etmek mumkundur; uretilen ReflectiveProposal ise
        // kaynagina geri goturen HICBIR alan tasimaz (ne DecisionId, ne trace, ne bellek
        // referansi). Insan onaycisi (P7) oneriyi bagimsizca dogrulayamaz — ENS-4025 L8
        // proof-trace borcunun somut maliyeti budur.
        var forged = Enumerable.Range(0, 3).Select(_ => Rec(purpose: "uydurma", magnitude: 42)).ToArray();

        var p = Assert.Single(ReflectiveDoubleLoop.Propose(forged, minSupportingRecords: 3, magnitudeThreshold: 5.0));
        Assert.Equal("uydurma", p.PurposeType);

        string properties = string.Join(",", typeof(ReflectiveProposal).GetProperties()
            .Select(x => x.Name).OrderBy(x => x, StringComparer.Ordinal));
        Assert.Equal("AverageAttributionConfidence,Description,PurposeType,SupportingRecordCount", properties);
    }

    [Fact]
    public void AUDIT_FIXED_MEM_G6_The_minSupportingRecords_guard_is_no_longer_cosmetic()
    {
        // KUSURDU (AUDIT 5.6): mesaj "tek gozlemden sistematik iddia edilemez" diyordu ama
        // minSupportingRecords: 1 KABUL ediliyordu. KAPANDI: < 2 artik reddediliyor ve tek
        // gozlem gercekten oneri uretmiyor.
        Assert.Throws<ArgumentOutOfRangeException>(() => ReflectiveDoubleLoop.Propose([], minSupportingRecords: 1));

        var single = new[] { Rec(purpose: "tek", magnitude: 100) };
        Assert.Empty(ReflectiveDoubleLoop.Propose(single, minSupportingRecords: 2, magnitudeThreshold: 5.0));
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_G7_Propose_mutates_nothing_enumerates_once_and_is_idempotent()
    {
        // P7'nin DAVRANISSAL kaniti (yapisal kanit G8'de): girdi dizisi, icindeki kayitlar ve
        // cagrilar arasi sonuc degismiyor; kaynak tam bir kez dolasiliyor (gizli yeniden-okuma
        // ya da yan etki yok).
        var records = Enumerable.Range(0, 5).Select(_ => Rec(purpose: "fiyatlandirma", magnitude: 9, confidence: 0.6)).ToArray();
        var snapshot = records.ToArray();
        var values = records.Select(r => (r.PurposeType, r.LearningMagnitude, r.AttributionConfidence, r.AssertedAt)).ToArray();

        var source = new CountingSource(records);
        var first = ReflectiveDoubleLoop.Propose(source, minSupportingRecords: 3, magnitudeThreshold: 5.0);
        Assert.Equal(1, source.EnumeratorRequests);

        var second = ReflectiveDoubleLoop.Propose(records, minSupportingRecords: 3, magnitudeThreshold: 5.0);

        // record esitligi: ayni girdi -> ayni cikti, gizli/birikimli state yok
        Assert.Equal(first.Count, second.Count);
        Assert.Equal(first[0], second[0]);

        Assert.True(snapshot.SequenceEqual(records));
        Assert.True(values.SequenceEqual(
            records.Select(r => (r.PurposeType, r.LearningMagnitude, r.AttributionConfidence, r.AssertedAt))));
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_G8_ReflectiveDoubleLoop_has_no_mutation_surface_and_no_hidden_state()
    {
        // Mevcut ReflectiveDoubleLoopTests yalnizca PUBLIC STATIC metodlara bakiyor. Burada
        // private/instance uyeler ve ALANLAR da taraniyor (derleyici uretimi '<...>' adlar
        // haric): sinifta ne mutasyon metodu ne de saklanan durum var.
        const BindingFlags all = BindingFlags.Public | BindingFlags.NonPublic
            | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        var methods = typeof(ReflectiveDoubleLoop).GetMethods(all)
            .Where(m => !m.Name.StartsWith('<')).ToArray();
        var fields = typeof(ReflectiveDoubleLoop).GetFields(all)
            .Where(f => !f.Name.StartsWith('<')).ToArray();

        Assert.Single(methods);
        Assert.Equal(nameof(ReflectiveDoubleLoop.Propose), methods[0].Name);
        Assert.Empty(fields);
        Assert.True(typeof(ReflectiveDoubleLoop).IsAbstract && typeof(ReflectiveDoubleLoop).IsSealed); // static
    }

    // ========================================================================================
    // H. CONTEXT SCORE — kapinin confidence'i YUKSELTMESI mumkun mu?
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_MEM_H1_GateConfidence_never_raises_confidence_over_500_random_inputs()
    {
        // P6'nin can damari: kapi yalnizca KISITLAR. Fuzz'da negatif esik, negatif skor,
        // sifir esik ve tam sinir dahil hicbir kombinasyon ham confidence'i yukseltmiyor.
        var rng = new Random(4242);

        for (int i = 0; i < 500; i++)
        {
            double raw = rng.NextDouble();
            double score = (rng.NextDouble() - 0.5) * 2000;
            double threshold = rng.Next(0, 4) switch
            {
                0 => 0.0,
                1 => score,                       // tam sinir
                2 => -rng.NextDouble() * 1000,    // negatif esik
                _ => rng.NextDouble() * 1000,
            };

            double gated = ContextScore.GateConfidence(raw, score, threshold);

            Assert.True(gated <= raw, $"kapi confidence'i YUKSELTTI: raw={raw} score={score} esik={threshold} -> {gated}");
            Assert.True(gated is >= 0.0 and <= 1.0);
        }
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_H2_Threshold_zero_neither_divides_by_zero_nor_opens_the_gate()
    {
        // "threshold=0 ile bolme?" -> hayir: `threshold <= 0` dali bolmeyi hic yapmadan cap=0
        // veriyor. Skor esigin ALTINDA ise sonuc 0 (fail-closed); tam esikte ise raw doner.
        Assert.Equal(0.0, ContextScore.GateConfidence(1.0, contextScore: -double.Epsilon, threshold: 0.0));
        Assert.Equal(0.0, ContextScore.GateConfidence(1.0, contextScore: -1e300, threshold: 0.0));
        Assert.Equal(1.0, ContextScore.GateConfidence(1.0, contextScore: 0.0, threshold: 0.0));
        Assert.Equal(0.4, ContextScore.GateConfidence(0.4, contextScore: 1e-300, threshold: 0.0));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_H3_A_negative_threshold_silently_turns_the_gate_into_a_no_op()
    {
        // `threshold` yalnizca Guard.Finite'ten geciyor — isareti denetlenmiyor. Negatif esik,
        // ENS-2002 3'un anlam dunyasinda tanimsizdir (coverage - noise - staleness < 0 zaten
        // "gurultu kapsami boguyor" demektir). Ama kod bunu kabul eder ve derin negatif bir
        // ContextScore, TAM confidence ile kapidan gecer. Kapiyi kapatan degeri cagiran secer;
        // hicbir uyari, hicbir exception yok — Guard.cs'in "olculemeyen girdi otonomi
        // kazanamaz" politikasinin anlamsal karsiligi burada eksik (sayi olculebilir, politika
        // degil).
        Assert.Equal(0.99, ContextScore.GateConfidence(0.99, contextScore: -1000.0, threshold: -1e9));
        Assert.Equal(0.99, ContextScore.GateConfidence(0.99, contextScore: -1e8, threshold: -1e9));

        // Karsilastirma: ayni skor, anlamli (pozitif) esikle tamamen kapatiliyor.
        Assert.Equal(0.0, ContextScore.GateConfidence(0.99, contextScore: -1000.0, threshold: 10.0));
    }

    [Fact]
    public void AUDIT_DEFECT_MEM_H4_Compute_can_emit_negative_infinity_an_unmeasurable_score()
    {
        // Guard.cs politikasi: "olculemeyen deger kernel'in karar yollarina GIREMEZ." Compute
        // girdilerini denetliyor ama CIKTISINI denetlemiyor: iki buyuk sonlu ceza toplanip
        // -Infinity uretiyor. Sonuc fail-closed tarafta kaliyor (asagida: GateConfidence onu
        // reddediyor, yani gercek bir guvenlik acigi DEGIL) — ama uretici, olculemeyen bir
        // degeri sisteme salmis oluyor ve hata cagiranin cok ilerisinde patliyor.
        double score = ContextScore.Compute(coverage: 0.0, noisePenalty: double.MaxValue, staleness: double.MaxValue);

        Assert.True(double.IsNegativeInfinity(score));
        Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.GateConfidence(0.9, score, 1.0));
    }

    [Fact]
    public void AUDIT_HOLDS_MEM_H5_Both_entry_points_reject_every_unmeasurable_input()
    {
        foreach (double bad in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity })
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.Compute(bad, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.Compute(0, bad, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.Compute(0, 0, bad));
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.GateConfidence(0.5, bad, 1.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.GateConfidence(0.5, 1.0, bad));
            Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.GateConfidence(bad, 1.0, 1.0));
        }

        Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.Compute(-1, 0, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => ContextScore.GateConfidence(1.5, 1.0, 1.0));
    }

    /// <summary>`Propose`in kaynagi kac kez dolastigini olcen sarmalayici (yan etki avciligi).</summary>
    private sealed class CountingSource(IEnumerable<MemoryRecord> inner) : IEnumerable<MemoryRecord>
    {
        public int EnumeratorRequests { get; private set; }

        public IEnumerator<MemoryRecord> GetEnumerator()
        {
            EnumeratorRequests++;
            return inner.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
