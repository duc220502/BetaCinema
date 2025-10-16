using BetaCinema.Domain.Entities.Users;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IRankCustomerRepository : IRepository<RankCustomer>
    {
        //Task<RankCustomer?> GetDefaultRankAsync();

        Task<IEnumerable<RankCustomer>?> GetAllRankCustomersDesAsync();
    }
}
