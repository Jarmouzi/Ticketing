using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ticketing.DTO;
using Ticketing.Model;

namespace Ticketing.Repository
{
    public interface ITicketRepository
    {

        Task<Ticket> AddAsync(Ticket model);
        Task<Ticket> UpdateAsync(Ticket model);
        Task<Guid?> DeleteAsync(Guid id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<IEnumerable<Ticket>> GetAllAsync(Expression<Func<Ticket, bool>> filter);
        Task<List<TicketStatusDto>> GetCountByStatusAsync();
        Task<Ticket?> GetByIdAsync(Guid id);
    }
}
