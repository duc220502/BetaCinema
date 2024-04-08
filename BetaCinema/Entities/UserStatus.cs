namespace BetaCinema.Entities
{
    public class UserStatus:BaseEntity
    {
        public string Code { get; set; }
        public string  Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
