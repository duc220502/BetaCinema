using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = default!;
    }
}
