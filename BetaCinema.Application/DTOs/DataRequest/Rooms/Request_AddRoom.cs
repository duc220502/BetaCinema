using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Rooms
{
    public class Request_AddRoom
    {
        public string Name { get; set; } = default!;
        public int? Capacity { get; set; }

        public RoomType Type { get; set; }
        public string? Description { get; set; }

        public Guid CinemaId { get; set; }
    }
}
