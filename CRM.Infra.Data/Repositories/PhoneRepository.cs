using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class PhoneRepository : IPhoneRepository
{
    public IApplicationDbContext _dbContext;

    public PhoneRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbSet<PhoneNumber> Phones { get
        {
            return _dbContext.PhoneNumbers;
        } }

    public async Task<ICollection<PhoneNumber>> GetManyAsync(ICollection<string> values)
        => await Phones.Where(c => values.Contains(c.Value)).ToListAsync();
}
