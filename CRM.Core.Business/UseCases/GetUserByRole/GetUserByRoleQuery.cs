using CRM.Core.Business.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.GetUserByRole;

public class GetUserByRoleQuery: IRequest<ICollection<UserModel>>
{
    public string Role { get; set; }
    public string UserName { get; set; }

    public GetUserByRoleQuery(string role, string userName)
    {
        Role = role;
        UserName = userName;
    }
}
