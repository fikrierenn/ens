using System.Collections.Frozen;

namespace Ens.Kernel.Capability;

// TRACE: ADR-0001 (accepted, v0.3.1) §6 "Plugin mimarisi — Capability Runtime"
// TRACE: ADR-0001 §6.1 Prior art — OSGi (service registry), MCP (tools/list), Terraform
//        (versioned provider registry), VS Code (activation events/contribution points),
//        K8s Operators/CRD, WordPress hooks + iki dahili kanıt (CrewOps CapabilityPack,
//        Mosaik/reporthub ModuleLoader assembly-scan). Mekanizma ENS'in İCADI DEĞİL.
// TRACE: ENS-4010 `ens-core:Capability` node'u — Pack, bu node'un örnek-kümesini kaydeder.
// TRACE: Anayasa P7 + ADR-0001 §5.6 — `RequiresHumanApprovalFor`, Gate'in policy zarfını besler.
//
// ============================ ENS'İN DAR DELTASI (§6.1'den, abartısız) ============================
// §6 mekanizması (registry + versiyonlama + dinamik yükleme) olgun prior-art'ta zaten var ve
// defalarca bağımsız yeniden-keşfedilmiş. ENS'in gerçek katkısı yalnızca İKİ BAĞ:
//   (1) Eklenti-birimi ENS-4010 `Capability` node'una TİPLENİR (prior-art'ın hiçbiri eklentiyi
//       bir foundational ontology'ye bağlamaz).
//   (2) Pack'in deklaratif `AllowedTools`/`RequiresHumanApprovalFor` izinleri doğrudan
//       Bounded-Autonomy Gate'e (P7) beslenir — per-capability human-approval'ı birinci-sınıf
//       registry alanı yapan tek sistem. MCP tool-registry'si yetkilendirmeye AGNOSTİKTİR.
// Progressive-loading (§6 3-tier) delta SAYILMAZ — VS Code'la aynı mekanizma, farklı gerekçe.
// ==================================================================================================
//
// ============================ DÜRÜST SINIRLAR (Faz-4) ============================
// (a) LOCAL-FIRST, marketplace yok (§6'nın kendi V1 sınırı). Assembly-scan/dinamik yükleme
//     (Mosaik ModuleLoader deseni) YOK — Pack'ler kod içinde kaydedilir.
// (b) SANDBOX YOK — §6 "her Capability action'ı izole workspace'te koşar" der; izolasyon,
//     prompt-injection sanitizasyonu, kaynak kotası kodlanmadı (ActuationLayer'ın "guarded
//     yapısal ama operasyonel değil" sınırıyla aynı).
// (c) Progressive context (3-tier skill loading) kodlanmadı — Pack metadata'sı tek parça.
// (d) Versiyon çakışması: §6 "sessizce çözmez, uyarır" der; burada aynı isimli iki Pack
//     kaydı AÇIK HATA verir (uyarı-kanalı yok, en muhafazakâr yorum).
// ================================================================================
//
// ============================ DENETİM SONRASI İKİ DÜZELTME ============================
// (1) İZİN KÜMELERİ ARTIK GERÇEKTEN DEĞİŞTİRİLEMEZ (AUDIT §5.2). `AllowedTools` bir
//     `IReadOnlySet<string>` olarak sunuluyordu ama arkasındaki nesne canlı bir
//     `HashSet<string>`'ti; reflection GEREKMEDEN tek satırda yetki eklenebiliyordu:
//         ((HashSet<string>)pack.AllowedTools).Add("delete_database");   // yetki kaçağı
//     ve registry bunu fark etmiyordu. Artık `FrozenSet<string>` kullanılıyor — downcast
//     `InvalidCastException` atar, "deklaratif izinler" runtime'da gerçekten sabittir.
// (2) `Disable` ARTIK BİR GÜVENLİK KONTROLÜNÜ DÜŞÜREMEZ (AUDIT §5.5/F3). Bir aracı iki Pack
//     veriyorsa ve KATI olan (`RequiresHumanApprovalFor`) devre dışı bırakılırsa, araç yetkili
//     kalıyor ama insan-onayı şartı sessizce düşüyordu: bir "kapasite kapatma" jesti P7
//     korumasını kaldırıyordu. Artık YETKİ yalnızca ETKİN Pack'lerden gelir, ama ONAY ŞARTI
//     KAYITLI TÜM Pack'lerden toplanır (devre-dışı olanlar dahil). Disable bir yetki-KALDIRMA
//     jestidir; asla bir kısıtı kaldırmaz.
// ======================================================================================

