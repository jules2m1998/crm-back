using CRM.Core.Business.Models;
using MediatR;

namespace CRM.Core.Business.UseCases.AddUser
{
    public class AddAdminUserCommand: UserBody, IRequest<UserModel> {}
}
