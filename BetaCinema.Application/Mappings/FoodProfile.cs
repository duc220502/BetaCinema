using AutoMapper;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataRequest.Foods;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class FoodProfile : Profile
    {
        public FoodProfile()
        {
            CreateMap<Food, DataResponseFood>();

            CreateMap<Request_UpdateFood, Food>()
              .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
              .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price.HasValue))
              .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
              .ForMember(dest => dest.Image, opt => opt.Condition(src => src.Image != null))
              .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

        }
    }
}
