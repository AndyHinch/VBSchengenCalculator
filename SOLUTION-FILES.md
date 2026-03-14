# Solution Files

This repository contains two solution files for different purposes:

## ?? VBSCalc.sln (Full Solution)

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
# Open in Visual Studio
start VBSCalc.sln

# Or build from command line (Windows only)
dotnet restore VBSCalc.sln
dotnet build VBSCalc.sln
```

---

## ?? VBSCalc.CI.sln (CI Solution)

**Use for:** CI/CD builds on Linux/macOS, dependency submission, automated testing

**Contains:**
- ? SchengenCalculator (Blazor WebAssembly)
- ? SchengenCalculator.Core
- ? SchengenCalculator.Api
- ? SchengenCalculator.Tests
- ? ~~SchengenCalculator.Maui~~ (excluded)

**Requirements:**
- Any OS (Linux, macOS, Windows)
- .NET 9 SDK
- No MAUI workloads needed

**Usage:**
```bash
# Restore and build (works on any platform)
dotnet restore VBSCalc.CI.sln
dotnet build VBSCalc.CI.sln
dotnet test VBSCalc.CI.sln
```

**Why this exists:**
- MAUI workloads are only supported on Windows
- GitHub Actions dependency submission runs on Linux
- CI builds can run faster on Ubuntu runners
- Allows testing non-MAUI projects without MAUI dependencies

---

## Which Solution Should I Use?

| Scenario | Solution to Use |
|----------|----------------|
| Local development on Windows | `VBSCalc.sln` |
| Working on MAUI features | `VBSCalc.sln` |
| CI/CD on Linux/Ubuntu | `VBSCalc.CI.sln` |
| GitHub Actions workflows | `VBSCalc.CI.sln` (non-MAUI jobs) |
| Dependency submission | `VBSCalc.CI.sln` |
| Quick testing without MAUI | `VBSCalc.CI.sln` |

---

## CI/CD Workflow Strategy

The GitHub Actions workflows use both solutions:

### Ubuntu Jobs (Fast, Cost-Effective)
```yaml
runs-on: ubuntu-latest
steps:
  - run: dotnet restore VBSCalc.CI.sln   # ? CI solution
  - run: dotnet build VBSCalc.CI.sln
  - run: dotnet test VBSCalc.CI.sln
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
- ? Speeds up CI builds (Ubuntu is faster)
- ? Reduces costs (Ubuntu runners are cheaper)
- ? Allows parallel MAUI and non-MAUI builds

---

## Maintaining Both Solutions

When adding new projects:

1. **Add to `VBSCalc.sln`** if:
   - It's a MAUI project
   - It's any project you want in the full solution

2. **Add to `VBSCalc.CI.sln`** if:
   - It's NOT a MAUI project
   - It needs to be built/tested in CI on Linux
   - It's a library, API, or test project

**Example: Adding a new library project**
```bash
# Add to both solutions
dotnet sln VBSCalc.sln add src/NewProject/NewProject.csproj
dotnet sln VBSCalc.CI.sln add src/NewProject/NewProject.csproj
```

**Example: Adding a new MAUI project**
```bash
# Add only to full solution
dotnet sln VBSCalc.sln add src/NewMauiProject/NewMauiProject.csproj
# DON'T add to VBSCalc.CI.sln
```

---

## Testing

**Test everything (Windows):**
```bash
dotnet test VBSCalc.sln
```

**Test non-MAUI projects (any OS):**
```bash
dotnet test VBSCalc.CI.sln
```

---

## See Also

- [CI/CD Setup Documentation](.github/CI-SETUP.md)
- [GitHub Actions Workflows](.github/workflows/)
