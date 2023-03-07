using CRM.Core.Business.Models.Supervision;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.GetAllSupervision;

public class GetAllSupervisionQuery: IRequest<ICollection<SupervisionOutModel>>
{
    public string UserName { get; set; }
    public GetAllSupervisionQuery(string userName)
    {
        UserName = userName;
    }
}