/// <summary>
/// Versiyonlu, domain-scoped, kendine yeten yeti paketi (§6). ENS-4010 `Capability` node'unun
/// örnek-kümesini kernel'e kaydeder. İzinler DEKLARATİFTİR — Gate'in policy zarfını besler.
/// </summary>
public sealed record CapabilityPack
{
    public string Name { get; }
    public string Version { get; }

    /// <summary>
    /// Bu Pack'in kullanmasına izin verilen araçlar (§6 `allowedTools`). `FrozenSet` — kayıttan
    /// SONRA değiştirilemez (AUDIT §5.2: downcast'le yetki eklenmesi kapatıldı).
    /// </summary>
    public IReadOnlySet<string> AllowedTools => _allowedTools;

    /// <summary>
    /// İnsan onayı ZORUNLU olan araçlar (§6 `requiresHumanApprovalFor`). ENS deltası: bu alan
    /// doğrudan Bounded-Autonomy Gate'e (P7) beslenir (bkz. `BoundedAutonomyGate.Evaluate`'in
    /// `toolAuthorization` parametresi) — MCP/OSGi/Terraform'da karşılığı yok.
    /// </summary>
    public IReadOnlySet<string> RequiresHumanApprovalFor => _requiresHumanApprovalFor;

    private readonly FrozenSet<string> _allowedTools;
    private readonly FrozenSet<string> _requiresHumanApprovalFor;

    public CapabilityPack(
        string name,
        string version,
        IEnumerable<string> allowedTools,
        IEnumerable<string>? requiresHumanApprovalFor = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Pack adı zorunlu.", nameof(name));
        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException("Pack versiyonu zorunlu (§6: versiyonlu paket).", nameof(version));
        ArgumentNullException.ThrowIfNull(allowedTools);

        Name = name;
        Version = version;

        // Savunmacı kopya + GERÇEKTEN değiştirilemez tip (AUDIT §5.2): çağıranın elindeki
        // koleksiyon sonradan değişse de Pack'in izinleri değişmez; dönen küme downcast
        // edilemez.
        _allowedTools = allowedTools.ToFrozenSet(StringComparer.Ordinal);
        _requiresHumanApprovalFor = (requiresHumanApprovalFor ?? []).ToFrozenSet(StringComparer.Ordinal);

        // Tutarlılık: onay-gerektiren bir araç, izinli araçlar kümesinde olmalı — yoksa
        // policy sessizce anlamsızdır (§6 "sessizce çözmez").
        var orphan = _requiresHumanApprovalFor.FirstOrDefault(t => !_allowedTools.Contains(t));
        if (orphan is not null)
            throw new ArgumentException(
                $"'{orphan}' onay-gerektiren araç listesinde ama AllowedTools'ta yok — anlamsız policy.",
                nameof(requiresHumanApprovalFor));
    }
}

/// <summary>Bir aracın bu Pack altında çalıştırılabilirlik kararı (Gate'e girdi).</summary>
public sealed record ToolAuthorization(bool IsAllowed, bool RequiresHumanApproval, string Reason);

/// <summary>
/// Tüm Pack'lerin merkezî otoritesi (§6). Kernel değişmeden yeti eklenir: "kernel değişmez,
/// Pack takılır" (Anayasa Madde V Replaceable/Modular).
/// </summary>
public sealed class CapabilityRegistry
{
    private readonly Dictionary<string, CapabilityPack> _packs = new(StringComparer.Ordinal);
    private readonly HashSet<string> _disabled = new(StringComparer.Ordinal);

