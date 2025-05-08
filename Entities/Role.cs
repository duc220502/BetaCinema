namespace BetaCinema.Entities
{
    public class Role:BaseEntity
    {
        public string Code { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
