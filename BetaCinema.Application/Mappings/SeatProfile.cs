using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Seats;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class SeatProfile : Profile
    {
        public SeatProfile() 
        {
            CreateMap<Seat, DataResponseSeat>();

            CreateMap<Request_AddSeat, Seat>().ForMember(dest => dest.Line, opt =>
              opt.MapFrom(src => Enum.Parse<LineSeat>(src.Line, true))
            );

            CreateMap<Request_UpdateSeat, Seat>()
           .ForMember(dest => dest.Line, opt =>
           {
               opt.Condition(src => !string.IsNullOrEmpty(src.Line));
               opt.MapFrom(src => Enum.Parse<LineSeat>(src.Line!, true));
           })
           .ForMember(dest => dest.Number, opt => opt.Condition(src => src.Number.HasValue));
        }
    }
}
