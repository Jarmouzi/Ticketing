using Microsoft.EntityFrameworkCore;
using Ticketing.Repository;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Domain.Entities;

public class AuthRepository : IAuthRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DbSet<User> _service;

    public AuthRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _service = _unitOfWork.Set<User>();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _service.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> CreateAsync(User user)
    {
        try
        {
            _service.Add(user);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<User?> GetUserAsync(Guid id)
    {
        try
        {
            return await _service.Where(m => m.Id == id).FirstOrDefaultAsync();
        }
        catch (Exception)
        {
            throw;
        }

    }
}
