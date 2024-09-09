namespace HomeStream.Domain.Abstractions.Services;

public interface ICacheService
{
    public Task<bool> SetCacheItemAsync<ICacheType>(string key, ICacheType value, int? expiryInSeconds = null);

    public Task<ICacheType?> GetCachedItemAsync<ICacheType>(string key);

    public Task<bool> RemoveCachedItemAsync(string key);

    public Task<bool> ReloadCachedItemAsync<ICacheType>(string key, ICacheType newValue, int? expiryInSeconds = null);
}