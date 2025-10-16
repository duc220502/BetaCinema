using BetaCinema.Application.DTOs.DataRequest.Schedule;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Schedule
{
    public class UpdateScheduleValidator : AbstractValidator<Request_UpdateSchedule>
    {
        public UpdateScheduleValidator() 
        {
            RuleFor(x => x.StartAt)
            .NotEmpty().WithMessage("Thời gian bắt đầu không được để trống.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian bắt đầu phải lớn hơn thời gian hiện tại.")
            .When(x => x.StartAt.HasValue);
        }
    }
}
