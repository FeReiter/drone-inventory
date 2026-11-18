namespace DroneInventory.Api.Time;

public sealed class ViennaCalendar(TimeProvider timeProvider)
{
    private static readonly TimeZoneInfo Vienna = TimeZoneInfo.FindSystemTimeZoneById("Europe/Vienna");

    public DateOnly Today()
    {
        var localTime = TimeZoneInfo.ConvertTime(timeProvider.GetUtcNow(), Vienna);
        return DateOnly.FromDateTime(localTime.DateTime);
    }
}
