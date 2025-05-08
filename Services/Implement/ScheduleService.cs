using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.Services.Implement
{
    public class ScheduleService : BaseService, IScheduleService
    {
        private readonly ResponseObject<DataResponseSchedule> _responseObject;
        private readonly ScheduleConverter _converter;

        public ScheduleService()
        {
            _converter = new ScheduleConverter();
            _responseObject = new ResponseObject<DataResponseSchedule>();
        }

        public async Task<ResponseObject<DataResponseSchedule>> AddSchedule(Request_AddSchedule rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] { rq.Name, rq.StartAt.ToString(), rq.RoomId.ToString(), rq.MovieId.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            if(rq.StartAt<DateTime.Now)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Thời gian không hợp lệ", null);

            var isDuplicateName = await _context.Schedules.AnyAsync(x=>x.Name == rq.Name && x.IsActive);
            if(isDuplicateName)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên lịch chiếu bị trùng", null);

            var checkRoom = await _context.Rooms.AnyAsync(x=>x.Id == rq.RoomId && x.IsActive);
            if(!checkRoom)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Room không tồn tại", null);

            var movieCr = await _context.Movies.FirstOrDefaultAsync(x=>x.Id==rq.MovieId && x.IsActive);
            if(movieCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại", null);

            var endAt = rq.StartAt.AddHours(movieCr.MovieDuration);

            var isDuplicateTime = await _context.Schedules.AnyAsync(x => x.IsActive && x.RoomId == rq.RoomId && rq.StartAt < x.EndAt && endAt > x.StartAt);

            if(isDuplicateTime)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Lịch chiếu bị trùng", null);



            var newSchedule = new Schedule()
            {
                Name = rq.Name,
                StartAt = rq.StartAt,
                EndAt = endAt,
                Code = "S",
                IsActive = true,
                MovieId = rq.MovieId,
                RoomId = rq.RoomId,

            };

            _context.Schedules.Add(newSchedule);
            await _context.SaveChangesAsync();


            newSchedule.Code = newSchedule.Code + newSchedule.Id;

            _context.Schedules.Update(newSchedule);
            await _context.SaveChangesAsync();


            return _responseObject.ResponseSuccess("Thêm thành công schedule",_converter.EntityToDTO(newSchedule));

        }

        public async Task<ResponseObject<DataResponseSchedule>> DeleteSchedule(int scheduleId)
        {
            if (InputHelper.checkNull(new string[] {scheduleId.ToString()}))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var scheduleCr = await _context.Schedules.FirstOrDefaultAsync(x=>x.Id == scheduleId && x.IsActive);

            if(scheduleCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Schedule không tồn tại", null);

            scheduleCr.IsActive = false;

            _context.Schedules.Update(scheduleCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Xóa schedule thành công",_converter.EntityToDTO(scheduleCr));

        }

        public async Task<ResponseObject<DataResponseSchedule>> UpdateSchedule(Request_UpdateSchedule rq)
        {
            if(rq == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);


            var scheduleCr = await _context.Schedules.FirstOrDefaultAsync(x=>x.Id == rq.Id && x.IsActive);

            if(scheduleCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Schedule không tồn tại", null);

            var movieCr = await _context.Movies.FirstOrDefaultAsync(x=>x.Id == scheduleCr.MovieId && x.IsActive);

            var endAt = scheduleCr.EndAt;
            if (rq.StartAt != null)
                endAt = rq.StartAt?.AddHours(movieCr.MovieDuration);

            var checkTime = await _context.Schedules.AnyAsync(x=>x.Id != rq.Id && rq.StartAt < x.EndAt && endAt > x.StartAt && x.IsActive && x.RoomId == scheduleCr.RoomId);

            if(checkTime)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Lịch chiếu bị trùng", null);

            scheduleCr.Name = rq.Name??scheduleCr.Name;
            scheduleCr.StartAt = rq.StartAt??scheduleCr.StartAt;
            scheduleCr.EndAt = endAt;

            _context.Schedules.Update(scheduleCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Cập nhật lịch chiếu thành công",_converter.EntityToDTO(scheduleCr));
        }
    }
}
