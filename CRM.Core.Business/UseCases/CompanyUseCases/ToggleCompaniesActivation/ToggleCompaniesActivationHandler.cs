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

namespace CRM.Core.Business.UseCases.CompanyUseCases.ToggleCompaniesActivation;

public class ToggleCompaniesActivationHandler : IRequestHandler<ToggleCompaniesActivationCommand, ICollection<CompanyOutModel>>
{
    private readonly IUserRepository _userRepo;
    private readonly ICompanyRepository _repo;

    public ToggleCompaniesActivationHandler(IUserRepository userRepo, ICompanyRepository repo)
    {
        _userRepo = userRepo;
        _repo = repo;
    }

    public async Task<ICollection<CompanyOutModel>> Handle(ToggleCompaniesActivationCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        ICollection<Company> companies = isAdmin ? await _repo.GetManyAsync(request.Ids) : await _repo.GetManyAsync(request.Ids, request.UserName);
        if(companies.Count != request.Ids.Count) throw new UnauthorizedAccessException();
        foreach(var company in companies)
        {
            company.IsActivated = !company.IsActivated;
        }
        ICollection<Company> result = await _repo.UpdateManyAsync(companies);
        return result.Select(c => c.ToCompanyOutModel()).ToList();
        throw new NotImplementedException();
    }
}
