using AutoMapper;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class BillProfile : Profile
    {
        public BillProfile() 
        {
            CreateMap<Bill, DataResponseBill>()
            .ForMember(dest => dest.BillId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TradingCode, opt => opt.MapFrom(src => src.TradingCode))
            .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(src => src.CreateTime))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.BillStatus!.StatusName)) 

            
            .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src =>
                src.BillTickets.Select(bt => bt.Ticket!.Schedule!.Movie!.Name).FirstOrDefault()))

            .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src =>
                src.BillTickets.Select(bt => bt.Ticket!.Schedule!.Room!.Name).FirstOrDefault()))

            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
            .ForMember(dest => dest.DiscountAmount, opt => opt.MapFrom(src => src.DiscountAmount))
            .ForMember(dest => dest.FinalTotal, opt => opt.MapFrom(src => src.TotalMoney))

            .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.BillTickets))
            .ForMember(dest => dest.Foods, opt => opt.MapFrom(src => src.BillFoods));

            CreateMap<BillTicket, DataResponseTicketInBill>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Ticket!.Id))
                .ForMember(dest => dest.SeatPosition, opt => opt.MapFrom(src => $"{src.Ticket!.Seat!.Line}{src.Ticket.Seat.Number}"))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Ticket!.PriceTicket));
                
            CreateMap<BillFood, DataResponseFoodInBill>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Food!.Id))
                .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food!.Name))
                
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Food!.Price * src.Quantity))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        }
    }
}
