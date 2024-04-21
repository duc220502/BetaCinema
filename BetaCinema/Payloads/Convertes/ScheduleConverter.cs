using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Payloads.Convertes
{
    public class ScheduleConverter
    {
        private readonly AppDbContext _context;

        public ScheduleConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseSchedule EntityToDTO(Schedule schedule)
        {

            return new DataResponseSchedule
            {
                Price = schedule.Price,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                Code = schedule.Code,
                Name = schedule.Name,
                IsActive = schedule.IsActive ? "Hoạt động" : "Không hoạt động",
                MovieName = _context.Movies.FirstOrDefault(x => x.Id == schedule.MovieId).Name,
                RoomName = _context.Rooms.FirstOrDefault(x=>x.Id == schedule.RoomId).Name
            };
        }
    }
}
