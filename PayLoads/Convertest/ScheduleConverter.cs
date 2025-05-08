using BetaCinema.Entities;
using BetaCinema.PayLoads.DataResponses;
using System.Xml.Linq;

namespace BetaCinema.PayLoads.Convertest
{
    public class ScheduleConverter:BaseConverter
    {
        public DataResponseSchedule EntityToDTO(Schedule schedule)
        {

            return new DataResponseSchedule
            {
                Name = schedule.Name,
                StartAt = schedule.StartAt,
                EndAt = schedule.EndAt,
                StatusActive = schedule.IsActive ? "Hoạt động" : "Không hoạt động",
                MovieName = _context.Movies.FirstOrDefault(x => x.Id == schedule.MovieId && x.IsActive)?.Name,
                RoomName = _context.Rooms.FirstOrDefault(x=>x.Id == schedule.RoomId && x.IsActive)?.Name
            };
        }
    }
}
