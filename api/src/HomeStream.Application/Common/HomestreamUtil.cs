namespace HomeStream.Application.Common;

public static class HomestreamUtil
{
    public static string GetRedisNameForJob(long jobId)
    {
        return string.Format(HomeStreamConstants.RedisCacheKeyForJob, jobId);
    }
}
