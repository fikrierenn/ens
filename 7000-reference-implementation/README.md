# ENS Reference Implementation — Faz 4

**Yetki:** Anayasa Madde VII (Faz 4: teoriyi kanıtla, optimize etme) + Madde VIII (yalnızca
**Accepted** ADR'lere dayanır).

Bu, ENS'in ilk çalışan kodudur. Amaç production değil — **teorinin çalışabilir olduğunu
kanıtlamak** (P8).

## Yapı
```
Ens.Kernel/
  Domain/
    Identity.cs           — TRACE: ENS-4001 §Computational primitifler (deneysel, açık soru)
    DomainEvent.cs         — TRACE: ENS-4001 §Event, Axiom 3 (Non-Leakage)
    DecisionAggregate.cs   — TRACE: ENS-2001 §Individuation, ADR-0001 §5.4
    ContextScore.cs         — TRACE: ENS-2002 §3 (coverage−noise−staleness) + §Implications (gate)
    CompanyMemory.cs        — TRACE: ENS-2003 §1 (Memory Graph), §3 (retention ∝ |Learning|, sönümle-silme)
    Events/DecisionEvents.cs — TRACE: ENS-2001 §Lifecycle, ENS-2004 §3 (Attribution)
  Laws/
    DecisionEntropy.cs     — TRACE: ENS-3021 (H(A|C) = I(A;Owner|C) + H(A|C,Owner))
    DecisionGravity.cs     — TRACE: ENS-3022 (InfoNeed = Stake × (1−Confidence), Howard 1966)
    DecisionCapital.cs     — TRACE: ENS-3023 (Value/ΔCapital/ReuseROI — akış, stok değil)
  BoundedAutonomyGate.cs   — TRACE: ADR-0001 §5.6, Anayasa P7 (bounded autonomy'nin ilk kodu)
Ens.Kernel.Tests/           — invariant testleri (ADR/teori iddialarını doğrular)
Ens.Kernel.Demo/            — uçtan uca senaryo: tedarikçi seçimi kararı, tüm parçalar tek akışta
```

## Durum
| Yapıt | Test | Kanıt seviyesi (evidence-standard.md) |
|---|---|---|
| ENS-2001 (Decision, §Individuation) | 8/8 geçti | eng: E0 → **E1** |
| ENS-2002 (Context Score) | 9/9 geçti | eng: E0 → **E1** |
| ENS-2003 (Company Memory) | 8/8 geçti | eng: E0 → **E1** |
| ENS-3021 (Decision Entropy) | 5/5 geçti | eng: E0 → **E1** |
| ENS-3022 (Decision Gravity) | 8/8 geçti | eng: E0 → **E1** |
| ENS-3023 (Decision Capital) | 9/9 geçti | eng: E0 → **E1** |
| ADR-0001 §5.6 (Bounded-Autonomy Gate) | 6/6 geçti | eng: E0 → **E1** |

**Toplam: 54/54 geçti** (gerçek `dotnet test` çıktısı, iddia değil). **Fizik üçlüsü (Entropy,
Gravity, Capital) + Context Score + Company Memory, hepsi kodlanmış ve test edilmiş.**

## Bilinçli sadeleştirmeler (Faz-4, dürüstçe işaretli)
- **Identity**'nin primitif statüsü hâlâ açık (ENS-4001 Design Review) — kod bu tartışmayı
  çözmez, yalnızca mühendislik ihtiyacını (aggregate-id) karşılar.
- **DecisionEntropy** context-benzerliğini hâlâ `ContextKey` (string) ile temsil ediyor —
  `ContextScore` artık kodlu ama henüz `ContextKey` üretimine bağlanmadı (sıradaki adım).
- **ContextScore.coverage hâlâ dışarıdan verilir** — `CompanyMemory` artık kodlu ama
  `ContextScore.coverage`'a henüz bağlanmadı (ENS-2002 §Model 2'nin tam kapanışı, sıradaki adım).
- **CompanyMemory'nin decay fonksiyonu ENS-2003'ün taahhüdü değil** — §3 yalnızca "sönümle" der,
  fonksiyonel biçim vermez; burada basit üstel decay (`exp(−rate×gün)`) seçildi, dürüstçe işaretli.
- **Purpose-tipi string'dir** — ENS-2003 §Model 2'nin gerektirdiği Enterprise Ontology (ENS-4020)
  kaynaklı sınıflandırmaya henüz bağlı değil.
- **Exploration modu (§4) kodlanmadı** — CompanyMemory yalnızca exploitation-retrieval yapar.
- **DecisionGravity.Stake** dışarıdan verilir — Alternative-başına `ExpectedValue` (OL1) henüz
  Decision Object'te yok, `Stake = spread(ExpectedValue)` hesaplanamıyor.
- **BoundedAutonomyGate.ConformanceDeficit** Company Memory (ENS-2003) olmadan hesaplanamaz,
  çağıran katman 0 verirse `AttentionPriority` yanlış-negatif riski taşır (kod içi not).
- **BoundedAutonomyGate Policy** minimalist (iki eşik + isIrreversible bayrağı) — gerçek
  Policy/Constraint node'ları (ENS-4010) henüz bağlanmadı; bu modelin ilk çalışan yaklaşımı.
- **DecisionCapital Stok hesabı kasıtlı olarak yok** — ENS-3023 "stok=Memory, Capital=onun
  dinamiği" der; Company Memory (ENS-2003) henüz kodlanmadı, bu yüzden yalnızca akış
  (yatırım−amortisman) ve ROI kodlandı, `investment`/`amortization` toplamları dışarıdan verilir.
- **`intent: exploit|explore`** (OE1) ve **Alternative-başına ExpectedValue** (OL1) henüz
  Decision Object'te yok — ROADMAP'ta açık borç.

## Çalıştırma
```bash
dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj
dotnet run --project Ens.Kernel.Demo/Ens.Kernel.Demo.csproj
```
