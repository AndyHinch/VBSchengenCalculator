# Schengen Calculator

A cross-platform Schengen Area day-tracker for UK travellers. Track your 90-day / 180-day visa-free allowance across web, iOS, and Android.

## Features

- **90/180-day rolling window calculator** — accurate day counting with the true rolling window rule
- **Travel Planner** — "Can I travel?" check for proposed trips, and "Earliest safe date" finder
- **Dashboard** — live status card with progress bar, days used/remaining, and 30/60/90-day outlook
- **Approaching-limit warnings** — colour-coded alerts (green / amber / red)
- **Traveller profiles** — multiple independent trip histories on the same device
- **Airport autocomplete** — 28 UK departure airports + 100+ Schengen arrival airports; learns custom airports
- **iCal export** — download trips as a `.ics` calendar file
- **Print / PDF** — print-friendly summary layout
- **PWA** — installable as a home-screen app on mobile browsers
- **Dark mode** — persistent theme preference
- **Schengen rules guide** — collapsible explainer with country list and common misconceptions
- **Trip statistics** — charts of days per year, country breakdown
- **User registration** — free account for up to 10 trips + cloud sync across devices

## User Tiers

| Tier | Trip limit | Cloud sync |
|------|-----------|------------|
| Anonymous (no account) | 4 trips | No — localStorage only |
| Free account | 10 trips | Yes — synced via API |
| Paid (future) | Unlimited | Yes |

## Project Structure

```
VBSchengenCalculator/
├── blazor-app/                 # Blazor WebAssembly web app
│   ├── Components/             # Razor UI components
│   ├── Models/                 # Data models (TripDate, SchengenCalc, etc.)
│   ├── Pages/                  # Page routes (Home.razor)
│   └── Services/               # AirportService, ProfileService, AuthService, TripApiService
├── src/
│   ├── SchengenCalculator.Core/  # Shared models, DTOs, AppConfig/UserTier
│   ├── SchengenCalculator.Api/   # ASP.NET Core 8 REST API (Identity, JWT, EF Core, SQLite)
│   └── SchengenCalculator.Maui/  # .NET MAUI Blazor Hybrid (iOS + Android)
├── tests/                      # xUnit tests (52 tests covering all core logic)
├── VBSCalc/                    # Original VB.NET Windows Forms app (reference)
└── VBSCalc.sln                 # Visual Studio solution
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- For mobile (MAUI): [Visual Studio 2022](https://visualstudio.microsoft.com/) with the MAUI workload, or Visual Studio for Mac

### Run the API

```bash
cd src/SchengenCalculator.Api
dotnet run
# API available at https://localhost:7001
```

The API auto-migrates the SQLite database on first run. For production, set the `Jwt:Secret` and `ConnectionStrings:DefaultConnection` environment variables or use `appsettings.Production.json`.

### Run the Web App

```bash
# Update blazor-app/wwwroot/appsettings.json with your API URL first
cd blazor-app
dotnet run
# Open http://localhost:5000
```

The web app works without the API (anonymous mode, 4-trip limit). Register a free account to unlock 10 trips and cloud sync.

### Run the Tests

```bash
dotnet test tests/
# 52 tests, all passing
```

### Build the Mobile App (MAUI)

1. Open `VBSCalc.sln` in Visual Studio 2022 (Windows) or Visual Studio for Mac
2. Set `SchengenCalculator.Maui` as the startup project
3. Select your target: Android Emulator, iOS Simulator, or physical device
4. Update `MauiProgram.cs` with your API URL
5. Build and run

**iOS** requires Xcode and an Apple Developer account (free for simulator, $99/yr for App Store).
**Android** requires Android SDK (installed with Visual Studio MAUI workload).

## API Endpoints

| Method | Route | Auth | Description |
|--------|-------|------|-------------|
| POST | `/api/auth/register` | None | Create a free account |
| POST | `/api/auth/login` | None | Sign in, get JWT |
| GET | `/api/trips` | JWT | Get all trips |
| POST | `/api/trips` | JWT | Add a trip |
| PUT | `/api/trips/{id}` | JWT | Update a trip |
| DELETE | `/api/trips/{id}` | JWT | Delete a trip |
| DELETE | `/api/trips` | JWT | Delete all trips |

## Configuration

### API (`appsettings.Development.json`)

```json
{
  "ConnectionStrings": { "DefaultConnection": "Data Source=schengen-dev.db" },
  "Jwt": {
    "Secret": "your-secret-32-chars-minimum!",
    "Issuer": "SchengenCalculator",
    "Audience": "SchengenCalculatorUsers"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:5000"]
  }
}
```

**Important:** Never commit a real `Jwt:Secret` to source control. Use environment variables in production:
```bash
export Jwt__Secret="your-production-secret"
```

### Web App (`blazor-app/wwwroot/appsettings.json`)

```json
{
  "ApiBaseUrl": "https://your-api-url.com/"
}
```

## Monetisation

The app is architected for ads and paid tiers:

- **Free tier** (anonymous, 4 trips): show banner/interstitial ads
- **Registered free** (10 trips): show smaller banner ads
- **Paid tier** (unlimited): ad-free

**Web ads:** [Google AdSense](https://adsense.google.com) — add the AdSense script to `index.html` and place `<ins class="adsbygoogle">` elements in the layout.

**Mobile ads (iOS/Android):** [Google AdMob](https://admob.google.com) — add the `Plugin.MauiMTAdmob` NuGet package to the MAUI project. AdMob requires your app to be published on the App Store / Google Play.

## Deployment

### API

Any Linux host with .NET 8:
- [Azure App Service](https://azure.microsoft.com/services/app-service/) (F1 free tier available)
- [Railway](https://railway.app), [Fly.io](https://fly.io), [Render](https://render.com)
- Self-hosted VPS (e.g. DigitalOcean Droplet, Hetzner)

For production, swap SQLite for PostgreSQL by replacing the EF Core provider:
```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### Web App

- [GitHub Pages](https://pages.github.com) (static, anonymous only)
- [Azure Static Web Apps](https://azure.microsoft.com/services/app-service/static/) (free tier, integrates with API)
- Any static host (Netlify, Cloudflare Pages)

## Licence

MIT — © Andrew Hinchcliffe
