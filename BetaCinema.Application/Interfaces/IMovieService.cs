using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Movies;
using BetaCinema.Application.DTOs.DataResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IMovieService
    {
        Task<ResponseObject<DataResponseMovie>> AddMovie(Request_AddMovie rq);
        Task<ResponseObject<DataResponseMovie>> GetMovieById(Guid id);

        Task<ResponseObject<DataResponseMovie>> UpdateMovie(Guid id, Request_UpdateMovie rq);

        Task<ResponseObject<DataResponseMovie>> DeleteMovie(Guid id);
    }
}
