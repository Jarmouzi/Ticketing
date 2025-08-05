using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Interfaces;

namespace Ticketing.Infrastructure.Repositories
{
    public class HttpContextUserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextUserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                    return null;

                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : (Guid?)null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

}
