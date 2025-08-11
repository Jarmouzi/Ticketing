using Ticketing.Domain.Entities;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<bool> RegisterAsync(string fullName, string email, string password, UserRole role);
        Task<User?> GetUserAsync(Guid id);
    }
}
