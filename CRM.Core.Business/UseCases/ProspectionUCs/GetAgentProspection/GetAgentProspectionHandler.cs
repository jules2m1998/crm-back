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

namespace CRM.Core.Business.UseCases.ProspectionUCs.GetAgentProspection;

public class GetAgentProspectionHandler : IRequestHandler<GetAgentProspectionQuery, ICollection<ProspectionOutModel>>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepository;

    public GetAgentProspectionHandler(IProspectionRepository repo, IUserRepository userRepository)
    {
        _repo = repo;
        _userRepository = userRepository;
    }

    public async Task<ICollection<ProspectionOutModel>> Handle(GetAgentProspectionQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);

        ICollection<Prospect> prospection =
            isAdmin 
            ? await _repo.GetProspectionByAgent(request.AgentId) 
            : await _repo.GetProspectionByAgent(request.AgentId, request.UserName);

        return prospection
            .Select(pr => pr.ToModel())
            .ToList();
    }
}
