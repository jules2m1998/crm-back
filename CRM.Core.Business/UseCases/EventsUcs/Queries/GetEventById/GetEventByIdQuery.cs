using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.EventsUcs.Queries.GetEventById;

public class GetEventByIdQuery : IRequest<GetEventByIdQueryResponse>
{
    public Guid EventId { get; set; }
}
