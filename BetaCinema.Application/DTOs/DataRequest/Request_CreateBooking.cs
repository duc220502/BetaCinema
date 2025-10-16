using BetaCinema.Application.DTOs.DataRequest.Foods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest
{
    public class Request_CreateBooking
    {
        public Guid ScheduleId { get; set; }
        public List<Guid>? SeatIds { get; set; }
        public List<Request_FoodItem>? FoodItems { get; set; } 
        public List<Guid>? PromotionIds { get; set; } 
    }
}
