using BetaCinema.Application.DTOs.DataRequest.Rooms;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Rooms
{
    public class UpdateRoomValidator : AbstractValidator<Request_UpdateRoom>
    {
        public UpdateRoomValidator() 
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Tên không được để trống.")
           .When(x => x.Name != null);

            RuleFor(x => x.RoomType)
            .IsInEnum().WithMessage("Giá trị RoomType không hợp lệ.")
            .When(x => x.RoomType.HasValue);

            RuleFor(x => x.Capacity)
              .GreaterThanOrEqualTo(0).WithMessage("Sức chưa không hợp lí.")
              .When(x => x.Capacity.HasValue);


        }
    }
}
