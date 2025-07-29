using AutoMapper;
using System.Net.NetworkInformation;
using Ticketing.DTO;
using Ticketing.Model;

namespace Ticketing.AutoMapper
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<TicketDto, Ticket>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()));


            CreateMap<Ticket, TicketDto>();
        }
    }
}
