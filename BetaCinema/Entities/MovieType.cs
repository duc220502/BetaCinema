namespace BetaCinema.Entities
{
    public class MovieType:BaseEntity
    {
        public string MovieTypeName { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<Movie> Movies { get; set; }
    }
}
