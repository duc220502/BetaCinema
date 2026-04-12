using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Cart
{
    public class CartResponseDto
    {
        public int Count { get; set; }
        public decimal Subtotal { get; set; }
        public List<CartItemDto> Items { get; set; } = [];
    }
}
