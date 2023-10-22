using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Commands.DeleteEvent;

public class DeleteEventCommand : IRequest
{
    public Guid EventId { get; set; }
}
