namespace HomeStream.Application.Common;

public static class HomeStreamDateTime
{
    public static long EpochNow => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    public static DateTime ToDateTime(this long epoch) => DateTimeOffset.FromUnixTimeSeconds(epoch).DateTime;
    public static string ToDateTimeString(this long epoch) => epoch.ToDateTime().ToString();
}
