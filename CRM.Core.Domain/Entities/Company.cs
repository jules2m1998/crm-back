using CRM.Core.Domain.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class Company: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string CEOPicture { get; set; } = string.Empty;
    public string CEOName { get; set; } = string.Empty;
    public string Values { get; set; } = string.Empty;
    public string Mission { get; set; } = string.Empty;
    public string Concurrent { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ActivityArea ActivityArea { get; set; }
    public CompaniesType Size { get; set; }

    public virtual ICollection<CompanyContact>? CompanyContacts { get; set; }
    public virtual ICollection<Prospect>? Prospections { get; set; } 
}
