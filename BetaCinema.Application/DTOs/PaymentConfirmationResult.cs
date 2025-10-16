using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class PaymentConfirmationResult
    {
        public bool IsSuccess { get; set; }
        public Guid BillId { get; set; }
        public decimal Amount { get; set; }
        public string? PaymentGatewayTransactionId { get; set; }
        public string? PayDate { get; set; }

        public string? TxnRef { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
