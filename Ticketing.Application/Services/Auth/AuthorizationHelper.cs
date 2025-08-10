using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Services.Auth
{
    public static class AuthorizationHelper
    {
        /// <summary>
        ///   check if user is the ticket creator or assigned admin
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static bool IsTicketAssigned(this ClaimsPrincipal user, Ticket ticket)
        {                

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return false;
            }
            var isAdmin = user.IsInRole("Admin");

            return (ticket.CreatedByUserId == userId ||
                (ticket.AssignedToUserId == userId && isAdmin ));
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("Admin");
        }
        public static bool IsEmployee(this ClaimsPrincipal user)
        {
            return user.IsInRole("Employee");
        }

        public static Guid? UserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return null;
            }
            return userId;
        }
    }
}
