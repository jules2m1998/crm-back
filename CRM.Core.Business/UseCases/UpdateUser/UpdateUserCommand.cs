using CRM.Core.Business.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.UpdateUser;

public class UpdateUserCommand: IRequest<UserModel?>
{
    public UserUpdateModel User { get; set; } = null!;
    public string UserName { get; set; } = string.Empty;
}
