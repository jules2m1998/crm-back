using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.PhoneNumber;

public class PhoneInModel
{
    public string Value { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid CompanyId { get; set; }
}
