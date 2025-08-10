using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Core.Results
{
    public enum ServiceStatus
    {
        Success,
        NotFound,
        Forbidden,
        Unauthorized,
        Error
    }
}
