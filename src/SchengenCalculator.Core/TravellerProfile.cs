using System.Text.Json.Serialization;

namespace SchengenCalculator.Core.Models;

public class TravellerProfile
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("trips")]
    public List<TripDate> Trips { get; set; } = [];
}
