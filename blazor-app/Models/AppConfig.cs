namespace SchengenCalculator.Models;

public static class AppConfig
{
    public const int AnonymousTripLimit = 4;
    public const int FreeTierTripLimit  = 10;
    public const int PaidTierTripLimit  = int.MaxValue;

    public static int TripLimitFor(UserTier tier) => tier switch
    {
        UserTier.Anonymous => AnonymousTripLimit,
        UserTier.Free      => FreeTierTripLimit,
        UserTier.Paid      => PaidTierTripLimit,
        _                  => AnonymousTripLimit
    };
}

public enum UserTier { Anonymous, Free, Paid }
