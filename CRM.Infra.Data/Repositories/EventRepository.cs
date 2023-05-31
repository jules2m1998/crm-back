using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace CRM.Infra.Data.Repositories;

public class EventRepository : IEventRepository
{
    private readonly IApplicationDbContext _context;

    private DbSet<Event> _events => _context.Events;
    private IQueryable<Event> Included => 
        _events
        .Include(e => e.Creator)
        .Include(e => e.Prospect)
        .Include(e => e.Contact)
        .Include(e => e.Creator)
        .Include(e => e.Owner);

    public EventRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event e)
    {
        _events.Add(e);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Event>> GetAsync() =>
        await Included.ToListAsync();

    public async Task<ICollection<Event>> GetByUserAsync(string userName) => 
        await Included
        .Where(e => e.Owner.UserName == userName || (e.Creator != null && e.Creator.UserName == userName))
        .ToListAsync();

    public async Task<Event?> GetAsync(Guid id, string userName) =>
        await Included
        .FirstOrDefaultAsync(e => (e.Owner.UserName == userName || (e.Creator != null && e.Creator.UserName == userName)) && e.Id == id);

    public Task<Event?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
