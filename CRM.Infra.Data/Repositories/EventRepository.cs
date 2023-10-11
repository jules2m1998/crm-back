﻿using CRM.Core.Business.Repositories;
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
        .Include(e => e.Prospect).ThenInclude(p => p.Product)
        .Include(e => e.Prospect).ThenInclude(p => p.Company).ThenInclude(c => c.Creator)
        .Include(e => e.Prospect).ThenInclude(p => p.Agent)
        .Include(e => e.Prospect).ThenInclude(p => p.Creator)
        .Include(e => e.Contact).ThenInclude(c => c.Company)
        .Include(e => e.Creator)
        .Include(e => e.Owner);

    private IQueryable<Event> Simple =>
        _events
        .Include(e => e.Creator)
        .Include(e => e.Prospect).ThenInclude(p => p.Product)
        .Include(e => e.Prospect).ThenInclude(p => p.Company)
        .Include(e => e.Prospect).ThenInclude(p => p.Agent)
        .Include(e => e.Prospect).ThenInclude(p => p.Creator)
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
        _events.Attach(e);
    }

    public async Task<ICollection<Event>> GetAsync() =>
        await Simple.ToListAsync();

    public async Task<ICollection<Event>> GetByUserAsync(string userName) => 
        await Simple
        .Where(e => e.Owner.UserName == userName || (e.Creator != null && e.Creator.UserName == userName))
        .ToListAsync();

    public async Task<Event?> GetAsync(Guid id, string userName) =>
        await Included
        .FirstOrDefaultAsync(e => (e.Owner.UserName == userName || (e.Creator != null && e.Creator.UserName == userName)) && e.Id == id);

    public async Task<Event?> GetAsync(Guid id) => 
        await Included.FirstOrDefaultAsync(e => e.Id == id);

    public async Task UpdateAsync(Event e)
    {
        _events.Update(e);
        await _context.SaveChangesAsync();

    }

    public async Task DeleteAsync(Event e)
    {
        _events.Remove(e);
        await _context.SaveChangesAsync();
    }
}