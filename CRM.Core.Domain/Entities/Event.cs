using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class Event: BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public virtual Prospect? Prospect { get; set; } = null!;
    public virtual User Owner { get; set; } = null!;
    public virtual ICollection<Contact>? Contact { get; set; }
    public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
}
