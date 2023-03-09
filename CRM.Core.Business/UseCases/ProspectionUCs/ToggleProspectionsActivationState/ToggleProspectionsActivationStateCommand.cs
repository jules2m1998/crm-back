using CRM.Core.Business.Models.Prospect;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.ToggleProspectionsActivationState;

public class ToggleProspectionsActivationStateCommand: IRequest<ICollection<ProspectionOutModel>>
{
    public ICollection<ProspectionInModel> Models { get; set; }
    public string UserName { get; set; }

    public ToggleProspectionsActivationStateCommand(ICollection<ProspectionInModel> models, string userName)
    {
        Models = models;
        UserName = userName;
    }
}
