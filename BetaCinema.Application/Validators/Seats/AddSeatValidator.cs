using BetaCinema.Application.DTOs.DataRequest.Seats;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Seats
{
    public class AddSeatValidator : AbstractValidator<Request_AddSeat>
    {
        private readonly ISeatRepository _seatRepository ;
        private readonly IRoomRepository _roomRepository ;
        private readonly ISeatTypeRepository _seatTypeRepository ;

        public AddSeatValidator(ISeatRepository seatRepository , IRoomRepository roomRepository ,ISeatTypeRepository seatTypeRepository) 
        {
            _seatRepository = seatRepository;
            _roomRepository = roomRepository;
            _seatTypeRepository = seatTypeRepository;

            RuleFor(x => x.Number)
            .NotNull().NotEmpty().WithMessage("Số ghế không được để trống.")
            .GreaterThanOrEqualTo(1).WithMessage("Số ghế không hợp lí.");

            RuleFor(x => x.Line)
            .IsEnumName(typeof(LineSeat), caseSensitive: false).WithMessage("Vị trí hàng không hợp lệ.");


            RuleFor(x => x.RoomId)
            .NotNull().NotEmpty().WithMessage("RoomId không được để trống.")
            .MustAsync(CheckRoomId).WithMessage("RoomId không tồn tại.");


            RuleFor(x => x.SeatTypeId)
            .NotNull().NotEmpty().WithMessage("SeatTypeId không được để trống.")
            .MustAsync(CheckSeatTypeId).WithMessage("SeatTypeId không tồn tại.");

            RuleFor(x => x)
           .MustAsync(async (seatRequest, cancellationToken) =>
           {
               
               if (!Enum.TryParse<LineSeat>(seatRequest.Line, true, out var lineAsEnum))
               {
                   return true; 
               }
               return await _seatRepository.IsSeatUniqueAsync(seatRequest.Number, lineAsEnum, seatRequest.RoomId);
           })
           .WithMessage("Vị trí ghế (hàng và số) đã tồn tại trong phòng này.")
           .When(x => !string.IsNullOrEmpty(x.Line) && x.Number > 0 && x.RoomId != Guid.Empty);


        }

        private async Task<bool> CheckRoomId(Guid id, CancellationToken cancellationToken)
           => await _roomRepository.GetRoomByIdAsync(id) != null;

        private async Task<bool> CheckSeatTypeId(int id, CancellationToken cancellationToken)
          => await _seatTypeRepository.GetSeatTypeByIdAsync(id) != null;
    }
}
