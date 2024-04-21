using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Payloads.Convertes
{
    public class RoomConverter
    {
        private readonly AppDbContext _context;

        public RoomConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseRoom EntityToDTO(Room room)
        {
            return new DataResponseRoom
            {
                Capacity = room.Capacity,
                Type = room.Type,
                Description = room.Description,
                Code = room.Code,
                Name = room.Name,
                ActiveStatus = room.IsActive?"Hoạt động":"Không hoạt động",
                CinemaName = _context.Cinemas.FirstOrDefault(x=>x.Id==room.CinemaId).NameOfCinema

            };
        }
    }
}
