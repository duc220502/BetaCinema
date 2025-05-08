using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Xml.Linq;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.Services.Implement
{
    
    public class RoomService :BaseService, IRoomService
    {
        private readonly ResponseObject<DataResponseRoom> _responseObject;
        private readonly RoomConverter _converter;

        public RoomService()
        {
            _converter = new RoomConverter();
            _responseObject = new ResponseObject<DataResponseRoom>();
        }

        public async Task<ResponseObject<DataResponseRoom>> AddRoom(Request_AddRoom rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] { rq.Name, rq.Capacity.ToString(), rq.Type.ToString(), rq.CinemaId.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var checkName = await _context.Rooms.AnyAsync(x=>x.Name == rq.Name);

            if (checkName)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên phòng đã tồn tại", null);

            if (rq.Capacity <= 0) 
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Sức chứa không phù hợp", null);


            var checkCinema = await _context.Cinemas.AnyAsync(x=>x.Id == rq.CinemaId && x.IsActive == true);

            if(!checkCinema)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Cinema không tồn tại hoặc rạp không hoạt động", null);

            var newRoom = new Room()
            {
                Name = rq.Name,
                Code = "R",
                Capacity = rq.Capacity,
                RoomType = rq.Type.ToString(),
                Description = rq.Description,
                IsActive = true,
                CinemaId = rq.CinemaId,

            };

            _context.Rooms.Add(newRoom);
            await _context.SaveChangesAsync();

            newRoom.Code = newRoom.Code + newRoom.Id;
            _context.Rooms.Update(newRoom);
            await _context.SaveChangesAsync();


            return _responseObject.ResponseSuccess("Thêm room thành công",_converter.EntityToDTO(newRoom));
        }

        public async Task<ResponseObject<DataResponseRoom>> DeleteRoom(int id)
        {
            if (InputHelper.checkNull(new string[] { id.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id cần xóa", null);

            var roomCr = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (roomCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Phòng không tồn tại hoặc không hoạt động", null);

            roomCr.IsActive = false;
            _context.Rooms.Update(roomCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Xóa thành công", _converter.EntityToDTO(roomCr));
        }

        public async Task<ResponseObject<DataResponseRoom>> UpdateRoom(Request_UpdateRoom rq)
        {
            if(rq == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var roomCr = await _context.Rooms.FirstOrDefaultAsync(x=>x.Id == rq.Id && x.IsActive == true);

            if(roomCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Phòng không tồn tại hoặc đã ngưng hoạt động", null);

            if(rq.Name != roomCr.Name)
            {
                var isDuplicateName = await _context.Rooms.AnyAsync(x=>x.Id != rq.Id && x.Name == rq.Name && x.IsActive == true);

                if(isDuplicateName)
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên phòng bị trùng với tên phòng khác", null);
            }

            if(rq.Capacity<=0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số lượng ghế trong phòng không phù hợp", null);

            roomCr.Name = rq.Name??roomCr.Name;
            roomCr.Capacity = rq.Capacity??roomCr.Capacity;
            roomCr.RoomType = rq.RoomType.ToString()??roomCr.RoomType;
            roomCr.Description = rq.Description??roomCr.Description;

            _context.Rooms.Update(roomCr);
            await _context.SaveChangesAsync();


            return _responseObject.ResponseSuccess("Cập nhật thông tin thành công",_converter.EntityToDTO(roomCr));

        }
    }


}
