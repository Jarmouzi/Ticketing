using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Entities
{
    

    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // hashed
        public UserRole Role { get; set; }

        public ICollection<Ticket> CreatedTickets { get; set; }
        public ICollection<Ticket> AssignedTickets { get; set; }
    }
}
