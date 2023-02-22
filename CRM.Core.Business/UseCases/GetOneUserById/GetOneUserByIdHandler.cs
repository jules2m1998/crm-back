using CRM.Core.Business.Extensions;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.GetOneUserById;

public class GetOneUserByIdHandler : IRequestHandler<GetOneUserByIdQuery, UserModel>
{
    private readonly IUserRepository _repo;

    public GetOneUserByIdHandler(IUserRepository repo)
    {
        _repo= repo;
    }
    public async Task<UserModel> Handle(GetOneUserByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repo.GetUserAndRolesAsync(request.Id, request.UserName);
        if (result == null) return null;
        else return result.ToUserModel();
    }
}
