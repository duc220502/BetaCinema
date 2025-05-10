using BetaCinema.Entities;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IMovieService
    {
        Task<ResponseObject<DataResponseMovie>> AddMovie(Request_AddMovie rq);
        Task<ResponseObject<DataResponseMovie>> UpdateMovie(Request_UpdateMovie rq);

        Task<ResponseObject<DataResponseMovie>> DeleteMovie(int id);

        Task<ResponseObject<DataResponseMovie>> DetailMovie(int id);

        Task<ResponseObject<List<DataResponseMovie>>> ListMoviesOfCinemas(Pagination pagination,int cinemaId);

        Task<ResponseObject<List<DataResponseMovie>>> GetTopViewMovies(Pagination pagination);
    }
}
