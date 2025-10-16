using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Rooms
{
    public class Request_UpdateRoom
    {
        public string? Name { get; set; }
        public int? Capacity { get; set; }

        public RoomType? RoomType { get; set; }
        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
