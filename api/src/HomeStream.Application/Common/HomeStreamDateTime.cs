namespace HomeStream.Application.Common;

public static class HomeStreamDateTime
{
    public static long EpochNow => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    public static DateTime ToDateTime(this long epoch) => DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime;
    public static string ToDateTimeString(this long epoch) => epoch.ToDateTime().ToString();

    public static string ToHumanReadableTimeSpan(this TimeSpan timeSpan) => string.Format("{0:D2} hours, {1:D2} minutes, {2:D2} seconds",
                                      timeSpan.Hours,
                                      timeSpan.Minutes,
                                      timeSpan.Seconds);
}
