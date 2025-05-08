namespace BetaCinema.Entities
{
    public class BillStatus:BaseEntity
    {
        public string StatusName { get; set; }

        public virtual ICollection<Bill> Bills { get; set; }
    }
}
