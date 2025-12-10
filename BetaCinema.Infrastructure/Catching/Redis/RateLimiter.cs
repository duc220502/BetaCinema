using BetaCinema.Application.Interfaces.Catching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Catching.Redis
{
    public sealed class RateLimiter(IConnectionMultiplexer mux) : IRateLimiter
    {
        readonly IDatabase _db = mux.GetDatabase();
        public async Task<bool> HitAsync(string route, string subject, int limit, TimeSpan window)
        {
            var bucket = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmm"); // 1 phút
            var key = $"rl:{route}:{subject}:{bucket}";
            var n = await _db.StringIncrementAsync(key);
            if (n == 1) await _db.KeyExpireAsync(key, window);
            return n <= limit;
        }
    }
}
