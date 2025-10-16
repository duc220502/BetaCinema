using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;
using System.Net.Sockets;

namespace BetaCinema.Domain.Entities.Seats
{
    public class Seat:BaseEntity
    {
        public int Number { get; set; }
        public LineSeat Line { get; set; }
        public bool  IsActive { get; set; }

        public Guid RoomId { get; set; }
        public virtual Room? Room { get; set; }

        public int SeatStatusId { get; set; }
        public virtual SeatStatus? SeatStatus { get; set; }

        public int SeatTypeId { get; set; }
        public virtual SeatType? SeatType { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
