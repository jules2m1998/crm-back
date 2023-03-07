using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public class AssignSupervisorInModel
{
    public Guid Id { get; set; }
    public ICollection<Guid> UserIds { get; set; } = null!;
}
