using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IUserRegistrationService
    {
        Task<ResponseObject<DataResponseUser>> Register(Request_Register rq, ConfirmationMethod method);
        Task<ResponseObject<DataResponseUser>> ConfirmEmailRegister(Request_ConfirmEmailRegister rq);
    }
}
