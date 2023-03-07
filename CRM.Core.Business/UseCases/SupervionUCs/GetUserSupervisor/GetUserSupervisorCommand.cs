using CRM.Core.Business.Models.Supervision;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.GetUserSupervisor;

public class GetUserSupervisorCommand: IRequest<SupervisionOutModel>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
}
