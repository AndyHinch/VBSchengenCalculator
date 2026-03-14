using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchengenCalculator.Api.Data;
using SchengenCalculator.Core;
using SchengenCalculator.Core.Dto;

namespace SchengenCalculator.Api.Controllers;

[ApiController]
[Route("api/trips")]
[Authorize]
public class TripsController(
    AppDbContext db,
    UserManager<AppUser> userManager) : ControllerBase
{
    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<ActionResult<List<TripDto>>> GetAll()
    {
        var trips = await db.Trips
            .Where(t => t.UserId == UserId)
            .OrderBy(t => t.StartDate)
            .ToListAsync();

        return trips.Select(Map).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<TripDto>> Create(CreateTripRequest request)
    {
        var user = await userManager.FindByIdAsync(UserId);
        if (user is null) return Unauthorized();

        var limit = AppConfig.TripLimitFor(user.Tier);
        var count = await db.Trips.CountAsync(t => t.UserId == UserId);

        if (count >= limit)
            return BadRequest(new { message = $"Trip limit of {limit} reached for your account tier." });

        var trip = new TripEntity
        {
            UserId           = UserId,
            StartDate        = request.StartDate,
            EndDate          = request.EndDate,
            DepartureAirport = request.DepartureAirport,
            ArrivalAirport   = request.ArrivalAirport,
            Country          = request.Country,
        };

        db.Trips.Add(trip);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = trip.Id }, Map(trip));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TripDto>> GetById(Guid id)
    {
        var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
        return trip is null ? NotFound() : Map(trip);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TripDto>> Update(Guid id, UpdateTripRequest request)
    {
        var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
        if (trip is null) return NotFound();

        trip.StartDate        = request.StartDate;
        trip.EndDate          = request.EndDate;
        trip.DepartureAirport = request.DepartureAirport;
        trip.ArrivalAirport   = request.ArrivalAirport;
        trip.Country          = request.Country;

        await db.SaveChangesAsync();
        return Map(trip);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var trip = await db.Trips.FirstOrDefaultAsync(t => t.Id == id && t.UserId == UserId);
        if (trip is null) return NotFound();

        db.Trips.Remove(trip);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        var trips = db.Trips.Where(t => t.UserId == UserId);
        db.Trips.RemoveRange(trips);
        await db.SaveChangesAsync();
        return NoContent();
    }

    private static TripDto Map(TripEntity t) =>
        new(t.Id, t.StartDate, t.EndDate, t.DepartureAirport, t.ArrivalAirport, t.Country);
}
