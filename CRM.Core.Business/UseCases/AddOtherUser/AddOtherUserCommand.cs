using CRM.Core.Business.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.AddOtherUser
{
    public class AddOtherUserCommand: IRequest<UserModel>
    {
        public UserBodyAndRole User { get; set; } = null!;
        public List<string> Roles { get; set; } = null!;
    }
}
