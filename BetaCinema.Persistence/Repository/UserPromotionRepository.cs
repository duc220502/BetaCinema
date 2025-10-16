using BetaCinema.Domain.Entities.Users;
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
    public class UserPromotionRepository(AppDbContext context) : BaseRepository<UserPromotion>(context), IUserPromotionRepository
    {
        public async Task<UserPromotion?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
        => await _context.UserPromotions.Include(u => u.Promotion).FirstOrDefaultAsync(u => u.Id == id && u.IsActive , ct);
    }
}
