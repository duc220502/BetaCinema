using BetaCinema.Domain.Entities.Carts;
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
    public class CartFoodItemRepository(AppDbContext context) : BaseRepository<CartFoodItem>(context), ICartFoodItemRepository
    {
        public Task<CartFoodItem?> GetByCartAndFoodAsync(Guid cartId, Guid foodId, CancellationToken ct = default)
        => _context.CartFoodItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.FoodId == foodId, ct);


    }
}
