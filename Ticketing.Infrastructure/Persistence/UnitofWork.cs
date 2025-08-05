using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Interfaces;
using Ticketing.Domain.ValueObjects;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Infrastructure.Repositories;

namespace Ticketing.Infrastructure.Persistence
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly TicketingDBContext _context;
        private Dictionary<Type, object> _repositories;
        private readonly IUserContext _userContext;

        public UnitOfWork(TicketingDBContext context, IUserContext userContext)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _userContext = userContext; 
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

                    if(_userContext.UserId.HasValue)
                        ((BaseEntity)entityEntry.Entity).CreatedByUserId = _userContext.UserId.Value;
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
