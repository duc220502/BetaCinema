using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<List<Promotion>?> GetPromotionsByListIdAsync(List<Guid> ids);
    }
}
