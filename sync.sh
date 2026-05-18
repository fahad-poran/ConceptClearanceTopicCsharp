#!/usr/bin/env bash
set -euo pipefail

msg="${1:-update: $(date '+%Y-%m-%d %H:%M:%S')}"

# Stage all tracked/untracked changes except ignored files.
git add .

# Avoid failing when there is nothing new to commit.
if git diff --cached --quiet; then
  echo "No staged changes to commit."
else
  git commit -m "$msg"
fi

git push origin main
