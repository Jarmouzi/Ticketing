using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Model;

namespace Ticketing.Repository
{
    public interface IAuthRepository
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<bool> RegisterAsync(string fullName, string email, string password, UserRole role);
        Task<User?> GetUserAsync(Guid id);
    }
}
