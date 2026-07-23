# Künye Standardı (Universal Traceability Header)

**Yetki:** [ENS Anayasası, Madde XI & VIII](../../0000-constitution/ENS-0000-constitution.md)

Depodaki **her** `.md` ve her kaynak modül (README'si üzerinden) bir YAML front-matter
başlığıyla başlar. Bu başlık bağımlılık grafiğini makine-okunur biçimde açığa çıkarır.

## Şema
```yaml
---
id:            ENS-3021          # REGISTRY.md'deki benzersiz kimlik (zorunlu)
title:         Decision Entropy   # Türkçe başlık; kanonik terim orijinal (zorunlu)
type:          theory             # aşağıdaki set (zorunlu)
canon:         true               # bool (zorunlu)
origin:        ENS-0000 §III       # bu yapıtı yetkilendiren üst kaynak (zorunlu)
depends_on:    [ENS-2010]          # yukarı akış kimlikleri (zorunlu; kök hariç boş olabilir)
referenced_by: [RFC-6042]          # alt akış kimlikleri (validator tutabilir; opsiyonel)
principles:    [P1, P4]            # Anayasa Madde III kimlikleri (Külliyat için zorunlu)
status:        draft               # aşağıdaki set (zorunlu)
owner:         ens-philosopher     # sorumlu rol (zorunlu)
version:       0.1.0               # semver (zorunlu)
last_reviewed: 2026-07-23          # ISO tarih (zorunlu)
# Faz 1-2 Külliyat kavramları için ek:
failure_conditions: stated         # 'stated' | 'pending' (Anayasa Madde X)
skeptic_review:     SKR-014         # veya 'pending'
maturity:           M3              # M0..M5 (maturity-model.md); canon:true ⇔ M5
evidence:           {sci: E3, eng: E0, ops: E0, econ: E0}   # 4 boyut × E0..E4 (evidence-standard.md)
---
```

## Değer kümeleri
- `type`: `constitution | philosophy | theory | law | ontology | adr | rfc | module | book | standard | command | agent`
- `status`: `draft | review | skeptic-challenged | ratified | superseded`
- `canon`: `true | false` (bkz. [KULLIYAT.md](../../KULLIYAT.md)) — **yalnızca `maturity: M5` ise true**
- `maturity`: `M0 | M1 | M2 | M3 | M4 | M5` (bkz. [maturity-model.md](maturity-model.md)); M4+ Faz 4 ister
- `evidence`: `{sci, eng, ops, econ}` her biri `E0..E4` (bkz. [evidence-standard.md](evidence-standard.md))

### Opsiyonel — mekanik bağımlılık (otomatik analiz için)
`depends_on` artifact-kenarıdır; aşağıdakiler *semantik-yetenek* düzeyidir (bir belge neyi
sağlar/tüketir), otomatik dependency analizi için:
```yaml
requires:    [ENS-4001, ENS-4010, ENS-4025]     # ihtiyaç duyulan yetenekler/registry'ler
provides:    [Relation Registry, Semantic Connectors]  # bu belgenin sunduğu yetenekler
consumed_by: [ENS-4031, Validation Generator]   # bunu tüketen alt-katmanlar
```

## Kurallar
1. `id`, `REGISTRY.md`'de kayıtlı olmalı; kayıtsız kimlik hatadır.
2. `depends_on`'daki her kimlik gerçek bir yapıta çözülmeli (kırık atıf = hata).
3. `principles` yalnızca `P1`..`P8` içerir.
4. Faz 1-2 Külliyat kavramlarında `failure_conditions: stated` zorunlu (Anayasa Madde X).
5. Anahtarlar İngilizce; düz yazı değerler (`title`, `origin` açıklaması) Türkçe
   ([language-policy](language-policy.md)).
6. Kaynak modüllerde başlık, modül `README.md`'sinde durur; kod dosyaları ayrıca
   `// TRACE: ADR-NNNN` satırı taşır ([coding-standards](coding-standards.md)).

## Doğrulama
`/validate-theory` bu başlıkları gezer; öksüz düğüm, kırık atıf, eksik `failure_conditions`
ve tanımsız terim kullanımını raporlar. Ayrıntı: [traceability.md](traceability.md).
