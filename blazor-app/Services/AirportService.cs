using System.Text.Json;
using Microsoft.JSInterop;
using SchengenCalculator.Models;

namespace SchengenCalculator.Services;

/// <summary>
/// Provides airport search with support for custom user-defined airports
/// and a recently-used list that floats matching airports to the top.
/// Data is persisted in localStorage.
/// </summary>
public class AirportService
{
    private readonly IJSRuntime _js;

    private List<Airport> _custom = [];
    private List<string> _recentDepartures = [];
    private List<string> _recentArrivals = [];
    private bool _initialised;

    private const string CustomKey  = "schengen_custom_airports";
    private const string RecentDepKey = "schengen_recent_dep";
    private const string RecentArrKey = "schengen_recent_arr";
    private const int MaxRecent = 8;

    public AirportService(IJSRuntime js) => _js = js;

    // ── Initialisation ─────────────────────────────────────────────────────

    public async Task EnsureInitialisedAsync()
    {
        if (_initialised) return;

        _custom           = await LoadListAsync<List<Airport>>(CustomKey)  ?? [];
        _recentDepartures = await LoadListAsync<List<string>>(RecentDepKey) ?? [];
        _recentArrivals   = await LoadListAsync<List<string>>(RecentArrKey) ?? [];
        _initialised = true;
    }

    // ── Search ─────────────────────────────────────────────────────────────

    /// <summary>Search UK departure airports, recently-used float to the top.</summary>
    public async Task<IEnumerable<Airport>> SearchUkAsync(string query, CancellationToken _ = default)
    {
        await EnsureInitialisedAsync();
        return Search(AllUk(), query, _recentDepartures);
    }

    /// <summary>Search Schengen arrival airports, recently-used float to the top.</summary>
    public async Task<IEnumerable<Airport>> SearchSchengenAsync(string query, CancellationToken _ = default)
    {
        await EnsureInitialisedAsync();
        return Search(AllSchengen(), query, _recentArrivals);
    }

    /// <summary>Look up a single airport by IATA code across all lists.</summary>
    public Airport? FindByCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return null;
        return AllUk().Concat(AllSchengen())
                      .FirstOrDefault(a => a.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
    }

    // ── Custom airports ────────────────────────────────────────────────────

    public async Task AddCustomAsync(Airport airport)
    {
        await EnsureInitialisedAsync();
        if (!_custom.Any(a => a.Code.Equals(airport.Code, StringComparison.OrdinalIgnoreCase)))
        {
            _custom.Add(new Airport(airport.Code.ToUpperInvariant(), airport.Name,
                                    airport.City, airport.Country, isCustom: true));
            await SaveAsync(CustomKey, _custom);
        }
    }

    public IReadOnlyList<Airport> CustomAirports => _custom.AsReadOnly();

    // ── Recently used ──────────────────────────────────────────────────────

    public async Task RecordDepartureAsync(string code)
    {
        await EnsureInitialisedAsync();
        _recentDepartures = AddRecent(_recentDepartures, code);
        await SaveAsync(RecentDepKey, _recentDepartures);
    }

    public async Task RecordArrivalAsync(string code)
    {
        await EnsureInitialisedAsync();
        _recentArrivals = AddRecent(_recentArrivals, code);
        await SaveAsync(RecentArrKey, _recentArrivals);
    }

    // ── Private helpers ────────────────────────────────────────────────────

    private IEnumerable<Airport> AllUk()      => AirportData.UkAirports.Concat(_custom);
    private IEnumerable<Airport> AllSchengen() => AirportData.SchengenAirports.Concat(_custom);

    private static IEnumerable<Airport> Search(
        IEnumerable<Airport> source, string query, List<string> recent)
    {
        IEnumerable<Airport> matched;

        if (string.IsNullOrWhiteSpace(query))
        {
            // Show recently used first, then the full list
            var recentAirports = recent
                .Select(code => source.FirstOrDefault(a =>
                    a.Code.Equals(code, StringComparison.OrdinalIgnoreCase)))
                .OfType<Airport>();
            matched = recentAirports.Concat(source).DistinctBy(a => a.Code).Take(12);
        }
        else
        {
            matched = source.Where(a =>
                a.Code.StartsWith(query, StringComparison.OrdinalIgnoreCase) ||
                a.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                a.City.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                a.Country.Contains(query, StringComparison.OrdinalIgnoreCase));

            // Float recently used to top within results
            var recentCodes = recent.ToHashSet(StringComparer.OrdinalIgnoreCase);
            matched = matched.OrderByDescending(a => recentCodes.Contains(a.Code));
        }

        return matched;
    }

    private static List<string> AddRecent(List<string> list, string code)
    {
        var updated = list.Where(c => !c.Equals(code, StringComparison.OrdinalIgnoreCase)).ToList();
        updated.Insert(0, code.ToUpperInvariant());
        return updated.Take(MaxRecent).ToList();
    }

    private async Task<T?> LoadListAsync<T>(string key)
    {
        var json = await _js.InvokeAsync<string?>("localStorage.getItem", key);
        return string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json);
    }

    private async Task SaveAsync<T>(string key, T value) =>
        await _js.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
}
