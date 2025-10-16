using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Seats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        Task<List<Ticket>> GetTicketsByBillIdAsync(Guid billId);
    }
}
