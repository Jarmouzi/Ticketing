using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Model;

namespace Ticketing.DTO
{
    public class TicketUpdateDto
    {
        public TicketStatus Status { get; set; }

        public Guid AssignedToUserId { get; set; }
    }
}
