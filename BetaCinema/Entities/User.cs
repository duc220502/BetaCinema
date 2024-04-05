namespace BetaCinema.Entities
{
    public class User:BaseEntity
    {
        public int Point { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
