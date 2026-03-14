# GitHub Actions CI/CD Setup

This repository includes GitHub Actions workflows for automated building and testing.

## Workflows

### 1. **ci.yml** (Recommended - Simple)
A simple workflow that:
- Checks out the code
- Sets up .NET 9
- Automatically restores required MAUI workloads
- Restores dependencies
- Builds the entire solution
- Runs tests

**Triggers:** Push and Pull Requests to main, master, or develop branches

### 2. **build.yml** (Detailed)
A more detailed workflow that:
- Explicitly installs all MAUI workloads
- Builds each project individually
- Runs tests
- Publishes the Blazor WebAssembly app
- Uploads artifacts

**Triggers:** Push, Pull Requests, and manual workflow dispatch

## MAUI Workload Requirements

The MAUI project requires the following workloads to be installed:
- `maui-android` - For Android support
- `maui-ios` - For iOS support
- `maui-maccatalyst` - For macOS Catalyst support
- `maui-windows` - For Windows support

These are automatically installed in the CI workflows using:
```bash
dotnet workload restore
```

## Local Development

To set up your local environment:

```bash
# Install .NET 9 SDK
# Download from: https://dotnet.microsoft.com/download/dotnet/9.0

# Install MAUI workloads
dotnet workload install maui

# Or restore from the solution
dotnet workload restore

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

If you encounter the error:
```
error NETSDK1147: To build this project, the following workloads must be installed: maui-android
```

This means the MAUI workloads aren't installed. The workflows in this repository handle this automatically with `dotnet workload restore` or explicit `dotnet workload install` commands.
