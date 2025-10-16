using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Users
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = default!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
