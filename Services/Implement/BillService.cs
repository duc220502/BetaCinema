using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace BetaCinema.Services.Implement
{
    public class BillService : BaseService, IBillService
    {
        private readonly ResponseObject<DataResponseBill> _responseObject;
        private readonly BillConverter _billConverter;

        public BillService()
        {
            _billConverter = new BillConverter();
            _responseObject = new ResponseObject<DataResponseBill>();
        }

        public async Task<ResponseObject<DataResponseBill>> AddBill(int userId, Request_AddBill rq)
        {
            

            var ticketIds =  rq.BillTickets?.Select(x => x.TicketId).ToList()??new List<int>() ;

            var lockedTickets = await _context.Tickets
                .Where(x => ticketIds.Contains(x.Id) && x.IsActive && x.LockByUserId == userId)
                .ToListAsync();

            var result = await ExecuteWithTicketLockHandling(async () =>
            {


                if (rq == null || InputHelper.checkNull(new[] { userId.ToString() }))
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

                if (!(rq.BillFoods?.Any() == true || rq.BillTickets?.Any() == true))
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Không có lựa chọn gì để thanh toán", null);

                var userCr = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (userCr == null)
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "User không tồn tại", null);

                bool hasDuplicateTicketIds = ticketIds.Distinct().Count() != ticketIds.Count();
                if (hasDuplicateTicketIds)
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Có vé bị chọn trùng lặp trong hóa đơn", null);

                bool checkTicket = await _context.BillTickets.AnyAsync(x => ticketIds.Contains(x.TicketId) && x.Bill.BillStatusId == 1);

                if (checkTicket)
                {
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vé hiện tại đã được mua hoặc đang chờ thanh toán", null);
                }

                var bill = new Bill
                {
                    UserId = userId,
                    TradingCode = $"BILL{Guid.NewGuid():N}".Substring(0, 8),
                    CreateTime = DateTime.Now,
                    IsActive = true,
                    BillStatusId = 1,
                };

                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();

                var billFoods = new List<BillFood>();
                var billTickets = new List<BillTicket>();
                var billPromotions = new List<BillPromotion>();
                double total = 0;

                foreach (var billTicketrq in rq.BillTickets)
                {
                    var ticketCr = await _context.Tickets.Include(x => x.Seat)
                                           .Include(x => x.Schedule)
                                               .ThenInclude(s => s.Movie)
                                           .Include(x => x.Schedule)
                                               .ThenInclude(s => s.Room)
                                               .ThenInclude(r => r.Cinema)
                                           .FirstOrDefaultAsync(x => x.Id == billTicketrq.TicketId && x.IsActive && x.LockByUserId == userId);

                    if (ticketCr == null)
                        return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Ticket không tồn tại hoặc hiện tại bị tạm khóa", null);

                    var billTicket = new BillTicket { TicketId = billTicketrq.TicketId, BillId = bill.Id, Quantity = 1, Ticket = ticketCr };
                    billTickets.Add(billTicket);
                    total += ticketCr.PriceTicket;
                }

                foreach (var billFoodrq in rq.BillFoods)
                {
                    var foodCr = await _context.Foods.FirstOrDefaultAsync(x => x.Id == billFoodrq.FoodId && x.IsActive);
                    if (foodCr == null)
                        return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Food không tồn tại", null);

                    billFoods.Add(new BillFood
                    {
                        Quantity = billFoodrq.Quantity,
                        BillId = bill.Id,
                        FoodId = billFoodrq.FoodId,
                        Food = foodCr
                    });

                    total += foodCr.Price * billFoodrq.Quantity;
                }

                foreach (var billPromotionrq in rq.BillPromotions)
                {
                    var promotionCr = await _context.Promotions.FirstOrDefaultAsync(x => x.Id == billPromotionrq.PromotionId && x.IsActive);
                    if (promotionCr == null)
                        return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Promotion không tồn tại", null);

                    billPromotions.Add(new BillPromotion { PromotionId = billPromotionrq.PromotionId, BillId = bill.Id, Promotion = promotionCr });
                }

                bill.TotalMoney = total;
                _context.Bills.Update(bill);
                _context.BillFoods.AddRange(billFoods);
                _context.BillTickets.AddRange(billTickets);
                _context.BillPromotions.AddRange(billPromotions);

                await _context.SaveChangesAsync();

                bill = await _context.Bills.Include(b => b.BillStatus)
                                     .FirstOrDefaultAsync(b => b.Id == bill.Id);

                var ticketsToUpdate = await _context.Tickets
                                    .Where(x => ticketIds.Contains(x.Id))
                                    .ToListAsync();

                foreach (var ticket in ticketsToUpdate)
                {
                    ticket.IsActive = false;
                }

                _context.Tickets.UpdateRange(ticketsToUpdate);
                await _context.SaveChangesAsync();

                return _responseObject.ResponseSuccess("Thêm hóa đơn thành công", _billConverter.EntityToDTO(bill, billFoods, billTickets, billPromotions));
            }, lockedTickets, "Thêm hóa đơn thất bại");

            return result;

        }

        public async Task<ResponseObject<T>> ExecuteWithTicketLockHandling<T>(Func<Task<ResponseObject<T>>> action, List<Ticket> lockedTickets,string errorMessage)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = await action();

                if (result != null && result.status == StatusCodes.Status200OK)
                {
                   await transaction.CommitAsync();
                    return result;
                }


                await transaction.RollbackAsync();
                await UnlockTickets(lockedTickets);

                return result;
            }
            catch (Exception)
            {

                await transaction.RollbackAsync();
                await  UnlockTickets(lockedTickets);
                
                return new ResponseObject<T>().ResponseError(StatusCodes.Status500InternalServerError, errorMessage,default);
            }
            
        }

        private async Task UnlockTickets(List<Ticket> tickets)
        {
            if (tickets != null && tickets.Any())
            {
                foreach (var ticket in tickets)
                {
                    ticket.IsLocked = false;
                    ticket.LockedTime = null;
                    ticket.LockByUserId = null;
                    _context.Tickets.Update(ticket);
                }
              await  _context.SaveChangesAsync();
            }
            
        }
    }
}
