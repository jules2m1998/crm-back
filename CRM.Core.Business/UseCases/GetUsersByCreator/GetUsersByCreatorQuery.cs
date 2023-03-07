using CRM.Core.Business.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.GetUsersByCreator
{
    public class GetUsersByCreatorQuery: IRequest<ICollection<UserAndCreatorModel>>
    {
        public string CreatorUserName { get; set; } = null!;
    }
}
