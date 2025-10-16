using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class RoomProfile : Profile
    {
        public RoomProfile() 
        { 
            CreateMap<Room,DataResponseRoom>();

            CreateMap<Request_UpdateRoom, Room>()
          .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
          .ForMember(dest => dest.Capacity, opt => opt.Condition(src => src.Capacity.HasValue))

          .ForMember(dest => dest.RoomType, opt => opt.Condition(src => src.RoomType.HasValue))

          .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))

          .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));
        }
    }
}
