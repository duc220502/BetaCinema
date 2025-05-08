using BetaCinema.Entities;
using BetaCinema.PayLoads.DataResponses;
using System.Xml.Linq;

namespace BetaCinema.PayLoads.Convertest
{
    public class RoomConverter:BaseConverter
    {
        public DataResponseRoom EntityToDTO(Room room)
        {
            
            return new DataResponseRoom
            {
                Name = room.Name,
                Capacity = room.Capacity,
                TypeRoom = room.RoomType,
                Description = room.Description,
                StatusActive = room.IsActive?"Hoạt động":"Không hoạt động"
            };
        }
    }
}
