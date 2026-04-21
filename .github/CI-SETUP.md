# GitHub Actions CI/CD Setup

This repository includes GitHub Actions workflows for automated building and testing across different platforms.

## Overview

The solution contains both **cross-platform projects** (Blazor, Core, API) and **MAUI projects** (Android, iOS, Windows, macOS). Since MAUI workloads are **only supported on Windows runners**, the repository uses **two solution files**:

1. **`VBSCalc.sln`** - Default solution without MAUI (for CI/CD and cross-platform development)
2. **`VBSCalc.Full.sln`** - Full solution with MAUI (for local Windows development)

**Important:** `VBSCalc.sln` is the **default** solution that all workflows and third-party tools will use automatically.

This approach:
- ? Avoids MAUI workload errors on Linux
- ? Allows GitHub's automatic dependency submission to work
- ? Third-party GitHub Actions work automatically
- ? Speeds up CI builds (Ubuntu is faster)
- ? Reduces costs (Ubuntu runners are cheaper)
- ? Enables parallel MAUI and non-MAUI builds

## Solution Files

### VBSCalc.sln (Default - CI Solution)
**Contains:** Blazor, Core, API, Tests (no MAUI)  
**For:** CI/CD, dependency submission, automated builds, third-party workflows  
**Requires:** Only .NET 9 SDK (works on any OS)

### VBSCalc.Full.sln (Full Solution)
**Contains:** Blazor, Core, API, **MAUI**, Tests  
**For:** Local development on Windows  
**Requires:** MAUI workloads installed

See [SOLUTION-FILES.md](../SOLUTION-FILES.md) for detailed information.

## Workflows

### 1. **ci.yml** (Recommended - Simple)

Two parallel jobs:

**build-non-maui (Ubuntu):**
- Restores and builds **`VBSCalc.sln`** (default)
- Runs all tests
- Publishes Blazor artifacts
- ? Fast and cost-effective

**build-maui (Windows):**
- Installs MAUI workloads
- Builds MAUI Android app
- Builds MAUI Windows app
- ?? Required for MAUI

**Triggers:** Push and Pull Requests to main, master, or develop branches

### 2. **build.yml** (Detailed with Validation)

Three jobs with dependency chain:

**validate-non-maui (Ubuntu):**
- Validates **`VBSCalc.sln`** can restore

**build-and-test (Ubuntu):**
- Depends on validation
- Builds **`VBSCalc.sln`**
- Runs tests
- Publishes Blazor app

**build-maui (Windows):**
- Runs in parallel
- Builds MAUI projects for Android and Windows

**Triggers:** Push, Pull Requests, and manual workflow dispatch

### 3. **dependency-submission.yml** (Automatic)

Submits dependency information for security scanning:

**submit-dependencies (Ubuntu):**
- Restores **`VBSCalc.sln`** (default, no MAUI)
- Generates dependency graph
- Uploads for GitHub security scanning

**Triggers:** Push to main, master, or develop branches

## Third-Party Workflows

Third-party GitHub Actions (like dependency submission tools) will automatically use **`VBSCalc.sln`** because:
- ? It's the alphabetically first `.sln` file (after renaming)
- ? It doesn't require MAUI workloads
- ? It works on Linux/Ubuntu runners
- ? It contains all the important dependencies

**No additional configuration needed!** Third-party workflows should just work now.

## GitHub Automatic Dependency Submission

GitHub's automatic dependency submission will now:
- ? Find `VBSCalc.sln` (the default)
- ? Restore it successfully on Linux
- ? Generate dependency graph
- ? Enable security scanning

If you still see failures, see [DEPENDENCY-SUBMISSION.md](DEPENDENCY-SUBMISSION.md) for troubleshooting.

## Platform-Specific Requirements

### MAUI Projects (Windows Only)

MAUI workloads are **NOT supported on Linux or macOS** runners. They require:
- `runs-on: windows-latest`
- `dotnet workload install maui`

Supported MAUI target frameworks:
- ? `net9.0-android` - Builds on Windows
- ? `net9.0-windows` - Builds on Windows
- ?? `net9.0-ios` - Requires macOS runner (not included in workflows)
- ?? `net9.0-maccatalyst` - Requires macOS runner (not included in workflows)

### Non-MAUI Projects (Cross-Platform)

These projects build on any platform:
- ? Blazor WebAssembly (`net9.0`)
- ? Core library (`net9.0`)
- ? API (`net9.0`)
- ? Tests (`net9.0`)

## Composite Actions

### setup-dotnet-maui (?? Windows Only)
Located in `.github/actions/setup-dotnet-maui/action.yml`

**IMPORTANT:** This action only works on `windows-latest` or `windows-*` runners.

This composite action:
- Sets up the .NET SDK
- Validates it's running on Windows
- Installs all MAUI workloads
- Fails fast if used on non-Windows runners

**Usage:**
```yaml
jobs:
  build-maui:
    runs-on: windows-latest  # ? REQUIRED
    steps:
      - uses: actions/checkout@v4
      - name: Setup .NET with MAUI
        uses: ./.github/actions/setup-dotnet-maui
        with:
          dotnet-version: '9.0.x'
```

## Workflow Strategy

### Why Two Solution Files with VBSCalc.sln as Default?

**Problem:**
- Third-party workflows look for `*.sln` files
- They run on Linux (no MAUI support)
- If they find a solution with MAUI first, they fail

