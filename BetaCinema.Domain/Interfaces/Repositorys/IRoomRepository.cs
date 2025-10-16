using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<bool> IsRoomNameUniqueAsync(string? name, Guid? cinemaId, Guid? id = null);

        Task<Room?> GetRoomByIdAsync(Guid id);
    }
}
