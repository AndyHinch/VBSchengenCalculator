using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using SchengenCalculator.Models;

namespace SchengenCalculator.Services;

/// <summary>
/// Manages authentication state for the Blazor WASM client.
/// Stores the JWT token in localStorage and exposes the current user tier/limits.
/// </summary>
public class AuthService(HttpClient http, IJSRuntime js)
{
    private const string TokenKey = "schengen_token";
    private const string UserKey  = "schengen_user";

    public AuthResponse? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser is not null;

    public UserTier Tier  => CurrentUser?.Tier ?? UserTier.Anonymous;
    public int TripLimit  => CurrentUser?.TripLimit ?? AppConfig.AnonymousTripLimit;

    public event Action? OnAuthChanged;

    public async Task InitialiseAsync()
    {
        var json = await js.InvokeAsync<string?>("localStorage.getItem", UserKey);
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                CurrentUser = JsonSerializer.Deserialize<AuthResponse>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                SetAuthHeader();
            }
            catch { await LogoutAsync(); }
        }
    }

    public async Task<string?> RegisterAsync(RegisterRequest request)
    {
        var resp = await http.PostAsJsonAsync("api/auth/register", request);
        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadFromJsonAsync<ErrorResponse>();
            if (err?.Message is not null) return err.Message;
            if (err?.Errors is not null) return string.Join(" ", err.Errors);
            return "Registration failed.";
        }
        var auth = await resp.Content.ReadFromJsonAsync<AuthResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        await PersistAsync(auth!);
        return null; // null = success
    }

    public async Task<string?> LoginAsync(LoginRequest request)
    {
        var resp = await http.PostAsJsonAsync("api/auth/login", request);
        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadFromJsonAsync<ErrorResponse>();
            return err?.Message ?? "Login failed.";
        }
        var auth = await resp.Content.ReadFromJsonAsync<AuthResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        await PersistAsync(auth!);
        return null;
    }

    public async Task LogoutAsync()
    {
        CurrentUser = null;
        http.DefaultRequestHeaders.Authorization = null;
        await js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await js.InvokeVoidAsync("localStorage.removeItem", UserKey);
        OnAuthChanged?.Invoke();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private async Task PersistAsync(AuthResponse auth)
    {
        CurrentUser = auth;
        SetAuthHeader();
        await js.InvokeVoidAsync("localStorage.setItem", TokenKey, auth.Token);
        await js.InvokeVoidAsync("localStorage.setItem", UserKey,
            JsonSerializer.Serialize(auth));
        OnAuthChanged?.Invoke();
    }

    private void SetAuthHeader()
    {
        if (CurrentUser is not null)
            http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", CurrentUser.Token);
    }

    private record ErrorResponse(string? Message, IEnumerable<string>? Errors);
}
