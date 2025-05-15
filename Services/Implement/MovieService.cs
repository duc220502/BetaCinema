using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BetaCinema.Services.Implement
{
    public class MovieService : BaseService, IMovieService
    {

        private readonly ResponseObject<DataResponseMovie> _responseObject;
        private ResponseObject<List<DataResponseMovie>> _reponseObjectListMovie;
        private readonly MovieConverter _converter;

        public MovieService()
        {
            _responseObject = new ResponseObject<DataResponseMovie>();
            _reponseObjectListMovie = new ResponseObject<List<DataResponseMovie>>();
            _converter = new MovieConverter();
        }

        public async Task<ResponseObject<DataResponseMovie>> AddMovie(Request_AddMovie rq)
        {
            if(rq == null || InputHelper.checkNull(rq.Name,rq.Description,rq.Trailer,rq.MovieDuration.ToString(),rq.PremiereDate.ToString(),rq.Director,rq.HeroImage,rq.Language))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            var isDuplicateName = await _context.Movies.AnyAsync(x=>x.Name==rq.Name && x.IsActive == true  );

            if(isDuplicateName )
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên phim bị trùng vui lòng nhập lại", null);

            if(rq.MovieDuration<=0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Thời lượng phim không hợp lệ", null);

            if(rq.PremiereDate<DateTime.UtcNow)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Thời gian ra mắt phim không hợp lệ", null);

            var checkMovieType = await _context.MovieTypes.AnyAsync(x => x.Id == rq.MovieTypeId);

            if(!checkMovieType)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "MovieType không tồn tại", null);

            var checkRate = await _context.Rates.AnyAsync(x=>x.Id == rq.RateId);
            
            if(!checkRate )

                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Rate không tồn tại", null);

            var newMovie = new Movie()
            {
                Name = rq.Name,
                Trailer = rq.Trailer,
                MovieDuration = rq.MovieDuration,
                PremiereDate = rq.PremiereDate,
                Description = rq.Description,
                Director = rq.Director,
                HeroImage = rq.HeroImage,
                Language = rq.Language,
                IsActive = true,
                MovieTypeId = rq.MovieTypeId,
                RateId = rq.RateId,
            };

            _context.Movies.Add(newMovie);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Thêm movie thành công",_converter.EntityToDTO(newMovie));
        }

        public async Task<ResponseObject<DataResponseMovie>> DeleteMovie(int id)
        {
            if (InputHelper.checkNull(new string[] { id.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id cần xóa", null);

            var movieCr = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (movieCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại hoặc không hoạt động", null);

            movieCr.IsActive = false;
            _context.Movies.Update(movieCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Xóa thành công", _converter.EntityToDTO(movieCr));
        }

        public async Task<ResponseObject<DataResponseMovie>> DetailMovie(int id)
        {
            if (InputHelper.checkNull(new string[] { id.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id cần xem", null);

            var movieCr = await _context.Movies.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
            if (movieCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại hoặc không hoạt động", null);

            return _responseObject.ResponseSuccess("Lấy thông tin thành công", _converter.EntityToDTO(movieCr));

        }

        public async Task<ResponseObject<List<DataResponseMovie>>> ListMoviesOfCinemas(Pagination pagination,int cinemaId)
        {
            if (InputHelper.checkNull(new string[] { cinemaId.ToString() }))

                return _reponseObjectListMovie.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id cần xem", null);

            var movies = _context.Movies
                          .Where(m => m.IsActive)
                          .Where(m => m.Schedules.Any(s =>
                              s.IsActive &&
                              s.EndAt >= DateTime.Now && 
                              s.Room.CinemaId == cinemaId &&
                              s.Room.Cinema.IsActive
                          ))
                          .Include(m => m.MovieType)
                          .Include(m => m.Rate)
                          .Select(m => _converter.EntityToDTO(m));

            var result = await Result(pagination, movies).ToListAsync();


            return _reponseObjectListMovie.ResponseSuccess("Lấy danh sách phim thành công", result);
        }

        public async Task<ResponseObject<DataResponseMovie>> UpdateMovie(Request_UpdateMovie rq)
        {
            if(rq == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            if(rq.MovieDuration<=0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Thời lượng phim không hợp lệ", null);


            if (rq.PremiereDate < DateTime.UtcNow)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Thời gian ra mắt phim không hợp lệ", null);

            var movieCr = await _context.Movies.FirstOrDefaultAsync(x => x.Id == rq.Id && x.IsActive == true);

            if(movieCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại", null);


            var isDuplicateName = await _context.Movies.AnyAsync(x => x.Name == rq.Name && x.IsActive == true && x.Id == rq.Id);

            if(isDuplicateName )
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên phim bị trùng", null);


            movieCr.Name = rq.Name??movieCr.Name;
            movieCr.Trailer = rq.Trailer ?? movieCr.Trailer;
            movieCr.MovieDuration = rq.MovieDuration?? movieCr.MovieDuration;
            movieCr.PremiereDate = rq.PremiereDate ?? movieCr.PremiereDate;
            movieCr.Description = rq.Description ?? movieCr.Description;
            movieCr.Director = rq.Director ?? movieCr.Director;
            movieCr.HeroImage = rq.HeroImage ?? movieCr.HeroImage;
            movieCr.Language = rq.Language ?? movieCr.Language;

            _context.Movies.Update(movieCr);
            await _context.SaveChangesAsync();


            return _responseObject.ResponseSuccess("Cập nhật thành công",_converter.EntityToDTO(movieCr));
        }

        public async Task<ResponseObject<List<DataResponseMovie>>> GetTopViewMovies(Pagination pagination)
        {
            var topMovies = _context.Movies
                            .Select(movie => new
                            {
                                Movie = movie,
                                TicketCount = _context.BillTickets
                                    .Count(bt => _context.Tickets
                                        .Any(t => t.Id == bt.TicketId &&
                                                  _context.Schedules
                                                      .Any(s => s.Id == t.ScheduleId && s.MovieId == movie.Id)))
                            })
                            .OrderByDescending(m => m.TicketCount)
                            .Select(m => _converter.EntityToDTO(m.Movie));

            var result = await Result(pagination, topMovies).ToListAsync();

            return _reponseObjectListMovie.ResponseSuccess("Lấy danh sách phim có lượng xem cao nhất thành công", result);
        }

        public async Task<ResponseObject<List<DataResponseMovie>>> SearchMovie(string nameMovie ,Pagination pagination)
        {
            if(nameMovie == null)
                return _reponseObjectListMovie.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            var movies = _context.Movies
                                .Include(m => m.Rate)
                                .Include(m => m.MovieType)
                                .Where(m => m.Name.ToLower().Contains(nameMovie.ToLower()))
                                .Select(m => _converter.EntityToDTO(m));

            var result = await Result(pagination, movies).ToListAsync();

            return _reponseObjectListMovie.ResponseSuccess("Tim kiếm theo tên thành công", result);

        }
    }
}
