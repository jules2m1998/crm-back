using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class PhoneNumber
{
    public string Value { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual Contact Contact { get; set; } = null!;
}
