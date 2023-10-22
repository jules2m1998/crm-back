using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Repositories;

public interface IEventRepository : IAsyncRepository<Event>
{
    Task<ICollection<Event>> GetEventsByOwnerAsync(Guid? ownerId, CancellationToken cancellationToken);
    public Task<Event?> GetEventWithHeadAndContactByIdAsync(Guid id);
}
