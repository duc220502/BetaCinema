using BetaCinema.Entities;
using BetaCinema.PayLoads.DataResponses;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.PayLoads.Convertest
{
    public class TicketConverter:BaseConverter
    {
        public DataResponseTicket EntityToDTO(Ticket tk)
        {
            var ticket = _context.Tickets
                        .Include(t => t.Schedule)
                            .ThenInclude(s => s.Room)
                        .Include(t => t.Seat)
                        .FirstOrDefault(x => x.Id == tk.Id);
            return new DataResponseTicket
            {
                PriceTicket = tk.PriceTicket,
                StatusActive = tk.IsActive?"Hoạt động":"Không hoạt động",
                StatusLocked = tk.IsLocked == true?"Tạm khóa":"Không khóa",
                LockedTime = tk.LockedTime,
                LockByUserName = _context.Users.FirstOrDefault(x=>x.Id == tk.LockByUserId)?.FullName,
                ScheudleTime = ticket.Schedule.StartAt,
                SeatNumber = ticket.Seat.Number,
                RoomName =ticket.Schedule.Room.Name
            };
        }
    }
}
