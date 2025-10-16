using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IPriceService
    {
        Task<decimal> CalculateBaseTicketPriceAsync(Schedule schedule);
    }
}
