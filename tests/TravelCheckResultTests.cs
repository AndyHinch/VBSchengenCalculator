using SchengenCalculator.Models;

namespace SchengenCalculator.Tests;

public class TravelCheckResultTests
{
    [Fact]
    public void IsSafe_NoBreachDate_ReturnsTrue()
    {
        var result = new TravelCheckResult(50, 40, null);
        Assert.True(result.IsSafe);
    }

    [Fact]
    public void IsSafe_WithBreachDate_ReturnsFalse()
    {
        var result = new TravelCheckResult(95, -5, new DateTime(2025, 7, 10));
        Assert.False(result.IsSafe);
    }

    [Fact]
    public void IsTight_SafeWithLessThan10DaysRemaining_ReturnsTrue()
    {
        var result = new TravelCheckResult(83, 7, null);
        Assert.True(result.IsTight);
    }

    [Fact]
    public void IsTight_SafeWithExactly10DaysRemaining_ReturnsFalse()
    {
        var result = new TravelCheckResult(80, 10, null);
        Assert.False(result.IsTight);
    }

    [Fact]
    public void IsTight_NotSafe_ReturnsFalse()
    {
        // Not safe means IsTight must also be false
        var result = new TravelCheckResult(95, -5, new DateTime(2025, 7, 10));
        Assert.False(result.IsTight);
    }

    [Fact]
    public void IsTight_SafeWithMoreThan10DaysRemaining_ReturnsFalse()
    {
        var result = new TravelCheckResult(60, 30, null);
        Assert.False(result.IsTight);
    }
}
