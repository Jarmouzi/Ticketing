using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Repository;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Domain.ValueObjects;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Ticketing.Common.Results;
using Ticketing.Application.Services.Auth;

namespace Ticketing.Application.Services
{
    public class TicketService : BaseService, ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketService(
            ITicketRepository ticketRepository,
            IAuthRepository authRepository,
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<TicketService> logger) : base(logger)
        {
            _ticketRepository = ticketRepository;
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<TicketDto>> CreateAsync(TicketCreateDto dto, ClaimsPrincipal user)
        {
            try
            {
                if(user.UserId() == null)
                    return ServiceResult<TicketDto>.Unauthorized("you must login first!");

                if(!user.IsEmployee())
                    return ServiceResult<TicketDto>.Forbidden("Only employees can create tickets!");

                var model = _mapper.Map<Ticket>(dto);
                model.CreatedByUserId = user.UserId().Value;
                var result = await _ticketRepository.AddAsync(model);

                await _unitOfWork.SaveAsync();
                
                var ticket =_mapper.Map<TicketDto>(result);

                return ServiceResult<TicketDto>.Success(ticket); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error acurred on TicketService_CreateAsync: {ex.Message} {ex.InnerException?.Message ?? ""}", new object[] { dto, user });

                return ServiceResult<TicketDto>.Error("Something went wrong, Please contact your Admin or try again later.");
            }
        }

        public async Task<ServiceResult<TicketDto>> UpdateAsync(Guid id, TicketUpdateDto dto, ClaimsPrincipal user)
        {
            try
            {
                if (user.UserId() == null)
                    return ServiceResult<TicketDto>.Unauthorized("you must login first!");

                if (!user.IsAdmin())
                    return ServiceResult<TicketDto>.Forbidden("Only Admins can update tickets!");


                var assignedUser = await _authRepository.GetUserAsync(dto.AssignedToUserId);

                if (assignedUser == null)
                    return ServiceResult<TicketDto>.NotFound("Assigned User is not found!");

                if (assignedUser.Role != UserRole.Admin)
                    return ServiceResult<TicketDto>.Unauthorized("Assigned User is not admin! Tickets should only assigned to admins." );

                var ticket = await _ticketRepository.GetByIdAsync(id);
                if (ticket == null)
                    return ServiceResult<TicketDto>.NotFound("Ticket is not found!");

                ticket.Status = dto.Status;
                ticket.AssignedToUserId = dto.AssignedToUserId;

                await _unitOfWork.SaveAsync();

                var result =_mapper.Map<TicketDto>(ticket);

                return ServiceResult<TicketDto>.Success(result); 
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error acurred on TicketService_UpdateAsync: {ex.Message} {ex.InnerException?.Message ?? ""}", new object[] { id, user });

                return ServiceResult<TicketDto>.Error("Something went wrong, Please contact your Admin or try again later.");
            }
        }
        /// <summary>
        /// physical delete
        /// </summary>
        /// <param name="id">Ticket Id</param>
        /// <param name="user">Current user</param>
        /// <returns></returns>
        public async Task<ServiceResult<Guid>> DeleteAsync(Guid id, ClaimsPrincipal user)
        {
            var result = id;
            try
            {
                if (user.UserId() == null)
                    return ServiceResult<Guid>.Unauthorized("you must login first!");

                if (!user.IsAdmin())
                    return ServiceResult<Guid>.Forbidden("Only Admins can delete tickets!");


                var item = await _ticketRepository.DeleteAsync(id);
                if (item != null)
                {
                    if (await _unitOfWork.SaveAsync() > 0)
                    {
                        return ServiceResult<Guid>.Success(result);
                    }
                }
                return ServiceResult<Guid>.NotFound("Ticket is not found!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error acurred on TicketService_DeleteAsync: {ex.Message} {ex.InnerException?.Message ?? ""}", new object[] { id, user });

                return ServiceResult<Guid>.Error("Something went wrong, Please contact your Admin or try again later.");
            }
        }

        /// <summary>
        /// Physical Delete
        /// </summary>
        /// <param name="id"> Id of Ticket to delete </param>
        /// <returns></returns>
        public async Task<ServiceResult<TicketDto>> GetByIdAsync(Guid id, ClaimsPrincipal user)
        {
            try
            {
                var ticket = await _ticketRepository.GetByIdAsync(id);
                if (ticket == null)
                    return ServiceResult<TicketDto>.NotFound("Ticket is not found!");

                if (!user.IsTicketAssigned(ticket))
                    return ServiceResult<TicketDto>.Forbidden("Access denied! Only the ticket creator or assigned admin can access the ticket");

                var dto = _mapper.Map<TicketDto>(ticket);

                return ServiceResult<TicketDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error acurred on TicketService_GetByIdAsync: {ex.Message} {ex.InnerException?.Message ?? ""}", new object[] { id, user });

                return ServiceResult<TicketDto>.Error("Something went wrong, Please contact your Admin or try again later.");
            }
        }

        public async Task<ServiceResult<IEnumerable<TicketDto>>> GetAllTicketsAsync(ClaimsPrincipal user)
        {
            try
            {
                if (user.UserId() == null)
                    return ServiceResult<IEnumerable<TicketDto>>.Unauthorized("you must login first!");

                if (!user.IsAdmin())
                    return ServiceResult<IEnumerable<TicketDto>>.Forbidden("Only admins can get all tickets!");

                var tickets = await _ticketRepository.GetAllAsync();
                return ServiceResult<IEnumerable<TicketDto>>.Success(_mapper.Map<List<TicketDto>>(tickets));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error acurred on TicketService_GetAllTicketsAsync: {ex.Message} {ex.InnerException?.Message ?? ""}", new object[] {  user });

                return ServiceResult<IEnumerable<TicketDto>>.Error("Something went wrong, Please contact your Admin or try again later.");
            }
        }

        public async Task<ServiceResult<IEnumerable<TicketDto>>> GetTicketsByUserAsync(ClaimsPrincipal user)
        {
            try
            {
                if (user.UserId() == null)
                    return ServiceResult<IEnumerable<TicketDto>>.Unauthorized("you must login first!");

                if (!user.IsEmployee())
                    return ServiceResult<IEnumerable<TicketDto>>.Forbidden("Only employees can get their tickets!");

                var tickets = await _ticketRepository.GetAllAsync(m => m.CreatedByUserId == user.UserId());
                return ServiceResult<IEnumerable<TicketDto>>.Success(_mapper.Map<List<TicketDto>>(tickets));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error acurred on TicketService_GetTicketsByUserAsync: {ex.Message} {ex.InnerException?.Message ?? ""}", new object[] {  user });

                return ServiceResult<IEnumerable<TicketDto>>.Error("Something went wrong, Please contact your Admin or try again later.");
            }
        }

        public async Task<ServiceResult<IEnumerable<TicketStatusDto>>> GetCountByStatusAsync(ClaimsPrincipal user)
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
                    .AsEnumerable();

                return ServiceResult<IEnumerable<TicketStatusDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error acurred on TicketService_GetCountByStatusAsync: {ex.Message} {ex.InnerException?.Message ?? ""}", new object[] { user });

                return ServiceResult<IEnumerable<TicketStatusDto>>.Error("Something went wrong, Please contact your Admin or try again later.");
            }
        }
    }
}
