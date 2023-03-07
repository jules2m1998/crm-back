using CRM.Core.Business.Models;
using MediatR;

namespace CRM.Core.Business.UseCases.AddOtherUser
{
    public class AddOtherUserCommand: IRequest<UserModel>
    {
        public UserBodyAndRole User { get; set; } = null!;
        public string CurrentUserName { get; set; } = string.Empty;
    }
}
