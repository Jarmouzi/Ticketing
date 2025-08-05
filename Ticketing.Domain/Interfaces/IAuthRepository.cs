using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Repository
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> CreateAsync(User user);
        Task<User?> GetUserAsync(Guid id);
    }
}
