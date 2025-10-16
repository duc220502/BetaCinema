using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Orders
{
    public class BillFood:BaseEntity
    {
        public int Quantity { get; set; }
        public Guid BillId { get; set; }
        public virtual Bill? Bill { get; set; } 

        public Guid FoodId { get; set; }
        public virtual Food? Food { get; set; }
    }
}
