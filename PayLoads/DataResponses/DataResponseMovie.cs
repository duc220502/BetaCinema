namespace BetaCinema.PayLoads.DataResponses
{
    public class DataResponseMovie
    {
        public string Name { get; set; }
        public string Trailer { get; set; }
        public int MovieDuration { get; set; }

        public DateTime PremiereDate { get; set; }

        public string Description { get; set; }

        public string Director { get; set; }

        public string HeroImage { get; set; }

        public string Language { get; set; }


        public string StatusActive { get; set; }

        public string  RateName { get; set; }

        public string MovieTypeName { get; set; }
    }
}
