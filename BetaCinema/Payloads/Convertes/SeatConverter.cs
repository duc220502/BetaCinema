using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BetaCinema.Payloads.Convertes
{
    public class SeatConverter
    {
        private readonly AppDbContext _context;

        public SeatConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseSeat EntityToDTO(Seat seat)
        {
            return new DataResponseSeat
            {
                Number = seat.Number,
                Line = seat.Line,
                ActiveStatus = seat.IsActive ? "Hoạt động":"Không hoạt động",
                SeatStatus = _context.SeatStatuses.FirstOrDefault(x=>x.Id == seat.SeatStatusId).NameStatus,
                RoomName = _context.Rooms.FirstOrDefault(x=>x.Id == seat.RoomId).Name,
                SeatType = _context.SeatTypes.FirstOrDefault(x=>x.Id == seat.SeatTypeId).NameType
            };
        }
        
    }
}
