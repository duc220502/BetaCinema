using BetaCinema.Entities;
using BetaCinema.PayLoads.DataResponses;
using System.Xml.Linq;

namespace BetaCinema.PayLoads.Convertest
{
    public class CinemaConverter : BaseConverter
    {
        public DataResponseCinema EntityToDTO(Cinema cinema)
        {
            return new DataResponseCinema
            {
                Name = cinema.Name,
                Address = cinema.Address,
                Description = cinema.Description,
                StatusActive = cinema.IsActive?"Hoạt động":"Không hoạt động"
            };
        }
    }
}
