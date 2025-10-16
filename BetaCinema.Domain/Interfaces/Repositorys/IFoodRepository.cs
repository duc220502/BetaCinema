using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IFoodRepository: IRepository<Food>
    {
        Task<bool> IsFoodNameUniqueAsync(string? name,Guid? id = null);
        Task<Food?> GetFoodByIdAsync(Guid id);

        Task<List<Food>?> GetFoodsByIdsAsync(List<Guid> id);
    }
}
