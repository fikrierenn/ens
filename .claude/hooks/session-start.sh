#!/usr/bin/env bash
# SessionStart hook — ENS projesinde her oturum başında çalışır.
# Kaynak: D:\Dev\reporthub, D:\Dev\operax session-start.sh (prior art, adapte edildi).
# Çıktısı additionalContext olarak enjekte edilir.

set -e
REPO="${CLAUDE_PROJECT_DIR:-D:/Dev/ENS}"
cd "$REPO" 2>/dev/null || exit 0

echo "## ENS — Oturum Başı Özet"
echo ""

echo "### Son 3 gün commit'ler"
git log --since='3 days ago' --oneline 2>/dev/null | head -10
echo ""

echo "### Uncommitted dosya sayısı"
count=$(git status --porcelain 2>/dev/null | wc -l | tr -d ' ')
echo "$count dosya"
if [ "$count" -gt 15 ]; then
    echo ""
    echo "UYARI: 15 dosya eşiği aşıldı. Yeni iş başlamadan önce commit-split (.claude/standards/context-management.md İlke 4)."
fi
echo ""

echo "### ROADMAP.md — Sıradaki adım"
if [ -f ROADMAP.md ]; then
    awk '/^## Sıradaki adım|^## Blocking durumu/{p=1} p; /^---$/{if(p)exit}' ROADMAP.md | head -25
fi
echo ""

echo "### Açık kernel hattı satırları (K1-K5)"
if [ -f ROADMAP.md ]; then
    grep -E '^\| K[0-9]' ROADMAP.md 2>/dev/null
fi
echo ""

echo "### En son SKR/validation kaydı"
last_skr=$(find . -path ./.git -prune -o -path '*/reviews/SKR-*.md' -print 2>/dev/null | sort | tail -1)
if [ -n "$last_skr" ]; then
    echo "Dosya: $last_skr"
    grep -m1 "^## Verdict" -A1 "$last_skr" 2>/dev/null
fi
echo ""

echo "### En son journal girdisi"
last_journal=$(ls -t journal/*.md 2>/dev/null | head -1)
if [ -n "$last_journal" ]; then
    echo "Dosya: $last_journal"
    tail -25 "$last_journal"
else
    echo "(henüz journal yok)"
fi
echo ""

echo "### Kritik dosyalar"
echo "- Açık işler: ROADMAP.md (her oturum önce buradan devam)"
echo "- Numaralandırma: REGISTRY.md"
echo "- Bağlam disiplini: .claude/standards/context-management.md"
echo "- Governance: governance/000-governance-principles.md (G2: author kendi işini validate edemez)"

exit 0
