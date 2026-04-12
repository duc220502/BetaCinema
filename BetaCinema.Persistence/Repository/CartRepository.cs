using BetaCinema.Domain.Entities.Carts;
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
    public class CartRepository(AppDbContext context) : BaseRepository<Cart>(context), ICartRepository
    {
        public Task<Cart?> GetActiveCartByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return _context.Carts.FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive, ct);
        }

        public Task<Cart?> GetCartWithItemsAsync(Guid cartId, CancellationToken ct = default)
        => _context.Carts.Include(x => x.CartFoodItems)
            .ThenInclude(x => x.Food)
           .FirstOrDefaultAsync(x => x.Id == cartId, ct);
    
}
}
