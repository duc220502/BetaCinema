using BetaCinema.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface ICartFoodItemRepository : IRepository<CartFoodItem>
    {
        Task<CartFoodItem?> GetByCartAndFoodAsync(Guid cartId, Guid foodId, CancellationToken ct = default);

    }
}
