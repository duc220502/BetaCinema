using BetaCinema.Entities;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Xml.Linq;

namespace BetaCinema.Services.Implements
{
    public class BillService : BaseService, IBillService
    {
        private readonly ResponseObject<DataResponseBill> _responseObject;
        private readonly BillConverter _converter;

        public BillService()
        {
            _converter = new BillConverter();
            _responseObject = new ResponseObject<DataResponseBill>();
        }

        public ResponseObject<DataResponseBill> CreateBill(int id,Request_AddBill rq)
        {
            try
            {
                string code;
                int idBill;
                var billCode = _context.Bills.OrderByDescending(x => id).FirstOrDefault();
                if (billCode == null)
                {
                    idBill = 1;
                    code = "B1";
                }
                else
                {
                    idBill = billCode.Id + 1;
                    code = "B" + (billCode.Id + 1);
                }
                List<BillFood> billFoods = new List<BillFood>();
                List<BillTicket> billTickets = new List<BillTicket>();
                double total = 0;
                var scheduleCr = _context.Schedules.FirstOrDefault(r => r.Id == rq.ScheduleId && r.IsActive == true);
                if (scheduleCr == null)
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Lịch chiếu không tồn tại", null);

                if (rq.BillTickets == null && rq.BillFoods == null)
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Không có lựa chọn gì để thanh toán", null);

                foreach (var billF in rq.BillFoods)
                {
                    var foodCr = _context.Foods.FirstOrDefault(x => x.Id == billF.FoodId && x.IsActive == true);
                    if (foodCr == null)
                        return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Food không tồn tại", null);

                    BillFood billFood = new BillFood()
                    {
                        Quantity = billF.Quantity,
                        BillId = idBill,
                        FoodId = billF.FoodId,
                    };
                    billFoods.Add(billFood);
                    total += foodCr.Price * billF.Quantity;
                }

                foreach (var billTicket in rq.BillTickets)
                {
                    var ticketCr = _context.Tickets.FirstOrDefault(x => x.Id == billTicket.TicketId && x.IsActive == true);
                    if (ticketCr == null)
                        return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Ticket không tồn tại", null);
                    BillTicket billTk = new BillTicket()
                    {
                        Quantity = billTicket.Quantity,
                        BillId = idBill,
                        TicketId = billTicket.TicketId,
                    };
                    billTickets.Add(billTk);
                    total += ticketCr.PriceTicket * billTicket.Quantity;
                }
                var promotion = _context.Promotions.FirstOrDefault(x => x.Id == rq.PromotionId && x.IsActive == true);

                if (promotion == null)
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Promotion không tồn tại", null);

                Bill bill = new Bill()
                {
                    TotalMoney = total,
                    TradingCode = code,
                    CreateTime = DateTime.Now,
                    Name = "HD",
                    UpdateTime = null,
                    IsActive = true,
                    CustomerId = id,
                    PromotionId = promotion.Id,
                    BillStatusId = 1
                };

                _context.Bills.Add(bill);
                _context.SaveChanges();

                _context.BillFoods.AddRange(billFoods);
                _context.BillTickets.AddRange(billTickets);
                _context.SaveChanges();

                return _responseObject.ResponseSuccess("Lập hóa đơn thành công",_converter.EntityToDTO(scheduleCr,bill,billFoods,billTickets));

            }
            catch (Exception ex)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, ex.Message, null);
            }


        }
    }
}
