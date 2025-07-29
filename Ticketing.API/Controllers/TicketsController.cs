using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Security.Claims;
using Ticketing.DTO;
using Ticketing.Model;
using Ticketing.Repository;

namespace Ticketing.API.Controllers
{
    [ApiController]
    [Route("tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _TicketRepository;
        private readonly IAuthRepository _AuthRepository;

        public TicketsController(ITicketRepository ticketRepository, IAuthRepository authRepository)
        {
            _TicketRepository = ticketRepository;
            _AuthRepository = authRepository;
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketCreateDto dto)
        {
            try
            {
                Guid userId;
                if (!Guid.TryParse(User.FindFirst("id")?.Value, out userId))
                    return Unauthorized();

                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    Title = dto.Title,
                    Description = dto.Description,
                    Priority = dto.Priority,
                    Status = TicketStatus.Open,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedByUserId = userId
                };

                var result = await _TicketRepository.AddAsync(ticket);

                return Ok(new { data = result, message = $" Ticket {result.Title} updated successfully" });

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet("my")]
        public async Task<IActionResult> MyTickets()
        {
            try
            {
                Guid userId;
                if (!Guid.TryParse(User.FindFirst("id")?.Value, out userId))
                    return Unauthorized();

                var result = await _TicketRepository.GetAllAsync(m => m.CreatedByUserId == userId);

                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var result = await _TicketRepository.GetAllAsync();

                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        
        public async Task<IActionResult> Update(Guid id, TicketUpdateDto dto)
        {
            try
            {
                var assignedUser = await _AuthRepository.GetUserAsync(dto.AssignedToUserId);

                if (assignedUser == null)
                    return NotFound(new { data = dto, message = "Assigned User not found!" });

                if(assignedUser.Role != UserRole.Admin)
                    return Unauthorized(new { data = dto, message = "Assigned User is not admin! Tickets should only assigned to admins." });


                var ticket = await _TicketRepository.GetByIdAsync(id);
                if (ticket == null) return NotFound(new { data = dto, message = "Ticket not found!" });


                ticket.Status = dto.Status;
                ticket.AssignedToUserId = dto.AssignedToUserId;

                var result = await _TicketRepository.UpdateAsync(ticket);
                return Ok(new { data = result, message = $" Ticket {result.Title} updated successfully" });

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("stats")]
        //Show ticket counts by status(Admin only)
        public async Task<IActionResult> Stats()
        {
            try
            {
                var result = await _TicketRepository.GetCountByStatusAsync();
                return Ok(result);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                var ticket = await _TicketRepository.GetByIdAsync(id);
                if (ticket == null) return NotFound();

                // Restrict get for creator and assigned admin
                if (!(ticket.CreatedByUserId.ToString() == userId ||
                    (ticket.AssignedToUserId.ToString() == userId && userRole == "Admin")))
                    return Forbid();

                return Ok(ticket);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {                
                //physical delete
                var result = _TicketRepository.DeleteAsync(id);
                if (result == null) return NotFound();

                return Ok("Ticket Deleted Successfully.");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
