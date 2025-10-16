using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseAvailableSlotDto
    {
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
    }
}
