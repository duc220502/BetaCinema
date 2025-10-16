using AutoMapper;
using BetaCinema.Application.DTOs;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Mappings
{
    public class TicketProfile : Profile
    {
        public TicketProfile() 
        {
            CreateMap<Ticket, PreparedTicketDto>();

            CreateMap<PreparedTicketDto, Ticket>();
        }
    }
}
