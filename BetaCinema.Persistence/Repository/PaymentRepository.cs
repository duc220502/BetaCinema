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
    public class PaymentRepository(AppDbContext context) : BaseRepository<PaymentMethod>(context), IPaymentRepository
    {
        public async Task<PaymentMethod?> GetPaymentByIdAsync(int id)
        => await _context.PaymentMethods.FirstOrDefaultAsync(x => x.Id == id);
    }
}
