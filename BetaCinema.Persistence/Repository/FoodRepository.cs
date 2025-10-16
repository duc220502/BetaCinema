using BetaCinema.Domain.Entities.Foods;
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
    public class FoodRepository(AppDbContext context) : BaseRepository<Food>(context), IFoodRepository
    {
        public async Task<Food?> GetFoodByIdAsync(Guid id)
         => await _context.Foods.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);

        public async Task<List<Food>?> GetFoodsByIdsAsync(List<Guid> ids)
        => await _context.Foods.Where(f => ids.Contains(f.Id) && f.IsActive).ToListAsync();

        public async Task<bool> IsFoodNameUniqueAsync(string? name , Guid? id)
        {
           if(id == null)
                return !await _context.Foods.AnyAsync(x=>x.Name == name);

            return !await _context.Foods.AnyAsync(x => x.Name == name && x.Id !=id);
        }
        
         
    }
}
