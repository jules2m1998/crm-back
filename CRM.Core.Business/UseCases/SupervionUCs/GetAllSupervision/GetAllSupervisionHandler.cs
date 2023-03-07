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

namespace CRM.Core.Business.UseCases.SupervionUCs.GetAllSupervision;

public class GetAllSupervisionHandler : IRequestHandler<GetAllSupervisionQuery, ICollection<SupervisionOutModel>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISupervisionHistoryRepository _repo;

    public GetAllSupervisionHandler(IUserRepository userRepository, ISupervisionHistoryRepository repo)
    {
        _userRepository = userRepository;
        _repo = repo;
    }

    public async Task<ICollection<SupervisionOutModel>> Handle(GetAllSupervisionQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAndRolesAsync(request.UserName);
        if (user == null) throw new UnauthorizedAccessException();

        ICollection<SupervisionHistory> result =  await _repo.GetAllActivateSupervisionAsync();

        return result.Select(r => r.SupervisionHistoryToModel()).ToList();
    }
}
