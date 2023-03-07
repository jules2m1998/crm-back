using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.Repositories;
using CRM.Core.Business.UseCases.SupervionUCs.GetSupervisionHistory;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.GetSuperviseesHistory;

public class GetSuperviseesHistoryHandler : IRequestHandler<GetSuperviseesHistoryQuery, ICollection<SupervisionOutModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISupervisionHistoryRepository _repo;

    public GetSuperviseesHistoryHandler(IUserRepository userRepository, ISupervisionHistoryRepository repo)
    {
        _userRepository = userRepository;
        _repo = repo;
    }

    public async Task<ICollection<SupervisionOutModel>> Handle(GetSuperviseesHistoryQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();
        var isAdmin = _userRepository.IsAdminUser(user);

        ICollection<SupervisionHistory> history = 
            isAdmin 
            ? await _repo.GetSuperviseesHistoryAsync(request.UserId)
            : await _repo.GetSuperviseesHistoryAsync(request.UserId, request.UserName);

        return history.Select(h => h.SupervisionHistoryToModel()).ToList();
    }
}
