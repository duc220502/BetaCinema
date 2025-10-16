using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
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
    public class SeatStatusRepository(AppDbContext context) : BaseRepository<Domain.Entities.Seats.SeatStatus>(context), ISeatStatusRepository
    {
        public async Task<Domain.Entities.Seats.SeatStatus?> GetDefaultSeatStatusAsync()
         => await _context.SeatStatuses.FirstOrDefaultAsync(r => r.Id == (int)Domain.Enums.SeatStatus.Available);
    }
}
