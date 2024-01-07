using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class AsyncCommitRepository : BaseRepository<Commit>, IAsyncCommitRepository
{
    public AsyncCommitRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<Commit?> GetByIdAsync(Guid id)
    {
        return await Set
            .Include(x => x.Response)
            .Include(x => x.Parent)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
