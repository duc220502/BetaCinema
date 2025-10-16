using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class PromotionRepository(AppDbContext context) : BaseRepository<Promotion>(context), IPromotionRepository
    {
        public async Task<List<Promotion>?> GetPromotionsByListIdAsync(List<Guid> ids)
        => await _context.Promotions
        .Where(p => ids.Contains(p.Id)).ToListAsync();
        
    }
}
