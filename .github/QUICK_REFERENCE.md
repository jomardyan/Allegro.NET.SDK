# GitHub Actions Quick Reference

## Quick Commands

### View Workflow Status
```bash
# Using GitHub CLI
gh workflow list
gh workflow view ci.yml
gh run list --workflow=ci.yml
gh run watch
```

### Trigger Workflows Manually
```bash
# Trigger CI workflow
gh workflow run ci.yml

# Trigger with specific branch
gh workflow run ci.yml --ref develop

# Trigger publish workflow with version
gh workflow run publish-nuget.yml -f version=2.0.0
```

### Download Artifacts
```bash
# List recent runs
gh run list --limit 5

# Download artifacts from specific run
gh run download <run-id>

# Download specific artifact
gh run download <run-id> -n nuget-packages
```

### View Logs
```bash
# View latest run logs
gh run view --log

# View specific job logs
gh run view <run-id> --job=<job-id> --log
```


## Workflow Triggers

### CI Workflow
**Automatic:**

**Manual:**
```bash
gh workflow run ci.yml
```

### Build and Test (Multi-OS)
**Automatic:**

**Manual:**
```bash
gh workflow run build-test.yml
```

### Publish to NuGet
**Automatic:**

**Manual:**
```bash
# Via GitHub CLI
gh workflow run publish-nuget.yml -f version=2.0.0

# Via Git tag
git tag -a v2.0.0 -m "Release v2.0.0"
git push origin v2.0.0
```


## Required Secrets

| Secret | Required | Where to Get |
|--------|----------|--------------|
| `NUGET_API_KEY` | ✅ Yes | https://www.nuget.org/account/apikeys |
| `CODECOV_TOKEN` | ⚠️ Optional | https://codecov.io |
| `GITHUB_TOKEN` | ✅ Auto | Provided by GitHub |

### Add Secret via GitHub CLI
```bash
# Set NuGet API key
gh secret set NUGET_API_KEY

# Set Codecov token
gh secret set CODECOV_TOKEN

# List all secrets
gh secret list
```


## Workflow Status Badges

Add to README.md:
```markdown
[![CI](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml)
[![Build and Test](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml)
[![Publish to NuGet](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml)
```


## Common Tasks

### Release New Version

1. **Update version in project:**
   ```bash
   # Edit src/AllegroApi/AllegroApi.csproj
   # Change <Version>1.5.0</Version> to <Version>2.0.0</Version>
   ```

2. **Update RELEASE_NOTES.md:**
   ```bash
   # Add new version section at top
   ```

3. **Commit and tag:**
   ```bash
   git add .
   git commit -m "chore: release v1.6.0"
   git push
   
   git tag -a v1.6.0 -m "Release v1.6.0"
   git push origin v1.6.0
   ```

4. **Monitor workflow:**
   ```bash
   gh run watch
   ```

### Run Tests Locally Like CI

```bash
# Clean build
dotnet clean
dotnet restore
dotnet build --configuration Release --no-restore

# Run tests
dotnet test --configuration Release --no-build --verbosity normal

# With coverage
dotnet test --configuration Release --no-build \
  --collect:"XPlat Code Coverage" \
  --results-directory ./coverage
```

### Validate Workflows Locally

```bash
# Install act (https://github.com/nektos/act)
brew install act  # macOS
# or
choco install act-cli  # Windows

# List available workflows
act -l

# Run CI workflow
act -j build-and-test

# Run specific job
act -j security-scan

# Dry run
act --dryrun
```


## Troubleshooting

### Workflow Not Running

**Check:**
```bash
# View workflow definition
gh workflow view ci.yml

# Check if workflow is enabled
gh workflow enable ci.yml

# View recent runs
gh run list --workflow=ci.yml --limit 10
```

### Build Failing in CI

**Debug:**
```bash
# Enable debug logging (add secret)
gh secret set ACTIONS_STEP_DEBUG --body "true"

# View detailed logs
gh run view --log

# Download and inspect artifacts
gh run download <run-id>
```

### NuGet Publish Fails

**Common Issues:**

1. **Missing or invalid API key:**
   ```bash
   # Verify secret exists
   gh secret list | grep NUGET_API_KEY
   
   # Update secret
   gh secret set NUGET_API_KEY
   ```

2. **Version already exists:**
   ```bash
   # Increment version in .csproj
   # Push new tag
   git tag -a v1.5.1 -m "Release v1.5.1"
   git push origin v1.5.1
   ```

3. **Package validation failed:**
   ```bash
   # Test package locally
   dotnet pack --configuration Release
   dotnet validate package local ./nupkg/AllegroApi.2.0.0.nupkg
   ```


## Notifications

### Enable Email Notifications
1. Go to GitHub → Settings → Notifications
2. Under "Actions":
   - ✅ Send notifications for failed workflows
   - ✅ Send notifications for workflow run approvals

### Slack/Discord Integration

Add to workflow:
```yaml
  if: failure()
  uses: 8398a7/action-slack@v3
  with:
    status: ${{ job.status }}
    webhook_url: ${{ secrets.SLACK_WEBHOOK_URL }}
```


## Resources



**Last Updated:** October 14, 2025  
**AllegroApi Version:** 2.0.0
