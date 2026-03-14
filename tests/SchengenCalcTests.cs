using SchengenCalculator.Models;

namespace SchengenCalculator.Tests;

/// <summary>
/// Tests for the core rolling 90/180-day calculation engine.
/// All dates are deterministic — no DateTime.Today is used.
/// </summary>
public class SchengenCalcTests
{
    // ── NumberOfDaysInAreaOnDay ────────────────────────────────────────────────

    [Fact]
    public void NoTrips_Returns0()
    {
        var calc = new SchengenCalc(Array.Empty<TripDate>());
        Assert.Equal(0, calc.NumberOfDaysInAreaOnDay(new DateTime(2025, 7, 1)));
    }

    [Fact]
    public void SingleTrip_ReviewDayDuringTrip_CountsCorrectly()
    {
        // 14-day trip; reviewing on last day should count all 14 days
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        var calc = new SchengenCalc(new[] { trip });
        Assert.Equal(14, calc.NumberOfDaysInAreaOnDay(new DateTime(2025, 6, 14)));
    }

    [Fact]
    public void SingleTrip_ReviewDayAfterWindowExpiry_Returns0()
    {
        // Trip in Jan 2024; review 181+ days later — should be outside the 180-day window
        var trip = new TripDate(new DateTime(2024, 1, 1), new DateTime(2024, 1, 30));
        var calc = new SchengenCalc(new[] { trip });
        // Review on 1 Aug 2024 — 213 days after the trip ended, fully outside window
        Assert.Equal(0, calc.NumberOfDaysInAreaOnDay(new DateTime(2024, 8, 1)));
    }

    [Fact]
    public void SingleTrip_PartiallyInWindow_CountsOnlyWindowDays()
    {
        // 30-day trip starting 170 days before the review date
        // Only the last ~10 days of the trip fall inside the 180-day window
        var reviewDate = new DateTime(2025, 7, 1);
        var tripStart = reviewDate.AddDays(-170);          // 11 Feb 2025
        var tripEnd = tripStart.AddDays(29);               // 12 Mar 2025 (30 days)
        // Window starts at reviewDate - 180 = 2 Jan 2025
        // tripStart = 11 Feb 2025, tripEnd = 12 Mar 2025 → all inside window → 30 days
        var trip = new TripDate(tripStart, tripEnd);
        var calc = new SchengenCalc(new[] { trip });
        var result = calc.NumberOfDaysInAreaOnDay(reviewDate);
        Assert.Equal(30, result);
    }

