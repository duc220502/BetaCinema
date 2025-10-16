using BetaCinema.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IPriceRepository : IRepository<PriceTier>
    {
        Task<PriceTier?> FindBestMatchingRuleAsync(DateTime scheduleStartAt);
    }
}
