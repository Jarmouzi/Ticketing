using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Domain.Entities;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Application.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(TicketCreateDto dto, Guid userId);
        Task<TicketDto> UpdateStatusAsync(Guid ticketId, TicketUpdateDto dto);
        Task<Guid?> DeleteAsync(Guid id);
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync();
        Task<IEnumerable<TicketDto>> GetTicketsByUserAsync(Guid userId);
        Task<List<TicketStatusDto>> GetCountByStatusAsync();

        Task<TicketDto?> GetByIdAsync(Guid id);
    }
}
