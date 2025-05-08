using BetaCinema.Entities;
using BetaCinema.Enum;
using BetaCinema.PayLoads.DataResponses;
using System;
using System.Xml.Linq;

namespace BetaCinema.PayLoads.Convertest
{
    public class BillConverter:BaseConverter
    {
        public DataResponseBill EntityToDTO(Bill bill , IEnumerable<BillFood> billfoods , IEnumerable<BillTicket> billtickets , IEnumerable<BillPromotion> promotions)
        {
            var firstTicket = billtickets.FirstOrDefault()?.Ticket;
            var schedule = firstTicket?.Schedule;

            return new DataResponseBill
            {
                TotalMoney = bill.TotalMoney??0,
                TradingCode = bill.TradingCode,
                CreateTime = bill.CreateTime,
                UpdateTime = bill.UpdateTime,
                StatusActive = bill.IsActive ? "Còn hiệu lực" : "Hết hiệu lực",
                BillStatus = bill.BillStatus?.StatusName ?? "No status",
                UserName = bill.User?.UserName ?? "No name",



                MovieName = schedule?.Movie?.Name ?? "Unknown",
                CinemaName = schedule?.Room?.Cinema?.Name ?? "Unknown",
                RoomName = schedule?.Room?.Name ?? "Unknown",
                StartAt = schedule?.StartAt ?? DateTime.Now,
                EndAt = schedule?.EndAt ?? DateTime.Now,

                BillTickets = billtickets.Select(x=>new DataResponseBillTicket()
                {
                    Price = x.Ticket.PriceTicket,
                    SeatNumber = x.Ticket.Seat.Number
                }),
                BillFoods = billfoods.Select(x=>new DataResponseBillFood()
                {
                    NameFood = x.Food.Name,
                    Quantity = x.Quantity,
                    Price = x.Food.Price

                }),
                Promotions = promotions.Select(x=>new DataResponsePromotion()
                {
                    Name = x.Promotion.Name,
                    Description = x.Promotion.Description,
                    Type = PromotionType.GetName(typeof(PromotionType), x.Promotion.Type) ?? "Unknown"
                })
            };
        }
    }
}
