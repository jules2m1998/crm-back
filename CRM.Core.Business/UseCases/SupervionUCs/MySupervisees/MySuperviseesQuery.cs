using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Models.Supervision;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.MySupervisees;

public record MySuperviseesQuery(string UserName): IRequest<ICollection<SupervisionOutModel>>;