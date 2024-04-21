using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;

namespace BetaCinema.Payloads.Convertes
{
    public class BillConverter
    {
        private readonly AppDbContext _context;

        public BillConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseBill EntityToDTO(Schedule sc,Bill bill,List<BillFood> bfs,List<BillTicket> bts)
        {
            var roomCr = _context.Rooms.FirstOrDefault(x=>x.Id == sc.RoomId);
            var cinemaCr = _context.Cinemas.FirstOrDefault(x => x.Id == roomCr.CinemaId);
            var promotionCr = _context.Promotions.FirstOrDefault(x=>x.Id == bill.PromotionId);
            return new DataResponseBill
            {
                MovieName = _context.Movies.FirstOrDefault(x=>x.Id == sc.MovieId).Name,
                CinemaName = cinemaCr.NameOfCinema,
                CinemaAddress = cinemaCr.Address,
                OrderedFoods = bfs.Select(x=>new DataResponseBillFood
                {
                    FoodName = _context.Foods.FirstOrDefault(f=>f.Id==x.FoodId).NameOfFood,
                    Quantity = x.Quantity,
                    Price = _context.Foods.FirstOrDefault(f=>f.Id == x.FoodId).Price
                }),
                OrderedTickets = bts.Select(x=>new DataResponseBillTicket
                {
                    TicketName = _context.Tickets.FirstOrDefault(tk=>tk.Id == x.TicketId).Code,
                    Quantity = x.Quantity,
                    Price = _context.Tickets.FirstOrDefault(tk=>tk.Id == x.TicketId).PriceTicket
                }),
                TotalAmountBeforeDiscount = bill.TotalMoney,
                TotalAmountAfterDiscount = bill.TotalMoney - bill.TotalMoney*(promotionCr.Percent/100)
            };
        }
    }
}
