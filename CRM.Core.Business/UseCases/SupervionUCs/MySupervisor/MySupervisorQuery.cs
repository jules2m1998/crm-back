using CRM.Core.Business.Models.Supervision;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.MySupervisor;

public record MySupervisorQuery(string SupervisedUserName): IRequest<SupervisionOutModel?>;