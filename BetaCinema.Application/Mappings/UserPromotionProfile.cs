using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.UserPromotions;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class UserPromotionProfile :Profile
    {
        public UserPromotionProfile() 
        {
            CreateMap<UserPromotion, DataResponseUserPromotionPersonal>();

            CreateMap<Request_UpdateUserPromotion, UserPromotion>()
            .ForMember(dest => dest.Quantity, opt => opt.Condition(src => src.Quantity.HasValue));
        }
    }
}
