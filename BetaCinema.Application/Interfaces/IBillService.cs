using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IBillService
    {
        Task<Bill> CreateBillFromItemsAsync(Guid userId,List<PreparedFoodDto> foods , List<PreparedTicketDto> tickets , List<Guid>? promotionIds);

        Task<ResponseObject<DataResponseBill>> GetBillById(Guid id);
    }
}