    /// <summary>Pack kaydeder. Aynı isim iki kez = AÇIK HATA (§6: çakışmayı sessizce çözmez).</summary>
    public void Register(CapabilityPack pack)
    {
        ArgumentNullException.ThrowIfNull(pack);
        if (_packs.ContainsKey(pack.Name))
            throw new InvalidOperationException(
                $"'{pack.Name}' zaten kayıtlı (kayıtlı: v{_packs[pack.Name].Version}, gelen: v{pack.Version}). " +
                "§6: versiyon çakışması sessizce çözülmez.");

        _packs[pack.Name] = pack;
    }

    /// <summary>Proje/tenant bazında devre dışı bırakma (§6). Kayıt silinmez — yalnızca pasifleşir.</summary>
    public void Disable(string packName) => _disabled.Add(packName);
    public void Enable(string packName) => _disabled.Remove(packName);

    public bool IsEnabled(string packName) => _packs.ContainsKey(packName) && !_disabled.Contains(packName);
    public IReadOnlyCollection<CapabilityPack> Packs => _packs.Values;
    public IReadOnlyCollection<CapabilityPack> EnabledPacks =>
        _packs.Values.Where(p => !_disabled.Contains(p.Name)).ToList();

    /// <summary>
    /// **ENS deltası burada** (§6.1): deklaratif Pack izinlerini Gate'in anlayacağı bir karara
    /// çevirir. Bir araç hiçbir etkin Pack tarafından izinli değilse çalıştırılamaz; izinli ama
    /// onay-gerektiren bir araç Gate'e "insan onayı şart" sinyali taşır (P7).
    /// </summary>
    public ToolAuthorization Authorize(string toolName)
    {
        if (string.IsNullOrWhiteSpace(toolName))
            throw new ArgumentException("Araç adı zorunlu.", nameof(toolName));

        // YETKİ yalnızca ETKİN Pack'lerden gelir — Disable gerçekten yetkiyi kaldırır.
        var granting = EnabledPacks.Where(p => p.AllowedTools.Contains(toolName)).ToList();
        if (granting.Count == 0)
            return new ToolAuthorization(false, false,
                $"'{toolName}' hiçbir etkin Capability Pack tarafından izinli değil.");

        // ONAY ŞARTI ise KAYITLI TÜM Pack'lerden toplanır — devre dışı bırakılmış olanlar DAHİL
        // (AUDIT §5.5/F3). Gerekçe asimetriktir ve kasıtlıdır: `Disable` bir yetki-kaldırma
        // jestidir; bir kısıtı kaldırma yetkisi YOKTUR. Aksi hâlde katı bir Pack'i devre dışı
        // bırakmak, aracı gevşek bir Pack üzerinden ONAYSIZ çalıştırılabilir hâle getirirdi —
        // bir güvenlik kontrolünün sessizce düşmesi (P7 ihlali).
        var approvalPack = _packs.Values.FirstOrDefault(p => p.RequiresHumanApprovalFor.Contains(toolName));
        if (approvalPack is null)
            return new ToolAuthorization(true, false,
                $"'{toolName}' izinli ('{granting[0].Name}' v{granting[0].Version}), onay gerekmiyor.");

        bool fromDisabledPack = _disabled.Contains(approvalPack.Name);
        return new ToolAuthorization(true, true,
            $"'{toolName}' izinli ('{granting[0].Name}' v{granting[0].Version}) ancak " +
            $"'{approvalPack.Name}' v{approvalPack.Version} insan onayı şart koşuyor (P7)" +
            (fromDisabledPack
                ? " — bu Pack DEVRE DIŞI olsa da kısıt korunur: Disable yetki kaldırır, kısıt kaldırmaz."
                : "") + ".");
    }
}
