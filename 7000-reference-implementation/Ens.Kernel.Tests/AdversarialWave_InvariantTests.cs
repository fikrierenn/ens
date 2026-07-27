using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Ens.Kernel;
using Ens.Kernel.Domain;
using Ens.Kernel.Domain.Events;
using Xunit;

namespace Ens.Kernel.Tests;

// ============================================================================================
// DUSMANCA DALGA 2 — ENS'in EN YUKSEK SESLE IDDIA ETTIGI UC INVARIANT'A SALDIRI
//
// Hedef iddialar:
//   (1) "Atom siniri muhurlu" — ENS-2001 Individuation: tek Owner, tek Purpose,
//       acik Alternatives, tek Commitment.
//   (2) "Izsiz turetim TEMSIL EDILEMEZ" — Anayasa Madde VI / ENS-4025 L8.
//   (3) "Hicbir katman lifecycle'i ATLAYAMAZ" — ADR-0001 5.4.
//
// Bu tur AUDIT.md'nin duzeltmelerine GUVENMEZ: kapatildigi soylenen kapilar yeniden zorlanir,
// ve daha onemlisi YENI kapilar aranir (replay <-> canli yol ayrismasi, kimlik baglanmamasi,
// Unicode format karakterleri, Render enjeksiyonu, canli koleksiyon gorunumu, es zamanlilik).
//
// ADLANDIRMA (skill sozlesmesi; W2 = dalga 2):
//   AUDIT_DEFECT_INV_W2_*  -> GECERSE KUSUR VAR. Mevcut (kusurlu) davranisi sabitler.
//   AUDIT_HOLDS_INV_W2_*   -> iddia saldiridan SAG CIKTI.
//   AUDIT_FINDING_INV_W2_* -> kod calisir ama beyan/yorum gercegi asiyor.
//
// Rapor: 7000-reference-implementation/AUDIT-WAVE2-INVARIANTS.md
// ============================================================================================

/// <summary>
/// Kernel'in TANIMADIGI bir DomainEvent alt-tipi. `DomainEvent` public abstract record oldugu
/// icin disaridan turetilebilir — replay yoluna enjekte edilebilir mi?
/// </summary>
public sealed record ForgedUnknownEvent(string Payload) : DomainEvent;

public sealed class AdversarialWave_InvariantTests
{
    private static readonly DateTimeOffset T0 = new(2026, 7, 26, 9, 0, 0, TimeSpan.Zero);

    /// <summary>Kod noktasindan string — kaynak dosyaya gorunmez karakter yazmamak icin.</summary>
    private static string Ch(int codePoint) => ((char)codePoint).ToString();

    private static readonly string Lf = Ch(10);

    private static ProofTrace SampleTrace() =>
        new("W2-RULE", "test-sonucu", new[] { new Premise("kanit", 0.8) });

    private static GateResult AllowingGate() => new(GateDecision.Autonomous, "ok", 0.0);

    private static ActuationLayer LayerAt(ActionState target)
    {
        var layer = new ActuationLayer(Identity.New());
        if (target == ActionState.Planned) return layer;
        layer.ApplyGate(AllowingGate(), T0);
        if (target == ActionState.Contextualized) return layer;
        layer.BeginActing(T0);
        if (target == ActionState.Acting) return layer;
        layer.Observe(T0);
        if (target == ActionState.Observed) return layer;
        layer.RecordTrace(SampleTrace(), T0);
        if (target == ActionState.Traced) return layer;
        layer.RecordLearning(T0);
        if (target == ActionState.Learned) return layer;
        layer.Remember(T0);
        return layer;
    }

    private static DecisionAggregate CommittedDecision(Identity actor)
    {
        var d = DecisionAggregate.Frame(actor, "tedarikci degisimi");
        d.IdentifyAlternatives(actor, new[] { "A", "B" }, new[] { new Premise("kanit", 1.0) });
        d.Commit(actor, "A", 0.8, "maliyet dusecek");
        return d;
    }

