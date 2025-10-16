using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Seats;
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
    public class TicketRepository(AppDbContext context) : BaseRepository<Ticket>(context), ITicketRepository
    {
        public async Task<List<Ticket>> GetTicketsByBillIdAsync(Guid billId)
        => await _context.Tickets
            .Where(t => t.BillTickets.Any(bt => bt.BillId == billId))
            .ToListAsync();
    }
}
