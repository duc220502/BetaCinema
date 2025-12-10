using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Catching
{
    public interface IAppCache
    {
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T?>> factory, TimeSpan ttl);
        Task RemoveAsync(string key);
    }
}
