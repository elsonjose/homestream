namespace HomeStream.Domain.Abstractions.Services;

/// <summary>
/// Defines the cache service.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Sets the cache item
    /// </summary>
    /// <typeparam name="ICacheType">The type of item</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="value">The item value</param>
    /// <param name="expiryInSeconds">The expiration in seconds</param>
    /// <returns>True if cache is set, else false.</returns>
    public Task<bool> SetCacheItemAsync<ICacheType>(string key, ICacheType value, int? expiryInSeconds = null);

    /// <summary>
    /// Gets the cached item.
    /// </summary>
    /// <typeparam name="ICacheType">The type of item</typeparam>
    /// <param name="key">The cache key</param>
    /// <returns>The cache item of type <seealso cref="ICacheType"/></returns>
    public Task<ICacheType?> GetCachedItemAsync<ICacheType>(string key);

    /// <summary>
    /// Removes the cached item.
    /// </summary>
    /// <param name="key">The cache key</param>
    /// <returns>True if cache item is removed, else false.</returns>
    public Task<bool> RemoveCachedItemAsync(string key);

    /// <summary>
    /// Reloads the cache item by calling <seealso cref="RemoveCachedItemAsync(string)"/> and <seealso cref="SetCacheItemAsync{ICacheType}(string, ICacheType, int?)"/>
    /// <typeparam name="ICacheType">The type of item</typeparam>
    /// <param name="key">The cache key</param>
    /// <param name="newValue">The new item value</param>
    /// <param name="expiryInSeconds">The expiration in seconds</param>
    /// <returns>True if cache is set, else false.</returns>
    public Task<bool> ReloadCachedItemAsync<ICacheType>(string key, ICacheType newValue, int? expiryInSeconds = null);
}