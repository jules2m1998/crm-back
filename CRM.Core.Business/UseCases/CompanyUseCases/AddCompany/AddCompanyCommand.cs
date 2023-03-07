using CRM.Core.Business.Models.Company;
using CRM.Core.Domain.Entities;
using MediatR;

namespace CRM.Core.Business.UseCases.CompanyUseCases.AddCompany;

public class AddCompanyCommand: IRequest<CompanyOutModel>
{
    public CompanyInModel Company { get; set; } = null!;
    public string UserName { get; set; } = string.Empty;

    public AddCompanyCommand()
    {
    }

    public AddCompanyCommand(CompanyInModel company, string userName)
    {
        Company = company;
        UserName = userName;
    }
}
