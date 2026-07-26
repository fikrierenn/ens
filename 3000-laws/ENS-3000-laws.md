---
id: ENS-3000
title: Enterprise Laws (Kayıt)
type: law
canon: false
constitutive: true
origin: ENS-0000 §III, §VII
depends_on: [ENS-0000]
referenced_by: [ENS-3021, ENS-3022, ENS-3023]
principles: [P1, P2, P3, P4, P5]
status: draft
owner: ens-philosopher
version: 0.1.1
last_reviewed: 2026-07-27
failure_conditions: stated
skeptic_review: pending
---

# Enterprise Laws

**Yetki:** [ENS Anayasası, Madde III & VII](../0000-constitution/ENS-0000-constitution.md)
**Doğası:** Örgütsel ölçekte karar davranışı hakkında **yanlışlanabilir** önermeler. Her
yasa, Anayasa Madde X uyarınca, yanlış olacağı koşulları taşımalıdır. Yasalar `provisional`
statüsünden `canonical`'a yalnızca skeptic incelemesinden sonra yükselir.

Her yasanın bir kimliği (`LAW-*`), bir ifadesi, türediği ilke, sezgisi, önerilen biçimsel
şekli ve failure conditions'ı vardır. Biçimsel modeller `3000-laws/` içinde ayrı
belgelerde geliştirilir; bu dosya kayıt defteridir.

---

## LAW-DECISION-GRAVITY — Decision Gravity Yasası
- **İfade:** Büyük kararlar daha çok context çeker.
- **Türediği ilke:** P1, P2.
- **Sezgi:** Sonuç, context'e olan rasyonel talebi ölçekler; yüksek bahisli kararlar daha
  fazla kanıt, paydaş ve geçmiş çeker.
- **Önerilen biçim:** `context_pull(d) ∝ stake(d)` — friction yoksa, bir kararın etrafında
  biriken context, bahsiyle birlikte monoton artar.
- **Yanlışlanır eğer:** zaman baskısı altında örgütler büyük kararları *daha az* context ile
  alırsa (panik kararları). Yasa gözlenen davranışı değil, rasyonel eğilimi tanımlar.

## LAW-ORG-MEMORY — Organizational Memory Yasası
- **İfade:** Unutulan kararlar tekrarlanan hatalara dönüşür.
- **Türediği ilke:** P3.
- **Sezgi:** *Neden*'in hatırlanması yoksa, örgüt yeniden türetir — ve yeniden yanılır.
- **Önerilen biçim:** beklenen tekrar-hata oranı, karar-belleği geri getirme olasılığı
  düştükçe artar.
- **Yanlışlanır eğer:** çevre o kadar hızlı değişiyorsa ki geçmiş kararlar sinyal
  taşımıyorsa; ölü bir context'in belleği, belleğin yokluğu kadar yanıltabilir. Belleğin
  değeri context durağanlığına (stationarity) koşulludur.

## LAW-CONTEXT — Context Yasası
- **İfade:** Karar kalitesi, context azaldıkça düşer.
- **Türediği ilke:** P2.
- **Sezgi:** Bir yeterlilik eşiğinin altında kalite keskin biçimde bozulur.
- **Önerilen biçim:** `quality(d) = f(context_score(d))`, `f' > 0`, altında kalitenin
  çöktüğü bir eşikle.
- **Yanlışlanır eğer:** aşırı *alakasız* context gürültü ekleyip kaliteyi *düşürürse*; yasa
  *alakalı* context ile ilgilidir ve bir relevance/attention sınırıyla eşlenmelidir (P5).

## LAW-ENTROPY — Decision Entropy Yasası
- **İfade:** Örgütler büyüdükçe karar tutarlılığı doğal olarak azalır.
- **Türediği ilke:** P1, P5.
- **Sezgi:** Daha çok karar verici, daha çok yüzey, daha çok sürüklenme — tutarlılığa doğru
  çeken aktif bir memory/standart kuvveti olmadıkça.
- **Önerilen biçim:** tutarlılık, headcount/decision-surface üzerinden, bir memory/learning
  kuvveti tarafından dengelenmedikçe bozulur.
- **Yanlışlanır eğer:** güçlü kültür ya da sıkı standardizasyon büyümeye rağmen tutarlılığı
  sabit tutarsa; entropy *varsayılandır*, kaçınılmaz değil — yasa gradyanı öngörür ve
  karşı kuvvet olarak memory/learning'i adlandırır.

## LAW-LEARNING — Learning Yasası
- **İfade:** Ölçülmeyen kararlar asla iyileşmez.
- **Türediği ilke:** P4.
- **Sezgi:** Sonucu niyetle karşılaştırmadan, üzerinde iyileşilecek bir gradyan yoktur.
- **Önerilen biçim:** sonuç ölçümü yoksa, karar hacmi ne olursa olsun iyileşme oranı sıfırdır.
- **Yanlışlanır eğer:** iyileşme, iç ölçüm yerine dışarıdan ithal bilgiyle gelirse; yasa,
  bir kararın kendi döngüsünden gelen *öz*-iyileşmeyi sınırlar.

---

> Bir yasa yalnızca bir RFC ile eklenir. Failure conditions'ını belirtmeyen yasa girmez.
