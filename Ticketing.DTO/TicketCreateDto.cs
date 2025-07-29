using Ticketing.Model;

namespace Ticketing.DTO
{
    public class TicketCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketPriority Priority { get; set; }  // Enum: Low, Medium, High
    }
}
