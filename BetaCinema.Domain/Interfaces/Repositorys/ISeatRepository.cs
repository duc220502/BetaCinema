using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface ISeatRepository : IRepository<Seat>
    {
        Task<bool> IsSeatUniqueAsync(int numberSeat,LineSeat lineSeat, Guid roomId, Guid? id = null);

        Task<List<Seat>> GetAvailableSeatsAsync(List<Guid>? seatIds, Guid roomId);

        public void UpdateRange(List<Seat> seats);

        public Task<int> ReleaseExpiredHeldSeatsForScheduleAsync(Guid scheduleId);

        Task<List<Seat>> GetSeatsByScheduleAsync(Guid scheduleId);
    }
}
