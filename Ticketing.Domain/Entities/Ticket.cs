using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketStatus Status { get; set; }
        public TicketPriority Priority { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }
    }
}
