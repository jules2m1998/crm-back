using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class Product: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Logo { get; set;} = string.Empty;
    public string Description { get; set; } = string.Empty;

    public virtual ICollection<Prospect>? Prospections { get; set; }
}
