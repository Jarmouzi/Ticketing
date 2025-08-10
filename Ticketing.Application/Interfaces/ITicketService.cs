using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Core.Results;

namespace Ticketing.Application.Interfaces
{
    public interface ITicketService
    {
        Task<ServiceResult<TicketDto>> CreateAsync(TicketCreateDto dto, ClaimsPrincipal user);
        Task<ServiceResult<TicketDto>> UpdateAsync(Guid id, TicketUpdateDto dto, ClaimsPrincipal user);
        Task<ServiceResult<TicketDto>> GetByIdAsync(Guid id, ClaimsPrincipal user);
        Task<ServiceResult<Guid>> DeleteAsync(Guid id, ClaimsPrincipal user);
        Task<ServiceResult<IEnumerable<TicketDto>>> GetAllTicketsAsync(ClaimsPrincipal user);
        Task<ServiceResult<IEnumerable<TicketDto>>> GetTicketsByUserAsync(ClaimsPrincipal user);
        Task<ServiceResult<IEnumerable<TicketStatusDto>>> GetCountByStatusAsync(ClaimsPrincipal user);
    }
}
