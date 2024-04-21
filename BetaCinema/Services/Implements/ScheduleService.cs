using BetaCinema.Entities;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using Org.BouncyCastle.Crypto.Digests;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Services.Implements
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

        public ResponseObject<DataResponseSchedule> CreateSchedule(Request_AddSchedule rq)
        {
            var checkRoom = _context.Rooms.Any(x=>x.Id == rq.RoomId && x.IsActive == true);
            if (!checkRoom)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Room không tồn tại", null);
            

            var checkMovie = _context.Movies.Any(x=>x.Id == rq.MovieId && x.IsActive == true);
            if (!checkMovie)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại", null);


            var overlap = _context.Schedules.Any(s => s.RoomId == rq.RoomId && s.EndAt > rq.StartAt && s.StartAt < rq.EndAt);
            if (overlap)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Bị trùng lịch chiếu", null);

            Schedule schedule = new Schedule()
            {
                Price = rq.Price,
                StartAt = rq.StartAt,
                EndAt = rq.EndAt,
                Code = rq.Code,
                Name = rq.Name,
                IsActive = true,
                MovieId = rq.MovieId,
                RoomId = rq.RoomId,
            };

            _context.Schedules.Add(schedule);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Thêm thành công",_converter.EntityToDTO(schedule));
        }

        public ResponseObject<DataResponseSchedule> DeleteSchedule(int id)
        {
            var scheduleCr = _context.Schedules.FirstOrDefault(s => s.Id == id);
            if (scheduleCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Schedule không tồn tại", null);

            scheduleCr.IsActive  = false;
            _context.Schedules.Update(scheduleCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Xóa thành công",_converter.EntityToDTO(scheduleCr));
        }

        public ResponseObject<DataResponseSchedule> UpdateSchedule(int id, Request_UpdateSchedule rq)
        {
            var scheduleCr = _context.Schedules.FirstOrDefault(s => s.Id == id && s.IsActive == true);
            if (scheduleCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Schedule không tồn tại", null);
            var checkRoom = _context.Rooms.Any(x => x.Id == rq.RoomId && x.IsActive == true);
            if (!checkRoom)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Room không tồn tại", null);


            var checkMovie = _context.Movies.Any(x => x.Id == rq.MovieId && x.IsActive == true);
            if (!checkMovie)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại", null);


            var overlap = _context.Schedules.Any(s => s.RoomId == rq.RoomId && s.EndAt > rq.StartAt && s.StartAt < rq.EndAt);
            if (overlap)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Bị trùng lịch chiếu", null);

            scheduleCr.Price = rq.Price;
            scheduleCr.StartAt = rq.StartAt;
            scheduleCr.EndAt = rq.EndAt;
            scheduleCr.Code = rq.Code;
            scheduleCr.Name = rq.Name;
            scheduleCr.MovieId = rq.MovieId;
            scheduleCr.RoomId = rq.RoomId;

            _context.Schedules.Update(scheduleCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Cập nhật thành công", null);

        }
    }
}
