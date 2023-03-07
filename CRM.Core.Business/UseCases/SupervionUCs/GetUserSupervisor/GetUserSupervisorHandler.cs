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

namespace CRM.Core.Business.UseCases.SupervionUCs.GetUserSupervisor;

public class GetUserSupervisorHandler : IRequestHandler<GetUserSupervisorCommand, SupervisionOutModel>
{
    private readonly IUserRepository _userRepository;
    private readonly ISupervisionHistoryRepository _repo;

    public GetUserSupervisorHandler(IUserRepository userRepository, ISupervisionHistoryRepository repo)
    {
        _userRepository = userRepository;
        _repo = repo;
    }

    public async Task<SupervisionOutModel> Handle(GetUserSupervisorCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);

        SupervisionHistory? supervision = 
            isAdmin 
            ? await _repo.GetUserSupervisor(request.UserId) 
            : await _repo.GetUserSupervisor(request.UserId, request.UserName);
        if(supervision is null) 
            throw new NotFoundEntityException("Supervised not found !");
        return supervision.SupervisionHistoryToModel();
    }
}
