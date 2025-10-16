using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class RankCustomerRepository(IMemoryCache memoryCache,AppDbContext context) : BaseRepository<RankCustomer>(context), IRankCustomerRepository
    {
        private const string RankCacheKey = "AllRanksSorted";
        private readonly IMemoryCache _cache = memoryCache;
        /*public async Task<RankCustomer?> GetDefaultRankAsync()
        => await _context.RankCustomers.FirstOrDefaultAsync(r => r.Name == "Rank1");*/
        public async Task<IEnumerable<RankCustomer>?> GetAllRankCustomersDesAsync()
        {

            if (_cache.TryGetValue(RankCacheKey, out List<RankCustomer>? ranks))
            {
                return ranks;
            }

            ranks = await _context.RankCustomers
            .OrderByDescending(x => x.MinimumPoint)
            .AsNoTracking()
            .ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
           .SetAbsoluteExpiration(TimeSpan.FromHours(24));

            _cache.Set(RankCacheKey, ranks, cacheEntryOptions);

            return ranks;
        }
   
    }
}
