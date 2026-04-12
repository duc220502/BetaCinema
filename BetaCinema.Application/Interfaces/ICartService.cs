using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Cart;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface ICartService
    {
        Task<ResponseObject<CartResponseDto>> GetHeaderCartAsync(CancellationToken ct = default);
        Task<ResponseObject<CartItemDto>> AddFoodToCartAsync( AddFoodToCartRequest request, CancellationToken ct = default);

        Task<ResponseObject<CartItemDto>> RemoveCartItemAsync(Guid cartItemId, CancellationToken ct = default);
    }
}
