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

namespace CRM.Core.Business.UseCases.GetUserByRole;

public class GetUserByRoleHandler : IRequestHandler<GetUserByRoleQuery, ICollection<UserModel>>
{
    private readonly IUserRepository _repo;

    public GetUserByRoleHandler(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<ICollection<UserModel>> Handle(GetUserByRoleQuery request, CancellationToken cancellationToken)
    {
        var current = await _repo.GetUserAndRolesAsync(request.UserName);
        if (current == null) throw new UnauthorizedAccessException();

        var isAdmin = _repo.IsAdminUser(current);

        ICollection<User> users = isAdmin ? await _repo.GetUserByRoleAsync(request.Role) : await _repo.GetUserByRoleAsync(request.Role, request.UserName);

        return users.Select(u => u.ToUserModel()).ToList();
    }
}
