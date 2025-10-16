using AutoMapper;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Role, DataResponseRole>();

            CreateMap<UserStatus, DataResponseUserStatus>();

            CreateMap<User, DataResponseUser>();
               


            CreateMap<Request_UpdateMyProfile, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
            {
                if (srcMember is string str)
                {
                    return !string.IsNullOrEmpty(str);
                }
                return srcMember != null;
            }));


            CreateMap<Request_UpdateUserByAdmin, User>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom((src, dest) => src.RoleId ?? dest.RoleId))
            .ForMember(dest => dest.UserStatusId, opt => opt.MapFrom((src, dest) => src.UserStatusId ?? dest.UserStatusId))
            .ForMember(dest => dest.FullName, opt => opt.Condition(src => !string.IsNullOrEmpty(src.FullName)));
        }
    }
}
