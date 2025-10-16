using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class BillService(IBillStatusRepository billStatusRepository , 
        IMapper mapper , IPromotionService promotionService , 
        IBillRepository billRepository , IConfiguration configuration) : IBillService
    {
        private readonly IBillStatusRepository _billStatusRepository = billStatusRepository;
        private readonly IBillRepository _billRepository = billRepository;
        private readonly IPromotionService _promotionService = promotionService;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;


        

        public async Task<Bill> CreateBillFromItemsAsync(Guid userId, List<PreparedFoodDto> foods, List<PreparedTicketDto> tickets, List<Guid>? promotionIds)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("UserId không được rỗng.", nameof(userId));

            if (foods == null)
                throw new ArgumentNullException(nameof(foods));

            if (tickets == null)
                throw new ArgumentNullException(nameof(tickets));

            if (!tickets.Any() && !foods.Any())
                throw new InvalidOperationException("Không thể tạo hóa đơn rỗng mà không có vé hoặc đồ ăn.");

            var tradingCode = Guid.NewGuid().ToString().ToUpper().Substring(0, 8);

            var defaultHoldMinutes = _configuration.GetValue<int>("BookingSettings:DefaultHoldMinutes");

            var bill = new Bill
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TradingCode = tradingCode,
                CreateTime = DateTime.UtcNow,
                BillStatusId = (int)Domain.Enums.BillStatus.PendingPayment,
                ExpireAt = DateTime.UtcNow.AddMinutes(defaultHoldMinutes)
            };


            if (tickets.Any())
            {
                bill.BillTickets = tickets.Select(dto => new BillTicket
                {
                    Bill = bill,
                    Ticket = _mapper.Map<Ticket>(dto)
                }).ToList();
            }

            if (foods.Any())
            {
                bill.BillFoods = foods.Select(dto => new BillFood
                {
                    Bill = bill,
                    FoodId = dto.FoodId,
                    Quantity = dto.Quantity
                }).ToList();
            }
            decimal ticketsTotal = tickets.Sum(t => t.PriceTicket);
            decimal foodsTotal = foods.Sum(f => f.Price * f.Quantity);

            bill.SubTotal = ticketsTotal + foodsTotal;

            var billContext = new BillContext() {UserId = userId,SubTotal = bill.SubTotal??0,Tickets = tickets,Foods = foods};

            var promotionResult = await _promotionService.ApplyPromotionsAsync(billContext, promotionIds);
            bill.DiscountAmount = promotionResult.TotalDiscountAmount;
            bill.TotalMoney = promotionResult.FinalTotal;

            if (promotionResult.AppliedPromotions.Any())
            {
                foreach (var appliedPromo in promotionResult.AppliedPromotions)
                {
                    bill.BillPromotions.Add(new BillPromotion
                    {
                        Bill = bill,
                        PromotionId = appliedPromo.PromotionId,
                    });
                }
            }


            return bill;

        }



        public async Task<ResponseObject<DataResponseBill>> GetBillById(Guid id)
        {
            var result = await _billRepository.GetBillDetailsForResponseAsync(id) ?? throw new NotFoundException("Không tìm thấy Bill");

            var dto = _mapper.Map<DataResponseBill>(result);

            return ResponseObject<DataResponseBill>.ResponseSuccess("Lấy thông tin Bill thành công", dto);
        }
    }
}
