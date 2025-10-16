using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Promotions
{
    public class Request_UpdatePromotion
    {
        public string? Name { get; set; } = default!;
        public decimal? DiscountValue { get; set; }

        public decimal? MinBillValue { get; set; }

        public decimal? MaxDiscountValue { get; set; }

        public int? UsageLimit { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
