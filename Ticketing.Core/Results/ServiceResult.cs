using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticketing.Core.Results
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public ServiceStatus Status { get; set; }
        public string? ErrorMessage { get; set; }

        public static ServiceResult<T> Success(T data) => new() { Data = data, Status = ServiceStatus.Success };
        public static ServiceResult<T> NotFound(string message) => new() { Status = ServiceStatus.NotFound, ErrorMessage = message };
        public static ServiceResult<T> Forbidden(string message) => new() { Status = ServiceStatus.Forbidden, ErrorMessage = message };
        public static ServiceResult<T> Unauthorized(string message) => new() { Status = ServiceStatus.Unauthorized, ErrorMessage = message };
        public static ServiceResult<T> Error(string message) => new() { Status = ServiceStatus.Error, ErrorMessage = message };
    }
}
