using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface ITicketService
    {
        Task<TicketPreparationResult> PrepareTicketsForBookingAsync(Schedule schedule, List<Guid>? seatIds);

        Task<ResponseObject<object>> InvalidateTicketsForBillAsync(Guid billId);
    }
}
