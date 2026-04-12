using BetaCinema.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart?> GetActiveCartByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<Cart?> GetCartWithItemsAsync(Guid cartId, CancellationToken ct = default);

    }
}
