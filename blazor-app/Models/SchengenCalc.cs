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
}
