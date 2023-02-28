using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class Prospect
{
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
    public virtual User Agent { get; set; } = null!;
    public virtual User? Creator { get; set; } = null!;
    public virtual ICollection<ProspectionHistory> History { get; set;} = null!;
}
