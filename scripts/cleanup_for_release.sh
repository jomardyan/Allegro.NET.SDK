#!/usr/bin/env bash
set -euo pipefail

echo "Cleaning build artifacts and untracking them from git (dry-run first)..."

# Show what would be removed
echo "The following directories will be removed from the working tree and untracked from git:"
find . -type d \( -name bin -o -name obj -o -path './nupkg' \) -print

echo
read -p "Proceed and remove these directories from working tree and untrack them from git? [y/N] " confirm
if [[ "$confirm" != "y" && "$confirm" != "Y" ]]; then
  echo "Aborted by user. No changes made."
  exit 0
fi

# Remove from working tree and untrack previously committed build artifacts
echo "Removing bin/ obj/ and nupkg directories from working tree..."
find . -type d \( -name bin -o -name obj -o -path './nupkg' \) -exec rm -rf '{}' + || true

if git rev-parse --git-dir >/dev/null 2>&1; then
  echo "Untracking previously committed build artifacts from git index..."
  git rm -r --cached --ignore-unmatch **/bin || true
  git rm -r --cached --ignore-unmatch **/obj || true
  git rm -r --cached --ignore-unmatch nupkg || true
  echo "Note: You still need to commit the deletions: git commit -m 'chore: remove build artifacts from repository'"
fi

echo "Cleanup complete. Ensure .gitignore contains bin/ and obj/ (it already does)."
