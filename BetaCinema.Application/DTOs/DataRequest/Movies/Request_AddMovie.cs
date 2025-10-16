using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Movies
{
    public class Request_AddMovie
    {
        public string? Name { get; set; }
        public string? Trailer { get; set; }
        public int MovieDuration { get; set; }
        public DateTime PremiereDate { get; set; }

        public decimal BasePrice { get; set; }
        public string? Description { get; set; }

        public string? Director { get; set; }

        public string? HeroImage { get; set; }

        public string? Language { get; set; }
        public Guid MovieTypeId { get; set; }

        public int RateId { get; set; }
    }
}
