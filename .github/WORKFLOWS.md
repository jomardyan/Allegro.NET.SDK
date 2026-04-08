# GitHub Actions Workflows

This document describes the GitHub Actions CI/CD workflows configured for the AllegroApi SDK.

## Table of Contents

- [Workflows Overview](#workflows-overview)
- [CI Workflow](#ci-workflow)
- [Build and Test (Multi-OS)](#build-and-test-multi-os)
- [Publish to NuGet](#publish-to-nuget)
- [Setup Instructions](#setup-instructions)
- [Secrets Configuration](#secrets-configuration)
- [Troubleshooting](#troubleshooting)

---

## Workflows Overview

### 1. **CI** (`ci.yml`)
- **Trigger:** Push to `main`/`develop`, Pull Requests, Manual
- **Purpose:** Fast feedback on commits
- **Runs on:** Ubuntu Latest
- **Duration:** ~2-3 minutes

### 2. **Build and Test (Multi-OS)** (`build-test.yml`)
- **Trigger:** Push to `main`/`develop`, Pull Requests, Manual
- **Purpose:** Comprehensive testing across platforms
- **Runs on:** Ubuntu, Windows, macOS
- **Duration:** ~5-7 minutes

### 3. **Publish to NuGet** (`publish-nuget.yml`)
- **Trigger:** Git tags (`v*.*.*`), Manual
- **Purpose:** Publish releases to NuGet.org
- **Runs on:** Ubuntu Latest
- **Duration:** ~4-5 minutes
- **Requires:** `NUGET_API_KEY` secret

---

## CI Workflow

**File:** `.github/workflows/ci.yml`

### Triggers
```yaml
on:
  push:
    branches: [ main, develop ]
    paths:
      - 'src/**'
      - 'tests/**'
      - '*.sln'
      - '.github/workflows/ci.yml'
  pull_request:
    branches: [ main ]
  workflow_dispatch:
```

### Jobs

#### 1. **build-and-test**
- Checkout code
- Setup .NET 8.0
- Restore dependencies
- Build in Release mode
- Run all tests (143 tests)
- Publish test results
- Collect code coverage
- Pack NuGet package (main branch only)
- Upload artifacts

#### 2. **security-scan**
- Checkout code
- Setup .NET 8.0
- Restore dependencies
- Scan for vulnerable packages
- Fail if vulnerabilities found

### Artifacts
- `nuget-packages` - NuGet packages (.nupkg, .snupkg) - 7 days
- `coverage-reports` - Code coverage reports - 7 days

### Status Badge
```markdown
[![CI](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml)
```

---

## Build and Test (Multi-OS)

**File:** `.github/workflows/build-test.yml`

### Triggers
Same as CI workflow

### Jobs

#### 1. **build-and-test** (Matrix)
Runs on: `ubuntu-latest`, `windows-latest`, `macos-latest`

- Checkout code
- Setup .NET 8.0
- Display .NET info
- Restore dependencies
- Build in Release mode
- Run tests with TRX logger
- Publish test results (Linux only)
- Collect code coverage (Ubuntu only)
- Upload coverage to Codecov (Ubuntu only)

#### 2. **analyze**
- Code analysis
- Security scan
- Check for vulnerable packages

#### 3. **pack-validation**
- Build NuGet package
- Validate package structure
- Upload package artifact

### Artifacts
- `build-artifacts` - Build outputs - 1 day
- `nuget-package-preview` - Preview packages - 7 days

### Status Badge
```markdown
[![Build and Test](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml)
```

---

## Publish to NuGet

**File:** `.github/workflows/publish-nuget.yml`

### Triggers

#### Automatic (Git Tags)
```bash
# Create and push tag
git tag -a v2.0.0 -m "Release v2.0.0"
git push origin v2.0.0
```

#### Manual (Workflow Dispatch)
1. Go to Actions tab
2. Select "Publish to NuGet" workflow
3. Click "Run workflow"
4. Enter version (e.g., 2.0.0)
5. Click "Run workflow"

### Jobs

#### 1. **build-and-test**
- Checkout code
- Setup .NET 8.0
- Restore dependencies
- Build in Release mode
- Run all tests
- Upload build artifacts

#### 2. **pack**
- Checkout code
- Setup .NET 8.0
- Set version from tag or input
- Update project version
- Restore and build
- Pack NuGet package
- List package contents
- Upload package artifacts

#### 3. **publish**
- Download package artifacts
- Setup .NET 8.0
- Publish to NuGet.org
- Create GitHub Release (if tag)

### Environment Variables
- `DOTNET_VERSION: '8.0.x'`
- `PROJECT_PATH: 'src/AllegroApi/AllegroApi.csproj'`
- `CONFIGURATION: 'Release'`
- `PACKAGE_VERSION` - Set from tag or input

### Artifacts
- `build-artifacts` - Build outputs - 1 day
- `nuget-packages` - NuGet packages - 7 days

### Status Badge
```markdown
[![Publish to NuGet](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml)
```

---

## Setup Instructions

### 1. Enable GitHub Actions

GitHub Actions should be enabled by default. Verify:
1. Go to repository Settings
2. Navigate to Actions → General
3. Ensure "Allow all actions and reusable workflows" is selected
4. Save if needed

### 2. Configure Branch Protection (Optional)

For `main` branch:
1. Go to Settings → Branches
2. Add rule for `main`
3. Enable "Require status checks to pass before merging"
4. Select required checks:
   - `Build and Test / build-and-test (ubuntu-latest)`
   - `CI / build-and-test`
   - `CI / security-scan`
5. Save changes

### 3. Set Up Secrets

#### Required for Publishing

**NUGET_API_KEY**
1. Get API key from https://www.nuget.org/account/apikeys
2. Go to repository Settings → Secrets and variables → Actions
3. Click "New repository secret"
4. Name: `NUGET_API_KEY`
5. Value: Your NuGet API key
6. Click "Add secret"

#### Optional for Code Coverage

**CODECOV_TOKEN** (if using Codecov)
1. Sign up at https://codecov.io
2. Add repository
3. Get token
4. Add as repository secret

### 4. Test Workflows

#### Test CI Workflow
```bash
# Make a small change
echo "# Test" >> README.md
git add README.md
git commit -m "test: trigger CI"
git push
```

#### Test Manual Dispatch
1. Go to Actions tab
2. Select any workflow
3. Click "Run workflow"
4. Select branch
5. Click "Run workflow"

---

## Secrets Configuration

### Repository Secrets

| Secret Name | Required For | How to Get |
|-------------|--------------|------------|
| `NUGET_API_KEY` | Publishing to NuGet.org | https://www.nuget.org/account/apikeys |
| `CODECOV_TOKEN` | Code coverage reports | https://codecov.io |
| `GITHUB_TOKEN` | GitHub API access | Automatically provided |

### Creating NuGet API Key

1. Log in to https://www.nuget.org
2. Go to Account → API Keys
3. Click "Create"
4. Settings:
   - **Key Name:** `GitHub Actions - AllegroApi`
   - **Package Owner:** Select your account
   - **Scopes:** `Push`, `Push new packages and package versions`
   - **Packages:** `AllegroApi` (or `*` for all)
   - **Glob Pattern:** `AllegroApi*`
   - **Expiration:** 365 days (recommended)
5. Click "Create"
6. Copy the API key immediately (shown only once)
7. Add to GitHub secrets

### Security Best Practices

- ✅ Never commit secrets to repository
- ✅ Use least privilege principle (specific package access)
- ✅ Set expiration dates on API keys
- ✅ Rotate keys periodically
- ✅ Use environment protection for production
- ✅ Review workflow logs for exposed secrets

---

## Troubleshooting

### Common Issues

#### 1. Tests Failing in CI but Passing Locally

**Possible causes:**
- Different .NET SDK versions
- Missing dependencies
- Platform-specific code issues
- Environment variables not set

**Solutions:**
```bash
# Check .NET version locally
dotnet --version

# Run tests exactly as CI does
dotnet test --configuration Release --verbosity normal
```

#### 2. NuGet Push Fails

**Error:** `401 Unauthorized`

**Solutions:**
- Verify `NUGET_API_KEY` secret is set
- Check API key hasn't expired
- Ensure key has `Push` scope
- Verify package name matches key pattern

**Error:** `409 Conflict - Package version already exists`

**Solutions:**
- Increment version in `AllegroApi.csproj`
- Use `--skip-duplicate` flag (already included)
- Delete package version from NuGet.org (if needed)

#### 3. Workflow Not Triggering

**Check:**
- Workflow file syntax (YAML formatting)
- Branch names match triggers
- File paths in `paths:` filter
- GitHub Actions enabled in repository

**Solutions:**
```bash
# Validate YAML locally
yamllint .github/workflows/*.yml

# Manually trigger workflow
# Go to Actions → Select workflow → Run workflow
```

#### 4. Build Warnings Treated as Errors

**Error:** `error MSB4025` or similar

**Solutions:**
- Review warnings in workflow log
- Fix code issues
- Update `.csproj` to allow warnings:
  ```xml
  <TreatWarningsAsErrors>false</TreatWarningsAsErrors>
  ```

#### 5. Artifact Upload/Download Fails

**Error:** `Artifact not found`

**Solutions:**
- Check artifact name matches between upload/download
- Verify job dependencies (`needs:`)
- Ensure upload job completed successfully
- Check artifact retention period

### Debugging Workflows

#### Enable Debug Logging

1. Go to Settings → Secrets and variables → Actions
2. Add secret:
   - Name: `ACTIONS_STEP_DEBUG`
   - Value: `true`
3. Re-run workflow

#### View Detailed Logs

```bash
# Download workflow logs
gh run download <run-id>

# View specific job logs
gh run view <run-id> --log
```

#### Test Locally with Act

```bash
# Install act (https://github.com/nektos/act)
brew install act  # macOS
# or
choco install act-cli  # Windows

# Run workflow locally
act -j build-and-test

# Run specific job
act -j security-scan
```

---

## Workflow Best Practices

### 1. Keep Workflows Fast
- ✅ Use caching for dependencies
- ✅ Run only necessary jobs
- ✅ Use matrix builds for multi-platform
- ✅ Parallelize independent jobs

### 2. Security
- ✅ Pin action versions (`@v4`, not `@main`)
- ✅ Review third-party actions
- ✅ Limit secret access
- ✅ Use environment protection rules

### 3. Reliability
- ✅ Add status checks to branches
- ✅ Use `if:` conditions wisely
- ✅ Handle failures gracefully
- ✅ Set appropriate timeouts

### 4. Maintainability
- ✅ Document workflows (this file!)
- ✅ Use consistent naming
- ✅ Extract common steps
- ✅ Keep workflows simple

---

## Monitoring and Notifications

### GitHub Status Badges

Add to README.md:
```markdown
[![CI](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/ci.yml)
[![Build and Test](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/build-test.yml)
[![Publish to NuGet](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml/badge.svg)](https://github.com/jomardyan/Allegro.NET.SDK/actions/workflows/publish-nuget.yml)
```

### Email Notifications

GitHub sends emails automatically:
- On workflow failures (for you)
- On workflow runs (if watching repo)

Configure in: Settings → Notifications

### Slack/Discord Integration

Use GitHub Actions for Slack/Discord:
```yaml
- name: Notify Slack
  if: failure()
  uses: slackapi/slack-github-action@v1
  with:
    webhook-url: ${{ secrets.SLACK_WEBHOOK }}
```

---

## Resources

- **GitHub Actions Docs:** https://docs.github.com/en/actions
- **Workflow Syntax:** https://docs.github.com/en/actions/reference/workflow-syntax-for-github-actions
- **NuGet Publish:** https://docs.microsoft.com/en-us/nuget/nuget-org/publish-a-package
- **Action Marketplace:** https://github.com/marketplace?type=actions
- **Community Support:** https://github.community/c/actions

---

  
**Maintained By:** jomardyan
