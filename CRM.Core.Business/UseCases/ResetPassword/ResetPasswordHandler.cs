using CRM.Core.Business.Extensions;
using CRM.Core.Business.Helpers;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CsvHelper.TypeConversion;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, UserModel?>
{
    private readonly IUserRepository _repo;
    public ResetPasswordHandler(IUserRepository repository)
    {
        _repo = repository;
    }
    public async Task<UserModel?> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var isActive = await _repo.IsActivatedUserAsync(request.UserName);
        if (!isActive) throw new UnauthorizedAccessException();
        var user = await _repo.GetUserAndRolesAsync(request.Id, request.UserName);
        if (user is null) return null;
        await _repo.ResetUserPassword(user);
        return user.ToUserModel();
    }
}
