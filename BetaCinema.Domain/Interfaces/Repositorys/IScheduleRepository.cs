using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IScheduleRepository : IRepository<Schedule>
    {


        Task<Schedule?> GetScheduleDetailByIdAsync(Guid id);
        Task<bool> IsTimeSlotOverlappingAsync(Guid roomId, DateTime startAt, DateTime endAt , int bufferTime);

        Task<IEnumerable<Schedule>> GetSchedulesByRoomAndDateAsync(Guid roomId, DateTime startDate, DateTime endDate);

        Task<Schedule?> GetScheduleByBillIdAsync(Guid billId);
    }

}
