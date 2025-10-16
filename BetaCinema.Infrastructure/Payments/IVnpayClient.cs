using BetaCinema.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Payments
{
    public interface IVnpayClient
    {
        Task<VnpayQueryDrResponse?> QueryDrAsync(VnpayQueryDrRequest req,string hashSecret,CancellationToken ct = default);
    }
}
