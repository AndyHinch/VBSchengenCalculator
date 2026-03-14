using System.Text.Json.Serialization;

namespace SchengenCalculator.Models;

public class TripDate
{
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("departureAirport")]
    public string? DepartureAirport { get; set; }   // IATA code e.g. "LHR"

    [JsonPropertyName("arrivalAirport")]
    public string? ArrivalAirport { get; set; }     // IATA code e.g. "BCN"

    [JsonPropertyName("country")]
    public string? Country { get; set; }            // e.g. "Spain"

    // Parameterless constructor required for JSON deserialisation
    public TripDate() { }

    public TripDate(DateTime startDate, DateTime endDate,
                    string? departureAirport = null, string? arrivalAirport = null, string? country = null)
    {
        StartDate = startDate;
        EndDate = endDate;
        DepartureAirport = departureAirport;
        ArrivalAirport = arrivalAirport;
        Country = country;
    }

    /// <summary>Number of days inclusive of start and end.</summary>
    public int NumberOfDays() => (int)(EndDate - StartDate).TotalDays + 1;

    public bool WasInAreaOnDate(DateTime date) => date >= StartDate && date <= EndDate;

    public string RouteDisplay
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(DepartureAirport) || !string.IsNullOrEmpty(ArrivalAirport))
                parts.Add($"{DepartureAirport ?? "?"} → {ArrivalAirport ?? "?"}");
            if (!string.IsNullOrEmpty(Country))
                parts.Add(Country);
            return parts.Count > 0 ? string.Join("  ·  ", parts) : "";
        }
    }

    public override string ToString() =>
        $"{StartDate.ToShortDateString()} – {EndDate.ToShortDateString()} ({NumberOfDays()} days)";
}
