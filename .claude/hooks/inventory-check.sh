#!/usr/bin/env bash
# ENS — AUDIT envanteri doğrulama kapısı
#
# NEDEN VAR: DEFECT-REGISTER.md'nin ilk sürümü 68 DEFECT dedi; gerçek sayı 75'ti.
# Üç kesme hatası üst üste bindi ve hepsi aynı UTF-16 dosyayı vurdu. Elle sayım
# güvenilmezdir. Bu hook, sicildeki sayının testlerden makineyle üretilen sayıyla
# uyuşup uyuşmadığını mekanik olarak kontrol eder.
#
# ÇIKTI: uyuşuyorsa sessiz; uyuşmuyorsa uyarı basar. BLOKE ETMEZ — bilgilendirir.
# Gerekçe: sicil bilerek geride bırakılmış olabilir (fix turu sürüyordur). Sessiz
# sapma ile bilinçli sapmayı ayırt etmek insanın işidir; hook yalnız görünür kılar.

set -uo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
TESTS="$ROOT/7000-reference-implementation/Ens.Kernel.Tests"
REGISTER="$ROOT/7000-reference-implementation/DEFECT-REGISTER.md"

[ -d "$TESTS" ] || exit 0
[ -f "$REGISTER" ] || exit 0

# Kanonik envanter komutu. Her parçası bir hatadan doğdu:
#   grep -a        → SecurityTests.cs UTF-16; düz grep/rg onu "binary" sayıp 0 döner
#   tr -d '\000'   → UTF-16 null baytları
#   async Task     → W5d/W5e/W5f `public void` DEĞİL
#   head/tail YOK  → bir `head -40` 48 metotluk dosyanın son 8'ini yutmuştu
inventory() {
  local f
  for f in "$TESTS"/*.cs; do
    [ -e "$f" ] || continue
    grep -aoE "public (void|async Task) AUDIT_[A-Za-z0-9_]+" "$f" 2>/dev/null | tr -d '\000'
  done | sed -E 's/public (void|async Task) //' | sort -u
}

ALL="$(inventory)"
[ -n "$ALL" ] || exit 0

count() { printf '%s\n' "$ALL" | grep -c "^AUDIT_$1_" || true; }

D=$(count DEFECT); F=$(count FINDING); X=$(count FIXED); H=$(count HOLDS)

# Sicilin başlığındaki iddia edilen sayıları çek (ilk iki kalın sayı).
# `grep -a` ZORUNLU: sicil bir kez tek bir NUL baytı yüzünden `binary` sayıldı ve bu hook
# sayıyı okuyamayıp `DEFECT 7000` gibi anlamsız bir değer üretti — sessiz geçmedi ama
# YANLIŞ ALARM verdi. work-protocol.md §3.2: "yok" ile "okuyamadım" ayırt edilmeden
# "yok" yazılmaz. (2026-07-27, DEFECT-REGISTER §13.1)
CLAIMED="$(grep -a -m1 -oE '\*\*[0-9]+\*\* açık kusur' "$REGISTER" | grep -oE '[0-9]+' || true)"
CLAIMED_F="$(grep -a -m1 -oE '\*\*[0-9]+\*\* açık gözlem' "$REGISTER" | grep -oE '[0-9]+' || true)"

if [ -z "$CLAIMED" ]; then
  echo "⚠️  ENS envanter: DEFECT-REGISTER.md başlığından sayı okunamadı (biçim değişmiş olabilir)."
  echo "    Makine sayımı: DEFECT $D · FINDING $F · FIXED $X · HOLDS $H"
  exit 0
fi

if [ "$CLAIMED" != "$D" ] || { [ -n "$CLAIMED_F" ] && [ "$CLAIMED_F" != "$F" ]; }; then
  echo "❌ ENS envanter UYUŞMUYOR — DEFECT-REGISTER.md testlerle senkron değil"
  echo "   sicil : DEFECT ${CLAIMED} · FINDING ${CLAIMED_F:-?}"
  echo "   gerçek: DEFECT ${D} · FINDING ${F}   (FIXED ${X} · HOLDS ${H})"
  echo "   → sicili güncelle ya da neden bilerek geride olduğunu sicile yaz."
  exit 0
fi

# Sessiz başarı: gürültü yapma. Yalnız doğrulandığını belli et.
echo "✅ ENS envanter senkron: DEFECT $D · FINDING $F · FIXED $X · HOLDS $H"
