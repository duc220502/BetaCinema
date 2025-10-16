using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class PromotionResult
    {
        public decimal SubTotal { get; set; }

        public decimal FinalTotal { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        public List<AppliedPromotionDetailDto> AppliedPromotions { get; set; } = default!;
    }
}
