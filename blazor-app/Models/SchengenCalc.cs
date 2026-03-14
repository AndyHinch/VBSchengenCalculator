namespace SchengenCalculator.Models;

/// <summary>
/// Core Schengen day calculator.
/// Counts days spent in the area within a rolling window ending on the review date.
/// Ported from SchengenCalculator.vb
/// </summary>
public class SchengenCalc
{
    private readonly IEnumerable<TripDate> _trips;
    private readonly int _maximumDaysInArea;
    private readonly int _reviewDaysInArea;

    public int MaxDays => _maximumDaysInArea;

    public SchengenCalc(IEnumerable<TripDate> trips, int maximumDaysInArea = 90, int reviewDaysInArea = 180)
    {
        _trips = trips;
        _maximumDaysInArea = maximumDaysInArea;
        _reviewDaysInArea = reviewDaysInArea;
    }

    // ── Core calculation ───────────────────────────────────────────────────

    /// <summary>
    /// Returns days spent in the Schengen Area within the rolling window
    /// [reviewDay - reviewDaysInArea, reviewDay].
    /// </summary>
    public int NumberOfDaysInAreaOnDay(DateTime reviewDay)
    {
        int noOfDays = 0;
        DateTime calcDate = reviewDay.AddDays(-_reviewDaysInArea);

        while (calcDate <= reviewDay)
        {
            foreach (var trip in _trips)
            {
                if (trip.WasInAreaOnDate(calcDate))
                    noOfDays++;
            }
            calcDate = calcDate.AddDays(1);
        }

        return noOfDays;
    }

    /// <summary>Returns a human-readable summary for a given review date.</summary>
    public string SummaryForDay(DateTime reviewDay)
    {
        int used = NumberOfDaysInAreaOnDay(reviewDay);
        int remaining = _maximumDaysInArea - used;
        return remaining >= 0
            ? $"{used} used, {remaining} remaining"
            : $"{used} used, {Math.Abs(remaining)} over!";
    }

    public bool IsOverLimit(DateTime reviewDay) =>
        NumberOfDaysInAreaOnDay(reviewDay) > _maximumDaysInArea;

    // ── Dashboard helpers ──────────────────────────────────────────────────

    /// <summary>
    /// Days available (not used) on a future date.
    /// Useful for "how many days will I have in 30/60/90 days?".
    /// </summary>
    public int DaysAvailableOnDate(DateTime date) =>
        Math.Max(0, _maximumDaysInArea - NumberOfDaysInAreaOnDay(date));

    /// <summary>
    /// The next calendar date on which a used day rolls out of the 180-day window,
    /// freeing up one day of allowance. Returns null if there are no trips in the window.
    /// </summary>
    public DateTime? NextDayFreeDate(DateTime? from = null)
    {
        var reviewDay = from ?? DateTime.Today;
        var windowStart = reviewDay.AddDays(-_reviewDaysInArea);

        DateTime? earliest = null;
        foreach (var trip in _trips)
        {
            if (trip.EndDate < windowStart || trip.StartDate > reviewDay) continue;
            var firstInWindow = trip.StartDate < windowStart ? windowStart : trip.StartDate;
            if (earliest == null || firstInWindow < earliest.Value)
                earliest = firstInWindow;
        }

        return earliest?.AddDays(_reviewDaysInArea + 1);
    }

    // ── Travel planning ────────────────────────────────────────────────────

    /// <summary>
    /// Checks whether a proposed trip (start→end) would breach the limit given the
    /// existing trips. Returns the first date of breach, or null if the trip is safe.
    /// </summary>
    public TravelCheckResult CheckProposedTrip(DateTime start, DateTime end)
    {
        var withProposed = _trips.Append(new TripDate(start, end));
        var tempCalc = new SchengenCalc(withProposed, _maximumDaysInArea, _reviewDaysInArea);

        int maxUsed = 0;
        DateTime? breachDate = null;
        var d = start;

        while (d <= end)
        {
            int used = tempCalc.NumberOfDaysInAreaOnDay(d);
            if (used > maxUsed) maxUsed = used;
            if (used > _maximumDaysInArea && breachDate == null)
                breachDate = d;
            d = d.AddDays(1);
        }

        return new TravelCheckResult(maxUsed, _maximumDaysInArea - maxUsed, breachDate);
    }

    /// <summary>
    /// Finds the earliest date from <paramref name="searchFrom"/> on which a trip of
    /// <paramref name="desiredDays"/> days would be entirely within the 90-day limit.
    /// Searches up to two years ahead. Returns null if no safe window is found.
    /// </summary>
    public DateTime? EarliestSafeTripStart(int desiredDays, DateTime? searchFrom = null)
    {
        var from = searchFrom ?? DateTime.Today;
        var limit = from.AddYears(2);
        var candidate = from;

        while (candidate <= limit)
        {
            var result = CheckProposedTrip(candidate, candidate.AddDays(desiredDays - 1));
            if (result.IsSafe) return candidate;
            candidate = candidate.AddDays(1);
        }

        return null;
    }
}
