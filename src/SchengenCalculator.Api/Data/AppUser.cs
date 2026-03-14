using Microsoft.AspNetCore.Identity;
using SchengenCalculator.Core;

namespace SchengenCalculator.Api.Data;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
    public UserTier Tier { get; set; } = UserTier.Free;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public ICollection<TripEntity> Trips { get; set; } = [];
}
