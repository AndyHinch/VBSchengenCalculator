# Disabling GitHub's Automatic Dependency Submission

GitHub automatically runs a dependency submission workflow for .NET repositories. This can fail when your solution includes MAUI projects because it runs on Linux.

## The Problem

GitHub's automatic dependency submission:
- Runs on Linux
- Scans for `.sln` files
- Tries to restore them
- **Fails** if the solution includes MAUI (which requires Windows)

## Solutions

### Option 1: Use Our Custom Workflow (Recommended)

We've created `.github/workflows/dependency-submission.yml` which:
- ? Uses `VBSCalc.CI.sln` (no MAUI)
- ? Runs on Linux successfully
- ? Generates dependency information

**This workflow runs automatically on push to main/master/develop branches.**

### Option 2: Disable Automatic Dependency Submission

If the automatic workflow keeps failing, you can disable it in the repository settings:

1. Go to your repository on GitHub
2. Click **Settings**
3. Click **Code security and analysis** (in the left sidebar)
4. Find **Dependency graph**
5. Under **Automatic dependency submission**, click **Disable**

Then rely on our custom workflow instead.

### Option 3: Configure Repository Settings

You can also configure this via the GitHub API or repository settings file:

Create/update `.github/settings.yml`:
```yaml
repository:
  # Disable automatic dependency submission
  automatic_dependency_submission: false
```

**Note:** This requires the "Settings" GitHub App to be installed.

## Verification

To check if the automatic workflow is disabled:

1. Go to **Actions** tab
2. Look for workflows named "Dependency Submission" or similar
3. Check which ones are enabled

## Our Custom Workflow

The custom workflow (`.github/workflows/dependency-submission.yml`):

```yaml
name: Dependency Submission
on:
  push:
    branches: [ main, master, develop ]

jobs:
  submit-dependencies:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'
      - run: dotnet restore VBSCalc.CI.sln  # ? Uses CI solution
      - run: dotnet list VBSCalc.CI.sln package --format json > dependencies.json
```

This workflow:
- ? Works on Linux
- ? Uses the CI solution (no MAUI)
- ? Generates dependency information
- ? Won't fail due to MAUI workload issues

## Dependabot Configuration

We've also created `.github/dependabot.yml` which:
- Configures automatic dependency updates
- Groups related packages together
- Ignores MAUI packages (since they need Windows to test)

This is separate from dependency submission and will work correctly.

## Monitoring

After pushing these changes:

1. **Check the Actions tab** for any failing "Dependency Submission" workflows
2. **Look for our custom workflow** - it should succeed
3. **If the automatic one still fails**, disable it using Option 2 above

## Further Reading

- [GitHub Dependency Submission](https://docs.github.com/en/code-security/supply-chain-security/understanding-your-software-supply-chain/about-dependency-submission)
- [Dependabot Configuration](https://docs.github.com/en/code-security/dependabot/dependabot-version-updates/configuration-options-for-the-dependabot.yml-file)
