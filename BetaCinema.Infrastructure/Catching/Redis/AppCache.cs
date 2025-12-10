using BetaCinema.Application.Interfaces.Catching;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Catching.Redis
{
    public class AppCache(IDistributedCache cache) : IAppCache
    {
        private readonly IDistributedCache _cache = cache;

        static readonly JsonSerializerOptions J = new(JsonSerializerDefaults.Web);
        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan ttl)
        {
            var s = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(s)) return JsonSerializer.Deserialize<T>(s, J);
            var v = await factory();
            if (v is not null)
                await _cache.SetStringAsync(key, JsonSerializer.Serialize(v, J),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = ttl });
            return v;
        }

        public Task RemoveAsync(string key)
         => _cache.RemoveAsync(key);
    }
}