**Solution:**
- `VBSCalc.sln` (no MAUI) is the **primary/default** solution
- Third-party tools use it automatically
- It works on all platforms
- `VBSCalc.Full.sln` is available for Windows/MAUI development

### Job Execution Flow

```
Push/PR or Third-Party Workflow
??? Discovers VBSCalc.sln (default)
??? Ubuntu: validate-non-maui (VBSCalc.sln)
??? Ubuntu: build-and-test (VBSCalc.sln)
?   ??? ? Tests, Blazor publish
??? Windows: build-maui (MAUI project)
?   ??? ? Android, Windows builds
??? Ubuntu: dependency-submission (VBSCalc.sln)
    ??? ? Security scanning
```

## Local Development

To set up your local environment:

```bash
# Install .NET 9 SDK
# Download from: https://dotnet.microsoft.com/download/dotnet/9.0

# For MAUI development (Windows only):
dotnet workload install maui

# Build everything including MAUI (Windows):
dotnet restore VBSCalc.Full.sln
dotnet build VBSCalc.Full.sln
dotnet test VBSCalc.Full.sln

# Build non-MAUI projects (any platform):
dotnet restore VBSCalc.sln
dotnet build VBSCalc.sln
dotnet test VBSCalc.sln

# Build individual projects:
dotnet build blazor-app/SchengenCalculator.csproj
dotnet build src/SchengenCalculator.Core/SchengenCalculator.Core.csproj
dotnet build src/SchengenCalculator.Api/SchengenCalculator.Api.csproj

# Build MAUI (Windows only):
dotnet build src/SchengenCalculator.Maui/SchengenCalculator.Maui.csproj -f net9.0-android

# Run tests:
dotnet test VBSCalc.sln
```

## Projects in Solution

1. **SchengenCalculator (Blazor WebAssembly)** - Web application _(builds on any OS)_
2. **SchengenCalculator.Core** - Core business logic _(builds on any OS)_
3. **SchengenCalculator.Api** - Backend API _(builds on any OS)_
4. **SchengenCalculator.Maui** - Mobile/Desktop app _(requires Windows for build)_
5. **SchengenCalculator.Tests** - Unit tests _(runs on any OS)_

## Troubleshooting CI Builds

### Error: NETSDK1147 - MAUI workloads not supported

If you see:
```
error NETSDK1147: To build this project, the following workloads must be installed: maui-android
```

**This should NOT happen anymore** because `VBSCalc.sln` (the default) doesn't include MAUI.

**If it still happens:**
1. Check which workflow is failing
2. Check if it's explicitly using `VBSCalc.Full.sln` (it shouldn't on Linux)
3. Verify the file was renamed correctly (VBSCalc.CI.sln ? VBSCalc.sln)

**Quick fix:**
```yaml
# Make sure workflows use the default solution
- run: dotnet restore VBSCalc.sln  # ? No MAUI
- run: dotnet build VBSCalc.sln
```

### Error: Third-Party Workflow Failing

If a third-party workflow (like "submit-nuget" or similar) fails:

**Cause:** It's trying to use the wrong solution file.

**Solution:** The fix is already in place - `VBSCalc.sln` is now the default and doesn't include MAUI.

**If it persists:**
1. Check if there are any `.sln` files we don't know about
2. Verify `VBSCalc.sln` is the one without MAUI
3. Check the third-party action's configuration

### Best Practices

1. ? Use `VBSCalc.sln` for CI/CD (it's the default)
2. ? Use `VBSCalc.Full.sln` for local Windows/MAUI development
3. ? Use `ubuntu-latest` for non-MAUI jobs (fast, cheap)
4. ? Use `windows-latest` for MAUI jobs (required)
5. ? Keep both solution files in sync when adding non-MAUI projects
6. ? Third-party workflows will automatically use the correct solution
7. ? Don't restore `VBSCalc.Full.sln` on Linux
8. ? Don't try to install MAUI workloads on Linux/macOS

### Adding New Projects

**Non-MAUI project (library, API, test):**
```bash
# Add to BOTH solutions
dotnet sln VBSCalc.sln add src/NewProject/NewProject.csproj
dotnet sln VBSCalc.Full.sln add src/NewProject/NewProject.csproj
```

**MAUI project:**
```bash
# Add ONLY to full solution
dotnet sln VBSCalc.Full.sln add src/NewMauiApp/NewMauiApp.csproj
# Don't add to VBSCalc.sln (default)
```

### Workflow Template

```yaml
name: CI

on: [push, pull_request]

jobs:
  # Fast cross-platform builds on Ubuntu
  build-non-maui:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - run: dotnet restore VBSCalc.sln      # ? Default (no MAUI)
      - run: dotnet build VBSCalc.sln --no-restore
      - run: dotnet test VBSCalc.sln --no-build

  # MAUI builds on Windows
  build-maui:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - run: dotnet workload install maui --skip-sign-check
      - run: dotnet restore src/SchengenCalculator.Maui/SchengenCalculator.Maui.csproj
      - run: dotnet build src/SchengenCalculator.Maui/SchengenCalculator.Maui.csproj -f net9.0-android --no-restore
```

## See Also

- [Solution Files Documentation](../SOLUTION-FILES.md) - Detailed guide on when to use which solution
- [Dependency Submission Guide](DEPENDENCY-SUBMISSION.md) - How to handle dependency scanning
- [GitHub Actions Workflows](./) - View the actual workflow files
- [Dependabot Configuration](dependabot.yml) - Automatic dependency updates
