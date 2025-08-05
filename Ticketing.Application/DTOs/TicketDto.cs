using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.DTOs
{
    public class TicketDto
    {
        private string v;

        public TicketDto(Guid id, string title, string v, DateTime createdAt)
        {
            Id = id;
            Title = title;
            this.v = v;
            CreatedAt = createdAt;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }
    }
}
