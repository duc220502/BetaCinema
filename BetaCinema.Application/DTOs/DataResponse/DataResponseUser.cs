using BetaCinema.Application.DTOs.DataResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOS.DataResponse
{
    public class DataResponseUser
    {
        public Guid Id { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string NumberPhone { get; set; } = default!;

        public bool IsActive { get; set; } = default!;
        public int Point { get; set; }

        public DataResponseRole Role { get; set; } = default!;
        public DataResponseUserStatus UserStatus { get; set; } = default!;

        public DataResponseRankCustomer RankCustomer { get; set; } = default!;
    }
}
