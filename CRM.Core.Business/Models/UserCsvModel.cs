using CRM.Core.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models
{
    public class UserCsvModel : BaseUserCsvModel, IFileReadable
    {
        public FIleReadStatus Status { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
    }
}
