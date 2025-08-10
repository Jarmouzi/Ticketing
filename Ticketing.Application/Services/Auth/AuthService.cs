using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System;
using Ticketing.Repository;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.interfaces;
using Ticketing.Domain.ValueObjects;
using Ticketing.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ticketing.Application.Services.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthRepository _authRepository;
        private readonly IJWTService _JWTService;

        public AuthService(IAuthRepository authRepository, IUnitOfWork unitOfWork, IJWTService jWTService, ILogger<TicketService> logger) : base(logger)
        {
            _authRepository = authRepository;
            _unitOfWork = unitOfWork;
            _JWTService = jWTService;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            try
            {
                var user = await _authRepository.GetUserByEmailAsync(email);
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
                if (await _authRepository.GetUserByEmailAsync(email) != null)
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

                await _authRepository.CreateAsync(user);
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
                return await _authRepository.GetUserAsync(id);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Task<string> GenerateTokenAsync(User user)
        {
            return Task.FromResult(_JWTService.GenerateToken(user));
        }
    }
}
