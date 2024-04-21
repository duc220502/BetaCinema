using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BetaCinema.Services.Implements
{
    public class MovieService : BaseService, IMovieService
    {
        private readonly ResponseObject<IEnumerable<DataResponseMoviesQuanTiTy>> _responseObjects;
        private readonly ResponseObject<DataResponseMovieDetail> _responseObjectMovieDetail;
        private readonly ResponseObject<DataResponseMovie> _responseObjectMovie;
        private readonly MovieConverter _converter;

       

        public MovieService()
        {
            _responseObjects = new ResponseObject<IEnumerable<DataResponseMoviesQuanTiTy>>();
            _responseObjectMovieDetail = new ResponseObject<DataResponseMovieDetail>();
            _responseObjectMovie = new ResponseObject<DataResponseMovie>();
            _converter = new MovieConverter();

        }

        public ResponseObject<DataResponseMovie> CreateMovie(Request_AddMovie rq)
        {
            bool checkMovieType = _context.MovieTypes.Any(x => x.Id == rq.MovieTypeId);
            if (!checkMovieType)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "MovieType không tồn tại", null);
            bool checkRate = _context.Rates.Any(x=>x.Id == rq.RateId);

            if (!checkRate)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Rate không tồn tại", null);

            if (rq.PremiereDate < DateTime.UtcNow)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Thời gian khởi chiếu không thể nhỏ hơn thời gian hiện tại", null);

            Movie movie = new Movie()
            {
                MovieDuration = rq.MovieDuration,
                PremiereDate = rq.PremiereDate,
                Description = rq.Description,
                Director = rq.Director,
                Image = rq.Image,
                HeroImage = rq.HeroImage,
                Language = rq.Language,
                Name = rq.Name,
                Trailer = rq.Trailer,
                IsActive = true,
                MovieTypeId = rq.MovieTypeId,
                RateId = rq.RateId,
            };

            _context.Movies.Add(movie);
            _context.SaveChanges();

            return _responseObjectMovie.ResponseSuccess("Thành công", _converter.EntityToDTO(movie));

        }

        public ResponseObject<DataResponseMovie> DeleteMovie(int id)
        {
            var movieCr = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movieCr == null)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại", null);

            movieCr.IsActive = false;

            _context.Movies.Update(movieCr);
            _context.SaveChanges();

            return _responseObjectMovie.ResponseSuccess("Xóa thành công",_converter.EntityToDTO(movieCr));
        }

        public ResponseObject<DataResponseMovieDetail> get_MovieDetail(int id)
        {
            if (InputHelper.checkNull(id.ToString()))
                return _responseObjectMovieDetail.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin đầy đủ", null);

            var movieDetails = _context.Movies
                .Where(m => m.Id == id)
                .Select(m => new DataResponseMovieDetail
                {
                    MovieName = m.Name,
                    Rooms = _context.Schedules
                        .Where(s => s.MovieId == id)
                        .SelectMany(s => s.Room.Seats.Select(seat => new DataResponseMovieRoom
                        {
                            RoomName = s.Room.Name,
                            Seats = s.Room.Seats.Select(seat => new DataResponseRoomSeat
                            {
                                Number = seat.Number,
                                ActiveStatus = seat.IsActive ? "Hoạt động" : "Không hoạt động",
                                SeatStatus = seat.SeatStatus.NameStatus
                            }).ToList()
                        })).ToList()
                }).FirstOrDefault();

            if (movieDetails == null)
            {
                return _responseObjectMovieDetail.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại", null);
            }

            // Kiểm tra xem có lịch chiếu cho bộ phim hay không
            var hasSchedule = movieDetails.Rooms != null && movieDetails.Rooms.Any();
            if (!hasSchedule)
            {
                // Nếu không có lịch chiếu, trả về thông tin phim với các phần còn lại được gán bằng null hoặc giá trị mặc định
                movieDetails.Rooms = null;
                // Các phần còn lại có thể được gán bằng null hoặc giá trị mặc định tùy thuộc vào yêu cầu của bạn
            }

            return _responseObjectMovieDetail.ResponseSuccess("Thành công", movieDetails);
        }


        public ResponseObject<IEnumerable<DataResponseMoviesQuanTiTy>> get_MovieOutStanding()
        {
            try
            {
                var featureMovies = _context.BillTickets.Include(bt => bt.Ticket)
                    .ThenInclude(tk => tk.Schedule)
                    .ThenInclude(sc => sc.Movie)
                    .GroupBy(bt => bt.Ticket.Schedule.Movie.Name).
                    Select(group => new DataResponseMoviesQuanTiTy
                    {
                        MovieName = group.Key,
                        Quantity = group.Sum(bt => bt.Quantity)
                    })
                    .OrderByDescending(bt => bt.Quantity)
                    .Take(5);
                return _responseObjects.ResponseSuccess("Thành công",featureMovies);

            }
            catch (Exception ex)
            {
                return _responseObjects.ResponseError(StatusCodes.Status400BadRequest, ex.Message, null);
            }
        }

        public ResponseObject<DataResponseMovie> UpdateMovie(int id, Request_UpdateMovie rq)
        {
            var movieCr = _context.Movies.FirstOrDefault(x => x.Id == id && x.IsActive == true);
            if (movieCr == null)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Movie không tồn tại", null);

            bool checkMovieType = _context.MovieTypes.Any(x => x.Id == rq.MovieTypeId);
            if (!checkMovieType)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "MovieType không tồn tại", null);
            bool checkRate = _context.Rates.Any(x => x.Id == rq.RateId);

            if (!checkRate)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Rate không tồn tại", null);

            if (rq.PremiereDate < DateTime.UtcNow)
                return _responseObjectMovie.ResponseError(StatusCodes.Status400BadRequest, "Thời gian khởi chiếu không thể nhỏ hơn thời gian hiện tại", null);

            movieCr.MovieDuration = rq.MovieDuration;
            movieCr.PremiereDate = rq.PremiereDate;
            movieCr.Description = rq.Description;
            movieCr.Director = rq.Director;
            movieCr.Image = rq.Image;
            movieCr.HeroImage = rq.HeroImage;
            movieCr.Language = rq.Language;
            movieCr.Name = rq.Name;
            movieCr.Trailer = rq.Trailer;
            movieCr.MovieTypeId = rq.MovieTypeId;
            movieCr.RateId = rq.RateId;


            _context.Movies.Update(movieCr);
            _context.SaveChanges();

            return _responseObjectMovie.ResponseSuccess("Sửa Thành công",_converter.EntityToDTO(movieCr));
        }
    }
}
