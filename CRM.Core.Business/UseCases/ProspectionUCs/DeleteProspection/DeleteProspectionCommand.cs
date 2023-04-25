using CRM.Core.Business.Models.Prospect;
using MediatR;

namespace CRM.Core.Business.UseCases.ProspectionUCs.DeleteProspection;

public record DeleteProspectionCommand(
    Guid ProductId, 
    Guid CompanyId, 
    Guid AgentId,
    string UserName
) : IRequest<ProspectionOutModel>;
