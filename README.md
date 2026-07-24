# Enterprise Nervous System (ENS)

**Redefining companies in the age of artificial intelligence.**

> Şirketler süreç işleyen sistemler değildir — **karar üreten bilişsel sistemlerdir**
> (decision-producing cognitive systems). ERP, veri parçalanmasını çözdü. ENS, **karar
> karmaşıklığını** çözer: kurumsal kararların üretimini, gerekçelendirilmesini, hatırlanmasını
> ve iyileştirilmesini, ölçekte.

ENS bir ürün değil — **yeni bir kurumsal bilişim disiplinidir**, TCP/IP'nin ağlara, DDD'nin
yazılım modeline yaptığını kararlara yapmayı hedefler. **AI-native Enterprise OS**: reasoning
kernel'dir, ERP yalnızca üzerine takılan bir Capability'dir — asla temel değil.

```mermaid
graph LR
  G[Goals] --> C[Cognition] --> D[Decisions] --> A[Actions] --> CAP[Capabilities] --> DA[Data]
```

Geleneksel yığın bu oku tersten okur (Business→Process→ERP→DB→Report→AI). ENS tersine çevirir:
karar önce gelir, veri ve sistemler onu *hizmet eder*.

## Kanıt — bu iddia sadece felsefe değil

```bash
cd 7000-reference-implementation
dotnet test Ens.Kernel.Tests/Ens.Kernel.Tests.csproj   # 54/54 geçer, gerçek çıktı
dotnet run --project Ens.Kernel.Demo/Ens.Kernel.Demo.csproj   # uçtan uca karar döngüsü
```

Teorinin (ENS-2001 Decision, ENS-2002 Context, ENS-2003 Memory, ENS-3021/22/23 Entropy/
Gravity/Capital) event-sourced, test edilmiş bir çalışan çekirdeği var — **iddia değil, kod.**
Detay: [`7000-reference-implementation/README.md`](7000-reference-implementation/README.md).

## Nasıl okunmalı
1. [Anayasa](0000-constitution/ENS-0000-constitution.md) — tek doğruluk kaynağı, değişmez ilkeler.
2. [KULLIYAT.md](KULLIYAT.md) — teknolojiden bağımsız çekirdek (teori, yasalar, ontoloji).
3. [REGISTRY.md](REGISTRY.md) — numaralandırma (ENS-3021, ADR-0001, RFC-6001 ...).
4. [ROADMAP.md](ROADMAP.md) — **açık işler, kalıcı kayıt.** Yeni oturum buradan devam eder;
   hiçbir görev yalnızca sohbet geçmişinde yaşamaz.
5. Yürütme kuralları: [`.claude/standards/`](.claude/standards/).

## Depo yapısı (ISO-tarzı numaralı)
```
0000-constitution/             Anayasa                                   (Külliyat)
1000-philosophy/                Manifesto, First Principles               (Külliyat)
2000-theory/                    Decision/Context/Memory/Learning teorisi  (onayla Külliyat)
3000-laws/                      Entropy · Gravity · Capital (fizik üçlüsü)(Külliyat)
4000-ontology/                  Meta Model, Enterprise Ontology, Sözlük   (Külliyat)
5000-architecture/               ADR'ler (Cognitive Kernel, Capability Pack)
6000-rfc/                       RFC'ler (Anayasa/standart değişiklikleri)
7000-reference-implementation/   Teoriyi kanıtlayan, gerçekten çalışan kod
8000-product/                   Mimariden beliren modüller                (henüz yok)
9000-book/                      Disiplini tanımlayan kitap                (henüz yok)
.claude/standards/              Standartlar (nasıl)
.claude/agents/                 Agent'lar (kim düşünür) — felsefeyi tüketir, üretmez
```

## Şu an nerede — dürüst durum

| Faz | İçerik | Durum |
|---|---|---|
| 0 · Felsefe | Anayasa (v0.3), Manifesto | **Ratified** — Anayasa bir kez fiilen değişti (RFC-6001, Madde XV tam prosedürü) |
| 1 · Teori | Decision/Context/Memory/Learning (ENS-2001–2004) | **Ratified (M3)** — her biri en az bir skeptic turundan sağ çıktı |
| 2 · Bilim | Entropy/Gravity/Capital (ENS-3021–23), Ontoloji (ENS-4000 ailesi) | **Ratified/M2–M3** — Computational Ontology katmanı hâlâ deneysel |
| 3 · Mimari | Cognitive Kernel (ADR-0001, Accepted), Operations Capability (ADR-0002) | ADR-0001 kabul edildi; ADR-0002 küçük açık talepler taşıyor |
| 4 · Reference Platform | `Ens.Kernel` — fizik üçlüsü + Context Score + Memory + Bounded-Autonomy Gate | **54/54 test geçiyor**, gerçek `dotnet run` demosu var |
| 5 · Ürün / 6 · Kitap | — | Henüz başlamadı |

Bu satır satır değil, canlı bir kayıt: [ROADMAP.md](ROADMAP.md)'de güncel tutulur.

## Yönetim ilkeleri
- **Constitution-centric.** Agent'lar değiştirilebilir; Anayasa değildir.
- **Teori önce (P8).** Implementation teoriyi kanıtlar; teori koddan türetilmez.
- **İzlenebilirlik zorunlu.** `kod → ADR → theory → first principle`. Öksüz düğüm = kusur.
- **Külliyat'ı kimse atlamaz (Madde IX).** Mimari, Külliyat'ta olmayan kavram uyduramaz.
- **Yanlışlanabilirlik.** Her kavram `ens-skeptic`'ten geçer; failure conditions belirtir.
- **Kimse kendi işini onaylamaz (G2/G3).** Validasyon, yazardan bağımsız bir çağrıda yapılır.
- **Canon kazanılır, ilan edilmez.** `constitutive` (kural/tanım) ratifikasyonla; ampirik teori
  Faz-4 kanıt zinciriyle (RFC-6001, Madde IV).
- **Dil:** dokümanlar Türkçe, teknik terimler orijinal ([policy](.claude/standards/language-policy.md)).

## Katkı disiplini
Yeni bir kavram önce Külliyat'a girer (`ens-philosopher` önerir, `ens-skeptic` saldırır),
sonra mimariye (`ens-architect`), sonra koda (`ens-backend-architect`, ADR'siz kod yok).
Anayasa değişikliği yalnızca Madde XV'in tam yordamıyla: RFC → skeptic → çift-owner kabul.
Roller: [`.claude/agents/ROSTER.md`](.claude/agents/ROSTER.md).
