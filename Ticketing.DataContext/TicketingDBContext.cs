using Microsoft.EntityFrameworkCore;
using Ticketing.Model;

namespace Ticketing.DataContext
{
    public class TicketingDBContext: DbContext
    {
        public TicketingDBContext(DbContextOptions<TicketingDBContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.CreatedByUser)
                .WithMany()
                .HasForeignKey(t => t.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AssignedToUser)
                .WithMany()
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
