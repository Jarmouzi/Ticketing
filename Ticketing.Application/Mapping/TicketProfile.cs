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
