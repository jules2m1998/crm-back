using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Models.Supervision;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.SupervionUCs.MySupervisees;

public class MySuperviseesHandler : IRequestHandler<MySuperviseesQuery, ICollection<SupervisionOutModel>>
{
    private readonly ISupervisionHistoryRepository _repository;

    public MySuperviseesHandler(ISupervisionHistoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<SupervisionOutModel>> Handle(MySuperviseesQuery request, CancellationToken cancellationToken) => 
        (await _repository.GetAllSupervisedUserBySupervisorAsync(supervisorUserName: request.UserName))
        .Select(s => s.SupervisionHistoryToModel())
        .ToList();
}
