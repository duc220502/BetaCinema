using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Payloads.Convertes
{
    public class CinemaConverter
    {
        private readonly AppDbContext _context;

        public CinemaConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseCinema EntityToDTO(Cinema cnm)
        {
            return new DataResponseCinema
            {
                Address = cnm.Address,
                Description = cnm.Description,
                Code = cnm.Code,
                NameOfCinema = cnm.NameOfCinema,
                ActiveStatus = cnm.IsActive?"Hoạt động":"Không hoạt động"
            };
        }
    }
}
