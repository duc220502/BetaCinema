using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class AppliedPromotionDetailDto 
    {
        public Guid PromotionId { get; set; }
        public string Name { get; set; } = default!;

        public decimal DiscountAmount { get; set; }
    }
}
