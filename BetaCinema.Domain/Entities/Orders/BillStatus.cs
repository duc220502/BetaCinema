using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Orders
{
    public class BillStatus
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string StatusName { get; set; } = default!;

        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
    }
}
