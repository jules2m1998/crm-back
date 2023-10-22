using CRM.Core.Business.UseCases.EventsUcs.Commands.AddEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventById;

public class GetEventByIdQueryDto
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public AddEventCommandHeadProspectionDto? Prospect { get; set; } = null!;
}
