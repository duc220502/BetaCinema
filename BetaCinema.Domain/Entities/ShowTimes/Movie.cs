using BetaCinema.Domain.Interfaces;
using System.Xml.Schema;

namespace BetaCinema.Domain.Entities.ShowTimes
{
    public class Movie : BaseEntity
    {
        public string? Name { get; set; }
        public string? Trailer { get; set; }
        public int MovieDuration { get; set; }
        public DateTime PremiereDate { get; set; }

        public decimal BasePrice { get; set; }
        public string? Description { get; set; }

        public string?  Director { get; set; }

        public string? HeroImage { get; set; }

        public string? Language { get; set; }

        public bool IsActive { get; set; }

        public Guid MovieTypeId { get; set; }
        public virtual MovieType? MovieType { get; set; }

        public int RateId { get; set; }
        public virtual Rate? Rate { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
