using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.PatchCompany;

public class PatchCompanyHandler : IRequestHandler<PatchCompanyCommand, CompanyOutModel>
{
    private readonly IUserRepository _userRepo;
    private readonly ICompanyRepository _repo;

    public PatchCompanyHandler(IUserRepository userRepo, ICompanyRepository repo)
    {
        _userRepo = userRepo;
        _repo = repo;
    }

    public async Task<CompanyOutModel> Handle(PatchCompanyCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        var company = isAdmin ? await _repo.GetOneAsync(request.Id) : await _repo.GetOneAsync(request.Id, request.UserName);
        if (company == null) throw new NotFoundEntityException("Company not found !");
        request.JsonPatchDocument.ApplyTo(company);

        Company result = await _repo.UpdateAsync(company);

        return result.ToCompanyOutModel();
    }
}
