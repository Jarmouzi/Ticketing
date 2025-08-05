using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.ValueObjects;
using Ticketing.Repository;

namespace Ticketing.API.Controllers
{
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

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketCreateDto dto)
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            var result = await _TicketService.CreateTicketAsync(dto, userId);

            return Ok(new { data = result, message = $" Ticket {result.Title} updated successfully" });
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("my")]
        public async Task<IActionResult> MyTickets()
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            var result = await _TicketService.GetTicketsByUserAsync(userId);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _TicketService.GetAllTicketsAsync();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(Guid id, TicketUpdateDto dto)
        {
            var assignedUser = await _authService.GetUserAsync(dto.AssignedToUserId);

            if (assignedUser == null)
                return NotFound(new { data = dto, message = "Assigned User not found!" });

            if (assignedUser.Role != UserRole.Admin)
                return Unauthorized(new { data = dto, message = "Assigned User is not admin! Tickets should only assigned to admins." });

            var result = await _TicketService.UpdateStatusAsync(id, dto);

            if (result == null)
                return NotFound(new { data = dto, message = "Ticket not found!" });

            return Ok(new { data = result, message = $" Ticket {result.Title} updated successfully" });

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("stats")]
        //Show ticket counts by status(Admin only)
        public async Task<IActionResult> Stats()
        {
            var result = await _TicketService.GetCountByStatusAsync();
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            var ticket = await _TicketService.GetByIdAsync(id);
            if (ticket == null) return NotFound();

            // Restrict get for creator and assigned admin
            if (!(ticket.CreatedByUserId == userId ||
                (ticket.AssignedToUserId == userId && HttpContext.Items["UserRole"] == "Admin")))
                return Forbid();

            return Ok(ticket);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //physical delete
            var result = _TicketService.DeleteAsync(id);
            if (result == null) return NotFound();

            return Ok("Ticket Deleted Successfully.");
        }
    }
}
