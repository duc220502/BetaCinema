using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Users
{
    public class UserStatus
    {
        public  int  Id { get; set; }
        public string StatusName { get; set; } = default!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
