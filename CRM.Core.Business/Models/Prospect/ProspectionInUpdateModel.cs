using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Prospect;

public class ProspectionInUpdateModel : ProspectionInModel
{
    public Guid NewAgentId { get; set; }
    public ProspectionInUpdateModel(Guid productId, Guid companyId, Guid agentId, Guid newAgentId) : base(productId, companyId, agentId)
    {
        NewAgentId = newAgentId;
    }
}
