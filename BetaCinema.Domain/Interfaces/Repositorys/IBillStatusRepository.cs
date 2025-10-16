using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IBillStatusRepository : IRepository<BillStatus>
    {
        Task<BillStatus?> GetDefaultBillStatusAsync();
    }
}
