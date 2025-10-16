using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface ICinemaService
    {
        Task<ResponseObject<DataResponseCinema>> AddCinema(Request_AddCinema rq);
        Task<ResponseObject<DataResponseCinema>> GetCinemaById(Guid id);
        Task<ResponseObject<IEnumerable<DataResponseCinema>>> GetCinemas(Pagination pagination);

        Task<ResponseObject<DataResponseCinema>> UpdateCinema(Guid id , Request_UpdateCinema rq);
        Task<ResponseObject<DataResponseCinema>> DeleteCinema(Guid id);

    }
}
