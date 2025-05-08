using BetaCinema.Entities;
using BetaCinema.PayLoads.DataResponses;
using System.IO;
using System.Xml.Linq;

namespace BetaCinema.PayLoads.Convertest
{
    public class MovieConverter:BaseConverter
    {
        public DataResponseMovie EntityToDTO(Movie movie)
        {

            return new DataResponseMovie
            {
                Name = movie.Name,
                Trailer = movie.Trailer,
                MovieDuration = movie.MovieDuration,
                PremiereDate = movie.PremiereDate,
                Description = movie.Description,
                Director = movie.Director,
                HeroImage = movie.HeroImage,
                Language = movie.Language,
                StatusActive = movie.IsActive ? "Hoạt động" : "Không hoạt động",
                RateName = _context.Rates.FirstOrDefault(x => x.Id == movie.RateId)?.Description??"NoRate",
                MovieTypeName = _context.MovieTypes.FirstOrDefault(x=> x.Id == movie.MovieTypeId)?.MovieTypeName??"NoMovieType"

            };
        }
    }
}
