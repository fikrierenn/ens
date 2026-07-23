# Doküman Stili (Documentation Style)

**Yetki:** [ENS Anayasası, Madde XIII](../../0000-constitution/ENS-0000-constitution.md)

*Designing Data-Intensive Applications*, *Domain-Driven Design*, *Thinking in Systems* ve
IETF RFC'leri gibi yaz. Okur, fikirle ilk kez karşılaşan yetkin bir mühendis ya da
araştırmacıdır.

## Ses
- Akademik ama okunabilir. Kesin, ama katı değil.
- Özgün. Terminolojiyi gevşekçe ödünç almak yerine tanıt ve savun.
- Her iddia gerekçesini taşır. Gerekçesiz iddia reddedilir.
- Pazarlama dili yok. Buzzword yok. Yanlışlanamaz iddia yok. Abartı sıfatı yok.

## Bir teori kavramı belgesinin yapısı
İzlenebilirlik header'ı, sonra: **Definition · Motivation · Historical context ·
Theoretical model · Implications · Relationships · Examples · Laws · Failure conditions.**
Failure conditions bölümü Faz 1-2'de zorunludur (Anayasa Madde X).

## Bir RFC / ADR'nin yapısı
`.claude/standards/` şablonlarını ve `/create-rfc`, `/review-architecture` komutlarını izle.
Her zaman Anayasa'ya ve hizmet edilen yukarı akış kimliklerine atıf yap.

## Mekanik
- Diyagramlar: Mermaid, satır içi.
- Çapraz atıflar: atıf yapılan yapıta göreli link.
- Kaynaklar: bulunabilir gerçek kaynaklar; literatürü `ens-researcher` sağlar. **Asla
  kaynak uydurma** — doğrulanamayan atıf bir kusurdur.
- Kavram başına tek kanonik terim; sessiz takma ad yok (Anayasa Madde IV).
- **Dil:** dokümanlar Türkçe, teknik terimler orijinal ([language-policy](language-policy.md)).

## Yasak
Uydurma kaynaklar · adil/atıflı kısa alıntı ötesinde kopyalanmış telifli metin · terim
sürüklenmesi · `ens-skeptic`'e saldırma fırsatı verilmemiş iddialar.
