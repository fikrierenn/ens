using System.Collections.ObjectModel;
using Ens.Kernel.Laws;

namespace Ens.Kernel.Domain;

// TRACE: ENS-2003 (Company Memory) §1 (Memory Graph düğümleri = commit-edilmiş kararlar)
// TRACE: ENS-2003 v0.4.0 §3 — ÜÇ NİCELİK, ÜÇ AYRI SÜRÜCÜ:
//   RetentionPriority(m) = |Learning|                 (kayıp-koruma ekseni; c'den BAĞIMSIZ)
//   value(m)             = |Learning| · c             (= ENS-3023 §Model 1 value(d); epistemik ağırlık)
//   decayFactor(m,t)     = exp(−λ_π · Δt)             (tazelik ekseni; c'yi İÇERMEZ)
//   Salience(m,t)        = value(m) × decayFactor(m,t) (retrieval sıralaması)
// TRACE: ENS-2003 v0.4.0 §3 — karşı-survivorship TABANI (kesme invariant'ı): bkz. RetrieveTop.
// TRACE: ENS-2003 v0.4.0 §3a/§3b — asserted_at/last_verified ayrımı; İKİ inceleme sinyali
//   (stale = tazelik ekseni, weakly-attributed = epistemik eksen); asla otomatik silme/mutasyon.
// TRACE: AUDIT.md §5.2 (kayıt downcast'le siliniyordu) ve §5.4 (decay yasası denetimsiz devre
//   dışı bırakılabiliyordu — dört ayrı bypass).
//
// ════════════════════════════════════════════════════════════════════════════════════════════
// v0.4.0 BREAKING — AUDIT-WAVE2-FIDELITY / D-5'e yanıt (attribution confidence ÇİFT SAYILIYORDU)
// ════════════════════════════════════════════════════════════════════════════════════════════
// ESKİ (v0.3.1, HATALI):
//   RetentionPriority = |L| · c            ve      λ(c) = λ_base · (1−c)^γ
//   → `c` HER İKİ "dik" eksende de vardı. Yüksek-c kayıt iki kez ödüllendiriliyor, düşük-c kayıt
//     iki kez cezalandırılıyordu. §3'ün karşı-survivorship amacı (başarısız-ama-ölçülmüş kararı
//     korumak) tam da bu yüzden ters çalışıyordu: başarısızlığın atfı tipik olarak ZAYIFTIR
//     (ENS-2004 §3: çoğu karar L1'e sıkışır), yani korunması en çok istenen kayıt hem geri plana
//     itiliyor hem hızla sönüyordu. Kod teoriye SADIKTI; hata teorideydi (SKR-040 ve SKR-041
//     kaçırdı, bağımsız TRACE denetimi yakaladı).
// YENİ (v0.4.0):
//   RetentionPriority = |L|                (c YOK)
//   λ_π               = ln2 / τ_π          (Purpose-tipinin context yarı-ömrü; c YOK)
//   value             = |L| · c            (c YALNIZCA burada — ENS-3023 §Model 1)
//   + kesme invariant'ı (RetrieveTop) patolojinin kalan yarısını YAPISAL olarak kapatır.
// Gerekçe (ENS-2003 v0.4.0 §3a): `c` bir ÖLÇÜM özelliğidir (gözlem ne kadar güvenilirdi), `1/τ_π`
// bir SÜREÇ özelliğidir (dünya ne hızla değişiyor). Kalman (1960) / üstel-unutmalı RLS / concept
// drift (Gama ve ark. 2014) çerçevesinde bunlar ölçüm ve süreç gürültüsüdür ve AYRI yerlere girer.
//
// YAPISAL YAN ETKİ (adversarial testlerin bağımsız bulguları, artık KONUSUZ):
//   • `AttributionConfidence = 1.0` yazmak, kaydı denetimsiz biçimde sönüm yasasının dışına
//     çıkarıyordu (λ(1)=0). Artık `c`'nin sönüme HİÇBİR etkisi yok — muafiyet kapısı kapandı.
//   • Büyük `γ` (ör. 1e6), `(1−c)^γ`'yı IEEE-754'te TAM SIFIRA underflow ettirip Curator'ı KURUM
//     ÇAPINDA kapatabiliyordu. `γ` artık YOK — anahtar kaldırıldı.
//
// ⚠️ DÜRÜST SINIR (fabrikasyon yok): bu revizyonu yazan context'te shell aracı YOKTU;
//   `dotnet build` / `dotnet test` ÇALIŞTIRILAMADI. Değişiklik statik olarak teoriye hizalanmıştır;
//   derleme ve test teyidi CI/owner'a aittir. Hiçbir test çıktısı iddia edilmemektedir.
//
// AÇIK BORÇ (işaretli): `τ_π` (context yarı-ömrü) Purpose-tipi başına KALİBRE EDİLMEMİŞTİR ve
//   Enterprise Ontology henüz bu alanı taşımaz — bu yüzden API `contextDecayRate`'i çağırandan
//   alır ve varsayılan tek bir global değer kullanır. Bu hâliyle sönüm "Purpose-tipine koşullu"
//   OLMA iddiasını taşımaz (ENS-2003 v0.4.0 §Failure conditions, ilk madde — açıkça yazılı).
//
// AÇIK BORÇ (D-4, işaretli): burası bir Memory **Graph** DEĞİLDİR — düz bir kayıt listesidir.
//   Memory Links (§1: precedent/revision/influence/contradiction) kodlanmadı ve düğüm Decision
//   Object'i taşımıyor. (AUDIT-WAVE2/D-4; bu turda kapsam dışı.)

