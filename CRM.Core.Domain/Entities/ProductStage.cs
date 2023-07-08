using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class ProductStage: BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsFirst { get; set; } = false;
    public bool IsDone { get; set; } = false;
    public virtual ICollection<StageQuetion> Quetions { get; set; } = new List<StageQuetion>();
}
