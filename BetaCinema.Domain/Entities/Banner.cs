using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities
{
    public class Banner : BaseEntity
    {
        public string ImgUrl { get; set; } = default!;
        public string Title { get; set; } = default!;
    }
}
