using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Exceptions;

public class BaseException: Exception
{
    public Dictionary<string, List<string>> Errors { get; set; }

    public BaseException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }
}
