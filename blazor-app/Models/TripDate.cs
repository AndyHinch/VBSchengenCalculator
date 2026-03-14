using System.Text.Json.Serialization;

namespace SchengenCalculator.Models;

public class TripDate
{
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    // Parameterless constructor required for JSON deserialisation
    public TripDate() { }

    public TripDate(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    /// <summary>Number of days inclusive of start and end.</summary>
    public int NumberOfDays() => (int)(EndDate - StartDate).TotalDays + 1;

    public bool WasInAreaOnDate(DateTime date) => date >= StartDate && date <= EndDate;

    public override string ToString() =>
        $"{StartDate.ToShortDateString()} to {EndDate.ToShortDateString()} ({NumberOfDays()} days)";
}
