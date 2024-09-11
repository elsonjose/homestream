namespace HomeStream.Application.Common;

public static class HomeStreamConstants
{
    public const int JobExecutionDelay = 20000;
    public static readonly IReadOnlyList<string> AllowedVideoExtenstions = [".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv"];
    public static readonly string RedisCacheKeyForJob = "Homestream-Job#{0}";
}
