using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class HeadProspection: BaseEntity
{
    public Guid CommitId { get; set; }
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid AgentId { get; set; }

    public virtual Product Product { get; set; } = null!;
    public virtual Company Company { get; set; } = null!;
    public virtual User Agent { get; set; } = null!;
    public virtual Commit Commit { get; set; } = null!;
}
