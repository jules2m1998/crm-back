using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace CRM.Infra.Data.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly IApplicationDbContext _context;
    private readonly DbSet<Contact> _contacts;

    private IQueryable<Contact> Included { get
        {
            return _contacts
            .Include(c => c.Creator)
            .Include(c => c.Company)
            .Include(c => c.Phones)
            .Include(c => c.SharedTo);
        } }

    public ContactRepository(IApplicationDbContext context)
    {
        _context = context;
        _contacts = context.Contacts;
    }

    public async Task AddAsync(Contact contact, CancellationToken cancellationToken)
    {
        _contacts.Add(contact);
        await _context.SaveChangesAsync(cancellationToken);
        _contacts.Attach(contact);
    }

    public async Task<Contact?> GetAsync(string name, Guid companyId, CancellationToken cancellationToken) =>
        await _contacts
        .Where(c => c.Name == name && c.CompanyId == companyId)
        .FirstOrDefaultAsync(cancellationToken);

    public async Task<ICollection<Contact>> GetAsync()
        => await Included
        .AsNoTracking()
        .ToListAsync();

    public async Task<ICollection<Contact>> GetMineAsync(Guid userId)
    {
        return await Included
            .AsNoTracking()
            .Where(c => 
            (c.Creator != null && c.Creator.Id == userId)
            || c.SharedTo.FirstOrDefault(s => s.Id == userId) != null 
            || c.Visibility == Core.Domain.Types.ContactVisibility.Public
            ).ToListAsync();
    }

    public async Task<Contact?> GetOneAsync(Guid id, CancellationToken cancellationToken)
        => await Included.FirstOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);

    public async Task<Contact?> GetOneAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        => await Included
        .FirstOrDefaultAsync(c => c.Id == id || (c.Creator != null && c.Creator.Id == userId) || c.SharedTo.FirstOrDefault(u => u.Id == userId) != null, cancellationToken);

    public async Task RemoveAsync(Contact contact)
    {
        _contacts.Remove(contact);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Contact contact)
    {
        _contacts.Update(contact);
        await _context.SaveChangesAsync();
        _contacts.Attach(contact);
    }
}
