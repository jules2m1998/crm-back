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

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetOneProspection;

public class GetOneProspectionHandler : IRequestHandler<GetOneProspectionQuery, ProspectionOutModel?>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;

    public GetOneProspectionHandler(IProspectionRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<ProspectionOutModel?> Handle(GetOneProspectionQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        Prospect? p = 
            isAdmin 
                ? await _repo.GetOneAsync(request.Models.AgentId, request.Models.ProductId, request.Models.CompanyId)
                : await _repo.GetOneAsync(request.Models.AgentId, request.Models.ProductId, request.Models.CompanyId, creatorUserName: request.UserName);

        return p?.ToModel();
    }
}
