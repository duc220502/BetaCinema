using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetaCinema.Application.UseCases
{
    public class RoomService(IRoomRepository roomRepository , IMapper mapper , IUnitOfWork unitOfWork,ICinemaRepository cinemaRepository) : IRoomService
    {
        private readonly IRoomRepository _roomRepository = roomRepository;
        private readonly ICinemaRepository _cinemaRepository = cinemaRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private string GenerateRoomCode(string cinemaCode, string roomName)
        {
            var sanitizedRoomName = roomName.Replace(" ", "").ToUpper();
            return $"{cinemaCode}-{sanitizedRoomName}";
        }
        public async Task<ResponseObject<DataResponseRoom>> AddRoom(Request_AddRoom rq)
        {
            var cinemaCr = await _cinemaRepository.GetByIdAsync(rq.CinemaId);

            var roomCode = GenerateRoomCode(cinemaCr!.Code, rq.Name);

            var newRoom = new Room()
            {
                Name = rq.Name,
                Code = roomCode,
                Capacity = rq.Capacity??0,
                RoomType = rq.Type,
                Description = rq.Description,
                IsActive = true,
                CinemaId = rq.CinemaId,

            };
            _roomRepository.Add(newRoom);


            var dto = _mapper.Map<DataResponseRoom>(newRoom);
            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseRoom>.ResponseSuccess("Thêm room thành công", dto);
        }

        public async Task<ResponseObject<DataResponseRoom>> GetRoomById(Guid id)
        {
            var result = await _roomRepository.GetByIdAsync(id) ?? throw new NotFoundException("Không tìm thấy Room");

            var dto = _mapper.Map<DataResponseRoom>(result);

            return ResponseObject<DataResponseRoom>.ResponseSuccess("Lấy thông tin room thành công ", dto);
        }

        public async Task<ResponseObject<DataResponseRoom>> UpdateRoom(Guid id, Request_UpdateRoom rq)
        {
            var roomCr = await _roomRepository.GetRoomByIdAsync(id)
             ?? throw new NotFoundException($"Không tìm thấy Room với ID: {id}");

            if (!string.IsNullOrEmpty(rq.Name) && rq.Name != roomCr.Name)
            {

                var nameExists = await _roomRepository.IsRoomNameUniqueAsync(rq.Name, id);
                if (!nameExists)
                    throw new ConflictException($"Tên Room '{rq.Name}' đã tồn tại.");
            }

            _mapper.Map(rq, roomCr);

            _roomRepository.Update(roomCr);

            var dto = _mapper.Map<DataResponseRoom>(roomCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseRoom>.ResponseSuccess("Cập nhật thông tin thành công", dto);
        }

        public async Task<ResponseObject<DataResponseRoom>> DeleteRoom(Guid id)
        {
            var roomCr = await _roomRepository.GetRoomByIdAsync(id)
             ?? throw new NotFoundException("Room không tồn tại.");

            roomCr.IsActive = false;

            _roomRepository.Update(roomCr);

            var dto = _mapper.Map<DataResponseRoom>(roomCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseRoom>.ResponseSuccess("Xóa Room thành công", dto);
        }
    }
}
