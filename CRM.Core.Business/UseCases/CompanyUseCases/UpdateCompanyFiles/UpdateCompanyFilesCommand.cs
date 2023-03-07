using CRM.Core.Business.Models.Company;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.UpdateCompanyFiles;

public class UpdateCompanyFilesCommand: IRequest<CompanyOutModel>
{
    public UpdateCompanyInModel CompanyFiles { get; set; } = null!;
    public string UserName { get; set; } = string.Empty;
    public Guid Id { get; set; }
}
