using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Common.Results;

namespace Ticketing.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _TicketService;
        private readonly IAuthService _authService;

        public TicketsController(ITicketService ticketService, IAuthService authService)
        {
            _TicketService = ticketService;
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketCreateDto dto)
        {
            var result = await _TicketService.CreateAsync(dto, User);

            return result.Status switch
            {
                ServiceStatus.Success => Ok(result.Data),
                ServiceStatus.NotFound => NotFound(result.ErrorMessage),
                ServiceStatus.Unauthorized => Unauthorized(result.ErrorMessage),
                ServiceStatus.Forbidden => Forbid(),
                ServiceStatus.Error => StatusCode(500, result.ErrorMessage),
                _ => BadRequest()
            };
        }

        [HttpGet("my")]
        public async Task<IActionResult> MyTickets()
        {
            var result = await _TicketService.GetTicketsByUserAsync(User);

            return result.Status switch
            {
                ServiceStatus.Success => Ok(result.Data),
                ServiceStatus.NotFound => NotFound(result.ErrorMessage),
                ServiceStatus.Unauthorized => Unauthorized(result.ErrorMessage),
                ServiceStatus.Forbidden => Forbid(),
                ServiceStatus.Error => StatusCode(500, result.ErrorMessage),
                _ => BadRequest()
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _TicketService.GetAllTicketsAsync(User);

            return Ok(result);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(Guid id, TicketUpdateDto dto)
        {
            var result = await _TicketService.UpdateAsync(id, dto, User);

            return result.Status switch
            {
                ServiceStatus.Success => Ok(result.Data),
                ServiceStatus.NotFound => NotFound(result.ErrorMessage),
                ServiceStatus.Unauthorized => Unauthorized(result.ErrorMessage),
                ServiceStatus.Forbidden => Forbid(),
                ServiceStatus.Error => StatusCode(500, result.ErrorMessage),
                _ => BadRequest()
            };

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("stats")]
        //Show ticket counts by status(Admin only)
        public async Task<IActionResult> Stats()
        {
            var result = await _TicketService.GetCountByStatusAsync(User);

            return result.Status switch
            {
                ServiceStatus.Success => Ok(result.Data),
                ServiceStatus.NotFound => NotFound(result.ErrorMessage),
                ServiceStatus.Unauthorized => Unauthorized(result.ErrorMessage),
                ServiceStatus.Forbidden => Forbid(),
                ServiceStatus.Error => StatusCode(500, result.ErrorMessage),
                _ => BadRequest()
            };
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _TicketService.GetByIdAsync(id, User);

            return result.Status switch
            {
                ServiceStatus.Success => Ok(result.Data),
                ServiceStatus.NotFound => NotFound(result.ErrorMessage),
                ServiceStatus.Unauthorized => Unauthorized(result.ErrorMessage),
                ServiceStatus.Forbidden => Forbid(),
                ServiceStatus.Error => StatusCode(500, result.ErrorMessage),
                _ => BadRequest()
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {            
            var result = await _TicketService.DeleteAsync(id, User);

            return result.Status switch
            {
                ServiceStatus.Success => Ok(result.Data),
                ServiceStatus.NotFound => NotFound(result.ErrorMessage),
                ServiceStatus.Unauthorized => Unauthorized(result.ErrorMessage),
                ServiceStatus.Forbidden => Forbid(),
                ServiceStatus.Error => StatusCode(500, result.ErrorMessage),
                _ => BadRequest()
            };
        }
    }
}
