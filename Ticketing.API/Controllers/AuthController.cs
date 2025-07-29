using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ticketing.Repository;
using Ticketing.DTO;

namespace Ticketing.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly JwtService _jwtService;

        public AuthController(IAuthRepository authRepository, JwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid request payload.");

                var user = await _authRepository.AuthenticateAsync(request.Email, request.Password);
                if (user == null)
                    return Unauthorized("Invalid email or password.");

                var token = _jwtService.GenerateToken(user);

                return Ok(new
                {
                    Token = token,
                    User = new
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        Role = user.Role.ToString()
                    }
                });

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
