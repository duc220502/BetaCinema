using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Rooms
{
    public class AddRoomValidator : AbstractValidator<Request_AddRoom>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ICinemaRepository _cinemaRepository;

        public AddRoomValidator(IRoomRepository roomRepository , ICinemaRepository cinemaRepository)
        {
            _roomRepository = roomRepository;
            _cinemaRepository = cinemaRepository;

            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.Capacity)
               .GreaterThanOrEqualTo(0).WithMessage("Sức chưa không hợp lí.")
               .When(x => x.Capacity.HasValue);

            RuleFor(x => x.CinemaId)
           .NotEqual(Guid.Empty).WithMessage("CinemaId không được để trống.")
           .MustAsync(CheckCinema)
           .WithMessage("CinemaId không tồn tại.");

            RuleFor(x => x.Type)
              .IsInEnum().WithMessage("Giá trị của RoomType không hợp lệ.");

            RuleFor(x => x.Name)
               .NotEmpty().NotNull().WithMessage("Tên Room không được để trống")
                .MustAsync(async (roomRequest, roomName, cancellationToken) =>
                {
                    return await _roomRepository.IsRoomNameUniqueAsync(roomName, roomRequest.CinemaId);
                })
                 .WithMessage("Tên phòng trong rạp chiếu phim này đã được sử dụng.");

        }

        private async Task<bool> CheckCinema(Guid id, CancellationToken cancellationToken)
           => await _cinemaRepository.GetCinemaByIdAsync(id) != null;
    }
}
