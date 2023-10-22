using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.UpdateEvent;

public class UpdateEventCommandDto
{
    public Guid Id { get; set; }

    #region prospection ids
    public Guid? ProductId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? AgentId { get; set; }
    #endregion

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Topic { get; set; } = null!;

    public UpdateEventCommandOwnerDto Owner { get; set; } = null!;

    public ICollection<UpdateEventCommandContactDto> Contact { get; set; } = null!;
}
