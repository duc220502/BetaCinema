using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IBookingService
    {
        Task<ResponseObject<DataResponseBill>> CreateBookingAsync(Request_CreateBooking request);

        Task CancelPendingBookingAsync(Guid billId);
    }
}
