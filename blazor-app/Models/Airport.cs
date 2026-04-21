using System.Text.Json.Serialization;

namespace SchengenCalculator.Models;

public class Airport
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("city")]
    public string City { get; set; } = "";

    [JsonPropertyName("country")]
    public string Country { get; set; } = "";

    [JsonPropertyName("isCustom")]
    public bool IsCustom { get; set; }

    public Airport() { }

    public Airport(string code, string name, string city, string country, bool isCustom = false)
    {
        Code = code; Name = name; City = city; Country = country; IsCustom = isCustom;
    }

    /// <summary>Short display: "LHR – London Heathrow"</summary>
    public string DisplayName => $"{Code} – {Name}";

    /// <summary>Full display with city: "LHR – London Heathrow, London"</summary>
    public string FullDisplay => $"{Code} – {Name}, {City}";

    public override string ToString() => DisplayName;
}
