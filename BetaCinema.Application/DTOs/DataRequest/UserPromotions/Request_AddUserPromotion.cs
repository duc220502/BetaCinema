using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.UserPromotions
{
    public class Request_AddUserPromotion
    {
        public int? Quantity { get; set; }

        public Guid PromotionId { get; set; }

        public Guid UserId { get; set; }
    }
}
