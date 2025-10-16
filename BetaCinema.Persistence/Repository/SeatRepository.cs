using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Enums;
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
    public class SeatRepository(AppDbContext context) : BaseRepository<Seat>(context), ISeatRepository
    {
        public async Task<List<Seat>> GetAvailableSeatsAsync(List<Guid>? seatIds, Guid roomId)
        {
            if (seatIds == null || !seatIds.Any())
                return new List<Seat>();

            var availableSeats = await _context.Seats
                .Include(s => s.SeatType)
                .Where(s => seatIds.Contains(s.Id) && s.RoomId == roomId &&  s.SeatStatusId == (int) Domain.Enums.SeatStatus.Available && s.IsActive)
                .ToListAsync();

            return availableSeats;
        }

        public async Task<List<Seat>> GetSeatsByScheduleAsync(Guid scheduleId)
        {
            var room = await _context.Schedules
            .Where(s => s.Id == scheduleId)
            .Select(s => s.Room)
            .FirstOrDefaultAsync();

            if (room == null)
                return new List<Seat>();
            
            var seats = await _context.Seats
                .Include(s => s.SeatType)
                .Include(s => s.SeatStatus)
                .Where(s => s.RoomId == room.Id)
                .OrderBy(s => s.Line)
                .ThenBy(s => s.Number)
                .AsNoTracking()
                .ToListAsync();

            return seats;
        }

        public async Task<bool> IsSeatUniqueAsync(int numberSeat, LineSeat lineSeat, Guid roomId, Guid? id = null)
        {
            if(id == null)
                return !await _context.Seats.AnyAsync(x => x.Number == numberSeat && x.Line == lineSeat && x.RoomId == roomId && x.IsActive);

            return !await _context.Seats.AnyAsync(x => x.Number == numberSeat && x.Line == lineSeat && x.RoomId == roomId && x.IsActive && x.Id != id);
        }

        public async Task<int> ReleaseExpiredHeldSeatsForScheduleAsync(Guid scheduleId)
        {
            var currentTime = DateTime.UtcNow;

            var seatIdsToRelease = await _context.Bills

                .Where(b => b.BillStatusId == (int)Domain.Enums.BillStatus.PendingPayment &&
                            b.ExpireAt.HasValue &&
                            b.ExpireAt < currentTime)
    
                .SelectMany(b => b.BillTickets)
                .Select(bt => bt.Ticket)
                .Where(t => t!.ScheduleId  == scheduleId &&
                            t.Seat!.SeatStatusId == (int)Domain.Enums.SeatStatus.Held)
                .Select(t => t!.SeatId)
                .ToListAsync();

            if (!seatIdsToRelease.Any())
                return 0;

            return await _context.Seats
                .Where(s => seatIdsToRelease.Contains(s.Id))
                .ExecuteUpdateAsync(updates =>
                    updates.SetProperty(s => s.SeatStatusId, (int)Domain.Enums.SeatStatus.Available));
        }

        public void UpdateRange(List<Seat> seats)
        {
           _context.Seats.UpdateRange(seats);
        }
    } 
}
