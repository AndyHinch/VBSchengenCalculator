namespace SchengenCalculator.Core.Models;

public record TravelCheckResult(
    int MaxDaysUsed,
    int DaysRemainingAfter,
    DateTime? BreachDate)
{
    public bool IsSafe => BreachDate == null;
    public bool IsTight => IsSafe && DaysRemainingAfter < 10;
}
