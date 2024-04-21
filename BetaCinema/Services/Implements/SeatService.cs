using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BetaCinema.Services.Implements
{
    public class SeatService : BaseService, ISeatService
    {
        private readonly ResponseObject<DataResponseSeat> _responseObject;
        private readonly ResponseObject<IEnumerable<DataResponseSeat>> _responseObjects;
        private readonly SeatConverter _converter;

        public SeatService()
        {
            _converter = new SeatConverter();
            _responseObject = new ResponseObject<DataResponseSeat>();
            _responseObjects = new ResponseObject<IEnumerable<DataResponseSeat>>();
        }
        public ResponseObject<IEnumerable<DataResponseSeat>> CreateSeat(int roomId,IEnumerable<Request_AddSeat> rqs )
        {
            var checkRoom = _context.Rooms.Any(x => x.Id == roomId && x.IsActive==true);
            if (!checkRoom)
                return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "Room không tồn tại", null);
            
            List<Seat> list = new List<Seat>();
            HashSet<int> addedSeatNumbers = new HashSet<int>();
            foreach ( var rq in rqs )
            {
                if (InputHelper.checkNull(rq.Number.ToString(), rq.Line, rq.SeatStatusId.ToString(), rq.SeatTypeId.ToString()))
                    return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);

                if (addedSeatNumbers.Contains(rq.Number))
                {
                    return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, $"Số ghế {rq.Number} đã được thêm trước đó", null);
                }

                var checkSeatNumber = _context.Seats.Any(x=>x.Number == rq.Number);
                if (checkSeatNumber)
                    return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "SeatNumber đã tồn tại", null);

                var checkSeatStatus = _context.SeatStatuses.Any(x => x.Id == rq.SeatStatusId);
                var checkSeatType = _context.SeatTypes.Any(x => x.Id == rq.SeatTypeId);
                
                

                if (!checkSeatStatus)
                    return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "SeatStatus  không tồn tại", null);

                if (!checkSeatType)
                    return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "SeatType không tồn tại", null);


                if (rq.Number <= 0)
                    return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, "Number không được nhỏ hơn =  0", null);



                Seat seat = new Seat()
                {
                    Number = rq.Number,
                    Line = rq.Line,
                    IsActive = true,
                    SeatStatusId = rq.SeatStatusId,
                    RoomId = roomId,
                    SeatTypeId = rq.SeatTypeId


                };
               
                list.Add(seat);
                addedSeatNumbers.Add(rq.Number);


            }

            _context.Seats.AddRange(list);
            _context.SaveChanges();


            var listDto = list.Select(x=> _converter.EntityToDTO(x));
            return _responseObjects.ResponseSuccess("Thêm thành công", listDto);
        }

        public ResponseObject<DataResponseSeat> DeleteSeat(int id)
        {
            if (InputHelper.checkNull(id.ToString()))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id", null);

            var seatCr = _context.Seats.FirstOrDefault(x => x.Id == id);
            if (seatCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Id không tồn tại", null);

            seatCr.IsActive = false;
            _context.Seats.Update(seatCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Xóa thành công", _converter.EntityToDTO(seatCr));
        }

        public ResponseObject<DataResponseSeat> UpdateSeat(int id, Request_UpdateSeat rq)
        {
            if (InputHelper.checkNull(rq.Number.ToString(), rq.Line, rq.SeatStatusId.ToString(), rq.SeatTypeId.ToString()))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);

            var seatCr = _context.Seats.FirstOrDefault(x => x.Id == id&&x.IsActive == true);

            if (seatCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Seat không tồn tại", null);

            var checkSeatStatus = _context.SeatStatuses.Any(x => x.Id == rq.SeatStatusId);
            var checkSeatType = _context.SeatTypes.Any(x => x.Id == rq.SeatTypeId);
            if (!checkSeatStatus)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "SeatStatus  không tồn tại", null);

            if (!checkSeatType)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "SeatType không tồn tại", null);


            if (rq.Number <= 0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Number không được nhỏ hơn =  0", null);


            seatCr.Number = rq.Number;
            seatCr.Line = rq.Line;
            seatCr.IsActive = rq.IsActive;
            seatCr.SeatStatusId = rq.SeatStatusId;
            seatCr.SeatTypeId = rq.SeatTypeId;

            _context.Seats.Update(seatCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Sửa thành công", _converter.EntityToDTO(seatCr));
        }
    }
}
