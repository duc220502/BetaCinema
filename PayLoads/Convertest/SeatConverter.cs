using BetaCinema.Entities;
using BetaCinema.PayLoads.DataResponses;

namespace BetaCinema.PayLoads.Convertest
{
    public class SeatConverter:BaseConverter
    {
        public DataResponseSeat EntityToDTO(Seat seat)
        {

            return new DataResponseSeat
            {
                Number = seat.Number,
                Line = seat.Line,
                StatusActive = seat.IsActive ? "Hoạt động" : "Không hoạt động",
                NameRoom = _context.Rooms.FirstOrDefault(x => x.Id == seat.RoomId)?.Name ?? "Không room",
                SeatStatus = _context.SeatStatuses.FirstOrDefault(x => x.Id == seat.SeatStatusId)?.NameStatus ?? "Không seatstatus",
                SeatType = _context.SeatTypes.FirstOrDefault(x=>x.Id == seat.SeatTypeId)?.NameType??"Không seattype"
            };
        }
    }
}
