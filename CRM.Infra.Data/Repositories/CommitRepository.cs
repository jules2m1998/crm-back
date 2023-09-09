using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class CommitRepository : ICommitRepository
{
    private readonly ApplicationDbContext dbContext;
    private DbSet<Commit> Commits => dbContext.Commits;

    private IQueryable<Commit> _commits => Commits.Include(x => x.Response).Include(x => x.Parent);

    public CommitRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(Commit commit, CancellationToken cancellationToken)
    {
        await Commits.AddAsync(commit, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        Commits.Attach(commit);
    }

    public async Task<int> DeleteAsync(Commit commit, CancellationToken cancellationToken)
    {
        _ = await Commits.Where(x => x.ParentId == commit.Id).ExecuteUpdateAsync(s => s.SetProperty(e => e.ParentId, e => null), cancellationToken);
        Commits.Remove(commit);
        var count = await dbContext.SaveChangesAsync(cancellationToken);
        return count;
    }

    public async Task<ICollection<Commit>> GetAllAsync(CancellationToken cancellationToken) => 
        await _commits.ToListAsync(cancellationToken);

    public async Task<Commit?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _commits.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task UpdateAsync(Commit commit, CancellationToken cancellationToken)
    {
        Commits.Update(commit);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
