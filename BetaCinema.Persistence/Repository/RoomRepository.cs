using BetaCinema.Domain.Entities.ShowTimes;
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
    public class RoomRepository(AppDbContext context) : BaseRepository<Room>(context), IRoomRepository
    {
        public async Task<Room?> GetRoomByIdAsync(Guid id)
        => await _context.Rooms.FirstOrDefaultAsync(x=>x.Id ==  id && x.IsActive );

        public async Task<bool> IsRoomNameUniqueAsync(string? name, Guid? cinemaId, Guid? id = null)
        {
            if (id == null)
                return !await _context.Rooms.AnyAsync(x => x.Name == name && x.CinemaId == cinemaId);

            return !await _context.Rooms.AnyAsync(x => x.Name == name && x.CinemaId == cinemaId && x.Id != id);
        }
    }
}