    [Fact]
    public void MultipleTrips_SummedCorrectly()
    {
        var reviewDate = new DateTime(2025, 7, 1);
        var trips = new[]
        {
            new TripDate(new DateTime(2025, 5, 1), new DateTime(2025, 5, 14)),  // 14 days
            new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 7)),   //  7 days
        };
        var calc = new SchengenCalc(trips);
        Assert.Equal(21, calc.NumberOfDaysInAreaOnDay(reviewDate));
    }

    [Fact]
    public void Exactly90Days_NotOverLimit()
    {
        var reviewDate = new DateTime(2025, 9, 1);
        var tripStart = reviewDate.AddDays(-89);  // 90 days inclusive
        var trip = new TripDate(tripStart, reviewDate);
        var calc = new SchengenCalc(new[] { trip });
        Assert.Equal(90, calc.NumberOfDaysInAreaOnDay(reviewDate));
        Assert.False(calc.IsOverLimit(reviewDate));
    }

    [Fact]
    public void Exactly91Days_IsOverLimit()
    {
        var reviewDate = new DateTime(2025, 9, 1);
        var tripStart = reviewDate.AddDays(-90);  // 91 days inclusive, but only 90 days are in 180-day window from start
        // Actually let's construct carefully: 91-day trip ending on reviewDate, fully within 180-day window
        var tripStart91 = reviewDate.AddDays(-90); // 91 days
        var trip = new TripDate(tripStart91, reviewDate);
        var calc = new SchengenCalc(new[] { trip });
        // Days in window = min(91, 180+1 - overlap) — check isOverLimit
        var days = calc.NumberOfDaysInAreaOnDay(reviewDate);
        Assert.True(days > 90);
        Assert.True(calc.IsOverLimit(reviewDate));
    }

    // ── SummaryForDay ─────────────────────────────────────────────────────────

    [Fact]
    public void SummaryForDay_WithinLimit_ShowsRemaining()
    {
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14)); // 14 days
        var calc = new SchengenCalc(new[] { trip });
        var summary = calc.SummaryForDay(new DateTime(2025, 6, 14));
        Assert.Contains("14 used", summary);
        Assert.Contains("76 remaining", summary);
    }

    [Fact]
    public void SummaryForDay_OverLimit_ShowsOver()
    {
        // 91 days in area
        var tripStart = new DateTime(2025, 4, 1);
        var tripEnd = new DateTime(2025, 6, 30);  // 91 days
        var calc = new SchengenCalc(new[] { new TripDate(tripStart, tripEnd) });
        var summary = calc.SummaryForDay(tripEnd);
        Assert.Contains("over", summary.ToLower());
    }

    // ── DaysAvailableOnDate ───────────────────────────────────────────────────

    [Fact]
    public void DaysAvailableOnDate_NoTrips_Returns90()
    {
        var calc = new SchengenCalc(Array.Empty<TripDate>());
        Assert.Equal(90, calc.DaysAvailableOnDate(new DateTime(2025, 7, 1)));
    }

    [Fact]
    public void DaysAvailableOnDate_After14DayTrip_Returns76()
    {
        var trip = new TripDate(new DateTime(2025, 6, 1), new DateTime(2025, 6, 14));
        var calc = new SchengenCalc(new[] { trip });
        Assert.Equal(76, calc.DaysAvailableOnDate(new DateTime(2025, 6, 14)));
    }

    [Fact]
    public void DaysAvailableOnDate_CannotGoNegative()
    {
        // More than 90 days used — available should be 0, not negative
        var trip = new TripDate(new DateTime(2025, 1, 1), new DateTime(2025, 6, 30)); // 181 days
        var calc = new SchengenCalc(new[] { trip });
        Assert.Equal(0, calc.DaysAvailableOnDate(new DateTime(2025, 6, 30)));
    }

    // ── NextDayFreeDate ───────────────────────────────────────────────────────

    [Fact]
    public void NextDayFreeDate_NoTrips_ReturnsNull()
    {
        var calc = new SchengenCalc(Array.Empty<TripDate>());
        Assert.Null(calc.NextDayFreeDate(new DateTime(2025, 7, 1)));
    }

    [Fact]
    public void NextDayFreeDate_WithTrip_ReturnsCorrectDate()
    {
        // Trip started on 2025-01-01; from the review date 2025-01-05,
        // the earliest day in the window is Jan 1 which frees 180+1 days later
        var tripStart = new DateTime(2025, 1, 1);
        var trip = new TripDate(tripStart, new DateTime(2025, 1, 14));
        var calc = new SchengenCalc(new[] { trip });
        var reviewDate = new DateTime(2025, 1, 15);
        var freeDate = calc.NextDayFreeDate(reviewDate);
        // tripStart (Jan 1) + 181 days = July 1 2025
        Assert.NotNull(freeDate);
        var expected = tripStart.AddDays(181);
        Assert.Equal(expected, freeDate!.Value);
    }

    // ── CheckProposedTrip ─────────────────────────────────────────────────────

    [Fact]
    public void CheckProposedTrip_NoExistingTrips_SafeShortTrip()
    {
        var calc = new SchengenCalc(Array.Empty<TripDate>());
        var start = new DateTime(2025, 7, 1);
        var result = calc.CheckProposedTrip(start, start.AddDays(13)); // 14 days
        Assert.True(result.IsSafe);
        Assert.Equal(14, result.MaxDaysUsed);
        Assert.Equal(76, result.DaysRemainingAfter);
    }

    [Fact]
    public void CheckProposedTrip_WouldExceedLimit_ReturnsBreachDate()
    {
        // Already used 85 days; propose another 10 days
        var existingTrip = new TripDate(new DateTime(2025, 4, 1), new DateTime(2025, 6, 24)); // 85 days
        var calc = new SchengenCalc(new[] { existingTrip });
        var proposed = new DateTime(2025, 6, 25);
        var result = calc.CheckProposedTrip(proposed, proposed.AddDays(9)); // 10 days
        Assert.False(result.IsSafe);
        Assert.NotNull(result.BreachDate);
    }

    [Fact]
    public void CheckProposedTrip_ExactlyAt90Days_IsSafe()
    {
        // Already used 80 days, propose 10 more = 90 exactly
        var reviewBase = new DateTime(2025, 7, 1);
        var existingTrip = new TripDate(reviewBase.AddDays(-79), reviewBase.AddDays(-1)); // 79 days ago to yesterday = 79 days
        var calc = new SchengenCalc(new[] { existingTrip });
        // Verify existing days
        var existing = calc.NumberOfDaysInAreaOnDay(new DateTime(2025, 7, 1));
        var start = reviewBase;
        var remaining = 90 - existing;
        var result = calc.CheckProposedTrip(start, start.AddDays(remaining - 1));
        Assert.True(result.IsSafe);
    }

    [Fact]
    public void CheckProposedTrip_IsTight_WhenLessThan10DaysRemaining()
    {
        // Use 82 days, propose 4 more = 86 used, 4 remaining — "tight"
        var existingTrip = new TripDate(new DateTime(2025, 3, 1), new DateTime(2025, 5, 21)); // 82 days
        var calc = new SchengenCalc(new[] { existingTrip });
        var proposed = new DateTime(2025, 5, 22);
        var result = calc.CheckProposedTrip(proposed, proposed.AddDays(3)); // 4 days
        Assert.True(result.IsSafe);
        Assert.True(result.IsTight);
    }

    // ── EarliestSafeTripStart ─────────────────────────────────────────────────

    [Fact]
    public void EarliestSafeTripStart_NoExistingTrips_ReturnsSearchFrom()
    {
        var calc = new SchengenCalc(Array.Empty<TripDate>());
        var from = new DateTime(2025, 7, 1);
        var result = calc.EarliestSafeTripStart(14, from);
        Assert.Equal(from, result);
    }

    [Fact]
    public void EarliestSafeTripStart_AllowanceExhausted_ReturnsDateAfterWindowRolls()
    {
        // Full 90-day trip from today; the next safe start should be after those days roll out
        var anchor = new DateTime(2025, 1, 1);
        var trip = new TripDate(anchor, anchor.AddDays(89)); // 90 days
        var calc = new SchengenCalc(new[] { trip });
        var searchFrom = anchor.AddDays(90); // day after trip ends
        var result = calc.EarliestSafeTripStart(1, searchFrom);
        Assert.NotNull(result);
        // The earliest safe day must be after the window rolls over
        Assert.True(result!.Value > searchFrom);
    }

    [Fact]
    public void EarliestSafeTripStart_DesiredDaysZero_ReturnsSearchFrom()
    {
        // Edge case: 0-day trip is always safe
        var calc = new SchengenCalc(Array.Empty<TripDate>());
        var from = new DateTime(2025, 7, 1);
        var result = calc.EarliestSafeTripStart(0, from);
        Assert.Equal(from, result);
    }
}
