using System.Net.Http.Json;
using System.Text.Json;
using SchengenCalculator.Models;

namespace SchengenCalculator.Services;

/// <summary>
/// Syncs trips with the backend API when the user is authenticated.
/// Falls back to no-op when anonymous (caller uses localStorage via ProfileService).
/// </summary>
public class TripApiService(HttpClient http)
{
    private static readonly JsonSerializerOptions JsonOpts =
        new() { PropertyNameCaseInsensitive = true };

    public async Task<List<TripDate>> GetTripsAsync()
    {
        var dtos = await http.GetFromJsonAsync<List<TripDto>>("api/trips", JsonOpts) ?? [];
        return dtos.Select(ToModel).ToList();
    }

    public async Task<TripDate?> AddTripAsync(TripDate trip)
    {
        var req  = new CreateTripRequest(trip.StartDate, trip.EndDate,
                                         trip.DepartureAirport, trip.ArrivalAirport, trip.Country);
        var resp = await http.PostAsJsonAsync("api/trips", req);
        if (!resp.IsSuccessStatusCode) return null;
        var dto = await resp.Content.ReadFromJsonAsync<TripDto>(JsonOpts);
        return dto is null ? null : ToModel(dto);
    }

    public async Task<bool> UpdateTripAsync(Guid id, TripDate trip)
    {
        var req  = new UpdateTripRequest(trip.StartDate, trip.EndDate,
                                          trip.DepartureAirport, trip.ArrivalAirport, trip.Country);
        var resp = await http.PutAsJsonAsync($"api/trips/{id}", req);
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTripAsync(Guid id)
    {
        var resp = await http.DeleteAsync($"api/trips/{id}");
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAllTripsAsync()
    {
        var resp = await http.DeleteAsync("api/trips");
        return resp.IsSuccessStatusCode;
    }

    private static TripDate ToModel(TripDto d) =>
        new(d.StartDate, d.EndDate, d.DepartureAirport, d.ArrivalAirport, d.Country)
        {
            ApiId = d.Id
        };
}
