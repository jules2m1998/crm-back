using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.HeadProspectionModel;

public class HeadProspectionInModel
{
    public Guid? CommitId { get; set; }
    public Guid ProductId { get; set; }
    public Guid CompanyId { get; set; }
    public Guid AgentId { get; set; }
}
