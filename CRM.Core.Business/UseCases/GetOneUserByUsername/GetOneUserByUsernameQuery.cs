using CRM.Core.Business.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.GetOneUserByUsername
{
    public record GetOneUserByUsernameQuery(string UserName): IRequest<UserModel?>;
}
