---
id:            STD-METADATA-HEADER
title:         Künye Standardı (Universal Traceability Header)
type:          standard
canon:         false
origin:        ENS-0000 §XI, §VIII
depends_on:    [ENS-0000]
principles:    []
status:        ratified
owner:         ens-style-guardian
version:       0.2.0
last_reviewed: 2026-07-24
amended_by:    RFC-6001
---

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
constitutive:  true               # bool — Külliyat yapıtları için zorunlu (kök varsayılan: false)
                                   #   true  = normatif/kurucu (tanımla yürürlükte): kural, tanım,
                                   #           tip/şema belgesi; canon'u ratifikasyonla kazanır
                                   #   false = ampirik teori/yasa: canon'u kanıt zinciriyle (M5) kazanır
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
maturity:           M3              # M0..M5 (maturity-model.md); yalnızca constitutive:false'ta canon-gate eder
evidence:           {sci: E3, eng: E0, ops: E0, econ: E0}   # 4 boyut × E0..E4 (evidence-standard.md)
# Yalnızca constitutive:true, hard-core (immutable-core) statüsü taşıyan yapıtta (§Kurallar-7):
immutable_core_sections: [Madde III]   # opsiyonel — yalnızca hard-core taşıyan yapıtta (RFC-6001 §7.4)
                                        # şu an yalnızca ENS-0000. Ampirik yanlışlamaya kapalı (Madde XV);
                                        # Madde X'i yalnızca program-düzeyinde karşılar.
---
```

## Değer kümeleri
- `type`: `constitution | philosophy | theory | law | ontology | adr | rfc | module | book | standard | command | agent | skeptic-review | ceo-review | style-signoff | audit-report | plan | scan-report`
  (son 6 değer inceleme/hiza/tarama katmanına aittir: `skeptic-review` = SKR-*, `ceo-review` =
  Madde XIV hiza incelemesi [CEO-*], `style-signoff` = çift-owner kabul kapısının
  `ens-style-guardian` yarısı, `audit-report`/`scan-report` = governance tarama raporları
  [SCAN-*], `plan` = çalışma planı [PLAN-*]. Bu değer öbeği SCAN-02 (2026-07-27) taramasında
  şemaya eklendi — 45+ dosya zaten fiilen bu değerleri kullanıyordu, şema geriden geliyordu.)
- `status`: `draft | review | skeptic-challenged | ratified | superseded | skeptic-cleared | accepted | final`
  (`skeptic-cleared` = skeptic incelemesi sağ çıktı ama ratifikasyon ayrı bir governance edimi
  olarak bekletiliyor [ör. ontoloji katmanı]; `accepted`/`final` **ADR/RFC/style-signoff'un kendi
  terminal-durum alt-kümesidir** — ADR'ler geleneksel `Proposed → Accepted → Deprecated →
  Superseded` durum makinesini, RFC'ler ve style-signoff'lar `draft → … → accepted`/`final`
  izler; bunlar genel `draft|review|…|ratified` ekseninin YERİNE değil, YANINDA kullanılan
  tür-özel bir alt-küme. Bu ayrım SCAN-02 taramasında netleştirildi: değerlerin kendisi hatalı
  değildi, şema onları belgelemiyordu — bu bir şema boşluğuydu, dosyalar suçlu değildi. Casing
  tektir: küçük harf `accepted` [ADR-0001/0002 ve RFC-6001 arasında tekleştirildi].)
- `constitutive`: `true | false` — yapıt normatif/kurucu mu (kural/tanım/tip-şeması, tanımla
  yürürlükte) yoksa yanlışlanabilir ampirik bir iddia mı taşıyor (RFC-6001 §4)
- `canon`: `true | false` (bkz. [KULLIYAT.md](../../KULLIYAT.md)) — kazanılır, ilan edilmez.
  `constitutive: false` (ampirik) yapıtta **yalnızca `maturity: M5` ise true**; `constitutive: true`
  (kurucu) yapıtta **ratifiye edilip skeptic tutarlılık incelemesinden sağ çıkınca** true —
  maturity/evidence eksenine tabi değil (RFC-6001 §7.2)
- `maturity`: `M0 | M1 | M2 | M3 | M4 | M5` (bkz. [maturity-model.md](maturity-model.md)); M4+ Faz 4
  ister. `constitutive: false` yapıtlar bu alanı **taşır**; `constitutive: true` yapıtlar taşıyabilir
  ama zorunlu değildir — taşıdığı yerde bu etiket canon'u gate etmez (olumsal/uygulama-izleme ekseni,
  RFC-6001 §7.3)
- `evidence`: `{sci, eng, ops, econ}` her biri `E0..E4` (bkz. [evidence-standard.md](evidence-standard.md))
- `immutable_core_sections`: opsiyonel, yalnızca hard-core taşıyan `constitutive:true` yapıtta
  (RFC-6001 §7.4) — bkz. §Kurallar-7

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
7. `constitutive: false` yapıtlar `maturity` ve `evidence` alanlarını **taşır** ve
   `canon: true ⇔ maturity: M5`. `constitutive: true` yapıtlar canon'unu **ratifikasyonla**
   (kurucu-tutarlılık skeptic incelemesi) kazanır; M5/Faz-4 kanıt zincirine **tabi değildir**.
   Bir `constitutive: true` yapıt `maturity` alanı **taşıyabilir ama zorunlu değildir** — taşıdığı
   yerde bu etiket **canon'unu gate etmez**. `failure_conditions` her iki türde de zorunludur
   (Anayasa Madde X — kurucu için tutarlılık/örneklenebilirlik kipinde). Sınıflamayı belirleyen
   turnusol testidir (Test A/B/C, RFC-6001 §4.2), `maturity` alanının varlığı/yokluğu değil —
   `maturity` **taşımayan** bir yapıt kesin `constitutive: true`'dur (contrapositive), ama
   `maturity` **taşıyan** bir yapıt otomatik `constitutive: false` sayılmaz.

## Doğrulama
`/validate-theory` bu başlıkları gezer; öksüz düğüm, kırık atıf, eksik `failure_conditions`
ve tanımsız terim kullanımını raporlar. Ayrıntı: [traceability.md](traceability.md).
