# Kanıt Standardı (Evidence Standard)

**Yetki:** [ENS Anayasası, Madde X — Yanlışlanabilirlik](../../0000-constitution/ENS-0000-constitution.md)
**Amaç:** Her iddianın kanıt seviyesini açıkça işaretlemek — **dört ayrı boyutta.** Bir
kavram bilimsel olarak güçlü ama ekonomik olarak değersiz olabilir; tek eksenli kanıt bunu
gizler.

## İki eksen: seviye × boyut

### Seviyeler (E0-E4)
| Seviye | Ad | Anlamı |
|--------|-----|--------|
| **E0** | Opinion | Gerekçeli görüş; destek yok |
| **E1** | Case Study | Tek vaka/örnek |
| **E2** | Multiple Cases | Birden çok vaka / quasi-experimental |
| **E3** | Academic / Peer Support | Hakemli literatür / dış onay |
| **E4** | Formal Proof / Empirical Validation | Matematiksel ispat ya da kontrollü ampirik sonuç |

### Boyutlar (4)
| Boyut | Sorar | E4 ne zaman |
|-------|-------|-------------|
| **Scientific** | Teorik olarak sağlam/yanlışlanabilir mi? | Formal ispat / literatür |
| **Engineering** | İnşa edilebilir mi, inşa edildi mi? | Reference platform (Faz 4) |
| **Operational** | Gerçek operasyonda işliyor mu? | Sahada ölçüldü (Faz 5) |
| **Economic** | Değer üretiyor mu? | Ölçülmüş ROI (Faz 5) |

Künye: `evidence: {sci: E3, eng: E0, ops: E0, econ: E0}` — çerçeve/delta ayrımı gerekiyorsa
`evidence: {sci_frame: E3, sci_delta: E1, ...}`.

## Kullanım
- Bir iddia, seviyesini aşan kesinlikle sunulamaz (E1 "kanıtlandı" diyemez).
- `ens-skeptic` her boyutu ayrı atar/denetler; abartılmış seviye kusurdur. (Faz-aktivasyonlu:
  Scientific skeptic sci boyutunu, Engineering skeptic eng'i, Business skeptic econ/ops'u atar.)

## ENS'in dürüst mevcut durumu
- **Scientific:** çerçeve **E3** (Beer, Simon, Kahneman, Howard, Walsh-Ungson…), delta **E0-E1**.
- **Engineering / Operational / Economic:** hepsi **E0** — çünkü reference platform (Faz 4) ve
  saha (Faz 5) yok.

**Sonuç:** ENS bugün *bilimsel çerçevesi E3, geri kalan üç boyutu E0* bir çerçevedir. Bu bir
zayıflık değil, dürüst bir olgunluk beyanıdır ve Faz 4-5'in görevini tanımlar: **eng/ops/econ
boyutlarını E0'dan yukarı taşımak.** Maturity M4 Reference tam da Engineering boyutunun E3'e
çıkması demektir.

## Metadata alanı
Künyeye `evidence: {sci, eng, ops, econ}` eklenir (bkz. metadata-header.md).
