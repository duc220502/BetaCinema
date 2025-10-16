using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using BetaCinema.Persistence.Extensions;
using BetaCinema.Shared.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class CinemaRepository(AppDbContext context) : BaseRepository<Cinema>(context), ICinemaRepository
    {
        public async Task<PageResult<Cinema>> GetAllCinemasAsync(Pagination pagination)
         => await _context.Cinemas.ToPagedListAsync(pagination);

        public async Task<Cinema?> GetCinemaByIdAsync(Guid id)
       => await _context.Cinemas.FirstOrDefaultAsync(x=>x.Id == id && x.IsActive == true);

        public async Task<bool> IsNameUniqueAsync(string? name, Guid ?id)
        {
            if(id == null)
                return !await _context.Cinemas.AnyAsync(x => x.Name == name);

           return  !await _context.Cinemas.AnyAsync(u => u.Name == name && u.Id != id);
        }
 
    }
}
