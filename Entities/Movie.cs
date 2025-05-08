namespace BetaCinema.Entities
{
    public class Movie : BaseEntity
    {
        public string Name { get; set; }
        public string Trailer { get; set; }
        public int MovieDuration { get; set; }
        public DateTime PremiereDate { get; set; }

        public string Description { get; set; }

        public string  Director { get; set; }

        public string HeroImage { get; set; }

        public string Language { get; set; }

        public bool IsActive { get; set; }

        public int MovieTypeId { get; set; }
        public MovieType MovieType { get; set; }

        public int RateId { get; set; }
        public Rate Rate { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