    // ========================================================================================
    // R. REPLAY YOLU — "Rehydrate duzeltmesi gercek mi?"
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_INV_W2_R1_malformed_event_streams_are_all_rejected_on_replay()
    {
        // Gorevin acikca istedigi sahte akislar. Hepsi REDDEDILMELI — ve ediliyor.
        // AUDIT 5.3'un kapanisi bu eksende GERCEK.
        var id = Identity.New();
        var actor = Identity.New();

        var framed = new DecisionFramed("amac") { Emitter = actor, Target = id };
        var alts = new AlternativesIdentified(new[] { "A", "B" }, new[] { new Premise("kanit", 1.0) })
        { Emitter = actor, Target = id };
        var commit = new DecisionCommitted("A", actor, 0.8, "beklenen") { Emitter = actor, Target = id };
        var enacted = new DecisionEnacted("yapildi") { Emitter = actor, Target = id };
        var observed = new OutcomeObserved("gerceklesen") { Emitter = actor, Target = id };

        // (a) TERS SIRA
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { commit, alts, framed }));

        // (b) EKSIK OLAY — Alternatives yok
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, commit }));

        // (c) EKSIK OLAY — Framing yok
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { alts, commit }));

        // (d) TEKRARLANAN COMMITMENT — Individuation: tek Commitment
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, alts, commit, commit }));

        // (e) TEKRARLANAN FRAMING — Individuation: tek Purpose
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, framed }));

        // (f) Commitment OLMADAN OutcomeObserved
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, alts, observed }));

        // (g) Enact OLMADAN OutcomeObserved
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, alts, commit, observed }));

        // (h) Commitment SONRASI Alternatives — atom muhurlendikten sonra deliberation yok
        Assert.Throws<InvalidOperationException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, alts, commit, alts }));

        // (i) null olay
        Assert.Throws<ArgumentException>(
            () => DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, null! }));

        // (j) Saglikli akis GECIYOR (guard cok genis degil — bos bir sinav degil)
        var ok = DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, alts, commit, enacted, observed });
        Assert.True(ok.IsCommitted);
        Assert.True(ok.HasOutcome);
        Assert.Equal(5, ok.History.Count);
        Assert.Empty(ok.UncommittedEvents);   // replay yeniden yayinlamiyor
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_R2_rehydrate_accepts_events_belonging_to_other_decisions()
    {
        // KUSUR: `Rehydrate(id, history)` hicbir olayin `Target`'ini `id` ile karsilastirmaz,
        // `Emitter`'a da bakmaz. UC FARKLI karara ait olaylar, DORDUNCU bir kimlik altinda tek
        // bir "karar"a birlestirilebiliyor ve Individuation muhru hic uyarmiyor.
        //
        // Canli yol her olayin Target'ini `decision.Id` yapar; replay yolu bunu DOGRULAMAZ.
        // Yani "replay yolu canli yolla AYNI invariant'lardan gecer" iddiasi bu eksende YANLIS:
        // aggregate sinirinin butunlugu aggregate'in degil, olay deposu sorgusunun sorumlulugunda.
        var idA = Identity.New();
        var idB = Identity.New();
        var idC = Identity.New();
        var alice = Identity.New();
        var mallory = Identity.New();

        var framedA = new DecisionFramed("A'nin amaci") { Emitter = alice, Target = idA };
        var altsB = new AlternativesIdentified(new[] { "x", "y" }, new[] { new Premise("kanit", 1.0) })
        { Emitter = mallory, Target = idB };
        var commitC = new DecisionCommitted("x", mallory, 0.99, "harika")
        { Emitter = mallory, Target = idC };

        var frankenstein = DecisionAggregate.Rehydrate(
            Identity.New(), new DomainEvent[] { framedA, altsB, commitC });

        Assert.True(frankenstein.IsCommitted);
        Assert.Equal("A'nin amaci", frankenstein.Purpose);
        Assert.Equal(0.99, frankenstein.Confidence!.Value, precision: 10);

        // Uc olay, UC FARKLI Target — ve aggregate bunlarin hicbirine ait degil:
        Assert.Equal(3, frankenstein.History.Select(e => e.Target).Distinct().Count());
        Assert.Equal(2, frankenstein.History.Select(e => e.Emitter).Distinct().Count());
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_R3_replay_does_not_enforce_the_non_blank_alternative_guard()
    {
        // KUSUR: Canli yol (`IdentifyAlternatives`) "Bos Alternative olamaz — adsiz secenek
        // degerlendirilemez." diye REDDEDER. Replay yolu (`EnsureReplayInvariant`) YALNIZCA
        // `Count == 0`'a bakar. Iki yol AYRISIYOR: canli olarak URETILEMEYEN bir olay dizisi
        // replay ile uretilebiliyor. Sonuc: karsi-olgusu OLMAYAN, bosluktan (hatta null'dan)
        // ibaret bir "secenek"e commit edilmis bir atom.
        var id = Identity.New();
        var actor = Identity.New();
        var framed = new DecisionFramed("amac") { Emitter = actor, Target = id };

        // (a) canli yol REDDEDIYOR
        var live = DecisionAggregate.Frame(actor, "amac");
        Assert.Throws<ArgumentException>(
            () => live.IdentifyAlternatives(actor, new[] { "   " }, new[] { new Premise("kanit", 1.0) }));

        // (b) replay yolu KABUL EDIYOR
        var blankAlts = new AlternativesIdentified(new[] { "   " }, new[] { new Premise("kanit", 1.0) })
        { Emitter = actor, Target = id };
        var blankCommit = new DecisionCommitted("   ", actor, 0.95, "beklenen")
        { Emitter = actor, Target = id };

        var d = DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, blankAlts, blankCommit });

        Assert.True(d.IsCommitted);
        Assert.Single(d.Alternatives);
        Assert.True(string.IsNullOrWhiteSpace(d.Alternatives[0]));

        // (c) null bir "secenek" bile geciyor
        var nullAlts = new AlternativesIdentified(new string[] { null! }, new[] { new Premise("kanit", 1.0) })
        { Emitter = actor, Target = id };
        var nullCommit = new DecisionCommitted(null!, actor, 0.95, "beklenen")
        { Emitter = actor, Target = id };

        var d2 = DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, nullAlts, nullCommit });
        Assert.True(d2.IsCommitted);
        Assert.Null(d2.Alternatives[0]);
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_R4_replayed_alternatives_are_a_live_view_not_a_copy()
    {
        // EN AGIR BULGU. AUDIT 5.3 duzeltme (2) "ALTERNATIVES ARTIK KOPYALANIYOR" diyor ve
        // DecisionAggregate.Apply su yorumu tasiyor:
        //     "aggregate her zaman KENDI degistirilemez kopyasini tutar."
        // Bu YANLIS. Kod:
        //     Alternatives = e.Alternatives as ReadOnlyCollection<string> ?? new ReadOnlyCollection<...>
        // `ReadOnlyCollection<T>` bir KOPYA DEGIL, bir GORUNUMDUR. Cagiran kendi `List<string>`'ini
        // bir ReadOnlyCollection'a sarip olaya koyarsa `as` dali TUTAR ve aggregate CAGIRANIN
        // canli listesini saklar. Rehydrate'ten SONRA listeye eklenen bir secenek, aggregate'in
        // deliberation kumesine sizar — ve canli `Commit` onu mesru sayar.
        //
        // Yani AUDIT 5.3'te kapatilan "hic degerlendirilmemis alternatife commit" saldirisi,
        // replay kapisindan AYNEN geri geliyor.
        var id = Identity.New();
        var owner = Identity.New();

        var backing = new List<string> { "A", "B" };
        var liveView = new ReadOnlyCollection<string>(backing);

        var framed = new DecisionFramed("amac") { Emitter = owner, Target = id };
        var alts = new AlternativesIdentified(liveView, new[] { new Premise("kanit", 1.0) })
        { Emitter = owner, Target = id };

        var d = DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, alts });
        Assert.Equal(2, d.Alternatives.Count);

        // Aggregate DISARIDAN mutasyona ugruyor:
        backing.Add("HIC-DEGERLENDIRILMEMIS");

        Assert.Equal(3, d.Alternatives.Count);
        Assert.Contains("HIC-DEGERLENDIRILMEMIS", d.Alternatives);

        // Ve canli commit yolu bu secenegi MESRU sayiyor — "acik Alternatives" muhru delindi:
        d.Commit(owner, "HIC-DEGERLENDIRILMEMIS", 0.99, "harika");
        Assert.True(d.IsCommitted);
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_R5_unknown_event_types_are_silently_swallowed_by_the_fold()
    {
        // KUSUR: `DomainEvent` public abstract record — disaridan turetilebilir. `Apply` ve
        // `EnsureReplayInvariant` switch'lerinin `default` dali YOK: taninmayan bir olay
        // sessizce kabul edilir, history'ye yazilir ve duruma HIC etki etmez.
        //
        // Neden ciddi: "durum yalnizca Event akisinin fold'udur" (Axiom 2) iddiasi, fold'un
        // TOTAL olmasini gerektirir. Sema evrimi altinda eski bir okuyucu ayni akistan FARKLI
        // bir durum uretir ve bunu HIC bildirmez — sessiz ayrisma, fail-open.
        var id = Identity.New();
        var actor = Identity.New();
        var framed = new DecisionFramed("amac") { Emitter = actor, Target = id };
        var forged = new ForgedUnknownEvent("v2-semasindan-gelen-iptal-olayi")
        { Emitter = actor, Target = id };

        var d = DecisionAggregate.Rehydrate(id, new DomainEvent[] { framed, forged });

        Assert.Equal(2, d.History.Count);               // sessizce kabul edildi
        Assert.IsType<ForgedUnknownEvent>(d.History[1]);
        Assert.Equal("amac", d.Purpose);                // duruma hic etki etmedi
        Assert.False(d.IsCommitted);
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_R6_replay_ignores_timestamps_and_duplicate_event_ids()
    {
        // KUSUR (a): Lifecycle sirasi YALNIZCA liste konumuyla zorlaniyor; `Timestamp` hic
        // okunmuyor. Framing'den 10 yil ONCE commit edilmis bir karar replay'den geciyor.
        // Nedensel olarak imkansiz bir tarih, "olay gecmisidir" iddiasinin icine yazilabiliyor.
        //
        // KUSUR (b): `EventId` tekilligi dogrulanmiyor. Ayni olay NESNESI birden cok kez
        // uygulanabiliyor — at-least-once teslimat yapan bir event-store, kaydi sessizce cogaltir.
        var id = Identity.New();
        var actor = Identity.New();

        var framed = new DecisionFramed("amac") { Emitter = actor, Target = id, Timestamp = T0 };
        var alts = new AlternativesIdentified(new[] { "A" }, new[] { new Premise("kanit", 1.0) })
        { Emitter = actor, Target = id, Timestamp = T0.AddDays(-100) };
        var commit = new DecisionCommitted("A", actor, 0.8, "beklenen")
        { Emitter = actor, Target = id, Timestamp = T0.AddYears(-10) };
        var enacted = new DecisionEnacted("yapildi")
        { Emitter = actor, Target = id, Timestamp = T0.AddYears(-20) };
        var observed = new OutcomeObserved("gerceklesen")
        { Emitter = actor, Target = id, Timestamp = T0 };

        var d = DecisionAggregate.Rehydrate(
            id, new DomainEvent[] { framed, alts, commit, enacted, observed, observed, observed });

        Assert.True(d.HasOutcome);

        // (a) zaman geriye akiyor, kimse itiraz etmiyor
        Assert.True(d.History[2].Timestamp < d.History[0].Timestamp);
        Assert.True(d.History[3].Timestamp < d.History[2].Timestamp);

        // (b) ayni EventId uc kez audit izinde
        Assert.Equal(3, d.History.Count(e => e.EventId == observed.EventId));
    }

    // ========================================================================================
    // O. "TEK OWNER" — Individuation'in dorduncu kosulu
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_INV_W2_O1_the_single_owner_condition_is_not_implemented_at_all()
    {
        // KUSUR: ENS-2001 Individuation'in DORT kosulundan ucu (tek Purpose, acik Alternatives,
        // tek Commitment) gercekten zorlaniyor. DORDUNCUSU — "tek Owner" — hicbir yerde
        // zorlanmiyor, hatta TEMSIL BILE EDILMIYOR:
        //   - `DecisionAggregate`'in Owner diye bir property'si yok,
        //   - her metot AYRI bir `Identity` kabul ediyor, hicbiri digeriyle kiyaslanmiyor,
        //   - replay yolu `Emitter == Owner` bagini da dogrulamiyor.
        // Yani "Atom siniri MUHURLENDI: tek Owner, ..." cumlesinin ILK maddesi kodda karsiliksiz.
        var alice = Identity.New();
        var bob = Identity.New();
        var carol = Identity.New();
        var dave = Identity.New();
        var erin = Identity.New();

        var d = DecisionAggregate.Frame(alice, "amac");
        d.IdentifyAlternatives(bob, new[] { "A", "B" }, new[] { new Premise("kanit", 1.0) });
        d.Commit(carol, "A", 0.8, "beklenen");
        d.Enact(dave, "yapildi");
        d.ObserveOutcome(erin, "gerceklesen");

        Assert.True(d.IsCommitted);   // bes farkli aktor, tek "atom", sifir itiraz
        Assert.Equal(5, d.History.Select(e => e.Emitter).Distinct().Count());

        // Aggregate'te Owner diye bir yuzey yok — invariant'in tasiyicisi bile mevcut degil.
        Assert.DoesNotContain(
            typeof(DecisionAggregate).GetProperties(BindingFlags.Public | BindingFlags.Instance),
            p => p.Name.Contains("Owner", StringComparison.OrdinalIgnoreCase));

        // Ve replay yolunda Emitter ile Owner birbirinden BAGIMSIZ uydurulabiliyor:
        var id = Identity.New();
        var forged = new DecisionCommitted("A", alice, 0.9, "beklenen")
        { Emitter = bob, Target = id };    // Owner = Alice, ama olayi yayan Bob
        var replayed = DecisionAggregate.Rehydrate(id, new DomainEvent[]
        {
            new DecisionFramed("amac") { Emitter = bob, Target = id },
            new AlternativesIdentified(new[] { "A" }, new[] { new Premise("kanit", 1.0) }) { Emitter = bob, Target = id },
            forged
        });
        Assert.True(replayed.IsCommitted);
    }

    // ========================================================================================
    // P. PROOF-TRACE — "Izsiz turetim TEMSIL EDILEMEZ" (Anayasa Madde VI)
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_INV_W2_P1_a_completely_invisible_proof_trace_is_representable()
    {
        // EN AGIR PROOF-TRACE BULGUSU. `string.IsNullOrWhiteSpace` YALNIZCA Unicode whitespace'i
        // (Zs/Zl/Zp + birkac kontrol karakteri) yakalar. FORMAT (Cf) ve NUL karakterleri
        // whitespace DEGILDIR — dolayisiyla guard'dan gecerler.
        //
        // Sonuc: RuleId'si, Conclusion'i ve tek onculunun Source'u TAMAMEN GORUNMEZ olan,
        // Confidence'i 1.00 cikan bir proof-trace kurulabiliyor; Actuation Layer onu "kanit"
        // olarak kabul edip Traced'e geciyor. "Izsiz turetim temsil edilemez" iddiasinin en
        // guclu karsi-ornegi: iz VAR, ama ici bos ve gozle GORULMUYOR.
        var zwsp = Ch(0x200B);   // ZERO WIDTH SPACE          (Cf)
        var bom = Ch(0xFEFF);    // ZERO WIDTH NO-BREAK SPACE (Cf)
        var lrm = Ch(0x200E);    // LEFT-TO-RIGHT MARK        (Cf)
        var rlo = Ch(0x202E);    // RIGHT-TO-LEFT OVERRIDE    (Cf)
        var zwj = Ch(0x200D);    // ZERO WIDTH JOINER         (Cf)
        var nul = Ch(0x0000);    // NUL                       (Cc)

        foreach (var c in new[] { zwsp, bom, lrm, rlo, zwj, nul })
        {
            // Kok neden: bunlarin HICBIRI .NET'e gore whitespace degil.
            Assert.False(char.IsWhiteSpace(c[0]), $"U+{(int)c[0]:X4} whitespace sayiliyor");
            Assert.False(string.IsNullOrWhiteSpace(c));

            var p = new Premise(c, 1.0);           // KABUL EDILDI
            Assert.Equal(1.0, p.Confidence, precision: 10);
        }

        var ghost = new ProofTrace(ruleId: zwsp, conclusion: bom, premises: new[] { new Premise(nul, 1.0) });
        Assert.Equal(1.0, ghost.Confidence, precision: 10);

        // Render edilmis "aciklama"da sabit sablon disinda tek bir gorunur harf yok:
        var rendered = ghost.Render();
        Assert.True(rendered.Where(char.IsLetter).All(ch => "confmin".Contains(ch)),
            "Render'da yalnizca sablon harfleri kalmali — oncul/kural/sonuc metni gorunmez.");

        // Ve Actuation Layer bu hayaleti P6 kaniti olarak kabul ediyor:
        var layer = LayerAt(ActionState.Observed);
        layer.RecordTrace(ghost, T0);
        Assert.Equal(ActionState.Traced, layer.State);
    }

    [Fact]
    public void AUDIT_HOLDS_INV_W2_P2_real_unicode_whitespace_variants_are_all_rejected()
    {
        // Durustluk geregi: guard'in YAKALADIKLARI. Zs/Zl kategorileri gercekten reddediliyor,
        // yani kusur "guard yok" degil, "guard'in kapsami Unicode kategorisiyle sinirli".
        var rejected = new[]
        {
            "",
            " ",
            "   ",
            Ch(0x0009),   // TAB
            Ch(0x000A),   // LF
            Ch(0x000D),   // CR
            Ch(0x00A0),   // NO-BREAK SPACE
            Ch(0x2000),   // EN QUAD
            Ch(0x2009),   // THIN SPACE
            Ch(0x202F),   // NARROW NO-BREAK SPACE
            Ch(0x205F),   // MEDIUM MATHEMATICAL SPACE
            Ch(0x3000),   // IDEOGRAPHIC SPACE
            Ch(0x2028)    // LINE SEPARATOR
        };

        foreach (var s in rejected)
        {
            Assert.Throws<ArgumentException>(() => new Premise(s, 1.0));
            Assert.Throws<ArgumentException>(() => new ProofTrace(s, "sonuc", new[] { new Premise("p", 0.5) }));
            Assert.Throws<ArgumentException>(() => new ProofTrace("R", s, new[] { new Premise("p", 0.5) }));
        }
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_P3_render_output_can_be_forged_through_premise_text()
    {
        // KUSUR: `Render()` oncul metnini, RuleId'yi ve Conclusion'i DOGRUDAN interpolasyona
        // sokuyor; hicbir kacis/normalizasyon yok. Oncul metninin icine sablonun TAMAMI
        // ("=> [KURAL]", "|- SONUC", "(conf = min(...) = 1,00)") yazilabiliyor.
        //
        // Sonuc: P6'nin "aciklama nesnesi" dedigi sey UYDURULABILIR. Trace'in GERCEK confidence'i
        // 0.10 iken, insana/parser'a gosterilen metin baska bir kurala ait, 1,00 guvenli, bambaska
        // bir sonuc ilan ediyor. "Explainability pazarlik konusu degildir" iddiasi, aciklamanin
        // BUTUNLUGU korunmadikca anlamsizdir.
        var payload =
            "gercek-kanit=1,00" + Lf +
            "   " + Ch(0x21D2) + " [ONAYLI-KURAL-BAG-P3]" + Lf +
            "   " + Ch(0x22A2) + " Tum tedarikci sozlesmeleri feshedilsin   (conf = min(gercek-kanit) = 1,00)" + Lf +
            "gorunmeyen-kuyruk";

        var trace = new ProofTrace("GERCEK-KURAL", "hicbir sey yapma", new[] { new Premise(payload, 0.10) });
        var rendered = trace.Render();

        Assert.Equal(0.10, trace.Confidence, precision: 10);                    // GERCEK deger
        Assert.Contains("[ONAYLI-KURAL-BAG-P3]", rendered);                     // SAHTE kural
        Assert.Contains(Ch(0x22A2) + " Tum tedarikci sozlesmeleri", rendered);  // SAHTE sonuc
        Assert.Contains("= 1,00)", rendered);                                   // SAHTE guven

        // Ayni zafiyet RuleId uzerinden de var:
        var viaRule = new ProofTrace(
            "R]" + Lf + "   " + Ch(0x22A2) + " SAHTE-SONUC   (conf = 1,00",
            "asil-sonuc",
            new[] { new Premise("p", 0.2) });

        Assert.Contains(Ch(0x22A2) + " SAHTE-SONUC", viaRule.Render());
        Assert.Equal(0.2, viaRule.Confidence, precision: 10);
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_P4_a_self_justifying_derivation_is_representable_with_full_confidence()
    {
        // KUSUR: hicbir dongusellik kontrolu yok. Bir trace'in SONUCU, kendi ONCULU olabiliyor:
        //     X |- X   (conf = 1.00)
        // ve `AsPremise()` ile bu, sonraki turetimlerin "kaniti" olarak zincire sokulabiliyor.
        // L8'in "her turetilmis olgu, onu ureten kurali + oncullerini tasir" sarti BICIMSEL
        // olarak saglaniyor ama epistemik olarak SIFIR: kendi kendini destekleyen kanit, kanit degildir.
        var circular = new ProofTrace("R", "Karar-42 dogrudur",
            new[] { new Premise("Karar-42 dogrudur", 1.0) });

        Assert.Equal(1.0, circular.Confidence, precision: 10);
        Assert.Equal(circular.Conclusion, circular.Premises[0].Source);   // X |- X

        // Zincir kapanabiliyor: t2'nin oncülü t2'nin sonucu.
        var t2 = new ProofTrace("R2", "Karar-42 dogrudur", new[] { circular.AsPremise() });
        Assert.Equal(1.0, t2.Confidence, precision: 10);
        Assert.Equal(t2.Conclusion, t2.Premises[0].Source);

        // Actuation Layer bunu kanit sayiyor:
        var layer = LayerAt(ActionState.Observed);
        layer.RecordTrace(t2, T0);
        Assert.Equal(ActionState.Traced, layer.State);
    }

    [Fact]
    public void AUDIT_FINDING_INV_W2_P5_min_tnorm_is_idempotent_so_chain_length_never_lowers_confidence()
    {
        // KOD DOGRU (L7 = min, sadakatle uygulanmis) ama KAYNAK YORUMU FAZLA SOYLUYOR.
        // ProofTrace.AsPremise yorumu: "Confidence monoton azalir (min t-norm) — turetim zinciri
        // UZADIKCA GUVEN DUSER, artamaz."
        // `min` IDEMPOTENTTIR: zincir 1 adim da olsa 100 adim da olsa confidence AYNI kalir.
        // Dogru ifade "artmaz"dir, "duser" degil. 100 halkali bir cikarim zinciri, tek halkaliyla
        // ayni guveni tasiyor — ENS-4025 Failure'daki t-norm borcu tam olarak burada gorunur.
        var t = new ProofTrace("R0", "F0", new[] { new Premise("kaynak", 0.9) });
        for (int i = 1; i <= 100; i++)
            t = new ProofTrace($"R{i}", $"F{i}", new[] { t.AsPremise() });

        Assert.Equal(0.9, t.Confidence, precision: 12);   // 100 adim sonra bile 0.9

        // Ayni idempotanslik "1000 oncul" gosterisini de anlamsiz kiliyor: tekrar korroborasyon
        // uretmiyor, ama Render 1000 ayri kanit varmis gibi gosteriyor.
        var repeated = Enumerable.Repeat(new Premise("tek-kanit", 0.42), 1000).ToList();
        var bulky = new ProofTrace("R", "C", repeated);
        Assert.Equal(0.42, bulky.Confidence, precision: 12);
        Assert.Equal(1000, bulky.Premises.Count);
        Assert.Single(bulky.Premises.Distinct());        // hepsi AYNI oncul
    }

    [Fact]
    public void AUDIT_HOLDS_INV_W2_P6_premises_cannot_be_replaced_after_construction()
    {
        // Saldirdim, kirilmadi: hem downcast hem girdi-aliasing kapali, `Confidence` atanamaz.
        var source = new List<Premise> { new("a", 0.9), new("b", 0.4) };
        var trace = new ProofTrace("R", "C", source);

        source.Clear();
        source.Add(new Premise("sonradan", 0.01));

        Assert.Equal(2, trace.Premises.Count);
        Assert.Equal(0.4, trace.Confidence, precision: 12);
        Assert.Throws<InvalidCastException>(() => ((List<Premise>)trace.Premises).Clear());
        Assert.Throws<NotSupportedException>(() => ((IList<Premise>)trace.Premises).Clear());

        Assert.False(typeof(ProofTrace).GetProperty(nameof(ProofTrace.Confidence))!.CanWrite);
    }

    // ========================================================================================
    // L. ACTUATION LAYER — "Hicbir katman lifecycle'i ATLAYAMAZ" (ADR-0001 5.4)
    // ========================================================================================

    [Fact]
    public void AUDIT_DEFECT_INV_W2_L1_the_action_lifecycle_can_be_entered_without_any_decision()
    {
        // KUSUR: `Planned` durumunun tanimi "Decision commit edildi (ENS-2001), action planlandi".
        // Ama `new ActuationLayer(id)` HICBIR SEY dogrulamaz: verilen Identity'nin bir Decision'a
        // karsilik gelip gelmedigi, o Decision'in commit edilip edilmedigi sorulmuyor.
        //
        // Yani "hicbir katman lifecycle'i atlayamaz" iddiasi lifecycle'in YALNIZCA IKINCI YARISI
        // icin dogrudur. Birinci yari (Framing -> Alternatives -> Commitment) tumuyle ATLANABILIR:
        // hic commit edilmemis — hatta hic var olmamis — bir karar icin Planned'dan Remembered'a
        // tam bir "guarded action" kosturulabiliyor.
        var actor = Identity.New();
        var neverCommitted = DecisionAggregate.Frame(actor, "sadece cercevelendi");

        var layer = new ActuationLayer(neverCommitted.Id);
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0);
        layer.Observe(T0);
        layer.RecordTrace(SampleTrace(), T0);
        layer.RecordLearning(T0);
        layer.Remember(T0);

        Assert.False(neverCommitted.IsCommitted);            // karar hic muhurlenmedi
        Assert.Equal(ActionState.Remembered, layer.State);   // ama action tamamlandi
        Assert.Equal(7, layer.History.Count);

        // Daha da kotusu: hic var olmayan, BOS bir kimlik icin de ayni sey gecerli.
        var ghostLayer = new ActuationLayer(default);
        Assert.Null(ghostLayer.DecisionId.Value);
        ghostLayer.ApplyGate(AllowingGate(), T0);
        ghostLayer.BeginActing(T0);
        Assert.Equal(ActionState.Acting, ghostLayer.State);
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_L2_one_proof_trace_can_legitimize_unlimited_unrelated_actions()
    {
        // KUSUR: `ProofTrace`'in KANITLADIGI SEYE BAGI YOK — ne DecisionId tasiyor ne action
        // kimligi; `RecordTrace` de trace ile `DecisionId` arasinda hicbir iliski aramiyor.
        // Tek bir genel trace ("her sey yolunda"), birbiriyle ilgisiz butun action'larin P6
        // kaniti olarak yeniden kullanilabiliyor. AUDIT'in D5 bulgusundan (invariant yalnizca
        // KARDINALITE kontrol ediyor) daha derin: kanit, kanitladigi seye BAGLI DEGIL.
        var shared = new ProofTrace("R", "her sey yolunda", new[] { new Premise("kanit-yok", 1.0) });

        var layers = Enumerable.Range(0, 5).Select(_ => LayerAt(ActionState.Observed)).ToList();
        foreach (var l in layers)
            l.RecordTrace(shared, T0);

        Assert.All(layers, l => Assert.Same(shared, l.Trace));
        Assert.Equal(5, layers.Select(l => l.DecisionId).Distinct().Count());   // 5 farkli karar
        Assert.Single(layers.Select(l => l.Trace).Distinct());                  // 1 tek "kanit"

        // Ve trace'te hicbir kimlik alani yok:
        Assert.DoesNotContain(
            typeof(ProofTrace).GetProperties(BindingFlags.Public | BindingFlags.Instance),
            p => p.PropertyType == typeof(Identity));
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_L3_audit_timestamps_are_caller_controlled_and_never_validated()
    {
        // KUSUR: `Transition(to, at)` `at`'i hic dogrulamiyor. Tum lifecycle GERIYE akan zaman
        // damgalariyla kosturulabiliyor; audit izi "Remembered @ 1926" ile "Planned @ 2026"
        // gosteriyor ve katman itiraz etmiyor. Reflection GEREKMIYOR.
        // "Her gecis bir AuditEvent uretir" dogru; "AuditEvent guvenilir bir zaman tasir" DEGIL.
        var layer = new ActuationLayer(Identity.New());
        layer.ApplyGate(AllowingGate(), T0);
        layer.BeginActing(T0.AddDays(-1));
        layer.Observe(T0.AddYears(-10));
        layer.RecordTrace(SampleTrace(), DateTimeOffset.MinValue);
        layer.RecordLearning(DateTimeOffset.MaxValue);
        layer.Remember(T0.AddYears(-100));

        Assert.Equal(ActionState.Remembered, layer.State);
        Assert.True(layer.History[^1].At < layer.History[0].At);
        Assert.Equal(DateTimeOffset.MinValue, layer.History.First(h => h.To == ActionState.Traced).At);
    }

    [Fact]
    public void AUDIT_DEFECT_INV_W2_L4_history_is_a_live_unsynchronized_view_not_a_snapshot()
    {
        // KUSUR: AUDIT 5.2 duzeltmesi `History`'yi SILINEMEZ yapti ama CANLI birakti.
        // `ReadOnlyCollection<T>` bir kopya degil, alttaki `List<T>` uzerine bir gorunumdur ve
        // hicbir es zamanlilik guvencesi vermez. Denetci gecmisi OKURKEN bir gecis olursa okuma
        // `InvalidOperationException` ile COKER — tek is parcaciginda bile kanitlanabilir.
        // Cok is parcacikli kullanimda ayni desen `List<T>` bozulmasina aciktir; asagida sinifin
        // hicbir senkronizasyon primitifi tasimadiginin YAPISAL kaniti var.
        var layer = LayerAt(ActionState.Contextualized);
        Assert.Equal(2, layer.History.Count);

        Assert.Throws<InvalidOperationException>(() =>
        {
            foreach (var _ in layer.History)
                layer.BeginActing(T0);      // ilk yineleme basarili -> koleksiyon degisti
        });

        // Ayni zafiyet DecisionAggregate.History'de de var:
        var actor = Identity.New();
        var d = CommittedDecision(actor);
        Assert.Throws<InvalidOperationException>(() =>
        {
            foreach (var _ in d.History)
                d.Enact(actor, "yapildi");
        });

        // Yapisal kanit: ne kilit, ne concurrent koleksiyon, ne volatile durum alani.
        var instanceFields = typeof(ActuationLayer)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.Single(instanceFields.Where(f => f.FieldType == typeof(List<ActionTransition>)));

        var stateField = instanceFields.Single(f => f.FieldType == typeof(ActionState));
        Assert.DoesNotContain(typeof(IsVolatile), stateField.GetRequiredCustomModifiers());

        Assert.DoesNotContain(instanceFields, f =>
            f.FieldType == typeof(SemaphoreSlim) || f.FieldType == typeof(ReaderWriterLockSlim));

        // Gecis tablosu ise PROSES GENELINDE PAYLASILAN, DEGISTIRILEBILIR bir static:
        var table = typeof(ActuationLayer).GetField("Allowed", BindingFlags.NonPublic | BindingFlags.Static)!;
        Assert.Equal(typeof(Dictionary<ActionState, ActionState[]>), table.FieldType);
        // (Bilerek MUTASYONA UGRATMIYORUM: tek bir yazma, ayni prosesteki TUM ActuationLayer
        //  orneklerinin state machine'ini kalici olarak bozardi. Riskin kendisi bulgudur.)
    }

    [Fact]
    public void AUDIT_HOLDS_INV_W2_L5_out_of_range_gate_decision_fails_closed()
    {
        // Saldiri: enum'a tanimsiz bir deger enjekte et. Sinif `is Autonomous or NotifyHuman`
        // seklinde POZITIF liste kullaniyor -> bilinmeyen karar Blocked'a dusuyor. Fail-CLOSED.
        // Dogru desen; kayda gecsin.
        foreach (var bogus in new[] { (GateDecision)999, (GateDecision)(-1), GateDecision.CriticalBlock })
        {
            var layer = new ActuationLayer(Identity.New());
            layer.ApplyGate(new GateResult(bogus, "tanimsiz", 0.0), T0);
            Assert.Equal(ActionState.Blocked, layer.State);
            Assert.True(layer.IsTerminal);
            Assert.Throws<InvalidTransitionException>(() => layer.BeginActing(T0));
        }
    }

    [Fact]
    public void AUDIT_HOLDS_INV_W2_L6_ApplyGate_called_twice_is_rejected_and_leaves_no_partial_state()
    {
        // "ApplyGate iki kez cagrilirsa?" — ikinci cagri Contextualized'dan GateChecked'e gecmeye
        // calisir ve reddedilir. Yarim uygulanmis durum kalmiyor: ApplyGate'in iki gecisinden
        // ILKI reddedildigi icin ikincisine hic sira gelmiyor.
        var layer = new ActuationLayer(Identity.New());
        layer.ApplyGate(AllowingGate(), T0);
        Assert.Equal(ActionState.Contextualized, layer.State);
        Assert.Equal(2, layer.History.Count);

        var ex = Assert.Throws<InvalidTransitionException>(() => layer.ApplyGate(AllowingGate(), T0));
        Assert.Equal(ActionState.Contextualized, ex.From);
        Assert.Equal(ActionState.GateChecked, ex.To);

        Assert.Equal(ActionState.Contextualized, layer.State);
        Assert.Equal(2, layer.History.Count);   // audit izi kirletilmedi

        // Blocked terminalinden de ikinci bir gate uygulanamiyor:
        var blocked = new ActuationLayer(Identity.New());
        blocked.ApplyGate(new GateResult(GateDecision.CriticalBlock, "blok", 0), T0);
        Assert.Throws<InvalidTransitionException>(
            () => blocked.ApplyGate(new GateResult(GateDecision.Autonomous, "ikinci sans", 0), T0));
    }

    // ========================================================================================
    // C. ES ZAMANLILIK — hic test edilmemis yuzey
    // ========================================================================================

    [Fact]
    public void AUDIT_HOLDS_INV_W2_C1_independent_layers_run_correctly_in_parallel()
    {
        // Paylasilan durum YOKKEN paralellik sorun cikarmiyor: her action kendi katmaninda tam
        // lifecycle'i tamamliyor (statik `Allowed` tablosu salt-okunur kullanildigi surece guvenli).
        // Bu, asagidaki bulgunun kapsamini daraltmak icin: sorun paralellik degil, PAYLASILAN katman.
        var layers = Enumerable.Range(0, 64).Select(_ => new ActuationLayer(Identity.New())).ToArray();

        Parallel.ForEach(layers, l =>
        {
            l.ApplyGate(AllowingGate(), T0);
            l.BeginActing(T0);
            l.Observe(T0);
            l.RecordTrace(SampleTrace(), T0);
            l.RecordLearning(T0);
            l.Remember(T0);
        });

        Assert.All(layers, l =>
        {
            Assert.Equal(ActionState.Remembered, l.State);
            Assert.Equal(7, l.History.Count);
        });
    }

    [Fact]
    public void AUDIT_HOLDS_INV_W2_C2_shared_layer_under_parallel_load_still_rejects_illegal_moves()
    {
        // DURUST SINIR: bu test bir YARIS DURUMUNU KANITLAMAZ. Yarislar zamanlamaya baglidir ve
        // deterministik bir test yazilamaz; "kusuru gosteremedigi icin yesil yanan" bir
        // AUDIT_DEFECT yazmak da manipulasyon olurdu (skill: flaky kanit = kanit degil).
        //
        // Kanitladigi sey daha dar ama gercek: 64 is parcacigi x 6 hamle = 384 deneme, toplam 7
        // mesru gecise karsi kosturuldugunda state machine ezici cogunlugu REDDEDIYOR ve durum
        // alani tanimli bir enum degerinde kaliyor (enum okuma/yazma .NET'te atomik).
        //
        // KANITLANAMAYAN (ve L4'te yapisal olarak gosterilen) risk: `_history.Add` senkronize
        // degil. Bu test sayaclari TOPLAR ama uzerlerine iddia KURMAZ.
        var layer = new ActuationLayer(Identity.New());
        int rejected = 0, other = 0, succeeded = 0;

        Parallel.For(0, 64, _ =>
        {
            void Try(Action move)
            {
                try { move(); Interlocked.Increment(ref succeeded); }
                catch (InvalidTransitionException) { Interlocked.Increment(ref rejected); }
                catch { Interlocked.Increment(ref other); }
            }

            Try(() => layer.ApplyGate(AllowingGate(), T0));
            Try(() => layer.BeginActing(T0));
            Try(() => layer.Observe(T0));
            Try(() => layer.RecordTrace(SampleTrace(), T0));
            Try(() => layer.RecordLearning(T0));
            Try(() => layer.Remember(T0));
        });

        Assert.Equal(384, rejected + other + succeeded);
        Assert.True(rejected > 0, "State machine hicbir illegal hamleyi reddetmedi.");
        Assert.True(Enum.IsDefined(layer.State), $"Durum alani bozuldu: {(int)layer.State}");
    }

    [Fact]
    public void AUDIT_HOLDS_INV_W2_C3_parallel_commit_attempts_do_not_produce_a_visibly_broken_atom()
    {
        // Ayni durust sinir: `DecisionAggregate` de senkronize degil (`IsCommitted` check-then-act
        // + `_history.Add`). Deterministik kanit yazilamaz; burada yalnizca gozlemlenebilir sonuc
        // sabitleniyor. Kac `DecisionCommitted` olayinin history'ye yazildigi (ideal: 1) rapora
        // birakildi — bu test onun uzerine iddia kurmaz.
        var actor = Identity.New();
        var d = DecisionAggregate.Frame(actor, "amac");
        d.IdentifyAlternatives(actor, new[] { "A", "B" }, new[] { new Premise("kanit", 1.0) });

        int committed = 0;
        Parallel.For(0, 32, _ =>
        {
            try { d.Commit(actor, "A", 0.5, "beklenen"); Interlocked.Increment(ref committed); }
            catch (InvalidOperationException) { }
            catch { }
        });

        Assert.True(committed >= 1);
        Assert.True(d.IsCommitted);
    }
}
