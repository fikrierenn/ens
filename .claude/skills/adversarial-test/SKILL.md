---
name: adversarial-test
description: ENS Faz-4 kodunu düşmanca test eder — kendi yazdığı koda güvenmeyen, iddiaları kırmaya çalışan bağımsız test turu. Kullanıcı "test yaz", "bunu kır", "gerçekten çalışıyor mu", "manipüle etmediğini nereden bileyim", "/adversarial-test" dediğinde ya da yeni bir kernel bileşeni kodlandığında devreye gir.
allowed-tools: Read, Write, Edit, Bash, Grep, Glob
user-invocable: true
model: inherit
---

# ENS Düşmanca Test Skill'i

> **Doğuş nedeni (2026-07-25):** Kullanıcı, koordinatörün kendi yazdığı demo+testlere
> güvenmedi — *"test verisi yazıyorsun, manipüle etmediğini nereden bileceğim"*. Bağımsız bir
> denetim (`7000-reference-implementation/AUDIT.md`) haklı çıktı: manipülasyon yoktu ama
> **4 "yapısal olarak imkânsız" iddiadan 3'ü kırıldı** ve kimsenin aramadığı gerçek bir
> güvenlik açığı (P7 gate'inin NaN altında fail-open olması) bulundu.
>
> Bu skill, o denetimde işe yarayan saldırı desenlerini **tekrarlanabilir** kılar. Governance
> GOV-000 G2'nin ("yazar kendi işini Canonical yapamaz") koda uygulanmış hâlidir.

## Temel kural

**Testi yazan, kodu yazandan BAŞKA bir context olmalı.** Aynı turda hem kodlayıp hem "test
ettim" demek, kendi sınavını yazıp kendini geçmektir. Bu skill çağrıldığında yeni, taze bir
`Agent` çağrısı yap — mevcut context'te inline test yazma.

## Refleks soru

*"Bu test, iddiayı DOĞRULAMAK için mi yazıldı, yoksa KIRMAK için mi? Girdiler sonuç güzel
görünsün diye mi seçildi?"*

Bir test suite'i yalnızca mutlu yolu geçiyorsa, hiçbir şey kanıtlamaz.

## Saldırı yüzeyleri (kanıtlanmış — hepsi gerçek kusur buldu)

### 1. Sayısal fail-open (en verimli)
IEEE-754'te `NaN` her karşılaştırmada `false` döner → `is < 0 or > 1` deseni NaN'ı **görmez**.
Güvenlik-kritik bir eşik kontrolü NaN altında en permisif dala düşer.
- Her `double` parametreye: `NaN`, `+∞`, `−∞`, `double.MaxValue`, `-0.0`, aralık dışı (`5.0`
  bir `[0,1]` alanına), `double.Epsilon`.
- **Sor:** ölçülemez bir girdi otonomi mi kazanıyor, yoksa reddediliyor mu? (Fail-closed olmalı.)

### 2. Downcast mutasyonu — "değiştirilemez" yalanı
`IReadOnlyList<T>` döndürmek **korumaz**; arkadaki `List<T>` cast edilebilir:
```csharp
((List<T>)obj.ReadOnlyProperty).Clear();     // reflection GEREKMİYOR
((HashSet<string>)pack.AllowedTools).Add("delete_database");  // yetki kaçağı
```
- Her salt-okunur property için dene. Savunmacı kopya var mı, yoksa canlı referans mı?

### 3. Yan kapı / replay yolu
Yazma yolunda (`Commit`) korunan invariant, okuma/replay yolunda (`Rehydrate`, deserialize,
factory) korunmayabilir. Event-sourced sistemde **replay birincil yoldur.**
- Sahte event akışıyla nesne kur: invariant'ları atlıyor mu?
- Public constructor/record ile ara-nesne forge et (`new GateResult(Autonomous, "sahte", 0)`).

### 4. Zaman manipülasyonu
`Verify(id, gelecekTarih)`, geriye giden saat, `max(0, negatif)` sessiz kırpması.
- Bir decay/TTL mekanizması gelecek tarihle sonsuza dek devre dışı bırakılabiliyor mu?

### 5. Guard'ın kozmetik olması
Hata mesajı bir şey iddia ediyor ama kod onu zorlamıyor (`minSupportingRecords: 1` kabul
edilirken mesaj "tek gözlemden sistematik iddia edilemez" diyor).
- Her guard mesajını oku, kodun gerçekten onu zorladığını doğrula.

### 6. Cherry-picked demo/senaryo
- Girdileri değiştir: iddia hâlâ geçerli mi? (Curator "0 kayıt bayrakladı" — 400 gün seçilmiş,
  462 olsa bayraklanacaktı.)
- Bir çarpanı **sıfırla**: sonuç değişiyor mu? Değişmiyorsa o çarpan hiçbir şey kanıtlamıyor.

### 7. Formül sadakati (fuzz)
100-500 rastgele girdiyle çalıştır, beklenen değeri **elle/bağımsız hesapla** — koddan çağırıp
kendisiyle karşılaştırma (tautoloji).

### 8. Overload/isim tuzakları
Pozisyonel çağrı yanlış overload'a bağlanabilir (C# "better conversion target"). Testler
adlandırılmış argüman kullanırsa bu hatayı **asla** yakalayamaz — demo yakalar.
- Unicode homoglif, zero-width, NUL, case varyantı ile string-tabanlı yetki kontrolünü dene.

## Test adlandırma kuralı

| Önek | Anlamı |
|---|---|
| `AUDIT_DEFECT_*` | **Geçerse kusur VAR demektir.** Kusuru kanıtlar. |
| `AUDIT_HOLDS_*` | İddianın gerçekten sağlam olduğunu kanıtlar. |
| `AUDIT_FIXED_*` | Eskiden `DEFECT` idi, düzeltildi — artık kapanışı kanıtlar. |

Kusur düzeltilince `DEFECT` testini **silme**, ters çevirip `FIXED` yap. Böylece regresyon
korunur ve tarih kaybolmaz.

## Çıktı

1. `Ens.Kernel.Tests/AdversarialAuditTests.cs` (ya da konuya özel yeni dosya) — düşmanca testler.
2. `7000-reference-implementation/AUDIT.md` — bağımsız denetim raporu, koordinatörün
   README'sinden **ayrı** (farklı ses, çelişebilir).
3. **`dotnet test` GERÇEKTEN çalıştırılmış çıktı.** Çalıştıramadıysan bunu açıkça yaz,
   tek satır fabrike etme (SKR-041 emsali).

## Dürüstlük zorunlulukları

- Sağlam çıkanları da yaz — yalnızca kusur listelemek de bir tür manipülasyondur.
- Kendi denetiminin sınırını belirt (neyi test etmedin, hangi araca erişemedin).
- Bulguyu "kanıtlayan test hangisi" diye yaz — okuyucu senin raporunu da kırabilmeli.
