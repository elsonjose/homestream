namespace HomeStream.Application.Common;

public static class HomeStreamDateTime
{
    public static long EpochNow => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
}
