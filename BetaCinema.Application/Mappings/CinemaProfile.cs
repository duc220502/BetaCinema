using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class CinemaProfile : Profile
    {
        public CinemaProfile()
        {
            CreateMap<Cinema, DataResponseCinema>();
                


            CreateMap<Request_UpdateCinema, Cinema>()
           .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
           .ForMember(dest => dest.Address, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Address)))

           .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))

           .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

        }


    }
}
