using CRM.Core.Business.Models.Company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.GetOneCompany;

public class GetOneCompanyCommand: IRequest<CompanyOutModel>
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}
