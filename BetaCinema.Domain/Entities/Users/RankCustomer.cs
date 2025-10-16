using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Users
{
    public class RankCustomer
    {
        public int Id { get; set; }
        public int MinimumPoint { get; set; }

        public string RankName { get; set; } = default!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
