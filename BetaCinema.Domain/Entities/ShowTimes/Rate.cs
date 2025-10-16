using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.ShowTimes
{
    public class Rate
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
