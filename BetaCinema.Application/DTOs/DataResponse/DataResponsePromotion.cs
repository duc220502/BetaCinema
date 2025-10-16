using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponsePromotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public PromotionScope Scope { get; set; } = default!;
        public DiscountType DiscountType { get; set; } = default!;
        public decimal DiscountValue { get; set; }

        public decimal? MinBillValue { get; set; }

        public decimal? MaxDiscountValue { get; set; }

        public int UsageLimit { get; set; }
        public int CurrentUsage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
        public Guid? RankCustomerId { get; set; }
    }
}
