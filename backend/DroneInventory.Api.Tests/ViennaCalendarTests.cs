using System.Globalization;
using System.Text.Json;
using DroneInventory.Api.Time;

namespace DroneInventory.Api.Tests;

public sealed class ViennaCalendarTests
{
    [Theory]
    [InlineData("2026-01-18T22:59:59Z", "2026-01-18")]
    [InlineData("2026-01-18T23:00:00Z", "2026-01-19")]
    [InlineData("2026-08-18T21:59:59Z", "2026-08-18")]
    [InlineData("2026-08-18T22:00:00Z", "2026-08-19")]
    [InlineData("2026-08-18T22:30:00Z", "2026-08-19")]
    [InlineData("2026-08-19T00:00:00Z", "2026-08-19")]
    public void UsesViennaCalendarDayAndSerializesAsIsoDate(string utc, string expected)
    {
        var instant = DateTimeOffset.Parse(utc, CultureInfo.InvariantCulture);
        var calendar = new ViennaCalendar(new FixedTimeProvider(instant));

        var date = calendar.Today();

        Assert.Equal(DateOnly.ParseExact(expected, "yyyy-MM-dd", CultureInfo.InvariantCulture), date);
        Assert.Equal(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(date));
    }

    private sealed class FixedTimeProvider(DateTimeOffset instant) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => instant;
    }
}
