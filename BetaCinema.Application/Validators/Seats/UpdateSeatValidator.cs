using BetaCinema.Application.DTOs.DataRequest.Seats;
using BetaCinema.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Seats
{
    public class UpdateSeatValidator : AbstractValidator<Request_UpdateSeat>
    {
        public UpdateSeatValidator() 
        {
            RuleFor(x => x.Number)
            .GreaterThanOrEqualTo(1).WithMessage("Số ghế không hợp lí.")
            .When(x => x.Number.HasValue);

            RuleFor(x => x.Line)
            .IsEnumName(typeof(LineSeat), caseSensitive: false).WithMessage("Vị trí hàng không hợp lệ.")
            .When(x=>!string.IsNullOrEmpty(x.Line));

        }
    }
}
