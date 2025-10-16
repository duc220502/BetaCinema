using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class SecurityAlertViewModel
    {
        public Guid BillId { get; set; }

        public string TradingCode { get; set; } = default!;

        public decimal Amount { get; set; }

        public string GatewayTransactionId { get; set; } = default!;

        public DateTime  Alertime { get; set; }

    }
}
