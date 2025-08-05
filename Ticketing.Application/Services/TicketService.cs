using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Repository;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Domain.ValueObjects;
using AutoMapper;
using System.Net.Sockets;

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
                model.CreatedByUserId = userId;
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
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(ticketId);
                if (ticket == null)
                    return null;

                ticket.Status = dto.Status;
                ticket.AssignedToUserId = dto.AssignedToUserId;

                await _unitOfWork.SaveAsync();

                return _mapper.Map<TicketDto>(ticket);
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
        public async Task<TicketDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(id);
                return _mapper.Map<TicketDto>(ticket) ?? null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsAsync()
        {
            try
            {
                var tickets = await _ticketRepository.GetAllAsync();
                return _mapper.Map<List<TicketDto>>(tickets);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TicketDto>> GetTicketsByUserAsync(Guid userId)
        {
            try
            {
                var tickets = await _ticketRepository.GetAllAsync(m => m.CreatedByUserId == userId);
                return _mapper.Map<List<TicketDto>>(tickets);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<TicketStatusDto>> GetCountByStatusAsync()
        {
            try
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
            catch (Exception)
            {
                throw;
            }
        }
    }
}
