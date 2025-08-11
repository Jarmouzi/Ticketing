using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface IJWTService
    {
        string GenerateToken(User user);
    }
}
