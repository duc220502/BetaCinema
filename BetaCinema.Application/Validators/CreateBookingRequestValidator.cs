using BetaCinema.Application.DTOs.DataRequest;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators
{
    public class CreateBookingRequestValidator : AbstractValidator<Request_CreateBooking>
    {
        public CreateBookingRequestValidator() 
        {
            RuleFor(x => x.ScheduleId)
           .NotEmpty().WithMessage("Vui lòng chọn suất chiếu.");

            RuleFor(x => x.SeatIds)
           .Must(ids =>  ids?.Count <= 10) 
           .WithMessage("Bạn chỉ có thể đặt tối đa 10 ghế trong một lần.");


            When(request => request.FoodItems == null || !request.FoodItems.Any(), () => {
                RuleFor(request => request.SeatIds)
                    .NotEmpty() 
                    .WithMessage("Vui lòng chọn ít nhất một ghế hoặc một món ăn.");
            });
        }
    }
}
