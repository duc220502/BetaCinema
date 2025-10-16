using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface ICinemaRepository : IRepository<Cinema>
    {

        Task<PageResult<Cinema>> GetAllCinemasAsync(Pagination pagination);

        Task<Cinema?> GetCinemaByIdAsync(Guid id);

        Task<bool> IsNameUniqueAsync(string? name, Guid? id = null);
    }
}
