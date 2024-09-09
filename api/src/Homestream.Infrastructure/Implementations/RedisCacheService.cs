using HomeStream.Domain.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HomeStream.Infrastructure.Implementations;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _redisDb;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
    {
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }

    public async Task<ICacheType?> GetCachedItemAsync<ICacheType>(string key)
    {
        var rediCacheItem = await _redisDb.StringGetAsync(key);
        _logger.LogInformation("Getting cache item for key: {key} with value:{value}", key, rediCacheItem);
        return rediCacheItem.HasValue ? JsonConvert.DeserializeObject<ICacheType>(rediCacheItem.ToString()) : default;
    }

    public async Task<bool> ReloadCachedItemAsync<ICacheType>(string key, ICacheType newValue, int? expiryInSeconds = null)
    {
        await RemoveCachedItemAsync(key);
        return await SetCacheItemAsync(key, newValue, expiryInSeconds);
    }

    public async Task<bool> RemoveCachedItemAsync(string key)
    {
        _logger.LogInformation("Removing cache item for key: {key}", key);
        return await _redisDb.KeyDeleteAsync(key);
    }

    public async Task<bool> SetCacheItemAsync<ICacheType>(string key, ICacheType value, int? expiryInSeconds = null)
    {
        await RemoveCachedItemAsync(key);
        var serializedCache = JsonConvert.SerializeObject(value);
        _logger.LogInformation("Setting cache item for key: {key} with value:{value}", key, value);
        return await _redisDb.StringSetAsync(key, serializedCache, GetExpiryInSeconds(expiryInSeconds));
    }

    private static TimeSpan? GetExpiryInSeconds(int? expiryInSeconds)
    {
        return expiryInSeconds.HasValue ? TimeSpan.FromSeconds(expiryInSeconds.Value) : null;
    }
}