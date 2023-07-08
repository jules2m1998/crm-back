using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public class StageResponse: BaseEntity
{
    public string Response { get; set; } = string.Empty;
    public virtual StageQuetion Quetion { get; set; } = null!;
    public virtual ProductStage? NextStage { get; set; } = null;

}
