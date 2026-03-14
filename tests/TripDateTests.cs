using SchengenCalculator.Models;

namespace SchengenCalculator.Tests;

public class TripDateTests
{
    // ── NumberOfDays ─────────────────────────────────────────────────────────

    [Fact]
    public void NumberOfDays_SameDay_Returns1()
    {
        var trip = new TripDate(new DateTime(2025, 1, 1), new DateTime(2025, 1, 1));
        Assert.Equal(1, trip.NumberOfDays());
    }

    [Fact]
    public void NumberOfDays_TwoConsecutiveDays_Returns2()
    {
        var trip = new TripDate(new DateTime(2025, 1, 1), new DateTime(2025, 1, 2));
        Assert.Equal(2, trip.NumberOfDays());
    }

    [Theory]
    [InlineData("2025-01-01", "2025-01-14", 14)]  // 2 weeks
    [InlineData("2025-06-01", "2025-06-30", 30)]  // 30 days
    [InlineData("2025-03-01", "2025-05-29", 90)]  // 90 days
    public void NumberOfDays_VariousDurations_ReturnsCorrectCount(string start, string end, int expected)
    {
        var trip = new TripDate(DateTime.Parse(start), DateTime.Parse(end));
        Assert.Equal(expected, trip.NumberOfDays());
    }

    // ── WasInAreaOnDate ───────────────────────────────────────────────────────

    [Fact]
    public void WasInAreaOnDate_OnStartDate_ReturnsTrue()
    {
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        Assert.True(trip.WasInAreaOnDate(new DateTime(2025, 6, 1)));
    }

    [Fact]
    public void WasInAreaOnDate_OnEndDate_ReturnsTrue()
    {
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        Assert.True(trip.WasInAreaOnDate(new DateTime(2025, 6, 14)));
    }

    [Fact]
    public void WasInAreaOnDate_DuringTrip_ReturnsTrue()
    {
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        Assert.True(trip.WasInAreaOnDate(new DateTime(2025, 6, 7)));
    }

    [Fact]
    public void WasInAreaOnDate_BeforeTrip_ReturnsFalse()
    {
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        Assert.False(trip.WasInAreaOnDate(new DateTime(2025, 5, 31)));
    }

    [Fact]
    public void WasInAreaOnDate_AfterTrip_ReturnsFalse()
    {
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        Assert.False(trip.WasInAreaOnDate(new DateTime(2025, 6, 15)));
    }

    // ── RouteDisplay ──────────────────────────────────────────────────────────

    [Fact]
    public void RouteDisplay_NoAirportsNoCountry_ReturnsEmpty()
    {
        var trip = new TripDate(DateTime.Today, DateTime.Today);
        Assert.Equal("", trip.RouteDisplay);
    }

    [Fact]
    public void RouteDisplay_WithAirportsAndCountry_FormatsCorrectly()
    {
        var trip = new TripDate(DateTime.Today, DateTime.Today, "LHR", "BCN", "Spain");
        Assert.Equal("LHR → BCN  ·  Spain", trip.RouteDisplay);
    }

    [Fact]
    public void RouteDisplay_WithAirportsNoCountry_FormatsCorrectly()
    {
        var trip = new TripDate(DateTime.Today, DateTime.Today, "LGW", "MAD");
        Assert.Equal("LGW → MAD", trip.RouteDisplay);
    }

    [Fact]
    public void RouteDisplay_CountryOnlyNoAirports_FormatsCorrectly()
    {
        var trip = new TripDate(DateTime.Today, DateTime.Today, null, null, "France");
        Assert.Equal("France", trip.RouteDisplay);
    }

    // ── ToString ──────────────────────────────────────────────────────────────

    [Fact]
    public void ToString_IncludesDaysCount()
    {
        var trip = new TripDate(new DateTime(2025, 7, 1), new DateTime(2025, 7, 14));
        var s = trip.ToString();
        Assert.Contains("14 days", s);
    }
}
