using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Movies;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class MovieProfile : Profile
    {
        public MovieProfile() 
        {
            CreateMap<Movie, DataResponseMovie>();

            CreateMap<Request_UpdateMovie, Movie>()
          .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
          .ForMember(dest => dest.Trailer, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Trailer)))
          .ForMember(dest => dest.MovieDuration, opt => opt.Condition(src => src.MovieDuration.HasValue))

          .ForMember(dest => dest.PremiereDate, opt => opt.Condition(src => src.PremiereDate.HasValue))
          .ForMember(dest => dest.BasePrice, opt => opt.Condition(src => src.BasePrice.HasValue))

          .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
          .ForMember(dest => dest.Director, opt => opt.Condition(src => src.Director != null))
          .ForMember(dest => dest.HeroImage, opt => opt.Condition(src => src.HeroImage != null))
          .ForMember(dest => dest.Language, opt => opt.Condition(src => src.Language != null))
          .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue))
          .ForMember(dest => dest.RateId, opt => opt.Condition(src => src.RateId.HasValue))
          .ForMember(dest => dest.MovieTypeId, opt => opt.Condition(src => src.MovieTypeId.HasValue));


        }
    }
}
