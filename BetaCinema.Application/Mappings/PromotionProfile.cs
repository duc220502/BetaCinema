using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Promotions;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class PromotionProfile : Profile
    {
        public PromotionProfile() 
        {
            CreateMap<Promotion, DataResponsePromotion>();

            CreateMap<Request_UpdatePromotion, Promotion>()

              .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
              .ForMember(dest => dest.DiscountValue, opt => opt.Condition(src => src.DiscountValue.HasValue))

              .ForMember(dest => dest.MinBillValue, opt => opt.Condition(src => src.MinBillValue.HasValue))

              .ForMember(dest => dest.MaxDiscountValue, opt => opt.Condition(src => src.MaxDiscountValue.HasValue))

              .ForMember(dest => dest.UsageLimit, opt => opt.Condition(src => src.UsageLimit.HasValue))

              .ForMember(dest => dest.StartTime, opt => opt.Condition(src => src.StartTime.HasValue))

              .ForMember(dest => dest.EndTime, opt => opt.Condition(src => src.EndTime.HasValue));

        }
    }
}