/// <summary>
/// Memory Graph düğümü (ENS-2003 §1). **Kurucuda doğrulanır** (AUDIT §5.4/d): eskiden doğrulama
/// `RetentionPriority` ERİŞİMİNDE yapılıyordu, bu yüzden geçersiz bir kayıt (negatif
/// `|Learning|`, NaN confidence) sessizce yazılıyor ve sonra HER `Retrieve`/`FindStale`
/// çağrısını patlatıyordu — tek bozuk kayıt belleğin tamamını servis dışı bırakıyordu.
/// Artık geçersiz kayıt belleğe hiç GİREMEZ.
///
/// Not: pozisyonel record yerine açık kurucu kullanılıyor ki doğrulama hem `new` hem de
/// olası klon yollarında tek noktadan geçsin. Parametre adları PascalCase — çağıranların
/// adlandırılmış argümanları (`LearningMagnitude:` vb.) korunur.
/// </summary>
public sealed record MemoryRecord
{
    public Identity DecisionId { get; }
    public string PurposeType { get; }

    /// <summary>|Learning| — İŞARETSİZ büyüklük (ENS-2003 §3). Sonlu ve ≥ 0 olmak ZORUNDA.</summary>
    public double LearningMagnitude { get; }

    /// <summary>Attribution confidence (ENS-2004 §3). Sonlu ve [0,1] olmak ZORUNDA.</summary>
    public double AttributionConfidence { get; }

    /// <summary>İlk keşif damgası — DEĞİŞMEZ audit çapası (§3a). Yeniden-doğrulama buna dokunmaz.</summary>
    public DateTimeOffset AssertedAt { get; }

    public MemoryRecord(
        Identity DecisionId,
        string PurposeType,
        double LearningMagnitude,
        double AttributionConfidence,
        DateTimeOffset AssertedAt)
    {
        if (string.IsNullOrWhiteSpace(PurposeType))
            throw new ArgumentException(
                "Purpose-tipi boş olamaz — tipsiz kayıt hiçbir retrieval'da bulunamaz (ENS-2003 §Model 2).",
                nameof(PurposeType));

        Guard.NonNegativeFinite(LearningMagnitude, nameof(LearningMagnitude), "|Learning| büyüklüğü");
        Guard.UnitInterval(AttributionConfidence, nameof(AttributionConfidence), "Attribution confidence");

        this.DecisionId = DecisionId;
        this.PurposeType = PurposeType;
        this.LearningMagnitude = LearningMagnitude;
        this.AttributionConfidence = AttributionConfidence;
        this.AssertedAt = AssertedAt;
    }

