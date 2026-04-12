using AutoMapper;
using BetaCinema.Application.DTOs.Notification;
using BetaCinema.Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile() 
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.DispatchStatus, opt => opt.MapFrom(src => src.DispatchStatus.ToString()))
                .ForMember(dest => dest.NotificationType, opt => opt.MapFrom(src => src.NotificationType.ToString()));
        }
    }
}
