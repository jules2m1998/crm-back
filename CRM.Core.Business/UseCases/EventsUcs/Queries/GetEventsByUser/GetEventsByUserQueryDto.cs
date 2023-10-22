using CRM.Core.Business.UseCases.EventsUcs.Commands.UpdateEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventsByUser;

public class GetEventsByUserQueryDto
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string Topic { get; set; } = null!;

    public UpdateEventCommandOwnerDto Owner { get; set; } = null!;

    public ICollection<UpdateEventCommandContactDto> Contact { get; set; } = null!;
}
