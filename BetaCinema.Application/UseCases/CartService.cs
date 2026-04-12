using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Cart;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Carts;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class CartService(ICartRepository cartRepository , ICartFoodItemRepository cartFoodItemRepository ,
        IFoodRepository foodRepository, IUnitOfWork unitOfWork, IMapper mapper,ICurrentUserservice currentUserservice) : ICartService
    {
        private readonly ICartRepository _cartRepository = cartRepository;
        private readonly ICartFoodItemRepository _cartFoodItemRepository = cartFoodItemRepository;
        private readonly IFoodRepository _foodRepository = foodRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserservice _currentUserservice  = currentUserservice;


        public async Task<ResponseObject<CartItemDto>> AddFoodToCartAsync( AddFoodToCartRequest request, CancellationToken ct = default)
        {
            var userId = _currentUserservice.GetRequiredUserId();

            var cart = await GetOrCreateActiveCartAsync(userId, ct);

            var cartFoodItem = await _cartFoodItemRepository.GetByCartAndFoodAsync(cart.Id, request.FoodId, ct);


            if (cartFoodItem is not null)
            {
                cartFoodItem.Quantity += request.Quantity;

            }
           
            else
            {
                cartFoodItem = new CartFoodItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    FoodId = request.FoodId,
                    Quantity = request.Quantity,

                    CreatedAt = DateTime.UtcNow
                };

                _cartFoodItemRepository.Add(cartFoodItem);
            }

            await _unitOfWork.SaveChangesAsync(ct);

            return ResponseObject<CartItemDto>.ResponseSuccess("Thêm item food vào giỏ hàng thành công", _mapper.Map<CartItemDto>(cartFoodItem));

           
        }

        public async Task<ResponseObject<CartResponseDto>> GetHeaderCartAsync( CancellationToken ct = default)
        {
            var userId = _currentUserservice.GetRequiredUserId();

            var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId, ct)??throw new NotFoundAppException("Không tìm thấy cart")  ;

            cart = await _cartRepository.GetCartWithItemsAsync(cart.Id, ct) ?? cart;

            var result = new CartResponseDto();

            foreach (var foodItem in cart.CartFoodItems)
            {
                result.Items.Add(new CartItemDto
                {
                    Id = foodItem.Id,
                    DisplayName = foodItem.Food?.Name ?? "Food",
                    Quantity = foodItem.Quantity,
                    UnitPrice = foodItem.Food?.Price ?? 0
                });
            }

            result.Count = result.Items.Sum(x => x.Quantity);
            result.Subtotal = result.Items.Sum(x => x.TotalPrice);

            return ResponseObject<CartResponseDto>.ResponseSuccess("Lấy dữ  liệu cart thành công", result);


        }

        public async Task<ResponseObject<CartItemDto>> RemoveCartItemAsync( Guid cartItemId, CancellationToken ct = default)
        {
            var userId = _currentUserservice.GetRequiredUserId();

            var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId, ct)
                   ?? throw new Exception("Không tìm thấy giỏ hàng.");

            cart = await _cartRepository.GetCartWithItemsAsync(cart.Id, ct)
               ?? throw new Exception("Không tìm thấy giỏ hàng.");

            var foodItem = cart.CartFoodItems.FirstOrDefault(x => x.Id == cartItemId);
            if (foodItem is not null)
            {
                _cartFoodItemRepository.Delete(foodItem);
                await _unitOfWork.SaveChangesAsync(ct);

                return ResponseObject<CartItemDto>.ResponseSuccess("Xóa item cart thành công", _mapper.Map<CartItemDto>(foodItem));
            }


            throw new NotImplementedException();
        }

        private async Task<Cart> GetOrCreateActiveCartAsync(Guid userId, CancellationToken ct)
        {
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(userId, ct);
            if (cart is not null) return cart;

            cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsActive = true
            };

            _cartRepository.Add(cart);
            await _unitOfWork.SaveChangesAsync(ct);

            return cart;
        }
    }
}
