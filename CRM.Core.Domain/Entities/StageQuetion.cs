using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class StageQuetion: BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public virtual ProductStage Quetion { get; set; } = null!;
    public virtual ICollection<StageResponse> Responses { get; set; } = new List<StageResponse>();
}
