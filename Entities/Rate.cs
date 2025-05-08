namespace BetaCinema.Entities
{
    public class Rate:BaseEntity
    {
        public string Description { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Movie>  Movies { get; set; }
    }
}
