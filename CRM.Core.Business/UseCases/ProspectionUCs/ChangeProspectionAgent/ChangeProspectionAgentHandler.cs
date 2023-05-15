using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProspectionUCs.ChangeProspectionAgent;

public class ChangeProspectionAgentHandler : IRequestHandler<ChangeProspectionAgentCommand, ProspectionOutModel>
{
    private readonly IProspectionRepository _repo;
    private readonly IUserRepository _userRepo;
    private readonly ISupervisionHistoryRepository _supervisorHistoryRepo;

    public ChangeProspectionAgentHandler(IProspectionRepository repo, IUserRepository userRepo, ISupervisionHistoryRepository supervisorHistoryRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
        _supervisorHistoryRepo = supervisorHistoryRepo;
    }

    public async Task<ProspectionOutModel> Handle(ChangeProspectionAgentCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName) ?? throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);
        var supervisionHistory = await _supervisorHistoryRepo.GetUserSupervisor(request.NewAgentId);
        

        if (
            !isAdmin &&
            (supervisionHistory != null
            &&
            (!supervisionHistory.IsActive || supervisionHistory.Supervisor.UserName != request.UserName))
            ) throw new UnauthorizedAccessException();

        if(supervisionHistory is null && !isAdmin)
            throw new UnauthorizedAccessException();

        Prospect? current = await _repo.GetTheCurrentAsync(request.ProductId, request.CompanyId);
        
        var errors = new Dictionary<string, List<string>>();

        if (current is null) 
            throw new NotFoundEntityException("This prospection doesn't exist !");

        if (current.Agent.Id == request.NewAgentId)
        {
            errors.Add("AgentId", new List<string>() { "This agent is already assign to this prospection !" });
            throw new BaseException(errors);
        }
        current.Agent = supervisionHistory.Supervised;
        await _repo.UpdateAsync(current);

        return current.ToModel();
    }
}
