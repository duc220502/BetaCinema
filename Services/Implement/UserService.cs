using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace BetaCinema.Services.Implement
{
    public class UserService : BaseService, IUserSevice
    {
        private readonly ResponseObject<DataResponseUser> _response;
        private readonly UserConverter _converter;

        public UserService()
        {
            _converter = new UserConverter();
            _response = new ResponseObject<DataResponseUser>();
        }

        public async Task<ResponseObject<DataResponseUser>> ChangPassword(int id, Request_ChangPassword rq)
        {
            if (InputHelper.checkNull(new string[] { id.ToString(), rq.NewPass, rq.OldPass }))
                return  _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var userCr = await _context.Users.FirstOrDefaultAsync(x=> x.Id == id);

            if (userCr == null)
                return _response.ResponseError(StatusCodes.Status400BadRequest, "User không tồn tại", null);

            if (!userCr.IsActive)
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản chưa được kích hoạt", null);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(rq.OldPass, userCr.Password);

            if (!isPasswordValid)

                return _response.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);

            if (rq.OldPass == rq.NewPass)

                return _response.ResponseError(StatusCodes.Status400BadRequest, "Trùng mật khẩu cũ", null);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(rq.NewPass);

            userCr.Password = rq.NewPass;
            _context.Users.Update(userCr);
            await _context.SaveChangesAsync();

            return _response.ResponseSuccess("Thay đổi mật khẩu thành công", _converter.EntityToDTO(userCr));
        }

        public async Task<ResponseObject<DataResponseUser>> NewPassword(int id, string newPassword)
        {
            if (InputHelper.checkNull(new string[] { id.ToString(),newPassword }))
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var userCr = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (!userCr.IsActive)
                return _response.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản chưa được kích hoạt", null);

            if (userCr == null)
                return _response.ResponseError(StatusCodes.Status400BadRequest, "User không tồn tại", null);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(newPassword, userCr.Password);

            if (isPasswordValid)

                return _response.ResponseError(StatusCodes.Status400BadRequest, "Trùng mật khẩu cũ", null);

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            userCr.Password = hashedPassword;
            _context.Users.Update(userCr);

            await _context.SaveChangesAsync();

            return _response.ResponseSuccess("Tạo mới mật khẩu thành công", _converter.EntityToDTO(userCr));
        }
    }
}
