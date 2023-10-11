using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Exceptions;

public class NotValidDataException : BaseException
{
    public NotValidDataException(Dictionary<string, List<string>> errors) : base(errors)
    {
    }
}
