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
    public class CinemaService : BaseService, ICinemaService
    {
        private readonly ResponseObject<DataResponseCinema> _responseObject;
        private readonly CinemaConverter _converter;

        public CinemaService()
        {
            _responseObject = new ResponseObject<DataResponseCinema>();
            _converter = new CinemaConverter();
        }

        public async Task<ResponseObject<DataResponseCinema>> AddCinema(Request_AddCinema rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] { rq.Name, rq.Description, rq.Address }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            var checkName =await _context.Cinemas.AnyAsync(x=>x.Name == rq.Name);
            if (checkName)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên rạp bị trùng", null);



            var newCinema = new Cinema()
            {
                Name = rq.Name,
                Description = rq.Description,
                Address = rq.Address,
                Code = "C",
                IsActive = true,
            };

            
            _context.Cinemas.Add(newCinema);
            await _context.SaveChangesAsync();

            newCinema.Code = newCinema.Code + newCinema.Id;

            _context.Cinemas.Update(newCinema);
            await _context.SaveChangesAsync();


            return _responseObject.ResponseSuccess("Thêm thành công",_converter.EntityToDTO(newCinema));


        }

        public async Task<ResponseObject<DataResponseCinema>> DeleteCinema(int id)
        {
            if (InputHelper.checkNull(new string[] { id.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id cần xóa", null);

            var cinemaCr = await _context.Cinemas.FirstOrDefaultAsync(x=>x.Id == id && x.IsActive == true);
            if (cinemaCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Rạp không tồn tại hoặc không hoạt động", null);

            cinemaCr.IsActive = false;
            _context.Cinemas.Update(cinemaCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Xóa thành công",_converter.EntityToDTO(cinemaCr));

            
        }

        public async Task<ResponseObject<DataResponseCinema>> UpdateCinema(Request_UpdateCinema rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] {rq.Id.ToString(), rq.Name, rq.Description, rq.Address,rq.IsActive.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            var cinemaCr = await _context.Cinemas.FirstOrDefaultAsync(x=>x.Id == rq.Id && x.IsActive == true);
            if (cinemaCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Cinema không tồn tại hoặc không hoạt động", null);

            var checkName = await _context.Cinemas.AnyAsync(x => x.Name == rq.Name);

            if (checkName)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên đã tồn tại", null);

            cinemaCr.Name = rq.Name;
            cinemaCr.Address = rq.Address;
            cinemaCr.Description = rq.Description;
            cinemaCr.IsActive = rq.IsActive;

            _context.Cinemas.Update(cinemaCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Cập nhật thành công", _converter.EntityToDTO(cinemaCr));
        }

        
    }
}
