using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Repository;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Domain.ValueObjects;
using AutoMapper;

namespace Ticketing.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(ITicketRepository ticketRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TicketDto> CreateTicketAsync(TicketCreateDto dto, Guid userId)
        {
            try
            {
                var model = _mapper.Map<Ticket>(dto);
                var result = await _ticketRepository.AddAsync(model);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<TicketDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public async Task<TicketDto?> UpdateStatusAsync(Guid ticketId, TicketUpdateDto dto)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null)
                return null;

            ticket.Status = dto.Status;
            ticket.AssignedToUserId = dto.AssignedToUserId;

            await _unitOfWork.SaveAsync();

            return _mapper.Map<TicketDto>(ticket);
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
                var item = await _ticketRepository.DeleteAsync(id);
                if (item != null)
                {
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
        public async Task<Ticket?> GetByIdAsync(Guid id)
        {
            return await _ticketRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return tickets.Select(t => new TicketDto(t.Id, t.Title, t.Status.ToString(), t.CreatedAt));
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsByUserAsync(Guid userId)
        {
            var tickets = await _ticketRepository.GetAllAsync(m => m.CreatedByUserId == userId);
            return tickets.Select(t => new TicketDto(t.Id, t.Title, t.Status.ToString(), t.CreatedAt));
        }

        public async Task<List<TicketStatusDto>> GetCountByStatusAsync()
        {
            var rawCounts = await _ticketRepository.GetCountByStatusAsync();

            var result = rawCounts
                .Select(kvp => new TicketStatusDto
                {
                    Status = kvp.Key.ToString(),
                    Count = kvp.Value
                })
                .ToList();

            return result;
        }

        Task<TicketDto?> ITicketService.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
