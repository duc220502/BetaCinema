using BetaCinema.Entities;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IMovieService
    {

        ResponseObject<IEnumerable<DataResponseMoviesQuanTiTy>> get_MovieOutStanding();
        ResponseObject<DataResponseMovieDetail> get_MovieDetail(int id);
        ResponseObject<DataResponseMovie> CreateMovie(Request_AddMovie rq);
        ResponseObject<DataResponseMovie> UpdateMovie(int id,Request_UpdateMovie rq);

        ResponseObject<DataResponseMovie> DeleteMovie(int id);
    }
}