    /// <summary>
    /// KAYIP-KORUMA ekseni (ENS-2003 v0.4.0 §3): `RetentionPriority(m) = |Learning(m)|`.
    /// Attribution confidence'tan **BAĞIMSIZDIR** — v0.4.0'ın çekirdek düzeltmesi budur
    /// (AUDIT-WAVE2/D-5: eskiden `|L|·c` idi ve `c` sönümde de sayıldığı için çift-sayım vardı).
    /// Gerekçe: atfı zayıf bir ders bize daha az GÜVENLE konuşmalıdır (bkz. <see cref="CapitalValue"/>),
    /// ama daha az KORUNMAYI hak etmez. Bu nicelik kesme (truncation) karşısında korumayı belirler
    /// — bkz. <see cref="CompanyMemory.RetrieveTop"/> karşı-survivorship tabanı.
    /// </summary>
    public double RetentionPriority => LearningMagnitude;

    /// <summary>
    /// EPİSTEMİK AĞIRLIK ekseni: `value(m) = |Learning| · c`. **Yeni bir kavram değildir** —
    /// ENS-3023 §Model 1'in `value(d)`'sinin ta kendisidir (alias yasağı, Anayasa Madde IV;
    /// bu yüzden hesap `DecisionCapital.Value`'ya delege edilir, kopyalanmaz).
    /// Bir dersin yeni bir kararı NE KADAR AĞIRLIKLA yönlendireceğini söyler; zamanı içermez.
    /// </summary>
    public double CapitalValue => DecisionCapital.Value(LearningMagnitude, AttributionConfidence);
}

/// <summary>
/// Yeniden-doğrulama izi (§3a + audit). Denetim (AUDIT §5.4/a) `Verify`'ın "hiçbir kanıt
/// istemiyor, iz bırakmıyor" olduğunu tespit etti; iz artık bırakılıyor.
/// **Açık borç:** `Evidence` hâlâ serbest metin — ENS-4025 L8'in tipli proof-trace referansı
/// (ProofTrace.cs dürüst-sınır (b) ile aynı Faz-4 borcu) henüz yok.
/// </summary>
public sealed record MemoryVerification(MemoryRecord Record, DateTimeOffset At, string Evidence);

public sealed class CompanyMemory
{
    private readonly List<MemoryRecord> _records = [];
    private readonly ReadOnlyCollection<MemoryRecord> _recordsView;
    private readonly HashSet<MemoryRecord> _index = [];

    // AUDIT §5.4/b: decay saati KAYIT bazında tutulur, DecisionId bazında DEĞİL. Eskiden aynı
    // karardan iki öğrenim varsa birini doğrulamak diğerini de tazeliyordu (çapraz-kirlenme).
    private readonly Dictionary<MemoryRecord, DateTimeOffset> _lastVerified = [];

    private readonly List<MemoryVerification> _verifications = [];
    private readonly ReadOnlyCollection<MemoryVerification> _verificationsView;

    private readonly TimeProvider _time;

    /// <param name="timeProvider">
    /// "Şimdi"nin kaynağı — yalnızca GELECEK tarihli yeniden-doğrulamayı reddetmek için
    /// kullanılır (AUDIT §5.4/a). Test/replay senaryolarında sabitlenebilir; verilmezse sistem
    /// saati. Retrieval/decay hesapları saatten BAĞIMSIZDIR (hepsi `asOf` parametrelidir) —
    /// determinizm korunur.
    /// </param>
    public CompanyMemory(TimeProvider? timeProvider = null)
    {
        _time = timeProvider ?? TimeProvider.System;
        _recordsView = new ReadOnlyCollection<MemoryRecord>(_records);
        _verificationsView = new ReadOnlyCollection<MemoryVerification>(_verifications);
    }

    /// <summary>Kayıt eklenir, asla silinmez (§3, audit — EC-001 ile tutarlı).</summary>
    public void Record(MemoryRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        _records.Add(record);
        _index.Add(record);
    }

