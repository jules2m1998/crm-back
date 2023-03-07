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

namespace CRM.Core.Business.UseCases.SupervionUCs.AssignSupervisor;

public class AssignSupervisorHandler : IRequestHandler<AssignSupervisorCommand, ICollection<SupervisionOutModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISupervisionHistoryRepository _repo;

    public AssignSupervisorHandler(IUserRepository userRepository, ISupervisionHistoryRepository repo)
    {
        _userRepository = userRepository;
        _repo = repo;
    }

    public async Task<ICollection<SupervisionOutModel>> Handle(AssignSupervisorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);

        var supervisor = isAdmin ? await _userRepository.GetUserAndSupervisedAsync(request.Model.SupervisorId) : await _userRepository.GetUserAndSupervisedAsync(request.Model.SupervisorId, request.UserName);
        if (supervisor == null) throw new NotFoundEntityException("Supervisor not found !");

        ICollection<User> supervisees = 
            isAdmin 
            ? await _userRepository.GetManyCCLUserToSupervisionAsync(
                userIds: request.Model.SupervisedIds,
                supervisorId: request.Model.SupervisorId
                )
            : await _userRepository.GetManyCCLUserToSupervisionAsync(
                userIds: request.Model.SupervisedIds, 
                supervisorId: request.Model.SupervisorId, 
                request.UserName
                );
        if (supervisees.Count != request.Model.SupervisedIds.Count) throw new NotFoundEntityException("One or more supervised are not found !");

        ICollection<SupervisionHistory> supervisionHistories = supervisees.Select(s => new SupervisionHistory
        {
            Supervisor = supervisor,
            Supervised= s
        }).ToList();
        ICollection<SupervisionHistory> result = await _repo.AddRangeAsync(supervisionHistories);

        return result.Select(r => r.SupervisionHistoryToModel()).ToList();
    }
}
