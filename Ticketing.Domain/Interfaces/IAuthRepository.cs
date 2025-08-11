using System;
using Ticketing.Domain.Entities;

namespace Ticketing.Repository
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CreateAsync(User user);
        Task<User?> GetUserAsync(Guid id);
    }
}
