using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class SupervisionHistory
{
    public Guid SupervisedId { get; set; }
    public Guid SupervisorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User Supervised { get; set; } = null!;
    public User Supervisor { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
