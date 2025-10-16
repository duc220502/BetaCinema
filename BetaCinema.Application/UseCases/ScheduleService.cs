using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Movies;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.Schedule;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces.Repositorys;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetaCinema.Application.UseCases
{
    public class ScheduleService(IScheduleRepository scheduleRepository, 
    IUnitOfWork unitOfWork , IMapper mapper , IMovieRepository movieRepository , 
    IRoomRepository roomRepository , IOptions<ScheduleSettings> options , 
    ISeatRepository seatRepository , IBillCleanUpService billCleanUpService) : IScheduleService
    {

        private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
        private readonly IMovieRepository _movieRepository = movieRepository;
        private readonly IRoomRepository _roomRepository = roomRepository;
        private readonly ISeatRepository _seatRepository = seatRepository;
        private readonly IBillCleanUpService _billCleanUpService = billCleanUpService;
        private readonly ScheduleSettings _scheduleSettings = options.Value;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        private string GenerateScheduleName(Movie movie, Room room, DateTime startAt)
        {
            return $"{movie.Name} - {room.Name} - {startAt:HH:mm dd/MM/yyyy}";
        }

        private string GenerateScheduleCode(string roomCode, DateTime startAt)
        {
            var datePart = startAt.ToString("yyyyMMdd");
            var timePart = startAt.ToString("HHmm");
            return $"{roomCode}-{datePart}-{timePart}";
        }
        public async Task<ResponseObject<DataResponseSchedule>> AddSchedule(Request_AddSchedule rq)
        {
            var movieCr = await _movieRepository.GetByIdAsync(rq.MovieId)
            ?? throw new NotFoundException($"Không tìm thấy phim với ID: {rq.MovieId}");

            var roomCr = await _roomRepository.GetByIdAsync(rq.RoomId)
                ?? throw new NotFoundException($"Không tìm thấy phòng chiếu với ID: {rq.RoomId}");

            var bufferTime = _scheduleSettings.CleaningBufferMinutes;

            
            var endAt = rq.StartAt.AddMinutes(movieCr.MovieDuration);
            var isOverlapping = await _scheduleRepository.IsTimeSlotOverlappingAsync(rq.RoomId, rq.StartAt, endAt, bufferTime);
            if (isOverlapping)
            {
                throw new ConflictException("Lịch chiếu bị trùng (đã tính cả thời gian chờ).");
            }

            var nameSchedule = GenerateScheduleName(movieCr, roomCr, rq.StartAt);
            var codeSchedule = GenerateScheduleCode(roomCr.Code, rq.StartAt);
            var newSchedule = new Schedule
            {
                Name = nameSchedule,
                StartAt = rq.StartAt,
                EndAt = endAt , 
                Code = codeSchedule,
                IsActive = true,
                MovieId = rq.MovieId,
                RoomId = rq.RoomId
            };

            _scheduleRepository.Add(newSchedule);


            var dto = _mapper.Map<DataResponseSchedule>(newSchedule);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseSchedule>.ResponseSuccess("Thêm schedule thành công", dto);
        }

        public async Task<ResponseObject<DataResponseSchedule>> GetScheduleById(Guid id)
        {
            var result = await _scheduleRepository.GetByIdAsync(id) ?? throw new NotFoundException("Không tìm thấy Schedule");

            var dto = _mapper.Map<DataResponseSchedule>(result);

            return ResponseObject<DataResponseSchedule>.ResponseSuccess("Lấy thông tin schedule thành công ", dto);
        }

        public async Task<ResponseObject<List<DataResponseAvailableSlotDto>>> GetAvailableSlotsAsync(Guid roomId, Guid movieId, DateTime date)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId)
                        ?? throw new NotFoundException("Không tìm thấy phim.");

            var room = await _roomRepository.GetByIdAsync(roomId)
                        ?? throw new NotFoundException("Không tìm thấy room.");

            var bufferMinutes = _scheduleSettings.CleaningBufferMinutes;

            var startOfDay = date.Date.AddHours(8);
            var endOfDay = date.Date.AddDays(1).AddHours(2);

            var existingSchedules = (await _scheduleRepository.GetSchedulesByRoomAndDateAsync(roomId, startOfDay, endOfDay))
                                        .OrderBy(s => s.StartAt)
                                        .ToList();

            var availableSlots = new List<DataResponseAvailableSlotDto>();
            var lastAvailableTime = startOfDay;

            foreach (var schedule in existingSchedules)
            {

                var safeGapEnd = schedule.StartAt.AddMinutes(-bufferMinutes);

                var freeTimeSpan = safeGapEnd - lastAvailableTime;

                if (freeTimeSpan.TotalMinutes >= movie.MovieDuration)
                {
                    availableSlots.Add(new DataResponseAvailableSlotDto
                    {
                        StartAt = lastAvailableTime,
                        EndAt = safeGapEnd 
                    });
                }

                lastAvailableTime = schedule.EndAt.AddMinutes(bufferMinutes);
            }

            if ((endOfDay - lastAvailableTime).TotalMinutes >= movie.MovieDuration)
            {
                availableSlots.Add(new DataResponseAvailableSlotDto
                {
                    StartAt = lastAvailableTime,
                    EndAt = endOfDay
                });
            }

            return ResponseObject<List<DataResponseAvailableSlotDto>>.ResponseSuccess("Lấy danh sách thời gian trống thành công", availableSlots);
        }

        public async Task<ResponseObject<DataResponseSchedule>> UpdateSchedule(Guid id, Request_UpdateSchedule rq)
        {
            var scheduleCr = await _scheduleRepository.GetByIdAsync(id)
                       ?? throw new NotFoundException("Không tìm thấy lịch chiếu.");

            var movieCr = await _movieRepository.GetByIdAsync(scheduleCr.MovieId)
                       ?? throw new NotFoundException("Không tìm thấy phim.");

            var bufferTime = _scheduleSettings.CleaningBufferMinutes;

            if(rq.StartAt.HasValue)
            {
                var endAt = rq.StartAt.Value.AddMinutes(movieCr.MovieDuration);
                var isOverlapping = await _scheduleRepository.IsTimeSlotOverlappingAsync(scheduleCr.RoomId, rq.StartAt.Value, endAt, bufferTime);
                if (isOverlapping)
                {
                    throw new ConflictException("Lịch chiếu bị trùng (đã tính cả thời gian chờ).");
                }
            }

            _mapper.Map(rq, scheduleCr);

            _scheduleRepository.Update(scheduleCr);

            var dto = _mapper.Map<DataResponseSchedule>(scheduleCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseSchedule>.ResponseSuccess("Cập nhật thông tin thành công", dto);


        }

        public async Task<ResponseObject<DataResponseSchedule>> DeleteSchedule(Guid id)
        {
            var scheduleCr = await _scheduleRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Schedule không tồn tại.");

            scheduleCr.IsActive = false;

            _scheduleRepository.Update(scheduleCr);

            var dto = _mapper.Map<DataResponseSchedule>(scheduleCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseSchedule>.ResponseSuccess("Xóa Schedule thành công", dto);
        }

        public async Task<ResponseObject<List<DataResponseSeat>>> GetSeatsForScheduleAsync(Guid scheduleId)
        {
            await _billCleanUpService.ReleaseExpiredSeatsForScheduleAsync(scheduleId);

            var seats = await _seatRepository.GetSeatsByScheduleAsync(scheduleId);
            var dtos =  _mapper.Map<List<DataResponseSeat>>(seats);

            return ResponseObject< List < DataResponseSeat >>.ResponseSuccess("Lấy dữ liệu thành công" , dtos);
        }
    }
}
