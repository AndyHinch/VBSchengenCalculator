# CLAUDE.md — Developer Guide for AI Assistants

This file documents the architecture, conventions, and key decisions for the Schengen Calculator project. Read this before making changes.

## Repository Layout

```
blazor-app/          Blazor WebAssembly web frontend (primary project to edit)
src/
  SchengenCalculator.Core/   Shared models, DTOs, AppConfig
  SchengenCalculator.Api/    ASP.NET Core 8 REST API
  SchengenCalculator.Maui/   .NET MAUI Blazor Hybrid (iOS + Android shell)
tests/               xUnit tests — run with: dotnet test tests/
VBSCalc/             Original VB.NET app (reference only, do not edit)
```

## Development Branch

All changes go to: `claude/refactor-logic-ui-split-NeW6e`

## Build Commands

```bash
# Web app
dotnet build blazor-app/

# API
dotnet build src/SchengenCalculator.Api/

# Tests
dotnet test tests/

# Run API locally
cd src/SchengenCalculator.Api && dotnet run

# Run web app locally
cd blazor-app && dotnet run
```

## Architecture

### User Tiers & Trip Limits

Defined in `blazor-app/Models/AppConfig.cs` (and mirrored in `src/SchengenCalculator.Core/AppConfig.cs`):

```csharp
public const int AnonymousTripLimit = 4;
public const int FreeTierTripLimit  = 10;
public const int PaidTierTripLimit  = int.MaxValue;
```

- **Anonymous** — no account, trips stored in localStorage via `ProfileService`
- **Free** — registered account, trips synced via REST API
- **Paid** — future tier, unlimited trips

### Data Flow

```
Anonymous user:
  Home.razor ──► ProfileService ──► localStorage

Authenticated user:
  Home.razor ──► TripApiService ──► HTTP ──► SchengenCalculator.Api ──► SQLite/Postgres
```

### Authentication

- `AuthService` (`blazor-app/Services/AuthService.cs`): manages JWT token, persists to localStorage
- `TripApiService` (`blazor-app/Services/TripApiService.cs`): calls the REST API when authenticated
- `AuthBar.razor`: shows sign-in/out button and user tier badge
- `AuthModal.razor`: register/login modal with tab switcher
- `TripLimitBanner.razor`: shown when trip count reaches the tier limit

### API (`src/SchengenCalculator.Api/`)

- ASP.NET Core 8 with controllers
- ASP.NET Core Identity + JWT bearer auth
- Entity Framework Core 8 with SQLite (dev) / PostgreSQL (prod)
- Auto-migrates on startup
- **Never commit `Jwt:Secret`** — use environment variables in production

Key files:
- `Data/AppUser.cs` — IdentityUser with `Tier` and `DisplayName`
- `Data/TripEntity.cs` — trip record owned by a user
- `Controllers/AuthController.cs` — register + login endpoints
- `Controllers/TripsController.cs` — CRUD, enforces tier trip limits
- `Services/TokenService.cs` — creates 30-day JWT tokens

### Core Logic (`blazor-app/Models/SchengenCalc.cs`)

The rolling 90/180-day calculator. Key methods:
- `NumberOfDaysInAreaOnDay(date)` — days used in the 180-day window ending on `date`
- `DaysAvailableOnDate(date)` — days remaining on a future date
- `CheckProposedTrip(start, end)` → `TravelCheckResult` — safe/tight/breach
- `EarliestSafeTripStart(days, from)` — brute-force search, up to 2 years ahead

### Component Hierarchy (web)

```
Home.razor
├── AuthBar            ← sign-in/out, tier display
├── TripLimitBanner    ← shown when at/near limit
├── AuthModal          ← register/login modal
├── ProfileSwitcher    ← multi-profile management
├── Dashboard          ← status card, progress bar, outlook
├── TripList + CalendarView
├── TravelPlanner      ← "Can I travel?" + earliest date tabs
├── Calculator         ← single-date day count
├── TripStatistics     ← charts (MudChart)
├── Predictions
└── RulesGuide         ← collapsible explainer
```

## Conventions

- **Blazor WASM**: all JS interop calls (`localStorage`, `print`, `setTheme`) must happen in `OnAfterRenderAsync(firstRender)` or later — never in `OnInitialized`.
- **MudBlazor v9**: use `ChartSeries<double>` (not `ChartSeries`), `ChartLabels` (not `XAxisLabels`).
- **Date handling**: all dates are `DateTime` (not `DateOnly`) for broad compatibility. No `TimeZone` offsets — Schengen rules are calendar-day based.
- **Model copies**: `blazor-app/Models/` contains copies of the models. The `src/SchengenCalculator.Core/` library is the canonical source — keep them in sync if changing shared models.
- **Trip identification**: `TripDate.ApiId` (nullable `Guid`) links a local trip object to its API record. Null = not yet synced or anonymous.

## Environment Variables (Production API)

```
Jwt__Secret=<32+ char random string>
ConnectionStrings__DefaultConnection=Host=...;Database=...
Cors__AllowedOrigins__0=https://your-web-app-url.com
```

## Future: Paid Tier

The `UserTier.Paid` enum value is already defined. To activate:
1. Integrate a payment provider (e.g. Stripe)
2. Add a `POST /api/account/upgrade` endpoint that sets `user.Tier = UserTier.Paid` after payment confirmation
3. The trip limit enforcement in `TripsController` and `AuthService` will automatically apply `AppConfig.PaidTierTripLimit`

## Future: Ads

- **Web**: Add Google AdSense script to `blazor-app/wwwroot/index.html`, insert `<ins class="adsbygoogle">` components in `Home.razor` for anonymous/free tiers
- **Mobile**: Add `Plugin.MauiMTAdmob` to `SchengenCalculator.Maui` and initialise in `MauiProgram.cs`

## Tests

52 unit tests in `tests/` covering:
- `TripDate` — `NumberOfDays`, `WasInAreaOnDate`, `RouteDisplay`, `ToString`
- `SchengenCalc` — rolling window, over-limit, dashboard helpers, `CheckProposedTrip`, `EarliestSafeTripStart`
- `TravelCheckResult` — `IsSafe`, `IsTight` edge cases
- `TripDates` — CRUD, `MaxDate`/`MinDate`, JSON round-trip

Run: `dotnet test tests/`
