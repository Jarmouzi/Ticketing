
namespace Ticketing.Infrastructure.interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveAsync();
        IEnumerable<TResult> ExecWithStoreProcedure<TResult>(string query, params object[] parameters) where TResult : class;
        int ExecWithStoreProcedure(string query, params object[] parameters);
        Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class;

    }
}
