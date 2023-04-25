using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.GetAllCompanies;

public class GetAllCompaniesHandler : IRequestHandler<GetAllCompaniesQuery, ICollection<CompanyOutModel>>
{
    private readonly ICompanyRepository _repo;
    private readonly IUserRepository _userRepo;

    public GetAllCompaniesHandler(ICompanyRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<ICollection<CompanyOutModel>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken) =>
        (await _repo.GetAllAsync()).Select(c => c.ToCompanyOutModel()).ToList();
    
}
