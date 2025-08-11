using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.interfaces;

namespace Ticketing.Infrastructure.Persistence
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly TicketingDBContext _context;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TicketingDBContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public void Dispose()
        {
            //_context.Dispose();
        }

        public new Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        public async Task<int> SaveAsync()
        {
            var entries = _context.ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
                ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return await _context.SaveChangesAsync();
        }

        public IEnumerable<TResult> ExecWithStoreProcedure<TResult>(string query, params object[] parameters) where TResult : class
        {
            try
            {
                if (parameters == null)
                {
                    parameters = new object[] { };
                }

                var a = _context.Database.SqlQueryRaw<TResult>(query, parameters);
                return a;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int ExecWithStoreProcedure(string query, params object[] parameters)
        {
            try
            {
                return _context.Database.ExecuteSqlRaw(query, parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
