using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IResponse
    {
        string Message { get; set; }
        ResponseType ResponseType { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T Data { get; set; }
        List<CustomValidationError> ValidationErrors { get; set; }
    }
}
