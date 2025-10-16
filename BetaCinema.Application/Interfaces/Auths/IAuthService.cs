using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Auths
{
    public interface IAuthService
    {
        Task<ResponseObject<DataResponseToken>> Login(Request_Login rq);
        Task<ResponseObject<object>> SendMailResetPasswordAsync(string account, ConfirmationMethod method);
        Task<ResponseObject<string>> VerifyResetCodeAsync(Request_VerifyCode rq);
        Task<ResponseObject<DataResponseUser>> ResetPasswordAsync(Guid userId, Request_ResetPassword rq);

    }
}
