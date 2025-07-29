using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.DataContext
{
    public interface IUnitOfWork : IDisposable 
    {
        Task<int> SaveAsync();
        IEnumerable<TResult> ExecWithStoreProcedure<TResult>(string query, params object[] parameters) where TResult : class;
        int ExecWithStoreProcedure(string query, params object[] parameters);
        Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class;

    }
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
