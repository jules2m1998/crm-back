using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class EventRepository : BaseRepository<Event>, IEventRepository
{
    public EventRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Event?> GetEventWithHeadAndContactByIdAsync(Guid id)
    {
        return await Set
            .Include(x => x.Prospect)
            .Include(x => x.Contact)
            .Include(x => x.Owner)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ICollection<Event>> GetEventsByOwnerAsync(Guid? ownerId, CancellationToken cancellationToken)
    {
        return await Set
            .Include(x => x.Prospect)
            .Include(x => x.Contact)
            .Include(x => x.Owner)
            .Where(x => x.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }
}
