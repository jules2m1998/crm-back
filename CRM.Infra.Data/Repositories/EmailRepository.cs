using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly ApplicationDbContext _context;

    private DbSet<Email> Emails => _context.Emails;

    public EmailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event e)
    {
        var mails = new List<Email>
        {
            new Email
            {
                EmailType = EmailType.FIRST,
                Event = e
            },
            new Email
            {
                EmailType = EmailType.SECOND,
                Event = e
            },
            new Email
            {
                EmailType = EmailType.LAST,
                Event = e
            }
        };

        await Emails.AddRangeAsync(mails);
        _ = await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Email>> GetCurrentsAsync()
    {

        var now = DateTime.Now;
        return await Emails
            .Include(e => e.Event).ThenInclude(e => e.Owner)
            .Include(e => e.Event).ThenInclude(e => e.Contact)
            .Where(e => !e.IsSend)
            .Where(
                e => 
                    (e.EmailType == EmailType.FIRST && e.Event.CreatedAt <= now)
                    || (e.EmailType == EmailType.SECOND && e.Event.StartDate.AddMinutes(-5) <= now)
                    || (e.EmailType == EmailType.LAST && e.Event.StartDate.AddSeconds(-30) <= now)
                    )
            .ToListAsync();
    }

    public async Task UpdateAsync(Email mail)
    {
        Emails.Attach(mail);
        Emails.Update(mail);
        _ = await _context.SaveChangesAsync();
    }
}
