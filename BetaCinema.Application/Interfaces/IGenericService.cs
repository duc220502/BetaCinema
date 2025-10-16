using BetaCinema.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IGenericService<TEntity ,TDTO> 
    {
        Task<ResponseObject<TDTO>> GetByIdAsync(Guid id);
        Task<ResponseObject<IEnumerable<TDTO>>> GetAllAsync();
    }
}
