using CRM.Core.Business.Models.Company;
using CRM.Core.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.PatchCompany;

public class PatchCompanyCommand: IRequest<CompanyOutModel>
{
    public JsonPatchDocument<Company> JsonPatchDocument { get; set; } = null!;
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}
