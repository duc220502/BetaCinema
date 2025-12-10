using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseMovie
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Trailer { get; set; }
        public int MovieDuration { get; set; }
        public DateTime PremiereDate { get; set; }

        public Decimal BasePrice { get; set; }
        public string? Description { get; set; }

        public string? Director { get; set; }

        public string? HeroImage { get; set; }

        public string? Language { get; set; }

        public bool IsActive { get; set; }

        public Guid MovieTypeId { get; set; }
        public int RateId { get; set; }
    }
}
