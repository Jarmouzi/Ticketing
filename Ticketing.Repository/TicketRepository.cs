using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ticketing.DataContext;
using Ticketing.DTO;
using Ticketing.Model;

namespace Ticketing.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DbSet<Ticket> _service;

        public TicketRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _service = _unitOfWork.Set<Ticket>();
        }
        public async Task<Ticket> AddAsync(Ticket model)
        {
            try
            {
                _service.Add(model);
                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return model;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        /// <summary>
        /// Only updates assignedToUserId and Status
        /// </summary>
        /// <param name="model"></param>
        /// <returns>updated model</returns>
        public async Task<Ticket> UpdateAsync(Ticket model)
        {
            try
            {
                var newModel = _service.Find(model.Id);

                newModel.Status = model.Status;
                newModel.AssignedToUserId = model.AssignedToUserId;
                newModel.UpdatedAt = model.UpdatedAt;

                if (await _unitOfWork.SaveAsync() > 0)
                {
                    return newModel;
                }
                return new Ticket();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Physical Delete
        /// </summary>
        /// <param name="id"> Id of Ticket to delete </param>
        /// <returns></returns>
        public async Task<Guid?> DeleteAsync(Guid id)
        {
            var result = id;
            try
            {
                var item = _service.Find(id);
                if (item != null)
                {
                    _service.Remove(item);
                    if (await _unitOfWork.SaveAsync() > 0)
                    {
                        return result;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            IEnumerable<Ticket> result = new List<Ticket>();
            try
            {
                result = await _service.ToArrayAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public async Task<IEnumerable<Ticket>> GetAllAsync(Expression<Func<Ticket, bool>> filter)
        {
            IEnumerable<Ticket> result = new List<Ticket>();

            try
            {
                result = await _service.Where(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns ticket counts by status</returns>
        public async Task<List<TicketStatusDto>> GetCountByStatusAsync()
        {
            try
            {
                return await _service.GroupBy(m => m.Status).Select(m => new TicketStatusDto { Status = m.Key.ToString(), Count = m.Count() }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Ticket?> GetByIdAsync(Guid id)
        {
            var result = new Ticket();
            try
            {
                result = await _service.FindAsync(id);

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
