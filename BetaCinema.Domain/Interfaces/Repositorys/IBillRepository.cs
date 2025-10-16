using BetaCinema.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IBillRepository : IRepository<Bill>
    {
        Task<Bill?> GetBillDetailsForResponseAsync(Guid billId);

        Task<Bill?> GetBillWithDetailsAsync(Guid billId);

        Task<Bill?> GetBillForCancellationAsync(Guid billId);

        Task<List<Bill>> FindExpiredPendingBillsAsync();
    }
}
