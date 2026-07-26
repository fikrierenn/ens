# ens-ontology-linter (formal-checker V1)

ENS Külliyatı için **deterministik Ontology Linter**. Freeze-fix backlog'undaki **G-09/10**
maddesini kapatır ("Validation Generator + Ontology Linter (`formal-checker`) hiç yazılmadı").

> **Dil notu (2026-07-27):** bu belge önceden tamamen İngilizceydi ve SCAN-02'de dil
> politikasının en büyük tek ihlali olarak işaretlendi. Türkçeye çevrildi; teknik terimler
> (invariant, profile, range, domain, fixture, exit code…) politika gereği **orijinal**
> bırakıldı.

## Neden bir araç, LLM ajanı değil (tasarım kararı — ens-architect)

Bu yıl elle bulunan her gerçek ontoloji kusuru — Kusur 1/2/3 (ENS-4010), D-1 (ENS-4025),
Yara A/B (ENS-4010 v0.5.0), B1 (ENS-4031) — **aynı iki mekanik invariant'ın** ihlaliydi ve
her biri pahalı, çok turlu (kimi zaman 3 turluk) skeptic-ajan döngüleriyle bulundu. Oysa bu
denetimler iki markdown tablosu üzerinde saf yapısal küme-üyeliği testleridir. Değerleri tam
olarak **ucuz, tekrarlanabilir ve regresyon-önleyici** olmalarından gelir — bir LLM ajanının
iyi olduğu şeyin tersi. Bu yüzden `formal-checker` V1 bir ajan değil, deterministik bir .NET
konsol aracıdır.

## Neden `tools/` altında, `7000-reference-implementation/` altında değil

`7000-reference-implementation/`, Anayasa Madde VII gereği **teoriyi kanıtlayan referans
koddur** (`ContextScore` = ENS-2002'nin formülü, `CompanyMemory` = ENS-2003'ün sönümü…).
Bu linter **hiçbir teoriyi kanıtlamaz** — *korpus tutarlılığını* denetler: ENS-4010
markdown'ını okur ve iç iyi-biçimliliğini kontrol eder. Kernel'in içine koymak bir
concern-leak olurdu (linter korpusun **üzerinde** çalışır; korpusun kanıtının parçası
değildir). Bu yüzden ayrı bir üst-düzey `tools/` dizininde yaşar. `coding-standards.md`
gereği **.NET/C# yığınını korur** — `7000-` ile toolchain tutarlılığı için.

## Ne denetler (V1 kapsamı — tam olarak iki invariant)

İkisi de ENS-4010 §Relation Registry'den **birebir** alınmıştır; orada `formal-checker`'ın
iki invariant'ı olarak zaten yazılıdırlar:

1. **Profile satisfiability** — her Semantic Profile'ın gerektirdiği ilişkisel bağ, Relation
   Registry'de o profili taşıyan node tipini kabul eden bir domain/range ile **gerçekten**
   var olmalıdır. Profili, asla kuramayacağı bir bağı gerektiren bir node, uyumlu bir instance
   üretemez. (Kusur 1/2, Yara A-1'i yakalar.) Daha güçlü bir alt-vaka: **hiç kayıtlı olmayan**
   bir relation'ı gerektiren profil (Yara A-2, `derived_from` vakası) ayrıca
   `UnregisteredRelationReference` olarak raporlanır.
2. **Transitivity well-formedness** — `Trans: ✓` işaretli her relation için `range ⊆ domain`
   olmalıdır (aksi hâlde `A→B ∧ B→C` iki-adımlı zinciri kurulamaz). (Kusur 3'ü yakalar; not:
   D-1 *farklı* bir biçimdeydi — node/relation Registry'de tümüyle yoktu, onu da
   `UnregisteredRelationReference` varyantı kapsar.)

Linter her ikisini de **ham Node/Relation Registry tablolarından bağımsız olarak yeniden
türetir.** Belgenin kendi elle yazılmış "Invariant denetim tablosu"nu **okumaz** — o tablo
insanın vardığı sonuçları (`Sonuç: ✓`) gömer; ona güvenmek tautoloji olurdu. Amaç doğrulamak,
cevabı yeniden okumak değil.

## Nasıl çalıştırılır

.NET 10 SDK gerekir.

```sh
# Gerçek korpusu denetle (varsayılan hedef = 4000-ontology/ENS-4010-foundational-ontology.md)
dotnet run --project tools/ens-ontology-linter/src/Ens.OntologyLinter

# Belirli bir dosyayı denetle
dotnet run --project tools/ens-ontology-linter/src/Ens.OntologyLinter -- yol/ENS-4010.md

# Tüm test paketini çalıştır (unit + positive control + negative control)
dotnet test tools/ens-ontology-linter/Ens.OntologyLinter.slnx
```

Exit code'lar: `0` = sıfır ihlal, `1` = ihlal bulundu (CI/regresyon kapısına uygun),
`2` = araç/parse hatası.

## Kanıt — positive ve negative control

- **Positive control** (`ControlTests.PositiveControl_RealCorpus_HasZeroViolations`): mevcut
  temiz ENS-4010'a karşı çalışır ve **sıfır ihlal** olduğunu doğrular (ayrıca parser'ın
  gerçekten ≥20 relation / ≥15 node / ≥8 profil / tam 2 `Trans:✓` relation okuduğunu sınayan
  bir sanity-check ile — böylece boş bir parse "temiz" gibi görünemez). Bu tur bilinen tüm
  kusurları zaten düzeltmişti (SKR-038/039/031/032 survive), dolayısıyla sıfır olmayan bir
  sonuç ya aracın ya korpusun bozuk olduğu anlamına gelirdi — ikisi de araştırmaya değer.
