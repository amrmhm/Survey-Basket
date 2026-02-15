
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurveyBasket.Api.Services;

public class CacheServices(IDistributedCache distributedCache, ILogger<CacheServices> logger) : ICacheServices
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly ILogger<CacheServices> _logger = logger;

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Get Async ");

        var cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(cacheValue))
            return null;
        return JsonSerializer.Deserialize<T>(cacheValue);

    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Set Async ");
        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Remove Async ");


        await _distributedCache.RemoveAsync(key, cancellationToken);
    }
}
