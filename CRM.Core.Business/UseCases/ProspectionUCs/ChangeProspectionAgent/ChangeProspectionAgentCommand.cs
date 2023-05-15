using CRM.Core.Business.Models.Prospect;
using MediatR;

namespace CRM.Core.Business.UseCases.ProspectionUCs.ChangeProspectionAgent;

public class ChangeProspectionAgentCommand : ProspectionInUpdateModel, IRequest<ProspectionOutModel>
{
    public string UserName { get; set; }
    public ChangeProspectionAgentCommand(Guid productId, Guid companyId, Guid agentId, Guid newAgentId, string userName) : base(productId, companyId, agentId, newAgentId)
    {
        UserName = userName;
    }

}