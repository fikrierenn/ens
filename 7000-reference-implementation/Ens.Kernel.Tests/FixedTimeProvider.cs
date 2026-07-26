namespace Ens.Kernel.Tests;

// TRACE: AUDIT.md §5.4/a — `CompanyMemory.Verify` artık GELECEK tarihli damgayı reddediyor.
// O kontrolün "şimdi"ye ihtiyacı var; testlerin ise DETERMİNİZME. Bu sabit-saat sağlayıcısı
// ikisini uzlaştırır: sistem saatine hiçbir test bağımlı değildir.
internal sealed class FixedTimeProvider(DateTimeOffset now) : TimeProvider
{
    public override DateTimeOffset GetUtcNow() => now;
}
