using CRM.Core.Business.Models.Prospect;
using MediatR;

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetOneProspection;

public class GetOneProspectionQuery: IRequest<ProspectionOutModel?>
{
    public ProspectionInModel Models { get; set; }
    public string UserName { get; set; }

    public GetOneProspectionQuery(ProspectionInModel models, string userName)
    {
        Models = models;
        UserName = userName;
    }
}
