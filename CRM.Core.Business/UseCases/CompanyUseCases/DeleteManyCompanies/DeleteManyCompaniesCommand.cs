using CRM.Core.Business.Models.Company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.DeleteManyCompanies;

public class DeleteManyCompaniesCommand: IRequest<bool>
{
    public ICollection<Guid> Ids { get; set; }
    public string UserName { get; set; }

    public DeleteManyCompaniesCommand(ICollection<Guid> ids, string userName)
    {
        Ids = ids;
        UserName = userName;
    }
}