    /// <summary>
    /// "Kayıt eklenir, ASLA silinmez" artık bir invariant (AUDIT §5.2). Eskiden canlı bir
    /// `List&lt;MemoryRecord&gt;` dönüyordu ve `((List&lt;MemoryRecord&gt;)memory.AllRecords).Clear()` ile
    /// kurumsal bellek tek satırda (reflection GEREKMEDEN) yok edilebiliyordu.
    /// </summary>
    public IReadOnlyList<MemoryRecord> AllRecords => _recordsView;

    /// <summary>Yeniden-doğrulama izi — her `Verify` çağrısı burada kayıt bırakır (§3a, audit).</summary>
    public IReadOnlyList<MemoryVerification> Verifications => _verificationsView;

    /// <summary>
    /// Yeniden-doğrulama (§3a, `asserted_at`/`last_verified` ayrımı): kaydın kendisi
    /// (`AssertedAt` = değişmez ilk-keşif, audit çapası) DEĞİŞMEZ; yalnızca decay saatini
    /// sıfırlayan ayrı bir `last_verified` damgası güncellenir.
    ///
    /// AUDIT §5.4'ün kapanışı — dört kısıt:
    ///   (a) Kayıt bu belleğe YAZILMIŞ olmalı (hayalet kayıt doğrulanamaz).
    ///   (b) Damga GELECEKTE olamaz — eskiden `Verify(id, +100 yıl)` decay'i kalıcı olarak
    ///       1.0'a sabitliyor ve kaydı curator'a sonsuza dek görünmez yapıyordu.
    ///   (c) Damga kanıtın kendisinden (`AssertedAt`) ÖNCE olamaz.
    ///   (d) Damga geriye alınamaz (monotonluk) — "doğrulamayı geri sarma" ile decay uydurulamaz.
    /// Ayrıca her çağrı `Verifications`'a iz bırakır.
    /// </summary>
    /// <param name="evidence">Neye dayanarak yeniden doğrulandığı — boş olamaz (P6/L8 ruhu).</param>
    public void Verify(MemoryRecord record, DateTimeOffset at, string evidence = "manuel curator onayı")
    {
        ArgumentNullException.ThrowIfNull(record);
        if (string.IsNullOrWhiteSpace(evidence))
            throw new ArgumentException("Yeniden-doğrulama gerekçesiz olamaz — izsiz doğrulama, izsiz türetimdir (Madde VI).", nameof(evidence));

        if (!_index.Contains(record))
            throw new InvalidOperationException(
                "Bu belleğe yazılmamış bir kayıt yeniden-doğrulanamaz (ENS-2003 §3a: doğrulama, kayıtlı evidence üzerinedir).");

        var now = _time.GetUtcNow();
        if (at > now)
            throw new ArgumentOutOfRangeException(nameof(at), at,
                "Yeniden-doğrulama GELECEK tarihli olamaz — ileri tarihli damga, decay yasasını " +
                "denetimsiz biçimde devre dışı bırakır ve kaydı Curator'a görünmez yapar (ENS-2003 §3b, AUDIT §5.4).");
        if (at < record.AssertedAt)
            throw new ArgumentOutOfRangeException(nameof(at), at,
                "Yeniden-doğrulama, doğruladığı kanıttan (AssertedAt) önce olamaz.");
        if (_lastVerified.TryGetValue(record, out var previous) && at < previous)
            throw new ArgumentOutOfRangeException(nameof(at), at,
                "Yeniden-doğrulama saati geriye alınamaz (monotonluk) — decay geçmişi uydurulamaz.");

        _lastVerified[record] = at;
        _verifications.Add(new MemoryVerification(record, at, evidence));
    }

    /// <summary>`last_verified` hiç kaydedilmemişse `asserted_at` fallback olur.</summary>
    public DateTimeOffset LastVerifiedOf(MemoryRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        return _lastVerified.TryGetValue(record, out var v) ? v : record.AssertedAt;
    }

