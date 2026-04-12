using AutoMapper;
using BetaCinema.Application.DTOs.Cart;
using BetaCinema.Domain.Entities.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class CartFoodItemProfile : Profile
    {
        public CartFoodItemProfile()
        {
            CreateMap<CartFoodItem, CartItemDto>()
           .ForMember(dest => dest.DisplayName,
               opt => opt.MapFrom(src => src.Food != null ? src.Food.Name : string.Empty))
           .ForMember(dest => dest.UnitPrice,
               opt => opt.MapFrom(src => src.Food != null ? src.Food.Price : 0));
        }
    }
}
