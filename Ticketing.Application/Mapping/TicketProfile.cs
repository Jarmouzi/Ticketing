using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Ticketing.Application.DTOs;
using Ticketing.Domain.Entities;
using AutoMapper;

namespace Ticketing.Application.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
            CreateMap<TicketCreateDto, Ticket>();
        }
    }
}
