using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class CompanyContact: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Job { get; set; } = string.Empty;
    public ICollection<string> Phones { get; set; } = new List<string>();

    public virtual Company Company { get; set; } = null!;
}
