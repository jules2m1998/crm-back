using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Supervision;
public class SupervisionInModel
{
    public Guid SupervisorId { get; set; }
    public ICollection<Guid> SupervisedIds { get; set; } = new List<Guid>();
}
