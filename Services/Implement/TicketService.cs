using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BetaCinema.Services.Implement
{
    public class TicketService : BaseService, ITicketService
    {
        private readonly ResponseObject<DataResponseTicket> _responseObject;
        private readonly TicketConverter _ticketConverter;

        public TicketService()
        {
            _ticketConverter = new TicketConverter();
            _responseObject = new ResponseObject<DataResponseTicket>();
        }

        public async Task<ResponseObject<DataResponseTicket>> AddTicket(Request_AddTicket rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] { rq.PriceTicket.ToString(), rq.ScheduleId.ToString() , rq.SeatId.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            if(rq.PriceTicket<=0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Gía không hợp lệ", null);


            var scheduleCr = await _context.Schedules.FirstOrDefaultAsync(x => x.Id == rq.ScheduleId && x.IsActive );

            if(scheduleCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "ScheduleSeat không tồn tại", null);

            var seatCr  = await _context.Seats.FirstOrDefaultAsync(x=>x.Id == rq.SeatId && x.IsActive );
            if (seatCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Seat không tồn tại", null);

            if(scheduleCr.RoomId !=  seatCr.RoomId)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Schedule và Seat không khớp nhau", null);

            var isDuplicateId = await _context.Tickets.AnyAsync(x=>x.ScheduleId == rq.ScheduleId && x.SeatId == rq.SeatId);
            if(isDuplicateId)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "ScheduleSeat đã có vé", null);

            var newTicket = new Ticket()
            {
                Code = $"T{Guid.NewGuid():N}".Substring(0, 8),
                PriceTicket = rq.PriceTicket,
                IsActive = true,
                IsLocked = false,
                ScheduleId = rq.ScheduleId,
                SeatId = rq.SeatId

            };

            _context.Tickets.Add(newTicket);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Thêm ticket thành công",_ticketConverter.EntityToDTO(newTicket));
        }

        public async Task<ResponseObject<DataResponseTicket>> DeleteTicket(int ticketId)
        {

            if (InputHelper.checkNull(new string[] { ticketId.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var ticketCr = await _context.Tickets.FirstOrDefaultAsync(x=>x.Id == ticketId && x.IsActive);

            if (ticketCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Ticket không tồn tại", null);

            ticketCr.IsActive = false;

            _context.Tickets.Update(ticketCr);
             await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Xóa ticket thành công",_ticketConverter.EntityToDTO(ticketCr));
        }

        public async Task<ResponseObject<DataResponseTicket>> LockedTicket(int userId, Request_LockedTicket rq)
        {
            if(rq == null || InputHelper.checkNull(new string[] { userId.ToString() , rq.TicketId.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var checkUser = await _context.Users.AnyAsync(x=>x.Id == userId && x.IsActive);
            
            if(!checkUser)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "User không tồn tại", null);

            var ticketCr = await _context.Tickets.FirstOrDefaultAsync(x=>x.Id == rq.TicketId && x.IsActive && x.IsLocked == false);

            if(ticketCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Ticket không tồn tại hoặc đang tạm khóa", null);

            ticketCr.IsLocked = true;
            ticketCr.LockByUserId = userId;
            ticketCr.LockedTime = DateTime.Now;

            _context.Tickets.Update(ticketCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Lock vé thành thông",_ticketConverter.EntityToDTO(ticketCr));
        }

        public async Task<ResponseObject<DataResponseTicket>> UpdateTicket(Request_UpdateTicket rq)
        {

            if (rq == null || InputHelper.checkNull(new string[] { rq.Id.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var ticketCr = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == rq.Id && x.IsActive);

            if(ticketCr==null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Ticket không tồn tại", null);

            if(rq.PriceTicket != null && rq.PriceTicket<=0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Gía không hợp lệ", null);

            ticketCr.PriceTicket = rq.PriceTicket??ticketCr.PriceTicket;

            _context.Tickets.Update(ticketCr);
            await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Cập nhật ticket thành công", _ticketConverter.EntityToDTO(ticketCr));

        }
    }
}
