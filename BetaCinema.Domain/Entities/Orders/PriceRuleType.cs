using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Orders
{
    public class PriceRuleType
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public int PriorityValue { get; set; }

        public virtual ICollection<PriceTier> PriceTiers { get; set; } = new List<PriceTier>();

    }
}
