using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Exceptions
{
    public static class ErrorMessage
    {
        public static string GetFieldAlReadyExistMsg(string fieldName)
        {
            return $"This {fieldName} already exist !";
        }
        public static string GetFieldInvalidMsg(string fieldName)
        {
            return $"This {fieldName} is invalid !";
        }
    }
}
