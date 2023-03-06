using CRM.Core.Business.Models.Supervision;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.AssignSupervisor;

public class AssignSupervisorCommand: IRequest<ICollection<SupervisionOutModel>>
{
    public SupervisionInModel Model { get; set; }
    public string UserName { get; set; }

    public AssignSupervisorCommand(SupervisionInModel model, string userName)
    {
        Model = model;
        UserName = userName;
    }
}
