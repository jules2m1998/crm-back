using CRM.Core.Business.Models.Prospect;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.AttributeProspection;

public class AttributeProspectionCommand: IRequest<ProspectionOutModel>
{
    public ProspectionInModel Model { get; set; }
    public string UserName { get; set; }

    public AttributeProspectionCommand(ProspectionInModel model, string userName)
    {
        Model = model;
        UserName = userName;
    }
}
