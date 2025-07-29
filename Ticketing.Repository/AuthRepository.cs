using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketing.DataContext;
using Ticketing.Model;
using BCrypt.Net;
using System;
using Ticketing.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly DbSet<User> _service;

    public AuthRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _service = _unitOfWork.Set<User>();
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        try
        {
            var user = await _service.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<bool> RegisterAsync(string fullName, string email, string password, UserRole role)
    {
        try
        {
            if (await _service.AnyAsync(u => u.Email == email))
                return false;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = fullName,
                Email = email,
                PasswordHash = hashedPassword,
                Role = role
            };

            _service.Add(user);
            await _unitOfWork.SaveAsync();
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
