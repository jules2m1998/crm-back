using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class ProspectionHistory
{
    public Guid Id { get; set; }
    public User User { get; set; } = null!;
    public User? UpdatedBy { get; set; } = null!;
    public Prospect Prospect { get; set; } = null!;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
}
