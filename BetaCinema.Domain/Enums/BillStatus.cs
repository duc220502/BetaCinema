using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Enums
{
    public enum BillStatus
    {
        PendingPayment = 1 ,
        Paid  = 2,
        CancelledByUser = 3 ,
        Expired = 4,
        Failed =5,
        Refunded = 6,
        ReconciliationFailed = 7,
    }
}
