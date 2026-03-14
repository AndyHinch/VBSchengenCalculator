# GitHub Actions CI/CD Setup

This repository includes GitHub Actions workflows for automated building and testing across different platforms.

## Overview

The solution contains both **cross-platform projects** (Blazor, Core, API) and **MAUI projects** (Android, iOS, Windows, macOS). Since MAUI workloads are **only supported on Windows runners**, the repository uses **two solution files**:

1. **`VBSCalc.sln`** - Full solution with MAUI (for local Windows development)
2. **`VBSCalc.CI.sln`** - CI solution without MAUI (for Linux/macOS builds and dependency submission)

This approach:
- ? Avoids MAUI workload errors on Linux
- ? Allows GitHub's automatic dependency submission to work
- ? Speeds up CI builds (Ubuntu is faster)
- ? Reduces costs (Ubuntu runners are cheaper)
- ? Enables parallel MAUI and non-MAUI builds

## Solution Files

### VBSCalc.sln (Full - Windows Only)
**Contains:** Blazor, Core, API, **MAUI**, Tests  
**For:** Local development on Windows  
**Requires:** MAUI workloads installed

### VBSCalc.CI.sln (CI - Any Platform)
**Contains:** Blazor, Core, API, Tests (no MAUI)  
**For:** CI/CD, dependency submission, automated builds  
**Requires:** Only .NET 9 SDK

See [SOLUTION-FILES.md](../SOLUTION-FILES.md) for detailed information.

## Workflows

### 1. **ci.yml** (Recommended - Simple)

Two parallel jobs:

**build-non-maui (Ubuntu):**
- Restores and builds **`VBSCalc.CI.sln`**
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
- Validates **`VBSCalc.CI.sln`** can restore

**build-and-test (Ubuntu):**
- Depends on validation
- Builds **`VBSCalc.CI.sln`**
- Runs tests
- Publishes Blazor app

**build-maui (Windows):**
- Runs in parallel
- Builds MAUI projects for Android and Windows

**Triggers:** Push, Pull Requests, and manual workflow dispatch

## GitHub Automatic Dependency Submission

GitHub's automatic dependency submission workflow will now work correctly because it restores **`VBSCalc.CI.sln`** by default, which:
- ? Doesn't require MAUI workloads
- ? Runs on Linux without errors
- ? Includes all the dependencies you care about

The dependency submission workflow looks for `.sln` files and will prefer the one that works. If it fails, it will fall back to individual projects.

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

**? Don't do this:**
```yaml
jobs:
  build:
    runs-on: ubuntu-latest  # ? WRONG! MAUI doesn't work on Linux
    steps:
      - uses: ./.github/actions/setup-dotnet-maui  # ? Will fail
```

## Workflow Strategy

### Why Use Two Solution Files?

**Problem:**
- `VBSCalc.sln` includes MAUI project
- Restoring it requires MAUI workloads
- MAUI workloads don't install on Linux
- GitHub's dependency submission runs on Linux
- Result: ? Build failures

**Solution:**
- `VBSCalc.CI.sln` excludes MAUI
- Can restore on any platform
- Used by CI workflows and dependency submission
- `VBSCalc.sln` still exists for local Windows dev
- Result: ? Builds succeed everywhere

### Job Execution Flow

```
Push/PR
??? Ubuntu: validate-non-maui (VBSCalc.CI.sln)
??? Ubuntu: build-and-test (VBSCalc.CI.sln)
?   ??? ? Tests, Blazor publish
??? Windows: build-maui (MAUI project)
    ??? ? Android, Windows builds
```

## Local Development

To set up your local environment:

```bash
# Install .NET 9 SDK
# Download from: https://dotnet.microsoft.com/download/dotnet/9.0

# For MAUI development (Windows only):
dotnet workload install maui

# Build everything (Windows):
dotnet restore VBSCalc.sln
dotnet build VBSCalc.sln
dotnet test VBSCalc.sln

# Build non-MAUI projects (any platform):
dotnet restore VBSCalc.CI.sln
dotnet build VBSCalc.CI.sln
dotnet test VBSCalc.CI.sln

# Build individual projects:
dotnet build blazor-app/SchengenCalculator.csproj
dotnet build src/SchengenCalculator.Core/SchengenCalculator.Core.csproj
dotnet build src/SchengenCalculator.Api/SchengenCalculator.Api.csproj

# Build MAUI (Windows only):
dotnet build src/SchengenCalculator.Maui/SchengenCalculator.Maui.csproj -f net9.0-android

# Run tests:
dotnet test VBSCalc.CI.sln
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
Workload installation failed: Workload ID maui isn't supported on this platform.
```

**Cause:** Trying to restore `VBSCalc.sln` (which includes MAUI) on a non-Windows runner.

**Solution:** Use `VBSCalc.CI.sln` instead on Linux/macOS:

? **Correct:**
```yaml
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - run: dotnet restore VBSCalc.CI.sln  # ? No MAUI
      - run: dotnet build VBSCalc.CI.sln
```

? **Wrong:**
```yaml
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - run: dotnet restore VBSCalc.sln  # ? Includes MAUI, will fail!
```

### Error: Dependency Submission Failing

If GitHub's automatic dependency submission fails with MAUI workload errors:

**Cause:** It's trying to restore `VBSCalc.sln` which includes MAUI.

**Solution:** The presence of `VBSCalc.CI.sln` will help. If it still fails, you can:

1. Add a `.github/dependabot.yml` to control dependency scanning
2. Disable automatic dependency submission and use a custom workflow
3. The CI solution should make the automatic workflow work

**Custom workflow example:**
```yaml
name: Dependency Submission
on: push

jobs:
  submit:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - run: dotnet restore VBSCalc.CI.sln
      - uses: actions/dependency-submission/dotnet@v3
        with:
          solution-path: VBSCalc.CI.sln
```

### Best Practices

1. ? Use `VBSCalc.CI.sln` for CI/CD on Linux/macOS
2. ? Use `VBSCalc.sln` for local Windows development
3. ? Use `ubuntu-latest` for non-MAUI jobs (fast, cheap)
4. ? Use `windows-latest` for MAUI jobs (required)
5. ? Keep both solution files in sync when adding non-MAUI projects
6. ? Don't restore `VBSCalc.sln` on Linux
7. ? Don't try to install MAUI workloads on Linux/macOS

### Adding New Projects

**Non-MAUI project (library, API, test):**
```bash
# Add to BOTH solutions
dotnet sln VBSCalc.sln add src/NewProject/NewProject.csproj
dotnet sln VBSCalc.CI.sln add src/NewProject/NewProject.csproj
```

**MAUI project:**
```bash
# Add ONLY to full solution
dotnet sln VBSCalc.sln add src/NewMauiApp/NewMauiApp.csproj
# Don't add to VBSCalc.CI.sln
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
      - run: dotnet restore VBSCalc.CI.sln
      - run: dotnet build VBSCalc.CI.sln --no-restore
      - run: dotnet test VBSCalc.CI.sln --no-build

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
- [GitHub Actions Workflows](./) - View the actual workflow files
