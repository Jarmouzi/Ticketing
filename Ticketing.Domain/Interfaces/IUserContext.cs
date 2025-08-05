using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Domain.Interfaces
{
    public interface IUserContext
    {
        Guid? UserId { get; }
        bool IsAuthenticated { get; }
    }
}
