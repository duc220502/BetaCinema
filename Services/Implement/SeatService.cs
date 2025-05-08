using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.Services.Implement
{
    public class SeatService : BaseService, ISeatService
    {
        private readonly ResponseObject<DataResponseSeat> _responseObject;
        private readonly ResponseObject<List<DataResponseSeat>> _responseObjects;
        private readonly SeatConverter _converter;

        public SeatService()
        {
            _converter = new SeatConverter();
            _responseObject = new ResponseObject<DataResponseSeat>();
            _responseObjects = new ResponseObject<List<DataResponseSeat>>();
        }

        public async Task<ResponseObject<DataResponseSeat>> AddSeat(Request_AddSeat rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] { rq.Number.ToString(), rq.Line.ToString(), rq.SeatStatusId.ToString(), rq.SeatTypeId.ToString(), rq.RoomId.ToString() }))

                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            if(rq.Number <= 0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số ghế không hợp lệ", null);

            var isDuplicateNumber = await _context.Seats.AnyAsync(x=>x.Number == rq.Number && x.Line == rq.Line.ToString());

            if (isDuplicateNumber)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số ghế bị trùng", null);



            var checkSeatType = await _context.SeatTypes.AnyAsync(x=>x.Id == rq.SeatTypeId);

            if (!checkSeatType)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "SeatType không tồn tại", null);


            var checkSeatStatus = await _context.SeatStatuses.AnyAsync(x=>x.Id == rq.SeatStatusId);

            if(!checkSeatStatus)

               return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "SeatStatus không tồn tại", null);


            var checkRoom = await _context.Rooms.AnyAsync(x=>x.Id == rq.RoomId);

            if (!checkRoom)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Room không tồn tại", null);


            var newSeat = new Seat()
            {
                Number = rq.Number,
                Line = rq.Line.ToString(),
                IsActive = true,
                RoomId = rq.RoomId,
                SeatStatusId = rq.SeatStatusId,
                SeatTypeId = rq.SeatTypeId,

            };

            _context.Seats.Add(newSeat);
             await _context.SaveChangesAsync();


            return _responseObject.ResponseSuccess("Thêm ghế thành công", _converter.EntityToDTO(newSeat));
        }

        public async Task<ResponseObject<DataResponseSeat>> DeleteSeat(int id)
        {
            if (InputHelper.checkNull(new string[] { id.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id cần xóa", null);


            var seatCr = await _context.Seats.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);

            if (seatCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Seat không tồn tại hoặc không hoạt động", null);
            

            seatCr.IsActive = false;

            _context.Seats.Update(seatCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Xóa thành công",_converter.EntityToDTO(seatCr));
        }

        public async Task<ResponseObject<List<DataResponseSeat>>> ListSeatOfRooms(Pagination pagination,int roomId)
        {
            if (InputHelper.checkNull(new string[] { roomId.ToString() }))
                return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền roomId", null);


            var checkRoom = await _context.Rooms.AnyAsync(x=>x.Id == roomId && x.IsActive);
            if(!checkRoom)
                return  _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "Room không tồn tại hoặc không hoạt động", null);


            var seats =  _context.Seats.Where(x => x.RoomId == roomId && x.IsActive).Select(x => _converter.EntityToDTO(x));

            var result = await Result(pagination, seats).ToListAsync();

            return _responseObjects.ResponseSuccess("Lấy danh sách thành công",result);
           
        }

        public async Task<ResponseObject<DataResponseSeat>> UpdateSeat(Request_UpdateSeat rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] {rq.Id.ToString(), rq.Number.ToString(), rq.Line.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var seatCr = await _context.Seats.FirstOrDefaultAsync(x => x.Id == rq.Id && x.IsActive == true);

            if ((seatCr == null))

                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Seat không tồn tại hoặc không hoạt động", null);


            if (rq.Number<=0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số ghế không hợp lệ", null);

            var isDuplicateSeat = await _context.Seats.AnyAsync(x => x.Number == rq.Number && x.Line == rq.Line.ToString() && x.Id != rq.Id);

            if(isDuplicateSeat )
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số ghế bị trùng", null);


            seatCr.Number = rq.Number;
            seatCr.Line = rq.Line.ToString();

            _context.Seats.Update(seatCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Cập nhật thành công", _converter.EntityToDTO(seatCr));


        }


    }
}
