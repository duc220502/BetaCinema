using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class PreparedPromotionDto 
    {
        public Guid PromotionId { get; set; }
        public string Name { get; set; } = default!;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }

    }
}
