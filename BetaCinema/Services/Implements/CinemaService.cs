using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Services.Implements
{
    public class CinemaService : BaseService, ICinemaService
    {
        private readonly ResponseObject<DataResponseCinema> _responseObject;
        private readonly CinemaConverter _converter;

        public CinemaService()
        {
            _converter = new CinemaConverter();
            _responseObject = new ResponseObject<DataResponseCinema>();
        }

        public ResponseObject<DataResponseCinema> CreatCinema(Request_AddCinema rq)
        {
            if (InputHelper.checkNull(rq.Address, rq.Description, rq.Code, rq.NameOfCinema))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);
            var checkCode = _context.Cinemas.Any(x => x.Code.Equals(rq.Code));
            if (checkCode)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Code đã tồn tại", null);

            Cinema cinema = new Cinema()
            {
                Address = rq.Address,
                Description = rq.Description, 
                Code = rq.Code,
                NameOfCinema = rq.NameOfCinema,
                IsActive = true

            };


            _context.Cinemas.Add(cinema);
            _context.SaveChanges();

            

            if (rq.Rooms != null)
            {
                RoomService roomService = new RoomService();

                foreach (var room in rq.Rooms)
                {
                    room.CinemaId = cinema.Id;
                    roomService.CreateRoom(room);
                }
            }
            
            
            return _responseObject.ResponseSuccess("Thêm thành công", _converter.EntityToDTO(cinema));
        }

        public ResponseObject<DataResponseCinema> DeleteCinema(int id)
        {
            if (InputHelper.checkNull(id.ToString()))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id",null);

            var cinemaCr = _context.Cinemas.FirstOrDefault(x => x.Id == id);
            if (cinemaCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Id không tồn tại", null);


            cinemaCr.IsActive = false;
            _context.Cinemas.Update(cinemaCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Xóa thành công", _converter.EntityToDTO(cinemaCr));


        }

        public ResponseObject<DataResponseCinema> UpdateCinema(int id, Request_UpdateCinema rq)
        {


            if (InputHelper.checkNull(rq.Address, rq.Description, rq.Code, rq.NameOfCinema))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);

            var cinemaCr = _context.Cinemas.FirstOrDefault(x => x.Id == id);

            if (cinemaCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Cinema không tồn tại", null);

            if (!cinemaCr.IsActive)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Cinema đã không hoạt động", null);

            var checkCode = _context.Cinemas.Any(x => x.Code.Equals(rq.Code) && x.Id!=cinemaCr.Id);
            if (checkCode)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Code đã tồn tại", null);



            cinemaCr.Description = rq.Description;
            cinemaCr.Address = rq.Address;
            cinemaCr.NameOfCinema = rq.NameOfCinema;
            cinemaCr.Code = rq.Code;    

            _context.Cinemas.Update(cinemaCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Thành công", _converter.EntityToDTO(cinemaCr));
        }


    }
}
