using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class ScheduleRepository(AppDbContext context) : BaseRepository<Schedule>(context), IScheduleRepository
    {
        public async Task<Schedule?> GetScheduleByBillIdAsync(Guid billId)
        => await _context.Schedules.AsNoTracking()
            .FirstOrDefaultAsync(s =>
                s.Tickets.Any(t =>
                    t.BillTickets.Any(bt => bt.BillId == billId)
                )
            );

        public async Task<Schedule?> GetScheduleDetailByIdAsync(Guid id)
        => await _context.Schedules.Include(x=>x.Movie).FirstOrDefaultAsync(x=>x.Id == id && x.IsActive );

        public async Task<IEnumerable<Schedule>> GetSchedulesByRoomAndDateAsync(Guid roomId, DateTime startDate, DateTime endDate)
        => await _context.Schedules.Where(s => s.RoomId == roomId && s.StartAt >= startDate && s.StartAt < endDate).OrderBy(s => s.StartAt).ToListAsync();

        public async Task<bool> IsTimeSlotOverlappingAsync(Guid roomId, DateTime startAt, DateTime endAt, int bufferTime)
        => await _context.Schedules.AnyAsync(s =>s.RoomId == roomId && s.IsActive && startAt < s.EndAt.AddMinutes(bufferTime) && endAt.AddMinutes(bufferTime) > s.StartAt);



    }
}
