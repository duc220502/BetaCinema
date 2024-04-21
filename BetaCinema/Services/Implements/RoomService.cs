using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using System;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Services.Implements
{
    public class RoomService : BaseService, IRoomService
    {
        private readonly ResponseObject<DataResponseRoom> _responseObject;
        private readonly RoomConverter _converter;

        public RoomService()
        {
            _converter = new RoomConverter();
            _responseObject = new ResponseObject<DataResponseRoom>();
        }
        public ResponseObject<DataResponseRoom> CreateRoom(Request_AddRoom rq)
        {
            if (InputHelper.checkNull(rq.Capacity.ToString(), rq.Description, rq.Code, rq.Type.ToString(),rq.Name,rq.CinemaId.ToString()) || rq.CinemaId == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);

            var checkCinema = _context.Cinemas.Any(x=>x.Id == rq.CinemaId && x.IsActive == true);
            if (!checkCinema)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Cinema không tồn tại", null);

            var checkCode = _context.Rooms.Any(x => x.Code.Equals(rq.Code) && x.CinemaId==rq.CinemaId);
            if (checkCode)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Code đã tồn tại", null);


            Room room = new Room()
            {
                Capacity = rq.Capacity,
                Type = rq.Type,
                Description = rq.Description,
                Code = rq.Code,
                Name = rq.Name,
                IsActive = true,
                CinemaId = rq.CinemaId.Value,

            };


            _context.Rooms.Add(room);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Thêm thành công", _converter.EntityToDTO(room));
        }

        public ResponseObject<DataResponseRoom> DeleteRoom(int id)
        {
            if (InputHelper.checkNull(id.ToString()))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id", null);

            var roomCr = _context.Rooms.FirstOrDefault(x => x.Id == id);
            if (roomCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Id không tồn tại", null);

            roomCr.IsActive = false;
            _context.Rooms.Update(roomCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Xóa thành công", _converter.EntityToDTO(roomCr));

        }

        public ResponseObject<DataResponseRoom> UpdateRoom(int id, Request_UpdateRoom rq)
        {
            if (InputHelper.checkNull(rq.Capacity.ToString(), rq.Description, rq.Code, rq.Type.ToString(), rq.Name))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);

            var roomCr = _context.Rooms.FirstOrDefault(x => x.Id == id);
            if (roomCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Room không tồn tại", null);

            if (!roomCr.IsActive)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Room đã không hoạt động", null);
            var checkCode = _context.Rooms.Any(x => x.Code.Equals(rq.Code) && x.Id != roomCr.Id);
            if (checkCode)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Code đã tồn tại", null);

            roomCr.Capacity =  rq.Capacity;
            roomCr.Description = rq.Description;
            roomCr.Code = rq.Code;
            roomCr.Name = rq.Name;
            roomCr.Type = rq.Type;

            _context.Rooms.Update(roomCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Thành công", _converter.EntityToDTO(roomCr));
        }
    }
}
