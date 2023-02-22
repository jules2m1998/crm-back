using CRM.Core.Business.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.GetOneUserById;

public class GetOneUserByIdQuery: IRequest<UserModel>
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
}
