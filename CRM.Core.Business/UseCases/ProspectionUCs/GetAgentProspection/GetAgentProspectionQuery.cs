using CRM.Core.Business.Models.Prospect;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetAgentProspection;

public class GetAgentProspectionQuery: IRequest<ICollection<ProspectionOutModel>>
{
    public Guid AgentId { get; set; }
    public string UserName { get; set; }

    public GetAgentProspectionQuery(Guid agentId, string userName)
    {
        AgentId = agentId;
        UserName = userName;
    }
}
