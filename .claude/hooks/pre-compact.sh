#!/usr/bin/env bash
# PreCompact hook — /compact öncesi açık iş + son commit'ler journal'a snapshot'lanır.
# Kaynak: D:\Dev\operax .claude\hooks\pre-compact.sh (prior art, adapte edildi).
# Tetik: settings.json PreCompact eventi.

set -e
cd "$(git rev-parse --show-toplevel 2>/dev/null || echo "$PWD")"

today=$(date +%Y-%m-%d)
journal="journal/$today.md"
mkdir -p journal

if [ ! -f "$journal" ]; then
  echo "# Oturum Günlüğü — $today" > "$journal"
  echo "" >> "$journal"
fi

ts=$(date +%H:%M)

{
  echo ""
  echo "## Compact Snapshot — $ts"
  echo ""
  echo "### Son 5 Commit"
  git log --oneline -5 2>/dev/null | sed 's/^/- /' || echo "- (git log başarısız)"
  echo ""
  echo "### Uncommitted Dosya Sayısı"
  count=$(git status --porcelain 2>/dev/null | wc -l | tr -d ' ')
  echo "- $count uncommitted dosya"
  echo ""
  echo "### ROADMAP.md — Blocking / Sıradaki adım"
  if [ -f ROADMAP.md ]; then
    awk '/^## Sıradaki adım|^## Blocking durumu/{p=1} p; /^---$/{if(p)exit}' ROADMAP.md | head -15
  fi
  echo ""
} >> "$journal"

exit 0
