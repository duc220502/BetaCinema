using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.Schedule;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class ScheduleProfile : Profile
    {
        public ScheduleProfile() 
        {
            CreateMap<Schedule, DataResponseSchedule>();

            CreateMap<Request_UpdateSchedule, Schedule>()
            .ForMember(dest => dest.StartAt, opt => opt.Condition(src => src.StartAt.HasValue));
        }    
    }
}
