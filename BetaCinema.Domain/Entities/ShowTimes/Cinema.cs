using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.ShowTimes
{
    public class Cinema : BaseEntity
    {
        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string?  Description { get; set; }

        public string Code { get; set; } = default!;
        public bool IsActive { get; set; }

        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
