namespace BetaCinema.Entities
{
    public class MovieType:BaseEntity
    {
        public string MovieTypeName { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
