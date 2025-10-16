using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Orders
{
    public class PaymentMethod 
    {
        public int Id { get; set; }

        public string MethodCode { get; set; } = default!;

        public string MethodName { get; set; } = default!;

        public int? ExpirationTimeInMinutes { get; set; }

        public bool IsActive { get; set; }


    }
}
