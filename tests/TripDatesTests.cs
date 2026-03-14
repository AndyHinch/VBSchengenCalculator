using SchengenCalculator.Models;

namespace SchengenCalculator.Tests;

public class TripDatesTests
{
    // ── AddEntry / Entries ────────────────────────────────────────────────────

    [Fact]
    public void AddEntry_StoresTrip()
    {
        var td = new TripDates();
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        td.AddEntry(trip);
        Assert.Single(td.Entries);
        Assert.Equal(trip.StartDate, td.Entries[0].StartDate);
    }

    [Fact]
    public void RemoveEntry_ByStartDate_RemovesCorrectTrip()
    {
        var td = new TripDates();
        var trip1 = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        var trip2 = new TripDate(new DateTime(2025, 7, 1), new DateTime(2025, 7, 14));
        td.AddEntry(trip1);
        td.AddEntry(trip2);
        td.RemoveEntry(trip1.StartDate);
        Assert.Single(td.Entries);
        Assert.Equal(trip2.StartDate, td.Entries[0].StartDate);
    }

    [Fact]
    public void RemoveAll_ClearsAllEntries()
    {
        var td = new TripDates();
        td.AddEntry(new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14)));
        td.AddEntry(new TripDate(new DateTime(2025, 7, 1), new DateTime(2025, 7, 14)));
        td.RemoveAll();
        Assert.Empty(td.Entries);
    }

    // ── MaxDate / MinDate ─────────────────────────────────────────────────────

    [Fact]
    public void MaxDate_NoEntries_ReturnsToday()
    {
        var td = new TripDates();
        Assert.Equal(DateTime.Today, td.MaxDate());
    }

    [Fact]
    public void MinDate_NoEntries_ReturnsToday()
    {
        var td = new TripDates();
        Assert.Equal(DateTime.Today, td.MinDate());
    }

    [Fact]
    public void MaxDate_WithEntries_ReturnsLatestEndDate()
    {
        var td = new TripDates();
        td.AddEntry(new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14)));
        td.AddEntry(new TripDate(new DateTime(2025, 7, 1), new DateTime(2025, 7, 20)));
        Assert.Equal(new DateTime(2025, 7, 20), td.MaxDate());
    }

    [Fact]
    public void MinDate_WithEntries_ReturnsEarliestStartDate()
    {
        var td = new TripDates();
        td.AddEntry(new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14)));
        td.AddEntry(new TripDate(new DateTime(2025, 3, 15), new DateTime(2025, 3, 28)));
        Assert.Equal(new DateTime(2025, 3, 15), td.MinDate());
    }

    // ── JSON round-trip ───────────────────────────────────────────────────────

    [Fact]
    public void ToJson_ThenLoadFromJson_PreservesTrips()
    {
        var td = new TripDates();
        td.AddEntry(new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14), "LHR", "BCN", "Spain"));
        td.AddEntry(new TripDate(new DateTime(2025, 9, 1), new DateTime(2025, 9, 10), "LGW", "CDG", "France"));

        var json = td.ToJson();

        var td2 = new TripDates();
        td2.LoadFromJson(json);

        Assert.Equal(2, td2.Entries.Count);
        Assert.Equal(new DateTime(2025, 6, 1), td2.Entries[0].StartDate);
        Assert.Equal("BCN", td2.Entries[0].ArrivalAirport);
        Assert.Equal("Spain", td2.Entries[0].Country);
        Assert.Equal(new DateTime(2025, 9, 1), td2.Entries[1].StartDate);
        Assert.Equal("France", td2.Entries[1].Country);
    }

    [Fact]
    public void LoadFromJson_CaseInsensitiveKeys_ParsesCorrectly()
    {
        // Simulate PascalCase output from VB.NET/Newtonsoft
        var json = """[{"StartDate":"2025-06-01","EndDate":"2025-06-14"}]""";
        var td = new TripDates();
        td.LoadFromJson(json);
        Assert.Single(td.Entries);
        Assert.Equal(new DateTime(2025, 6, 1), td.Entries[0].StartDate);
    }

    [Fact]
    public void LoadFromJson_EmptyArray_ResultsInEmptyEntries()
    {
        var td = new TripDates();
        td.LoadFromJson("[]");
        Assert.Empty(td.Entries);
    }
}
