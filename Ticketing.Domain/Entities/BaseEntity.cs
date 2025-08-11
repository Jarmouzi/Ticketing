
namespace Ticketing.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
    }
}
