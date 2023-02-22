using CRM.Core.Business.Models;
using MediatR;

namespace CRM.Core.Business.UseCases.ResetPassword;

public class ResetPasswordCommand: IRequest<UserModel>
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}