- **Negative control** (`ControlTests.NegativeControl_BrokenFixture_CatchesAllThreeDefectClasses`):
  `tests/.../fixtures/broken-ontology.md` üzerinde çalışır — üç tarihsel kusur sınıfını
  bilerek geri getiren küçük, sentetik bir ontoloji (gerçek korpusa dokunulmaz) — ve linter'ın
  **tam olarak** onları yakaladığını doğrular:
  1. `part_of: Actor → Organization` + `Trans:✓` → TransitivityWellFormedness (Kusur 3 / D-1).
  2. `Claim`, `supports|invalidates`'i sağlayamaz → ProfileSatisfiability (Kusur 1).
  3. `Rule`, Registry'de olmayan `derived_from`'u gerektirir → UnregisteredRelationReference (Yara A-2).

> ### ✅ Çalıştırma dürüstlüğü — güncellendi (2026-07-27)
> **Önceki not artık geçerli değildir ve düzeltilmiştir.** Eski metin şunu diyordu: araç ve
> testler elle iz sürülerek doğrulandı ama *"canlı bir `dotnet build` / `dotnet test`
> ÇALIŞTIRILMADI"* (üreten ajan bağlamında shell yoktu), ve yeşil koşunun sahip/CI tarafından
> teyit edilmesi gerekiyordu.
>
> **Teyit edildi.** 2026-07-27'de oturum sahibi tarafından gerçek korpusa karşı çalıştırıldı:
> ```
> Parsed: 17 node types, 21 relations, 9 profiles.
> Trans:✓ relations: part_of, specializes
> RESULT: 0 violations. Corpus is consistent under invariants A + B.
> ```
> Hiçbir build/test çıktısı uydurulmamıştır — ne o zaman, ne şimdi (SKR-001 / SKR-041 emsali).

## Failure conditions / dürüst sınırlar (V1)

- **Markdown-tablo parse'ı kırılgandır.** Parser tabloları bölüm başlığıyla çapalar ve mevcut
  ENS-4010 kolon düzenini varsayar. Bir başlık yeniden adlandırılırsa, bir kolon yer
  değiştirirse ya da tablo yeniden biçimlenirse linter **sessizce yanlış veriyi okuyup yanlış
  bir "temiz" raporlayabilir.** Tek en büyük risk budur. İleri bir sağlamlaştırma adımı,
  sabit ve makine-okunur bir registry export'udur.
- **Profil gereksinimleri düzyazıdan çıkarılır.** Yalnız backtick içindeki
  `snake_case`/`hyphen-case` token'lar relation adayı sayılır (böylece düz Decision-Object
  sözcükleri — Evidence, Context, Alternatives, Outcome — doğru biçimde yok sayılır, SKR-039
  keskinleştirme #1 gereği). Küçük bir stop-list (`is_root`, `identity`, `lifecycle`, `decay`,
  `scarcity`, `timestamp`, `formula`) yapısal yüklemleri dışlar. Stop-list güncellenmeden
  backtick'li yeni bir relation-olmayan yüklem eklemek, yanlış bir
  `UnregisteredRelationReference` üretir.
- **Profile satisfiability "node başına en az bir sağlanabilir zorunlu bağ" kullanır** —
  korpusun kendi "en az bir … bağ" profil biçimine uyar. Saf **konjonktif** gereksinimler
  (ör. Deliberative = `serves` VE `constrained_by`) bu yüzden **eksik denetlenir**: bir
  konjunktteki kusur, başka bir konjunkt sağlanabiliyorsa kaçırılır. Tam
  konjonksiyon/disjonksiyon parse'ına sıkılaştırma **V2**'ye ertelendi.
- **Yalnız iki invariant.** **V2**'ye ertelenenler: `depends_on`↔`referenced_by` geri-bağlantı
  hijyeni (**G-18**), node/edge tamlığı, cardinality, identity ve Semantic Closure
  erişilebilirliği — yani `validation-framework.md`'nin Ontology-boyutu checklist'inin geri
  kalanı. `≥2 kök → uyarı` sezgiseli (ENS-4010 §Kök operasyonelleştirmesi) da V2'dedir.
