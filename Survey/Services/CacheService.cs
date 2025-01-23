
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Survey.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);
            return String.IsNullOrEmpty(cachedValue) ?
                 JsonSerializer.Deserialize<T>(cachedValue!) : null;
        }
        public async Task SetAsync<T>(string key, T Value, CancellationToken cancellationToken = default) where T : class
        {
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(Value), cancellationToken); 
        }
        public async Task Remove(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}
