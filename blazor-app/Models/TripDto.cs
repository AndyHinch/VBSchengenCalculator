namespace SchengenCalculator.Models;

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
