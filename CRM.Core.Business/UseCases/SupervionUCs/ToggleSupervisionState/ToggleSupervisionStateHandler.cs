using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.ToggleSupervisionState;

internal class ToggleSupervisionStateHandler : IRequestHandler<ToggleSupervisionStateCommand, SupervisionOutModel?>
{
    private readonly ISupervisionHistoryRepository _repo;
    private readonly IUserRepository _userRepo;

    public ToggleSupervisionStateHandler(ISupervisionHistoryRepository repo, IUserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<SupervisionOutModel?> Handle(ToggleSupervisionStateCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepo.IsAdminUser(user);

        SupervisionHistory? current = isAdmin 
            ? await _repo.GetSupervisionAsync(supervisorId: request.SupervisorId, supervisedId: request.SupervisedId) 
            : await _repo.GetSupervisionAsync(supervisorId: request.SupervisorId, supervisedId: request.SupervisedId, creatorName: request.UserName);
        if (current == null) throw new NotFoundEntityException(null);

        current.IsActive = !current.IsActive;

        SupervisionHistory currentUpdate = await _repo.UpdateAsync(history: current);

        return currentUpdate.SupervisionHistoryToModel();
    }
}
