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
    public class SeatTypeRepository(AppDbContext context) : BaseRepository<SeatType>(context), ISeatTypeRepository
    {
        public async Task<SeatType?> GetSeatTypeByIdAsync(int id)
        => await _context.SeatTypes.FirstOrDefaultAsync(x => x.Id == id);
    }
}
