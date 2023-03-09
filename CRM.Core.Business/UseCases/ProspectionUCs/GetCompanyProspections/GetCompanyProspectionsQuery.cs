using CRM.Core.Business.Models.Prospect;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetCompanyProspections;

public class GetCompanyProspectionsQuery: IRequest<ICollection<ProspectionOutModel>>
{
    public Guid CompanyId { get; set; }
    public string UserName { get; set; }

    public GetCompanyProspectionsQuery(Guid companyId, string userName)
    {
        CompanyId = companyId;
        UserName = userName;
    }
}
