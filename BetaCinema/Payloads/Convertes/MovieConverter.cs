using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace BetaCinema.Payloads.Convertes
{
    public class MovieConverter
    {
        private readonly AppDbContext _context;

        public MovieConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseMovie EntityToDTO(Movie mv)
        {
            return new DataResponseMovie
            {
                MovieDuration = mv.MovieDuration,
                PremiereDate = mv.PremiereDate,
                Description = mv.Description,
                Director = mv.Director,
                Image = mv.Image,
                HeroImage = mv.HeroImage,
                Language = mv.Language,
                Name = mv.Name,
                Trailer = mv.Trailer,   
                IsActiveStatus = mv.IsActive?"Hoạt động":"Không hoạt động",
                MovieTypeName = _context.MovieTypes.FirstOrDefault(x=>x.Id == mv.MovieTypeId).MovieTypeName,
                RateNumber = _context.Rates.FirstOrDefault(x=>x.Id == mv.RateId).Description

            };
        }
    }
}
