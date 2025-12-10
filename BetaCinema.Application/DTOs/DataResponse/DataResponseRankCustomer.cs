using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseRankCustomer
    {
        public int Id { get; set; }
        public int MinimumPoint { get; set; }

        public string RankName { get; set; } = default!;

        public string Description { get; set; } = default!;
    }
}
