using BetaCinema.Domain.Entities.Orders;
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
    public class BillStatusRepository(AppDbContext context) : BaseRepository<BillStatus>(context), IBillStatusRepository
    {
        public async  Task<BillStatus?> GetDefaultBillStatusAsync()
        => await _context.BillStatuses.FirstOrDefaultAsync(r => r.StatusName == Domain.Enums.BillStatus.PendingPayment.ToString());
    }
}
