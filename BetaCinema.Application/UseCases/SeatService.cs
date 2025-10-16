using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Seats;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BetaCinema.Application.UseCases
{
    public class SeatService(ISeatStatusRepository seatStatusRepository , ISeatRepository seatRepository , IMapper mapper , IUnitOfWork unitOfWork ) : ISeatService
    {
        private readonly ISeatStatusRepository _seatStatusRepository = seatStatusRepository;
        private readonly ISeatRepository _seatRepository = seatRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseObject<DataResponseSeat>> AddSeatAsync(Request_AddSeat rq)
        {
            var seatStatusDefault = await _seatStatusRepository.GetDefaultSeatStatusAsync() ?? throw new NotFoundException("SeatStatus không tìm thấy");

            var lineAsEnum = Enum.Parse<LineSeat>(rq.Line, true);

            var newSeat = new Seat()
            {
                Number = rq.Number,
                Line = lineAsEnum,
                IsActive = true,
                RoomId = rq.RoomId,
                SeatStatusId = seatStatusDefault.Id,
                SeatTypeId = rq.SeatTypeId,

            };

            _seatRepository.Add(newSeat);

            var dto = _mapper.Map<DataResponseSeat>(newSeat);
            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseSeat>.ResponseSuccess("Thêm room thành công", dto);
        }

        public async Task<ResponseObject<DataResponseSeat>> DeleteSeat(Guid id)
        {
            var seatCr = await _seatRepository.GetByIdAsync(id)
             ?? throw new NotFoundException("Seat không tồn tại.");

            seatCr.IsActive = false;

            _seatRepository.Update(seatCr);

            var dto = _mapper.Map<DataResponseSeat>(seatCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseSeat>.ResponseSuccess("Xóa Seat thành công", dto);
        }

        public async Task<ResponseObject<DataResponseSeat>> GetSeatByIdAsync(Guid id)
        {
            var result = await _seatRepository.GetByIdAsync(id) ?? throw new NotFoundException("Không tìm thấy Seat");

            var dto = _mapper.Map<DataResponseSeat>(result);

            return ResponseObject<DataResponseSeat>.ResponseSuccess("Lấy thông tin seat thành công ", dto);
        }

       

        public async Task<ResponseObject<DataResponseSeat>> UpdateSeat(Guid id, Request_UpdateSeat rq)
        {
            var seatCr = await _seatRepository.GetByIdAsync(id)
             ?? throw new NotFoundException($"Không tìm thấy Seat với ID: {id}");

            var newNumber = rq.Number ?? seatCr.Number;
            var newLine = !string.IsNullOrEmpty(rq.Line)
                        ? Enum.Parse<LineSeat>(rq.Line, true)
                        : seatCr.Line;

            if (newNumber != seatCr.Number || newLine != seatCr.Line)
            {
                var isDuplicate = !await _seatRepository.IsSeatUniqueAsync(newNumber, newLine, seatCr.RoomId);
                if (isDuplicate)
                {
                    throw new ConflictException($"Vị trí ghế {newLine}{newNumber} đã tồn tại trong phòng.");
                }
            }

            _mapper.Map(rq, seatCr);


            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<DataResponseSeat>(seatCr);

            return ResponseObject<DataResponseSeat>.ResponseSuccess("Cập nhật ghế thành công", dto);
        }

        public void UpdateSeatsAfterPurchase(List<Seat> seats)
        {
            if (seats.Any())
            {
                seats.ForEach(seat => {
                    seat.SeatStatusId = (int)Domain.Enums.SeatStatus.Booked;
                });
                
            }
        }
    }
}
