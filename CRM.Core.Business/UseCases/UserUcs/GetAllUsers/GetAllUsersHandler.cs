using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.UserUcs.GetAllUsers;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, ICollection<UserModel>>
{
    private readonly IUserRepository _repo;

    public GetAllUsersHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ICollection<UserModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repo.GetAllAsync();
        return users
            .Select(u => u.ToUserModel())
            .ToList();
    }
}
