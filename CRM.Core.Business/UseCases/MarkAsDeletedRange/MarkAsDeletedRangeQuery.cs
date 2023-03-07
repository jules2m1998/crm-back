using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.MarkAsDeletedRange
{
    public class MarkAsDeletedRangeQuery: IRequest<bool>
    {
        public List<Guid> Ids { get; set; } = new List<Guid>();
        public string UserName { get; set; } = string.Empty;
    }
}
