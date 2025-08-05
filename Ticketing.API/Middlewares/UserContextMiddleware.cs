using System.Security.Claims;

namespace Ticketing.API.Middlewares
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst("id")?.Value;

                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid or missing user ID claim.");
                    return;
                }

                context.Items["UserId"] = userId;
                context.Items["UserRole"] = context.User.FindFirst(ClaimTypes.Role)?.Value;
            }

            await _next(context);
        }
    }

}
