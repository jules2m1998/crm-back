using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.Repositories;
using MediatR;

namespace CRM.Core.Business.UseCases.SupervionUCs.MySupervisor;

public class MySupervisorHandler : IRequestHandler<MySupervisorQuery, SupervisionOutModel?>
{
    private readonly ISupervisionHistoryRepository _repo;

    public MySupervisorHandler(ISupervisionHistoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<SupervisionOutModel?> Handle(MySupervisorQuery request, CancellationToken cancellationToken) => 
        (await _repo.GetUserSupervisor(userName: request.SupervisedUserName))?
        .SupervisionHistoryToModel();
}
