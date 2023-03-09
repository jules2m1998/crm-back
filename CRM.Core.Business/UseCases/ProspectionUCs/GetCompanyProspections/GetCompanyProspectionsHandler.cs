using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetCompanyProspections;

public class GetCompanyProspectionsHandler : IRequestHandler<GetCompanyProspectionsQuery, ICollection<ProspectionOutModel>>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;

    public GetCompanyProspectionsHandler(IProspectionRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<ICollection<ProspectionOutModel>> Handle(GetCompanyProspectionsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        ICollection<Prospect> prospection =
            isAdmin
            ? await _repo.GetProspectionByCompany(request.CompanyId)
            : await _repo.GetProspectionByCompany(request.CompanyId, request.UserName);

        return prospection.Select(pr => pr.ToModel()).ToList();
    }
}
