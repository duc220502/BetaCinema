using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Schedule;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Schedules
{
    public class AddScheduleValidator : AbstractValidator<Request_AddSchedule>
    {
        public AddScheduleValidator() 
        {
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.StartAt)
            .NotEmpty().WithMessage("Ngày phát hành không được để trống.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian bắt đầu phải lớn hơn thời gian hiện tại.");

            RuleFor(x => x.RoomId)
            .NotEqual(Guid.Empty).WithMessage("ID là bắt buộc và không được để trống.");


            RuleFor(x => x.MovieId)
            .NotEqual(Guid.Empty).WithMessage("ID là bắt buộc và không được để trống.");
            

        }
       


    }
}
