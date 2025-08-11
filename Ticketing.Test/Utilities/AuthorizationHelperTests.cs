using System.Security.Claims;
using Ticketing.Application.Services.Auth;
using Ticketing.Domain.Entities;
using FluentAssertions;

namespace Ticketing.Test.Utilities
{
    public class AuthorizationHelperTests
    {
        [Fact]
        public void IsTicketAssigned_ReturnsTrue_ForAdmin()
        {
            var userId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                            new Claim(ClaimTypes.Role, "Admin")
                        }));

            var ticket = new Ticket { CreatedByUserId = userId, AssignedToUserId = userId };

            var result = AuthorizationHelper.IsTicketAssigned(user, ticket);

            result.Should().BeTrue();
        }

    }
}
