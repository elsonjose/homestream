using HomeStream.Domain.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace HomeStream.Infrastructure.Implementations;

/// <summary>
/// Defines the <seealso cref="ICacheService"/> implementation for redis-server.
/// </summary>
public class RedisCacheService : ICacheService
{
    /// <inheritdoc/>
    private readonly IDatabase _redisDb;

    /// <inheritdoc/>
    private readonly ILogger<RedisCacheService> _logger;

    /// <summary>
    /// Initializes a new instance of <seealso cref="RedisCacheService"/>
    /// </summary>
    /// <param name="redis">The redis multiplexer API</param>
    /// <param name="logger">The logger instance</param>
    public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
    {
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ICacheType?> GetCachedItemAsync<ICacheType>(string key)
    {
        var rediCacheItem = await _redisDb.StringGetAsync(key);
        _logger.LogInformation("Getting cache item for key: {key} with value: {value}", key, rediCacheItem);
        return rediCacheItem.HasValue ? JsonConvert.DeserializeObject<ICacheType>(rediCacheItem.ToString()) : default;
    }

    /// <inheritdoc/>
    public async Task<bool> ReloadCachedItemAsync<ICacheType>(string key, ICacheType newValue, int? expiryInSeconds = null)
    {
        await RemoveCachedItemAsync(key);
        return await SetCacheItemAsync(key, newValue, expiryInSeconds);
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveCachedItemAsync(string key)
    {
        _logger.LogInformation("Removing cache item for key: {key}", key);
        return await _redisDb.KeyDeleteAsync(key);
    }

    /// <inheritdoc/>
    public async Task<bool> SetCacheItemAsync<ICacheType>(string key, ICacheType value, int? expiryInSeconds = null)
    {
        await RemoveCachedItemAsync(key);
        var serializedCache = JsonConvert.SerializeObject(value);
        _logger.LogInformation("Setting cache item for key: {key} with value: {value}", key, value);
        return await _redisDb.StringSetAsync(key, serializedCache, GetExpiryInSeconds(expiryInSeconds));
    }

    /// <summary>
    /// Gets the expiry in seconds.
    /// </summary>
    /// <param name="expiryInSeconds">The expiry in seconds</param>
    /// <returns>Timespan equivalent of seconds to expire if valid expiryInSeconds argument else null.</returns>
    private static TimeSpan? GetExpiryInSeconds(int? expiryInSeconds)
    {
        return expiryInSeconds.HasValue ? TimeSpan.FromSeconds(expiryInSeconds.Value) : null;
    }
}