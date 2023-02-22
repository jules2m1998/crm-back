using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ToogleAccountActiveted;

public class ToogleAccountActivatedHandler : IRequestHandler<ToogleAccountActivatedCommand, ICollection<UserModel>>
{
    private readonly IUserRepository _repo;

    public ToogleAccountActivatedHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ICollection<UserModel>> Handle(ToogleAccountActivatedCommand request, CancellationToken cancellationToken)
    {
        ICollection<User> users = await _repo.GetUsersByCreatorUsernameAndIdsAsync(ids: request.Ids, creatorUserName: request.UserName);
        users = await _repo.ToogleUsersActivationStatus(users);
        return users.Select(u => u.ToUserModel()).ToList();
    }
}
