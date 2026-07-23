# Enterprise Nervous System (ENS)

**Redefining Companies in the Age of Artificial Intelligence**

ENS bir ürün değil, bir **disiplin** ve onunla birlikte gelen bir **standarttır**. Tez:
şirketler süreç sistemleri değil, **decision-producing cognitive system**'lerdir (karar
üreten bilişsel sistemler). ERP veri parçalanmasını çözdü; ENS karar karmaşıklığını çözer.

> Bu depo bir standart gibi düzenlenmiştir. Tek doğruluk kaynağı
> [ENS Anayasası](0000-constitution/ENS-0000-constitution.md)'dır. Her yapıt,
> referans zinciriyle Anayasa'ya geri bağlanır.

## Nasıl okunmalı
1. Önce [Anayasa](0000-constitution/ENS-0000-constitution.md) — değişmez ilkeler ve kurallar.
2. Sonra [KULLIYAT.md](KULLIYAT.md) — değiştirilemez, teknolojiden bağımsız çekirdek.
3. Sonra [REGISTRY.md](REGISTRY.md) — numaralandırma (ENS-3021, RFC-6042 ...).
4. Yürütme kuralları: [`.claude/standards/`](.claude/standards/).

## Depo yapısı (ISO-tarzı numaralı)
```
0000-constitution/            Anayasa                         (Külliyat)
1000-philosophy/              Manifesto, First Principles     (Külliyat)
2000-theory/                  Teori kavramları                (onayla Külliyat)
3000-laws/                    Enterprise Laws                 (Külliyat)
4000-ontology/                Ontoloji, Sözlük, Anti-pattern  (Külliyat)
5000-architecture/            C4, ADR, Context Maps
6000-rfc/                     RFC'ler
7000-reference-implementation/ Teoriyi kanıtlayan referans kod
8000-product/                 Mimariden beliren modüller
9000-book/                    Disiplini tanımlayan kitap
.claude/standards/            Standartlar (nasıl)
.claude/commands/             Komutlar / iş akışları (ne)
.claude/agents/               Agent'lar (kim düşünür) — felsefeyi tüketir, üretmez
```

## Yönetim ilkeleri
- **Constitution-centric.** Agent'lar değiştirilebilir; Anayasa değildir.
- **Teori önce.** Implementation teoriyi kanıtlar; teori koddan türetilmez (P8).
- **İzlenebilirlik zorunlu.** `kod → ADR → theory → first principle`. Öksüz düğüm = defect.
- **Külliyat'ı kimse atlamaz.** Mimari, Külliyat'ta olmayan kavram uyduramaz (Madde IX).
- **Yanlışlanabilirlik.** Her kavram `ens-skeptic`'ten geçer; failure conditions belirtir.
- **Dil:** dokümanlar Türkçe, teknik terimler orijinal ([policy](.claude/standards/language-policy.md)).

## Durum
Faz 0 (Felsefe) sürüyor. Anayasa v0.2 onaylı; Külliyat ve numaralandırma iskeleti kuruldu.
Sıradaki: Manifesto (`1000-philosophy/`), teori kavramlarının ilk taslakları
(`2000-theory/`), agent fleet'i (yapı donunca).
