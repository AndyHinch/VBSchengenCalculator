using System.Text.Json;

namespace SchengenCalculator.Models;

public class TripDates
{
    private List<TripDate> _entries = [];

    public IReadOnlyList<TripDate> Entries => _entries.AsReadOnly();

    public void AddEntry(TripDate trip) => _entries.Add(trip);

    public void RemoveEntry(DateTime startDate) =>
        _entries.RemoveAll(t => t.StartDate == startDate);

    public void RemoveAll() => _entries.Clear();

    public DateTime MaxDate() =>
        _entries.Count == 0 ? DateTime.Today : _entries.Max(t => t.EndDate);

    public DateTime MinDate() =>
        _entries.Count == 0 ? DateTime.Today : _entries.Min(t => t.StartDate);

    public string ToJson() => JsonSerializer.Serialize(_entries);

    /// <summary>
    /// Loads trips from JSON. Handles both camelCase keys (this app) and
    /// PascalCase keys (original VB.NET / Newtonsoft.Json output).
    /// </summary>
    public void LoadFromJson(string json)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _entries = JsonSerializer.Deserialize<List<TripDate>>(json, options) ?? [];
    }
}
