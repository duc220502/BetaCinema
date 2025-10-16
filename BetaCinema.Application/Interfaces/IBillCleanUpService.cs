using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IBillCleanUpService
    {
        Task ProcessExpiredBillsAsync();
        Task ReleaseExpiredSeatsForScheduleAsync(Guid scheduleId);
    }
}
