using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface ISeatTypeRepository : IRepository<SeatType>
    {
        Task<SeatType?> GetSeatTypeByIdAsync(int id);
    }
}
