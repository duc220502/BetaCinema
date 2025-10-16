using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Auths
{
    public interface ITokenService
    {
        Task<DataResponseToken> GenerateTokenAsync(User user);
        Task<ResponseObject<DataResponseToken>> RenewToken(Request_RenewToken rq);
    }
}
