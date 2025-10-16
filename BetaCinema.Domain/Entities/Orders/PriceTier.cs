using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Orders
{
    public class PriceTier : BaseEntity
    {
        public string?  Name  { get; set; }

        public decimal AdjustmentValue { get; set; }

        public AdjustmentType AdjustmentType { get; set; }

        public int? ApplicableDay { get; set; }

        public DateOnly? ApplicableDate { get; set; }

        public TimeOnly StartAt { get; set; }

        public TimeOnly EndAt { get; set; }
        public int PriceRuleTypeId { get; set; }
        public PriceRuleType? PriceRuleType { get; set; }


    }
}
