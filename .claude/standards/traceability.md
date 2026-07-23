# İzlenebilirlik Standardı (Traceability)

**Yetki:** [ENS Anayasası, Madde VIII — İzlenebilirlik Yasası](../../0000-constitution/ENS-0000-constitution.md)

ENS'te hiçbir şey, ilkelere doğru bir referans kenarı olmadan var olamaz. Bu standart o
yasayı mekanik hale getirir.

## Zincir
```
kod / test   ──atıf──▶  ADR  ──atıf──▶  theory / law / ontology  ──atıf──▶  first principle
 (Faz 4-5)             (Faz 3)          (Külliyat: Faz 1-2)                  (Anayasa Madde III)
```

Her düğüm **yukarı akışa** işaret eder. Yukarı akış kenarı olmayan bir düğüm — **öksüz düğüm** —
kalitesinden bağımsız olarak bir kusurdur.

## Atıf nasıl yapılır
Her yapıt bir künye taşır ([metadata-header.md](metadata-header.md)). Makine
de okur, insan da.

**Teori/Bilim kavramı** (`2000-theory/*.md`, `3000-laws/*.md`):
```yaml
id: ENS-3021
type: theory
principles: [P1, P4]
depends_on: [ENS-2001]          # Decision Capital
failure_conditions: stated       # Faz 1-2'de zorunlu (Anayasa Madde X)
skeptic_review: SKR-014
canon: true
```

**Mimari kararı** (`5000-architecture/adr/*.md`):
```yaml
id: ADR-0007
type: adr
realizes: [ENS-2021, ENS-2005]   # Company Memory, Context
depends_on: [ENS-2021]
principles: [P2, P3]
```

**Implementation** (kaynak dosyalar, modül README'leri):
```
// TRACE: ADR-0007, ADR-0012
```
veya modül `README.md`:
```yaml
id: MOD-COMPANY-MEMORY
type: module
depends_on: [ADR-0007, ADR-0012]
```

## Kimlik şeması
Ayrıntı için [REGISTRY.md](../../REGISTRY.md): `P1`..`P8` · `ENS-Nxxx` · `LAW-*` · `ADR-NNNN`
· `RFC-6xxx` · `MOD-*` · `SKR-NNN`.

## Doğrulama (`/validate-theory`)
Her yapıtın header'ını gezer ve şu durumlarda **başarısız** olur:
1. Herhangi bir öksüz düğüm (eksik yukarı akış kenarı).
2. Var olmayan bir kimliğe işaret eden atıf.
3. `failure_conditions: stated` olmayan bir Faz 1-2 kavramı.
4. Sözlükte tanımlı olmayan kanonik bir terimin kullanımı.
5. Külliyat'ta olmayan bir kavramı tanıtan mimari/kod (Anayasa Madde IX).

Ayrıca kapsam raporu üretir: hangi ilke, hangi ADR'ler ve modüller tarafından
gerçekleştiriliyor. Doğrulama başarısızken hiçbir faz kapısı açılmaz.
