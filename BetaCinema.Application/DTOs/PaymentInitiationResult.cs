using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class PaymentInitiationResult
    {
        public bool IsSuccess { get; set; }
        public string? RedirectUrl { get; set; }
        public object? QrCodeData { get; set; } 
        public string Message { get; set; } = default!;
    }
}
