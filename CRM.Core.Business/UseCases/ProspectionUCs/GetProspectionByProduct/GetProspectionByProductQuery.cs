using CRM.Core.Business.Models.Prospect;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetProspectionByProduct;

public class GetProspectionByProductQuery: IRequest<ICollection<ProspectionOutModel>>
{
    public Guid ProductId { get; set; }
    public string UserName { get; set; }

    public GetProspectionByProductQuery(Guid productId, string userName)
    {
        ProductId = productId;
        UserName = userName;
    }
}
