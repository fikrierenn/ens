using Ens.Kernel.Capability;
using Xunit;

namespace Ens.Kernel.Tests;

// TRACE: ADR-0001 §6 / §6.1 — registry mekanizması prior-art; ENS deltası = deklaratif izinlerin
// Bounded-Autonomy Gate'e (P7) beslenmesi. Bu testler ÖZELLİKLE o deltayı doğrular.

public sealed class CapabilityRegistryTests
{
    private static CapabilityPack OpsPack() => new(
        "Operations", "1.2",
        allowedTools: ["read_stock", "create_purchase_order"],
        requiresHumanApprovalFor: ["create_purchase_order"]);

    private static CapabilityPack ReportingPack() => new(
        "Reporting", "0.9",
        allowedTools: ["read_stock", "render_report"]);

    [Fact]
    public void Unknown_tool_is_not_authorized()
    {
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());

        var auth = registry.Authorize("delete_database");

        Assert.False(auth.IsAllowed);
        Assert.Contains("izinli değil", auth.Reason);
    }

    [Fact]
    public void Allowed_tool_without_approval_requirement_passes_freely()
    {
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());

        var auth = registry.Authorize("read_stock");

        Assert.True(auth.IsAllowed);
        Assert.False(auth.RequiresHumanApproval);
    }

    [Fact]
    public void Declarative_approval_requirement_reaches_the_gate()
    {
        // ENS deltası (§6.1): per-capability human-approval birinci-sınıf registry alanı.
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());

        var auth = registry.Authorize("create_purchase_order");

        Assert.True(auth.IsAllowed);
        Assert.True(auth.RequiresHumanApproval);
        Assert.Contains("P7", auth.Reason);
    }

    [Fact]
    public void Any_pack_requiring_approval_wins_conservatively()
    {
        // İki Pack aynı aracı veriyor; biri onay istiyor → onay gerekir (P7 yönünde hata yap).
        var registry = new CapabilityRegistry();
        registry.Register(new CapabilityPack("Loose", "1.0", ["shared_tool"]));
        registry.Register(new CapabilityPack("Strict", "1.0", ["shared_tool"], ["shared_tool"]));

        Assert.True(registry.Authorize("shared_tool").RequiresHumanApproval);
    }

    [Fact]
    public void Kernel_gains_capability_without_changing_kernel_code()
    {
        // §6: "kernel değişmez, Pack takılır" — yeni Pack, yeni araç.
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());
        Assert.False(registry.Authorize("render_report").IsAllowed);

        registry.Register(ReportingPack());
        Assert.True(registry.Authorize("render_report").IsAllowed);
    }

    [Fact]
    public void Duplicate_pack_name_is_an_explicit_error_not_silent_resolution()
    {
        // §6: "versiyon çakışmalarını sessizce çözmez, uyarır."
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());

        var ex = Assert.Throws<InvalidOperationException>(
            () => registry.Register(new CapabilityPack("Operations", "2.0", ["read_stock"])));
        Assert.Contains("sessizce", ex.Message);
    }

    [Fact]
    public void Disabled_pack_stops_granting_but_is_not_deleted()
    {
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());
        registry.Disable("Operations");

        Assert.False(registry.Authorize("read_stock").IsAllowed);
        Assert.False(registry.IsEnabled("Operations"));
        Assert.Single(registry.Packs);          // kayıt duruyor
        Assert.Empty(registry.EnabledPacks);
    }

    [Fact]
    public void Re_enabling_restores_authorization()
    {
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());
        registry.Disable("Operations");
        registry.Enable("Operations");

        Assert.True(registry.Authorize("read_stock").IsAllowed);
    }

    [Fact]
    public void Pack_requires_version()
    {
        Assert.Throws<ArgumentException>(() => new CapabilityPack("X", "  ", ["t"]));
    }

    [Fact]
    public void Approval_list_referencing_unallowed_tool_is_rejected()
    {
        // Anlamsız policy sessizce kabul edilmez.
        Assert.Throws<ArgumentException>(
            () => new CapabilityPack("X", "1.0", allowedTools: ["a"], requiresHumanApprovalFor: ["b"]));
    }

    [Fact]
    public void Empty_tool_name_throws()
    {
        var registry = new CapabilityRegistry();
        registry.Register(OpsPack());
        Assert.Throws<ArgumentException>(() => registry.Authorize("  "));
    }
}
