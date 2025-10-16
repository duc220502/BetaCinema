using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseUserPromotionPersonal
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public DataResponsePromotion Promotion { get; set; } = default!;
        public Guid UserId { get; set; }
    }
}
