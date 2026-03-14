# Solution Files

This repository contains two solution files for different purposes:

## ?? VBSCalc.sln (Default - CI/CD Solution)

**Use for:** CI/CD, dependency submission, automated testing, cross-platform development

**Contains:**
- ? SchengenCalculator (Blazor WebAssembly)
- ? SchengenCalculator.Core
- ? SchengenCalculator.Api
- ? SchengenCalculator.Tests
- ? ~~SchengenCalculator.Maui~~ (excluded - see VBSCalc.Full.sln)

**Requirements:**
- Any OS (Linux, macOS, Windows)
- .NET 9 SDK
- No MAUI workloads needed

**Usage:**
```bash
# Default solution - works on any platform
dotnet restore VBSCalc.sln
dotnet build VBSCalc.sln
dotnet test VBSCalc.sln

# This is the solution used by:
# - GitHub Actions CI/CD
# - Dependency submission
# - Third-party workflows
# - Automated builds
```

**Why this is the default:**
- Works on all platforms (no MAUI dependency)
- Used by GitHub Actions (runs on Linux)
- Third-party workflows can restore it successfully
- Faster to build (no MAUI compilation)

---

## ?? VBSCalc.Full.sln (Full Solution)

**Use for:** Local development on Windows with MAUI support

**Contains:**
- ? SchengenCalculator (Blazor WebAssembly)
- ? SchengenCalculator.Core
- ? SchengenCalculator.Api
- ? SchengenCalculator.Maui (Android, iOS, Windows, macOS)
- ? SchengenCalculator.Tests

**Requirements:**
- Windows OS (for MAUI projects)
- .NET 9 SDK
- MAUI workloads installed: `dotnet workload install maui`

**Usage:**
```bash
# Full solution (Windows only)
dotnet restore VBSCalc.Full.sln
dotnet build VBSCalc.Full.sln
dotnet test VBSCalc.Full.sln

# Or open in Visual Studio
start VBSCalc.Full.sln
```

---

## Which Solution Should I Use?

| Scenario | Solution to Use |
|----------|----------------|
| Local development (non-MAUI) | `VBSCalc.sln` |
| Local development (with MAUI) | `VBSCalc.Full.sln` |
| Working on MAUI features | `VBSCalc.Full.sln` |
| CI/CD on Linux/Ubuntu | `VBSCalc.sln` ? automatic |
| GitHub Actions workflows | `VBSCalc.sln` ? automatic |
| Dependency submission | `VBSCalc.sln` ? automatic |
| Third-party workflows | `VBSCalc.sln` ? automatic |
| Quick testing without MAUI | `VBSCalc.sln` |

---

## CI/CD Workflow Strategy

The GitHub Actions workflows use `VBSCalc.sln` (the default):

### Ubuntu Jobs (Fast, Cost-Effective)
```yaml
runs-on: ubuntu-latest
steps:
  - run: dotnet restore VBSCalc.sln        # ? Default solution
  - run: dotnet build VBSCalc.sln
  - run: dotnet test VBSCalc.sln
```

### Windows Jobs (MAUI Only)
```yaml
runs-on: windows-latest
steps:
  - run: dotnet workload install maui
  - run: dotnet restore src/SchengenCalculator.Maui/SchengenCalculator.Maui.csproj
  - run: dotnet build src/SchengenCalculator.Maui/SchengenCalculator.Maui.csproj
```

This strategy:
- ? Avoids MAUI workload errors on Linux
- ? Third-party workflows work automatically
- ? Speeds up CI builds (Ubuntu is faster)
- ? Reduces costs (Ubuntu runners are cheaper)
- ? Allows parallel MAUI and non-MAUI builds

---

## Why VBSCalc.sln is the Default

**Problem we solved:**
- Third-party GitHub Actions (like dependency submission) look for `*.sln` files
- They typically use the first or "main" solution they find
- They run on Linux (no MAUI support)
- If they try to restore a solution with MAUI, it fails

**Solution:**
- `VBSCalc.sln` (no MAUI) is the primary solution
- It works on all platforms
- Third-party workflows can restore it successfully
- `VBSCalc.Full.sln` is available for Windows/MAUI development

---

## Maintaining Both Solutions

When adding new projects:

1. **Add to `VBSCalc.sln`** (default) if:
   - It's NOT a MAUI project
   - It's a library, API, Blazor, or test project
   - You want it built/tested in CI

2. **Add to `VBSCalc.Full.sln`** if:
   - It's ANY project (this solution has everything)
   - It's a MAUI project

**Example: Adding a new library project**
```bash
# Add to both solutions
dotnet sln VBSCalc.sln add src/NewProject/NewProject.csproj
dotnet sln VBSCalc.Full.sln add src/NewProject/NewProject.csproj
```

**Example: Adding a new MAUI project**
```bash
# Add only to full solution
dotnet sln VBSCalc.Full.sln add src/NewMauiProject/NewMauiProject.csproj
# DON'T add to VBSCalc.sln
```

---

## Testing

**Test non-MAUI projects (any OS):**
```bash
dotnet test VBSCalc.sln
```

**Test everything including MAUI (Windows):**
```bash
dotnet test VBSCalc.Full.sln
```

---

## Visual Studio

When you open the repository in Visual Studio:

1. **For general development:** Open `VBSCalc.sln`
   - Fast to load
   - Works on any OS
   - Builds Blazor, Core, API, Tests

2. **For MAUI development:** Open `VBSCalc.Full.sln`
   - Includes MAUI projects
   - Requires Windows
   - Full development experience

---

## File Locations

```
VBSchengenCalculator/
??? VBSCalc.sln              ? DEFAULT (no MAUI, any OS)
??? VBSCalc.Full.sln         ? Full (with MAUI, Windows only)
??? src/
?   ??? SchengenCalculator.Core/     (in both)
?   ??? SchengenCalculator.Api/      (in both)
?   ??? SchengenCalculator.Maui/     (only in Full.sln)
??? blazor-app/                      (in both)
??? tests/                           (in both)
```

---

## See Also

- [CI/CD Setup Documentation](.github/CI-SETUP.md)
- [GitHub Actions Workflows](.github/workflows/)
- [Dependency Submission Guide](.github/DEPENDENCY-SUBMISSION.md)
