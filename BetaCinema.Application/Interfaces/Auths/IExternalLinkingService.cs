using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Auth.Requests;
using BetaCinema.Application.DTOs.DataResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Auths
{
    public interface IExternalLinkingService
    {
        Task<ResponseObject<DataResponseToken>> ConfirmLinkAsync(ConfirmExternalLinkRequest req, CancellationToken ct);
    }
}
