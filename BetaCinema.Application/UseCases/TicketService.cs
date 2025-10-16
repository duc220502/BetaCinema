using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces.Repositorys;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{

    public class TicketService(IPriceService priceService , ITicketRepository ticketRepository,IMapper mapper , ISeatRepository seatRepository) : ITicketService
    {
        private readonly IPriceService _priceService = priceService;
        private readonly ISeatRepository _seatRepository = seatRepository;
        private readonly ITicketRepository _ticketRepository = ticketRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<ResponseObject<object>> InvalidateTicketsForBillAsync(Guid billId)
        {
            var ticketsToUpdate = await _ticketRepository.GetTicketsByBillIdAsync(billId);
            ticketsToUpdate.ForEach(t => t.IsActive = false);

            return ResponseObject<object>.ResponseSuccess(" InvalidateTicketsForBill success", null);
        }

        public async Task<TicketPreparationResult> PrepareTicketsForBookingAsync(Schedule schedule, List<Guid>? seatIds)
        {
            if (seatIds == null || !seatIds.Any())
                return new TicketPreparationResult() { TicketDtos = new() , ValidatedSeats = new()};

            var availableSeats = await _seatRepository.GetAvailableSeatsAsync(seatIds, schedule.RoomId);

            if (availableSeats.Count != seatIds?.Count)
                throw new InvalidOperationException("Một hoặc nhiều ghế bạn chọn đã có người khác đặt hoặc không hợp lệ. Vui lòng chọn lại.");

            var basePrice = await _priceService.CalculateBaseTicketPriceAsync(schedule);

            var tickets = new List<Ticket>();

            foreach (var seat in availableSeats)
            {
                if (seat.SeatType == null)
                {
                    throw new NotFoundException($"SeatType is not loaded for SeatId {seat.Id}");
                }

                var finalPrice = basePrice + seat.SeatType.Surcharge;

                tickets.Add(new Ticket
                {
                    Id = Guid.NewGuid(),
                    ScheduleId = schedule.Id,
                    SeatId = seat.Id,
                    PriceTicket = finalPrice,
                    Code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                    IsActive = true
                });
            }
            var ticketDtos = tickets.Select(x=>_mapper.Map<PreparedTicketDto>(x)).ToList();
            return new TicketPreparationResult() { TicketDtos = ticketDtos , ValidatedSeats = availableSeats };

        }
    }
}
