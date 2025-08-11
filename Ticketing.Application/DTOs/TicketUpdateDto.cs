using Ticketing.Domain.ValueObjects;

namespace Ticketing.Application.DTOs
{
    public class TicketUpdateDto
    {
        public TicketStatus Status { get; set; }

        public Guid AssignedToUserId { get; set; }
    }
}
