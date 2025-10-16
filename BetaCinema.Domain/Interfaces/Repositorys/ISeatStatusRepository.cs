using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface ISeatStatusRepository : IRepository<SeatStatus>
    {
        Task<SeatStatus?> GetDefaultSeatStatusAsync();
    }
}
