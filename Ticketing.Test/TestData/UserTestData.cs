using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Test.TestData
{
    public static class UserTestData
    {
        public static ClaimsPrincipal CreateEmployeeUser() =>
           new ClaimsPrincipal(new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, UserRole.Employee.ToString())
                        }));

        public static ClaimsPrincipal CreateAdminUser() =>
           new ClaimsPrincipal(new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                            new Claim(ClaimTypes.Role, UserRole.Admin.ToString())
                        }));
    }
}
