using CRM.Core.Business.Models.Prospect;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetAllProspection;

public record GetAllProspectionQuery(string UserName): IRequest<ICollection<ProspectionOutModel>>;
