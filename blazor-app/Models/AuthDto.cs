namespace SchengenCalculator.Models;

public record RegisterRequest(string Email, string Password, string DisplayName);
public record LoginRequest(string Email, string Password);
public record AuthResponse(
    string Token,
    string Email,
    string DisplayName,
    UserTier Tier,
    int TripLimit);
