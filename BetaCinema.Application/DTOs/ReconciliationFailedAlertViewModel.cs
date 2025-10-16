using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{ 
    public class ReconciliationFailedAlertViewModel
    {
        public Guid BillId { get; set; }

        public string TradingCode { get; set; } = default!;

        public decimal DbAmount { get; set; }

        public decimal GatewayAmount { get; set; }

        public string PaymentGatewayTransactionId { get; set; } = default!;

        public DateTime AlertTime { get; set; }
    }
}
