using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Model
{
    public enum TicketStatus { Open, InProgress, Closed }
    public enum TicketPriority { Low, Medium, High }

    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }

        [DefaultValue("getdate()")]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }
    }
}
