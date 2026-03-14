# GitHub Actions CI/CD Setup

This repository includes GitHub Actions workflows for automated building and testing.

## Workflows

### 1. **ci.yml** (Recommended - Simple)
A simple workflow that:
- Checks out the code
- Sets up .NET 9 with MAUI workloads (using composite action)
- Restores dependencies
- Builds the entire solution
- Runs tests

**Triggers:** Push and Pull Requests to main, master, or develop branches

### 2. **build.yml** (Detailed with Validation)
A more detailed workflow with two jobs:

**Validate Job:**
- Sets up .NET 9 with MAUI workloads
- Validates the solution can be restored

**Build Job:**
- Sets up .NET 9 with MAUI workloads
- Builds the entire solution
- Runs tests
- Publishes the Blazor WebAssembly app
- Uploads artifacts

**Triggers:** Push, Pull Requests, and manual workflow dispatch

## Composite Actions

### setup-dotnet-maui
Located in `.github/actions/setup-dotnet-maui/action.yml`

This reusable composite action:
- Sets up the .NET SDK
- Installs all MAUI workloads (`maui-android`, `maui-ios`, `maui-maccatalyst`, `maui-windows`)
- Handles installation fallback if the grouped install fails

**Usage:**
```yaml
- name: Setup .NET with MAUI
  uses: ./.github/actions/setup-dotnet-maui
  with:
    dotnet-version: '9.0.x'
```

## MAUI Workload Requirements

The MAUI project requires the following workloads to be installed:
- `maui-android` - For Android support
- `maui-ios` - For iOS support
- `maui-maccatalyst` - For macOS Catalyst support
- `maui-windows` - For Windows support

**Important:** These workloads MUST be installed BEFORE any `dotnet restore` or `dotnet build` commands that touch the MAUI project or solution file.

## How the Workflows Handle MAUI

The workflows use a composite action (`.github/actions/setup-dotnet-maui`) that:

1. Sets up the .NET SDK
2. Immediately installs MAUI workloads using:
   ```bash
   dotnet workload install maui --skip-sign-check
   ```
3. Falls back to individual workload installation if needed

This ensures MAUI workloads are available before any project validation, restoration, or build steps.

## Local Development

To set up your local environment:

```bash
# Install .NET 9 SDK
# Download from: https://dotnet.microsoft.com/download/dotnet/9.0

# Install MAUI workloads
dotnet workload install maui

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run tests
dotnet test
```

## Projects in Solution

1. **SchengenCalculator (Blazor WebAssembly)** - Web application
2. **SchengenCalculator.Core** - Core business logic
3. **SchengenCalculator.Api** - Backend API
4. **SchengenCalculator.Maui** - Mobile/Desktop app (Android, iOS, macOS, Windows)
5. **SchengenCalculator.Tests** - Unit tests

## Troubleshooting CI Builds

### Error: NETSDK1147 - MAUI workloads not installed

If you see:
```
error NETSDK1147: To build this project, the following workloads must be installed: maui-android
```

**Cause:** The MAUI workloads were not installed before trying to restore/build the solution.

**Solution:** Ensure your workflow:
1. Uses the `setup-dotnet-maui` composite action, OR
2. Manually installs workloads BEFORE any restore/build steps:
   ```yaml
   - name: Install MAUI workloads
     run: dotnet workload install maui --skip-sign-check
   ```

### Workload Installation Order

The correct order in a workflow is:
1. ? Checkout code
2. ? Setup .NET SDK
3. ? **Install MAUI workloads** ? MUST be here
4. ? Restore dependencies
5. ? Build
6. ? Test

**Never** try to restore or build before installing MAUI workloads!
