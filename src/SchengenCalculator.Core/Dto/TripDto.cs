namespace SchengenCalculator.Core.Dto;

/// <summary>Data transfer objects for Trip API endpoints.</summary>

public record TripDto(
    Guid Id,
    DateTime StartDate,
    DateTime EndDate,
    string? DepartureAirport,
    string? ArrivalAirport,
    string? Country);

public record CreateTripRequest(
    DateTime StartDate,
    DateTime EndDate,
    string? DepartureAirport,
    string? ArrivalAirport,
    string? Country);

public record UpdateTripRequest(
    DateTime StartDate,
    DateTime EndDate,
    string? DepartureAirport,
    string? ArrivalAirport,
    string? Country);
