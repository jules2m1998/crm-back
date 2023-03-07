using CRM.Core.Business.Models.Supervision;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.ToggleSupervisionState;

public class ToggleSupervisionStateCommand: IRequest<SupervisionOutModel?>
{
    public Guid SupervisorId { get; set; }
    public Guid SupervisedId { get; set; }
    public string UserName { get; set; } = string.Empty;
}
