using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ticketing.Application.DTOs;
using Ticketing.Application.Services;
using Ticketing.Repository;

namespace Ticketing.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtService _jwtService;

        public AuthController(IAuthService authService, JwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid request payload.");

            var user = await _authService.AuthenticateAsync(request.Email, request.Password);
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
    }
}