    /// <summary>
    /// NEDENSELLİK KISITI (AUDIT §5.4/a, ikinci savunma katmanı): `asOf` anındaki bir sorgu,
    /// `asOf`'tan SONRA gerçekleşmiş bir doğrulamadan yararlanamaz. Saat-tabanlı kontrol
    /// atlansa/bozulsa bile decay yasası geçmişe dönük olarak devre dışı bırakılamaz.
    /// </summary>
    private DateTimeOffset EffectiveVerifiedAt(MemoryRecord record, DateTimeOffset asOf)
    {
        var verified = LastVerifiedOf(record);
        return verified <= asOf ? verified : record.AssertedAt;
    }

    /// <summary>
    /// Retrieval: benzer Purpose-tipi + salience sıralaması (§Model 2, §3). Zayıf/sönmüş
    /// kayıtlar hâlâ dönebilir (silinmez) — yalnızca sırası düşer.
    ///
    /// NOT (ENS-2003 v0.4.0 §3): bu metot KESMEZ, dolayısıyla karşı-survivorship tabanına ihtiyaç
    /// duymaz — kesilmeyen bir retrieval hiçbir dersi kaybetmez, yalnızca dikkat sırasını değiştirir.
    /// Kayıp "ilk k" alındığı anda doğar; taban orada devrededir (<see cref="RetrieveTop"/>).
    /// </summary>
    /// <param name="contextDecayRate">
    /// λ_π — Purpose-tipinin context sönüm hızı (1/gün). `λ_π = ln2/τ_π`, `τ_π` = context yarı-ömrü.
    /// ⚠️ Kalibre EDİLMEMİŞTİR ve şu an ontolojiden değil çağırandan gelir (açık borç, dosya başı).
    /// </param>
    public IReadOnlyList<MemoryRecord> Retrieve(string purposeType, DateTimeOffset asOf, double contextDecayRate = 0.01)
    {
        if (string.IsNullOrWhiteSpace(purposeType))
            throw new ArgumentException("Purpose-tipi zorunlu.", nameof(purposeType));

        // AUDIT_FIXED_E6: doğrulama KAPIDA yapılır, kayıt-başına değil. Eskiden `Salience`
        // yalnızca eşleşen kayıt varsa çağrıldığından, `NaN` bir oran bellek boşken sessizce
        // kabul edilip doluyken patlıyordu — hata anı VERİYE BAĞLIYDI. Fail-closed politikası
        // (Guard.cs) girdiyi kapıda reddetmeyi şart koşar.
        Guard.NonNegativeFinite(contextDecayRate, nameof(contextDecayRate), "Context sönüm hızı");

        return _records
            .Where(r => r.PurposeType == purposeType)
            .OrderByDescending(r => Salience(r, asOf, contextDecayRate))
            .ToList();
    }

    /// <summary>
    /// KARŞI-SURVIVORSHIP TABANI (ENS-2003 v0.4.0 §3, kesme invariant'ı):
    /// bir Purpose-tipinin `argmax RetentionPriority` (= `argmax |Learning|`) kaydı.
    /// Eşitlikte determinizm için EN ESKİ `AssertedAt` kazanır (özgün ders, kopyası değil).
    /// Tipte hiç kayıt yoksa `null`.
    /// </summary>
    public MemoryRecord? CounterSurvivorshipFloor(string purposeType)
    {
        if (string.IsNullOrWhiteSpace(purposeType))
            throw new ArgumentException("Purpose-tipi zorunlu.", nameof(purposeType));

        MemoryRecord? best = null;
        foreach (var r in _records)
        {
            if (r.PurposeType != purposeType) continue;
            if (best is null
                || r.RetentionPriority > best.RetentionPriority
                || (r.RetentionPriority == best.RetentionPriority && r.AssertedAt < best.AssertedAt))
            {
                best = r;
            }
        }
        return best;
    }

