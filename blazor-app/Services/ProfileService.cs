using System.Text.Json;
using Microsoft.JSInterop;
using SchengenCalculator.Models;

namespace SchengenCalculator.Services;

/// <summary>
/// Manages multiple traveller profiles, each with their own trip history.
/// Profiles are persisted in localStorage.
/// </summary>
public class ProfileService
{
    private readonly IJSRuntime _js;
    private const string StorageKey = "schengen_profiles";
    private const string ActiveKey  = "schengen_active_profile";

    private List<TravellerProfile> _profiles = [];
    private string? _activeId;
    private bool _initialised;

    public event Action? OnChanged;

    public ProfileService(IJSRuntime js) => _js = js;

    public IReadOnlyList<TravellerProfile> Profiles => _profiles.AsReadOnly();

    public TravellerProfile? ActiveProfile =>
        _profiles.FirstOrDefault(p => p.Id == _activeId) ?? _profiles.FirstOrDefault();

    public async Task EnsureInitialisedAsync()
    {
        if (_initialised) return;

        var json = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (!string.IsNullOrEmpty(json))
        {
            try { _profiles = JsonSerializer.Deserialize<List<TravellerProfile>>(json) ?? []; }
            catch { _profiles = []; }
        }

        // Seed a default profile if none exist
        if (_profiles.Count == 0)
        {
            _profiles.Add(new TravellerProfile { Name = "My Profile" });
            await SaveAsync();
        }

        _activeId = await _js.InvokeAsync<string?>("localStorage.getItem", ActiveKey)
                   ?? _profiles[0].Id;

        _initialised = true;
    }

    public async Task SwitchProfileAsync(string id)
    {
        _activeId = id;
        await _js.InvokeVoidAsync("localStorage.setItem", ActiveKey, id);
        OnChanged?.Invoke();
    }

    public async Task AddProfileAsync(string name)
    {
        var p = new TravellerProfile { Name = name.Trim() };
        _profiles.Add(p);
        await SaveAsync();
        await SwitchProfileAsync(p.Id);
    }

    public async Task RenameActiveAsync(string name)
    {
        if (ActiveProfile is null) return;
        ActiveProfile.Name = name.Trim();
        await SaveAsync();
        OnChanged?.Invoke();
    }

    public async Task DeleteActiveAsync()
    {
        if (_profiles.Count <= 1 || ActiveProfile is null) return;
        _profiles.Remove(ActiveProfile);
        _activeId = _profiles[0].Id;
        await SaveAsync();
        OnChanged?.Invoke();
    }

    /// <summary>Replaces the active profile's trip list and persists.</summary>
    public async Task SaveTripsAsync(List<TripDate> trips)
    {
        if (ActiveProfile is null) return;
        ActiveProfile.Trips = trips;
        await SaveAsync();
    }

    private async Task SaveAsync()
    {
        var json = JsonSerializer.Serialize(_profiles);
        await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }
}
