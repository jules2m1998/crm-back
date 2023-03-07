using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.CompanyUseCases.DeleteManyCompanies;

public class DeleteManyCompaniesHandler : IRequestHandler<DeleteManyCompaniesCommand, bool>
{
    private readonly IUserRepository _userRepo;
    private readonly ICompanyRepository _repo;

    public DeleteManyCompaniesHandler(IUserRepository userRepo, ICompanyRepository repo)
    {
        _userRepo = userRepo;
        _repo = repo;
    }

    public async Task<bool> Handle(DeleteManyCompaniesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        ICollection<Company> companies = isAdmin ? await _repo.GetManyAsync(request.Ids) : await _repo.GetManyAsync(request.Ids, request.UserName);
        if (companies.Count != request.Ids.Count) throw new UnauthorizedAccessException();
        if(isAdmin) await _repo.DeleteManyAsync(companies);
        else await _repo.MarkAsDeletedAsync(companies);
        return true;
    }
}