    /// <summary>
    /// KESİLMİŞ retrieval + karşı-survivorship TABANI (ENS-2003 v0.4.0 §3 — D-5'in çözümünün
    /// yapısal yarısı).
    ///
    /// Invariant: `limit ≥ 1` kayıt döndürülürken, o Purpose-tipinin en yüksek
    /// `RetentionPriority`'li kaydı sonuç kümesinde **kalmak ZORUNDADIR** — `AttributionConfidence`'ı
    /// ne kadar düşük, yaşı ne kadar büyük olursa olsun. Taban kaydı, salience'ı EN DÜŞÜK olan
    /// slotun yerini alır; sıralamanın geri kalanı bozulmaz.
    ///
    /// NEDEN: ENS-2003 §3'ün varlık sebebi karşı-survivorship'tir (*"başarısız ama ölçülmüş kararlar
    /// en yüksek retention önceliğini alır"*), ama bir başarısızlığın attribution'ı tipik olarak
    /// ZAYIFTIR (ENS-2004 §3: çoğu karar L1'e sıkışır). `c` sönümden çıkarılınca çifte ceza kalktı;
    /// bu invariant, kalan TEK cezanın (düşük `value` → düşük sıra) dersi görünmez yapmasını da
    /// engeller. §3'ün üçüncü politikasının (*"sıkıştır ama en az bir başarısızlık örneğini koru"*)
    /// zorlanabilir hâlidir.
    ///
    /// BEDELİ (dürüstçe, ENS-2003 v0.4.0 §Failure conditions): (a) her çağrıda bir slot tüketir;
    /// (b) YALNIZCA BİR kaydı korur — `k−1` zayıf-atıflı büyük ders hâlâ kesilir; (c) `|Learning|`
    /// yanlış ölçülmüşse zehirli kaydın görünürlüğünü GARANTİ eder (memory poisoning amplifikasyonu).
    /// </summary>
    public IReadOnlyList<MemoryRecord> RetrieveTop(
        string purposeType, int limit, DateTimeOffset asOf, double contextDecayRate = 0.01)
    {
        if (limit < 1)
            throw new ArgumentOutOfRangeException(nameof(limit), limit,
                "Kesme sınırı en az 1 olmalı — 0 kayıt döndüren bir retrieval, karşı-survivorship tabanını (ENS-2003 §3) taşıyamaz.");

        var ordered = Retrieve(purposeType, asOf, contextDecayRate);
        if (ordered.Count <= limit) return ordered;

        var top = ordered.Take(limit).ToList();
        var floor = CounterSurvivorshipFloor(purposeType);

        // Referans kimliği bilinçli: değer-eşit bir "ikiz" kayıt, tabanın KENDİSİ değildir.
        if (floor is not null && !top.Any(r => ReferenceEquals(r, floor)))
            top[^1] = floor;   // en düşük salience'lı slot tabana bırakılır

        return top;
    }

    /// <summary>
    /// SAF TAZELİK ekseni (ENS-2003 v0.4.0 §3a): `decayFactor(m,t) = exp(−λ_π·(t−last_verified))`,
    /// değer aralığı (0,1]. **`AttributionConfidence`'ı İÇERMEZ** — v0.4.0'ın çekirdek düzeltmesi
    /// (AUDIT-WAVE2/D-5). Stale-yargısı bu eksende tanımlıdır; epistemik ağırlık onu maskelemez.
    /// </summary>
    public double DecayFactor(MemoryRecord record, DateTimeOffset asOf, double contextDecayRate)
    {
        ArgumentNullException.ThrowIfNull(record);
        Guard.NonNegativeFinite(contextDecayRate, nameof(contextDecayRate),
            "λ_π context sönüm hızı (ENS-2003 v0.4.0 §3a)");

        double ageDays = Math.Max(0, (asOf - EffectiveVerifiedAt(record, asOf)).TotalDays);
        return Math.Exp(-contextDecayRate * ageDays);
    }

