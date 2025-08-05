using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Infrastructure.Persistence
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
                .Property(e => e.Status)
                .HasConversion<byte>();

            modelBuilder.Entity<Ticket>()
                .Property(e => e.Priority)
                .HasConversion<byte>();

            modelBuilder.Entity<Ticket>()
                .Property(b => b.Id)
                .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<Ticket>()
                .Property(p => p.Status)
                .HasDefaultValue(TicketStatus.Open);

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

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion<byte>();
        }

    }
}
