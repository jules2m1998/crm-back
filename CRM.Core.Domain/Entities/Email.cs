using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class Email: BaseEntity
{
    public string ReceiverEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime ToSendThe { get; set; }

    public virtual Event Event { get; set; } = null!;
}
