---
id:            STYLE-SIGNOFF-RFC-6001
title:         RFC-6001 Şema/Tutarlılık İmzası (ens-style-guardian, §7.5 çift-owner kapısı)
type:          style-signoff
canon:         false
origin:        RFC-6001 §7.5
depends_on:    [RFC-6001, STD-METADATA-HEADER, STD-TRACEABILITY, STD-LANGUAGE-POLICY, STD-DOCUMENTATION-STYLE, ENS-4000]
referenced_by: []
principles:    [P8]
status:        final
owner:         ens-style-guardian
version:       0.1.0
last_reviewed: 2026-07-24
---

# STYLE-SIGNOFF-RFC-6001 — Şema/Tutarlılık İmzası

**Yetki:** RFC-6001 §7.5 (çift-owner kabul kapısı: `ens-ceo` hiza **ve** `ens-style-guardian`
şema-imzası). Bu not yalnızca **ikinci** imzadır — biçim/şema/tutarlılık kapsamında. Anlam/hiza
kararı `ens-ceo`'nun paralel incelemesine aittir; bu belge ona dokunmaz.

## Kapsam ve yöntem

İncelenenler: `RFC-6001-constitutive-artifact-ayrimi.md` (v0.3.0, tam metin — §1-15), mevcut
`STD-METADATA-HEADER` (v0.1.0), `STD-TRACEABILITY`, `STD-DOCUMENTATION-STYLE`,
`STD-LANGUAGE-POLICY`, `REGISTRY.md`, `ENS-4000-glossary.md` (`constitutive` girdisi, G-11
kapanışı), `KULLIYAT.md` (gap #1). Ayrıca mekanik doğrulama: `grep ^maturity:` tüm depoda,
GOV-* id↔dosya eşlemesi, RFC'nin `depends_on` zincirinin REGISTRY'ye çözülmesi.

## Bulgular

1. **Alan adı çakışması yok.** Önerilen `constitutive: true|false` (§7.1) ve opsiyonel
   `immutable_core_sections` (§7.4), mevcut künye şemasındaki hiçbir alanla (`id, title, type,
   canon, origin, depends_on, referenced_by, principles, status, owner, version, last_reviewed,
   failure_conditions, skeptic_review, maturity, evidence, requires, provides, consumed_by`) ad
   çakışması yaratmıyor.
2. **YAML tipi tutarlı.** `constitutive: bool`, mevcut `canon: bool` konvansiyonuyla birebir aynı
   biçimde tipleniyor; `true|false` değer kümesi şemanın geri kalanıyla (ör. `canon`) tutarlı.
   `immutable_core_sections` bir string-listesi (`[Madde III]`) — mevcut `depends_on`/`principles`
   liste biçimiyle tutarlı.
3. **Zorunlu/opsiyonel sıralama.** `constitutive` "Külliyat yapıtları için zorunlu" olarak
   önerilmiş; bu, `canon`/`principles` gibi diğer Külliyat-zorunlu alanlarla aynı statüde —
   tutarlı. `immutable_core_sections` açıkça opsiyonel ve tek-locus (ENS-0000) ile sınırlı
   tutulmuş; scope-creep'ten kaçınma gerekçesi (§4.1 sonu) mevcut şemanın minimalist tarzına
   uyuyor. **Küçük gözlem (bloke etmeyen):** §7.1, alanın künye bloğundaki tam **konumunu**
   (örn. `canon`'dan hemen önce/sonra) belirtmiyor; bu, retrofit adımında (§10) netleştirilmeli —
   RFC'nin şu anki metnini değiştirmeyi gerektirmez, yalnızca sıradaki uygulama adımı için not.
4. **Dokümantasyon stili ve dil politikası.** Düz yazı Türkçe, teknik/felsefi terimler
   (`constitutive`, `hard core`, `protective belt`, `Grundnorm`, `rule of recognition`) orijinal
   biçimde — `language-policy.md` ile tutarlı (precedent: felsefi/teknik terimler için aynı
   muamele zaten Külliyat'ta var). RFC yapısı `documentation-style.md`'nin "Bir RFC / ADR'nin
   yapısı" ölçütüne (Anayasa'ya ve yukarı akış kimliklerine atıf) uyuyor; 9-bölüm teori şablonu
   (`Definition·Motivation·...·Failure conditions`) yalnızca `2000-theory`/`3000-laws` kavram
   belgeleri için zorunludur, RFC'ye uygulanmaz — kapsam doğru.
5. **Numaralandırma ve REGISTRY.** `RFC-6001`, `6000-6999` (RFC) aralığında doğru numaralandırılmış
   ve `REGISTRY.md`'de kayıtlı (satır 32); numara tekrarı yok.
6. **İzlenebilirlik — `depends_on` çözünürlüğü (traceability.md kural-1/2).** RFC-6001'in
   `depends_on: [ENS-0000, ENS-4000, STD-METADATA-HEADER, STD-MATURITY-MODEL]` listesindeki
   dört id de REGISTRY'de kayıtlı ve gerçek yapıtlara çözülüyor; öksüz düğüm yok.
7. **§12 korpus taraması — mekanik doğrulama (bağımsız tekrar).** `grep ^maturity:` sonucu RFC'nin
   iddiasıyla birebir örtüşüyor:
   - `ENS-2001/2002/2003/2004`, `ENS-3021/3022/3023` → hepsi `maturity: M3` ✓
   - `ENS-4001/4010/4020/4025/4030` → hepsi `maturity: M2` ✓
   - `ENS-4031` → `maturity: M0` ✓
   - `GOV-000/010/020/030` → hepsi `maturity: M1` ✓; dosya↔id eşlemesi doğrulandı
     (`governance/000-governance-principles.md`=GOV-000, `roles.md`=GOV-010,
     `capability-matrix.md`=GOV-020, `canonical-process.md`=GOV-030) — RFC'nin §12 dipnotuyla
     birebir uyuşuyor.
   - `ENS-0000`, `ENS-4000` → `maturity` alanı yok ✓ (RFC'nin "contrapositive: M yok ⇒ true"
     iddiasıyla tutarlı).
8. **Terminoloji — ENS-4000-glossary.md çelişkisi yok.** Sözlüğün G-11 kapanışı (`ENS-4000-glossary.md`
   satır 34-39) `constitutive` ile `maturity: M0..M5`'i zaten **iki bağımsız eksen** olarak
   tanımlıyor ve `KULLIYAT.md`'nin constitutive/canonical ayrımına atıf yapıyor. RFC-6001'in
   tanımı (§4) bu girdiyle **çelişmiyor**, onu üst-kaynağa (Anayasa + şema) terfi ettiriyor — RFC'nin
   kendi iddiasıyla (§1, §3 "Külliyat-içi prior art") tutarlı. `KULLIYAT.md`'nin "gap #1" notu
   (satır 8, 17) da RFC'nin kapatmayı önerdiği tam borçla eşleşiyor.
9. **`skeptic_review` çoğul-liste biçimi.** RFC-6001 `skeptic_review: [SKR-034, SKR-035, SKR-036]`
   kullanıyor; bu, mevcut şablonun tekil örneğinden (`SKR-014`) farklı görünse de, depoda zaten
   yerleşik bir precedent (`ENS-4020`, `ENS-4031`, `ENS-4001`, `ENS-4010`, `ADR-0001` hepsi liste
   biçimini kullanıyor çok turlu inceleme için) — tutarsızlık değil.
10. **Küçük, bloke-etmeyen gözlem — `amends:` alanı.** RFC-6001 künyesinde `amends: [ENS-0000 §IV,
    STD-METADATA-HEADER]` kullanılıyor; bu alan `metadata-header.md`'nin temel şema bloğunda
    **belgelenmemiş** (yalnızca `requires/provides/consumed_by` opsiyonel-mekanik alanlar olarak
    listeli). Ancak bu, mevcut ADR şablonunun da tip-özel bir alan (`realizes:`) kullanıp temel
    şemada belgelenmemesiyle aynı, zaten var olan bir örüntü (`traceability.md` §Atıf). Bu yüzden
    bir şema-ihlali **değil**, ileride `STD-METADATA-HEADER` güncellenirken (RFC `Accepted`
    sonrası, §10) tip-özel alan olarak resmîleştirilmesi gereken küçük bir belgeleme borcu. RFC-6001
    metnine dokunmadım — çünkü `amends` listesinin **içeriği** (STD-MATURITY-MODEL eklenmeli mi)
    SKR-036'nın kendi S2 keskinleştirmesi olarak zaten açık ve owner'ın (ens-philosopher) /
    `ens-ceo` hiza turunun kapsamında; bu benim biçim yetkimin dışında bir içerik kararı.
11. **Dosya/dizin adlandırma.** `6000-rfc/RFC-6001-constitutive-artifact-ayrimi.md` ve
    `6000-rfc/reviews/SKR-03{4,5,6}-rfc-6001-constitutive*.md` adlandırma şemasına (id + kebab-case
    Türkçe/İngilizce slug) uyuyor.

## Düzeltilen (bu ziyarette)

Yok. RFC-6001 üç bağımsız skeptic turundan geçmiş bir metin olarak biçimsel/mekanik açıdan zaten
temizdi; küçük bloke-etmeyen gözlemler (madde 3, 10) yukarıda not edildi, RFC gövdesine dokunulmadı.

## İmza kararı

**Şema/biçim açısından itirazım yoktur.** `constitutive` ve `immutable_core_sections` alan
önerileri mevcut künye şemasıyla adlandırma, tip ve zorunluluk-sırası bakımından tutarlı;
`constitutive` terimi Sözlük'teki (ENS-4000, G-11) mevcut kullanımla çelişmiyor; §12 korpus
taraması gerçek dosya/`maturity` değerleriyle mekanik olarak doğrulandı; terminoloji ve dil
politikası ihlali yok. Bu, RFC-6001'in **kabul edildiği** (`Accepted`) anlamına gelmez — yalnızca
§7.5'in ikinci imzasıdır. `Accepted` edimi hâlâ `ens-ceo` Madde XIV hiza incelemesinin
tamamlanmasını **da** gerektirir (çift-owner kapısı, §7.5).