    /// <summary>
    /// BİLEŞİK retrieval-sıralama skoru (ENS-2003 v0.4.0 §3a):
    ///   `Salience(m,t) = value(m) × decayFactor(m,t) = |Learning|·c · exp(−λ_π·Δt)`.
    /// İki eksen çarpımla birleşir ama KARIŞMAZ: argüman kümeleri ayrıktır — `c` yalnızca
    /// `value`'da, zaman/volatilite yalnızca `decayFactor`'da. (v0.3'te ikisi de `c`'ye bağlıydı:
    /// çift-sayım, AUDIT-WAVE2/D-5.) `FindStale` doğrudan `DecayFactor`'a bakar; `RetrieveTop`
    /// doğrudan `RetentionPriority`'ye — üç nicelik operasyonel olarak da ayrık kalır.
    /// </summary>
    public double Salience(MemoryRecord record, DateTimeOffset asOf, double contextDecayRate)
        => record.CapitalValue * DecayFactor(record, asOf, contextDecayRate);

    /// <summary>
    /// Curator sweep — TAZELİK ekseni (ENS-2003 v0.4.0 §3a/§3b; P5/P7: yalnızca inceleme sinyali,
    /// otonom mutasyon yok): `decayFactor &lt; staleThreshold` olan kayıtları **bulur** — asla silmez
    /// ya da değiştirmez. Anlamı: *"bağlam değişmiş olabilir, YENİDEN DOĞRULA"*.
    ///
    /// AUDIT §5.4/c'nin kapanışı: eskiden filtre `RetentionPriority &gt; 0 &amp;&amp;
    /// Salience/RetentionPriority &lt; eşik` idi; `|Learning| = 0` veya `confidence = 0` olan kayıt
    /// 100 yıl geçse de ASLA bayraklanmıyordu ve bölme 0/0 üretebiliyordu. Artık tazelik ekseni
    /// DOĞRUDAN hesaplanır; retention filtresi yoktur.
    /// </summary>
    public IReadOnlyList<MemoryRecord> FindStale(DateTimeOffset asOf, double contextDecayRate, double staleThreshold = 0.5)
    {
        Guard.UnitInterval(staleThreshold, nameof(staleThreshold), "Stale eşiği");
        // AUDIT_FIXED_E6: kapıda doğrula (bkz. Retrieve) — boş bellek geçersiz oranı maskelemesin.
        Guard.NonNegativeFinite(contextDecayRate, nameof(contextDecayRate), "Context sönüm hızı");

        return _records
            .Where(r => DecayFactor(r, asOf, contextDecayRate) < staleThreshold)
            .ToList();
    }

    /// <summary>
    /// Curator sweep — EPİSTEMİK eksen (ENS-2003 v0.4.0 §3a "ikinci inceleme sinyali", §3b).
    /// `AttributionConfidence &lt; minConfidence` olan kayıtları **bulur**; asla silmez/değiştirmez
    /// ve `RetentionPriority`'yi DÜŞÜRMEZ.
    ///
    /// NEDEN VAR: v0.3'te düşük-`c` kayıtlar sönüm üzerinden dolaylı olarak bayraklanıyordu — ama o,
    /// D-5'in çift-sayımının bir yan ürünüydü. `c` sönümden çıkarılınca o sinyal kaybolurdu; v0.4.0
    /// onu DOĞRU eksene taşır. Anlamı farklıdır ve fark önemlidir: `stale` "yeniden doğrula" der,
    /// bu ise **"attribution SEVİYESİNİ yükselt"** der — ENS-2004 §4a adım 3(iii)'ün doğrudan
    /// tetikleyicisi (*"bu Purpose-tipi L1'e sıkışıyor, L2 doğal-deney eşlemesi kurulabilir"*).
    ///
    /// AÇIK BORÇ (işaretli): `minConfidence` için teoride türetilmiş bir değer YOKTUR; politika
    /// parametresidir (ENS-2003 v0.4.0 §Failure "c_min ve θ keyfi"). Çok yüksek seçilirse liste
    /// taşar (P5 öneri-yorgunluğu), çok düşük seçilirse sinyal hiç ateşlenmez.
    /// </summary>
    public IReadOnlyList<MemoryRecord> FindWeaklyAttributed(double minConfidence = 0.5)
    {
        Guard.UnitInterval(minConfidence, nameof(minConfidence), "Zayıf-atıf eşiği (c_min)");

        return _records
            .Where(r => r.AttributionConfidence < minConfidence)
            .ToList();
    }
}

