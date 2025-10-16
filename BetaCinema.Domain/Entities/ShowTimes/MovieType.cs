using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.ShowTimes
{
    public class MovieType:BaseEntity
    {
        public string MovieTypeName { get; set; } = default!;

        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
