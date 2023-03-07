using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.GetSupervisionHistory;

public class GetSupervisionHistoryHandler : IRequestHandler<GetSupervisionHistoryQuery, ICollection<SupervisionOutModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISupervisionHistoryRepository _repo;

    public GetSupervisionHistoryHandler(IUserRepository userRepository, ISupervisionHistoryRepository repo)
    {
        _userRepository = userRepository;
        _repo = repo;
    }

    public async Task<ICollection<SupervisionOutModel>> Handle(GetSupervisionHistoryQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);

        ICollection<SupervisionHistory> history = 
            isAdmin 
            ? await _repo.GetSupervisionHistory(request.UserId) 
            : await _repo.GetSupervisionHistory(request.UserId, request.UserName);

        return history.Select(h => h.SupervisionHistoryToModel()).ToList();
    }
}