// TRACE: ENS-2003 v0.4.0 §3a — context-koşullu sürekli decay: λ_π = ln2/τ_π, decayFactor = exp(−λ_π·Δt).
// `τ_π` (context yarı-ömrü, gün) Purpose-tipinin KARARLARINI GEÇERLİ KILAN BAĞLAMIN yarı yarıya
// bayatlama süresidir — bir domain uzmanına DOĞRUDAN sorulabilir ("bu karar sınıfının context'i kaç
// günde yarı yarıya bayatlar?"). v0.3'ün `γ`'sı sorulamıyordu ve hiçbir tek `γ` eldeki üç çapa
// noktasını fit edemiyordu (SKR-040/D3); v0.4.0 o açmazı bir parametreyi KALDIRARAK çözer.
//
// KALDIRILANLAR (v0.4.0, AUDIT-WAVE2/D-5): `attributionConfidence` ve `gamma` parametreleri.
// Sönüm hızı artık attribution confidence'a BAĞLI DEĞİLDİR — `c` yalnızca `MemoryRecord.CapitalValue`
// (= ENS-3023 §Model 1 `value(d)`) içinde sayılır. Bu, iki adversarial açığı da yapısal olarak
// kapatır: `c=1.0`'ın denetimsiz sönüm muafiyeti ve büyük `γ`'nın rate'i sıfıra underflow ettirmesi.
//
// DÜRÜSTÇE İŞARETLİ: `τ_π` HENÜZ ampirik kalibre EDİLMEMİŞTİR ve Enterprise Ontology bu alanı
// taşımaz (ENS-2003 v0.4.0 §Failure conditions, ilk madde — v0.4.0'ın en zayıf noktası olarak
// açıkça yazılı). Tek global bir değer kullanıldığı sürece sönüm "Purpose-tipine koşullu" DEĞİLDİR.
public static class DecayFunction
{
    /// <summary>λ_π = ln2 / τ_π — context yarı-ömründen (gün) sönüm hızına (1/gün).</summary>
    public static double RateFromHalfLife(double contextHalfLifeDays)
    {
        Guard.PositiveFinite(contextHalfLifeDays, nameof(contextHalfLifeDays),
            "τ_π context yarı-ömrü (gün; ENS-2003 v0.4.0 §3a — sıfır/negatif yarı-ömrün anlamı yok)");

        return Math.Log(2) / contextHalfLifeDays;
    }

    /// <summary>τ_π = ln2 / λ_π — sönüm hızından yarı-ömre. `λ_π = 0` ⇒ sönüm yok (∞).</summary>
    public static double HalfLifeDays(double contextDecayRate)
    {
        Guard.NonNegativeFinite(contextDecayRate, nameof(contextDecayRate),
            "λ_π context sönüm hızı");

        return contextDecayRate == 0 ? double.PositiveInfinity : Math.Log(2) / contextDecayRate;
    }

    /// <summary>
    /// Bayatlama süresi: `t_stale(π) = τ_π · log₂(1/θ)` (ENS-2003 v0.4.0 §3a "Yarı-ömür / TTL denkliği").
    /// θ = 0.5'te `t_stale = τ_π` — eşik ile yarı-ömür çakışır. `c`'den BAĞIMSIZDIR.
    /// </summary>
    public static double DaysUntilStale(double contextDecayRate, double staleThreshold)
    {
        Guard.NonNegativeFinite(contextDecayRate, nameof(contextDecayRate), "λ_π context sönüm hızı");
        if (!(staleThreshold > 0 && staleThreshold < 1))
            throw new ArgumentOutOfRangeException(nameof(staleThreshold), staleThreshold,
                "Stale eşiği (0,1) aralığında olmalı — 0 Curator'ı sessizce kapatır, 1 her kaydı bayat sayar.");

        return contextDecayRate == 0 ? double.PositiveInfinity : Math.Log(1.0 / staleThreshold) / contextDecayRate;
    }
}
