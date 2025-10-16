using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class AdminUpdateUserValidator : AbstractValidator<Request_UpdateUserByAdmin>
    {

        public AdminUpdateUserValidator(IRoleRepository roleRepository , IUserStatusRepository userStatusRepository)
        {
                RuleFor(x => x.RoleId)
                   .Must(id => Enum.IsDefined(typeof(UserRole), id!))
                   .WithMessage("Giá trị RoleId không hợp lệ hoặc không tồn tại.")
                   .When(x => x.RoleId.HasValue);

                RuleFor(x => x.UserStatusId)
                    .Must(id => Enum.IsDefined(typeof(UserRole), id!))
                    .WithMessage("Giá trị UserStatusId không hợp lệ hoặc không tồn tại.")
                    .When(x => x.UserStatusId.HasValue);
        }
       
    }
}
